# Pool v23 — série ㉛ : fidélité des **exemples** espagnols du deck (mesure 0-écriture)

**Base** : `master` `0f408864`. **Instrument** : [`definitions-fidelity-instrument.py`](definitions-fidelity-instrument.py), `--field example --lang es` (capacité portée par la branche #1724 — rejouer depuis sa tête tant qu'elle n'est pas mergée). **Nature** : mesure, **aucune écriture** — `git status --porcelain Cards/` vide après la passe.

Sixième grain de la série des exemples (dispatch c.5968567990). Comme zh/ar/fa (et contrairement au pt du ㉚), **l'espagnol n'a jamais été imprimé** : `example_es` est absente des archives — REFERENCE EXPLOITABLE **0/175**. Le grain est donc une lecture pure FR↔es, sans arbitre et sans question de générations. Verdict deck : **colonne saine** (0 A, 0 M) — et c'est le grain qui **rattache le plus de cartes au réseau des jumeaux** : 10 rangées, dont un groupe à cinq langues découvert ici.

## Synthèse — 0 A / 0 M / 23 rangées observées / 145 ✓ (168 non vides)

| Verdict | Rangées |
|---|---:|
| **A — sens faux ou inversé** | **0** |
| **M — contenu manquant ou ajouté** | **0** |
| **C-note — observation consignée, aucune écriture proposée** | **23** (dont 796, état restauré gate-épinglé, cf. §4) |
| **✓ — fidèle** | **145** |
| Vide structurel (têtes de famille, `example_fr` vide aussi) | 7 (PK 1, 175, 594, 696, 798, 887, 1280 — mêmes que ru/zh/ar/fa/pt) |

**Les 168 rangées non vides ont été lues, aucun échantillonnage** (cellules multi-lignes 813/943/974 lues entières — leçon ㉗ ; voir §5 pour ce que la lecture entière révèle sur 974).

> **Erratum d'exécution (03/10, grain 2 du dispatch c.5971513525, PR de typographie des dialogues).** Le défaut de forme du §2 ci-dessous (974 es aplati) est **corrigé** : les trois répliques sont rétablies sur 3 lignes avec le marqueur **collé** `—` (usage RAE, arbitré c.5971513525 pt 2), épinglées par `CorpusDialogueTypographyGuardTests`. Le §6/943 es reçoit aussi sa ponctuation arbitrée : « —¡Deslumbrante por su estupidez! ». Le calque 673 (« Midamos el punto medio ») est corrigé par le grain 1 (#1735, « Partamos la diferencia »).

## Jumeaux inter-langues — le fait structurel du grain

es rejoint **10** familles de déviations déjà cartographiées — dont le 5-way découvert en croisant es contre les quatre autres langues (134). Il reste **fidèle** exactement là où les familles ne tiennent pas (659, 699, 1330).

| PK | Famille de déviation | zh | ar | fa | pt | **es** |
|---|---|---|---|---|---|---|
| **134** | « la croissance **suivra d'elle-même** » → « **regarder** la croissance » — **nouveau, 5-way** | ✓ 看着公司成长 | ✓ مشاهدة النمو يحدث | ✓ شاهد رشد باشید | ✓ assistir o crescimento acontecer | **✓ observar el crecimiento** |
| 79 | condition « **platine** » ajoutée à l'album | ✓ 白金唱片 | ✓ بلاتينياً | fidèle | fidèle (imprimé IDENTIQUE) | **✓ que haya sido platino** |
| 154 | membres **miroirs** (monte/descend) + « **nous** a dit » | miroir + 我们 | miroir + لنا | fidèle | miroir + nos | **miroir + Nos** (« subamos… el lado derecho está bajando ») |
| 595 | hedging « **je pense** » ajouté | 我认为 | أعتقد أنّ | fidèle | Acho que | **Creo que** |
| 638 | « une même entité » → **extraterrestre** + ordre **Égypte-premier** (le FR dit Mexique-premier) | ✓ | ✓ | ✓ | cousin (« civilização antiga » + « indevidamente ») | **✓ un extraterrestre… a estas 3 civilizaciones** |
| 707 | désimpersonnalisation « **certaines personnes** considèrent » | ✓ | ✓ (passif « est vu comme ») | ✓ | fidèle | **✓ Algunas personas consideran** |
| 726 | « fromage » **nommé** | ✓ 瑞士奶酪 | ✓ الجبن السويسري | ✓ پنیر سوئیسی | gruyère (déjà dans l'imprimé) | **✓ queso suizo** |
| 814 | perte de « **retourner** » vivre | ✓ | ✓ | ✓ | fidèle | **✓ vivir en una cueva** (sans « volver ») |
| 977 | « n'empêche **nullement** » → « **pas tous** » | ✓ | ✓ | ✓ | cousin (passé + « mortais ») | **✓ no previene todos** |
| 994 | cousin « **je reconnais** votre colère » | ✓ 我理解 | ✓ أعترف | ✓ قابل درک | fidèle | **✓ Reconozco vuestra legítima indignación** |
| 659 | « une cigarette **de plus ou de moins** » réduite à « une de plus » | ✓ | ✓ | ✓ | fidèle | **fidèle** (« un cigarrillo más o menos ») |
| 699 | restructuration circulaire | ✓ | fidèle | ✓ | ✓ | **fidèle** |
| 1330 | (contrôle — billes) | fidèle | fidèle | glissade تیرهایم (㉙) | « bolinhas de gude » (BR) | **fidèle « canicas »** |

Deux lectures du tableau :

- **134 est le premier groupe à cinq langues mesurées** : les cinq colonnes non-françaises déjà lues remplacent le même membre de phrase intransitif (« la croissance suivra d'elle-même ») par la même idée périphrastique (« regarder la croissance [arriver] »). Découvert en croisant es contre zh/ar/fa/pt — aucune des lectures seules ne l'avait isolé (aucune n'était fausse : la déviation n'était simplement pas cartographiée). **ru (㉖) et en (grain suivant) restent à croiser.**
- **es suit le cœur zh+ar partout où il tient** (79, 154, 595), **rejoint les quadruples zh+ar+fa** (638, 707, 726, 814, 977, 994), et **reste fidèle sur les deux rangées où la famille se dénoue sans lui** (659, 699 — les rangées où pt/ar respectivement dévient). Le micro-détail 638 (réordonnancement Égypte-premier) est partagé zh+ar+fa+es — quatre langues, même ordre, contre le FR.

⇒ Règle inchangée : toute arbitration sur une de ces rangées décide **toutes** les langues cochées **ensemble** — es entre dans le lot, et 134 porte désormais cinq langues.

## Drapeaux — 23, tous lus

- **7 `EMPTY-target`** : vides structurels, `example_fr` vide aussi (mêmes PK que les cinq grains précédents).
- **14 `POLARITY`** : 300, 356, 362, 614, 735, 942, 953, 977, 992, 1174, 1297, 1360, 1361, 1388 — toutes lues, **parité respectée** (no/sin/nunca vs négations FR, dont restrictifs « solo », « ni ») ; FP par construction, même famille que zh/ar/fa/pt.
- **1 paire `LONG(3.34) + CLAUSES(6>3)`** : 796 — **état restauré gate-épinglé** (glose « avocat » ; `RestoredExceptions` inclut `796:es`).
- **1 `LONG(3.37)`** : 855 — glose conservée (C-note définitive ai-01, c.5968567847).
- **0 `LENDEV`** (colonne latine — l'écran calibré par écriture ne s'applique qu'aux colonnes CJK/arabe), **0 `NUM`** (les chiffres es concordent avec le FR : « 130 km », « 10 % », « 80 % ») — contraste pt, qui écrivait les nombres en lettres (2 NUM).
- ⚠️ Vocabulaire d'écran : `LONG/SHORT` sur colonne latine, pas de ligne `CALIBRATION` — normal (cf. ㉚).
- Reproductibilité prouvée : re-passe saine **byte-identique** (`cmp` sans diff) depuis un worktree jetable à la tête #1724, retiré après usage.

## Contrôle inverse — hérité du ㉚

Aucune réparation d'instrument entre les grains (l'instrument est resté à la tête #1724 pendant toute la série ㉗-㉛) : les contrôles inverses du ㉗ (`LENDEV`/troncature) et du ㉚ (`--csv` : polarité plantée lève ✓, troncature 50 % efface la ligne `LONG+CLAUSES` ✗, témoin sain muet ✓) couvrent les écrans employés ici. ⭐ Rappel de la leçon des totaux (㉚) : le compte global de lignes drapeautées peut rester identique malgré un +1/−1 — **seul le diff par PK nomme**.

## C-notes — 23 rangées, aucune écriture proposée

### 1. Jumeaux (10) — voir le tableau ci-dessus

134, 79, 154, 595, 638, 707, 726, 814, 977, 994.

### 2. Répliques de dialogue aplaties (1)

- **PK 974** — les trois répliques sont collées en **une seule ligne** : « - No sabes conducir**.-** Pero tengo mi carnet de conducir**.-** Sí, pero… », là où le FR garde trois lignes `\n`. Mesuré au CSV brut (`repr`) : **es 813 et 943 gardent `\n`** (et le préfixe « - »), es 974 non — la cellule a manifestement perdu ses sauts de ligne à l'écriture. Défaut de forme localisé, une cellule ; es jamais imprimé ⇒ **aucune régénération impliquée**. Correction à proposer dans un grain d'écriture séparé si le réseau la juge utile.

### 3. Jeu de mots « avocat » — une seule stratégie, deux formulations (2)

- **PK 796** — le syllogisme reste naturel (« Todos los abogados… Esta fruta es un **aguacate**… ») et la glose est **méta** : « “Abogado” y “aguacate” corresponden a la misma palabra en francés, “avocat” ». État restauré gate-épinglé (`796:es`), `LONG+CLAUSES` = conséquence mécanique de la glose. Rien à proposer.
- **PK 855** — glose **directe** : « Este oso se comió un “avocat”: la palabra puede significar tanto “abogado” como “aguacate” ». Deux formulations, une seule stratégie (expliquer le jeu de mots français) — cohérent intra-es, contrairement au pt qui change de stratégie entre 796 (glose) et 855 (« manga », ㉚). C-note définitive ai-01 : garder.

### 4. Registre — hétérogénéité tú/vosotros/usted(es) (1)

- **PK 432** — « Puesto que **estáis** a favor…, que **levanten** la mano quienes… » : **deux personnes grammaticales dans la même phrase** (vosotros puis ustedes). La colonne mélange *tú* (219, 313, 1355, 1388, 1398…), *vosotros* (51, 177, 888, 973, 994…), *usted(es)* (176, 299, 356, 420, 1015, 1020, 1282, 1360, 1361…) — relevé brut, **matière du grain « registre pt/es » (dispatch, item 4)**, aucune écriture ici.

### 5. Variante régionale (1)

- **PK 153** — « su **auto** » (régionalisme LatAm) au milieu d'une colonne qui dit « **coche** » (614, 666, 1174 — Espagne). Même famille que le registre (pt : « carro », BR, ㉚).

### 6. Ponctuation (2)

- **PK 943** — « - Deslumbrante por su estupidez » **sans ponctuation finale** (le FR porte « ! ») ; et le « pourtant » du FR (« Vous m'avez pourtant dit ») n'est pas rendu (« Dijiste que »).
- **PK 133** — le FR « Il ment, puisqu'il rougit. » (neutre) devient « **¡**Miente, ya que se sonroja**!** » — exclamations ajoutées. Cosmétique.

### 7. Dérives de détail (6)

- **PK 361** — « ils **devraient donc aussi** déterminer » → « también **les corresponde** decidir » : perte du conditionnel-normatif (le « devoir » devient une convenance). PK par ailleurs porteur d'une correction ru (#1725) — aucune interaction.
- **PK 1011** — « match » → « **momento** », et « **Claro** » ajouté en tête. Modulation légère, mécanisme intact.
- **PK 673** — « Coupons la poire en deux » → « **Midamos el punto medio** » : calque gauche (l'idiome es serait « partir la difference »/« llegar a un punto medio ») ; intelligible, d'autant que la carte s'appelle « Falacia del punto medio ».
- **PK 809** — « cette **substance** chimique » → « este **polvo** químico » : précision ajoutée (poudre).
- **PK 800** — « cela pouvait **aussi** vouloir dire » → « eso podía significar » : le « aussi » n'est pas rendu.
- **PK 690** — « **cela empêcherait donc** le lièvre de combler son retard » (conditionnel) → « **impidiendo** que la liebre la alcance » (gérondif) : affaiblissement modal léger.

## N'établit pas

- **Que la colonne es du deck soit sans défaut** — lecture unique, non native.
- **La généalogie des jumeaux** — l'ancêtre commun (deck EN d'époque, conclusion ㉕) n'a pas été relu pour ce grain ; es n'ayant jamais été imprimé, il n'existe **aucun** état imprimé es : la question des générations (㉚-pt) ne se pose pas ici.
- **Les appartenances ru et en** du réseau de jumeaux — ru (㉖) et en (grain suivant) restent à croiser, en particulier pour **134** (premier 5-way mesuré).
- **Rien sur** les 7 vides structurels, les `link_*`, les 1233 rangées hors deck.
- **Aucune écriture n'a eu lieu et aucune n'est proposée.**

⛔ Gel `v2.0.0-review` respecté — aucune republication.

## Reproductibilité

```
python docs/corpus/definitions-fidelity-instrument.py --self-test
python docs/corpus/definitions-fidelity-instrument.py --field example --lang es --out <sortie>
# re-passe de parité : worktree jetable à la tête #1724, cmp byte-identique
```

- 175 cartes lues, 23 drapeaux, jointure 168/175 (157 nom + 11 position) ; REFERENCE EXPLOITABLE **0/175** (`example_es` absente des archives `['v3']`) ; pas de ligne `CALIBRATION` (colonne latine).
- ⚠️ Nécessite la tête de #1724 (`--field`, `--csv`) tant qu'elle n'est pas sur master.
