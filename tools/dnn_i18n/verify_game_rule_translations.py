#!/usr/bin/env python3
"""
Verify a #684 Game Rule translation artifact against its 2sxc export source.

DoD gates enforced (no fabrication, no FR-contamination, HTML preserved):
  1. CELL PARITY: every populated FR cell in the export has all 7 target langs present.
  2. EMPTY STAYS EMPTY: the 7 structurally-empty prose cells are ABSENT (no fabrication).
  3. HTML PRESERVED: the set of HTML tags in each translation == the FR source set.
  4. SCRIPT CORRECTNESS: each translation is in its target script, not FR-contaminated
     (#216-style guard). Cyrillic for ru, CJK for zh, Arabic-script for ar/fa, Latin
     for en/pt/es.

Exit 0 = all gates pass; exit 1 = violations found (printed).

Usage:
  python verify_game_rule_translations.py \
      --artifact docs/dnn-localization/684-translations.json \
      --export <12-game-rule-content-items.json>
"""
import json, os, re, sys, argparse

TARGETS = ["en", "ru", "pt", "es", "ar", "fa", "zh"]
PROSE_FIELDS = ["Summary", "Material", "Installation", "Content", "Variants", "Memo"]
TRANSLATE_FIELDS = ["Title"] + PROSE_FIELDS

# Unicode script ranges (start, end) inclusive
CYRILLIC = [(0x0400, 0x04FF)]
CJK = [(0x4E00, 0x9FFF), (0x3400, 0x4DBF)]
ARABIC = [(0x0600, 0x06FF), (0x0750, 0x077F), (0xFB50, 0xFDFF), (0xFE70, 0xFEFF)]
LATIN_ACCENTED = "àâäçéèêëîïôöùûüÿœæÀÂÄÇÉÈÊËÎÏÔÖÙÛÜŸŒÆ"  # FR diacritics


def in_ranges(ch, ranges):
    o = ord(ch)
    return any(a <= o <= b for a, b in ranges)


def count_script(text, ranges):
    return sum(1 for ch in text if in_ranges(ch, ranges))


def html_tags(s):
    return sorted(re.findall(r"</?[a-zA-Z][^>]*>", s))


def check_script(lang, text):
    """Return (ok, detail). Detects FR-contamination / wrong script."""
    n = max(1, len(text))
    if lang == "ru":
        cyr = count_script(text, CYRILLIC)
        return (cyr >= 3 and cyr / n > 0.1, f"cyrillic={cyr}")
    if lang == "zh":
        cjk = count_script(text, CJK)
        return (cjk >= 1, f"cjk={cjk}")
    if lang in ("ar", "fa"):
        # Two-sided guard against the two real failure modes:
        #  (a) FR-contamination — leftover French in a non-Latin cell (the #216 failure).
        #  (b) a non-Arabic-script answer (model defaulted to English/Latin).
        # Symbol/emoji-heavy cells (scoring diagrams) legitimately have few Arabic letters,
        # so a positive Arabic-letter *ratio* is not a sound gate. Instead: reject if there
        # are FR diacritics OR if there are zero Arabic letters at all.
        ar = count_script(text, ARABIC)
        fr_accents = sum(1 for ch in text if ch in "àâäçéèêëîïôöùûüÿœæÀÂÄÇÉÈÊËÎÏÔÖÙÛÜŸŒ")
        ok = (ar >= 1) and (fr_accents == 0)
        return (ok, f"arabic-script={ar} fr-accents={fr_accents}")
    if lang in ("en", "pt", "es"):
        wrong = (count_script(text, CYRILLIC) + count_script(text, CJK)
                 + count_script(text, ARABIC))
        return (wrong == 0, f"non-latin={wrong}")
    return (True, "n/a")


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--artifact", required=True)
    ap.add_argument("--export", required=True)
    args = ap.parse_args()

    art = json.load(open(args.artifact, encoding="utf-8"))
    exp = json.load(open(args.export, encoding="utf-8"))
    ents = art["entities"]

    # build source map: (eid, field) -> fr
    src = {}
    titles = {}
    for v in exp["values"]:
        if v["StaticName"] == "Title":
            titles[v["EntityID"]] = v["Value"]
        if v["StaticName"] in TRANSLATE_FIELDS and (v["Value"] or "").strip():
            src[(str(v["EntityID"]), v["StaticName"])] = v["Value"]

    violations = []
    cells_checked = tag_ok = script_ok = 0

    # Gate 1+3+4: each source cell present, tags preserved, script correct
    for (ekey, field), fr in sorted(src.items()):
        cells_checked += 1
        ent = ents.get(ekey)
        if not ent or field not in ent.get("fields", {}):
            violations.append(f"[G1-PRESENT] e{ekey} {field}: missing from artifact")
            continue
        cell = ent["fields"][field]
        if cell.get("fr", "") != fr:
            violations.append(f"[G1-FRSRC] e{ekey} {field}: FR source mismatch")
        fr_tags = html_tags(fr)
        for lang in TARGETS:
            if lang not in cell or not cell[lang]:
                violations.append(f"[G1-LANG] e{ekey} {field}->{lang}: missing/empty")
                continue
            t = cell[lang]
            if html_tags(t) != fr_tags:
                violations.append(
                    f"[G3-HTML] e{ekey} {field}->{lang}: tag mismatch "
                    f"(fr={fr_tags} vs {html_tags(t)})")
            else:
                tag_ok += 1
            ok, detail = check_script(lang, t)
            if not ok:
                violations.append(f"[G4-SCRIPT] e{ekey} {field}->{lang}: {detail} "
                                  f"text='{t[:80]}...'")
            else:
                script_ok += 1

    # Gate 2: structurally-empty cells absent (no fabrication)
    populated = set(src.keys())
    for ekey, ent in ents.items():
        for field, cell in ent.get("fields", {}).items():
            if (ekey, field) not in populated:
                violations.append(f"[G2-FABRICATE] e{ekey} {field}: cell exists in artifact "
                                  f"but source is empty/absent (fabrication)")

    total_lang_cells = cells_checked * len(TARGETS)
    print(f"[verify] cells={cells_checked} lang-cells={total_lang_cells}")
    print(f"  tag-preserverd OK: {tag_ok}/{total_lang_cells}")
    print(f"  script-correct OK: {script_ok}/{total_lang_cells}")
    print(f"  violations: {len(violations)}")
    for v in violations[:40]:
        print("    " + v)
    if len(violations) > 40:
        print(f"    ... +{len(violations) - 40} more")
    sys.exit(0 if not violations else 1)


if __name__ == "__main__":
    main()
