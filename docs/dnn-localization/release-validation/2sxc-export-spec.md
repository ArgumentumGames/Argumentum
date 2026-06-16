# Spec — Export 2sxc pour vérifier le FR inféré & débloquer #490

**Auteur :** Claude Code @ myia-po-2023 (worker) — 2026-06-16
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

| # | Ce qu'on exporte | Source 2sxc/DNN | But | Priorité |
|---|------------------|-----------------|-----|----------|
| 1 | **App Resources** (dictionnaire valeurs FR) | App Argumentum → Content-type `Resources` | Vérifier les 7 FR inférés `res.*` | **P0 — débloque #490** |
| 2 | **Rules content** (Summary/Material/Installation/Variants/Memo) | Content-items Rules | FR canonique des corps de règles (≈24 règles × 5 champs) | P1 |
| 3 | **Glossary entries** | App `Glossary3` | ≈50 entrées glossaire | P2 |
| 4 | **FAQ entries** | App `Faq4` | FAQ | P2 |
| 5 | **Homepage/About/landing** | App `Content` + modules | ≈10 pages | P2 |
| 6 | **Nav labels** | DNN tabs | Labels menu | P3 |
| 7 | **SEO meta** (titres/descriptions) | DNN page settings | ≈40 pages | P3 |

> **Déjà localisé, pas d'export requis :** Fallacies Explorer lit le CSV taxonomy via
> `App.Query["FallaciesFromCSV"]` → le contenu des fallacies est déjà couvert par les CSV cartes 8
> langues. (Caveat : le template pin `_en` — bug §4, à corriger en code.)

## 3. Méthode A — UI 2sxc (recommandé, non destructif)

1. Se connecter au portail DNN (local `http://localhost:8090` ou prod) en admin.
2. **2sxc → App Argumentum → Data** (ou *Content*).
3. Sélectionner le content-type **`Resources`** → liste des entrées key/value.
4. Export : bouton **Export** (JSON ou CSV). 2sxc génère un fichier avec toutes les paires key/value
   (toutes langues présentes en DB).
5. Répéter pour `Rules`, `Glossary3`, `Faq4`, `Content` selon le périmètre §2.

**Pour les DNN tabs (nav) et page settings (SEO)** : DNN admin → *Pages* → export, ou SQL (§4).

## 4. Méthode B — SQL direct (si l'UI 2sxc est indisponible)

2sxc stocke les entités dans `dbo.[ToSIC_SexyContent_<App>_<ContentType>_Entity]` + valeurs dans des
tables `*_Value` / attributs EAV. La structure exacte varie ; requête de départ (adapter) :

```sql
-- Lister les ressources FR de l'app Argumentum (vérifier le schéma EAV réel au préalable)
SELECT  e.EntityId, e.Guid, a.Name AS [Key], v.Value
FROM    ToSIC_SexyContent_AttributeValues v
JOIN    ToSIC_SexyContent_Attributes a       ON a.AttributeId = v.AttributeId
JOIN    ToSIC_SexyContent_Entities e         ON e.EntityId = v.EntityId
JOIN    ToSIC_SexyContent_AttributeSets s    ON s.AttributeSetId = a.AttributeSetId
WHERE   s.Name = 'Resources'                 -- content-type Resources de l'app Argumentum
  AND   v.Language = 'fr-FR'                 -- (ou NULL si mono-langue)
ORDER BY a.Name;
```

⚠️ **Le schéma EAV de 2sxc dépend de la version** (15.02 ici). Vérifier les noms de tables réels
(`SELECT name FROM sys.tables WHERE name LIKE 'ToSIC_SexyContent%'`) avant la jointure. L'UI (§3) est
plus sûre si disponible.

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
- **Version 2sxc** : 15.02. L'UI d'export peut différer des captures génériques — s'adapter.

---

*Cet export est l'action jsboige qui débloque #490 et ouvre les Phases 2–3. Worker signale ; jsboige
exécute l'accès admin. Gate release intacte.*
