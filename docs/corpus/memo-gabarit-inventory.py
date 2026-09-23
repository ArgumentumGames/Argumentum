#!/usr/bin/env python3
"""#1499/G8 — Memo : chiffrage du contenu embarqué dans le gabarit (instrument, lecture seule).

La carte Mémo n'a PAS de CSV source : son contenu vit dans la clé `csv` INTERNE du
gabarit `Cards/Memo/Argumentum_Memo_{Face,Back}_fr.json` (⛔ jamais de grep brut du
.json : l'extraction passe par json.loads). Le pipeline, lui, INJECTE la taxonomie
vivante filtrée `carte ∈ {1,2}` (WebBasedGeneratorConfig.cs, DataSet FallaciesTaxonomy) :
le csv embarqué n'est donc rendu que hors pipeline (CardPen autonome) — sa dérive
est LATENTE, pas imprimée.

Cet instrument MESURE (tout est calculé, rien d'imprimé en chaîne fixe) :
- le chiffrage croisé gabarit ↔ `Argumentum Fallacies - Taxonomy.csv` (HEAD) ↔
  `Cards/Memo/Archive/2022/` (référence imprimée, jointure par `path` — l'archive
  2022 n'a PAS de colonne PK) ;
- les invariants structurels (voir rc ci-dessous), dont la preuve de renumérotation
  PK inter-génération qui interdit toute jointure PK au-delà de HEAD.

Gardes : lecture seule stricte, aucune écriture CSV/gabarit.

Usage :
  python docs/corpus/memo-gabarit-inventory.py [face.json [back.json]]
    chemins optionnels : copies mutées pour le CONTRÔLE INVERSE
    (PK fabriqué ⇒ INV-B casse ; Face ≠ Back ⇒ INV-A casse ; …).

Codes de sortie :
  0  chiffrage complet et invariants tenus (les écarts mesurés sont des RÉSULTATS) ;
  2  invariant cassé ou structure illisible : gabarit sans clé `csv`, Face ≠ Back,
     chemins dupliqués, valeur `carte` hors {1,2}, PK embarqué absent de la taxonomie
     HEAD, couverture de chemins < 90 %, ou archive 2022 < 140 lignes.
"""
import csv
import io
import json
import os
import sys

FACE = os.path.join("Cards", "Memo", "Argumentum_Memo_Face_fr.json")
BACK = os.path.join("Cards", "Memo", "Argumentum_Memo_Back_fr.json")
TAXONOMY = os.path.join("Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")
ARCHIVE_2022 = os.path.join("Cards", "Memo", "Archive", "2022", "Argumentum_Memo_Face_Francais.json")
MEMO_CARTE_VALUES = {"1", "2"}
MIN_ARCHIVE_ROWS = 140
MIN_PATH_COVERAGE = 0.90


def load_csv_key(path):
    with open(path, encoding="utf-8-sig") as fh:
        d = json.load(fh)
    if "csv" not in d or not d["csv"].strip():
        print(f"ERREUR: clé 'csv' absente/vide dans {path}")
        return None
    return d["csv"]


def rows_of(text):
    return list(csv.DictReader(io.StringIO(text)))


def main(argv):
    sys.stdout.reconfigure(encoding="utf-8")
    face_path = argv[1] if len(argv) > 1 else FACE
    back_path = argv[2] if len(argv) > 2 else BACK
    fail = False

    face_csv = load_csv_key(face_path)
    back_csv = load_csv_key(back_path)
    if face_csv is None or back_csv is None:
        return 2
    print(f"=== INV-A : Face et Back partagent le même csv embarqué ({face_path} vs {back_path}) ===")
    inv_a = face_csv == back_csv
    print(f"  identiques octet à octet : {'OUI' if inv_a else 'NON'}")
    if not inv_a:
        fail = True

    live = rows_of(face_csv)
    print(f"\n=== Chiffrage du csv embarqué ===\n  lignes : {len(live)} | colonnes : {len(live[0])}")
    paths = [r["path"] for r in live]
    dup = len(paths) - len(set(paths))
    print(f"  INV-E chemins uniques : {'OUI' if dup == 0 else f'NON ({dup} doublons)'}")
    if dup:
        fail = True
    hors = sorted({r["carte"] for r in live} - MEMO_CARTE_VALUES)
    by_carte = {v: sum(1 for r in live if r["carte"] == v) for v in sorted({r["carte"] for r in live})}
    print(f"  INV-D domaine carte {{1,2}} : {'OUI' if not hors else 'NON ' + str(hors)} | répartition : {by_carte}")
    if hors:
        fail = True

    tax = list(csv.DictReader(open(TAXONOMY, encoding="utf-8-sig")))
    tax_by_pk = {r["PK"] for r in tax}
    tax_by_path = {r["path"] for r in tax}
    head_memo = [r for r in tax if r.get("carte") in MEMO_CARTE_VALUES]
    print(f"\n=== Taxonomie HEAD ===\n  lignes totales : {len(tax)} | filtre memo carte∈{{1,2}} : {len(head_memo)}")

    orphan = sorted({r["PK"] for r in live} - tax_by_pk)
    print(f"  INV-B PK embarqués tous dans HEAD : {'OUI' if not orphan else 'NON ' + str(orphan[:5])}")
    if orphan:
        fail = True

    common = [p for p in set(paths) if p in tax_by_path]
    cov = len(common) / len(set(paths))
    print(f"  INV-C couverture chemins embarqués dans HEAD : {len(common)}/{len(set(paths))} = {cov:.1%} "
          f"({'OK' if cov >= MIN_PATH_COVERAGE else 'SOUS LE SEUIL'})")
    if cov < MIN_PATH_COVERAGE:
        fail = True

    tax_path = {r["path"]: r for r in tax}
    renum = [(p, pk_of(live, p), tax_path[p]["PK"]) for p in common
             if pk_of(live, p) != tax_path[p]["PK"]]
    print(f"\n=== Renumérotation PK (preuve : jointure PK inter-génération INTERDITE) ===")
    print(f"  chemins communs avec PK gabarit != PK HEAD : {len(renum)}/{len(common)}")

    div_text = [p for p in common if text_fr_of(live, p) != (tax_path[p].get("text_fr") or "")]
    fam_div = [p for p in common if famille_of(live, p) != (tax_path[p].get("Famille") or "")]
    print(f"\n=== Dérive du contenu embarqué vs HEAD (jointure par chemin, {len(common)} communs) ===")
    print(f"  text_fr divergents : {len(div_text)} (ex : {div_text[:5]})")
    print(f"  Famille divergents : {len(fam_div)} (stable à 0 = attendu)")

    arch_csv = load_csv_key(ARCHIVE_2022)
    if arch_csv is None:
        return 2
    arch = rows_of(arch_csv)
    arch_paths = [r["path"] for r in arch]
    arch_dup = len(arch_paths) - len(set(arch_paths))
    print(f"\n=== Référence imprimée 2022 ({os.path.basename(ARCHIVE_2022)}) ===")
    print(f"  lignes : {len(arch)} | colonnes : {len(arch[0])} | colonne PK présente : "
          f"{'OUI' if 'PK' in arch[0] else 'NON (jointure par chemin uniquement)'}")
    print(f"  INV-F chemins uniques : {'OUI' if arch_dup == 0 else 'NON'}")
    if arch_dup or len(arch) < MIN_ARCHIVE_ROWS:
        fail = True

    hp = {r["path"] for r in head_memo}
    ap = set(arch_paths)
    ep = set(paths)
    print(f"\n=== Chiffrage croisé (clé = chemin) ===")
    print(f"  rendu HEAD (carte∈{{1,2}}) : {len(hp)} | imprimé 2022 : {len(ap)} | embarqué : {len(ep)}")
    print(f"  imprimé → rendu HEAD : {len(hp & ap)} communs, +{len(hp - ap)} ajoutés, -{len(ap - hp)} disparus")
    print(f"  embarqué vs imprimé : {len(ep & ap)} communs, +{len(ep - ap)} post-print, -{len(ap - ep)} retirés")
    arch_by_path = {r["path"]: r for r in arch}
    printed_titles = [(p, (arch_by_path[p].get("text_fr") or ""),
                       text_fr_of(live, p)) for p in (ep & ap)
                      if (arch_by_path[p].get("text_fr") or "") != text_fr_of(live, p)]
    print(f"  titres imprimés ≠ embarqués (chemins communs) : {len(printed_titles)}")
    for p, a, b in printed_titles[:8]:
        print(f"    {p:12s} imprimé « {a[:34]} » → embarqué « {b[:34]} »")

    print("\n=== LIMITES (ce que cet instrument n'établit pas) ===")
    print("  - l'archive 2022 n'a pas de PK : la jointure est par CHEMIN ; des sœurs")
    print("    restructurées peuvent mal s'apparier (leçon #1516) — deltas directionnels ;")
    print("  - la renumérotation PK mesurée ci-dessus interdit la jointure PK au-delà de HEAD ;")
    print("  - l'attribution owner/agentique des titres divergents = couche G1 (#1499), pas ici ;")
    print("  - le csv embarqué n'est PAS rendu par le pipeline (injection DataSet) : sa dérive")
    print("    n'est visible que hors pipeline (CardPen autonome) ;")
    print("  - aucune mesure de rendu (couleurs, tenue en page) — QA visuelle = ai-01.")

    if fail:
        print("\n[STRUCTURE: au moins un invariant est cassé]")
        return 2
    print(f"\n[{len(live)} nœuds embarqués] Instrument cohérent — les écarts ci-dessus sont des résultats.")
    return 0


def pk_of(rows, path):
    return next(r["PK"] for r in rows if r["path"] == path)


def text_fr_of(rows, path):
    return next((r.get("text_fr") or "") for r in rows if r["path"] == path)


def famille_of(rows, path):
    return next((r.get("Famille") or "") for r in rows if r["path"] == path)


if __name__ == "__main__":
    sys.exit(main(sys.argv))
