#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contrat du temoin HTTP des trois defauts servis en prod (#2398, #2397, #2181).

    python tools/test_2398-page-witness.py

POURQUOI CE FICHIER EXISTE. La contre-revue ai-01 (c.7202541558) a produit trois faux
verts sur la tete d956ab0d de la PR #2401 en ne remplacant que `fetch` : des reponses
HTTP 200 toutes en fr-FR, une page Argumentation sans corps, un lien `/legal/details`
place avant le vrai article. Les neuf constats EN sortaient OK alors que rien n'avait
ete mesure — le vrai article n'avait jamais ete charge, l'accueil de depart etait deja
francais. Un vert rendu par un instrument qui n'a rien vu est pire qu'un rouge : il
arrete la recherche.

CONDITION D'EXECUTION : std-lib seulement, OFFLINE. `run()` du temoin est execute EN
ENTIER, `fetch` remplacee par un faux qui sert un site SYNTHETIQUE dans la forme
mesuree sur le servi (attributs en quotes simples, menu `nav-fr-fr`, corps principal
dans le panneau `dnn_ContentPane`, liens de detail `/Actus/details/…`). Aucun DNS,
aucun reseau, aucune horloge : les deux environnements reels ne sont jamais demandes.

CE QUI EST EPINGLE (19 cas) :
  1. Le temoin (cas 2) : un site sain rend 70 constats, tous OK — 7 pour le francais,
     9 pour chacune des sept autres cultures — en 70 requetes ; le suivi du lien FR
     porte la preference de langue de la culture de depart, lui seul.
  2. Faux vert 1, tout en fr-FR (cas 3) : les neuf constats de chaque culture non
     francaise sont ERREUR, en nommant la langue servie et la langue attendue. Les
     sept constats francais (six pages et le detail sous Actus) restent les seuls
     verts de cette injection — et le seul detail demande est le francais.
  3. Faux vert 2, Argumentation sans corps (cas 4 a 7) : panneau vide, panneau absent
     ou reduit a un bloc d'erreur — ERREUR « corps principal absent », jamais OK. Un
     corps plein qui porte l'ancien texte reste un ECHEC : le constat detecte sa
     PRESENCE. Le panneau est lu jusqu'a SA balise fermante, pas jusqu'au premier
     `</div>` rencontre.
  4. Faux vert 3, un lien `/legal/details` avant l'article (cas 8 a 11) : le detail
     est pris SOUS le chemin Actus, le vrai article est charge, son marqueur est vu,
     le piege n'est jamais demande. Un detail qui sort du chemin, servi dans une
     autre langue, une liste sans detail ou servie ailleurs : ERREUR nomme.
  5. Le lien FR se juge en deux temps (cas 12 a 14) : le href (URL courante, prefixe
     non FR, lien absent), puis l'atterrissage (pas francais, pas la racine, non lu).
     Un point de depart servi dans une autre langue rend le lien non jugeable, et il
     n'est pas suivi.
  6. Aucune ligne ne saute (cas 15, 16) : une page non lue rend TOUJOURS ses
     constats (70 lignes), en ERREUR.
  7. Les codes de sortie (cas 17 a 19) : 0 sans defaut, 1 des qu'un constat n'est
     pas OK (ECHEC ou ERREUR), 2 sur une erreur d'instrument.
  8. Les formes attendues sont epinglees en LITTERAUX ici (cas 1 : les 8 cultures et
     leurs prefixes, les parcours, le marqueur, le debut de l'ancien texte, les deux
     environnements) : un reglage du temoin mute ne peut pas emporter le contrat
     avec lui.

CONTROLE INVERSE DE CES FIXTURES (mesure du 26/11, re-mesurable depuis le depot) :
le meme fichier de test, le temoin remplace par l'ancien blob d956ab0d --
    git show d956ab0d:docs/dnn-localization/dnn-page-witness.py > <racine>/docs/dnn-localization/dnn-page-witness.py
    cp tools/test_2398-page-witness.py <racine>/tools/
    python <racine>/tools/test_2398-page-witness.py
-- rougit 15 cas sur 19 (tous sauf le cas 1 et les cas 17 a 19). Sur un site sain,
l'ancien temoin rendait deja 70 constats OK : le rouge vient des injections, pas du
temoin.
"""
import contextlib
import importlib.util
import io
import os
import sys
import unittest
import urllib.error
import urllib.parse

HERE = os.path.dirname(os.path.abspath(__file__))
TOOL = os.path.join(HERE, "..", "docs", "dnn-localization", "dnn-page-witness.py")

# --- litteraux epingles : la forme mesuree sur le servi, pas celle du temoin
CULTURES = [
    ("fr-FR", ""),
    ("en-US", "en"),
    ("es-ES", "es"),
    ("pt-BR", "pt"),
    ("ru-RU", "ru"),
    ("zh-CN", "zh"),
    ("ar-SA", "ar"),
    ("fa-IR", "fa"),
]
PAGE_PATHS = ["", "Règles", "Argumentation", "actus", "Amis", "Téléchargements"]
MARK = "<!--2sxc:error-->"
FR_OLD = "intéresse à cet art qui a traversé les âges"
FR_LINK_CLASS = "nav-fr-fr"
PANE_ID = "dnn_ContentPane"
BASE = "https://temoin.invalid"

# Un corps sain : plus que le seuil du temoin, et sans l'ancien texte francais.
BODY_TEXT = " ".join(["contenu"] * 120)
FILLER = "<p>%s</p>" % BODY_TEXT


def load():
    """Module frais a chaque chargement : un cas peut remplacer `fetch` sans fuiter."""
    spec = importlib.util.spec_from_file_location("page_witness", TOOL)
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    return mod


def page(lang, pane_body="", fr_href="/", links="", marker=False):
    """Une page dans la forme mesuree sur le servi : attributs en quotes simples, entree
    FR du menu, corps principal dans le panneau de contenu ; `marker` plante le bloc
    d'erreur 2sxc tel qu'il est rendu (classe Warning, commentaire, litteral)."""
    block = ("<div class='dnnFormMessage dnnFormWarning'>%sError Showing Content - "
             "please login as admin for details.</div>" % MARK) if marker else ""
    menu = "" if fr_href is None else "<li class='%s'><a title='fr' href='%s'>FR</a></li>" % (FR_LINK_CLASS, fr_href)
    return ("<html lang='%s'><head><title>t</title></head><body>"
            "<nav><ul>%s</ul></nav><main><div id='%s'>%s%s%s</div></main>"
            "</body></html>" % (lang, menu, PANE_ID, block, pane_body, links))


def home_of(prefix):
    return "/" + (prefix + "/" if prefix else "")


class FakeSite(object):
    """Un site synthetique, servi par le faux `fetch` : chemin (minuscules) -> page.
    `calls` garde (chemin, preference de langue envoyee) pour chaque requete : c'est
    la preuve qu'une page a ete demandee — ou ne l'a jamais ete."""

    def __init__(self, base=BASE):
        self.base = base
        self.routes = {}
        self.fallback = None          # tout chemin sans route est servi par cette page
        self.missing = set()          # chemins qui repondent 404
        self.by_language = {}         # chemin -> {Accept-Language: chemin d'arrivee}
        self.calls = []

    def add(self, path, text, final=None):
        self.routes[path.lower()] = (final or path, 200, text)

    def serve_everything(self, text):
        """L'injection de la contre-revue : TOUTES les reponses sont cette page."""
        self.routes = dict((path, (path, 200, text)) for path in self.routes)
        self.fallback = text

    def fetch(self, opener, url, timeout, accept_language=None):
        path = urllib.parse.unquote(urllib.parse.urlsplit(url).path) or "/"
        self.calls.append((path, accept_language))
        if accept_language and path in self.by_language and accept_language in self.by_language[path]:
            path = self.by_language[path][accept_language]       # re-routage par langue du navigateur
        if path in self.missing:
            raise urllib.error.HTTPError(url, 404, "Not Found", None, None)
        if path.lower() in self.routes:
            final, status, text = self.routes[path.lower()]
            return self.base + final, status, text
        if self.fallback is not None:
            return self.base + path, 200, self.fallback
        raise urllib.error.URLError("aucune route pour %s" % path)

    def requested(self, path):
        return any(p.lower() == path.lower() for p, _ in self.calls)


def healthy():
    """Un site sans aucun des trois defauts, dans les huit cultures."""
    site = FakeSite()
    for culture, prefix in CULTURES:
        home = home_of(prefix)
        site.add(home, page(culture))
        for slug in ("Règles", "Amis", "Téléchargements"):
            site.add(home + slug, page(culture))
        # la page francaise porte legitimement le texte francais ; les autres non
        site.add(home + "Argumentation",
                 page(culture, pane_body=(FILLER + "<p>" + FR_OLD + "</p>") if culture == "fr-FR" else FILLER))
        site.add(home + "actus", page(culture, links="<a href='%sActus/details/article'>Article</a>" % home))
        site.add(home + "Actus/details/article", page(culture, pane_body=FILLER))
    return site


def constats(rows, culture):
    """Les lignes d'une culture : `en-US accueil`, `en-US actu-detail`, `depuis en-US`."""
    return [r for r in rows if r[1] == "depuis %s" % culture or r[1].startswith(culture + " ")]


class PinnedShapes(unittest.TestCase):

    def test_01_the_served_forms_are_pinned_as_literals(self):
        mod = load()
        self.assertEqual(mod.CULTURES, CULTURES)
        self.assertEqual([p for _, p in mod.PAGES], PAGE_PATHS)
        self.assertEqual((mod.MARK_2SXC, mod.MARK_TEXT), (MARK, "Error Showing Content"))
        self.assertEqual(mod.FR_BODY, FR_OLD)
        self.assertEqual((mod.ENVS["prod"], mod.ENVS["preprod"]),
                         ("https://www.argumentum.games", "https://dnn.argumentum.myia.io"))
        self.assertEqual(mod.self_test(), 0)


class Witness(unittest.TestCase):

    def run_witness(self, site):
        mod = load()
        mod.fetch = site.fetch
        # le faux `fetch` ne se sert pas de l'opener : pas de session a construire, donc
        # aucune lecture des proxys de la machine qui execute le contrat.
        mod.new_opener = lambda: None
        return mod.run("synthetique", site.base, 5)

    def only_red(self, site, pick, why):
        """Tout est OK sauf les constats que `pick` designe — un defaut injecte rougit
        SON constat et lui seul. Rend les lignes rouges."""
        rows, errors = self.run_witness(site)
        self.assertEqual(errors, [], why + " : erreur d'instrument inattendue")
        wanted = [r for r in rows if pick(r)]
        self.assertTrue(wanted, why + " : le constat vise n'existe pas")
        self.assertEqual([r for r in rows if r[2] != "OK"], wanted,
                         why + " : seuls les constats vises doivent rougir")
        return wanted

    # --- 1. le temoin
    def test_02_a_healthy_site_is_green_with_every_constat_present(self):
        site = healthy()
        rows, errors = self.run_witness(site)
        self.assertEqual(errors, [])
        self.assertEqual(len(rows), 70)
        self.assertEqual(sorted(set(r[2] for r in rows)), ["OK"])
        tally = {}
        for family, _, _, _ in rows:
            tally[family] = tally.get(family, 0) + 1
        self.assertEqual(tally, {"marqueur": 56, "ancien-texte-FR": 7, "bouton-FR": 7})
        for culture, _ in CULTURES:
            self.assertEqual(len(constats(rows, culture)), 7 if culture == "fr-FR" else 9, culture)

        # le constat renomme dit ce qu'il detecte : la presence de l'ancien texte FR.
        old = [r for r in rows if r[0] == "ancien-texte-FR"]
        self.assertEqual([r[2] for r in old], ["OK"] * 7)
        self.assertEqual([r[1] for r in old], ["%s Argumentation" % c for c, _ in CULTURES if c != "fr-FR"])
        for r in old:
            self.assertTrue(r[3].startswith("ancien texte FR absent (corps de %d caracteres" % len(BODY_TEXT)), r[3])
            self.assertNotIn("corps non francais", r[3])

        # 70 constats, 70 requetes : rien n'est demande deux fois, rien en plus.
        self.assertEqual(len(site.calls), 70)
        # le suivi du lien FR porte la preference de langue de la culture de depart, lui seul
        with_header = [(p, h) for p, h in site.calls if h]
        self.assertEqual(sorted(with_header), sorted(("/", c) for c, _ in CULTURES if c != "fr-FR"))
        # un detail par culture, sous SON chemin Actus
        details = [p for p, _ in site.calls if "details" in p.lower()]
        self.assertEqual(sorted(details),
                         sorted(home_of(p) + "Actus/details/article" for _, p in CULTURES))

    # --- 2. faux vert 1 : toutes les reponses en fr-FR
    def test_03_every_response_french_is_never_green(self):
        site = healthy()
        # la page injectee porte un lien de detail : l'ancien temoin, qui prenait « le
        # premier lien contenant details », trouvait donc a les juger — neuf OK.
        site.serve_everything(page("fr-FR", links="<a href='/actus/details/article'>Article</a>"))
        # l'injection ne porte AUCUN marqueur : un rouge ne peut venir que de la garde de langue
        self.assertNotIn(MARK, site.fallback)
        rows, errors = self.run_witness(site)
        self.assertEqual(errors, [])
        self.assertEqual(len(rows), 70)
        for culture, _ in CULTURES[1:]:
            lines = constats(rows, culture)
            self.assertEqual(len(lines), 9, culture)
            self.assertEqual([r[2] for r in lines], ["ERREUR"] * 9, culture)
            for r in lines:
                self.assertIn("fr-FR", r[3], culture)     # la langue servie est nommee
                self.assertIn(culture, r[3], culture)     # et la langue attendue aussi
            self.assertEqual(sorted(f for f, _, _, _ in lines),
                             ["ancien-texte-FR", "bouton-FR"] + ["marqueur"] * 7, culture)
            link = [r for r in lines if r[0] == "bouton-FR"]
            self.assertIn("point de depart servi en fr-FR, attendu %s" % culture, link[0][3])
            body = [r for r in lines if r[0] == "ancien-texte-FR"]
            self.assertIn("ancien texte FR non jugeable", body[0][3])
        # les sept constats francais — six pages et le detail sous /actus — sont la seule
        # chose vraie de cette injection : le temoin ne juge que ce qui est dans sa langue.
        self.assertEqual([r[2] for r in constats(rows, "fr-FR")], ["OK"] * 7)
        # un seul detail est demande : celui de la liste francaise, seule liste lue dans
        # sa propre langue.
        self.assertEqual([p for p, _ in site.calls if "details" in p.lower()], ["/actus/details/article"])

    # --- 3. faux vert 2 : Argumentation sans corps
    def test_04_argumentation_without_a_body_is_an_error_never_a_pass(self):
        cases = [
            ("panneau vide", page("en-US", pane_body="")),
            ("panneau absent, le texte est ailleurs",
             "<html lang='en-US'><head></head><body><main><p>%s</p></main></body></html>" % BODY_TEXT),
        ]
        for name, text in cases:
            with self.subTest(case=name):
                site = healthy()
                site.add("/en/Argumentation", text)
                red = self.only_red(site, lambda r: r[0] == "ancien-texte-FR" and r[1] == "en-US Argumentation", name)
                self.assertEqual([(r[0], r[2]) for r in red], [("ancien-texte-FR", "ERREUR")])
                self.assertIn("corps principal absent", red[0][3])

    def test_05_the_pane_is_read_to_its_own_closing_div(self):
        # un div imbrique court, puis le gros du corps : une lecture qui s'arrete au
        # premier </div> ne verrait que « court » et declarerait le corps absent.
        site = healthy()
        site.add("/en/Argumentation",
                 "<html lang='en-US'><body><div id='%s'><div><p>court</p></div>%s</div></body></html>"
                 % (PANE_ID, FILLER))
        rows, errors = self.run_witness(site)
        self.assertEqual(errors, [])
        self.assertEqual([r[2] for r in rows], ["OK"] * 70)
        row = [r for r in rows if r[0] == "ancien-texte-FR" and r[1] == "en-US Argumentation"][0]
        self.assertIn("(corps de %d caracteres" % (len("court ") + len(BODY_TEXT)), row[3])

    def test_06_a_body_reduced_to_the_error_block_is_absent_and_the_marker_is_seen(self):
        site = healthy()
        site.add("/en/Argumentation", page("en-US", marker=True))
        rows, errors = self.run_witness(site)
        self.assertEqual(errors, [])
        red = [r for r in rows if r[2] != "OK"]
        self.assertEqual([(r[0], r[1], r[2]) for r in red],
                         [("marqueur", "en-US Argumentation", "ECHEC"),
                          ("ancien-texte-FR", "en-US Argumentation", "ERREUR")])
        self.assertIn("corps principal absent ou vide", red[1][3])

    def test_07_the_old_french_text_present_in_a_full_body_is_still_detected(self):
        site = healthy()
        site.add("/en/Argumentation", page("en-US", pane_body=FILLER + "<p>Argumentum " + FR_OLD + "</p>"))
        red = self.only_red(site, lambda r: r[0] == "ancien-texte-FR" and r[1] == "en-US Argumentation",
                            "ancien texte present")
        self.assertEqual([(r[0], r[2]) for r in red], [("ancien-texte-FR", "ECHEC")])
        self.assertIn("ancien texte francais servi", red[0][3])

    # --- 4. faux vert 3 : le detail sous le chemin Actus
    def test_08_a_link_outside_the_actus_path_placed_first_is_never_taken(self):
        site = healthy()
        site.add("/en/actus", page("en-US", links="<a href='/legal/details/mentions'>Mentions legales</a>"
                                                       "<a href='/en/Actus/details/article'>Article</a>"))
        site.add("/en/Actus/details/article", page("en-US", pane_body=FILLER, marker=True))
        site.add("/legal/details/mentions", page("en-US", pane_body=FILLER))       # le piege : sans marqueur
        red = self.only_red(site, lambda r: r[1] == "en-US actu-detail", "lien legal avant l'article")
        self.assertEqual(red[0][2], "ECHEC")
        self.assertIn("commentaire 2sxc:error", red[0][3])
        self.assertTrue(site.requested("/en/Actus/details/article"), "le vrai article doit etre charge")
        self.assertFalse(site.requested("/legal/details/mentions"), "le piege ne doit jamais etre demande")

    def test_09_a_list_without_any_detail_under_actus_is_an_error(self):
        site = healthy()
        site.add("/en/actus", page("en-US", links="<a href='/legal/details/mentions'>Mentions legales</a>"))
        site.add("/legal/details/mentions", page("en-US", pane_body=FILLER))
        red = self.only_red(site, lambda r: r[1] == "en-US actu-detail", "aucun detail sous Actus")
        self.assertEqual(red[0][2], "ERREUR")
        self.assertIn("aucun lien de detail sous /en/actus/details", red[0][3])
        self.assertFalse(site.requested("/legal/details/mentions"))

    def test_10_a_detail_that_leaves_the_actus_path_or_the_culture_is_an_error(self):
        cases = [
            ("redirige hors du chemin", "/en/Actus/details/article", "en-US", "/legal/details/article",
             "destination hors du chemin Actus : /legal/details/article"),
            ("servi en fr-FR", "/en/Actus/details/article", "fr-FR", None,
             "detail servi en fr-FR, attendu en-US"),
        ]
        for name, path, lang, final, reason in cases:
            with self.subTest(case=name):
                site = healthy()
                site.add(path, page(lang, pane_body=FILLER), final=final)
                red = self.only_red(site, lambda r: r[1] == "en-US actu-detail", name)
                self.assertEqual(red[0][2], "ERREUR")
                self.assertIn(reason, red[0][3])

    def test_11_a_list_served_in_another_language_is_not_trusted_for_its_detail(self):
        site = healthy()
        site.add("/en/actus", page("fr-FR", links="<a href='/actus/details/piege'>Article</a>"))
        site.add("/actus/details/piege", page("fr-FR", pane_body=FILLER, marker=True))
        red = self.only_red(site, lambda r: r[1] in ("en-US actus", "en-US actu-detail"), "liste servie en fr-FR")
        self.assertEqual([(r[0], r[1], r[2]) for r in red],
                         [("marqueur", "en-US actus", "ERREUR"), ("marqueur", "en-US actu-detail", "ERREUR")])
        self.assertIn("servie en fr-FR, attendue en-US", red[0][3])
        self.assertIn("liste Actus servie en fr-FR, attendue en-US", red[1][3])
        self.assertFalse(site.requested("/actus/details/piege"),
                         "un detail choisi dans une liste d'une autre langue n'est pas demande")

    # --- 5. le lien FR : le href, puis l'atterrissage
    def test_12_the_fr_link_is_judged_on_its_href_then_on_its_landing(self):
        target = lambda r: r[0] == "bouton-FR" and r[1] == "depuis en-US"
        cases = [
            # (nom, href de l'entree FR de /en/, re-routage par langue, chemins en 404, verdict, extrait)
            ("defaut de prod : le href est l'URL courante", "/en/", {}, [], "ECHEC", "href = URL courante (/en/)"),
            ("href prefixe non FR", "/es/", {}, [], "ECHEC", "destination prefixee non FR (/es/)"),
            ("entree FR sans lien", None, {}, [], "ECHEC", "aucun lien nav-fr-fr"),
            ("atterrissage anglais malgre un href correct", "/", {"/": {"en-US": "/en/"}}, [],
             "ECHEC", "suivi -> /en/ lang=en-US"),
            ("atterrissage francais hors de la racine", "/actus", {}, [], "ECHEC",
             "suivi -> /actus : attendu la racine /"),
            ("atterrissage non lu", "/introuvable", {}, ["/introuvable"], "ECHEC",
             "suivi -> /introuvable lang=absent (HTTP 404)"),
        ]
        for name, href, by_language, missing, verdict, reason in cases:
            with self.subTest(case=name):
                site = healthy()
                site.add("/en/", page("en-US", fr_href=href))
                site.by_language.update(by_language)
                site.missing.update(missing)
                red = self.only_red(site, target, name)
                self.assertEqual(red[0][2], verdict, red[0][3])
                self.assertIn(reason, red[0][3])

    def test_13_a_correct_href_is_followed_in_the_same_session_in_french(self):
        site = healthy()
        rows, errors = self.run_witness(site)
        self.assertEqual(errors, [])
        self.assertEqual([r[2] for r in rows], ["OK"] * 70)
        link = [r for r in rows if r[0] == "bouton-FR" and r[1] == "depuis en-US"][0]
        self.assertEqual(link[3], "href / -> / lang=fr-FR")
        # la page de depart est lue, puis le lien est suivi — avec la preference de langue du depart
        # l'accueil est demande deux fois : une fois par le parcours du marqueur, une fois
        # pour lire le lien — le suivi vient juste apres la seconde lecture.
        readings = [k for k, c in enumerate(site.calls) if c == ("/en/", None)]
        self.assertEqual(len(readings), 2)
        self.assertEqual(site.calls[readings[1] + 1], ("/", "en-US"))

    def test_14_a_departure_page_in_the_wrong_language_makes_the_link_unjudgeable(self):
        site = healthy()
        site.add("/pt/", page("fr-FR", fr_href="/"))       # l'accueil pt sert deja du francais
        red = self.only_red(site, lambda r: r[1] in ("pt-BR accueil", "depuis pt-BR"), "depart en fr-FR")
        self.assertEqual([(r[0], r[1], r[2]) for r in red],
                         [("marqueur", "pt-BR accueil", "ERREUR"), ("bouton-FR", "depuis pt-BR", "ERREUR")])
        self.assertIn("point de depart servi en fr-FR, attendu pt-BR", red[1][3])
        self.assertNotIn(("/", "pt-BR"), site.calls, "un lien non jugeable n'est pas suivi")

    # --- 6. aucune ligne ne saute
    def test_15_an_unreadable_page_still_rends_its_constat(self):
        cases = [
            ("HTTP 404", "missing"),
            ("reseau", "down"),
        ]
        for name, how in cases:
            with self.subTest(case=name):
                site = healthy()
                if how == "missing":
                    site.missing.add("/en/Règles")
                else:
                    site.routes.pop("/en/règles")
                rows, errors = self.run_witness(site)
                self.assertEqual(len(rows), 70)
                self.assertEqual(len(errors), 1 if how == "down" else 0)
                red = [r for r in rows if r[2] != "OK"]
                self.assertEqual([(r[0], r[1], r[2]) for r in red], [("marqueur", "en-US Regles", "ERREUR")])
                self.assertIn("non jugeable", red[0][3])

    def test_16_an_unreadable_argumentation_and_actus_keep_their_constats(self):
        site = healthy()
        site.missing.update(["/es/Argumentation", "/es/actus"])
        rows, errors = self.run_witness(site)
        self.assertEqual(errors, [])
        self.assertEqual(len(rows), 70)
        red = [r for r in rows if r[2] != "OK"]
        self.assertEqual([(r[0], r[1], r[2]) for r in red],
                         [("marqueur", "es-ES Argumentation", "ERREUR"),
                          ("ancien-texte-FR", "es-ES Argumentation", "ERREUR"),
                          ("marqueur", "es-ES actus", "ERREUR"),
                          ("marqueur", "es-ES actu-detail", "ERREUR")])
        self.assertIn("HTTP 404", red[1][3])
        self.assertIn("liste Actus non lue (HTTP 404)", red[3][3])
        self.assertEqual([p for p, _ in site.calls if p.lower().startswith("/es/actus/details")], [])


class ExitCodes(unittest.TestCase):

    def run_main(self, rows, errors):
        """`main()` en entier : `run` est remplace, les deux environnements sont parcourus."""
        mod = load()
        mod.run = lambda env, base, timeout: (rows, errors)
        argv, sys.argv = sys.argv, ["dnn-page-witness.py", "--env", "both"]
        out = io.StringIO()
        try:
            with contextlib.redirect_stdout(out):
                code = mod.main()
        except SystemExit as e:
            code = e.code
        finally:
            sys.argv = argv
        return code, out.getvalue()

    def test_17_a_green_run_exits_zero(self):
        code, out = self.run_main([("marqueur", "en-US accueil", "OK", "HTTP 200, aucun marqueur")], [])
        self.assertEqual(code, 0)
        self.assertIn("total echecs = 0 ; erreurs d'instrument = 0", out)

    def test_18_any_constat_not_ok_exits_one(self):
        for verdict in ("ECHEC", "ERREUR"):
            with self.subTest(verdict=verdict):
                code, out = self.run_main([("marqueur", "en-US accueil", "OK", ""),
                                           ("bouton-FR", "depuis en-US", verdict, "vu")], [])
                self.assertEqual(code, 1)
                self.assertIn("total echecs = 2 ; erreurs d'instrument = 0", out)

    def test_19_an_instrument_error_exits_two(self):
        code, out = self.run_main([], ["en-US accueil : reseau"])
        self.assertEqual(code, 2)
        self.assertIn("erreurs d'instrument = 2", out)


if __name__ == "__main__":
    unittest.main(verbosity=2)
