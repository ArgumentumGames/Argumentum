# #1499 G9-C — pertes dans les traductions substituées (en/ru/pt) : liste courte, 0 écriture

**Auteur** : po-2024 (worker) · **Date** : 2026-09-25 · **Base** : `origin/master` `9e79e88d`
**Grain** : pool #458 v13, ordre ai-01 c.5831306928 — *« liste courte des pertes dans les 463 traductions substituées, règle C (le défaut se garde, seul "revenir" demande l'owner), couche nommée, similarité par couche avant de proposer, ⛔ 0 écriture, PK 636 exclue jusqu'à Q-19 »*
**Instrument** : [`tools/1499-g9c-pertes.py`](../../tools/1499-g9c-pertes.py) — lecture `git show` des deux couches, **0 écriture** · détail nominatif : [`tools/1499-g9c-pertes.json`](../../tools/1499-g9c-pertes.json)

---

## 1. La couche, nommée — et l'écart au « 463 », déclaré

- **Couche B (pré-agentique)** : `62b561e75` (2025-07-27), désigné tel par la spec G-série. Une perte due à l'agent se mesure B → C.
- **Couche C (courant)** : `origin/master` `9e79e88d`.
- **Le « 463 »** (ai-01, #1499 c.5777618740) n'est **pas reproductible sur l'arbre courant**, pour deux raisons cumulées : il portait sur l'**archive imprimée** (169 rangées, référentiel tiers — ni B ni C), et il a été mesuré **avant** la restauration G2-C-W (#1546), qui a déplacé l'arbre. La règle de couche de la spec elle-même (§ en-tête) donne la population honnête, chiffrée ci-dessous. L'écart est déclaré, non absorbé.

## 2. Les chiffres (colonnes prose en/ru/pt + libellés, PK 636 exclue)

| | cellules |
|---|---:|
| Cartes cascade (substitution FR B→C) | **1 300** |
| Cellules FR substituées | 2 456 |
| **Prose en/ru/pt substituée** | **1 999** (en 1 014 · ru 532 · pt 453) |
| — dont sur cartes cascade | 1 916 (hors cascade : 83) |
| Libellés substitués (Family/Sub…) | 176 |
| **PERDU** (B rempli → C vide) | **19** |
| GAGNE (créations) | 48 |

Similarité token (casefold + NFKC, ponctuation retirée — calibration G6) sur la prose substituée : `[0 ; 0,2[` **670** · `[0,2 ; 0,4[` 411 · `[0,4 ; 0,6[` 352 · `[0,6 ; 0,8[` 336 · `[0,8 ; 1]` 230. La moitié de la population est une réécriture complète — cohérent avec les campagnes desc/example (#994 #1102, #1089) qui retravaillaient le texte, pas la typographie.

## 3. Les 19 PERDU : gestes documentés, pas de pertes

- **18 cellules** (PK 1, 175, 594, 696, 798, 887, 1280 — `example_en/ru/pt`) viennent de `df561c0c` (2026-06-02, #424) : *« blank example on 7 family roots in non-fr langs to mirror fr »*. Vérifié : `example_fr` était **déjà vide en B** pour 7 de ces 8 PK — les exemples en/ru/pt étaient des orphelins sans source.
- **1 cellule** (PK 61 `Simple_name_en`) : une URL don-lindsay logée dans un champ de nom — pollution retirée par `ed66f800` (2026-06-23, #579, hygiène données).

Verdict : **garder** — supprimer ces 19 cellules était le geste documenté, pas une perte de couche.

## 4. La lecture : 215 cellules, méthode nominative

1. **Pire bande** (jac ≤ 0,15, ou jac ≤ 0,30 avec effondrement de longueur, ou ratio ≤ 0,35 — après triage mécanique des 27 corrections de langue) : **156 cellules** lues une à une (ancien vs nouveau vs FR courant).
2. **Bande médiane** (0,15 < jac ≤ 0,45) : **échantillon aléatoire de 40** (graine 20260925), lu pareil.
3. Les **19 PERDU** (§ 3).

Verdicts dominants : paraphrase suivant le style FR courant (« Vous… »), correction de titres faux (PK 97 « Is-Ought Fallacy » sur une carte *Appel à la coutume* ; PK 960 « Чернь » sur *Bouc émissaire*), réparations de traduction machine (PK 223), gains nets (DARVO PK 1356, négativité PK 1092, régression infinie PK 658). **Aucune des 40 médianes n'est une perte.**

## 5. La liste courte — 0 « revenir » à proposer à l'owner

**Aucune substitution mesurée ne justifie un retour.** Quatre pertes culturelles sont nommées pour mémoire, toutes **cascade-justifiées** (la source FR a perdu la même chose, ou l'ancien texte était une localisation libre ne suivant pas sa source) — règle C : garder.

| PK | Cellule | Perdu | Pourquoi garder |
|---|---|---|---|
| 176 | `example_ru` | la fable de Krylov (Corbeau et renard) → compliment générique | la source FR a perdu La Fontaine dans le même mouvement (`example_fr` B→C) |
| 855 | `example_ru` | le calembour russe banque/conserve → calembour FR avocat/avocat | `example_fr` inchangé (« Cet ours a mangé un avocat ») — l'ancien ru était une localisation libre |
| 41 | `example_pt` | la citation Potter Stewart 1964 → exemple jouable | style éditorial assumé (exemples jouables, notes encyclopédiques retirées) |
| 1330 | `example_pt` | la « défense Chewbacca » → anecdote marbles | idem, et l'anecdote de remplacement est jouable |

**Constat pré-B, hors couche** (candidat grain futur, aucune action ici) : PK 201 `text_fr` = « Jargon **juridique** » mais `desc_fr` = « jargon **scientifique** » — l'incohérence est **identique en B et en C**, donc pré-agentique ; les traductions ont été alignées sur le `desc_fr`, pas dérivées.

## 6. Libellés : 176 substitutions = renommages documentés

Têtes de série vérifiées : le relabel PK 511 (#1294, « Influence non verbale » → « Communication non verbale », 8 langues) et les renommages de familles (#981/#982a/#998/#1002, motif additif). Une ligne d'agrégat, pas 176.

## 7. Ce que ce dossier n'établit pas

- **Lecture partielle** : 215 cellules sur 1 999 (10,8 %), concentrée sur la pire bande ; l'extrapolation vers les bandes similaires supérieures s'appuie sur la nature des campagnes (réécriture suivant FR) et sur l'échantillon médian à 0 perte — pas sur une lecture exhaustive.
- Les 27 corrections de langue (FR → langue de colonne) sont des gains, non relues une à une au-delà du triage.
- PK 1361 (dans le périmètre) lue : `text_pt` aligné sur le FR « Procès en incohérence » — garder. **PK 636 exclue** partout (instrument, attente Q-19).
- ⛔ **0 écriture corpus** — le grain mesure et nomme, il ne tranche pas.

## 8. Exécutable

```
python tools/1499-g9c-pertes.py --self-test   # rc=0 — similarité, jointure doublons, NFKC
python tools/1499-g9c-pertes.py               # mesure + dump classé -> tools/1499-g9c-pertes.json
```

Référentiel des verdicts de lecture : `subst_prose_classees` dans le JSON (jac/cont/ratio + ancien/nouveau par cellule).

Refs #1499
