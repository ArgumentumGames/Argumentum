# #1499 grain G4-C — Liste courte des pertes `remark_fr` Virtues (pré-#367 → HEAD)

**Date** : 2026-09-24 · **Lane** : po-2024 (worker) · **Base** : `origin/master` `3d887242`
**Instrument** : [`docs/corpus/virtues-remark-g4c.py`](../corpus/virtues-remark-g4c.py) — témoins a–d PASS, jointure `pk`, invariant `path` 223/223, similarité difflib **même métrique que le dossier G4** ([`1499-g4-virtues-dossier-2026-09-22.md`](1499-g4-virtues-dossier-2026-09-22.md)).
**Mandat** : Q-7 tranchée (owner 23/09 : « la réduction de taille est souhaitable ») = règle C, **défaut = garder HEAD**. Sur les **45 remarques les plus réécrites** (similarité < 0,3 sur `card=1` — le « 45 » du dossier G4 §4, **reproduit à l'identique** : 75 sim<0,3 sur l'arbre, dont 45 imprimables), relever **seulement** les pertes (fait, nuance, exactitude) ou les erreurs introduites. « L'ancien » = **pré-#367** (`b76af806^`, veille de la passe gpt-5.5 #367 du 28/05) — consigne [#458 c.5801633624](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5801633624).

**⛔ Aucune écriture CSV** — dossier d'arbitrage seul.

---

## Réponse en une ligne

Les **45 paires ont été lues une à une** (deux textes entiers chacune) : **42 sont des condensations sans perte** — soit fidèles au propos, soit le passage définition → exemple avec la définition portée par `description_fr` (vérifié cellule par cellule sur 61, 72, 73, 92, 93, 95, 134, 135) — et **3 emportent une perte signalée** (nuance ×2, fait référentiel ×1). **Aucune erreur introduite** par HEAD dans les 45 ; **aucun « revenir » proposé**. En contexte (les 30 paires sim<0,3 non imprimées, lues aussi) : **5 erreurs factuelles de l'ANCIEN corrigées par HEAD** — toutes en logique formelle.

## 1. Ce que la lecture a compté (VERIFIÉ, cellule par cellule)

| Catégorie | # | Détail |
|---|---:|---|
| Condensation fidèle (propos couvert) | 34 | reformulations raccourcies sans amputation |
| Remark → exemple, définition portée par `desc_fr` | 8 | 61, 72, 73, 92, 93, 95, 134, 135 |
| **Perte signalée (garder)** | **3** | **33 (nuance), 94 (nuance), 203 (fait)** |
| Erreur introduite par HEAD | 0 | — |
| **Total** | **45** | = le « 45 » du dossier G4 §4 ✓ |

Longueurs (45) : médiane 283 → 125 car., 45/45 raccourcies — le régime voulu par Q-7. Similarités : min 0,05 (PK 165) · médiane 0,22.

## 2. Écarts d'époque (section séparée — gestes PRÉ-agentiques, ≠ pertes agentiques)

Entre la baseline 2024 (`62b561e75`) et le pré-#367, **14 cellules fr** divergent — toutes des **normalisations d'espacement** (double espace → simple), commit `9296f75d` « normalize whitespace » : 7 titres « Inférence:  » (PK 96–103), 2 descriptions (69, 152), 5 remarks (0, 55, 98, 124, 164). **Zéro changement de fond.** La comparaison principale (pré-#367 → HEAD) n'en est donc pas polluée.

## 3. Liste courte — 3 items, deux textes entiers, verdict en mots courants

**PK 33 · Rasoir de Hanlon — garder, nuance signalée**
- Ancien (355 c.) : « Le Rasoir de Hanlon est un principe d'interprétation qui guide la réflexion pour donner la priorité aux explications les plus simples et les plus plausibles afin de limiter les hypothèses supplémentaires et d'éviter les accusations infondées. Cela permet de prévenir les sophismes liés à la théorie du complot et de favoriser les relations de coopération. »
- Actuel (134 c.) : « Avant d'accuser quelqu'un de sabotage, vérifiez si une erreur, une maladresse ou un manque d'information suffit à expliquer les faits. »
- `desc_fr` HEAD porte le principe (« ne pas attribuer à la malveillance ce qui s'explique par l'incompétence ou l'ignorance »). Perte : les **deux usages** du rasoir — parade au complotisme, apaisement des relations — ne sont plus énoncés nulle part sur la carte. L'exemple (sabotage) les suggère à peine.

**PK 94 · Validité formelle — garder, nuance signalée**
- Ancien (407 c.) : « La pertinence de la logique formelle réside dans son aspect normatif qui évalue la validité d'un raisonnement indépendamment du contenu. La forme logique d'une argumentation déterminera si les conclusions suivies sont nécessairement vraies, à condition que les prémisses le soient aussi. Ce formalisme est essentiel pour parvenir à un raisonnement non seulement concluant mais aussi universellement reconnu. »
- Actuel (83 c.) : « Même avec des termes absurdes, la forme « Si A alors B ; A ; donc B » reste valide. »
- L'exemple montre l'indépendance au contenu (le cœur de la carte). Perte : la clause **« à condition que les prémisses le soient aussi »** — validité ≠ vérité de la conclusion — absente de `desc_fr` HEAD aussi (« Évaluer un raisonnement selon sa forme logique, indépendamment de son contenu »). C'était la seule garde contre « forme valide donc conclusion vraie ».

**PK 203 · Concentration sur l'essentiel — garder, fait signalé**
- Ancien (278 c.) : « Il est important de discuter les éléments les plus critiques d'un sujet de discussion pour en maximiser l'efficacité. Les méthodes d'identification et de priorisation des éléments clés tels que le mind mapping ou la méthode PRIOR peuvent être utilisées pour aider à cette tâche. »
- Actuel (117 c.) : « Avant de débattre d'une réforme, identifiez les deux ou trois enjeux décisifs : coût, effets attendus et faisabilité. »
- Perte : les **deux méthodes nommées** (mind mapping, méthode PRIOR) disparaissent de la carte — les seules références pratiques du corpus pour cette vertu, absentes de `desc_fr` HEAD également.

## 4. Hors liste courte — ce que la lecture a vu corriger par HEAD

**Cinq erreurs factuelles de l'ancien, corrigées** (parmi les 30 paires sim<0,3 **non imprimées**, lues en contexte — argument pour le régime C) :

- **PK 102 · Résolution unitaire** — l'ancien définissait le *raisonnement synthétique* (anatomie), hors sujet ; HEAD donne la règle correcte (« A vrai ⇒ ¬A ou B se réduit à B »).
- **PK 110 · Syllogisme de 2ᵉ figure** — l'ancien décrivait la 1ʳᵉ figure (exemple Socrate) ; HEAD donne Cesare (chat/reptile/serpent).
- **PK 113 · Festino** — l'ancien prétendait Festino = « deux applications du syllogisme Baroco » (faux) ; HEAD donne EIO-2 correct (poisson/mammifère).
- **PK 123 · Camenes** — l'ancien donnait un schéma de 3ᵉ figure (Darapti) qui ne conclut pas ; HEAD donne AEE-4 avec forme et exemple.
- **PK 143 · Comparaison adéquate** — l'ancien inversait le critère (« similitudes *plutôt que* différences », contredisant PK 142) ; HEAD montre l'équilibre (échecs : stratégie oui, coopération oubliée).

Et dans les 45 : dé-duplication des remarks identiques 17/18 (agence de santé vs climatologue), visée améliorée PK 54 (l'ancien définissait le chantage émotionnel générique, HEAD vise le chantage *aux conséquences* du titre), coquilles corrigées (« poisition » 57, « confirmation du biais » → « biais de confirmation » 131, « la retenu »/« entrainer » 158, « encourge » 213, « met l'accent » 218).

## 5. Ce que ce dossier n'établit pas

- ⛔ Les 123 `remark_fr` de similarité 0,3–0,6 (`card=1`) : hors du « 45 » dispatché — le seuil est un rideau de lecture, pas une frontière de qualité.
- ⛔ `title_fr` (53 cellules, traité par G1-W côté Fallacies) et `description_fr` (128) : hors périmètre.
- ⛔ Les autres langues (le grain est fr) ; la qualité littéraire (règle C) ; le rendu (QA visuelle 80/80 déjà passée).
- ⚠️ La similarité difflib sous-estime les remplacements lexicaux (leçon #1516) — compensé ici par la lecture intégrale des 45.

## DoD

- [x] 45/45 lues cellule par cellule (+30 non imprimées en contexte), paires entières, instrument committé.
- [x] Liste courte : 3 items, deux textes cités **en entier**, verdict et raison en mots courants — **tous « garder »**, 0 « revenir ».
- [x] Écarts d'époque isolés en section séparée (14 cellules, toutes d'espacement).
- [x] ⛔ Zéro écriture CSV (dossier + instrument seulement).
- [x] Le « 45 » du dossier G4 §4 reproduit exactement (75 arbre / 45 imprimables).

---
*Mesuré et lu par `myia-po-2024` (worker lane) le 2026-09-24. Instrument stdlib, lecture seule stricte. Reproduire : `python docs/corpus/virtues-remark-g4c.py --pairs <fichier>`.*
