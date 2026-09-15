# 2026-07-22 — #498 AIF chantier : cluster « Effet cigogne » (Erreur de raisonnement, Causalité douteuse) — **first native-rich causal cluster**

**Scope** : dedicated cluster doc for the **Effet cigogne** sub-sub (family **Erreur de raisonnement**, sub-family
**Causalité douteuse**) — **6 unmapped leaves** (720-725) under the anchor **719** (serialized `undercut`/`RA-node`,
**the first anchor in the chantier carrying native skos on 2 layers**). **Proposition only — GATED, 0 write to prod
CSV.** Continues the cluster-docs backlog (ai-01 dispatch `msg-9akcg4` strate-6 deep-queue idle lane, post-T&A
regime). Third cluster of the dispatch (#837 Complication exagérée + #839 Opération inappropriée + this).

**Repo reference** : master `3a87eb0e`. Issue : #498. Predecessors : #770 (anchor audit), #837 (first mixed
cluster), #839 (second mixed cluster), #760 (rebut cluster). Layer C = **~1 258 unmapped leaves** remain after
#837/#839 (this doc opens 6 more).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies → worker applies
> the CSV edits in a follow-up PR (gated), same flow as #753/#760/#837/#839. Serialization deferred to gated apply.

---

## TL;DR

- **Effet cigogne** (Erreur de raisonnement > Causalité douteuse, sub-sub, 7 rows) : anchor **719** serialized
  `undercut`/`RA-node` **with native skos on 2 layers** (`OtherCausalFactorsInvolved_Conflict` direct +
  `CorrelationToCause_Inference` exception) — the **first anchor in the chantier to carry both layers** (prior
  anchors #1282/#1345/#690 were attack-columns-only). **6 unmapped leaves** — 720 Réductionnisme causal, 721
  Sophisme de la cause unique, 722 Négation de l'antécédent, 723 Personnalisation, 724 Sophisme de la tierce cause,
  725 Appel à la temporalité comme cause.
- **First native-rich causal cluster.** Where #839 (Opération inappropriée) was all-fail-loud (0/5 native schemes),
  this cluster is the **opposite pole**: 4/6 leaves cleanly inherit the anchor's causal scheme vocabulary
  (`CorrelationToCause_Inference` + `OtherCausalFactorsInvolved_Conflict`), because the cluster's mechanism
  *is* the canonical correlation→cause inference error. The two remaining leaves (722, 723) are cataloguing
  edge-cases — formal-logic / cognitive-bias entries rattachés to Causalité douteuse — modelled undercut/RA-node
  but fail-loud (no clean native causal scheme).
- **Cluster is near-uniform undercut/RA-node** (6/6) — no sub-mechanism split like #837/#839. The coherence is
  high: every leaf attacks the same causal-inference rule. The variety is in the *skos layer* (4 native-fit, 2
  fail-loud), not the attack-type.
- **725 Appel à la temporalité comme cause = the #838 causal bridge twin.** P725 (Err. rais. 4.1) is the
  word-for-word identical twin of P635 (Err. math 3.2 « Confusion entre antériorité et causalité ») — one of the
  84 `Mirrors` bridges proposed in PR #838. This cluster models P725's AIF attack-type, *complementing* the #838
  inter-family bridge (the bridge links the two duplicates; this cluster types P725's attack on the inference).
- **6 attack-typed leaves proposed, 0 fabrication, axiom 0 violation** (6 undercut→RA). Projected coverage
  **145 → 156 / 1 408** once applied (cluster run on master `3a87eb0e` = 145/1 408, 10.3 %).

---

## 1. Cluster state (code=truth, master `3a87eb0e`)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Erreur de raisonnement**, sub-family
**Causalité douteuse**, sub-sub **Effet cigogne** (7 rows) :

| pk | text_fr | depth | AIF_attackType | AIF_attackedNode | skos cols | State |
|----|---------|-------|----------------|------------------|-----------|-------|
| **719** | **Effet cigogne** (anchor) | d3 | `undercut` | `RA-node` | direct=`OtherCausalFactorsInvolved_Conflict`, exc=`CorrelationToCause_Inference` | **✅ serialized (2 skos layers)** |
| 720 | Réductionnisme causal | d4 | — | — | — | unmapped (**this cluster**) |
| 721 | Sophisme de la cause unique | d5 | — | — | — | unmapped (**this cluster**) |
| 722 | Négation de l'antécédent | d6 | — | — | — | unmapped (**this cluster**) |
| 723 | Personnalisation | d6 | — | — | — | unmapped (**this cluster**) |
| 724 | Sophisme de la tierce cause | d4 | — | — | — | unmapped (**this cluster**) |
| 725 | Appel à la temporalité comme cause | d4 | — | — | — | unmapped (**this cluster**) |

**Leaf content (code=truth `desc_fr` / `example_fr`)** :
- **719 Effet cigogne** — « Vous croyez à tort qu'une corrélation entre deux événements implique nécessairement
  un lien de causalité. » / « naissances plus nombreuses dans les régions qui comptent le plus de nids de
  cigognes »
- **720 Réductionnisme causal** — « Vous attribuez des causes à un phénomène de façon trop simpliste, en
  négligeant l'ensemble des facteurs. » / « voiture sortie de route à cause d'un écureuil (ivre + téléphone
  négligés) »
- **721 Sophisme de la cause unique** — « Vous attribuez à tort un résultat complexe à une seule et unique
  cause. » / « l'introduction du renard a entraîné la disparition des lapins »
- **722 Négation de l'antécédent** — « Vous rejetez une conclusion simplement parce qu'une cause possible ne
  s'est pas réalisée, confondant ainsi cause et condition nécessaire. » / « S'il pleut, le sol est mouillé. Il
  n'a pas plu, donc le sol n'est pas mouillé. »
- **723 Personnalisation** — « Vous vous attribuez une responsabilité personnelle… pour des événements sur
  lesquels vous avez peu ou pas de contrôle. » / « C'est de ma faute si la réunion s'est mal passée. »
- **724 Sophisme de la tierce cause** — « Vous affirmez qu'un événement est la conséquence d'un autre, alors
  que tous deux sont dus à un troisième facteur. » / « lampadaires + vandalisme (tous deux dus à un troisième
  facteur) »
- **725 Appel à la temporalité comme cause** — « Vous supposez qu'un événement est la cause d'un autre
  uniquement parce qu'il l'a précédé dans le temps. » / « le merle chante juste avant le lever du soleil ; le
  chant du merle fait donc se lever le soleil »

The sub-sub is **semantically coherent around one mechanism** — *the correlation→cause inference error* —
which is why the cluster is near-uniform undercut/RA-node. The two cataloguing edge-cases (722 formal logic,
723 cognitive bias) are rattachés to Causalité douteuse by the taxonomy's construction but their mechanism is
adjacent, not identical — flagged in §3.

---

## 2. The anchor model (719, serialized) — first 2-layer skos anchor

`719` is serialized `AIF_attackType=undercut`, `AIF_attackedNode=RA-node`, **`AIF_skosDirectRef=
OtherCausalFactorsInvolved_Conflict`**, **`AIF_skosExceptionRef=CorrelationToCause_Inference`**. This is the
**first anchor in the #498 chantier carrying native skos on both layers** — anchors #1282 (Relativisme abusif),
#1345 (Complication exagérée), #690 (Opération inappropriée) were all attack-columns-only (no native scheme fit
their generic-procedural mechanisms). The Effet cigogne anchor fits the canonical `CorrelationToCause_Inference`
scheme exactly — the fallacy attacks that inference rule by neglecting other causal factors
(`OtherCausalFactorsInvolved_Conflict`).

**Native causal token inventory (code=truth)** : `OtherCausalFactorsInvolved_Conflict`, `CorrelationToCause_Inference`,
`CauseToEffect_Inference`, `CausalSlipperySlope_Inference` — all natively attested. The cluster's leaves that
share the anchor's correlation→cause mechanism cleanly inherit `CorrelationToCause_Inference` (the attacked
scheme) + `OtherCausalFactorsInvolved_Conflict` (the neglected-factors conflict). The two edge-case leaves (722,
723) do not — they are formal-logic / cognitive-bias entries with no clean causal-scheme fit → fail-loud.

---

## 3. Proposed AIF structure for the leaves

Per-leaf derivation. `attackType` + `attackedNode` follow the AIF/ASPIC+ axiom (**0 violation**). `skosDirectRef` /
`skosExceptionRef` carry native causal tokens where the leaf shares the anchor's correlation→cause mechanism
(4/6) ; otherwise empty with fail-loud (2/6). **0 fabrication (#677).**

### Native-fit leaves → **undercut / RA-node** + `CorrelationToCause_Inference` (matches anchor)

#### pk 720 — Réductionnisme causal (oversimplistic causation) ✅ undercut
- **Mechanism** : attributes a cause too simplistically, neglecting the full set of contributing factors. Attacks
  the causal-inference rule (RA-node) by ignoring other factors.
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef=OtherCausalFactorsInvolved_Conflict`
  (native — neglected factors), `ExceptionRef=CorrelationToCause_Inference` (native — the attacked inference),
  `MappingType=skos:narrowMatch` (d4).
- **Why not undermine** : the alleged cause is not false — it is incomplete. Why the native fit : the leaf *is*
  the canonical neglected-factors reading of the anchor's mechanism.

#### pk 721 — Sophisme de la cause unique (single-cause) ✅ undercut
- **Mechanism** : attributes a complex result to a single cause. Same as 720 — attacks the causal inference by
  postulating one factor for a multifactorial effect.
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef=OtherCausalFactorsInvolved_Conflict`
  (native), `ExceptionRef=CorrelationToCause_Inference` (native), `MappingType=skos:narrowMatch` (d5).
- **Why not undermine** : the single cause is insufficient, not false. Sibling of 720 — same native-fit.

#### pk 724 — Sophisme de la tierce cause (third-factor confound) ✅ undercut
- **Mechanism** : asserts A causes B when both are due to a third factor C (the confound). Attacks the direct
  causal inference by revealing the confound.
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef=OtherCausalFactorsInvolved_Conflict`
  (native — the third factor), `ExceptionRef=CorrelationToCause_Inference` (native), `MappingType=skos:narrowMatch`
  (d4).
- **Why not undermine** : the A↔B correlation is not denied — it is explained by C. **Cleanest ratification test
  case** in the cluster (confound = canonical CQ of correlation→cause).

#### pk 725 — Appel à la temporalité comme cause (temporal precedence) ✅ undercut — **#838 bridge twin**
- **Mechanism** : mistakes temporal precedence for causation (the merle/soleil rooster-sun case). Attacks the
  correlation→cause inference.
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef` **empty** (no clean native
  *conflict* token for « temporal precedence alone » — the temporal-coincidence is the issue, not a neglected
  factor per se), `ExceptionRef=CorrelationToCause_Inference` (native — the attacked inference),
  `MappingType=skos:narrowMatch` (d4).
- **Why not undermine** : the temporal succession is not denied — it is real but not causal. **Cross-reference
  to #838**: P725 is the word-for-word identical twin of P635 (Err. math 3.2 « Confusion entre antériorité et
  causalité »). PR #838 proposes the `Mirrors` inter-family bridge P725↔P635 ; this cluster types P725's AIF
  attack. The two PRs are complementary (one links the duplicates, one types the attack).

### Cataloguing edge-cases → **undercut / RA-node**, fail-loud (no clean native causal scheme)

#### pk 722 — Négation de l'antécédent (denying the antecedent) ⚠ undercut (boundary: formal logic)
- **Mechanism** : rejects a conclusion because a possible cause (necessary condition) did not occur — a malformed
  modus tollens (« si P alors Q ; non P ; donc non Q »). This is a **formal propositional-logic error**, rattaché
  to Causalité douteuse via the cause/condition confusion.
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef` **empty**, `ExceptionRef` **empty**,
  fail-loud note, `MappingType=skos:narrowMatch` (d6).
- **Why not undermine** : no premise contested — the *form* of the reasoning is invalid. Why fail-loud : no clean
  native causal scheme captures a propositional-logic malformation (it is a deduction error, not a Walton
  argument scheme). **Cataloguing edge-case flagged for jsboige** — may belong more naturally in a
  « Mauvaise déduction » cluster than in Causalité douteuse.

#### pk 723 — Personnalisation (egocentric causal attribution) ⚠ undercut (boundary: cognitive bias, odd-one-out)
- **Mechanism** : self-attributes responsibility for events with little/no control. This is a **cognitive bias**
  (egocentric causal attribution), rattaché to Causalité douteuse via the erroneous causal inference « I caused
  X ».
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef` **empty**, `ExceptionRef` **empty**,
  fail-loud note, `MappingType=skos:narrowMatch` (d6).
- **Why not undermine** : no factual premise contested — the *attribution inference* is flawed. Why fail-loud :
  no clean native causal scheme captures an egocentric attribution bias. **Odd-one-out / cataloguing edge-case
  flagged** — may belong more naturally in a cognitive-bias cluster (cf. Insuffisance family) than in Causalité
  douteuse. Not reclassified here (out of scope — proposition only).

---

## 4. Cluster summary (proposed)

| pk | text_fr | attackType | attackedNode | DirectRef | ExceptionRef | MappingType | Honest? |
|----|---------|-----------|--------------|-----------|--------------|-------------|---------|
| 719 | Effet cigogne (anchor) | undercut | RA-node | `OtherCausalFactorsInvolved_Conflict` (native) | `CorrelationToCause_Inference` (native) | — | ✅ serialized (2 skos) |
| 720 | Réductionnisme causal | undercut | RA-node | `OtherCausalFactorsInvolved_Conflict` (native) | `CorrelationToCause_Inference` (native) | narrowMatch | ✅ native-fit |
| 721 | Sophisme de la cause unique | undercut | RA-node | `OtherCausalFactorsInvolved_Conflict` (native) | `CorrelationToCause_Inference` (native) | narrowMatch | ✅ native-fit |
| 722 | Négation de l'antécédent | undercut | RA-node | *(empty — fail-loud)* | *(empty — fail-loud)* | narrowMatch | ⚠ boundary formal-logic |
| 723 | Personnalisation | undercut | RA-node | *(empty — fail-loud)* | *(empty — fail-loud)* | narrowMatch | ⚠ odd-one-out (cognitive bias) |
| 724 | Sophisme de la tierce cause | undercut | RA-node | `OtherCausalFactorsInvolved_Conflict` (native) | `CorrelationToCause_Inference` (native) | narrowMatch | ✅ native-fit (ratification case) |
| 725 | Appel à la temporalité comme cause | undercut | RA-node | *(empty — fail-loud)* | `CorrelationToCause_Inference` (native) | narrowMatch | ✅ native-fit (1 layer) — #838 twin |

**6 attack-typed leaves proposed, 0 fabrication.** Axiom **0 violation** (6 undercut→RA). Near-uniform attack-type
(no sub-mechanism split — the cluster is coherent around one causal-inference mechanism). Variety in the skos
layer : **4/6 native-fit** (720/721/724 full 2-layer + 725 1-layer), **2/6 fail-loud** (722/723 cataloguing
edge-cases). First cluster where the anchor itself seeds the skos vocabulary the leaves inherit.

---

## 5. Method notes (additions for the chantier)

- **Native-rich vs all-fail-loud clusters — two poles of the chantier.** #839 (Opération inappropriée) was the
  all-fail-loud pole (0/5 native schemes — math-operational defects are not Walton schemes). This cluster is the
  native-rich pole (4/6 native schemes) because the anchor seeds the causal vocabulary (`CorrelationToCause_Inference`
  + `OtherCausalFactorsInvolved_Conflict`) and the leaves share its mechanism. The chantier's modelling quality
  spans both poles — neither is better; the honesty is in deriving per-leaf (native where it fits, fail-loud
  where it does not).
- **Anchor-with-skos seeds leaf inheritance.** The prior anchors (#1282/#1345/#690) were attack-columns-only,
  so their leaves could not inherit a scheme — each leaf had to find its own native fit or fail-loud. Anchor 719
  carries skos on 2 layers, so leaves sharing its mechanism inherit cleanly (720/721/724 full inheritance). This
  is the cleanest case of legitimate in-cluster skos inheritance — distinct from the
  `[[aif-no-inherit-attacktype-from-anchor]]` warning (which concerns *attack-type* inheritance, not *skos*
  inheritance : skos inheritance is legitimate when the leaf genuinely shares the anchor's scheme).
- **725 + #838 cross-reference — bridge + attack-type complementarity.** P725 is modelled here (its AIF
  attack-type) AND linked in #838 (its `Mirrors` inter-family bridge to P635). The two PRs are complementary :
  #838 densifies the inter-family *relation* graph (the typed graph #7289 Phase-B consumes) ; this cluster types
  P725's *attack* on the inference. Together they give a Phase-B consumer both the inter-family structure and
  the AIF typing on the causal bridge's twin.
- **724 Sophisme de la tierce cause = ratification test case.** The confound (third factor) is the canonical CQ
  of correlation→cause — if ai-01/jsboige want a single leaf to ratify the cluster's native-fit undercut/RA +
  scheme inheritance, 724 is the cleanest (the conflict token fits exactly).
- **Cataloguing edge-cases (722, 723) flagged, not reclassified.** Both are rattachés to Causalité douteuse but
  their mechanism is adjacent (formal logic / cognitive bias). Modelled honestly (undercut/RA, fail-loud) and
  flagged for jsboige — potential taxonomy restructuring, out of scope here (proposition only).

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not « 100 % leaves ») | ✅ §1/§4 (6 leaves attack-typed ; sub-sub Effet cigogne fully opened, 7/7 rows addressed incl. anchor) |
| Per unmapped leaf : legitimate decomposition | ✅ §3 (attack-type + attacked-node + native scheme where honest, 0 fabrication) |
| `AIF_skosMappingType` coherent | ✅ §4 (narrowMatch throughout — all leaves specialize the anchor's causal-inference mechanism) |
| Fail-loud when no native token fits | ✅ 2/6 double fail-loud (722/723 — formal-logic / cognitive-bias edge-cases) ; 1/6 single fail-loud (725 DirectRef) ; 3/6 full native-fit |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies → CSV edits (gated) → OWL regen (#763 wiring ready) |

### Coverage accounting (code=truth)

- **Master coverage (rescan, `3a87eb0e`)** : **145/1 408 mapped** (`AIF_attackType` filled, 10.3 %). Dist :
  {undermine:53, undercut:87, rebut:5}. **Axiom 0 violation.**
- **This cluster (proposed)** : **+6 attack-typed** (6 undercut/RA) → **151/1 408** projected once applied (this
  cluster alone) ; **156/1 408** if #839's 5 are also applied (they are independent PRs).
- **Erreur de raisonnement family footprint** : the cluster adds 6 undercut/RA causal-inference attacks to the
  family. Combined with #839's Erreur mathématique cluster, the chantier now covers the causal-inference +
  math-operation error families.

**Effet cigogne sub-sub : fully opened** (anchor 719 serialized + 6 leaves proposed = 7/7 addressed).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### Native-fit leaves → undercut / RA-node (causal-inference attack)

#### pk 720 — Réductionnisme causal → **undercut** (neglected factors)
- **I-nodes** : the alleged cause (granted — not false, incomplete) + the neglected factors (the conflict).
- **RA-node** : `CorrelationToCause_Inference` — the cause→effect rule-application is oversimplified.
- **CA-node** : none.
- **Attack type** : **undercut** (the inference rule is attacked by neglecting factors).

#### pk 721 — Sophisme de la cause unique → **undercut** (single-factor)
- **I-nodes** : the single alleged cause (insufficient) + the other factors (the conflict).
- **RA-node** : `CorrelationToCause_Inference` — the multifactorial effect attributed to one factor.
- **CA-node** : none.
- **Attack type** : **undercut** (sibling of 720).

#### pk 724 — Sophisme de la tierce cause → **undercut** (confound)
- **I-nodes** : the A↔B correlation (granted — real) + the third factor C (the confound).
- **RA-node** : `CorrelationToCause_Inference` — the direct A→B inference is invalidated by C.
- **CA-node** : none.
- **Attack type** : **undercut** (the confound breaks the direct causal inference).

#### pk 725 — Appel à la temporalité comme cause → **undercut** (temporal precedence) — #838 twin
- **I-nodes** : the temporal succession (granted — real) + the absence of causal mechanism.
- **RA-node** : `CorrelationToCause_Inference` — the temporal-coincidence→cause inference.
- **CA-node** : none.
- **Attack type** : **undercut**. **#838 cross-ref**: `Mirrors` bridge to P635 (Err. math 3.2) proposed there.

### Cataloguing edge-cases → undercut / RA-node, fail-loud

#### pk 722 — Négation de l'antécédent → **undercut** (formal logic)
- **I-nodes** : the antecedent condition (P) + the conclusion (Q) — the *form* (si P alors Q ; non P ; donc non Q)
  is invalid.
- **RA-node** : *(no clean native causal scheme — a propositional-logic malformation)*.
- **CA-node** : none.
- **Attack type** : **undercut** (the inference form is invalid), boundary *formal-logic* documented.

#### pk 723 — Personnalisation → **undercut** (cognitive bias, odd-one-out)
- **I-nodes** : your presence + the event — the *attribution inference* « I caused X » is flawed.
- **RA-node** : *(no clean native causal scheme — an egocentric attribution bias)*.
- **CA-node** : none.
- **Attack type** : **undercut** (the attribution inference is attacked), odd-one-out documented.

### Cluster attack-type distribution
| pk | sub-mechanism | I-node | RA-node (scheme) | Attack type |
|----|---------------|--------|------------------|-------------|
| 719 | causal-inference (anchor) | correlation (granted) | `CorrelationToCause_Inference` | undercut (serialized) |
| 720 | causal-inference | cause (incomplete) | `CorrelationToCause_Inference` | undercut |
| 721 | causal-inference | single cause (insufficient) | `CorrelationToCause_Inference` | undercut |
| 722 | formal-logic (edge) | antecedent condition | *(none — fail-loud)* | undercut |
| 723 | cognitive-bias (odd-one-out) | your presence / the event | *(none — fail-loud)* | undercut |
| 724 | causal-inference | A↔B correlation (confound C) | `CorrelationToCause_Inference` | undercut |
| 725 | causal-inference (temporal) | temporal succession | `CorrelationToCause_Inference` | undercut (#838 twin) |

**6/6 undercut / RA-node** — near-uniform (the cluster is coherent around one causal-inference mechanism, with
2 cataloguing edge-cases fail-loud on the scheme layer). Serialization = `AIF_attackType` + `AIF_attackedNode`
per the table ; native causal skos on 720/721/724 (2-layer) + 725 (1-layer) ; fail-loud on 722/723.

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted. Post-T&A regime.
- ❌ No self-merge. No régén launch. Verdict QA = ai-01.
- ✅ Proposition derived code=truth from the taxonomy CSV (master `3a87eb0e`) + AIF vocabulary verified native on
  master (`OtherCausalFactorsInvolved_Conflict`, `CorrelationToCause_Inference` attested in the anchor 719) ;
  0 fabricated token (0 fabrication #677). Native causal schemes inherited only where the leaf genuinely shares
  the anchor's mechanism (legitimate skos inheritance — distinct from the attack-type-inheritance warning
  `[[aif-no-inherit-attacktype-from-anchor]]`).
- ✅ First native-rich cluster documented (4/6 native-fit vs #839's 0/5) ; #838 cross-reference for P725 ;
  cataloguing edge-cases (722/723) flagged for jsboige ; machine-readable annotation CSV
  `498-aif-effet-cigogne-annotations.csv` provided for the gated serialization.

Relates : #498 (chantier), #837 (first mixed cluster — Complication exagérée), #839 (second mixed cluster —
Opération inappropriée), #838 (inter-family bridges — P725↔P635 `Mirrors` twin, this cluster types P725's
attack), #770 (anchor audit), #760 (rebut cluster precedent), #763 (OWL AIF wiring), #677 (0 fabrication),
`[[aif-no-inherit-attacktype-from-anchor]]` (anchor-inheritance discipline — this cluster's skos inheritance is
legitimate because leaves share the anchor's scheme, not its attack-type). Base master `3a87eb0e`.
