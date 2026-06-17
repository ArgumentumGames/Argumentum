#!/usr/bin/env python3
"""#457 DNN i18n — round-trip DoD test (extract -> verify -> reimport on fixture).

Self-contained proof that the 3 bricks work end-to-end on a committed fixture, with zero
prod mutation. Run with plain `python` (stdlib only). Exit 0 = pass.

What it proves:
  1. extract_dnn_ui_strings.py runs against the fixture templates and emits the dialect.
  2. The extracted key SET matches the expected fixture-derived set (anti-fab assertion).
  3. reimport_dnn_ui_strings.py `verify` reports key-set match vs a reference snapshot.
  4. reimport_dnn_ui_strings.py `reimport` renders the dry-run payload without writing.

Nothing is written outside this tool dir's scratch area (cleaned at the end).
"""
from __future__ import annotations

import os
import subprocess
import sys
import tempfile

HERE = os.path.dirname(os.path.abspath(__file__))
ROOT = os.path.abspath(os.path.join(HERE, "..", ".."))  # repo root


def _run(script: str, *args: str) -> str:
    """Run a sibling script; return stdout. Raise on non-zero exit."""
    res = subprocess.run(
        [sys.executable, os.path.join(HERE, script), *args],
        capture_output=True, text=True, check=False,
    )
    assert res.returncode == 0, (
        f"{script} exited {res.returncode}\nSTDOUT:\n{res.stdout}\nSTDERR:\n{res.stderr}"
    )
    return res.stdout


def main() -> int:
    fixture_root = os.path.join(HERE, "fixtures", "sample_templates")
    assert os.path.isdir(fixture_root), f"fixture root missing: {fixture_root}"

    with tempfile.TemporaryDirectory() as tmp:
        extracted = os.path.join(tmp, "extracted.csv")
        reference = os.path.join(HERE, "fixtures", "reference_snapshot.csv")

        # 1. Extract from fixture.
        out = _run("extract_dnn_ui_strings.py",
                   "--templates-root", fixture_root, "--out", extracted)
        print("[1/4] extract OK:", out.strip().splitlines()[0])

        # 2. Assert the extracted key set is the expected one (anti-fab).
        import csv as _csv
        with open(extracted, encoding="utf-8") as f:
            keys = {r["key"] for r in _csv.DictReader(f) if r.get("key")}
        expected = {
            "ui.fallacy.find_out_more",
            "ui.rules.players_range",
            "res.RuleSummary", "res.RuleMaterial", "res.RuleInstallation",
            "res.RuleVariants", "res.RuleMemoCard", "res.RuleMemoInstructions",
            "res.RuleMemoCardFileNamePrefix", "res.RuleMemoCardDownload",
        }
        assert keys == expected, f"key set drift:\n  expected={sorted(expected)}\n  got={sorted(keys)}"
        print(f"[2/4] key set OK ({len(keys)} keys): {sorted(keys)}")

        # 3. Verify against the fixture reference snapshot (key-set HARD match).
        vout = _run("reimport_dnn_ui_strings.py",
                    "verify", "--extracted", extracted, "--reference", reference)
        assert "key sets match" in vout, f"verify did not report match:\n{vout}"
        print("[3/4] verify OK (key sets match, res.* fr empty by design)")

        # 4. Reimport renders the dry-run payload (writes nothing).
        rout = _run("reimport_dnn_ui_strings.py", "reimport", "--csv", extracted)
        assert "RENDERED, NOT APPLIED" in rout and "Nothing was written" in rout
        assert "target: template_patch" in rout and "target: app_resource" in rout
        print("[4/4] reimport dry-run OK (payload rendered, nothing written)")

    print("\nROUND-TRIP DoD PASS: extract -> verify -> reimport on fixture, zero prod mutation.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
