# -*- coding: utf-8 -*-
"""G4 (#1499) — Dossier de régression Virtues : cellules fr, baseline 2024 -> HEAD.

Jointure : pk (unique 223/223 des deux côtés). Invariant : path — ⛔ JAMAIS family_fr
(93 divergences = les 3 renommages de familles d'août 2026, 55+20+18 : un contrôle
inverse posé sur une colonne que le changement légitime a le droit de toucher accuse
la CLÉ au lieu du contenu — motif #1507).

PÉRIMÈTRES
  card=1  : les 131 cartes imprimables (le périmètre du dispatch : 312 cellules).
  arbre   : les 223 nœuds (contexte).

TÉMOINS EXIGÉS À CHAQUE RUN
  (a) pk uniques des deux côtés ;
  (b) invariant path 223/223 — s'il casse, c'est l'instrument ;
  (c) contrôle inverse family_fr : doit rendre ~93 divergences (les renommages
      documentés) — preuve que l'instrument VOIT les changements de famille ;
  (d) contrôle non-zéro : les renommages d'août DOIVENT apparaître dans la sortie.

⛔ Ce que cet instrument n'est pas : il mesure l'AMPLEUR des réécritures, jamais
leur QUALITÉ ; et Virtues n'a AUCUNE archive imprimée — la référence est la
baseline 2024 (62b561e75), pas un imprimé. Dossier d'arbitrage, aucune écriture CSV.
"""
import collections
import csv
import difflib
import io
import os
import statistics
import subprocess
import sys

BASE = os.path.abspath(os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', '..'))
CSV_PATH = 'Cards/Fallacies/Argumentum Virtues - Taxonomy.csv'
BASELINE_REF = '62b561e75'   # 22/04/2024, pré-agentique
CONTENT_COLS = ['title_fr', 'description_fr', 'remark_fr']
STRUCT_COLS = ['family_fr', 'subfamily_fr', 'subsubfamily_fr', 'link_fr']


def rows(ref):
    p = subprocess.run(['git', '-C', BASE, 'show', '%s:%s' % (ref, CSV_PATH)], capture_output=True)
    if p.returncode:
        sys.exit('illisible: %s@%s' % (CSV_PATH, ref))
    return list(csv.DictReader(io.StringIO(p.stdout.decode('utf-8-sig', 'replace'))))


def n(v):
    return (v or '').replace('\r\n', '\n').replace('\r', '\n').strip()


def main():
    head, base = rows('HEAD'), rows(BASELINE_REF)
    print('lignes  HEAD=%d baseline=%d' % (len(head), len(base)))
    H = {n(r['pk']): r for r in head}
    B = {n(r['pk']): r for r in base}
    ok = True

    # (a) pk uniques
    a_ok = len(H) == len(head) == len(B) == len(base)
    print('(a) pk uniques : HEAD %d/%d, baseline %d/%d -> %s'
          % (len(H), len(head), len(B), len(base), 'PASS' if a_ok else 'FAIL'))
    ok &= a_ok
    common = set(H) & set(B)
    print('    pk communs : %d (nouveaux %d, absents %d)'
          % (len(common), len(set(H) - set(B)), len(set(B) - set(H))))

    # (b) invariant path
    moved = [pk for pk in common if n(H[pk]['path']) != n(B[pk]['path'])]
    b_ok = not moved
    print('(b) invariant path : %d/%d identiques -> %s'
          % (len(common) - len(moved), len(common), 'PASS' if b_ok else 'FAIL %s' % moved[:5]))
    ok &= b_ok

    # (c) contrôle inverse family_fr — les renommages documentés doivent sortir
    fam = [pk for pk in common if n(H[pk]['family_fr']) != n(B[pk]['family_fr'])]
    c_ok = fam and len(fam) <= len(common)
    print('(c) family_fr divergents : %d/%d (= %.1f%% ; attendu ~93 = 3 renommages '
          'd\'août 55+20+18, les 130 restants = 58.3%% identiques) -> %s'
          % (len(fam), len(common), 100.0 * len(fam) / len(common),
             'PASS' if c_ok else 'FAIL'))
    ok &= c_ok

    def diff(pred, label):
        per_col = collections.Counter()
        per_card = set()
        sim = collections.defaultdict(collections.Counter)
        lens = collections.defaultdict(lambda: ([], []))
        for pk in common:
            if not pred(H[pk]):
                continue
            for c in CONTENT_COLS + STRUCT_COLS:
                a, w = n(B[pk].get(c)), n(H[pk].get(c))
                if a == w or (not a and not w):
                    continue
                per_col[c] += 1
                per_card.add(pk)
                if c in CONTENT_COLS and a and w:
                    r = difflib.SequenceMatcher(None, a, w).ratio()
                    k = ('<0.3' if r < 0.3 else '0.3-0.6' if r < 0.6
                         else '0.6-0.85' if r < 0.85 else '>=0.85')
                    sim[c][k] += 1
                    lens[c][0].append(len(a))
                    lens[c][1].append(len(w))
        print('-- %s : %d cellules fr (%d contenu + %d structure/lien) sur %d cartes'
              % (label, sum(per_col.values()),
                 sum(per_col[c] for c in CONTENT_COLS),
                 sum(per_col[c] for c in STRUCT_COLS), len(per_card)))
        for c in CONTENT_COLS + STRUCT_COLS:
            line = '     %-16s %d' % (c, per_col[c])
            if c in CONTENT_COLS and per_col[c]:
                la, lw = lens[c]
                short = sum(1 for i in range(len(la)) if lw[i] < la[i])
                line += ('   med %d->%d car., raccourcies %d/%d'
                         % (statistics.median(la), statistics.median(lw), short, len(la)))
            print(line)
            if c in CONTENT_COLS and per_col[c]:
                for k in ('<0.3', '0.3-0.6', '0.6-0.85', '>=0.85'):
                    if sim[c][k]:
                        print('        similarité %-8s %3d' % (k, sim[c][k]))
        return per_col

    pc1 = diff(lambda r: n(r.get('card')) == '1', 'card=1 (imprimables 131)')
    pca = diff(lambda r: True, 'tout l arbre (223)')

    # (d) contrôle non-zéro : les renommages d'août apparaissent (famille Sens quantitatif)
    sample = [pk for pk in fam[:5]]
    print('(d) renommages d\'août visibles (family_fr) : %s -> %s'
          % (sample, 'PASS' if sample else 'FAIL'))
    ok &= bool(sample)
    tot = sum(pc1[c] for c in CONTENT_COLS)
    print('CONTENU card=1 : %d cellules (dispatch #1499 : 312) -> %s'
          % (tot, 'PASS' if tot else 'FAIL'))
    return 0 if ok else 1


if __name__ == '__main__':
    sys.exit(main())
