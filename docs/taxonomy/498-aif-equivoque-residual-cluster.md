# 2026-07-06 — #498 AIF chantier, PR-11 : cluster « Equivoque / mécanismes résiduels » (continuum, précision, diachronie, substitution, perception)

**Scope**: eleventh cluster of the #498 chantier — second installment of the **Equivoque** sub-sub
(Ambiguïté subfamily). Models the **residual mechanisms** left over once lexical polysemy (PR-10) and
reification/magic-of-words (PR-12, follow-up) are carved out: continuum/sorite (859), quantitative
false-precision (857), diachronic semantic shift (865), conceptual substitution/metonymy (866), and
perceptual mishearing/mondegreen (861). **Proposition only — GATED, 0 write to prod CSV.** Triggered by
ai-01 dispatch `5ymo70` (primary: Equivoque sub-sub, split by mechanism; tertiary: continue clusters in
autonomy). **In-sub-sub template reuse — independent of the I/RA/CA serialization decision.**

**Repo reference**: master `bdba45d8`. Issue: #498. Predecessors: PR-1 #699, PR-2 #701, PR-3 #703,
PR-4 #705, PR-6 #708, PR-7 #711 (Amphibologie), PR-8 #713, PR-9 #714 (**Ambiguïté subfamily 3/3**),
PR-10 #717 (Equivoque / lexical polysemy).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies →
> worker applies the CSV edits in a follow-up PR (gated).

---

## TL;DR

The **Equivoque** sub-sub (`Soussousfamille=Equivoque`, 21 rows, largest in the chantier) was split by
mechanism into three PRs. **PR-10** (#717) covered lexical polysemy (5 leaves, 0 FAIL-LOUD).
**PR-11 (this)** covers the **residual mechanisms** — everything in Equivoque that is neither polysemy
nor reification: the sorite/continuum (859, reusing mapped template 858 `VerbalSlipperySlope_Inference`),
quantitative false-precision (857 `ArbitraryVerbalClassification_Inference`), diachronic semantic
shift (865, reusing 856 `VagueVerbalClassification_Inference`), and conceptual substitution/metonymy
(866 `VerbalClassification_Inference`). **PR-12** (follow-up) will cover reification/magic-of-words
(pk 867-875, 9 leaves).

**PR-11 scopes to 5 residual leaves + the 858 template recap.** The honest finding: **4 of the 5
residual leaves map cleanly to native verbal-classification family schemes** (Arbitrary / Vague /
Verbal / VerbalSlipperySlope — all native, 0 fabrication), but **pk 861 (Mondegreen) is a 6th
ontological gap** — a perceptual/phonetic defect with no argumentative inference scheme in AIF,
FAIL-LOUD per #677. This breaks the 3-PR clean streak (PR-8/9/10 = 0 FAIL-LOUD) but is the honest call:
discipline over streak.

**5 rows proposed (4 fully-modeled + 1 FAIL-LOUD), 0 fabrication.** Accounting headline (adopting
po-2024's #716 uniform-strict criterion — the dual criterion used in PR-8/9/10 §6 is dropped): **109
strict fully-modeled** cumulative (+4 this PR).

---

## 1. Cluster state (code=truth)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Abus de langage** (Misleading
language), subfamily **Ambiguïté**, sub-sub **Equivoque** — the residual rows (PR-10 took 855/856/860/
862/863/864; PR-12 will take 867-875):

| pk | text_fr | depth | DirectRef | ExceptionRef | MappingType | State |
|----|---------|-------|-----------|--------------|-------------|-------|
| **858** | **Pente glissante sémantique** (mapped template) | d5 | — | `VerbalSlipperySlope_Inference` | `skos:closeMatch` | **✅ MAPPED (PR-11 template)** |
| 857 | Fausse précision | d5 | — | — | — | unmapped (**PR-11**, quantitative) |
| 859 | Argument de la barbe | d6 | — | — | — | unmapped (**PR-11**, continuum/sorite) |
| 861 | Mondegreen | d5 | — | — | — | unmapped (**PR-11**, perceptual → FAIL-LOUD) |
| 865 | Changement sémantique | d4 | — | — | — | unmapped (**PR-11**, diachronic polysemy) |
| 866 | Métonymie | d5 | — | — | — | unmapped (**PR-11**, substitution) |

The Equivoque sub-sub spans **three mechanisms** (PR-10 polysemy / PR-11 residual / PR-12 reification).
PR-11 is intentionally **heterogeneous** — the residual rows do not share a single mechanism. The
coherent core is the **sorite/continuum** pair (858 template + 859 leaf); the other three (857, 865,
866) each exploit a different lexical-semantic defect; 861 is a pre-argumentative perceptual gap. This
heterogeneity is documented honestly in §5 rather than papered over.

---

## 2. The template model (858, the sorite anchor)

**Scheme**: `VerbalSlipperySlope_Inference` (AIF native, 1 row in prod — the 858 template itself). A
slippery-slope inference over a verbal/semantic continuum: because the term's extension admits
borderline cases, the arguer slides from one end to the other without a principled stopping point.

**desc_fr** (858): *(read in CSV)* "Pente glissante sémantique" — the semantic slippery slope.

**Pattern**: exception-ref (the scheme sits in `ExceptionRef`, no `DirectRef` — the slippery slope is
the *legitimate* scheme being abused; the conflict comes from a violated critical question of that
scheme, namely "where does the slide stop?"). This is the **exception pattern** (vs PR-10's
direct-conflict pattern): the sorite leaf takes the scheme as an exception to flag, not as the direct
defect. The continuum leaf 859 mirrors this pattern.

---

## 3. Proposed AIF structure for the leaves

For each leaf: reuse a native verbal-classification-family scheme, vary the `MappingType` by the leaf's
specialization. Vocabulary restricted to AIF-native tokens (0 fabrication, #677).

### pk 859 — Argument de la barbe (Continuum fallacy / Line-drawing / Sorites paradox) ✅
- **desc_fr**: "En soutenant qu'il n'existe pas de frontière claire entre deux états, vous refusez de
  les distinguer."
- **Mechanism**: the **continuum fallacy** proper (a.k.a. sorites paradox, beard fallacy — "how many
  hairs make a beard?"). Because no sharp boundary separates two states, the arguer refuses to draw
  *any* line, collapsing the distinction. This is the **line-drawing face of the sorite**, the direct
  sibling of 858's verbal-slope face.
- **Proposal**: `DirectRef=`, `ExceptionRef=VerbalSlipperySlope_Inference` *(exception pattern,
  mirroring template 858)*, `MappingType=skos:closeMatch` (a direct sibling: continuum/line-drawing
  vs verbal-slope — two faces of the same sorite mechanism; 859 is the boundary-refusal variant,
  closest to 858).
- **Alternative considered (§5)**: `Gradualism_Inference` (native, 1x) — the legitimate "small steps
  are safe" scheme that the continuum fallacy abuses. Rejected for **in-sub-sub coherence**: 858 (the
  mapped template) uses `VerbalSlipperySlope_Inference`, so the leaf mirrors the template rather than
  introducing a second sorite scheme. Documented for the consumer.

### pk 857 — Fausse précision (False precision / Spurious accuracy) ✅
- **desc_fr**: "Vous présentez des données numériques qui semblent précises mais dont la précision
  n'est pas justifiée."
- **Mechanism**: **over-specific quantitative claim** — the displayed granularity (e.g. "100 000 005
  years") is not justified by the underlying measurement. This is the **precision-flip of vagueness**:
  where 856/PR-10's defect is *under*-specified (vague), 857's is *over*-specified (spuriously exact).
  Both are defects of verbal/numerical **classification granularity**.
- **Proposal**: `DirectRef=ArbitraryVerbalClassification_Inference`, `ExceptionRef=` *(direct-conflict
  pattern)*, `MappingType=skos:closeMatch` (a direct variant: the classification's granularity is
  *arbitrary* — the scheme names the defect: classifying the dinosaur's age to the single year is an
  arbitrary verbal classification).
- **Native-token note**: `ArbitraryVerbalClassification_Inference` is native (appears in 3 existing
  combos with `OppositeConsequences_Conflict` / `BiasedClassification_Conflict` /
  `GeneralAcceptanceDoubt_Conflict`). PR-11 is the **first standalone use** of this token (previously
  always bundled in a combo). The token is native → 0 fabrication; the standalone use is a legitimate
  first, documented here.

### pk 865 — Changement sémantique (Diachronic semantic shift) ✅
- **desc_fr**: "Vous utilisez un terme dont le sens a changé au fil de l'histoire."
- **Mechanism**: **diachronic polysemy** — the term's extension shifted over historical time, and the
  arguer exploits the shift (e.g. reading the modern sense into an old text, or vice versa). This is
  the **temporal mirror of PR-10's 860** (glissement polysémique, synchronic drift): both exploit a
  term whose extension is not fixed, differing only in the *axis* of drift (time vs discourse).
- **Proposal**: `DirectRef=VagueVerbalClassification_Inference`, `ExceptionRef=` *(direct-conflict
  pattern, mirroring template 856 / PR-10 860)*, `MappingType=skos:closeMatch` (the diachronic sibling
  of 860's synchronic drift — reuses PR-10's template scheme; the classification is vague because the
  term's extension is time-dependent).

### pk 866 — Métonymie (Metonymy) ✅
- **desc_fr**: "Vous remplacez une idée par une autre qui lui est conceptuellement liée, sans que cela
  soit clairement justifié."
- **Mechanism**: **conceptual substitution via contiguity** — the figure of metonymy exploited
  fallaciously (e.g. "the pens this company hands out → its success"). The arguer classifies via a
  *substitute* term (one connected by association) rather than the literal referent, and the
  substitution is not justified.
- **Proposal**: `DirectRef=VerbalClassification_Inference`, `ExceptionRef=` *(direct-conflict
  pattern)*, `MappingType=skos:broadMatch` (a broad instance: classification via a non-literal
  substitute term — the generic verbal-classification scheme, of which metonymic substitution is a
  loose/broad instance).
- **Native-token note**: `VerbalClassification_Inference` is native (1 row in prod). This is a
  legitimate second use. `broad` encodes that metonymy is a *loose* specialization (the substitute
  term relation is not itself modeled by a more specific native scheme).

### pk 861 — Mondegreen (Mishearing) ⚠ FAIL-LOUD (6th ontological gap)
- **desc_fr**: "Vous comprenez ou rapportez une phrase de manière erronée parce qu'elle ressemble
  phonétiquement à une autre."
- **Mechanism**: **perceptual/phonetic mishearing** — the listener mistakes one utterance for a
  phonetically similar one (e.g. "Mariage plus vieux" for "Mariage pluvieux"). The defect is
  **pre-argumentative**: it is a failure of auditory perception/recognition, not a violation of a
  defeasible-reasoning scheme.
- **Why FAIL-LOUD**: AIF models **argumentative inference schemes** (defeasible reasoning). The
  mondegreen's defect is not reasoning at all — it is perception. No native token honestly covers
  "the ear misheard." Forcing `VerbalClassification_Inference` (classifying the heard string as the
  wrong word) would stretch a reasoning scheme over a perceptual defect — adjacent to the #677
  prohibition on fabricating/stretching tokens. **Rejected as weak-fit; FAIL-LOUD is the honest call.**
- **Proposal**: `AIF_skosOther` = "Perceptual/phonetic mishearing (mondegreen) — no argumentative
  inference scheme in AIF; the defect is pre-argumentative (auditory perception), not a
  defeasible-reasoning violation. Candidate `VerbalClassification_Inference` rejected as weak-fit
  stretch." No DirectRef, no ExceptionRef. This is the chantier's **6th ontological gap** (after
  circularity PR-1/6, pure-comparison PR-2, syntactic-ambiguity PR-7, implicature PR-8).

---

## 4. Cluster summary (proposed)

| pk | text_fr | Proposed ExceptionRef | Proposed DirectRef | MappingType | Honest? |
|----|---------|----------------------|-------------------|-------------|---------|
| 858 | Pente glissante sémantique | `VerbalSlipperySlope_Inference` | *(absent)* | `skos:closeMatch` | ✅ (template, recap) |
| 857 | Fausse précision | *(absent)* | `ArbitraryVerbalClassification_Inference` | `skos:closeMatch` | ✅ |
| 859 | Argument de la barbe | `VerbalSlipperySlope_Inference` | *(absent)* | `skos:closeMatch` | ✅ |
| 861 | Mondegreen | *(FAIL-LOUD)* | *(FAIL-LOUD)* | *(absent)* | ⚠ gap (perceptual) |
| 865 | Changement sémantique | *(absent)* | `VagueVerbalClassification_Inference` | `skos:closeMatch` | ✅ |
| 866 | Métonymie | *(absent)* | `VerbalClassification_Inference` | `skos:broadMatch` | ✅ |

**5 rows proposed: 4 fully-modeled + 1 FAIL-LOUD, 0 fabrication.** The 4 modelable leaves all reuse the
native **verbal-classification scheme family** (Arbitrary / Vague / Verbal / VerbalSlipperySlope) —
the Equivoque sub-sub is, mechanistically, the verbal-classification-defect sub-sub. 861 (mondegreen)
is the lone perceptual outlier, FAIL-LOUD.

### Residual-mechanism taxonomy (§5 lesson)
PR-11 surfaces **three sub-families of verbal-classification defect** beyond PR-10's polysemy:
- **Granularity defects** (857 false precision / PR-10 856 vagueness): the classification's granularity
  is wrong — *over*-specific (857, arbitrary) vs *under*-specified (856, vague). Mirror pair across
  PR-10/PR-11.
- **Axis-of-drift defects** (865 diachronic / PR-10 860 synchronic): the term's extension is not fixed
  — drift over *time* (865) vs over *discourse* (860). Mirror pair across PR-10/PR-11.
- **Sorite defects** (858 verbal-slope / 859 continuum): the classification slides along a continuum
  without a stopping point — verbal-slope (858) vs boundary-refusal (859). In-sub-sub pair.

And one **pre-argumentative perceptual defect** (861 mondegreen) that falls **outside** the AIF
argumentative-scheme ontology entirely → the 6th gap.

The `MappingType` encodes breadth (866 metonymy = broad loose instance; the rest = close direct
variants). The desc captures the specific axis.

---

## 5. Method notes (additions for the chantier)

- **Residual-cluster honesty (large sub-sub)**: PR-11 is the chantier's first explicitly
  **heterogeneous** cluster — its rows share a *family* (Equivoque / verbal-classification defects)
  but not a single *mechanism*. The coherent core (858/859 sorite) is bundled with three singletons
  (857 quantitative, 865 diachronic, 866 substitution) and one perceptual gap (861). This is the
  honest alternative to either (a) forcing a fake unifying mechanism, or (b) splitting into 4 micro-PRs
  of 1-2 leaves each. Documented as residual-after-split, not as a clean mechanism cluster.
- **Mirror-pair detection across PRs (PR-10 ↔ PR-11)**: the granularity pair (856↔857) and the
  axis-of-drift pair (860↔865) span the PR-10/PR-11 boundary. PR-10 (polysemy) took 856/860; PR-11
  (residual) takes their mirrors 857/865. This is a post-hoc discovery — the CSV depth/structure did
  not pre-announce the pairing. Recorded so the consumer sees the graded family, not isolated leaves.
- **First standalone use of a combo-only native token (857)**: `ArbitraryVerbalClassification_Inference`
  appeared in prod only inside 3-combos (bundled with a Conflict). PR-11 uses it **standalone** as a
  DirectRef (direct-conflict pattern, scheme-as-defect). The token is native → 0 fabrication; the
  standalone use is a legitimate first, flagged so the audit (#709 addendum) can verify it is not a
  fabricated token.
- **Pre-argumentative-defect gap (861, 6th ontological gap)**: the mondegreen is the chantier's first
  leaf whose defect is **pre-argumentative** (perceptual, not reasoning). The 5 prior gaps
  (circularity / pure-comparison / syntactic-ambiguity / implicature) were all *argumentative* schemes
  absent from AIF; 861 is a *non-argumentative* defect. This refines the gap typology: AIF lacks both
  (a) some argumentative schemes, and (b) any coverage of pre-argumentative perceptual errors. The
  Equivoque sub-sub is where the taxonomy touches the reasoning/perception boundary.
- **`Gradualism_Inference` alternative for 859**: the native `Gradualism_Inference` (the "small steps
  are safe" scheme the continuum fallacy abuses) is arguably a *closer* fit for 859 than the borrowed
  `VerbalSlipperySlope_Inference`. Rejected for in-sub-sub coherence (leaf mirrors its 858 template).
  Flagged for jsboige: if the ontology later adds a `Continuum_Inference` / line-drawing scheme, 859 is
  the canonical candidate to re-point.

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not "100% leaves") | ✅ §4 (4 residual leaves fully-modeled + 1 FAIL-LOUD gap; Equivoque sub-sub split documented, 9 reification leaves deferred to PR-12) |
| Per unmapped leaf: legitimate scheme + exception/CQ (not Latin alone) | ✅ §3 (4 leaves scheme + mechanism each; 861 FAIL-LOUD honest) |
| `AIF_skosMappingType` coherent | ✅ §4 (close for direct variants, broad for loose metonymy instance) |
| Fail-loud when no honest scheme fits | ✅ §3 (861 mondegreen = 6th ontological gap, perceptual, documented in `AIF_skosOther`) |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies, then CSV edits, then OWL regen (#133) |

### Coverage accounting (cumulative — **uniform-strict headline**, per po-2024 #716)

po-2024's audit addendum (#716, in batch pending) resolved the PR-8/9/10 §6 dual-criterion FLAG: the
"inclusive" count (counting CA-missing rows as fully-modeled) is **non-uniform** and is dropped. **A
single authoritative headline is adopted: strict = both scheme AND conflict.** PR-11 adopts this going
forward; PR-10's §6 dual row is retroactively noted as superseded by the uniform criterion.

| PR | Cluster | +strict fully-modeled | strict cumulative |
|----|---------|----------------------|------------------|
| baseline | — | — | 70 |
| PR-1 #699 | False analogy | +3 (840 CA-missing partial) | 73 |
| PR-2 #701 | Faulty comparison | +2 (834/835/837 RA-missing partial) | 75 |
| PR-3 #703 | Association fallacy | +2 | 77 |
| PR-4 #705 | Vague definition | +3 | 80 |
| PR-6 #708 | Inconsistent definition | +5 (829 CA-missing, 832 RA-missing partial) | 85 |
| PR-7 #711 | Amphibologie | +5 (847/848/853 CA-missing partial) | 90 |
| PR-8 #713 | Narrative ambiguity / insinuation | +4 | 94 |
| PR-9 #714 | Narrative ambiguity / deception | +6 | 100 |
| PR-10 #717 | Equivoque / polysemy | +5 | 105 |
| **PR-11 (this)** | **Equivoque / residual** | **+4** (861 FAIL-LOUD excluded) | **109** |
| **total (strict headline)** | **4 subfamilies, 11 clusters** | **+39** | **109** |

**Partial / gap leaves tracked honestly (10 total)**:
- 5 CA-missing (argumentative, undercut if ontological extension granted): 840 (PR-1), 829 (PR-6),
  847/848/853 (PR-7) → +5 → **114 potential**.
- 4 RA-missing (need a new scheme): 834/835/837 (PR-2 pure-comparison), 832 (PR-6) → deferred.
- 1 FAIL-LOUD perceptual gap: **861 (PR-11 mondegreen)** → pre-argumentative, not scheme-assignable.

**This PR's +4 is criterion-independent** — all 4 modelable leaves have both a scheme and a conflict.
861 is excluded from strict (no scheme) and tracked as a gap, not a partial.

**Equivoque sub-sub: residual mechanisms covered (4 modelable + 1 gap done)**. Remaining Equivoque
mechanism: **reification/magic-of-words (pk 867-875, 9 leaves → PR-12)** — the largest remaining
Equivoque block, likely needing a reification-critical-question (7th gap candidate).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### pk 858 — Pente glissante sémantique (template) → **undercut** (exception pattern)
- **RA-node**: `VerbalSlipperySlope_Inference`.
- **CA-node**: the violated CQ "where does the slide stop?" — the conflict node. Undercut (the
  legitimate scheme is abused; its conclusion may hold but the inference is defective without a
  stopping condition).

### pk 859 — Argument de la barbe → **undercut** (exception pattern, mirrors 858)
- **RA-node**: `VerbalSlipperySlope_Inference` (reused from 858 — continuum = boundary-refusal face
  of the verbal sorite).
- **I-nodes**: premise P1 "no sharp boundary separates state A from state B"; conclusion C "therefore
  A and B cannot be distinguished / no line can be drawn".
- **CA-node**: **undercut** — the inference (no boundary ⟹ no distinction) is defective because the
  absence of a *sharp* boundary does not entail the absence of *any* usable boundary. Degenerate form
  of the exception pattern: the CQ names the defect.

### pk 857 — Fausse précision → **undercut** (degenerate, direct-conflict)
- **RA-node**: `ArbitraryVerbalClassification_Inference` — classifying the quantity to an unjustified
  granularity.
- **CA-node**: degenerate **undercut** — the classification is defective because its granularity is
  arbitrary (not supported by the measurement). The scheme names the defect (direct-conflict pattern).

### pk 865 — Changement sémantique → **undercut** (degenerate, direct-conflict)
- **RA-node**: `VagueVerbalClassification_Inference` (reused from 856/PR-10 — the term's extension is
  not fixed, here temporally).
- **CA-node**: degenerate **undercut** — the classification is vague because the term's extension
  shifted historically and the arguer suppresses the time-axis.

### pk 866 — Métonymie → **undercut** (degenerate, direct-conflict)
- **RA-node**: `VerbalClassification_Inference` — classifying via a substitute (metonymic) term.
- **CA-node**: degenerate **undercut** — the classification is defective because the substitute is not
  a justified referent (the contiguity does not license the classification).

### pk 861 — Mondegreen → **gap** (no RA-node assignable)
- **RA-node**: *(none)* — the defect is pre-argumentative (perceptual); AIF has no scheme for auditory
  misrecognition.
- **CA-node**: *(none)* — without an RA-node there is no inference to attack.
- This is the chantier's first leaf with **no I/RA/CA decomposition** at all — it falls outside the
  AIF argumentation ontology. Recorded as a structural finding (§5 6th gap), not forced into a
  degenerate-undercut frame.

### Cluster attack-type distribution
| pk | RA-node | CA-node | Attack type |
|----|---------|---------|-------------|
| 858 | `VerbalSlipperySlope_Inference` | CQ "where does the slide stop?" | undercut (exception) |
| 857 | `ArbitraryVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |
| 859 | `VerbalSlipperySlope_Inference` | CQ "no sharp ⟹ no usable boundary?" | undercut (exception) |
| 861 | *(none — perceptual gap)* | *(none)* | **gap** |
| 865 | `VagueVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |
| 866 | `VerbalClassification_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |

**4 of 5 leaves are degenerate/exception undercuts** (direct-conflict or CQ-named defect, as in PR-4
800 / PR-9 885 / PR-10); 859 is the exception-pattern sibling of the 858 template; **861 has no
decomposition** (the perceptual gap). The Equivoque residual fallacies are, where argumentative,
defective verbal-classification inferences; the mondegreen is not argumentative at all.

### Representation note
As in PR-1 to PR-10, the I-node/RA-node/CA-node decomposition is **recorded here but not serialized**
in the CSV — pending jsboige's schema decision (§4 of the coverage-status note #707, ratified option
(a): 2 new columns `AIF_attackType`+`AIF_attackedNode`, post-tag). This PR is **in-sub-sub template
reuse and independent of that decision**: the modeling (schemes + conflicts) holds regardless.

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No #674/#596/#666/#686 merge, no régén launch.
- ✅ Proposition derived code=truth from the taxonomy CSV + AIF vocabulary confirmed by existing usage
  (all 4 proposed tokens — `ArbitraryVerbalClassification_Inference`, `VerbalClassification_Inference`,
  `VagueVerbalClassification_Inference`, `VerbalSlipperySlope_Inference` — are native in prod, 0
  fabrication; 861 mondegreen FAIL-LOUD with rejected weak alternative documented).
- ✅ Residual-cluster heterogeneity documented; mirror-pairs across PR-10/11 noted; first standalone
  use of a combo-only token (857) flagged; 6th ontological gap (861 pre-argumentative perceptual)
  recorded; uniform-strict accounting headline adopted (po-2024 #716).

Relates: dispatch `5ymo70` (primary/tertiary), #498 (chantier), PR-1 #699, PR-2 #701, PR-3 #703,
PR-4 #705, PR-6 #708, PR-7 #711, PR-8 #713, PR-9 #714, PR-10 #717, coverage-status #707,
verification-audit #709 + addendum #716, family-gap #712, #133/#130 (existing OWL), #499 (inverse),
#677 (0 fabrication), #192, #458.
