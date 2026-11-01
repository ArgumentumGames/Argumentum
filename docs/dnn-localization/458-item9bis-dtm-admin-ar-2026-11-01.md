# #458 grain module-level — lot `DesktopModules/Admin` ar-SA (2026-11-01)

Jumeau arabophone du lot zh-CN 3b (`458-item9bis-dtm-admin-zh-2026-10-31.md`) :
mêmes **20 fichiers**, mêmes **640 clés livrées** sur **645 neutres**, mêmes
5 clés déclarées non livrées, mêmes 6 valeurs verbatim.

## Périmètre — l'étiquette du pool n'est pas le périmètre

Le pool désigne « `DesktopModules/Admin` ar-SA puis fa-IR ». Mesuré avant écriture :

| Objet | Fichiers | Clés |
|---|---:|---:|
| Arbre `DNNPlatform/DesktopModules/Admin` (tous neutres) | **65** | **4 395** |
| Périmètre du lot = table `Covered` de la garde sœur zh | **20** | **645** |
| Livré ici | **20** | **640** |

Les 45 fichiers restants (`Dnn.PersonaBar/Modules/*`, `Console`, `EditBar`,
`HtmlEditorManager`, `ModuleCreator`, `SearchResults`, `EditExtension`,
`Authentication`, …) n'appartiennent à aucun lot livré : ils relèvent de volets
ultérieurs, pas d'un oubli. Même famille d'écart que « 9 fichiers / 381 clés →
37 / 689 » (lot 3c zh), « 67 → 63 » (Registration), « 38 → 29 » (IP filters).

`Admin/Security` = 17 fichiers (626 clés neutres), `Admin/ViewProfile` = 3
fichiers (19 clés neutres).

## Ce que le lot livre

- **21 fichiers `*.ar-SA.resx`** : 19 porteurs de traduction + 1 coquille
  (`ViewProfile/SharedResources`, **0 clé**, 5 699 o — **taille identique** à la
  coquille zh-CN) + `Settings.ascx`/`ViewProfile.ascx`.
- 2 JSON de traduction : `458-item9bis-dtm-admin-ar-SA.{security,viewprofile}.translations.json`.
- `DnnModuleLevelDtmAdminArResxGuardTests` — miroir de la jumelle zh, **126 tests**.

### Non livrées (repli neutre DNN, clé par clé)

| Fichier | Clés | Raison |
|---|---|---|
| `Security/ProfileDefinitions.ascx` | `Edit.Header`, `Del.Header`, `Dn.Header`, `Up.Header` | neutre **vide** — icônes de grille, rien à localiser |
| `ViewProfile/SharedResources` | `DefaultTemplate.Text` | toute la valeur vit dans des jetons `[PROFILE:…]`/`[USER:…]` |

Ni duplication ni trou : ces 5 clés sont **déclarées** dans la garde, comptées,
et une dispense ajoutée pour faire passer un rouge rougirait le fait de comptage.

### Verbatim par conception (6, épinglés)

Quatre marques — `Facebook`, `Google+`, `LinkedIn`, `Twitter` —, `Skype ID`, et
le filtre alphabétique `A,B,C,…,Z`. Le filtre est verbatim délibérément : le
filtrage par initiale d'un utilisateur arabophone se fait encore sur les lettres
latines de l'interface.

## Persan ≠ arabe — le contrôle ajouté par ce lot

Le persan et l'arabe **partagent le bloc U+0600–U+06FF**. Une garde qui exige
« au moins une lettre du bloc » accepte donc un pack **fa-IR recopié** sous
`ar-SA`, à 100 %.

Le persan écrit correctement emploie `ی` U+06CC et `ک` U+06A9 là où l'arabe
écrit `ي` U+064A et `ك` U+0643. La garde exclut six lettres exclusives au
persan : `پ` U+067E, `چ` U+0686, `ژ` U+0698, `گ` U+06AF, `ک` U+06A9, `ی` U+06CC.

Mesuré dans les deux sens le 2026-11-01 :

| Contrôle | Résultat |
|---|---|
| Corpus `*.fa-IR.resx` du dépôt (48 fichiers, 1 710 valeurs non vides) | **1 452** portent une lettre persane exclusive |
| Pack ar-SA livré (640 valeurs) | **0** |

Un zéro n'est un résultat que si le même instrument rend non-zéro là où le
défaut existe. L'étalonnage est **rejoué à chaque passage de suite**
(`ThePersianDetectorIsCalibratedOnTheFaCorpus`, plancher 1 000) et doublé d'un
contrôle inverse en séance sur échantillons `\u`-échappés
(`ThePersianDetectorCanSeeADefect`), immunisé contre la normalisation d'un
copier-coller.

## Mesures

| Instrument | Résultat |
|---|---|
| Couverture (union des 2 fichiers de traduction contre le neutre) | **640/640**, 0 erreur de contenu |
| Vérificateur indépendant (relit les 20 `.ar-SA.resx`, ne relit aucune traduction) | **20/20 fichiers, 640 clés, 0 écart** |
| Garde `DnnModuleLevelDtmAdminArResxGuardTests` | **126/126** |
| Banc de mutations | **4/4 rouges** sur la règle visée, restauration au sha exact |

Transport : BOM **dérivé du neutre fichier par fichier** (`--bom mirror`), **LF
partout**, écriture par le constructeur du dépôt
(`458-item9bis-langpack-build.py --bom mirror`), espaces de tête conservés
(`Register.ascx:RegisterWarning.Text` commence par une espace).

### Banc de mutations

| Mutation | Fichier | Rouge attendu |
|---|---|---|
| M1 lettre persane (`ی`) injectée | `Membership.ascx` | 1 — `lettre persane exclusive` |
| M2 valeur remplacée par l'anglais | `Membership.ascx` | 2 — `identique au neutre` + `aucune lettre arabe` |
| M3 bloc `<data>` supprimé | `Membership.ascx` | 1 — `ordre/jeu de cles different du neutre` |
| M4 **latin non anglais** (`Jamais`) | `Membership.ascx` | 1 — `aucune lettre arabe` |

M4 est le contrôle discriminant : il attrape ce que « ≠ anglais » rate. Chaque
mutation est restaurée par `cp` + comparaison sha256 — **jamais**
`git checkout --`, qui restaurerait depuis l'index et effacerait tout le
non-committé du fichier.

## Limites

- Une valeur **à moitié traduite** passe la règle d'écriture (le fragment
  traduit porte déjà une lettre arabe) : même limite que les lots zh et fa, non
  revendiquée comme fermée.
- La **qualité éditoriale** de l'arabe n'est pas prouvée par ces instruments ;
  une relecture native reste le seul juge du sens.
- L'arabe du pack est du **registre standard** (MSA) sans vocalisation, aligné
  sur les lots `controls`/`admin` déjà livrés ; aucune variante régionale n'est
  revendiquée.
