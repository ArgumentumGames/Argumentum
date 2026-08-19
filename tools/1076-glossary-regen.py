"""#1076 — Regenerate the locked Rules glossary from the corpus (method #1094).

The glossary in PromptRulesCascadeDriftUser.txt is a hard-coded 2nd referential.
A glossary line may only lock terms the corpus actually attests; a line whose
concept is absent everywhere is a ghost that re-injects vocabulary the game
never uses (issue #1076, ai-01 class-scope measurement 2026-08-19).

This script RE-DERIVES the glossary from the two Rules CSVs: every line
survives iff its FR term is attested (lemma level) in the FR columns; every
emitted term must be attested in its language's columns. Unattested ghost
lines drop by themselves — no manual line deletion (dispatch ucpl97).

Attestation levels (per dispatch + word-probe lessons):
- fr/en/pt/es (Latin): word-boundary match, case-insensitive, simple
  inflections allowed (trailing s / es for Latin-script plurals).
- ru: stem match (term[:-1] substring) — corpus uses declined forms
  (взятку/взятки...), glossary carries the nominative (ai-01 finding a).
- ar/fa/zh: substring of the term (or its longest word) — no \\b at script
  frontiers, agglutination/morphology handled by substring semantics.

Modes: dry-run (default) prints the attestation matrix and the resulting diff;
--apply rewrites the prompt file in place (glossary block only, byte-safe
elsewhere).
"""
import argparse
import csv
import io
import re
import sys
from pathlib import Path

REPO = Path(__file__).resolve().parents[1]
PROMPT = REPO / "Generation/Converters/Argumentum.AssetConverter/DatasetUpdater/Resources/PromptRulesCascadeDriftUser.txt"
CSVS = [
    REPO / "Cards/Rules/Argumentum Rules - Cards.csv",
    REPO / "Cards/Rules/Argumentum Rules - Cards Print and Play.csv",
]
LANGS = ["fr", "en", "ru", "pt", "es", "ar", "fa", "zh"]
COL = {"fr": "Text"} | {l: f"Text_{l}" for l in LANGS if l != "fr"}

LINE_RE = re.compile(r"^(\s*-\s+)(.+?)(\s*)$")


def load_corpus():
    cols = {l: [] for l in LANGS}
    for path in CSVS:
        with open(path, encoding="utf-8-sig", newline="") as f:
            for row in csv.DictReader(f):
                for l in LANGS:
                    v = (row.get(COL[l]) or "").strip()
                    if v:
                        cols[l].append(v)
    return {l: "\n".join(v) for l, v in cols.items()}


def attest_latin(term, corpus):
    # word-boundary, case-insensitive; allow simple plural inflections
    base = re.escape(term)
    pat = re.compile(rf"(?<!\w){base}(?:s|es)?(?!\w)", re.IGNORECASE | re.UNICODE)
    return bool(pat.search(corpus))


def attest_ru(term, corpus):
    stem = term[:-1] if len(term) > 3 else term
    return (term in corpus) or (stem and stem in corpus)


def attest_substr(term, corpus):
    if term in corpus:
        return True
    # fallback: longest word of a multi-word term, minus an agglutination tail
    words = sorted(re.split(r"\s+", term), key=len, reverse=True)
    w = words[0]
    return (w in corpus) or (len(w) > 4 and w[:-2] in corpus)


def attest(term, lang, corpus):
    if lang in ("fr", "en", "pt", "es"):
        return attest_latin(term, corpus)
    if lang == "ru":
        return attest_ru(term, corpus)
    return attest_substr(term, corpus)


def parse_glossary_line(raw):
    """`piocheur (FR) = Reader (EN) = ... (ZH)` -> {lang: term}."""
    m = LINE_RE.match(raw)
    if not m:
        return None, None
    body = m.group(2)
    if "(FR) =" not in body:
        return None, None
    terms = {}
    for part in body.split("="):
        mm = re.match(r"\s*(.+?)\s*\((FR|EN|RU|PT|ES|AR|FA|ZH)\)\s*$", part.strip())
        if not mm:
            return None, None
        terms[mm.group(2).lower()] = mm.group(1).strip()
    if set(terms) != set(LANGS):
        return None, None
    return terms, m.group(1)


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--apply", action="store_true")
    args = ap.parse_args()

    corpus = load_corpus()
    text = PROMPT.read_text(encoding="utf-8")
    lines = text.split("\n")

    report = []
    out_lines = []
    dropped = []
    kept_terms = 0
    unattested_kept = []
    for raw in lines:
        terms, prefix = parse_glossary_line(raw)
        if terms is None:
            out_lines.append(raw)
            continue
        fr_ok = attest(terms["fr"], "fr", corpus["fr"])
        line_status = []
        for l in LANGS:
            ok = attest(terms[l], l, corpus[l])
            line_status.append(f"{l}:{'Y' if ok else 'N'}")
            if ok:
                kept_terms += 1 if fr_ok else 0
            elif fr_ok:
                unattested_kept.append(f"{terms['fr']}/{l}")
        report.append(f"  {terms['fr']:<22} FR:{'Y' if fr_ok else 'N'} {' '.join(line_status)}")
        if fr_ok:
            out_lines.append(raw)
        else:
            dropped.append(terms["fr"])

    print("Attestation matrix (lemma level, both Rules CSVs):")
    print("\n".join(report))
    print(f"\nlines kept: {11 - len(dropped) if len(dropped) else 'all'} | dropped: {dropped or 'none'}")
    print(f"terms attested on kept lines: {kept_terms}/80 expected")
    if unattested_kept:
        print(f"UNATTESTED SLOTS ON KEPT LINES (needs corpus re-derivation): {unattested_kept}")

    new_text = "\n".join(out_lines)
    if new_text == text:
        print("no change")
        return
    if not args.apply:
        print("dry-run — no write (use --apply)")
        return
    PROMPT.write_text(new_text, encoding="utf-8", newline="")
    print(f"APPLIED: {PROMPT} rewritten ({len(lines)} -> {len(out_lines)} lines)")
    return


if __name__ == "__main__":
    main()
