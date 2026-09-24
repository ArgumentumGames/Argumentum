# -*- coding: utf-8 -*-
"""G1-W étape 1 — extraire les valeurs cibles des titres depuis l'historique.

Sources (pool #458 c.5794922231 + c.5795213716 + c.5795375786) :
  9d45b4f9^ : PK 2, 176, 632, 666, 888, 973, 134, 713, 1287, 658, 799
  74557ea6^ : PK 492
  97431d64^ : PK 598
Colonnes text_<lang> ; ⛔ desc/example inchangés.
"""
import csv
import io
import os
import subprocess
import sys

BASE = os.path.abspath(os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', '..'))
CSV_PATH = 'Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv'

TARGETS = {
    '9d45b4f9^': ['2', '176', '632', '666', '888', '973', '134', '713', '1287', '658', '799'],
    '74557ea6^': ['492'],
    '97431d64^': ['598'],
}
LANGS = ['fr', 'en', 'ru', 'pt', 'ar', 'es', 'fa', 'zh']


def show(ref):
    p = subprocess.run(['git', '-C', BASE, 'show', '%s:%s' % (ref, CSV_PATH)], capture_output=True)
    if p.returncode:
        sys.exit('illisible @ %s' % ref)
    return list(csv.DictReader(io.StringIO(p.stdout.decode('utf-8-sig', 'replace'))))


for ref, pks in TARGETS.items():
    rows = show(ref)
    by_pk = {}
    for r in rows:
        pk = (r.get('PK') or '').strip()
        if pk:
            by_pk[pk] = r
    print('=== %s (%d lignes) ===' % (ref, len(rows)))
    for pk in pks:
        r = by_pk.get(pk)
        if r is None:
            print('  PK %s : ABSENT de %s' % (pk, ref))
            continue
        vals = {l: (r.get('text_' + l) or '').strip() for l in LANGS}
        print('  PK %-5s path=%s' % (pk, (r.get('path') or '').strip()))
        for l in LANGS:
            v = vals[l]
            if v:
                print('      text_%s : %r' % (l, v))
