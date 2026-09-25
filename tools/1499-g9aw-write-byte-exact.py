#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1499 G9-A-W — écrire les cellules DÉSALIGNÉES actionnables de la mesure G9-A (PR #1564).

  492  text_es / text_fa / text_zh : rejoindre la famille « jouer la victime » déjà
       portée par fr/en/ru/pt/ar (l'ancien titre fr « Auto-victimisation » survit
       dans ces 3 langues).

⛔ Hors périmètre, nommé (worklist `hors_perimetre`) :
  796  example_fa  — glose non restaurée : exclusion portée par la décision Q-16 (c),
                    la PR #1549 et la garde FallaciesGlossRegisterGate ;
  1361 example_ar  — VOULU, pas désaligné (erratum G9-A) : #1546, « l'arabe standard
                    ne distingue pas tu/vous », épinglé par la garde
                    Pk1361_Example_Matches_The_Polite_Register_Decision. Une première
                    écriture à 4 cellules a ROUGI cette garde ; révertée depuis le
                    backup, jamais poussée. ⭐ Le rouge de la garde était le signal,
                    pas l'obstacle.

GARDES
  (1) PÉRIMÈTRE : la diff de valeurs == exactement les 3 cellules ;
  (2) ANCIENNES ÉPINGLÉES + ALIGNÉES INTOUCHABLES : les 3 valeurs courantes == les
      anciennes attendues ; les 5 langues déjà alignées de 492 == leurs valeurs ;
  (3) structure : 1408 lignes, BOM, terminaisons, 104 champs sur CHAQUE ligne ;
  (4) re-parse csv stdlib : les 3 cellules == worklist ;
  (5) post-état typographique #994 sur les 3 valeurs ;
  (6) mutation sur LITTÉRAUX + TÉMOIN.

Usage :
    python tools/1499-g9aw-write-byte-exact.py             # --check : plan, 0 écriture
    python tools/1499-g9aw-write-byte-exact.py --apply     # écrit + backup .BEFORE-G9AW
    python tools/1499-g9aw-write-byte-exact.py --mutation-test
"""
import argparse
import csv
import io
import json
import os
import sys

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_REL = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
WORKLIST = os.path.join(REPO, "tools", "1499-g9aw-worklist.json")
BACKUP_SUFFIX = ".BEFORE-G9AW"
BOM = b"\xef\xbb\xbf"
N_ROWS = 1408
STRAIGHT, DQUOTE, CURVED = "'", '"', "’"

# valeurs anciennes ÉPINGLÉES (fail-closed : le monde a bougé => rouge, pas d'écriture)
OLD_492 = {
    "text_es": "Auto-victimización",
    "text_fa": "خود-قربانی‌گری",
    "text_zh": "自我受害",
}
KEEP_492 = {  # les 5 langues déjà alignées (mesure G9-A) : intouchables, épinglées
    "text_fr": "Jouer la victime", "text_en": "Playing the Victim",
    "text_ru": "Игра в жертву", "text_pt": "Jogar a Vítima", "text_ar": "لعب دور الضحية",
}


def field_spans(record):
    spans, i, start, quoted = [], 0, 0, False
    while i < len(record):
        ch = record[i]
        if ch == '"':
            if quoted and i + 1 < len(record) and record[i + 1] == '"':
                i += 2
                continue
            quoted = not quoted
        elif ch == "," and not quoted:
            spans.append((start, i))
            start = i + 1
        i += 1
    spans.append((start, len(record)))
    return spans


def unquote(field):
    if len(field) >= 2 and field.startswith('"') and field.endswith('"'):
        return field[1:-1].replace('""', '"')
    return field


def split_raw(rec):
    return [rec[s:e] for s, e in field_spans(rec)]


def parse_raw(raw):
    bom = raw.startswith(BOM)
    text = raw[len(BOM):].decode("utf-8") if bom else raw.decode("utf-8")
    rows, cur, in_q, i, n = [], [], False, 0, len(text)
    while i < n:
        ch = text[i]
        if ch == '"':
            if in_q and i + 1 < n and text[i + 1] == '"':
                cur.append('""'); i += 2
            else:
                in_q = not in_q; cur.append(ch); i += 1
        elif ch in "\r\n" and not in_q:
            if ch == "\r" and i + 1 < n and text[i + 1] == "\n":
                rows.append(("".join(cur), "\r\n")); cur = []; i += 2
            else:
                rows.append(("".join(cur), "\n")); cur = []; i += 1
        else:
            cur.append(ch); i += 1
    if cur:
        rows.append(("".join(cur), ""))
    return bom, rows, [unquote(f) for f in split_raw(rows[0][0])]


def reserialize(raw_field, new_value):
    was_wrapped = len(raw_field) >= 2 and raw_field[0] == '"' and raw_field[-1] == '"'
    if was_wrapped:
        return '"' + new_value.replace('"', '""') + '"'
    assert not any(c in new_value for c in ',"\r\n'), \
        f"champ non quoté qui deviendrait spécial : {new_value[:60]!r}"
    return new_value


def residual_of(col, v):
    errs = []
    if DQUOTE in v:
        errs.append('" droit')
    if "en" in col and CURVED in v:
        errs.append("’ courbe")
    if "en" not in col and STRAIGHT in v:
        errs.append("' droit")
    if v != v.strip():
        errs.append("espace en tête/queue")
    return errs


def plan(cells):
    bom, rows, header = parse_raw(open(os.path.join(REPO, CSV_REL), "rb").read())
    assert len(rows) - 1 == N_ROWS, f"lignes {len(rows) - 1} != {N_ROWS}"
    text = "".join(r + t for r, t in rows)
    pk_i = header.index("PK")
    hits = [i for i, (rec, _t) in enumerate(rows)
            if i and unquote(split_raw(rec)[pk_i]).strip() == "492"]
    assert len(hits) == 1, f"PK 492 : {len(hits)} enregistrements"
    rec = rows[hits[0]][0]
    rec_off, spans = text.index(rec), field_spans(rec)
    fields = {h: unquote(v) for h, v in zip(header, split_raw(rec))}

    # garde (2) : anciennes épinglées + alignées intouchables, AVANT toute écriture
    for col, old in OLD_492.items():
        assert fields[col] == old, f"492 {col} : attendu l'ancienne {old!r}, lu {fields[col]!r}"
    for col, keep in KEEP_492.items():
        assert fields[col] == keep, f"492 {col} alignée doit rester {keep!r}, lu {fields[col]!r}"

    edits, avant = [], {}
    for c in cells:
        col, new = c["col"], c["valeur"]
        errs = residual_of(col, new)
        assert not errs, f"garde (5) [492] {col} : {errs} — {new[:60]!r}"
        assert fields[col] != new, f"[492] {col} : valeur identique, rien à écrire"
        avant[col] = fields[col]
        s, e = spans[header.index(col)]
        edits.append((rec_off + s, rec_off + e, reserialize(rec[s:e], new), col))
    edits.sort(key=lambda t: t[0], reverse=True)
    new_text = text
    for s, e, new, _col in edits:
        new_text = new_text[:s] + new + new_text[e:]
    return (BOM if bom else b"") + new_text.encode("utf-8"), sorted(edits, key=lambda t: t[3]), avant


def verify(old_raw, new_raw, cells):
    def rows_of(raw):
        t = raw[len(BOM):].decode("utf-8") if raw.startswith(BOM) else raw.decode("utf-8")
        return list(csv.DictReader(io.StringIO(t)))

    o, n = rows_of(old_raw), rows_of(new_raw)
    assert len(o) == len(n) == N_ROWS
    expected = {("492", c["col"]) for c in cells}
    changed = []
    for ro, rn in zip(o, n):
        assert ro["PK"] == rn["PK"], f"ordre modifié : {ro['PK']}"
        for k in ro:
            if ro[k] != rn[k]:
                changed.append((rn["PK"].strip(), k))
    assert set(changed) == expected, f"cellules changées != worklist : {set(changed) ^ expected}"
    bo, ro_, ho = parse_raw(old_raw)
    bn, rn_, hn = parse_raw(new_raw)
    assert ho == hn and bo == bn, "en-tête/BOM modifiés"
    assert [t for _r, t in ro_] == [t for _r, t in rn_], "terminaisons modifiées"
    # garde de FRONTIÈRE : le nombre de champs de CHAQUE ligne est inchangé
    for i, ((rec_o, _t), (rec_n, _u)) in enumerate(zip(ro_, rn_)):
        no, nn = len(split_raw(rec_o)), len(split_raw(rec_n))
        assert no == nn, f"ligne {i} : {no} -> {nn} champs (frontière de champ déplacée)"
    row = next(r for r in n if r["PK"].strip() == "492")
    for c in cells:
        assert row[c["col"]] == c["valeur"], f"492.{c['col']} non écrite"
    return changed


def mutation_test(cells):
    """Garde (6) : mutations LITTÉRALES de la worklist + un TÉMOIN vert.

    ⚠️ Les mutations portent sur la worklist, jamais sur l'arbre (un contrôle qui
    lit l'état qu'il valide ne se rejoue pas).
    """
    def patched(**valeur_by_col):
        return [dict(c, valeur=valeur_by_col[c["col"]]) if c["col"] in valeur_by_col
                else dict(c) for c in cells]

    def guard_worklist(patch_cells):
        try:
            for c in patch_cells:
                if c["col"] in OLD_492:
                    assert c["valeur"] != OLD_492[c["col"]], "valeur identique à l'ancienne"
                errs = residual_of(c["col"], c["valeur"])
                assert not errs, f"garde (5) : {errs}"
            return False
        except AssertionError:
            return True

    cases = [
        ("text_es laissée à l'ancienne valeur (Auto-victimización)",
         guard_worklist(patched(text_es=OLD_492["text_es"])), True),
        ("text_fa avec apostrophe droite (typographie #994)",
         guard_worklist(patched(text_fa="قربانی ج'لوه")), True),
        ("text_zh avec espace de tête (post-état)",
         guard_worklist(patched(text_zh=" 扮演受害者")), True),
        ("TEMOIN : worklist inchangée (3 valeurs retenues)",
         guard_worklist(patched()), False),
    ]
    ok = True
    for name, rougi, attendu in cases:
        good = rougi == attendu
        ok &= good
        print(f"  [{'PASS' if good else 'FAIL'}] {name} -> {'ROUGE' if rougi else 'vert'} "
              f"(attendu {'ROUGE' if attendu else 'vert'})")
    print("MUTATION " + ("OK (3 mutations vues, témoin vert)" if ok else "NON PROUVÉE"))
    return 0 if ok else 1


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--apply", action="store_true")
    ap.add_argument("--mutation-test", action="store_true")
    args = ap.parse_args()

    w = json.load(open(WORKLIST, encoding="utf-8"))
    cells = w["cellules"]
    assert len(cells) == 3 and all(c["pk"] == "492" for c in cells), \
        "worklist : attendu exactement les 3 cellules 492 (1361 et 796 hors périmètre)"
    if args.mutation_test:
        return mutation_test(cells)

    old_raw = open(os.path.join(REPO, CSV_REL), "rb").read()
    new_raw, edits, avant = plan(cells)
    changed = verify(old_raw, new_raw, cells)
    print(f"plan : {len(edits)} cellules, {len(changed)} changées, octets "
          f"{len(old_raw)} -> {len(new_raw)} ({len(new_raw) - len(old_raw):+d})")
    for _s, _e, _raw, col in edits:
        print(f"  492.{col:9s} {avant[col][:40]!r} -> "
              f"{next(c['valeur'] for c in cells if c['col'] == col)!r}")

    if not args.apply:
        print("MODE --check : aucune écriture.")
        return 0

    path = os.path.join(REPO, CSV_REL)
    with open(path + BACKUP_SUFFIX, "wb") as fh:
        fh.write(old_raw)
    with open(path, "wb") as fh:
        fh.write(new_raw)
    assert open(path, "rb").read() == new_raw, "relecture != écrit"
    print(f"APPLIQUÉ. backup : {CSV_REL}{BACKUP_SUFFIX}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
