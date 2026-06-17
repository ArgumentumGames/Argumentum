# -*- coding: utf-8 -*-
"""#497 grappe-comparison substrate generator (ai-01 lane).
Produces a per-grappe inventory + empirical crossLink bubble-up (responding grappes)
+ a lexical affinity candidate pass. Gate-safe: writes only to docs/taxonomy/.
"""
import csv, re, io
from collections import Counter, defaultdict

SRC = 'Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv'
rows = list(csv.DictReader(open(SRC, encoding='utf-8-sig', newline='')))

CL = ['crossLink_PredatesOn','crossLink_Denounces','crossLink_Leverages','crossLink_Allows',
      'crossLink_Opposes','crossLink_Inverts','crossLink_Mirrors','crossLink_IsRelatedTo']

bypath = {r['path']: r for r in rows}
def name(r): return (r.get('text_fr') or r.get('nom_vulgarisé') or '').strip()
def fam(r):  return (r.get('Famille') or '').strip()

# ---- ancestor helpers -------------------------------------------------------
def ancestor_at_depth(path, d):
    segs = path.split('.')
    if len(segs) <= d: return path           # node shallower than target depth = itself
    return '.'.join(segs[:d])

# ---- 1) GRAPPE INVENTORY (depth 2 and depth 3 roots) ------------------------
def build_grappes(grappe_depth):
    g = defaultdict(list)
    for r in rows:
        depth = int(r['depth']) if r['depth'].isdigit() else 0
        if depth < grappe_depth:   # root/family above grappe level - skip as members
            continue
        groot = ancestor_at_depth(r['path'], grappe_depth)
        g[groot].append(r)
    out = []
    for groot, members in g.items():
        rootrow = bypath.get(groot)
        if not rootrow:   # grappe root path not itself a node -> skip (shouldn't happen)
            continue
        depths = [int(m['depth']) for m in members if m['depth'].isdigit()]
        ncl = sum(1 for m in members for c in CL if (m.get(c) or '').strip())
        out.append({
            'root': groot, 'pk': rootrow['PK'], 'name': name(rootrow), 'fam': fam(rootrow),
            'size': len(members), 'dmin': min(depths) if depths else 0,
            'dmax': max(depths) if depths else 0, 'ncl': ncl, 'members': members,
        })
    out.sort(key=lambda x: [int(s) for s in x['root'].split('.')])
    return out

g2 = build_grappes(2)
g3 = build_grappes(3)

# ---- 2) CROSSLINK BUBBLE-UP (empirical responding grappes) ------------------
# climb each existing leaf crossLink endpoint to its depth-2 grappe root.
edges2 = Counter()       # (srcG2, tgtG2) -> count
edge_detail = defaultdict(list)
raw_links = []
for r in rows:
    for c in CL:
        v = (r.get(c) or '').strip()
        if not v: continue
        tgt = bypath.get(v)
        if not tgt: continue
        typ = c.replace('crossLink_','')
        raw_links.append((r['path'], name(r), typ, v, name(tgt), fam(r), fam(tgt)))
        sG = ancestor_at_depth(r['path'], 2)
        tG = ancestor_at_depth(v, 2)
        key = (sG, tG)
        edges2[key]+=1
        edge_detail[key].append((r['path'], name(r), typ, v, name(tgt)))

# ---- 3) LEXICAL AFFINITY CANDIDATES (heuristic, UNVALIDATED) ----------------
STOP = set('''de la le les des du un une et en au aux dans par pour sur avec sans sous
ce cette ces qui que quoi dont ou où à a ses son sa leur leurs il elle on nous vous ils
est sont être avoir fait faire plus moins très bien mal tout tous toute toutes autre
comme ne pas ni se sʼ s lʼ l d n c j m t qu y'''.split())
def toks(s):
    s = s.lower()
    s = re.sub(r"[''`]", ' ', s)
    return [w for w in re.findall(r"[a-zàâäéèêëîïôöùûüçœ]{4,}", s) if w not in STOP]

def grappe_vocab(grp):
    c = Counter()
    for m in grp['members']:
        c.update(toks(name(m)))
        c.update(toks((m.get('desc_fr') or '')))
    return c

vocab2 = {grp['root']: grappe_vocab(grp) for grp in g2}
# candidate cross-family pairs by shared distinctive vocabulary (Jaccard on top terms)
def top_terms(c, n=25):
    return set(w for w,_ in c.most_common(n))
cands = []
g2_index = {grp['root']: grp for grp in g2}
roots = [grp['root'] for grp in g2]
for i in range(len(roots)):
    for j in range(i+1, len(roots)):
        a, b = roots[i], roots[j]
        ga, gb = g2_index[a], g2_index[b]
        if ga['fam'] == gb['fam']:   # cross-family only (intra-family already grouped)
            continue
        ta, tb = top_terms(vocab2[a]), top_terms(vocab2[b])
        inter = ta & tb
        if len(inter) >= 3:
            jac = len(inter)/len(ta|tb)
            cands.append((jac, len(inter), a, ga['name'], ga['fam'], b, gb['name'], gb['fam'], sorted(inter)))
cands.sort(reverse=True)

# ---- WRITE OUTPUTS ----------------------------------------------------------
import os
os.makedirs('docs/taxonomy', exist_ok=True)

# CSV inventory (depth-2 grappes)
with open('docs/taxonomy/497-grappe-inventory.csv','w', encoding='utf-8', newline='') as f:
    w = csv.writer(f)
    w.writerow(['grappe_path','grappe_pk','grappe_name','famille','subtree_size','depth_min','depth_max','existing_crosslinks'])
    for grp in g2:
        w.writerow([grp['root'],grp['pk'],grp['name'],grp['fam'],grp['size'],grp['dmin'],grp['dmax'],grp['ncl']])

print('rows', len(rows), '| g2', len(g2), '| g3', len(g3))
print('bubble-up edges (grappe-pairs from leaf crossLinks):', len(edges2))
print('top bubble edges:')
for cnt,k in sorted(((cnt,k) for k,cnt in edges2.items()), reverse=True)[:12]:
    sN = g2_index.get(k[0],{}).get('name','?'); tN = bypath.get(k[1]) and name(bypath[k[1]]) or '?'
    sG = g2_index.get(k[0]); tG = g2_index.get(k[1])
    print(f'  {cnt}x  {k[0]} {sN[:22]!r} [{sG["fam"] if sG else "?"}]  ->  {k[1]} {tN[:22]!r} [{tG["fam"] if tG else "?"}]')
print('lexical affinity candidates (cross-family, >=3 shared top-terms):', len(cands))
for c in cands[:8]:
    print(f'  jac={c[0]:.2f} share={c[1]}  {c[3][:20]!r}[{c[4]}] <-> {c[6][:20]!r}[{c[7]}]  {c[8][:6]}')

# ---- REFINED lexical pass (down-weight generic argumentation scaffolding) ----
STOP2 = STOP | set('''argument arguments debat débat sophisme appel position point parce
afin deux meme même fausse presentez présentez affirmez idee idée raisonnement conclusion
premisse prémisse exemple personne facon façon plutot plutôt alors donc ainsi cela celui
chose choses cas type sorte maniere manière terme termes mot mots phrase elements éléments'''.split())
def toks2(s):
    s=s.lower(); s=re.sub(r"[''`]",' ',s)
    return [w for w in re.findall(r"[a-zàâäéèêëîïôöùûüçœ]{4,}",s) if w not in STOP2]
def gv2(grp):
    c=Counter()
    for m in grp['members']:
        c.update(toks2(name(m))); c.update(toks2((m.get('desc_fr') or '')))
    return c
vv={grp['root']:gv2(grp) for grp in g2}
def tt(c,n=22): return set(w for w,_ in c.most_common(n))
cands2=[]
for i in range(len(roots)):
    for j in range(i+1,len(roots)):
        a,b=roots[i],roots[j]; ga,gb=g2_index[a],g2_index[b]
        if ga['fam']==gb['fam']: continue
        ia,ib=tt(vv[a]),tt(vv[b]); inter=ia&ib
        if len(inter)>=3:
            jac=len(inter)/len(ia|ib)
            cands2.append((round(jac,3),len(inter),a,ga['name'],ga['fam'],b,gb['name'],gb['fam'],sorted(inter)))
cands2.sort(reverse=True)
print('\n=== REFINED lexical candidates:', len(cands2))
for c in cands2[:12]:
    print(f'  jac={c[0]:.2f} n={c[1]}  {c[3][:22]!r}[{c[4][:4]}] <-> {c[6][:22]!r}[{c[7][:4]}]  {c[8][:5]}')
print('\n=== FULL depth-2 grappe inventory (21) ===')
for grp in g2:
    print(f'  {grp["root"]:>4}  {grp["fam"][:14]:<14}  sz={grp["size"]:>3} d{grp["dmin"]}-{grp["dmax"]}  cl={grp["ncl"]}  {grp["name"]}')

# ---- emit responding-grappes CSV (empirical bubble-up, machine-consumable) ---
with open('docs/taxonomy/497-responding-grappes.csv','w', encoding='utf-8', newline='') as f:
    w=csv.writer(f)
    w.writerow(['weight','src_grappe_path','src_grappe_name','src_famille',
                'tgt_grappe_path','tgt_grappe_name','tgt_famille','cross_family','leaf_link_examples'])
    for cnt,k in sorted(((cnt,k) for k,cnt in edges2.items()), reverse=True):
        sg=g2_index.get(k[0]); tg=g2_index.get(k[1])
        sN=sg['name'] if sg else (name(bypath[k[0]]) if k[0] in bypath else '?')
        tN=tg['name'] if tg else (name(bypath[k[1]]) if k[1] in bypath else '?')
        sF=sg['fam'] if sg else '?'; tF=tg['fam'] if tg else '?'
        ex='; '.join(f'{d[1]}-[{d[2]}]->{d[4]}' for d in edge_detail[k][:3])
        w.writerow([cnt,k[0],sN,sF,k[1],tN,tF, 'YES' if sF!=tF else 'no', ex])
print('\nWROTE docs/taxonomy/497-responding-grappes.csv  (', len(edges2),'edges )')
