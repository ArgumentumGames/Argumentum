#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#498 P1 reconciliation tranche-1d APPLY — back-fill the 2 AIF attack columns
for 4 skos-only rows, the whole 'Erreur mathematique' family (GATED — dry-run
by default).

Companion to `docs/taxonomy/498-reconciliation-p1d.md` (proposition) and its
machine-readable `docs/taxonomy/498-reconciliation-p1d-annotations.csv`. Mirrors
`tools/498-p1c-apply.py`: the 2 columns already exist post-#753, so this is
byte-exact CELL FILL, not column insertion.

tranche-1d = 4 SUFFIX-ONLY rows (595, 614, 632, 677). None has an exact-token
precedent among the fully-modeled anchors (p1d-precedent-check: all
precedent_votes={}, checked against the enlarged fully-modeled set). They are the
complete 'Erreur mathematique' family: improper generalization (inductive leap),
fallacy of accident (exception defeats the rule-application), erroneous
quantitative interpretation (evidence->hypothesis inference), slippery slope
(weakest-link chain). Modeled per-row from each fallacy's OWN scheme + desc_fr
(Walton "what does the CQ defeat"): every one attacks the INFERENTIAL step -> all
undercut/RA-node. Like 'Erreur de raisonnement' (tranche-1c), this is a family
whose defect is structurally the inference/deduction/induction step; the
uniformity is a per-row signature, coherence-checked against the in-set anchor 858
VerbalSlipperySlope (undercut, audit #770 — pk677's genus) and the ASPIC+
exception=undercut principle (pk614), NOT a mechanical default. The suffix prior
(_Conflict/_Inference -> undermine) is PROSCRIBED (in-set disproof pk804
_Conflict->undercut). attackType is a NEW judgment; NODE is the deterministic
ASPIC+ map (#707§4 Option a): undercut->RA, undermine->I, rebut->CA. No token
fabricated (#677): we TYPE rows that already carry a vetted skos.

NB — the remaining SUFFIX-ONLY families (Tricherie, Abus de langage, Insuffisance)
are NOT inference-uniform: e.g. Faux dilemme (814, false disjunctive premise) and
misrepresented facts (888) will type undermine. This tranche is undercut-uniform
because 'Erreur mathematique' is, by taxonomy, an inference-flaw family — the
method discriminates, it does not default to undercut.

GATE (per pilote GO #498 jsboige 2026-06-17, relay ai-01): the proposition is
reviewed by ai-01 before prod write. `--write` is GATED until ai-01 relays the go.

NOTE on baseline: the attack-typed total is driven by prod --write serializations,
NOT by PR merges (each tranche's proposition merges first; its --write is a separate
gated step relayed by ai-01). It is 107 if only tranche-1 (#771) is written, 111 if
tranche-1d is added, or higher if tranche-1b/1c are written first. This script reads
the ACTUAL current count and reports total_now -> total_now+4 dynamically — it never
hardcodes the baseline, so it is correct regardless of write order (the 4 target PKs
are disjoint from tranche-1's 14, tranche-1b's 7 and tranche-1c's 7).

    python tools/498-p1d-apply.py            # dry-run, 0 prod write (this PR)
    python tools/498-p1d-apply.py --write    # APPLY 4 cells (GATED — ai-01 relay)
"""
import csv, io, sys, os
from collections import Counter

PATH   = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
ANNOT  = "docs/taxonomy/498-reconciliation-p1d-annotations.csv"
BACKUP = "tmp/Fallacies-backup-pre-p1d.csv"   # saved before --write (independent verify)
NODE   = {"undercut": "RA-node", "undermine": "I-node", "rebut": "CA-node"}
WRITE  = "--write" in sys.argv

# ── 4-row tranche-1d map (attackType). Load-bearing: re-verified below to match
#    the annotation CSV 4/4 (the CSV is the human-reviewed source of truth). All
#    SUFFIX-ONLY, whole 'Erreur mathematique' family -> all inference-level. ──
P1D_MAP = {
    "595":  "undercut",   # Generalisation abusive (inductive leap unwarranted)
    "614":  "undercut",   # Sophisme de l'accident (exception defeats rule-application)
    "632":  "undercut",   # Interpretation quantitative erronee (evidence->hypothesis inference)
    "677":  "undercut",   # Pente glissante (weakest-link chain; in-set anchor 858)
}
assert len(P1D_MAP) == 4
assert Counter(P1D_MAP.values()) == {"undercut": 4}

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

# ── load-bearing: re-verify P1D_MAP vs the annotation CSV (human-reviewed source) ─
annot_map = {}
with open(ANNOT, encoding="utf-8-sig") as fh:
    for row in csv.DictReader(fh):
        annot_map[row["fallacy_pk"].strip()] = row["AIF_attackType"].strip()
        assert row["AIF_attackedNode"].strip() == NODE[row["AIF_attackType"].strip()], \
            f"annotation node/type inconsistent at pk {row['fallacy_pk']}"
assert set(annot_map) == set(P1D_MAP), (
    f"P1D_MAP vs annotation-CSV PK mismatch\n"
    f"  only CSV: {set(annot_map)-set(P1D_MAP)}\n  only MAP: {set(P1D_MAP)-set(annot_map)}")
assert all(annot_map[pk] == P1D_MAP[pk] for pk in P1D_MAP), "attack_type mismatch vs annotation CSV"

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
    if pk in P1D_MAP:
        pre[pk] = (s[ATI].strip(), s[ANI].strip(),
                   bool(s[DIR].strip() or s[EXC].strip() or s[OTH].strip()))
assert set(pre) == set(P1D_MAP), f"target PKs missing in CSV: {set(P1D_MAP)-set(pre)}"
not_empty = {pk for pk,(at,an,_) in pre.items() if at or an}
assert not not_empty, f"ABORT: target PKs not empty (would overwrite): {not_empty}"
no_skos = {pk for pk,(_,_,hs) in pre.items() if not hs}
assert not no_skos, f"ABORT: target PKs lack skos (not skos-only back-fill): {no_skos}"

# ── apply: cell-fill byte-exact (only ATI/ANI of the 4 PK change) ─────────────
new_rows = [rows[0]]
filled = Counter()
for rtext in rows[1:]:
    s = split_fields(rtext); pk = s[PKI].strip()
    if pk in P1D_MAP:
        at = P1D_MAP[pk]; s[ATI] = at; s[ANI] = NODE[at]; filled[at] += 1
    new_rows.append(",".join(s))
new_text = "\r\n".join(new_rows) + ("\r\n" if ended_crlf else "")

# ── byte-preservation proof (only ATI/ANI of the 4 PK may differ) ─────────────
new_rows2 = split_logical_rows(new_text)
mismatches = 0
for i in range(len(rows)):
    o = split_fields(rows[i]); n = split_fields(new_rows2[i])
    assert len(o) == len(n) == NCOL, f"row {i} col count drift"
    for j in range(NCOL):
        if o[j] != n[j] and not (i > 0 and j in (ATI, ANI) and o[PKI].strip() in P1D_MAP):
            mismatches += 1
            if mismatches <= 3:
                print(f"  MISMATCH row {i} pk {o[PKI].strip()!r} col {j}: {o[j]!r} -> {n[j]!r}")

# re-parse well-formedness
chk = list(csv.reader(io.StringIO(new_text)))
assert len(chk) == len(rows) and all(len(r) == NCOL for r in chk), "well-formedness"

# ── report ─────────────────────────────────────────────────────────────────────
total_now = sum(1 for r in rows[1:] if split_fields(r)[ATI].strip())
baseline_note = {
    107: "107 (tranche-1 written; 1b/1c writes pending)",
    111: "111 (tranche-1 + tranche-1d written)",
    114: "114 (tranche-1 + tranche-1b written)",
    121: "121 (tranche-1 + tranche-1b + tranche-1c written)",
}.get(total_now, str(total_now))
print("="*72)
print(f"#498 P1 RECONCILIATION tranche-1d APPLY  (write={WRITE})")
print("="*72)
print(f"annotation CSV re-verified 4/4 vs P1D_MAP: OK")
print(f"pre-state: all 4 target PKs empty + carry skos (skos-only back-fill): OK")
print(f"apply_set: 4 PKs -> distribution: {dict(filled)}  (whole 'Erreur mathematique' family)")
print(f"byte-preservation mismatches: {mismatches} (must be 0)")
print(f"well-formedness: {len(chk)} rows x {NCOL} cols, CRLF({ended_crlf})+BOM({bom}) preserved")
print(f"delta if written: {len(new_text)-len(text)} bytes")
print(f"CSV attack-typed total: {total_now} -> {total_now + 4}   [baseline {baseline_note}; disjoint from tranche-1's 14 + 1b's 7 + 1c's 7]")
if WRITE:
    os.makedirs("tmp", exist_ok=True)
    open(BACKUP, "wb").write(raw)
    payload = (b'\xef\xbb\xbf' if bom else b'') + new_text.encode('utf-8')  # encode FIRST
    tmp = PATH + ".tmp"
    with open(tmp, "wb") as fh:
        fh.write(payload)
    os.replace(tmp, PATH)   # atomic — target intact if encode/write raised (write-safety #498)
    print(f">>> WRITTEN (4 cells filled). Backup saved to {BACKUP} for independent verify.")
    print(f"    GATE lifted by ai-01 relay.")
else:
    print(">>> DRY-RUN (pass --write to APPLY; GATED until ai-01 relays go).")
