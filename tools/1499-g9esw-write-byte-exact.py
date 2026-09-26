#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1499 G9-es-W — les 3 `desc_es` d'adresse incohérente de la mesure G9-es.

  1297 desc_es : « Repetís tu… » -> « Repites tu… »   (hybride 2e plur. + possessif sing.)
  1301 desc_es : « Repetís tu… » -> « Repites tu… »   (même hybride)
   492 desc_es : « Os presentáis como una víctima… » -> « Te presentas como una víctima… »

⛔ Hors périmètre, nommé :
  887 / 1092 / 1120 desc_es — vosotros COHÉRENT (verbe ET réflexif au pluriel) : GARDÉ,
      décision du dispatch grain 3. Épinglés par la garde (2) pour prouver qu'ils n'ont
      pas bougé.
  example_es — couvert par la décision Q-12 3a (#1546) et sa garde.
  Les 58 autres adressantes et les 114 impersonnelles restent : G9-es conclut
      « mélange stable », aucune harmonisation n'est écrite ici.

GARDES
  (1) PÉRIMÈTRE : la diff de valeurs == exactement les 3 cellules ;
  (2) ANCIENNES ÉPINGLÉES + PROTÉGÉES INTACTES : les 3 valeurs courantes == les
      anciennes attendues ; 887/1092/1120 desc_es et 492 text_es == leurs valeurs ;
  (3) SUFFIXE ÉPINGLÉ : chaque nouvelle valeur partage le suffixe de l'ancienne —
      seul le mot d'adresse en tête change (le dispatch : « the rest stays
      byte-identical ») ;
  (4) structure : 1408 lignes, BOM, terminaisons CRLF, 104 champs sur CHAQUE ligne ;
  (5) re-parse csv stdlib : les 3 cellules == worklist ;
  (6) post-état typographique #994 sur les 3 valeurs ;
  (7) CONTRÔLE INVERSE : les formes remplacées ont disparu du CSV, les formes gardées
      y sont toujours — un « 0 » n'est une absence que si l'instrument pouvait voir un « 1 » ;
  (8) mutation sur LITTÉRAUX + TÉMOIN.

Usage :
    python tools/1499-g9esw-write-byte-exact.py             # --check : plan, 0 écriture
    python tools/1499-g9esw-write-byte-exact.py --apply     # écrit + backup .BEFORE-G9ESW
    python tools/1499-g9esw-write-byte-exact.py --mutation-test
"""
import argparse
import csv
import io
import json
import os
import sys

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_REL = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
WORKLIST = os.path.join(REPO, "tools", "1499-g9esw-worklist.json")
BACKUP_SUFFIX = ".BEFORE-G9ESW"
BOM = b"\xef\xbb\xbf"
N_ROWS = 1408
N_FIELDS = 104          # mesuré : 104 champs sur 1409 lignes (en-tête incluse)
EOL = "\r\n"
STRAIGHT, DQUOTE, CURVED = "'", '"', "’"


# ── primitives de découpage (un seul analyseur, comme G9-A-W) ──────────────────
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
                cur.append('""')
                i += 2
            else:
                in_q = not in_q
                cur.append(ch)
                i += 1
        elif ch in "\r\n" and not in_q:
            if ch == "\r" and i + 1 < n and text[i + 1] == "\n":
                rows.append(("".join(cur), "\r\n"))
                cur = []
                i += 2
            else:
                rows.append(("".join(cur), "\n"))
                cur = []
                i += 1
        else:
            cur.append(ch)
            i += 1
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


# ── gardes pures (rejouables sur littéraux mutés) ─────────────────────────────
def errs_typo(col, v):
    """(#994) post-état typographique, par langue."""
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


def errs_cellule(c, ancienne_lue):
    """Garde d'une cellule : identité, suffixe épinglé, typographie."""
    errs = []
    if ancienne_lue != c["ancienne"]:
        errs.append(f"ancienne lue {ancienne_lue[:50]!r} != épinglée {c['ancienne'][:50]!r}")
    if c["valeur"] == c["ancienne"]:
        errs.append("valeur identique à l'ancienne — rien à écrire")
    suf = c["suffixe_epingle"]
    if not c["ancienne"].endswith(suf):
        errs.append("l'ancienne ne porte pas le suffixe épinglé")
    if not c["valeur"].endswith(suf):
        errs.append("la nouvelle ne porte pas le suffixe épinglé — la phrase a bougé au-delà du mot d'adresse")
    errs.extend(errs_typo(c["col"], c["valeur"]))
    return errs


def errs_protegees(protegees, lit):
    """Garde (2) : chaque cellule protégée doit tenir EXACTEMENT sa valeur au CSV."""
    errs = []
    for p in protegees:
        got = lit(p["pk"], p["col"])
        if got != p["valeur"]:
            errs.append(f"{p['pk']}.{p['col']} protégée : lu {got[:50]!r} != épinglée {p['valeur'][:50]!r}")
    return errs


# ── plan / écriture ───────────────────────────────────────────────────────────
def plan(cells, protegees):
    path = os.path.join(REPO, CSV_REL)
    old_raw = open(path, "rb").read()
    bom, rows, header = parse_raw(old_raw)
    assert len(rows) - 1 == N_ROWS, f"lignes {len(rows) - 1} != {N_ROWS}"
    assert all(len(field_spans(rec)) == N_FIELDS for rec, _t in rows), \
        f"toutes les lignes ne portent pas {N_FIELDS} champs"
    text = "".join(r + t for r, t in rows)
    pk_i = header.index("PK")

    def raw_of(pk):
        hits = [i for i, (rec, _t) in enumerate(rows)
                if i and unquote(split_raw(rec)[pk_i]).strip() == pk]
        assert len(hits) == 1, f"PK {pk} : {len(hits)} enregistrements"
        return hits[0]

    def lit(pk, col):
        rec = rows[raw_of(pk)][0]
        return unquote(split_raw(rec)[header.index(col)])

    # garde (2) AVANT toute écriture
    prot_errs = errs_protegees(protegees, lit)
    assert not prot_errs, "garde (2) protégées :\n  " + "\n  ".join(prot_errs)

    edits, avant = [], {}
    for c in cells:
        errs = errs_cellule(c, lit(c["pk"], c["col"]))
        assert not errs, f"garde cellule {c['pk']}.{c['col']} :\n  " + "\n  ".join(errs)
        rec = rows[raw_of(c["pk"])][0]
        rec_off = text.index(rec)
        spans = field_spans(rec)
        s, e = spans[header.index(c["col"])]
        avant[(c["pk"], c["col"])] = lit(c["pk"], c["col"])
        edits.append((rec_off + s, rec_off + e, reserialize(rec[s:e], c["valeur"]),
                      f"{c['pk']}.{c['col']}"))
    edits.sort(key=lambda t: t[0], reverse=True)
    new_text = text
    for s, e, new, _k in edits:
        new_text = new_text[:s] + new + new_text[e:]
    new_raw = (BOM if bom else b"") + new_text.encode("utf-8")
    return old_raw, new_raw, sorted(edits, key=lambda t: t[3]), avant


def verify(old_raw, new_raw, cells, controle):
    def rows_of(raw):
        t = raw[len(BOM):].decode("utf-8") if raw.startswith(BOM) else raw.decode("utf-8")
        return list(csv.DictReader(io.StringIO(t)))

    o, n = rows_of(old_raw), rows_of(new_raw)
    assert len(o) == len(n) == N_ROWS, f"lignes {len(o)} -> {len(n)}"
    expected = {(c["pk"], c["col"]) for c in cells}
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
    for i, ((rec_o, t_o), (rec_n, t_n)) in enumerate(zip(ro_, rn_)):
        assert t_o == t_n == EOL, f"ligne {i} : terminaison {t_o!r} -> {t_n!r}"
        no, nn = len(split_raw(rec_o)), len(split_raw(rec_n))
        assert no == nn == N_FIELDS, f"ligne {i} : {no} -> {nn} champs (frontière déplacée)"

    # garde (7) contrôle inverse, sur le TEXTE BRUT des deux états
    for absent in controle["absentes_apres"]:
        assert old_raw.decode("utf-8").count(absent) > 0, \
            f"contrôle inverse : {absent!r} était déjà absent AVANT — l'instrument ne pouvait pas voir un 1"
        got = new_raw.decode("utf-8").count(absent)
        assert got == 0, f"contrôle inverse : {absent!r} survit {got}× après écriture"
    for present in controle["presentes_apres"]:
        got = new_raw.decode("utf-8").count(present)
        assert got == 1, f"contrôle inverse : {present!r} attendu 1× (garde), lu {got}×"
    return changed


def mutation_test(cells, protegees, lit):
    """Garde (8) : mutations LITTÉRALES de la worklist + un TÉMOIN vert.

    ⚠️ Les mutations portent sur la WORKLIST, jamais sur l'arbre : un contrôle qui lit
    l'état qu'il valide ne se rejoue pas.
    """
    def patched_cells(**by_key):
        out = []
        for c in cells:
            key = c["pk"] + "." + c["col"]
            out.append(dict(c, **by_key[key]) if key in by_key else dict(c))
        return out

    def rouge_cellules(cs):
        try:
            for c in cs:
                errs = errs_cellule(c, lit(c["pk"], c["col"]))
                if errs:
                    return True
            return False
        except KeyError:
            return True

    def rouge_protegees(ps):
        return bool(errs_protegees(ps, lit))

    cases = [
        ("1297 : valeur laissée à l'ancienne (Repetís)",
         rouge_cellules(patched_cells(**{"1297.desc_es": {"valeur": cells[0]["ancienne"]}})), True),
        ("1301 : suffixe épinglé cassé (phrase réécrite au-delà du mot d'adresse)",
         rouge_cellules(patched_cells(**{"1301.desc_es": {"suffixe_epingle": " punto de vista"}})), True),
        ("492 : nouvelle valeur tronquée (suffixe perdu)",
         rouge_cellules(patched_cells(**{"492.desc_es": {"valeur": "Te presentas como una víctima"}})), True),
        # le préfixe porte l'apostrophe, le suffixe épinglé reste intact : la garde
        # typographique est isolée, elle ne rougit pas par le suffixe
        ("492 : apostrophe droite dans la nouvelle valeur (post-état #994)",
         rouge_cellules(patched_cells(**{"492.desc_es": {
             "valeur": "Te presentas d'un" + cells[2]["suffixe_epingle"]}})), True),
        ("887 (GARDÉ) : attente de protection fausse — la garde doit comparer, pas reciter",
         rouge_protegees([dict(p, valeur="Os liberáis") if p["pk"] == "887" else dict(p)
                          for p in protegees]), True),
        ("TÉMOIN : worklist inchangée",
         rouge_cellules(patched_cells()) or rouge_protegees([dict(p) for p in protegees]), False),
    ]
    ok = True
    for name, rougi, attendu in cases:
        good = rougi == attendu
        ok &= good
        print(f"  [{'PASS' if good else 'FAIL'}] {name} -> {'ROUGE' if rougi else 'vert'} "
              f"(attendu {'ROUGE' if attendu else 'vert'})")
    print("MUTATION " + ("OK (5 mutations vues, témoin vert)" if ok else "NON PROUVÉE"))
    return 0 if ok else 1


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--apply", action="store_true")
    ap.add_argument("--mutation-test", action="store_true")
    args = ap.parse_args()

    w = json.load(open(WORKLIST, encoding="utf-8"))
    cells, protegees, controle = w["cellules"], w["protegees"], w["controle_inverse"]
    assert len(cells) == 3, "worklist : attendu exactement 3 cellules"

    if args.mutation_test:
        # la garde des protégées lit le CSV : on lui donne la même lecture que le plan
        old_raw = open(os.path.join(REPO, CSV_REL), "rb").read()
        _bom, rows, header = parse_raw(old_raw)
        pk_i = header.index("PK")

        def lit(pk, col):
            rec = next(rec for i, (rec, _t) in enumerate(rows)
                       if i and unquote(split_raw(rec)[pk_i]).strip() == pk)
            return unquote(split_raw(rec)[header.index(col)])
        return mutation_test(cells, protegees, lit)

    old_raw, new_raw, edits, avant = plan(cells, protegees)
    changed = verify(old_raw, new_raw, cells, controle)
    print(f"plan : {len(edits)} cellules, {len(changed)} changées, octets "
          f"{len(old_raw)} -> {len(new_raw)} ({len(new_raw) - len(old_raw):+d})")
    for _s, _e, _raw, key in edits:
        new = next(c["valeur"] for c in cells if c["pk"] + "." + c["col"] == key)
        print(f"  {key:16s} {avant[(key.split('.')[0], key.split('.')[1])][:44]!r} -> {new[:44]!r}")
    print("contrôle inverse : " +
          " · ".join(f"{a!r} {old_raw.decode('utf-8').count(a)} -> "
                     f"{new_raw.decode('utf-8').count(a)}" for a in controle["absentes_apres"]))

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
