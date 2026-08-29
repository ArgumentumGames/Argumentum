#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#498 P1 reconciliation tranche-1 APPLY — back-fill the 2 AIF attack columns
for the 14 PRECEDENT-derived skos-only rows (GATED — dry-run by default).

Companion to `docs/taxonomy/498-reconciliation-p1.md` (proposition) and its
machine-readable `docs/taxonomy/498-reconciliation-p1-annotations.csv`. Mirrors
`tools/498-phase13-apply.py` (#757): the 2 columns already exist post-#753, so
this is byte-exact CELL FILL, not column insertion.

These 14 rows are *skos-only* (they already carry vetted native skos tokens; only
the attack columns are empty). Their attackType is derived from their OWN skos
signature, anchored on the 18 fully-modeled rows (ground truth). NODE is the
deterministic ASPIC+ map (ratified #707§4 Option a): undercut→RA-node,
undermine→I-node, rebut→CA-node. No token is fabricated (#677): we TYPE rows that
already have a vetted skos.

GATE (per ai-01 GO `msg-20260710T180845-5i1v03`, covered by pilote GO #498
jsboige 2026-06-17): the proposition is reviewed by ai-01 before prod write.
`--write` is GATED until ai-01 relays the go. Dry-run (default) proves
byte-preservation without touching prod.

    python tools/498-p1-apply.py            # dry-run, 0 prod write (this PR)
    python tools/498-p1-apply.py --write    # APPLY 14 cells (GATED — ai-01 relay)
"""
import csv, io, sys, glob
from collections import Counter

PATH   = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
ANNOT  = "docs/taxonomy/498-reconciliation-p1-annotations.csv"
BACKUP = "tmp/Fallacies-backup-pre-p1.csv"   # saved before --write (independent verify)
NODE   = {"undercut": "RA-node", "undermine": "I-node", "rebut": "CA-node"}
WRITE  = "--write" in sys.argv

# ── 14-row tranche-1 map (attackType). Load-bearing: re-verified below to match
#    the annotation CSV 14/14 (the CSV is the human-reviewed source of truth). ──
P1_MAP = {
    # Cheating (5)
    "1198": "undermine", "1083": "undermine", "1090": "undermine",
    "1092": "undermine", "1104": "undermine",
    # Insufficiency (5)
    "1": "undermine", "3": "undermine", "70": "undermine", "133": "undermine",
    "4": "undercut",
    # Misleading language (2)
    "799": "undermine", "846": "undermine",
    # Mathematical error (1)
    "621": "undercut",
    # Obstruction (1)
    "1281": "rebut",
}
assert len(P1_MAP) == 14
assert Counter(P1_MAP.values()) == {"undermine": 11, "undercut": 2, "rebut": 1}

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

# ── load-bearing: re-verify P1_MAP vs the annotation CSV (human-reviewed source) ─
annot_map = {}
with open(ANNOT, encoding="utf-8-sig") as fh:
    for row in csv.DictReader(fh):
        annot_map[row["fallacy_pk"].strip()] = row["AIF_attackType"].strip()
        # node consistency in the annotation itself
        assert row["AIF_attackedNode"].strip() == NODE[row["AIF_attackType"].strip()], \
            f"annotation node/type inconsistent at pk {row['fallacy_pk']}"
assert set(annot_map) == set(P1_MAP), (
    f"P1_MAP vs annotation-CSV PK mismatch\n"
    f"  only CSV: {set(annot_map)-set(P1_MAP)}\n  only MAP: {set(P1_MAP)-set(annot_map)}")
assert all(annot_map[pk] == P1_MAP[pk] for pk in P1_MAP), "attack_type mismatch vs annotation CSV"

# ── read current CSV ───────────────────────────────────────────────────────────
raw = open(PATH, "rb").read()
bom = raw[:3] == b'\xef\xbb\xbf'
text = (raw[3:] if bom else raw).decode('utf-8')
ended_crlf = text.endswith('\r\n')
rows = split_logical_rows(text)
header = split_fields(rows[0])
NCOL = len(header)
ATI = header.index('AIF_attackType')
ANI = header.index('AIF_attackedNode')
PKI = 0  # uppercase 'PK'
assert header[ATI-1] == 'AIF_skosMappingType', "AIF col block moved?"

# ── pre-state: every target PK must be empty (fill, not overwrite) + carry skos ─
DIR = header.index('AIF_skosDirectRef'); EXC = header.index('AIF_skosExceptionRef')
OTH = header.index('AIF_skosOther')
pre = {}
for r in rows[1:]:
    s = split_fields(r); pk = s[PKI].strip()
    if pk in P1_MAP:
        pre[pk] = (s[ATI].strip(), s[ANI].strip(),
                   bool(s[DIR].strip() or s[EXC].strip() or s[OTH].strip()))
assert set(pre) == set(P1_MAP), f"target PKs missing in CSV: {set(P1_MAP)-set(pre)}"
not_empty = {pk for pk,(at,an,_) in pre.items() if at or an}
assert not not_empty, f"ABORT: target PKs not empty (would overwrite): {not_empty}"
no_skos = {pk for pk,(_,_,hs) in pre.items() if not hs}
assert not no_skos, f"ABORT: target PKs lack skos (not skos-only back-fill): {no_skos}"

# ── apply: cell-fill byte-exact (only ATI/ANI of the 14 PK change) ────────────
new_rows = [rows[0]]
filled = Counter()
for rtext in rows[1:]:
    s = split_fields(rtext); pk = s[PKI].strip()
    if pk in P1_MAP:
        at = P1_MAP[pk]; s[ATI] = at; s[ANI] = NODE[at]; filled[at] += 1
    new_rows.append(",".join(s))
new_text = "\r\n".join(new_rows) + ("\r\n" if ended_crlf else "")

# ── byte-preservation proof (only ATI/ANI of the 14 PK may differ) ────────────
new_rows2 = split_logical_rows(new_text)
mismatches = 0
for i in range(len(rows)):
    o = split_fields(rows[i]); n = split_fields(new_rows2[i])
    assert len(o) == len(n) == NCOL, f"row {i} col count drift"
    for j in range(NCOL):
        if o[j] != n[j] and not (i > 0 and j in (ATI, ANI) and o[PKI].strip() in P1_MAP):
            mismatches += 1
            if mismatches <= 3:
                print(f"  MISMATCH row {i} pk {o[PKI].strip()!r} col {j}: {o[j]!r} -> {n[j]!r}")

# re-parse well-formedness
chk = list(csv.reader(io.StringIO(new_text)))
assert len(chk) == len(rows) and all(len(r) == NCOL for r in chk), "well-formedness"

# ── report ─────────────────────────────────────────────────────────────────────
total_now = sum(1 for r in rows[1:] if split_fields(r)[ATI].strip())
print("="*72)
print(f"#498 P1 RECONCILIATION tranche-1 APPLY  (write={WRITE})")
print("="*72)
print(f"annotation CSV re-verified 14/14 vs P1_MAP: OK")
print(f"pre-state: all 14 target PKs empty + carry skos (skos-only back-fill): OK")
print(f"apply_set: 14 PKs -> distribution: {dict(filled)}")
print(f"byte-preservation mismatches: {mismatches} (must be 0)")
print(f"well-formedness: {len(chk)} rows x {NCOL} cols, CRLF({ended_crlf})+BOM({bom}) preserved")
print(f"delta if written: {len(new_text)-len(text)} bytes")
print(f"CSV attack-typed total: {total_now} -> {total_now + 14}")
if WRITE:
    import os
    os.makedirs("tmp", exist_ok=True)
    open(BACKUP, "wb").write(raw)
    payload = (b'\xef\xbb\xbf' if bom else b'') + new_text.encode('utf-8')  # encode FIRST
    tmp = PATH + ".tmp"
    with open(tmp, "wb") as fh:
        fh.write(payload)
    os.replace(tmp, PATH)   # atomic — target intact if encode/write raised (write-safety #498)
    print(f">>> WRITTEN (14 cells filled). Backup saved to {BACKUP} for independent verify.")
    print(f"    GATE lifted by ai-01 relay. Two-layer fully-modeled: 18 -> 32.")
else:
    print(">>> DRY-RUN (pass --write to APPLY; GATED until ai-01 relays go).")
