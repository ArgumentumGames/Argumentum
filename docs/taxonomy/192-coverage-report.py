#!/usr/bin/env python3
"""#192 secondary — TranslationCoverageReport (READ-ONLY).
Per dataset, per translatable text field, per language: filled/total + empty count.
Also reports link_* URL coverage (the known i18n gap per memory i18n-coverage-gap-is-link-urls).
NO WRITE to any CSV. Measure only.
"""
import csv

LANGS = ["en", "ru", "pt", "es", "ar", "zh", "fa"]

# (name, path, {text_field_base: [lang cols]}, [link cols])
DATASETS = [
    ("Virtues", "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv",
     {"title": ["title"], "description": ["description"], "remark": ["remark"]},
     [("link", "link_fr", [f"link_{l}" for l in ["en", "ru", "pt", "ar", "es", "zh", "fa"]])]),
    ("Fallacies", "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv",
     {"text": ["text"], "desc": ["desc"], "example": ["example"]},
     [("link", "link_fr", [f"link_{l}" for l in ["en", "ru", "pt", "ar", "es", "zh", "fa"]])]),
    ("Scenarii", "Cards/Scenarii/Argumentum Scenarii - Cards.csv",
     {"context": ["context"], "issue": ["issue"], "suggestion": ["suggestion", "suggestion_en"],
      "title": ["title"], "smoothTalker": ["smoothTalker"], "drawer": ["drawer"]},
     []),
    ("Rules", "Cards/Rules/Argumentum Rules - Cards.csv",
     {"Text": ["Text"]},
     []),
]


def col_map(field_base, lang, bases):
    """Return actual column name for field_base in language lang.
    bases = the list of col-stems for this field (e.g. ['text'] or ['suggestion','suggestion_en'])."""
    # try stem_<lang>, then for en try bare stem
    for stem in bases:
        for cand in (f"{stem}_{lang}", stem if lang == "en" else None):
            if cand and cand in HEADER_IDX:
                return cand
    return None


for name, rel, fields, links in DATASETS:
    with open(rel, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.reader(f))
    header = rows[0]
    HEADER_IDX = {c: i for i, c in enumerate(header)}
    data = rows[1:]
    total_rows = len(data)
    print(f"\n{'=' * 70}\n## {name}  ({total_rows} rows)\n{'=' * 70}")
    for fbase, stems in fields.items():
        line = f"  {fbase:<12} "
        parts = []
        for lang in LANGS:
            col = None
            for stem in stems:
                cands = [f"{stem}_{lang}"]
                if lang == "en":
                    cands.append(stem)
                for c in cands:
                    if c in HEADER_IDX:
                        col = c
                        break
                if col:
                    break
            if not col:
                parts.append(f"{lang}:(col?)")
                continue
            ci = HEADER_IDX[col]
            filled = sum(1 for r in data if ci < len(r) and r[ci].strip())
            pct = 100.0 * filled / total_rows if total_rows else 0
            parts.append(f"{lang}:{filled}/{total_rows}({pct:.0f}%)")
        print(line + " | ".join(parts))
    for label, fr_col, tgt_cols in links:
        line = f"  link{'_'+label if label!='link' else '':<8} "
        parts = []
        if fr_col in HEADER_IDX:
            ci = HEADER_IDX[fr_col]
            filled = sum(1 for r in data if ci < len(r) and r[ci].strip())
            parts.append(f"fr:{filled}/{total_rows}")
        for lang, col in zip(LANGS, tgt_cols):
            if col in HEADER_IDX:
                ci = HEADER_IDX[col]
                filled = sum(1 for r in data if ci < len(r) and r[ci].strip())
                pct = 100.0 * filled / total_rows if total_rows else 0
                parts.append(f"{lang}:{filled}({pct:.0f}%)")
        print(line + " | ".join(parts))
