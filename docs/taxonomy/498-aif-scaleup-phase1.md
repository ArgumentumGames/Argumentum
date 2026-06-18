# #498 — AIF Scale-up, Phase 1 (GATED proposal — TRIPLE AIF)

**Issue:** [#498 — generative AIF layer for the no-AIF Fallacies (Walton scheme + violated CQ)](https://github.com/ArgumentumGames/Argumentum/issues/498)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-17
**Base:** master `fc8313b3`
**Predecessor:** [#498 generative pilot (18 fallacies, PR #505, merged `fe0410b7`)](498-aif-generative-pilot.md)
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Fallacies CSV change until jsboige approves.** This document + the annotations CSV are the proposal.

---

## 1. The refinement — TRIPLE AIF (why this is more than the #505 pilot)

The #505 pilot annotated 18 fallacies with **RA scheme + violated CQ** (2 elements). jsboige validated it but set a higher bar for the scale-up: **« AIF démonte la mécanique, pas juste scheme+nom »**, and the scale-up must **unify with the EPITA `aspic_analysis`** (ASPIC+ formalization).

ASPIC+ formalizes argument defeat by **attack type** — the structural component of an argument that an attack targets. The pilot's 2-element model does not encode this; the scale-up adds it as the **third element**, producing the **triple AIF**:

| Element | AIF node | Meaning |
|---------|----------|---------|
| **RA-node** | the Walton scheme the fallacy abuses (the legitimate inference) | "which legitimate move is being mimicked?" |
| **attack_type** ∈ {undermine, undercut, rebut} | the ASPIC+ defeat mode | "**how** does the fallacy defeat the RA?" |
| **CA-node / CQ** | the specific critical question violated | "which CQ does it evade?" (names the conflict) |

### ASPIC+ attack-type semantics (the discriminator)

- **undermine** → the CQ attacks the **truth/acceptability of a PREMISE** (`attacked_component = premise`). The fallacy supplies a false, unrepresentative, incomplete, or circular premise. *e.g. biased sample, begging-the-question (premise = conclusion).*
- **undercut** → the CQ attacks the **applicability of the INFERENCE RULE itself**, independent of premise truth (`attacked_component = inference_rule`). The scheme does not license the conclusion in this case. *e.g. equivocation (term shifts), amphiboly (syntax), invalid logical form, slippery slope past threshold.*
- **rebut** → the CQ raises a **counter-conclusion** that conflicts directly with the RA's conclusion (`attacked_component = conclusion`). *e.g. appeal to consequences ("X is desirable" conflicts with "X is true").*

A **coherence rule** is enforced in generation and verification: `attack_type` ↔ `attacked_component` must match (`undermine→premise`, `undercut→inference_rule`, `rebut→conclusion`).

## 2. Reconciliation with the existing convention (the key SDDD finding)

The 42 existing `AIF_skosDirectRef` nodes use **AIFdb-style descriptive names** — the CA node is named by its *concept* (`OppositeConsequences_Conflict`, `ExceptionSimilarityCase_Conflict`, `LackOfCompleteKnowledge_Conflict`, `DifferencesUndermineSimilarity_Conflict`…). They do **NOT** carry an explicit ASPIC+ attack-type tag. This is precisely the structural gap the triple-AIF closes.

**Important nuance (preempts a likely review flag):** PK 839 "Fausse analogie" is named `DifferencesUndermineSimilarity_Conflict` — the word "Undermine" appears in the AIFdb *name*. This is the **colloquial verb** ("the differences undermine the similarity"), **NOT** the ASPIC+ attack type. In ASPIC+ typing, an analogy attacked on similarity-relevance is an **undercut** (the analogy *inference rule* does not apply), which is exactly how the scale-up classifies its sibling PK 834 "Comparaison abusive" (`attack_type = undercut`, same CA concept `DifferencesUndermineSimilarity`). The naming convention (AIFdb) and the typing dimension (ASPIC+) are orthogonal; the triple-AIF makes the typing explicit where the existing layer left it implicit.

## 3. Scope of the scale-up — the 44 no-AIF depth-3 fallacies

The full taxonomy has 1408 nodes; **42 have an `AIF_skosDirectRef`** (3.0%). The depth-3 "named fallacy" level — the consumable concepts that appear on cards and in consumers — has **44 no-AIF nodes across 7 families** (deeper nodes inherit). This is the scale-up target, **phased by family**:

| Family | no-AIF depth-3 | Phase 1? |
|--------|---------------:|:--------:|
| Abus de langage | 5 | ✅ |
| Erreur de raisonnement | 6 | ✅ |
| Erreur mathématique | 6 | phase 2 |
| Insuffisance | 6 | phase 2 |
| Obstruction | 6 | phase 3 |
| Influence | 7 | phase 3 |
| Tricherie | 8 | phase 3 |

**Phase 1 = 2 complete families (Abus de langage + Erreur de raisonnement) = 11 fallacies.** These two are chosen first because (a) they complete the 6 pilot nodes in those families (834/847/855/698/707/784 are upgraded from pilot's 2-element to triple-AIF) + add 5 new, and (b) they are the families where the attack-type classification is cleanest to calibrate (language → inference-rule; formal logic → inference-rule), establishing the method before the messier emotional/evidence families.

## 4. Phase 1 — the 11 annotations

| PK | Fallacy | RA scheme | attack | CA node |
|----|---------|-----------|--------|---------|
| 826 | Définition incohérente | Argument from Verbal Classification | **undercut** | DefinitionConsistency |
| 834 | Comparaison abusive | Argument from Analogy | **undercut** | DifferencesUndermineSimilarity |
| 844 | Sophisme d'association | Argument from Analogy | **undercut** | RelevantSimilarity |
| 847 | Amphibologie | Argument from Verbal Classification | **undercut** | SyntacticAmbiguity |
| 855 | Équivoque | Argument from Verbal Classification | **undercut** | AmbiguousTerm |
| 698 | Pétition de principe | Argument from Commitment | **undermine** | CircularPremise |
| 707 | Inversion de causalité | Argument from Correlation to Cause | **undercut** | ReverseCausation |
| 727 | Erreur de logique propositionnelle | Argument from Rule | **undercut** | InvalidLogicalForm |
| 735 | Erreur de quantification | Argument from Verbal Classification | **undercut** | IllicitQuantifierShift |
| 750 | Erreur de modalité | Argument from Rule | **undercut** | ModalForceMismatch |
| 784 | Syllogisme invalide | Argument from Rule | **undercut** | InvalidLogicalForm |

Full per-fallacy detail (violated CQ, why-not-the-others, justification, AIF `_Inference`/`_Conflict` node forms): [`498-scaleup-phase1-annotations.csv`](498-scaleup-phase1-annotations.csv).

**Attack-type distribution: undercut 10, undermine 1.** This is **theoretically expected, not collapse** — see §5.

## 5. Anti-fabrication + discrimination guarantee

**5-layer verification, all PASS:**
1. `RA_scheme` ∈ the 24-scheme Walton catalog (exact match). ✅
2. `attack_type` ∈ {undermine, undercut, rebut}. ✅
3. `attacked_component` **coheres** with `attack_type` (ASPIC+ map). ✅ 11/11.
4. `fallacy_pk` ∈ the real 1408-row CSV; all 11 confirmed no-`AIF_skosDirectRef`. ✅
5. `CA_node` non-empty, CamelCase, AIFdb-consistent. ✅

**Discrimination was actively tested, not assumed.** The first (naive) generation run returned **all-11-undercut** — a red flag for model anchoring. The run also produced a self-contradiction (PK 698 "Pétition de principe" had CA=`LackOfIndependentSupport` — a *premise* concept — yet classified as undercut, a *rule* attack). A **discrimination-enforced** second pass (deliberate all-3-types + `why_not_others` field + coherence check) **correctly flipped PK 698 to undermine** (`CircularPremise`): begging-the-question is a premise problem, not a rule-applicability problem. The remaining 10 undercut are sound: language fallacies attack the inference rule's applicability (terms/syntax shift) and formal-logic errors attack the inference form — **both are the canonical ASPIC undercut.**

**Why the distribution predicts the other families:** language + formal-logic are *inference-rule* families → undercut-dominant. Phase 2 (Erreur mathématique, Insuffisance — *evidence/premise* families) should shift toward **undermine** (bad data, unrepresentative samples). Phase 3 (Influence, Obstruction — *diversion/counter-claim* families) should surface **rebut** (counter-conclusions, appeals). The triple-AIF's value is exactly this family-conditional discrimination; the phase-1 distribution is the calibration anchor.

## 6. Method & anti-fabrication grounding

- **Grounding catalog:** the 24 canonical Walton schemes (Walton, Reed & Macagno 2008). `RA_scheme` must be an exact catalog name — the model cannot free-associate a scheme.
- **Grounding truth:** the real 1408-row Fallacies CSV (PK + name + description + family). The 11 fallacies are taken directly from the no-AIF depth-3 set (§3).
- **Model / endpoint:** gpt-5.5 via `/v1/responses` + `reasoning:{effort:"low"}` (the cluster's proven reliable-content method; see [reference-gpt55-reasoning-model-api](../../../C:/Users/jsboi/.claude/projects/d--Dev-Argumentum/memory/reference-gpt55-reasoning-model-api.md)).
- **Smoked before batch:** a 1-call smoke verified key-live + schema before the 11-call run.
- Generation script: `tmp/498_scaleup_gen.py` (ephemeral). Raw output: `tmp/498_scaleup_phase1.json`.

## 7. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Fallacies - Taxonomy.csv` (no `AIF_skosDirectRef` cells filled for the 11).
- ❌ Does **not** assert the 11 are final — they are a **calibration batch** for the triple-AIF method on 2 complete families.
- ❌ Does **not** touch the OWL ontology, cards, mindmaps, or any consumer.

## 8. Proposed next steps (gated on jsboige)

1. **jsboige validates** (a) the triple-AIF schema (RA + attack-type + CA/CQ), (b) the attack-type semantics + the AIFdb-name-vs-ASPIC-type reconciliation (§2), (c) the 11 specific classifications (§4), especially the 698-undermine flip and the 834/839 reconciliation.
2. **ai-01 spot-checks** structure (catalog membership, coherence, grounding).
3. On approval: **phase 2** (Erreur mathématique + Insuffisance — expected undermine-dominant), then **phase 3** (Influence + Obstruction + Tricherie — expected rebut-bearing), completing the 44 no-AIF depth-3.
4. **Final gated step:** write the `AIF_skosDirectRef` cells to the production CSV (the `AIF_RA_node`/`AIF_CA_node` forms from the CSV) only after the full 44-pass + ai-01 spot-check + jsboige nod.

---

*GATED proposal. Worker signals structure + grounding + discrimination; ai-01 reviews, jsboige validates content. No production data changed.*
