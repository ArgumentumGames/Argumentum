#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#654 — scanner des mnemoniques Virtues : perimetre DU CHANTIER (etat actuel).

Rend, pour les 4 langues a script natif (RU/AR/FA/ZH), les cellules ou le
mnemonique syllogistique n'est pas en forme latine canonique — c'est-a-dire
l'etat a corriger. Quand le chantier #654 sera execute (post-tag v2.0.0), ce
scanner doit rendre **0 finding** : c'est le DoD « scanner re-run = 0 finding »,
qui n'etait pas executable sans instrument committe.

    python tools/654-mnemonics-scanner.py            # etat courant
    python tools/654-mnemonics-scanner.py --expect 0 # code de sortie = DoD

Lecture seule. Aucune ecriture CSV.

CRITERE, et pourquoi il est ecrit ici (le chiffrage de reference, c. #654 du
2026-09-20, enonçait « jeton latin absent (title) ; forme translitteree native
presente (remark) » — la seconde moitie n'etait pas rejouable telle quelle) :

  title_<lang>   le jeton mnemonique LATIN est absent de la cellule.
  remark_<lang>  la translitteration native est REPETEE dans le remark.
                 Mesure : le mnemonique = le titre natif MOINS le mot du
                 syllogisme de la langue ; on teste si ce reste apparait dans
                 le remark.

  Le mot du syllogisme n'est PAS code en dur : c'est le jeton natif le plus
  frequent des 19 titres de la langue (Силлогизм 14/19, قياس 16/19,
  قیاس 16/19, 三段论 pour zh). Une liste en dur aurait ete un referentiel de
  plus a maintenir ; la frequence le derive du corpus.

  Pieges d'instrument deja payes sur ce chantier, a ne pas reintroduire :
  - tokeniser le CJK par mots : « 达拉普蒂三段论 » reste UN jeton et le
    scanner rend ZH=0 (faux zero). Ici le CJK est decoupe caractere par caractere.
  - retenir tous les jetons natifs du titre : le MOT DU SYLLOGISME se repete
    legitimement dans le remark et produit des faux positifs (RU 7 au lieu de 4).
  - lire  r.get("title")  : l'en-tete est  title_fr  — un pass sur « title »
    rend « FR vide 223/223 », artefact pur (cf. matcher NO-MATCH).
"""
import argparse
import importlib.util
import os
import re
import sys
from collections import Counter

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
VIRTUES = "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv"
TARGET = ["ru", "ar", "fa", "zh"]
LATIN = ["fr", "en", "pt", "es"]

MNEMONICS = ["Barbara", "Celarent", "Darii", "Ferio", "Cesare", "Camestres",
             "Festino", "Baroco", "Darapti", "Felapton", "Disamis", "Datisi",
             "Bocardo", "Ferison", "Camenes", "Dimatis", "Fesapo", "Fresison",
             "Bamalip"]
LOWER = [m.lower() for m in MNEMONICS]

# etalonnage : le chiffrage de reference du 2026-09-20, re-mesure sur master 6694d702
EXPECT_TITLE = {("title", "ru"): 14, ("title", "ar"): 16, ("title", "fa"): 16,
                ("title", "zh"): 6}
EXPECT_REMARK = {("remark", "ru"): 4, ("remark", "ar"): 4, ("remark", "fa"): 4,
                 ("remark", "zh"): 2}

CJK = re.compile(r"[一-鿿]")
SPLIT = re.compile(r"[^\w؀-ۿ一-鿿Ѐ-ӿ‌]+")


def load_instrument():
    path = os.path.join(REPO, "tools", "994-apostrophe-dryrun.py")
    spec = importlib.util.spec_from_file_location("apostrophe_instr", path)
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    return mod


def has_latin_mnemonic(cell):
    low = (cell or "").lower()
    return any(m in low for m in LOWER)


def tokens(cell):
    """Un caractere CJK = un jeton ; les autres scripts se decoupent par mots."""
    out = []
    for part in SPLIT.split(cell or ""):
        if not part:
            continue
        out.extend(list(part) if CJK.search(part) else [part])
    return out


def syllogism_word(titles):
    """Le mot du syllogisme = le plus long AFFIXE commun aux titres natifs.

    Ni prefixe ni suffixe code en dur : le mot est un PREFIXE pour ru/ar/fa
    (« Силлогизм … », « قياس … ») et un SUFFIXE pour zh (« …三段论 »). Une
    derivation par token le plus frequent est degeneree en CJK — elle rendait
    « 三 » (un caractere sur trois), ce qui laissait un stem pollue et faisait
    tomber le compte juste PAR CHANCE.
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


def strip_syllogism(cell, word):
    """Le titre natif prive du mot du syllogisme -> le mnemonique translittere."""
    c = (cell or "").strip()
    if not word:
        return c
    if c.startswith(word):
        c = c[len(word):]
    elif c.endswith(word):
        c = c[:-len(word)]
    return c.strip(" ,;:，：、·-")


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--expect", type=int, default=None,
                    help="code de sortie 1 si le total de findings differe")
    ap.add_argument("--quiet", action="store_true")
    a = ap.parse_args()

    instr = load_instrument()
    corp = instr.load(REPO, VIRTUES)
    cols = {h: n for n, h in enumerate(corp["header"])}
    for need in ["pk", "title_fr"] + [f"{k}_{l}" for l in TARGET for k in ("title", "remark")]:
        if need not in cols:
            sys.exit(f"COLONNE ABSENTE : {need!r} — instrument invalide")

    rows = []
    for idx, (row, _t) in enumerate(corp["rows"][1:], start=2):
        f = [instr.unquote(x) for x in instr.split_fields(row)]
        rows.append((idx, f[cols["pk"]].strip(), f))
    mnemonic_rows = [(i, p, f) for i, p, f in rows if has_latin_mnemonic(f[cols["title_fr"]])]

    # mot du syllogisme, derive par AFFIXE COMMUN sur les seuls titres NATIFS
    syllogism = {}
    for lang in TARGET:
        native = [f[cols[f"title_{lang}"]] for _, _, f in mnemonic_rows
                  if f[cols[f"title_{lang}"]].strip()
                  and not has_latin_mnemonic(f[cols[f"title_{lang}"]])]
        syllogism[lang] = syllogism_word(native)

    findings = []
    counts = {}
    for lang in TARGET:
        for _, pk, f in mnemonic_rows:
            title = f[cols[f"title_{lang}"]]
            remark = f[cols[f"remark_{lang}"]]
            if title.strip() and not has_latin_mnemonic(title):
                findings.append((lang, "title", pk, title))
                counts[("title", lang)] = counts.get(("title", lang), 0) + 1
                stem = strip_syllogism(title, syllogism[lang])
                if stem and (stem in (remark or "") or
                             (len(stem) > 4 and stem[:4] in (remark or ""))):
                    findings.append((lang, "remark", pk, remark))
                    counts[("remark", lang)] = counts.get(("remark", lang), 0) + 1

    total = len(findings)
    if not a.quiet:
        print(f"=== #654 — mnemoniques Virtues : {len(mnemonic_rows)} rangees, "
              f"pk {min(p for _, p, _ in mnemonic_rows)}..{max(p for _, p, _ in mnemonic_rows)} ===")
        print(f"    mot du syllogisme derive par langue : "
              + ", ".join(f"{l}={syllogism[l]!r}" for l in TARGET))
        print()
        print("  colonne     " + "".join(f"{l:>6}" for l in TARGET) + f"{'total':>8}")
        for kind in ("title", "remark"):
            line = f"  {kind:<11} "
            s = 0
            for l in TARGET:
                v = counts.get((kind, l), 0)
                s += v
                line += f"{v:>6}"
            print(line + f"{s:>8}")
        print()
        if findings:
            for lang, kind, pk, cell in findings:
                frag = cell if len(cell) <= 96 else cell[:93] + "..."
                print(f"  {lang} {kind:<6} pk {pk:<5} {frag}")
            print()
        # controle : les langues latines ne portent aucun titre fautif
        bad_latin = [l for l in LATIN
                     if any(not has_latin_mnemonic(f[cols[f"title_{l}"]])
                            for _, _, f in mnemonic_rows)]
        print(f"  controle langues latines ({'/'.join(LATIN)}) : "
              + ("0 titre fautif — OK" if not bad_latin else f"FAUTIF : {bad_latin}"))
        print(f"\n  TOTAL findings = {total}")

        # etalonnage : re-mesurer contre le chiffrage de reference AVANT de conclure
        gaps = []
        for key, exp in list(EXPECT_TITLE.items()) + list(EXPECT_REMARK.items()):
            got = counts.get(key, 0)
            if got != exp:
                gaps.append(f"{key[0]}_{key[1]} attendu {exp} -> {got}")
        print(f"\n  ETALONNAGE (chiffrage de reference 2026-09-20) : "
              + ("conforme" if not gaps else "ECART — " + " ; ".join(gaps)))
        if gaps:
            print("  ⚠️ L'instrument ne reproduit plus la reference : "
                  "soit le corpus a bouge, soit le scanner a derive.")

    if a.expect is not None:
        if total != a.expect:
            print(f"\n  DoD NON ATTEINT : {total} finding(s), attendu {a.expect}")
            return 1
        print(f"\n  DoD ATTEINT : {total} finding(s)")
    return 0


if __name__ == "__main__":
    sys.exit(main())
