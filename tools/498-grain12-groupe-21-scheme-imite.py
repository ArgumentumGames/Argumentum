#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#498 grain 12 — group 2.1 « Procédé rhétorique »: the FIRST writing PR of the
imitated-scheme representation (a). Creates the `AIF_imitatedScheme` column and
writes the four adjudicated nodes of the group (cadrage #2229, arbitrations
ai-01 10/11 c.6775643852 + 11/11 c.6783069610, pool 12/11 c.6810465721).

Base re-measured on `557a9692` BEFORE writing (cadrage §5 anti-drift):
  - group 2.1 subtree = 123 nodes; 2.1.1 (42) already carries (a) → sans-(a) = 80
    = 2.1.2 Humour (28) + 2.1.3 Poésie (52). Reproduces the cadrage §1 table.
  - internal sub-tree sizes 2.1.2.1=11, 2.1.2.2=13, 2.1.2.3=3, 2.1.3=52.
  - the two heterogeneous heads (2.1 group head, 2.1.2 intermediate head) get
    NO write: no single mechanic is true for their whole subtree (cadrage §4).

The four writes (mechanic posed at the HIGHEST node where it is true):
  path 2.1.2.1 Comique  (PK 220): AIF_skosExceptionRef = Deductive_Inference
  path 2.1.2.2 Satire   (PK 231): AIF_skosExceptionRef = Deductive_Inference
      → both + AIF_skosMappingType = skos:broadMatch (grain-10 doctrine,
        c.6775643852: an ExceptionRef target is ALL conflicts of the scheme,
        the node is narrower ⇒ broadMatch; the OWL emission block is gated on
        a non-empty MappingType — a ref without a type would emit nothing).
        `Deductive_Inference` is in the closed native vocabulary (carried by
        4.3/4.2/5.3.1.2); the *_Conflict classes already exist in the OWL ⇒
        zero new IRI (the #133 invariant the counter-review re-measures).
  path 2.1.2.3 Cynisme  (PK 244): AIF_imitatedScheme = FR phrase (cadrage §3,
        verbatim): « imite la défiance méthodique envers les affirmations — la
        dévaluation généralisée tient lieu d'examen »
  path 2.1.3   Poésie   (PK 247): AIF_skosOther = declared gap using the
        EXISTING marker prefix and language (ai-01 10/11: English prefix, not a
        half-French new one): "No native AIF Inference scheme for poetic
        ornament; the effect is stylistic (figures of style), not inferential"

Column layout (mirrors the #1192/#989 insertion pattern, grain-2 Fallacies
precedent tools/498-fallacies-aif-critical-question-column.py):
    before: ..., AIF_skosDirectRef, AIF_skosExceptionRef, AIF_skosOther, ...
    after:  ..., AIF_skosDirectRef, AIF_skosExceptionRef, AIF_imitatedScheme,
             AIF_skosOther, ...
105 → 106 columns. The phrase sits between the (a)-reference and the declared
gap: reference / phrase / gap is the adjudicated representation table order.

Quoting conventions measured on master: scheme tokens are BARE (PK 758:
'Deductive_Inference', 'skos:broadMatch'), prose cells are QUOTED (PK 834
skosOther marker, every filled AIF_criticalQuestion).

SURGICAL: byte-exact field-segment splitter (RFC4180, doubled quotes +
embedded newlines safe — same technique as the grain-2 instrument). No csv
round-trip, no re-quoting: every pre-existing segment is re-emitted verbatim;
only the new column and the six target cells change. BOM + CRLF preserved.

GATED: dry-run by default; `--write` applies.

Usage:
    python tools/498-grain12-groupe-21-scheme-imite.py            # dry-run + verify
    python tools/498-grain12-groupe-21-scheme-imite.py --write    # apply
"""
import csv, io, os, sys

HERE = os.path.dirname(os.path.abspath(__file__))
PATH = os.path.join(HERE, "..", "Cards", "Fallacies",
                    "Argumentum Fallacies - Taxonomy.csv")
NEW_COL = "AIF_imitatedScheme"
WRITE = "--write" in sys.argv

# ── the six writes, keyed by `path` (stable dotted hierarchy key) ────────────
PHRASE_CYNISME = ("imite la défiance méthodique envers les affirmations — "
                  "la dévaluation généralisée tient lieu d'examen")
GAP_POESIE = ("No native AIF Inference scheme for poetic ornament; "
              "the effect is stylistic (figures of style), not inferential")
WRITES = {
    "2.1.2.1": {"AIF_skosExceptionRef": "Deductive_Inference",
                "AIF_skosMappingType": "skos:broadMatch"},
    "2.1.2.2": {"AIF_skosExceptionRef": "Deductive_Inference",
                "AIF_skosMappingType": "skos:broadMatch"},
    "2.1.2.3": {NEW_COL: PHRASE_CYNISME},
    "2.1.3":   {"AIF_skosOther": GAP_POESIE},
}
QUOTE_COLS = {NEW_COL, "AIF_skosOther"}          # prose → quoted (convention)
EXPECT_SUBTREES = {"2.1.1": 42, "2.1.2": 28, "2.1.3": 52,
                   "2.1.2.1": 11, "2.1.2.2": 13, "2.1.2.3": 3}

# ── byte-exact CSV splitters (doubled quotes + embedded newlines safe) ───────
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
assert tail == "", f"unexpected bytes after last row terminator: {tail!r}"
assert "\r\n".join(rows) + "\r\n" == text, "splitter round-trip mismatch on ORIGINAL"

header = split_fields(rows[0])
assert len(header) == 105, f"expected 105 header cols, got {len(header)}"
INSERT_IDX = header.index("AIF_skosOther")
assert header[INSERT_IDX - 1] == "AIF_skosExceptionRef", header[INSERT_IDX - 1]
path_idx = header.index("path")
data = [split_fields(r) for r in rows[1:]]
assert all(len(r) == 105 for r in data), "ragged rows"
assert len(data) == 1408, f"expected 1408 data rows, got {len(data)}"

paths = [r[path_idx].strip() for r in data]
assert len(paths) == len(set(paths)), "duplicate path keys"
missing = [p for p in WRITES if p not in paths]
assert not missing, f"write paths absent from CSV: {missing}"

# ── base re-measure (BEFORE any mutation — cadrage §5 anti-drift) ────────────
parsed_old = list(csv.DictReader(io.StringIO(text)))
def old_col(name):
    return [r[name] for r in parsed_old]
dp = old_col("decimal_path")
for sub, expected in EXPECT_SUBTREES.items():
    # decimal_path form is "<family>,<concatenated sub-levels>" (2.1.1 → '2,11',
    # 2.1.2.1 → '2,121'): family comma, then the sub-level digits concatenated.
    levels = sub.split(".")
    pref = levels[0] + "," + "".join(levels[1:])
    got = sum(1 for d in dp if d.startswith(pref))
    assert got == expected, f"subtree {sub} (pref {pref}): {got} nodes, cadrage says {expected}"
def carries_a(row):
    direct = row["AIF_skosDirectRef"].strip()
    return bool(row["AIF_skosExceptionRef"].strip()) or (
        direct and ("_Inference" in direct or "_Scheme" in direct))
carriers_21 = [r["path"] for r in parsed_old
               if r["decimal_path"].startswith("2,1") and carries_a(r)]
assert carriers_21 == ["2.1.1"], \
    f"(a)-carriers inside 2.1 changed: {carriers_21} (cadrage: only 2.1.1)"
# anti-overwrite: every target cell must be empty on the CURRENT base
for p, cols in WRITES.items():
    row = next(r for r in parsed_old if r["path"] == p)
    for c in cols:
        if c == NEW_COL:
            continue  # the column does not exist on the old base — nothing to overwrite
        assert row[c].strip() == "", f"anti-overwrite: {p}.{c} not empty"

print(f"base OK: 1408 rows, group 2.1 sans-(a) = 80 (2.1.1 carries (a), 42 nodes), "
      f"subtrees {EXPECT_SUBTREES}, all 6 target cells empty")

# ── transform ─────────────────────────────────────────────────────────────────
new_header = header[:INSERT_IDX] + [NEW_COL] + header[INSERT_IDX:]
new_col_idx = {c: i for i, c in enumerate(new_header)}

def encode(col, value):
    if col in QUOTE_COLS:
        assert '"' not in value, "doubled-quote path not exercised — extend encoder"
        return f'"{value}"'
    assert ',' not in value and '"' not in value, "bare segment must stay atomic"
    return value

new_rows = []
for row, p in zip(data, paths):
    out = row[:INSERT_IDX] + [""] + row[INSERT_IDX:]
    if p in WRITES:
        for col, val in WRITES[p].items():
            j = new_col_idx[col]
            assert out[j] == "", f"target {p}.{col} unexpectedly non-empty post-insert"
            out[j] = encode(col, val)
    new_rows.append(",".join(out))
new_text = "\r\n".join([",".join(new_header)] + new_rows) + "\r\n"

# ── verify (runs in dry-run AND after --write) ────────────────────────────────
def verify(t):
    got_rows, got_tail = split_logical_rows(t)
    assert got_tail == ""
    assert "\r\n".join(got_rows) + "\r\n" == t, "splitter round-trip mismatch on NEW"
    assert t.count("\r\n") == text.count("\r\n"), "CRLF count changed"
    got_header = split_fields(got_rows[0])
    assert len(got_header) == 106, len(got_header)
    assert got_header[INSERT_IDX] == NEW_COL, got_header[INSERT_IDX]
    assert got_header[INSERT_IDX - 1] == "AIF_skosExceptionRef"
    assert got_header[INSERT_IDX + 1] == "AIF_skosOther"
    got = [split_fields(r) for r in got_rows[1:]]
    assert len(got) == len(data)
    changed_cells = 0
    new_col_cells = 0
    for old_r, new_r, p in zip(data, got, paths):
        assert len(new_r) == 106
        # byte-preservation: dropping the inserted col must give the old row,
        # except the target cells (which live at indexes != INSERT_IDX on the
        # old columns; the NEW_COL cell itself sits AT INSERT_IDX and is
        # verified separately just below).
        stripped = new_r[:INSERT_IDX] + new_r[INSERT_IDX + 1:]
        if p in WRITES:
            target_idx = {header.index(c) for c in WRITES[p] if c != NEW_COL}
            for i, (a, b) in enumerate(zip(old_r, stripped)):
                if i in target_idx:
                    if a != b:
                        changed_cells += 1
                else:
                    assert a == b, f"byte drift on {p} col {header[i]}"
        else:
            assert stripped == old_r, f"byte drift on row {p}"
        expected_new = encode(NEW_COL, WRITES[p][NEW_COL]) if NEW_COL in WRITES.get(p, {}) else ""
        assert new_r[INSERT_IDX] == expected_new, f"unexpected {NEW_COL} value on {p}"
        if expected_new:
            new_col_cells += 1
    assert changed_cells == 5, f"expected 5 changed old-column cells, got {changed_cells}"
    assert new_col_cells == 1, f"expected 1 filled {NEW_COL} cell, got {new_col_cells}"
    # independent re-parse with the csv module + decoded-value equality
    parsed_new = list(csv.DictReader(io.StringIO(t)))
    assert len(parsed_new) == 1408 and all(len(r) == 106 for r in parsed_new)
    by_path_new = {r["path"]: r for r in parsed_new}
    for r_old in parsed_old:
        r_new = by_path_new[r_old["path"]]
        for k in header:
            if k in WRITES.get(r_old["path"], {}):
                assert r_new[k] == WRITES[r_old["path"]][k], \
                    f"value drift {r_old['path']}.{k}"
            else:
                assert r_new[k] == r_old[k], f"value drift {r_old['path']}.{k}"
        assert r_new[NEW_COL] == WRITES.get(r_old["path"], {}).get(NEW_COL, ""), \
            f"{NEW_COL} drift on {r_old['path']}"
    filled = sum(1 for r in parsed_new if r[NEW_COL].strip())
    assert filled == 1, f"{NEW_COL} filled on {filled} rows (expected 1: 2.1.2.3)"
    return changed_cells

changed_cells = verify(new_text)
print(f"verify OK: 106 cols, {NEW_COL} inserted at idx {INSERT_IDX}, "
      f"{changed_cells}+1 cells written on 4 rows, every other segment "
      f"byte-identical, csv-module re-parse equal, BOM + {text.count(chr(13)+chr(10))} CRLF preserved")

if WRITE:
    out = b'\xef\xbb\xbf' + new_text.encode("utf-8")   # encode FIRST — never inside open()
    tmp = PATH + ".tmp"
    with open(tmp, "wb") as fh:
        fh.write(out)
    os.replace(tmp, PATH)   # atomic — target intact if anything above raised
    print(f"WRITTEN: {os.path.basename(PATH)} ({len(out)} bytes)")
    recheck = open(PATH, "rb").read()[3:].decode("utf-8")
    verify(recheck)
    print("post-write verify OK")
else:
    print("dry-run only — pass --write to apply")
