# Newsletter — la chaîne de rendu, mesurée de bout en bout (2026-10-26, tick 16:0xZ)

**Nature : mesure seule, lecture seule (SQL + HTTP + fichiers de vue). Aucune écriture, aucune soumission de formulaire, `RawData` non lue.**

Machine : `myia-po-2023`. Cible : `https://dnn.argumentum.myia.io` (préprod).

Ce document **corrige les deux précédents du jour** :

| Document | Ce qui est faux | Ce qui reste vrai |
|---|---|---|
| `458-newsletter-form-unserved-translations-2026-10-26.md` (matin) | §3 : « app 33, entité 10176, valeurs non sélectionnées » — 10176 n'a aucun consommateur, et le newsletter n'est pas rendu par l'app 33 | §1 (texte servi identique sur les 8 cultures), §2 (piste app 48 `HtmlFragment`) — **cette piste était la bonne** |
| `458-newsletter-rendered-store-identified-2026-10-26.md` (après-midi) | « le magasin rendu est l'app 50 MobiusForms5 » — c'est le magasin du **formulaire de contact**, pas du newsletter | la mesure sur 10176 (sans consommateur) et l'élimination du drapeau partagé |

⚠️ La 2ᵉ correction a été produite par un **instrument qui regardait la mauvaise table** (§4). C'est consigné ici plutôt que réécrit : un remède qui cite son propre défaut le ré-indexe, mais une erreur de mesure non tracée se rejoue.

---

## 1. La chaîne, maillon par maillon

| # | Fait mesuré | Instrument |
|---|---|---|
| 1 | Module **600** : `TsDynDataApp` = `8b110476-…` → **AppId 35** (Accordion4) ; `TsDynDataContentGroup` = `d0af6d7f-…` → entité **E10171** (app 35) | `ModuleSettings` + `TsDynDataApp` |
| 2 | Le groupe d'E10171 porte 2 contenus : `64243866…` = **E10182** « Contactez-nous » et `5f1d0d48…` = **E10172** « Inscription à la Newsletter » ; template `14f785f2…` = **E7528** app 35, `bs5/_Accordion.cshtml` | `TsDynDataEntity.Json` |
| 3 | La vue accordéon rend chaque item via `foreach (var designedContent in accord.DesignedContent) { @render.One(designedContent); }` | `Accordion4/bs5/_Accordion.cshtml:31-35` |
| 4 | `DesignedContent` est un attribut **`Entity`** — ses valeurs ne sont **pas** dans `TsDynDataValue` (0 rangée pour E10172 et E10182) mais dans **`TsDynDataRelationship`** (2 396 rangées) : `10172 → 10179` et `10182 → 10183` | SQL, `WHERE ParentEntityId IN (…)` |
| 5 | **E10179** = `ContentGroupReference` (app 35), titre « Newsletter », `App` = `8a4ae8c7…` = **app 48**, `ContentGroup` = `3ea2bed2…` = **E10180** (app 48), `Template` = `5cfb54da…` = **E8679** app 48 « Html Snippet » → `_SnippetInject.cshtml` | `TsDynDataEntity.Json` |
| 6 | Contenu d'E10180 = **E10181** (app 48) : `Html` = **13 314 caractères**, commençant par `<!-- Begin Sendinblue Form -->`, **0 rangée dimensionnelle** | SQL |
| 7 | **E10183** = `ContentGroupReference`, titre « Formulaire de contact », `App` = `ea777610…` = **app 50**, `ContentGroup` = `df35c738…` = **E10184**, `Template` = `7d3a9033…` = **E9170** app 50 « Contact Form » → `live/_Contact Form.cshtml` | `TsDynDataEntity.Json` |
| 8 | Servi, bloc `data-accordion-child="10172"` : `data-cb-id='-10179'` puis `<!-- Begin Sendinblue Form -->` — 6 `sendinblue`, 15 `sib-form`, 2 `sib-form-container`, **0** `app-mobius5`, **0** `iframe` | HTML de `/zh/` |
| 9 | Servi, bloc `data-accordion-child="10182"` : `<div class="app-mobius5-wrapper" mobius-600 data-webservice="app/auto/live/api/Form/ProcessForm?workflowId=ContactDefault">` | HTML de `/zh/` |

⇒ **Le newsletter servi sur les 8 cultures est le fragment HTML Sendinblue de l'app 48 (E10181), stocké en français, sans aucune dimension.** Le formulaire Mobius de l'app 50 visible sur la même page est le **formulaire de contact** (item 10182) — une surface distincte, qui a son propre déficit (ses 41 valeurs `Message*`/`Button*`/`Label*` de E9169 sont aussi en dim 0 seule, donc françaises sur 8 cultures).

## 2. Ce que `_SnippetInject.cshtml` implique pour le correctif

La vue fait `@Html.Raw(Content.Html)` — `Content` est l'API dynamique 2sxc qui **résout les dimensions**. Donc des valeurs `Html` en dims 7-13 sur E10181 seraient servies par le mécanisme standard, exactement comme l'injection RuleMemo (app 60) sert 8/8 cultures et comme le type `Link` de l'app 33 sert ses traductions.

Le correctif du newsletter ne demande donc **aucune** structure nouvelle : 1 attribut, 7 valeurs. Le travail réel est éditorial — le fragment contient son `<style>`, ses libellés, son texte de consentement et son bouton ; traduire ces chaînes ne se déduit d'aucune autre traduction du dépôt.

## 3. Deux autres surfaces, à ne pas confondre

| Surface | Magasin | État mesuré |
|---|---|---|
| **Newsletter** (visiteur, accordéon item 10172) | app 48 · E10181 · `Html` | 13 314 c., dim 0 seule → français sur 8 cultures |
| **Formulaire de contact** (accordéon item 10182) | app 50 · E9169 · `App-Resources` | 41 valeurs dim 0 seule → français sur 8 cultures |
| ~~app 33 · E10176~~ | aucun consommateur (0 référence de groupe, `French_CI_AS`) | hors sujet pour les deux |

## 4. Pourquoi la mesure de 15:0xZ s'est trompée (instrument, pas objet)

- Elle a cherché les marqueurs `appMobius5.init` / `app-mobius5-message` / `mobius-600` / `msgSending` dans `TsDynDataValue` et a trouvé **0/0/0/0** — exact, et elle en a conclu « rendu vivant » sans identifier **quel** bloc vivant. Le bloc vivant trouvé (Mobius) était le **contact**, pas le newsletter : deux blocs dans le même module, et l'instrument ne les distinguait pas.
- Elle a cherché `DesignedContent` dans `TsDynDataValue` et a trouvé **0** : un attribut de type `Entity` vit dans `TsDynDataRelationship`. **Un zéro dans la mauvaise table n'est pas une absence.**
- Elle a attribué le module à une app par son `TsDynDataApp` — vrai — puis a supposé que les ressources de cette app alimentaient le contenu — faux : le contenu vient d'une **référence inter-apps** (`ContentGroupReference.App`).

⭐ Règle générale : quand deux blocs se suivent dans le même module, **attribuer par ancre de rendu** (`data-accordion-child`, `data-cb-id`), jamais par « le module contient X ».

## 5. Comment vérifier que ce constat est mort

```sql
-- 1. le magasin newsletter porte-t-il enfin des dimensions ?
SELECT a.StaticName, d.DimensionId, LEN(v.Value)
FROM TsDynDataValue v
JOIN TsDynDataEntity e ON e.EntityId = v.EntityId
JOIN TsDynDataAttribute a ON a.AttributeId = v.AttributeId
JOIN TsDynDataValueDimension d ON d.ValueId = v.ValueId
WHERE e.AppId = 48 AND e.EntityId = 10181;      -- 0 rangée aujourd'hui
```

Et servi : sur `/zh/`, le bloc `data-accordion-child="10172"` doit porter les libellés chinois du formulaire (et non les libellés français actuels). Tant que 0 rangée et libellés FR, le constat tient.

*Instruments : `ModuleSettings`, `TsDynDataApp`, `TsDynDataEntity.Json`, `TsDynDataValue`, `TsDynDataValueDimension`, `TsDynDataRelationship`, `Portals/1/2sxc/Accordion4/bs5/_Accordion.cshtml`, `Portals/1/2sxc/SnippetInject3/_SnippetInject.cshtml`, HTML servi de `/zh/`. Aucune écriture.*
