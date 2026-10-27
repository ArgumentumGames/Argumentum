#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contrat de la liste blanche EAV app 48 / app 50 (#458 pool item 7) - stdlib, hors ligne.

    python tools/test_458-eav-export-guards.py

POURQUOI CE FICHIER EXISTE. `tools/458-eav-content-export.py` lit la base DNN, ce que
seule la lane qui la sert peut faire : son `selftest` exige la base. Les gardes ajoutées
pour l'app 50 (MobiusForms5) doivent pourtant pouvoir se rejouer partout, parce que la
même app stocke les soumissions des visiteurs (`RawData`, données personnelles) et que le
dépôt est public. Ce test charge le module SANS se connecter et éprouve chaque garde sur
des rangées synthétiques, avec son contrôle inverse.

CE QUI EST ÉPINGLÉ :
  1. `RawData` refusé à la configuration (aussi en casse différente : la collation de la
     base est insensible à la casse), dans le SQL, et sur chaque rangée lue.
  2. La liste des champs exportés de l'app 50 est RE-DÉRIVÉE des vues committées du
     formulaire de contact - jamais recopiée : une vue qui change rougit ce test.
  3. Le filtre de champs garde `Html` seul pour l'app 48, les champs lus par le
     formulaire pour l'app 50, et ne touche pas aux autres types.
  4. Les témoins : une entrée qui ne rapporte rien fait échouer l'export au lieu de
     produire un vert silencieux (le défaut de l'ancienne entrée `(52, "Content")`).
  5. L'app 48 est bornée à l'entité mesurée E10181 : un autre fragment HTML de la même
     app, ou une rangée sans EntityId, ne sort jamais (fermé, pas ouvert).
  6. Le registre committé passe toujours la garde (le jeton `RawData` ajouté ne casse pas
     l'export courant).
"""
import importlib.util
import pathlib
import re
import unittest

HERE = pathlib.Path(__file__).resolve().parent
REPO = HERE.parent
TOOL = HERE / "458-eav-content-export.py"
LIVE = REPO / "DNNPlatform" / "Portals" / "1" / "2sxc" / "MobiusForms5" / "live"
REGISTRY = REPO / "docs" / "dnn-localization" / "eav-content.csv"

_spec = importlib.util.spec_from_file_location("eav_export", TOOL)
eav = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(eav)

APP48 = (48, "HtmlFragment")
APP50 = (50, "App-Resources")


def row(app, ctype, field, fr="x", guid="00000000-0000-0000-0000-000000000001", eid=None):
    # The SQL returns EntityId as a string (the reader casts every value).
    if eid is None:
        eid = "10181" if (app, ctype) == APP48 else "1"
    return {
        "app_id": str(app),
        "content_type": ctype,
        "entity_id": eid,
        "entity_guid": guid,
        "field": field,
        "fr": fr,
        "key": f"{guid}|{field}",
    }


def strip_comments(text):
    text = re.sub(r"/\*.*?\*/", "", text, flags=re.S)
    text = re.sub(r"@\*.*?\*@", "", text, flags=re.S)
    return re.sub(r"(?m)^\s*//.*$", "", text)


def derive_contact_form_fields(strip=True):
    """Every Resources.<Field> read by the contact form's view chain."""
    seen, todo, fields = set(), ["_Contact Form.cshtml"], set()
    while todo:
        rel = todo.pop()
        if rel in seen:
            continue
        seen.add(rel)
        text = (LIVE / rel).read_text(encoding="utf-8")
        if strip:
            text = strip_comments(text)
        for m in re.finditer(r'(?:Html\.Partial|CreateInstance)\("([^"]+)"', text):
            todo.append(m.group(1))
        for m in re.finditer(r"\bResources\.(\w+)", text):
            if m.group(1) != "Get":
                fields.add(m.group(1))
        for m in re.finditer(
            r'\bFieldBuilder\.(?:Text|Multiline|EMail|DropDown)\("(\w+)"', text
        ):
            fields.add("Label" + m.group(1))
    return seen, fields


class RawDataIsNeverExported(unittest.TestCase):
    def test_config_refuses_rawdata_in_any_case(self):
        eav.assert_whitelist_safe()  # the committed configuration passes
        for name in ("RawData", "rawdata", "RAWDATA"):
            with self.assertRaises(SystemExit) as ctx:
                eav.assert_whitelist_safe(
                    whitelist=[*eav.WHITELIST, (50, name, "label")]
                )
            self.assertIn("type interdit", str(ctx.exception))

    def test_config_refuses_field_allow_outside_whitelist(self):
        with self.assertRaises(SystemExit) as ctx:
            eav.assert_whitelist_safe(
                field_allow={**eav.FIELD_ALLOW, (50, "RawData"): {"Html"}}
            )
        self.assertIn("FIELD_ALLOW hors liste blanche", str(ctx.exception))

    def test_sql_excludes_rawdata_after_the_whole_or_group(self):
        clause = eav.where_clause()
        self.assertTrue(clause.startswith("((ct.AppId = "), clause[:40])
        self.assertTrue(clause.endswith(") AND ct.Name NOT IN ('RawData')"), clause[-60:])
        self.assertIn("(ct.AppId = 50 AND ct.Name IN ('App-Resources'))", clause)
        self.assertIn("(ct.AppId = 48 AND ct.Name IN ('HtmlFragment'))", clause)

    def test_rawdata_row_is_refused_with_its_own_reason(self):
        eav.assert_export_safe([row(50, "App-Resources", "ButtonSend")])
        for name in ("RawData", "rawdata"):
            with self.assertRaises(SystemExit) as ctx:
                eav.assert_export_safe([row(50, name, "Message")])
            # Inverse control: the refusal comes from the forbidden-type rule,
            # not from the generic whitelist rule that would also reject it.
            self.assertIn("type interdit", str(ctx.exception))
            self.assertNotIn("hors liste blanche", str(ctx.exception))

    def test_rawdata_token_in_a_whitelisted_value_is_refused(self):
        with self.assertRaises(SystemExit) as ctx:
            eav.assert_export_safe([row(50, "App-Resources", "ButtonSend", fr="see RawData")])
        self.assertIn("jeton interdit", str(ctx.exception))


class FieldAllowIsDerivedFromTheConsumer(unittest.TestCase):
    def test_contact_form_fields_equal_the_view_chain(self):
        seen, derived = derive_contact_form_fields()
        # Anti-vacuity: the walk reached the partial that renders the messages
        # and the button, and found the measured served button.
        self.assertIn("shared/_Submit with Messages.cshtml", seen)
        self.assertIn("shared/FieldBuilders.cs", seen)
        self.assertGreaterEqual(len(derived), 20)
        self.assertIn("ButtonSend", derived)
        self.assertIn("LabelSubject", derived)
        self.assertEqual(derived, eav.FIELD_ALLOW[APP50])

    def test_comment_stripping_is_load_bearing(self):
        # Inverse control: FieldBuilders.cs carries an example call in a block
        # comment (`FieldBuilder.EMail("Email", true)`). Without stripping, the
        # walk reads a field no view uses - the instrument can see a 1.
        _seen, raw = derive_contact_form_fields(strip=False)
        _seen, clean = derive_contact_form_fields()
        self.assertIn("LabelEmail", raw)
        self.assertNotIn("LabelEmail", clean)

    def test_mail_templates_stay_out(self):
        for field in eav.FIELD_ALLOW[APP50]:
            self.assertFalse(field.startswith(("Mail", "Owner", "Customer")), field)

    def test_filter_keeps_only_allowed_fields(self):
        rows = [
            row(48, "HtmlFragment", "Html", guid="a"),
            row(48, "HtmlFragment", "Title", guid="a"),
            row(50, "App-Resources", "LabelSubject", guid="b"),
            row(50, "App-Resources", "MailOwnerSubject", guid="b"),
            row(50, "App-Resources", "OwnerMail", guid="b"),
            row(33, "Content", "Title", guid="c"),
        ]
        kept = [(r["app_id"], r["field"]) for r in eav.apply_field_allow(rows)]
        self.assertEqual(kept, [("48", "Html"), ("50", "LabelSubject"), ("33", "Title")])

    def test_app48_exports_html_only(self):
        self.assertEqual(eav.FIELD_ALLOW[APP48], {"Html"})
        self.assertEqual(eav.KIND_BY_TYPE[APP48], "editorial")
        self.assertEqual(eav.KIND_BY_TYPE[APP50], "label")


def witness_rows():
    return [
        row(48, "HtmlFragment", "Html", fr="<!-- Begin Sendinblue Form --><style>", guid="a"),
        row(50, "App-Resources", "ButtonSend", fr="Envoyer", guid="b"),
    ]


class WitnessesFailLoud(unittest.TestCase):
    def rows(self):
        return witness_rows()

    def test_both_witnesses_present_pass(self):
        eav.assert_witnesses(self.rows())

    def test_missing_carrier_fails(self):
        for drop in (0, 1):
            rows = [r for i, r in enumerate(self.rows()) if i != drop]
            with self.assertRaises(SystemExit) as ctx:
                eav.assert_witnesses(rows)
            self.assertIn("témoin absent", str(ctx.exception))

    def test_wrong_value_fails(self):
        rows = self.rows()
        rows[0]["fr"] = "<div>autre fragment</div>"
        with self.assertRaises(SystemExit):
            eav.assert_witnesses(rows)

    def test_null_value_is_a_missing_witness_not_a_crash(self):
        # A NULL cell can reach the guard as None: it must read as "absent"
        # (SystemExit with the reason), never as an AttributeError.
        rows = self.rows()
        rows[1]["fr"] = None
        with self.assertRaises(SystemExit) as ctx:
            eav.assert_witnesses(rows)
        self.assertIn("témoin absent", str(ctx.exception))
        # ...and a None row before the real carrier does not hide it.
        eav.assert_witnesses([row(50, "App-Resources", "ButtonSend", fr=None, guid="c"),
                              *self.rows()])


class HtmlFragmentIsBoundToTheMeasuredEntity(unittest.TestCase):
    def test_entity_allow_pins_the_measured_carrier(self):
        self.assertEqual(eav.ENTITY_ALLOW, {APP48: {10181}})
        with self.assertRaises(SystemExit) as ctx:
            eav.assert_whitelist_safe(entity_allow={(50, "RawData"): {1}})
        self.assertIn("ENTITY_ALLOW hors liste blanche", str(ctx.exception))

    def test_read_filter_keeps_only_e10181_for_app48(self):
        rows = [
            row(48, "HtmlFragment", "Html", guid="a", eid="10181"),
            row(48, "HtmlFragment", "Html", guid="b", eid="10182"),
            row(48, "HtmlFragment", "Html", guid="c", eid=""),
            row(48, "HtmlFragment", "Html", guid="d", eid="10181x"),
            row(50, "App-Resources", "ButtonSend", guid="e", eid="9169"),
            row(33, "Content", "Title", guid="f", eid="7"),
        ]
        kept = [r["entity_guid"] for r in eav.apply_entity_allow(rows)]
        # Inverse control: the unlisted, empty and non-numeric ids are dropped,
        # and the types without an entity bound are untouched.
        self.assertEqual(kept, ["a", "e", "f"])

    def test_post_read_applies_both_bounds(self):
        # finish_rows is what fetch_rows returns: both bounds, kind and key.
        raw = [
            {"app_id": "48", "content_type": "HtmlFragment", "entity_id": "10181",
             "entity_guid": "a", "field": "Html", "fr": "<p>"},
            {"app_id": "48", "content_type": "HtmlFragment", "entity_id": "10181",
             "entity_guid": "a", "field": "Title", "fr": "t"},
            {"app_id": "48", "content_type": "HtmlFragment", "entity_id": "999",
             "entity_guid": "b", "field": "Html", "fr": "<p>"},
            {"app_id": "50", "content_type": "App-Resources", "entity_id": "9169",
             "entity_guid": "c", "field": "OwnerMail", "fr": "m"},
        ]
        out = eav.finish_rows(raw)
        self.assertEqual([r["key"] for r in out], ["a|Html"])
        self.assertEqual(out[0]["value_kind"], "editorial")

    def test_export_guard_refuses_an_unlisted_or_absent_entity(self):
        eav.assert_export_safe([row(48, "HtmlFragment", "Html")])
        for eid in ("10182", "", None):
            bad = row(48, "HtmlFragment", "Html", eid="x")
            if eid is None:
                del bad["entity_id"]
            else:
                bad["entity_id"] = eid
            with self.assertRaises(SystemExit) as ctx:
                eav.assert_export_safe([bad])
            self.assertIn("entité hors liste", str(ctx.exception))

    def test_a_read_without_entity_id_fails_loud(self):
        # If the SELECT lost its EntityId column, the bound must not open: every
        # app 48 row is dropped, and the witness turns that into a refusal.
        rows = witness_rows()
        for r in rows:
            del r["entity_id"]
        with self.assertRaises(SystemExit) as ctx:
            eav.assert_witnesses(eav.apply_entity_allow(rows))
        self.assertIn("Sendinblue", str(ctx.exception))


class CommittedRegistryStillPasses(unittest.TestCase):
    def test_registry_passes_the_content_guards(self):
        # The registry carries no EntityId (not a COLUMN), so the per-row DB guard
        # does not apply to it; the content guards and the type bound do.
        rows = eav.read_csv(REGISTRY)
        self.assertGreater(len(rows), 500)
        eav.assert_no_forbidden(rows)
        for r in rows:
            self.assertIn((int(r["app_id"]), r["content_type"]), eav.ALLOWED_TYPES)
        # verify and classify join on the key: it stays unique, as the DB guard
        # required. Inverse control: one duplicated row is seen.
        def dupes(rs):
            keys = [r["key"] for r in rs]
            return sorted({k for k in keys if keys.count(k) > 1})
        self.assertEqual(dupes(rows), [])
        self.assertEqual(dupes([*rows, rows[0]]), [rows[0]["key"]])


if __name__ == "__main__":
    unittest.main(verbosity=2)
