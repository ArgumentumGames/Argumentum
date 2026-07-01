#!/usr/bin/env python3
"""#141 AIF WARN `bad_map:*` triage (read-only, 0 write under Cards/).

Triage helper for the 87 non-card nodes whose Stage-1 proposal used an AIF
`mappingType` outside the *observed* on-disk set {broadMatch, closeMatch,
narrowMatch}. These are NOT fabrications (all are legitimate SKOS predicates or
explicit hedges) — they are a schema-extension decision for the expert gate.

Source of truth for WARNs is the structured JSON (the flat sidecar CSV loses the
warn for the ~23 nodes that have a mappingType but 0 DirectRef/ExceptionRef,
since no AIF row is emitted for them). The JSON is regenerable via the
full-scale generator's `--finalize` (from the tmp/ checkpoint), or by re-running
the (non-deterministic) full-scale run; the committed *output* of this script is
the reviewable `141-aif-warn-triage.csv` (87 rows), so a reviewer does not need
to re-run anything to read the triage.

Outputs:
  docs/taxonomy/141-aif-warn-triage.csv  (one row per WARN node)
  stdout summary

Run:  python docs/taxonomy/141-aif-warn-triage.py
"""
import csv, json, os
from collections import defaultdict

JSON_IN = r"docs/taxonomy/141-aif-candidates-fullscale.json"
OUT = r"docs/taxonomy/141-aif-warn-triage.csv"

# recommendation per mappingType family
def reco(mt):
    m = (mt or "").strip()
    if m in ("skos:relatedMatch", "skos:exactMatch"):
        return "ADOPT — extend observed set (legit SKOS predicate)"
    if m in ("none", "skos:noMatch", ""):
        return "DROP — weak hedge, no defensible mapping"
    return "REVIEW"


def main():
    if not os.path.exists(JSON_IN):
        print(f"ERROR: {JSON_IN} not found. Regenerate via:")
        print("  python docs/taxonomy/141-aif-fullscale.py --finalize")
        return
    recs = json.load(open(JSON_IN, encoding="utf-8"))
    warn_nodes = []
    for r in recs:
        ws = r.get("warns", [])
        if not any("bad_map" in w for w in ws):
            continue
        dp = r["source_dp"]
        label = r.get("source_label", "")
        mt = r.get("data", {}).get("aif", {}).get("mappingType", "")
        # extract the exact bad_map verdict(s)
        bm = sorted({w.split("bad_map:", 1)[1] for w in ws if w.startswith("bad_map:")})
        warn_nodes.append((dp, label, mt, ";".join(bm), reco(mt)))
    warn_nodes.sort(key=lambda x: (x[2], x[0]))  # by mappingType then dp

    with open(OUT, "w", encoding="utf-8", newline="") as f:
        w = csv.writer(f)
        w.writerow(["source_dp", "source_label", "mappingType", "bad_map_verdict", "recommendation"])
        w.writerows(warn_nodes)

    # aggregate
    by_mt = defaultdict(int)
    by_reco = defaultdict(int)
    for _, _, mt, _, r in warn_nodes:
        by_mt[mt] += 1
        by_reco[r] += 1
    print(f"#141 AIF WARN bad_map:* triage")
    print(f"  WARN nodes: {len(warn_nodes)} / 1232 ({100*len(warn_nodes)//1232}%)")
    print(f"  --- by mappingType ---")
    for mt, c in sorted(by_mt.items(), key=lambda x: -x[1]):
        print(f"    {mt or '(empty)':24s} {c}")
    print(f"  --- by recommendation ---")
    for r, c in sorted(by_reco.items(), key=lambda x: -x[1]):
        print(f"    {c:3d}  {r}")
    print(f"  Sidecar: {OUT}")


if __name__ == "__main__":
    main()
