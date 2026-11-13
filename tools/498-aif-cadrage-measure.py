#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#498 — cadrage AIF/Walton : re-mesure LECTURE SEULE des figures du chantier.

    python tools/498-aif-cadrage-measure.py            # mesure sur le CSV vivant
    python tools/498-aif-cadrage-measure.py --self-test

Aucune ecriture. Le corpus n'est pas mute (chantier POST-TAG, gate owner nomme).

CORRECTION DU 06/11 (grain 1, pool c.6665295882 / #498 c.6665070534).
La version precedente reconstruisait les ancetres en decoupant `decimal_path`
aux VIRGULES, et sa propre docstring enseignait ce decoupage comme la methode
correcte. Mesure sur l'arbre vivant : `decimal_path` porte **une seule virgule**
(1 400 rangees sur 1 408 ; 8 n'en ont aucune), juste apres la famille — les
niveaux suivants sont COLLES ("2,1151251"). Le decoupage aux virgules ne
voyait donc d'ancetre que la tete de famille : aucun heritage depuis un noeud
intermediaire, et tous les chiffres d'heritage publies depuis le 21/09 sur
cette issue sont VOIDS (retraites par ai-01, c.6665070534).

La vraie hierarchie est la colonne `path` ("2.1.1.5.1.2.5.1"), separee par des
POINTS, un segment par niveau : 1 408 chemins uniques, aucun orphelin. Les
ancetres se lisent ici, et seulement ici.

Les jumeaux `docs/taxonomy/141-aif-*.py` ne partagent PAS ce defaut : leur
`comma2dot` eclate le reste colle CHIFFRE PAR CHIFFRE, ce qui restitue `path`
— sauf pour un rang a deux chiffres ("24"), ou l'eclatement corrompt le
chemin (rang mesure sur 1 rangee : `5,123224`). La section 4 rend ce compte.

DEFINITIONS DES TROIS ELEMENTS DE LA MECANIQUE (chacun herite du plus proche
ancetre porteur ; c'est la precision de l'owner du 05/11 : une mecanique se
definit au niveau d'un noeud intermediaire, pas a chaque feuille) :

  (a) le schema legitime imite   : `AIF_skosExceptionRef` non vide, OU
                                   `AIF_skosDirectRef` dont une cible finit
                                   en `_Inference` / `_Scheme`, OU
                                   `AIF_imitatedScheme` non vide (prose
                                   « imite… », grains 12+ : scheme sans nom
                                   AIF — tranche de representation #2229).
  (b) l'element attaque          : `AIF_attackType` non vide.
  (c) la question sautee, nommee : une cible de `AIF_skosDirectRef` ou
                                   `AIF_skosExceptionRef` finissant en
                                   `_Conflict` (mesure PAR APPROXIMATION :
                                   AIF ne nomme qu'une vingtaine de conflits ;
                                   pour les autres mecanismes la question
                                   sautee s'ecrira en prose et restera
                                   invisible a cet instrument).

Pieges d'instrument propres a ce chantier :

  - `AIF_skosOther` est une colonne de PROSE (« mirroring anchor 833) »), pas
    une colonne de references. Elle est lue comme NOTE, jamais comme ref.
  - `AIF_skosMappingType` porte des predicats SKOS (`skos:broadMatch`…), PAS
    la nature Inference/Scheme/Conflict : celle-ci vit dans le SUFFIXE de la
    cible IRI. La confondre fabrique un (c) a zero.
  - Un prefixe de CHAINE nu sur `path` lirait "1.2" comme ancetre de "1.24" :
    les ancetres se decoupent aux POINTS, jamais par prefixe de chaine.
  - Les CSV de `docs/ontology/aif-export/` sont des INSTANTANES. La source
    vivante est le CSV de taxonomie ; l'export sert de recoupement, jamais de
    base.
"""
import collections
import importlib.util
import os
import re
import sys

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
FAL = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"

REF_COLS = ("AIF_skosDirectRef", "AIF_skosExceptionRef")
ATTACK_COLS = ("AIF_attackType", "AIF_attackedNode")
SCHEME_SUFFIXES = ("_Inference", "_Scheme")
CONFLICT_SUFFIX = "_Conflict"


# ---------------------------------------------------------------- helpers ---

def ancestors_path(p):
    """Ancetres d'un `path` pointe : segments separes par des POINTS.

    "2.1.1.5" -> ["2", "2.1", "2.1.1", "2.1.1.5"]. Un segment peut porter
    plusieurs chiffres ("2.1.24" a pour ancetre "2.1", jamais "2.1.2").
    """
    seg = p.split(".")
    return [".".join(seg[:k]) for k in range(1, len(seg) + 1)]


def ancestors_comma(dp):
    """ANCIENNE methode (DEFECTUEUSE, conservee pour le controle inverse).

    Decoupe `decimal_path` aux virgules : sur l'arbre reel, la colonne n'a
    qu'une virgule, donc ceci ne rend que la tete de famille comme ancetre.
    """
    seg = [x for x in dp.split(",") if x != ""]
    return [",".join(seg[:k]) for k in range(1, len(seg))]


def comma2dot(c):
    """Formule des jumeaux 141 : famille, puis chaque CHIFFRE du reste."""
    c = (c or "").strip()
    if "," not in c:
        return c
    fam, rest = c.split(",", 1)
    return ".".join([fam] + list(rest))


def targets(v):
    """Cibles d'une valeur de ref : coupe ; , | et :, sans fabriquer de vide."""
    return [p.split(":")[-1] if ":" in p else p
            for p in (s.strip() for s in re.split(r"[;,|]", v or "")) if p]


def load_rows(repo=REPO, fal=FAL):
    p = os.path.join(repo, "tools", "994-apostrophe-dryrun.py")
    spec = importlib.util.spec_from_file_location("apostrophe_instr", p)
    m = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(m)
    corp = m.load(repo, fal)
    header = [h.lstrip("﻿") for h in corp["header"]]
    cols = {h: n for n, h in enumerate(header)}
    if "carte" not in cols:
        sys.exit("COLONNE ABSENTE : 'carte' — instrument invalide")
    if "path" not in cols:
        sys.exit("COLONNE ABSENTE : 'path' — instrument invalide (hierarchie)")
    rows = [[m.unquote(x) for x in m.split_fields(r)] for r, _t in corp["rows"][1:]]
    return rows, cols


def measure(rows, cols):
    """Tout le calcul, pur : main() imprime, --self-test falsifie."""

    def g(f, name):
        n = cols.get(name)
        return (f[n] if n is not None and n < len(f) else "").strip()

    def has_suffix(f, col, sfx):
        return any(t.endswith(sfx) for t in targets(g(f, col)))

    def carriers(pred):
        return {g(f, "path") for f in rows if pred(f)}

    def covered_by(carrier_set, f):
        p = g(f, "path")
        return p in carrier_set or any(a in carrier_set for a in ancestors_path(p)[:-1])

    # (a) carriers — grain 12/13 : la colonne dédiée AIF_imitatedScheme (prose
    # « imite… », décision de représentation d'ai-01 sur #2229) compte comme
    # porteur, à côté de la référence skos ; g() rend "" si la colonne est
    # absente (bases pré-grain-12 → comportement inchangé). La re-dérivation
    # #2239 avait laissé tomber cette extension — les phrases (244 Cynisme, 2.2)
    # étaient comptées « sans (a) » alors que la tranche de représentation leur
    # donne une colonne précisément pour être des porteuses.
    a_set = carriers(lambda f: g(f, "AIF_skosExceptionRef")
                     or g(f, "AIF_imitatedScheme")
                     or (g(f, "AIF_skosDirectRef")
                         and any(has_suffix(f, "AIF_skosDirectRef", s)
                                 for s in SCHEME_SUFFIXES)))
    b_set = carriers(lambda f: g(f, "AIF_attackType"))
    c_set = carriers(lambda f: (g(f, "AIF_skosDirectRef") or g(f, "AIF_skosExceptionRef"))
                     and (has_suffix(f, "AIF_skosDirectRef", CONFLICT_SUFFIX)
                          or has_suffix(f, "AIF_skosExceptionRef", CONFLICT_SUFFIX)))

    cards = [f for f in rows if g(f, "carte")]
    cov = {lab: {g(f, "path") for f in rows if covered_by(s, f)}
           for lab, s in (("(a)", a_set), ("(b)", b_set), ("(c)", c_set))}
    cov["les trois"] = cov["(a)"] & cov["(b)"] & cov["(c)"]
    cov["au moins un"] = cov["(a)"] | cov["(b)"] | cov["(c)"]

    # groupes de niveau 2 ou (a) ou (c) manque quelque part
    label = {g(f, "path"): (g(f, "Sous-Famille") or g(f, "Famille") or "?") for f in rows}
    groups = {}
    for f in rows:
        seg = g(f, "path").split(".")
        if len(seg) < 2:
            continue
        grp = ".".join(seg[:2])
        d = groups.setdefault(grp, [0, 0, 0, 0])  # nœuds, sans(a), sans(c), sans(a)ou(c)
        d[0] += 1
        ma = g(f, "path") not in cov["(a)"]
        mc = g(f, "path") not in cov["(c)"]
        if ma:
            d[1] += 1
        if mc:
            d[2] += 1
        if ma or mc:
            d[3] += 1

    missing = {g(f, "path") for f in rows
               if g(f, "path") not in cov["(a)"] or g(f, "path") not in cov["(c)"]}
    outside = sorted(p for p in missing if p.count(".") == 0)

    twins = [(g(f, "decimal_path"), g(f, "path"), comma2dot(g(f, "decimal_path")))
             for f in rows if comma2dot(g(f, "decimal_path")) != g(f, "path")]

    raw = {
        "N": len(rows), "cards": len(cards),
        "dr": sum(1 for f in rows if g(f, "AIF_skosDirectRef")),
        "er": sum(1 for f in rows if g(f, "AIF_skosExceptionRef")),
        "mt": sum(1 for f in rows if g(f, "AIF_skosMappingType")),
        "attack": sum(1 for f in rows if any(g(f, c) for c in ATTACK_COLS)),
        "attack_type_only": sum(1 for f in rows if g(f, "AIF_attackType")),
    }
    raw["union_ref"] = len(carriers(lambda f: g(f, "AIF_skosDirectRef") or g(f, "AIF_skosExceptionRef")))
    raw["attack_only"] = raw["attack"] - raw["union_ref"]

    return {
        "raw": raw, "porteurs": {"(a)": len(a_set), "(b)": len(b_set), "(c)": len(c_set)},
        "cov": cov, "cov_cards": {lab: sum(1 for f in cards if g(f, "path") in s)
                                  for lab, s in cov.items()},
        "sets": {"(a)": a_set, "(b)": b_set, "(c)": c_set},
        "groups": groups, "group_label": label,
        "missing": len(missing), "outside": outside,
        "twins": twins,
        "g": g,
    }


# ------------------------------------------------------------------ main ---

def main():
    rows, cols = load_rows()
    r = measure(rows, cols)
    g, N, C = r["g"], r["raw"]["N"], r["raw"]["cards"]

    print("=== #498 — cadrage AIF/Walton (lecture seule, hierarchie `path`) ===")
    print(f"  base : {FAL}")
    print(f"  {N} noeuds · {C} cartes (le deck imprime)")
    print()

    print("=== 1) LES PORTEURS (bruts, sans heritage — inchanges par la correction) ===")
    print(f"  AIF_skosDirectRef      {r['raw']['dr']:>5} noeuds")
    print(f"  AIF_skosExceptionRef   {r['raw']['er']:>5} noeuds")
    print(f"  union des deux         {r['raw']['union_ref']:>5} noeuds  <- = AIF_skosMappingType ({r['raw']['mt']})")
    print(f"  AIF_attackType/node    {r['raw']['attack']:>5} noeuds (attackType seul : {r['raw']['attack_type_only']})")
    print(f"  porte ATTACK sans ref   {r['raw']['attack_only']:>4} noeuds")
    print(f"  porteurs par element : (a) {r['porteurs']['(a)']} · (b) {r['porteurs']['(b)']} · (c) {r['porteurs']['(c)']}")
    print()

    print("=== 2) LE TRIPLET — chaque element herite du plus proche ancetre porteur ===")
    print(f"  {'element':<52}{'porteurs':>9}{'noeuds':>9}{'cartes':>10}")
    order = (("(a)", "schema legitime imite (ExceptionRef | DirectRef _Inference/_Scheme | imitatedScheme prose)"),
             ("(b)", "element attaque (AIF_attackType)"),
             ("(c)", "question sautee nommee par un conflit AIF (_Conflict)"))
    for lab, desc in order:
        print(f"  {lab} {desc:<58}{r['porteurs'][lab]:>9}"
              f"{len(r['cov'][lab]):>6}/{N}{r['cov_cards'][lab]:>6}/{C}")
    print(f"  {'les trois (intersection)':<52}{'':>9}{len(r['cov']['les trois']):>6}/{N}"
          f"{r['cov_cards']['les trois']:>6}/{C}")
    print(f"  {'au moins un (union)':<52}{'':>9}{len(r['cov']['au moins un']):>6}/{N}"
          f"{r['cov_cards']['au moins un']:>6}/{C}")
    print()

    print("=== 3) LES GROUPES — ou (a) ou (c) manque (niveau juste sous la famille) ===")
    print(f"  {'groupe':<36}{'noeuds':>7}{'sans(a)':>8}{'sans(c)':>8}{'sans(a)ou(c)':>13}")
    tot = 0
    for grp in sorted((k for k, v in r["groups"].items() if v[3] > 0),
                      key=lambda x: [int(i) for i in x.split(".")]):
        n, ma, mc, mac = r["groups"][grp]
        name = r["group_label"].get(grp, "")
        print(f"  {grp} {name[:26]:<33}{n:>7}{ma:>8}{mc:>8}{mac:>13}")
        tot += mac
    print(f"  {'total dans les groupes':<36}{'':>7}{'':>8}{'':>8}{tot:>13}")
    print(f"  hors groupes : {len(r['outside'])} noeuds de tete ({', '.join(r['outside'])})")
    print(f"  -> {r['missing']} noeuds manquent (a) ou (c) ; le chantier = ces groupes, pas 900 descriptions")
    print()

    print("=== 4) LES JUMEAUX 141 (comma2dot vs path) ===")
    print(f"  comma2dot(decimal_path) == path : {N - len(r['twins'])}/{N} rangées")
    for dp, p, bad in r["twins"]:
        print(f"  MISMATCH decimal_path={dp!r} path={p!r} comma2dot={bad!r} (rang a deux chiffres)")
    print("  -> l'eclatement CHIFFRE PAR CHIFFRE des jumeaux echoue sur un rang >= 10 :")
    print("     le parent calcule de cette rangee n'existe pas dans l'arbre.")
    print()

    print("=== 5) CIBLES DISTINCTES (par suffixe) ===")
    for col in REF_COLS:
        t = set()
        for f in rows:
            for part in targets(g(f, col)):
                t.add(part)
        by = collections.Counter(x.rsplit("_", 1)[-1] if "_" in x else "(no_)" for x in t)
        print(f"  {col:<24}{len(t):>4} cibles  {dict(by.most_common(6))}")
    print()
    print("  ⛔ AIF_skosOther n'est PAS comptee : c'est une colonne de PROSE, pas de refs.")
    print("  ⚠️ (c) est une mesure par approximation : voir docstring — AIF ne nomme")
    print("     qu'une vingtaine de conflits ; la question sautee en prose est invisible ici.")
    return 0


# ------------------------------------------------------------- self-test ---

def self_test():
    """Controles inverses SYNTHETIQUES (aucune lecture du corpus, aucune ecriture)."""
    ok = [0]

    def check(cond, label):
        ok[0] += 1
        if not cond:
            sys.exit(f"SELF-TEST ROUGE : {label}")
        print(f"  ok  {label}")

    # 1) LE CONTROLE INVERSE DU DISPATCH : un enfant de porteur INTERMEDIAIRE
    #    doit etre compte couvert. Arbre synthetique ou l'ancien instrument echoue.
    header = ["PK", "path", "decimal_path", "carte", "Famille", "Sous-Famille",
              "AIF_skosDirectRef", "AIF_skosExceptionRef", "AIF_attackType",
              "AIF_attackedNode", "AIF_skosMappingType"]
    synth = [
        ["1",  "2",        "2",      "1", "F", "",  "", "", "", "", ""],          # tete de famille
        ["2",  "2.1",      "2,1",    "",  "F", "Groupe", "", "", "Trust", "", ""],# porteur INTERMEDIAIRE de (b)
        ["3",  "2.1.1",    "2,11",   "1", "F", "", "", "", "", "", ""],           # feuille : doit heriter
        ["4",  "3",        "3",      "1", "F2", "", "", "", "", "", ""],          # autre famille
    ]
    cols = {h: n for n, h in enumerate(header)}
    r = measure(synth, cols)
    leaf = "2.1.1"
    check(leaf in r["cov"]["(b)"],
          "l'enfant d'un porteur INTERMEDIAIRE est compte couvert (via `path`)")
    # la meme feuille vue par l'ANCIENNE methode (decoupage aux virgules) :
    # ancetres de "2,11" aux virgules = ["2"] seulement.
    check(ancestors_comma("2,11") == ["2"],
          "l'ancienne methode ne voit que la tete de famille (defaut reproduit)")
    check("2.1" not in {a for a in ancestors_comma("2,11")},
          "l'ancienne methode ne rend PAS le porteur intermediaire 2,1")

    # 2) SEGMENTS A PLUSIEURS CHIFFRES : "2.1.24" a pour ancetre "2.1", pas "2.1.2".
    check(ancestors_path("2.1.24") == ["2", "2.1", "2.1.24"],
          "un rang a deux chiffres n'est pas dechire (ancetre = 2.1)")
    check(comma2dot("5,123224") == "5.1.2.3.2.2.4" and "5.1.2.3.2.24".split(".")[-1] == "24",
          "l'eclatement chiffre-par-chiffre des jumeaux corrompt le rang 24 (defaut nomme)")

    # 3) MONOTONIE ET COHERENCE sur l'arbre synthetique.
    check(r["sets"]["(b)"] <= r["cov"]["(b)"],
          "chaque porteur est couvert par lui-meme (porteurs inclus dans couverts)")
    check(r["cov"]["les trois"] <= r["cov"]["(a)"] | r["cov"]["(b)"] | r["cov"]["(c)"]
          and r["cov"]["les trois"] <= r["cov"]["(b)"],
          "l'intersection est incluse dans chaque element")
    check("4" not in r["cov"]["(b)"] and "2" not in r["cov"]["(b)"],
          "une famille sans porteur n'est pas couverte (pas de faux positif)")

    # 4) CLASSIFICATION PAR SUFFIXE.
    check(targets("Example_Inference, InductiveSchemes_Scheme") ==
          ["Example_Inference", "InductiveSchemes_Scheme"],
          "les cibles se coupent proprement")
    check(targets("") == [] and targets("  ") == [],
          "une valeur vide ne fabrique pas de cible")
    check(any(t.endswith("_Conflict") for t in targets("GeneralAcceptanceDoubt_Conflict")),
          "un suffixe _Conflict est vu")
    check(not any(t.endswith("_Conflict") for t in targets("PopularOpinion_Inference")),
          "un _Inference n'est pas compte comme _Conflict")

    # 5) (a) : DirectRef ne compte que suffixe Inference/Scheme ; ExceptionRef toujours.
    synth2 = list(synth)
    synth2[3] = ["4", "3", "3", "1", "F2", "", "Ignorance_Inference", "", "", "", ""]
    r2 = measure(synth2, cols)
    check("3" in r2["cov"]["(a)"], "DirectRef _Inference compte pour (a)")
    synth3 = list(synth)
    synth3[3] = ["4", "3", "3", "1", "F2", "", "Logical_Conflict", "", "", "", ""]
    r3 = measure(synth3, cols)
    check("3" not in r3["cov"]["(a)"] and "3" in r3["cov"]["(c)"],
          "DirectRef _Conflict compte pour (c), pas pour (a)")

    # 6) (a) grains 12/13 : la colonne dédiée AIF_imitatedScheme (prose « imite… »)
    #    compte comme porteur ; une base SANS la colonne ne change pas de compte.
    #    (Contrôle inverse du défaut #2239 : sans cette extension, les phrases
    #    étaient comptées « sans (a) » — l'instrument contredisant la tranche de
    #    représentation qui leur donne une colonne dédiée.)
    header12 = header + ["AIF_imitatedScheme"]
    synth12 = [[*row, ""] for row in synth]
    synth12[3][header12.index("AIF_imitatedScheme")] = "imite la défiance méthodique"
    r12 = measure(synth12, {h: n for n, h in enumerate(header12)})
    check("3" in r12["cov"]["(a)"],
          "AIF_imitatedScheme porteur compte pour (a)")
    synth12b = [[*row, ""] for row in synth]  # colonne présente mais vide partout
    r12b = measure(synth12b, {h: n for n, h in enumerate(header12)})
    check("3" not in r12b["cov"]["(a)"],
          "colonne AIF_imitatedScheme vide ne couvre rien")
    check(measure(synth, cols)["sets"]["(a)"] == r12b["sets"]["(a)"],
          "base sans la colonne = même compte de porteurs (rétro-compatible)")

    print(f"SELF-TEST VERT : {ok[0]} controles")
    return 0


if __name__ == "__main__":
    sys.exit(self_test() if "--self-test" in sys.argv else main())
