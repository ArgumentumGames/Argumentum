# #458 grain ⑨-bis — pack de langue fa-IR, famille AGR cœur (Tier 2, lot 3)

> **Statut :** pack **partiel** livré (358 clés), préprod/webroot **non touché** (les `.resx`
> sont des fichiers du dépôt ; leur compilation par IIS se fait au recyclage du pool, geste
> po-2023 distinct). Suivi : [#458](https://github.com/ArgumentumGames/Argumentum/issues/458).
> Dispatch : ai-01, pool v3 (c.6368412786), grain ⑨-bis. Lot 1 (zh-CN) : PR #2067, lot 2
> (ar-SA) : PR #2068, dont la méthode est reprise telle quelle ici. Dénominateur : PR #2065
> (1 227 clés/langue).

---

## 1. Ce que ce lot livre

Le **même périmètre** que les lots 1-2 — les 4 fichiers de la famille AGR cœur — pour la culture
**fa-IR**. Le jeu de clés est **identique aux lots précédents**, clé pour clé (mesuré à la
fusion : le dictionnaire couvre le même ensemble de 356 clés traduites que zh-CN et ar-SA).

| Fichier | Clés livrées | Clés du neutre | Reste |
|---|---:|---:|---:|
| `FileUpload.fa-IR.resx` (nouveau) | 4 | 4 | 0 |
| `WebControls.fa-IR.resx` (nouveau) | 50 | 50 | 0 |
| `Exceptions.fa-IR.resx` (nouveau) | 132 | 132 | 0 |
| `GlobalResources.fa-IR.resx` (étendu) | 172 | 182 | 10 |
| `SharedResources.fa-IR.resx` | *non touché* | 617 | 614 |
| `List_Country.fa-IR.resx` | *non créé* | 256 | 256 |
| **Total** | **358** | **1 241** | **880** |

Les deux textes juridiques (`MESSAGE_PORTAL_TERMS` / `MESSAGE_PORTAL_PRIVACY`) sont **préservés
tels quels** — y compris leur **miroir RTL** (E4 : `align="right"`, padding racine échangé),
arbitrés par le grain juridique. Le constructeur les reporte verbatim (précédence : une clé du
fichier culturel absente du dictionnaire est préservée).

## 2. Méthode — identique aux lots 1-2

Voir [`458-item9bis-zh-cn-pack.md`](458-item9bis-zh-cn-pack.md) §2-3. Résumé : on part du neutre,
on ne remplace que le contenu des `<value>` ; en-tête, `resheader`, ordre des clés, **BOM UTF-8**,
fins de ligne **LF** préservés ; une clé non traduite est **absente** ⇒ DNN retombe sur l'anglais
clé par clé. Le constructeur est le **même script** (`458-item9bis-langpack-build.py`), appelé avec
`--lang fa-IR`. Dictionnaire source : `458-item9bis-fa-IR.translations.json` (356 entrées).

La fusion des traductions est **validée avant construction** (script de fusion scratchpad) :
jeu de clés **identique** au pack zh-CN (tout écart = refus), multiset des placeholders et identité
des jetons DNN égaux au neutre pour chaque clé, aucune valeur vide. Le constructeur rejoue les
mêmes contrôles à l'écriture.

## 3. Les 10 clés de `GlobalResources` non livrées

**Les mêmes que les lots 1-2, pour les mêmes motifs nommés** (licence MIT, jeton pur, marques,
coquilles HTML, pages d'erreur hébergeur, liste de mots vides de l'indexeur, 2 clés juridiques
déjà traduites) — voir [`458-item9bis-zh-cn-pack.md`](458-item9bis-zh-cn-pack.md) §4. DNN sert
l'anglais pour ces dix chaînes en fa-IR.

## 4. Spécificités fa-IR par rapport aux lots 1-2

- **Verbatim par conception : 2 clés, comme ar-SA.** Le sujet du courriel de bannière
  (`EMAIL_BANNER_NOTIFICATION_SUBJECT.Text`, jeton pur `[Custom:0]`) **plus** le suffixe de champ
  `FieldSuffix.Text` (« `:` ») : **le persan n'a pas de variante locale du deux-points** (ni du
  « ： » plein-chasse zh, ni d'autre forme) — la valeur traduite EST le deux-points latin.
- **Orthographe persane, contrôlée à la fusion** : lettres **ی/ک persanes** (U+06CC / U+06A9)
  uniquement — les lettres arabes **ي/ك/ى sont interdites** (contrôle bloquant du script de
  fusion, même politique que la garde juridique `DnnLegalZhArFaGuardTests`) ; **نیم‌فاصله**
  (ZWNJ, U+200C) employé pour les préfixes/suffixes (می‌کند، فایل‌ها، ثبت‌نام).
- **Chiffres latins uniquement** (contrôle bloquant à la fusion : aucun chiffre arabo-indien
  ٠-٩ ni persan ۰-۹) — même décision que le grain juridique (décision 2).
- **Dialecte RTL** : la direction de la PAGE appartient à DNN (`dir=` du portail), pas au
  contenu — **aucun `dir=` introduit** (E4).
- **Jetons DNN** : identités conservées exactement, y compris les différences de casse du neutre
  (`[User:UserName]` / `[User:Username]` — chaque clé conserve **sa** forme) et les URLs de
  réinitialisation reproduites telles quelles.

## 5. La garde d'intégrité — évolutions du lot 2

| Garde | État avant (lot 2) | Adaptation (lot 3) |
|---|---|---|
| `DnnAgrCoreResxIntegrityGuardTests` | paramétrée zh-CN + ar-SA (18 tests) | **fa-IR ajouté** à `Covered` (4 fichiers) et à la liste blanche verbatim (2 clés) — 27 tests |
| `DnnLegalZhArFaGuardTests.Resx_Carries_The_Two_Legal_Keys_Preserved` | {zh-CN, ar-SA} assouplis, fa-IR épinglé à 2 clés | l'épinglage fa-IR est **retiré** (branche devenue vacue : les trois cultures portent le pack) ; l'assertion de base `Contain` reste |
| `ChromeTier1LocalizationContractTests` | **non touchée** | **toujours non touchée** : ce lot ne modifie pas `SharedResources` |

Toutes les autres assertions de `DnnLegalZhArFaGuardTests` (valeurs exactes des clés juridiques,
`xml:space`, séquence de balises, **miroir RTL**, multiset de chiffres, ی/ک persans) **continuent
de s'appliquer à fa-IR et passent** — le lot ne touche pas aux deux clés juridiques, il les
préserve.

## 6. Suite

| Reste | Clés | Note |
|---|---:|---|
| `SharedResources.{zh-CN,ar-SA,fa-IR}` | 614 × 3 | exige l'évolution de `ChromeTier1LocalizationContractTests` (arbitrage ai-01) |
| `List_Country.{zh-CN,ar-SA,fa-IR}` | 256 × 3 | 256 codes ISO ; le neutre est une **coquille** (valeurs vides) |
| `GlobalResources.{zh-CN,ar-SA,fa-IR}` | 10 × 3 | motifs nommés §3, à trancher par ai-01 |

La famille AGR cœur (4 fichiers, 358 clés) est désormais **couverte pour les trois cultures**
zh-CN / ar-SA / fa-IR : le grain ⑨-bis est fermé sur son périmètre actionnable, le reste est
gated arbitrage ai-01.

## 7. Portée et réversibilité

- **Aucune** valeur de production n'est touchée ; aucun `.resx` de la prod n'est modifié.
- Fichiers **nouveaux** (3) ou **étendus** (1), tous sous `DNNPlatform/App_GlobalResources/`,
  versionnés, réversibles par un revert du commit.
- Le webroot servi (`D:\Dev\Argumentum`) **reste sur `master`** : travail dans le worktree détaché
  (`D:\A9ZC`), sur une branche **empilée** sur le lot 2 (`feat/458-item9bis-ar-pack`, PR #2068) —
  l'ordre de merge recommandé est #2067, #2068 puis ce lot.
- Aucune donnée membre, aucun envoi de formulaire, aucune valeur secrète.
