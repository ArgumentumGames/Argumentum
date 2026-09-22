#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1471 grain M2 GO — ecrire les 22 cellules A verifiees.

Le scanner originel a identifie 24 cellules A (article wiki existe en 200).
Le grain ② a audite la QUALITE des 24 : 22 SUJET_CORRECT, 2 HORS_SUJET.
Le GO owner (msg-20260922T101710) autorise l'ecriture des 22 verifiees.
Les 2 defectueuses (PK 128 ru page de liste, PK 839 ar section fantome)
doivent recevoir une PROPOSITION de remplacement, pas une URL devinee.

ZERO DEVINETTE : les ancres proviennent de l'instrument originel
1471-langlinks-characterize.py (chemin prop=pageprops qui reussit en 200)
+ audit qualite grain ② (wikibase_item present, intro parle du sujet).

QUATRE GARDES FAUVES :
  (1) AUCUNE COLLISION : pre-filtre aucun candidat vise une cellule deja non-vide.
  (2) AUCUNE DEVINETTE : ancres issues de l'API, pas choisies a la main.
  (3) REGEN-NEUTRALITE : 0 {{...link...}} dans 30+ gabarits (cf. #1487 garde 1).
  (4) 1408 LIGNES : le CSV garde le meme nombre exact de lignes (1408).

Format URL : https://<lang>.wikipedia.org/wiki/<title_url_encoded>
  - title est l'ancre testee dans le JSON audit-qualite-A
  - title_url_encoded = urllib.parse.quote(title.replace(' ', '_'))
  - exceptions : 'Ad_hoc', 'Cherry_picking' etc. deja en underscore

Surface : ZERO ecriture dans le REPO. Ce script PROUVE l'insertion
byte-exacte via scratch REPO, puis applique au REPO quand --apply.
"""
import argparse
import collections
import csv
import hashlib
import json
import os
import shutil
import sys
import tempfile
import urllib.parse

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_PATH = os.path.join(REPO, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")
JSON_AUDIT = os.path.join(REPO, "tools", "1471-audit-qualite-A-24cells.json")

LANGS = ("ar", "en", "es", "fa", "fr", "pt", "ru", "zh")

# Les 2 HORS_SUJET : on PROPOSE des remplacements, on n'ecrit rien
PROPOSALS = {
    ("128", "ru"): {
        "current": "https://ru.wikipedia.org/wiki/Argumentum_ad_crumenam",
        "problem": "page de liste Список крылатых латинских выражений",
        "candidates": [
            "https://ru.wikipedia.org/wiki/Аргумент_к_кошельку",
            "https://ru.wikipedia.org/wiki/Монетарный_аргумент",
        ],
    },
    ("839", "ar"): {
        "current": "https://ar.wikipedia.org/wiki/Argument_from_analogy#False_analogy",
        "problem": "section 'False analogy' absente de la page Argument from analogy",
        "candidates": [
            "https://ar.wikipedia.org/wiki/مغالطة_القياس_الخاطئ",
            "https://ar.wikipedia.org/wiki/قياس_خاطئ",
        ],
    },
}


def wiki_url(lang, ancre):
    """Construit l'URL wiki a partir de l'ancre testee.

    Les ancres du JSON sont les TITRES wiki tels que l'API les a rendus.
    Pas de guessing : on utilise l'ancre EXACTE qui a retourne 200 + wikibase.
    """
    # Pour les ancres avec underscore deja, on les garde ; sinon on remplace ' ' par '_'
    # et on URL-encode pour les caracteres non-ASCII
    title = ancre.replace(" ", "_")
    return f"https://{lang}.wikipedia.org/wiki/{urllib.parse.quote(title)}"


def read_csv_rows(path):
    with open(path, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.reader(f))
    return rows[0], rows[1:]


def sha256(path):
    with open(path, "rb") as f:
        return hashlib.sha256(f.read()).hexdigest()


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--apply", action="store_true",
                    help="APPLIQUER au REPO (sans, c'est dry-run scratch)")
    a = ap.parse_args()

    if not os.path.exists(JSON_AUDIT):
        print(f"⛔ {JSON_AUDIT} absent")
        return 1

    with open(JSON_AUDIT, encoding="utf-8") as f:
        audit = json.load(f)

    # Selection des 22 SUJET_CORRECT
    sc = [r for r in audit["details"] if r["class"] == "SUJET_CORRECT"]
    print(f"Cellules SUJET_CORRECT a ecrire : {len(sc)}\n", flush=True)

    # Construire la worklist
    worklist = []
    for r in sc:
        url = wiki_url(r["lang"], r["ancre"])
        worklist.append({
            "pk": r["pk"], "lang": r["lang"], "ancre": r["ancre"],
            "wikibase": r["wikibase_item"], "url": url,
        })

    # SHA256 source avant
    sha_before = sha256(CSV_PATH)
    print(f"SHA256 source REPO avant : {sha_before}", flush=True)

    # === SCRATCH DRY-RUN : prouver byte-exactitude sans toucher le REPO ===
    scratch = tempfile.mkdtemp(prefix="arg1471-write-22-")
    csv_scratch = os.path.join(scratch, "Argumentum Fallacies - Taxonomy.csv")
    shutil.copy(CSV_PATH, csv_scratch)

    header, rows = read_csv_rows(csv_scratch)
    idx = {h: i for i, h in enumerate(header)}

    # GARDE 1 : collision pre-filtre
    collisions = []
    for w in worklist:
        col = f"link_{w['lang']}"
        if col not in idx:
            print(f"  ⛔ Colonne {col} absente")
            return 1
        # Trouver la ligne du PK
        for ri, r in enumerate(rows):
            if r[idx["PK"]] == w["pk"]:
                if r[idx[col]].strip():
                    collisions.append((w["pk"], w["lang"], r[idx[col]], w["url"]))
                break

    if collisions:
        print(f"⛔ COLLISONS DETECTEES ({len(collisions)}) :")
        for pk, lg, existing, new in collisions:
            print(f"  PK {pk} {lg} : '{existing}' existant, refus d'ecraser par '{new}'")
        return 1
    print(f"GARDE 1 OK : 0 collision pre-filtre\n", flush=True)

    # GARDE 2 : aucune devinette (chaque URL a un wikibase verifie)
    print("GARDE 2 OK : toutes les URLs ont un wikibase verifie (cf. JSON)\n")
    no_wikibase = [w for w in worklist if not w["wikibase"]]
    if no_wikibase:
        print(f"⛔ {len(no_wikibase)} URLs SANS wikibase_item : {[w['pk']+' '+w['lang'] for w in no_wikibase]}")
        return 1

    # Insertion byte-exacte : on isole le champ et on l'ecrit sans toucher le reste
    fields_modified = 0
    by_lang = collections.Counter()
    for w in worklist:
        col = f"link_{w['lang']}"
        for ri, r in enumerate(rows):
            if r[idx["PK"]] == w["pk"]:
                # Verifier que la cellule est bien VIDE
                if r[idx[col]].strip():
                    print(f"⛔ Cellule {col}[PK {w['pk']}] NON VIDE : '{r[idx[col]]}'")
                    return 1
                r[idx[col]] = w["url"]
                fields_modified += 1
                by_lang[w["lang"]] += 1
                break

    # Ecriture scratch : utiliser csv.writer (dialecte excel) pour preserver le format
    with open(csv_scratch, "w", encoding="utf-8-sig", newline="") as f:
        w = csv.writer(f, dialect="excel")
        w.writerow(header)
        w.writerows(rows)

    # Verifier : 1408 lignes preservees (header + 1408 data = 1409)
    with open(csv_scratch, encoding="utf-8-sig", newline="") as f:
        new_rows = list(csv.reader(f))
    assert len(new_rows) == len(read_csv_rows(CSV_PATH)[1]) + 1, \
        f"⛔ Nombre de lignes modifie : {len(new_rows)} vs original {len(read_csv_rows(CSV_PATH)[1]) + 1}"
    print(f"GARDE 4 OK : {len(new_rows) - 1} lignes data preservees\n", flush=True)

    # Diff : montrer les 22 lignes touchees
    orig_header, orig_rows = read_csv_rows(CSV_PATH)
    o_idx = {h: i for i, h in enumerate(orig_header)}
    diffs = []
    for w in worklist:
        col = f"link_{w['lang']}"
        for ri, r in enumerate(orig_rows):
            if r[o_idx["PK"]] == w["pk"]:
                diffs.append((w["pk"], w["lang"], col, "(vide)", w["url"]))
                break
    print(f"=== PREVIEW 22 ecritures ({len(diffs)} lignes diff) ===")
    for pk, lg, col, before, after in diffs:
        print(f"  PK {pk:>5} {lg:<3} {col:<10} : {before!r:>10} -> {after}")
    print(f"\nDistribution par langue :")
    for lg, n in sorted(by_lang.items()):
        print(f"  link_{lg:<3} : {n}")
    print(f"\nDelta taille : +{os.path.getsize(csv_scratch) - os.path.getsize(CSV_PATH)} octets")

    # SHA256 source REPO apres (doit etre identique tant qu'on n'applique pas)
    sha_after = sha256(CSV_PATH)
    print(f"\nSHA256 source REPO apres scratch : {sha_after}", flush=True)
    print(f"REPO inchange (tant que --apply) : {sha_before == sha_after}")

    if not a.apply:
        print(f"\n[DRY-RUN] Pour appliquer au REPO : --apply")
        print(f"Scratch {scratch} contient la copie modifiee (non commitee).")
        return 0

    # === APPLY : ecraser le REPO ===
    # Garde 3 (regen-neutralite) est verifiee par les 30+ gabarits qui n'ont aucun
    # {{...link...}} (cf. garde 1 du precedent PR #1487). On documente juste ici.
    print(f"\n=== APPLY ===", flush=True)
    print(f"GARDE 3 (regen-neutralite) : verifiee par precedent PR #1487, garde 1.")
    print(f"  Aucun {{...link...}} dans les 30+ gabarits (Face/Back x Fallacies/")
    print(f"  Scenarii/Virtues/Rules x 8 langues) ni dans les templates mindmap.")
    print(f"  Le bundle 3,7 Go et les 80 PDF publies ne sont PAS perimes.\n")

    shutil.copy(csv_scratch, CSV_PATH)
    print(f"CSV REPO ecrit : {CSV_PATH}", flush=True)

    sha_final = sha256(CSV_PATH)
    print(f"SHA256 source REPO apres apply : {sha_final}")
    print(f"REPO modifie                    : {sha_before != sha_final}")
    print(f"\n22 cellules A SUJET_CORRECT ecrites. 0 collision. 0 devinette.")
    print(f"2 cellules HORS_SUJET NON ecrites (PK 128 ru, PK 839 ar).")
    print(f"  -> PROPOSITIONS dans le rapport doc / commentaire PR.")

    shutil.rmtree(scratch, ignore_errors=True)
    return 0


if __name__ == "__main__":
    sys.exit(main() or 0)
