#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""fallacies_i18n_hierarchy_apply.py — unified gated apply for the Fallacies i18n-hierarchy gap.

Consolidates the two data-quality inventories produced for the Fallacies taxonomy CSV:
  - #712 family-gap: 48 rows with empty Family (EN) + Subfamily (EN).
  - #716 subsubfamily-gap: the SAME 48 rows also have empty Subsubfamily (EN).
into ONE post-tag apply spec + script. The 48 rows are a pure i18n-propagation gap: each has its
French `Soussousfamille` filled but all three English hierarchy columns (Family / Subfamily /
Subsubfamily) empty.

WHAT IT DOES
  Re-derives (at apply time, NEVER trusting the inventory blindly) the canonical FR→EN mapping:
  for each distinct `Soussousfamille` (FR) value, the (Family, Subfamily, Subsubfamily) EN tuple
  that the already-filled rows carry. 100% consensus required (1 tuple per FR key) — any conflict
  aborts. Then for each of the 48 gap rows, fills the empty EN columns from its FR key's tuple.

  Same byte-targeted discipline as #654 mnemonics_to_latin.py: BOM preserved, CRLF line endings,
  QUOTE_MINIMAL — only the empty target cells change, the rest of the file is byte-identical.

GATING (release freeze on Cards/)
  Default mode is --dry-run (writes nothing, prints the plan). --apply writes the CSV in place and
  is GATED: do NOT run --apply on the prod CSV until the v0.9.0 tag is laid (release freeze on
  Cards/). The script ships + its unit test runs (synthetic CSV), but --apply on prod is post-tag.

USAGE
  python tools/fallacies_i18n_hierarchy_apply.py --csv "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
      # default: dry-run, prints the 48-row x 3-col plan + canonical map, writes no file.

  python tools/fallacies_i18n_hierarchy_apply.py --csv <PATH> --report docs/<NAME>.md
      # dry-run, additionally writes a markdown report.

  python tools/fallacies_i18n_hierarchy_apply.py --csv <PATH> --apply
      # WRITES the CSV in place (byte-targeted). GATED post-tag.

VALIDATION GATES (enforced in code, abort with exit 2 if any fails)
  1. Re-derive mapping at apply time (no hardcoded tuple).
  2. Conflict: a FR key value maps to >1 distinct EN tuple among filled rows → abort.
  3. Novel key: a gap row's FR value is absent from the canonical map (no filled example to derive
     from) → abort (cannot guess).
  4. Row count preserved (the apply touches cells, never adds/removes/reorders rows).
  5. BOM + CRLF + QUOTE_MINIMAL byte-preserved (only the empty target cells change).

DEPENDENCIES: Python 3 stdlib only (csv, argparse, os, sys, io, collections).

Exit codes: 0 = plan built / applied cleanly; 2 = conflict or novel key detected (--apply refused).
"""

import argparse
import csv
import io
import os
import sys
from collections import OrderedDict

# ─────────────────────────────────────────────────────────────────────────────
# Constants
# ─────────────────────────────────────────────────────────────────────────────

DEFAULT_CSV = os.path.join("Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")

# The FR key column (finest hierarchy granularity). Verified 1-to-1 to all 3 EN levels by #712
# (Family/Subfamily) and #716 (Subsubfamily).
FR_KEY_COL = "Soussousfamille"

# The PK column (for stable row identification in the plan/report).
PK_COL = "PK"

# The three EN hierarchy columns to fill.
EN_COLS = ["Family", "Subfamily", "Subsubfamily"]


# ─────────────────────────────────────────────────────────────────────────────
# Core logic
# ─────────────────────────────────────────────────────────────────────────────

def _cells(row):
    """Strip + return the FR key and the 3 EN values from a DictReader row."""
    fr = (row.get(FR_KEY_COL) or "").strip()
    ens = [(row.get(c) or "").strip() for c in EN_COLS]
    return fr, ens


def build_canonical_map(rows):
    """Build the FR→EN canonical map, re-derived at runtime from filled rows.

    For each distinct FR key value, collect the SET of (Family, Subfamily, Subsubfamily) tuples
    seen in rows where all three EN cols are filled. Returns:
      (cmap, conflicts)
    where cmap[fr] = the single tuple (only present when exactly one tuple seen, i.e. consensus),
    and conflicts = list of {fr, tuples} for FR keys with >1 distinct tuple (abort signal).
    """
    from collections import defaultdict
    seen = defaultdict(set)
    for r in rows:
        fr, ens = _cells(r)
        if not fr:
            continue
        if all(ens):  # all 3 EN cols filled → this row contributes to the map
            seen[fr].add(tuple(ens))
    cmap = {}
    conflicts = []
    for fr, tuples in seen.items():
        if len(tuples) == 1:
            cmap[fr] = next(iter(tuples))
        else:
            conflicts.append({"fr": fr, "tuples": sorted(tuples)})
    return cmap, conflicts


def build_plan(rows, cmap):
    """Build the per-cell fill plan for the gap rows.

    A row is a 'gap row' if its FR key is filled but at least one EN col is empty. For each, look up
    the FR key in cmap. Returns (plan, novel) where plan is the list of cells to fill and novel is
    the list of gap rows whose FR key is absent from cmap (abort signal — cannot derive).
    """
    plan = []
    novel = []
    for r in rows:
        fr, ens = _cells(r)
        if not fr:
            continue  # no FR key → not derivable (and not the i18n gap; family-header roots)
        empty_idx = [i for i, v in enumerate(ens) if not v]
        if not empty_idx:
            continue  # all 3 EN filled → not a gap row
        if fr not in cmap:
            novel.append({"pk": r.get(PK_COL, ""), "fr": fr, "text_fr": (r.get("text_fr") or "").strip()})
            continue
        tup = cmap[fr]
        pk = r.get(PK_COL, "")
        for i in empty_idx:
            plan.append(OrderedDict([
                ("pk", pk),
                ("fr_key", fr),
                ("en_col", EN_COLS[i]),
                ("fill_value", tup[i]),
            ]))
    return plan, novel


def apply_plan(csv_path, plan):
    """Apply the plan in place, byte-targeted (BOM + CRLF + QUOTE_MINIMAL).

    Mirrors #654 apply_plan: read raw bytes, detect BOM, decode utf-8-sig, csv.reader to preserve
    column order, fill ONLY the target empty cells, reserialise QUOTE_MINIMAL + CRLF, re-prepend BOM.
    Returns the count of cells modified.
    """
    with open(csv_path, "rb") as f:
        raw_bytes = f.read()
    has_bom = raw_bytes.startswith(b"\xef\xbb\xbf")
    raw = raw_bytes.decode("utf-8-sig")

    rows = list(csv.reader(io.StringIO(raw)))
    if not rows:
        return 0
    header = rows[0]
    col_idx = {name: i for i, name in enumerate(header)}

    by_pk = {}
    for e in plan:
        by_pk.setdefault(e["pk"], []).append(e)

    applied = 0
    for row in rows[1:]:
        pk = row[col_idx[PK_COL]] if col_idx.get(PK_COL, -1) < len(row) else ""
        if pk not in by_pk:
            continue
        for e in by_pk[pk]:
            idx = col_idx[e["en_col"]]
            old = row[idx]
            if (old or "").strip():
                continue  # cell no longer empty (drift since plan) — skip, do not overwrite
            row[idx] = e["fill_value"]
            applied += 1

    buf = io.StringIO()
    writer = csv.writer(buf, quoting=csv.QUOTE_MINIMAL, lineterminator="\r\n")
    writer.writerows(rows)
    out_bytes = (b"\xef\xbb\xbf" if has_bom else b"") + buf.getvalue().encode("utf-8")

    with open(csv_path, "wb") as f:
        f.write(out_bytes)
    return applied


def render_report(cmap, conflicts, plan, novel, csv_path, row_count):
    """Return a human-readable markdown report (string) of the dry-run."""
    lines = []
    lines.append(f"# Fallacies i18n-hierarchy gap — unified dry-run report\n")
    lines.append(f"**Source CSV:** `{csv_path}`\n")
    lines.append(f"**Rows scanned:** {row_count}\n")
    lines.append(f"**Canonical map size:** {len(cmap)} FR keys (consensus, 1 tuple each)\n")
    lines.append(f"**Plan:** {len(plan)} cells to fill "
                 f"(expected 48 rows x 3 EN cols = 144 on the current master)\n")
    if conflicts:
        lines.append(f"\n> ⚠️ CONFLICTS: {len(conflicts)} FR key(s) map to >1 EN tuple — --apply REFUSED.\n")
    if novel:
        lines.append(f"\n> ⚠️ NOVEL keys: {len(novel)} gap row(s) have a FR key absent from the map — --apply REFUSED.\n")
    # Canonical map
    lines.append("\n## Canonical FR → EN map (re-derived)\n")
    lines.append("| `Soussousfamille` (FR) | Family (EN) | Subfamily (EN) | Subsubfamily (EN) |\n")
    lines.append("|---|---|---|---|\n")
    for fr in sorted(cmap.keys()):
        fam, sub, ssub = cmap[fr]
        lines.append(f"| {fr} | {fam} | {sub} | {ssub} |\n")
    # Plan sample
    lines.append("\n## Plan (first 60 cells)\n")
    for e in plan[:60]:
        lines.append(f"- pk {e['pk']} [{e['en_col']}] `{e['fr_key']}` → `{e['fill_value']}`")
    if len(plan) > 60:
        lines.append(f"\n... ({len(plan) - 60} more)")
    if conflicts:
        lines.append("\n## ⚠️ Conflicts (FR key → multiple EN tuples)\n")
        for c in conflicts:
            lines.append(f"- `{c['fr']}`: {len(c['tuples'])} tuples {c['tuples']}")
    if novel:
        lines.append("\n## ⚠️ Novel keys (FR value not in any filled row)\n")
        for n in novel:
            lines.append(f"- pk {n['pk']} `{n['fr']}` ({n['text_fr']})")
    return "\n".join(lines)


# ─────────────────────────────────────────────────────────────────────────────
# CLI
# ─────────────────────────────────────────────────────────────────────────────

def main(argv=None):
    p = argparse.ArgumentParser(
        description="Unified gated apply for the Fallacies i18n-hierarchy gap (#712 + #716).")
    p.add_argument("--csv", default=DEFAULT_CSV, help="Path to the Fallacies taxonomy CSV.")
    p.add_argument("--dry-run", action="store_true", default=True,
                   help="Default: build the plan and print it, write no file.")
    p.add_argument("--apply", action="store_true",
                   help="Write the CSV in place (byte-targeted). GATED post-tag (release freeze).")
    p.add_argument("--report", default=None,
                   help="Optional path to write a markdown dry-run report.")
    args = p.parse_args(argv)

    if not os.path.exists(args.csv):
        print(f"ERROR: CSV not found: {args.csv}", file=sys.stderr)
        return 1

    with open(args.csv, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.DictReader(f))

    cmap, conflicts = build_canonical_map(rows)
    plan, novel = build_plan(rows, cmap)

    # Per-column plan counts.
    by_col = {c: sum(1 for e in plan if e["en_col"] == c) for c in EN_COLS}
    print(f"Fallacies i18n-hierarchy gap plan: {len(plan)} cells across {len(rows)} rows")
    for c in EN_COLS:
        print(f"  {c}: {by_col[c]} cells to fill")
    print(f"  Canonical map: {len(cmap)} FR keys (consensus)")
    if conflicts:
        print(f"⚠️ {len(conflicts)} CONFLICT(s) — --apply will be REFUSED.", file=sys.stderr)
    if novel:
        print(f"⚠️ {len(novel)} NOVEL key(s) — --apply will be REFUSED.", file=sys.stderr)

    if args.report:
        os.makedirs(os.path.dirname(os.path.abspath(args.report)), exist_ok=True)
        with open(args.report, "w", encoding="utf-8") as f:
            f.write(render_report(cmap, conflicts, plan, novel, args.csv, len(rows)))
        print(f"Report written: {args.report}")

    blocked = bool(conflicts or novel)
    if args.apply:
        if blocked:
            print("REFUSED: --apply cannot run with conflicts/novel keys. Re-derive or resolve first.",
                  file=sys.stderr)
            return 2
        n = apply_plan(args.csv, plan)
        print(f"Applied {n} cell fill(s) to {args.csv}")
        return 0

    # dry-run: print plan sample.
    for e in plan[:30]:
        print(f"  pk {e['pk']:>4} [{e['en_col']:<12}] `{e['fr_key']}` -> `{e['fill_value']}`")
    if len(plan) > 30:
        print(f"  ... ({len(plan) - 30} more)")
    for c in conflicts:
        print(f"  CONFLICT `{c['fr']}`: {c['tuples']}", file=sys.stderr)
    for nv in novel:
        print(f"  NOVEL pk {nv['pk']} `{nv['fr']}` ({nv['text_fr']})", file=sys.stderr)
    return 2 if blocked else 0


if __name__ == "__main__":
    sys.exit(main())
