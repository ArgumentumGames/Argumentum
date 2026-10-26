# Newsletter — le magasin réellement rendu est l'app 50 MobiusForms5 (mesure du 2026-10-26, tick 15:0xZ)

**Nature : mesure seule, lecture seule (SQL + HTTP + fichiers). Aucune écriture, aucune soumission de formulaire, `RawData` non lue.**
Machine : `myia-po-2023`. Cible : `https://dnn.argumentum.myia.io` (préprod). Suite de [`458-newsletter-form-unserved-translations-2026-10-26.md`](458-newsletter-form-unserved-translations-2026-10-26.md) — **ce document corrige son attribution** (§3) et clôt ses trois candidats ouverts (§5).

---

## 1. La chaîne d'évidence (chaque maillon mesuré)

| # | Fait | Instrument |
|---|---|---|
| 1 | Le bloc servi porte `app-mobius5-message` et `<turnOn turn-on='{"run":"window.appMobius5.init()","data":{"domAttribute":"mobius-600"}}'>` | HTML brut de `/` |
| 2 | Le formulaire est rendu par la vue `MobiusForms5/live/shared/_Submit with Messages.cshtml`, qui lit **chaque** chaîne dans `Resources.*` (`MessageSending`, `MessageNewsletterSuccess`, `ButtonSend`, `MessageSendOk`…) | lecture du fichier |
| 3 | `Resources.*` = type `App-Resources` de l'**app 50** : **4 entités** (11979, 9169, 11973, 11961) ; tous les `Message*`/`Button*`/`Label*` (25 attributs) existent **en dim 0 seule** — aucune valeur dimensionnelle 7-13 | SQL lecture seule |
| 4 | L'entité 10176 (app 33) n'est référencée par **aucun** JSON d'entité (`LIKE '%44088360-…%'` = 0 ; collation `French_CI_AS` **insensible à la casse** ⇒ le zéro est vrai, pas un artefact de casse) | SQL lecture seule |
| 5 | Le module « Newsletter » M598 (groupe e10168, même vue « Text only » que M587) rend l'entité **10169** : un `Text` = iframe Brevo **447 c, sha identique sur dim 0 + dims 7-13** (identique par conception) + `Title` localisé | SQL + JSON du groupe |

## 2. Ce que cela réfute — les trois candidats de §5 du document précédent

1. **« Le magasin réellement rendu »** : c'est l'**app 50 MobiusForms5** (via le module 600 / accordéon app 35, entité 10172 pour les en-têtes). Pas l'app 33 (aucune entité rendue ne porte le texte du formulaire), pas l'app 48 (`HtmlFragment` = encore une copie). L'attribution initiale de l'Item 8 (tier3 §2 → app 48) **et** celle du document précédent (« app 33, entité 10176, valeurs non sélectionnées ») sont toutes deux fausses — la 2ᵉ par le **piège d'égalité de texte** : le `Text` de 10176 (5 763 c) est une *copie locale du markup du formulaire* ; ses 7/7 sondes verbatim prouvaient la copie, pas le rendu. **Troisième instance de la famille** « une égalité de texte visible ne distingue pas deux magasins » (app 48, app 33 base, app 33 copie).
2. **« Drapeau partagé entre langues »** : éliminé. `SysSettings` et `Json` de `TsDynDataContentType` sont **NULL sur tous les types** des apps 33/52/60 — y compris ceux qui servent leurs traductions (`Link` app 33, `ViewConfiguration` app 52, `App-Resources` app 60). Une valeur uniforme ne peut pas expliquer une différence.
3. **« Écrites par duplication DNN vs traduction 2sxc »** : **sans objet** — 10176 n'a **aucun chemin de rendu** (maillon 4). Ses 14 valeurs localisées ne sont pas « non sélectionnées », elles sont **sans consommateur** (même frontière que License/Link/Album : injection prouvée à la couche données ≠ visibilité servie).

## 3. La résolution dimensionnelle de la zone 3 fonctionne partout où elle a un consommateur

Mesuré servi ce jour : le bloc du module 587 (entité 10065, type `Content`, app 33 — « Retrouvez-nous ») rend **ses traductions** — `/` « Retrouvez-nous », `/zh/` «找到我们 », `/en/` « Find us ». Donc **type `Content` app 33 inclus**. Le français uniforme du formulaire n'est un défaut de sélection nulle part : c'est une **absence de traductions dans le magasin rendu** (app 50, dim 0 seule).

## 4. Le chemin de correctif (aucun geste posé)

Injecter des valeurs dims 7-13 sur les attributs `Message*`/`Button*` de l'App-Resources de l'app 50 — **exactement le motif RuleMemo** (app 60 `App-Resources`, injection du 26/10, servie traduite 8/8 cultures). À noter :

- **~20 clés × 7 langues** à traduire (les clés visibles du formulaire newsletter : `MessageSending`, `MessageNewsletterSuccess`, `MessageNewsletterUnexpectedError`, `MessageSendOk`, `MessageSendError`, `ButtonSend`, + libellés si le formulaire les expose) — les traductions n'existent **pas** dans le dépôt ; les 14 valeurs de 10176 sont des copies HTML de formulaire, **pas** réutilisables clé par clé (elles peuvent informer le travail, pas le remplacer).
- L'**iframe Brevo** (10169) est identique sur les 8 langues **par conception** : son contenu interne est rendu par Brevo (distant). Localiser la base ne changera pas l'intérieur de l'iframe — frontière à traiter séparément si besoin (formulaires par langue côté Brevo).
- Les 4 entités App-Resources de l'app 50 correspondent vraisemblablement à autant de formulaires ; à cadrer au geste (laquelle alimente le module 600).
- Gate : GO ai-01 + traductions validées. Le « whitelist app 48/50 » du pool se réduit à **app 50 seul** (l'app 48 est définitivement hors cause).

## 5. Comment vérifier que ce constat est mort

```sql
-- le magasin rendu porte-t-il enfin des dims ?
SELECT a.StaticName, d.DimensionId FROM TsDynDataValue v
JOIN TsDynDataEntity e ON e.EntityId = v.EntityId
JOIN TsDynDataAttribute a ON a.AttributeId = v.AttributeId
JOIN TsDynDataValueDimension d ON d.ValueId = v.ValueId
WHERE e.AppId = 50 AND e.EntityId IN (11979, 9169, 11973, 11961);
-- 0 rangée aujourd'hui ; > 0 après le geste de traduction.
```

Et servi : `/zh/` doit rendre `MessageSending` zh (et non « Message en cours d'envoi, patientez... ») dans le bloc `#msgSending`.

*Instruments : SQL lecture seule via `web.config` (jamais imprimé), HTTP sans cache, lecture de `MobiusForms5/live/shared/_Submit with Messages.cshtml`, `ModuleSettings`, `TsDynDataEntity.Json`, `TsDynDataContentType.SysSettings`. Aucune écriture.*
