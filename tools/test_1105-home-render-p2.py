#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contrat du script du jour J : sonde P2 et reveil d'un site endormi (#131).

    python tools/test_1105-home-render-p2.py

POURQUOI CE FICHIER EXISTE. La sonde P2 du runbook (« Sondes apres la bascule »)
etait faite a la main sur les 8 accueils : `dir="rtl"` en ar et fa SEULEMENT, meta
description non vide, 7 liens de langue, marqueur d'audience >= 1. P24-45 l'ajoute au
script du jour J (`tools/1105-cutover-day-controls.py`, section 6 du rapport), et ce
contrat epingle que chaque lecture voit son defaut. P24-50 y joint le REVEIL : la prod
endormie a mis 93 a 107 s a repondre le 24/11 (rapporte par web2), au-dela de la borne
de passe de 60 s — la requete d'eveil a sa propre borne, le rapport la nomme, et un
site qui ne repond pas du tout ne fait pas tourner 42 requetes pour rien.

CONDITION D'EXECUTION : std-lib seulement, OFFLINE. Le module est charge pour ses
fonctions pures (`analyze_home`, `control_home_render`, `hard_failures`) ; le cas
bout-en-bout remplace `fetch` par un faux qui sert du HTML synthetique ET une horloge
simulee (`mod.time.monotonic`) : le serveur a demarrage a froid est un ETAT du faux
transport, pas une attente reelle. Aucun `curl` lance, aucun DNS, aucun reseau, aucune
horloge lue. La forme de la ligne de commande de `fetch` (la borne qui atteint
`--max-time`) est eprouvee separement, sur `subprocess.run` neutralise.

CE QUI EST EPINGLE :
  1. Le temoin : 8 accueils sains => les 4 lectures vertes sur les 8, et les deux
     longueurs de description de la reference du 23/11 (38 et 104 caracteres bruts)
     passent.
  2. Chaque defaut injecte rougit SON controle et lui seul : rtl sur une langue ltr,
     dir absent (une balise <html> sans dir n'est pas ltr) ou pose plus bas, description
     vide ou blanchie, meta absente, entree de langue retiree ou sans lien, active
     absente / sur la mauvaise langue / double, code de culture inconnu, marqueur
     d'audience absent.
  3. Les deux instruments de la description sont distincts : longueur BRUTE de
     l'attribut (celle de la reference, 38 a 104) et texte DECODE (celui du verdict,
     `&eacute;` = 1 caractere, pas 8). La PREMIERE <meta> est celle qui compte.
  4. Un selecteur a 0 bloc est un signal d'INSTRUMENT (classe du theme renommee) :
     rouge, et le rapport le nomme. Deux blocs doivent passer tous les deux.
  5. Un accueil non 200 n'est pas mesure (`ok=None`) : c'est deja un echec dur des
     routes, pas un second rouge de la sonde.
  6. La discipline du runbook : la sonde INFORME. Un ecart P2 laisse le code de
     sortie a 0 et se lit dans la ligne « Verdict » ; une route rouge, elle, sort 1.
     Les deux sens sont dans la meme passe.
  7. Les formes attendues sont epinglees en LITTERAUX ici (ar et fa seulement, le
     marqueur public, la classe du theme, les 8 cultures et leurs prefixes) : un
     reglage du module mute ne peut pas emporter le temoin avec lui.
  8. Le comportement de la direction, en litteraux, pour `fa` : une page `fa` servie
     en `ltr` est rouge et une page `fa` servie en `rtl` est verte (P24-50, review
     ai-01 c.7143482432 — jusqu'ici seule l'epingle des constantes voyait `fa`
     retire de `RTL_LANGS` ; ces deux cas le voient aussi).
  9. Le reveil (P24-50) : un serveur qui ne repond qu'APRES la borne de passe passe
     grace a l'eveil, et le rapport nomme statut, duree et bornes ; le meme serveur,
     sans eveil, rougit sur sa premiere route a l'ancienne borne (le controle qui
     montre que c'est bien l'eveil qui sauve la passe) ; un serveur muet sort 1 sans
     faire tourner la passe ; un serveur qui repond 503 a l'eveil n'est PAS un site
     muet — la passe le juge. `--skip-web` n'envoie aucun reveil. Bornes par defaut
     epinglees (60 / 240), et les bornes passees en ligne de commande atteignent
     `curl --max-time`.
"""
import importlib.util
import io
import os
import types
import unittest

HERE = os.path.dirname(os.path.abspath(__file__))
TOOL = os.path.join(HERE, "1105-cutover-day-controls.py")
BASE = "https://sonde.invalid"


def load():
    """Module frais a chaque chargement : un cas peut remplacer `fetch` sans fuiter."""
    spec = importlib.util.spec_from_file_location("cutover_day", TOOL)
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    return mod


MOD = load()


def selector(lang, active="auto", extra_active=None, drop=None,
             selector_class="to-shine-page-language", unlinked=None, bogus=None):
    """Le <ul> du theme servi, tel que mesure le 24/11 : 8 entrees, 1 active.

    `unlinked` : une entree presentee sans son <a> ; `bogus` : une entree dont la
    classe ne nomme aucune culture servie — deux defauts que seuls leur clause voit."""
    if active == "auto":
        active = lang
    items = []
    for other, (prefix, code) in MOD.CULTURES.items():
        if other == drop:
            continue
        tokens = ["nav-%s" % (code.lower() if other != bogus else "xx-xx")]
        if other == active:
            tokens.append("active")
        if other == extra_active:
            tokens.append("active")
        anchor = other.upper() if other == unlinked else '<a href="%s/%s">%s</a>' % (
            BASE, prefix, other.upper())
        items.append('<li class="%s">\n  %s\n</li>' % (" ".join(tokens), anchor))
    return '<ul class="%s">\n%s\n</ul>' % (selector_class, "\n".join(items))


def html_page(lang, dir_attr="auto", description=None, meta=True, marker=2,
              active="auto", extra_active=None, drop=None,
              selector_class="to-shine-page-language", unlinked=None, bogus=None,
              tail="", head_extra=""):
    """Un accueil synthetique minimal, de la forme mesuree sur le servi le 24/11."""
    code = MOD.CULTURES[lang][1]
    if dir_attr == "auto":
        dir_attr = "rtl" if lang in MOD.RTL_LANGS else "ltr"
    html_attrs = ' lang="%s"' % code
    if dir_attr is not None:
        html_attrs += ' dir="%s"' % dir_attr
    if description is None:
        description = "Description de test pour la langue %s, non vide et mesurable." % lang
    head = ['<meta charset="utf-8" />', head_extra]
    if meta:
        head.append('<meta id="MetaDescription" name="description" content="%s" />' % description)
    head.append(
        selector(lang, active=active, extra_active=extra_active, drop=drop,
                 selector_class=selector_class, unlinked=unlinked, bogus=bogus)
    )
    body = ["<p>marqueur %s</p>" % MOD.AUDIENCE_MARKER for _ in range(marker)]
    return (
        "<!DOCTYPE html>\n<html%s>\n<head>\n%s\n</head>\n<body>\n%s%s\n</body>\n</html>"
        % (html_attrs, "\n".join(head), "\n".join(body), tail)
    )


def run_rows(pages=None, statuses=None):
    """Les 8 lignes de la sonde, par langue, sur des pages synthetiques."""
    bodies, codes = {}, {}
    for lang in MOD.CULTURES:
        bodies[lang] = (pages or {}).get(lang) or html_page(lang)
        codes[lang] = (statuses or {}).get(lang, 200)
    return {r["lang"]: r for r in MOD.control_home_render(bodies, codes)}


class P2HomeRender(unittest.TestCase):
    def test_expected_shapes_are_pinned_as_literals(self):
        # Les formes attendues sont epinglees en LITTERAUX : le temoin ne partage pas
        # la surface de panne de ce qu'il garde. Un reglage du module mute ne peut
        # donc pas emporter le temoin avec lui — ar et fa SEULEMENT, le marqueur
        # public, la classe du theme, les 8 cultures et leurs prefixes.
        self.assertEqual(tuple(MOD.RTL_LANGS), ("ar", "fa"))
        self.assertEqual(MOD.AUDIENCE_MARKER, "G-VHLTL18PEW")
        self.assertEqual(MOD.LANGUAGE_SELECTOR_CLASS, "to-shine-page-language")
        self.assertEqual(MOD.CULTURES, {
            "fr": ("", "fr-FR"), "en": ("en/", "en-US"), "es": ("es/", "es-ES"),
            "pt": ("pt/", "pt-BR"), "ru": ("ru/", "ru-RU"), "zh": ("zh/", "zh-CN"),
            "ar": ("ar/", "ar-SA"), "fa": ("fa/", "fa-IR"),
        })

    def test_html_attributes_are_read_case_insensitively(self):
        # HTML ne distingue pas la casse de ses attributs : `dir="RTL"` dirigent la
        # page comme `rtl`, et `<META NAME="Description">` est la description.
        rows = run_rows({
            "ar": html_page("ar", dir_attr="RTL", meta=False,
                            head_extra='<META CONTENT=" Description en majuscules " NAME="Description">'),
            "en": html_page("en", dir_attr="LTR"),
        })
        self.assertTrue(rows["ar"]["dir_ok"])
        self.assertEqual(rows["ar"]["description_raw"], 27)
        self.assertTrue(rows["ar"]["description_ok"])
        self.assertTrue(rows["en"]["dir_ok"])
    def test_witness_eight_healthy_homes_are_green(self):
        rows = run_rows()
        self.assertEqual(sorted(rows), sorted(MOD.CULTURES))
        for lang, row in rows.items():
            for key in MOD.P2_CHECKS:
                self.assertTrue(row[key], "%s: %s" % (lang, key))
            self.assertTrue(row["ok"], lang)
            self.assertEqual(row["lang_links"], 7, lang)
            self.assertEqual(row["audience_count"], 2, lang)

    def test_the_reference_description_lengths_of_the_23rd_pass(self):
        # La reference preprod du 23/11 13:00Z va de 38 (zh) a 104 (ru) caracteres
        # bruts. Les deux bornes passent : la regle est « non vide », pas une borne.
        rows = run_rows({
            "zh": html_page("zh", description="z" * 38),
            "ru": html_page("ru", description="r" * 104),
        })
        self.assertEqual(rows["zh"]["description_raw"], 38)
        self.assertEqual(rows["ru"]["description_raw"], 104)
        self.assertTrue(rows["zh"]["description_ok"])
        self.assertTrue(rows["ru"]["description_ok"])

    def test_description_verdict_uses_decoded_text_and_report_uses_raw_length(self):
        rows = run_rows({"fr": html_page("fr", description="Caf&eacute; &amp; th&eacute;")})
        self.assertEqual(rows["fr"]["description_raw"], 28)
        self.assertEqual(rows["fr"]["description_text"], 10)
        self.assertTrue(rows["fr"]["description_ok"])

    def test_rtl_on_a_ltr_language_is_red_on_that_page_only(self):
        rows = run_rows({"fr": html_page("fr", dir_attr="rtl")})
        self.assertFalse(rows["fr"]["dir_ok"])
        self.assertFalse(rows["fr"]["ok"])
        for key in ("description_ok", "selector_ok", "audience_ok"):
            self.assertTrue(rows["fr"][key], key)
        for other in ("en", "ar", "fa"):
            self.assertTrue(rows[other]["ok"], other)

    def test_ltr_on_an_rtl_language_is_red(self):
        self.assertFalse(run_rows({"ar": html_page("ar", dir_attr="ltr")})["ar"]["dir_ok"])

    def test_fa_served_ltr_is_red_and_fa_served_rtl_is_green(self):
        # P24-50 : le cas de comportement que la review ai-01 (c.7143482432) demandait
        # a cote de l'epinglage des constantes. La page est construite en litteral
        # (`dir_attr=`), PAS derivee de RTL_LANGS comme le fait le temoin : retirer
        # `fa` de RTL_LANGS rend le premier cas vert, et cette assertion rougit.
        ltr = run_rows({"fa": html_page("fa", dir_attr="ltr")})["fa"]
        self.assertEqual(ltr["dir_expected"], "rtl")
        self.assertEqual(ltr["dir"], "ltr")
        self.assertFalse(ltr["dir_ok"], "une page fa servie en ltr doit etre rouge")
        self.assertFalse(ltr["ok"])
        rtl = run_rows({"fa": html_page("fa", dir_attr="rtl")})["fa"]
        self.assertEqual(rtl["dir"], "rtl")
        self.assertTrue(rtl["dir_ok"], "une page fa servie en rtl est verte")
        self.assertTrue(rtl["ok"])

    def test_missing_dir_is_not_ltr(self):
        row = run_rows({"fa": html_page("fa", dir_attr=None)})["fa"]
        self.assertIsNone(row["dir"])
        self.assertFalse(row["dir_ok"])

    def test_dir_is_read_on_the_html_tag_not_deeper(self):
        # Un `dir` pose plus bas (un bloc, un champ) ne dirige pas la page : il ne
        # remplace pas celui de <html>.
        rows = run_rows({
            "fr": html_page("fr", dir_attr=None, tail='<div dir="ltr">bloc</div>'),
            "ar": html_page("ar", dir_attr=None, tail='<div dir="rtl">bloc</div>'),
        })
        self.assertFalse(rows["fr"]["dir_ok"])
        self.assertFalse(rows["ar"]["dir_ok"])

    def test_empty_and_whitespace_only_descriptions_are_red(self):
        rows = run_rows({
            "es": html_page("es", description=""),
            "pt": html_page("pt", description="&nbsp; "),
        })
        self.assertFalse(rows["es"]["description_ok"])
        self.assertFalse(rows["pt"]["description_ok"])
        self.assertTrue(rows["en"]["description_ok"])

    def test_missing_meta_description_is_red(self):
        row = run_rows({"en": html_page("en", meta=False)})["en"]
        self.assertIsNone(row["description_raw"])
        self.assertFalse(row["description_ok"])

    def test_dropped_language_entry_is_red(self):
        row = run_rows({"pt": html_page("pt", drop="ru")})["pt"]
        self.assertFalse(row["selector_ok"])
        self.assertEqual(row["lang_links"], 6)
        self.assertEqual(row["selector_items"], 7)

    def test_missing_active_entry_is_red(self):
        row = run_rows({"ru": html_page("ru", active=None)})["ru"]
        self.assertFalse(row["selector_ok"])
        self.assertEqual(row["selector_active"], [])

    def test_active_on_the_wrong_language_is_red(self):
        row = run_rows({"zh": html_page("zh", active="en")})["zh"]
        self.assertFalse(row["selector_ok"])
        self.assertEqual(row["selector_active"], ["nav-en-us"])

    def test_two_active_entries_are_red(self):
        # 8 entrees, 8 liens, la bonne active : seule la clause « une seule active,
        # et c'est la mienne » voit la seconde. Le defaut de chaque clause a son cas.
        self.assertFalse(run_rows({"zh": html_page("zh", extra_active="en")})["zh"]["selector_ok"])

    def test_entry_presented_without_its_link_is_red(self):
        # 8 entrees, la bonne active, mais l'une n'est pas un lien : la clause
        # « 7 liens » est la seule a le voir.
        row = run_rows({"pt": html_page("pt", unlinked="es")})["pt"]
        self.assertEqual(row["selector_items"], 8)
        self.assertEqual(row["selector_active"], ["nav-pt-br"])
        self.assertEqual(row["lang_links"], 6)
        self.assertFalse(row["selector_ok"])

    def test_unknown_culture_code_is_red(self):
        # 8 entrees, 7 liens, la bonne active : seule la clause « les 8 cultures
        # servies » voit qu'un code ne correspond a aucune culture.
        row = run_rows({"pt": html_page("pt", bogus="es")})["pt"]
        self.assertEqual(row["selector_items"], 8)
        self.assertEqual(row["selector_active"], ["nav-pt-br"])
        self.assertEqual(row["lang_links"], 7)
        self.assertFalse(row["selector_ok"])

    def test_every_selector_block_must_pass(self):
        # Deux blocs (bureau et mobile) : un seul de casse est un ecart ; deux sains
        # passent — le temoin de « tous », contre un instrument qui lirait « un ».
        broken = run_rows({"es": html_page("es", tail=selector("es", active=None))})["es"]
        self.assertEqual(broken["selector_blocks"], 2)
        self.assertFalse(broken["selector_ok"])
        healthy = run_rows({"es": html_page("es", tail=selector("es"))})["es"]
        self.assertEqual(healthy["selector_blocks"], 2)
        self.assertTrue(healthy["selector_ok"])

    def test_first_description_meta_wins(self):
        # Deux <meta name="description"> : la PREMIERE est lue — l'ordre du document,
        # ce qu'un moteur lit en premier. Le choix est epinglé, pas subi.
        row = run_rows({"fr": html_page(
            "fr", description="Une description non vide, seconde dans le document.",
            head_extra='<meta name="description" content="" />')})["fr"]
        self.assertEqual(row["description_raw"], 0)
        self.assertFalse(row["description_ok"])

    def test_renamed_theme_class_is_a_named_instrument_signal(self):
        rows = run_rows({"zh": html_page("zh", selector_class="une-autre-classe")})
        self.assertEqual(rows["zh"]["selector_blocks"], 0)
        self.assertFalse(rows["zh"]["selector_ok"])
        md = MOD.render_md("t", BASE, "stamp", [], [], [], [], [], False, list(rows.values()))
        self.assertIn("❌ 0 bloc", md)
        self.assertIn("1 écart(s) de la sonde P2", md)

    def test_absent_audience_marker_is_red(self):
        row = run_rows({"ru": html_page("ru", marker=0)})["ru"]
        self.assertEqual(row["audience_count"], 0)
        self.assertFalse(row["audience_ok"])
        self.assertFalse(row["ok"])

    def test_non_200_home_is_unmeasured_not_red_twice(self):
        rows = run_rows(statuses={"en": 404})
        self.assertIsNone(rows["en"]["ok"])
        self.assertTrue(all(rows[l]["ok"] for l in MOD.CULTURES if l != "en"))


def fake_pages():
    """Les 28 pages que le script demande, synthetiques et suffisantes."""
    pages = {}
    for lang, (prefix, code) in MOD.CULTURES.items():
        pages["/" + prefix] = html_page(lang)
        pages["/" + prefix + "Règles"] = (
            '<html lang="%s" dir="ltr"><body><p>%s</p></body></html>'
            % (code, MOD.INTERVAL_FORMS[lang].format(4, 10))
        )
    pages["/en/terms"] = "<html><body><p>ARGUMENTUM SAS</p></body></html>"
    pages["/en/privacy"] = (
        "<html><body><p>The French version of this page is the authoritative version</p>"
        "</body></html>"
    )
    return pages


def fake_fetch(pages, statuses=None, calls=None, cold=None, clock=None):
    """Un faux transport qui sert les pages ET modele un serveur endormi.

    `calls` recoit `(url, max_time)` de chaque requete — c'est lui qui prouve quelle
    borne a atteint le transport. `cold` / `clock` modelisent le demarrage a froid :
    le serveur ne peut pas repondre avant `cold["ready_after"]` secondes apres sa
    PREMIERE requete, et une requete dont la borne est plus courte que l'attente
    rend (0, "") — exactement ce que curl fait (sortie 28, statut 000). Aucun
    `sleep` : `clock` est une liste `[t]` simulee, que `mod.time.monotonic` rend.
    """
    def fetch(url, resolve, cb, max_time=MOD.PASS_MAX_TIME):
        assert url.startswith(BASE), "le contrat ne sort pas de son faux domaine : %r" % url
        if calls is not None:
            calls.append((url, max_time))
        path = url[len(BASE):].split("?")[0]
        if clock is not None and cold is not None:
            now = clock[0]
            cold.setdefault("start", now)
            wait = max(0.0, cold["start"] + cold["ready_after"] - now)
            if wait > max_time:
                clock[0] = now + max_time
                return 0, ""
            clock[0] = now + wait
        status = (statuses or {}).get(path, 200)
        return status, (pages.get(path, "") if status == 200 else "")
    return fetch


def run_main(argv_extra=(), pages=None, statuses=None, calls=None, cold=None,
             clock=None, mutate=None):
    """`main()` en entier, hors ligne : `fetch`, l'horloge et le journal sont remplaces.

    `mutate(mod)` s'applique au module juste avant `main()` : c'est par la que le
    controle du reveil desactive l'eveil (mutation M8), au lieu de maintenir un
    drapeau `--no-wake` dans l'outil que personne n'utiliserait en production."""
    mod = load()
    mod.fetch = fake_fetch(pages if pages is not None else fake_pages(), statuses,
                           calls, cold, clock)
    if clock is not None:
        mod.time = types.SimpleNamespace(monotonic=lambda: clock[0])
    mod.control_eventlog = lambda hours: []
    if mutate is not None:
        mutate(mod)
    argv = ["1105-cutover-day-controls.py", "--base-url", BASE,
            "--skip-eventlog", "--label", "contrat"] + list(argv_extra)
    buf, err = io.StringIO(), io.StringIO()
    old = (mod.sys.argv, mod.sys.stdout, mod.sys.stderr)
    mod.sys.argv, mod.sys.stdout, mod.sys.stderr = argv, buf, err
    try:
        code = mod.main()
    finally:
        mod.sys.argv, mod.sys.stdout, mod.sys.stderr = old
    return code, buf.getvalue()


class ExitCodeDiscipline(unittest.TestCase):
    def test_a_p2_gap_leaves_the_exit_code_at_zero(self):
        pages = fake_pages()
        pages["/ru/"] = html_page("ru", marker=0)
        code, md = run_main(pages=pages)
        self.assertEqual(code, 0, md[-600:])
        self.assertIn("## 6. Sonde P2", md)
        self.assertIn("✅ tous les contrôles durs passent", md)
        self.assertIn("1 écart(s) de la sonde P2", md)

    def test_a_red_route_still_exits_one(self):
        code, md = run_main(statuses={"/en/": 404})
        self.assertEqual(code, 1, md[-600:])
        self.assertIn("échec(s) dur(s)", md)
        # L'accueil en 404 est « non mesuré » pour la sonde — du statut de l'ACCUEIL,
        # pas de la dernière surface fetchée pour cette langue.
        sec6 = md[md.index("## 6. Sonde P2"):]
        self.assertIn("| en | — | — | — | — | ⚠️ non mesuré (accueil à 404) |", sec6)
        self.assertNotIn("sonde P2**", md[md.index("**Verdict**"):])

    def test_skip_web_marks_the_probe_unmeasured(self):
        code, md = run_main(argv_extra=["--skip-web"])
        self.assertEqual(code, 0)
        # Lu dans la SECTION 6 seulement : les sections 1 a 3 portent la meme phrase,
        # et une assertion sur tout le rapport serait verte sans que la sonde le dise.
        sec6 = md[md.index("## 6. Sonde P2"):]
        self.assertIn("**Non mesuré** (`--skip-web`)", sec6)
        self.assertNotIn("| fr |", sec6)
        self.assertNotIn("sonde P2**", md[md.index("**Verdict**"):])


class ColdStartWake(unittest.TestCase):
    """P24-50 — le reveil d'un site endormi (mesure web2 du 24/11 : 93 a 107 s)."""

    def test_bounds_are_pinned_as_literals_and_cover_the_measured_start(self):
        self.assertEqual(MOD.PASS_MAX_TIME, 60)
        self.assertEqual(MOD.WAKE_MAX_TIME, 240)
        # La borne de reveil couvre la mesure rapportee (93 a 107 s), pas l'inverse.
        self.assertGreater(MOD.WAKE_MAX_TIME, 107)

    def test_the_bound_reaches_curl_max_time(self):
        # Le transport reel est un sous-processus curl : ce que le faux `fetch` ne peut
        # pas montrer, c'est que la borne arrive jusqu'a la ligne de commande.
        mod = load()
        seen = {}

        class Done:
            stdout = b"<html></html>\n200"

        def fake_run(cmd, capture_output=False):
            seen["cmd"] = cmd
            return Done()

        mod.subprocess = types.SimpleNamespace(run=fake_run)
        status, _ = mod.fetch("https://sonde.invalid/fr/", None, "cb", max_time=240)
        self.assertEqual(status, 200)
        self.assertEqual(seen["cmd"][seen["cmd"].index("--max-time") + 1], "240")
        mod.fetch("https://sonde.invalid/fr/", None, "cb")
        self.assertEqual(seen["cmd"][seen["cmd"].index("--max-time") + 1], "60")

    def test_cold_server_after_the_old_bound_passes_thanks_to_the_wake(self):
        # Le serveur ne peut pas repondre avant 97 s — au-dela de la borne de passe
        # (60 s), dans la mesure de web2. L'eveil, lui, a 240 s : il passe, la passe
        # suit sur un site chaud, et le rapport nomme statut, duree et bornes.
        clock, calls, cold = [0.0], [], {"ready_after": 97.0}
        code, md = run_main(calls=calls, cold=cold, clock=clock)
        self.assertEqual(code, 0, md[-800:])
        self.assertEqual(calls[0], (BASE + "/", 240), "le reveil porte la borne de reveil")
        self.assertTrue(all(mt == 60 for _, mt in calls[1:]), "la passe garde la sienne")
        self.assertEqual(clock[0], 97.0, "une seule attente : celle du demarrage")
        # 1 reveil + 42 requetes de passe (32 routes, 2 pages legales, 8 pages Regles) :
        # le compte que dit la docstring, lu sur le transport.
        self.assertEqual(len(calls), 43)
        self.assertIn(
            "- Réveil : `GET /` → **200 en 97 s** "
            "(borne de réveil 240 s ; borne de passe 60 s)", md)

    def test_same_cold_server_without_the_wake_is_red_at_the_old_bound(self):
        # Le controle : le meme serveur, la passe SANS l'eveil. La premiere route est
        # coupee a 60 s (statut 0) et rougit ; les suivantes passent (le site s'est
        # reveille pendant l'attente). C'est ce rouge-la que l'eveil supprime — et il
        # se rejoue en entier par `main()`, avec l'eveil neutralise (mutation M8).
        clock, calls, cold = [0.0], [], {"ready_after": 97.0}

        def no_wake(base, resolve, cb, max_time=None):
            return {"url": "/", "status": None, "seconds": 0.0, "max_time": max_time,
                    "requested": False, "ok": True}

        code, md = run_main(calls=calls, cold=cold, clock=clock,
                           mutate=lambda m: setattr(m, "wake_site", no_wake))
        self.assertEqual(code, 1, md[-800:])
        self.assertIn("- Réveil : non exécuté (état d'instrument du contrat).", md)
        first = next(l for l in md.splitlines() if l.startswith("| fr | accueil |"))
        self.assertIn("❌ 0", first)
        self.assertEqual(clock[0], 97.0)

    def test_mute_server_exits_one_without_running_the_pass(self):
        # Aucune reponse HTTP : le site n'est pas « lent », il est absent. La passe ne
        # doit pas couter 41 bornes de 60 s pour le dire — une seule borne de reveil.
        clock, calls = [0.0], []
        code, md = run_main(argv_extra=["--wake-max-time", "30", "--max-time", "5"],
                            calls=calls, cold={"ready_after": float("inf")}, clock=clock)
        self.assertEqual(code, 1, md[-800:])
        self.assertEqual(len(calls), 1, "aucune requete de passe apres un reveil muet")
        self.assertEqual(calls[0], (BASE + "/", 30))
        self.assertEqual(clock[0], 30.0)
        self.assertIn("aucune réponse HTTP en 30 s", md)
        self.assertIn("⛔ **réveil sans réponse HTTP** (contrôles 1 à 3 non mesurés)", md)
        sec1 = md[md.index("## 1."):md.index("## 2.")]
        self.assertIn("réveil sans réponse HTTP", sec1)
        self.assertNotIn("--skip-web", sec1, "un reveil muet n'est pas un choix de l'operateur")

    def test_an_http_status_at_the_wake_is_not_a_mute_site(self):
        # 503 est une REPONSE : le site est la, la passe juge la route comme elle est.
        calls = []
        code, md = run_main(statuses={"/": 503}, calls=calls)
        self.assertEqual(code, 1, md[-800:])
        self.assertGreater(len(calls), 1, "un statut HTTP ne coupe pas la passe")
        self.assertIn("Réveil : `GET /` → **503 en", md)
        self.assertNotIn("réveil sans réponse HTTP", md)

    def test_skip_web_sends_no_wake(self):
        calls = []
        code, md = run_main(argv_extra=["--skip-web"], calls=calls)
        self.assertEqual(code, 0, md[-800:])
        self.assertEqual(calls, [], "l'etape 5 n'envoie aucune requete, reveil compris")
        self.assertIn("- Réveil : non demandé (`--skip-web`).", md)


if __name__ == "__main__":
    unittest.main(verbosity=2)