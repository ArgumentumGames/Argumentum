# #1520 phase 0 — figer le corpus, extraire les 5 grappes, vérifier les comptes (oracle : 13/29/85/123/257 = 507).
# Le FR fait foi (lecture seule) ; champs desc_<l>/example_<l> pour en ru pt es ar zh fa.
import csv, io, subprocess, sys

SHA = "25327fef"
REPO = r"D:\Dev\Argumentum"
CSV_PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
CLUSTERS = {"5.2": 13, "4.1": 29, "6.1": 85, "2.1": 123, "6.3": 257}
LANGS = ["en", "ru", "pt", "es", "ar", "zh", "fa"]
# FR inclus (il fait foi — référence du prompt de sélection ET source de la classe 3 des défauts plantés)
FIELDS = [f"{p}_{l}" for l in ["fr"] + LANGS for p in ("desc", "example")]

raw = subprocess.run(["git", "-C", REPO, "show", f"{SHA}:{CSV_PATH}"],
                     capture_output=True, check=True).stdout
assert raw.startswith(b"\xef\xbb\xbf"), "BOM absent du blob"
text = raw.decode("utf-8-sig")
rows = list(csv.DictReader(io.StringIO(text)))
print(f"corpus {SHA}: {len(rows)} lignes")

out = {}
for prefix, expected in CLUSTERS.items():
    members = [r for r in rows if r["path"] == prefix or r["path"].startswith(prefix + ".")]
    cards = [r for r in members if (r.get("carte") or "").strip()]
    print(f"grappe {prefix}: {len(members)} noeuds ({len(cards)} cartes) — attendu {expected}")
    assert len(members) == expected, f"grappe {prefix}: {len(members)} != {expected}"
    out[prefix] = [{"pk": r["PK"], "path": r["path"], "titre_fr": r.get("text_fr", ""),
                    "carte": (r.get("carte") or "").strip() != "",
                    **{f: r.get(f, "") for f in FIELDS}} for r in members]

import json
json.dump({"sha": SHA, "clusters": out},
          open(r"D:\Dev\Argumentum\.tmp\1520-luna\clusters.json", "w", encoding="utf-8"),
          ensure_ascii=False, indent=1)
total = sum(len(v) for v in out.values())
print(f"TOTAL {total} noeuds (attendu 507)")
assert total == 507
print("OK — clusters.json ecrit")
