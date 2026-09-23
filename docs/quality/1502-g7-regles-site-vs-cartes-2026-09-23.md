# #1502 / G7 — Règles : le site publié vs les cartes du dépôt (dossier d'arbitrage)

**Date** : 2026-09-23 (tick 04:41) · **Lane** : po-2023 (worker) · **Base** : `origin/master` `c3d07132`
**Instrument** : [`docs/corpus/rules-site-vs-cards.py`](../corpus/rules-site-vs-cards.py) — exécuté **tel que livré**
avant commit (rc=0 ; la CI n'exécute pas `docs/`). Extraction verbatim du HTML publié (lecture seule,
GET sur pages publiques), appariement intra-variante par nom de section normalisé, similarité
SequenceMatcher, lecture croisée des paires divergentes.

**⛔ Gardes respectées** : lecture seule stricte côté DNN (gel #972 — aucun POST, aucune auth) ;
**zéro écriture** CSV/gabarit/site ; la prose de `regles.md` est **hors périmètre** (co-écrite, décision
owner) — elle n'est ici que comme **troisième référentiel mesuré**.

---

## Réponse en une ligne

Le site et les cartes sont **deux générations de la même prose** — les cartes sont un copy-edit
récent (coquilles corrigées, pictogrammes inlinés, une règle ajoutée), le site une génération
antérieure (coquilles propres, pictogrammes séparés) **mais un compteur joueur plus récent** :
la divergence est **bidirectionnelle et de vintages mélangés**, aucun des deux n'est un sous-ensemble
de l'autre. S'y ajoute une **tension interne à la carte elle-même** (4-8 annoncé, règles 3-4 joueurs présentes).

## 1. Généalogie des trois textes (mesuré)

| Référentiel | État | Faits mesurés |
|---|---|---|
| **Imprimé 2022** (`Cards/Rules/Archive/2022/`, #1504) | plus ancien | « de 4 à 8 joueurs » partout (4 fichiers) ; **ni** la règle « 3 ou 4 joueurs » **ni** la condition « 5 joueurs et plus » |
| **Site publié** (`www.argumentum.games/Règles` + 5 pages `details/mid/602`, relevé 23/09) | intermédiaire | mêmes règles que les cartes (sens vérifié par lecture), coquilles propres (« **sont** petit objet », « plebicités », « à l'issu », « commançant », « reprend son cour »), pictogrammes en sections séparées, compteur école **4 à 10** |
| **Cartes** (`Argumentum Rules - Cards.csv`, 15 cartes) | plus récent | copy-edit : coquilles corrigées, reformulations, pictogrammes **inlinés** dans la prose, compteur **4 à 8**, + 1 règle (T2), − 1 condition (T3) |
| `regles.md` (3ᵉ référentiel, hors périmètre prose) | lignée distincte | ensemble de variantes **différent**, **aucune** section « Les pioches », similarité faible aux deux autres (0,0–0,9 selon section) : le md n'est la source **d'aucun** des deux |

## 2. Table d'arbitrage — deltas de CONTENU nominatifs (les 7 points)

Chaque ligne : les deux versions en regard + la question. **⛔ Aucune réécriture sans décision owner.**

### T1 — Nombre de joueurs, école des menteurs ⭐ le point de l'owner

| Référentiel | Annonce |
|---|---|
| Site (landing + sous-titre de la page détail) | **« de 4 à 10 joueurs »** |
| Carte `Rules_02` (1ʳᵉ ligne) | **« Règles du jeu : de 4 à 8 joueurs »** |
| Imprimé février 2022 | **« de 4 à 8 joueurs »** (4 fichiers d'archive concordants) |

**Question owner** : 4-10 est-il une évolution de design non répercutée sur les cartes (le site serait
faux), ou une erreur du site ? ⚠️ Si 4-10 est retenu, la carte change ⇒ régén Tarot 8 langues (fenêtre).

### T2 — Règle « À 3 ou 4 joueurs » : carte seule + tension interne ⭐

La carte porte **deux** règles d'adaptation à 3-4 joueurs, absentes du site **et** de l'imprimé 2022 :
- `Rules_05` (jury) : « **À 3 ou 4 joueurs, les jurés choisissent 2 cartes.** »
- `Rules_06` (pioches) : « Tous les autres joueurs piochent une carte, **ou 2 cartes à 3 ou 4 joueurs**. »

Datée lignée cartes : introduite avant/pendant les optimisations de layout (`993e2f13` → #119/#201),
retouchée par le clarity pass #366 (`git log -S`, 5 commits). **Tension interne** : la même carte annonce
« 4 à 8 » (T1) et contient des règles « à 3 ou 4 joueurs » — les deux ne peuvent être vrais ensemble.
Si la règle 3-4 est légitime, le compteur juste serait « **3 à 8** » ; ni le site (4-10) ni la carte (4-8)
ne le disent. **Question owner** : la borne basse est-elle 3, 4 — et la règle 3-4 reste-t-elle ?

### T3 — Condition « A 5 joueurs et plus » : site seul, jamais cartes

La 1ʳᵉ variante du site : « **A 5 joueurs et plus**, si la carte du baratineur est désignée par
l'ensemble du jury, il ne remporte pas la manche… ». La carte `Rules_06` porte la même variante
**sans la condition**. **Mesuré** : `git log -S "5 joueurs et plus" -- Cards/Rules/` = **0 commit**,
et 0 occurrence dans `Cards.old.csv` — la condition n'a **jamais** existé dans la lignée cartes.
**Question owner** : la condition 5+ est-elle une précision à reporter sur la carte, ou du bruit site ?

### T4 — Compteurs des 4 variantes secondaires : site seul, dont une coquille

Le site annonce (landing) : bingo **1-10** · moulin **2-8** · beau parleur **1-8** · **parlote « de 4 à 4
joueurs »**. Les cartes de ces variantes n'ont **aucune ligne compteur** (mesuré : seul `Rules_02` en porte
une). « De 4 à 4 » est une formulation suspecte (4 joueurs fixes mal exprimé, ou borne erronée).
**Question owner** : faut-il un compteur sur les cartes des 4 variantes, et que vaut « 4 à 4 » ?

### T5 — Pictogrammes : même contenu, deux présentations

Les cartes inlinent les pictogrammes (🥇👇👇…➜🏆) dans « Le décompte » et « Les pioches » ; le site les
sépare en deux sections dédiées « Conditions de Victoire » et « Nombre de pioches » (les mêmes lignes
emoji, à l'identique). **Pas un delta de contenu** — un choix éditorial de support. La question (site :
reprendre le style inline ou garder les sections ?) ne se pose qu'au moment d'une re-synchronisation.

### T6 — Sections site sans contrepartie carte (école)

- **Carte Mémo** : bloc de téléchargement (lien fichier) — fonctionnaliale web, sans objet carte.
- **Conditions de Victoire** / **Nombre de pioches** : cf. T5 (pictogrammes).
Aucune règle de jeu ne vit uniquement dans ces sections (T2/T3 couvert ci-dessus).

### T7 — Coquilles du site corrigées sur les cartes (non exhaustif, mesuré)

| Site publié | Carte |
|---|---|
| « en y posant **sont** petit objet » | « son petit objet » |
| « jurés **plebicités** » | « jurés plébiscités » |
| « **à l'issu** d'un premier décompte » | « à l'issue » |
| « En **commançant** par le voisin » | « En commençant » |
| « la partie reprend son **cour** » | « son cours » |

⇒ Une re-synchronisation site ← cartes corrigerait ces coquilles **sans toucher au sens**.

## 3. Matrice de similarité (résumé de l'instrument, 43 paires + 3 sans contrepartie)

- **3 IDENT** (≥0,985) · **10 proches** (0,85-0,985) · **26 réécrites** (<0,85) · **4 ancres** vides des
  deux côtés (titres intermédiaires) · **3 sections site sans carte** (école).
- Les similarités basses (0,05-0,7) reflètent la **densité de reformulation**, pas des jeux différents :
  lecture croisée de 8 paires (résumé, jury, décompte, pioches, conditions bingo, tours parlote,
  débat bingo, variantes) = **même règles, même sens** dans toutes, hormis T1/T2/T3.
- ⚠️ Limite instrument : SequenceMatcher en **caractères** sous-estime les reformulations (leçon #1516 :
  plancher ~0,45 en caractères ≈ bruit) — c'est pourquoi chaque delta nominatif a été **lu**, pas seulement
  mesuré.

## 4. Ce que ce dossier n'établit pas

- **Qui a raison** sur T1/T2/T3/T4 : décisions éditoriales owner (et co-auteurs pour la prose).
- **Les 7 autres langues** : comparaison fr seulement (le site publié est fr).
- **L'ancienneté exacte** de la règle 3-4 dans la lignée cartes (le `-S` date les *changements* du
  libellé, pas l'introduction conceptuelle — première apparition visible `993e2f13`).
- **Le contenu DNN édité** : nous avons mesuré le **HTML servi** ; un contenu en cache ou non publié
  peut différer en base.
- Le rendu des cartes (QA visuelle = ai-01).

## 5. Référentiel faisant foi — proposition (à arbitrer)

Pour rendre le prochain écart **détectable automatiquement**, il faut UN référentiel. Proposition
worker (non tranchante) :

1. **Les cartes (dépôt)** = référentiel des **règles** : versionnées, testées, régénérables —
   l'instrument livré (`rules-site-vs-cards.py`) peut tourner en contrôle périodique site↔dépôt.
2. **L'imprimé 2022** = référence de l'édition commerciale figée (ne bouge jamais).
3. **Le site** = **dérivé** à re-synchroniser depuis les cartes (T5/T7 lui apportent les coquilles
   corrigées ; T1-T4 nécessitent d'abord l'arbitrage owner).
4. Après arbitrage T1-T4 : une seule passe d'écriture cartes (si décision) + republication site +
   régén 8 langues dans la fenêtre — **jamais l'inverse** (le site n'alimente pas le dépôt).

## Reproductibilité

```bash
python docs/corpus/rules-site-vs-cards.py   # depuis la racine, lecture seule, ~15 s
```

---
*po-2023 (worker lane) — signale, ne déclare pas PASS · arbitrage : owner & ai-01*
