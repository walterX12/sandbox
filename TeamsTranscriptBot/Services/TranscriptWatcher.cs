using System.Text;
using Microsoft.Extensions.Options;
using TeamsTranscriptBot.Models;

namespace TeamsTranscriptBot.Services;

/// <summary>
/// Hlavní background service (IHostedService).
/// Každých N sekund:
///   1. Zjistí aktuálně probíhající online meetingy organizátora.
///   2. Pro každý meeting zkontroluje nové transkriptové chunky.
///   3. Nové chunky parsuje a přidá do výstupního Markdown souboru.
///   4. Po skončení meetingu (žádné nové chunky 2× po sobě) vygeneruje AI shrnutí.
/// </summary>
public sealed class TranscriptWatcher : BackgroundService
{
    private readonly GraphService _graph;
    private readonly SummaryService _summary;
    private readonly BotSettings _cfg;
    private readonly bool _aiEnabled;
    private readonly ILogger<TranscriptWatcher> _logger;

    // Stav sledovaných meetingů (in-memory)
    private readonly Dictionary<string, TrackedMeeting> _tracked = new();

    public TranscriptWatcher(
        GraphService graph,
        SummaryService summary,
        IOptions<AppSettings> options,
        ILogger<TranscriptWatcher> logger)
    {
        _graph     = graph;
        _summary   = summary;
        _cfg       = options.Value.Bot;
        _aiEnabled = options.Value.Bot.EnableAiSummary;
        _logger    = logger;
    }

    // -------------------------------------------------------------------------

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TeamsTranscriptBot spuštěn. Interval: {Interval}s", _cfg.PollingIntervalSeconds);
        EnsureOutputDirectory();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await TickAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Neočekávaná chyba v hlavní smyčce");
            }

            await Task.Delay(TimeSpan.FromSeconds(_cfg.PollingIntervalSeconds), stoppingToken);
        }

        _logger.LogInformation("TeamsTranscriptBot zastaven.");
    }

    // -------------------------------------------------------------------------

    private async Task TickAsync(CancellationToken ct)
    {
        // Hledáme meetingy, které začaly v posledních 24 hodinách
        // a skončí nejdříve teď (jsou stále "aktivní" nebo nedávno skončily)
        var now  = DateTimeOffset.UtcNow;
        var from = now.AddHours(-24);
        var to   = now.AddHours(1);   // mírně do budoucna pro jistotu

        var meetings = await _graph.GetOnlineMeetingsAsync(_cfg.OrganizerUserId, from, to, ct);

        foreach (var meeting in meetings)
        {
            if (meeting.Id is null) continue;

            // Filtrování podle klíčových slov (pokud je nastaveno)
            if (_cfg.SubjectKeywords.Count > 0)
            {
                var subject = meeting.Subject ?? "";
                var matches = _cfg.SubjectKeywords.Any(kw =>
                    subject.Contains(kw, StringComparison.OrdinalIgnoreCase));

                if (!matches)
                {
                    _logger.LogDebug("Meeting '{Subject}' přeskočen (neodpovídá klíčovým slovům).", subject);
                    continue;
                }
            }

            await ProcessMeetingAsync(meeting.Id, meeting.Subject ?? "Bez názvu", ct);
        }
    }

    // -------------------------------------------------------------------------

    private async Task ProcessMeetingAsync(string meetingId, string subject, CancellationToken ct)
    {
        // Vytvoříme záznamu meetingu pokud ho ještě nesledujeme
        if (!_tracked.TryGetValue(meetingId, out var tracked))
        {
            var fileName = SanitizeFileName($"{DateTime.Now:yyyyMMdd_HHmm}_{subject}.md");
            tracked = new TrackedMeeting
            {
                MeetingId        = meetingId,
                Subject          = subject,
                OrganizerUserId  = _cfg.OrganizerUserId,
                StartTime        = DateTimeOffset.UtcNow,
                OutputFilePath   = Path.Combine(_cfg.OutputDirectory, fileName),
            };
            _tracked[meetingId] = tracked;

            WriteHeader(tracked);
            _logger.LogInformation("Začínám sledovat meeting: '{Subject}' → {File}", subject, tracked.OutputFilePath);
        }

        var transcripts = await _graph.GetTranscriptsAsync(_cfg.OrganizerUserId, meetingId, ct);
        var newTranscripts = transcripts
            .Where(t => t.Id is not null && !tracked.ProcessedTranscriptIds.Contains(t.Id!))
            .ToList();

        if (newTranscripts.Count == 0)
        {
            _logger.LogDebug("Meeting '{Subject}': žádné nové transkripty.", subject);

            // Pokud uplynuly více než 2 hodiny od posledního chunku a ještě není shrnutí,
            // považujeme meeting za ukončený a vygenerujeme shrnutí.
            var elapsed = DateTimeOffset.UtcNow - tracked.StartTime;
            if (elapsed > TimeSpan.FromHours(2) && _aiEnabled && !tracked.SummaryGenerated)
            {
                await GenerateAndAppendSummaryAsync(tracked, ct);
            }
            return;
        }

        foreach (var transcript in newTranscripts)
        {
            _logger.LogInformation("Meeting '{Subject}': stahuju transkript {Id}.", subject, transcript.Id);

            var vttContent = await _graph.GetTranscriptContentAsync(
                _cfg.OrganizerUserId, meetingId, transcript.Id!, ct);

            if (vttContent is null)
                continue;

            var entries = VttParser.Parse(vttContent);
            var markdown = VttParser.ToMarkdown(entries);

            AppendTranscriptChunk(tracked, markdown, transcript.Id!);
            tracked.ProcessedTranscriptIds.Add(transcript.Id!);

            _logger.LogInformation(
                "Meeting '{Subject}': přidáno {Count} řádků transkriptu.", subject, entries.Count);
        }
    }

    // -------------------------------------------------------------------------
    // Zápis do souboru
    // -------------------------------------------------------------------------

    private void WriteHeader(TrackedMeeting meeting)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"# Zápis ze schůzky: {meeting.Subject}");
        sb.AppendLine($"**Datum:** {meeting.StartTime.ToLocalTime():dd.MM.yyyy HH:mm}  ");
        sb.AppendLine($"**Meeting ID:** `{meeting.MeetingId}`  ");
        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine("## Průběh schůzky (transkript)");
        sb.AppendLine();

        File.WriteAllText(meeting.OutputFilePath, sb.ToString(), Encoding.UTF8);
    }

    private void AppendTranscriptChunk(TrackedMeeting meeting, string markdownChunk, string transcriptId)
    {
        var separator = $"\n<!-- chunk: {transcriptId} @ {DateTime.UtcNow:HH:mm:ss} UTC -->\n";
        File.AppendAllText(meeting.OutputFilePath, separator + markdownChunk, Encoding.UTF8);
    }

    private async Task GenerateAndAppendSummaryAsync(TrackedMeeting meeting, CancellationToken ct)
    {
        _logger.LogInformation("Generuji AI shrnutí pro '{Subject}'...", meeting.Subject);

        var fullTranscript = await File.ReadAllTextAsync(meeting.OutputFilePath, ct);
        var aiSummary      = await _summary.SummarizeAsync(meeting.Subject, fullTranscript, ct);

        var block = new StringBuilder();
        block.AppendLine();
        block.AppendLine("---");
        block.AppendLine();
        block.AppendLine("## AI Shrnutí (Claude)");
        block.AppendLine();
        block.AppendLine(aiSummary);

        await File.AppendAllTextAsync(meeting.OutputFilePath, block.ToString(), Encoding.UTF8, ct);

        meeting.SummaryGenerated = true;
        _logger.LogInformation("AI shrnutí zapsáno do {File}", meeting.OutputFilePath);
    }

    // -------------------------------------------------------------------------

    private void EnsureOutputDirectory()
    {
        if (!Directory.Exists(_cfg.OutputDirectory))
        {
            Directory.CreateDirectory(_cfg.OutputDirectory);
            _logger.LogInformation("Vytvořen výstupní adresář: {Dir}", _cfg.OutputDirectory);
        }
    }

    private static string SanitizeFileName(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        return string.Concat(name.Select(c => invalid.Contains(c) ? '_' : c));
    }
}
