#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#498 — cadrage AIF/Walton : re-mesure LECTURE SEULE des figures du chantier.

    python tools/498-aif-cadrage-measure.py

Aucune ecriture. Le corpus n'est pas mute (chantier POST-TAG, gate owner nomme).

POURQUOI CET INSTRUMENT, ET PAS UN COMPTE DE PLUS.
Trois chiffres de « couverture » circulent sur le meme corpus, et ils ne mesurent PAS
le meme objet :

  - **116**  les PORTEURS de ref (>= 1 `DirectRef`/`ExceptionRef`) — la definition du
             recensement a 3 etats du 14/09, adossee a un test (`FallacySkosCensusTests`).
  - **442**  la « couverture effective par heritage » du corps de #498 (juin) — un noeud
             compte s'il descend d'une grappe mappee.
  - **488 / 505** la meme notion d'heritage, RE-MESUREE ici, sous DEUX definitions
             explicites : refs seules (488) ou refs-ou-attack (505).

Ces nombres sont tous vrais. Aucun n'est interchangeable. Un dispatch qui dit « la
couverture est de 31 % » sans nommer sa definition rend le chiffre inverifiable : c'est la
famille de defauts « le compte suit la definition publiee ».

Pieges d'instrument propres a ce chantier :

  - `AIF_skosOther` est une colonne de PROSE (« mirroring anchor 833) », « swap deferred to
    owner GO »), pas une colonne de references. La compter comme des cibles fabrique des
    « concepts » qui sont des phrases. Elle est lue ici comme NOTE, jamais comme ref.
  - `decimal_path` est en decimal POINTE par VIRGULES ("1,1,2") : l'ancetre de "1,1,2" est
    "1,1" puis "1". Un prefixe de CHAINE nu lirait "1,10" comme enfant de "1,1".
  - Les CSV de `docs/ontology/aif-export/` sont des INSTANTANES. La source vivante est le
    CSV de taxonomie ; l'export sert de recoupement, jamais de base.
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


def instrument():
    p = os.path.join(REPO, "tools", "994-apostrophe-dryrun.py")
    spec = importlib.util.spec_from_file_location("apostrophe_instr", p)
    m = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(m)
    return m


def main():
    i = instrument()
    corp = i.load(REPO, FAL)
    cols = {h.lstrip("﻿"): n for n, h in enumerate(corp["header"])}
    if "carte" not in cols:
        sys.exit("COLONNE ABSENTE : 'carte' — instrument invalide")
    rows = [[i.unquote(x) for x in i.split_fields(r)] for r, _t in corp["rows"][1:]]

    def g(f, name):
        n = cols.get(name)
        return (f[n] if n is not None and n < len(f) else "").strip()

    def anc(dp):
        """Ancetres d'un decimal_path : segments separes par des VIRGULES."""
        p = [x for x in dp.split(",") if x != ""]
        return [",".join(p[:k]) for k in range(1, len(p))]

    def carriers(cols_):
        return {g(f, "decimal_path") for f in rows if any(g(f, c) for c in cols_)}

    def covered(s, f):
        dp = g(f, "decimal_path")
        return dp in s or any(a in s for a in anc(dp))

    cards = [f for f in rows if g(f, "carte")]
    N = len(rows)

    print("=== #498 — cadrage AIF/Walton (lecture seule) ===")
    print(f"  base : {FAL}")
    print(f"  {N} noeuds · {len(cards)} cartes (le deck imprime)")
    print()

    print("=== 1) LES PORTEURS (definition du recensement 14/09) ===")
    dr = [f for f in rows if g(f, "AIF_skosDirectRef")]
    er = [f for f in rows if g(f, "AIF_skosExceptionRef")]
    mt = [f for f in rows if g(f, "AIF_skosMappingType")]
    at = [f for f in rows if any(g(f, c) for c in ATTACK_COLS)]
    union_ref = {g(f, "decimal_path") for f in dr} | {g(f, "decimal_path") for f in er}
    print(f"  AIF_skosDirectRef      {len(dr):>5} noeuds")
    print(f"  AIF_skosExceptionRef   {len(er):>5} noeuds")
    print(f"  union des deux         {len(union_ref):>5} noeuds  <- = AIF_skosMappingType ({len(mt)})")
    print(f"  AIF_attackType/node    {len(at):>5} noeuds")
    print(f"  porte ATTACK sans ref   {len({g(f,'decimal_path') for f in at} - union_ref):>4} noeuds")
    print()

    print("=== 2) LA COUVERTURE PAR HERITAGE — elle depend de la DEFINITION ===")
    print(f"  {'definition':<34}{'noeuds':>8}{'%':>7}{'cartes':>9}")
    defs = [("refs seules (DirectRef|ExceptionRef)", REF_COLS),
            ("refs ou attack", REF_COLS + ATTACK_COLS)]
    for lab, cc in defs:
        s = carriers(cc)
        n = sum(1 for f in rows if covered(s, f))
        c = sum(1 for f in cards if covered(s, f))
        print(f"  {lab:<34}{n:>8}{n/N*100:>6.1f}%{c:>6}/{len(cards)}")
    print(f"  {'porteurs seuls (= recensement 14/09)':<34}{len(union_ref):>8}"
          f"{len(union_ref)/N*100:>6.1f}%"
          f"{sum(1 for f in cards if g(f,'decimal_path') in union_ref):>6}/{len(cards)}")
    print(f"  le corps de #498 publie 442/1408 = 31.4 % — aucune de ces definitions ne le rend")
    print()

    print("=== 3) ASYMETRIE PAR FAMILLE (refs ou attack, heritage inclus) ===")
    s = carriers(REF_COLS + ATTACK_COLS)
    by, tot = collections.Counter(), collections.Counter()
    for f in rows:
        fam = g(f, "Famille") or "(sans)"
        tot[fam] += 1
        if covered(s, f):
            by[fam] += 1
    print(f"  {'famille':<30}{'couverts':>9}{'noeuds':>8}{'%':>7}")
    for fam, n in tot.most_common():
        print(f"  {fam[:29]:<30}{by.get(fam, 0):>9}{n:>8}{by.get(fam, 0)/n*100:>7.1f}")
    empty = sum(n for fam, n in tot.items() if by.get(fam, 0) / n < 0.15)
    print(f"  -> {empty} noeuds ({empty/N*100:.0f} % de la taxonomie) sont dans des familles"
          f" couvertes a moins de 15 %")
    print()

    print("=== 4) LES DEUX PERIMETRES ===")
    cov_cards = [f for f in cards if covered(s, f)]
    exc = {g(f, "decimal_path") for f in rows if g(f, "AIF_skosExceptionRef")}
    no_exc = [f for f in cards
              if not (g(f, "decimal_path") in exc
                      or any(a in exc for a in anc(g(f, "decimal_path"))))]
    print(f"  taxonomie : {sum(1 for f in rows if covered(s, f))}/{N} noeuds couverts")
    print(f"  deck      : {len(cov_cards)}/{len(cards)} cartes couvertes "
          f"({len(cov_cards)/len(cards)*100:.1f} %)")
    print(f"  deck sans STRUCTURE D'EXCEPTION (ni directe ni heritee) : "
          f"{len(no_exc)}/{len(cards)} = {len(no_exc)/len(cards)*100:.1f} %")
    print(f"  -> c'est la WORKLIST du chantier generatif, POST-TAG et gatee")
    print(f"  pk (20 premiers) : {', '.join(g(f,'PK') for f in no_exc[:20])}")
    print()

    print("=== 5) CIBLES DISTINCTES (par suffixe) ===")
    for col in REF_COLS:
        t = set()
        for f in rows:
            v = g(f, col)
            for part in re.split(r"[;,|]", v):
                part = part.strip()
                if part:
                    t.add(part.split(":")[-1] if ":" in part else part)
        by = collections.Counter(x.rsplit("_", 1)[-1] for x in t)
        print(f"  {col:<24}{len(t):>4} cibles  {dict(by.most_common(6))}")
    print()
    print("  ⛔ AIF_skosOther n'est PAS comptee : c'est une colonne de PROSE, pas de refs.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
