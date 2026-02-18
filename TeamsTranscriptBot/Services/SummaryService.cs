using Anthropic.SDK;
using Anthropic.SDK.Messaging;
using Microsoft.Extensions.Options;
using TeamsTranscriptBot.Models;

namespace TeamsTranscriptBot.Services;

/// <summary>
/// Generuje AI shrnutí transkriptu pomocí Anthropic Claude API.
/// </summary>
public class SummaryService
{
    private readonly AnthropicClient _client;
    private readonly string _model;
    private readonly ILogger<SummaryService> _logger;

    public SummaryService(IOptions<AppSettings> options, ILogger<SummaryService> logger)
    {
        var cfg = options.Value.Anthropic;
        _client = new AnthropicClient(cfg.ApiKey);
        _model  = cfg.Model;
        _logger = logger;
    }

    /// <summary>
    /// Pošle celý transkript Claude a vrátí strukturovaný zápis.
    /// </summary>
    public async Task<string> SummarizeAsync(
        string meetingSubject,
        string transcriptMarkdown,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Generuji AI shrnutí pro schůzku: {Subject}", meetingSubject);

        var prompt = $"""
            Jsi asistent pro tvorbu zápisů z porad.
            Níže je přepis (transkript) ze schůzky s názvem: "{meetingSubject}".

            {transcriptMarkdown}

            ---

            Ze záznamu vytvoř přehledný strukturovaný zápis v češtině. Použij tento formát Markdown:

            ## Shrnutí
            (2–4 věty o čem schůzka byla)

            ## Účastníci
            - seznam mluvčích z transkriptu

            ## Klíčová rozhodnutí
            - bod 1
            - bod 2

            ## Úkoly a akční body
            | Úkol | Zodpovědná osoba | Termín |
            |------|-----------------|--------|
            | ...  | ...             | ...    |

            ## Poznámky a důležité informace
            - další relevantní body

            Pokud některá sekce neobsahuje relevantní informace, napiš "– žádné –".
            """;

        try
        {
            var messages = new List<Message>
            {
                new() { Role = RoleType.User, Content = prompt }
            };

            var request = new MessageParameters
            {
                Model    = _model,
                MaxTokens = 2048,
                Messages = messages,
            };

            var response = await _client.Messages.GetClaudeMessageAsync(request, ct);
            return response.Content[0].ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Chyba při generování AI shrnutí");
            return "_(AI shrnutí se nepodařilo vygenerovat)_";
        }
    }
}
