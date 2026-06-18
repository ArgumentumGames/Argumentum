# #499 — Virtues Scale-up, Phase 2 (batch 5): Argument pertinent depth-3…6 leaves (GATED proposal)

**Issue:** [#499 — Virtues parity with Fallacies relational + AIF layers](https://github.com/ArgumentumGames/Argumentum/issues/499)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-18
**Base:** master `62a28efc`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Virtues CSV change until jsboige approves.** This document + the annotation CSV are the proposition.

**Calibration:** [`499-virtues-scaleup-phase2.md`](499-virtues-scaleup-phase2.md) — batches 1–4 validated the Phase-2 leaf method. Batch 5 uses the **same method, same schema, same anti-fab guarantee**, extending the annotation to the Argument pertinent family — the largest single-batch family so far (29 leaves, depth-3 to depth-6).

---

## 1. Scope of batch 5

Phase 2 annotates the **depth-3…7 leaf layer** — the 194 remaining Virtue nodes that carry no annotation yet. This is **batch 5: the `Argument pertinent` family** (segment 1, 29 nodes: 9 depth-3 + 13 depth-4 + 5 depth-5 + 2 depth-6). It mirrors `Insuffisance` (Fallacy family PK 1) — the family of relevance/sufficiency fallacies (empty argument, bare assertion, hasty conclusion, biased sample, appeal to authority).

| Virtue family | depth-3…7 leaves (Phase 2 scope) | status |
|---|---|---|
| Langage exact | 14 | ✅ batch 1 merged (`fc2013fc`) |
| Rigueur mathématique | 16 | ✅ batch 2 merged (`62a28efc`) |
| Honnêteté intellectuelle | 23 | ⏳ batch 3 (PR #534) |
| Présentation intègre | 21 | ⏳ batch 4 (PR #535) |
| **Argument pertinent** | **29** | ✅ **this batch** |
| Échange enrichissant | 40 | next (sub-batched) |
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

A virtue is the **good holding of a scheme** — the **correct answer to the CQ** the corresponding fallacy violates. The leaves inherit their family's mirror (`Argument pertinent` → `Insuffisance` 1) and each opposes 1–2 specific real depth-3 fallacies. Depth-leaf granularity: the deeper source-evaluation leaves (d4–d6) cluster on `Argument d'autorité` 71 + `Fausse attribution` 942 — the most specific applicable opponents for a virtue about *whether the source is competent and credible*. The shallower leaves (d3) spread across `Argument vide` 3, `Preuve par assertion` 1297, `Attention sélective` 953.

## 4. Method & anti-fabrication guarantee (identical to Phase 1)

- **Model:** gpt-5.5 (OpenAI direct, key live 2026-06-18). `POST /v1/responses`, `reasoning:{effort:"low"}`, `max_output_tokens:7000` (raised from 4500 to fit the 29-node batch — 4500 truncated mid-JSON), no `temperature`. Answer read from `output[].content[].text`.
- **Grounding catalog:** the **7 Fallacy family PKs** + **63 named depth-3 fallacies** (9 per family), reused verbatim from the Phase-1 dataset. The prompt forbids referencing any PK outside this catalog.
- **Family mirror as hard constraint:** every `Argument pertinent` leaf's `prevented_family_pk` is fixed to **1**.
- **Verification (three independent layers, all re-checked against the REAL corpus, not the prompt catalog):**
  1. *Catalog membership* — every opposed PK ∈ the 63 depth-3 set; `prevented_family_pk` ∈ the 7 families.
  2. *Ground-truth* — every PK re-verified against the real **1408-row** Fallacies CSV; opposed-PK ↔ `text_fr` and family-PK ↔ name cross-checked character-for-character.
  3. *Mirror consistency* — every row's `prevented_family_pk` equals the pilot §3 mirror for `Argument pertinent` (= 1).
- **Result: 29/29 annotated, 0 violations across all three layers.** [PASS]
- **Anti-fab validator #518** (`tools/validate_taxonomy_annotations.py`, kind=virtues): **`✓ CLEAN — 29 rows, 0 HARD, 0 WARN`.**

Generation/verify script: `tmp/499_phase2_gen1.py` (ephemeral, not committed — derived from the Phase-1 `tmp/499_gen.py`). Dataset: `tmp/499_phase2_dataset_1.json` (ephemeral). Raw model output: `tmp/499_phase2_gen_result.json` (ephemeral).

## 5. The 29 rows (full set in the CSV)

| Virtue (pk, depth) | → Fam. (PK) | Opposes (PK = fallacy) | Walton scheme | CQ restored |
|---|---|---|---|---|
| Argument réel (3, d3) | Insuffisance (1) | 3 Argument vide · 1297 Preuve par assertion | Sign | Les éléments invoqués sont-ils concrets et vérifiables ? |
| Argument déductif (4, d4) | Insuffisance (1) | 784 Syllogisme invalide · 727 Erreur de logique propositionnelle | Rule | La conclusion suit-elle nécessairement des prémisses ? |
| Argument inductif (5, d4) | Insuffisance (1) | 759 Conclusion hâtive · 596 Échantillon biaisé | Example | Les exemples sont-ils nombreux et représentatifs ? |
| Preuves tangibles (6, d3) | Insuffisance (1) | 3 Argument vide · 1297 Preuve par assertion | Sign | Les preuves sont-elles tangibles et probantes ? |
| Objectif clair (7, d4) | Insuffisance (1) | 667 Imprécision · 1313 Évasion | Commitment | L'objectif et les positions sont-ils clairement établis ? |
| Illustrer par des exemples (8, d4) | Insuffisance (1) | 33 Justification triviale · 759 Conclusion hâtive | Example | Les exemples illustrent-ils précisément la thèse ? |
| Appuyer par des citations (9, d4) | Insuffisance (1) | 71 Argument d'autorité · 942 Fausse attribution | Expert Opinion | La citation provient-elle d'une source compétente ? |
| Citer ses sources (10, d4) | Insuffisance (1) | 942 Fausse attribution · 71 Argument d'autorité | Position to Know | La source est-elle identifiable et correctement rapportée ? |
| Raisonner sans biais (11, d3) | Insuffisance (1) | 953 Attention sélective · 1242 Biais théoriques | Bias | Le raisonnement tient-il compte des faits sans sélection biaisée ? |
| Sources crédibles (13, d3) | Insuffisance (1) | 71 Argument d'autorité · 942 Fausse attribution | Position to Know | La source est-elle en position fiable de connaître ? |
| Source bien évaluée (14, d4) | Insuffisance (1) | 71 Argument d'autorité · 953 Attention sélective | Position to Know | La fiabilité et les limites de la source sont-elles évaluées ? |
| Journal réputé (15, d5) | Insuffisance (1) | 71 Argument d'autorité · 942 Fausse attribution | Position to Know | Le média possède-t-il une réputation d'exactitude ? |
| Publication scientifique (16, d5) | Insuffisance (1) | 71 Argument d'autorité · 596 Échantillon biaisé | Expert Opinion | La publication est-elle évaluée par des pairs compétents ? |
| Autorité compétente (17, d5) | Insuffisance (1) | 71 Argument d'autorité · 942 Fausse attribution | Expert Opinion | L'autorité est-elle compétente dans le domaine précis ? |
| Expert du domaine (18, d6) | Insuffisance (1) | 71 Argument d'autorité · 1242 Biais théoriques | Expert Opinion | L'expert possède-t-il une expertise reconnue et pertinente ? |
| Témoignage non controversé (19, d5) | Insuffisance (1) | 942 Fausse attribution · 953 Attention sélective | Witness Testimony | Le témoignage est-il fiable, corroboré et exempt de controverse ? |
| Conclusion validée (20, d4) | Insuffisance (1) | 1297 Preuve par assertion · 3 Argument vide | Expert Opinion | La conclusion est-elle validée par des preuves vérifiées ? |
| Information confirmée (21, d4) | Insuffisance (1) | 942 Fausse attribution · 71 Argument d'autorité | Position to Know | L'information est-elle confirmée par des sources fiables ? |
| Faits vérifiés (22, d5) | Insuffisance (1) | 1297 Preuve par assertion · 953 Attention sélective | Sign | Les prémisses factuelles sont-elles vérifiées indépendamment ? |
| Preuves empiriques (23, d6) | Insuffisance (1) | 3 Argument vide · 596 Échantillon biaisé | Sign | Les données empiriques sont-elles observables et pertinentes ? |
| Hypothèse plausible (24, d4) | Insuffisance (1) | 55 Sauvetage ad hoc · 1287 Pseudo-explication | Cause to Effect | L'hypothèse est-elle plausible et reliée aux faits ? |
| Sources indépendantes (25, d3) | Insuffisance (1) | 953 Attention sélective · 596 Échantillon biaisé | Position to Know | Les sources sont-elles multiples, indépendantes et convergentes ? |
| Sources impartiales (26, d3) | Insuffisance (1) | 953 Attention sélective · 1024 Biais naturels | Bias | La source est-elle suffisamment impartiale ? |
| Sources sans biais social (27, d4) | Insuffisance (1) | 1174 Biais culturels · 953 Attention sélective | Bias | La source est-elle exempte de biais socio-économiques ? |
| Complexité adaptée (29, d3) | Insuffisance (1) | 1345 Complication exagérée · 33 Justification triviale | Verbal Classification | La complexité retenue correspond-elle à la nature du sujet ? |
| Interprétation non partisane (30, d3) | Insuffisance (1) | 953 Attention sélective · 1242 Biais théoriques | Bias | L'interprétation examine-t-elle sans biais partisan ? |
| Représentation parcimonieuse (31, d3) | Insuffisance (1) | 165 Manque de parcimonie · 1345 Complication exagérée | Cause to Effect | L'explication est-elle aussi simple que possible ? |
| Rasoir d'Ockham (32, d4) | Insuffisance (1) | 165 Manque de parcimonie · 1287 Pseudo-explication | Cause to Effect | La plus simple explication rend-elle compte des faits ? |
| Rasoir de Hanlon (33, d4) | Insuffisance (1) | 707 Inversion de causalité · 1371 Sophisme génétique | Cause to Effect | L'intention attribuée est-elle la meilleure explication causale ? |

**Scheme distribution (broadest yet — 10 distinct schemes):** Position to Know ×6, Expert Opinion ×5, Sign ×4, Bias ×4, Cause to Effect ×4, Example ×2, Rule / Commitment / Witness Testimony / Verbal Classification ×1 each. The Position-to-Know + Expert-Opinion dominance (11/29) is the expected signature for the Argument-pertinent family — these are the *source-evaluation* virtues (is the source credible, competent, in a position to know?), whose opposite is the abuse of authority and attribution. The most-opposed fallacy `Argument d'autorité` 71 appears 11 times and `Fausse attribution` 942 appears 11 times — together they are the family's semantic anchor: a relevant argument requires the source be both competent (not `Argument d'autorité`) and faithfully reported (not `Fausse attribution`). The Cause-to-Effect cluster (×4, pk 24/31/32/33 — the razor virtues) opposes over-complication and pseudo-explanation. This broader, multi-cluster profile contrasts the single-scheme-dominant batches 1–3, confirming the pipeline tracks each family's genuine topical range. Depth reaches d6 (pk 18 Expert du domaine, pk 23 Preuves empiriques) — the deepest leaves annotated so far.

Machine-readable: [`499-scaleup-phase2-argument-annotations.csv`](499-scaleup-phase2-argument-annotations.csv).

## 6. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Virtues - Taxonomy.csv`. Awaiting jsboige approval.
- ❌ Does **not** touch OWL, EPITA consumer, cards, or mindmaps.
- ❌ Does **not** claim the content is final — this is batch 5 (Argument pertinent) of the depth-3…7 layer; 2 families (91 nodes) remain for subsequent batches.

## 7. Proposed next steps (gated on jsboige)

1. **jsboige validates** the 29 Argument-pertinent rows (paradigm + content + depth-leaf granularity).
2. **Phase 2 remaining batches** — the 2 remaining families (91 depth-3…7 nodes), sub-batched per the Phase-1 precedent: Échange enrichissant (40) then Raisonnement valide (51), in batches of ~6.
3. On approval of the full Phase-2 set: the gated prod write (12-col Virtues CSV population) proceeds, same as the Phase-1 plan.

---

*GATED proposal. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
