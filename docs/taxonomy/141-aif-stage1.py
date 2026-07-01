#!/usr/bin/env python3
"""#141 AIF Stage-1 candidate generator (gpt-5.5 assist, CLOSED-SET selection).

DRY-RUN PILOT. Writes ONLY to docs/taxonomy/ (sidecar). 0 write under Cards/.
Anti-Fab design: model PICKS from real nodes + real AIF tokens (no open generation).

Run:  python docs/taxonomy/141-aif-stage1.py [N]
  N = number of sample nodes to process (default all in tmp/141-aif-sample.json).
"""
import csv, json, os, sys, time, re, urllib.request, urllib.error

CSV_PATH = r"Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
SAMPLE_JSON = r"tmp/141-aif-sample.json"
KEY_FILE = r"C:/Users/jsboi/AppData/Local/Temp/claude/c--dev-Argumentum/1607d26f-99dc-4fab-b48a-6f18db62c89b/scratchpad/openai_key.txt"
OUT_CSV = r"docs/taxonomy/141-aif-candidates-sample.csv"
OUT_JSON = r"tmp/141-aif-candidates-sample.json"
API = "https://api.openai.com/v1/responses"
MODEL = "gpt-5.5"
DELAY = 0.4

CROSSLINK = ["PredatesOn","Denounces","Leverages","Allows","Opposes","Inverts","Mirrors","IsRelatedTo"]
VERB_DEFS = {
 "PredatesOn": "A historically or conceptually precedes B (A is a precursor/forerunner of B).",
 "Denounces":  "A publicly condemns or calls out B as a misuse.",
 "Leverages":  "A exploits or relies on B as a mechanism.",
 "Allows":     "A permits or creates an opening that B exploits.",
 "Opposes":    "A is the logical contrary or functional antagonist of B.",
 "Inverts":    "A is the directional reversal / mirror-image of B (same form, flipped polarity).",
 "Mirrors":    "A structurally parallels B (analogous form) without being identical.",
 "IsRelatedTo":"A has a genuine but weaker/generic relation to B (catch-all; prefer a specific verb).",
}
MAP_TYPES = ["skos:broadMatch","skos:closeMatch","skos:narrowMatch"]

def parent_of(dp):
    dp=(dp or "").strip()
    if "," not in dp: return ""
    fam, rest = dp.split(",",1)
    if len(rest)<=1: return fam
    return fam+","+rest[:-1]

def comma2dot(c):
    c=(c or "").strip()
    if "," not in c: return c
    fam,rest=c.split(",",1)
    return ".".join([fam]+list(rest))

def load_taxonomy():
    with open(CSV_PATH, encoding="utf-8-sig", newline="") as f:
        rows=list(csv.DictReader(f))
    bycomma={}; bydot={}
    for r in rows:
        c=(r.get("decimal_path") or "").strip()
        if not c: continue
        bycomma[c]=r; bydot[comma2dot(c)]=r
    return rows, bycomma, bydot

def aif_vocab(rows):
    toks=set()
    for r in rows:
        for col in ["AIF_skosDirectRef","AIF_skosExceptionRef","AIF_skosOther"]:
            v=(r.get(col) or "").strip()
            for t in v.split(","):
                t=t.strip()
                if t: toks.add(t)
    return sorted(toks)

def build_pool(node, rows, bycomma):
    """Candidate targets: siblings + parent + children + 7 family roots (all REAL nodes)."""
    dp=(node.get("decimal_path") or "").strip()
    fam = dp.split(",",1)[0] if "," in dp else dp
    pool=[]; seen=set()
    def add(c):
        c=c.strip()
        if not c or c in seen: return
        r=bycomma.get(c)
        if not r: return
        seen.add(c)
        pool.append({"dotted":comma2dot(c),"label":(r.get("text_fr") or r.get("text_en") or "").strip(),
                     "desc":(r.get("desc_fr") or r.get("desc_en") or "").strip()[:140]})
    # siblings (same parent)
    for s in rows:
        sdp=(s.get("decimal_path") or "").strip()
        if parent_of(sdp)==parent_of(dp) and sdp!=dp: add(sdp)
    # parent + children
    par=parent_of(dp)
    if par: add(par)
    for s in rows:
        sdp=(s.get("decimal_path") or "").strip()
        if parent_of(sdp)==dp: add(sdp)
    # 7 family roots as cross-family landmarks
    for s in rows:
        sdp=(s.get("decimal_path") or "").strip()
        if (s.get("depth") or "").strip()=="1": add(sdp)
    return pool[:30]

def call_gpt(prompt, key):
    body=json.dumps({
        "model":MODEL,"input":prompt,"reasoning":{"effort":"low"},
        "max_output_tokens":4000,
        "text":{"format":{"type":"json_object"}},
    }).encode("utf-8")
    req=urllib.request.Request(API, data=body, headers={
        "Authorization":"Bearer "+key,"Content-Type":"application/json"})
    with urllib.request.urlopen(req, timeout=120) as resp:
        data=json.load(resp)
    # extract message text
    for blk in data.get("output",[]):
        if blk.get("type")=="message":
            for c in blk.get("content",[]):
                if c.get("type")=="output_text":
                    return c.get("text",""), data.get("model","")
    return "", data.get("model","")

def parse_json(text):
    if not text: return None
    m=re.search(r"\{.*\}", text, re.S)
    if not m: return None
    try: return json.loads(m.group(0))
    except Exception: return None

def main():
    n = int(sys.argv[1]) if len(sys.argv)>1 else 999
    key=open(KEY_FILE).read().strip()
    rows, bycomma, bydot = load_taxonomy()
    vocab = aif_vocab(rows)
    print(f"AIF vocab: {len(vocab)} tokens | nodes: {len(rows)}")
    sample=json.load(open(SAMPLE_JSON, encoding="utf-8"))[:n]
    print(f"Processing {len(sample)} sample nodes...")
    results=[]
    for i, node in enumerate(sample):
        dp=node["decimal_path"]; lbl=node.get("label_fr") or node.get("label_en")
        pool=build_pool(node, rows, bycomma)
        pool_lines="\n".join(f"- {p['dotted']} | {p['label']} | {p['desc']}" for p in pool)
        vocab_str=", ".join(vocab)
        prompt=(f"You are an argumentation-theory expert mapping fallacies to the Walton AIF scheme and to each other.\n\n"
                f"SOURCE NODE:\n- decimal_path(dotted): {comma2dot(dp)}\n- label: {lbl}\n"
                f"- desc FR: {node.get('desc_fr','')}\n- desc EN: {node.get('desc_en','')}\n"
                f"- example FR: {node.get('example_fr','')[:200]}\n\n"
                f"CANDIDATE TARGET NODES (pick targets ONLY from this list, use the dotted path verbatim):\n{pool_lines}\n\n"
                f"RELATIONSHIP VERBS (choose from these 8):\n"
                + "\n".join(f"- {k}: {v}" for k,v in VERB_DEFS.items()) + "\n\n"
                f"WALTON AIF SCHEME TOKENS (choose DirectRef/ExceptionRef ONLY from this closed list):\n{vocab_str}\n\n"
                f"TASK: Propose 0-4 cross-links to OTHER nodes (not the source) from the candidate list, "
                f"and map the source to 0-2 Walton AIF schemes. Be CONSERVATIVE and PRECISE: only propose a "
                f"link if the rationale is defensible. Confidence in [0,1].\n"
                f"Return STRICT JSON: "
                f'{{"crossLinks":[{{"verb":"Opposes","target":"1.2.3","confidence":0.8,"rationale":"one line"}}],'
                f'"aif":{{"directRef":["Ignorance_Inference"],"exceptionRef":["PopularOpinion_Inference"],"mappingType":"skos:broadMatch"}}}}. '
                f"Empty arrays if nothing fits.")
        try:
            text, model = call_gpt(prompt, key)
            obj = parse_json(text)
            if obj is None:
                print(f"  [{i+1}/{len(sample)}] {dp} {lbl[:30]:30s} -> PARSE FAIL"); continue
            # validate (Anti-Fab)
            warns=[]
            cl=obj.get("crossLinks",[])
            for c in cl:
                t=c.get("target","")
                if t not in bydot: warns.append(f"bad_target:{t}")
                if c.get("verb") not in CROSSLINK: warns.append(f"bad_verb:{c.get('verb')}")
            a=obj.get("aif",{})
            for k in ("directRef","exceptionRef"):
                for t in a.get(k,[]):
                    if t not in vocab: warns.append(f"fab_aif_{k}:{t}")
            if a.get("mappingType") and a.get("mappingType") not in MAP_TYPES: warns.append(f"bad_map:{a.get('mappingType')}")
            results.append({"source_dp":dp,"source_dotted":comma2dot(dp),"source_label":lbl,
                            "model":model,"data":obj,"warns":warns})
            ncl=len(cl); naif=len(a.get("directRef",[]))+len(a.get("exceptionRef",[]))
            w=(" WARN:"+",".join(set(warns))) if warns else ""
            print(f"  [{i+1}/{len(sample)}] {dp} {lbl[:30]:30s} -> {ncl} clink, {naif} aif{w}")
        except Exception as e:
            print(f"  [{i+1}/{len(sample)}] {dp} {lbl[:30]:30s} -> ERROR {type(e).__name__}: {str(e)[:80]}")
        time.sleep(DELAY)
    # write sidecar JSON
    json.dump(results, open(OUT_JSON,"w",encoding="utf-8"), ensure_ascii=False, indent=1)
    # write sidecar CSV (flat)
    with open(OUT_CSV,"w",encoding="utf-8",newline="") as f:
        w=csv.writer(f); w.writerow(["source_dp","source_label","kind","verb_or_field","target","target_label","confidence","rationale","warns"])
        for r in results:
            dp=r["source_dp"]; lbl=r["source_label"]; ws=";".join(set(r["warns"]))
            for c in r["data"].get("crossLinks",[]):
                tgt=c.get("target",""); tl=(bydot.get(tgt,{}).get("text_fr") or "") if tgt in bydot else ""
                w.writerow([dp,lbl,"crossLink",c.get("verb"),tgt,tl,c.get("confidence"),c.get("rationale"),ws])
            a=r["data"].get("aif",{})
            for t in a.get("directRef",[]): w.writerow([dp,lbl,"AIF_skosDirectRef","",t,"","",a.get("mappingType",""),ws])
            for t in a.get("exceptionRef",[]): w.writerow([dp,lbl,"AIF_skosExceptionRef","",t,"","",a.get("mappingType",""),ws])
    # summary
    tot_cl=sum(len(r["data"].get("crossLinks",[])) for r in results)
    tot_aif=sum(len(r["data"].get("aif",{}).get("directRef",[]))+len(r["data"].get("aif",{}).get("exceptionRef",[])) for r in results)
    nwarn=sum(1 for r in results if r["warns"])
    print(f"\n=== SUMMARY: {len(results)} nodes | {tot_cl} crossLinks | {tot_aif} AIF refs | {nwarn} nodes w/ warns ===")
    print(f"Sidecar: {OUT_CSV}")

if __name__=="__main__":
    main()
