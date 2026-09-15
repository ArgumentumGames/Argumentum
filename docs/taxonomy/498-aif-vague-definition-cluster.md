# 2026-07-05 — #498 AIF chantier, PR-4 : cluster « Vague definition » (Inexact definition, nouvelle subfamily)

**Scope**: fourth cluster of the #498 chantier — first cluster of a **new subfamily**
(**Inexact definition**, Misleading language family). Models the **Vague definition** sub-sub
(pk 800 anchor + 801/802/803 leaves). **Proposition only — GATED, 0 write to prod CSV.** Triggered
by ai-01 dispatch `mkzt2v` (secondary: « prochaine subfamily de Misleading language »).

**Repo reference**: master `70bd1605`. Issue: #498. Predecessors: PR-1 #699 (False analogy),
PR-2 #701 (Faulty comparison), PR-3 #703 (Association fallacy) — Fallacious comparison subfamily
complete.

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies →
> worker applies the CSV edits in a follow-up PR (gated).

---

## TL;DR

The **Vague definition** sub-sub has an **in-sub-sub mapped anchor** — pk 800 (d3 "Acception vague",
MAPPED `VagueVerbalClassification_Inference`, directRef-only, `skos:closeMatch`) — and **3 unmapped
leaves** (pk 801, 802, 803). This is the **same cluster shape as PR-1** (in-sub-sub anchor + leaves,
exception pattern), but with a **new anchor scheme**: `VagueVerbalClassification_Inference` (AIF
native, confirmed in existing usage), the scheme for vague/imprecise verbal classification.

All 3 leaves are **vagueness-flavored** — imprecise, obscure, undefined, or contested terms used to
keep the argument slippery. They honestly reuse the anchor's scheme and specialize along the
*vagueness mechanism*: over-complex (801), absent (802), intrinsically disputed (803).

**3 leaves proposed, all fully-modeled reusing `VagueVerbalClassification_Inference`, 0 FAIL-LOUD.**
This PR opens the **Inexact definition** subfamily (next sub-subs: Arbitrary definition PR-5,
Inconsistent definition PR-6).

---

## 1. Cluster state (code=truth)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Misleading language**, subfamily
**Inexact definition**, sub-sub **Vague definition**:

| pk | text_fr | depth | DirectRef | ExceptionRef | MappingType | State |
|----|---------|-------|-----------|--------------|-------------|-------|
| **800** | **Acception vague** (d3 anchor) | d3 | `VagueVerbalClassification_Inference` | — | `skos:closeMatch` | **✅ MAPPED (PR-4 anchor)** |
| 801 | Défaut d'élucidation | d4 | — | — | — | unmapped (**PR-4**) |
| 802 | Indéfinissabilité | d4 | — | — | — | unmapped (**PR-4**) |
| 803 | Concept essentiellement contesté | d4 | — | — | — | unmapped (**PR-4**) |

The mapped anchor (800) is the **template**: a vague definition = a `VagueVerbalClassification_Inference`
(a verbal classification left deliberately imprecise). Note 800 uses the **direct-conflict pattern**
(the scheme is in DirectRef, not ExceptionRef — the fallacy *is* the vague classification itself,
there is no "legitimate scheme defeated" framing). The 3 leaves follow the same direct-conflict
shape, varying the `MappingType` by how tightly they specialize the anchor.

---

## 2. The anchor model (800, the template)

**Scheme**: `VagueVerbalClassification_Inference` (AIF native, confirmed 2x in existing usage) — a
verbal classification (grouping/labeling items via a term) where the term is left vague/imprecise,
allowing the arguer to shift its extension mid-discussion.

**desc_fr** (800): "Vous utilisez des termes imprécis pour rester évasif et pouvoir modifier votre
argumentation en cours de route."

**Pattern**: direct-conflict — the scheme itself names the defect (the classification *is* vague).
This differs from PR-1's anchor 839 (exception pattern: a legitimate `Analogy_Inference` defeated by
a CQ). Here there is no "defeated legitimate scheme"; the vagueness is the whole point.

---

## 3. Proposed AIF structure for the leaves

For each leaf: reuse the anchor's scheme `VagueVerbalClassification_Inference`, vary the
`MappingType` by the leaf's specialization. Vocabulary restricted to AIF-native tokens.

### pk 801 — Défaut d'élucidation (Failure to elucidate) ✅
- **desc_fr**: "Vous employez des définitions trop complexes qui obscurcissent plutôt qu'elles
  n'éclairent vos propos."
- **Mechanism**: vagueness by **obscurantism** — the term is technically defined but the definition
  is so complex it fails to elucidate. A specialized variant of 800 (imprecision via over-complexity
  rather than plain underspecification).
- **Proposal**: `DirectRef=VagueVerbalClassification_Inference`, `ExceptionRef=` *(direct-conflict
  pattern, mirroring 800)*, `MappingType=skos:narrowMatch` (a narrower case: vague *via complexity*,
  not plain vagueness).

### pk 802 — Indéfinissabilité (Undefinability) ✅
- **desc_fr**: "Votre argument repose sur un concept crucial qui n'est pas clairement défini."
- **Mechanism**: vagueness by **absence** — the crucial concept is simply undefined. A broader
  variant of 800 (the vagueness is total — no definition at all — rather than a slippery imprecise
  one).
- **Proposal**: `DirectRef=VagueVerbalClassification_Inference`, `ExceptionRef=`,
  `MappingType=skos:broadMatch` (a broader case: any undefined concept, not just deliberately
  slippery terms).

### pk 803 — Concept essentiellement contesté (Essentially contested concept) ✅
- **desc_fr**: "Vous utilisez un terme ou un concept qui admet plusieurs interprétations et dont le
  sens est intrinsèquement disputé, ce qui entraîne des malentendus ou des débats sans fin."
- **⚠ Semantic note**: this is the philosophically deepest of the three — the contestability is
  *intrinsic* (Gallie's essentially-contested concepts), not a defect of the speaker. Structurally
  though, the AIF mechanism is the same: a verbal classification whose extension is not fixed.
- **Mechanism**: vagueness by **intrinsic polysemy** — the concept's meaning is genuinely disputed,
  so any classification using it is vague-by-nature. A direct variant of 800.
- **Proposal**: `DirectRef=VagueVerbalClassification_Inference`, `ExceptionRef=`,
  `MappingType=skos:closeMatch` (a direct variant: vague because intrinsically contested, the
  canonical essentially-contested-concept case).

---

## 4. Cluster summary (proposed)

| pk | text_fr | Proposed ExceptionRef | Proposed DirectRef | MappingType | Honest? |
|----|---------|----------------------|-------------------|-------------|---------|
| 800 | Acception vague | *(absent — direct-conflict)* | `VagueVerbalClassification_Inference` | `skos:closeMatch` | ✅ (existing anchor) |
| 801 | Défaut d'élucidation | *(absent)* | `VagueVerbalClassification_Inference` | `skos:narrowMatch` | ✅ |
| 802 | Indéfinissabilité | *(absent)* | `VagueVerbalClassification_Inference` | `skos:broadMatch` | ✅ |
| 803 | Concept essentiellement contesté | *(absent)* | `VagueVerbalClassification_Inference` | `skos:closeMatch` | ✅ |

**3 leaves proposed, 3 fully-modeled, 0 FAIL-LOUD.** All reuse the anchor's
`VagueVerbalClassification_Inference` (direct-conflict pattern). The only variation is the
`MappingType`, encoding the vagueness mechanism: 801 narrow (complexity), 802 broad (total absence),
803 close (intrinsic contestability).

---

## 5. Method notes (additions for the chantier)

- **Direct-conflict cluster (all-leaves)**: when the anchor itself uses the direct-conflict pattern
  (scheme in DirectRef, no ExceptionRef) and every leaf shares that scheme, the cluster is uniform —
  all leaves take DirectRef-only and vary only the MappingType. This is structurally simpler than
  PR-1 (exception pattern, varied schemes) or PR-2 (mixed, FAIL-LOUD). A useful "clean baseline"
  shape for the chantier.
- **Vagueness-mechanism taxonomy**: within a single scheme, leaves can specialize along *how* the
  vagueness arises (complexity / absence / contestability). The MappingType encodes this:
  `narrowMatch` = a specific mechanism, `broadMatch` = a broader phenomenon, `closeMatch` = a direct
  sibling case. This gives the eventual consumer (the EPITA harness) a graded notion of vagueness,
  not a binary one.
- **Intrinsic-contestability is structural, not defective**: pk 803's contestability is a property
  of the concept, not a rhetorical move by the speaker — yet the AIF mechanism (vague verbal
  classification) still applies because any classification using such a concept is vague. Modeling
  it as a `VagueVerbalClassification_Inference` (rather than inventing an "intrinsically-contested"
  Conflict node) is the honest reuse; the philosophical depth is captured in the desc, not the
  token.

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not "100% leaves") | ✅ §4 (78→81 fully-modeled / 81→84 DirectRef-loose; new subfamily opened) |
| Per unmapped leaf: legitimate scheme + exception/CQ (not Latin alone) | ✅ §3 (3 leaves, scheme + mechanism each, 0 FAIL-LOUD) |
| `AIF_skosMappingType` coherent | ✅ §4 (narrow/broad/close encode the vagueness mechanism) |
| Fail-loud when no honest scheme fits | N/A this cluster (all leaves reuse the anchor's native scheme) |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies, then CSV edits, then OWL regen (#133) — not in this PR |

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### pk 801 — Défaut d'élucidation → **undercut**
- **RA-node**: `VagueVerbalClassification_Inference` — classifying via a term.
- **I-nodes**: premise P "term T is defined (by a complex definition)"; conclusion C "items labeled
  T are classified".
- **CA-node**: the inference (that the complex definition licenses classification) is **undercut** —
  the definition is so complex it fails to fix T's extension; the classification is effectively
  vague. The conclusion C may hold; the *inference* is defective.

### pk 802 — Indéfinissabilité → **undercut**
- **RA-node**: `VagueVerbalClassification_Inference`.
- **I-nodes**: premise P "T is a meaningful concept (used in the argument)"; conclusion C "items can
  be classified via T".
- **CA-node**: **undercut** — T is undefined, so the classification has no fixed extension. Again
  the inference, not the conclusion, is defective (T *might* still pick out a real category; we
  just cannot tell).

### pk 803 — Concept essentiellement contesté → **undercut**
- **RA-node**: `VagueVerbalClassification_Inference`.
- **I-nodes**: premise P "T has a single determinate meaning"; conclusion C "items classified via T
  are grouped unambiguously".
- **CA-node**: **undercut** — P is false (T is intrinsically polysemous), so the classification has
  no single extension. The inference (that T licenses unambiguous grouping) is defective.

### Cluster attack-type distribution
| pk | RA-node | CA-node | Attack type |
|----|---------|---------|-------------|
| 801 | `VagueVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut |
| 802 | `VagueVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut |
| 803 | `VagueVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut |

**All 3 leaves are undercuts.** Note on the direct-conflict pattern: here the "CA-node" *is* the
RA-node scheme itself (the vague classification is both the inference and its own defect) — a
degenerate case of undercut where the attack and the inference coincide. This is consistent with the
direct-conflict anchor pattern (§2) and is the honest representation: vagueness fallacies do not
defeat a separate legitimate scheme, they *are* a defective scheme.

### Representation note
As in PR-1/PR-2/PR-3, the I-node/RA-node/CA-node decomposition is **recorded here but not
serialized** — it would require new CSV columns (`AIF_attackType`, `AIF_attackedNode`) and is the
terminal target of jsboige's comment. Four PRs now demonstrate the decomposition across all cluster
shapes and both patterns (exception + direct-conflict).

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No #674/#596/#666 merge, no régén launch.
- ✅ Proposition derived code=truth from the taxonomy CSV + AIF vocabulary confirmed by existing
  usage (the only token proposed — `VagueVerbalClassification_Inference` — is reused from the anchor,
  no fabrication).
- ✅ Direct-conflict pattern documented; vagueness-mechanism encoded in MappingType, not in
  fabricated tokens.

Relates: dispatch `mkzt2v` (secondary), #498 (chantier), PR-1 #699 (False analogy), PR-2 #701
(Faulty comparison), PR-3 #703 (Association fallacy), #133/#130 (existing OWL), #499 (inverse),
#192 (terminology), #458.
