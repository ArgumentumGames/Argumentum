# Spec — Export 2sxc pour vérifier le FR inféré & débloquer #490

> **⚠ SUPERSEDED (partiellement) — 2026-07-11.** Cette spec a été écrite pour **2sxc 15.02**
> (`ToSIC_SexyContent_*`, app=31, FR « inféré »). Le site live tourne en **2sxc v21**
> (`ToSIC_EAV_*`, **app=60**), et l'export a été **livré** (#681 → PR #774 merged, 7 fichiers sous
> [`exports/DNN-Argumentum-export-2026-07-07/`](exports/DNN-Argumentum-export-2026-07-07/)). Les
> objectifs P0/P1 de cette spec sont **accomplis** : les `res.*` FR sont **VERIFIED** (plus inférés),
> le HOLD #490 est **levé** (4 arbitrages `res.Rule*` exécutés, PR #772). Les sections §3/§4/§7 sont
> **corrigées ci-dessous** pour refléter v21 ; §5/§6 restent valides comme patron de ré-ingestion.
> Source de vérité actuelle = le manifest live [`exports/.../manifest.json`](exports/DNN-Argumentum-export-2026-07-07/manifest.json)
> + [`682-field-model-revision-2sxc21.md`](../682-field-model-revision-2sxc21.md).

**Auteur :** Claude Code @ myia-po-2023 (worker) — 2026-06-16 (corrigé 2026-07-11 vs export live #774)
**Objet :** Spécification de l'export 2sxc qui (1) vérifie les **7 FR inférés** `res.*` de
`dnn-ui-strings.csv` vs le dictionnaire Resources live, et (2) extrait le contenu DB-only
(Rules/Glossary/FAQ/homepage) pour la localisation complète. C'est le **vrai unblocker** de #490 et
des Phases 2–3 de #457.
**Accès requis :** jsboige (admin DNN/2sxc). Outil : UI 2sxc **ou** SQL direct sur la base du portail.

---

## 1. Pourquoi cet export

`PHASE1-content-audit.md` §1b établit : les 8 clés `res.*` sont **présentes** dans les templates
(`@Resources.X`) mais leurs **valeurs vivent en SQL** (dictionnaire Resources de l'app Argumentum) —
le repo n'a pas d'`App_Data/`. Les 7 valeurs FR de `dnn-ui-strings.csv` sont donc **inférées** du
contexte `.cshtml`. jsboige a **holdé #490** jusqu'à vérification de ce FR vs le DB live.

Tant que ces 7 FR ne sont pas confirmés, les 7×7 = 49 traductions dépendantes sont **provisoires**.

## 2. Périmètre de l'export

| # | Ce qu'on exporte | Source 2sxc/DNN | But | Priorité | Statut (2026-07-11) |
|---|------------------|-----------------|-----|----------|--------------------|
| 1 | **App Resources** (dictionnaire valeurs FR) | app=60 → eid=10340 | Vérifier les `res.*` FR | **P0** | ✅ **DONE** — 9 `res.Rule*` VERIFIED (#772), FR=!inféré |
| 2 | **Rules content** (Summary/Material/Installation/Content/Variants/Memo) | app=60 `Game Rule` (id=377) | FR canonique des corps de règles | P1 | ✅ **DONE** — 5 entités (pas 24-30), export #774 `12-...` |
| 3 | **Glossary entries** | App `Glossary3` | ≈50 entrées glossaire | P2 | ❌ **NOT FOUND** — `Glossary3` absent ; candidats app=60 : Fallacy/Scenario/Comment. Mapping à clarifier. |
| 4 | **FAQ entries** | App `Faq4` | FAQ | P2 | ❌ **NOT FOUND** — `Faq4` absent (cf manifest `findings.glossary3Faq4`). |
| 5 | **Homepage/About/landing** | App `Content` + modules | ≈10 pages | P2 | ⏸ non couvert par l'export #774 (scope résolu P0/P1) |
| 6 | **Nav labels** | DNN tabs | Labels menu | P3 | ⏸ non couvert |
| 7 | **SEO meta** (titres/descriptions) | DNN page settings | ≈40 pages | P3 | ⏸ non couvert |

> **Déjà localisé, pas d'export requis :** Fallacies Explorer lit le CSV taxonomy via
> `App.Query["FallaciesFromCSV"]` → le contenu des fallacies est déjà couvert par les CSV cartes 8
> langues. (✅ Le template est culture-aware depuis PR #464 — bug §4 résolu, pas d'export requis pour l'Explorer.)

## 3. Méthode A — UI 2sxc (recommandé, non destructif)

> **Corrigé 2026-07-11 vs live v21 (export #774).** L'app Argumentum est **app=60** (ZoneID=3), le
> content-type Rules s'appelle **`Game Rule`** (AttributeSet 377), et les `res.*` vivent sur l'entité
> **App-Resource eid=10340**. Les noms `Rules`/`Glossary3`/`Faq4` ci-dessous étaient des hypothèses
> 15.02 — `Glossary3`/`Faq4` sont **absents** du live (cf §2 statut + manifest `findings`).

1. Se connecter au portail DNN (local `http://localhost:8090` ou prod) en admin.
2. **2sxc → App 60 (Argumentum) → Data** (ou *Content*).
3. Pour les `res.*` : entité App-Resource **eid=10340** (pas un content-type ordinaire). Pour les
   corps de règles : content-type **`Game Rule`** (AttributeSet 377) → 5 entités publiées.
4. Export : bouton **Export** (JSON ou CSV). 2sxc génère un fichier avec toutes les paires key/value
   (toutes langues présentes en DB).
5. Répéter selon le périmètre §2 (app=60 : `Game Rule` ✓ ; `Glossary3`/`Faq4` absents — à clarifier).

**Pour les DNN tabs (nav) et page settings (SEO)** : DNN admin → *Pages* → export, ou SQL (§4).

## 4. Méthode B — SQL direct (si l'UI 2sxc est indisponible)

> **Corrigé 2026-07-11 vs live v21 (export #774).** 2sxc v21 stocke l'EAV dans la famille
> **`ToSIC_EAV_*`** (`ToSIC_EAV_Values` / `ToSIC_EAV_Attributes` / `ToSIC_EAV_Entities` /
> `ToSIC_EAV_AttributeSets`), **PAS** `ToSIC_SexyContent_*` (qui était la famille 15.02 de la spec
> originale). Le FR canonique est la **valeur dimensionless** (`DimensionID IS NULL`), pas une
> colonne `v.Language = 'fr-FR'`. L'export #681/#774 a été produit exactement par cette méthode
> (read-only, on-box `myia-web1`).

```sql
-- Lister les ressources FR (dimensionless default) de l'app Argumentum, app=60, eid=10340.
-- Schéma VERIFIE sur le live v21 (cf export #774 / 13-app60-resources.json).
SELECT  e.EntityId, e.Guid, a.Name AS [Key], v.Value
FROM    ToSIC_EAV_Values v
JOIN    ToSIC_EAV_Attributes a   ON a.AttributeId = v.AttributeId
JOIN    ToSIC_EAV_Entities e     ON e.EntityId = v.EntityId
WHERE   e.EntityId = 10340           -- App-Resource entity (res.* rail), app=60
  AND   v.DimensionID IS NULL        -- FR dimensionless default
ORDER BY a.Name;
```

⚠️ **Le schéma EAV de 2sxc dépend de la version** : **v21 = `ToSIC_EAV_*`** (correct ici), 15.02 et
antérieur = `ToSIC_SexyContent_*`. Vérifier `SELECT name FROM sys.tables WHERE name LIKE 'ToSIC%'`
avant la jointure. L'UI (§3) reste plus sûre si disponible.

## 5. Comparaison & action (comment lever le HOLD #490)

1. **Mapper** les 7 clés `res.*` de `dnn-ui-strings.csv` → valeurs FR de l'export.
2. Pour chaque clé :

   | Cas | Action |
   |-----|--------|
   | FR exporté = FR inféré | ✅ Confirmé — la ligne est fiable |
   | FR exporté ≠ FR inféré | ⚠️ Corriger la cellule `fr` dans `dnn-ui-strings.csv`, **puis re-run #457** (OpenAI direct, clé rechargée) pour régénérer les 7 traductions de cette ligne |
   | Clé absente de l'export | La ressource n'existe peut-être plus → à clarifier avec jsboige |

3. **`res.RuleMemoInstructions`** : extraire la **valeur FR multi-phrases** (DB-only, non inférée).
   La remplir dans `dnn-ui-strings.csv` (colonne `fr`), puis re-run #457 → produit les 7 langues.
4. Une fois les 7 (8 avec MemoInstructions) FR confirmés → **lever le HOLD #490** et valider les
   langues (voir `non-latin-verification-guide.md`).

## 6. Format d'export attendu (pour ré-ingestion #457)

Pour que le contenu DB-only (Rules/Glossary/FAQ, périmètre #2-5) soit **traduisible par le
DatasetUpdater**, l'export doit atterrir dans un CSV au schéma `dnn-ui-strings.csv` :
`key, context, source_file, fr, en, ru, pt, es, ar, fa, zh, notes` — une ligne par chaîne, `fr`
rempli, cibles vides. Le DatasetUpdater (OpenAI direct gpt-5.5, clé rechargée) fait ensuite le reste.

## 7. Risques / précautions

- **Non destructif** : export = lecture seule. Aucun write sur le portail.
- **Ne pas modifier les CSV avant injection CardPen** (règle projet) — N/A ici (pas de CardPen), mais
  l'export reste une source de vérité ; ne pas éditer à la main les cellules traduites.
- **Version 2sxc** : **v21** (corrigé 2026-07-11 ; la spec originale mentionnait 15.02). Famille EAV
  `ToSIC_EAV_*`. L'UI d'export peut différer des captures génériques — s'adapter.

---

*Cet export est l'action jsboige qui débloque #490 et ouvre les Phases 2–3. Worker signale ; jsboige
exécute l'accès admin. Gate release intacte.*
