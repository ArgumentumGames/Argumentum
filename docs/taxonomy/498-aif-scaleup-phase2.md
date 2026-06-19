# #498 — AIF Scale-up, Phase 2 (GATED proposal — TRIPLE AIF)

**Issue:** [#498 — generative AIF layer for the no-AIF Fallacies (Walton scheme + violated CQ)](https://github.com/ArgumentumGames/Argumentum/issues/498)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-19
**Base:** master `909d04c3`
**Predecessor:** [#498 AIF scale-up phase 1 (11 fallacies, PR #509, merged `cae93dc8`)](498-aif-scaleup-phase1.md)
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Fallacies CSV change until jsboige approves.** This document + the annotations CSV are the proposal.

**Calibration:** [`498-aif-scaleup-phase1.md`](498-aif-scaleup-phase1.md) — phase 1 (Abus de langage + Erreur de raisonnement, 11 fallacies) validated the triple-AIF method (RA + ASPIC+ attack-type + CA/CQ) and predicted phase 2 would shift toward **undermine** (evidence/premise families). Phase 2 extends the triple-AIF to **2 more families (Erreur mathématique + Insuffisance = 12 fallacies)**. Same schema, same 24-scheme Walton catalog grounding, same ASPIC+ coherence rule, same anti-fab guarantee.

---

## 1. Scope of phase 2

Phase 2 annotates the **no-AIF depth-3 fallacies** of **Erreur mathématique (PK 594) + Insuffisance (PK 1)** — the **evidence/premise** families (bad data, unrepresentative samples, ad-hoc rescue, unparsimonious explanations). These are the families where the ASPIC+ attack-type classification is predicted to shift toward **undermine** (the fallacy supplies a false/unrepresentative/incomplete premise), contrasting with phase 1's language/formal-logic families (undercut-dominant — inference-rule attacks).

| Family | no-AIF depth-3 | Phase? | Predicted dominant attack |
|--------|---------------:|:------:|:--------------------------|
| Abus de langage | 5 | ✅ phase 1 | undercut |
| Erreur de raisonnement | 6 | ✅ phase 1 | undercut |
| **Erreur mathématique** | **6** | **✅ phase 2** | undermine |
| **Insuffisance** | **6** | **✅ phase 2** | undermine |
| Obstruction | 6 | phase 3 | rebut-bearing |
| Influence | 7 | phase 3 | rebut-bearing |
| Tricherie | 8 | phase 3 | rebut-bearing |

**Phase 2 = 2 families, 12 fallacies.** Combined with phase 1 (11), that's **23/44 no-AIF depth-3 fallacies annotated** (52%) after this batch. Phase 3 (Obstruction + Influence + Tricherie, 21 fallacies) completes the 44.

## 2. Schema — identical triple-AIF schema (phase 1, unchanged)

```
fallacy_pk, family, fallacy_name, RA_scheme, attack_type, attacked_component,
CA_node, AIF_RA_node, AIF_CA_node, violated_cq, why_not_others, justification
```

The triple-AIF (§1 of phase-1 doc): **RA-node** (the Walton scheme abused) + **attack_type** ∈ {undermine, undercut, rebut} (the ASPIC+ defeat mode) + **CA-node/CQ** (the specific critical question violated). The **coherence rule** is enforced: `attack_type` ↔ `attacked_component` must match (`undermine→premise`, `undercut→inference_rule`, `rebut→conclusion`).

## 3. The inverse paradigm (honored per fallacy, same as phase 1)

A fallacy is a **defeasible exception to a legitimate Walton scheme** — it mimics a legitimate inference but defeats it by evading one of the scheme's critical questions. Phase 2's families are **evidence/premise** families: the fallacies counterfeit an evidence-based inference (Sign, Example, Cause to Effect) but supply a **bad premise** (unrepresentative sample, neglected base rate, imprecise data, ad-hoc cause, unparsimonious explanation) — the canonical ASPIC+ **undermine**. The formal/model fallacies (Infini fallacieux, Opération inappropriée) and the values/analogy fallacies (Appel à la nature, Sophisme moraliste, Sophisme du jeu, Mauvaises raisons) attack the **inference rule's applicability** instead — the canonical **undercut**.

## 4. Method & anti-fabrication guarantee (identical to phase 1)

- **Model:** gpt-5.5 (OpenAI direct, key live 2026-06-19). `POST /v1/responses`, `reasoning:{effort:"low"}`, `max_output_tokens:7000`, no `temperature`. Answer read from `output[].content[].text`.
- **Grounding catalog:** the **24 canonical Walton schemes** (Walton, Reed & Macagno 2008), reused verbatim from phase 1. The prompt forbids any scheme name outside this catalog.
- **Grounding truth:** the real **1408-row** Fallacies CSV (PK + `text_fr` + `desc_fr` + `example_fr` + family). The 12 targets are taken directly from the no-AIF depth-3 set.
- **ASPIC+ coherence as hard constraint:** every row's `attack_type` ↔ `attacked_component` must match the ASPIC+ map.
- **Active discrimination:** the `why_not_others` field forces the model to explain why the other two attack types do NOT apply — preventing collapse to a single type (the failure mode that produced all-11-undercut in phase 1's naive run).
- **Verification (three independent layers, all re-checked against the REAL corpus, not the prompt catalog):**
  1. *Catalog membership* — `RA_scheme` ∈ the 24-scheme set; `attack_type` ∈ {undermine,undercut,rebut}.
  2. *Ground-truth* — every `fallacy_pk` re-verified against the real 1408-row CSV; family + name cross-checked; **all 12 confirmed no-`AIF_skosDirectRef`** (no scope collision with the existing layer).
  3. *ASPIC+ coherence* — every row's `attack_type` ↔ `attacked_component` matches the map.
- **Result: 12/12 annotated, 0 violations across all three layers.** [PASS]
- **Anti-fab validator #518** (`tools/validate_taxonomy_annotations.py`, kind=aif-scaleup): **`✓ CLEAN — 12 rows, 0 HARD, 0 WARN`.**

Generation/verify script: `tmp/498_phase2_gen.py` (ephemeral, not committed). Dataset: `tmp/498_phase2_dataset.json` (ephemeral). Raw model output: `tmp/498_phase2_gen_result.json` (ephemeral).

## 5. The 12 annotations

| PK | Fallacy | RA scheme | attack | CA node |
|----|---------|-----------|--------|---------|
| 55 | Sauvetage ad hoc | Argument from Cause to Effect | **undermine** | AdHocPremise |
| 96 | Appel à la nature | Argument from Values | **undercut** | NaturalisticValueTransfer |
| 112 | Sophisme moraliste | Argument from Values | **undercut** | MoralFactInference |
| 134 | Sophisme du jeu | Argument from Analogy | **undercut** | OversimplifiedAnalogy |
| 153 | Argument des mauvaises raisons | Argument from Bias | **undercut** | BiasDoesNotRefuteClaim |
| 165 | Manque de parcimonie | Argument from Sign | **undermine** | UnparsimoniousPremise |
| 596 | Échantillon biaisé | Argument from Example | **undermine** | UnrepresentativeSample |
| 644 | Probabilités faussées | Argument from Sign | **undermine** | BaseRateNeglect |
| 658 | Infini fallacieux | Argument from Slippery Slope | **undercut** | UnwarrantedContinuum |
| 667 | Imprécision | Argument from Sign | **undermine** | ImpreciseData |
| 681 | Erreur de calcul | Argument from Rule | **undermine** | CalculationError |
| 690 | Opération inappropriée | Argument from Gradualism | **undercut** | InvalidMathematicalModel |

Full per-fallacy detail (violated CQ, why-not-the-others, justification, AIF `_Inference`/`_Conflict` node forms): [`498-scaleup-phase2-annotations.csv`](498-scaleup-phase2-annotations.csv).

**Attack-type distribution: undermine 6, undercut 6, rebut 0.** This **confirms the phase-1 prediction** — phase 2 shifts toward undermine (the evidence/premise families supply bad premises) — but with genuine per-case discrimination, not family-level collapse:

- **Erreur mathématique: undermine 4 / undercut 2.** The data/premise fallacies (Échantillon biaisé `UnrepresentativeSample`, Probabilités faussées `BaseRateNeglect`, Imprécision `ImpreciseData`, Erreur de calcul `CalculationError`) are all **undermine** — the premise (the datum) is false/unrepresentative/imprecise/wrong, but the inference rule (Example/Sign/Rule) is legitimate. The two undercuts (Infini fallacieux `UnwarrantedContinuum`, Opération inappropriée `InvalidMathematicalModel`) are **inference-rule** problems — the continuum/model does not license the conclusion.
- **Insuffisance: undermine 2 / undercut 4.** Counter-intuitively *not* undermine-dominant — because the Insuffisance fallacies here are not "bad data" but **bad inference rules**: Appel à la nature (`NaturalisticValueTransfer` — the value-transfer rule doesn't license "natural → good"), Sophisme moraliste (`MoralFactInference` — the is→ought rule), Sophisme du jeu (`OversimplifiedAnalogy` — the analogy rule), Mauvaises raisons (`BiasDoesNotRefuteClaim` — bias doesn't defeat the claim's rule). The two undermines (Sauvetage ad hoc `AdHocPremise`, Manque de parcimonie `UnparsimoniousPremise`) are the genuine premise problems.

The `why_not_others` field documents the discrimination per case — e.g. pk 596 Échantillon biaisé: *"Ce n'est ni un undercut, car l'inférence par exemple reste légitime si l'échantillon est représentatif, ni un rebut, car aucune contre-conclusion directe n'est produite"* (the Example rule is fine; only the premise is bad → undermine). pk 96 Appel à la nature: *"Ce n'est pas un undermine car la naturalité alléguée peut être factuellement vraie, ni un rebut car aucune valeur opposée n'est directement démontrée"* (the premise "natural" may be true; the rule "natural→valuable" fails → undercut).

**Phase-1 prediction status:** the family-level prediction (undermine-shift) was **half-right** — Erreur mathématique shifted to undermine (4/6) as predicted, but Insuffisance inverted to undercut (4/6) because its fallacies are rule-applicability errors, not data errors. This is the triple-AIF's value: it discriminates *within* a family by actual defeat mode, not by family label. Phase 3 (Obstruction/Influence/Tricherie) is now expected to surface **rebut** (counter-conclusions, appeals) — the third attack type, absent from phases 1–2.

## 6. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Fallacies - Taxonomy.csv` (no `AIF_skosDirectRef` cells filled for the 12).
- ❌ Does **not** touch the OWL ontology, cards, mindmaps, or any consumer.
- ❌ Does **not** complete the 44 no-AIF depth-3 — **phase 3** (Obstruction + Influence + Tricherie, 21 fallacies, expected rebut-bearing) follows.

## 7. Proposed next steps (gated on jsboige)

1. **jsboige validates** (a) the 12 specific triple-AIF classifications (§5), especially the Insuffisance-cut inversion (predicted undermine, got undercut — is the rule-applicability reading correct?), (b) the CA-node naming consistency with phase 1's AIFdb convention.
2. **ai-01 spot-checks** structure (catalog membership, ASPIC+ coherence, no-AIF grounding, why_not_others discrimination).
3. On approval: **phase 3** (Obstruction + Influence + Tricherie, 21 fallacies, expected rebut-bearing), completing the 44 no-AIF depth-3.
4. **Final gated step:** write the `AIF_skosDirectRef` cells to the production CSV (the `AIF_RA_node`/`AIF_CA_node` forms) only after the full 44-pass + ai-01 spot-check + jsboige nod.

---

*GATED proposal. Worker signals structure + grounding + discrimination; ai-01 reviews, jsboige validates content. No production data changed.*
