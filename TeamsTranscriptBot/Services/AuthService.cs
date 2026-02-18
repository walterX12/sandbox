using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using TeamsTranscriptBot.Models;

namespace TeamsTranscriptBot.Services;

/// <summary>
/// Obstarává získání přístupového tokenu přes MSAL (client credentials flow).
/// Token je automaticky cachován a obnoven před vypršením.
/// </summary>
public class AuthService
{
    private readonly IConfidentialClientApplication _msalApp;
    private readonly string[] _scopes = ["https://graph.microsoft.com/.default"];

    public AuthService(IOptions<AppSettings> options)
    {
        var cfg = options.Value.AzureAd;

        _msalApp = ConfidentialClientApplicationBuilder
            .Create(cfg.ClientId)
            .WithClientSecret(cfg.ClientSecret)
            .WithAuthority($"https://login.microsoftonline.com/{cfg.TenantId}")
            .Build();
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken ct = default)
    {
        var result = await _msalApp
            .AcquireTokenForClient(_scopes)
            .ExecuteAsync(ct);

        return result.AccessToken;
    }
}
