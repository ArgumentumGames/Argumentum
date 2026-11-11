# -*- coding: utf-8 -*-
# Essai #1520 — agregateur. Lit runs/ et produit aggregate.json :
#   - par grappe x taille: propositions, severites, precision, degradation, rappel (plante), rang moyen
#   - comparaison Luna / Sol / Haiku sur 4.1+5.2
#   - cout par correction utile (verdict "meilleur")
#   - exemples (meilleur + pire) avec textes entiers
# Rejouable a tout moment (s'arrete proprement si le juge n'a pas tout couvert).
import json, os, sys
from collections import Counter, defaultdict

ROOT = os.path.dirname(os.path.abspath(__file__))
RUNS = os.path.join(ROOT, "runs")

def load_clusters(name="clusters"):
    return json.load(open(os.path.join(ROOT, f"{name}.json"), encoding="utf-8"))["clusters"]

def iter_runs(stage_prefix=""):
    for stage in sorted(os.listdir(RUNS)):
        sd = os.path.join(RUNS, stage)
        if not os.path.isdir(sd) or stage == "judge":
            continue
        if stage_prefix and not stage.startswith(stage_prefix):
            continue
        for fn in sorted(os.listdir(sd)):
            if fn.endswith(".json"):
                yield stage, fn[:-5], json.load(open(os.path.join(sd, fn), encoding="utf-8"))

def load_judges():
    jd = os.path.join(RUNS, "judge")
    out = {}
    if not os.path.isdir(jd):
        return out
    for fn in os.listdir(jd):
        if fn.endswith(".json"):
            j = json.load(open(os.path.join(jd, fn), encoding="utf-8"))
            out[j["key"]] = j
    return out

def jkey(stage, run, p):
    return f"{stage}__{run}__{p['pk']}__{p['langue']}__{p['champ']}"

def main():
    cl = load_clusters()
    pcl = load_clusters("planted-clusters")
    corpus = {n["pk"]: n for c in cl.values() for n in c}
    pcorpus = {n["pk"]: n for c in pcl.values() for n in c}
    judges = load_judges()
    manifest = json.load(open(os.path.join(ROOT, "planted-defects.json"), encoding="utf-8"))["défauts"]
    planted_keys = {(d["pk"], d["langue"], d["champ"]): d for d in manifest}

    out = {"per_stage": {}, "recall": {}, "witness": {}, "examples": {"meilleur": [], "pire": []}}
    useful_total = 0
    cost = 0.0
    spend_path = os.path.join(RUNS, "spend.jsonl")
    if os.path.exists(spend_path):
        recs = [json.loads(l) for l in open(spend_path, encoding="utf-8") if l.strip()]
        cost = sum(r["cost_usd"] for r in recs)
        out["spend"] = dict(calls=len(recs), total_usd=round(cost, 4),
                            by_arm={a: round(sum(r["cost_usd"] for r in recs if r["arm"] == a), 4)
                                    for a in {r["arm"] for r in recs}},
                            by_stage={s: round(sum(r["cost_usd"] for r in recs if r["stage"] == s), 4)
                                      for s in {r["stage"] for r in recs}})

    for stage, run, r in iter_runs():
        props = r["reponse"].get("propositions", [])
        sev = Counter(p.get("severite", "?") for p in props)
        verd = Counter()
        judged = 0
        for p in props:
            k = jkey(stage, run, p)
            if k in judges:
                verd[judges[k]["verdict"]] += 1
                judged += 1
                if judges[k]["verdict"] == "meilleur":
                    useful_total += 1
                    src = pcorpus if stage.startswith("planted") else corpus
                    n = src.get(p["pk"])
                    if n and len(out["examples"]["meilleur"]) < 12:
                        out["examples"]["meilleur"].append(
                            dict(stage=stage, run=run, pk=p["pk"], langue=p["langue"], champ=p["champ"],
                                 severite=p.get("severite"), probleme=p.get("probleme"),
                                 fr=n[f"{p['champ']}_fr"], avant=n[f"{p['champ']}_{p['langue']}"],
                                 apres=p["correction"], raison=judges[k]["raison"]))
                elif judges[k]["verdict"] == "pire" and len(out["examples"]["pire"]) < 12:
                    src = pcorpus if stage.startswith("planted") else corpus
                    n = src.get(p["pk"])
                    if n:
                        out["examples"]["pire"].append(
                            dict(stage=stage, run=run, pk=p["pk"], langue=p["langue"], champ=p["champ"],
                                 severite=p.get("severite"), probleme=p.get("probleme"),
                                 fr=n[f"{p['champ']}_fr"], avant=n[f"{p['champ']}_{p['langue']}"],
                                 apres=p["correction"], raison=judges[k]["raison"]))
        st = out["per_stage"].setdefault(stage, dict(runs=0, propositions=0, severite={}, verdicts={},
                                                     judged=0, precision=None, degradation=None))
        st["runs"] += 1
        st["propositions"] += len(props)
        st["judged"] += judged
        st["severite"].update({k: st["severite"].get(k, 0) + v for k, v in sev.items()})
        st["verdicts"].update({k: st["verdicts"].get(k, 0) + v for k, v in verd.items()})
        tot_j = st["judged"]
        if tot_j:
            st["precision"] = round(st["verdicts"].get("meilleur", 0) / tot_j, 3)
            st["degradation"] = round(st["verdicts"].get("pire", 0) / tot_j, 3)

    # rappel : defauts plantes retrouves dans les runs plantes
    hits = []
    for stage, run, r in iter_runs("planted"):
        for i, p in enumerate(r["reponse"].get("propositions", [])):
            key = (str(p["pk"]), p["langue"], p["champ"])
            if key in planted_keys:
                planted_cell = planted_keys[key]["plante"]
                hits.append(dict(pk=key[0], langue=key[1], champ=key[2],
                                 classe=planted_keys[key]["classe"], label=planted_keys[key]["label"],
                                 rang=p.get("rang"), stage=stage, run=run,
                                 vise_le_plante=(p.get("extrait_fautif") == planted_cell),
                                 correction_differe_du_plante=(p.get("correction") != planted_cell)))
    # rang moyen + part retrouves
    found_keys = {(h["pk"], h["langue"], h["champ"]) for h in hits}
    rangs = [h["rang"] for h in hits if h.get("rang")]
    out["recall"] = dict(plantes=len(manifest), retrouves=len(found_keys),
                         rappel=round(len(found_keys) / len(manifest), 3),
                         rang_moyen=round(sum(rangs) / len(rangs), 2) if rangs else None,
                         detail=sorted(hits, key=lambda h: (h["classe"], h["pk"])))

    out["useful_corrections"] = useful_total
    if useful_total:
        out["cost_per_useful_usd"] = round(cost / useful_total, 4)

    json.dump(out, open(os.path.join(RUNS, "aggregate.json"), "w", encoding="utf-8"),
              ensure_ascii=False, indent=1)
    # resume console
    print(f"depense {cost:.4f} $ | corrections utiles {useful_total} "
          f"| cout/correction {out.get('cost_per_useful_usd', '-')} $")
    print(f"rappel: {out['recall']['retrouves']}/{out['recall']['plantes']} "
          f"({out['recall']['rappel']*100:.0f}%), rang moyen {out['recall']['rang_moyen']}")
    for s, st in sorted(out["per_stage"].items()):
        print(f"  {s:16s} props={st['propositions']:4d} jugees={st['judged']:4d} "
              f"precision={st['precision']} degradation={st['degradation']}")

if __name__ == "__main__":
    main()
