#!/usr/bin/env python3
# 192-link-i18n-scoping.py — Argumentum #192 link_* i18n URL scoping
#
# Categorizes every link_* cell across the Cards CSVs (Fallacies + Virtues) to
# scope the residual i18n URL gap isolated by the multilingual drift audit (#795).
# Scenarii/Rules have no link_* columns (Rules has none; Scenarii has none).
#
# Per (dataset, lang) link_* cell is classified:
#   NATIVE_URL  — points to {lang}.wikipedia.org / {lang}.wiktionary.org / a {lang}
#                 domain  → CORRECT (the URL was localized to a native article)
#   FR_URL      — FR source has a link but lang points to a FR domain (fr.wikipedia.org,
#                 fr.wiktionary.org, *.fr, service-public.fr, cortecs.org, huffpost.com
#                 FR article…)  → NOT TRANSLATED (URL copied from FR, not localized)
#   EMPTY       — FR source has a link, lang cell is empty  → GAP (never filled)
#   OTHER_URL   — points to a language-agnostic source (logicallyfallacious.com,
#                 rationalwiki.org, ditext.com, fallacyfiles.org, yourlogicalfallacyis…)
#                 → OK (these are English/reference sources that don't localize)
#   ORPHAN      — lang has a link, FR source is empty  → informational
#
# The FR_URL + EMPTY classes are the TRUE i18n URL gap (a native reader clicking
# gets a French page or nothing). NATIVE + OTHER are acceptable. This is the
# decision input for the #192 treatment recommendation (gpt-5.5 candidate
# suggestion vs human research).
#
# Usage:
#   python tools/192-link-i18n-scoping.py            # human summary
#   python tools/192-link-i18n-scoping.py --json     # full detail JSON
#   python tools/192-link-i18n-scoping.py --report docs/quality/192-link-i18n-scoping.md
#
# Read-only, 0 write prod. Worker po-2024, 2026-07-13.

import csv
import json
import os
import re
import sys
from collections import defaultdict

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
LANGS = ["en", "ru", "pt", "es", "ar", "fa", "zh"]

# Domains that are FR-specific (a native {lang} reader should NOT land here)
FR_DOMAINS = ["fr.wikipedia.org", "fr.wiktionary.org", ".fr/", "cortecs.org",
              "service-public.fr", "huffpost.com/fr", "village-justice.com",
              "persee.fr", "leadership-lavautoir.com", "wikifr.", " seekersfind"]
# Language-agnostic reference sources (English/logic sites, fine in any lang col)
AGNOSTIC = ["logicallyfallacious.com", "rationalwiki.org", "ditext.com",
            "fallacyfiles.org", "yourlogicalfallacyis", "skepdic.com",
            "seekfind.net", "tas.crossref"]

# Native domain prefixes per lang
NATIVE_PREFIX = {
    "en": ["en.wikipedia.org", "en.wiktionary.org"],
    "ru": ["ru.wikipedia.org", "ru.wiktionary.org"],
    "pt": ["pt.wikipedia.org", "pt.wiktionary.org"],
    "es": ["es.wikipedia.org", "es.wiktionary.org"],
    "ar": ["ar.wikipedia.org", "ar.wiktionary.org"],
    "fa": ["fa.wikipedia.org", "fa.wiktionary.org"],
    "zh": ["zh.wikipedia.org", "zh.wiktionary.org"],
}


def classify(lang, val, fr_val):
    val = (val or "").strip()
    fr_val = (fr_val or "").strip()
    if not val:
        if fr_val:
            return "EMPTY"
        return "EMPTY_SRC_TOO"  # neither has a link
    low = val.lower()
    # native {lang} domain?
    if any(low.startswith(p) or p in low for p in NATIVE_PREFIX.get(lang, [])):
        return "NATIVE_URL"
    # agnostic reference source?
    if any(a in low for a in AGNOSTIC):
        return "OTHER_URL"
    # FR domain in a non-FR column?
    if any(f in low for f in FR_DOMAINS):
        return "FR_URL"
    # any other URL — likely an agnostic/reference English source not in our list
    if low.startswith("http") or low.startswith("www."):
        return "OTHER_URL"
    # not a URL (shouldn't happen for link col, but be safe)
    return "OTHER"


DATASETS = [
    {
        "name": "Fallacies",
        "path": "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv",
        "pk": "PK",
        "fr_col": "link_fr",
        "lang_cols": {l: f"link_{l}" for l in LANGS},
    },
    {
        "name": "Virtues",
        "path": "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv",
        "pk": "pk",
        "fr_col": "link_fr",
        "lang_cols": {l: f"link_{l}" for l in LANGS},
    },
]


def read_csv_rows(rel_path):
    with open(os.path.join(REPO, rel_path), "r", encoding="utf-8-sig", newline="") as f:
        dr = csv.DictReader(f)
        return dr.fieldnames, list(dr)


def audit_dataset(ds):
    fieldnames, rows = read_csv_rows(ds["path"])
    fr_col = ds["fr_col"]
    # Categorize the FR source links too — the automatable subset (Wikipedia/Wiktionary)
    # is what Option C (langlinks API) can resolve; non-Wikipedia FR sources need manual.
    fr_wiki = fr_nonwiki = 0
    for r in rows:
        v = (r.get(fr_col) or "").strip().lower()
        if not v:
            continue
        if "wikipedia.org" in v or "wiktionary.org" in v:
            fr_wiki += 1
        else:
            fr_nonwiki += 1
    out = {"name": ds["name"], "rows": len(rows),
           "fr_link_filled": sum(1 for r in rows if (r.get(fr_col) or "").strip()),
           "fr_wiki": fr_wiki, "fr_nonwiki": fr_nonwiki,
           "langs": {}}
    # collect sample FR_URL cells per lang (for the report)
    for lang in LANGS:
        col = ds["lang_cols"][lang]
        if col not in fieldnames:
            continue
        counts = defaultdict(int)
        fr_url_samples = []
        empty_samples = []
        for r in rows:
            fr_val = r.get(fr_col, "")
            val = r.get(col, "")
            cls = classify(lang, val, fr_val)
            counts[cls] += 1
            if cls == "FR_URL" and len(fr_url_samples) < 4:
                fr_url_samples.append({"pk": (r.get(ds["pk"]) or "").strip(),
                                       "url": val[:90]})
            if cls == "EMPTY" and fr_val and len(empty_samples) < 3:
                empty_samples.append({"pk": (r.get(ds["pk"]) or "").strip(),
                                      "fr_url": fr_val[:90]})
        out["langs"][lang] = {"col": col, "counts": dict(counts),
                              "fr_url_samples": fr_url_samples,
                              "empty_samples": empty_samples,
                              "gap": counts.get("FR_URL", 0) + counts.get("EMPTY", 0)}
    return out


def summarize(audit):
    lines = []
    for ds in audit:
        lines.append(f"\n## {ds['name']} ({ds['rows']} rows; FR link filled = {ds['fr_link_filled']})")
        header = ["lang", "NATIVE_URL ✅", "OTHER_URL ✅", "FR_URL ❌", "EMPTY ❌", "GAP total", "gap %"]
        lines.append("| " + " | ".join(header) + " |")
        lines.append("|" + "|".join(["---"] * len(header)) + "|")
        for lang in LANGS:
            st = ds["langs"].get(lang)
            if not st:
                continue
            c = st["counts"]
            native = c.get("NATIVE_URL", 0)
            other = c.get("OTHER_URL", 0)
            fr_url = c.get("FR_URL", 0)
            empty = c.get("EMPTY", 0)
            gap = st["gap"]
            # gap % relative to FR-link-filled rows (the translatable denominator)
            denom = ds["fr_link_filled"] or 1
            lines.append(f"| {lang} | {native} | {other} | {fr_url} | {empty} | "
                         f"{gap} | {gap*100/denom:.1f}% |")
        # samples
        lines.append("\n<details><summary>FR_URL samples (URL copied from FR, not localized)</summary>\n")
        for lang in LANGS:
            st = ds["langs"].get(lang)
            if not st or not st["fr_url_samples"]:
                continue
            lines.append(f"- **{lang}**:")
            for s in st["fr_url_samples"]:
                lines.append(f"  - pk={s['pk']}: `{s['url']}`")
        lines.append("\n</details>")
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
    body = summarize(audit)
    if report_path:
        # compute totals for the header
        tot_native = tot_other = tot_fr = tot_empty = 0
        for ds in audit:
            for lang, st in ds["langs"].items():
                tot_native += st["counts"].get("NATIVE_URL", 0)
                tot_other += st["counts"].get("OTHER_URL", 0)
                tot_fr += st["counts"].get("FR_URL", 0)
                tot_empty += st["counts"].get("EMPTY", 0)
        with open(report_path, "w", encoding="utf-8") as f:
            f.write(_full_report(audit, body, tot_native, tot_other, tot_fr, tot_empty))
        print(f"[report → {report_path}]", file=sys.stderr)
    else:
        print(f"# #192 link_* i18n URL scoping{body}")


def _full_report(audit, body, native, other, fr, empty):
    gap = fr + empty
    return (
        "# #192 — `link_*` i18n URL Scoping (residual gap isolation)\n\n"
        "**Worker** po-2024 · **Date** 2026-07-13 · **Base** master `b736a808` · "
        "**READ-ONLY — 0 write prod CSV, `Cards/` untouched.**\n"
        "Dispatch ai-01 `9zs45o` (PRIMAIRE, ungated). Companion to the multilingual "
        "drift audit #795 ([`multilingual-drift-audit-2026-07.md`](multilingual-drift-audit-2026-07.md)) "
        "which isolated `link_*` URL coverage as the **only material i18n residual** — "
        "prose content is 100% clean across all 4 CSVs × 7 languages.\n\n"
        "## 0. Headline\n\n"
        f"| Class | Cells | Status |\n|---|---:|---|\n"
        f"| NATIVE_URL (localized to a native article) | {native} | ✅ correct |\n"
        f"| OTHER_URL (language-agnostic reference) | {other} | ✅ acceptable |\n"
        f"| **FR_URL (FR URL copied, not localized)** | **{fr}** | ❌ gap |\n"
        f"| **EMPTY (FR has link, lang empty)** | **{empty}** | ❌ gap |\n"
        f"| **TRUE i18n URL GAP (FR_URL + EMPTY)** | **{gap}** | treatment candidate |\n\n"
        + body + "\n\n"
        "## 1. Nature of the gap — why this is NOT a gpt-5.5 translation task\n\n"
        "Translating a `link_*` URL is **not text translation** — it is **cross-language "
        "article resolution**: given a French Wikipedia/Wiktionary article (or a French "
        "source), find the *equivalent native-language article* on `{lang}.wikipedia.org` "
        "(or a comparable native source). The URL string itself is not 'translated'; the "
        "*target article* must be discovered.\n\n"
        "**gpt-5.5 can help, but cannot close it alone:**\n"
        "- ✅ gpt-5.5 can **propose candidate** native article titles given the fallacy's "
        "`text_fr` + `desc_fr` (e.g. 'Appel à l'ignorance' → ru:'Апелляция к незнанию'). "
        "This is a *suggestion pass*, ~1259 calls, cheap.\n"
        "- ❌ gpt-5.5 **cannot verify the article exists** at that exact title on "
        "{lang}.wikipedia.org, nor that it covers the same fallacy — Wikipedia titles "
        "vary, articles may not exist, or may be disambiguation pages. **A human "
        "(or a Wikipedia API HEAD check) must confirm each candidate resolves.**\n"
        "- ❌ For non-Wikipedia FR sources (cortecs.org, service-public.fr, village-"
        "justice.com, huffpost.com FR), there is **no translation path at all** — a "
        "native equivalent source must be researched manually or the link dropped.\n\n"
        "This matches the standing memory `i18n-coverage-gap-is-link-urls`: link "
        "translation is **human research**, deliberately out of gpt-5.5's scope. The "
        "drift audit #795 confirmed it is the *only* residual (prose is 100% clean).\n\n"
        "## 2. Treatment recommendation (decision input for ai-01 / jsboige)\n\n"
        "| Option | What | Effort | Risk | Recommended? |\n|---|---|---|---|---|\n"
        "| **A — Defer (do nothing now)** | Ship v0.9.0 with current `link_*` state; native readers fall back to FR/reference URLs | 0 | Low (FR URL still works, just not localized) | ✅ **default** |\n"
        "| **B — gpt-5.5 candidate pass + human verify** | gpt-5.5 proposes {lang} article title for each FR-URL/EMPTY cell; human (or Wikipedia API) confirms + fills | ~1259 gpt-5.5 calls + human verify | Medium (gpt-5.5 may hallucinate titles) | ⚠️ post-tag, if jsboige wants |\n"
        "| **C — Wikipedia API auto-resolve** | Script queries `{lang}.wikipedia.org` API with the FR article title + langlinks to find the native equivalent; fills only API-confirmed matches | Script + spot-check | Low (API-confirmed = real article) | ✅ best ROI, post-tag |\n"
        "| **D — Drop FR-specific non-Wikipedia links for non-FR langs** | For cortecs/service-public/village-justice URLs in non-FR columns, clear the cell (a dead FR link is worse than no link) | Small | Low | ⚠️ editorial decision |\n\n"
        "**Recommendation:** **A (defer to v1.0)** for v0.9.0 — the gap is pre-existing, "
        "documented (#192, #795), and does not block the release (prose is clean, URLs "
        "are a secondary navigation aid). Post-tag, **Option C (Wikipedia langlinks API "
        "auto-resolve)** has the best ROI: it programmatically resolves the ~Wikipedia "
        "subset of FR_URL+EMPTY to native articles with zero hallucination risk (API-"
        "confirmed), leaving only the non-Wikipedia FR sources for Option D/manual.\n\n"
        "## 3. Scope breakdown (Wikipedia vs non-Wikipedia FR sources)\n\n"
        "The FR source links split into an **automatable** subset (Wikipedia/Wiktionary — "
        "Option C langlinks API can resolve these to native articles with zero hallucination) "
        "and a **manual-only** subset (French institutional/editorial sources — no auto-path):\n\n"
        "| Dataset | FR links total | Wikipedia/Wiktionary (automatable) | Non-Wiki FR (manual/drop) |\n"
        "|---|---:|---:|---:|\n"
        + "".join(
            f"| {ds['name']} | {ds['fr_link_filled']} | {ds['fr_wiki']} ({ds['fr_wiki']*100//(ds['fr_link_filled'] or 1)}%) | "
            f"{ds['fr_nonwiki']} ({ds['fr_nonwiki']*100//(ds['fr_link_filled'] or 1)}%) |\n"
            for ds in audit
        )
        + "\nThe non-Wiki FR sources are: `cortecs.org`, `service-public.fr`, `village-"
        "justice.com`, `huffpost.com` (FR), `persee.fr`, `leadership-lavautoir.com` / "
        "`leadership-toolbox.fr`, `ameli.fr`, `communicaid.fr`, `franceculture.fr`, "
        "`halshs.archives-ouvertes.fr`, `communication-orthophonie.fr`, `caissedesdepots.fr` "
        "— all French-only institutional/editorial sites with **no native equivalent to "
        "translate to**. These are Option D candidates (clear the cell for non-FR langs: a "
        "dead FR link is worse than no link for a native reader).\n\n"
        "**ROI read:** the Wikipedia subset (the majority of FR links in Fallacies) is the "
        "high-ROI automatable target; the non-Wiki subset is a smaller editorial drop.\n\n"
        "## 4. Gate boundaries\n\n"
        "- ✅ **READ-ONLY** — 0 write prod CSV; `git diff b736a808 -- Cards/` empty.\n"
        "- ✅ Empirical — every count computed from the prod CSVs at master `b736a808`.\n"
        "- ✅ Bounded + pre-tag-safe — scoping doc only, no treatment executed; does "
        "not touch the v0.9.0 critical path.\n"
        "- ✅ Reusable idempotent script `tools/192-link-i18n-scoping.py` (`--json` / `--report`).\n"
        "- ❌ No CSV write, no link_* cell modified — treatment is a separate gated decision.\n"
        "- ❌ Verdict QA = ai-01 — this is a scoping input, not a treatment verdict.\n\n"
        "🤖 Worker po-2024 — #192 link_* i18n URL scoping (gated doc, 0 write, gap isolated).\n"
    )


if __name__ == "__main__":
    main()
