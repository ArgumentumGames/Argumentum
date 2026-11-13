#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contrat de tools/issue-closing-block.py — std-lib, offline.

    python tools/test_issue-closing-block.py

Le `gh` de l'outil est une fonction injectée : ici il est simulé. Aucun appel réseau, aucune
écriture sur GitHub.

CE QUI EST ÉPINGLÉ
   1-3. aller-retour : `split(compose(b, o))` rend `o` octet pour octet (CRLF, texte vide,
        texte qui commence par un trait ou des lignes vides compris) ; poser deux fois le
        même bloc ne change rien ; un corps CRLF (édité dans le navigateur) ne bouge pas
   4-7. chaque refus, par son code : marqueurs seuls, dupliqués, dans le désordre, texte
        avant le bloc, trait absent, bloc vide, marqueur dans le bloc ou dans le texte
        d'origine, mention `@` hors code (le code en est exempt), corps trop long
   8-14. `set` : dry-run sans écriture ; --apply sans --expect-login refusé avant tout appel ;
        compte différent, corps changé entre les lectures, relecture différente : refusés ;
        relecture en CRLF acceptée ; écriture par --body-file ; texte d'origine altéré refusé
   15-17. `list` en un seul appel, un corps malformé => sortie 1 ; liste tronquée => 2 ; `show`
   18. un `gh` en échec, ou une réponse vide, sort en 2 avec son stderr — jamais un vide
"""
import importlib.util
import io
import json
import os
import shutil
import tempfile
import unittest

HERE = os.path.dirname(os.path.abspath(__file__))
_spec = importlib.util.spec_from_file_location("issue_closing_block", os.path.join(HERE, "issue-closing-block.py"))
tool = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(tool)

BEGIN, END = tool.BEGIN, tool.END
LOGIN = "compte-attendu"
BLOCK = ("## État et chemin de clôture\n\n**Fait** — 165 cartes sur 175.\n"
         "**Reste** — 10 vides à justifier.\n**Prochaine étape** — po-2024.")
ORIGINAL = "> **Ce qui manque pour fermer**\n\nTexte d'origine, écrit à la main.\n\n- [ ] un item\n"
ORIGINALS = ["", "x", ORIGINAL, "\n\ncommence par deux lignes vides", "---\ncommence par un trait\n",
             "fin sans retour", "a\r\nb\r\n\r\n- [ ] c\r\n", "mixte\r\nlf\nfin\n", "  \nespaces\n\n\n",
             "é — « guillemets » 🙂\n"]


class FakeGh(object):
    """`gh` simulé : un corps par issue (une liste = lectures successives), un compte, une liste."""

    def __init__(self, bodies=None, login=LOGIN, listing=None, fail=None, store=None):
        self.bodies = dict((n, list(v) if isinstance(v, list) else [v]) for n, v in (bodies or {}).items())
        self.login, self.listing, self.fail = login, listing, fail
        self.store = store or (lambda sent: sent)
        self.calls, self.sent = [], []

    def __call__(self, args):
        args = list(args)
        self.calls.append(args)
        if self.fail is not None and args[:len(self.fail)] == self.fail:
            return 1, "", "HTTP 502: Bad Gateway (https://api.github.com/graphql)"
        if args[:2] == ["issue", "view"]:
            reads = self.bodies[int(args[2])]
            return 0, json.dumps({"body": reads.pop(0) if len(reads) > 1 else reads[0]}), ""
        if args[:2] == ["issue", "edit"]:
            if "--body-file" in args:
                with io.open(args[args.index("--body-file") + 1], encoding="utf-8", newline="") as handle:
                    sent = handle.read()
            else:                                       # `--body <x>` : gh poste x tel quel
                sent = args[args.index("--body") + 1]
            self.sent.append(sent)
            self.bodies[int(args[2])] = [self.store(sent)]
            return 0, "https://github.com/o/r/issues/%s\n" % args[2], ""
        if args[:2] == ["api", "user"]:
            return 0, self.login + "\n", ""
        if args[:2] == ["issue", "list"]:
            return 0, json.dumps(self.listing), ""
        raise AssertionError("appel gh inattendu : %r" % args)

    def edits(self):
        return [call for call in self.calls if call[:2] == ["issue", "edit"]]


def run(argv, gh):
    out, err = io.StringIO(), io.StringIO()
    return tool.main(argv, gh=gh, out=out, err=err), out.getvalue(), err.getvalue()


class Shape(unittest.TestCase):
    def refused(self, code, call, *args):
        with self.assertRaises(tool.Refusal) as caught:
            call(*args)
        self.assertEqual(caught.exception.code, code, str(caught.exception))

    def test_01_round_trip_is_byte_identical(self):
        for original in ORIGINALS:
            block, back = tool.split(tool.compose(BLOCK, original))
            self.assertEqual(back, original, repr(original))
            self.assertEqual(tool.eol(block), BLOCK, repr(original))
        self.assertEqual(tool.split(ORIGINAL), (None, ORIGINAL))
        # La forme posée le 13/11 : marqueurs, ligne vide, trait, ligne vide.
        self.assertEqual(tool.compose(BLOCK, ORIGINAL),
                         BEGIN + "\n\n" + BLOCK + "\n\n" + END + "\n\n---\n\n" + ORIGINAL)

    def test_02_the_same_block_twice_changes_nothing(self):
        for original in ORIGINALS:
            body = tool.compose(BLOCK, original)
            self.assertEqual(tool.compose(BLOCK, tool.split(body)[1]), body, repr(original))
            self.assertEqual(tool.compose(*tool.split(body)), body, repr(original))
        # Un bloc relu d'un fichier CRLF, avec BOM retiré et blancs autour, donne le même corps.
        self.assertEqual(tool.compose("\r\n  " + BLOCK.replace("\n", "\r\n") + "\r\n\r\n", ORIGINAL),
                         tool.compose(BLOCK, ORIGINAL))
        # Un nouveau bloc remplace l'ancien, le texte d'origine reste.
        body = tool.compose("ancien bloc", ORIGINAL)
        self.assertEqual(tool.split(tool.compose(BLOCK, tool.split(body)[1])), (BLOCK, ORIGINAL))

    def test_03_a_crlf_body_from_the_browser_is_kept(self):
        crlf = tool.compose(BLOCK, ORIGINAL).replace("\n", "\r\n")
        block, original = tool.split(crlf)
        self.assertEqual(original, ORIGINAL.replace("\n", "\r\n"))
        self.assertEqual(tool.eol(block), BLOCK)
        self.assertEqual(tool.compose(BLOCK, original), crlf)
        # Un corps sans bloc, en CRLF : tout est texte d'origine, octet pour octet.
        self.assertEqual(tool.split(ORIGINAL.replace("\n", "\r\n")), (None, ORIGINAL.replace("\n", "\r\n")))

    def test_04_malformed_markers_are_refused(self):
        body = tool.compose(BLOCK, ORIGINAL)
        self.refused("unpaired-marker", tool.split, body.replace(END, ""))
        self.refused("unpaired-marker", tool.split, body.replace(BEGIN, ""))
        self.refused("unpaired-marker", tool.split, ORIGINAL + "\n" + BEGIN + "\n")
        self.refused("duplicate-marker", tool.split, body + "\n" + BEGIN + "\n" + END + "\n")
        self.refused("duplicate-marker", tool.split, body + END)
        self.refused("marker-order", tool.split, END + "\n\nbloc\n\n" + BEGIN + "\n\n---\n\n" + ORIGINAL)
        self.refused("text-before-block", tool.split, "Ce texte serait perdu.\n\n" + body)
        self.assertEqual(tool.split("\n \n" + body), (BLOCK, ORIGINAL))      # des blancs, pas du texte
        self.refused("no-rule", tool.split, body.replace("\n---\n", "\n"))
        self.refused("no-rule", tool.split, body.replace(END + "\n\n", END + "\n\nperdu\n\n"))
        self.refused("no-rule", tool.split, body.replace(END + "\n\n---", END + "---"))

    def test_05_a_bad_block_is_refused(self):
        for empty in ("", "   ", "\r\n\r\n\n \t"):
            self.refused("empty-block", tool.compose, empty, ORIGINAL)
        self.refused("marker-in-block", tool.compose, BLOCK + "\n" + END, ORIGINAL)
        self.refused("marker-in-block", tool.compose, BEGIN + "\n" + BLOCK, ORIGINAL)
        self.refused("marker-in-original", tool.compose, BLOCK, ORIGINAL + BEGIN)
        self.refused("marker-in-original", tool.compose, BLOCK, tool.compose(BLOCK, ORIGINAL))

    def test_06_a_mention_is_refused_but_code_is_exempt(self):
        # Des logins fictifs : ce fichier n'est posté nulle part, un login réel n'y a rien à faire.
        for text in ("revue par @relecteur-fictif", "(@lane-fictive)", "voir @org/equipe", "@a fin", "`code` @x"):
            self.refused("mention", tool.compose, BLOCK + "\n" + text, ORIGINAL)
        for text in ("revue par `@relecteur-fictif`", "``a ` @b``", "```\n@fictif\n```", "~~~text\n@x\n~~~",
                     "contact@example.invalid", "icone@2x.png", "@-mention", "un @ seul"):
            self.assertTrue(tool.compose(BLOCK + "\n" + text, ORIGINAL).startswith(BEGIN), text)

    def test_07_a_body_over_the_github_limit_is_refused(self):
        frame = len(tool.compose("b", ""))
        self.assertEqual(len(tool.compose("b", "x" * (tool.LIMIT - frame))), tool.LIMIT)
        self.refused("too-long", tool.compose, "b", "x" * (tool.LIMIT - frame + 1))


class SetCommand(unittest.TestCase):
    def setUp(self):
        self.folder = tempfile.mkdtemp(prefix="issue-closing-block-test-")
        self.block_file = os.path.join(self.folder, "block.md")
        with io.open(self.block_file, "w", encoding="utf-8", newline="") as handle:
            handle.write(BLOCK + "\n")
        self.before = tool.compose("**Fait** — rien encore.", ORIGINAL)

    def tearDown(self):
        shutil.rmtree(self.folder, ignore_errors=True)

    def apply(self, gh, *extra):
        return run(["set", "7", "--block-file", self.block_file, "--apply"] + list(extra), gh)

    def test_08_dry_run_writes_nothing_and_shows_the_head_only(self):
        gh = FakeGh({7: self.before})
        code, out, err = run(["set", "7", "--block-file", self.block_file], gh)
        self.assertEqual(code, 0, err)
        self.assertEqual(gh.edits(), [])
        self.assertIn("-**Fait** — rien encore.", out)
        self.assertIn("+**Reste** — 10 vides à justifier.", out)
        self.assertNotIn("Texte d'origine, écrit à la main", out)           # la tête seule
        size = len(ORIGINAL.encode("utf-8"))
        self.assertIn("%d octets avant, %d octets après" % (size, size), out)
        self.assertIn("dry-run", out)
        # Premier bloc d'une issue : un diff du corps entier montrerait le début du texte
        # d'origine en contexte ; celui de la tête seule n'en montre rien.
        code, out, err = run(["set", "7", "--block-file", self.block_file], FakeGh({7: ORIGINAL}))
        self.assertEqual(code, 0, err)
        self.assertIn("+" + BEGIN, out)
        self.assertNotIn("Ce qui manque pour fermer", out)

    def test_09_apply_without_expected_login_is_refused_before_any_call(self):
        gh = FakeGh({7: self.before})
        code, _, err = self.apply(gh)
        self.assertEqual(code, 2)
        self.assertIn("--expect-login", err)
        self.assertEqual(gh.calls, [])

    def test_10_another_account_is_refused(self):
        gh = FakeGh({7: self.before}, login="autre-compte")
        code, _, err = self.apply(gh, "--expect-login", LOGIN)
        self.assertEqual(code, 2)
        self.assertIn("(identity)", err)
        self.assertEqual(gh.edits(), [])

    def test_11_a_body_changed_since_the_first_read_is_refused(self):
        gh = FakeGh({7: [self.before, self.before + "\najout d'un humain\n"]})
        code, _, err = self.apply(gh, "--expect-login", LOGIN)
        self.assertEqual(code, 2)
        self.assertIn("(body-changed)", err)
        self.assertEqual(gh.edits(), [])

    def test_12_apply_writes_by_file_and_reads_back(self):
        gh = FakeGh({7: self.before})
        code, out, err = self.apply(gh, "--expect-login", LOGIN)
        self.assertEqual(code, 0, err)
        self.assertEqual(len(gh.edits()), 1)
        self.assertIn("--body-file", gh.edits()[0])
        self.assertNotIn("--body", gh.edits()[0])
        self.assertEqual(gh.sent, [tool.compose(BLOCK, ORIGINAL)])
        self.assertEqual(tool.split(gh.bodies[7][0]), (BLOCK, ORIGINAL))
        self.assertIn("relu identique", out)
        # Une seconde passe avec le même bloc : rien à écrire.
        code, out, err = self.apply(gh, "--expect-login", LOGIN)
        self.assertEqual(code, 0, err)
        self.assertIn("inchangé", out)
        self.assertEqual(len(gh.edits()), 1)

    def test_13_read_back_is_compared_after_line_endings_only(self):
        gh = FakeGh({7: self.before}, store=lambda sent: sent.replace("\n", "\r\n"))
        code, out, err = self.apply(gh, "--expect-login", LOGIN)
        self.assertEqual(code, 0, err)
        self.assertIn("normalisation des fins de ligne", out)
        gh = FakeGh({7: self.before}, store=lambda sent: sent.replace("vides", "vide"))
        code, _, err = self.apply(gh, "--expect-login", LOGIN)
        self.assertEqual(code, 2)
        self.assertIn("(read-back)", err)

    def test_14_an_altered_original_is_refused(self):
        real = tool.compose
        tool.compose = lambda block, original: real(block, original[:-1])
        try:
            gh = FakeGh({7: self.before})
            code, _, err = self.apply(gh, "--expect-login", LOGIN)
        finally:
            tool.compose = real
        self.assertEqual(code, 2)
        self.assertIn("(original-altered)", err)
        self.assertEqual(gh.edits(), [])
        # Un bloc refusé par compose arrête aussi tout, avant l'écriture.
        with io.open(self.block_file, "w", encoding="utf-8") as handle:
            handle.write("revue par @relecteur-fictif\n")
        gh = FakeGh({7: self.before})
        code, _, err = self.apply(gh, "--expect-login", LOGIN)
        self.assertEqual((code, gh.edits()), (2, []))
        self.assertIn("(mention)", err)


class ListAndShow(unittest.TestCase):
    LISTING = [{"number": 3, "title": "sans bloc", "body": ORIGINAL},
               {"number": 1, "title": "avec bloc", "body": tool.compose(BLOCK, ORIGINAL)},
               {"number": 2, "title": "abîmé", "body": tool.compose(BLOCK, ORIGINAL).replace(END, "")}]

    def test_15_list_is_one_bulk_call_and_a_malformed_body_exits_1(self):
        gh = FakeGh(listing=self.LISTING)
        code, out, err = run(["list"], gh)
        self.assertEqual(code, 1, err)
        self.assertEqual(len(gh.calls), 1)
        self.assertIn("3 issues ouvertes : 1 avec bloc, 1 sans, 1 malformées", out)
        self.assertRegex(out, r"#2 +malformé .*un seul des deux marqueurs")
        code, out, _ = run(["list"], FakeGh(listing=self.LISTING[:2]))
        self.assertEqual(code, 0)
        self.assertIn("2 issues ouvertes : 1 avec bloc, 1 sans, 0 malformées", out)

    def test_16_a_list_at_the_limit_may_be_truncated(self):
        saved = tool.LIST_LIMIT
        tool.LIST_LIMIT = 3
        try:
            code, _, err = run(["list"], FakeGh(listing=self.LISTING))
        finally:
            tool.LIST_LIMIT = saved
        self.assertEqual(code, 2)
        self.assertIn("tronquée", err)

    def test_17_show_prints_the_block_or_exits_1(self):
        code, out, _ = run(["show", "1"], FakeGh({1: tool.compose(BLOCK, ORIGINAL).replace("\n", "\r\n")}))
        self.assertEqual((code, out), (0, BLOCK + "\n"))
        code, out, err = run(["show", "3"], FakeGh({3: ORIGINAL}))
        self.assertEqual((code, out), (1, ""))
        self.assertIn("aucun bloc", err)
        code, _, err = run(["show", "2"], FakeGh({2: self.LISTING[2]["body"]}))
        self.assertEqual(code, 2)
        self.assertIn("(unpaired-marker)", err)

    def test_18_a_gh_failure_exits_2_and_never_reads_as_empty(self):
        for argv, fail in ((["list"], ["issue", "list"]), (["show", "1"], ["issue", "view"])):
            code, out, err = run(argv, FakeGh({1: ORIGINAL}, listing=[], fail=fail))
            self.assertEqual(code, 2, argv)
            self.assertIn("HTTP 502", err)
            self.assertNotIn("0 issues", out)
        for answer in ("", "<html>rate limited</html>"):
            code, _, err = run(["list"], lambda args, answer=answer: (0, answer, ""))
            self.assertEqual(code, 2, repr(answer))
            self.assertIn("illisible", err)
        code, _, err = run(["show", "1"], lambda args: (0, json.dumps({"title": "x"}), ""))
        self.assertEqual(code, 2)
        self.assertIn("pas de champ body", err)
        code, _, err = run(["list"], FakeGh(listing=[{"number": 4, "title": "x", "body": None}]))
        self.assertEqual(code, 2)
        self.assertIn("#4 : pas de champ body", err)
        folder = tempfile.mkdtemp(prefix="issue-closing-block-test-")
        try:
            path = os.path.join(folder, "b.md")
            with io.open(path, "w", encoding="utf-8") as handle:
                handle.write(BLOCK)
            gh = FakeGh({7: ORIGINAL}, fail=["api", "user"])
            code, _, err = run(["set", "7", "--block-file", path, "--apply", "--expect-login", LOGIN], gh)
        finally:
            shutil.rmtree(folder, ignore_errors=True)
        self.assertEqual((code, gh.edits()), (2, []))
        self.assertIn("HTTP 502", err)


if __name__ == "__main__":
    unittest.main()
