# -*- coding: utf-8 -*-
"""Moissonne les blobs orphelins du CSV Taxonomy que le BFG a decroches de l'arbre.

Fenetre limitee : GitHub sert encore ces objets non-atteignables, mais peut les
collecter. On les rapatrie, on les verifie (sha1 git recalcule), on les parse.
"""
import io, os, csv, json, base64, hashlib, subprocess, sys

OUT = os.path.dirname(os.path.abspath(__file__)) + r'\recovered'
MAN = os.path.join(OUT, 'manifest.txt')
REPO = 'ArgumentumGames/Argumentum'

rows = [l.rstrip('\n').split('|') for l in io.open(MAN, encoding='utf-8') if l.strip()]
print('%d versions a rapatrier\n' % len(rows))
print('%-11s %-10s %-13s %9s %6s %5s %6s %s' % (
    'date', 'commit', 'blob', 'octets', 'lignes', 'col', 'cartes', 'sha1'))
print('-' * 92)

index, fails = [], []
for date, h, blob, subj in rows:
    dest = os.path.join(OUT, 'taxonomy_%s_%s.csv' % (date, blob[:8]))
    if os.path.exists(dest):
        raw = io.open(dest, 'rb').read()
    else:
        try:
            j = subprocess.run(['gh', 'api', 'repos/%s/git/blobs/%s' % (REPO, blob)],
                               capture_output=True, timeout=180)
            if j.returncode != 0:
                fails.append((date, blob, j.stderr.decode('utf-8', 'replace')[:60])); continue
            raw = base64.b64decode(json.loads(j.stdout)['content'])
        except Exception as e:
            fails.append((date, blob, repr(e)[:60])); continue
        # ecriture atomique : tmp puis replace (cf hazard_open_w_truncates_before_write)
        tmp = dest + '.part'
        io.open(tmp, 'wb').write(raw); os.replace(tmp, dest)

    # controle d'integrite : le sha1 git doit redonner le blob annonce
    sha1 = hashlib.sha1(b'blob %d\x00' % len(raw) + raw).hexdigest()
    ok = 'OK' if sha1 == blob else 'MISMATCH'

    txt = raw.decode('utf-8-sig', 'replace')
    rdr = list(csv.DictReader(io.StringIO(txt)))
    cards = sum(1 for r in rdr if (r.get('carte') or '').strip() not in ('', '0'))
    cols = len(rdr[0]) if rdr else 0
    print('%-11s %-10s %-13s %9d %6d %5d %6d %s' % (
        date, h, blob[:12], len(raw), len(rdr), cols, cards, ok))
    index.append(dict(date=date, commit=h, blob=blob, subject=subj, bytes=len(raw),
                      rows=len(rdr), cols=cols, cards=cards, file=os.path.basename(dest),
                      integrity=ok))

io.open(os.path.join(OUT, 'index.json'), 'w', encoding='utf-8', newline='\n').write(
    json.dumps(index, ensure_ascii=False, indent=1))
print('\nrapatries : %d/%d   |   echecs : %d' % (len(index), len(rows), len(fails)))
for f in fails:
    print('  ECHEC %s %s : %s' % f)
