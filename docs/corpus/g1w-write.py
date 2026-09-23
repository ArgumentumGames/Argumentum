# -*- coding: utf-8 -*-
"""G1-W — éditeur chirurgical : applique g1w-plan.json au CSV, octet à octet hors cible.

Le CSV contient des champs multi-lignes (1408 enregistrements, 1547 lignes
physiques) : on découpe donc le document brut PAR ENREGISTREMENT avec un
scanner positionnel (champs quotés traversent , \n \r), et chaque mutation
remplace le segment brut exact de SA cellule (offsets absolus, appliqués en
ordre décroissant). Précondition : le scanner redonne les valeurs du plan
(ancien) ; post : re-parse == valeurs nouvelles, diff de cellules == plan.

Les enregistrements non ciblés ne sont pas réécrits (octet à octet).
"""
import csv
import io
import json
import os
import subprocess
import sys

BASE = os.path.abspath(os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', '..'))
CSV = os.path.join(BASE, 'Cards', 'Fallacies', 'Argumentum Fallacies - Taxonomy.csv')
CSV_REL = 'Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv'
PLAN = os.path.join(os.path.dirname(os.path.abspath(__file__)), 'g1w-plan.json')


def parse_records(text):
    """-> (header_fields, [ (fields, cell_segments, rec_end, eol) ]).
    cell_segments : offset (absolu, start, end) de chaque champ, hors quotes.
    """
    n = len(text)
    recs = []

    def scan_record(i):
        fields, segs = [], []
        while True:
            start = i
            if i < n and text[i] == '"':
                i += 1
                buf = []
                while i < n:
                    c = text[i]
                    if c == '"':
                        if i + 1 < n and text[i + 1] == '"':
                            buf.append('"')
                            i += 2
                            continue
                        i += 1
                        break
                    buf.append(c)
                    i += 1
                fields.append(''.join(buf))
                segs.append((start, i))
            else:
                j = i
                while j < n and text[j] != ',' and text[j] != '\n' and text[j] != '\r':
                    j += 1
                fields.append(text[i:j])
                segs.append((start, j))
                i = j
            if i < n and text[i] == ',':
                i += 1
                continue
            break
        # fin d'enregistrement : \r\n, \n ou EOF
        eol = ''
        if text[i:i + 2] == '\r\n':
            eol = '\r\n'
            end = i
            i += 2
        elif i < n and text[i] in '\r\n':
            eol = text[i]
            end = i
            i += 1
        else:
            end = i
        return fields, segs, end, eol, i

    # header
    header, hsegs, hend, heol, i = scan_record(0)
    while i < n and (n - i) > 0 and all(c in '\r\n' for c in text[i:i + 1]):
        i += 1
    while i < n:
        f, s, e, eol, i2 = scan_record(i)
        if not f or (len(f) == 1 and f[0] == '' and e == i):
            i = i2
            continue
        recs.append((f, s, e, eol))
        i = i2
    return header, recs


def needs_quote(v):
    return any(c in v for c in (',', '"', '\n', '\r'))


def main():
    plan = json.load(io.open(PLAN, encoding='utf-8'))['plan']
    raw_bytes = io.open(CSV, 'rb').read()
    bom = raw_bytes[:3] == b'\xef\xbb\xbf'
    text = raw_bytes.decode('utf-8-sig')

    header, recs = parse_records(text)
    assert len(header) == 104, 'header : %d colonnes' % len(header)
    assert len(recs) == 1408, 'enregistrements : %d' % len(recs)

    by_rec = {}
    for m in plan:
        by_rec.setdefault(m['ligne'], []).append(m)

    edits = []   # (offset_start, offset_end, nouveau_seg)
    for ri, muts in by_rec.items():
        fields, segs, end, eol = recs[ri]
        assert len(fields) == len(header), 'rec %d : %d champs' % (ri, len(fields))
        for m in muts:
            ci = header.index(m['col'])
            if fields[ci] != m['ancien']:
                sys.exit('PRECOND FAIL rec %d %s : %r != %r'
                         % (ri, m['col'], fields[ci][:40], m['ancien'][:40]))
            v = m['nouveau']
            seg = '"' + v.replace('"', '""') + '"' if needs_quote(v) else v
            s, e = segs[ci]
            edits.append((s, e, seg, ri, m['col']))

    new_text = text
    for s, e, seg, ri, col in sorted(edits, key=lambda x: -x[0]):
        new_text = new_text[:s] + seg + new_text[e:]

    # post : re-parse et valeurs nouvelles
    header2, recs2 = parse_records(new_text)
    assert len(recs2) == len(recs), 'nb enregistrements changé'
    for s, e, seg, ri, col in edits:
        ci = header.index(col)
        assert recs2[ri][0][ci] == dict((m['col'], m['nouveau']) for m in by_rec[ri])[col], \
            'POSTCOND rec %d %s' % (ri, col)

    # écriture (BOM et octets préservés hors segments)
    data = new_text.encode('utf-8')
    if bom:
        data = b'\xef\xbb\xbf' + data
    io.open(CSV, 'wb').write(data)

    # diff de cellules contre HEAD
    old_raw = subprocess.run(['git', '-C', BASE, 'show', 'HEAD:' + CSV_REL],
                             capture_output=True).stdout.decode('utf-8-sig')
    o = list(csv.reader(io.StringIO(old_raw)))
    n2 = list(csv.reader(io.StringIO(new_text)))
    assert len(o) == len(n2)
    diff_cells, touched = 0, set()
    for ri, (ro, rn) in enumerate(zip(o, n2)):
        for ci, (a, b) in enumerate(zip(ro, rn)):
            if a != b:
                diff_cells += 1
                touched.add(ri)
    print('diff cellules : %d (attendu %d) sur %d enregistrements'
          % (diff_cells, len(plan), len(touched)))
    blob = subprocess.run(['git', '-C', BASE, 'hash-object', '--no-filters', CSV],
                          capture_output=True).stdout.decode().strip()
    print('hash-object --no-filters après : %s' % blob)
    if diff_cells != len(plan):
        sys.exit(1)
    print('OK')


if __name__ == '__main__':
    main()
