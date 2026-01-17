Zpracuj fotografii měřidla a aktualizuj tabulku odečtů.

## Tvůj úkol:

1. **Přečti obrázek** z cesty: $ARGUMENTS (pokud není zadána, zeptej se uživatele na cestu k souboru)

2. **Zjisti datum pořízení fotky z EXIF dat:**
   ```bash
   exiftool -DateTimeOriginal -CreateDate -FileModifyDate "cesta/k/souboru.jpg"
   ```
   - Použij `DateTimeOriginal` nebo `CreateDate` jako datum odečtu
   - Pokud EXIF data nejsou k dispozici, použij aktuální datum/čas

3. **Identifikuj z obrázku:**
   - Typ měřidla (elektřina/plyn/voda)
   - Aktuální stav měřidla (číslo)
   - Číslo/ID měřidla (pokud je viditelné)
   - Jednotku (kWh, m³)

4. **Přečti soubor** `Meter/meter-readings.md`

5. **Přidej nový řádek** do příslušné tabulky:
   - Datum: z EXIF dat (YYYY-MM-DD)
   - Čas: z EXIF dat (HH:MM)
   - Stav měřidla: hodnota z fotky
   - Spotřeba: vypočítej rozdíl od posledního odečtu
   - Foto: odkaz na obrázek

6. **Seřaď záznamy chronologicky** podle data a času

7. **Vypočítej spotřebu:**
   - Najdi předchozí odečet ve stejné tabulce
   - Spotřeba = Nový stav - Předchozí stav
   - Pokud je to první odečet, napiš "-"
   - Vypočítej denní průměr: Spotřeba / počet dní mezi odečty

8. **Aktualizuj souhrn** na konci souboru (celková a průměrná denní spotřeba)

9. **Přesuň obrázek** do správné složky (pokud ještě není):
   - Elektřina → `Meter/images/electricity/`
   - Plyn → `Meter/images/gas/`
   - Voda → `Meter/images/water/`

10. **Potvrď uživateli:**
    - Datum pořízení fotky (z EXIF)
    - Jakou hodnotu jsi přečetl
    - Kolik činí spotřeba od posledního odečtu
    - Denní průměr spotřeby
    - Kde je uložen záznam

## Příklad použití:
```
/meter /path/to/photo.jpg
/meter Meter/images/electricity/IMG_20260117.jpg
```

## Příklad výstupu exiftool:
```
$ exiftool -DateTimeOriginal IMG_001.jpg
Date/Time Original : 2026:01:01 10:30:00
```
→ Datum odečtu: 2026-01-01, Čas: 10:30
