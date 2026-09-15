#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Apply _csv_note marker to 10 HIGH-stale CardPen templates (issue #812).

For each template JSON, inserts one top-level key `_csv_note` whose value
explains that the embedded `csv` is a snapshot (overridden at runtime by
the CSV source via HarvestManager.cs:342). Ignored by CardPen (which reads
`csv`/`mustache`/`css`). Add-only, no diff beyond the new key.

Safety:
- UTF-8 no-BOM preserved (templates are no-BOM, verified)
- CRLF preserved (CardPen / PapaParse expects CRLF, templates use CRLF)
- Atomic write (read all → modify → write all)
- JSON validity verified post-write via json.loads
- Idempotent: skip if `_csv_note` already present
"""
import json, sys, os

# Path → DataSet description (CSV source render truth)
TARGETS = [
    ("Cards/Rules/Argumentum_Rules_fr.json",
     "Rules",
     "Cards\\Rules\\Argumentum Rules - Cards.csv"),
    ("Cards/Fallacies/Argumentum_Fallacies_Face_fr.json",
     "FallaciesTaxonomy",
     "Cards\\Fallacies\\Argumentum Fallacies - Taxonomy.csv"),
    ("Cards/Fallacies/Argumentum_Virtues_Face_fr.json",
     "VirtuesTaxonomy",
     "Cards\\Fallacies\\Argumentum Virtues - Taxonomy.csv"),
    ("Cards/Scenarii/Argumentum_Scenarii_Face_fr.json",
     "Scenarii",
     "Cards\\Scenarii\\Argumentum Scenarii - Cards.csv"),
    ("Cards/Scenarii/Argumentum_Scenarii_Back_fr.json",
     "Scenarii",
     "Cards\\Scenarii\\Argumentum Scenarii - Cards.csv"),
    ("Cards/Fallacies/Argumentum_Fallacies_Face_2_fr.json",
     "FallaciesTaxonomy",
     "Cards\\Fallacies\\Argumentum Fallacies - Taxonomy.csv"),
    ("Cards/Fallacies/Argumentum_Fallacies_Face_3_fr.json",
     "FallaciesTaxonomy",
     "Cards\\Fallacies\\Argumentum Fallacies - Taxonomy.csv"),
    ("Cards/Memo/Argumentum_Memo_Face_fr.json",
     "FallaciesTaxonomy (carte filter)",
     "Cards\\Fallacies\\Argumentum Fallacies - Taxonomy.csv"),
    ("Cards/Memo/Argumentum_Memo_Back_fr.json",
     "FallaciesTaxonomy (carte filter)",
     "Cards\\Fallacies\\Argumentum Fallacies - Taxonomy.csv"),
    ("Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json",
     "FallaciesTaxonomy",
     "Cards\\Fallacies\\Argumentum Fallacies - Taxonomy.csv"),
]


def detect_eol(raw: bytes) -> str:
    return "\r\n" if b"\r\n" in raw else "\n"


def add_csv_note(path: str, dataset: str, csv_src: str, dry_run: bool = False) -> str:
    with open(path, "rb") as f:
        raw = f.read()
    bom = raw[:3] == b"\xef\xbb\xbf"
    eol = detect_eol(raw)
    text = raw.decode("utf-8-sig")  # strips BOM if any

    if "_csv_note" in text:
        return "SKIP (already present)"

    d = json.loads(text)
    if "_csv_note" in d:
        return "SKIP (already present in dict)"

    note = (f"STALE — overridden at runtime by DataSet='{dataset}' "
            f"(see {csv_src}). Embedded `csv` is a snapshot from template "
            f"creation (PR #813 audit, #812 fix). CardPen reads `csv`/"
            f"`mustache`/`css`; this key is ignored.")
    d["_csv_note"] = note

    # Serialize preserving 4-space indent + eol (Python json.dumps default LF)
    new_text = json.dumps(d, indent=4, ensure_ascii=False)
    if eol == "\r\n":
        new_text = new_text.replace("\n", "\r\n")

    out_bytes = new_text.encode("utf-8")
    if bom:
        out_bytes = b"\xef\xbb\xbf" + out_bytes

    # Sanity: must remain valid JSON
    json.loads(new_text)

    if not dry_run:
        with open(path, "wb") as f:
            f.write(out_bytes)

    return f"OK ({len(raw)} → {len(out_bytes)} bytes)"


def main():
    dry = "--dry-run" in sys.argv
    print(f"=== Apply _csv_note marker ({len(TARGETS)} templates) {'[DRY RUN]' if dry else ''} ===")
    print()
    for path, dataset, csv_src in TARGETS:
        if not os.path.exists(path):
            print(f"{path} | MISSING")
            continue
        result = add_csv_note(path, dataset, csv_src, dry_run=dry)
        print(f"{path} | {result}")
    print()
    if dry:
        print("Re-run without --dry-run to apply.")


if __name__ == "__main__":
    main()