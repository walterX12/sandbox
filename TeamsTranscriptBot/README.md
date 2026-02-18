# Teams Transcript Bot

.NET 8 Worker Service který průběžně sleduje Microsoft Teams meetingy, stahuje transkript a ukládá ho jako Markdown zápis. Volitelně na konci schůzky vygeneruje AI shrnutí pomocí Claude API.

## Jak to funguje

```
Teams schůzka (transkripce zapnutá)
         ↓  každých 30s
Microsoft Graph API  (users/{id}/onlineMeetings/{id}/transcripts)
         ↓
VttParser  →  Markdown chunk
         ↓
output/20250101_1400_Standup.md   ←  průběžně doplňováno
         ↓  po skončení meetingu
Claude API  →  shrnutí + úkoly + rozhodnutí
```

## Požadavky

| Nástroj | Verze |
|---------|-------|
| .NET SDK | 8.0+ |
| Azure AD tenant | libovolný (firemní / vývojářský) |
| Teams transkripce | musí být povolena IT adminem |
| Anthropic API klíč | jen pokud `EnableAiSummary: true` |

## Krok 1 – Azure App Registration

1. Otevři [portal.azure.com](https://portal.azure.com)
2. **Azure Active Directory** → **App registrations** → **New registration**
   - Name: `TeamsTranscriptBot`
   - Supported account types: `Accounts in this organizational directory only`
3. Po vytvoření si poznamenej **Application (client) ID** a **Directory (tenant) ID**
4. **Certificates & secrets** → **New client secret** – zkopíruj hodnotu
5. **API permissions** → **Add a permission** → **Microsoft Graph** → **Application permissions**:

   | Permission | Proč |
   |-----------|------|
   | `OnlineMeetings.Read.All` | Čtení online meetingů |
   | `OnlineMeetingTranscript.Read.All` | Čtení transkriptů |
   | `Calendars.Read` | Hledání meetingů v kalendáři |
   | `User.Read.All` | Načtení User ID organizátora |

6. Klikni **Grant admin consent for [firma]** – bez toho Application permissions nefungují

## Krok 2 – Zjisti User ID organizátora

Spusť v [Graph Exploreru](https://developer.microsoft.com/graph/graph-explorer) jako admin:

```
GET https://graph.microsoft.com/v1.0/users?$filter=mail eq 'organizator@firma.cz'&$select=id,displayName
```

Zkopíruj hodnotu pole `id` (GUID).

## Krok 3 – Konfigurace

Uprav `appsettings.json`:

```json
{
  "TeamsBot": {
    "AzureAd": {
      "TenantId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
      "ClientId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
      "ClientSecret": "vas~tajny~klic"
    },
    "Bot": {
      "OrganizerUserId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
      "PollingIntervalSeconds": 30,
      "OutputDirectory": "output",
      "EnableAiSummary": true,
      "SubjectKeywords": ["standup", "sprint", "porada"]
    },
    "Anthropic": {
      "ApiKey": "sk-ant-...",
      "Model": "claude-opus-4-6"
    }
  }
}
```

> **Bezpecnost:** Nikdy necommituj `appsettings.json` s reálnými hodnotami.
> Místo toho použij environment proměnné (viz níže) nebo User Secrets.

### Alternativa: Environment proměnné

```bash
export TeamsBot__AzureAd__TenantId="..."
export TeamsBot__AzureAd__ClientId="..."
export TeamsBot__AzureAd__ClientSecret="..."
export TeamsBot__Bot__OrganizerUserId="..."
export TeamsBot__Anthropic__ApiKey="sk-ant-..."
```

### Alternativa: .NET User Secrets (lokální vývoj)

```bash
cd TeamsTranscriptBot
dotnet user-secrets set "TeamsBot:AzureAd:TenantId" "..."
dotnet user-secrets set "TeamsBot:AzureAd:ClientId" "..."
dotnet user-secrets set "TeamsBot:AzureAd:ClientSecret" "..."
dotnet user-secrets set "TeamsBot:Bot:OrganizerUserId" "..."
dotnet user-secrets set "TeamsBot:Anthropic:ApiKey" "sk-ant-..."
```

## Krok 4 – Spuštění

```bash
cd TeamsTranscriptBot
dotnet restore
dotnet run
```

V adresáři `output/` se budou průběžně vytvářet Markdown soubory:

```
output/
  20250301_1000_Tydenni_standup.md
  20250302_1400_Sprint_planning.md
```

## Výstupní formát zápisu

```markdown
# Zápis ze schůzky: Týdenní standup

**Datum:** 01.03.2025 10:00
**Meeting ID:** `MSo1Njc4OTAxMi0...`

---

## Průběh schůzky (transkript)

**Jan Novák** `[00:00:05]`
> Dobrý den, zahajujeme standupové setkání.

**Eva Svobodová** `[00:00:12]`
> Včera jsem dokončila integraci platební brány.

---

## AI Shrnutí (Claude)

## Shrnutí
Týdenní standup týmu, ...

## Klíčová rozhodnutí
- Nasazení na produkci proběhne v pátek

## Úkoly a akční body
| Úkol | Zodpovědná osoba | Termín |
|------|-----------------|--------|
| Otestovat platební bránu | Eva Svobodová | 05.03.2025 |
```

## Nasazení jako služba (Windows / Linux)

### Windows Service

```bash
dotnet publish -c Release -o publish
sc create TeamsTranscriptBot binPath="C:\path\publish\TeamsTranscriptBot.exe"
sc start TeamsTranscriptBot
```

### Linux systemd

```ini
# /etc/systemd/system/teams-transcript-bot.service
[Unit]
Description=Teams Transcript Bot

[Service]
WorkingDirectory=/opt/teams-transcript-bot
ExecStart=/usr/bin/dotnet /opt/teams-transcript-bot/TeamsTranscriptBot.dll
Restart=always
Environment=DOTNET_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
```

```bash
systemctl enable teams-transcript-bot
systemctl start teams-transcript-bot
journalctl -u teams-transcript-bot -f
```

## Struktura projektu

```
TeamsTranscriptBot/
├── Models/
│   ├── AppSettings.cs       # Konfigurace (AzureAd, Bot, Anthropic)
│   ├── Meeting.cs           # Stav sledovaného meetingu
│   └── TranscriptEntry.cs   # Jeden řádek transkriptu
├── Services/
│   ├── AuthService.cs       # MSAL – získání access tokenu
│   ├── GraphService.cs      # Microsoft Graph API volání
│   ├── VttParser.cs         # Parsování WebVTT → Markdown
│   ├── TranscriptWatcher.cs # Hlavní background worker (polling)
│   └── SummaryService.cs    # AI shrnutí přes Claude API
├── Program.cs               # DI kontejner a spuštění hosta
├── appsettings.json         # Šablona konfigurace
└── README.md
```

## Omezení a poznámky

- **Transkript není real-time** – Graph API zpřístupní chunky s ~15–60s zpožděním
- **Transkripce musí být zapnuta** – v Teams admin centru nebo organizátorem schůzky
- **Application permissions** – vyžadují souhlas globálního admina Azure AD
- **Bot se nepřipojuje jako účastník** – pracuje čistě přes Graph API (není potřeba Bot Framework)
