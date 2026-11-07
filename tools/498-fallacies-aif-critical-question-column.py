#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#498 grain 2, PR 1 — give the Fallacies taxonomy the `AIF_criticalQuestion`
column the Virtues have had since #989/#1192 (`b670073f`).

The Fallacies CSV has no home for the Walton critical question a fallacy SKIPS
(component (c) of the #498 generative rule): `AIF_skosOther` is reserved for
declared gaps (the three-state census depends on it) and the token columns are
pinned by `FallacyAifVocabularyLockTests`. A question AIF does not name is
PROSE — it lives here, never as a fabricated `*_Conflict` token.

    before: ..., AIF_skosOther, AIF_skosMappingType, AIF_attackType, ...
    after:  ..., AIF_skosOther, AIF_criticalQuestion, AIF_skosMappingType, ...

Column position mirrors the Virtues layout exactly (between AIF_skosOther and
AIF_skosMappingType, #989). The 10 sample values below are the articulation
sample of grappe 6.3 « Raisonnement biaisé » (head + children) required by the
first #498 group PR (ai-01, 05/11, issue #498 c.6663438132 / 06/11 c.6678039968).

SURGICAL: byte-exact field-segment splitter (same technique as
tools/989-virtues-aif-critical-question-move.py / memory
[[csv-byte-exact-column-insertion]]). No csv round-trip, no re-quoting: every
pre-existing segment is re-emitted verbatim; only the new column is added.
BOM + CRLF preserved.

GATED: dry-run by default; `--write` applies.

Usage:
    python tools/498-fallacies-aif-critical-question-column.py            # dry-run + verify
    python tools/498-fallacies-aif-critical-question-column.py --write    # apply
"""
import csv, io, os, sys

PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
NEW_COL = "AIF_criticalQuestion"
WRITE = "--write" in sys.argv

# ── the 10-node articulation sample of grappe 6.3 (head + children) ───────────
# Keyed by `path` (stable dotted hierarchy key, see #498 grain 1b). Each value
# is the critical question the fallacy skips, anchored on the node's own AIF
# modelling (scheme refs for the modelled nodes, the grappe head's undermine
# mechanic for the attack-only heads, desc_fr for the first-time articulations).
# Corpus typography: U+2019 apostrophes, plain space before « ? » (measured on
# the Virtues AIF_criticalQuestion column).
SAMPLE = {
    "6.3":      "La conclusion repose-t-elle sur des raisons, plutôt que sur un biais qui a orienté le choix des informations ?",
    "6.3.1":    "Ce qui semble évident a-t-il été vérifié, ou n’est-ce qu’un réflexe naturel de la pensée ?",
    "6.3.1.1":  "Les informations négligées parce qu’elles n’intéressent pas ont-elles été cherchées avant de conclure ?",
    "6.3.1.2":  "Les alternatives sont-elles réellement exclusives, ou l’émotion n’en fait-t-elle voir qu’une seule ?",
    "6.3.2":    "La règle suivie vaut-elle au-delà du groupe qui la partage, ou seulement à l’intérieur de lui ?",
    "6.3.2.1":  "Le cas observé est-il représentatif, ou n’est-il qu’un exemple choisi par la commodité de l’explication ?",
    "6.3.2.2":  "Le trait présenté comme naturel a-t-il été démontré, ou l’est-il seulement parce que le groupe le dit ?",
    "6.3.2.3":  "Le point de vue personnel est-il distingué du fait établi, et vérifié par autre chose que lui-même ?",
    "6.3.3":    "Le concept théorique s’applique-t-il à la situation, ou la situation est-elle pliée pour entrer dans le concept ?",
    "6.3.3.1":  "La croyance de départ accepte-t-elle d’être confrontée à des preuves contraires ?",
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
    return rows, tail  # tail = bytes after the last row terminator

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
assert len(header) == 104, f"expected 104 header cols, got {len(header)}"
INSERT_IDX = header.index("AIF_skosMappingType")
assert header[INSERT_IDX - 1] == "AIF_skosOther", header[INSERT_IDX - 1]
path_idx = header.index("path")
data = [split_fields(r) for r in rows[1:]]
assert all(len(r) == 104 for r in data), "ragged rows"

paths = [r[path_idx].strip() for r in data]
assert len(paths) == len(set(paths)), "duplicate path keys"
missing = [p for p in SAMPLE if p not in paths]
assert not missing, f"sample paths absent from CSV: {missing}"

print(f"source: {len(data)} data rows, insertion at idx {INSERT_IDX} "
      f"(between AIF_skosOther and AIF_skosMappingType)")

# ── transform ─────────────────────────────────────────────────────────────────
def segment(row, question):
    # always quote the filled values (they contain commas) — RFC4180, doubled
    # quotes if ever needed; empty stays bare like the surrounding empty cols.
    q = f'"{question}"' if question else ""
    return ",".join(row[:INSERT_IDX] + [q] + row[INSERT_IDX:])

new_header = header[:INSERT_IDX] + [NEW_COL] + header[INSERT_IDX:]
new_rows = [segment(r, SAMPLE.get(p.strip(), "")) for r, p in zip(data, paths)]
new_text = "\r\n".join([",".join(new_header)] + new_rows) + "\r\n"

# ── verify (runs in dry-run AND after --write) ────────────────────────────────
def verify(t):
    got_rows, got_tail = split_logical_rows(t)
    assert got_tail == ""
    assert "\r\n".join(got_rows) + "\r\n" == t, "splitter round-trip mismatch on NEW"
    got_header = split_fields(got_rows[0])
    assert len(got_header) == 105, len(got_header)
    assert got_header[INSERT_IDX] == NEW_COL, got_header[INSERT_IDX]
    assert got_header[INSERT_IDX - 1] == "AIF_skosOther"
    assert got_header[INSERT_IDX + 1] == "AIF_skosMappingType"
    got = [split_fields(r) for r in got_rows[1:]]
    assert len(got) == len(data)
    filled = 0
    for old_r, new_r, p in zip(data, got, paths):
        assert len(new_r) == 105
        # byte-preservation: every pre-existing segment identical, verbatim
        assert new_r[:INSERT_IDX] == old_r[:INSERT_IDX]
        assert new_r[INSERT_IDX + 1:] == old_r[INSERT_IDX:]
        v = new_r[INSERT_IDX]
        if p in SAMPLE:
            assert v == f'"{SAMPLE[p]}"', f"value drift on {p}: {v!r}"
            filled += 1
        else:
            assert v == "", f"unexpected value outside the sample ({p}): {v!r}"
    assert filled == len(SAMPLE), filled
    # independent re-parse with the csv module + decoded-value equality
    parsed = list(csv.reader(io.StringIO(t, newline="")))
    assert len(parsed) == 1 + len(data) and all(len(r) == 105 for r in parsed)
    h = parsed[0]
    cq_col, p_col = h.index(NEW_COL), h.index("path")
    got_cq = {r[p_col].strip(): r[cq_col] for r in parsed[1:]}
    for k, v in SAMPLE.items():
        assert got_cq[k] == v, f"csv-module value differs on {k}: {got_cq[k]!r}"
    assert sum(1 for v in got_cq.values() if v.strip()) == len(SAMPLE), \
        "more filled critical questions than the sample"
    sm = [r[h.index("AIF_skosMappingType")] for r in parsed[1:]]
    src_sm = [r[INSERT_IDX] for r in data]  # source AIF_skosMappingType segments
    assert len([v for v in sm if v.strip()]) == len([v for v in src_sm if v.strip()]), \
        "AIF_skosMappingType fill count changed — this PR must not touch the token column"
    return filled

filled = verify(new_text)
print(f"verify OK: 105 cols, {NEW_COL} filled={filled}/{len(data)}, every other "
      f"segment byte-identical, quoting preserved, csv-module re-parse equal")

if WRITE:
    out = b'\xef\xbb\xbf' + new_text.encode("utf-8")   # encode FIRST — never inside open()
    tmp = PATH + ".tmp"
    with open(tmp, "wb") as fh:
        fh.write(out)
    os.replace(tmp, PATH)   # atomic — target intact if anything above raised
    print(f"WRITTEN: {PATH} ({len(out)} bytes)")
    recheck = open(PATH, "rb").read()[3:].decode("utf-8")
    verify(recheck)
    print("post-write verify OK")
else:
    print("dry-run only — pass --write to apply")
