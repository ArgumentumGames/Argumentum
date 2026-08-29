#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#498 P2 apply — WRITE-SAFETY WITNESS (organe, ai-01 precondition, dispatch dba2s0).

The pre-hardening write block shipped in PR #1219 was:

    open(BACKUP, "wb").write(raw)
    open(PATH, "wb").write((b'\\xef\\xbb\\xbf' if bom else b'') + new_text.encode('utf-8'))

`open(path, "wb")` TRUNCATES AT OPEN, and Python evaluates the receiver `open(...)`
before the argument: if `new_text.encode('utf-8')` raises, the target is ALREADY
EMPTY, closed cleanly by the GC — non-zero exit, no trace, production corpus gone.
This exact motif zeroed MEMORY.md on 2026-08-27.

This suite is the organ the hardening PR must carry:
  1. test_old_motif_clobbers_target_on_encode_raise  — the RED witness, live: the
     verbatim pre-hardening motif DOES empty the target when encode raises. It was
     seen red against PR #1219's shipped lines and re-proves the failure mode on
     every run (a fix without this witness is a note, not a guard).
  2. test_hardened_path_intact_on_encode_raise       — the GUARD: the hardened block
     (encode BEFORE open, tmp + os.replace) leaves the target byte-intact.
  3. test_hardened_write_*                           — success path, mid-write failure.
  4. test_no_bare_open_write_motif_in_script         — static: the motif never
     returns to the shipped script.

    python docs/taxonomy/498-reconciliation-p2-apply-test.py   # 5 tests, ~0s
"""
import ast, os, re, sys, tempfile, unittest
from pathlib import Path

SCRIPT = Path(__file__).with_name("498-reconciliation-p2-apply.py")
SRC = SCRIPT.read_text(encoding="utf-8")

# The shipped hardened_write, extracted by AST — the test exercises the exact code
# in the script (no copy, no drift). The linear dry-run body is NOT executed.
def _load_hardened_write():
    tree = ast.parse(SRC)
    fn = next(n for n in tree.body
              if isinstance(n, ast.FunctionDef) and n.name == "hardened_write")
    ns = {"os": os}
    exec(compile(ast.Module(body=[fn], type_ignores=[]), str(SCRIPT), "exec"), ns)
    return ns["hardened_write"]

SENTINEL = b'\xef\xbb\xbfPK,Nom\r\n520,Attaque personnelle\r\n521,Pente savonneuse\r\n'
BAD_TEXT = "\ud800"          # lone surrogate: .encode('utf-8') raises UnicodeEncodeError


class WriteSafetyWitness(unittest.TestCase):

    def setUp(self):
        self._dir = tempfile.TemporaryDirectory()
        self.target = Path(self._dir.name) / "corpus-prod.csv"
        self.target.write_bytes(SENTINEL)

    def tearDown(self):
        self._dir.cleanup()

    # ── 1. TÉMOIN ROUGE (le motif d'origine vide bien la cible) ──────────────────
    def test_old_motif_clobbers_target_on_encode_raise(self):
        def old_write(path, text):
            # verbatim motif PR #1219 (498-reconciliation-p2-apply.py avant durcissement)
            open(path, "wb").write(text.encode("utf-8"))
        with self.assertRaises(UnicodeEncodeError):
            old_write(self.target, BAD_TEXT)          # open() ran BEFORE encode raised
        self.assertEqual(self.target.stat().st_size, 0,
                         "le motif d'origine DOIT démontrer le défaut : cible vidée")
        self.assertFalse(os.path.exists(str(self.target) + ".tmp"))

    # ── 2. GARDE : la voie durcie laisse la cible intacte ────────────────────────
    def test_hardened_path_intact_on_encode_raise(self):
        hardened_write = _load_hardened_write()
        bom = True
        new_text = BAD_TEXT
        with self.assertRaises(UnicodeEncodeError):
            payload = (b'\xef\xbb\xbf' if bom else b'') + new_text.encode('utf-8')
            hardened_write(self.target, payload)      # jamais atteint : encode d'abord
        self.assertEqual(self.target.read_bytes(), SENTINEL, "cible INTACTE")
        self.assertFalse(os.path.exists(str(self.target) + ".tmp"),
                         "aucun tmp résiduel — l'encodage a levé avant toute ouverture")

    # ── 3. succès : écriture atomique, byte-exacte, sans résidu ─────────────────
    def test_hardened_write_atomic_success(self):
        hardened_write = _load_hardened_write()
        payload = b'\xef\xbb\xbf' + "PK,Nom\r\n".encode("utf-8")
        hardened_write(self.target, payload)
        self.assertEqual(self.target.read_bytes(), payload)
        self.assertFalse(os.path.exists(str(self.target) + ".tmp"))

    # ── 3b. échec AU MILIEU de l'écriture tmp : cible intacte, replace non atteint
    def test_hardened_write_intact_on_midwrite_failure(self):
        hardened_write = _load_hardened_write()
        with self.assertRaises(TypeError):
            hardened_write(self.target, "pas-des-bytes")   # fh.write(str) lève dans le with
        self.assertEqual(self.target.read_bytes(), SENTINEL,
                         "os.replace jamais atteint ⇒ cible INTACTE")
        tmp = str(self.target) + ".tmp"
        if os.path.exists(tmp):
            os.remove(tmp)                              # résidu abandonné — nettoyage

    # ── 4. statique : le motif nu ne revient pas dans le script livré ───────────
    def test_no_bare_open_write_motif_in_script(self):
        bare = re.findall(r'^\s*open\([^)]*\)\.write', SRC, re.M)
        self.assertEqual(bare, [],
                         "motif open(...).write() sans with détecté — durcissement régressé")


if __name__ == "__main__":
    unittest.main(verbosity=2)
