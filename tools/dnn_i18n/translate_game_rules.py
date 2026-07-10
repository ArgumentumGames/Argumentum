#!/usr/bin/env python3
"""
Translate DNN Game Rule prose (FR source) -> 7 target languages via gpt-5.5.

Source: 2sxc Game Rule content-type export (5 published entities, 23 populated prose
cells + 5 titles). Preserves HTML structure/entities; translates visible text content only.
Output: re-import-ready JSON (per entity x field x {fr + 7 langs}).

gpt-5.5 API specifics (verified 2026-06-16, #499 Virtues pilot):
  - OpenAI Chat Completions endpoint (/v1/chat/completions).
  - NO `temperature` (HTTP 400 on reasoning models). Use `max_completion_tokens`
    (NOT `max_tokens`). `reasoning_effort=low`.
  - Reasoning tokens eat the completion budget -> max_completion_tokens is sized to
    field length (floor 1500, cap 8000).
  - OpenAI direct key (sk-proj-) is LIVE; OpenRouter (openai/gpt-5.5) is automatic
    fallback on 401/429.

Resume: the output JSON doubles as a cache. Re-running skips any cell-lang already
present, so a long run interrupted mid-way continues where it stopped.

Usage:
  python translate_game_rules.py --export <path.json> --out <out.json> --smoke   # 1 cell x 7 langs
  python translate_game_rules.py --export <path.json> --out <out.json> --lang en  # 1 lang, all cells
  python translate_game_rules.py --export <path.json> --out <out.json> --all      # full pass
"""
import json, os, sys, time, argparse, urllib.request, urllib.error

DEFAULT_EXPORT = os.environ.get(
    "DNN_2SXC_EXPORT",
    r"G:/Mon Drive/Synchronisation/RooSync/.shared-state/attachments/"
    r"DNN-Argumentum-export-2026-07-07/12-game-rule-content-items.json")
DEFAULT_KEYS = os.environ.get("DNN_KEYS_DIR", ".keys")
TARGETS = ["en", "ru", "pt", "es", "ar", "fa", "zh"]
LANG_NAMES = {"en": "English", "ru": "Russian", "pt": "Portuguese",
              "es": "Spanish", "ar": "Arabic", "fa": "Persian",
              "zh": "Chinese (Simplified)"}
PROSE_FIELDS = ["Summary", "Material", "Installation", "Content", "Variants", "Memo"]
# Title is plain text (not HTML prose) but translated for a complete re-import artifact.
# Game-title translation is additive/reversible: a maintainer may keep the FR brand at re-import.
TRANSLATE_FIELDS = ["Title"] + PROSE_FIELDS


def load_key(keys_dir, name):
    with open(os.path.join(keys_dir, name), encoding="utf-8") as f:
        return f.read().strip()


def call_chat(messages, model, key, base_url, max_tokens, reasoning_effort="low", timeout=240):
    body = {"model": model, "messages": messages, "max_completion_tokens": max_tokens}
    if reasoning_effort:
        body["reasoning_effort"] = reasoning_effort
    data = json.dumps(body).encode("utf-8")
    req = urllib.request.Request(
        base_url.rstrip("/") + "/chat/completions", data=data,
        headers={"Authorization": "Bearer " + key, "Content-Type": "application/json"})
    t0 = time.time()
    with urllib.request.urlopen(req, timeout=timeout) as r:
        resp = json.loads(r.read())
    dt = time.time() - t0
    content = resp["choices"][0]["message"]["content"]
    usage = resp.get("usage", {})
    rt = usage.get("completion_tokens_details", {}).get("reasoning_tokens", 0)
    return content, dt, rt


def translate_cell(fr_html, target_lang, model, key, base_url):
    """Translate one FR HTML cell to one target language. Returns (html, dt, rt)."""
    approx_in = len(fr_html) / 4
    max_tokens = max(1500, min(8000, int(approx_in * 1.6) + 800))
    lang_name = LANG_NAMES[target_lang]
    sys_msg = (
        "You are a professional translator for Argumentum, an educational card game about "
        "logical fallacies and reasoning. You translate game-rule prose from French.")
    user_msg = (
        f"Translate the following French HTML into {lang_name}.\n"
        "STRICT RULES:\n"
        "- Preserve ALL HTML tags, attributes, and structural entity references "
        "(&amp; &nbsp; &lt; &gt;) EXACTLY as-is. Only translate the visible text content "
        "between and inside tags.\n"
        "- Keep the same number and order of HTML elements. Do NOT add, remove, merge, or "
        "reorder tags.\n"
        "- Keep game-specific proper nouns (game titles, card names) consistent.\n"
        "- Write in the target language's native script (Cyrillic for Russian, CJK for Chinese, "
        "Arabic script for Arabic/Persian).\n"
        "- Return ONLY the translated HTML. No explanation, no markdown fences, no preamble.\n\n"
        f"FRENCH HTML TO TRANSLATE:\n{fr_html}")
    msgs = [{"role": "system", "content": sys_msg}, {"role": "user", "content": user_msg}]
    return call_chat(msgs, model, key, base_url, max_tokens)


def extract_cells(data):
    """Return list of (entity_id, title, field, fr_html) for populated translatable cells."""
    titles = {}
    for v in data["values"]:
        if v["StaticName"] == "Title":
            titles[v["EntityID"]] = v["Value"]
    cells = []
    for v in data["values"]:
        sn = v["StaticName"]
        if sn in TRANSLATE_FIELDS and (v["Value"] or "").strip():
            cells.append((v["EntityID"], titles.get(v["EntityID"], ""), sn, v["Value"]))
    return cells


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--export", default=DEFAULT_EXPORT, help="2sxc Game Rule export JSON")
    ap.add_argument("--out", required=True, help="output re-import JSON (also the resume cache)")
    ap.add_argument("--keys-dir", default=DEFAULT_KEYS)
    ap.add_argument("--smoke", action="store_true")
    ap.add_argument("--all", action="store_true")
    ap.add_argument("--lang", help="single target lang code")
    args = ap.parse_args()

    with open(args.export, encoding="utf-8") as f:
        data = json.load(f)
    cells = extract_cells(data)
    cells.sort(key=lambda c: (len(c[3]), c[0], c[2]))  # shortest first
    print(f"[load] {len(cells)} populated cells across "
          f"{len(set(c[0] for c in cells))} entities", file=sys.stderr)

    providers = [
        ("gpt-5.5", load_key(args.keys_dir, "openai-key.txt"), "https://api.openai.com/v1"),
        ("openai/gpt-5.5", load_key(args.keys_dir, "openrouter-key.txt"),
         "https://openrouter.ai/api/v1"),
    ]

    cache = {}
    if os.path.exists(args.out):
        with open(args.out, encoding="utf-8") as f:
            cache = json.load(f).get("entities", json.load(open(args.out, encoding="utf-8")))
        print(f"[cache] resumed existing output", file=sys.stderr)

    if args.smoke:
        cells, langs = cells[:1], TARGETS
    elif args.lang:
        langs = [args.lang]
    elif args.all:
        langs = TARGETS
    else:
        print("specify --smoke / --lang X / --all", file=sys.stderr)
        sys.exit(1)

    model = providers[0][0]
    total = len(cells) * len(langs)
    done = fails = 0
    for eid, title, field, fr in cells:
        ekey = str(eid)
        cache.setdefault(ekey, {"title": title, "fields": {}})
        cache[ekey]["fields"].setdefault(field, {"fr": fr})
        for lang in langs:
            ck = cache[ekey]["fields"][field]
            if ck.get(lang):
                continue
            ok = False
            for attempt, (model, key, base) in enumerate(providers):
                try:
                    out, dt, rt = translate_cell(fr, lang, model, key, base)
                    ck[lang] = out.strip()
                    ok = True
                    done += 1
                    print(f"  [{done}/{total}] e{eid} {field[:6]}->{lang} "
                          f"({len(out)}c, {dt:.1f}s, rt={rt}) via {model}", file=sys.stderr)
                    break
                except urllib.error.HTTPError as e:
                    body = e.read().decode("utf-8", "replace")[:200]
                    print(f"  [warn] e{eid} {field}->{lang} {model} HTTP {e.code}: {body}",
                          file=sys.stderr)
                    if e.code in (401, 429) and attempt < len(providers) - 1:
                        print(f"  [fallback] switching provider", file=sys.stderr)
                        continue
                    time.sleep(3)
                except Exception as e:
                    print(f"  [warn] e{eid} {field}->{lang} {model} ERR {type(e).__name__}: {e}",
                          file=sys.stderr)
                    time.sleep(3)
            if not ok:
                fails += 1
                print(f"  [FAIL] e{eid} {field}->{lang} all providers exhausted", file=sys.stderr)
            with open(args.out, "w", encoding="utf-8") as f:
                json.dump({"_meta": {}, "entities": cache}, f, ensure_ascii=False, indent=2)
            time.sleep(0.5)

    meta = {"source": os.path.basename(args.export),
            "model": model, "fields_translated": TRANSLATE_FIELDS, "targets": TARGETS,
            "entity_count": len(set(c[0] for c in cells)), "cell_count": len(cells)}
    with open(args.out, "w", encoding="utf-8") as f:
        json.dump({"_meta": meta, "entities": cache}, f, ensure_ascii=False, indent=2)
    print(f"\n[done] {done} translated, {fails} failed -> {args.out}", file=sys.stderr)


if __name__ == "__main__":
    main()
