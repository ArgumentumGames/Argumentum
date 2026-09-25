# Licence du contenu éditorial — CC BY-SA 4.0

> ⚠️ **Ce dépôt porte DEUX licences distinctes.** Ce fichier ne couvre que le **corpus éditorial**
> énuméré au §1. Tout le reste — et notamment **le code** — reste sous [`LICENSE`](LICENSE)
> (LGPL-3.0). En cas de doute sur un fichier : s'il n'est pas dans la liste du §1,
> **il n'est pas sous CC BY-SA**.

---

## 1. Périmètre — énuméré, pas déduit d'un répertoire

La licence **Creative Commons Attribution - Partage dans les Mêmes Conditions 4.0 International
(CC BY-SA 4.0)** s'applique **exactement** aux fichiers suivants, et à eux seuls :

| Fichier | Contenu |
|---|---|
| `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` | Taxonomie des sophismes (titres, définitions, exemples, contre-exemples, 8 langues) |
| `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` | Taxonomie des vertus argumentatives (8 langues) |
| `Cards/Scenarii/Argumentum Scenarii - Cards.csv` | Scénarios de jeu (contextes, enjeux, répliques, 8 langues) |
| `Cards/Rules/Argumentum Rules - Cards.csv` | Règles du jeu, version cartes |
| `Cards/Rules/Argumentum Rules - Cards Print and Play.csv` | Règles du jeu, version Print & Play |
| `docs/ontology/argumentum.owl` | Ontologie OWL des sophismes — **artefact dérivé** de la taxonomie (Q-15a) |
| `docs/ontology/argumentum_virtues.owl` | Ontologie OWL des vertus — **artefact dérivé** de la taxonomie (Q-15a) |

Soit **7 fichiers** : les **5 fichiers du texte des cartes**, plus les **2 ontologies OWL
dérivées** de ces taxonomies (ajoutées le 24/09/2026, décision Q-15a : les `.owl` reprennent le
texte des taxonomies — 2 816 `skos:definition` — donc la licence du contenu s'y applique, et non
celle du générateur qui les produit, lequel reste sous LGPL-3.0).

### 1.a — ⛔ Deux fichiers en sont **retirés**, et c'est délibéré

`Cards/Rules/regles.md` et `Cards/Rules/rules.md` — la prose des règles — **ne sont pas** sous CC
BY-SA. Mesure d'attribution ligne à ligne à `2f299fcc` :

| Fichier | Lignes | Dont écrites par un **co-auteur** |
|---|---:|---|
| `Cards/Rules/regles.md` | 70 | **40 — soit 57 %** |
| `Cards/Rules/rules.md` | 68 | 7 — soit 10 % |

⭐ **Un titulaire ne peut pas relicencier seul une œuvre qu'il n'a pas seul écrite.** Ces deux
fichiers ne pourront rejoindre le périmètre qu'avec **l'accord du co-auteur concerné**, demandé
nommément. Les 5 fichiers ci-dessus sont, eux, mesurés à **100 % d'écriture du titulaire** —
c'est ce qui rend leur cession possible aujourd'hui.

⚠️ Limite de la mesure, déclarée : `git blame` attribue le **dernier rédacteur** d'une ligne, pas
son créateur, et **ne voit aucune contribution hors dépôt**. Elle est donc utilisée ici pour
**restreindre** le périmètre, ⛔ jamais pour l'élargir.

### 1.b — Ce que le périmètre exclut par ailleurs, et pourquoi

⛔ **Le périmètre n'est PAS « le répertoire `Cards/` ».** Ce répertoire compte **803 fichiers**,
dont **617 images PNG** : le délimiter par chemin placerait sous CC BY-SA des œuvres graphiques
qui n'y ont pas leur place. Sont donc **hors** de cette licence, même situés sous `Cards/` :

- **les illustrations** (`.png`, `.jpg`, `.svg`) — travail graphique, régime distinct ;
- **les fichiers de façonnage** (`.ai`, `Cards/Packaging/`) — travail de prestataire externe ;
- **les polices** (`.otf`) — sous les licences de leurs fonderies respectives, qui **priment** ;
- **les gabarits de cartes** (`.json`, `.css`, `.html`) — ce sont des programmes de rendu,
  couverts par [`LICENSE`](LICENSE) ;
- **tout le contenu sous `Cards/**/Archive/`** — états historiques non livrés ;
- `Cards/Rules/Argumentum Rules - Cards.old.csv` — copie de travail supplantée, non livrée.
- **la prose des règles** (`regles.md`, `rules.md`) — voir §1.a : co-écrites, accord requis.

---

## 2. La licence

Le corpus du §1 est mis à disposition selon les termes de la licence
**Creative Commons Attribution - Partage dans les Mêmes Conditions 4.0 International**.

- Résumé lisible : <https://creativecommons.org/licenses/by-sa/4.0/deed.fr>
- **Texte juridique intégral, qui seul fait foi** :
  <https://creativecommons.org/licenses/by-sa/4.0/legalcode.fr>

En deux phrases : vous pouvez **copier, diffuser, adapter et exploiter commercialement** ce corpus,
y compris à des fins d'enseignement ou de recherche, **à condition** de créditer les auteurs et
d'indiquer les modifications apportées ; et si vous diffusez une version modifiée, vous devez la
diffuser **sous la même licence**.

### 2.a — Comment créditer

Mention minimale acceptable :

> « Argumentum », projet ArgumentumGames — <https://github.com/ArgumentumGames/Argumentum> —
> sous licence CC BY-SA 4.0. [Modifié, le cas échéant.]

---

## 3. Titulaires et co-auteurs

Le corpus est l'œuvre de **Jean-Sylvain Boige** et des contributeurs du projet Argumentum.

⚠️ **La liste des co-auteurs ne se dérive pas d'un `git log`, et ce fichier ne prétend pas la
clore.** La mesure conduite pour ce dépôt
([`docs/licensing/1188-attribution-par-fichier-et-survivance.md`](docs/licensing/1188-attribution-par-fichier-et-survivance.md))
établit trois choses qui interdisent l'inférence automatique :

1. Sur le corpus livré, **une seule** contribution externe a une expression qui survit dans
   l'arbre courant — celle de **`ThomasWatanabeVermorel`**.
2. Des contributrices et contributeurs **nommés par les documents de recette du projet sont
   structurellement invisibles à git** (aucun commit, aucun *trailer*). Un dossier qui listerait
   les co-auteurs par `git log` les **manquerait**.
3. Inversement, des identités présentes dans l'historique git n'ont **aucune expression
   survivante** dans le corpus livré.

⇒ **« 0 commit » ne vaut pas « 0 contribution »**, et « N commits » ne vaut pas « N co-auteurs ».
Toute demande d'accord ou d'ajout à cette liste s'adresse **nommément**, jamais par requête
sur l'historique.

---

## 4. Ce que ce fichier ne décide pas

Il est plus sûr de déclarer ces points que de les laisser croire réglés :

1. **Le régime des illustrations** — hors périmètre, non tranché ici.
2. **Les polices déclarées par les gabarits** — plusieurs familles nommées dans les feuilles de
   style des gabarits vivants sont des polices **commerciales**, dont la licence est celle de leur
   fonderie et **prime sur tout ce qui est écrit ici**. ⚠️ Qualifié **SUPPOSÉ** : une déclaration
   `font-family` dans une CSS n'établit ni que la police est effectivement utilisée dans le rendu
   final, ni quelle licence a été acquise. Une **mesure partielle** a été conduite (quelles familles
   sont déclarées, et vers quoi pointe leur `@font-face`) et figure au
   [dossier de périmètre §3.a](docs/licensing/licence-contenu-perimetre-2026-09-21.md) ; elle
   **resserre** le point sans le clore. Il demande une vérification propre, et il n'est **pas**
   couvert par cette licence.
3. **La coexistence LGPL-3.0 / GPL-3.0 dans le dépôt** — le fichier racine [`LICENSE`](LICENSE)
   déclare LGPL-3.0, tandis que `Generation/CardPen/LICENSE.txt` déclare **GPL-3.0** pour le
   composant de rendu, qui est un *fork*. Les deux sont des licences de **code** et ne sont pas
   modifiées par le présent fichier ; leur articulation reste à documenter.
4. **Le contenu sous `Archive/`** — états historiques, hors périmètre.

---

## 5. Conditions d'application

- **Entrée en vigueur** : à la fusion du présent fichier dans `master`.
- **Non-rétroactivité** : une licence ouverte n'est pas révocable pour les exemplaires déjà
  diffusés sous ses termes ; elle n'affecte pas davantage les droits antérieurs des co-auteurs.
- **Contact** : par les issues du dépôt, <https://github.com/ArgumentumGames/Argumentum/issues>.

---

## English summary

This repository is **dual-licensed**. This file covers **only** the seven files enumerated in §1 —
the five editorial text files (the taxonomies of fallacies and argumentative virtues, the game
scenarios and the two rules card sets) plus the **two derived OWL ontologies** (Q-15a) — which are
released under
**[CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/legalcode)**.

⛔ The **prose** of the rules (`regles.md`, `rules.md`) is **deliberately excluded**: line-level
attribution measures 57% and 10% of those two files as written by a co-author, and a rightsholder
cannot relicense alone a work they did not write alone. They can only join the scope with that
co-author's named agreement.

Everything else, **including all source code**, remains under [`LICENSE`](LICENSE) (LGPL-3.0).
Illustrations, packaging artwork, fonts and card templates are **explicitly out of scope** — see
§1.a. The scope is defined by **explicit enumeration**, never by directory: `Cards/` holds 803
files of which 617 are images, and a path-based reading would license artwork that this file does
not cover.

Attribution notice:

> "Argumentum", ArgumentumGames — https://github.com/ArgumentumGames/Argumentum —
> licensed under CC BY-SA 4.0. [Modified, where applicable.]
