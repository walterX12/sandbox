# Deepgram ASR - Komplexní analýza možností

## Shrnutí

Deepgram je přední poskytovatel Voice AI platformy specializující se na převod řeči na text (Speech-to-Text, STT), text na řeč (Text-to-Speech, TTS) a plné speech-to-speech (STS) řešení. Platforma využívá více než 200 000 vývojářů po celém světě.

---

## Dostupné modely

### Nova-3 (Nejnovější flagship model)

**Klíčové vlastnosti:**
- **Přesnost**: 50%+ nižší Word Error Rate (WER) oproti konkurenci
- **WER benchmarky**: 7.6-18% v závislosti na typu dat
- **Latence**: Pod 300 ms pro real-time transkripci
- **Jazyková podpora**: 36+ jazyků, code-switching pro 10 jazyků v reálném čase

**Pokročilé funkce:**
- Vylepšené rozpoznávání čísel a numerických entit
- Doménově specifická slovní zásoba (long-tail terminologie)
- Real-time redakce až 50 typů citlivých entit
- Přesné word-level časové značky
- Code-switching: angličtina, španělština, francouzština, němčina, hindština, ruština, portugalština, japonština, italština, holandština

### Nova-3 Medical

Specializovaný model pro zdravotnictví:
- Optimalizován pro lékařskou terminologii
- Farmaceutické názvy
- Lékařské zkratky
- Latinské názvy nemocí

### Flux (Nový v 2025)

**První konverzační model pro rozpoznávání řeči (CSR):**
- Speciálně navržen pro hlasové agenty
- Automatické řízení turn-taking (střídání mluvčích)
- Rozumí konverzačnímu toku
- Řeší problém přerušování v hlasových aplikacích
- Ví kdy naslouchat, kdy přemýšlet a kdy mluvit

### Enhanced model

- Silný výkon pro neobvyklá slova
- Vhodný pro specializované slovníky

### Base model

- Základní model s dobrým poměrem přesnosti a ceny
- Vhodný pro běžné použití

---

## Cenová struktura

### Speech-to-Text (Nova-3)

| Typ zpracování | Cena za minutu |
|----------------|----------------|
| Pre-recorded (batch) | $0.0043/min |
| Real-time streaming | $0.0077/min |

**Cenové výhody:**
- Účtování po sekundách (ne po minutách jako konkurence)
- Žádné minimum, žádné expirační lhůty
- Registrace zdarma bez kreditní karty

### Cenové plány

| Plán | Popis |
|------|-------|
| **Pay-as-you-go** | Platba podle skutečného využití |
| **Growth** | Předplacené kredity na rok |
| **Enterprise** | Self-hosted/on-premises řešení |

### Porovnání cen s konkurencí

| Poskytovatel | Cena za 1000 min | WER |
|--------------|------------------|-----|
| Deepgram Nova-3 | ~$4.30 | 7.6-18% |
| Google Chirp 2 | ~$16.00 | 9.8-13.1% |
| Azure Speech | Variabilní | Střední |

---

## Technická integrace

### WebSocket real-time streaming

**Speech-to-Text:**
```
wss://api.deepgram.com/v1/listen
```

**Funkce streamingu:**
- Full-duplex komunikace
- Interim transkripce + finální transkripce
- Speaker diarization (identifikace mluvčích)
- Konfigurovatelná finalizace řeči (`speech_final`)
- 60minutový timeout na jedno spojení

### Text-to-Speech WebSocket

- Real-time konverze textu na řeč
- Ideální pro chatboty a IVR systémy
- Nízká latence
- Streamování tokenů z LLM přímo do TTS

### Dostupné SDK

| Jazyk | Status |
|-------|--------|
| JavaScript/TypeScript | Oficiální |
| Python | Oficiální |
| .NET | Oficiální |
| Go | Oficiální |
| Rust | Komunitní |

### Autentizace

```python
# Možnosti autentizace:
# 1. Explicitní access token
# 2. Environment variable: DEEPGRAM_TOKEN
# 3. Generovaný token z API klíče
```

---

## Funkce zahrnuté v ceně

- **Speaker Diarization**: Identifikace mluvčích
- **Smart Formatting**: Interpunkce, velká písmena, odstavce
- **Automatic Language Detection**: Automatická detekce jazyka
- **Deep Search**: Vyhledávání v transkriptech
- **Keyword Boosting**: Zvýšení přesnosti klíčových slov (až 90%)
- **Multichannel Support**: Podpora více kanálů
- **Callbacks**: Webhooky pro notifikace
- **Numeral Conversion**: Čísla jako číslice
- **Filler Word Detection**: Detekce "uh", "um"
- **Vocabulary Prompting**: Vlastní slovník (až 100 termínů)
- **Redaction**: Odstranění osobních údajů

---

## Podporované jazyky

### Nova-3 (Code-switching)
1. Angličtina (en)
2. Španělština (es)
3. Francouzština (fr)
4. Němčina (de)
5. Hindština (hi)
6. Ruština (ru)
7. Portugalština (pt)
8. Japonština (ja)
9. Italština (it)
10. Holandština (nl)

### Nova-2 a další modely
- 36+ jazyků a dialektů
- Nedávno přidané: němčina, holandština, švédština, dánština

### Aura-2 TTS (Text-to-Speech)
- Angličtina
- Holandština (nově 2025)
- Němčina (nově 2025)
- Francouzština (nově 2025)
- Italština (nově 2025)
- Japonština (nově 2025)

---

## Porovnání s konkurencí

### Deepgram vs Google Cloud Speech

| Aspekt | Deepgram | Google |
|--------|----------|--------|
| WER | 7.6% | 13.1% |
| Latence | <300ms | Vyšší |
| Cena/1000min | $4.30 | $16.00 |
| Jazyky | 36+ | 125+ |
| Šum | Robustnější | Standardní |

### Deepgram vs Azure Speech

| Aspekt | Deepgram | Azure |
|--------|----------|-------|
| Latence | <300ms | 400-800ms |
| Jazyky | 36+ | 140+ |
| Integrace | Specializovaná | Plná Azure ekosystém |
| Speaker diarization | Ano | Ano |

### Deepgram vs OpenAI Whisper

| Aspekt | Deepgram | Whisper |
|--------|----------|---------|
| Real-time | Ano | Omezené |
| Latence | <300ms | Vyšší |
| On-premise | Enterprise | Open-source |
| Přesnost | Srovnatelná | Srovnatelná |

---

## Případy užití

### Ideální pro:
1. **Call centra** - nízká latence, vysoká přesnost
2. **Lékařská dokumentace** - Nova-3 Medical
3. **Hlasové agenty** - Flux model
4. **Live titulky** - real-time streaming
5. **Konference a eventy** - speaker diarization
6. **Konverzační AI** - speech-to-speech

### Silné stránky:
- Nejlepší poměr cena/výkon
- Nejnižší latence na trhu
- Robustnost v hlučném prostředí
- Doménově specifické slovníky
- Flexibilní deployment (cloud/on-premise)

### Omezení:
- Méně jazyků než Google/Azure
- Nova-3 code-switching pouze 10 jazyků
- 60minutový timeout WebSocket spojení

---

## Doporučení

### Pro české projekty:
- Čeština není v primární podpoře Nova-3
- Zkontrolovat dostupnost v Nova-2 (36+ jazyků)
- Možnost custom modelu pro Enterprise

### Pro real-time aplikace:
- Použít Nova-3 nebo Flux
- WebSocket integrace
- Implementovat reconnection logiku

### Pro medicínu:
- Nova-3 Medical je nejlepší volba
- Redakce HIPAA-compliant dat

---

## Zdroje

- [Deepgram Pricing](https://deepgram.com/pricing)
- [Model Options Documentation](https://developers.deepgram.com/docs/model)
- [Nova-3 Announcement](https://deepgram.com/learn/introducing-nova-3-speech-to-text-api)
- [Flux Launch](https://www.businesswire.com/news/home/20251002758871/en/Deepgram-Launches-Flux)
- [Speech-to-Text API Benchmarks](https://deepgram.com/learn/speech-to-text-benchmarks)
- [Deepgram vs Google Comparison](https://deepgram.com/learn/deepgram-vs-google-speech-to-text-comparison)
- [Deepgram Python SDK](https://github.com/deepgram/deepgram-python-sdk)
- [WebSocket Documentation](https://developers.deepgram.com/docs/lower-level-websockets)

---

*Analýza vytvořena: Leden 2025*
