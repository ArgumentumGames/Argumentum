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

rows_out = []
for path, c in sorted(CUR.items(), key=lambda kv: [int(x) for x in kv[0].split('.') if x.isdigit()] or [0]):
    ref_row, ref_src = None, None
    if path in E22:
        ref_row, ref_src = E22[path], '2022'
    elif path in V3:
        ref_row, ref_src = V3[path], 'v3'
    if ref_row is None:
        continue
    for f in FIELDS:
        cls, why = classify(ref_row.get(f), c.get(f))
        if cls == 'IDENT':
            continue
        rows_out.append(dict(path=path, pk_cur=c.get('PK'), pk_ref=ref_row.get('PK'),
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
                if (p in E22 or p in V3)
                and all(classify((E22.get(p) or V3.get(p)).get(f), c.get(f))[0] == 'IDENT' for f in FIELDS))

# ---------- rendu ----------
print("=" * 78)
print("PERIMETRE")
print("  cartes imprimees courantes ........ %d" % len(CUR))
print("  appariees sur `path` .............. %d  (2022: %d, v3 seul: %d)"
      % (sum(1 for p in CUR if p in E22 or p in V3),
         sum(1 for p in CUR if p in E22),
         sum(1 for p in CUR if p not in E22 and p in V3)))
# ⛔ « cartes nouvelles » est une SUR-LECTURE : `path` est ce que la restructuration de taxonomie
# deplace, donc une orpheline de `path` peut etre une carte ancienne DEPLACEE. Mesure du 22/09 :
# sur 22 orphelines, 15 sont retrouvees en archive via `archive-bridge-instrument.py`
# (archive --nom--> baseline 2024 --PK--> HEAD). Seules 7 le sont vraiment pas.
print("  NON appariees sur `path` .......... %d  (⛔ PAS 'nouvelles' : cf archive-bridge-instrument.py)"
      % sum(1 for p in CUR if p not in E22 and p not in V3))
print()
print("CONTROLES INVERSES")
print("  [1] 2022 vs v3 sur %d paths communs : %d desaccords de cellule%s"
      % (len(both), len(disagree), '  <- la reference est stable' if not disagree else ''))
print("  [2] temoin PK176/path 2.1 detecte  : %s (%d cellules)" % ('OUI' if probe_ok else 'NON', len(probe)))
print("  [3] cartes rendues INTACTES        : %d  <- l'instrument peut rendre 'rien a signaler'" % untouched)
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
