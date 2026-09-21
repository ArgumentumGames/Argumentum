#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1471 grain 2 (pool v9) - appliquer les 225 cibles langlinks sur la base vivante.

Reutilise les primitives byte-exactes de `tools/994-apostrophe-dryrun.py` :
  - load(repo, relpath) -> {bom, rows, header, raw}
  - split_logical_rows(text), split_fields(row), unquote(field)

NE MODIFIE AUCUNE CELLULE NON VIDE (filtre prealablement applique par l'auditeur).
NE TOUCHE PAS LES COLONNES RENDUES : le scanner a deja verifie qu'aucun {{link}}
n'apparait dans les gabarits de carte ou de mindmap.
PRESERVE BOM, CRLF, et l'ordre exact des lignes logiques.

Usage :
    python tools/1471-langlinks-apply.py [--dry-run | --apply] [--repo .]
"""
import argparse
import importlib.util
import json
import os
import sys
import shutil

HERE = os.path.dirname(os.path.abspath(__file__))
APOST = os.path.join(HERE, "994-apostrophe-dryrun.py")
DEFAULTS_REPO = r"D:\Dev\Argumentum"
DEFAULTS_CANDS = (
    r"C:\Users\jsboi\AppData\Local\Temp\claude\d--Dev-Argumentum"
    r"\172191ee-3477-446e-806a-cba92154afc5\scratchpad\cands.jsonl"
)
REL = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"


def load_instr():
    spec = importlib.util.spec_from_file_location("apostrophe_instr", APOST)
    m = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(m)
    return m


def load_candidates(path):
    cands = []
    with open(path, encoding="utf-8") as fh:
        for line in fh:
            line = line.strip()
            if not line:
                continue
            cands.append(json.loads(line))
    return cands


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--dry-run", action="store_true",
                    help="affiche le plan sans toucher au CSV (defaut)")
    ap.add_argument("--apply", action="store_true",
                    help="ecrit le CSV (atomiquement : .tmp + rename)")
    ap.add_argument("--repo", default=DEFAULTS_REPO)
    ap.add_argument("--cands", default=DEFAULTS_CANDS)
    a = ap.parse_args()
    if a.dry_run and a.apply:
        sys.exit("--dry-run et --apply s'excluent")
    if not a.dry_run and not a.apply:
        a.dry_run = True

    instr = load_instr()
    corpus = instr.load(a.repo, REL)
    header = corpus["header"]
    cols = {h: i for i, h in enumerate(header)}

    rows_raw = corpus["rows"]
    bom = corpus["bom"]

    rows = []
    for r, t in rows_raw:
        f = [instr.unquote(x) for x in instr.split_fields(r)]
        rows.append((f, t))

    pk_col = cols["PK"]
    pk_to_idx = {f[pk_col]: i for i, (f, _) in enumerate(rows) if f and f[pk_col]}

    cands = load_candidates(a.cands)
    print(f"=== #1471 grain 2 -- {'DRY RUN' if a.dry_run else 'APPLY'} ===")
    print(f"  corpus   : {REL}")
    print(f"  candidats: {len(cands)}")

    plan = []
    skipped = []
    for c in cands:
        pk = c["pk"]
        col_name = c["colonne_csv"]
        idx_col = cols.get(col_name)
        if idx_col is None:
            skipped.append((pk, col_name, "colonne_absente"))
            continue
        idx_row = pk_to_idx.get(pk)
        if idx_row is None:
            skipped.append((pk, col_name, "pk_absente"))
            continue
        cur = rows[idx_row][0][idx_col]
        if cur.strip():
            skipped.append((pk, col_name, "cellule_deja_non_vide"))
            continue
        plan.append((pk, col_name, idx_row, idx_col, c["url_cible"], c["titre_cible"]))

    print(f"  plan     : {len(plan)} (ecritures prevues)")
    print(f"  rejets   : {len(skipped)}")
    if skipped[:3]:
        for s in skipped[:3]:
            print(f"    skip : {s}")
    if plan[:5]:
        print("  ecritures prevues (5 premieres) :")
        for p in plan[:5]:
            print(f"    PK {p[0]:>4}  {p[1]:<10} <- {p[4]}")
    print(f"  distribution par colonne :")
    from collections import Counter
    by_col = Counter(p[1] for p in plan)
    for c, n in sorted(by_col.items()):
        print(f"    {c:<10} : {n}")
    print(f"  taille CSV avant : {os.path.getsize(os.path.join(a.repo, REL))} o")

    if a.dry_run:
        print("\n  (dry-run : aucune ecriture)")
        return 0

    # APPLICATION : on reconstitue le fichier ligne par ligne, en injectant la
    # nouvelle valeur a l'index idx_col de la ligne idx_row. La structure rows_raw
    # porte deja (champ, terminateur) donc on preserve la mise en forme.
    new_rows = list(rows_raw)
    for (pk, col_name, idx_row, idx_col, url, titre) in plan:
        r_str, term = new_rows[idx_row]
        fields = instr.split_fields(r_str)
        # fields[idx_col] est le champ brut (avec quotes si applicable)
        # On l'overwrite avec la nouvelle URL, en quotant si elle contient un separateur
        new_val = url
        if any(ch in new_val for ch in (",", '"', "\n", "\r")):
            new_val = '"' + new_val.replace('"', '""') + '"'
        fields[idx_col] = new_val
        new_rows[idx_row] = (",".join(fields), term)

    body = "".join(r + t for r, t in new_rows)
    if bom:
        body = "\ufeff" + body
    target = os.path.join(a.repo, REL)
    tmp = target + ".tmp"
    with open(tmp, "wb") as fh:
        fh.write(body.encode("utf-8"))
    os.replace(tmp, target)
    print(f"  taille CSV apres : {os.path.getsize(target)} o")

    # Sanity : recharge et verifie que chaque ecriture est en place
    chk = instr.load(a.repo, REL)
    chk_cols = {h: i for i, h in enumerate(chk["header"])}
    chk_pk = chk_cols["PK"]
    chk_rows = [[instr.unquote(x) for x in instr.split_fields(r)]
                for r, _t in chk["rows"][1:]]
    chk_by_pk = {f[chk_pk]: f for f in chk_rows if f}
    bad = 0
    for (pk, col_name, _idx_row, idx_col, url, _titre) in plan:
        if chk_by_pk[pk][idx_col] != url:
            bad += 1
    print(f"  verif relecture : {len(plan)-bad}/{len(plan)} ecritures en place")
    if bad:
        return 1
    return 0


if __name__ == "__main__":
    sys.exit(main())
