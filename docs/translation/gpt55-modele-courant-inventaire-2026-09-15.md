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
*po-2024 — pool #458 renouvelé (c.5666260217), grain ⑦. Suite naturelle : le protocole smoke #202
(grain ⑩) doit poser le bump de config comme prérequis gated — les deux grains se répondent.*
