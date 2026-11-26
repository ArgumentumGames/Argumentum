#!/usr/bin/env python3
"""Temoin de recette DNN — lecture seule, aucune ecriture, aucun formulaire, aucun login.

Trois familles de constats, chacune mesuree sur les HUIT cultures :

  1. marqueur 2sxc   — une page servie en HTTP 200 ne doit pas porter le message
                       d'erreur de rendu 2sxc (bloc `<!--2sxc:error-->`).
                       Demande de l'issue #2398 : « Ajouter un controle qui echoue
                       sur une reponse HTTP 200 portant le message ».
  2. bouton FR       — depuis chacun des sept accueils non francais, le lien
                       `li.nav-fr-fr` doit designer une destination francaise et,
                       une fois suivi dans la MEME session, aboutir a
                       `<html lang="fr-FR">`. Issue #2397.
  3. langue du corps — la page Argumentation de chaque culture ne doit pas servir
                       le texte francais de l'ancien contenu. Issue #2181.

Chaque famille a un controle inverse (`--self-test`) : on plante l'aiguille dans
une chaine fabriquee et on exige que le detecteur la trouve. Un `0` n'est une
absence que si l'instrument pouvait voir un `1`.

Usage :
    python dnn-page-witness.py --env both
    python dnn-page-witness.py --env prod --self-test
    python dnn-page-witness.py --env preprod --timeout 45

Sortie : une ligne par constat, puis un compte. Code 0 = aucun echec,
1 = au moins un echec, 2 = erreur d'instrument (reseau, parse).
"""

import argparse
import html
import re
import sys
import urllib.error
import urllib.parse
import urllib.request
from http.cookiejar import CookieJar

# --- cultures : prefixe d'URL reellement servi par le site (fr n'a pas de prefixe)
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

LANG_RE = re.compile(r"<html[^>]*\blang=[\"']([^\"']+)[\"']", re.I)
# Le menu 2shine rend ses href en QUOTES SIMPLES ; une regex a doubles quotes
# seules est aveugle par construction.
FR_LI_RE = re.compile(r"<li[^>]*class=[\"'][^\"']*\bnav-fr-fr\b[^\"']*[\"'][^>]*>(.*?)</li>", re.I | re.S)
A_RE = re.compile(r"<a\b[^>]*\bhref=[\"']([^\"']*)[\"']", re.I)


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


def fr_href(page_html):
    """href du lien FR dans le menu, ou None si le lien est absent."""
    li = FR_LI_RE.search(page_html)
    if not li:
        return None
    a = A_RE.search(li.group(1))
    return html.unescape(a.group(1)) if a else None


def fetch(opener, url, timeout):
    req = urllib.request.Request(url, headers={"User-Agent": "argumentum-page-witness/1.0"})
    with opener.open(req, timeout=timeout) as r:
        raw = r.read()
        return r.geturl(), r.status, raw.decode("utf-8", "replace")


def page_lang(text):
    m = LANG_RE.search(text[:4000])
    return m.group(1) if m else ""


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
    print("[self-test] %s" % ("OK" if ok else "ECHEC"))
    return 0 if ok else 2


def run(env_name, base, timeout):
    rows = []  # (famille, cible, verdict, detail)
    errors = []

    for culture, prefix in CULTURES:
        home = base + "/" + (prefix + "/" if prefix else "")

        # --- 1. marqueur 2sxc sur l'accueil, Amis et Argumentation (HTTP 200 seulement)
        for label, path in (("accueil", ""), ("Amis", "Amis"), ("Argumentation", "Argumentation")):
            url = base + "/" + (prefix + "/" if prefix else "") + path
            try:
                jar = CookieJar()
                opener = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(jar))
                final, status, text = fetch(opener, url, timeout)
            except urllib.error.HTTPError as e:
                rows.append(("marqueur", "%s %s" % (culture, label), "ERREUR", "HTTP %s" % e.code))
                continue
            except Exception as e:  # reseau
                errors.append("%s %s : %s" % (culture, label, e))
                continue
            hits = detect_2sxc(text)
            if hits:
                rows.append(("marqueur", "%s %s" % (culture, label), "ECHEC", ", ".join(hits) + " (HTTP %s)" % status))
            else:
                rows.append(("marqueur", "%s %s" % (culture, label), "OK", "HTTP %s, aucun marqueur" % status))

            # --- 3. langue du corps, sur Argumentation seule
            if label == "Argumentation" and culture != "fr-FR":
                if detect_fr_body(text):
                    rows.append(("corps", "%s Argumentation" % culture, "ECHEC", "corps francais servi sous lang=%s" % page_lang(text)))
                else:
                    rows.append(("corps", "%s Argumentation" % culture, "OK", "corps non francais"))

        # --- 2. bouton FR depuis les sept accueils non francais, dans la MEME session
        if culture == "fr-FR":
            continue
        try:
            jar = CookieJar()
            opener = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(jar))
            cur_url, _, cur_html = fetch(opener, home, timeout)
            href = fr_href(cur_html)
        except Exception as e:
            errors.append("%s bouton FR : %s" % (culture, e))
            continue
        if href is None:
            rows.append(("bouton-FR", "depuis %s" % culture, "ECHEC", "aucun lien nav-fr-fr"))
            continue
        target = urllib.parse.urljoin(cur_url, href)
        target_path = urllib.parse.urlparse(target).path or "/"
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

        # meme session : on suit le lien et on exige la culture francaise
        try:
            landing, status, land_html = fetch(opener, target, timeout)
            lang = page_lang(land_html)
        except urllib.error.HTTPError as e:
            landing, status, lang = target, e.code, ""
        except Exception as e:
            errors.append("%s suivi FR : %s" % (culture, e))
            continue
        if verdict == "OK":
            if lang.lower().startswith("fr"):
                detail += " -> %s lang=%s" % (urllib.parse.urlparse(landing).path or "/", lang)
            else:
                verdict, detail = "ECHEC", "suivi -> lang=%s (HTTP %s)" % (lang or "absent", status)
        else:
            # echec deja acte : on documente la consequence observee
            try:
                p_url, p_status, p_html = fetch(opener, base + "/?language=fr-FR", timeout)
                detail += " ; ?language=fr-FR -> %s lang=%s" % (urllib.parse.urlparse(p_url).path or "/", page_lang(p_html))
            except Exception:
                pass
        rows.append(("bouton-FR", "depuis %s" % culture, verdict, detail))

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
            print("%-11s %-*s %-6s %s" % (family, width, target, verdict, detail))
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
