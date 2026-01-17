# Odečty měřidel / Meter Readings

Tento soubor sleduje odečty všech měřidel a automaticky počítá spotřebu.

---

## Elektroměr 1 / Electricity Meter 1 - Daisy Technology

**Číslo měřidla / Meter ID:** `2080259390`
**Model:** Daisy Technology ADX12A-AD-U2H-V2C-G1-OK1
**Typ:** Dvoutarifový (2T) - T1 (VT) / T2 (NT)
**Jednotka / Unit:** kWh

### Tarif T1 (Vysoký tarif / Peak)

| Datum / Date | Čas / Time | Stav měřidla / Reading (kWh) | Spotřeba / Consumption (kWh) | Foto / Image |
|--------------|------------|------------------------------|------------------------------|--------------|
| 2026-01-17 | 12:00 | 10003.876 | - | [IMG_T1_001](images/electricity/daisy_T1_001.jpg) |

### Tarif T2 (Nízký tarif / Off-peak)

| Datum / Date | Čas / Time | Stav měřidla / Reading (kWh) | Spotřeba / Consumption (kWh) | Foto / Image |
|--------------|------------|------------------------------|------------------------------|--------------|
| 2026-01-17 | 12:00 | 1591.0 | - | [IMG_T2_001](images/electricity/daisy_T2_001.jpg) |
| 2026-01-17 | 14:00 | 1591.0 | 0 | [IMG_T2_002](images/electricity/daisy_T2_002.jpg) |

**Celkem (T1 + T2):** 11594.876 kWh

---

## Elektroměr 2 / Electricity Meter 2 - ZPA Třífázový

**Číslo měřidla / Meter ID:** `1023298130`
**Model:** ZE314.D0B1B012-061
**Typ:** Třífázový, dvoutarifový (2T)
**Jednotka / Unit:** kWh

### Tarif T1

| Datum / Date | Čas / Time | Stav měřidla / Reading (kWh) | Spotřeba / Consumption (kWh) | Foto / Image |
|--------------|------------|------------------------------|------------------------------|--------------|
| 2026-01-17 | 12:00 | 24736.0 | - | [IMG_001](images/electricity/zpa_001.jpg) |
| 2026-01-17 | 14:00 | 24874.0 | **138.0** | [IMG_002](images/electricity/zpa_002.jpg) |

---

## Plynoměr 1 / Gas Meter 1 - Actaris Gallus 2000

**Číslo měřidla / Meter ID:** `005541801`
**Model:** Actaris G4 Gallus 2000
**Jednotka / Unit:** m³

| Datum / Date | Čas / Time | Stav měřidla / Reading (m³) | Spotřeba / Consumption (m³) | Foto / Image |
|--------------|------------|-----------------------------|-----------------------------|--------------|
| 2026-01-17 | 12:00 | 3264.575 | - | [IMG_001](images/gas/actaris_001.jpg) |
| 2026-01-17 | 14:00 | 3268.094 | **3.519** | [IMG_002](images/gas/actaris_002.jpg) |

---

## Plynoměr 2 / Gas Meter 2 - Honeywell BK-G4M

**Číslo měřidla / Meter ID:** `004012869380`
**Model:** Honeywell BK-G4M (Elster)
**Jednotka / Unit:** m³

| Datum / Date | Čas / Time | Stav měřidla / Reading (m³) | Spotřeba / Consumption (m³) | Foto / Image |
|--------------|------------|-----------------------------|-----------------------------|--------------|
| 2026-01-17 | 12:00 | 1576.377 | - | [IMG_001](images/gas/honeywell_001.jpg) |
| 2026-01-17 | 14:00 | 1599.855 | **23.478** | [IMG_002](images/gas/honeywell_002.jpg) |

---

## Vodoměr / Water Meter - Sensus

**Číslo měřidla / Meter ID:** `4201238657`
**Model:** Sensus R80
**Jednotka / Unit:** m³

| Datum / Date | Čas / Time | Stav měřidla / Reading (m³) | Spotřeba / Consumption (m³) | Foto / Image |
|--------------|------------|-----------------------------|-----------------------------|--------------|
| 2026-01-17 | 14:00 | 459.0 | - | [IMG_001](images/water/sensus_001.jpg) |

---

## Jak používat / How to Use

### Přidání nového odečtu:

1. **Vyfoťte měřidlo** na telefonu
2. **Nahrajte fotku** do příslušné složky:
   - `images/electricity/` - elektřina
   - `images/gas/` - plyn
   - `images/water/` - voda
3. **Použijte příkaz:** `/meter`

### Výpočet spotřeby:

Spotřeba = Aktuální stav - Předchozí stav

---

## Souhrn spotřeby / Consumption Summary

### Elektřina / Electricity

**Elektroměr Daisy (2080259390):**
- Stav T1 (VT): 10003.876 kWh
- Stav T2 (NT): 1591.0 kWh
- **Celkem:** 11594.876 kWh
- Spotřeba T2: 0 kWh

**Elektroměr ZPA (1023298130):**
- Stav T1: 24874.0 kWh
- **Spotřeba T1:** 138.0 kWh

### Plyn / Gas

**Plynoměr Actaris (005541801):**
- Stav: 3268.094 m³
- **Spotřeba:** 3.519 m³

**Plynoměr Honeywell (004012869380):**
- Stav: 1599.855 m³
- **Spotřeba:** 23.478 m³

**Celková spotřeba plynu:** 26.997 m³

### Voda / Water

**Vodoměr Sensus (4201238657):**
- Stav: 459.0 m³
- Spotřeba: - (první odečet)
