#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#1058 — Fill the 48 empty EN `Family` cells in the Fallacies taxonomy.

`Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` has `family_en` (the
header reads `Family`, col idx 20 — the unsuffixed family column IS the English
reference; `Famille` col 4 is FR) populated on 1360/1408 rows. The 48 holes are
the ONLY incomplete field across all 8 language columns (FR/RU/PT/AR/ES/ZH/FA
are all 1408/1408, measured).

Method (ai-01 dispatch zapj9y): DERIVE from the nearest ancestor carrying a
non-empty EN Family, by walking the `path` column (positional dot-notation,
parent = drop last segment). Do NOT translate, do NOT guess. All 48 are
derivable (0 fail-loud). Distribution: 42 → Influence (FR Influence), 4 →
Cheating (FR Tricherie), 2 → Misleading language (FR Abus de langage). FR→EN
mapping is univoque; `Quantitative reasoning` is not triggered (owner keep-off).

SURGICAL: byte-exact field-segment splitter (reused from #753
tools/498-fallacies-aif-columns-apply.py). Respects doubled-quote escapes +
LF embedded inside quoted cells. Only the idx-20 field-segment of the 48
target rows is substituted (non-quoted form, matching original style); every
other field of every row is byte-identical to the source.

Usage:
    python tools/1058-family-en-derive-apply.py            # dry-run + verify
    python tools/1058-family-en-derive-apply.py --write    # commit to CSV
"""
import io, csv, sys

PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
TARGET_IDX = 20  # 'Family' (EN reference; unsuffixed)
WRITE = "--write" in sys.argv

# ── byte-exact CSV field/row splitters (from #753, unchanged) ──────────────────
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
    assert len(hsegs) == 104 and hsegs[TARGET_IDX] == 'Family', \
        f"header col {TARGET_IDX} = {hsegs[TARGET_IDX]!r}, expected 'Family'"

    # path -> raw row text, for ancestor lookup
    by_path = {}
    for rtext in rows[1:]:
        segs = split_fields(rtext)
        assert len(segs) == 104, f"row PK={segs[0]} has {len(segs)} cols"
        by_path[segs[1]] = segs  # path is idx 1

    def derive_en(segs):
        """Walk path up until ancestor has non-empty EN Family. Return value or None."""
        p = segs[1]
        while '.' in p:
            p = p.rsplit('.', 1)[0]
            anc = by_path.get(p)
            if anc and len(anc) > TARGET_IDX and anc[TARGET_IDX].strip():
                return anc[TARGET_IDX].strip()
        return None

    # pass 1: compute derivations
    mods = []  # (row_idx_in_rows, pk, path, fr, derived)
    non_derivable = []
    from collections import Counter
    dist = Counter()
    fr_map = {}
    for ri, rtext in enumerate(rows[1:], start=1):
        segs = split_fields(rtext)
        if not segs[TARGET_IDX].strip():  # empty EN Family
            val = derive_en(segs)
            if val is None:
                non_derivable.append((segs[0], segs[1], segs[4]))
            else:
                mods.append((ri, segs[0], segs[1], segs[4], val, rtext, segs))
                dist[val] += 1
                fr_map.setdefault(val, set()).add(segs[4])

    print(f"=== derivation plan ===")
    print(f"  empty EN Family rows: {len(mods) + len(non_derivable)}")
    print(f"  derivable: {len(mods)} | non-derivable: {len(non_derivable)}")
    for v, c in dist.most_common():
        print(f"    {c:3d} → {v!r}  (FR: {sorted(fr_map[v])})")
    if non_derivable:
        print(f"  NON-DERIVABLE (would be left empty + listed):")
        for pk, p, fr in non_derivable:
            print(f"    PK={pk} path={p} FR={fr!r}")

    assert len(mods) == 48, f"expected 48 derivable, got {len(mods)}"
    assert not non_derivable, "non-derivable present — abort (ai-01: leave empty + list)"

    # pass 2: reconstruct rows, substituting ONLY idx 20 of target rows
    new_rows = [header]
    touched = 0
    mod_row_idx = {m[0] for m in mods}
    derived_by_ri = {m[0]: m[4] for m in mods}
    for ri, rtext in enumerate(rows[1:], start=1):
        if ri in mod_row_idx:
            segs = split_fields(rtext)
            new_val = derived_by_ri[ri]
            # non-quoted output (matches original filled style; value has no comma/quote/LF)
            assert ',' not in new_val and '"' not in new_val and '\n' not in new_val, \
                "derived value would need quoting — abort"
            new_row = ",".join(segs[:TARGET_IDX] + [new_val] + segs[TARGET_IDX+1:])
            new_rows.append(new_row)
            touched += 1
        else:
            new_rows.append(rtext)  # untouched row, byte-identical
    assert touched == 48

    # pass 3: VERIFY byte-preservation of every non-target field on the 48 rows
    for (ri, pk, p, fr, val, orig_rtext, orig_segs) in mods:
        new_rtext = new_rows[ri]
        new_segs = split_fields(new_rtext)
        assert len(new_segs) == 104
        # idx 20 changed to derived value
        assert new_segs[TARGET_IDX] == val, f"PK={pk} idx20 = {new_segs[TARGET_IDX]!r} != {val!r}"
        # every OTHER field byte-identical
        for i in range(104):
            if i == TARGET_IDX:
                continue
            assert new_segs[i] == orig_segs[i], \
                f"PK={pk} field {i} drifted: {orig_segs[i]!r} → {new_segs[i]!r}"

    # pass 4: re-parse for well-formedness; count EN non-empty now
    new_text = "\r\n".join(new_rows) + ("\r\n" if ended_crlf else "")
    chk = list(csv.reader(io.StringIO(new_text)))
    assert len(chk) == 1409 and all(len(r) == 104 for r in chk), "re-parse shape mismatch"
    en_nonempty = sum(1 for r in chk[1:] if r[TARGET_IDX].strip())
    assert en_nonempty == 1408, f"EN non-empty after = {en_nonempty}, expected 1408"
    # all 8 language family columns now 1408/1408
    for idx, name in [(4,'FR'),(20,'EN'),(37,'RU'),(44,'PT'),(51,'AR'),(58,'ES'),(65,'ZH'),(72,'FA')]:
        c = sum(1 for r in chk[1:] if r[idx].strip())
        assert c == 1408, f"{name} col {idx} = {c}/1408"
        print(f"  {name} col{idx}: {c}/1408 ✓")

    print(f"=== result ===")
    print(f"  48 rows touched (idx {TARGET_IDX} only); EN now 1408/1408")
    print(f"  byte delta = {len(new_text)-len(text)} (48 cells filled)")
    print(f"  BOM preserved: {bom} | CRLF preserved: {ended_crlf}")

    if WRITE:
        open(PATH, "wb").write((b'\xef\xbb\xbf' if bom else b'') + new_text.encode('utf-8'))
        print(">>> WRITTEN")
    else:
        print(">>> DRY-RUN (pass --write to commit)")

if __name__ == "__main__":
    main()
