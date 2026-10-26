#!/usr/bin/env python3
"""T3 dossier replay instrument — composition of the App_GlobalResources variant files.

Replays §1 of docs/dnn-localization/457-t3-globalresources-arbitrage-dossier-2026-10-10.md
(PR #1852). For each family that has an fr-FR variant (the 6 'variant files' of the
erratum #1846), measures from the BASE (EN/invariant) resx:
  - entries (data elements, real ones — the 24 MSDN-schema ghosts of #1846 are excluded
    by parsing the XML tree, not by regex on raw bytes)
  - composition classes:
      prose   : value has >= 2 space-separated words (sentences, labels with context)
                -> what an LLM translates, what upstream packs also ship
      single  : one word / short token (buttons 'Cancel', 'Search')
      List_*  : treated as datalist by family name (names of countries etc., upstream wins)
  - fr-FR coverage: base keys present/absent in fr-FR, and extras (fr-FR keys absent
    from base — the version-drift warning of the dossier)

Self-check: ElementTree parse only; a file whose parse fails raises — it is never
silently skipped (an unparsed file would read as 'no entries').

USAGE
    python tools/dnn_i18n/resx_variant_composition.py
Exit code 0 on success. Expected figures on the dossier tree: 1 241 base entries,
796 prose / 445 single-word, 5 910 + 1 536 = 7 446 cells if 6 cultures were produced.
"""
import sys
import xml.etree.ElementTree as ET
from pathlib import Path

REPO = Path(__file__).resolve().parents[2]
ROOT = REPO / "DNNPlatform" / "App_GlobalResources"
FAMILIES = ["Exceptions", "FileUpload", "GlobalResources",
            "List_Country", "SharedResources", "WebControls"]


def entries(path):
    """Real <data> children of the root, name->value. Ghost literals inside the
    xs:schema comment block are NOT elements — the tree parse drops them, which is
    exactly the reconciliation #1846 performed (2 731 brut vs 2 707 real)."""
    root = ET.parse(path).getroot()
    out = {}
    for d in root.findall("data"):
        v = d.find("value")
        out[d.get("name")] = "" if v is None or v.text is None else v.text
    return out


def classify(value):
    words = value.split()
    if len(words) >= 2:
        return "prose"
    return "single"


def main():
    total = {"prose": 0, "single": 0}
    print(f"{'family':<18}{'base':>6}{'prose':>7}{'single':>7}"
          f"{'frFR':>6}{'cov':>7}{'extra':>6}")
    for fam in FAMILIES:
        base = entries(ROOT / f"{fam}.resx")
        fr = entries(ROOT / f"{fam}.fr-FR.resx")
        comp = {"prose": 0, "single": 0}
        for v in base.values():
            comp[classify(v)] += 1
        for k in comp:
            total[k] += comp[k]
        covered = sum(1 for k in base if k in fr)
        extras = sum(1 for k in fr if k not in base)
        print(f"{fam:<18}{len(base):>6}{comp['prose']:>7}{comp['single']:>7}"
              f"{len(fr):>6}{covered:>4}/{len(base):<2}{extras:>6}")
    n = sum(total.values())
    print(f"\nbase entries (6 variant families): {n}")
    print(f"  prose (>=2 words): {total['prose']} = {100*total['prose']/n:.1f}%")
    print(f"  single-word      : {total['single']} = {100*total['single']/n:.1f}%")
    print(f"cells if 6 cultures produced: {n} x 6 = {n*6}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
