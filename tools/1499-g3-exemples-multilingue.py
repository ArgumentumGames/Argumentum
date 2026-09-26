# -*- coding: utf-8 -*-
"""#1499 grain ③ — surface MULTILINGUE des `example_*` (Fallacies, deck imprimé).

GRAIN (epic #1499 §5, ligne ③) : « Fallacies ×7 langues — 25 PK distincts,
détecteur B multilingue », statut 🟢 READY après ②. ② a été absorbé par G2
(`docs/corpus/fallacies-exemples-diff.py`, FIELD = 'example_fr' SEUL) et G3 par
`fallacies-desc-diff.py` (`desc_fr` seul). Aucun dossier ne couvre les 7 langues :
c'est ce que cet instrument mesure.

PÉRIMÈTRE : les 175 cartes du deck imprimé (`carte` non vide), 8 langues.
⛔ ZÉRO ÉCRITURE — le livrable est un dossier, jamais un patch.

DÉTECTEURS — repris de l'epic §2, appliqués langue par langue
  A (croissance)     : len(HEAD) > len(référence) + 20   [exige une référence]
  B (signature)      : tiret cadratin/demi-cadratin suivi d'un segment
  C (auto-citation)  : l'exemple contient le titre de sa PROPRE carte

⚠️ LA PRÉMISSE STRUCTURELLE, mesurée AVANT tout compte (§A)
  La baseline canonique `62b561e75` (22/04/2024) porte **74 colonnes** : elle a
  `example_{fr,en,en_bis,ru,pt}` et **n'a PAS** `example_{es,ar,fa,zh}` — ces
  colonnes n'existent pas à cette date, elles naissent avec la vague i18n.
  ⇒ Pour ces 4 langues :
     - le détecteur A est **inapplicable** (aucune référence) ;
     - le contrôle inverse « la signature était-elle déjà là ? » **n'existe pas** ;
     - un compte « N cellules porteuses » y est donc un NIVEAU, jamais un DELTA.
  Un instrument qui sommerait les 8 langues rendrait un chiffre qui se lit comme
  une surface de régression alors qu'il mélange 3 deltas et 4 niveaux.
  ⭐ Formulation : *une colonne entièrement nouvelle n'est pas une réécriture.*

RÉFÉRENCE DE REPLI pour les 4 langues neuves (§C) : la cellule `example_fr` de
la MÊME carte — la vague i18n est une traduction. Une signature présente en
`zh` et absente en `fr` n'a PAS été propagée depuis la source : elle est née
dans la traduction. C'est mesurable, et ça ne remplace pas un contrôle inverse.

TÉMOIN OBLIGATOIRE (§E) : la mutation doit pouvoir rendre un `1` — injecter la
signature dans une copie littérale d'une cellule la fait voir, la retirer la
fait disparaître. Un détecteur jamais vu rouge est un no-op.
"""
import csv
import io
import os
import re
import subprocess
import sys
import unicodedata

BASE = os.path.abspath(os.path.join(os.path.dirname(os.path.abspath(__file__)), '..'))
CSV_PATH = 'Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv'
BASELINE_REF = '62b561e75'
LANGS = ['fr', 'en', 'ru', 'pt', 'es', 'ar', 'fa', 'zh']
NON_FR = ['en', 'ru', 'pt', 'es', 'ar', 'fa', 'zh']
GROWTH_DELTA = 20
MIN_TITLE_SKEL = 6

DASH = re.compile(r'[—–]\s*\S')          # — ou – suivi d'un segment
DASH_SEG = re.compile(r'[—–]\s*(\S.*)$', re.S)


# ---------------------------------------------------------------- utilitaires
def git_show(commit, path):
    p = subprocess.run(['git', '-C', BASE, 'show', '%s:%s' % (commit, path)],
                       capture_output=True)
    if p.returncode:
        sys.exit('illisible: %s@%s' % (path, commit))
    return p.stdout.decode('utf-8-sig', 'replace')


def rows(content):
    return list(csv.DictReader(io.StringIO(content)))


def header(content):
    return next(csv.reader(io.StringIO(content)))


def nfc(s):
    return unicodedata.normalize('NFC', s or '')


def norm_ws(s):
    s = nfc(s).replace(' ', ' ').replace(' ', ' ')
    s = s.replace('’', "'").replace('‘', "'")
    s = s.replace('“', '"').replace('”', '"')
    s = s.replace('«', '"').replace('»', '"')
    s = s.replace('–', '-').replace('—', '-')
    return re.sub(r'\s+', ' ', s).strip()


def deaccent(s):
    return ''.join(c for c in unicodedata.normalize('NFD', s)
                   if unicodedata.category(c) != 'Mn')


def skeleton(s):
    return re.sub(r'[^a-z0-9 ]', '', deaccent(norm_ws(s)).lower()).strip()


def card_non_vide(r):
    return bool(nfc(r.get('carte', '')).strip())


def pk_of(r):
    return nfc(r.get('PK', '')).strip()


# ------------------------------------------------------------------ détecteurs
def sig_b(text):
    """Signature B — le tiret introduit-il un segment ? (le motif de l'epic §2)

    ⚠️ Sur le texte NFC BRUT, jamais sur `norm_ws` : celui-ci ramène — et – à
    `-` (il normalise pour COMPARER), ce qui rendrait le détecteur muet sur le
    motif même qu'il cherche. Attrapé par le self-test, cas 1 et 2.
    """
    return bool(DASH.search(nfc(text)))


def sig_b_segment(text):
    """Le segment explicatif, pour lecture humaine (jamais compté seul)."""
    m = DASH_SEG.search(nfc(text))
    return m.group(1).strip() if m else ''


def sig_c(example, title):
    """Auto-citation — l'exemple contient le titre de sa propre carte."""
    t = skeleton(title)
    if len(t) < MIN_TITLE_SKEL:
        return False
    return t in skeleton(example)


def sig_a(now, ref):
    """Croissance — exige une référence. Rend None si la référence est absente."""
    if ref is None:
        return None
    return len(norm_ws(now)) > len(norm_ws(ref)) + GROWTH_DELTA


def classify(ref, now):
    """Mêmes classes que le ledger #1503 / G2 — la comparaison reste homogène."""
    r, n = norm_ws(ref), norm_ws(now)
    if r == n:
        return 'IDENT'
    if not r:
        return 'FILL'
    if not n:
        return 'VIDE'
    if skeleton(r) == skeleton(n):
        return 'C'
    if skeleton(n) and skeleton(n) in skeleton(r):
        return 'TRONQ'
    if skeleton(r) in skeleton(n):
        return 'AJOUT'
    return 'SUBST'


# --------------------------------------------------------------------- rapport
def report(ref='HEAD'):
    head_raw = git_show(ref, CSV_PATH)
    base_raw = git_show(BASELINE_REF, CSV_PATH)
    head, base = rows(head_raw), rows(base_raw)
    h_cols, b_cols = set(header(head_raw)), set(header(base_raw))

    print('référence mesurée : %s' % ref)
    print('lignes %s=%d  baseline=%d' % (ref, len(head), len(base)))
    deck = [r for r in head if card_non_vide(r)]
    print('deck imprimé (carte non vide) : %d' % len(deck))

    # ---------------------------------------------------------------- §A
    print('\n=== §A — la prémisse : la colonne existe-t-elle à la baseline ?')
    print('%-12s %-10s %-10s %s' % ('colonne', 'baseline', 'HEAD', 'verdict'))
    ref_exists = {}
    for L in LANGS:
        col = 'example_%s' % L
        in_b, in_h = col in b_cols, col in h_cols
        ref_exists[L] = in_b
        verdict = ('référence DISPONIBLE' if in_b
                   else '⛔ AUCUNE référence — colonne née après la baseline')
        print('%-12s %-10s %-10s %s' % (col, 'oui' if in_b else 'non',
                                        'oui' if in_h else 'non', verdict))
    print('⛔ barrière de périmètre : %d langues avec delta (%s) · %d langues en niveau seul (%s)'
          % (sum(1 for L in LANGS if ref_exists[L]),
             ','.join(L for L in LANGS if ref_exists[L]),
             sum(1 for L in LANGS if not ref_exists[L]),
             ','.join(L for L in LANGS if not ref_exists[L])))

    base_by_pk = {}
    for r in base:
        pk = pk_of(r)
        if pk and card_non_vide(r):
            base_by_pk[pk] = r

    # ---------------------------------------------------------------- §B
    print('\n=== §B — surfaces sur le deck (175), par langue')
    print('%-5s %8s %8s %8s %8s %8s' % ('lang', 'non-vide', 'sign.B', 'auto-C',
                                         'croiss.A', 'delta-B'))
    per_lang = {}
    for L in LANGS:
        col, tcol = 'example_%s' % L, 'text_%s' % L
        d = {'nonempty': 0, 'B': 0, 'C': 0, 'A': 0, 'A_na': 0, 'B_new': 0,
             'B_pre': 0, 'cells': {}}
        for r in deck:
            pk = pk_of(r)
            now = nfc(r.get(col, '')).strip()
            ref = None
            if ref_exists[L]:
                rb = base_by_pk.get(pk)
                ref = nfc(rb.get(col, '')).strip() if rb is not None else None
            d['cells'][pk] = (now, ref)
            if not now:
                continue
            d['nonempty'] += 1
            if sig_b(now):
                d['B'] += 1
                if ref is None:
                    d['A_na'] += 1
                elif sig_b(ref):
                    d['B_pre'] += 1
                else:
                    d['B_new'] += 1
            if sig_c(now, nfc(r.get(tcol, ''))):
                d['C'] += 1
            a = sig_a(now, ref)
            if a is None:
                pass
            elif a:
                d['A'] += 1
        per_lang[L] = d
        print('%-5s %8d %8d %8d %8s %8s'
              % (L, d['nonempty'], d['B'], d['C'],
                 d['A'] if ref_exists[L] else 'n/a',
                 ('+%d' % d['B_new']) if ref_exists[L] else 'n/a'))

    # ---------------------------------------------------------------- §C
    print('\n=== §C — propagation : PK portant la signature B dans ≥1 langue non-fr')
    fr_sig = {pk: sig_b(now) for pk, (now, _) in per_lang['fr']['cells'].items()}
    union, by_cnt = set(), {}
    for pk in fr_sig:
        langs_fr_sig = [L for L in NON_FR
                        if per_lang[L]['cells'].get(pk, ('', None))[0]
                        and sig_b(per_lang[L]['cells'][pk][0])]
        if not langs_fr_sig:
            continue
        union.add(pk)
        by_cnt[pk] = langs_fr_sig
    print('PK distincts (non-fr, ≥1 langue) : %d' % len(union))
    src_sig = sum(1 for pk in union if fr_sig.get(pk))
    print('  dont la source fr porte AUSSI la signature : %d  (propagée)'
          % src_sig)
    print('  dont la source fr NE la porte PAS          : %d  (née en traduction)'
          % (len(union) - src_sig))
    print('\n  %-6s %-4s %s' % ('PK', 'fr', 'langues non-fr porteuses'))
    for pk in sorted(union, key=lambda x: (-len(by_cnt[x]), x)):
        print('  %-6s %-4s %s' % (pk, 'oui' if fr_sig.get(pk) else 'non',
                                  ','.join(by_cnt[pk])))
    if not union:
        print('  (aucune)')

    # auto-citation hors fr
    c_union = {}
    for L in NON_FR:
        for pk, (now, _) in per_lang[L]['cells'].items():
            if now and sig_c(now, nfc(next(r for r in deck if pk_of(r) == pk)
                                      .get('text_%s' % L, ''))):
                c_union.setdefault(pk, []).append(L)
    print('\n=== §C bis — auto-citation (C) hors fr : %d PK' % len(c_union))
    for pk in sorted(c_union):
        print('  %-6s fr=%s  %s' % (pk, 'oui' if sig_c(
            per_lang['fr']['cells'][pk][0],
            nfc(next(r for r in deck if pk_of(r) == pk).get('text_fr', ''))) else 'non',
            ','.join(c_union[pk])))
    if not c_union:
        print('  (aucune)')

    # ---------------------------------------------------------------- §D
    # La question qui décide de la LECTURE : la signature non-fr est-elle
    # (1) vivante des deux côtés, (2) un RÉSIDU du retrait fr, ou
    # (3) née dans la traduction ? Sans cette colonne, les trois se lisent pareil.
    print('\n=== §D — diagnostic par PK : d\'où vient la signature non-fr ?')
    base_by_pk_nfc = {pk_of(r): r for r in base if pk_of(r) and card_non_vide(r)}
    kind = {}
    print('  %-6s %-4s %-4s %s' % ('PK', 'frB', 'frH', 'non-fr porteuses (-> = présente aussi à la baseline)'))
    for pk in sorted(union, key=lambda x: (-len(by_cnt[x]), x)):
        rb = base_by_pk_nfc.get(pk)
        fr_base = sig_b(rb.get('example_fr', '')) if rb is not None else False
        fr_now = fr_sig.get(pk, False)
        parts = []
        for L in by_cnt[pk]:
            col = 'example_%s' % L
            was = (sig_b(rb.get(col, '')) if (rb is not None and col in b_cols)
                   else None)
            # « lang -> » = la colonne portait DÉJÀ la signature à la baseline
            parts.append('%s%s' % (L, '' if was is None else ('->' if was else '')))
        kind[pk] = ('residu' if (fr_base and not fr_now) else
                    ('vivante' if fr_now else 'nee-en-traduction'))
        print('  %-6s %-4s %-4s %s' % (pk, 'oui' if fr_base else 'non',
                                       'oui' if fr_now else 'non',
                                       ' '.join(parts)))
    from collections import Counter
    cnt = Counter(kind.values())
    print('\n  ventilation : vivante=%d · residu-du-retrait-fr=%d · nee-en-traduction=%d'
          % (cnt['vivante'], cnt['residu'], cnt['nee-en-traduction']))
    print('  ⚠️ « residu-du-retrait-fr » = la glose fr a été retirée (#1546) et la')
    print('     traduction porte encore la sienne : ce n\'est PAS une traduction fautive.')

    # ---------------------------------------------------------------- §F
    # LE RELEVÉ, imprimé pour LECTURE. Un tiret suivi d'un segment n'est pas un
    # défaut en soi (dialogue, incise, ponctuation) — le détecteur B est un
    # PRÉ-FILTRE. La classification éditoriale se fait sur ces lignes, jamais
    # sur le compte.
    print('\n=== §F — relevé des segments (lecture humaine, ⛔ non comptés)')
    by_pk = {pk_of(r): r for r in deck}
    for pk in sorted(union, key=lambda x: (-len(by_cnt[x]), x)):
        r = by_pk[pk]
        print('  --- PK %s  « %s »' % (pk, nfc(r.get('text_fr', '')).strip()[:60]))
        for L in ['fr'] + by_cnt[pk]:
            col = 'example_%s' % L
            txt = nfc(r.get(col, '')).strip()
            if not txt or not sig_b(txt):
                continue
            seg = sig_b_segment(txt)
            print('      %-3s %s' % (L, seg[:150].replace('\n', ' ')))

    return per_lang, union, by_cnt, ref_exists, kind


# ------------------------------------------------------------------ self-test
def self_test():
    cases = [
        ('B tiret cadratin', sig_b('Un exemple — suivi de son explication'), True),
        ('B tiret demi', sig_b('Un exemple – suivi'), True),
        ('B sans segment', sig_b('Un exemple —'), False),
        ('B aucune', sig_b('Un exemple ordinaire.'), False),
        ('B vide', sig_b(''), False),
        ('C titre present', sig_c('Ceci est un biais de negativite classique',
                                  'Biais de négativité'), True),
        ('C titre absent', sig_c('Une phrase quelconque ici',
                                 'Biais de négativité'), False),
        ('C titre trop court', sig_c('abcd', 'Abc'), False),
        ('A croit', sig_a('x' * 100, 'x' * 10), True),
        ('A ne croit pas', sig_a('x' * 25, 'x' * 10), False),
        ('A sans reference', sig_a('x' * 100, None), None),
        ('classe IDENT', classify('abc', 'abc'), 'IDENT'),
        ('classe FILL', classify('', 'abc'), 'FILL'),
        ('classe VIDE', classify('abc', ''), 'VIDE'),
        ('classe TRONQ', classify('abcdef', 'abc'), 'TRONQ'),
        ('classe AJOUT', classify('abc', 'abcdef'), 'AJOUT'),
        ('classe SUBST', classify('abcdef', 'xyz'), 'SUBST'),
    ]
    bad = 0
    for name, got, want in cases:
        if got != want:
            bad += 1
            print('  ✗ %s : attendu %r, obtenu %r' % (name, want, got))
    print('SELF-TEST %s (%d cas)' % ('OK' if bad == 0 else 'ROUGE %d' % bad,
                                     len(cases)))
    return 0 if bad == 0 else 2


# -------------------------------------------------------------- mutation (E)
def mutation_test():
    """La mutation doit pouvoir rendre un 1 : on mute un LITTÉRAL, jamais l'arbre."""
    print('=== §E — mutation du détecteur (littéraux, aucun fichier touché)')
    ok = True
    clean = 'Une phrase ordinaire sans explication greffée.'
    dirty = clean + ' — Biais de négativité : un élément négatif pèse plus lourd.'
    v1 = sig_b(clean)
    v2 = sig_b(dirty)
    print('  cellule propre  -> B = %s' % v1)
    print('  signature injectée -> B = %s' % v2)
    if v1 or not v2:
        ok = False
        print('  ✗ la mutation ne discrimine pas')
    t = 'Biais de négativité'
    print('  auto-citation avant = %s' % sig_c(clean, t))
    print('  auto-citation après = %s' % sig_c(dirty, t))
    if sig_c(clean, t) or not sig_c(dirty, t):
        ok = False
        print('  ✗ l\'auto-citation ne discrimine pas')
    print('  MUTATION %s' % ('OK' if ok else 'ROUGE'))
    return 0 if ok else 2


if __name__ == '__main__':
    if '--self-test' in sys.argv:
        sys.exit(self_test())
    if '--mutation-test' in sys.argv:
        sys.exit(mutation_test())
    ref = 'HEAD'
    if '--ref' in sys.argv:
        ref = sys.argv[sys.argv.index('--ref') + 1]
    report(ref)
