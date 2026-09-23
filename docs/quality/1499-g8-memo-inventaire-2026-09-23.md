# #1499 / G8 — Memo : chiffrage du contenu embarqué dans le gabarit (dossier)

**Date** : 2026-09-23 (tick 19:41) · **Lane** : po-2023 (worker) · **Base** : `origin/master` `88d6713f`
**Instrument** : [`docs/corpus/memo-gabarit-inventory.py`](../corpus/memo-gabarit-inventory.py) — exécuté
**tel que livré** avant commit (rc=0 ; stdlib, lecture seule, aucun réseau). Le contenu du Memo vit
dans la clé `csv` INTERNE du gabarit `Cards/Memo/Argumentum_Memo_{Face,Back}_fr.json` — extrait par
`json.loads`, ⛔ jamais par grep brut du `.json`.

**⛔ Gardes** : zéro écriture CSV/gabarit ; dossier de chiffrage seulement (motif G4 : clé stable,
invariants calculés, contrôle inverse qui voit un écart injecté).

---

## Réponse en une ligne

Le Memo existe en **trois strates** : ce que le pipeline **rend** aujourd'hui (taxonomie HEAD filtrée
`carte ∈ {1,2}` = **175 nœuds**), ce que le gabarit **embarque** par défaut (**169 nœuds**, vintage
intermédiaire entre l'imprimé et HEAD), et l'**imprimé 2022** (**156 nœuds**) — et la clé `PK` a été
**renumérotée** entre le vintage embarqué et HEAD (**159/163 chemins communs changent de PK**),
ce qui interdit toute jointure PK inter-génération.

## 1. Les trois strates (mesuré)

| Strate | Lignes | Colonnes | Source |
|---|---:|---:|---|
| Rendu pipeline (HEAD) | 175 (86+89) | 104 | `Taxonomy.csv` filtré `carte ∈ {1,2}`, **injecté** au harvest (`WebBasedGeneratorConfig.cs`, DataSet `FallaciesTaxonomy`) |
| Csv embarqué (Face == Back, octet à octet) | 169 (83+86) | 54 | clé `csv` du gabarit — vue par défaut de CardPen **hors** pipeline |
| Imprimé 2022 | 156 (84+72) | 40 | `Cards/Memo/Archive/2022/` (6 fichiers, csv identique entre eux) |

Le csv embarqué n'est **pas** ce que le pipeline imprime : sa dérive est **latente** (visible
seulement en CardPen autonome / GitHub Pages). Le PDF, lui, suit la taxonomie vivante.

## 2. Chiffrage croisé (clé = chemin)

- Imprimé 2022 → rendu HEAD : **140 communs, +35 ajoutés, −16 disparus** (restructuration de
  l'arbre depuis février 2022).
- Embarqué vs imprimé : **153 communs, +16 post-print, −3 retirés** — l'embarqué est postérieur à
  l'imprimé.
- Embarqué vs HEAD : 163/169 chemins couverts (96,4 %), **6 chemins de l'embarqué n'existent plus**
  (restructurés) ; les 169 PK embarqués existent tous dans HEAD.
- Dérive du contenu embarqué vs HEAD (jointure par chemin) : **59 `text_fr` divergents**,
  `Famille` **stable à 0** — la structure de familles tient, les titres ont bougé (couche G1).

## 3. Découverte instrument : renumérotation PK

Sur les 163 chemins communs embarqué↔HEAD, **159 ont un PK différent** (ex : `1.1.1.2` PK 13 → 20,
`1.1.2` PK 23 → 33). Une jointure PK gabarit↔HEAD apparie donc **des nœuds différents** — la
première passe de mesure donnait 167/169 `text_fr` « divergents », artefact uniforme typique
(leçon #1516 généralisée : la jointure inter-génération se fait par **chemin**, jamais par PK).
L'archive 2022, elle, **n'a pas de colonne PK du tout** : jointure par chemin uniquement.

## 4. Titres imprimés ≠ embarqués (les 5, chemins communs)

| Chemin | Imprimé 2022 | Embarqué |
|---|---|---|
| `4` | Paralogisme | Erreur de raisonnement |
| `5` | Détournement de la langue | Abus de langage |
| `6.3.2` | Culturocentrisme | Ethnocentrisme |
| `2.1.1.4` | Poncif anticritique | Poncif anti critique |
| `7.2.3` | Empoisonner le puit (coquille) | Empoisonner le puits |

Ces 5 renommages sont **antérieurs** à la strate embarquée (post-print, pré-HEAD) — pas des
dérives agentiques récentes. L'attribution owner/agentique des 59 divergents embarqué↔HEAD
relève du dossier G1 (#1499 c.5794911968+), pas de celui-ci.

## 5. Contrôles inverses (exigés par le motif G4)

| # | Mutation (copie temporaire du gabarit) | Attendu | Mesuré |
|---|---|---|---|
| M1 | PK d'une ligne fabriqué (`13` → `999998`) | `INV-B` NON + **rc=2** | **PASS** (`NON ['999998']`, rc=2) |
| M2 | Face mutée + Back original | `INV-A` NON + **rc=2** | **PASS** (rc=2) |

Un écart injecté dans la clé `csv` est donc **vu** par l'instrument — les invariants peuvent
virer au rouge.

## 6. Ce que ce dossier n'établit pas

- Aucune recommandation d'écriture : l'embarqué n'alimente pas le PDF (injection DataSet) ;
  resynchroniser la clé `csv` du gabarit serait un confort CardPen-autonome, à arbitrer séparément.
- L'appariement chemin-à-chemin peut mal pairer des sœurs restructurées (#1516) : les deltas
  +35/−16/+16/−3 sont **directionnels**, pas carte-à-carte.
- Aucune mesure de rendu (couleurs, tenue en page) — QA visuelle = ai-01.
- Les 7 autres langues : le Memo est fr-seul par config.

## Reproductibilité

```bash
python docs/corpus/memo-gabarit-inventory.py   # depuis la racine, lecture seule, < 5 s
```

---
*po-2023 (worker lane) — signale, ne déclare pas PASS · arbitrage : owner & ai-01*
