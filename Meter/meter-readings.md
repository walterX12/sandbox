# Odečty měřidel / Meter Readings

Tento soubor sleduje odečty všech měřidel a automaticky počítá spotřebu.

---

# Hlavní plynoměr - Honeywell (Celý dům)

**Číslo měřidla / Meter ID:** `004012869380`
**Model:** Honeywell BK-G4M (Elster)
**Typ:** Hlavní plynoměr - měří spotřebu celého domu
**Jednotka / Unit:** m³

| Datum / Date | Čas / Time | Stav měřidla / Reading (m³) | Spotřeba / Consumption (m³) | Foto / Image |
|--------------|------------|-----------------------------|-----------------------------|--------------|
| 2026-01-01 | 11:00 | 1576.377 | - | [IMG_001](images/gas/honeywell_001.jpg) |
| 2026-01-17 | 14:00 | 1599.855 | **23.478** | [IMG_002](images/gas/honeywell_002.jpg) |
| 2026-01-18 | - | 1608.421 | **8.566** | [IMG_003](images/gas/honeywell_003.jpg) |

---

# Domácnost 1 - Komárkovi

## Elektroměr - ZPA Třífázový (Komárkovi)

**Číslo měřidla / Meter ID:** `1023298130`
**Model:** ZE314.D0B1B012-061
**Typ:** Třífázový, dvoutarifový (2T)
**Jednotka / Unit:** kWh

### Tarif T1

| Datum / Date | Čas / Time | Stav měřidla / Reading (kWh) | Spotřeba / Consumption (kWh) | Foto / Image |
|--------------|------------|------------------------------|------------------------------|--------------|
| 2026-01-01 | 11:00 | 24736.0 | - | [IMG_001](images/electricity/zpa_001.jpg) |
| 2026-01-17 | 14:00 | 24874.0 | **138.0** | [IMG_002](images/electricity/zpa_002.jpg) |
| 2026-01-18 | - | 24885.0 | **11.0** | [IMG_003](images/electricity/zpa_003.jpg) |

---

## Plynoměr - Actaris (Komárkovi - podružný)

**Číslo měřidla / Meter ID:** `005541801`
**Model:** Actaris G4 Gallus 2000
**Typ:** Podružný plynoměr - měří spotřebu Komárků
**Jednotka / Unit:** m³

| Datum / Date | Čas / Time | Stav měřidla / Reading (m³) | Spotřeba / Consumption (m³) | Foto / Image |
|--------------|------------|-----------------------------|-----------------------------|--------------|
| 2026-01-01 | 11:00 | 3264.575 | - | [IMG_001](images/gas/actaris_001.jpg) |
| 2026-01-17 | 14:00 | 3268.094 | **3.519** | [IMG_002](images/gas/actaris_002.jpg) |
| 2026-01-18 | - | 3268.268 | **0.174** | [IMG_003](images/gas/actaris_003.jpg) |

---

# Domácnost 2 - Charvátovi

## Elektroměr - Daisy Technology (Charvátovi)

**Číslo měřidla / Meter ID:** `2080259390`
**Model:** Daisy Technology ADX12A-AD-U2H-V2C-G1-OK1
**Typ:** Dvoutarifový (2T) - T1 (denní) / T2 (noční)
**Jednotka / Unit:** kWh

### Tarif T1 (Denní proud / Day)

| Datum / Date | Čas / Time | Stav měřidla / Reading (kWh) | Spotřeba / Consumption (kWh) | Foto / Image |
|--------------|------------|------------------------------|------------------------------|--------------|
| 2026-01-01 | 11:00 | 3802.0 | - | [IMG_T1_001](images/electricity/daisy_T1_001.jpg) |
| 2026-01-17 | 14:00 | 3876.0 | **74.0** | [IMG_T1_002](images/electricity/daisy_T1_002.jpg) |
| 2026-01-18 | - | 3881.0 | **5.0** | [IMG_T1_003](images/electricity/daisy_T1_003.jpg) |

### Tarif T2 (Noční proud / Night)

| Datum / Date | Čas / Time | Stav měřidla / Reading (kWh) | Spotřeba / Consumption (kWh) | Foto / Image |
|--------------|------------|------------------------------|------------------------------|--------------|
| 2026-01-01 | 11:00 | 1591.0 | - | [IMG_T2_001](images/electricity/daisy_T2_001.jpg) |
| 2026-01-17 | 14:00 | 1591.0 | 0 | [IMG_T2_002](images/electricity/daisy_T2_002.jpg) |

---

## Plyn - Charvátovi (výpočet)

**Výpočet:** Honeywell (celý dům) - Actaris (Komárci) = spotřeba Charvátů

| Období | Honeywell (dům) | Actaris (Komárci) | Charvátovi |
|--------|-----------------|-------------------|------------|
| 01.01. - 17.01. | 23.478 m³ | 3.519 m³ | **19.959 m³** |
| 17.01. - 18.01. | 8.566 m³ | 0.174 m³ | **8.392 m³** |
| **CELKEM 01.01. - 18.01.** | **32.044 m³** | **3.693 m³** | **28.351 m³** |

---

# Společné měřidlo

## Vodoměr - Sensus (Celý dům)

**Číslo měřidla / Meter ID:** `4201238657`
**Model:** Sensus R80
**Typ:** Společný vodoměr pro celý dům
**Jednotka / Unit:** m³

| Datum / Date | Čas / Time | Stav měřidla / Reading (m³) | Spotřeba / Consumption (m³) | Foto / Image |
|--------------|------------|-----------------------------|-----------------------------|--------------|
| 2026-01-01 | 11:00 | 459.0 | - | [IMG_001](images/water/sensus_001.jpg) |
| 2026-01-18 | - | 467.0 | **8.0** (17 dní) | [IMG_002](images/water/sensus_002.jpg) |

---

# Souhrn spotřeby / Consumption Summary

**Období:** 2026-01-01 11:00 → 2026-01-17 14:00 (**16 dní**)

## Domácnost 1 - Komárkovi

| Energie | Spotřeba | Denní průměr | Měsíční odhad | Roční projekce |
|---------|----------|--------------|---------------|----------------|
| **Elektřina (ZPA) T1** | **138.0 kWh** | **8.6 kWh/den** | **258 kWh** | **3 139 kWh** |
| **Plyn (Actaris)** | **3.519 m³** | **0.22 m³/den** | **6.6 m³** | **80 m³** |

---

## Domácnost 2 - Charvátovi

| Energie | Spotřeba | Denní průměr | Měsíční odhad | Roční projekce |
|---------|----------|--------------|---------------|----------------|
| **Elektřina (Daisy) T1 - denní** | **74.0 kWh** | **4.6 kWh/den** | **138 kWh** | **1 679 kWh** |
| Elektřina (Daisy) T2 - noční | 0 kWh | 0 kWh/den | 0 kWh | 0 kWh |
| **Plyn (Honeywell-Actaris)** | **19.959 m³** | **1.25 m³/den** | **37.5 m³** | **456 m³** |

---

## Celý dům

| Energie | Spotřeba | Denní průměr | Měsíční odhad | Roční projekce |
|---------|----------|--------------|---------------|----------------|
| Elektřina celkem | 212.0 kWh | 13.3 kWh/den | 399 kWh | **4 855 kWh** |
| **Plyn celkem (Honeywell)** | **23.478 m³** | **1.47 m³/den** | **44.1 m³** | **537 m³** |
| Voda | - | - | - | - |

---

# Jak používat / How to Use

### Přidání nového odečtu:

1. **Vyfoťte měřidlo** na telefonu
2. **Nahrajte fotku** do příslušné složky (pro automatické čtení EXIF data)
3. **Použijte příkaz:** `/meter cesta/k/souboru.jpg`

### Struktura měřidel:

```
PLYN:
Honeywell (hlavní - celý dům)
    └── Actaris (podružný - Komárci)
        Charvátovi = Honeywell - Actaris

ELEKTŘINA:
├── ZPA T1 (Komárkovi)
└── Daisy T1+T2 (Charvátovi) - denní/noční proud

VODA:
└── Sensus (společná)
```

### Vzorec pro výpočet plynu Charvátů:

```
Spotřeba Charvátů = Honeywell (celý dům) - Actaris (Komárci)
```

---

# Poznámky / Notes

- **Komárkovi:** ZPA elektřina + Actaris plyn (podružný měřič)
- **Charvátovi:** Daisy elektřina (T1 denní, T2 noční) + plyn = rozdíl Honeywell - Actaris
- **Voda:** společná pro celý dům (Sensus)

---

# Poznámky k období 01.01. - 17.01.2026

## Plyn - nižší spotřeba
- **Od 6. ledna 2026** je plynový kotel mimo provoz (porucha)
- Čekáme na opravu - **předpokládané dokončení: 25. ledna 2026**
- Proto je spotřeba plynu v tomto období nižší než obvykle

## Elektřina - vyšší spotřeba
- Proběhly **2× řezání dřeva** na elektrické cirkulárce (příkon **5 kW**)
- Toto pravděpodobně navýšilo spotřebu elektřiny

---

# Poznámky k období 17.01. - 18.01.2026 (sobota)

## Plyn - vysoká spotřeba (8.566 m³ za 1 den!)
- **Babička s dědou byli doma** - babička vařila (používá plyn)
- Proto vysoká spotřeba Charvátů: **8.392 m³** za jediný den
- Komárci spotřebovali pouze **0.174 m³** (bez kotle)

## Elektřina
- Řezání dřeva na cirkulárce **5 kW** přibližně **2 hodiny** (~10 kWh)
- ZPA (Komárci): +11 kWh
- Daisy (Charvátovi): +5 kWh
- Jinak běžný provoz, nikdo se nekoupal (neohřívali vodu)

---

# Profil domácnosti (pro odhady spotřeby)

## Základní údaje
- **Počet osob:** 7 lidí (2 domácnosti)
- **Komárkovi:** ? osob
- **Charvátovi:** ? osob (babička a děda)

## Vybavení domácnosti

### Pračka
- **Rok výroby:** 2020 (moderní)
- **Odhadovaná spotřeba:** 35-45 litrů/cyklus
- **Frekvence praní:** 1-2× denně

### Myčka nádobí
- **Spotřeba:** 10-15 litrů/cyklus
- **Nádobí myjeme v myčce** (ne ručně)

### Koupání
- **Vana:** téměř nepoužíváme (výjimečně)
- **Sprchy:** 2-4 osoby denně
- **Spotřeba sprchy:** 30-50 litrů (5 min)

### WC
- **Odhad splachování:** 7 osob × ~5× denně = 35× denně
- **Spotřeba:** 6-9 litrů/spláchnutí
- **Celkem WC:** ~210-315 litrů/den

## Typický denní odhad spotřeby vody

| Činnost | Počet | Spotřeba |
|---------|-------|----------|
| Sprchy | 2-4× | 70-150 l |
| Praní | 1-2× | 40-80 l |
| Myčka | 1× | 12 l |
| WC | ~35× | 210-315 l |
| Pití/vaření/ruce | - | ~40 l |
| **CELKEM** | | **~370-600 l** |

**Skutečná spotřeba:** 470 l/den = **v normě**

## Ceny energií (2025/2026)

### Voda (Aquaconsult - Všenory, platné od 1.1.2026)
- **Vodné:** 76,10 Kč/m³ (vč. DPH 12%)
- **Stočné:** 80,14 Kč/m³ (vč. DPH 12%)
- **Celkem:** **156,24 Kč/m³**
- Zdroj: [Aquaconsult ceník 2026](https://aquaconsult.cz/wp-content/uploads/2024/01/CENIK_VODNE_STOCNE_AKTUALIZACE_01012024.pdf)

### Plyn
- TODO: doplnit aktuální cenu

### Elektřina
- TODO: doplnit aktuální cenu ČEZ
