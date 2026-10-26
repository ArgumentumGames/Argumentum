# Pool v23 — série ㉜ : fidélité des **exemples** anglais du deck (mesure 0-écriture)

**Base** : `master` `0f408864`. **Instrument** : [`definitions-fidelity-instrument.py`](definitions-fidelity-instrument.py), `--field example --lang en` (capacité portée par la branche #1724 — rejouer depuis sa tête tant qu'elle n'est pas mergée). **Nature** : mesure, **aucune écriture** — `git status --porcelain Cards/` vide après la passe.

Septième et dernier grain de la série des exemples (dispatch c.5968567990 : ㉖ ru, ㉗ zh, ㉘ ar, ㉙ fa, ㉚ pt, ㉛ es, ㉜ en). Deuxième grain avec arbitre imprimé (REFERENCE EXPLOITABLE 157/175) — et comme le pt (㉚), **deux générations** : 16 IDENTIQUE / 123 DIFFERE / ~12 cellules imprimées vides. Verdict deck : **colonne saine** (0 A, 0 M). Mais ce grain fait plus que mesurer la colonne : **il établit, preuves à l'appui et daté au pickaxe, l'origine des jumeaux inter-langues** — la conclusion ㉕ cesse d'être une thèse.

## Synthèse — 0 A / 0 M / 16 rangées observées / 151 ✓ · corrigées (coquilles / forme) : 1 (168 non vides)

> **Erratum d'arbitrage (03/10, ai-01, [c.5971513525](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5971513525) ; requalifié 04/10, review c.5972921914).** L'asymétrie 673 (idiome « couper la poire en deux » non rendu — cellule IDENTIQUE à l'imprimé) est tranchée **au niveau coquille seulement** : « let's meetup » employait le nom « meetup » comme verbe → **« meet up »** (PR grain 1). L'en imprimé garde son texte ; l'observation d'idiome (calque absent) reste ouverte pour un éventuel grain d'écriture. Une coquille d'espacement verbe/adverbe n'est pas un « A » (review #1735) — la colonne reste **0 A / 0 M**. Verdict courant : **0 A / 0 M / 16 rangées observées / 151 ✓ · corrigées (coquilles / forme) : 1 (673)**.

| Verdict | Rangées |
|---|---:|
| **A — sens faux ou inversé** | **0** |
| **M — contenu manquant ou ajouté** | **0** |
| **C-note — observation consignée, aucune écriture proposée** | **17** (dont 796, état restauré gate-épinglé) |
| **✓ — fidèle** | **151** |
| Vide structurel (têtes de famille, `example_fr` vide aussi) | 7 (PK 1, 175, 594, 696, 798, 887, 1280 — mêmes que les six grains précédents) |

**Les 168 rangées non vides ont été lues, aucun échantillonnage** (multi-lignes 813/974 lues entières — et l'en les garde propres, cf. §6).

## Le fait du grain — l'origine des jumeaux, prouvée par la colonne EN elle-même

L'imprimé EN porte **mot pour mot** les déviations que les decks zh/ar/fa/pt/es portent aujourd'hui, précisément sur les rangées où le deck EN **actuel** a été réaligné sur le FR courant :

| PK | Déviation (famille) | Imprimé EN (v3) | Deck EN | Decks actuels |
|---|---|---|---|---|
| 79 | condition « platine » | « a record that **went platinum** » ✓ | fidèle | zh+ar+es |
| 154 | miroir + « nous » | « He told **us** to go **upwards**… the right side is going **downwards** » ✓ | fidèle | zh+ar+pt+es |
| 595 | hedging « je pense » | « **I think** cats are inoffensive » ✓ | fidèle | zh+ar+pt+es |
| 638 | extraterrestre + Égypte-premier + 3 civilisations | « an **alien** taught these **3 distinct civilizations** » ✓ | fidèle | zh+ar+fa+es |
| 699 | restructuration circulaire | « God exists because the Bible says it **and can't be wrong** » ✓ | fidèle | zh+fa+pt |
| 814 | perte du « retourner » | « you may well **go live** in a cave » ✓ | fidèle (« going **back** to living ») | zh+ar+fa+es |
| 596 | « price-quality ratio » | « the best **price-quality ratio** » ✓ | fidèle | pt (« relação preço-qualidade », dérive consignée ㉚) |
| 977 | « deadly » + passé | « hasn't eliminated **deadly** car crashes » ✓ | fidèle (« does nothing to prevent ») | **pt seul** (« mortais », cousin ㉚) — zh+ar+fa+es suivent l'AUTRE famille (« pas tous ») |
| 644 | reformatage ADN | « The **culprit and the suspect share** a DNA trait… **90% chance** » ✓ | fidèle | **es seul** (calque mot à mot, non isolé au ㉛) |
| 994 | « je reconnais » | « **I understand** your legitimate anger » ✓ | dévie encore (« **I recognize** ») | zh+ar+fa+es (cousin) |

### Chronologie mesurée (pickaxe, 12 chaînes)

| Date | Commit | Geste mesuré |
|---|---|---|
| 2025-07-02 | `e8482fe5` | l'EN « d'ère platine » est écrit (79/154/595/638/699/814…) **et** les traductions pt — « Acho que os gatos » date de ce commit |
| (entre les deux) | archive v3 | capturée dans cette ère : elle porte l'EN « platine » et **pas encore** d'`example_es` |
| 2026-03-14 | `74557ea6` | traductions zh/ar/fa/es écrites — six chaînes testées (es 79/154/638, zh 79, ar 79, fa 638), toutes à ce commit ; l'EN porte alors **encore** les déviations |
| 2026-05-30 | `95db4425` | salvage #396 : le deck EN est réaligné sur le FR courant — **seul** ; les traductions restent figées sur l'ère d'avant |

⇒ **Les jumeaux sont l'artefact d'un réalignement unilatéral** : l'EN a été corrigé (2026-05-30) sans que les traductions écrites trois et neuf mois plus tôt ne le suivent. La conclusion ㉕ (« le deck EN d'époque, demi-réécrit, est l'origine des jumeaux ») devient un **fait mesuré** ; la question laissée ouverte au ㉚ — la source du miroir de 154 en pt — est close : **l'EN imprimé**. Et pt est d'une **génération antérieure** aux quatre autres (e8482fe5 vs 74557ea6), ce que son compteur (2 IDENTIQUE, ㉚) disait déjà.

### Vague B — le deck EN d'aujourd'hui est la source vivante de 8 rangées

Sur ces rangées l'EN n'a **jamais** été réaligné : les traductions (écrites 2026-03-14, après l'ère platine mais sur ces cellules-là telles quelles) suivent donc l'en **actuel** :

| PK | Deck EN | Suivi par |
|---|---|---|
| 134 | « **watch the growth happen** » | zh+ar+fa+pt+es → le 5-way de ㉛ devient un **6-way** |
| 659 | « **What difference does one more cigarette make?** » | zh+ar+fa → **4-way** (l'es est fidèle, ㉛ — inchangé) |
| 726 | « **Swiss cheese** » | zh+ar+fa+es (imprimé en : cellule vide) |
| 994 | « **I recognize** your legitimate anger » | zh+ar+fa+es |
| 690 | « **preventing** the hare… » (gérondif) | es (« impidiendo », C-note ㉛) |
| 809 | « chemical **powder** » | es (« polvo químico », C-note ㉛) |
| 844 | « **Jane** » | es (« Jane » ; le FR dit « Jeanne ») |
| 361 | « it's also **up to them** to decide » (modalité affaiblie) | es (« también les corresponde », C-note ㉛) |

⇒ Deux vagues, un mécanisme : **les traductions épousent la colonne EN au moment de leur écriture ; ses réalignements ultérieurs ne se propagent pas**. Règle d'arbitrage renforcée : toute décision sur une rangée jumelle couvre **l'EN avec les autres langues**.

## L'imprimé EN — deux générations comme pt, autre qualité

- **16 IDENTIQUE** (70, 71, 179, 603, 614, 622, 667, 670, 673, 677, 735, 752, 876, 889, 1004, 1398) / **123 DIFFERE** / **~12 cellules vides** côté imprimé là où le deck est rempli (177, 357, 420, 432, 726, 729, 833, 878, 1287, 1362, 1365, 1373).
- Contrairement au pt : **aucun `#VALUE!`, aucune MT gâtée** — un anglais cohérent portant des exemples alternatifs souvent idiomatiques (Loch Ness 989, porridge 813 — « No Scotsman adds sugar to porridge », Zénon 690, défense Chewbacca 1330, « 3 Bretons » 598, Nicolas Cage 632, « beans or eggs » 733). Coquilles ponctuelles (« the see » 784, « We must to something » 787, « Feedom of Expression » 800, « whos » 185, « theirs parents » 112) et une **note de traducteur laissée dans la cellule** (376 : « BEN je ne vois pas cen koi FR is NLP, donc je ne vois pas comment traduire… ») — l'archive est un atelier, pas seulement un état.
- Doctrine inchangée (㉚) : deux générations ⇒ **l'imprimé n'arbitre pas cellule à cellule**. Ici il fait mieux qu'arbitrer : il explique.

## Drapeaux — 40, tous lus

- **7 `EMPTY-target`** : vides structurels, `example_fr` vide aussi (mêmes PK que les six grains précédents).
- **31 `POLARITY`** : 3, 55, 71, 128, 134, 300, 313, 323, 343, 356, 598, 614, 729, 733, 768, 800, 833, 839, 845, 878, 900, 953, 977, 989, 1015, 1020, 1174, 1281, 1282, 1297, 1371 — toutes lues, parité respectée, FP par construction.
- **1 paire `LONG(2.87) + CLAUSES(6>3)`** : 796 — **état restauré gate-épinglé** (glose « avocat » ; `RestoredExceptions` inclut `796:en`).
- **1 paire `SHORT(0.49) + POLARITY`** : 43 — condensation (« Everyone speeds; it shouldn't be penalized. »), mécanisme intact — FP documenté (même famille que 1345-pt).
- **0 `LENDEV`** (colonne latine), **0 `NUM`** (chiffres conformes : 130 km, 10 %, 80 %).
- Reproductibilité prouvée : re-passe saine **byte-identique** (`cmp` sans diff) depuis un worktree jetable à la tête #1724, retiré après usage.

## Contrôle inverse — hérité (㉗/㉚)

Aucune réparation d'instrument entre les grains de la série ; les contrôles inverses du ㉗ (`LENDEV`/troncature) et du ㉚ (`--csv` : polarité plantée lève ✓, troncature 50 % efface `LONG+CLAUSES` ✗, témoin sain muet ✓) couvrent les écrans employés ici. ⭐ Rappel ㉚ : seul le diff par PK nomme, jamais le total.

## C-notes — 17 rangées, aucune écriture proposée

### 1. Jumeaux vivants côté deck-en (8) — cf. vague B ci-dessus

134, 659, 726, 994, 690, 809, 844, 361.

### 2. Titre/exemple désynchronisés (1)

- **PK 1330** — le titre en dit « **Chewbacca defense** », l'exemple en est devenu « les billes » (plus aucun Chewbacca dans la carte). L'imprimé était cohérent (il portait la défense Chewbacca complète) ; c'est le remplacement d'exemple (ère du salvage, cf. chronologie) qui a créé l'écart. **Le titre s'imprime sur la carte** : toute régénération en rendrait la désynchronisation visible. Correction à proposer dans un grain d'écriture séparé.

### 3. État restauré gate-épinglé (1)

- **PK 796** — glose méta (« “Lawyer” and “avocado” are the same word in French ») ; `LONG+CLAUSES` = conséquence mécanique de la glose. Rien à proposer.

### 4. Appositions sans glose — la stratégie propre à l'anglais (2)

- **PK 855** — « That bear ate an **avocado—or a lawyer** » ; **PK 847** — « I prefer **chicken with olives—or chicken over olives** ». L'homonymie française n'existe pas en anglais : la carte rend **les deux sens en apposition**, sans glose longue. Troisième stratégie de la série après la glose (zh/fa/es) et le remplacement (pt « manga », ㉚). 855 en position seule (pas d'arbitre imprimé).

### 5. Écran documenté (1)

- **PK 43** — `SHORT(0.49)`, cf. § Drapeaux.

### 6. Dérives stables (4)

- **PK 625** — « Royal **Bengal** tigers » : précision ajoutée (le FR dit « tigres royaux » ; l'imprimé disait « Bengal tigers »).
- **PK 670** — reformulation équivalente (« The reality of global warming is a matter of debate within the scientific community » vs « la communauté scientifique est divisée ») — IDENTIQUE imprimé, force équivalente, consignée.
- **PK 673** — l'idiome « couper la poire en deux » n'est pas rendu (« let's meetup on Tuesday », IDENTIQUE imprimé) — asymétrie mesurée avec l'es qui calque (« Midamos el punto medio », ㉛).
- **PK 1011** — « match » → « most shining moment » — même famille que l'es « momento » (㉛) : **l'en est la source**.

### Contraste de forme (hors compteur, cf. ㉛)

Le deck en garde ses dialogues propres : 974 et 813 en 3 lignes `\n` avec cadratins « — », là où l'es a aplati 974 (défaut mesuré ㉛) — la source était propre, la perte est côté es.

## N'établit pas

- **Que la colonne en du deck soit sans défaut** — lecture unique.
- **L'appartenance ru au réseau de jumeaux** — le dossier ㉖ n'est pas recroisé ici ; à faire si le réseau doit être complet avant arbitrage.
- **La qualité relative des exemples alternatifs de l'imprimé** (Loch Ness, porridge, Zénon…) — décrits, pas jugés.
- **Rien sur** les 7 vides structurels, les `link_*`, les 1233 rangées hors deck.
- **Aucune écriture n'a eu lieu et aucune n'est proposée.**

⛔ Gel `v2.0.0-review` respecté — aucune republication.

## Reproductibilité

```
python docs/corpus/definitions-fidelity-instrument.py --self-test
python docs/corpus/definitions-fidelity-instrument.py --field example --lang en --out <sortie>
# parité : worktree jetable à la tête #1724, cmp byte-identique
# généalogie : git log -S <chaîne> -- "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
```

- 175 cartes lues, 40 drapeaux, jointure 168/175 (157 nom + 11 position) ; REFERENCE EXPLOITABLE 157/175 ; pas de ligne `CALIBRATION` (colonne latine) ; 12 chaînes pickaxées (`e8482fe5`, `74557ea6`, `95db4425`).
- ⚠️ Nécessite la tête de #1724 (`--field`, `--csv`) tant qu'elle n'est pas sur master.
