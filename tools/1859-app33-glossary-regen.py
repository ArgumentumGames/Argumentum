"""#457/#1859 — Regenerate the locked app33 glossary from the cards CSV (model #1094).

The app33 site-content prompt (PromptDnnApp33TranslateMultiUser.txt) must carry a
game glossary DERIVED from the cards the game prints — never hand-copied
(review c.6098996342: the 10/10 trial renamed the flagship game in en/zh/fa
because the prompt carried no glossary at all).

Two engines, one contract (same split as #1076):
- this script derives and rewrites the block between the GLOSSARY-APP33
  sentinels in the prompt;
- PromptDnnApp33GlossaryConsistencyTests re-derives the same expectations on
  every test run and fails on divergence.

Derivation:
- the 5 game names come verbatim from the first '## ' markdown heading of rows
  Rules_01/_07/_09/_11/_13 of Cards/Rules/Argumentum Rules - Cards.csv, in all
  8 language columns (proper nouns: exact match, no lemmatisation);
- the card term (« cartes ») is attested per language from a measured candidate
  table (lemma level, same attestation semantics as tools/1076-glossary-regen.py);
  the Russian rival « карточк » must stay unattested (measured 10/10: карты
  stem x231, карточк x0) — its appearance means the corpus moved and the table
  must be re-derived deliberately, not silently.

Modes: dry-run (default) prints the derived block and the attestation matrix;
--apply rewrites ONLY the lines between the sentinels (rest of the file
byte-preserved, BOM and CRLF included).
"""
import argparse
import csv
import re
import sys
from pathlib import Path

REPO = Path(__file__).resolve().parents[1]
PROMPT = REPO / "Generation/Converters/Argumentum.AssetConverter/DatasetUpdater/Resources/PromptDnnApp33TranslateMultiUser.txt"
GAME_CSV = REPO / "Cards/Rules/Argumentum Rules - Cards.csv"
CORPUS_CSVS = [
    REPO / "Cards/Rules/Argumentum Rules - Cards.csv",
    REPO / "Cards/Rules/Argumentum Rules - Cards Print and Play.csv",
]
LANGS = ["fr", "en", "ru", "pt", "es", "ar", "fa", "zh"]
COL = {"fr": "Text"} | {l: f"Text_{l}" for l in LANGS if l != "fr"}
TAGS = {"fr": "FR", "en": "EN", "ru": "RU", "pt": "PT", "es": "ES", "ar": "AR", "fa": "FA", "zh": "ZH"}

GAME_ROWS = ["Rules_01", "Rules_07", "Rules_09", "Rules_11", "Rules_13"]
HEADING_RE = re.compile(r"^##\s*(.+?)\s*$", re.MULTILINE)

# Measured card-term candidates (10/10, review c.6098996342 + local re-measure):
# each candidate must be attested (lemma level) in its language's Rules corpus,
# else the script fails loud — a silent swap would re-create the hand-copied
# glossary this tool exists to prevent.
CARD_TERM_CANDIDATES = {
    "fr": "cartes",
    "en": "cards",
    "ru": "карты",
    "pt": "cartas",
    "es": "cartas",
    "ar": "بطاقات",
    "fa": "کارت‌ها",  # KART-HA with ZWNJ, as printed in the fa rules
    "zh": "卡牌",
}
RU_RIVAL = "карточк"


def load_corpus():
    cols = {l: [] for l in LANGS}
    for path in CORPUS_CSVS:
        with open(path, encoding="utf-8-sig", newline="") as f:
            for row in csv.DictReader(f):
                for l in LANGS:
                    v = (row.get(COL[l]) or "").strip()
                    if v:
                        cols[l].append(v)
    return {l: "\n".join(v) for l, v in cols.items()}


def attest_latin(term, corpus):
    base = re.escape(term)
    pat = re.compile(rf"(?<!\w){base}(?:s|es)?(?!\w)", re.IGNORECASE | re.UNICODE)
    return bool(pat.search(corpus))


def attest(term, lang, corpus):
    if lang in ("fr", "en", "pt", "es"):
        return attest_latin(term, corpus)
    return term in corpus  # ru stem / ar/fa/zh substring: no usable \b at script frontiers


def derive_game_names():
    """Row -> {lang: name}, verbatim from the first '## ' heading of each language cell."""
    names = {}
    with open(GAME_CSV, encoding="utf-8-sig", newline="") as f:
        for row in csv.DictReader(f):
            pk = (row.get("pk") or "").strip()
            if pk not in GAME_ROWS:
                continue
            entry = {}
            for l in LANGS:
                cell = row.get(COL[l]) or ""
                m = HEADING_RE.search(cell)
                if not m:
                    sys.exit(f"FAIL: row {pk} has no '## ' heading in column {COL[l]} — "
                             f"the derivation source must be able to see every game name")
                entry[l] = m.group(1)
            names[pk] = entry
    missing = [r for r in GAME_ROWS if r not in names]
    if missing:
        sys.exit(f"FAIL: game-name rows not found in {GAME_CSV.name}: {missing}")
    return names


def derive_card_terms(corpus):
    terms = {}
    matrix = []
    for l in LANGS:
        term = CARD_TERM_CANDIDATES[l]
        ok = attest(term, l, corpus[l])
        matrix.append(f"  {l}:{term} -> {'attested' if ok else 'NOT ATTESTED'}")
        if not ok:
            print("\n".join(matrix))
            sys.exit(f"FAIL: card-term candidate '{term}' ({l}) is not attested in the Rules "
                     f"corpus — the corpus moved; re-derive the candidate table deliberately")
        terms[l] = term
    rival = corpus["ru"].count(RU_RIVAL)
    matrix.append(f"  ru rival '{RU_RIVAL}' -> {rival} occurrence(s) (must be 0)")
    print("Card-term attestation matrix (lemma level, both Rules CSVs):")
    print("\n".join(matrix))
    if rival:
        sys.exit(f"FAIL: the rival '{RU_RIVAL}' is now attested ({rival}x) — re-derive the "
                 f"card term instead of keeping the locked карты")
    return terms


def render_block(game_names, card_terms):
    lines = []
    for row in GAME_ROWS:
        lines.append("- " + " = ".join(f"{game_names[row][l]} ({TAGS[l]})" for l in LANGS))
    lines.append("- " + " = ".join(f"{card_terms[l]} ({TAGS[l]})" for l in LANGS))
    return lines


BEGIN = "<!-- GLOSSARY-APP33:BEGIN -->"
END = "<!-- GLOSSARY-APP33:END -->"


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--apply", action="store_true")
    args = ap.parse_args()

    corpus = load_corpus()
    game_names = derive_game_names()
    card_terms = derive_card_terms(corpus)
    block = render_block(game_names, card_terms)

    print("\nDerived glossary block:")
    for line in block:
        print(line)

    raw = PROMPT.read_bytes()
    bom = raw.startswith(b"\xef\xbb\xbf")
    text = raw.decode("utf-8-sig")
    lines = text.split("\n")

    try:
        begin_idx = next(i for i, l in enumerate(lines) if l.rstrip("\r") == BEGIN)
        end_idx = next(i for i, l in enumerate(lines) if l.rstrip("\r") == END)
    except StopIteration:
        sys.exit(f"FAIL: sentinels not found in {PROMPT.name} — insert '{BEGIN}' / '{END}' "
                 f"around the glossary section first (this tool only rewrites between them)")

    if end_idx < begin_idx:
        sys.exit("FAIL: END sentinel before BEGIN sentinel — fix the prompt structure")

    crlf = "\r\n" if any(l.endswith("\r") for l in lines) else "\n"
    new_lines = lines[:begin_idx + 1] + [l + ("\r" if crlf == "\r\n" else "") for l in block] + lines[end_idx:]
    new_text = "\n".join(new_lines)

    old_block = [l.rstrip("\r") for l in lines[begin_idx + 1:end_idx] if l.strip()]
    if old_block == block:
        print("\nno change — prompt block already matches the derivation")
        return
    if not args.apply:
        print("\ndry-run — no write (use --apply)")
        for old, new in zip(old_block, block):
            if old != new:
                print(f"  - {old}\n  + {new}")
        return

    PROMPT.write_bytes((b"\xef\xbb\xbf" if bom else b"") + new_text.encode("utf-8"))
    print(f"\nAPPLIED: glossary block rewritten in place ({len(old_block)} -> {len(block)} lines)")


if __name__ == "__main__":
    main()
