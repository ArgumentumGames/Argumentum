#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1499 grain 5 — MESURE du registre d'adresse de desc_es (⛔ 0 écriture).

175 cartes du deck (colonne `carte` non vide) :
  classe desc_es en T (tú) / V (usted) / I (impersonnel), puis ventile
  l'adresse en 4 formes (tú · vosotros · usted sg · ustedes pl) et croise
  avec desc_fr (référence).

⭐ Instrument : les marqueurs V sont limités à « usted(es) » EXPLICITE —
  « le/su » + verbe 3e pers. est indissociable de l'impersonnel en espagnol,
  donc une part des I peut adresser poliment le lecteur sans le dire :
  le chiffre I est un PLAFOND d'impersonnel, pas une borne exacte.

Usage :
    python tools/1499-g9es-registre-defs.py               # tables
    python tools/1499-g9es-registre-defs.py --dump        # les 61 cartes adressantes
    python tools/1499-g9es-registre-defs.py --self-test
"""
import argparse
import csv
import io
import json
import os
import re
import sys

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_REL = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"

ES_T = re.compile(r"\btú\b|\btu\b|\bte\b|\bti\b|\bcontigo\b|\btuy[oa]s?\b|\bvosotr[oa]s?\b|\bos\b", re.I)
ES_V = re.compile(r"\busted(es)?\b", re.I)
ES_VOS = re.compile(r"\bos\b|\bsois\b|\bvosotr[oa]s?\b|áis|\béis\b|\bís\b(?= \b)", re.I)
FR_T = re.compile(r"\btu\b|\bte\b|\bt'\b|\btoi\b|\bton\b|\bta\b|\btiens\b", re.I)
FR_V = re.compile(r"\bvous\b|\bvotre\b|\bvos\b", re.I)


def classe(text, t_re, v_re):
    a, b = bool(t_re.search(text)), bool(v_re.search(text))
    return "M" if a and b else "T" if a else "V" if b else "I"


def forme_es(desc):
    """Ventile l'adresse es : vosotros / ustedes / usted / tú (singulier)."""
    m = ES_V.search(desc)
    if m and m.group(0).lower().startswith("ustedes"):
        return "ustedes"
    if m:
        return "usted"
    if ES_VOS.search(desc):
        return "vosotros"
    return "tú"


def deck_rows():
    with open(os.path.join(REPO, CSV_REL), "rb") as fh:
        rows = list(csv.DictReader(io.StringIO(fh.read().decode("utf-8-sig"))))
    return [r for r in rows if (r.get("carte") or "").strip()]


def run(dump=False):
    rows = deck_rows()
    par_classe, par_forme, par_famille, croise = {}, {}, {}, {}
    adressantes = []
    for r in rows:
        es = (r["desc_es"] or "").strip()
        fr = (r["desc_fr"] or "").strip()
        c = classe(es, ES_T, ES_V)
        par_classe[c] = par_classe.get(c, 0) + 1
        croise.setdefault(classe(fr, FR_T, FR_V), {}).setdefault(c, 0)
        croise[classe(fr, FR_T, FR_V)][c] += 1
        if c in ("T", "V"):
            f = forme_es(es)
            par_forme[f] = par_forme.get(f, 0) + 1
            fam = r["Famille"].strip()
            par_famille.setdefault(fam, {}).setdefault(f, 0)
            par_famille[fam][f] += 1
            adressantes.append({"pk": r["PK"].strip(), "forme": f, "famille": fam,
                                "desc_es": es[:180]})
    print(f"deck : {len(rows)} cartes")
    print("desc_es par classe :", dict(sorted(par_classe.items())))
    print("adresse es par forme :", dict(sorted(par_forme.items())))
    print("croisement desc_fr -> desc_es :", json.dumps(croise, ensure_ascii=False))
    for fam in sorted(par_famille):
        print(f"  {fam:30s} {par_famille[fam]}")
    if dump:
        for x in adressantes:
            print(f"PK {x['pk']} [{x['forme']}] {x['desc_es']}")
    return 0


def self_test():
    cases = [
        ("Manipulas a tu audiencia para persuadir.", "T"),
        ("Ustedes fundamentan sus argumentos en ideas.", "V"),
        ("Os presentáis como una víctima.", "T"),
        ("Falacia que consiste en desviar el debate.", "I"),
        ("Usted considera que un comportamiento está justo.", "V"),
        ("A veces te expresas de manera vaga.", "T"),
    ]
    ok = all(classe(t, ES_T, ES_V) == c for t, c in cases)
    formes = [
        ("Os presentáis como una víctima.", "vosotros"),
        ("Ustedes fundamentan sus argumentos.", "ustedes"),
        ("Usted considera que está justo.", "usted"),
        ("Manipulas a tu audiencia.", "tú"),
    ]
    ok &= all(forme_es(t) == f for t, f in formes)
    print("SELF-TEST " + ("OK" if ok else "NON PROUVÉ"))
    return 0 if ok else 1


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--dump", action="store_true")
    ap.add_argument("--self-test", action="store_true")
    args = ap.parse_args()
    if args.self_test:
        return self_test()
    return run(args.dump)


if __name__ == "__main__":
    sys.exit(main())
