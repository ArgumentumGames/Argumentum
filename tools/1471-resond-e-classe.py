#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1471 grain ① (pool v10) — re-sonder la classe E avec un instrument DISTINCT.

L'instrument originel `1471-langlinks-characterize.py` classe une cellule en
E_SUJET_NON_ANCRE quand la carte ne porte AUCUNE ancre wikipedia dans les 8
langues. On s'apprete a abandonner 355 cellules sur cette base -- une
affirmation negative produite par un seul instrument (cf. l'avertissement
d'instrument du pool v10).

Ce script RE-SONDE la classe E par un chemin DIFFERENT :
  - on prend le TITRE NATIF du sophisme en chaque langue (colonne text_<lang>
    du CSV, deja traduit par les passes i18n) ;
  - on lance `action=query&list=search&srsearch=<titre natif>` contre la wiki
    cible -- chemin different de `prop=langlinks` qui traduit automatiquement ;
  - une reponse non vide de `search` indique qu'un article existe, sous un
    titre possiblement different de celui qu'on a transmis.

Definition de la classe E (meme que le scanner originel) :
  Une carte est en classe E officielle ssi AUCUNE de ses 8 cellules link_<lang>
  ne contient une URL wikipedia.org/wiki/. Les URLs hors-wikipedia (cairn.info,
  halshs, sophismes.net, ...) ne comptent PAS comme ancre wiki.

Instrumentation de l'incertitude :
  - echantillon >= 40 cellules de classe E
  - intervalle de confiance binomial (Wilson) sur le taux de faux-<< non ancre >>
  - controle inverse : >= 3 sujets REPUTES ANCRES sont re-sondes -- l'instrument
    doit les trouver (sinon, l'instrument est aveugle et le resultat ne vaut rien)

ZERO-CORPUS-WRITE : lecture seule.

Usage :
    python tools/1471-resond-e-classe.py [--sample 50] [--json-out PATH]
"""
import argparse
import collections
import csv
import json
import math
import os
import re
import sys
import time
import urllib.parse
import urllib.request

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_PATH = os.path.join(REPO, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")
LANGS = ("ar", "en", "es", "fa", "fr", "pt", "ru", "zh")
UA = "Argumentum-AssetConverter/2.0 (https://argumentum.games; jsboige@gmail.com) 1471-resond"

# Sujets REPUTES ANCRES : titres canoniques qui existent forcement dans fr+en
# depuis longtemps ; servent de CONTROLE INVERSE pour prouver que l'instrument
# n'est pas aveugle. Source : wikipedia, en.wikipedia.org/wiki/Fallacy, etc.
POSITIVE_CONTROLS = [
    ("fr", "Sophisme"),
    ("en", "Fallacy"),
    ("es", "Falacia"),
    ("ar", "السفسطة"),
    ("de", "Fehlschluss"),  # pas dans le scope des 8 langues -- juste pour verifier
]


def read_csv():
    with open(CSV_PATH, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.reader(f))
    return rows[0], rows[1:]


def get_class_e_candidates(header, rows):
    """Renvoie la liste des (pk, text_<lang>) pour les cartes classe E OFFICIELLES.

    Meme algo que 1471-langlinks-characterize.py : wiki_titles_per_card vide
    sur les 8 link_<lang> -> classe E.
    """
    WIKI_RE = re.compile(r"https?://([a-z\-]+)\.(?:m\.)?wikipedia\.org/wiki/(.+)")
    idx = {h: i for i, h in enumerate(header)}
    out = []
    for r in rows:
        pk = r[idx["PK"]]
        # Meme filtre que le scanner originel : si 'carte' absent ou vide, on EXCLUT
        if "carte" not in idx or not r[idx["carte"]]:
            continue
        wiki_titles = set()
        for lg in LANGS:
            col = f"link_{lg}"
            if col not in idx:
                continue
            val = r[idx[col]] or ""
            for url in re.split(r"\s+", val.strip()):
                wm = WIKI_RE.match(url)
                if wm:
                    wiki_titles.add((wm.group(1), urllib.parse.unquote(wm.group(2)).replace("_", " ")))
        if not wiki_titles:
            # Carte classe E officielle : on collecte les titres natifs
            titles = {}
            for lg in LANGS:
                col = f"text_{lg}"
                if col in idx and r[idx[col]]:
                    titles[lg] = r[idx[col]]
            out.append((pk, titles))
    return out


def wiki_search(lang, query, timeout=15):
    """Appel API : list=search&srsearch=<query> -- retourne (nb_hits, top_titres).

    Chemin DIFFERENT de wiki_exists() : on cherche par MOTS-CLES au lieu de
    demander un titre exact. Renvoie le nombre de resultats et les 3 premiers.
    """
    params = {
        "action": "query", "format": "json", "formatversion": "2",
        "list": "search", "srsearch": query, "srlimit": "5",
    }
    url = f"https://{lang}.wikipedia.org/w/api.php?" + urllib.parse.urlencode(params)
    try:
        req = urllib.request.Request(url, headers={"User-Agent": UA})
        with urllib.request.urlopen(req, timeout=timeout) as r:
            d = json.loads(r.read().decode("utf-8"))
        search = d.get("query", {}).get("search", [])
        return len(search), [s.get("title", "") for s in search[:3]]
    except Exception as e:
        return None, [f"ERR: {type(e).__name__}: {str(e)[:60]}"]


def wilson_interval(successes, total, z=1.96):
    """Intervalle de confiance binomial de Wilson (95 %)."""
    if total == 0:
        return (0.0, 0.0)
    p = successes / total
    denom = 1 + z**2 / total
    centre = (p + z**2 / (2 * total)) / denom
    half = z * math.sqrt(p * (1 - p) / total + z**2 / (4 * total**2)) / denom
    return (max(0.0, centre - half), min(1.0, centre + half))


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--sample", type=int, default=50,
                    help="taille echantillon (defaut 50, min 40 selon DoD)")
    ap.add_argument("--json-out")
    ap.add_argument("--timeout", type=int, default=15,
                    help="timeout par sonde en secondes (defaut 15)")
    a = ap.parse_args()

    print("=== #1471 grain ① — re-sondage classe E (chemin distinct) ===\n", flush=True)
    print(f"UA : {UA}", flush=True)
    print(f"Timeout par sonde : {a.timeout}s", flush=True)

    header, rows = read_csv()
    candidates = get_class_e_candidates(header, rows)
    print(f"Candidats classe E detectes : {len(candidates)}")

    sample_size = max(a.sample, 40)
    sample = candidates[:sample_size]
    print(f"Echantillon sonde         : {len(sample)}")

    results = []
    by_lang = collections.Counter()
    false_negatives_by_lang = collections.Counter()
    probes_done = 0
    total_planned = sum(1 for _, titles in sample for lg in LANGS if lg in titles)
    print(f"Sondes prevues : {total_planned}", flush=True)
    for pk, titles in sample:
        for lg in LANGS:
            if lg not in titles:
                continue
            query = titles[lg]
            n_hits, top = wiki_search(lg, query, timeout=a.timeout)
            probes_done += 1
            found = n_hits is not None and n_hits > 0
            by_lang[(lg, "hit" if found else "miss")] += 1
            if found:
                false_negatives_by_lang[lg] += 1
            results.append({
                "pk": pk, "lang": lg, "query": query,
                "n_hits": n_hits, "top_titles": top,
                "found": found,
            })
            if probes_done % 5 == 0:
                print(f"  sonde {probes_done}/{total_planned} (pk {pk}, {lg}) -- "
                      f"{n_hits} hits", flush=True)
            time.sleep(0.6)

    total_probes = sum(by_lang.values())
    n_found = sum(false_negatives_by_lang.values())
    fn_rate = n_found / total_probes if total_probes else 0.0
    lo, hi = wilson_interval(n_found, total_probes)

    print(f"\nSondes effectuees        : {total_probes}")
    print(f"Trouvailles (hits > 0)   : {n_found}  ({fn_rate*100:.1f} %)")
    print(f"Intervalle Wilson 95 %   : [{lo*100:.1f} %, {hi*100:.1f} %]")
    print(f"\nDetail par langue :")
    print(f"  {'langue':<8} {'hits':>5} {'miss':>5} {'taux_hit':>10}")
    for lg in LANGS:
        h = by_lang.get((lg, "hit"), 0)
        m = by_lang.get((lg, "miss"), 0)
        n = h + m
        if n == 0:
            continue
        print(f"  {lg:<8} {h:>5} {m:>5} {h/n*100:>9.1f} %")

    print(f"\n=== CONTROLE INVERSE (sujets REPUTES ANCRES) ===")
    print(f"L'instrument doit trouver ces titres ; sinon, il est aveugle.")
    ctrl_results = []
    for lg, q in POSITIVE_CONTROLS:
        n, top = wiki_search(lg, q, timeout=a.timeout)
        ok = n is not None and n > 0
        ctrl_results.append({"lang": lg, "query": q, "n_hits": n, "ok": ok})
        marker = "OK " if ok else "KO "
        print(f"  [{marker}] {lg:<5} '{q}' -> {n} hit(s), top={top}")
        time.sleep(0.6)

    n_ctrl_ok = sum(1 for r in ctrl_results if r["ok"])
    print(f"\nControles OK : {n_ctrl_ok}/{len(ctrl_results)}")

    print(f"\n=== SYNTHESE GRAIN ① ===")
    print(f"  Echantillon            : {sample_size} cartes (>= 40 selon DoD)")
    print(f"  Sondes effectuees       : {total_probes}")
    print(f"  Trouvailles (faux E)    : {n_found} ({fn_rate*100:.1f} %, IC95 [{lo*100:.1f} %, {hi*100:.1f}%])")
    print(f"  Controles positifs OK   : {n_ctrl_ok}/{len(ctrl_results)}")
    if n_ctrl_ok < 3:
        print(f"  ⚠ INSTRUMENT AVEUGLE : <3 controles OK -- taux ci-dessus NON FIABLE")
    if fn_rate > 0:
        print(f"  ⚠ La classe E est sous-estimee par l'instrument originel.")
        print(f"    Extrapolation : 355 cellules * {fn_rate*100:.1f} % = ~{int(355*fn_rate)} "
              f"faux-<< non ancre >>.")
        print(f"    Strate E re-caracterisee AVANT arbitrage owner.")
    else:
        print(f"  Classe E tient : 0 faux-<< non ancre >> trouve, l'instrument originel tient.")
        print(f"    Strate E peut etre tranchee par arbitrage owner (abandon acceptable).")

    if a.json_out:
        with open(a.json_out, "w", encoding="utf-8") as f:
            json.dump({
                "sample_size": sample_size,
                "total_probes": total_probes,
                "n_found": n_found,
                "fn_rate": fn_rate,
                "wilson_lo": lo,
                "wilson_hi": hi,
                "by_lang": {f"{k[0]}_{k[1]}": v for k, v in by_lang.items()},
                "controls": ctrl_results,
                "details": results,
            }, f, ensure_ascii=False, indent=1)
        print(f"\nResultat ecrit -> {a.json_out}")


if __name__ == "__main__":
    main()
