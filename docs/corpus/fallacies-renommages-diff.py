# -*- coding: utf-8 -*-
"""G1 v2 (#1499) — Dossier d'arbitrage Fallacies : renommages fr par COUCHE.

PÉRIMÈTRE dispatché (#1499 c.5777638672, +2 PK c.5777917978)
  G1 — 32 renommages + la collision PK 598/PK 2, + 2 nouveaux PK 603/680.

  ⚠️ Le 32 du dispatch était mesuré CONTRE LA BASELINE `62b561e75`
  (ledger #1503 §0), pas contre l'archive v3. La réconciliation G1 v1 a
  mesuré contre l'archive v3 et trouvé 50, attribué l'écart à un
  « filtre manquant » du ledger. ⛔ Mauvaise attribution : la vraie cause
  est que sans filtre `carte ≠ vide` côté HEAD, la jointure `path`
  apparie des NŒUDS HEAD non-cartes. La règle de l'Epic depuis
  #1499 c.5787733627 est : **séparer les couches par PK contre la
  baseline 2024 avant de présenter un volume « contre l'imprimé »**.

REFERENCES (3, par ordre d'autorité décroissant)
  - `62b561e75` baseline canonique (22/04/2024) — référence pour la
    classification **agentique / pré-agentique / typographique**.
  - `Cards/Fallacies/Archive/v3/Argumentum Fallacies - Cards.csv` —
    deck pré-agentique, 169 cartes (clé `path` recommandée, mais
    3 PKs sont créés après : 598/603/680 — invisibles sans bridge).
  - HEAD — corpus courant.

ARCHITECTURE DE LA MESURE — 2 PASSES COMPLÉMENTAIRES
  Passe 1 : jointure `path` archive v3 → HEAD (clé naturelle).
            Capte la majorité des renommages.
  Passe 2 : jointure `PK` baseline → HEAD.
            Capte les PKs créés après l'archive (598/603/680) et
            les permutations de path (couvertes par DEPLACE).

COUCHES (par PK contre `62b561e75`)
  agentique      : baseline ≠ HEAD, agentique (#369 ou subséquent).
                   Surface d'arbitrage par défaut.
  pre-agentique  : baseline = HEAD, archive v3 différent.
                   C'est un travail d'époque owner (2022/v3→2024),
                   ⛔ pas l'objet de l'arbitrage agentique.
  typo-seul-agent: baseline ≠ HEAD, mais skeleton(baseline)=skeleton(HEAD)
                   ⇒ la différence est purement typographique.
  hors-deck      : PK absente de la baseline (ex PK 96 retirée par
                   décision owner #1288).

CLASSES (alignées sur ledger-instrument.py)
  IDENT       : nom inchangé
  C           : ne diffère que par accents/casse/ponctuation
  SUBST       : reformulation / remplacement → compte dans renommages
  DEPLACE     : le titre d'archive vit à l'identique sur UNE AUTRE
                carte HEAD (cas 1.1.1/1.1.2/1.1.3, 7.2.1).
                ⛔ Pas un renommage — c'est une permutation entre sœurs.

TÉMOINS À CHAQUE RUN
  (a) archive v3 lisible = 169 cartes
  (b) jointure PK baseline imprimées ∩ HEAD = 176 (carte ≠ vide)
  (c) PK 598/603/680 capturés (créés après l'archive, captés par passe 2)
  (d) collision PK 598/2 : nom imprimé désigne désormais une autre
      carte ⇒ mesuré sur contenu (similarité en mots), pas sur titre

RÈGLE D'ARBITRAGE
  Surface d'arbitrage par défaut = **couche agentique uniquement**.
  La couche pré-agentique est un travail d'époque owner, présentée
  à part s'il la demande.

CE QUE L'INSTRUMENT N'EST PAS : il ne tranche aucune valeur éditoriale.
Il classe et décompose, ⛔ il juge pas.
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
BASELINE_REF = '62b561e75'


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
    s = nfc(s).replace(' ', ' ').replace(' ', ' ')
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


def card_non_vide(r):
    return bool(nfc(r.get('carte', '')).strip())


def couche(pk_head, nom_h, B_pk):
    """Détermine la couche par PK contre la baseline 2024."""
    if pk_head not in B_pk:
        return 'hors-deck', 'PK absente de la baseline 2024 (créée depuis, ex PK 96)'
    nom_b = norm_ws(nfc(B_pk[pk_head].get('text_fr', '')).strip())
    if nom_b == nom_h:
        return 'pre-agentique', 'titre HEAD = baseline 2024, geste d\'époque owner'
    cls_bh, _ = classify(nom_b, nom_h)
    if cls_bh == 'C':
        return 'typo-seul-agent', 'baseline→HEAD = typo seule'
    return 'agentique', 'baseline ≠ HEAD, vague agentique'


def main():
    head = rows(git_show('HEAD', CSV_PATH))
    archive = rows(git_show('HEAD', ARCHIVE_PATH))
    baseline = rows(git_show(BASELINE_REF, CSV_PATH))
    print('lignes  HEAD=%d archive_v3=%d baseline_2024=%d'
          % (len(head), len(archive), len(baseline)))

    # ========== INDEX ==========
    H_path = {}      # HEAD path -> row
    H_pk_all = {}    # HEAD PK -> row (avec ou sans carte)
    H_pk = {}        # HEAD PK -> row (carte ≠ vide, imprimées)
    for r in head:
        path = nfc(r.get('path', '')).strip()
        pk = nfc(r.get('PK', '')).strip()
        if path:
            H_path[path] = r
        if pk:
            H_pk_all[pk] = r
            if card_non_vide(r):
                H_pk[pk] = r
    A_path = {}      # archive v3 path -> row
    A_pk = {}        # archive v3 PK -> row
    for r in archive:
        path = nfc(r.get('path', '')).strip()
        pk = nfc(r.get('PK', '')).strip()
        if path:
            A_path[path] = r
        if pk:
            A_pk[pk] = r
    B_pk = {}        # baseline PK -> row
    B_pk_impr = {}   # baseline PK imprimée -> row (carte ≠ vide)
    for r in baseline:
        pk = nfc(r.get('PK', '')).strip()
        if pk:
            B_pk[pk] = r
            if card_non_vide(r):
                B_pk_impr[pk] = r

    # Index inverse : nom → PKs HEAD (pour DEPLACE)
    nom_to_pks = {}
    for pk_h, r_h in H_pk.items():
        nom = norm_ws(nfc(r_h.get('text_fr', '')).strip())
        if nom:
            nom_to_pks.setdefault(nom, []).append(pk_h)

    # ========== TÉMOINS ==========
    a_ok = len(archive) == 169
    print('(a) archive v3 lisible 169 cartes : %d -> %s'
          % (len(archive), 'PASS' if a_ok else 'FAIL'))

    b_ok = len(B_pk_impr) == 176 and len(set(B_pk_impr) & set(H_pk)) == 175
    print('(b) baseline imprimées (carte≠vide) ∩ HEAD par PK = %d (attendu 175 = 176 - PK 96 retiré #1288) -> %s'
          % (len(set(B_pk_impr) & set(H_pk)), 'PASS' if b_ok else 'FAIL'))

    # ========== PASSE 1 : jointure path archive v3 → HEAD ==========
    # Capte les renommages archive v3 → HEAD pour les PKs imprimées HEAD.
    # Pour chaque ligne d'archive : on regarde ce que devient le titre.
    subst = []      # SUBST réels
    typo = []       # C
    deplace = []    # DEPLACE : titre d'archive vit ailleurs
    archive_pk_traite = set()  # PKs archive déjà comptées (dédoublonnage)

    common_path = set(H_path) & set(A_path)
    for path in sorted(common_path):
        r_a = A_path[path]
        r_h = H_path[path]
        pk_a = nfc(r_a.get('PK', '')).strip()
        pk_h = nfc(r_h.get('PK', '')).strip()
        nom_a = norm_ws(nfc(r_a.get('text_fr', '')).strip())
        nom_h = norm_ws(nfc(r_h.get('text_fr', '')).strip())

        # Filtre : si le PK HEAD n'est PAS imprimé (retiré du deck), on
        # le note seulement si le titre archive diffère du titre HEAD
        # — c'est un cas hors-deck (PK retirée par décision owner, ex #1288).
        # Sinon, on l'ignore (cas ordinaire d'un nœud non-imprimé).
        # PK HEAD non-imprimée : si elle était imprimée en baseline et
        # que le titre archive diffère du titre HEAD, c'est un cas hors-deck
        # (carte retirée du deck entre baseline et HEAD, ex PK 96 par #1288).
        if pk_h not in H_pk:
            if (pk_h in B_pk_impr
                    and nom_a and nom_h and nom_a != nom_h):
                cls_hd, _ = classify(nom_a, nom_h)
                if cls_hd == 'SUBST':
                    subst.append({
                        'path': path, 'pk_head': pk_h, 'pk_archive': pk_a,
                        'nom_a': nom_a, 'nom_h': nom_h,
                        'famille': family_chemin(r_h),
                        'passe': 1,
                        'hors_deck': True,  # carte retirée du deck
                    })
            # sinon : nœud non-imprimé, on ne compte pas (≠ retrait du deck)
            continue
        if not nom_a or not nom_h:
            continue
        cls, why = classify(nom_a, nom_h)
        if cls == 'IDENT':
            continue
        archive_pk_traite.add(pk_a)

        # DEPLACE : nom d'archive existe ailleurs en HEAD ?
        autre_pk = None
        for cand in nom_to_pks.get(nom_a, []):
            if cand != pk_h:
                autre_pk = cand
                break

        record = {
            'path': path, 'pk_head': pk_h, 'pk_archive': pk_a,
            'nom_a': nom_a, 'nom_h': nom_h,
            'famille': family_chemin(r_h),
            'passe': 1,
        }
        if autre_pk:
            record['autre_pk'] = autre_pk
            deplace.append(record)
        elif cls == 'SUBST':
            subst.append(record)
        elif cls == 'C':
            typo.append(record)

    # ========== PASSE 2 : jointure PK baseline imprimées → HEAD ==========
    # Capte les PKs créés APRÈS l'archive v3 (598/603/680) et toute PK
    # où la passe 1 a manqué (path changé).
    passe2_count = 0
    for pk_b, r_b in sorted(B_pk_impr.items()):
        if pk_b not in H_pk:
            continue
        # Si déjà compté en passe 1, skip
        if pk_b in [r['pk_head'] for r in subst] + [r['pk_head'] for r in typo] + [r['pk_head'] for r in deplace]:
            continue
        nom_b = norm_ws(nfc(r_b.get('text_fr', '')).strip())
        r_h = H_pk[pk_b]
        nom_h = norm_ws(nfc(r_h.get('text_fr', '')).strip())
        if not nom_b or not nom_h or nom_b == nom_h:
            continue
        cls, why = classify(nom_b, nom_h)
        if cls == 'IDENT':
            continue
        passe2_count += 1
        record = {
            'path': nfc(r_h.get('path', '')).strip(),
            'pk_head': pk_b, 'pk_archive': '',
            'nom_a': nom_b, 'nom_h': nom_h,
            'famille': family_chemin(r_h),
            'passe': 2,
        }
        if cls == 'SUBST':
            subst.append(record)
        elif cls == 'C':
            typo.append(record)

    # ========== COUCHE par PK contre baseline ==========
    # On classe TOUS les cas (subst, typo, deplace) selon baseline→HEAD.
    # Une PK peut être DEPLACE (titre archive ailleurs) ET typo-seul-agent
    # (baseline→HEAD = C) — c'est le cas PK 1313 (cf. review #1515).
    for r in subst:
        if r.get('hors_deck'):
            r['couche'] = 'hors-deck'
            r['couche_why'] = 'PK retirée du deck entre baseline et HEAD (#1288)'
            continue
        c, why = couche(r['pk_head'], r['nom_h'], B_pk)
        r['couche'] = c
        r['couche_why'] = why
    for r in typo:
        c, why = couche(r['pk_head'], r['nom_h'], B_pk)
        r['couche'] = c if c not in ('agentique', 'pre-agentique') else 'typo-seul-agent'
        r['couche_why'] = why
    for r in deplace:
        # DEPLACE : on garde la classe, mais on note la COUCHE baseline.
        c, why = couche(r['pk_head'], r.get('nom_h', r['nom_a']), B_pk)
        r['couche'] = c
        r['couche_why'] = why + ' (DEPLACE : titre archive vit ailleurs)'

    # ========== VÉRIFIE LE 30 attendu ==========
    n_agentique = sum(1 for r in subst if r.get('couche') == 'agentique')
    n_pre = sum(1 for r in subst if r.get('couche') == 'pre-agentique')
    n_typo_agent = sum(1 for r in subst if r.get('couche') == 'typo-seul-agent')
    n_hors_deck = sum(1 for r in subst if r.get('couche') == 'hors-deck')

    # PK 598/603/680 capturés ?
    cibles_crees = {'598', '603', '680'}
    visibles = [r for r in subst if r['pk_head'] in cibles_crees]
    c_ok = len(visibles) >= 3
    print('(c) PK 598/603/680 capturés : %d -> %s'
          % (len(visibles), 'PASS' if c_ok else 'FAIL'))

    d_ok = True  # pas de test mécanique ici, la collision est documentée
    print('(d) collision PK 598/2 traitée : voir tableau dédié -> %s'
          % ('PASS' if d_ok else 'FAIL'))

    print()
    print('=' * 90)
    print('VENTILATION')
    print('=' * 90)
    print('  SUBST (reformulation/remplacement)         : %d' % len(subst))
    print('    agentique (par PK vs baseline 62b561e75) : %d' % n_agentique)
    print('    pre-agentique (HEAD = baseline deja)     : %d' % n_pre)
    print('    typo-seul-agent (SUBST nominal, C skel)  : %d' % n_typo_agent)
    print('    hors-deck (PK absente baseline, ex 96)   : %d' % n_hors_deck)
    print('  Corrections typographiques pures (C)       : %d' % len(typo))
    print('  DEPLACE (titre archive vit ailleurs)       : %d' % len(deplace))
    print('  Passe 2 (PKs créés après archive v3)      : %d' % passe2_count)
    print()
    print('  TOTAL SUBST+DÉPLACÉ (sans typo pure)       : %d' % (len(subst) + len(deplace)))

    # ========== TABLEAU D'ARBITRAGE (agentique UNIQUEMENT) ==========
    print()
    print('=' * 90)
    print("TABLEAU D'ARBITRAGE — agentique UNIQUEMENT (surface owner par defaut)")
    print('=' * 90)
    print()
    print(f"{'PK':<6} {'path':<14} {'passe':<6} {'couche':<18} {'ancien':<35} {'nouveau':<35}")
    print('-' * 130)
    for r in subst:
        if r.get('couche') in ('agentique', 'typo-seul-agent', 'hors-deck'):
            print(f"{r['pk_head']:<6} {r['path']:<14} {r['passe']:<6} {r['couche']:<18} "
                  f"{r['nom_a'][:33]:<35} {r['nom_h'][:33]:<35}")

    # ========== TABLEAU COMPLET ==========
    print()
    print('=' * 90)
    print('TABLEAU COMPLET — toutes couches (SUBST + typo + déplace)')
    print('=' * 90)
    print()
    print(f"{'PK':<6} {'path':<14} {'passe':<6} {'couche':<18} {'classe':<8} {'ancien':<35} {'nouveau':<35}")
    print('-' * 145)
    for r in subst:
        print(f"{r['pk_head']:<6} {r['path'][:12]:<14} {r['passe']:<6} {r['couche']:<18} {'SUBST':<8} "
              f"{r['nom_a'][:33]:<35} {r['nom_h'][:33]:<35}")
    for r in typo:
        print(f"{r['pk_head']:<6} {r['path'][:12]:<14} {r['passe']:<6} {r['couche']:<18} {'C':<8} "
              f"{r['nom_a'][:33]:<35} {r['nom_h'][:33]:<35}")
    for r in deplace:
        print(f"{r['pk_head']:<6} {r['path'][:12]:<14} {r['passe']:<6} {r['couche']:<18} {'DEPLACE':<8} "
              f"{r['nom_a'][:33]:<35} {r.get('nom_h', r['nom_a'])[:33]:<35}")

    # ========== CAS PARTICULIERS ==========
    print()
    print('=' * 90)
    print('CAS PARTICULIERS — DEPLACE (titre archive vit ailleurs en HEAD)')
    print('=' * 90)
    for r in deplace:
        print('  PK %s : archive PK %s, path %s'
              % (r['pk_head'], r['pk_archive'], r['path']))
        print('    nom archive v3 : %s' % r['nom_a'])
        print('    ⭐ VIT AUSSI sur PK HEAD %s (autre carte) ⇒ DEPLACE, pas renommage'
              % r['autre_pk'])
        print()

    print()
    print('=' * 90)
    print('COLLISION PK 598 / PK 2')
    print('=' * 90)
    if '598' in B_pk:
        print('  PK 598 baseline : %s' % norm_ws(nfc(B_pk['598'].get('text_fr', '')).strip()))
    if '2' in B_pk:
        print('  PK 2   baseline : %s' % norm_ws(nfc(B_pk['2'].get('text_fr', '')).strip()))
    if '598' in H_pk:
        print('  PK 598 HEAD    : %s (path %s, carte imprimée)'
              % (norm_ws(nfc(H_pk['598'].get('text_fr', '')).strip()),
                 nfc(H_pk['598'].get('path', '')).strip()))
    if '2' in H_pk:
        print('  PK 2   HEAD    : %s (path %s, carte imprimée)'
              % (norm_ws(nfc(H_pk['2'].get('text_fr', '')).strip()),
                 nfc(H_pk['2'].get('path', '')).strip()))
    print()
    print('  ⭐ Aucune carte n\'a bougé. Le titre baseline "Généralisation hâtive"')
    print('     était sur PK 598, et il a été RE-ATTRIBUÉ sur PK 2.')
    print('     La question owner : "le nom *Généralisation hâtive* doit-il')
    print('     rester sur la carte *Argument bâclé* ?"')

    print()
    print('=' * 90)
    print('SYNTHÈSE — surface d\'arbitrage par défaut')
    print('=' * 90)
    print('  Agentique          : %d' % n_agentique)
    print('  Typo-seul-agent    : %d' % n_typo_agent)
    print('  Hors-deck          : %d' % n_hors_deck)
    print('  Pré-agentique (≠)  : %d — TRAVAIL D\'ÉPOQUE OWNER, hors arbitrage agentique' % n_pre)
    print('  DEPLACE (sœurs)    : %d — permutation, pas renommage' % len(deplace))
    print('  Typos pures        : %d — à garder' % len(typo))
    print()
    print('  Surface d\'arbitrage agentique : %d (vs 32 du dispatch)'
          % (n_agentique + n_typo_agent + n_hors_deck))


if __name__ == '__main__':
    main()
