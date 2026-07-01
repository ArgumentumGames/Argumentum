#!/usr/bin/env python3
"""141 AIF Stage-0 exploration: hierarchy structure, fill state, sample design.

Read-only. 0 write under Cards/. Outputs structural summary to stdout.
"""
import csv
import sys
from collections import defaultdict, Counter

CSV_PATH = r"Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
CROSSLINK = ["crossLink_PredatesOn","crossLink_Denounces","crossLink_Leverages",
             "crossLink_Allows","crossLink_Opposes","crossLink_Inverts",
             "crossLink_Mirrors","crossLink_IsRelatedTo"]
AIF = ["AIF_skosDirectRef","AIF_skosExceptionRef","AIF_skosOther","AIF_skosMappingType"]

def is_card(row):
    c = (row.get("carte") or "").strip()
    return c in ("1","2")

def parent_of(dp):
    # comma form is "family,rest" with ONE comma; rest = 1 digit per level.
    # parent drops the LAST digit of rest: "1,1111"(d5) -> "1,111"(d4); "1,1"(d2) -> "1"(d1).
    dp=(dp or "").strip()
    if "," not in dp: return ""              # depth-1 "1".."7" -> root ""
    fam, rest = dp.split(",", 1)
    if len(rest) <= 1: return fam            # depth-2 -> family root
    return fam + "," + rest[:-1]             # drop last level digit

def main():
    with open(CSV_PATH, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.DictReader(f))
    print(f"Total rows: {len(rows)}")
    cards = [r for r in rows if is_card(r)]
    noncards = [r for r in rows if not is_card(r)]
    print(f"Cards (carte in 1/2): {len(cards)} | Non-card nodes: {len(noncards)}")

    # depth distribution (non-cards)
    depths = Counter((r.get("depth") or "").strip() for r in noncards)
    print(f"\nNon-card depth distribution: {dict(sorted(depths.items(), key=lambda x:(len(x[0]),x[0])))}")

    # top-level families (depth 1)
    fams = sorted(set((r.get("decimal_path") or "") for r in rows if (r.get("depth") or "").strip()=="1"))
    print(f"\nDepth-1 families ({len(fams)}):")
    for fp in fams:
        lbl = next((r.get("text_fr") or r.get("Famille") or "") for r in rows if (r.get("decimal_path") or "").strip()==fp)
        print(f"  {fp}: {lbl}")

    # fill state of crossLink_* / AIF_skos* on non-cards
    print(f"\n=== Fill state on non-card nodes ({len(noncards)}) ===")
    for col in CROSSLINK + AIF:
        filled = sum(1 for r in noncards if (r.get(col) or "").strip())
        print(f"  {col:28s}: {filled:4d} / {len(noncards)} ({100*filled/len(noncards):4.1f}%)")

    # siblings structure: group ALL nodes by parent (comma-sep hierarchy)
    by_parent = defaultdict(list)
    for r in rows:
        dp = (r.get("decimal_path") or "").strip()
        by_parent[parent_of(dp)].append(dp)

    # ORACLE: dump every already-filled crossLink_*/AIF_skos* cell (human gold-standard)
    print(f"\n=== ORACLE: existing filled cross-ref cells (validation gold) ===")
    for r in rows:
        for col in CROSSLINK + AIF:
            v=(r.get(col) or "").strip()
            if v:
                dp=(r.get("decimal_path") or "").strip()
                lbl=(r.get("text_fr") or "").strip()[:34]
                print(f"  {dp:12s} {col:24s} = {v[:46]:46s}  [{lbl}]")

    # SAMPLE DESIGN: stratified across the 7 families (~4 each), mixing subfamily
    # headers (have children) and leaves (have siblings). Targets ~28 nodes.
    sample_nodes=[]; seen=set()
    for fp in fams:
        fam_prefix = fp + ","
        fam_nodes=[r for r in noncards if (r.get("decimal_path") or "").strip().startswith(fam_prefix)]
        # 2 deeper subfamily headers (d4-5) + 2 leaves (d6-7) with siblings
        picked=0
        for r in fam_nodes:
            dp=(r.get("decimal_path") or "").strip()
            if dp in seen: continue
            d=(r.get("depth") or "").strip()
            sib=[s for s in by_parent.get(parent_of(dp),[]) if s!=dp]
            if d in ("4","5") and picked<2:
                sample_nodes.append(r); seen.add(dp); picked+=1
            elif d in ("6","7") and len(sib)>=1 and picked<4:
                sample_nodes.append(r); seen.add(dp); picked+=1
            if picked>=4: break
    print(f"\n=== SAMPLE DESIGN: {len(sample_nodes)} non-card nodes for Stage-1 pilot ===")
    for r in sample_nodes:
        dp=(r.get("decimal_path") or "").strip(); d=(r.get("depth") or "").strip()
        lbl=(r.get("text_fr") or "").strip()
        sib=[s for s in by_parent.get(parent_of(dp),[]) if s!=dp]
        print(f"  [d{d}] {dp:12s} sibs={len(sib):2d}  {lbl[:55]}")

    # dump full context JSON for the sample (for Stage-1 generator input)
    import json
    out = []
    for r in sample_nodes:
        dp=(r.get("decimal_path") or "").strip()
        sibs=[]
        for s in rows:
            sdp=(s.get("decimal_path") or "").strip()
            if parent_of(sdp)==parent_of(dp) and sdp!=dp:
                sibs.append({"decimal_path":sdp,"label":(s.get("text_fr") or s.get("text_en") or "").strip()})
        out.append({
            "decimal_path":dp,
            "depth":(r.get("depth") or "").strip(),
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
