# #499 — Virtues Scale-up, Phase 2 (batch 6b): Échange enrichissant depth-3…5 leaves — sub-batch B (pks 200–222) (GATED proposal)

**Issue:** [#499 — Virtues parity with Fallacies relational + AIF layers](https://github.com/ArgumentumGames/Argumentum/issues/499)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-19
**Base:** master `f17d8e6e`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Virtues CSV change until jsboige approves.** This document + the annotation CSV are the proposition.

**Calibration:** [`499-virtues-scaleup-phase2-echange-a.md`](499-virtues-scaleup-phase2-echange-a.md) — batch 6a (pks 181–199, 18 leaves, **merged `f17d8e6e`**) was the first sub-batch of the Échange enrichissant family. Batch 6b completes the family (pks 200–222, 22 leaves). Same method, same schema, same anti-fab guarantee.

---

## 1. Scope of batch 6b

Phase 2 annotates the **depth-3…7 leaf layer** — the 194 remaining Virtue nodes that carry no annotation yet. This is **batch 6b: the second half of the `Échange enrichissant` family** (segment 7, pks 200–222, 22 nodes: 6 depth-3 + 14 depth-4 + 2 depth-5). It mirrors `Obstruction` (Fallacy family PK 1280) — the family of discussion-obstructing fallacies. **With 6a merged, batch 6b completes the Échange enrichissant family** (40/40 leaves).

> **Note on the pk 207 gap:** the range 200–222 spans 23 numbers but this batch has 22 rows. The missing **pk 207** is `Respect de la personne` (path `7.3`), a **depth-2 backbone sub-family** — it was annotated in **Phase 1** (the structural backbone), not a Phase-2 leaf. Phase 2 annotates only depth-≥3 leaves, so the depth-2 node at pk 207 is correctly skipped. The 22 rows here are exactly the depth-3…5 leaves of the family.

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
| Respect du sujet (200, d3) | Obstruction (1280) | 1313 Évasion · 1297 Preuve par assertion | Commitment | L'interlocuteur traite-t-il effectivement le sujet auquel il s'est engagé ? |
| Principe de clarté (201, d4) | Obstruction (1280) | 1345 Complication exagérée · 1287 Pseudo-explication | Verbal Classification | Les termes et distinctions employés rendent-ils la thèse clairement compréhensible ? |
| Éviter les digressions (202, d4) | Obstruction (1280) | 1313 Évasion · 1345 Complication exagérée | Commitment | La contribution reste-t-elle liée à la question principale en discussion ? |
| Concentration sur l'essentiel (203, d3) | Obstruction (1280) | 1345 Complication exagérée · 1313 Évasion | Sign | Les éléments retenus sont-ils les signes pertinents pour trancher la question centrale ? |
| Gérer le temps imparti (204, d4) | Obstruction (1280) | 1345 Complication exagérée · 1313 Évasion | Consequences | La priorité donnée aux points essentiels permet-elle d'atteindre utilement l'objectif du débat dans le temps disponible ? |
| Principe de charité (205, d3) | Obstruction (1280) | 322 Repoussoir · 1361 Procès en incohérence | Commitment | L'argument adverse est-il interprété selon l'engagement le plus cohérent que son auteur peut raisonnablement soutenir ? |
| Présomption de bonne foi (206, d4) | Obstruction (1280) | 1352 Empoisonnement du puits · 1398 Attaque personnelle | Bias | Existe-t-il des raisons suffisantes de conclure que l'interlocuteur est de mauvaise foi ? |
| Évaluation loyale de la position adverse (208, d3) | Obstruction (1280) | 322 Repoussoir · 1398 Attaque personnelle | Commitment | La critique vise-t-elle la position réellement défendue par l'interlocuteur ? |
| Critique axée sur les arguments (209, d4) | Obstruction (1280) | 1398 Attaque personnelle · 1352 Empoisonnement du puits | Commitment | La critique porte-t-elle sur les raisons avancées plutôt que sur la personne qui les avance ? |
| Reconnaissance de la position adverse (210, d4) | Obstruction (1280) | 322 Repoussoir · 1313 Évasion | Commitment | La position attribuée à l'interlocuteur correspond-elle fidèlement à ses engagements explicites ? |
| Réfutation respectueuse (211, d4) | Obstruction (1280) | 322 Repoussoir · 1361 Procès en incohérence | Commitment | La réfutation répond-elle précisément aux engagements réels de l'argument adverse ? |
| Respect de l’origine des idées (212, d3) | Obstruction (1280) | 1371 Sophisme génétique · 942 Fausse attribution | Commitment | L'origine d'une idée est-elle reconnue sans remplacer l'évaluation de son contenu ? |
| Principe de non-disqualification (213, d4) | Obstruction (1280) | 1371 Sophisme génétique · 1352 Empoisonnement du puits | Bias | L'origine ou l'affiliation invoquée suffit-elle réellement à invalider l'argument ? |
| Valorisation de la provenance (214, d4) | Obstruction (1280) | 942 Fausse attribution · 1371 Sophisme génétique | Witness Testimony | La provenance de l'idée est-elle attribuée fidèlement et utilisée de façon pertinente ? |
| Collaboration argumentative (215, d4) | Obstruction (1280) | 1313 Évasion · 1282 Relativisme abusif | Commitment | Les participants construisent-ils ensemble à partir de leurs engagements communs plutôt que de bloquer l'échange ? |
| Courtoisie dans le désaccord (216, d3) | Obstruction (1280) | 1398 Attaque personnelle · 1352 Empoisonnement du puits | Commitment | Le désaccord est-il exprimé de manière compatible avec la poursuite du dialogue ? |
| Éviter les attaques personnelles (217, d4) | Obstruction (1280) | 1398 Attaque personnelle · 1352 Empoisonnement du puits | Bias | Les traits personnels invoqués sont-ils pertinents pour évaluer l'argument présenté ? |
| Communication non violente (218, d5) | Obstruction (1280) | 1398 Attaque personnelle · 420 Jeu de pouvoir | Commitment | La réponse respecte-t-elle l'interlocuteur tout en traitant le fond du désaccord ? |
| Équité dans le dialogue (219, d4) | Obstruction (1280) | 420 Jeu de pouvoir · 1313 Évasion | Commitment | Chaque participant dispose-t-il d'une possibilité équitable d'exprimer ses engagements ? |
| Ne pas interrompre (220, d5) | Obstruction (1280) | 420 Jeu de pouvoir · 1313 Évasion | Commitment | L'interlocuteur peut-il formuler complètement sa position avant qu'elle soit évaluée ? |
| Ton respectueux (221, d4) | Obstruction (1280) | 1398 Attaque personnelle · 1352 Empoisonnement du puits | Commitment | Le ton employé permet-il de poursuivre l'examen rationnel du désaccord ? |
| Engagement mutuel (222, d4) | Obstruction (1280) | 1313 Évasion · 1282 Relativisme abusif | Commitment | Les interlocuteurs restent-ils engagés dans une recherche commune de vérité ou de solution ? |

**Scheme distribution (Commitment-dominant, even stronger than 6a):** Commitment ×15, Bias ×3, Verbal Classification / Sign / Consequences / Witness Testimony ×1 each. The **Commitment dominance (15/22)** is the respect/civility signature: these virtues (critique axée sur les arguments, réfutation respectueuse, courtoisie, ton respectueux, équité…) are the *good holding of the commitment to fair engagement* — keeping the discussion on the arguments, not the person.

The most-opposed fallacies (exact tally recomputed from the 44 `crossLink_Opposes` PKs) form the **engagement-vs-disqualification cluster**, anchored on genuine engagement rather than personal attack:

| Rank | Fallacy (PK) | Count |
|---|---|---|
| #1 | `Évasion` 1313 | ×9 |
| #2 | `Attaque personnelle` 1398 | ×7 |
| #3 | `Empoisonnement du puits` 1352 | ×6 |
| #4 | `Complication exagérée` 1345 · `Repoussoir` 322 | ×4 each |
| #5 | `Sophisme génétique` 1371 · `Jeu de pouvoir` 420 | ×3 each |

`Évasion` 1313 is the **true #1 (×9)** — the family's primary semantic anchor for 6b: an enriching exchange requires the interlocutor to genuinely stay engaged with the subject (not evade), then to criticise the argument rather than the person (not `Attaque personnelle` ×7 / `Empoisonnement du puits` ×6), to argue in good faith rather than disqualify by origin (not `Repoussoir` 322 ×4 / `Sophisme génétique` 1371 ×3), and to keep the exchange tractable (not `Complication exagérée` 1345 ×4) or imposed (not `Jeu de pouvoir` 420 ×3). Depth reaches d5 (pk 218, pk 220). Combined with 6a (Évasion ×10, Jeu de pouvoir ×4), the full Échange enrichissant family (40 leaves) is anchored on engagement-vs-obstruction — exactly the mirror of `Obstruction` 1280.

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
