# #2227 — harmonisation typographique des guillemets (en / pt-BR / es) dans les 21 cellules traduites des 7 articles ; reste du fichier à l'octet près.
# en : “ ” et ’ (le texte mêlait droits et courbes) · pt-BR : “ ” (mêlait « » et “ ”) · es : « » (mêlait « » et “ ”, sans imbrication).
# Balises et <code>…</code> intacts. Usage : python tools/2227-guillemets-en-pt-es.py [chemin] [--write] (défaut : registre du dépôt, dry-run).
import csv,io,re,sys
import os
ARGS=[a for a in sys.argv[1:] if not a.startswith("--")]
P=ARGS[0] if ARGS else os.path.join(os.path.dirname(os.path.abspath(__file__)),"..","docs","dnn-localization","eav-content.csv")
WRITE="--write" in sys.argv
raw=open(P,'rb').read(); t=raw.decode('utf-8')
H=list(csv.reader(io.StringIO(t,newline='')))
hdr=H[0]; idx={c:i for i,c in enumerate(hdr)}
KEYS=set()
for r in H[1:]:
    if r[idx['content_type']]=='News' and r[idx['field']] in ('Title','Teaser','Content') and r[idx['en']] and r[idx['entity_guid']][:8] in ('D6A2A0CF','39355C1D','F2D87598','84094BAE','3F913BF3','8EC968E1','2E10F534'):
        KEYS.add(r[0])
assert len(KEYS)==21, len(KEYS)
SEG=re.compile(r'(<code>.*?</code>|<[^>]+>)',re.S)
def map_text(v,fn):
    parts=SEG.split(v)
    return ''.join(p if (i%2==1) else fn(p) for i,p in enumerate(parts))
def fix_en(v):
    st={'open':True}
    def fn(s):
        s=s.replace("'",'’')
        out=[]
        for ch in s:
            if ch=='"':
                out.append('“' if st['open'] else '”'); st['open']=not st['open']
            else: out.append(ch)
        return ''.join(out)
    r=map_text(v,fn); assert st['open'], 'odd straight quotes'
    return r
def fix_pt(v): return map_text(v,lambda s:s.replace('«','“').replace('»','”'))
def fix_es(v): return map_text(v,lambda s:s.replace('“','«').replace('”','»'))
def nested(v,o,c,io_,ic):
    # inner quotes io_/ic appearing inside an outer o..c span?
    tx=SEG.sub('',v); depth=0; bad=0
    for ch in tx:
        if ch==o: depth+=1
        elif ch==c: depth-=1
        elif ch in (io_,ic) and depth>0: bad+=1
    return bad
FIX={'en':fix_en,'pt':fix_pt,'es':fix_es}
out=t; n=0
for i,r in enumerate(H):
    if r[0] not in KEYS: continue
    key=r[0]; nxt=H[i+1][0]
    s=out.index('\n'+key+',')+1; e=out.index('\n'+nxt+',',s)+1
    b=io.StringIO(); csv.writer(b,lineterminator='\n').writerow(r); assert b.getvalue()==out[s:e]
    nr=list(r)
    assert nested(r[idx['pt']],'“','”','«','»')==0 and nested(r[idx['pt']],'«','»','“','”')==0, ('pt nested',key)
    assert nested(r[idx['es']],'«','»','“','”')==0, ('es nested',key)
    for l,f in FIX.items():
        nv=f(r[idx[l]])
        if nv!=r[idx[l]]: n+=1; print(key[:8],r[idx['field']],l,'changed')
        nr[idx[l]]=nv
    b=io.StringIO(); csv.writer(b,lineterminator='\n').writerow(nr)
    out=out[:s]+b.getvalue()+out[e:]
print('cells changed',n)
if WRITE:
    tmp=P+'.tmp'; open(tmp,'wb').write(out.encode('utf-8'))
    os.replace(tmp,P); print('written')
