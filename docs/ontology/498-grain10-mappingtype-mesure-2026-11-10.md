# #498 — Grain 10 : mesure du DoD 2 — `AIF_skosMappingType` cohérent sur les 116 correspondances

**Auteur** : po-2024 (worker) · **Date** : 2026-11-10 · **Base** : master `25327fef`
**Grain** : 10 du plan du 10/11 ([#498 c.6665070534](https://github.com/ArgumentumGames/Argumentum/issues/498#issuecomment-6665070534)) — *« mesure du DoD 2, lecture seule : les 116 correspondances, avec le type déclaré et l'étroitesse mesurée »*. Le DoD 2 : **« `broadMatch` quand le sophisme est plus étroit que le schéma »**.

> ⛔ **0 écriture CSV.** Un instrument de dump rejouable + ce document de verdicts. Rien d'autre.
> **Instrument** : [`tools/498-grain10-mappingtype-dump.py`](../../tools/498-grain10-mappingtype-dump.py) — reproduit les 116 lignes (PK, type déclaré, path, libellé FR, cibles) ; les verdicts vivent ici.

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

## 2. Distribution déclarée

| Type déclaré | Lignes | Part |
|---|---:|---:|
| `skos:broadMatch` | 64 | 55 % |
| `skos:closeMatch` | 33 | 28 % |
| `skos:narrowMatch` | 19 | 16 % |

## 3. Verdicts — 65 cohérentes, 25 inversées, 26 à arbitrer

### 3.1 Les 16 `narrowMatch` inversés — motif A, le plus net du corpus

Le sens SKOS de `narrowMatch` (« la cible est plus étroite que le nœud ») a été lu **à l'envers** : ces lignes sont des **feuilles précises** (régime R1) posées en narrow alors que leur cible est un scheme générique **plus large** qu'elles. Attendu : `broadMatch`.

| PK | Nœud | Cible | Pourquoi le nœud est plus étroit |
|---|---|---|---|
| 801 | Défaut d'élucidation (5.1.1.1) | VagueVerbalClassification (D) | éluder la définition est un cas de classification vague ; le frère 802 est broadM pour la même cible |
| 806 | Définition trop large (5.1.2.1.1) | ArbitraryVC (D), PropertyNotExistant (E) | cas précis de classification arbitraire |
| 807 | Définition trop restrictive (5.1.2.1.2) | idem | idem — la paire 806/807 est symétrique et devrait avoir le même type |
| 830 | Argument du dictionnaire (5.1.3.2.1) | ArbitraryVC (D), VerbalClassification (E) | cas précis d'autorité lexicale détournée |
| 831 | Sophisme définiste (5.1.3.2.2) | VagueVC (D) | cas précis de glissement définitionnel |
| 841 | Analogie étendue (5.2.2.2) | Analogy_Inference (E) | pousser l'analogie trop loin est un sous-cas de l'abus d'analogie — **le frère 842 (5.2.2.3) est broadM sur la même cible** : la fratrie porte deux types contradictoires |
| 850 | Glissement du quantificateur (5.3.1.2.1) | Deductive_Inference (E) | erreur déductive ultra-précise ; le père 849 (portée modale) est closeM — voir §3.3 |
| 852 | Contraste illicite (5.3.1.3.1) | ArbitraryVC (D) | cas précis du sophisme d'accent |
| 854 | Barbarisme (5.3.1.5) | VagueVC (D) | faute verbale précise |
| 863 | Hétérosémie (5.3.2.2.2.1) | VagueVC (D) | cas précis de polysémie ; le père 862 (homonymie) est broadM sur la même cible |
| 877 | Fausse implication (5.3.3.1) | ConflictingGoals (D) | le père 876 est broadM et le fils 878 closeM sur la même famille de cibles — trois types sur trois étages |
| 880 | Indiscrétion anonyme (5.3.3.1.1.2) | LackOfCompleteKnowledge (D), PTK (E) | technique précise |
| 881 | Propagande grise (5.3.3.2) | ExpertiseInconsistency (D), ExpertOpinion (E) | forme précise de biais d'expertise — **le frère 883 (propagande noire), même cible, est closeM** |
| 882 | Campagne de murmures (5.3.3.2.1) | LackOfCompleteKnowledge (D), PTK (E) | technique précise |
| 884 | Interrogatoire clandestin (5.3.3.3.1) | ConflictingGoals (D), PracticalReasoning (E) | technique précise |
| 885 | Prêcher le faux pour savoir le vrai (5.3.3.3.2) | Bias_Inference (D) | technique précise |

**Concentration** : 13/16 dans la famille 5 (Abus de langage), dont 9 dans le seul sous-arbre 5.3. Le motif est systémique, pas ponctuel : à corriger en une passe si l'arbitrage confirme.

### 3.2 Les 3 `narrowMatch` corrects

| PK | Nœud | Cible | Pourquoi le nœud est plus large |
|---|---|---|---|
| 677 | Pente glissante (3.3.1.3.1) | Precedent + Full + Causal SlipperySlope (E) | le concept FR englobe les trois variantes Walton — chaque cible est plus étroite que lui |
| 705 | Pente glissante (4.1.1.6) | idem | idem |
| 727 | Erreur de logique propositionnelle (4.2.1) | ModusPonens_Inference (E) | le groupe (affirmation du conséquent, négation de l'antécédent…) englobe un scheme précis |

### 3.3 Les 8 `broadMatch` inversés — motif B : têtes englobantes

Ces têtes de groupe/famille sont **plus larges** que leur cible unique (régime R3 inversé) : `narrowMatch` attendu — ou pas de `MappingType` du tout si la correspondance est trop lâche pour être honnête (à arbitrer par ligne).

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

**Motif commun** : la cible est un `DirectRef` de conflit (le type d'attaque critique), pas un scheme — la tête mappe « l'attaque la plus représentative » de son sous-arbre. La question d'arbitrage : une tête englobante doit-elle porter un `narrowMatch` vers un conflit précis, ou aucun `MappingType` (la mécanique vivant sur les descendants) ?

### 3.4 Le `closeMatch` inversé (1)

| PK | Nœud | Cible | Pourquoi |
|---|---|---|---|
| 696 | Erreur de raisonnement (racine famille 4) | Logical_Conflict (D) | tête de famille de 102 nœuds vs un conflit précis — même motif B que §3.3, mais déclaré closeM |

### 3.5 Les 26 lignes à arbitrer — motif C : `closeMatch` vs la lettre du DoD 2

Sous le régime R2, ces 22 `closeMatch` sont **défendables** (le nom FR désigne le scheme entier) ; sous la lettre du DoD 2 (« broadMatch quand le sophisme est plus étroit »), l'usage fallacieux est plus étroit que le scheme complet → `broadMatch`. **Le DoD doit trancher la convention** — c'est l'arbitrage de fond de cette mesure. Gradient honnête en deux blocs :

**Les 10 à extension réellement proche** (closeM défendable même sous une lecture stricte — le sophisme est le nom FR du scheme) :

| PK | Nœud ≈ scheme | PK | Nœud ≈ scheme |
|---|---|---|---|
| 43 | Pratique courante ↔ PopularPractice | 719 | Effet cigogne ↔ CorrelationToCause |
| 71 | Argument d'autorité ↔ ExpertOpinion | 800 | Acception vague ↔ VagueVC |
| 337 | Appel à la terreur ↔ FearAppeal | 855 | Équivoque ↔ VagueVC |
| 858 | Pente glissante sémantique ↔ VerbalSlipperySlope | 859 | Argument de la barbe ↔ VerbalSlipperySlope |
| 865 | Changement sémantique ↔ VagueVC | 886 | Polytélie ↔ ConflictingGoals |

**Les 12 penchés `broadMatch`** (feuilles précises dont l'usage fallacieux est clairement plus étroit que le scheme complet) :

| PK | Nœud ↔ scheme | PK | Nœud ↔ scheme |
|---|---|---|---|
| 803 | Concept essentiellement contesté ↔ VagueVC | 847 | Amphibologie ↔ VerbalClassification |
| 805 | Incongruité ↔ ArbitraryVC | 849 | Portée modale ↔ Deductive_Inference |
| 814 | Faux dilemme ↔ Dilemma_Inference | 851 | Accent ↔ ArbitraryVC |
| 826 | Définition incohérente ↔ VerbalClassification | 857 | Fausse précision ↔ ArbitraryVC |
| 828 | Concept volé ↔ InconsistentCommitment | 860 | Glissement lexical ↔ VagueVC |
| 836 | Classification non exclusive ↔ VerbalClassification | 864 | Jingle-jangle ↔ VagueVC |
| 839 | **Fausse analogie ↔ Analogy_Inference — tête du groupe 5.2.2** | 867 | Confusion type-exemple ↔ Example_Inference |
| 843 | Fausse équivalence ↔ Analogy_Inference | 875 | Motte castrale ↔ VagueVC |
| 844 | Sophisme d'association ↔ VerbalClassification | 879 | Compliment empoisonné ↔ OpposedCommitment |
| 845 | Amalgame ↔ ArbitraryVC | 883 | Propagande noire ↔ ExpertOpinion |
| 878 | Insinuation ↔ ConflictingGoals | 1023 | Raisonnement biaisé ↔ Bias_Inference (tête famille 6.3) |

(10 + 12 = 22 ; ces deux tableaux plus les 4 ambigus ci-dessous forment les 26 de la synthèse)

**Les 4 `broadMatch` ambigus** :

| PK | Nœud | Pourquoi ambigu |
|---|---|---|
| 3 | Argument vide (1.1.1) ↔ PopularOpinion (E) | le groupe 1.1.1 ≈ le scheme popular opinion → closeM défendable |
| 70 | Préjugé (1.2) ↔ Bias_Inference (D) + 6 schemes (E) | « préjugé » ≈ « bias » → closeM défendable ; 6 cibles E pour un type singulier |
| 177 | Langage persuasif (2.1.1) ↔ PopularOpinion + EstablishedRule (E) | 2 schemes cibles — le type ne peut être cohérent que pour les deux à la fois |
| 799 | Définition inexacte (5.1) ↔ VerbalClassification (E) | le groupe 5.1 ≈ le scheme générique VC → closeM défendable |

### 3.6 Les 65 cohérentes (récapitulatif par motif)

- **52 `broadMatch` R1** (feuilles/têtes de mécanisme ⊂ scheme) : 4, 33, 34, 133, 153, 300, 322, 340, 357, 432, 595, 614, 621, 632, 633, 658, 697, 707, 726, 750, 758, 759, 777, 802, 804, 808, 827, 834, 838, 842, 856, 862, 866, 888, 953, 1020, 1066, 1083, 1087, 1090, 1092, 1104, 1148, 1175, 1198, 1281, 1297, 1313, 1360, 1361, 1371, 1398 — dont les biais cognitifs 6.3.* (tous ⊂ leur scheme générique) et les mécanismes 2.2/4.x/7.x.
- **10 `closeMatch` R2 à extension proche** (§3.5 tableau) : 43, 71, 337, 719, 800, 855, 858, 859, 865, 886.
- **3 `narrowMatch` R3 corrects** : 677, 705, 727.

## 4. Synthèse

| Verdict | Lignes | Contenu |
|---|---:|---|
| **Cohérent** | 65 | §3.6 |
| **Inversé** | 25 | 16 narrowM-feuilles (§3.1) + 8 broadM-têtes (§3.3) + 1 closeM-tête (§3.4) |
| **À arbitrer** | 26 | 22 closeM convention-dépendants (§3.5) + 4 broadM ambigus |

**Les trois questions d'arbitrage que cette mesure pose au DoD 2** :

1. **Le sens SKOS de `narrowMatch` s'applique-t-il tel quel ?** Les 16 inversés du §3.1 le lisent à l'envers. Corriger vers `broadMatch` est mécanique (une passe, famille 5 surtout) dès le GO.
2. **Une tête englobante porte-t-elle un `MappingType` ?** Les 9 têtes du §3.3/§3.4 sont plus larges que leur cible : `narrowMatch`, ou retrait du type (la mécanique vit sur les descendants) — les deux lectures sont honnêtes, le DoD doit en choisir une.
3. **La convention d'extension pour R2** : « le sophisme est plus étroit que le scheme » au sens extension stricte (→ broadM partout, les 22 closeM partent) ou « correspondance nominative » admise (→ closeM maintenu pour les 10 les plus proches, broadM pour les 12 penchés). La lettre actuelle du DoD 2 penche pour la première ; l'usage du corpus penche pour la seconde.

## 5. Ce que cette mesure n'établit pas

⛔ **Aucune écriture** : les verdicts sont des propositions d'arbitrage, pas des corrections. · ⛔ **La justesse des cibles elles-mêmes** (pourquoi 759 « Conclusion hâtive » porte-t-elle `OppositeConsequences_Conflict` en DirectRef plutôt qu'un scheme inductif ?) est hors périmètre : la mesure porte sur le **type déclaré vs l'étroitesse**, pas sur le choix de la cible. · ⛔ **L'héritage** : la mesure ne porte que sur les 116 porteurs directs, pas sur leurs descendants. · ⛔ **Le jugement des lignes R2 est un jugement d'extension sémantique** — raisonnable mais discutable ligne par ligne ; c'est pourquoi elles sont à arbitrer et non inversées. · ⛔ **Rien sur le DoD 3** (OWL régénéré) : la PR de rafraîchissement de fin de chantier viendra avec le diff des IRI.

---

*Une mesure qui trouve 25 lignes inversées quelle que soit la convention retenue (motifs A et B), et 26 dont le sort dépend d'une convention que le DoD 2 n'a pas encore fixée. Les 16 narrowMatch à l'envers sont le défaut le plus mécanique du corpus AIF — une passe de correction les éteint dès l'arbitrage.*
