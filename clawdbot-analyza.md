# Analýza Clawdbot

## Co je Clawdbot?

**Clawdbot** je open-source, self-hosted AI asistent, který funguje jako prostředník mezi vašimi komunikačními platformami a Claude (nebo jinými AI modely).

- **GitHub**: https://github.com/clawdbot/clawdbot
- **Licence**: MIT (zdarma)

## Hlavní funkce

| Funkce | Popis |
|--------|-------|
| **Multi-platforma** | WhatsApp, Telegram, Slack, Discord, Signal, iMessage, WebChat |
| **Hlasové ovládání** | Rozpoznávání řeči na macOS/iOS/Android |
| **Lokální běh** | Běží na vašem zařízení, ne v cloudu |
| **Sandbox** | Volitelná Docker izolace pro bezpečnost |
| **Companion apps** | Aplikace pro macOS, iOS, Android s přístupem ke kameře, nahrávání obrazovky |

## Požadavky

- Node.js 22+
- pnpm (nebo npm/bun)
- Volitelně: Docker pro sandboxing

## Jak funguje předplatné?

### Potřebujete Claude Code předplatné?
**NE.** Clawdbot je úplně odlišný produkt od Claude Code.

### Potřebujete Anthropic API?
**ANO, ale máte dvě možnosti:**

1. **Claude Pro/Max předplatné** ($20/$100/$200/měsíc) - Clawdbot podporuje OAuth přihlášení
2. **Anthropic API klíč** - Platíte za tokeny podle spotřeby

Clawdbot samotný je **zdarma**. Platíte pouze za přístup k AI modelu.

## Architektura

```
┌─────────────────────────────────────────────────┐
│           VÁŠ POČÍTAČ (localhost)               │
│                                                 │
│  ┌─────────────┐      ┌──────────────────────┐  │
│  │  Clawdbot   │ ───► │ Bash, soubory, git,  │  │
│  │  Gateway    │      │ kompilace, testy...  │  │
│  └──────┬──────┘      └──────────────────────┘  │
│         │                                       │
└─────────┼───────────────────────────────────────┘
          │ API calls
          ▼
   ┌──────────────┐
   │  Anthropic   │  (váš Pro/Max nebo API klíč)
   │  Cloud       │
   └──────────────┘
```

## Srovnání nástrojů

| Aspekt | Claude.ai Web | Claude Code CLI | Clawdbot |
|--------|---------------|-----------------|----------|
| **Kde běží AI** | Anthropic cloud | Anthropic cloud | Anthropic cloud |
| **Kde běží nástroje** | Nikde (jen chat) | Váš počítač | Váš počítač |
| **Přístup** | Prohlížeč | Terminál | WhatsApp, Telegram, Discord... |
| **Kompilace/testy** | ❌ | ✅ | ✅ |
| **Bash příkazy** | ❌ | ✅ | ✅ |
| **Přístup k souborům** | Pouze upload | ✅ Plný | ✅ Plný |
| **Mobilní přístup** | Prohlížeč/app | ❌ | ✅ Nativní chat apps |
| **Push notifikace** | ❌ | ❌ | ✅ |
| **Skupinové chaty** | ❌ | ❌ | ✅ |
| **Self-hosted** | ❌ | ❌ | ✅ |
| **Oficiální podpora** | ✅ | ✅ | ❌ (komunitní) |

## Výhody Clawdbot

1. **Pohodlí** - Píšete Claudovi ve stejné appce, kde máte kamarády
2. **Notifikace** - Push notifikace jako od kohokoliv jiného
3. **Skupinové chaty** - Můžete přidat bota do skupiny
4. **Systémový přístup** - Může ovládat váš počítač (bash, soubory, screenshoty)
5. **Soukromí** - Vše běží lokálně u vás

## Výhody Claude Code

1. **Oficiální produkt** - Stabilní, dobře udržovaný
2. **Jednoduchost** - Stačí napsat `claude` v terminálu
3. **Žádná komplexní instalace** - Není potřeba spravovat server

## Kdy použít co?

### Clawdbot je pro vás, pokud:
- Chcete komunikovat s Claude přes **WhatsApp/Telegram/Discord** z mobilu
- Chcete **hlasové ovládání**
- Chcete mít **plnou kontrolu** nad daty (self-hosted)
- Chcete sdílet přístup s ostatními (skupinové chaty)
- Máte Claude Pro/Max a chcete ho využít jinak než přes web

### Claude Code je pro vás, pokud:
- Potřebujete primárně **programovat a pracovat s kódem**
- Preferujete **oficiální, podporovaný nástroj**
- Nechcete spravovat vlastní server
- Stačí vám přístup z terminálu

### Claude.ai web je pro vás, pokud:
- Stačí vám **jednoduchý chat** bez spouštění kódu
- Nechcete nic instalovat
- Nepotřebujete přístup k lokálním souborům

## Shrnutí

**Clawdbot = Claude Code možnosti + přístup odkudkoli přes chat aplikace**

Je to v podstatě "most" mezi vašimi oblíbenými chat aplikacemi a Claude s plným přístupem k vašemu systému. Hlavní výhoda je pohodlí přístupu odkudkoli přes aplikace, které už používáte.
