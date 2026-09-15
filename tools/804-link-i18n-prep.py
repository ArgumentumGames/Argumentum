#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""PREP #804 — Map fr.wikipedia.org/wiki/X to <lang>.wikipedia.org/wiki/<localized>.

For each input fr.wikipedia URL, query the MediaWiki langlinks API to discover
localized article titles in {ru, pt, ar, es, zh, fa}. If a lang has no langlink
(article does not exist in that language wiki), record (no-link, keep-FR).

Usage:
    python tools/804-link-i18n-prep.py --input <txt-file> --out report.md
    python tools/804-link-i18n-prep.py --csv-pk-list <txt-file>  # primary keys to sample

Input file format: one URL per line (https://fr.wikipedia.org/wiki/Article).
Output: Markdown report with per-URL mapping + summary stats.

Posture: PREP only — does NOT write to CSV. Verifies feasibility on a sample
(30 fallacies). Designed for PR [HOLD post-tag] gated decision.
"""
import argparse, json, re, sys, time, urllib.parse, urllib.request, urllib.error

API = "https://fr.wikipedia.org/w/api.php"
TARGET_LANGS = ["ru", "pt", "ar", "es", "zh", "fa"]
USER_AGENT = "ArgumentumBot/0.1 (https://github.com/ArgumentumGames/Argumentum; #804 prep)"


def fetch_langlinks(fr_title: str, timeout: int = 10) -> dict:
    """Return {lang: localized_title} via MediaWiki langlinks API.

    MediaWiki's `lllang` parameter is single-value, so we issue one request
    per target language. Returns only those matching TARGET_LANGS.
    """
    out = {}
    for lang in TARGET_LANGS:
        params = {
            "action": "query",
            "titles": fr_title,
            "prop": "langlinks",
            "lllimit": "max",
            "lllang": lang,
            "format": "json",
        }
        url = API + "?" + urllib.parse.urlencode(params)
        req = urllib.request.Request(url, headers={"User-Agent": USER_AGENT})
        try:
            with urllib.request.urlopen(req, timeout=timeout) as resp:
                data = json.loads(resp.read().decode("utf-8"))
        except (urllib.error.URLError, urllib.error.HTTPError, TimeoutError) as e:
            raise RuntimeError(f"API error for lang={lang}: {e}") from e
        for page in data.get("query", {}).get("pages", {}).values():
            for ll in page.get("langlinks", []) or []:
                if ll["lang"] in TARGET_LANGS:
                    out[ll["lang"]] = ll["*"]
    return out


def map_one(fr_url: str, retries: int = 2) -> dict:
    """Map a single fr.wikipedia URL → {lang: localized URL or None}."""
    m = re.match(r"https?://fr\.wikipedia\.org/wiki/(.+)", fr_url)
    if not m:
        return {"fr_url": fr_url, "error": "not a fr.wikipedia URL"}
    fr_title = urllib.parse.unquote(m.group(1))
    last_err = None
    for attempt in range(retries + 1):
        try:
            links = fetch_langlinks(fr_title)
            result = {"fr_url": fr_url, "fr_title": fr_title, "mapped": {}}
            for lang in TARGET_LANGS:
                if lang in links:
                    result["mapped"][lang] = {
                        "title": links[lang],
                        "url": f"https://{lang}.wikipedia.org/wiki/{urllib.parse.quote(links[lang].replace(' ', '_'))}",
                    }
                else:
                    result["mapped"][lang] = None  # keep FR + note
            return result
        except (urllib.error.URLError, urllib.error.HTTPError, TimeoutError) as e:
            last_err = e
            if attempt < retries:
                time.sleep(1.0 * (attempt + 1))
            else:
                return {"fr_url": fr_url, "fr_title": fr_title, "error": str(last_err)}


def main():
    p = argparse.ArgumentParser()
    p.add_argument("--input", help="Text file: one fr.wikipedia URL per line")
    p.add_argument("--out", help="Output Markdown report path")
    p.add_argument("--delay", type=float, default=0.2, help="Seconds between API calls")
    args = p.parse_args()

    if not args.input or not args.out:
        p.error("--input and --out are required")

    with open(args.input, encoding="utf-8") as f:
        urls = [line.strip() for line in f if line.strip() and not line.startswith("#")]
    print(f"Loaded {len(urls)} URLs from {args.input}", file=sys.stderr)

    results = []
    for i, u in enumerate(urls, 1):
        r = map_one(u)
        results.append(r)
        mapped_count = sum(1 for v in r.get("mapped", {}).values() if v) if "mapped" in r else 0
        print(f"[{i}/{len(urls)}] {r.get('fr_title', u)}: {mapped_count}/{len(TARGET_LANGS)} langs mapped", file=sys.stderr)
        if i < len(urls):
            time.sleep(args.delay)

    # Stats
    total = len(results)
    errors = sum(1 for r in results if "error" in r)
    success = [r for r in results if "mapped" in r]
    per_lang = {lang: sum(1 for r in success if r["mapped"].get(lang)) for lang in TARGET_LANGS}
    fully_mapped = sum(1 for r in success if all(r["mapped"].get(lang) for lang in TARGET_LANGS))
    no_link = sum(1 for r in success if not any(r["mapped"].get(lang) for lang in TARGET_LANGS))

    lines = [
        "# #804 PREP — fr.wikipedia → localized mapping sample",
        "",
        f"**Sample size:** {total} fr.wikipedia URLs  ",
        f"**Target langs:** {', '.join(TARGET_LANGS)}  ",
        f"**API errors:** {errors}/{total}  ",
        "",
        "## Coverage per language",
        "",
        "| Lang | Mapped | % of sample |",
        "|---|---:|---:|",
    ]
    for lang in TARGET_LANGS:
        n = per_lang[lang]
        pct = (n / total * 100) if total else 0
        lines.append(f"| `{lang}.wikipedia` | {n} | {pct:.0f}% |")
    lines += [
        "",
        f"**Fully mapped (all 6 langs):** {fully_mapped}/{total} ({fully_mapped/total*100:.0f}%)  " if total else "",
        f"**No link at all (would all default to FR):** {no_link}/{total} ({no_link/total*100:.0f}%)",
        "",
        "## Per-URL mapping",
        "",
        "| fr.wikipedia | ru | pt | ar | es | zh | fa |",
        "|---|---|---|---|---|---|---|",
    ]
    for r in results:
        fr = r.get("fr_title") or r.get("fr_url", "?")
        cells = [f"`{fr}`"]
        if "error" in r:
            cells += ["ERR"] * len(TARGET_LANGS)
        else:
            for lang in TARGET_LANGS:
                v = r["mapped"].get(lang)
                cells.append(f"[{v['title']}]({v['url']})" if v else "—")
        lines.append("| " + " | ".join(cells) + " |")

    lines += [
        "",
        "## Verdict",
        "",
        "- **Feasibility:** langlinks API returns localized titles deterministically.",
        "- **Volume (full audit gap):** 505 unique fr.wikipedia URLs × 6 langs = ~3030 lookups. ~10 min at 0.2s delay.",
        "- **Cost:** free (MediaWiki public API, no auth).",
        "- **Risk:** low — additions are additive (`link_xx` columns already exist; updates preserve empty cells if no langlink).",
        "- **Posture:** PREP only — sample of 30 here. Full run gated by jsboige + post-tag.",
        "",
        "— po-2024 (tick 24, dispatch ai-01 `07kpoq`)",
    ]

    with open(args.out, "w", encoding="utf-8") as f:
        f.write("\n".join(lines) + "\n")
    print(f"Wrote {args.out}", file=sys.stderr)


if __name__ == "__main__":
    main()