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

RESERVES DE LA CONTRE-REVUE #2362 (ai-01 c.7131327983, P23-38) — deux cas de plus :
  R4  le fragment FROM..WHERE de g1_entity_ids est le MEME que celui de la requete
      de comptage : les ids listes sont exactement ceux qui ont ete comptes. Le
      detecteur compare les SQL captures a l'execution (ce que l'outil ENVOIE) ;
      deux mutations retirent `TransDeletedId IS NULL` d'un seul des deux cotes, une
      troisieme ajoute un JOIN a la seule liste (contre-revue po-2024 : sous une
      ancre WHERE, ce mutant restait vert), et un controle inverse verifie qu'une
      divergence hors FROM..WHERE ne rougit pas.
  R5  l'avertissement « la base a bouge entre les deux lectures » (liste plus
      courte ou plus longue que le compte) a son cas, son temoin muet quand les
      deux concordent, et sa mutation.
ECRASEMENT NOMME (P24-51, ne de la mesure de P24-32). Le bloc de tete dit qu'un
CONFLIT « aucune ecriture » ; la base servie porte encore l'ancienne valeur du
registre pour trois cellules corrigees en typographie. Quatre proprietes de plus :
  F1  --force ne sort du CONFLIT que la cellule NOMMEE (GUID complet, champ,
      langue) ; tout autre conflit refuse l'apply comme avant (compute_actions).
  F2  un nom qui ne designe aucune cellule du plan est un REFUS (faute de frappe) ;
      un nom dont la cellule n'est pas en conflit est rapporte sans ecriture forcee.
  F3  le writer epingle la rangee lue par le dry-run (ValueId MAX de la dimension)
      et exige qu'elle porte ENCORE la valeur mesuree, comparaison ORDINALE — une
      rangee, sinon ROLLBACK (WRITER_PS, lue ici, jamais executee).
  F4  la valeur d'avant d'un ecrasement est sauvegardee AVANT la transaction
      (`reinject-force-prior-*.json`, ecrit avant l'appel du writer) — garde de
      SOURCE, comme R3 : elle voit la place du nom, pas le contenu du fichier.

BORNE JUSQU'AU WRITER (P24-55, contre-revue po-2023 c.6093174887) — trois de plus :
  B1  l'apply borne ne transmet au writer QUE les cellules nommees : 7 traductions
      nommees -> 7 INSERT et aucune base fr, meme quand une base fr hors lot derive.
      Le writer est remplace par un CAPTEUR (il n'ecrit rien) : c'est ce que l'apply
      PASSE qui est affirme, pas un effet en base. Controle inverse : nommer en plus
      B Summary en et sa base fr rend 8 ecritures dimensionnees dont 1 FR_UPDATE.
  B2  les deux listes de cellules ecartees (`SKIP_PRESERVED_2181`, `FR_EXCLUDED`) ne
      valent que pour un perimetre NOMME : sans `--only-cell` elles ne bloquent ni le
      dry-run ni l'apply (une cellule que personne n'a nommee n'est pas une demande).
  B3  `--force` sur une cellule que `--only-cell` a exclue est un REFUS de perimetre.
  B4  quand le seul blocage est une cellule nommee qu'un reglage ecarte, le verdict et
      le refus d'apply ne parlent pas de « conflits ou cibles manquantes » — il n'y en
      a pas ; ils nomment le reglage. Le temoin de l'autre branche est le cas du
      conflit hors lot, qui epingle la phrase quand le conflit existe.

LEVIER NOMME de la base fr du bloc (P24-54). La base fr du bloc « L'argumentation »
est FR_EXCLUDED par defaut (arbitrage du 20/11) et AUCUNE option ne levait l'exclusion :
la correction du registre n'avait aucun chemin vers la base servie. `--fr-force
GUID:CHAMP` la leve pour UN nom, le sien. Neuf proprietes de plus :
  L1  sans le levier, le bloc reste FR_EXCLUDED — nomme ou pas en `--only-cell` ;
  L2  le nom complet leve l'exclusion pour cette seule base : op FR_FORCE (jamais le
      chemin FR_UPDATE), montree a blanc, GUID COMPLET au rapport ;
  L3  tout autre nom, et toute forme incomplete, est un REFUS avant toute lecture ;
  L4  exclue (`--fr-exclude`) ET nommee (`--fr-force`) sur la meme base : REFUS ;
  L5  le levier sur une base hors du perimetre `--only-cell` est un REFUS de perimetre ;
  L6  une base nommee deja egale au registre est FR_SAME : rien a reecrire ;
  L7  l'apply transmet UN FR_FORCE avec sa valeur d'avant, et ecrit
      `reinject-fr-force-prior-*.json` AVANT l'appel du writer (capteur) ;
  L8  la branche FR_FORCE du writer porte la discipline de `--force` (source LUE), et
      la garde « #2181 identique » n'accepte que LE nom, UNE fois, sur la rangee du
      dry-run, la base valant le registre — cette garde est aussi JOUEE : ses lignes de
      decision sont extraites du script et executees sous PowerShell, un payload factice
      a la main (le script entier, lui, exige la base) ;
  L9  le post-contrat de `run_writer` n'accepte la base changee que pour LE nom, UNE
      fois, la base valant le registre sur la rangee mesuree avant, #2182 intact.
"""
import contextlib
import hashlib
import importlib.util
import io
import json
import os
import pathlib
import re
import shutil
import subprocess
import tempfile
import unittest
from unittest import mock

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


def from_of(sql):
    """Fragment `FROM..WHERE` d'une requete, normalise : de « FROM » a la clause
    suivante (GROUP BY / ORDER BY / fin d'instruction). Sert a comparer deux
    requetes telles qu'elles ont ete ENVOYEES, sans epingler leur mise en forme.

    L'ancre est FROM, pas WHERE : un JOIN ajoute a une seule des deux requetes
    change l'ensemble des lignes lues sans toucher un mot du WHERE. Constat de la
    contre-revue po-2024 de #2362 — un `JOIN TsDynDataValue` sur la seule liste
    des EntityId laissait R4 vert, alors que « les ids listes sont exactement ceux
    qu'elle a comptes » (docstring de `g1_entity_ids`) ne tenait plus.

    Limite : c'est la PREMIERE clause FROM de l'instruction qui est lue, et jusqu'a
    GROUP BY / ORDER BY / fin. Une sous-requete dans le SELECT deroberait l'ancre a
    la sienne ; aucune des deux requetes n'en porte. Detecteur des SQL envoyees, pas
    une preuve d'equivalence des resultats.
    """
    m = re.search(r"\bFROM\b(.*?)(?:\bGROUP BY\b|\bORDER BY\b|;)", sql, re.S)
    assert m, f"requete sans FROM: {sql[:120]!r}"
    return " ".join(m.group(1).split())


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


class G1MemeClauseWhere(unittest.TestCase):
    """R4 : le fragment FROM..WHERE de g1_entity_ids est celui de la requete de comptage.

    « Les ids listes sont exactement ceux qu'elle a comptes » est une affirmation du
    docstring de l'outil, tenue par RIEN avant ce cas : la mutation « sans
    TransDeletedId IS NULL », appliquee a une seule des deux requetes, laissait la
    suite verte. Le detecteur compare les SQL captures a l'execution — ce que
    l'outil envoie, pas une formulation du source. L'ancre porte sur FROM et non sur
    WHERE : un JOIN ajoute a la seule liste change les lignes lues sans toucher au
    WHERE (constat po-2024, mutant ci-dessous).
    """

    def setUp(self):
        self.mod = load_tool()
        self.mod.CURRENT_PLAN = [{"app_id": 33, "entity_guid": "AAAAAAAA-0000-0000-0000-000000000001",
                                  "field": "Title", "lang": "en", "value": "Title en",
                                  "content_type": "News"}]
        self.mod.CURRENT_FR_PLAN = []
        self.dup = [{"app": 33, "g": "aaaaaaaa-0000-0000-0000-000000000001", "n": 2}]

    def clauses(self, mod):
        """(FROM..WHERE du comptage, FROM..WHERE de la liste des EntityId), captures
        a l'execution."""
        fake = FakeQuery(mod, self.dup, val_rows_healthy(), eid_rows=[
            {"app": 33, "g": "AAAAAAAA-0000-0000-0000-000000000001", "eid": 101},
            {"app": 33, "g": "AAAAAAAA-0000-0000-0000-000000000001", "eid": 102}])
        try:
            with self.assertRaises(SystemExit):
                mod.fetch_target("fake-conn", {33: 33})
        finally:
            fake.restore(mod)
        count = [s for s in fake.calls if "HAVING COUNT(DISTINCT e.EntityId) > 1" in s]
        listed = [s for s in fake.calls if "e.EntityId AS eid" in s]
        self.assertEqual((len(count), len(listed)), (1, 1), "une requete de chaque attendue")
        return from_of(count[0]), from_of(listed[0])

    def mutant(self, replacements):
        ns = load_source(mutated_source(replacements))
        mod = type("M", (), ns)
        mod.CURRENT_PLAN = self.mod.CURRENT_PLAN
        mod.CURRENT_FR_PLAN = []
        return mod

    def test_clauses_identiques_et_vivantes(self):
        compte, liste = self.clauses(self.mod)
        self.assertEqual(compte, liste)
        self.assertIn("e.TransDeletedId IS NULL", compte)
        self.assertIn("e.TransDeletedId IS NULL", liste)

    def test_mutation_sans_transdeletedid_dans_la_liste(self):
        """Retirer la clause de la SEULE liste des EntityId doit etre vu."""
        mod = self.mutant([(
            "WHERE e.TransDeletedId IS NULL AND e.AppId IN ({in_apps})\n"
            "  AND CONVERT(nvarchar(64), e.EntityGuid) IN ({in_guids});",
            "WHERE e.AppId IN ({in_apps})\n"
            "  AND CONVERT(nvarchar(64), e.EntityGuid) IN ({in_guids});",
        )])
        compte, liste = self.clauses(mod)
        self.assertNotIn("e.TransDeletedId IS NULL", liste)
        self.assertNotEqual(compte, liste)  # la mutation ne passe plus inapercue

    def test_mutation_sans_transdeletedid_dans_le_comptage(self):
        """Retirer la clause du SEUL comptage doit etre vu."""
        mod = self.mutant([(
            "WHERE e.TransDeletedId IS NULL AND e.AppId IN ({in_apps})\n"
            "  AND CONVERT(nvarchar(64), e.EntityGuid) IN ({in_guids})\nGROUP BY",
            "WHERE e.AppId IN ({in_apps})\n"
            "  AND CONVERT(nvarchar(64), e.EntityGuid) IN ({in_guids})\nGROUP BY",
        )])
        compte, liste = self.clauses(mod)
        self.assertNotIn("e.TransDeletedId IS NULL", compte)
        self.assertNotEqual(compte, liste)

    def test_mutation_join_dans_la_liste(self):
        """Un JOIN ajoute a la SEULE liste des EntityId doit etre vu (po-2024).

        Sous l'ancre WHERE de P23-38 ce mutant restait vert : le WHERE est identique
        caractere pour caractere, et pourtant la requete ne lit plus les memes lignes
        — un JOIN interne ne rend que les entites qui ont une valeur. La promesse du
        docstring de `g1_entity_ids` tombe alors sans un rouge.
        """
        mod = self.mutant([(
            "FROM TsDynDataEntity e\n"
            "WHERE e.TransDeletedId IS NULL AND e.AppId IN ({in_apps})\n"
            "  AND CONVERT(nvarchar(64), e.EntityGuid) IN ({in_guids});",
            "FROM TsDynDataEntity e\n"
            "JOIN TsDynDataValue v ON v.EntityId = e.EntityId\n"
            "WHERE e.TransDeletedId IS NULL AND e.AppId IN ({in_apps})\n"
            "  AND CONVERT(nvarchar(64), e.EntityGuid) IN ({in_guids});",
        )])
        compte, liste = self.clauses(mod)
        self.assertIn("JOIN TsDynDataValue", liste)
        self.assertNotIn("JOIN", compte)
        self.assertNotEqual(compte, liste)

    def test_mutation_select_seul_reste_verte(self):
        """Controle inverse de l'ancre : une divergence hors FROM..WHERE ne rougit pas.

        Sans ce cas, l'ancre FROM pourrait passer pour un « comparer toute la
        requete » — et rougirait sur une divergence legitime de colonnes lues, que
        R4 n'a jamais eu a interdire.
        """
        mod = self.mutant([(
            "COUNT(DISTINCT e.EntityId) AS n",
            "COUNT(DISTINCT e.EntityId) AS entites",
        )])
        compte, liste = self.clauses(mod)
        self.assertEqual(compte, liste)


class G1AvertissementBaseBougee(unittest.TestCase):
    """R5 : la liste des EntityId peut ne pas concorder avec le compte — le dire.

    Entre la requete de comptage et la lecture des EntityId, la base peut bouger
    (une entite supprimee, une autre apparue). `g1_pair_detail` porte la branche qui
    l'annonce ; avant ce cas, aucun test ne la couvrait, dans aucun sens.
    """

    PHRASE = "la base a bouge entre les deux lectures"

    def setUp(self):
        self.mod = load_tool()
        self.mod.CURRENT_PLAN = [{"app_id": 33, "entity_guid": "AAAAAAAA-0000-0000-0000-000000000001",
                                  "field": "Title", "lang": "en", "value": "Title en",
                                  "content_type": "News"}]
        self.mod.CURRENT_FR_PLAN = []
        self.dup = [{"app": 33, "g": "aaaaaaaa-0000-0000-0000-000000000001", "n": 2}]

    def message(self, mod, eid_rows):
        fake = FakeQuery(mod, self.dup, val_rows_healthy(), eid_rows=eid_rows)
        try:
            with self.assertRaises(SystemExit) as cm:
                mod.fetch_target("fake-conn", {33: 33})
        finally:
            fake.restore(mod)
        return str(cm.exception)

    def ids(self, *eids):
        return [{"app": 33, "g": "AAAAAAAA-0000-0000-0000-000000000001", "eid": e} for e in eids]

    def test_avertissement_quand_la_liste_est_plus_courte(self):
        msg = self.message(self.mod, self.ids(101))
        self.assertIn("EntityId 101, " + self.PHRASE, msg)

    def test_avertissement_quand_la_liste_est_plus_longue(self):
        msg = self.message(self.mod, self.ids(101, 102, 103))
        self.assertIn("EntityId 101/102/103, " + self.PHRASE, msg)

    def test_pas_d_avertissement_quand_le_compte_concorde(self):
        msg = self.message(self.mod, self.ids(101, 102))
        self.assertIn("EntityId 101/102)", msg)
        self.assertNotIn(self.PHRASE, msg)

    def test_mutation_avertissement_toujours_muet(self):
        """Remplacer la branche par `moved = ""` doit se voir : sans elle, le cas
        « liste plus courte » ci-dessus resterait vert."""
        ns = load_source(mutated_source([(
            'moved = "" if len(ids) == int(dup_row["n"]) else ", la base a bouge entre les deux lectures"',
            'moved = ""',
        )]))
        mod = type("M", (), ns)
        mod.CURRENT_PLAN = self.mod.CURRENT_PLAN
        mod.CURRENT_FR_PLAN = []
        msg = self.message(mod, self.ids(101))
        self.assertNotIn(self.PHRASE, msg)


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


class ForceNomme(unittest.TestCase):
    """F1/F2 (P24-51) : un CONFLIT ne s'ecrit que si la cellule est NOMMEE, entiere."""

    GUID = "AE1EDEFA-6F1B-4593-8230-97FA1EDF4F78"
    OLD = "alubia..."     # l'ancienne valeur du registre, encore en base
    NEW = "alubia…"       # la correction typographique (P24-32)
    KEY = (GUID, "Installation", "es")

    def setUp(self):
        self.mod = load_tool()
        self.plan = [{"app_id": 60, "entity_guid": self.GUID, "field": "Installation",
                      "lang": "es", "value": self.NEW, "content_type": "Configuration"}]
        self.target = {"existing": {(60, self.GUID, "Installation", 0): "Installation",
                                    (60, self.GUID, "Installation", 12): self.OLD},
                       "lang_dim_by_app": {60: {"es": 12}}}

    def ops(self, mod, force=None):
        return mod.compute_actions(self.plan, self.target, {60: 60}, force=force)

    def check_forcee(self, mod):
        """Ce que F1 demande : nommee => FORCE, avec sa valeur d'avant, et au plan d'ecriture."""
        (a,) = self.ops(mod, force={self.KEY})
        self.assertEqual(a["op"], "FORCE")
        self.assertEqual((a["dim"], a["db_value"], a["value"]), (12, self.OLD, self.NEW))
        self.assertEqual(mod.planned_writes([a]), [a])

    def test_sans_nom_le_conflit_refuse(self):
        (a,) = self.ops(self.mod)
        self.assertEqual(a["op"], "CONFLICT")
        self.assertEqual(self.mod.planned_writes([a]), [])

    def test_nommee_la_cellule_passe_en_force(self):
        self.check_forcee(self.mod)

    def test_un_autre_nom_ne_force_rien(self):
        autre = {(self.GUID, "Installation", "ru")}
        (a,) = self.ops(self.mod, force=autre)
        self.assertEqual(a["op"], "CONFLICT")   # la cellule es n'est pas nommee
        self.assertEqual(self.mod.unmatched_force([a], autre), sorted(autre))

    def test_mutation_f1_retombe_en_conflit(self):
        """F1 neutralisee (la clause `in force`): la meme entree donne CONFLICT — c'est
        bien `test_nommee_la_cellule_passe_en_force` qui voit la classification."""
        ns = load_source(mutated_source([(
            "elif (g.upper(), field, lang) in force:", "elif False:")]))
        (a,) = self.ops(type("M", (), ns), force={self.KEY})
        self.assertEqual(a["op"], "CONFLICT")

    def test_nom_hors_conflit_rapporte_sans_forcer(self):
        """F2 : une cellule nommee qui n'est PAS en conflit n'est pas ecrite."""
        self.target["existing"][(60, self.GUID, "Installation", 12)] = self.NEW
        (a,) = self.ops(self.mod, force={self.KEY})
        self.assertEqual(a["op"], "SKIP_SAME")
        self.assertEqual(self.mod.planned_writes([a]), [])
        self.assertEqual(self.mod.unmatched_force([a], {self.KEY}), [])

    def test_parse_force(self):
        """Le GUID doit etre COMPLET, la langue doit exister : un prefixe ne nomme rien."""
        p = self.mod.parse_force
        self.assertEqual(p(f"{self.GUID.lower()}:Installation:es"), self.KEY)
        for bad in ("AE1EDEFA:Installation:es",       # prefixe de 8 : refuse
                    f"{self.GUID}:Installation:xx",    # langue inconnue
                    f"{self.GUID}:Installation",       # deux morceaux
                    f"{self.GUID}::es",                # champ vide
                    f"{self.GUID}:A:B:es"):            # quatre morceaux
            self.assertIsNone(p(bad), bad)

    def test_cli_refuse_un_force_mal_forme_avant_toute_lecture(self):
        """Le REFUS tombe avant le registre et avant la base : rien n'est lu."""
        self.mod.read_registry = lambda path=None: self.fail("registre lu avant le refus")
        self.mod.EXP.read_connection_string = (
            lambda *a, **k: self.fail("base lue avant le refus"))
        out = io.StringIO()
        with contextlib.redirect_stdout(out):
            rc = self.mod.main(["--force", "AE1EDEFA:Installation:es"])
        self.assertEqual(rc, 1)
        self.assertIn("REFUS: --force mal forme", out.getvalue())


class WriterForce(unittest.TestCase):
    """F3/F4 (P24-51) : la branche FORCE du writer, et la sauvegarde d'avant — LUES."""

    START = '} elseif ($w.op -eq "FORCE") {'
    END = "# Reecritures de BASE fr (FR_DRIFT)"
    # Ce que la branche doit porter, formes ecrites (detecteur de source, pas une preuve
    # que le script fait ce qu'il dit — lui, seule la lane qui sert la base l'execute).
    ASSERTS = (
        "[System.StringComparison]::Ordinal",                        # comparaison exacte
        "UPDATE TsDynDataValue SET [Value]=@v WHERE ValueId=@vid",   # rangee epinglee
        "ORDER BY v.ValueId DESC",                                   # ... la MAX lue au dry-run
        "if ($n -ne 1)",                                             # une rangee, sinon throw
        "$old = $w.db_value",                                        # la valeur d'avant du PLAN
        "WITH (UPDLOCK,HOLDLOCK)",                                   # sous verrou
        "if (-not $r.Read())",                                       # la rangee epinglee est LUE...
        "aucune rangee a cette dimension",                           # ...absente = refus nomme (M1)
    )

    def block(self, text):
        i = text.index(self.START)
        return text[i:text.index(self.END, i)]

    def check(self, text):
        b = self.block(text)
        self.assertEqual([a for a in self.ASSERTS if a not in b], [])

    def test_une_seule_branche_et_conforme(self):
        ps = load_tool().WRITER_PS
        self.assertEqual(ps.count(self.START), 1)   # un seul chemin FORCE : celui qu'on lit
        self.check(ps)

    def test_mutation_comparaison_non_ordinale_rougit(self):
        ns = load_source(mutated_source([
            (", [System.StringComparison]::Ordinal)", ")")]))
        with self.assertRaises(AssertionError):
            self.check(ns["WRITER_PS"])

    def test_mutation_update_non_epinglee_rougit(self):
        ns = load_source(mutated_source([(
            "UPDATE TsDynDataValue SET [Value]=@v WHERE ValueId=@vid",
            "UPDATE TsDynDataValue SET [Value]=@v WHERE EntityId=@e")]))
        with self.assertRaises(AssertionError):
            self.check(ns["WRITER_PS"])

    def test_mutation_sans_verrou_rougit(self):
        ns = load_source(mutated_source([(
            "SELECT TOP 1 v.ValueId, CONVERT(nvarchar(max), v.[Value]) FROM TsDynDataValue v WITH (UPDLOCK,HOLDLOCK)",
            "SELECT TOP 1 v.ValueId, CONVERT(nvarchar(max), v.[Value]) FROM TsDynDataValue v")]))
        with self.assertRaises(AssertionError):
            self.check(ns["WRITER_PS"])

    def test_mutation_sans_refus_de_rangee_rougit(self):
        """M1 (verification du merger de #2373, c.7218159963) : retirer ce refus laissait
        38/38 verts. Le comportement restait sur (catch + ROLLBACK de la transaction),
        mais AUCUN cas ne fixait le refus lui-meme — donc rien n'empechait de le perdre.
        Ici le refus est retire en entier : les deux assertions de la branche tombent."""
        ns = load_source(mutated_source([(
            'if (-not $r.Read()) { $r.Close(); throw "FORCE $guid.$field dim$dim : '
            'aucune rangee a cette dimension (base modifiee depuis le dry-run?)" }',
            "# M1 : refus de rangee absente retire")]))
        with self.assertRaises(AssertionError):
            self.check(ns["WRITER_PS"])

    def check_prior_force_avant_writer(self, src):
        """F4, garde de SOURCE : le fichier des valeurs d'avant est ecrit AVANT l'appel."""
        self.assertLess(src.index("reinject-force-prior-{stamp}.json"),
                        src.index("return run_writer("))

    def test_prior_force_sauvegarde_avant_le_writer(self):
        self.check_prior_force_avant_writer(TOOL.read_text(encoding="utf-8"))

    def test_mutation_prior_force_absent_rougit(self):
        mutant = mutated_source([("reinject-force-prior-{stamp}.json", "reinject-aucun-{stamp}.json")])
        with self.assertRaises(ValueError):
            self.check_prior_force_avant_writer(mutant)


class PerimetreBorneOnlyCell(unittest.TestCase):
    """#2181 (W2-14) : `--only-cell GUID:CHAMP:LANGUE` borne l'ECRITURE, pas la mesure.

    Le fait reproduit est celui de la premiere passe W2-14 (web2, 27/11, c.7215188925) :
    sur le registre derive, la seule raison qui refusait `--apply` etait 3 CONFLIT sur
    une AUTRE entite (la regle AE1EDEFA, Installation es/ar/zh) ; le lot des 7 langues
    du bloc « L'argumentation » n'en portait aucun. `--only-cell` resserre le plan
    d'ecriture sur les cellules nommees ; la carte d'apps, la lecture de la cible (G1)
    et la preservation restent mesurees sur le registre COMPLET — un perimetre de
    lecture borne ferait perdre leur marge a la carte (G2 refuse une egalite) et
    masquerait les paires G1 hors lot.

    Tout est hors ligne : registre synthetique sur disque, `EXP.run_query` remplace par
    une cible synthetique, `EXP.read_connection_string` par une chaine factice. Aucun
    `curl`, aucune base, aucun `powershell` (les cas `--apply` s'arretent aux refus, ou
    remplacent `run_writer` par un capteur qui ne lance rien).
    """
    BODY = {"app_id": "33", "entity_guid": "8A8636A4-069A-4692-A139-4773B161931B",
            "content_type": "Content", "field": "Text"}
    B = {"app_id": "33", "entity_guid": "BBBBBBBB-0000-0000-0000-00000000000B",
         "content_type": "Content", "field": "Summary"}

    def setUp(self):
        self.mod = load_tool()
        self.tmp = pathlib.Path(tempfile.mkdtemp(prefix="onlycell-"))
        self.addCleanup(shutil.rmtree, self.tmp, True)
        self.registry = self.tmp / "eav-content.csv"
        self.write_registry()
        self.fr_report = self.tmp / "fr-report.json"
        self.log = self.TargetLog()

    # -- le registre synthetique : le bloc (7 traductions) + une AUTRE entite en
    #    conflit sur es, hors du lot. Le fr du bloc est exclu par defaut (PRESERVED).
    def write_registry(self):
        header = ("key,app_id,content_type,entity_guid,field,field_type,value_kind,"
                  "fr,en,ru,pt,es,ar,fa,zh")
        rows = []
        body = dict(self.BODY, fr="Corps du registre (adouci)")
        body.update({lang: "Bloc %s" % lang for lang in self.mod.LANGS})
        rows.append(body)
        b = dict(self.B, fr="Base B", en="B en", es="B es du registre")
        rows.append(b)
        with open(self.registry, "w", newline="", encoding="utf-8") as fh:
            fh.write(header + "\r\n")
            for r in rows:
                vals = [f"{r['entity_guid']}|{r['field']}", r["app_id"], r["content_type"],
                        r["entity_guid"], r["field"], "String", "editorial"]
                vals += [r.get(lang, "") or "" for lang in ("fr",) + tuple(self.mod.LANGS)]
                fh.write(",".join('"%s"' % v.replace('"', '""') for v in vals) + "\r\n")

    class TargetLog:
        """Cible synthetique : sert les deux entites, compte les appels, garde les SQL."""
        def __init__(self):
            self.sqls = []

        def __call__(self, conn, sql):
            self.sqls.append(sql)
            if "HAVING COUNT(DISTINCT e.EntityId) > 1" in sql:
                return []
            if "e.EntityId AS eid" in sql:
                return []
            if "FROM TsDynDataApp" in sql:
                return [{"app": 33, "zone": 3}]
            if "FROM TsDynDataDimension" in sql:
                spec = importlib.util.spec_from_file_location(
                    "eavexp_probe2", HERE / "458-eav-content-export.py")
                exp = importlib.util.module_from_spec(spec)
                spec.loader.exec_module(exp)
                return [{"id": 7 + i, "ext": ext, "zone": 3}
                        for i, ext in enumerate(sorted(exp.DIM_EXTERNAL_KEY.values()))
                        if ext != "fr-FR"]
            if "ct.Name IN" in sql and "vd.ValueId IS NULL" in sql:  # build_app_map
                return [
                    {"app": 33, "g": "8A8636A4-069A-4692-A139-4773B161931B",
                     "f": "Text", "val": "Corps du registre (adouci)"},
                    {"app": 33, "g": "BBBBBBBB-0000-0000-0000-00000000000B",
                     "f": "Summary", "val": "Base B"},
                    {"app": 2, "g": "8A8636A4-069A-4692-A139-4773B161931B",
                     "f": "Text", "val": "corps de la copie demo"},
                ]
            if "TOP 1 v.ValueId AS vid" in sql:  # preservation du bloc #2181
                return [{"vid": 27942, "val": "Corps servi (plus long)"}]
            if "CONVERT(nvarchar(64), e.EntityGuid) AS g, e.EntityId AS id" in sql:
                return []  # aucune entite News synthetique
            if "ORDER BY app, entity_guid, field, dimension_id, vid" in sql:
                return self.rows()
            raise AssertionError("SQL inattendu sous cible synthetique:\n" + sql[:200])

        def rows(self):
            # base (dim 0) des deux entites ; le fr du bloc DIFFERE du registre (exclu
            # par defaut, donc jamais compare), celui de B concorde ; B.es est en
            # CONFLIT ; toutes les traductions du bloc sont ABSENTES (7 INSERT).
            out = [
                {"app": 33, "entity_guid": "8A8636A4-069A-4692-A139-4773B161931B",
                 "entity_id": 11915, "field": "Text", "dimension_id": None, "vid": 27942,
                 "val": "Corps servi (plus long)"},
                {"app": 33, "entity_guid": "BBBBBBBB-0000-0000-0000-00000000000B",
                 "entity_id": 13001, "field": "Summary", "dimension_id": None, "vid": 500,
                 "val": "Base B"},
                {"app": 33, "entity_guid": "BBBBBBBB-0000-0000-0000-00000000000B",
                 "entity_id": 13001, "field": "Summary", "dimension_id": 9, "vid": 501,
                 "val": "B es servi, different"},
            ]
            return out

    def run_main(self, extra=()):
        self.mod.EXP.run_query = self.log
        self.mod.EXP.read_connection_string = lambda p: "Server=synthetic;Database=x"
        argv = ["--config", str(self.tmp / "web.config"), "--registry", str(self.registry),
                "--fr-report", str(self.fr_report)] + list(extra)
        buf = io.StringIO()
        with contextlib.redirect_stdout(buf):
            rc = self.mod.main(argv)
        return rc, buf.getvalue()

    def only_cells(self, which="body"):
        if which == "body":
            return [(self.BODY["entity_guid"], "Text", lang) for lang in self.mod.LANGS]
        return [(self.B["entity_guid"], "Summary", lang) for lang in ("en", "es")]

    def args_for(self, cells, **flags):
        argv = []
        for g, f, lang in cells:
            argv += ["--only-cell", f"{g}:{f}:{lang}"]
        if flags.get("include_preserved"):
            argv.append("--include-preserved-2181")
        return argv

    # -- formes -------------------------------------------------------------------
    def test_parse_only_cell_formes(self):
        p = self.mod.parse_only_cell
        self.assertEqual(p("8a8636a4-069a-4692-a139-4773b161931b:Text:en"),
                         ("8A8636A4-069A-4692-A139-4773B161931B", "Text", "en"))
        self.assertEqual(p("8A8636A4-069A-4692-A139-4773B161931B:Text:fr")[2], "fr")
        # MEME validateur que --force : 32 hex sans tirets n'est pas une cle (uuid le
        # tolérait, FORCE_GUID_RE non) — le témoin est la fonction voisine, pas une idée.
        self.assertIsNone(p("8A8636A4069A4692A1394773B161931B:Text:ru"))
        self.assertIsNone(self.mod.parse_force("8A8636A4069A4692A1394773B161931B:Text:ru"))
        self.assertEqual(self.mod.parse_force("8a8636a4-069a-4692-a139-4773b161931b:Text:ru"),
                         p("8a8636a4-069a-4692-a139-4773b161931b:Text:ru"))
        for bad in ("8A8636A4:Text:en", "8A8636A4-069A-4692-A139-4773B161931B:Text",
                    "8A8636A4-069A-4692-A139-4773B161931B:Text:de",
                    "8A8636A4-069A-4692-A139-4773B161931B::en",
                    "pas-un-guid:Text:en", ""):
            self.assertIsNone(p(bad), bad)

    def test_scope_plan_borne_et_garde_les_bases_fr_nommees(self):
        plan = [{"entity_guid": self.BODY["entity_guid"], "field": "Text", "lang": "en"},
                {"entity_guid": self.B["entity_guid"], "field": "Summary", "lang": "en"}]
        fr = [{"entity_guid": self.B["entity_guid"], "field": "Summary", "lang": "fr"}]
        kept, kept_fr, unknown = self.mod.scope_plan(
            plan, fr, {(self.B["entity_guid"], "Summary", "fr")})
        self.assertEqual(kept, [])
        self.assertEqual(kept_fr, fr)
        self.assertEqual(unknown, [])
        kept, kept_fr, unknown = self.mod.scope_plan(
            plan, fr, {(self.B["entity_guid"], "Summary", "ru")})
        self.assertEqual((kept, kept_fr), ([], []))
        self.assertEqual(unknown, [(self.B["entity_guid"], "Summary", "ru")])

    def test_les_deux_listes_se_partagent_les_ops(self):
        # Bloquantes des le dry-run : reglage oublie (levier : un drapeau) et cellule
        # jamais inscriptible (file:NNN). Montrees a blanc, refusees a l'apply : la
        # politique de base fr.
        nw = self.mod.named_cells_not_written
        pol = self.mod.named_cells_policy
        acts = [{"op": "SKIP_PRESERVED_2181"}, {"op": "SKIP_SAME"}, {"op": "INSERT"}]
        fr = [{"op": "FR_EXCLUDED"}, {"op": "FR_DB_LOCAL"}, {"op": "FR_SAME"}]
        self.assertEqual([a["op"] for a in nw(acts, fr)], ["SKIP_PRESERVED_2181", "FR_DB_LOCAL"])
        self.assertEqual([a["op"] for a in pol(acts, fr)], ["FR_EXCLUDED"])

    # -- refus de perimetre ---------------------------------------------------------
    def test_un_nom_absent_du_plan_est_refuse_avant_toute_lecture(self):
        # `ru` de B est vide au registre : le nom ne designe aucune cellule. Un
        # perimetre borne a rien rendrait « plan coherent » sans ecrire.
        rc, out = self.run_main(self.args_for(
            [(self.B["entity_guid"], "Summary", "ru")]))
        self.assertEqual(rc, 1, out)
        self.assertIn("--only-cell nomme une cellule absente du plan", out)
        self.assertEqual(self.log.sqls, [], "le refus tombe avant la connexion")

    def test_un_nom_mal_forme_est_refuse(self):
        rc, out = self.run_main(["--only-cell", "pas-un-guid:Summary:en"])
        self.assertEqual(rc, 1, out)
        self.assertIn("REFUS: --only-cell mal forme", out)
        self.assertEqual(self.log.sqls, [])

    # -- le fait de W2-14, reproduit puis leve --------------------------------------
    def test_le_conflit_hors_lot_bloque_l_apply_sans_perimetre(self):
        rc, out = self.run_main(["--include-preserved-2181"])
        self.assertEqual(rc, 1, out[-900:])
        self.assertIn("CONFLICT: 1", out)
        self.assertIn("VERDICT: ECHEC", out)
        rc, out = self.run_main(["--include-preserved-2181", "--apply",
                                 "--backup-dir", str(self.tmp / "backup")])
        self.assertEqual(rc, 1, out[-900:])
        # Le message EXACT : le refus des apps #2182 partage le prefixe
        # « REFUS d'appliquer » et suffirait a une assertion sur le prefixe.
        self.assertIn("REFUS d'appliquer: conflits ou cibles manquantes non arbitres.", out)

    def test_le_perimetre_borne_leve_le_blocage_et_ne_garde_que_ses_cellules(self):
        rc, out = self.run_main(self.args_for(self.only_cells("body"), include_preserved=True))
        self.assertEqual(rc, 0, out[-900:])
        self.assertIn("INSERT: 7", out)
        self.assertNotIn("CONFLICT", out.split("DRY-RUN")[0])
        self.assertIn("perimetre (--only-cell): 7 cellule(s) nommee(s)", out)
        self.assertIn("2 cellule(s) traduisible(s) et 2 base(s) fr HORS perimetre", out)
        self.assertIn("VERDICT: plan coherent, pret pour --apply", out)
        # Le perimetre passe les gardes : le run va jusqu'au refus de sauvegarde.
        rc, out = self.run_main(self.args_for(self.only_cells("body"),
                                              include_preserved=True) + ["--apply"])
        self.assertEqual(rc, 1, out[-600:])
        self.assertIn("REFUS: --apply exige --backup-dir", out)

    def test_le_plan_borne_ne_borne_pas_la_lecture_de_la_cible(self):
        self.run_main(self.args_for(self.only_cells("body"), include_preserved=True))
        big = [s for s in self.log.sqls
               if "ORDER BY app, entity_guid, field, dimension_id, vid" in s]
        self.assertEqual(len(big), 1)
        # L'autre entite (hors lot) est LUE : la garde G1 et la preservation la voient.
        self.assertIn(self.B["entity_guid"], big[0])
        self.assertIn(self.BODY["entity_guid"], big[0])

    def test_une_cellule_nommee_ecartee_par_defaut_refuse_l_apply(self):
        # Sans --include-preserved-2181 les 7 cellules nommees tombent en SKIP : le
        # run n'ecrirait RIEN en pretendant un plan coherent.
        rc, out = self.run_main(self.args_for(self.only_cells("body")))
        self.assertEqual(rc, 1, out[-900:])
        self.assertIn("7 cellule(s) NOMMEE(S) qu'un reglage ecarte de l'ecriture", out)
        self.assertIn("--include-preserved-2181", out)
        self.assertIn("VERDICT: ECHEC", out)
        # Le seul blocage est un REGLAGE, pas un conflit : le verdict doit le nommer,
        # sinon il envoie chercher des « conflits ou cibles manquantes » qui n'existent
        # pas (contre-revue po-2023 6093174887, §4.4). Le temoin de l'autre branche
        # est `test_le_conflit_hors_lot_bloque_l_apply_sans_perimetre`, qui epingle la
        # phrase « conflits ou cibles manquantes » quand il y a vraiment un conflit.
        self.assertIn("ECHEC (7 cellule(s) NOMMEE(S) qu'un reglage ecarte de l'ecriture "
                      "— rien a arbitrer)", out)
        self.assertNotIn("conflits ou cibles manquantes", out)
        # Sans --backup-dir : meme mute, l'apply ne peut pas atteindre le writer
        # PowerShell ; le refus epingle est celui du reglage, qui vient avant.
        rc, out = self.run_main(self.args_for(self.only_cells("body")) + ["--apply"])
        self.assertEqual(rc, 1, out[-600:])
        self.assertIn("REFUS d'appliquer: cellule(s) nommee(s) qu'un reglage ecarte de "
                      "l'ecriture", out)
        self.assertNotIn("conflits ou cibles manquantes", out)

    def test_une_base_fr_hors_perimetre_n_est_pas_comparee(self):
        # Lot = B.en seulement : la base fr du BLOC (exclue par defaut) ne doit pas
        # entrer dans le perimetre, sinon une cellule NON nommee se lirait
        # « NOMMEE(S) EXCLUE(S) » et sa base fr serait comparee.
        rc, out = self.run_main(self.args_for([(self.B["entity_guid"], "Summary", "en")]))
        self.assertEqual(rc, 0, out[-900:])
        self.assertIn("INSERT: 1", out)
        self.assertNotIn("FR_EXCLUDED", out)
        # la section des bases fr est VIDE, et les 2 bases du registre sont hors lot.
        self.assertIn("base fr (dim-0) :\nperimetre (--only-cell): 1 cellule(s) nommee(s)", out)
        self.assertIn("et 2 base(s) fr HORS perimetre", out)

    # -- les bases fr nommees ------------------------------------------------------
    def append_registry(self, guid, field, **langs):
        with open(self.registry, "a", newline="", encoding="utf-8") as fh:
            vals = [f"{guid}|{field}", "33", "Content", guid, field, "String", "editorial",
                    langs.get("fr", "")]
            vals += [langs.get(lang, "") for lang in self.mod.LANGS]
            fh.write(",".join('"%s"' % v for v in vals) + "\r\n")

    def with_base_row(self, guid, field, value):
        base_rows = self.log.rows
        self.log.rows = lambda: base_rows() + [
            {"app": 33, "entity_guid": guid, "entity_id": 14001, "field": field,
             "dimension_id": None, "vid": 600, "val": value}]

    def test_une_base_fr_nommee_exclue_est_montree_a_blanc_et_refusee_a_l_apply(self):
        # La base fr du bloc #2181 est preservee par defaut (arbitrage du 20/11).
        # La NOMMER n'est pas le levier : ni `--only-cell`, ni meme
        # --include-preserved-2181 (drapeau des traductions) ne levent l'exclusion —
        # seul `--fr-force` la leve, et pour ce nom (P24-54, `LevierBaseFrBloc`).
        # A blanc : montree, et le verdict ne dit pas « pret pour --apply ». En apply :
        # refuse.
        cell = f"{self.BODY['entity_guid']}:Text:fr"
        for extra in ([], ["--include-preserved-2181"]):
            rc, out = self.run_main(["--only-cell", cell] + extra)
            self.assertEqual(rc, 0, (extra, out[-900:]))
            self.assertIn("1 cellule(s) NOMMEE(S) EXCLUE(S) par la politique de base fr", out)
            self.assertIn("FR_EXCLUDED", out)
            self.assertIn("0 cellule(s) inscriptible(s) ; 1 nommee(s) EXCLUE(S)", out)
            self.assertNotIn("pret pour --apply", out)
        rc, out = self.run_main(["--only-cell", cell, "--apply",
                                 "--backup-dir", str(self.tmp / "backup")])
        self.assertEqual(rc, 1, out[-600:])
        self.assertIn("REFUS d'appliquer: cellule(s) nommee(s) exclue(s) par la politique", out)

    def test_la_passe_de_onze_cellules_de_w2_09_se_joue_a_blanc(self):
        # W2-09 : « un mode a blanc borne aux 11 cellules (dont le francais) » — les
        # 7 langues du bloc, son francais, et les 3 cellules de P24-32. Jouee sur une
        # cible synthetique : le conflit d'une AUTRE entite ne bloque plus, les 10
        # cellules inscriptibles sont coherentes, le francais du bloc est MONTRE comme
        # exclu — et l'apply le refuse tant qu'il reste nomme.
        e = "EEEEEEEE-0000-0000-0000-00000000000E"
        self.append_registry(e, "Installation", fr="Base E", en="E en", ru="E ru", zh="E zh")
        self.with_base_row(e, "Installation", "Base E")
        cells = self.only_cells("body") + [(self.BODY["entity_guid"], "Text", "fr")] + [
            (e, "Installation", lang) for lang in ("en", "ru", "zh")]
        self.assertEqual(len(cells), 11)
        # sans le drapeau du bloc : les 7 traductions tomberaient en SKIP — refus
        rc, out = self.run_main(self.args_for(cells))
        self.assertEqual(rc, 1, out[-900:])
        self.assertIn("VERDICT: ECHEC", out)
        # a blanc, drapeau leve
        rc, out = self.run_main(self.args_for(cells, include_preserved=True))
        self.assertEqual(rc, 0, out[-900:])
        self.assertIn("INSERT: 10", out)
        self.assertNotIn("CONFLICT", out.split("DRY-RUN")[0])
        self.assertIn("perimetre (--only-cell): 11 cellule(s) nommee(s)", out)
        self.assertIn("1 cellule(s) NOMMEE(S) EXCLUE(S) par la politique de base fr", out)
        self.assertIn("10 cellule(s) inscriptible(s) ; 1 nommee(s) EXCLUE(S)", out)
        # l'apply des 11 refuse ; celui des 10 part (jusqu'au refus de sauvegarde)
        rc, out = self.run_main(self.args_for(cells, include_preserved=True) + [
            "--apply", "--backup-dir", str(self.tmp / "backup")])
        self.assertEqual(rc, 1, out[-600:])
        self.assertIn("REFUS d'appliquer: cellule(s) nommee(s) exclue(s) par la politique", out)
        ten = self.args_for([c for c in cells if c[2] != "fr"], include_preserved=True)
        rc, out = self.run_main(ten + ["--apply"])
        self.assertEqual(rc, 1, out[-600:])
        self.assertIn("REFUS: --apply exige --backup-dir", out)

    def test_une_base_fr_nommee_hors_bloc_se_reecrit(self):
        # Controle inverse : une base fr NON exclue qui derive s'ecrit bien en mode
        # borne — l'exclusion ci-dessus est celle de la politique, pas celle du mode.
        d = "DDDDDDDD-0000-0000-0000-00000000000D"
        self.append_registry(d, "Title", fr="Titre D du registre")
        self.with_base_row(d, "Title", "Titre D servi")
        rc, out = self.run_main(["--only-cell", f"{d}:Title:fr"])
        self.assertEqual(rc, 0, out[-900:])
        self.assertIn("FR_DRIFT: 1", out)
        self.assertIn("1 REECRITURE(S) de base fr", out)
        self.assertIn("VERDICT: plan coherent, pret pour --apply", out)
        rc, out = self.run_main(["--only-cell", f"{d}:Title:fr", "--apply"])
        self.assertEqual(rc, 1, out[-600:])
        self.assertIn("REFUS: --apply exige --backup-dir", out)

    def test_une_reference_file_nommee_est_refusee(self):
        # `file:NNN` = id ADAM local a la base : jamais ecrit, quel que soit le nom.
        c = "CCCCCCCC-0000-0000-0000-00000000000C"
        self.append_registry(c, "Link", fr="file:777")
        self.with_base_row(c, "Link", "file:42")
        rc, out = self.run_main(["--only-cell", f"{c}:Link:fr"])
        self.assertEqual(rc, 1, out[-900:])
        self.assertIn("FR_DB_LOCAL (reference file:NNN locale", out)

    # -- ce que l'apply borne TRANSMET au writer (contre-revue po-2023 6093174887 §3.1) --
    def capteur(self):
        """Remplace `run_writer` : garde les ecritures transmises, n'ecrit rien.

        Le writer PowerShell ne s'execute qu'avec la base servie ; la promesse du mode
        borne est ce qu'il PASSE a ce writer. Avant ces cas, aucun `--apply` de la
        suite n'atteignait `run_writer` : tous s'arretoient a un refus, et l'on
        n'affirmait donc rien de ce qui part.
        """
        vu = {"writes": None, "fr": None, "appels": 0}

        def sensor(conn, writes, fr_writes, pres_before, backup_dir, stamp, app_map):
            vu["appels"] += 1
            vu["writes"] = sorted((w["entity_guid"], w["field"], w["lang"], w["op"])
                                  for w in writes)
            vu["fr"] = sorted((w["entity_guid"], w["field"], w["lang"], w["op"])
                              for w in fr_writes)
            return 0

        original = self.mod.run_writer
        self.mod.run_writer = sensor
        self.addCleanup(setattr, self.mod, "run_writer", original)
        return vu

    def test_l_apply_borne_ne_transmet_au_writer_que_les_cellules_nommees(self):
        # La base fr de B DERIVE de la cible : hors perimetre ce serait un FR_DRIFT.
        # Le lot ne nomme que les 7 traductions du bloc -> 7 INSERT, aucune base fr.
        self.with_base_row(self.B["entity_guid"], "Summary", "Base B servie")
        vu = self.capteur()
        rc, out = self.run_main(self.args_for(self.only_cells("body"), include_preserved=True)
                                + ["--apply", "--backup-dir", str(self.tmp / "backup")])
        self.assertEqual(rc, 0, out[-900:])
        self.assertEqual(vu["appels"], 1, out[-900:])
        self.assertEqual(vu["writes"], sorted(
            (self.BODY["entity_guid"], "Text", lang, "INSERT") for lang in self.mod.LANGS))
        self.assertEqual(vu["fr"], [])

    def test_controle_inverse_nommer_la_base_et_la_traduction_transmet_huit_ecritures(self):
        # Controle inverse du cas precedent : les deux cellules NOMMEES de B partent
        # bien (8 ecritures dimensionnees, dont B Summary en ; 1 base fr). Sans lui,
        # un perimetre qui ne transmettrait RIEN passerait pour « la borne marche ».
        self.with_base_row(self.B["entity_guid"], "Summary", "Base B servie")
        vu = self.capteur()
        cells = self.only_cells("body") + [(self.B["entity_guid"], "Summary", "en"),
                                           (self.B["entity_guid"], "Summary", "fr")]
        rc, out = self.run_main(self.args_for(cells, include_preserved=True)
                                + ["--apply", "--backup-dir", str(self.tmp / "backup")])
        self.assertEqual(rc, 0, out[-900:])
        self.assertEqual(len(vu["writes"]), 8, vu["writes"])
        self.assertIn((self.B["entity_guid"], "Summary", "en", "INSERT"), vu["writes"])
        self.assertEqual(vu["fr"], [(self.B["entity_guid"], "Summary", "fr", "FR_UPDATE")])

    def test_sans_only_cell_les_reglages_hors_perimetre_ne_bloquent_pas(self):
        """§3.2 : sans `--only-cell`, SKIP_PRESERVED_2181 et FR_EXCLUDED ne bloquent rien.

        Le mode borne a ete ajoute pour qu'un conflit d'une AUTRE entite ne bloque plus ;
        il ne doit pas, a l'inverse, rendre bloquant ce qui ne l'etait pas. Cible rendue
        conforme sur son seul conflit (B Summary es) : il ne reste que 7 SKIP et 1
        FR_EXCLUDED, et le plan est coherent. Ces deux listes ne valent que pour un
        perimetre NOMME — hors mode borne, elles decrivent des cellules que personne
        n'a demandees.
        """
        base_rows = self.log.rows
        self.log.rows = lambda: [
            dict(r, val="B es du registre")
            if (r["entity_guid"] == self.B["entity_guid"] and r["field"] == "Summary"
                and r["dimension_id"] == 9) else r
            for r in base_rows()]
        rc, out = self.run_main([])
        self.assertEqual(rc, 0, out[-900:])
        self.assertIn("SKIP_PRESERVED_2181: 7", out)
        self.assertIn("FR_EXCLUDED: 1", out)
        self.assertNotIn("NOMMEE(S)", out)
        self.assertIn("VERDICT: plan coherent, pret pour --apply", out)
        # L'apply passe les deux listes : il s'arrete au dossier de sauvegarde.
        rc, out = self.run_main(["--apply"])
        self.assertEqual(rc, 1, out[-600:])
        self.assertIn("REFUS: --apply exige --backup-dir", out)

    def test_force_hors_perimetre_est_refuse(self):
        """§3.3 : `--force` sur une cellule que `--only-cell` a exclue est un contresens.

        Demander l'ecrasement d'une cellule qu'on vient de retirer du perimetre n'a pas
        de sens lisible : c'est une faute de perimetre, pas « rien a forcer ». Le refus
        tombe apres la lecture de la cible (il se calcule sur les actions), et avant
        toute ecriture.
        """
        rc, out = self.run_main(
            self.args_for([(self.B["entity_guid"], "Summary", "en")])
            + ["--force", f"{self.BODY['entity_guid']}:Text:ru"])
        self.assertEqual(rc, 1, out[-900:])
        self.assertIn("REFUS: --force nomme une cellule qui n'est pas au perimetre", out)


# ---------------------------------------------------------------------------
# P24-54 : le levier NOMME de la base fr du bloc #2181 (`--fr-force GUID:CHAMP`).
# ---------------------------------------------------------------------------
def powershell_disponibles():
    """Les shells PowerShell de la machine, le plus ancien d'abord : le writer part
    sous `powershell` (Windows PowerShell 5.1) ; `pwsh` est celui d'une CI Linux."""
    return [b for b in ("powershell", "pwsh") if shutil.which(b)]


# Le harnais qui JOUE la garde : une fonction par variante du bloc de decision, les cas
# en JSON, une ligne par (variante, cas). Les messages sont en ASCII : ce qui traverse
# le pipe ne depend pas de la page de code de la console.
HARNAIS_PS = r"""
$ErrorActionPreference = "Stop"
try { [Console]::OutputEncoding = [System.Text.Encoding]::UTF8 } catch {}
%s
$cases = ConvertFrom-Json -InputObject ([System.IO.File]::ReadAllText($env:FRFF_CASES, [System.Text.Encoding]::UTF8))
foreach ($v in $env:FRFF_VARIANTES.Split("|")) {
    foreach ($c in $cases) {
        $script:dernier = "?"
        $res = "PASSE"
        try {
            & ("Invoke-Garde-" + $v) -frwrites $c.frwrites -payload @{ expected_2181_vid = [int]$c.vid_attendu; expected_2181_value = [string]$c.valeur_attendue } -vidfr ([int]$c.vid_ecrit) -v2181 $c.valeur_lue
        } catch { $res = "THROW: " + $_.Exception.Message }
        Write-Output ("CAS|" + $v + "|" + $c.nom + "|" + $script:dernier + "|" + $res)
    }
}
foreach ($k in @("reel", "brise")) {
    $t = $null; $e = $null
    $chemin = if ($k -eq "reel") { $env:FRFF_PS } else { $env:FRFF_PS_BRISE }
    [void][System.Management.Automation.Language.Parser]::ParseInput([System.IO.File]::ReadAllText($chemin, [System.Text.Encoding]::UTF8), [ref]$t, [ref]$e)
    Write-Output ("PARSE|" + $k + "|" + @($e).Count + "|" + ((@($e) | ForEach-Object { $_.Message }) -join " / "))
}
"""


def ligne_bloc(valeur, vid=27942):
    """Une ligne de `preservation_probe` pour le bloc #2181 (meme forme que l'outil)."""
    return {"value_id": vid, "len": len(valeur),
            "sha256": hashlib.sha256(valeur.encode("utf-16-le")).hexdigest(), "value": valeur}


class LevierBaseFrBloc(unittest.TestCase):
    """L1 a L7 : `--fr-force`, du parseur au seuil du writer.

    Le banc est celui de `PerimetreBorneOnlyCell`, COMPOSE : un heritage ferait charger
    ses cas une seconde fois sous un autre nom de classe. Sa cible synthetique sert, pour
    la base fr du bloc, « Corps servi (plus long) » (ValueId 27942) ; son registre porte
    « Corps du registre (adouci) ». Le GUID du banc EST celui du bloc — la base que la
    prod sert en avance sur le registre (6709 c. contre 6687, #2181 c.7218258887).
    """
    SERVI = "Corps servi (plus long)"
    REGISTRE = "Corps du registre (adouci)"

    def setUp(self):
        self.h = PerimetreBorneOnlyCell(methodName="test_parse_only_cell_formes")
        self.h.setUp()
        self.addCleanup(self.h.doCleanups)
        self.mod = self.h.mod
        self.G = self.h.BODY["entity_guid"]
        self.NOM = f"{self.G}:Text"

    def report(self):
        return json.loads(self.h.fr_report.read_text(encoding="utf-8"))

    def capteur(self):
        """`run_writer` remplace : garde ce qui lui est TRANSMIS, et un instantane du
        dossier de sauvegarde AU MOMENT de l'appel — c'est la preuve que la valeur d'avant
        est ecrite avant le writer, pas apres."""
        vu = {"appels": 0, "fr": None, "writes": None, "fichiers": None, "backup": None}

        def sensor(conn, writes, fr_writes, pres_before, backup_dir, stamp, app_map):
            vu["appels"] += 1
            vu["fr"] = [dict(w) for w in fr_writes]
            vu["writes"] = [dict(w) for w in writes]
            vu["backup"] = pathlib.Path(backup_dir)
            vu["fichiers"] = sorted(p.name for p in vu["backup"].iterdir())
            return 0

        original = self.mod.run_writer
        self.mod.run_writer = sensor
        self.addCleanup(setattr, self.mod, "run_writer", original)
        return vu

    # -- L1, L2 -------------------------------------------------------------------
    def test_le_banc_parle_du_vrai_bloc(self):
        # Sans cette amorce, tout ce qui suit eprouverait un GUID que l'outil ne connait pas.
        self.assertEqual(self.G, self.mod.PRESERVED_2181_GUID)
        self.assertEqual(self.h.BODY["field"], self.mod.PRESERVED_2181_FIELD)
        self.assertEqual([r["val"] for r in self.h.log.rows() if r["vid"] == 27942], [self.SERVI])

    def test_sans_le_levier_le_bloc_reste_exclu(self):
        for extra in ([], ["--only-cell", f"{self.G}:Text:fr"]):
            rc, out = self.h.run_main(extra)
            self.assertIn("FR_EXCLUDED: 1", out, extra)
            self.assertNotIn("FR_FORCE", out, extra)
            self.assertNotIn("NOMMEE(S) de base fr", out, extra)
            # La ligne du dry-run ne parle pas d'un levier que personne n'a leve.
            self.assertNotIn("--fr-force", out.split("DRY-RUN")[-1].split("VERDICT")[0], extra)
            # Sans levier, le rapport ne porte AUCUNE section du levier : il reste celui
            # de master, octet pour octet.
            self.assertNotIn("fr_named", self.report(), extra)

    def test_le_nom_complet_reecrit_la_base(self):
        for nom in (self.NOM, f"{self.G.lower()}:Text"):  # un GUID ne distingue pas la casse
            rc, out = self.h.run_main(["--only-cell", f"{self.G}:Text:fr", "--fr-force", nom])
            self.assertEqual(rc, 0, out[-900:])
            self.assertIn("base fr (dim-0) :\n  FR_FORCE: 1", out)
            self.assertNotIn("FR_EXCLUDED", out)
            self.assertNotIn("NOMMEE(S) EXCLUE(S)", out)
            self.assertIn("1 REECRITURE(S) NOMMEE(S) de base fr (--fr-force)", out)
            # GUID COMPLET sur la ligne que l'operateur relit avant de nommer la base.
            self.assertIn(f"  - {self.G} Content.Text [fr] db={len(self.SERVI)}c "
                          f"-> registre={len(self.REGISTRE)}c", out)
            self.assertIn("+ 1 REECRITURE(S) de base fr (FR_DRIFT non exclues, "
                          "dont 1 NOMMEE(S) par --fr-force)", out)
            self.assertIn("VERDICT: plan coherent, pret pour --apply", out)
            self.assertEqual(self.report()["fr_named"], [{
                "guid": self.G, "key": f"{self.G}/Text", "content_type": "Content",
                "field": "Text", "len_base": len(self.SERVI), "len_registre": len(self.REGISTRE)}])
            self.assertEqual(self.report()["excluded"], [])

    # -- L3, L4, L5 : les refus ----------------------------------------------------
    def test_le_nom_faux_est_refuse_avant_toute_lecture(self):
        # Un nom JUSTE mais pas le sien : l'outil refuse, et dit lequel il attend.
        faux = {"un autre GUID": f"{self.h.B['entity_guid']}:Summary",
                "un autre champ": f"{self.G}:Summary",
                "le champ en minuscules": f"{self.G}:text"}
        for label, nom in faux.items():
            rc, out = self.h.run_main(["--fr-force", nom])
            self.assertEqual(rc, 1, (label, out[-400:]))
            self.assertIn("REFUS: --fr-force n'accepte qu'un nom, celui de la base fr du "
                          "bloc #2181", out, label)
            self.assertIn(f"({self.G} champ Text) — refuse", out, label)
        incomplets = ("8A8636A4:Text", self.G.replace("-", "") + ":Text", f"{self.G}:Text:fr",
                      f"{self.G}:", f"{self.G}::", "", ":Text")
        for nom in incomplets:
            rc, out = self.h.run_main(["--fr-force", nom])
            self.assertEqual(rc, 1, (nom, out[-300:]))
            self.assertIn("REFUS: --fr-force mal forme", out, nom)
        self.assertEqual(self.h.log.sqls, [], "aucune lecture de la cible")

    def test_exclue_et_nommee_sur_la_meme_base_refuse(self):
        rc, out = self.h.run_main(["--fr-exclude", self.NOM, "--fr-force", f"{self.G.lower()}:Text"])
        self.assertEqual(rc, 1, out[-400:])
        self.assertIn("REFUS: --fr-force et --fr-exclude nomment la meme base fr "
                      "(8A8636A4… Text) — choisir l'un des deux", out)
        self.assertEqual(self.h.log.sqls, [])

    def test_le_levier_hors_perimetre_est_refuse(self):
        # Le lot ne nomme qu'une traduction : la base fr n'y est pas. Croire avoir demande
        # sa reecriture et n'ecrire rien — meme contresens que --force hors perimetre.
        rc, out = self.h.run_main(["--only-cell", f"{self.G}:Text:en", "--include-preserved-2181",
                                   "--fr-force", self.NOM])
        self.assertEqual(rc, 1, out[-400:])
        self.assertIn("REFUS: --fr-force nomme une base fr qui n'est pas au perimetre : "
                      "8A8636A4… Text", out)
        self.assertEqual(self.h.log.sqls, [], "le refus tombe avant la connexion")
        # Controle inverse : la base nommee au perimetre, le levier passe.
        rc, out = self.h.run_main(["--only-cell", f"{self.G}:Text:fr", "--only-cell",
                                   f"{self.G}:Text:en", "--include-preserved-2181",
                                   "--fr-force", self.NOM])
        self.assertEqual(rc, 0, out[-900:])
        self.assertIn("INSERT: 1", out)
        self.assertIn("FR_FORCE: 1", out)
        self.assertIn("VERDICT: plan coherent, pret pour --apply", out)

    # -- L6 ------------------------------------------------------------------------
    def test_une_base_deja_egale_au_registre_rien_a_reecrire(self):
        rows = self.h.log.rows
        self.h.log.rows = lambda: [dict(r, val=self.REGISTRE) if r["vid"] == 27942 else r
                                   for r in rows()]
        vu = self.capteur()
        rc, out = self.h.run_main(["--only-cell", f"{self.G}:Text:fr", "--fr-force", self.NOM,
                                   "--apply", "--backup-dir", str(self.h.tmp / "backup")])
        self.assertEqual(rc, 0, out[-900:])
        self.assertIn("FR_SAME: 1", out)
        self.assertNotIn("FR_FORCE", out)
        self.assertNotIn("NOMMEE(S) de base fr", out)
        self.assertEqual(vu["fr"], [])
        self.assertEqual([n for n in vu["fichiers"] if "fr-force-prior" in n], [])

    # -- L7 ------------------------------------------------------------------------
    def test_l_apply_transmet_un_fr_force_et_sauvegarde_l_avant_avant_le_writer(self):
        vu = self.capteur()
        rc, out = self.h.run_main(["--only-cell", f"{self.G}:Text:fr", "--fr-force", self.NOM,
                                   "--apply", "--backup-dir", str(self.h.tmp / "backup")])
        self.assertEqual(rc, 0, out[-900:])
        self.assertEqual(vu["appels"], 1, out[-600:])
        self.assertEqual(vu["writes"], [])
        (w,) = vu["fr"]
        self.assertEqual({k: w[k] for k in ("entity_guid", "field", "lang", "op", "db_value",
                                            "value", "target_app")},
                         {"entity_guid": self.G, "field": "Text", "lang": "fr", "op": "FR_FORCE",
                          "db_value": self.SERVI, "value": self.REGISTRE, "target_app": 33})
        self.assertIn("1 base(s) fr nommee(s) (--fr-force), valeur d'avant sauvegardee "
                      "+ 1 bases fr", out)
        # L'inverse se lit : la valeur d'avant, la nouvelle valeur, et la rangee mesuree.
        (nom,) = [n for n in vu["fichiers"] if n.startswith("reinject-fr-force-prior-")]
        avant = json.loads((vu["backup"] / nom).read_text(encoding="utf-8"))
        self.assertEqual(avant, [{"entity_guid": self.G, "content_type": "Content", "field": "Text",
                                  "lang": "fr", "target_app": 33, "base_value_id": 27942,
                                  "prior_value": self.SERVI, "new_value": self.REGISTRE}])

    def test_sans_le_levier_rien_de_cela_n_arrive_au_writer(self):
        vu = self.capteur()
        rc, out = self.h.run_main(["--only-cell", f"{self.G}:Text:en", "--include-preserved-2181",
                                   "--apply", "--backup-dir", str(self.h.tmp / "backup")])
        self.assertEqual(rc, 0, out[-900:])
        self.assertEqual(vu["fr"], [])
        self.assertEqual([n for n in vu["fichiers"] if "fr-force-prior" in n], [])
        self.assertNotIn("base(s) fr nommee(s)", out)


class WriterFrForce(unittest.TestCase):
    """L8 (branche) : le chemin FR_FORCE du writer, LU — il exige la base pour tourner.

    Il porte la discipline de `--force` (branche de P24-51), sur la rangee de BASE :
    la MAX lue au dry-run, sous verrou, la valeur d'avant comparee OCTETALEMENT, une
    rangee, sinon ROLLBACK — et il ne tombe JAMAIS dans le chemin FR_UPDATE.
    """

    START = 'if ($w.op -eq "FR_FORCE") {'
    END = "# Epingle une seule rangee dim-0"
    ORDINAL = "[string]::Equals([string]$cur, [string]$old, [System.StringComparison]::Ordinal)"
    REFUS_ORDINAL = ("throw \"FR_FORCE $guid.$field : la valeur en base n'est plus celle du "
                     "dry-run - ROLLBACK\"")
    ASSERTS = (
        ORDINAL,                                                    # la valeur d'avant, octets a octets
        "UPDATE TsDynDataValue SET [Value]=@v WHERE ValueId=@vid",  # rangee epinglee par son ValueId
        "ORDER BY v.ValueId DESC",                                  # ... la MAX lue au dry-run
        "WITH (UPDLOCK,HOLDLOCK)",                                  # sous verrou
        "LEFT JOIN TsDynDataValueDimension vd ON vd.ValueId = v.ValueId",
        "AND vd.ValueId IS NULL",                                   # la rangee de BASE (dim-0)
        "if (-not $r.Read())",                                      # la rangee epinglee est LUE...
        "aucune rangee de base (dim-0)",                            # ...absente = refus nomme
        "if ($n -ne 1)",                                            # une rangee, sinon throw
        REFUS_ORDINAL,                                              # valeur d'avant differente = refus
        "$frforced++",
        "continue",                                                 # jamais le chemin FR_UPDATE ensuite
    )

    def block(self, text):
        self.assertEqual(text.count(self.START), 1)   # un seul chemin FR_FORCE
        i = text.index(self.START)
        return text[i:text.index(self.END, i)]

    def check(self, text):
        b = self.block(text)
        self.assertEqual([a for a in self.ASSERTS if a not in b], [])
        self.assertEqual(b.count("continue"), 1)
        self.assertNotIn("@old", b)   # pas l'egalite SQL du chemin FR_UPDATE
        # Apres la resolution de l'entite (le G1 in-tx vaut pour elle aussi), avant
        # l'epingle du chemin FR_UPDATE.
        self.assertLess(text.index('throw "FR G1 in-tx: plusieurs entites'), text.index(self.START))
        self.assertLess(text.index(self.END),
                        text.index("SELECT MAX(v.ValueId) FROM TsDynDataValue v WITH (UPDLOCK,HOLDLOCK)"))

    def test_une_seule_branche_fr_force_et_conforme(self):
        self.check(load_tool().WRITER_PS)

    def test_mutation_fr_force_non_ordinale_rougit(self):
        ns = load_source(mutated_source([(self.ORDINAL, "($cur -eq $old)")]))
        with self.assertRaises(AssertionError):
            self.check(ns["WRITER_PS"])

    def test_mutation_fr_force_update_non_epinglee_rougit(self):
        ns = load_source(mutated_source([(
            "UPDATE TsDynDataValue SET [Value]=@v WHERE ValueId=@vid",
            "UPDATE TsDynDataValue SET [Value]=@v WHERE EntityId=@e")]))
        with self.assertRaises(AssertionError):
            self.check(ns["WRITER_PS"])

    def test_mutation_fr_force_sans_verrou_rougit(self):
        ns = load_source(mutated_source([(
            "SELECT TOP 1 v.ValueId, CONVERT(nvarchar(max), v.[Value]) FROM TsDynDataValue v WITH (UPDLOCK,HOLDLOCK)",
            "SELECT TOP 1 v.ValueId, CONVERT(nvarchar(max), v.[Value]) FROM TsDynDataValue v")]))
        with self.assertRaises(AssertionError):
            self.check(ns["WRITER_PS"])

    def test_mutation_fr_force_hors_base_rougit(self):
        # Sans `vd.ValueId IS NULL`, la rangee epinglee pourrait etre une TRADUCTION.
        ns = load_source(mutated_source([(
            "WHERE v.EntityId=@e AND v.AttributeId=@a AND vd.ValueId IS NULL",
            "WHERE v.EntityId=@e AND v.AttributeId=@a")]))
        with self.assertRaises(AssertionError):
            self.check(ns["WRITER_PS"])

    def test_mutation_fr_force_sans_refus_de_rangee_rougit(self):
        ns = load_source(mutated_source([(
            'if (-not $r.Read()) { $r.Close(); throw "FR_FORCE $guid.$field : aucune rangee de '
            'base (dim-0) — base modifiee depuis le dry-run" }',
            "# refus de rangee absente retire")]))
        with self.assertRaises(AssertionError):
            self.check(ns["WRITER_PS"])

    def test_mutation_fr_force_plusieurs_rangees_rougit(self):
        ns = load_source(mutated_source([(
            'if ($n -ne 1) { throw "FR_FORCE', 'if ($false) { throw "FR_FORCE')]))
        with self.assertRaises(AssertionError):
            self.check(ns["WRITER_PS"])

    def test_mutation_fr_force_tombe_dans_fr_update_rougit(self):
        # Sans `continue`, la base nommee prendrait AUSSI le chemin FR_UPDATE : un second
        # UPDATE sur la meme rangee, sous egalite SQL.
        ns = load_source(mutated_source([(
            "            $frforced++\n            continue\n", "            $frforced++\n")]))
        with self.assertRaises(AssertionError):
            self.check(ns["WRITER_PS"])


class Garde2181Levier(unittest.TestCase):
    """L8 (garde) : « #2181 identique » ne cede que pour LE nom, UNE fois.

    Deux instruments. Le premier LIT le script : le nom ecrit en LITTERAL, le refus
    d'avant toujours present — une fois —, la verification de rangee. Le second JOUE :
    les lignes de DECISION de la garde sont extraites de `WRITER_PS` telles quelles et
    executees sous PowerShell sur un payload factice — le cas sain, et chacun des cas
    qui doit rougir. Sans PowerShell sur la machine, le second est saute et le dit ; en
    CI, l'absence est un ECHEC : une garde sautee n'y serait jamais vue.

    Ce que le second n'etablit pas : que le VRAI script, lui, fait cela. La lecture
    (premier instrument) et le refus d'une valeur qui n'est pas celle du dry-run
    (`WriterFrForce`) sont les deux autres cotes ; le script entier exige la base.
    """

    START = '    $ff = @($frwrites | Where-Object { $_.op -eq "FR_FORCE" })'
    FIN = "    if (-not $named2181 -and ("
    NOMMEE = "    $named2181 = ("
    REFUS = ('if (-not $named2181 -and ($null -eq $v2181 -or -not ([string]$v2181 -ceq '
             '[string]$payload.expected_2181_value))) { throw "preservation #2181: valeur de base '
             'MODIFIEE (attendu identique au dry-run) - ROLLBACK" }')
    # Les quatre lignes qui lisent la base (remplacees, a l'execution, par la valeur lue).
    LECTURES = ("    $cmd.", "    [void]$cmd.", "    $v2181 = ")
    ANCIEN = "THROW: preservation #2181: valeur de base MODIFIEE (attendu identique au dry-run) - ROLLBACK"
    VALEUR = "THROW: preservation #2181 (base fr nommee) : la base ne vaut pas le registre - ROLLBACK"
    RANGEE = ("THROW: preservation #2181 (base fr nommee) : la rangee reecrite n'est pas celle "
              "du dry-run - ROLLBACK")
    AUTRE = "BBBBBBBB-0000-0000-0000-00000000000B"

    # -- ce qui est LIT --------------------------------------------------------------
    def decision(self, text):
        """Les lignes de decision de la garde, DECALEES de 4 espaces, dans l'ordre.

        Les quatre lignes qui lisent la base (`$cmd...`) sont remplacees, a l'execution,
        par la valeur lue passee en parametre. Toute autre forme entre les deux bornes —
        une deuxieme naissance du bloc, une cinquieme lecture, une decision entre le
        predicat et son bloc — est un echec : le harnais ne jouerait plus la garde lue.
        """
        lines = text.split("\n")
        debuts = [i for i, l in enumerate(lines) if l == self.START]
        fins = [i for i, l in enumerate(lines) if l.startswith(self.FIN)]
        self.assertEqual((len(debuts), len(fins)), (1, 1), "la garde doit tenir en UN bloc")
        garde, lectures, captures = [], 0, 0
        for l in lines[debuts[0]:fins[0] + 1]:
            if l.startswith(self.LECTURES):
                lectures += 1
                continue
            garde.append(l[4:])
            if l.startswith(self.NOMMEE):
                garde.append("$script:dernier = [string]$named2181")   # sonde du harnais
                captures += 1
        self.assertEqual((lectures, captures), (4, 1))
        i = next((k for k, l in enumerate(garde) if l.startswith(self.NOMMEE[4:])), None)
        self.assertIsNotNone(i, "le predicat de la garde est introuvable")
        self.assertEqual(garde[i + 2], "if ($named2181) {")
        return garde

    def check_source(self, text, mod):
        self.assertEqual(text.count(self.REFUS), 1, "le refus d'avant, une fois, inchange")
        self.assertIn("$ff.Count -eq 1 -and ", text)                  # UNE fois, pas « au moins »
        # Le nom est ecrit en LITTERAL dans le script, pas recu du payload — et ce
        # litteral est celui de la constante de l'outil (les deux ne doivent pas diverger).
        self.assertIn(f'-ieq "{mod.PRESERVED_2181_GUID}"', text)
        self.assertIn(f'-ceq "{mod.PRESERVED_2181_FIELD}"', text)
        self.assertIn("if ($vidfr -ne [int]$payload.expected_2181_vid) { throw", text)
        self.assertIn("[string]$v2181 -ceq [string]$ff[0].value", text)
        self.decision(text)
        # Apres la relecture in-tx, avant le COMMIT.
        self.assertLess(text.index("RELECTURE FR ECHEC"), text.index(self.START))
        self.assertLess(text.index(self.START), text.index("$tx.Commit()"))

    def test_la_garde_est_lue_dans_le_script(self):
        mod = load_tool()
        self.check_source(mod.WRITER_PS, mod)

    def test_mutation_le_refus_d_avant_perd_sa_condition_rougit(self):
        ns = load_source(mutated_source([("if (-not $named2181 -and (", "if ((")]))
        with self.assertRaises(AssertionError):
            self.check_source(ns["WRITER_PS"], load_tool())

    def test_mutation_le_nom_deux_fois_accepte_rougit(self):
        ns = load_source(mutated_source([("$ff.Count -eq 1 -and ", "$ff.Count -ge 1 -and ")]))
        with self.assertRaises(AssertionError):
            self.check_source(ns["WRITER_PS"], load_tool())

    def test_mutation_le_nom_litteral_diverge_de_la_constante_rougit(self):
        ns = load_source(mutated_source([(
            '-ieq "8A8636A4-069A-4692-A139-4773B161931B"',
            '-ieq "0A8636A4-069A-4692-A139-4773B161931B"')]))
        with self.assertRaises(AssertionError):
            self.check_source(ns["WRITER_PS"], load_tool())

    def test_mutation_sans_verification_de_rangee_rougit(self):
        ns = load_source(mutated_source([(
            "if ($vidfr -ne [int]$payload.expected_2181_vid) { throw", "if ($false) { throw")]))
        with self.assertRaises(AssertionError):
            self.check_source(ns["WRITER_PS"], load_tool())

    def test_mutation_le_nom_n_est_plus_verifie_rougit(self):
        ns = load_source(mutated_source([(
            '([string]$ff[0].entity_guid) -ieq "8A8636A4-069A-4692-A139-4773B161931B" -and ',
            "")]))
        with self.assertRaises(AssertionError):
            self.check_source(ns["WRITER_PS"], load_tool())

    # -- ce qui est JOUE --------------------------------------------------------------
    def cas(self, g, f):
        """(nom, frwrites, ValueId epingle par la branche, valeur lue in-tx, nomme, issue).

        La valeur attendue au dry-run est « avant » partout, la valeur du registre
        « registre ». `None` = liste absente (pas une liste vide) : PowerShell les
        distingue, la garde ne doit pas.
        """
        def ff(guid, champ):
            return {"op": "FR_FORCE", "entity_guid": guid, "field": champ, "value": "registre"}
        return [
            ("sans_FR_FORCE_base_intacte", [], 0, "avant", "False", "PASSE"),
            ("sans_FR_FORCE_liste_absente", None, 0, "avant", "False", "PASSE"),
            ("sans_FR_FORCE_base_modifiee", [], 0, "apres", "False", self.ANCIEN),
            ("une_FR_UPDATE_base_intacte", [{"op": "FR_UPDATE", "entity_guid": g, "field": f}],
             0, "avant", "False", "PASSE"),
            ("le_nom_base_egale_au_registre", [ff(g, f)], 27942, "registre", "True", "PASSE"),
            ("le_nom_en_minuscules_base_egale_au_registre", [ff(g.lower(), f)], 27942,
             "registre", "True", "PASSE"),
            ("le_nom_base_differente_du_registre", [ff(g, f)], 27942, "autre", "True", self.VALEUR),
            ("le_nom_autre_rangee", [ff(g, f)], 27940, "registre", "True", self.RANGEE),
            ("le_nom_rangee_juste_base_nulle", [ff(g, f)], 27942, None, "True", self.VALEUR),
            ("un_AUTRE_GUID", [ff(self.AUTRE, f)], 27942, "registre", "False", self.ANCIEN),
            ("le_GUID_du_bloc_un_AUTRE_champ", [ff(g, "Summary")], 27942, "registre",
             "False", self.ANCIEN),
            ("le_champ_d_une_autre_casse", [ff(g, "text")], 27942, "registre", "False", self.ANCIEN),
            ("le_nom_DEUX_fois", [ff(g, f), ff(g, f)], 27942, "registre", "False", self.ANCIEN),
            ("le_nom_ET_un_autre", [ff(g, f), ff(self.AUTRE, f)], 27942, "registre",
             "False", self.ANCIEN),
        ]

    def variantes(self, mod):
        """(nom, remplacements sur le bloc de decision, cas qui DOIT changer).

        Chaque variante retire UNE decision de la garde — dans une COPIE du bloc, le
        bloc reel reste celui qui est joue en premier. Le cas nomme est celui qui passe
        (ou echoue) autrement des qu'elle manque.
        """
        g, f = mod.PRESERVED_2181_GUID, mod.PRESERVED_2181_FIELD
        return [
            ("le_nom_deux_fois", [("$ff.Count -eq 1", "$ff.Count -ge 1")], "le_nom_DEUX_fois"),
            ("n_importe_quel_GUID", [(f'([string]$ff[0].entity_guid) -ieq "{g}"',
                                      '([string]$ff[0].entity_guid) -ieq ([string]$ff[0].entity_guid)')],
             "un_AUTRE_GUID"),
            ("champ_insensible", [(f'-ceq "{f}"', f'-ieq "{f}"')], "le_champ_d_une_autre_casse"),
            ("sans_verification_de_rangee",
             [("if ($vidfr -ne [int]$payload.expected_2181_vid) { throw", "if ($false) { throw")],
             "le_nom_autre_rangee"),
            ("sans_verification_de_valeur",
             [("if ($null -eq $v2181 -or -not ([string]$v2181 -ceq [string]$ff[0].value)) { throw",
               "if ($false) { throw")], "le_nom_base_differente_du_registre"),
            ("refus_sans_condition", [("if (-not $named2181 -and (", "if ((")],
             "le_nom_base_egale_au_registre"),
        ]

    def fonction(self, nom, lignes):
        return ("function Invoke-Garde-%s {\n    param($frwrites, $payload, $vidfr, $v2181)\n%s\n}\n"
                % (nom, "\n".join(lignes)))

    def jouer(self, ps, binaire):
        """Un processus PowerShell : toutes les variantes, tous les cas, les 2 parses."""
        mod = load_tool()
        cas = self.cas(mod.PRESERVED_2181_GUID, mod.PRESERVED_2181_FIELD)
        reel = self.decision(ps)
        texte = "\n".join(reel)
        variantes = [("reel", texte)]
        for nom, remplacements, _ in self.variantes(mod):
            t = texte
            for aiguille, remplace in remplacements:
                assert t.count(aiguille) == 1, f"aiguille de variante introuvable ou ambigue: {aiguille[:60]!r}"
                t = t.replace(aiguille, remplace)
            variantes.append((nom, t))
        d = pathlib.Path(tempfile.mkdtemp(prefix="garde2181-"))
        self.addCleanup(shutil.rmtree, d, True)
        (d / "harnais.ps1").write_text(
            HARNAIS_PS % "".join(self.fonction(n, t.split("\n")) for n, t in variantes),
            encoding="utf-8-sig")
        (d / "reel.ps1").write_text(ps, encoding="utf-8")
        (d / "brise.ps1").write_text(ps + "\n}\n", encoding="utf-8")   # une accolade de trop
        (d / "cas.json").write_text(json.dumps([
            {"nom": n, "frwrites": fr, "vid_attendu": 27942, "valeur_attendue": "avant",
             "vid_ecrit": vid, "valeur_lue": lu} for n, fr, vid, lu, _, _ in cas]), encoding="utf-8")
        env = os.environ.copy()
        env.update(FRFF_CASES=str(d / "cas.json"), FRFF_PS=str(d / "reel.ps1"),
                   FRFF_PS_BRISE=str(d / "brise.ps1"),
                   FRFF_VARIANTES="|".join(n for n, _ in variantes))
        p = subprocess.run([binaire, "-NoProfile", "-NonInteractive", "-ExecutionPolicy", "Bypass",
                            "-File", str(d / "harnais.ps1")],
                           capture_output=True, text=True, encoding="utf-8", errors="replace",
                           env=env, timeout=300)
        vus, parses = {}, {}
        for l in p.stdout.splitlines():
            c = l.split("|")
            if c[0] == "CAS" and len(c) >= 5:
                vus[(c[1], c[2])] = (c[3], "|".join(c[4:]))
            elif c[0] == "PARSE" and len(c) >= 4:
                parses[c[1]] = (int(c[2]), "|".join(c[3:]))
        self.assertEqual(p.returncode, 0, p.stderr[-800:])
        self.assertEqual(len(vus), len(cas) * len(variantes), (p.stdout[-800:], p.stderr[-800:]))
        self.assertEqual(sorted(parses), ["brise", "reel"], p.stdout[-800:])
        return vus, parses

    def test_la_garde_est_jouee_sous_powershell(self):
        mod = load_tool()
        dispo = powershell_disponibles()
        if not dispo:
            if os.environ.get("CI"):
                self.fail("CI sans PowerShell : la garde jouee ne peut pas l'etre (instrument absent)")
            self.skipTest("aucun PowerShell sur cette machine : garde LUE, pas JOUEE")
        attendu = {n: (nomme, issue) for n, _, _, _, nomme, issue in
                   self.cas(mod.PRESERVED_2181_GUID, mod.PRESERVED_2181_FIELD)}
        for binaire in dispo:
            with self.subTest(shell=binaire):
                vus, parses = self.jouer(mod.WRITER_PS, binaire)
                for n, (nomme, issue) in attendu.items():
                    self.assertEqual(vus[("reel", n)], (nomme, issue), n)
                for v, _, temoin in self.variantes(mod):
                    self.assertNotEqual(vus[(v, temoin)], vus[("reel", temoin)], (v, temoin))
                # Le script ENTIER se parse — et l'instrument voit un script casse.
                self.assertEqual(parses["reel"], (0, ""), parses["reel"])
                self.assertGreaterEqual(parses["brise"][0], 1, parses["brise"])

    def test_le_script_tient_sur_une_ligne_de_commande(self):
        # Le writer part par `-Command <script>` : Windows borne une ligne de commande a
        # 32 767 caracteres. Au dela, l'outil ne parte plus — loudement, mais sans dire pourquoi.
        self.assertLess(len(load_tool().WRITER_PS), 30_000)


class PostContratLevier(unittest.TestCase):
    """L9 : le post-contrat de `run_writer` (APRES le COMMIT), sur un writer factice.

    Le contrat general — « preservation APRES identique a AVANT » — ne cede que sur la
    base du bloc, et seulement pour LE nom, UNE fois, la base valant le registre sur la
    rangee mesuree avant, les 7 News #2182 intactes. Le script PowerShell n'est pas
    lance : `subprocess.run` est remplace, la sonde de preservation sert AVANT puis APRES.
    """

    def setUp(self):
        self.mod = load_tool()
        self.G = self.mod.PRESERVED_2181_GUID
        self.F = self.mod.PRESERVED_2181_FIELD
        self.AVANT = "Corps servi (plus long)"
        self.REGISTRE = "Corps du registre (adouci)"
        self.news = [{"guid": g.upper(), "entity_id": i + 1}
                     for i, g in enumerate(self.mod.NEWS_2182_GUIDS)]
        self.assertEqual(len(self.news), 7)
        self.tmp = pathlib.Path(tempfile.mkdtemp(prefix="postcontrat-"))
        self.addCleanup(shutil.rmtree, self.tmp, True)
        self.appels = []

    def fr_force(self, **touches):
        return {"entity_guid": self.G, "field": self.F, "lang": "fr", "op": "FR_FORCE",
                "value": self.REGISTRE, "db_value": self.AVANT, "target_app": 33,
                "content_type": "Content"} | touches

    def appliquer(self, fr_writes, apres, news_apres=None, avant_valeur=None):
        mod = self.mod
        # Les apps #2182 se resolvent par la carte, sur le plan COMPLET.
        mod.CURRENT_PLAN = [{"app_id": "33", "entity_guid": mod.NEWS_2182_GUIDS[0],
                             "field": "Title", "lang": "en"}]
        if avant_valeur is None:
            avant_valeur = self.AVANT
        avant = {"block_2181": [ligne_bloc(avant_valeur)], "news_2182": self.news}
        pile = [dict(apres, news_2182=self.news if news_apres is None else news_apres)]
        mod.preservation_probe = lambda conn, app_map=None: pile.pop(0)

        def faux_writer(cmdl, **kwargs):
            self.appels.append((cmdl[0], kwargs.get("env") or {}))
            return subprocess.CompletedProcess(cmdl, 0, stdout="COMMIT OK\n", stderr="")

        out = io.StringIO()
        with mock.patch("subprocess.run", faux_writer), contextlib.redirect_stdout(out):
            rc = mod.run_writer("Server=synthetic;Database=x", [], list(fr_writes), avant,
                                self.tmp, "20261128T000000Z", {"33": 33})
        self.assertEqual(len(pile), 0, "la sonde doit etre lue AVANT et APRES, pas plus")
        resultat = json.loads((self.tmp / "reinject-result-20261128T000000Z.json")
                              .read_text(encoding="utf-8"))
        return rc, out.getvalue(), resultat

    def test_sans_fr_force_une_base_intacte_est_conforme(self):
        rc, out, r = self.appliquer([], {"block_2181": [ligne_bloc(self.AVANT)]})
        self.assertEqual((rc, r["preservation_intact"]), (0, True))
        # Sans levier, le fichier de resultat est celui de master : aucune cle du levier.
        self.assertEqual(set(r), {"preservation_after", "preservation_intact"})
        self.assertIn("preservation APRES identique a AVANT: True", out)
        self.assertIn("VERDICT: apply termine — preservation OK", out)
        self.assertNotIn("LEVIER --fr-force", out)

    def test_sans_fr_force_une_base_changee_est_un_echec(self):
        rc, out, r = self.appliquer([], {"block_2181": [ligne_bloc("autre chose")]})
        self.assertEqual(rc, 1, out)
        self.assertEqual(r["preservation_intact"], False)
        self.assertEqual(set(r), {"preservation_after", "preservation_intact"})
        self.assertIn("VERDICT: apply termine — ⚠️ PRESERVATION A QUALIFIER", out)

    def test_le_nom_base_egale_au_registre_sur_la_rangee_du_dry_run_est_conforme(self):
        rc, out, r = self.appliquer([self.fr_force()], {"block_2181": [ligne_bloc(self.REGISTRE)]})
        self.assertEqual(rc, 0, out)
        # Honnete : la base N'EST PAS identique. Le post-contrat le dit, et dit pourquoi il l'accepte.
        self.assertEqual((r["preservation_intact"], r["base_fr_2181_reecrite_nommee"]),
                         (False, True))
        self.assertIn("preservation APRES identique a AVANT: False", out)
        self.assertIn(f"LEVIER --fr-force : base fr du bloc #2181 reecrite NOMMEMENT, comme "
                      f"demande — rangee du dry-run (ValueId 27942), apres = registre "
                      f"({len(self.REGISTRE)} c.), avant = {len(self.AVANT)} c.", out)
        self.assertIn("reinject-fr-force-prior-20261128T000000Z.json", out)
        self.assertIn("VERDICT: apply termine — preservation OK", out)

    def test_un_fr_force_d_un_autre_nom_ne_relache_rien(self):
        alien = self.fr_force(entity_guid="BBBBBBBB-0000-0000-0000-00000000000B", field="Summary",
                              value="autre chose")
        rc, out, r = self.appliquer([alien], {"block_2181": [ligne_bloc("autre chose")]})
        self.assertEqual(rc, 1, out)
        # Un FR_FORCE d'un autre nom n'est pas « le levier » : le fichier de resultat ne
        # porte meme pas la cle.
        self.assertNotIn("base_fr_2181_reecrite_nommee", r)
        self.assertNotIn("reecrite NOMMEMENT, comme demande", out)

    def test_le_nom_deux_fois_ne_relache_rien(self):
        rc, out, r = self.appliquer([self.fr_force(), self.fr_force()],
                                    {"block_2181": [ligne_bloc(self.REGISTRE)]})
        self.assertEqual(rc, 1, out)
        self.assertNotIn("base_fr_2181_reecrite_nommee", r)

    def test_base_apres_differente_du_registre_est_un_echec(self):
        rc, out, r = self.appliquer([self.fr_force()], {"block_2181": [ligne_bloc(
            self.REGISTRE[:-4])]})   # tronquee : ce n'est pas le registre
        self.assertEqual(rc, 1, out)
        self.assertIn("le post-contrat la REFUSE", out)
        self.assertEqual(r["base_fr_2181_reecrite_nommee"], False)

    def test_autre_rangee_est_un_echec(self):
        rc, out, r = self.appliquer([self.fr_force()], {"block_2181": [ligne_bloc(self.REGISTRE, 27941)]})
        self.assertEqual(rc, 1, out)
        self.assertEqual(r["base_fr_2181_reecrite_nommee"], False)

    def test_le_nom_sans_changement_de_base_est_un_echec(self):
        # Le plan nomme la base et elle n'a pas bouge : l'ecriture demandee n'a pas eu lieu.
        rc, out, r = self.appliquer([self.fr_force()], {"block_2181": [ligne_bloc(self.AVANT)]})
        self.assertEqual(rc, 1, out)
        self.assertEqual((r["preservation_intact"], r["base_fr_2181_reecrite_nommee"]), (True, False))
        self.assertIn("VERDICT: apply termine — ⚠️ PRESERVATION A QUALIFIER", out)

    def test_le_nom_une_base_deja_egale_au_registre_n_est_pas_une_reecriture(self):
        # AVANT vaut deja le registre, APRES aussi : rien n'a ete reecrit, et le post-contrat ne
        # dit pas « reecrite » sur la seule foi du plan — il compare SES deux mesures. Etat que le
        # dry-run ne produit pas (une base egale au registre est FR_SAME) : ce cas epingle la
        # comparaison AVANT/APRES elle-meme, que le cas precedent n'atteint pas.
        rc, out, r = self.appliquer([self.fr_force()], {"block_2181": [ligne_bloc(self.REGISTRE)]},
                                    avant_valeur=self.REGISTRE)
        self.assertEqual(rc, 1, out)
        self.assertEqual((r["preservation_intact"], r["base_fr_2181_reecrite_nommee"]), (True, False))
        self.assertIn("preservation APRES identique a AVANT: True", out)
        self.assertNotIn("reecrite NOMMEMENT, comme demande", out)
        self.assertIn("le post-contrat la REFUSE", out)

    def test_une_news_2182_disparue_est_un_echec(self):
        rc, out, r = self.appliquer([self.fr_force()], {"block_2181": [ligne_bloc(self.REGISTRE)]},
                                    news_apres=self.news[:6])
        self.assertEqual(rc, 1, out)
        self.assertEqual(r["base_fr_2181_reecrite_nommee"], False)

    def test_le_payload_porte_la_base_et_sa_valeur_d_avant(self):
        rc, out, r = self.appliquer([self.fr_force()], {"block_2181": [ligne_bloc(self.REGISTRE)]})
        ((binaire, env),) = self.appels
        self.assertEqual(binaire, "powershell")
        payload = json.loads(pathlib.Path(env["REINJECT_PAYLOAD"]).read_text(encoding="utf-8"))
        self.assertEqual(payload["fr_writes"], [self.fr_force()])
        self.assertEqual((payload["expected_2181_vid"], payload["expected_2181_value"]),
                         (27942, self.AVANT))


if __name__ == "__main__":
    unittest.main(verbosity=2)
