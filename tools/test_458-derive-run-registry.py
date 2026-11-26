#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contrat de la copie d'exécution du registre EAV (#458, P23-30) - stdlib, hors ligne.

    python tools/test_458-derive-run-registry.py

POURQUOI CE FICHIER EXISTE. L'arbitrage ai-01 du 22/11 (option iii) interdit qu'une
URL de pré-release atteigne le site : l'apply du staging se fait sur une COPIE
d'exécution du registre officiel, jamais sur le registre lui-même. Le geste doit
pouvoir se rejouer partout, parce que le dépôt est public et que le jour J repose
sur le registre intact. Ce test éprouve l'outil ``458-derive-run-registry.py``
contre le registre committé, sans toucher à la base.

CE QUI EST ÉPINGLÉ :
  1. Le registre officiel committé passe : 673 corps -> 22 retirées -> 651
     conservées, et l'empreinte de sortie est EXACTEMENT celle mesurée sur le
     registre committé (``75a6028d…``, 1 186 808 octets depuis #2399 le 26/11 ;
     ``ef3eaa6a…``, 1 187 696 octets ; ``a130a663…``, 1 166 868 octets avant
     l'append P23-36 du 25/11 ; ``92bf1e06…``, 1 166 864 octets avant la correction
     typographique P24-32 du 24/11) - toute dérive du registre officiel rougit ici
     AVANT d'atteindre le staging.
  2. Les 22 clés retirées sont nommées : les 3 entités P&P absentes de prod
     (7 champs chacune) ET la ligne ``6A24AE92-…|Link`` de la carte existante -
     la 4e cellule porteuse d'URL de pré-release, celle que l'option (i) aurait
     réécrite sur les 8 langues.
  3. La carte 6A24AE92 garde ses AUTRES champs (LinkText traduit, etc.) : on retire
     le lien, pas la carte.
  4. Aucune chaîne ``-review`` ne survit dans la sortie.
  5. Contrôle inverse : une entrée qui n'est pas le registre officiel (un octet
     muté) est refusée par l'épinglage d'empreinte d'entrée - exit non nul, jamais
     une copie dérivée d'un faux registre.
"""
import csv
import hashlib
import io
import pathlib
import subprocess
import sys
import tempfile
import unittest

HERE = pathlib.Path(__file__).resolve().parent
TOOL = HERE / "458-derive-run-registry.py"
REGISTRY = HERE.parent / "docs" / "dnn-localization" / "eav-content.csv"

OFFICIAL_IN = "cf34842d691b8968"
EXPECTED_OUT_SHA = "75a6028d09e6e5cf0a8724573e6588cfaff324a1918f6dbafdc240efc90a2f99"
EXPECTED_OUT_BYTES = 1186808

MISSING_ENTITIES = {
    "494AB453-EC8B-4372-A6F7-A4A13A142153",
    "8BF57CB5-4AC3-4E21-902C-32700B3C66BC",
    "AA9754AF-86D8-4F5F-BEA9-C0DD6FCDBA4F",
}
EXISTING_CARD = "6A24AE92-8177-44EF-841B-6A7C41937597"


def run_tool(src, dst):
    return subprocess.run(
        [sys.executable, "-I", str(TOOL), str(src), str(dst)],
        capture_output=True, text=True, encoding="utf-8", errors="replace",
    )


class DeriveRunRegistryContract(unittest.TestCase):

    @classmethod
    def setUpClass(cls):
        cls._tmp = tempfile.TemporaryDirectory()
        cls.out = pathlib.Path(cls._tmp.name) / "run.csv"
        cls.proc = run_tool(REGISTRY, cls.out)
        cls.out_bytes = cls.out.read_bytes() if cls.out.exists() else b""

    @classmethod
    def tearDownClass(cls):
        cls._tmp.cleanup()

    def test_1_official_registry_passes_with_the_pinned_output(self):
        self.assertEqual(self.proc.returncode, 0, self.proc.stderr)
        self.assertIn(f"input sha256[:16] {OFFICIAL_IN}", self.proc.stdout)
        self.assertIn(f"output sha256 {EXPECTED_OUT_SHA}", self.proc.stdout)
        self.assertEqual(len(self.out_bytes), EXPECTED_OUT_BYTES)
        self.assertEqual(hashlib.sha256(self.out_bytes).hexdigest(), EXPECTED_OUT_SHA)

    def test_2_exactly_22_rows_dropped_and_651_kept(self):
        self.assertIn("removed 22", self.proc.stdout)
        self.assertIn("kept rows 651", self.proc.stdout)
        rows = list(csv.reader(io.StringIO(self.out_bytes.decode("utf-8"), newline="")))
        body = rows[1:]
        self.assertEqual(len(body), 651)
        keys = {r[rows[0].index("key")] for r in body}
        for guid in MISSING_ENTITIES:
            self.assertFalse(any(k.startswith(guid) for k in keys), guid)
        self.assertNotIn(f"{EXISTING_CARD}|Link", keys)

    def test_3_existing_card_keeps_its_other_fields(self):
        rows = list(csv.reader(io.StringIO(self.out_bytes.decode("utf-8"), newline="")))
        keys = {r[rows[0].index("key")] for r in rows[1:]}
        kept_fields = sorted(k.split("|")[1] for k in keys if k.startswith(EXISTING_CARD))
        self.assertIn("LinkText", kept_fields, "on retire le lien, pas la carte")
        self.assertNotIn("Link", kept_fields)

    def test_4_no_prerelease_marker_survives(self):
        self.assertNotIn("-review", self.out_bytes.decode("utf-8"))

    def test_5_inverse_control_mutated_input_is_refused(self):
        raw = REGISTRY.read_bytes()
        # Un octet muté dans une cellule conservée (ajout d'un espace en fin de
        # dernière ligne) : ce n'est plus le registre officiel -> STOP, exit non nul.
        mutated = raw[:-2] + b" \r\n" if raw.endswith(b"\r\n") else raw + b" "
        with tempfile.TemporaryDirectory() as td:
            src = pathlib.Path(td) / "mutated.csv"
            dst = pathlib.Path(td) / "out.csv"
            src.write_bytes(mutated)
            proc = run_tool(src, dst)
            self.assertNotEqual(proc.returncode, 0, "un registre muté doit être refusé")
            self.assertIn("STOP", proc.stderr)
            self.assertFalse(dst.exists(), "aucune sortie ne doit être écrite")


if __name__ == "__main__":
    unittest.main(verbosity=2)
