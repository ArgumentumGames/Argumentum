# #1499 grain ④ G4bis — Planche d'arbitrage Virtues : échantillon stratifié `remark_fr`, baseline 2024 → HEAD

**Date** : 2026-09-24 · **Lane** : po-2023 (worker) · **Base** : `origin/master` `0681242b`
**Instrument** : [`docs/corpus/virtues-remark-g4bis.py`](../corpus/virtues-remark-g4bis.py) — témoins (a)-(d) PASS · métrique difflib **identique** à [`virtues-remark-g4c.py`](../corpus/virtues-remark-g4c.py) (PR #1530) et au [dossier G4](1499-g4-virtues-dossier-2026-09-22.md) (PR #1513).
**Ordre** ([#458 c.5784688699](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5784688699), grain 4) : « ~10 `remark_fr` **stratifiées** parmi les 45 à similarité < 0,3 (+ 2-3 témoins à similarité haute), baseline `62b561e75` vs HEAD côte à côte, sur une page. Objectif : rendre le choix de régime A / B / C de #1513 tranchable sans ouvrir l'issue. »

**⛔ Aucune écriture CSV** — planche d'arbitrage seule. Les textes des paires ci-dessous sont la **sortie intégrale de l'instrument** (non retapés).

---

## Réponse en une ligne

Le tirage stratifié a retenu **10 paires sur les 45** (critère §1, écrit avant le tirage) : les 10 montrent le même mouvement — **2024 énonce la règle en prose, HEAD la fait voir par un cas concret** — et les 3 témoins de similarité haute (0,89 · 0,83 · 0,78, haut du spectre) le montrent en version légère. La **seule question ouverte est Q-13 (PK 94, « valide ≠ vrai »)**, portée par l'annexe (§4 ; liste courte G4-C : 3 pertes, toutes « garder »).

## 1. Critère de stratification — écrit AVANT le tirage

| Élément | Règle déclarée | Mesure |
|---|---|---|
| Population | `remark_fr` de `card=1`, `sim(2024 → HEAD) < 0,30` | **45** (sur 131 remarks modifiées, 223 pks) |
| Recoupement | — | population identique au « 45 » mesuré sur pré-#367 → HEAD : **45 communs, 0 écart** |
| Strates | 3 bandes : B1 [0,00-0,10) · B2 [0,10-0,20) · B3 [0,20-0,30) | **4 · 12 · 29** |
| Allocation | proportionnelle (plus fort reste), total 10 | **1 · 3 · 6** |
| Tirage | déterministe, sans RNG (pk croissants, indices uniformément répartis) | PK **135 · 9 · 134 · 214 · 2 · 57 · 128 · 170 · 200 · 213** |
| Couverture familles | rapportée, non contrainte | **6 familles / 8** (Échange enrichissant 3 · Justesse lexicale 2 · Argument pertinent 2 · Présentation intègre 1 · Inférence maîtrisée 1 · Honnêteté intellectuelle 1) |
| Témoins | 3 plus hautes similarités (`card=1`) ; seuil déclaré 0,90 | seuil **non atteint** (0 paire) → montrées : **PK 91 (0,891) · 104 (0,826) · 141 (0,778)** |
| Distribution | rapportée | <0,30 : **45** · 0,30-0,60 : **72** · 0,60-0,85 : **13** · ≥0,85 : **1** |
| Annexe | PK 33 / 94 / 203 (liste courte G4-C), hors tirage | 3 paires — portent la question ouverte Q-13 |

**Écarts d'époque** (témoin c) : 2024 → pré-#367 = **14 cellules** fr (7 titres, 5 remarks, 2 descriptions) — les normalisations d'espacement déjà documentées par G4-C §2. La population mesurée sur la paire **affichée** (2024 → HEAD) étant identique à celle du pré-#367 → HEAD (45/45, 0 écart), la planche n'est pas polluée par la couche d'époque.

## 2. Le tirage — 10 paires (critère §1)

**[B1]** **PK 135 · Définitions claires — Justesse lexicale** (similarité 0.08 · 196 → 146 car.)

- 2024 : « Définir les termes avec exactitude et cohérence est essentiel pour établir une base solide pour le discours argumentatif, évitant les malentendus et fournissant un terrain d'entente pour le débat. »
- HEAD : « Avant de débattre de « liberté », assurez-vous que vous et votre interlocuteur parlez du même concept : liberté politique, morale ou individuelle. »

**[B2]** **PK 9 · Appuyer par des citations — Argument pertinent** (similarité 0.20 · 237 → 87 car.)

- 2024 : « L'utilisation de sources fiables peut renforcer l'argumentation en apportant des exemples concrets et une crédibilité supplémentaire. Il est important de citer des sources reconnues pour leur fiabilité, leur pertinence et leur précision. »
- HEAD : « Citer une experte reconnue ou un rapport officiel donne du poids à votre argumentation. »

**[B2]** **PK 134 · Justesse lexicale — Justesse lexicale** (similarité 0.17 · 304 → 166 car.)

- 2024 : « Un langage rigoureux utilise des termes clairement définis pour éviter toute ambiguïté, imprécision, mauvaise interprétation ou confusion. Cela permet de mieux comprendre les arguments avancés et de mieux communiquer. Attention aux différences de définition des termes qui peuvent causer des malentendus. »
- HEAD : « Définissez clairement les termes que vous employez : le mot « théorie » n’a pas toujours le même sens dans une conversation courante et dans un contexte scientifique. »

**[B2]** **PK 214 · Valorisation de la provenance — Échange enrichissant** (similarité 0.18 · 200 → 112 car.)

- 2024 : « Valoriser la contribution de chaque participant encourage une diversité d'opinions et renforce l'intégration de multiples perspectives, menant à une meilleure compréhension collective du sujet abordé. »
- HEAD : « Citer la personne qui a introduit une piste utile montre que le débat progresse grâce aux contributions de tous. »

**[B3]** **PK 2 · Argument fondé — Argument pertinent** (similarité 0.29 · 287 → 163 car.)

- 2024 : « Un argument fondé est l'aboutissement d'un travail d'analyse qui tire ses conclusions de données probantes et factuelles. Cet argument est construit de manière logique, de telle sorte que chaque étape de son développement contribue à la solidité et à la validité de la position soutenue. »
- HEAD : « Un argument fondé s’appuie sur des données probantes : par exemple, défendre une mesure en citant des chiffres fiables et en expliquant le lien avec la conclusion. »

**[B3]** **PK 57 · Neutralité bienveillante — Présentation intègre** (similarité 0.22 · 349 → 135 car.)

- 2024 : « Le positionnement neutre implique d'éviter d'essayer de trouver une poisition de force pour favoriser une proposition ou une position et de garantir ainsi une évaluation équitable des arguments et des positions en présence. Il favorise l'écoute active et l'échange constructif, en évitant les conflits d'intérêt et les diverses formes de partialité. »
- HEAD : « Vous ne cherchez pas à prendre l’avantage par le ton, le statut ou la pression : vous laissez les arguments être évalués équitablement. »

**[B3]** **PK 128 · Logique informelle solide — Inférence maîtrisée** (similarité 0.26 · 298 → 136 car.)

- 2024 : « Un raisonnement informel valide doit s'appuyer sur un cadre logique intuitif, pertinent au contexte de discussion, et qui, bien qu'il s'écarte de la rigueur d'une logique formelle, conserve néanmoins les qualités d'un raisonnement bien structuré qui peut être communément accepté et compréhensible. »
- HEAD : « Un argument peut être solide sans syllogisme formel : s’il reste cohérent, pertinent et compréhensible, il peut convaincre légitimement. »

**[B3]** **PK 170 · Comprendre les biais d’autrui — Honnêteté intellectuelle** (similarité 0.28 · 476 → 148 car.)

- 2024 : « La prise en compte des biais cognitifs de l'interlocuteur, tels que l'effet de cadrage et le biais de la croyance, peut aider à mieux comprendre le point de vue de l'autre et à éviter les malentendus lors d'un échange d'idées. Prenons l'exemple du biais de conformité social : en comprenant que quelqu'un peut se ranger à l'opinion de la majorité par souci d'acceptation, on peut choisir d'aborder une discussion de manière à encourager l'expression d'une pensée indépendante. »
- HEAD : « Si une personne suit l’avis de la majorité par biais de conformité sociale, l’encourager à exprimer son propre raisonnement peut enrichir l’échange. »

**[B3]** **PK 200 · Respect du sujet — Échange enrichissant** (similarité 0.26 · 233 → 136 car.)

- 2024 : « Veiller à maintenir le focus sur le sujet de débat écarte les risques de dispersion et renforce la qualité des échanges. La capacité à concentrer la discussion autour du thème principal est un gage d'un débat cohérent et constructif. »
- HEAD : « Revenir à la question posée évite la dispersion : si le débat porte sur une mesure précise, n’élargissez pas aussitôt à tout le système. »

**[B3]** **PK 213 · Principe de non-disqualification — Échange enrichissant** (similarité 0.27 · 301 → 121 car.)

- 2024 : « Dans un échange, il est crucial d'évaluer les contributions de tous sans préjugés, indépendamment de l'identité ou des affiliations de leur auteur. La non-disqualification encourge un environnement respectueux où chaque idée peut être considérée sur la base de son mérite propre et non de son origine. »
- HEAD : « Ne rejetez pas un argument parce qu’il vient d’un militant, d’un expert ou d’un novice : jugez-le sur sa solidité propre. »

## 3. Témoins de similarité haute — le haut du spectre

Seuil déclaré 0,90 non atteint (0 paire) ; les 3 plus hautes similarités mesurées sont montrées ; distribution en §1.

**PK 91 · Cohérence des prémisses — Inférence maîtrisée** (similarité 0.89 · 185 → 156 car.)

- 2024 : « Dans l'exemple 'Si il pleut, alors les rues sont mouillées. Il pleut. Donc, les rues sont mouillées', les prémisses sont vérifiables, cohérentes et soutiennent validement la conclusion. »
- HEAD : « Dans « S’il pleut, les rues sont mouillées. Il pleut. Donc les rues sont mouillées », les prémisses sont cohérentes et soutiennent validement la conclusion. »

**PK 104 · Syllogisme valide — Inférence maîtrisée** (similarité 0.83 · 233 → 176 car.)

- 2024 : « Un syllogisme est considéré comme logiquement valide si la conclusion découle inévitablement des prémisses, sans erreur logique. Un exemple classique est : Tous les hommes sont mortels. Socrate est un homme. Donc, Socrate est mortel. »
- HEAD : « Un syllogisme est valide si la conclusion découle nécessairement des prémisses. Exemple classique : Tous les hommes sont mortels. Socrate est un homme. Donc Socrate est mortel. »

**PK 141 · Définition cohérente — Justesse lexicale** (similarité 0.78 · 186 → 156 car.)

- 2024 : « Dans le domaine du droit, des définitions cohérentes de termes comme 'contrat' ou 'négligence' sont indispensables à la formulation d'arguments juridiques et à l'interprétation des lois. »
- HEAD : « En droit, des définitions cohérentes de « contrat » ou de « négligence » sont indispensables pour formuler des arguments juridiques et interpréter les lois. »

## 4. Annexe — liste courte G4-C (hors tirage)

Référence : [liste courte G4-C](1499-g4c-virtues-liste-courte-2026-09-24.md) (PR #1530, po-2024) — 3 pertes signalées, toutes « garder ». La seule question ouverte est **Q-13 (PK 94, « valide ≠ vrai »)**.

**PK 33 · Rasoir de Hanlon — Argument pertinent** (similarité 0.21 · 355 → 134 car.)

- 2024 : « Le Rasoir de Hanlon est un principe d'interprétation qui guide la réflexion pour donner la priorité aux explications les plus simples et les plus plausibles afin de limiter les hypothèses supplémentaires et d'éviter les accusations infondées. Cela permet de prévenir les sophismes liés à la théorie du complot et de favoriser les relations de coopération. »
- HEAD : « Avant d’accuser quelqu’un de sabotage, vérifiez si une erreur, une maladresse ou un manque d’information suffit à expliquer les faits. »

**PK 94 · Validité formelle — Inférence maîtrisée** (similarité 0.18 · 407 → 83 car.)

- 2024 : « La pertinence de la logique formelle réside dans son aspect normatif qui évalue la validité d'un raisonnement indépendamment du contenu. La forme logique d'une argumentation déterminera si les conclusions suivies sont nécessairement vraies, à condition que les prémisses le soient aussi. Ce formalisme est essentiel pour parvenir à un raisonnement non seulement concluant mais aussi universellement reconnu. »
- HEAD : « Même avec des termes absurdes, la forme « Si A alors B ; A ; donc B » reste valide. »

**PK 203 · Concentration sur l’essentiel — Échange enrichissant** (similarité 0.20 · 278 → 117 car.)

- 2024 : « Il est important de discuter les éléments les plus critiques d'un sujet de discussion pour en maximiser l'efficacité. Les méthodes d'identification et de priorisation des éléments clés tels que le mind mapping ou la méthode PRIOR peuvent être utilisées pour aider à cette tâche. »
- HEAD : « Avant de débattre d’une réforme, identifiez les deux ou trois enjeux décisifs : coût, effets attendus et faisabilité. »

## 5. Ce que cette planche n'établit pas

- Elle **n'arbitre pas d'elle-même** : le régime est **tranché** (règle C, Q-7, owner 23/09 — par défaut on garde HEAD, retour arrière **item par item** sur perte ou erreur). La planche est le matériau de cet examen item par item, pas un choix A / B / C encore ouvert.
- Elle **n'évalue pas la qualité littéraire** : la réduction de taille est voulue (Q-7, owner 23/09 — règle C).
- Elle ne couvre **pas** les 123 remarks de similarité 0,30-0,60, ni `title_fr` (53) ni `description_fr` (128) — périmètre du dossier G4.
- La similarité difflib **sous-estime les remplacements lexicaux** (leçon #1516) — contre-mesure : les deux textes sont affichés **entiers**, non tronqués.
- Aucune mesure de **rendu** : la QA visuelle des 80 PDF est un verdict séparé (ai-01).

## DoD

- [x] Critère de stratification **écrit avant le tirage** (§1), tirage déterministe sans RNG.
- [x] 10 paires stratifiées + 3 témoins haute similarité + annexe G4-C, **baseline 2024 | HEAD côte à côte, textes entiers**.
- [x] Population 2024 → HEAD **identique** au « 45 » pré-#367 → HEAD ; témoins instrument (a)-(d) PASS.
- [x] ⛔ Zéro écriture CSV (instrument + planche seulement).
- [x] Reproductible : `python docs/corpus/virtues-remark-g4bis.py --out <chemin>` (lecture seule, stdlib + git).

---
*Mesuré par `myia-po-2023` (worker lane) le 2026-09-24 — signale, ne déclare pas PASS · verdict/arbitrage : ai-01 & jsboige*