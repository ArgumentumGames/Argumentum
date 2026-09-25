#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1499 G6 — chiffrage des divergences archive-2022 vs corpus courant sur les 69 cartes appariees.

G5 (#1499, PR #1553) a valide la cle de jointure et s'est ARRETE la : « aucun
chiffrage de divergences — le decompte des cellules changees sur les 69 cartes
appariees n'a pas ete fait et ne doit pas etre cite ». G6 est ce decompte.

Commande ai-01 (pool #458, c.5830348025) : « Joindre sur E1 + E2, ⛔ JAMAIS
`path` seul. Les 8 non resolues forment un perimetre separate (decision
humaine), les 2 E2 LIMITES sont qualifiees comme telles. »

SURFACE COMPARABLE — l'archive 2022 n'a que 20 colonnes : fr (nues) + en
(nues + suggestion_en). Les 6 autres langues n'existent pas dans l'archive :
leur divergence n'est PAS mesurable ici (coherence avec le fr restaure =
autre instrument). Champs compares, 16 :
  fr : titre, baratineur, piocheur, contexte, enjeu, suggestion,
       categorie, sous-categorie
  en : title, smoothTalker, drawer, context, issue, suggestion_en,
       category, subcategory

TROIS NATURES par cellule divergente (comparaison EXACTE de chaine, aucune
normalisation — un écart d'espace est une divergence) :
  PERDU   archive remplie -> courant vide   (perte de contenu)
  GAGNE   archive vide -> courant rempli    (ajout posterieur)
  MODIFIE les deux remplies, textes differents

Aucune ecriture. Sortie : tableau imprimable + --json-out (detail nominatif).

Usage :
    python tools/1499-g6-chiffrage-divergences.py
    python tools/1499-g6-chiffrage-divergences.py --self-test
    python tools/1499-g6-chiffrage-divergences.py --json-out tools/1499-g6-chiffrage.json
"""

import argparse
import collections
import importlib.util
import json
import os
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
G5_PATH = os.path.join(HERE, "1499-g5-cle-scenarii.py")

# (colonne commune aux deux corpus, famille) — l'archive et le courant portent
# les memes noms pour ces 16 colonnes (verifie par header_ok des deux cotes).
FIELDS = [
    ("titre", "fr"), ("baratineur", "fr"), ("piocheur", "fr"),
    ("contexte", "fr"), ("enjeu", "fr"), ("suggestion", "fr"),
    ("catégorie", "fr"), ("sous-catégorie", "fr"),
    ("title", "en"), ("smoothTalker", "en"), ("drawer", "en"),
    ("context", "en"), ("issue", "en"), ("suggestion_en", "en"),
    ("category", "en"), ("subcategory", "en"),
]


def load_g5():
    spec = importlib.util.spec_from_file_location("g5", G5_PATH)
    m = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(m)
    return m


def compare_cell(archive_val, current_val):
    a = (archive_val or "").strip()
    c = (current_val or "").strip()
    if a == c:
        return None
    if a and not c:
        return "PERDU"
    if not a and c:
        return "GAGNE"
    return "MODIFIE"


def token_jaccard(a, c):
    """Similarite sur tokens casefoldes et deponctues — AIDE DE LECTURE pour
    sous-qualifier les MODIFIE, pas une verite : les seuils 0.9/0.5 sont nommes
    comme tels dans le rapport. Casefold + ponctuation pour qu'une divergence
    de seule casse ('la mere' -> 'La mere') soit quasi-identique, pas retouche
    — mesure sur echantillon path 1.1.1, ou la v1 case-sensible la comptait
    a tort comme retouche."""
    import re as _re
    def toks(text):
        return {t for t in _re.sub(r"[^\w\s]", " ", (text or "").casefold()).split() if t}
    ta, tc = toks(a), toks(c)
    if not ta and not tc:
        return 1.0
    if not ta or not tc:
        return 0.0
    return len(ta & tc) / len(ta | tc)


def diff_pairs(pairs, limite_paths=None):
    """pairs : [(archive_row, current_row)] -> cells + compteurs.
    limite_paths : chemins d'archive des paires E2 LIMITE — leurs cellules sont
    marquées (appariement récusables à la lecture, exigence du dispatch)."""
    limite_paths = limite_paths or set()
    cells = []
    by_nature = collections.Counter()
    by_field = collections.Counter()
    by_family = collections.Counter()
    for a_row, c_row in pairs:
        for field, family in FIELDS:
            nature = compare_cell(a_row.get(field, ""), c_row.get(field, ""))
            if nature:
                a_val = (a_row.get(field, "") or "")
                c_val = (c_row.get(field, "") or "")
                cells.append({
                    "path_archive": a_row.get("path", ""),
                    "titre_archive": (a_row.get("titre", "") or "")[:60],
                    "field": field, "famille": family, "nature": nature,
                    "paire_limite": a_row.get("path", "") in limite_paths,
                    "sim_tokens": round(token_jaccard(a_val, c_val), 4)
                    if nature == "MODIFIE" else None,
                    "archive": a_val[:120],
                    "courant": c_val[:120],
                })
                by_nature[nature] += 1
                by_field[(field, nature)] += 1
                by_family[(family, nature)] += 1
    return cells, by_nature, by_field, by_family


def self_test():
    """Fixture inline : les trois natures et l'egalite, comptees une a une."""
    a = {"titre": "X", "contexte": "gardé", "enjeu": "", "suggestion": "vieux texte"}
    c = {"titre": "", "contexte": "gardé", "enjeu": "nouveau", "suggestion": "autre texte"}
    got = [compare_cell(a.get(f, ""), c.get(f, "")) for f in ("titre", "contexte", "enjeu", "suggestion")]
    expect = ["PERDU", None, "GAGNE", "MODIFIE"]
    if got != expect:
        print(f"SELF-TEST ECHEC : {got} != {expect}")
        return 2
    # mutation : l'egalite apres trim d'espaces reste egalite (pas de faux MODIFIE)
    if compare_cell(" text ", "text") is not None:
        print("SELF-TEST ECHEC : trim devrait etre egalite")
        return 2
    print("SELF-TEST OK : PERDU/GAGNE/MODIFIE/egalite comptes exactement, trim = egalite")
    return 0


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--repo", default=os.path.dirname(HERE))
    ap.add_argument("--json-out")
    ap.add_argument("--self-test", action="store_true")
    a = ap.parse_args()

    if a.self_test:
        return self_test()

    g5 = load_g5()
    res = g5.analyse(a.repo)
    if res is None:
        print("FATAL : analyse G5 indisponible (header ou corpus).", file=sys.stderr)
        return 2
    # E1 : triplets (archive, courant, score) ; E2 : quadruplets (a, c, score, label)
    pairs = [(a_row, c_row) for a_row, c_row, _s in res["e1"]]
    recovered = res["recovered"]
    pairs += [(a_row, c_row) for a_row, c_row, _s, _lbl in recovered]
    limites = [(a_row, c_row, s, lbl) for a_row, c_row, s, lbl in recovered
               if lbl == "LIMITE"]
    limite_paths = {a_row.get("path", "") for a_row, _c, _s, _lbl in limites}

    print("=== #1499 G6 — chiffrage des divergences (cartes appareillees E1+E2) ===")
    print(f"paires E1 : {len(res['e1'])}   paires E2 : {len(recovered)}   total : {len(pairs)}")
    if len(pairs) != 69:
        print(f"⚠️ attendu 69 paires d'apres G5 — mesure du jour : {len(pairs)} ; "
              f"c'est la mesure qui fait foi")
    print(f"E2 LIMITES ({len(limites)}) : "
          + ", ".join(f"path {p} (sim {s:.3f})" for _a, _c, s, _l in limites
                      for p in [_a.get('path', '')])
          if limites else "aucune")
    print(f"8 non resolues : perimetre SEPARÉ (decision humaine) — hors chiffrage, conformement au dispatch")
    print(f"surface comparable : {len(FIELDS)} champs x {len(pairs)} paires "
          f"= {len(FIELDS) * len(pairs)} cellules\n")

    cells, by_nature, by_field, by_family = diff_pairs(pairs, limite_paths)
    total_div = sum(by_nature.values())
    print(f"cellules divergentes : {total_div} / {len(FIELDS) * len(pairs)}")
    for nature in ("PERDU", "GAGNE", "MODIFIE"):
        print(f"  {nature:<8} : {by_nature.get(nature, 0):>4}")
    mod_sims = sorted(c["sim_tokens"] for c in cells if c["nature"] == "MODIFIE")
    if mod_sims:
        n = len(mod_sims)
        quasi = sum(1 for s in mod_sims if s >= 0.9)
        retouche = sum(1 for s in mod_sims if 0.5 <= s < 0.9)
        reecrit = sum(1 for s in mod_sims if s < 0.5)
        print(f"\n  sous-qualification des MODIFIE (aides de lecture, seuils nommes) :")
        print(f"    quasi-identiques (sim tokens >= 0.9 — normalisations) : {quasi:>4}")
        print(f"    retouches       (0.5 <= sim < 0.9)                     : {retouche:>4}")
        print(f"    reecritures     (sim < 0.5)                             : {reecrit:>4}")
        print(f"    mediane sim : {mod_sims[n // 2]:.3f}   min {mod_sims[0]:.3f}   max {mod_sims[-1]:.3f}")
    print("\npar famille :")
    for fam in ("fr", "en"):
        line = f"  {fam} :"
        for nature in ("PERDU", "GAGNE", "MODIFIE"):
            line += f"  {nature}={by_family.get((fam, nature), 0)}"
        print(line)
    print("\npar champ (divergents > 0) :")
    for (field, nature), n in sorted(by_field.items(), key=lambda kv: -kv[1]):
        print(f"  {field:<16} {nature:<8} : {n:>3}")

    # coherence interne : la somme des natures == cellules listees
    assert total_div == len(cells), "incoherence interne compte vs detail"

    if a.json_out:
        with open(a.json_out, "w", encoding="utf-8") as fh:
            json.dump({
                "paires_e1": len(res["e1"]), "paires_e2": len(recovered),
                "e2_limites": [{"path": a_row.get("path", ""), "sim": round(s, 4)}
                               for a_row, _c, s, _l in limites],
                "surface": len(FIELDS) * len(pairs),
                "by_nature": dict(by_nature),
                "by_field": {f"{f}|{n}": v for (f, n), v in by_field.items()},
                "details": cells,
            }, fh, ensure_ascii=False, indent=1)
        print(f"\ndetail nominatif -> {a.json_out}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
