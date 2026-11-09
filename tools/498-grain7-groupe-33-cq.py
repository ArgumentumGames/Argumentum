#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#498 grain 7 — groupe 3.3 « Résultat invalide » : poser la question critique
(élément (c)) sur ses trois sous-groupes porteurs — 3.3.1 « Imprécision »,
3.3.2 « Erreur de calcul », 3.3.3 « Opération inappropriée ».

État mesuré du groupe avant ce geste (master `a34fbcda`, instrument = héritage par
`path`, (c) = cible de ref finissant en `_Conflict` OU prose `AIF_criticalQuestion` ;
définitions de docs/ontology/498-instrument-path-2026-11-06.md, sonde calibrée
grain 6 : arbre = 101, 3.2 = 0, 3.3 = 29) :

    3.3  Résultat invalide     30 nœuds, 29 sans (c)
      ├─ 3.3   (tête)  AUCUN token, AUCUNE attaque → tête NUE, exemptée
      │                (même statut que 2.1/2.2 aux grains 3-4 : sans mécanique
      │                propre, la mécanique vit dans les sous-groupes)
      ├─ 3.3.1 Imprécision        undermine / I-node, aucun schéma → (c) ICI (14)
      │    └─ 3.3.1.3.1 Pente glissante  WeakestLink_Conflict → (c) DÉJÀ LÀ
      ├─ 3.3.2 Erreur de calcul   undermine / I-node, aucun schéma → (c) ICI (9)
      └─ 3.3.3 Opération inappropriée  undercut / RA-node, aucun schéma → (c) ICI (6)

Placement (plan #498 c.6665070534 : « le mécanisme se pose au plus haut nœud où il
est vrai : le groupe lui-même, ou ses trois sous-groupes ; on ne descend vers une
feuille que si sa mécanique diffère »). La tête 3.3 ne porte NI schéma NI attaque :
elle est exempte (précédents 2.1 grain 3, 2.2 grain 4 — le groupe finit à 1, la
tête, documenté comme tel). La mécanique vit dans les trois sous-groupes :

- 3.3.1 et 3.3.2 partagent la polarité (undermine / I-node) mais leurs questions
  sautées diffèrent : la précision affichée vs l'exactitude du calcul ;
- 3.3.3 attaque autre chose (undercut / RA-node : l'opération ne s'applique pas à
  l'argument, la prémisse n'est pas seulement fausse) ;
- 3.3.1.3.1 « Pente glissante » porte déjà WeakestLink_Conflict : (c) est
  satisfait, pas de prose par-dessus.

Balayage mesuré : les seules rangées à tokens du groupe sont 3.3.1.3.1 (refs) et
les trois sous-groupes (attaque seule) — aucun autre nœud ne porte D/E/AT.

Effet attendu : groupe 3.3 29 → 1 (la tête nue), arbre 101 → 73, voisins 3.2 (0)
et 6.1 (65) inchangés — tout asserté ci-dessous.

PROSE ONLY : jamais de token AIF fabriqué (les colonnes tokens sont épinglées par
FallacyAifVocabularyLockTests) ; la colonne AIF_criticalQuestion existe depuis le
grain 2 (PR #2194). Corpus : apostrophes U+2019, espace U+0020 avant « ? ».

SURGICAL : byte-exact field-segment splitter, même technique que les grains 2-6
([[csv-byte-exact-column-insertion]]). BOM + CRLF préservés.
Ne met à jour AUCUNE cellule existante — 3 cellules ajoutées, rien d'autre.

GATED : dry-run par défaut ; `--write` applique.

Usage:
    python tools/498-grain7-groupe-33-cq.py            # dry-run + verify
    python tools/498-grain7-groupe-33-cq.py --write    # apply
"""
import csv, io, os, sys

PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
CQ_COL = "AIF_criticalQuestion"
WRITE = "--write" in sys.argv

REF_COLS = ("AIF_skosDirectRef", "AIF_skosExceptionRef", "AIF_skosOther")

# ── les 3 questions posées (keyed by path) ────────────────────────────────────
# Ancrage : desc_fr de chaque nœud + l'attaque. Chaque question est à deux
# branches : l'usage honnête d'un côté, le procédé de l'autre.
SAMPLE = {
    "3.3.1": "La certitude de la conclusion correspond-elle à la précision réelle des données, ou l’impression d’exactitude tient-elle lieu de mesure ?",
    "3.3.2": "Le résultat du calcul est-il exact et vérifiable, ou l’erreur qu’il contient tient-elle lieu de démonstration ?",
    "3.3.3": "L’opération appliquée est-elle valide pour les objets qu’elle manipule, ou le résultat tient-il lieu de preuve alors que l’opération ne s’applique pas ?",
}

NUDE_HEAD = "3.3"   # tête sans mécanique propre : reste sans (c), documentée

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
# garde : la tête exemptée doit être NUE (aucun token, aucune attaque, aucune prose)
head_r = data[paths.index(NUDE_HEAD)]
head_carries = any(head_r[header.index(c)].strip() for c in REF_COLS) \
    or head_r[header.index("AIF_attackType")].strip() or head_r[CQ_IDX].strip()
assert not head_carries, f"{NUDE_HEAD} is no longer a nude head — placement must be re-derived"

print(f"source: {len(data)} rows, CQ col idx {CQ_IDX}, targets {sorted(SAMPLE)} (vierges), "
      f"head {NUDE_HEAD} nude (exemptée)")

# ── sonde de couverture (prose-aware, héritage par `path`) ────────────────────
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
    return {g: [p for p in group(g) if not covered(p)] for g in ("3.2", "3.3", "6.1")}, \
           [p for p in by if not covered(p)]

before_g, before_tree = coverage(data)
print(f"couverture AVANT : groupe 3.3 = {len(before_g['3.3'])} sans (c) "
      f"(3.2 = {len(before_g['3.2'])}, 6.1 = {len(before_g['6.1'])}), arbre = {len(before_tree)}")
assert len(before_g["3.3"]) == 29, f"expected 29 uncovered nodes in 3.3, measured {len(before_g['3.3'])}"
assert len(before_tree) == 101, f"expected tree at 101 (grain 6 merged), measured {len(before_tree)}"

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
print(f"couverture APRÈS : groupe 3.3 = {len(after_g['3.3'])} sans (c) "
      f"[{', '.join(after_g['3.3'])} — tête nue exemptée] "
      f"(3.2 = {len(after_g['3.2'])}, 6.1 = {len(after_g['6.1'])} inchangés), "
      f"arbre = {len(after_tree)} (avant {len(before_tree)})")
assert after_g["3.3"] == [NUDE_HEAD], \
    f"only the nude head {NUDE_HEAD} may remain uncovered, got {after_g['3.3']}"
assert len(after_g["3.2"]) == len(before_g["3.2"]) and len(after_g["6.1"]) == len(before_g["6.1"]), \
    "neighbouring groups changed — forbidden"
assert len(after_tree) == len(before_tree) - 28, \
    f"tree count {len(before_tree)} -> {len(after_tree)}, expected -28"

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
