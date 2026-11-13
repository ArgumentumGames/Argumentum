#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contrat de `doc-move-preservation.py` — stdlib seulement, dépôts git SYNTHÉTIQUES.

    python tools/test_doc-move-preservation.py

L'organe prétend prouver qu'un déplacement de documentation ne perd aucune ligne. Ce test
éprouve la prétention dans les deux sens : un déplacement propre sort PRÉSERVÉ, et chaque
façon de perdre une ligne sort PERTE avec le numéro de la ligne. Les dépôts sont créés
dans un répertoire temporaire (`git init`), donc le test ne dépend ni du dépôt réel, ni
de son historique, ni de la machine — il tourne sur un checkout nu.

CE QUI EST ÉPINGLÉ
   1. déplacement propre en deux fichiers            -> 0, PRÉSERVÉ, suites affichées
   2. une ligne supprimée                            -> 1, PERDUES nomme son numéro
   3. une ligne reformulée (un mot changé)           -> 1 — « presque pareil » est une perte
   4. la même ligne, déclarée au manifeste           -> 0, DÉCLARÉES 1
   5. manifeste déclarant une ligne NON absente      -> 1, MANIFESTE PÉRIMÉ
   6. ligne présente 2 fois, reprise 1 fois          -> 1 (multi-ensemble, pas ensemble)
   7. CRLF, espaces de fin, indentation changés      -> 0 (déclaré hors périmètre)
   8. aucun fichier d'arrivée                        -> 2, ORGANE AVEUGLE, jamais un vert
   9. source absente à la ref                        -> 2, ORGANE AVEUGLE
  10. ref d'origine inconnue                         -> 2, ORGANE AVEUGLE
  11. `--head <ref>` lit les BLOBS, pas l'arbre      -> le verdict suit la ref, pas le disque
  12. un dossier d'arrivée est parcouru (ses *.md)   -> 0
  13. manifeste en chemin relatif                    -> lu depuis la racine du dépôt, pas le cwd
  14. `--head <ref>` + manifeste relatif             -> lu DANS ce commit ; absent du commit -> 2
  15. deux sources partageant une ligne              -> décompte commun (2 reprises exigées)
  16. manifeste malformé (raison vide, doublon…)     -> 2, ORGANE AVEUGLE
"""
import os
import shutil
import subprocess
import sys
import tempfile
import unittest

HERE = os.path.dirname(os.path.abspath(__file__))
TOOL = os.path.join(HERE, "doc-move-preservation.py")

ORIGINAL = "\n".join([
    "# Guide",
    "",
    "## Build",
    "",
    "run the build",
    "then run the tests",
    "| a | b |",
    "|---|---|",
    "| 1 | 2 |",
    "",
    "## Traps",
    "",
    "never edit the json",
    "| a | b |",
    "|---|---|",
    "| 3 | 4 |",
    "the last line",
]) + "\n"

SLIM = "# Guide\n\nsee docs/build.md and docs/traps.md\n"
BUILD = "# Build notes\n\n## Build\n\nrun the build\nthen run the tests\n| a | b |\n|---|---|\n| 1 | 2 |\n"
TRAPS = "# Traps\n\n## Traps\n\nnever edit the json\n| a | b |\n|---|---|\n| 3 | 4 |\nthe last line\n"


class Repo:
    def __init__(self):
        self.root = tempfile.mkdtemp(prefix="doc-move-")
        self.git("init", "-q")
        self.write("GUIDE.md", ORIGINAL)
        self.base = self.commit("base")

    def git(self, *args):
        done = subprocess.run(
            ["git", "-C", self.root, "-c", "user.name=test", "-c", "user.email=test@example.invalid",
             "-c", "core.autocrlf=false", "-c", "commit.gpgsign=false"] + list(args),
            capture_output=True, text=True, encoding="utf-8")
        if done.returncode != 0:
            raise RuntimeError("git %s : %s" % (" ".join(args), done.stderr))
        return done.stdout.strip()

    def write(self, rel, text, newline="\n"):
        full = os.path.join(self.root, *rel.split("/"))
        os.makedirs(os.path.dirname(full), exist_ok=True)
        with open(full, "w", encoding="utf-8", newline=newline) as handle:
            handle.write(text)
        return full

    def commit(self, message):
        self.git("add", "-A")
        self.git("commit", "-q", "-m", message)
        return self.git("rev-parse", "HEAD")

    def move(self, slim=SLIM, build=BUILD, traps=TRAPS):
        self.write("GUIDE.md", slim)
        self.write("docs/build.md", build)
        self.write("docs/traps.md", traps)

    def run(self, *extra, into=("GUIDE.md", "docs"), sources=("GUIDE.md",), cwd=None):
        cmd = [sys.executable, TOOL, "--repo", self.root, "--base", self.base]
        for source in sources:
            cmd += ["--source", source]
        for spec in into:
            cmd += ["--into", spec]
        done = subprocess.run(cmd + list(extra), capture_output=True, cwd=cwd)
        return done.returncode, done.stdout.decode("utf-8", "replace")


class DocMovePreservationContract(unittest.TestCase):
    def setUp(self):
        self.repo = Repo()

    def tearDown(self):
        shutil.rmtree(self.repo.root, ignore_errors=True)

    def manifest(self, *entries):
        text = "# source\tligne\traison\n" + "".join("%s\t%d\t%s\n" % e for e in entries)
        return self.repo.write("manifest.tsv", text)

    def test_01_clean_move_is_preserved_and_shows_its_runs(self):
        self.repo.move()
        code, out = self.repo.run()
        self.assertEqual(code, 0, out)
        self.assertIn("VERDICT : PRÉSERVÉ", out)
        # Une suite englobe les lignes vides qui la bordent : elle part de L2, pas de L3.
        self.assertIn("L2-L9 -> docs/build.md L2-L9", out)
        self.assertIn("L10-L17 -> docs/traps.md L2-L9", out)
        self.assertIn("PERDUES : 0", out)

    def test_02_a_dropped_line_is_named(self):
        self.repo.move(traps=TRAPS.replace("never edit the json\n", ""))
        code, out = self.repo.run()
        self.assertEqual(code, 1, out)
        self.assertIn("PERDUES : 1", out)
        self.assertIn("L13  never edit the json", out)

    def test_03_a_reworded_line_is_a_loss(self):
        self.repo.move(traps=TRAPS.replace("never edit the json", "never edit that json"))
        code, out = self.repo.run()
        self.assertEqual(code, 1, out)
        self.assertIn("L13  never edit the json", out)

    def test_04_a_declared_line_is_accepted(self):
        self.repo.move(traps=TRAPS.replace("never edit the json", "never edit that json"))
        code, out = self.repo.run("--manifest", self.manifest(("GUIDE.md", 13, "reformulée")))
        self.assertEqual(code, 0, out)
        self.assertIn("DÉCLARÉES au manifeste : 1", out)
        self.assertIn("raison : reformulée", out)

    def test_05_a_useless_declaration_is_an_error(self):
        # Une déclaration qui ne couvre plus rien masquerait la prochaine perte au même numéro.
        self.repo.move()
        code, out = self.repo.run("--manifest", self.manifest(("GUIDE.md", 13, "plus vrai")))
        self.assertEqual(code, 1, out)
        self.assertIn("MANIFESTE PÉRIMÉ : 1", out)
        self.assertIn("GUIDE.md L13", out)

    def test_06_duplicates_are_counted_not_merely_found(self):
        # « | a | b | » figure 2 fois à l'origine ; une seule reprise = une perte.
        self.repo.move(traps=TRAPS.replace("| a | b |\n", ""))
        code, out = self.repo.run()
        self.assertEqual(code, 1, out)
        self.assertIn("PERDUES : 1", out)
        self.assertIn("L14  | a | b |", out)

    def test_07_line_endings_and_edge_whitespace_are_out_of_scope(self):
        self.repo.move()
        self.repo.write("docs/build.md", BUILD.replace("run the build", "   run the build  "),
                        newline="\r\n")
        code, out = self.repo.run()
        self.assertEqual(code, 0, out)

    def test_08_no_target_file_is_blind_not_green(self):
        self.repo.move()
        code, out = self.repo.run(into=("nowhere",))
        self.assertEqual(code, 2, out)
        self.assertIn("ORGANE AVEUGLE", out)

    def test_09_missing_source_is_blind(self):
        self.repo.move()
        done = subprocess.run([sys.executable, TOOL, "--repo", self.repo.root, "--base",
                               self.repo.base, "--source", "ABSENT.md", "--into", "docs"],
                              capture_output=True)
        self.assertEqual(done.returncode, 2)
        self.assertIn("ORGANE AVEUGLE", done.stdout.decode("utf-8", "replace"))

    def test_10_unknown_base_ref_is_blind(self):
        self.repo.move()
        done = subprocess.run([sys.executable, TOOL, "--repo", self.repo.root, "--base",
                               "refs/heads/does-not-exist", "--source", "GUIDE.md",
                               "--into", "docs"], capture_output=True)
        self.assertEqual(done.returncode, 2)

    def test_11_head_ref_reads_blobs_not_the_working_tree(self):
        self.repo.move()
        good = self.repo.commit("moved")
        # Le disque perd une ligne APRÈS le commit : --head doit rester vert, l'arbre rouge.
        self.repo.write("docs/traps.md", TRAPS.replace("the last line\n", ""))
        code_head, out_head = self.repo.run("--head", good)
        code_tree, out_tree = self.repo.run()
        self.assertEqual(code_head, 0, out_head)
        self.assertEqual(code_tree, 1, out_tree)
        self.assertIn("L17  the last line", out_tree)

    def test_12_a_directory_target_is_walked(self):
        self.repo.move()
        code, out = self.repo.run(into=("docs", "GUIDE.md"))
        self.assertEqual(code, 0, out)
        self.assertIn("docs/build.md", out)
        self.assertIn("docs/traps.md", out)

    def test_13_a_relative_manifest_is_read_from_the_repo_root_not_the_cwd(self):
        # La commande documentée donne un chemin relatif au dépôt ; lancée d'ailleurs, elle
        # doit lire le même fichier — pas rendre « introuvable », encore moins un autre fichier.
        self.repo.move(traps=TRAPS.replace("never edit the json", "never edit that json"))
        self.repo.write("docs/manifest.tsv", "GUIDE.md\t13\treformulée\n")
        elsewhere = tempfile.mkdtemp(prefix="doc-move-cwd-")
        try:
            code, out = self.repo.run("--manifest", "docs/manifest.tsv", cwd=elsewhere)
        finally:
            shutil.rmtree(elsewhere, ignore_errors=True)
        self.assertEqual(code, 0, out)
        self.assertIn("MANIFESTE : docs/manifest.tsv (arbre de travail), 1 déclaration(s)", out)
        code, out = self.repo.run("--manifest", "docs/absent.tsv")
        self.assertEqual(code, 2, out)

    def test_14_with_head_the_manifest_is_read_from_that_commit(self):
        # La preuve à un commit n'emprunte rien au disque : ni les fichiers, ni le manifeste.
        self.repo.move(traps=TRAPS.replace("never edit the json", "never edit that json"))
        self.repo.write("docs/manifest.tsv", "GUIDE.md\t13\treformulée\n")
        good = self.repo.commit("moved, declared")
        self.repo.write("docs/manifest.tsv", "# vidé après le commit\n")
        code_head, out_head = self.repo.run("--manifest", "docs/manifest.tsv", "--head", good)
        code_tree, out_tree = self.repo.run("--manifest", "docs/manifest.tsv")
        self.assertEqual(code_head, 0, out_head)
        self.assertIn("MANIFESTE : docs/manifest.tsv @ %s, 1 déclaration(s)" % good, out_head)
        self.assertEqual(code_tree, 1, out_tree)
        # Un manifeste absent du commit examiné : aveugle, jamais un repli sur le disque.
        self.repo.write("docs/later.tsv", "GUIDE.md\t13\treformulée\n")
        code, out = self.repo.run("--manifest", "docs/later.tsv", "--head", good)
        self.assertEqual(code, 2, out)
        self.assertIn("absent du commit", out)

    def test_15_two_sources_share_one_count(self):
        # « the last line » figure dans les deux sources : une seule reprise = une perte.
        self.repo.write("OTHER.md", "# Other\n\nthe last line\n")
        self.repo.base = self.repo.commit("second source")
        self.repo.move()
        self.repo.write("docs/other.md", "# Other\n")
        code, out = self.repo.run(sources=("GUIDE.md", "OTHER.md"))
        self.assertEqual(code, 1, out)
        self.assertIn("L3  the last line", out)
        self.repo.write("docs/other.md", "# Other\n\nthe last line\n")
        code, out = self.repo.run(sources=("GUIDE.md", "OTHER.md"))
        self.assertEqual(code, 0, out)

    def test_16_a_malformed_manifest_is_blind(self):
        self.repo.move(traps=TRAPS.replace("never edit the json", "never edit that json"))
        for text in ("GUIDE.md\t13\t\n",                                   # raison vide
                     "GUIDE.md\ttreize\treformulée\n",                      # numéro illisible
                     "GUIDE.md\t13\tune\nGUIDE.md\t13\tdeux\n"):            # déclarée deux fois
            path = self.repo.write("manifest.tsv", text)
            code, out = self.repo.run("--manifest", path)
            self.assertEqual(code, 2, (text, out))
            self.assertIn("ORGANE AVEUGLE", out)


if __name__ == "__main__":
    unittest.main()
