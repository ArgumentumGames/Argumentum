# 2026-07-22 — #498 AIF chantier : cluster « Procès en incohérence » (Obstruction > Ad hominem) — **second rebut-dominant + heterogeneous sub-sub (taxonomy finding)**

**Scope** : dedicated cluster doc for the **Procès en incohérence** sub-sub (family **Obstruction**, sub-family
**Ad hominem**, sub-sub **Procès en inconsistance**) — anchor **1361** (serialized `rebut`/`CA-node`,
attack-columns-only) + **3 unmapped consistency-attack leaves** (1362-1364). **Plus a taxonomy-quality finding** :
the 6 *remaining* leaves of the sub-sub (1365-1370) are **out-of-mechanism** (straw man / circularity / pathos…) —
misgrouped fallacies documented for reclassification, **not** serialized here. **Proposition only — GATED, 0 write to
prod CSV.** Continues the cluster-docs backlog (ai-01 dispatch `msg-9akcg4` + ACK `msg-6uufhx` naming this candidate
"Procès en inconsistance", post-T&A regime). Seventh cluster of the dispatch.

**Repo reference** : master `eebfea73`. Issue : #498. Predecessors : #770 (anchor audit), #837/#839 (mixed),
#840 (cigogne), #841 (accident), #843 (inconsistance — first undermine-dominant), #844 (attaque personnelle —
second undermine-dominant), #760 (first rebut-dominant — Relativisme abusif).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies → worker applies
> the CSV edits in a follow-up PR (gated), same flow as #837/#839/#840/#841/#843/#844. Serialization deferred to gated apply.

---

## TL;DR

- **Procès en inconsistance** (Obstruction > Ad hominem, sub-sub) : anchor **1361** serialized `rebut`/`CA-node`,
  attack-columns-only (skos-empty). **3 unmapped consistency-attack leaves proposed** — 1362 Tu quoque, 1363
  Whataboutisme, 1364 Critique de l'incohérence passée — all `rebut`/`CA-node` with native `InconsistentCommitment_Conflict`
  (the conflict token, *not* an inference scheme).
- **Second rebut-dominant cluster of the chantier** (after #760 Relativisme). The mechanism is the **consistency-attack**
  rebuttal : the opponent's position (CA-node) is opposed by the counter-conclusion *"your position is void because you
  are inconsistent"*. This is the rebut-pole's consistency-attack variant (vs #760's relativism variant).
- **3/3 native-fit (conflict token)** — `InconsistentCommitment_Conflict` is natively attested and captures the
  inconsistency-attack. The anchor is skos-empty (no inference-scheme token), so the leaves are **fail-loud on the
  inference-scheme layer** (only the conflict token is native) — the same attack-columns-only posture as #839, but
  here the conflict layer *is* native.
- **Taxonomy-quality finding (the real value of this cluster).** The sub-sub "Procès en inconsistance" is
  **heterogeneous** : only 3 of its 10 rows (anchor + 1362-1364) are consistency-attacks. The other 6 (1365-1370) are
  **misgrouped fallacies with entirely different AIF mechanisms** :
  - **1365 Homme de paille** = *straw man* (misrepresentation) — an undercut/misrepresentation, not a consistency-attack.
  - **1366 Extension** / **1367 Tirer de fausses conclusions** = *misrepresentation* (caricature of the opponent's thesis) — not a consistency-attack.
  - **1368 Invocation fallacieuse des sophismes** = *false accusation of fallacy* — not a consistency-attack.
  - **1369 Accusation de petitio principii** = *begging the question* (circularity) — a distinct structural fallacy.
  - **1370 Argument du pathos** = *emotional appeal* — not a consistency-attack.
  These 6 should be reclassified under their proper mechanisms. **Flagged for jsboige** — not serialized here (forcing
  them into rebut/CA would violate the axiom and fabricate a consistency-attack where none exists). This is the first
  cluster where the chantier **qualifies the taxonomy** rather than merely serializing it.
- **3 attack-typed leaves proposed, 0 fabrication, axiom 0 violation** (3 rebut→CA). Projected coverage **145 → 148 /
  1 408** once applied (live rescan master `eebfea73` = 145/1 408 — propositions are docs-only, not yet written to
  prod CSV).

---

## 1. Cluster state (code=truth, master `eebfea73`)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Obstruction**, sub-family **Ad hominem**,
sub-sub **Procès en inconsistance** (10 rows) :

| pk | text_fr | depth | AIF_attackType | AIF_attackedNode | skos cols | State |
|----|---------|-------|----------------|------------------|-----------|-------|
| **1361** | **Procès en incohérence** (anchor) | d3 | `rebut` | `CA-node` | *(empty)* | **✅ serialized (attack-columns-only)** |
| 1362 | Tu quoque | d4 | — | — | — | unmapped — **consistency-attack (this cluster)** |
| 1363 | Whataboutisme | d5 | — | — | — | unmapped — **consistency-attack (this cluster)** |
| 1364 | Critique de l'incohérence passée | d5 | — | — | — | unmapped — **consistency-attack (this cluster)** |
| 1365 | Homme de paille | d4 | — | — | — | **out-of-mechanism** (straw man — finding) |
| 1366 | Extension | d5 | — | — | — | **out-of-mechanism** (misrepresentation — finding) |
| 1367 | Tirer de fausses conclusions | d5 | — | — | — | **out-of-mechanism** (misrepresentation — finding) |
| 1368 | Invocation fallacieuse des sophismes | d4 | — | — | — | **out-of-mechanism** (false accusation — finding) |
| 1369 | Accusation de petitio principii | d5 | — | — | — | **out-of-mechanism** (circularity — finding) |
| 1370 | Argument du pathos | d5 | — | — | — | **out-of-mechanism** (emotional appeal — finding) |

**Leaf content (code=truth `desc_fr`)** :
- **1361 Procès en incohérence** — « Vous relevez les incohérences ou les contradictions de votre interlocuteur, ou
  vous lui opposez des arguments… »
- **1362 Tu quoque** — « Vous rejetez ce que dit quelqu'un en affirmant qu'il n'agit pas toujours conformément à ses
  propres principes. »
- **1363 Whataboutisme** — « Vous détournez les critiques en rappelant à votre adversaire qu'il n'a pas condamné une
  situation similaire. »
- **1364 Critique de l'incohérence passée** — « Vous accusez votre contradicteur d'avoir changé d'opinion au fil du
  temps. »

The sub-sub is **heterogeneous** : the umbrella "Procès en inconsistance" (consistency-attack) genuinely covers only
1361-1364. The 6 remaining rows (1365-1370) describe fallacies of *misrepresentation* (straw man), *structure*
(circularity), and *appeal* (pathos) that have no consistency-attack mechanism — they are taxonomically misplaced
under this sub-sub. **This is the finding.**

---

## 2. The anchor model (1361, serialized) — consistency-attack rebuttal

`1361` is serialized `AIF_attackType=rebut`, `AIF_attackedNode=CA-node`, `AIF_skosDirectRef` **empty**
(attack-columns-only), `AIF_skosMappingType=skos:broadMatch`. The fallacy opposes a **counter-conclusion** (CA-node)
to the opponent's position : *"you are inconsistent, therefore your position is void"*. This is the rebut-pole's
**consistency-attack** variant — distinct from #760's **relativism** rebut ("truth is relative, therefore your
position is void") and from the undermine-pole's source-discrediting (#843, #844 — which attack the *source* as an
I-node, not the *position* as a CA-node).

> **Honest note on the rebut-vs-undermine tension.** Tu quoque / whataboutisme are *classically* modelled in AIF as
> **undermine/I-node** (circumstantial ad hominem — discrediting the *source* by their inconsistency). The taxonomy
> here models the anchor 1361 as **rebut/CA-node** (the counter-conclusion reading : the *position* is voided by
> the opponent's inconsistency). **This cluster follows the anchor's rebut/CA modelling** (we do not re-model the
> anchor — that is out of scope for a Layer-C leaf cluster). The rebut reading is defensible : the consistency-attack
> is *formulated as a counter-conclusion* ("your position is caduque"), which targets the conclusion (CA-node). The
> tension is flagged for jsboige — a future anchor re-modelling pass could read these as undermine/I
> (`CircumstantialAdHominem_Inference`), but that is a separate, gated decision.

**Native inconsistency token inventory (code=truth)** : `InconsistentCommitment_Conflict` (×2), `Commitment_Conflict`
(×2), `OpposedCommitment_Conflict` (×2) — the inconsistency-family *conflict* tokens are native. There is **no
native inference-scheme token** for "rebuttal-from-inconsistency" (the Walton schemes model the circumstantial ad
hominem as an *inference*, i.e. undermine-flavoured ; the rebut-as-counter-conclusion reading has no dedicated
scheme). Hence the anchor is skos-empty on the scheme layer, and the leaves inherit only the conflict token — a
**native-conflict / fail-loud-scheme** posture (3/3 native-fit on the conflict layer, fail-loud on the scheme layer).

---

## 3. Proposed AIF structure for the consistency-attack leaves

Per-leaf derivation. `attackType` + `attackedNode` follow the anchor's rebut/CA modelling (axiom **0 violation**,
all rebut→CA). `skosDirectRef` carries the native `InconsistentCommitment_Conflict` (conflict token). **0 fabrication
(#677)** — the conflict token is native ; the scheme layer is honestly fail-loud (no native scheme fits the
rebut-as-counter-conclusion reading).

### All 3 leaves → **rebut / CA-node** + `InconsistentCommitment_Conflict` (native conflict token)

The 3 leaves are the consistency-attack rebuttal in 3 vectors : hypocrisy (tu quoque), selective treatment
(whataboutisme), temporal drift (incohérence passée). They all oppose a counter-conclusion to the opponent's position
via the opponent's inconsistency — the canonical rebut-pole consistency-attack.

| pk | text_fr | consistency vector | Proposal |
|----|---------|--------------------|----------|
| 1362 | Tu quoque | hypocrisy (acts against own principles) | `rebut`/CA, `InconsistentCommitment_Conflict` (native conflict) |
| 1363 | Whataboutisme | selective treatment (failed to condemn similar) | `rebut`/CA, `InconsistentCommitment_Conflict` (native conflict) |
| 1364 | Critique de l'incohérence passée | temporal drift (changed opinion over time) | `rebut`/CA, `InconsistentCommitment_Conflict` (native conflict) |

### Cluster summary (proposed)

| pk | text_fr | attackType | attackedNode | DirectRef | ExceptionRef | MappingType | consistency vector |
|----|---------|-----------|--------------|-----------|--------------|-------------|--------------------|
| 1361 | Procès en incohérence (anchor) | rebut | CA-node | *(empty — attack-columns-only)* | *(empty)* | broadMatch | (umbrella) |
| 1362 | Tu quoque | rebut | CA-node | `InconsistentCommitment_Conflict` (native conflict) | *(empty)* | narrowMatch | hypocrisy |
| 1363 | Whataboutisme | rebut | CA-node | `InconsistentCommitment_Conflict` (native conflict) | *(empty)* | narrowMatch | selective treatment |
| 1364 | Critique de l'incohérence passée | rebut | CA-node | `InconsistentCommitment_Conflict` (native conflict) | *(empty)* | narrowMatch | temporal drift |

**3 attack-typed leaves proposed, 0 fabrication.** Axiom **0 violation** (3 rebut→CA). **Native-fit on the conflict
layer (3/3), fail-loud on the scheme layer** (no native inference-scheme for the rebut-as-counter-conclusion reading).
**Out-of-mechanism leaves (1365-1370) NOT serialized** — see §4.

---

## 4. Taxonomy-quality finding — the 6 out-of-mechanism leaves

This is the **distinctive value** of this cluster : the chantier does not merely serialize, it **qualifies**. The 6
remaining rows of the sub-sub are **misgrouped fallacies** whose AIF mechanism is *not* a consistency-attack. Forcing
them into rebut/CA would violate the axiom and fabricate a consistency-attack. They are documented here for
**reclassification** (gated, jsboige), not serialized.

| pk | text_fr | its ACTUAL mechanism | why it is NOT a consistency-attack |
|----|---------|----------------------|-------------------------------------|
| **1365** | Homme de paille | **straw man** — misrepresentation of the opponent's thesis (caricature to refute more easily) | Attacks a *fabricated* thesis, not the opponent's consistency. Distinct mechanism (undercut/misrepresentation). *Note : 1365 is itself a near-duplicate hub — it appears cross-family in the #838 84-pair set.* |
| **1366** | Extension | **misrepresentation** — pushing the opponent's argument to absurdity | Caricatures the thesis, not the source's consistency. |
| **1367** | Tirer de fausses conclusions | **misrepresentation** — distorting the spirit of the thesis to draw a false conclusion | Misrepresents the thesis, not a consistency-attack. |
| **1368** | Invocation fallacieuse des sophismes | **false accusation of fallacy** — attributing fallacious procedures the opponent did not use | A meta-accusation, not a consistency-attack on the opponent's commitments. |
| **1369** | Accusation de petitio principii | **begging the question** (circularity) — refusing the principle founding the opponent's argument | A *structural* fallacy (the argument's premises), not a consistency-attack. |
| **1370** | Argument du pathos | **emotional appeal** — presenting the opponent as irrational / emotion-driven | Attacks via *emotion*, not via the opponent's *consistency*. |

**Recommendation (gated jsboige).** These 6 leaves should be reclassified under their proper mechanisms (straw-man
family, circularity family, appeal family). They are taxonomically misplaced under "Procès en inconsistance".
Serialization deferred until reclassification — **0 forced serialization here** (axiom integrity over coverage).

> **Why this matters for the chantier.** Prior clusters (#837-#844) serialized *homogeneous* sub-subs. This is the
> first **heterogeneous** sub-sub. The honest move is *not* to force-serialize all 9 leaves for coverage — it is to
> serialize the 3 that genuinely belong and flag the 6 that don't. Coverage integrity > coverage count. This is the
> chantier's quality discipline made concrete.

---

## 5. Method notes (additions for the chantier)

- **Second rebut-dominant cluster — the consistency-attack pole.** #760 (Relativisme) was the first rebut-dominant,
  with the *relativism* variant ("truth is relative → your position is void"). This cluster is the second
  rebut-dominant, with the *consistency-attack* variant ("you are inconsistent → your position is void"). The
  rebut-pole now has 2 documented variants (relativism + consistency-attack), mirroring the undermine-pole's 2
  variants (#843 contradiction-shape + #844 attack-vector). The chantier's attack-type catalog is now symmetric.
- **Native-conflict / fail-loud-scheme posture — a new modelling shape.** #839 (Opération) was *fully* fail-loud
  (no native token at all). #840/#841 (cigogne/accident) were *fully* native-fit (scheme + conflict). This cluster is
  **hybrid** : native-fit on the *conflict* layer (`InconsistentCommitment_Conflict`), fail-loud on the *scheme*
  layer (no native inference-scheme for the rebut-as-counter-conclusion reading). This is because the Walton schemes
  model the circumstantial ad hominem as an *inference* (undermine-flavoured) ; the taxonomy's rebut/CA reading has
  no dedicated scheme. The conflict layer captures the inconsistency-attack ; the scheme layer is honestly absent.
- **Heterogeneous sub-sub — first "qualify, don't just serialize" cluster.** The chantier's discipline `[[aif-no-inherit-attacktype-from-anchor]]`
  plus the axiom-enforcement (rebut→CA) mean a sub-sub cannot be blanket-serialized when its leaves diverge in
  mechanism. This cluster introduces the **out-of-mechanism flag** : leaves that are taxonomically misplaced are
  documented with their *actual* mechanism and flagged for reclassification, rather than force-serialized. This
  protects coverage integrity and surfaces taxonomy-quality issues — a net gain for the corpus.
- **1365 Homme de paille bridges to #838.** The straw man (1365) is itself a near-duplicate hub in the #838 84-pair
  inter-family set (it appears under multiple families). Its reclassification will interact with the #838 bridges —
  flagged for coordination when jsboige ratifies #838.
- **Rebut-vs-undermine tension honestly flagged.** Tu quoque / whataboutisme are classically undermine/I
  (circumstantial ad hominem). The taxonomy's anchor 1361 is rebut/CA. This cluster follows the anchor (rebut/CA)
  rather than re-modelling it — the re-modelling is a separate gated decision. Flagging the tension is the honest move.

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented | ✅ §1/§3 (3 consistency-attack leaves attack-typed) + §4 (6 out-of-mechanism leaves documented with actual mechanisms) — sub-sub fully *addressed* (10/10 rows dispositioned) |
| Per unmapped leaf : legitimate decomposition | ✅ §3 (3 consistency-attack leaves : rebut/CA + native conflict token, 0 fabrication) + §4 (6 leaves : out-of-mechanism, not serialized, flagged for reclassification) |
| `AIF_skosMappingType` coherent | ✅ §3 (narrowMatch for the 3 serialized leaves ; broadMatch on the anchor) |
| Fail-loud when no native token fits | ✅ §3 (fail-loud on the *scheme* layer — no native inference-scheme for rebut-as-counter-conclusion ; native-fit on the *conflict* layer) + §4 (fail-loud = out-of-mechanism flag for the 6 misgrouped leaves) |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies → CSV edits (gated) → OWL regen (#763 wiring ready) |

### Coverage accounting (code=truth)

- **Master coverage (rescan, `eebfea73`)** : **145/1 408 mapped** (`attackType` non-empty). **Axiom 0 violation.**
  Rebut population rises to 6 (5 + ... well, the 3 here once applied) — the second rebut batch after #760. *(Note :
  propositions #841/#843/#844 are docs-only, not yet in prod CSV — the live count is 145, the strict "fully-modeled"
  figure. The +3 is *projected* on ratification + apply.)*
- **This cluster (proposed)** : **+3 attack-typed** (3 rebut/CA) → **148/1 408** projected once applied. The 6
  out-of-mechanism leaves are **not** counted (they are flagged for reclassification, not serialized).
- **Obstruction family footprint** : this is the **second Ad-hominem sub-sub** opened (after #844 Attaque personnelle
  = direct/abusive variant ; this = consistency-attack rebuttal variant). The third sibling (Sophisme génétique =
  circumstantial/bias variant) remains a queue candidate.

**Procès en inconsistance sub-sub : fully addressed** (3 serialized + 6 out-of-mechanism documented = 9/9 leaves
dispositioned, + anchor).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### The 3 serialized leaves → rebut / CA-node (consistency-attack counter-conclusion)

The attacked node is the **opponent's position/conclusion** (CA-node) — opposed by a counter-conclusion. The attack
is *not* on the source (undermine) nor on the inference rule (undercut) ; it is a counter-conclusion *"your position
is void because you are inconsistent"*. No leaf invokes an I-node (the source is not the defeated node) or an RA-node
(the rule is not challenged).

#### pk 1362 — Tu quoque → **rebut** (hypocrisy vector)
- **I-nodes** : the opponent's own principles (invoked as evidence of inconsistency). *(not the defeated node)*
- **RA-node** : *(none — the rule is not challenged)*.
- **CA-node** : **the opponent's position** (the counter-conclusion "your argument is void because you're a hypocrite").
- **Attack type** : **rebut** (the position is opposed by the counter-conclusion of hypocrisy).

#### pk 1363 — Whataboutisme → **rebut** (selective-treatment vector)
- **I-nodes** : the opponent's selective condemnation record. *(not the defeated node)*
- **RA-node** : *(none)*.
- **CA-node** : **the opponent's position** (the counter-conclusion "your criticism is void because you didn't condemn the similar case").
- **Attack type** : **rebut**.

#### pk 1364 — Critique de l'incohérence passée → **rebut** (temporal-drift vector)
- **I-nodes** : the opponent's past opinions (evidence of temporal inconsistency). *(not the defeated node)*
- **RA-node** : *(none)*.
- **CA-node** : **the opponent's present position** (the counter-conclusion "your position is void because you changed your mind").
- **Attack type** : **rebut**.

### Cluster attack-type distribution
| pk | consistency vector | CA-node (opposed position) | I-node | RA-node | Attack type |
|----|---------------------|----------------------------|--------|---------|-------------|
| 1361 | (umbrella) | opponent position | opponent commitments | *(none)* | rebut (serialized) |
| 1362 | hypocrisy | opponent position | opponent principles | *(none)* | rebut |
| 1363 | selective treatment | opponent position | opponent condemnation record | *(none)* | rebut |
| 1364 | temporal drift | opponent position | opponent past opinions | *(none)* | rebut |

**3/3 rebut / CA-node** — uniform attack-type, native conflict token (`InconsistentCommitment_Conflict`), fail-loud
scheme layer, variety in the consistency vector (hypocrisy / selective treatment / temporal drift). Serialization =
`AIF_attackType` + `AIF_attackedNode` per the table ; native conflict token on all 3.

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted. Post-T&A regime.
- ❌ No self-merge. No régén launch. Verdict QA = ai-01.
- ✅ Proposition derived code=truth from the taxonomy CSV (master `eebfea73`) + AIF vocabulary verified native on
  master (`InconsistentCommitment_Conflict` attested) ; 0 fabricated token (0 fabrication #677). The 6
  out-of-mechanism leaves are **not** serialized (axiom integrity over coverage).
- ✅ Seventh cluster documented (second rebut-dominant — consistency-attack pole ; **first heterogeneous sub-sub** —
  qualify-don't-force-serialize) ; taxonomy-quality finding on 6 misgrouped leaves (flagged for jsboige
  reclassification) ; rebut-vs-undermine tension honestly flagged ; machine-readable annotation CSV
  `498-aif-proces-inconsistance-annotations.csv` provided for the gated serialization.

Relates : #498 (chantier), #837 (mixed 1 — Complication exagérée), #839 (mixed 2 — Opération inappropriée), #840
(native-rich cigogne, undercut), #841 (native-rich accident, undercut), #843 (first undermine-dominant — Inconsistance),
#844 (second undermine-dominant — Attaque personnelle), #760 (first rebut-dominant — Relativisme abusif), #838
(inter-family bridges — 1365 straw man is a hub in the 84-pair set), #770 (anchor audit), #763 (OWL AIF wiring),
#677 (0 fabrication), `[[aif-no-inherit-attacktype-from-anchor]]` (discipline — anchor rebut/CA followed, not
re-modelled). Base master `eebfea73`.
