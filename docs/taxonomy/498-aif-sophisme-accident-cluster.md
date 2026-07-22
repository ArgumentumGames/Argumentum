# 2026-07-22 — #498 AIF chantier : cluster « Sophisme de l'accident » (Erreur mathématique, Généralisation abusive)

**Scope** : dedicated cluster doc for the **Sophisme de l'accident** sub-sub (family **Erreur mathématique**, sub-family
**Généralisation abusive**) — **6 unmapped leaves** (615-620) under the anchor **614** (serialized `undercut`/`RA-node`
**with native skos on 2 layers**, like cigogne). **Proposition only — GATED, 0 write to prod CSV.** Continues the
cluster-docs backlog (ai-01 dispatch `msg-9akcg4` strate-6 deep-queue idle lane, post-T&A regime). Fourth cluster of
the dispatch (#837 + #839 + #840 + this).

**Repo reference** : master `c721066c`. Issue : #498. Predecessors : #770 (anchor audit), #837/#839 (mixed clusters),
#840 (native-rich cigogne). Layer C = **~1 252 unmapped leaves** remain after the prior clusters (this opens 6 more).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies → worker applies
> the CSV edits in a follow-up PR (gated), same flow as #753/#760/#837/#839/#840. Serialization deferred to gated apply.

---

## TL;DR

- **Sophisme de l'accident** (Erreur mathématique > Généralisation abusive, sub-sub, 7 rows) : anchor **614** serialized
  `undercut`/`RA-node` **with native skos on 2 layers** (`ExceptionSimilarityCase_Conflict` direct +
  `ExceptionalCase_Inference` exception) — a second native-rich anchor (after cigogne #840). **6 unmapped leaves** —
  615 Sophisme de l'accident (d4 twin), 616 Sophisme du vrai Écossais, 617 Sophisme de l'accident contraire, 618
  Généralisation excessive, 619 Stéréotype, 620 Exception écrasante.
- **Second native-rich cluster (after cigogne), but with scheme-DIVERSITY.** Cigogne's leaves all inherited one causal
  scheme. This cluster's leaves attack the *exception/generalization inference* via **4 distinct native schemes**
  (exceptional-case, example/induction, verbal-classification, bias) — the umbrella « apply a rule ignoring the
  exception » forks into 4 Walton scheme-families. **All 6 are native-fit** (6/6 carry a native scheme), the highest
  native-fit rate in the chantier.
- **Cluster is uniform undercut/RA-node (6/6)** — no attack-type split. The variety is entirely in the *scheme layer*
  (which Walton inference the leaf attacks). Mechanism-coherent : every leaf attacks a generalization/exception
  inference rule.
- **Two distinctive leaves** : **617 accident contraire** is the *inverse* of the anchor (erects an exception as a rule,
  rather than applying a rule ignoring the exception) — same attack-type, same scheme, honest note. **616 vrai Écossais**
  attacks via *verbal reclassification* (the « no true Scotsman » move) — a different native scheme
  (`ArbitraryVerbalClassification_Inference`) but same undercut/RA-node.
- **6 attack-typed leaves proposed, 0 fabrication, axiom 0 violation** (6 undercut→RA). Projected coverage **151 → 157 /
  1 408** once applied (live rescan master `c721066c` = 151/1 408 after #838/#839/#840 merged, 10.7 %).

---

## 1. Cluster state (code=truth, master `c721066c`)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Erreur mathématique**, sub-family
**Généralisation abusive**, sub-sub **Sophisme de l'accident** (7 rows) :

| pk | text_fr | depth | AIF_attackType | AIF_attackedNode | skos cols | State |
|----|---------|-------|----------------|------------------|-----------|-------|
| **614** | **Sophisme de l'accident** (anchor) | d3 | `undercut` | `RA-node` | direct=`ExceptionSimilarityCase_Conflict`, exc=`ExceptionalCase_Inference` | **✅ serialized (2 skos)** |
| 615 | Sophisme de l'accident | d4 | — | — | — | unmapped (**this cluster**) |
| 616 | Sophisme du vrai Écossais | d5 | — | — | — | unmapped (**this cluster**) |
| 617 | Sophisme de l'accident contraire | d4 | — | — | — | unmapped (**this cluster**) |
| 618 | Généralisation excessive | d4 | — | — | — | unmapped (**this cluster**) |
| 619 | Stéréotype | d5 | — | — | — | unmapped (**this cluster**) |
| 620 | Exception écrasante | d4 | — | — | — | unmapped (**this cluster**) |

**Leaf content (code=truth `desc_fr` / `example_fr`)** :
- **614 Sophisme de l'accident** — « Vous appliquez une règle générale à un cas particulier, sans tenir compte des
  exceptions pertinentes. » / « Ma voiture peut rouler à 130 km/h ; traverser la ville ne me prendra donc que quelques
  minutes »
- **615 Sophisme de l'accident** (d4 twin) — « Vous appliquez une règle générale à une situation particulière, sans
  tenir compte des exceptions pertinentes. » / « Couper avec un couteau est un crime. Les chirurgiens coupent… Donc
  les chirurgiens sont des criminels »
- **616 Sophisme du vrai Écossais** — « Pour réfuter un contre-exemple, vous redéfinissez les contours de l'idée
  générale que vous soutenez. » / « Tous les Écossais sont roux. Angus est écossais mais pas roux. Alors ce n'est pas
  un vrai Écossais »
- **617 Sophisme de l'accident contraire** — « Vous transformez une exception en règle générale. » / « Si l'on autorise
  les personnes souffrant de sclérose en plaques à consommer de la marijuana, alors tout le monde… »
- **618 Généralisation excessive** — « Vous accordez trop d'importance à des cas particuliers et les traitez comme
  s'ils constituaient une norme universelle. » / « pic d'agressions à la sortie des bars à 2h. Cela prouve que les
  agressions sont principalement… »
- **619 Stéréotype** — « Votre raisonnement repose sur une généralisation abusive de croyances communes qui
  catégorisent des objets, des personnes ou des groupes. » / « On m'a dit tellement de bien de la cuisine française »
- **620 Exception écrasante** — « Votre argumentation repose sur une généralisation dont la validité exige de
  nombreuses conditions particulières. » / « Ces entreprises sont très rentables parce qu'elles utilisent des
  stratégies marketing novatrices »

The sub-sub is **semantically coherent around one umbrella** — *misapplying a generalization/exception inference* —
which is why the cluster is uniform undercut/RA-node. The variety is in *which* Walton generalization scheme the leaf
attacks (exceptional-case, example/induction, verbal-classification, bias).

---

## 2. The anchor model (614, serialized) — native-rich 2-layer anchor

`614` is serialized `AIF_attackType=undercut`, `AIF_attackedNode=RA-node`, **`AIF_skosDirectRef=
ExceptionSimilarityCase_Conflict`**, **`AIF_skosExceptionRef=ExceptionalCase_Inference`**. The fallacy attacks the
**generalization-application inference rule** (RA-node) by applying a general rule to a particular case while ignoring
a pertinent exception. The anchor fits the canonical `ExceptionalCase_Inference` scheme, with the conflict being the
neglected exceptional-similarity-case.

**Native generalization-family token inventory (code=truth)** : `ExceptionalCase_Inference`, `ExceptionSimilarityCase_Conflict`,
`Example_Inference` (×4), `InductiveInference_Scheme` (×3), `GeneralAcceptanceDoubt_Conflict` (×3),
`PopularOpinion_Inference` (×3), `ArbitraryVerbalClassification_Inference` (×3), `Bias_Inference` (×2),
`BiasedClassification_Conflict` (×2), `VagueVerbalClassification_Inference` (×2) — **all natively attested**. The
cluster's leaves map onto 4 of these scheme-families (exception / example / verbal-classification / bias), giving the
**highest native-fit rate in the chantier** (6/6).

---

## 3. Proposed AIF structure for the leaves

Per-leaf derivation. `attackType` + `attackedNode` follow the AIF/ASPIC+ axiom (**0 violation**, all undercut→RA).
`skosDirectRef` / `skosExceptionRef` carry native generalization-family tokens (6/6 native-fit). **0 fabrication (#677).**

### Exception-family leaves → **undercut / RA-node** + `ExceptionalCase_Inference` (matches anchor)

#### pk 615 — Sophisme de l'accident (d4 twin) ✅ undercut
- **Mechanism** : applies a general rule (cutting with a knife = crime) to a particular case (surgeon) ignoring the
  pertinent exception (medical context). Attacks the rule-application inference (RA-node).
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef=ExceptionSimilarityCase_Conflict` (native —
  the neglected exception), `ExceptionRef=ExceptionalCase_Inference` (native — the attacked inference),
  `MappingType=skos:narrowMatch` (d4).
- **Closest to the anchor** : description quasi-identical (règle générale → cas particulier, exception négligée).
  Full 2-layer native inheritance.

#### pk 617 — Sophisme de l'accident contraire (inverse) ✅ undercut
- **Mechanism** : transforms an exception (medical marijuana) into a general rule (everyone) — the *inverse* of the
  anchor. Attacks the generalization inference (RA-node) by erecting the exception as a norm.
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef=ExceptionSimilarityCase_Conflict` (native),
  `ExceptionRef=ExceptionalCase_Inference` (native), `MappingType=skos:narrowMatch` (d4).
- **Distinctive note** : same attack-type + scheme as the anchor, but the *direction* is inverted (exception→rule vs
  rule→case). The exceptional-case scheme captures both manipulations of the exception. Honest inverse flagged.

#### pk 620 — Exception écrasante (overwhelming conditions) ✅ undercut
- **Mechanism** : relies on a generalization whose validity requires numerous particular conditions, all neglected.
  Attacks the generalization inference (RA-node) — the accumulated conditions act as exceptions.
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef=ExceptionSimilarityCase_Conflict` (native —
  the neglected conditions), `ExceptionRef=ExceptionalCase_Inference` (native), `MappingType=skos:narrowMatch` (d4).

### Example/induction-family leaf → **undercut / RA-node** + `Example_Inference`

#### pk 618 — Généralisation excessive (hasty over-generalization) ✅ undercut
- **Mechanism** : treats particular cases (2am bar assaults) as a universal norm. Attacks the *inductive* inference
  (RA-node) by over-weighting particular cases.
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef=GeneralAcceptanceDoubt_Conflict` (native —
  the over-generalized norm), `ExceptionRef=Example_Inference` (native — the attacked induction), `MappingType=skos:narrowMatch` (d4).
- **Scheme divergence from anchor** : attacks the *induction* inference (`Example_Inference`), not the exceptional-case
  application — but same attack-type (undercut/RA-node). The leaf generalizes *from* cases, where the anchor generalizes
  *to* cases ignoring exceptions.

### Verbal-classification leaf → **undercut / RA-node** + `ArbitraryVerbalClassification_Inference`

#### pk 616 — Sophisme du vrai Écossais (no-true-Scotsman) ✅ undercut
- **Mechanism** : to refute a counter-example (Angus not red-haired), redefines the contours of the general idea
  (« no *true* Scotsman »). Attacks the rule-application inference (RA-node) by saving the generalization through an
  arbitrary reclassification.
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef` **empty** (no clean native conflict token
  for « defensive reclassification » — fail-loud on DirectRef), `ExceptionRef=ArbitraryVerbalClassification_Inference`
  (native — the reclassification), `MappingType=skos:narrowMatch` (d5).
- **Distinctive** : the *mechanism* is defensive (saving the rule by reclassification) rather than applying the rule
  blindly — but the attack-type is still undercut/RA-node (the inference is attacked). Different native scheme
  (verbal-classification vs exceptional-case). Fail-loud on DirectRef.

### Bias-family leaf → **undercut / RA-node** + `Bias_Inference`

#### pk 619 — Stéréotype (categorical bias) ✅ undercut
- **Mechanism** : reasons on an abusive generalization of common categorizing beliefs (French cuisine = good food).
  Attacks the inference (RA-node) — declared biased by the stereotyped categorization.
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef=BiasedClassification_Conflict` (native — the
  stereotyped categorization), `ExceptionRef=Bias_Inference` (native — the biased inference), `MappingType=skos:narrowMatch` (d5).
- **Odd-pattern of the cluster** : attacks via a *bias* inference (`Bias_Inference`) rather than an
  exception/generalization inference — but same attack-type (undercut/RA-node). The stereotyping is a categorical bias,
  adjacent to the generalization umbrella.

---

## 4. Cluster summary (proposed)

| pk | text_fr | attackType | attackedNode | DirectRef | ExceptionRef | MappingType | scheme-family |
|----|---------|-----------|--------------|-----------|--------------|-------------|---------------|
| 614 | Sophisme de l'accident (anchor) | undercut | RA-node | `ExceptionSimilarityCase_Conflict` (native) | `ExceptionalCase_Inference` (native) | — | exception |
| 615 | Sophisme de l'accident | undercut | RA-node | `ExceptionSimilarityCase_Conflict` (native) | `ExceptionalCase_Inference` (native) | narrowMatch | exception |
| 616 | Sophisme du vrai Écossais | undercut | RA-node | *(empty — fail-loud)* | `ArbitraryVerbalClassification_Inference` (native) | narrowMatch | verbal-classification |
| 617 | Sophisme de l'accident contraire | undercut | RA-node | `ExceptionSimilarityCase_Conflict` (native) | `ExceptionalCase_Inference` (native) | narrowMatch | exception (inverse) |
| 618 | Généralisation excessive | undercut | RA-node | `GeneralAcceptanceDoubt_Conflict` (native) | `Example_Inference` (native) | narrowMatch | example/induction |
| 619 | Stéréotype | undercut | RA-node | `BiasedClassification_Conflict` (native) | `Bias_Inference` (native) | narrowMatch | bias |
| 620 | Exception écrasante | undercut | RA-node | `ExceptionSimilarityCase_Conflict` (native) | `ExceptionalCase_Inference` (native) | narrowMatch | exception |

**6 attack-typed leaves proposed, 0 fabrication.** Axiom **0 violation** (6 undercut→RA). Uniform attack-type (no
split — the cluster is coherent around one generalization/exception inference mechanism). **6/6 native-fit** (highest
rate in the chantier) — the leaves attack the umbrella via **4 distinct native scheme-families** (exception 3 leaves,
example/induction 1, verbal-classification 1, bias 1). 1/6 single fail-loud (616 DirectRef — no native conflict token
for defensive reclassification).

---

## 5. Method notes (additions for the chantier)

- **Second native-rich cluster — but scheme-DIVERSE.** Cigogne (#840) was native-rich but mono-scheme (all 4 native-fit
  leaves inherited the one causal scheme). This cluster is native-rich *and* scheme-diverse : the umbrella « apply a
  rule ignoring the exception » forks into 4 Walton scheme-families (exception, example/induction, verbal-classification,
  bias). **All 6/6 native-fit** is the highest rate in the chantier — the cluster sits at the rich end of the spectrum
  that #839 (0/5) and cigogne (4/6) bracketed.
- **Uniform attack-type, diverse scheme — a new chantier pattern.** #837 and #839 were *mixed attack-type* clusters
  (undercut + undermine split). This cluster is *uniform attack-type* (6/6 undercut/RA) but *diverse scheme* — the
  variety is in the skos layer, not the attack columns. Both patterns are honest : a cluster's coherence is either in
  its attack-type (mixed-scheme, uniform-type — this one) or in its scheme (mixed-type, uniform-scheme — cigogne) or
  neither (all-fail-loud — #839). The chantier now documents all three.
- **617 accident contraire = inverse mechanism, same modelling.** The leaf erects an exception as a rule (the inverse
  of the anchor's rule-applied-ignoring-exception). Honest note : same attack-type + same native scheme — the
  exceptional-case scheme captures both manipulations of the exception (applying a rule ignoring it, vs erecting it as
  a rule). Not a divergence, a symmetry.
- **616 vrai Écossais = defensive mechanism.** Unlike the other leaves (which apply a rule), 616 *saves* a rule by
  reclassifying — a defensive move. Modelled undercut/RA-node (the inference is still attacked) via
  `ArbitraryVerbalClassification_Inference`. Distinctive : the only leaf whose DirectRef is fail-loud (no native
  conflict token for « defensive reclassification »), while its ExceptionRef is native.
- **619 Stéréotype bridges to the bias family.** Modelled via `Bias_Inference` + `BiasedClassification_Conflict` — the
  leaf's categorical bias connects this generalization cluster to the broader bias modelling used elsewhere (e.g.
  `Bias_Inference` is also attested in Influence/Obstruction rows). A cross-cluster scheme reuse, not a new token.

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented | ✅ §1/§4 (6 leaves attack-typed ; sub-sub Sophisme de l'accident fully opened, 7/7 rows addressed incl. anchor) |
| Per unmapped leaf : legitimate decomposition | ✅ §3 (attack-type + attacked-node + native scheme, 0 fabrication) |
| `AIF_skosMappingType` coherent | ✅ §4 (narrowMatch throughout — all leaves specialize the anchor's generalization/exception mechanism) |
| Fail-loud when no native token fits | ✅ 1/6 single fail-loud (616 DirectRef — defensive reclassification has no native conflict token) ; 6/6 carry a native ExceptionRef |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies → CSV edits (gated) → OWL regen (#763 wiring ready) |

### Coverage accounting (code=truth)

- **Master coverage (rescan, `c721066c`)** : **151/1 408 mapped** (`AIF_attackType` filled, 10.7 % — post #838/#839/#840
  merged). **Axiom 0 violation.**
- **This cluster (proposed)** : **+6 attack-typed** (6 undercut/RA) → **157/1 408** projected once applied.
- **Erreur mathématique family footprint** : the cluster adds 6 undercut/RA generalization/exception attacks. Combined
  with #839 (Opération inappropriée) + #840's causal twin (P725 is Err. raisonnement), the chantier now covers 3 of
  the math/reasoning sub-families' generalization-inference errors.

**Sophisme de l'accident sub-sub : fully opened** (anchor 614 serialized + 6 leaves proposed = 7/7 addressed).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### Exception-family leaves → undercut / RA-node (exceptional-case inference attack)

#### pk 615 — Sophisme de l'accident → **undercut** (neglected exception)
- **I-nodes** : the general rule (granted) + the neglected exception (medical context).
- **RA-node** : `ExceptionalCase_Inference` — the rule-application (knife→crime) ignores the exception.
- **CA-node** : none.
- **Attack type** : **undercut** (the inference rule is attacked by the neglected exception).

#### pk 617 — Sophisme de l'accident contraire → **undercut** (inverse, exception as rule)
- **I-nodes** : the exception (medical marijuana) + the over-generalization (everyone).
- **RA-node** : `ExceptionalCase_Inference` — the exception is erected as a rule.
- **CA-node** : none.
- **Attack type** : **undercut** (inverse direction, same scheme).

#### pk 620 — Exception écrasante → **undercut** (overwhelming conditions)
- **I-nodes** : the generalization + the neglected particular conditions.
- **RA-node** : `ExceptionalCase_Inference` — the generalization ignores the accumulated conditions (exceptions).
- **CA-node** : none.
- **Attack type** : **undercut** (conditions act as cumulated exceptions).

### Example/induction leaf → undercut / RA-node

#### pk 618 — Généralisation excessive → **undercut** (hasty induction)
- **I-nodes** : the particular cases (2am assaults) + the over-generalized norm.
- **RA-node** : `Example_Inference` — the induction (cases→norm) over-weights particular cases.
- **CA-node** : none.
- **Attack type** : **undercut** (the induction is excessive).

### Verbal-classification leaf → undercut / RA-node

#### pk 616 — Sophisme du vrai Écossais → **undercut** (defensive reclassification)
- **I-nodes** : the counter-example (Angus) + the redefined general idea (« true Scotsman »).
- **RA-node** : `ArbitraryVerbalClassification_Inference` — the generalization is saved by an arbitrary reclassification.
- **CA-node** : none.
- **Attack type** : **undercut** (the inference is attacked, defensively). DirectRef fail-loud.

### Bias leaf → undercut / RA-node

#### pk 619 — Stéréotype → **undercut** (categorical bias)
- **I-nodes** : the common categorizing beliefs + the over-generalized category.
- **RA-node** : `Bias_Inference` — the inference is biased by the stereotyped categorization.
- **CA-node** : none.
- **Attack type** : **undercut** (the inference is biased).

### Cluster attack-type distribution
| pk | scheme-family | I-node | RA-node (scheme) | Attack type |
|----|---------------|--------|------------------|-------------|
| 614 | exception (anchor) | rule + neglected exception | `ExceptionalCase_Inference` | undercut (serialized) |
| 615 | exception | rule + neglected exception | `ExceptionalCase_Inference` | undercut |
| 616 | verbal-classification | counter-example + redefinition | `ArbitraryVerbalClassification_Inference` | undercut |
| 617 | exception (inverse) | exception + over-generalization | `ExceptionalCase_Inference` | undercut |
| 618 | example/induction | cases + over-generalized norm | `Example_Inference` | undercut |
| 619 | bias | categorizing beliefs + category | `Bias_Inference` | undercut |
| 620 | exception | generalization + neglected conditions | `ExceptionalCase_Inference` | undercut |

**6/6 undercut / RA-node** — uniform attack-type, **4 distinct native scheme-families** (exception ×3, verbal-classification
×1, example/induction ×1, bias ×1). Serialization = `AIF_attackType` + `AIF_attackedNode` per the table ; native skos
on all 6 (6/6 ExceptionRef native ; 5/6 DirectRef native, 1/6 fail-loud).

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted. Post-T&A regime.
- ❌ No self-merge. No régén launch. Verdict QA = ai-01.
- ✅ Proposition derived code=truth from the taxonomy CSV (master `c721066c`) + AIF vocabulary verified native on
  master (`ExceptionalCase_Inference`, `ExceptionSimilarityCase_Conflict`, `Example_Inference`,
  `GeneralAcceptanceDoubt_Conflict`, `ArbitraryVerbalClassification_Inference`, `Bias_Inference`,
  `BiasedClassification_Conflict` all attested) ; 0 fabricated token (0 fabrication #677). Native generalization
  schemes mapped per leaf to the scheme-family each attacks (exception / example / verbal-classification / bias).
- ✅ Fourth cluster documented (second native-rich + first scheme-diverse uniform-type) ; distinctive leaves flagged
  (617 inverse, 616 defensive reclassification, 619 bias-bridge) ; machine-readable annotation CSV
  `498-aif-sophisme-accident-annotations.csv` provided for the gated serialization.

Relates : #498 (chantier), #837 (mixed cluster 1 — Complication exagérée), #839 (mixed cluster 2 — Opération
inappropriée), #840 (native-rich 1 — Effet cigogne), #770 (anchor audit), #760 (rebut cluster precedent), #763 (OWL
AIF wiring), #677 (0 fabrication), `[[aif-no-inherit-attacktype-from-anchor]]` (discipline — this cluster's skos
mapping is per-leaf to the scheme each attacks, not blanket inheritance). Base master `c721066c`.
