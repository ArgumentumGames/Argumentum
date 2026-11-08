#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#498 grain 3 — groupe 2.1 « Procédé rhétorique » : poser la question critique
(élément (c)) aux deux sous-groupes attack-only, 2.1.2 Humour et 2.1.3 Poésie.

État mesuré du groupe avant ce geste (master `4763360a`, instrument = héritage
par `path` validé contre le tableau c.6665070534, 9/11 groupes reproduits) :

    2.1  Procédé rhétorique   123 nœuds, 81 sans (c)
      ├─ 2.1.1 Langage persuasif  COUVERT (D=GeneralAcceptanceDoubt_Conflict → (c) par héritage)
      ├─ 2.1.2 Humour             attack-only (undermine/I-node) → (c) à poser ICI
      └─ 2.1.3 Poésie             attack-only (undermine/I-node) → (c) à poser ICI

Aucun nœud sous 2.1.2/2.1.3 ne porte D/E/AT (balayage complet du sous-arbre) :
la mécanique se pose au plus haut nœud où elle est vraie — les deux têtes de
sous-groupe — et l'on ne descend pas vers les feuilles (plan #498 c.6665070534).
La tête 2.1 reste sans cellule propre : elle ne porte aucune mécanique mesurée
(même statut que les têtes de famille exemptées du tableau).

PROSE ONLY : jamais de token AIF fabriqué (les colonnes tokens sont épinglées
par FallacyAifVocabularyLockTests) ; la colonne AIF_criticalQuestion existe
depuis le grain 2 (PR #2194). Corpus : apostrophes U+2019, espace avant « ? ».

SURGICAL : byte-exact field-segment splitter, même technique que
tools/498-fallacies-aif-critical-question-column.py (grain 2) /
[[csv-byte-exact-column-insertion]]. BOM + CRLF préservés.
Ne met à jour AUCUNE cellule existante — 2 cellules ajoutées, rien d'autre.

GATED : dry-run par défaut ; `--write` applique.

Usage:
    python tools/498-grain3-groupe-21-cq.py            # dry-run + verify
    python tools/498-grain3-groupe-21-cq.py --write    # apply
"""
import csv, io, os, sys

PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
CQ_COL = "AIF_criticalQuestion"
WRITE = "--write" in sys.argv

# ── les 2 questions posées (keyed by path) ────────────────────────────────────
# Ancrage : desc_fr des deux têtes (« sans vous appuyer sur des faits » pour
# l'humour ; « images et un style fleuri » pour la poésie) + leur attaque
# commune undermine/I-node — le procédé discrédite/ supplante la prémisse au
# lieu de la traiter. La question sautée est celle qui distingue l'usage honnête
# (l'ornement porte un fait établi) du procédé (l'ornement tient lieu du fait).
SAMPLE = {
    "2.1.2": "Le rire a-t-il établi quelque chose sur le fond, ou a-t-il seulement évité d’y répondre ?",
    "2.1.3": "L’image embellit-elle un fait établi, ou tient-elle lieu du fait qui manque ?",
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
