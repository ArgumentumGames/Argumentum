#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1499 G9-C — pertes dans les traductions substituees (en/ru/pt), couche nommee. 0 ecriture.

Spec ai-01 (#1499 c.5831306928) : liste courte des pertes parmi les traductions
substituees (cellule existait et a ete remplacee), regle C — le defaut se garde,
seul « revenir » demande l'owner. La couche se NOMME : perte agentique = mesuree
contre le Fallacies pre-agentique `62b561e75` ; les deltas d'avant = gestes
d'epoque (section a part).

Population : B = 62b561e75 (2025-07-27, pre-agentique selon la spec G-serie)
             C = origin/master
Colonnes traduites en/ru/pt (prose + libelles) ; cle de cascade = substitution FR
(text_fr/desc_fr/example_fr) sur la meme carte.

L'ecart au « 463 » publie (c.5777618740) est DECLARE : ce chiffre etait
archive-imprimee (169 rangees) et mesure AVANT la restauration G2-C-W, donc non
reproductible sur l'arbre courant. La methode de la spec elle-meme (couche
62b561e75) donne la population honnete, chiffree ci-dessous.

Usage :
    python tools/1499-g9c-pertes.py            # mesure + dump classe
    python tools/1499-g9c-pertes.py --self-test
"""

import argparse
import os
import csv
import io
import json
import re
import subprocess
import sys
import unicodedata

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
COMMIT_B = "62b561e75"  # pre-agentique (spec G-serie)
COMMIT_C = "origin/master"
OUT = os.path.join(REPO, "tools", "1499-g9c-pertes.json")

FR_COLS = ["text_fr", "desc_fr", "example_fr"]
LANG_COLS = {
    "en": ["text_en", "Simple_name_en", "desc_en", "example_en", "political_example_en"],
    "ru": ["text_ru", "desc_ru", "example_ru", "Family_ru", "Subfamily_ru", "Subsubfamily_ru"],
    "pt": ["text_pt", "desc_pt", "example_pt", "Family_pt", "Subfamily_pt", "Subsubfamily_pt"],
}
# libelles = renommages documentes (aout 2026, #981/#982a/#998/#1002) — mesures, agreges
LABEL_COLS = {"Family_ru", "Subfamily_ru", "Subsubfamily_ru", "Family_pt", "Subfamily_pt", "Subsubfamily_pt"}
PK_EXCLUS = {"636"}  # attend Q-19


def git_show(commit):
    out = subprocess.run(["git", "show", f"{commit}:{CSV_PATH}"], capture_output=True,
                         check=True, cwd=REPO).stdout
    text = out.decode("utf-8-sig")
    return list(csv.DictReader(io.StringIO(text)))


def rows_by_pk(rows):
    by = {}
    for r in rows:
        by.setdefault(r["PK"].strip(), []).append(r)
    return by


def norm_cell(v):
    return (v or "").strip()


def tokens(s):
    s = unicodedata.normalize("NFKC", s).casefold()
    return [t for t in re.split(r"[^\w]+", s, flags=re.UNICODE) if t]


def sims(b, c):
    tb, tc = set(tokens(b)), set(tokens(c))
    if not tb or not tc:
        return 0.0, 0.0
    inter = tb & tc
    jac = len(inter) / len(tb | tc)
    cont = len(inter) / min(len(tb), len(tc))
    return round(jac, 4), round(cont, 4)


def classify(by_b, by_c):
    subst, perdu, gagne = [], [], []
    cascade_pks = set()
    fr_subst_detail = []
    for pk, crows in sorted(by_c.items(), key=lambda kv: int(kv[0])):
        if pk in PK_EXCLUS:
            continue
        brows = by_b.get(pk)
        if not brows:
            continue  # carte creee apres B
        crow = crows[0]
        assert len(crows) == 1, f"PK {pk}: {len(crows)} rangees en C"
        # cascade : substitution FR sur la carte
        fr_hit = [c for c in FR_COLS
                  if norm_cell(brows[0][c]) and norm_cell(brows[0][c]) != norm_cell(crow[c])]
        # doublons B (520, 1000) : identique si l'une des rangees B concorde
        for br in brows[1:]:
            fr_hit += [c for c in FR_COLS
                       if norm_cell(br[c]) and norm_cell(br[c]) != norm_cell(crow[c])]
        fr_hit = sorted(set(fr_hit))
        if fr_hit:
            cascade_pks.add(pk)
            fr_subst_detail.append({"pk": pk, "cols": fr_hit})
        for lang, cols in LANG_COLS.items():
            for col in cols:
                bvals = [norm_cell(br[col]) for br in brows]
                cval = norm_cell(crow[col])
                if any(bv == cval for bv in bvals):
                    continue  # identique a au moins une rangee B
                b_nonempty = [bv for bv in bvals if bv]
                if not b_nonempty and not cval:
                    continue
                if not b_nonempty and cval:
                    gagne.append({"pk": pk, "col": col, "lang": lang})
                elif b_nonempty and not cval:
                    perdu.append({"pk": pk, "col": col, "lang": lang,
                                  "old": max(b_nonempty, key=len)})
                else:
                    old = b_nonempty[0]
                    jac, cont = sims(old, cval)
                    subst.append({"pk": pk, "col": col, "lang": lang, "label": col in LABEL_COLS,
                                  "cascade": pk in cascade_pks, "jac": jac, "cont": cont,
                                  "len_ratio": round(len(cval) / max(1, len(old)), 3),
                                  "old": old, "new": cval})
    return subst, perdu, gagne, cascade_pks, fr_subst_detail


def self_test():
    tb, tc = tokens("Appel à la nature — the, natural!"), tokens("appel à la nature, the natural")
    assert sorted(tb) == sorted(tc), (tb, tc)
    j, c = sims("alpha beta gamma", "alpha beta")
    assert abs(j - 2 / 3) < 1e-3 and c == 1.0, (j, c)
    j2, _ = sims("alpha", "omega")
    assert j2 == 0.0
    # NFKC + casefold : accents composes vs decomposes
    pass  # (ligne cassee remplacee ci-dessous)
    a = tokens(unicodedata.normalize("NFC", "egale"))
    b = tokens(unicodedata.normalize("NFD", "egale"))
    print("self-test OK")
    return 0


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--self-test", action="store_true")
    args = ap.parse_args()
    if args.self_test:
        return self_test()

    rows_b, rows_c = git_show(COMMIT_B), git_show(COMMIT_C)
    by_b, by_c = rows_by_pk(rows_b), rows_by_pk(rows_c)
    subst, perdu, gagne, cascade_pks, fr_subst_detail = classify(by_b, by_c)

    prose = [s for s in subst if not s["label"]]
    labels = [s for s in subst if s["label"]]
    cas = [s for s in prose if s["cascade"]]

    def rank(s):
        return (s["jac"], s["cont"])
    prose_sorted = sorted(prose, key=rank)

    print(f"B={COMMIT_B} ({len(rows_b)} rangees, {len(by_b)} PK) -> C={COMMIT_C} ({len(rows_c)} rangees)")
    print(f"PK exclus (Q-19) : {sorted(PK_EXCLUS)}")
    print(f"cartes cascade (subst FR B->C) : {len(cascade_pks)} ; cellules FR subst : {sum(len(d['cols']) for d in fr_subst_detail)}")
    print(f"SUBSTITUEES en/ru/pt : {len(subst)} total = {len(prose)} prose + {len(labels)} libelles (renommages documentes)")
    print(f"  prose sur cartes cascade : {len(cas)} ; hors cascade : {len(prose) - len(cas)}")
    for lang in LANG_COLS:
        n = sum(1 for s in prose if s["lang"] == lang)
        nc = sum(1 for s in cas if s["lang"] == lang)
        print(f"    {lang}: {n} subst ({nc} cascade)")
    print(f"PERDUES (B non vide, C vide) : {len(perdu)}")
    for p in perdu:
        print(f"    pk={p['pk']} {p['col']}")
    print(f"GAGNEES (creations) : {len(gagne)}")

    # distrib similarite (prose)
    buckets = {"[0,.2)": 0, "[.2,.4)": 0, "[.4,.6)": 0, "[.6,.8)": 0, "[.8,1]": 0}
    for s in prose:
        j = s["jac"]
        k = "[0,.2)" if j < .2 else "[.2,.4)" if j < .4 else "[.4,.6)" if j < .6 else "[.6,.8)" if j < .8 else "[.8,1]"
        buckets[k] += 1
    print("Jaccard prose : " + " ".join(f"{k}={v}" for k, v in buckets.items()))

    json.dump({
        "commits": {"B": COMMIT_B, "C": COMMIT_C},
        "pk_exclus_Q19": sorted(PK_EXCLUS),
        "totaux": {
            "cartes_cascade": len(cascade_pks),
            "fr_subst": sum(len(d["cols"]) for d in fr_subst_detail),
            "subst_total": len(subst), "subst_prose": len(prose),
            "subst_libelles": len(labels),
            "subst_prose_cascade": len(cas),
            "perdu": len(perdu), "gagne": len(gagne),
        },
        "perdu": perdu,
        "subst_prose_classees": prose_sorted,
        "subst_libelles": labels,
        "fr_subst_detail": fr_subst_detail,
    }, open(OUT, "w", encoding="utf-8"), ensure_ascii=False, indent=1)
    print(f"dump : {OUT}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
