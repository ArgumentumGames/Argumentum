# Récupération de l'historique CSV décroché par le BFG — 2026-09-22

> **Écrit pour** : les mainteneurs du dépôt et tout agent qui voudra dater une modification du
> corpus. Ce document explique pourquoi `git log` sur les CSV rend un historique troué, ce qui a
> été récupéré, et quelle baseline fait désormais autorité.

## 1. Le constat de départ

L'historique GitHub du fichier `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` paraît
s'arrêter : aucune version du fichier n'est consultable entre **mai 2023** et **juillet 2025**,
alors que le dépôt porte **43 commits en 2024** et **51 en 2025**. Le propriétaire a signalé
l'anomalie et émis l'hypothèse d'une réécriture d'arbre *« peut-être pour récupérer de la place »*.

**L'hypothèse est exacte.**

## 2. Le mécanisme — BFG Repo-Cleaner, 30 juin 2025

```
3cc8af23  2025-06-30  build: Met à jour .gitignore pour exclure les artefacts
                      de nettoyage BFG et l'exécutable bfg.jar
```

Le [BFG Repo-Cleaner](https://rtyley.github.io/bfg-repo-cleaner/) a retiré les gros blobs de
**tout** l'arbre et les a remplacés par des marqueurs `<fichier>.REMOVED.git-id` contenant le
**SHA-1 du blob d'origine**. Les commits n'ont pas disparu — c'est leur **contenu** qui a été
décroché.

| Mesure | Valeur |
|---|---:|
| Marqueurs au Golden Master `0087f0ec` (avril 2024) | **301** |
| Marqueurs sur HEAD | **99** |
| Marqueurs sur HEAD dont le fichier original est **présent** | **98** |
| Marqueurs sur HEAD **sans** fichier original ⇒ perte réelle | **1** |

Le clone n'est **pas** superficiel (`shallow`), et il ne porte ni *graft* ni `replace` ref : la
troncature ne vient pas de la copie locale.

⚠️ La ligne de 2023 ne rejoint HEAD que par un chemin détourné — `afd8801f` (2026-03-17), une
fusion de `origin/master` dans une branche dependabot restée en arrière (`1ca28f21`, 2023-12-15).
C'est pourquoi quelques commits anciens restent visibles et donnent l'illusion d'un historique
continu.

## 3. Ce qui a été récupéré

⭐ **GitHub sert encore les blobs devenus non-atteignables.** Tant que le ramasse-miettes distant
ne les a pas collectés, `gh api repos/OWNER/REPO/git/blobs/<sha>` les rend, là où
`git cat-file -e` échoue localement. **C'est une fenêtre, pas un acquis.**

**29 versions du CSV Taxonomy récupérées, de mai 2023 à avril 2024.** Chaque blob a été vérifié en
recalculant son SHA-1 git (`sha1("blob <len>\0" + contenu)`) : **29/29 exacts**.

| Repère | Lignes | Colonnes | Cartes |
|---|---:|---:|---:|
| 2023-05-06 `54f6e3298` — première version | 1027 | 54 | **169** |
| 2024-02-08 `435f30854` | 1403 | 65 | 178 |
| 2024-04-22 `6f2f36fbe` — **baseline canonique** | 1408 | 74 | 176 |
| 2024-04-25 `0087f0eca` — Golden Master | 1408 | 74 | 176 |

Instruments conservés dans ce dossier : [`bfg-recovered-blobs-manifest.txt`](bfg-recovered-blobs-manifest.txt)
(date, commit, SHA de blob, sujet) et [`bfg-blob-recovery.py`](bfg-blob-recovery.py) (rejoue la
récupération tant que la fenêtre est ouverte).

⛔ **Les 40 Mo de CSV récupérés ne sont pas recommités** — ce serait annuler le nettoyage qui a
motivé le BFG. Ils sont conservés compressés (**10,1 Mo**) hors dépôt.

### 3.1 L'unique perte réelle, réparée

`Generation/Converters/Argumentum.AssetConverter/Logo_Argumentum & QRCode_Fallacies.png`
(566 445 o) était le seul marqueur sans fichier. Récupéré, SHA-1 exact, PNG valide.

⚠️ **Rien n'était cassé pour autant** : le `.csproj` le déclare en `<None Update=…>`, qui est un
*no-op* sur fichier absent, et la configuration charge un autre fichier
(`Logo_Argumentum & QRCode.png`, présent). C'est une restauration propre, **pas** un correctif de
panne — le dire autrement surévaluerait le geste.

## 4. La baseline canonique « avant l'agentique »

Le propriétaire a demandé que la version de référence soit *« celle de début 2025, avant qu'on
commence à travailler en agentique sur le dépôt »*. Elle est désormais **mesurée**, pas supposée.

1. Le CSV réintroduit après le BFG (`e8482fe5`, 2025-07-02) est **byte-identique** à l'état du
   **22 avril 2024** — `diff` : **0 ligne divergente**.
2. Les deux seuls autres commits 2025 touchant le fichier (`4b6d1627f` 24/07, `62b561e75` 27/07)
   se **neutralisent exactement** : `6361d489…` → `29ad52b3…` → `6361d489…`.
3. ⇒ Le contenu est **stable d'avril 2024 à mars 2026**.

> **Baseline = `62b561e75:Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`** — dernier état
> pré-agentique, déjà dans le dépôt, atteignable sans aucune récupération.

⚠️ **La restauration BFG a repris le 22 avril, pas le 25.** Les 17 cellules du commit
« New logos + taxo update » ont été perdues : 15 corrections d'URL Wikipédia, et **un seul
renommage de carte imprimée** — PK 492 « Jouer la victime » → « Auto-victimisation », annulé de
fait. *(La vague agentique a refait ce renommage en 2026, indépendamment.)*

## 5. Le delta agentique, corrigé

Le registre [`ledger-cartes-existantes-2026-09-22.md`](ledger-cartes-existantes-2026-09-22.md)
compare le corpus à **l'édition imprimée de février 2022**. Il capture donc **deux dérives
superposées** et impute l'ensemble au corpus courant.

⭐ **La dérive des descriptions est antérieure à l'agentique, et elle était bien faite.** Sur la
carte signalée par le propriétaire (PK 176, « Procédé rhétorique ») :

| Date | Description |
|---|---|
| 2022 → déc. 2023 | « Vous faites preuve d'une éloquence persuasive. » |
| **06/01/2024** `4de266e0a` | « …**sans nécessairement faire appel à la raison.** » |
| **09/01/2024** `01a5403ee` « MAJ desc_fr » | « …**sans nécessairement argumenter de manière logique.** » |
| 2024 → 2025 | *stable* |
| 2026, agentique | titre → « **Technique** rhétorique » · exemple → une flatterie |

L'édit de janvier 2024 **ajoute la réserve à la description imprimée et conserve l'exemple de
La Fontaine** — c'est-à-dire exactement le geste que le propriétaire décrit comme la bonne
correction. Le geste agentique de 2026 n'a pas dérivé la description : il a **renommé la carte**
et **remplacé l'illustration**.

### Mesure contre la baseline

Clé `PK`, contrôle inverse `Famille` **175/175 = 100 %** (la clé `path` passe aussi ; les deux
donnent le même résultat).

| | |
|---|---:|
| Cellules FR changées (`text_fr`, `desc_fr`, `example_fr`) | **322** sur **159** cartes |
| — `example_fr` | 145 |
| — `desc_fr` | 130 |
| — `text_fr` | 47 |
| Renommages **typographiques seuls** (apostrophe courbe…) | 15 |
| **Renommages réels** | **32** |
| ⛔ **Collisions de nom** | **1** |

⛔ **La collision est le défaut le plus lourd trouvé.** Le libellé « **Généralisation hâtive** » a
quitté PK 598 (devenue « Induction hâtive ») et atterri sur PK 2 (ex-« Argument bâclé ») : qui
connaît la carte par son nom en a désormais une autre sous les yeux.

Sur **toutes** les colonnes et **toutes** les langues : 5 478 cellules, dont **4 553 créations**
(les traductions `es`/`ar`/`fa`/`zh`, travail additif légitime), 906 substitutions et
19 suppressions.

## 6. Corroboration indépendante — le classeur GDrive

Le classeur maître (`Argumentum Fallacies`, onglet `Taxonomy`) a été exporté et comparé.

| Source | Lignes | Colonnes | Cartes |
|---|---:|---:|---:|
| GSheet `Taxonomy` | 1408 | 102 | **176** |
| Baseline 2024-04-22 | 1408 | 74 | **176** |
| HEAD | 1408 | 104 | **175** |

Contrôle inverse `Famille` : **176/176** et **175/175** à 100 %. Sur les 525 cellules FR de carte :

| | |
|---|---:|
| GSheet ≠ **des deux** ⇒ travail resté dans le classeur | **0** (0,0 %) |
| GSheet == baseline 2024, ≠ HEAD ⇒ **c'est le dépôt qui a dérivé** | **321** (61,1 %) |
| GSheet == HEAD | 1 |
| Cartes exclusives à l'une ou l'autre source | **0** |

⭐ **Le classeur est un gel de l'état d'avril 2024.** Il ne porte **rien** que le dépôt n'ait pas,
et il corrobore la baseline sur **321 des 322** cellules divergentes. Le « trou » redouté entre
les deux sources **n'existe pas**.

Les 2 colonnes « en plus » du classeur sont un artefact de renommage : le dépôt a corrigé
`crossLink_Levarages` → `crossLink_Leverages` et `latin` → `latin1`, puis ajouté
`AIF_attackType` / `AIF_attackedNode`.

## 6bis. ⚠️ Quelle archive fait référence — v3, pas 2022

Mesuré sur les **175 cartes imprimées actuelles** :

| Archive | Cartes couvertes |
|---|---:|
| **`Archive/v3/`** (169 lignes) | **153 / 175** |
| `Archive/2022/` (70 lignes) | 63 / 175 |
| couvertes par **2022 seul** | **0** |
| couvertes par aucune des deux | 22 |

⭐ **`Archive/2022/` ⊂ `Archive/v3/`** pour les cartes courantes : le tirage de février 2022
(70 lignes, toutes marquées `edition_fevrier_2022`) n'apporte **aucune** carte que v3 n'ait pas.
La carte signalée par l'owner (`path` 2.1) **n'y figure même pas** — « Procédé rhétorique » n'y
est qu'une **Sous-Famille**. ⇒ **La référence du deck effectivement joué est `v3`** ; 2022 est un
tirage antérieur plus court, utile pour dater, pas pour couvrir. ⛔ Une consigne « prendre 2022 en
priorité, v3 en repli » inverse les rôles et laisse 90 cartes sans référence.

## 7. Ce que ce document n'établit pas

- ⚠️ **Seul l'onglet `Taxonomy` a été comparé.** `Cards`, `Cards Print and Play`,
  `12/2023EN_AdelinePrint&Play` et les 10 autres onglets ne sont pas mesurés.
- ⚠️ **L'historique de révisions du classeur n'a pas été lu** — des édits 2025 ultérieurement
  annulés y seraient invisibles à la comparaison de contenu.
- ⚠️ **Correction du 22/09 — la perte d'attribution EST mesurable, contre la bonne référence.**
  Une première rédaction affirmait qu'aucun instrument ne pouvait la voir. C'était un **artefact
  de référence** : la sonde tournait contre la baseline 2025, où l'attribution avait **déjà**
  disparu — un contrôle inverse incapable de voir le défaut. Contre `Archive/v3/` :

  | Sonde « exemple cité / attribué » | lignes | guillemets | **attribution** |
  |---|---:|---:|---:|
  | Archive 2022 | 70 | 0 | 0 |
  | **Archive v3** | 169 | 1 | **1** |
  | HEAD (cartes) | 175 | 4 | **0** |

  ⇒ **1 sur 1 perdue — 100 % de la classe.** ⚠️ Mais la classe ne compte qu'**un** cas : un seul
  exemple du corpus de référence a jamais été une citation attribuée. ⛔ **Ne pas en tirer un taux.**
- ⚠️ **Ce qui reste hors de portée d'un instrument**, et qui est le vrai sujet : un exemple
  remplacé par un autre qui illustre un sophisme **voisin mais différent** (ici, une flatterie à la
  place d'un mouvement poétique). Aucune sonde ne juge l'adéquation d'un exemple à la définition
  qu'il sert. ⇒ **Les 145 exemples modifiés demandent une relecture humaine** ; c'est ce chiffre
  qui dimensionne la revue.
- ⚠️ **Les blobs orphelins ne sont pas garantis** : la récupération dépend du ramasse-miettes de
  GitHub. Le manifeste rejoue la récupération, il ne la remplace pas.
- Aucune autre famille de fichiers décrochés (images CardPen, données Mindmap, gabarits
  `Cards/Packaging`) n'a été auditée au-delà du constat « 98/99 originaux présents ».
