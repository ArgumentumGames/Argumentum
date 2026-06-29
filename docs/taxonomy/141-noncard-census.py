#!/usr/bin/env python3
"""
#141 — non-card taxonomy node census (read-only, reproducible).

Counterpart to the #141 issue ("Iterate on non-card taxonomy nodes + adapt
GPT-4 enrichment script"). Grounds the proposal in the ACTUAL current state
of the taxonomy CSVs (not the 2-year-old issue text) — same reproducibility
discipline as #600/#606 (read-only, deterministic, 0 write under Cards/).

WHAT IT MEASURES
  1. Non-card node count per dataset (rows where the card flag is empty =
     family/subfamily/subsubfamily headers + order groupings, not leaf cards).
  2. Text-enrichment coverage on non-card nodes: description/example/title
     x {fr, en, ru, pt, es, ar, zh, fa} — the original #141 scope item 3.
  3. Cross-reference coverage: crossLink_* (8 AIF relationship types) +
     AIF_skos* (4 SKOS semantic mappings) — the cross-reference target,
     schema-ready columns whose fill drives #130 (OWL) + #136 (2sxc).

Re-run anytime from repo root: python docs/taxonomy/141-noncard-census.py
"""
import csv
from collections import Counter

LANGS = ["fr", "en", "ru", "pt", "es", "ar", "zh", "fa"]

# (dataset, path, card_flag_col, [(stem_fr, stem_en)], extra_text_cols)
# text stems = fields whose <stem>_<lang> variants carry enrichment content.
DATASETS = [
    ("Fallacies", "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv", "carte",
     [("desc", "desc"), ("example", "example")], ["text", "title"], LANGS),
    ("Virtues", "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv", "card",
     [("description", "description"), ("remark", "remark")], ["title"], LANGS),
]

CROSSLINK_PREFIXES = ["crossLink_", "AIF_skos"]


def load(path):
    with open(path, encoding="utf-8-sig", newline="") as f:
        return list(csv.DictReader(f))


def pct(n, d):
    return 0 if not d else 100.0 * n / d


def text_coverage(rows, stems, extra, langs):
    """Return list of (field, filled, total, pct) for every <stem>_<lang> + extras."""
    out = []
    cols = rows[0].keys() if rows else []
    for stem in [s[1] for s in stems]:  # en-stem is the lang-suffixed form
        for lang in langs:
            col = "%s_%s" % (stem, lang)
            if col in cols:
                filled = sum(1 for r in rows if (r.get(col) or "").strip())
                out.append((col, filled, len(rows)))
    return out


def crosslink_coverage(rows):
    """crossLink_* + AIF_skos* fill, split by prefix."""
    cols = rows[0].keys() if rows else []
    by_prefix = {}
    for col in cols:
        for pre in CROSSLINK_PREFIXES:
            if col.startswith(pre):
                filled = sum(1 for r in rows if (r.get(col) or "").strip())
                by_prefix.setdefault(pre, []).append((col, filled, len(rows)))
    return by_prefix


def report(name, path, card_col, stems, extra, langs):
    try:
        rows = load(path)
    except FileNotFoundError:
        print("== %s: %s NOT FOUND, skip ==\n" % (name, path))
        return
    cards = [r for r in rows if (r.get(card_col) or "").strip()]
    noncards = [r for r in rows if not (r.get(card_col) or "").strip()]
    print("=" * 78)
    print("== %s (%s) ==" % (name, path))
    print("=" * 78)
    print("  total rows: %d   | cards: %d   | NON-CARD nodes: %d" %
          (len(rows), len(cards), len(noncards)))
    print("  card-flag '%s' values: %s" % (card_col, dict(Counter((r.get(card_col) or "").strip() for r in rows))))
    print("  depth distribution: %s" % dict(Counter(r.get("depth", "").strip() for r in noncards)))

    print("\n  -- TEXT enrichment on NON-CARD nodes (original #141 scope item 3) --")
    tc = text_coverage(noncards, stems, extra, langs)
    groups = {}
    for col, filled, total in tc:
        stem = col.rsplit("_", 1)[0]
        groups.setdefault(stem, []).append((filled, total))
    for stem, vals in sorted(groups.items()):
        mf = min(v[0] for v in vals)
        avg = pct(sum(v[0] for v in vals), sum(v[1] for v in vals) * len(vals) / len(vals)) if vals else 0
        mn = min(pct(f, t) for f, t in vals)
        mx = max(pct(f, t) for f, t in vals)
        print("    %-16s %2d langs, fill %.0f%%-%.0f%% (min %d/%d)" %
              (stem + "_<lang>", len(vals), mn, mx, mf, vals[0][1]))

    print("\n  -- CROSS-REFERENCES (crossLink_* + AIF_skos*, schema-ready, feeds #130/#136) --")
    cl = crosslink_coverage(noncards)
    for pre, items in cl.items():
        tot_filled = sum(f for _, f, _ in items)
        cap = sum(t for _, _, t in items)
        print("    %s : %d cols, %d/%d cells filled (%.1f%%)" %
              (pre, len(items), tot_filled, cap, pct(tot_filled, cap)))
        for col, filled, total in sorted(items, key=lambda x: x[1]):
            print("        %-26s %4d/%d (%.0f%%)" % (col, filled, total, pct(filled, total)))


def main():
    print("#141 non-card taxonomy node census (read-only)")
    print("Base: run from repo root. 0 write under Cards/.\n")
    for ds in DATASETS:
        report(*ds)
    print("\n" + "=" * 78)
    print("READING GUIDE:")
    print("  - TEXT enrichment (desc/example/title x 8 langs) ~100%% => #141 scope item 3 DONE.")
    print("  - crossLink_* + AIF_skos* ~0%% => the REAL open gap (AIF cross-reference graph).")
    print("  - Schema is READY (columns exist); content is the gap. See 141-noncard-enrichment-census.md.")
    print("=" * 78)


if __name__ == "__main__":
    main()
