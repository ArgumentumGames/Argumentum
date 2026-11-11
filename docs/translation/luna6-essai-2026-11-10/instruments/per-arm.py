# -*- coding: utf-8 -*-
# Analyse revue #2231 c.6788606039 : chiffres PAR BRAS, cellules distinctes du corpus
# propre (dédoublonnées (pk,langue,champ) + valeur de correction), coûts selecteur/juge
# séparés, rappel par bras. Contre-mesures attendues (Luna seul, avant bras haiku) :
#   348 verdicts / 337 meilleur = 96,8 % ; 157/163 meilleur sur clean+axis ; 125 cellules
#   distinctes ; 180 meilleur sur copie plantée ; 26 verdicts témoins.
import json, os, sys
from collections import Counter, defaultdict

ROOT = r"D:\Dev\Argumentum\.tmp\1520-luna"
RUNS = os.path.join(ROOT, "runs")

ARM_OF = {
    "clean": "luna", "axis": "luna", "planted": "luna", "incr": "luna",
    "witness-sol": "sol", "witness-haiku": "haiku",
    "haiku-clean": "haiku", "haiku-axis": "haiku", "haiku-planted": "haiku",
}
def arm_for(stage):
    for pre, arm in ARM_OF.items():
        if stage.startswith(pre):
            return arm
    return "?"

verdicts = {}
for fn in os.listdir(os.path.join(RUNS, "judge")):
    if fn.endswith(".json"):
        j = json.load(open(os.path.join(RUNS, "judge", fn), encoding="utf-8"))
        verdicts[j["key"]] = j

per_arm = defaultdict(Counter)          # verdicts par bras
clean_cells = defaultdict(dict)         # arm -> (pk,langue,champ) -> set(corrections)
for j in verdicts.values():
    a = arm_for(j["stage"])
    per_arm[a][j["verdict"]] += 1
    per_arm[a]["total"] += 1
    if j["verdict"] == "meilleur" and not j["stage"].startswith("planted"):
        # cellules candidates corpus propre : clean/axis/haiku-*/witness-* (incr traité à part)
        if j["stage"].startswith(("clean", "axis", "haiku-clean", "haiku-axis", "witness")):
            clean_cells[a].setdefault((j["pk"], j["langue"], j["champ"]), set())

print("== verdicts par bras ==")
for a, c in sorted(per_arm.items()):
    tot = c["total"]
    print(f"  {a:6s}: {tot:3d} verdicts | meilleur {c['meilleur']:3d} = {100*c['meilleur']/tot:.1f} % | pire {c['pire']} | equiv {c['equivalent']}")

print("\n== corpus propre Luna seul (clean+axis, hors incr) ==")
luna_clean = {k: j for k, j in verdicts.items() if arm_for(j["stage"]) == "luna" and j["stage"].startswith(("clean", "axis"))}
tot = len(luna_clean); mb = sum(1 for j in luna_clean.values() if j["verdict"] == "meilleur")
print(f"  {tot} verdicts, {mb} meilleur ({100*mb/tot:.1f} %)")
cells_luna = {(j["pk"], j["langue"], j["champ"]) for j in luna_clean.values() if j["verdict"] == "meilleur"}
print(f"  cellules distinctes 'meilleur': {len(cells_luna)}")
planted_mb = sum(1 for j in verdicts.values() if j["verdict"] == "meilleur" and j["stage"].startswith("planted"))
print(f"  (controle: meilleur sur copie plantee = {planted_mb}, attendu 180)")

# ---- rappel par bras (corpus plante) ----
manifest = json.load(open(os.path.join(ROOT, "planted-defects.json"), encoding="utf-8"))["défauts"]
planted_keys = {(str(d["pk"]), d["langue"], d["champ"]): d for d in manifest}

def recall(prefix):
    hits = {}
    sd_all = []
    for stage in sorted(os.listdir(RUNS)):
        if not stage.startswith(prefix):
            continue
        sd = os.path.join(RUNS, stage)
        if not os.path.isdir(sd):
            continue
        for fn in sorted(os.listdir(sd)):
            if not fn.endswith(".json"):
                continue
            r = json.load(open(os.path.join(sd, fn), encoding="utf-8"))
            for p in r.get("reponse", {}).get("propositions", []):
                key = (str(p["pk"]), p["langue"], p["champ"])
                if key in planted_keys:
                    exact = p.get("extrait_fautif") == planted_keys[key]["plante"]
                    hits.setdefault(key, []).append((p.get("rang"), exact))
    exact_n = sum(1 for lst in hits.values() for _, e in lst if e)
    total_n = sum(len(lst) for lst in hits.values())
    rangs = [r for lst in hits.values() for r, _ in lst if r]
    return dict(retrouves=len(hits), rappel=round(len(hits) / 30, 3),
                retrouvailles=total_n, exactes=exact_n,
                rang_moyen=round(sum(rangs) / len(rangs), 2) if rangs else None,
                manques=[dict(pk=k[0], langue=k[1], champ=k[2], classe=v["classe"], label=v["label"])
                         for k, v in planted_keys.items() if k not in hits])

print("\n== rappel par bras ==")
rec_luna = recall("planted")
print("  Luna :", {k: v for k, v in rec_luna.items() if k != "manques"})
rec_haiku = recall("haiku-planted")
print("  Haiku:", {k: v for k, v in rec_haiku.items() if k != "manques"})
print("  Haiku manques:", json.dumps(rec_haiku["manques"], ensure_ascii=False))

# ---- couts selecteur vs juge ----
recs = [json.loads(l) for l in open(os.path.join(RUNS, "spend.jsonl"), encoding="utf-8") if l.strip()]
def cost(pred):
    return round(sum(r["cost_usd"] for r in recs if pred(r)), 4)
sel_cost = {a: cost(lambda r, a=a: r["arm"] == a and r["stage"] not in ("judge", "etalonnage") and not r["stage"].endswith("-judge"))
            for a in ("luna", "sol", "haiku")}
print("\n== couts ==")
print("  selecteur par bras (propositions, toutes phases):", sel_cost)
print("  juge sol:", cost(lambda r: r["stage"] == "judge" or r["stage"].endswith("-judge")),
      "| etalonnage:", cost(lambda r: r["stage"] == "etalonnage"))
print("  total:", cost(lambda r: True), "$ sur", len(recs), "appels")
judge_n = sum(1 for r in recs if r["stage"] == "judge")
print(f"  cout juge moyen/verdict: {cost(lambda r: r['stage']=='judge')/max(judge_n,1):.5f} $ ({judge_n} verdicts)")

# cout par cellule utile (corpus propre) = (selecteur du bras + quote-part juge) / cellules distinctes
for a in ("luna", "haiku"):
    n_cells = len({(j["pk"], j["langue"], j["champ"]) for j in verdicts.values()
                   if arm_for(j["stage"]) == a and j["verdict"] == "meilleur"
                   and j["stage"].startswith(("clean", "axis", "haiku-clean", "haiku-axis", "witness-haiku"))})
    n_verdicts = per_arm[a]["total"]
    sel = sel_cost[a]
    jpart = n_verdicts * cost(lambda r: r["stage"] == "judge") / max(judge_n, 1)
    if n_cells:
        print(f"  {a}: {n_cells} cellules distinctes | selecteur {sel:.4f} $ + juge ~{jpart:.4f} $ "
              f"=> {(sel + jpart) / n_cells:.4f} $/cellule (selecteur seul {sel / n_cells:.4f} $)")

out = dict(per_arm={a: dict(c) for a, c in per_arm.items()},
           luna_clean=dict(verdicts=tot, meilleur=mb, cellules_distinctes=len(cells_luna)),
           recall=dict(luna=rec_luna, haiku=rec_haiku),
           costs=dict(selecteur=sel_cost, total=cost(lambda r: True)))
haiku_clean = {k: j for k, j in verdicts.items()
               if j["stage"].startswith(("haiku-clean", "haiku-axis", "witness-haiku"))}
hmb = sum(1 for j in haiku_clean.values() if j["verdict"] == "meilleur")
hcells = {(j["pk"], j["langue"], j["champ"]) for j in haiku_clean.values() if j["verdict"] == "meilleur"}
out["haiku_clean"] = dict(verdicts=len(haiku_clean), meilleur=hmb, cellules_distinctes=len(hcells))
print(f"\nhaiku corpus propre: {len(haiku_clean)} verdicts, {hmb} meilleur, {len(hcells)} cellules distinctes")
json.dump(out, open(os.path.join(RUNS, "per-arm.json"), "w", encoding="utf-8"), ensure_ascii=False, indent=1)
print("\necrit runs/per-arm.json")
