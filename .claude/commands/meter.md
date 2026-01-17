Zpracuj fotografii měřidla a aktualizuj tabulku odečtů.

## Tvůj úkol:

1. **Přečti obrázek** z cesty: $ARGUMENTS (pokud není zadána, zeptej se uživatele na cestu k souboru)

2. **Identifikuj z obrázku:**
   - Typ měřidla (elektřina/plyn/voda)
   - Aktuální stav měřidla (číslo)
   - Číslo/ID měřidla (pokud je viditelné)
   - Jednotku (kWh, m³)

3. **Přečti soubor** `Meter/meter-readings.md`

4. **Přidej nový řádek** do příslušné tabulky:
   - Datum: dnešní datum (YYYY-MM-DD)
   - Čas: aktuální čas (HH:MM)
   - Stav měřidla: hodnota z fotky
   - Spotřeba: vypočítej rozdíl od posledního odečtu
   - Foto: odkaz na obrázek

5. **Vypočítej spotřebu:**
   - Najdi předchozí odečet ve stejné tabulce
   - Spotřeba = Nový stav - Předchozí stav
   - Pokud je to první odečet, napiš "-"

6. **Aktualizuj souhrn** na konci souboru (celková a průměrná spotřeba)

7. **Přesuň obrázek** do správné složky (pokud ještě není):
   - Elektřina → `Meter/images/electricity/`
   - Plyn → `Meter/images/gas/`
   - Voda → `Meter/images/water/`

8. **Potvrď uživateli:**
   - Jakou hodnotu jsi přečetl
   - Kolik činí spotřeba od posledního odečtu
   - Kde je uložen záznam

## Příklad použití:
```
/meter /path/to/photo.jpg
/meter Meter/images/electricity/IMG_20260117.jpg
```
