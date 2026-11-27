#!/usr/bin/env python3
"""Témoin de recette DNN — lecture seule, aucune ecriture, aucun formulaire, aucun login.

Trois familles de constats, chacune mesuree sur les HUIT cultures :

  1. marqueur 2sxc   — une page servie en HTTP 200 ne doit pas porter le message
                       d'erreur de rendu 2sxc (bloc `<!--2sxc:error-->`), sur les
                       sept parcours publics (accueil, Règles, Argumentation, liste
                       et détail d'actualité, Amis, Téléchargements).
                       Demande de l'issue #2398 : « Ajouter un controle qui echoue
                       sur une reponse HTTP 200 portant le message ».
  2. bouton FR       — depuis chacun des sept accueils non francais, le lien
                       `li.nav-fr-fr` doit designer une destination francaise et,
                       une fois suivi dans la MEME session — avec la preference de
                       langue du navigateur de la culture de depart — aboutir a la
                       racine `<html lang="fr-FR">`. Issue #2397.
  3. ancien texte FR — la page Argumentation de chaque culture non francaise ne doit
                       pas servir le texte francais de l'ancien contenu. Issue #2181.
                       Le constat dit ce qu'il detecte : la PRESENCE de l'ancien
                       texte, pas « un corps non français ».

TROIS GARDES AVANT TOUT VERDICT OK (contre-revue ai-01, c.7202541558). En remplacant
seulement `fetch`, neuf constats EN sortaient OK alors que rien n'avait été mesuré :
des réponses toutes en fr-FR, une page Argumentation sans corps, un lien
`/legal/details` placé avant le vrai article. Un verdict OK exige donc :

  G1 langue du point de depart — la page lue est servie dans la culture attendue,
     sur les trois familles ; sinon ERREUR, le constat n'est pas jugeable ;
  G2 detail sous le chemin Actus — le detail est le premier lien dont le chemin est
     `<chemin de la liste>/details/…` (insensible a la casse), jamais « le premier
     lien contenant details » ; sa destination est verifiee (chemin reste sous
     Actus, langue attendue) ;
  G3 corps principal present — un panneau de contenu `dnn_ContentPane` existe et
     depasse MIN_BODY_CHARS caracteres ; son absence est un ERREUR, jamais un OK.

Chaque culture rend TOUJOURS ses constats : 7 pour le francais, 9 pour chacune des
sept autres, 70 en tout. Une page non lue, servie dans une autre langue ou sans
corps rend un ERREUR nomme, jamais une ligne absente : une absence de ligne ne peut
pas se lire « rien à signaler ».

Verdicts : OK = le defaut cherche est absent, et les gardes ont ete passees ;
ECHEC = le defaut est vu ; ERREUR = le constat n'a pas pu etre fait. Les deux
derniers font echouer la recette.

Chaque famille a un controle inverse (`--self-test`) : on plante l'aiguille dans
une chaine fabriquee et on exige que le detecteur la trouve. Un `0` n'est une
absence que si l'instrument pouvait voir un `1`. Les fixtures de bout en bout
(les trois faux verts, un cas sain) vivent dans `tools/test_2398-page-witness.py`.

Usage :
    python dnn-page-witness.py --env both
    python dnn-page-witness.py --env prod --self-test
    python dnn-page-witness.py --env preprod --timeout 45

Sortie : une ligne par constat, puis un compte. Code 0 = aucun echec,
1 = au moins un echec ou une erreur de constat, 2 = erreur d'instrument (reseau, parse).
"""

import argparse
import html
import re
import sys
import urllib.error
import urllib.parse
import urllib.request
from http.cookiejar import CookieJar

# --- cultures : prefixe d'URL reelement servi par le site (fr n'a pas de prefixe)
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

ENVS = {
    "prod": "https://www.argumentum.games",
    "preprod": "https://dnn.argumentum.myia.io",
}

# Marqueur de rendu 2sxc en echec. Le conteneur servi est
#   <div class='dnnFormMessage dnnFormWarning'><!--2sxc:error-->Error Showing Content ...</div>
# — classe « Warning » et attributs en quotes simples : ni `class*=error`, ni
# `dnnFormError`, ni `alert-danger` ne peuvent le matcher. On cherche donc le
# commentaire et le litteral, jamais une classe.
MARK_2SXC = "<!--2sxc:error-->"
MARK_TEXT = "Error Showing Content"

# Debut du corps francais de l'ancien contenu Argumentation (sans apostrophe,
# pour ne pas dependre de l'entite HTML choisie par le gabarit).
FR_BODY = "intéresse à cet art qui a traversé les âges"

# Parcours publics sondes pour le marqueur 2sxc, dans cet ordre. Les slugs sont
# accentues en francais et servis tels quels dans toutes les cultures (mesure du
# 26/11 : 200 et lang correct sur les deux environnements).
PAGES = [
    ("accueil", ""),
    ("Regles", "Règles"),
    ("Argumentation", "Argumentation"),
    ("actus", "actus"),
    ("Amis", "Amis"),
    ("Telechargements", "Téléchargements"),
]

LANG_RE = re.compile(r"<html[^>]*\blang=[\"']([^\"']+)[\"']", re.I)
# Le menu 2shine rend ses href en QUOTES SIMPLES ; une regex a doubles quotes
# seules est aveugle par construction.
FR_LI_RE = re.compile(r"<li[^>]*class=[\"'][^\"']*\bnav-fr-fr\b[^\"']*[\"'][^>]*>(.*?)</li>", re.I | re.S)
A_RE = re.compile(r"<a\b[^>]*\bhref=[\"']([^\"']*)[\"']", re.I)

# Panneau de contenu du corps principal (skin 2shine BS5). Mesure du 26/11 : present
# UNE fois sur les huit cultures, dans les deux environnements.
PANE_OPEN_RE = re.compile(r"<div\b[^>]*\bid=[\"']dnn_ContentPane[\"'][^>]*>", re.I)
DIV_TAG_RE = re.compile(r"<(/?)div\b", re.I)
TAG_STRIP_RE = re.compile(r"<script\b.*?</script>|<style\b.*?</style>|<[^>]+>", re.I | re.S)
# Seuil de presence d'un corps, en CARACTERES de texte (balises retirees) : moins de la
# moitie du plus petit corps mesure (921, zh-CN en preprod ; 5244 et plus en
# production). Un bloc d'erreur 2sxc en fait une soixantaine, un panneau vide 0. Sous
# le seuil le constat n'est jugeable que sur un corps ABSENT : il n'est jamais rendu OK.
MIN_BODY_CHARS = 400


def detect_2sxc(text):
    """Rend la liste des marqueurs trouves (commentaire ou litteral)."""
    hits = []
    if MARK_2SXC in text:
        hits.append("commentaire 2sxc:error")
    if MARK_TEXT in text:
        hits.append("litteral Error Showing Content")
    return hits


def detect_fr_body(text):
    return FR_BODY in html.unescape(text)


def main_body_text(text):
    """Texte du panneau de contenu, balises retirees ; None si le panneau est absent.

    Le panneau est lu jusqu'a la balise </div> qui ferme la sienne (divs imbriques
    comptes), pas jusqu'a la premiere fermeture rencontree."""
    m = PANE_OPEN_RE.search(text)
    if not m:
        return None
    if text[m.start():m.end()].rstrip().endswith("/>"):
        return ""
    depth = 1
    end = len(text)
    for tag in DIV_TAG_RE.finditer(text, m.end()):
        depth += -1 if tag.group(1) else 1
        if depth == 0:
            end = tag.start()
            break
    stripped = TAG_STRIP_RE.sub(" ", text[m.end():end])
    return re.sub(r"\s+", " ", html.unescape(stripped)).strip()


def fr_href(page_html):
    """href du lien FR dans le menu, ou None si le lien est absent."""
    li = FR_LI_RE.search(page_html)
    if not li:
        return None
    a = A_RE.search(li.group(1))
    return html.unescape(a.group(1)) if a else None


def new_opener():
    """Session anonyme fraiche : un panier de cookies par parcours."""
    return urllib.request.build_opener(urllib.request.HTTPCookieProcessor(CookieJar()))


def fetch(opener, url, timeout, accept_language=None):
    """GET anonyme ; rend (URL finale, statut, texte). L'en-tete de preference de
    langue du navigateur n'est envoye que lorsqu'il est donne : le suivi du lien FR
    se fait avec celui de la culture de depart, comme le navigateur d'un visiteur."""
    headers = {"User-Agent": "argumentum-page-witness/1.0"}
    if accept_language:
        headers["Accept-Language"] = accept_language
    req = urllib.request.Request(url, headers=headers)
    with opener.open(req, timeout=timeout) as r:
        raw = r.read()
        return r.geturl(), r.status, raw.decode("utf-8", "replace")


def page_lang(text):
    m = LANG_RE.search(text[:4000])
    return m.group(1) if m else ""


def same_culture(served, expected):
    return served.lower() == expected.lower()


def path_of(url):
    return urllib.parse.urlparse(url).path or "/"


def details_prefix(actus_url):
    """Chemin sous lequel un lien de detail doit se trouver : `<chemin de la liste>/details`,
    en minuscules (la liste est servie `/en/actus`, ses liens `/en/Actus/details/…`)."""
    return (path_of(actus_url).rstrip("/") + "/details").lower()


def detail_link(actus_url, actus_html):
    """Premier lien de la liste dont le chemin est sous `<chemin de la liste>/details/…`,
    insensible a la casse ; None s'il n'y en a aucun."""
    prefix = details_prefix(actus_url)
    for href in A_RE.findall(actus_html):
        target = urllib.parse.urljoin(actus_url, html.unescape(href))
        if path_of(target).lower().startswith(prefix):
            return target
    return None


def self_test():
    """Controle inverse : chaque detecteur doit voir l'aiguille plantee."""
    ok = True
    planted = "<div class='dnnFormMessage dnnFormWarning'><!--2sxc:error-->Error Showing Content - please login as admin for details.</div>"
    if not detect_2sxc(planted):
        print("[self-test] ECHEC : detect_2sxc ne voit pas l'aiguille")
        ok = False
    if detect_2sxc("<div>contenu normal</div>"):
        print("[self-test] ECHEC : detect_2sxc voit une aiguille absente")
        ok = False
    if not detect_fr_body("<p>Argumentum " + FR_BODY + " et construit les</p>"):
        print("[self-test] ECHEC : detect_fr_body ne voit pas l'aiguille")
        ok = False
    li = "<li class='nav-fr-fr'><a title='fr' href='/'>FR</a></li>"
    if fr_href(li) != "/":
        print("[self-test] ECHEC : fr_href ne lit pas un href en quotes simples")
        ok = False
    if fr_href("<li class='nav-en-us'><a href='/en/'>EN</a></li>") is not None:
        print("[self-test] ECHEC : fr_href rend un href pour une entree non FR")
        ok = False

    # corps principal : un panneau present et plein est vu, un panneau absent est None,
    # un panneau vide est vide — et les divs imbriques ne tronquent pas la lecture.
    full = "<main><div id='dnn_ContentPane'><div class='inner'><p>%s</p></div></div></main>" % ("mot " * 200)
    body = main_body_text(full)
    if body is None or len(body) < MIN_BODY_CHARS:
        print("[self-test] ECHEC : main_body_text ne lit pas un panneau present (%r)" % (body,))
        ok = False
    if main_body_text("<main><div id='autre'><p>%s</p></div></main>" % ("mot " * 200)) is not None:
        print("[self-test] ECHEC : main_body_text rend un corps sans panneau de contenu")
        ok = False
    if main_body_text("<div id='dnn_ContentPane'></div>") != "":
        print("[self-test] ECHEC : main_body_text rend un corps dans un panneau vide")
        ok = False
    if body is not None and len(body.split()) != 200:
        print("[self-test] ECHEC : main_body_text tronque les divs imbriques (%d mots)" % len(body.split()))
        ok = False

    # detail sous le chemin Actus : un lien hors Actus place avant le vrai article n'est pas choisi.
    actus = "https://temoin.invalid/en/actus"
    liste = "<a href='/legal/details/mentions'>Mentions</a><a href='/en/Actus/details/article'>Article</a>"
    if detail_link(actus, liste) != "https://temoin.invalid/en/Actus/details/article":
        print("[self-test] ECHEC : detail_link choisit un lien hors du chemin Actus")
        ok = False
    if detail_link(actus, "<a href='/legal/details/mentions'>Mentions</a>") is not None:
        print("[self-test] ECHEC : detail_link rend un lien sans detail sous Actus")
        ok = False
    print("[self-test] %s" % ("OK" if ok else "ECHEC"))
    return 0 if ok else 2


def run(env_name, base, timeout):
    """Les constats d'un environnement : [(famille, cible, verdict, detail)], et la liste
    des erreurs d'instrument (reseau). Aucune ligne ne saute : chaque culture rend ses
    7 (francais) ou 9 (autres) constats, quel que soit l'etat des pages."""
    rows = []
    errors = []

    for culture, prefix in CULTURES:
        basepath = base + "/" + (prefix + "/" if prefix else "")
        home = basepath
        actus_url = None
        actus_html = None
        actus_note = "liste Actus non lue : detail non jugeable"

        # --- 1. marqueur 2sxc sur chaque parcours public
        for label, path in PAGES:
            target = "%s %s" % (culture, label)
            url = basepath + urllib.parse.quote(path, safe="")
            try:
                opener = new_opener()
                final, status, text = fetch(opener, url, timeout)
                unread = None
            except urllib.error.HTTPError as e:
                unread = "non lue (HTTP %s)" % e.code
            except Exception as e:  # reseau
                unread = "non lue (%s)" % e
                errors.append("%s : %s" % (target, e))

            if unread:
                rows.append(("marqueur", target, "ERREUR", "page %s : marqueur non jugeable" % unread))
                if label == "Argumentation" and culture != "fr-FR":
                    rows.append(("ancien-texte-FR", target, "ERREUR",
                                 "page %s : ancien texte FR non jugeable" % unread))
                if label == "actus":
                    actus_note = "liste Actus %s : detail non jugeable" % unread
                continue

            # --- G1 : la page lue est-elle servie dans la culture attendue ?
            served = page_lang(text)
            in_culture = same_culture(served, culture)
            if not in_culture:
                rows.append(("marqueur", target, "ERREUR",
                             "servie en %s, attendue %s (HTTP %s) : marqueur non jugeable"
                             % (served or "lang absent", culture, status)))
            else:
                hits = detect_2sxc(text)
                rows.append(("marqueur", target, "ECHEC" if hits else "OK",
                             (", ".join(hits) + " (HTTP %s)" % status) if hits
                             else "HTTP %s, aucun marqueur" % status))

            # --- 3. ancien texte FR dans le corps principal, sur Argumentation seule
            if label == "Argumentation" and culture != "fr-FR":
                if not in_culture:
                    rows.append(("ancien-texte-FR", target, "ERREUR",
                                 "servie en %s, attendue %s : ancien texte FR non jugeable"
                                 % (served or "lang absent", culture)))
                else:
                    body = main_body_text(text)
                    if body is None:
                        rows.append(("ancien-texte-FR", target, "ERREUR",
                                     "corps principal absent : aucun panneau de contenu (dnn_ContentPane)"))
                    elif len(body) < MIN_BODY_CHARS:
                        rows.append(("ancien-texte-FR", target, "ERREUR",
                                     "corps principal absent ou vide : %d caracteres (seuil %d)"
                                     % (len(body), MIN_BODY_CHARS)))
                    elif detect_fr_body(body):
                        rows.append(("ancien-texte-FR", target, "ECHEC",
                                     "ancien texte francais servi sous lang=%s (corps de %d caracteres)"
                                     % (served, len(body))))
                    else:
                        rows.append(("ancien-texte-FR", target, "OK",
                                     "ancien texte FR absent (corps de %d caracteres, lang=%s)"
                                     % (len(body), served)))

            if label == "actus":
                if in_culture:
                    actus_url, actus_html, actus_note = final, text, ""
                else:
                    actus_note = "liste Actus servie en %s, attendue %s : detail non jugeable" % (
                        served or "lang absent", culture)

        # --- 1bis. detail d'actualite : premier detail SOUS le chemin Actus (G2),
        # destination verifiee.
        dtarget = "%s actu-detail" % culture
        if actus_html is None:
            rows.append(("marqueur", dtarget, "ERREUR", actus_note))
        else:
            dprefix = details_prefix(actus_url)
            durl = detail_link(actus_url, actus_html)
            if durl is None:
                rows.append(("marqueur", dtarget, "ERREUR",
                             "aucun lien de detail sous %s" % dprefix))
            else:
                try:
                    dfinal, dstatus, dtext = fetch(new_opener(), durl, timeout)
                except urllib.error.HTTPError as e:
                    rows.append(("marqueur", dtarget, "ERREUR",
                                 "detail non lu (HTTP %s) : marqueur non jugeable" % e.code))
                except Exception as e:
                    errors.append("%s : %s" % (dtarget, e))
                    rows.append(("marqueur", dtarget, "ERREUR",
                                 "detail non lu (%s) : marqueur non jugeable" % e))
                else:
                    dpath = path_of(dfinal).lower()
                    dserved = page_lang(dtext)
                    if not dpath.startswith(dprefix):
                        rows.append(("marqueur", dtarget, "ERREUR",
                                     "destination hors du chemin Actus : %s (HTTP %s)" % (dpath, dstatus)))
                    elif not same_culture(dserved, culture):
                        rows.append(("marqueur", dtarget, "ERREUR",
                                     "detail servi en %s, attendu %s (HTTP %s)"
                                     % (dserved or "lang absent", culture, dstatus)))
                    else:
                        dhits = detect_2sxc(dtext)
                        rows.append(("marqueur", dtarget, "ECHEC" if dhits else "OK",
                                     (", ".join(dhits) + " (HTTP %s)" % dstatus) if dhits
                                     else "HTTP %s, aucun marqueur" % dstatus))

        # --- 2. bouton FR depuis les sept accueils non francais, dans la MEME session
        if culture == "fr-FR":
            continue
        btarget = "depuis %s" % culture
        opener = new_opener()
        try:
            cur_url, _, cur_html = fetch(opener, home, timeout)
        except urllib.error.HTTPError as e:
            rows.append(("bouton-FR", btarget, "ERREUR",
                         "point de depart non lu (HTTP %s) : lien FR non jugeable" % e.code))
            continue
        except Exception as e:
            errors.append("%s : %s" % (btarget, e))
            rows.append(("bouton-FR", btarget, "ERREUR",
                         "point de depart non lu (%s) : lien FR non jugeable" % e))
            continue

        # --- G1 : le point de depart est-il servi dans la culture de depart ?
        served = page_lang(cur_html)
        if not same_culture(served, culture):
            rows.append(("bouton-FR", btarget, "ERREUR",
                         "point de depart servi en %s, attendu %s : lien FR non jugeable"
                         % (served or "lang absent", culture)))
            continue

        href = fr_href(cur_html)
        if href is None:
            rows.append(("bouton-FR", btarget, "ECHEC", "aucun lien nav-fr-fr"))
            continue
        target = urllib.parse.urljoin(cur_url, href)
        target_path = path_of(target)
        non_fr_prefix = any(
            target_path == "/" + p + "/" or target_path.startswith("/" + p + "/")
            for c, p in CULTURES if p
        )
        if target.rstrip("/") == cur_url.rstrip("/"):
            verdict, detail = "ECHEC", "href = URL courante (%s)" % href
        elif non_fr_prefix:
            verdict, detail = "ECHEC", "destination prefixee non FR (%s)" % href
        else:
            verdict, detail = "OK", "href %s" % href

        # meme session, preference de langue du navigateur = culture de depart : on suit
        # le lien et on exige la racine francaise.
        try:
            landing, status, land_html = fetch(opener, target, timeout, accept_language=culture)
            lang = page_lang(land_html)
        except urllib.error.HTTPError as e:
            landing, status, lang = target, e.code, ""
        except Exception as e:
            errors.append("%s suivi FR : %s" % (culture, e))
            rows.append(("bouton-FR", btarget, "ERREUR", "%s ; suivi non lu (%s)" % (detail, e)))
            continue
        landing_path = path_of(landing)
        if verdict == "OK":
            if not lang.lower().startswith("fr"):
                verdict, detail = "ECHEC", "suivi -> %s lang=%s (HTTP %s), attendu la racine / en francais" % (
                    landing_path, lang or "absent", status)
            elif landing_path != "/":
                verdict, detail = "ECHEC", "suivi -> %s : attendu la racine / (HTTP %s)" % (landing_path, status)
            else:
                detail += " -> %s lang=%s" % (landing_path, lang)
        else:
            # echec deja acte : on documente la consequence observee
            try:
                p_url, p_status, p_html = fetch(opener, base + "/?language=fr-FR", timeout)
                detail += " ; ?language=fr-FR -> %s lang=%s" % (path_of(p_url), page_lang(p_html))
            except Exception:
                pass
        rows.append(("bouton-FR", btarget, verdict, detail))

    return rows, errors


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--env", choices=["prod", "preprod", "both"], default="both")
    ap.add_argument("--timeout", type=int, default=45)
    ap.add_argument("--self-test", action="store_true", help="controle inverse puis sortie")
    args = ap.parse_args()

    if args.self_test:
        sys.exit(self_test())

    names = ["prod", "preprod"] if args.env == "both" else [args.env]
    failures = 0
    instrument_errors = []
    for name in names:
        print("")
        print("===== %s — %s =====" % (name.upper(), ENVS[name]))
        rows, errors = run(name, ENVS[name], args.timeout)
        width = max([len(r[1]) for r in rows] or [10])
        for family, target, verdict, detail in rows:
            if verdict != "OK":
                failures += 1
            print("%-15s %-*s %-6s %s" % (family, width, target, verdict, detail))
        if errors:
            instrument_errors.extend(errors)
            print("-- erreurs d'instrument : %d" % len(errors))
            for e in errors[:10]:
                print("   %s" % e)

    print("")
    print("total echecs = %d ; erreurs d'instrument = %d" % (failures, len(instrument_errors)))
    if instrument_errors:
        sys.exit(2)
    sys.exit(1 if failures else 0)


if __name__ == "__main__":
    main()
