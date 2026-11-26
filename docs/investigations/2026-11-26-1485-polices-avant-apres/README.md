# #1485 — substitution DINPro → Barlow : rendus avant/après

**Branche** `feat/1485-embedded-fonts` · **PR** #2394 (brouillon) · **tête mesurée** `974319eb`
**Comparaison** `4da09a14` (avant) → `974319eb` (après)
**Portée mesurée** 6 CardSets × 2 langues (`fr`, `ru`), build **Debug**
**Date** 2026-11-26 · lane `myia-po-2023`

> ⚠️ Ce dossier porte des **mesures**, pas un verdict visuel. Le verdict visuel est
> réservé à `ai-01` (règle de la lane) : les planches ci-dessous lui sont destinées.

---

## 1. Ce que la branche change

| Objet | Nature |
|---|---|
| `Cards/Fallacies/Argumentum_Fallacies_Face_fr.json` | 4 blocs `@font-face` DINPro **distants** (cdnfonts) retirés ; `'DINpro',sans-serif` → `'Barlow', 'Rubik',sans-serif` (2 sélecteurs) |
| `WebBasedGenerator/Cardpen/EmbeddedFontFaces.cs` (nouveau) | 12 `@font-face` bâtis depuis des WOFF2 **embarqués** sous `Cards/fonts/`, émis en `data:font/woff2;base64,…`, `font-display: block`, `unicode-range` par sous-ensemble |
| `WebBasedGenerator/Cardpen/HarvestManager.cs` | **une** ligne de comportement : `css = EmbeddedFontFaces.Inject(css)` avant sérialisation du payload |
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
| APRÈS, injection active | 12 `@font-face` + WOFF2 réels (~1,5 Mo) | différent : 364 314 px |
| APRÈS, **déclarations seules** | 12 `@font-face`, `src` = `"AAAA"` (4 octets) | **identique au run actif** (même compte, même boîte) |
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

`frame.js:204` appelle `domtoimage.getFontsBefore()` → `resolveAll()` →
`readAll(document)` → filtre `CSSRule.FONT_FACE_RULE` → `inliner.shouldProcess(src)` ;
les règles retenues sont résolues puis **inlinées dans le clone** capturé. Les
`@font-face` injectés étant en `data:`, ils ne devraient pas passer ce filtre.
La mesure dit le contraire, et l'effet observé (rythme vertical, faces locales
inchangées) est celui d'une **perte de métriques de webfont dans le clone**.
Le mécanisme exact n'est **pas** établi ; ce qui l'est, c'est que l'artefact est
invisible aux instruments qui lisent le DOM :

- la sonde de polices rend le **même** ensemble de replis des deux côtés ;
- le différentiel de débordement est **identique** (6/6 rapports, 0 ligne de
  différence) ;
- `[FONT-LOAD-FAILURE]` = 0 et `[FONT-PROBE]` = 0.

### Conséquences pour la revue

1. **L'injection n'est pas neutre pour les CardSets qu'elle ne vise pas.** Elle
   doit être restreinte aux documents dont le gabarit référence une famille
   embarquée, ou l'écart Memo doit être accepté explicitement. Il ne peut pas
   être passé sous silence : aucun test ne le verrait.
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

---

*myia-po-2023*
