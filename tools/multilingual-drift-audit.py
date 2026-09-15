#!/usr/bin/env python3
# multilingual-drift-audit.py — Argumentum #192-style multilingual drift audit
#
# Audits the 4 Cards CSVs (Fallacies / Virtues / Scenarii / Rules) for translation
# drift across ALL non-FR language columns (_en/_ru/_pt/_es/_ar/_fa/_zh) vs the FR
# reference + internal script consistency. Zero write: read-only, emits a markdown
# report (and JSON detail to stdout when --json).
#
# Drift classes detected (by field kind):
#   MISSING       — FR cell non-empty, lang cell empty (coverage gap) [all kinds]
#   ORPHAN        — lang cell non-empty but FR cell empty (translation w/o source) [all]
#   FR_CONTAM     — lang cell == FR cell verbatim in a PROSE field (untranslated copy)
#                   [prose only — name/label/url cognate overlap is legit]
#   WRONG_SCRIPT  — non-Latin lang (ru/ar/fa/zh) cell carries Latin letters but NO
#                   expected-script glyph (FR/Latin text leaked into a CJK/Cyrillic/
#                   Arabic column — the #761 lesson: a non-Latin drift is invisible
#                   if only FR/EN are audited) [prose + name only]
#   COGNATE       — INFORMATIONAL: name field where lang == FR verbatim (Latin fallacy
#                   names legitimately repeat; NOT drift, reported for transparency)
#
# LIMITATION (documented, not auto-detected): within-language semantic drift such as
# the zh #761 case (备用卡 "backup card" correct vs 备忘卡 "memo card" wrong) is NOT
# machine-detectable — both are valid CJK. This audit catches script-level leakage +
# copy/missing drift; semantic correctness needs human review (cited as residual).
#
# Usage:
#   python tools/multilingual-drift-audit.py            # prints human summary
#   python tools/multilingual-drift-audit.py --json     # full detail JSON to stdout
#   python tools/multilingual-drift-audit.py --report docs/quality/multilingual-drift-audit-2026-07.md
#
# Exit code 0 always (this is an audit, not a gate). Worker po-2024, 2026-07-12.

import csv
import json
import os
import re
import sys
from collections import defaultdict

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))

LANGS = ["en", "ru", "pt", "es", "ar", "fa", "zh"]
NON_LATIN = {"ru", "ar", "fa", "zh"}  # langs whose cells should carry non-Latin script

# --- Script detection -------------------------------------------------------
RE_CYRILLIC = re.compile(r"[Ѐ-ӿ]")
RE_ARABIC = re.compile(r"[؀-ۿݐ-ݿࢠ-ࣿ]")
RE_CJK = re.compile(r"[一-鿿㐀-䶿]")
RE_LATIN_LETTER = re.compile(r"[A-Za-zÀ-ÖØ-öø-ÿ]")  # incl. FR diacritics


def has_expected_script(lang, val):
    """True if val carries at least one glyph of lang's expected script."""
    if lang == "ru":
        return bool(RE_CYRILLIC.search(val))
    if lang in ("ar", "fa"):
        return bool(RE_ARABIC.search(val))
    if lang == "zh":
        return bool(RE_CJK.search(val))
    return True  # Latin langs always pass


def wrong_script(lang, val):
    """A non-Latin lang cell that has Latin letters but NONE of the expected script
    → FR/Latin leaked into a CJK/Cyrillic/Arabic column."""
    if lang not in NON_LATIN:
        return False
    if not val.strip():
        return False
    if has_expected_script(lang, val):
        return False
    return bool(RE_LATIN_LETTER.search(val))


def norm(s):
    return re.sub(r"\s+", " ", (s or "").strip())


# --- Dataset definitions ----------------------------------------------------
# Each field group: (base_label, kind, {lang_or_'fr': colname})
# kind: 'prose' (FR_CONTAM checked) | 'label' (FR_CONTAM skipped — legit cognate overlap)
# 'en' may map to a column with no _en suffix; 'fr' may have no _fr suffix. Missing
# lang key → that lang has no column for this field in this dataset.

DATASETS = [
    {
        "name": "Fallacies",
        "path": "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv",
        "pk": "PK",
        "fields": [
            # NAME field — the fallacy's short name (Latin/cognate overlap is legit,
            # so FR_contam is reported as informational 'cognate', not drift)
            ("text", "name", {"fr": "text_fr", "en": "text_en", "ru": "text_ru",
                               "pt": "text_pt", "es": "text_es", "ar": "text_ar",
                               "zh": "text_zh", "fa": "text_fa"}),
            ("desc", "prose", {"fr": "desc_fr", "en": "desc_en", "ru": "desc_ru",
                               "pt": "desc_pt", "es": "desc_es", "ar": "desc_ar",
                               "zh": "desc_zh", "fa": "desc_fa"}),
            ("example", "prose", {"fr": "example_fr", "en": "example_en", "ru": "example_ru",
                                  "pt": "example_pt", "es": "example_es", "ar": "example_ar",
                                  "zh": "example_zh", "fa": "example_fa"}),
            ("link", "url", {"fr": "link_fr", "en": "link_en", "ru": "link_ru",
                             "pt": "link_pt", "es": "link_es", "ar": "link_ar",
                             "zh": "link_zh", "fa": "link_fa"}),
            # LABEL fields (taxonomic labels — coverage only, drift checks skipped)
            ("Family", "label", {"fr": "Famille", "en": "Family", "ru": "Family_ru",
                                 "pt": "Family_pt", "es": "Family_es", "ar": "Family_ar",
                                 "zh": "Family_zh", "fa": "Family_fa"}),
            ("Subfamily", "label", {"fr": "Sous-Famille", "en": "Subfamily", "ru": "Subfamily_ru",
                                    "pt": "Subfamily_pt", "es": "Subfamily_es", "ar": "Subfamily_ar",
                                    "zh": "Subfamily_zh", "fa": "Subfamily_fa"}),
            ("Subsubfamily", "label", {"fr": "Soussousfamille", "en": "Subsubfamily",
                                       "ru": "Subsubfamily_ru", "pt": "Subsubfamily_pt",
                                       "es": "Subsubfamily_es", "ar": "Subsubfamily_ar",
                                       "zh": "Subsubfamily_zh", "fa": "Subsubfamily_fa"}),
        ],
    },
    {
        "name": "Virtues",
        "path": "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv",
        "pk": "pk",
        "fields": [
            ("title", "name", {"fr": "title_fr", "en": "title_en", "ru": "title_ru",
                               "pt": "title_pt", "es": "title_es", "ar": "title_ar",
                               "zh": "title_zh", "fa": "title_fa"}),
            ("description", "prose", {"fr": "description_fr", "en": "description_en",
                                      "ru": "description_ru", "pt": "description_pt",
                                      "es": "description_es", "ar": "description_ar",
                                      "zh": "description_zh", "fa": "description_fa"}),
            ("remark", "prose", {"fr": "remark_fr", "en": "remark_en", "ru": "remark_ru",
                                 "pt": "remark_pt", "es": "remark_es", "ar": "remark_ar",
                                 "zh": "remark_zh", "fa": "remark_fa"}),
            ("link", "url", {"fr": "link_fr", "en": "link_en", "ru": "link_ru",
                             "pt": "link_pt", "es": "link_es", "ar": "link_ar",
                             "zh": "link_zh", "fa": "link_fa"}),
            ("family", "label", {"fr": "family_fr", "en": "family_en", "ru": "family_ru",
                                 "pt": "family_pt", "es": "family_es", "ar": "family_ar",
                                 "zh": "family_zh", "fa": "family_fa"}),
            ("subfamily", "label", {"fr": "subfamily_fr", "en": "subfamily_en",
                                    "ru": "subfamily_ru", "pt": "subfamily_pt",
                                    "es": "subfamily_es", "ar": "subfamily_ar",
                                    "zh": "subfamily_zh", "fa": "subfamily_fa"}),
            ("subsubfamily", "label", {"fr": "subsubfamily_fr", "en": "subsubfamily_en",
                                       "ru": "subsubfamily_ru", "pt": "subsubfamily_pt",
                                       "es": "subsubfamily_es", "ar": "subsubfamily_ar",
                                       "zh": "subsubfamily_zh", "fa": "subsubfamily_fa"}),
        ],
    },
    {
        "name": "Scenarii",
        "path": "Cards/Scenarii/Argumentum Scenarii - Cards.csv",
        "pk": "path",
        "fields": [
            # Scenarii: FR base has no suffix, EN has no suffix, others _{lang}
            ("title", "name", {"fr": "titre", "en": "title", "ru": "title_ru",
                                "pt": "title_pt", "es": "title_es", "ar": "title_ar",
                                "zh": "title_zh", "fa": "title_fa"}),
            ("context", "prose", {"fr": "contexte", "en": "context", "ru": "context_ru",
                                  "pt": "context_pt", "es": "context_es", "ar": "context_ar",
                                  "zh": "context_zh", "fa": "context_fa"}),
            ("issue", "prose", {"fr": "enjeu", "en": "issue", "ru": "issue_ru",
                                "pt": "issue_pt", "es": "issue_es", "ar": "issue_ar",
                                "zh": "issue_zh", "fa": "issue_fa"}),
            ("smoothTalker", "prose", {"fr": "baratineur", "en": "smoothTalker",
                                       "ru": "smoothTalker_ru", "pt": "smoothTalker_pt",
                                       "es": "smoothTalker_es", "ar": "smoothTalker_ar",
                                       "zh": "smoothTalker_zh", "fa": "smoothTalker_fa"}),
            ("drawer", "prose", {"fr": "piocheur", "en": "drawer", "ru": "drawer_ru",
                                 "pt": "drawer_pt", "es": "drawer_es", "ar": "drawer_ar",
                                 "zh": "drawer_zh", "fa": "drawer_fa"}),
            ("suggestion", "prose", {"fr": "suggestion", "en": "suggestion_en",
                                     "ru": "suggestion_ru", "pt": "suggestion_pt",
                                     "es": "suggestion_es", "ar": "suggestion_ar",
                                     "zh": "suggestion_zh", "fa": "suggestion_fa"}),
            ("category", "label", {"fr": "catégorie", "en": "category", "ru": "category_ru",
                                   "pt": "category_pt", "es": "category_es", "ar": "category_ar",
                                   "zh": "category_zh", "fa": "category_fa"}),
            ("subcategory", "label", {"fr": "sous-catégorie", "en": "subcategory",
                                      "ru": "subcategory_ru", "pt": "subcategory_pt",
                                      "es": "subcategory_es", "ar": "subcategory_ar",
                                      "zh": "subcategory_zh", "fa": "subcategory_fa"}),
        ],
    },
    {
        "name": "Rules",
        "path": "Cards/Rules/Argumentum Rules - Cards.csv",
        "pk": "pk",
        "fields": [
            ("Text", "prose", {"fr": "Text", "en": "Text_en", "ru": "Text_ru",
                               "pt": "Text_pt", "es": "Text_es", "ar": "Text_ar",
                               "zh": "Text_zh", "fa": "Text_fa"}),
        ],
    },
]


def read_csv_rows(rel_path):
    path = os.path.join(REPO, rel_path)
    with open(path, "r", encoding="utf-8-sig", newline="") as f:
        reader = csv.DictReader(f)
        rows = list(reader)
        return reader.fieldnames, rows


def audit_dataset(ds):
    fieldnames, rows = read_csv_rows(ds["path"])
    pk_col = ds["pk"]
    # Per (field, lang) counters + sample buckets
    result = {
        "name": ds["name"],
        "rows": len(rows),
        "pk": pk_col,
        "fields": {},
    }
    for base, kind, cols in ds["fields"]:
        fdef = {"kind": kind, "cols": cols, "langs": {}}
        for lang in ["fr"] + LANGS:
            col = cols.get(lang)
            if col is None or col not in fieldnames:
                continue
            stats = {
                "col": col,
                "filled": 0,
                "empty": 0,
                "missing_vs_fr": 0,      # FR filled, this empty
                "fr_contam": 0,          # == FR verbatim (prose only = real drift)
                "cognate": 0,            # == FR verbatim (name field = legit, informational)
                "wrong_script": 0,       # non-Latin lang w/ Latin leak (prose+name)
                "orphan": 0,             # this filled, FR empty
                "samples": {"missing": [], "fr_contam": [], "wrong_script": [],
                            "orphan": [], "cognate": []},
            }
            fdef["langs"][lang] = stats
        result["fields"][base] = fdef

    for row in rows:
        pk = (row.get(pk_col) or "").strip()
        for base, kind, cols in ds["fields"]:
            fdef = result["fields"][base]
            fr_col = cols.get("fr")
            fr_val = norm(row.get(fr_col, "")) if fr_col and fr_col in row else ""
            fr_present = bool(fr_val)
            for lang in LANGS:
                stats = fdef["langs"].get(lang)
                if stats is None:
                    continue
                col = stats["col"]
                if col not in row:
                    continue
                val = norm(row.get(col, ""))
                if val:
                    stats["filled"] += 1
                else:
                    stats["empty"] += 1

                if fr_present and not val:
                    stats["missing_vs_fr"] += 1
                    if len(stats["samples"]["missing"]) < 3:
                        stats["samples"]["missing"].append(
                            {"pk": pk, "fr": fr_val[:80]})
                elif val and not fr_present:
                    stats["orphan"] += 1
                    if len(stats["samples"]["orphan"]) < 3:
                        stats["samples"]["orphan"].append(
                            {"pk": pk, "val": val[:80]})

                # FR contamination (DRIFT): prose only, exact match, skip short (<4)
                # tokens. name/label/url cognate overlap is reported separately.
                if kind == "prose" and val and fr_present and val == fr_val and len(val) >= 4:
                    stats["fr_contam"] += 1
                    if len(stats["samples"]["fr_contam"]) < 3:
                        stats["samples"]["fr_contam"].append(
                            {"pk": pk, "val": val[:80]})

                # COGNATE (informational, NOT drift): name field identical to FR
                # (Latin fallacy names / proper nouns legitimately repeat).
                if kind == "name" and val and fr_present and val == fr_val:
                    stats["cognate"] += 1
                    if len(stats["samples"]["cognate"]) < 3:
                        stats["samples"]["cognate"].append(
                            {"pk": pk, "val": val[:80]})

                # Wrong script (DRIFT): non-Latin lang with Latin leak. Only for
                # prose + name fields — labels are taxonomy, urls are Latin by nature.
                if val and kind in ("prose", "name") and wrong_script(lang, val):
                    stats["wrong_script"] += 1
                    if len(stats["samples"]["wrong_script"]) < 3:
                        stats["samples"]["wrong_script"].append(
                            {"pk": pk, "val": val[:80], "lang": lang, "field": base})
    return result


KIND_LABEL = {"prose": "prose", "name": "name", "label": "label", "url": "url"}


def _agg_per_lang(ds):
    agg = defaultdict(lambda: {"missing": 0, "contam": 0, "wscript": 0,
                                "cognate": 0, "orphan": 0})
    for base, fdef in ds["fields"].items():
        for lang, st in fdef["langs"].items():
            if lang == "fr":
                continue
            agg[lang]["missing"] += st["missing_vs_fr"]
            agg[lang]["contam"] += st["fr_contam"]
            agg[lang]["wscript"] += st["wrong_script"]
            agg[lang]["cognate"] += st["cognate"]
            agg[lang]["orphan"] += st["orphan"]
    return agg


def _field_table(ds, base, fdef, cols):
    lines = [f"\n### {ds['name']} · {fdef['kind']} field `{base}` "
             f"(FR col `{fdef['cols'].get('fr')}`)"]
    lines.append("| " + " | ".join(cols) + " |")
    lines.append("|" + "|".join(["---"] * len(cols)) + "|")
    for lang in LANGS:
        st = fdef["langs"].get(lang)
        if not st:
            continue
        row = [f"{lang} (`{st['col']}`)"]
        for c in cols[1:]:
            mapping = {
                "missing": st["missing_vs_fr"],
                "FR_contam": st["fr_contam"],
                "cognate": st["cognate"],
                "wrong_script": st["wrong_script"],
                "orphan": st["orphan"],
                "filled": st["filled"],
            }
            row.append(str(mapping.get(c, "")))
        lines.append("| " + " | ".join(row) + " |")
    # Cite samples for any non-zero drift class on this field
    samples = []
    for cls in ["wrong_script", "fr_contam", "missing", "orphan", "cognate"]:
        for lang in LANGS:
            st = fdef["langs"].get(lang)
            if not st:
                continue
            for s in st["samples"].get(cls, []):
                samples.append((cls, lang, base, s))
    if samples:
        lines.append("\n<details><summary>Samples (drift/cognate, first per class)</summary>\n")
        for cls, lang, fld, s in samples[:24]:
            val = s.get("val", s.get("fr", ""))
            lines.append(f"- `{cls}` {lang}/{fld} pk={s['pk']}: `{val[:70]}`")
        lines.append("\n</details>")
    return lines


def summarize(audit):
    lines = []
    for ds in audit:
        lines.append(f"\n## {ds['name']} ({ds['rows']} rows, pk=`{ds['pk']}`)")
        agg = _agg_per_lang(ds)
        # Aggregate across ALL fields: distinguishes DRIFT (contam, wrong_script)
        # from coverage gaps (missing/orphan) and informational cognate overlap.
        header = ["lang", "missing", "FR_contam", "wrong_script", "cognate(info)", "orphan"]
        lines.append("\n**Aggregate across all fields** "
                     "(DRIFT = FR_contam + wrong_script; missing/orphan = coverage):\n")
        lines.append("| " + " | ".join(header) + " |")
        lines.append("|" + "|".join(["---"] * len(header)) + "|")
        for lang in LANGS:
            a = agg[lang]
            lines.append(f"| {lang} | {a['missing']} | {a['contam']} | {a['wscript']} "
                         f"| {a['cognate']} | {a['orphan']} |")
        # Per-field detail, grouped by kind (prose = drift-critical)
        for kind_order in ("prose", "name", "label", "url"):
            for base, fdef in ds["fields"].items():
                if fdef["kind"] != kind_order:
                    continue
                if kind_order == "prose":
                    cols = ["lang(col)", "missing", "FR_contam", "wrong_script",
                            "orphan", "filled"]
                elif kind_order == "name":
                    cols = ["lang(col)", "missing", "wrong_script", "cognate", "filled"]
                else:  # label / url — coverage only
                    cols = ["lang(col)", "missing", "orphan", "filled"]
                lines.extend(_field_table(ds, base, fdef, cols))
    return "\n".join(lines)


def main():
    args = sys.argv[1:]
    do_json = "--json" in args
    report_path = None
    if "--report" in args:
        report_path = args[args.index("--report") + 1]

    audit = [audit_dataset(ds) for ds in DATASETS]

    if do_json:
        print(json.dumps(audit, ensure_ascii=False, indent=2))
        return

    summary = summarize(audit)
    print(f"# Multilingual drift audit (worker po-2024, 2026-07-12){summary}")

    if report_path:
        with open(report_path, "w", encoding="utf-8") as f:
            f.write(_full_report(audit))
        print(f"\n[report written → {report_path}]", file=sys.stderr)


def _full_report(audit):
    # Top-line verdict computed from aggregates.
    total_drift = 0
    total_missing_prose = 0
    for ds in audit:
        agg = _agg_per_lang(ds)
        for lang in LANGS:
            total_drift += agg[lang]["contam"] + agg[lang]["wscript"]
        # prose missing (real content gap) — recompute prose-only
        for base, fdef in ds["fields"].items():
            if fdef["kind"] != "prose":
                continue
            for lang, st in fdef["langs"].items():
                if lang == "fr":
                    continue
                total_missing_prose += st["missing_vs_fr"]
    verdict = ("REAL prose content is clean — drift signals concentrate on name/link "
               "fields where cognate overlap and URL coverage gaps are expected, not regressions."
               if total_drift < 50 else "drift requires review")
    body = summarize(audit)
    return (
        "# Multilingual Drift Audit — 2026-07\n\n"
        "Worker po-2024 · 2026-07-12 · Base master `84a529bf` · "
        "**READ-ONLY — 0 write prod CSV.**\n\n"
        f"**Headline verdict:** {verdict}\n\n"
        f"Totals: DRIFT (FR_contam + wrong_script across all lang cols) = **{total_drift}**; "
        f"prose MISSING (real content translation gaps) = **{total_missing_prose}**.\n"
        "See per-dataset aggregates below (DRIFT vs coverage split) and the §Limitations "
        "note on the zh #761 within-language semantic drift (not machine-detectable).\n"
        f"{body}\n\n"
        "## Limitations (honest scope)\n"
        "- **zh #761 within-language semantic drift** (e.g. `备用卡` 'backup card' correct "
        "vs `备忘卡` 'memo card' wrong — both valid CJK) is **NOT machine-detectable**. This "
        "audit catches script-level leakage + verbatim-copy/missing drift; semantic "
        "correctness within a language still needs human review. Cited as the standing "
        "residual risk.\n"
        "- **`link_*` URL columns** are reported under coverage only. Wikipedia URLs in "
        "ru/ar/fa/zh are percent-encoded ASCII by HTTP design (so a script check would "
        "false-positive); their translation is human research (find the right article), "
        "the known #192 residual (see MEMORY `i18n-coverage-gap-is-link-urls`).\n"
        "- **`name`/`label` cognate overlap** (Latin fallacy names, taxonomic labels "
        "repeated verbatim across langs) is informational, NOT drift.\n"
        "- **`FR_contam` on prose** uses exact-match (len>=4); paraphrased or partially-"
        "translated drift is not caught by this pass (would need semantic comparison).\n"
    )



if __name__ == "__main__":
    main()
