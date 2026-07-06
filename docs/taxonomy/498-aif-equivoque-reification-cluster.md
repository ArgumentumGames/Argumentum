# 2026-07-06 — #498 AIF chantier, PR-12 : cluster « Equivoque / Réification & magie des mots » (level-confusion) — **complète la sub-sub**

**Scope**: twelfth cluster of the #498 chantier — third and final installment of the **Equivoque**
sub-sub (Ambiguïté subfamily). Models the **reification / magic-of-words / level-confusion mechanism**
(pk 867-875, 9 leaves), the largest remaining Equivoque block. **Proposition only — GATED, 0 write to
prod CSV.** Triggered by ai-01 dispatch `5ymo70` (primary: Equivoque sub-sub split by mechanism;
tertiary: continue clusters in autonomy). **Completes the Equivoque sub-sub** (after PR-10 polysemy,
PR-11 residual). Independent of the I/RA/CA serialization decision.

**Repo reference**: master `bdba45d8`. Issue: #498. Predecessors: PR-1 #699, PR-2 #701, PR-3 #703,
PR-4 #705, PR-6 #708, PR-7 #711 (Amphibologie), PR-8 #713, PR-9 #714 (**Ambiguïté subfamily 3/3**),
PR-10 #717 (Equivoque / polysemy), PR-11 #718 (Equivoque / residual).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies →
> worker applies the CSV edits in a follow-up PR (gated).

---

## TL;DR

The **Equivoque** sub-sub (21 rows, largest in the chantier) was split by mechanism into three PRs.
**PR-10** (#717) covered lexical polysemy (5 leaves, 0 FAIL-LOUD). **PR-11** (#718) covered residual
mechanisms (4 modeled + 1 FAIL-LOUD, the mondegreen perceptual gap). **PR-12 (this)** covers the
**reification / magic-of-words / level-confusion mechanism** (pk 867-875, 9 leaves) — and completes
the sub-sub.

The honest finding: **only 2 of the 9 leaves map to a native AIF scheme**. The other **7 leaves
(868-874) form the chantier's largest single gap** — a **reification / language-creates-reality /
level-confusion family** (Idola fori, Réification, Sophisme référentiel, Magie des mots, Carte/
territoire, Usage/Mention, Simulacre) whose defect is a **semantic-ontological category error** that
no defeasible-reasoning scheme in AIF honestly covers. This is the **7th ontological gap** (after
circularity PR-1/6, pure-comparison PR-2, syntactic-ambiguity PR-7, implicature PR-8, perceptual
PR-11), and unlike the 6th (perceptual/pre-argumentative) it is **argumentative** — a genuine
reasoning pattern (word-existence ⟹ referent-existence) that AIF's scheme vocabulary does not model.

**9 rows proposed: 2 fully-modeled + 7 FAIL-LOUD, 0 fabrication.** The 2 modelable leaves each reuse a
native scheme with an honest mechanism:
- **867** Confusion type-exemple → `Example_Inference` (direct-conflict) — the legitimate
  argument-from-example scheme abused by crossing the general/particular boundary.
- **875** Motte-and-bailey → `VagueVerbalClassification_Inference` (direct-conflict, PR-10 reuse) —
  equivocation between the modest (motte) and controversial (bailey) reading of a concept.

Accounting headline (uniform-strict, per po-2024 #716, adopted in PR-11): **+2 strict, 111 cumulative**.

---

## 1. Cluster state (code=truth)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Abus de langage** (Misleading
language), subfamily **Ambiguïté**, sub-sub **Equivoque** — the reification block (PR-10 took
855/856/860/862/863/864; PR-11 took 857/858/859/861/865/866):

| pk | text_fr | depth | DirectRef | ExceptionRef | MappingType | State |
|----|---------|-------|-----------|--------------|-------------|-------|
| 867 | Sophisme de confusion type-exemple | d6 | — | — | — | unmapped (**PR-12**, type↔example) |
| 868 | Idola fori | d5 | — | — | — | unmapped (**PR-12**, level-confusion → FAIL-LOUD) |
| 869 | Réification | d6 | — | — | — | unmapped (**PR-12**, level-confusion → FAIL-LOUD) |
| 870 | Sophisme référentiel | d7 | — | — | — | unmapped (**PR-12**, level-confusion → FAIL-LOUD) |
| 871 | La magie des mots | d7 | — | — | — | unmapped (**PR-12**, level-confusion → FAIL-LOUD) |
| 872 | Confondre la carte avec le territoire | d6 | — | — | — | unmapped (**PR-12**, level-confusion → FAIL-LOUD) |
| 873 | Erreur Usage Mention | d7 | — | — | — | unmapped (**PR-12**, level-confusion → FAIL-LOUD) |
| 874 | Simulacre | d6 | — | — | — | unmapped (**PR-12**, level-confusion → FAIL-LOUD) |
| 875 | Sophisme de la motte castrale | d6 | — | — | — | unmapped (**PR-12**, equivocation) |

The reification block spans **two distinct sub-mechanisms** (§3): a single type/example-confusion leaf
(867), a motte-and-bailey equivocation leaf (875), and a 7-leaf level-confusion family (868-874). The
block is the chantier's most gap-dense: 7/9 leaves have no honest native scheme.

---

## 2. The two modelable mechanisms (867, 875)

### pk 867 — Sophisme de confusion type-exemple → `Example_Inference` (direct-conflict)
- **desc_fr**: "Vous confondez un concept général avec un exemple particulier, ce qui rend votre
  argument ambigu."
- **Scheme**: `Example_Inference` (AIF native, used standalone at pk 1066/1175 — argument from a
  particular example to a general claim). The legitimate scheme reasons from an *instance* to the
  *type*; the fallacy crosses the boundary — treating the example as the type, or the type as reducible
  to one example.
- **Pattern**: direct-conflict (the scheme in DirectRef names the defect — the example-based
  classification is defective because the general/particular boundary is violated). Degenerate
  undercut.
- **Proposal**: `DirectRef=Example_Inference`, `ExceptionRef=`, `MappingType=skos:closeMatch`
  (a direct variant: argument-from-example abused by type/example confusion — the closest native
  scheme to the general↔particular mechanism).

### pk 875 — Sophisme de la motte castrale (Motte-and-bailey) → `VagueVerbalClassification_Inference` (direct-conflict, PR-10 reuse)
- **desc_fr**: "De deux positions qui présentent des similitudes, vous défendez la position modeste
  consensuelle et affirmez prouver la position complexe controversée."
- **Mechanism**: **equivocation between two senses of a concept/thesis** — the arguer defends the
  modest, easy-to-defend sense (the *motte*, e.g. "stars influence nature via tides") then claims to
  have proven the controversial sense (the *bailey*, e.g. "astrology works"). This is **genuinely the
  lexical-polysemy mechanism of PR-10** (a concept with two readings, the arguer exploiting the
  shift), specialized to the modest/controversial axis.
- **Scheme**: `VagueVerbalClassification_Inference` (reused from PR-10 template 856). The
  classification of the concept (modest vs controversial) is vague — its extension is not fixed, and
  the arguer shifts reading mid-discussion.
- **Pattern**: direct-conflict (mirroring PR-10's 855/860). Degenerate undercut.
- **Proposal**: `DirectRef=VagueVerbalClassification_Inference`, `ExceptionRef=`,
  `MappingType=skos:closeMatch` (a direct variant: equivocation specialized to the
  modest-vs-controversial thesis axis — the motte/bailey mirror of PR-10's unidirectional cases).

---

## 3. The level-confusion family (868-874) — 7th ontological gap (cluster)

### The shared defect
All 7 leaves commit a **semantic-ontological category error**: they confuse two levels of description
that AIF's argumentation schemes do not distinguish.

| pk | text_fr | the two levels confused |
|----|---------|------------------------|
| 868 | Idola fori | language ↔ reality (perfect correspondence assumed) |
| 869 | Réification | abstract concept ↔ physical particular |
| 870 | Sophisme référentiel | word ↔ object (indissociable) |
| 871 | La magie des mots | word-exists ↔ referent-exists |
| 872 | Confondre la carte avec le territoire | model/representation ↔ reality |
| 873 | Erreur Usage Mention | use of a word ↔ mention of a word |
| 874 | Simulacre | reality ↔ idealized representation |

### Why FAIL-LOUD (no honest native scheme)
The candidate native schemes — `VerbalClassification_Inference`, `VagueVerbalClassification_Inference`,
`ArbitraryVerbalClassification_Inference`, `VerbalSlipperySlope_Inference` — all model **classification
defects** (grouping/labeling items via a term, where the term's extension is wrong: too vague, too
arbitrary, too granular, sliding). The reification/magic-of-words defect is **not a classification
defect** — it is an **existence/reference claim** (the word exists, therefore the referent exists; the
model is the reality; the abstract is concrete). Forcing a verbal-classification scheme over an
existence/reference claim would be the #677 "stretch tokens over gaps" failure, not honest modeling.

The 7th gap is **argumentative** (these are genuine fallacies used in reasoning, unlike PR-11's
mondegreen which is pre-argumentative perception). AIF's scheme vocabulary models *defeasible
inference patterns* (from example, from expert, from sign, etc.) but has **no scheme for the
"language-creates-reality" / "sign=referent" inference pattern** — the reification family reasons
from a word/existence to a referent/existence, a pattern AIF does not name. This is a genuine
ontology gap, documented rather than fabricated.

### Proposal (all 7 — FAIL-LOUD, documented in `AIF_skosOther`)
For each of 868-874: `DirectRef=`, `ExceptionRef=`, `AIF_skosOther` = a level-confusion-specific note.
Shared framing: *"Reification / level-confusion defect (sign↔referent / abstract↔concrete /
model↔reality / use↔mention). No native AIF defeasible-reasoning scheme models the
language-creates-reality inference pattern; verbal-classification schemes model classification-via-term
defects, not existence/reference category errors (7th ontological gap, argumentative)."* Plus a
leaf-specific clause per pk (e.g. 871: "word-existence ⟹ referent-existence"; 872:
"model↔territory level error"; 873: "use↔mention level error").

### Considered alternatives (rejected, documented per #677)
- `VerbalClassification_Inference` for 868-874: rejected — stretches a classification scheme over an
  existence/reference claim. Adjacent in *topic* (both concern language) but distinct in *scheme type*.
- A hypothetical `Reification_Inference` / `Reference_Inference` / `UseMention_Inference` scheme:
  does not exist in AIF native vocabulary → fabrication forbidden (#677). Flagged as the candidate
  ontology extension if jsboige/curators add the reification-family scheme later.

---

## 4. Cluster summary (proposed)

| pk | text_fr | Proposed DirectRef | Proposed ExceptionRef | MappingType | Honest? |
|----|---------|-------------------|----------------------|-------------|---------|
| 867 | Confusion type-exemple | `Example_Inference` | *(absent)* | `skos:closeMatch` | ✅ |
| 868 | Idola fori | *(FAIL-LOUD)* | *(FAIL-LOUD)* | *(absent)* | ⚠ gap (level-confusion) |
| 869 | Réification | *(FAIL-LOUD)* | *(FAIL-LOUD)* | *(absent)* | ⚠ gap (level-confusion) |
| 870 | Sophisme référentiel | *(FAIL-LOUD)* | *(FAIL-LOUD)* | *(absent)* | ⚠ gap (level-confusion) |
| 871 | La magie des mots | *(FAIL-LOUD)* | *(FAIL-LOUD)* | *(absent)* | ⚠ gap (level-confusion) |
| 872 | Carte/territoire | *(FAIL-LOUD)* | *(FAIL-LOUD)* | *(absent)* | ⚠ gap (level-confusion) |
| 873 | Usage/Mention | *(FAIL-LOUD)* | *(FAIL-LOUD)* | *(absent)* | ⚠ gap (level-confusion) |
| 874 | Simulacre | *(FAIL-LOUD)* | *(FAIL-LOUD)* | *(absent)* | ⚠ gap (level-confusion) |
| 875 | Motte castrale | `VagueVerbalClassification_Inference` | *(absent)* | `skos:closeMatch` | ✅ |

**9 rows proposed: 2 fully-modeled + 7 FAIL-LOUD, 0 fabrication.** The reification block is the
chantier's most gap-dense cluster (7/9 leaves). The 2 modelable leaves reuse native schemes with
honest mechanisms (867 example-boundary, 875 equivocation).

### Reification-block taxonomy (§5 lesson)
The 9 leaves split into **three sub-mechanisms**:
- **Type/example boundary** (867, 1 leaf): general↔particular confusion → `Example_Inference`
  (legitimate scheme abused). Modelable.
- **Modest/controversial equivocation** (875, 1 leaf): motte-and-bailey = PR-10 polysemy mechanism
  specialized to the thesis-axis → `VagueVerbalClassification_Inference`. Modelable.
- **Level-confusion family** (868-874, 7 leaves): sign↔referent / abstract↔concrete / model↔reality /
  use↔mention category errors → **no native scheme (7th gap)**. FAIL-LOUD.

The level-confusion family is itself graded along its confused-pair axis (868 language/reality most
general → 870 word/object → 871 existence-claim → 872 model/reality → 873 use/mention → 874 real/ideal
→ 869 abstract/concrete). The 7 leaves form a tight semantic cluster (all "two levels of description
conflated"), confirming they are one mechanism with one gap, not seven unrelated gaps.

---

## 5. Method notes (additions for the chantier)

- **Largest gap cluster (7 leaves, argumentative)**: PR-12 surfaces the chantier's largest single
  ontological gap — the 7-leaf reification/level-confusion family. Unlike PR-11's 861 mondegreen
  (1 leaf, pre-argumentative/perceptual), this gap is **argumentative and cluster-shaped**: 7 genuine
  fallacies share one reasoning pattern (language-creates-reality / sign=referent) that AIF does not
  model. This refines the gap typology again: AIF lacks (a) some argumentative schemes
  (circularity/comparison/syntactic-ambiguity/implicature/reification), (b) any perceptual coverage
  (mondegreen). The reification gap is the **most impactful** — 7 leaves is more than all 5 prior gaps
  combined, and it is the canonical "abuse of language" mechanism (Bacon's Idola fori), so its absence
  from AIF is a structural finding worth surfacing to the ontology curators.
- **Motte-and-bailey = PR-10 polysemy specialization (875)**: the motte-and-bailey fallacy is often
  classified as its own fallacy, but mechanistically it is the lexical-polysemy mechanism (a concept
  with two readings, exploited by shifting) specialized to the **modest/controversial thesis axis**.
  PR-12 models it via PR-10's `VagueVerbalClassification_Inference` rather than inventing a new token
  — the cross-PR reuse (PR-10 → PR-12) confirms the polysemy family's generality. This is the second
  cross-PR-10 reuse (after PR-11's 865 diachronic-drift mirror).
- **First use of `Example_Inference` in DirectRef (867)**: `Example_Inference` appeared natively only
  in `ExceptionRef` (pk 1066/1175, the cognitive-bias region). PR-12 uses it as `DirectRef`
  (direct-conflict pattern, scheme-as-defect) for the first time. Native → 0 fabrication; the DirectRef
  use is a legitimate first, flagged for audit.
- **Cluster-shaped gap vs singleton gap**: the chantier's first 5 gaps were singletons (one leaf
  each, or a 3-leaf cluster PR-7 syntactic-ambiguity). PR-12's 7-leaf cluster is the first
  **mechanism-wide gap** — an entire reasoning-pattern family with no AIF coverage. This shifts the
  ontology-extension recommendation from "add a few missing schemes" to "consider a reification-family
  scheme cluster (Reference/UseMention/ModelReality) as a coherent addition."

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not "100% leaves") | ✅ §4 (2 leaves fully-modeled + 7 FAIL-LOUD documented; Equivoque sub-sub split complete) |
| Per unmapped leaf: legitimate scheme + exception/CQ (not Latin alone) | ✅ §2-3 (2 leaves scheme + mechanism; 7 FAIL-LOUD honest) |
| `AIF_skosMappingType` coherent | ✅ §4 (close for the 2 direct variants) |
| Fail-loud when no honest scheme fits | ✅ §3 (868-874 = 7th ontological gap, argumentative, documented in `AIF_skosOther`) |
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
| **PR-12 (this)** | **Equivoque / reification** | **+2** (7 FAIL-LOUD excluded) | **111** |
| **total (strict headline)** | **4 subfamilies, 12 clusters** | **+41** | **111** |

**Partial / gap leaves tracked honestly (17 total)**:
- 5 CA-missing (argumentative, undercut if ontological extension): 840, 829, 847/848/853 → +5 → 116 potential.
- 4 RA-missing (need new scheme): 834/835/837, 832 → deferred.
- 8 FAIL-LOUD gaps: 861 (perceptual, PR-11) + **868-874 (reification cluster, PR-12)** → all
  documented in `AIF_skosOther`, none scheme-assignable without ontology extension.

**This PR's +2 is criterion-independent** — both modelable leaves have a scheme and a conflict. The 7
FAIL-LOUD leaves are excluded from strict (no scheme) and tracked as the 7th gap cluster.

**Equivoque sub-sub: COMPLETE** (polysemy PR-10 + residual PR-11 + reification PR-12 = 21 rows
accounted: 11 fully-modeled + 1 perceptual gap + 7 reification gap + 2 template recaps). This is the
chantier's **third fully-accounted subfamily-level block** (after Fallacious comparison 3/3 and
Ambiguïté 3/3), and the Equivoque sub-sub is the **largest single block closed** (21 rows).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### pk 867 — Confusion type-exemple → **undercut** (degenerate, direct-conflict)
- **RA-node**: `Example_Inference` — reasoning from an instance to the type (or classifying via an
  example).
- **CA-node**: degenerate **undercut** — the example-based inference is defective because the
  general/particular boundary is violated (the example is treated as the type, or the type reduced to
  one example).

### pk 875 — Motte castrale → **undercut** (degenerate, direct-conflict)
- **RA-node**: `VagueVerbalClassification_Inference` — classifying the concept (whose extension is not
  fixed between modest and controversial).
- **I-nodes**: premise P1 (motte) "concept C means M_modest"; premise P2 (bailey) "concept C means
  M_controversial"; conclusion C "having defended M_modest, I have proven M_controversial".
- **CA-node**: degenerate **undercut** — the inference (defending the motte ⟹ proving the bailey) is
  defective because P1 and P2 use different senses of C (the equivocation).

### pk 868-874 — level-confusion family → **gap** (no RA-node assignable)
- **RA-node**: *(none)* — the inference pattern (word-existence ⟹ referent-existence / model ⟹
  reality / sign ⟹ referent) has no AIF scheme. These are genuine reasoning patterns (argumentative),
  but AIF's scheme vocabulary does not name the language-creates-reality pattern.
- **CA-node**: *(none)* — without an RA-node there is no inference node to attack.
- This is the chantier's **first cluster with no I/RA/CA decomposition for any member** — the entire
  7-leaf family falls outside the AIF argumentation ontology. Recorded as a structural finding (§5
  7th gap, argumentative cluster), not forced into degenerate-undercut frames.

### Cluster attack-type distribution
| pk | RA-node | CA-node | Attack type |
|----|---------|---------|-------------|
| 867 | `Example_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |
| 868-874 | *(none — reification gap)* | *(none)* | **gap (cluster)** |
| 875 | `VagueVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |

**2 of 9 leaves are degenerate undercuts** (direct-conflict, as in PR-4 800 / PR-9 885 / PR-10 / PR-11);
**7 leaves have no decomposition** (the reification gap cluster). The reification block is, where
argumentatively modelable at all, two isolated scheme reuses (867 example, 875 equivocation) surrounded
by a family that AIF does not reach.

### Representation note
As in PR-1 to PR-11, the I-node/RA-node/CA-node decomposition is **recorded here but not serialized**
in the CSV — pending jsboige's schema decision (#707 §4, ratified option (a): 2 new columns
`AIF_attackType`+`AIF_attackedNode`, post-tag). This PR is **in-sub-sub scheme reuse and independent
of that decision**: the modeling holds regardless.

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No #674/#596/#666/#686 merge, no régén launch.
- ✅ Proposition derived code=truth from the taxonomy CSV + AIF vocabulary confirmed by existing usage
  (both proposed tokens — `Example_Inference`, `VagueVerbalClassification_Inference` — are native in
  prod, 0 fabrication; 7 FAIL-LOUD leaves documented with rejected weak alternatives per #677).
- ✅ Largest-gap-cluster finding (7-leaf reification family) documented; motte-and-bailey = PR-10
  polysemy specialization noted; first `Example_Inference` DirectRef use flagged; uniform-strict
  accounting headline maintained (po-2024 #716); Equivoque sub-sub completion recorded.

Relates: dispatch `5ymo70` (primary/tertiary), #498 (chantier), PR-1 #699, PR-2 #701, PR-3 #703,
PR-4 #705, PR-6 #708, PR-7 #711, PR-8 #713, PR-9 #714, PR-10 #717, PR-11 #718, coverage-status #707,
verification-audit #709 + addendum #716, family-gap #712, #133/#130 (existing OWL), #499 (inverse),
#677 (0 fabrication), #192, #458.
