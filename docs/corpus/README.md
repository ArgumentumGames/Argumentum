# docs/corpus — constats datés des chantiers de corpus

Ce dossier ne fait que grossir : chaque chantier y dépose ses rapports datés et ses
instruments compagnons. Rien ne s'y supprime — chaque fichier est une pièce. Ce README
indexe la **tranche 1 : les constats de chantier** — 53 fichiers `1499-*` et `458-*`.
Il n'est **pas encore** un index de record : la marque viendra avec la dernière tranche
(24 instruments `.py`, le manifeste `bfg-recovered-blobs-manifest.txt` et 4 pièces
éparses `.md` — 29 fichiers), conformément à
[`docs/agents/consolidation.md`](../agents/consolidation.md) (§ *Procedure* :
« Add the marker last »). En attendant, **rien ne tient cet index en CI** — une ligne
oubliée ne rougira rien ; la complétude ci-dessous est celle de sa PR.

Compte établi par `git ls-files docs/corpus | grep -cE '^docs/corpus/(1499|458)-.*\.md$'`
→ **53**.

## Où en sont les deux chantiers (mesuré le 2026-11-20 sur `cf5b2d26`)

- **#1499 — clos (COMPLETED)** : l'epic « archive 2022 → corpus courant ». Les 7 pièces
  ci-dessous datent de la passe de septembre 2026 (pertes des substitutions, chiffrage des
  69 cartes appareillées, cohérence PK 636, registres es, alignement de cascade). L'état
  final et la clôture vivent sur [#1499](https://github.com/ArgumentumGames/Argumentum/issues/1499).
- **#458 — ouvert (roadmap)** : les 46 pièces `458-*` sont les vagues **v17→v23**
  (26/09–10/10/2026) de la campagne de fidélité multilingue — titres, définitions,
  exemples, bandeaux, registres, casse, typographie, langue par langue. Toutes sont des
  **mesures 0-écriture** dont les propositions sont passées à l'arbitrage (Règle C) ; les
  écritures qui ont suivi vivent dans les lots mergés de novembre (série #1621→#1612 puis
  lots #1471, clos le 17/11 avec résiduel déclaré). Le programme de contre-lecture native
  lancé par g38 a été **arrêté** depuis (décision consignée sur #458, P24-07 :
  « plus de contre-lecture des 147 actuelles »). L'état vivant vit sur
  [#458](https://github.com/ArgumentumGames/Argumentum/issues/458).

## La tranche — 53 fichiers

### #1499 — archive 2022 → corpus courant (7)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`1499-g3-exemples-multilingue-2026-09-26.md`](1499-g3-exemples-multilingue-2026-09-26.md) | 2026-09-26 | `historical` | Surface multilingue des `example_*` : 0 défaut résiduel, et pourquoi un compte par langue ne se lit pas comme un compte de défauts |
| [`1499-g6-chiffrage-divergences-2026-09-25.md`](1499-g6-chiffrage-divergences-2026-09-25.md) | 2026-09-25 | `historical` | Chiffrage des divergences archive 2022 vs corpus courant, 69 cartes appareillées (jointure E1+E2, jamais `path` seul) |
| [`1499-g636-coherence-terminologique-2026-09-26.md`](1499-g636-coherence-terminologique-2026-09-26.md) | 2026-09-26 | `historical` | PK 636 « Sophisme de régression » : cohérence terminologique des 8 `desc_*` réécrites (#1563), 0 écriture |
| [`1499-g6c-pertes-scenarii-2026-09-25.md`](1499-g6c-pertes-scenarii-2026-09-25.md) | 2026-09-25 | `historical` | Liste courte des pertes du fr Scenarii substitué (345 MODIFIE réduits à une liste), 0 écriture |
| [`1499-g9-es-registre-defs-2026-09-26.md`](1499-g9-es-registre-defs-2026-09-26.md) | 2026-09-26 | `historical` | Registre es des définitions — deux couches datées : la mesure (0 écriture), puis 3 incohérences écrites (G9-es-W) |
| [`1499-g9a-alignement-cascade-2026-09-26.md`](1499-g9a-alignement-cascade-2026-09-26.md) | 2026-09-26 | `historical` | Alignement de cascade : 15 cellules fr changées, 7 traductions pas toutes — mesure 0 écriture |
| [`1499-g9c-pertes-substituees-2026-09-25.md`](1499-g9c-pertes-substituees-2026-09-25.md) | 2026-09-25 | `historical` | Pertes des 463 traductions substituées (en/ru/pt) — liste courte, règle C, 0 écriture |

### #458 v17–v19 — invariants, collisions, typographie (8)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`458-v17-g8-pk661-regression-infinie-2026-09-26.md`](458-v17-g8-pk661-regression-infinie-2026-09-26.md) | 2026-09-26 | `historical` | PK 661 « régression infinie » : deux concepts, pas un doublon (mesure, 0 écriture) |
| [`458-v17-g9-example-en-bis-2026-09-26.md`](458-v17-g9-example-en-bis-2026-09-26.md) | 2026-09-26 | `historical` | `example_en_bis` : rien ne se rend, par construction |
| [`458-v18-g2-collisions-fr-origine-2026-09-26.md`](458-v18-g2-collisions-fr-origine-2026-09-26.md) | 2026-09-26 | `historical` | Origine des 86 collisions de titres fr (fenêtre 93 commits du CSV) |
| [`458-v18-g3-collisions-7langues-2026-09-26.md`](458-v18-g3-collisions-7langues-2026-09-26.md) | 2026-09-26 | `historical` | Collisions de titres dans les 7 autres langues (même méthode, convergence) |
| [`458-v18-g6-typographie-hors-1562-2026-09-26.md`](458-v18-g6-typographie-hors-1562-2026-09-26.md) | 2026-09-26 | `historical` | Typographie des surfaces joueur hors #1562 — inventaire |
| [`458-v19-g4-bandeaux-invariant-2026-09-26.md`](458-v19-g4-bandeaux-invariant-2026-09-26.md) | 2026-09-26 | `historical` | Bandeaux imprimés ≠ titre de leur niveau — mesure (grain 4a corrigé) |
| [`458-v19-g5-majuscules-sans-accent-2026-09-26.md`](458-v19-g5-majuscules-sans-accent-2026-09-26.md) | 2026-09-26 | `historical` | Majuscules sans accent dans le corpus — mesure |
| [`458-v19-g7-ecarts-sens-fr-en-2026-09-26.md`](458-v19-g7-ecarts-sens-fr-en-2026-09-26.md) | 2026-09-26 | `historical` | Écarts de sens fr ↔ en — mesure |

### #458 v20–v22 — fidélité des titres et des bandeaux, 8 langues (16)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`458-v20-g7-bandeaux-virtues-2026-09-27.md`](458-v20-g7-bandeaux-virtues-2026-09-27.md) | 2026-09-27 | `historical` | Invariant bandeau sur le deck Virtues |
| [`458-v20-g8-titres-en-fidelite-2026-09-27.md`](458-v20-g8-titres-en-fidelite-2026-09-27.md) | 2026-09-27 | `historical` | Fidélité des 175 titres anglais du deck |
| [`458-v21-g11-casse-pt-es-2026-09-27.md`](458-v21-g11-casse-pt-es-2026-09-27.md) | 2026-09-27 | `historical` | Casse des titres pt/es |
| [`458-v22-g2-titres-es-fidelite-2026-09-27.md`](458-v22-g2-titres-es-fidelite-2026-09-27.md) | 2026-09-27 | `historical` | Fidélité des 175 titres espagnols |
| [`458-v22-g3-titres-pt-fidelite-2026-09-27.md`](458-v22-g3-titres-pt-fidelite-2026-09-27.md) | 2026-09-27 | `historical` | Fidélité des 175 titres portugais |
| [`458-v22-g4-titres-virtues-en-fidelite-2026-09-27.md`](458-v22-g4-titres-virtues-en-fidelite-2026-09-27.md) | 2026-09-27 | `historical` | Fidélité des 131 titres anglais des Vertus |
| [`458-v22-g11-titres-ru-fidelite-2026-09-28.md`](458-v22-g11-titres-ru-fidelite-2026-09-28.md) | 2026-09-28 | `historical` | Fidélité des 175 titres russes (sources externes ru.wiki, glossaire Digitable) |
| [`458-v22-g11b-beurre-reference-imprimee-2026-09-28.md`](458-v22-g11b-beurre-reference-imprimee-2026-09-28.md) | 2026-09-28 | `historical` | « Vouloir le beurre et l'argent du beurre » contre la référence imprimée — résout la C-note 5 du grain ⑪ |
| [`458-v22-g12-titres-zh-fidelite-2026-09-29.md`](458-v22-g12-titres-zh-fidelite-2026-09-29.md) | 2026-09-29 | `historical` | Fidélité des 175 titres chinois (zh.wiki, 《逻辑思维与诡辩》, Copi trad.) |
| [`458-v22-g13-titres-ar-fidelite-2026-09-29.md`](458-v22-g13-titres-ar-fidelite-2026-09-29.md) | 2026-09-29 | `historical` | Fidélité des 175 titres arabes (ar.wiki, glossaires Obstan/shamela, Stanford trad.) |
| [`458-v22-g14-titres-fa-fidelite-2026-09-29.md`](458-v22-g14-titres-fa-fidelite-2026-09-29.md) | 2026-09-29 | `historical` | Fidélité des 175 titres farsi (fa.wiki, wikifeqh, manuel « منطق دهم », abadis) |
| [`458-v22-g15-vertus-pt-es-fidelite-2026-09-29.md`](458-v22-g15-vertus-pt-es-fidelite-2026-09-29.md) | 2026-09-29 | `historical` | Fidélité des 131 titres pt/es du deck Virtues |
| [`458-v22-g16-vertus-ru-fidelite-2026-10-01.md`](458-v22-g16-vertus-ru-fidelite-2026-10-01.md) | 2026-10-01 | `historical` | Fidélité des 131 titres ru du deck Virtues |
| [`458-v22-g17-vertus-zh-ar-fa-fidelite-2026-10-02.md`](458-v22-g17-vertus-zh-ar-fa-fidelite-2026-10-02.md) | 2026-10-02 | `historical` | Fidélité des 131 titres zh/ar/fa du deck Virtues |
| [`458-v22-g18-bandeaux-hors-deck-2026-09-29.md`](458-v22-g18-bandeaux-hors-deck-2026-09-29.md) | 2026-09-29 | `historical` | Bandeaux hors deck ≠ titre de l'ancêtre — tri (7 langues, fr exclu : 0 par construction) |
| [`458-v22-g19-vertus-bandeaux-tri-2026-09-29.md`](458-v22-g19-vertus-bandeaux-tri-2026-09-29.md) | 2026-09-29 | `historical` | Tri des bandeaux Virtues ≠ titre du rang (223 rangées × 8 langues) |

### #458 v22–v23 — fidélité des définitions (6)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`458-v22-g20-definitions-ru-fidelite-2026-10-03.md`](458-v22-g20-definitions-ru-fidelite-2026-10-03.md) | 2026-10-03 | `historical` | Fidélité des définitions russes — série ⑳, instrument dédié (`definitions-fidelity-instrument.py`) |
| [`458-v22-g21-definitions-zh-fidelite-2026-10-03.md`](458-v22-g21-definitions-zh-fidelite-2026-10-03.md) | 2026-10-03 | `historical` | Fidélité des définitions chinoises — série ㉑ |
| [`458-v23-g22-definitions-ar-fidelite-2026-10-03.md`](458-v23-g22-definitions-ar-fidelite-2026-10-03.md) | 2026-10-03 | `historical` | Définitions arabes — série ㉒ (chaîne #1713→…) |
| [`458-v23-g23-definitions-fa-fidelite-2026-10-03.md`](458-v23-g23-definitions-fa-fidelite-2026-10-03.md) | 2026-10-03 | `historical` | Définitions persanes — série ㉓ |
| [`458-v23-g24-definitions-pt-es-fidelite-2026-10-03.md`](458-v23-g24-definitions-pt-es-fidelite-2026-10-03.md) | 2026-10-03 | `historical` | Définitions portugaises et espagnoles — série ㉔ |
| [`458-v23-g25-definitions-en-fidelite-2026-10-03.md`](458-v23-g25-definitions-en-fidelite-2026-10-03.md) | 2026-10-03 | `historical` | Définitions anglaises — série ㉕ |

### #458 v23 — fidélité des exemples (7)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`458-v23-g27-examples-ru-fidelite-2026-10-03.md`](458-v23-g27-examples-ru-fidelite-2026-10-03.md) | 2026-10-03 | `historical` | Exemples russes — série ㉖ (l'instrument gagne `--field example`) |
| [`458-v23-g28-examples-zh-fidelite-2026-10-03.md`](458-v23-g28-examples-zh-fidelite-2026-10-03.md) | 2026-10-03 | `historical` | Exemples chinois — série ㉗ |
| [`458-v23-g29-examples-ar-fidelite-2026-10-03.md`](458-v23-g29-examples-ar-fidelite-2026-10-03.md) | 2026-10-03 | `historical` | Exemples arabes — série ㉘ |
| [`458-v23-g30-examples-fa-fidelite-2026-10-03.md`](458-v23-g30-examples-fa-fidelite-2026-10-03.md) | 2026-10-03 | `historical` | Exemples persans — série ㉙ |
| [`458-v23-g31-examples-pt-fidelite-2026-10-03.md`](458-v23-g31-examples-pt-fidelite-2026-10-03.md) | 2026-10-03 | `historical` | Exemples portugais — série ㉚ |
| [`458-v23-g32-examples-es-fidelite-2026-10-03.md`](458-v23-g32-examples-es-fidelite-2026-10-03.md) | 2026-10-03 | `historical` | Exemples espagnols — série ㉛ |
| [`458-v23-g33-examples-en-fidelite-2026-10-03.md`](458-v23-g33-examples-en-fidelite-2026-10-03.md) | 2026-10-03 | `historical` | Exemples anglais — série ㉜ |

### #458 v23 — registres, typographie, remises (6)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`458-v23-g26-registre-pt-es-2026-10-03.md`](458-v23-g26-registre-pt-es-2026-10-03.md) | 2026-10-03 | `historical` | Registre de personne pt/es du deck (instrument registre dédié, 21 contrôles) |
| [`458-v23-g34-registre-pt-es-exemples-2026-10-03.md`](458-v23-g34-registre-pt-es-exemples-2026-10-03.md) | 2026-10-03 | `historical` | Registre des exemples pt/es + cohérence par carte |
| [`458-v23-g35-tirets-typographiques-2026-10-03.md`](458-v23-g35-tirets-typographiques-2026-10-03.md) | 2026-10-03 | `historical` | Tirets typographiques — inventaire par colonne × langue |
| [`458-v23-g38-liste-relecture-native-2026-10-04.md`](458-v23-g38-liste-relecture-native-2026-10-04.md) | 2026-10-04 | `historical` | Liste de relecture native (document de remise) — le programme a été arrêté depuis (décision #458, P24-07) |
| [`458-v23-g39-registre-es-tu-tableau-2026-10-04.md`](458-v23-g39-registre-es-tu-tableau-2026-10-04.md) | 2026-10-04 | `historical` | Registre es des définitions — tableau de propositions « tú » |
| [`458-v23-g40-virtues-wikipedia-deadlinks-2026-10-10.md`](458-v23-g40-virtues-wikipedia-deadlinks-2026-10-10.md) | 2026-10-10 | `historical` | Census des liens Wikipedia des Virtues + cellules mortes — antérieur à la reprise P24-27 (#2299) |

### #458 hors vague — instruments et dossiers (3)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`458-instrument-regles-2026-10-04.md`](458-instrument-regles-2026-10-04.md) | 2026-10-04 | `historical` | Cinq défauts de mesure de l'instrument Règles réparés ou nommés (0 écriture) |
| [`458-pt-continental-mesure-2026-10-10.md`](458-pt-continental-mesure-2026-10-10.md) | 2026-10-10 | `historical` | Marqueurs du portugais continental par corpus — diagnostic demandé par l'owner (grain ②.3) |
| [`458-scenarii-huit-cartes-2026-10-01.md`](458-scenarii-huit-cartes-2026-10-01.md) | 2026-10-01 | `historical` | Les 8 cartes « non tranchées » de G5 sont 8 survies déplacées (dossier 0-écriture) |
