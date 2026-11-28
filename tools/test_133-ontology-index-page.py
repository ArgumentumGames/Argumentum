#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contrat de la page lisible des ontologies — `docs/ontology/index.html` (#133, P24-46).

    python tools/test_133-ontology-index-page.py

CE QUE CE CONTRAT TIENT.

  1. La page committée est **à jour** : le script relu sur l'arbre la regenere
     octet pour octet. C'est la definition de « script rejoué ⇒ diff 0 » — sans
     elle, la page est un fichier ecrit une fois et jamais revu.
  2. Le controle `--check` **voit** une page perimee : un octet change dans un
     `.owl` (le sha256 change), une page modifiee a la main, un `.owl` ajoute au
     dossier — les trois font sortir 1, avec le diff.
  3. La page **suit le dossier** : un fichier `.owl` ajoute n'y est pas cite, donc
     la page est perimee — elle ne se resynchronise pas toute seule.
  4. La generation est **deterministe** : deux generations donnent les memes
     octets (aucune date, aucun horodatage dans la page).

CONDITION D'EXECUTION : std-lib seulement, hors ligne. Les cas 2 a 4 travaillent
sur un **jeu d'essai** minuscule (deux `.owl` de quelques lignes), construit dans
un dossier temporaire ; le cas 1 travaille sur l'arbre reel (lecture seule, ~6 Mo
lus). Aucun reseau, aucun `sleep`, aucun service demande.

LE CONTROLE INVERSE. Le script est charge par son chemin (`__file__`), donc le
contrat eprouve **le fichier du depot**, pas une copie de travail. Pour prouver
que les cas 2 a 4 discriminent, il faut rendre le controle aveugle : le mutant
`--check` qui rend toujours 0 doit les faire rougir (batterie de mutants,PR).
"""
import contextlib
import importlib.util
import io
import os
import shutil
import tempfile
import unittest

HERE = os.path.dirname(os.path.abspath(__file__))
ROOT = os.path.dirname(HERE)
TOOL = os.path.join(HERE, "133-ontology-index-page.py")

FIXTURE = """<?xml version="1.0" encoding="utf-8"?>
<Ontology xmlns:owl="http://www.w3.org/2002/07/owl#"
          xmlns:rdfs="http://www.w3.org/2000/01/rdf-schema#"
          xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#"
          ontologyIRI="https://example.invalid/{name}.owl#">
  <Annotation>
    <AnnotationProperty IRI="http://www.w3.org/2000/01/rdf-schema#comment" />
    <Literal xml:lang="en">A fixture ontology about {name}.</Literal>
  </Annotation>
  <Annotation>
    <AnnotationProperty IRI="http://www.w3.org/2002/07/owl#versionInfo" />
    <Literal>1.0.0</Literal>
  </Annotation>
  <Annotation>
    <AnnotationProperty IRI="http://purl.org/dc/terms/title" />
    <Literal xml:lang="en">Fixture {name}</Literal>
  </Annotation>
  <Annotation>
    <AnnotationProperty IRI="http://purl.org/dc/terms/creator" />
    <Literal>Fixture</Literal>
  </Annotation>
  <Annotation>
    <AnnotationProperty IRI="http://purl.org/dc/terms/license" />
    <IRI>https://creativecommons.org/licenses/by-sa/4.0/</IRI>
  </Annotation>
</Ontology>
"""


def load():
    """Le script du depot, charge par son chemin."""
    spec = importlib.util.spec_from_file_location("ontology_index_page", TOOL)
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    return mod


MOD = load()


def run(argv):
    """`main()` du script, sortie capturee — renvoie (code, stdout, stderr)."""
    out, err = io.StringIO(), io.StringIO()
    with contextlib.redirect_stdout(out), contextlib.redirect_stderr(err):
        code = MOD.main(argv)
    return code, out.getvalue(), err.getvalue()


class FixtureTree(object):
    """Un dossier jetable qui ressemble a `docs/ontology/`."""

    def __enter__(self):
        self.root = tempfile.mkdtemp(prefix="ontology-page-")
        self.folder = os.path.join(self.root, "docs", "ontology")
        os.makedirs(os.path.join(self.folder, "v1.0.0"))
        self.write("a.owl", FIXTURE.format(name="a"))
        self.write(os.path.join("v1.0.0", "a.owl"), FIXTURE.format(name="a"))
        return self

    def __exit__(self, *exc):
        shutil.rmtree(self.root, ignore_errors=True)
        return False

    def write(self, rel, content):
        path = os.path.join(self.folder, rel)
        with open(path, "w", encoding="utf-8", newline="\n") as fh:
            fh.write(content)
        return path

    def page(self):
        return os.path.join(self.folder, "index.html")

    def generate(self):
        return run(["--root", self.root])


class OntologyIndexPage(unittest.TestCase):
    """P24-46 — la page lisible des ontologies (#133)."""

    def test_the_committed_page_is_up_to_date(self):
        # Le DoD du grain : le script rejoue sur l'arbre, et la page sort identique.
        code, out, err = run(["--check"])
        self.assertEqual(code, 0, err[-800:])
        self.assertIn("identique", out)

    def test_a_fixture_tree_generates_a_page_that_checks(self):
        with FixtureTree() as tree:
            code, out, err = tree.generate()
            self.assertEqual(code, 0, err)
            self.assertTrue(os.path.exists(tree.page()))
            code, out, err = run(["--root", tree.root, "--check"])
            self.assertEqual(code, 0, err[-800:])
            self.assertIn("identique", out)

    def test_a_changed_artefact_makes_the_page_stale(self):
        # Un octet change dans le `.owl` : son sha256 change, la page ne decrit
        # plus les octets du dossier. Le controle doit le voir.
        with FixtureTree() as tree:
            tree.generate()
            path = os.path.join(tree.folder, "a.owl")
            with open(path, encoding="utf-8") as fh:
                text = fh.read()
            with open(path, "w", encoding="utf-8", newline="") as fh:
                fh.write(text.replace("1.0.0", "1.0.1"))
            code, out, err = run(["--root", tree.root, "--check"])
            self.assertEqual(code, 1, "la page perimee doit sortir 1")
            self.assertIn("sha-256", err.lower())
            self.assertNotIn("identique", out)

    def test_a_hand_edited_page_is_stale(self):
        with FixtureTree() as tree:
            tree.generate()
            with open(tree.page(), encoding="utf-8") as fh:
                text = fh.read()
            with open(tree.page(), "w", encoding="utf-8", newline="") as fh:
                fh.write(text.replace("Fixture a", "Fixture A"))
            code, out, err = run(["--root", tree.root, "--check"])
            self.assertEqual(code, 1)
            self.assertIn("perimee", err)

    def test_an_added_artefact_is_stale(self):
        # La page suit le dossier : un `.owl` ajoute n'y est pas cite, donc la
        # page est perimee. Elle ne se resynchronise pas toute seule.
        with FixtureTree() as tree:
            tree.generate()
            tree.write("b.owl", FIXTURE.format(name="b"))
            code, out, err = run(["--root", tree.root, "--check"])
            self.assertEqual(code, 1)
            self.assertIn("perimee", err)

    def test_the_generation_is_deterministic(self):
        # Deux generations, les memes octets : pas de date, pas d'horodatage.
        with FixtureTree() as tree:
            tree.generate()
            with open(tree.page(), "rb") as fh:
                first = fh.read()
            tree.generate()
            with open(tree.page(), "rb") as fh:
                second = fh.read()
            self.assertEqual(first, second)

    def test_the_page_carries_the_artefacts_metadata(self):
        # Ce que la page AFFICHE est lu dans les fichiers : l'IRI, la version, le
        # titre, l'auteur, la licence, la taille et le sha256 ne sont pas recopies
        # d'un document.
        with FixtureTree() as tree:
            tree.generate()
            with open(tree.page(), encoding="utf-8") as fh:
                page = fh.read()
            self.assertIn("https://example.invalid/a.owl#", page)
            self.assertIn("1.0.0", page)
            self.assertIn("creativecommons.org/licenses/by-sa/4.0/", page)
            self.assertIn("Fixture", page)
            self.assertIn("v1.0.0/a.owl", page)
            self.assertIn("SHA-256", page)

    def test_an_annotation_assertion_shows_its_object(self):
        # Deux formes coexistent dans les `.owl` reels. Sous la forme
        # `AnnotationAssertion`, l'annotation porte sur un SUJET : la valeur est
        # l'objet (le dernier element), pas le sujet (le premier). Mesure : la
        # licence sortait l'IRI de l'ontologie au lieu de son URL.
        licence = ('    <AnnotationProperty IRI="http://purl.org/dc/terms/license" />\n'
                   "    <IRI>https://creativecommons.org/licenses/by-sa/4.0/</IRI>\n"
                   "  </Annotation>\n")
        assertion = ("  <AnnotationAssertion>\n"
                     '    <AnnotationProperty IRI="http://purl.org/dc/terms/license" />\n'
                     "    <IRI>https://example.invalid/a.owl#</IRI>\n"
                     "    <IRI>https://creativecommons.org/licenses/by-sa/4.0/</IRI>\n"
                     "  </AnnotationAssertion>\n")
        with FixtureTree() as tree:
            path = os.path.join(tree.folder, "a.owl")
            with open(path, encoding="utf-8") as fh:
                text = fh.read()
            with open(path, "w", encoding="utf-8", newline="") as fh:
                fh.write(text.replace(licence, assertion))
            tree.generate()
            with open(tree.page(), encoding="utf-8") as fh:
                page = fh.read()
            # La partie « artefacts courants » seulement : l'instantane garde la
            # forme `Annotation`, il ne prouverait rien sur la forme lue.
            courant = page.split("<h2>Versioned snapshot")[0]
            self.assertIn("<dt>Licence</dt><dd><code>https://creativecommons.org/"
                          "licenses/by-sa/4.0/</code></dd>", courant)

    def test_an_absent_annotation_is_not_invented(self):
        # Une annotation absente de l'en-tete ne donne pas une ligne vide sur la
        # page : la ligne n'apparait pas. Sans ce cas, l'ajout d'une ligne vide
        # pourrait passer inaperçu.
        licence = ('    <AnnotationProperty IRI="http://purl.org/dc/terms/license" />\n'
                   "    <IRI>https://creativecommons.org/licenses/by-sa/4.0/</IRI>\n"
                   "  </Annotation>\n")
        with FixtureTree() as tree:
            for rel in ("a.owl", os.path.join("v1.0.0", "a.owl")):
                path = os.path.join(tree.folder, rel)
                with open(path, encoding="utf-8") as fh:
                    text = fh.read()
                with open(path, "w", encoding="utf-8", newline="") as fh:
                    fh.write(text.replace(licence, ""))
            tree.generate()
            with open(tree.page(), encoding="utf-8") as fh:
                page = fh.read()
            self.assertNotIn("creativecommons.org/licenses", page)
            self.assertNotIn("<dt>Licence</dt>", page)


if __name__ == "__main__":
    unittest.main(verbosity=2)
