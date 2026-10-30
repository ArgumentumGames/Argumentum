# Surface de données 2sxc mesurée — préprod `dnn.argumentum.myia.io`

**Date :** 2026-10-30 · **Lane :** po-2023 · **Portée :** lecture seule (aucune écriture, aucun import)
**Contexte :** tête de file du pool #458, item ② — « importer le contenu Rules dans 2sxc ».

Ce rapport remplace plusieurs conclusions antérieures **fausses**, listées en §5 avec leur
cause instrumentale. Il ne conclut pas que l'import est fait : il établit **ce que la base
contient réellement**, ce que l'import devait produire, et où.

---

## 1. Cartographie (VÉRIFIÉ — requêtes SQL directes sur la base servie)

| Objet | Valeur mesurée |
|---|---|
| Zone du portail Argumentum | **`ZoneId = 3`** — `Argumentum (Portal 1)` |
| Autres zones | `1` = Default · `2` = `Argumentum Games (Portal 0)` |
| App principale | **`AppId = 60`**, GUID `bbc87873-645d-4c08-8a6b-418086567554` |
| App des fragments HTML | `AppId = 48` (`HtmlFragment`, `Script`, `Style`) |
| App du formulaire de contact | `AppId = 50` (`ContactSimple`, `GenericForm`, …) — cf. PR #2052 |
| Content-type des règles | **`ContentTypeId = 377`**, `StaticName = a9d3420a-3644-4be1-8df1-12c27ced8c19`, nom **`Game Rule`** |
| Content-type des sophismes | `ContentTypeId = 376`, nom `Fallacy` |
| Magasin EAV actif | tables **`TsDynData*`** (14 tables) — pas `ToSIC_EAV_*` (une seule table legacy `ToSIC_EAV_Attachments`) |

**Contrôle croisé indépendant** : l'app **48** est bien celle qui porte le fragment newsletter
`E10181` (mémoire `project-dnn-lane-oct2026`). La cartographie AppId → application est donc
ancrée sur deux mesures séparées, pas sur une seule lecture.

### Dimensions de langue (VÉRIFIÉ)

`TsDynDataDimension`, zone 3 :

| DimensionId | Culture | Rôle |
|---|---|---|
| 5 | `Culture Root` (SystemKey `Culture`) | **base / défaut** |
| 6 | `fr-FR` | français |
| 7 | `en-US` | anglais |
| 8 | `ru-RU` | russe |
| 9 | `pt-BR` | portugais |
| 10 | `es-ES` | espagnol |
| 11 | `ar-SA` | arabe |
| 12 | `fa-IR` | persan |
| 13 | `zh-CN` | chinois |

**Les 8 langues du projet sont présentes, exactement, sans surplus.**

⚠️ **Le français n'utilise pas sa dimension (6)** : il est servi depuis la **valeur de base**,
c'est-à-dire les lignes de `TsDynDataValue` **sans ligne** dans `TsDynDataValueDimension`.
Conséquence pour tout instrument : une requête qui joint `TsDynDataValue` à
`TsDynDataValueDimension` en `INNER JOIN` **perd silencieusement tout le français**. Ce
piège est la forme données de la mémoire `feedback-eav-base-value-has-no-dimension-row`.

## 2. Contenu `Game Rule` réellement présent (VÉRIFIÉ)

**5 entités**, toutes publiées côté lecture :

| EntityId | Titre (français, valeur de base) |
|---:|---|
| 11378 | L'école des menteurs |
| 11380 | Le Bingo mixologie argumentative |
| 11387 | Le dernier beau parleur |
| 11388 | Le moulin à baratin |
| 11389 | La parlote coinchée |

**Couverture linguistique — attribut `Title` (`AttributeId = 1933`) : 5 valeurs par langue,
soit 5/5 dans les 8 langues, 40 valeurs au total.**

```
Title  BASE(sans ligne) : 5
Title  dim=7  (en-US)   : 5
Title  dim=8  (ru-RU)   : 5
Title  dim=9  (pt-BR)   : 5
Title  dim=10 (es-ES)   : 5
Title  dim=11 (ar-SA)   : 5
Title  dim=12 (fa-IR)   : 5
Title  dim=13 (zh-CN)   : 5
```

Toutes les valeurs sont **remplies et distinctes** : ce sont de vraies traductions, pas des
reports du français. Exemple mesuré sur `EntityId 11378` :

| Langue | `Title` |
|---|---|
| fr (base) | L'école des menteurs |
| en | The school of liars |
| es | La escuela de los mentirosos |
| pt | A escola dos mentirosos |
| ru | Школа лжецов |
| zh | 说谎者学校 |
| ar | مدرسة الكاذبين |
| fa | مدرسهٔ دروغ‌پردازان |

### ⚠️ Ce que la mesure ne dit PAS

Le total de valeurs par entité est de **52 en base contre 28 par langue**. L'écart n'est
**pas** une lacune de traduction : une partie des 15 attributs de `Game Rule` est
**non traduisible par nature** — `Parent` (Entity), `MinNbPlayers` / `MaxNbPlayers`
(Number), `Date` (DateTime), `Author` / `Licence` / `Original` (Entity), `UrlKey` (String
technique). Attribuer l'écart 52−28 à un défaut de traduction serait une conclusion
**calculée** sans mesure, exactement le motif `feedback-instrument-computed-conclusions`.

⇒ **Action de suite, non faite ici :** un audit **par attribut** (lesquels sont traduisibles,
lesquels couvrent les 5 entités dans les 8 langues) est le seul instrument qui puisse
prononcer une couverture. Il n'a pas été exécuté dans ce cycle.

## 3. Modèle d'écriture EAV (VÉRIFIÉ — schéma)

```
TsDynDataEntity     (EntityId, EntityGuid, ContentTypeId, AppId, IsPublished, KeyNumber, …)
  └─ TsDynDataValue (ValueId, EntityId, AttributeId, Value nvarchar)
       └─ TsDynDataValueDimension (ValueId, DimensionId, ReadOnly)
```

- `TsDynDataAttribute` : `AttributeId, StaticName, Type, Guid, ContentTypeId, SortOrder, IsTitle`.
  ⚠️ **La colonne de rattachement est `ContentTypeId`, pas `AttributeSetId`** (nommage EAV
  historique) — une requête sur `AttributeSetId` échoue.
- Une valeur **de base** (culture root) = ligne `TsDynDataValue` **sans** ligne dans
  `TsDynDataValueDimension`.
- Une valeur **traduite** = même `AttributeId`, `EntityId` identique, **+ une ligne**
  `TsDynDataValueDimension` vers `DimensionId` 6–13.

Volumétrie de la table de dimensions sur la zone 3 : **207 lignes pour chacune des
dimensions 7→13** (symétrie parfaite des 7 langues non-françaises), 0 ligne pour la
dimension 6 — cohérent avec le français servi depuis la base.

## 4. Volumétrie générale (VÉRIFIÉ)

| Table | Lignes |
|---|---:|
| `TsDynDataEntity` | 11 541 |
| `TsDynDataValue` | 15 823 |
| `TsDynDataContentType` | 371 |
| `TsDynDataAttribute` | 1 893 |
| `TsDynDataValueDimension` | 6 630 |

`Fallacy` (ct 376) : **1 013 entités**. `Scenario` (ct 380) et `Scenario Category` (ct 381) :
**0 entité** — le contenu des scénarios n'est pas dans 2sxc, contrairement aux sophismes.

## 5. Conclusions antérieures réfutées (et leur cause instrumentale)

| Croyance | Statut | Cause réelle |
|---|---|---|
| « La surface d'admin 2sxc est absente » | **FAUX** | La route est vivante : `admin/zone/*` rend **401** là où un chemin inexistant rend un **404 IIS XHTML**. Sélection d'action **avant** le filtre d'autorisation (prouvé par le couple 401/404 sur `admin/app/*`). |
| « L'API renvoie une boucle 302 » | **FAUX** | Artefact d'un ticket `.DOTNETNUKE` **périmé** exporté. Isolation : sans cookie → 200 en 1 saut · `.DOTNETNUKE` seul → 302 ×5 · tous **sauf** `.DOTNETNUKE` → 200 en 1 saut. |
| « L'identité n'est pas reconnue par le site » | **FAUX** | Avec session fraîche : `/` = 200 / 167 367 octets (`Logoff` ×2), `/Admin` = **200 / 75 018**, `ctl=Profile` = 200 / 170 212. |
| « `dbo.Modules` a 0 ligne pour les defs 139/140 » | **FAUX** | Échec d'instrument. La colonne **`Modules.TabID` n'existe pas** en DNN 10 (le placement vit dans `dbo.TabModules`). Re-mesuré : **97 placements**, **0 orphelin** sur 161. |
| « Aucun module 2sxc sur aucun onglet » | **FAUX** | Même cause. Les modules 2sxc (defs 139 = `2sxc`, 140 = `2sxc-app`) sont placés sur des onglets. |

## 6. Ce qui reste ouvert

1. **`admin/zone/*` rend 401 même authentifié, sans en-tête `WWW-Authenticate`.** Deux
   hypothèses restent à départager — aucune n'est tranchée ici :
   - (a) le pipeline Web API ne porte pas l'identité Forms (gate **structurel**) ;
   - (b) il manque un **contexte zone/app** dans l'appel.
   Le test de (b) par `?zoneId=1` / `?zoneId=2` a échoué (401 persiste) ; le test par
   `tabId`/`moduleId` réels **n'a pas été exécuté**.
2. **Audit de couverture par attribut** sur `Game Rule` (§2) — le seul instrument qui puisse
   prononcer si les 8 langues sont complètes.
3. Les **16 règles** du CSV `Cards/Rules/` ne se projettent pas 1:1 sur les **5 entités**
   `Game Rule` : la correspondance entre les deux référentiels n'est pas établie.

**Aucun geste d'écriture n'a été effectué.** Aucun import, aucune injection EAV, aucune
modification de contenu.
