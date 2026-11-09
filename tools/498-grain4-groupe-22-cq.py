#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#498 grain 4 — groupe 2.2 « Appel à l'émotion » : poser la question critique
(élément (c)) aux trois têtes de sous-groupe sans (c) : 2.2.1 Connivence,
2.2.2 Repoussoir et 2.2.3 Appel aux conséquences.

État mesuré du groupe avant ce geste (master `4b1b8bf9`, instrument = héritage
par `path`, (c) = token *_Conflict OU prose AIF_criticalQuestion — reproduit
l'arithmétique du grain 3 : arbre 271→196) :

    2.2  Appel à l'émotion   57 nœuds, 54 sans (c)
      ├─ 2.2.1 Connivence              PositiveConsequences_Inference, undermine/I-node → (c) à poser ICI
      ├─ 2.2.2 Repoussoir              NegativeConsequences_Inference,  undermine/I-node → (c) à poser ICI
      │    └─ 2.2.2.4 Appel à la terreur  IrrationalFearAppeal_Conflict → (c) DÉJÀ LÀ (couvre ses 2 enfants)
      └─ 2.2.3 Appel aux conséquences  EstablishedRule_Inference,        undermine/I-node → (c) à poser ICI

Aucun nœud sous 2.2.1/2.2.2/2.2.3 ne porte D/E/AT (balayage complet du
sous-arbre : les seules rangées à tokens du groupe sont les quatre ci-dessus) :
la mécanique se pose au plus haut nœud où elle est vraie — les trois têtes de
sous-groupe — et l'on ne descend pas vers les feuilles (plan #498 c.6665070534).
La tête 2.2 reste sans cellule propre : elle ne porte aucune mécanique mesurée
(même statut que 2.1 au grain 3 et les têtes de famille exemptées du tableau).

Ancrage des trois questions : desc_fr de chaque tête + le schéma légitime imité
(skosExceptionRef) + l'attaque commune undermine/I-node. La question sautée est
celle qui distingue l'usage honnête du schéma (le bénéfice/le mal/la règle est
DÉMONTRÉ) du procédé (l'émotion tient lieu de preuve).

PROSE ONLY : jamais de token AIF fabriqué (les colonnes tokens sont épinglées
par FallacyAifVocabularyLockTests) ; la colonne AIF_criticalQuestion existe
depuis le grain 2 (PR #2194). Corpus : apostrophes U+2019, espace avant « ? ».

SURGICAL : byte-exact field-segment splitter, même technique que
tools/498-fallacies-aif-critical-question-column.py (grain 2) et
tools/498-grain3-groupe-21-cq.py (grain 3) / [[csv-byte-exact-column-insertion]].
BOM + CRLF préservés. Ne met à jour AUCUNE cellule existante — 3 cellules
ajoutées, rien d'autre.

GATED : dry-run par défaut ; `--write` applique.

Usage:
    python tools/498-grain4-groupe-22-cq.py            # dry-run + verify
    python tools/498-grain4-groupe-22-cq.py --write    # apply
"""
import csv, io, os, sys

PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
CQ_COL = "AIF_criticalQuestion"
WRITE = "--write" in sys.argv

# ── les 3 questions posées (keyed by path) ────────────────────────────────────
SAMPLE = {
    "2.2.1": "Le bénéfice promis est-il démontré, ou l’alliance émotionnelle tient-elle lieu de preuve ?",
    "2.2.2": "Le mal invoqué est-il démontré, ou l’aversion tient-elle lieu d’examen ?",
    "2.2.3": "Les conséquences avancées sont-elles établies, ou tiennent-elles lieu de la justesse de l’idée ?",
}

# ── byte-exact CSV splitters (doubled quotes + embedded newlines safe) ────────
def split_logical_rows(text):
    rows, cur, in_q = [], [], False
    i, n = 0, len(text)
    while i < n:
        ch = text[i]
        if ch == '"':
            if in_q and i + 1 < n and text[i + 1] == '"':
                cur.append('""'); i += 2
            else:
                in_q = not in_q; cur.append(ch); i += 1
        elif ch == '\r' and not in_q and i + 1 < n and text[i + 1] == '\n':
            rows.append(''.join(cur)); cur = []; in_q = False; i += 2
        elif ch == '\n' and not in_q:
            rows.append(''.join(cur)); cur = []; in_q = False; i += 1
        else:
            cur.append(ch); i += 1
    tail = ''.join(cur)
    return rows, tail

def split_fields(row):
    fields, cur, in_q = [], [], False
    i, n = 0, len(row)
    while i < n:
        ch = row[i]
        if ch == '"':
            if in_q and i + 1 < n and row[i + 1] == '"':
                cur.append('""'); i += 2
            else:
                in_q = not in_q; cur.append(ch); i += 1
        elif ch == ',' and not in_q:
            fields.append(''.join(cur)); cur = []; i += 1
        else:
            cur.append(ch); i += 1
    fields.append(''.join(cur))
    return fields

# ── load ──────────────────────────────────────────────────────────────────────
raw_bytes = open(PATH, "rb").read()
assert raw_bytes[:3] == b'\xef\xbb\xbf', "BOM expected"
text = raw_bytes[3:].decode("utf-8")
rows, tail = split_logical_rows(text)
assert tail == "", f"unexpected bytes after last row terminator: {tail!r}"
assert "\r\n".join(rows) + "\r\n" == text, "splitter round-trip mismatch on ORIGINAL"

header = split_fields(rows[0])
assert len(header) == 105, f"expected 105 header cols, got {len(header)}"
CQ_IDX = header.index(CQ_COL)
assert header[CQ_IDX - 1] == "AIF_skosOther" and header[CQ_IDX + 1] == "AIF_skosMappingType"
path_idx = header.index("path")
data = [split_fields(r) for r in rows[1:]]
assert all(len(r) == 105 for r in data), "ragged rows"

paths = [r[path_idx].strip() for r in data]
assert len(paths) == len(set(paths)), "duplicate path keys"
missing = [p for p in SAMPLE if p not in paths]
assert not missing, f"sample paths absent from CSV: {missing}"
# garde : les cibles doivent être VIERGES (ce geste n'écrase aucune valeur)
prefilled = [p for p in SAMPLE if data[paths.index(p)][CQ_IDX].strip()]
assert not prefilled, f"target cells already filled (refuse overwrite): {prefilled}"

print(f"source: {len(data)} rows, CQ col idx {CQ_IDX}, targets {sorted(SAMPLE)} (vierges)")

# ── transform ─────────────────────────────────────────────────────────────────
new_rows = []
for r, p in zip(data, paths):
    r = list(r)
    if p in SAMPLE:
        r[CQ_IDX] = f'"{SAMPLE[p]}"'   # toujours quotées (virgule interne possible)
    new_rows.append(r)
new_text = "\r\n".join([",".join(header)] + [",".join(r) for r in new_rows]) + "\r\n"

# ── verify (dry-run AND after --write) ────────────────────────────────────────
def verify(t):
    got_rows, got_tail = split_logical_rows(t)
    assert got_tail == ""
    assert "\r\n".join(got_rows) + "\r\n" == t, "splitter round-trip mismatch on NEW"
    got_header = split_fields(got_rows[0])
    assert got_header == header, "header changed — forbidden"
    got = [split_fields(r) for r in got_rows[1:]]
    assert len(got) == len(data)
    filled_before = sum(1 for r in data if r[CQ_IDX].strip())
    for old_r, new_r, p in zip(data, got, paths):
        assert len(new_r) == 105
        # every cell except the targets byte-identical, verbatim
        if p in SAMPLE:
            assert new_r[CQ_IDX] == f'"{SAMPLE[p]}"', f"value drift on {p}"
        else:
            assert new_r[CQ_IDX] == old_r[CQ_IDX], f"touched a non-target cell ({p})"
        for j in range(105):
            if j != CQ_IDX:
                assert new_r[j] == old_r[j], f"touched col {header[j]} on {p}"
    filled_after = sum(1 for r in got if r[CQ_IDX].strip())
    assert filled_after == filled_before + len(SAMPLE), \
        f"fill count {filled_before} -> {filled_after}, expected +{len(SAMPLE)}"
    # independent re-parse + decoded-value equality
    parsed = list(csv.reader(io.StringIO(t, newline="")))
    assert len(parsed) == 1 + len(data) and all(len(r) == 105 for r in parsed)
    h = parsed[0]
    cq, pcol = h.index(CQ_COL), h.index("path")
    got_cq = {r[pcol].strip(): r[cq] for r in parsed[1:]}
    for k, v in SAMPLE.items():
        assert got_cq[k] == v, f"csv-module value differs on {k}: {got_cq[k]!r}"
    sm = [r[h.index("AIF_skosMappingType")] for r in parsed[1:]]
    src_sm = [r[CQ_IDX + 1] for r in data]
    assert len([v for v in sm if v.strip()]) == len([v for v in src_sm if v.strip()]), \
        "AIF_skosMappingType fill count changed — forbidden"
    return filled_after

filled = verify(new_text)
print(f"verify OK: 105 cols, fill {filled - len(SAMPLE)} -> {filled} "
      f"(+{len(SAMPLE)}), every other cell byte-identical, csv-module re-parse equal")

if WRITE:
    out = b'\xef\xbb\xbf' + new_text.encode("utf-8")   # encode FIRST — never inside open()
    tmp = PATH + ".tmp"
    with open(tmp, "wb") as fh:
        fh.write(out)
    os.replace(tmp, PATH)   # atomic
    print(f"WRITTEN: {PATH} ({len(out)} bytes)")
    recheck = open(PATH, "rb").read()[3:].decode("utf-8")
    verify(recheck)
    print("post-write verify OK")
else:
    print("dry-run only — pass --write to apply")
