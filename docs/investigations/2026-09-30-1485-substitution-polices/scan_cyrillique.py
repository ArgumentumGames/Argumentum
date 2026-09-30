"""Ou y a-t-il des lettres cyrilliques dans des colonnes qui n'en veulent pas ?

Complement de `mesurer_substitution.py couverture`. La couverture dit quels
caracteres du corpus peint une police candidate ne sait pas rendre ; ce script
dit, parmi ceux-la, lesquels sont des COQUILLES dans les donnees plutot que des
besoins legitimes.

Le defaut cherche est une lettre cyrillique dans une colonne de langue latine
(fr/en/es/pt), ou elle se fait passer pour son sosie latin. Le glyphe etant
identique, la coquille est invisible a la lecture ET a un `grep` du mot : le mot
« conclusion » ecrit avec un `с` U+0441 ne matche pas la recherche « conclusion ».

Les colonnes de langue russe (title_ru, desc_ru, ...) sont legitimement en
cyrillique : elles sont hors du filtre.

Usage :
    python scan_cyrillique.py <gabarit.json>
    python scan_cyrillique.py Cards/Fallacies/Argumentum_Fallacies_Face_fr.json

Le script se termine par un code 1 si au moins une occurrence est trouvee, pour
servir de garde dans un balayage `Cards/*/*.json`.
"""

import csv
import io
import json
import sys
from collections import defaultdict
from pathlib import Path

# colonnes dont la langue d'ecriture est latine -> le cyrillique y est une coquille
LATINES = ("_fr", "_en", "_es", "_pt")

# cyrillique visuellement confondable avec une lettre latine. Ce n'est pas "tout le
# cyrillique" : seuls les caracteres dont le glyphe se confond rendent la coquille
# invisible, et c'est cette invisibilite qui fait le defaut.
CONFONDABLES = {
    "а": "a", "е": "e", "о": "o", "р": "p", "с": "c",
    "х": "x", "у": "y", "к": "k", "м": "m", "т": "t",
    "н": "h", "в": "b", "і": "i", "ѕ": "s", "ј": "j",
    "А": "A", "В": "B", "Е": "E", "К": "K", "М": "M",
    "Н": "H", "О": "O", "Р": "P", "С": "C", "Т": "T",
    "Х": "X", "У": "Y",
}


def scanner(gabarit: Path):
    payload = json.loads(gabarit.read_text(encoding="utf-8-sig"))
    rdr = list(csv.reader(io.StringIO(payload.get("csv", ""))))
    if not rdr:
        return [], []
    entete, corps = rdr[0], rdr[1:]
    idx = {h: i for i, h in enumerate(entete)}

    cibles = [h for h in entete if h.endswith(LATINES)]
    trouves = defaultdict(list)
    for r in corps:
        pk = r[0] if r else "?"
        for col in cibles:
            i = idx.get(col)
            if i is None or i >= len(r):
                continue
            val = r[i]
            for pos, ch in enumerate(val):
                if ch in CONFONDABLES:
                    trouves[col].append((pk, pos, ch, CONFONDABLES[ch], val))
    return cibles, trouves


def main():
    if len(sys.argv) != 2:
        raise SystemExit(__doc__)
    gabarit = Path(sys.argv[1])
    cibles, trouves = scanner(gabarit)

    total = sum(len(v) for v in trouves.values())
    print(f"{gabarit} : {len(cibles)} colonnes latines, {total} occurrence(s)")
    for col in sorted(trouves):
        for pk, pos, ch, lat, val in trouves[col]:
            debut, fin = max(0, pos - 30), min(len(val), pos + 30)
            ctx = val[debut:fin].replace("\n", " ")
            print(f"  {col}  PK={pk:<6} pos={pos:<5} U+{ord(ch):04X} "
                  f"{ch!r} (sosie latin {lat!r})")
            print(f"      ...{ctx}...")

    sys.exit(1 if total else 0)


if __name__ == "__main__":
    main()