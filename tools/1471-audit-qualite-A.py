#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1471 grain ② (pool v10) — audit qualite des 38 cellules A.

L'instrument originel `1471-langlinks-characterize.py` classe A_ARTICLE_EXISTE
une cellule des qu'un appel `wiki_exists(lg, titre)` reussit (article existe).
Mais un 200 HTTP ne dit pas que la page parle DU BON SUJET : elle peut etre
une page d'homonymie, une redirection vers un autre article, ou un article
qui mentionne le sophisme sans lui etre consacre.

Cet instrument mesure la QUALITE des 24 cellules A officielles (dernier
recomptage du 2026-09-22, --limit 9999) :
  - extrait l'intro wiki (action=query&prop=extracts&exintro=1&explaintext=1)
  - demande a l'API si la page a une wikidata Q-id (presence d'un PageProps
    wikibase_item)
  - en deduit une classification automatique par heuristique de MOTS-CLES :
      SUJET_CORRECT   = intro contient le mot-cle du sophisme
      HOMONYMIE       = wikibase_item absent OU intro tres courte
      HORS_SUJET      = intro ne contient aucun mot-cle du sophisme
  - archive les intros + classification dans un JSON pour audit humain

ZERO-CORPUS-WRITE. 0 ecriture CSV.

Usage :
    python tools/1471-audit-qualite-A.py [--json-out PATH]
"""
import argparse
import csv
import json
import os
import re
import sys
import time
import urllib.parse
import urllib.request

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_PATH = os.path.join(REPO, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")
LANGS = ("ar", "en", "es", "fa", "fr", "pt", "ru", "zh")
UA = "Argumentum-AssetConverter/2.0 (https://argumentum.games; jsboige@gmail.com) 1471-auditA"


def read_csv():
    with open(CSV_PATH, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.reader(f))
    return rows[0], rows[1:]


def wiki_get_extract(lang, title, timeout=15):
    """Retourne (extract, wikibase_item) ou (None, None)."""
    params = {
        "action": "query", "format": "json", "formatversion": "2",
        "prop": "extracts|pageprops", "exintro": "1", "explaintext": "1",
        "redirects": "1", "titles": title,
    }
    url = f"https://{lang}.wikipedia.org/w/api.php?" + urllib.parse.urlencode(params)
    try:
        req = urllib.request.Request(url, headers={"User-Agent": UA})
        with urllib.request.urlopen(req, timeout=timeout) as r:
            d = json.loads(r.read().decode("utf-8"))
        pages = d.get("query", {}).get("pages", [])
        if not pages:
            return None, None
        p = pages[0]
        extract = (p.get("extract") or "").strip()[:500]
        wikibase = p.get("pageprops", {}).get("wikibase_item")
        return extract, wikibase
    except Exception as e:
        return None, None


def normalize(s):
    """Normalise pour comparaison : minuscules + retire accents (approx)."""
    s = (s or "").lower()
    # Table de translitteration simple ASCII
    repl = str.maketrans({
        "à": "a", "â": "a", "ä": "a", "á": "a",
        "è": "e", "ê": "e", "ë": "e", "é": "e",
        "î": "i", "ï": "i", "í": "i",
        "ô": "o", "ö": "o", "ó": "o",
        "ù": "u", "û": "u", "ü": "u", "ú": "u",
        "ç": "c", "ñ": "n", "ß": "ss",
    })
    return s.translate(repl)


def extract_keywords(card_text, ancre):
    """Mots-cles derives du titre FR de la carte + ancre latine.

    On prend le titre FR (deja traduit) + on extrait le radical de l'ancre.
    Strategie : prendre les mots >3 chars du titre FR en minuscules.
    """
    kws = set()
    if card_text:
        for w in re.findall(r"\w{4,}", card_text.lower()):
            kws.add(w)
    return kws


def classify(extract, wikibase, ancre):
    """Classifie en SUJET_CORRECT / HOMONYMIE / HORS_SUJET.

    Heuristique amelioree :
      - HOMONYMIE si extract absent ou tres court (< 30 chars)
      - HOMONYMIE si wikibase_item absent (page d'homonymie typique)
      - SUJET_CORRECT si l'extract mentionne un MOT-CLE LOGIQUE (falacia, sophisme,
        fallacy, argumentum, sofisma, مغالطة, مغالطة, ...) OU contient l'ancre
        elle-meme (cas 'Ad hoc', 'Argumentum ad X', ...)
      - HORS_SUJET sinon : la page existe mais ne parle pas d'un sophisme
    """
    if not extract or len(extract) < 30:
        return "HOMONYMIE"
    if not wikibase:
        return "HOMONYMIE"
    norm = normalize(extract)
    # Mots-cles logiques : les termes qui signalent qu'on parle D'un sophisme
    LOGIC_TERMS = (
        "falacia", "fallacy", "fallacies", "sophisme", "sofisma",
        "argumentum", "argument", "argomento", "falsche", "schluss",
        "مغالطة",  # ar : مغالطة
        "sophism", "مغالطة", "مغالطه", "سفسطة",
        "谬误", "谬论",
        "argumentum",
    )
    has_logic = any(t in norm for t in LOGIC_TERMS)
    ancre_norm = normalize(ancre)
    # L'ancre elle-meme doit etre dans l'extract (au moins partiellement)
    # pour les ancrex courtes comme "Ad hoc", on teste une sous-chaine
    ancre_present = (ancre_norm in norm) or (
        len(ancre_norm) >= 6 and ancre_norm.split()[0] in norm
    )
    if has_logic or ancre_present:
        return "SUJET_CORRECT"
    return "HORS_SUJET"


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--json-out")
    a = ap.parse_args()

    print("=== #1471 grain ② — audit qualite cellules A (24 cellules) ===\n", flush=True)
    print(f"UA : {UA}", flush=True)

    header, rows = read_csv()
    idx = {h: i for i, h in enumerate(header)}

    # Lire le JSON produit par le scanner originel (avec --limit 9999)
    # Sinon, regenerer --limit 9999 et extraire les A
    src = os.path.join(REPO, "tools", "1471-resond-e-classe-officialE50.json")
    # Le JSON produit par le scanner originel est dans /tmp/char1471_full.json
    char_path = r"C:/Users/jsboi/AppData/Local/Temp/char1471_full.json"
    if not os.path.exists(char_path):
        print(f"  ⛔ Fichier {char_path} absent -- relancer d'abord :")
        print(f"     python tools/1471-langlinks-characterize.py --limit 9999 --json-out {char_path}")
        return 1
    with open(char_path, encoding="utf-8") as f:
        char_data = json.load(f)

    A_cells = [r for r in char_data["detail"] if r["classe"] == "A_ARTICLE_EXISTE"]
    print(f"Cellules A officielles : {len(A_cells)}")

    results = []
    by_class = {"SUJET_CORRECT": 0, "HOMONYMIE": 0, "HORS_SUJET": 0}
    for i, cell in enumerate(A_cells, 1):
        pk = cell["pk"]
        lang = cell["lang"]
        ancre = cell["ancre_testee"]
        # Trouver le titre FR de la carte
        title_fr = None
        for r in rows:
            if r[idx["PK"]] == pk:
                title_fr = r[idx.get("text_fr", -1)] if "text_fr" in idx else None
                break
        if not title_fr:
            title_fr = "(absent)"
        keywords = extract_keywords(title_fr, ancre)
        extract, wikibase = wiki_get_extract(lang, ancre)
        cls = classify(extract, wikibase, ancre)
        by_class[cls] += 1
        results.append({
            "pk": pk, "lang": lang, "ancre": ancre, "title_fr": title_fr,
            "wikibase_item": wikibase, "extract_preview": extract,
            "keywords": sorted(keywords),
            "class": cls,
        })
        print(f"  [{i:2}/{len(A_cells)}] PK {pk:>4} {lang} ancre={ancre!r} -> {cls}", flush=True)
        time.sleep(0.5)

    print(f"\nDistribution :")
    for cls, n in sorted(by_class.items()):
        print(f"  {cls:<16} : {n}")

    if a.json_out:
        with open(a.json_out, "w", encoding="utf-8") as f:
            json.dump({
                "total_A_official": len(A_cells),
                "by_class": by_class,
                "details": results,
            }, f, ensure_ascii=False, indent=1)
        print(f"\nResultat ecrit -> {a.json_out}")


if __name__ == "__main__":
    sys.exit(main() or 0)
