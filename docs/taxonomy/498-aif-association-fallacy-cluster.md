# 2026-07-05 — #498 AIF chantier, PR-3 : cluster « Association fallacy » (Fallacious comparison, fin)

**Scope**: third cluster of the #498 chantier — model the **Association fallacy** sub-sub
(`Soussousfamille=Association fallacy`, pk 844-845) of the Fallacious comparison subfamily. Completes
PR-1 (False analogy #699) + PR-2 (Faulty comparison #701) → the entire **Fallacious comparison**
subfamily is now mapped. **Proposition only — GATED, 0 write to prod CSV.** Triggered by ai-01
dispatch `mkzt2v` (primary).

**Repo reference**: master `70bd1605`. Issue: #498 (reformulated, GO jsboige 2026-06-17 verified).
Predecessors: PR-1 #699 (False analogy), PR-2 #701 (Faulty comparison).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. This doc proposes
> the AIF structure for the cluster's unmapped leaves, derived code=truth from the CSV + the AIF
> native vocabulary confirmed by existing usage. jsboige ratifies → worker applies the CSV edits in
> a follow-up PR (gated).

---

## TL;DR

The **Association fallacy** sub-sub has **2 leaves** — pk 844 (d3 anchor "Sophisme d'association") +
pk 845 (d4 "Amalgame"). As in PR-2, there is **no in-sub-sub mapped anchor** → the cluster borrows
the d2 root pk 833 ("Comparaison fallacieuse", MAPPED, direct-conflict pattern).

Both leaves are **classification-flavored** (like PR-2's 836/838): they wrongly assign items to a
category / association on the basis of a shared trait that is either irrelevant (844) or trivially
singular (845). They honestly reuse native classification schemes — `VerbalClassification_Inference`
(844) and `ArbitraryVerbalClassification_Inference` (845) — and native Conflict nodes inherited from
the borrowed anchor 833 (`BiasedClassification_Conflict`, `ExceptionSimilarityCase_Conflict`).

**This is the cleanest cluster of the chantier: 2/2 leaves fully-modeled, 0 FAIL-LOUD.** No missing
scheme, no missing CQ. It also **completes the Fallacious comparison subfamily** (13 leaves across
3 sub-subs, all now mapped).

---

## 1. Cluster state (code=truth)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Misleading language**, subfamily
**Fallacious comparison**, sub-sub **Association fallacy**:

| pk | text_fr | depth | DirectRef | ExceptionRef | MappingType | State |
|----|---------|-------|-----------|--------------|-------------|-------|
| **833** | **Comparaison fallacieuse** (borrowed anchor) | d2 | `BiasedClassification_Conflict`, `ExceptionSimilarityCase_Conflict` | — | `skos:broadMatch` | **✅ MAPPED (d2 root, parent of this sub-sub)** |
| 844 | Sophisme d'association | d3 | — | — | — | unmapped (d3 anchor, **PR-3**) |
| 845 | Amalgame | d4 | — | — | — | unmapped (**PR-3**) |

The borrowed anchor (833) is the same template as PR-2: a **direct-conflict pattern** (DirectRef to
two Conflict nodes, ExceptionRef empty). But unlike PR-2's pure-comparison leaves (which had no
native scheme), both Association-fallacy leaves honestly fit a native classification scheme, so they
follow PR-1's **exception pattern** (named scheme in ExceptionRef + CQ in DirectRef).

---

## 2. The borrowed anchor (833, recap)

**Pattern**: direct-conflict (no ExceptionRef).

**Conflict nodes** (DirectRef): `BiasedClassification_Conflict` (the invoked association biases the
classification) + `ExceptionSimilarityCase_Conflict` (the case treated as similar is exceptional, not
representative).

Both Association-fallacy leaves specialize these: 844 leans on the *biased-classification* side
(irrelevant trait), 845 leans on the *exceptional-similarity* side (a single common point).

---

## 3. Proposed AIF structure for the leaves

For each leaf: **(a) legitimate scheme, (b) exception/CQ, (c) `AIF_skosMappingType`**. Vocabulary
restricted to AIF-native tokens confirmed by existing usage.

### pk 844 — Sophisme d'association (Association fallacy / guilt by association, d3 anchor) ✅
- **desc_fr**: "Vous associez à tort des éléments entre eux en vous fondant sur des caractéristiques
  communes **sans rapport avec l'argument**."
- **Legitimate scheme**: `VerbalClassification_Inference` (AIF native) — grouping/classifying items
  together on the basis of a shared property.
- **Exception/CQ**: the shared property is **not relevant** to the invoked association — the
  classification is biased by an irrelevant trait. Native conflict: `BiasedClassification_Conflict`
  (borrowed from 833).
- **Proposal**: `ExceptionRef=VerbalClassification_Inference`, `DirectRef=BiasedClassification_Conflict`,
  `MappingType=skos:closeMatch` (a direct classification-level variant of 833's first Conflict node).

### pk 845 — Amalgame (Conflation) ✅
- **desc_fr**: "Vous associez à tort une personne ou une chose à une catégorie négative en vous
  fondant sur **un seul point commun**."
- **⚠ Semantic note**: the "negative category" framing is emotionally loaded but structurally
  secondary; the core is *classification on a single, hence unrepresentative, common point*.
- **Legitimate scheme**: `ArbitraryVerbalClassification_Inference` (AIF native, confirmed 3x in
  existing usage) — drawing a classification/distinction on an arbitrary (here, singular) basis.
- **Exception/CQ**: a single common point is an exceptional, non-representative basis for the
  invoked equivalence. Native conflict: `ExceptionSimilarityCase_Conflict` (borrowed from 833 — the
  case treated as similar is exceptional, not representative).
- **Proposal**: `ExceptionRef=ArbitraryVerbalClassification_Inference`,
  `DirectRef=ExceptionSimilarityCase_Conflict`, `MappingType=skos:closeMatch` (a direct variant of
  833's second Conflict node).

---

## 4. Cluster summary (proposed)

| pk | text_fr | Proposed ExceptionRef | Proposed DirectRef | MappingType | Honest? |
|----|---------|----------------------|-------------------|-------------|---------|
| 844 | Sophisme d'association | `VerbalClassification_Inference` | `BiasedClassification_Conflict` | `skos:closeMatch` | ✅ |
| 845 | Amalgame | `ArbitraryVerbalClassification_Inference` | `ExceptionSimilarityCase_Conflict` | `skos:closeMatch` | ✅ |

**2 leaves proposed, 2 fully-modeled, 0 FAIL-LOUD.** The cleanest cluster of the chantier. Both
leaves honestly split the borrowed anchor's two Conflict nodes (844 → biased classification, 845 →
exceptional similarity), each paired with the native classification scheme that fits its semantics.

---

## 5. Method notes (additions for the chantier, beyond PR-1/PR-2)

- **Anchor-split pattern**: when a borrowed anchor carries **two** Conflict nodes (833), a 2-leaf
  sub-sub can cleanly assign one Conflict node per leaf if the leaves' semantics differ along the
  same fault line (844 = irrelevant trait → biased classification; 845 = singular trait →
  exceptional similarity). This is more honest than forcing both leaves onto the same Conflict.
- **Emotional-loading is structurally secondary**: 845's "catégorie négative" is a valence feature,
  not a structural one — the AIF modeling targets the *classification* defect (single arbitrary
  point), not the negativity. Do not invent a "negative-valence" Conflict node.
- **Cluster cleanliness signal**: a 0-FAIL-LOUD cluster (this one) vs a high-FAIL-LOUD cluster
  (PR-2, 3/5) is an honest signal about the AIF ontology's coverage: it covers classification
  fallacies well, pure-comparison fallacies poorly. Useful for the eventual `Comparison_Inference`
  ontology-extension decision (PR-2 §7).

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not "100% leaves") | ✅ §4 (76→78 fully-modeled / 79→81 DirectRef-loose; subfamily 13/13 leaves addressed) |
| Per unmapped leaf: legitimate scheme + exception/CQ (not Latin alone) | ✅ §3 (2 leaves, scheme + CQ each, 0 FAIL-LOUD) |
| `AIF_skosMappingType` coherent | ✅ §4 (closeMatch for both — direct variants of 833's Conflict nodes) |
| Fail-loud when no honest scheme fits | N/A this cluster (both leaves have a native scheme) |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies PR-1 + PR-2 + this PR first, then CSV edits applied, then OWL regen (#133) — not in this PR |

### Coverage accounting (cumulative, chantier-wide)
| PR | Cluster | Leaves | Fully-modeled (DoD) | DirectRef-loose |
|----|---------|--------|---------------------|-----------------|
| baseline | — | — | 70 | 70 |
| PR-1 #699 | False analogy (839-843) | 4 | 74 (+4) | 74 (+4) |
| PR-2 #701 | Faulty comparison (834-838) | 5 | 76 (+2: 836, 838) | 79 (+5) |
| **PR-3 (this)** | **Association fallacy (844-845)** | **2** | **78 (+2)** | **81 (+2)** |
| **total** | **Fallacious comparison subfamily** | **13** | **78** | **81** |

The **Fallacious comparison subfamily is now fully addressed** (3/3 sub-subs). #498 chantier can
move to the next Misleading-language subfamily (see PR-3 secondaire / next dispatch).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

Per jsboige's enriched DoD (#498 last comment): decompose each fallacy into **I-nodes** (premises +
conclusion), an **RA-node** (the Walton scheme), and a **CA-node** (the Conflict Application, typed
undermine/undercut/rebut).

### pk 844 — Sophisme d'association → **undercut**
- **RA-node**: `VerbalClassification_Inference` — grouping items by a shared property.
- **I-nodes**: premise P1 "items A and B share property p"; premise P2 "p is classification-relevant
  to the invoked association"; conclusion C "A and B are associated/equivalent".
- **CA-node**: `BiasedClassification_Conflict`, applied as an **undercut** on the RA-node — the
  inference (that sharing p licenses the association) is defective because P2 is false (p is
  *without rapport* to the argument). The conclusion C may still hold; it is the *inference* that is
  defective. Classic undercut.
- **CQ that fails**: "is the shared property relevant to the invoked association?" — no.

### pk 845 — Amalgame → **undercut**
- **RA-node**: `ArbitraryVerbalClassification_Inference` — classifying an item into a category on an
  arbitrary (here, singular) basis.
- **I-nodes**: premise P "X shares a single common point p with category Y"; conclusion C "X ∈ Y".
- **CA-node**: `ExceptionSimilarityCase_Conflict`, applied as an **undercut** on the RA-node — the
  inference (that one common point licenses membership in Y) is defective because the case is
  exceptional/non-representative. Again the inference, not the conclusion, is the target.
- **CQ that fails**: "is one common point a representative basis for the classification?" — no.

### Cluster attack-type distribution
| pk | RA-node | CA-node | Attack type |
|----|---------|---------|-------------|
| 844 | `VerbalClassification_Inference` | `BiasedClassification_Conflict` | undercut |
| 845 | `ArbitraryVerbalClassification_Inference` | `ExceptionSimilarityCase_Conflict` | undercut |

**Both leaves are undercuts** — consistent with jsboige's note that "most fallacies live in the
undercut": the invoked association *could* be real, it is the *inference* (trait → association) that
is defective. No undermine (no false premise — the shared trait genuinely exists, it is just
insufficient) and no rebut (the conclusion is not directly denied).

### Representation note
As in PR-1/PR-2, the I-node/RA-node/CA-node decomposition is **recorded here but not serialized**
in the CSV — it would require new columns (`AIF_attackType`, `AIF_attackedNode`) and is the terminal
target of jsboige's comment. Three PRs now demonstrate the decomposition across all three cluster
shapes (in-sub-sub anchor, borrowed-anchor mixed, borrowed-anchor clean) and all FAIL-LOUD layers
documented (CA-missing PR-1 840, RA-missing PR-2 834/835/837, none here).

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No #674/#596/#666 merge, no régén launch.
- ✅ Proposition derived code=truth from the taxonomy CSV + AIF vocabulary confirmed by existing
  usage (no fabricated token — all 4 tokens here are reused from 833 / native classification schemes).
- ✅ Anchor-split pattern (844 → biased classification, 845 → exceptional similarity) documented, not
  forced.

Relates: dispatch `mkzt2v` (primary), #498 (chantier), PR-1 #699 (False analogy), PR-2 #701 (Faulty
comparison), #133/#130 (existing OWL), #499 (inverse: virtue = good tenor of a scheme), #192
(terminology), #458.
