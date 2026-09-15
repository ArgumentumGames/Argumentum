#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#1226 — fill the 74 empty `CCby` cells with the corpus constant 'Argumentum'.

The Scenarii card footer is `<div>{{CCby}}</div>` — a CSV column. At the Golden
Master (0087f0ec, April 2024) the deck was 91 cards at 91/91 CCby; it has since
grown to 167 cards and the added rows never received the attribution, so 44% of
the deck ships without its Creative-Commons line, identically in all 8 boxes
(CCby has no language variant). Measured 29/08 on master 8705f234:
93 x 'Argumentum' / 74 x '' over 167 data rows (csv.DictReader count, not wc -l).

SURGICAL: byte-exact (house pattern, cf. tools/499-virtues-aif-columns-apply.py
/ memory [[csv-byte-exact-column-insertion]]). Each logical row is split into
byte-exact field-segments (CSV-aware: doubled-quote escapes + embedded LF); only
segments at the CCby index that are exactly '' are replaced by 'Argumentum'
(bare form — the form of the 93 existing cells); every other segment is
re-emitted verbatim. No BOM is added (source has none), CRLF terminators are
preserved (168 measured = header + 167 data, zero embedded newlines).

WRITE-SAFETY (#498/#1222 house motif): payload encoded BEFORE opening anything,
written to a tmp file, promoted atomically via os.replace — the target stays
byte-intact if anything above raises.

GATED: dry-run by default; `--write` applies (GO: dispatch msg-20260829T184207-nb4yyd).

Usage:
    python tools/1226-scenarii-ccby-fill.py            # dry-run + full verify
    python tools/1226-scenarii-ccby-fill.py --write    # apply (dispatch GO)
"""
import csv, io, sys

PATH = "Cards/Scenarii/Argumentum Scenarii - Cards.csv"
COL = "CCby"
FILL = "Argumentum"
WRITE = "--write" in sys.argv

# ── byte-exact CSV splitters (doubled quotes + embedded newlines safe) ────────
def split_logical_rows(text):
    rows, cur, in_q = [], [], False
    i, n = 0, len(text)
    while i < n:
        ch = text[i]
        if ch == '"':
            if in_q and i + 1 < n and text[i + 1] == '"':
                cur.append('""'); i += 2
            else:
                in_q = not in_q; cur.append(ch); i += 1
        elif ch == '\r' and not in_q and i + 1 < n and text[i + 1] == '\n':
            rows.append(''.join(cur)); cur = []; in_q = False; i += 2
        elif ch == '\n' and not in_q:
            rows.append(''.join(cur)); cur = []; i += 1
        else:
            cur.append(ch); i += 1
    tail = ''.join(cur)
    return rows, tail  # tail = bytes after the last row terminator ("" or partial)

def split_fields(row):
    fields, cur, in_q = [], [], False
    i, n = 0, len(row)
    while i < n:
        ch = row[i]
        if ch == '"':
            if in_q and i + 1 < n and row[i + 1] == '"':
                cur.append('""'); i += 2
            else:
                in_q = not in_q; cur.append(ch); i += 1
        elif ch == ',' and not in_q:
            fields.append(''.join(cur)); cur = []; i += 1
        else:
            cur.append(ch); i += 1
    fields.append(''.join(cur))
    return fields

def main():
    raw = open(PATH, "rb").read()
    bom = raw[:3] == b'\xef\xbb\xbf'
    text = (raw[3:] if bom else raw).decode('utf-8')
    crlf, lf = text.count('\r\n'), text.count('\n')
    assert lf == crlf, f"bare LF present ({lf - crlf}) — not the measured shape"
    ended_crlf = text.endswith('\r\n')
    rows, tail = split_logical_rows(text)
    assert tail == "", f"unexpected bytes after last row terminator: {tail!r}"
    assert len(rows) == 168, f"expected 168 logical rows (header + 167 data), got {len(rows)}"
    # round-trip proof: the splitter must reproduce the original file exactly
    assert "\r\n".join(rows) + "\r\n" == text, "splitter round-trip mismatch on ORIGINAL"

    header = split_fields(rows[0])
    assert len(header) == 70, f"expected 70 header cols, got {len(header)}"
    idx = header.index(COL)
    assert header[10] == COL, f"CCby measured at index 10, found at {idx}"

    fills = already = 0
    new_rows = [rows[0]]
    for r in rows[1:]:
        segs = split_fields(r)
        assert len(segs) == 70, f"ragged row ({len(segs)} segs)"
        if segs[idx] == '':
            segs[idx] = FILL; fills += 1
        else:
            assert segs[idx] == FILL, f"unexpected {COL} value (corpus constant violated): {segs[idx]!r}"
            already += 1
        new_rows.append(",".join(segs))
    assert fills == 74 and already == 93, f"fill census drift: fills={fills}, already={already}"
    print(f"fill plan: {fills} empty -> '{FILL}', {already} already filled (expect 74/93)")

    new_text = "\r\n".join(new_rows) + ("\r\n" if ended_crlf else "")
    # terminator shape preserved
    assert new_text.count('\r\n') == crlf and new_text.count('\n') == lf, "CRLF shape changed"
    assert "\r\n".join(split_logical_rows(new_text)[0]) + "\r\n" == new_text, "splitter round-trip mismatch on NEW"

    # census + column-by-column proof via independent csv re-parse
    old_parsed = list(csv.DictReader(io.StringIO(text, newline="")))
    new_parsed = list(csv.DictReader(io.StringIO(new_text, newline="")))
    assert len(old_parsed) == 167 and len(new_parsed) == 167
    empty_after = sum(1 for r in new_parsed if not (r[COL] or '').strip())
    assert empty_after == 0, f"{empty_after} empty CCby remain"
    changed_cols = {}
    for o, n in zip(old_parsed, new_parsed):
        for k in o:
            if o[k] != n[k]:
                changed_cols[k] = changed_cols.get(k, 0) + 1
    assert changed_cols == {COL: 74}, f"columns other than {COL} changed: {changed_cols}"
    print(f"verify OK: CCby 93/167 -> 167/167 non-empty; only column changed = {COL} ({changed_cols[COL]} cells); "
          f"CRLF {crlf} preserved; BOM={bom} (none added)")

    if WRITE:
        import os
        payload = (b'\xef\xbb\xbf' if bom else b'') + new_text.encode('utf-8')  # encode FIRST
        tmp = PATH + ".tmp"
        with open(tmp, "wb") as fh:
            fh.write(payload)
        os.replace(tmp, PATH)   # atomic — target intact if anything above raised (write-safety #498)
        print(f">>> WRITTEN: {PATH} ({len(payload)} bytes)")
        recheck = open(PATH, "rb").read()
        assert (recheck[:3] == b'\xef\xbb\xbf') == bom and recheck.decode('utf-8') == (text[:0] + new_text), "post-write mismatch"
        print("post-write verify OK")
    else:
        print(">>> DRY-RUN (pass --write; GO = dispatch msg-20260829T184207-nb4yyd)")

if __name__ == "__main__":
    main()
