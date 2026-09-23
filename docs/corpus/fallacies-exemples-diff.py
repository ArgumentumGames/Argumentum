# -*- coding: utf-8 -*-
"""G2 (#1499) — Dossier d'arbitrage Fallacies : champ `example_fr` par COUCHE.

PÉRIMÈTRE dispatché (pool #458 c.5784688699, lane po-2024 v12)
  G2 — 145 `example_fr` (chiffre du dispatch, mesuré PRÉ-erratum #1516 ;
  ce chiffre se re-mesure ici sur l'arbre courant).

RÈGLE EPIC (#1499 c.5787733627) : séparer les couches PAR PK contre la
baseline `62b561e75` avant de présenter un volume « contre l'imprimé ».
Un tableau d'arbitrage porte 3 colonnes : imprimé / baseline / HEAD.

RÉFÉRENCES (3, par ordre d'autorité décroissant)
  - `Archive/2022/...edition fevrier 2022.csv` — l'IMPRIMÉ (autorité max)
  - `Archive/v3/...Cards.csv` — deck pré-agentique (169 cartes, étage 2)
  - `62b561e75` baseline canonique (22/04/2024) — référence des COUCHES
    ⚠️ la baseline n'est PAS l'imprimé (« annuler l'agentique » et
    « retour à l'imprimé » sont deux issues distinctes — review #1515).
  - HEAD — corpus courant.

ARCHITECTURE (motif G1 v3 + motif ledger #1503/#1516)
  Contrôle 5 : la jointure `path` peut apparier des SŒURS renumérotées ;
  on ré-apparie par TITRE dans le sens archive → HEAD uniquement
  (⛔ le sens inverse n'est pas une preuve : PK 2 porte le titre imprimé
  de PK 598 alors que le contenu confirme la jointure path).
  Passe 1 : imprimé (2022 > v3 par étage, jointure path + contrôle 5) →
            HEAD, filtre `carte ≠ vide`.
  Passe 2 : PK baseline imprimées → HEAD (capte les PK sans jointure
            path : 492/1357 sans référence, 598/603/680 sous un autre
            path — cf. #1507).

CLASSES (ledger-instrument.py, inchangées)
  IDENT / C / TRONQ / AJOUT / SUBST / FILL / VIDE.
  SUBST = reformulation ou remplacement → LA classe à arbitrer.

COUCHES (par PK contre baseline, sur example_fr)
  agentique       : baseline ≠ HEAD (le geste date d'après 2024).
  pre-agentique   : baseline = HEAD (le geste est d'époque owner).
  hors-deck       : PK absente du deck HEAD (retirée, ex 96 par #1288).

RÈGLE D'ARBITRAGE : surface = couche agentique, classes SUBST/TRONQ/VIDE.
FILL (remplissage d'un vide imprimé) et AJOUT (l'imprimé inclus tel quel)
sont des gestes additifs, pas des réécritures — hors surface par défaut.

TÉMOINS À CHAQUE RUN
  (a) archives lisibles : 2022 + v3 (169)
  (b) baseline imprimées ∩ HEAD par PK = 175 (176 - PK 96, #1288)
  (c) PK 598/603/680 visibles par la passe 2
  (d) témoin positif : path 2.1 (PK 176) doit rendre ≥ 1 cellule
      example_fr non-IDENT (contrôle inverse 2 du ledger #1503)

CE QUE L'INSTRUMENT N'EST PAS : il ne tranche aucune valeur éditoriale.
La similarité en caractères SOUS-ESTIME les reformulations (leçon
#1516) : les extraits sont livrés pour lecture humaine, la classe
mécanique est un point de départ, pas un verdict.
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
ARCHIVE_2022 = 'Cards/Fallacies/Archive/2022/Argumentum Fallacies - Cards - edition fevrier 2022.csv'
ARCHIVE_V3 = 'Cards/Fallacies/Archive/v3/Argumentum Fallacies - Cards.csv'
BASELINE_REF = '62b561e75'
FIELD = 'example_fr'


def git_show(commit, path):
    p = subprocess.run(['git', '-C', BASE, 'show', '%s:%s' % (commit, path)],
                       capture_output=True)
    if p.returncode:
        sys.exit('illisible: %s@%s' % (path, commit))
    return p.stdout.decode('utf-8-sig', 'replace')


def rows(content):
    return list(csv.DictReader(io.StringIO(content)))


def nfc(s):
    return unicodedata.normalize('NFC', s or '')


def norm_ws(s):
    s = nfc(s).replace(' ', ' ').replace(' ', ' ')
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
        return 'IDENT', 'inchangé'
    if not r:
        return 'FILL', 'vide à la référence → remplissage, pas une réécriture'
    if not n:
        return 'VIDE', 'cellule vidée'
    if skeleton(r) == skeleton(n):
        return 'C', 'typographique seul'
    if skeleton(n) and skeleton(n) in skeleton(r):
        return 'TRONQ', 'le texte actuel est contenu dans la référence → retrait'
    if skeleton(r) in skeleton(n):
        return 'AJOUT', 'la référence est contenuse dans le texte actuel → ajout en place'
    return 'SUBST', 'reformulation ou remplacement'


def card_non_vide(r):
    return bool(nfc(r.get('carte', '')).strip())


def main():
    head = rows(git_show('HEAD', CSV_PATH))
    a22 = rows(git_show('HEAD', ARCHIVE_2022))
    a3 = rows(git_show('HEAD', ARCHIVE_V3))
    baseline = rows(git_show(BASELINE_REF, CSV_PATH))
    print('lignes  HEAD=%d  arch2022=%d  archv3=%d  baseline=%d'
          % (len(head), len(a22), len(a3), len(baseline)))

    # ---- index cartes (carte ≠ vide) ----
    H_path, H_pk = {}, {}
    for r in head:
        p, pk = nfc(r.get('path', '')).strip(), nfc(r.get('PK', '')).strip()
        if p and card_non_vide(r):
            H_path[p] = r
        if pk and card_non_vide(r):
            H_pk[pk] = r
    A_path = {}   # imprimé par path, étage 2022 > v3
    for src, coll in (('2022', a22), ('v3', a3)):
        for r in coll:
            p = nfc(r.get('path', '')).strip()
            if p and card_non_vide(r) and p not in A_path:
                A_path[p] = (src, r)
    B_pk_impr = {}
    for r in baseline:
        pk = nfc(r.get('PK', '')).strip()
        if pk and card_non_vide(r):
            B_pk_impr[pk] = r

    # ---- CONTRÔLE 5 : sœurs renumérotées, ré-appariement par titre ----
    # Sens archive -> HEAD UNIQUEMENT (cf. docstring).
    H_by_title = {}
    for p, r in H_path.items():
        t = skeleton(r.get('text_fr', ''))
        if t:
            H_by_title.setdefault(t, p)
    misjoined, repair = {}, {}
    for p, (src, r) in A_path.items():
        t = skeleton(r.get('text_fr', ''))
        q = H_by_title.get(t)
        if q is not None and q != p:
            misjoined[p] = q
    for p, q in misjoined.items():
        own = A_path.get(q)
        if own is None or skeleton(own[1].get('text_fr', '')) != skeleton(H_path[q].get('text_fr', '')):
            repair[q] = p   # carte HEAD q <- imprimé du path p

    # ---- PASSE 1 : imprimé -> HEAD (path + contrôle 5), example_fr ----
    out = []
    seen_pk = set()
    for p in sorted(H_path):
        r_h = H_path[p]
        pk = nfc(r_h.get('PK', '')).strip()
        ref_row, ref_src, ref_path = None, None, None
        if p in repair:
            ref_src, ref_row = A_path[repair[p]]
            ref_path = repair[p]
        elif p in A_path and p not in [q for q in misjoined.values()]:
            ref_src, ref_row = A_path[p]
            ref_path = p
        # p appartient aux cibles de misjoined -> carte insérée, sans ancêtre
        if ref_row is None:
            continue
        cls, why = classify(ref_row.get(FIELD), r_h.get(FIELD))
        if cls == 'IDENT':
            seen_pk.add(pk)
            continue
        out.append(dict(pk=pk, path=p, ref_path=ref_path, src=ref_src, passe=1,
                        cls=cls, why=why,
                        ref=norm_ws(ref_row.get(FIELD)), now=norm_ws(r_h.get(FIELD))))
        seen_pk.add(pk)

    # ---- PASSE 2 : PK baseline imprimées -> HEAD ----
    for pk, r_b in sorted(B_pk_impr.items()):
        if pk not in H_pk or pk in seen_pk:
            continue
        r_h = H_pk[pk]
        cls, why = classify(r_b.get(FIELD), r_h.get(FIELD))
        if cls == 'IDENT':
            continue
        out.append(dict(pk=pk, path=nfc(r_h.get('path', '')).strip(), ref_path='', src='baseline',
                        passe=2, cls=cls, why=why,
                        ref=norm_ws(r_b.get(FIELD)), now=norm_ws(r_h.get(FIELD))))

    # ---- COUCHES par PK contre baseline ----
    for r in out:
        pk = r['pk']
        if pk not in B_pk_impr:
            r['couche'] = 'hors-deck'
        else:
            b = norm_ws(B_pk_impr[pk].get(FIELD))
            h = norm_ws(H_pk[pk].get(FIELD)) if pk in H_pk else r['now']
            r['couche'] = 'pre-agentique' if b == h else 'agentique'
            if r['couche'] == 'agentique':
                cb, _ = classify(b, h)
                if cb == 'C':
                    r['couche'] = 'typo-seul-agent'

    # ---- CONTRÔLE 6 : SUBST = texte DÉPLACÉ verbatim ? ----
    arch_skel, head_skel = {}, {}
    for p_a, (src, r) in A_path.items():
        arch_skel.setdefault(skeleton(r.get(FIELD)), set()).add(p_a)
    for p, r in H_path.items():
        head_skel.setdefault(skeleton(r.get(FIELD)), set()).add(p)
    moved = []
    for r in out:
        if r['cls'] != 'SUBST' or len(skeleton(r['now'])) <= 12:
            continue
        if ((arch_skel.get(skeleton(r['now']), set()))
                or (head_skel.get(skeleton(r['ref']), set()) - {r['path']})):
            moved.append(r['pk'])

    # ---- TÉMOINS ----
    a_ok = len(a22) > 0 and len(a3) == 169
    b_ok = len(set(B_pk_impr) & set(H_pk)) == 175
    # (c) couverture des PK créées après l'archive : présentes dans `out`
    # (écart mesuré, toute passe) OU identiques contre la baseline (mesuré
    # aussi — pas de saut silencieux possible).
    couv = []
    for pk3 in ('598', '603', '680'):
        ecart = any(r['pk'] == pk3 for r in out)
        ident_base = (pk3 in B_pk_impr and pk3 in H_pk
                      and norm_ws(B_pk_impr[pk3].get(FIELD)) == norm_ws(H_pk[pk3].get(FIELD)))
        couv.append((pk3, ecart or ident_base))
    c_ok = all(ok for _, ok in couv)
    d_ok = any(r['path'] == '2.1' for r in out)
    print('(a) archives lisibles (2022=%d, v3=%d attendu 169) : %s'
          % (len(a22), len(a3), 'PASS' if a_ok else 'FAIL'))
    print('(b) baseline imprimées ∩ HEAD par PK = 175 : %d : %s'
          % (len(set(B_pk_impr) & set(H_pk)), 'PASS' if b_ok else 'FAIL'))
    print('(c) PK 598/603/680 couverts (écart mesuré ou IDENT baseline) : %s %s'
          % ('PASS' if c_ok else 'FAIL', couv))
    print('(d) témoin path 2.1 (PK 176) example_fr non-IDENT : %s'
          % ('PASS' if d_ok else 'FAIL'))
    print('contrôle 5 : %d jointures path entre sœurs ré-appariées %s'
          % (len(repair), sorted(repair.items())))
    print('contrôle 6 : SUBST = déplacement verbatim : %d %s' % (len(moved), sorted(moved)))

    # ---- VENTILATION ----
    from collections import Counter
    print()
    print('VENTILATION example_fr (classes vs imprimé × couche baseline)')
    cnt = Counter((r['cls'], r['couche']) for r in out)
    couches = ['agentique', 'pre-agentique', 'typo-seul-agent', 'hors-deck']
    classes = ['SUBST', 'TRONQ', 'VIDE', 'AJOUT', 'FILL', 'C']
    print('  %-8s %s' % ('classe', ''.join('%18s' % c for c in couches) + '%8s' % 'TOTAL'))
    for c in classes:
        line = ''.join('%18d' % cnt[(c, k)] for k in couches)
        print('  %-8s %s%8d' % (c, line, sum(cnt[(c, k)] for k in couches)))
    print('  TOTAL   %s%8d'
          % (''.join('%18d' % sum(cnt[(c, k)] for c in classes) for k in couches), len(out)))
    surface = sum(1 for r in out
                  if r['couche'] == 'agentique' and r['cls'] in ('SUBST', 'TRONQ', 'VIDE'))
    print()
    print('  TOTAL lignes example_fr non-IDENT : %d (dispatch v12 : 145, pré-erratum #1516)'
          % len(out))
    print('  SURFACE arbitrage (agentique × SUBST/TRONQ/VIDE) : %d' % surface)

    # ---- TABLEAU (extraits 48c) ----
    print()
    print('TABLEAU — surface agentique SUBST/TRONQ/VIDE (extraits)')
    print()
    print(f"{'PK':<6} {'path':<12} {'p':<3} {'cls':<6} {'référence (p1=imprimé, p2=baseline)':<50} {'HEAD':<50}")
    print('-' * 125)
    for r in sorted(out, key=lambda r: int(r['pk']) if r['pk'].isdigit() else 0):
        if r['couche'] == 'agentique' and r['cls'] in ('SUBST', 'TRONQ', 'VIDE'):
            print(f"{r['pk']:<6} {r['path'][:10]:<12} {r['passe']:<3} {r['cls']:<6} "
                  f"{r['ref'][:46]:<50} {r['now'][:46]:<50}")

    import json
    json.dump(out, io.open(os.path.join(os.path.dirname(os.path.abspath(__file__)),
                                        'fallacies-exemples-diff.json'), 'w',
                           encoding='utf-8'), ensure_ascii=False, indent=1)
    print()
    print('-> %d lignes dans fallacies-exemples-diff.json' % len(out))
    if not (a_ok and b_ok and c_ok and d_ok):
        sys.exit(1)


if __name__ == '__main__':
    main()
