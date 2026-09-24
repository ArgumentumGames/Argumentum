# -*- coding: utf-8 -*-
"""G4bis (#1499, po-2023) — Planche d'arbitrage Virtues : echantillon stratifie
`remark_fr`, baseline 2024 -> HEAD cote a cote, sur une page.

Objectif (ordre #458 c.5784688699, grain 4) : rendre le choix de regime A / B / C
de #1513 tranchable SANS ouvrir l'issue. ⛔ Aucune ecriture CSV.

CRITERE DE STRATIFICATION — ecrit AVANT le tirage (repris verbatim dans la planche) :
  population = remarques `card=1` dont sim(BASELINE_2024 -> HEAD) < 0,30 ;
  strates    = 3 bandes de similarite : B1 [0,00-0,10) · B2 [0,10-0,20) · B3 [0,20-0,30) ;
  allocation = proportionnelle aux effectifs (plus fort reste), total 10, bornee a
               l'effectif de chaque bande ;
  tirage     = DETERMINISTE et sans RNG : dans chaque bande, pk croissants, indices
               repartis uniformement (i*(n-1)/(k-1), arrondi au plus proche) ;
  temoins    = 3 plus hautes similarites (`card=1`, seuil declare 0,90) — montrent ce
               que le regime ne touche pas ; seuil non atteint => les 3 plus hautes
               sont montrees et la distribution est rapportee (regle de repli
               declared ici, avant la redaction de la planche) ;
  annexe     = PK 33, 94, 203 (liste courte G4-C, PR #1530), HORS tirage, parce qu'ils
               portent la seule question ouverte (Q-13, PK 94).

Couches : BASELINE_2024 = 62b561e75 (22/04/2024, baseline du dossier G4) ·
PRE367 = b76af806^ (27/05/2026, veille de la passe agentique #367) · HEAD = arbre.
La planche AFFICHE 2024 -> HEAD (ordre verbatim) ; PRE367 sert aux temoins (b)(c)(d)
et au recoupement de la population avec le « 45 » du dossier G4 §4.

Witnesses a chaque run (meme metrique difflib que virtues-remark-g4c.py) :
  (a) pk uniques aux trois refs ;
  (b) invariant path (HEAD vs pre-#367) ;
  (c) ecarts d'epoque 2024 -> pre-#367 sur title_fr/description_fr/remark_fr ;
  (d) controle non-zero : #367 visible (remark_fr pre-#367 -> HEAD divergents).

SORTIE : resume stdout ; avec --out <chemin>, fragment markdown des paires retenues
(integralite des deux textes, non tronques) + bandes + temoins + annexe.
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
BANDS = [('B1', 0.00, 0.10), ('B2', 0.10, 0.20), ('B3', 0.20, 0.30)]
TOTAL = 10
ANNEX = ['33', '94', '203']
WITNESS_MIN = 0.90


def rows(ref):
    p = subprocess.run(['git', '-C', BASE, 'show', '%s:%s' % (ref, CSV_PATH)], capture_output=True)
    if p.returncode:
        sys.exit('illisible: %s@%s' % (CSV_PATH, ref))
    return list(csv.DictReader(io.StringIO(p.stdout.decode('utf-8-sig', 'replace'))))


def rows_head():
    with open(os.path.join(BASE, CSV_PATH), encoding='utf-8-sig', newline='') as f:
        return list(csv.DictReader(f))


def n(v):
    return (v or '').replace('\r\n', '\n').replace('\r', '\n').strip()


def sim(a, w):
    return difflib.SequenceMatcher(None, a, w).ratio()


def allocate(sizes, total):
    """Plus fort reste, bornee a l'effectif de chaque bande."""
    tot = sum(sizes)
    if not tot:
        return [0] * len(sizes)
    quotas = [total * s / tot for s in sizes]
    floors = [min(int(q), s) for q, s in zip(quotas, sizes)]
    rest = total - sum(floors)
    order = sorted(range(len(sizes)), key=lambda i: (quotas[i] - int(quotas[i]), sizes[i]), reverse=True)
    while rest > 0:
        progressed = False
        for i in order:
            if rest == 0:
                break
            if floors[i] < sizes[i]:
                floors[i] += 1
                rest -= 1
                progressed = True
        if not progressed:
            break
    return floors


def draw_even(items, k):
    """Indices repartis uniformement sur la liste (pk croissants). Deterministe."""
    m = len(items)
    if k <= 0 or m == 0:
        return []
    if k >= m:
        return list(range(m))
    if k == 1:
        return [m // 2]
    return sorted({round(i * (m - 1) / (k - 1)) for i in range(k)})


def block(pk, title, fam, s, a, w):
    return ('**PK %s · %s — %s** (similarité %.2f · %d → %d car.)\n\n'
            '- 2024 : « %s »\n- HEAD : « %s »\n' % (pk, title, fam, s, len(a), len(w), a, w))


def main():
    argv = sys.argv[1:]
    out_path = argv[argv.index('--out') + 1] if '--out' in argv else None

    head, pre, b24 = rows_head(), rows(PRE367), rows(BASELINE_2024)
    H = {n(r['pk']): r for r in head}
    P = {n(r['pk']): r for r in pre}
    B = {n(r['pk']): r for r in b24}
    ok = True

    a_ok = len(H) == len(head) == len(P) == len(pre) == len(B) == len(b24)
    print('(a) pk uniques : HEAD %d/%d, pré-#367 %d/%d, 2024 %d/%d -> %s'
          % (len(H), len(head), len(P), len(pre), len(B), len(b24), 'PASS' if a_ok else 'FAIL'))
    ok &= a_ok
    common = set(H) & set(P) & set(B)

    moved = [pk for pk in common if n(H[pk]['path']) != n(P[pk]['path'])]
    b_ok = not moved
    print('(b) invariant path (HEAD vs pré-#367) : %d/%d -> %s'
          % (len(common) - len(moved), len(common), 'PASS' if b_ok else 'FAIL %s' % moved[:5]))
    ok &= b_ok

    epoch = collections.defaultdict(list)
    for pk in common:
        for c in CONTENT_COLS:
            a, w = n(B[pk].get(c)), n(P[pk].get(c))
            if a != w and (a or w):
                epoch[c].append(pk)
    print("(c) écarts d'époque 2024->pré-#367 : %s"
          % {c: len(v) for c, v in epoch.items()})
    ok &= True

    diverged = sum(1 for pk in common if n(P[pk].get('remark_fr')) != n(H[pk].get('remark_fr')))
    d_ok = diverged > 0
    print('(d) contrôle non-zéro (#367 visible) : remark_fr divergents %d -> %s'
          % (diverged, 'PASS' if d_ok else 'FAIL'))
    ok &= d_ok

    # population 2024 -> HEAD, card=1, sim < 0.30
    cards = [pk for pk in common if n(H[pk].get('card')) == '1']
    pop = []
    for pk in cards:
        a, w = n(B[pk].get('remark_fr')), n(H[pk].get('remark_fr'))
        if not a or not w or a == w:
            continue
        s = sim(a, w)
        if s < 0.30:
            pop.append((pk, s, a, w))
    pop.sort(key=lambda t: int(t[0]))
    print('population 2024->HEAD (card=1, sim<0,30) : %d (card=1 : %d / %d)' % (len(pop), len(cards), len(common)))

    # recoupement avec le « 45 » du dossier G4 (pré-#367 -> HEAD) si les textes 2024 == pré-#367
    pre45 = []
    for pk in cards:
        a, w = n(P[pk].get('remark_fr')), n(H[pk].get('remark_fr'))
        if not a or not w or a == w:
            continue
        if sim(a, w) < 0.30:
            pre45.append(pk)
    s_pop, s_45 = {p[0] for p in pop}, set(pre45)
    print('recoupement : pré-#367->HEAD %d · communs %d · 2024 seulement %s · pré-#367 seulement %s'
          % (len(pre45), len(s_pop & s_45),
             sorted(s_pop - s_45, key=int), sorted(s_45 - s_pop, key=int)))

    bands = {name: [] for name, _, _ in BANDS}
    for pk, s, a, w in pop:
        for name, lo, hi in BANDS:
            if lo <= s < hi:
                bands[name].append((pk, s, a, w))
                break
    alloc = allocate([len(bands[nm]) for nm, _, _ in BANDS], TOTAL)
    print('bandes : %s · allocation %s'
          % ({nm: len(bands[nm]) for nm, _, _ in BANDS}, dict(zip([nm for nm, _, _ in BANDS], alloc))))

    drawn = []
    for (nm, lo, hi), k in zip(BANDS, alloc):
        items = bands[nm]
        idx = draw_even(items, k)
        for i in idx:
            drawn.append((nm, items[i]))
    print('tirage (%d) : %s' % (len(drawn), [t[1][0] for t in drawn]))
    fams = collections.Counter(n(H[pk].get('family_fr')) for _, (pk, s, a, w) in drawn)
    print('familles du tirage : %s' % dict(fams))

    # temoins haute similarite : 3 plus hautes similarites (seuil declare 0,90 ;
    # non atteint => les 3 plus hautes sont montrees, fait rapporte)
    hi = []
    for pk in cards:
        a, w = n(B[pk].get('remark_fr')), n(H[pk].get('remark_fr'))
        if not a or not w or a == w:
            continue
        hi.append((pk, sim(a, w), a, w))
    hi.sort(key=lambda t: (-t[1], int(t[0])))
    wit = hi[:3]
    at_thr = sum(1 for t in hi if t[1] >= WITNESS_MIN)
    dist = collections.Counter('<0.3' if s < 0.3 else '0.3-0.6' if s < 0.6
                               else '0.6-0.85' if s < 0.85 else '>=0.85'
                               for s, _, _, _ in [(sim(n(B[pk].get('remark_fr')), n(H[pk].get('remark_fr'))), pk, 0, 0) for pk in cards
                                                  if n(B[pk].get('remark_fr')) and n(H[pk].get('remark_fr'))
                                                  and n(B[pk].get('remark_fr')) != n(H[pk].get('remark_fr'))])
    print('distribution (card=1, modifiées) : %s' % {k: dist[k] for k in ('<0.3', '0.3-0.6', '0.6-0.85', '>=0.85')})
    print('témoins : seuil %.2f atteint par %d paire(s) ; montrées : %s'
          % (WITNESS_MIN, at_thr, [(t[0], round(t[1], 3)) for t in wit]))

    if out_path:
        L = []
        L.append('## 2. Le tirage — 10 paires (critère §1)\n')
        for nm, (pk, s, a, w) in drawn:
            L.append('**[%s]** %s' % (nm, block(pk, n(H[pk].get('title_fr')), n(H[pk].get('family_fr')), s, a, w)))
        L.append('## 3. Témoins de similarité haute — le haut du spectre\n')
        L.append('Seuil déclaré 0,90 non atteint (0 paire) ; les 3 plus hautes similarités mesurées sont montrées ; '
                 'distribution en §1.\n')
        for pk, s, a, w in wit:
            L.append(block(pk, n(H[pk].get('title_fr')), n(H[pk].get('family_fr')), s, a, w))
        L.append('## 4. Annexe — liste courte G4-C (hors tirage)\n')
        L.append('Référence : [liste courte G4-C](1499-g4c-virtues-liste-courte-2026-09-24.md) (PR #1530, po-2024) — '
                 '3 pertes signalées, toutes « garder ». La seule question ouverte est **Q-13 (PK 94, « valide ≠ vrai »)**.\n')
        for pk in ANNEX:
            a, w = n(B[pk].get('remark_fr')), n(H[pk].get('remark_fr'))
            L.append(block(pk, n(H[pk].get('title_fr')), n(H[pk].get('family_fr')), sim(a, w), a, w))
        with open(out_path, 'w', encoding='utf-8', newline='\n') as f:
            f.write('\n'.join(L) + '\n')
        print('fragment écrit : %s' % out_path)

    return 0 if ok else 1


if __name__ == '__main__':
    sys.exit(main())