# -*- coding: utf-8 -*-
"""G1-W — instrument d'impact : plan d'écriture des titres et bandeaux.

Spec : pool #458 c.5794922231 (méthode/DoD) + c.5795213716 (complément,
REMPLACE les listes) + c.5795375786 (PK 598). Base : origin/master 9fe92572.

  (1) 40 titres text_<lang> depuis 9d45b4f9^ / 74557ea6^ / 97431d64^,
      sauf 1287 fr := bandeau actuel (U+2019).
  (2) bandeaux des têtes gardes : 726, 758, 1023, 1312 (niv. 2, Sous-Famille)
      et 992, 1352, 826, 1361 (niv. 3, Soussousfamille), la tête et toutes
      ses descendantes, 8 langues ; cellules vides pré-existantes non
      remplies (listées). 1287 : AUCUN bandeau (le titre revient au nom
      que portent déjà les bandeaux).
  (3) comptes attendus (pool, recalculés ici) : 40 titres ; 482 bandeaux
      dont 58 imprimées.
  (4) contrôles finaux simulés : invariant bandeau = titre sur les 18 têtes
      décidées ; unicité des titres dans les 8 langues après écriture.

Sortie : g1w-plan.json (liste des mutations PK/ligne/colonne/ancien/nouveau).
rc != 0 si un compte ou un contrôle diverge.
"""
import csv
import io
import json
import os
import subprocess
import sys

BASE = os.path.abspath(os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', '..'))
CSV_PATH = 'Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv'
OUT = os.path.dirname(os.path.abspath(__file__))
LANGS = ['fr', 'en', 'ru', 'pt', 'ar', 'es', 'fa', 'zh']

# (1) titres : PK -> (ref source, [langues])
TITRES = {
    '2':    ('9d45b4f9^', ['fr', 'en', 'ru', 'pt', 'ar', 'es']),
    '176':  ('9d45b4f9^', ['fr', 'fa']),
    '632':  ('9d45b4f9^', ['fr']),
    '666':  ('9d45b4f9^', ['fr']),
    '888':  ('9d45b4f9^', ['fr', 'en', 'ar']),
    '973':  ('9d45b4f9^', ['fr']),
    '134':  ('9d45b4f9^', ['fr']),
    '713':  ('9d45b4f9^', ['fr', 'ru', 'pt', 'ar', 'zh', 'fa']),
    '1287': ('9d45b4f9^', LANGS),
    '658':  ('9d45b4f9^', ['fr']),
    '799':  ('9d45b4f9^', ['fr', 'en', 'ru']),
    '492':  ('74557ea6^', ['fr']),
    '598':  ('97431d64^', ['fr', 'en', 'ru', 'pt', 'ar', 'es']),
}
GARDES_NIV2 = ['726', '758', '1023', '1312']
GARDES_NIV3 = ['992', '1352', '826', '1361']
TETES_DECIDEES = ['2', '176', '632', '666', '888', '973', '134', '1287', '658', '799',
                  '726', '758', '1023', '1312', '992', '1352', '826', '1361']


def git_show(ref):
    p = subprocess.run(['git', '-C', BASE, 'show', '%s:%s' % (ref, CSV_PATH)], capture_output=True)
    if p.returncode:
        sys.exit('illisible @ %s' % ref)
    return list(csv.DictReader(io.StringIO(p.stdout.decode('utf-8-sig', 'replace'))))


def bandeau_col(niveau, lang):
    if niveau == 2:
        return 'Sous-Famille' if lang == 'fr' else ('Subfamily' if lang == 'en' else 'Subfamily_' + lang)
    return 'Soussousfamille' if lang == 'fr' else ('Subsubfamily' if lang == 'en' else 'Subsubfamily_' + lang)


def nfc(s):
    import unicodedata
    return unicodedata.normalize('NFC', s or '').strip()


def main():
    head = git_show('HEAD')
    H = {}
    for i, r in enumerate(head):
        pk = nfc(r.get('PK'))
        if pk:
            H[pk] = (i, r)

    src_cache = {}
    def src_row(ref, pk):
        if ref not in src_cache:
            m = {}
            for r in git_show(ref):
                p = nfc(r.get('PK'))
                if p:
                    m[p] = r
            src_cache[ref] = m
        return src_cache[ref].get(pk)

    plan = []   # mutations : dict(pk, ligne(index), col, ancien, nouveau)

    # ---- (1) titres ----
    for pk, (ref, langs) in sorted(TITRES.items(), key=lambda kv: int(kv[0])):
        r_src = src_row(ref, pk)
        if r_src is None:
            sys.exit('PK %s absente de %s' % (pk, ref))
        i, r_h = H[pk]
        for l in langs:
            cible = nfc(r_src.get('text_' + l))
            if pk == '1287' and l == 'fr':
                # consigne c.5795213716 : chaîne EXACTE du bandeau (U+2019)
                cible = nfc(r_h.get('Soussousfamille'))
                assert '’' in cible, 'bandeau 1287 sans U+2019 ?!'
            ancien = nfc(r_h.get('text_' + l))
            if ancien != cible:
                plan.append(dict(pk=pk, ligne=i, col='text_' + l, ancien=ancien, nouveau=cible))

    # ---- (2) bandeaux des gardes ----
    vides_toleres = []
    for pk in GARDES_NIV2 + GARDES_NIV3:
        i_t, r_t = H[pk]
        path_t = nfc(r_t.get('path'))
        niveau = 2 if pk in GARDES_NIV2 else 3
        # arbre : la tête + toutes les descendantes (path préfixe)
        cibles = [(i_t, r_t)] + [
            (i, r) for i, r in enumerate(head)
            if nfc(r.get('path')).startswith(path_t + '.') and nfc(r.get('PK'))
        ]
        for l in LANGS:
            col = bandeau_col(niveau, l)
            titre_apres = nfc(r_t.get('text_' + l))   # garde : titre inchangé
            if not titre_apres:
                continue
            for i, r in cibles:
                cur = nfc(r.get(col))
                if not cur:
                    vides_toleres.append((nfc(r.get('PK')), l, nfc(r.get('path'))))
                    continue
                if cur != titre_apres:
                    plan.append(dict(pk=nfc(r.get('PK')), ligne=i, col=col,
                                     ancien=cur, nouveau=titre_apres))

    # ---- (3) comptes ----
    n_titres = len([m for m in plan if m['col'].startswith('text_')])
    n_bandeaux = len(plan) - n_titres
    n_bandeaux_impr = sum(1 for m in plan
                          if not m['col'].startswith('text_')
                          and nfc(head[m['ligne']].get('carte')))
    print('PLAN : %d titres (attendu 40), %d bandeaux (attendu 482), '
          '%d bandeaux imprimés (attendu 58), %d vides tolérés'
          % (n_titres, n_bandeaux, n_bandeaux_impr, len(vides_toleres)))
    ok3 = (n_titres, n_bandeaux, n_bandeaux_impr) == (40, 482, 58)

    # ---- (4) contrôles finaux simulés ----
    # simulation : appliquer le plan en mémoire
    sim = [dict(r) for r in head]
    for m in plan:
        sim[m['ligne']][m['col']] = m['nouveau']
    S = {}
    for r in sim:
        pk = nfc(r.get('PK'))
        if pk:
            S[pk] = r

    # invariant bandeau = titre, 18 têtes, 8 langues
    inv_ecarts = []
    for pk in TETES_DECIDEES:
        r_t = S[pk]
        path_t = nfc(r_t.get('path'))
        niveau = len(path_t.split('.'))
        for l in LANGS:
            col = bandeau_col(niveau, l)
            titre = nfc(r_t.get('text_' + l))
            if not titre:
                continue
            for r in sim:
                p = nfc(r.get('path'))
                if (p == path_t or p.startswith(path_t + '.')) and nfc(r.get('PK')):
                    cur = nfc(r.get(col))
                    if cur and cur != titre:
                        inv_ecarts.append((nfc(r.get('PK')), l, col))

    # unicité des titres, 8 langues (cartes imprimées). Le DoD vise les
    # doublons CRÉÉS par l'écriture ; les pré-existants sont recensés.
    def doublons(corpus):
        d = set()
        for l in LANGS:
            seen = {}
            for r in corpus:
                if not nfc(r.get('carte')):
                    continue
                t = nfc(r.get('text_' + l))
                if not t:
                    continue
                if t in seen:
                    d.add((l, t, seen[t], nfc(r.get('PK'))))
                seen.setdefault(t, nfc(r.get('PK')))
        return d
    dups_avant = doublons(head)
    dups = doublons(sim) - dups_avant
    print('DOUBLONS titres pré-existants (recensés, hors DoD) : %d %s'
          % (len(dups_avant), sorted(dups_avant)[:6]))

    print('INVARIANT bandeau=titre (18 têtes × 8 langues) : %d écart(s) résiduel(s) %s'
          % (len(inv_ecarts), inv_ecarts[:6]))
    print('UNICITÉ titres (8 langues, cartes) : %d doublon(s) NOUVEAU(X) %s' % (len(dups), sorted(dups)[:4]))
    ok4 = not inv_ecarts and not dups

    # 598/2 : après écriture, plus aucune carte ne porte un titre d'une autre
    p2 = nfc(S['2'].get('text_fr'))
    p598 = nfc(S['598'].get('text_fr'))
    print('PK 2 -> %r ; PK 598 -> %r ; distincts : %s' % (p2, p598, p2 != p598))

    json.dump(dict(plan=plan, vides_toleres=vides_toleres,
                   comptes=dict(titres=n_titres, bandeaux=n_bandeaux,
                                bandeaux_imprimes=n_bandeaux_impr)),
              io.open(os.path.join(OUT, 'g1w-plan.json'), 'w', encoding='utf-8'),
              ensure_ascii=False, indent=1)
    print('-> g1w-plan.json (%d mutations)' % len(plan))
    if not (ok3 and ok4):
        print('FAIL: comptes ok=%s, contrôles ok=%s' % (ok3, ok4))
        sys.exit(1)
    print('PASS')


if __name__ == '__main__':
    main()
