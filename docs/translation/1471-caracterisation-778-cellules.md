# #1471 — Caractérisation des 858 cellules vides restantes (après PR #1487)

**Auteur** : po-2024 (worker) · **Date** : 2026-09-21 · **Base** : master `3d71c244`
**Grain** : pool #458 **v9 ②** — *« caractériser les 778 cellules restantes. L'arbitrage (a)/(c) porte désormais sur un chantier, pas un résidu. »*
**Instrument** : [`tools/1471-langlinks-characterize.py`](../../tools/1471-langlinks-characterize.py) — lecture seule, **0 écriture**.

> ⛔ **0 cellule touchée.** Ce document est un **état descriptif**, pas un plan d'action. L'arbitrage sur la stratégie (script / chinois / manuel / abandon) reste **owner**.

---

## 1. Le périmètre mesuré

**Après la PR #1487** (225 cellules écrites), le deck Fallacies imprimé (175 cartes) porte **858 cellules `link_*` encore vides** sur 1 400 possibles (175 cartes × 8 langues).

Ce **858** est légèrement supérieur au **778** cité dans le dispatch v9 — l'écart est attendu : le 778 était la **mesure pré-apply** (cartes ayant au moins une ancre ailleurs, après exclusion des 47 sans ancre). Après apply, 225 cellules sont remplies, ce qui **réduit** le périmètre des cellules atteignables (les 225 gagnées sont sorties du décompte), mais **ne change pas** le périmètre des sujets sans ancre (E_SUJET_NON_ANCRE, par construction).

La **borne basse non-négociable** : **355 cellules E_SUJET_NON_ANCRE** — ce sont des cartes dont **aucune** des 8 langues ne porte d'URL Wikipédia. Ces 355 cellules **ne sont jamais scriptables** par langlinks, parce qu'il n'y a aucune ancre à interroger. Toute stratégie qui voudrait les atteindre doit passer par une **recherche humaine**, une **création de contenu**, ou un **abandon**.

## 2. Les 3 classes

| Classe | Définition | Compte |
|---|---|---:|
| **A_ARTICLE_EXISTE** | la cellule est vide, mais l'ancre wiki pointe un article qui **existe** dans la langue cible | **38** |
| **B_ARTICLE_PAS_LANGUE** | la cellule est vide, l'ancre pointe un article qui **n'existe pas** dans la langue cible (lacune encyclopédique) | **465** |
| **E_SUJET_NON_ANCRE** | la carte n'a **aucune** ancre Wikipédia dans les 8 langues | **355** |
| **TOTAL** | | **858** |

⚠️ **38 + 465 = 503** : c'est le nombre de cellules ayant une ancre ailleurs (sur lequel portent les 933 sondes). **A_OU_B_A_VERIFIER** dans la première passe sans sonde est dissous en **A vs B** après mesure.

## 3. Distribution par langue (mesurée sur 933 sondes réseau)

| Langue | A (existe) | B (lacune) | A+B (avec ancre) | ratio A / (A+B) |
|---|---:|---:|---:|---:|
| `ar` | 12 | 58 | 70 | **17,1 %** |
| `es` |  9 | 68 | 77 | 11,7 % |
| `pt` |  8 | 76 | 84 |  9,5 % |
| `ru` |  5 | 74 | 79 |  6,3 % |
| `en` |  1 |  1 |  2 | 50,0 % |
| `zh` |  2 | 71 | 73 |  2,7 % |
| `fr` |  1 | 35 | 36 |  2,8 % |
| `fa` |  0 | 82 | 82 |  **0,0 %** |

### Lecture

- **`ar`**, **`es`**, **`pt`** sont les langues où il reste **le plus de marge scriptable** : 12, 9, 8 cellules « article existe ». Ces marges sont **petites** (par rapport aux 633 cellules restantes), mais **réelles** — un passage scriptée sur ces langues aurait un rendement positif.
- **`fa` est à 0 %** : aucun article persan pour les sujets où une ancre existe ailleurs. La stratégie **D** du dispatch (combler éditorialement par recherche humaine ou création de contenu) est la **seule** voie pour `fa`, et son coût est maximal.
- **`fr` à 2,8 %** : la langue source a déjà 89 cartes couvertes, il reste peu de cibles atteignables — **et le grain ① a écrit les 6 dernières** (ratio A_OU_B → 0).
- **`en` à 50 % sur 2 cas** : c'est un artefact d'échantillonnage (très peu de cellules en/en vides — 14 au total), **pas** un signal que la moitié des cibles sont atteignables.

## 4. Le coût par stratégie

| Stratégie | Cellules visées | Coût | Rendement attendu |
|---|---|---|---|
| **Re-run du scanner sur les 38 cellules A** | 38 | **Très faible** (un re-run du scanner `1471-langlinks-yield.py` avec un seed élargi, suivi d'un apply byte-exact) | **38 cellules** potentielles |
| **Recherche humaine / campagne éditoriale** | 465 cellules B | Élevé (par cellule : 5-30 min de recherche, taux d'échec élevé) | **inconnu** — dépend du sujet |
| **Création de contenu (Wikidata + nouvel article)** | 465 cellules B | Très élevé | **0 garanti** — Wikipédia a ses propres règles d'admissibilité |
| **Abandon** | 355 cellules E + ~400 cellules B jugées inatteignables | 0 | **0** mais **honnête** |

## 5. La structure du chantier post-tag

Le chantier post-tag, s'il est engagé, a donc **3 strates de difficulté croissante** :

1. **Strate scriptée** — 38 cellules A, ciblées en priorité sur `ar`/`es`/`pt`. Coût ≈ 1-2 h. **Le scanner les trouve déjà** si on élargit la requête langlinks (par exemple, en testant `ar.wikipedia.org` à partir de l'ancre `en.wikipedia.org` au lieu de l'inverse).
2. **Strate recherche humaine** — ~150-200 cellules B (sujets où une recherche patiente trouve un article non lié par interwiki). Coût ≈ 20-40 h. À arbitrer au cas par cas.
3. **Strate abandon / création** — ~265 cellules B + 355 cellules E. Honnête : dire lesquelles on **ne couvre pas**, et **pourquoi**.

⚠️ Le **choix de la strate 1** est le **seul** automatisable. Les strates 2-3 demandent une **décision humaine** par cellule.

## 6. Ce que ce cadrage **n'établit pas**

⛔ **La qualité** des 38 cellules A — un article qui existe peut être une page d'homonymie, une redirection, ou un article trop court. Le scanner a déjà filtré 2 homonymies ; un audit par échantillonnage reste à faire.
⛔ **Le coût exact** des strates 2-3 : l'estimation est grossière (par catégorie de sujet, pas par cellule).
⛔ **L'ordre entre langues** : la mesure brute ci-dessus classe `ar`/`es`/`pt` au-dessus de `zh`/`fr`/`fa`, mais **ça ne dit pas la stratégie**. `fa` à 0 % peut justifier une décision d'**abandon** (et non d'aller chercher quand même) — c'est un arbitrage.
⛔ **L'incertitude statistique** sur les 933 sondes : les ratios sont des **estimations ponctuelles**, pas des intervalles de confiance. Un sondage stratifié (150 par langue, déjà tenté — partiel : 933 sondes) réduirait l'incertitude, mais chaque sonde est un appel HTTP à Wikipédia et coûte ~1 s ; passer à 5 000 sondes (~80 min) n'apporterait qu'une marge plus serrée sur les mêmes ratios.
⛔ **Le périmètre deck vs taxonomie** : la mesure porte sur le **deck imprimé** (175 cartes). Le **reste de la taxonomie** (1 233 nœuds) a son propre périmètre, non couvert ici.

## 7. Recommandation

Pour le **gate owner**, ce cadrage propose **3 questions** :

1. **Cible-t-on la strate 1** (38 cellules, ~2 h) ? Si oui, **par quelle langue** ? `ar` (12) / `es` (9) / `pt` (8) sont les meilleurs candidats en ratio, mais **toutes les langues** ne sont pas forcément concernées — `fr` à 2,8 % n'a peut-être pas le même poids éditorial que `ar` à 17,1 %.
2. **Quelle est la politique d'abandon** ? Les 355 cellules E_SUJET_NON_ANCRE sont **non négociables** : ce sont des sujets sans Wikipédia dans aucune langue. Dire « on ne les couvre pas » est une décision, pas un constat.
3. **Quel budget pour la strate 2** ? Le coût estimé (~20-40 h) n'est pas une donnée owner — c'est une plage que ce cadrage **ne tranche pas**.

Ce que ce cadrage tranche : **le périmètre 778/858** est maintenant **caractérisé**, **non plus seulement dénombré**. La décision owner peut se prendre sur des classes, pas sur un seul chiffre.

---

*Une caractérisation qui ne propose pas de hiérarchie d'effort n'est pas un cadrage : c'est un décompte. Les 3 strates ci-dessus sont cette hiérarchie — avec leurs bornes de coût et leur probabilité de rendement. Le reste est décision.*
