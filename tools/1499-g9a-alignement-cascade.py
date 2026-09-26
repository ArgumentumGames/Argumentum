#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1499 G9-A — alignement de cascade : re-derivation des candidats + controle de registre.

Candidat = PK x champ (text|desc|example) tel que la cellule fr a change depuis la base
(dernier master avant le 22/09 13:00Z) sans que les 7 traductions changent toutes.
Mesure ai-01 (pool v14, #458 c.5838808923) : 15 candidats sur 906284ab. Rejouee ici
INDEPENDAMMENT : la base est derivee par date, pas recitee.

Le classement ALIGNÉ / VOULU / DÉSALIGNÉ est une LECTURE (les 8 textes sont dans le
dossier docs/corpus/1499-g9a-alignement-cascade-2026-09-26.md) — sauf le registre de
PK 1361, mecanique : chaque langue porte des marqueurs formels/informels, et le DoD
exige le controle « une cellule alignee, modifiee dans une copie, doit sortir
DÉSALIGNÉ » (--self-test, sur LITTERAUX, avec temoin).

Usage :
    python tools/1499-g9a-alignement-cascade.py             # derive les candidats
    python tools/1499-g9a-alignement-cascade.py --dump      # + dump des 8 textes par cellule
    python tools/1499-g9a-alignement-cascade.py --self-test # controle de registre (1361)
"""
import argparse
import csv
import io
import os
import subprocess
import sys

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_REL = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
LANGS = ["fr", "en", "ru", "pt", "es", "ar", "fa", "zh"]
FIELDS = ["text", "desc", "example"]
EXPECTED_N = 15  # garde : la re-derivation doit rendre exactement la mesure ai-01


def rows_of(ref):
    out = subprocess.run(
        ["git", "show", f"{ref}:{CSV_REL}"], capture_output=True, check=True,
        cwd=REPO,
    ).stdout.decode("utf-8-sig")
    return {r["PK"].strip(): r for r in csv.DictReader(io.StringIO(out))}


def derive():
    base_ref = subprocess.run(
        ["git", "rev-list", "-1", "--before=2026-09-22T13:00:00Z", "origin/master"],
        capture_output=True, check=True, text=True, cwd=REPO,
    ).stdout.strip()
    # ⚠️ Tête ÉPINGLÉE (dispatch v17 grain 1, #458 c.5843387663) : la mesure
    # vise l'arbre de la revue ai-01 (15 candidats sur 906284ab). Sur un master
    # qui avance, une tête flottante rend 19 candidats et rougit la garde pour
    # une raison qui n'est pas un défaut : la cascade a continué d'écrire.
    HEAD_PINNED = "906284ab3897dddaf9f73f805860b6d1f3d7232d"
    head_ref = HEAD_PINNED
    base, head = rows_of(base_ref), rows_of(head_ref)
    cands = []
    for pk in sorted(set(base) & set(head), key=lambda p: int(p)):
        for f in FIELDS:
            ch = [L for L in LANGS if base[pk].get(f + "_" + L, "") != head[pk].get(f + "_" + L, "")]
            if "fr" in ch and len(ch) < 8:
                cands.append((pk, f, ch))
    assert len(cands) == EXPECTED_N, \
        f"re-derivation : {len(cands)} candidats != {EXPECTED_N} (mesure ai-01) — l'epic a bouge"
    return base_ref, head_ref, cands, base, head


# ── registre PK 1361 : marqueurs FORMELS/INFORMELS mesurés sur les textes du corpus ──
# en : exempt par construction (pas de distinction tutoiement/vouvoiement).
# ar : exempt PAR DÉCISION (erratum G9-A, 26/09) — #1546 : « l'arabe standard ne
# distingue pas tu/vous », épinglé par la garde Pk1361_Example_Matches_The_Polite_
# Register_Decision. Le classificateur naïf le comptait « retardataire » : faux positif.
FORMAL = {
    "fr": ["À vous entendre", "je vous ai vu"],
    "ru": ["вас послушать", "как вы ходили"],
    "pt": ["A ouvi-lo", "vi-o a aproveitar"],
    "es": ["Oyéndole", "le vi ir"],
    "fa": ["حرف‌هایتان", "می‌کردید"],
    "zh": ["您"],
}
INFORMAL = {
    "fr": ["À t’entendre", "je t’ai vu"],
    "ru": ["тебя послушать", "как ты ходил"],
    "pt": ["A ouvir-te", "vi-te a aproveitar"],
    "es": ["Oyéndote", "te vi ir"],
    "fa": ["حرف‌هایت", "می‌کردی"],
    "zh": ["你"],
}
EXEMPT = {"en", "ar"}


def register_of(cell):
    """cell = {lang: texte}. Rend la liste des langues NON exemptees au registre
    qui restent au tutoiement pendant que le fr (cible) est vouvoye."""
    if not all(m in cell["fr"] for m in FORMAL["fr"]):
        return ["fr"]  # la cible n'est pas au registre attendu : instrument hors sol
    lag = []
    for lg in LANGS:
        if lg in EXEMPT:
            continue
        informal = any(m in cell[lg] for m in INFORMAL[lg])
        formal = any(m in cell[lg] for m in FORMAL[lg])
        if informal and not formal:
            lag.append(lg)
    return lag


def cell_1361(head):
    return {L: head["1361"]["example_" + L] for L in LANGS}


def self_test(head):
    """Controle falsifiant sur LITTERERAUX + TEMOINS. Le HEAD (ar exempt par
    decision #1546) est le temoin vert ; les mutations portent sur des copies."""
    cur = cell_1361(head)
    cases = [
        ("HEAD (ar : exempt par décision #1546)", dict(cur), [], "ALIGNÉ (témoin)"),
        ("copie : pt (aligné) reverti au tutoiement",
         dict(cur, pt="A ouvir-te, toda compra é imoral. No entanto, vi-te a aproveitar os saldos outro dia, e a minha moral tolera isso."),
         ["pt"], "DÉSALIGNÉ (mutation vue — le contrôle DoD)"),
        ("copie : fr (cible) reverti au tutoiement",
         dict(cur, fr="À t’entendre, tout achat est immoral. Pourtant je t’ai vu faire les soldes l’autre jour, et ma morale le tolère."),
         ["fr"], "ROUGE instrument (la cible n'est plus au registre attendu)"),
    ]
    ok = True
    for name, cell, want, verdict in cases:
        got = register_of(cell)
        good = got == want
        ok &= good
        print(f"  [{'PASS' if good else 'FAIL'}] {name} -> retardataires {got or 'aucun'} "
              f"(attendu {want or 'aucun'}) : {verdict}")
    print("SELF-TEST " + ("OK (témoin HEAD vert ; mutation pt vue ; dérive fr vue)" if ok else "NON PROUVE"))
    return 0 if ok else 1


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--dump", action="store_true")
    ap.add_argument("--self-test", action="store_true")
    args = ap.parse_args()

    base_ref, head_ref, cands, base, head = derive()
    print(f"base = {base_ref[:8]}  head = {head_ref[:8]}  candidats = {len(cands)}")
    for pk, f, ch in cands:
        print(f"  PK {pk:5s} {f:7s} changés={','.join(ch)}")

    if args.dump:
        out = os.path.join(REPO, "g9a-dump.md")
        with open(out, "w", encoding="utf-8") as fh:
            for pk, f, ch in cands:
                fh.write(f"\n## PK {pk} — {f} (changés : {', '.join(ch)})\n\n")
                for L in LANGS:
                    col = f + "_" + L
                    fh.write(f"### {L} ({'CHANGÉ' if base[pk].get(col) != head[pk].get(col) else 'inchangé'})\n"
                             f"- base : {base[pk].get(col, '')}\n- tête : {head[pk].get(col, '')}\n")
        print(f"dump : {out}")

    if args.self_test:
        return self_test(head)
    return 0


if __name__ == "__main__":
    sys.exit(main())
