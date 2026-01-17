# Odečty měřidel / Meter Readings

Tento soubor sleduje odečty všech měřidel a automaticky počítá spotřebu.

---

# Domácnost 1 - Komárkovi

## Elektroměr - Daisy Technology (Komárkovi)

**Číslo měřidla / Meter ID:** `2080259390`
**Model:** Daisy Technology ADX12A-AD-U2H-V2C-G1-OK1
**Typ:** Dvoutarifový (2T) - T1 (VT) / T2 (NT)
**Jednotka / Unit:** kWh

### Tarif T1 (Vysoký tarif / Peak)

| Datum / Date | Čas / Time | Stav měřidla / Reading (kWh) | Spotřeba / Consumption (kWh) | Foto / Image |
|--------------|------------|------------------------------|------------------------------|--------------|
| 2026-01-01 | 11:00 | 10003.876 | - | [IMG_T1_001](images/electricity/daisy_T1_001.jpg) |

### Tarif T2 (Nízký tarif / Off-peak)

| Datum / Date | Čas / Time | Stav měřidla / Reading (kWh) | Spotřeba / Consumption (kWh) | Foto / Image |
|--------------|------------|------------------------------|------------------------------|--------------|
| 2026-01-01 | 11:00 | 1591.0 | - | [IMG_T2_001](images/electricity/daisy_T2_001.jpg) |
| 2026-01-17 | 14:00 | 1591.0 | 0 | [IMG_T2_002](images/electricity/daisy_T2_002.jpg) |

---

## Plynoměr - Honeywell (Komárkovi - podružný)

**Číslo měřidla / Meter ID:** `004012869380`
**Model:** Honeywell BK-G4M (Elster)
**Typ:** Podružný plynoměr pro domácnost Komárků
**Jednotka / Unit:** m³

| Datum / Date | Čas / Time | Stav měřidla / Reading (m³) | Spotřeba / Consumption (m³) | Foto / Image |
|--------------|------------|-----------------------------|-----------------------------|--------------|
| 2026-01-01 | 11:00 | 1576.377 | - | [IMG_001](images/gas/honeywell_001.jpg) |
| 2026-01-17 | 14:00 | 1599.855 | **23.478** | [IMG_002](images/gas/honeywell_002.jpg) |

---

# Domácnost 2 - Druhá rodina

## Elektroměr - ZPA Třífázový (Druhá rodina)

**Číslo měřidla / Meter ID:** `1023298130`
**Model:** ZE314.D0B1B012-061
**Typ:** Třífázový, dvoutarifový (2T)
**Jednotka / Unit:** kWh

### Tarif T1

| Datum / Date | Čas / Time | Stav měřidla / Reading (kWh) | Spotřeba / Consumption (kWh) | Foto / Image |
|--------------|------------|------------------------------|------------------------------|--------------|
| 2026-01-01 | 11:00 | 24736.0 | - | [IMG_001](images/electricity/zpa_001.jpg) |
| 2026-01-17 | 14:00 | 24874.0 | **138.0** | [IMG_002](images/electricity/zpa_002.jpg) |

---

## Plynoměr - Actaris (Druhá rodina)

**Číslo měřidla / Meter ID:** `005541801`
**Model:** Actaris G4 Gallus 2000
**Typ:** Samostatný plynoměr pro druhou rodinu
**Jednotka / Unit:** m³

| Datum / Date | Čas / Time | Stav měřidla / Reading (m³) | Spotřeba / Consumption (m³) | Foto / Image |
|--------------|------------|-----------------------------|-----------------------------|--------------|
| 2026-01-01 | 11:00 | 3264.575 | - | [IMG_001](images/gas/actaris_001.jpg) |
| 2026-01-17 | 14:00 | 3268.094 | **3.519** | [IMG_002](images/gas/actaris_002.jpg) |

---

# Společné měřidlo

## Vodoměr - Sensus (Celý dům)

**Číslo měřidla / Meter ID:** `4201238657`
**Model:** Sensus R80
**Typ:** Společný vodoměr pro celý dům
**Jednotka / Unit:** m³

| Datum / Date | Čas / Time | Stav měřidla / Reading (m³) | Spotřeba / Consumption (m³) | Foto / Image |
|--------------|------------|-----------------------------|-----------------------------|--------------|
| 2026-01-17 | 14:00 | 459.0 | - | [IMG_001](images/water/sensus_001.jpg) |

---

# Souhrn spotřeby / Consumption Summary

**Období:** 2026-01-01 11:00 → 2026-01-17 14:00 (16 dní)

## Domácnost 1 - Komárkovi

### Elektřina (Daisy)
| Tarif | Spotřeba | Denní průměr |
|-------|----------|--------------|
| T1 (VT) | 0 kWh | 0 kWh/den |
| T2 (NT) | 0 kWh | 0 kWh/den |
| **Celkem** | **0 kWh** | **0 kWh/den** |

### Plyn (Honeywell)
| Spotřeba | Denní průměr |
|----------|--------------|
| **23.478 m³** | **1.47 m³/den** |

---

## Domácnost 2 - Druhá rodina

### Elektřina (ZPA)
| Tarif | Spotřeba | Denní průměr |
|-------|----------|--------------|
| T1 | **138.0 kWh** | **8.6 kWh/den** |

### Plyn (Actaris)
| Spotřeba | Denní průměr |
|----------|--------------|
| **3.519 m³** | **0.22 m³/den** |

---

## Voda (společná)
| Stav | Spotřeba |
|------|----------|
| 459.0 m³ | - (první odečet) |

---

# Jak používat / How to Use

### Přidání nového odečtu:

1. **Vyfoťte měřidlo** na telefonu
2. **Nahrajte fotku** do příslušné složky (pro automatické čtení EXIF data)
3. **Použijte příkaz:** `/meter cesta/k/souboru.jpg`

### Struktura měřidel:

- **Komárkovi:** Daisy (elektřina) + Honeywell (plyn)
- **Druhá rodina:** ZPA (elektřina) + Actaris (plyn)
- **Společné:** Sensus (voda)
