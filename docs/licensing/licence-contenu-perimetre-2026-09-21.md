# Licence de contenu CC BY-SA 4.0 — périmètre mesuré, et les trois points qu'il ne tranche pas

> **Date** : 2026-09-21 · **Base** : `2f299fcc` · **Agent** : `myia-ai-01:Argumentum` (coordinateur)
> **Décision** : GO owner, interactif, 2026-09-21 — **VÉRIFIÉ** (reçu en direct, pas relayé).
> Question posée : variante CC à retenir. Recommandation présentée : **BY-SA**. Réponse : « OK Go ».

Ce document porte la **mesure** qui a servi à écrire [`LICENSE-CONTENT.md`](../../LICENSE-CONTENT.md).
Il est séparé de l'acte lui-même parce qu'une licence doit rester courte et stable, alors qu'une
mesure se re-fait.

---

## 1. Pourquoi le périmètre est énuméré et non délimité par chemin

C'était le premier réflexe, et il était **faux**. Mesure sur `Cards/**` à `2f299fcc` :

| Extension | Fichiers |
|---|---:|
| `.png` | **617** |
| `.json` | 46 |
| `.svg` | 45 |
| `.html` | 38 |
| **`.csv`** | **14** |
| `.jpg` | 9 |
| `.pdf` | 5 |
| `.otf` | 5 |
| `.css` | 4 |
| `.ini` | 3 |
| `.ai` | 3 |
| `.md` | 2 |
| `.git-id` | 10 |
| `.xlsx` | 1 |
| `.mm` | 1 |
| **Total** | **803** |

⇒ Écrire « le contenu de `Cards/` est sous CC BY-SA » aurait placé sous licence de contenu
**617 images** et 3 fichiers de façonnage `.ai`, c'est-à-dire l'essentiel de ce que le périmètre
est censé **exclure**. ⭐ **Un répertoire n'est pas un périmètre juridique** : il décrit où les
fichiers sont rangés, pas ce qu'ils sont.

Second piège, plus discret : sur les **14** `.csv`, **8 sont sous `Archive/`**. Une énumération
mécanique « tous les CSV » aurait relicencié des états historiques non livrés. Le périmètre retenu
est donc de **7 fichiers** — 5 CSV vivants + 2 prose de règles — chacun nommé.

⚠️ **Correction d'une affirmation antérieure de ma part** : j'avais annoncé « les 14 CSV » comme
périmètre candidat. C'est le compte **tous dossiers confondus**, Archive inclus. Le périmètre
livrable en compte **5**. L'écart n'était pas une approximation, c'était une mesure faite sur le
mauvais ensemble.

---

## 2. La liste des co-auteurs ne se dérive pas de git — mesuré deux fois, dans les deux sens

Le grain #1188 conduit par `po-2024` ([`1188-attribution-par-fichier-et-survivance.md`](1188-attribution-par-fichier-et-survivance.md),
base `6694d702`) mesure l'attribution **fichier par fichier** et **par survivance à HEAD**. Ses
conclusions **corrigent ma propre liste**, et dans les deux directions :

| Piste | Ce que git voit | Expression survivante dans `Cards/**` |
|---|---|---:|
| `Jean-Sylvain Boige` / `jsboige` | 1 025 commits | 328 / 341 fichiers livrés |
| `ThomasWatanabeVermorel` | 5 commits | **13 fichiers** |
| `Ludovic Pelletier` | 6 commits | **0** |
| `Nathaniel Richand` | 178 fichiers touchés | **0** |
| **Une relectrice nommée par tous les documents de recette** | **aucun commit, aucun *trailer*** | **non mesurable par git** |

⭐ **Le chiffre « 2 co-auteurs externes » est juste par coïncidence de méthode, pas par identité
de liste.** Par `git log` c'est {Pelletier, Thomas} ; par les documents de recette c'est
{Thomas, la relectrice}. **Les deux valent 2, et ce ne sont pas les mêmes deux.** Publier l'un en
croyant publier l'autre aurait nommé quelqu'un sans expression survivante et omis quelqu'un dont
la contribution est documentée partout.

### 2.a — Ce que j'ai refusé d'écrire

⛔ **Aucun patronyme n'a été inventé.** Mesure : le prénom de la relectrice apparaît dans **8
documents** du dépôt, **jamais suivi d'un nom de famille**. Et « Thomas » se présente dans la
prose du dépôt sous une forme qui désigne **une autre personne** (figure de la communauté DNN,
sans rapport avec le projet) — un `grep` naïf sur le prénom aurait attribué le corpus à un tiers.

⇒ [`LICENSE-CONTENT.md`](../../LICENSE-CONTENT.md) nomme **une identité mesurée** (le compte git)
et renvoie au présent dossier pour le reste, plutôt que de clore une liste qu'aucun instrument
disponible ne peut établir. **Compléter la liste par les noms civils est un geste owner** : lui
seul dispose de la source.

---

## 3. Les trois points signalés, non tranchés

Ils sont **déclarés dans la licence** plutôt que laissés à l'implicite. Aucun ne bloque le tag.

### 3.a — Polices commerciales déclarées dans des gabarits vivants

Les feuilles de style des gabarits de cartes **en service** déclarent des familles de polices
**commerciales**, aux côtés de familles libres. Le produit est destiné à la vente.

⚠️ Qualifié **SUPPOSÉ**, et la nuance porte tout le poids : une déclaration `font-family` dans une
CSS **n'établit pas** que la police est utilisée dans le rendu final (la cascade peut retomber sur
une autre), ni **quelle licence a été acquise** — un nom de police n'est pas une licence. Deux
mesures manquent, et aucune n'est faite ici : (i) quelles familles sont **effectivement résolues**
dans le rendu, (ii) quelles licences sont détenues. ⇒ **Point à instruire avant commercialisation**,
indépendant de la licence de contenu.

### 3.b — LGPL-3.0 à la racine, GPL-3.0 dans le composant de rendu

`LICENSE` déclare **LGPL-3.0** ; `Generation/CardPen/LICENSE.txt` déclare **GPL-3.0** (mesuré :
en-tête « GNU GENERAL PUBLIC LICENSE Version 3 », sans la clause *Lesser*). CardPen est un *fork*,
donc sa licence amont s'impose à lui. Les deux sont des licences de **code** ; la licence de
contenu ne les modifie pas et n'en dépend pas. ⇒ **Articulation à documenter**, pas à improviser.

### 3.c — Illustrations et façonnage

617 PNG, 9 JPG, 3 `.ai` de packaging : travail graphique, potentiellement de prestataires externes.
Explicitement **hors périmètre**. Les placer sous CC BY-SA demanderait leur accord, qui n'a pas été
demandé — et **ne pas les mentionner** aurait laissé croire qu'ils sont couverts.

---

## 4. Pourquoi BY-SA, et une erreur de raisonnement à ne pas reproduire

La recommandation présentée à l'arbitrage était **BY-SA**, contre **BY-NC-SA**.

⭐ **Le motif qui a fait pencher, et qui corrige une affirmation antérieure fausse de ma part** :
j'avais soutenu que la clause **NC** « referme la cession » et protégerait donc la vente du jeu.
**C'est faux.** Une licence ouverte ne lie que le **licencié** ; elle ne s'applique **jamais au
titulaire des droits**, qui conserve la faculté de commercialiser son œuvre et de la concéder sous
d'autres termes. Ajouter NC n'aurait donc **rien protégé du côté du projet**, tout en interdisant
les usages qui font la valeur d'un corpus pédagogique : reprise en cours payant, manuel, MOOC,
formation professionnelle.

Le **SA** (partage à l'identique), lui, fait un travail réel : il garantit que les enrichissements
du corpus — traductions, nouveaux sophismes, exemples — reviennent sous la même licence.

⇒ **NC coûtait des usages légitimes sans rien acheter.** Le raisonnement à retenir : *vérifier qui
une clause lie avant de lui prêter un effet protecteur.*

---

## 5. Instruments

| Mesure | Commande |
|---|---|
| Inventaire par extension | `git ls-files 'Cards/**'` + comptage par suffixe |
| CSV livrables vs archivés | `git ls-files 'Cards/**/*.csv' \| grep -vi archive` |
| Identités git | `git log --all --format='%an\|%ae' -- Cards Generation \| sort \| uniq -c` |
| Forme nominale | `git grep -rhoiE "<prénom>[ ]+[A-ZÀ-Þ]…" -- '*.md'` |
| Licence CardPen | lecture de l'en-tête de `Generation/CardPen/LICENSE.txt` |

⛔ **Limites déclarées.** `git ls-files` ne voit que **HEAD** : il ne dit rien de l'historique
(cf. la leçon #415, où un pathspec vérifié à HEAD rend 0 pendant que l'historique porte 462 objets).
Aucun de ces instruments **ne peut voir une contribution hors dépôt** — relecture, direction
éditoriale, arbitrage. C'est la limite qui compte le plus ici, et elle joue **contre** la
conclusion rassurante.

---

🤖 Generated with [Claude Code](https://claude.com/claude-code)
