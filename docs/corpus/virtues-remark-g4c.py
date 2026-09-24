# -*- coding: utf-8 -*-
"""G4-C (#1499) — Liste courte des pertes remark_fr Virtues (pré-#367 -> HEAD).

Mandat (Q-7, owner 23/09 : « la réduction de taille est souhaitable » = règle C,
défaut garder HEAD) : sur les remarks les plus réécrits (similarité < 0,3 — le
« 45 » du dossier G4 §4), relever SEULEMENT les pertes (fait, nuance, exactitude)
ou les erreurs introduites. ⛔ Aucune écriture CSV.

COUCHES (consigne #458 c.5801633624 : « l'ancien » = version d'avant la couche
agentique) :
  BASELINE_2024 = 62b561e75 (22/04/2024)  — baseline du dossier G4 ;
  PRE367       = b76af806^ (27/05/2026)   — veille du « FR clarity pass » #367 ;
  HEAD         = arbre de travail.
L'ancien de la comparaison principale = PRE367 (dispatch : « comparées au
pré-#367 »). Le témoin (c) mesure 2024 vs pré-#367 sur les colonnes fr : tout
écart y est un geste D'ÉPOQUE (pré-couche agentique ou passes i18n de printemps
2026 ne touchant en principe pas le fr) — à porter en section séparée, jamais
compté comme perte agentique.

Jointure : pk. Invariant : path (⛔ pas family_fr — motif #1507). Similarité :
difflib.SequenceMatcher(None, a, w).ratio() sur textes normalisés — MÊME métrique
que virtues-baseline-diff.py (dossier G4), pour que les chiffres se recoupent.

TÉMOINS À CHAQUE RUN
  (a) pk uniques 223/223 aux trois refs ;
  (b) invariant path 223/223 (HEAD vs pré-#367) ;
  (c) 2024 vs pré-#367 sur title_fr/description_fr/remark_fr : comptage des
      écarts d'époque (attendu faible ; ≠ 0 => section séparée du dossier) ;
  (d) contrôle non-zéro : #367 visible (remark_fr divergents pré-#367->HEAD).

SORTIE
  Résumé sur stdout. Avec --pairs <chemin> : fichier TSV des paires sim<0,3
  (pk, path, card, sim, longueurs, les deux textes ENTIERS) pour lecture
  cellule par cellule — le fichier vit hors du dépôt (scratchpad).
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
BASELINE_2024 = '62b561e75'
PRE367 = 'b76af806^'
CONTENT_COLS = ['title_fr', 'description_fr', 'remark_fr']


def rows(ref):
    p = subprocess.run(['git', '-C', BASE, 'show', '%s:%s' % (ref, CSV_PATH)], capture_output=True)
    if p.returncode:
        sys.exit('illisible: %s@%s' % (CSV_PATH, ref))
    return list(csv.DictReader(io.StringIO(p.stdout.decode('utf-8-sig', 'replace'))))


def n(v):
    return (v or '').replace('\r\n', '\n').replace('\r', '\n').strip()


def sim(a, w):
    return difflib.SequenceMatcher(None, a, w).ratio()


def main():
    argv = sys.argv[1:]
    pairs_path = None
    if '--pairs' in argv:
        pairs_path = argv[argv.index('--pairs') + 1]

    head, pre, b24 = rows('HEAD'), rows(PRE367), rows(BASELINE_2024)
    print('lignes  HEAD=%d  pré-#367=%d  2024=%d' % (len(head), len(pre), len(b24)))
    H = {n(r['pk']): r for r in head}
    P = {n(r['pk']): r for r in pre}
    B = {n(r['pk']): r for r in b24}
    ok = True

    a_ok = len(H) == len(head) == len(P) == len(pre) == len(B) == len(b24)
    print('(a) pk uniques : HEAD %d/%d, pré-#367 %d/%d, 2024 %d/%d -> %s'
          % (len(H), len(head), len(P), len(pre), len(B), len(b24), 'PASS' if a_ok else 'FAIL'))
    ok &= a_ok
    common = set(H) & set(P) & set(B)
    print('    pk communs : %d' % len(common))

    moved = [pk for pk in common if n(H[pk]['path']) != n(P[pk]['path'])]
    b_ok = not moved
    print('(b) invariant path (HEAD vs pré-#367) : %d/%d -> %s'
          % (len(common) - len(moved), len(common), 'PASS' if b_ok else 'FAIL %s' % moved[:5]))
    ok &= b_ok

    # (c) écarts d'époque : 2024 vs pré-#367 sur les colonnes fr
    epoch = collections.defaultdict(list)
    for pk in sorted(common, key=lambda k: int(k) if k.isdigit() else 0):
        for c in CONTENT_COLS:
            a, w = n(B[pk].get(c)), n(P[pk].get(c))
            if a != w and (a or w):
                epoch[c].append(pk)
    tot_epoch = sum(len(v) for v in epoch.values())
    for c in CONTENT_COLS:
        if epoch[c]:
            print('    (c) époque %s : %d — %s' % (c, len(epoch[c]), epoch[c][:12]))
    print('(c) écarts d\'époque 2024->pré-#367 (fr) : %d cellules -> %s'
          % (tot_epoch, 'PASS (section séparée si > 0)' if tot_epoch >= 0 else 'FAIL'))
    ok &= tot_epoch >= 0

    # Mesure principale : remark_fr pré-#367 -> HEAD
    changed = 0
    emptied = 0
    filled = 0
    sims, lens = [], ([], [])
    low = []
    for pk in sorted(common, key=lambda k: int(k) if k.isdigit() else 0):
        a, w = n(P[pk].get('remark_fr')), n(H[pk].get('remark_fr'))
        if a == w:
            continue
        changed += 1
        if a and not w:
            emptied += 1
            continue
        if not a and w:
            filled += 1
            continue
        r = sim(a, w)
        sims.append(r)
        lens[0].append(len(a))
        lens[1].append(len(w))
        if r < 0.3:
            low.append((pk, r, a, w))
    diverged = [pk for pk in common
                if n(P[pk].get('remark_fr')) != n(H[pk].get('remark_fr'))]
    d_ok = len(diverged) == changed
    print('(d) contrôle non-zéro (#367 visible) : remark_fr divergents %d/%d -> %s'
          % (changed, len(common), 'PASS' if d_ok and changed else 'FAIL'))
    ok &= d_ok and bool(changed)

    la, lw = lens
    short = sum(1 for i in range(len(la)) if lw[i] < la[i])
    buckets = collections.Counter('<0.3' if r < 0.3 else '0.3-0.6' if r < 0.6
                                  else '0.6-0.85' if r < 0.85 else '>=0.85' for r in sims)
    print('remark_fr pré-#367->HEAD : %d changées (vidées %d, remplies %d), médiane %d->%d car., '
          'raccourcies %d/%d' % (changed, emptied, filled,
                                 statistics.median(la), statistics.median(lw), short, len(la)))
    for k in ('<0.3', '0.3-0.6', '0.6-0.85', '>=0.85'):
        if buckets[k]:
            print('    similarité %-8s %3d' % (k, buckets[k]))
    low_cards = [t for t in low if n(H[t[0]].get('card')) == '1']
    print('sim<0,3 : %d au total, dont card=1 : %d (dossier G4 §4 : « 45 »)' % (len(low), len(low_cards)))

    if pairs_path:
        with open(pairs_path, 'w', encoding='utf-8', newline='') as f:
            f.write('pk\tpath\tcard\ttitle_fr\tsim\tlen_pre\tlen_head\n')
            for pk, r, a, w in low:
                t = H[pk]
                f.write('%s\t%s\t%s\t%s\t%.3f\t%d\t%d\n' % (pk, n(t['path']), n(t.get('card', '')),
                                                             n(t.get('title_fr', '')).replace('\t', ' '),
                                                             r, len(a), len(w)))
                f.write('--- PRÉ-#367 ---\n%s\n--- HEAD ---\n%s\n===\n' % (a, w))
        print('paires sim<0,3 écrites : %s' % pairs_path)
    return 0 if ok else 1


if __name__ == '__main__':
    sys.exit(main())
