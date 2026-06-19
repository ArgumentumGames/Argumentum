#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contract tests for the reusable anti-fab validator (#518) — `validate_taxonomy_annotations.py`.

Self-contained, stdlib-only (`unittest`). Run with plain `python`:

    python tools/test_validate_taxonomy_annotations.py
    python -m unittest tools.test_validate_taxonomy_annotations  # via repo root

What it pins (the contract the taxonomy scale-up lane depends on):
  1. detect_kind — schema auto-detection for the 4 proposition kinds.
  2. split_pks — ';'- and ','-separated PK-cell parsing.
  3. validate_virtues — PK membership + link_type HARD checks.
  4. validate_aif_scaleup — PK membership + attack_type HARD + coherence WARN.
  5. validate_aif_pilot — legacy pilot PK membership.
  6. validate_crosslinks — PK membership + link_type HARD + symmetry reciprocity HARD.
  7. build_walton_catalog — drift-detector scheme collection.
  8. Constants — CROSSLINK_TYPES (8), ATTACK_TYPES (3), ATTACK_COMPONENT_OK (3 coherent pairs).

The validator's per-kind functions take (rows, gt, walton_catalog) where `rows` is a list of
dicts and `gt` is a ground-truth map shaped like `load_ground_truth`'s output. The tests build
synthetic `rows` + `gt` in-memory (no real-corpus dependency) so they exercise the LOGIC, not
the data — a HARD-violation detection regression surfaces as a failing assertion.

Exit code: 0 = all pass.
"""
import os
import sys
import unittest

HERE = os.path.dirname(os.path.abspath(__file__))
if HERE not in sys.path:
    sys.path.insert(0, HERE)

import validate_taxonomy_annotations as V  # noqa: E402


# ─── Synthetic ground-truth + row builders ──────────────────────────────────
# Shaped exactly like load_ground_truth() output so the validators consume it.

def _gt(fallacies=None, virtues=None):
    """Build a minimal ground-truth map. fallacies/virtues = {pk_str: {...}}."""
    return {
        "fallacies": fallacies or {},
        "virtues": virtues or {},
    }


def _fall(pk, depth="3", famille="FamA", text_fr="Fallacy"):
    return {"text_fr": text_fr, "famille": famille, "depth": depth, "path": f"x.{pk}"}


def _virt(pk, depth="2", title_fr="Virtue", family_fr="FamV"):
    return {"title_fr": title_fr, "family_fr": family_fr, "depth": depth}


def _vrow(**kw):
    """A virtues-schema row. Defaults are valid; tests override the field under test."""
    base = {
        "virtue_pk": "136", "virtue_title": "Virtue",
        "prevented_family_pk": "1", "prevented_family_name": "FamA",
        "crossLink_Opposes": "777;759",
        "AIF_skosDirectRef": "Argument from Sign",
        "link_type": "crossLink_Opposes",
    }
    base.update(kw)
    return base


def _arow(**kw):
    """An aif-scaleup-schema row."""
    base = {
        "fallacy_pk": "777", "family": "FamA", "fallacy_name": "Fallacy",
        "RA_scheme": "Argument from Sign",
        "attack_type": "undermine", "attacked_component": "premise",
        "CA_node": "SomeConflict", "AIF_RA_node": "Sign_Inference",
        "AIF_CA_node": "SomeConflict_Conflict",
        "violated_cq": "Q?", "why_not_others": "n/a", "justification": "j",
    }
    base.update(kw)
    return base


def _crow(**kw):
    """A crosslinks-schema row."""
    base = {"source_pk": "777", "target_pk": "136", "link_type": "crossLink_Opposes",
            "symmetric": "false"}
    base.update(kw)
    return base


# ─── Constants contract ──────────────────────────────────────────────────────

class TestConstants(unittest.TestCase):
    def test_crosslink_types_count(self):
        self.assertEqual(len(V.CROSSLINK_TYPES), 8)

    def test_crosslink_types_membership(self):
        for t in ("crossLink_Opposes", "crossLink_Mirrors", "crossLink_Inverts"):
            self.assertIn(t, V.CROSSLINK_TYPES)

    def test_attack_types(self):
        self.assertEqual(V.ATTACK_TYPES, {"undermine", "undercut", "rebut"})

    def test_attack_component_coherent_pairs(self):
        # exactly the 3 ASPIC+ coherent pairs
        self.assertEqual(V.ATTACK_COMPONENT_OK, {
            ("undermine", "premise"),
            ("undercut", "inference_rule"),
            ("rebut", "conclusion"),
        })


# ─── detect_kind ─────────────────────────────────────────────────────────────

class TestDetectKind(unittest.TestCase):
    def test_virtues(self):
        self.assertEqual(V.detect_kind(["virtue_pk", "prevented_family_pk", "x"]), "virtues")

    def test_aif_scaleup(self):
        self.assertEqual(V.detect_kind(["fallacy_pk", "attack_type", "CA_node"]), "aif-scaleup")

    def test_aif_pilot_legacy(self):
        self.assertEqual(V.detect_kind(["fallacy_pk", "walton_scheme"]), "aif-pilot")

    def test_crosslinks(self):
        self.assertEqual(
            V.detect_kind(["source_pk", "target_pk", "symmetric", "link_type"]), "crosslinks")

    def test_unknown(self):
        self.assertEqual(V.detect_kind(["foo", "bar"]), "unknown")

    def test_aif_scaleup_takes_precedence_over_pilot(self):
        # fallacy_pk + attack_type + walton_scheme -> scaleup (not legacy pilot)
        self.assertEqual(
            V.detect_kind(["fallacy_pk", "attack_type", "walton_scheme"]), "aif-scaleup")


# ─── split_pks ───────────────────────────────────────────────────────────────

class TestSplitPks(unittest.TestCase):
    def test_empty(self):
        self.assertEqual(V.split_pks(""), [])
        self.assertEqual(V.split_pks(None), [])

    def test_semicolon(self):
        self.assertEqual(V.split_pks("777;759"), ["777", "759"])

    def test_comma_normalized(self):
        self.assertEqual(V.split_pks("777,759"), ["777", "759"])

    def test_mixed_and_whitespace(self):
        self.assertEqual(V.split_pks(" 777 ; 759 ,3 "), ["777", "759", "3"])

    def test_single(self):
        self.assertEqual(V.split_pks("777"), ["777"])


# ─── validate_virtues ────────────────────────────────────────────────────────

class TestValidateVirtues(unittest.TestCase):
    def test_clean_row(self):
        gt = _gt(fallacies={"1": _fall("1", depth="1"), "777": _fall("777"), "759": _fall("759")},
                 virtues={"136": _virt("136")})
        hard, warn = V.validate_virtues([_vrow()], gt, set())
        self.assertEqual(hard, [])
        # warnings may include the scheme drift note since walton_catalog is empty -> no warn
        self.assertEqual(warn, [])

    def test_virtue_pk_not_in_corpus_HARD(self):
        gt = _gt(fallacies={"1": _fall("1", depth="1")}, virtues={})  # 136 absent
        hard, _ = V.validate_virtues([_vrow()], gt, set())
        self.assertTrue(any("virtue_pk 136 not in Virtues corpus" in m for m in hard))

    def test_family_pk_wrong_depth_HARD(self):
        # family must be depth 1; depth 3 -> HARD
        gt = _gt(fallacies={"1": _fall("1", depth="3")}, virtues={"136": _virt("136")})
        hard, _ = V.validate_virtues([_vrow()], gt, set())
        self.assertTrue(any("depth=3" in m and "expected family depth 1" in m for m in hard))

    def test_family_pk_not_in_corpus_HARD(self):
        gt = _gt(fallacies={}, virtues={"136": _virt("136")})
        hard, _ = V.validate_virtues([_vrow()], gt, set())
        self.assertTrue(any("prevented_family_pk 1 not in Fallacies corpus" in m for m in hard))

    def test_opposed_pk_not_in_corpus_HARD(self):
        gt = _gt(fallacies={"1": _fall("1", depth="1")}, virtues={"136": _virt("136")})
        # crossLink_Opposes 777;759 both absent
        hard, _ = V.validate_virtues([_vrow()], gt, set())
        self.assertEqual(len([m for m in hard if "opposed fallacy PK" in m]), 2)

    def test_bad_link_type_HARD(self):
        gt = _gt(fallacies={"1": _fall("1", depth="1"), "777": _fall("777"), "759": _fall("759")},
                 virtues={"136": _virt("136")})
        hard, _ = V.validate_virtues([_vrow(link_type="crossLink_Bogus")], gt, set())
        self.assertTrue(any("not one of the 8 cross-link types" in m for m in hard))

    def test_novel_scheme_is_WARN_not_hard(self):
        gt = _gt(fallacies={"1": _fall("1", depth="1"), "777": _fall("777"), "759": _fall("759")},
                 virtues={"136": _virt("136")})
        catalog = {"Argument from Sign"}  # scheme used is NOT in catalog -> novel
        hard, warn = V.validate_virtues(
            [_vrow(AIF_skosDirectRef="Argument from Bogus")], gt, catalog)
        self.assertEqual(hard, [])
        self.assertTrue(any("NOVEL scheme" in m for m in warn))


# ─── validate_aif_scaleup ────────────────────────────────────────────────────

class TestValidateAifScaleup(unittest.TestCase):
    def test_clean_row(self):
        gt = _gt(fallacies={"777": _fall("777")})
        hard, warn = V.validate_aif_scaleup([_arow()], gt, set())
        self.assertEqual(hard, [])
        self.assertEqual(warn, [])

    def test_pk_not_in_corpus_HARD(self):
        gt = _gt(fallacies={})
        hard, _ = V.validate_aif_scaleup([_arow()], gt, set())
        self.assertTrue(any("fallacy_pk 777 not in Fallacies corpus" in m for m in hard))

    def test_bad_attack_type_HARD(self):
        gt = _gt(fallacies={"777": _fall("777")})
        hard, _ = V.validate_aif_scaleup([_arow(attack_type="refute")], gt, set())
        self.assertTrue(any("attack_type 'refute'" in m for m in hard))

    def test_coherent_pairs_no_warn(self):
        gt = _gt(fallacies={"777": _fall("777")})
        for at, comp in V.ATTACK_COMPONENT_OK:
            hard, warn = V.validate_aif_scaleup(
                [_arow(attack_type=at, attacked_component=comp)], gt, set())
            self.assertEqual(hard, [])
            self.assertFalse(any("NOVEL pair" in m for m in warn),
                             f"coherent pair {at}/{comp} flagged as novel")

    def test_incoherent_pair_WARN_not_hard(self):
        gt = _gt(fallacies={"777": _fall("777")})
        hard, warn = V.validate_aif_scaleup(
            [_arow(attack_type="undermine", attacked_component="conclusion")], gt, set())
        self.assertEqual(hard, [])
        self.assertTrue(any("NOVEL pair" in m for m in warn))

    def test_name_drift_WARN(self):
        gt = _gt(fallacies={"777": _fall("777", text_fr="Real Name")})
        hard, warn = V.validate_aif_scaleup([_arow(fallacy_name="Wrong Name")], gt, set())
        self.assertEqual(hard, [])
        self.assertTrue(any("fallacy_name" in m and "!=" in m for m in warn))


# ─── validate_aif_pilot (legacy #505) ────────────────────────────────────────

class TestValidateAifPilot(unittest.TestCase):
    def test_clean(self):
        gt = _gt(fallacies={"777": _fall("777")})
        row = {"fallacy_pk": "777", "walton_scheme": "Argument from Sign", "fallacy_name": "Fallacy"}
        hard, warn = V.validate_aif_pilot([row], gt, set())
        self.assertEqual(hard, [])
        self.assertEqual(warn, [])

    def test_pk_missing_HARD(self):
        gt = _gt(fallacies={})
        row = {"fallacy_pk": "999", "walton_scheme": "Argument from Sign"}
        hard, _ = V.validate_aif_pilot([row], gt, set())
        self.assertTrue(any("fallacy_pk 999 not in Fallacies corpus" in m for m in hard))


# ─── validate_crosslinks (symmetry HARD) ─────────────────────────────────────

class TestValidateCrosslinks(unittest.TestCase):
    def test_asymmetric_clean(self):
        gt = _gt(fallacies={"777": _fall("777"), "136": _fall("136")})
        hard, _ = V.validate_crosslinks([_crow(symmetric="false")], gt, set())
        self.assertEqual(hard, [])

    def test_symmetric_with_reverse_edge_clean(self):
        gt = _gt(fallacies={"777": _fall("777"), "136": _fall("136")})
        rows = [
            _crow(source_pk="777", target_pk="136", symmetric="true"),
            _crow(source_pk="136", target_pk="777", symmetric="true"),
        ]
        hard, _ = V.validate_crosslinks(rows, gt, set())
        self.assertEqual(hard, [])

    def test_symmetric_missing_reverse_HARD(self):
        gt = _gt(fallacies={"777": _fall("777"), "136": _fall("136")})
        rows = [_crow(source_pk="777", target_pk="136", symmetric="true")]  # no reverse edge
        hard, _ = V.validate_crosslinks(rows, gt, set())
        self.assertTrue(any("NO reverse edge" in m for m in hard))

    def test_bad_link_type_HARD(self):
        gt = _gt(fallacies={"777": _fall("777"), "136": _fall("136")})
        hard, _ = V.validate_crosslinks([_crow(link_type="crossLink_Bogus")], gt, set())
        self.assertTrue(any("not one of the 8 cross-link types" in m for m in hard))

    def test_source_pk_missing_HARD(self):
        gt = _gt(fallacies={"136": _fall("136")})  # 777 absent
        hard, _ = V.validate_crosslinks([_crow()], gt, set())
        self.assertTrue(any("source_pk 777 not in Fallacies corpus" in m for m in hard))


# ─── build_walton_catalog (drift detector) ───────────────────────────────────

class TestBuildWaltonCatalog(unittest.TestCase):
    def test_collects_scheme_names_from_annotations_csv(self):
        import tempfile, csv
        with tempfile.TemporaryDirectory() as d:
            # write a fake annotations CSV with a scheme name
            p = os.path.join(d, "x-annotations.csv")
            with open(p, "w", encoding="utf-8", newline="") as f:
                w = csv.writer(f)
                w.writerow(["fallacy_pk", "RA_scheme", "other"])
                w.writerow(["777", "Argument from Sign", "z"])
                w.writerow(["778", "Argument from Bias", "z"])
            cat = V.build_walton_catalog(d)
            self.assertIn("Argument from Sign", cat)
            self.assertIn("Argument from Bias", cat)
            # 'other' (single word, not capitalized multi-word) excluded
            self.assertNotIn("z", cat)

    def test_missing_dir_returns_empty(self):
        self.assertEqual(V.build_walton_catalog("/nonexistent/path/xyz"), set())

    def test_skips_non_annotation_files(self):
        import tempfile
        with tempfile.TemporaryDirectory() as d:
            with open(os.path.join(d, "readme.md"), "w", encoding="utf-8") as f:
                f.write("Argument from Sign")
            self.assertEqual(V.build_walton_catalog(d), set())


if __name__ == "__main__":
    rc = unittest.main(verbosity=2, exit=False).result
    sys.exit(0 if rc.wasSuccessful() else 1)
