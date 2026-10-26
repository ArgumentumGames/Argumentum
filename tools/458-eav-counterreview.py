"""Counter-review of EAV pivot cells by a superior model — one call per row.

Why this file exists: `variant_clause()` in `458-eav-translate.py` was written to
have exactly one source of truth for the pinned language variants, but nothing
imported it (web2's finding on #1940). This is that consumer: the review prompt
is built from the same module, so a variant pinned for translation is pinned for
review too — the #1938 failure was a review that blessed European Portuguese
because the variant was nowhere stated.

It also refuses the two shapes the translation pass guarantees and a reviewer can
therefore check cheaply:
  * URL rule (web2 W11): a localized page carries the cell's own `/xx/` segment,
    the FR source may spell the same path percent-encoded — both erase to the
    same path, a foreign `/de/` does not;
  * pictograms are game notation and must survive in the same order.

Usage
  python -I tools/458-eav-counterreview.py --rev HEAD \\
      --only 'DEA9410D.*RuleMemoCardDownload|2538A3F0.*\\|Text' --langs ar,fa,ru,pt,zh

Prints a per-cell markdown table (ready for a PR body) and, with --out, a CSV.
The API key is read from a file and never printed.
"""
import argparse
import csv
import importlib.util
import io
import json
import re
import sys
import urllib.error
import urllib.request

TOOL = "tools/458-eav-translate.py"


def load_tool(path=TOOL):
    spec = importlib.util.spec_from_file_location("eav", path)
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    return mod


SYSTEM = (
    "You are the counter-reviewer of a localization pivot for a public educational "
    "game site. You receive, for one row, the French source cell and the target cells "
    "that were just changed. Judge every target cell and answer with a JSON object "
    "mapping each language code to {verdict, reason, fixed}.\n"
    "Criteria, in order: (1) SENSE — the target says what the French says, in the "
    "register of a game site; no added or dropped information. (2) GRAMMAR — agreement, "
    "contractions, articles, natural word order in that language. (3) TERMINOLOGY — a "
    "term already used by the same site for the same object must be reused (the other "
    "languages of the row, and the corpus convention named in the prompt, are the "
    "reference). (4) STRUCTURE — every HTML tag, attribute and href must be preserved; "
    "a localized page link carries the cell's own /xx/ segment (e.g. /ar/Règles/…); "
    "pictograms and arrows are game notation and must survive in the same order.\n"
    "verdict is \"OK\" or \"FIX\". Answer FIX only when you can state the replacement: "
    "put the full corrected cell in `fixed`, byte-compatible with the structure rules. "
    "`reason` is at most 140 characters. Do not comment on the French source."
)

PRICE = {"claude-opus-5-5": (4.00, 20.00), "claude-fable-5-1": (10.00, 50.00)}


def call_api(base, key, model, payload, max_tokens):
    body = {
        "model": model,
        "messages": [
            {"role": "system", "content": SYSTEM},
            {"role": "user", "content": json.dumps(payload, ensure_ascii=False)},
        ],
        # The compat endpoint refuses `temperature` on this model — never send it.
        "max_tokens": max_tokens,
        "response_format": {
            "type": "json_schema",
            "json_schema": {
                "name": "verdicts",
                "strict": True,
                "schema": {
                    "type": "object",
                    "properties": {
                        l: {
                            "type": "object",
                            "properties": {
                                "verdict": {"type": "string", "enum": ["OK", "FIX"]},
                                "reason": {"type": "string"},
                                "fixed": {"type": "string"},
                            },
                            "required": ["verdict", "reason", "fixed"],
                            "additionalProperties": False,
                        }
                        for l in payload["targets"]
                    },
                    "required": list(payload["targets"]),
                    "additionalProperties": False,
                },
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
            return json.loads(resp.read().decode("utf-8"))
    except urllib.error.HTTPError as e:
        raise SystemExit(f"HTTP {e.code}: {e.read().decode('utf-8', 'replace')[:800]}")


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--in", dest="src", default="docs/dnn-localization/eav-content.csv")
    ap.add_argument("--rev", default="HEAD", help="git revision holding the pre-change pivot")
    ap.add_argument("--baseline", default=None,
                    help="pivot path inside --rev (defaults to --in); needed when --in is a scratch copy")
    ap.add_argument("--only", default="", help="regex on the row key")
    ap.add_argument("--langs", default="en,ru,pt,es,ar,fa,zh")
    ap.add_argument("--model", default="claude-opus-5-5")
    ap.add_argument("--base", default="https://api.anthropic.com/v1")
    ap.add_argument("--key-path", default=".keys/anthropic-key.txt")
    ap.add_argument("--max-tokens", type=int, default=4096)
    ap.add_argument("--out", default=None)
    a = ap.parse_args()

    eav = load_tool()
    langs = [x.strip() for x in a.langs.split(",") if x.strip()]
    cur = eav.load(a.src)
    old = eav.load(a.src) if a.rev == "WORKTREE" else None
    if old is None:
        import subprocess

        raw = subprocess.run(["git", "show", f"{a.rev}:{a.baseline or a.src}"],
                             capture_output=True).stdout
        assert raw, f"cannot read {a.src} at {a.rev}"
        old = list(csv.DictReader(io.StringIO(raw.decode("utf-8"), newline="")))
    before = {r["key"]: r for r in old}

    rows = []
    for r in cur:
        if a.only and not re.search(a.only, r["key"]):
            continue
        b = before.get(r["key"])
        if b is None:
            continue
        changed = [l for l in langs if (r.get(l) or "") != (b.get(l) or "")]
        if changed:
            rows.append((r, b, changed))
    if not rows:
        print("no changed cell in the selection — nothing to review")
        return 0

    if a.model not in PRICE:
        raise SystemExit(f"unknown model {a.model!r}: no published price")
    key = open(a.key_path, encoding="utf-8").read()
    assert key and "\n" not in key.strip(), "key file must be a single line"
    key = key.strip()
    pin, pout = PRICE[a.model]
    tin = tout = 0
    verdicts = []

    for r, b, changed in rows:
        payload = {
            "row": r["key"],
            "field_name": r.get("field"),
            "variant_pins": eav.variant_clause(),
            "source_fr": r.get("fr") or "",
            "targets": changed,
            "changed_cells": {l: {"before": b.get(l) or "", "after": r.get(l) or ""} for l in changed},
            "sibling_languages": {
                l: (r.get(l) or "") for l in langs if l not in changed and (r.get(l) or "")
            },
        }
        data = call_api(a.base, key, a.model, payload, a.max_tokens)
        usage = data.get("usage") or {}
        tin += usage.get("prompt_tokens", 0)
        tout += usage.get("completion_tokens", 0)
        try:
            out = json.loads(data["choices"][0]["message"]["content"])
        except (json.JSONDecodeError, KeyError):
            print("  !! non-JSON answer on", r["key"], file=sys.stderr)
            continue
        for l in changed:
            v = out.get(l) or {}
            fixed = (v.get("fixed") or "").strip()
            # A FIX is admissible only if it survives the same structural rule the
            # translator's output had to survive — same language, same rule.
            if v.get("verdict") == "FIX" and fixed:
                bad = eav.validate(r.get("fr") or "", fixed, l)
                if bad:
                    verdicts.append((r["key"], r.get("field"), l, "REJECTED", "; ".join(bad)[:140], ""))
                    continue
            verdicts.append((r["key"], r.get("field"), l, v.get("verdict", "?"),
                             (v.get("reason") or "")[:140], fixed))

    print("\n| Rangée | Champ | Langue | Verdict | Motif |")
    print("|---|---|---|---|---|")
    for key_, field, l, v, why, _ in verdicts:
        print(f"| `{key_.split('|')[0][:8]}` | {field} | {l} | {'**' + v + '**' if v != 'OK' else v} | {why} |")
    ok = sum(1 for v in verdicts if v[3] == "OK")
    fix = sum(1 for v in verdicts if v[3] == "FIX")
    rej = sum(1 for v in verdicts if v[3] == "REJECTED")
    spend = tin / 1e6 * pin + tout / 1e6 * pout
    print(f"\n{ok} OK · {fix} FIX · {rej} REJECTED — tokens in={tin} out={tout} spend=${spend:.4f} ({a.model})")
    if a.out:
        with io.open(a.out, "w", encoding="utf-8", newline="") as f:
            w = csv.writer(f, lineterminator="\n")
            w.writerow(["key", "field", "lang", "verdict", "reason", "fixed"])
            w.writerows(verdicts)
        print("written", a.out)
    return 0


if __name__ == "__main__":
    sys.exit(main())
