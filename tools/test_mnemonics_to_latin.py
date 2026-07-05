#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contract tests for mnemonics_to_latin.py (#654, Option B keep-Latin) — stdlib only.

Run with plain `python`:

    python tools/test_mnemonics_to_latin.py
    python -m unittest tools.test_mnemonics_to_latin   # via repo root

What it pins (the contract #654 Option B depends on):
  1. extract_mnemonic_latin — the 19 canonical forms parse from title_fr; figure-rows give None.
  2. has_latin_mnemonic — Latin mnemonic detection independent of \b (the CJK-boundary case:
     "Festino三段论" MUST be detected, which \b...\b fails under re.UNICODE).
  3. extract_translit_token — structural-word stripping per lang (RU/AR/ZH/FA), with the
     ambiguity guards (empty residue, multi-token, Latin-leak).
  4. build_conversion_plan — on a synthetic CSV that mirrors the real structure, produces the
     expected plan and correctly classifies kept-Latin vs transliterated.
  5. apply_plan — modifies ONLY title_<lang> (remark_*/description_* untouched), preserves
     BOM + CRLF, and is idempotent (apply twice == apply once).
  6. GROUNDING on the real Virtues CSV — the plan is exactly 52 title cells
     (RU 14 / AR 16 / ZH 6 / FA 16) with 0 ambiguous, matching dispatch q9xpks. If the corpus
     drifts, this test surfaces it before a worker re-runs the script.

Exit code: 0 = all pass. The grounding test is skipped if the real CSV is absent (e.g. running
in a checkout without Cards/).
"""

import io
import os
import sys
import csv
import tempfile
import unittest

HERE = os.path.dirname(os.path.abspath(__file__))
if HERE not in sys.path:
    sys.path.insert(0, HERE)

import mnemonics_to_latin as M  # noqa: E402

REPO_ROOT = os.path.dirname(HERE)
REAL_CSV = os.path.join(REPO_ROOT, "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv")


# ─── 1. extract_mnemonic_latin ───────────────────────────────────────────────

class TestExtractMnemonicLatin(unittest.TestCase):
    def test_all_19_canonical_forms_parse(self):
        for m in M.MNEMONICS:
            self.assertEqual(M.extract_mnemonic_latin(f"Syllogisme {m}"), m, f"title for {m}")

    def test_pk_to_mnemonic_map_complete(self):
        # Every mnemonic pk must map to a canonical form, and vice-versa.
        self.assertEqual(len(M.MNEMONIC_PKS), 19)
        for pk in M.MNEMONIC_PKS:
            self.assertIn(pk, M._PK_TO_MNEMONIC)
            self.assertIn(M._PK_TO_MNEMONIC[pk], M.MNEMONICS)

    def test_figure_rows_return_none(self):
        # pks 110/115/122 are the figure-rows ("Syllogisme de Nème figure") — no mnemonic.
        self.assertIsNone(M.extract_mnemonic_latin("Syllogisme de deuxième figure"))
        self.assertIsNone(M.extract_mnemonic_latin("Syllogisme de 3e figure"))
        self.assertIsNone(M.extract_mnemonic_latin("Syllogisme de quatrième figure"))

    def test_empty_and_none(self):
        self.assertIsNone(M.extract_mnemonic_latin(""))
        self.assertIsNone(M.extract_mnemonic_latin(None))

    def test_case_sensitive(self):
        # Canonical forms are capitalised; lowercase must NOT match (avoids false positives).
        self.assertIsNone(M.extract_mnemonic_latin("syllogisme barbara minuscule"))


# ─── 2. has_latin_mnemonic (the \b vs CJK-boundary contract) ──────────────────

class TestHasLatinMnemonic(unittest.TestCase):
    def test_latin_with_space(self):
        self.assertTrue(M.has_latin_mnemonic("Силлогизм Camestres"))

    def test_cjk_glued_boundary(self):
        # THIS IS THE BUG THE FIX ADDRESSES: "Festino三段论" — \b fails (三 is \w under UNICODE),
        # so the Latin-letter lookaround must catch it. If this regresses, ZH cells get
        # spuriously flagged ambiguous and excluded from the plan.
        self.assertTrue(M.has_latin_mnemonic("Festino三段论"))
        self.assertTrue(M.has_latin_mnemonic("Bamalip式三段论"))

    def test_pure_transliterated(self):
        self.assertFalse(M.has_latin_mnemonic("Силлогизм Каместрес"))
        self.assertFalse(M.has_latin_mnemonic("قياس داريي"))
        self.assertFalse(M.has_latin_mnemonic("费斯蒂诺三段论"))

    def test_substring_rejected(self):
        # A mnemonic glued to another Latin letter is NOT a match (lookaround assertions on
        # [A-Za-z] only). "Barbaracide" / "preBarbara" must not be detected as the mnemonic.
        self.assertFalse(M._MNEMONIC_RE.search("Barbaracide"))
        self.assertFalse(M._MNEMONIC_RE.search("preBarbara"))


# ─── 3. extract_translit_token ───────────────────────────────────────────────

class TestExtractTranslitToken(unittest.TestCase):
    def test_ru(self):
        tok, amb = M.extract_translit_token("Силлогизм Каместрес", "ru")
        self.assertEqual(tok, "Каместрес")
        self.assertFalse(amb)

    def test_ar_with_suffix(self):
        # AR kept-Latin cells have " المنطقي" suffix; the transliterated ones may too.
        tok, amb = M.extract_translit_token("قياس كامستريس", "ar")
        self.assertEqual(tok, "كامستريس")
        self.assertFalse(amb)

    def test_ar_strip_qualifier(self):
        tok, amb = M.extract_translit_token("قياس كامستريس المنطقي", "ar")
        self.assertEqual(tok, "كامستريس")
        self.assertFalse(amb)

    def test_zh_plain(self):
        tok, amb = M.extract_translit_token("费斯蒂诺三段论", "zh")
        self.assertEqual(tok, "费斯蒂诺")
        self.assertFalse(amb)

    def test_zh_shi_form(self):
        tok, amb = M.extract_translit_token("弗雷西松式三段论", "zh")
        self.assertEqual(tok, "弗雷西松")
        self.assertFalse(amb)

    def test_fa(self):
        tok, amb = M.extract_translit_token("قیاس داریی", "fa")
        self.assertEqual(tok, "داریی")
        self.assertFalse(amb)

    def test_ambiguous_empty(self):
        tok, amb = M.extract_translit_token("", "ru")
        self.assertTrue(amb)

    def test_ambiguous_latin_leak(self):
        # If a structural word fails to strip, Latin letters remain -> ambiguous (unsafe).
        # Here we pass an AR cell but a made-up lang with no structural tokens known.
        tok, amb = M.extract_translit_token("Силлогизм Каместрес", "fa")
        # 'fa' has no 'Силлогизм' token -> residue still contains Cyrillic (not Latin) so the
        # Latin-leak guard is fine, but the residue has a space -> ambiguous via multi-token.
        self.assertTrue(amb)


# ─── 4. build_conversion_plan (synthetic CSV) ────────────────────────────────

def _row(pk, title_fr, title_ru="", title_ar="", title_zh="", title_fa="",
         remark_ru="", description_ru=""):
    """Build a CSV-shaped dict with the columns the planner reads."""
    return {
        "pk": str(pk), "title_fr": title_fr,
        "title_ru": title_ru, "title_ar": title_ar,
        "title_zh": title_zh, "title_fa": title_fa,
        "remark_ru": remark_ru, "description_ru": description_ru,
    }


class TestBuildConversionPlan(unittest.TestCase):
    def test_mixed_row_transliterated_and_kept(self):
        rows = [
            # pk 112: RU transliterated + AR transliterated + FA transliterated, EN kept (not in scope)
            _row(112, "Syllogisme Camestres",
                 title_ru="Силлогизм Каместрес",    # transliterated -> convert
                 title_ar="قياس كامستريس",            # transliterated -> convert
                 title_zh="Camestres 三段论",         # kept-Latin -> skip
                 title_fa="قیاس کامسترس",             # transliterated -> convert
                 remark_ru="see Каместрес in remark"),  # must NOT trigger a plan entry
        ]
        plan, ambig = M.build_conversion_plan(rows)
        langs = sorted(e["lang"] for e in plan)
        self.assertEqual(langs, ["ar", "fa", "ru"])  # zh skipped (kept-Latin)
        self.assertEqual(ambig, [])
        # proposed titles carry the Latin mnemonic
        by_lang = {e["lang"]: e for e in plan}
        self.assertEqual(by_lang["ru"]["proposed_title"], "Силлогизм Camestres")
        self.assertEqual(by_lang["ar"]["proposed_title"], "قياس Camestres")
        self.assertEqual(by_lang["fa"]["proposed_title"], "قیاس Camestres")
        # remark_ru is NOT in the plan (title-only scope)
        self.assertFalse(any("remark" in e["proposed_title"] for e in plan))

    def test_kept_latin_all_skip(self):
        rows = [_row(106, "Syllogisme Barbara",
                     title_ru="Силлогизм Barbara", title_ar="قياس Barbara",
                     title_zh="Barbara 三段论", title_fa="قیاس Barbara")]
        plan, ambig = M.build_conversion_plan(rows)
        self.assertEqual(plan, [])
        self.assertEqual(ambig, [])

    def test_figure_row_excluded(self):
        rows = [_row(110, "Syllogisme de deuxième figure",
                     title_ru="Силлогизм второй фигуры")]
        plan, ambig = M.build_conversion_plan(rows)
        self.assertEqual(plan, [])
        self.assertEqual(ambig, [])

    def test_zh_cjk_glued_not_flagged_ambiguous(self):
        # Regression: "Festino三段论" must convert cleanly (was spuriously ambiguous pre-fix).
        rows = [_row(113, "Syllogisme Festino", title_zh="费斯蒂诺三段论")]
        plan, ambig = M.build_conversion_plan(rows)
        self.assertEqual(len(plan), 1)
        self.assertEqual(plan[0]["proposed_title"], "Festino三段论")
        self.assertEqual(ambig, [])


# ─── 5. apply_plan (CSV write contract) ──────────────────────────────────────

class TestApplyPlan(unittest.TestCase):
    def _write_csv(self, path, rows_dict):
        fields = ["pk", "title_fr", "title_ru", "title_ar", "title_zh", "title_fa",
                  "remark_ru", "description_ru"]
        with open(path, "wb") as f:
            f.write(b"\xef\xbb\xbf")  # BOM like the real file
            buf = io.StringIO()
            w = csv.writer(buf, quoting=csv.QUOTE_MINIMAL, lineterminator="\r\n")
            w.writerow(fields)
            for r in rows_dict:
                w.writerow([r.get(k, "") for k in fields])
            f.write(buf.getvalue().encode("utf-8"))

    def _read_csv(self, path):
        with open(path, encoding="utf-8-sig", newline="") as f:
            return list(csv.DictReader(f))

    def test_apply_modifies_title_only(self):
        rows = [_row(112, "Syllogisme Camestres",
                     title_ru="Силлогизм Каместрес",
                     remark_ru="remark has Каместрес")]
        with tempfile.TemporaryDirectory() as d:
            p = os.path.join(d, "virtues.csv")
            self._write_csv(p, rows)
            plan, ambig = M.build_conversion_plan(self._read_csv(p))
            self.assertEqual(ambig, [])
            self.assertEqual(len(plan), 1)
            n = M.apply_plan(p, plan)
            self.assertEqual(n, 1)
            after = self._read_csv(p)
            self.assertEqual(after[0]["title_ru"], "Силлогизм Camestres")
            # remark_ru is UNCHANGED despite containing the transliterated token
            self.assertEqual(after[0]["remark_ru"], "remark has Каместрес")

    def test_apply_preserves_bom_and_crlf(self):
        rows = [_row(112, "Syllogisme Camestres", title_ru="Силлогизм Каместрес")]
        with tempfile.TemporaryDirectory() as d:
            p = os.path.join(d, "virtues.csv")
            self._write_csv(p, rows)
            plan, _ = M.build_conversion_plan(self._read_csv(p))
            M.apply_plan(p, plan)
            with open(p, "rb") as fh:
                raw = fh.read()
            self.assertTrue(raw.startswith(b"\xef\xbb\xbf"), "BOM must be preserved")
            # every newline is CRLF
            self.assertEqual(raw.replace(b"\r\n", b"").count(b"\n"), 0)

    def test_apply_is_idempotent(self):
        rows = [_row(112, "Syllogisme Camestres", title_ru="Силлогизм Каместрес",
                     title_fa="قیاس کامسترس")]
        with tempfile.TemporaryDirectory() as d:
            p = os.path.join(d, "virtues.csv")
            self._write_csv(p, rows)
            plan1, _ = M.build_conversion_plan(self._read_csv(p))
            n1 = M.apply_plan(p, plan1)
            plan2, _ = M.build_conversion_plan(self._read_csv(p))
            n2 = M.apply_plan(p, plan2)
            self.assertEqual(n1, 2)
            self.assertEqual(n2, 0, "second apply must change nothing (already Latin)")


# ─── 6. GROUNDING on the real Virtues CSV ────────────────────────────────────

@unittest.skipUnless(os.path.exists(REAL_CSV),
                     "real Virtues CSV not present (run from a full checkout)")
class TestGroundingRealCSV(unittest.TestCase):
    """The corpus contract: on master d90ce613 the plan is exactly 52 cells (RU 14 / AR 16 /
    ZH 6 / FA 16) with 0 ambiguous. If this drifts, the corpus changed under us — investigate
    BEFORE re-running --apply (the dispatch count is stale)."""

    def test_plan_is_52_cells_0_ambiguous(self):
        with open(REAL_CSV, encoding="utf-8-sig", newline="") as f:
            rows = list(csv.DictReader(f))
        plan, ambig = M.build_conversion_plan(rows)
        by_lang = {lang: sum(1 for e in plan if e["lang"] == lang) for lang in M.LANGS}
        self.assertEqual(len(plan), 52, f"plan size drifted: {len(plan)} (was 52)")
        self.assertEqual(by_lang, {"ru": 14, "ar": 16, "zh": 6, "fa": 16},
                         f"per-lang drift: {by_lang}")
        self.assertEqual(ambig, [], f"ambiguous cells appeared: {ambig}")

    def test_all_19_mnemonic_pks_present_in_corpus(self):
        with open(REAL_CSV, encoding="utf-8-sig", newline="") as f:
            rows = {int(r["pk"]): r for r in csv.DictReader(f)}
        for pk in M.MNEMONIC_PKS:
            self.assertIn(pk, rows, f"mnemonic pk {pk} missing from corpus")
            self.assertIsNotNone(M.extract_mnemonic_latin(rows[pk]["title_fr"]),
                                 f"pk {pk} title_fr lost its mnemonic")


if __name__ == "__main__":
    unittest.main(verbosity=2)
