# -*- coding: utf-8 -*-
# #498 grain 10 — dump lecture-seule des 116 correspondances AIF_skosMappingType.
# Produit une ligne par porteur : PK, type déclaré, path, libellé FR, cibles Direct/Exception.
# Ne lit rien d'autre, n'écrit rien : rejouable, sortie = le tableau de travail du doc
# docs/ontology/498-grain10-mappingtype-mesure-2026-11-10.md.
import csv, os, sys

CSV_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Cards", "Fallacies",
                        "Argumentum Fallacies - Taxonomy.csv")

rows = list(csv.DictReader(open(CSV_PATH, encoding="utf-8-sig")))
mt = [r for r in rows if (r.get("AIF_skosMappingType") or "").strip()]
assert len(mt) == 116, f"attendu 116 porteurs, lu {len(mt)}"

for r in sorted(mt, key=lambda r: (r.get("path") or "")):
    d = (r.get("AIF_skosDirectRef") or "").strip().replace("\n", " ")
    e = (r.get("AIF_skosExceptionRef") or "").strip().replace("\n", " ")
    t = (r.get("text_fr") or "").strip().replace("\n", " ")[:46]
    typ = r["AIF_skosMappingType"].replace("skos:", "")[:6]
    print(f"{r['PK']:>5}|{typ:>6}|{(r.get('path') or '')[:22]:<22}|{t:<46}|D={d[:34]:<34}|E={e[:46]}")
