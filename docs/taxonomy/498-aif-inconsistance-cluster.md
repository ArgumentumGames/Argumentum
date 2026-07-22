# 2026-07-22 — #498 AIF chantier : cluster « Inconsistance » (Erreur de raisonnement, Mauvaise déduction) — **first undermine-dominant cluster**

**Scope** : dedicated cluster doc for the **Inconsistance** sub-sub (family **Erreur de raisonnement**, sub-family
**Mauvaise déduction**) — **6 unmapped leaves** (778-783) under the anchor **777** (serialized `undermine`/`I-node`
**with native skos**). **Proposition only — GATED, 0 write to prod CSV.** Continues the cluster-docs backlog
(ai-01 dispatch `msg-9akcg4` strate-6 deep-queue idle lane, post-T&A regime). Fifth cluster of the dispatch
(#837 + #839 + #840 + #841 + this).

**Repo reference** : master `c721066c`. Issue : #498. Predecessors : #770 (anchor audit), #837/#839 (mixed clusters),
#840 (cigogne), #841 (accident). Layer C = **~1 246 unmapped leaves** remain after the prior clusters (this opens 6 more).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies → worker applies
> the CSV edits in a follow-up PR (gated), same flow as #753/#760/#837/#839/#840/#841. Serialization deferred to gated apply.

---

## TL;DR

- **Inconsistance** (Erreur de raisonnement > Mauvaise déduction, sub-sub, 7 rows) : anchor **777** serialized
  `undermine`/`I-node` **with native skos** (`OpposedCommitment_Conflict` + `InconsistentCommitment_Inference` in
  DirectRef). **6 unmapped leaves** — 778 Prémisses incompatibles, 779 Concept volé, 780 Contradiction interne,
  781 Logique du chaudron, 782 Triade inconsistante, 783 Contradiction entre prémisse et conclusion.
- **First undermine-dominant cluster of the chantier.** The 4 prior clusters (#837/#839 mixed-type, #840/#841
  undercut-dominant) attacked the *inference rule* (RA-node). This cluster attacks a *premise* (I-node) — its
  mechanism is **inter-premise contradiction**: the reasoning rests on claims that contradict one another, so the
  attacked premise is invalidated by another premise in the set. **Uniform undermine/I-node (6/6)** — the canonical
  AIF shape of a self-contradictory argument.
- **Native-rich + uniform (mono-scheme).** All 6 leaves inherit the anchor's `InconsistentCommitment_Inference`
  scheme (the inference whose premises are inconsistent) + a native conflict token (`OpposedCommitment_Conflict` or
  `Logical_Conflict`). **6/6 native-fit.** The cluster is both native-rich *and* uniform — the cleanest mono-mechanism
  cluster in the chantier (one mechanism, one scheme, all leaves inherit).
- **One boundary leaf (783)** : the contradiction is between a premise and the *conclusion* (not premise↔premise).
  Modelled undermine/I-node honestly (the attacked target is still the premise the conclusion renders incompatible),
  flagged for ratification.
- **6 attack-typed leaves proposed, 0 fabrication, axiom 0 violation** (6 undermine→I). Projected coverage **157 → 163 /
  1 408** once applied (live rescan master `c721066c` = 157/1 408, post-#841 not yet merged).

---

## 1. Cluster state (code=truth, master `c721066c`)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Erreur de raisonnement**, sub-family
**Mauvaise déduction**, sub-sub **Inconsistance** (7 rows) :

| pk | text_fr | depth | AIF_attackType | AIF_attackedNode | skos cols | State |
|----|---------|-------|----------------|------------------|-----------|-------|
| **777** | **Inconsistance** (anchor) | d3 | `undermine` | `I-node` | direct=`OpposedCommitment_Conflict`, `InconsistentCommitment_Inference` | **✅ serialized (skos)** |
| 778 | Prémisses incompatibles | d4 | — | — | — | unmapped (**this cluster**) |
| 779 | Concept volé | d5 | — | — | — | unmapped (**this cluster**) |
| 780 | Contradiction interne | d4 | — | — | — | unmapped (**this cluster**) |
| 781 | Logique du chaudron | d5 | — | — | — | unmapped (**this cluster**) |
| 782 | Triade inconsistante | d5 | — | — | — | unmapped (**this cluster**) |
| 783 | Contradiction entre prémisse et conclusion | d4 | — | — | — | unmapped (**this cluster**) |

**Leaf content (code=truth `desc_fr` / `example_fr`)** :
- **777 Inconsistance** — « Votre raisonnement repose sur des affirmations qui se contredisent entre elles. » /
  « Ce n'est pas moi qui ai abîmé ce cahier ; j'en ai pris le plus grand soin, mais en plus je n'y ai pas touché »
- **778 Prémisses incompatibles** — « Vous construisez un raisonnement sur des bases qui ne peuvent pas coexister. » /
  « Je vous ai demandé de ne faire confiance à personne. Vous me faites confiance ? »
- **779 Concept volé** — « Vous niez un concept tout en continuant à l'utiliser dans votre raisonnement. » /
  « La raison n'est pas toujours fiable ; il ne faut donc pas compter sur elle pour établir la vérité »
- **780 Contradiction interne** — « Votre argumentation comporte des éléments qui se contredisent entre eux. » /
  « défendre la liberté d'expression, mais certaines opinions ne devraient jamais être exprimées »
- **781 Logique du chaudron** — « Vous multipliez les justifications qui semblent valables seules, mais qui, prises
  ensemble, se contredisent. » / « 1) je n'ai pas emprunté le chaudron ; 2) il avait déjà un trou… » (Hegel)
- **782 Triade inconsistante** — « Vous vous basez sur trois prémisses parmi lesquelles seules deux peuvent être
  vraies en même temps. » / « Alice est honnête + dit toujours la vérité + a affirmé… »
- **783 Contradiction entre prémisse et conclusion** — « Vous démontrez une conclusion incompatible avec au moins une
  de vos hypothèses de départ. » / « tout a une cause + on ne peut régresser indéfiniment → Dieu existe »

The sub-sub is **semantically coherent around one mechanism** — *inter-premise contradiction* — which is why the
cluster is uniform undermine/I-node (the canonical AIF shape: a premise attacked by its incompatibility with another
claim in the argument). The variety is in the *shape* of the contradiction (pair / self-reference / cumulative / n-ary /
premise↔conclusion), not the attack-type.

---

## 2. The anchor model (777, serialized) — first undermine-dominant anchor

`777` is serialized `AIF_attackType=undermine`, `AIF_attackedNode=I-node`, **`AIF_skosDirectRef=
OpposedCommitment_Conflict, InconsistentCommitment_Inference`**, ExceptionRef empty. The fallacy attacks a **premise**
(I-node) by the contradiction between the claims on which the reasoning rests. This is the **first anchor in the
#498 chantier with `undermine`/`I-node`** — prior anchors were `rebut`/CA (#1282 Relativisme), `undercut`/RA
(#1345 Complication, #690 Opération, #719 cigogne, #614 accident). The undermine/I-node shape is the canonical AIF
representation of a self-contradictory argument (a premise is defeated by its incompatibility with another premise).

**Native inconsistency-family token inventory (code=truth)** : `OpposedCommitment_Conflict` (×2), `InconsistentCommitment_Inference`
(×2), `Commitment_Conflict` (×2), `Commitment_Inference` (×1), `Logical_Conflict` (×1), `ConflictingGoals_Conflict` (×3),
`ExpertiseInconsistency_Conflict` (×1) — **all natively attested**. The cluster's leaves inherit `InconsistentCommitment_Inference`
(the scheme of an argument whose premises/commitments are inconsistent) + a conflict token matching the contradiction shape.

---

## 3. Proposed AIF structure for the leaves

Per-leaf derivation. `attackType` + `attackedNode` follow the AIF/ASPIC+ axiom (**0 violation**, all undermine→I).
`skosDirectRef` / `skosExceptionRef` carry native inconsistency-family tokens (6/6 native-fit). **0 fabrication (#677).**

### All leaves → **undermine / I-node** + `InconsistentCommitment_Inference` (matches anchor)

#### pk 778 — Prémisses incompatibles (mutually exclusive bases) ✅ undermine
- **Mechanism** : builds the reasoning on bases that cannot coexist (« trust no one » + « you trust me »). The attacked
  premise (I-node) is invalidated by its incompatibility with the other base.
- **Proposal** : `attackType=undermine`, `attackedNode=I-node`, `DirectRef=OpposedCommitment_Conflict, InconsistentCommitment_Inference`
  (native), `ExceptionRef` empty, `MappingType=skos:narrowMatch` (d4).

#### pk 779 — Concept volé (stolen concept — performative self-contradiction) ✅ undermine
- **Mechanism** : denies a concept while continuing to use it (« reason is unreliable » argued using reason). The
  attacked premise (I-node) is the denied-yet-used concept, self-contradicted by auto-reference.
- **Proposal** : `attackType=undermine`, `attackedNode=I-node`, `DirectRef=InconsistentCommitment_Inference` (native),
  `ExceptionRef` empty, `MappingType=skos:narrowMatch` (d5). **Distinctive** : auto-reference (performative
  self-contradiction) — the concept contradicts itself by its own use.

#### pk 780 — Contradiction interne (mutually contradicting elements) ✅ undermine
- **Mechanism** : the argumentation contains elements that contradict one another (« defend free speech » + « some
  opinions should never be expressed »). The attacked premise (I-node) is contradicted by another element.
- **Proposal** : `attackType=undermine`, `attackedNode=I-node`, `DirectRef=Logical_Conflict, InconsistentCommitment_Inference`
  (native — `Logical_Conflict` fits the bare logical contradiction), `ExceptionRef` empty, `MappingType=skos:narrowMatch`
  (d4). **Ratification test case** : contradiction interne is the paradigmatic inconsistency case.

#### pk 781 — Logique du chaudron (jointly contradicting justifications) ✅ undermine
- **Mechanism** : multiplies justifications valid alone but contradicting together (Hegel's kettle : « I didn't borrow
  it » + « it had a hole » + …). The attacked premise (I-node) is the common premise the joint set contradicts.
- **Proposal** : `attackType=undermine`, `attackedNode=I-node`, `DirectRef=InconsistentCommitment_Inference` (native),
  `ExceptionRef` empty, `MappingType=skos:narrowMatch` (d5). **Famous case** (Hegel's kettle) = the paradigm of
  *cumulative* inconsistency (each branch valid alone, the set incoherent).

#### pk 782 — Triade inconsistante (mutually exclusive triad) ✅ undermine
- **Mechanism** : rests on three premises of which only two can be true (Alice honest + always truthful + claimed…).
  At least one premise (I-node) is invalidated by the n-ary mutual exclusion.
- **Proposal** : `attackType=undermine`, `attackedNode=I-node`, `DirectRef=OpposedCommitment_Conflict, InconsistentCommitment_Inference`
  (native), `ExceptionRef` empty, `MappingType=skos:narrowMatch` (d5). **Distinctive** : n-ary contradiction (> pair).

#### pk 783 — Contradiction entre prémisse et conclusion (premise↔conclusion) ⚠ undermine (boundary)
- **Mechanism** : demonstrates a conclusion incompatible with a starting hypothesis (« everything has a cause » + « no
  infinite regress » → God exists, but « everything has a cause » excludes an uncaused God). The attacked target is the
  starting hypothesis (I-node) the conclusion renders incompatible.
- **Proposal** : `attackType=undermine`, `attackedNode=I-node`, `DirectRef=InconsistentCommitment_Inference` (native),
  `ExceptionRef` empty, `MappingType=skos:narrowMatch` (d4). **Boundary** : the contradiction is premise↔*conclusion*
  (not premise↔premise), but the attacked target is still the premise (I-node) → undermine. Flagged for ratification —
  the only leaf whose contradiction involves the conclusion rather than another premise.

---

## 4. Cluster summary (proposed)

| pk | text_fr | attackType | attackedNode | DirectRef | ExceptionRef | MappingType | contradiction shape |
|----|---------|-----------|--------------|-----------|--------------|-------------|---------------------|
| 777 | Inconsistance (anchor) | undermine | I-node | `OpposedCommitment_Conflict`, `InconsistentCommitment_Inference` (native) | *(empty)* | — | pair |
| 778 | Prémisses incompatibles | undermine | I-node | `OpposedCommitment_Conflict`, `InconsistentCommitment_Inference` (native) | *(empty)* | narrowMatch | pair (exclusive bases) |
| 779 | Concept volé | undermine | I-node | `InconsistentCommitment_Inference` (native) | *(empty)* | narrowMatch | self-reference (performative) |
| 780 | Contradiction interne | undermine | I-node | `Logical_Conflict`, `InconsistentCommitment_Inference` (native) | *(empty)* | narrowMatch | pair (internal) |
| 781 | Logique du chaudron | undermine | I-node | `InconsistentCommitment_Inference` (native) | *(empty)* | narrowMatch | cumulative (jointly) |
| 782 | Triade inconsistante | undermine | I-node | `OpposedCommitment_Conflict`, `InconsistentCommitment_Inference` (native) | *(empty)* | narrowMatch | n-ary (triad) |
| 783 | Contradiction prémisse/conclusion | undermine | I-node | `InconsistentCommitment_Inference` (native) | *(empty)* | narrowMatch | premise↔conclusion (boundary) |

**6 attack-typed leaves proposed, 0 fabrication.** Axiom **0 violation** (6 undermine→I). **Uniform undermine/I-node**
(the cluster is the canonical AIF shape of self-contradiction). **6/6 native-fit** — all inherit the anchor's
`InconsistentCommitment_Inference` scheme. Variety is in the *contradiction shape* (pair / self-reference / cumulative /
n-ary / premise↔conclusion), not the attack-type or scheme.

---

## 5. Method notes (additions for the chantier)

- **First undermine-dominant cluster — the third chantier pattern-pole.** #837/#839 were *mixed-type* (undercut +
  undermine split) ; #840 (cigogne) and #841 (accident) were *undercut-dominant* (attack the inference rule, RA-node).
  This cluster is *undermine-dominant* (attack a premise, I-node) — its mechanism is **inter-premise contradiction**,
  not inference-rule invalidity. The chantier now documents all three attack-type poles (mixed / undercut-dominant /
  undermine-dominant) plus the rebut pole (#760 Relativisme) — the full AIF attack-type spectrum.
- **Native-rich + uniform (mono-mechanism, mono-scheme).** Unlike #841 (native-rich but scheme-diverse, 4 schemes),
  this cluster is native-rich *and* uniform — one mechanism (inter-premise contradiction), one scheme
  (`InconsistentCommitment_Inference`), all 6 leaves inherit. The cleanest mono-mechanism cluster in the chantier.
  Variety is in the contradiction *shape*, which the scheme accommodates without forking.
- **781 Logique du chaudron = paradigm of cumulative inconsistency.** Hegel's kettle (« I didn't borrow it » + « it
  had a hole » + « it was already cracked » + …) is the textbook case where each justification is valid alone but the
  joint set is incoherent. The `InconsistentCommitment_Inference` scheme captures this — the inference's premises are
  jointly inconsistent even if pairwise compatible.
- **783 boundary — premise↔conclusion contradiction.** The only leaf whose contradiction involves the *conclusion*
  rather than another premise. Modelled undermine/I-node honestly (the attacked target is the starting hypothesis,
  an I-node) and flagged for ratification — ai-01/jsboige may prefer to read it as a rebut (the conclusion conflicts
  with a premise) or keep it undermine (the premise is the defeated node). The honest reading here is undermine
  (the *premise* is what gets invalidated), but the boundary is real.
- **Legitimate skos inheritance, again.** As in cigogne (#840) and accident (#841), the native scheme is inherited
  because the leaves genuinely share the anchor's mechanism — distinct from the attack-type-inheritance warning
  `[[aif-no-inherit-attacktype-from-anchor]]` (which concerns *attack-type* inheritance ; skos inheritance is
  legitimate when the leaf shares the anchor's *scheme*).

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented | ✅ §1/§4 (6 leaves attack-typed ; sub-sub Inconsistance fully opened, 7/7 rows addressed incl. anchor) |
| Per unmapped leaf : legitimate decomposition | ✅ §3 (attack-type + attacked-node + native scheme, 0 fabrication) |
| `AIF_skosMappingType` coherent | ✅ §4 (narrowMatch throughout — all leaves specialize the anchor's inter-premise-contradiction mechanism) |
| Fail-loud when no native token fits | ✅ 6/6 native-fit (no fail-loud needed — the inconsistency scheme vocabulary is rich and all leaves inherit) ; boundary on 783 documented |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies → CSV edits (gated) → OWL regen (#763 wiring ready) |

### Coverage accounting (code=truth)

- **Master coverage (rescan, `c721066c`)** : **157/1 408 mapped** (post #838/#839/#840 merged, #841 not yet merged).
  **Axiom 0 violation.** Undermine population rises to 59 (53 + 6) once applied — the first cluster to add a
  significant batch of undermine/I-node modelling.
- **This cluster (proposed)** : **+6 attack-typed** (6 undermine/I) → **163/1 408** projected once applied.
- **Erreur de raisonnement family footprint** : the cluster adds 6 undermine/I inconsistency attacks. Combined with
  #840 (cigogne, undercut) and #841 (accident, undercut), the chantier now covers 3 Err. de raisonnement / Err. math
  sub-families' inference errors (causal + generalization + inconsistency).

**Inconsistance sub-sub : fully opened** (anchor 777 serialized + 6 leaves proposed = 7/7 addressed).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### All leaves → undermine / I-node (inter-premise contradiction)

#### pk 778 — Prémisses incompatibles → **undermine** (exclusive bases)
- **I-nodes** : the incompatible bases (« trust no one » + « you trust me »). The attacked premise is invalidated by
  its incompatibility with the other.
- **RA-node** : *(none — the attack is on the premise, not the rule)*.
- **CA-node** : none.
- **Attack type** : **undermine** (a *premise* is attacked by incompatibility).

#### pk 779 — Concept volé → **undermine** (performative self-reference)
- **I-nodes** : the denied-yet-used concept (reason/logic). The premise self-contradicts by auto-reference.
- **RA-node** : *(none)*.
- **CA-node** : none.
- **Attack type** : **undermine** (the premise is self-defeated).

#### pk 780 — Contradiction interne → **undermine** (internal contradiction)
- **I-nodes** : the mutually contradicting elements. The attacked premise is contradicted by another element.
- **RA-node** : *(none)*.
- **CA-node** : none.
- **Attack type** : **undermine** (the premise is contradicted internally).

#### pk 781 — Logique du chaudron → **undermine** (cumulative contradiction)
- **I-nodes** : the jointly contradicting justifications. The common premise is invalidated by the joint set.
- **RA-node** : *(none)*.
- **CA-node** : none.
- **Attack type** : **undermine** (the premise is defeated by the cumulative set).

#### pk 782 — Triade inconsistante → **undermine** (n-ary contradiction)
- **I-nodes** : the mutually exclusive triad. At least one premise is invalidated by the n-ary exclusion.
- **RA-node** : *(none)*.
- **CA-node** : none.
- **Attack type** : **undermine** (the premise is excluded by the triad).

#### pk 783 — Contradiction prémisse/conclusion → **undermine** (boundary)
- **I-nodes** : the starting hypothesis + the demonstrated conclusion. The hypothesis is rendered incompatible by
  the conclusion.
- **RA-node** : *(none — the contradiction is between premise and conclusion, but the defeated node is the premise)*.
- **CA-node** : *(arguably the conclusion, but the canonical AIF reading is that the premise is the defeated I-node)*.
- **Attack type** : **undermine** (the premise is invalidated by the conclusion). Boundary documented.

### Cluster attack-type distribution
| pk | contradiction shape | I-node (attacked premise) | RA-node | Attack type |
|----|---------------------|---------------------------|---------|-------------|
| 777 | pair (anchor) | mutually contradicting claims | *(none)* | undermine (serialized) |
| 778 | pair (exclusive bases) | incompatible base | *(none)* | undermine |
| 779 | self-reference | denied-yet-used concept | *(none)* | undermine |
| 780 | pair (internal) | internally contradicted element | *(none)* | undermine |
| 781 | cumulative | common premise (joint set) | *(none)* | undermine |
| 782 | n-ary (triad) | excluded triad member | *(none)* | undermine |
| 783 | premise↔conclusion (boundary) | conclusion-incompatible hypothesis | *(none)* | undermine |

**6/6 undermine / I-node** — uniform attack-type, **one native scheme** (`InconsistentCommitment_Inference`), variety in
the contradiction shape. Serialization = `AIF_attackType` + `AIF_attackedNode` per the table ; native skos on all 6.

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted. Post-T&A regime.
- ❌ No self-merge. No régén launch. Verdict QA = ai-01.
- ✅ Proposition derived code=truth from the taxonomy CSV (master `c721066c`) + AIF vocabulary verified native on
  master (`InconsistentCommitment_Inference`, `OpposedCommitment_Conflict`, `Logical_Conflict` all attested) ;
  0 fabricated token (0 fabrication #677). Native inconsistency scheme inherited legitimately (leaves share the
  anchor's mechanism).
- ✅ Fifth cluster documented (first undermine-dominant — completes the chantier's attack-type-pole coverage) ;
  boundary leaf 783 flagged (premise↔conclusion contradiction) ; machine-readable annotation CSV
  `498-aif-inconsistance-annotations.csv` provided for the gated serialization.

Relates : #498 (chantier), #837 (mixed 1 — Complication exagérée), #839 (mixed 2 — Opération inappropriée), #840
(native-rich cigogne, undercut), #841 (native-rich accident, undercut), #760 (rebut cluster — Relativisme abusif),
#770 (anchor audit), #763 (OWL AIF wiring), #677 (0 fabrication), `[[aif-no-inherit-attacktype-from-anchor]]`
(discipline — this cluster's skos inheritance is legitimate, leaves share the anchor's scheme). Base master `c721066c`.
