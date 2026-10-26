#!/usr/bin/env python3
"""#1066 volet 2a -- parite des balises GTM/GA4 prod <-> preprod, LECTURE SEULE.

Objet : `/terms` et `/privacy` portent-ils `GTM-TZBQ57M` ET `G-VHLTL18PEW` sur la preprod,
comme la prod ? (Runbook de bascule, R4 ; item 5-4 du pool c.6046609129.)

Disciplines d'instrument, reprises du controle du 06/10 et du runbook :
  1. `GET` uniquement -- sur ce site le `HEAD` rend 404 la ou le `GET` rend 200.
  2. Un TEMOIN NEGATIF a chaque passe : un chemin inexistant doit rendre >= 400 des DEUX
     cotes. S'il ne le fait pas, le script SORT EN ERREUR sans imprimer de verdict --
     aucun "200" n'est interpretable derriere un temoin invalide.
  3. Cache-buster sur CHAQUE sonde (le cache prod est a 1 an).
  4. Aucun `catch` n'ecrit un verdict : une exception sort en `error=`, jamais en "404".

⚠️ Piege de sous-chaine, mesure le 08/10 : le marqueur `ns.html` remonte 1 occurrence sur la
page statique `fallacies_fr.html` -- c'est un faux positif, la chaine est prise dans
`.../imptrans.html`. Un marqueur de sous-chaine n'est pas une balise. Les compteurs bruts
sont donc imprimes tels quels, et la page statique est exclue du verdict de parite.

Usage :
    python tools/1066-ga4-marker-parity-probe.py [--prod URL] [--preprod URL]
Sortie : table `prod / preprod` par marqueur, plus une ligne de verdict.
Code retour : 0 si le temoin est valide (le verdict, lui, se lit dans la sortie),
              2 si le temoin est invalide (instrument aveugle).
"""
from __future__ import annotations

import argparse
import sys
from urllib.parse import urljoin

import requests

MARKERS = ["GTM-TZBQ57M", "G-VHLTL18PEW", "gtm.js", "gtag.js", "ns.html", "<noscript>"]

# page -> libelle. `measured=True` = entre dans le verdict de parite.
PAGES = [
    ("/", "accueil", True),
    ("/terms", "mentions legales", True),
    ("/privacy", "confidentialite", True),
    ("/Acheter-le-jeu", "acheter le jeu", True),
    ("/Argumentum", "argumentum", True),
    ("/Amis", "amis", True),
    ("/fallacies_fr.html", "carte mentale (STATIQUE)", False),
]
WITNESS = "/__temoin_negatif_1066_ga4__"


def probe(url: str, ua: str) -> dict:
    """Une sonde. En cas d'echec reseau, rend `error=` -- jamais un verdict."""
    try:
        r = requests.get(url, headers={"User-Agent": ua}, timeout=30, allow_redirects=True)
    except requests.RequestException as exc:
        return {"error": f"{type(exc).__name__}: {exc}"[:160]}
    text = r.text
    return {
        "status": r.status_code,
        "bytes": len(r.content),
        "markers": {m: text.count(m) for m in MARKERS},
    }


def with_cb(url: str, n: int) -> str:
    return f"{url}{'&' if '?' in url else '?'}cb=1066ga4-{n}"


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("--prod", default="https://www.argumentum.games")
    ap.add_argument("--preprod", default="https://dnn.argumentum.myia.io")
    ap.add_argument("--ua", default="Argumentum-ga4-parity-probe/1.0 (read-only; #1066)")
    args = ap.parse_args()

    rows: dict[str, dict] = {}
    for n, (path, label, _) in enumerate(PAGES + [(WITNESS, "TEMOIN NEGATIF", False)], start=1):
        rows[path] = {
            "label": label,
            "prod": probe(with_cb(urljoin(args.prod, path), n), args.ua),
            "preprod": probe(with_cb(urljoin(args.preprod, path), n), args.ua),
        }

    w = rows[WITNESS]
    wp, wq = w["prod"], w["preprod"]
    witness_ok = (
        "error" not in wp and "error" not in wq
        and wp["status"] >= 400 and wq["status"] >= 400
    )
    print(f"TEMOIN NEGATIF {WITNESS}")
    print(f"  prod={wp.get('status', wp.get('error'))}  "
          f"preprod={wq.get('status', wq.get('error'))}  "
          f"-> {'OK' if witness_ok else '*** INSTRUMENT AVEUGLE : AUCUN VERDICT ***'}")
    if not witness_ok:
        return 2
    print()

    hdr = f"{'page':28s} {'prod':>5s} {'prep':>5s} | " + " ".join(f"{m:>13s}" for m in MARKERS)
    print(hdr)
    print("-" * len(hdr))

    differing: list[str] = []
    for path, _label, measured in PAGES:
        row = rows[path]
        p, q = row["prod"], row["preprod"]
        if "error" in p or "error" in q:
            print(f"{path:28s} ERREUR  prod={p.get('error')}  preprod={q.get('error')}")
            continue
        cells = []
        for m in MARKERS:
            a, b = p["markers"][m], q["markers"][m]
            cells.append(f"{a:>6d}/{b:<5d}{' ' if a == b else '!'}")
        tag = "" if measured else "   (hors verdict)"
        print(f"{path:28s} {p['status']:>5d} {q['status']:>5d} | " + " ".join(cells) + tag)
        if measured:
            if p["status"] != q["status"] or p["markers"] != q["markers"]:
                differing.append(path)

    print()
    print("legende : colonnes = prod/preprod ; '!' = les deux cotes DIFFERENT.")
    print()
    if differing:
        print(f"VERDICT : ECART sur {len(differing)} page(s) : {', '.join(differing)}")
    else:
        print("VERDICT : aucune divergence prod<->preprod sur les pages mesurees.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
