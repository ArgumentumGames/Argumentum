# #499 — Virtues Scale-up, Phase 2 (batch 1): Langage exact depth-3…4 leaves (GATED proposal)

**Issue:** [#499 — Virtues parity with Fallacies relational + AIF layers](https://github.com/ArgumentumGames/Argumentum/issues/499)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-18
**Base:** master `cae93dc8`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Virtues CSV change until jsboige approves.** This document + the annotation CSV are the proposition.

**Calibration:** [`499-virtues-scaleup-phase1.md`](499-virtues-scaleup-phase1.md) + [`499-scaleup-phase1-annotations.csv`](499-scaleup-phase1-annotations.csv) — Phase 1 (18 depth-2 sub-families) **merged** (`0ad40259`) after jsboige's interactive nod 2026-06-18. Phase 2 uses the **same method, same schema, same anti-fab guarantee**, extending the annotation one layer deeper.

---

## 1. Scope of Phase 2

Phase 1 completed the **depth-2 structural backbone** (pilot + 18 sub-families). Phase 2 begins the **depth-3…7 leaf layer** — the 194 remaining Virtue nodes that carry no annotation yet.

This is **Phase 2 batch 1: the `Langage exact` family** (smallest family, 14 nodes, depth-3 and depth-4 only). It is a **calibration batch**: same granularity question as Phase 1, but now answered for *leaves* (a depth-4 virtue like "Ponctuation correcte" should oppose a more *specific* fallacy than its depth-2 anchor did). If the granularity holds, batches for the 6 remaining families follow in subsequent ticks.

| Virtue family | depth-3…7 leaves (Phase 2 scope) | this batch | prevented Fallacy family |
|---|---|---|---|
| **Langage exact** | **14** (9 d3 + 5 d4) | ✅ this batch | Abus de langage (798) |
| Raisonnement valide | 51 | next | Erreur de raisonnement (696) |
| Échange enrichissant | 40 | next | Obstruction (1280) |
| Argument pertinent | 29 | next | Insuffisance (1) |
| Honnêteté intellectuelle | 23 | next | Tricherie (887) |
| Présentation intègre | 21 | next | Tricherie (887) |
| Rigueur mathématique | 16 | next | Erreur mathématique (594) |
| **Total remaining** | **194** | | |

## 2. Schema — identical 10-col presentation schema (Phase 1, unchanged)

```
virtue_pk, virtue_title, prevented_family_pk, prevented_family_name,
crossLink_Opposes, opposed_fallacies_readable,
AIF_skosDirectRef, AIF_skosMappingType, link_type, justification
```

Same naming notes as Phase 1 (`AIF_skosDirectRef` = Walton scheme; `AIF_skosMappingType` = CQ restored). The eventual 12-col prod write is the separate final gated step.

## 3. The inverse paradigm (honored per leaf, same as Phase 1)

A virtue is the **good holding of a scheme** — the **correct answer to the CQ** the corresponding fallacy violates. The Phase-2 leaves inherit their family's mirror (`Langage exact` → `Abus de langage` 798) and each opposes 1–2 specific real depth-3 fallacies. The granularity refinement for leaves: a depth-4 virtue opposes the *most specific* applicable fallacy under the family (e.g. "Ponctuation correcte" → `Amphibologie` 847, not a broad ambiguity).

## 4. Method & anti-fabrication guarantee (identical to Phase 1)

- **Model:** gpt-5.5 (OpenAI direct, key live 2026-06-18). `POST /v1/responses`, `reasoning:{effort:"low"}`, `max_output_tokens:4500`, no `temperature`. Answer read from `output[].content[].text`.
- **Grounding catalog:** the **7 Fallacy family PKs** + **63 named depth-3 fallacies** (9 per family), reused verbatim from the Phase-1 dataset. The prompt forbids referencing any PK outside this catalog.
- **Family mirror as hard constraint:** every `Langage exact` leaf's `prevented_family_pk` is fixed to **798**.
- **Verification (three independent layers, all re-checked against the REAL corpus, not the prompt catalog):**
  1. *Catalog membership* — every opposed PK ∈ the 63 depth-3 set; `prevented_family_pk` ∈ the 7 families.
  2. *Ground-truth* — every PK re-verified against the real **1408-row** Fallacies CSV; opposed-PK ↔ `text_fr` and family-PK ↔ name cross-checked character-for-character. (Note: the Fallacies corpus uses `text_fr` as its title field, not `title_fr` — verified.)
  3. *Mirror consistency* — every row's `prevented_family_pk` equals the pilot §3 mirror for `Langage exact` (= 798).
- **Result: 14/14 annotated, 0 violations across all three layers.** [PASS]
- **Anti-fab validator #518** (`tools/validate_taxonomy_annotations.py`, kind=virtues): **`✓ CLEAN — 14 rows, 0 HARD, 0 WARN`.**

Generation/verify script: `tmp/499_phase2_gen.py` (ephemeral, not committed — derived from the Phase-1 `tmp/499_gen.py`). Dataset: `tmp/499_phase2_dataset_5.json` (ephemeral). Raw model output: `tmp/499_phase2_gen_result.json` (ephemeral).

## 5. The 14 rows (full set in the CSV)

| Virtue (pk, depth) | → Fam. (PK) | Opposes (PK = fallacy) | Walton scheme | CQ restored |
|---|---|---|---|---|
| Acceptions claires (136, d3) | Abus langage (798) | 800 Acception vague · 855 Équivoque | Verbal Classification | L'acception des termes est-elle claire et univoque ? |
| Langage précis (137, d4) | Abus langage (798) | 855 Équivoque · 804 Acception arbitraire | Verbal Classification | La terminologie est-elle employée avec cohérence ? |
| Langage concis (138, d4) | Abus langage (798) | 667 Imprécision · 1345 Complication exagérée | Verbal Classification | Le propos est-il exprimé sans surcharge inutile ? |
| Définition conforme (139, d3) | Abus langage (798) | 804 Acception arbitraire · 800 Acception vague | Verbal Classification | La définition repose-t-elle sur une acception reconnue ? |
| Terminologie technique (140, d4) | Abus langage (798) | 804 Acception arbitraire · 855 Équivoque | Expert Opinion | L'usage des termes techniques est-il correct pour le domaine ? |
| Définition cohérente (141, d3) | Abus langage (798) | 826 Définition incohérente · 777 Inconsistance | Verbal Classification | La définition est-elle libre de contradiction ? |
| Comparaison adéquate (143, d3) | Abus langage (798) | 834 Comparaison abusive · 839 Fausse analogie | Analogy | La comparaison met-elle en lumière des similitudes réelles ? |
| Analogie appropriée (144, d3) | Abus langage (798) | 839 Fausse analogie · 834 Comparaison abusive | Analogy | L'analogie éclaire-t-elle le concept sans le distordre ? |
| Association pertinente (145, d3) | Abus langage (798) | 844 Sophisme d'association · 1371 Sophisme génétique | Sign | L'association repose-t-elle sur un lien logique pertinent ? |
| Syntaxe claire (147, d3) | Abus langage (798) | 847 Amphibologie · 667 Imprécision | Verbal Classification | La structure des phrases évite-t-elle toute ambiguïté ? |
| Grammaire correcte (148, d4) | Abus langage (798) | 847 Amphibologie · 667 Imprécision | Verbal Classification | La correction grammaticale soutient-elle la clarté ? |
| Ponctuation correcte (149, d4) | Abus langage (798) | 847 Amphibologie · 876 Ambiguïté narrative | Verbal Classification | La ponctuation dissipe-t-elle toute ambiguïté ? |
| Expressions univoques (150, d3) | Abus langage (798) | 855 Équivoque · 800 Acception vague | Verbal Classification | Les termes ont-ils une seule acception claire ? |
| Narration claire (151, d3) | Abus langage (798) | 876 Ambiguïté narrative · 667 Imprécision | Witness Testimony | Le récit est-il dépourvu d'ambiguïté contextuelle ? |

Machine-readable: [`499-scaleup-phase2-annotations.csv`](499-scaleup-phase2-annotations.csv).

## 6. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Virtues - Taxonomy.csv`. Awaiting jsboige approval.
- ❌ Does **not** touch OWL, EPITA consumer, cards, or mindmaps.
- ❌ Does **not** claim the content is final — this is batch 1 (Langage exact) of the depth-3…7 layer; 6 families (180 nodes) remain for subsequent batches.

## 7. Proposed next steps (gated on jsboige)

1. **jsboige validates** the 14 Langage-exact rows (paradigm + content + depth-leaf granularity).
2. **Phase 2 batches 2+** — the 6 remaining families (180 depth-3…7 nodes), same method + anti-fab, in subsequent cron ticks. Raisonnement valide (51) and Échange enrichissant (40) are the largest.
3. On approval of the full Phase-2 set: the gated prod write (12-col Virtues CSV population) proceeds, same as the Phase-1 plan.

---

*GATED proposal. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
