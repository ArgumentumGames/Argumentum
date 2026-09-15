# 2026-07-05 — #498 AIF chantier, PR-6 : cluster « Inconsistent definition » (Inexact definition, suite)

**Scope**: sixth cluster of the #498 chantier — model the **Inconsistent definition** sub-sub
(`Soussousfamille=Inconsistent definition`, pk 826-832) of the Inexact definition subfamily. Continues
PR-4 (Vague definition #705). **Proposition only — GATED, 0 write to prod CSV.** Triggered by ai-01
dispatch `mkzt2v` (tertiary: « continue en autonomie sur les clusters suivants »). Borrow-root shape
— **independent of the I/RA/CA serialization decision** (no schema dependency).

**Repo reference**: master `70bd1605`. Issue: #498. Predecessors: PR-1 #699, PR-2 #701, PR-3 #703
(Fallacious comparison complete), PR-4 #705 (Vague definition).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies →
> worker applies the CSV edits in a follow-up PR (gated).

---

## TL;DR

The **Inconsistent definition** sub-sub has **no in-sub-sub mapped anchor** → it borrows the d2 root
**pk 799** ("Définition biaisée", MAPPED, exception pattern: `VerbalClassification_Inference` defeated
by `BiasedClassification_Conflict` + `ArbitraryVerbalClassification_Inference`, `skos:broadMatch`).
The sub-sub has **7 leaves** (pk 826-832, depth 3-5) — the **largest cluster of the chantier** so far
(PR-2 Faulty comparison had 5).

The cluster is **semantically heterogeneous** — it spans contradiction (826/827), stolen-concept
inconsistency (828), circularity (829), lexical-dependency definition (830/831), and a philosophically
vague meta-fallacy (832). This heterogeneity is honest: the leaves do not all fit one scheme, so they
split across **three patterns** (exception / direct-conflict / FAIL-LOUD), reusing native schemes
(`VerbalClassification_Inference`, `InconsistentCommitment_Inference`, `Logical_Conflict`,
`VagueVerbalClassification_Inference`).

**7 leaves proposed: 5 fully-modeled + 2 FAIL-LOUD** (829 circularity CA-missing, 832 vague RA-missing).
The largest and most varied cluster — demonstrates the méthode holds under heterogeneity.

---

## 1. Cluster state (code=truth)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Misleading language**, subfamily
**Inexact definition**, sub-sub **Inconsistent definition**:

| pk | text_fr | depth | DirectRef | ExceptionRef | MappingType | State |
|----|---------|-------|-----------|--------------|-------------|-------|
| **799** | **Définition biaisée** (borrowed anchor) | d2 | `BiasedClassification_Conflict`, `ArbitraryVerbalClassification_Inference` | `VerbalClassification_Inference` | `skos:broadMatch` | **✅ MAPPED (d2 root)** |
| 826 | Définition incohérente | d3 | — | — | — | unmapped (d3 anchor, **PR-6**) |
| 827 | Conditions conflictuelles | d4 | — | — | — | unmapped (**PR-6**) |
| 828 | Concept volé | d5 | — | — | — | unmapped (**PR-6**) |
| 829 | Définition circulaire | d4 | — | — | — | unmapped (**PR-6**) |
| 830 | Argument du dictionnaire | d5 | — | — | — | unmapped (**PR-6**) |
| 831 | Sophisme définiste | d5 | — | — | — | unmapped (**PR-6**) |
| 832 | Sophisme philosophique | d4 | — | — | — | unmapped (**PR-6**) |

The borrowed anchor (799) uses the **exception pattern** (a legitimate `VerbalClassification_Inference`
defeated by `BiasedClassification_Conflict` + `ArbitraryVerbalClassification_Inference`). The leaves
follow whichever pattern honestly fits their semantics.

---

## 2. The borrowed anchor (799, recap)

**Pattern**: exception — a legitimate `VerbalClassification_Inference` (classifying items via a
defined term) defeated because the definition is biased/arbitrary.

**desc_fr** (799): "Vous définissez les termes de façon à favoriser votre argument, en écartant leur
sens établi."

The Inconsistent-definition leaves inherit the *verbal-classification* framing but specialize the
defect: contradiction (826/827), inconsistency (828), circularity (829), lexical rigidity (830),
recursive vagueness (831), meta-level vagueness (832).

---

## 3. Proposed AIF structure for the leaves

For each leaf: **(a) legitimate scheme (or FAIL-LOUD), (b) exception/CQ or direct conflict,
(c) `AIF_skosMappingType`**. Vocabulary restricted to AIF-native tokens confirmed by existing usage.

### pk 826 — Définition incohérente (Inconsistent definition, d3 anchor) ✅
- **desc_fr**: "Vous donnez aux termes que vous utilisez des définitions contradictoires ou
  incohérentes."
- **Legitimate scheme**: `VerbalClassification_Inference` (native, from anchor 799).
- **Exception/CQ**: the definitions assigned to the term are mutually contradictory → the
  classification has no determinate extension. Native conflict: `Logical_Conflict` (confirmed 1x in
  existing usage — captures direct logical contradiction).
- **Proposal**: `ExceptionRef=VerbalClassification_Inference`, `DirectRef=Logical_Conflict`,
  `MappingType=skos:closeMatch` (a direct variant of 799 where the bias is internal contradiction).

### pk 827 — Conditions conflictuelles (Conflicting conditions) ✅
- **desc_fr**: "Vous présentez un argument qui se contredit lui-même et ne peut donc pas être valide."
- **⚠ Direct-conflict pattern**: the defect is the self-contradiction itself — there is no separate
  "legitimate scheme" being defeated; the inference is invalid by internal contradiction.
- **Legitimate scheme**: *(none named — direct-conflict)*.
- **Direct conflict**: `Logical_Conflict` (the argument's conditions conflict).
- **Proposal**: `DirectRef=Logical_Conflict`, `ExceptionRef=`, `MappingType=skos:broadMatch` (a broad
  self-contradiction fallacy, the conflict is the whole defect).

### pk 828 — Concept volé (Stolen concept) ✅
- **desc_fr**: "Vous niez un concept tout en continuant à l'utiliser dans votre raisonnement."
- **Legitimate scheme**: `InconsistentCommitment_Inference` (native, confirmed 1x in existing usage) —
  an inference that is inconsistent with the arguer's own commitments (here, denying a concept while
  relying on it).
- **Pattern**: direct-conflict — the inconsistency *is* the scheme (the inference is performative
  inconsistency).
- **Proposal**: `DirectRef=InconsistentCommitment_Inference`, `ExceptionRef=`,
  `MappingType=skos:closeMatch` (a direct variant: stolen-concept = performative inconsistency).

### pk 829 — Définition circulaire (Circular definition) ⚠ FAIL-LOUD
- **desc_fr**: "Vous définissez un terme au moyen de la notion même qu'il représente et que vous
  prétendez définir."
- **⚠ FAIL-LOUD at the CA-node level** (same shape as PR-1's 840): circularity in definition would be
  an **undercut** (the definition presupposes what it establishes), but **no native AIF Conflict node
  captures circularity/question-begging**. `Logical_Conflict` does not fit (circularity ≠
  contradiction; the premises are mutually consistent, just non-grounded).
- **Proposal**: `DirectRef=` *(absent — FAIL LOUD)*, `ExceptionRef=VerbalClassification_Inference`,
  `AIF_skosOther="Circularity: definition presupposes the notion it defines (no native AIF Conflict
  node — Logical_Conflict does not fit, circularity is not contradiction)"`, `MappingType=skos:closeMatch`.
  Do **not** fabricate a `Circularity_Conflict` token.

### pk 830 — Argument du dictionnaire (Lexical definition / appeal to definition) ✅
- **desc_fr**: "Vous affirmez que la réalité décrite par un terme dépend strictement de sa définition
  dans le dictionnaire."
- **Legitimate scheme**: `VerbalClassification_Inference` (the term's lexical definition drives the
  classification).
- **Exception/CQ**: the lexical definition is **arbitrary** for the argument's purpose — treating
  dictionary meaning as binding reality is an arbitrary verbal classification. Native conflict:
  `ArbitraryVerbalClassification_Inference` (borrowed from anchor 799).
- **Proposal**: `ExceptionRef=VerbalClassification_Inference`,
  `DirectRef=ArbitraryVerbalClassification_Inference`, `MappingType=skos:narrowMatch` (a narrow case:
  arbitrariness *specifically from lexical rigidity*, narrower than 799's general bias).

### pk 831 — Sophisme définiste (Definist fallacy) ✅
- **desc_fr**: "Vous définissez un terme en utilisant d'autres termes qui sont tout aussi indéfinis."
- **⚠ Semantic link to PR-4**: this is the recursive cousin of PR-4's 802 (Indéfinissabilité) — the
  definition is *non-vacuous in form* but *vacuous in content* (defined via equally-undefined terms).
  It honestly reuses PR-4's anchor scheme.
- **Legitimate scheme**: `VagueVerbalClassification_Inference` (PR-4 anchor scheme, native).
- **Pattern**: direct-conflict (the vagueness *is* the scheme — the definition is recursively vague).
- **Proposal**: `DirectRef=VagueVerbalClassification_Inference`, `ExceptionRef=`,
  `MappingType=skos:narrowMatch` (a narrow case: vagueness *via recursive undefinition*, narrower
  than 802's plain absence).

### pk 832 — Sophisme philosophique (Philosophical fallacy) ⚠ FAIL-LOUD
- **desc_fr**: "Vous argumentez d'une manière qui soulève des problèmes quant à l'application de la
  logique à la connaissance."
- **⚠ FAIL-LOUD at the RA-node level**: the desc_fr is too vague to map to a specific AIF scheme —
  "problèmes quant à l'application de la logique à la connaissance" does not designate a determinate
  inference scheme or conflict. No native token honestly fits; forcing one would be fabrication.
- **Proposal**: `DirectRef=`, `ExceptionRef=`,
  `AIF_skosOther="Vague meta-fallacy (application of logic to knowledge) — no determinate AIF scheme;
  deferred pending jsboige clarification of intended scope"`, `MappingType=` *(none — unmappable as
  stated)*. Do **not** force a token.

---

## 4. Cluster summary (proposed)

| pk | text_fr | Proposed ExceptionRef | Proposed DirectRef | MappingType | Honest? |
|----|---------|----------------------|-------------------|-------------|---------|
| 826 | Définition incohérente | `VerbalClassification_Inference` | `Logical_Conflict` | `skos:closeMatch` | ✅ |
| 827 | Conditions conflictuelles | *(absent)* | `Logical_Conflict` | `skos:broadMatch` | ✅ |
| 828 | Concept volé | *(absent)* | `InconsistentCommitment_Inference` | `skos:closeMatch` | ✅ |
| 829 | Définition circulaire | `VerbalClassification_Inference` | *(absent — FAIL LOUD)* | `skos:closeMatch` | ⚠ FAIL LOUD (no circularity CQ) |
| 830 | Argument du dictionnaire | `VerbalClassification_Inference` | `ArbitraryVerbalClassification_Inference` | `skos:narrowMatch` | ✅ |
| 831 | Sophisme définiste | *(absent)* | `VagueVerbalClassification_Inference` | `skos:narrowMatch` | ✅ |
| 832 | Sophisme philosophique | *(absent)* | *(absent — FAIL LOUD)* | *(none)* | ⚠ FAIL LOUD (vague meta-fallacy) |

**7 leaves proposed: 5 fully-modeled (826, 827, 828, 830, 831) + 2 FAIL-LOUD (829, 832).** The cluster
spans **three patterns**: exception (826, 830), direct-conflict (827, 828, 831), FAIL-LOUD (829, 832).
It also crosses into a **neighbor cluster's scheme** (831 reuses PR-4's `VagueVerbalClassification_Inference`)
— an honest cross-reference, documented.

---

## 5. Method notes (additions for the chantier)

- **Heterogeneous cluster (multi-pattern)**: when a sub-sub's leaves genuinely span distinct defects
  (contradiction / inconsistency / circularity / lexical rigidity / vagueness), the modeling splits
  across patterns rather than forcing one. This is more honest than a uniform mapping and is the
  expected shape for semantically-broad sub-subs (Inconsistent definition is the broadest so far).
- **FAIL-LOUD layer reuse across chantier**: this cluster adds one CA-missing case (829, like PR-1
  840 — circularity) and one RA-missing case (832, like PR-2 834/835/837 — no determinate scheme).
  The chantier now has FAIL-LOUD cases in **3 of 6 PR** (PR-1, PR-2, PR-6) — circularity is a
  recurring AIF gap worth flagging for the eventual ontology-extension decision.
- **Cross-cluster scheme reuse** (831 → PR-4 scheme): a leaf may honestly fit a *neighbor* cluster's
  anchor scheme better than its own sub-sub's borrow-root. Reuse it and document the link (§3 831) —
  do not force the borrow-root's scheme where a closer native scheme exists.
- **Vague-meta-fallacy deferral** (832): when a desc_fr is too indeterminate to map (no determinate
  scheme *or* conflict), FAIL-LOUD with a `pending jsboige clarification` note rather than guessing.
  This is distinct from "no native token exists" (829) — 832 is "the fallacy itself is underspecified".

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not "100% leaves") | ✅ §4 (81→86 fully-modeled / 84→89 DirectRef-loose; subfamily 2/3 sub-subs) |
| Per unmapped leaf: legitimate scheme + exception/CQ (not Latin alone) | ✅ §3 — 5 leaves fully-modeled; 2 FAIL-LOUD (829 no circularity CQ, 832 vague meta-fallacy) |
| `AIF_skosMappingType` coherent | ✅ §4 (varied: close/broad/narrow per leaf semantics) |
| Fail-loud when no honest scheme fits | ✅ §3 pk 829 (circularity) + 832 (vague meta) — documented, not fabricated |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies, then CSV edits, then OWL regen (#133) |

### Coverage accounting (cumulative, chantier-wide)
| PR | Cluster | Leaves | Fully-modeled (DoD) | DirectRef-loose |
|----|---------|--------|---------------------|-----------------|
| baseline | — | — | 70 | 70 |
| PR-1 #699 | False analogy | 4 | 74 (+4) | 74 (+4) |
| PR-2 #701 | Faulty comparison | 5 | 76 (+2) | 79 (+5) |
| PR-3 #703 | Association fallacy | 2 | 78 (+2) | 81 (+2) |
| PR-4 #705 | Vague definition | 3 | 81 (+3) | 84 (+3) |
| **PR-6 (this)** | **Inconsistent definition** | **7** | **86 (+5)** | **89 (+5)** |
| **total** | **3 subfamilies touched** | **21** | **86** | **89** |

**Inexact definition subfamily: 2/3 sub-subs done** (Vague definition PR-4 + Inconsistent definition
PR-6). Remaining: Arbitrary definition (3 mapped anchors + 19 unmapped — large, candidate PR-5/7,
needs splitting by depth).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### pk 826 — Définition incohérente → **undercut**
- **RA-node**: `VerbalClassification_Inference` — classifying via a defined term.
- **I-nodes**: premise P1 "term T means X"; premise P2 "term T means Y"; conclusion C "items labeled
  T are classified". P1 and P2 are mutually contradictory.
- **CA-node**: `Logical_Conflict`, applied as an **undercut** on the RA-node — the inference (that T
  licenses classification) is defective because the definition is self-contradictory. The conclusion C
  may still hold; the *inference* is defective.

### pk 827 — Conditions conflictuelles → **undercut**
- **RA-node**: *(the argument's inference, scheme-untyped)*.
- **I-nodes**: the argument's premises, which are mutually conflicting.
- **CA-node**: `Logical_Conflict`, applied as an **undercut** — the inference is invalid because its
  own conditions conflict. Direct-conflict pattern: the conflict *is* the defect.

### pk 828 — Concept volé → **undercut**
- **RA-node**: `InconsistentCommitment_Inference` — inferring while committed against the premise.
- **I-nodes**: premise P "concept C is invalid/non-existent"; the reasoning nonetheless *uses* C.
- **CA-node**: the scheme itself names the defect (performative inconsistency) — a degenerate
  **undercut** where the RA-node is self-undermining.

### pk 829 — Définition circulaire → FAIL-LOUD at **CA-node**
- **RA-node**: `VerbalClassification_Inference` (exists).
- **CA-node**: ⚠ **absent (FAIL LOUD)** — circularity would be an undercut, but no native AIF
  Conflict node captures it (same gap as PR-1 840).

### pk 830 — Argument du dictionnaire → **undercut**
- **RA-node**: `VerbalClassification_Inference` — classifying via the lexical definition.
- **CA-node**: `ArbitraryVerbalClassification_Inference`, applied as an **undercut** — the inference
  (that the lexical definition licenses classification of reality) is defective because the definition
  is arbitrary for that purpose.

### pk 831 — Sophisme définiste → **undercut**
- **RA-node**: `VagueVerbalClassification_Inference` — the definition is recursively vague.
- **CA-node**: the scheme itself names the defect (direct-conflict pattern) — degenerate **undercut**.

### pk 832 — Sophisme philosophique → FAIL-LOUD at **RA-node**
- **RA-node**: ⚠ **absent (FAIL LOUD)** — the desc is too vague to identify a determinate scheme. No
  RA-node to attack.

### Cluster attack-type distribution
| pk | RA-node | CA-node | Attack type |
|----|---------|---------|-------------|
| 826 | `VerbalClassification_Inference` | `Logical_Conflict` | undercut |
| 827 | *(untyped)* | `Logical_Conflict` | undercut |
| 828 | `InconsistentCommitment_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |
| 829 | `VerbalClassification_Inference` | *(absent — FAIL LOUD)* | would be undercut (circularity) — untyped |
| 830 | `VerbalClassification_Inference` | `ArbitraryVerbalClassification_Inference` | undercut |
| 831 | `VagueVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |
| 832 | *(absent — FAIL LOUD)* | *(absent)* | unmappable |

**All typed leaves are undercuts** — consistent with jsboige's "most fallacies live in the undercut":
definitional fallacies defeat the *inference* (the term→classification step), they do not (usually)
deny the conclusion. No undermine, no rebut in this cluster.

### Representation note
As in PR-1 to PR-4, the I-node/RA-node/CA-node decomposition is **recorded here but not serialized**
in the CSV — pending jsboige's schema decision (§4 of the coverage-status note #707). This PR is
**borrow-root and independent of that decision**: the modeling (schemes + conflicts) holds regardless
of whether I/RA/CA is serialized in new columns.

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No #674/#596/#666 merge, no régén launch.
- ✅ Proposition derived code=truth from the taxonomy CSV + AIF vocabulary confirmed by existing
  usage (all proposed tokens native: `VerbalClassification_Inference`, `Logical_Conflict`,
  `InconsistentCommitment_Inference`, `ArbitraryVerbalClassification_Inference`,
  `VagueVerbalClassification_Inference`). No fabrication — 829/832 FAIL-LOUD with documented gaps.
- ✅ Heterogeneous multi-pattern modeling documented; cross-cluster scheme reuse (831 → PR-4) noted.

Relates: dispatch `mkzt2v` (tertiary), #498 (chantier), PR-1 #699, PR-2 #701, PR-3 #703, PR-4 #705,
coverage-status #707, #133/#130 (existing OWL), #499 (inverse), #677 (0 fabrication), #192, #458.
