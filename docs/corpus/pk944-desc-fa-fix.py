# -*- coding: utf-8 -*-
"""#1524 G944 — PK 944 `desc_fa` porte la fiche entière : écrire le seul segment explication.

Constat (issue #1524, mesuré 23/09 sur `789afcd8`) : `desc_fa` de PK 944
(chemin 6.1.2.1.1, hors deck) contient les TROIS segments étiquetés de la
fiche ([نام] nom, [توضیح] explication, [مثال] exemple) sur 3 lignes, au lieu
de la seule explication. Les segments nom et exemple sont déjà à leur place
(text_fa / example_fa, identiques octet pour octet).

Geste : `desc_fa` := segment explication, balise retirée, sauts de ligne
retirés. Octets COPIÉS (ZWNJ U+200C compris), jamais retapés. Écriture au
dialecte du CSV (quote minimal, EOL préservés), fichier temporaire puis
os.replace. Préconditions : les 3 lignes balisées, nom == text_fa,
exemple == example_fa. Post : re-parse, text_fa/example_fa/les 7 autres
desc_* de la ligne intacts, diff vs HEAD = 1 cellule exactement.
"""
import csv
import io
import os
import re
import subprocess
import sys

BASE = os.path.abspath(os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', '..'))
CSV = os.path.join(BASE, 'Cards', 'Fallacies', 'Argumentum Fallacies - Taxonomy.csv')
CSV_REL = 'Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv'
PK = '944'
COL = 'desc_fa'
LANGS = ['fr', 'en', 'ru', 'pt', 'ar', 'es', 'fa', 'zh']
BALISE = re.compile(r'^\[[^\]]{1,20}\]\s?')


def parse_records(text):
    """Scanner positionnel par ENREGISTREMENTS (champs multi-lignes traversés)."""
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
        eol = ''
        if text[i:i + 2] == '\r\n':
            eol, end, i = '\r\n', i, i + 2
        elif i < n and text[i] in '\r\n':
            eol, end, i = text[i], i, i + 1
        else:
            end = i
        return fields, segs, end, eol, i

    header, _, _, _, i = scan_record(0)
    while i < n and text[i] in '\r\n':
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
    raw_bytes = io.open(CSV, 'rb').read()
    bom = raw_bytes[:3] == b'\xef\xbb\xbf'
    text = raw_bytes.decode('utf-8-sig')
    header, recs = parse_records(text)
    assert header.count('PK') == 1 and COL in header, 'header inattendu'
    ci_pk, ci_col = header.index('PK'), header.index(COL)

    target = None
    for ri, (fields, segs, end, eol) in enumerate(recs):
        if fields[ci_pk].strip() == PK:
            target = (ri, fields, segs)
            break
    if target is None:
        sys.exit('PK %s introuvable' % PK)
    ri, fields, segs = target
    cur = fields[ci_col]
    lines = cur.split('\n')

    # idempotence : si la cellule est déjà mono-ligne sans balise, ne pas réécrire
    if len(lines) == 1 and not BALISE.match(cur):
        print('déjà corrigé — desc_fa mono-ligne sans balise (%d c.) ; vérification seule' % len(cur))
        new_text = text
    else:
        assert len(lines) == 3, 'attendu 3 lignes, vu %d' % len(lines)
        assert all(BALISE.match(l) for l in lines), 'balises attendues sur les 3 lignes'
        nom = BALISE.sub('', lines[0])
        expl = BALISE.sub('', lines[1])
        exem = BALISE.sub('', lines[2])
        assert nom == fields[header.index('text_fa')], 'segment nom != text_fa'
        assert exem == fields[header.index('example_fa')], 'segment exemple != example_fa'
        print('préconditions OK — desc_fa %d c. -> segment explication %d c.' % (len(cur), len(expl)))

        seg = '"' + expl.replace('"', '""') + '"' if needs_quote(expl) else expl
        s, e = segs[ci_col]
        new_text = text[:s] + seg + text[e:]

        header2, recs2 = parse_records(new_text)
        f2 = recs2[ri][0]
        assert f2[ci_col] == expl, 'POST: valeur écrite divergente'
        for l in LANGS:
            c = 'desc_' + l
            if c == COL:
                continue
            assert f2[header.index(c)] == fields[header.index(c)], 'POST: %s modifiée ?!' % c
        for c in ('text_fa', 'example_fa'):
            assert f2[header.index(c)] == fields[header.index(c)], 'POST: %s modifiée ?!' % c

        tmp = CSV + '.tmp-1524'
        data = new_text.encode('utf-8')
        if bom:
            data = b'\xef\xbb\xbf' + data
        io.open(tmp, 'wb').write(data)
        os.replace(tmp, CSV)

    old_raw = subprocess.run(['git', '-C', BASE, 'show', 'HEAD:' + CSV_REL],
                             capture_output=True).stdout.decode('utf-8-sig')
    o = list(csv.reader(io.StringIO(old_raw)))
    n2 = list(csv.reader(io.StringIO(new_text)))
    assert len(o) == len(n2)
    diffs = [(ri2, ci) for ri2, (ro, rn) in enumerate(zip(o, n2))
             for ci, (a, b) in enumerate(zip(ro, rn)) if a != b]
    # index de la ligne PK 944 dans csv.reader (le header décale de 1 vs le scanner)
    ci_pk_reader = o[0].index('PK')
    ri_ref = next(i for i, r in enumerate(o) if r[ci_pk_reader].strip() == PK)
    print('diff vs HEAD : %d cellule(s) %s (attendue : ligne %d, col %d)'
          % (len(diffs), diffs, ri_ref, ci_col))
    blob = subprocess.run(['git', '-C', BASE, 'hash-object', '--no-filters', CSV],
                          capture_output=True).stdout.decode().strip()
    print('hash-object --no-filters après : %s' % blob)
    if diffs != [(ri_ref, ci_col)]:
        sys.exit('FAIL: diff != 1 cellule attendue')
    print('OK')


if __name__ == '__main__':
    main()