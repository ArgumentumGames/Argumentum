#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#499 Virtues AIF — apply the deterministic back-fill (plan #750 v2) as 2 new
columns on `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv`.

This is the [secondaire] MIRROR of the Fallacies contract (#498, PR #753): once
the Fallacies column contract is merged + reviewed, the same 2 columns
(`AIF_attackType` + `AIF_attackedNode`) are added to the Virtues CSV so both
taxonomies share an identical AIF column contract (anti-drift).

DERIVATION (deterministic, plan #750 v2 — ratified):
    default      : undercut / RA-node
    override mine: undermine / I-node   if the virtue opposes {889 Mensonge, 804 Acception arbitraire}
                                        (fabricated/false underlying premise)
    override rebut: rebut / CA-node     if the virtue opposes {340 Appel aux conséquences}
                                        (counter-conclusion via consequences)
Distribution EXACT (programmatically re-confirmed on master): 206 undercut + 13
undermine + 3 rebut = 222 nodes with scheme; 1 root node (pk 0, no scheme) = empty.

SURGICAL: byte-exact (same technique as tools/498-fallacies-aif-columns-apply.py).
Re-writes NO existing field. See memory [[csv-byte-exact-column-insertion]].

PLACEMENT: inserted AFTER `AIF_skosMappingType` (col idx 77), before
`print_and_play` (idx 78) — mirrors the Fallacies placement (PR #753) so the AIF
column block is consistent across both CSVs. If the Fallacies placement decision
(PR #753 open question #2) lands as true file-end instead, change INSERT_AFTER_IDX
accordingly here to stay anti-drift.

⚠ GATED EXECUTION: this script is dry-run by default. Apply ONLY after the
Fallacies contract (PR #753) is merged + reviewed by ai-01 + jsboige veto.
Until then: 0 write to the Virtues prod CSV.

Usage:
    python tools/499-virtues-aif-columns-apply.py            # dry-run + verify
    python tools/499-virtues-aif-columns-apply.py --write    # GATED — apply (post-contract)
"""
import csv, io, sys

PATH = "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv"
INSERT_AFTER_IDX = 77  # AIF_skosMappingType ; new cols land at idx 78, 79; print_and_play shifts to 80
NODE = {"undercut": "RA-node", "undermine": "I-node", "rebut": "CA-node"}
WRITE = "--write" in sys.argv

# plan #750 v2 override sets (ratified; re-confirmed programmatically on master)
UNDERMINE_FALLACIES = {"889", "804"}   # fabricated/false underlying premise
REBUT_FALLACIES     = {"340"}          # counter-conclusion via consequences

# ── byte-exact CSV splitters (respect doubled quotes + embedded newlines) ──────
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
    if cur:
        rows.append(''.join(cur))
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

def main():
    raw = open(PATH, "rb").read()
    bom = raw[:3] == b'\xef\xbb\xbf'
    text = (raw[3:] if bom else raw).decode('utf-8')
    ended_crlf = text.endswith('\r\n')
    rows = split_logical_rows(text)
    assert len(rows) == 224, f"expected 224 logical rows (223 data + header), got {len(rows)}"

    header = rows[0]
    hsegs = split_fields(header)
    assert len(hsegs) == 79, f"header cols {len(hsegs)} != 79"
    assert hsegs[INSERT_AFTER_IDX] == 'AIF_skosMappingType', f"col {INSERT_AFTER_IDX} is {hsegs[INSERT_AFTER_IDX]!r}"
    new_rows = [",".join(hsegs[:INSERT_AFTER_IDX+1] + ["AIF_attackType","AIF_attackedNode"] + hsegs[INSERT_AFTER_IDX+1:])]

    under = mine = rebut = empty = 0
    mine_pks = []
    rebut_pks = []
    for rtext in rows[1:]:
        segs = split_fields(rtext)
        assert len(segs) == 79, f"data row pk {segs[0]!r} has {len(segs)} segs"
        pk = segs[0].strip()
        # locate scheme + opposes by header position (AIF_skosDirectRef idx 74, crossLink_Opposes idx 71)
        # but read robustly via re-parse of this single row
        rowmap = dict(zip(hsegs, segs))
        scheme = (rowmap.get('AIF_skosDirectRef') or '').strip()
        opp = {x.strip() for x in (rowmap.get('crossLink_Opposes') or '').split(';') if x.strip()}
        if not scheme:
            vals = ["", ""]; empty += 1   # root node pk 0 (no scheme) -> empty (plan §7)
        elif opp & UNDERMINE_FALLACIES:
            vals = ["undermine", NODE["undermine"]]; mine += 1; mine_pks.append(pk)
        elif opp & REBUT_FALLACIES:
            vals = ["rebut", NODE["rebut"]]; rebut += 1; rebut_pks.append(pk)
        else:
            vals = ["undercut", NODE["undercut"]]; under += 1
        new_rows.append(",".join(segs[:INSERT_AFTER_IDX+1] + vals + segs[INSERT_AFTER_IDX+1:]))

    new_text = "\r\n".join(new_rows) + ("\r\n" if ended_crlf else "")
    # verify
    chk = list(csv.reader(io.StringIO(new_text)))
    assert len(chk) == 224 and all(len(r) == 81 for r in chk), f"reparsed {len(chk)} rows, cols vary"
    assert chk[0][78] == 'AIF_attackType' and chk[0][79] == 'AIF_attackedNode' and chk[0][80] == 'print_and_play'
    print(f"distribution: undercut={under}, undermine={mine}, rebut={rebut}, empty(root)={empty}")
    print(f"  undermine pks ({len(mine_pks)}): {sorted(mine_pks, key=int)}")
    print(f"  rebut pks     ({len(rebut_pks)}): {sorted(rebut_pks, key=int)}")
    print(f"total with scheme={under+mine+rebut} (expect 222), +root empty={empty} (expect 1)")
    assert under == 206 and mine == 13 and rebut == 3 and empty == 1, "distribution mismatch vs plan #750"
    print(f"new cols at idx 78/79; print_and_play shifted to 80; delta={len(new_text)-len(text)} bytes; all 224 rows 81 cols")

    # byte-preservation proof (same as Fallacies verify): existing fields unchanged
    orig_rows = split_logical_rows(text)
    mismatch = 0
    for i in range(224):
        o = split_fields(orig_rows[i]); n = split_fields(new_rows[i])
        if n[:INSERT_AFTER_IDX+1] != o[:INSERT_AFTER_IDX+1] or n[INSERT_AFTER_IDX+3:] != o[INSERT_AFTER_IDX+1:]:
            mismatch += 1
    print(f"byte-preservation mismatches: {mismatch} (must be 0)")
    assert mismatch == 0, "BYTE PRESERVATION FAILED"

    if WRITE:
        import os
        payload = (b'\xef\xbb\xbf' if bom else b'') + new_text.encode('utf-8')  # encode FIRST
        tmp = PATH + ".tmp"
        with open(tmp, "wb") as fh:
            fh.write(payload)
        os.replace(tmp, PATH)   # atomic — target intact if encode/write raised (write-safety #498)
        print(">>> WRITTEN (GATED — only run post Fallacies contract merge)")
    else:
        print(">>> DRY-RUN (pass --write; GATED until Fallacies contract PR #753 merged + reviewed)")

if __name__ == "__main__":
    main()
