# #498 — Grain 10 : mesure du DoD 2 — `AIF_skosMappingType` cohérent sur les 116 correspondances

**Auteur** : po-2024 (worker) · **Date** : 2026-11-10 · **Base** : master `25327fef`
**Grain** : 10 du plan du 10/11 ([#498 c.6665070534](https://github.com/ArgumentumGames/Argumentum/issues/498#issuecomment-6665070534)) — *« mesure du DoD 2, lecture seule : les 116 correspondances, avec le type déclaré et l'étroitesse mesurée »*. Le DoD 2 : **« `broadMatch` quand le sophisme est plus étroit que le schéma »**.

> ⛔ **0 écriture CSV.** Deux instruments lecture-seule rejouables + ce document de verdicts. Rien d'autre.
> **Instruments** : [`tools/498-grain10-mappingtype-dump.py`](../../tools/498-grain10-mappingtype-dump.py) — reproduit les 116 lignes (PK, type déclaré, path, libellé FR, cibles) ; [`tools/498-grain10-mappingtype-reclass.py`](../../tools/498-grain10-mappingtype-reclass.py) — applique Q1-Q3 aux 116 lignes et **asserte** la partition 60 inchangées + 56 reclassées, broad 103 / close 1 / narrow 4 / related 8 (révision ai-01 du 11/11 : 879 retournée, §3.5). Les verdicts vivent ici.

---

## 1. Doctrine de mesure

**Le sens SKOS des trois propriétés** (orienté source→cible) :

| Propriété déclarée | Sens SKOS | Se lit : |
|---|---|---|
| `skos:broadMatch` | la cible est plus **large** | le nœud est **plus étroit** que sa cible |
| `skos:closeMatch` | équivalence sémantique approximative | nœud ≈ cible |
| `skos:narrowMatch` | la cible est plus **étroite** | le nœud est **plus large** que sa cible |

**L'étroitesse se mesure par l'extension du concept-sophisme contre l'extension du scheme Walton** (le paradigme de l'issue : un sophisme = exception défaisable à un scheme légitime). Trois régimes :

- **R1 — feuille ou tête de mécanisme** (« Appel à l'ignorance », « Fausse analogie ») : le concept désigne l'*usage fallacieux* d'un scheme dont l'usage légitime existe → le sophisme est **plus étroit** que le scheme → `broadMatch` attendu. C'est exactement la lettre du DoD 2.
- **R2 — nom-sophisme ≈ scheme** (« Effet cigogne », « Pente glissante sémantique ») : le nom français désigne le scheme entier, usage fallacieux compris (l'usage légitime est marginal ou le nom EST le nom du scheme) → `closeMatch` défendable. **C'est ici que la lettre du DoD 2 et l'usage divergent** : sous la règle lue à la lettre, ces lignes sont `broadMatch` aussi.
- **R3 — groupe englobant** (« Pente glissante » couvrant les variantes Precedent/Full/Causal ; têtes de famille) : le nœud englobe plusieurs schemes ou une cible plus précise que lui → `narrowMatch` attendu — ou l'absence de correspondance honnête.

**Cibles multiples** (D et E, ou plusieurs E) : le type déclaré est singulier alors que les cibles sont plurielles — la cohérence s'apprécie cible par cible ; une divergence sur l'une suffit à marquer la ligne, et l'ambiguïté multi-cibles est signalée comme telle.

### 1bis. La classe émise — axe ajouté le 10/11 (correctif ai-01 : « réécrire §1 contre la classe émise »)

La doctrine ci-dessus mesure une **extension** ; elle ne regardait pas **ce que la cible est**. Or la classe émise décide du type honnête autant que l'extension. Les 116 lignes émettent quatre classes, lues au suffixe et au champ :

| Classe émise | Lecture | Où |
|---|---|---|
| **Scheme** (`*_Inference`, `*_Scheme`) | un scheme d'inférence Walton | champs D et E |
| **Conflit** (`*_Conflict`) | une **question critique** (le type d'attaque), pas un scheme | champ D surtout |
| **Union** (≥ 2 cibles dans un champ) | une correspondance plurielle pour un type singulier | D ou E |
| **ExceptionRef** (champ E non vide) | le nœud se déclare **exception** à un scheme | champ E |

**Règles opérationnelles (arbitrages ai-01 du 10/11 19:40, c.6775643852 — appliquées au §4)** :

- **Q1** — les 16 `narrowMatch` du motif A (feuilles précises, §3.1) : **`broadMatch`**.
- **Q2** — les têtes englobantes (§3.3 + §3.4) : **`narrowMatch` si la cible est un scheme unique qu'elles englobent ; `relatedMatch` sans inclusion si la cible est un conflit ou une union ; jamais retirées**.
- **Q3** — **`closeMatch` seulement si la cible de conflit est unique et nomme le même défaut ; `ExceptionRef` ou union → `broadMatch`**. Le reste des `closeMatch` (scheme cible) suit la lettre du DoD 2 : le sophisme-exception est plus étroit → `broadMatch`.

*Interprétation déclarée* : « la cible de conflit est unique » se lit dans le vocabulaire du corpus (cible `X_Conflict` unique, sans autre cible) ; le test « nomme le même défaut » compare le libellé du nœud au nom du conflit. Les trois lignes qui passent le filtre mécanique sont tranchées nommément au §3.5 — la review peut les retourner, elles sont énumérées.

## 2. Distribution déclarée → recommandée (après arbitrages du 10/11)

| Type | Déclaré | Recommandé (Q1-Q3) | Flux |
|---|---:|---:|---|
| `skos:broadMatch` | 64 | **103** | +47 entrées (16 Q1 + 29 Q3 mécanique + 878 et 879 tranchées) − 8 sorties (PK 2 → narrow, 7 têtes → related) = **+39 net** |
| `skos:closeMatch` | 33 | **1** | −32 sorties (29 Q3 mécanique + 878 et 879 tranchées + 696 → related) |
| `skos:narrowMatch` | 19 | **4** | −16 sorties (Q1) + 1 entrée (PK 2, Q2 scheme unique) = **−15 net** |
| `skos:relatedMatch` | 0 | **8** | +8 (7 têtes §3.3 + 696 §3.4 — classe introduite par Q2) |
| **Total** | **116** | **116** | **56 lignes changent de type** |

| Type déclaré | Lignes | Part |
|---|---:|---:|
| `skos:broadMatch` | 64 | 55 % |
| `skos:closeMatch` | 33 | 28 % |
| `skos:narrowMatch` | 19 | 16 % |

## 3. Verdicts — 60 inchangées, 56 reclassées (arbitrages Q1-Q3 du 10/11 appliqués, 879 retournée le 11/11)

### 3.1 Les 16 `narrowMatch` inversés — motif A, le plus net du corpus — **ARBITRÉ le 10/11 : `broadMatch` (Q1)**

Le sens SKOS de `narrowMatch` (« la cible est plus étroite que le nœud ») a été lu **à l'envers** : ces lignes sont des **feuilles précises** (régime R1) posées en narrow alors que leur cible est un scheme générique **plus large** qu'elles. Attendu : `broadMatch`. **Verdict (ai-01 10/11, c.6775643852) : les 16 passent à `broadMatch` — correction mécanique dès le GO, en une passe.**

| PK | Nœud | Cible | Pourquoi le nœud est plus étroit |
|---|---|---|---|
| 801 | Défaut d'élucidation (5.1.1.1) | VagueVerbalClassification (D) | éluder la définition est un cas de classification vague ; le frère 802 est broadM pour la même cible |
| 806 | Définition trop large (5.1.2.1.1) | ArbitraryVC (D), PropertyNotExistant (E) | cas précis de classification arbitraire |
| 807 | Définition trop restrictive (5.1.2.1.2) | idem | idem — la paire 806/807 est symétrique et devrait avoir le même type |
| 830 | Argument du dictionnaire (5.1.3.2.1) | ArbitraryVC (D), VerbalClassification (E) | cas précis d'autorité lexicale détournée |
| 831 | Sophisme définiste (5.1.3.2.2) | VagueVC (D) | cas précis de glissement définitionnel |
| 841 | Analogie étendue (5.2.2.2) | Analogy_Inference (E) | pousser l'analogie trop loin est un sous-cas de l'abus d'analogie — **le frère 842 (5.2.2.3) est broadM sur la même cible** : la fratrie porte deux types contradictoires |
| 850 | Glissement du quantificateur (5.3.1.2.1) | Deductive_Inference (E) | erreur déductive ultra-précise ; le père 849 (portée modale) est closeM — voir §3.5 |
| 852 | Contraste illicite (5.3.1.3.1) | ArbitraryVC (D) | cas précis du sophisme d'accent |
| 854 | Barbarisme (5.3.1.5) | VagueVC (D) | faute verbale précise |
| 863 | Hétérosémie (5.3.2.2.2.1) | VagueVC (D) | cas précis de polysémie ; le père 862 (homonymie) est broadM sur la même cible |
| 877 | Fausse implication (5.3.3.1) | ConflictingGoals (D) | le père 876 est broadM et le fils 878 closeM sur la même famille de cibles — trois types déclarés sur trois étages |
| 880 | Indiscrétion anonyme (5.3.3.1.1.2) | LackOfCompleteKnowledge (D), PTK (E) | technique précise |
| 881 | Propagande grise (5.3.3.2) | ExpertiseInconsistency (D), ExpertOpinion (E) | forme précise de biais d'expertise — **le frère 883 (propagande noire), même cible, est closeM déclaré** (Q3 → broad) |
| 882 | Campagne de murmures (5.3.3.2.1) | LackOfCompleteKnowledge (D), PTK (E) | technique précise |
| 884 | Interrogatoire clandestin (5.3.3.3.1) | ConflictingGoals (D), PracticalReasoning (E) | technique précise |
| 885 | Prêcher le faux pour savoir le vrai (5.3.3.3.2) | Bias_Inference (D) | technique précise |

**Concentration** : 13/16 dans la famille 5 (Abus de langage), dont 9 dans le seul sous-arbre 5.3. Le motif est systémique, pas ponctuel : arbitré (Q1) — à exécuter en une passe dès le GO.

### 3.2 Les 3 `narrowMatch` corrects — **confirmés inchangés par Q1-Q3**

| PK | Nœud | Cible | Pourquoi le nœud est plus large |
|---|---|---|---|
| 677 | Pente glissante (3.3.1.3.1) | Precedent + Full + Causal SlipperySlope (E) | le concept FR englobe les trois variantes Walton — chaque cible est plus étroite que lui |
| 705 | Pente glissante (4.1.1.6) | idem | idem |
| 727 | Erreur de logique propositionnelle (4.2.1) | ModusPonens_Inference (E) | le groupe (affirmation du conséquent, négation de l'antécédent…) englobe un scheme précis |

### 3.3 Les 8 `broadMatch` inversés — motif B : têtes englobantes — **ARBITRÉ le 10/11 : 7 → `relatedMatch` sans inclusion, PK 2 → `narrowMatch` (Q2), jamais retirées**

Ces têtes de groupe/famille sont **plus larges** que leur cible unique (régime R3 inversé). **Verdict Q2 (ai-01 10/11)** : `narrowMatch` si la cible est un **scheme unique** qu'elles englobent ; `relatedMatch` **sans inclusion** si la cible est un **conflit** ou une **union** ; ⛔ jamais de retrait du type.

| PK | Nœud | Cible | Taille du sous-arbre |
|---|---|---|---|
| 1 | Insuffisance (racine famille 1) | GeneralAcceptanceDoubt_Conflict (D) | 174 nœuds |
| 2 | Argument bâclé (1.1) | LackOfPTKReliability_Scheme (D) | 33 nœuds |
| 356 | Manipulation mentale (2.3) | ConflictingGoals_Conflict (D) | 156 nœuds |
| 833 | Comparaison fallacieuse (5.2) | BiasedClassification + ExceptionSimilarityCase (D) | 89 nœuds (5.2 + 5.3 sœurs) |
| 846 | Ambiguïté (5.3) | ArbitraryVerbalClassification (D) | 89 nœuds |
| 876 | Ambiguïté narrative (5.3.3) | ConflictingGoals_Conflict (D) | 17 nœuds |
| 973 | Changement de cap (6.2) | ConflictingGoals_Conflict (D) | 49 nœuds |
| 1280 | Obstruction (racine famille 7) | ResolvingInconsistency_Conflict (D) | 126 nœuds |

**Motif commun** : la cible est un `DirectRef` de conflit (le type d'attaque critique), pas un scheme — la tête mappe « l'attaque la plus représentative » de son sous-arbre. **Application Q2, ligne par ligne** : 7 têtes ont une cible **conflit unique** (`*_Conflict`) ou une **union** (833 : `BiasedClassification + ExceptionSimilarityCase`) → **`relatedMatch` sans inclusion** : PK 1, 356, 833, 846, 876, 973, 1280. La 8ᵉ, **PK 2 « Argument bâclé »**, vise le scheme unique `LackOfPTKReliability_Scheme` (D) qu'elle englobe → **`narrowMatch`**.

### 3.4 Le `closeMatch` inversé (1) — **ARBITRÉ le 10/11 : `relatedMatch` (Q2)**

| PK | Nœud | Cible | Pourquoi |
|---|---|---|---|
| 696 | Erreur de raisonnement (racine famille 4) | Logical_Conflict (D) | tête de famille de 102 nœuds vs un conflit précis — même motif B que §3.3, mais déclaré closeM. **Verdict Q2 : cible conflit, pas d'inclusion → `relatedMatch`** |

### 3.5 Les 32 lignes R2 + 4 ambigus — motif C : `closeMatch` vs la lettre du DoD 2 — **TRANCHÉ le 10/11 (Q3)**

> **Correctif de décompte (10/11)** : la version initiale de ce § annonçait « 22 `closeMatch` à arbitrer » en comptant « 10 + 12 » — mais le second tableau en liste **22**, pas 12. Le vrai R2 = **10 + 22 = 32** lignes, qui avec la tête 696 (§3.4) reconstitue exactement les **33 `closeMatch` déclarés** du §2. Le « 26 à arbitrer » du titre initial (et de la synthèse §4) sous-comptait de 10 — c'était le même écart que le double-comptage symétrique du §3.6 (voir ci-dessous).

Sous le régime R2, ces 32 `closeMatch` étaient **défendables** (le nom FR désigne le scheme entier) ; sous la lettre du DoD 2, l'usage fallacieux est plus étroit que le scheme complet. **Verdict Q3 (ai-01 10/11) — un filtre mécanique, puis trois tranchées nommées** :

1. **`ExceptionRef` présent ou union de cibles → `broadMatch`.**
2. **Cible de scheme unique → `broadMatch`** (DoD 2 strict : le sophisme-exception est plus étroit que le scheme).
3. **Cible de conflit unique ET même défaut → seul cas où `closeMatch` survit.**

**Les 10 à extension réellement proche** (le sophisme est le nom FR du scheme) :

| PK | Nœud ≈ cible | Sort Q3 |
|---|---|---|
| 43 | Pratique courante ↔ PopularPractice (scheme) | → `broadMatch` |
| 71 | Argument d'autorité ↔ ExpertOpinion (scheme) | → `broadMatch` |
| 337 | Appel à la terreur ↔ FearAppeal (scheme) | → `broadMatch` |
| 719 | Effet cigogne ↔ CorrelationToCause (scheme) | → `broadMatch` |
| 800 | Acception vague ↔ VagueVC (scheme) | → `broadMatch` |
| 855 | Équivoque ↔ VagueVC (scheme) | → `broadMatch` |
| 858 | Pente glissante sémantique ↔ VerbalSlipperySlope (scheme) | → `broadMatch` |
| 859 | Argument de la barbe ↔ VerbalSlipperySlope (scheme) | → `broadMatch` |
| 865 | Changement sémantique ↔ VagueVC (scheme) | → `broadMatch` |
| **886** | **Polytélie ↔ ConflictingGoals_Conflict** | **✓ `closeMatch` retenu** — cible de conflit unique, et la polytélie est *littéralement* le conflit de buts (même défaut) |

**Les 22 penchés `broadMatch`** (feuilles précises dont l'usage fallacieux est clairement plus étroit que le scheme complet) :

| PK | Nœud ↔ cible | Sort Q3 |
|---|---|---|
| 803 | Concept essentiellement contesté ↔ VagueVC | → `broadMatch` |
| 805 | Incongruité ↔ ArbitraryVC | → `broadMatch` |
| 814 | Faux dilemme ↔ Dilemma_Inference | → `broadMatch` |
| 826 | Définition incohérente ↔ VerbalClassification | → `broadMatch` |
| 828 | Concept volé ↔ InconsistentCommitment | → `broadMatch` |
| 836 | Classification non exclusive ↔ VerbalClassification | → `broadMatch` |
| 839 | Fausse analogie ↔ Analogy_Inference (tête du groupe 5.2.2) | → `broadMatch` |
| 843 | Fausse équivalence ↔ Analogy_Inference | → `broadMatch` |
| 844 | Sophisme d'association ↔ VerbalClassification | → `broadMatch` |
| 845 | Amalgame ↔ ArbitraryVC | → `broadMatch` |
| 847 | Amphibologie ↔ VerbalClassification | → `broadMatch` |
| 849 | Portée modale ↔ Deductive_Inference | → `broadMatch` |
| 851 | Accent ↔ ArbitraryVC | → `broadMatch` |
| 857 | Fausse précision ↔ ArbitraryVC | → `broadMatch` |
| 860 | Glissement lexical ↔ VagueVC | → `broadMatch` |
| 864 | Jingle-jangle ↔ VagueVC | → `broadMatch` |
| 867 | Confusion type-exemple ↔ Example_Inference | → `broadMatch` |
| 875 | Motte castrale ↔ VagueVC | → `broadMatch` |
| **879** | **Compliment empoisonné ↔ OpposedCommitment_Conflict** | **→ `broadMatch` (retournée par la review ai-01 du 11/11)** — passe le filtre mécanique (conflit unique), mais `OpposedCommitment_Conflict` est un conflit **générique** : il est aussi la cible de 777 « Inconsistance » et de 1297 « Preuve par assertion ». Le compliment empoisonné n'en est qu'une forme — et il est le fils de 878 « Insinuation », refusée pour la même raison. Plus étroit que sa cible ⇒ `broadMatch` |
| 883 | Propagande noire ↔ ExpertOpinion | → `broadMatch` |
| **878** | **Insinuation ↔ ConflictingGoals_Conflict** | **→ `broadMatch` (candidate refusée)** — passe le filtre mécanique (conflit unique) mais l'insinuation n'est pas *le* conflit de buts : défaut différent |
| 1023 | Raisonnement biaisé ↔ Bias_Inference (tête famille 6.3) | → `broadMatch` |

**Les 4 `broadMatch` ambigus** — Q3 les rend **cohérents tels quels** (union ou scheme → broad) :

| PK | Nœud | Sort Q3 |
|---|---|---|
| 3 | Argument vide (1.1.1) ↔ PopularOpinion (E) | scheme unique → `broadMatch` (inchangé) |
| 70 | Préjugé (1.2) ↔ Bias_Inference (D) + 6 schemes (E) | union (7 cibles) → `broadMatch` (inchangé) |
| 177 | Langage persuasif (2.1.1) ↔ PopularOpinion + EstablishedRule (E) | union → `broadMatch` (inchangé) |
| 799 | Définition inexacte (5.1) ↔ VerbalClassification (E) | scheme unique → `broadMatch` (inchangé) |

**Bilan §3.5** : 32 R2 → **29 `broadMatch` + 1 `closeMatch` (886) + 2 refusées vers broad (878, 879)** ; 4 ambigus inchangés broad. Des trois tranchées nommées, la review ai-01 du 11/11 a retourné **879** (initialement retenue `closeMatch`).

### 3.6 Les cohérentes après arbitrages (60 inchangées + 56 reclassées = 116)

> **Correctif de décompte (10/11)** : la version initiale comptait « 65 cohérentes » en incluant les **10 `closeMatch` R2** du §3.5 — qui y étaient simultanément « à arbitrer ». Les mêmes 10 lignes comptaient deux fois ; 65 − 10 = **55 lignes réellement inchangées à l'époque**, et la partition initiale « 65 + 25 + 26 = 116 » n'était pas une partition (65 + 25 + 36 = 126 avec le vrai R2 = 32). La partition fermée ci-dessous re-dérive tout de l'instrument ([`tools/498-grain10-mappingtype-reclass.py`](../../tools/498-grain10-mappingtype-reclass.py), rejouable, assertions intégrées).

**Les 60 inchangées (cohérentes déclarées et confirmées par Q1-Q3)** :

- **56 `broadMatch`** — les 52 feuilles/têtes de mécanisme R1 : 4, 33, 34, 133, 153, 300, 322, 340, 357, 432, 595, 614, 621, 632, 633, 658, 697, 707, 726, 750, 758, 759, 777, 802, 804, 808, 827, 834, 838, 842, 856, 862, 866, 888, 953, 1020, 1066, 1083, 1087, 1090, 1092, 1104, 1148, 1175, 1198, 1281, 1297, 1313, 1360, 1361, 1371, 1398 — **plus les 4 « ambigus » du §3.5** (3, 70, 177, 799), que Q3 rend cohérents en broad.
- **1 `closeMatch`** : 886 (Polytélie ↔ ConflictingGoals_Conflict) — seul survivant du filtre Q3 (879 retournée en review, §3.5).
- **3 `narrowMatch`** : 677, 705, 727 (§3.2).

**Les 56 reclassées (une par une, par motif)** :

| Motif | Lignes | Vers |
|---|---:|---|
| A — narrowM-feuilles inversées (§3.1, Q1) | 16 | `broadMatch` |
| B — têtes à cible conflit/union (§3.3, Q2) | 7 (1, 356, 833, 846, 876, 973, 1280) | `relatedMatch` |
| B — tête à scheme unique (§3.3, Q2) | 1 (PK 2) | `narrowMatch` |
| B — tête closeM (§3.4, Q2) | 1 (696) | `relatedMatch` |
| C — R2 scheme/union/ExceptionRef (§3.5, Q3) | 29 | `broadMatch` |
| C — R2 candidates refusées (§3.5, tranchées) | 2 (878, 879) | `broadMatch` |

## 4. Synthèse

**Partition fermée** : 60 inchangées + 56 reclassées = **116** (aucune ligne double-comptée, aucun retrait — Q2 interdit de retirer une tête).

| Type | Déclaré | Recommandé (Q1-Q3) |
|---|---:|---:|
| `skos:broadMatch` | 64 | **103** |
| `skos:closeMatch` | 33 | **1** (886) |
| `skos:narrowMatch` | 19 | **4** (677, 705, 727, 2) |
| `skos:relatedMatch` | 0 | **8** (1, 356, 696, 833, 846, 876, 973, 1280) |

**Les trois questions d'arbitrage que cette mesure posait — TRANCHÉES le 10/11 (ai-01, c.6775643852)** :

1. **Le sens SKOS de `narrowMatch` s'applique-t-il tel quel ?** → **Oui (Q1)** : les 16 du §3.1 passent à `broadMatch` — correction mécanique en une passe, famille 5 surtout.
2. **Une tête englobante porte-t-elle un `MappingType` ?** → **Oui, jamais retirée (Q2)** : scheme unique englobé → `narrowMatch` (PK 2) ; conflit ou union → `relatedMatch` sans inclusion (les 8 autres).
3. **La convention d'extension pour R2** → **Lettre stricte du DoD 2 (Q3)** : `closeMatch` réservé au cas « cible de conflit unique ET même défaut » ; tout le reste (scheme, union, ExceptionRef) → `broadMatch`. Survit : 886. Les candidates 878 et 879 sont refusées (défaut différent ; 879 retournée en review le 11/11).

## 5. Ce que cette mesure n'établit pas

⛔ **Aucune écriture** : les verdicts sont **arbitrés (10/11) mais pas exécutés** — la passe CSV viendra sur GO, en une fois. · ⛔ **La justesse des cibles elles-mêmes** (pourquoi 759 « Conclusion hâtive » porte-t-elle `OppositeConsequences_Conflict` en DirectRef plutôt qu'un scheme inductif ?) est hors périmètre : la mesure porte sur le **type déclaré vs l'étroitesse**, pas sur le choix de la cible. · ⛔ **L'héritage** : la mesure ne porte que sur les 116 porteurs directs, pas sur leurs descendants. · ⛔ **Les tranchées nommées (886, 879, 878) sont des jugements sémantiques** — raisonnable mais discutable ligne par ligne ; la review peut les retourner, elles sont énumérées au §3.5. · ⛔ **Rien sur le DoD 3** (OWL régénéré) : la PR de rafraîchissement de fin de chantier viendra avec le diff des IRI.

---

*Une mesure qui trouvait 25 lignes inversées et 36 dont le sort dépendait d'une convention non fixée — les arbitrages Q1-Q3 du 10/11 tranchent tout : 56 lignes reclassées, 0 retrait, une distribution cible broad 103 / close 1 / narrow 4 / related 8. Les 16 narrowMatch à l'envers restent le défaut le plus mécanique du corpus AIF — une passe de correction les éteint dès le GO.*
