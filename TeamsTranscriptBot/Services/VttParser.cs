using System.Text.RegularExpressions;
using TeamsTranscriptBot.Models;

namespace TeamsTranscriptBot.Services;

/// <summary>
/// Parsuje WebVTT formát který vrací Microsoft Graph transcript API.
///
/// Příklad VTT bloku:
/// 00:00:05.000 --> 00:00:10.000
/// &lt;v Jan Novák&gt;Dobrý den, zahajujeme schůzku.&lt;/v&gt;
/// </summary>
public static class VttParser
{
    // Regex pro timestamp řádek: 00:00:05.000 --> 00:00:10.450
    private static readonly Regex TimestampRegex = new(
        @"^(\d{2}:\d{2}:\d{2}[\.,]\d{3})\s*-->\s*(\d{2}:\d{2}:\d{2}[\.,]\d{3})",
        RegexOptions.Compiled);

    // Regex pro hlasový tag: <v Jméno Příjmení>text</v>
    private static readonly Regex VoiceTagRegex = new(
        @"<v\s+([^>]+)>(.*?)</v>",
        RegexOptions.Compiled | RegexOptions.Singleline);

    public static List<TranscriptEntry> Parse(string vttContent)
    {
        var entries = new List<TranscriptEntry>();

        // Rozdělíme na bloky (prázdný řádek jako oddělovač)
        var blocks = vttContent.Split(
            ["\r\n\r\n", "\n\n"],
            StringSplitOptions.RemoveEmptyEntries);

        foreach (var block in blocks)
        {
            var lines = block.Split(
                ["\r\n", "\n"],
                StringSplitOptions.RemoveEmptyEntries);

            TimeSpan? start = null;
            TimeSpan? end = null;
            var textLines = new List<string>();

            foreach (var line in lines)
            {
                var trimmed = line.Trim();

                // Přeskočíme hlavičku WEBVTT a čísla bloků
                if (trimmed == "WEBVTT" || int.TryParse(trimmed, out _))
                    continue;

                var tsMatch = TimestampRegex.Match(trimmed);
                if (tsMatch.Success)
                {
                    start = ParseTimestamp(tsMatch.Groups[1].Value);
                    end   = ParseTimestamp(tsMatch.Groups[2].Value);
                    continue;
                }

                if (start.HasValue)
                    textLines.Add(trimmed);
            }

            if (start is null || textLines.Count == 0)
                continue;

            // Spojíme textové řádky a extrahujeme mluvčího z <v> tagu
            var fullText = string.Join(" ", textLines);
            var voiceMatch = VoiceTagRegex.Match(fullText);

            if (voiceMatch.Success)
            {
                entries.Add(new TranscriptEntry
                {
                    Speaker   = voiceMatch.Groups[1].Value.Trim(),
                    Text      = CleanText(voiceMatch.Groups[2].Value),
                    StartTime = start.Value,
                    EndTime   = end ?? start.Value,
                });
            }
            else
            {
                // Blok bez voice tagu – přiřadíme jako "Neznámý"
                entries.Add(new TranscriptEntry
                {
                    Speaker   = "Neznámý",
                    Text      = CleanText(fullText),
                    StartTime = start.Value,
                    EndTime   = end ?? start.Value,
                });
            }
        }

        return entries;
    }

    // -------------------------------------------------------------------------

    /// <summary>
    /// Převede seznam entries na čitelný Markdown zápis.
    /// Sousední vstupy od stejného mluvčího se sloučí.
    /// </summary>
    public static string ToMarkdown(IEnumerable<TranscriptEntry> entries)
    {
        var sb = new System.Text.StringBuilder();
        string? lastSpeaker = null;

        foreach (var e in entries)
        {
            if (e.Speaker != lastSpeaker)
            {
                if (lastSpeaker is not null)
                    sb.AppendLine();

                sb.AppendLine($"**{e.Speaker}** `[{e.StartTime:hh\\:mm\\:ss}]`");
                lastSpeaker = e.Speaker;
            }

            sb.AppendLine($"> {e.Text}");
        }

        return sb.ToString();
    }

    // -------------------------------------------------------------------------

    private static TimeSpan ParseTimestamp(string ts)
    {
        // Normalizujeme tečku/čárku jako oddělovač milisekund
        ts = ts.Replace(',', '.');
        return TimeSpan.Parse(ts);
    }

    private static string CleanText(string text)
    {
        // Odstraníme případné HTML tagy a nadbytečné mezery
        var noTags = Regex.Replace(text, "<[^>]+>", " ");
        return Regex.Replace(noTags.Trim(), @"\s+", " ");
    }
}
