# Protocole smoke DatasetUpdater round-trip (gpt-5.6-sol) — 3 records, sandbox par construction, gated

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base lecture** : master `6fbde739`
**Grain** : pool #458 renouvelé (c.5666260217), grain ⑩. ⛔ **Rédigé sans être exécuté** —
l'exécution est gated sur **GO owner + clé nommée**. Ce protocole hérite du prérequis mesuré par le
grain ⑦ ([inventaire gpt-5.5](gpt55-modele-courant-inventaire-2026-09-15.md)) : la config compile
44× `gpt-5.5`, 0× gpt-5.6.

---

## §0 Objet

Prouver le round-trip **prompt → API gpt-5.6-sol → parse → CSV écrit** sur 3 records, **sans toucher
au corpus gelé**, pour lever le dernier « Valider DatasetUpdater round-trip » (CLAUDE.md §Prochaines
étapes n°9, hérité de l'epic #202 close). Un vert ici n'autorise aucune campagne : il valide
l'instrument, pas la qualité de traduction (verdict qualité = passes dédiées + ai-01).

## §1 Prérequis (chacun mesuré ou tracé)

| # | Prérequis | Détail | Statut |
|---|---|---|---|
| P1 | **Bump de la task smoke — gated GO** | Créer/éditer UNE task dédiée `Model = "gpt-5.6-sol"` (1 ligne) — le reste de la config reste 5.5 (44 occurrences). Lancer sans bump = tester 5.5 en croyant tester 5.6. | ⛔ à faire, GO requis |
| P2 | **Clé nommée** | `OpenAIKeyPath` → fichier hors dépôt (`.keys/`, jamais committé — le défaut historique pointe un chemin G:Drive : le remplacer par le chemin local de la lane). | ⛔ GO requis |
| P3 | **Probe crédit AVANT** | `POST /v1/responses` minimal : un `429 credit_balance_exhausted` = stop immédiat. Un `GET /v1/models` 200 ne prouve rien. | à l'exécution |
| P4 | **API Responses + effort low** | `UseResponsesApi = true`, `ReasoningEffort = "low"` — sans quoi un modèle raisonneur brûle le budget en raisonnement (précédent mesuré, chat/completions inclu). | config à poser |
| P5 | **Sandbox par construction** | `TargetPath` = **copie scratch** du CSV cible dans un dossier jetable (jamais sous `Cards/`). Mesuré : l'écriture va à `TargetPath` (`DatasetUpdaterConfig.cs:434`), la source est lue séparément — 0 write corpus garanti par le chemin. | config à poser |

## §2 La task smoke (valeurs exactes, dérivées des champs réels)

```
Enabled = true                    // le seul geste d'armement, sur CETTE task uniquement
Name = "smoke-202-roundtrip"
Model = "gpt-5.6-sol"             // P1 — la seule occurrence 5.6 du dépôt au moment du smoke
UseResponsesApi = true ; ReasoningEffort = "low" ; MaxOutputTokens = 4096
SourceDataset = <DataSet Rules — 15 rangées, le plus petit corpus>
TargetPath = <scratchpad>/smoke202-rules-copy.csv   // P5 — copie fraîche du corpus Rules
ChunkSize = 3 ; TakeChunkNb = 1 ; SkipChunkNb = 0 ; RandomizeChunks = false   // = 3 records, reproductible
FieldsToUpdate = { une colonne Text_<lang> }        // 1 seul champ : le diff minimal lisible
MaxTokensPerMinute = 70000                          // 1 chunk ⇒ jamais atteint
```

Les mécanismes cités existent tous (`DatasetUpdaterConfig.cs:21-103`) : `TakeChunkNb` (l.72, −1 =
tout — **le laisser à −1 traite le corpus entier**), `SelectEmptyTargets` existe mais son bloc
d'usage est **commenté** (l.176-194) — ne pas compter dessus pour restreindre. `CompareMode=true`
saute la traduction (l.163) : ce n'est PAS un smoke LLM, l'ignorer.

## §3 Critères d'arrêt (n'importe lequel = stop + rapport)

1. **429 crédit** (P3) à la première requête ou en cours.
2. **Réponse non parsable** × 2 consécutives (JSON/function-calling cassé).
3. **Budget tokens > 200 k** consommés pour 3 records (≈ 30× l'attendu = quelque chose boucle).
4. **Dérive de sortie** : cellule écrite non vide mais identique à la source FR (contamination
   pass-through) ou vide — 1 occurrence = stop et inspection.
5. **Toute écriture détectée hors TargetPath** (`git status` du dépôt non propre pendant le run).

## §4 Critères de succès

- 3/3 records : réponse parsée, cellule écrite **dans la copie scratch uniquement**.
- `git status --porcelain` du dépôt **vide** à la fin du run (corpus intact — preuve, pas croyance).
- Diff scratch-vs-corpus : exactement 3 cellules modifiées (3 rangées × 1 champ), rien d'autre
  (formatage/quoting compris — une divergence de quoting est un échec du round-trip, pas un détail).
- Consommation tokens rapportée < plafond §3.3.

## §5 Rollback

Le corpus n'étant jamais cible (P5), le rollback = supprimer le scratch. En cas d'écriture accidentelle
dans le dépôt : `git checkout -- <fichier>` AVANT tout commit (le run ne committe pas — commit et PR
restent le protocole normal, worker ne merge jamais).

## §6 Ce que ce protocole n'établit pas

⛔ Pas exécuté — aucune mesure de latence/coût réel du 5.6-sol (budget §3.3 = borne dérivée, pas
mesure) · ⛔ la qualité de traduction n'est PAS jugée (3 records ne valident pas un style) · ⛔ le
nom exact du modèle (`gpt-5.6-sol` vs suffixe API) est à confirmer à P1 — RAPPORTÉ du GO 14/08, la
référence API vive fait partie du GO à nommer · ⛔ les prompts embarqués (5 fichiers citant 5.5) ne
sont pas audités pour la compatibilité 5.6 (grain ⑦ §1) · ⛔ n'autorise aucune campagne : le
smoke vert ouvre la QUESTION des passes, pas la réponse.

---
*po-2024 — pool #458 renouvelé (c.5666260217), grain ⑩. Prérequis P1-P2 = le GO owner à nommer ;
P3-P5 = mécanique mesurée sur `6fbde739`. L'exécution suit le GO, jamais l'inverse.*
