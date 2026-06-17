#!/usr/bin/env python3
"""#457 DNN site localization — content-type -> CSV extractor (BRICK 1).

Codifies the *manual* Phase-1 extraction (see docs/dnn-localization/PHASE1-content-audit.md)
into a reproducible tool. Parses the Argumentum 2sxc Razor templates (.cshtml) and emits the
localization CSV in the exact dialect of `docs/dnn-localization/dnn-ui-strings.csv`:

    key,context,source_file,fr,en,ru,pt,es,ar,fa,zh,notes

Two content-types are extracted (matching the PHASE1 audit):

  * ``ui.*``  — hardcoded UI strings inside the templates (a regex over literal patterns).
                The FR value IS in the repo (hardcoded), so it is populated.
  * ``res.*`` — ``@Resources.<Key>`` references. The KEY is in the repo; the canonical FR
                VALUE lives in SQL (2sxc App Resources) — DB-only. So ``fr`` is left empty and
                ``notes`` flags it ``INFERRED FR / DB-only`` (matches the PHASE1 manual rows).

WHY a regex over literal patterns for ``ui.*``: the hardcoded strings are bespoke
(e.g. ``de {0} à {1} joueurs``), not a systematic API. A free-text scanner would fabricate
"translatable strings" out of every literal. Instead we mirror the curated PHASE1 set: each
``ui.*`` entry is declared below with its anchor pattern + canonical FR, and the tool
*verifies the anchor still exists in the source* (anti-fabrication: fail loud if an anchor
disappears, never invent a row).

This is the repo-extractable slice only. The bulk of DNN strings (glossary, FAQ, homepage,
per-rule content, the App resource VALUES) is DB-only and requires a portal/2sxc export
(jsboige, gated) — see PHASE1-content-audit.md. This tool does NOT touch prod or the DB.

USAGE
    python tools/dnn_i18n/extract_dnn_ui_strings.py \\
        --templates-root DNNPlatform/Portals/1/2sxc/Argumentum \\
        --out <output.csv>

The output is a STANDALONE extraction; it does not write to dnn-ui-strings.csv (that file is
another worker's lane, #490). Use reimport_dnn_ui_strings.py to diff/reimport.
"""
from __future__ import annotations

import argparse
import csv
import os
import re
import sys
from dataclasses import dataclass, field
from typing import List

# ---------------------------------------------------------------------------
# Dialect — exact column order of docs/dnn-localization/dnn-ui-strings.csv
# ---------------------------------------------------------------------------
COLUMNS = ["key", "context", "source_file", "fr",
           "en", "ru", "pt", "es", "ar", "fa", "zh", "notes"]
LANG_TARGETS = ["en", "ru", "pt", "es", "ar", "fa", "zh"]  # FR is the source, not a target


# ---------------------------------------------------------------------------
# Declared ui.* extraction entries (curated from PHASE1-content-audit.md §1a).
# Each anchor is a verbatim substring that MUST exist in the named source file;
# the tool asserts it. If a refactor moves/removes the string, the tool fails
# loud rather than silently dropping the row.
# ---------------------------------------------------------------------------
@dataclass
class UiEntry:
    key: str
    context: str
    source_files: List[str]      # relative to --templates-root
    anchor: str                  # verbatim substring expected in the file
    fr: str                      # canonical FR (hardcoded in the template)


UI_ENTRIES: List[UiEntry] = [
    UiEntry(
        key="ui.fallacy.find_out_more",
        context="FallacyExplorer link label '(find out more)'",
        source_files=["_FallacyExplorer_Root.cshtml"],
        anchor="find out more",
        fr="en savoir plus",
    ),
    UiEntry(
        key="ui.rules.players_range",
        context="Rules list/detail player-count line 'de {0} a {1} joueurs'",
        source_files=["_RulesExplorer_RuleList.cshtml", "_RulesExplorer_RuleDetail.cshtml"],
        anchor="joueurs",
        fr="de {0} à {1} joueurs",
    ),
]


# ---------------------------------------------------------------------------
# res.* extraction — @Resources.<Key> references.
# The KEY is extracted from the template; the FR VALUE is DB-only (2sxc App
# Resources), so we emit an empty fr + a DB-only note. The PHASE1 audit also
# carries INFERRED FR scaffolds for some keys; those are a human curation step
# (not reproducible from the repo) and are intentionally NOT regenerated here —
# the tool emits the honest "DB-only" row and leaves inference to the audit.
# ---------------------------------------------------------------------------
# Match both the directive form ``@Resources.<Key>`` AND the expression form
# ``Resources.<Key>`` (e.g. inside ``@Html.Raw(Resources.RuleMemoInstructions)`` — note
# the absence of a leading ``@`` before ``Resources``). The PHASE1 audit captured both
# manually; the optional ``@?`` mirrors that. Without it, the nested-expression refs
# (RuleMemoInstructions) would be silently dropped.
RES_REF_RE = re.compile(r"@?Resources\.([A-Za-z][A-Za-z0-9_]*)")

# Which template files to scan for @Resources.* references (skip the stock
# landing-page builder template "_Album List.cshtml" — no Argumentum content,
# per PHASE1 audit §1).
RES_SOURCE_FILES = [
    "_RulesExplorer_RuleDetail.cshtml",
    "_RulesExplorer_RuleList.cshtml",
    "_FallacyExplorer_Root.cshtml",
]

# Human context for the known @Resources.* keys (from PHASE1 audit §1b). Keys
# not in this map get a generic context — they are still emitted so a later
# portal export can fill their values.
RES_CONTEXTS = {
    "RuleSummary": "Rule detail section heading (<h2>)",
    "RuleMaterial": "Rule detail section heading (<h2>)",
    "RuleInstallation": "Rule detail section heading (<h2>)",
    "RuleVariants": "Rule detail section heading (<h2>)",
    "RuleMemoCard": "Rule detail memo-card heading (<h2>)",
    "RuleMemoInstructions": "Rule detail memo-card instructions (Html.Raw, multi-sentence)",
    "RuleMemoCardFileNamePrefix": "Memo card filename prefix in card name",
    "RuleMemoCardDownload": "Memo card download button label",
}


@dataclass
class Row:
    key: str
    context: str
    source_file: str
    fr: str
    notes: str
    targets: dict = field(default_factory=dict)  # lang -> value (empty by default)

    def as_csv(self) -> dict:
        d = {"key": self.key, "context": self.context,
             "source_file": self.source_file, "fr": self.fr, "notes": self.notes}
        for lang in LANG_TARGETS:
            d[lang] = self.targets.get(lang, "")
        return d


def _read(root: str, rel: str) -> str:
    path = os.path.join(root, rel)
    with open(path, encoding="utf-8") as f:
        return f.read(), path


def extract_ui_entries(root: str) -> List[Row]:
    """Extract declared ui.* rows, asserting each anchor still exists."""
    rows: List[Row] = []
    for e in UI_ENTRIES:
        found_in: List[str] = []
        for rel in e.source_files:
            text, path = _read(root, rel)
            if e.anchor not in text:
                sys.stderr.write(
                    f"ANTI-FAB: anchor {e.anchor!r} for {e.key} not found in {path}. "
                    "The template was refactored — update UI_ENTRIES (do NOT silently drop the row).\n"
                )
                raise SystemExit(2)
            found_in.append(f"Portals/1/2sxc/Argumentum/{rel}")
        note = ""
        # Preserve the bespoke PHASE1 notes for the 2 known buggy keys.
        if e.key == "ui.fallacy.find_out_more":
            note = ("Template hardcodes EN value AND reads text_en/desc_en/link_en "
                    "regardless of culture (i18n bug, see audit s4)")
        elif e.key == "ui.rules.players_range":
            note = "Hardcoded FR; keep {0}/{1} placeholders; source uses &agrave; entity"
        rows.append(Row(key=e.key, context=e.context,
                        source_file=";".join(found_in), fr=e.fr, notes=note))
    return rows


def extract_res_entries(root: str) -> List[Row]:
    """Extract @Resources.* references (key in repo, value DB-only)."""
    seen: dict[str, List[str]] = {}  # key -> list of source files
    for rel in RES_SOURCE_FILES:
        text, _ = _read(root, rel)
        for m in RES_REF_RE.finditer(text):
            key = m.group(1)
            seen.setdefault(key, []).append(f"Portals/1/2sxc/Argumentum/{rel}")
    rows: List[Row] = []
    for key in sorted(seen):
        ctx = RES_CONTEXTS.get(key, "@Resources reference (DB-only value)")
        rows.append(Row(
            key=f"res.{key}",
            context=ctx,
            source_file=";".join(sorted(set(seen[key]))),
            fr="",  # DB-only — value is NOT in the repo
            notes="INFERRED FR; canonical value is DB-only (2sxc App Resources) - verify vs export",
        ))
    return rows


def write_csv(rows: List[Row], out_path: str) -> None:
    with open(out_path, "w", encoding="utf-8", newline="") as f:
        w = csv.DictWriter(f, fieldnames=COLUMNS, quoting=csv.QUOTE_MINIMAL)
        w.writeheader()
        for r in rows:
            w.writerow(r.as_csv())


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__,
                                 formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--templates-root", default="DNNPlatform/Portals/1/2sxc/Argumentum",
                    help="Root dir of the Argumentum 2sxc templates")
    ap.add_argument("--out", required=True, help="Output CSV path (standalone extraction)")
    args = ap.parse_args()

    if not os.path.isdir(args.templates_root):
        return _fail(f"templates root not found: {args.templates_root}")

    rows = extract_ui_entries(args.templates_root) + extract_res_entries(args.templates_root)
    write_csv(rows, args.out)

    ui_n = sum(1 for r in rows if r.key.startswith("ui."))
    res_n = sum(1 for r in rows if r.key.startswith("res."))
    print(f"Extracted {len(rows)} rows ({ui_n} ui.*, {res_n} res.*) -> {args.out}")
    print("NOTE: res.* fr values are DB-only (2sxc App Resources) — left empty by design.")
    return 0


def _fail(msg: str) -> int:
    print(f"ERROR: {msg}", file=sys.stderr)
    return 2


if __name__ == "__main__":
    raise SystemExit(main())
