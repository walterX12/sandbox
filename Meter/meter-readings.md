# Odečty měřidel / Meter Readings

Tento soubor sleduje odečty všech měřidel a automaticky počítá spotřebu.

---

## Elektroměr / Electricity Meter

**Číslo měřidla / Meter ID:** `_DOPLNIT_`
**Jednotka / Unit:** kWh

| Datum / Date | Čas / Time | Stav měřidla / Reading (kWh) | Spotřeba / Consumption (kWh) | Foto / Image |
|--------------|------------|------------------------------|------------------------------|--------------|
| 2026-01-17 | 10:00 | 12345.6 | - | [IMG_001](images/electricity/IMG_001.jpg) |
| | | | | |

---

## Plynoměr / Gas Meter

**Číslo měřidla / Meter ID:** `_DOPLNIT_`
**Jednotka / Unit:** m³

| Datum / Date | Čas / Time | Stav měřidla / Reading (m³) | Spotřeba / Consumption (m³) | Foto / Image |
|--------------|------------|-----------------------------|-----------------------------|--------------|
| 2026-01-17 | 10:00 | 1234.567 | - | [IMG_001](images/gas/IMG_001.jpg) |
| | | | | |

---

## Vodoměr / Water Meter

**Číslo měřidla / Meter ID:** `_DOPLNIT_`
**Jednotka / Unit:** m³

| Datum / Date | Čas / Time | Stav měřidla / Reading (m³) | Spotřeba / Consumption (m³) | Foto / Image |
|--------------|------------|-----------------------------|-----------------------------|--------------|
| 2026-01-17 | 10:00 | 567.890 | - | [IMG_001](images/water/IMG_001.jpg) |
| | | | | |

---

## Jak používat / How to Use

### Přidání nového odečtu:

1. **Vyfoťte měřidlo** na telefonu
2. **Nahrajte fotku** do příslušné složky:
   - `images/electricity/` - elektřina
   - `images/gas/` - plyn
   - `images/water/` - voda
3. **Pošlete Claude prompt:**

```
Podívej se na obrázek měřidla v souboru [cesta k souboru].
Identifikuj:
1. Aktuální stav měřidla (číslo)
2. Číslo měřidla (pokud je viditelné)
3. Datum a čas z fotky (pokud je v EXIF datech)

Přidej nový řádek do tabulky v meter-readings.md a vypočítej spotřebu od posledního odečtu.
```

### Výpočet spotřeby:

Spotřeba = Aktuální stav - Předchozí stav

---

## Souhrn spotřeby / Consumption Summary

### Elektřina / Electricity
- **Celková spotřeba:** _vypočítat_ kWh
- **Průměrná denní spotřeba:** _vypočítat_ kWh/den

### Plyn / Gas
- **Celková spotřeba:** _vypočítat_ m³
- **Průměrná denní spotřeba:** _vypočítat_ m³/den

### Voda / Water
- **Celková spotřeba:** _vypočítat_ m³
- **Průměrná denní spotřeba:** _vypočítat_ l/den
