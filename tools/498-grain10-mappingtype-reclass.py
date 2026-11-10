# -*- coding: utf-8 -*-
# #498 grain 10 — reclassification lecture-seule des 116 AIF_skosMappingType
# sous les arbitrages ai-01 du 10/11 19:40 (c.6775643852) :
#   Q1 : les 16 narrowM-feuilles du motif A -> broadMatch.
#   Q2 : têtes englobantes -> narrowMatch si scheme unique englobé,
#        relatedMatch sans inclusion si conflit/union ; JAMAIS retirées.
#   Q3 : closeMatch gardé SEULEMENT si cible de conflit unique ET même défaut ;
#        ExceptionRef (E non vide) ou union -> broadMatch ; scheme -> broad (DoD 2 strict).
# Reproduit la partition du doc docs/ontology/498-grain10-mappingtype-mesure-2026-11-10.md :
#   61 inchangées + 55 reclassées = 116 ; broad 102 / close 2 / narrow 4 / related 8.
# Ne lit rien d'autre, n'écrit rien.
import csv, os, sys, io
from collections import Counter

sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding="utf-8", errors="replace")
CSV_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Cards", "Fallacies",
                        "Argumentum Fallacies - Taxonomy.csv")

rows = list(csv.DictReader(open(CSV_PATH, encoding="utf-8-sig")))
mt = [r for r in rows if (r.get("AIF_skosMappingType") or "").strip()]
assert len(mt) == 116, f"attendu 116 porteurs, lu {len(mt)}"

def targets(field):
    return [t.strip() for t in (field or "").split(",") if t.strip()]

def tclass(t):
    if t.endswith("_Conflict"):
        return "conflict"
    if t.endswith("_Inference") or t.endswith("_Scheme"):
        return "scheme"
    return "autre"

# listes du doc (motif A §3.1 ; têtes §3.3 + §3.4)
Q1 = {801, 806, 807, 830, 831, 841, 850, 852, 854, 863, 877, 880, 881, 882, 884, 885}
Q2 = {1, 2, 356, 696, 833, 846, 876, 973, 1280}
# tranchées nommées au §3.5 (passent le filtre mécanique, verdict humain) :
NAMED_CLOSE = {886, 879}   # retenues closeMatch (même défaut que le conflit cible)
NAMED_REFUSED = {878}      # candidate refusée -> broadMatch (défaut différent)

reco = {}
notes = {}
for r in mt:
    pk = int(r["PK"])
    decl = r["AIF_skosMappingType"].replace("skos:", "")
    D = targets(r.get("AIF_skosDirectRef"))
    E = targets(r.get("AIF_skosExceptionRef"))
    allT = D + E
    classes = {tclass(t) for t in allT}
    n = len(allT)
    if pk in Q1:
        reco[pk] = "broadMatch"; notes[pk] = "Q1: motif A arbitre -> broadMatch"
    elif pk in Q2:
        if n > 1 or "conflict" in classes:
            reco[pk] = "relatedMatch"; notes[pk] = f"Q2: tete, cible {'union' if n>1 else 'conflit'} -> relatedMatch sans inclusion"
        else:
            reco[pk] = "narrowMatch"; notes[pk] = "Q2: tete, scheme unique englobe -> narrowMatch"
    elif decl == "closeMatch":
        # Q3 : survit seulement si cible de conflit UNIQUE et meme defaut
        if pk in NAMED_CLOSE:
            reco[pk] = "closeMatch"; notes[pk] = "Q3: tranchee nommement -> closeMatch (meme defaut)"
        elif pk in NAMED_REFUSED:
            reco[pk] = "broadMatch"; notes[pk] = "Q3: candidate refusee -> broadMatch (defaut different)"
        elif E:
            reco[pk] = "broadMatch"; notes[pk] = "Q3: ExceptionRef present -> broadMatch"
        elif n > 1:
            reco[pk] = "broadMatch"; notes[pk] = "Q3: union de cibles -> broadMatch"
        elif n == 1 and tclass(allT[0]) == "conflict":
            reco[pk] = "closeMatch"; notes[pk] = f"Q3: cible de conflit unique {allT[0]} -> closeMatch A VERIFIER (meme defaut ?)"
        elif n == 1 and tclass(allT[0]) == "scheme":
            reco[pk] = "broadMatch"; notes[pk] = f"Q3: scheme unique {allT[0]} -> broadMatch (DoD 2 strict)"
        else:
            reco[pk] = "broadMatch"; notes[pk] = f"Q3: cible {allT} -> broadMatch"
    else:
        reco[pk] = decl; notes[pk] = "inchange"

# ---- rapport ----
changed = {pk: (r["AIF_skosMappingType"].replace("skos:", ""), reco[pk]) for r in mt for pk in [int(r["PK"])]
           if r["AIF_skosMappingType"].replace("skos:", "") != reco[int(r["PK"])]}
unchanged = 116 - len(changed)
print(f"116 lignes ; {unchanged} inchangees ; {len(changed)} reclassees")
assert unchanged == 61 and len(changed) == 55, "partition attendue : 61 + 55"

c = Counter(reco.values())
print("distribution finale recommandee :", dict(c))
assert c == Counter({"broadMatch": 102, "closeMatch": 2, "narrowMatch": 4, "relatedMatch": 8}), \
    f"attendu broad 102 / close 2 / narrow 4 / related 8, lu {dict(c)}"

print("\n== detail des 55 reclassees ==")
for r in sorted(mt, key=lambda r: int(r["PK"])):
    pk = int(r["PK"])
    decl = r["AIF_skosMappingType"].replace("skos:", "")
    if decl != reco[pk]:
        D = targets(r.get("AIF_skosDirectRef")); E = targets(r.get("AIF_skosExceptionRef"))
        print(f"  PK {pk:5d} {decl:10s} -> {reco[pk]:12s} | {(r.get('text_fr') or '')[:38]:38s} | D={';'.join(D)} E={';'.join(E)}")

print("\nOK : partition 61 inchangees + 55 reclassees = 116 ; broad 102 / close 2 / narrow 4 / related 8.")
