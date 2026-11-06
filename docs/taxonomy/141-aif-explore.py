#!/usr/bin/env python3
"""141 AIF Stage-0 exploration: hierarchy structure, fill state, sample design.

Read-only. 0 write under Cards/. Outputs structural summary to stdout.

#498 grain 1b (06/11) : la hierarchie est lue dans `path` via `141-hierarchy`
(parent, freres, profondeur). La forme coma reste le RENDU des sorties
(dot2comma), jamais la source de la structure. ⚠️ Le filtre de profondeur
utilise path_depth : la colonne CSV `depth` vaut 7 sur la rangee 824 (6 segments)
et 0 sur la racine '0' — la filtrer lirait la mauvaise population.
"""
import csv
import os
import sys
from collections import defaultdict, Counter

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, HERE)
import importlib
H = importlib.import_module("141-hierarchy")

CSV_PATH = r"Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
CROSSLINK = ["crossLink_PredatesOn","crossLink_Denounces","crossLink_Leverages",
             "crossLink_Allows","crossLink_Opposes","crossLink_Inverts",
             "crossLink_Mirrors","crossLink_IsRelatedTo"]
AIF = ["AIF_skosDirectRef","AIF_skosExceptionRef","AIF_skosOther","AIF_skosMappingType"]

def is_card(row):
    c = (row.get("carte") or "").strip()
    return c in ("1","2")

def dp_of(row):
    """Rendu coma du noeud (derive de `path`, pas la colonne d'origine)."""
    return H.dot2comma((row.get("path") or "").strip())

def depth_of(row):
    return H.path_depth((row.get("path") or "").strip())

def main():
    with open(CSV_PATH, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.DictReader(f))
    print(f"Total rows: {len(rows)}")
    cards = [r for r in rows if is_card(r)]
    noncards = [r for r in rows if not is_card(r)]
    print(f"Cards (carte in 1/2): {len(cards)} | Non-card nodes: {len(noncards)}")

    # depth distribution (non-cards) — profondeur derivee de `path`
    depths = Counter(str(depth_of(r)) for r in noncards)
    print(f"\nNon-card depth distribution: {dict(sorted(depths.items(), key=lambda x:(len(x[0]),x[0])))}")

    # top-level families (depth 1)
    fams = sorted(set(dp_of(r) for r in rows if depth_of(r)==1))
    print(f"\nDepth-1 families ({len(fams)}):")
    for fp in fams:
        lbl = next((r.get("text_fr") or r.get("Famille") or "") for r in rows if dp_of(r)==fp)
        print(f"  {fp}: {lbl}")

    # fill state of crossLink_* / AIF_skos* on non-cards
    print(f"\n=== Fill state on non-card nodes ({len(noncards)}) ===")
    for col in CROSSLINK + AIF:
        filled = sum(1 for r in noncards if (r.get(col) or "").strip())
        print(f"  {col:28s}: {filled:4d} / {len(noncards)} ({100*filled/len(noncards):4.1f}%)")

    # siblings structure: group ALL nodes by parent (hierarchie lue dans `path`)
    by_parent = defaultdict(list)
    for r in rows:
        p = (r.get("path") or "").strip()
        by_parent[H.path_parent(p)].append(p)

    # ORACLE: dump every already-filled crossLink_*/AIF_skos* cell (human gold-standard)
    print(f"\n=== ORACLE: existing filled cross-ref cells (validation gold) ===")
    for r in rows:
        for col in CROSSLINK + AIF:
            v=(r.get(col) or "").strip()
            if v:
                dp=dp_of(r)
                lbl=(r.get("text_fr") or "").strip()[:34]
                print(f"  {dp:12s} {col:24s} = {v[:46]:46s}  [{lbl}]")

    # SAMPLE DESIGN: stratified across the 7 families (~4 each), mixing subfamily
    # headers (have children) and leaves (have siblings). Targets ~28 nodes.
    sample_nodes=[]; seen=set()
    for fp in fams:
        fam_prefix = fp + "."
        fam_nodes=[r for r in noncards if (r.get("path") or "").strip().startswith(fam_prefix)]
        # 2 deeper subfamily headers (d4-5) + 2 leaves (d6-7) with siblings
        picked=0
        for r in fam_nodes:
            dp=dp_of(r)
            if dp in seen: continue
            d=depth_of(r)
            sib=[s for s in by_parent.get(H.path_parent((r.get("path") or "").strip()),[]) if s!=(r.get("path") or "").strip()]
            if d in (4,5) and picked<2:
                sample_nodes.append(r); seen.add(dp); picked+=1
            elif d in (6,7) and len(sib)>=1 and picked<4:
                sample_nodes.append(r); seen.add(dp); picked+=1
            if picked>=4: break
    print(f"\n=== SAMPLE DESIGN: {len(sample_nodes)} non-card nodes for Stage-1 pilot ===")
    for r in sample_nodes:
        dp=dp_of(r); d=depth_of(r)
        lbl=(r.get("text_fr") or "").strip()
        sib=[s for s in by_parent.get(H.path_parent((r.get("path") or "").strip()),[]) if s!=(r.get("path") or "").strip()]
        print(f"  [d{d}] {dp:12s} sibs={len(sib):2d}  {lbl[:55]}")

    # dump full context JSON for the sample (for Stage-1 generator input)
    import json
    out = []
    for r in sample_nodes:
        p=(r.get("path") or "").strip()
        dp=H.dot2comma(p)
        sibs=[]
        for s in rows:
            sp=(s.get("path") or "").strip()
            if H.path_parent(sp)==H.path_parent(p) and sp!=p:
                sibs.append({"decimal_path":H.dot2comma(sp),"label":(s.get("text_fr") or s.get("text_en") or "").strip()})
        out.append({
            "decimal_path":dp,
            "depth":str(depth_of(r)),
            "label_fr":(r.get("text_fr") or "").strip(),
            "label_en":(r.get("text_en") or "").strip(),
            "desc_fr":(r.get("desc_fr") or "").strip(),
            "desc_en":(r.get("desc_en") or "").strip(),
            "example_fr":(r.get("example_fr") or "").strip(),
            "current_crossLink":{c:(r.get(c) or "").strip() for c in CROSSLINK},
            "current_AIF":{c:(r.get(c) or "").strip() for c in AIF},
            "siblings":sibs[:40],
        })
    with open(r"tmp/141-aif-sample.json","w",encoding="utf-8") as f:
        json.dump(out,f,ensure_ascii=False,indent=1)
    print(f"\nWrote tmp/141-aif-sample.json ({len(out)} nodes w/ full context + siblings)")

if __name__=="__main__":
    main()
