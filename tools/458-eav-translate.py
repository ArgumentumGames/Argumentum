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
import urllib.parse
import urllib.request

LANGS = ["en", "ru", "pt", "es", "ar", "fa", "zh"]

# Fields that carry human-readable prose. Anything not listed is never written.
TRANSLATABLE = {
    # 2sxc app resources / labels (59, 52, 60, 33 …)
    "GoBackText", "MoreText", "GlossaryMenu", "TermNotFoundTitle", "TermNotFoundText",
    "LabelReadMore", "LabelBackToList", "LabelCategoryAll", "LabelEventNotExists",
    "LabelEventNotExistsText", "LabelShowFromPill",
    "LabelShowToPill", "LabelAdminMenuNew", "LabelAdminMenuManage", "LabelAdminDetailPageWarning",
    # content fields
    "Title", "Summary", "Details", "Description", "Introduction", "Content", "Teaser",
    "PageTitle", "Name", "Abbreviation", "FullName", "Author", "Licence",
    # game rule blocks
    "RuleSummary", "RuleMaterial", "RuleInstallation", "RuleContent", "RuleVariants",
    "RuleMemoInstructions", "Material", "Installation", "Variants", "Memo",
    # Labels, not the assets they point at. `RuleMemoCardDownload` is a *button
    # caption*, not the URL: measured 23/10 on the 579-row pivot, FR reads
    # "Télécharger la carte mémo" and 6 languages carry a translated caption —
    # no value anywhere has a URL shape (web2's C4 claimed the field was absent
    # from the source; it was in this NEVER list, which is why ar/fa stayed empty).
    "RuleMemoCard", "RuleMemoCardDownload",
}
# Shapes that must never be translated even when the field name looks prose-y.
# `RuleMemoCardFileNamePrefix` stays here: it is the *file name* the download
# link resolves to, so translating it would point at an asset that need not exist
# (ar/fa/ru/zh keep it empty → FR fallback name, which is the served-file reality).
NEVER = {"UrlKey", "Path", "Email", "Link", "Images", "AlbumThumbnail", "Image",
         "CropAnchor", "SortMode", "HeadingType", "RuleMemoCardFileNamePrefix",
         "PlaceholderMissingImage"}
# 2sxc ships every content type with a demo item, and its fields are prose with a
# French source — so the field allowlist cannot tell them from real content.
# Measured 23/10 on the pivot: 23 of the 59 candidate rows are demo material
# ("Demo Slide CR", "Demo content without Caption", lorem ipsum). Translating them
# would be paid waste and, worse, would put demo text on the site in 7 languages.
#
# The words are ANCHORED. A bare substring `cta` fires on `respectable`,
# `dictature`, `spectateur` and `pacta sunt servanda` — all four are excluded by
# the unanchored form and kept by this one (measured 23/10, reserve 6 of the
# #1945 review). On a corpus that is *about arguments* that is not hypothetical:
# it is a silent hole in the todo, and it grows as the corpus does. The anchored
# form fires on the same 23 rows, none lost, none gained. `lorem`, `ipsum` and
# `dolor` stay unanchored: they only ever occur inside filler text.
DEMO = re.compile(r"lorem|ipsum|dolor|\b(demo|example|sample|placeholder|cta)\b", re.I)
# A decline must be EXPLICIT. The model answers this token, and nothing else,
# when a value is a CMS default rather than content. An empty string is not a
# decline: a truncated answer, a wrong-shaped answer or a degenerate generation
# all produce `""` too, so it is rejected like any other invalid cell (reserve 1
# of the #1945 review — the previous code short-circuited `validate()` and
# counted it as a benign skip, so a systematic model failure would have printed
# `skipped=413 written=0` and read exactly like a clean pass).
SKIP_TOKEN = "__SKIP__"
# The four classes every requested cell falls into, and the only ones. A tuple so
# that the exhaustiveness assert runs over the class list itself and a class added
# later cannot escape it.
CLASSES = ("written", "skipped", "rejected", "absent")
# Content types whose fields are identities (people, brands, mailboxes, slugs).
SKIP_TYPES = {"Contributor", "Location", "Person"}
# Game Rule fields whose single source is the Rules CSV (owner decision 10/10):
# the site cards ARE the printed cards' Text_* — po-2023 injects them from the
# CSV (method of #458 c.6281223831, applied to es/pt 17/10, ru 19/10, zh 20/10).
# Translating them here would put another wording than the printed card on the
# site (ar title measured in the #1924 review). They stay empty in the pivot.
CSV_SOURCED = {"Game Rule": {"Title", "Summary", "Material", "Installation",
                             "Content", "Variants"}}

# Language variants pinned by fleet decision. The translation prompt AND every
# counter-review prompt must carry these — a bare "pt" made the working model
# answer in European Portuguese (3 cells of #1938, corrected by ai-01 in
# ae5090cc), and the Opus counter-review had validated them as "natural PT-PT"
# because the variant was nowhere stated. `en` is now pinned too: web2's W7
# counted the shipped corpus 268 American vs 18 British spellings (~15:1).
# `ru` and `ar` have no competing shipped variant (dispatch 22/10 09:32).
LANGUAGE_VARIANTS = {
    "en": "en-US, American English (web2 W7: corpus 268 AmE vs 18 BrE)",
    "pt": "pt-BR, Brazilian Portuguese (decision #1095)",
    "zh": "zh-CN, Simplified Chinese",
    "fa": "fa-IR, Iranian Persian",
    "es": "es-ES, Peninsular Spanish (name of the shipped pack)",
}


def variant_clause():
    """The pinning sentence shared by translation and counter-review prompts.

    Counter-review scripts import this module and append the clause to their
    system prompt, so the pinning has one source of truth.
    """
    return ("Language variants are pinned and override your default register: "
            + "; ".join(f"{code} = {desc}" for code, desc in LANGUAGE_VARIANTS.items())
            + ". Produce the pinned variant, not the neutral or European default.")


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
    "information. (6) If a value is a pure machine identifier — no natural-language "
    "words, the shape given to the code rather than to a reader, e.g. "
    "LabelShowFromPill or MoreText_Sub — echo it unchanged. "
    "(6b) Some values are not content but a CMS default, an editing instruction or a "
    "demo sample (\"New image\", \"Edit this by clicking the pencil icon…\", "
    "\"Demo Title\"): answer with exactly " + SKIP_TOKEN + " and nothing else. "
    "Never answer with an empty string: an empty answer is treated as a defective "
    "answer, not as a decline. "
    "(7) " + variant_clause() + " Respond with JSON only."
)

# Published per-MTok prices. Sonnet 5.5 is 2/10, not 3/15: the first version of
# this table overstated every Sonnet run by 1.5x (caught in review, #1924).
# Opus 5.5 and Fable 5.1 from the 2025-Epita mandate message (20/10) relayed in
# the same review — a model absent from this table FAILS the run below, because
# the previous `PRICES.get(model, (0, 0))` silently billed Opus and Fable at $0.
PRICES = {"claude-haiku-5-5": (0.10, 0.50), "claude-sonnet-5-5": (2.00, 10.00),
          "claude-opus-5-5": (4.00, 20.00), "claude-fable-5-1": (10.00, 50.00)}


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


def _normalise_record_endings(text):
    """Turn CRLF RECORD separators into LF, leaving quoted field content alone.

    Two different things wear the same bytes. A checkout with `core.autocrlf`
    materialises the whole file as CRLF — that is transport. This corpus also
    carries CRLF *inside* quoted HTML cells (2 523 of them, measured 23/10) — that
    is content, and `csv` preserves it on write. Universal-newline reading erases
    both and would hide a cell whose embedded CRLF silently became LF; normalising
    nothing makes the self-test red on every CRLF checkout (ai-01's finding).
    Quote state is tracked the CSV way: `""` inside a quoted field is an escaped
    quote, not a delimiter.
    """
    out, in_quote, i = [], False, 0
    while i < len(text):
        c = text[i]
        if c == '"':
            if in_quote and text[i + 1 : i + 2] == '"':
                out.append('""')
                i += 2
                continue
            in_quote = not in_quote
            out.append(c)
        elif not in_quote and text.startswith("\r\n", i):
            out.append("\n")
            i += 2
            continue
        else:
            out.append(c)
        i += 1
    return "".join(out)


def select(rows, apps, types, langs, overwrite, only=None, stats=None):
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
        if field in CSV_SOURCED.get(r.get("content_type") or "", ()):
            continue
        src = (r.get("fr") or "").strip()
        if not src:
            continue
        if DEMO.search(src):
            # A filter that does not say what it drops reads exactly like an
            # empty corpus. `stats` carries the count out to the run's own
            # selection line, so every pass declares its perimeter.
            if stats is not None:
                stats["demo"] = stats.get("demo", 0) + 1
            continue  # 2sxc demo item — see the DEMO comment above
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


def _norm_url(u, lang):
    """Erase exactly the two differences a correct localization is allowed to make.

    (a) A localized page is addressed through the cell's own language segment:
    `/en/Règles/…`, `/ru/Règles/…` (web2's W11, measured 23/10 — all 7 prefixes
    answer 200 in the right language). Only the *cell's own* prefix is erased, so
    a `/de/` or a foreign prefix is still a defect.
    (b) The FR source may spell the same path percent-encoded (`/R%C3%A8gles/…`)
    while another language keeps the literal character; both decode identically.
    """
    u = urllib.parse.unquote(u)
    if lang and lang != "fr":
        u = re.sub(r"^/" + re.escape(lang) + r"/", "/", u)
    return u


def validate(src, tgt, lang=None):
    """A cell is written only if the markup and the marker sequence survive intact.

    `lang` is the target language: pass it so that URL comparison tolerates that
    language's own page prefix (see `_norm_url`). Leaving it None keeps the strict
    byte-for-byte URL rule, which is what the counter-review importers rely on.
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
    if sorted(_norm_url(u, None) for u in urls(src)) != sorted(_norm_url(u, lang) for u in urls(tgt)):
        problems.append(f"URLs differ: {urls(src)} -> {urls(tgt)}")
    if markers(src) != markers(tgt):
        problems.append(f"pictograms differ: {markers(src)} -> {markers(tgt)}")
    return problems


def apply_answers(batch, out):
    """Turn one API answer into writes PLUS an exhaustive per-cell accounting.

    Every requested cell lands in exactly one class:

      * `written`  — validated and written into the row;
      * `skipped`  — the model answered the decline sentinel (`SKIP_TOKEN`);
      * `rejected` — an answer that did not validate; an empty string is one;
      * `absent`   — no answer at all: row key missing, value null, language
                     missing, or the whole batch unparseable.

    The caller asserts that the four add up to the number of cells it asked for.
    A total that is a lower bound reads exactly like a clean pass: the previous
    code did `out.get(key) or {}` and skipped a non-string value with `continue`,
    so a null key, a null value and a missing language were all invisible —
    precisely the shapes a model chooses when it declines (reserve 2 of the
    #1945 review).
    """
    res = {c: [] for c in CLASSES}
    for r, missing in batch:
        got = out.get(key_of(r))
        for l in missing:
            if not isinstance(got, dict):
                res["absent"].append((r["key"], r["field"], l, "row key absent from the answer"))
                continue
            v = got.get(l)
            if not isinstance(v, str):
                res["absent"].append((r["key"], r["field"], l, f"no string answer ({type(v).__name__})"))
            elif v.strip() == SKIP_TOKEN:
                res["skipped"].append((r["key"], r["field"], l, "model declined: not content"))
            else:
                bad = validate(r["fr"], v, l)
                if bad:
                    res["rejected"].append((r["key"], r["field"], l, "; ".join(bad)[:220]))
                else:
                    r[l] = v
                    res["written"].append((r["key"], r["field"], l, "ok"))
    return res


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
    stats = {}
    todo = select(rows, apps, types, langs, a.overwrite, a.only or None, stats)

    if a.self_test:
        # Serialization must round-trip the untouched pivot: what the reader
        # parsed, the writer reproduces.
        buf = io.StringIO(newline="")
        w = csv.DictWriter(buf, fieldnames=header, lineterminator="\n")
        w.writeheader()
        w.writerows(rows)
        # Read raw and normalise ONLY the record separators: the writer emits LF
        # between records but preserves CRLF inside quoted cells, so the two must
        # be treated differently (see `_normalise_record_endings`).
        orig = _normalise_record_endings(open(a.src, encoding="utf-8", newline="").read())
        assert buf.getvalue() == orig, "round-trip is not content-identical"
        # The normalisation itself, asserted on a literal with both kinds of CRLF.
        assert _normalise_record_endings('a,"x\r\ny"\r\nb\n') == 'a,"x\r\ny"\nb\n', \
            "record CRLF must normalise, in-quote CRLF must survive"
        # Inverse control, and it has to be a construct the WRITER would not
        # reproduce — otherwise the comparison is a no-op that can never fail
        # (a value change is invisible here, because `buf` is re-derived from
        # the file's own parse). A redundantly quoted field is such a construct:
        # it parses to `abc`, the writer emits `abc`, so the two differ.
        redundant = 'key,value\r\n"abc",def\r\n'
        parsed = list(csv.DictReader(io.StringIO(redundant, newline="")))
        b2 = io.StringIO(newline="")
        w2 = csv.DictWriter(b2, fieldnames=list(parsed[0].keys()), lineterminator="\n")
        w2.writeheader()
        w2.writerows(parsed)
        assert b2.getvalue() != _normalise_record_endings(redundant), \
            "a redundantly quoted field must be detected, or the round-trip proves nothing"
        # URL rule (web2 W11): tolerate the cell's own page prefix and the
        # percent-encoded vs literal spelling of the same path — and prove with
        # inverse controls that the tolerance did not become a blank cheque.
        fr_cell = '<a href="/R%C3%A8gles/details/x">t</a>'
        controls = [
            (validate(fr_cell, '<a href="/ru/Règles/details/x">т</a>', "ru"), False, "own prefix must pass"),
            (validate(fr_cell, '<a href="/Règles/details/x">т</a>', "ru"), False, "unprefixed target must pass"),
            (validate(fr_cell, '<a href="/de/Règles/details/x">т</a>', "ru"), True, "foreign prefix must fail"),
            (validate(fr_cell, '<a href="/ru/Règles/other">т</a>', "ru"), True, "different path must fail"),
            (validate(fr_cell, '<a href="/ru/Règles/details/x">т</a>'), True, "lang=None keeps the strict rule"),
            (validate(fr_cell, "<b>т</b>", "ru"), True, "tag drift must fail"),
            (validate("<p>🥇➜🏆</p>", "<p>🥈➜🏆</p>", "ru"), True, "pictogram drift must fail"),
            (validate("<p>🥇➜🏆</p>", "<p>🥇➜🏆</p>", "ru"), False, "identical markup must pass"),
        ]
        for problems, want_problems, why in controls:
            assert bool(problems) == want_problems, f"{why}: {problems}"
        # The demo filter must actually bite, and must not bite real content: a
        # filter that excludes nothing reads exactly like a clean corpus. Both
        # polarities are asserted, on the real `select()`.
        demo_row = {"key": "X|Title", "app_id": "1", "content_type": "Slide",
                    "field": "Title", "fr": "Demo Slide CR"}
        real_row = {"key": "Y|Title", "app_id": "1", "content_type": "Accordion",
                    "field": "Title", "fr": "Étudiant ou passionné de philosophie"}
        assert select([demo_row], {"1"}, set(), ["en"], False) == [], "demo row must be excluded"
        assert len(select([real_row], {"1"}, set(), ["en"], False)) == 1, "real row must be selected"
        # Anchoring, both ways (reserve 6). The four inverse controls are real
        # sentences of this corpus, excluded by the unanchored form because of the
        # substring `cta`; the anchored form must keep them. The demo material
        # must still be excluded — a widened tolerance with no positive control
        # would just be a filter that no longer filters.
        for keep in ("Un argument respectable", "La dictature de la majorité",
                     "Le spectateur", "pacta sunt servanda"):
            assert not DEMO.search(keep), f"DEMO wrongly excludes real content: {keep!r}"
        for drop in ("Demo Slide CR", "Demo Title", "Example which is still empty",
                     "Lorem ipsum dolor sit amet", "PLACEHOLDER"):
            assert DEMO.search(drop), f"DEMO must still exclude demo material: {drop!r}"
        # A field listed in both allowlists is a *reading* ambiguity, not an
        # evaluation-order dependency: `NEVER or field not in TRANSLATABLE`
        # short-circuits deterministically and NEVER wins either way. Measured
        # 23/10 on PlaceholderMissingImage — `select()` always excluded it, while
        # a reader of TRANSLATABLE alone counted 7 cells as translatable.
        assert not (TRANSLATABLE & NEVER), f"field in both lists: {TRANSLATABLE & NEVER}"
        # The decline is a sentinel, and an empty answer is a defect (reserve 1):
        # `validate()` refuses it, and the write path below proves the caller no
        # longer short-circuits that refusal.
        assert validate("Some FR text", "", "en") == ["empty translation"], "empty must not validate"
        assert "(6b)" in PROMPT and SKIP_TOKEN in PROMPT, "the prompt must name the decline token"
        assert "Never answer with an empty string" in PROMPT, "the prompt must say empty is a defect"

        # --- behaviour guard (reserve 5) -----------------------------------
        # A fabricated answer is injected where the API answer would go, and the
        # accounting is read back. Pinning a label ("(6b)" in PROMPT) stays green
        # if the rule is rewritten with the opposite meaning; this does not. Each
        # of the four classes is exercised, and the two that must never write are
        # asserted to write nothing.
        def fab():
            return [({"key": "K|Title", "field": "Title", "fr": "Bonjour"}, ["en", "ru"])]

        empty = apply_answers(fab(), {"K|Title": {"en": ""}})
        assert empty["written"] == [], "an empty answer must never be written"
        assert len(empty["rejected"]) == 1, "an empty answer must be rejected"
        assert empty["rejected"][0][:3] == ("K|Title", "Title", "en"), "and rejected by name"
        declined = apply_answers(fab(), {"K|Title": {"en": SKIP_TOKEN, "ru": SKIP_TOKEN}})
        assert declined["written"] == [] and len(declined["skipped"]) == 2, "the sentinel must skip, not write"
        missing_lang = apply_answers(fab(), {"K|Title": {"en": "Hello"}})
        assert len(missing_lang["absent"]) == 1, "a missing language must be counted absent"
        assert missing_lang["absent"][0][2] == "ru", "and named as the missing one"
        nulled = apply_answers(fab(), {"K|Title": {"en": None, "ru": "Привет"}})
        assert len(nulled["absent"]) == 1 and nulled["absent"][0][2] == "en", "null must not vanish"
        assert len(nulled["written"]) == 1, "the other language still writes"
        no_key = apply_answers(fab(), {})
        assert len(no_key["absent"]) == 2, "a missing row key must count every cell absent"
        good = apply_answers(fab(), {"K|Title": {"en": "Hello", "ru": "Привет"}})
        assert len(good["written"]) == 2, "a valid answer must write (positive control)"
        # Exhaustive by construction: the four classes always partition the
        # request, and that is the invariant the run asserts batch by batch.
        for res, want in ((empty, 2), (declined, 2), (missing_lang, 2), (nulled, 2),
                          (no_key, 2), (good, 2)):
            assert sum(len(res[c]) for c in CLASSES) == want, f"accounting is not exhaustive: {res}"
        print(f"self-test OK: round-trip content-identical (LF tree and CRLF checkout, "
              f"in-cell CRLF preserved), {len(rows)} rows, {len(todo)} to translate, "
              f"{stats.get('demo', 0)} demo rows excluded; URL rule accepts the own prefix and "
              f"rejects a foreign one; DEMO anchored on 4 inverse controls; the 4 accounting "
              f"classes partition 6 fabricated answers")
        # `--self-test` is TERMINAL. It used to fall through to the selection and,
        # without `--dry-run`, to a real paid pass: measured 23/10 on ai-01, one
        # run of that shape spent $0.17 on 413 cells and wrote nothing. A control
        # that only asserts is not a control on the control flow — this return is,
        # and it is measured by running the tool with a key path that does not
        # exist: the exit stays 0 only if no key is ever read.
        print("self-test only — the run stops here, before any key read or API call")
        return 0

    if a.limit:
        todo = todo[: a.limit]
    cells = sum(len(m) for _, m in todo)
    print(f"selection: {len(todo)} rows, {cells} cells, demo-excluded={stats.get('demo', 0)} rows, "
          f"apps={sorted(apps) or 'ALL'} types={sorted(types) or 'ALL'} langs={langs}")
    if a.dry_run or not todo:
        for r, missing in todo[:10]:
            print(f"  {r['app_id']}/{r['content_type']}/{r['field']}: {r['fr'][:50]!r} -> {missing}")
        return 0

    if a.model not in PRICES:
        raise SystemExit(f"unknown model {a.model!r}: no published price, refusing to run "
                         f"(priced models: {', '.join(sorted(PRICES))})")
    key = open(a.key_path, encoding="utf-8").read()
    assert key and "\n" not in key.strip(), "key file must be a single line"
    key = key.strip()
    pin, pout = PRICES[a.model]
    tin = tout = 0
    requested = 0
    tally = {c: [] for c in CLASSES}
    for i in range(0, len(todo), a.batch):
        batch = todo[i : i + a.batch]
        requested += sum(len(m) for _, m in batch)
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
            # A batch with no usable answer loses no cell: every one of them is
            # counted absent (reserve 2 — the old `continue` dropped the batch).
            for r, missing in batch:
                for l in missing:
                    tally["absent"].append((r["key"], r["field"], l, "batch answer was not JSON"))
        else:
            got = apply_answers(batch, out)
            for c in CLASSES:
                tally[c].extend(got[c])
        counted = sum(len(tally[c]) for c in CLASSES)
        assert counted == requested, (
            f"accounting broken at batch {i // a.batch + 1}: "
            f"{ {c: len(tally[c]) for c in CLASSES} } sum to {counted}, {requested} requested")
        print(f"  batch {i // a.batch + 1}: {len(batch)} rows, "
              f"written={len(tally['written'])} skipped={len(tally['skipped'])} "
              f"rejected={len(tally['rejected'])} absent={len(tally['absent'])}, "
              f"in={tin} out={tout}")

    # Declined and absent cells are semantics the operator must be able to audit,
    # not a silent remainder: a decline is a judgement by the model, and a false
    # negative there costs a cell that never gets translated (reserve 3).
    for cls in ("rejected", "skipped", "absent"):
        for key, field, lang, why in tally[cls]:
            print(f"  {cls.upper()}", key[:8], field, lang, "::", why, file=sys.stderr)
    counted = sum(len(tally[c]) for c in CLASSES)
    assert counted == requested, (
        f"accounting broken: { {c: len(tally[c]) for c in CLASSES} } sum to "
        f"{counted}, {requested} requested")
    spend = tin / 1e6 * pin + tout / 1e6 * pout
    print(f"TOTAL cells requested={requested} written={len(tally['written'])} "
          f"skipped={len(tally['skipped'])} rejected={len(tally['rejected'])} "
          f"absent={len(tally['absent'])} tokens in={tin} out={tout} "
          f"spend=${spend:.4f} ({a.model})")
    if a.out:
        save(a.out, rows, header)
        print("written", a.out)
    return 0


if __name__ == "__main__":
    sys.exit(main())