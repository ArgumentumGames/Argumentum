# 2026-07-06 — #498 AIF chantier, PR-5 : cluster « Acception arbitraire / breadth defects » (arbitrary definition — too broad / too narrow)

**Scope**: fifth cluster of the #498 chantier — opens the **Acception arbitraire** sub-sub (Inexact
definition subfamily, Abus de langage / Misleading language family) with the **breadth-defects
mechanism** (definition whose extension is arbitrarily too large or too narrow). **Proposition only —
GATED, 0 write to prod CSV.** Triggered by ai-01 dispatch `0e5mhk` (primary: PR-5 Arbitrary-definition,
unblocked now that the I/RA/CA schema is ratified option a). **Borrowed-root anchor shape — now
serializable post-tag (option a).**

**Repo reference**: master `7ebeda18`. Issue: #498. Predecessors: PR-1 #699, PR-2 #701, PR-3 #703,
PR-4 #705 (Acception vague), PR-6 #708 (Définition inconsistante), PR-7 #711, PR-8 #713, PR-9 #714,
PR-10 #717, PR-11 #718, PR-12 #720 (**Equivoque sub-sub COMPLETE**).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies →
> worker applies the CSV edits in a follow-up PR (gated). The I/RA/CA decomposition (§7) is recorded
> but NOT serialized — pending post-tag application of the ratified option (a) columns.

> **PR-5 was previously HELD** (the cluster with back-fill risk: if the I/RA/CA serialization decision
> had gone differently, the §7 decomposition would have needed rework). With option (a) ratified
> (2 new columns `AIF_attackType`+`AIF_attackedNode`, post-tag serialization), PR-5 is unblocked. This
> PR is the first to be written knowing the serialization shape.

---

## TL;DR

The **Inexact definition subfamily** (`Sous-Famille=Définition inexacte`) has 3 sub-subs: **Acception
vague** (PR-4 #705, complete), **Définition inconsistante** (PR-6 #708, complete), and **Acception
arbitraire** (pk 804-825, 22 rows — **this PR's sub-sub, the largest untouched in the subfamily**).
Acception arbitraire has **2 mapped rows** — pk 804 (d3 anchor "Acception arbitraire", MAPPED
`PropertyNotExistant_Conflict` direct-conflict, `skos:broadMatch`) and pk 808 (d4 "Sophisme des
corrélatifs", MAPPED `AlternativeMeans_Conflict`, `skos:broadMatch`) — and **20 unmapped leaves**.

The sub-sub is **the chantier's joint-largest** (22 rows, tied with nothing — Equivoque was 21) and
semantically spans **three distinct mechanisms**, so it is split by mechanism (this PR = breadth
defects; PR-13 = dichotomy/correlate; PR-14 = persuasive language).

**PR-5 scopes to the breadth-defects mechanism**: anchor pk 804 (recap) + 3 leaves (805 incongruité,
806 too-large, 807 too-restrictive). All 3 leaves honestly model the arbitrary-breadth defect via the
**exception pattern**: `ArbitraryVerbalClassification_Inference` (the RA-node — the definition is a
verbal classification with arbitrary extension) + `PropertyNotExistant_Conflict` (the CA-node — the
made-up extension conflicts with the legitimate property/extension, reusing the anchor's conflict
token). The `MappingType` encodes the breadth axis: 805 = abstract (close), 806/807 = the two
breadth-direction specializations (narrow each).

**4 rows proposed (3 fully-modeled + 1 template recap), 0 FAIL-LOUD, 0 fabrication.** Accounting
(uniform-strict, po-2024 #716): **+3 strict, 114 cumulative**. This opens the last Inexact-definition
sub-sub; the dichotomy (PR-13) and persuasive-language (PR-14) mechanisms are deferred.

---

## 1. Cluster state (code=truth)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Abus de langage** (Misleading
language), subfamily **Définition inexacte**, sub-sub **Acception arbitraire**:

| pk | text_fr | depth | DirectRef | ExceptionRef | MappingType | State |
|----|---------|-------|-----------|--------------|-------------|-------|
| **804** | **Acception arbitraire** (anchor/template) | d3 | `PropertyNotExistant_Conflict` | — | `skos:broadMatch` | **✅ MAPPED (PR-5 template)** |
| 805 | Incongruité | d4 | — | — | — | unmapped (**PR-5**, breadth abstract) |
| 806 | Définition trop large | d5 | — | — | — | unmapped (**PR-5**, too-broad) |
| 807 | Définition trop restrictive | d5 | — | — | — | unmapped (**PR-5**, too-narrow) |
| 808 | Sophisme des corrélatifs | d4 | `AlternativeMeans_Conflict` | — | `skos:broadMatch` | ✅ MAPPED (PR-13 template, dichotomy) |
| 809-825 | *(18 leaves)* | d5-d7 | — | — | — | unmapped (PR-13 dichotomy + PR-14 persuasive) |

The anchor (804) is the **paradigm**: an arbitrary definition = a made-up term extension that conflicts
with legitimate usage. The breadth leaves (805, 806, 807) are the direct specializations — they vary
by the *direction* of the breadth error (too large / too narrow / either). The dichotomy mechanism
(808-818, anchored at the mapped 808) and the persuasive-language mechanism (819-825) are deferred to
PR-13/14.

---

## 2. The template model (804, the anchor)

**Conflict token**: `PropertyNotExistant_Conflict` (AIF native — the conflict arises when a property/
extension is claimed that does not legitimately exist). For an arbitrary definition, the arguer invents
a term extension (a "property") that has no standing in common usage — the definition attributes a
non-existent property to the term.

**desc_fr** (804): "Vous inventez une définition sur mesure pour vos termes, sans respecter leur sens
commun."

**Pattern**: **direct-conflict-via-Conflict-token** (the Conflict token sits in DirectRef, ExceptionRef
empty). This is an *unusual* pattern in the chantier (most direct-conflicts use an Inference scheme in
DirectRef, with the conflict implicit). Here the fallacy is modeled *as* a bare conflict — the
arbitrary definition *is* a property-not-existent conflict, with no separate inference scheme named.
The RA-node is therefore **untyped** (the definition-based inference is implicit). This is the same
"untyped-RA-node" shape as PR-8's implicature gap (§5 of #713) — recorded honestly in §7.

The breadth leaves (PR-5) **improve on the anchor's precision**: they name the RA-node explicitly
(`ArbitraryVerbalClassification_Inference`) and keep the conflict as the CA-node (`PropertyNotExistant_
Conflict`, reused). This is the **exception pattern** (scheme + conflict) — richer than the anchor's
bare direct-conflict, and the natural shape now that I/RA/CA serialization is ratified.

---

## 3. Proposed AIF structure for the leaves

For each leaf: **exception pattern** — `ArbitraryVerbalClassification_Inference` (the RA-node: the
definition is a verbal classification with arbitrary extension) in DirectRef, `PropertyNotExistant_
Conflict` (the CA-node: the made-up extension conflicts with legitimate usage, reused from anchor 804)
in ExceptionRef. The `MappingType` encodes the breadth direction. Vocabulary restricted to AIF-native
tokens (0 fabrication, #677).

### pk 805 — Incongruité (Incongruous definition — too broad OR too narrow) ✅
- **desc_fr**: "Votre argument repose sur une définition trop large ou trop restrictive d'un terme."
- **Mechanism**: the **abstract breadth-arbitrary case** — the definition's extension is wrong-sized
  (either direction). This is the general form of which 806 (too-large) and 807 (too-restrictive) are
  the two specializations.
- **Proposal**: `DirectRef=ArbitraryVerbalClassification_Inference`, `ExceptionRef=PropertyNotExistant_
  Conflict`, `MappingType=skos:closeMatch` (a direct sibling of the anchor 804 — the abstract
  breadth-arbitrary case; the classification is arbitrary, conflicting with legitimate usage).

### pk 806 — Définition trop large (Definition too broad / Over-inclusive) ✅
- **desc_fr**: "Votre argument repose sur une définition trop large."
- **Mechanism**: breadth-arbitrary in the **over-inclusive direction** — the term's extension admits
  items it should not (the definition is too wide). A *narrower* specialization than 805: specifically
  the too-large direction, not either.
- **Proposal**: `DirectRef=ArbitraryVerbalClassification_Inference`, `ExceptionRef=PropertyNotExistant_
  Conflict`, `MappingType=skos:narrowMatch` (a narrower case: breadth-arbitrary *specifically in the
  over-inclusive direction*).

### pk 807 — Définition trop restrictive (Definition too narrow / Under-inclusive) ✅
- **desc_fr**: "Votre argument repose sur une définition trop restrictive."
- **Mechanism**: breadth-arbitrary in the **under-inclusive direction** — the term's extension excludes
  items it should not (the definition is too tight). The mirror of 806: specifically the too-narrow
  direction.
- **Proposal**: `DirectRef=ArbitraryVerbalClassification_Inference`, `ExceptionRef=PropertyNotExistant_
  Conflict`, `MappingType=skos:narrowMatch` (a narrower case: breadth-arbitrary *specifically in the
  under-inclusive direction* — the mirror-sibling of 806).

---

## 4. Cluster summary (proposed)

| pk | text_fr | Proposed DirectRef | Proposed ExceptionRef | MappingType | Honest? |
|----|---------|-------------------|----------------------|-------------|---------|
| 804 | Acception arbitraire | `PropertyNotExistant_Conflict` | *(absent)* | `skos:broadMatch` | ✅ (template, recap) |
| 805 | Incongruité | `ArbitraryVerbalClassification_Inference` | `PropertyNotExistant_Conflict` | `skos:closeMatch` | ✅ |
| 806 | Définition trop large | `ArbitraryVerbalClassification_Inference` | `PropertyNotExistant_Conflict` | `skos:narrowMatch` | ✅ |
| 807 | Définition trop restrictive | `ArbitraryVerbalClassification_Inference` | `PropertyNotExistant_Conflict` | `skos:narrowMatch` | ✅ |

**4 rows proposed (3 fully-modeled + 1 template recap), 0 FAIL-LOUD, 0 fabrication.** All 3 leaves
reuse the anchor 804's `PropertyNotExistant_Conflict` (in-sub-sub coherence, as PR-10 leaves reused
856) and add `ArbitraryVerbalClassification_Inference` to name the RA-node (the exception pattern,
richer than the anchor's bare direct-conflict). The `MappingType` encodes the breadth axis: 805 close
(abstract), 806/807 narrow (the two direction-specializations, a mirror-pair).

### Breadth-mechanism taxonomy (§5 lesson)
Within the breadth-defects mechanism, the leaves specialize along **one axis** (breadth direction):
- **805** = the abstract case (too-large OR too-narrow) → `close`.
- **806 / 807** = the two direction-specializations (over-inclusive / under-inclusive) → `narrow`
  each, a graded mirror-pair.

This is the **simplest axis structure** in the chantier (one axis, vs PR-10's three polysemy axes or
PR-11's heterogeneous residual). The breadth mechanism is genuinely one-dimensional: the only question
is which direction the extension is wrong. Same MappingType-encodes-breadth discipline as PR-10/11.

---

## 5. Method notes (additions for the chantier)

- **Direct-conflict-via-Conflict-token pattern (anchor 804)**: PR-5's anchor models the fallacy *as a
  bare conflict* (`PropertyNotExistant_Conflict` in DirectRef, no Inference scheme). This is the
  chantier's first anchor using a Conflict token (not an Inference scheme) as the direct reference.
  It implies an **untyped RA-node** (the definition-based inference is implicit, not named) — the same
  shape as PR-8's implicature gap. Documented honestly; the leaves (805-807) improve precision by
  naming the RA-node (`ArbitraryVerbalClassification_Inference`), the exception pattern.
- **Leaves richer than anchor (exception pattern)**: PR-5 is the chantier's clearest case of leaves
  being *more precisely modeled* than their anchor. The anchor (804) uses the bare direct-conflict
  pattern; the leaves (805-807) use the exception pattern (scheme + conflict), naming both the RA-node
  and CA-node. This is legitimate — an anchor can be a broad-brush template while its leaves are
  precisely decomposed. The discipline: the leaves' tokens must still be native (they are:
  `ArbitraryVerbalClassification_Inference` native, `PropertyNotExistant_Conflict` reused from anchor).
- **First PR written knowing the serialization shape (option a)**: PR-5 was held back precisely because
  its §7 I/RA/CA decomposition risked rework if the serialization decision changed. With option (a)
  ratified (2 new columns, post-tag), PR-5 is written knowing the decomposition will serialize
  cleanly: `AIF_attackType=undercut`, `AIF_attackedNode=<RA-node>` for the leaves. This validates the
  hold-wait decision — the decomposition holds regardless, and now serializes.
- **Cross-sub-sub scheme reuse (ArbitraryVerbalClassification_Inference → PR-11)**: the scheme
  `ArbitraryVerbalClassification_Inference` was first used standalone in PR-11 (pk 857 Fausse précision).
  PR-5 reuses it for the breadth leaves — the arbitrary-definition breadth defect and the
  false-precision defect are both arbitrary verbal classifications, in different sub-subs (Inexact
  definition vs Equivoque). Cross-sub-sub reuse documented (same level as PR-6/PR-7 cross-sub-sub
  reuse), confirming the verbal-classification scheme family's reach.

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not "100% leaves") | ✅ §4 (3 breadth leaves fully-modeled; Acception arbitraire sub-sub split documented, ~18 leaves deferred to PR-13/14) |
| Per unmapped leaf: legitimate scheme + exception/CQ (not Latin alone) | ✅ §3 (3 leaves, scheme + conflict each, 0 FAIL-LOUD) |
| `AIF_skosMappingType` coherent | ✅ §4 (close for abstract, narrow for the two direction-specializations) |
| Fail-loud when no honest scheme fits | N/A this cluster (all leaves reuse native scheme + conflict) |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies, then CSV edits, then OWL regen (#133) |

### Coverage accounting (cumulative — uniform-strict headline, po-2024 #716)

| PR | Cluster | +strict fully-modeled | strict cumulative |
|----|---------|----------------------|------------------|
| baseline | — | — | 70 |
| PR-1 #699 | False analogy | +3 | 73 |
| PR-2 #701 | Faulty comparison | +2 | 75 |
| PR-3 #703 | Association fallacy | +2 | 77 |
| PR-4 #705 | Vague definition | +3 | 80 |
| PR-6 #708 | Inconsistent definition | +5 | 85 |
| PR-7 #711 | Amphibologie | +5 | 90 |
| PR-8 #713 | Narrative ambiguity / insinuation | +4 | 94 |
| PR-9 #714 | Narrative ambiguity / deception | +6 | 100 |
| PR-10 #717 | Equivoque / polysemy | +5 | 105 |
| PR-11 #718 | Equivoque / residual | +4 | 109 |
| PR-12 #720 | Equivoque / reification | +2 | 111 |
| **PR-5 (this)** | **Arbitrary definition / breadth** | **+3** | **114** |
| **total (strict headline)** | **4 subfamilies, 13 clusters** | **+44** | **114** |

**Partial / gap leaves tracked honestly (17 total, unchanged by this PR)**:
- 5 CA-missing (→119 potential): 840, 829, 847/848/853.
- 4 RA-missing (deferred): 834/835/837, 832.
- 8 FAIL-LOUD gaps: 861 (perceptual, PR-11) + 868-874 (reification cluster, PR-12).

**This PR's +3 is criterion-independent** — all 3 leaves have both a scheme and a conflict.

**Inexact definition subfamily: all 3 sub-subs now opened** (PR-4 Acception vague + PR-5 Acception
arbitraire breadth + PR-6 Définition inconsistante). Remaining Acception arbitraire mechanisms:
dichotomy/correlate (808 template + 809-818, ~11 leaves → PR-13), persuasive language (819-825, 7
leaves → PR-14).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### pk 804 — Acception arbitraire (anchor/template) → **undercut** (untyped RA-node)
- **RA-node**: *(untyped)* — the definition-based inference is implicit (the anchor models the fallacy
  as a bare conflict, no Inference scheme named). This is the bare-direct-conflict-via-Conflict-token
  pattern.
- **CA-node**: `PropertyNotExistant_Conflict` — the made-up extension attributes a property that does
  not legitimately exist.
- **Attack type**: **undercut** (the conflict attacks the implicit inference's warrant, not its
  conclusion — the conclusion may hold under a legitimate definition).

### pk 805 — Incongruité → **undercut** (exception pattern)
- **RA-node**: `ArbitraryVerbalClassification_Inference` — defining/classifying via an arbitrarily-
  sized extension (too large or too narrow).
- **I-nodes**: premise P1 "term T's extension is E (the made-up size)"; conclusion C "items fall
  under T as per E".
- **CA-node**: `PropertyNotExistant_Conflict` — the made-up extension E conflicts with the legitimate
  extension.
- **Attack type**: **undercut** — the inference (T's extension ⟹ classification) is defective because
  E is arbitrary; the conclusion may hold under a legitimate extension.

### pk 806 — Définition trop large → **undercut** (exception pattern, over-inclusive)
- **RA-node**: `ArbitraryVerbalClassification_Inference` — the extension is over-inclusive.
- **CA-node**: `PropertyNotExistant_Conflict` — the over-inclusive extension admits items with no
  legitimate claim to the property.
- **Attack type**: **undercut** (degenerate — the CA-node names the defect).

### pk 807 — Définition trop restrictive → **undercut** (exception pattern, under-inclusive)
- **RA-node**: `ArbitraryVerbalClassification_Inference` — the extension is under-inclusive.
- **CA-node**: `PropertyNotExistant_Conflict` — the under-inclusive extension excludes items that
  legitimately hold the property.
- **Attack type**: **undercut** (degenerate — the CA-node names the defect).

### Cluster attack-type distribution
| pk | RA-node | CA-node | Attack type |
|----|---------|---------|-------------|
| 804 | *(untyped)* | `PropertyNotExistant_Conflict` | undercut |
| 805 | `ArbitraryVerbalClassification_Inference` | `PropertyNotExistant_Conflict` | undercut |
| 806 | `ArbitraryVerbalClassification_Inference` | `PropertyNotExistant_Conflict` | undercut (degenerate) |
| 807 | `ArbitraryVerbalClassification_Inference` | `PropertyNotExistant_Conflict` | undercut (degenerate) |

**All 4 rows are undercuts** — the arbitrary definition attacks the warrant of the definition-based
inference (the conclusion may hold under a legitimate definition). The leaves name both nodes
(exception pattern); the anchor leaves the RA-node untyped (bare direct-conflict). No undermine, no
rebut.

### Representation note — **first PR with serialization-known decomposition**
PR-5 is the chantier's first PR written knowing the I/RA/CA decomposition will serialize (option (a)
ratified). The decomposition maps cleanly to the post-tag columns: for each leaf, `AIF_attackType=
undercut` and `AIF_attackedNode=<the RA-node>` (the anchor's attacked-node would be the untyped
inference, recorded as such). This validates the hold-wait: the decomposition was correct independent
of the decision, and now serializes without rework. The decomposition is recorded here; the CSV edits
remain gated (proposition only).

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No #674/#596/#666/#686 merge, no régén launch.
- ✅ Proposition derived code=truth from the taxonomy CSV + AIF vocabulary confirmed by existing usage
  (both proposed tokens — `ArbitraryVerbalClassification_Inference` (native, first standalone use
  PR-11), `PropertyNotExistant_Conflict` (native, reused from anchor 804) — are native, 0 fabrication).
- ✅ Breadth-mechanism split documented; leaves-richer-than-anchor (exception pattern) noted;
  direct-conflict-via-Conflict-token pattern (anchor) recorded; first serialization-known-§7 PR flagged;
  uniform-strict accounting headline maintained (po-2024 #716).

Relates: dispatch `0e5mhk` (primary), #498 (chantier), PR-1 #699, PR-2 #701, PR-3 #703, PR-4 #705,
PR-6 #708, PR-7 #711, PR-8 #713, PR-9 #714, PR-10 #717, PR-11 #718, PR-12 #720, coverage-status #707,
verification-audit #709 + addendum #716, family-gap #712, #133/#130 (existing OWL), #499 (inverse),
#677 (0 fabrication), #192, #458.
