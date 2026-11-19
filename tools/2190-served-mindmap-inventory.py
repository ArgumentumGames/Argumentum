#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#2190 — inventaire lecture seule des 16 cartes intégrées réellement servies.

    python tools/2190-served-mindmap-inventory.py --output <manifest.json>

Pour chaque route `fallacies_<lang>.html` et `Virtues_<lang>.html` (8 langues),
le script compare le SHA-256 des octets servis à celui du BLOB git de `ref`,
et non à un fichier de working tree (dont les fins de ligne dépendent de la
machine). Il enregistre aussi des sondes HTML des marqueurs de navigation :
champ de recherche, dictionnaire MM_UI et relations annotées. Les attributs sont
parsés avec leurs entités décodées, quels que soient guillemets et casse. Ce sont
des marqueurs de structure, pas une preuve du comportement JavaScript. Aucune
écriture HTTP, aucun déploiement.

Le manifeste distingue volontairement :
  * la divergence d'octets servi/dépôt, mesurée ici ;
  * la preuve géométrique C4 des annotations, portée par la garde #2312, que
    l'égalité des marqueurs ou des empreintes ne remplace pas.

Sortie : exit 0 si les 16 pages ont été lues et le manifeste écrit, même si elles
divergent — une divergence est un résultat, pas un échec de l'instrument.
Exit 1 si une page manque, ne répond pas 200 ou si le blob est introuvable.
"""

from __future__ import annotations

import argparse
import concurrent.futures
import datetime
import hashlib
import json
import subprocess
import sys
import urllib.error
import urllib.request
from html.parser import HTMLParser
from pathlib import Path

LANGS = ["ar", "en", "es", "fa", "fr", "pt", "ru", "zh"]
TARGETS = [
    ("fallacies", "fallacies_{lang}.html", "Fallacies_{lang}.html"),
    ("virtues", "Virtues_{lang}.html", "Argumentation_Virtues_{lang}.html"),
]
ROOT = Path(__file__).resolve().parents[1]


def blob_sha(repo: Path, ref: str, path: str):
    """SHA-256 du blob git exact ; hash-object --stdin lit les octets bruts."""
    show = subprocess.run(["git", "-C", str(repo), "show", f"{ref}:{path}"],
                          capture_output=True, check=True)
    oid = subprocess.run(["git", "-C", str(repo), "rev-parse", f"{ref}:{path}"],
                         capture_output=True, text=True, check=True).stdout.strip()
    return hashlib.sha256(show.stdout).hexdigest(), oid, len(show.stdout)


class NavigationMarkers(HTMLParser):
    """Sonde HTML : entités décodées, attributs quotés ou non, noms insensibles à la casse."""

    def __init__(self):
        super().__init__(convert_charrefs=True)
        self.search_input_count = 0
        self.crosslink_source_count = 0

    def handle_starttag(self, tag, attrs):
        # Un attribut sans valeur (`<div id>`) est livré avec `value=None` par
        # HTMLParser : le normaliser en chaîne vide avant tout `.lower()`.
        attributes = {name.lower(): "" if value is None else value
                      for name, value in attrs}
        # Le champ de recherche est un contrôle `<input>` ; un autre élément
        # portant le même id n'est pas un champ (témoin 1f du contrat).
        if tag.lower() == "input" and attributes.get("id", "").lower() == "mmsearch":
            self.search_input_count += 1
        if "data-crosslink-source" in attributes:
            self.crosslink_source_count += 1


def navigation_markers(body: bytes):
    parser = NavigationMarkers()
    parser.feed(body.decode("utf-8", errors="replace"))
    parser.close()
    return {
        "search_input_count": parser.search_input_count,
        "mm_ui_present": b"var MM_UI" in body,
        "crosslink_source_count": parser.crosslink_source_count,
    }


def fetch(base: str, route: str):
    url = f"{base.rstrip('/')}/{route}"
    req = urllib.request.Request(url, headers={
        "User-Agent": "argumentum-mindmap-inventory/1.0",
        "Cache-Control": "no-cache",
    })
    with urllib.request.urlopen(req, timeout=60) as response:
        body = response.read()
        return {
            "requested_url": url,
            "effective_url": response.geturl(),
            "http_status": response.status,
            "served_bytes": len(body),
            "served_sha256": hashlib.sha256(body).hexdigest(),
            **navigation_markers(body),
        }


def main():
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--base", default="https://dnn.argumentum.myia.io")
    ap.add_argument("--ref", default="origin/master")
    ap.add_argument("--repo", type=Path, default=ROOT)
    ap.add_argument("--output", type=Path, required=True)
    args = ap.parse_args()

    ref = subprocess.run(["git", "-C", str(args.repo), "rev-parse", args.ref],
                         capture_output=True, text=True, check=True).stdout.strip()
    work = []
    for family, route_pattern, file_pattern in TARGETS:
        for lang in LANGS:
            route = route_pattern.format(lang=lang)
            path = f"Cards/Fallacies/Mindmaps/{lang}/{file_pattern.format(lang=lang)}"
            sha, oid, size = blob_sha(args.repo, ref, path)
            work.append((family, lang, route, path, sha, oid, size))

    def measure(item):
        family, lang, route, path, sha, oid, size = item
        entry = {"family": family, "lang": lang, "source_path": path,
                 "source_ref": ref, "blob_oid": oid, "blob_bytes": size,
                 "blob_sha256": sha}
        try:
            entry.update(fetch(args.base, route))
        except Exception as exc:
            entry["error"] = f"{type(exc).__name__}: {exc}"
        if "served_sha256" in entry:
            entry["same_bytes_as_blob"] = entry["served_sha256"] == sha
        return entry

    with concurrent.futures.ThreadPoolExecutor(max_workers=4) as pool:
        entries = list(pool.map(measure, work))

    manifest = {
        "schema": 1,
        "measured_at_utc": datetime.datetime.now(datetime.UTC).isoformat(),
        "base": args.base,
        "source_ref": ref,
        "scope": "2 families x 8 languages; no FR-only cards route",
        "hash_definition": "SHA-256(raw served response bytes) vs SHA-256(git blob bytes)",
        "geometric_proof": "Not measured here: C1/C2/C3/C4 are in tools/2190-crosslink-integrity.py",
        "entries": entries,
    }
    args.output.parent.mkdir(parents=True, exist_ok=True)
    args.output.write_text(json.dumps(manifest, ensure_ascii=False, indent=2) + "\n",
                           encoding="utf-8", newline="\n")

    for e in entries:
        if "error" in e:
            print(f"{e['family']:9} {e['lang']} ERREUR {e['error']}")
        else:
            print(f"{e['family']:9} {e['lang']} HTTP {e['http_status']} "
                  f"same={e['same_bytes_as_blob']} search={e['search_input_count']} "
                  f"MM_UI={e['mm_ui_present']} crosslinks={e['crosslink_source_count']} "
                  f"served={e['served_sha256'][:12]} blob={e['blob_sha256'][:12]}")
    ok = sum(e.get("http_status") == 200 for e in entries)
    same = sum(e.get("same_bytes_as_blob") is True for e in entries)
    print(f"\n{ok}/{len(entries)} HTTP 200 ; {same}/{len(entries)} identiques au blob ; "
          f"manifeste: {args.output}")
    return 0 if ok == len(entries) else 1


if __name__ == "__main__":
    sys.exit(main())
