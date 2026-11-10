# #498 — Grain 9 : cadrage du schéma imité (a) pour les 7 groupes restants

**Auteur** : po-2024 (worker) · **Date** : 2026-11-10 · **Base** : master `25327fef`
**Grain** : 9 du plan du 05/11 ([#498 c.6665070534](https://github.com/ArgumentumGames/Argumentum/issues/498#issuecomment-6665070534)) — *« cadrage de (a), docs seuls, sans écriture CSV. Pour chacun des 7 groupes, une ligne par mécanisme : le nœud, le schéma AIF candidat avec son IRI sourcé (ou "aucun"), la proposition (référence, phrase ou lacune) et sa justification. »*

> ⛔ **0 écriture CSV, 0 mutation de taxonomie, 0 code C# touché.** Ce document tranche la représentation et prépare les PRs « un groupe par PR » du plan ; il n'écrit rien.

---

## 1. Périmètre, re-mesuré

Héritage par `path` (instrument du 06/11, contrôle inverse inclus). Le total reproduit à l'unité le tableau d'ai-01 : **336 nœuds sans (a) dans les 7 groupes**, dont **6 têtes de groupe hétérogènes** (2.1, 2.3, 3.3, 5.2, 6.2, 7.2 — un nœud chacune) dont aucune mécanique unique n'est vraie pour tout le sous-arbre : elles rejoignent les 8 têtes nues déjà documentées pour (c). Restent **330 nœuds** à couvrir, ventilés ci-dessous.

> ⚠️ **Correction du 10/11 (ai-01, review du grain)** : les comptes ci-dessous sont des **sans-(a)**, pas des tailles de sous-arbre — re-vérifiés 14/14 par l'instrument (définition (a) : `AIF_skosExceptionRef` non vide, ou `AIF_skosDirectRef` avec cible en `_Inference`/`_Scheme`, héritée du plus proche ancêtre porteur ; base `25327fef`). Cinq sous-groupes contiennent des porteurs internes qui expliquent l'écart sous-arbre/sans-(a) : 2.2 (porteurs 300/322/337/340 → 56 nœuds couverts, reste la tête), 2.3.2.2.1 (432), 3.3.1.3.1 (677), 5.2.1.2/.4 (836/838), 6.2.3.4 (1020).

| Groupe | Sous-groupe | Nœuds | Proposition (§3) |
|---|---|---:|---|
| 2.1 Procédé rhétorique | 2.1.2 Humour | 28 | référence (.1/.2) + phrase (.3) |
| | 2.1.3 Poésie | 52 | **lacune déclarée** |
| 2.2 Appel à l'émotion | tête seule | 1 | phrase |
| 2.3 Manipulation mentale | 2.3.2 Jeu de pouvoir | 71 | référence (.1/.4) + lacune (.2/.3) |
| | 2.3.3 Communication non verbale | 84 | **lacune déclarée** |
| 3.3 Résultat invalide | 3.3.1 Imprécision | 13 | référence |
| | 3.3.2 Erreur de calcul | 9 | référence |
| | 3.3.3 Opération inappropriée | 6 | référence |
| 5.2 Comparaison fallacieuse | 5.2.1 Comparaison abusive | 3 | lacune (.1, arbitrage ai-01 du 10/11) + phrase (.3) ; tête = lacune déjà en place |
| 6.2 Changement de cap | 6.2.1 Exigence renforcée | 18 | phrase (tête) + référence (.3/.4/.1.1.1.1) + phrase (.1/.2/.5) |
| | 6.2.2 Beurre et argent du beurre | 20 | référence (tête) |
| | 6.2.3 Exigence relâchée | 10 | référence (.1/.3) + phrase (.2/.5) |
| 7.2 Sabotage du débat | 7.2.2 Complication exagérée | 7 | référence (.2) + phrase (.1/.3) |
| | 7.2.3 Empoisonnement du puits | 8 | référence (tête) |

## 2. La tranche de représentation (demandée par ai-01)

*« `AIF_skosOther` existe déjà en prose, faut-il une colonne dédiée comme pour (c) ? »* Trois cas, trois destinations — le test est : **la destination doit rendre (a) mesurable par le même instrument que (c)**.

| Proposition | Destination | Pourquoi |
|---|---|---|
| **Référence** (un IRI qui existe déjà) | `AIF_skosExceptionRef` | C'est **exactement sa sémantique actuelle** : sur 38 cibles distinctes d'`ExceptionRef`, 32 sont des `*_Inference` et 5 des `*_Scheme` (cadrage du 21/09, §5, confirmé par l'instrument). Le nœud est *une exception au scheme S* — écrire la référence ailleurs créerait deux colonnes pour un seul métier. |
| **Phrase** (« imite… » quand AIF n'a pas le scheme) | **colonne dédiée `AIF_imitatedScheme`**, créée **une seule fois au premier PR de groupe**, sur le modèle `AIF_criticalQuestion` (#1192 : CSV + `Fallacy.cs` `.Optional()` + émission `OwlGeneratorConfig` + tests + diff des IRI) | `AIF_skosOther` porte déjà trois métiers (divergences d'arbitrage ai-01, flags type « swap deferred to owner GO », absences déclarées) — y entasser le quatrième rend la phrase « imite » non distinguishable des flags, donc (a) non mesurable. La symétrie avec (c) donne au grain 10 la même rejouabilité. |
| **Lacune déclarée** (fail-loud) | `AIF_skosOther`, avec le **marqueur déjà en place** (« No native AIF Inference scheme for… », pattern posé sur 5.2.1) | Une lacune n'est pas une mécanique : elle se **compte** (3ᵉ état du recensement `FallacySkosCensusTests`), elle ne se décrit pas. Le marqueur existant la rend déjà lisible par l'instrument. |

**Origine des IRI candidats** : uniquement le vocabulaire **déjà référencé dans le corpus** (60 concepts de `docs/ontology/aif-export/aif-canonical-concepts.csv`, émis dans l'OWL comme fragments `aif#<Name>`). ⛔ Aucun IRI du standard AIF externe n'est proposé à l'écriture sans vérification à la source **au moment du PR de groupe** — le cadrage peut nommer un candidat externe, il ne le légitime pas.

## 3. Cadrage par mécanisme

Chaque ligne : le nœud (mécanique posée **au plus haut nœud où elle est vraie**), le candidat sourcé par son porteur existant dans le corpus, la proposition, la justification. Les références sont des valeurs `AIF_skosExceptionRef` prêtes ; les phrases sont des propositions de rédaction (la colonne n'existe pas encore).

### 2.1.2 Humour — référence (.1, .2) + phrase (.3)

| Nœud | Candidat (porteur existant) | Proposition | Justification |
|---|---|---|---|
| 2.1.2.1 Comique (11) | `Deductive_Inference` (porté par 4.3 Déduction invalide, 4.2, 5.3.1.2) | **référence** | Le rire « réfute » par l'absurde : il imite la *reductio ad absurdum*, une déduction, sans la chaîne déductive. La CQ posée (« le rire a-t-il établi quelque chose sur le fond, ou a-t-il seulement évité d'y répondre ? ») nomme exactement l'imitation. |
| 2.1.2.2 Satire (13) | idem | **référence** | La satire dérive l'absurde de la thèse adverse : même imitation de la reductio, sans la preuve que l'absurde s'ensuit. |
| 2.1.2.3 Cynisme (3) | aucun | **phrase** : « imite la défiance méthodique envers les affirmations — la dévaluation généralisée tient lieu d'examen » | Le cynisme ne réfute pas : il dévalue par avance. Aucun scheme AIF ne couvre la défiance générale ; la phrase nomme ce qui est imité sans fabriquer de scheme. |

### 2.1.3 Poésie — lacune déclarée (52 nœuds, descentes testées)

> **Test des descentes (demandé par ai-01 10/11)** : les six enfants immédiats ont été passés en revue un à un — 2.1.3.1 Jeu de sonorités (7), 2.1.3.2 Répétition (13), 2.1.3.3 Adjonction (5), 2.1.3.4 Ellipse (9), 2.1.3.5 Déplacement (8), 2.1.3.6 Substitution (9) + la tête (1) = 52. Aucun ne porte une mécanique inférentielle distincte de l'ornement : tous restent dans la lacune de la tête. Aucune descente retenue.

| Nœud | Candidat | Proposition | Justification |
|---|---|---|---|
| 2.1.3 Poésie (52) | **aucun** | **lacune déclarée** : « No honest AIF scheme imitated — l'effet est ornemental (figures de style), pas inférentiel » | Les six sous-arbres (jeu de sonorités, répétition, adjonction, ellipse, déplacement, substitution) sont des **procédés stylistiques** : ils ne portent aucune proposition et n'imitent aucune inférence. La CQ posée (« l'image embellit-elle un fait établi, ou tient-elle lieu du fait qui manque ? ») constate l'absence : l'embellissement imite au mieux l'*illustration* d'un fait, qui n'est pas un scheme AIF. Fail-loud, 52 nœuds honnêtement fermés. |

### 2.2 Appel à l'émotion — phrase (tête, 1 nœud)

| Nœud | Candidat | Proposition | Justification |
|---|---|---|---|
| 2.2 tête (1) | aucun schéma général couvrant | **phrase** : « imite l'appel à l'émotion pertinent et fondé — l'émotion proportionnée à un enjeu établi (Walton : fear appeal, appeal to pity) » | Les trois enfants portent déjà chacun son scheme à conséquences (2.2.1 `PositiveConsequences`, 2.2.2 `NegativeConsequences`, 2.2.3 `EstablishedRule`), et le seul scheme émotionnel du vocabulaire (`FearAppeal_Inference`) est déjà porté par le petit-fils 2.2.2.4 Appel à la terreur. La tête ne peut ni les contredire ni dupliquer : la phrase nomme le scheme parent waltonien qu'AIF ne couvre que partiellement. |

### 2.3.2 Jeu de pouvoir — référence (.1, .4) + lacune (.2, .3)

| Nœud | Candidat (porteur existant) | Proposition | Justification |
|---|---|---|---|
| 2.3.2.1 Charme personnel (10) | `Ethotic_Inference` (porté par 7.3 Ad hominem, 1360) | **référence** | Le charme fait valoir la personne comme raison d'accepter : c'est l'ethos waltonien, dont l'ad hominem fallacieux est déjà modélisé comme exception (précédent 7.3). Le charme est sa face séductrice. |
| 2.3.2.2 Prise de contrôle (25) | **aucun** | **lacune déclarée** | La contrainte et la domination ne passent par aucune inférence imitée — le contrôle s'exerce, il ne se déduit pas. (2.3.2.2.1 Engagement porte déjà `Waste_Inference` pour sa part.) |
| 2.3.2.3 Jeu psychologique (41) | **aucun** | **lacune déclarée** | La manipulation psychologique exploite des mécanismes (culpabilisation, menace implicite) sans contenu propositionnel imité. Fail-loud sur le plus gros sous-arbre du groupe. |
| 2.3.2.4 Conditionnement social (14) | `PopularOpinion_Inference` (porté par 1.1.1, 1.2, 2.x — refs 3/70/177) | **référence** | « Tout le monde le fait / le pense » comme raison d'adopter : le conditionnement par la norme du groupe imite l'argument d'acceptation générale, dont l'appel au peuple est l'exception type (précédents 3, 70, 177). |

### 2.3.3 Communication non verbale — référence sur .5 (décision ai-01) + lacune tête (81)

> **Test des descentes (demandé par ai-01 10/11, tranché le même jour)** : les six enfants immédiats ont été passés en revue — 2.3.3.1 Paralangage (6), 2.3.3.2 Chronémique (1), 2.3.3.3 Oculésique (2), 2.3.3.4 Kinésique (70), 2.3.3.5 Apparence (3), 2.3.3.6 Haptique (1) + la tête (1) = 84. **Une seule descente retenue : 2.3.3.5 Apparence → `Sign_Inference`** (le statut affiché comme signe — c.6775643852). Les cinq autres canaux et la tête restent lacune : ils transmettent des signaux, pas des inférences.

| Nœud | Candidat | Proposition | Justification |
|---|---|---|---|
| 2.3.3 tête (81) | **aucun** au niveau tête | **lacune déclarée** : « No honest AIF scheme imitated — le canal non verbal ne porte pas de proposition » | Les canaux transmettent des **signaux**, pas des inférences. Le précédent voisin 2.3.1 Conditionnement → `Sign_Inference` couvre l'association-apprentissage, pas le canal. |
| 2.3.3.5 Apparence (PK 590, 3 — dont .1 Marqueur socioculturel, .2 Habillement) | `Sign_Inference` (porté par 2.3.1 Conditionnement) | **référence** (décision ai-01 10/11) | L'apparence affiche un statut **comme signe** : le marqueur socioculturel et l'habillement fonctionnent par association-signale, exactement la mécanique du scheme du voisin 2.3.1. |

### 3.3 Résultat invalide — référence aux trois têtes (28 nœuds)

| Nœud | Candidat (porteur existant) | Proposition | Justification |
|---|---|---|---|
| 3.3.1 Imprécision (13) | `EvidenceToHypothesis_Inference` (porté par 3.1 Généralisation abusive et 3.2 Mauvaise interprétation — **les deux voisins directs**) | **référence** | La précision apparente (fausse précision, faux équilibre, appel aux extrêmes) imite la preuve par les données : le nombre exact comme raison de croire. Les deux groupes voisins de la même famille Erreur sont déjà modélisés par ce scheme — la continuité est immédiate. |
| 3.3.2 Erreur de calcul (9) | `Deductive_Inference` (porté par 4.3 Déduction invalide, 4.2) | **référence** | Le calcul est une déduction : le double compte ou l'erreur numérique imite la rigueur déductive du calcul, l'erreur tenant lieu de démonstration (CQ posée : « le résultat du calcul est-il exact et vérifiable, ou l'erreur qu'il contient tient-elle lieu de démonstration ? »). |
| 3.3.3 Opération inappropriée (6) | `Deductive_Inference` | **référence** | L'opération mathématique appliquée à des objets qui n'y ont pas droit (sophisme mathématique, détournement de symbole, pseudo-science) imite la validité formelle de l'opération. |

### 5.2.1 Comparaison abusive — arbitré par ai-01 le 10/11

| Nœud | Candidat (porteur existant) | Proposition | Justification |
|---|---|---|---|
| 5.2.1 tête (1) | — | **lacune déjà en place** (`skosOther` : « No native AIF Inference scheme for non-faulty comparison ; DirectRef-only ») | Précédent arbitré, non rouvert ici. |
| 5.2.1.1 Comparaison incomplète (PK 835, 1) | — | **lacune déclarée** (arbitrage ai-01 du 10/11 : « 5.2.1.1 reste une lacune ») | La proposition `Analogy_Inference` (par le frère 5.2.2) avait été défendue comme analogie tronquée, en tension avec la prose arbitrée de la tête (« no native scheme for non-faulty comparison »). Le coordinateur tranche pour la **cohérence avec la tête** : pas de référence posée, la lacune de la tête couvre .1. |
| 5.2.1.3 Comparaison incohérente (1) | aucun | **phrase** : « imite la comparaison complète et à critères stables — l'incohérence des critères tient lieu de mesure » | Le défaut n'est pas la similarité mais la stabilité des critères : aucun scheme AIF ne couvre la cohérence de mesure. |

### 6.2.1 Exigence renforcée — phrase (tête + .1/.2/.5) + référence (.3/.4/feuille 977)

| Nœud | Candidat (porteur existant) | Proposition | Justification |
|---|---|---|---|
| 6.2.1 tête (PK 974 « Exigence renforcée », 1 nœud) | aucun scheme AIF « standard of proof » | **phrase** : « imite la fixation légitime d'un standard de preuve avant acceptation — l'exigence est déplacée après coup » | AIF n'a pas de scheme de charge de preuve ; la phrase nomme la norme imitée. *(Correctif du 10/11 : la carte deck « Déplacer les buts » = « Moving the goalposts » est le PK **973**, qui est la tête de groupe hétérogène **6.2** « Changement de cap », pas cette tête-ci ; le PK 974 porte « Exigence renforcée » / « Stronger requirements ».)* |
| 6.2.1.1 Objections sans fin (9) | aucun | **phrase** : « imite l'examen critique exhaustif — l'itération sans fin d'objections tient lieu de réfutation » | Le doute méthodique n'est pas un scheme AIF nommé. |
| 6.2.1.2 Confondre explication et réfutation (3) | aucun | **phrase** : « imite la réfutation — une explication concurrente est présentée comme une défaite de la thèse » | La distinction expliquer/réfuter n'a pas de scheme AIF. |
| 6.2.1.3 Preuve d'un seul type exigée (2) | `EvidenceToHypothesis_Inference` (porté par 3.1, 3.2) | **référence** | Exiger un seul type de preuve imite la norme de convergence des preuves : le scheme des données vers l'hypothèse, restreint de façon défaisable. |
| 6.2.1.4 Renverser la charge de la preuve (PK 989, 2) | `Ignorance_Inference` (porté par 1.1.1.1 Appel à l'ignorance, 4.2.3) | **référence** | Renverser la charge = traiter l'absence de preuve comme preuve d'absence : c'est l'argument d'ignorance lui-même (précédent 1.1.1.1). |
| 6.2.1.1.1.1 Solution parfaite (PK 977, 2) | `PracticalReasoning_Inference` (porté par 1.2, 4.1, 5.3.3.3.1) | **référence** | Le nirvana exige l'option parfaite : il imite le raisonnement pratique waltonien (comparer les options disponibles pour agir) en le rendant indéfaisable. **Note** : le jumeau taxonomique 7.2.2.2 Solution parfaite reçoit la même référence (§7.2.2) — la duplication deck/taxonomie ne doit pas créer deux mécaniques. |
| 6.2.1.5 Mots compliqués (1) | aucun | **phrase** : « imite la précision technique — l'obscurité terminologique tient lieu de rigueur » | L'obscurité n'est pas une classification verbale : pas de candidat honnête. |

### 6.2.2 Beurre et argent du beurre — référence à la tête (20 nœuds)

| Nœud | Candidat (porteur existant) | Proposition | Justification |
|---|---|---|---|
| 6.2.2 tête (20) | `Commitment_Inference` (porté par 4.3.2 Inconsistance, 7.3.1 Procès en incohérence, 1.3.2) | **référence** | Tout le sous-arbre (ne pas se mouiller, ménager la chèvre, démenti qui n'en est pas un, excuses factices, couverture, porte de sortie) dévoie **l'engagement de position** : il imite la prise de position cohérente dont l'argument d'engagement waltonien est la forme légitime. Les précédents 4.3.2 et 7.3.1 couvrent exactement cette mécanique pour l'incohérence. |

### 6.2.3 Exigence relâchée — référence (.1, .3) + phrase (.2, .5)

| Nœud | Candidat (porteur existant) | Proposition | Justification |
|---|---|---|---|
| 6.2.3.1 Argumentum a silentio (1) | `Ignorance_Inference` (porté par 1.1.1.1) | **référence** | Le silence comme preuve : c'est l'appel à l'ignorance par définition. |
| 6.2.3.2 Trivialisme (2) | aucun | **phrase** : « imite la tolérance aux incohérences mineures — tout admettre tient lieu de souplesse » | Aucun scheme AIF ne couvre la trivialité. |
| 6.2.3.3 Argument de l'effort notable (5) | `Waste_Inference` (porté par le frère 6.2.3.4 Coûts irrécupérables, 2.3.2.2.1) | **référence** | « On a tant investi » comme raison de continuer : la mécanique du gaspillage (waste), déjà posée sur le frère jumeau 6.2.3.4. |
| 6.2.3.4 Coûts irrécupérables (2) | — | **déjà couvert** (`Waste_Inference` en place) | — |
| 6.2.3.5 C'était il y a longtemps (1) | aucun | **phrase** : « imite la pertinence de l'ancienneté — l'âge d'un fait tient lieu d'objection » | L'objection temporelle n'a pas de scheme AIF. |

### 7.2.2 Complication exagérée — référence (.2) + phrase (.1, .3)

| Nœud | Candidat (porteur existant) | Proposition | Justification |
|---|---|---|---|
| 7.2.2.1 Méthode hypercritique (4) | aucun | **phrase** : « imite la rigueur méthodique — la complication du protocole tient lieu d'examen » | Pas de scheme AIF de rigueur procédurale. |
| 7.2.2.2 Solution parfaite (1) | `PracticalReasoning_Inference` (porté par 1.2, 4.1) | **référence** | Jumeau du PK 977 (§6.2.1) : même mécanique nirvana, même scheme imité — le raisonnement pratique rendu indéfaisable. |
| 7.2.2.3 Objection triviale (1) | aucun | **phrase** : « imite la réfutation par détail — un défaut mineur tient lieu de réfutation du tout » | La partie-pour-le-tout procédurale n'a pas de scheme AIF. |

### 7.2.3 Empoisonnement du puits — référence à la tête (8 nœuds)

| Nœud | Candidat (porteur existant) | Proposition | Justification |
|---|---|---|---|
| 7.2.3 tête (8) | `Ethotic_Inference` (porté par 7.3 Ad hominem, 1360) | **référence** | Tout le sous-arbre (questions piégées, convaincre le public plutôt que l'interlocuteur, projection, arguments factices) disqualifie **la source à l'avance** : il imite l'examen éthique légitime de la crédibilité — l'ethos waltonien — dont l'ad hominem est déjà l'exception référencée (précédent 7.3, même famille Obstruction). |

## 4. Bilan du cadrage — partition re-dérivée par l'instrument (correctif du 10/11)

> La première version de ce bilan (106 / 19 / 163) était fausse, pour **trois causes mesurées** : (1) des **tailles de sous-arbre** utilisées là où il faut compter les **sans-(a)** — l'ancienne ligne « 2.3.2.2 (25) » comptait 20 nœuds qui portent déjà (a) par héritage du porteur 2.3.2.2.1 (PK 432) ; (2) **quatre têtes intermédiaires** (2.1.2, 2.3.2, 6.2.3, 7.2.2) n'étaient affectées à rien ; (3) les arbitrages du coordinateur du 10/11 (5.2.1.1 → lacune, 2.3.3.5 → référence) n'étaient pas appliqués. Re-dérivation : définition (a) du cadrage du 06/11 (`ExceptionRef` non vide, ou `DirectRef` avec cible `_Inference`/`_Scheme`, héritée du plus proche ancêtre porteur), base `25327fef`, §1 re-vérifié **14/14**. Les lignes ci-dessous sont **non chevauchantes** : chaque nœud sans-(a) compte exactement une fois.

| Proposition | Nœuds | Détail (non chevauchant) |
|---|---:|---|
| **Référence** (`ExceptionRef` prête) | **120** | 3.3.1 (13) · 3.3.2 (9) · 3.3.3 (6) · 6.2.2 (20) · 7.2.3 (8) · 2.1.2.1 (11) · 2.1.2.2 (13) · 2.3.2.1 (10) · 2.3.2.4 (14) · 6.2.1.3 (2) · 6.2.1.4 (2) · 6.2.1.1.1.1/977 (2) · 6.2.3.1 (1) · 6.2.3.3 (5) · 7.2.2.2 (1) · **2.3.3.5 Apparence (3, décision ai-01 10/11)** |
| **Phrase** (colonne `AIF_imitatedScheme` à créer) | **25** | 2.2 tête (1) · 2.1.2.3 (3) · 6.2.1 tête (1) + .1 (7) + .2 (3) + .5 (1) · 6.2.3.2 (2) + .5 (1) · 7.2.2.1 (4) + .3 (1) · 5.2.1.3 (1) |
| **Lacune déclarée** (`skosOther`) | **181** | 2.1.3 Poésie (52, descentes testées, aucune retenue) · 2.3.3 hors .5 (81) · 2.3.2.2 hors porteur 432 (5) · 2.3.2.3 (41) · 5.2.1 tête (1, déjà en place) + 5.2.1.1 (1, arbitrage ai-01 10/11) |
| Têtes intermédiaires sans mécanique propre | 4 | 2.1.2, 2.3.2, 6.2.3, 7.2.2 (enfants hétérogènes — même statut que les têtes de groupe) |
| Têtes de groupe hétérogènes (rejoignent les 8 têtes nues) | 6 | 2.1, 2.3, 3.3, 5.2, **6.2 (PK 973 « Déplacer les buts » / « Moving the goalposts »)**, 7.2 |
| **Total** | **336** | 120 + 25 + 181 + 4 + 6 |

**Décisions tranchées par le coordinateur le 10/11** (c.6775643852) — intégrées ci-dessus :
1. **5.2.1.1 Comparaison incomplète** : reste une **lacune** (la défense `Analogy_Inference` de la première version n'est pas retenue ; cohérence avec la prose arbitrée de la tête).
2. **2.3.3.5 Apparence** : **descente retenue** → `Sign_Inference` en référence (3 nœuds), la tête 2.3.3 reste lacune sur les 81 autres.
3. **Descentes 2.3.3 et 2.1.3 testées avant fermeture** : testés enfant par enfant (§2.3.3, §2.1.3) — une retenue (2.3.3.5), aucune pour 2.1.3.

## 5. Ce que ce cadrage n'établit pas

⛔ **Aucune écriture** : rien n'est posé, tout est proposé ; chaque PR de groupe re-mesure à sa base avant d'écrire (anti-dérive `path`). · ⛔ **La justesse des 731 nœuds déjà couverts** n'est pas revisitée. · ⛔ **Aucun IRI externe** n'est validé : les candidats sont tous dans le vocabulaire référencé du corpus ; un scheme AIF externe exigerait sa vérification à la source au PR. · ⛔ **La colonne `AIF_imitatedScheme` n'existe pas** : sa création suit le modèle #1192 au premier PR de groupe, et le diff des IRI (#133) viendra avec. · ⛔ **`AIF_skosMappingType`** (DoD 2) est hors périmètre : c'est le grain 10, mesure lecture-seule.

---

*Un cadrage de (a) qui pose 120 nœuds en références sourcées par des précédents du corpus, 25 en phrases nommées, 181 en lacunes déclarées, 4 têtes intermédiaires et 6 têtes de groupe sans mécanique unique (336 au total, partition re-dérivée par l'instrument) — et qui ne fabrique aucun scheme. C'est le fail-loud du DoD appliqué à l'échelle des groupes.*
