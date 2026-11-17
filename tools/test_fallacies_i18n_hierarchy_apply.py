#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Unit tests for fallacies_i18n_hierarchy_apply.py (#712 + #716 consolidated apply).

Stdlib unittest only. Tests the logic contract (canonical map derivation, gap detection, conflict
abort, novel-key abort, apply round-trip byte-targeting) on synthetic CSV rows, plus a grounding
check on the real Fallacies CSV: the #712/#716 gap (48 rows x 3 EN cols = 144 cells) is either
still open (144, pre-apply inventory) or fully closed (0, applied — the state of master since
the apply landed); anything in between is a partial application and stays red.

Run: python tools/test_fallacies_i18n_hierarchy_apply.py
"""

import csv
import io
import os
import sys
import tempfile
import unittest

# Import the module under test (sibling file).
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import fallacies_i18n_hierarchy_apply as mod

REAL_CSV = os.path.join("Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")

FIELDS = ["PK", "text_fr", "Famille", "Sous-Famille", "Soussousfamille",
          "Family", "Subfamily", "Subsubfamily"]


def _row(pk, ssub_fr, fam_en="", sub_en="", ssub_en="", text="t"):
    return {
        "PK": str(pk), "text_fr": text, "Famille": "", "Sous-Famille": "",
        "Soussousfamille": ssub_fr,
        "Family": fam_en, "Subfamily": sub_en, "Subsubfamily": ssub_en,
    }


class CanonicalMapTests(unittest.TestCase):
    def test_single_consensus_tuple(self):
        rows = [
            _row(1, "Jeu de pouvoir", "Influence", "Psychological manipulation", "Power games"),
            _row(2, "Jeu de pouvoir", "Influence", "Psychological manipulation", "Power games"),
            _row(3, "Mensonge", "Cheating", "Spin doctoring", "Lying"),
        ]
        cmap, conflicts = mod.build_canonical_map(rows)
        self.assertEqual(cmap["Jeu de pouvoir"], ("Influence", "Psychological manipulation", "Power games"))
        self.assertEqual(cmap["Mensonge"], ("Cheating", "Spin doctoring", "Lying"))
        self.assertEqual(conflicts, [])

    def test_conflict_detected(self):
        rows = [
            _row(1, "X", "A", "B", "C"),
            _row(2, "X", "A", "B", "D"),  # same FR key, different Subsubfamily → conflict
        ]
        cmap, conflicts = mod.build_canonical_map(rows)
        self.assertNotIn("X", cmap)
        self.assertEqual(len(conflicts), 1)
        self.assertEqual(conflicts[0]["fr"], "X")

    def test_empty_fr_key_excluded(self):
        rows = [_row(1, "", "A", "B", "C")]  # no FR key → not in map
        cmap, conflicts = mod.build_canonical_map(rows)
        self.assertEqual(cmap, {})
        self.assertEqual(conflicts, [])


class PlanTests(unittest.TestCase):
    def test_gap_row_fills_all_three_empty(self):
        cmap = {"Jeu de pouvoir": ("Influence", "Psychological manipulation", "Power games")}
        # gap row: FR filled, all 3 EN empty
        rows = [_row(10, "Jeu de pouvoir")]
        plan, novel = mod.build_plan(rows, cmap)
        self.assertEqual(len(plan), 3)
        cols = {e["en_col"] for e in plan}
        self.assertEqual(cols, {"Family", "Subfamily", "Subsubfamily"})
        self.assertEqual(novel, [])

    def test_partial_gap_fills_only_empty(self):
        cmap = {"X": ("A", "B", "C")}
        rows = [_row(10, "X", fam_en="A")]  # Family already filled, 2 empty
        plan, novel = mod.build_plan(rows, cmap)
        self.assertEqual(len(plan), 2)
        cols = {e["en_col"] for e in plan}
        self.assertEqual(cols, {"Subfamily", "Subsubfamily"})

    def test_novel_key_aborts(self):
        cmap = {"X": ("A", "B", "C")}
        rows = [_row(10, "UNKNOWN")]  # FR key not in map
        plan, novel = mod.build_plan(rows, cmap)
        self.assertEqual(plan, [])
        self.assertEqual(len(novel), 1)
        self.assertEqual(novel[0]["fr"], "UNKNOWN")

    def test_no_fr_key_skipped(self):
        cmap = {"X": ("A", "B", "C")}
        rows = [_row(10, "")]  # family-header root (no FR key) → skipped, not novel
        plan, novel = mod.build_plan(rows, cmap)
        self.assertEqual(plan, [])
        self.assertEqual(novel, [])

    def test_full_row_not_a_gap(self):
        cmap = {"X": ("A", "B", "C")}
        rows = [_row(10, "X", "A", "B", "C")]  # all filled → not a gap
        plan, novel = mod.build_plan(rows, cmap)
        self.assertEqual(plan, [])


class ApplyRoundTripTests(unittest.TestCase):
    def _write_csv(self, rows):
        f = tempfile.NamedTemporaryFile(mode="wb", suffix=".csv", delete=False)
        buf = io.StringIO()
        w = csv.writer(buf, quoting=csv.QUOTE_MINIMAL, lineterminator="\r\n")
        w.writerow(FIELDS)
        for r in rows:
            w.writerow([r.get(c, "") for c in FIELDS])
        data = b"\xef\xbb\xbf" + buf.getvalue().encode("utf-8")  # with BOM
        f.write(data)
        f.close()
        return f.name

    def test_apply_fills_empty_cells_preserves_bom_crlf(self):
        cmap = {"X": ("A", "B", "C")}
        rows = [_row(10, "X")]  # gap row
        path = self._write_csv(rows)
        plan, _ = mod.build_plan(rows, cmap)
        n = mod.apply_plan(path, plan)
        self.assertEqual(n, 3)
        with open(path, "rb") as f:
            raw = f.read()
        self.assertTrue(raw.startswith(b"\xef\xbb\xbf"))  # BOM preserved
        self.assertIn(b"\r\n", raw)  # CRLF preserved
        # Verify the cells were filled.
        text = raw.decode("utf-8-sig")
        parsed = list(csv.DictReader(io.StringIO(text)))
        self.assertEqual(parsed[0]["Family"], "A")
        self.assertEqual(parsed[0]["Subfamily"], "B")
        self.assertEqual(parsed[0]["Subsubfamily"], "C")
        os.unlink(path)

    def test_apply_never_overwrites_filled_cell(self):
        cmap = {"X": ("A", "B", "C")}
        rows = [_row(10, "X", fam_en="EXISTING")]  # Family already has a value
        path = self._write_csv(rows)
        plan, _ = mod.build_plan(rows, cmap)  # plans Subfamily + Subsubfamily only
        n = mod.apply_plan(path, plan)
        self.assertEqual(n, 2)
        with open(path, "rb") as f:
            raw = f.read()
        parsed = list(csv.DictReader(io.StringIO(raw.decode("utf-8-sig"))))
        self.assertEqual(parsed[0]["Family"], "EXISTING")  # untouched
        os.unlink(path)


@unittest.skipUnless(os.path.exists(REAL_CSV), "Real Fallacies CSV not present (run from repo root).")
class RealCSVGroundingTests(unittest.TestCase):
    def test_real_csv_gap_open_144_or_applied_0(self):
        """Grounding on the living corpus, two coherent states (P24-24, #458):

        - 144 plan cells = the pre-apply inventory of #712/#716 (48 gap rows x 3 EN cols);
        - 0 plan cells = the gap fully closed: the consolidated apply has landed and every
          FR-keyed row carries its 3 EN hierarchy columns (measured on master 6ad2c9a1,
          2026-11-17: 1379/1379 FR-keyed rows complete, canonical map 63 keys).

        Any other count is a PARTIAL application or a NEW gap (a row added with an FR key
        but empty EN columns) — both must go through the apply instrument, so both stay red.
        """
        with open(REAL_CSV, encoding="utf-8-sig", newline="") as f:
            rows = list(csv.DictReader(f))
        cmap, conflicts = mod.build_canonical_map(rows)
        plan, novel = mod.build_plan(rows, cmap)
        self.assertEqual(conflicts, [], "no FR key should map to >1 EN tuple")
        self.assertEqual(novel, [], "every gap row's FR key must be in the map")
        self.assertIn(len(plan), (144, 0),
                      f"plan = {len(plan)} cells: neither the pre-apply inventory (144 = 48x3) "
                      f"nor the applied state (0) — partial application or new gap, re-audit")
        if len(plan) == 0:
            # Applied-state witness: the canonical map still derives from the corpus
            # (63 keys measured; the inventory spoke of 6 — the map has since grown with
            # the corpus, the floor keeps the derivation from being vacuously empty).
            self.assertGreaterEqual(len(cmap), 6,
                                    "canonical map should still derive from the living corpus")


if __name__ == "__main__":
    unittest.main(verbosity=2)
