namespace TeamsTranscriptBot.Models;

public class AppSettings
{
    public AzureAdSettings AzureAd { get; set; } = new();
    public BotSettings Bot { get; set; } = new();
    public AnthropicSettings Anthropic { get; set; } = new();
}

public class AzureAdSettings
{
    public string TenantId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}

public class BotSettings
{
    /// <summary>
    /// User ID (GUID) organizátora meetingů – z Azure AD
    /// </summary>
    public string OrganizerUserId { get; set; } = string.Empty;

    /// <summary>
    /// Jak často (v sekundách) kontrolovat nové transkriptové chunky
    /// </summary>
    public int PollingIntervalSeconds { get; set; } = 30;

    /// <summary>
    /// Adresář, kam se ukládají zápisy (relativní nebo absolutní cesta)
    /// </summary>
    public string OutputDirectory { get; set; } = "output";

    /// <summary>
    /// Pokud true, na konci meetingu se vygeneruje AI shrnutí přes Claude
    /// </summary>
    public bool EnableAiSummary { get; set; } = true;

    /// <summary>
    /// Klíčová slova pro filtrování – sleduj jen meetingy jejichž subject je obsahuje.
    /// Prázdný seznam = sleduj všechny.
    /// </summary>
    public List<string> SubjectKeywords { get; set; } = new();
}

public class AnthropicSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = "claude-opus-4-6";
}
