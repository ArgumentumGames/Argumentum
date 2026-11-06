#!/usr/bin/env python3
"""#498 grain 1b — les jumeaux 141-aif-* lisent la hierarchie dans `path`.

Historique : les jumeaux reconstruisaient parent et freres depuis `decimal_path`
(une seule virgule, niveaux colles chiffre par chiffre). La mesure (PR #2167) a
montre que cette forme s'ecarte de `path` sur la rangee `5,123224` (rang 24) :
l'eclatement chiffre par chiffre fabriquait `5.1.2.3.2.2.4`, un parent qui
n'existe pas dans l'arbre.

Regle desormais : la hierarchie se lit dans `path` (pointe, un segment par
niveau, 1 408 uniques, 0 orphelin, profondeur max 10). La forme coma reste la
CLE de rendu des sorties (prompts, sidecars) : elle se DEDUIT de `path` par
`dot2comma`, jamais l'inverse. Le `parent_of` historique est conserve sous le
nom `comma_parent_DEFECTUEUX` pour le controle inverse.
"""


def path_parent(p):
    """Parent d'un `path` pointe : on retire le dernier segment.

    '5.1.2.3.2.24' -> '5.1.2.3.2' ; '1.2' -> '1' ; '1' -> '' ; '0' -> ''.
    """
    p = (p or "").strip()
    return p.rsplit(".", 1)[0] if "." in p else ""


def path_depth(p):
    """Profondeur = nombre de segments. ⚠️ La colonne CSV `depth` ne doit PAS
    servir de filtre : elle vaut 0 sur la racine '0' et 7 sur '5.1.2.3.2.24'
    (6 segments), mesure 06/11."""
    p = (p or "").strip()
    return len(p.split(".")) if p else 0


def dot2comma(p):
    """Rendu coma depuis `path` : famille + virgule + niveaux colles.

    Identique a la forme historique sauf aux rangs >= 10, qui restent des
    entiers : '5.1.2.3.2.24' -> '5,123224' (non ambigu, la colonne d'origine
    porte exactement cette valeur).
    """
    p = (p or "").strip()
    if not p:
        return ""
    seg = p.split(".")
    return seg[0] if len(seg) == 1 else seg[0] + "," + "".join(seg[1:])


def comma2dot(c):
    """DIRECTION DEFECTUEUSE aux rangs >= 10 — conservee pour le controle
    inverse : '5,123224' -> '5.1.2.3.2.2.4' (parent inexistant, mesure)."""
    c = (c or "").strip()
    if "," not in c:
        return c
    fam, rest = c.split(",", 1)
    return ".".join([fam] + list(rest))


def comma_parent_DEFECTUEUX(dp):
    """L'ancien parent_of : on retire le dernier CHIFFRE de la queue. Garde tel
    quel pour le controle inverse (cf. 141-hierarchy-self-test.py)."""
    dp = (dp or "").strip()
    if "," not in dp:
        return ""
    fam, rest = dp.split(",", 1)
    if len(rest) <= 1:
        return fam
    return fam + "," + rest[:-1]


def load_taxonomy(csv_path):
    """Charge le CSV une fois et construit les trois vues coherentes.

    Retourne (rows, bypath, bycomma, bydot) :
      - bypath  : `path`  -> rang   (hierarchie, parents, profondeur)
      - bycomma : forme coma -> rang (rendu, cles de sidecar)
      - bydot   : `path`  -> rang   (cibles pointees des prompts ; bydot == bypath,
                                   le doublon documente l'ancien contrat)
    """
    import csv
    with open(csv_path, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.DictReader(f))
    bypath, bycomma, bydot = {}, {}, {}
    for r in rows:
        p = (r.get("path") or "").strip()
        if not p:
            continue
        bypath[p] = r
        bydot[p] = r
        bycomma[dot2comma(p)] = r
    return rows, bypath, bycomma, bydot


def build_pool(node, rows, bypath):
    """Voisinage d'un noeud : freres + parent + enfants + racines (depth == 1).

    Meme contrat qu'avant, hierarchie lue dans `path`. Le pool expose la forme
    POINTEE (`dotted` = `path`) pour le prompt, et `dot2comma` au rendu coma.
    """
    dp = (node.get("path") or "").strip()
    pool, seen = [], set()

    def add(p):
        p = (p or "").strip()
        if not p or p in seen:
            return
        r = bypath.get(p)
        if not r:
            return
        seen.add(p)
        pool.append({"dotted": p,
                     "label": (r.get("text_fr") or r.get("text_en") or "").strip(),
                     "desc": (r.get("desc_fr") or r.get("desc_en") or "").strip()[:140]})

    par = path_parent(dp)
    for s in rows:
        sp = (s.get("path") or "").strip()
        if path_parent(sp) == par and sp != dp:
            add(sp)                     # freres
    if par:
        add(par)                        # parent
    for s in rows:
        sp = (s.get("path") or "").strip()
        if path_parent(sp) == dp:
            add(sp)                     # enfants
    for s in rows:
        sp = (s.get("path") or "").strip()
        if path_depth(sp) == 1:
            add(sp)                     # racines de familles
    return pool[:30]
