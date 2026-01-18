#!/bin/bash
# Skript pro oříznutí a zvětšení části obrázku měřidla
# Použití: ./crop-meter.sh input.jpg [output.jpg] [oblast]
# Oblast: center (výchozí), top, bottom, nebo souřadnice "WxH+X+Y"

INPUT="$1"
OUTPUT="${2:-cropped_$(basename "$1")}"
AREA="${3:-center}"

if [ -z "$INPUT" ]; then
    echo "Použití: $0 input.jpg [output.jpg] [oblast]"
    echo "Oblast: center, top, bottom, nebo souřadnice 'WxH+X+Y'"
    exit 1
fi

if [ ! -f "$INPUT" ]; then
    echo "Soubor '$INPUT' neexistuje!"
    exit 1
fi

# Získat rozměry obrázku
WIDTH=$(identify -format "%w" "$INPUT")
HEIGHT=$(identify -format "%h" "$INPUT")

echo "Originál: ${WIDTH}x${HEIGHT}"

case "$AREA" in
    center)
        # Vyřízne středovou třetinu a zvětší 2x
        CROP_W=$((WIDTH / 3))
        CROP_H=$((HEIGHT / 3))
        CROP_X=$((WIDTH / 3))
        CROP_Y=$((HEIGHT / 3))
        GEOMETRY="${CROP_W}x${CROP_H}+${CROP_X}+${CROP_Y}"
        ;;
    top)
        CROP_W=$((WIDTH))
        CROP_H=$((HEIGHT / 3))
        GEOMETRY="${CROP_W}x${CROP_H}+0+0"
        ;;
    bottom)
        CROP_W=$((WIDTH))
        CROP_H=$((HEIGHT / 3))
        CROP_Y=$((HEIGHT * 2 / 3))
        GEOMETRY="${CROP_W}x${CROP_H}+0+${CROP_Y}"
        ;;
    *)
        # Vlastní souřadnice
        GEOMETRY="$AREA"
        ;;
esac

echo "Ořezávám: $GEOMETRY"

# Oříznout a zvětšit 200%
convert "$INPUT" -crop "$GEOMETRY" +repage -resize 200% "$OUTPUT"

echo "Výstup uložen: $OUTPUT"
echo "Pro zobrazení: termux-open $OUTPUT"
