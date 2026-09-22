# -*- coding: utf-8 -*-
"""G1 (#1499) — Dossier d'arbitrage Fallacies : renommages fr, ARCHIVE v3 -> HEAD.

PÉRIMÈTRE dispatché (#1499 c.5777638672, +2 PK c.5777917978)
  G1 — 32 renommages + la collision PK 598/PK 2, + 2 nouveaux PK 603/680.
  Source du chiffre 32 : ledger #1503 (docs/corpus/ledger-cartes-existantes.md)
  mesure contre `Archive/v3/Argumentum Fallacies - Cards.csv` (169 cartes
  pré-agentique), pas contre la baseline 2024.

REFERENCE : `Cards/Fallacies/Archive/v3/Argumentum Fallacies - Cards.csv`
  169 cartes, clé candidate `path` (153/169 appariées, contre 29/169 pour PK).
  ⚠️ La clé PK a été ré-attribuée entre l'archive et HEAD (29 cas) — utiliser
  `path` comme jointure (celle du bridge instrument PR #1507).

JOINTURE : path (le chemin dans la taxonomie).
  Le bridge instrument #1507 gere les cas ou path change mais le nom reste ;
  ici, on mesure le changement de NOM, pas de chemin. Les PK 598, 603, 680
  (deplacé+renommé) restent dans le scope car `path` est resté stable entre
  baseline 2024 et HEAD (cf. spec : `path` HEAD = `path` baseline pour ces 3).

CLASSIFICATION (alignee sur ledger-instrument.py) :
  - IDENT  : nom inchange
  - C      : ne differe que par accents/casse/ponctuation (typographique, ignore)
  - SUBST  : reformulation / remplacement -> compte dans les renommages reels

TÉMOINS À CHAQUE RUN
  (a) archive lisible (v3 = 169 cartes)
  (b) jointure path HEAD >= 153 (cf. ledger) ; si inferieur, c'est l'instrument
  (c) au moins UN des 3 cas « déplacé+renommé » (598, 603, 680) doit sortir
  (d) la COLLISION PK 598/2 (« Généralisation hâtive » sur PK 2 en HEAD)
      doit sortir : PK 598 HEAD ≠ PK 2 HEAD (noms distincts après renommage)

CE QUE L'INSTRUMENT N'EST PAS : il mesure les renommages fr au niveau du nom,
jamais la qualite editoriale ; il ne CHIFFRE PAS la collision.

⛔ Aucun write CSV. Dossier d'arbitrage seulement.
"""
import csv
import io
import os
import re
import subprocess
import sys
import unicodedata

BASE = os.path.abspath(os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', '..'))
CSV_PATH = 'Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv'
ARCHIVE_PATH = 'Cards/Fallacies/Archive/v3/Argumentum Fallacies - Cards.csv'


def rows(repo_path):
    p = subprocess.run(['git', '-C', BASE, 'show', 'HEAD:%s' % repo_path],
                       capture_output=True)
    if p.returncode:
        sys.exit('illisible: HEAD:%s' % repo_path)
    return list(csv.DictReader(io.StringIO(p.stdout.decode('utf-8-sig', 'replace'))))


def nfc(s):
    return unicodedata.normalize('NFC', s or '')


def norm_ws(s):
    s = nfc(s).replace(' ', ' ').replace(' ', ' ')
    s = s.replace('’', "'").replace('‘', "'")
    s = s.replace('“', '"').replace('”', '"').replace('«', '"').replace('«', '"')
    s = s.replace('»', '"').replace('–', '-').replace('—', '-')
    return re.sub(r'\s+', ' ', s).strip()


def deaccent(s):
    return ''.join(c for c in unicodedata.normalize('NFD', s) if unicodedata.category(c) != 'Mn')


def skeleton(s):
    return re.sub(r'[^a-z0-9 ]', '', deaccent(norm_ws(s)).lower()).strip()


def classify(ref, now):
    r, n = norm_ws(ref), norm_ws(now)
    if r == n:
        return 'IDENT', ''
    if not r:
        return 'FILL', ''
    if not n:
        return 'VIDE', ''
    if skeleton(r) == skeleton(n):
        return 'C', 'typographique (accents/casse/ponctuation)'
    return 'SUBST', 'reformulation / remplacement'


def family_chemin(r):
    parts = [nfc(r.get('Family', '')).strip(), nfc(r.get('Sous-Famille', '')).strip(),
             nfc(r.get('Soussousfamille', '')).strip()]
    return ' / '.join(p for p in parts if p)


def rows_baseline(ref='62b561e75'):
    """Lire la baseline 2024 (commit 62b561e75)."""
    p = subprocess.run(['git', '-C', BASE, 'show', '%s:%s' % (ref, CSV_PATH)],
                       capture_output=True)
    if p.returncode:
        sys.exit('illisible: %s@%s' % (CSV_PATH, ref))
    return list(csv.DictReader(io.StringIO(p.stdout.decode('utf-8-sig', 'replace'))))


def main():
    head = rows(CSV_PATH)
    archive = rows(ARCHIVE_PATH)
    baseline = rows_baseline()
    print('lignes  HEAD=%d archive_v3=%d baseline_2024=%d' % (len(head), len(archive), len(baseline)))

    # HEAD : index by path
    H = {}
    H_pk = {}
    for r in head:
        path = nfc(r.get('path', '')).strip()
        if path:
            H[path] = r
        pk = nfc(r.get('PK', '')).strip()
        if pk:
            H_pk[pk] = r
    # ARCHIVE : index by path AND by name (pour bridge)
    A = {}
    A_by_name = {}
    for r in archive:
        path = nfc(r.get('path', '')).strip()
        if path:
            A[path] = r
        name = nfc(r.get('text_fr', '')).strip()
        if name:
            A_by_name[name] = r
    # BASELINE 2024 : index by name (le bridge name -> baseline -> PK)
    B_by_name = {}
    B_by_pk = {}
    for r in baseline:
        name = nfc(r.get('text_fr', '')).strip()
        if name:
            B_by_name[name] = r
        pk = nfc(r.get('PK', '')).strip()
        if pk:
            B_by_pk[pk] = r

    # (a) archive lisible
    a_ok = len(archive) == 169
    print('(a) archive v3 lisible 169 cartes : %d -> %s'
          % (len(archive), 'PASS' if a_ok else 'FAIL'))

    # PASS 1 : jointure directe archive v3 --(path)--> HEAD
    common = set(H) & set(A)
    b_ok = len(common) >= 153
    print('(b) jointure path >= 153 : %d communs -> %s'
          % (len(common), 'PASS' if b_ok else 'FAIL'))

    # Classification text_fr (PASS 1, par path)
    subst = []
    typo = []
    seen_pks = set()  # PKs déjà capturés en PASS 1
    for path in sorted(common):
        old = nfc(A[path].get('text_fr', '')).strip()
        new = nfc(H[path].get('text_fr', '')).strip()
        if not old and not new:
            continue
        cls, why = classify(old, new)
        if cls == 'IDENT':
            continue
        pk_head = nfc(H[path].get('PK', '')).strip()
        if cls == 'SUBST':
            subst.append({
                'path': path, 'pk': pk_head,
                'pk_archive': nfc(A[path].get('PK', '')).strip(),
                'old': old, 'new': new,
                'famille_head': family_chemin(H[path]),
            })
            seen_pks.add(pk_head)
        elif cls == 'C':
            typo.append({'path': path, 'pk': pk_head, 'old': old, 'new': new})

    # PASS 2 : bridge instrument pattern (PR #1507)
    # archive --(nom)--> baseline 2024 --(PK)--> HEAD
    # capture les cartes déplacées ET renommées (path changé entre archive et HEAD)
    print()
    print('=== PASS 2 : bridge archive --(nom)--> baseline 2024 --(PK)--> HEAD ===')
    bridge_count = 0
    for path_arch, r_arch in sorted(A.items()):
        name_arch = nfc(r_arch.get('text_fr', '')).strip()
        if not name_arch or name_arch not in B_by_name:
            continue
        # baseline name -> baseline row
        r_base = B_by_name[name_arch]
        pk_base = nfc(r_base.get('PK', '')).strip()
        if not pk_base or pk_base not in H_pk:
            continue
        r_head = H_pk[pk_base]
        # Si PK déjà vu en PASS 1 (path identique), skip
        if pk_base in seen_pks:
            continue
        # Si path HEAD == path archive, déjà compté en PASS 1
        path_head = nfc(r_head.get('path', '')).strip()
        if path_head == path_arch:
            continue
        # Cas spécial : déplacé ET renommé entre archive et HEAD
        old = name_arch
        new = nfc(r_head.get('text_fr', '')).strip()
        if not old or not new or old == new:
            continue
        cls, why = classify(old, new)
        if cls == 'SUBST':
            subst.append({
                'path': '%s -> %s' % (path_arch, path_head),
                'pk': pk_base, 'pk_archive': nfc(r_arch.get('PK', '')).strip(),
                'old': old, 'new': new,
                'famille_head': family_chemin(r_head),
            })
            seen_pks.add(pk_base)
            bridge_count += 1
    print('Cartes rattrapées par le bridge (déplacé+renommé) : %d' % bridge_count)

    print()
    print('Renommages SUBST (reformulation/remplacement) : %d' % len(subst))
    print('Corrections typographiques (C, ignorees)    : %d' % len(typo))

    # (c) au moins 1 des 3 « déplacé+renommé » doit sortir (598, 603, 680)
    cibles = {'598', '603', '680'}
    visibles = [r for r in subst if r['pk'] in cibles]
    c_ok = len(visibles) >= 1
    print('(c) PK 598/603/680 visibles en SUBST : %d -> %s'
          % (len(visibles), 'PASS' if c_ok else 'FAIL'))

    # (d) collision PK 598 / PK 2 — sur la HEAD courante (les deux PK existent)
    hk_598 = nfc(H_pk.get('598', {}).get('text_fr', '')).strip() if '598' in H_pk else None
    hk_2 = nfc(H_pk.get('2', {}).get('text_fr', '')).strip() if '2' in H_pk else None
    collision_head = hk_598 and hk_2 and hk_598 != hk_2
    print('(d) collision PK 598/2 HEAD : 598=%r 2=%r -> %s'
          % (hk_598, hk_2, 'PASS (collision)' if collision_head else 'FAIL'))

    print()
    print('=' * 110)
    print('TABLEAU D''ARBITRAGE — renommages SUBST text_fr Archive/v3 -> HEAD')
    print('=' * 110)
    print()
    print(f"{'PK':<6} {'path':<14} {'old (archive v3)':<48} {'new (HEAD)':<48}")
    print('-' * 175)
    for r in subst:
        old = r['old'][:46]
        new = r['new'][:46]
        print(f"{r['pk']:<6} {r['path']:<14} {old:<48} {new:<48}")

    print()
    print('=' * 110)
    print('CAS PARTICULIERS — collision + déplacé+renommé')
    print('=' * 110)
    print()
    print('Collision PK 598 / PK 2 (le défaut que G1 sert à détecter) :')
    print('  PK 598 HEAD : path=3.1.1.1.1 name=%s' % hk_598)
    print('  PK 2 HEAD   : path=1.1         name=%s' % hk_2)

    print()
    print('PK déplacés ET renommés (invisibles aux jointures par path OU par nom) :')
    for r in visibles:
        print('  PK %s : %s' % (r['pk'], r['path']))
        print('    old (archive v3): %s' % r['old'])
        print('    new (HEAD)      : %s' % r['new'])
        print('    famille HEAD    : %s' % r['famille_head'])
        print()

    print()
    print('=' * 110)
    print('SYNTHÈSE')
    print('=' * 110)
    print('  Renommages SUBST mesures : %d' % len(subst))
    print('  Corrections typo (C)     : %d' % len(typo))
    print('  Bridge (déplacé+renommé) : %d' % bridge_count)
    print('  Témoin (a) archive 169   : %s' % ('PASS' if a_ok else 'FAIL'))
    print('  Témoin (b) jointure 153+ : %s' % ('PASS' if b_ok else 'FAIL'))
    print('  Témoin (c) PK 598/603/680: %s' % ('PASS' if c_ok else 'FAIL'))
    print('  Témoin (d) collision 598/2: %s' % ('PASS' if collision_head else 'FAIL'))


if __name__ == '__main__':
    main()
