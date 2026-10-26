# Débordement horizontal mobile — overlay News5 (grain #458 ③ / G-CC-bis)

## Symptôme mesuré

À 390 px de large, la page d'accueil se déplace horizontalement de **164 px de trop** :
`documentElement.scrollWidth` = **539** pour `clientWidth` = 375 (`/en` **et** `/ar`), avant que les
vignettes News5 différées ne soient chargées. Après chargement complet, le même document retombe à 378.

## Cause — mesurée, pas supposée

`shared/_Article Image.cshtml` produit `<div class="app-news5-img"><span class="overlay"></span><img loading="lazy" …>`.
La règle issue de `bs5/styles/_list.scss` (et `bs4`) pose l'overlay en `position: absolute` avec
`top: 0; bottom: 0; width: 100%` **mais sans `left`** :

```scss
span {
  position: absolute;
  background-color: rgba($dark, .0);
  top: 0;
  bottom: 0;          // <-- pas de left
  width: 100%;
```

Sans `left`, un élément en position absolue prend sa **position statique**. Tant que l'image différée
n'est pas chargée, `.app-news5-img` s'effondre à une boîte de ligne de 24 px et le `span` (inline,
conteneur `text-align: center`) se place à **50 %** de la largeur : `left` calculé = **175,5 px**
sur une carte de 351 px. Avec `width: 100%`, la boîte s'étend alors d'une largeur pleine **vers la
droite** → bord droit 538,5 = `scrollWidth` 539.

Preuve par différentiel (mêmes conditions, page froide) :

| État | `left` calculé des overlays | `scrollWidth` |
|---|---|---|
| Défaut présent | `0`, puis `175.5` ×4 | **539** |
| `left: 0` injecté à chaud | `0` ×5 | **378** |
| Après chargement des images, sans correctif | `0` ×5 | 378 |

## Correctif

`left: 0;` restauré dans `bs5/styles/_list.scss` et `bs4/styles/_list.scss` (+ les deux
`dist/styles.min.css` correspondants, faute de `node_modules` pour rejouer webpack).

⚠️ **Ce n'est pas une invention** : `bs3/styles/_list.scss` porte déjà `left: 0;` et son
`bs3/dist/styles.min.css` le sert — les builds bs4/bs5 l'ont perdu. Le correctif rétablit la parité
entre variantes (l'organe `News5OverlayCssParityTests` épingle cette parité).

## Contrôles après correctif

| Contrôle | Mesure |
|---|---|
| `/en` page froide, 390 px | `scrollWidth` **539 → 378** (`clientWidth` 375) |
| `/ar` page froide, 390 px | **539 → 378**, identique |
| Après chargement complet | 378 (inchangé : le correctif ne change que l'état pré-chargement) |
| `left` calculé des 5 overlays | `0` ×5 dès le chargement |
| Non-régression de l'overlay | couvre exactement son parent (mêmes `x`, `width`, `height`) ; survol → `rgba(52, 58, 64, 0.4)` |
| CSS servi | la règle servie contient `left:0` (vérifié sur l'URL réelle de la page) |

## Résidu hors périmètre — 3 px

`scrollWidth` reste à 378 pour `clientWidth` 375 (+3 px, et 393/390 en émulation mobile). Cause
distincte, **pré-existante** : un `.row` Bootstrap à gouttières `-15px` placé dans un
`.sc-content-block` sans padding horizontal (bord droit 393, gauche -3). Aucun rapport avec
l'overlay ; à traiter dans son propre grain (choix de la couche à ne pas improviser : padding du
conteneur, gouttière du `.row` ou `overflow-x: clip` ont des effets visuels différents).

## Images

`mobile-before.png` et `mobile-after.png` sont des captures pleine page à 390 px : **la largeur de
l'image est le `scrollWidth` du document** (539 px vs 378 px), recadrées à 1100 px de haut.
Elles sont produites par réinjection de `left: auto` sur la page vivante pour l'état « avant » —
aucune modification du site n'a servi à les obtenir.
