# -*- coding: utf-8 -*-
"""P23-08 (#1369) — la déclaration @font-face TrendSlab du gabarit Rules est-elle morte ?

CSS lu PARSÉ. Trois relevés :
  1. chaque bloc @font-face : famille déclarée, hôtes, fichiers ;
  2. chaque `font-family:` HORS @font-face : familles effectivement demandées ;
  3. croisement : une famille déclarée et jamais demandée est une déclaration morte
     (dont le simple url() est pourtant récupéré par dom-to-image).

Contrôle inverse : injecter une famille inventée dans les déclarations demandées
et vérifier qu'elle APPARAÎT dans le relevé 2 — un instrument qui ne peut que
confirmer ne vaut rien.

    python face_usage.py <gabarit.json>
"""
import json
import re
import sys
from urllib.parse import urlparse

PATH = sys.argv[1]
FACE = re.compile(r"@font-face\s*\{(.*?)\}", re.S | re.I)
URL = re.compile(r"url\(\s*['\"]?([^'\")]+)['\"]?\s*\)", re.I)
FF = re.compile(r"font-family\s*:\s*([^;}]+)", re.I)


def norm(f):
    return re.sub(r"\s+", " ", f.strip().strip("'\"").strip()).lower()


def main():
    css = json.loads(open(PATH, encoding="utf-8-sig").read())["css"]
    faces = FACE.findall(css)
    rest = FACE.sub("", css)  # le CSS hors @font-face

    print(f"gabarit : {PATH}")
    print(f"css {len(css)} caractères | {len(faces)} blocs @font-face")
    print("\n## 1. Déclarations @font-face")
    declared = {}
    for i, face in enumerate(faces, 1):
        fams = FF.findall(face)
        urls = URL.findall(face)
        hosts = sorted({urlparse(u).netloc or "(relatif)" for u in urls})
        fam = norm(fams[0]) if fams else "?"
        declared.setdefault(fam, []).append(i)
        print(f"  [{i}] famille={fam!r}")
        print(f"      hôtes={hosts}")
        for u in urls:
            print(f"      url  {u[:130]}")

    print("\n## 2. Familles DEMANDÉES hors @font-face")
    used = {}
    for m in FF.finditer(rest):
        for part in m.group(1).split(","):
            used[norm(part)] = used.get(norm(part), 0) + 1
    for fam, n in sorted(used.items(), key=lambda kv: (-kv[1], kv[0])):
        mark = "  <-- déclarée en @font-face" if fam in declared else ""
        print(f"  {n:3}x {fam!r}{mark}")

    print("\n## 3. Croisement")
    for fam in declared:
        n = used.get(fam, 0)
        print(f"  {fam!r} : déclarée {len(declared[fam])}x, demandée {n}x "
              f"=> {'MORTE' if n == 0 else 'VIVANTE'}")
    print(f"  genérique `serif`/`sans-serif` demandée "
          f"{used.get('serif', 0)}x/{used.get('sans-serif', 0)}x")

    # contrôle inverse : une famille inventée doit apparaître dans le relevé 2
    probe = "zz-inverse-control-1485"
    used2 = {}
    for m in FF.finditer(rest + f"\nx{{font-family: {probe}, serif}}"):
        for part in m.group(1).split(","):
            used2[norm(part)] = used2.get(norm(part), 0) + 1
    print(f"\ncontrôle inverse : famille inventée vue {used2.get(probe, 0)}x "
          f"=> {'OK' if used2.get(probe) else 'INSTRUMENT CASSÉ'}")


if __name__ == "__main__":
    main()
