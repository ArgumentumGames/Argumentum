# AIF attack-graph export — empirical findings + honest verdict (read-only, 0 write)

**Date** : 2026-07-19
**Auteur** : po-2024 (tick 43, dispatch ai-01 grain msg-eltaoz — capacity fill while release v0.9.0 is gated)
**Posture** : read-only export of EXISTING AIF modelling. 0 new modelling. 0 write to CSV/OWL. Post-tag-safe.

## Contexte

ai-01 a dispatché un grain non-bloquant (msg-eltaoz, MEDIUM) : exporter le graphe d'attaques AIF typé — « les 145 fallacies fully-modeled, adjacence undercut/undermine/rebut via colonnes `AIF_attackType` + `AIF_attackedNode` » — en artefact consommable par CoursIA (CSV d'adjacence + slice OWL). DoD : script reproductible + README mapping + **verdict honnête si une partie n'est pas exportable (pas de fabrication)**.

## Méthode

Triple grounding : (Technique) lecture empirique du CSV master `c877c5ba` + parse OWL/XML de `argumentum.owl` ; (Conversationnel) dispatch ai-01 + mémoires `aif-no-inherit-attacktype-from-anchor` (historique tranches 1–1d) et `csv-byte-exact-column-insertion` (impl #753) ; (Sémantique) sémantique AIF/ASPIC+ (undercut/undermine/rebut → RA/I/CA-node). Script `tools/aif-attack-graph-export.py` (Python stdlib + `xml.etree`, read-only).

## Découverte empirique — le compte de ai-01 (145) est correct

| Métrique | Valeur |
|---|---|
| Lignes CSV total | 1408 |
| Fully-modeled (`AIF_attackType` + `AIF_attackedNode` populés) | **145** |
| `AIF_attackType` distincts | `undercut` 87 / `undermine` 53 / `rebut` 5 |
| `AIF_attackedNode` distincts | `RA-node` 87 / `I-node` 53 / `CA-node` 5 |
| Correspondance attackType→node-type | **145/145 axiomatique** (0 violation) |
| Concepts AIF canoniques référencés (skos) | 60 distincts, 70/145 edges portent ≥1 ref |
| Distribution familiale des 145 | Abus de langage 57 · Tricherie 22 · Insuffisance 16 · Erreur de raisonnement 14 · Influence 12 · Erreur mathématique 12 · Obstruction 12 |

## Découverte critique — `AIF_attackedNode` n'est pas un PK cible

**C'est le point qui change l'interprétation du grain.** `AIF_attackedNode` ne contient **pas** un PK de fallacy cible (un autre row). Elle encode le **type de nœud AIF abstrait attaqué** :

```
undercut  → RA-node   (attaque le lien de rule-application)
undermine → I-node    (attaque le nœud d'information/prémisse)
rebut     → CA-node   (attaque via conflicting application)
```

Correspondance **1:1 parfaite** (87=87, 53=53, 5=5, 0 violation). Donc :

**Le graphe d'attaque AIF est BIPARTITE** — 145 fallacies-attaquantes → 3 types de nœuds AIF abstraits — **PAS un graphe d'adjacence inter-fallacies** (« fallacy X attaque fallacy Y »).

L'OWL confirme indépendamment : chaque individual fallacy porte deux AnnotationProperties `aifAttackType` (Literal) + `aifAttackedNode` (IRI `http://www.arg.dundee.ac.uk/aif#{RA,I,CA}-node`). Le « atacqué » est un type de nœud, pas une autre fallacy.

## Deux graphes distincts exportés

| Graphe | Source | Nature | Artefact |
|---|---|---|---|
| **(A) Attaque AIF typé** | CSV cols AIF + OWL `aifAttackType`/`aifAttackedNode` | **Bipartite** fallacy → node-TYPE, 145 edges typés | `aif-attack-edges.csv` + `aif-owl-attack-edges.csv` |
| **(B) Relations inter-fallacies** | OWL ObjectProperties `isRelatedTo`, `mirrors`, `predatesOn`, `denounces`, `leverages`, `allows`, `opposes`, `inverts` | Fallacy → fallacy, **1734 edges**, mais relations sémantiques **génériques NON typées AIF** | `aif-relations-graph.csv` |

Le graphe (B) EST un vrai graphe d'adjacence inter-fallacies, mais il ne répond **pas** au critère « adjacence typée undercut/undermine/rebut » — ce sont des relations sémantiques génériques. Les deux sont exportés honnêtement avec leur vraie sémantique.

## Note de granularité CSV ↔ OWL (pas une incohérence)

`aif-attack-edges.csv` (CSV, 145) et `aif-owl-attack-edges.csv` (OWL, 142) **ne sont pas un cross-check bijectif** :

- CSV = granularité **row** (PK-keyed).
- OWL = granularité **fallacy individual** (IRI camelCase, ex `hastyGeneralization`).

Le CSV n'a pas de colonne PK→IRI, donc pas de mapping 1:1 établissable sans clé de join séparée. Les deux vues sont **indépendamment cohérentes** (0 violation d'axiome chacune). Distribution attackType alignée (CSV 87u/53m/5r vs OWL ~85u/~52m/5r) ; l'OWL lag de quelques rows (modelling CSV post-tranche pas encore régénéré dans l'OWL — attendu, l'OWL est dérivé).

## Artefacts livrés (`docs/ontology/aif-export/`)

| Fichier | Lignes | Contenu |
|---|---:|---|
| `aif-attack-edges.csv` | 145 | Graphe d'attaque AIF bipartite typé (CSV, PK-keyed, primaire) |
| `aif-canonical-concepts.csv` | 60 | Concepts AIF canoniques référencés via skos |
| `taxonomy-tree-edges.csv` | 1408 | Arbre taxonomique complet (backbone) |
| `aif-owl-attack-edges.csv` | 142 | Graphe d'attaque AIF (vue OWL, individual-keyed) |
| `aif-relations-graph.csv` | 1734 | Relations inter-fallacies (NON-AIF, sémantiques génériques) |
| `README.md` | — | Mapping colonnes → adjacence + verdict + schemas |

Script reproductible : `python tools/aif-attack-graph-export.py` (idempotent, stdlib only, ~3s, 0 write aux sources).

## Verdict

✅ **Export honnête livré.** Le graphe d'attaque AIF typé est exporté (145 edges, bipartite, 0 violation d'axiome) + l'arbre taxonomique (1408) + les concepts canoniques (60) + les relations inter-fallacies (1734, non-AIF).

⚠ **Pas de fabrication.** L'adjacence inter-fallacies d'attaque typée **n'existe pas** dans les données — les colonnes AIF encodent un graphe bipartite fallacy→node-type. Si le consumer CoursIA veut un graphe d'attaque inter-fallacies typé, c'est un **nouveau modelling** (grain séparé, gated), pas un export. Le graphe (B) des relations sémantiques est dispo en complément mais n'est pas typé AIF.

## Out of scope

- ⛔ Nouveau modelling AIF (edges inter-fallacies d'attaque).
- ⛔ Colonne PK↔IRI (changement de schéma CSV, pas un export).
- ⛔ Write aux sources (CSV taxonomy, `argumentum.owl`).

## Refs

- Dispatch ai-01 : msg-eltaoz (2026-07-19), grain non-bloquant post-tag-safe.
- AIF modelling : tranches 1–1d (PRs #498/#753/#769/#776/#779).
- Mémoires : `aif-no-inherit-attacktype-from-anchor`, `csv-byte-exact-column-insertion`, `test-counter-empirical-dotnet-test` (ne pas reprendre un compte sur parole — j'ai vérifié 145 empiriquement), `matcher-no-match-is-not-content-absent` (vérifier la cible empiriquement avant tout verdict content-loss).
- Consumers : CoursIA ICT #7289, uplift #5721/#6409 ; Argumentum Layer C v1.0 #790.

— po-2024 (tick 43, dispatch ai-01 grain msg-eltaoz)
