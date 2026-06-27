#!/usr/bin/env python3
"""
192-link-coverage-langlinks-probe.py
Read-only measurement: MEASURE the real fill-rate of the link_* gap via the
MediaWiki langlinks API. Converts #600's theoretical ceiling into a measured
number. By default (SAMPLE_SIZE=0) probes ALL unique candidate articles
(full census); pass an int N to probe a deterministic-strided sample of N.

Method (per #600):
  link_<lang> cells = per-language Wikipedia article URLs. Candidates to fill =
  rows that have a link_en which is a WIKIPEDIA URL and miss link_<target>.
  For each candidate article, langlinks returns the cross-language article
  titles (when they exist). A missing cell is "fillable+resolvable" iff the
  article has a langlink to the target language.

  NOTE: only en.wikipedia.org/wiki/<Title> link_en values are resolvable.
  rationalwiki / logicallyfallacious / philosophy.lander etc. are NOT
  Wikipedia -> excluded from the langlinks pool (they are a separate,
  human-curated category, preserved as-is).

Read-only: opens CSVs read-only, queries the public MediaWiki API (no key,
rate-limited), prints results to stdout. 0 write under Cards/.
"""
import csv, json, sys, time, urllib.parse, urllib.request

DATASETS = {
    "fallacies": "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv",
    "virtues": "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv",
}
CSV_PATH = DATASETS["fallacies"]
DATASET_NAME = "fallacies"
TARGET_LANGS = ["ru", "pt", "es", "ar", "fa", "zh"]  # fr already ~45%, en = source
SAMPLE_SIZE = 0           # 0 = probe ALL unique candidate articles (full run); >0 = strided sample
API_DELAY = 0.3           # polite delay between API calls (MediaWiki best practice)
API_TIMEOUT = 25
EN_WIKI_PREFIX = "https://en.wikipedia.org/wiki/"
# MediaWiki REQUIRES a descriptive User-Agent — default urllib UA is 403-forbidden.
USER_AGENT = "ArgumentumLinkCoverageProbe/1.0 (educational card game taxonomy research; contact: jsboige@gmail.com)"


def extract_wiki_title(link_en):
    """Return the article title if link_en is an en.wikipedia.org/wiki/ URL, else None."""
    url = (link_en or "").strip()
    if not url.startswith(EN_WIKI_PREFIX):
        return None
    title = url[len(EN_WIKI_PREFIX):]
    title = urllib.parse.unquote(title).replace("_", " ").strip()
    return title or None


def query_langlinks(title):
    """Query MediaWiki langlinks for one article. Return set of langs that have a link."""
    api = ("https://en.wikipedia.org/w/api.php?action=query&prop=langlinks"
           "&titles=%s&lllimit=500&format=json" % urllib.parse.quote(title))
    try:
        req = urllib.request.Request(api, headers={"User-Agent": USER_AGENT})
        with urllib.request.urlopen(req, timeout=API_TIMEOUT) as r:
            data = json.load(r)
    except Exception as e:
        return None, "error: %s" % e
    pages = data.get("query", {}).get("pages", {})
    langs = set()
    for p in pages.values():
        for ll in p.get("langlinks", []):
            langs.add(ll.get("lang"))
    return langs, None


def main():
    sample_size = SAMPLE_SIZE
    dataset = DATASET_NAME
    csv_path = CSV_PATH
    args = [a for a in sys.argv[1:]]
    # parse: optional [sample_size] [dataset], dataset name recognized by substring
    for a in args:
        if a.isdigit() or a == "0":
            sample_size = int(a)
        elif a.lower() in DATASETS:
            dataset = a.lower()
            csv_path = DATASETS[dataset]
    print("=== DATASET: %s (%s) ===" % (dataset, csv_path))
    with open(csv_path, encoding="utf-8-sig") as f:
        rows = list(csv.DictReader(f))

    total = len(rows)
    # Categorize link_en
    wiki_titles = {}      # row -> title (only Wikipedia URLs)
    non_wiki_en = 0
    empty_en = 0
    for row in rows:
        en = (row.get("link_en") or "").strip()
        if not en:
            empty_en += 1
            continue
        t = extract_wiki_title(en)
        if t:
            wiki_titles[id(row)] = t
        else:
            non_wiki_en += 1

    print("=== link_en categorization (%s, %d rows) ===" % (dataset, total))
    print("  Wikipedia URLs (langlinks-resolvable): %d" % len(wiki_titles))
    print("  Non-Wikipedia URLs (excluded from langlinks): %d" % non_wiki_en)
    print("  Empty: %d" % empty_en)

    # Build candidate cells: for each Wikipedia-URL row, which target langs are MISSING?
    # candidate_missing[lang] = list of (row_id, title)
    candidate_missing = {lang: [] for lang in TARGET_LANGS}
    for row in rows:
        title = wiki_titles.get(id(row))
        if not title:
            continue
        for lang in TARGET_LANGS:
            val = (row.get("link_" + lang) or "").strip()
            if not val:
                candidate_missing[lang].append((id(row), title))

    print("\n=== candidate missing cells (have wiki link_en, miss link_<lang>) ===")
    for lang in TARGET_LANGS:
        print("  link_%s: %d missing" % (lang, len(candidate_missing[lang])))

    # Unique article titles to probe (union of all candidates), deterministic strided sample
    all_titles = sorted({t for lst in candidate_missing.values() for _, t in lst})
    if sample_size and len(all_titles) > sample_size:
        stride = len(all_titles) / sample_size
        sample = [all_titles[int(i * stride)] for i in range(sample_size)]
    else:
        sample = all_titles
    print("\n=== probing %d unique candidate articles (of %d) via langlinks ===" % (len(sample), len(all_titles)))

    # Resolve langlinks for sampled titles
    title_langs = {}  # title -> set of available langs (or None if error)
    errors = 0
    for i, title in enumerate(sample):
        langs, err = query_langlinks(title)
        if err:
            errors += 1
            title_langs[title] = set()
        else:
            title_langs[title] = langs or set()
        time.sleep(API_DELAY)
        if (i + 1) % 10 == 0:
            print("  ...%d/%d (errors=%d)" % (i + 1, len(sample), errors), file=sys.stderr)

    # Measure: of sampled candidates per lang, how many resolve?
    print("\n=== MEASURED fill-rate (sample, langlinks-confirmed) ===")
    print("  (resolvable = missing cell AND article has a langlink to that lang)")
    results = {}
    for lang in TARGET_LANGS:
        sampled = [(rid, t) for (rid, t) in candidate_missing[lang] if t in title_langs]
        if not sampled:
            results[lang] = (0, 0, 0.0)
            continue
        resolvable = sum(1 for (_, t) in sampled if lang in title_langs[t])
        rate = resolvable / len(sampled) * 100
        results[lang] = (resolvable, len(sampled), rate)
        print("  link_%s: %d / %d sampled resolve = %.0f%%  (projected of %d missing: ~%d fillable)"
              % (lang, resolvable, len(sampled), rate, len(candidate_missing[lang]),
                 round(rate / 100 * len(candidate_missing[lang]))))

    total_missing = sum(len(candidate_missing[l]) for l in TARGET_LANGS)
    projected_fill = sum(round(results[l][2] / 100 * len(candidate_missing[l])) for l in TARGET_LANGS)
    print("\n=== projection (sample-derived, NOT a guarantee) ===")
    print("  total candidate missing cells: %d" % total_missing)
    print("  projected fillable (langlinks-resolvable): ~%d (%.0f%% of candidates)"
          % (projected_fill, (projected_fill / total_missing * 100) if total_missing else 0))
    print("  errors during probe: %d/%d" % (errors, len(sample)))
    print("\nCaveat: sample of %d articles, deterministic-strided. Full run would refine." % len(sample))


if __name__ == "__main__":
    main()
