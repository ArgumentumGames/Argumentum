# #499 — Virtues Scale-up, Phase 2 (batch 6b): Échange enrichissant depth-3…5 leaves — sub-batch B (pks 200–222) (GATED proposal)

**Issue:** [#499 — Virtues parity with Fallacies relational + AIF layers](https://github.com/ArgumentumGames/Argumentum/issues/499)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-19
**Base:** master `f17d8e6e`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Virtues CSV change until jsboige approves.** This document + the annotation CSV are the proposition.

**Calibration:** [`499-virtues-scaleup-phase2-echange-a.md`](499-virtues-scaleup-phase2-echange-a.md) — batch 6a (pks 181–199, 18 leaves, **merged `f17d8e6e`**) was the first sub-batch of the Échange enrichissant family. Batch 6b completes the family (pks 200–222, 22 leaves). Same method, same schema, same anti-fab guarantee.

---

## 1. Scope of batch 6b

Phase 2 annotates the **depth-3…7 leaf layer** — the 194 remaining Virtue nodes that carry no annotation yet. This is **batch 6b: the second half of the `Échange enrichissant` family** (segment 7, pks 200–222, 22 nodes: 7 depth-3 + 13 depth-4 + 2 depth-5). It mirrors `Obstruction` (Fallacy family PK 1280) — the family of discussion-obstructing fallacies. **With 6a merged, batch 6b completes the Échange enrichissant family** (40/40 leaves).

| Virtue family | depth-3…7 leaves | status |
|---|---|---|
| Langage exact | 14 | ✅ batch 1 merged (`fc2013fc`) |
| Rigueur mathématique | 16 | ✅ batch 2 merged (`62a28efc`) |
| Honnêteté intellectuelle | 23 | ✅ batch 3 merged (`95068581`) |
| Présentation intègre | 21 | ✅ batch 4 merged (`79452d71`) |
| Argument pertinent | 29 | ✅ batch 5 merged (`3c5e62da`) |
| **Échange enrichissant** | **40** (6a=18 + 6b=22) | **6a merged (`d15ed44d`) · 6b this batch** |
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

A virtue is the **good holding of a scheme** — the **correct answer to the CQ** the corresponding fallacy violates. The leaves inherit their family's mirror (`Échange enrichissant` → `Obstruction` 1280) and each opposes 1–2 specific real depth-3 fallacies. Batch 6b is the **respect/civility** cluster (pks 200–222: respect du sujet, principe de charité, critique axée sur les arguments, courtoisie, communication non violente, ton respectueux…). These virtues are the *good holding of intellectual commitment* to fair, respectful engagement — opposing the disqualifying fallacies (`Attaque personnelle`, `Empoisonnement du puits`, `Repoussoir`, `Évasion`).

## 4. Method & anti-fabrication guarantee (identical to Phase 1)

- **Model:** gpt-5.5 (OpenAI direct, key live 2026-06-19). `POST /v1/responses`, `reasoning:{effort:"low"}`, `max_output_tokens:7000`, no `temperature`. Answer read from `output[].content[].text`.
- **Grounding catalog:** the **7 Fallacy family PKs** + **63 named depth-3 fallacies** (9 per family), reused verbatim from the Phase-1 dataset. The prompt forbids referencing any PK outside this catalog.
- **Family mirror as hard constraint:** every `Échange enrichissant` leaf's `prevented_family_pk` is fixed to **1280**.
- **Verification (three independent layers, all re-checked against the REAL corpus, not the prompt catalog):**
  1. *Catalog membership* — every opposed PK ∈ the 63 depth-3 set; `prevented_family_pk` ∈ the 7 families.
  2. *Ground-truth* — every PK re-verified against the real **1408-row** Fallacies CSV; opposed-PK ↔ `text_fr` and family-PK ↔ name cross-checked character-for-character.
  3. *Mirror consistency* — every row's `prevented_family_pk` equals the pilot §3 mirror for `Échange enrichissant` (= 1280).
- **Result: 22/22 annotated, 0 violations across all three layers.** [PASS]
- **Anti-fab validator #518** (`tools/validate_taxonomy_annotations.py`, kind=virtues): **`✓ CLEAN — 22 rows, 0 HARD, 0 WARN`.**

Generation/verify script: `tmp/499_phase2_gen7.py` (ephemeral, not committed — derived from the Phase-1 `tmp/499_gen.py`). Dataset: `tmp/499_phase2_dataset_7.json` (ephemeral). Raw model output: `tmp/499_phase2_gen_6b.json` (ephemeral).

## 5. The 22 rows (full set in the CSV)

| Virtue (pk, depth) | → Fam. (PK) | Opposes (PK = fallacy) | Walton scheme | CQ restored |
|---|---|---|---|---|
| Respect du sujet (200, d3) | Obstruction (1280) | 1313 Évasion · 1297 Preuve par assertion | Commitment | L'interlocuteur traite-t-il le sujet engagé ? |
| Principe de clarté (201, d4) | Obstruction (1280) | 1345 Complication exagérée · 1287 Pseudo-explication | Verbal Classification | Les termes rendent-ils la thèse compréhensible ? |
| Éviter les digressions (202, d4) | Obstruction (1280) | 1313 Évasion · 1345 Complication exagérée | Commitment | La contribution reste-t-elle liée à la question ? |
| Concentration sur l'essentiel (203, d3) | Obstruction (1280) | 1345 Complication exagérée · 1313 Évasion | Sign | Les éléments retenus sont-ils pertinents ? |
| Gérer le temps imparti (204, d4) | Obstruction (1280) | 1345 Complication exagérée · 1313 Évasion | Consequences | La priorité permet-elle d'atteindre l'objectif ? |
| Principe de charité (205, d3) | Obstruction (1280) | 322 Repoussoir · 1361 Procès en incohérence | Commitment | L'argument adverse est-il interprété charitablement ? |
| Présomption de bonne foi (206, d4) | Obstruction (1280) | 1352 Empoisonnement du puits · 1398 Attaque personnelle | Bias | Y a-t-il des raisons de conclure à la mauvaise foi ? |
| Évaluation loyale de la position adverse (208, d3) | Obstruction (1280) | 322 Repoussoir · 1398 Attaque personnelle | Commitment | La critique vise-t-elle la position réellement défendue ? |
| Critique axée sur les arguments (209, d4) | Obstruction (1280) | 1398 Attaque personnelle · 1352 Empoisonnement du puits | Commitment | La critique porte-t-elle sur les raisons, pas la personne ? |
| Reconnaissance de la position adverse (210, d4) | Obstruction (1280) | 322 Repoussoir · 1313 Évasion | Commitment | La position attribuée correspond-elle aux engagements ? |
| Réfutation respectueuse (211, d4) | Obstruction (1280) | 322 Repoussoir · 1361 Procès en incohérence | Commitment | La réfutation répond-elle aux engagements réels ? |
| Respect de l’origine des idées (212, d3) | Obstruction (1280) | 1371 Sophisme génétique · 942 Fausse attribution | Commitment | L'origine est-elle reconnue sans remplacer l'évaluation ? |
| Principe de non-disqualification (213, d4) | Obstruction (1280) | 1371 Sophisme génétique · 1352 Empoisonnement du puits | Bias | L'origine suffit-elle à invalider l'argument ? |
| Valorisation de la provenance (214, d4) | Obstruction (1280) | 942 Fausse attribution · 1371 Sophisme génétique | Witness Testimony | La provenance est-elle attribuée fidèlement ? |
| Collaboration argumentative (215, d4) | Obstruction (1280) | 1313 Évasion · 1282 Relativisme abusif | Commitment | Construit-on ensemble plutôt que de bloquer ? |
| Courtoisie dans le désaccord (216, d3) | Obstruction (1280) | 1398 Attaque personnelle · 1352 Empoisonnement du puits | Commitment | Le désaccord est-il compatible avec le dialogue ? |
| Éviter les attaques personnelles (217, d4) | Obstruction (1280) | 1398 Attaque personnelle · 1352 Empoisonnement du puits | Bias | Les traits personnels sont-ils pertinents ? |
| Communication non violente (218, d5) | Obstruction (1280) | 1398 Attaque personnelle · 420 Jeu de pouvoir | Commitment | La réponse respecte-t-elle l'interlocuteur ? |
| Équité dans le dialogue (219, d4) | Obstruction (1280) | 420 Jeu de pouvoir · 1313 Évasion | Commitment | Chacun peut-il exprimer ses engagements ? |
| Ne pas interrompre (220, d5) | Obstruction (1280) | 420 Jeu de pouvoir · 1313 Évasion | Commitment | L'interlocuteur peut-il formuler sa position ? |
| Ton respectueux (221, d4) | Obstruction (1280) | 1398 Attaque personnelle · 1352 Empoisonnement du puits | Commitment | Le ton permet-il l'examen rationnel ? |
| Engagement mutuel (222, d4) | Obstruction (1280) | 1313 Évasion · 1282 Relativisme abusif | Commitment | Les interlocuteurs restent-ils engagés dans la recherche ? |

**Scheme distribution (Commitment-dominant, even stronger than 6a):** Commitment ×15, Bias ×3, Verbal Classification / Sign / Consequences / Witness Testimony ×1 each. The **Commitment dominance (15/22)** is the respect/civility signature: these virtues (critique axée sur les arguments, réfutation respectueuse, courtoisie, ton respectueux, équité…) are the *good holding of the commitment to fair engagement* — keeping the discussion on the arguments, not the person. The most-opposed fallacies form the **disqualification cluster**: `Attaque personnelle` 1398 (×7), `Évasion` 1313 (×6), `Empoisonnement du puits` 1352 (×6), `Repoussoir` 322 (×4), `Jeu de pouvoir` 420 (×3) — together the family's semantic anchor for 6b: an enriching exchange requires criticism aimed at arguments (not the person), genuine engagement (not evasion/imposition), and charitable interpretation (not strawman/disqualification). Depth reaches d5 (pk 218, pk 220). Combined with 6a (Évasion ×10, Jeu de pouvoir ×4), the full Échange enrichissant family (40 leaves) is anchored on engagement-vs-obstruction — exactly the mirror of `Obstruction` 1280.

Machine-readable: [`499-scaleup-phase2-echange-b-annotations.csv`](499-scaleup-phase2-echange-b-annotations.csv).

## 6. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Virtues - Taxonomy.csv`. Awaiting jsboige approval.
- ❌ Does **not** touch OWL, EPITA consumer, cards, or mindmaps.
- ❌ Does **not** complete Phase 2 — **Raisonnement valide (51 leaves)** remains, to be sub-batched next.

## 7. Proposed next steps (gated on jsboige)

1. **jsboige validates** the 22 Échange-B rows (paradigm + content + depth-leaf granularity).
2. **Batch 7 = Raisonnement valide (51 leaves)** — the last and largest family, sub-batched (~3 sub-batches of ~17) in subsequent ticks.
3. On approval of the full Phase-2 set: the gated prod write (12-col Virtues CSV population) proceeds, same as the Phase-1 plan.

---

*GATED proposal. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
