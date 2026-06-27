# #136 — 2Sxc taxonomy entities + collaborative editing : scoping (état réel, gap, gating)

**Date** : 2026-06-27 · **Auteur** : po-2023 (idle fallback du dispatch ai-01 `msg-jo2hjr`) · **Statut** : Scoping read-only (0 code, 0 `Cards/`). Bornifie #136 pour la revue WE jsboige.
**Objet** : [#136](https://github.com/ArgumentumGames/Argumentum/issues/136) est ouvert depuis longtemps et flou. Ce doc établit, code à l'appui, **ce qui est déjà fait, ce qui manque, et ce qui est gated** — pour que jsboige puisse décider du scope go-live DNN sans relire le code.
**Related** : [#136](https://github.com/ArgumentumGames/Argumentum/issues/136), [2sxc-export-spec.md](release-validation/2sxc-export-spec.md) (pendent DB→repo), [131-2sxc-migration-plan.md](131-2sxc-migration-plan.md), [README.md](README.md) (arc index), #131/#132, #458.

---

## TL;DR

| Partie | Scope #136 | État réel (vérifié `c20d5d2c`) | Gating |
|--------|-----------|-------------------------------|--------|
| **Part 1** — Pipeline `Dnn2sxc` (CSV → entités 2sxc XML) | Fallacies + Virtues + Scenarios + Rules, **8 langues** | **Codé mais limité** : Fallacies **seulement**, **2 langues (FR/EN)**, dépend d'un XML import **2021**, mode **OFF** par défaut | **Non-gated DNN** (code pipeline pur) — mais feature substantiel (×6 langues, ×3 datasets) |
| **Part 2** — Config 2sxc (content-types, import, permissions) | Content-types matchant les entités + rôles (anon/user/editor/admin) | **Non commencé** | **Gated DNN booté + 2sxc 21** |
| **Part 3** — Collaboratif (commentaires, exemples, votes, modération) | Système complet de contribution communauté | **Non commencé** | **Gated DNN + scope produit** (design, pas une tâche worker) |

**Recommandation po-2023** : #136 est un **epic**, pas une tâche. Pour la release v0.9.0 couplée DNN : **out of scope** (le go-live 10.3.2 + 2sxc 21 [#131/#132] ne dépend pas du collaboratif). Subdiviser :
- **#136-A** (Part 1, non-gated, pipeline) : étendre `Dnn2sxcConfig` à 8 langues + Virtues/Scenarios/Rules. Feature code substantiel → **dispatch ai-01**, pas initiative worker.
- **#136-B** (Parts 2/3, gated) : post-go-live DNN, epic produit séparé.

---

## Part 1 — État réel du pipeline `Dnn2sxc` (grounded)

### Ce qui existe ✅

- **Mode câblé** : `ConverterMode.Dnn2sxc = 1 << 3` ([ConverterMode.cs:12](../../Generation/Converters/Argumentum.AssetConverter/ConverterMode.cs#L12)), dispatcher [AssetConverterConfig.cs:498-506](../../Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs#L498).
- **Module complet** [`Dnn2sxc/`](../../Generation/Converters/Argumentum.AssetConverter/Dnn2sxc/) : `Dnn2sxcConfig.cs` (orchestrateur), `Entity.cs`, `SexyContentData.cs` (modèles XML 2sxc).
- **Data** [`Data/Dnn2sxc/`](../../Generation/Converters/Argumentum.AssetConverter/Data/Dnn2sxc/) : CSV source `Argumentum Fallacies - Taxonomy.csv` + XML import 2021 `2sxc Fallacy Data 20210212131131.xml` + export `2sxcContentExport_Argumentum_Fallacies_0.0.1.xml`.
- **Logique `Apply()`** ([Dnn2sxcConfig.cs:128-285](../../Generation/Converters/Argumentum.AssetConverter/Dnn2sxc/Dnn2sxcConfig.cs#L128)) : deserialize l'XML 2sxc existant → charge le CSV Fallacies → reconstruit la hiérarchie path-based (EN-US root + FR-FR translation) → crée/met-à-jour les entités (Title/Desc/Example via expressions interpolées) → serialize vers l'export XML.

### Ce qui manque / est limité ⚠️

1. **2 langues seulement** (FR/EN). `Apply()` hardcode `enUsCulture = "en-US"` + `frFrCulture = "fr-FR"` (l.153-154), expressions `*Fr`/`*En` uniquement. **Incohérent avec v0.9.0 (8 langues : + RU/PT/ES/AR/FA/ZH).** Aucune expression `*Ru`/`*Pt`/`*Es`/`*Ar`/`*Fa`/`*Zh`.
2. **Fallacies seulement**. Le scope #136 demande Virtues + Scenarios + Rules. `Apply()` ne charge que `CsvPathFallacies` (l.24, 147). Pas de `CsvPathVirtues`/`Scenarios`/`Rules`.
3. **XML import 2021** (`2sxc Fallacy Data 20210212131131.xml`). Structure 2sxc **pré-2sxc-21**. Compatibilité avec 2sxc 21 LTS (target #131) **inconnue / non testée** — risque que le schéma `SexyContentData`/`Entity` ait évolué côté 2sxc.
4. **Mode OFF par défaut**. `Mode = WebBasedImageGeneration | QuestPdfGeneration` ([AssetConverterConfig.cs:37](../../Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs#L37)). `Dnn2sxc` n'est jamais exécuté dans un run standard → le code n'a **pas tourné depuis 2021**, fragilité silencieuse possible (APIs Fallacy/CsvHelper ont bougé depuis).
5. **Export marqué version `0.0.1`** (pré-recovery, pré-Golden-Master `0087f0ec`).

### Pour aligner Part 1 sur v0.9.0 (chemin non-gated)

Travail code pipeline pur (pas de DNN, pas de `Cards/`) :
- Ajouter 6 paires d'expressions `*Ru`/`*Pt`/`*Es`/`*Ar`/`*Fa`/`*Zh` (Title/Desc/Example/Link) + leurs `*Func`.
- Généraliser `Apply()` sur la liste des cultures actives (boucle plutôt que hardcode 2).
- Ajouter Virtues/Scenarios/Rules : `CsvPath*` + `Apply*()` par dataset (ou refactor générique).
- **Prérequis validation** : faire tourner `Mode |= Dnn2sxc` une fois sur les CSV actuels (8 langues, 1408 fallacies) pour confirmer que `Apply()` n'a pas régressé silencieusement depuis 2021. C'est un **run local non-committé** (l'export XML va dans `Data/Dnn2sxc/`).

**Effort estimé** : feature substantiel (pas un fix). Mérite **dispatch ai-01 + PR dédiée**, pas initiative worker.

---

## Parts 2 & 3 — Gated DNN boot + scope produit

Rien n'existe côté code repo (légitimement : c'est de la config/runtime DNN).

- **Part 2** (content-types 2sxc, import, rôles/permissions) : nécessite DNN booté + 2sxc 21 = **gated session RDP jsboige** (B1→B4 du [go-live-turnkey-checklist](../dnn/go-live-turnkey-checklist.md)). L'import des entités Part 1 dans 2sxc se fait via l'UI/SQL 2sxc (cf. [2sxc-export-spec.md §3](release-validation/2sxc-export-spec.md)).
- **Part 3** (commentaires, exemples communauté, votes, modération) : **scope produit**, pas une tâche technique worker. Design UX + choix techno (2sxc comments natif ? module DNN tiers ? custom ?) + permissions. À cadrer par jsboige comme un epic post-go-live.

---

## Recommandation

1. **#136 ≠ blocker v0.9.0-DNN**. La release couplée (#134 + #131/#132) porte sur le go-live 10.3.2 + 2sxc 21 + 12 templates Razor14 + smoke. Le collaboratif communautaire est **post-go-live**.
2. **Subdiviser #136** :
   - **#136-A** (Part 1 pipeline, non-gated) : à dispatcher à un worker (ai-01 arbitre). Borné ci-dessus (8 langues × 4 datasets + run validation).
   - **#136-B** (Parts 2/3, gated + produit) : epic séparé, post-go-live DNN.
3. **Ne pas activer `Mode.Dnn2sxc` maintenant** : le XML 2021 + le gap 8-langues rendraient l'export incohérent avec v0.9.0. Attendre #136-A.

---

## Relation à l'arc existant

- [2sxc-export-spec.md](release-validation/2sxc-export-spec.md) = pendent **DB→repo** (export contenu 2sxc pour localisation #490). Ce doc = pendent **repo→2sxc** (génération entités). Les deux sont complémentaires, non dupliqués.
- [131-2sxc-migration-plan.md](131-2sxc-migration-plan.md) = migration 2sxc 15.02 → 21.07 LTS (runtime). #136-A doit être **validé contre 2sxc 21** post-migration (le XML 2021 pré-21 peut casser).
