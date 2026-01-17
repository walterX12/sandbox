# Návod: Automatické odečty měřidel s Claude

## Rychlý postup (Android telefon)

### 1. Vyfoťte měřidlo
- Otevřete fotoaparát
- Vyfoťte displej měřidla (čitelně!)

### 2. Nahrajte fotku
Fotku uložte do složky `Meter/images/` podle typu:
- `electricity/` - elektroměr
- `gas/` - plynoměr
- `water/` - vodoměr

### 3. Požádejte Claude o zpracování

**Příklad promptu:**

```
Přečti hodnotu z měřidla na obrázku Meter/images/electricity/foto.jpg
a přidej nový řádek do tabulky v Meter/meter-readings.md.
Vypočítej spotřebu od posledního odečtu.
```

---

## Claude automaticky:

1. **Rozpozná hodnotu** na měřidle z fotky
2. **Přidá řádek** do správné tabulky
3. **Vypočítá spotřebu** (rozdíl od předchozího odečtu)
4. **Aktualizuje souhrn** spotřeby

---

## Tipy pro kvalitní fotky

- Foťte **kolmo** na displej měřidla
- Zajistěte **dobré osvětlení**
- Celé číslo musí být **ostré a čitelné**
- Pokud má měřidlo více tarifů (NT/VT), foťte oba

---

## Struktura složek

```
Meter/
├── meter-readings.md    # Hlavní tabulka s odečty
├── NAVOD.md             # Tento návod
└── images/
    ├── electricity/     # Fotky elektroměru
    ├── gas/             # Fotky plynoměru
    └── water/           # Fotky vodoměru
```
