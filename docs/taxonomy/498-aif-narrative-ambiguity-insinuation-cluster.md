# 2026-07-05 — #498 AIF chantier, PR-8 : cluster « Ambiguïté narrative / Insinuation » (Ambiguïté, suite)

**Scope**: eighth cluster of the #498 chantier — second cluster of the **Ambiguïté** subfamily
(Abus de langage / Misleading language family), opened by PR-7 (Amphibologie). Models the **insinuation
mechanism** of the Ambiguïté narrative sub-sub: anchor pk 876 + 4 leaves (pk 877-880). **Proposition
only — GATED, 0 write to prod CSV.** Triggered by ai-01 dispatch `ih617l` (tertiary: « continue en
autonomie sur les clusters suivants »). **In-sub-sub anchor shape — independent of the I/RA/CA
serialization decision** (no schema dependency).

**Repo reference**: master `34c7702c`. Issue: #498. Predecessors: PR-1 #699, PR-2 #701, PR-3 #703
(Fallacious comparison complete), PR-4 #705 (Vague definition), PR-6 #708 (Inconsistent definition),
PR-7 #711 (Amphibologie).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies →
> worker applies the CSV edits in a follow-up PR (gated).

---

## TL;DR

The **Ambiguïté narrative** sub-sub (`Soussousfamille=Ambiguïté narrative`) has an **in-sub-sub mapped
anchor** — pk 876 (d3 "Ambiguïté narrative", MAPPED, direct-conflict pattern:
`ConflictingGoals_Conflict`, `skos:broadMatch`) — and **10 unmapped leaves** (pk 877-886). This is
the **same cluster shape as PR-1/PR-4** (in-sub-sub anchor + leaves), the cleanest shape. The sub-sub
is too large for one PR (10 leaves), so **PR-8 scopes to the insinuation mechanism** (anchor 876 +
pk 877-880, 4 leaves); the propaganda (881-883) and interrogation (884-885) mechanisms are deferred
to follow-up PRs (different AIF schemes — source-deception vs implicature).

All 4 insinuation leaves are **say-without-saying** variants — the speaker's narrative serves
**conflicting communicative goals** (a benign surface reading + a deniable insinuated reading), which
is exactly the anchor 876's `ConflictingGoals_Conflict` mechanism. They honestly split across **two
patterns**: direct-conflict reusing the anchor's token (877, 878, 879) and a genuine exception pattern
with a distinct native scheme (880, anonymity → position-to-know defeated).

**4 leaves proposed, all fully-modeled, 0 FAIL-LOUD** — the cleanest cluster since PR-3 (Association
fallacy). This is the first chantier cluster to use `OpposedCommitment_Conflict` (879) and
`LackOfCompleteKnowledge_Conflict` (880), expanding the demonstrated native palette.

---

## 1. Cluster state (code=truth)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Abus de langage** (Misleading
language), subfamily **Ambiguïté**, sub-sub **Ambiguïté narrative**:

| pk | text_fr | depth | DirectRef | ExceptionRef | MappingType | State |
|----|---------|-------|-----------|--------------|-------------|-------|
| **876** | **Ambiguïté narrative** (in-sub-sub anchor) | d3 | `ConflictingGoals_Conflict` | — | `skos:broadMatch` | **✅ MAPPED (PR-8 anchor)** |
| 877 | Fausse implication | d4 | — | — | — | unmapped (**PR-8**) |
| 878 | Argument par l'insinuation | d5 | — | — | — | unmapped (**PR-8**) |
| 879 | Compliment empoisonné | d6 | — | — | — | unmapped (**PR-8**) |
| 880 | Indiscrétion anonyme | d6 | — | — | — | unmapped (**PR-8**) |
| 881 | Propagande grise | d4 | — | — | — | unmapped (→ PR-9, propaganda) |
| 882 | Campagne de murmures | d5 | — | — | — | unmapped (→ PR-9) |
| 883 | Propagande noire | d4 | — | — | — | unmapped (→ PR-9) |
| 884 | Interrogatoire clandestin | d5 | — | — | — | unmapped (→ PR-9, interrogation) |
| 885 | Prêcher le faux pour savoir le vrai | d5 | — | — | — | unmapped (→ PR-9) |
| 886 | Polytélie | d4 | — | — | — | unmapped (→ PR-9, multi-goal) |

The mapped anchor (876) is the **template**: a narrative ambiguity = a narrative whose goals conflict
(a benign surface reading + a deniable insinuated reading), so it `ConflictingGoals_Conflict`. The
insinuation leaves (877-880) are specialized ways the speaker engineers that conflict via implication.
The propaganda (881-883) and interrogation (884-885) leaves target **source-deception** and
**information-extraction** mechanisms — genuinely different AIF schemes (likely `PositionToKnow` /
`ExpertOpinion` family) — so they are honestly deferred to a separate PR rather than forced onto the
anchor's conflicting-goals token.

---

## 2. The anchor model (876, the template)

**Conflict node** (DirectRef): `ConflictingGoals_Conflict` (AIF native, confirmed 3x in existing
usage) — the narrative is engineered to serve conflicting goals: it must read benignly (deniability)
*and* carry the insinuated payload (the actual persuasive goal). The two goals conflict because
stating the payload explicitly would destroy the deniability.

**desc_fr** (876): "Vous présentez votre argument sous la forme d'un récit qui peut être interprété de
plusieurs manières."

**Pattern**: direct-conflict (the scheme/conflict is in DirectRef, no ExceptionRef — the fallacy *is*
the conflicting-goals narrative, there is no "legitimate scheme defeated" framing).

The insinuation leaves specialize along *how* the conflicting goals are engineered: pure implication
(877), deliberate insinuation (878), ironic opposition (879), anonymous sourcing (880).

---

## 3. Proposed AIF structure for the leaves

For each leaf: **(a) legitimate scheme (or direct-conflict reuse), (b) exception/CQ or direct
conflict, (c) `AIF_skosMappingType`**. Vocabulary restricted to AIF-native tokens confirmed by
existing usage.

### pk 877 — Fausse implication (False implication) ✅
- **desc_fr**: "Votre argument suggère des idées sans les énoncer clairement."
- **Mechanism**: pure implication — the statement's surface is benign, but its pragmatic implication
  plants an idea the speaker can deny having asserted. This is the *core* of the anchor's
  conflicting-goals mechanism (benign surface + insinuated payload).
- **Direct conflict**: `ConflictingGoals_Conflict` (native, **borrowed from anchor 876**) — the
  implication engineers the conflict between the deniable surface and the planted idea.
- **Proposal**: `DirectRef=ConflictingGoals_Conflict`, `ExceptionRef=` *(direct-conflict pattern,
  mirroring 876)*, `MappingType=skos:narrowMatch` (a narrower case: conflicting goals *specifically
  via unspoken implication*, narrower than 876's general multi-interpretation narrative).

### pk 878 — Argument par l'insinuation (Argument from innuendo) ✅
- **desc_fr**: "Vous argumentez en insinuant des Choses sans les dire clairement, et laissez au public
  le soin d'interpréter vos sous-entendus."
- **Mechanism**: deliberate insinuation — the speaker offloads the inference to the audience, retaining
  deniability. This is the *canonical* insinuation case (the anchor's mechanism made explicit).
- **Direct conflict**: `ConflictingGoals_Conflict` (borrowed from anchor 876).
- **Proposal**: `DirectRef=ConflictingGoals_Conflict`, `ExceptionRef=`,
  `MappingType=skos:closeMatch` (a direct variant: 878 *is* the insinuation mechanism that 876 frames
  abstractly — the closest sibling of the anchor).

### pk 879 — Compliment empoisonné (Poisoned compliment / backhanded compliment) ✅
- **⚠ Semantic specialization**: unlike 877/878 (benign surface + insinuated payload), the poisoned
  compliment's surface (praise) and payload (discredit) are **opposites** — the speaker's expressed
  commitment (praise) is directly opposed to their real commitment (discredit). This is a cleaner fit
  for `OpposedCommitment_Conflict` than for the anchor's `ConflictingGoals_Conflict`: the defect is
  not just conflicting goals but an *opposed* commitment.
- **Direct conflict**: `OpposedCommitment_Conflict` (AIF native, confirmed 2x in existing usage —
  captures a commitment that opposes the agent's actual position).
- **Proposal**: `DirectRef=OpposedCommitment_Conflict`, `ExceptionRef=`,
  `MappingType=skos:closeMatch` (a direct variant: opposed-commitment *via a compliment whose
  insinuated meaning is the opposite of its surface*).

### pk 880 — Indiscrétion anonyme (Anonymous indiscretion / anonymous sourcing) ✅
- **⚠ Genuine exception pattern (distinct scheme)**: unlike 877-879 (implication mechanisms), the
  anonymous-indiscretion defect is **epistemic** — the rumor is spread without a verifiable source, so
  the audience cannot assess whether the source is in a position to know. This honestly targets
  `PositionToKnow_Inference` (the rumor presupposes a source-in-a-position-to-know), defeated because
  the anonymity hides whether that precondition holds.
- **Legitimate scheme**: `PositionToKnow_Inference` (AIF native, confirmed 2x in existing usage).
- **Exception/CQ**: the source's position-to-know is **unverifiable** due to anonymity → the inference
  is defective. Native conflict: `LackOfCompleteKnowledge_Conflict` (AIF native, confirmed 1x —
  captures the gap between what is claimed and what the source can actually know).
- **Proposal**: `ExceptionRef=PositionToKnow_Inference`, `DirectRef=LackOfCompleteKnowledge_Conflict`,
  `MappingType=skos:narrowMatch` (a narrower case: position-to-know defeated *specifically via
  anonymous sourcing*, narrower than the general lack-of-knowledge defeat).

---

## 4. Cluster summary (proposed)

| pk | text_fr | Proposed ExceptionRef | Proposed DirectRef | MappingType | Honest? |
|----|---------|----------------------|-------------------|-------------|---------|
| 877 | Fausse implication | *(absent)* | `ConflictingGoals_Conflict` | `skos:narrowMatch` | ✅ |
| 878 | Argument par l'insinuation | *(absent)* | `ConflictingGoals_Conflict` | `skos:closeMatch` | ✅ |
| 879 | Compliment empoisonné | *(absent)* | `OpposedCommitment_Conflict` | `skos:closeMatch` | ✅ |
| 880 | Indiscrétion anonyme | `PositionToKnow_Inference` | `LackOfCompleteKnowledge_Conflict` | `skos:narrowMatch` | ✅ |

**4 leaves proposed, 4 fully-modeled, 0 FAIL-LOUD.** The cleanest cluster since PR-3 (Association
fallacy). The leaves split across **two patterns**: direct-conflict reusing the anchor's
`ConflictingGoals_Conflict` (877, 878) or a semantically-closer native conflict (879
`OpposedCommitment_Conflict`), and a genuine exception pattern with a distinct native scheme (880
`PositionToKnow_Inference` + `LackOfCompleteKnowledge_Conflict`).

### ⚠ Accounting transparency note (re: po-2024 audit #709)
po-2024's adversarial audit flagged a fully-modeled **counting-criterion inconsistency** between PR-1
(pk 840, RA-exists/CA-missing circularity, counted in PR-1's "+4") and PR-6 (pk 829, same shape,
excluded from "+5"). To avoid compounding: this PR's "+4 fully-modeled" uses the **strict criterion**
(all 4 leaves have BOTH a scheme and a conflict) — it is unaffected by the inclusive/strict
discrepancy. The cumulative headline (§6) is reported under both criteria for jsboige's criterion
decision. See PR #709 §7 flag and coverage-status #707 §4.

---

## 5. Method notes (additions for the chantier)

- **Sub-sub splitting by mechanism (large sub-sub)**: the Ambiguïté narrative sub-sub has 10 leaves
  spanning three distinct AIF-relevant mechanisms — **insinuation** (877-880, conflicting-goals /
  opposed-commitment / position-to-know), **propaganda/source-deception** (881-883, likely
  `PositionToKnow`/`ExpertOpinion` family), and **interrogation/information-extraction** (884-885).
  Forcing them into one PR would mix schemes incoherently. The méthode's "2-5 leaves/PR" rule (§3 of
  #707) applies: split by mechanism, one coherent AIF-scheme family per PR. PR-8 = insinuation;
  PR-9 = propaganda + interrogation (deferred).
- **Anchor-token reuse vs semantic-closer native conflict (879)**: when a leaf's mechanism is a
  specialization of the anchor's token (877/878 → `ConflictingGoals_Conflict`), reuse it and vary only
  the MappingType. But when a leaf has a *semantically closer* native conflict available (879's
  praise/discredit opposition → `OpposedCommitment_Conflict`), prefer the closer token over rote
  anchor reuse — same discipline as PR-2 §5 scheme-divergence, applied at the conflict-node level.
- **Epistemic vs implication mechanisms (880)**: not all narrative-ambiguity leaves are implication
  mechanisms. 880 (anonymous sourcing) is an **epistemic** defect (unverifiable source), honestly
  targeting `PositionToKnow_Inference` rather than the anchor's conflicting-goals token. Recognizing
  the mechanism family (implication vs epistemic vs source-deception) drives the scheme choice — this
  is why the sub-sub splits cleanly along mechanism lines.
- **Native palette expansion**: this cluster adds two native tokens not previously used in the
  chantier — `OpposedCommitment_Conflict` (879) and `LackOfCompleteKnowledge_Conflict` (880). Both are
  confirmed in existing usage (2x and 1x respectively), so no fabrication. The chantier has now
  demonstrated 17 distinct native tokens across 8 PR.

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not "100% leaves") | ✅ §4 (4 insinuation leaves fully-modeled; sub-sub split documented, 6 leaves deferred to PR-9) |
| Per unmapped leaf: legitimate scheme + exception/CQ (not Latin alone) | ✅ §3 (4 leaves, scheme + CQ/conflict each, 0 FAIL-LOUD) |
| `AIF_skosMappingType` coherent | ✅ §4 (narrow/close encode the specialization relation to the anchor) |
| Fail-loud when no honest scheme fits | N/A this cluster (all leaves have a native scheme) |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies, then CSV edits, then OWL regen (#133) |

### Coverage accounting (cumulative, chantier-wide — dual criterion)
| PR | Cluster | Leaves | Fully-modeled (strict) | DirectRef-loose |
|----|---------|--------|------------------------|-----------------|
| baseline | — | — | 70 | 70 |
| PR-1 #699 | False analogy | 4 | 3 (840 CA-missing)¹ | 74 (+4) |
| PR-2 #701 | Faulty comparison | 5 | 2 (834/835/837 RA-missing) | 79 (+5) |
| PR-3 #703 | Association fallacy | 2 | 2 | 81 (+2) |
| PR-4 #705 | Vague definition | 3 | 3 | 84 (+3) |
| PR-6 #708 | Inconsistent definition | 7 | 5 (829 CA-missing, 832 RA-missing) | 89 (+5) |
| PR-7 #711 | Amphibologie | 8 | 5 (847/848/853 CA-missing) | 94 (+5) |
| **PR-8 (this)** | **Narrative ambiguity / insinuation** | **4** | **4** | **98 (+4)** |
| **total (strict)** | **4 subfamilies touched** | **33** | **94** | **98** |
| **total (inclusive²)** | | **33** | **95** | **98** |

¹ PR-1 originally reported "+4 mapped" counting pk 840 (RA-exists/CA-missing) as mapped. Under the
strict criterion (CA must exist), PR-1 = +3. Flagged by po-2024 audit #709.
² Inclusive = count RA-exists/CA-missing rows (840) as fully-modeled (PR-1's original convention).
The 1-row discrepancy (94 strict vs 95 inclusive) is the accounting decision pending jsboige.

**This PR's +4 is criterion-independent** — all 4 leaves have both a scheme and a conflict, so they
count as fully-modeled under any criterion. The discrepancy is entirely in the PR-1 baseline (840).

**Ambiguïté subfamily: 2/3 sub-subs touched** (Amphibologie PR-7 + Narrative-ambiguity/insinuation
PR-8). Narrative-ambiguity sub-sub: 4/10 leaves done (insinuation), 6 deferred (propaganda +
interrogation + polytélie → PR-9). Remaining Ambiguïté sub-sub: Equivoque (2 mapped + 18 unmapped,
large — split by depth).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### pk 877 — Fausse implication → **undercut**
- **RA-node**: *(the audience's inference from the benign surface to the implied idea, scheme-untyped
  — an implicature)*.
- **I-nodes**: premise P "the speaker asserted the benign surface S"; the audience infers the implied
  idea I (S ⟹ I pragmatically).
- **CA-node**: `ConflictingGoals_Conflict`, applied as an **undercut** — the inference (that S licenses
  I) is defective because the speaker engineered S to serve conflicting goals (deniability + payload);
  the implicature is not a genuine inference but a planted one. The conclusion I may still be true; the
  *inference* is defective.

### pk 878 — Argument par l'insinuation → **undercut**
- **RA-node**: *(the audience's inference from the insinuation to the implied claim, scheme-untyped)*.
- **CA-node**: `ConflictingGoals_Conflict`, applied as an **undercut** — same shape as 877, the
  insinuation is engineered for deniability, so the audience's inference is not a genuine licensed
  inference.

### pk 879 — Compliment empoisonné → **undercut**
- **RA-node**: *(the audience's inference that the compliment sincerely expresses praise)*.
- **I-nodes**: premise P "the speaker uttered praise P about the target".
- **CA-node**: `OpposedCommitment_Conflict`, applied as an **undercut** — the inference (that P
  expresses the speaker's sincere praise) is defective because the speaker's actual commitment is
  opposed to P (they intend discredit). The praise is a vehicle for the opposite. Classic undercut:
  the premise P is true (the words were spoken) but the inference to "sincere praise" is defective.

### pk 880 — Indiscrétion anonyme → **undercut**
- **RA-node**: `PositionToKnow_Inference` — the rumor presupposes an anonymous source in a position to
  know.
- **I-nodes**: premise P "an anonymous source S reported claim C"; premise P2 "S is in a position to
  know C"; conclusion "C holds".
- **CA-node**: `LackOfCompleteKnowledge_Conflict`, applied as an **undercut** on P2 — the inference
  (that S's report licenses C) is defective because S's anonymity hides whether S is actually in a
  position to know. P2 is unverifiable, so the position-to-know inference is ungrounded.

### Cluster attack-type distribution
| pk | RA-node | CA-node | Attack type |
|----|---------|---------|-------------|
| 877 | *(untyped implicature)* | `ConflictingGoals_Conflict` | undercut |
| 878 | *(untyped implicature)* | `ConflictingGoals_Conflict` | undercut |
| 879 | *(untyped sincerity-inference)* | `OpposedCommitment_Conflict` | undercut |
| 880 | `PositionToKnow_Inference` | `LackOfCompleteKnowledge_Conflict` | undercut |

**All 4 leaves are undercuts** — consistent with jsboige's "most fallacies live in the undercut":
insinuation fallacies defeat the *inference* the audience is nudged to draw (surface ⟹ implied), they
do not deny the surface statement (which is, by design, benign and true). No undermine, no rebut.

Note on the untyped RA-nodes (877/878/879): the audience's inference in insinuation is a **Gricean
implicature**, not a named Walton scheme. AIF has no native `Implicature_Inference` scheme (would be
an ontology extension). The CA-node (the conflict) is typed natively; the RA-node is left scheme-
untyped rather than fabricating an `Implicature_Inference` token (FAIL-LOUD discipline at the scheme
layer, even though the conflict layer is fully native). This is a fourth AIF ontology gap shape —
**implicature** — distinct from the three CA-missing gaps (circularity, pure-comparison, ambiguity),
flagged here for the eventual ontology-extension decision.

### Representation note
As in PR-1 to PR-7, the I-node/RA-node/CA-node decomposition is **recorded here but not serialized**
in the CSV — pending jsboige's schema decision (§4 of the coverage-status note #707). This PR is
**in-sub-sub anchor and independent of that decision**: the modeling (schemes + conflicts) holds
regardless of whether I/RA/CA is serialized in new columns.

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No #674/#596/#666/#686 merge, no régén launch.
- ✅ Proposition derived code=truth from the taxonomy CSV + AIF vocabulary confirmed by existing
  usage (all proposed tokens native: `ConflictingGoals_Conflict`, `OpposedCommitment_Conflict`,
  `PositionToKnow_Inference`, `LackOfCompleteKnowledge_Conflict`). No fabrication — the only
  untyped-RA cases (877/878/879 implicature) document the absent `Implicature_Inference` scheme
  rather than fabricating it.
- ✅ Sub-sub split by mechanism documented (insinuation PR-8; propaganda/interrogation deferred
  PR-9); anchor-token reuse vs semantic-closer conflict (879) documented; accounting transparency
  (dual criterion, §6) provided re: po-2024 audit #709.

Relates: dispatch `ih617l` (tertiary), #498 (chantier), PR-1 #699, PR-2 #701, PR-3 #703, PR-4 #705,
PR-6 #708, PR-7 #711, coverage-status #707, verification-audit #709, family-gap #712, #133/#130
(existing OWL), #499 (inverse), #677 (0 fabrication), #192, #458.
