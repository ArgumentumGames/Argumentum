# 2026-07-05 — #498 AIF chantier, PR-2 : cluster « Faulty comparison » (Fallacious comparison, suite)

**Scope**: second cluster of the #498 chantier — model the **Faulty comparison** sub-sub
(`Soussousfamille=Faulty comparison`, pk 834-838) of the Fallacious comparison subfamily. Continues
PR-1 (False analogy sub-sub, #699). **Proposition only — GATED, 0 write to prod CSV.** Triggered by
ai-01 dispatch `yj7u3j` (tertiary/idle: « avance #498 grappe suivante »).

**Repo reference**: master `dc02e847`. Issue: #498 (reformulated, GO jsboige 2026-06-17 verified).
Predecessor: PR-1 (#699, False analogy sub-sub).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. This doc proposes
> the AIF structure for the cluster's unmapped leaves, derived code=truth from the CSV + the AIF
> native vocabulary confirmed by existing usage (26 Conflict nodes / 36 Inference schemes across
> the 70 mapped rows). jsboige ratifies → worker applies the CSV edits in a follow-up PR (gated).

---

## TL;DR

The **Faulty comparison** sub-sub has **no in-sub-sub mapped anchor** — unlike PR-1's False analogy
(which had pk 839). It is therefore **anchored by the borrowed d2 root pk 833** "Comparaison
fallacieuse" (MAPPED, `skos:broadMatch`). 833 establishes a **different AIF pattern** from PR-1's
anchor: a **direct-conflict pattern** (DirectRef to `BiasedClassification_Conflict` +
`ExceptionSimilarityCase_Conflict`, ExceptionRef empty) rather than PR-1's exception pattern
(ExceptionRef to `Analogy_Inference`).

This PR proposes the AIF modeling for the **5 leaves** (834 d3 anchor + 835/836/837/838 d4). The
cluster splits cleanly along a **semantic fault line**:

- **2 classification leaves (honest, fully modeled)** — 836 + 838 are genuinely *classification*
  fallacies (overlapping categories / distinction without difference) and reuse the native
  `VerbalClassification_Inference` / `ArbitraryVerbalClassification_Inference` schemes.
- **3 comparison leaves (FAIL-LOUD on the scheme)** — 834/835/837 are *pure comparison* failures
  (abusive / incomplete / incoherent comparison) for which **no native AIF Inference scheme exists**
  (verified: the 36-scheme vocabulary has no `Comparison_Inference`). They take a DirectRef
  (`ExceptionSimilarityCase_Conflict`, borrowed from 833) but their ExceptionRef is documented as
  absent rather than fabricated.

**Net**: a higher FAIL-LOUD rate (3/5) than PR-1 (1/4) — an honest signal that the AIF ontology
covers analogy (PR-1) better than pure comparison (this PR).

---

## 1. Cluster state (code=truth)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Misleading language**, subfamily
**Fallacious comparison**, sub-sub **Faulty comparison**:

| pk | text_fr | depth | DirectRef | ExceptionRef | MappingType | State |
|----|---------|-------|-----------|--------------|-------------|-------|
| **833** | **Comparaison fallacieuse** (borrowed anchor) | d2 | `BiasedClassification_Conflict`, `ExceptionSimilarityCase_Conflict` | — | `skos:broadMatch` | **✅ MAPPED (d2 root, parent of this sub-sub)** |
| 834 | Comparaison abusive | d3 | — | — | — | unmapped (d3 anchor, **PR-2**) |
| 835 | Comparaison incomplète | d4 | — | — | — | unmapped (**PR-2**) |
| 836 | Classification non exclusive | d4 | — | — | — | unmapped (**PR-2**) |
| 837 | Comparaison incohérente | d4 | — | — | — | unmapped (**PR-2**) |
| 838 | Distinction sans différence | d4 | — | — | — | unmapped (**PR-2**) |

The borrowed anchor (833) is the **template**, but its pattern differs from PR-1's anchor (839):

- **839 (PR-1)** = exception pattern: a legitimate scheme (`Analogy_Inference`) is named in
  ExceptionRef, defeated by a violated CQ (`DifferencesUndermineSimilarity_Conflict`) in DirectRef.
- **833 (PR-2 anchor)** = direct-conflict pattern: no ExceptionRef (no legitimate scheme named);
  DirectRef carries two Conflict nodes directly. The fallacy *is* the conflict.

This PR honors both patterns: the comparison leaves (834/835/837) follow 833's direct-conflict
shape (DirectRef only), while the classification leaves (836/838) follow 839's exception shape
(named scheme + CQ) because a native scheme honestly fits.

---

## 2. The borrowed anchor (833, the template)

**Pattern**: direct-conflict (no ExceptionRef).

**Conflict nodes** (DirectRef): `BiasedClassification_Conflict` (the comparison biases the
classification) + `ExceptionSimilarityCase_Conflict` (the invoked similarity is an exceptional case,
not representative).

**Mapping type**: `skos:broadMatch` — "Comparaison fallacieuse" is the broad informal-logic name;
the two Conflict nodes are the specific AIF mechanisms.

**Why two Conflict nodes**: a fallacious comparison can fail because (a) the classification it
rests on is biased (`BiasedClassification_Conflict`) or (b) the case treated as similar is actually
exceptional (`ExceptionSimilarityCase_Conflict`). 833 carries both; the leaves specialize.

---

## 3. Proposed AIF structure for the leaves

For each leaf: **(a) legitimate scheme (or FAIL-LOUD), (b) exception/CQ or direct conflict,
(c) `AIF_skosMappingType`**. Vocabulary restricted to AIF-native tokens confirmed by existing usage.

### pk 834 — Comparaison abusive (Abusive comparison, d3 anchor)
- **desc_fr**: "Vous utilisez une comparaison excessive ou inappropriée pour appuyer votre argument."
- **Legitimate scheme**: ⚠ **FAIL LOUD** — no native AIF Inference scheme models "comparison" as a
  legitimate presumptive scheme. The 36-scheme vocabulary has no `Comparison_Inference`; the closest
  (`VerbalClassification_Inference`, `Example_Inference`, `Sign_Inference`) do not honestly capture
  *comparison* as the operative inference.
- **Direct conflict**: `ExceptionSimilarityCase_Conflict` (borrowed from 833 — the comparison treats
  an excessive/inappropriate case as representative).
- **Proposal**: `DirectRef=ExceptionSimilarityCase_Conflict`, `ExceptionRef=` *(absent — FAIL LOUD)*,
  `AIF_skosOther="No native AIF Inference scheme for non-faulty comparison; DirectRef-only
  (direct-conflict pattern, mirroring anchor 833)"`, `MappingType=skos:broadMatch`. Do **not**
  fabricate a `Comparison_Inference` token.

### pk 835 — Comparaison incomplète (Incomplete comparison)
- **desc_fr**: "Vous faites une comparaison en omettant des informations importantes, ce qui rend
  votre argument peu clair ou incorrect."
- **Legitimate scheme**: ⚠ **FAIL LOUD** — same gap as 834 (no native comparison scheme).
- **Direct conflict**: `ExceptionSimilarityCase_Conflict` (the comparison rests on an incomplete,
  hence unrepresentative, case).
- **Proposal**: `DirectRef=ExceptionSimilarityCase_Conflict`, `ExceptionRef=` *(absent — FAIL LOUD)*,
  `AIF_skosOther="No native AIF scheme for non-faulty comparison; DirectRef-only"`,
  `MappingType=skos:broadMatch`.

### pk 836 — Classification non exclusive (Non-exclusive classification) ✅ honest
- **desc_fr**: "Vous utilisez des catégories qui se chevauchent, faussant ainsi votre comparaison."
- **⚠ Semantic shift**: despite sitting in the comparison sub-sub, this leaf is explicitly a
  *classification* fallacy (overlapping categories). It honestly targets a classification scheme.
- **Legitimate scheme**: `VerbalClassification_Inference` (AIF native, confirmed in existing usage).
- **Exception/CQ**: `BiasedClassification_Conflict` (borrowed from 833 — the non-exclusive categories
  bias the classification).
- **Proposal**: `ExceptionRef=VerbalClassification_Inference`, `DirectRef=BiasedClassification_Conflict`,
  `MappingType=skos:closeMatch` (a direct classification-level variant of 833's first Conflict node).

### pk 837 — Comparaison incohérente (Incoherent comparison)
- **desc_fr**: "Vous comparez des choses en ne retenant que certains aspects, ce qui fausse la
  comparaison globale."
- **Legitimate scheme**: ⚠ **FAIL LOUD** — same gap (no native comparison scheme).
- **Direct conflict**: `ExceptionSimilarityCase_Conflict` (cherry-picked aspects → unrepresentative
  case).
- **Proposal**: `DirectRef=ExceptionSimilarityCase_Conflict`, `ExceptionRef=` *(absent — FAIL LOUD)*,
  `AIF_skosOther="No native AIF scheme for non-faulty comparison; DirectRef-only"`,
  `MappingType=skos:broadMatch`.

### pk 838 — Distinction sans différence (Distinction without difference) ✅ honest scheme-divergence
- **desc_fr**: "Vous distinguez deux éléments qui sont en réalité semblables ou qui ne présentent
  aucune différence significative."
- **⚠ Mirror fallacy** (moved here from PR-1's earlier draft — see PR-1 §1 boundary correction):
  claiming a distinction where none exists. It is a *classification* failure, not an analogy failure.
- **Legitimate scheme**: `ArbitraryVerbalClassification_Inference` (AIF native, confirmed 3x in
  existing usage).
- **Exception/CQ**: `PropertyNotExistant_Conflict` (the distinguishing property does not obtain —
  confirmed 3x in existing usage).
- **Proposal**: `ExceptionRef=ArbitraryVerbalClassification_Inference`,
  `DirectRef=PropertyNotExistant_Conflict`, `MappingType=skos:broadMatch`. **This is the honest
  scheme** — 838 is a classification fallacy, and it sits in the comparison subfamily only because
  the distinction it attacks is comparison-adjacent.

---

## 4. Cluster summary (proposed)

| pk | text_fr | Proposed ExceptionRef | Proposed DirectRef | MappingType | Honest? |
|----|---------|----------------------|-------------------|-------------|---------|
| 834 | Comparaison abusive | *(absent — FAIL LOUD)* | `ExceptionSimilarityCase_Conflict` | `skos:broadMatch` | ⚠ FAIL LOUD (no native scheme) |
| 835 | Comparaison incomplète | *(absent — FAIL LOUD)* | `ExceptionSimilarityCase_Conflict` | `skos:broadMatch` | ⚠ FAIL LOUD (no native scheme) |
| 836 | Classification non exclusive | `VerbalClassification_Inference` | `BiasedClassification_Conflict` | `skos:closeMatch` | ✅ |
| 837 | Comparaison incohérente | *(absent — FAIL LOUD)* | `ExceptionSimilarityCase_Conflict` | `skos:broadMatch` | ⚠ FAIL LOUD (no native scheme) |
| 838 | Distinction sans différence | `ArbitraryVerbalClassification_Inference` | `PropertyNotExistant_Conflict` | `skos:broadMatch` | ✅ (scheme-divergence, honest) |

**5 leaves proposed: 2 fully-modeled (836, 838) + 3 FAIL-LOUD (834/835/837).** The 3 FAIL-LOUD take
a DirectRef (direct-conflict pattern, mirroring anchor 833) but document the absence of a native
comparison Inference scheme in `AIF_skosOther` rather than fabricating one.

**Honest accounting**:
- If "mapped" = has a DirectRef (the CSV's looser criterion, which counts 833 itself as mapped):
  this PR adds 5 → coverage 74→79.
- If "mapped" = satisfies #498 DoD (legitimate scheme + CQ, not DirectRef-only): this PR adds 2
  fully-modeled + 3 partial (documented gap) → coverage 74→76 fully-modeled, with 3 honestly
  deferred to the `AIF_skosOther` track.

Both counts are reported; the DoD-honest number (76) is the one that matters for #498.

---

## 5. Method notes (additions for the chantier, beyond PR-1)

- **Borrowed-anchor cluster shape**: when a sub-sub has no in-sub-sub mapped anchor (Faulty
  comparison), it borrows the nearest mapped ancestor (d2 root 833). The borrowed anchor's
  **pattern** (direct-conflict vs exception) dictates the leaves' shape. This is the second cluster
  shape of the chantier (PR-1 = in-sub-sub anchor + exception pattern; PR-2 = borrowed anchor +
  mixed pattern).
- **Direct-conflict vs exception pattern**: an anchor with ExceptionRef empty (833) models the
  fallacy *as* the conflict; an anchor with a named scheme in ExceptionRef (839) models it *as a
  defeater of* the scheme. Leaves should follow their anchor's pattern where the semantics allow,
  and shift to the exception pattern (named scheme + CQ) only when a native scheme honestly fits
  (836, 838).
- **FAIL-LOUD on scheme, not on conflict**: where a native Inference scheme is absent (pure
  comparison), still assign the honest Conflict node in DirectRef (don't fail loud on both) — the
  conflict is real, only the scheme is missing. Document the missing scheme in `AIF_skosOther`.
- **Semantic-shift leaf**: a leaf's sub-sub placement is not always its true scheme (836 is a
  classification fallacy in a comparison sub-sub; 838 too). Reuse the right scheme when the desc_fr
  clearly indicates it; don't force the sub-sub's nominal theme.
- **Native-vocabulary discipline**: restrict proposals to AIF tokens confirmed by existing usage
  (the 26 Conflict nodes / 36 Inference schemes observed in the 70 mapped rows). Never fabricate a
  `*_Conflict` or `*_Inference` token — FAIL LOUD instead.

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not "100% leaves") | ✅ §4 (74→76 fully-modeled / 79 DirectRef-loose, cluster-level) |
| Per unmapped leaf: legitimate scheme + exception/CQ (not Latin alone) | ✅ §3 — 2 leaves fully (836, 838); 3 leaves FAIL-LOUD on scheme (834/835/837), DirectRef-only documented |
| `AIF_skosMappingType` coherent (broadMatch when looser) | ✅ §4 (broadMatch for DirectRef-only + scheme-divergence; closeMatch for the direct variant 836) |
| Fail-loud when no honest scheme fits | ✅ §3 pk 834/835/837 (no native comparison scheme — documented, not fabricated) |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies PR-1 + this PR first, then CSV edits applied, then OWL regen (#133) — not in this PR |

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

The last jsboige comment on #498 enriches the DoD: modeling a fallacy "AIF-style" is not just
`scheme + exception-name` — it **decomposes the argument** into **I-nodes** (premises + conclusion),
an **RA-node** (the inference principle = the Walton scheme, a first-class attackable object), and a
**CA-node** (the Conflict Application, typed by attack: **undermine** a premise / **undercut** the
RA-node / **rebut** the conclusion). Most fallacies live in the **undercut**. This section maps the
2 fully-modeled leaves to that decomposition, and documents where the 3 FAIL-LOUD leaves fall short.

### pk 836 — Classification non exclusive → **undercut**
- **RA-node** (inference principle): `VerbalClassification_Inference` — classifying an item into a
  category on the basis of its properties.
- **I-nodes**: premise P1 "categories X and Y are mutually exclusive"; premise P2 "item A has the
  defining property of X"; conclusion C "A ∈ X (not Y)".
- **CA-node**: `BiasedClassification_Conflict`, applied as an **undercut** on the RA-node — the
  inference (that A can be *cleanly* classified) is defective because P1 is false (the categories
  overlap). The conclusion C may still hold; it is the *inference* that is defective, not the
  conclusion. Classic undercut.
- **CQ that fails**: "are the categories mutually exclusive?" — no (they overlap).

### pk 838 — Distinction sans différence → **undermine**
- **RA-node**: `ArbitraryVerbalClassification_Inference` — drawing a distinction between two classes
  on the basis of a distinguishing property.
- **I-nodes**: premise P1 "A has property p that B lacks"; premise P2 "p is classification-relevant";
  conclusion C "A and B are in different classes".
- **CA-node**: `PropertyNotExistant_Conflict`, applied as an **undermine** on P1 — the premise "A
  has p that B lacks" is false (p does not obtain; A and B are indistinguishable on p). Unlike 836,
  the attack lands on a *premise* (I-node), not the inference itself.
- **CQ that fails**: "does the distinguishing property p actually obtain?" — no.

### pk 834/835/837 — FAIL-LOUD at the **RA-node level**
- These leaves cannot be decomposed because the **RA-node is missing**: there is no native AIF
  Inference scheme for "comparison" (verified — the 36-scheme vocabulary has no
  `Comparison_Inference`). Without an RA-node there is nothing to undercut and no inference to
  attack; only a dangling CA-node (`ExceptionSimilarityCase_Conflict`) survives.
- This is a deeper gap than a missing CQ: it is a missing *scheme*. The enriched DoD makes the gap
  sharper — FAIL-LOUD here is not "no CQ" (PR-1's 840) but "no RA", which is structurally prior.
- **Terminal remedy (out of scope, jsboige decision)**: either (a) extend the AIF vocabulary with a
  `Comparison_Inference` scheme (ontology change, requires ratification), or (b) re-express these
  leaves as `Example_Inference` / `Sign_Inference` defeaters (semantic stretch, lower fidelity).
  Both are gated; this PR documents the gap, does not pick.

### Representation note
This PR (and PR-1) record the modeling in the **existing CSV columns** (`AIF_skosDirectRef` =
CA-node conflict, `AIF_skosExceptionRef` = RA-node scheme), consistent with the 70 already-mapped
rows. The enriched I-node/RA-node/CA-node decomposition above is **not yet serialized** — it would
require new CSV columns (e.g. `AIF_attackType`, `AIF_attackedNode`) and is the terminal target
flagged in jsboige's comment. Recording the decomposition here (in the proposition doc) lets jsboige
ratify the CA-node typing *before* any CSV schema evolution. PR-1's leaves can be back-filled with
the same decomposition in a follow-up once the column schema is agreed.

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No #674/#596 merge, no régén launch.
- ✅ Proposition derived code=truth from the taxonomy CSV + AIF vocabulary confirmed by existing
  usage (no fabricated token — `Comparison_Inference` absent → FAIL LOUD, not invented).
- ✅ Honest scheme-divergence on 836/838 (classification leaves in a comparison sub-sub); FAIL-LOUD
  on the scheme for the 3 pure-comparison leaves, with DirectRef preserved.

Relates: dispatch `yj7u3j` (tertiary/idle), #498 (chantier), PR-1 #699 (False analogy sub-sub,
predecessor), #133/#130 (existing OWL), #499 (inverse: virtue = good tenor of a scheme), #192
(terminology), #458.
