# #497 — Cross-Links Additions Pilot (GATED proposal)

**Issue:** [#497 — inter-fallacy cross-links additions with documented TYPE + réciprocité](https://github.com/ArgumentumGames/Argumentum/issues/497)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-17
**Base:** master `5ed6e7d5`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Fallacies CSV change until jsboige approves.** This document + the pilot CSV are the proposal.

---

## 1. The gap

The Fallacies relational layer (`crossLink_*`) is **extremely sparse**: **22 non-empty cells across 1408 nodes (1.6%)**, with the 8 types unevenly used (`PredatesOn`=9, `Leverages`=4, `Mirrors`/`IsRelatedTo`/`Opposes`=2 each, `Denounces`/`Allows`/`Inverts`=1 each). po-2024 validated read-only (PR #502) that all 22 existing links resolve correctly and `Mirrors` is reciprocated (PK 673↔814) — so there is **nothing to curate without fabrication**; the need is **additions**. This pilot proposes 12 new, defensible cross-links.

## 2. Convention (verified from existing data)

- A `crossLink_*` cell holds the **target node's `path` column value** (e.g. `5.1.2.2.4`), **not** its PK. Verified: PK 814 (path `5.1.2.2.4`) `crossLink_Mirrors` = `3.3.1.2.2`, which resolves to PK 673 (Juste milieu); reciprocally PK 673 `crossLink_Mirrors` = `5.1.2.2.4`.
- **Reciprocity** = for a symmetric relation A↔B, A's cell = B's path AND B's cell = A's path.
- The 8 types: `crossLink_PredatesOn`, `crossLink_Denounces`, `crossLink_Leverages`, `crossLink_Allows`, `crossLink_Opposes`, `crossLink_Inverts`, `crossLink_Mirrors`, `crossLink_IsRelatedTo` (typo `Levarages`→`Leverages` fixed by #502).

## 3. The pilot — 12 new cross-links (1 Mirrors + 11 IsRelatedTo)

**Selection (the anti-fabrication backbone):** candidate pairs were **curated from real taxonomy structure** — (a) the canonical formal mirror pair, (b) cross-family duplicate nodes (same fallacy under two family positions), (c) cluster siblings (same conceptual sub-family). gpt-5.5 then **typed** each relation + justified it (or would have REJECTED a non-defensible pair). All 12 were accepted as defensible; 0 rejected.

| # | A | TYPE | B | Rationale (short) |
|---|---|------|---|-------------------|
| 1 | 708 Affirmation du conséquent | **crossLink_Mirrors** | 722 Négation de l'antécédent | The twin invalid conditional-inference forms (affirming Q→P vs denying P→¬Q) — the one genuine structural dual |
| 2 | 708 Affirmation du conséquent | crossLink_IsRelatedTo | 731 Affirmation du conséquent | Same fallacy under two family positions (path 4.1.2.1 vs 4.2.1.2.1) — **cross-family duplicate** |
| 3 | 722 Négation de l'antécédent | crossLink_IsRelatedTo | 729 Négation de l'antécédent | Same formal schema under two positions (4.1.3.1.1.1 vs 4.2.1.1.1) — **cross-family duplicate** |
| 4 | 703 Pétition de principe analogique | crossLink_IsRelatedTo | 840 Pétition de principe analogique | Identical mechanism (analogical begging-the-question) under two positions (4.1.1.4 vs 5.2.2.1) — **cross-family duplicate** |
| 5 | 4 Appel à l'ignorance | crossLink_IsRelatedTo | 355 Appel à l'ignorance assumée | Both exploit ignorance as the argumentative spring; variant relation |
| 6 | 319 Appel à la pitié | crossLink_IsRelatedTo | 299 Appel à l'émotion | Pity appeal is a specific form of emotion appeal (cluster) |
| 7 | 163 Appel à la colère | crossLink_IsRelatedTo | 299 Appel à l'émotion | Anger appeal is a specific form of emotion appeal (cluster) |
| 8 | 834 Comparaison abusive | crossLink_IsRelatedTo | 839 Fausse analogie | Sibling comparison-fallacies (paths 5.2.1 vs 5.2.2) |
| 9 | 634 Confusion corrélation/causalité | crossLink_IsRelatedTo | 635 Confusion antériorité/causalité | Same causal-confusion family |
| 10 | 707 Inversion de causalité | crossLink_IsRelatedTo | 719 Effet cigogne | Stork effect is a form of causal inversion |
| 11 | 642 Corrélation illusoire | crossLink_IsRelatedTo | 634 Confusion corrélation/causalité | Both misread statistical relations |
| 12 | 1352 Empoisonnement du puits | crossLink_IsRelatedTo | 1361 Procès en incohérence | Both ad-hominem-family obstruction tactics (paths 7.2.3 vs 7.3.1) |

**Notable structural finding (for jsboige):** pairs 2-4 expose that the taxonomy carries the **same fallacy under multiple family positions** (3 confirmed duplicate-name pairs). The `IsRelatedTo` links surface this duplication — jsboige may want to dedupe, merge, or keep as deliberate cross-references. The links are valuable either way.

All 24 directed cells (12 pairs × 2 reciprocal directions): [`497-pilot-crosslinks.csv`](497-pilot-crosslinks.csv).

## 4. Method & anti-fabrication guarantee

- **Curation > generation.** The candidate PAIRS come from real taxonomy structure (keyword search across the 1408 nodes for formal-logic, emotional, ad-hominem, causal, analogy, ignorance clusters). gpt-5.5 only **types + justifies** — it does not invent which fallacies relate. This bounds fabrication risk: pairs are grounded, the model labels them.
- **Conservative typing.** The model was told to **prefer symmetric types** (Mirrors/Inverts/Opposes/IsRelatedTo) for clean reciprocity and to REJECT non-defensible pairs. It reserved `crossLink_Mirrors` for the **single** genuine structural dual (708↔722) and used `IsRelatedTo` for everything else — appropriately conservative. 0 pairs rejected (all 12 defensible).
- **Model / endpoint:** gpt-5.5 via `/v1/responses` + `reasoning:{effort:"low"}` (cluster's proven reliable-content method).
- **Verification (4 layers):**
  1. both PKs ∈ real 1408-row CSV,
  2. both target paths ∈ real paths (resolve to a real node),
  3. `link_type` ∈ the 8 valid types,
  4. **novelty** — none of the 12 overlaps the existing 22 cross-links (all genuinely NEW).
- **Result: 0 fabrication violations, 0 duplicates-with-existing. PASS.** 24 net-new directed cells.

Generation script: `tmp/497_pilot_gen.py` (ephemeral). Raw output: `tmp/497_pilot_links.json`.

## 5. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Fallacies - Taxonomy.csv` (no `crossLink_*` cells filled).
- ❌ Does **not** assert the 12 are exhaustive — they are a **calibration pilot** for the curation+typing method.
- ❌ Does **not** touch the OWL ontology, cards, mindmaps, or any consumer.

## 6. Proposed next steps (gated on jsboige)

1. **jsboige validates** (a) the curation+typing method, (b) the 12 specific links in §3, (c) the duplicate-name finding (pairs 2-4) — dedupe/merge/keep-as-cross-ref decision.
2. On approval: scale the curation across the full 1408 nodes (systematic duplicate detection + cluster-sibling linking), with this pilot as the calibration exemplar. The reciprocal-path convention is now documented for the DatasetUpdater scale-up.

---

*GATED proposal. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
