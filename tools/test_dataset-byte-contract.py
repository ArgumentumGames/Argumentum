#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contrat d'octets des 5 CSV de données vivants — stdlib seulement, hors ligne, aucun artefact.

    python tools/test_dataset-byte-contract.py                 # l'arbre de travail
    python tools/test_dataset-byte-contract.py --ref <ref>     # les blobs d'une ref git
    python tools/test_dataset-byte-contract.py --report        # la mesure, sans verdict

POURQUOI CE FICHIER EXISTE. Les 5 CSV qui rendent une carte ont une forme d'octets que
rien n'épinglait : BOM UTF-8 (sauf Scenarii), fin d'enregistrement CRLF, et des LF nus
À L'INTÉRIEUR de cellules entre guillemets. Chaque outil d'écriture du dépôt porte sa
propre discipline « byte-exact » ; un outil qui ne la porte pas — un éditeur, un
`csv.writer` au dialecte par défaut, un `git add` sur une machine en `core.autocrlf=true`
quand l'entrée est absente de l'index — réécrit TOUTES les fins de ligne. Le diff passe
alors d'une ligne à la taille du fichier, la revue cellule par cellule devient
impossible, et le vrai changement est invisible dedans.

Ce contrat rend le défaut ROUGE en CI au lieu de le laisser à la vigilance du relecteur.
Il tourne sur un checkout nu (ubuntu, aucun `Target/`, aucun réseau) : c'est la condition
pour qu'il protège aussi la PR d'un agent qui ne voit que git.

CE QUI EST ÉPINGLÉ, par fichier (table `DATASETS`) :
  1. UTF-8 strict ;
  2. BOM présent ou absent, tel que déclaré ;
  3. HORS guillemets, chaque enregistrement finit par CRLF — ni LF nu, ni CR nu ;
  4. le fichier finit par un terminateur d'enregistrement ;
  5. ENTRE guillemets, un saut de ligne est un LF nu, jamais un CRLF (ce qu'un
     `unix2dos` ou un tableur produirait) ;
  6. aucun enregistrement n'a PLUS de champs que l'en-tête, et aucun n'en a MOINS —
     sauf le budget déclaré de `Argumentum Rules - Cards.csv` : 10 enregistrements y
     omettent le dernier champ `variant_class` (posé par #1421 sur les seules couvertures
     de variante, lu en `.Optional()`). Le budget est une ÉGALITÉ : il ne peut ni monter
     ni descendre sans que la table `DATASETS` bouge dans la même PR ;
  7. l'automate de guillemets et le module `csv` comptent le MÊME nombre
     d'enregistrements — sinon l'instrument lui-même se trompe sur ce fichier.

CE QUI N'EST PAS ÉPINGLÉ, exprès : le nombre de lignes, le nombre de colonnes, le contenu.
Ils changent légitimement ; les épingler ferait de chaque grain de données une PR rouge.

CONTRÔLES INVERSES (classe `InverseControls`) : chaque règle est éprouvée par une mutation
des OCTETS RÉELS du dépôt, et le test exige que la violation attendue — et nommée — sorte.
Un contrat qui ne voit pas le défaut qu'il annonce est un vert vide.

⚠️ `--ref` lit les blobs par `git show` : c'est la seule lecture qui dise ce qui est
COMMITTÉ. L'arbre de travail d'une machine en `core.autocrlf=true` peut différer du blob
pour un fichier sans règle de fin de ligne ; `.gitattributes` marque ces 5 fichiers
`-text` pour que ce ne soit plus le cas.
"""
import csv
import io
import os
import subprocess
import sys
import unittest

HERE = os.path.dirname(os.path.abspath(__file__))
REPO = os.path.dirname(HERE)
BOM = b"\xef\xbb\xbf"

# (chemin relatif au dépôt, BOM attendu, nombre EXACT d'enregistrements plus courts que l'en-tête)
DATASETS = [
    ("Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv", True, 0),
    ("Cards/Fallacies/Argumentum Virtues - Taxonomy.csv", True, 0),
    ("Cards/Rules/Argumentum Rules - Cards.csv", True, 10),
    ("Cards/Rules/Argumentum Rules - Cards Print and Play.csv", True, 0),
    ("Cards/Scenarii/Argumentum Scenarii - Cards.csv", False, 0),
]

REF = None  # posé par --ref ; None = arbre de travail


def read_bytes(rel, ref=None):
    """Octets du fichier : arbre de travail, ou blob de `ref` (aucun filtre git)."""
    if ref is None:
        with open(os.path.join(REPO, *rel.split("/")), "rb") as handle:
            return handle.read()
    done = subprocess.run(["git", "-C", REPO, "show", "%s:%s" % (ref, rel)],
                          capture_output=True)
    if done.returncode != 0:
        raise RuntimeError("git show %s:%s a échoué : %s"
                           % (ref, rel, done.stderr.decode("utf-8", "replace").strip()))
    return done.stdout


def scan(data):
    """Mesure la forme d'octets d'un CSV. Ne juge pas : rend des compteurs."""
    has_bom = data.startswith(BOM)
    body = data[len(BOM):] if has_bom else data
    m = {
        "bom": has_bom,
        "utf8": True,
        "records": 0,           # terminateurs vus hors guillemets
        "row_crlf": 0,
        "row_bare_lf": 0,
        "row_bare_cr": 0,
        "cell_lf": 0,           # LF nus entre guillemets
        "cell_crlf": 0,         # CRLF entre guillemets
        "cell_bare_cr": 0,
        "ends_with_terminator": body.endswith(b"\r\n") or body.endswith(b"\n"),
        "unclosed_quote": False,
        "first_bad_record": None,   # 1er enregistrement (base 1) fini par autre chose que CRLF
    }
    try:
        text = body.decode("utf-8")
    except UnicodeDecodeError:
        m["utf8"] = False
        text = body.decode("utf-8", "replace")

    in_quotes = False
    i, n = 0, len(body)
    while i < n:
        c = body[i]
        nxt = body[i + 1] if i + 1 < n else None
        if in_quotes:
            if c == 0x22:
                if nxt == 0x22:
                    i += 2
                    continue
                in_quotes = False
            elif c == 0x0D:
                if nxt == 0x0A:
                    m["cell_crlf"] += 1
                    i += 2
                    continue
                m["cell_bare_cr"] += 1
            elif c == 0x0A:
                m["cell_lf"] += 1
        else:
            if c == 0x22:
                in_quotes = True
            elif c == 0x0D:
                if nxt == 0x0A:
                    m["records"] += 1
                    m["row_crlf"] += 1
                    i += 2
                    continue
                m["records"] += 1
                m["row_bare_cr"] += 1
                if m["first_bad_record"] is None:
                    m["first_bad_record"] = m["records"]
            elif c == 0x0A:
                m["records"] += 1
                m["row_bare_lf"] += 1
                if m["first_bad_record"] is None:
                    m["first_bad_record"] = m["records"]
        i += 1
    m["unclosed_quote"] = in_quotes
    if n and not m["ends_with_terminator"]:
        m["records"] += 1       # dernier enregistrement sans terminateur : il compte quand même

    rows = list(csv.reader(io.StringIO(text, newline="")))
    m["csv_records"] = len(rows)
    m["header_fields"] = len(rows[0]) if rows else 0
    short = [(k + 1, len(r)) for k, r in enumerate(rows) if len(r) < m["header_fields"]]
    longer = [(k + 1, len(r)) for k, r in enumerate(rows) if len(r) > m["header_fields"]]
    m["short"], m["first_short"] = len(short), (short[0] if short else None)
    m["long"], m["first_long"] = len(longer), (longer[0] if longer else None)
    return m


def violations(m, expect_bom, short_budget=0):
    """Liste NOMMÉE des écarts au contrat. Vide = conforme."""
    out = []
    if not m["utf8"]:
        out.append("UTF8: le fichier n'est pas de l'UTF-8 strict")
    if m["bom"] != expect_bom:
        out.append("BOM: %s, attendu %s"
                   % ("présent" if m["bom"] else "absent",
                      "présent" if expect_bom else "absent"))
    if m["row_bare_lf"]:
        out.append("ROW_LF: %d enregistrement(s) finissent par un LF nu au lieu de CRLF "
                   "(1er : enregistrement %s)" % (m["row_bare_lf"], m["first_bad_record"]))
    if m["row_bare_cr"]:
        out.append("ROW_CR: %d enregistrement(s) finissent par un CR nu" % m["row_bare_cr"])
    if not m["ends_with_terminator"]:
        out.append("EOF: le fichier ne finit pas par un terminateur d'enregistrement")
    if m["cell_crlf"]:
        out.append("CELL_CRLF: %d saut(s) de ligne CRLF entre guillemets (attendu : LF nu)"
                   % m["cell_crlf"])
    if m["cell_bare_cr"]:
        out.append("CELL_CR: %d CR nu(s) entre guillemets" % m["cell_bare_cr"])
    if m["unclosed_quote"]:
        out.append("QUOTE: un guillemet ouvert n'est jamais refermé")
    if m["long"]:
        out.append("FIELDS_LONG: %d enregistrement(s) ont plus de champs que les %d de "
                   "l'en-tête (1er : enregistrement %s, %s champs)"
                   % (m["long"], m["header_fields"], m["first_long"][0], m["first_long"][1]))
    if m["short"] != short_budget:
        hint = ("abaisser le budget de DATASETS dans la même PR" if m["short"] < short_budget
                else "1er : enregistrement %s, %s champs" % m["first_short"])
        out.append("FIELDS_SHORT: %d enregistrement(s) ont moins de champs que les %d de "
                   "l'en-tête, budget déclaré %d (%s)"
                   % (m["short"], m["header_fields"], short_budget, hint))
    if m["records"] != m["csv_records"]:
        out.append("INSTRUMENT: l'automate compte %d enregistrements, le module csv %d"
                   % (m["records"], m["csv_records"]))
    return out


def kinds(found):
    return sorted(v.split(":", 1)[0] for v in found)


class DatasetByteContract(unittest.TestCase):
    """Le contrat, sur les octets réels (arbre de travail, ou blobs de --ref)."""

    def test_each_dataset_honours_the_byte_contract(self):
        for rel, expect_bom, short_budget in DATASETS:
            with self.subTest(dataset=rel):
                found = violations(scan(read_bytes(rel, REF)), expect_bom, short_budget)
                self.assertEqual(found, [], "\n  " + "\n  ".join(found))

    def test_the_contract_is_not_measuring_an_empty_file(self):
        # Un fichier vide ou réduit à son en-tête passerait toutes les règles ci-dessus.
        for rel, _, _ in DATASETS:
            with self.subTest(dataset=rel):
                m = scan(read_bytes(rel, REF))
                self.assertGreaterEqual(m["csv_records"], 2, "en-tête seul ou fichier vide")
                self.assertGreaterEqual(m["header_fields"], 2)


class GitAttributes(unittest.TestCase):
    """`.gitattributes` empêche-t-il encore git de convertir ces 5 fichiers ?"""

    def test_each_dataset_is_marked_minus_text(self):
        try:
            inside = subprocess.run(["git", "-C", REPO, "rev-parse", "--is-inside-work-tree"],
                                    capture_output=True, text=True)
            usable = inside.returncode == 0
        except OSError:
            usable = False
        if not usable:
            if os.environ.get("CI"):
                self.fail("git indisponible en CI : la règle `-text` ne peut pas être vérifiée")
            self.skipTest("pas un dépôt git (archive ?) : `git check-attr` indisponible")
        for rel, _, _ in DATASETS:
            with self.subTest(dataset=rel):
                done = subprocess.run(["git", "-C", REPO, "check-attr", "text", "--", rel],
                                      capture_output=True, text=True, encoding="utf-8")
                self.assertTrue(done.stdout.strip().endswith("text: unset"),
                                "attendu `text: unset` (règle `-text` de .gitattributes), "
                                "obtenu : %r" % done.stdout.strip())


class InverseControls(unittest.TestCase):
    """Chaque règle voit-elle le défaut qu'elle annonce ? Mutations des octets réels."""

    FALLACIES = DATASETS[0][0]
    SCENARII = DATASETS[4][0]

    def mutated(self, rel, expect_bom, mutate):
        data = read_bytes(rel, REF)
        changed = mutate(data)
        self.assertNotEqual(changed, data, "la mutation n'a rien changé : contrôle inerte")
        return kinds(violations(scan(changed), expect_bom))

    def test_witness_unmutated_bytes_are_clean(self):
        # Témoin : sans mutation, aucune violation — sinon les cas ci-dessous ne prouvent rien.
        self.assertEqual(violations(scan(read_bytes(self.FALLACIES, REF)), True), [])

    def test_bom_stripped_is_seen(self):
        self.assertIn("BOM", self.mutated(self.FALLACIES, True, lambda d: d[len(BOM):]))

    def test_bom_added_where_none_is_expected_is_seen(self):
        self.assertIn("BOM", self.mutated(self.SCENARII, False, lambda d: BOM + d))

    def test_line_endings_normalised_to_lf_are_seen_on_every_record(self):
        # Le défaut `core.autocrlf` : CRLF -> LF sur tout le fichier.
        data = read_bytes(self.FALLACIES, REF)
        before = scan(data)
        after = scan(data.replace(b"\r\n", b"\n"))
        self.assertEqual(after["row_bare_lf"], before["row_crlf"])
        self.assertGreater(after["row_bare_lf"], 1000)
        self.assertIn("ROW_LF", kinds(violations(after, True)))

    def test_a_single_lf_terminated_record_is_seen(self):
        # Le défaut d'un `csv.writer(lineterminator="\n")` sur UNE ligne ajoutée.
        def one_record(d):
            cut = d.rindex(b"\r\n")
            return d[:cut] + b"\n" + d[cut + 2:]
        self.assertEqual(self.mutated(self.SCENARII, False, one_record), ["ROW_LF"])

    def test_in_cell_lf_turned_into_crlf_is_seen(self):
        # Le défaut `unix2dos` / tableur : les LF nus de cellule deviennent CRLF.
        def dos(d):
            return d.replace(b"\r\n", b"\x00").replace(b"\n", b"\r\n").replace(b"\x00", b"\r\n")
        data = read_bytes(self.FALLACIES, REF)
        self.assertGreater(scan(data)["cell_lf"], 0, "ce fichier n'a plus de LF de cellule : "
                           "choisir un autre témoin, ce cas ne prouve plus rien")
        self.assertIn("CELL_CRLF", self.mutated(self.FALLACIES, True, dos))

    def test_missing_final_terminator_is_seen(self):
        self.assertIn("EOF", self.mutated(self.SCENARII, False, lambda d: d[:-2]))

    def test_a_record_with_a_missing_field_is_seen(self):
        def drop_last_field(d):
            end = d.rindex(b"\r\n")
            start = d.rindex(b"\r\n", 0, end) + 2
            record = d[start:end]
            return d[:start] + record[:record.rindex(b",")] + d[end:]
        self.assertIn("FIELDS_SHORT", self.mutated(self.SCENARII, False, drop_last_field))

    def test_a_record_with_an_extra_field_is_seen(self):
        self.assertIn("FIELDS_LONG",
                      self.mutated(self.SCENARII, False, lambda d: d[:-2] + b",en trop\r\n"))

    def test_the_rules_short_record_budget_is_an_equality(self):
        # Le budget ne masque pas une dérive : un enregistrement court DE PLUS sort rouge,
        # et un de MOINS aussi (le budget doit alors baisser dans la même PR).
        rel, expect_bom, budget = DATASETS[2]
        m = scan(read_bytes(rel, REF))
        self.assertEqual(m["short"], budget, "le budget déclaré ne décrit plus le fichier")
        self.assertEqual(kinds(violations(m, expect_bom, budget + 1)), ["FIELDS_SHORT"])
        self.assertEqual(kinds(violations(m, expect_bom, budget - 1)), ["FIELDS_SHORT"])

    def test_invalid_utf8_is_seen(self):
        self.assertIn("UTF8", self.mutated(self.SCENARII, False, lambda d: d + b"\xff\r\n"))


def report():
    for rel, expect_bom, short_budget in DATASETS:
        m = scan(read_bytes(rel, REF))
        print(rel)
        print("  BOM=%s  enregistrements=%d (csv: %d)  champs=%d  CRLF=%d  LF nus=%d  "
              "cellule: LF=%d CRLF=%d  courts=%d longs=%d  fin=%s"
              % (m["bom"], m["records"], m["csv_records"], m["header_fields"], m["row_crlf"],
                 m["row_bare_lf"], m["cell_lf"], m["cell_crlf"], m["short"], m["long"],
                 m["ends_with_terminator"]))
        for line in violations(m, expect_bom, short_budget):
            print("  VIOLATION " + line)


if __name__ == "__main__":
    argv = sys.argv[:]
    if "--ref" in argv:
        at = argv.index("--ref")
        REF = argv[at + 1]
        del argv[at:at + 2]
    if "--report" in argv:
        report()
        sys.exit(0)
    unittest.main(argv=argv)
