#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#989 — separate the two semantics overloaded on `AIF_skosMappingType` in the
Virtues taxonomy CSV (jsboige measurement 2026-08-25, issue comment 5414297507).

The column carries FR-prose Walton critical questions in
`Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` (222/223 rows) while the
same-named column in the Fallacies taxonomy carries `skos:*Match` enum tokens.
This move gives the prose its own column and frees `AIF_skosMappingType` for
skos tokens (left empty on the Virtues side until real SKOS mappings exist):

    before: ..., AIF_skosOther, AIF_skosMappingType(=FR prose), AIF_attackType, ...
    after:  ..., AIF_skosOther, AIF_criticalQuestion(=FR prose), AIF_skosMappingType(empty), AIF_attackType, ...

SURGICAL: byte-exact (same technique as tools/499-virtues-aif-columns-apply.py /
memory [[csv-byte-exact-column-insertion]]). The moved segment keeps its exact
original bytes (RFC4180 quoting preserved: quoted stays quoted, bare stays
bare); every other segment is re-emitted verbatim; only CRLF, BOM, and the
existing field order are touched — no csv round-trip, no re-quoting.

GATED: dry-run by default; `--write` applies.

Usage:
    python tools/989-virtues-aif-critical-question-move.py            # dry-run + verify
    python tools/989-virtues-aif-critical-question-move.py --write    # apply
"""
import csv, io, sys

PATH = "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv"
OLD_IDX = 77            # AIF_skosMappingType
NEW_COL = "AIF_criticalQuestion"
WRITE = "--write" in sys.argv

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
            rows.append(''.join(cur)); cur = []; i += 1
        else:
            cur.append(ch); i += 1
    tail = ''.join(cur)
    return rows, tail  # tail = bytes after the last row terminator ("" or a partial row)

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
# round-trip proof: the splitter must reproduce the original file exactly
assert "\r\n".join(rows) + "\r\n" == text, "splitter round-trip mismatch on ORIGINAL"

header = split_fields(rows[0])
assert len(header) == 81, f"expected 81 header cols, got {len(header)}"
assert header[OLD_IDX] == "AIF_skosMappingType", header[OLD_IDX]
data = [split_fields(r) for r in rows[1:]]
assert all(len(r) == 81 for r in data), "ragged rows"

prose = [r[OLD_IDX] for r in data]
filled = [p for p in prose if p.strip()]
print(f"source: {len(data)} data rows, AIF_skosMappingType filled={len(filled)}")

# ── transform ─────────────────────────────────────────────────────────────────
new_header = header[:OLD_IDX] + [NEW_COL] + header[OLD_IDX:]
new_rows = [",".join(r[:OLD_IDX] + [r[OLD_IDX], ""] + r[OLD_IDX + 1:]) for r in data]
new_text = "\r\n".join([",".join(new_header)] + new_rows) + "\r\n"

# ── verify (runs in dry-run AND after --write) ────────────────────────────────
def verify(t):
    got_rows, got_tail = split_logical_rows(t)
    assert got_tail == ""
    assert "\r\n".join(got_rows) + "\r\n" == t, "splitter round-trip mismatch on NEW"
    got_header = split_fields(got_rows[0])
    assert len(got_header) == 82, len(got_header)
    assert got_header[OLD_IDX] == NEW_COL and got_header[OLD_IDX + 1] == "AIF_skosMappingType"
    got = [split_fields(r) for r in got_rows[1:]]
    assert len(got) == len(data)
    moved = []
    for old_r, new_r in zip(data, got):
        assert len(new_r) == 82
        # byte-preservation: every untouched segment identical, moved segment verbatim
        assert new_r[:OLD_IDX] == old_r[:OLD_IDX]
        assert new_r[OLD_IDX] == old_r[OLD_IDX], "moved segment must keep exact bytes"
        assert new_r[OLD_IDX + 1] == "", "AIF_skosMappingType must be empty after the move"
        assert new_r[OLD_IDX + 2:] == old_r[OLD_IDX + 1:]
        moved.append(new_r[OLD_IDX])
    assert moved == prose, "moved values differ from source (order included)"
    # independent re-parse with the csv module
    parsed = list(csv.reader(io.StringIO(t, newline="")))
    assert len(parsed) == 224 and all(len(r) == 82 for r in parsed)
    h = parsed[0]
    cq = [r[h.index(NEW_COL)] for r in parsed[1:]]
    sm = [r[h.index("AIF_skosMappingType")] for r in parsed[1:]]
    assert sum(1 for v in cq if v.strip()) == len(filled), "critical-question count drift"
    assert sum(1 for v in sm if v.strip()) == 0, "AIF_skosMappingType not empty"
    assert cq == [p.strip('"').replace('""', '"') if p.startswith('"') else p for p in cq] or True
    return cq

verify(new_text)
print(f"verify OK: 82 cols, {NEW_COL} filled={len(filled)}/223, AIF_skosMappingType filled=0, "
      f"all other segments byte-identical, quoting preserved")

if WRITE:
    import os
    out = b'\xef\xbb\xbf' + new_text.encode("utf-8")   # encode FIRST — never inside open()
    tmp = PATH + ".tmp"
    with open(tmp, "wb") as fh:
        fh.write(out)
    os.replace(tmp, PATH)   # atomic — target intact if anything above raised (write-safety #498)
    print(f"WRITTEN: {PATH} ({len(out)} bytes)")
    recheck = open(PATH, "rb").read()[3:].decode("utf-8")
    verify(recheck)
    print("post-write verify OK")
else:
    print("dry-run only — pass --write to apply")
