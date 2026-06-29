#!/usr/bin/env python3
"""
#600/#606 follow-up — link_* langlinks RESOLVER (sidecar, 0 write under Cards/).

Extends docs/taxonomy/192-link-coverage-langlinks-probe.py (which MEASURES the
resolvable ceiling) into the RESOLUTION step: it captures the target-language
article TITLE (the probe discards it) and builds the candidate fill URLs,
emitting a sidecar report for human spot-validation. This is step 1 of the
#600 §6 fill methodology — without ever touching Cards/.

WHAT IT DOES
  For every node that has an en.wikipedia.org/wiki/<Title> link_en and is
  MISSING link_<lang>, query the MediaWiki langlinks API, capture the target
  title, and emit:
    <dataset>, <pk/decimal_path>, <lang>, <resolved_url>
  The report goes to STDOUT by default, or to a sidecar file (--out). It is
  NEVER written into Cards/ (pre-tag freeze + #600 §6 human-validation gate).

WHY A SEPARATE TOOL (not just run the probe)
  The probe returns {set of lang codes} -> answers "is it resolvable?".
  This tool returns {lang -> target_title} -> answers "what is the URL?" and
  produces the candidate-fill list. The measured ceiling (2919 cells, #600 §5.1)
  is the budget; this tool materializes the candidate URLs behind that budget.

SAFETY
  - 0 write under Cards/. Sidecar only (stdout or --out path).
  - Public MediaWiki API, no key, rate-limited (API_DELAY), descriptive UA.
  - RTL/CJK homonym risk (#600 §6.4): AR/FA/ZH resolved URLs are CANDIDATES,
    not authoritative — human spot-validation is non-optional before any write.

USAGE (from repo root)
  python tools/link-langlinks-resolve.py                     # full, fallacies, stdout
  python tools/link-langlinks-resolve.py 50 virtues           # strided sample of 50, virtues
  python tools/link-langlinks-resolve.py 0 fallacies --out tmp/link-resolve-fallacies.csv
"""
import csv
import json
import sys
import time
import urllib.parse
import urllib.request

DATASETS = {
    "fallacies": "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv",
    "virtues": "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv",
}
# fr is ~45-97% (partly filled, source mix); en is the SOURCE we resolve FROM.
# Resolve the 6 under-filled targets (matches the #600 probe).
TARGET_LANGS = ["ru", "pt", "es", "ar", "fa", "zh"]
DEFAULT_DATASET = "fallacies"
SAMPLE_SIZE = 0           # 0 = full run; >0 = deterministic-strided sample
API_DELAY = 0.3           # polite throttle (MediaWiki best practice)
API_TIMEOUT = 25
EN_WIKI_PREFIX = "https://en.wikipedia.org/wiki/"
USER_AGENT = "ArgumentumLinkResolver/1.0 (educational card game taxonomy; contact: jsboige@gmail.com)"

# per-dataset: (pk column for the sidecar key, link_en column)
DATASET_KEYS = {
    "fallacies": ("PK", "decimal_path"),
    "virtues": ("pk", "decimal_path_padded"),
}


def extract_wiki_title(link_en):
    url = (link_en or "").strip()
    if not url.startswith(EN_WIKI_PREFIX):
        return None
    title = url[len(EN_WIKI_PREFIX):]
    title = urllib.parse.unquote(title).replace("_", " ").strip()
    return title or None


def build_url(lang, title):
    """Build the canonical <lang>.wikipedia.org URL from a resolved title."""
    return "https://%s.wikipedia.org/wiki/%s" % (lang, urllib.parse.quote(title.replace(" ", "_")))


def query_langlinks_titles(title):
    """Query langlinks for one article. Return ({lang: target_title}, error)."""
    api = ("https://en.wikipedia.org/w/api.php?action=query&prop=langlinks"
           "&titles=%s&lllimit=500&format=json" % urllib.parse.quote(title))
    try:
        req = urllib.request.Request(api, headers={"User-Agent": USER_AGENT})
        with urllib.request.urlopen(req, timeout=API_TIMEOUT) as r:
            data = json.load(r)
    except Exception as e:
        return None, "error: %s" % e
    pages = data.get("query", {}).get("pages", {})
    out = {}
    for p in pages.values():
        for ll in p.get("langlinks", []):
            lang = ll.get("lang")
            target = ll.get("*")
            if lang and target:
                out[lang] = target
    return out, None


def main():
    sample_size = SAMPLE_SIZE
    dataset = DEFAULT_DATASET
    out_path = None
    args = sys.argv[1:]
    i = 0
    while i < len(args):
        a = args[i]
        if a == "--out":
            out_path = args[i + 1] if i + 1 < len(args) else None
            i += 2
        elif a.isdigit() or a == "0":
            sample_size = int(a)
            i += 1
        elif a.lower() in DATASETS:
            dataset = a.lower()
            i += 1
        else:
            i += 1
    csv_path = DATASETS[dataset]
    pk_cols = DATASET_KEYS[dataset]

    with open(csv_path, encoding="utf-8-sig") as f:
        rows = list(csv.DictReader(f))

    # categorize link_en + build candidate (row -> title) for missing target cells
    wiki_title_of = {}     # id(row) -> en title
    for row in rows:
        t = extract_wiki_title((row.get("link_en") or "").strip())
        if t:
            wiki_title_of[id(row)] = t

    # candidate_missing[lang] = list of (row, title)
    candidate_missing = {lang: [] for lang in TARGET_LANGS}
    for row in rows:
        title = wiki_title_of.get(id(row))
        if not title:
            continue
        for lang in TARGET_LANGS:
            if not (row.get("link_" + lang) or "").strip():
                candidate_missing[lang].append((row, title))

    all_titles = sorted({t for lst in candidate_missing.values() for _, t in lst})
    if sample_size and len(all_titles) > sample_size:
        stride = len(all_titles) / sample_size
        sample = [all_titles[int(i * stride)] for i in range(sample_size)]
    else:
        sample = all_titles

    print("=== link_* RESOLVER: %s (%s) | %d/%d unique articles ===" %
          (dataset, csv_path, len(sample), len(all_titles)), file=sys.stderr)
    print("    candidates missing: %s" %
          {lang: len(lst) for lang, lst in candidate_missing.items()}, file=sys.stderr)

    # resolve: title -> {lang: target_title}
    title_targets = {}
    errors = 0
    for idx, title in enumerate(sample):
        targets, err = query_langlinks_titles(title)
        if err:
            errors += 1
            title_targets[title] = {}
        else:
            title_targets[title] = targets or {}
        time.sleep(API_DELAY)
        if (idx + 1) % 10 == 0:
            print("    ...%d/%d (errors=%d)" % (idx + 1, len(sample), errors), file=sys.stderr)

    # emit sidecar: dataset, key, lang, resolved_url  (only resolvable candidates)
    def row_key(row):
        for c in pk_cols:
            v = (row.get(c) or "").strip()
            if v:
                return v
        return ""

    lines = ["dataset,key,link_lang,resolved_url"]
    resolved_count = {lang: 0 for lang in TARGET_LANGS}
    for lang in TARGET_LANGS:
        for row, title in candidate_missing[lang]:
            if title not in title_targets:
                continue
            targets = title_targets[title]
            if lang not in targets:
                continue
            url = build_url(lang, targets[lang])
            lines.append("%s,%s,%s,%s" % (dataset, row_key(row), lang, url))
            resolved_count[lang] += 1

    payload = "\n".join(lines) + "\n"
    if out_path:
        import os
        os.makedirs(os.path.dirname(out_path) or ".", exist_ok=True)
        with open(out_path, "w", encoding="utf-8", newline="") as f:
            f.write(payload)
        print("wrote sidecar: %s (%d candidate fills)" % (out_path, len(lines) - 1), file=sys.stderr)
    else:
        sys.stdout.write(payload)

    total_resolved = sum(resolved_count.values())
    print("\n=== SUMMARY (%s, %d articles probed) ===" % (dataset, len(sample)), file=sys.stderr)
    for lang in TARGET_LANGS:
        print("  link_%s: %d resolved URLs" % (lang, resolved_count[lang]), file=sys.stderr)
    print("  TOTAL candidate fills: %d  (probe ceiling was ~57%%; this materializes them)" %
          total_resolved, file=sys.stderr)
    print("  errors: %d/%d" % (errors, len(sample)), file=sys.stderr)
    print("  SAFETY: sidecar only. 0 write under Cards/. AR/FA/ZH need human spot-validation.", file=sys.stderr)


if __name__ == "__main__":
    main()
