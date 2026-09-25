#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#994 B+C — verificateur INDEPENDANT du balayage typographique (contre-instrument).

Ne partage AUCUNE logique avec tools/994-write-BC-sweep-byte-exact.py : il re-mesure
les marques par canal, cellule par cellule, et confronte trois sources :

  1. le PLAN        — ce que la worklist du writeur dit avoir consomme (JSON --json-out) ;
  2. la REALITE     — ce que portent les backups .BEFORE-994BC, comptage brut ;
  3. l'APRES        — ce que porte l'arbre ecrit : doit etre 0 sur les 4 canaux.

Canaux (colonnes RENDUES seulement, whitelist du dry-run #994 — donc link_* et les
colonnes sans lecteur sont hors champ par construction) :
  P : guillemet droit "            (toutes langues)
  A : apostrophe droite '          (hors EN : la norme EN la garde ; hors fr : volet A ferme)
  C : apostrophe courbe ’          (EN seulement — courbe interdite)
  Z : guillemet droit " en zh/ar   (inclus dans P, compte a part pour lisibilite)

PREUVE DE NON-AVEUGLEMENT : lance sur les seuls backups, l'instrument DOIT rendre des
comptes non nuls (--avant). Un instrument qui rend 0 partout ne prouve rien.

Usage :
    python tools/994-verifie-BC-independant.py            # plan == realite, et apres == 0
    python tools/994-verifie-BC-independant.py --avant     # sur les backups : doit etre non nul
    python tools/994-verifie-BC-independant.py --worklist <json>
"""

import argparse
import collections
import importlib.util
import json
import os
import sys

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DRY_PATH = os.path.join(REPO, "tools", "994-apostrophe-dryrun.py")
BACKUP_SUFFIX = ".BEFORE-994BC"
STRAIGHT = "'"
DQUOTE = '"'
CURVED = "’"

_spec = importlib.util.spec_from_file_location("dry994v", DRY_PATH)
dry = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(dry)


def cells(path_rel):
    """{(corpus, rowid, lang, col): valeur} — colonnes rendues uniquement."""
    text = open(os.path.join(REPO, path_rel), "rb").read().decode("utf-8-sig")
    rows = dry.split_logical_rows(text)
    header = [dry.unquote(f) for f in dry.split_fields(rows[0][0])]
    out = {}
    for key, rel, idcol in dry.CORPORA:
        if not path_rel.startswith(rel):
            continue
        for rowno, (row, _t) in enumerate(rows[1:], start=2):
            f = [dry.unquote(x) for x in dry.split_fields(row)]
            rid = (f[header.index(idcol)] if idcol in header and header.index(idcol) < len(f)
                   else "").strip() or f"L{rowno}"
            for (lang, col), idx in dry.branch_scope("C", key, header).items():
                if idx < len(f):
                    out[(key, rid, lang, col)] = f[idx]
    return out


def channel_counts(values):
    """{(corpus,rowid,lang,col): {P,A,C}} — comptage brut, sans predicat d'appariement."""
    res = {}
    for k, v in values.items():
        lang = k[2]
        res[k] = {
            "P": v.count(DQUOTE),
            "A": 0 if lang in ("en", "fr") else v.count(STRAIGHT),
            "C": v.count(CURVED) if lang == "en" else 0,
        }
    return res


def plan_counts(worklist_path):
    """{cellule: {P,A,C}} — ce que la worklist declare avoir consomme.

    Les 'paire-*' comptent des PAIRES (n x 2 marques) ; paire-reparee consomme UNE
    marque droite et en ecrit deux (ouvrante+fermante), donc 1 seule consommee.
    """
    d = json.load(open(worklist_path, encoding="utf-8"))
    res = {}
    for w in d["worklist"]:
        for c in w["cells"]:
            key = (w["corpus"], w["rowid"], c["lang"], c["col"])
            acc = res.setdefault(key, {"P": 0, "A": 0, "C": 0})
            for kind, n in c["changes"]:
                if kind == 'paire-"':
                    acc["P"] += n * 2
                elif kind == "paire-'":
                    acc["A"] += n * 2
                elif kind in ("elision->courbe", "reliquat->courbe"):
                    acc["A"] += n
                elif kind == "paire-reparee":
                    acc["A"] += 1
                elif kind == "en-courbe->droite":
                    acc["C"] += n
    return res


def totals(counts):
    t = collections.Counter()
    for v in counts.values():
        for ch in ("P", "A", "C"):
            t[ch] += v[ch]
    return t


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--worklist", default=None)
    ap.add_argument("--avant", action="store_true",
                    help="mesure les BACKUPS seuls : doit rendre des comptes NON NULS")
    args = ap.parse_args()

    if args.avant:
        t = collections.Counter()
        for key, rel, _id in dry.CORPORA:
            p = rel + BACKUP_SUFFIX
            if not os.path.exists(os.path.join(REPO, p)):
                continue
            tc = totals(channel_counts(cells(p)))
            print(f"  {key:9s} P={tc['P']:4d} A={tc['A']:4d} C={tc['C']:4d}")
            t.update(tc)
        print(f"  TOTAL AVANT  P={t['P']} A={t['A']} C={t['C']}")
        vides = [ch for ch in ("P", "A", "C") if t[ch] == 0]
        if vides:
            print(f"  AVERTISSEMENT : canal {vides} nul AVANT — l'instrument ne le voit pas")
        print("non-aveuglement : " + ("OK" if not vides else "NON PROUVE"))
        return 0

    # 1) APRES : l'arbre ecrit doit etre a 0 sur les 3 canaux
    apres = collections.Counter()
    for key, rel, _id in dry.CORPORA:
        tc = totals(channel_counts(cells(rel)))
        apres.update(tc)
        print(f"  apres {key:9s} P={tc['P']:4d} A={tc['A']:4d} C={tc['C']:4d}")
    print(f"  TOTAL APRES  P={apres['P']} A={apres['A']} C={apres['C']}")

    # 2) PLAN == REALITE (backups), cellule par cellule
    if args.worklist:
        plan = plan_counts(args.worklist)
        real = collections.Counter()
        for key, rel, _id in dry.CORPORA:
            p = rel + BACKUP_SUFFIX
            if not os.path.exists(os.path.join(REPO, p)):
                continue
            for k, v in channel_counts(cells(p)).items():
                for ch in ("P", "A", "C"):
                    real[(k, ch)] += v[ch]
        ecarts = []
        for k in sorted(set(plan) | {kk[0] for kk in real}):
            for ch in ("P", "A", "C"):
                pv = plan.get(k, {}).get(ch, 0)
                rv = real.get((k, ch), 0)
                if pv != rv:
                    ecarts.append((k, ch, pv, rv))
        print(f"  reconciliation plan<->backups : {len(ecarts)} ecart(s)")
        for k, ch, pv, rv in ecarts[:20]:
            print(f"    ECART {ch} {k}: plan={pv} reel={rv}")

    residuels = {ch: n for ch, n in apres.items() if n}
    if residuels:
        print(f"ECHEC : residuel apres : {residuels}")
        return 1
    print("OK : 0 residuel sur les 3 canaux (colonnes rendues, 5 CSV x 8 langues).")
    return 0


if __name__ == "__main__":
    sys.exit(main())
