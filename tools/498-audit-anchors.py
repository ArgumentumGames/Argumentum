#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#498 P1 rigor audit — the 18 FULLY-MODELED anchors (READ-ONLY, never writes).

Every P1 attackType derivation (#769) is anchored on the 18 rows that carry BOTH
skos tokens AND attack columns (the only ground-truth "this skos signature -> this
attackType"). If any anchor is mis-typed, the error propagates into every derived
tranche and, eventually, prod. This script re-derives the anchor set from prod
(code=truth) and cross-checks it against the human-reviewed audit CSV.

Two independent checks:
  1. MECHANICAL — the deterministic node map (#707§4a: undercut->RA-node,
     undermine->I-node, rebut->CA-node) holds for all 18 (0 violation expected).
  2. CROSS-VERIFY — the factual columns (pk / attackType / node) re-read from prod
     match `docs/taxonomy/498-audit-18-anchors.csv` exactly (verdict+note in that
     CSV are the authored Walton review; §2bis of 498-reconciliation-p1.md).

    python tools/498-audit-anchors.py    # read-only; prints the census + checks
"""
import csv, io, sys
from collections import Counter

PATH  = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
AUDIT = "docs/taxonomy/498-audit-18-anchors.csv"
NODE  = {"undercut": "RA-node", "undermine": "I-node", "rebut": "CA-node"}

# ── read prod CSV (read-only) ────────────────────────────────────────────────
raw  = open(PATH, "rb").read()
bom  = raw[:3] == b'\xef\xbb\xbf'
text = (raw[3:] if bom else raw).decode('utf-8')
rdr  = list(csv.reader(io.StringIO(text)))
H    = {name: i for i, name in enumerate(rdr[0])}
PK, ATT, NODc = H['PK'], H['AIF_attackType'], H['AIF_attackedNode']
DIR, EXC = H['AIF_skosDirectRef'], H['AIF_skosExceptionRef']
OTH = H.get('AIF_skosOther')

fully, attack_only, skos_only = [], 0, 0
for s in rdr[1:]:
    has_attack = bool(s[ATT].strip())
    has_skos = bool(s[DIR].strip() or s[EXC].strip() or (OTH is not None and s[OTH].strip()))
    if has_attack and has_skos: fully.append(s)
    elif has_attack: attack_only += 1
    elif has_skos: skos_only += 1

# ── check 1: layer census + node map ─────────────────────────────────────────
viol = [(s[PK].strip(), s[ATT].strip(), s[NODc].strip())
        for s in fully if s[NODc].strip() != NODE.get(s[ATT].strip())]
prod = {s[PK].strip(): (s[ATT].strip(), s[NODc].strip()) for s in fully}

# ── check 2: cross-verify vs the audit CSV ───────────────────────────────────
audit = {}
with open(AUDIT, encoding="utf-8-sig") as fh:
    for row in csv.DictReader(fh):
        audit[row["anchor_pk"].strip()] = (row["AIF_attackType"].strip(),
                                            row["AIF_attackedNode"].strip(),
                                            row["verdict"].strip())
csv_pk_mismatch = set(prod) ^ set(audit)
csv_val_mismatch = {pk for pk in prod if pk in audit and prod[pk] != audit[pk][:2]}
verdicts = Counter(v[2] for v in audit.values())

# ── report ───────────────────────────────────────────────────────────────────
ok = (len(fully) == 18 and not viol and not csv_pk_mismatch and not csv_val_mismatch)
print("=" * 72)
print("#498 P1 anchor audit  (READ-ONLY)")
print("=" * 72)
print(f"layer census: fully-modeled={len(fully)}  attack-only={attack_only}  skos-only={skos_only}")
print(f"  (expect 18 / 75 / 52 per #768)")
print(f"attackType distribution (18): {dict(Counter(s[ATT].strip() for s in fully))}")
print(f"check 1 — node map #707§4a violations: {len(viol)} (expect 0)")
for pk, at, nd in viol:
    print(f"    VIOLATION pk {pk}: {at} -> node {nd} (expected {NODE.get(at)})")
print(f"check 2 — cross-verify vs {AUDIT}:")
print(f"    pk set mismatch: {csv_pk_mismatch or 'none'}")
print(f"    value mismatch : {csv_val_mismatch or 'none'}")
print(f"audit verdicts: {dict(verdicts)}  (2 SOFT = 1313 Evasion, 1361 Proces en incoherence — defendable, see §2bis)")
print("=" * 72)
print("RESULT:", "OK — anchor foundation validated" if ok else "FAIL — investigate above")
sys.exit(0 if ok else 1)
