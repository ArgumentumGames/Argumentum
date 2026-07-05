#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
mnemonics_to_latin.py — deterministic Virtues syllogistic-mnemonic normaliser (#654).

Decision (VERIFIED jsboige 2026-07-04, dispatch q9xpks): Option B "keep-Latin" (Scenario
S2 of docs/investigations/2026-07-03-654-mnemonics-celltable.md). The 19 classical syllogistic
mnemonics (Barbara, Celarent, ... Bamalip) are LATIN CANONICAL technical terms; the languages
that transliterated some of them to native script (RU/AR/ZH/FA) revert to the Latin form for
consistency with FR/EN/ES/PT, which already keep them Latin (20/20).

WHY A SCRIPT (not gpt-5.5, not a C# test)
  This is NOT translation. It is a deterministic script-conversion: a fixed Latin technical
  term already present in title_fr must replace its transliterated rendering in the 4 non-Latin
  languages. gpt-5.5 would be inappropriate (would hallucinate transliterations / drift). A
  Python stdlib script reads the CSV, builds a verified per-cell replacement plan, and applies
  it. The transliteration per language is NOT invented — it is detected in the current cell and
  swapped for the canonical Latin form already in title_fr for that pk.

WHAT IT DOES (Option B / S2 — keep-Latin)
  For each mnemonic pk (106-127, the 19 depth-7 CQ rows whose title_fr = "Syllogisme <M>"),
  and each of RU/AR/ZH/FA:
    - If title_<lang> already contains the Latin mnemonic  -> kept-Latin, SKIP.
    - Else (transliterated) -> extract the transliterated token by removing the language's
      structural native words (Силлогизм / قياس / 三段论 / 式 / ...), verify the residue is a
      single non-empty token with no Latin leakage, and plan the replacement
      "<translit>" -> "<M>" inside title_<lang>.

SCOPE (verified code=truth on master d90ce613, where FR title carries a mnemonic)
  52 title cells transliterated: RU 14 / AR 16 / ZH 6 / FA 16.
  description cells: 0 (descriptions carry no mnemonic). remark cells: 14 — OUT OF SCOPE per
  dispatch (title only); surfaced in the dry-run report as a follow-up flag, not converted.

USAGE
  python tools/mnemonics_to_latin.py --csv "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv"
      # default: dry-run, prints the 52-cell plan + summary, writes no file.

  python tools/mnemonics_to_latin.py --csv <PATH> --report docs/investigations/<NAME>.md
      # dry-run, additionally writes a markdown report (the 52 cells before/after).

  python tools/mnemonics_to_latin.py --csv <PATH> --apply
      # WRITES the CSV in place (UTF-8 BOM preserved, line-by-line targeted replace so the
      # rest of the file — quoting, delimiters, line endings — is byte-preserved except the
      # swapped mnemonic token). GATED: do NOT run --apply on prod CSV until #654 is unblocked
      # post-tag (dispatch q9xpks). Use --dry-run until then.

  python tools/test_mnemonics_to_latin.py
      # stdlib unittest: logic contract (mnemonic extraction, translit detection per lang,
      # plan-building on synthetic CSV, apply round-trip, 52-cell grounding on the real CSV).

DEPENDENCIES: Python 3 stdlib only (csv, argparse, os, sys, re, collections).

Exit codes: 0 = plan built / applied cleanly; 2 = one or more AMBIGUOUS cells detected
            (residue not a single clean token) — the plan still prints but --apply is refused
            until the ambiguous cells are reviewed and resolved in this file.
"""

import argparse
import csv
import os
import re
import sys
from collections import OrderedDict

# ─────────────────────────────────────────────────────────────────────────────
# Constants — grounded in the corpus (code=truth), never from memory.
# 19 classical syllogistic mnemonics, source: docs/investigations/2026-07-03-654
# (verified against title_fr of pks 106-127 on master d90ce613).
# ─────────────────────────────────────────────────────────────────────────────

MNEMONICS = [
    "Barbara", "Celarent", "Darii", "Ferio", "Cesare", "Camestres", "Festino",
    "Baroco", "Darapti", "Felapton", "Disamis", "Datisi", "Bocardo", "Ferison",
    "Camenes", "Dimatis", "Fesapo", "Fresison", "Bamalip",
]

# The 19 pks whose title_fr = "Syllogisme <M>" (pks 110/115/122 are figure-rows, no mnemonic).
MNEMONIC_PKS = [106, 107, 108, 109, 111, 112, 113, 114, 116, 117, 118, 119,
                120, 121, 123, 124, 125, 126, 127]

# Map pk -> canonical Latin mnemonic (extracted from title_fr, verified code=truth).
# Built once from the FR column; asserted in tests.
_PK_TO_MNEMONIC = {
    106: "Barbara", 107: "Celarent", 108: "Darii", 109: "Ferio", 111: "Cesare",
    112: "Camestres", 113: "Festino", 114: "Baroco", 116: "Darapti", 117: "Felapton",
    118: "Disamis", 119: "Datisi", 120: "Bocardo", 121: "Ferison", 123: "Camenes",
    124: "Dimatis", 125: "Fesapo", 126: "Fresison", 127: "Bamalip",
}

# Languages in scope (the 4 that transliterate Latin mnemonics).
LANGS = ["ru", "ar", "zh", "fa"]

# Structural native words surrounding the mnemonic in each language.
# Learned from the kept-Latin cells (e.g. RU pk106 "Силлогизм Barbara"): remove these and the
# residue is the mnemonic token (Latin in kept cells, transliterated in the cells we convert).
# Order matters for ZH (longest first so "式三段论" strips before "三段论").
STRUCTURAL_TOKENS = {
    "ru": ["Силлогизм"],
    "ar": ["قياس", "المنطقي"],   # "قياس <M>" or "قياس <M> المنطقي"
    "zh": ["式三段论", "三段论"],  # "<M>三段论" or "<M> 式三段论"
    "fa": ["قیاس"],
}

# Regex: detect a Latin mnemonic as a standalone Latin word (case-sensitive — the canonical
# forms are capitalised). Used to (a) extract M from title_fr, (b) detect kept-Latin cells to
# skip, (c) verify the proposed title now carries M.
#
# We CANNOT use \b...\b here: under re.UNICODE (default for str), \w includes CJK/Arabic/Cyrillic
# letters, so \b does NOT fire at the Latin→CJK boundary (e.g. "Festino三段论" — no \b between the
# 'o' and '三'). We assert Latin-letter boundaries only: (?<![A-Za-z]) / (?![A-Za-z]). This still
# matches "Festino" glued to CJK/Arabic/Cyrillic script, which is exactly what we want.
_MNEMONIC_RE = re.compile(r"(?<![A-Za-z])(" + "|".join(MNEMONICS) + r")(?![A-Za-z])")

# Latin-letter range (basic) to detect Latin leakage in a supposedly-transliterated residue.
_LATIN_RE = re.compile(r"[A-Za-z]")


# ─────────────────────────────────────────────────────────────────────────────
# Core logic
# ─────────────────────────────────────────────────────────────────────────────

def extract_mnemonic_latin(title_fr):
    """Return the canonical Latin mnemonic present in title_fr, or None.

    title_fr for mnemonic pks is always "Syllogisme <M>" (verified). We match <M> against the
    19 known forms. Returns None if no mnemonic is present (e.g. a figure-row title).
    """
    if not title_fr:
        return None
    m = _MNEMONIC_RE.search(title_fr)
    return m.group(1) if m else None


def extract_translit_token(title_lang, lang):
    """Extract the transliterated mnemonic token from a non-Latin title cell.

    Strips the language's structural native words (STRUCTURAL_TOKENS[lang]) and surrounding
    whitespace from title_lang. The residue is the mnemonic token — either Latin (kept) or
    transliterated. Caller distinguishes via has_latin_mnemonic().

    Returns (residue, ambiguous_flag):
      - residue: the stripped string
      - ambiguous_flag: True if the residue is empty OR contains a space (multi-token) OR
        contains Latin letters (would mean we missed a structural word). Ambiguous cells are
        excluded from the plan and surfaced for manual review.
    """
    if not title_lang:
        return "", True  # missing cell — ambiguous (cannot convert safely)
    s = title_lang
    for tok in STRUCTURAL_TOKENS.get(lang, []):
        s = s.replace(tok, "")
    s = s.strip().strip("‏‎").strip()  # strip RTL/LTR marks + whitespace
    ambiguous = False
    if not s or " " in s:
        ambiguous = True
    if _LATIN_RE.search(s):
        # Latin letters remain -> a structural word was not stripped -> unsafe.
        ambiguous = True
    return s, ambiguous


def has_latin_mnemonic(cell):
    """True if the cell already contains a Latin mnemonic (kept-Latin -> skip)."""
    return bool(cell) and bool(_MNEMONIC_RE.search(cell))


def build_conversion_plan(rows):
    """Build the per-cell replacement plan for Option B (keep-Latin).

    rows: list of dict (DictReader rows), keyed by column name incl. 'pk', 'title_fr',
          'title_ru', ... 'title_fa'. Pass the full Virtues CSV rows.

    Returns a list of OrderedDict entries:
      {pk, lang, mnemonic, current_title, translit_token, proposed_title}
    for every transliterated title cell (Option B reverts them to Latin). Ambiguous cells
    are returned separately via the second return value (list of {pk, lang, current_title,
    reason}) so the caller can surface them without blocking the clean plan.
    """
    plan = []
    ambiguous = []
    for r in rows:
        try:
            pk = int(r["pk"])
        except (KeyError, ValueError, TypeError):
            continue
        if pk not in MNEMONIC_PKS:
            continue
        M = extract_mnemonic_latin(r.get("title_fr", ""))
        if M is None:
            # title_fr has no mnemonic for this pk — skip (should not happen for MNEMONIC_PKS).
            continue
        for lang in LANGS:
            cell = r.get(f"title_{lang}", "")
            if has_latin_mnemonic(cell):
                continue  # kept-Latin
            if not cell.strip():
                # blank cell — not a transliteration, out of scope (blank-fill is another lane)
                continue
            token, is_amb = extract_translit_token(cell, lang)
            if is_amb:
                ambiguous.append({
                    "pk": pk, "lang": lang, "mnemonic": M,
                    "current_title": cell, "residue": token,
                    "reason": "empty/multi-token/Latin-leak" if token else "empty-residue",
                })
                continue
            proposed = cell.replace(token, M)
            # Safety: the replacement must actually change the cell and M must now be present.
            if proposed == cell or not has_latin_mnemonic(proposed):
                ambiguous.append({
                    "pk": pk, "lang": lang, "mnemonic": M,
                    "current_title": cell, "residue": token,
                    "reason": "replace-noop-or-missing-after",
                })
                continue
            plan.append(OrderedDict([
                ("pk", pk), ("lang", lang), ("mnemonic", M),
                ("current_title", cell), ("translit_token", token),
                ("proposed_title", proposed),
            ]))
    return plan, ambiguous


def render_report(plan, ambiguous, csv_path):
    """Return a human-readable markdown report (string) of the dry-run plan."""
    lines = []
    lines.append(f"# #654 mnemonics → Latin (Option B / S2) — dry-run report\n")
    lines.append(f"**Source CSV:** `{csv_path}`\n")
    lines.append(f"**Decision:** keep-Latin (VERIFIED jsboige 2026-07-04, dispatch q9xpks).\n")
    total = len(plan)
    by_lang = {lang: sum(1 for e in plan if e["lang"] == lang) for lang in LANGS}
    lines.append(f"**Cells to convert (title only): {total}** "
                 f"(RU {by_lang['ru']} / AR {by_lang['ar']} / ZH {by_lang['zh']} / FA {by_lang['fa']})\n")
    if ambiguous:
        lines.append(f"\n> ⚠️ AMBIGUOUS cells excluded from the plan: {len(ambiguous)} (see bottom).\n")
    lines.append("\n## Plan (before → after)\n")
    for e in plan:
        lines.append(
            f"- **pk {e['pk']} [{e['lang']}]** `{e['mnemonic']}`: "
            f"`{e['current_title']}` → `{e['proposed_title']}` "
            f"(token `{e['translit_token']}` → `{e['mnemonic']}`)"
        )
    if ambiguous:
        lines.append("\n## ⚠️ Ambiguous cells (excluded — manual review)\n")
        for a in ambiguous:
            lines.append(
                f"- pk {a['pk']} [{a['lang']}] `{a['mnemonic']}`: "
                f"`{a['current_title']}` (residue `{a['residue']}`, reason: {a['reason']})"
            )
    lines.append("\n## Out-of-scope flags\n")
    lines.append("- **remark cells** also carry transliterated mnemonics (~14 cells; RU 4 / AR 4 "
                 "/ ZH 2 / FA 4) — NOT in scope per dispatch (title only). Surfaced for jsboige "
                 "to decide whether #654 extends to remark.\n")
    return "\n".join(lines)


def apply_plan(csv_path, plan):
    """Apply the plan in place, modifying ONLY the title_<lang> field of each target row.

    Strategy: parse the CSV with csv.reader (preserves column order), replace the transliterated
    token ONLY inside the title_<lang> field (the description_*/remark_* fields of the same row
    are left untouched — they are out of the title-only scope of dispatch q9xpks, even when they
    also contain the transliterated mnemonic), then reserialise with QUOTE_MINIMAL + CRLF + BOM.

    Round-trip fidelity: verified that QUOTE_MINIMAL + lineterminator='\\r\\n' reproduces the
    original bytes byte-for-byte (modulo the BOM, which we re-prepend). So the diff between the
    input and the written file is exactly the 52 swapped mnemonic tokens, nothing else.

    Returns the count of fields modified.
    """
    import io
    with open(csv_path, "rb") as f:
        raw_bytes = f.read()
    has_bom = raw_bytes.startswith(b"\xef\xbb\xbf")
    # utf-8-sig strips the BOM when decoding (so we detected it at byte level above).
    raw = raw_bytes.decode("utf-8-sig")

    reader = csv.reader(io.StringIO(raw))
    rows = list(reader)
    if not rows:
        return 0
    header = rows[0]
    col_idx = {name: i for i, name in enumerate(header)}

    by_pk = {}
    for e in plan:
        by_pk.setdefault(e["pk"], []).append(e)

    applied = 0
    for row in rows[1:]:
        try:
            pk = int(row[col_idx["pk"]])
        except (KeyError, IndexError, ValueError):
            continue
        if pk not in by_pk:
            continue
        for e in by_pk[pk]:
            idx = col_idx[f"title_{e['lang']}"]
            old = row[idx]
            token = e["translit_token"]
            if token not in old:
                continue  # already converted / mismatch — skip, do not fail the run
            new = old.replace(token, e["mnemonic"], 1)
            if new != old:
                row[idx] = new
                applied += 1

    buf = io.StringIO()
    writer = csv.writer(buf, quoting=csv.QUOTE_MINIMAL, lineterminator="\r\n")
    writer.writerows(rows)
    out_bytes = (b"\xef\xbb\xbf" if has_bom else b"") + buf.getvalue().encode("utf-8")

    with open(csv_path, "wb") as f:
        f.write(out_bytes)
    return applied


# ─────────────────────────────────────────────────────────────────────────────
# CLI
# ─────────────────────────────────────────────────────────────────────────────

DEFAULT_CSV = os.path.join("Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv")


def main(argv=None):
    p = argparse.ArgumentParser(description="Virtues syllogistic-mnemonic normaliser (#654, Option B keep-Latin).")
    p.add_argument("--csv", default=DEFAULT_CSV, help="Path to the Virtues taxonomy CSV.")
    p.add_argument("--dry-run", action="store_true", default=True,
                   help="Default: build the plan and print it, write no file.")
    p.add_argument("--apply", action="store_true",
                   help="Write the CSV in place (line-by-line targeted replace). GATED post-tag.")
    p.add_argument("--report", default=None,
                   help="Optional path to write a markdown dry-run report.")
    args = p.parse_args(argv)

    if not os.path.exists(args.csv):
        print(f"ERROR: CSV not found: {args.csv}", file=sys.stderr)
        return 1

    with open(args.csv, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.DictReader(f))

    plan, ambiguous = build_conversion_plan(rows)

    by_lang = {lang: sum(1 for e in plan if e["lang"] == lang) for lang in LANGS}
    print(f"#654 Option B (keep-Latin) plan: {len(plan)} title cells "
          f"(RU {by_lang['ru']} / AR {by_lang['ar']} / ZH {by_lang['zh']} / FA {by_lang['fa']})")
    if ambiguous:
        print(f"⚠️ {len(ambiguous)} ambiguous cell(s) excluded — see below.", file=sys.stderr)

    if args.report:
        os.makedirs(os.path.dirname(os.path.abspath(args.report)), exist_ok=True)
        with open(args.report, "w", encoding="utf-8") as f:
            f.write(render_report(plan, ambiguous, args.csv))
        print(f"Report written: {args.report}")

    if args.apply:
        if ambiguous:
            print("REFUSED: --apply cannot run while ambiguous cells exist. Resolve them in "
                  "STRUCTURAL_TOKENS / extract_translit_token first.", file=sys.stderr)
            return 2
        n = apply_plan(args.csv, plan)
        print(f"Applied {n} replacement(s) to {args.csv}")
        return 0

    # dry-run: always print the plan summary (first 60 entries) for eyeballing
    for e in plan[:60]:
        print(f"  pk {e['pk']:>3} [{e['lang']}] {e['mnemonic']:<10} "
              f"`{e['current_title']}` -> `{e['proposed_title']}`")
    if len(plan) > 60:
        print(f"  ... ({len(plan) - 60} more)")
    for a in ambiguous:
        print(f"  AMBIGUOUS pk {a['pk']} [{a['lang']}]: `{a['current_title']}` "
              f"(residue `{a['residue']}`, {a['reason']})", file=sys.stderr)
    return 2 if ambiguous else 0


if __name__ == "__main__":
    sys.exit(main())
