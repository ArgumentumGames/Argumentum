#!/usr/bin/env python3
"""Famille AGR coeur (#458 grain 9-bis) : mesure et liste de travail pour ar-SA / fa-IR / zh-CN.

LECTURE SEULE sur DNNPlatform/App_GlobalResources : le script n'ecrit que le fichier
demande par --write (hors de DNNPlatform).

Usage :
    python -I agr-core-extract.py --report
    python -I agr-core-extract.py --write worklist.json

Regles de travail encodees ici (mesurees le 2026-10-30) :
  * un fichier neutre peut etre une COQUILLE (toutes valeurs vides, ex. List_Country.resx) :
    dans ce cas le contenu vit dans les fichiers culturels et les cles sont a produire
    malgre un neutre vide. Comparer a `neutre non vide` rendrait un faux `0 manquante`.
  * une cle neutre vide dans un fichier NON coquille n'est pas de la matiere a traduire.
  * les placeholders {0}/{1} doivent etre preserves a l'identique (202 cles concernees).

Le script porte un CONTROLE POSITIF : il verifie qu'il sait voir des cles avant de
conclure quoi que ce soit (un instrument muet ne doit pas passer pour un arbre propre).
"""
from __future__ import annotations

import argparse
import json
import os
import re
import sys
import xml.etree.ElementTree as ET

_HERE = os.path.dirname(os.path.abspath(__file__))
_REPO = os.path.abspath(os.path.join(_HERE, "..", "..", ".."))
_WEBNROOT = os.path.join(_REPO, "DNNPlatform")
ROOT = os.environ.get("ARGUMENTUM_AGR_ROOT") or os.path.join(_WEBNROOT, "App_GlobalResources")
FILES = ["SharedResources", "List_Country", "GlobalResources", "Exceptions", "WebControls", "FileUpload"]
LANGS = ["ar-SA", "fa-IR", "zh-CN"]
REF = "fr-FR"
PLACEHOLDER = re.compile(r"\{\d+\}")


def resx_path(base: str, culture: str = "") -> str:
    name = f"{base}.{culture}.resx" if culture else f"{base}.resx"
    return os.path.join(ROOT, name)


def load(path: str) -> dict[str, str]:
    """name -> value ('' si <value> absent ou vide)."""
    out: dict[str, str] = {}
    for node in ET.parse(path).getroot().findall("data"):
        value = node.find("value")
        out[node.get("name")] = value.text if value is not None and value.text is not None else ""
    return out


def positive_control() -> None:
    """Le script doit savoir voir des cles, sinon tout compte rendu est un faux zero."""
    shared = resx_path("SharedResources")
    if not os.path.exists(shared):
        sys.exit(f"CONTROLE POSITIF ECHOUE : {shared} introuvable — instrument aveugle, arret.")
    keys = load(shared)
    if len(keys) < 100:
        sys.exit(f"CONTROLE POSITIF ECHOUE : {len(keys)} cles lues dans SharedResources (attendu >100) — arret.")
    if "Calendar.Text" not in keys:
        sys.exit("CONTROLE POSITIF ECHOUE : 'Calendar.Text' absent — mauvais fichier ou mauvais decodage.")


def build():
    positive_control()
    counts: dict[str, dict] = {}
    work: dict[str, dict[str, list]] = {lang: {} for lang in LANGS}

    for base in FILES:
        neutral_path = resx_path(base)
        if not os.path.exists(neutral_path):
            counts[base] = {"error": "neutre absent"}
            continue
        neutral = load(neutral_path)
        ref = load(resx_path(base, REF)) if os.path.exists(resx_path(base, REF)) else {}
        is_shell = bool(neutral) and all(not v.strip() for v in neutral.values())

        counts[base] = {
            "neutral_keys": len(neutral),
            "neutral_nonempty": sum(1 for v in neutral.values() if v.strip()),
            "neutral_is_shell": is_shell,
        }

        for lang in LANGS:
            target_path = resx_path(base, lang)
            target = load(target_path) if os.path.exists(target_path) else {}
            items = []
            for key, value in neutral.items():
                already = target.get(key, "")
                if already.strip():
                    continue
                # coquille : le neutre est vide par construction -> la cle est a produire
                if not is_shell and not value.strip():
                    continue
                items.append({
                    "key": key,
                    "en": value,
                    "fr": ref.get(key, ""),
                    "placeholder_count": len(PLACEHOLDER.findall(value)),
                })
            work[lang][base] = items
            counts[base][lang] = {"file_present": os.path.exists(target_path), "to_produce": len(items)}

    totals = {lang: sum(len(items) for items in work[lang].values()) for lang in LANGS}
    return counts, work, totals


def main() -> int:
    parser = argparse.ArgumentParser(description="Mesure de la famille AGR coeur (#458 grain 9-bis)")
    parser.add_argument("--report", action="store_true", help="affiche les comptes par fichier et par langue")
    parser.add_argument("--write", metavar="PATH", help="ecrit la liste de travail JSON a cet emplacement")
    args = parser.parse_args()

    if not args.report and not args.write:
        parser.print_help()
        return 2

    counts, work, totals = build()

    if args.report:
        print(f"{'fichier':<18}{'cles':>6}{'nonvide':>9}{'coquille':>10}   "
              + " ".join(f"{lang[:2]:>6}" for lang in LANGS))
        for base, row in counts.items():
            if "error" in row:
                print(f"{base:<18} {row['error']}")
                continue
            line = (f"{base:<18}{row['neutral_keys']:>6}{row['neutral_nonempty']:>9}"
                    f"{str(row['neutral_is_shell']):>10}   ")
            for lang in LANGS:
                cell = row[lang]
                line += f"{cell['to_produce']:>6}" if cell["file_present"] else f"{'ABS':>6}"
            print(line)
        for lang in LANGS:
            print(f"TOTAL {lang} = {totals[lang]} cles a produire")
        all_keys = sum(totals.values())
        with_ph = sum(1 for lang in LANGS for items in work[lang].values()
                      for item in items if item["placeholder_count"])
        print(f"cles portant un placeholder sur les 3 langues : {with_ph} / {all_keys}")

    if args.write:
        destination = os.path.abspath(args.write)
        # DNNPlatform/ est le webroot SERVI : aucune ecriture dedans, meme un fichier inoffensif.
        if destination.lower().startswith(_WEBNROOT.lower() + os.sep):
            sys.exit(f"REFUS : {destination} est sous DNNPlatform/ (webroot servi) — ce script est en lecture seule sur ce dossier.")
        with open(destination, "w", encoding="utf-8") as handle:
            json.dump({"totals": totals, "counts": counts, "work": work}, handle,
                      ensure_ascii=False, indent=1)
        print(f"liste de travail -> {destination}")

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
