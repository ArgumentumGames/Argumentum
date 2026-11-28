#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Épinglage de l'instantané `docs/ontology/v1.0.0/` — stdlib + `git`, hors ligne.

    python tools/test_133-owl-snapshot-pin.py

POURQUOI. L'instantané `v1.0.0/` porte la génération d'OWL **servie avant** la
régénération Vertus du 24/11 (P24-37). C'est une pièce : elle ne doit plus bouger.
Or rien ne la tenait — un octet changé dans un de ses deux fichiers passait sans
bruit, et la page servie aurait alors été « l'instantané » d'autre chose. Le §6 du
README du dossier l'annonce désormais (même `ontologyIRI` que le fichier canonique,
octets déjà servis) ; ce contrat est la moitié exécutable de cette phrase.

CE QUI EST ÉPINGLÉ
  1. les deux oids — l'identité du blob committé (`git rev-parse HEAD:<chemin>`),
     pas un hachage du fichier de travail, qui dépend des fins de ligne de la
     machine qui a fait le checkout
  2. l'ensemble exact des fichiers de l'instantané : deux, pas trois ; un fichier
     ajouté ou retiré sous `v1.0.0/` rougit ici, même s'il est cité à l'index
  3. l'`ontologyIRI` déclaré : l'instantané et son fichier canonique portent le
     même, et il est non vide — c'est la phrase du §6 qui devient un cas
  4-5. contrôles inverses : le même instrument, appliqué aux octets du blob avec un
     seul octet retourné, rend un oid DIFFÉRENT (il voit donc un octet), et rend
     exactement l'oid épinglé sur les octets intacts (il est aligné sur
     `rev-parse`, pas sur un autre calcul)

CONDITION D'EXÉCUTION. Offline, aucun artefact, aucun Target/, aucun réseau. Il
faut `git` et un `HEAD` (le checkout de `actions/checkout`, `fetch-depth` par
défaut, suffit : les blobs épinglés sont dans l'arbre du commit). Si git est
absent ou si `HEAD` ne se lit pas, les cas ÉCHOUENT bruyamment — jamais un saut :
un contrat aveugle ne doit pas être vert (leçon de #2412, [[git-archive-extract-blind-or-false-red]]).
Le rejeu vise l'arbre du checkout ; pour éprouver un autre arbre, `--repo <chemin>`.
"""
import os
import re
import subprocess
import sys
import unittest

HERE = os.path.dirname(os.path.abspath(__file__))
ROOT = os.path.dirname(HERE)

# Les deux blobs de l'instantané, mesurés le 2026-11-27 sur master `5ad16ebf`
# (`git rev-parse HEAD:docs/ontology/v1.0.0/<fichier>`).
PINS = {
    "docs/ontology/v1.0.0/argumentum.owl":
        "6ae370778a243ae67c40a1429169f1a9bd3ccb4f",
    "docs/ontology/v1.0.0/argumentum_virtues.owl":
        "a9f5b7ecea272325589b6124d1025081e271d72c",
}
# Chaque fichier de l'instantané regarde son canonique du même sujet.
CANONICAL = {
    "docs/ontology/v1.0.0/argumentum.owl": "docs/ontology/argumentum.owl",
    "docs/ontology/v1.0.0/argumentum_virtues.owl": "docs/ontology/argumentum_virtues.owl",
}
IRI = re.compile(r'ontologyIRI="([^"]*)"')


def git(repo, *args, **kwargs):
    """git dans `repo` ; lève au lieu de rendre un résultat vide silencieux."""
    proc = subprocess.run(["git", "-C", repo, *args], capture_output=True, **kwargs)
    if proc.returncode != 0:
        raise RuntimeError("git %s a échoué (rc=%d) : %s"
                           % (" ".join(args), proc.returncode,
                              proc.stderr.decode("utf-8", "replace").strip()))
    return proc.stdout


def blob_bytes(repo, path):
    return git(repo, "cat-file", "blob", "HEAD:" + path)


def hash_object(repo, data):
    return git(repo, "hash-object", "-t", "blob", "--stdin", input=data).strip().decode()


def tree_paths(repo, folder):
    out = git(repo, "ls-tree", "-r", "--name-only", "HEAD", folder).decode("utf-8")
    return sorted(line for line in out.splitlines() if line.strip())


class SnapshotPin(unittest.TestCase):
    repo = ROOT

    def test_1_the_two_snapshot_blobs_keep_their_pinned_oids(self):
        for path, oid in sorted(PINS.items()):
            measured = git(self.repo, "rev-parse", "HEAD:" + path).strip().decode()
            self.assertEqual(
                oid, measured,
                "%s : l'instantané a bougé (épinglé %s, lu %s). v1.0.0/ est une pièce "
                "figée : si le changement est voulu, c'est l'épinglage ET le §6 du "
                "README qui changent, dans la même revue." % (path, oid, measured))

    def test_2_the_snapshot_holds_exactly_these_two_files(self):
        measured = tree_paths(self.repo, "docs/ontology/v1.0.0")
        self.assertEqual(sorted(PINS), measured,
                         "l'instantané doit contenir exactement ses deux artefacts ; lu : %r"
                         % (measured,))

    def test_3_the_pin_sees_a_single_byte_change(self):
        for path, oid in sorted(PINS.items()):
            data = blob_bytes(self.repo, path)
            self.assertTrue(data, "%s : blob vide — l'instrument ne mesure rien" % path)
            self.assertEqual(oid, hash_object(self.repo, data),
                             "%s : hash-object ne rend pas l'oid épinglé — "
                             "les octets lus ne sont pas ceux que rev-parse a vus" % path)
            mutated = bytearray(data)
            mutated[len(mutated) // 2] ^= 0x01
            self.assertNotEqual(oid, hash_object(self.repo, bytes(mutated)),
                                "%s : un octet retourné laisse l'oid inchangé — "
                                "l'épinglage ne verrait pas une dérive" % path)

    def test_4_snapshot_and_canonical_declare_the_same_ontology_iri(self):
        for snapshot, canonical in sorted(CANONICAL.items()):
            iri_snapshot = self._ontology_iri(snapshot)
            iri_canonical = self._ontology_iri(canonical)
            self.assertTrue(iri_snapshot,
                            "%s : aucun ontologyIRI dans l'en-tête" % snapshot)
            self.assertEqual(iri_canonical, iri_snapshot,
                             "%s et %s doivent déclarer le même ontologyIRI (§6 du "
                             "README) ; lu : %r et %r"
                             % (snapshot, canonical, iri_snapshot, iri_canonical))

    def _ontology_iri(self, path):
        head = git(self.repo, "cat-file", "blob", "HEAD:" + path)[:4096]
        match = IRI.search(head.decode("utf-8", "replace"))
        return match.group(1) if match else ""


if __name__ == "__main__":
    if "--repo" in sys.argv:
        index = sys.argv.index("--repo")
        SnapshotPin.repo = os.path.abspath(sys.argv[index + 1])
        del sys.argv[index:index + 2]
    unittest.main(verbosity=2)
