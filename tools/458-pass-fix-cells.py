"""Review cascade-drift candidates with a model and write the kept cells back.

Input is the worklist produced by `458-cascade-drift-scan.py`: groups of
(row, field) whose French was tightened while the translation stayed put. Each
group is one API call, all target languages side by side — a review that sees
only one language cannot tell a drift from a legitimate localization.

The acceptance screen is deliberately narrow. The previous passes measured that
a model asked to "improve" translations returns mostly churn (synonym for
synonym: 19 of 25 rejected on Scenarii pass 2), so:

  * `kind` must be DRIFT (the target carries content the French no longer has)
    or ERROR (typo, agreement, mistranslation). STYLE is refused outright;
  * a FIX is only written if it survives `structure_ok()`;
  * the `carte` column is never touched, and rows are written through a
    field-span splitter — this corpus does not round-trip through the csv
    module byte-for-byte (it mixes quoted and bare fields), so a whole-file
    rewrite would show hundreds of phantom changes.

Usage
  python -I tools/458-pass-fix-cells.py --worklist w.csv --csv "Cards/..." --dry-run
  python -I tools/458-pass-fix-cells.py --worklist w.csv --csv "Cards/..." --apply
"""
import argparse
import csv
import io
import json
import re
import sys
import urllib.error
import urllib.request

LANGS = ["en", "ru", "pt", "es", "ar", "fa", "zh"]
VARIANTS = {
    "en": "en-US, American English", "pt": "pt-BR, Brazilian Portuguese",
    "es": "es-ES, Peninsular Spanish", "zh": "zh-CN, Simplified Chinese",
    "fa": "fa-IR, Iranian Persian", "ru": "ru-RU, Russian", "ar": "ar, Modern Standard Arabic",
}
PRICES = {"claude-haiku-5-5": (0.10, 0.50), "claude-sonnet-5-5": (2.00, 10.00),
          "claude-opus-5-5": (4.00, 20.00), "claude-fable-5-1": (10.00, 50.00)}

SYSTEM = (
    "You review translations of an educational card game about logical fallacies. "
    "For one row you receive the FRENCH source as it was, the FRENCH source as it is "
    "now (it was tightened), and the current translation in each target language, "
    "which has NOT been updated since. Some of those translations therefore still "
    "carry content the French no longer has.\n"
    "Classify every language and answer with a JSON object mapping the language code "
    "to {verdict, kind, reason, fixed}.\n"
    "kind is one of:\n"
    "  NONE  — the current translation is acceptable. Use it when the extra matter is a "
    "legitimate localization: an explanation the target language needs (a pun that does "
    "not survive, a culture-specific reference), or when the French rewrite did not "
    "change the substance.\n"
    "  DRIFT — the translation carries content the CURRENT French no longer has. `fixed` "
    "is the translation of the current French, in the same style and length register.\n"
    "  ERROR — a defect independent of the rewrite: typo, agreement, wrong word, a "
    "sentence fragment where the French is a full sentence. `fixed` repairs it.\n"
    "  STYLE — a matter of taste. NEVER answer STYLE; a synonym for a synonym is not an "
    "improvement and will be discarded.\n"
    "verdict is \"OK\" for kind NONE and \"FIX\" otherwise. `reason` is at most 140 "
    "characters and must name the removed or added content. Translate the CURRENT French "
    "only: never reintroduce the old wording, never add material of your own.\n"
    "Language variants are pinned and override your default register: "
    + "; ".join("%s = %s" % kv for kv in VARIANTS.items())
    + ". Keep the typographic conventions of the current cell (quotation marks, dashes). "
    "Respond with JSON only."
)


def structure_ok(old, new):
    """Reject a FIX that changes more than the prose: pictograms must survive."""
    def markers(s):
        return sorted(c for c in s if 0x2190 <= ord(c) <= 0x2BFF or ord(c) >= 0x1F000)
    if not new or not new.strip():
        return "empty"
    if markers(old) != markers(new):
        return "pictograms differ"
    if len(new) > 3 * max(len(old), 40):
        return "suspiciously long"
    return None


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
                    "verdict": {"type": "string", "enum": ["OK", "FIX"]},
                    "kind": {"type": "string", "enum": ["NONE", "DRIFT", "ERROR", "STYLE"]},
                    "reason": {"type": "string"}, "fixed": {"type": "string"}},
                    "required": ["verdict", "kind", "reason", "fixed"], "additionalProperties": False}
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


def split_row(line):
    """Return [(start, end)] spans of the fields of one CSV line (CRLF stripped)."""
    spans, start, inq, i = [], 0, False, 0
    while i < len(line):
        ch = line[i]
        if inq:
            if ch == '"':
                if i + 1 < len(line) and line[i + 1] == '"':
                    i += 2
                    continue
                inq = False
        else:
            if ch == '"':
                inq = True
            elif ch == ',':
                spans.append((start, i))
                start = i + 1
        i += 1
    spans.append((start, len(line)))
    return spans


def render(value, was_quoted):
    needs = any(c in value for c in ',"\r\n') or value != value.strip()
    if was_quoted or needs:
        return '"' + value.replace('"', '""') + '"'
    return value


def _apply(csv_path, proposals):
    """Write the kept cells through field spans, never through a whole-file rewrite."""
    raw = open(csv_path, "rb").read()
    bom = raw[:3] == b"\xef\xbb\xbf"
    lines = raw.decode("utf-8-sig").split("\r\n")
    header = next(csv.reader([lines[0]]))
    idx = {name: i for i, name in enumerate(header)}
    pk_i = idx["PK"]
    by_pk = {}
    for p in proposals:
        by_pk.setdefault(p["pk"], []).append(p)
    touched, mismatched = 0, 0
    for li in range(1, len(lines)):
        if not lines[li]:
            continue
        spans = split_row(lines[li])
        pk = lines[li][spans[pk_i][0]:spans[pk_i][1]].strip('"')
        if pk not in by_pk:
            continue
        for p in by_pk[pk]:
            ci = idx[p["col"]]
            s, e = spans[ci]
            cur = lines[li][s:e]
            was_quoted = cur.startswith('"') and cur.endswith('"') and len(cur) > 1
            parsed = next(csv.reader([cur]))[0] if cur else ""
            if parsed != p["old"]:
                print("  !! span mismatch PK %s %s: file=%r proposal=%r"
                      % (pk, p["col"], parsed[:60], p["old"][:60]), file=sys.stderr)
                mismatched += 1
                continue
            repl = render(p["new"], was_quoted)
            delta = len(repl) - (e - s)
            lines[li] = lines[li][:s] + repl + lines[li][e:]
            for k in range(ci + 1, len(spans)):
                spans[k] = (spans[k][0] + delta, spans[k][1] + delta)
            touched += 1
    open(csv_path, "wb").write((b"\xef\xbb\xbf" if bom else b"") + "\r\n".join(lines).encode("utf-8"))
    print("applied %d cell(s) to %s (%d mismatched)" % (touched, csv_path, mismatched))
    return touched


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--worklist", default=None)
    ap.add_argument("--csv", required=True)
    ap.add_argument("--model", default="claude-opus-5-5")
    ap.add_argument("--base", default="https://api.anthropic.com/v1")
    ap.add_argument("--key-path", default=".keys/anthropic-key.txt")
    ap.add_argument("--max-tokens", type=int, default=6000)
    ap.add_argument("--groups", type=int, default=0, help="limit the number of (row, field) groups")
    ap.add_argument("--dry-run", action="store_true")
    ap.add_argument("--apply", action="store_true")
    ap.add_argument("--apply-from", default=None,
                    help="apply the cells of a proposals CSV instead of calling the model")
    ap.add_argument("--proposals", default=None)
    a = ap.parse_args()

    if a.apply_from:
        with io.open(a.apply_from, encoding="utf-8", newline="") as f:
            loaded = list(csv.DictReader(f))
        print("applying %d proposal(s) from %s" % (len(loaded), a.apply_from))
        _apply(a.csv, loaded)
        return 0
    if not a.worklist:
        raise SystemExit("--worklist is required unless --apply-from is given")

    work = list(csv.DictReader(io.open(a.worklist, encoding="utf-8", newline="")))
    groups = {}
    for w in work:
        groups.setdefault((w["pk"], w["field"]), []).append(w)
    ordered = sorted(groups.items(), key=lambda kv: -max(int(x["loss"]) for x in kv[1]))
    if a.groups:
        ordered = ordered[: a.groups]
    print("groups: %d (of %d in the worklist) — %d candidate cells"
          % (len(ordered), len(groups), sum(len(v) for _, v in ordered)))

    if a.dry_run:
        for (pk, field), v in ordered:
            print("  PK %-5s %-8s loss=%s langs=%s" % (pk, field, v[0]["loss"],
                                                       ",".join(sorted(x["lang"] for x in v))))
        return 0

    if a.model not in PRICES:
        raise SystemExit("unknown model %r: no published price" % a.model)
    key = open(a.key_path, encoding="utf-8").read().strip()
    pin, pout = PRICES[a.model]
    tin = tout = 0
    proposals, rejects = [], []

    for (pk, field), cells in ordered:
        head = cells[0]
        payload = {
            "row_pk": pk, "field": field,
            "french_before": head["fr_old"], "french_now": head["fr_new"],
            "targets": sorted(x["lang"] for x in cells),
            "current_translations": {x["lang"]: x["target"] for x in cells},
        }
        data = call_api(a.base, key, a.model, payload, a.max_tokens)
        usage = data.get("usage") or {}
        tin += usage.get("prompt_tokens", 0)
        tout += usage.get("completion_tokens", 0)
        try:
            out = json.loads(data["choices"][0]["message"]["content"])
        except (json.JSONDecodeError, KeyError):
            print("  !! non-JSON on PK %s %s" % (pk, field), file=sys.stderr)
            continue
        for x in cells:
            l = x["lang"]
            v = out.get(l) or {}
            kind, verdict = v.get("kind", "?"), v.get("verdict", "?")
            fixed = (v.get("fixed") or "").strip()
            reason = (v.get("reason") or "").strip()
            if kind not in ("DRIFT", "ERROR") or verdict != "FIX":
                rejects.append(dict(pk=pk, col=x["col"], lang=l, kind=kind,
                                    reason="not in scope (%s/%s) %s" % (kind, verdict, reason[:110])))
                continue
            bad = structure_ok(x["target"], fixed)
            if bad:
                rejects.append(dict(pk=pk, col=x["col"], lang=l, kind=kind,
                                    reason="screen: %s" % bad))
                continue
            proposals.append(dict(pk=pk, col=x["col"], lang=l, kind=kind,
                                  old=x["target"], new=fixed, reason=reason[:140]))

    print("\nproposals: %d | rejected: %d" % (len(proposals), len(rejects)))
    for p in proposals:
        print("  KEEP PK %-5s %-12s [%s] %s" % (p["pk"], p["col"], p["kind"], p["reason"][:90]))
    for r in rejects:
        print("  drop PK %-5s %-12s [%s] %s" % (r["pk"], r["col"], r["kind"], r["reason"][:90]))

    spend = tin / 1e6 * pin + tout / 1e6 * pout
    print("\ntokens in=%d out=%d spend=$%.4f (%s)" % (tin, tout, spend, a.model))
    if a.proposals:
        with io.open(a.proposals, "w", encoding="utf-8", newline="") as f:
            w = csv.DictWriter(f, fieldnames=["pk", "col", "lang", "kind", "old", "new", "reason"],
                               lineterminator="\n")
            w.writeheader()
            w.writerows(proposals)
        print("written", a.proposals)

    if a.apply and proposals:
        _apply(a.csv, proposals)
    return 0


if __name__ == "__main__":
    sys.exit(main())
