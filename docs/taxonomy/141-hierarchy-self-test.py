#!/usr/bin/env python3
"""#498 grain 1b — controle inverse du module `141-hierarchy`.

    python -I docs/taxonomy/141-hierarchy-self-test.py

Sonde 1 : la hierarchie du CSV VIVANT (read-only) — 1 408 `path` uniques,
0 orphelin, le parent de la rangee 824 (rang 24) existe, dot2comma inverse
comma2dot sur 1 407 rangs et CORRIGE le 1 rang defectueux.
Sonde 2 : controles synthetiques — la direction defectueuse fabrique bien le
parent inexistant (le defaut est REPRODUIT, pas raconte).
"""
import os
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
REPO = os.path.dirname(os.path.dirname(HERE))
sys.path.insert(0, HERE)
import importlib
h = importlib.import_module("141-hierarchy")

CHECKS = []


def check(name, cond):
    CHECKS.append((name, bool(cond)))
    print(f"  [{'OK' if cond else 'FAIL'}] {name}")


def main():
    print("=== 1) Corpus vivant (read-only) ===")
    rows, bypath, bycomma, bydot = h.load_taxonomy(
        os.path.join(REPO, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv"))
    check("1 408 noeuds, 1 408 path uniques", len(rows) == len(bypath) == 1408)

    orphans = [p for p in bypath if h.path_parent(p) and h.path_parent(p) not in bypath]
    check("0 orphelin (chaque parent path existe)", not orphans)

    r824 = bypath.get("5.1.2.3.2.24")
    check("la rangee 824 (rang 24) est presente sous son vrai path", r824 is not None)
    check("son parent derive (5.1.2.3.2) existe dans l'arbre",
          h.path_parent("5.1.2.3.2.24") in bypath)
    check("le rendu coma de cette rangee est la valeur d'origine '5,123224'",
          h.dot2comma("5.1.2.3.2.24") == "5,123224")
    check("la direction defectueuse fabrique le parent faux '5,12322'",
          h.comma_parent_DEFECTUEUX("5,123224") == "5,12322")
    check("la cible POINTEE fabriquee (5.1.2.3.2.2.4) n'existe PAS dans l'arbre "
          "— c'est elle que l'ancien bydot servait aux prompts",
          "5.1.2.3.2.2.4" not in bypath)
    check("subtilite mesuree : '5,12322' EST un noeud reel (5.1.2.3.2.2, frere "
          "de 824) — le parent coma tombe par accident sur un voisin existant, "
          "seule la cible pointee est inexistante",
          bycomma.get("5,12322") is bypath.get("5.1.2.3.2.2"))

    identity = sum(1 for p in bypath if h.comma2dot(h.dot2comma(p)) == p)
    check(f"comma2dot(dot2comma(p)) == p sur 1 407/1 408 (mesure : {identity})",
          identity == 1407)
    corrected = [p for p in bypath if h.comma2dot(h.dot2comma(p)) != p]
    check("l'unique ecart est la rangee au rang 24", corrected == ["5.1.2.3.2.24"])

    # voisinage de la rangee 824 : le parent correct + ses freres, 0 parent faux
    pool = h.build_pool(r824, rows, bypath)
    dots = [p["dotted"] for p in pool]
    check("le pool de 824 contient son parent reel 5.1.2.3.2", "5.1.2.3.2" in dots)
    check("le pool ne contient PAS la cible fabriquee 5.1.2.3.2.2.4",
          "5.1.2.3.2.2.4" not in dots)
    siblings = [p for p in dots if h.path_parent(p) == "5.1.2.3.2"]
    check(f"les freres de 824 (meme parent 5.1.2.3.2) sont dans le pool ({len(siblings)})",
          "5.1.2.3.2.24" not in siblings and len(siblings) >= 1)
    check("les 8 noeuds depth-1 (racine document '0' + 7 familles) sont dans le pool",
          sum(1 for p in dots if h.path_depth(p) == 1) == 8)

    # profondeurs derivees vs colonne depth (les 2 ecarts connus et nommes)
    mism = [(p, r.get("depth")) for p, r in bypath.items()
            if (r.get("depth") or "").strip() and h.path_depth(p) != int(r["depth"])]
    check("2 ecarts path-vs-colonne depth, dont la racine '0' et la rangee 824",
          sorted(mism) == [("0", "0"), ("5.1.2.3.2.24", "7")])

    print("=== 2) Controles synthetiques ===")
    check("path_parent('1') == '' (racine)", h.path_parent("1") == "")
    check("path_parent('0') == '' (racine document)", h.path_parent("0") == "")
    check("path_parent('1.2') == '1'", h.path_parent("1.2") == "1")
    check("path_parent('2.1.1.5.1.2.5.1') == '2.1.1.5.1.2.5'",
          h.path_parent("2.1.1.5.1.2.5.1") == "2.1.1.5.1.2.5")
    check("path_depth('1') == 1, path_depth('') == 0",
          h.path_depth("1") == 1 and h.path_depth("") == 0)
    check("dot2comma('1.2.3') == '1,23'", h.dot2comma("1.2.3") == "1,23")
    check("dot2comma('1') == '1' (pas de virgule aux racines)", h.dot2comma("1") == "1")
    check("comma2dot('1,23') == '1.2.3' (rangs a un chiffre : direction saine)",
          h.comma2dot("1,23") == "1.2.3")
    check("comma2dot('5,123224') == '5.1.2.3.2.2.4' (defaut reproduit)",
          h.comma2dot("5,123224") == "5.1.2.3.2.2.4")
    check("parent defectueux sur rang simple : '1,23' -> '1,2' == path_parent rendu coma",
          h.comma_parent_DEFECTUEUX("1,23") == h.dot2comma(h.path_parent("1.2.3")))

    n_fail = sum(1 for _, ok in CHECKS if not ok)
    print(f"\n=== {len(CHECKS) - n_fail}/{len(CHECKS)} controles verts ===")
    return 1 if n_fail else 0


if __name__ == "__main__":
    sys.exit(main())
