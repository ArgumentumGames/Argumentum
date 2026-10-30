# Item ② — état réel de l'import des Rules dans 2sxc, et état de la porte

**Date :** 2026-10-30 · **Lane :** po-2023 · **Portée :** lecture seule (SQL + DLL sur disque + 1 connexion authentifiée, aucune écriture)
**Complète** : [2026-10-30-sxc-data-surface-measured.md](2026-10-30-sxc-data-surface-measured.md) (PR #2073)

> **Correction de la formulation de #2073.** Ce rapport y annonçait « le contenu Rules est déjà
> dans 2sxc, en 8 langues ». C'est **inexact par confusion de granularité** : ce qui est dans la
> base, c'est le contenu **au niveau du jeu** (`Game Rule`, 5 entités). Ce que le runbook veut
> importer, c'est le contenu **au niveau de la ligne CSV** (type `Rule`, 16 entités par langue).
> Les deux ne sont pas la même chose, et le second **n'est pas** dans la base.

---

## 1. Le payload est prêt, et son DoD est déjà automatisé (VÉRIFIÉ)

`Generation/Converters/Argumentum.AssetConverter/Data/Dnn2sxc/2sxcContentExport_Argumentum_Rules_0.0.1.xml`

| Mesure | Valeur |
|---|---|
| Racine | `SexyContentData` |
| Entités | **128** = `Type="Rule"` × 16 × 8 cultures |
| Répartition | `en-US` 16 · `fr-FR` 16 · `ru-RU` 16 · `pt-BR` 16 · `es-ES` 16 · `ar-SA` 16 · `fa-IR` 16 · `zh-CN` 16 |
| Champs | `Guid`, `Language`, `Name`, `Description`, `Example`, `Parent` |
| GUID | **identiques entre langues** (une entité logique × 8 cultures) |
| `Parent` | tous pointent un `Guid` **présent dans l'export** |

**Les « 16 règles » du runbook sont un décompte d'entités, pas de règles.** Par culture, l'export
contient la racine `Rules` + `Rules_01`…`Rules_15`. Le CSV source
(`Cards/Rules/Argumentum Rules - Cards.csv`) compte **15 lignes**, `Rules_01` → `Rules_15`,
continues, sans trou — et la config du générateur le dit mot pour mot
(`Dnn2sxcConfig.cs:256`) : *« Rules — 15 rangées PLATES (aucune hiérarchie : pas de colonne path),
texte intégral en markdown par langue. »* Projection : **`Pk` → `Name`**, **`Text` → `Description`**,
`Example` **vide** (pas de 3ᵉ champ), chemins synthétiques « 1 »…« 15 ».
`EntityTypeName = "Rule"`, `RootName = "Rules"`.

⚠️ `Example` doit rester **vide** et non `"{}"` : `Interpolate` découpe sur `{(.+?)}`, donc un `{}`
vide ne correspond pas et traverse littéralement l'export (mesuré 29/10 : 120 `<Example>{}</Example>`).

**Le DoD de ce payload est déjà couvert par des gardes vertes** — `Dnn2sxcCommonExportContractTests`
vérifie, pour chaque taxonomie câblée : racine unique par culture, **tout `Parent` résolu dans
l'export**, **aucun littéral `[]`/`{}`**, **aucun double échappement `&#`**, et tout `Name` non vide.
⇒ La qualité de la charge utile n'est plus une question ouverte. Seule **la porte** l'est.

## 2. Ce que la base contient réellement (VÉRIFIÉ)

| Content-type | App | Attributs | Entités | Langues |
|---|---|---|---:|---|
| **`Game Rule`** (ct 377) | 60 | Title, Summary, Material, MinNbPlayers, MaxNbPlayers, Installation, Content, Variants, Memo, Date, Author, Licence, Original, UrlKey, Parent | **5** | 8 (base FR + dim 7→13) |
| **`Rule`** | — | — | **inexistant en zone 3** | — |

- **App 60 ne possède que 13 content-types, et `Rule` n'en fait pas partie** — aujourd'hui **et**
  dans l'export DNN du 2026-07-07 (`10-app60-content-types.json`, 13 types, sans `Rule`). Le type
  `Rule` n'a jamais existé dans l'app cible : l'import le **créerait**.
- Les 5 entités `Game Rule` sont **5 jeux** Argumentum — `L'école des menteurs`,
  `Le Bingo mixologie argumentative`, `Le dernier beau parleur`, `Le moulin à baratin`,
  `La parlote coinchée` — **owner `dnn:userid=1`**, donc contenu de projet, **pas** de la démo.
- Elles portent le contenu **agrégé au jeu** : `Summary`, `Material`, `Installation`, `Content`,
  `Variants`, `Memo` — c'est-à-dire exactement les clés que le site adresse, via
  `App-Resources` de l'app 60 (`14-res-rule-dictionary.json` : `RuleSummary`, `RuleMaterial`,
  `RuleInstallation`, `RuleContent`, `RuleVariants`, `RuleMemoCard`, `RuleMemoCardDownload`,
  `RuleMemoCardFileNamePrefix`).
- `Title` est **5/5 dans les 8 langues** (40 valeurs) ; 52 valeurs en base contre 28 par langue.
  L'écart s'explique par les attributs **non traduisibles** (`Parent`, `MinNbPlayers`,
  `MaxNbPlayers`, `Date`, `Author`, `Licence`, `Original`, `UrlKey`) — pas par une lacune de
  traduction. **L'audit par attribut n'a pas été exécuté** : c'est le seul instrument qui puisse
  prononcer une couverture.

⇒ **Les deux contenus ne sont pas redondants et ne sont pas équivalents.** L'export XML apporte
le **texte par ligne CSV** (le corps de chaque carte-règle, en markdown, dans les 8 langues) ; la
base porte la **fiche de jeu**. Le site rend aujourd'hui la seconde. L'import du premier n'est donc
pas fait, et il n'est pas non plus redondant — mais **il n'est pas nécessaire au rendu actuel**.

## 3. La porte : (b) réfutée, (a) contredite, le rôle aussi

Le contrat de l'API a été lu **dans les DLL du site** (`ToSic.Eav.WebApi.dll`,
`ToSic.Sxc.Dnn.WebApi.dll`), sans toucher au site :

- gabarit de route : **`admin/{controller}/{action}`**, plus `admin/{controller}/{action}/{name}`
  et `api/2sxc/admin/[controller]/{name}` ;
- paramètres lus : **`zoneId`**, **`appId`** ;
- contrôle de permission : **`IsSystemAdmin` / `IsSiteAdmin` / `IsContentAdmin`**,
  `ThrowIfNotSiteAdmin`, `ThrowIfNotContentAdmin`, attribut **`DnnAuthorizeAttribute`**,
  `DnnModuleAuthorizeAttribute`, et `get_IsSuperUser` ;
- `web.config` du module `ToSic.Sxc` : **fichier factice** (redirections de binding uniquement) —
  **ni section `authorization` ni section `handlers`**.

**Sonde authentifiée (session fraîche, compte dédié, une seule connexion) :**

| Requête | HTTP | Corps |
|---|---:|---|
| `…/api/admin/zone/GetSystemInfo?zoneId=3` | 401 | `{"Message":"Authorization has been denied for this request."}` (61 o) |
| `…?zoneId=3&appId=60` | 401 | **rigoureusement identique** (61 o) |
| `…?appId=60` | 401 | identique |
| `…/api/admin/zone/GetLanguages?zoneId=3` | 401 | identique |
| `…/api/admin/zone/GetSystemInfo/3` | 401 | identique |
| `…/api/2sxc/admin/zone/3` | **404 IIS** | route non mappée |

- **(b) « contexte zone/app manquant » est RÉFUTÉ par la mesure** : la zone réelle (3) et l'app
  réelle (60), ensemble et séparément, ne changent **rien** au 401 — même octet pour octet.
  (Il faut noter que `zoneId=1` et `zoneId=2` seuls n'auraient rien prouvé : les zones mesurées
  sont 1 = Default, 2 = Argumentum Games (portal 0), **3 = Argumentum (portal 1)**.)
- **(a) « le pipeline ne porte pas l'identité Forms » est CONTRADIT** : le journal DNN
  (`dbo.EventLog`, schéma v10 : `LogCreateDate`, `LogUserName`, `LogTypeKey`, `LogProperties`)
  enregistre les exceptions 2sxc de cette **même famille d'URL** avec
  `LogUserName = import-test@argumentum.games` ⇒ l'identité DNN **est** établie sur ces URL.
  De plus, la connexion est journalisée **`LOGIN_SUPERUSER`**, et **aucun échec** de connexion.
  ⚠️ Le contre-indice invoqué auparavant (« événement ASP.NET 1309, `Is authenticated: True` ») **ne se
  retrouve pas** dans `dbo.EventLog` sur les 3 dernières heures — il n'est pas corroboré par cet
  oracle.
- **Le rôle manquant est RÉFUTÉ aussi** : `UserID 120` a **exactement** les mêmes portes que le
  compte owner (`UserID 1`) : portail **1** `Authorised=1`, rôles **9 Administrators**,
  **10 Registered Users**, **11 Subscribers**, `IsSuperUser=1`, `IsDeleted=0`.
- Le refus ne laisse **aucune trace** dans le journal DNN et **aucun en-tête `WWW-Authenticate`** :
  c'est un refus de la couche ASP.NET Web API (`DnnAuthorizeAttribute` →
  `HandleUnauthorizedRequest`), pas un défi HTTP, et pas un refus métier 2sxc (« Permission
  denied. » est une chaîne distincte, présente dans la DLL mais **jamais** rendue ici).

⇒ **Ce qui reste n'est ni l'identité, ni le contexte, ni les rôles.** C'est la **configuration
d'administration 2sxc du site lui-même** (son app/session d'admin, ses features, sa zone d'admin).

**Le discriminant qui tranche en une donnée** : est-ce que la session **du owner** obtient le même
401 sur cette API ? Si oui, le site n'a pas d'administration 2sxc opérationnelle et l'import par
cette voie est hors de portée — il faut une autre voie (import direct en base, ou 2sxc en mode
« app admin »). Si non, c'est la configuration de la session du compte de test qui bloque.

## 4. Ce qui reste ouvert

1. **Le discriminant ci-dessus** (session owner sur `admin/zone/GetSystemInfo`) — une donnée,
   et l'item ② bascule soit vers « autre voie d'import », soit vers « simple configuration à
   corriger ». ⚠️ Le compte owner n'a pas de mot de passe utilisable côté lane (le fichier
   `preprod-admin-20261029.txt` contient une valeur **rejetée**, 20 o) : cette donnée ne peut pas
   être produite seule par la lane.
2. ~~**Audit par attribut** de `Game Rule` (8 langues) — seul oracle de couverture.~~
   **Exécuté le même jour (§5) : couverture prononcée COMPLETE** pour tout ce qui est traduisible ;
   l'écart « 52 valeurs de base vs 28 par langue » est intégralement expliqué par la mesure.
3. **Décider du périmètre** : l'import du type `Rule` (16 entités × 8 langues, texte par ligne
   CSV) crée-t-il un 14ᵉ content-type dans l'app 60, que **rien dans le site ne rend aujourd'hui** ?
   C'est une **décision de contenu**, pas une tâche technique.

## 5. Audit par attribut de `Game Rule` — couverture prononcée (exécuté 30/10, lecture seule)

Matrice attribut × langue mesurée sur les 5 entités (11378, 11380, 11387, 11388, 11389), dims
6→13 = fr-FR…zh-CN toutes actives sous la racine 5 « Culture Root » (contrôle refait sur
`TsDynDataDimension.Name`). **Rappel : le français vit dans la valeur de base** (ligne sans
dimension).

| Attribut | Nature | Couverture mesurée |
|---|---|---|
| `Title`, `Summary`, `Material`, `Installation`, `Content` | traduisibles | **5/5 entités × (base + 7 langues), 0 valeur vide, 0 contamination** — complet |
| `Variants` | traduisible | 2/5 entités traduites (11378, 11388) ; les 3 autres ont une base **vide** = jeux sans variantes (**vide structurel cohérent**, pas une lacune) |
| `Memo` | traduisible | 1/5 entité traduite (11378 : base + 7 langues) ; la base de 11388 est une **ligne vide résiduelle** (placeholder sans contenu — seul résidu cosmétique) |
| `MinNbPlayers`, `MaxNbPlayers`, `Date`, `UrlKey` | non traduisibles | 5/5 base, 0 vide |
| `Parent`, `Author`, `Licence`, `Original` | jamais remplis | **0 valeur** — déclarés au schéma, jamais alimentés |

**Trois oracles croisés, tous verts :**

1. **Contamination : 0.** Aucune valeur dimensionnée n'est identique (octets) à la base FR de la
   même paire entité×attribut.
2. **Base manquante : 0.** Aucune paire entité×attribut traduite sans sa base FR (contrôle inverse).
3. **Profondeur zh-CN : pleine fidélité.** Les longueurs zh (~4-5× plus courtes que FR en *caractères*)
   auraient pu masquer des traductions squelettiques ; sonde de fond sur e=11378 : le `Summary`
   zh porte **les deux paragraphes complets** (saynète improvisée, classification ordres/familles,
   exemple « flattery » sous-classe de « appeal to emotion ») et le `Content` zh porte **toute la
   structure** (本轮流程 → 抽牌者 → 诡辩者 → 即兴短剧 → 评审团). C'est la densité CJK, pas une troncature.

**Réconciliation arithmétique exacte** de l'écart qui intriguait (« 52 valeurs de base contre 28 par
langue ») : base = 5 attributs cœur × 5 entités (25) + `Variants` (5) + `Memo` (2) + 4 non-traduisibles
× 5 (20) = **52** ; par langue = 25 + 2 + 1 = **28**. La mise en garde de
[`2026-10-30-sxc-data-surface-measured.md`](2026-10-30-sxc-data-surface-measured.md) (« écart
attribuable aux attributs non traduisibles — l'audit seul peut prononcer ») est donc **confirmée au
chiffre exact** : **aucune traduction ne manque** dans `Game Rule`.

## 6. Aucune écriture

Aucun import, aucune injection EAV, aucune modification de contenu, aucun changement de rôle.
Une seule connexion authentifiée (compte de test dédié), aucun échec. Le compte
`import-test@argumentum.games` (UserID 120) reste à purger en fin d'usage
(`purge-import-admin.ps1` + `rollback-import-admin.sql`, scratchpad) ; son secret hors dépôt
(`G:\Mon Drive\MyIA\Argumentum\Admin\po2023-import-admin-20261030.txt`) **n'est pas supprimé**.
