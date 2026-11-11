# -*- coding: utf-8 -*-
# Chiffres PAR BRAS, cellules distinctes du corpus propre (dédoublonnées
# (pk,langue,champ)), coûts sélecteur/juge séparés, rappel par bras, recouvrement
# Luna/Haiku et bornes haute du candidat Luna.
# Entrées : les fichiers COMMITÉS du dossier — resultats/verdicts.jsonl,
# resultats/selector-runs.jsonl, resultats/spend.jsonl, planted-defects.json
# (aucune dépendance au répertoire de campagne .tmp — reproductible depuis tout clone).
# Sortie : resultats/per-arm.json (surchargeable par LUNA6_RESULTS).
import json, os, sys
from collections import Counter, defaultdict
from decimal import Decimal

HERE = os.path.dirname(os.path.abspath(__file__))
DOSSIER = os.path.dirname(HERE)
RESULTS = os.environ.get("LUNA6_RESULTS", os.path.join(DOSSIER, "resultats"))
MANIFEST = os.path.join(os.path.dirname(RESULTS), "planted-defects.json")

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

def read_jsonl(name):
    path = os.path.join(RESULTS, name)
    with open(path, encoding="utf-8") as f:
        return [json.loads(l) for l in f if l.strip()]

verdicts = {j["key"]: j for j in read_jsonl("verdicts.jsonl")}
runs = read_jsonl("selector-runs.jsonl")

per_arm = defaultdict(Counter)          # verdicts par bras
for j in verdicts.values():
    a = arm_for(j["stage"])
    per_arm[a][j["verdict"]] += 1
    per_arm[a]["total"] += 1

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

haiku_clean = {k: j for k, j in verdicts.items()
               if j["stage"].startswith(("haiku-clean", "haiku-axis", "witness-haiku"))}
hmb = sum(1 for j in haiku_clean.values() if j["verdict"] == "meilleur")
hcells = {(j["pk"], j["langue"], j["champ"]) for j in haiku_clean.values() if j["verdict"] == "meilleur"}
print(f"  haiku corpus propre: {len(haiku_clean)} verdicts, {hmb} meilleur, {len(hcells)} cellules distinctes")

common = cells_luna & hcells
union = cells_luna | hcells
print(f"  recouvrement Luna/Haiku: {len(common)} cellules communes, union {len(union)}")

# ---- bornes haute du candidat Luna (corpus propre + incrémental) ----
incr_all = {(j["pk"], j["langue"], j["champ"]) for j in verdicts.values() if j["stage"].startswith("incr")}
incr_best = {(j["pk"], j["langue"], j["champ"]) for j in verdicts.values()
             if j["stage"].startswith("incr") and j["verdict"] == "meilleur"}
borne_best = len(cells_luna | incr_best)      # seules les corrections jugées « meilleur »
borne_all = len(cells_luna | incr_all)        # toutes cellules incrémentales proposées (y c. non « meilleur »)
print(f"  bornes Luna: {borne_best} (meilleur seul) / {borne_all} (toutes propositions incrémentales)")

# ---- rappel par bras (corpus planté) ----
manifest = json.load(open(MANIFEST, encoding="utf-8"))["défauts"]
planted_keys = {(str(d["pk"]), d["langue"], d["champ"]): d for d in manifest}

def recall(prefix):
    hits = {}
    for r in runs:
        if "meta" not in r or not r["meta"]["stage"].startswith(prefix):
            continue
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

# ---- coûts sélecteur vs juge ----
recs = read_jsonl("spend.jsonl")
def cost(pred):
    return round(sum((Decimal(str(r["cost_usd"])) for r in recs if pred(r)), Decimal(0)), 4)
def is_judge(r):
    return r["stage"] == "judge" or r["stage"].endswith("-judge")
sel_cost = {a: cost(lambda r, a=a: r["arm"] == a and r["stage"] not in ("judge", "etalonnage") and not r["stage"].endswith("-judge"))
            for a in ("luna", "sol", "haiku")}
print("\n== couts ==")
print("  selecteur par bras (propositions, toutes phases):", {a: float(v) for a, v in sel_cost.items()})
print("  juge sol:", cost(is_judge),
      "| etalonnage:", cost(lambda r: r["stage"] == "etalonnage"))
print("  total:", cost(lambda r: True), "$ sur", len(recs), "appels")
judge_n = sum(1 for r in recs if is_judge(r))
judge_total = cost(is_judge)
print(f"  cout juge moyen/appel: {judge_total/max(judge_n,1):.5f} $ ({judge_n} appels juge, {len(verdicts)} verdicts persistes)")

# coût par cellule utile (corpus propre) = (sélecteur du bras + quote-part juge) / cellules distinctes
for a, cells in (("luna", cells_luna), ("haiku", hcells)):
    n_cells = len(cells)
    n_verdicts = per_arm[a]["total"]
    sel = sel_cost[a]
    jpart = n_verdicts * judge_total / max(judge_n, 1)
    if n_cells:
        print(f"  {a}: {n_cells} cellules distinctes | selecteur {sel:.4f} $ + juge ~{jpart:.4f} $ "
              f"=> {(sel + jpart) / n_cells:.4f} $/cellule (selecteur seul {sel / n_cells:.4f} $)")

out = dict(per_arm={a: dict(c) for a, c in per_arm.items()},
           luna_clean=dict(verdicts=tot, meilleur=mb, cellules_distinctes=len(cells_luna)),
           haiku_clean=dict(verdicts=len(haiku_clean), meilleur=hmb, cellules_distinctes=len(hcells)),
           recouvrement=dict(luna=len(cells_luna), haiku=len(hcells),
                             cellules_communes=len(common), union=len(union)),
           bornes_luna=dict(meilleur_seul=borne_best, toutes_propositions_incrementales=borne_all,
                            cellules_incrementales_meilleur=len(incr_best),
                            cellules_incrementales_proposees=len(incr_all)),
           recall=dict(luna=rec_luna, haiku=rec_haiku),
           costs=dict(selecteur={a: float(v) for a, v in sel_cost.items()}, judge=float(judge_total),
                      judge_calls=judge_n, judge_verdicts=len(verdicts),
                      calibration=float(cost(lambda r: r["stage"] == "etalonnage")),
                      calls=len(recs), total=float(cost(lambda r: True))))
out_path = os.path.join(RESULTS, "per-arm.json")
json.dump(out, open(out_path, "w", encoding="utf-8"), ensure_ascii=False, indent=1)
print(f"\necrit {out_path}")
