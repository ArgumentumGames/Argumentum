#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#498 grain 13 — groupe 2.2 « Appel à l'émotion » : écriture du schéma imité (a).

Deuxième PR de groupe du plan « grains 12 et suivants » (ai-01, 11/11 + dispatch
13:09 du 12/11). Le groupe 2.2 compte UN seul nœud sans (a) : la tête 2.2 elle-même
(cadrage §1/§3.2.2). La colonne `AIF_imitatedScheme` existe déjà (créée au grain 12,
#2239) — ce grain est UNE seule écriture de cellule, aucun changement C# :

    path 2.2 -> AIF_imitatedScheme = « imite l'appel à l'émotion pertinent et fondé
    — l'émotion proportionnée à un enjeu établi (Walton : fear appeal, appeal to pity) »

La PHRASE EST EXTRAITE DU CADRAGE MERGÉ (docs/ontology/498-grain9-cadrage-
scheme-imite-2026-11-10.md, ligne « | 2.2 tête ») au moment de l'exécution —
source de vérité unique, zéro risque de dérive de transcription (U+2019, U+2014,
espaces françaises). Justification (cadrage) : les trois enfants portent déjà
chacun son scheme à conséquences (2.2.1 PositiveConsequences, 2.2.2
NegativeConsequences, 2.2.3 EstablishedRule) et le seul scheme émotionnel AIF
(FearAppeal_Inference) est déjà porté par 2.2.2.4 — la tête ne peut ni les
contredire ni dupliquer : la phrase nomme le scheme parent waltonien.

SURGICAL: byte-exact field-segment splitter ([[csv-byte-exact-column-insertion]]).
Aucun csv round-trip ; chaque rangée ré-émise verbatim sauf LA cellule visée.
BOM+CRLF. Anti-overwrite : la cellule doit être VIDE sur la base.

GATED: dry-run par défaut ; `--write` applique.
"""
import csv, io, os, re, sys

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))  # repo root
PATH = os.path.join(ROOT, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")
CADRAGE = os.path.join(ROOT, "docs", "ontology", "498-grain9-cadrage-scheme-imite-2026-11-10.md")
TARGET_PATH, TARGET_COL = "2.2", "AIF_imitatedScheme"
WRITE = "--write" in sys.argv

# ── phrase extraite du cadrage (source de vérité), jamais recopiée à la main ──
doc = open(CADRAGE, encoding="utf-8").read()
m = re.search(r"^\|\s*2\.2 tête \(1\).*?\*\*phrase\*\* :\s*«\s*(.*?)\s*»", doc, re.M)
assert m, "ligne « 2.2 tête » introuvable dans le cadrage — re-mesurer la base"
PHRASE = m.group(1)
assert "Walton" in PHRASE and "émotion" in PHRASE, f"extraction suspecte: {PHRASE!r}"
print(f"phrase extraite du cadrage ({len(PHRASE)} car.) : {PHRASE[:60]!r}…")

# ── splitters byte-exact (identiques au grain 12 / #2194) ─────────────────────
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
    return rows, ''.join(cur)

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
assert tail == "" and "\r\n".join(rows) + "\r\n" == text, "splitter round-trip mismatch on ORIGINAL"

header = split_fields(rows[0])
assert len(header) == 106, f"expected 106 header cols (post-grain-12), got {len(header)}"
COL_IDX = header.index(TARGET_COL)  # must EXIST now — created by grain 12
assert header[COL_IDX - 1] == "AIF_skosExceptionRef", header[COL_IDX - 1]
path_idx = header.index("path")
data = [split_fields(r) for r in rows[1:]]
assert all(len(r) == 106 for r in data), "ragged rows"

paths = [r[path_idx].strip() for r in data]
assert TARGET_PATH in paths, "path 2.2 absent de la base"
ti = paths.index(TARGET_PATH)
cur = data[ti][COL_IDX]
assert cur == "", f"anti-overwrite : 2.2/{TARGET_COL} NON VIDE ({cur[:60]!r}) — re-mesurer la base"

filled_before = sum(1 for r in data if r[COL_IDX].strip())
print(f"source : {len(data)} rangées, 106 cols ; {TARGET_COL} rempli = {filled_before} avant écriture")

# ── transform : UNE cellule ───────────────────────────────────────────────────
new_seg = f'"{PHRASE}"'  # prose quotée (convention colonnes prose ; contient une virgule)
new_data = [list(r) for r in data]
new_data[ti][COL_IDX] = new_seg
new_text = "\r\n".join([",".join(header)] + [",".join(r) for r in new_data]) + "\r\n"

# ── verify ────────────────────────────────────────────────────────────────────
def verify(t, orig_text):
    gr, gt = split_logical_rows(t)
    assert gt == "" and "\r\n".join(gr) + "\r\n" == t, "splitter round-trip mismatch on NEW"
    gh = split_fields(gr[0])
    assert gh == header, "header changé"
    got = [split_fields(r) for r in gr[1:]]
    assert len(got) == len(data)
    # raw byte-preservation : chaque rangée identique SAUF la cible
    for old_r, new_r, p in zip(data, got, paths):
        want = new_seg if p == TARGET_PATH else old_r[COL_IDX]
        assert new_r[COL_IDX] == want, f"segment drift sur {p}"
        assert new_r[:COL_IDX] == old_r[:COL_IDX] and new_r[COL_IDX + 1:] == old_r[COL_IDX + 1:], \
            f"autre colonne touchée sur {p}"
    # decoded equality sur TOUTES les colonnes, csv module des deux côtés
    op = list(csv.reader(io.StringIO(orig_text, newline="")))
    np_ = list(csv.reader(io.StringIO(t, newline="")))
    assert len(op) == len(np_)
    n_filled = 0
    for o_row, n_row in zip(op[1:], np_[1:]):
        assert o_row[path_idx].strip() == n_row[path_idx].strip(), "path déplacé"
        for ci in range(len(o_row)):
            want = PHRASE if (o_row[path_idx].strip() == TARGET_PATH and ci == COL_IDX) else o_row[ci]
            assert n_row[ci] == want, \
                f"decoded drift {o_row[path_idx].strip()}/{header[ci]}: {o_row[ci]!r} -> {n_row[ci]!r}"
        if n_row[COL_IDX].strip():
            n_filled += 1
    assert n_filled == filled_before + 1, (n_filled, filled_before)
    assert np_[ti + 1][COL_IDX] == PHRASE, "valeur décodée différente de la phrase du cadrage"
    return n_filled

n_filled = verify(new_text, text)
print(f"verify OK : 1 cellule écrite (2.2/{TARGET_COL}), {n_filled} remplis au total, "
      f"toutes les autres cellules byte-identiques + decoded-equal")

if WRITE:
    out = b'\xef\xbb\xbf' + new_text.encode("utf-8")
    tmp = PATH + ".tmp"
    with open(tmp, "wb") as fh:
        fh.write(out)
    os.replace(tmp, PATH)
    print(f"WRITTEN: {PATH} ({len(out)} bytes)")
    verify(open(PATH, "rb").read()[3:].decode("utf-8"), text)
    print("post-write verify OK")
else:
    print("dry-run only — pass --write to apply")
