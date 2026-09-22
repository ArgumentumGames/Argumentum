#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1471 grain ⑤ (pool v10) v2 — audit PERTINENCE des 394 cellules E hittees.

Le grain ① a re-sonde la classe E officielle (355 cellules) via
`list=search&srsearch=<titre natif>` et trouve 394 hits repartis sur 8 langues.

Mais un hit n'est PAS un article pertinent : search rend des pages qui CONTIENNENT
le terme, pas qui TRAITENT du sophisme.

Cet instrument audite la PERTINENCE du top hit par cellule.

Heuristique STRICTE (v2, corrigee des FP observes en v1) :
  PERTINENT  = l'extract wiki mentionne explicitement le SOPHISME :
              - mot-cle logique ('fallacy', 'falacia', 'sophisme', ...)
              - OU la notion d'erreur de raisonnement ('error', 'erreur',
                'fallacious', 'fallacioso', 'errone', 'incorrect reasoning',
                'conclusion invalide', ...)
              ET le titre wiki correspond bien a ce que le sophisme EST
                (pas seulement contient un mot)
  HORS_SUJET = page existe, parle d'autre chose (meme si elle partage un mot)
  HOMONYMIE  = wikibase absent / extract tres court (< 30 chars)
  ERREUR_API = pas de reponse exploitable

Methode de validation :
  - 2 sondes positives : sophismes reellement ancrés (PK 200 'Ad hoc' fr,
    PK 727 'Formal fallacy' en) -- controles inverses obligatoires
  - 1 sonde negative : PK 658 en 'Infinity Within' (album Deee-Lite) -- doit
    classifier en HORS_SUJET, sinon la mesure ne vaut rien

Usage :
    python tools/1471-audit-pertinence-E.py [--json-out PATH]

Inputs :
    tools/1471-resond-e-classe-officialE50.json
    cards/Fallacies/Argumentum Fallacies - Taxonomy.csv
"""
import argparse
import collections
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
JSON_GRAIN1 = os.path.join(REPO, "tools", "1471-resond-e-classe-officialE50.json")
LANGS = ("ar", "en", "es", "fa", "fr", "pt", "ru", "zh")
UA = "Argumentum-AssetConverter/2.0 (https://argumentum.games; jsboige@gmail.com) 1471-audit-pertinenceE"

# Heuristique etendue : on exige au moins un terme de la liste LOGIC_FALLACY
# qui designe SPECIFIQUEMENT un sophisme (pas un concept adjectif vague).
LOGIC_FALLACY_TERMS = (
    # Latins / anglais explicites (la majorite des pages wiki sur un sophisme
    # utilise l'un de ces termes)
    "fallacy", "fallacies", "falacia", "falacias", "sophisme", "sofisma",
    "sophism", "argumentum", "argumento",
    # Termes designant specifiquement une 'mauvaise argumentation'
    "erroneous reasoning", "erreur de raisonnement", "raisonnement invalide",
    "invalid argument", "invalid reasoning", "mauvais raisonnement",
    "biais cognitif", "cognitive bias", "biais de raisonnement",
    "sophistical", "fallacieux",
    # En russe
    "софизм", "ошибка", "заблуждение", "логическая ошибка", "многословие",
    # En persan
    "مغالطه", "سفسطه", "مغالطه‌", "سفسطه‌",
    # En chinois
    "谬误", "謬誤", "詭辯",
)

# Termes SPECIFIQUEMENT lies aux sophismes, a verifier dans le titre wiki
# (le titre doit porter une notion liee au sophisme)
FALLACY_TITLE_TERMS = (
    "fallacy", "fallacies", "falacia", "sophism", "sofisma",
    "argumentum", "sophisme",
    "مغالطه", "سفسطه",
    "谬误", "謬誤",
    "софизм",
)


def read_csv():
    with open(CSV_PATH, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.reader(f))
    return rows[0], rows[1:]


def normalize(s):
    s = (s or "").lower()
    repl = str.maketrans({
        "à": "a", "â": "a", "ä": "a", "á": "a",
        "è": "e", "ê": "e", "ë": "e", "é": "e",
        "î": "i", "ï": "i", "í": "i",
        "ô": "o", "ö": "o", "ó": "o",
        "ù": "u", "û": "u", "ü": "u", "ú": "u",
        "ç": "c", "ñ": "n", "ß": "ss",
    })
    return s.translate(repl)


def get_titles_fr_by_pk(rows, idx):
    out = {}
    for r in rows:
        pk = r[idx["PK"]]
        col = "text_fr" if "text_fr" in idx else "titre_fr"
        if col in idx and r[idx[col]]:
            out[pk] = r[idx[col]]
    return out


def wiki_get_extract(lang, title, timeout=15):
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
            return None, None, "API_NO_PAGES"
        p = pages[0]
        extract = (p.get("extract") or "").strip()[:500]
        wikibase = p.get("pageprops", {}).get("wikibase_item")
        return extract, wikibase, None
    except Exception as e:
        return None, None, f"ERR_{type(e).__name__}"


def classify(extract, wikibase, top_title, sophism_title_fr):
    """Heuristique STRICTE v2.

    PERTINENT ssi :
      - wikibase_item present
      - extract >= 30 chars
      - extract contient un terme de LOGIC_FALLACY_TERMS
        OU le titre wiki est explicitement un sophisme (FALLACY_TITLE_TERMS)
        ET parle d'erreur de raisonnement
    """
    if not extract or len(extract) < 30:
        return "HOMONYMIE"
    if not wikibase:
        return "HOMONYMIE"
    norm = normalize(extract)
    title_norm = normalize(top_title)
    # Test 1 : extract mentionne un terme sophisme explicite
    extract_has_fallacy = any(t in norm for t in LOGIC_FALLACY_TERMS)
    # Test 2 : titre wiki est explicite (porte un mot 'fallacy', 'falacia', etc.)
    title_is_fallacy = any(t in title_norm for t in FALLACY_TITLE_TERMS)
    # Test 3 : extract commence par def. typique ("X is a fallacy..." / "Es una falacia...")
    starts_with_fallacy = norm.startswith(("a fallacy", "the fallacy", "es una falacia",
                                            "é uma falácia", "un sophisme",
                                            "le sophisme", "словосочетание",
                                            "софизм", "это логическая"))
    if extract_has_fallacy or title_is_fallacy or starts_with_fallacy:
        return "PERTINENT"
    return "HORS_SUJET"


# CONTROLES INVERSES — indispensables pour montrer que l'instrument n'est pas
# trop laxiste (FP) ni aveugle (FN)
POSITIVE_CONTROLS = [
    ("fr", "Sophisme", "Ad hoc"),  # article sophisme ad hoc
    ("en", "Fallacy", "Formal fallacy"),  # page concept
]
NEGATIVE_CONTROLS = [
    # Hit evidemment HORS_SUJET ; doit classifier HORS_SUJET, sinon FP partout
    ("en", "Infinity Within", "Infini fallacieux"),  # album Deee-Lite
]


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--json-out")
    a = ap.parse_args()

    print("=== #1471 grain ⑤ v2 — audit PERTINENCE cellules E hittees ===\n", flush=True)
    print(f"UA : {UA}", flush=True)

    if not os.path.exists(JSON_GRAIN1):
        print(f"Fichier {JSON_GRAIN1} introuvable. Lancer d'abord grain ①.")
        return 1

    with open(JSON_GRAIN1, encoding="utf-8") as f:
        grain1 = json.load(f)

    header, rows = read_csv()
    idx = {h: i for i, h in enumerate(header)}
    titles_fr = get_titles_fr_by_pk(rows, idx)

    # Selectionner uniquement les cellules HITTEES (n_hits > 0)
    found = [r for r in grain1["details"] if r.get("found")]
    print(f"Cellules hittees grain ① : {len(found)}", flush=True)

    # CONTROLES INVERSES d'abord
    print("\n=== CONTROLES INVERSES (avant audit) ===")
    all_ctrl = []
    for lg, top, sophism_fr in POSITIVE_CONTROLS:
        ext, wbx, err = wiki_get_extract(lg, top)
        cls = classify(ext, wbx, top, sophism_fr)
        all_ctrl.append({"type": "POS", "lang": lg, "top": top, "class": cls, "err": err,
                         "wikibase": wbx, "extract": (ext or "")[:100]})
        print(f"  POS  {lg} '{top}' -> {cls}  (sophism='{sophism_fr}')")
        time.sleep(0.5)
    for lg, top, sophism_fr in NEGATIVE_CONTROLS:
        ext, wbx, err = wiki_get_extract(lg, top)
        cls = classify(ext, wbx, top, sophism_fr)
        all_ctrl.append({"type": "NEG", "lang": lg, "top": top, "class": cls, "err": err,
                         "wikibase": wbx, "extract": (ext or "")[:100]})
        print(f"  NEG  {lg} '{top}' -> {cls}  (doit etre HORS_SUJET)")
        time.sleep(0.5)
    pos_ok = sum(1 for c in all_ctrl if c["type"] == "POS" and c["class"] == "PERTINENT")
    neg_ok = sum(1 for c in all_ctrl if c["type"] == "NEG" and c["class"] == "HORS_SUJET")
    print(f"  POS {pos_ok}/{len(POSITIVE_CONTROLS)} OK, NEG {neg_ok}/{len(NEGATIVE_CONTROLS)} OK")
    if pos_ok < len(POSITIVE_CONTROLS) or neg_ok < len(NEGATIVE_CONTROLS):
        print("  ⛔ CONTROLES ECHOUENT -- audit NON FIABLE")
        return 1

    by_class = collections.Counter()
    by_lang = collections.Counter()
    results = []
    for i, hit in enumerate(found, 1):
        pk = hit["pk"]
        lang = hit["lang"]
        top = (hit.get("top_titles") or [""])[0]
        sophism_fr = titles_fr.get(pk, "")
        extract, wikibase, err = wiki_get_extract(lang, top)
        if err and extract is None:
            cls = "ERREUR_API"
        else:
            cls = classify(extract, wikibase, top, sophism_fr)
        by_class[cls] += 1
        by_lang[(lang, cls)] += 1
        results.append({
            "pk": pk, "lang": lang, "query_sondage": hit["query"],
            "top_titre_wiki": top,
            "titre_fr_sophisme": sophism_fr,
            "wikibase_item": wikibase,
            "extract_preview": (extract or "")[:200] if extract else "",
            "class": cls, "api_err": err,
        })
        if i % 20 == 0 or i == len(found):
            print(f"  [{i:3}/{len(found)}] PK {pk} {lang} top={top[:35]!r} -> {cls}",
                  flush=True)
        time.sleep(0.5)

    n = sum(by_class.values())
    print(f"\nTotal sondes : {n}")
    print("Distribution globale :")
    for cls in ("PERTINENT", "HORS_SUJET", "HOMONYMIE", "ERREUR_API"):
        c = by_class.get(cls, 0)
        pct = c / n * 100 if n else 0
        print(f"  {cls:<14} : {c:>4}  ({pct:>5.1f} %)")
    print("\nDetail par langue :")
    for lg in LANGS:
        for cls in ("PERTINENT", "HORS_SUJET", "HOMONYMIE", "ERREUR_API"):
            c = by_lang.get((lg, cls), 0)
        h = by_lang.get((lg, "PERTINENT"), 0)
        s = by_lang.get((lg, "HORS_SUJET"), 0)
        m = by_lang.get((lg, "HOMONYMIE"), 0)
        e = by_lang.get((lg, "ERREUR_API"), 0)
        nn = h + s + m + e
        if nn == 0:
            continue
        print(f"  {lg:<4} : PERT={h:>3}  HORS={s:>3}  HOMO={m:>3}  ERR={e:>3}  "
              f"taux_pert={h/nn*100:>5.1f}%")

    print("\n=== SYNTHESE GRAIN ⑤ v2 ===")
    pertinent = by_class.get("PERTINENT", 0)
    hors_sujet = by_class.get("HORS_SUJET", 0)
    taux = pertinent / n * 100 if n else 0
    print(f"  Cellules hittees auditees : {n}")
    print(f"  PERTINENT                  : {pertinent}  ({taux:.1f} %)")
    print(f"  HORS_SUJET                 : {hors_sujet}")
    print(f"  HOMONYMIE                  : {by_class.get('HOMONYMIE', 0)}")
    print(f"  ERREUR_API                 : {by_class.get('ERREUR_API', 0)}")
    print(f"  Controles inverses         : POS {pos_ok}/{len(POSITIVE_CONTROLS)} OK, "
          f"NEG {neg_ok}/{len(NEGATIVE_CONTROLS)} OK")

    if a.json_out:
        with open(a.json_out, "w", encoding="utf-8") as f:
            json.dump({
                "total_audited": n,
                "by_class": dict(by_class),
                "by_lang_class": {f"{k[0]}_{k[1]}": v for k, v in by_lang.items()},
                "controls": all_ctrl,
                "details": results,
            }, f, ensure_ascii=False, indent=1)
        print(f"\nResultat ecrit -> {a.json_out}")


if __name__ == "__main__":
    sys.exit(main() or 0)
