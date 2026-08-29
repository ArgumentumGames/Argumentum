#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#141 Layer A — transverse cross-link APPLY (GATED, dry-run by default).

Fills the 8 `crossLink_*` columns of `Cards/Fallacies/Argumentum Fallacies -
Taxonomy.csv` from the 0-fabrication scan candidates in
`docs/taxonomy/141-aif-candidates-fullscale.csv`, so the transverse relational
layer (predatesOn / denounces / leverages / allows / opposes / inverts /
mirrors / isRelatedTo) becomes emittable in the OWL (the CSV->OWL wiring was
added alongside this script in Fallacy.cs + OwlGeneratorConfig.cs).

SELECTION (jsboige interactive decision, this chantier):
    kind == crossLink  AND  confidence >= 0.6  AND  warns empty  AND  TRANSVERSE
where TRANSVERSE = target is neither an ancestor nor a descendant of source in
the dotted-path hierarchy (same-branch parent/child links are noise, dropped).
    -> 1210 relations; 1 has a source path absent from the taxonomy
       (5.1.2.3.2.2.4, a scan artefact) -> dropped -> 1209 applicable.

ADDITIVE-ONLY (byte-exact, #760 discipline): writes ONLY into empty source-verb
cells; the 22 manually-seeded cells are preserved untouched (a candidate that
targets a verb-cell already carrying a seed is SKIPPED, never appended/over-
written). Targets are dotted paths (matching fallacy.path), ';'-joined, sorted
hierarchically, max 3/cell. CRLF + BOM + 104-column shape preserved.

GATE: `--write` is off by default. Dry-run proves byte-preservation without
touching prod. Coverage of source nodes with >=1 crossLink: ~21 -> ~59% (833
distinct source nodes over 1408).

    python tools/141-crosslink-apply.py            # dry-run (proof only)
    python tools/141-crosslink-apply.py --write     # APPLY (fills empty cells)
"""
import csv, io, sys
from collections import Counter, defaultdict

TAX = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
CAND = "docs/taxonomy/141-aif-candidates-fullscale.csv"
BACKUP = "tmp/Fallacies-backup-pre-crosslink.csv"
WRITE = "--write" in sys.argv
NCOLS = 104

VERB_TO_COL = {
    "PredatesOn": "crossLink_PredatesOn",
    "Denounces":  "crossLink_Denounces",
    "Leverages":  "crossLink_Leverages",
    "Allows":     "crossLink_Allows",
    "Opposes":    "crossLink_Opposes",
    "Inverts":    "crossLink_Inverts",
    "Mirrors":    "crossLink_Mirrors",
    "IsRelatedTo": "crossLink_IsRelatedTo",
}

# ── byte-exact splitters (CSV-aware: doubled quotes + embedded LF) ─────────────
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
            rows.append(''.join(cur)); cur = []; i += 2
        else:
            cur.append(ch); i += 1
    if cur:
        rows.append(''.join(cur))
    return rows

def split_fields(row):
    segs, cur, in_q = [], [], False
    i, n = 0, len(row)
    while i < n:
        ch = row[i]
        if ch == '"':
            if in_q and i + 1 < n and row[i + 1] == '"':
                cur.append('""'); i += 2
            else:
                in_q = not in_q; cur.append(ch); i += 1
        elif ch == ',' and not in_q:
            segs.append(''.join(cur)); cur = []; i += 1
        else:
            cur.append(ch); i += 1
    segs.append(''.join(cur))
    return segs

def path_key(p):
    try:
        return [int(x) for x in p.split('.')]
    except ValueError:
        return [10**9, p]  # non-numeric paths sort last, deterministically

# ── read current CSV ───────────────────────────────────────────────────────────
raw = open(TAX, "rb").read()
bom = raw[:3] == b'\xef\xbb\xbf'
text = (raw[3:] if bom else raw).decode('utf-8')
ended_crlf = text.endswith('\r\n')
rows = split_logical_rows(text)
header = split_fields(rows[0])
assert len(header) == NCOLS, f"header has {len(header)} cols, expected {NCOLS}"
PI = header.index('path')
COL = {v: header.index(c) for v, c in VERB_TO_COL.items()}
CL_IDX = set(COL.values())
assert header[82] == 'crossLink_PredatesOn' and header[89] == 'crossLink_IsRelatedTo', "crossLink block moved?"

paths = set(split_fields(r)[PI].strip() for r in rows[1:] if r.strip())

# ── load + filter candidates ─────────────────────────────────────────────────
def is_anc(a, b):
    return b == a or b.startswith(a + '.')

with open(CAND, encoding='utf-8-sig') as f:
    cand = [r for r in csv.DictReader(f) if r['kind'] == 'crossLink']

sel = [r for r in cand
       if float(r['confidence']) >= 0.6
       and not r['warns'].strip()
       and not (is_anc(r['source_dotted'].strip(), r['target'].strip())
                or is_anc(r['target'].strip(), r['source_dotted'].strip()))]
transverse_total = len(sel)

dropped_bad_src = sorted({r['source_dotted'].strip() for r in sel if r['source_dotted'].strip() not in paths})
sel = [r for r in sel if r['source_dotted'].strip() in paths]
# every target must exist (0 fabrication) — assert, don't silently drop
bad_tgt = sorted({r['target'].strip() for r in sel if r['target'].strip() not in paths})
assert not bad_tgt, f"ABORT: candidate targets absent from taxonomy (fabrication): {bad_tgt[:10]}"

# group (source_path, verb) -> sorted unique targets, drop self-loops
grp = defaultdict(set)
for r in sel:
    s, v, t = r['source_dotted'].strip(), r['verb_or_field'].strip(), r['target'].strip()
    if v not in VERB_TO_COL:
        raise SystemExit(f"unknown verb {v!r}")
    if t == s:
        continue
    grp[(s, v)].add(t)
groups = {k: ';'.join(sorted(v, key=path_key)) for k, v in grp.items()}

# ── apply: fill EMPTY source-verb cells only (preserve seeds) ─────────────────
new_rows = [rows[0]]
filled_cells = {}          # (row_index, col_index) -> new value  (for byte proof)
skipped_seed = []          # (path, verb, seed, candidates) collisions
per_verb = Counter()
relations_applied = 0
for ri, rtext in enumerate(rows[1:], start=1):
    s = split_fields(rtext)
    p = s[PI].strip()
    for v, col in COL.items():
        key = (p, v)
        if key not in groups:
            continue
        if s[col].strip():
            skipped_seed.append((p, v, s[col], groups[key]))
            continue
        s[col] = groups[key]
        filled_cells[(ri, col)] = groups[key]
        per_verb[v] += 1
        relations_applied += groups[key].count(';') + 1
    new_rows.append(",".join(s))
new_text = "\r\n".join(new_rows) + ("\r\n" if ended_crlf else "")

# ── byte-preservation proof (only filled crossLink cells may differ) ──────────
new_rows2 = split_logical_rows(new_text)
mismatches = 0
for i in range(len(rows)):
    o = split_fields(rows[i]); n = split_fields(new_rows2[i])
    assert len(o) == len(n) == NCOLS, f"row {i} col count drift ({len(o)}/{len(n)})"
    for j in range(NCOLS):
        if o[j] != n[j] and (i, j) not in filled_cells:
            mismatches += 1
            if mismatches <= 5:
                print(f"  MISMATCH row {i} path {o[PI].strip()!r} col {j} ({header[j]}): {o[j]!r} -> {n[j]!r}")
# every changed cell must be a crossLink column (never text/desc/etc.)
non_cl = [(i, j) for (i, j) in filled_cells if j not in CL_IDX]
assert not non_cl, f"ABORT: filled a non-crossLink column: {non_cl[:5]}"

# well-formedness re-parse
chk = list(csv.reader(io.StringIO(new_text)))
assert len(chk) == len(rows) and all(len(r) == NCOLS for r in chk), "well-formedness"

# ── coverage (nodes with >=1 non-empty crossLink cell) ────────────────────────
def coverage(rowset):
    cov = set()
    for r in rowset[1:]:
        s = split_fields(r)
        if any(s[c].strip() for c in CL_IDX):
            cov.add(s[PI].strip())
    return cov
cov_before = coverage(rows)
cov_after = coverage(new_rows)

# ── report ─────────────────────────────────────────────────────────────────────
print("=" * 72)
print(f"#141 LAYER A — CROSSLINK APPLY  (write={WRITE})")
print("=" * 72)
print(f"transverse conf>=0.6 no-warns: {transverse_total} relations")
print(f"dropped (source path absent from taxonomy): {len(dropped_bad_src)} {dropped_bad_src}")
print(f"seed collisions SKIPPED (cell already seeded, additive-only): {len(skipped_seed)}")
for c in skipped_seed[:5]:
    print(f"    path {c[0]} {c[1]}: seed={c[2]!r} kept, candidates {c[3]!r} skipped")
print(f"cells filled (empty source-verb cells): {len(filled_cells)}  ->  per-verb {dict(per_verb)}")
print(f"relations applied (targets written): {relations_applied}")
print(f"byte-preservation mismatches OUTSIDE filled crossLink cells: {mismatches} (must be 0)")
print(f"changed non-crossLink columns: {len(non_cl)} (must be 0)")
print(f"well-formedness: {len(chk)} rows x {NCOLS} cols, CRLF({ended_crlf})+BOM({bom}) preserved")
print(f"delta if written: {len(new_text) - len(text)} bytes")
print(f"coverage (nodes with >=1 crossLink): {len(cov_before)} -> {len(cov_after)} "
      f"({100*len(cov_before)/1408:.1f}% -> {100*len(cov_after)/1408:.1f}% of 1408)")
print(f"seeds preserved: {len(cov_before)} seeded nodes still present: "
      f"{cov_before.issubset(cov_after)}")
assert cov_before.issubset(cov_after), "ABORT: a seeded node lost its crossLink"
if WRITE:
    import os
    os.makedirs("tmp", exist_ok=True)
    open(BACKUP, "wb").write(raw)
    payload = (b'\xef\xbb\xbf' if bom else b'') + new_text.encode('utf-8')  # encode FIRST
    tmp = TAX + ".tmp"
    with open(tmp, "wb") as fh:
        fh.write(payload)
    os.replace(tmp, TAX)    # atomic — target intact if encode/write raised (write-safety #498)
    print(f">>> WRITTEN ({len(filled_cells)} cells). Backup: {BACKUP} for independent verify.")
else:
    print(">>> DRY-RUN (pass --write to APPLY).")
