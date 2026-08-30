# 2026-08-30 — #498 AIF chantier, P3 palier 1 : les 6 têtes à ancre de sous-sous-famille

**Scope**: première tranche P3 — modélisation AIF **fraîche** (plus de docs ratifiés à sérialiser,
contrairement à P2) des 6 lignes arbitrées palier 1 par ai-01 (`msg-20260830T095516-fu6h6e`,
remplace pzb8qy §3) : `1024 1174 698 420 1011 667`. **Proposition only — GATED, 0 write au CSV
de prod.**

**Repo reference**: master `b59961eb` (post-#1234, skos-modeled = 116). Issue: #498.
Predecessors: P2-A write (#1234, `49ef5fad`), verrou de vocabulaire #1233 (`d5829b57`).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no OWL regen. Les 6 propositions ci-dessous
> dérivent code=truth du CSV (ancres mesurées) + du vocabulaire natif 60 tokens épinglé par
> `FallacyAifVocabularyLockTests`. Arbitrage ai-01 → GO owner → apply gated.

---

## TL;DR

Les 6 lignes du palier 1 sont toutes des **têtes de sous-sous-famille depth-3** (le compte
« ancres SSS » d'ai-01 compte leurs *descendants* modélisés, pas des sœurs). Trois familles de
cas émergent :

- **3 héritages directs** (`1024`, `1174`, `420`) — le token du parent modélisé s'applique
  tel quel à l'enfant, précédent massif dans P2-A (25+ paires parent-enfant partagent un token :
  801-803←800, 841-843←839, 856+←855…). Risque de fabrication minimal — c'est le palier 1
  comme promis.
- **1 héritage de stratégie de famille** (`1011`) — hérite du pattern critères-manipulés du
  parent 973. Marqué FLAG léger : l'attaque est `undercut/RA-node` alors que le parent est
  `undermine/I-node`.
- **2 FAIL-LOUD** (`698`, `667`) — aucune ligne de circularité ni de défaut-de-précision
  n'existe nulle part dans les 116 modélisées (vérifié : 699/829/948 circulaires toutes
  unmapped ; aucune notion de précision dans le vocabulaire 60). Le note seule (sans refs)
  serait une **forme nouvelle** — FLAG pour arbitrage avant tout write.

**Vérification des comptes d'ancres** : reproduits 25/25 avec la définition « modélisé = au
moins une cellule skos (dir/exc/other/map), moins soi-même, moins le contingent P3 ». Ma
première sonde (filtre DirectRef-seul) contredisait les chiffres d'ai-01 — c'était l'instrument
qui était fautif : le corpus porte une forme « exc-only » (432, 1020, 1066…) que ce filtre
sous-compte. Le classement par paliers d'ai-01 **tient exactement**.

---

## 1. État des 6 lignes (code=truth, master `b59961eb`)

| pk | path | famille | sous-sous-famille | attaque prod | ancres SSS (modélisées) |
|---:|---|---|---|---|---|
| **1024** | 6.3.1 | Tricherie | Biais naturels | undermine/I-node | **7** : 1066, 1083, 1087, 1090, 1092, 1104, 1148 |
| **1174** | 6.3.2 | Tricherie | Biais culturels | undermine/I-node | **2** : 1175, 1198 |
| **698** | 4.1.1 | Erreur de raisonnement | Pétition de principe | undermine/I-node | **1** : 705 (Pente glissante — taxonomiquement distante) |
| **420** | 2.3.2 | Influence | Jeu de pouvoir | undermine/I-node | **1** : 432 (Engagement, Waste_Inference) |
| **1011** | 6.2.3 | Tricherie | Exigence relâchée | undercut/RA-node | **1** : 1020 (Coûts irrécupérables, Waste_Inference) |
| **667** | 3.3.1 | Erreur mathématique | Imprécision | undermine/I-node | **1** : 677 (Pente glissante — taxonomiquement distante) |

## 2. Propositions

### 2.1 — pk 1024 « Biais naturel » (Tricherie > Raisonnement biaisé > Biais naturels)

**Parent modélisé** : 1023 « Raisonnement biaisé » (d2) — `dir=Bias_Inference`,
`exc=PresumptiveInference_Scheme`, `skos:closeMatch`, undermine/I-node. Autre précédent :
pk 70 « Préjugé » (d2) porte aussi `dir=Bias_Inference`.

| champ | proposé |
|---|---|
| `AIF_skosDirectRef` | `Bias_Inference` |
| `AIF_skosExceptionRef` | — |
| `AIF_skosMappingType` | `skos:narrowMatch` |

**Justification** : les biais naturels sont une *sous-classe stricte* du raisonnement biaisé —
héritage du token parent (précédent : 801-803←800, 841-843←839). `narrowMatch` car la relation
est une restriction (naturels ⊂ biaisés), le parent étant déjà `closeMatch`. Forme
scheme-as-DirectRef conforme aux têtes undermine/I 800/1023. Les 7 descendants modélisés
gardent leurs schémas par-feuille (Sign, Example, PositiveConsequences…) — rien n'est écrasé,
cette proposition ne touche que la ligne 1024.

### 2.2 — pk 1174 « Biais culturel » (Tricherie > Raisonnement biaisé > Biais culturels)

Même héritage que 1024 (SSF sœur sous le même parent 1023) :

| champ | proposé |
|---|---|
| `AIF_skosDirectRef` | `Bias_Inference` |
| `AIF_skosExceptionRef` | — |
| `AIF_skosMappingType` | `skos:narrowMatch` |

**Justification** : biais culturels = sous-classe stricte du raisonnement biaisé, symétrique
exacte de 1024. Les 2 ancres in-SSF (1175 `Example_Inference`, 1198 `Preference_Scheme`) sont
toutes deux exc-only/broadMatch — cohérent avec une tête qui porte le token générique de
famille en DirectRef. **Effet cascade (fu6h6e a)** : modéliser 1024 + 1174 crée les ancres
adjacentes de 1242 (Biais théoriques, palier 2) — traiter les trois Biais comme une séquence.

### 2.3 — pk 420 « Jeu de pouvoir » (Influence > Manipulation mentale > Jeu de pouvoir)

**Parent modélisé** : 356 « Manipulation mentale » (d2) — `dir=ConflictingGoals_Conflict`,
`skos:broadMatch`, undermine/I-node.

| champ | proposé |
|---|---|
| `AIF_skosDirectRef` | `ConflictingGoals_Conflict` |
| `AIF_skosExceptionRef` | — |
| `AIF_skosMappingType` | `skos:broadMatch` |

**Justification** : le jeu de pouvoir *est* l'instanciation relationnelle du conflit de buts —
les objectifs du manipulateur s'opposent au jugement autonome de la cible. Héritage direct du
token parent (356), `broadMatch` comme la sœur 357 (Conditionnement) sous le même parent.
L'ancre in-SSF 432 (Engagement, `Waste_Inference`) est spécifique au mécanisme
coûts-irrécupérables — pas empruntable pour la tête, et non écrasée.

### 2.4 — pk 1011 « Baisser la barre » (Tricherie > Déplacement des critères > Exigence relâchée) — ⚠ FLAG léger

**Parent modélisé** : 973 « Déplacement des critères » (d2) — `dir=ConflictingGoals_Conflict`,
`skos:broadMatch`, undermine/I-node.

| champ | proposé |
|---|---|
| `AIF_skosDirectRef` | `ConflictingGoals_Conflict` |
| `AIF_skosExceptionRef` | — |
| `AIF_skosMappingType` | `skos:broadMatch` |

**Justification** : baisser la barre est la stratégie sœur du déplacement de critères (même
sous-famille « manipuler le seuil d'acceptation ») : les critères relâchés entrent en conflit
avec les standards légitimes de l'enquête. Ancre in-SSF 1020 (`Waste_Inference`) = spécifique
au sunk-cost, pas empruntable.

**⚠ FLAG** : l'attaque prod de 1011 est `undercut/RA-node` alors que le parent 973 est
`undermine/I-node` — l'héritage du token *skos* ne préjuge pas de la couche d'attaque (elle
reste telle quelle en prod), mais c'est le seul des 3 héritages où parent et enfant diffèrent
sur la couche d'attaque. Arbitrage demandé.

### 2.5 — pk 698 « Pétition de principe » — ⛔ FAIL-LOUD, forme nouvelle

| champ | proposé |
|---|---|
| `AIF_skosDirectRef` | — |
| `AIF_skosExceptionRef` | — |
| `AIF_skosMappingType` | — |
| `AIF_skosOther` | `No native AIF Inference scheme for circular reasoning (petitio principii) in the pinned 60-token vocabulary; the single in-sub-sub anchor (705, slippery slope) is taxonomically distant. Candidates considered and rejected: Dialogue_Scheme (users 1281/1313 are rebut-attack dialogue withdrawals, not undermine/I circularity); Deductive_Inference / ModusPonens_Inference (name valid forms, not the failure); Inference_Scheme (too generic to carry the circularity structure). Gap serialized per #677.` |

**Fondé par mesure** : aucune circularité modélisée dans les 116 (699 « Argument circulaire »,
829 « Définition circulaire », 948 « Référencement circulaire » : toutes unmapped). L'ancre
unique 705 (Pente glissante, `RequiredSteps_Conflict`) est sémantiquement étrangère à la
circularité — l'emprunter serait une fabrication.

**⚠ FLAG forme** : il n'existe **aucune ligne note-only** dans le corpus (les 5 skosOther
actuels — 805-807, 834, 847 — portent tous des refs). Une ligne à note seule est une forme
nouvelle ; elle comptera « modélisée » au recensement (définition = au moins une cellule skos)
ce qui est le comportement voulu (#677 : la note EST la sérialisation du gap), mais l'arbitrage
doit le confirmer explicitement avant write.

### 2.6 — pk 667 « Imprécision » — ⛔ FAIL-LOUD, forme nouvelle

| champ | proposé |
|---|---|
| `AIF_skosDirectRef` | — |
| `AIF_skosExceptionRef` | — |
| `AIF_skosMappingType` | — |
| `AIF_skosOther` | `No native AIF scheme for precision-failure (an imprecise result presented as valid support) in the pinned 60-token vocabulary; the single in-sub-sub anchor (677, slippery slope) is taxonomically distant. Statistics/precision concepts have no AIF counterpart (EvidenceToHypothesis_Inference names the evidence relation, not the precision defect). Gap serialized per #677.` |

Même fondement et même FLAG forme que 698.

## 3. Verrou de vocabulaire

Tous les tokens proposés (`Bias_Inference`, `ConflictingGoals_Conflict`) sont dans les 60
épinglés de `FallacyAifVocabularyLockTests` — **0 token novel**, aucune modification du verrou
requise. Les deux FAIL-LOUD n'introduisent aucun token.

## 4. Gates et bornes

- ⛔ **0 écriture CSV prod** — proposition docs only, GO owner requis pour tout write.
- La cascade Biais (1024 → 1174 → 1242) : traiter en séquence, 1242 reste palier 2.
- `219`/`247` (Humour/Poésie) hors périmètre de ce document (palier 3, LAST).
- Fichiers : ce document + `498-reconciliation-p3a-palier1-annotations.csv` (13 colonnes,
  convention BOM+CRLF du P2). Rien d'autre.

Refs #498
