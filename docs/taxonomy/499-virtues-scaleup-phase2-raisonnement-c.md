# #499 — Virtues Scale-up, Phase 2 (batch 7c): Raisonnement valide depth-4…5 leaves — sub-batch C (pks 128–133) (GATED proposal — FINAL batch, completes Phase 2)

**Issue:** [#499 — Virtues parity with Fallacies relational + AIF layers](https://github.com/ArgumentumGames/Argumentum/issues/499)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-19
**Base:** master `909d04c3`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Virtues CSV change until jsboige approves.** This document + the annotation CSV are the proposition.

**Calibration:** [`499-virtues-scaleup-phase2-raisonnement-b.md`](499-virtues-scaleup-phase2-raisonnement-b.md) — batch 7b (pks 105–127, the 19 depth-7 syllogistic modes) delivered (PR #541, merged `909d04c3`). This is **batch 7c: the informal/analytical cluster** (pks 128–133, 6 leaves: 1 depth-4 + 5 depth-5) — the **FINAL sub-batch** of the `Raisonnement valide` family and of **the entire Phase-2 Virtues scale-up**. With 7a + 7b merged and 7c approved, Phase 2 reaches **194/194 leaves**.

---

## 1. Scope of batch 7c (FINAL)

Phase 2 annotates the **depth-3…7 leaf layer** — the 194 remaining Virtue nodes that carry no annotation yet. This is **batch 7c: the final sub-batch of the `Raisonnement valide` family** (segment 4, pks 128–133, 6 nodes: 1 depth-4 + 5 depth-5). It mirrors `Erreur de raisonnement` (Fallacy family PK 696). Batch 7c is the **informal/analytical-reasoning** cluster: the virtues of informal logic, analytic decomposition, synthesis, critical evaluation, abductive inference, and analogical reasoning — the *non-formal* modes of valid reasoning, distinct from the formal-validity (7a) and syllogistic-form (7b) clusters.

| Virtue family | depth-3…7 leaves | status |
|---|---|---|
| Langage exact | 14 | ✅ batch 1 merged |
| Rigueur mathématique | 16 | ✅ batch 2 merged |
| Honnêteté intellectuelle | 23 | ✅ batch 3 merged |
| Présentation intègre | 21 | ✅ batch 4 merged |
| Argument pertinent | 29 | ✅ batch 5 merged |
| Échange enrichissant | 40 | ✅ batch 6 (6a+6b) merged |
| **Raisonnement valide** | **51** (7a=22 + 7b=23 + **7c=6**) | **7a+7b merged · 7c this FINAL batch** |
| **Total** | **194** | **7c = the last 6 → 194/194 complete** |

## 2. Schema — identical 10-col presentation schema (Phase 1, unchanged)

```
virtue_pk, virtue_title, prevented_family_pk, prevented_family_name,
crossLink_Opposes, opposed_fallacies_readable,
AIF_skosDirectRef, AIF_skosMappingType, link_type, justification
```

Same naming notes as Phase 1 (`AIF_skosDirectRef` = Walton scheme; `AIF_skosMappingType` = CQ restored; `link_type` = `crossLink_Opposes`). The eventual 12-col prod write is the separate final gated step.

## 3. The inverse paradigm (honored per leaf, same as Phase 1)

A virtue is the **good holding of a scheme** — the **correct answer to the CQ** the corresponding fallacy violates. The leaves inherit their family's mirror (`Raisonnement valide` → `Erreur de raisonnement` 696) and each opposes 1–2 specific real depth-3 fallacies. Batch 7c is the **informal/analytical** cluster: these virtues are the *good holding of distinct non-formal reasoning schemes* — Sign (informal/synthetic indicators), Rule (analytic decomposition), Bias-detection (critical evaluation), Cause to Effect (abduction), Analogy (analogical transfer) — opposing the reasoning errors specific to each mode (`Inconsistance`, `Sophisme de l'accident`, `Relation infondée`, `Pétition de principe`, `Inversion de causalité`, `Fausse analogie`…).

## 4. Method & anti-fabrication guarantee (identical to Phase 1)

- **Model:** gpt-5.5 (OpenAI direct, key live 2026-06-19). `POST /v1/responses`, `reasoning:{effort:"low"}`, `max_output_tokens:7000`, no `temperature`. Answer read from `output[].content[].text`.
- **Grounding catalog:** the **7 Fallacy family PKs** + **63 named depth-3 fallacies** (9 per family), reused verbatim from the Phase-1 dataset. The prompt forbids referencing any PK outside this catalog.
- **Family mirror as hard constraint:** every `Raisonnement valide` leaf's `prevented_family_pk` is fixed to **696**.
- **Verification (three independent layers, all re-checked against the REAL corpus, not the prompt catalog):**
  1. *Catalog membership* — every opposed PK ∈ the 63 depth-3 set; `prevented_family_pk` ∈ the 7 families.
  2. *Ground-truth* — every PK re-verified against the real **1408-row** Fallacies CSV (`PK` + `text_fr` columns); opposed-PK ↔ `text_fr` and family-PK ↔ name cross-checked character-for-character.
  3. *Mirror consistency* — every row's `prevented_family_pk` equals the pilot §3 mirror for `Raisonnement valide` (= 696).
- **Result: 6/6 annotated, 0 violations across all three layers.** [PASS]
- **Anti-fab validator #518** (`tools/validate_taxonomy_annotations.py`, kind=virtues): **`✓ CLEAN — 6 rows, 0 HARD, 0 WARN`.**

Generation/verify script: `tmp/499_phase2_gen4.py` (ephemeral, not committed — derived from the Phase-1 `tmp/499_gen.py`). Dataset: `tmp/499_phase2_dataset_4.json` (ephemeral). Raw model output: `tmp/499_phase2_gen_7c.json` (ephemeral).

## 5. The 6 rows (full set in the CSV)

| Virtue (pk, depth) | → Fam. (PK) | Opposes (PK = fallacy) | Walton scheme | CQ restored |
|---|---|---|---|---|
| Logique informelle solide (128, d4) | Erreur raisonn. (696) | 777 Inconsistance · 759 Conclusion hâtive | Sign | Les indices invoqués soutiennent-ils la conclusion de façon cohérente et suffisamment forte ? |
| Raisonnement analytique solide (129, d5) | Erreur raisonn. (696) | 614 Sophisme de l'accident · 621 Transfert illicite | Rule | La décomposition distingue-t-elle correctement la règle générale, ses conditions d'application et les cas particuliers ? |
| Raisonnement synthétique solide (130, d5) | Erreur raisonn. (696) | 633 Relation infondée · 777 Inconsistance | Sign | Les parties réunies forment-elles un ensemble cohérent qui justifie réellement la conclusion globale ? |
| Raisonnement critique solide (131, d5) | Erreur raisonn. (696) | 698 Pétition de principe · 727 Erreur de logique propositionnelle | Bias | L'évaluation détecte-t-elle les biais, présupposés circulaires et erreurs logiques qui affaiblissent l'argument ? |
| Abduction correcte (132, d5) | Erreur raisonn. (696) | 707 Inversion de causalité · 719 Effet cigogne | Cause to Effect | L'hypothèse proposée est-elle la cause la plus plausible des faits observés plutôt qu'une simple corrélation ? |
| Analogie correcte (133, d5) | Erreur raisonn. (696) | 839 Fausse analogie · 834 Comparaison abusive | Analogy | Les ressemblances entre les cas sont-elles pertinentes et suffisantes pour transférer la conclusion ? |

**Scheme distribution (maximally dispersed — 5 schemes across 6 leaves):** Sign ×2, Rule / Bias / Cause to Effect / Analogy ×1 each. The **maximal dispersion (5/6 distinct schemes, no dominance)** is the **informal/analytical signature** — the polar opposite of batch 7a (Rule 17/22) and 7b (Rule 23/23). Where 7a was *formal validity* (applying valid inferential Rules) and 7b was *syllogistic form* (100% Rule), 7c is *non-formal reasoning*: each virtue engages a distinct reasoning mode (informal sign-tracking, analytic decomposition, synthesis, bias-detection, abduction, analogy). The **opposed tally is equally dispersed — 11 distinct fallacies across 6 rows, no fallacy opposed more than twice** (`Inconsistance` 777 ×2 being the only repeat): an enriching analytical reasoner applies the correct mode for each problem, opposing whichever reasoning error that mode licenses — *not* a single recurring error. The CQs carry per-mode specificity: abduction (pk 132) names "the most plausible cause vs. mere correlation"; analogy (pk 133) names "relevant + sufficient similarity for transfer"; critical reasoning (pk 131) names "detecting biases, circular presuppositions, and logical errors".

**Cluster contrast across Raisonnement valide (51 leaves):** 7a = formal-inference-rule (Rule-dominant 17/22), 7b = syllogistic-form (Rule 23/23), 7c = informal/analytical (5 schemes, no dominance). Together the three sub-batches trace the family's full topical range — and confirm gpt-5.5 tracks genuine cluster semantics, not a generic template, all the way through the family.

Machine-readable: [`499-scaleup-phase2-raisonnement-c-annotations.csv`](499-scaleup-phase2-raisonnement-c-annotations.csv).

## 6. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Virtues - Taxonomy.csv`. Awaiting jsboige approval.
- ❌ Does **not** touch OWL, EPITA consumer, cards, or mindmaps.
- ❌ **Completes the proposal layer of Phase 2** (194/194 leaves annotated + verified) — but does **not** perform the gated prod write (12-col Virtues CSV population), which remains the final step gated on jsboige's content approval.

## 7. Proposed next steps (gated on jsboige)

1. **jsboige validates** the 6 Raisonnement-C rows (paradigm + content + the informal/analytical cluster granularity). This completes the content review of the **entire Phase-2 set (194 leaves across 7 families)**.
2. On approval of the full Phase-2 set: the **gated prod write** (12-col Virtues CSV population) proceeds — the final step of #499, writing `prevented_family_pk`, `crossLink_Opposes`, `AIF_skosDirectRef`, `AIF_skosMappingType`, and `justification` to the production `Argumentum Virtues - Taxonomy.csv` for all 194 leaves.
3. With Virtues Phase 2 complete (content), the **#498 AIF phase 3** lane (Obstruction + Influence + Tricherie, the rebut-bearing families) is unblocked as the next taxonomy track.

---

*GATED proposal — FINAL Phase-2 batch. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
