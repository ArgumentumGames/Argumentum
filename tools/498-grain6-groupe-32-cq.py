#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#498 grain 6 — groupe 3.2 « Mauvaise interprétation » : poser la question critique
(élément (c)) à la tête du groupe et à ses deux sous-groupes porteurs non couverts,
3.2.2 « Probabilités faussées » et 3.2.3 « Infini trompeur ».

État mesuré du groupe avant ce geste (master `d0ca02c8`, instrument = héritage par
`path`, (c) = cible de ref finissant en `_Conflict` OU prose `AIF_criticalQuestion` ;
mêmes définitions que docs/ontology/498-instrument-path-2026-11-06.md) :

    3.2  Mauvaise interprétation   34 nœuds, 23 sans (c)
      ├─ 3.2   (tête)  skosExceptionRef = EvidenceToHypothesis_Inference ;
      │                undercut / RA-node                     → (c) à poser ICI
      ├─ 3.2.1 Relation infondée   DirectRef PropertyNotExistant_Conflict
      │                → (c) DÉJÀ LÀ (11 nœuds héritent), pas de prose par-dessus
      ├─ 3.2.2 Probabilités faussées  undermine / I-node, AUCUN schéma propre
      │                → (c) à poser ICI (14 nœuds héritent)
      └─ 3.2.3 Infini trompeur     skosExceptionRef = Gradualism_Inference ;
                       undercut / RA-node                     → (c) à poser ICI
                                       (8 nœuds héritent)

Placement (plan #498 c.6665070534 : « le mécanisme se pose au plus haut nœud où il
est vrai : le groupe lui-même, ou ses trois sous-groupes ; on ne descend vers une
feuille que si sa mécanique diffère »). La tête 3.2 porte (a) un schéma et (b) une
attaque ⇒ le plus haut nœud où la mécanique est vraie est la tête elle-même
(précédent grain 5 pour 3.1, précédent 6.3 pour la tête + sous-groupes).

3.2.2 descend d'un cran parce que sa mécanique DIFFÈRE : attaque `undermine` sur un
`I-node` (l'inférence probabiliste elle-même est mauvaise), là où la tête `undercut`
un `RA-node`. 3.2.3 descend d'un cran pour une autre raison : même polarité
d'attaque que la tête, mais le schéma légitime imité est `Gradualism_Inference`, et
non `EvidenceToHypothesis_Inference` — la question sautée n'est pas la même.

Balayage mesuré : les seules rangées à tokens du groupe sont 3.2, 3.2.1, 3.2.2
(attaque seule) et 3.2.3 — aucun nœud plus profond ne porte D/E/AT.

PROSE ONLY : jamais de token AIF fabriqué (les colonnes tokens sont épinglées par
FallacyAifVocabularyLockTests) ; la colonne AIF_criticalQuestion existe depuis le
grain 2 (PR #2194). Corpus : apostrophes U+2019, espace U+0020 avant « ? ».

SURGICAL : byte-exact field-segment splitter, même technique que
tools/498-fallacies-aif-critical-question-column.py (grain 2),
tools/498-grain3-groupe-21-cq.py (grain 3), tools/498-grain4-groupe-22-cq.py
(grain 4) et tools/498-grain5-groupe-31-cq.py (grain 5) /
[[csv-byte-exact-column-insertion]]. BOM + CRLF préservés.
Ne met à jour AUCUNE cellule existante — 3 cellules ajoutées, rien d'autre.

GATED : dry-run par défaut ; `--write` applique.

Usage:
    python tools/498-grain6-groupe-32-cq.py            # dry-run + verify
    python tools/498-grain6-groupe-32-cq.py --write    # apply
"""
import csv, io, os, sys

PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
CQ_COL = "AIF_criticalQuestion"
WRITE = "--write" in sys.argv

REF_COLS = ("AIF_skosDirectRef", "AIF_skosExceptionRef", "AIF_skosOther")

# ── les 3 questions posées (keyed by path) ────────────────────────────────────
# Ancrage : desc_fr de chaque nœud + le schéma légitime imité + l'attaque. Chaque
# question est à deux branches : l'usage honnête d'un côté, le procédé de l'autre.
SAMPLE = {
    "3.2": "La relation établie entre les données est-elle celle qu’elles portent, ou l’interprétation leur fait-elle dire plus qu’elles n’établissent ?",
    "3.2.2": "La probabilité invoquée s’applique-t-elle au cas considéré, ou le raisonnement en tire-t-il une conclusion qu’elle n’autorise pas ?",
    "3.2.3": "Le passage d’un degré au suivant est-il justifié par un critère établi, ou la série est-elle traitée comme si elle devait avoir une limite nette qu’aucune étape n’établit ?",
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

# ── sonde de couverture (prose-aware, héritage par `path`) ────────────────────
# Mêmes définitions que l'instrument committé (tools/498-aif-cadrage-measure.py,
# doc corrigé 2026-11-06) étendu à la prose — l'instrument committé est
# prose-aveugle par construction (il ne compte que les cibles `*_Conflict`).
def coverage(rows_fields):
    by = {r[path_idx].strip(): r for r in rows_fields}
    def carries_c(r):
        for c in REF_COLS:
            for tgt in r[header.index(c)].replace(";", ",").split(","):
                if tgt.strip().endswith("_Conflict"):
                    return True
        return bool(r[CQ_IDX].strip())
    def covered(p):
        parts = p.split(".")
        return any(carries_c(by[".".join(parts[:i])])
                   for i in range(1, len(parts) + 1) if ".".join(parts[:i]) in by)
    def group(pfx):
        return [p for p in by if p == pfx or p.startswith(pfx + ".")]
    return {g: sum(1 for p in group(g) if not covered(p)) for g in ("3.1", "3.2", "3.3")}, \
           sum(1 for p in by if not covered(p))

before_g, before_tree = coverage(data)
print(f"couverture AVANT : groupe 3.2 = {before_g['3.2']} sans (c) "
      f"(3.1 = {before_g['3.1']}, 3.3 = {before_g['3.3']}), arbre = {before_tree}")
assert before_g["3.2"] == 23, f"expected 23 uncovered nodes in 3.2, measured {before_g['3.2']}"

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
    return filled_after, got

filled, got = verify(new_text)
print(f"verify OK: 105 cols, fill {filled - len(SAMPLE)} -> {filled} "
      f"(+{len(SAMPLE)}), every other cell byte-identical, csv-module re-parse equal")

after_g, after_tree = coverage(got)
print(f"couverture APRÈS : groupe 3.2 = {after_g['3.2']} sans (c) "
      f"(3.1 = {after_g['3.1']}, 3.3 = {after_g['3.3']} inchangés), arbre = {after_tree} "
      f"(avant {before_tree})")
assert after_g["3.2"] == 0, f"group 3.2 still has {after_g['3.2']} uncovered nodes"
assert after_g["3.1"] == before_g["3.1"] and after_g["3.3"] == before_g["3.3"], \
    "neighbouring groups changed — forbidden"
assert after_tree == before_tree - 23, f"tree count {before_tree} -> {after_tree}, expected -23"

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
