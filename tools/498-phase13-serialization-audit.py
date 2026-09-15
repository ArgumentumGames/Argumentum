#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#498 phase 1-3 serialization AUDIT (READ-ONLY diagnostic — 0 prod write).

Context: the 3 scaleup phase docs (`docs/taxonomy/498-aif-scaleup-phase{1,2,3}.md`)
are GATED proposals decomposing 50 depth-3 fallacies (5 families) into triple-AIF
(RA + ASPIC+ attack_type + CA/CQ). Their tables carry an explicit `attack` column =
the ASPIC+ attack-type (undermine/undercut/rebut) — exactly the `AIF_attackType`
value of the ratified 2-col contract (#707§4 Option a, serialized by #753 for the
Misleading-language family = 46 PKs).

#753 scoped OUT the phase 1-3 set (other families). This audit reports what
serializing phase 1-3 WOULD do, without writing anything:

  1. Encodes the 50-leaf phase 1-3 map (verbatim from the 3 docs' tables).
  2. Reads the current prod CSV, categorizes each phase PK:
       NEW      — CSV empty, phase fills it (no #753 history)
       CONFLICT — #753 left it FAIL-LOUD (empty), phase says fill (would override)
       CONFIRM  — already serialized by #753, phase agrees (no-op)
  3. Dry-run byte-exact apply proof (NO WRITE): proves byte-preservation WOULD
     hold if/when the jsboige content gate lifts.

Two decisions for ai-01/jsboige (the audit surfaces, does not decide):
  D1 — lift the phase 1-3 content gate? (phase docs self-gate on jsboige nod;
       closure §6.1 lists it as the prod-write gate for the 50 generative leaves)
  D2 — PKs 834/847: override #753's deliberate strict-conservateur FAIL-LOUD with
       phase 1's undercut? (ai-01's #753 verdict was "garde tel quel")

Source of the 50-leaf map (code=truth, read from the docs):
  phase1 §4  (11): 826,834,844,847,855,698,707,727,735,750,784
  phase2 §5  (12): 55,96,112,134,153,165,596,644,658,667,681,690
  phase3 §5.2(27): 177,219,247,300,322,340,357,420,511,889,942,953,974,992,
                   1011,1024,1174,1242,1282,1287,1297,1313,1345,1352,1361,1371,1398

Usage:
    python tools/498-phase13-serialization-audit.py     # read-only audit (no write)
"""
import csv, io
from collections import Counter

CSV_PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
NODE = {"undercut": "RA-node", "undermine": "I-node", "rebut": "CA-node"}
# #753 deliberate FAIL-LOUD set (strict-conservateur — left empty on purpose)
FAIL_LOUD_753 = {"829","840","847","848","853","832","834","835","837",
                 "861","868","869","870","871","872","873","874"}

# ── the 50-leaf phase 1-3 map (verbatim from doc tables) ───────────────────────
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
assert len(PHASE_MAP) == 50, f"expected 50, got {len(PHASE_MAP)}"
assert Counter(PHASE_MAP.values()) == {"undercut":20, "undermine":27, "rebut":3}  # closure §3

# ── INDEPENDENT VERIFICATION: cross-check PHASE_MAP against the 3 machine-readable
#    annotation CSVs (the source-of-truth the phase docs reference). This proves the
#    50-leaf verbatim-from-table extraction above is exact — every pk + attack_type
#    in PHASE_MAP matches the annotation CSV's fallacy_pk + attack_type columns.
import glob
ANNOT_CSVS = sorted(glob.glob("docs/taxonomy/498-scaleup-phase*-annotations.csv"))
assert len(ANNOT_CSVS) == 3, f"expected 3 annotation CSVs, found {len(ANNOT_CSVS)}: {ANNOT_CSVS}"
annot_map = {}
for c in ANNOT_CSVS:
    import csv as _csv
    with open(c, encoding="utf-8-sig") as fh:  # utf-8-sig strips the BOM on header
        for row in _csv.DictReader(fh):
            pk = row["fallacy_pk"].strip()
            annot_map[pk] = row["attack_type"].strip()
assert set(annot_map) == set(PHASE_MAP), (
    f"annotation-CSV PK set != PHASE_MAP PK set\n"
    f"  only in annotation CSVs: {set(annot_map) - set(PHASE_MAP)}\n"
    f"  only in PHASE_MAP:       {set(PHASE_MAP) - set(annot_map)}")
_mism = {pk for pk in PHASE_MAP if annot_map[pk] != PHASE_MAP[pk]}
assert not _mism, f"PHASE_MAP vs annotation-CSV attack_type mismatch on PKs: {_mism}"
ANNOT_VERIFIED = True  # 50/50 match — extraction is byte-faithful to the source CSVs

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

# ── 1. read current CSV ────────────────────────────────────────────────────────
raw = open(CSV_PATH, "rb").read()
bom = raw[:3] == b'\xef\xbb\xbf'
text = (raw[3:] if bom else raw).decode('utf-8')
ended_crlf = text.endswith('\r\n')
rows = split_logical_rows(text)
header = split_fields(rows[0])
ATI = header.index('AIF_attackType')
ANI = header.index('AIF_attackedNode')
PKI = 0  # uppercase 'PK' col
assert header[ATI-1] == 'AIF_skosMappingType', "AIF col block moved?"

csv_now = {}
for r in rows[1:]:
    s = split_fields(r)
    at = s[ATI].strip()
    if at:
        csv_now[s[PKI].strip()] = at

# ── 2. categorize phase PKs vs current CSV ─────────────────────────────────────
NEW, CONFLICT, CONFIRM = [], [], []
for pk, at in sorted(PHASE_MAP.items(), key=lambda kv: int(kv[0])):
    cur = csv_now.get(pk, "")
    if cur == "" and pk in FAIL_LOUD_753:
        CONFLICT.append((pk, at))      # would override #753 strict-conservateur
    elif cur == "":
        NEW.append((pk, at))
    elif cur == at:
        CONFIRM.append((pk, at))
    else:
        CONFLICT.append((pk, at))      # filled with a DIFFERENT value

print("="*72)
print("#498 PHASE 1-3 SERIALIZATION AUDIT (READ-ONLY, 0 prod write)")
print("="*72)
print(f"phase 1-3 map: {len(PHASE_MAP)} leaves  |  distribution: {dict(Counter(PHASE_MAP.values()))}")
print(f"  └─ ✓ 50/50 match vs the 3 annotation CSVs (machine-readable source): "
      f"PHASE_MAP extraction is byte-faithful, 0 transcription error" if ANNOT_VERIFIED else "")
print(f"current CSV:   {len(csv_now)} filled  |  distribution: {dict(Counter(csv_now.values()))}")
print()
print(f"NEW fills (empty -> fill, no #753 history): {len(NEW)}  -> {dict(Counter(a for _,a in NEW))}")
print(f"CONFLICT (#753 FAIL-LOUD or diff value, phase says fill): {len(CONFLICT)}")
for pk, at in CONFLICT:
    print(f"    pk {pk}: CSV={csv_now.get(pk,'<empty>')!r}  phase={at!r}")
print(f"CONFIRM (already filled, phase agrees — no-op): {len(CONFIRM)}")
for pk, at in CONFIRM:
    print(f"    pk {pk}: CSV={csv_now[pk]!r} == phase={at!r}")

proj = Counter(csv_now.values())
for pk, at in NEW + CONFLICT:
    proj[at] += 1
print()
print(f"IF gate lifts + apply (new + conflict-upgrade): +{len(NEW)+len(CONFLICT)} filled")
print(f"  CSV total: {len(csv_now)} -> {len(csv_now)+len(NEW)+len(CONFLICT)}  |  projected: {dict(proj)}")

# ── 3. dry-run byte-exact apply proof (NO WRITE) ───────────────────────────────
apply_set = {pk: at for pk, at in NEW + CONFLICT}
new_rows = [rows[0]]
filled_count = 0
for rtext in rows[1:]:
    s = split_fields(rtext)
    pk = s[PKI].strip()
    if pk in apply_set:
        s[ATI] = apply_set[pk]
        s[ANI] = NODE[apply_set[pk]]
        filled_count += 1
    new_rows.append(",".join(s))
new_text = "\r\n".join(new_rows) + ("\r\n" if ended_crlf else "")

chk = list(csv.reader(io.StringIO(new_text)))
assert len(chk) == len(rows) and all(len(r) == 104 for r in chk), "well-formedness"

# byte-preservation: only ATI/ANI of apply_set PKs may differ
new_rows2 = split_logical_rows(new_text)
mismatches = 0
for i in range(len(rows)):
    o = split_fields(rows[i]); n = split_fields(new_rows2[i])
    for j in range(104):
        if o[j] != n[j] and not (i > 0 and j in (ATI, ANI) and o[PKI].strip() in apply_set):
            mismatches += 1
            if mismatches <= 3:
                print(f"  MISMATCH row {i} pk {o[PKI].strip()!r} col {j}: {o[j]!r} -> {n[j]!r}")

print()
print("─"*72)
print("DRY-RUN byte-exact apply proof (NO WRITE — would only fill empty cells):")
print(f"  would-fill: {filled_count} PKs  |  byte-preservation mismatches: {mismatches} (must be 0)")
print(f"  delta if written: {len(new_text)-len(text)} bytes  |  CRLF({ended_crlf}) + BOM({bom}) preserved, 104 cols/row")
print()
print(">>> AUDIT ONLY — no write. jsboige phase 1-3 content gate NOT crossed.")
print(">>> Decisions for ai-01: (D1) lift phase 1-3 content gate? (D2) 834/847 override #753 FAIL-LOUD?")
