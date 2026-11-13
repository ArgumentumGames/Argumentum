#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contrat des documents d'entrée pour agents — stdlib seulement, aucun réseau, aucun git.

    python tools/test_agent-docs-contract.py

POURQUOI. `AGENTS.md` et `CLAUDE.md` sont chargés à CHAQUE session ; le détail vit dans
`docs/agents/`, lu à la demande. Ce partage ne tient que si trois choses restent vraies,
et aucune ne se voit à la relecture d'une PR :

  - un renvoi (« lis `regeneration.md` § *Regeneration runs* ») mène quelque part. Une
    section renommée laisse un renvoi mort, et un renvoi mort se lit « rien à lire » ;
  - le détail ne revient pas dans les fichiers chargés par défaut — c'est ainsi que
    `CLAUDE.md` avait atteint 61 Ko ;
  - `docs/agents/` n'est pas importé : un `@docs/agents/…` le rechargerait à chaque session.

CE QUI EST ÉPINGLÉ
   1. tout lien markdown relatif des documents d'entrée désigne un chemin qui existe
   2. tout renvoi `fichier.md` § *Titre* désigne un titre qui existe dans ce fichier
   3. `CLAUDE.md` importe `AGENTS.md` une fois, et rien d'autre n'est importé
   4. `AGENTS.md` + `CLAUDE.md` restent sous BUDGET octets
   5-7. contrôles inverses, sur un arbre synthétique : un lien mort, un renvoi vers un
        titre absent, un import ajouté SONT vus (sinon les cas 1 à 3 ne prouveraient rien)

CE QUI N'EST PAS ÉPINGLÉ : que le texte soit juste ; les liens http(s) ; les ancres `#…` ;
les renvois en prose sans la forme `fichier.md` § *Titre*. La conservation ligne à ligne
d'un déplacement se prouve ailleurs (`tools/doc-move-preservation.py`).

CHANGER LE BUDGET est légitime : c'est une décision, à prendre dans la PR qui le change,
pas un chiffre à relever pour faire passer un ajout. Le détail va dans `docs/agents/`.
"""
import io
import os
import re
import shutil
import tempfile
import unittest

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
ENTRY = ("AGENTS.md", "CLAUDE.md")
DETAIL_DIR = "docs/agents"
BUDGET = 32000      # octets, AGENTS.md + CLAUDE.md. Mesuré 2026-11-13 : 21 938 (61 Ko avant).

LINK = re.compile(r"\]\(([^)\s]+)\)")
POINTER = re.compile(r"`([^`]+\.md)`|§ \*([^*]+)\*")
HEADING = re.compile(r"(?m)^#{1,6} (.+)$")
IMPORT = re.compile(r"(?m)^@(\S+)[ \t]*$")


def read(root, rel):
    with io.open(os.path.join(root, *rel.split("/")), encoding="utf-8") as handle:
        return handle.read()


def documents(root):
    """Les documents d'entrée et les fichiers de détail présents."""
    found = [rel for rel in ENTRY if os.path.isfile(os.path.join(root, rel))]
    detail = os.path.join(root, *DETAIL_DIR.split("/"))
    if os.path.isdir(detail):
        found += sorted("%s/%s" % (DETAIL_DIR, name) for name in os.listdir(detail)
                        if name.lower().endswith(".md"))
    return found


def dead_links(root):
    """[(document, cible)] — liens markdown relatifs dont la cible n'existe pas."""
    dead = []
    for rel in documents(root):
        for match in LINK.finditer(read(root, rel)):
            target = match.group(1)
            if re.match(r"^(https?:|mailto:|#)", target):
                continue
            path = target.split("#")[0]
            full = os.path.normpath(os.path.join(root, os.path.dirname(rel), path))
            if not os.path.exists(full):
                dead.append((rel, target))
    return dead


def resolve(root, citing, name):
    """Un fichier cité : depuis la racine, sous docs/agents/, ou à côté du document citant."""
    for candidate in (name, "%s/%s" % (DETAIL_DIR, name),
                      os.path.normpath(os.path.join(os.path.dirname(citing), name)).replace(os.sep, "/")):
        if os.path.isfile(os.path.join(root, *candidate.split("/"))):
            return candidate
    return None


def pointers(root):
    """[(document, fichier cité, titre)] — la forme `fichier.md` § *Titre* [, § *Titre*…]."""
    found = []
    for rel in documents(root):
        for line in read(root, rel).split("\n"):
            current = None
            for match in POINTER.finditer(line):
                if match.group(1):
                    current = match.group(1)
                elif current:
                    found.append((rel, current, match.group(2).rstrip("…").strip()))
    return found


def dead_pointers(root):
    dead = []
    for rel, name, title in pointers(root):
        target = resolve(root, rel, name)
        if target is None:
            dead.append((rel, name, title, "fichier absent"))
            continue
        titles = [m.group(1).strip() for m in HEADING.finditer(read(root, target))]
        if not any(t.startswith(title) for t in titles):
            dead.append((rel, target, title, "titre absent"))
    return dead


def imports(root):
    """[(document, cible)] — lignes `@chemin` des documents d'entrée."""
    return [(rel, m.group(1)) for rel in ENTRY if os.path.isfile(os.path.join(root, rel))
            for m in IMPORT.finditer(read(root, rel))]


class AgentDocsContract(unittest.TestCase):
    def test_01_relative_links_resolve(self):
        self.assertEqual(dead_links(ROOT), [])
        # Plancher : un zéro n'est une absence que si l'instrument a vu des liens.
        seen = sum(len(LINK.findall(read(ROOT, rel))) for rel in documents(ROOT))
        self.assertGreaterEqual(seen, 20, "trop peu de liens vus : l'instrument est-il aveugle ?")

    def test_02_section_pointers_resolve(self):
        self.assertEqual(dead_pointers(ROOT), [])
        self.assertGreaterEqual(len(pointers(ROOT)), 10,
                                "trop peu de renvois vus : la forme `fichier.md` § *Titre* a-t-elle changé ?")

    def test_03_only_agents_md_is_imported(self):
        self.assertEqual(imports(ROOT), [("CLAUDE.md", "AGENTS.md")],
                         "CLAUDE.md importe AGENTS.md, une fois ; rien d'autre ne se charge par défaut")

    def test_04_default_loaded_documents_stay_under_budget(self):
        sizes = dict((rel, os.path.getsize(os.path.join(ROOT, rel))) for rel in ENTRY)
        self.assertLessEqual(
            sum(sizes.values()), BUDGET,
            "%s = %d octets > %d. Ces deux fichiers sont chargés à chaque session : le détail va "
            "dans docs/agents/ (règle : docs/agents/README.md)." % (sizes, sum(sizes.values()), BUDGET))


class InverseControls(unittest.TestCase):
    """Chaque détecteur doit VOIR le défaut qu'il prétend exclure."""

    def setUp(self):
        self.root = tempfile.mkdtemp(prefix="agent-docs-")
        os.makedirs(os.path.join(self.root, "docs", "agents"))
        self.write("AGENTS.md", "# Entry\n\nSee [detail](docs/agents/topic.md) and `topic.md` § *Real title*.\n")
        self.write("CLAUDE.md", "# Claude\n\n@AGENTS.md\n")
        self.write("docs/agents/topic.md", "# Topic\n\n## Real title (dated)\n\ntext\n")

    def tearDown(self):
        shutil.rmtree(self.root, ignore_errors=True)

    def write(self, rel, text):
        with io.open(os.path.join(self.root, *rel.split("/")), "w", encoding="utf-8", newline="\n") as handle:
            handle.write(text)

    def test_05_healthy_synthetic_tree_is_clean_then_a_dead_link_is_seen(self):
        self.assertEqual((dead_links(self.root), dead_pointers(self.root)), ([], []))
        self.write("AGENTS.md", "# Entry\n\nSee [detail](docs/agents/renamed.md).\n")
        self.assertEqual(dead_links(self.root), [("AGENTS.md", "docs/agents/renamed.md")])

    def test_06_a_pointer_to_a_renamed_heading_is_seen(self):
        self.write("docs/agents/topic.md", "# Topic\n\n## Another title\n\ntext\n")
        self.assertEqual(dead_pointers(self.root),
                         [("AGENTS.md", "docs/agents/topic.md", "Real title", "titre absent")])
        self.write("AGENTS.md", "# Entry\n\nSee `gone.md` § *Real title*.\n")
        self.assertEqual(dead_pointers(self.root), [("AGENTS.md", "gone.md", "Real title", "fichier absent")])

    def test_07_an_added_import_is_seen(self):
        self.write("CLAUDE.md", "# Claude\n\n@AGENTS.md\n@docs/agents/topic.md\n")
        self.assertEqual(imports(self.root),
                         [("CLAUDE.md", "AGENTS.md"), ("CLAUDE.md", "docs/agents/topic.md")])


if __name__ == "__main__":
    unittest.main()
