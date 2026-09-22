#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#654 grain ④ — dry-run byte-exact de la worklist post-tag.

ZERO-CORPUS-WRITE : tout se passe dans %TEMP%. Le repo n'est jamais modifie.

Architecture :
  - On cree un mini-REPO scratch dans %TEMP% :
      <scratch>/tools/994-apostrophe-dryrun.py     (copie du vrai, byte-faithful)
      <scratch>/tools/654-mnemonics-scanner.py     (copie du vrai, byte-faithful)
      <scratch>/Cards/Fallacies/Argumentum Virtues - Taxonomy.csv   (copie scratch)
  - Le scanner officiel sur ce mini-REPO rend 66 findings (l'original)
  - On simule 52 remplacements sur la copie scratch du CSV
  - Le scanner officiel sur la copie simulee rend 0 findings
  - Le SHA256 du CSV REPO reste identique avant/apres (preuve 0 octet ecrit)

Distinction worklist / ecriture reelle :
  Le mnemonique latin canonique est DERIVE du titre_fr (qui porte deja la forme
  latine : 'Syllogisme Barbara' -> 'Barbara'). La forme owner-arbitree reelle peut
  etre differente (prefixe seul, ordre inverse, suffixe local), mais ici on PROUVE
  que le moteur byte-exact peut substituer 52 cellules et 0 ailleurs. La forme
  cible finale sera tranchee post-tag par l'owner ; ce dry-run pose l'instrument,
  pas la convention.

Usage :
    python tools/654-dryrun-byte-exact.py

Sortie (texte sur stdout) :
    SHA256 source REPO avant/apres  -- preuve 0 octet ecrit
    Distribution pre / post         -- 66 -> 0
    Worklist titre par langue       -- ru=14, ar=16, fa=16, zh=6

Prerequis :
    python >= 3.10
    Aucune dependance externe (stdlib only).
"""
import csv
import hashlib
import os
import shutil
import subprocess
import sys
import tempfile

# Racine derivee du fichier lui-meme : tools/<script>.py -> <repo>.
# ⛔ Jamais de chemin absolu en dur — un instrument doit tourner chez son destinataire.
REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
TOOLS_SRC = os.path.join(REPO, "tools", "654-mnemonics-scanner.py")
TOOLS_994_SRC = os.path.join(REPO, "tools", "994-apostrophe-dryrun.py")
VIRTUES_REL = "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv"
VIRTUES_SRC = os.path.join(REPO, VIRTUES_REL)

MNEMONICS = ["Barbara", "Celarent", "Darii", "Ferio", "Cesare", "Camestres",
             "Festino", "Baroco", "Darapti", "Felapton", "Disamis", "Datisi",
             "Bocardo", "Ferison", "Camenes", "Dimatis", "Fesapo", "Fresison",
             "Bamalip"]
TARGET = ["ru", "ar", "fa", "zh"]


def has_latin(cell):
    low = (cell or "").lower()
    return any(m.lower() in low for m in MNEMONICS)


def derive_syllogism_word(titles):
    """Plus long affixe commun a floor=max(3, N//2) titres natifs.

    Reprend l'algo du scanner 654-mnemonics-scanner.py (cf. sa syllogism_word()).
    La fonction est dupliquee ici pour rester autonome ; un drift entre les deux
    serait visible par un cross-check des deux instruments.
    """
    clean = [t.strip() for t in titles if t.strip()]
    if not clean:
        return None
    floor = max(3, len(clean) // 2)
    best = None
    for t in clean:
        for n in range(len(t), 0, -1):
            for cand in (t[:n], t[-n:]):
                if best is not None and len(cand) <= len(best):
                    break
                if sum(1 for u in clean if u.startswith(cand) or u.endswith(cand)) >= floor:
                    if best is None or len(cand) > len(best):
                        best = cand
    return best


def build_scratch_repo():
    """Cree <scratch>/{tools,Cards/Fallacies}/ et copie les 3 fichiers requis.

    Retourne (scratch_dir, csv_path_in_scratch).
    """
    scratch = tempfile.mkdtemp(prefix="arg654-dryrun-")
    tools_dst = os.path.join(scratch, "tools")
    cards_dst = os.path.join(scratch, "Cards", "Fallacies")
    os.makedirs(tools_dst)
    os.makedirs(cards_dst)
    shutil.copy(TOOLS_SRC, os.path.join(tools_dst, "654-mnemonics-scanner.py"))
    shutil.copy(TOOLS_994_SRC, os.path.join(tools_dst, "994-apostrophe-dryrun.py"))
    csv_dst = os.path.join(cards_dst, "Argumentum Virtues - Taxonomy.csv")
    shutil.copy(VIRTUES_SRC, csv_dst)
    return scratch, csv_dst


def run_scanner(scratch):
    """Execute le scanner sur le mini-REPO scratch, retourne stdout et exit code."""
    scanner = os.path.join(scratch, "tools", "654-mnemonics-scanner.py")
    r = subprocess.run([sys.executable, scanner], capture_output=True, text=True)
    return r.stdout, r.returncode


def main():
    print("=== grain ④ #654 — dry-run byte-exact ===\n")

    with open(VIRTUES_SRC, "rb") as f:
        sha_before = hashlib.sha256(f.read()).hexdigest()
    print(f"SHA256 source REPO avant : {sha_before}")

    scratch, csv_scratch = build_scratch_repo()
    print(f"Mini-REPO scratch        : {scratch}")

    out, rc = run_scanner(scratch)
    last = [l for l in out.splitlines() if "TOTAL findings" in l]
    print(f"Scanner sur SCRATCH pre-modif : {last[0].strip() if last else '???'} (exit={rc})")

    with open(csv_scratch, encoding="utf-8-sig", newline="") as f:
        reader = csv.reader(f)
        header = next(reader)
        idx = {h: i for i, h in enumerate(header)}
        rows = list(reader)

    mn_rows = [(i, r) for i, r in enumerate(rows) if has_latin(r[idx["title_fr"]])]
    syll = {}
    for lang in TARGET:
        native = [r[idx[f"title_{lang}"]] for _, r in mn_rows
                  if r[idx[f"title_{lang}"]].strip()
                  and not has_latin(r[idx[f"title_{lang}"]])]
        syll[lang] = derive_syllogism_word(native)
    print(f"\nMots du syllogisme : ru={syll['ru']!r} ar={syll['ar']!r} "
          f"fa={syll['fa']!r} zh={syll['zh']!r}")

    worklist = []
    for ri, r in mn_rows:
        title_fr = r[idx["title_fr"]]
        low = title_fr.lower()
        canon = None
        for m in MNEMONICS:
            if m.lower() in low:
                canon = m
                break
        if not canon:
            continue
        for lang in TARGET:
            col = f"title_{lang}"
            title = r[idx[col]]
            if title.strip() and not has_latin(title):
                word = syll[lang]
                if word:
                    if title.startswith(word):
                        after = f"{word}{canon}"
                    elif title.endswith(word):
                        after = f"{canon} {word}"
                    else:
                        after = f"{title} {canon}"
                else:
                    after = f"{title} {canon}"
                if after != title:
                    worklist.append((ri, col, title, after))

    print(f"\nWorklist (titres) : {len(worklist)} cellules")
    by_lang = {}
    for _, col, _, _ in worklist:
        lang = col.split("_")[1]
        by_lang[lang] = by_lang.get(lang, 0) + 1
    for lang, c in sorted(by_lang.items()):
        print(f"  title_{lang}: {c}")

    fields_modified = 0
    for ri, col, before, after in worklist:
        if rows[ri][idx[col]] != after:
            rows[ri][idx[col]] = after
            fields_modified += 1

    with open(csv_scratch, "w", encoding="utf-8-sig", newline="") as f:
        w = csv.writer(f, dialect="excel")
        w.writerow(header)
        w.writerows(rows)

    print(f"\nChamps reecrits sur la copie scratch : {fields_modified}")
    assert fields_modified == len(worklist), "INCOHERENCE : nombre de remplacements != worklist"

    out2, rc2 = run_scanner(scratch)
    last2 = [l for l in out2.splitlines() if "TOTAL findings" in l]
    print(f"Scanner sur SCRATCH post-modif    : {last2[0].strip() if last2 else '???'} (exit={rc2})")

    with open(VIRTUES_SRC, "rb") as f:
        sha_after = hashlib.sha256(f.read()).hexdigest()
    print(f"\nSHA256 source REPO apres : {sha_after}")
    print(f"Repo byte-identique       : {sha_before == sha_after}  (preuve 0 octet ecrit)")

    print("\n=== Synthese grain ④ ===")
    print(f"  Distribution qui fait foi (ref 04/07 + cross-check 2026-09-22) :")
    print(f"    title_ru={by_lang.get('ru', 0)} title_ar={by_lang.get('ar', 0)} "
          f"title_fa={by_lang.get('fa', 0)} title_zh={by_lang.get('zh', 0)} "
          f"(total {sum(by_lang.values())})")
    print(f"  Publication FAUSSE (commentaire v9 du 21/09 21:51Z) :")
    print(f"    ru 16 / ar 14 / fa 14 / zh 6 -- inversion sur ru/ar/fa.")
    print(f"  Source : commentaire #654 c.2026-09-21T21:51:41Z,")
    print(f"          ligne 'Distribution des 66 findings (telle que rendue)'.")
    print(f"  Cause probable : copier-coller entre ticks, sans cross-check.")
    print(f"  Scanner SOURCE   : {last[0].strip() if last else 'N/A'}")
    print(f"  Scanner POST-SIM : {last2[0].strip() if last2 else 'N/A'}  (cible post-campagne = 0)")
    print(f"  Repo SHA inchange: {sha_before == sha_after}")
    print(f"  Worklist titre   : {fields_modified} cellules")

    shutil.rmtree(scratch, ignore_errors=True)
    print(f"\nScratch {scratch} supprime. AUCUNE ecriture dans le repo.")


if __name__ == "__main__":
    main()
