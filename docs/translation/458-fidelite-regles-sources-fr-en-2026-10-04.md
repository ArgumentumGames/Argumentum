# #458 — Fidélité des **Règles** : les sources d'abord (FR | EN), 0 écriture

**Objet** : `Cards/Rules/Argumentum Rules - Cards.csv` — **15 documents** de règles, **8 langues**.
**Portée de ce dossier** : la paire **source** FR | EN, lue intégralement, les 15 documents.
**Non lu, et dit tel quel** : les **6 langues cibles** (ru, pt, ar, es, zh, fa) — voir §6.
**Statut** : **0 écriture** — aucune cellule CSV modifiée.

---

## 1. Méthode — pourquoi les sources d'abord

La campagne des Scénarios a établi, sur 167 cartes, que **l'EN n'est pas une référence neutre** :
il dévie lui-même, et les écarts des langues cibles sont **hérités** plutôt que produits. Juger
une langue cible contre un EN non audité, c'est mesurer contre une règle qu'on n'a pas vérifiée.

Ce dossier fait donc **le pas qui vient avant** : lire les deux sources, nommer les défauts de
l'EN, et consigner les singularités du FR. Les 6 langues cibles se liront **après**, contre cette
référence-là.

⚠️ **Ce n'est pas un dossier de fidélité des Règles.** C'est l'audit de sa référence.

### Ce que « document » veut dire ici

Contrairement aux Scénarios (7 champs courts par carte), chaque cellule des Règles est un
**document markdown entier** (36 à 1569 caractères : titres `##`, listes `*`, emoji
`❌🏆➜✅1🎴`). Une carte = un document ; une comparaison cellule-à-cellule ne peut donc rien dire
d'un paragraphe. D'où la lecture intégrale, et l'écran **corrigé** (`regles-fidelity-instrument.py`,
PR #1755) relégué à ce qu'il sait faire : des nombres et des ponctuations de fin.

---

## 2. Écran (restitution, instrument corrigé)

```
=== totaux par signal === {'CHIFFRE': 6, 'DUEL': 6, 'MOT-NOMBRE': 1, 'PONCT': 8}
rangees : 15 | langues : 7 | cellules : 105 | remplies : 105
```

- **105/105 cellules remplies, 0 `=FR`** — aucune cellule de Règles n'est restée en français.
- **21 drapeaux = 13 faux positifs mesurés + 8 cellules réelles.**
- La seule famille réelle : **`PONCT`** — le dernier bloc de **Rules_08** et **Rules_15** sans
  ponctuation finale en **ar · es · zh · fa** (FR, EN, ru et pt y terminent par « . »).
- **Forme** : l'en-tête déclare 11 colonnes, **10 des 15 rangées n'en portent que 10**, le champ
  manquant étant **toujours le dernier** (`variant_class`, porté par les 5 couvertures) ⇒ l'écran
  lit juste ; deux gardes (`check_rows`, `check_scripts`) séparent ce cas de celui qui décalerait
  les colonnes. Détail : PR #1755, §6 du dossier de l'instrument.

⛔ **L'écran n'établit rien sur le sens.** Les 8 cellules `PONCT` sont un **fait typographique**,
pas un défaut de traduction : rien ne dit qu'un document arabe doive finir par un point.

---

## 3. Défauts **EN** établis

### 3.1 Pronoms genrés — **Rules_10, et cette cellule seule**

**Mesure** : sur les 15 cellules EN, **une seule** emploie des pronoms genrés —
`his` + `he`, deux occurrences, dans `Text_en` de **Rules_10** :

> « If a player manages to get rid of all **his** cards, **he** wins the game. »

Les **14 autres** cellules EN n'emploient que `they`/`their`/`them`. Et Rules_10 est
**incohérente avec elle-même**, quelques lignes plus haut :

> « If a player cannot find a new argument within 10 seconds, **they** draw a new card and are
> excluded from the round. »

⇒ **Défaut interne** : la même cellule dit `they` puis `he`. Le FR dit « il », donc la source
explique la forme — mais la convention de l'EN est `they` partout ailleurs, y compris dans cette
cellule.

### 3.2 Calque — **Rules_11**

FR : « À chaque manche, à partir d'un scénario tiré au sort, l'un des joueurs trouve… »
EN : « With each round, **on a scenario drawn**, one of the players finds in a limited time… »

`on a scenario drawn` est une transposition littérale de « à partir d'un scénario tiré au sort »,
**non idiomatique**. L'EN sait le dire ailleurs : Rules_02 « a **randomly drawn** scenario »,
Rules_03 « a randomly drawn scenario ». ⇒ Formulation propre à Rules_11, à corriger dans la
cellule.

### 3.3 Terminologie de « paquet » — **trois mots pour un, dont deux dans la même cellule**

**Mesure** sur le bloc EN (`Text_en`) :

| Mot | Cellules |
|---|---|
| `deck` | Rules_02 (×2), Rules_11 (×2), Rules_13 (×2) |
| `pack` | Rules_09 (×1), Rules_11 (×1) |
| `package` | Rules_09 (×1), Rules_11 (×1) |

⇒ Dans **Rules_09** et **Rules_11**, la **même cellule** dit `pack` **et** `package` pour le
même objet. Le FR dit « paquet » dans les deux cas, sans variation ⇒ **l'inconstance est née
dans l'EN**, pas héritée.

### 3.4 « classes de couleurs » — **trois rendus**

| Rendu | Cellule |
|---|---|
| `7 color-coded classes` | Rules_02 |
| `7 classes of colors` | Rules_09, Rules_11 |
| `7 color classes` | Rules_13 |

`classes of colors` est le seul des trois à n'être pas idiomatique (`color classes` /
`color-coded classes` le sont). Même remarque que 3.3 : le FR ne varie pas.

### 3.5 Ajout d'interprétation — **Rules_05** *(à juger, pas tranché)*

FR : « À défaut (❌🎭), les **jurés plébiscités** qui ont trouvé la carte du baratineur sont tous
déclarés vainqueurs »
EN : « Otherwise (❌🎭), all **tied** jurors who received the most votes and identified the smooth
talker's card are declared winners »

L'EN ajoute **`tied`**, que le FR ne dit pas (plébiscité = « qui a recueilli le plus de voix »).
Ce peut être une **clarification juste** du mécanisme, ou un **ajout**. ⛔ **Non tranché ici** :
il faudrait l'arbitrage du concepteur. Consigné pour ne pas être tranché en silence.

### 3.6 Cosmétique — **Rules_10**

- Ligne vide **double** après le titre `## End of game and count` (le FR n'en a qu'une).
- `Otherwise the game stops…` sans la virgule que le FR porte (« **Sinon, la partie s'arrête…** »).
- Capitalisation des titres : `### 1. The Reader` (R majuscule) contre
  `### 2. The smooth talker` (s minuscule) **dans la même liste**.

*(Vérifié au passage, pour ne pas l'inscrire à tort : `The Reader` en tête de phrase contre
`the reader` en milieu de phrase n'est **pas** une incohérence — c'est la position dans la
phrase. La mesure a évité un faux défaut.)*

---

## 4. Singularités de la source **FR** — à juger, **pas à corriger**

Le mandat de la campagne est explicite : une bizarrerie de la source se **consigne**, elle ne se
corrige pas d'office. En voici trois, relevées en lisant le FR.

### 4.1 **Rules_13 : 32 cartes annoncées, 28 demandées** — contradiction interne

- Matériel : « 1 paquet de cartes d'arguments fallacieux, organisé en 7 classes de couleurs,
  chacune répartie en 3 ordres puis en 3 familles. **Une sélection de 32 cartes** »
- Installation : « **Sélectionnez 28 cartes** d'arguments fallacieux pour la partie, **soit 4 par
  couleur**. »

7 classes × 4 = **28**. La ligne de Matériel annonce **32**. Et l'EN **recopie les deux**,
fidèlement (`A selection of 32 cards` / `Select 28 fallacious argument cards for the game, 4 per
color`) ⇒ **le défaut est dans la source, la traduction est fidèle** — et c'est exactement le
genre de cas qu'une passe sur l'EN seul aurait attribué à l'anglais.

### 4.2 **Rules_08 : « l'enregistrement » n'est jamais listé au Matériel**

Rules_08 dit « À partir de leurs notes **et de l'enregistrement**… », mais le Matériel de la même
famille de règles (Rules_07) ne liste que « De quoi prendre des notes **et un moyen de noter le
temps écoulé** » et un débat « qui pourra faire l'objet d'un **revisionnage** » — jamais un
enregistrement. L'EN dit `the recording`, fidèlement. ⇒ **Lâche dans la source**, pas dans la
traduction.

### 4.3 **Instructions en « vous » ici, à l'impératif là — pour la MÊME phrase**

- Rules_03 : « **vous constituez** la pioche… **Vous choisissez** la durée de la partie. »
- Rules_09 / Rules_11 : « **constituez** la pioche… **Choisissez** le nombre maximum de manches… »

Même geste, deux registres. L'EN **normalise** en impératif (`build the draw pile`, `Choose the
length` / `Choose the maximum number`) ⇒ ici l'EN **lisse** une variation du FR au lieu de la
recopier. À consigner : ce n'est ni un manque ni un contresens, c'est une **décision de
traduction**, et elle est plutôt heureuse.

*(Même famille, plus petit : « Cartes mémo**.** » avec point aux Rules_02/11/13, « Cartes mémo »
sans point aux Rules_09 — l'EN suit, `Memo cards.` / `Memo cards`.)*

---

## 5. Matrice — ce que cette lecture donne aux 6 langues cibles

Ce tableau n'est pas un verdict : c'est la **liste des choses à regarder** quand les 6 langues
seront lues, parce que chacune peut les avoir **héritées**.

| Signal établi en EN | Cellule EN | Ce qu'une langue cible hérite si elle suit l'EN |
|---|---|---|
| pronoms genrés | Rules_10 | `he`/`his` au lieu de `they` (le FR dit « il » — l'héritage peut venir du FR *ou* de l'EN) |
| calque « on a scenario drawn » | Rules_11 | une phrase non idiomatique |
| `pack` / `package` / `deck` mêlés | Rules_09, Rules_11, Rules_02, Rules_13 | la variation, ou la réparation |
| `classes of colors` | Rules_09, Rules_11 | idem |
| `tied` ajouté | Rules_05 | la précision, ou l'ajout |
| **32 vs 28** | Rules_13 (**et FR**) | ⚠️ **héritage neutre** : FR **et** EN disent 32 ⇒ une langue cible qui dit 32 **n'a rien inventé** |
| « l'enregistrement » non listé | Rules_08 | idem — source, pas traduction |
| ponctuation finale absente | Rules_08, Rules_15 | ⛔ **l'EN est propre ici** : l'absence en ar/es/zh/fa n'est **pas** héritée de l'EN |

⭐ La dernière ligne est la plus utile : la famille `PONCT` est mesurée **contre un EN qui
ponctue**. Elle ne peut donc pas être imputée à la source.

---

## 6. Ce que ce dossier **n'établit pas**

- ⛔ **La fidélité des 6 langues cibles.** Elles ne sont **pas lues**. Ce dossier est l'audit de
  la référence, et rien de plus. Un « 0 défaut » en ar, es, fa, pt, ru ou zh ne serait pas établi
  ici — et ne peut pas l'être par l'écran, qui ne voit ni un ajout ni un contresens.
- ⛔ **Que l'EN soit fautif partout où il se distingue du FR.** Trois des écarts relevés
  (§4.3, et les variantes lexicales de §3.3/§3.4) peuvent être des **choix de traduction
  légitimes**. Ce qui est établi est plus étroit : **l'EN varie là où le FR ne varie pas**,
  et **une cellule se contredit elle-même** (§3.1).
- ⛔ **Le statut de §3.5 (`tied`)** — ajout ou clarification : **non tranché**, et volontairement.
- ⛔ **Que les 8 `PONCT` soient un défaut.** C'est un fait typographique mesuré contre FR/EN
  (qui ponctuent) ; la conclusion appartient à la relecture native.
- ⛔ **La lecture des documents hors `Text_<lang>`.** La colonne `variant_class` (5 couvertures :
  `cover-argumentum`, `cover-bingo`, `cover-beau-parleur`, `cover-moulin`, `cover-parlote`) et
  `print_and_play` n'ont pas été lues ici.

---

## 7. Verdict et suite

- **Portée** : 15 documents × 2 sources = **30 cellules lues**, sur les **105** du corpus.
- **6 défauts EN** nommés, dont **un mesuré comme unique dans tout le bloc** (pronoms genrés,
  Rules_10) et **un calque** (Rules_11).
- **3 singularités de source FR** consignées, dont une **contradiction interne** (32 vs 28,
  Rules_13) que l'EN **recopie fidèlement** — le contre-exemple parfait au réflexe « la langue
  cible a fauté ».
- **Reste dû** : les **6 langues cibles** (ru, pt, ar, es, zh, fa), lues **contre cette
  référence-ci**, puis le dossier de fidélité complet.

---

## 8. Écrit — grain 4 du pool c.5988407613 (05/10)

**11 cellules écrites** dans `Argumentum Rules - Cards.csv` (chirurgie de fragments uniques avec
comptes assertés, jamais de round-trip CSV — le fichier porte un BOM et des cellules multi-lignes ;
delta 0, `numstat 15/16`, la ligne de moins = la ligne vide retirée dans Rules_10).

### 8.1 Rules_13 — 32 → 28, les 8 langues (GO owner 05/10 « oui à tout »)

La contradiction interne du §4.1 sort du corpus : la ligne de Matériel dit **28** comme
l'Installation (7 couleurs × 4), dans les 8 colonnes — `Text` («Une sélection de 28 cartes»),
`Text_en` ("A selection of 28 cards"), `Text_ru` («колода из 28 карт»), `Text_pt`, `Text_ar`,
`Text_es`, `Text_zh` (共选用28张牌) et `Text_fa` (chiffres persans ۳۲ → ۲۸). L'imprimé 2022 porte
le « 32 » FR et EN : la correction était **soumise à l'owner** (dossier #1773 volet 1.3) et il a
dit oui. **Le CSV Print&Play est mesuré** : il ne porte aucune ligne de sélection — rien à y
écrire (le GO disait « mesure aussi le CSV PP »).

### 8.2 EN — les défauts §3.3/§3.4/§3.2/§3.1 + deux cosmétiques §3.6

| Cellule | Avant | Après |
|---|---|---|
| Rules_09 `Text_en` | `1 pack of…` · `1 package of…` · `7 classes of colors` | `1 deck of…` ×2 · `7 color classes` |
| Rules_11 `Text_en` | idem Rules_09 + `on a scenario drawn` | idem + `from a randomly drawn scenario` (comme Rules_02/03) |
| Rules_10 `Text_en` | `all his cards, he wins` · `Otherwise the game stops` · ligne vide double | `all their cards, they win` · `Otherwise, the game stops` · une ligne vide |

« his/he » : la phrase n'a **jamais été imprimée** (vérifié par ai-01 sur l'archive 2022,
c.5988397998) et la même cellule dit déjà `they` plus haut — correction libre. **Le 3ᵉ
cosmétique du §3.6 (majuscules « The smooth talker ») n'existe pas dans la cellule mesurée** :
la liste réelle est « The Reader / The round of arguments / End of the round » — l'item est un
écart de mesure, consigné ici, non corrigé.

### 8.3 Gardé

- **Rules_05 `tied`** — GARDE (arbitrage ai-01 05/10) : le FR dit « tous déclarés vainqueurs »,
  donc plusieurs vainqueurs à égalité ; `tied` est **fidèle**. ⚠️ Ce n'est **pas** de la
  typographie. Épinglé par la garde (`Tied_Is_Kept_By_Arbitration`).

Aucune langue cible ne reproduit les défauts 8.2 : re-mesuré sur les lignes de Matériel de
Rules_09 — ru «колода», pt «baralho», es «mazo», zh 卡牌 uniformes dans la cellule ; ar unifié
par #1768 (رزمة) ; «он» en Rules_10 ru est la grammaire russe fidèle au « il » source, pas
l'incohérence interne EN. Garde : `RulesEnCorrectionsGuardTests` (7 faits, 11 épingles SHA-256,
écran des 15 formes déviantes couvrant **les deux** CSV, présence des formes restaurées,
mutation mesurée). Suite : 1477 + 7 = **1484 attendu**.

*po-2024*
