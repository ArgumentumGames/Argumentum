#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#498 phase 1-3 AIF serialization APPLY — fill the 2 AIF columns for the
scaleup phase 1-3 generative leaves (GATED execution — dry-run by default).

Companion to `tools/498-phase13-serialization-audit.py` (#756, merged), which
proved the 50-leaf map is byte-faithful to the 3 machine-readable annotation
CSVs (50/50 match). This script does the actual byte-exact CELL FILL into
`Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` (the 2 columns already
exist post-#753; this is cell-fill, not column insertion).

GATE (per ai-01 dispatch `hlqdw4`, 2026-07-09): jsboige content nod for the 50
generative leaves is the prod-write gate. #756 surfaced the decision (D1);
ai-01 escalated D1 to jsboige. This script is READY TO RUN but `--write` is
GATED until ai-01 relays the jsboige verdict. Running dry-run (default) proves
byte-preservation without touching prod.

D2 coupling (834/847): ai-01 lean = override → undercut (ASPIC+ supersedes
#753's strict-conservateur FAIL-LOUD), but bundles it in the D1 escalation.
This script is PARAMETERIZED so a single flag flips at the verdict:
    python tools/498-phase13-apply.py            # dry-run, 45 NEW only (834/847 stay FAIL-LOUD)
    python tools/498-phase13-apply.py --write    # APPLY 45 NEW (GATED — jsboige nod)
    python tools/498-phase13-apply.py --write --with-overrides   # APPLY 47 (45 NEW + 834/847 undercut)

Source: PHASE_MAP + FAIL_LOUD_753 are byte-copied from
`tools/498-phase13-serialization-audit.py` (#756), which cross-checks them 50/50
against the annotation CSVs. NODE is the deterministic ASPIC+ map (ratified
#707§4 Option a): undercut→RA-node, undermine→I-node, rebut→CA-node.
"""
import csv, io, sys
from collections import Counter

PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
BACKUP = "tmp/Fallacies-backup-pre-phase13.csv"  # saved before --write (for independent verify)
NODE = {"undercut": "RA-node", "undermine": "I-node", "rebut": "CA-node"}
WRITE = "--write" in sys.argv
WITH_OVERRIDES = "--with-overrides" in sys.argv

# ── 50-leaf phase 1-3 map (copied from #756 audit; cross-checked 50/50 vs
#    annotation CSVs there — re-verified below as a load-bearing assertion) ────
PHASE_MAP = {
    # phase 1 (Abus langage + Erreur raisonnement) — 11
    "826":"undercut","834":"undercut","844":"undercut","847":"undercut","855":"undercut",
    "698":"undermine","707":"undercut","727":"undercut","735":"undercut","750":"undercut","784":"undercut",
    # phase 2 (Erreur math + Insuffisance) — 12
    "55":"undermine","96":"undercut","112":"undercut","134":"undercut","153":"undercut",
    "165":"undermine","596":"undermine","644":"undermine","658":"undercut","667":"undermine",
    "681":"undermine","690":"undercut",
    # phase 3 (Influence + Tricherie + Obstruction) — 27
    "177":"undermine","219":"undermine","247":"undermine","300":"undermine","322":"undermine",
    "340":"undermine","357":"undermine","420":"undermine","511":"undermine",
    "889":"undermine","942":"undermine","953":"undermine",
    "974":"undercut","992":"undercut","1011":"undercut",
    "1024":"undermine","1174":"undermine","1242":"undermine",
    "1282":"rebut","1287":"undermine","1297":"undermine","1313":"rebut",
    "1345":"undercut","1352":"undermine","1361":"rebut",
    "1371":"undermine","1398":"undermine",
}
# #753 deliberate FAIL-LOUD set (strict-conservateur verdict "garde tel quel").
# 834/847 are in this set → they are the D2 CONFLICT (override vs keep empty).
FAIL_LOUD_753 = {"829","840","847","848","853","832","834","835","837",
                 "861","868","869","870","871","872","873","874"}
assert len(PHASE_MAP) == 50
assert Counter(PHASE_MAP.values()) == {"undercut":20, "undermine":27, "rebut":3}

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

# ── load-bearing: re-verify PHASE_MAP vs the annotation CSVs (independent of #756)
import glob
ANNOT_CSVS = sorted(glob.glob("docs/taxonomy/498-scaleup-phase*-annotations.csv"))
annot_map = {}
for c in ANNOT_CSVS:
    with open(c, encoding="utf-8-sig") as fh:
        for row in csv.DictReader(fh):
            annot_map[row["fallacy_pk"].strip()] = row["attack_type"].strip()
assert set(annot_map) == set(PHASE_MAP), (
    f"PHASE_MAP vs annotation-CSV PK mismatch\n"
    f"  only CSVs: {set(annot_map)-set(PHASE_MAP)}\n  only MAP: {set(PHASE_MAP)-set(annot_map)}")
assert all(annot_map[pk] == PHASE_MAP[pk] for pk in PHASE_MAP), "attack_type mismatch"
# (50/50 re-verified — extraction is byte-faithful)

# ── read current CSV ───────────────────────────────────────────────────────────
raw = open(PATH, "rb").read()
bom = raw[:3] == b'\xef\xbb\xbf'
text = (raw[3:] if bom else raw).decode('utf-8')
ended_crlf = text.endswith('\r\n')
rows = split_logical_rows(text)
header = split_fields(rows[0])
ATI = header.index('AIF_attackType')
ANI = header.index('AIF_attackedNode')
PKI = 0  # uppercase 'PK' col
assert header[ATI-1] == 'AIF_skosMappingType', "AIF col block moved?"

# ── determine the apply_set per the D2 flag (based on CURRENT CSV state) ──────
# Snapshot which PHASE_MAP PKs are currently empty (= fillable) vs filled.
# The 3 CONFIRM PKs (826/844/855) are already filled undercut by #753 → not
# fillable → excluded from apply_set (no-op, no overwrite).
pre_state = {}
for r in rows[1:]:
    s = split_fields(r)
    pk = s[PKI].strip()
    if pk in PHASE_MAP:
        pre_state[pk] = s[ATI].strip()
empty_in_phase = {pk for pk, v in pre_state.items() if not v}      # fillable
filled_in_phase = {pk for pk, v in pre_state.items() if v}         # CONFIRM (no-op)
# D2 CONFLICT = fillable phase PKs that #753 deliberately left FAIL-LOUD.
conflict_pks = empty_in_phase & FAIL_LOUD_753                       # {834, 847}
new_pks = empty_in_phase - FAIL_LOUD_753                            # 45 NEW (fillable, not fail-loud)
if WITH_OVERRIDES:
    apply_set = empty_in_phase                 # 47 (45 NEW + 834/847)
    d2_note = "834/847 OVERRIDE → undercut (per jsboige nod)"
else:
    apply_set = new_pks                        # 45 NEW only (834/847 stay FAIL-LOUD)
    d2_note = "834/847 KEPT FAIL-LOUD (#753 strict-conservateur)"

# safety: every apply_set PK must be empty (fill, not overwrite).
non_empty = {pk for pk in apply_set if pre_state.get(pk)}
assert not non_empty, (
    f"ABORT: apply_set PKs not empty (would overwrite, not fill): {non_empty}")

# ── apply: cell-fill byte-exact (only ATI/ANI of apply_set PKs change) ────────
new_rows = [rows[0]]
filled = Counter()
for rtext in rows[1:]:
    s = split_fields(rtext)
    pk = s[PKI].strip()
    if pk in apply_set:
        at = PHASE_MAP[pk]
        s[ATI] = at
        s[ANI] = NODE[at]
        filled[at] += 1
    new_rows.append(",".join(s))
new_text = "\r\n".join(new_rows) + ("\r\n" if ended_crlf else "")

# ── byte-preservation proof (only ATI/ANI of apply_set may differ) ────────────
new_rows2 = split_logical_rows(new_text)
mismatches = 0
for i in range(len(rows)):
    o = split_fields(rows[i]); n = split_fields(new_rows2[i])
    assert len(o) == len(n) == 104, f"row {i} col count drift"
    for j in range(104):
        if o[j] != n[j] and not (i > 0 and j in (ATI, ANI) and o[PKI].strip() in apply_set):
            mismatches += 1
            if mismatches <= 3:
                print(f"  MISMATCH row {i} pk {o[PKI].strip()!r} col {j}: {o[j]!r} -> {n[j]!r}")

# re-parse well-formedness
chk = list(csv.reader(io.StringIO(new_text)))
assert len(chk) == len(rows) and all(len(r) == 104 for r in chk), "well-formedness"

# ── report ─────────────────────────────────────────────────────────────────────
total_now = sum(1 for r in rows[1:] if split_fields(r)[ATI].strip())
total_after = total_now + len(apply_set)
print("="*72)
print(f"#498 PHASE 1-3 APPLY  (write={WRITE}, with-overrides={WITH_OVERRIDES})")
print("="*72)
print(f"D2 handling: {d2_note}")
print(f"apply_set: {len(apply_set)} PKs  ->  distribution: {dict(filled)}")
print(f"  (NEW={len(new_pks)}, conflict-overridden={len(apply_set-new_pks)}, CONFIRM no-op={len(filled_in_phase)})")
print(f"PHASE_MAP re-verified 50/50 vs annotation CSVs: OK")
print(f"pre-state check: all {len(apply_set)} apply_set PKs empty (fill, no overwrite): OK")
print(f"byte-preservation mismatches: {mismatches} (must be 0)")
print(f"well-formedness: {len(chk)} rows × 104 cols, CRLF({ended_crlf})+BOM({bom}) preserved")
print(f"delta if written: {len(new_text)-len(text)} bytes")
print(f"CSV filled total: {total_now} -> {total_after}")
if WRITE:
    import os
    os.makedirs("tmp", exist_ok=True)
    open(BACKUP, "wb").write(raw)  # save the ORIGINAL bytes (pre-write) for independent verify
    open(PATH, "wb").write((b'\xef\xbb\xbf' if bom else b'') + new_text.encode('utf-8'))
    print(f">>> WRITTEN ({len(apply_set)} cells filled). Backup saved to {BACKUP} for independent verify.")
    print(f"    GATE was lifted by ai-01 relay of jsboige nod. Run: python tools/498-phase13-verify.py [--with-overrides]")
else:
    print(">>> DRY-RUN (pass --write to APPLY; GATED until ai-01 relays jsboige nod. [--with-overrides] for 834/847)")
