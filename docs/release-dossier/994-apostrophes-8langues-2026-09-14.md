# Apostrophes, 8 langues — état re-dérivé, coût QA, convention cible

**Auteur** : po-2024 (worker) · **Date** : 2026-09-14 · **Base** : master `21a72385`
**Statut** : **MESURE / DOSSIER** — lecture seule. ⛔ `0` écriture CSV, `0` écriture OWL, `0` régénération.
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
| Colonne oubliée qui atteint un artefact publié | **`AIF_criticalQuestion` = 139 cellules FR** → `rdfs:comment` dans `argumentum_virtues.owl` |

---

## §1 Convention cible (proposition — l'owner tranche)

La règle **existe déjà** et n'a pas besoin d'être inventée : **FR = `’` (U+2019), EN = `'` (U+0027)** (#1073). Ce qui manquait n'était pas la règle mais **son périmètre**. La proposition porte donc sur les cas que #1073 n'a pas listés :

| surface | règle proposée | pourquoi |
|---|---|---|
| **FR** — colonnes rendues **et** colonnes émises | `’` | #1073 ; fallacies FR y est déjà à 0/2 984 |
| **EN** | `'` | #1073 ; 1 166 `'` légitimes dans Fallacies (dont 765 dans `example_en` seul) |
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
| Scenarii | en | 142 | 12 | — | ✅ contractions EN (`don't`, `we'll`…) |
| Scenarii | ru | 3 | 0 | 2 | `д'Арк`, `trompe-l'œil` |
| Scenarii | pt | 3 | 2 | 2 | `d'Arc` |
| **Fallacies** | **fr** | **0** | **2 984** | **0** | ✅ **close** |
| Fallacies | en | 1 166 | 9 | — | ✅ dont **1 162 contractions EN**, 4 à vérifier |
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

Les cinq colonnes FR que le gabarit imprime portent **0** apostrophe droite :

| colonne | droite | courbe |
|---|---:|---:|
| `example_fr` | **0** | 1 618 |
| `desc_fr` | **0** | 1 110 |
| `text_fr` | **0** | 155 |
| `Sous-Famille` | **0** | 57 |
| `Soussousfamille` | **0** | 44 |

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

Douze apostrophes courbes vivent là où la convention demande une droite :

| fichier | colonne | courbes |
|---|---|---:|
| Fallacies | `example_en` | 8 |
| Fallacies | `desc_en` | 1 |
| Virtues | `remark_en` | 1 |
| Virtues | `description_en` | 1 |
| Scenarii | `suggestion_en` | 1 |
| **total** | | **12** |

#1030 avait fait ce balayage (1 146 substitutions) ; il en reste 12. C'est le lot le moins coûteux du dossier.

---

## §4 Coût QA — ce qu'une mutation invaliderait dans le bundle validé

**Bundle de référence** : `review-v2.0.0-regen-20260912` — **80 PDF** (8 langues × 10 types), 3 745,6 Mo, manifeste `80/80` sha256, base `65dd4742` (`docs/release-dossier/134-bundle-verification-recipes.md`).

**Correspondance carte → PDF** (lue dans `package-v2.0.0.ps1`, `$types`) :

| jeu touché | PDF de la langue qui changent |
|---|---|
| **Fallacies** | `TarotCards`, `TarotCards_Print&Play_A4`, `TarotCards_Print&Play_Light_A4`, `Fallacies_Web_A0`, `Fallacies_Web_A4`, `Fallacies_Web_Thumbnails_A4` → **6** |
| **Scenarii** | `PokerCards`, `PokerCards_Print&Play_A4`, `PokerCards_Print&Play_Light_A4` → **3** |
| **Rules** | `TarotCards`, `TarotCards_Print&Play_A4`, `TarotCards_Print&Play_Light_A4` → **3** |
| **Virtues** | `TarotCards_Virtues` → **1** |

### §4.1 Branche « FR seule » — la moins chère

| langue | cartes dont le pixel change | PDF dont le sha256 change |
|---|---:|---:|
| **fr** | **142** (Scenarii 138 + Rules 4) | **6** : Poker ×3 + Tarot ×3 |
| | | **6 / 80 = 7,5 %** |

Les **mindmaps ne bougent pas** : les libellés de famille FR sont à 0, et Scenarii/Rules n'alimentent aucune carte mentale. C'est précisément ce que la fermeture de Fallacies FR protège — les 24 SVG restent valides.

### §4.2 Branche « 8 langues » — si l'on normalise aussi les autres

| lg | cartes | fichiers sources | PDF |
|---|---:|---|---:|
| fr | 142 | Scenarii 138 + Rules 4 | 6 |
| en | 12 | Fallacies 9 + Virtues 2 + Scenarii 1 | 10 (6 + 1 + 3) |
| pt | 8 | Fallacies 6 + Scenarii 2 | 9 |
| ru | 3 | Fallacies 1 + Scenarii 2 | 9 |
| zh | 6 | Fallacies 6 | 6 |
| es · ar | 1 · 1 | Fallacies | 6 · 6 |
| **total** | **173** | | **52 / 80 = 65 %** |

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
- Scenarii porte **huit colonnes EN nues** (`category`…`issue`) plus une suffixée (`suggestion_en`) — les nues partaient en FR ;
- Virtues porte `AIF_criticalQuestion`, `KIDZ`, `card`, `update`, `locked` — sans suffixe, donc « français » pour la sonde.

Le premier passage annonçait donc **Fallacies FR = 404** et **Scenarii FR = 455**. Le vrai chiffre est **0** et **392**. La sonde était fausse de 404 cellules dans un sens et de 63 dans l'autre — *dans les deux sens à la fois, sur le même fichier de sortie.*

**FP #2 — tout ce qui est dans le CSV n'est pas sur une carte.** Le premier passage comptait `Remarques`, `proverbe`, `exemple politique`, `image`, `link_fr` comme du texte de carte. Ils ne le sont pas : le mustache des gabarits Fallacies rend `Famille, Sous-Famille, Soussousfamille, text_fr, desc_fr, example_fr` (+ `desc_en` sur Face 3) — **et rien d'autre**, et `.Proverbe`/`.ExemplePolitique`/`.Remarques` n'ont **aucun lecteur** dans `Generation/`. Sans ce tri, la « fermeture Fallacies FR » se chiffrait à 391 au lieu de **0**.

**La règle qui en sort** : une mesure d'apostrophes se scope à **(fichier × colonne × langue × rendu-ou-émis)**, jamais au couple (fichier × langue). Le tri « rendu / non rendu / URL » n'est pas un raffinement — c'est ce qui sépare 0 de 391.

---

## §6 Feuille de décision (une feuille)

| | |
|---|---|
| **La question** | La convention FR=`’` / EN=`'` étant acquise, **jusqu'où l'appliquer** ? |
| **Le fait mesuré** | Trois surfaces ne l'ont jamais reçue : Scenarii FR (**392 occ., 138/167 cartes**), Rules FR (19), `AIF_criticalQuestion` (**139**, émis dans l'OWL). Fallacies FR et Virtues FR sont **déjà à 0** là où ça s'imprime. |
| **(A) FR seule — Scenarii + Rules + `AIF_criticalQuestion`** | **142 cartes · 6 PDF / 80 · 1 OWL** (régén **`fr/` seule**). Supprime le seul défaut *visible sur une carte*. **Recommandé** : même règle, même langue, aucune convention étrangère touchée. |
| **(B) FR + le balayage inverse EN (12 courbes)** | (A) **+ 12 cartes · +10 PDF / 80**. Le lot le moins cher par unité — #1030 avait déjà fait 1 146 des 1 158. |
| **(C) 8 langues** | **173 cartes · 52 PDF / 80 (65 %)** · régén complète. ⚠️ **Ne se justifie pas comme un nettoyage** : pour ru/pt/es cela **change une convention de langue** (`d'água` est correct en pt), pour zh/ar cela traite des **guillemets** — une autre question. |
| **(D) Ne rien faire** | 0 écriture. Le défaut visible reste : **138 des 167 cartes FR de Scenarii** portent au moins une apostrophe droite (392 au total, ≈ 2,8 par carte touchée), et l'OWL FR reste mixte (143 droites / 184 courbes). |
| **Nature** | Convention typographique — aucune branche ne corrige un contresens. |
| **Interaction avec les autres arbitrages** | Aucune dépendance à A/B/C/D du dispatch #458. Le lot EN (B) recoupe la « moitié EN du GO » déjà tracée dans `DECISION-v2.0.0-jsboige.md` (PR B, 102 subst.) — **12 cellules en sont le solde**. |
| **Ce qu'il faut savoir avant de trancher** | Le lot « FR seule » **n'invalide aucun mindmap** (les libellés de famille FR sont à 0) : c'est la fermeture de Fallacies FR qui rend la branche (A) sûre. Et `link_*` doit rester **exempt** dans toutes les branches — normaliser une URL la casse. |

---

## §7 Ce que ce grain n'a pas fait

⛔ `0` cellule CSV écrite · ⛔ `0` OWL régénéré ou modifié · ⛔ aucune régénération · ⛔ aucune branche choisie · ⛔ aucun verdict visuel rendu (c'est ai-01).

Mesure uniquement — le GO d'écriture est un geste séparé.

---

*po-2024 — grain ③ du dispatch #458 c.5656689863. Le worker mesure et signale ; la décision reste à l'owner.*
