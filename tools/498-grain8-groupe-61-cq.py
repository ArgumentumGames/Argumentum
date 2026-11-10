#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#498 grain 8 — groupe 6.1 « Arranger les faits » : poser la question critique
(élément (c)) sur sa tête porteuse et ses deux sous-groupes porteurs — 6.1.1
« Mensonge », 6.1.2 « Fausse attribution ». DERNIER groupe de la campagne.

État mesuré du groupe avant ce geste (master `2f203bcd`, instrument = héritage par
`path`, (c) = cible de ref finissant en `_Conflict` OU prose `AIF_criticalQuestion` ;
définitions de docs/ontology/498-instrument-path-2026-11-06.md, sonde calibrée
grains 6-7 : arbre = 73, 3.2 = 0, 3.3 = 1 (tête nue), 6.1 = 65) :

    6.1  Arranger les faits     85 nœuds, 65 sans (c)
      ├─ 6.1   (tête)  skosDirectRef=LackOfPTKReliability_Scheme +
      │               skosExceptionRef=PositionToKnow_Inference + undermine/I-node
      │               → tête PORTEUSE (a)+(b) → CQ ICI (précédents 3.1 grain 5,
      │               3.2 grain 6, 6.3 grain 2)
      ├─ 6.1.1 Mensonge          undermine / I-node, aucun schéma → CQ ICI (53)
      ├─ 6.1.2 Fausse attribution undermine / I-node, aucun schéma → CQ ICI (11)
      └─ 6.1.3 Attention sélective  GeneralAcceptanceDoubt_Conflict → (c) DÉJÀ LÀ
                                  (sous-arbre entier couvert par héritage, intact)

Placement (plan #498 c.6665070534 : « le mécanisme se pose au plus haut nœud où il
est vrai : le groupe lui-même, ou ses trois sous-groupes ; on ne descend vers une
feuille que si sa mécanique diffère »). La tête 6.1 PORTE (a)+(b) : elle reçoit sa
propre CQ (symétrique des grains 5-6 ; l'inverse du grain 7, où la tête 3.3 était
nue et exemptée). La descente sur les deux sous-groupes est justifiée par des
questions sautées DIFFÉRENTES, même si la polarité est partagée (undermine /
I-node ; précédent grain 7 : 3.3.1 vs 3.3.2, même polarité, question différente) :

- la tête interroge la position pour savoir : la présentation reflète-t-elle ce
  que la source peut établir ?
- 6.1.1 « Mensonge » : le menteur, lui, EST en position de savoir — c'est ce qui
  fait le mensonge ; la question sautée est la contradiction entre l'affirmation
  et ce que son auteur sait être vrai (« Vous affirmez quelque chose que vous
  savez faux ») ;
- 6.1.2 « Fausse attribution » : la question sautée porte sur la source citée —
  soutient-elle réellement ce qui lui est attribué (« une source fausse, mal
  identifiée, incompétente ou inventée ») ?

Balayage mesuré : les seules rangées du sous-arbre portant D/E/AT sont 6.1,
6.1.1, 6.1.2, 6.1.3 — aucun autre nœud ne porte rien.

Effet attendu : groupe 6.1 65 → 0, arbre 73 → 8 — le RÉSIDU TERMINAL de la
campagne : exactement les 8 têtes exemptées et documentées (0, 2, 2.1, 2.2, 3,
3.3, 5, 6 — têtes de famille sans mécanique propre + têtes nues des grains 3, 4,
7). Voisins 3.2 (0) et 3.3 (1, tête nue) inchangés — tout asserté ci-dessous.

PROSE ONLY : jamais de token AIF fabriqué (les colonnes tokens sont épinglées par
FallacyAifVocabularyLockTests) ; la colonne AIF_criticalQuestion existe depuis le
grain 2 (PR #2194). Corpus : apostrophes U+2019, espace U+0020 avant « ? ».

SURGICAL : byte-exact field-segment splitter, même technique que les grains 2-7
([[csv-byte-exact-column-insertion]]). BOM + CRLF préservés.
Ne met à jour AUCUNE cellule existante — 3 cellules ajoutées, rien d'autre.

GATED : dry-run par défaut ; `--write` applique.

Usage:
    python tools/498-grain8-groupe-61-cq.py            # dry-run + verify
    python tools/498-grain8-groupe-61-cq.py --write    # apply
"""
import csv, io, os, sys

PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
CQ_COL = "AIF_criticalQuestion"
WRITE = "--write" in sys.argv

REF_COLS = ("AIF_skosDirectRef", "AIF_skosExceptionRef", "AIF_skosOther")

# ── les 3 questions posées (keyed by path) ────────────────────────────────────
# Ancrage : desc_fr de chaque nœud + la mécanique portée. Chaque question est à
# deux branches : l'usage honnête d'un côté, le procédé de l'autre.
SAMPLE = {
    "6.1": "La présentation des faits reflète-t-elle ce que la source peut établir, ou l’arrangement tient-il lieu de connaissance qu’il ne fonde pas ?",
    "6.1.1": "L’affirmation correspond-elle à ce que celui qui la formule sait être vrai, ou soutient-il à dessein ce qu’il sait faux ?",
    "6.1.2": "La source invoquée soutient-elle réellement ce qui lui est attribué, ou le poids qu’on lui prête tient-il lieu de preuve qu’elle ne fournit pas ?",
}

CARRIER_HEAD = "6.1"        # tête PORTEUSE : (a) refs + (b) attaque → CQ propre
ALREADY_COVERED = "6.1.3"   # GeneralAcceptanceDoubt_Conflict — jamais de prose dessus
TERMINAL_RESIDUE = ["0", "2", "2.1", "2.2", "3", "3.3", "5", "6"]  # têtes exemptées

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
# garde : la tête doit TOUJOURS porter sa mécanique (a)+(b) — c'est ce qui fonde
# sa CQ propre ; si un futur grain retire ses refs ou son attaque, le placement
# doit être re-dérivé, pas hérité.
head_r = data[paths.index(CARRIER_HEAD)]
assert head_r[header.index("AIF_skosDirectRef")].strip() == "LackOfPTKReliability_Scheme", \
    "carrier head 6.1 lost LackOfPTKReliability_Scheme — placement must be re-derived"
assert head_r[header.index("AIF_skosExceptionRef")].strip() == "PositionToKnow_Inference", \
    "carrier head 6.1 lost PositionToKnow_Inference — placement must be re-derived"
assert head_r[header.index("AIF_attackType")].strip() == "undermine", \
    "carrier head 6.1 lost its attack — placement must be re-derived"
# garde : les deux sous-groupes portent l'attaque seule (aucun schéma) — c'est la
# justification de la descente (question sautée différente, même polarité)
for sg in ("6.1.1", "6.1.2"):
    r = data[paths.index(sg)]
    assert r[header.index("AIF_attackType")].strip() == "undermine", \
        f"{sg} no longer carries undermine — placement must be re-derived"
    assert not any(r[header.index(c)].strip() for c in REF_COLS), \
        f"{sg} now carries refs — placement must be re-derived"
# garde : 6.1.3 doit rester couvert par son Conflict — jamais de prose par-dessus
cov_r = data[paths.index(ALREADY_COVERED)]
assert "GeneralAcceptanceDoubt_Conflict" in cov_r[header.index("AIF_skosDirectRef")], \
    f"{ALREADY_COVERED} lost GeneralAcceptanceDoubt_Conflict — re-derive"
assert not cov_r[CQ_IDX].strip(), \
    f"{ALREADY_COVERED} already carries prose — over an existing (c)?"

print(f"source: {len(data)} rows, CQ col idx {CQ_IDX}, targets {sorted(SAMPLE)} (vierges), "
      f"head {CARRIER_HEAD} porteuse (a)+(b), {ALREADY_COVERED} couvert par Conflict")

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
print(f"couverture AVANT : groupe 6.1 = {len(before_g['6.1'])} sans (c) "
      f"(3.2 = {len(before_g['3.2'])}, 3.3 = {len(before_g['3.3'])}), arbre = {len(before_tree)}")
assert len(before_g["6.1"]) == 65, f"expected 65 uncovered nodes in 6.1, measured {len(before_g['6.1'])}"
assert len(before_tree) == 73, f"expected tree at 73 (grain 7 merged), measured {len(before_tree)}"

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
print(f"couverture APRÈS : groupe 6.1 = {len(after_g['6.1'])} sans (c) {after_g['6.1']} "
      f"(3.2 = {len(after_g['3.2'])}, 3.3 = {len(after_g['3.3'])} inchangés), "
      f"arbre = {len(after_tree)} (avant {len(before_tree)})")
assert after_g["6.1"] == [], \
    f"group 6.1 must be fully covered, still uncovered: {after_g['6.1']}"
assert len(after_g["3.2"]) == len(before_g["3.2"]) and len(after_g["3.3"]) == len(before_g["3.3"]), \
    "neighbouring groups changed — forbidden"
assert sorted(after_tree) == TERMINAL_RESIDUE, \
    f"tree must land on the 8 documented exempt heads, got {sorted(after_tree)}"
print(f"RÉSIDU TERMINAL atteint : {len(after_tree)} = les 8 têtes exemptées "
      f"({', '.join(TERMINAL_RESIDUE)}) — campagne (c) complète sur tout le reste")

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
