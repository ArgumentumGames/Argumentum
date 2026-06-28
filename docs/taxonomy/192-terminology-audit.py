#!/usr/bin/env python3
"""#192 — dump FULL variant breakdown for every multi-variant grouping label, all 4 datasets.

For each (dataset, grouping_field, language), maps the FR source label -> Counter of
translations seen across rows. Any label with >1 distinct translation is printed with the
full variant breakdown (variant -> count), classified OBVIOUS (>=80% majority) or
ARBITRARY (<80% majority). Post-#595, OBVIOUS should be ~0; ARBITRARY = the residue
needing glossary/register ratification (jsboige/native).

Rules has no taxonomy grouping (Text x langs only) -> reported N/A.

Output is plain markdown-friendly stdout. NO WRITE to any CSV.
"""
import csv, collections

MAJ = 0.80

# (name, path, [(fr_field, en_base_stem), ...], [target langs])
# target column for (fr_field, lang) = f"{en_base}_{lang}"  (incl. en)
DATASETS = [
    ("Virtues", "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv", [
        ("family_fr", "family"),
        ("subfamily_fr", "subfamily"),
        ("subsubfamily_fr", "subsubfamily"),
    ], ["en", "ru", "pt", "es", "ar", "zh", "fa"]),
    ("Fallacies", "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv", [
        ("Famille", "Family"),
        ("Sous-Famille", "Subfamily"),
        ("Soussousfamille", "Subsubfamily"),
    ], ["en", "ru", "pt", "es", "ar", "zh", "fa"]),
    ("Scenarii", "Cards/Scenarii/Argumentum Scenarii - Cards.csv", [
        ("catégorie", "category"),
        ("sous-catégorie", "subcategory"),
    ], ["en", "ru", "pt", "es", "ar", "zh", "fa"]),
]


def main():
    grand_obvious, grand_arb, grand_cells = 0, 0, 0
    for name, rel, fields, langs in DATASETS:
        print(f"\n{'=' * 78}\n## {name}  ({rel})\n{'=' * 78}")
        with open(rel, encoding="utf-8-sig", newline="") as f:
            rows = list(csv.reader(f))
        header = rows[0]
        idx = {c: i for i, c in enumerate(header)}
        n_obvious = n_arb = n_cells = 0
        for fr_field, en_base in fields:
            if fr_field not in idx:
                print(f"  [{fr_field}] NOT in header — skip")
                continue
            fr_ci = idx[fr_field]
            for lang in langs:
                tcol = f"{en_base}_{lang}"
                if tcol not in idx:
                    continue
                t_ci = idx[tcol]
                fr2cnt = collections.defaultdict(collections.Counter)
                for r in rows[1:]:
                    fr = r[fr_ci].strip() if fr_ci < len(r) else ""
                    t = r[t_ci].strip() if t_ci < len(r) else ""
                    if fr and t:
                        fr2cnt[fr][t] += 1
                for fr_term, cnt in sorted(fr2cnt.items()):
                    if len(cnt) <= 1:
                        continue  # consistent
                    tot = sum(cnt.values())
                    winner, wcount = cnt.most_common(1)[0]
                    ratio = wcount / tot
                    verdict = "OBVIOUS" if ratio >= MAJ else "ARBITRARY"
                    n_to_change = tot - wcount
                    n_cells += n_to_change
                    if verdict == "OBVIOUS":
                        n_obvious += 1
                    else:
                        n_arb += 1
                    # FULL variant dump
                    variants = " · ".join(
                        f'"{v[:40]}"×{c}' for v, c in cnt.most_common())
                    print(f"  [{fr_field}.{lang}] {verdict} "
                          f"({ratio * 100:.0f}% maj, {len(cnt)} variants, "
                          f"{n_to_change} outliers)")
                    print(f'      FR="{fr_term[:55]}"')
                    print(f"      {variants}")
        print(f"\n  >> {name} SUM: {n_obvious} OBVIOUS groups, {n_arb} ARBITRARY groups, "
              f"{n_cells} cells (OBVIOUS outliers would-change)")
        grand_obvious += n_obvious
        grand_arb += n_arb
        grand_cells += n_cells

    print(f"\n{'=' * 78}\n## Rules (Argumentum Rules - Cards.csv)\n{'=' * 78}")
    print("  Text x 8 langs, NO taxonomy grouping field -> terminological-consistency N/A")
    print("  (each rule row is standalone; no shared labels across rows to harmonize)")

    print(f"\n{'=' * 78}\nGRAND TOTAL: {grand_obvious} OBVIOUS, {grand_arb} ARBITRARY "
          f"({grand_obvious + grand_arb} multi-variant groups across Virtues/Fallacies/Scenarii)")
    print("Post-#595 expectation: OBVIOUS ~ 0 (harmonized), ARBITRARY = glossary residue")


if __name__ == "__main__":
    main()
