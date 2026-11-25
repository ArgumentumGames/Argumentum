#!/usr/bin/env python3
"""Construit les polices TrueType minimales embarquées par la sonde de polices (#1485).

La sonde de repli (`FontFallbackDetector`, chemin de récolte) doit dire, lettre par lettre,
QUELLE famille d'une pile `font-family` rend le caractère. `document.fonts.check()` ne le dit
pas (il ne teste que les polices de sa liste), et comparer la largeur d'un texte rendu par
`<famille>` à celle des génériques ne le dit pas davantage : un caractère absent passe à la
police de repli du système, dont le choix dépend de la famille générique de la primaire —
deux piles différentes peuvent donc rendre la même largeur pour la mauvaise raison.

La parade est une police SENTINELLE : elle couvre tout Unicode avec une chasse absurde
(3 em). Pour un caractère `c` et une famille `F` :

    largeur("F, Sentinel", c) == largeur("Sentinel", c)   <=>  F ne rend pas c

Quatre polices, toutes déterministes (horodatages fixés, tables triées) :

* `ArgumentumProbeSentinel`  — tout U+0000..U+10FFFF, chasse 3000/1000 em (la sentinelle) ;
* `ArgumentumProbeLatin`     — U+0020..U+007E, chasse 500/1000 em : « la famille est chargée
  et ne rend pas le cyrillique », le cas Barlow de #1485, sans réseau, sur tout système ;
* `ArgumentumProbeCyrCaps`   — U+0410..U+042F seulement : verdict « partiel » ;
* `ArgumentumProbeCyrLower`  — U+0430..U+044F seulement : avec la précédente dans la même
  pile, verdict « partagé » ; seule avec `ArgumentumProbeLatin`, le dessin (a) de #1485
  (une famille latine + une compagne cyrillique).

Les trois dernières ne servent qu'aux tests. Aucun glyphe n'est peint : le contour du glyphe
unique n'existe que parce que le sanitiseur de Chromium refuse une table `glyf` vide. La
sonde ne mesure que des chasses (`measureText().width`).

Usage :

    python -I make-probe-fonts.py                 # affiche les quatre base64
    python -I make-probe-fonts.py --csharp        # constantes C# prêtes à recopier
    python -I make-probe-fonts.py --check         # vérifie les constantes du code C#
    python -I make-probe-fonts.py --out <dossier> # écrit les .ttf et les .b64

stdlib uniquement.
"""

import base64
import re
import struct
import sys
from pathlib import Path

UPEM = 1000

# (nom dans la table name, chasse, plages cmap)
FONTS = {
    "sentinel": ("ArgumentumProbeSentinel", 3000, [(0x0000, 0x10FFFF)]),
    "latin": ("ArgumentumProbeLatin", 500, [(0x0020, 0x007E)]),
    "cyr_caps": ("ArgumentumProbeCyrCaps", 500, [(0x0410, 0x042F)]),
    "cyr_lower": ("ArgumentumProbeCyrLower", 500, [(0x0430, 0x044F)]),
}

DETECTOR_CS = ("Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/"
               "FontFallbackDetector.cs")
BINDING_TESTS_CS = ("Generation/Converters/Argumentum.AssetConverter.Tests/WebBasedGenerator/"
                    "FontFallbackDetectorBindingTests.cs")

# Constantes C# : (fichier depuis la racine du dépôt, nom du champ, police).
ANCHORS = (
    (DETECTOR_CS, "SentinelFontBase64", "sentinel"),
    (BINDING_TESTS_CS, "LatinOnlyFontBase64", "latin"),
    (BINDING_TESTS_CS, "CyrillicCapsFontBase64", "cyr_caps"),
    (BINDING_TESTS_CS, "CyrillicLowerFontBase64", "cyr_lower"),
)

CHUNK = 112  # caractères base64 par littéral C#


def _u16(value):
    return struct.pack(">H", value)


def _u32(value):
    return struct.pack(">I", value)


def _head():
    return (
        _u32(0x00010000)
        + _u32(0x00010000)
        + _u32(0)
        + _u32(0x5F0F3CF5)
        + _u16(0x0003)
        + _u16(UPEM)
        + _u32(0) * 4  # created, modified (int64 chacun, horodatage fixé = reproductible)
        + _u16(0) + _u16(0) + _u16(0) + _u16(0)
        + _u16(0)
        + _u16(8)
        + _u16(2)
        + _u16(0)  # indexToLocFormat : court
        + _u16(0)
    )


def _hhea(num_glyphs, advance):
    return (
        _u32(0x00010000)
        + _u16(800) + _u16(-200 & 0xFFFF) + _u16(0)
        + _u16(advance)
        + _u16(0) + _u16(0) + _u16(0)
        + _u16(1) + _u16(0) + _u16(0)
        + _u16(0) * 4
        + _u16(0)
        + _u16(num_glyphs)
    )


def _maxp(num_glyphs):
    return (
        _u32(0x00010000)
        + _u16(num_glyphs)
        + _u16(4) + _u16(1) + _u16(0) + _u16(0)   # maxPoints, maxContours
        + _u16(2)
        + _u16(0) * 8
    )


def _hmtx(num_glyphs, advance):
    return (_u16(advance) + _u16(0)) * num_glyphs


def _cmap(ranges):
    """Un seul sous-tableau (plateforme 3, encodage 10), format 13 (« last resort ») : toute
    une plage de caractères rend le MÊME glyphe. Le format 12, qui a la même disposition,
    associe une plage de caractères à une plage de glyphes consécutifs — inutilisable avec un
    seul glyphe (refusé par le sanitiseur de Chromium, mesuré 25/11/2026)."""
    subtable = _u16(13) + _u16(0) + _u32(16 + 12 * len(ranges)) + _u32(0) + _u32(len(ranges))
    for start, end in ranges:
        subtable += _u32(start) + _u32(end) + _u32(1)
    return _u16(0) + _u16(1) + _u16(3) + _u16(10) + _u32(12) + subtable


def _glyf():
    """Glyphe 0 (.notdef) vide, glyphe 1 = un rectangle. Chromium passe chaque police web par
    son sanitiseur OTS, qui refuse une table `glyf` de longueur nulle (mesuré 25/11/2026 :
    « OTS parsing error: glyf: zero-length table ») : le contour n'existe que pour être
    accepté, la sonde ne lit que des chasses."""
    rect = (
        _u16(1)                      # numberOfContours
        + _u16(0) + _u16(0) + _u16(400) + _u16(700)
        + _u16(3)                    # endPtsOfContours
        + _u16(0)                    # instructionLength
        + bytes([0x01, 0x01, 0x01, 0x01])   # sur courbe, deltas sur 16 bits
        + _u16(400) + _u16(0) + _u16(-400 & 0xFFFF) + _u16(0)
        + _u16(700) + _u16(0) + _u16(-700 & 0xFFFF) + _u16(0)
    )
    return rect, len(rect)


def _name(family):
    names = {1: family, 2: "Regular", 3: f"{family}; 1.0", 4: family, 5: "Version 1.0",
             6: family}
    strings = b""
    records = b""
    for name_id, text in names.items():
        data = text.encode("utf-16-be")
        records += _u16(3) + _u16(1) + _u16(0x0409) + _u16(name_id) + _u16(len(data)) \
            + _u16(len(strings))
        strings += data
    return _u16(0) + _u16(len(names)) + _u16(6 + 12 * len(names)) + records + strings


def _post():
    return (
        _u32(0x00030000)
        + _u32(0)  # italicAngle (Fixed 16.16)
        + _u16(-100 & 0xFFFF)
        + _u16(50)
        + _u32(0) * 5
    )


def _os2(first, last):
    return (
        _u16(4)
        + _u16(500)
        + _u16(400)
        + _u16(5)
        + _u16(0)
        + _u16(650) + _u16(600) + _u16(-75 & 0xFFFF) + _u16(350)
        + _u16(650) + _u16(600) + _u16(-75 & 0xFFFF) + _u16(350)
        + _u16(50) + _u16(250)
        + _u16(0)
        + bytes(10)
        + _u32(0) * 4
        + b"NONE"
        + _u16(0x40)
        + _u16(first) + _u16(last)
        + _u16(800) + _u16(-200 & 0xFFFF) + _u16(0)
        + _u16(800) + _u16(200)
        + _u32(1) + _u32(0)
        + _u16(500) + _u16(700)
        + _u16(0) + _u16(32) + _u16(0)
    )


def build(family, advance, ranges):
    """Assemble la police : .notdef vide et un glyphe rectangulaire, chasse `advance`."""
    num_glyphs = 2
    glyph_data, glyph_length = _glyf()
    tables = {
        b"OS/2": _os2(min(s for s, _ in ranges), min(0xFFFF, max(e for _, e in ranges))),
        b"cmap": _cmap(ranges),
        b"glyf": glyph_data,
        b"head": _head(),
        b"hhea": _hhea(num_glyphs, advance),
        b"hmtx": _hmtx(num_glyphs, advance),
        b"loca": _u16(0) + _u16(0) + _u16(glyph_length // 2),
        b"maxp": _maxp(num_glyphs),
        b"name": _name(family),
        b"post": _post(),
    }
    tags = sorted(tables)
    num = len(tags)
    entry_selector = max(num.bit_length() - 1, 0)
    search_range = (1 << entry_selector) * 16
    directory = _u32(0x00010000) + _u16(num) + _u16(search_range) + _u16(entry_selector) \
        + _u16(num * 16 - search_range)

    offset = 12 + 16 * num
    records = b""
    body = b""
    data_offsets = {}
    for tag in tags:
        data = tables[tag]
        padded = data + bytes(-len(data) % 4)
        # Somme calculée checkSumAdjustment à zéro, comme l'exige le répertoire.
        checksum = sum(struct.unpack(f">{len(padded) // 4}I", padded)) & 0xFFFFFFFF
        records += tag + _u32(checksum) + _u32(offset) + _u32(len(data))
        data_offsets[tag] = offset
        body += padded
        offset += len(padded)

    font = bytearray(directory + records + body)
    # checkSumAdjustment : la somme du fichier doit valoir 0xB1B0AFBA (champ à l'offset 8
    # de la table head).
    adjustment_pos = data_offsets[b"head"] + 8
    total = sum(struct.unpack(f">{len(font) // 4}I", font)) & 0xFFFFFFFF
    font[adjustment_pos:adjustment_pos + 4] = _u32((0xB1B0AFBA - total) & 0xFFFFFFFF)
    total = sum(struct.unpack(f">{len(font) // 4}I", font)) & 0xFFFFFFFF
    assert total == 0xB1B0AFBA, hex(total)
    return bytes(font)


def fonts():
    """police -> (famille, octets, base64)."""
    out = {}
    for key, (family, advance, ranges) in FONTS.items():
        data = build(family, advance, ranges)
        out[key] = (family, data, base64.b64encode(data).decode("ascii"))
    return out


def _const(field, b64):
    """Déclaration C# prête à recopier : littéraux de CHUNK caractères joints par `+`."""
    chunks = [b64[i:i + CHUNK] for i in range(0, len(b64), CHUNK)]
    return f"\tprivate const string {field} =\n" + "\n".join(
        f'\t\t"{chunk}"' + (" +" if i < len(chunks) - 1 else ";")
        for i, chunk in enumerate(chunks)) + "\n"


def _embedded(field, text):
    """Valeur d'une constante C# composée de littéraux : `"a" + "b" ;`."""
    match = re.search(rf"\b{field}\s*=\s*([^;]+);", text)
    if not match:
        return None
    return "".join(re.findall(r'"([^"]*)"', match.group(1)))


def _check(repo_root):
    """Les constantes du code doivent être exactement la sortie du constructeur."""
    ok = True
    texts = {}
    built = fonts()
    for rel, field, key in ANCHORS:
        path = Path(repo_root) / rel
        if rel not in texts:
            texts[rel] = path.read_text(encoding="utf-8") if path.exists() else None
        found = None if texts[rel] is None else _embedded(field, texts[rel])
        if found == built[key][2]:
            print(f"OK   {rel} :: {field}")
        else:
            ok = False
            print(f"DIFF {rel} :: {field} — relancer avec --csharp et recopier "
                  f"(trouvé : {'absent' if found is None else str(len(found)) + ' caractères'}, "
                  f"attendu : {len(built[key][2])})")
    return 0 if ok else 1


def main(argv):
    if len(argv) >= 2 and argv[1] == "--check":
        repo_root = argv[2] if len(argv) > 2 else str(Path(__file__).resolve().parents[2])
        return _check(repo_root)
    built = fonts()
    if len(argv) >= 3 and argv[1] == "--out":
        out = Path(argv[2])
        out.mkdir(parents=True, exist_ok=True)
        for key, (family, data, b64) in built.items():
            (out / f"{key}.ttf").write_bytes(data)
            (out / f"{key}.b64").write_text(b64 + "\n", encoding="ascii")
            print(f"{out / (key + '.ttf')} {family} {len(data)} octets, base64 {len(b64)}")
        return 0
    if len(argv) >= 2 and argv[1] == "--csharp":
        for rel, field, key in ANCHORS:
            print(f"// {rel}")
            print(_const(field, built[key][2]))
        return 0
    for key, (family, data, b64) in built.items():
        print(f"# {family} — {len(data)} octets")
        print(b64)
    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv))
