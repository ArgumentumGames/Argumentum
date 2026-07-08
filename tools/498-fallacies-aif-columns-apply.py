#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#498 Fallacies AIF — serialize §7 I/RA/CA decomposition into 2 new CSV columns.

Adds `AIF_attackType` + `AIF_attackedNode` to
`Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, inserted AFTER
`AIF_skosMappingType` (col idx 94), before `shape` (col idx 95) — i.e. the 2 new
columns join the existing AIF column block (cols 91-94 → 91-96).

DERIVATION SOURCE: §7 ("Mapping to jsboige's enriched DoD") of the 12
`docs/taxonomy/498-aif-*-cluster.md` cluster docs on master. Each §7 has
`### pk NNN — Title → **attack-type**` headers; the bolded attack-type is
serialized. attackedNode is DETERMINISTIC from attackType per the ratified
contract (#707§4 Option a):  undercut→RA-node, undermine→I-node, rebut→CA-node.

FAIL-LOUD discipline (#677, 0 fabrication): where §7 marks a leaf as
FAIL-LOUD / gap (no derivable attack-type, or no RA-node to attack), the 2 new
cells are LEFT EMPTY. See FAIL_LOUD set below.

SURGICAL: byte-exact. Re-writes NO existing field. Each logical row is split
into byte-exact field-segments (CSV-aware: respects doubled-quote escapes + LF
embedded inside quoted cells); the 2 new field-segments are inserted at idx
95/96; rejoined. Verified post-write: for every row, existing fields (idx 0-94
and 95-101 original → 97-103 after) are byte-identical between backup and result.

Usage:
    python tools/498-fallacies-aif-columns-apply.py            # dry-run + verify
    python tools/498-fallacies-aif-columns-apply.py --write    # commit to CSV
"""
import csv, io, json, os, sys

PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
INSERT_AFTER_IDX = 94  # AIF_skosMappingType ; new cols land at idx 95, 96
NODE = {"undercut": "RA-node", "undermine": "I-node", "rebut": "CA-node"}
WRITE = "--write" in sys.argv

# ── Derived from §7 of the 12 cluster docs (see extraction audit) ──────────────
# 46 PKs with explicit attack-type in §7 header. 44 undercut + 2 undermine. 0 rebut.
UNDERCUT_PKS = {
    "801","802","803","804","805","806","807",          # vague-definition
    "826","827","828","830","831",                       # inconsistent-definition (829/832 fail-loud)
    "836",                                               # faulty-comparison (834/835/837 fail-loud)
    "841","842",                                         # fallacious-comparison (840 fail-loud)
    "844","845",                                         # association-fallacy
    "849","850","851","852","854",                       # amphibologie (847/848/853 fail-loud)
    "855","860","862","863","864",                       # equivoque-polysemie (861 fail-loud)
    "857","858","859","865","866",                       # equivoque-residual
    "867","875",                                         # equivoque-reification (868-874 fail-loud)
    "877","878","879","880",                             # narrative-ambiguity-insinuation
    "881","882","883","884","885","886",                 # narrative-ambiguity-deception
}
UNDERMINE_PKS = {"838","843"}                            # faulty-comparison: fabricated premise
# ── FAIL-LOUD / gap: no derivable attack-type → cells EMPTY ────────────────────
# CA-node-token missing (RA-node exists, attack-type hedged "would be"):
#   829 840 847 848 853
# RA-node / gap (no inference to attack):
#   832 834 835 837 861 868 869 870 871 872 873 874
FAIL_LOUD = {
    "829","840","847","848","853",                       # CA-token missing
    "832","834","835","837","861",                       # RA-node missing / gap
    "868","869","870","871","872","873","874",           # level-confusion family gap
}
TYPED = {pk: "undercut" for pk in UNDERCUT_PKS}
TYPED.update({pk: "undermine" for pk in UNDERMINE_PKS})
assert TYPED.keys().isdisjoint(FAIL_LOUD), "typed/fail-loud overlap!"

# ── byte-exact CSV field/row splitters (respect doubled quotes + embedded LF) ─
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

# ── main ───────────────────────────────────────────────────────────────────────
def main():
    raw = open(PATH, "rb").read()
    bom = raw[:3] == b'\xef\xbb\xbf'
    text = (raw[3:] if bom else raw).decode('utf-8')
    ended_crlf = text.endswith('\r\n')
    rows = split_logical_rows(text)
    assert len(rows) == 1409, f"expected 1409 logical rows, got {len(rows)}"

    header = rows[0]
    hsegs = split_fields(header)
    assert len(hsegs) == 102 and hsegs[INSERT_AFTER_IDX] == 'AIF_skosMappingType'
    new_rows = [",".join(hsegs[:INSERT_AFTER_IDX+1] + ["AIF_attackType","AIF_attackedNode"] + hsegs[INSERT_AFTER_IDX+1:])]

    filled = undercut = undermine = empty = 0
    for rtext in rows[1:]:
        segs = split_fields(rtext)
        assert len(segs) == 102
        pk = segs[0].strip()
        if pk in TYPED:
            at = TYPED[pk]; vals = [at, NODE[at]]
            filled += 1; undermine += (at == "undermine"); undercut += (at == "undercut")
        else:
            vals = ["", ""]; empty += 1
        new_rows.append(",".join(segs[:INSERT_AFTER_IDX+1] + vals + segs[INSERT_AFTER_IDX+1:]))

    new_text = "\r\n".join(new_rows) + ("\r\n" if ended_crlf else "")
    # re-parse well-formedness
    chk = list(csv.reader(io.StringIO(new_text)))
    assert len(chk) == 1409 and all(len(r) == 104 for r in chk)
    assert chk[0][95] == 'AIF_attackType' and chk[0][96] == 'AIF_attackedNode' and chk[0][97] == 'shape'
    print(f"filled={filled} (undercut={undercut}, undermine={undermine}), empty={empty}")
    print(f"new cols at idx 95/96; shape shifted to 97; delta={len(new_text)-len(text)} bytes; all 1409 rows 104 cols")

    if WRITE:
        open(PATH, "wb").write((b'\xef\xbb\xbf' if bom else b'') + new_text.encode('utf-8'))
        print(">>> WRITTEN")
    else:
        print(">>> DRY-RUN (pass --write to commit)")

if __name__ == "__main__":
    main()
