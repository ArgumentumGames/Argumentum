#!/usr/bin/env python3
"""#1066 §2c — balayage systematique de l'ecart de contenu prod <-> preprod.

Question posee par l'issue : « quels AUTRES artefacts la prod sert-elle que la
preprod ignore ? »  Les deux connus (balises GTM/GA, 16 wrappers de cartes
mentales) ont ete trouves **par accident**, en cherchant autre chose.  Aucune
enumeration systematique n'avait ete faite — et c'est precisement la classe de
defaut qui ne se decouvre qu'en production, le jour du cutover.

Methode
-------
1. Crawl BFS de chaque hote depuis ``/``, liens de meme hote uniquement, borne
   en pages et en profondeur.
2. Sur chaque page, collecte de TOUTES les ressources referencees (``src``,
   ``href``, ``srcset``, ``poster``, ``data``, ``action``, ``url()`` inline).
3. Chaque chemin unique est interroge sur LES DEUX hotes.
4. Verdict par chemin :
     - prod < 400  ET  preprod >= 400  -> MANQUANT_EN_PREPROD  (la trouvaille)
     - l'inverse                       -> MANQUANT_EN_PROD     (sens contraire)
     - les deux < 400                  -> sain
     - demande par un hote et en echec CHEZ LUI -> CASSE_CHEZ_LE_DEMANDEUR
     - tout le reste                   -> indetermine

⚠️ Deux angles morts de la comparaison inter-hotes, mesures le 08/10/2026 (grain
L-login, #1781) — le crawl COLLECTE la provenance, les verdicts ne s'en servaient
pas :

  (a) Un chemin **demande** par un hote et absent **des deux** tombait dans
      ``indetermine`` : ni « manquant relativement a l'autre » (l'autre ne l'a pas
      non plus, souvent parce qu'il ne le demande pas — versions de chemins
      differentes entre DNN 9 et DNN 10), ni « sain ».  Cas reel : les 3
      bibliotheques de ``/Login`` preprod (``Resources/libraries/<Lib>/<ver>/``)
      rendaient 404 sur les deux hotes — page cassee, verdict muet.
      ⇒ nouveau verdict ``CASSE_CHEZ_LE_DEMANDEUR`` (+ champ ``broken_on``).

  (b) Reciproquement, un ``MANQUANT_EN_PROD`` / ``MANQUANT_EN_PREPROD`` porte sur
      un chemin que l'hote en echec ne **demande pas** (``in_<hote>_crawl: false``)
      est un **ecart de forme**, pas un defaut de service.  Le champ existe depuis
      l'origine ; le lire avant de conclure.

Usage
-----
    python tools/cutover-gap-sweep.py --out rapport.json
    python tools/cutover-gap-sweep.py --max-pages 60      # passe rapide
    python tools/cutover-gap-sweep.py --self-test         # verdicts, sans reseau

Lecture seule stricte : uniquement des GET, aucun formulaire, aucun POST, aucun
en-tete d'authentification.  Cadence volontairement basse (``--sleep``).
"""

from __future__ import annotations

import argparse
import json
import re
import sys
import time
from collections import deque
from urllib.parse import urldefrag, urljoin, urlparse

import requests
from bs4 import BeautifulSoup

PROD = "https://www.argumentum.games"
PREPROD = "https://dnn.argumentum.myia.io"
HOSTS = {"prod": PROD, "preprod": PREPROD}
UA = {"User-Agent": "Argumentum-cutover-audit/1.0 (read-only; #1066)"}

# Chemins qu'on refuse de PARCOURIR (couteux).  Ils restent testes des lors
# qu'une page les reference : on ne fait que ne pas les crawler.
SKIP_CRAWL = re.compile(
    r"\.(pdf|zip|png|jpe?g|gif|svg|webp|ico|css|js|xml|json|owl|txt|csv"
    r"|xlsx?|docx?|pptx?|mp4|mp3|woff2?|ttf|eot)$",
    re.I,
)

# Schemas qu'on ne suit jamais.
SKIP_SCHEME = ("mailto:", "tel:", "javascript:", "data:", "sms:", "whatsapp:")


def norm(url: str) -> str:
    """Clef de comparaison : chemin + query, sans fragment ni slash final."""
    parsed = urlparse(urldefrag(url)[0])
    path = parsed.path or "/"
    if len(path) > 1 and path.endswith("/"):
        path = path.rstrip("/")
    return f"{path}?{parsed.query}" if parsed.query else path


def extract_resources(html: str, base: str) -> set[str]:
    """Toutes les URLs referencees par la page, ramenees en absolu."""
    found: set[str] = set()
    soup = BeautifulSoup(html, "html.parser")
    attrs = {
        "a": ["href"],
        "area": ["href"],
        "link": ["href"],
        "script": ["src"],
        "img": ["src"],
        "source": ["src", "srcset"],
        "iframe": ["src"],
        "object": ["data"],
        "embed": ["src"],
        "video": ["src", "poster"],
        "audio": ["src"],
        "track": ["src"],
        "form": ["action"],
    }
    for tag, names in attrs.items():
        for element in soup.find_all(tag):
            for name in names:
                raw = element.get(name)
                if not raw or raw.startswith(SKIP_SCHEME):
                    continue
                found.add(urljoin(base, raw.split()[0]))
    for css in re.findall(r"url\(\s*['\"]?([^'\")]+)['\"]?\s*\)", html, re.I):
        if css.startswith(("data:", "#")):
            continue
        found.add(urljoin(base, css))
    return found


def fetch(session: requests.Session, url: str, *, bust: bool, timeout: int):
    """GET tolerant : renvoie la reponse, ou l'exception (jamais de crash)."""
    if bust:
        url += ("&" if "?" in url else "?") + f"cb={int(time.time() * 1000) % 10_000_000}"
    try:
        return session.get(url, headers=UA, timeout=timeout, allow_redirects=True)
    except requests.RequestException as exc:
        return exc


def crawl(root: str, label: str, session: requests.Session, args) -> tuple[dict, set]:
    """BFS depuis ``/``.  Renvoie (pages, ressources)."""
    pages: dict[str, dict] = {}
    resources: set[str] = set()
    host = urlparse(root).netloc
    queue = deque([(root + "/", 0)])
    seen = {"/"}

    while queue and len(pages) < args.max_pages:
        url, depth = queue.popleft()
        if depth > args.max_depth:
            continue
        resp = fetch(session, url, bust=True, timeout=args.timeout)
        path = norm(url)

        if isinstance(resp, requests.RequestException):
            pages[path] = {"status": None, "error": str(resp)[:120], "depth": depth}
            continue

        ctype = resp.headers.get("Content-Type", "")
        pages[path] = {
            "status": resp.status_code,
            "final": norm(resp.url),
            "bytes": len(resp.content),
            "ctype": ctype.split(";")[0],
            "depth": depth,
        }
        if len(pages) % 25 == 0:
            sys.stderr.write(
                f"  [{label}] {len(pages)} pages, file {len(queue)}, "
                f"{len(resources)} ressources\n"
            )
            sys.stderr.flush()

        if "html" not in ctype.lower():
            continue

        for ref in extract_resources(resp.text, resp.url):
            if urlparse(ref).netloc != host:
                continue
            resources.add(norm(ref))
            if depth + 1 <= args.max_depth and not SKIP_CRAWL.search(urlparse(ref).path):
                candidate = norm(ref)
                if candidate not in seen:
                    seen.add(candidate)
                    queue.append((urljoin(root, ref), depth + 1))
        time.sleep(args.sleep)

    sys.stderr.write(
        f"  [{label}] TERMINE : {len(pages)} pages, {len(resources)} ressources\n"
    )
    return pages, resources


def classify(path: str, paths_by_host: dict[str, set[str]], row: dict) -> tuple[str, list[str]]:
    """Verdict d'un chemin + hotes chez qui il est casse **pour leurs visiteurs**.

    Fonction pure : ni reseau ni horloge, donc testable (``--self-test``).

    ``broken_on`` = hotes qui DEMANDENT le chemin (page ou ressource de leur crawl)
    ET le recoivent en echec.  C'est la seule information qui distingue « casse »
    de « pas applicable » — deux situations que la comparaison inter-hotes rend
    identiques (cf. l'en-tete du module, angles morts (a) et (b)).

    Les 4 verdicts historiques gardent leur semantique au caractere pres : leurs
    comptes sont cites dans des rapports anterieurs, on ne les reclasse pas.
    """
    status = {label: row.get(label, {}).get("status") for label in ("prod", "preprod")}
    ok = {label: status[label] is not None and status[label] < 400 for label in status}
    broken_on = [
        label for label in ("prod", "preprod")
        if not ok[label] and path in paths_by_host[label]
    ]

    if ok["prod"] and not ok["preprod"]:
        verdict = "MANQUANT_EN_PREPROD"
    elif ok["preprod"] and not ok["prod"]:
        verdict = "MANQUANT_EN_PROD"
    elif ok["prod"] and ok["preprod"]:
        verdict = "sain"
    elif broken_on:
        # Angle mort (a) : demande par au moins un hote, en echec chez lui-meme.
        verdict = "CASSE_CHEZ_LE_DEMANDEUR"
    else:
        verdict = "indetermine"
    return verdict, broken_on


def self_test() -> int:
    """Verdicts sur des cas synthetiques — aucun acces reseau."""
    asked = {"prod": {"/p"}, "preprod": {"/x"}}
    cases = [
        # (nom, chemin, statuts, hotes demandeurs, verdict attendu, broken_on attendu)
        ("sain", "/p", {"prod": 200, "preprod": 200}, {"prod": {"/p"}, "preprod": {"/p"}},
         "sain", []),
        ("manquant en preprod", "/p", {"prod": 200, "preprod": 404},
         {"prod": {"/p"}, "preprod": {"/p"}}, "MANQUANT_EN_PREPROD", ["preprod"]),
        ("manquant en prod", "/p", {"prod": 404, "preprod": 200},
         {"prod": {"/p"}, "preprod": {"/p"}}, "MANQUANT_EN_PROD", ["prod"]),
        # L-login 08/10 : demande par la preprod, 404 des DEUX cotes (la prod est en
        # DNN 9, elle ne demande pas ce chemin) -> doit SORTIR du silence.
        ("casse chez le demandeur (L-login)", "/lib/x.js", {"prod": 404, "preprod": 404},
         {"prod": set(), "preprod": {"/lib/x.js"}}, "CASSE_CHEZ_LE_DEMANDEUR", ["preprod"]),
        ("casse des deux cotes", "/lib/y.js", {"prod": 500, "preprod": 500},
         {"prod": {"/lib/y.js"}, "preprod": {"/lib/y.js"}},
         "CASSE_CHEZ_LE_DEMANDEUR", ["prod", "preprod"]),
        # 404 des deux cotes mais PERSONNE ne le demande : orphelin, pas une page cassee.
        ("orphelin", "/lib/z.js", {"prod": 404, "preprod": 404},
         {"prod": set(), "preprod": set()}, "indetermine", []),
        ("erreur reseau", "/lib/w.js", {"prod": None, "preprod": None},
         {"prod": set(), "preprod": set()}, "indetermine", []),
    ]
    failures = 0
    for name, path, statuses, hosts, want_verdict, want_broken in cases:
        row = {label: {"status": statuses[label]} for label in statuses}
        got_verdict, got_broken = classify(path, hosts, row)
        ok = got_verdict == want_verdict and got_broken == want_broken
        failures += 0 if ok else 1
        print(f"  [{'OK ' if ok else 'KO '}] {name}: {got_verdict} broken_on={got_broken}"
              + ("" if ok else f"  (attendu {want_verdict} {want_broken})"))
    print(f"self-test : {len(cases) - failures}/{len(cases)}")
    return 1 if failures else 0


def main(argv: list[str] | None = None) -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--out", default="cutover-gap-sweep.json")
    parser.add_argument("--max-pages", type=int, default=400)
    parser.add_argument("--max-depth", type=int, default=6)
    parser.add_argument("--sleep", type=float, default=0.10)
    parser.add_argument("--timeout", type=int, default=30)
    parser.add_argument("--prod", default=PROD)
    parser.add_argument("--preprod", default=PREPROD)
    parser.add_argument("--self-test", action="store_true",
                        help="verifie les verdicts sur des cas synthetiques (sans reseau)")
    args = parser.parse_args(argv)

    if args.self_test:
        return self_test()
    hosts = {"prod": args.prod, "preprod": args.preprod}

    sessions = {name: requests.Session() for name in hosts}
    report: dict = {"hosts": hosts, "crawl": {}, "gaps": {}}

    # --- fichiers de meta (robots / sitemap), quand ils existent -------------
    meta = {}
    for label, root in hosts.items():
        for name in ("/robots.txt", "/sitemap.xml"):
            resp = fetch(sessions[label], root + name, bust=False, timeout=args.timeout)
            meta[f"{label}{name}"] = {
                "status": None if isinstance(resp, requests.RequestException) else resp.status_code,
                "bytes": None if isinstance(resp, requests.RequestException) else len(resp.content),
            }
    report["meta_files"] = meta

    # --- crawl ---------------------------------------------------------------
    paths_by_host: dict[str, set[str]] = {}
    for label, root in hosts.items():
        sys.stderr.write(f"[crawl] {label} <- {root}\n")
        sys.stderr.flush()
        pages, resources = crawl(root, label, sessions[label], args)
        report["crawl"][label] = {
            "pages": pages,
            "n_pages": len(pages),
            "n_resources": len(resources),
            "resources": sorted(resources),
        }
        paths_by_host[label] = set(pages) | resources

    # --- sonde de l'union des chemins, des deux cotes ------------------------
    union = sorted(paths_by_host["prod"] | paths_by_host["preprod"])
    sys.stderr.write(f"[probe] {len(union)} chemins uniques x {len(hosts)} hotes\n")
    verdicts: dict[str, dict] = {}
    for index, path in enumerate(union, 1):
        if index % 50 == 0:
            sys.stderr.write(f"  {index}/{len(union)}\n")
            sys.stderr.flush()
        row = {}
        for label in hosts:
            resp = fetch(sessions[label], urljoin(hosts[label], path), bust=False, timeout=args.timeout)
            if isinstance(resp, requests.RequestException):
                row[label] = {"status": None, "error": str(resp)[:120]}
            else:
                row[label] = {"status": resp.status_code, "bytes": len(resp.content)}
            time.sleep(args.sleep)
        verdict, broken_on = classify(path, paths_by_host, row)
        row["verdict"] = verdict
        row["broken_on"] = broken_on
        row["in_prod_crawl"] = path in paths_by_host["prod"]
        row["in_preprod_crawl"] = path in paths_by_host["preprod"]
        verdicts[path] = row

    report["verdicts"] = verdicts
    report["summary"] = {
        verdict: sum(1 for row in verdicts.values() if row["verdict"] == verdict)
        for verdict in ("MANQUANT_EN_PREPROD", "MANQUANT_EN_PROD", "sain",
                        "CASSE_CHEZ_LE_DEMANDEUR", "indetermine")
    }

    with open(args.out, "w", encoding="utf-8") as handle:
        json.dump(report, handle, ensure_ascii=False, indent=1)

    print(json.dumps(report["summary"], ensure_ascii=False, indent=1))
    print(f"rapport -> {args.out}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
