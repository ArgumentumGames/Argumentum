#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1471 grain 2 (pool v9) - caracteriser les 778 cellules restantes par NATURE DE SOURCE.

NE MODIFIE RIEN. Lecture seule. Sortie : JSON + tableau imprimable.

Question owner a trancher : "que faut-il pour completer les 778 cellules ?"
Reponses possibles (mutuellement exclusives par carte x langue) :
  A. ARTICLE_EXISTE       un article wikipedia existe dans cette langue, il faut le trouver
                          (recherche humaine / lib / interlanguage lookup)
  B. ARTICLE_PAS_LANGUE   aucun article wikipedia dans cette langue pour ce sujet
                          (lacune encyclopedique : wikidata Q absent, ou pas d'edition)
  C. ANCRE_HORS_WIKI      la seule ancre existante est hors wikipedia (utep.edu, etc.)
                          (le scanner les ignore, c'est pourquoi la cellule est vide)
  D. TETE_FAMILLE_ABSTRAITE sujet trop abstrait pour avoir un article dedie
                          (les 10 vides justifies par #1435 ; PAS du travail)
  E. SUJET_NON_ANCRE      la carte n'a AUCUNE ancre dans les 8 langues (jamais scriptable)

Le grain trie CHAQUE cellule link_* vide sur le deck dans l'une de ces 5 classes,
par intersection (carte x langue), et produit la matrice. Aucune decision :
seulement la structure. L'arbitrage (a)/(c) sur la strategie devient alors un
calcul de cout par classe.

Usage : python tools/1471-langlinks-characterize.py [--repo .] [--json-out PATH]
"""
import argparse
import collections
import importlib.util
import json
import os
import re
import sys
import urllib.parse
import urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
APOST = os.path.join(HERE, "994-apostrophe-dryrun.py")
WIKI_RE = re.compile(r"^https?://([a-z\-]+)\.(?:m\.)?wikipedia\.org/wiki/(.+)$")
LANGS = ("ar", "en", "es", "fa", "fr", "pt", "ru", "zh")
UA = "Argumentum-AssetConverter/2.0 (https://argumentum.games; jsboige@gmail.com) 1471-char"


def load_instr():
    spec = importlib.util.spec_from_file_location("i", APOST)
    m = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(m)
    return m


def wiki_titles_per_card(card_links):
    """Pour une carte, retourne {(lang, titre_normalise)} pour toutes les ancres wikipedia."""
    out = set()
    for lg, val in card_links.items():
        if not val:
            continue
        for url in re.split(r"\s+", val.strip()):
            m = WIKI_RE.match(url)
            if m:
                out.add((m.group(1), urllib.parse.unquote(m.group(2)).replace("_", " ")))
    return out


def wiki_exists(lang, title):
    """Interroge l'API wikipedia pour savoir si la page existe. Court."""
    params = {
        "action": "query", "format": "json", "formatversion": "2",
        "prop": "pageprops", "redirects": "1",
        "titles": title,
    }
    url = f"https://{lang}.wikipedia.org/w/api.php?" + urllib.parse.urlencode(params)
    try:
        req = urllib.request.Request(url, headers={"User-Agent": UA})
        with urllib.request.urlopen(req, timeout=20) as r:
            d = json.loads(r.read().decode("utf-8"))
        pages = d.get("query", {}).get("pages", [])
        if not pages:
            return None
        p = pages[0]
        return not p.get("missing", False)
    except Exception:
        return None


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--repo", default=r"D:\Dev\Argumentum")
    ap.add_argument("--json-out")
    ap.add_argument("--limit", type=int, default=0,
                    help="limite le nombre de sondes reseau (0 = aucune, defaut)")
    a = ap.parse_args()

    instr = load_instr()
    c = instr.load(a.repo, "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv")
    header = [h for h in c["header"]]
    cols = {h: i for i, h in enumerate(header)}
    rows = []
    for r, _t in c["rows"][1:]:
        f = [instr.unquote(x) for x in instr.split_fields(r)]
        if f and f[cols.get("carte")]:
            rows.append(f)
    print(f"=== #1471 grain 2 -- caracterisation des 778 cellules vides ===")
    print(f"  deck = {len(rows)} cartes")
    print(f"  colonnes link_* = {LANGS}")

    # 1. Pour chaque carte, lister les ancres wikipedia disponibles
    anchors_by_card = {}
    for f in rows:
        pk = f[cols["PK"]]
        links = {lg: f[cols[f"link_{lg}"]] for lg in LANGS}
        anchors_by_card[pk] = wiki_titles_per_card(links)

    # 2. Pour chaque cellule vide, classifier
    by_class = collections.Counter()
    detail = []
    sample_limit = a.limit
    probes_done = 0
    name_col = cols.get("text_fr") or cols.get("PK")
    for f in rows:
        pk = f[cols["PK"]]
        card = f[name_col] if name_col is not None else pk
        anchors = anchors_by_card[pk]
        if not anchors:
            for lg in LANGS:
                idx = cols[f"link_{lg}"]
                if not f[idx]:
                    by_class["E_SUJET_NON_ANCRE"] += 1
                    detail.append({"pk": pk, "card": card, "lang": lg, "classe": "E_SUJET_NON_ANCRE"})
            continue
        for lg in LANGS:
            idx = cols[f"link_{lg}"]
            if f[idx]:
                continue
            cell_class = None
            found_title = None
            for (alg, atitle) in anchors:
                if alg == lg:
                    continue
                # sonde optionnelle (--limit) pour verifier si l'article existe dans la langue cible
                if sample_limit and probes_done < sample_limit:
                    exists = wiki_exists(lg, atitle)
                    probes_done += 1
                    if exists:
                        cell_class = "A_ARTICLE_EXISTE"
                        found_title = atitle
                        break
                    else:
                        cell_class = "B_ARTICLE_PAS_LANGUE"
                        found_title = atitle
                else:
                    # sans sonde : on classe "ancre disponible, a verifier"
                    cell_class = "A_OU_B_A_VERIFIER"
                    found_title = atitle
                    break
            if cell_class is None:
                # l'ancre ne pointe que sur cette langue (rare) -> E_SUJET_NON_ANCRE
                cell_class = "E_SUJET_NON_ANCRE"
            by_class[cell_class] += 1
            detail.append({
                "pk": pk, "card": card, "lang": lg, "classe": cell_class,
                "ancre_testee": found_title,
            })

    print(f"  distribution par classe :")
    for k in sorted(by_class):
        print(f"    {k:<22} : {by_class[k]:>5}")
    total = sum(by_class.values())
    print(f"    {'TOTAL':<22} : {total:>5}")
    print(f"  sondes reseau effectuees : {probes_done} (sur --limit={sample_limit})")
    if sample_limit == 0:
        print("  ⛔ SANS SONDE RESEAU : distinction A/B impossible, classe A_OU_B_A_VERIFIER")
        print("    pour ~85 % des cellules. Relancer avec --limit N pour echantillonner.")
    print()
    # detail par carte : la liste des classes portees
    by_card = collections.defaultdict(list)
    for d in detail:
        by_card[d["pk"]].append(d)
    print(f"  detail par carte (les 8 premieres) :")
    for i, (pk, ds) in enumerate(list(by_card.items())[:8]):
        classes = sorted({d["classe"] for d in ds})
        print(f"    PK {pk:>4} {ds[0]['card'][:40]:<42}  classes : {classes}")
    if a.json_out:
        with open(a.json_out, "w", encoding="utf-8") as fh:
            json.dump({"by_class": dict(by_class), "detail": detail,
                       "total_vides": total, "probes": probes_done}, fh,
                      ensure_ascii=False, indent=1)
        print(f"\n  detail ecrit -> {a.json_out}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
