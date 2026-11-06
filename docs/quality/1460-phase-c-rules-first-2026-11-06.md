# #1460 phase C — organe 1 : le deck Tarot commence par les Rules

**Date :** 2026-11-06 · **Lane :** po-2023 · **Branche :** `test/1460-phase-c-rules-first`
**Réf. :** [#1460](https://github.com/ArgumentumGames/Argumentum/issues/1460) (charpente de non-régression), [#119](https://github.com/ArgumentumGames/Argumentum/issues/119) (les Rules en tête), [#438](https://github.com/ArgumentumGames/Argumentum/issues/438) (Rules ramenées à 15)

---

## 1. Ce que l'organe mesure

Les **15 premières pages** de `Argumentum_TarotCards_{lang}.pdf` doivent être
`rules_01_face.png` … `rules_15_face.png`, **dans cet ordre**, sur **les 8 langues**.

Les candidats sont **toutes** les cartes de l'arbre (Rules **et** Fallacies, 191 empreintes),
jamais les seules Rules : restreindre le jeu de candidats rendrait l'organe **aveugle à son
propre défaut** — un deck qui commence par une Fallacy gagnerait contre un candidat restreint.

L'organe est arrosé par `ARGUMENTUM_PDF_BUNDLE_ROOT` (la variable des autres organes de
`VisualTests`). Absente **et** arbre par défaut absent : `NON MESURABLE`, écrit tel quel.
Présente mais inutilisable : **échec** — un gate armé affirme « j'ai un arbre ».

## 2. Pourquoi les pixels, et pas le texte

**Mesuré par ai-01 le 06/11, prérequis de la phase B** : les PDF de cartes n'ont **aucune couche
texte** — 0 caractère avec `pypdf` **et** avec `pdftotext`, 0 police, contre 17 616 caractères sur
un contrôle positif. Un organe « Rules d'abord » qui aurait lu le texte était donc **vert sur
n'importe quel défaut**.

Le seul contenu lisible est l'image. Deux façons de la lire, et une seule qui marche :

| Instrument | Résultat |
|---|---|
| `sha256` des pixels décodés | **jamais** égal — l'image embarquée est en **CMYK**, le PNG source en **RGB** (mesuré : `R9.tif` `mode=CMYK` 826×1417 vs `rules_01_face.png` `RGB` mêmes dimensions) |
| Empreinte perceptuelle (luminance → grille fixe → écart moyen absolu) | sépare nettement (voir §3) |

L'organe rasterise donc la page via **Magick.NET → Ghostscript** (150 dpi), réduit en luminance
`Grid`×`Grid` avec `IgnoreAspectRatio = true`, et compare en **écart moyen absolu**.

> ⚠️ `IgnoreAspectRatio` n'est pas cosmétique : le `Resize(w, h)` par défaut **conserve le ratio**
> et rend des empreintes de tailles différentes, donc incomparables (`NaN` par construction dans
> `Distance`). Constaté pendant la mise au point : 64×37 au lieu de 64×64.

## 3. Le calibrage — mesuré, pas espéré

Arbre : `.prep-1114-worktree` Release, 06/11. **8 langues × 15 pages = 120 pages**, 1 min 53.

| Langue | pages | marge min | (page) | écart gagnant max | (page) |
|---|---:|:---:|:---:|:---:|:---:|
| ar | 15 | **2,92×** | 9 | 6,35 | 1 |
| en | 15 | 3,78× | 9 | 6,49 | 1 |
| es | 15 | 4,85× | 9 | 6,51 | 1 |
| fa | 15 | 3,58× | 9 | 6,34 | 1 |
| fr | 15 | 4,63× | 9 | 6,49 | 1 |
| pt | 15 | 4,76× | 9 | 6,47 | 1 |
| ru | 15 | 4,99× | 10 | 6,48 | 1 |
| zh | 15 | 3,10× | 9 | 6,52 | 1 |

Sur les 120 pages, **chaque** page trouve sa propre règle au premier rang. La meilleure **mauvaise**
mesure reste **≥ 82,6** (82,63 en `es` à 83,17 en `en`, page 1 de chaque langue).

Deux planchers, posés **sous** le mesuré et **au-dessus** de la valeur qui ne prouverait rien :

| Constante | Valeur | Justification |
|---|---|---|
| `MinSeparationRatio` | **2,0** | mesuré ≥ 2,92 ; 1 = « ambigu », le cas que l'organe doit refuser |
| `MaxBestDistance` | **20,0** | mesuré ≤ 6,52 ; la première fausse réponse est à ≥ 82,6, soit un facteur 4 |

### 3.1 La grille a été portée de 32 à 48 **par la mesure**

À `Grid = 32`, la page 9 de `ar` ne rendait qu'une marge de **2,29×** — au-dessus du plancher de 2,
mais avec **0,29 de réserve**, c'est-à-dire un organe qui rougirait sur un frémissement de rendu.
À `Grid = 48` la même page remonte à **2,92×**.

⚠️ La réserve la plus mince du corpus reste **1,46×** (`ar` page 9). Toute modification du gabarit
de carte ou de la résolution de lecture **doit re-mesurer les 8 langues** avant de déplacer ce
plancher. C'est écrit dans la doc du moteur, à côté de la constante, pas seulement ici.

### 3.2 Deux instruments indépendants concordent

Sur `fr`, page 1, `Magick.NET` (C#) donne **6,11** et `pypdf+PIL` (Python) **5,70**. Deux
implémentations séparées, deux décodeurs PDF séparés, le même verdict — la marge de l'instrument
n'est pas un artefact d'un seul outil.

## 4. Les contrôles inverses — dans la suite, exécutés en CI sans artefact

`RulesFirstOrderIntegrityTests` (8 témoins, `Argumentum.AssetConverter.Tests`, aucune dépendance à
un arbre) :

| Témoin | Ce qu'il prouve |
|---|---|
| `Healthy_Set_Produces_No_Failure` | **contrôle inverse du contrôle** — sans lui, un vérificateur toujours-rouge passerait tous les autres |
| `Swapped_Pages_Are_Red_And_Name_Both_Cards` | un ordre inversé est nommé avec attendu **et** obtenu |
| `A_Deck_Starting_With_A_Fallacy_Is_Red` | **le défaut gardé** : le gagnant existe, gagne nettement, et la garde refuse quand même |
| `An_Ambiguous_Match_Is_Red_Even_When_The_Name_Is_Right` | le nom est le bon, la décision ne repose sur rien → seul le garde de marge l'attrape |
| `A_Page_That_Resembles_Nothing_Is_Red_Even_When_It_Wins` | une page blanche « gagnerait » toujours sans borne absolue |
| `Too_Few_Pages_Is_Red` | attendu > mesuré |
| `An_Empty_Expectation_Is_Red_Not_Silently_Green` | la forme la plus discrète du faux vert |
| `Distance_Refuses_Mismatched_Sizes...` | deux empreintes de tailles différentes rendent `NaN`, jamais un verdict inventé |

Les empreintes de ces témoins sont **synthétiques et uniformes** (niveau constant) : la distance
entre deux d'entre elles vaut exactement la différence de niveau, ce qui rend chaque témoin lisible
sans instrument. **Le calibrage sur de vraies cartes (§3) n'est pas re-dérivé par ces témoins** —
il est cité dans la doc du moteur.

## 5. Ce que cet organe n'établit pas

- Il ne dit **rien de la lisibilité** d'une carte : deux cartes peuvent être correctement ordonnées
  et illisibles.
- Il ne dit rien de la **justesse d'une traduction**, ni de l'absence de doublon dans une famille.
- Il **ne compare pas les langues entre elles** : chaque deck porte son propre attendu (son propre
  `rules_XX_face.png`).
- Un `ratio` élevé ne vaut pas certitude : il dit « le meilleur est nettement le meilleur », pas
  « c'est la bonne carte ». C'est `MaxBestDistance` qui porte cette seconde exigence — et il ne la
  porte que dans la limite d'un écart moyen sur une grille 48×48.
- Il dépend de **Ghostscript sur le PATH**. Absent : échec nommé, jamais un skip silencieux.

## 6. Mesures de la PR

| Objet | Mesure |
|---|---|
| Suite unitaire (`Argumentum.AssetConverter.Tests`) | **4688 total · 4682 réussis · 0 échec · 6 ignorés** (5 min 8 s) |
| Témoins phase C | 8 / 8 verts (23 ms) |
| Organe arbre (`VisualTests`, 8 langues) | **1 / 1 vert, 120 pages**, 1 min 53 |
| Organe sans variable, sans arbre par défaut | `NON MESURABLE` — écrit, pas un échec |

---

*po-2023*
