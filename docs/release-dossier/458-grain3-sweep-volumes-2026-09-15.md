# Pool #458 grain ③ — Sweep erratum-volumes : trace du balayage (191/358/489, 359/490, 192)

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base** : master `608a0fd9`
**Instrument** : `git grep` sur `origin/master` (`608a0fd9`), motif `191|358|489|359|490` (+ passe
complémentaire `192` pré-#1288 et fichiers non-`.md`), contexte cartes/deck/boîte/volume/instance,
extraction de fragments `-o` pour juger chaque hit ; lignes erratum/superseded/« 197 » écartées
comme déjà-corrigées.

## Résultat : 1 porteur réel, corrigé dans cette PR

| fichier | état | geste |
|---|---|---|
| `docs/publication/cards-catalog.en.md:37` | portait « **191 unique cards** » **sans** la phrase de volume décidé — le **jumeau EN du catalogue FR** que #1385 a corrigé (l.37 côté FR) mais dont la version anglaise a été manquée | erratum daté **dans cette PR** : phrase « Manufacturing volume decided (2026-09-14, #1187 c.5665864605) : **197** / boîte **364**/`**495** ; 358/489 superseded », miroir de la ligne FR |

## Ce qui a été balayé et écarté (néant constaté, motifs nommés)

- **Les 5 fichiers déjà erratés par #1385** (`CHANGELOG.md`, `CLAUDE.md` l.492/494,
  `REGEN_RELEASE_RUNBOOK.md:52`, `cards-catalog.fr.md:37`,
  `134-guide-validation-v2.0.0.md:71`) — exclus d'office : leurs occurrences de 191/358/489
  vivent **dans** l'erratum (comportement correct).
- **`PdfDeckCountContractTests.cs`** (seul hit non-`.md`) : épingle **197** instances
  (l.132) ; ses mentions de 191/358/192 (l.15, l.202-203) sont des **commentaires
  explicatifs** qui distinguent instances vs cartes uniques — lecture toujours vraie,
  **pas un porteur**. Aucun geste.
- **Passe `192`** (pré-#1288) : 0 porteur en contexte cartes hors errata existants.
- **Passe non-`.md`** : seul le test de contrat ci-dessus.
- **Fichiers possédés par des PR en vol** : aucun fichier du sweep n'appartient à #1390
  (dossier #1369, en attente merge), #666 (HOLD) ou aux PR Dependabot gelées.

## Ce que ce sweep n'établit pas

⛔ Il balaye **ce dépôt** à `608a0fd9` — pas les corps d'issues/PR GitHub ni les artefacts
figés hors dépôt (release drafts, GDrive), déjà couverts par #1383 (drift) et le grain ⑨
du cycle précédent. ⛔ La décision de volume elle-même (#1187 c.5665864605, 14/09) est
**citée, pas re-ouverte**. ⛔ Aucun CSV/template touché ; « 191 cartes uniques » reste une
lecture vraie (le contrat CI 197 instances/379 pages épinglé par #1288/#1187 est intact).

---
*po-2024 — pool #458 (commentaire c.5666260217, mis à jour 15/09 10:45), grain ③.*
