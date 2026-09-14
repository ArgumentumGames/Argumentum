# Apostrophes, 8 langues — état re-dérivé, coût QA, convention cible

**Auteur** : po-2024 (worker) · **Date** : 2026-09-14 · **Base** : master `21a72385`
*(`origin/master` a avancé de 3 commits depuis — `874a5d98`, `836d9274`, `26aeaedd` — tous
**docs-only** : le recensement CSV est donc inchangé **par construction**.)*
**Statut** : **MESURE / DOSSIER** — lecture seule. ⛔ `0` écriture CSV, `0` écriture OWL, `0` régénération.
**Révision** : `2` — corrections de la revue ai-01 du 14/09 (6 points + 2 raccords durables ; détail
par point en §2, §2.1, §3, §4, §5, §6 et §8).
**Dispatch** : ai-01, deep-queue #458 c.5656689863, grain **③** (`Apostrophes 8 langues — mesure et coût QA`).
**Convention source** : PR **#1073** `3f7c95ec` — *« normalize apostrophes FR-curly EN-straight in text columns (corpus sweep) »*, mergée 2026-08-13 sur GO owner `msg-20260813T112921-finz50`. Antériorité : #1020/#1021/#1025 (FR), #1030 (EN).

---

## §0 Le résultat en une phrase

**La passe d'apostrophes n'a jamais couvert les colonnes FR de Scenarii** (392 occurrences, **138 des 167 cartes**), **ni la colonne `AIF_criticalQuestion` de Virtues** (139 cellules, qui partent dans l'ontologie) ; **Fallacies FR est close à 0** sur tout ce qui s'imprime.

Trois chiffres, dans cet ordre d'importance :

| | mesure |
|---|---:|
| **Fermeture Fallacies FR** (colonnes rendues) | **0** — close ✅ |
| **Résiduel Scenarii FR** (colonnes rendues) | **392 occurrences / 138 lignes sur 167** (`82,6 %` du deck) |
| Colonne oubliée qui atteint un artefact publié | **`AIF_criticalQuestion` = 139 occurrences sur 102 cellules FR** → `rdfs:comment` dans `argumentum_virtues.owl` |

---

## §1 Convention cible (proposition — l'owner tranche)

La règle **existe déjà** et n'a pas besoin d'être inventée : **FR = `’` (U+2019), EN = `'` (U+0027)** (#1073). Ce qui manquait n'était pas la règle mais **son périmètre**. La proposition porte donc sur les cas que #1073 n'a pas listés :

| surface | règle proposée | pourquoi |
|---|---|---|
| **FR** — colonnes rendues **et** colonnes émises | `’` | #1073 ; fallacies FR y est déjà à 0/2 984 |
| **EN** | `'` | #1073 ; **1 161** `'` légitimes dans les colonnes Fallacies **rendues** (dont 765 dans `example_en` seul) — `Simple_name_en` **exclu** (sans lecteur, cf. la règle du §5) |
| **`link_*` (toutes langues)** | ⛔ **exempt** — jamais touchées | ce sont des **URLs** : `…/Argumentation#Types_d'arguments`, `…/Biais_cognitifs#Biais_d'attribution`. Normaliser **casse le lien** |
| **`Remarques`, `proverbe`, `exemple politique`, `KIDZ`, `image`** | ⛔ **exempt** | **aucun générateur ne les lit** (mesuré : `.Proverbe`/`.ExemplePolitique`/`.Remarques` n'apparaissent nulle part hors du `ClassMap` de `Fallacy.cs`) — les normaliser ne changerait **aucun octet publié** |
| **`AIF_criticalQuestion`** | `’` (**à faire**) | **est émis** — `VirtueOwlGeneratorConfig.cs:224` écrit `new RDFPlainLiteral(AIFCriticalQuestion, "fr")` en `rdfs:comment`. 139 cellules droites aujourd'hui |
| **ru** — `д'Арк` (1) · **pt** — `d'água`, `Joana d'Arc` (8+3) · **es/ar/zh** | **arbitrage owner** | ce ne sont **pas** des résidus FR : ce sont des élisions légitimes de ces langues, des emprunts au français, ou des **guillemets** (`'自由'`, `'علم المناخ'` — autre classe de caractère, autre question) |
| **guillemets simples** (`'自由'`, `'علم المناخ'`) | **hors périmètre** de cette convention | un guillemet n'est pas une élision ; les traiter demande sa propre règle (`‘…’` en zh, `«…»` en ar) |

---

## §2 État re-dérivé — colonnes **rendues**, par langue

Apostrophes **intra-mot** (lettre·apostrophe·lettre), mesurées sur `21a72385`. `R` = la colonne est rendue par le gabarit (mustache extrait des JSON de `Cards/`).

| fichier | lg | droite | courbe | lignes | lecture |
|---|---|---:|---:|---:|---|
| **Scenarii** | **fr** | **392** | 61 | **138 / 167** | ❌ jamais balayé |
| Scenarii | en | 142 | **11** *(12 brut)* | — | ✅ contractions EN (`don't`, `we'll`…) — ⚠️ **11 en intra-mot**, 12 avec `context` compté brut (§3) |
| Scenarii | ru | 3 | 0 | 2 | `д'Арк`, `trompe-l'œil` |
| Scenarii | pt | 3 | 2 | 2 | `d'Arc` |
| **Fallacies** | **fr** | **0** | **2 984** | **0** | ✅ **close** |
| Fallacies | en | **1 161** | **9** | — | ✅ colonnes **rendues** seules ; `Simple_name_en` (5 `'`) **exclu** — il n'a **aucun lecteur** (§5, même règle que `Remarques`) |
| Fallacies | pt | 8 | 0 | 6 | `d'água`, `d'honneur`, `d'Arc` |
| Fallacies | zh | 10 | 3 | 6 | guillemets `'自由'` |
| Fallacies | es | 3 | 0 | 1 | `bras d'honneur` |
| Fallacies | ru | 1 | 0 | 1 | `д'Арк` |
| Fallacies | ar | 1 | 0 | 1 | guillemets `'علم المناخ'` |
| **Virtues** | **fr** | **0** | **414** | **0** | ✅ **close** |
| Virtues | en | 63 | 2 | — | ✅ contractions EN |
| **Rules** | **fr** | **19** | 115 | **4 / 15** | ❌ jamais balayé |
| Rules | en | 18 | 0 | — | ✅ |
| **RulesPnP** | **fr** | **8** | 35 | **4 / 6** | ❌ jamais balayé |
| RulesPnP | en | 4 | 0 | — | ✅ |

### §2.1 Fermeture Fallacies FR — **0**, et voici pourquoi c'est vrai

Les **six** colonnes FR que le gabarit imprime portent **0** apostrophe droite :

| colonne | droite | courbe |
|---|---:|---:|
| `example_fr` | **0** | 1 618 |
| `desc_fr` | **0** | 1 110 |
| `text_fr` | **0** | 155 |
| `Sous-Famille` | **0** | 57 |
| `Soussousfamille` | **0** | 44 |
| **`Famille`** | **0** | **0** |

⚠️ **Correction du 14/09** : cette table en listait **cinq** et omettait **`Famille`** — alors que le
§5 (FP #2) la citait, elle, dans les six colonnes rendues. Vérifié sur les gabarits
(`Argumentum_Fallacies_Face_fr.json`, `Face_2`, `Face_3`, `Face_Web`) : `Famille` **est** référencée.
Elle porte 0 des deux côtés, donc le total « 0 » est inchangé — mais une table de fermeture qui
oublie une colonne rendue ne prouve rien de cette colonne.

Ce qui reste en FR dans Fallacies (389 occurrences) vit dans **trois colonnes que rien n'émet** — `Remarques` 290, `proverbe` 56, `exemple politique` 43 — plus `link_fr` (3, URLs). **Une passe « FR → courbe » sur ce fichier n'a donc aucun effet publié.**

### §2.2 Résiduel Scenarii FR — le deck entier

| colonne FR rendue | occurrences | cellules |
|---|---:|---:|
| `contexte` | 129 | 84 |
| `enjeu` | 102 | 78 |
| `suggestion` | 97 | 70 |
| `piocheur` | 22 | 21 |
| `titre` | 21 | 20 |
| `baratineur` | 21 | 20 |
| **union des lignes** | **392** | **138 / 167** |

Ce n'est pas un résidu : c'est **le deck**. #1073 n'a touché Scenarii que par sa colonne **`suggestion_en`** (le tableau de sa propre PR ne liste que celle-là) — les six colonnes FR n'ont jamais été dans le périmètre.

### §2.3 Contrôle inverse — le résidu est de la même nature que ce qui a été corrigé

Deux vérifications indépendantes, et elles tombent juste :

| ce qui a été corrigé ailleurs | ce qui reste, au même endroit |
|---|---|
| Fallacies FR `text_fr`/`desc_fr`/`example_fr` : **0 droite / 2 984 courbe** | Scenarii FR `titre`…`suggestion` : **392 droite / 61 courbe** — mêmes mots, même langue, même élision (`l'`, `d'`, `n'`, `qu'`) |
| Fallacies **ontologie** FR : **3** droite (= ses 3 `link_fr`) | Virtues **ontologie** FR : **143** droite = **139 `AIF_criticalQuestion` + 4 `link_fr`** |

Le second point se referme à l'unité près : `argumentum_virtues.owl` porte **143** apostrophes droites intra-mot en `xml:lang="FR"`, et le CSV porte exactement **139 + 4**. La colonne oubliée **est** le résidu de l'ontologie — il n'y a rien d'autre dedans.

---

## §3 Le balayage inverse — `’` dans les colonnes EN

**22** apostrophes courbes **intra-mot** vivent là où la convention demande une droite :

| fichier | colonne | courbes |
|---|---|---:|
| Fallacies | `example_en` | 8 |
| Fallacies | `desc_en` | 1 |
| Virtues | `remark_en` | 1 |
| Virtues | `description_en` | 1 |
| **Scenarii** | **`context`** | **6** |
| **Scenarii** | **`title`** | **3** |
| **Scenarii** | **`issue`** | **1** |
| **Scenarii** | `suggestion_en` | 1 |
| **total** | | **22** |

⚠️ **Correction du 14/09 — deux erreurs dans la version antérieure.** Elle annonçait **12** et ne
listait de Scenarii que `suggestion_en`. Or :

1. **Scenarii en porte 11**, pas 1 : `context` **6**, `title` **3**, `issue` **1**, `suggestion_en`
   **1**. Le tableau **contredisait le §2**, qui donnait déjà 12 pour Scenarii en — et Scenarii en
   est précisément la colonne EN **rendue** (ses 8 colonnes EN, §5).
2. Le total réel est donc **22** (Fallacies 9 + Virtues 2 + **Scenarii 11**), pas 12.

Les 22 sont **intra-mot** ; le **12** du §2 pour Scenarii en est le compte **brut** — l'écart d'une
unité est la courbe de `context` non flanquée de lettres. Les deux colonnes du §2 et du §3 sont
désormais dérivées de la **même** sonde.

**Ce que #1030 a réellement fait.** Le balayage inverse EN **a bien eu lieu** — mais sur
**87 cellules / 102 occurrences**, pas 1 146. Le chiffre **1 146** (881 cellules) vient de
**#1026**, la PR **en sens inverse** (`'`→`’`), **fermée sans merge** ; #1030 est son successeur
correct (`’`→`'`) et le dit noir sur blanc dans son propre corps (« 87 cellules au lieu de 1146,
~13× plus petit »). #1030 n'a par ailleurs porté que sur les **6 colonnes EN des deux taxonomies**
(Fallacies `text_en`/`desc_en`/`example_en`, Virtues `title_en`/`description_en`/`remark_en`) —
**jamais Scenarii**. C'est pourquoi le solde de Scenarii en est resté à 11.

⇒ Le solde réel est **22 occurrences**, dont **11 en Scenarii** — la partie Scenarii n'était pas
« le reste de #1030 », elle n'a **jamais été dans son périmètre**.

---

## §4 Coût QA — ce qu'une mutation invaliderait dans le bundle validé

**Bundle de référence** : `review-v2.0.0-regen-20260912` — **80 PDF** (8 langues × 10 types), 3 745,6 Mo, manifeste `80/80` sha256, base `65dd4742` (`docs/release-dossier/134-bundle-verification-recipes.md`).

**Correspondance carte → PDF** (lue dans `package-v2.0.0.ps1`, `$types`) :

| jeu touché | PDF de la langue qui changent |
|---|---|
| **Fallacies** | `TarotCards`, `TarotCards_Print&Play_A4`, `TarotCards_Print&Play_Light_A4`, `Fallacies_Web_A0`, `Fallacies_Web_A4`, `Fallacies_Web_Thumbnails_A4` → **6** |
| **Scenarii** | `PokerCards`, `PokerCards_Print&Play_A4`, `PokerCards_Print&Play_Light_A4` → **3** |
| **Rules** | `TarotCards`, `TarotCards_Print&Play_A4`, `TarotCards_Print&Play_Light_A4` → **3** |
| **Virtues** | `TarotCards_Virtues`, `TarotCards_Print&Play_A4`, `TarotCards_Print&Play_Light_A4` → **3** |
| **RulesPnP** | `TarotCards_Print&Play_A4`, `TarotCards_Print&Play_Light_A4` — **déjà comptés** via Rules (pas de PDF propre) → **0** |

⚠️ **Correction du 14/09 — Virtues atteint TROIS PDF, pas un.** La version antérieure écrivait
`TarotCards_Virtues → 1`. Une carte Virtues se rend dans **`TarotCards_Virtues`** *et* dans
**`TarotCards_Print&Play_A4`** *et* **`TarotCards_Print&Play_Light_A4`** — les deux derniers étant
partagés avec les autres jeux. **L'union des 52 PDF masquait l'erreur** : comme ces deux fichiers
sont déjà comptés pour Fallacies/Rules, la ligne Virtues « 1 » ne changeait pas le total — elle
était fausse **sans être visible**. C'est la raison pour laquelle le tableau se lit **par jeu**,
pas seulement par total.

### §4.1 Branche « FR seule » — la moins chère

| langue | cartes dont le pixel change | PDF dont le sha256 change |
|---|---:|---:|
| **fr** | **146** (Scenarii 138 + Rules 4 + **RulesPnP 4**) | **6** : Poker ×3 + Tarot ×3 |
| | | **6 / 80 = 7,5 %** |

⚠️ **Correction du 14/09** : la version antérieure annonçait **142** et omettait **RulesPnP FR**
(8 occurrences droite sur 4 des 6 cartes, §2). Le compte FR est **146**. Les PDF sont inchangés —
RulesPnP n'a pas de PDF propre, il se rend dans les deux `TarotCards_Print&Play_*` déjà comptés
pour Rules.

Les **mindmaps ne bougent pas** : les libellés de famille FR sont à 0, et Scenarii/Rules n'alimentent aucune carte mentale. C'est précisément ce que la fermeture de Fallacies FR protège — les 24 SVG restent valides.

### §4.2 Branche « 8 langues » — si l'on normalise aussi les autres

| lg | cartes | fichiers sources | PDF |
|---|---:|---|---:|
| fr | **146** | Scenarii 138 + Rules 4 + RulesPnP 4 | 6 |
| en | **18** | Fallacies 5 + Virtues 2 + Scenarii 11 | **3** *(Virtues seul — voir ⚠️)* |
| pt | 8 | Fallacies 6 + Scenarii 2 | 9 |
| ru | 3 | Fallacies 1 + Scenarii 2 | 9 |
| zh | 6 | Fallacies 6 | 6 |
| es · ar | 1 · 1 | Fallacies | 6 · 6 |
| fa | 0 | — | 0 |
| **total** | **183** | | **52 / 80 = 65 %** |

⚠️ **Correction du 14/09 — les trois comptes sont faux dans la version antérieure**, qui annonçait
**fr 142 · en 12 · total 173**. Re-mesurés par union **par langue × fichier × colonne rendue** :

| branche | annoncé | **mesuré** | ce qui manquait |
|---|---:|---:|---|
| **(A)** FR seule | 142 | **146** | **RulesPnP FR** (4 cartes, §2) |
| **(B)** A + balayage inverse EN | 154 | **164** | l'union EN rend **18** cartes, pas 12 : `context` (6) + `title` (3) + `issue` (1) de Scenarii, plus Fallacies 5 et Virtues 2 |
| **(C)** 8 langues | 173 | **183** | les deux ci-dessus |

⚠️ **Le total (C) se calcule par langue, pas par union de PK.** Une même `coordonnée` (`5,0101`)
désigne une carte **FR** *et* une carte **EN** — deux objets physiques, deux PDF. Sommer les
ensembles de PK inter-langues écraserait 10 cartes ; c'est exactement ce que fait une lecture
« union » et c'est ce qui masquait les 4 RulesPnP. **146 + 18 + 8 + 3 + 1 + 1 + 6 = 183.**
Les **totaux PDF 6 / 16 / 52 restent inchangés** — RulesPnP n'a pas de PDF propre.

⚠️ **Cette branche n'est pas « la même chose en plus grand »** : pour ru/pt/es, elle ne corrige pas un défaut, elle **change une convention de langue** (`d'água` est correct en portugais ; `д'Арк` est une graphie russe répandue). C'est un arbitrage éditorial, pas un nettoyage.

### §4.3 Ce que le coût QA n'est pas

- **Pas** une régénération complète 8 langues. Le pipeline écrit par `{lang}/` : la branche FR seule ne régénère **que** `fr/`, soit **1/8** du coût.
- **Pas** une re-publication du bundle. Le manifeste est à `80/80` sur la base `65dd4742` ; une mutation FR périme **6 de ses 80 lignes**, pas le manifeste.
- **Mais** un verdict visuel est invalidé sur ces 6 PDF — et le verdict visuel est **ai-01**, pas le worker.

---

## §5 Les deux faux positifs d'instrument — à ne pas refaire

Ce dossier a d'abord produit **deux chiffres faux**. Ils sont nommés ici parce qu'ils se lisent tous les deux comme des résultats.

**FP #1 — la langue d'une colonne n'est pas dans son nom.** Une première sonde attribuait à `fr` toute colonne sans suffixe `_xx`. Or :

- Fallacies porte `Famille` (FR) **et** `Family` (EN) — un `endswith` insensible à la casse range `Family` dans FR ;
- Scenarii porte **sept colonnes EN nues** — `category`, `subcategory`, `title`, `smoothTalker`, `drawer`, `context`, `issue` — **plus une suffixée** (`suggestion_en`) : **8 colonnes EN au total**, dont 7 que la sonde rangeait en FR. *(La version antérieure en annonçait « huit nues » et comptait `suggestion_en` à part — elle le décrivait comme nues alors qu'il porte le suffixe.)*
- Virtues porte `AIF_criticalQuestion`, `KIDZ`, `card`, `update`, `locked` — sans suffixe, donc « français » pour la sonde.

Le premier passage annonçait donc **Fallacies FR = 404** et **Scenarii FR = 455**. Le vrai chiffre est **0** et **392**. La sonde était fausse de 404 cellules dans un sens et de 63 dans l'autre — *dans les deux sens à la fois, sur le même fichier de sortie.*

**FP #2 — tout ce qui est dans le CSV n'est pas sur une carte.** Le premier passage comptait `Remarques`, `proverbe`, `exemple politique`, `image`, `link_fr` comme du texte de carte. Ils ne le sont pas : le mustache des gabarits Fallacies rend **six** colonnes FR — `Famille`, `Sous-Famille`, `Soussousfamille`, `text_fr`, `desc_fr`, `example_fr` — et leurs **trois homologues EN** (`text_en`, `desc_en`, `example_en`), **et rien d'autre**. `.Proverbe`/`.ExemplePolitique`/`.Remarques`/`Simple_name_en` n'ont **aucun lecteur** dans `Generation/`. Sans ce tri, la « fermeture Fallacies FR » se chiffrait à 391 au lieu de **0**, et la ligne EN à 1 166 au lieu de **1 161** — c'est la **même** règle qui produit les deux corrections.

**La règle qui en sort** : une mesure d'apostrophes se scope à **(fichier × colonne × langue × rendu-ou-émis)**, jamais au couple (fichier × langue). Le tri « rendu / non rendu / URL » n'est pas un raffinement — c'est ce qui sépare 0 de 391.

---

## §6 Feuille de décision (une feuille)

| | |
|---|---|
| **La question** | La convention FR=`’` / EN=`'` étant acquise, **jusqu'où l'appliquer** ? |
| **Le fait mesuré** | Quatre surfaces ne l'ont jamais reçue : Scenarii FR (**392 occ., 138/167 cartes**), Rules FR (19), **RulesPnP FR (8)**, `AIF_criticalQuestion` (**139 occurrences sur 102 cellules**, émis dans l'OWL). Fallacies FR et Virtues FR sont **déjà à 0** là où ça s'imprime. |
| **(A) FR seule — Scenarii + Rules + RulesPnP + `AIF_criticalQuestion`** | **146 cartes · 6 PDF / 80 · 1 OWL** (régén **`fr/` seule**). Supprime le seul défaut *visible sur une carte*. **Recommandé** : même règle, même langue, aucune convention étrangère touchée. |
| **(B) FR + le balayage inverse EN (22 courbes)** | (A) **+ 18 cartes · +10 PDF / 80**. Le lot le moins cher par unité — **#1030 avait déjà fait 87 cellules / 102 occurrences** sur les 6 colonnes EN des deux taxonomies ; il en reste **11** dans ces colonnes, **plus les 11 de Scenarii**, qui n'ont **jamais été dans son périmètre** (§3). |
| **(C) 8 langues** | **183 cartes · 52 PDF / 80 (65 %)** · régén complète. ⚠️ **Ne se justifie pas comme un nettoyage** : pour ru/pt/es cela **change une convention de langue** (`d'água` est correct en pt), pour zh/ar cela traite des **guillemets** — une autre question. |
| **(D) Ne rien faire** | 0 écriture. Le défaut visible reste : **138 des 167 cartes FR de Scenarii** portent au moins une apostrophe droite (392 au total, ≈ 2,8 par carte touchée), et l'OWL FR reste mixte (143 droites / 184 courbes). |
| **Nature** | Convention typographique — aucune branche ne corrige un contresens. |
| **Interaction avec les autres arbitrages** | **⚠️ Ordre d'exécution — il compte.** Le lot EN (B) recoupe la « moitié EN du GO » déjà tracée dans `DECISION-v2.0.0-jsboige.md` (PR B, 102 subst.). **Le GO voix EN #994 (47 cellules) doit passer EN PREMIER**, puis les apostrophes être re-mesurées : normaliser dans l'ordre inverse réécrirait des cellules que le GO va réécrire, et pourrait **réintroduire des courbes** dans le texte normalisé à la 3ᵉ personne. Les 22 ci-dessus sont donc un **solde à re-dériver après** le GO, pas un chiffre à graver. |
| **Ce qu'il faut savoir avant de trancher** | Le lot « FR seule » **n'invalide aucun mindmap** (les libellés de famille FR sont à 0) : c'est la fermeture de Fallacies FR qui rend la branche (A) sûre. Et `link_*` doit rester **exempt** dans toutes les branches — normaliser une URL la casse. |

---

## §7 Ce que ce grain n'a pas fait

⛔ `0` cellule CSV écrite · ⛔ `0` OWL régénéré ou modifié · ⛔ aucune régénération · ⛔ aucune branche choisie · ⛔ aucun verdict visuel rendu (c'est ai-01).

Mesure uniquement — le GO d'écriture est un geste séparé.

---

## §8 Le raccord durable sur #994 — l'erratum, pas seulement ce document

Ce dossier ne suffit pas à corriger l'état : **#994 porte un commentaire antérieur qui déclare les
apostrophes résolues par #1073.** C'est faux pour **Scenarii FR** et pour **RulesPnP FR** — et ce
document le prouve. Un fait qui ne vit que dans une PR ouverte n'est pas corrigé : l'erratum doit
être **gravé sur l'issue**.

**Ce qui part sur #994** (commentaire po-2024, 14/09) :

1. **#1073 n'a pas couvert Scenarii FR.** Sa propre table ne liste qu'une colonne de Scenarii,
   **`suggestion_en`** — donc une colonne **EN**. Les **six colonnes FR** (`titre`, `baratineur`,
   `piocheur`, `contexte`, `enjeu`, `suggestion`) n'ont **jamais** été dans son périmètre :
   **392 occurrences droite sur 138 des 167 cartes**.
2. **RulesPnP FR non plus** — **8 occurrences sur 4 des 6 cartes** — ni **Rules FR** (19 sur 4/15).
3. **`AIF_criticalQuestion`** n'est pas une colonne de carte mais **est émise** :
   **139 occurrences sur 102 cellules** partent en `rdfs:comment` dans
   `argumentum_virtues.owl` (contrôle inverse : l'OWL porte 143 droites FR = 139 + 4 `link_fr`).
4. **L'ordre d'exécution** : le GO voix EN #994 (47 cellules) passe **avant** toute normalisation
   d'apostrophes ; sinon on réécrit deux fois les mêmes cellules et on risque de réintroduire des
   courbes (§6, ligne « Interaction »).

⚠️ Rappel de périmètre : `link_*` reste **exempt** dans toutes les branches — ce sont des URLs.

---

*po-2024 — grain ③ du dispatch #458 c.5656689863. Le worker mesure et signale ; la décision reste à l'owner.*
