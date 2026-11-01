# #458 grain module-level — lot `DesktopModules/Admin` fa-IR (2026-11-01)

Jumeau persan du lot ar-SA (`458-item9bis-dtm-admin-ar-2026-11-01.md`) et, par
lui, du lot zh-CN 3b (`458-item9bis-dtm-admin-zh-2026-10-31.md`) : mêmes
**20 fichiers**, mêmes **640 clés livrées** sur **645 neutres**, mêmes 5 clés
déclarées non livrées, mêmes 6 valeurs verbatim.

## Périmètre — l'étiquette du pool n'est pas le périmètre

Le pool désigne « `DesktopModules/Admin` fa-IR ». Mesuré avant écriture :

| Objet | Fichiers | Clés |
|---|---:|---:|
| Arbre `DNNPlatform/DesktopModules/Admin` (tous neutres) | **65** | **4 395** |
| Périmètre du lot = table `Covered` de la garde | **20** | **645** |
| Livré ici | **20** | **640** |

Les 45 fichiers restants (`Dnn.PersonaBar/Modules/*`, `Console`, `EditBar`,
`HtmlEditorManager`, `ModuleCreator`, `SearchResults`, `EditExtension`,
`Authentication`, …) n'appartiennent à aucun lot livré : volets ultérieurs, pas
un oubli.

`Admin/Security` = 17 fichiers (626 clés neutres), `Admin/ViewProfile` = 3
fichiers (19 clés neutres).

### Un résidu hors lot, nommé et mesuré : `Security/DataConsent.ascx`

7 clés neutres. Il porte des sœurs **`es-ES`/`ru-RU`/`zh-CN`** — des packs DNN
d'origine, pas des lots de traduction — et **ni `ar-SA` ni `fa-IR`**. Le jumeau
ar-SA ne l'a pas livré non plus : il est donc hors lot ici, et il le reste.

Ce point est une **découverte de ce lot**, pas un détail : le vérificateur ar
sautait en silence tout neutre sans cible (`if (d, stem) not in MISSING and not
os.path.isfile(target): continue`), donc il ne dérivait pas son périmètre du
disque. Le vérificateur fa déclare l'exclusion avec sa raison mesurée
(`EXCLUDED`), et la garde C# épingle le résidu
(`TheOutOfLotResidualIsNamedAndStillOutOfLot`) : le jour où
`DataConsent.ascx.fa-IR.resx` apparaît, il faudra l'ajouter à `Covered`
**délibérément**.

## Ce que le lot livre

- **20 fichiers `*.fa-IR.resx`** (mesuré : 17 `Security` + 3 `ViewProfile`) :
  19 porteurs de traduction (dont `Settings.ascx` et `ViewProfile.ascx`) +
  1 coquille (`ViewProfile/SharedResources`, **0 clé**, 5 699 o — **taille
  identique** aux coquilles zh-CN et ar-SA).
- 2 JSON de traduction : `458-item9bis-dtm-admin-fa-IR.{security,viewprofile}.translations.json`.
- `DnnModuleLevelDtmAdminFaResxGuardTests` — miroir de la jumelle ar, **127 tests**.

### Non livrées (repli neutre DNN, clé par clé)

| Fichier | Clés | Raison |
|---|---|---|
| `Security/ProfileDefinitions.ascx` | `Edit.Header`, `Del.Header`, `Dn.Header`, `Up.Header` | neutre **vide** — icônes de grille, rien à localiser |
| `ViewProfile/SharedResources` | `DefaultTemplate.Text` | toute la valeur vit dans des jetons `[PROFILE:…]`/`[USER:…]` |

### Verbatim par conception (6, épinglés)

Quatre marques — `Facebook`, `Google+`, `LinkedIn`, `Twitter` —, `Skype ID`, et
le filtre alphabétique `A,B,C,…,Z`.

## Persan ≠ arabe — le sens inversé par rapport à la jumelle ar

Le persan et l'arabe **partagent le bloc U+0600–U+06FF**. « Porte au moins une
lettre de l'écriture » ne distingue donc **rien** : un pack fa-IR recopié sous
`ar-SA` passe à 100 %, et un pack ar-SA recopié sous `fa-IR` aussi.

Chaque garde ferme **son** sens :

| Garde | Règle | Ce qu'elle attrape |
|---|---|---|
| `…AdminArResxGuardTests` | **zéro** lettre exclusive au persan (`پ چ ژ گ ک ی`) | un pack **fa** recopié sous `ar-SA` |
| `…AdminFaResxGuardTests` (ici) | **zéro** lettre exclusive à l'arabe (`ي` U+064A, `ك` U+0643, `ى` U+0649) | un pack **ar** recopié sous `fa-IR` |

Mesuré le 2026-11-01, dans les deux sens :

| Contrôle | Résultat |
|---|---|
| Corpus `*.ar-SA.resx` du dépôt (75 fichiers, 2 378 valeurs non vides) | **1 616** valeurs portent `ي`/`ك`/`ى` |
| Pack fa-IR livré (640 valeurs) | **0** |

Un zéro n'est un résultat que si le même instrument rend non-zéro là où le
défaut existe. L'étalonnage est **rejoué à chaque passage de suite**
(`TheArabicExclusiveDetectorIsCalibratedOnTheArCorpus`, plancher **1 000** —
plancher, pas compte exact : les lots ar-SA à venir ne doivent pas casser cette
garde, ils ne peuvent que l'augmenter), et doublé d'un contrôle inverse en
séance sur échantillons `\u`-échappés (`TheArabicExclusiveDetectorCanSeeADefect`),
immunisé contre la normalisation d'un copier-coller.

### Dispenses d'écriture : aucune de plus que la jumelle ar

`ScriptExempt` **égale** `VerbatimByDesign` (6). Contrairement à la jumelle zh,
le gabarit `TrialFee.Text` n'a pas besoin de dispense : sa prose persane
(`{0} برای {1} {2}`) porte l'écriture. Le fait est épinglé par
`TheScriptExemptListCannotGrowSilently`, qui exige l'égalité des deux listes —
toute exemption nouvelle est une décision, pas un constat.

## Mesures

| Instrument | Résultat |
|---|---|
| Couverture (union des 2 fichiers de traduction contre le neutre) | **640/640**, 0 erreur de contenu |
| Vérificateur indépendant (relit les 20 `.fa-IR.resx`, ne relit aucune traduction) | **20/20 fichiers, 640 clés, 0 écart** |
| Garde `DnnModuleLevelDtmAdminFaResxGuardTests` | **127/127** |
| Suite complète | **3 927 = 3 922 / 0 / 5** |
| Banc de mutations (vérificateur Python **et** garde C#) | **4/4 rouges** sur la règle visée dans les deux, restauration au sha exact |

Le nombre d'ignorés (5 ici, 6 sur ai-01) est **local à la machine** : les `Skip`
sont conditionnels (`python` exécutable, index git). Le total, lui, est exact.

Transport : BOM **dérivé du neutre fichier par fichier** (`--bom mirror`), **LF
partout**, écriture par le constructeur du dépôt
(`458-item9bis-langpack-build.py --bom mirror`), espaces de tête conservés
(`Register.ascx:RegisterWarning.Text` commence par une espace).

### Banc de mutations

Cible : `Security/Membership.ascx.fa-IR.resx`, valeur `CreatedDate.Text`
(`تاریخ ایجاد:`). sha avant = sha après = `c992262e…fee01`.

| Mutation | Rouge observé (garde C#) | Rouge observé (vérificateur) |
|---|---|---|
| M1 lettre arabe `ي` injectée | `lettres arabes exclusives [ي]` — 1 test | `LETTRE ARABE EXCLUSIVE` |
| M2 valeur remplacée par l'anglais | `aucun caractère persan` (« Created: ») — 1 test | `aucune ecriture persane` |
| M3 bloc `<data>` supprimé | `30 item(s)` attendus, 29 trouvés — 2 tests | `ordre/jeu de cles different du neutre` |
| M4 **latin non anglais** (`Jamais`) | `aucun caractère persan` — 1 test | `aucune ecriture persane` |

M4 est le contrôle discriminant : il attrape ce que « ≠ anglais » rate.

Chaque mutation est restaurée par `cp` + comparaison sha256 — **jamais**
`git checkout --`, qui restaurerait depuis l'index et effacerait tout le
non-committé du fichier.

## Limites

- **Le banc ne couvre pas la règle BOM/LF.** `BomMirrorsTheNeutralAndLineEndingsAreLf`
  lit le **blob committé** (`git cat-file -p HEAD:<path>`, nécessaire car le
  runner CI a `core.autocrlf=true`) : une mutation de l'arbre de travail ne
  l'atteint pas. Le banc prouve les règles de **contenu**, pas celle du
  transport ; celle-ci est prouvée par le passage vert sur les fichiers
  réellement committés.
- Corollaire du même point : les mutations réécrivent en `utf-8-sig`, ce que le
  vérificateur indépendant signale comme `BOM ne suit pas le neutre` — artefact
  du **banc**, pas du pack (le neutre n'a pas de BOM, le `--bom mirror` non
  plus). Le rouge surnuméraire est attendu et ne compte pas dans le verdict.
- Une valeur **à moitié traduite** passe la règle d'écriture (le fragment
  traduit porte déjà une lettre persane) : même limite que les lots zh et ar,
  non revendiquée comme fermée.
- La **qualité éditoriale** du persan n'est pas prouvée par ces instruments ;
  une relecture native reste le seul juge du sens.
- Le persan du pack est du **registre standard** sans vocalisation, aligné sur
  les lots `controls`/`admin`/`dtm` déjà livrés ; aucune variante régionale
  (tadjik, dari) n'est revendiquée.
