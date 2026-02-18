using Azure.Core;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions.Authentication;
using TeamsTranscriptBot.Models;

namespace TeamsTranscriptBot.Services;

/// <summary>
/// Wrapper nad Microsoft Graph SDK pro operace s online meetingy a transkripty.
/// </summary>
public class GraphService
{
    private readonly AuthService _authService;
    private readonly ILogger<GraphService> _logger;

    public GraphService(AuthService authService, ILogger<GraphService> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    // -------------------------------------------------------------------------
    // Graph klient (vždy s čerstvým tokenem)
    // -------------------------------------------------------------------------

    private async Task<GraphServiceClient> CreateClientAsync(CancellationToken ct)
    {
        var token = await _authService.GetAccessTokenAsync(ct);
        var authProvider = new StaticTokenProvider(token);
        return new GraphServiceClient(authProvider);
    }

    // -------------------------------------------------------------------------
    // Meetingy
    // -------------------------------------------------------------------------

    /// <summary>
    /// Vrátí online meetingy daného uživatele, jejichž start je v rozsahu
    /// [od, do]. Využívá /users/{userId}/onlineMeetings endpoint.
    /// </summary>
    public async Task<List<OnlineMeeting>> GetOnlineMeetingsAsync(
        string userId,
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken ct = default)
    {
        var client = await CreateClientAsync(ct);

        try
        {
            // Graph API podporuje filtrování přes $filter na datech startu
            var response = await client.Users[userId].OnlineMeetings
                .GetAsync(config =>
                {
                    config.QueryParameters.Filter =
                        $"startDateTime ge {from:O} and startDateTime le {to:O}";
                    config.QueryParameters.Select =
                        ["id", "subject", "startDateTime", "endDateTime", "joinWebUrl"];
                }, ct);

            return response?.Value ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Chyba při načítání online meetingů pro uživatele {UserId}", userId);
            return [];
        }
    }

    // -------------------------------------------------------------------------
    // Transkript – seznam
    // -------------------------------------------------------------------------

    /// <summary>
    /// Vrátí seznam transkriptů pro daný meeting.
    /// Transkripce musí být zapnuta organizátorem nebo Teams adminem.
    /// </summary>
    public async Task<List<CallTranscript>> GetTranscriptsAsync(
        string userId,
        string meetingId,
        CancellationToken ct = default)
    {
        var client = await CreateClientAsync(ct);

        try
        {
            var response = await client.Users[userId]
                .OnlineMeetings[meetingId]
                .Transcripts
                .GetAsync(cancellationToken: ct);

            return response?.Value ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Nelze načíst transkript pro meeting {MeetingId} – transkripce ještě nemusí být k dispozici.",
                meetingId);
            return [];
        }
    }

    // -------------------------------------------------------------------------
    // Transkript – obsah (WebVTT)
    // -------------------------------------------------------------------------

    /// <summary>
    /// Stáhne obsah transkriptu ve formátu WebVTT (text/vtt).
    /// </summary>
    public async Task<string?> GetTranscriptContentAsync(
        string userId,
        string meetingId,
        string transcriptId,
        CancellationToken ct = default)
    {
        var client = await CreateClientAsync(ct);

        try
        {
            // Graph SDK vrací stream; přečteme ho jako string
            var stream = await client.Users[userId]
                .OnlineMeetings[meetingId]
                .Transcripts[transcriptId]
                .Content
                .GetAsync(config =>
                {
                    config.Headers.Add("Accept", "text/vtt");
                }, ct);

            if (stream is null) return null;

            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Chyba při stahování obsahu transkriptu {TranscriptId}", transcriptId);
            return null;
        }
    }
}

// -------------------------------------------------------------------------
// Pomocný token provider pro Graph SDK
// -------------------------------------------------------------------------

file sealed class StaticTokenProvider : IAuthenticationProvider
{
    private readonly string _token;

    public StaticTokenProvider(string token) => _token = token;

    public Task AuthenticateRequestAsync(
        RequestInformation request,
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        request.Headers.Add("Authorization", $"Bearer {_token}");
        return Task.CompletedTask;
    }
}
