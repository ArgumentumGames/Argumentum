# -*- coding: utf-8 -*-
"""Pont d'appariement carte imprimee <-> archive du depot, quand `path` ne suffit plus.

POURQUOI CE SECOND INSTRUMENT
-----------------------------
`ledger-instrument.py` joint sur `path` et mesure 153/175 cartes appariees. Il etiquette les
22 restantes « cartes nouvelles ». ⛔ C'est FAUX pour 15 d'entre elles : la taxonomie a ete
RESTRUCTUREE entre l'archive v3 et la baseline 2024, et `path` est precisement ce que cette
restructuration deplace (mesure : 34 lignes d'archive changent de path a nom constant).

⭐ La lecon generale : un invariant de jointure doit survivre au changement legitime. Choisi sur
ce que le changement recherche a le droit de toucher, il accuse la CLE au lieu du CONTENU.
Ici `path` bouge (restructuration) ET le nom bouge (renommages agentiques 2025+) — aucune des
deux cles ne tient seule.

LE PONT
-------
    archive --(nom)--> baseline 2024 --(PK)--> HEAD

Maillon 1 sain parce que la derive de NOMS est posterieure a 2024 : en 2024 les noms sont encore
ceux des archives. Maillon 2 deja valide ailleurs (invariant `Famille` 175/175 sur PK).

⛔ CE QUE LE PONT N'ETABLIT PAS. Il apparie, il ne juge pas : qu'une carte soit retrouvee en
archive ne dit rien sur la qualite du texte actuel. Et un appariement reste a confirmer a l'oeil
quand le nom a change — c'est un candidat, pas une preuve.

CONTROLE INVERSE INCLUS. (a) une jointure par nom SEULE apparierait a tort « Generalisation
hative » de HEAD (branche Insuffisance) avec celle de v3 (branche mathematique) : 0 niveau de
famille commun. (b) Le pont doit laisser des irresolues ; s'il resout 22/22, il est trop permissif.
"""
import csv, io, os, subprocess, sys, unicodedata

BASE = os.path.abspath(os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', '..'))
DECK = 'Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv'
BASELINE_REF = '62b561e75'   # contenu du 22/04/2024, pre-agentique
ARCHIVES = [
    ('v3',     'Cards/Fallacies/Archive/v3/Argumentum Fallacies - Cards.csv'),
    ('v3pp',   'Cards/Fallacies/Archive/v3/Argumentum Fallacies - Cards Print and Play.csv'),
    ('2022',   'Cards/Fallacies/Archive/2022/'
               'Argumentum Fallacies - Cards - edition fevrier 2022.csv'),
    ('2022pp', 'Cards/Fallacies/Archive/2022/'
               'Argumentum Fallacies - Cards - edition fevrier 2022 - Print and Play.csv'),
]


def rows(path, ref='HEAD'):
    """Lit via `git show` : l'instrument doit pouvoir lire la BASELINE, pas seulement l'arbre."""
    p = subprocess.run(['git', 'show', '%s:%s' % (ref, path)], capture_output=True, cwd=BASE)
    if p.returncode:
        sys.stderr.write('illisible: %s@%s\n' % (path, ref))
        return []
    return list(csv.DictReader(io.StringIO(p.stdout.decode('utf-8-sig', 'replace'))))


def n(v):
    return (v or '').strip()


def key(s):
    """Comparaison de noms : casse, accents, apostrophes typographiques."""
    s = n(s).lower().replace(u'’', "'")
    return u''.join(c for c in unicodedata.normalize('NFD', s) if unicodedata.category(c) != 'Mn')


def famille(r):
    parts = [n(r.get(c)) for c in ('Famille', 'Sous-Famille', 'Soussousfamille')]
    return ' > '.join([p for p in parts if p])


def main():
    head, base = rows(DECK), rows(DECK, BASELINE_REF)
    deck = [r for r in head if n(r.get('carte'))]
    if len(deck) != 175:
        sys.stderr.write('ATTENDU 175 cartes imprimees, mesure %d -- instrument suspect\n' % len(deck))
        return 2

    base_by_pk = {n(r.get('PK')): r for r in base if n(r.get('PK'))}
    arch = [(t, r) for t, p in ARCHIVES for r in rows(p)]
    arch_paths = set(n(r.get('path')) for _, r in arch if n(r.get('path')))

    # --- controle inverse (a) : la jointure par nom seule est-elle dangereuse ? ---
    piege = key(u'Généralisation hâtive')
    h = [r for r in deck if key(r.get('text_fr')) == piege]
    a = [r for _, r in arch if key(r.get('text_fr')) == piege]
    if h and a:
        print('controle inverse (a) -- jointure par NOM seule :')
        print('   HEAD  path %-14s %s' % (n(h[0].get('path')), famille(h[0])[:52]))
        print('   arch. path %-14s => 0 niveau de famille commun : appariement FAUX'
              % n(a[0].get('path')))
    else:
        print('controle inverse (a) INOPERANT : le temoin a disparu du corpus -- revoir le temoin')

    # --- sante du MAILLON 1 : le nom apparie-t-il l'archive a la baseline 2024 ? ---
    # C'est la seule justification du pont : si ce taux etait bas, le maillon serait sans valeur.
    # Le `path` qui bouge a nom constant est la mesure DIRECTE de la restructuration de taxonomie.
    base_by_name = {}
    for r in base:
        if key(r.get('text_fr')):
            base_by_name.setdefault(key(r.get('text_fr')), []).append(r)
    m1_ok = m1_moved = m1_miss = 0
    for _t, a in arch:
        k = key(a.get('text_fr'))
        if not k:
            continue
        hit = base_by_name.get(k)
        if hit and len(hit) == 1:
            m1_ok += 1
            if n(hit[0].get('path')) != n(a.get('path')):
                m1_moved += 1
        else:
            m1_miss += 1   # absent, ou nom ambigu dans la baseline -> inexploitable
    print('\nmaillon 1 -- archive --(nom)--> baseline 2024 : %d/%d noms uniques appaires (%.1f%%)'
          % (m1_ok, m1_ok + m1_miss, 100.0 * m1_ok / max(1, m1_ok + m1_miss)))
    print('   dont %d a `path` DIFFERENT => mesure directe de la restructuration de taxonomie'
          % m1_moved)
    if m1_moved == 0:
        print('   ⛔ 0 deplacement mesure : si `path` n\'a pas bouge, le pont n\'a pas lieu d\'etre')

    # --- appariement etage 1 : `path` ---
    apparie = [r for r in deck if n(r.get('path')) in arch_paths]
    orphelines = [r for r in deck if n(r.get('path')) not in arch_paths]
    print('\netage 1 -- jointure `path`   : %d/%d appariees, %d orphelines'
          % (len(apparie), len(deck), len(orphelines)))

    # --- appariement etage 2 : le pont ---
    ponte, irresolues, renommees = [], [], []
    for r in orphelines:
        b = base_by_pk.get(n(r.get('PK')))
        if not b:
            irresolues.append((r, 'PK absent de la baseline 2024'))
            continue
        bk = key(b.get('text_fr'))
        hits = [(t, x) for t, x in arch if key(x.get('text_fr')) == bk]
        if not hits:
            irresolues.append((r, 'nom de 2024 absent de toute archive'))
            continue
        ponte.append((r, b, hits[0]))
        if bk != key(r.get('text_fr')):
            renommees.append((r, b, hits[0]))

    print('etage 2 -- pont via baseline : %d resolues, %d irresolues'
          % (len(ponte), len(irresolues)))
    print('\nCOUVERTURE REELLE : %d/%d  (⛔ pas %d : les %d orphelines de `path` ne sont PAS '
          'des cartes nouvelles)' % (len(apparie) + len(ponte), len(deck), len(apparie),
                                     len(orphelines)))

    # --- controle inverse (b) : le pont doit rater ---
    if not irresolues:
        sys.stderr.write('\nCONTROLE INVERSE (b) ROUGE : le pont resout tout -- trop permissif, '
                         '⛔ ne pas publier ses chiffres\n')
        return 3
    print('controle inverse (b) : %d irresolues -- le pont n\'est pas trop permissif' % len(irresolues))

    if renommees:
        print('\n--- retrouvees SOUS UN AUTRE NOM (renommees depuis 2024) ---')
        for r, b, (t, x) in renommees:
            print('   PK %-6s %-14s "%s"  <--  2024 "%s"  <--  %s path %s'
                  % (n(r.get('PK')), n(r.get('path')), n(r.get('text_fr')),
                     n(b.get('text_fr')), t, n(x.get('path'))))

    print('\n--- AUCUNE ARCHIVE, sous aucun des deux noms : exemplaire PHYSIQUE seul ---')
    for r, why in irresolues:
        print('   PK %-6s %-14s %-34s (%s)'
              % (n(r.get('PK')), n(r.get('path')), n(r.get('text_fr')), why))
    return 0


if __name__ == '__main__':
    sys.exit(main())
