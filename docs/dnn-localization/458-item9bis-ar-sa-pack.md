# #458 grain ⑨-bis — pack de langue ar-SA, famille AGR cœur (Tier 2, lot 2)

> **Statut :** pack **partiel** livré (358 clés), préprod/webroot **non touché** (les `.resx`
> sont des fichiers du dépôt ; leur compilation par IIS se fait au recyclage du pool, geste
> po-2023 distinct). Suivi : [#458](https://github.com/ArgumentumGames/Argumentum/issues/458).
> Dispatch : ai-01, pool v3 (c.6368412786), grain ⑨-bis. Lot 1 (zh-CN) : PR #2067 + le document
> [`458-item9bis-zh-cn-pack.md`](458-item9bis-zh-cn-pack.md), dont la méthode est reprise telle
> quelle ici. Dénominateur : PR #2065 (1 227 clés/langue).

---

## 1. Ce que ce lot livre

Le **même périmètre** que le lot zh-CN — les 4 fichiers de la famille AGR cœur — pour la culture
**ar-SA**. Le jeu de clés est **identique au lot 1**, clé pour clé (mesuré à la fusion : les deux
dictionnaires couvrent le même ensemble de 356 clés traduites).

| Fichier | Clés livrées | Clés du neutre | Reste |
|---|---:|---:|---:|
| `FileUpload.ar-SA.resx` (nouveau) | 4 | 4 | 0 |
| `WebControls.ar-SA.resx` (nouveau) | 50 | 50 | 0 |
| `Exceptions.ar-SA.resx` (nouveau) | 132 | 132 | 0 |
| `GlobalResources.ar-SA.resx` (étendu) | 172 | 182 | 10 |
| `SharedResources.ar-SA.resx` | *non touché* | 617 | 614 |
| `List_Country.ar-SA.resx` | *non créé* | 256 | 256 |
| **Total** | **358** | **1 241** | **880** |

Les deux textes juridiques (`MESSAGE_PORTAL_TERMS` / `MESSAGE_PORTAL_PRIVACY`) sont **préservés
tels quels** — y compris leur **miroir RTL** (E4 : `align="right"`, padding racine échangé),
arbitré par le grain juridique. Le constructeur les reporte verbatim (précédence : une clé du
fichier culturel absente du dictionnaire est préservée).

## 2. Méthode — identique au lot 1

Voir [`458-item9bis-zh-cn-pack.md`](458-item9bis-zh-cn-pack.md) §2-3. Résumé : on part du neutre,
on ne remplace que le contenu des `<value>` ; en-tête, `resheader`, ordre des clés et fins de
ligne **LF** préservés, **BOM UTF-8 ré-émis à l'écriture** (voir lot 1 §2) ; une clé non traduite
est **absente** ⇒ DNN retombe sur l'anglais
clé par clé. Le constructeur est le **même script** (`458-item9bis-langpack-build.py`), appelé avec
`--lang ar-SA`. Dictionnaire source : `458-item9bis-ar-SA.translations.json` (358 entrées).

La fusion des traductions est **validée avant construction** (script de fusion scratchpad) :
jeu de clés **identique** au lot zh-CN (tout écart = refus), multiset des placeholders et identité
des jetons DNN égaux au neutre pour chaque clé, aucune valeur vide. Le constructeur rejoue les
mêmes contrôles à l'écriture.

## 3. Les 10 clés de `GlobalResources` non livrées

**Les mêmes que le lot 1, pour les mêmes motifs nommés** (licence MIT, jeton pur, marques,
coquilles HTML, pages d'erreur hébergeur, liste de mots vides de l'indexeur, 2 clés juridiques
déjà traduites) — voir [`458-item9bis-zh-cn-pack.md`](458-item9bis-zh-cn-pack.md) §4. DNN sert
l'anglais pour ces dix chaînes en ar-SA.

## 4. Spécificités ar-SA par rapport au lot 1

- **Verbatim par conception : 2 clés au lieu d'une.** Le sujet du courriel de bannière
  (`EMAIL_BANNER_NOTIFICATION_SUBJECT.Text`, jeton pur `[Custom:0]`) — comme zh-CN — **plus** le
  suffixe de champ `FieldSuffix.Text` (« `:` »). Là où zh-CN dispose du deux-points plein-chasse
  « ： », **l'arabe n'a pas de variante locale du deux-points** : la valeur traduite EST le
  deux-points latin. La liste blanche est **par culture** dans la garde.
- **Dialecte RTL** : les valeurs traduites sont en arabe standard moderne ; la direction de la
  PAGE appartient à DNN (`dir=` du portail), pas au contenu — aucun `dir=` introduit, conformément
  au grain E4. Les deux-points et les placeholders `{0}` restent en forme latine, comme les
  chiffres (décision 2 du grain juridique).
- **Jetons DNN** : identités conservées exactement, y compris les différences de casse du neutre
  (`[User:UserName]` dans certains corps, `[User:Username]` dans d'autres — chaque clé conserve
  **sa** forme) et les URLs de réinitialisation
  (`[Portal:PASSWORDREMINDERURL]?ctl=PasswordReset&resetToken=[Membership:PasswordResetToken]`,
  `[Portal:FULLURL]/default.aspx?ctl=…`) reproduites telles quelles.

## 5. La garde d'intégrité — évolutions du lot 1

| Garde | État avant (lot 1) | Adaptation (lot 2) |
|---|---|---|
| `DnnZhCnResxIntegrityGuardTests` | classe monolithique zh-CN (10 tests) | **renommée** `DnnAgrCoreResxIntegrityGuardTests` et **paramétrée par culture** (zh-CN + ar-SA, mêmes assertions, liste blanche verbatim par culture) — 18 tests |
| `DnnLegalZhArFaGuardTests.Resx_Carries_The_Two_Legal_Keys_Preserved` | assouplie pour zh-CN seul | assouplissement **étendu à ar-SA** ; **fa-IR reste épinglé à 2 clés** (son pack n'est pas livré) |
| `ChromeTier1LocalizationContractTests` | **non touchée** | **toujours non touchée** : ce lot ne modifie pas `SharedResources` |

Toutes les autres assertions de `DnnLegalZhArFaGuardTests` (valeurs exactes des clés juridiques,
`xml:space`, séquence de balises, **miroir RTL**, multiset de chiffres) **continuent de
s'appliquer à ar-SA et passent** — le lot ne touche pas aux deux clés juridiques, il les
préserve.

## 6. Suite

| Reste | Clés | Note |
|---|---:|---|
| `SharedResources.{zh-CN,ar-SA}` | 614 × 2 | exige l'évolution de `ChromeTier1LocalizationContractTests` (arbitrage ai-01) |
| `List_Country.{zh-CN,ar-SA}` | 256 × 2 | 256 codes ISO ; le neutre est une **coquille** (valeurs vides) |
| `GlobalResources.{zh-CN,ar-SA}` | 10 × 2 | motifs nommés §3, à trancher par ai-01 |
| `fa-IR` | 358 | lot 3, même méthode ; la garde est déjà prête à l'accueillir |

## 7. Portée et réversibilité

- **Aucune** valeur de production n'est touchée ; aucun `.resx` de la prod n'est modifié.
- Fichiers **nouveaux** (3) ou **étendus** (1), tous sous `DNNPlatform/App_GlobalResources/`,
  versionnés, réversibles par un revert du commit.
- Le webroot servi (`D:\Dev\Argumentum`) **reste sur `master`** : travail dans le worktree détaché
  (`D:\A9ZC`), sur une branche **empilée** sur le lot 1 (`feat/458-item9bis-zh-pack`, PR #2067) —
  l'ordre de merge recommandé est #2067 puis ce lot.
- Aucune donnée membre, aucun envoi de formulaire, aucune valeur secrète.
