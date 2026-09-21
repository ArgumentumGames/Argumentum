#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contrat de la garde d'arbre #1460 — stdlib seulement, aucun artefact requis (#458 v8 ⑦).

    python tools/test_1460-structure-charpente-pilot.py

POURQUOI CE FICHIER EXISTE. Le DoD du pool exige, par grain : *instrument + contrôle
inverse étalonné + mutation falsifiante*. Le pilot de la garde (`1460-structure-charpente-
pilot.py`) a été livré AVEC sa mutation, mais celle-ci vivait dans un scratchpad : un
relecteur ne pouvait pas la rejouer, il devait croire une transcription. Une mutation
non committée n'est pas un contrôle, c'est une affirmation.

CE QUI EST ÉPINGLÉ (les 4 états d'arbre, et rien d'autre) :
  1. arbre ANTÉRIEUR au corpus      -> sortie 2, motif « ANTERIEUR au corpus »
  2. arbre POSTÉRIEUR au corpus     -> sortie 0, « MESURABLE »
  3. arbre frais SANS PDF           -> sortie 2, motif « 0 PDF »
  4. arbre ABSENT                   -> sortie 2, motif « arbre ABSENT »
  5. dépôt sans `Cards/`            -> refus net (« REPO INVALIDE »), jamais un vert
  6. l'état 1 (DIVERGENT) est DÉCLARÉ INATTEIGNABLE par l'organe lui-même — le contrat
     porte que le mot est imprimé, pas qu'un état soit atteint : ⛔ « verdict à trois
     états » ne se lit pas comme « trois états implémentés ».

GROUNDING (cas 7) — sur le dépôt réel : la garde doit pouvoir tourner et rendre un
VERDICT sans jamais planter. ⚠️ Ce cas n'asserte PAS la valeur du verdict (0 ou 2 selon
la machine et la fraîcheur de l'arbre) : asserter un code ici rendrait le test rouge sur
la machine qui n'a pas d'arbre, c'est-à-dire exactement sur le cas nominal de la garde.

⚠️ Les arbres des cas 1-5 sont SYNTHÉTIQUES (tempdir) et datés par `os.utime` : le test
ne dépend ni de la machine, ni d'un `Target/`, ni du bundle. C'est la condition pour
qu'il soit exécutable là où il n'y a rien à mesurer — comme la garde qu'il éprouve.
"""
import os
import shutil
import subprocess
import sys
import tempfile
import time
import unittest

HERE = os.path.dirname(os.path.abspath(__file__))
PILOT = os.path.join(HERE, "1460-structure-charpente-pilot.py")
REL = os.path.join("Generation", "Converters", "Argumentum.AssetConverter",
                   "bin", "Debug", "net9.0-windows", "Target")
NOW = time.time()


def run(repo):
    return subprocess.run([sys.executable, PILOT, "--repo", repo],
                          capture_output=True, text=True, encoding="utf-8")


class TreeGuardContract(unittest.TestCase):
    def setUp(self):
        self.roots = []

    def tearDown(self):
        for r in self.roots:
            shutil.rmtree(r, ignore_errors=True)

    def mk(self, days_tree, days_corpus, with_pdf=True):
        """Arbre synthétique : `days_*` = ancienneté en jours (plus grand = plus vieux)."""
        root = tempfile.mkdtemp(prefix="guard1460-")
        self.roots.append(root)
        os.makedirs(os.path.join(root, "Cards"))
        csv = os.path.join(root, "Cards", "Argumentum Fallacies - Taxonomy.csv")
        with open(csv, "w", encoding="utf-8") as fh:
            fh.write("pk;carte\n1;x\n")
        os.utime(csv, (NOW - days_corpus * 86400,) * 2)
        tree = os.path.join(root, REL)
        os.makedirs(os.path.join(tree, "fr", "Documents"))
        if with_pdf:
            pdf = os.path.join(tree, "fr", "Documents", "Argumentum_TarotCards_fr.pdf")
            with open(pdf, "wb") as fh:
                fh.write(b"%PDF-1.4\n")
            os.utime(pdf, (NOW - days_tree * 86400,) * 2)
        return root

    def test_01_stale_tree_is_not_measurable(self):
        """Arbre antérieur au corpus : les organes asserteraient sur du pré-édition."""
        r = run(self.mk(days_tree=5, days_corpus=0))
        self.assertEqual(r.returncode, 2, r.stdout)
        self.assertIn("ANTERIEUR au corpus", r.stdout)

    def test_02_fresh_tree_is_measurable(self):
        """Le seul état qui autorise les phases A/B/C à asserter."""
        r = run(self.mk(days_tree=0, days_corpus=1))
        self.assertEqual(r.returncode, 0, r.stdout)
        self.assertIn("MESURABLE", r.stdout)

    def test_03_fresh_tree_without_pdf_is_not_measurable(self):
        """Le piège central : une boucle vide ne produit aucun échec, donc « vert »."""
        r = run(self.mk(days_tree=0, days_corpus=1, with_pdf=False))
        self.assertEqual(r.returncode, 2, r.stdout)
        self.assertIn("0 PDF", r.stdout)

    def test_04_absent_tree_is_not_measurable(self):
        root = tempfile.mkdtemp(prefix="guard1460-")
        self.roots.append(root)
        os.makedirs(os.path.join(root, "Cards"))
        with open(os.path.join(root, "Cards", "x.csv"), "w", encoding="utf-8") as fh:
            fh.write("pk\n1\n")
        r = run(root)
        self.assertEqual(r.returncode, 2, r.stdout)
        self.assertIn("arbre ABSENT", r.stdout)

    def test_05_repo_without_cards_is_refused(self):
        """Un dépôt invalide ne doit jamais rendre un verdict d'arbre : refus net."""
        root = tempfile.mkdtemp(prefix="guard1460-")
        self.roots.append(root)
        r = run(root)
        self.assertNotEqual(r.returncode, 0, r.stdout)
        self.assertIn("REPO INVALIDE", r.stdout + r.stderr)

    def test_06_divergent_state_is_declared_unreachable(self):
        """⛔ L'organe doit DIRE que l'état 1 est inatteignable, pas le laisser croire."""
        r = run(self.mk(days_tree=0, days_corpus=1))
        self.assertIn("INATTEIGNABLE", r.stdout)

    def test_07_grounding_on_real_repo(self):
        """Grounding : la garde tourne sur le dépôt réel et rend un verdict, sans planter.

        ⚠️ On n'asserte PAS le code (0 ou 2) : il dépend de la fraîcheur de l'arbre de la
        machine, et figer 0 rendrait ce test rouge partout où la garde est justement
        censée dire « non mesurable »."""
        r = run(os.path.dirname(HERE))
        self.assertIn(r.returncode, (0, 1, 2), r.stdout)
        self.assertIn("VERDICT", r.stdout)


if __name__ == "__main__":
    unittest.main(verbosity=2)
