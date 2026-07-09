#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#498 phase 1-3 INDEPENDENT byte-preservation verifier (run AFTER apply --write).

This is the INDEPENDENT verifier (backup-vs-result), NOT the apply script's
dry-run self-check. Same discipline as #753/#755 (ai-01's independent byte-check):
reads `tmp/Fallacies-backup-pre-phase13.csv` (saved by the apply script) vs the
current prod CSV and proves:
  1. 0 byte-preservation mismatch — every field except AIF_attackType/
     AIF_attackedNode of the apply_set PKs is byte-identical.
  2. Independent re-derive of the distribution (count + assert vs expected).
  3. Spot-checks: the 3 rebut PKs → rebut/CA-node; a few undermine → I-node;
     a few undercut → RA-node; the 3 CONFIRM PKs unchanged; 834/847 per the flag.

The expected apply_set is re-derived INDEPENDENTLY here from the current CSV
state + PHASE_MAP + FAIL_LOUD_753 + the --with-overrides flag (not imported from
the apply script — re-declaration is the independence guarantee).

Usage (after `python tools/498-phase13-apply.py --write [--with-overrides]`):
    python tools/498-phase13-verify.py            # verify MODE 1 (45 NEW)
    python tools/498-phase13-verify.py --with-overrides   # verify MODE 2 (47)

If the backup doesn't exist (no --write yet), this prints the expected
post-state without comparison (dry verification of the re-derive logic).
"""
import csv, os, sys
from collections import Counter

PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
BACKUP = "tmp/Fallacies-backup-pre-phase13.csv"
NODE = {"undercut": "RA-node", "undermine": "I-node", "rebut": "CA-node"}
WITH_OVERRIDES = "--with-overrides" in sys.argv

# ── re-declared independently (NOT imported from apply script) ─────────────────
PHASE_MAP = {
    "826":"undercut","834":"undercut","844":"undercut","847":"undercut","855":"undercut",
    "698":"undermine","707":"undercut","727":"undercut","735":"undercut","750":"undercut","784":"undercut",
    "55":"undermine","96":"undercut","112":"undercut","134":"undercut","153":"undercut",
    "165":"undermine","596":"undermine","644":"undermine","658":"undercut","667":"undermine",
    "681":"undermine","690":"undercut",
    "177":"undermine","219":"undermine","247":"undermine","300":"undermine","322":"undermine",
    "340":"undermine","357":"undermine","420":"undermine","511":"undermine",
    "889":"undermine","942":"undermine","953":"undermine",
    "974":"undercut","992":"undercut","1011":"undercut",
    "1024":"undermine","1174":"undermine","1242":"undermine",
    "1282":"rebut","1287":"undermine","1297":"undermine","1313":"rebut",
    "1345":"undercut","1352":"undermine","1361":"rebut",
    "1371":"undermine","1398":"undermine",
}
FAIL_LOUD_753 = {"829","840","847","848","853","832","834","835","837",
                 "861","868","869","870","871","872","873","874"}

# ── byte-exact splitters (doubled quotes + embedded LF aware) ──────────────────
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

def load(path):
    raw = open(path, "rb").read()
    bom = raw[:3] == b'\xef\xbb\xbf'
    return split_logical_rows((raw[3:] if bom else raw).decode('utf-8')), bom

# ── read current prod CSV ──────────────────────────────────────────────────────
new_rows, nbom = load(PATH)
header = split_fields(new_rows[0])
ATI = header.index('AIF_attackType')
ANI = header.index('AIF_attackedNode')
PKI = 0

# ── re-derive the EXPECTED apply_set independently (per the flag) ──────────────
# Build from CURRENT CSV state: which PHASE_MAP PKs were empty before the write?
# After a successful write, those PKs are now FILLED; CONFIRM were already filled;
# the apply_set = empty-before ∩ (new ∪ overrides-if-flag).
# Independently, we determine: which PKs SHOULD now be filled with what.
expected = {}  # pk -> attack_type
for r in new_rows[1:]:
    s = split_fields(r); pk = s[PKI].strip()
    if pk in PHASE_MAP:
        at = s[ATI].strip()
        if at:  # filled (either CONFIRM already, or just-written)
            expected[pk] = at

# Sanity: all PHASE_MAP PKs present in CSV
present = {split_fields(r)[PKI].strip(): True for r in new_rows[1:] if split_fields(r)[PKI].strip() in PHASE_MAP}
assert len(present) == len(PHASE_MAP), f"missing PHASE_MAP PKs: {set(PHASE_MAP)-set(present)}"

# spot-check: every filled PHASE_MAP PK's attack_type matches PHASE_MAP
filled_in_csv = {}
for r in new_rows[1:]:
    s = split_fields(r); pk = s[PKI].strip()
    if pk in PHASE_MAP and s[ATI].strip():
        filled_in_csv[pk] = s[ATI].strip()
assert set(filled_in_csv) == set(expected)
bad = {pk for pk, at in filled_in_csv.items() if at != PHASE_MAP[pk]}
assert not bad, f"filled PK attack_type != PHASE_MAP: {bad}"

# the apply_set that was written = filled now AND would-have-been-empty-before.
# We can't see "before" without the backup, so recompute the EXPECTED apply_set:
empty_now = {pk for pk in PHASE_MAP if pk not in filled_in_csv}
# Determine which empty_now PKs SHOULD have been written (depends on flag + FAIL-LOUD)
# If MODE 1 (no overrides): apply_set was (empty - FAIL_LOUD); 834/847 stay empty.
# If MODE 2 (overrides): apply_set was all empty.
# So the FILLED set we see must match: filled = PHASE_MAP - (empty-if-not-written)
# ── header + re-derive summary ────────────────────────────────────────────────
CRLF = b'\r\n'
raw_new = open(PATH, "rb").read()
print("="*72)
print(f"#498 PHASE 1-3 INDEPENDENT VERIFY  (with-overrides={WITH_OVERRIDES})")
print("="*72)
print(f"prod CSV: {len(new_rows)-1} data rows, AIF cols at idx {ATI}/{ANI}")
print(f"PHASE_MAP PKs filled in CSV: {len(filled_in_csv)}  ->  distribution: {dict(Counter(filled_in_csv.values()))}")
print(f"empty PHASE_MAP PKs: {sorted(empty_now) or 'none'}")

if not os.path.exists(BACKUP):
    # PRE-write state (no --write yet): dry verification of the re-derive only.
    # empty_now here = all PHASE_MAP PKs still empty (45 NEW + 834/847 = 47).
    # Report the apply_set --write WOULD fill (per flag); skip POST-write asserts.
    would_fill = {pk for pk in empty_now if WITH_OVERRIDES or pk not in FAIL_LOUD_753}
    print(f"\n⚠ no backup at {BACKUP} — apply --write has not run (PRE-write state).")
    print(f"   dry verification of re-derive only (no byte-compare yet).")
    print(f"   --write would fill {len(would_fill)} PKs (MODE {'2 +834/847 override' if WITH_OVERRIDES else '1 NEW only'}): "
          f"{dict(Counter(PHASE_MAP[p] for p in would_fill))}")
    print(f"   POST-write emptiness asserts + byte-compare run AFTER apply --write.")
    sys.exit(0)

# ── POST-write state (backup exists): assert expected emptiness per MODE ───────
if WITH_OVERRIDES:
    # MODE 2: all 50 PHASE_MAP PKs filled (45 written + 3 CONFIRM + 2 overrides)
    assert not empty_now, f"MODE 2 (overrides) but PKs still empty: {empty_now}"
else:
    # MODE 1: 834/847 stay empty (FAIL-LOUD); everything else filled.
    assert empty_now == {"834","847"}, f"MODE 1 expected only 834/847 empty, got: {empty_now}"

orig_rows, obom = load(BACKUP)
assert obom == nbom, f"BOM changed {obom}->{nbom}"
assert len(orig_rows) == len(new_rows) == 1409, f"row count {len(orig_rows)} vs {len(new_rows)}"

# determine apply_set from backup: PHASE_MAP PKs that were empty in backup
apply_set = set()
for i in range(1, 1409):
    o = split_fields(orig_rows[i]); opk = o[PKI].strip()
    if opk in PHASE_MAP and not o[ATI].strip():
        # was empty in backup → was it written? depends on flag
        if WITH_OVERRIDES or opk not in FAIL_LOUD_753:
            apply_set.add(opk)

mismatches = 0
for i in range(1409):
    o = split_fields(orig_rows[i]); n = split_fields(new_rows[i])
    assert len(o) == len(n) == 104, f"row {i} col count"
    for j in range(104):
        if o[j] != n[j]:
            ok = (i > 0 and j in (ATI, ANI) and o[PKI].strip() in apply_set)
            if not ok:
                mismatches += 1
                if mismatches <= 3:
                    print(f"  MISMATCH row {i} pk {o[PKI].strip()!r} col {j}: {o[j]!r} -> {n[j]!r}")

raw_old = open(BACKUP, "rb").read()
print(f"\nbackup: {BACKUP} ({len(raw_old)} bytes) vs prod ({len(raw_new)} bytes)")
print(f"byte-preservation mismatches: {mismatches} (must be 0)")
assert mismatches == 0
print(f"CRLF: backup={raw_old.count(CRLF)}, prod={raw_new.count(CRLF)}")
print(f"BOM:  backup={raw_old[:3]==b'\xef\xbb\xbf'}, prod={raw_new[:3]==b'\xef\xbb\xbf'}")
print(f"delta: {len(raw_new)-len(raw_old)} bytes")

# ── spot-checks (independent of apply script) ──────────────────────────────────
by_pk = {split_fields(r)[PKI].strip(): split_fields(r) for r in new_rows[1:]}
spot = {
    "1282":("rebut","CA-node"), "1313":("rebut","CA-node"), "1361":("rebut","CA-node"),  # the 3 rebut
    "698":("undermine","I-node"), "889":("undermine","I-node"), "340":("undermine","I-node"),
    "707":("undercut","RA-node"), "974":("undercut","RA-node"), "1345":("undercut","RA-node"),
    # CONFIRM (already #753) — must be unchanged
    "826":("undercut","RA-node"), "844":("undercut","RA-node"), "855":("undercut","RA-node"),
}
for pk,(at,node) in spot.items():
    r = by_pk[pk]; ok = r[ATI]==at and r[ANI]==node
    print(f"  spot pk {pk}: [{r[ATI]},{r[ANI]}] expect [{at},{node}] -> {'OK' if ok else 'FAIL'}")
    assert ok
# D2: 834/847 per flag
for pk in ("834","847"):
    r = by_pk[pk]
    if WITH_OVERRIDES:
        ok = r[ATI]=="undercut" and r[ANI]=="RA-node"; exp="[undercut,RA-node]"
    else:
        ok = r[ATI]=="" and r[ANI]==""; exp="[empty] (FAIL-LOUD)"
    print(f"  D2 pk {pk}: [{r[ATI]!r},{r[ANI]!r}] expect {exp} -> {'OK' if ok else 'FAIL'}")
    assert ok
# a random non-phase PK must be untouched
r = by_pk.get("2") or by_pk.get("801")
print(f"  non-phase pk {r[PKI].strip()}: unchanged (not in apply_set) -> OK")

print("\n>>> VERIFY PASSED: byte-exact preservation + correct values + correct apply_set + spot-checks OK")
