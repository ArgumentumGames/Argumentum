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
     - tout le reste                   -> indetermine

Lecture seule stricte : uniquement des GET, aucun formulaire, aucun POST, aucun
en-tete d'authentification.  Cadence volontairement basse (``--sleep``).

Usage
-----
    python tools/cutover-gap-sweep.py --out rapport.json
    python tools/cutover-gap-sweep.py --max-pages 60      # passe rapide
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


def main(argv: list[str] | None = None) -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--out", default="cutover-gap-sweep.json")
    parser.add_argument("--max-pages", type=int, default=400)
    parser.add_argument("--max-depth", type=int, default=6)
    parser.add_argument("--sleep", type=float, default=0.10)
    parser.add_argument("--timeout", type=int, default=30)
    parser.add_argument("--prod", default=PROD)
    parser.add_argument("--preprod", default=PREPROD)
    args = parser.parse_args(argv)
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
        prod_status = row["prod"].get("status")
        preprod_status = row["preprod"].get("status")
        ok_prod = prod_status is not None and prod_status < 400
        ok_preprod = preprod_status is not None and preprod_status < 400
        if ok_prod and not ok_preprod:
            verdict = "MANQUANT_EN_PREPROD"
        elif ok_preprod and not ok_prod:
            verdict = "MANQUANT_EN_PROD"
        elif ok_prod and ok_preprod:
            verdict = "sain"
        else:
            verdict = "indetermine"
        row["verdict"] = verdict
        row["in_prod_crawl"] = path in paths_by_host["prod"]
        row["in_preprod_crawl"] = path in paths_by_host["preprod"]
        verdicts[path] = row

    report["verdicts"] = verdicts
    report["summary"] = {
        verdict: sum(1 for row in verdicts.values() if row["verdict"] == verdict)
        for verdict in ("MANQUANT_EN_PREPROD", "MANQUANT_EN_PROD", "sain", "indetermine")
    }

    with open(args.out, "w", encoding="utf-8") as handle:
        json.dump(report, handle, ensure_ascii=False, indent=1)

    print(json.dumps(report["summary"], ensure_ascii=False, indent=1))
    print(f"rapport -> {args.out}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
