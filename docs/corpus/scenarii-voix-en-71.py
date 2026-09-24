# -*- coding: utf-8 -*-
"""#994 volet A étendu — garde du GO owner 14/09 : 71 cellules voix EN Scenarii.

GO (c.566, relais ai-01 de la décision owner « Go pour tes recos ») :
  périmètre = EXACTEMENT le §A.4 de docs/release-dossier/994-tournesol-et-delta-branche-A-2026-09-14.md :
  context 25 + smoothTalker 12 + drawer 10 + issue 24 = 71, fichier Scenarii CSV, colonnes EN seules.
  Cible : homogénéisation à la 3ᵉ personne, FR canonique en référence (71/71 FR à la 3ᵉ).
  Garde : « re-dériver les 71 depuis le CSV courant avant écriture. Si le compte ou la liste
  diffère du §A.4, publier le delta et arrêter — ne jamais forcer l'attendu. »

TÉMOINS À CHAQUE RUN (organe POST-merge : la passe #1375 `29ce5d03` a exécuté le GO
le 14/09/2026 — cet instrument vérifie que la cible est atteinte et tient)
  (a) les 71 paires (colonne, coordonnée) existent dans le CSV (jointure `coordonnées`),
      sans doublon de PAIRE — ⚠️ une même coordonnée peut être dans 2 colonnes
      (ex. 4,0301 : context + issue), l'unicité se juge sur la paire ;
  (b) sonde pronom explicite 2ᵉ personne (you/your/yours, \\b, insensible casse) sur les
      4 colonnes EN de TOUTES les rangées : attendu **0** — la cible du GO est
      « EN entièrement à la 3ᵉ personne » (§A.3 : 0 carte mixte après les 71) ;
      ⚠️ sonde par pronom explicite SEULEMENT : l'impératif nu (« Convince him… ») est 2ᵉ
      personne implicite mais sans marqueur non ambigu (leçon §6 du dossier : la regex
      « personne » diverge d'un facteur 3 sur les noms propres et impératifs à tiers) ;
  (c) chaque cellule worklist est qualifiée : you-explicite (attendu 0 post-merge) / sans
      marqueur ;
  (d) les 15 `issue` hors-25 : leur `context` doit être SANS you (déjà 3ᵉ pers.) ;
  (e) cartes mixtes restantes par sonde pronom (attendu 0).

SORTIE : rc=0 garde PASS ; rc=2 anomalie publiée ; --extract <fichier> : TSV des 71 paires
FR canonique / EN courant.
"""
import csv
import io
import os
import re
import subprocess
import sys

BASE = os.path.abspath(os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', '..'))
CSV_PATH = 'Cards/Scenarii/Argumentum Scenarii - Cards.csv'
KEY = 'coordonnées'
COLS = ['context', 'smoothTalker', 'drawer', 'issue']
FR_REF = {'context': 'contexte', 'smoothTalker': 'baratineur', 'drawer': 'piocheur', 'issue': 'enjeu'}

ISSUE_9_BASCULES = ['3,0104', '4,0301', '7,0102', '7,0103', '7,0104', '7,0105', '7,0107', '7,0201', '7,0206']
ISSUE_15_HORS25 = ['1,0301', '2,0101', '3,0102', '3,0106', '3,0107', '3,0108', '3,0202', '3,0203',
                   '4,0102', '5,0203', '7,0106', '7,0203', '7,0204', '7,0205', '7,0303']

WORKLIST = {
    'context': ['3,0104', '3,0305', '3,0306', '3,0311', '4,0101', '4,0103', '4,0202', '4,0203',
                '4,0301', '4,0302', '5,0101', '5,0102', '5,0201', '6,0101', '6,0201', '6,0301',
                '7,0102', '7,0103', '7,0104', '7,0105', '7,0107', '7,0201', '7,0206', '7,0301', '7,0302'],
    'smoothTalker': ['3,0101', '3,0102', '3,0103', '3,0104', '3,0203', '3,0306', '3,0311',
                     '4,0202', '4,0208', '4,0302', '7,0207', '7,0301'],
    'drawer': ['3,0102', '3,0103', '3,0202', '3,0203', '3,0306', '3,0311', '4,0208', '4,0302',
               '7,0106', '7,0207'],
    'issue': ISSUE_9_BASCULES + ISSUE_15_HORS25,
}
EXPECTED = {'context': 25, 'smoothTalker': 12, 'drawer': 10, 'issue': 24}

YOU = re.compile(r'\b(you|your|yours|yourself|yourselves)\b', re.I)


def rows():
    p = subprocess.run(['git', '-C', BASE, 'show', 'HEAD:%s' % CSV_PATH], capture_output=True)
    if p.returncode:
        sys.exit('illisible: %s@HEAD' % CSV_PATH)
    return list(csv.DictReader(io.StringIO(p.stdout.decode('utf-8-sig', 'replace'))))


def n(v):
    return (v or '').replace('\r\n', '\n').replace('\r', '\n').strip()


def main():
    argv = sys.argv[1:]
    extract = argv[argv.index('--extract') + 1] if '--extract' in argv else None

    data = rows()
    by_key = {n(r.get(KEY)): r for r in data}
    ok = True

    # (a) présence + unicité des PAIRES (colonne, coordonnée)
    all_pairs = [(col, c) for col, cs in WORKLIST.items() for c in cs]
    missing = [(col, c) for col, c in all_pairs if c not in by_key]
    dup = len(all_pairs) != len(set(all_pairs))
    a_ok = not missing and not dup and len(all_pairs) == 71
    print('(a) worklist 71 paires : présentes %d/71, doublons de paire %s -> %s'
          % (71 - len(missing), 'OUI' if dup else 'non', 'PASS' if a_ok else 'FAIL %s' % missing))
    ok &= a_ok
    for col in COLS:
        k = len(WORKLIST[col])
        print('    %-13s %2d (§A.4 attendu %d) -> %s' % (col, k, EXPECTED[col],
                                                         'PASS' if k == EXPECTED[col] else 'FAIL'))
        ok &= k == EXPECTED[col]

    # (b) sonde pronom you sur les 4 colonnes EN, toutes rangées — attendu 0 post-merge
    you_cells = set()
    for r in data:
        key = n(r.get(KEY))
        for col in COLS:
            if YOU.search(n(r.get(col))):
                you_cells.add((key, col))
    b_ok = not you_cells
    print('(b) cellules EN à pronom you : %d (attendu 0 = cible GO atteinte) -> %s'
          % (len(you_cells), 'PASS' if b_ok else 'ÉCART (lister, arrêter)'))
    for key, col in sorted(you_cells)[:20]:
        print('    ÉCART  %s  %s' % (key, col))
    ok &= b_ok

    # (c) qualification des 71 : you-explicite (attendu 0) vs sans marqueur
    wl = set((c, col) for col, cs in WORKLIST.items() for c in cs)
    n_you = sum(1 for p in wl if p in you_cells)
    print('(c) qualification : %d à pronom you explicite (attendu 0), %d sans marqueur' % (n_you, 71 - n_you))
    ok &= n_you == 0

    # (d) les 15 issue hors-25 : leur context doit être sans you
    bad_d = [c for c in ISSUE_15_HORS25 if (c, 'context') in you_cells]
    d_ok = not bad_d
    print('(d) les 15 issue hors-25 : context sans you -> %s' % ('PASS' if d_ok else 'ÉCART %s' % bad_d))
    ok &= d_ok

    # (e) cartes mixtes restantes par sonde pronom (§A.3 : 0 après les 71)
    mixed = sorted({k for (k, col) in you_cells})
    e_ok = not mixed
    print('(e) cartes EN avec pronom you dans une colonne rendue : %d (§A.3 attendu 0) -> %s'
          % (len(mixed), 'PASS' if e_ok else 'ÉCART %s' % mixed[:10]))
    ok &= e_ok

    if extract:
        with open(extract, 'w', encoding='utf-8', newline='') as f:
            f.write('coord\tcolonne\tFR_ref\tEN_actuel\tyou_explicite\n')
            for col in COLS:
                for c in WORKLIST[col]:
                    r = by_key[c]
                    f.write('%s\t%s\t%s\t%s\t%s\n' % (
                        c, col,
                        n(r.get(FR_REF[col])).replace('\t', ' ').replace('\n', ' | '),
                        n(r.get(col)).replace('\t', ' ').replace('\n', ' | '),
                        'oui' if (c, col) in you_cells else 'non'))
        print('extraction écrite : %s' % extract)
    return 0 if ok else 2


if __name__ == '__main__':
    sys.exit(main())
