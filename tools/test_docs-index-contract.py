#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contrat des index de dossier (« index of record ») — stdlib + `git ls-files`.

    python tools/test_docs-index-contract.py

POURQUOI. Un dossier de constats datés ne fait que grossir. À 150 fichiers, plus personne
ne sait lequel dit l'état courant : le plus ancien est souvent le plus cité, et un constat
remplacé se fait encore exécuter. Consolider un tel dossier ne veut pas dire le vider —
ses fichiers sont des pièces — mais dire, pour CHACUN, ce qu'il vaut aujourd'hui.

Un INDEX est un `README.md` qui porte, seule sur une ligne, la marque

    <!-- index-of-record -->

Par cette marque il s'engage à citer chaque fichier suivi de son dossier, avec son statut.
Sans marque, un README n'est tenu à rien : le contrat ne s'applique qu'aux dossiers qui
ont été consolidés, et il les empêche ensuite de se défaire.

CE QUI EST ÉPINGLÉ, pour chaque index (sous `docs/` et `tools/`)
   1. complétude — chaque fichier SUIVI du dossier, sous-dossiers compris, est cité : par
      un lien vers lui, ou par un lien vers un sous-dossier qui le contient. Un fichier
      ajouté sans ligne d'index rougit la PR qui l'ajoute
   2. aucun lien mort, à la casse exacte
   3. statut — toute ligne de tableau ou de liste qui cite un fichier du dossier porte UN
      statut, entre accents graves : `current` (dit l'état d'aujourd'hui), `superseded`
      (remplacé : la ligne nomme aussi ce qui le remplace, par un second lien ou une
      référence #N), `historical` (pièce d'un travail clos : vraie à sa date, à ne pas
      exécuter)
   4. un sous-dossier qui porte son propre index est tenu par cet index-là ; l'index
      parent doit seulement le citer
   5. plancher — au moins un index existe : sinon ce contrat est vert sans rien tenir
   6-12. contrôles inverses, sur des dépôts `git init` jetables : chaque défaut ci-dessus EST
      vu, et ce qui n'en est pas un (prose, lien hors dossier, fichier non suivi) ne l'est pas

CE QUI N'EST PAS ÉPINGLÉ : que le statut soit JUSTE (c'est le jugement de qui consolide,
et la contre-revue le relit) ; les fichiers non suivis par git ; le contenu des fichiers
cités. Les liens sont lus par les détecteurs de `tools/test_agent-docs-contract.py`
(mêmes formes, mêmes limites).
"""
import importlib.util
import io
import os
import re
import shutil
import subprocess
import tempfile
import unittest

HERE = os.path.dirname(os.path.abspath(__file__))
ROOT = os.path.dirname(HERE)
ROOTS = ("docs", "tools")
MARKER = "<!-- index-of-record -->"
STATUS = re.compile(r"`(current|superseded|historical)`")
ISSUE_REF = re.compile(r"(?<![\w&])#[0-9]+")

_spec = importlib.util.spec_from_file_location("agent_docs_contract",
                                               os.path.join(HERE, "test_agent-docs-contract.py"))
docs = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(docs)


def tracked(root, under):
    """Les fichiers suivis par git sous `under`. Git muet = contrat aveugle, jamais vert."""
    done = subprocess.run(["git", "-C", root, "ls-files", "-z", "--", under], capture_output=True)
    if done.returncode != 0:
        raise RuntimeError("git ls-files a échoué sous %s : %s"
                           % (root, done.stderr.decode("utf-8", "replace").strip()))
    return [path for path in done.stdout.decode("utf-8").split("\0") if path]


def read(root, rel):
    with io.open(os.path.join(root, *rel.split("/")), encoding="utf-8") as handle:
        return handle.read()


def is_index(text):
    """La marque, seule sur sa ligne, hors bloc de code : la citer n'est pas la porter."""
    return any(line.strip() == MARKER for line in docs.prose(text).split("\n"))


def indexes(root):
    """Les dossiers dont le README.md suivi porte la marque."""
    found = []
    for top in ROOTS:
        for rel in tracked(root, top):
            if os.path.basename(rel) == "README.md" and is_index(read(root, rel)):
                found.append(os.path.dirname(rel))
    return sorted(found)


def cited(root, folder):
    """[(ligne, [(cible telle qu'écrite, chemin depuis la racine)], cibles distinctes)] — les lignes
    du README qui lient vers le dépôt. Le troisième terme compte TOUTES les cibles de la ligne,
    externes comprises : ce qui remplace un fichier peut être une issue ou une page."""
    lines = []
    for line in docs.prose(read(root, folder + "/README.md")).split("\n"):
        raws = list(docs.link_targets(line))
        targets = []
        for raw in raws:
            path = docs.local_path(raw)
            if path is None:
                continue
            where = path.lstrip("/") if path.startswith("/") else os.path.join(folder, path)
            targets.append((raw, os.path.normpath(where).replace(os.sep, "/")))
        if targets:
            lines.append((line, targets, len(set(raws))))
    return lines


def defects(root, folder, every_index):
    """Les défauts de l'index du dossier `folder` — liste vide = l'index tient."""
    found = []
    nested = [d for d in every_index if d.startswith(folder + "/")]
    owed = [path for path in tracked(root, folder)
            if path != folder + "/README.md" and not any(path.startswith(d + "/") for d in nested)]
    files, folders = set(), set()
    for line, targets, distinct in cited(root, folder):
        inside = []
        for raw, where in targets:
            if not docs.exists_exact(root, where):
                found.append("lien mort : %s" % raw)
            elif where.startswith(folder + "/"):
                inside.append(where)
                (folders if os.path.isdir(os.path.join(root, *where.split("/"))) else files).add(where)
        if inside and line.lstrip().startswith(("|", "-", "*")):
            statuses = STATUS.findall(line)
            if len(statuses) != 1:
                found.append("statut : %d sur la ligne de %s (attendu : un seul)" % (len(statuses), inside[0]))
            elif statuses[0] == "superseded" and distinct < 2 and not ISSUE_REF.search(line):
                found.append("superseded sans successeur : %s" % inside[0])
    for path in owed:
        if path not in files and not any(path.startswith(d + "/") for d in folders):
            found.append("non cité : %s" % path)
    for sub in nested:
        if any(sub.startswith(other + "/") for other in nested):
            continue                                    # tenu par un sous-index plus proche
        if sub not in folders and sub + "/README.md" not in files:
            found.append("sous-index non cité : %s" % sub)
    return found


class DocsIndexContract(unittest.TestCase):
    def test_01_every_index_of_record_holds(self):
        found = indexes(ROOT)
        # Plancher : sans aucun index, ce contrat serait vert sans rien tenir.
        self.assertTrue(found, "aucun README.md ne porte la marque %s" % MARKER)
        for folder in found:
            self.assertEqual(defects(ROOT, folder, found), [], folder)


class InverseControls(unittest.TestCase):
    """Chaque défaut doit être VU — et ce qui n'en est pas un ne doit pas l'être."""

    INDEX = ("# Area\n\n" + MARKER + "\n\n| File | Status |\n|---|---|\n"
             "| [a](a.md) | `current` |\n| [b](b.md) | `historical` |\n")

    def setUp(self):
        self.root = tempfile.mkdtemp(prefix="docs-index-")
        subprocess.run(["git", "-C", self.root, "init", "-q"], check=True, capture_output=True)
        self.write("docs/area/README.md", self.INDEX)
        self.write("docs/area/a.md", "# A\n")
        self.write("docs/area/b.md", "# B\n")

    def tearDown(self):
        shutil.rmtree(self.root, ignore_errors=True)

    def write(self, rel, text, track=True):
        full = os.path.join(self.root, *rel.split("/"))
        if not os.path.isdir(os.path.dirname(full)):
            os.makedirs(os.path.dirname(full))
        with io.open(full, "w", encoding="utf-8", newline="\n") as handle:
            handle.write(text)
        if track:
            subprocess.run(["git", "-C", self.root, "add", "--", rel], check=True, capture_output=True)

    def check(self, folder="docs/area"):
        return defects(self.root, folder, indexes(self.root))

    def test_06_a_healthy_index_is_clean_then_an_unlisted_file_is_seen(self):
        self.assertEqual(indexes(self.root), ["docs/area"])
        self.assertEqual(self.check(), [])
        # Une cible écrite depuis la racine du dépôt (`/docs/…`) désigne le même fichier.
        self.write("docs/area/README.md", self.INDEX.replace("(a.md)", "(/docs/area/a.md)"))
        self.assertEqual(self.check(), [])
        self.write("docs/area/new-finding.md", "# New\n")
        self.assertEqual(self.check(), ["non cité : docs/area/new-finding.md"])
        # Un fichier non suivi par git n'est dû à personne : c'est du brouillon local.
        self.write("docs/area/scratch.md", "# Scratch\n", track=False)
        self.assertEqual(self.check(), ["non cité : docs/area/new-finding.md"])

    def test_07_a_dead_or_wrong_case_link_is_seen(self):
        self.write("docs/area/README.md", self.INDEX + "| [c](gone.md) | `current` |\n")
        self.assertEqual(self.check(), ["lien mort : gone.md"])
        self.write("docs/area/README.md", self.INDEX.replace("(a.md)", "(A.md)"))
        self.assertEqual(self.check(), ["lien mort : A.md", "non cité : docs/area/a.md"])

    def test_08_every_row_carries_exactly_one_status(self):
        self.write("docs/area/README.md", self.INDEX.replace("| [a](a.md) | `current` |", "| [a](a.md) | à jour |"))
        self.assertEqual(self.check(), ["statut : 0 sur la ligne de docs/area/a.md (attendu : un seul)"])
        self.write("docs/area/README.md", self.INDEX.replace("| `current` |", "| `current` `historical` |"))
        self.assertEqual(self.check(), ["statut : 2 sur la ligne de docs/area/a.md (attendu : un seul)"])
        # En liste aussi ; une phrase de prose qui cite un fichier, elle, n'a pas à porter de statut.
        self.write("docs/area/README.md", "# Area\n\n" + MARKER + "\n\nStart with [a](a.md).\n\n"
                                          "- [a](a.md) — `current`\n- [b](b.md)\n")
        self.assertEqual(self.check(), ["statut : 0 sur la ligne de docs/area/b.md (attendu : un seul)"])
        # Une ligne de liste qui ne cite que l'extérieur du dossier n'indexe rien : pas de statut dû.
        self.write("docs/other.md", "# Other\n")
        self.write("docs/area/README.md", self.INDEX + "\n- See also [other](../other.md).\n")
        self.assertEqual(self.check(), [])

    def test_09_superseded_names_its_successor(self):
        row = "| [b](b.md) | `historical` |"
        self.write("docs/area/README.md", self.INDEX.replace(row, "| [b](b.md) | `superseded` |"))
        self.assertEqual(self.check(), ["superseded sans successeur : docs/area/b.md"])
        for successor in ("by [a](a.md)", "by #1234", "by [the issue](https://example.invalid/1)"):
            self.write("docs/area/README.md", self.INDEX.replace(row, "| [b](b.md) | `superseded` %s |" % successor))
            self.assertEqual(self.check(), [], successor)
        # `&#39;` est une entité HTML, pas une référence d'issue.
        self.write("docs/area/README.md", self.INDEX.replace(row, "| [b](b.md) | `superseded` &#39; |"))
        self.assertEqual(self.check(), ["superseded sans successeur : docs/area/b.md"])

    def test_10_a_folder_link_covers_its_files_and_only_them(self):
        self.write("docs/area/evidence/shot-1.png", "x")
        self.write("docs/area/evidence/deep/shot-2.png", "x")
        self.assertEqual(self.check(), ["non cité : docs/area/evidence/deep/shot-2.png",
                                        "non cité : docs/area/evidence/shot-1.png"])
        self.write("docs/area/README.md", self.INDEX + "| [evidence](evidence/) | `historical` |\n")
        self.assertEqual(self.check(), [])
        # Un lien vers le dossier de l'index lui-même ne couvre rien : ce serait tout déclarer d'un mot.
        self.write("docs/area/README.md", self.INDEX + "| [all](./) | `historical` |\n")
        self.assertEqual(self.check(), ["non cité : docs/area/evidence/deep/shot-2.png",
                                        "non cité : docs/area/evidence/shot-1.png"])

    def test_11_a_nested_index_holds_its_own_files_and_must_be_cited(self):
        self.write("docs/area/sub/README.md", "# Sub\n\n" + MARKER + "\n\n- [x](x.md) — `current`\n")
        self.write("docs/area/sub/x.md", "# X\n")
        self.write("docs/area/sub/y.md", "# Y\n")
        self.assertEqual(indexes(self.root), ["docs/area", "docs/area/sub"])
        self.assertEqual(self.check(), ["sous-index non cité : docs/area/sub"])
        self.assertEqual(self.check("docs/area/sub"), ["non cité : docs/area/sub/y.md"])
        self.write("docs/area/README.md", self.INDEX + "| [sub](sub/README.md) | `current` |\n")
        self.assertEqual(self.check(), [])
        # Un index plus profond est dû à l'index le plus proche, pas à tous ses ancêtres.
        self.write("docs/area/sub/deep/README.md", "# Deep\n\n" + MARKER + "\n")
        self.assertEqual(self.check(), [])
        self.assertEqual(self.check("docs/area/sub"), ["non cité : docs/area/sub/y.md",
                                                       "sous-index non cité : docs/area/sub/deep"])
        # `tools/` est lu comme `docs/`.
        self.write("tools/README.md", "# Tools\n\n" + MARKER + "\n")
        self.write("tools/x.py", "pass\n")
        self.assertEqual(indexes(self.root), ["docs/area", "docs/area/sub", "docs/area/sub/deep", "tools"])
        self.assertEqual(self.check("tools"), ["non cité : tools/x.py"])

    def test_12_quoting_the_marker_is_not_carrying_it_and_no_git_is_blind(self):
        self.write("docs/area/README.md", "# Area\n\nAdd `" + MARKER + "` to opt in:\n\n```\n" + MARKER + "\n```\n")
        self.assertEqual(indexes(self.root), [])
        elsewhere = tempfile.mkdtemp(prefix="docs-index-nogit-")
        try:
            with self.assertRaises(RuntimeError):
                indexes(elsewhere)
        finally:
            shutil.rmtree(elsewhere, ignore_errors=True)


if __name__ == "__main__":
    unittest.main()
