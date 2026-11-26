# #1485 — substitution DINPro → Barlow : rendus avant/après

**Branche** `feat/1485-embedded-fonts` · **PR** #2394 (brouillon) · **tête mesurée** `974319eb`
**Comparaison** `4da09a14` (avant) → `974319eb` (après)
**Portée mesurée** 6 CardSets × 2 langues (`fr`, `ru`), build **Debug**
**Date** 2026-11-26 · lane `myia-po-2023`

> Les rendus mesurés dans ce dossier sont ceux de `974319eb`. La restriction d'injection, les
> empreintes épinglées et la garde du hook (§8) sont **postérieures** : aucun rendu n'a été relancé
> pour elles, et le §4 dit à chaque fois ce qui est mesuré et ce qui est dérivé.

> ⚠️ Ce dossier porte des **mesures**, pas un verdict visuel. Le verdict visuel est
> réservé à `ai-01` (règle de la lane) : les planches ci-dessous lui sont destinées.

---

## 1. Ce que la branche change

| Objet | Nature |
|---|---|
| `Cards/Fallacies/Argumentum_Fallacies_Face_fr.json` | 4 blocs `@font-face` DINPro **distants** (cdnfonts) retirés ; `'DINpro',sans-serif` → `'Barlow', 'Rubik',sans-serif` (2 sélecteurs) |
| `WebBasedGenerator/Cardpen/EmbeddedFontFaces.cs` (nouveau) | 11 `@font-face` bâtis depuis des WOFF2 **embarqués** sous `Cards/fonts/`, émis en `data:font/woff2;base64,…`, `font-display: block`, `unicode-range` par sous-ensemble |
| `WebBasedGenerator/Cardpen/HarvestManager.cs` | **une** ligne de comportement : `css = EmbeddedFontFaces.Inject(css)` avant sérialisation du payload — `Inject` n'agit que sur un CSS qui nomme une famille embarquée |
| `Cards/fonts/**` | WOFF2 `vendorés` (Barlow 300/500/700/900 ×latin/latin-ext, Rubik 300-900 ×cyrillic/cyrillic-ext, Alfa Slab One) |

La forme `data:` est le seul véhicule **identique en Debug, en Release et sur une
branche** : le `/Cards/` servi est celui du checkout principal (mesuré 2026-11-26),
donc une URL relative rendrait la preuve dépendante de l'arbre qui sert, pas de
l'arbre qui construit.

---

## 2. Protocole

Deux worktrees jetables, même machine, même build :

| Côté | Worktree | Révision |
|---|---|---|
| AVANT | `.tmp/wt-p2309-before` (détaché) | `4da09a14` |
| APRÈS | `.tmp/wt-p2309-fonts` (= branche de la PR) | `974319eb` |

- **Debug**, jamais Release : un run Release télécharge les gabarits depuis
  `master` (`JsonFilePathRelease`, cf. #1225/#1228) et ne lirait donc pas la
  branche.
- Portée **temporaire et non commitée** dans les deux arbres : `Mode =
  WebBasedImageGeneration` seul, `CardSets` et `CardSetDocuments` réduits à
  Tarot / Virtues / Poker / Fallacies_Web_A4, `Translations = ("fr","ru")` par
  document. Restauration par copie de sauvegarde + comparaison `sha256`
  (`render-scope.py restore`), jamais `git checkout --`.
- Lançé depuis les jonctions courtes `D:\A2309B` / `D:\A2309F`, en **PowerShell**
  uniquement : depuis Git-Bash la jonction est résolue au spawn et le chemin
  franchit la garde des 250 caractères (#1179).

### Instrument de comparaison

`compare.py` — les PNG ne sont jamais comparés par `sha256` nu : Magick.NET
réécrit les chunks `tIME` et `tEXt date:` à chaque run, donc **tout** diffère
toujours. L'outil compare les **séquences de chunks en ignorant ces horodatages**
et exige l'identité octet pour octet de tous les autres (IDAT compris) ; seules
les paires réellement différentes sont décodées, pour le compte de pixels, la
boîte englobante et l'écart moyen.

---

## 3. Résultat global

| Langue | CardSet | avant | après | identiques | différents |
|---|---|---:|---:|---:|---:|
| fr | Fallacies | 176 | 176 | 0 | 176 |
| fr | Fallacies-Web | 175 | 175 | 0 | 175 |
| fr | Memo | 2 | 2 | 0 | 2 |
| fr | Rules | 15 | 15 | **1** | 14 |
| fr | Scenarii | 174 | 174 | 0 | 174 |
| fr | Virtues | 132 | 132 | 0 | 132 |
| ru | Fallacies | 176 | 176 | 0 | 176 |
| ru | Fallacies-Web | 175 | 175 | 0 | 175 |
| ru | Memo | 2 | 2 | 0 | 2 |
| ru | Rules | 15 | 15 | **1** | 14 |
| ru | Scenarii | 174 | 174 | 0 | 174 |
| ru | Virtues | 132 | 132 | 0 | 132 |

**1 348 images comparées : 1 346 différentes, 2 identiques.**

Les deux identiques sont **la même carte** — `rules_01_face.png`, la couverture
des Règles, dans les deux langues. C'est le **contrôle interne** de l'instrument :
une carte qui ne doit pas bouger ne bouge pas, donc la différence n'est pas un
artefact de run. Elle établit aussi que le run est déterministe, et que l'écart
n'est pas universel — il dépend du gabarit.

---

## 4. L'écart Memo : mesuré, bissecté, et **il ne vient pas de la substitution**

`memo_face_face.png` change de **364 314 px** (`fr`) et **464 136 px** (`ru`)
alors que son gabarit est **identique octet pour octet** dans les deux arbres
(`sha256 94123331A3556CF7`) et qu'il ne référence **aucune** des familles
substituées. Cinq runs sur le même arbre, Memo seul, ont isolé la cause :

| Run | Ce qui est injecté | `memo_face_face.png` (`fr`) |
|---|---|---|
| AVANT (arbre `4da09a14`) | — | référence |
| APRÈS, injection **coupée** | rien | **identique à AVANT** |
| APRÈS, injection active | 11 `@font-face` + WOFF2 réels (194 344 octets ; **259 144** caractères de base64) | différent : 364 314 px |
| APRÈS, **déclarations seules** | 11 `@font-face`, `src` = `"AAAA"` (4 octets) | **identique au run actif** (même compte, même boîte) |
| APRÈS, **marqueur seul** | le commentaire `/* argumentum-embedded-fonts */` | **identique à l'injection coupée** |

Trois conclusions, toutes mesurées :

1. **Le commentaire est inerte** — il ne change rien, dans aucune langue.
2. **Ce ne sont ni la taille ni les octets de police** : une charge invalide de
   4 octets reproduit l'écart à l'identique.
3. **La présence des déclarations suffit**, sur une carte qui ne les référence
   pas.

### Ce qui change n'est pas la police, c'est la mise en page

Mesuré sur `memo_back.png`, recadrage 1:1 à 3× : la **face** des titres de
rubrique est identique de part et d'autre ; ce qui change, c'est le **rythme
vertical** — les items sont repoussés vers le bas, la ligne « Argument vide »
sort du cadre qu'elle occupait. Même lecture sur `memo_face` par les hauteurs
d'encre :

| Bande de texte | AVANT | APRÈS |
|---|---:|---:|
| en-tête | 157 px | 157 px (inchangée) |
| bloc haut | 104 px | 62 px |
| bloc de corps | 99 px | 112 px |
| bloc de corps | 105 px | 114 px |

Et sur `memo_back`, l'écart AVANT→APRÈS couvre **43,2 % de la carte** (`fr`) /
42,1 % (`ru`, boîte `[0,0,826,1335]`, carte entière), alors que le même arbre
**sans** les déclarations ne diffère de AVANT que sur une bande de 75 px de haut
(`bbox=[2,60,818,135]`) — c'est-à-dire la substitution réelle, et rien d'autre.

### Mécanisme : non établi, et invisible aux instruments du DOM

Le chemin de capture, relu dans le code — cette lecture **corrige** celle du premier
jeton de ce dossier (contre-revue po-2024, c.7202574459) :

1. `frame.js:204` appelle `domtoimage.getFontsBefore()` → `fontFaces.resolveAll()`
   → `readAll(document)` ;
2. `selectWebFontRules` (l. 909-917) garde les règles `CSSRule.FONT_FACE_RULE` dont
   `src` passe `inliner.shouldProcess` (l. 824) — un test de **présence d'un `url(`**,
   rien de plus : une règle `data:` **passe** ;
3. les 11 règles injectées sont donc retenues ; `inlineAll` (l. 858) les rend telles
   quelles (`isSrcAsDataUrl`), et `embedFontsAlreadyDownloaded` (l. 533-541) réémet
   le tout dans le clone, base64 comprise, pour **chaque** CardSet.

Ce que ce chemin ne peut pas expliquer : au run « déclarations seules » (`src` =
`"AAAA"`, sans `url(`), le filtre de l'étape 2 **écarte** les règles — le clone n'en
reçoit aucune — et l'écart est reproduit à l'identique. La présence des règles
**dans le clone** n'est donc pas la variable ; celle qui reste mesurée est leur
présence **dans le document vivant**.

Candidat nommé, **non établi** : `frame.js:205-207` exécute trois passes
d'ajustement sur le DOM vivant immédiatement après `getFontsBefore()` —
`autoFitCardTitles`, `autoFitBodyText`, `autoFitFamilyGrid` (cette dernière porte
explicitement la grille du dos Memo). Ce sont elles qui déplacent le rythme
vertical, et elles mesurent à l'instant où le document vient de recevoir les
règles. Aucun rendu n'a été rejoué pour départager ce candidat d'un autre.

Le mécanisme exact n'est **pas** établi ; ce qui l'est, c'est que l'artefact est
invisible aux instruments qui lisent le DOM :

- la sonde de polices rend le **même** ensemble de replis des deux côtés ;
- le différentiel de débordement est **identique** (6/6 rapports, 0 ligne de
  différence) ;
- `[FONT-LOAD-FAILURE]` = 0 et `[FONT-PROBE]` = 0.

### Conséquences pour la revue

1. **L'injection n'est pas neutre pour les CardSets qu'elle ne vise pas — corrigé
   le 26/11.** `EmbeddedFontFaces.Inject` n'ajoute plus les règles à un document
   dont le CSS ne nomme aucune des trois familles (`ReferencesAnEmbeddedFamily`),
   et un test de corpus l'y tient : sur les 12 gabarits hors `Archive/`, **10**
   reçoivent les 11 règles et **2** n'en reçoivent aucune
   (`Argumentum_Memo_Face_fr.json`, et `Argumentum_Fallacies_Face_3_fr.json`, que
   nul document activé ne récolte). Sur la **face** Memo, l'effet est celui du run
   « injection coupée », mesuré identique à AVANT — **dérivé**, rendu non relancé.
   Sur le **dos** Memo, **aucun** effet : son CSS nomme `'Barlow', 'Rubik'` (il
   déclarait `'DINPro'` avant), donc les règles y restent et son écart de 43,2 %
   demeure, mêlé à la substitution. La restriction répond à « ne pas toucher ceux
   qu'elle ne vise pas », pas à « expliquer le dos Memo ».
2. **Les rendus de ce dossier mélangent deux causes** — la substitution voulue
   (DINPro → Barlow) et cet artefact d'injection — pour tous les CardSets sauf
   Memo, où l'artefact est isolé à l'état pur. Un verdict visuel « Barlow est-elle
   la bonne substitution ? » se lit donc sur les planches **Memo** sans réserve,
   et sur les autres **sous réserve** de cet écart.
3. **Aucune porte de non-régression du rendu.** Rien dans la suite de tests ne
   détecte cet écart ; c'est un manque, pas une garantie.

---

## 5. Sonde de polices (#2390) — état des deux côtés

La sonde rapporte, par lettre et par nœud, quelle famille a réellement rendu.
**Les deux côtés ont le même ensemble de replis** : `"Bebas Neue", sans-serif` en
`ru` (Fallacies 636 nœuds, Scenarii 334, Virtues 331, Rules 1), Memo
`Dosis`/`Gadugi` — aucune de ces piles n'est touchée par la PR.

Les piles **substituées** sont propres côté après :

| Langue | Rendu par | Fallacies | Virtues | Rules |
|---|---|---:|---:|---:|
| fr | Barlow | 175/175 + 171/171 | 262/262 + 131/131 | 107/121 + 52/52 |
| ru | Rubik | 175/175 + 171/171 | 262/262 + 131/131 | 102/102 + 52/52 |

Le critère d'acceptation du pool — « police absente ⇒ alerte » — **n'a pas
déclenché**. `[FONT-FALLBACK]` : 11 lignes avant (6 CardSets) contre 8 après
(4 CardSets), cohérent avec les rapports QA.

⚠️ La sonde lit le **DOM** ; elle est structurellement aveugle à l'artefact du
§4, qui ne vit que dans l'image capturée. Un instrument qui interroge le DOM ne
peut pas, par construction, blanchir un rendu.

---

## 6. Planches

Une planche par carte (`AVANT | APRÈS | masque des pixels différents`) et, quand
l'écart est localisé, un recadrage 1:1 des deux côtés.

| # | Carte | Pourquoi elle | Écart |
|---|---|---|---|
| 01 | `ru/Fallacies/card_001.png` | dos Fallacies, cyrillique | 94 915 px (8,1 %) |
| 02 | `fr/Fallacies/card_001.png` | dos Fallacies, latin | 86 001 px (7,3 %) |
| 03 | `ru/Fallacies/…пустой_аргумент_face.png` | carte substituée, `ru` | 61 698 px (5,3 %) |
| 04 | `fr/Fallacies/…argument_vide_face.png` | carte substituée, `fr` | 52 395 px (4,5 %) |
| 05 | `ru/Virtues/…аргумент,_основанный_на_фактах_face.png` | Virtues, `ru` | 289 635 px (24,7 %) |
| 06 | `fr/Virtues/…argument_fondé_face.png` | Virtues, `fr` | 62 213 px (5,3 %) |
| 07 | `ru/Rules/rules_01_face.png` | **contrôle : identique** | 0 |
| 08 | `ru/Scenarii/…мать_цезаря_и_клеопатры_face.png` | Scenarii, `ru` | 122 903 px (15,6 %) |
| 09 | `ru/Memo/memo_back.png` | **artefact isolé** (§4) | 492 293 px (42,1 %) |
| 10 | `fr/Memo/memo_back.png` | **artefact isolé** (§4) | 505 896 px (43,2 %) |
| 11 | `fr/Fallacies-Web/…argument_vide.png` | variante Web (400 dpi) | 23 186 px (3,8 %) |

Fichiers : `NN-<lang>-<dossier>-planche.jpg` (320–600 Ko) et
`NN-<lang>-<dossier>-detail.png` (recadrage 1:1, 50–940 Ko).

---

## 7. Ce que ce dossier ne dit pas

- **Aucun verdict visuel.** La question « Barlow est-elle la bonne substitution
  pour DINPro ? » est ouverte (choix (a)/(b)/(c) porté par `ai-01`) ; les mesures
  disent *ce qui change*, jamais *si c'est bon*.
- **Aucune couverture `en`/`es`/`pt`/`ar`/`fa`/`zh`.** Deux langues suffisent à
  couvrir les deux écritures en jeu (latin, cyrillique) ; les autres n'ont pas
  été rendues.
- **Aucun test automatisé de non-régression du rendu** (cf. §4.3).
- **L'effet de la restriction d'injection sur le rendu** : la face Memo n'a pas été
  re-rendue après la correction du 26/11 (effet dérivé du run « injection coupée »),
  et le dos Memo garde son écart.

---

## 8. Corrections du 26/11 (contre-revue po-2024, c.7202574459)

- **11 `@font-face`, pas 12** : c'est ce que `EmbeddedFontFaces.Faces` déclare et
  ce que `Cards/fonts/` vend. Le chiffre 12 figurait trois fois (§1, §4) ; les runs
  n'ont pas été rejoués — c'est un compte faux, pas une mesure fausse.
- **La charge en base64** : 194 344 octets → **259 144** caractères tels que le CSS
  les porte (chaque fichier encodé séparément, donc rempli séparément). La
  dérivation sur le total, ⌈194 344 / 3⌉ × 4, donne 259 128 : les deux nombres
  mesurent la même charge, à 16 caractères de remplissage près. « ~1,5 Mo » était
  faux d'un facteur ~8.
- **Lecture de code du §4** : les règles `data:` passent le filtre de sélection,
  seul l'*inlining* est sauté, et les règles sont réémises dans le clone pour
  chaque CardSet — le §4 le dit maintenant, avec ses lignes. La conséquence
  « restreindre l'injection » est appliquée (§4, *Conséquences*, point 1).
- **Ce que la restriction laisse ouvert** : le dos Memo. Un rendu rejoué avec la
  restriction, sur ce seul gabarit, départagerait « les règles dans le document »
  d'un autre candidat ; il n'a pas été lancé.
- **La garde du hook (`HarvestManager_InjectsTheFacesIntoTheCssItSerialises`)** : elle
  lisait le fichier brut, donc restait verte sur un appel commenté. Elle lit maintenant
  le code sans ses commentaires — littéraux copiés tels quels, y compris une chaîne
  interpolée avec les littéraux de ses trous — et porte un contrôle inverse : le même
  fichier avec l'appel commenté doit lui échapper. Mesuré sur le fichier réel le 26/11
  avec sauvegarde et restauration par empreinte : verte sur l'appel commenté avant,
  rouge maintenant.
- **Les empreintes des 11 fichiers** : SHA-256 épinglés dans `EmbeddedFontFaces`,
  vérifiés à la construction du CSS, et tenus égaux à la table de
  `Cards/fonts/README.md` par un test. Un octet retourné passé l'en-tête `wOF2`
  allumait 0 test ; il lève maintenant une exception qui nomme le fichier.

---

*myia-po-2023*
