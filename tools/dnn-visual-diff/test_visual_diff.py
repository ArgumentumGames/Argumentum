#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Offline unit tests for dnn-visual-diff (no network, no playwright).

Covers: pure helpers, route config validation (zero pairs / structural
errors), cache-buster outcomes (incl. fragment), freshness SEMANTICS
(marker presence is NOT currency), known-delta registry, diff engine
(identical / noise / real diff / dimension mismatch / empty / corrupt /
uniform-capture), memory guard BEFORE decode, HTTP gates (an error page can
never be compared into an IDENTIQUE verdict — regression tests for the three
parent-reproduced false successes), aggregate semantics (empty rows fail),
nondeterminism accumulation, fingerprint verification, offline report mode,
parameter validation, run preservation, HTML escaping, and the
no-aesthetic-verdict invariant.

Run: python -m unittest test_visual_diff -v   (from tools/dnn-visual-diff/)
"""

from __future__ import annotations

import argparse
import json
import os
import sys
import tempfile
import types
import unittest
from unittest import mock

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))

from PIL import Image, ImageDraw  # noqa: E402

import visual_diff as vd  # noqa: E402


def make_entry(name: str, prod: Dict, preprod: Dict, **extra) -> Dict:
    entry = {"name": name, "note": "", "errors": [],
             "expect_login_redirect": False, "expected_status_both": None,
             "expected_markers": None, "hosts": {"prod": prod, "preprod": preprod},
             "nondeterminism": {}}
    entry.update(extra)
    return entry


class SlugAndHashTests(unittest.TestCase):
    def test_slugify_strips_unsafe_and_transliterates(self):
        self.assertEqual(vd.slugify("Règles / accueil!"), "regles-accueil")
        self.assertEqual(vd.slugify("Téléchargements"), "telechargements")
        self.assertEqual(vd.slugify("///"), "route")  # never empty

    def test_sha256_stable(self):
        self.assertEqual(vd.sha256_bytes(b"x"), vd.sha256_bytes(b"x"))
        self.assertNotEqual(vd.sha256_bytes(b"x"), vd.sha256_bytes(b"y"))

    def test_now_iso_has_timezone(self):
        iso = vd.now_iso()
        self.assertRegex(iso, r"[+-]\d{2}:\d{2}$")  # local offset present


class RoutesConfigTests(unittest.TestCase):
    def _write(self, obj):
        f = tempfile.NamedTemporaryFile("w", suffix=".json", delete=False, encoding="utf-8")
        json.dump(obj, f)
        f.close()
        self.addCleanup(os.unlink, f.name)
        return f.name

    def test_valid_config_loads(self):
        path = self._write({
            "hosts": {"prod": "https://p", "preprod": "https://q"},
            "pairs": [{"name": "home", "prod_path": "/"},
                      {"name": "two", "prod_path": "/b", "enabled": False}]})
        hosts, pairs = vd.load_routes(path)
        self.assertEqual(hosts["prod"], "https://p")
        self.assertEqual(len(pairs), 2)
        self.assertFalse(pairs[1].enabled)

    def test_zero_pairs_is_config_error(self):
        path = self._write({"hosts": {"prod": "p", "preprod": "q"}, "pairs": []})
        with self.assertRaises(vd.RouteConfigError):
            vd.load_routes(path)

    def test_missing_file_is_config_error(self):
        with self.assertRaises(vd.RouteConfigError):
            vd.load_routes(os.path.join(tempfile.gettempdir(), "nope-xyz.json"))

    def test_missing_required_keys(self):
        for obj in (
            {},
            {"hosts": {"prod": "p", "preprod": "q"}},
            {"pairs": [{"name": "a", "prod_path": "/"}]},
            {"hosts": {"prod": "p"}, "pairs": [{"name": "a", "prod_path": "/"}]},
        ):
            with self.assertRaises(vd.RouteConfigError, msg=str(obj)):
                vd.load_routes(self._write(obj))

    def test_pair_missing_fields(self):
        path = self._write({"hosts": {"prod": "p", "preprod": "q"},
                            "pairs": [{"name": "a"}]})
        with self.assertRaises(vd.RouteConfigError):
            vd.load_routes(path)

    def test_duplicate_slug_rejected(self):
        path = self._write({"hosts": {"prod": "p", "preprod": "q"},
                            "pairs": [{"name": "Règles", "prod_path": "/"},
                                      {"name": "regles", "prod_path": "/x"}]})
        with self.assertRaises(vd.RouteConfigError):
            vd.load_routes(path)

    def test_expected_markers_must_be_object(self):
        path = self._write({"hosts": {"prod": "p", "preprod": "q"},
                            "pairs": [{"name": "a", "prod_path": "/",
                                       "expected_markers": "nope"}]})
        with self.assertRaises(vd.RouteConfigError):
            vd.load_routes(path)


class CacheBusterTests(unittest.TestCase):
    def test_append_without_query(self):
        self.assertEqual(vd.append_cache_buster("/x", "7"), "/x?cb=7")

    def test_append_with_query(self):
        self.assertEqual(vd.append_cache_buster("/x?y=1", "7"), "/x?y=1&cb=7")

    def test_fragment_stays_last(self):
        self.assertEqual(vd.append_cache_buster("/x#frag", "7"), "/x?cb=7#frag")
        self.assertEqual(vd.append_cache_buster("/x?a=1#frag", "7"),
                         "/x?a=1&cb=7#frag")

    def test_outcomes(self):
        self.assertEqual(vd.detect_buster_outcome("/?cb=1", "/x?cb=1"), "kept-query")
        self.assertEqual(vd.detect_buster_outcome("/?cb=1", "/Argumentum/cb/1"),
                         "rewritten-to-path")
        self.assertEqual(vd.detect_buster_outcome("/?cb=1", "/elsewhere"), "lost")


class MarkerExpectationTests(unittest.TestCase):
    """Regression round 2: presence of tosic_sexycontent + expectations {}
    used to yield 'proven-against-expected'. The qualification is REMOVED:
    the tool only reports marker expectations matched/mismatched/none-declared,
    and freshness stays 'unknown' — no independent deployment proof exists
    client-side, and no freshness protocol is invented."""

    PROD = ("x tosic_sexycontent y jquery-3.5.1.min.js jquery-migrate.min.js GTM-TZBQ57M "
            "googletagmanager.com/ns.html?id=GTM-TZBQ57M /js/dnn.js?cdv=181 "
            "url(http://db.onlinewebfonts.com/t/x.eot)")
    PRE = "x tosic.sxc y tosic.sxc jquery-3.7.1.min.js GTM-TZBQ57M /js/dnn.js?cdv=325"

    def test_parse_counts(self):
        m = vd.parse_freshness_markers(self.PROD)
        self.assertEqual(m["tosic_sexycontent"], 1)
        self.assertEqual(m["tosic_sxc"], 0)
        self.assertEqual(m["gtm_ids"], ["GTM-TZBQ57M"])
        self.assertEqual(m["gtm_noscript"], 1)
        self.assertEqual(m["cdv"], "181")
        self.assertEqual(m["jquery"], "3.5.1")
        self.assertEqual(m["jquery_migrate"], 1)
        self.assertEqual(m["onlinewebfonts_http"], 1)

    def test_empty_expectation_dict_declares_nothing(self):
        # reviewer probe: presence + {} must NOT confer any qualification
        m = vd.parse_freshness_markers(self.PROD)
        state, _ = vd.check_marker_expectations(m, {})
        self.assertEqual(state, "none-declared")

    def test_no_expectation_declares_nothing(self):
        state, _ = vd.check_marker_expectations(
            vd.parse_freshness_markers(self.PROD), None)
        self.assertEqual(state, "none-declared")

    def test_outcomes_are_only_matched_mismatched_none(self):
        m = vd.parse_freshness_markers(self.PROD)
        outcomes = {
            vd.check_marker_expectations(m, None)[0],
            vd.check_marker_expectations(m, {})[0],
            vd.check_marker_expectations(m, {"tosic_sexycontent>": "0"})[0],
            vd.check_marker_expectations(m, {"tosic_sxc>": "0"})[0],
        }
        self.assertEqual(outcomes, {"none-declared", "matched", "mismatched"})
        # no 'proven'/'fresh' qualification may ever come back
        for out in outcomes:
            self.assertNotIn("proven", out)
            self.assertNotIn("fresh", out)

    def test_matched_is_not_freshness(self):
        m = vd.parse_freshness_markers(self.PROD)
        state, det = vd.check_marker_expectations(m, {"tosic_sexycontent>": "0"})
        self.assertEqual(state, "matched")
        self.assertTrue(all("OK" in d for d in det))
        # matched expectations do NOT flip any freshness claim: the sidecar
        # state is written by run_capture as a constant 'unknown' (verified
        # by the report rendering test below)
        self.assertNotIn("proven", state)

    def test_violated_expectation_is_mismatched(self):
        m = vd.parse_freshness_markers(self.PRE)
        state, det = vd.check_marker_expectations(m, {"tosic_sexycontent>": "0"})
        self.assertEqual(state, "mismatched")
        self.assertTrue(any("VIOLATED" in d for d in det))

    def test_expectation_operators(self):
        m = {"tosic_sxc": 3, "cdv": "325"}
        ok, _ = vd.check_expected_markers(m, {"cdv=": "325"})
        self.assertTrue(ok)
        ok, _ = vd.check_expected_markers(m, {"cdv!=": "181"})
        self.assertTrue(ok)
        ok, _ = vd.check_expected_markers(m, {"cdv=": "181"})
        self.assertFalse(ok)
        ok, _ = vd.check_expected_markers(m, {"tosic_sxc>": "2"})
        self.assertTrue(ok)
        ok, _ = vd.check_expected_markers(m, {"tosic_sxc": "2"})
        self.assertTrue(ok)  # bare field defaults to "count > operand" (3 > 2)
        ok, _ = vd.check_expected_markers(m, {"tosic_sxc": "3"})
        self.assertFalse(ok)  # 3 > 3 is False — strict

    def test_known_deltas_classified(self):
        kd = vd.classify_known_deltas(vd.parse_freshness_markers(self.PROD),
                                      vd.parse_freshness_markers(self.PRE))
        ids = [k.split(":")[0] for k in kd]
        for expected in ("D1", "D2", "D4", "D5", "D6", "D7"):
            self.assertIn(expected, ids, f"{expected} missing from {ids}")

    def test_d1_fires_both_directions(self):
        m_old = {"tosic_sexycontent": 1, "tosic_sxc": 0}
        m_new = {"tosic_sexycontent": 0, "tosic_sxc": 2}
        ids = lambda a, b: [k.split(":")[0] for k in vd.classify_known_deltas(a, b)]
        self.assertIn("D1", ids(m_old, m_new))
        self.assertIn("D1", ids(m_new, m_old))
        self.assertNotIn("D1", ids(m_old, m_old))

    def test_identical_markers_no_deltas(self):
        m = vd.parse_freshness_markers(self.PRE)
        self.assertEqual(vd.classify_known_deltas(m, m), [])


class DiffEngineTests(unittest.TestCase):
    def setUp(self):
        self.tmp = tempfile.TemporaryDirectory()
        self.addCleanup(self.tmp.cleanup)
        self.dir = self.tmp.name
        self.base = Image.new("RGB", (64, 64), (100, 100, 100))
        self.pa = self._save(self.base, "a.png")

    def _save(self, img, name):
        p = os.path.join(self.dir, name)
        img.save(p)
        return p

    def test_identical_zero(self):
        m = vd.compute_diff(self.pa, self._save(self.base, "b.png"))
        self.assertTrue(m.dims_match)
        self.assertEqual(m.pct_raw, 0.0)
        self.assertEqual(m.pct_strong, 0.0)
        self.assertEqual(m.max_delta, 0)

    def test_noise_split_raw_vs_strong(self):
        px = [(100, 100, 100) if (x + y) % 2 == 0 else (101, 101, 101)
              for y in range(64) for x in range(64)]
        noisy = Image.new("RGB", (64, 64))
        noisy.putdata(px)
        m = vd.compute_diff(self.pa, self._save(noisy, "n.png"))
        self.assertGreaterEqual(m.pct_raw, 49.0)
        self.assertLessEqual(m.pct_raw, 51.0)
        self.assertEqual(m.pct_strong, 0.0)

    def test_real_difference_seen(self):
        blk = self.base.copy()
        ImageDraw.Draw(blk).rectangle([8, 8, 24, 24], fill=(0, 0, 0))
        m = vd.compute_diff(self.pa, self._save(blk, "blk.png"))
        self.assertGreaterEqual(m.pct_strong, 5.0)
        self.assertEqual(m.max_delta, 100)

    def test_dimension_gap_explicit_no_resize(self):
        small = Image.new("RGB", (64, 32), (100, 100, 100))
        m = vd.compute_diff(self.pa, self._save(small, "s.png"))
        self.assertFalse(m.dims_match)
        verdict, reasons = vd.classify_pair(m, [], [])
        self.assertEqual(verdict, vd.VERDICT_DIM_GAP)
        self.assertTrue(any("dimensions differ" in r for r in reasons))

    def test_empty_capture_detected(self):
        empty = os.path.join(self.dir, "e.png")
        open(empty, "wb").close()
        with self.assertRaises(vd.EmptyCaptureError):
            vd.compute_diff(empty, empty)

    def test_corrupt_image_detected(self):
        corrupt = os.path.join(self.dir, "c.png")
        with open(corrupt, "wb") as fh:
            fh.write(b"\x89PNG\r\n\x1a\nbroken")
        with self.assertRaises(vd.CorruptImageError):
            vd.compute_diff(corrupt, corrupt)

    def test_missing_image_detected(self):
        with self.assertRaises(vd.CorruptImageError):
            vd.compute_diff(os.path.join(self.dir, "ghost.png"), self.pa)

    def test_low_delta_is_descriptive_not_causal(self):
        v, reasons = vd.classify_pair(
            vd.compute_diff(self.pa, self._save(self.base, "same.png")), [], [])
        self.assertEqual(v, vd.VERDICT_IDENTICAL)
        self.assertTrue(any("DESCRIPTIVE" in r and "cause" in r for r in reasons))
        self.assertFalse(any("anticrénelage" in r for r in reasons))

    def test_verdicts(self):
        v, _ = vd.classify_pair(vd.compute_diff(self.pa, self._save(self.base, "s2.png")),
                                [], [])
        self.assertEqual(v, vd.VERDICT_IDENTICAL)
        v, _ = vd.classify_pair(vd.compute_diff(self.pa, self._save(self.base, "s3.png")),
                                ["D4: x"], [])
        self.assertEqual(v, vd.VERDICT_KNOWN_DELTA)
        blk = self.base.copy()
        ImageDraw.Draw(blk).rectangle([0, 0, 63, 63], fill=(0, 0, 0))
        v, _ = vd.classify_pair(vd.compute_diff(self.pa, self._save(blk, "d.png")), [], [])
        self.assertEqual(v, vd.VERDICT_DIFF)
        v, _ = vd.classify_pair(None, [], ["preprod: CAPTURE ERROR boom"])
        self.assertEqual(v, vd.VERDICT_ERROR)

    def test_memory_guard_trips_on_header_before_decode(self):
        big = self._save(Image.new("RGB", (4200, 4200), (0, 0, 0)), "big.png")  # 17.6 MPx
        with self.assertRaises(vd.MemoryLimitError):
            vd.memory_guard(big, 10.0)
        vd.memory_guard(self.pa, 10.0)  # under the guard: no exception

    def test_uniform_and_tiny_captures_flagged(self):
        uni = self._save(Image.new("RGB", (200, 200), (255, 255, 255)), "uni.png")
        susp, detail = vd.is_suspicious_capture(uni)
        self.assertTrue(susp); self.assertIn("uniform", detail)
        tiny = self._save(Image.new("RGB", (10, 10), (1, 2, 3)), "tiny.png")
        # gradient-free but TINY -> suspicious
        susp, detail = vd.is_suspicious_capture(tiny)
        self.assertTrue(susp); self.assertIn("tiny", detail)
        # textured capture: not flagged
        grad = Image.new("RGB", (64, 64))
        grad.putdata([(x * 4 % 256, y * 4 % 256, 7) for y in range(64) for x in range(64)])
        susp, _ = vd.is_suspicious_capture(self._save(grad, "grad.png"))
        self.assertFalse(susp)


class HTTPGateTests(unittest.TestCase):
    """Regression for the parent-reproduced false success: two uniform
    HTTP-500 captures used to compare as IDENTIQUE-BRUIT with global success.
    The HTTP gate runs BEFORE any pixel logic and always yields a failing
    verdict — even for a declared-expected status."""

    def _uniform_pair(self, tmp, name="u"):
        p = os.path.join(tmp, f"{name}.png")
        Image.new("RGB", (300, 300), (240, 240, 240)).save(p)
        return p

    def test_500_both_is_http_error_never_identical(self):
        gate, reasons = vd.http_gate(500, 500, None)
        self.assertEqual(gate, vd.VERDICT_HTTP_ERROR)
        self.assertTrue(any("comparison not performed" in r for r in reasons))

    def test_uniform_500_pair_end_to_end(self):
        with tempfile.TemporaryDirectory() as tmp:
            p = self._uniform_pair(tmp)
            entries = [make_entry("x",
                                  {"http_status": 500, "memory_limit": False,
                                   "screenshot": os.path.basename(p)},
                                  {"http_status": 500, "memory_limit": False,
                                   "screenshot": os.path.basename(p)})]
            rows = vd.build_report_rows(entries, tmp)
            self.assertEqual(rows[0]["verdict"], vd.VERDICT_HTTP_ERROR)
            self.assertIsNone(rows[0]["metrics"])  # never compared
            clean, _ = vd.aggregate(rows, inverse_ok=True)
            self.assertFalse(clean)  # explicit global failure

    def test_expected_404_distinct_but_still_failing(self):
        gate, _ = vd.http_gate(404, 404, 404)
        self.assertEqual(gate, vd.VERDICT_EXPECTED_HTTP)
        self.assertNotEqual(gate, vd.VERDICT_IDENTICAL)
        clean, _ = vd.aggregate([{"verdict": gate}], True)
        self.assertFalse(clean)

    def test_status_asymmetry_is_http_error(self):
        gate, reasons = vd.http_gate(200, 500, None)
        self.assertEqual(gate, vd.VERDICT_HTTP_ERROR)
        gate, _ = vd.http_gate(None, 200, None)  # blocked response object
        self.assertEqual(gate, vd.VERDICT_HTTP_ERROR)

    def test_200_both_passes_gate(self):
        gate, _ = vd.http_gate(200, 200, None)
        self.assertIsNone(gate)


class AggregateTests(unittest.TestCase):
    """Regression: aggregate([], True) used to return (True, {}) — an empty
    run was a success. It is now a FAILURE."""

    def test_empty_rows_fail_even_with_inverse_ok(self):
        clean, counts = vd.aggregate([], True)
        self.assertFalse(clean)
        self.assertEqual(counts, {})

    def test_no_verdict_and_errors_fail(self):
        for verdict in (vd.VERDICT_ERROR, vd.VERDICT_NO_VERDICT,
                        vd.VERDICT_HTTP_ERROR, vd.VERDICT_EXPECTED_HTTP):
            clean, _ = vd.aggregate([{"verdict": verdict}], True)
            self.assertFalse(clean, verdict)

    def test_results_are_not_global_failures(self):
        rows = [{"verdict": v} for v in
                (vd.VERDICT_DIFF, vd.VERDICT_DIM_GAP, vd.VERDICT_IDENTICAL,
                 vd.VERDICT_KNOWN_DELTA)]
        clean, _ = vd.aggregate(rows, True)
        self.assertTrue(clean)

    def test_inverse_failure_fails_everything(self):
        clean, _ = vd.aggregate([{"verdict": vd.VERDICT_IDENTICAL}], False)
        self.assertFalse(clean)


class InverseControlTests(unittest.TestCase):
    def test_healthy_engine_passes(self):
        with tempfile.TemporaryDirectory() as tmp:
            ok, val, detail = vd.inverse_control(tmp)
            self.assertTrue(ok, detail)
            self.assertGreaterEqual(val, vd.INVERSE_CONTROL_FLOOR_PCT)

    def test_expected_magnitude(self):
        with tempfile.TemporaryDirectory() as tmp:
            _, val, _ = vd.inverse_control(tmp)
            self.assertAlmostEqual(val, 18.75, delta=1.5)


class GuardsAndContainmentTests(unittest.TestCase):
    def test_memory_limited_route_never_compared(self):
        with tempfile.TemporaryDirectory() as tmp:
            entries = [make_entry("x",
                                  {"http_status": 200, "memory_limit": False,
                                   "screenshot": None},
                                  {"http_status": 200, "memory_limit": True,
                                   "screenshot": None},
                                  errors=["preprod: memory limit"])]
            rows = vd.build_report_rows(entries, tmp)
            self.assertEqual(rows[0]["verdict"], vd.VERDICT_NO_VERDICT)
            self.assertIsNone(rows[0]["metrics"])

    def test_io_failure_contained_per_route(self):
        with tempfile.TemporaryDirectory() as tmp:
            grad = Image.new("RGB", (64, 64))
            grad.putdata([(x * 4 % 256, y * 4 % 256, 7) for y in range(64) for x in range(64)])
            grad.save(os.path.join(tmp, "a.png"))
            grad.save(os.path.join(tmp, "b.png"))
            entries = [
                # route 1: files missing on disk -> ERROR (missing capture),
                # must not stop route 2
                make_entry("bad",
                           {"http_status": 200, "memory_limit": False,
                            "screenshot": "ghost.png"},
                           {"http_status": 200, "memory_limit": False,
                            "screenshot": "ghost2.png"}),
                # route 2: healthy -> compared normally
                make_entry("good",
                           {"http_status": 200, "memory_limit": False,
                            "screenshot": "a.png"},
                           {"http_status": 200, "memory_limit": False,
                            "screenshot": "b.png"}),
                # route 3: corrupt PNG that EXISTS -> ERROR (suspicious/
                # undecodable), must not stop anything either
                make_entry("corrupt",
                           {"http_status": 200, "memory_limit": False,
                            "screenshot": "c.png"},
                           {"http_status": 200, "memory_limit": False,
                            "screenshot": "c.png"})]
            with open(os.path.join(tmp, "c.png"), "wb") as fh:
                fh.write(b"\x89PNG\r\n\x1a\nbroken")
            rows = vd.build_report_rows(entries, tmp)
            self.assertEqual(rows[0]["verdict"], vd.VERDICT_ERROR)
            self.assertTrue(any("missing" in e for e in rows[0]["errors"]))
            self.assertEqual(rows[1]["verdict"], vd.VERDICT_IDENTICAL)
            # corrupt PNG: caught either by the header revalidation
            # (COMPARISON ERROR) or by the suspicious screen — ERROR either way
            self.assertEqual(rows[2]["verdict"], vd.VERDICT_ERROR)
            self.assertTrue(any(("COMPARISON ERROR" in e) or ("SUSPICIOUS" in e)
                                for e in rows[2]["errors"]))

    def test_structured_ab_verdict_gates_before_compute_diff(self):
        # round 3: an ab_verdict state (set by run_capture on an unusable or
        # nondeterministic same-host A/B) must make build_report_rows return
        # NO-VERDICT BEFORE the prod/preprod compute_diff
        with tempfile.TemporaryDirectory() as tmp:
            grad = Image.new("RGB", (64, 64))
            grad.putdata([(x * 4 % 256, y * 4 % 256, 7)
                          for y in range(64) for x in range(64)])
            grad.save(os.path.join(tmp, "a.png"))
            grad.save(os.path.join(tmp, "b.png"))
            entry = make_entry(
                "x",
                {"http_status": 200, "memory_limit": False, "screenshot": "a.png"},
                {"http_status": 200, "memory_limit": False, "screenshot": "b.png"})
            vd.flag_ab_no_verdict(entry, "prod", ["second capture unusable"])
            rows = vd.build_report_rows([entry], tmp)
            self.assertEqual(rows[0]["verdict"], vd.VERDICT_NO_VERDICT)
            self.assertIsNone(rows[0]["metrics"])   # compute_diff never ran
            self.assertFalse(os.path.exists(os.path.join(tmp, "x_sbs.png")))
            self.assertFalse(os.path.exists(os.path.join(tmp, "x_heat.png")))
            self.assertTrue(any("same-host A/B" in r for r in rows[0]["reasons"]))

    def test_nondeterminism_accumulates_both_hosts(self):
        # superseded in-depth by NondeterminismTraceTests (basename/out_dir);
        # kept here for the accumulation-only property
        with tempfile.TemporaryDirectory() as out:
            grad = Image.new("RGB", (64, 64), (1, 2, 3))
            grad.save(os.path.join(out, "x.png"))
            entry = {}
            m = vd.DiffMetrics(64, 64, 64, 64, True, 1.0, 2.0, 5)
            for host in ("prod", "preprod"):
                vd.record_nondeterminism(entry, host, m,
                                         {"screenshot": "x.png", "http_status": 200,
                                          "captured_at": "2026-09-08T20:00:00+02:00",
                                          "page_dims": [64, 64]}, out)
            self.assertIn("prod", entry["nondeterminism"])
            self.assertIn("preprod", entry["nondeterminism"])  # not overwritten
            second = entry["nondeterminism"]["prod"]["second_capture"]
            self.assertEqual(second["http_status"], 200)
            self.assertEqual(len(second["sha256"]), 64)
            self.assertEqual(second["dims"], [64, 64])


class SecondCaptureTests(unittest.TestCase):
    """Regression round 2: a second capture with memory_limit or screenshot
    None used to be silently IGNORED (the A/B comparison was just skipped);
    it must make the route NO-VERDICT. HTTP status, header memory guard and
    the suspicious-capture screen are all enforced BEFORE compute_diff."""

    def _gradient(self, tmp, name):
        grad = Image.new("RGB", (64, 64))
        grad.putdata([(x * 4 % 256, y * 4 % 256, 7) for y in range(64) for x in range(64)])
        p = os.path.join(tmp, name)
        grad.save(p)
        return name

    def test_memory_limited_second_capture_rejected(self):
        usable, reasons = vd.assess_second_capture(
            "/anywhere", {"memory_limit": True, "screenshot": None,
                          "http_status": 200}, 40.0)
        self.assertFalse(usable)
        self.assertTrue(any("unusable" in r for r in reasons))

    def test_none_screenshot_second_capture_rejected(self):
        usable, reasons = vd.assess_second_capture(
            "/anywhere", {"memory_limit": False, "screenshot": None,
                          "http_status": 200}, 40.0)
        self.assertFalse(usable)

    def test_http_error_second_capture_rejected(self):
        with tempfile.TemporaryDirectory() as tmp:
            name = self._gradient(tmp, "b.png")
            usable, reasons = vd.assess_second_capture(
                tmp, {"memory_limit": False, "screenshot": name,
                      "http_status": 500}, 40.0)
            self.assertFalse(usable)
            self.assertTrue(any("HTTP 500" in r for r in reasons))

    def test_header_guard_on_second_capture(self):
        with tempfile.TemporaryDirectory() as tmp:
            big = os.path.join(tmp, "big.png")
            Image.new("RGB", (4200, 4200), (1, 2, 3)).save(big)
            usable, reasons = vd.assess_second_capture(
                tmp, {"memory_limit": False, "screenshot": "big.png",
                      "http_status": 200}, 10.0)
            self.assertFalse(usable)
            self.assertTrue(any("guard" in r for r in reasons))

    def test_suspicious_second_capture_rejected(self):
        with tempfile.TemporaryDirectory() as tmp:
            uni = os.path.join(tmp, "uni.png")
            Image.new("RGB", (300, 300), (240, 240, 240)).save(uni)
            usable, reasons = vd.assess_second_capture(
                tmp, {"memory_limit": False, "screenshot": "uni.png",
                      "http_status": 200}, 40.0)
            self.assertFalse(usable)
            self.assertTrue(any("suspicious" in r for r in reasons))

    def test_missing_second_capture_file_rejected(self):
        usable, reasons = vd.assess_second_capture(
            tempfile.gettempdir(), {"memory_limit": False, "screenshot": "nope-x.png",
                                    "http_status": 200}, 40.0)
        self.assertFalse(usable)
        self.assertTrue(any("missing" in r for r in reasons))

    def test_healthy_second_capture_usable(self):
        with tempfile.TemporaryDirectory() as tmp:
            name = self._gradient(tmp, "ok.png")
            usable, reasons = vd.assess_second_capture(
                tmp, {"memory_limit": False, "screenshot": name,
                      "http_status": 200}, 40.0)
            self.assertTrue(usable, str(reasons))


class NondeterminismTraceTests(unittest.TestCase):
    """Regression round 2: the second capture's fingerprint used to be
    resolved against the process CWD instead of out_dir. The trace must
    resolve the BASENAME against out_dir and record the real dims + sha."""

    def test_basename_resolved_against_out_dir_not_cwd(self):
        with tempfile.TemporaryDirectory() as out, tempfile.TemporaryDirectory() as elsewhere:
            grad = Image.new("RGB", (64, 64))
            grad.putdata([(x * 4 % 256, y * 4 % 256, 7) for y in range(64) for x in range(64)])
            name = "home_prod_b.png"
            grad.save(os.path.join(out, name))
            entry = {}
            m = vd.DiffMetrics(64, 64, 64, 64, True, 1.0, 2.0, 5)
            cwd = os.getcwd()
            os.chdir(elsewhere)  # prove no CWD dependence
            try:
                vd.record_nondeterminism(
                    entry, "prod", m,
                    {"screenshot": name, "http_status": 200,
                     "captured_at": "2026-09-08T23:30:00+02:00",
                     "page_dims": [64, 64]}, out)
            finally:
                os.chdir(cwd)
            trace = entry["nondeterminism"]["prod"]["second_capture"]
            self.assertEqual(trace["screenshot"], name)
            self.assertEqual(trace["dims"], [64, 64])  # probed from out_dir
            self.assertEqual(trace["sha256"],
                             vd.sha256_file(os.path.join(out, name)))

    def test_both_hosts_accumulate(self):
        with tempfile.TemporaryDirectory() as out:
            grad = Image.new("RGB", (64, 64), (1, 2, 3))
            grad.save(os.path.join(out, "x.png"))
            entry = {}
            m = vd.DiffMetrics(64, 64, 64, 64, True, 1.0, 2.0, 5)
            for host in ("prod", "preprod"):
                vd.record_nondeterminism(entry, host, m,
                                         {"screenshot": "x.png", "http_status": 200,
                                          "captured_at": "t", "page_dims": [64, 64]}, out)
            self.assertIn("prod", entry["nondeterminism"])
            self.assertIn("preprod", entry["nondeterminism"])

    def test_dims_mismatch_same_host_invalidates_stability(self):
        # round 3: 64x64 vs 64x128 with an IDENTICAL common area used to
        # read as "deterministic" (pct_strong == 0) — a stability claim on
        # the common area alone is invalid; dims_match must flip the verdict
        with tempfile.TemporaryDirectory() as out:
            g = Image.new("RGB", (64, 64))
            g.putdata([(x * 4 % 256, y * 4 % 256, 7)
                       for y in range(64) for x in range(64)])
            g.save(os.path.join(out, "a.png"))
            tall = Image.new("RGB", (64, 128), (1, 2, 3))
            tall.paste(g, (0, 0))
            tall.save(os.path.join(out, "b.png"))
            m = vd.compute_diff(os.path.join(out, "a.png"),
                                os.path.join(out, "b.png"))
            self.assertFalse(m.dims_match)
            self.assertEqual(m.pct_strong, 0.0)   # common area IS identical...
            entry = {}
            nondet = vd.record_nondeterminism(
                entry, "prod", m,
                {"screenshot": "b.png", "http_status": 200,
                 "captured_at": "t", "page_dims": [64, 128]}, out)
            self.assertTrue(nondet)               # ...yet NOT deterministic
            trace = entry["nondeterminism"]["prod"]
            self.assertTrue(trace["nondeterministic"])
            self.assertFalse(trace["dims_match"])


class HeaderRevalidationTests(unittest.TestCase):
    """Regression round 2: report mode used to trust the manifest
    memory_limit flag alone. Dimensions are REVALIDATED from the file
    headers before any decode — a tampered/stale manifest cannot smuggle an
    oversized PNG into compute_diff."""

    def test_oversized_png_without_flag_yields_no_verdict(self):
        with tempfile.TemporaryDirectory() as tmp:
            big = os.path.join(tmp, "x_prod.png")
            Image.new("RGB", (4200, 4200), (10, 20, 30)).save(big)
            entries = [make_entry("x",
                                  {"http_status": 200, "memory_limit": False,
                                   "screenshot": "x_prod.png"},
                                  {"http_status": 200, "memory_limit": False,
                                   "screenshot": "x_prod.png"})]
            rows = vd.build_report_rows(entries, tmp, max_mp=10.0)
            self.assertEqual(rows[0]["verdict"], vd.VERDICT_NO_VERDICT)
            self.assertIsNone(rows[0]["metrics"])  # never decoded/compared
            self.assertTrue(any("header revalidation" in r for r in rows[0]["reasons"]))


class FingerprintCompletenessTests(unittest.TestCase):
    """Regression round 2: stored_fingerprints {} (or a missing per-capture
    entry) used to be silently accepted. Every capture WITH a screenshot must
    have a stored fingerprint — including same-host A/B second captures."""

    def _mk(self, tmp, with_png=True):
        grad = Image.new("RGB", (64, 64))
        grad.putdata([(x * 4 % 256, y * 4 % 256, 7) for y in range(64) for x in range(64)])
        names = {}
        if with_png:
            for n in ("a.png", "b.png"):
                grad.save(os.path.join(tmp, n))
                names[n] = vd.sha256_file(os.path.join(tmp, n))
        entries = [make_entry("home",
                              {"http_status": 200, "memory_limit": False,
                               "screenshot": ("a.png" if with_png else None)},
                              {"http_status": 200, "memory_limit": False,
                               "screenshot": ("b.png" if with_png else None)})]
        return entries, names

    def test_empty_stored_fingerprints_is_explicit_failure(self):
        with tempfile.TemporaryDirectory() as tmp:
            entries, _ = self._mk(tmp)
            problems = vd.verify_fingerprints(entries, tmp, {})
            self.assertEqual(len(problems), 2)  # one per host
            self.assertTrue(all("NO stored fingerprint" in p[1] for p in problems))

    def test_healthy_fingerprints_no_problems(self):
        with tempfile.TemporaryDirectory() as tmp:
            entries, names = self._mk(tmp)
            stored = {"home": {"prod": names["a.png"], "preprod": names["b.png"]}}
            self.assertEqual(vd.verify_fingerprints(entries, tmp, stored), [])

    def test_mismatch_reported(self):
        with tempfile.TemporaryDirectory() as tmp:
            entries, names = self._mk(tmp)
            stored = {"home": {"prod": "0" * 64, "preprod": names["b.png"]}}
            problems = vd.verify_fingerprints(entries, tmp, stored)
            self.assertEqual(len(problems), 1)
            self.assertIn("MISMATCH", problems[0][1])

    def test_fingerprint_without_file_reported(self):
        with tempfile.TemporaryDirectory() as tmp:
            entries, names = self._mk(tmp)
            # delete the preprod capture but keep its fingerprint
            os.unlink(os.path.join(tmp, "b.png"))
            stored = {"home": {"prod": names["a.png"], "preprod": names["b.png"]}}
            problems = vd.verify_fingerprints(entries, tmp, stored)
            self.assertTrue(any("missing on disk" in p[1] for p in problems))

    def test_guarded_capture_without_fingerprint_is_consistent(self):
        with tempfile.TemporaryDirectory() as tmp:
            entries, _ = self._mk(tmp, with_png=False)
            entries[0]["hosts"]["preprod"]["memory_limit"] = True
            stored = {"home": {"prod": "0" * 64}}  # preprod guarded: no fp needed
            problems = vd.verify_fingerprints(entries, tmp, stored)
            # prod has no file and no fp and no guard -> inconsistent; preprod guarded -> ok
            self.assertEqual(len(problems), 1)

    def test_second_capture_without_fingerprint_reported(self):
        with tempfile.TemporaryDirectory() as tmp:
            entries, names = self._mk(tmp)
            grad = Image.new("RGB", (64, 64), (9, 9, 9))
            grad.save(os.path.join(tmp, "home_prod_b.png"))
            entries[0]["nondeterminism"] = {"prod": {
                "pct_strong_same_host": 0.0, "nondeterministic": False,
                "second_capture": {"screenshot": "home_prod_b.png",
                                   "dims": [64, 64], "sha256": None,
                                   "http_status": 200, "captured_at": "t"}}}
            stored = {"home": {"prod": names["a.png"], "preprod": names["b.png"]}}
            problems = vd.verify_fingerprints(entries, tmp, stored)
            self.assertTrue(any("second capture" in p[1] and "WITHOUT recorded"
                                in p[1] for p in problems))

    def test_second_capture_fingerprint_mismatch_reported(self):
        with tempfile.TemporaryDirectory() as tmp:
            entries, names = self._mk(tmp)
            grad = Image.new("RGB", (64, 64), (9, 9, 9))
            grad.save(os.path.join(tmp, "home_prod_b.png"))
            entries[0]["nondeterminism"] = {"prod": {
                "pct_strong_same_host": 0.0, "nondeterministic": False,
                "second_capture": {"screenshot": "home_prod_b.png",
                                   "dims": [64, 64], "sha256": "0" * 64,
                                   "http_status": 200, "captured_at": "t"}}}
            stored = {"home": {"prod": names["a.png"], "preprod": names["b.png"]}}
            problems = vd.verify_fingerprints(entries, tmp, stored)
            self.assertTrue(any("second-capture FINGERPRINT MISMATCH" in p[1]
                                for p in problems))

    def test_healthy_second_capture_fingerprint_ok(self):
        with tempfile.TemporaryDirectory() as tmp:
            entries, names = self._mk(tmp)
            grad = Image.new("RGB", (64, 64), (9, 9, 9))
            p2 = os.path.join(tmp, "home_prod_b.png")
            grad.save(p2)
            entries[0]["nondeterminism"] = {"prod": {
                "pct_strong_same_host": 0.0, "nondeterministic": False,
                "second_capture": {"screenshot": "home_prod_b.png",
                                   "dims": [64, 64], "sha256": vd.sha256_file(p2),
                                   "http_status": 200, "captured_at": "t"}}}
            stored = {"home": {"prod": names["a.png"], "preprod": names["b.png"]}}
            self.assertEqual(vd.verify_fingerprints(entries, tmp, stored), [])



    """Offline report rebuild from a manifest: fingerprints re-verified, raw
    PNG links present, mismatch flagged per route."""

class ReportModeTests(unittest.TestCase):
    """Offline report rebuild from a manifest: fingerprints re-verified
    (completeness enforced — see FingerprintCompletenessTests), raw PNG
    links present, mismatch flagged per route."""

    def _write_manifest_run(self, tmp, tamper=False):
        # two identical captures of a textured image
        grad = Image.new("RGB", (64, 64))
        grad.putdata([(x * 4 % 256, y * 4 % 256, 7) for y in range(64) for x in range(64)])
        pa = os.path.join(tmp, "home_prod.png")
        pb = os.path.join(tmp, "home_preprod.png")
        grad.save(pa); grad.save(pb)
        entries = [make_entry("home",
                              {"http_status": 200, "memory_limit": False,
                               "screenshot": "home_prod.png",
                               "final_url": "https://p/", "markers": {},
                               "freshness": {"state": "unknown", "details": []}},
                              {"http_status": 200, "memory_limit": False,
                               "screenshot": "home_preprod.png",
                               "final_url": "https://q/", "markers": {},
                               "freshness": {"state": "unknown", "details": []}})]
        fps = {"home": {"prod": vd.sha256_file(pa), "preprod": vd.sha256_file(pb)}}
        if tamper:  # pretend the capture hashed differently
            fps["home"]["preprod"] = "0" * 64
        manifest = {"meta": {"generated_at": vd.now_iso(), "viewport": "1440x900",
                             "max_mp": 40, "source": {"tool": "x", "git_rev": "y"}},
                    "entries": entries, "stored_fingerprints": fps}
        with open(os.path.join(tmp, "manifest.json"), "w", encoding="utf-8") as fh:
            json.dump(manifest, fh)

    def test_report_mode_rebuilds_offline(self):
        with tempfile.TemporaryDirectory() as tmp:
            self._write_manifest_run(tmp)
            rc = vd.main(["--mode", "report", "--out", tmp])
            self.assertEqual(rc, 0)  # identical textured pair, fingerprints OK
            with open(os.path.join(tmp, "manifest.json"), encoding="utf-8") as fh:
                man = json.load(fh)
            self.assertEqual(man["rows"][0]["verdict"], vd.VERDICT_IDENTICAL)
            self.assertIn("inverse_control", man)
            with open(os.path.join(tmp, "report.html"), encoding="utf-8") as fh:
                html = fh.read()
            self.assertIn("home_prod.png", html)   # raw PNG link
            self.assertIn("home_preprod.png", html)

    def test_report_mode_flags_fingerprint_mismatch(self):
        with tempfile.TemporaryDirectory() as tmp:
            self._write_manifest_run(tmp, tamper=True)
            rc = vd.main(["--mode", "report", "--out", tmp])
            self.assertEqual(rc, 1)  # global failure
            with open(os.path.join(tmp, "manifest.json"), encoding="utf-8") as fh:
                man = json.load(fh)
            self.assertEqual(man["rows"][0]["verdict"], vd.VERDICT_ERROR)
            self.assertTrue(any("FINGERPRINT" in e for e in man["rows"][0]["errors"]))


class ParamAndPreservationTests(unittest.TestCase):
    def _args(self, **over):
        base = {"mode": "capture", "routes": "/nonexistent.json", "out": "x",
                "only": None, "viewport": "1440x900", "timeout": 60,
                "settle": 800, "max_mp": 40.0, "double_capture": False,
                "self_test": False}
        base.update(over)
        return argparse.Namespace(**base)

    def test_bad_viewport_rejected(self):
        for vp in ("0x900", "1440x0", "1440", "-10x100", "axb"):
            with self.assertRaises(vd.VisualDiffError, msg=vp):
                vd.validate_params(self._args(viewport=vp))

    def test_bad_limits_rejected(self):
        with self.assertRaises(vd.VisualDiffError):
            vd.validate_params(self._args(max_mp=0))
        with self.assertRaises(vd.VisualDiffError):
            vd.validate_params(self._args(max_mp=-1))
        with self.assertRaises(vd.VisualDiffError):
            vd.validate_params(self._args(timeout=0))
        with self.assertRaises(vd.VisualDiffError):
            vd.validate_params(self._args(settle=-1))
        self.assertEqual(vd.validate_params(self._args(viewport="1280x720")), (1280, 720))

    def test_previous_run_manifest_never_overwritten(self):
        with tempfile.TemporaryDirectory() as tmp:
            with open(os.path.join(tmp, "manifest.json"), "w") as fh:
                fh.write("{}")
            rc = vd.main(["--mode", "capture", "--out", tmp,
                          "--routes", "/nonexistent.json"])
            self.assertEqual(rc, 1)  # explicit failure, previous run preserved
            with open(os.path.join(tmp, "manifest.json"), encoding="utf-8") as fh:
                self.assertEqual(fh.read(), "{}")


class ArtifactsTests(unittest.TestCase):
    def test_heatmap_and_side_by_side_written(self):
        with tempfile.TemporaryDirectory() as tmp:
            a = Image.new("RGB", (64, 64), (255, 255, 255))
            b = Image.new("RGB", (64, 64), (255, 255, 255))
            ImageDraw.Draw(b).rectangle([4, 4, 10, 10], fill=(0, 0, 0))
            pa, pb = os.path.join(tmp, "a.png"), os.path.join(tmp, "b.png")
            a.save(pa); b.save(pb)
            hm = vd.render_heatmap(pa, pb, os.path.join(tmp, "h.png"))
            sbs = vd.render_side_by_side(pa, pb, "PROD label", "PREPROD label",
                                         os.path.join(tmp, "sbs.png"))
            self.assertGreater(os.path.getsize(hm), 0)
            self.assertGreater(os.path.getsize(sbs), 0)


class ReportTests(unittest.TestCase):
    def _row(self, **over):
        row = {"name": "home", "verdict": vd.VERDICT_DIFF, "errors": [],
               "reasons": [], "note": "", "metrics": None, "fingerprints": {},
               "artifacts": {}, "sidecars": {}, "nondeterminism": {}}
        row.update(over)
        return row

    def test_html_escaping_hostile_strings(self):
        hostile = "<script>alert('x')</script> & \"q\" 'quote'"
        rows = [self._row(name=hostile, errors=[hostile], reasons=[hostile], note=hostile)]
        with tempfile.TemporaryDirectory() as tmp:
            rp = vd.render_html_report(rows, {"viewport": "1440x900", "max_mp": 40},
                                       {"ok": True, "detail": hostile},
                                       os.path.join(tmp, "r.html"))
            with open(rp, encoding="utf-8") as fh:
                content = fh.read()
        self.assertNotIn("<script>alert", content)
        self.assertIn("&lt;script&gt;", content)
        self.assertIn("&quot;q&quot;", content)
        self.assertIn("&#x27;quote&#x27;", content)

    def test_aesthetic_banner_always_present(self):
        with tempfile.TemporaryDirectory() as tmp:
            rp = vd.render_html_report([self._row()], {"viewport": "1x1"},
                                       {"ok": True, "detail": "d"},
                                       os.path.join(tmp, "r.html"))
            with open(rp, encoding="utf-8") as fh:
                content = fh.read()
        self.assertIn(vd.AESTHETIC_BANNER, content)

    def test_no_aesthetic_pass_verdict_vocabulary(self):
        allowed = {vd.VERDICT_IDENTICAL, vd.VERDICT_KNOWN_DELTA, vd.VERDICT_DIFF,
                   vd.VERDICT_DIM_GAP, vd.VERDICT_ERROR, vd.VERDICT_NO_VERDICT,
                   vd.VERDICT_HTTP_ERROR, vd.VERDICT_EXPECTED_HTTP}
        for token in ("ERGO", "ESTH", "AESTH", "PASS-HUMAIN", "BEAU"):
            for v in allowed:
                self.assertNotIn(token, v)

    def test_sidecar_values_escaped(self):
        sc = {"prod": {"http_status": 200, "final_url": "https://x/?a=<b>",
                       "buster_outcome": "kept-query",
                       "freshness": {"state": "unknown", "details": ["a<b"]},
                       "markers": {"tosic_sxc": 3, "tosic_sexycontent": 0,
                                   "cdv": "325"},
                       "console_error_count": 0, "captured_at": "t"}}
        rows = [self._row(sidecars=sc)]
        with tempfile.TemporaryDirectory() as tmp:
            rp = vd.render_html_report(rows, {"viewport": "1x1"},
                                       {"ok": True, "detail": "d"},
                                       os.path.join(tmp, "r.html"))
            with open(rp, encoding="utf-8") as fh:
                content = fh.read()
        self.assertNotIn("?a=<b>", content)
        self.assertIn("&lt;b&gt;", content)


class SelfTestModeTests(unittest.TestCase):
    def test_cli_self_test_exits_zero(self):
        self.assertEqual(vd.main(["--self-test"]), 0)


# ---------------------------------------------------------------------------
# Round 3: MOCKED integration tests. These drive run_capture() and the
# run()/report CLI path END-TO-END with a stubbed playwright module and a
# scripted _capture_one — no network, no browser — proving the INTEGRATED
# paths, not only the isolated helper functions.
# ---------------------------------------------------------------------------

class _StubBrowser:
    def new_context(self, **kw):
        return self

    def close(self):
        pass


def _stub_playwright_overlay():
    """sys.modules overlay making 'from playwright.sync_api import
    sync_playwright' resolve to a stub (works whether or not the real
    playwright package is installed)."""
    pw = types.ModuleType("playwright")
    sync_api = types.ModuleType("playwright.sync_api")

    class _Chromium:
        @staticmethod
        def launch(**kw):
            return _StubBrowser()

    class _Ctx:
        def __enter__(self):
            return types.SimpleNamespace(chromium=_Chromium())

        def __exit__(self, *a):
            return False

    sync_api.sync_playwright = _Ctx
    pw.sync_api = sync_api
    return {"playwright": pw, "playwright.sync_api": sync_api}


def _fake_capture_one(script):
    """Scripted _capture_one replacement. Each call consumes one step:
    {"image": PIL.Image} or {"raw": bytes} is written to out_png
    (screenshot = basename(out_png)); {"memory_limit": True, "page_dims":
    [...]} simulates a browser-side guard trip (screenshot None);
    http_status / markers / final_url override the defaults."""
    state = {"i": 0}

    def fake(context, url, out_png, settle_ms, timeout_ms, max_mp):
        step = script[state["i"]] if state["i"] < len(script) else {}
        state["i"] += 1
        wrote = False
        if step.get("raw") is not None:
            with open(out_png, "wb") as fh:
                fh.write(step["raw"])
            wrote = True
        elif step.get("image") is not None:
            step["image"].save(out_png)
            wrote = True
        return {"requested_url": url,
                "final_url": step.get("final_url", url),
                "http_status": step.get("http_status", 200),
                "captured_at": "2026-09-08T12:00:00+02:00",
                "console_error_count": 0, "console_errors_first": [],
                "markers": step.get("markers", {}),
                "screenshot": (os.path.basename(out_png) if wrote else None),
                "memory_limit": (not wrote) and bool(step.get("memory_limit")),
                "page_dims": step.get("page_dims")}
    return fake


def _grad64():
    g = Image.new("RGB", (64, 64))
    g.putdata([(x * 4 % 256, y * 4 % 256, 7)
               for y in range(64) for x in range(64)])
    return g


class RunCaptureIntegrationTests(unittest.TestCase):
    """Mocked end-to-end drives of run_capture (round 3: structured
    NO-VERDICT for an unusable A/B second capture, dims-instability
    nondeterminism, per-route containment of a corrupt first capture)."""

    HOSTS = {"prod": "https://prod.example", "preprod": "https://pre.example"}

    def _run(self, out, pairs, script, double):
        with mock.patch.dict(sys.modules, _stub_playwright_overlay()), \
                mock.patch.object(vd, "_capture_one", _fake_capture_one(script)):
            return vd.run_capture(self.HOSTS, pairs, out, None,
                                  (1440, 900), 30000, 0, 40.0, double)

    def test_unusable_second_capture_structured_no_verdict(self):
        # prod's A/B second capture is memory-limited: the failure must be
        # a STRUCTURED ab_verdict honored by build_report_rows BEFORE the
        # prod/preprod compute_diff (no metrics, no diff artifacts)
        with tempfile.TemporaryDirectory() as out:
            g = _grad64()
            entries = self._run(
                out, [vd.RoutePair(name="home", prod_path="/")],
                [{"image": g},
                 {"memory_limit": True, "page_dims": [9000, 9000]},
                 {"image": g}, {"image": g}],
                double=True)
            entry = entries[0]
            self.assertIn("prod", entry["ab_verdict"])
            self.assertNotIn("preprod", entry["ab_verdict"])
            self.assertIn("preprod", entry["nondeterminism"])
            self.assertNotIn("prod", entry["nondeterminism"])
            rows = vd.build_report_rows(entries, out)
            self.assertEqual(rows[0]["verdict"], vd.VERDICT_NO_VERDICT)
            self.assertIsNone(rows[0]["metrics"])  # compute_diff never ran
            self.assertFalse(os.path.exists(os.path.join(out, "home_sbs.png")))
            self.assertFalse(os.path.exists(os.path.join(out, "home_heat.png")))
            self.assertTrue(any("same-host A/B" in r for r in rows[0]["reasons"]))

    def test_same_host_dims_mismatch_is_nondeterministic(self):
        # 64x64 vs 64x128 with an IDENTICAL common area: pct_strong == 0
        # but dims_match False — nondeterministic, route NO-VERDICT before
        # the prod/preprod comparison
        with tempfile.TemporaryDirectory() as out:
            g = _grad64()
            tall = Image.new("RGB", (64, 128), (1, 2, 3))
            tall.paste(g, (0, 0))
            entries = self._run(
                out, [vd.RoutePair(name="home", prod_path="/")],
                [{"image": g}, {"image": tall}, {"image": g}, {"image": g}],
                double=True)
            entry = entries[0]
            trace = entry["nondeterminism"]["prod"]
            self.assertFalse(trace["dims_match"])
            self.assertTrue(trace["nondeterministic"])
            self.assertTrue(any("dimension instability" in r
                                for r in entry["ab_verdict"]["prod"]))
            rows = vd.build_report_rows(entries, out)
            self.assertEqual(rows[0]["verdict"], vd.VERDICT_NO_VERDICT)
            self.assertIsNone(rows[0]["metrics"])

    def test_corrupt_first_capture_contained_per_route(self):
        # a corrupt PNG header on the FIRST capture (memory_guard raises
        # CorruptImageError) must not kill the run: the route is errored and
        # the FOLLOWING routes are still captured
        with tempfile.TemporaryDirectory() as out:
            g = _grad64()
            entries = self._run(
                out, [vd.RoutePair(name="bad", prod_path="/"),
                      vd.RoutePair(name="good", prod_path="/x")],
                [{"raw": b"\x89PNG\r\n\x1a\nbroken"},
                 {"image": g}, {"image": g}, {"image": g}],
                double=False)
            self.assertEqual(len(entries), 2)
            bad = entries[0]
            self.assertIsNone(bad["hosts"]["prod"]["screenshot"])
            self.assertTrue(any("capture header unreadable" in e
                                for e in bad["errors"]))
            rows = vd.build_report_rows(entries, out)
            self.assertEqual(rows[0]["verdict"], vd.VERDICT_ERROR)
            self.assertEqual(rows[1]["verdict"], vd.VERDICT_IDENTICAL)


class RunReportIntegrationTests(unittest.TestCase):
    """Mocked end-to-end drives of the run()/CLI path (round 3: sha256
    PermissionError contained with the manifest still written; orphan
    _b.png without a trace fails report mode; the operator's stricter
    --max-mp wins over the manifest in report mode)."""

    def _routes_file(self, tmp, names):
        rp = os.path.join(tmp, "routes.json")
        pairs = [{"name": n, "prod_path": "/", "enabled": True} for n in names]
        with open(rp, "w", encoding="utf-8") as fh:
            json.dump({"hosts": {"prod": "https://prod.example",
                                 "preprod": "https://pre.example"},
                       "pairs": pairs}, fh)
        return rp

    def _fixture(self, tmp):
        """Healthy one-route manifest (identical textured pair, complete
        fingerprints) for report-mode tests."""
        grad = _grad64()
        pa = os.path.join(tmp, "home_prod.png")
        pb = os.path.join(tmp, "home_preprod.png")
        grad.save(pa)
        grad.save(pb)
        entries = [make_entry(
            "home",
            {"http_status": 200, "memory_limit": False,
             "screenshot": "home_prod.png", "final_url": "https://p/",
             "markers": {}},
            {"http_status": 200, "memory_limit": False,
             "screenshot": "home_preprod.png", "final_url": "https://q/",
             "markers": {}})]
        manifest = {"meta": {"generated_at": vd.now_iso(),
                             "viewport": "1440x900", "max_mp": 40,
                             "source": {"tool": "x", "git_rev": "y"}},
                    "entries": entries,
                    "stored_fingerprints": {
                        "home": {"prod": vd.sha256_file(pa),
                                 "preprod": vd.sha256_file(pb)}}}
        with open(os.path.join(tmp, "manifest.json"), "w", encoding="utf-8") as fh:
            json.dump(manifest, fh)

    def test_sha256_permission_error_contained_manifest_written(self):
        # the capture-time fingerprint collection hits a PermissionError on
        # ONE host: contained per route — the manifest is still written, the
        # missing fingerprint becomes an explicit failure, exit 1
        with tempfile.TemporaryDirectory() as out:
            rp = self._routes_file(out, ["home"])
            g = _grad64()
            real_sha = vd.sha256_file

            def sha_may_fail(path):
                if os.path.basename(path) == "home_prod.png":
                    raise PermissionError(13, "Permission denied", path)
                return real_sha(path)

            with mock.patch.dict(sys.modules, _stub_playwright_overlay()), \
                    mock.patch.object(vd, "_capture_one",
                                      _fake_capture_one([{"image": g},
                                                         {"image": g}])), \
                    mock.patch.object(vd, "sha256_file", sha_may_fail):
                rc = vd.main(["--routes", rp, "--out", out])
            self.assertEqual(rc, 1)
            self.assertTrue(os.path.exists(os.path.join(out, "manifest.json")))
            with open(os.path.join(out, "manifest.json"), encoding="utf-8") as fh:
                man = json.load(fh)
            self.assertNotIn("prod", man["stored_fingerprints"].get("home", {}))
            self.assertIn("fingerprint could not be collected",
                          " ".join(man["entries"][0]["errors"]))

    def test_orphan_second_capture_without_trace_fails_report(self):
        # an orphan home_prod_b.png on disk with nondeterminism == {} used
        # to be INVISIBLE (the disk scan only iterated nondeterminism
        # blocks); it must be an explicit report-mode failure
        with tempfile.TemporaryDirectory() as tmp:
            self._fixture(tmp)
            _grad64().save(os.path.join(tmp, "home_prod_b.png"))  # the orphan
            rc = vd.main(["--mode", "report", "--out", tmp])
            self.assertEqual(rc, 1)
            with open(os.path.join(tmp, "manifest.json"), encoding="utf-8") as fh:
                man = json.load(fh)
            self.assertEqual(man["rows"][0]["verdict"], vd.VERDICT_ERROR)
            self.assertTrue(any("WITHOUT any nondeterminism trace" in e
                                for e in man["rows"][0]["errors"]))

    def test_operator_stricter_max_mp_honored_in_report(self):
        # the operator's --max-mp, when stricter than the manifest's
        # capture-time guard, must WIN in report mode (header revalidation
        # trips the guard BEFORE any decode); with the default it does not
        with tempfile.TemporaryDirectory() as strict, \
                tempfile.TemporaryDirectory() as loose:
            self._fixture(strict)
            self._fixture(loose)
            rc_strict = vd.main(["--mode", "report", "--out", strict,
                                 "--max-mp", "0.000001"])
            self.assertEqual(rc_strict, 1)
            with open(os.path.join(strict, "manifest.json"),
                      encoding="utf-8") as fh:
                man = json.load(fh)
            self.assertEqual(man["rows"][0]["verdict"], vd.VERDICT_NO_VERDICT)
            self.assertTrue(any("header revalidation tripped the memory guard"
                                in r for r in man["rows"][0]["reasons"]))
            rc_loose = vd.main(["--mode", "report", "--out", loose])
            self.assertEqual(rc_loose, 0)
            with open(os.path.join(loose, "manifest.json"),
                      encoding="utf-8") as fh:
                man = json.load(fh)
            self.assertEqual(man["rows"][0]["verdict"], vd.VERDICT_IDENTICAL)


if __name__ == "__main__":
    unittest.main()
