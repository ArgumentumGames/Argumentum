#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1499 G6-C — pertes dans le fr Scenarii substitue, couche pre-agentique nommee. 0 ecriture.

Commande ai-01 (pool #458 v13) : reduire les 345 MODIFIE fr (G6, #1556) a une
liste courte de pertes ; couche pre-agentique Scenarii derivee de l'historique
git via la cle E1+E2 (jamais `path` seul) ; deltas 2022->pre-agentique en
section SEPAREE ; 25 cellules paire_limite marquees, 8 non resolues hors
perimetre ; 0 ecriture.

La couche, derivee et declaree :
  B_SC = b08b62fa (2023-10-27) — DERNIER commit du CSV Scenarii avant l'ere
  agentique ; le fichier dort d'octobre 2023 a mai 2026 (mesure : git log).
  L'ancre Fallacies 62b561e75 (2025-07-27) voit le meme etat : rien n'a bouge
  entre les deux. Tout commit posterieur sur ce CSV est l'ere agentique
  (premier : 2026-05-23).
  C = origin/master.

Deux jointures, chacune controlee par le plancher de bruit q99 exhaustif de G5 :
  (1) B_SC -> C      : la couche agentique, 8 champs fr x paires E1+E2
  (2) archive2022 -> B_SC : les gestes d'epoque (auteurs), section separee

Natures par cellule : PERDU (B rempli -> C vide), GAGNE, MODIFIE (les deux
remplis, texte different). Similarite token casefold+NFKD (calibration G6) et
rapport de longueur comme aides de lecture pour prioriser la lecture.

Usage :
    python tools/1499-g6c-pertes-scenarii.py                 # mesure + resume
    python tools/1499-g6c-pertes-scenarii.py --json-out tools/1499-g6c-pertes.json
    python tools/1499-g6c-pertes-scenarii.py --self-test
"""

import argparse
import collections
import csv
import importlib.util
import io
import json
import os
import re
import subprocess
import sys
import unicodedata

HERE = os.path.dirname(os.path.abspath(__file__))
REPO = os.path.dirname(HERE)
G5_PATH = os.path.join(HERE, "1499-g5-cle-scenarii.py")

CSV_REL = "Cards/Scenarii/Argumentum Scenarii - Cards.csv"
ARCHIVE_REL = os.path.join("Cards", "Scenarii", "Archive", "2022",
                           "Argumentum Scenarii - Cards fevrier 2022.csv")
B_SC = "b08b62fa"
C_REF = "origin/master"

FR_FIELDS = ["titre", "baratineur", "piocheur", "contexte", "enjeu", "suggestion",
             "catégorie", "sous-catégorie"]


def load_g5():
    spec = importlib.util.spec_from_file_location("g5", G5_PATH)
    m = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(m)
    return m


def git_rows(commit, rel):
    out = subprocess.run(["git", "show", f"{commit}:{rel}"], capture_output=True,
                         check=True, cwd=REPO).stdout
    return list(csv.DictReader(io.StringIO(out.decode("utf-8-sig"))))


def file_rows(path):
    with open(path, "rb") as fh:
        return list(csv.DictReader(io.StringIO(fh.read().decode("utf-8-sig"))))


def join(g5, left, right):
    """E1 + E2 de G5, sur deux listes de rangees quelconques, plancher calibre
    sur TOUTES les paires left x right. Une paire E1 SOUS le plancher n'est pas
    appariee (le controle la revoque) : titre commun, contenu sans rapport —
    reecriture totale ou collision, la cle seule ne decide pas."""
    threshold, n_pairs = g5.noise_floor(left, right)
    e1, e1_below = g5.judge(g5.key_title, left, right, threshold)
    recovered, unresolved = g5.stage2_recovery(left, right, threshold)
    below_ids = {id(x[0]) for x in e1_below}
    pairs = [(a, c) for a, c, _s in e1 if id(a) not in below_ids] \
        + [(a, c) for a, c, _s, _l in recovered]
    limites = {a.get("path", "") for a, _c, _s, lbl in recovered if lbl == "LIMITE"}
    return pairs, limites, threshold, len(unresolved), e1_below


def compare_cell(b, c):
    a_, c_ = (b or "").strip(), (c or "").strip()
    if a_ == c_:
        return None
    if a_ and not c_:
        return "PERDU"
    if not a_ and c_:
        return "GAGNE"
    return "MODIFIE"


def toks(text):
    t = unicodedata.normalize("NFKC", (text or "").casefold())
    return {w for w in re.sub(r"[^\w\s]", " ", t).split() if w}


def token_jaccard(a, c):
    ta, tc = toks(a), toks(c)
    if not ta and not tc:
        return 1.0
    if not ta or not tc:
        return 0.0
    return len(ta & tc) / len(ta | tc)


def diff_fr(pairs, limite_paths, src_label):
    cells = []
    by_nature = collections.Counter()
    by_field = collections.Counter()
    for b_row, c_row in pairs:
        for field in FR_FIELDS:
            nature = compare_cell(b_row.get(field, ""), c_row.get(field, ""))
            if not nature:
                continue
            b_val = (b_row.get(field, "") or "")
            c_val = (c_row.get(field, "") or "")
            cells.append({
                "path_B": b_row.get("path", ""),
                "titre_B": (b_row.get("titre", "") or "")[:60],
                "field": field, "nature": nature, "src": src_label,
                "paire_limite": b_row.get("path", "") in limite_paths,
                "sim_tokens": round(token_jaccard(b_val, c_val), 4) if nature == "MODIFIE" else None,
                "len_ratio": round(len(c_val.strip()) / max(1, len(b_val.strip())), 3)
                if nature == "MODIFIE" else None,
                "old": b_val, "new": c_val,
            })
            by_nature[nature] += 1
            by_field[(field, nature)] += 1
    return cells, by_nature, by_field


def self_test():
    a = {"titre": "X", "contexte": "gardé", "enjeu": "", "suggestion": "vieux"}
    c = {"titre": "", "contexte": "gardé", "enjeu": "nouveau", "suggestion": "autre"}
    got = [compare_cell(a.get(f, ""), c.get(f, "")) for f in ("titre", "contexte", "enjeu", "suggestion")]
    assert got == ["PERDU", None, "GAGNE", "MODIFIE"], got
    assert compare_cell(" text ", "text") is None
    assert abs(token_jaccard("La mère de César !", "la mère de césar,") - 1.0) < 1e-9
    assert token_jaccard("alpha", "omega") == 0.0
    print("SELF-TEST OK : natures, trim, similarite")
    return 0


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--json-out")
    ap.add_argument("--self-test", action="store_true")
    args = ap.parse_args()
    if args.self_test:
        return self_test()

    g5 = load_g5()
    archive = file_rows(os.path.join(REPO, ARCHIVE_REL))
    b_rows = git_rows(B_SC, CSV_REL)
    c_rows = git_rows(C_REF, CSV_REL)

    print(f"B_SC = {B_SC} ({len(b_rows)} rangees) -> C = {C_REF} ({len(c_rows)} rangees)")
    pairs_bc, limites_bc, thr_bc, unres_bc, below_bc = join(g5, b_rows, c_rows)
    print(f"jointure B_SC->C : {len(pairs_bc)} paires (E1+E2), plancher q99={thr_bc:.3f}, "
          f"E2 LIMITES={len(limites_bc)}, non resolues={unres_bc}, E1 sous plancher={len(below_bc)}")

    cells_bc, by_bc, byf_bc = diff_fr(pairs_bc, limites_bc, "B_SC->C")
    print(f"fr : {dict(by_bc)}")

    pairs_ab, limites_ab, thr_ab, unres_ab, below_ab = join(g5, archive, b_rows)
    print(f"\nsection separee — archive2022 ({len(archive)}) -> B_SC : {len(pairs_ab)} paires, "
          f"plancher q99={thr_ab:.3f}, non resolues={unres_ab}, "
          f"E1 sous plancher={len(below_ab)} (revoquees)")
    for a, c, s in below_ab:
        print(f"    REVOQUEE sim={s:.3f} {a.get('path', ''):<7} {a['titre'][:36]!r}")
    cells_ab, by_ab, byf_ab = diff_fr(pairs_ab, limites_ab, "2022->B_SC")
    cells_rej, by_rej, _ = diff_fr([(a, c) for a, c, _s in below_ab], set(), "2022->B_SC revoquees")
    print(f"cellules fr des {len(below_ab)} paires REVOQUEES (plancher dégénéré) : {dict(by_rej)}")
    print(f"fr : {dict(by_ab)}")

    mod = [c for c in cells_bc if c["nature"] == "MODIFIE"]
    mod.sort(key=lambda c: (c["sim_tokens"], c["len_ratio"] if c["len_ratio"] is not None else 1))
    buckets = {"[0,.2)": 0, "[.2,.4)": 0, "[.4,.6)": 0, "[.6,.8)": 0, "[.8,1]": 0}
    for c in mod:
        s = c["sim_tokens"]
        k = "[0,.2)" if s < .2 else "[.2,.4)" if s < .4 else "[.4,.6)" if s < .6 else "[.6,.8)" if s < .8 else "[.8,1]"
        buckets[k] += 1
    print("\nJaccard MODIFIE B_SC->C : " + " ".join(f"{k}={v}" for k, v in buckets.items()))
    print("par champ (MODIFIE) : " + ", ".join(f"{f}={n}" for (f, nat), n in sorted(byf_bc.items())
                                               if nat == "MODIFIE"))
    per = [c for c in cells_bc if c["nature"] == "PERDU"]
    print(f"PERDU nommes : " + ", ".join(f"{c['path_B']}:{c['field']}" for c in per) or "aucun")

    if args.json_out:
        json.dump({
            "couche": {"B_SC": B_SC, "C": C_REF},
            "jointure_bc": {"paires": len(pairs_bc), "plancher": round(thr_bc, 4),
                            "e2_limites": sorted(limites_bc), "non_resolues": unres_bc,
                            "e1_sous_plancher": len(below_bc)},
            "totaux_bc": dict(by_bc),
            "perdu_bc": per,
            "modifie_classees_bc": mod,
            "section_2022_preagentic": {
                "paires": len(pairs_ab), "plancher": round(thr_ab, 4),
                "non_resolues": unres_ab, "e1_sous_planchier": len(below_ab),
                "e1_revoquees": [{"path": a.get("path", ""), "titre": a["titre"][:60], "sim": round(s, 4)}
                                  for a, _c, s in below_ab],
                "totaux": dict(by_ab), "details": cells_ab,
            },
        }, open(args.json_out, "w", encoding="utf-8"), ensure_ascii=False, indent=1)
        print(f"detail nominatif -> {args.json_out}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
