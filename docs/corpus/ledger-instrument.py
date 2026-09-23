# -*- coding: utf-8 -*-
"""Ledger des changements sur cartes EXISTANTES, contre la reference ARCHIVE (dans le depot).

Reference a deux etages :
  - 2022/...edition fevrier 2022.csv  -> l'objet IMPRIME  (autorite maximale)
  - v3/...Cards.csv                   -> le deck pre-agentique (169 cartes)
Cle de jointure : `path` (mesure : 153/169 vs 29/169 pour PK).
"""
import csv, io, json, os, re, sys, unicodedata

BASE = r'D:\Argumentum'
OUT = os.path.dirname(os.path.abspath(__file__))

def load(rel):
    p = os.path.join(BASE, rel)
    return list(csv.DictReader(io.StringIO(io.open(p, encoding='utf-8-sig', errors='replace').read())))

cur = load(r'Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv')
v3  = load(r'Cards\Fallacies\Archive\v3\Argumentum Fallacies - Cards.csv')
e22 = load(r'Cards\Fallacies\Archive\2022\Argumentum Fallacies - Cards - edition fevrier 2022.csv')

def cards(rows):
    return {(r.get('path') or '').strip(): r for r in rows if (r.get('carte') or '').strip()}

CUR, V3, E22 = cards(cur), cards(v3), cards(e22)

# ---------- normalisation ----------
def nfc(s):
    return unicodedata.normalize('NFC', s or '')

def norm_ws(s):
    s = nfc(s).replace(u'\u00a0', ' ').replace(u'\u202f', ' ')
    s = s.replace(u'\u2019', "'").replace(u'\u2018', "'")
    s = s.replace(u'\u201c', '"').replace(u'\u201d', '"').replace(u'\u00ab', '"').replace(u'\u00ab', '"')
    s = s.replace(u'\u00bb', '"').replace(u'\u2013', '-').replace(u'\u2014', '-')
    return re.sub(r'\s+', ' ', s).strip()

def deaccent(s):
    return u''.join(c for c in unicodedata.normalize('NFD', s) if unicodedata.category(c) != 'Mn')

def skeleton(s):
    """forme insensible aux accents, a la casse et a la ponctuation"""
    return re.sub(r'[^a-z0-9 ]', '', deaccent(norm_ws(s)).lower()).strip()

# ---------- classification MECANIQUE (propose, ne tranche pas) ----------
def classify(ref, now):
    r, n = norm_ws(ref), norm_ws(now)
    if r == n:
        return 'IDENT', ''
    if not r:
        return 'FILL', 'cellule vide a la reference -> remplissage, pas une reecriture'
    if not n:
        return 'VIDE', 'cellule videe'
    if skeleton(r) == skeleton(n):
        return 'C', 'differe seulement par accents/casse/ponctuation -> correction typographique'
    # suppression d'une attribution de citation
    if skeleton(n) and skeleton(n) in skeleton(r):
        return 'TRONQ', 'le texte actuel est contenu dans la reference -> retrait'
    if skeleton(r) in skeleton(n):
        return 'AJOUT', 'la reference est contenue dans le texte actuel -> ajout en place'
    return 'SUBST', 'reformulation ou remplacement'

FIELDS = ['text_fr', 'desc_fr', 'example_fr']

def ref_by_path(p):
    if p in E22:
        return E22[p], '2022'
    if p in V3:
        return V3[p], 'v3'
    return None, None

# ---------- CONTROLE 5 : `path` apparie-t-il des SOEURS renumerotees ? (#1515, 23/09) ----------
# Le controle 4 (`Famille` identique) ne voit pas une renumerotation ENTRE soeurs d'une meme famille :
# 1.1.1/1.1.2/1.1.3 ont permute leurs titres, et `7.2.1` a recu un parent insere (« Evasion ») qui a
# pousse « Fausse piste » en 7.2.1.1 -- toutes deux PRE-agentiques (deja dans la baseline 62b561e75).
# Signal : le titre de la carte d'archive vit, A L'IDENTIQUE, sur une AUTRE carte HEAD.
# ⛔ Le signal inverse (le titre HEAD etait celui d'une autre carte d'archive) n'est PAS une preuve :
# PK 2 porte aujourd'hui le nom imprime de PK 598 (titre reattribue) alors que le CONTENU confirme la
# jointure `path` (0,48 vs 0,00). Seul le sens archive -> HEAD est retenu.
CUR_BY_TITLE = {skeleton(c.get('text_fr')): p for p, c in CUR.items()}
assert len(CUR_BY_TITLE) == len(CUR), 'titres HEAD non uniques : le controle 5 est aveugle'
MISJOINED = {}          # path HEAD mal apparie -> path HEAD ou vit la carte d'archive
for p in CUR:
    ref, _ = ref_by_path(p)
    if ref is None:
        continue
    q = CUR_BY_TITLE.get(skeleton(ref.get('text_fr')))
    if q is not None and q != p:
        MISJOINED[p] = q
REPAIR = {q: p for p, q in MISJOINED.items()}   # carte HEAD q <- carte d'archive du path p
for q, p in REPAIR.items():   # la re-jointure ne doit pas ecraser une jointure deja exacte
    own, _ = ref_by_path(q)
    assert own is None or skeleton(own.get('text_fr')) != skeleton(CUR[q].get('text_fr')), q

def reference(path):
    if path in REPAIR:
        ref, src = ref_by_path(REPAIR[path])
        return ref, src, REPAIR[path]
    if path in MISJOINED:
        return None, None, None          # carte inseree sur un path deja pris : sans ancetre
    ref, src = ref_by_path(path)
    return ref, src, path

rows_out = []
for path, c in sorted(CUR.items(), key=lambda kv: [int(x) for x in kv[0].split('.') if x.isdigit()] or [0]):
    ref_row, ref_src, ref_path = reference(path)
    if ref_row is None:
        continue
    for f in FIELDS:
        cls, why = classify(ref_row.get(f), c.get(f))
        if cls == 'IDENT':
            continue
        rows_out.append(dict(path=path, ref_path=ref_path, pk_cur=c.get('PK'), pk_ref=ref_row.get('PK'),
                             src=ref_src, field=f, cls=cls, why=why,
                             ref=norm_ws(ref_row.get(f)), now=norm_ws(c.get(f)),
                             famille=(c.get('Famille') or '').strip()))

# ---------- CONTROLE INVERSE 1 : la reference est-elle coherente avec elle-meme ? ----------
both = [p for p in E22 if p in V3]
disagree = []
for p in both:
    for f in FIELDS:
        if norm_ws(E22[p].get(f)) != norm_ws(V3[p].get(f)):
            disagree.append((p, f))

# ---------- CONTROLE INVERSE 2 : l'instrument voit-il un cas CONNU ? ----------
probe = [r for r in rows_out if r['path'] == '2.1']
probe_ok = len(probe) == 3 and all(r['cls'] in ('SUBST', 'C', 'AJOUT') for r in probe)

# ---------- CONTROLE INVERSE 3 : rend-il IDENT sur une carte non touchee ? ----------
untouched = sum(1 for p, c in CUR.items()
                if reference(p)[0] is not None
                and all(classify(reference(p)[0].get(f), c.get(f))[0] == 'IDENT' for f in FIELDS))

# ---------- CONTROLE 6 : un SUBST est-il un texte DEPLACE plutot que reecrit ? ----------
# Temoin positif : l'exemple d'archive de 3.2.1 vit verbatim sur 3.2.1.3 (deplacement pre-agentique).
ARCH_CELLS, HEAD_CELLS = {}, {}
for S in (E22, V3):
    for p, r in S.items():
        for f in FIELDS[1:]:
            ARCH_CELLS.setdefault(skeleton(r.get(f)), set()).add(p)
for p, r in CUR.items():
    for f in FIELDS[1:]:
        HEAD_CELLS.setdefault(skeleton(r.get(f)), set()).add(p)
moved = [r for r in rows_out if r['field'] != 'text_fr' and len(skeleton(r['now'])) > 12
         and ((ARCH_CELLS.get(skeleton(r['now']), set()) - {r['ref_path']})
              or (HEAD_CELLS.get(skeleton(r['ref']), set()) - {r['path']}))]

# ---------- rendu ----------
print("=" * 78)
print("PERIMETRE")
print("  cartes imprimees courantes ........ %d" % len(CUR))
print("  appariees (path + controle 5) ..... %d  (2022: %d, v3 seul: %d)"
      % (sum(1 for p in CUR if reference(p)[0] is not None),
         sum(1 for p in CUR if reference(p)[1] == '2022'),
         sum(1 for p in CUR if reference(p)[1] == 'v3')))
# ⛔ « cartes nouvelles » est une SUR-LECTURE : `path` est ce que la restructuration de taxonomie
# deplace, donc une orpheline de `path` peut etre une carte ancienne DEPLACEE. Mesure du 22/09 :
# sur 22 orphelines, 15 sont retrouvees en archive via `archive-bridge-instrument.py`
# (archive --nom--> baseline 2024 --PK--> HEAD). Seules 7 le sont vraiment pas.
print("  NON appariees (path + controle 5) .. %d  (⛔ PAS 'nouvelles' : cf archive-bridge-instrument.py)"
      % sum(1 for p in CUR if reference(p)[0] is None))
print()
print("CONTROLES INVERSES")
print("  [1] 2022 vs v3 sur %d paths communs : %d desaccords de cellule%s"
      % (len(both), len(disagree), '  <- la reference est stable' if not disagree else ''))
print("  [2] temoin PK176/path 2.1 detecte  : %s (%d cellules)" % ('OUI' if probe_ok else 'NON', len(probe)))
print("  [3] cartes rendues INTACTES        : %d  <- l'instrument peut rendre 'rien a signaler'" % untouched)
print("  [5] jointures `path` entre SOEURS  : %d re-appariees par titre %s" % (len(MISJOINED), sorted(MISJOINED.items())))
print("      (⛔ le controle 4 `Famille` ne peut pas les voir : meme famille des deux cotes)")
print("  [6] SUBST desc/exemple = texte DEPLACE verbatim : %d %s" % (len(moved), sorted((r['path'], r['field']) for r in moved)))
print()
print("VENTILATION DES CELLULES CHANGEES (cartes existantes uniquement)")
from collections import Counter
cnt = Counter((r['field'], r['cls']) for r in rows_out)
allcls = ['C', 'AJOUT', 'TRONQ', 'SUBST', 'FILL', 'VIDE']
print("  %-12s %s" % ('champ', ''.join('%8s' % c for c in allcls) + '%9s' % 'TOTAL'))
for f in FIELDS:
    line = ''.join('%8d' % cnt[(f, c)] for c in allcls)
    print("  %-12s %s%9d" % (f, line, sum(cnt[(f, c)] for c in allcls)))
tot = ''.join('%8d' % sum(cnt[(f, c)] for f in FIELDS) for c in allcls)
print("  %-12s %s%9d" % ('TOTAL', tot, len(rows_out)))
print()
print("  C     = accents/casse/ponctuation seuls  -> garder, pas d'arbitrage")
print("  AJOUT = la reference est INCLUSE telle quelle -> le geste que l'owner aurait accepte")
print("  TRONQ = du texte de reference a disparu")
print("  SUBST = reformulation/remplacement -> c'est LA classe a arbitrer")

json.dump(rows_out, io.open(os.path.join(OUT, 'ledger.json'), 'w', encoding='utf-8'),
          ensure_ascii=False, indent=1)
print("\n-> %d lignes ecrites dans ledger.json" % len(rows_out))
