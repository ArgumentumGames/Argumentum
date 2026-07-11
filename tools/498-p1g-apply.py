#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#498 P1 reconciliation tranche-1g APPLY — back-fill the 2 AIF attack columns
for 8 skos-only rows, the whole 'Tricherie' family (GATED — dry-run default).

Companion to `docs/taxonomy/498-reconciliation-p1g.md` (proposition) and its
machine-readable `docs/taxonomy/498-reconciliation-p1g-annotations.csv`. Mirrors
`tools/498-p1f-apply.py`: the 2 columns already exist post-#753, so this is
byte-exact CELL FILL, not column insertion.

tranche-1g = 8 SUFFIX-ONLY rows (888, 973, 1020, 1023, 1066, 1087, 1148, 1175).
They are the complete 'Tricherie' family (Arranger les faits + Changement de cap
+ Pensée biaisée). This is the LAST SUFFIX-ONLY family -> closes the 36-row
back-fill (14 tranche-1 + 7 1b + 7 1c + 4 1d + 5 1e + 7 1f + 8 1g = 52... wait
the 36 figure is the SUFFIX-ONLY subset: 5 1b + 7 1c + 4 1d + 5 1e + 7 1f + 8 1g
= 36). MIXTE distribution: 5 undercut / 3 undermine.

Per-row Walton modeling from each fallacy's OWN scheme + desc_fr. Three families
of defect:
  - deceptive arrangement / goalpost-moving / biased reasoning -> premise-level
    (undermine/I): 888, 973, 1023 (precedents PositionToKnow pk70,
    ConflictingGoals pk356, Bias pk70).
  - sunk-cost / effort-justification -> inference rule undercut via Waste scheme
    (undercut/RA): 1020, 1148 (precedent Waste_Inference pk432).
  - mere-exposure / anecdotal-evidence / attribution-bias -> weak induction ->
    inference rule undercut (undercut/RA): 1066, 1087, 1175 (1066/1175 Example_
    Inference novel; 1087 InductiveInference_Scheme, desc twin of tranche-1e
    pk34).

Precedent (token-level): 6/8 share a typed token (PositionToKnow, ConflictingGoals,
Waste x2, Bias, PresumptiveInference); 2/8 novel tokens (Example_Inference x2,
InductiveInference). The suffix prior is PROSCRIBED (memo aif-no-inherit).
attackType is a NEW judgment; NODE is the deterministic ASPIC+ map (#707§4 a):
undercut->RA, undermine->I, rebut->CA. No token fabricated (#677).

3 MED flags for ai-01 review:
  - pk888 (Presentation trompeuse des faits): SAME-TOKEN DIVERGENCE with
    tranche-1e pk2 (LackOfPTKReliability_Scheme). pk2 -> undercut (induction
    weak); pk888 -> undermine (deceptive arrangement = biased premise, reinforced
    by PositionToKnow_Inference precedent pk70 -> undermine). Verdicts diverge
    by desc, NOT by token (aif-no-inherit).
  - pk1066 (Effet de simple exposition): Example_Inference novel; undercut
    (familiarity -> acceptable is a defective inference rule); alt undermine
    (familiarity premise over-valued).
  - pk1175 (Biais d'attribution): Example_Inference novel; undercut (attribution
    inference over-systematic); alt undermine (causal premise false).

NOTE on baseline: attack-typed total is write-driven. This script reads the
ACTUAL current count and reports total_now -> total_now+8 dynamically. The 8
target PKs are disjoint from all prior tranches.

    python tools/498-p1g-apply.py            # dry-run, 0 prod write (this PR)
    python tools/498-p1g-apply.py --write    # APPLY 8 cells (GATED — ai-01 relay)
"""
import csv, io, sys, os
from collections import Counter

PATH   = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
ANNOT  = "docs/taxonomy/498-reconciliation-p1g-annotations.csv"
BACKUP = "tmp/Fallacies-backup-pre-p1g.csv"
NODE   = {"undercut": "RA-node", "undermine": "I-node", "rebut": "CA-node"}
WRITE  = "--write" in sys.argv

# ── 8-row tranche-1g map (attackType). Re-verified 8/8 vs annotation CSV below.
#    Whole 'Tricherie' family -> MIXTE (5 undercut / 3 undermine). ──
P1G_MAP = {
    "888":  "undermine",  # Presentation trompeuse des faits [MED: same-token LackOfPTK vs pk2->undercut + PositionToKnow pk70]
    "973":  "undermine",  # Deplacement des criteres (ConflictingGoals precedent pk356)
    "1020": "undercut",   # Sophisme des couts irrecuperables (Waste precedent pk432, sunk cost textbook)
    "1023": "undermine",  # Raisonnement biaise (Bias precedent pk70)
    "1066": "undercut",   # Effet de simple exposition [MED: Example_Inference novel, alt undermine]
    "1087": "undercut",   # Preuve anecdotique (desc twin of pk34, InductiveInference_Scheme)
    "1148": "undercut",   # Justification de l'effort (Waste precedent pk432, parallel 1020)
    "1175": "undercut",   # Biais d'attribution [MED: Example_Inference novel, alt undermine]
}
assert len(P1G_MAP) == 8
assert Counter(P1G_MAP.values()) == {"undercut": 5, "undermine": 3}

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

# ── load-bearing: re-verify P1G_MAP vs the annotation CSV (human-reviewed source) ─
annot_map = {}
with open(ANNOT, encoding="utf-8-sig") as fh:
    for row in csv.DictReader(fh):
        annot_map[row["fallacy_pk"].strip()] = row["AIF_attackType"].strip()
        assert row["AIF_attackedNode"].strip() == NODE[row["AIF_attackType"].strip()], \
            f"annotation node/type inconsistent at pk {row['fallacy_pk']}"
assert set(annot_map) == set(P1G_MAP), (
    f"P1G_MAP vs annotation-CSV PK mismatch\n"
    f"  only CSV: {set(annot_map)-set(P1G_MAP)}\n  only MAP: {set(P1G_MAP)-set(annot_map)}")
assert all(annot_map[pk] == P1G_MAP[pk] for pk in P1G_MAP), "attack_type mismatch vs annotation CSV"

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
PKI = 0
assert header[ATI-1] == 'AIF_skosMappingType', "AIF col block moved?"

# ── pre-state: every target PK must be empty (fill, not overwrite) + carry skos ─
DIR = header.index('AIF_skosDirectRef'); EXC = header.index('AIF_skosExceptionRef')
OTH = header.index('AIF_skosOther')
pre = {}
for r in rows[1:]:
    s = split_fields(r); pk = s[PKI].strip()
    if pk in P1G_MAP:
        pre[pk] = (s[ATI].strip(), s[ANI].strip(),
                   bool(s[DIR].strip() or s[EXC].strip() or s[OTH].strip()))
assert set(pre) == set(P1G_MAP), f"target PKs missing in CSV: {set(P1G_MAP)-set(pre)}"
not_empty = {pk for pk,(at,an,_) in pre.items() if at or an}
assert not not_empty, f"ABORT: target PKs not empty (would overwrite): {not_empty}"
no_skos = {pk for pk,(_,_,hs) in pre.items() if not hs}
assert not no_skos, f"ABORT: target PKs lack skos (not skos-only back-fill): {no_skos}"

# ── apply: cell-fill byte-exact ────────────────────────────────────────────────
new_rows = [rows[0]]
filled = Counter()
for rtext in rows[1:]:
    s = split_fields(rtext); pk = s[PKI].strip()
    if pk in P1G_MAP:
        at = P1G_MAP[pk]; s[ATI] = at; s[ANI] = NODE[at]; filled[at] += 1
    new_rows.append(",".join(s))
new_text = "\r\n".join(new_rows) + ("\r\n" if ended_crlf else "")

# ── byte-preservation proof ────────────────────────────────────────────────────
new_rows2 = split_logical_rows(new_text)
mismatches = 0
for i in range(len(rows)):
    o = split_fields(rows[i]); n = split_fields(new_rows2[i])
    assert len(o) == len(n) == NCOL, f"row {i} col count drift"
    for j in range(NCOL):
        if o[j] != n[j] and not (i > 0 and j in (ATI, ANI) and o[PKI].strip() in P1G_MAP):
            mismatches += 1
            if mismatches <= 3:
                print(f"  MISMATCH row {i} pk {o[PKI].strip()!r} col {j}: {o[j]!r} -> {n[j]!r}")

chk = list(csv.reader(io.StringIO(new_text)))
assert len(chk) == len(rows) and all(len(r) == NCOL for r in chk), "well-formedness"

# ── report ─────────────────────────────────────────────────────────────────────
total_now = sum(1 for r in rows[1:] if split_fields(r)[ATI].strip())
baseline_note = {121: "121 (1+1b+1c written; 1d/1e/1f write pending)",
                 137: "137 (+1d+1e+1f written)"}.get(total_now, str(total_now))
print("="*72)
print(f"#498 P1 RECONCILIATION tranche-1g APPLY  (write={WRITE})")
print("="*72)
print(f"annotation CSV re-verified 8/8 vs P1G_MAP: OK")
print(f"pre-state: all 8 target PKs empty + carry skos (skos-only back-fill): OK")
print(f"apply_set: 8 PKs -> distribution: {dict(filled)}  (whole 'Tricherie' family, MIXTE)")
print(f"byte-preservation mismatches: {mismatches} (must be 0)")
print(f"well-formedness: {len(chk)} rows x {NCOL} cols, CRLF({ended_crlf})+BOM({bom}) preserved")
print(f"delta if written: {len(new_text)-len(text)} bytes")
print(f"CSV attack-typed total: {total_now} -> {total_now + 8}   [baseline {baseline_note}; "
      f"LAST SUFFIX-ONLY family -> closes the 36-row back-fill]")
if WRITE:
    os.makedirs("tmp", exist_ok=True)
    open(BACKUP, "wb").write(raw)
    open(PATH, "wb").write((b'\xef\xbb\xbf' if bom else b'') + new_text.encode('utf-8'))
    print(f">>> WRITTEN (8 cells filled). Backup saved to {BACKUP} for independent verify.")
    print(f"    GATE lifted by ai-01 relay.")
else:
    print(">>> DRY-RUN (pass --write to APPLY; GATED until ai-01 relays go).")
