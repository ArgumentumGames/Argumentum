#!/usr/bin/env python3
"""#457 DNN site localization — CSV -> DNN re-import DRY-RUN verifier (BRICK 3).

Proves the round-trip extract -> CSV -> re-import on a **fixture**, with **zero prod
mutation**. It does NOT touch the live DNN database, the portal, or 2sxc App Resources —
the live re-import is DB/RDP-gated (jsboige). This tool is the dry-run/diff half:

  * ``verify``  — compare a standalone extraction (from extract_dnn_ui_strings.py) against
                  a reference CSV (e.g. docs/dnn-localization/dnn-ui-strings.csv). Reports
                  added / removed / changed rows by ``key``. Non-zero exit if the key SET
                  diverges (the extractable surface changed — a human must re-audit).
  * ``reimport``— render the canonical DNN re-import payload (one record per row) from a
                  CSV, **to stdout only**. This is what a future live re-import would feed
                  to the DNN/2sxc App-Resources writer. It is printed, never applied.

The verification contract:

  * ``key`` SET match is HARD (exit 1 on divergence) — a missing/new key means the
    template surface changed and the reference CSV is stale or the extractor drifted.
  * ``fr`` / target values are reported as WARNINGS, not failures — the canonical FR for
    res.* is DB-only and legitimately empty in a fresh extraction; target cells are empty
    until a translation run populates them. A value drift on ui.* fr (repo-hardcoded) IS
    surfaced prominently because that value IS in the repo.

USAGE
    # Dry-run round-trip against the reference CSV (DoD proof):
    python tools/dnn_i18n/reimport_dnn_ui_strings.py verify \\
        --extracted <fresh_extract.csv> --reference docs/dnn-localization/dnn-ui-strings.csv

    # Render the re-import payload to stdout (never writes anywhere):
    python tools/dnn_i18n/reimport_dnn_ui_strings.py reimport \\
        --csv <translations.csv>
"""
from __future__ import annotations

import argparse
import csv
import sys
from typing import Dict, List

# Same dialect as the extractor / reference CSV.
KEY_COLUMNS = ["key"]
VALUE_COLUMNS = ["fr", "en", "ru", "pt", "es", "ar", "fa", "zh"]
META_COLUMNS = ["context", "source_file", "notes"]


def _load(path: str) -> Dict[str, dict]:
    rows: Dict[str, dict] = {}
    with open(path, encoding="utf-8", newline="") as f:
        for row in csv.DictReader(f):
            key = (row.get("key") or "").strip()
            if not key:
                continue
            rows[key] = row
    return rows


def _prefix(key: str) -> str:
    return key.split(".", 1)[0] if "." in key else key


def verify(extracted_path: str, reference_path: str) -> int:
    ext = _load(extracted_path)
    ref = _load(reference_path)

    ext_keys, ref_keys = set(ext), set(ref)
    added = sorted(ext_keys - ref_keys)
    removed = sorted(ref_keys - ext_keys)
    common = sorted(ext_keys & ref_keys)

    print(f"extracted: {len(ext)} rows | reference: {len(ref)} rows | common: {len(common)}")
    if added:
        print(f"  + ADDED (in extraction, not in reference): {added}")
    if removed:
        print(f"  - REMOVED (in reference, not in extraction): {removed}")

    # HARD contract: the key SET must match. A divergence means the template surface
    # changed (refactor) or the extractor/reference drifted — a human must re-audit.
    hard_fail = bool(added or removed)

    # Value drift on common keys.
    ui_value_drifts = []
    res_value_notes = []
    for k in common:
        e, r = ext[k], ref[k]
        if _prefix(k) == "ui":
            # ui.* fr IS in the repo (hardcoded). A drift here is real.
            if (e.get("fr") or "").strip() != (r.get("fr") or "").strip():
                ui_value_drifts.append((k, r.get("fr"), e.get("fr")))
        elif _prefix(k) == "res":
            # res.* fr is DB-only — empty in a fresh extraction, possibly populated in
            # the reference (INFERRED scaffold). Report but do not fail.
            if (e.get("fr") or "").strip() != (r.get("fr") or "").strip():
                res_value_notes.append(k)

    if ui_value_drifts:
        print("  ! UI.* FR VALUE DRIFT (repo-hardcoded — real divergence):")
        for k, old, new in ui_value_drifts:
            print(f"      {k}: reference fr={old!r}  extracted fr={new!r}")
    if res_value_notes:
        print(f"  ~ res.* fr differs (expected — DB-only in fresh extract): {res_value_notes}")

    if hard_fail:
        print("\nRESULT: KEY-SET DIVERGENCE — re-audit required (exit 1).")
        return 1
    print("\nRESULT: key sets match. res.* fr empty-in-extract is by design (DB-only).")
    return 0


def reimport_render(csv_path: str) -> int:
    """Render the canonical DNN/2sxc App-Resources re-import payload to stdout.

    One record per row. This is the shape a live re-import (jsboige, gated) would consume.
    Printed only — NEVER applied. The live writer is out of scope (DB/RDP-gated).
    """
    rows = _load(csv_path)
    print(f"# DNN re-import DRY-RUN payload (RENDERED, NOT APPLIED) — {len(rows)} records")
    print("# target: 2sxc App Resources + template hardcoded-string patches")
    print("# LIVE APPLY IS GATED (jsboige DB/RDP). This output writes nothing.\n")
    for key, row in sorted(rows.items()):
        prefix = _prefix(key)
        if prefix == "ui":
            # ui.* = hardcoded template string patch. The re-import would rewrite the
            # literal in the .cshtml to read the culture-correct value (fixes the i18n bug
            # flagged in PHASE1 §4 for ui.fallacy.find_out_more).
            target = "template_patch"
        elif prefix == "res":
            # res.* = 2sxc App Resources dictionary entry (key -> per-language value).
            target = "app_resource"
        else:
            target = "unknown"
        translations = {lang: (row.get(lang) or "").strip() for lang in VALUE_COLUMNS if lang != "fr"}
        translations = {l: v for l, v in translations.items() if v}
        fr = (row.get("fr") or "").strip()
        print(f"- key: {key}")
        print(f"  target: {target}")
        print(f"  source_file: {row.get('source_file', '')}")
        if fr:
            print(f"  fr: {fr}")
        for lang, val in translations.items():
            print(f"  {lang}: {val}")
        note = (row.get("notes") or "").strip()
        if note:
            print(f"  notes: {note}")
        print()
    print("# END dry-run payload. Nothing was written.")
    return 0


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__,
                                 formatter_class=argparse.RawDescriptionHelpFormatter)
    sub = ap.add_subparsers(dest="cmd", required=True)

    v = sub.add_parser("verify", help="diff an extraction vs a reference CSV (key-set HARD)")
    v.add_argument("--extracted", required=True)
    v.add_argument("--reference", required=True)

    r = sub.add_parser("reimport", help="render the re-import payload to stdout (dry-run)")
    r.add_argument("--csv", required=True)

    args = ap.parse_args()
    if args.cmd == "verify":
        return verify(args.extracted, args.reference)
    if args.cmd == "reimport":
        return reimport_render(args.csv)
    return 2


if __name__ == "__main__":
    raise SystemExit(main())
