# Inventaire « gpt-5.5 périmé » — la décision court devant la config : 44× gpt-5.5 dans la config compilée, 0× gpt-5.6 dans le dépôt

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base** : master `6fbde739`
**Grain** : pool #458 renouvelé (c.5666260217), grain ⑦. **⛔ 0 write de config, 0 exécution** —
inventaire + erratum CLAUDE.md uniquement (le bump de modèle est un geste gated).

---

## §0 Le fait central (MESURÉ)

Le GO owner du **14/08** (coordination hors dépôt — trace écrite la plus proche dans le dépôt : pool
#458 c.5666260217, grain ⑩ « protocole gpt-5.6-sol ») fait de **gpt-5.6-sol** le modèle de
traduction ; gpt-5.5 est supersédé. Or, mesuré sur `6fbde739` :

| Instrument | Occurrences | Verdict |
|---|---|---|
| `DatasetUpdaterRootConfig.cs` (config courante compilée, `SkipConfigFile=true` ⇒ les valeurs C# **sont** la vérité) | **44× `Model = "gpt-5.5"`** + 4× `gpt-5.4` | **0× gpt-5.6** — la décision n'a jamais été migrée dans la config |
| Tout le dépôt (`docs/`, `tools/`, `Generation/`, README, CLAUDE.md) | `gpt-5.6` : **0 occurrence** | le nom du modèle courant n'existe nulle part dans le dépôt |
| Docs + code citant `gpt-5.5` | 1 590 lignes / 79 fichiers | à classer (§1) |

**Conséquence opérationnelle** : tout run `DatasetUpdater` lancé aujourd'hui (`Enabled=true` sur une
tâche quelconque) partirait sur **gpt-5.5** malgré le GO. Le protocole smoke #202 (grain ⑩ du pool)
héritera de ce fait — son prérequis doit nommer le bump de config (gated GO) ou l'écart assumé.

## §1 Taxonomie des 79 fichiers (classe, pas correction)

| Classe | Exemples | Traitement |
|---|---|---|
| **Config courante** | `DatasetUpdaterRootConfig.cs` (44× 5.5, 4× 5.4) | ⛔ inventorié, **pas touché** — le bump est gated (GO nommé) ; à faire AVANT tout run |
| **Doc vivante de référence** | `CLAUDE.md:510` (Models) et `:546` (smoke « en cours ») | **erratés ce tick** (§2) — en ajoutant l'état de la décision, sans nier l'état de la config |
| **Doc vivante d'outil** | `tools/dnn_i18n/README.md` (12+ mentions, décrit le chemin config #487), `translate_game_rules.py` | inventoriées — décrivent la config **réelle** (5.5) : correctes tant que la config n'est pas bumpée ; erratum à poser **au moment du bump**, pas avant (sinon divergence doc>config dans l'autre sens) |
| **Prompts embarqués** | `DatasetUpdater/Resources/Prompt*CascadeDriftUser.txt`, `PromptScenariiPtRefineUser.txt` | inventoriés — à revoir au bump (contenu du prompt ≠ nom du modèle : vérifier si le nom y est prescriptif) |
| **Audits/rapports datés** (≈70 fichiers) | `docs/taxonomy/141-*`, `192-*`, `499-*`, `docs/investigations/2026-06/07-*`, dossiers `dnn-localization/` | ⛔ historiques — ils décrivent des runs passés sur 5.5/5.4 : les corriger falsifierait l'audit trail |
| **Code entity/test** | `Entities/DnnUiString.cs`, `RulesSectionLexiconTests.cs` | occurrences contextuelles (historique de traduction) — aucune n'est un modèle configurable |

## §2 Erratum CLAUDE.md (posé ce tick)

- **L.510** « **Models**: `gpt-5.5` (EN translations primary, PR #302), … » → devient descriptif
  **daté** + état de la décision : gpt-5.5 = ce que la **config** porte encore (44 occurrences,
  0× 5.6 mesuré le 15/09) ; le modèle de traduction **décidé** est gpt-5.6-sol (GO 14/08), migration
  de config gated.
- **L.546** « smoke test gpt-5.5 en cours (po-2023, cycle 61bis) » → la mention « en cours » datait
  du cycle 61bis ; le protocole smoke est désormais explicité **gpt-5.6-sol** (pool #458 grain ⑩).

## §3 Ce que cet inventaire n'établit pas

⛔ La référence GitHub exacte du GO 14/08 n'est **pas** trouvée dans le dépôt ni les issues sondées
(#458/#202/#994/#192 — 0 hit « 5.6 ») : le GO est **RAPPORTÉ** de la coordination (dashboard/pool),
qualification explicite · ⛔ aucun bump, aucun run, aucune clé manipulée · ⛔ les ~70 audits datés ne
sont pas re-vérifiés un à un (classement par nom de dossier + lecture d'échantillons) · ⛔ le coût du
bump 44+4 occurrences n'est pas chiffré en risque de prompt-drift (les prompts embarqués pourraient
porter des instructions spécifiques au modèle 5.5).

---

## Erratum 2026-09-17 — les 2 réserves de la contre-review #1408 (grain ⑥, pool c.5666260217)

**Auteur** : po-2024 (worker) · **Bases de re-mesure** : `6fbde739` (base d'origine du dossier,
pour rejouer les claims dans leur portée) et `f69029c4` (master du jour). Instruments publiés
ci-dessous. Corps du dossier inchangé : mesure figée du 15/09, corrigée en dessous.

### Réserve ① — « 0× gpt-5.6 dans le dépôt » (titre + §0 ligne 3) : FAUX en portée, corroborant

`gpt-5.6` comptait **2 occurrences dans 1 fichier à la base `6fbde739` elle-même** :
`.claude/skills/coordinate/SKILL.md` l.20 et l.164 (`gpt-5.6-sol`, déjà assigné à la lane
po-2024 comme tier de traduction). MESURÉ : `git grep -n "gpt-5\.6" 6fbde739 -- .claude/skills/`
→ 2 hits. La parenthèse de §0 (`docs/`, `tools/`, `Generation/`, README, CLAUDE.md) excluait
`.claude/` — la claim restait vraie dans cette sous-portée — mais le titre « dans le dépôt » et
le verdict « n'existe nulle part dans le dépôt » étaient faux. **Incidence config nulle** (skill
de coordination, pas un chemin de config). Les 2 occurrences citent le GO 14/08 : la correction
**corrobore** la conclusion §0, elle ne l'affaiblit pas.

### Réserve ② — « 1 590 lignes / 79 fichiers » (§0 ligne 4) : non reproductible, chiffre retiré

Aucun variant `git grep` rejoué sur `6fbde739` ne reproduit 1590/79, et l'instrument d'origine
n'est pas publié dans le dossier. Instruments publiés (MESURÉ le 17/09) :

| Instrument (sur base `6fbde739`) | Résultat |
|---|---|
| `git grep "gpt-5\.5" 6fbde739 \| wc -l` | **366 lignes** |
| `git grep -l "gpt-5\.5" 6fbde739 \| wc -l` | **82 fichiers** |
| `git grep -o "gpt-5\.5" 6fbde739 \| wc -l` | 387 occurrences (= contre-chiffrage #1408) |

Chiffres à retenir : **366 lignes / 82 fichiers / 387 occurrences** à la base d'origine
(à `f69029c4` : 378/85 — croissance post-dossier, les merges #1403/#1405 citent le modèle).
« 1 590 / 79 » est retiré. Aucune conclusion du dossier n'en dépend : la classification §1
est par classe et non par cardinalité, et la claim config (44× 5.5 + 4× 5.4 dans
`DatasetUpdaterRootConfig.cs`) est indépendante et reste vraie.

### Ce que cet erratum n'établit pas

⛔ L'instrument ayant produit « 1590 » n'est pas identifié (SUPPOSÉ : balayage hors-index ou
outil dérivé — non tranché) · ⛔ aucune mutation du corps du dossier (figé, erratum en dessous)
· ⛔ 0 write de config, 0 bump, 0 run — le geste reste gated exactement comme avant.

---
*po-2024 — pool #458 renouvelé (c.5666260217), grain ⑦. Suite naturelle : le protocole smoke #202
(grain ⑩) doit poser le bump de config comme prérequis gated — les deux grains se répondent.*
