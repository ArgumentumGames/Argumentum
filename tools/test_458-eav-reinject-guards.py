#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Gardes de refus G1-G3 de l'outil de reinjection EAV (#2326) - stdlib, hors ligne.

    python tools/test_458-eav-reinject-guards.py

POURQUOI CE FICHIER EXISTE. `tools/458-eav-reinject.py` lit et ecrit la base DNN, ce
que seule la lane qui la sert peut faire : son dry-run exige la base. Les gardes
exigees par la revue ai-01 (c.7053569313) doivent pourtant se rejouer partout,
parce que l'outil tournera sur le staging - une copie de la prod que M1 n'a pas
mesuree. Ce test charge le module SANS se connecter (EXP.run_query remplace par un
faux qui sert des rangees synthetiques) et eprouve chaque garde, son controle
inverse (entree saine acceptee) ET SA MUTATION (garde neutralisee dans le source ->
le meme defaut passe sans refus : le test epingle la garde, pas un effet de bord).

LES TROIS GARDES (revue c.7053569313) :
  G1  une paire (app cible, GUID) qui designe plus d'une entite vivante — les
      valeurs des deux entites se melangeraient sous une meme cle et le writer
      prendrait la premiere ligne. Refus au dry-run (fetch_target) + re-detection
      in-tx dans le writer (source PS, non executee ici).
  G2  un vainqueur de la carte d'apps sans marge — matched == 0 (le contenu n'a
      rien departage) ou 2e candidat a matched == vainqueur (l'overlap de GUIDs
      ne departage plus rien entre deux copies d'un meme gabarit).
  G3  une carte non injective — deux apps du registre vers la meme app cible.

RETOUCHES P23-27 (contre-revue po-2024 c.7057778259) — trois proprietes de plus :
  R1  la carte d'apps est imprimee AVANT fetch_target : un refus (G1, dimension
      absente) se lit alors sur des apps etablies (resolve_target).
  R2  le REFUS (G1) liste les EntityId des paires, pas seulement leur nombre, et
      le refus tient meme si cette seconde lecture echoue.
  R3  le source ne met plus une rangee « servie » et le ValueId MAX sur la meme
      ligne — regle refutee par M1/M3 (c.7050026044 : le MAX ne departage que les
      rangees d'une meme entite). Garde de SOURCE : un detecteur des formulations
      ecrites, pas une preuve d'absence d'idee. La retouche du docstring (l. 39)
      est une precision de prose, sans garde machine : le defaut y etait une
      implicature (« la rangee au MAX »), pas une phrase.
"""
import contextlib
import importlib.util
import io
import pathlib
import re
import unittest

HERE = pathlib.Path(__file__).resolve().parent
TOOL = HERE / "458-eav-reinject.py"


def load_tool():
    """Charge le module hors ligne (aucune connexion : main() n'est pas appele)."""
    spec = importlib.util.spec_from_file_location("eav_reinject", TOOL)
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    return mod


def mutated_source(replacements):
    """Copie du source avec des remplacements litteraux (mutations). Chaque aiguille
    doit exister : une mutation muette ferait passer le test pour une garde qui mord."""
    src = TOOL.read_text(encoding="utf-8")
    for needle, repl in replacements:
        assert needle in src, f"aiguille de mutation introuvable: {needle[:70]!r}"
        src = src.replace(needle, repl)
    return src


def load_source(src):
    ns = {"__file__": str(TOOL)}
    exec(compile(src, str(TOOL), "exec"), ns)  # noqa: S102 (mutation volontaire)
    return ns


def mutated_tool(tag):
    """Charge une COPIE du source ou la garde `tag` (G1/G2/G3) est neutralisee :
    son `raise SystemExit(f"REFUS (tag)...")` devient un simple print. Si le test
    echoue sur cette copie, c'est qu'il epingle autre chose que la garde."""
    return load_source(mutated_source([(f'raise SystemExit(f"REFUS ({tag})',
                                        f'print(f"REFUS ({tag})')]))


class FakeQuery:
    """EXP.run_query de remplacement : sert des rangees synthetiques par marqueur SQL."""

    def __init__(self, mod, dup_rows, val_rows, eid_rows=None, eid_fails=False):
        self.dup_rows, self.val_rows = dup_rows, val_rows
        self.eid_rows, self.eid_fails = eid_rows or [], eid_fails
        self.original = mod.EXP.run_query
        mod.EXP.run_query = self
        self.calls = []

    def __call__(self, conn, sql):
        self.calls.append(sql)
        if "HAVING COUNT(DISTINCT e.EntityId) > 1" in sql:
            return self.dup_rows
        if "e.EntityId AS eid" in sql:  # liste des EntityId du refus G1 (P23-27, R2)
            if self.eid_fails:
                raise RuntimeError("liste des EntityId indisponible (test)")
            return self.eid_rows
        if "FROM TsDynDataApp" in sql:
            return [{"app": 33, "zone": 3}]
        if "FROM TsDynDataDimension" in sql:
            # Les 7 cultures de LANGS dans la zone 3 (ids 7..13 comme la preprod).
            ext_by_lang = {v: k for k, v in self.ext_map.items()}
            out = []
            for i, lang in enumerate(sorted(ext_by_lang)):
                if lang in ("fr-FR",):
                    continue
                out.append({"id": 7 + i, "ext": lang, "zone": 3})
            return out
        if "vd.ValueId IS NULL" in sql and "ct.Name IN" in sql:
            return self.val_rows  # requete de build_app_map
        if "ORDER BY app, entity_guid, field, dimension_id, vid" in sql:
            return []  # grand SELECT de fetch_target : cible vierge
        raise AssertionError("SQL inattendu sous faux run_query:\n" + sql[:200])

    @property
    def ext_map(self):
        # LANGS -> codes culture, relus depuis le module d'export recharge (stable).
        spec = importlib.util.spec_from_file_location("eavexp_probe", HERE / "458-eav-content-export.py")
        exp = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(exp)
        return dict(exp.DIM_EXTERNAL_KEY)

    def restore(self, mod):
        mod.EXP.run_query = self.original


def reg_rows():
    """Registre synthetique : 1 app (33), 2 lignes, cellules fr non vides."""
    return [
        {"app_id": "33", "entity_guid": "AAAAAAAA-0000-0000-0000-000000000001",
         "content_type": "News", "field": "Title", "fr": "Titre fr", "en": "Title en"},
        {"app_id": "33", "entity_guid": "AAAAAAAA-0000-0000-0000-000000000002",
         "content_type": "News", "field": "Content", "fr": "Contenu fr", "en": "Content en"},
    ]


def val_rows_healthy():
    """La copie configuree (app 33) dit fr = registre ; la demo (app 2) dit autre chose."""
    rows = []
    for r in reg_rows():
        rows.append({"app": 33, "g": r["entity_guid"], "f": r["field"], "val": r["fr"]})
        rows.append({"app": 2, "g": r["entity_guid"], "f": r["field"], "val": "demo value"})
    return rows


class G1MultiEntityPair(unittest.TestCase):
    """Une paire (app, guid) qui designe plus d'une entite vivante -> REFUS."""

    def setUp(self):
        self.mod = load_tool()
        self.mod.CURRENT_PLAN = [{"app_id": 33, "entity_guid": "AAAAAAAA-0000-0000-0000-000000000001",
                                  "field": "Title", "lang": "en", "value": "Title en",
                                  "content_type": "News"}]
        self.mod.CURRENT_FR_PLAN = []

    def run_fetch(self, mod, dup_rows):
        fake = FakeQuery(mod, dup_rows, val_rows_healthy())
        try:
            return mod.fetch_target("fake-conn", {33: 33})
        finally:
            fake.restore(mod)

    def test_refuse(self):
        dup = [{"app": 33, "g": "aaaaaaaa-0000-0000-0000-000000000001", "n": 2}]
        with self.assertRaises(SystemExit) as cm:
            self.run_fetch(self.mod, dup)
        self.assertIn("REFUS (G1)", str(cm.exception))

    def test_saine_acceptee(self):
        out = self.run_fetch(self.mod, [])
        self.assertIn(33, out["lang_dim_by_app"])

    def test_mutation_la_garde_passe(self):
        ns = mutated_tool("G1")
        mod = type("M", (), ns)  # namespace -> attributs
        mod.CURRENT_PLAN = self.mod.CURRENT_PLAN
        mod.CURRENT_FR_PLAN = []
        dup = [{"app": 33, "g": "aaaaaaaa-0000-0000-0000-000000000001", "n": 2}]
        out = self.run_fetch(mod, dup)  # sans la garde : PAS de refus
        self.assertIn(33, out["lang_dim_by_app"])

    def run_fetch_with_ids(self, mod, dup_rows, **kw):
        fake = FakeQuery(mod, dup_rows, val_rows_healthy(), **kw)
        try:
            return mod.fetch_target("fake-conn", {33: 33})
        finally:
            fake.restore(mod)

    def test_refus_liste_les_entityid(self):
        """R2 : le refus nomme les EntityId relus, pas seulement leur nombre."""
        dup = [{"app": 33, "g": "aaaaaaaa-0000-0000-0000-000000000001", "n": 2}]
        with self.assertRaises(SystemExit) as cm:
            self.run_fetch_with_ids(self.mod, dup, eid_rows=[
                {"app": 33, "g": "AAAAAAAA-0000-0000-0000-000000000001", "eid": 102},
                {"app": 33, "g": "AAAAAAAA-0000-0000-0000-000000000001", "eid": 101}])
        msg = str(cm.exception)
        self.assertIn("REFUS (G1)", msg)
        self.assertIn("EntityId 101/102", msg)

    def test_refus_tient_si_la_liste_echoue(self):
        """R2 : la liste est une aide au debogage — son echec ne desarme pas le refus."""
        dup = [{"app": 33, "g": "aaaaaaaa-0000-0000-0000-000000000001", "n": 2}]
        with self.assertRaises(SystemExit) as cm:
            self.run_fetch_with_ids(self.mod, dup, eid_fails=True)
        msg = str(cm.exception)
        self.assertIn("REFUS (G1)", msg)
        self.assertIn("EntityId non listes", msg)

    def test_mutation_sans_les_ids(self):
        """R2 mutation : remettre l'ancien message (nombre seul) doit faire rougir R2."""
        ns = load_source(mutated_source([(
            "g1_pair_detail(d, ids)",
            '''f"app {d['app']} {str(d['g']).upper()} ({d['n']} entites)"''',
        )]))
        mod = type("M", (), ns)
        mod.CURRENT_PLAN = self.mod.CURRENT_PLAN
        mod.CURRENT_FR_PLAN = []
        dup = [{"app": 33, "g": "aaaaaaaa-0000-0000-0000-000000000001", "n": 2}]
        with self.assertRaises(SystemExit) as cm:
            self.run_fetch_with_ids(mod, dup, eid_rows=[
                {"app": 33, "g": "AAAAAAAA-0000-0000-0000-000000000001", "eid": 101},
                {"app": 33, "g": "AAAAAAAA-0000-0000-0000-000000000001", "eid": 102}])
        self.assertNotIn("EntityId 101/102", str(cm.exception))  # defaut revu par R2


class G2CarteSansMarge(unittest.TestCase):
    """Vainqueur matched=0, ou 2e candidat a matched == vainqueur -> REFUS."""

    def setUp(self):
        self.mod = load_tool()

    def run_map(self, mod, val_rows):
        fake = FakeQuery(mod, [], val_rows)
        try:
            return mod.build_app_map("fake-conn", reg_rows())
        finally:
            fake.restore(mod)

    def test_saine_acceptee(self):
        out = self.run_map(self.mod, val_rows_healthy())
        self.assertEqual(out["map"], {33: 33})

    def test_matched_zero_refuse(self):
        rows = [{"app": 33, "g": r["entity_guid"], "f": r["field"], "val": "autre chose"}
                for r in reg_rows()]
        with self.assertRaises(SystemExit) as cm:
            self.run_map(self.mod, rows)
        self.assertIn("REFUS (G2)", str(cm.exception))

    def test_egalite_sans_marge_refuse(self):
        # Deux copies du meme gabarit : les DEUX disent fr = registre (matched 2/2
        # chacune). L'ancien code departageait par l'overlap de GUIDs — c'est
        # exactement le tie-break silencieux que G2 interdit.
        rows = []
        for r in reg_rows():
            rows.append({"app": 33, "g": r["entity_guid"], "f": r["field"], "val": r["fr"]})
            rows.append({"app": 34, "g": r["entity_guid"], "f": r["field"], "val": r["fr"]})
        with self.assertRaises(SystemExit) as cm:
            self.run_map(self.mod, rows)
        self.assertIn("REFUS (G2)", str(cm.exception))

    def test_mutation_egalite_passe(self):
        ns = mutated_tool("G2")
        mod = type("M", (), ns)
        rows = []
        for r in reg_rows():
            rows.append({"app": 33, "g": r["entity_guid"], "f": r["field"], "val": r["fr"]})
            rows.append({"app": 34, "g": r["entity_guid"], "f": r["field"], "val": r["fr"]})
        out = self.run_map(mod, rows)  # sans la garde : PAS de refus
        self.assertEqual(out["map"], {33: 33})


class G3CarteNonInjective(unittest.TestCase):
    """Deux apps du registre mappees vers la meme app cible -> REFUS."""

    def setUp(self):
        self.mod = load_tool()

    def rows_two_apps(self):
        # Le registre porte les apps 33 et 35 ; la cible ne reconnait ses GUIDs
        # que dans l'app 33 -> les deux lignes de la carte pointent vers 33.
        rows = []
        for r in reg_rows():
            rows.append({"app": 33, "g": r["entity_guid"], "f": r["field"], "val": r["fr"]})
        rows.append({"app": 33, "g": "BBBBBBBB-0000-0000-0000-000000000001",
                     "f": "Title", "val": "Titre fr 2"})
        return rows

    def reg_two_apps(self):
        return reg_rows() + [
            {"app_id": "35", "entity_guid": "BBBBBBBB-0000-0000-0000-000000000001",
             "content_type": "News", "field": "Title", "fr": "Titre fr 2", "en": "Title en 2"}]

    def run_map(self, mod):
        fake = FakeQuery(mod, [], self.rows_two_apps())
        try:
            return mod.build_app_map("fake-conn", self.reg_two_apps())
        finally:
            fake.restore(mod)

    def test_refuse(self):
        with self.assertRaises(SystemExit) as cm:
            self.run_map(self.mod)
        self.assertIn("REFUS (G3)", str(cm.exception))

    def test_mutation_passe_et_carte_fausse(self):
        ns = mutated_tool("G3")
        mod = type("M", (), ns)
        out = self.run_map(mod)  # sans la garde : PAS de refus, carte non injective
        self.assertEqual(sorted(out["map"].values()), [33, 33])


class CarteImprimeeAvantFetch(unittest.TestCase):
    """R1 (P23-27) : la carte d'apps est a l'ecran avant que fetch_target puisse refuser."""

    def setUp(self):
        self.mod = load_tool()
        self.mod.CURRENT_PLAN = [{"app_id": 33, "entity_guid": "AAAAAAAA-0000-0000-0000-000000000001",
                                  "field": "Title", "lang": "en", "value": "Title en",
                                  "content_type": "News"}]
        self.mod.CURRENT_FR_PLAN = []
        self.dup = [{"app": 33, "g": "aaaaaaaa-0000-0000-0000-000000000001", "n": 2}]

    def run_resolve(self, mod):
        fake = FakeQuery(mod, self.dup, val_rows_healthy(), eid_rows=[
            {"app": 33, "g": "AAAAAAAA-0000-0000-0000-000000000001", "eid": 101},
            {"app": 33, "g": "AAAAAAAA-0000-0000-0000-000000000001", "eid": 102}])
        out, exc = io.StringIO(), None
        try:
            with contextlib.redirect_stdout(out):
                mod.resolve_target("fake-conn", reg_rows())
        except SystemExit as e:
            exc = e
        finally:
            fake.restore(mod)
        return out.getvalue(), exc

    def test_carte_imprimee_avant_le_refus(self):
        stdout, exc = self.run_resolve(self.mod)
        self.assertIsNotNone(exc, "G1 doit refuser ce cas")
        self.assertIn("REFUS (G1)", str(exc))
        self.assertIn("carte d'apps", stdout)
        self.assertIn("app 33 -> 33", stdout)

    def test_mutation_carte_apres_fetch(self):
        """R1 mutation : imprimer la carte apres fetch_target la rend invisible au refus."""
        before = ('    info = build_app_map(conn, reg_rows)\n'
                  '    print_app_map(info["evidence"])\n'
                  '    return info, fetch_target(conn, info["map"])')
        after = ('    info = build_app_map(conn, reg_rows)\n'
                 '    target = fetch_target(conn, info["map"])\n'
                 '    print_app_map(info["evidence"])\n'
                 '    return info, target')
        ns = load_source(mutated_source([(before, after)]))
        mod = type("M", (), ns)
        mod.CURRENT_PLAN = self.mod.CURRENT_PLAN
        mod.CURRENT_FR_PLAN = []
        stdout, exc = self.run_resolve(mod)
        self.assertIsNotNone(exc)                 # le refus tombe toujours...
        self.assertNotIn("carte d'apps", stdout)  # ...mais sans la carte : R1 rougit


class SourceSansLaRegleRefutee(unittest.TestCase):
    """R3 (P23-27) : le source ne met pas une rangee « servie » ET le ValueId MAX sur la
    meme ligne — la regle refutee par M1/M3 (c.7050026044).

    Garde de SOURCE : elle voit les formulations ecrites sur une ligne, pas une
    reformulation en deux lignes ni une implicature. C'est un detecteur des deux
    phrases historiques, pas une preuve que l'idee est absente.
    """

    # Les deux formulations explicites retirees par P23-27, telles qu'elles etaient
    # sur master avant le changement (writer PowerShell : epingle et relecture FR).
    HISTORIQUES = (
        "porter plusieurs rangees dim-0 (orphelines + servie = ValueId MAX, mesure",
        "Relecture in-tx des bases fr reecrites : la rangee SERVIE (ValueId MAX) doit",
    )

    @staticmethod
    def accuse(line):
        return bool(re.search(r"servi", line, re.IGNORECASE)) and "MAX" in line

    def accusations(self, text):
        return [(i, ln.strip()) for i, ln in enumerate(text.splitlines(), 1) if self.accuse(ln)]

    def test_source_propre(self):
        hits = self.accusations(TOOL.read_text(encoding="utf-8"))
        self.assertEqual(hits, [], f"formulation refutee remise dans le source: {hits}")

    def test_mutation_reintroduction_vue(self):
        """Re-introduire une formulation historique dans une copie du source doit etre vu."""
        marker = "# Reecritures de BASE fr (FR_DRIFT)"
        src = TOOL.read_text(encoding="utf-8")
        self.assertIn(marker, src)
        for h in self.HISTORIQUES:
            mutant = src.replace(marker, "# " + h + "\n    " + marker)
            hits = self.accusations(mutant)
            self.assertEqual(len(hits), 1, f"reintroduction non vue: {h!r} -> {hits}")


if __name__ == "__main__":
    unittest.main(verbosity=2)
