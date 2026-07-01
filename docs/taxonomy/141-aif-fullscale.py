#!/usr/bin/env python3
"""#141 AIF Stage-1 FULL-SCALE generator (gpt-5.5 assist, CLOSED-SET selection).

Scales the #620 pilot (28 nodes) to ALL Fallacies non-card nodes (1232).
Same anti-fab closed-set design (0 fabrication on pilot). CHECKPOINT/RESUME:
one JSON line per node in CKPT; re-run skips already-processed nodes.

DRY-RUN. Writes checkpoint+interim to tmp/ (not committed); final sidecar to
docs/taxonomy/ only when aggregated. 0 write under Cards/.

Run:
  python docs/taxonomy/141-aif-fullscale.py              # resume, all
  python docs/taxonomy/141-aif-fullscale.py --limit 50   # cap new nodes this run
  python docs/taxonomy/141-aif-fullscale.py --finalize   # aggregate tmp-> sidecar CSV/JSON, no API calls
"""
import csv, json, os, sys, time, re, urllib.request, urllib.error, argparse

CSV_PATH = r"Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
KEY_FILE = r"C:/Users/jsboi/AppData/Local/Temp/claude/c--dev-Argumentum/1607d26f-99dc-4fab-b48a-6f18db62c89b/scratchpad/openai_key.txt"
CKPT = r"tmp/141-aif-fullscale.jsonl"        # checkpoint (1 line/node, not committed)
LOGF = r"tmp/141-aif-fullscale.log"          # progress log (not committed)
OUT_CSV = r"docs/taxonomy/141-aif-candidates-fullscale.csv"
OUT_JSON = r"docs/taxonomy/141-aif-candidates-fullscale.json"
API = "https://api.openai.com/v1/responses"
MODEL = "gpt-5.5"
DELAY = 0.35

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

def log(msg):
    line=f"[{time.strftime('%H:%M:%S')}] {msg}"
    print(line, flush=True)
    try:
        with open(LOGF,"a",encoding="utf-8") as f: f.write(line+"\n")
    except Exception: pass

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

def noncard_nodes(rows):
    """Fallacies non-card nodes: carte not in {1,2}, ordered by decimal_path."""
    nc=[]
    for r in rows:
        c=(r.get("carte") or "").strip()
        dp=(r.get("decimal_path") or "").strip()
        if dp and c not in ("1","2"):
            nc.append(r)
    # sort by (family, depth, rest) for stable, family-grouped ordering
    nc.sort(key=lambda r: (r.get("decimal_path",""),))
    return nc

def build_pool(node, rows, bycomma):
    dp=(node.get("decimal_path") or "").strip()
    pool=[]; seen=set()
    def add(c):
        c=c.strip()
        if not c or c in seen: return
        r=bycomma.get(c)
        if not r: return
        seen.add(c)
        pool.append({"dotted":comma2dot(c),"label":(r.get("text_fr") or r.get("text_en") or "").strip(),
                     "desc":(r.get("desc_fr") or r.get("desc_en") or "").strip()[:140]})
    for s in rows:
        sdp=(s.get("decimal_path") or "").strip()
        if parent_of(sdp)==parent_of(dp) and sdp!=dp: add(sdp)
    par=parent_of(dp)
    if par: add(par)
    for s in rows:
        sdp=(s.get("decimal_path") or "").strip()
        if parent_of(sdp)==dp: add(sdp)
    for s in rows:
        if (s.get("depth") or "").strip()=="1": add((s.get("decimal_path") or "").strip())
    return pool[:30]

def call_gpt(prompt, key):
    body=json.dumps({"model":MODEL,"input":prompt,"reasoning":{"effort":"low"},
                     "max_output_tokens":4000,"text":{"format":{"type":"json_object"}}}).encode("utf-8")
    req=urllib.request.Request(API, data=body, headers={
        "Authorization":"Bearer "+key,"Content-Type":"application/json"})
    with urllib.request.urlopen(req, timeout=120) as resp:
        data=json.load(resp)
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

def validate(obj, bydot, vocab):
    warns=[]
    for c in obj.get("crossLinks",[]):
        t=c.get("target","")
        if t not in bydot: warns.append(f"bad_target:{t}")
        if c.get("verb") not in CROSSLINK: warns.append(f"bad_verb:{c.get('verb')}")
    a=obj.get("aif",{})
    for k in ("directRef","exceptionRef"):
        for t in a.get(k,[]):
            if t not in vocab: warns.append(f"fab_aif_{k}:{t}")
    if a.get("mappingType") and a.get("mappingType") not in MAP_TYPES: warns.append(f"bad_map:{a.get('mappingType')}")
    return warns

def load_ckpt():
    done=set()
    if os.path.exists(CKPT):
        with open(CKPT, encoding="utf-8") as f:
            for line in f:
                line=line.strip()
                if not line: continue
                try: done.add(json.loads(line)["source_dp"])
                except Exception: pass
    return done

def run(limit):
    key=open(KEY_FILE).read().strip()
    rows, bycomma, bydot = load_taxonomy()
    vocab = aif_vocab(rows)
    nodes = noncard_nodes(rows)
    done = load_ckpt()
    todo=[n for n in nodes if (n.get("decimal_path") or "").strip() not in done]
    log(f"START | vocab={len(vocab)} | non-card nodes={len(nodes)} | done(ckpt)={len(done)} | todo={len(todo)}")
    ckpt=open(CKPT,"a",encoding="utf-8")
    processed=0; errs=0
    for i, node in enumerate(todo):
        if limit and processed>=limit:
            log(f"LIMIT {limit} reached -> stop (resume next run)"); break
        dp=(node.get("decimal_path") or "").strip()
        lbl=(node.get("text_fr") or node.get("text_en") or "").strip()
        pool=build_pool(node, rows, bycomma)
        pool_lines="\n".join(f"- {p['dotted']} | {p['label']} | {p['desc']}" for p in pool)
        vocab_str=", ".join(vocab)
        prompt=(f"You are an argumentation-theory expert mapping fallacies to the Walton AIF scheme and to each other.\n\n"
                f"SOURCE NODE:\n- decimal_path(dotted): {comma2dot(dp)}\n- label: {lbl}\n"
                f"- desc FR: {node.get('desc_fr','')}\n- desc EN: {node.get('desc_en','')}\n"
                f"- example FR: {(node.get('example_fr','') or '')[:200]}\n\n"
                f"CANDIDATE TARGET NODES (pick targets ONLY from this list, use the dotted path verbatim):\n{pool_lines}\n\n"
                f"RELATIONSHIP VERBS (choose from these 8):\n"
                + "\n".join(f"- {k}: {v}" for k,v in VERB_DEFS.items()) + "\n\n"
                f"WALTON AIF SCHEME TOKENS (choose DirectRef/ExceptionRef ONLY from this closed list):\n{vocab_str}\n\n"
                f"TASK: Propose 0-4 cross-links to OTHER nodes (not the source) from the candidate list, "
                f"and map the source to 0-2 Walton AIF schemes. Be CONSERVATIVE and PRECISE. Confidence in [0,1].\n"
                f"Return STRICT JSON: "
                f'{{"crossLinks":[{{"verb":"Opposes","target":"1.2.3","confidence":0.8,"rationale":"one line"}}],'
                f'"aif":{{"directRef":["Ignorance_Inference"],"exceptionRef":["PopularOpinion_Inference"],"mappingType":"skos:broadMatch"}}}}. '
                f"Empty arrays if nothing fits.")
        try:
            text, model = call_gpt(prompt, key)
            obj = parse_json(text)
            if obj is None:
                errs+=1; log(f"[{i+1}/{len(todo)}] {dp} {lbl[:28]:28s} -> PARSE FAIL (skipped, not ckpt'd)");
                time.sleep(DELAY); continue
            warns=validate(obj, bydot, vocab)
            rec={"source_dp":dp,"source_dotted":comma2dot(dp),"source_label":lbl,"model":model,"data":obj,"warns":warns}
            ckpt.write(json.dumps(rec, ensure_ascii=False)+"\n"); ckpt.flush()
            processed+=1
            ncl=len(obj.get("crossLinks",[])); a=obj.get("aif",{})
            naif=len(a.get("directRef",[]))+len(a.get("exceptionRef",[]))
            w=(" WARN:"+",".join(set(warns))) if warns else ""
            if (processed%10==0) or warns:
                log(f"[{i+1}/{len(todo)}] done={len(done)+processed}/{len(nodes)} {dp} {lbl[:24]:24s} -> {ncl}cl {naif}aif{w}")
        except Exception as e:
            errs+=1
            log(f"[{i+1}/{len(todo)}] {dp} {lbl[:28]:28s} -> ERROR {type(e).__name__}: {str(e)[:70]} (skipped, not ckpt'd)")
        time.sleep(DELAY)
    ckpt.close()
    log(f"DONE-RUN | processed={processed} | errors={errs} | total_done={len(done)+processed}/{len(nodes)}")

def finalize():
    rows, bycomma, bydot = load_taxonomy()
    recs=[]
    if os.path.exists(CKPT):
        with open(CKPT, encoding="utf-8") as f:
            for line in f:
                line=line.strip()
                if not line: continue
                try: recs.append(json.loads(line))
                except Exception: pass
    log(f"FINALIZE | {len(recs)} records from checkpoint")
    json.dump(recs, open(OUT_JSON,"w",encoding="utf-8"), ensure_ascii=False, indent=1)
    with open(OUT_CSV,"w",encoding="utf-8",newline="") as f:
        w=csv.writer(f); w.writerow(["source_dp","source_dotted","source_label","kind","verb_or_field","target","target_label","confidence","rationale","warns"])
        for r in recs:
            dp=r["source_dp"]; lbl=r["source_label"]; ws=";".join(set(r.get("warns",[])))
            for c in r["data"].get("crossLinks",[]):
                tgt=c.get("target",""); tl=(bydot.get(tgt,{}).get("text_fr") or "") if tgt in bydot else ""
                w.writerow([dp,r.get("source_dotted",""),lbl,"crossLink",c.get("verb"),tgt,tl,c.get("confidence"),c.get("rationale"),ws])
            a=r["data"].get("aif",{})
            for t in a.get("directRef",[]): w.writerow([dp,r.get("source_dotted",""),lbl,"AIF_skosDirectRef","",t,"","",a.get("mappingType",""),ws])
            for t in a.get("exceptionRef",[]): w.writerow([dp,r.get("source_dotted",""),lbl,"AIF_skosExceptionRef","",t,"","",a.get("mappingType",""),ws])
    tot_cl=sum(len(r["data"].get("crossLinks",[])) for r in recs)
    tot_aif=sum(len(r["data"].get("aif",{}).get("directRef",[]))+len(r["data"].get("aif",{}).get("exceptionRef",[])) for r in recs)
    nwarn=sum(1 for r in recs if r.get("warns"))
    log(f"FINALIZE | {len(recs)} nodes | {tot_cl} crossLinks | {tot_aif} AIF refs | {nwarn} nodes w/ warns")
    log(f"Sidecar: {OUT_CSV}")

if __name__=="__main__":
    ap=argparse.ArgumentParser()
    ap.add_argument("--limit", type=int, default=0, help="cap NEW nodes processed this run (0=all)")
    ap.add_argument("--finalize", action="store_true", help="aggregate checkpoint -> sidecar, no API")
    a=ap.parse_args()
    if a.finalize: finalize()
    else: run(a.limit)
