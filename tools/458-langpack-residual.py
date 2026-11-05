#!/usr/bin/env python3
"""Résidu des langpacks #458 — familles resx sans jumeau localisé, par culture.

Une famille = un fichier neutre `Foo.resx` ; elle est « suivie » pour une culture quand
`Foo.<culture>.resx` existe dans le dépôt. Le résidu = les familles qui n'ont PAS de
jumeau : ce qu'un pack construit au HEAD courant **ne peut pas** couvrir.

⚠️ Deux objets que ce script ne confond pas :
  · le résidu **du dépôt** (mesuré ici) — ce qui manque à master ;
  · le résidu **servi** — le site de test sert aussi des **packs amont installés le
    12/10** (absents du dépôt) : une famille manquante ici peut être couverte là-bas.
    Seul `tools/458-langpack-build.py` + l'installation tranchent le servi.

Contrôle croisé : pour es-ES, le compte « suivis » ici (192 au 05/11) égalait le
« master resx: 192 » du build #1915 — les deux instruments comptent le même objet.

Usage :  python tools/458-langpack-residual.py [racine-du-dépôt]
⛔ Lecture seule.
"""

from __future__ import annotations

import glob
import os
import re
import sys

CULTURES = ["fr-FR", "es-ES", "pt-BR", "ru-RU", "zh-CN", "ar-SA", "fa-IR"]
# en-US n'est PAS une culture de pack : il n'existe quasiment aucun *.en-US.resx —
# le NEUTRE est le texte anglais (mesuré 05/11 : 3 suivis / 213 familles). On le
# liste séparément pour le dire, pas pour le construire.

RX_CULT = re.compile(r"\.([a-z]{2}(?:-[A-Za-z2-4]{2,4})?)\.resx$")


def key_count(path: str) -> int:
    """Compte grossier des <data name=…> — suffisant pour dimensionner, pas pour
    traduire (inclut quelques attributs name= hors data ; même biais partout)."""
    try:
        with open(path, encoding="utf-8-sig", errors="replace") as fh:
            return fh.read().count('name="')
    except OSError:
        return 0


def main() -> int:
    root = sys.argv[1] if len(sys.argv) > 1 else "."
    neutrals: list[str] = []
    followed: dict[str, dict[str, str]] = {c: {} for c in CULTURES}
    for path in glob.glob(os.path.join(root, "DNNPlatform", "**", "*.resx"), recursive=True):
        norm = path.replace(os.sep, "/")
        if "/bin/" in norm or "/obj/" in norm:
            continue
        m = RX_CULT.search(norm)
        if m and m.group(1) in followed:
            c = m.group(1)
            followed[c][norm[: norm.rfind(f".{c}.resx")]] = path
        elif not m:
            neutrals.append(path)

    print(f"Familles neutres : {len(neutrals)}")
    print()
    print("| Culture | Familles suivies | Résidu fichiers | Résidu entrées | Plus gros manque |")
    print("|---|---|---|---|---|")
    for c in CULTURES:
        missing = [n for n in neutrals if n[:-5] not in followed[c]]
        entries = sum(key_count(n) for n in missing)
        dirs: dict[str, int] = {}
        for n in missing:
            d = os.path.dirname(n).replace(os.sep, "/").rsplit("/", 1)[-1]
            dirs[d] = dirs.get(d, 0) + 1
        top = ", ".join(f"{d} ×{k}" for d, k in sorted(dirs.items(), key=lambda x: -x[1])[:2]) or "—"
        print(f"| {c} | {len(followed[c])} | {len(missing)} | ~{entries} | {top} |")
    print()
    print("(`name=\"` compte quelques attributs hors <data> : même biais sur chaque ligne —")
    print("à comparer entre cultures, pas à additionner avec un compte exact de clés.)")
    return 0


if __name__ == "__main__":
    sys.exit(main())
