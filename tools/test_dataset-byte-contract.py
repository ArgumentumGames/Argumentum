#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contrat d'octets des 5 CSV de données vivants — stdlib seulement, hors ligne, aucun artefact.

    python tools/test_dataset-byte-contract.py                 # l'arbre de travail
    python tools/test_dataset-byte-contract.py --ref <ref>     # les blobs d'une ref git
    python tools/test_dataset-byte-contract.py --report        # la mesure, fichier par fichier
                                                               # (code 1 si une violation est affichée)

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
  2. BOM tel que déclaré : UN SEUL en tête de fichier, ou aucun — jamais deux (un outil
     qui « ajoute le BOM » sans regarder s'il y est déjà), jamais un U+FEFF ailleurs
     (deux fichiers à BOM mis bout à bout) ;
  3. HORS guillemets, chaque enregistrement finit par CRLF — ni LF nu, ni CR nu ;
  4. le fichier finit par un terminateur d'enregistrement ;
  5. ENTRE guillemets, un saut de ligne est un LF nu, jamais un CRLF (ce qu'un
     `unix2dos` ou un tableur produirait) ;
  6. aucun enregistrement n'a PLUS de champs que l'en-tête, et aucun n'en a MOINS —
     sauf ceux que `DATASETS` NOMME par leur clé (1er champ) : dans
     `Argumentum Rules - Cards.csv`, 10 enregistrements omettent le dernier champ
     `variant_class` (posé par #1421 sur les seules couvertures de variante, lu en
     `.Optional()`). Un enregistrement nommé a EXACTEMENT un champ de moins que
     l'en-tête ; la liste est une ÉGALITÉ : un enregistrement court non nommé est
     rouge, un enregistrement nommé devenu complet aussi (la table bouge alors dans la
     même PR). Un simple COMPTE ne suffisait pas : il restait à 10 quand un
     enregistrement nommé perdait un second champ, ou quand l'un se complétait pendant
     qu'un autre se tronquait ;
  7. l'automate de guillemets et le module `csv` comptent le MÊME nombre
     d'enregistrements, et le module `csv` accepte le fichier — sinon l'instrument
     lui-même se trompe sur ce fichier ;
  8. aucun octet de contrôle (NUL compris) autre que tabulation, LF et CR.

CE QUI N'EST PAS ÉPINGLÉ, exprès : le nombre de lignes, le nombre de colonnes, le contenu.
Ils changent légitimement ; les épingler ferait de chaque grain de données une PR rouge.

CE QUE LE CONTRAT NE VOIT PAS :
  - QUEL champ manque à un enregistrement court. Il compte les champs : un champ perdu
    au milieu d'un enregistrement nommé (et le dernier présent) a le même compte ;
  - un fichier de données qui ne figure pas dans `DATASETS` — sauf s'il tombe sous un
    motif `-text` de `.gitattributes`, auquel cas la classe `GitAttributes` rougit ;
  - sous `--ref`, la classe `GitAttributes` lit quand même le `.gitattributes` et
    l'index de L'ARBRE DE TRAVAIL, pas ceux de la ref ;
  - hors d'un dépôt git (archive), la classe `GitAttributes` se SAUTE au lieu
    d'échouer, sauf si la variable `CI` est posée. Un saut n'est pas un vert.

CONTRÔLES INVERSES (classe `InverseControls`) : chaque règle est éprouvée par une mutation
des OCTETS RÉELS du dépôt, et le test exige que la violation attendue — et nommée — sorte.
Un contrat qui ne voit pas le défaut qu'il annonce est un vert vide.

⚠️ `--ref` lit les blobs par `git show` : c'est la seule lecture qui dise ce qui est
COMMITTÉ. L'arbre de travail d'une machine en `core.autocrlf=true` peut différer du blob
pour un fichier sans règle de fin de ligne ; `.gitattributes` marque ces 5 fichiers
`-text` pour que ce ne soit plus le cas. `-text` EMPÊCHE une conversion, il ne RÉPARE
rien : un blob déjà commité en LF reste en LF, et c'est la règle 3 qui le voit.
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
# Octets de contrôle refusés : tout C0 sauf tabulation (0x09), LF (0x0A) et CR (0x0D), plus DEL.
CONTROL_BYTES = bytes(sorted(set(range(0x20)) - {0x09, 0x0A, 0x0D} | {0x7F}))

# Clés (1er champ) des enregistrements de Rules qui omettent le dernier champ `variant_class`.
RULES_SHORT = ("Rules_02", "Rules_03", "Rules_04", "Rules_05", "Rules_06",
               "Rules_08", "Rules_10", "Rules_12", "Rules_14", "Rules_15")

# (chemin relatif au dépôt, BOM attendu, clés des enregistrements autorisés à omettre le DERNIER champ)
DATASETS = [
    ("Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv", True, ()),
    ("Cards/Fallacies/Argumentum Virtues - Taxonomy.csv", True, ()),
    ("Cards/Rules/Argumentum Rules - Cards.csv", True, RULES_SHORT),
    ("Cards/Rules/Argumentum Rules - Cards Print and Play.csv", True, ()),
    ("Cards/Scenarii/Argumentum Scenarii - Cards.csv", False, ()),
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
    lead = 0
    while data.startswith(BOM, lead * len(BOM)):
        lead += 1
    body = data[lead * len(BOM):]       # TOUS les BOM de tête retirés : l'en-tête se lit propre
    controls = [b for b in CONTROL_BYTES if b in body]
    m = {
        "bom": lead > 0,
        "bom_lead": lead,               # BOM consécutifs en tête de fichier
        "bom_stray": body.count(BOM),   # U+FEFF ailleurs qu'en tête
        "control_bytes": sum(body.count(bytes([b])) for b in controls),
        "first_control": controls[0] if controls else None,
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

    m["csv_error"] = None
    try:
        rows = list(csv.reader(io.StringIO(text, newline="")))
    except csv.Error as error:
        # NUL (Python < 3.11), champ plus long que la limite du module (un guillemet jamais
        # refermé avale le reste du fichier) : un écart NOMMÉ, pas une trace de pile.
        m["csv_error"], rows = str(error), []
    m["csv_records"] = len(rows)
    m["header_fields"] = len(rows[0]) if rows else 0
    # (rang base 1, nombre de champs, clé = 1er champ)
    m["short_rows"] = [(k + 1, len(r), r[0] if r else "") for k, r in enumerate(rows)
                       if len(r) < m["header_fields"]]
    longer = [(k + 1, len(r)) for k, r in enumerate(rows) if len(r) > m["header_fields"]]
    m["short"] = len(m["short_rows"])
    m["long"], m["first_long"] = len(longer), (longer[0] if longer else None)
    return m


def violations(m, expect_bom, short_keys=()):
    """Liste NOMMÉE des écarts au contrat. Vide = conforme.

    `short_keys` : clés (1er champ) des enregistrements autorisés à omettre le dernier champ.
    """
    out = []
    if not m["utf8"]:
        out.append("UTF8: le fichier n'est pas de l'UTF-8 strict")
    if m["bom"] != expect_bom:
        out.append("BOM: %s, attendu %s"
                   % ("présent" if m["bom"] else "absent",
                      "présent" if expect_bom else "absent"))
    if m["bom_lead"] > 1:
        out.append("BOM_EXTRA: %d BOM consécutifs en tête de fichier, un seul attendu"
                   % m["bom_lead"])
    if m["bom_stray"]:
        out.append("BOM_STRAY: %d U+FEFF ailleurs qu'en tête de fichier" % m["bom_stray"])
    if m["control_bytes"]:
        out.append("CONTROL: %d octet(s) de contrôle autres que tabulation, LF et CR "
                   "(le plus petit : 0x%02X)" % (m["control_bytes"], m["first_control"]))
    if m["csv_error"]:
        out.append("CSV: le module csv refuse le fichier (%s)" % m["csv_error"])
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
    width = m["header_fields"]
    declared = sorted(short_keys)
    actual = sorted(key for _, _, key in m["short_rows"])
    undeclared = [(rank, count, key) for rank, count, key in m["short_rows"] if key not in declared]
    if undeclared:
        rank, count, key = undeclared[0]
        out.append("FIELDS_SHORT: %d enregistrement(s) ont moins de champs que les %d de "
                   "l'en-tête sans être nommés dans DATASETS (1er : enregistrement %d, "
                   "clé %r, %d champs)" % (len(undeclared), width, rank, key, count))
    too_short = [(rank, count, key) for rank, count, key in m["short_rows"]
                 if key in declared and count != width - 1]
    if too_short:
        rank, count, key = too_short[0]
        out.append("FIELDS_SHORT: %d enregistrement(s) nommé(s) omettent plus que le dernier "
                   "champ (1er : enregistrement %d, clé %r, %d champs pour %d à l'en-tête)"
                   % (len(too_short), rank, key, count, width))
    complete = [key for key in declared if key not in actual]
    if complete:
        out.append("FIELDS_SHORT: %d enregistrement(s) nommé(s) dans DATASETS ne sont plus "
                   "courts (%s) : les retirer de la table dans la même PR"
                   % (len(complete), ", ".join(complete)))
    if not (undeclared or complete) and actual != declared:
        out.append("FIELDS_SHORT: une clé d'enregistrement court est en double (%s)"
                   % ", ".join(sorted(set(k for k in actual if actual.count(k) > 1))))
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


# Git compare les motifs de .gitattributes SANS la casse quand `core.ignorecase` est vrai
# (Windows, macOS) et AVEC sinon (Linux). Un motif à la mauvaise casse est donc vert sur
# une machine Windows et inerte sur le runner. La lecture est forcée à la casse exacte.
CHECK_ATTR = ["git", "-C", REPO, "-c", "core.ignorecase=false", "check-attr"]


class GitAttributes(unittest.TestCase):
    """`.gitattributes` empêche-t-il encore git de convertir ces 5 fichiers, et eux seuls ?"""

    def setUp(self):
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

    def test_each_dataset_is_marked_minus_text(self):
        for rel, _, _ in DATASETS:
            with self.subTest(dataset=rel):
                done = subprocess.run(CHECK_ATTR + ["text", "--", rel],
                                      capture_output=True, text=True, encoding="utf-8")
                self.assertTrue(done.stdout.strip().endswith("text: unset"),
                                "attendu `text: unset` (règle `-text` de .gitattributes), "
                                "obtenu : %r" % done.stdout.strip())

    def test_no_other_tracked_csv_under_cards_is_marked_minus_text(self):
        # L'autre sens : les motifs de .gitattributes ne débordent pas. Un CSV marqué `-text`
        # sans être dans DATASETS serait protégé de git mais invisible au contrat.
        listed = subprocess.run(["git", "-C", REPO, "ls-files", "-z", "--", "Cards/*.csv"],
                                capture_output=True)
        self.assertEqual(listed.returncode, 0, listed.stderr.decode("utf-8", "replace"))
        done = subprocess.run(CHECK_ATTR + ["-z", "--stdin", "text"],
                              input=listed.stdout, capture_output=True)
        self.assertEqual(done.returncode, 0, done.stderr.decode("utf-8", "replace"))
        fields = done.stdout.decode("utf-8").split("\0")
        answers = [(fields[i], fields[i + 2]) for i in range(0, len(fields) - 2, 3)]
        self.assertGreater(len(answers), len(DATASETS),
                           "aucun autre CSV suivi sous Cards/ : ce cas ne prouve plus rien")
        self.assertEqual(sorted(path for path, value in answers if value == "unset"),
                         sorted(rel for rel, _, _ in DATASETS))


class InverseControls(unittest.TestCase):
    """Chaque règle voit-elle le défaut qu'elle annonce ? Mutations des octets réels."""

    FALLACIES = DATASETS[0][0]
    RULES = DATASETS[2][0]
    SCENARII = DATASETS[4][0]

    def mutated(self, rel, expect_bom, mutate, short_keys=()):
        data = read_bytes(rel, REF)
        changed = mutate(data)
        self.assertNotEqual(changed, data, "la mutation n'a rien changé : contrôle inerte")
        return kinds(violations(scan(changed), expect_bom, short_keys))

    def test_witness_unmutated_bytes_are_clean(self):
        # Témoin : sans mutation, aucune violation — sinon les cas ci-dessous ne prouvent rien.
        self.assertEqual(violations(scan(read_bytes(self.FALLACIES, REF)), True), [])

    def test_bom_stripped_is_seen(self):
        self.assertIn("BOM", self.mutated(self.FALLACIES, True, lambda d: d[len(BOM):]))

    def test_bom_added_where_none_is_expected_is_seen(self):
        self.assertIn("BOM", self.mutated(self.SCENARII, False, lambda d: BOM + d))

    def test_a_doubled_bom_is_seen(self):
        # Le défaut d'un outil qui « ajoute le BOM » à un fichier qui l'a déjà. Un booléen
        # « commence par un BOM » le lit vert, et l'en-tête devient '﻿pk'.
        self.assertEqual(self.mutated(self.FALLACIES, True, lambda d: BOM + d), ["BOM_EXTRA"])
        self.assertEqual(self.mutated(self.FALLACIES, True, lambda d: BOM + BOM + d), ["BOM_EXTRA"])

    def test_a_bom_elsewhere_than_at_the_head_is_seen(self):
        # Deux fichiers à BOM mis bout à bout : le second BOM atterrit en tête d'un enregistrement.
        def second_record(d):
            return d.replace(b"\r\n", b"\r\n" + BOM, 1)
        self.assertEqual(self.mutated(self.FALLACIES, True, second_record), ["BOM_STRAY"])

    def test_a_control_byte_is_seen_and_a_tab_is_not(self):
        for byte in (0x00, 0x0B, 0x1B, 0x7F):
            with self.subTest(byte="0x%02X" % byte):
                def control(d, byte=byte):
                    return d.replace(b",", b"," + bytes([byte]), 1)
                self.assertIn("CONTROL", self.mutated(self.SCENARII, False, control))
        # Témoin : la tabulation est un octet de contrôle PERMIS (Fallacies en porte).
        self.assertEqual(self.mutated(self.SCENARII, False, lambda d: d.replace(b",", b",\t", 1)), [])

    def test_a_file_the_csv_module_refuses_is_a_named_violation_not_a_crash(self):
        # Un guillemet jamais refermé avale la suite : au-delà de la limite de champ du
        # module csv, c'est une exception. Le contrat doit la NOMMER, pas dérouler une pile.
        def runaway(d):
            return d + b'"' + b"x" * 200000 + b"\r\n"
        found = self.mutated(self.SCENARII, False, runaway)
        self.assertIn("CSV", found)
        self.assertIn("QUOTE", found)

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

    def test_a_cr_terminated_record_is_seen(self):
        # Fin de ligne « vieux Mac » sur le dernier enregistrement.
        def one_cr(d):
            return d[:-2] + b"\r"
        self.assertIn("ROW_CR", self.mutated(self.SCENARII, False, one_cr))

    def test_in_cell_lf_turned_into_bare_cr_is_seen(self):
        def cr(d):
            return d.replace(b"\r\n", b"\x00").replace(b"\n", b"\r").replace(b"\x00", b"\r\n")
        self.assertIn("CELL_CR", self.mutated(self.FALLACIES, True, cr))

    def test_an_unclosed_quote_is_seen(self):
        self.assertIn("QUOTE", self.mutated(self.SCENARII, False, lambda d: d + b'"jamais fermee\r\n'))

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

    def rules(self):
        """(octets, mesure, clés déclarées) de Rules, et ce que les cas ci-dessous supposent."""
        rel, expect_bom, keys = DATASETS[2]
        data = read_bytes(rel, REF)
        m = scan(data)
        self.assertEqual(violations(m, expect_bom, keys), [], "témoin : Rules est conforme")
        self.assertTrue(keys, "plus aucun enregistrement court déclaré dans Rules : ces cas "
                              "n'ont plus de matière, les retirer avec la liste RULES_SHORT")
        self.assertEqual(m["short_rows"][-1][0], m["csv_records"],
                         "le dernier enregistrement de Rules n'est plus court : choisir un autre témoin")
        return data, m, keys

    def test_the_rules_short_record_list_is_an_equality(self):
        # La liste ne masque pas une dérive : un enregistrement court NON NOMMÉ sort rouge,
        # et un enregistrement nommé qui n'est PAS court aussi.
        _, m, keys = self.rules()
        self.assertEqual(kinds(violations(m, True, keys[1:])), ["FIELDS_SHORT"])
        self.assertEqual(kinds(violations(m, True, keys + ("Rules_01",))), ["FIELDS_SHORT"])

    def test_a_short_record_trading_places_with_a_full_one_is_seen(self):
        # Un compte ne voit pas ce défaut : un enregistrement nommé se complète, un autre
        # perd son dernier champ — il y a toujours autant d'enregistrements courts.
        data, m, keys = self.rules()
        rows = list(csv.reader(io.StringIO(data[len(BOM):].decode("utf-8"), newline="")))
        full = [r for r in rows[1:] if len(r) == m["header_fields"] and r[-1]]
        self.assertTrue(full, "plus d'enregistrement complet au dernier champ non vide : autre témoin")
        needle = b"," + full[-1][-1].encode("utf-8") + b"\r\n"
        self.assertEqual(data.count(needle), 1, "aiguille ambiguë : choisir un autre témoin")
        swapped = data.replace(needle, b"\r\n", 1)[:-2] + b",\r\n"
        after = scan(swapped)
        self.assertEqual(after["short"], m["short"], "le compte devait rester le même")
        self.assertEqual(kinds(violations(after, True, keys)), ["FIELDS_SHORT", "FIELDS_SHORT"])

    def test_a_named_short_record_losing_a_second_field_is_seen(self):
        # Un compte ne voit pas ce défaut non plus : même enregistrement, même clé, un champ
        # de moins encore. Le dernier enregistrement est réécrit sans son dernier champ.
        data, m, keys = self.rules()
        start = data.rindex(b"\r\n" + keys[-1].encode("utf-8") + b",") + 2
        row = next(csv.reader(io.StringIO(data[start:].decode("utf-8"), newline="")))
        self.assertEqual(len(row), m["header_fields"] - 1)
        rewritten = io.StringIO(newline="")
        csv.writer(rewritten, lineterminator="\r\n").writerow(row[:-1])
        after = scan(data[:start] + rewritten.getvalue().encode("utf-8"))
        self.assertEqual(after["short"], m["short"], "le compte devait rester le même")
        self.assertEqual(sorted(key for _, _, key in after["short_rows"]), sorted(keys))
        self.assertEqual(kinds(violations(after, True, keys)), ["FIELDS_SHORT"])

    def test_a_duplicated_short_record_key_is_seen(self):
        # La 3e branche de l'égalité RULES_SHORT, restée sans témoin : dupliquer un
        # enregistrement court NOMMÉ ne crée ni un court non nommé (sa clé l'est), ni un
        # nommé complété (il reste court) — les deux branches ci-dessus restent muettes.
        # L'enregistrement est recollé à l'identique (CRLF final compris) : seule la
        # liste des clés dérive, et la violation doit NOMMER la clé doublée.
        data, m, keys = self.rules()
        start = data.rindex(b"\r\n" + keys[-1].encode("utf-8") + b",") + 2
        self.assertTrue(data[start:].endswith(b"\r\n"),
                        "l'enregistrement à dupliquer doit finir par CRLF")
        after = scan(data + data[start:])
        self.assertEqual(after["short"], m["short"] + 1,
                         "la duplication devait ajouter exactement un enregistrement court")
        found = violations(after, True, keys)
        self.assertEqual(len(found), 1, "une seule violation attendue : %r" % found)
        self.assertIn("en double", found[0])
        self.assertIn(keys[-1], found[0])

    def test_invalid_utf8_is_seen(self):
        self.assertIn("UTF8", self.mutated(self.SCENARII, False, lambda d: d + b"\xff\r\n"))


class ReportMode(unittest.TestCase):
    """`--report` rend-il un code d'échec quand il affiche une violation ?"""

    def test_report_exit_code_follows_what_it_prints(self):
        sink = io.StringIO()
        self.assertEqual(report(out=sink), 0, sink.getvalue())
        self.assertNotIn("VIOLATION", sink.getvalue())
        sink = io.StringIO()
        self.assertEqual(report(reader=lambda rel, ref: read_bytes(rel, ref)[:-2], out=sink), 1)
        self.assertEqual(sink.getvalue().count("VIOLATION EOF"), len(DATASETS))


def report(reader=read_bytes, out=None):
    """Affiche la mesure de chaque fichier. Rend 1 si une violation est affichée, sinon 0."""
    out = out or sys.stdout
    shown = 0
    for rel, expect_bom, short_keys in DATASETS:
        m = scan(reader(rel, REF))
        print(rel, file=out)
        print("  BOM en tête=%d (ailleurs: %d)  enregistrements=%d (csv: %d)  champs=%d  CRLF=%d  "
              "LF nus=%d  cellule: LF=%d CRLF=%d  courts=%d longs=%d  contrôles=%d  fin=%s"
              % (m["bom_lead"], m["bom_stray"], m["records"], m["csv_records"], m["header_fields"],
                 m["row_crlf"], m["row_bare_lf"], m["cell_lf"], m["cell_crlf"], m["short"],
                 m["long"], m["control_bytes"], m["ends_with_terminator"]), file=out)
        for line in violations(m, expect_bom, short_keys):
            shown += 1
            print("  VIOLATION " + line, file=out)
    return 1 if shown else 0


if __name__ == "__main__":
    argv = sys.argv[:]
    if "--ref" in argv:
        at = argv.index("--ref")
        if at + 1 >= len(argv):
            sys.exit("--ref attend une ref git (ex. --ref origin/master)")
        REF = argv[at + 1]
        del argv[at:at + 2]
    if "--report" in argv:
        # Une mesure qui affiche une violation ne rend pas 0 : `--report && suite` ne doit
        # pas enchaîner sur un fichier non conforme.
        sys.exit(report())
    unittest.main(argv=argv)
