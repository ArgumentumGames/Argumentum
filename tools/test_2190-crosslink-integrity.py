#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Test du contrat de tools/2190-crosslink-integrity.py — garde des liens transverses #2190.

CONDITION D'EXECUTION (cf. en-tete de .github/workflows/python-tools-tests.yml)
  std-lib, offline, AUCUN artefact : le test lit le CSV de taxonomie et les cartes
  committées (`Cards/Fallacies/Mindmaps/<lang>/`) — présents dans tout checkout —
  et un dépôt synthétique construit en répertoire temporaire. Ni bundle, ni
  Target/, ni réseau, ni appel payé. Durée mesurée : ~7 s (check 8 langues 2 s,
  self-test mutations 4,5 s, synthétique < 0,1 s).

POURQUOI CE FICHIER EXISTE. La garde #2190 protège les annotations des traits
transverses (`data-crosslink-*`, `pk`/`path`) contre la prochaine re-dérivation.
Un test de garde qui se contente de la lancer sur l'arbre ne prouve qu'une chose :
qu'elle est verte aujourd'hui. Ce contrat éprouve les DEUX sens :

  1. SYNTHÉTIQUE (cas 1-4) — un dépôt minimal d'UNE rangée et DEUX nœuds où chaque
     compte est non-nul PAR CONSTRUCTION : la garde doit y être verte (cas 1 — un
     vert sur un arbre vide ne serait pas un vert), puis rouge sur chaque défaut
     injecté : extrémité déplacée au-delà de TOL (cas 2, C4), triplet retiré du
     SVG (cas 3, C1), _ext retargeté (cas 4, C3).
  2. RÉEL (cas 5-6) — la garde sur les 8 langues du checkout (cas 5) et les huit
     témoins de mutation du module (cas 6, m1-m8) : chaque couche (C1-C4) doit
     rougir sur sa mutation, m2 doit laisser C1 VERT — la permutation entre
     deux traits de même verbe est le trou que la couche géométrique ferme —
     et m7/m8 doivent rougir l'embarqué SANS toucher l'autonome.

Sortie : exit 0 si tout passe, exit 1 avec la liste des cas rouges sinon.
"""

import importlib.util
import sys
import tempfile
from pathlib import Path

REPO = Path(__file__).resolve().parents[1]
TOOL = REPO / "tools" / "2190-crosslink-integrity.py"

spec = importlib.util.spec_from_file_location("p2190", TOOL)
p2190 = importlib.util.module_from_spec(spec)
spec.loader.exec_module(p2190)

failures = []


def case(name, ok, detail=""):
    if ok:
        print(f"  {name}: OK")
    else:
        failures.append(f"{name}: {detail or 'rouge'}")
        print(f"  {name}: ROUGE — {detail}")


# ---------------------------------------------------------------- dépôt synthétique

SYN_CSV = (
    "PK,path,Famille,crossLink_IsRelatedTo\r\n"
    '1280,7,Test,1.1\r\n'
    '1281,1.1,Test,\r\n'
)
# Deux nœuds éloignés (≈ 2000 unités), un trait de l'un vers l'autre. L'ancre
# texte du nœud = translate + (x, y) du <text> ; l'extrémité du trait tombe
# exactement dessus — la distance mesurée est 0, très en deçà de TOL.
SYN_SVG = """<?xml version="1.0" encoding="UTF-8"?>
<svg xmlns="http://www.w3.org/2000/svg">
<g id="1280" class="node" transform="translate(1000,2000)" pk="1280" path="7">
  <text x="56" y="63">Noeud A</text>
</g>
<g id="1281" class="node" transform="translate(3000,2000)" pk="1281" path="1.1">
  <text x="56" y="63">Noeud B</text>
</g>
<g class="crosslink" transform="translate(0,0)"
   data-crosslink-verb="IsRelatedTo" data-crosslink-source="7" data-crosslink-target="1.1">
  <path fill="none" d="M 1056 2063 C 2000 2063 2956 2063 3056 2063"/>
</g>
</svg>
"""
SYN_HTML = (
    "<!DOCTYPE html><html><body><header>"
    "<svg xmlns='http://www.w3.org/2000/svg' id='icon'><rect width='4' height='4'/></svg>"
    "</header><main>"
    + SYN_SVG.replace('<?xml version="1.0" encoding="UTF-8"?>\n', "")
    + "</main></body></html>"
)
SYN_EXT = ('<!DOCTYPE html><html><body>'
           '<object data="Fallacies_fr.content.svg" type="image/svg+xml"></object>'
           '</body></html>')


def build_syn_repo(tmp: Path, svg=SYN_SVG, ext=SYN_EXT, csv=SYN_CSV):
    mm = tmp / "Cards/Fallacies/Mindmaps/fr"
    mm.mkdir(parents=True)
    (tmp / "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv").write_text(csv, encoding="utf-8", newline="")
    (mm / "Fallacies_fr.content.svg").write_text(svg, encoding="utf-8", newline="")
    (mm / "Fallacies_fr.html").write_text(SYN_HTML if svg is SYN_SVG
                                          else SYN_HTML.replace(SYN_SVG.replace('<?xml version="1.0" encoding="UTF-8"?>\n', ""), svg),
                                          encoding="utf-8", newline="")
    (mm / "Fallacies_fr_ext.html").write_text(ext, encoding="utf-8", newline="")


def syn_errors(**kw) -> list:
    with tempfile.TemporaryDirectory(prefix="t2190-") as td:
        build_syn_repo(Path(td), **kw)
        errors, _ = p2190.check_repo(Path(td), langs=["fr"])
        return errors


print("test_2190-crosslink-integrity.py — contrat de la garde des liens transverses")

# Cas 1 — synthétique intact : VERT, et le rapport doit compter 1 trait / 2 nœuds
# (un vert à compte nul ne serait pas une mesure — l'instrument devait voir un 1).
with tempfile.TemporaryDirectory(prefix="t2190-") as td:
    build_syn_repo(Path(td))
    errors, report = p2190.check_repo(Path(td), langs=["fr"])
    case("1 synthétique intact vert", not errors, "; ".join(errors[:2]))
    case("1b comptes non nuls (1 trait, 2 nœuds)",
         any("traits=1 nœuds=2" in line for line in report),
         f"rapport: {report}")

# Cas 2 — extrémité déplacée au-delà de TOL : C4 rouge (la géométrie voit).
bad_end = SYN_SVG.replace("3056 2063", "9056 2063")
case("2 extrémité > TOL -> C4", any("C4" in e for e in syn_errors(svg=bad_end)),
     "C4 n'a pas vu l'extrémité déplacée")

# Cas 3 — triplet retiré du SVG : C1 rouge (le corpus et le SVG divergent).
case("3 triplet retiré -> C1",
     any("C1" in e for e in syn_errors(svg=SYN_SVG.replace(
         'data-crosslink-verb="IsRelatedTo" data-crosslink-source="7" data-crosslink-target="1.1"', ""))),
     "C1 n'a pas vu le triplet retiré")

# Cas 4 — _ext retargeté : C3 rouge.
case("4 _ext retargeté -> C3",
     any("C3" in e for e in syn_errors(ext=SYN_EXT.replace(
         'data="Fallacies_fr.content.svg"', 'data="Fallacies_fr.links.svg"'))),
     "C3 n'a pas vu le object data retargeté")

# Cas 4b — _ext pointe un mauvais RÉPERTOIRE, basename intact : C3 rouge.
case("4b _ext mauvais répertoire -> C3",
     any("C3" in e for e in syn_errors(ext=SYN_EXT.replace(
         'data="Fallacies_fr.content.svg"', 'data="missing/Fallacies_fr.content.svg"'))),
     "C3 s'est contenté du basename")

# Cas 4c — couple path/pk inconnu ajouté au SEUL SVG autonome : C2 rouge aussi sur
# la branche « extra » (l'ancienne soustraction avec elle-même était toujours vide).
case("4c couple inconnu -> C2",
     any("INCONNU" in e for e in syn_errors(svg=SYN_SVG.replace(
         '<g id="1280" class="node"',
         '<g id="9999" class="node" transform="translate(0,0)" pk="99999" path="99"><text x="0" y="0">Extra</text></g>\n'
         '<g id="1280" class="node"'))),
     "C2 n'a pas nommé le couple inconnu")

# Cas 5 — la garde réelle sur les 8 langues du checkout (cas nominal en CI :
# les cartes sont committées, aucun artefact externe).
errors, report = p2190.check_repo(REPO)
case("5 arbre réel 8 langues vert", not errors, "; ".join(errors[:2]))

# Cas 6 — les huit témoins de mutation du module : chaque couche voit sa
# mutation, m2 laisse C1 vert (le trou que C4 ferme), m7/m8 isolent l'embarqué.
witness_failures = p2190.selftest(REPO)
case("6 self-test mutations m1-m8", not witness_failures,
     "; ".join(witness_failures[:2]))

print()
if failures:
    print(f"CONTRAT ROUGE ({len(failures)} cas) :")
    for f in failures:
        print("  -", f)
    sys.exit(1)
print("CONTRAT VERT : garde éprouvée dans les deux sens (synthétique + réel + mutations).")
sys.exit(0)
