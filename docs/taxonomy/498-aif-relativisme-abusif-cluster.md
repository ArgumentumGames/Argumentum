# 2026-07-10 — #498 AIF chantier : cluster « Relativisme abusif » (Obstruction) — **first rebut cluster doc**

**Scope**: first dedicated cluster doc for a **rebut** family in the Fallacies taxonomy. Opens the
**Relativisme abusif** sub-sub (family **Obstruction**) — 4 unmapped leaves (1283-1286) under the
anchor **1282** (already serialized `rebut`/`CA-node` by #760). **Proposition only — GATED, 0 write
to prod CSV.** Continues the cluster-docs authoring backlog (ai-01 dispatches `kxxbqd`/`nnzspt`/`hgicfj`,
primary = ~80 unmapped sub-subs).

**Repo reference**: master `7796c127`. Issue: #498. Predecessors (undercut/undermine clusters):
PR-1 #699 … PR-5 #706 (Arbitrary definition), PR-12 #720 (Equivoque complete). Serialization:
#753 (§7 cluster docs), #755 (Virtues mirror), #757 (phase 1-3 apply/verify), **#760 (phase 1-3
serialize — wrote the anchor 1282 + the 3 first Fallacies rebuts)**. Ontology wiring: #763 (OWL now
emits `AIF_attack*`).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies →
> worker applies the CSV edits in a follow-up PR (gated), same flow as #753/#755/#760. The
> attack-type decomposition is recorded here + in the machine-readable annotation CSV
> `498-relativisme-abusif-annotations.csv`; serialization to the 2 columns (`AIF_attackType`,
> `AIF_attackedNode`) is deferred to the gated apply.

---

## TL;DR

- **Relativisme abusif** (Obstruction, sub-sub, 5 leaves total): anchor **1282** MERGED
  (`rebut`/`CA-node`, #760), **4 unmapped leaves** — 1283 Sophisme subjectiviste, 1284 Truthiness,
  1285 Droit à mon opinion, 1286 Plus faux que faux.
- **First rebut cluster.** All 4 leaves honestly model the **same structural move as the anchor**:
  opposing a **subjective/leveling counter-conclusion** that denies the shared conclusion's scope or
  authority → **rebut / CA-node** (deterministic map ratified #707§4 option a: rebut → CA-node).
- **Attack-columns-only pattern (load-bearing #677 finding).** The 3 merged rebut rows (1282/1313/
  1361) serialize as **attack columns only** — `AIF_skosDirectRef` **empty** (no native Walton
  CQ-conflict token for a bare rebut), `ExceptionRef` = the rebutted *native* scheme *iff one honestly
  applies*. The coined `CounterConclusionConflict_Conflict` from the phase-3 annotation notes is **NOT
  native** (verified: absent from all filled skos columns) and is **correctly not serialized**. This
  cluster follows that reality: **0 fabricated token**.
- **Honest per-leaf serialization**: `AIF_attackType=rebut` + `AIF_attackedNode=CA-node` for all 4;
  `AIF_skosExceptionRef=Commitment_Inference` (native) for **1283 only** (the self-exemption honestly
  rebuts the arguer's own commitment); the other 3 have **no clean native rebutted scheme** →
  ExceptionRef empty, fail-loud documented in `AIF_skosOther`.
- **4 attack-typed rebut leaves proposed, 0 fabrication, 3 documented boundary nuances** (1284
  undermine-flavor, 1285 undercut-flavor, 1286 false-equivalence).

---

## 1. Cluster state (code=truth, master `7796c127`)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Obstruction**, sub-sub
**Relativisme abusif** (5 rows):

| pk | text_fr | depth | AIF_attackType | AIF_attackedNode | skos cols | State |
|----|---------|-------|----------------|------------------|-----------|-------|
| **1282** | **Relativisme abusif** (anchor) | d3 | `rebut` | `CA-node` | *(all empty)* | **✅ MERGED #760** |
| 1283 | Sophisme subjectiviste | d4 | — | — | — | unmapped (**this cluster**) |
| 1284 | Truthiness | d5 | — | — | — | unmapped (**this cluster**) |
| 1285 | Droit à mon opinion | d5 | — | — | — | unmapped (**this cluster**) |
| 1286 | Plus faux que faux | d4 | — | — | — | unmapped (**this cluster**) |

**Leaf content (code=truth `desc_fr` / `example_fr`)**:
- **1282 Relativisme abusif** — « Vous affirmez que la vérité est subjective et propre à chaque
  individu. » / « C'est peut-être vrai pour vous, mais cela ne l'est pas pour moi. »
- **1283 Sophisme subjectiviste** — « Vous affirmez qu'une idée est vraie pour les autres, mais pas
  pour vous, afin d'éviter de vous l'appliquer. » / « Oui, fumer est mauvais pour la santé, mais pas
  pour moi. »
- **1284 Truthiness** — « Vous affirmez qu'une chose est vraie parce que vous la ressentez ainsi, sans
  preuve concrète. » / « Je n'ai pas besoin de statistiques pour savoir que le changement climatique
  est un mythe : je le vois bien. »
- **1285 Droit à mon opinion** — « Vous prétendez que votre avis vaut autant qu'un fait établi, sans
  avoir à le justifier. » / « J'ai le droit de croire que ces médicaments sont plus dangereux
  qu'utiles. Après tout, chacun a droit à sa propre opinion. »
- **1286 Plus faux que faux** — « Vous assimilez des erreurs de gravité différente en les présentant
  comme équivalentes. » / « Il a peut-être grillé le feu rouge à cent à l'heure, mais vous traversiez
  en dehors des clous. Vous avez tous les deux tort. »

The sub-sub is **semantically coherent around one mechanism**: abusive relativism *opposes a subjective
or leveling counter-conclusion* to a shared/graded conclusion, refusing its scope or authority. Every
leaf is a specialization of that move (self-exemption, felt truth, epistemic parity, false leveling).

---

## 2. The anchor model (1282, MERGED #760) — attack-columns-only

`1282` was serialized by #760 as `AIF_attackType=rebut`, `AIF_attackedNode=CA-node`, **all skos
relational columns empty**. This is deliberate and #677-honest: a bare relativist rebut is a
**structural counter-conclusion conflict**, not a Walton-CQ conflict. There is **no native
CQ-conflict token** for it — the phase-3 annotation note recorded a *placeholder*
`CounterConclusionConflict_Conflict`, but that token is **not native** and was **correctly dropped**
at serialization.

**Verification (code=truth, `498-native-whitelist` scan of prod CSV)**:

| token | native? (in a filled skos column) |
|-------|-----------------------------------|
| `CounterConclusionConflict_Conflict` | ❌ NOT native (absent) |
| `IssueAvoidance_Conflict` (1313 note) | ❌ NOT native (absent) |
| `TuQuoque_Conflict` (1361 note) | ❌ NOT native (absent) |
| `Commitment_Inference` | ✅ native |

The 3 merged rebut rows confirm the serialized shape:

| pk | AIF_attackType | AIF_attackedNode | AIF_skosDirectRef | AIF_skosExceptionRef |
|----|----------------|------------------|-------------------|----------------------|
| 1282 Relativisme abusif | rebut | CA-node | *(empty)* | *(empty)* |
| 1313 Évasion | rebut | CA-node | *(empty)* | `Dialogue_Scheme` (native) |
| 1361 Procès en incohérence | rebut | CA-node | *(empty)* | `InconsistentCommitment_Inference, CircumstantialAdHominem_Inference` (native) |

**Pattern (rebut cluster)**: `attackType=rebut` + `attackedNode=CA-node` always; `DirectRef` empty
(the fallacy is a bare counter-conclusion conflict, no CQ token); `ExceptionRef` = the **rebutted
native scheme** *iff* one honestly applies, else empty.

---

## 3. Proposed AIF structure for the leaves

For each leaf: `AIF_attackType=rebut`, `AIF_attackedNode=CA-node` (structural — the leaf opposes a
counter-conclusion). `AIF_skosExceptionRef` gets a **native** rebutted scheme only where honest;
otherwise empty with a fail-loud note in `AIF_skosOther`. **0 fabrication (#677).**

### pk 1283 — Sophisme subjectiviste (self-exemption) ✅ rebut
- **Mechanism**: the arguer *accepts* the shared truth (smoking harms) yet opposes a counter-conclusion
  that it does not apply *to them* — a self-exemption that blocks the truth's application.
- **Proposal**: `attackType=rebut`, `attackedNode=CA-node`, `ExceptionRef=Commitment_Inference`
  (native — the arguer's own commitment to the general truth is the rebutted argument), `DirectRef`
  empty (structural conflict, no native CQ token), `MappingType=skos:narrowMatch` (d4 specialization).
- **Why rebut**: not undermine (the admitted premise is not contested — the arguer *grants* smoking
  harms), not undercut (no inference rule declared inapplicable); the payload is a counter-conclusion
  of self-exemption. The rebutted scheme (`Commitment_Inference`) is native → serializable.

### pk 1284 — Truthiness (felt truth) ⚠ rebut (boundary: undermine)
- **Mechanism**: asserts a counter-conclusion guaranteed by feeling alone (« c'est un mythe, je le
  vois »), substituting felt certainty for evidence.
- **Proposal**: `attackType=rebut`, `attackedNode=CA-node`, `ExceptionRef` **empty** (no clean native
  rebutted scheme — the felt assertion does not cleanly rebut one named Walton scheme), `DirectRef`
  empty, fail-loud note in `AIF_skosOther`, `MappingType=skos:narrowMatch` (d5).
- **Documented boundary**: close to an *undermine* (the ressenti substitutes for the probatory
  premise), but the charge is an asserted counter-conclusion, so **rebut**. Recorded honestly for
  ai-01/jsboige adjustment.

### pk 1285 — Droit à mon opinion (epistemic parity) ⚠ rebut (boundary: undercut)
- **Mechanism**: claims an unjustified opinion equals an established fact, refusing the burden of
  justification.
- **Proposal**: `attackType=rebut`, `attackedNode=CA-node`, `ExceptionRef` **empty** (no clean native
  rebutted scheme), `DirectRef` empty, fail-loud note, `MappingType=skos:narrowMatch` (d5).
- **Documented boundary**: an *undercut* flavor (refusing the warrant that claims require
  justification), but the payload is a counter-conclusion of epistemic parity (« mon avis vaut un
  fait »), so **rebut**.

### pk 1286 — Plus faux que faux (false leveling) ⚠ rebut (boundary: false-equivalence)
- **Mechanism**: equates errors of different gravity as equivalent (« vous avez tous les deux tort »),
  relativizing the graded distinction.
- **Proposal**: `attackType=rebut`, `attackedNode=CA-node`, `ExceptionRef` **empty** (no clean native
  rebutted scheme), `DirectRef` empty, fail-loud note, `MappingType=skos:closeMatch` (d4 sibling,
  distinct leveling mechanism).
- **Documented boundary**: false equivalence is often treated as a *comparison* defect, but here the
  charge opposes a leveling counter-conclusion to the graded conclusion, and the leveling relativizes
  gravity (the sub-sub theme), so **rebut**.

---

## 4. Cluster summary (proposed)

| pk | text_fr | attackType | attackedNode | ExceptionRef (rebutted scheme) | DirectRef | MappingType | Honest? |
|----|---------|-----------|--------------|-------------------------------|-----------|-------------|---------|
| 1282 | Relativisme abusif | rebut | CA-node | *(empty)* | *(empty)* | — | ✅ MERGED #760 |
| 1283 | Sophisme subjectiviste | rebut | CA-node | `Commitment_Inference` (native) | *(empty)* | narrowMatch | ✅ |
| 1284 | Truthiness | rebut | CA-node | *(empty — fail-loud)* | *(empty)* | narrowMatch | ⚠ boundary undermine |
| 1285 | Droit à mon opinion | rebut | CA-node | *(empty — fail-loud)* | *(empty)* | narrowMatch | ⚠ boundary undercut |
| 1286 | Plus faux que faux | rebut | CA-node | *(empty — fail-loud)* | *(empty)* | closeMatch | ⚠ boundary false-equiv |

**4 attack-typed rebut leaves proposed, 0 fabrication.** 1/4 (1283) carries a native rebutted scheme;
3/4 are attack-columns-only with a documented fail-loud on the rebutted-scheme layer. **4/4 fail-loud
on the CA-conflict token** (no native token for a bare relativist rebut — consistent with the merged
anchor 1282).

---

## 5. Method notes (additions for the chantier)

- **First rebut cluster → attack-columns-only pattern.** Undercut/undermine clusters (PR-1…PR-12)
  serialize a scheme (`*_Inference`) + a conflict (`*_Conflict`) in the skos columns. **Rebut clusters
  do not**: the load-bearing serialization is the 2 attack columns (`attackType=rebut`,
  `attackedNode=CA-node`), the `DirectRef` stays empty (no CQ-conflict token), and `ExceptionRef`
  holds the rebutted native scheme only where one honestly applies. This is not a gap — it is the
  faithful AIF shape of a rebut (a symmetric conflict between conclusions, structural not CQ-based).
- **Non-native placeholder tokens must NOT be serialized (#677).** The phase-3 annotation notes coined
  `CounterConclusionConflict_Conflict` / `IssueAvoidance_Conflict` / `TuQuoque_Conflict` as *decomposition
  labels*. Verified against the prod CSV: **none is native**. #760 correctly serialized the 3 rebuts
  as attack-columns-only, dropping these. This cluster keeps that discipline — the annotation CSV uses
  the honest descriptor `(structural rebut — no native CQ-conflict token, fail-loud #677)` in the
  `AIF_CA_node` column rather than propagating a fabricated token. **Improvement over the phase-3
  placeholder practice.**
- **In-sub-sub coherence + honest boundary flags.** All 4 leaves inherit the anchor's rebut structure
  (the sub-sub is one coherent mechanism). Where a leaf has a genuine secondary reading (1284
  undermine, 1285 undercut, 1286 false-equivalence), it is **flagged in prose + `why_not_others`**, not
  silently forced. This gives ai-01/jsboige the material to ratify or reclassify per leaf.
- **Rebutted-scheme identifiability varies.** Only 1283 (self-exemption of one's own commitment) cleanly
  rebuts a *named* native scheme (`Commitment_Inference`). Truthiness/parity/leveling rebut the
  *general* evidenced or graded conclusion without a single named Walton scheme → ExceptionRef empty,
  fail-loud. Same honesty as 1313 (`Dialogue_Scheme` identified) vs 1282 (none identified).

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not "100% leaves") | ✅ §1/§4 (4 leaves attack-typed; sub-sub Relativisme abusif fully opened, 5/5 leaves addressed incl. anchor) |
| Per unmapped leaf: legitimate decomposition (not Latin alone) | ✅ §3 (attack-type + attacked-node + rebutted scheme where honest, 0 fabrication) |
| `AIF_skosMappingType` coherent | ✅ §4 (narrow for d5 specializations, close for d4 leveling sibling) |
| Fail-loud when no native token fits | ✅ 4/4 CA-conflict fail-loud + 3/4 rebutted-scheme fail-loud, documented in `AIF_skosOther` / annotation CSV |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies → CSV edits (gated) → OWL regen (#763 wiring ready) |

### Coverage accounting (code=truth)

- **Master coverage (rescan)**: **93/1408 mapped** (`AIF_attackType` filled, 6.6%).
- **This cluster (proposed)**: **+4 attack-typed rebut leaves** → 97/1408 projected once applied.
- **Rebut population**: currently **3 rebut** in prod (1282/1313/1361, all Obstruction). This cluster
  would take it to **7 rebut** (+1283/1284/1285/1286) — the Relativisme abusif sub-sub becomes the
  taxonomy's rebut nucleus (5 of the 7).
- **Accounting note**: rebut leaves are counted as **attack-typed** (structural), distinct from the
  **strict scheme+conflict** count used for undercut/undermine clusters (this cluster adds 0 to the
  strict count — the conflict layer is structural, not a native token — and 4 to the attack-typed
  count). This distinction is the §5 method lesson.

**Relativisme abusif sub-sub: fully opened** (anchor 1282 merged + 4 leaves proposed = 5/5 addressed).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### pk 1283 — Sophisme subjectiviste → **rebut** (self-exemption)
- **I-nodes**: premise P (the shared truth, *granted* by the arguer — « fumer nuit ») ; counter-conclusion
  C′ « … mais pas pour moi » opposing the conclusion C « donc cela vaut pour vous aussi ».
- **RA-node**: `Commitment_Inference` — the rebutted argument runs from the arguer's own commitment.
- **CA-node**: structural rebut (C′ conflicts with C) — no native CQ-conflict token.
- **Attack type**: **rebut** (the counter-conclusion opposes the conclusion, not a premise or the warrant).

### pk 1284 — Truthiness → **rebut** (felt counter-conclusion; boundary undermine)
- **I-nodes**: counter-conclusion C′ « X est vrai/faux parce que je le ressens » opposing the evidenced C.
- **RA-node**: *(no clean native rebutted scheme — the felt assertion does not rebut one named scheme)*.
- **CA-node**: structural rebut — no native CQ-conflict token.
- **Attack type**: **rebut**, boundary *undermine* documented (feeling substitutes for the probatory premise).

### pk 1285 — Droit à mon opinion → **rebut** (epistemic parity; boundary undercut)
- **I-nodes**: counter-conclusion C′ « mon avis vaut un fait » opposing C « le fait établi prime ».
- **RA-node**: *(no clean native rebutted scheme)*.
- **CA-node**: structural rebut — no native CQ-conflict token.
- **Attack type**: **rebut**, boundary *undercut* documented (refuses the justification-requiring warrant).

### pk 1286 — Plus faux que faux → **rebut** (false leveling; boundary false-equivalence)
- **I-nodes**: counter-conclusion C′ « les deux torts sont équivalents » opposing the graded C « le tort A
  est plus grave que B ».
- **RA-node**: *(no clean native rebutted scheme)*.
- **CA-node**: structural rebut — no native CQ-conflict token.
- **Attack type**: **rebut**, boundary *false-equivalence* documented (the leveling relativizes gravity).

### Cluster attack-type distribution
| pk | RA-node (rebutted scheme) | CA-node | Attack type |
|----|---------------------------|---------|-------------|
| 1282 | *(none identified)* | structural rebut | rebut (MERGED #760) |
| 1283 | `Commitment_Inference` | structural rebut | rebut |
| 1284 | *(none)* | structural rebut | rebut (boundary undermine) |
| 1285 | *(none)* | structural rebut | rebut (boundary undercut) |
| 1286 | *(none)* | structural rebut | rebut (boundary false-equiv) |

**All 5 rows are rebuts** — the abusive-relativism mechanism *is* the opposition of a subjective/leveling
counter-conclusion. Serialization = `AIF_attackType=rebut` + `AIF_attackedNode=CA-node` for all;
`AIF_skosExceptionRef=Commitment_Inference` for 1283 only; skos otherwise empty (fail-loud, documented).

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No #674/#596/#666/#686 merge, no régén launch. No self-merge.
- ✅ Proposition derived code=truth from the taxonomy CSV (master `7796c127`) + AIF vocabulary verified
  native (`Commitment_Inference` native; the coined rebut tokens verified **NOT native** and excluded
  — 0 fabrication #677).
- ✅ First rebut cluster documented (attack-columns-only pattern); non-native placeholder exclusion
  recorded; per-leaf boundary nuances flagged for ai-01/jsboige adjustment; machine-readable annotation
  CSV `498-relativisme-abusif-annotations.csv` provided for the gated serialization.

Relates: #498 (chantier), #760 (phase 1-3 serialize — anchor 1282 + first 3 rebuts), #757 (apply/verify),
#753/#755 (serialization precedents), #763 (OWL AIF wiring), #707§4 (attack-type map option a),
#677 (0 fabrication), #499 (inverse: virtues), #133/#130 (OWL). Base master `7796c127`.
