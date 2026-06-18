# #499 — Virtues Scale-up, Phase 2 (batch 6a): Échange enrichissant depth-3…6 leaves — sub-batch A (pks 181–199) (GATED proposal)

**Issue:** [#499 — Virtues parity with Fallacies relational + AIF layers](https://github.com/ArgumentumGames/Argumentum/issues/499)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-18
**Base:** master `62a28efc`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Virtues CSV change until jsboige approves.** This document + the annotation CSV are the proposition.

**Calibration:** [`499-virtues-scaleup-phase2.md`](499-virtues-scaleup-phase2.md) — batches 1–5 validated the Phase-2 leaf method. Batch 6 is the first **sub-batched** family: Échange enrichissant has 40 leaves, too large for one gpt-5.5 generation (batch 5 truncated at 4500 tokens). Split into **6a (pks 181–199, 18 leaves, epistemic-honesty virtues)** and **6b (pks 200–222, 22 leaves, focus/respect/civility virtues)**. Same method, same schema, same anti-fab guarantee.

---

## 1. Scope of batch 6a

Phase 2 annotates the **depth-3…7 leaf layer** — the 194 remaining Virtue nodes that carry no annotation yet. This is **batch 6a: the first half of the `Échange enrichissant` family** (segment 7, pks 181–199, 18 nodes: 5 depth-3 + 9 depth-4 + 3 depth-5 + 1 depth-6). It mirrors `Obstruction` (Fallacy family PK 1280) — the family of discussion-obstructing fallacies (evasion, power play, poisoning the well, selective attention, bare assertion).

| Virtue family | depth-3…7 leaves | status |
|---|---|---|
| Langage exact | 14 | ✅ batch 1 merged (`fc2013fc`) |
| Rigueur mathématique | 16 | ✅ batch 2 merged (`62a28efc`) |
| Honnêteté intellectuelle | 23 | ⏳ batch 3 (PR #534) |
| Présentation intègre | 21 | ⏳ batch 4 (PR #535) |
| Argument pertinent | 29 | ⏳ batch 5 (PR #537) |
| **Échange enrichissant** | **40** (6a=18 + 6b=22) | **6a this batch · 6b next** |
| Raisonnement valide | 51 | next (sub-batched) |
| **Total remaining** | **194** | |

## 2. Schema — identical 10-col presentation schema (Phase 1, unchanged)

```
virtue_pk, virtue_title, prevented_family_pk, prevented_family_name,
crossLink_Opposes, opposed_fallacies_readable,
AIF_skosDirectRef, AIF_skosMappingType, link_type, justification
```

Same naming notes as Phase 1 (`AIF_skosDirectRef` = Walton scheme; `AIF_skosMappingType` = CQ restored; `link_type` = `crossLink_Opposes`). The eventual 12-col prod write is the separate final gated step.

## 3. The inverse paradigm (honored per leaf, same as Phase 1)

A virtue is the **good holding of a scheme** — the **correct answer to the CQ** the corresponding fallacy violates. The leaves inherit their family's mirror (`Échange enrichissant` → `Obstruction` 1280) and each opposes 1–2 specific real depth-3 fallacies. The epistemic-honesty cluster (pks 183–198) centers on `Évasion` 1313 — the most specific opponent for a virtue about *staying engaged with the argument rather than evading*: a good intellectual commitment is the correct holding of the Commitment scheme, opposing its evasion.

## 4. Method & anti-fabrication guarantee (identical to Phase 1)

- **Model:** gpt-5.5 (OpenAI direct, key live 2026-06-18). `POST /v1/responses`, `reasoning:{effort:"low"}`, `max_output_tokens:7000` (raised from 4500 per the batch-5 truncation fix), no `temperature`. Answer read from `output[].content[].text`.
- **Grounding catalog:** the **7 Fallacy family PKs** + **63 named depth-3 fallacies** (9 per family), reused verbatim from the Phase-1 dataset. The prompt forbids referencing any PK outside this catalog.
- **Family mirror as hard constraint:** every `Échange enrichissant` leaf's `prevented_family_pk` is fixed to **1280**.
- **Verification (three independent layers, all re-checked against the REAL corpus, not the prompt catalog):**
  1. *Catalog membership* — every opposed PK ∈ the 63 depth-3 set; `prevented_family_pk` ∈ the 7 families.
  2. *Ground-truth* — every PK re-verified against the real **1408-row** Fallacies CSV; opposed-PK ↔ `text_fr` and family-PK ↔ name cross-checked character-for-character.
  3. *Mirror consistency* — every row's `prevented_family_pk` equals the pilot §3 mirror for `Échange enrichissant` (= 1280).
- **Result: 18/18 annotated, 0 violations across all three layers.** [PASS]
- **Anti-fab validator #518** (`tools/validate_taxonomy_annotations.py`, kind=virtues): **`✓ CLEAN — 18 rows, 0 HARD, 0 WARN`.**

Generation/verify script: `tmp/499_phase2_gen7.py` (ephemeral, not committed — derived from the Phase-1 `tmp/499_gen.py`). Dataset: `tmp/499_phase2_dataset_7.json` (ephemeral). Raw model output: `tmp/499_phase2_gen_6a.json` (ephemeral).

## 5. The 18 rows (full set in the CSV)

| Virtue (pk, depth) | → Fam. (PK) | Opposes (PK = fallacy) | Walton scheme | CQ restored |
|---|---|---|---|---|
| Réalité objective (181, d3) | Obstruction (1280) | 1287 Pseudo-explication · 1297 Preuve par assertion | Sign | Les faits invoqués sont-ils objectifs et vérifiables ? |
| Explications suffisantes (182, d3) | Obstruction (1280) | 1287 Pseudo-explication · 1345 Complication exagérée | Cause to Effect | L'explication établit-elle le lien pertinent ? |
| Justifications explicites (183, d4) | Obstruction (1280) | 1297 Preuve par assertion · 1313 Évasion | Commitment | Les raisons sont-elles explicites et discutables ? |
| Preuves suffisantes (184, d4) | Obstruction (1280) | 1297 Preuve par assertion · 974 Exigence renforcée | Witness Testimony | Les preuves sont-elles suffisantes et proportionnées ? |
| Prémisses réfutables (185, d4) | Obstruction (1280) | 1282 Relativisme abusif · 1313 Évasion | Commitment | Les prémisses peuvent-elles être contestées ? |
| Ouverture au dialogue (186, d3) | Obstruction (1280) | 1313 Évasion · 1352 Empoisonnement du puits | Commitment | L'interlocuteur prend-il en compte les objections ? |
| Recherche de la vérité (187, d4) | Obstruction (1280) | 953 Attention sélective · 1242 Biais théoriques | Bias | L'évaluation est-elle sans sélection partiale ? |
| Quête de consensus (188, d5) | Obstruction (1280) | 420 Jeu de pouvoir · 1313 Évasion | Commitment | Cherche-t-on une issue acceptable pour tous ? |
| Suspension du jugement (189, d4) | Obstruction (1280) | 759 Conclusion hâtive · 953 Attention sélective | Sign | Faut-il suspendre le jugement ? |
| Écoute active (190, d5) | Obstruction (1280) | 1313 Évasion · 1398 Attaque personnelle | Commitment | La position adverse est-elle comprise avant discussion ? |
| Empathie (191, d6) | Obstruction (1280) | 1398 Attaque personnelle · 1352 Empoisonnement du puits | Bias | L'attitude permet-elle d'évaluer sans hostilité ? |
| Principe de faillibilité (192, d4) | Obstruction (1280) | 1282 Relativisme abusif · 1297 Preuve par assertion | Commitment | L'engagement reste-t-il révisable ? |
| Identifier ses biais (193, d5) | Obstruction (1280) | 1024 Biais naturels · 953 Attention sélective | Bias | Les biais sont-ils reconnus et corrigés ? |
| Reconnaître ses erreurs (194, d5) | Obstruction (1280) | 1361 Procès en incohérence · 1313 Évasion | Commitment | Les engagements erronés sont-ils corrigés ? |
| Cerner les limites d'un argument (195, d5) | Obstruction (1280) | 974 Exigence renforcée · 992 Vouloir le beurre et l’argent du beurre | Commitment | Les limites de portée sont-elles reconnues ? |
| Ouverture au changement (196, d4) | Obstruction (1280) | 1313 Évasion · 1242 Biais théoriques | Commitment | La position est-elle révisée si requis ? |
| Principe de résolution (197, d5) | Obstruction (1280) | 420 Jeu de pouvoir · 1313 Évasion | Rule | Les parties acceptent-elles la règle commune ? |
| Recherche de compromis (198, d5) | Obstruction (1280) | 420 Jeu de pouvoir · 992 Vouloir le beurre et l’argent du beurre | Values | Les concessions respectent-elles les valeurs des parties ? |

**Scheme distribution (Commitment-dominant):** Commitment ×9, Bias ×3, Sign ×2, Cause to Effect / Witness Testimony / Rule / Values ×1 each. The **Commitment dominance (9/18)** is the family's epistemic-honesty signature: these virtues (justifications explicites, prémisses réfutables, ouverture au dialogue, écoute active, faillibilité, reconnaissance d'erreurs…) are the *good holding of intellectual commitment* — staying engaged with the argument rather than evading it. The most-opposed fallacy `Évasion` 1313 appears 10 times — the family's semantic anchor: an enriching exchange requires the interlocutor actually engage (not `Évasion`), evaluate without hostility (not `Attaque personnelle` / `Empoisonnement du puits`), and not impose (not `Jeu de pouvoir`, which appears 4×). Depth reaches d6 (pk 191 Empathie). Distinct from batches 1–5 (single-scheme or source-evaluation dominant) — confirms the pipeline tracks each family's genuine topical range.

Machine-readable: [`499-scaleup-phase2-echange-a-annotations.csv`](499-scaleup-phase2-echange-a-annotations.csv).

## 6. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Virtues - Taxonomy.csv`. Awaiting jsboige approval.
- ❌ Does **not** touch OWL, EPITA consumer, cards, or mindmaps.
- ❌ Does **not** complete the Échange enrichissant family — **6b (pks 200–222, 22 leaves) follows in the next tick.** Then Raisonnement valide (51, sub-batched) remains.

## 7. Proposed next steps (gated on jsboige)

1. **jsboige validates** the 18 Échange-A rows (paradigm + content + depth-leaf granularity).
2. **Batch 6b** (pks 200–222, 22 leaves) next tick — same method + anti-fab. Then Raisonnement valide (51) sub-batched.
3. On approval of the full Phase-2 set: the gated prod write (12-col Virtues CSV population) proceeds, same as the Phase-1 plan.

---

*GATED proposal. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
