"""Counter-review of proposed cell fixes by a superior model.

The proposer sees the FR that was and the FR that is, and is told to follow the
current French. That instruction is exactly what a reviewer must test: a model
told to remove stale content will also remove content the French never had, or
reintroduce the old wording while claiming to follow the new one.

One call per (row, field) group, all target languages side by side, so a verdict
is given with the siblings in view. Verdicts:

  OK      — the proposal is faithful; keep it as it stands
  FIX     — keep it, with the reviewer's own replacement
  REJECT  — drop the proposal (churn, added material, mistranslation, regression)

The output is (a) a markdown table ready for the PR body and (b) an accepted
proposals CSV that `458-pass-fix-cells.py --apply-from` can write.

Usage
  python -I tools/458-counterreview-cells.py --proposals p.csv --csv "Cards/..." \\
      --model claude-fable-5-1 --out accepted.csv
"""
import argparse
import csv
import importlib.util
import io
import json
import sys
import urllib.error
import urllib.request

PRICES = {"claude-opus-5-5": (4.00, 20.00), "claude-fable-5-1": (10.00, 50.00),
          "claude-sonnet-5-5": (2.00, 10.00)}

VARIANTS = {
    "en": "en-US, American English", "pt": "pt-BR, Brazilian Portuguese",
    "es": "es-ES, Peninsular Spanish", "zh": "zh-CN, Simplified Chinese",
    "fa": "fa-IR, Iranian Persian", "ru": "ru-RU, Russian", "ar": "ar, Modern Standard Arabic",
}

SYSTEM = (
    "You are the counter-reviewer of a translation pass on an educational card game "
    "about logical fallacies. You receive the CURRENT French source of one field, the "
    "translation as it was, the translation a previous model proposes, and that model's "
    "reason. The French was tightened at some point and the proposal intends to bring "
    "the translation in line with the current French.\n"
    "Answer with a JSON object mapping each language code to {verdict, reason, fixed}.\n"
    "verdict:\n"
    "  OK     — the proposal faithfully renders the CURRENT French, adds nothing, drops "
    "nothing that carries meaning, and reads naturally in the pinned variant.\n"
    "  FIX    — the intent is right but the wording is wrong (grammar, wrong word, a "
    "clumsy calque). Put your corrected version in `fixed`, which must itself render the "
    "CURRENT French and nothing more.\n"
    "  REJECT — the proposal must be dropped: it removes content the current French still "
    "carries, it invents material, it is a synonym-for-synonym change with no fidelity "
    "gain, or it destroys a pun/explanation the target language needs.\n"
    "Judge fidelity to the CURRENT French only — never reward length, never reward change. "
    "A proposal identical to the previous cell is a REJECT. `reason` is at most 140 "
    "characters and must name the concrete point.\n"
    "Language variants are pinned and override your default register: "
    + "; ".join("%s = %s" % kv for kv in VARIANTS.items())
    + ". Respond with JSON only."
)


def load_module(path="tools/458-pass-fix-cells.py"):
    spec = importlib.util.spec_from_file_location("pf", path)
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    return mod


def call_api(base, key, model, payload, max_tokens):
    langs = payload["targets"]
    body = {
        "model": model,
        "messages": [{"role": "system", "content": SYSTEM},
                     {"role": "user", "content": json.dumps(payload, ensure_ascii=False)}],
        "max_tokens": max_tokens,
        "response_format": {"type": "json_schema", "json_schema": {
            "name": "verdicts", "strict": True,
            "schema": {
                "type": "object",
                "properties": {l: {"type": "object", "properties": {
                    "verdict": {"type": "string", "enum": ["OK", "FIX", "REJECT"]},
                    "reason": {"type": "string"}, "fixed": {"type": "string"}},
                    "required": ["verdict", "reason", "fixed"], "additionalProperties": False}
                    for l in langs},
                "required": list(langs), "additionalProperties": False}}},
    }
    req = urllib.request.Request(
        base.rstrip("/") + "/chat/completions",
        data=json.dumps(body).encode("utf-8"),
        headers={"Content-Type": "application/json", "Authorization": "Bearer " + key},
        method="POST")
    try:
        with urllib.request.urlopen(req, timeout=300) as resp:
            return json.loads(resp.read().decode("utf-8"))
    except urllib.error.HTTPError as e:
        raise SystemExit("HTTP %d: %s" % (e.code, e.read().decode("utf-8", "replace")[:800]))


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--proposals", required=True)
    ap.add_argument("--csv", required=True)
    ap.add_argument("--model", default="claude-fable-5-1")
    ap.add_argument("--base", default="https://api.anthropic.com/v1")
    ap.add_argument("--key-path", default=".keys/anthropic-key.txt")
    ap.add_argument("--max-tokens", type=int, default=6000)
    ap.add_argument("--out", default=None, help="accepted proposals CSV")
    ap.add_argument("--table", default=None, help="markdown verdict table")
    a = ap.parse_args()

    pf = load_module()
    props = list(csv.DictReader(io.open(a.proposals, encoding="utf-8", newline="")))
    corpus = {}
    for r in csv.DictReader(io.open(a.csv, encoding="utf-8-sig", newline="")):
        corpus[r["PK"]] = r
    groups = {}
    for p in props:
        groups.setdefault((p["pk"], p["col"].rsplit("_", 1)[0]), []).append(p)
    print("counter-reviewing %d proposal(s) in %d group(s)" % (len(props), len(groups)))

    if a.model not in PRICES:
        raise SystemExit("unknown model %r" % a.model)
    key = open(a.key_path, encoding="utf-8").read().strip()
    pin, pout = PRICES[a.model]
    tin = tout = 0
    verdicts = []

    for (pk, base_field), cells in sorted(groups.items()):
        row = corpus[pk]
        payload = {
            "row_pk": pk, "field": base_field,
            "french_now": row.get("%s_fr" % base_field) or "",
            "targets": sorted(c["lang"] for c in cells),
            "cells": {c["lang"]: {"before": c["old"], "proposed": c["new"],
                                  "proposer_reason": c["reason"]} for c in cells},
        }
        data = call_api(a.base, key, a.model, payload, a.max_tokens)
        usage = data.get("usage") or {}
        tin += usage.get("prompt_tokens", 0)
        tout += usage.get("completion_tokens", 0)
        try:
            out = json.loads(data["choices"][0]["message"]["content"])
        except (json.JSONDecodeError, KeyError):
            print("  !! non-JSON on PK %s %s" % (pk, base_field), file=sys.stderr)
            continue
        for c in cells:
            v = out.get(c["lang"]) or {}
            verdict = v.get("verdict", "?")
            reason = (v.get("reason") or "").strip()[:140]
            final = c["new"]
            if verdict == "FIX":
                cand = (v.get("fixed") or "").strip()
                bad = pf.structure_ok(c["old"], cand) if cand else "no replacement given"
                if bad:
                    verdict, reason = "REJECT", "FIX without a usable replacement (%s)" % bad
                else:
                    final = cand
            status = {"OK": "kept", "FIX": "kept (corrected)", "REJECT": "dropped"}.get(verdict, "?")
            verdicts.append(dict(pk=pk, col=c["col"], lang=c["lang"], verdict=verdict,
                                 status=status, reason=reason, old=c["old"], new=final))

    print("\n| Rangée | Champ | Langue | Verdict | Motif |")
    print("|---|---|---|---|---|")
    for v in verdicts:
        print("| `%s` | %s | %s | %s | %s |" % (v["pk"], v["col"], v["lang"],
              "**%s**" % v["verdict"] if v["verdict"] != "OK" else "OK", v["reason"]))
    kept = [v for v in verdicts if v["verdict"] in ("OK", "FIX")]
    spend = tin / 1e6 * pin + tout / 1e6 * pout
    print("\n%d kept · %d dropped · tokens in=%d out=%d spend=$%.4f (%s)"
          % (len(kept), len(verdicts) - len(kept), tin, tout, spend, a.model))

    if a.out:
        with io.open(a.out, "w", encoding="utf-8", newline="") as f:
            w = csv.DictWriter(f, fieldnames=["pk", "col", "lang", "kind", "old", "new", "reason"],
                               lineterminator="\n")
            w.writeheader()
            for v in kept:
                w.writerow(dict(pk=v["pk"], col=v["col"], lang=v["lang"], kind="REVIEWED",
                                old=v["old"], new=v["new"], reason=v["reason"]))
        print("accepted ->", a.out)
    if a.table:
        with io.open(a.table, "w", encoding="utf-8", newline="") as f:
            f.write("| Rangée | Champ | Langue | Verdict | Motif |\n|---|---|---|---|---|\n")
            for v in verdicts:
                f.write("| `%s` | %s | %s | %s | %s |\n" % (v["pk"], v["col"], v["lang"],
                        v["verdict"], v["reason"]))
        print("table ->", a.table)
    return 0


if __name__ == "__main__":
    sys.exit(main())
