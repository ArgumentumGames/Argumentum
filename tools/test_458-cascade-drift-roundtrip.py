#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contrat des allers-retours FR du detecteur de derive en cascade (#458, P24-42).

    python tools/test_458-cascade-drift-roundtrip.py

POURQUOI CE FICHIER EXISTE. Le detecteur `458-cascade-drift-scan.py` signale une
cellule dont la traduction n'a pas bouge pendant que le FR retrecissait. Le rapport
P24-07 (docs/translation/458-cascade-drift-scan-2026-11-23.md, §3) a mesure un angle
mort : le FR peut faire un ALLER-RETOUR (A -> A' -> A), et la traduction alignee sur
A est alors correcte -- pourtant le retour vers A ressemble a un resserrage. Sur le
corpus Fallacies, 19 des 45 candidats sont de cette forme ; les ecarter laisse 26.

CE QUI EST EPINGLE (depot git jetable, aucun corpus reel touche) :
  1. Un aller-retour est ECARTE par defaut : FR court -> long -> court, traduction
     jamais retouchee. C'est la forme de PK 1282 mesuree dans le rapport.
  2. Un vrai resserrage est GARDE : FR long -> court, jamais revenu, traduction
     jamais retouchee. Le controle inverse vit dans la meme passe : un instrument
     qui ecarterait tout ne passerait pas.
  3. Une cellule dont la traduction a suivi le FR n'est jamais candidate.
  4. `--keep-round-trips` restaure le comportement d'avant le 23/11 : 2 candidats
     au lieu de 1, sans quoi la correction serait invisible a la mesure.
  5. Le compte des ecartes et le groupe concerne sont imprimes : un chiffre sans
     son groupe ne se verifie pas.
"""
import csv
import io
import os
import pathlib
import shutil
import subprocess
import sys
import tempfile
import unittest

HERE = pathlib.Path(__file__).resolve().parent

SHORT = "Vous affirmez que la verite est subjective et propre a chaque individu."
LONG = ("Vous affirmez que la verite est subjective et propre a chacun, afin "
        "d'eviter de discuter des arguments ou des faits.")
B_LONG = ("L'argument s'appuie sur une particularite presente chez l'accuse que "
          "l'on ne trouve que chez dix pour cent de la population.")
B_SHORT = "L'argument s'appuie sur une particularite rare chez l'accuse."
C_LONG = ("La conclusion ne decoule pas des premisses avancees par l'adversaire "
          "dans ce debat contradictoire.")
C_SHORT = "La conclusion ne decoule pas des premisses."
HEADER = ["PK", "text_fr", "text_en", "text_es"]


def write_csv(path, rows):
    with io.open(path, "w", encoding="utf-8", newline="") as f:
        w = csv.writer(f, lineterminator="\r\n")
        w.writerow(HEADER)
        for r in rows:
            w.writerow(r)


def git(repo, *args):
    p = subprocess.run(["git"] + list(args), cwd=repo, capture_output=True)
    if p.returncode != 0:
        raise AssertionError("git %s: %s" % (" ".join(args), p.stderr.decode("utf-8", "replace")))


class RoundTripContract(unittest.TestCase):
    def setUp(self):
        self.repo = tempfile.mkdtemp(prefix="cascade-rt-")
        self.addCleanup(shutil.rmtree, self.repo, True)
        (pathlib.Path(self.repo) / "tools").mkdir()
        # The scanner loads `458-pass-fix-cells.py` for its `key_of`; copy both tools
        # so the disposable repo is self-contained and the real tree is never touched.
        for name in ("458-cascade-drift-scan.py", "458-pass-fix-cells.py"):
            shutil.copy(HERE / name, pathlib.Path(self.repo) / "tools" / name)
        git(self.repo, "init", "-q")
        git(self.repo, "config", "user.email", "test@example.invalid")
        git(self.repo, "config", "user.name", "contract")
        self.csv = os.path.join(self.repo, "corpus.csv")

        # c1: PK1 starts SHORT (the value it will return to), PK2/C start long.
        write_csv(self.csv, [
            [1, SHORT, "You claim truth is subjective.", "Afirmas que la verdad es subjetiva."],
            [2, B_LONG, "The argument leans on a rare trait.", "El argumento se apoya en un rasgo raro."],
            [3, C_LONG, "The conclusion does not follow.", "La conclusion no se sigue."],
        ])
        git(self.repo, "add", "corpus.csv")
        git(self.repo, "commit", "-q", "-m", "c1")

        # c2: PK1 GROWS (A -> A', no candidate), PK2 tightens (real drift),
        # PK3 tightens but its translations follow (never a candidate).
        write_csv(self.csv, [
            [1, LONG, "You claim truth is subjective.", "Afirmas que la verdad es subjetiva."],
            [2, B_SHORT, "The argument leans on a rare trait.", "El argumento se apoya en un rasgo raro."],
            [3, C_SHORT, "The conclusion does not follow from the premises.",
             "La conclusion no se sigue de las premisas."],
        ])
        git(self.repo, "add", "corpus.csv")
        git(self.repo, "commit", "-q", "-m", "c2")

        # c3: PK1 RETURNS to the short wording it already carried -> round trip.
        write_csv(self.csv, [
            [1, SHORT, "You claim truth is subjective.", "Afirmas que la verdad es subjetiva."],
            [2, B_SHORT, "The argument leans on a rare trait.", "El argumento se apoya en un rasgo raro."],
            [3, C_SHORT, "The conclusion does not follow from the premises.",
             "La conclusion no se sigue de las premisas."],
        ])
        git(self.repo, "add", "corpus.csv")
        git(self.repo, "commit", "-q", "-m", "c3")

    def run_scan(self, *extra):
        out = os.path.join(self.repo, "out.csv")
        p = subprocess.run(
            [sys.executable, "-I", os.path.join(self.repo, "tools", "458-cascade-drift-scan.py"),
             "--csv", "corpus.csv", "--out", out] + list(extra),
            cwd=self.repo, capture_output=True)
        self.assertEqual(p.returncode, 0, p.stderr.decode("utf-8", "replace"))
        rows = []
        if os.path.exists(out):
            with io.open(out, encoding="utf-8", newline="") as f:
                rows = list(csv.DictReader(f))
        return p.stdout.decode("utf-8", "replace"), rows

    def test_round_trip_is_dropped_and_real_drift_is_kept(self):
        stdout, rows = self.run_scan()
        # Rows are (PK, language) pairs; the group is (PK, field). The round trip is
        # ONE group and TWO rows (en + es), exactly the distinction the P24-07 report
        # draws between "19 lignes" and "6 groupes".
        self.assertIn("dropped as FR round-trips: 2 (groups: 1 text) | actionable: 2", stdout)
        self.assertEqual({r["pk"] for r in rows}, {"2"}, "one group left, the genuine tightening")
        self.assertEqual({r["lang"] for r in rows}, {"en", "es"})

    def test_keep_flag_restores_pre_2026_11_23_behaviour(self):
        stdout, rows = self.run_scan("--keep-round-trips")
        self.assertIn("actionable: 4", stdout)
        self.assertEqual({r["pk"] for r in rows}, {"1", "2"})

    def test_translation_that_followed_the_french_is_never_a_candidate(self):
        _, rows = self.run_scan("--keep-round-trips")
        self.assertNotIn("3", [r["pk"] for r in rows])


if __name__ == "__main__":
    unittest.main(verbosity=2)