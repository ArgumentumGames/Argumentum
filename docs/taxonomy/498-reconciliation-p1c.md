# #498 — AIF two-layer reconciliation, P1 tranche-1c (SUFFIX-ONLY, famille « Erreur de raisonnement »)

**Worker** po-2024 · **Date** 2026-07-11 · **Base** master `7406bb8e` (post-#771 + proposition #773 mergée ; tranche-1 en prod) · **Statut** proposition **gated** (0 write prod CSV dans ce PR) · **Track** GO ai-01 « suite SUFFIX-ONLY par sous-lots famille (autonome) » (`msg-20260711T030324-wgh0ks`), couvert par le pilote GO #498 (jsboige 2026-06-17).

> Scope de ce PR : **docs + apply-script (dry-run)**. Aucune cellule du CSV de production n'est modifiée dans le diff. La sérialisation prod suit le flow #753/#760 (gate ai-01), après revue de cette proposition.

---

## 0. TL;DR

Suite de la réconciliation P1 (back-fill de la couche attack pour les 52 lignes skos-only). La **tranche-1** (#769/#771) a livré les **14 PRECEDENT** ; la **tranche-1b** (#773, en revue) livre les **2 PREC-TIE + 5 SUFFIX-ONLY**. Ce PR livre la **tranche-1c = 7 lignes SUFFIX-ONLY**, la **famille « Erreur de raisonnement » complète** :

| pk | sophisme | sous-famille |
|---:|---|---|
| 696 | Erreur de raisonnement (parente) | — |
| 697 | Causalité douteuse | Causalité douteuse |
| 705 | Pente glissante | Causalité douteuse / Pétition de principe |
| 719 | Effet cigogne | Causalité douteuse / Effet cigogne |
| 726 | Composition fautive | Mauvaise composition |
| 758 | Déduction invalide | Mauvaise déduction |
| 759 | Conclusion hâtive | Mauvaise déduction / Conclusion hâtive |

**Aucun** token de ces 7 lignes n'a de précédent exact in-set (vérifié précédent-check : tous `precedent_votes={}`). Modélisation Walton **au cas par cas** depuis le scheme propre + `desc_fr` (« que défait le CQ »). Le **prior de suffixe est proscrit** (contre-preuve in-set pk804 `_Conflict`→undercut, audit #770).

Distribution : **7 undercut / 0 undermine / 0 rebut**. Cette **uniformité est la signature attendue d'une famille d'erreurs de raisonnement** — le défaut porte par nature sur le **pas inférentiel** (§3 : ce n'est pas un défaut par défaut, chaque ligne est modélisée et deux nuances sont flaguées MED). `attackType` = **jugement neuf** (0-risque token, pas 0-risque modélisation) ; **0 fabrication de token** (#677). `attackedNode` déterministe (#707§4 a) → tous **RA-node**.

Back-fill **+7** : attack-typed **107 → 114** (baseline dynamique, §5). Reste après ce sous-lot : **24 SUFFIX-ONLY** (Tricherie 8, Abus de langage 7, Insuffisance 5, Erreur mathématique 4).

---

## 1. Fondation (héritée de #769 + #770)

- **Méthode** (#769 §2) : l'`attackType` de chaque ligne skos-only se dérive de **sa propre** signature skos, ancrée sur les **18 lignes fully-modeled** (ground truth token→type). Pas d'héritage mécanique du sous-sous anchor (0/19 alignées, #769 §1).
- **Audit des 18 anchors** (#770) : **16 CLEAN / 2 SOFT / 0 erreur franche, node-map 18/18 propre**. Trois trouvailles de l'audit **ancrent directement** ce sous-lot inférentiel :
  1. **suffixe non décisif** (pk804 `PropertyNotExistant_Conflict`→undercut) → le prior de suffixe est proscrit ;
  2. **slippery-slope penche undercut** (pk858 `VerbalSlipperySlope_Inference`→undercut) → ancre in-set directe pour **pk705** ;
  3. les **défauts d'inférence causale/déductive in-set sont undercut** (pk707 `CauseToEffect_Inference`, pk727 `ModusPonens_Inference`) → contrôle de cohérence pour 697/719 (causal) et 726/758 (déductif).

Node déterministe (tous undercut ici) :

| attackType | attackedNode | composant attaqué |
|---|---|---|
| undercut | `RA-node` | inférence (la règle ne s'applique pas) |
| undermine | `I-node` | prémisse (acceptabilité contestée) |
| rebut | `CA-node` | conclusion (contre-conclusion / blocage du dialogue) |

---

## 2. SUFFIX-ONLY — famille « Erreur de raisonnement », modélisation Walton (7 lignes)

Aucun token de ces lignes n'a de précédent in-set. On modélise **depuis le scheme propre + `desc_fr`** (« que défait le CQ »). **Prior de suffixe proscrit.**

| pk | sophisme | scheme propre (signature skos) | `desc_fr` (abrégé) | → type | node | conf |
|---:|---|---|---|---|---|---|
| 696 | Erreur de raisonnement (parente) | `Logical_Conflict` | « votre thèse repose sur un raisonnement incohérent » | **undercut** | RA | MED |
| 697 | Causalité douteuse | `PracticalReasoning_Inference` + `OppositeConsequences_Conflict` | « lien de cause à effet non démontré ou incorrect » | **undercut** | RA | HIGH |
| 705 | Pente glissante | slippery-slope inferences (Precedent/Full/Causal) + `RequiredSteps_Conflict` | « prédisant une séquence peu probable d'événements négatifs » | **undercut** | RA | HIGH |
| 719 | Effet cigogne | `CorrelationToCause_Inference` + `OtherCausalFactorsInvolved_Conflict` | « une corrélation implique nécessairement un lien de causalité » | **undercut** | RA | HIGH |
| 726 | Composition fautive | `Deductive_Inference` + `Inference_Scheme` + `Commitment_Conflict` | « vous combinez des propositions logiques de façon incorrecte » | **undercut** | RA | HIGH |
| 758 | Déduction invalide | `Deductive_Inference` + `Commitment_Conflict` | « conclusions qui ne découlent pas logiquement de vos prémisses » | **undercut** | RA | HIGH |
| 759 | Conclusion hâtive | `OppositeConsequences_Conflict` | « conclusions trop vite, sans suffisamment de preuves » | **undercut** | RA | MED |

- **696** — parente générique. `desc_fr` cible le **RAISONNEMENT** (le pas inférentiel) incohérent, pas les prémisses isolées → RA. **Distinction explicite avec pk777 Inconsistance** (undermine, tranche-1b) : pk777 = prémisses mutuellement contradictoires (`OpposedCommitment`/`InconsistentCommitment` → au moins une prémisse inacceptable, I-node) ; pk696 = `Logical_Conflict` générique visant la cohérence du raisonnement lui-même. **Flag MED** : alternative undermine écartée car `desc_fr` cible « raisonnement » et non « affirmations qui se contredisent ».
- **697** — le pas causal (X ⇒ Y) est non démontré/incorrect → attaque de l'inférence → RA. Contrôle (pas héritage) : in-set pk707 `CauseToEffect_Inference`→undercut.
- **705** — pente glissante : la **chaîne** inférentielle est le défaut (séquence improbable) ; `RequiredSteps_Conflict` challenge un maillon → RA. **Ancre in-set pk858** `VerbalSlipperySlope_Inference`→undercut (audit #770) ; contre-exemple canonique de la mémoire (677/705 : ne PAS hériter l'undermine de l'anchor attack-only ; le modeling propre donne undercut).
- **719** — corrélation ⇏ causalité : le pas est injustifié (autres facteurs causaux) → RA. `CorrelationToCause_Inference` est exactement l'inférence challengée.
- **726** — composition fautive : la **combinaison déductive** est invalide → RA. `Deductive_Inference` + `Inference_Scheme` ; in-set pk727 `ModusPonens_Inference`→undercut.
- **758** — déduction invalide : cas d'école de l'undercut, la **garantie** (prémisses ⇒ conclusion) ne tient pas sans que les prémisses soient fausses → RA. `Deductive_Inference` ; in-set pk727.
- **759** — conclusion hâtive : **saut inférentiel prématuré** → RA. Même token que pk697 (undercut). **Flag MED** : lecture alternative undermine possible (« sans suffisamment de preuves » = insuffisance de prémisses) ; écartée car le défaut central de la hasty generalization Walton est le **caractère prématuré du pas**, la CQ de suffisance portant sur le support de l'inférence.

Détail complet en colonne `justification` de [`498-reconciliation-p1c-annotations.csv`](498-reconciliation-p1c-annotations.csv).

---

## 3. Uniformité undercut — signature de famille, pas défaut par défaut

Ce sous-lot ressort **7/7 undercut**. C'est **attendu et cohérent**, non un artefact de facilité :

- **Sémantique** : « Erreur de raisonnement » regroupe les défauts du **pas inférentiel** (causalité douteuse, pente glissante, corrélation≠cause, composition/déduction fautives, conclusion hâtive). Le défaut porte structurellement sur la **règle d'inférence** (undercut/RA), pas sur l'acceptabilité d'une prémisse (undermine/I) ni sur une contre-conclusion/blocage de dialogue (rebut/CA).
- **Ancrage in-set** : **tous** les tokens d'inférence causale/déductive/slippery des 18 fully-modeled sont undercut (pk707, pk727, pk858 — audit #770). La famille hérite de cette signature par **cohérence vérifiée**, pas par prior de suffixe (proscrit).
- **Garde-fou** : chaque ligne est modélisée séparément et **deux nuances sont flaguées MED** avec leur alternative explicite — **696** (vs undermine si lu comme prémisses contradictoires ; écarté par `desc_fr`) et **759** (vs undermine si lu comme insuffisance de preuves ; écarté par la sémantique hasty-generalization). Les cinq autres (697/705/719/726/758) sont HIGH.

**Les 2 MED-flag load-bearing à revoir en priorité par ai-01 : 696 et 759.**

---

## 4. Justification par ligne

Voir colonne `justification` du CSV machine-readable. Mécanisme unique de ce sous-lot : **la règle d'inférence ne tient pas (undercut/RA)** —

- **inférence causale non justifiée** : 697 (cause⇏effet), 719 (corrélation⇏cause) ;
- **chaîne inférentielle faible** : 705 (pente glissante) ;
- **combinaison / déduction invalide** : 726 (composition), 758 (déduction) ;
- **saut inférentiel prématuré** : 759 (conclusion hâtive) ;
- **incohérence du raisonnement** : 696 (parente générique).

---

## 5. Sérialisation (flow #753/#760)

`tools/498-p1c-apply.py` — **gated, dry-run par défaut**, miroir de `tools/498-p1b-apply.py` :

- lit `498-reconciliation-p1c-annotations.csv` et **re-vérifie** que sa carte interne concorde 7/7 (assertion load-bearing) ;
- splitters byte-exact (guillemets doublés + LF encadrés), cell-fill des seules colonnes `AIF_attackType`/`AIF_attackedNode` des 7 PK ;
- pre-state : les 7 PK **vides** (fill, pas overwrite) + **portent une skos** (back-fill skos-only) ;
- preuve de **byte-preservation** (0 mismatch hors les 2 cellules × 7 lignes), well-formedness 104 cols, BOM+CRLF préservés ;
- `--write` **gaté** (ai-01), backup `tmp/Fallacies-backup-pre-p1c.csv` avant écriture pour vérif indépendante.

**Baseline dynamique (pilotée par les `--write`, pas par les merges).** Les 7 PK sont **disjoints** des 14 de la tranche-1 (#771) et des 7 de la tranche-1b (#773). La baseline dépend des **sérialisations prod `--write` effectuées**, pas des PR mergées : chaque proposition merge d'abord (docs + dry-run), son `--write` est une **étape gated séparée** (relais ai-01). Le script lit le compte **réel** et rapporte `total → total+7` sans jamais coder en dur la baseline :

- si seul le `--write` tranche-1 (#771) a tourné : `107 → 114` ;
- si les `--write` tranche-1 **et** tranche-1b ont tourné : `114 → 121`.

L'ordre est donc sans impact sur la correction (cellules disjointes). Dry-run actuel (master `7406bb8e`, proposition #773 **mergée** mais son `--write` prod **encore en attente** → baseline 107) : `107 → 114`, delta **+105 B**, **0 mismatch**, **1409×104**, CRLF+BOM préservés.

```bash
python tools/498-p1c-apply.py            # dry-run (0 write prod) — ce PR
python tools/498-p1c-apply.py --write    # APPLY 7 cellules (GATÉ — relais ai-01)
```

---

## 6. Bornes du gate

- ✅ **0 write prod CSV** dans ce PR (docs + apply-script dry-run uniquement — `git diff f34ac77c -- Cards/…/Taxonomy.csv` vide).
- ✅ **0 fabrication token #677** — aucun token AIF créé ; on type des lignes déjà skos-vettées.
- ✅ Code=truth : tokens/labels/`desc_fr` lus du CSV master `7406bb8e` ; précédents = les 18 fully-modeled (audités CLEAN, #770) ; SUFFIX-ONLY confirmé par précédent-check (tous `precedent_votes={}`).
- ✅ Prior de suffixe **proscrit** ; uniformité undercut = signature de famille vérifiée, chaque ligne modélisée, 2 MED flaguées.
- ❌ #674/#666/#596 non touchés (HOLD). Pas de self-merge — verdict QA ai-01.
- ⏸️ Sérialisation prod = étape suivante gated (relais ai-01), pas dans ce PR.

🤖 Worker po-2024 — réconciliation P1 tranche-1c (famille « Erreur de raisonnement », 7 SUFFIX-ONLY).
