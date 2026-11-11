# -*- coding: utf-8 -*-
# Essai #1520 — runner phase payante. Etapes (une par appel CLI), toutes checkpointables :
#   probe          : GET /v1/models (gratuit) — cles + modeles presents
#   etalonnage     : juge sol sur defauts plantes (<=0,05 $) — doit preferer l'original
#   clean          : selecteur Luna, 5 grappes, 1 appel/grappe (taille a)
#   axis           : 6.3 en (b)~50 et (c)~12 ; 2.1 en (c)
#   planted        : selecteur Luna sur corpus plante, tailles (a)+(c), 5 grappes
#   witness-sol    : selecteur gpt-5.6-sol sur 4.1+5.2
#   witness-haiku  : selecteur claude-haiku-5-5 sur 4.1+5.2
#   judge          : juge sol sur TOUTES les propositions non encore jugees
#   incremental    : passes 6.1+2.1 (<=3), corrections "meilleur" appliquees a la copie
# Regles : plafond dur 25 $ (refus d'appel si depassement projete), ledger spend.jsonl,
# reprise : un fichier resultat existant est saute, jamias refait.
# Cles : .keys/openai-key.txt (api.openai.com) et .keys/anthropic-key.txt (compat
#        api.anthropic.com/v1). Jamais la valeur n'est affichee ni ecrite ailleurs.
import json, os, sys, time, hashlib, random, urllib.request, urllib.error
from collections import Counter

ROOT = os.path.dirname(os.path.abspath(__file__))
RUNS = os.path.join(ROOT, "runs")
os.makedirs(RUNS, exist_ok=True)
LEDGER = os.path.join(RUNS, "spend.jsonl")

CEILING = 25.0
ARMS = {
    "luna":   dict(base="https://api.openai.com/v1", keyfile=r"D:\Dev\Argumentum\.keys\openai-key.txt",
                   model="gpt-6-luna", pin=0.10, pout=0.50),
    "sol":    dict(base="https://api.openai.com/v1", keyfile=r"D:\Dev\Argumentum\.keys\openai-key.txt",
                   model="gpt-5.6-sol", pin=4.00, pout=20.00),
    "haiku":  dict(base="https://api.anthropic.com/v1", keyfile=r"D:\Dev\Argumentum\.keys\anthropic-key.txt",
                   model="claude-haiku-5-5", pin=0.10, pout=0.50),
}
LANGS = ["en", "ru", "pt", "es", "ar", "zh", "fa"]
CLUSTER_ORDER = ["5.2", "4.1", "6.1", "2.1", "6.3"]

PROMPT_SELECT = open(os.path.join(ROOT, "prompt-selection.md"), encoding="utf-8").read()
PROMPT_JUGE = open(os.path.join(ROOT, "prompt-juge.md"), encoding="utf-8").read()

# ---------------- ledger ----------------

def cum_spend():
    if not os.path.exists(LEDGER):
        return 0.0
    return sum(json.loads(l)["cost_usd"] for l in open(LEDGER, encoding="utf-8") if l.strip())

def ledger(stage, arm, cluster, batch, tin, tout, extra=None):
    a = ARMS[arm]
    cost = tin / 1e6 * a["pin"] + tout / 1e6 * a["pout"]
    rec = dict(ts=time.strftime("%Y-%m-%dT%H:%M:%S"), stage=stage, arm=arm, model=a["model"],
               cluster=cluster, batch=batch, in_tokens=tin, out_tokens=tout,
               cost_usd=round(cost, 6), cum_usd=round(cum_spend() + cost, 6))
    if extra:
        rec.update(extra)
    with open(LEDGER, "a", encoding="utf-8") as f:
        f.write(json.dumps(rec, ensure_ascii=False) + "\n")
    return rec

def gate(projected=0.0):
    c = cum_spend()
    if c + projected > CEILING:
        print(f"GATE: cumule {c:.4f} $ + projete {projected:.4f} $ > plafond {CEILING} $ — ARRET, post sur l'issue.")
        sys.exit(3)

# ---------------- transport ----------------

def call_api(arm, messages, schema, max_tokens=16000, retries=2):
    """chat.completions avec json_schema strict. Renvoie (dict_parse, tin, tout)."""
    a = ARMS[arm]
    key = open(a["keyfile"], encoding="utf-8").read().strip()
    tok_key = "max_tokens" if arm == "haiku" else "max_completion_tokens"
    body = dict(model=a["model"], messages=messages,
                response_format={"type": "json_schema",
                                 "json_schema": {"name": schema["name"], "strict": True,
                                                  "schema": schema["schema"]}})
    body[tok_key] = max_tokens
    url = a["base"].rstrip("/") + "/chat/completions"
    headers = {"Authorization": "Bearer " + key, "content-type": "application/json"}
    if arm == "haiku":
        headers["anthropic-version"] = "2023-06-01"
    last = None
    for attempt in range(retries + 1):
        req = urllib.request.Request(url, data=json.dumps(body).encode(), headers=headers)
        try:
            with urllib.request.urlopen(req, timeout=180) as r:
                resp = json.loads(r.read())
            u = resp.get("usage", {})
            tin, tout = u.get("input_tokens", u.get("prompt_tokens", 0)), u.get("output_tokens", u.get("completion_tokens", 0))
            content = resp["choices"][0]["message"]["content"]
            return json.loads(content), tin, tout
        except urllib.error.HTTPError as e:
            err = e.read().decode()[:300]
            last = f"HTTP {e.code}: {err}"
            if e.code in (429, 500, 502, 503, 529) and attempt < retries:
                time.sleep(20 * (attempt + 1))
                continue
            raise RuntimeError(last)
        except (json.JSONDecodeError, KeyError, IndexError) as e:
            last = f"reponse non conforme: {e}"
            if attempt < retries:
                time.sleep(5)
                continue
            raise RuntimeError(last)
    raise RuntimeError(last or "echec inconnu")

# ---------------- corpus ----------------

def load(which="clusters"):
    d = json.load(open(os.path.join(ROOT, f"{which}.json"), encoding="utf-8"))
    return d["sha"], d["clusters"]

def render_nodes(nodes):
    """Charge utile comptee au dry-run : 16 cellules cibles + 2 FR + enveloppe."""
    out = []
    for n in nodes:
        item = {"pk": n["pk"], "path": n["path"], "titre_fr": n["titre_fr"],
                "desc_fr": n["desc_fr"], "example_fr": n["example_fr"]}
        for l in LANGS:
            item[f"desc_{l}"] = n[f"desc_{l}"]
            item[f"example_{l}"] = n[f"example_{l}"]
        out.append(item)
    return out

def chunks(nodes, target):
    return [nodes[i:i + target] for i in range(0, len(nodes), target)]

def fingerprint(cell_fr, cell_target):
    h1 = hashlib.sha256(cell_fr.encode("utf-8")).hexdigest()[:16]
    h2 = hashlib.sha256((cell_target or "").encode("utf-8")).hexdigest()[:16]
    return f"{h1}/{h2}"

SEL_SCHEMA = {"name": "propositions", "schema": {
    "type": "object", "additionalProperties": False,
    "required": ["propositions", "rien_a_corriger"],
    "properties": {
        "propositions": {"type": "array", "items": {
            "type": "object", "additionalProperties": False,
            "required": ["rang", "pk", "langue", "champ", "severite", "probleme", "extrait_fautif", "correction"],
            "properties": {
                "rang": {"type": "integer"}, "pk": {"type": "string"},
                "langue": {"type": "string", "enum": LANGS},
                "champ": {"type": "string", "enum": ["desc", "example"]},
                "severite": {"type": "string", "enum": ["grossier", "moyen", "nuance"]},
                "probleme": {"type": "string"}, "extrait_fautif": {"type": "string"},
                "correction": {"type": "string"}}}},
        "rien_a_corriger": {"type": "boolean"}}}}

JUDGE_SCHEMA = {"name": "verdict", "schema": {
    "type": "object", "additionalProperties": False,
    "required": ["verdict", "raison"],
    "properties": {"verdict": {"type": "string", "enum": ["A_meilleur", "B_meilleur", "equivalent"]},
                    "raison": {"type": "string"}}}}

# ---------------- stages ----------------

def stage_probe():
    for arm in ("luna", "haiku"):
        a = ARMS[arm]
        key = open(a["keyfile"], encoding="utf-8").read().strip()
        headers = {"Authorization": "Bearer " + key}
        if arm == "haiku":
            headers["anthropic-version"] = "2023-06-01"
        req = urllib.request.Request(a["base"].rstrip("/") + "/models", headers=headers)
        try:
            with urllib.request.urlopen(req, timeout=30) as r:
                ids = sorted(m.get("id", "") for m in json.loads(r.read()).get("data", []))
            hits = [i for i in ids if ("luna" in i or "sol" in i or "haiku" in i)]
            print(f"{arm}: GET /models OK — {len(ids)} modeles ; pertinents: {hits}")
        except urllib.error.HTTPError as e:
            print(f"{arm}: HTTP {e.code} — {e.read().decode()[:200]}")

def selector_run(arm, stage, clusters_map, cluster_id, nodes, batch_label):
    """1 appel selecteur ; resultat = runs/<stage>/<cluster>__<batch>.json"""
    outdir = os.path.join(RUNS, stage)
    os.makedirs(outdir, exist_ok=True)
    fn = os.path.join(outdir, f"{cluster_id}__{batch_label}.json")
    if os.path.exists(fn):
        print(f"  skip (existant): {os.path.basename(fn)}")
        return json.load(open(fn, encoding="utf-8"))
    payload = render_nodes(nodes)
    user = "Grappe de la taxonomie (JSON) :\n" + json.dumps(payload, ensure_ascii=False)
    approx_in = len(user) / 3
    gate(approx_in / 1e6 * ARMS[arm]["pin"] * 3 + 16000 / 1e6 * ARMS[arm]["pout"])
    t0 = time.time()
    parsed, tin, tout = call_api(arm, [
        {"role": "system", "content": PROMPT_SELECT},
        {"role": "user", "content": user}], SEL_SCHEMA)
    rec = ledger(stage, arm, cluster_id, batch_label, tin, tout, dict(seconds=round(time.time() - t0, 1)))
    # empreintes au SHA fige (calculees ici, pas par le modele)
    by_pk = {n["pk"]: n for n in nodes}
    for p in parsed.get("propositions", []):
        n = by_pk.get(p["pk"])
        if n:
            fr = n[f"{p['champ']}_fr"]
            tgt = n[f"{p['champ']}_{p['langue']}"]
            p["empreinte_fige"] = fingerprint(fr, tgt)
            p["extrait_conforme"] = (p["extrait_fautif"] == tgt)
    rec_out = dict(meta=rec, reponse=parsed, n_nodes=len(nodes))
    json.dump(rec_out, open(fn, "w", encoding="utf-8"), ensure_ascii=False, indent=1)
    print(f"  {os.path.basename(fn)}: {len(parsed.get('propositions', []))} prop. "
          f"({tin}/{tout} tok, {rec['cost_usd']:.5f} $, cum {rec['cum_usd']:.4f} $)")
    return rec_out

def stage_etalonnage():
    sha, cl = load()
    corpus = {n["pk"]: n for c in cl.values() for n in c}
    manifest = json.load(open(os.path.join(ROOT, "planted-defects.json"), encoding="utf-8"))
    defs = manifest["défauts"]
    rng = random.Random(1520)
    sample = [defs[i] for i in (0, 4, 10, 16, 22, 27)]  # etale sur les 7 classes
    results, prefer_orig = [], 0
    for d in sample:
        n = corpus.get(d["pk"])  # corpus ET manifeste en pk CHAINES (mesure : cast int = 100 % de ratés)
        assert n is not None, f"pk {d['pk']} introuvable — GARDE: pas d'appel avec reference cassee"
        fr = n[f"{d['champ']}_fr"]
        original, plante = d["original"], d["plante"]
        ab = rng.random() < 0.5
        a_txt, b_txt = (original, plante) if ab else (plante, original)
        user = (f"Texte FR de référence (fait foi) :\n{fr}\n\n"
                f"Version A :\n{a_txt}\n\nVersion B :\n{b_txt}\n\n"
                f"Contexte — nœud : {d['pk']} ; langue cible : {d['langue']} ; champ : {d['champ']}")
        parsed, tin, tout = call_api("sol", [
            {"role": "system", "content": PROMPT_JUGE}, {"role": "user", "content": user}],
            JUDGE_SCHEMA, max_tokens=300)
        ledger("etalonnage", "sol", "plantes", f"pk{d['pk']}-{d['langue']}-{d['champ']}", tin, tout,
               dict(classe=d["classe"]))
        original_label = "A" if ab else "B"
        v = parsed["verdict"]
        ok = v == f"{original_label}_meilleur"
        prefer_orig += ok
        results.append(dict(pk=d["pk"], langue=d["langue"], champ=d["champ"], classe=d["classe"],
                            original_en=original_label, verdict=v, prefer_original=ok,
                            raison=parsed["raison"]))
        print(f"  pk{d['pk']} {d['langue']}/{d['champ']} classe{d['classe']}: {v} "
              f"({'OK original prefere' if ok else 'ECART'})")
    out = dict(n=len(sample), preferent_original=prefer_orig,
               verdict="juge etalonne" if prefer_orig >= len(sample) - 1 else "JUGE AVEUGLE A DECLARER",
               detail=results)
    json.dump(out, open(os.path.join(RUNS, "etalonnage.json"), "w", encoding="utf-8"),
              ensure_ascii=False, indent=1)
    print(f"ETALONNAGE: {prefer_orig}/{len(sample)} preferent l'original — {out['verdict']}")

def stage_clean():
    sha, cl = load()
    print(f"clean (Luna, taille a) — sha fige {sha}")
    for cid in CLUSTER_ORDER:
        selector_run("luna", "clean", cl, cid, cl[cid], "a")

def stage_axis():
    sha, cl = load()
    for cid, sizes in [("6.3", [("b", 50), ("c", 12)]), ("2.1", [("c", 12)])]:
        for lbl, target in sizes:
            for i, ch in enumerate(chunks(cl[cid], target)):
                selector_run("luna", f"axis-{cid}-{lbl}", cl, cid, ch, f"{lbl}{i+1:02d}")

def stage_planted():
    sha, cl = load("planted-clusters")
    print(f"planted (Luna, tailles a+c) — sha fige {sha}")
    for cid in CLUSTER_ORDER:
        selector_run("luna", "planted-a", cl, cid, cl[cid], "a")
        for i, ch in enumerate(chunks(cl[cid], 12)):
            selector_run("luna", "planted-c", cl, cid, ch, f"c{i+1:02d}")

def stage_witness(arm, stage):
    sha, cl = load()
    for cid in ("4.1", "5.2"):
        selector_run(arm, stage, cl, cid, cl[cid], "a")

# ---------------- juge ----------------

def iter_proposals():
    """Toutes les propositions de tous les runs selecteur, avec leur cellule cible."""
    for stage in sorted(os.listdir(RUNS)):
        sd = os.path.join(RUNS, stage)
        if not os.path.isdir(sd) or stage.startswith("judge"):
            continue
        for fn in sorted(os.listdir(sd)):
            if not fn.endswith(".json"):
                continue
            run = json.load(open(os.path.join(sd, fn), encoding="utf-8"))
            for p in run.get("reponse", {}).get("propositions", []):
                yield stage, fn[:-5], p

def cell_for(p, corpus):
    n = corpus.get(p["pk"])
    if not n:
        return None
    return n[f"{p['champ']}_{p['langue']}"], n[f"{p['champ']}_fr"]

def stage_judge():
    sha, cl = load()
    _, pcl = load("planted-clusters")
    corpus = {n["pk"]: n for c in cl.values() for n in c}
    pcorpus = {n["pk"]: n for c in pcl.values() for n in c}
    jdir = os.path.join(RUNS, "judge")
    os.makedirs(jdir, exist_ok=True)
    rng = random.Random(1520)
    seen, done = set(), 0
    for fn in os.listdir(jdir):
        if fn.endswith(".json"):
            seen.add(fn[:-5])
    props = list(iter_proposals())
    print(f"juge: {len(props)} propositions, {len(seen)} deja jugees")
    for stage, run, p in props:
        key = f"{stage}__{run}__{p['pk']}__{p['langue']}__{p['champ']}"
        if key in seen:
            continue
        current, fr = cell_for(p, pcorpus if stage.startswith("planted") else corpus)
        if current is None or fr is None:
            continue
        prop_txt = p["correction"]
        ab = rng.random() < 0.5
        a_txt, b_txt = (current, prop_txt) if ab else (prop_txt, current)
        user = (f"Texte FR de référence (fait foi) :\n{fr}\n\nVersion A :\n{a_txt}\n\nVersion B :\n{b_txt}\n\n"
                f"Contexte — nœud : {p['pk']} ; langue cible : {p['langue']} ; champ : {p['champ']}")
        gate(2000 / 1e6 * 4 + 400 / 1e6 * 20)
        parsed, tin, tout = call_api("sol", [
            {"role": "system", "content": PROMPT_JUGE}, {"role": "user", "content": user}],
            JUDGE_SCHEMA, max_tokens=400)
        ledger("judge", "sol", stage, run, tin, tout)
        current_label = "A" if ab else "B"
        v = parsed["verdict"]
        verdict = ("meilleur" if v == f"{'B' if ab else 'A'}_meilleur"
                   else "pire" if v == f"{current_label}_meilleur" else "equivalent")
        out = dict(key=key, stage=stage, run=run, pk=p["pk"], langue=p["langue"], champ=p["champ"],
                   severite=p.get("severite"), probleme=p.get("probleme"),
                   verdict=verdict, raison=parsed["raison"],
                   empreinte_fige=p.get("empreinte_fige"), extrait_conforme=p.get("extrait_conforme"))
        json.dump(out, open(os.path.join(jdir, key.replace("/", "_") + ".json"), "w", encoding="utf-8"),
                  ensure_ascii=False, indent=1)
        seen.add(key)
        done += 1
        if done % 25 == 0:
            print(f"  {done} jugees (cum {cum_spend():.4f} $)")
    print(f"juge: termine, {done} nouvelles verdicts, cum {cum_spend():.4f} $")

def stage_incremental():
    """Passes 6.1+2.1 (<=3) : selection Luna sur copie de travail, juge par proposition,
    application des 'meilleur', arret si reste que des nuance ou precision < 70 %."""
    import copy, random
    rng = random.Random(1520)
    sha, cl = load()
    working = {cid: copy.deepcopy(cl[cid]) for cid in ("6.1", "2.1")}
    by_pk = {n["pk"]: n for c in working.values() for n in c}
    summary = []
    fn = os.path.join(RUNS, "incremental.json")
    if os.path.exists(fn):
        print("  skip (existant): incremental.json — deja jouee")
        return
    for npass in range(1, 4):
        stage_name = f"incr{npass}"
        props_all = []
        for cid in ("6.1", "2.1"):
            r = selector_run("luna", stage_name, working, cid, working[cid], "a")
            props_all += [(cid, p) for p in r["reponse"].get("propositions", [])]
        if not props_all:
            summary.append(dict(passe=npass, propositions=0, note="rien a corriger — arret"))
            break
        verd = Counter()
        applied = 0
        for cid, p in props_all:
            n = by_pk.get(p["pk"])
            if not n:
                continue
            cur = n[f"{p['champ']}_{p['langue']}"]
            fr = n[f"{p['champ']}_fr"]
            ab = rng.random() < 0.5
            a_txt, b_txt = (cur, p["correction"]) if ab else (p["correction"], cur)
            user = (f"Texte FR de référence (fait foi) :\n{fr}\n\nVersion A :\n{a_txt}\n\nVersion B :\n{b_txt}\n\n"
                    f"Contexte — nœud : {p['pk']} ; langue cible : {p['langue']} ; champ : {p['champ']}")
            gate(2000 / 1e6 * 4 + 400 / 1e6 * 20)
            parsed, tin, tout = call_api("sol", [
                {"role": "system", "content": PROMPT_JUGE}, {"role": "user", "content": user}],
                JUDGE_SCHEMA, max_tokens=400)
            ledger(f"incr{npass}-judge", "sol", cid, f"pk{p['pk']}-{p['langue']}-{p['champ']}", tin, tout)
            cur_label = "A" if ab else "B"
            v = parsed["verdict"]
            verdict = ("meilleur" if v == f"{'B' if ab else 'A'}_meilleur"
                       else "pire" if v == f"{cur_label}_meilleur" else "equivalent")
            verd[verdict] += 1
            if verdict == "meilleur":
                n[f"{p['champ']}_{p['langue']}"] = p["correction"]
                applied += 1
        judged_n = sum(verd.values())
        precision = verd.get("meilleur", 0) / judged_n if judged_n else 1.0
        degradation = verd.get("pire", 0) / judged_n if judged_n else 0.0
        sevs = Counter(p.get("severite", "?") for _, p in props_all)
        summary.append(dict(passe=npass, propositions=len(props_all), severites=dict(sevs),
                            verdicts=dict(verd), appliquees=applied,
                            precision=round(precision, 3), degradation=round(degradation, 3)))
        print(f"  passe {npass}: {len(props_all)} prop., precision {precision:.0%}, "
              f"degradation {degradation:.0%}, appliquees {applied}")
        only_nuance = set(sevs) <= {"nuance"}
        if only_nuance or precision < 0.70:
            summary[-1]["note"] = ("arret: reste que des nuance" if only_nuance
                                    else f"arret: precision {precision:.0%} < 70 %")
            break
    json.dump(summary, open(fn, "w", encoding="utf-8"), ensure_ascii=False, indent=1)
    print("incremental:", json.dumps(summary, ensure_ascii=False)[:200])

STAGES = dict(probe=stage_probe, etalonnage=stage_etalonnage, clean=stage_clean,
              axis=stage_axis, planted=stage_planted,
              witness_sol=lambda: stage_witness("sol", "witness-sol"),
              witness_haiku=lambda: stage_witness("haiku", "witness-haiku"),
              judge=stage_judge, incremental=stage_incremental)

if __name__ == "__main__":
    if len(sys.argv) != 2 or sys.argv[1] not in STAGES:
        print("usage: essai.py <probe|etalonnage|clean|axis|planted|witness_sol|witness_haiku|judge|incremental>")
        sys.exit(2)
    STAGES[sys.argv[1]]()
    print(f"cumul spend: {cum_spend():.4f} $ / {CEILING} $")
