#!/usr/bin/env python3
"""#141 AIF Stage-2 DRY-RUN diff (read-only, 0 write under Cards/).

Compares the Stage-1 full-scale sidecar (#626, gpt-5.5 candidates) against the
EXISTING on-disk AIF values on the taxonomy. The taxonomy has only 12 non-card
nodes with any pre-existing AIF value (0.97%), so the diff is tight and
high-signal: for those 12, did gpt-5.5 confirm the existing token, propose a
different one (CONFLICT -> surface for the expert gate), or stay silent? For the
other 1220 nodes the candidates are entirely net-new (no conflict possible).

Outputs:
  docs/taxonomy/141-aif-stage2-diff.csv   (flat diff rows for the 12 nodes)
  stdout summary

Run:  python docs/taxonomy/141-aif-stage2-diff.py
"""
import csv

CSV_PATH = r"Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
SIDECAR = r"docs/taxonomy/141-aif-candidates-fullscale.csv"
OUT = r"docs/taxonomy/141-aif-stage2-diff.csv"
COLS = ["AIF_skosDirectRef", "AIF_skosExceptionRef", "AIF_skosOther"]


def split_tokens(v):
    return {t.strip() for t in (v or "").split(",") if t.strip()}


def main():
    rows = list(csv.DictReader(open(CSV_PATH, encoding="utf-8-sig", newline="")))
    by_dp = {}
    for r in rows:
        dp = (r.get("decimal_path") or "").strip()
        if dp:
            by_dp[dp] = r

    # existing AIF per node (non-card only)
    existing = {}
    for r in rows:
        c = (r.get("carte") or "").strip()
        dp = (r.get("decimal_path") or "").strip()
        if not dp or c in ("1", "2"):
            continue
        ex = {}
        for col in COLS:
            toks = split_tokens(r.get(col))
            if toks:
                ex[col] = toks
        if ex:
            existing[dp] = ex

    # load Stage-1 sidecar -> proposals per source_dp
    proposals = {}  # dp -> {"DirectRef": set, "ExceptionRef": set}
    with open(SIDECAR, encoding="utf-8", newline="") as f:
        for row in csv.DictReader(f):
            if row.get("kind") not in ("AIF_skosDirectRef", "AIF_skosExceptionRef"):
                continue
            dp = row["source_dp"]
            field = "DirectRef" if row["kind"] == "AIF_skosDirectRef" else "ExceptionRef"
            tok = (row.get("target") or "").strip()
            if tok:
                proposals.setdefault(dp, {}).setdefault(field, set()).add(tok)

    # diff each existing node
    out_rows = []
    counts = {"confirm": 0, "conflict": 0, "silent": 0}
    for dp, ex in sorted(existing.items()):
        label = (by_dp.get(dp, {}).get("text_fr") or by_dp.get(dp, {}).get("text_en") or "").strip()
        prop = proposals.get(dp, {})
        for col, toks in ex.items():
            field = col.replace("AIF_skos", "")  # DirectRef / ExceptionRef / Other
            pset = prop.get(field, set())
            for t in sorted(toks):
                if t in pset:
                    verdict = "CONFIRM"
                    counts["confirm"] += 1
                elif pset:
                    verdict = "CONFLICT"
                    counts["conflict"] += 1
                else:
                    verdict = "SILENT"
                    counts["silent"] += 1
                prop_str = ";".join(sorted(pset)) if pset else "(none)"
                out_rows.append([dp, label, col, t, verdict, prop_str])

    with open(OUT, "w", encoding="utf-8", newline="") as f:
        w = csv.writer(f)
        w.writerow(["source_dp", "source_label", "existing_col", "existing_token",
                    "verdict", "proposed_tokens_for_same_field"])
        w.writerows(out_rows)

    n_nodes = len(existing)
    print(f"#141 AIF Stage-2 DRY-RUN diff")
    print(f"  existing-AIF non-card nodes: {n_nodes} / 1232 ({100*n_nodes//1232}%)")
    print(f"  existing token rows compared: {len(out_rows)}")
    print(f"  CONFIRM (gpt-5.5 agrees w/ existing): {counts['confirm']}")
    print(f"  CONFLICT (gpt-5.5 proposed different): {counts['conflict']}")
    print(f"  SILENT  (gpt-5.5 stayed silent on that field): {counts['silent']}")
    print(f"  -> remaining {1232-n_nodes} nodes: candidates are entirely net-new (no conflict possible)")
    print(f"Sidecar: {OUT}")


if __name__ == "__main__":
    main()
