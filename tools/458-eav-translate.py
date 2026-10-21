#!/usr/bin/env python3
"""#458 grain 24b-3 — translate the displayed 2sxc EAV content (docs/dnn-localization/eav-content.csv).

Reads the pivot produced by tools/458-eav-content-export.py, selects the
translatable rows, calls the Anthropic OpenAI-compatible endpoint, and writes
the target cells into a COPY. The pivot is never modified in place.

Why a dedicated tool instead of the DatasetUpdater engine: the EAV pivot's
column set (app_id/entity_guid/field/field_type/value_kind) does not match the
engine's DnnApp33 dataset (app/guid/attribute), and the engine cannot express
"translate Term.Title but never Term.UrlKey". The field allowlist below is the
whole point — a slug, an e-mail, a path or an asset name must never be
translated, and a URL inside an HTML fragment must survive byte-for-byte.

Usage:
  python -I tools/458-eav-translate.py --apps 59 --dry-run --self-test
  python -I tools/458-eav-translate.py --apps 59 --out out.csv --model claude-sonnet-5-5
  python -I tools/458-eav-translate.py --apps 60,52 --limit 8 --dry-run

Spend is printed per call and totalled (prices are the published per-MTok ones).
"""
import argparse
import csv
import io
import json
import os
import re
import sys
import urllib.error
import urllib.request

LANGS = ["en", "ru", "pt", "es", "ar", "fa", "zh"]

# Fields that carry human-readable prose. Anything not listed is never written.
TRANSLATABLE = {
    # 2sxc app resources / labels (59, 52, 60, 33 …)
    "GoBackText", "MoreText", "GlossaryMenu", "TermNotFoundTitle", "TermNotFoundText",
    "LabelReadMore", "LabelBackToList", "LabelCategoryAll", "LabelEventNotExists",
    "LabelEventNotExistsText", "PlaceholderMissingImage", "LabelShowFromPill",
    "LabelShowToPill", "LabelAdminMenuNew", "LabelAdminMenuManage", "LabelAdminDetailPageWarning",
    # content fields
    "Title", "Summary", "Details", "Description", "Introduction", "Content", "Teaser",
    "PageTitle", "Name", "Abbreviation", "FullName", "Author", "Licence",
    # game rule blocks
    "RuleSummary", "RuleMaterial", "RuleInstallation", "RuleContent", "RuleVariants",
    "RuleMemoInstructions", "Material", "Installation", "Variants", "Memo",
    # a *label*, not the asset it points at (RuleMemoCardDownload is the URL)
    "RuleMemoCard",
}
# Shapes that must never be translated even when the field name looks prose-y.
NEVER = {"UrlKey", "Path", "Email", "Link", "Images", "AlbumThumbnail", "Image",
         "CropAnchor", "SortMode", "HeadingType", "RuleMemoCardFileNamePrefix",
         "RuleMemoCardDownload"}
# Content types whose fields are identities (people, brands, mailboxes, slugs).
SKIP_TYPES = {"Contributor", "Location", "Person"}

PROMPT = (
    "You translate user-visible web content. The source text is French or English. "
    "Return a JSON object with exactly the keys you were given; each value is another "
    "object mapping each requested language code to the translation. "
    "Rules: (1) HTML fragments are common — keep every tag, attribute, class, href, "
    "src, GUID and URL byte-for-byte; translate only human-readable text nodes and "
    "alt/title attribute values. (2) Never translate brand names, organisation names, "
    "people's names, e-mail addresses or slugs unless a conventional exonym exists. "
    "(2b) Creative titles — of a game rule, an activity, a workshop, an article — are "
    "content, not brands: translate them the way the other languages of the site do. "
    "(3) Emoji, arrows and symbol sequences (🥇 ✅ ➜ 🏆 …) are game notation: copy them "
    "exactly as they appear, in the same order, and translate only the words around them. "
    "HTML entities such as &eacute; or &rsquo; may stay as they are. "
    "(4) Keep the register of a public educational game site. (5) Do not add or drop "
    "information. (6) If a value is a pure identifier or an empty string, echo it "
    "unchanged. Respond with JSON only."
)

# Published per-MTok prices. Sonnet 5.5 is 2/10, not 3/15: the first version of
# this table overstated every Sonnet run by 1.5x (caught in review, #1924).
PRICES = {"claude-haiku-5-5": (0.10, 0.50), "claude-sonnet-5-5": (2.00, 10.00)}


def load(path):
    raw = open(path, "rb").read()
    assert raw[:3] != b"\xef\xbb\xbf", "pivot must stay BOM-less"
    return list(csv.DictReader(io.StringIO(raw.decode("utf-8"), newline="")))


def save(path, rows, header):
    buf = io.StringIO(newline="")
    w = csv.DictWriter(buf, fieldnames=header, lineterminator="\n")
    w.writeheader()
    w.writerows(rows)
    open(path, "w", encoding="utf-8", newline="").write(buf.getvalue())


def select(rows, apps, types, langs, overwrite, only=None):
    todo = []
    for r in rows:
        if only and not re.search(only, r.get("key") or ""):
            continue
        if apps and str(r.get("app_id")) not in apps:
            continue
        if types and (r.get("content_type") or "") not in types:
            continue
        if (r.get("content_type") or "") in SKIP_TYPES:
            continue
        field = r.get("field") or ""
        if field in NEVER or field not in TRANSLATABLE:
            continue
        src = (r.get("fr") or "").strip()
        if not src:
            continue
        missing = [l for l in langs if overwrite or not (r.get(l) or "").strip()]
        if missing:
            todo.append((r, missing))
    return todo


def key_of(r):
    return r["key"]


# Ranges that carry pictograms, arrows and marker sequences in this corpus.
MARKER_RANGES = ((0x2190, 0x21FF), (0x2200, 0x22FF), (0x2300, 0x23FF),
                 (0x2460, 0x24FF), (0x2600, 0x27BF), (0x2B00, 0x2BFF),
                 (0x1F000, 0x1FAFF), (0xFE00, 0xFE0F), (0x200D, 0x200D))


def markers(s):
    """Multiset of symbol characters — a translation must preserve every one."""
    return {c: s.count(c) for c in set(s)
            if any(lo <= ord(c) <= hi for lo, hi in MARKER_RANGES)}


def validate(src, tgt):
    """A cell is written only if the markup and the marker sequence survive intact.

    Returns a list of problems; an empty list means the translation is admissible.
    """
    if not tgt or not tgt.strip():
        return ["empty translation"]
    problems = []
    tags = lambda s: sorted(re.findall(r"</?([a-zA-Z][a-zA-Z0-9]*)", s.lower()))
    attrs = lambda s: sorted(re.findall(r"\s([a-zA-Z-]+)\s*=", s))
    urls = lambda s: sorted(re.findall(r'(?:href|src)="([^"]*)"', s))
    if tags(src) != tags(tgt):
        problems.append(f"HTML tags differ: {tags(src)} -> {tags(tgt)}")
    if attrs(src) != attrs(tgt):
        problems.append(f"HTML attributes differ: {attrs(src)} -> {attrs(tgt)}")
    if urls(src) != urls(tgt):
        problems.append(f"URLs differ: {urls(src)} -> {urls(tgt)}")
    if markers(src) != markers(tgt):
        problems.append(f"pictograms differ: {markers(src)} -> {markers(tgt)}")
    return problems


def schema_for(payload):
    """Strict mode forbids open objects: every key must be enumerated."""
    props = {}
    for k, langs in payload.items():
        props[k] = {
            "type": "object",
            "properties": {l: {"type": "string"} for l in langs},
            "required": list(langs),
            "additionalProperties": False,
        }
    return {
        "type": "object",
        "properties": props,
        "required": list(payload),
        "additionalProperties": False,
    }


def call_api(base, key, model, payload, max_tokens):
    body = {
        "model": model,
        "messages": [
            {"role": "system", "content": PROMPT},
            {"role": "user", "content": json.dumps(payload, ensure_ascii=False)},
        ],
        # `temperature` is refused by this model on the compat endpoint — never send it.
        "max_tokens": max_tokens,
        "response_format": {
            "type": "json_schema",
            "json_schema": {
                "name": "translations",
                "strict": True,
                "schema": schema_for(payload),
            },
        },
    }
    req = urllib.request.Request(
        base.rstrip("/") + "/chat/completions",
        data=json.dumps(body).encode("utf-8"),
        headers={"Content-Type": "application/json", "Authorization": "Bearer " + key},
        method="POST",
    )
    try:
        with urllib.request.urlopen(req, timeout=300) as resp:
            data = json.loads(resp.read().decode("utf-8"))
    except urllib.error.HTTPError as e:
        # The compat endpoint puts the real reason in the body, not the status line.
        detail = e.read().decode("utf-8", "replace")[:800]
        raise SystemExit(f"HTTP {e.code}: {detail}") from None
    return data


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--in", dest="src", default="docs/dnn-localization/eav-content.csv")
    ap.add_argument("--out", default=None)
    ap.add_argument("--apps", default="")
    ap.add_argument("--types", default="")
    ap.add_argument("--only", default="", help="regex on the row key (GUID|Field)")
    ap.add_argument("--langs", default=",".join(LANGS))
    ap.add_argument("--model", default="claude-sonnet-5-5")
    ap.add_argument("--base", default="https://api.anthropic.com/v1")
    ap.add_argument("--key-path", default=".keys/anthropic-key.txt")
    ap.add_argument("--batch", type=int, default=5)
    ap.add_argument("--limit", type=int, default=0)
    ap.add_argument("--max-tokens", type=int, default=8192)
    ap.add_argument("--overwrite", action="store_true")
    ap.add_argument("--dry-run", action="store_true")
    ap.add_argument("--self-test", action="store_true")
    a = ap.parse_args()

    apps = {x.strip() for x in a.apps.split(",") if x.strip()}
    types = {x.strip() for x in a.types.split(",") if x.strip()}
    langs = [x.strip() for x in a.langs.split(",") if x.strip()]
    rows = load(a.src)
    header = list(rows[0].keys())
    todo = select(rows, apps, types, langs, a.overwrite, a.only or None)

    if a.self_test:
        # Serialization must round-trip the untouched pivot byte-for-byte.
        buf = io.StringIO(newline="")
        w = csv.DictWriter(buf, fieldnames=header, lineterminator="\n")
        w.writeheader()
        w.writerows(rows)
        orig = open(a.src, encoding="utf-8", newline="").read()
        assert buf.getvalue() == orig, "round-trip is not byte-identical"
        print(f"self-test OK: round-trip byte-identical, {len(rows)} rows, {len(todo)} rows to translate")

    if a.limit:
        todo = todo[: a.limit]
    cells = sum(len(m) for _, m in todo)
    print(f"selection: {len(todo)} rows, {cells} cells, apps={sorted(apps) or 'ALL'} "
          f"types={sorted(types) or 'ALL'} langs={langs}")
    if a.dry_run or not todo:
        for r, missing in todo[:10]:
            print(f"  {r['app_id']}/{r['content_type']}/{r['field']}: {r['fr'][:50]!r} -> {missing}")
        return 0

    key = open(a.key_path, encoding="utf-8").read()
    assert key and "\n" not in key.strip(), "key file must be a single line"
    key = key.strip()
    pin, pout = PRICES.get(a.model, (0.0, 0.0))
    tin = tout = 0
    done = 0
    rejects = []
    for i in range(0, len(todo), a.batch):
        batch = todo[i : i + a.batch]
        payload = {key_of(r): {l: r["fr"] for l in missing} for r, missing in batch}
        data = call_api(a.base, key, a.model, payload, a.max_tokens)
        usage = data.get("usage") or {}
        tin += usage.get("prompt_tokens", 0)
        tout += usage.get("completion_tokens", 0)
        content = data["choices"][0]["message"]["content"]
        try:
            out = json.loads(content)
        except json.JSONDecodeError:
            print("  !! non-JSON answer on batch", i, content[:200], file=sys.stderr)
            continue
        for r, missing in batch:
            got = out.get(key_of(r)) or {}
            for l in missing:
                v = got.get(l)
                if not isinstance(v, str):
                    continue
                bad = validate(r["fr"], v)
                if bad:
                    rejects.append((r["key"], r["field"], l, "; ".join(bad)[:220]))
                    continue
                r[l] = v
                done += 1
        print(f"  batch {i // a.batch + 1}: {len(batch)} rows, cumulative cells={done}, "
              f"in={tin} out={tout}")

    for rej in rejects:
        print("  REJECT", rej[0][:8], rej[1], rej[2], "::", rej[3], file=sys.stderr)
    spend = tin / 1e6 * pin + tout / 1e6 * pout
    print(f"TOTAL cells written={done} rejected={len(rejects)} tokens in={tin} out={tout} "
          f"spend=${spend:.4f} ({a.model})")
    if a.out:
        save(a.out, rows, header)
        print("written", a.out)
    return 0


if __name__ == "__main__":
    sys.exit(main())