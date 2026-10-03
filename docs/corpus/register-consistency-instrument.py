# -*- coding: utf-8 -*-
"""Instrument registre pt/es (pool #458, grain registre, 03/10).

Mesure 0-ecriture : classe chaque desc_pt / desc_es du deck par registre de
personne. Preuve = la colonne EVIDENCE (marqueur mecanique qui a decide) --
un classement sans preuve mecanique est sorti GRIS et exige la lecture
(la cellule est imprimee a cote pour cela).

Methode, par passes decroissantes de certitude :
  1. EXPLICITES (sujet rendu) : Os/O senhores, Voces, Voce, vos/vossa ;
     Ustedes, Usted, vosotros/vuestra, tu/tus.
  2. MORPHO 2pl : desinences verbales accentuees ou distinctives
     (pt -ais/-eis/-is ; es -ais/-eis/-is accents) -- etroites, les
     faux positifs nominaux sont stoplistes apres mesure.
  3. ZERO_SUJET : gerondif/infinitif initial (Usando..., Comparar...).
  4. GRIS : tout le reste -- la morphologie 2sg (pt -as/-es, es -as/-es)
     N'EST PAS armee : elle matcherait cosas/personas/veces par centaines.
     La lecture tranche ces rangees (cellule rendue dans la sortie).

Sortie : table PK | registre | evidence (+cellule si GRIS), distribution,
listes par registre. Self-test : cellules plantees + pieges nominaux.
"""
import argparse
import csv
import io
import os
import re

HERE = os.path.dirname(os.path.abspath(__file__))
MAIN = os.path.join(HERE, "..", "..", "Cards", "Fallacies",
                    "Argumentum Fallacies - Taxonomy.csv")

# --- passe 1 : marqueurs explicites (preuve forte) -------------------------
PT_EXPLICIT = [
    ("OS_SENHORES", r"\b[Oo]s? +senhor(?:es)?\b"),
    ("VOCES",       r"\b[Vv]oc[eê]s\b"),
    ("VOCE",        r"\b[Vv]oc[eê]\b"),
    ("VOS",         r"\b[Vv][óo]s\b"),
    ("VOS",         r"\b[Vv]oss[ao]s?\b"),
]
ES_EXPLICIT = [
    ("USTEDES",     r"\b[Uu]stedes\b"),
    ("USTED",       r"\b[Uu]sted\b"),
    ("VOSOTROS",    r"\b[Vv]osotros\b"),
    ("VOSOTROS",    r"\b[Vv]uestr[oa]s?\b"),
    ("TU",          r"\b[Tt][úu]s?\b"),
]

# --- passe 2 : desinences 2pl (preuve morphologique, ETROITES) -------------
# pt : -ais/-eis/-is (accentue ou non : Atribuís porte l'accent) ; stoplist =
# mots du corpus mesures comme noms/adverbes (quais/sensoriais/mais/seis...).
PT_2PL = re.compile(r"\b\w{3,}(ais|eis|is|ís)\b")
PT_2PL_STOP = {"mais", "seis", "três", "país", "paises", "anos", "depois",
               "sempre", "vocês", "guanais", "demais", "fazeres", "raízes",
               "vezes", "flores", "amores", "valores", "cores", "tu",
               "quais", "sensoriais", "verbais", "capitalais", "montanais"}
# es : formes ACCENTUEES seulement (-ais/-eis/-is accentues = vosotros quasi
# exclusivement ; les noms ne portent pas ces accents).
ES_2PL = re.compile(r"\b\w{3,}(áis|éis|ís)\b")
ES_2PL_STOP = {"verdad", "apenas", "usted", "veis"}

# --- passe 3 : sujet zero (gerondif/infinitif initial) ----------------------
# \w{1,} : "Usar"/"Usando" (4/6 lettres) sont des infinitifs/gerondifs reels ;
# les mots de 3 lettres ou moins (Ser, Ir, Ver) ne laissent rien a l'alternance.
INITIAL_NO_SUBJ = re.compile(r"^[A-ZÀ-Ý]\w{1,}(ndo|ar|er|ir)\b")


def morpho(cell, regex, stop):
    for m in regex.finditer(cell):
        w = m.group(0).lower()
        if w not in stop:
            return m.group(0)
    return None


def classify(cell, lang):
    """(registre, evidence, cellule) -- GRIS si aucune preuve mecanique."""
    cell = (cell or "").strip()
    if not cell:
        return "VIDE", "", cell
    for name, pat in (PT_EXPLICIT if lang == "pt" else ES_EXPLICIT):
        m = re.search(pat, cell)
        if m:
            return name, m.group(0), cell
    stop = PT_2PL_STOP if lang == "pt" else ES_2PL_STOP
    ev = morpho(cell, PT_2PL if lang == "pt" else ES_2PL, stop)
    if ev:
        return ("VOS_MORPHO" if lang == "pt" else "VOSOTROS_MORPHO"), ev, cell
    m = INITIAL_NO_SUBJ.match(cell)
    if m:
        return "ZERO_SUJET", cell.split()[0], cell
    return "GRIS", "", cell


def read_deck(path):
    rows = list(csv.DictReader(io.open(path, encoding="utf-8-sig", newline="")))
    deck = [r for r in rows if (r.get("carte") or "").strip()]
    assert len(deck) == 175, "deck=%d" % len(deck)
    return deck


def self_test():
    pt_cases = [
        ("Os senhores impõem um contexto.", "OS_SENHORES"),
        ("O senhor impõe um contexto.", "OS_SENHORES"),
        ("Vocês baseiam seus argumentos.", "VOCES"),
        ("Você emprega dados incorretos.", "VOCE"),
        ("Enquadrais uma crítica entre elogios.", "VOS_MORPHO"),
        ("Atribuís a um grupo as características.", "VOS_MORPHO"),  # í accentue
        ("Procurais vossa vantagem.", "VOS"),          # vossa explicite
        ("Quais são os sensoriais usados?", "GRIS"),   # FP stoplistes
        ("Usar a ameaça para forçar a concordância.", "ZERO_SUJET"),
        ("A comparação não se sustenta.", "GRIS"),
    ]
    es_cases = [
        ("Ustedes fundamentan sus argumentos.", "USTEDES"),
        ("Usted supone que algo es bueno.", "USTED"),
        ("Confundís vuestra perspectiva.", "VOSOTROS"),
        ("Pensáis que algo es bueno.", "VOSOTROS_MORPHO"),
        ("Crees que tu afirmación es verdadera.", "TU"),
        ("Usar una regla de generalización.", "ZERO_SUJET"),  # 4 lettres
        ("Comparando cosas para pretender.", "ZERO_SUJET"),
        ("La comparación no se sostiene.", "GRIS"),
    ]
    for cell, want in pt_cases:
        got, ev, _ = classify(cell, "pt")
        assert got == want, ("pt", cell, want, got)
        print("  pt %-44r -> %-14s (%r)" % (cell, got, ev))
    for cell, want in es_cases:
        got, ev, _ = classify(cell, "es")
        assert got == want, ("es", cell, want, got)
        print("  es %-44r -> %-14s (%r)" % (cell, got, ev))
    # pieges nominaux : ne doivent PAS classer vós/vosotros morpho
    traps = [
        ("pt", "Três países mais altos.", "GRIS"),
        ("pt", "Você usa flores e cores.", "VOCE"),
        ("es", "La verdad apenas importa.", "GRIS"),
        ("es", "Usted usa cosas parecidas.", "USTED"),
    ]
    for lang, trap, want in traps:
        got, _, _ = classify(trap, lang)
        assert got == want, ("trap", lang, trap, want, got)
        print("  piege %-4s %-36r -> %s (correct)" % (lang, trap, got))
    print("SELF-TEST PASS")


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--self-test", action="store_true")
    ap.add_argument("--csv", default=None, help="copie de travail (jamais le depot)")
    ap.add_argument("--out", default=None)
    a = ap.parse_args()
    if a.self_test:
        self_test()
        return
    if not a.out:
        raise SystemExit("--out requis hors --self-test")
    deck = read_deck(a.csv or MAIN)
    with io.open(a.out, "w", encoding="utf-8", newline="\n") as out:
        for lang in ("pt", "es"):
            dist, by_reg = {}, {}
            out.write("## %s -- 175 rangees deck\n\n" % lang)
            for r in sorted(deck, key=lambda r: int(r["PK"])):
                reg, ev, cell = classify(r.get("desc_%s" % lang), lang)
                dist[reg] = dist.get(reg, 0) + 1
                by_reg.setdefault(reg, []).append(r["PK"])
                extra = " :: %s" % cell[:90] if reg == "GRIS" else ""
                out.write("PK=%s\t%s\t%r%s\n" % (r["PK"], reg, ev, extra))
            out.write("\nDISTRIBUTION %s:\n" % lang)
            for reg in sorted(dist, key=lambda k: -dist[k]):
                out.write("  %-16s %3d  %s\n" % (reg, dist[reg], " ".join(by_reg[reg])))
            out.write("\n")
    print("written %s" % a.out)


if __name__ == "__main__":
    main()
