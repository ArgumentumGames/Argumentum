#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#498 P2 reconciliation APPLY — deep-serialize the 3 skos columns (+Other) of the
46 'Abus de langage' attack-only rows (GATED — dry-run default).

Companion to `docs/taxonomy/498-reconciliation-p2.md` (proposition) and its
machine-readable `docs/taxonomy/498-reconciliation-p2-annotations.csv`. Mirrors
`tools/498-p1g-apply.py` byte-exact cell-fill machinery, but in the MIRROR
direction of the P1 series:

  P1 tranches (1..1g): attack back-fill of skos-only rows  (attack 93 -> 145,
                        fully-modeled 18 -> 70, skos-only -> 0, CLOSED).
  P2 (this):           skos deep-serialize of attack-only rows (census §5
                        Priorite 2 — "sérialisation d'un contenu vetté, pas de
                        re-modélisation"). Content transcribed VERBATIM from the
                        ratified cluster docs PR-1..PR-12 (explicit per-leaf
                        Proposal lines; 0 re-modeling, 0 novel token — every
                        token verified against the 60-token in-prod native
                        whitelist, #677).

46 rows = the COMPLETE 'Abus de langage' attack-only population (measured on
master c7634007): vague-definition 3 (801-803) + arbitrary-definition 3 (805-807)
+ inconsistent-definition 5 (826-831) + faulty-comparison 3 (834/836/838) +
fallacious-comparison 3 (841-843) + association-fallacy 2 (844/845) +
amphibologie 6 (847-854) + equivoque-polysemie 5 (855-864) + equivoque-residual
4 (857-866) + equivoque-reification 2 (867/875) + narrative-insinuation 4
(877-880) + narrative-deception 6 (881-886).

Shapes (per doc proposals, NOT uniform): 25 direct-conflict (Dir+Map) / 19 full
(Dir+Exc+Map) / 2 exception (Exc+Map: 847 amphibologie syntactic-gap, 859
sorite). Two leaves carry doc-flagged partial FAIL-LOUD shapes serialized as
proposed (834 DirectRef-only; 847 ExceptionRef-only) — see proposition §3.

NOTE (gate): this script lives in docs/taxonomy/ because the ai-01 GO scopes
repo writes to that directory; at serialization time it can be moved to tools/
to join the 498-p1*-apply.py family (it imports nothing location-specific).

    python docs/taxonomy/498-reconciliation-p2-apply.py            # dry-run, 0 write
    python docs/taxonomy/498-reconciliation-p2-apply.py --write    # APPLY 46x3 cells (GATED)
"""
import csv, io, sys, os
from collections import Counter

PATH   = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
ANNOT  = "docs/taxonomy/498-reconciliation-p2-annotations.csv"
BACKUP = "tmp/Fallacies-backup-pre-p2.csv"
WRITE  = "--write" in sys.argv

TARGET_COLS = ("AIF_skosDirectRef", "AIF_skosExceptionRef", "AIF_skosMappingType", "AIF_skosOther")

# ── byte-exact splitters (CSV-aware: doubled quotes + embedded LF) ─────────────
def split_logical_rows(text):
    rows, cur, in_q = [], [], False
    i, n = 0, len(text)
    while i < n:
        ch = text[i]
        if ch == '"':
            if in_q and i+1 < n and text[i+1] == '"':
                cur.append('""'); i += 2
            else:
                in_q = not in_q; cur.append(ch); i += 1
        elif ch == '\r' and not in_q and i+1 < n and text[i+1] == '\n':
            rows.append(''.join(cur)); cur = []; i += 2
        else:
            cur.append(ch); i += 1
    if cur: rows.append(''.join(cur))
    return rows

def split_fields(row):
    segs, cur, in_q = [], [], False
    i, n = 0, len(row)
    while i < n:
        ch = row[i]
        if ch == '"':
            if in_q and i+1 < n and row[i+1] == '"':
                cur.append('""'); i += 2
            else:
                in_q = not in_q; cur.append(ch); i += 1
        elif ch == ',' and not in_q:
            segs.append(''.join(cur)); cur = []; i += 1
        else:
            cur.append(ch); i += 1
    segs.append(''.join(cur))
    return segs

# ── load-bearing: the annotation CSV (human-reviewed source) IS the map ────────
P2_MAP = {}   # pk -> {col: value}
with open(ANNOT, encoding="utf-8-sig") as fh:
    for row in csv.DictReader(fh):
        pk = row["fallacy_pk"].strip()
        P2_MAP[pk] = {c: row[c].strip() for c in TARGET_COLS}
assert len(P2_MAP) == 46, f"expected 46 annotated rows, got {len(P2_MAP)}"
# every row must propose at least one non-empty skos cell (no full fail-loud rows here)
empty_all = [pk for pk, m in P2_MAP.items() if not any(m.values())]
assert not empty_all, f"annotation rows proposing NOTHING: {empty_all}"
# native-token whitelist (in-prod usage) — 0 fabrication (#677)
def clean_tokens(seg):
    """Strip the CSV field quote-pair (multi-token cells are `"A, B"` in prod) then
    split on comma/space. split_fields preserves the quote chars inside a segment,
    so without this a quoted cell yields `'"A'`/`'B"'` and loses the bare tokens."""
    seg = seg.strip()
    if len(seg) >= 2 and seg[0] == '"' and seg[-1] == '"':
        seg = seg[1:-1]
    return [t.strip() for t in seg.replace(",", " ").split() if t.strip()]

raw0 = open(PATH, "rb").read()
text0 = (raw0[3:] if raw0[:3] == b'\xef\xbb\xbf' else raw0).decode('utf-8')
rows0 = [split_fields(r) for r in split_logical_rows(text0)]
h0 = rows0[0]
native = set()
for r in rows0[1:]:
    for ci in (h0.index('AIF_skosDirectRef'), h0.index('AIF_skosExceptionRef')):
        native.update(clean_tokens(r[ci]))
novel = {t for m in P2_MAP.values() for c in ("AIF_skosDirectRef", "AIF_skosExceptionRef")
         for t in m[c].replace(",", " ").split() if t.strip() and t.strip() not in native}
assert not novel, f"NOVEL TOKENS — fabrication interdite (#677): {novel}"

# ── read current CSV ───────────────────────────────────────────────────────────
raw = open(PATH, "rb").read()
bom = raw[:3] == b'\xef\xbb\xbf'
text = (raw[3:] if bom else raw).decode('utf-8')
ended_crlf = text.endswith('\r\n')
rows = split_logical_rows(text)
header = split_fields(rows[0])
NCOL = len(header)
PKI = 0
IDX = {c: header.index(c) for c in TARGET_COLS}
ATI = header.index('AIF_attackType')

# ── pre-state: every target PK must be attack-typed (non-empty) AND skos-empty ─
pre = {}
for r in rows[1:]:
    s = split_fields(r); pk = s[PKI].strip()
    if pk in P2_MAP:
        pre[pk] = (s[ATI].strip(), any(s[IDX[c]].strip() for c in TARGET_COLS))
assert set(pre) == set(P2_MAP), f"target PKs missing in CSV: {set(P2_MAP)-set(pre)}"
not_typed = {pk for pk,(at,_) in pre.items() if not at}
assert not not_typed, f"ABORT: target PKs not attack-typed (not attack-only deep-serialize): {not_typed}"
has_skos = {pk for pk,(_,hs) in pre.items() if hs}
assert not has_skos, f"ABORT: target PKs already carry skos (would overwrite): {has_skos}"

# ── apply: cell-fill byte-exact (only the 46 rows x the 4 skos columns) ────────
new_rows = [rows[0]]
shapes = Counter()
for rtext in rows[1:]:
    s = split_fields(rtext); pk = s[PKI].strip()
    if pk in P2_MAP:
        for c in TARGET_COLS:
            if P2_MAP[pk][c]:
                s[IDX[c]] = P2_MAP[pk][c]
        d, e = P2_MAP[pk]["AIF_skosDirectRef"], P2_MAP[pk]["AIF_skosExceptionRef"]
        shapes["full" if d and e else ("direct-conflict" if d else "exception")] += 1
    new_rows.append(",".join(s))
new_text = "\r\n".join(new_rows) + ("\r\n" if ended_crlf else "")

# ── byte-preservation proof ────────────────────────────────────────────────────
new_rows2 = split_logical_rows(new_text)
mismatches = 0
for i in range(len(rows)):
    o = split_fields(rows[i]); n = split_fields(new_rows2[i])
    assert len(o) == len(n) == NCOL, f"row {i} col count drift"
    for j in range(NCOL):
        if o[j] != n[j] and not (i > 0 and j in IDX.values() and o[PKI].strip() in P2_MAP):
            mismatches += 1
            if mismatches <= 3:
                print(f"  MISMATCH row {i} pk {o[PKI].strip()!r} col {j}: {o[j]!r} -> {n[j]!r}")

chk = list(csv.reader(io.StringIO(new_text)))
assert len(chk) == len(rows) and all(len(r) == NCOL for r in chk), "well-formedness"

# ── report ─────────────────────────────────────────────────────────────────────
def counts(rows_, text_=None):
    skos_n = attack_n = both_n = 0
    for r in rows_[1:]:
        s = split_fields(r)
        has_s = any(s[IDX[c]].strip() for c in TARGET_COLS)
        has_a = bool(s[ATI].strip())
        skos_n += has_s; attack_n += has_a; both_n += (has_s and has_a)
    return skos_n, attack_n, both_n

sk_before, at_before, bo_before = counts(rows)
sk_after,  at_after,  bo_after  = counts(new_rows2)
mtypes = Counter(P2_MAP[pk]["AIF_skosMappingType"] for pk in P2_MAP)
print("="*72)
print(f"#498 P2 RECONCILIATION APPLY — skos deep-serialize of 46 ML attack-only  (write={WRITE})")
print("="*72)
print(f"annotation CSV re-verified: 46/46 PKs, >=1 skos cell each, 0 novel token "
      f"(whitelist {len(native)}) — OK")
print(f"pre-state: all 46 target PKs attack-typed + skos-empty (attack-only): OK")
print(f"apply_set: 46 PKs x skos cells — shapes: {dict(shapes)}")
print(f"MappingType distribution: {dict(mtypes)}")
print(f"byte-preservation mismatches: {mismatches} (must be 0)")
print(f"well-formedness: {len(chk)} rows x {NCOL} cols, CRLF({ended_crlf})+BOM({bom}) preserved")
print(f"delta if written: {len(new_text)-len(text)} bytes")
print(f"layers: skos {sk_before} -> {sk_after} | attack {at_before} (unchanged) | "
      f"fully-modeled {bo_before} -> {bo_after}")
print(f"attack-only after: {at_after - bo_after} (was {at_before - bo_before}; "
      f"non-ML remainder = 29 for a future P3 tranche, fresh modeling)")
if WRITE:
    os.makedirs("tmp", exist_ok=True)
    open(BACKUP, "wb").write(raw)
    open(PATH, "wb").write((b'\xef\xbb\xbf' if bom else b'') + new_text.encode('utf-8'))
    print(f">>> WRITTEN (46 rows x skos cells filled). Backup saved to {BACKUP}.")
    print(f"    GATE: this write requires the distinct owner GO (dispatch 4kx29h scoped repo")
    print(f"    writes to docs/taxonomy/ — prod CSV write is a SEPARATE gate).")
else:
    print("DRY-RUN — no file written (pass --write after the owner GO).")
