# #499 — Virtues Scale-up, Phase 2 (batch 4): Présentation intègre depth-3…5 leaves (GATED proposal)

**Issue:** [#499 — Virtues parity with Fallacies relational + AIF layers](https://github.com/ArgumentumGames/Argumentum/issues/499)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-18
**Base:** master `62a28efc`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Virtues CSV change until jsboige approves.** This document + the annotation CSV are the proposition.

**Calibration:** [`499-virtues-scaleup-phase2.md`](499-virtues-scaleup-phase2.md) — batches 1-3 (Langage exact, Rigueur mathématique, Honnêteté intellectuelle) validated the Phase-2 leaf method. Batch 4 uses the **same method, same schema, same anti-fab guarantee**, extending the annotation to the Présentation intègre family.

---

## 1. Scope of batch 4

Phase 2 annotates the **depth-3…7 leaf layer** — the 194 remaining Virtue nodes that carry no annotation yet. This is **batch 4: the `Présentation intègre` family** (segment 2, 21 nodes: 9 depth-3 + 7 depth-4 + 5 depth-5). It mirrors `Tricherie` (Fallacy family PK 887) — the same family mirror as Honnêteté intellectuelle (batch 3), but a distinct Virtue family covering presentation/rhetoric rather than intellectual honesty.

| Virtue family | depth-3…7 leaves (Phase 2 scope) | status |
|---|---|---|
| Langage exact | 14 | ✅ batch 1 merged (`fc2013fc`) |
| Rigueur mathématique | 16 | ✅ batch 2 merged (`62a28efc`) |
| Honnêteté intellectuelle | 23 | ⏳ batch 3 (PR #534, ai-01 review) |
| **Présentation intègre** | **21** | ✅ **this batch** |
| Raisonnement valide | 51 | next (sub-batched) |
| Échange enrichissant | 40 | next (sub-batched) |
| Argument pertinent | 29 | next |
| **Total remaining** | **194** | |

## 2. Schema — identical 10-col presentation schema (Phase 1, unchanged)

```
virtue_pk, virtue_title, prevented_family_pk, prevented_family_name,
crossLink_Opposes, opposed_fallacies_readable,
AIF_skosDirectRef, AIF_skosMappingType, link_type, justification
```

Same naming notes as Phase 1 (`AIF_skosDirectRef` = Walton scheme; `AIF_skosMappingType` = CQ restored; `link_type` = `crossLink_Opposes`). The eventual 12-col prod write is the separate final gated step.

## 3. The inverse paradigm (honored per leaf, same as Phase 1)

A virtue is the **good holding of a scheme** — the **correct answer to the CQ** the corresponding fallacy violates. The leaves inherit their family's mirror (`Présentation intègre` → `Tricherie` 887) and each opposes 1–2 specific real depth-3 fallacies. Depth-leaf granularity: deeper leaves (d4/d5) oppose the *most specific* applicable fallacy under the family — e.g. "Vocabulaire neutre" (d5) → `Langage persuasif` 177 + `Empoisonnement du puits` 1352, not a broad bias.

## 4. Method & anti-fabrication guarantee (identical to Phase 1)

- **Model:** gpt-5.5 (OpenAI direct, key live 2026-06-18). `POST /v1/responses`, `reasoning:{effort:"low"}`, `max_output_tokens:4500`, no `temperature`. Answer read from `output[].content[].text`.
- **Grounding catalog:** the **7 Fallacy family PKs** + **63 named depth-3 fallacies** (9 per family), reused verbatim from the Phase-1 dataset. The prompt forbids referencing any PK outside this catalog.
- **Family mirror as hard constraint:** every `Présentation intègre` leaf's `prevented_family_pk` is fixed to **887**.
- **Verification (three independent layers, all re-checked against the REAL corpus, not the prompt catalog):**
  1. *Catalog membership* — every opposed PK ∈ the 63 depth-3 set; `prevented_family_pk` ∈ the 7 families.
  2. *Ground-truth* — every PK re-verified against the real **1408-row** Fallacies CSV; opposed-PK ↔ `text_fr` and family-PK ↔ name cross-checked character-for-character.
  3. *Mirror consistency* — every row's `prevented_family_pk` equals the pilot §3 mirror for `Présentation intègre` (= 887).
- **Result: 21/21 annotated, 0 violations across all three layers.** [PASS]
- **Anti-fab validator #518** (`tools/validate_taxonomy_annotations.py`, kind=virtues): **`✓ CLEAN — 21 rows, 0 HARD, 0 WARN`.**

Generation/verify script: `tmp/499_phase2_gen2.py` (ephemeral, not committed — derived from the Phase-1 `tmp/499_gen.py`). Dataset: `tmp/499_phase2_dataset_2.json` (ephemeral). Raw model output: `tmp/499_phase2_gen_result.json` (ephemeral).

## 5. The 21 rows (full set in the CSV)

| Virtue (pk, depth) | → Fam. (PK) | Opposes (PK = fallacy) | Walton scheme | CQ restored |
|---|---|---|---|---|
| Rhétorique éloquente (36, d3) | Tricherie (887) | 177 Langage persuasif · 889 Mensonge | Verbal Classification | La formulation renforce-t-elle la clarté sans orienter indûment l'adhésion ? |
| Adaptation discursive (37, d4) | Tricherie (887) | 300 Connivence · 953 Attention sélective | Position to Know | L'adaptation à l'auditoire respecte-t-elle l'information pertinente ? |
| Vocabulaire approprié (38, d4) | Tricherie (887) | 855 Équivoque · 800 Acception vague | Verbal Classification | Les termes classent-ils correctement ce dont on parle ? |
| Jargon mesuré (39, d5) | Tricherie (887) | 1345 Complication exagérée · 667 Imprécision | Verbal Classification | Le vocabulaire spécialisé est-il nécessaire et compréhensible ? |
| Termes techniques justifiés (40, d5) | Tricherie (887) | 1345 Complication exagérée · 826 Définition incohérente | Verbal Classification | Les termes techniques sont-ils définis clairement ? |
| Vocabulaire neutre (41, d5) | Tricherie (887) | 177 Langage persuasif · 1352 Empoisonnement du puits | Bias | La formulation est-elle exempte de connotations biaisées ? |
| Langage respectueux (42, d4) | Tricherie (887) | 1398 Attaque personnelle · 1352 Empoisonnement du puits | Commitment | Le langage respecte-t-il les interlocuteurs ? |
| Humour de bon goût (43, d3) | Tricherie (887) | 219 Humour · 1398 Attaque personnelle | Values | L'humour sert-il une valeur de clarté sans offenser ? |
| Humour adapté au contexte (44, d4) | Tricherie (887) | 219 Humour · 1174 Biais culturels | Values | L'humour est-il adapté aux sensibilités de l'auditoire ? |
| Humour respectueux (45, d4) | Tricherie (887) | 219 Humour · 1398 Attaque personnelle | Values | L'humour respecte-t-il les personnes visées ? |
| Autodérision éclairée (46, d5) | Tricherie (887) | 300 Connivence · 420 Jeu de pouvoir | Commitment | L'autodérision favorise-t-elle une discussion ouverte ? |
| Humour inclusif (47, d4) | Tricherie (887) | 219 Humour · 1174 Biais culturels | Values | L'humour inclut-il sans exploiter de stéréotypes ? |
| Style adapté (48, d3) | Tricherie (887) | 247 Poésie · 876 Ambiguïté narrative | Analogy | Le style éclaire-t-il le propos sans le rendre ambigu ? |
| Éloquence véridique (49, d4) | Tricherie (887) | 247 Poésie · 889 Mensonge | Verbal Classification | L'expression éloquente reste-t-elle fidèle au contenu ? |
| Figures soutenant la clarté (50, d5) | Tricherie (887) | 247 Poésie · 847 Amphibologie | Analogy | La figure de style rend-elle l'argument plus intelligible ? |
| Neutralité émotionnelle (52, d3) | Tricherie (887) | 300 Connivence · 177 Langage persuasif | Bias | L'adhésion repose-t-elle sur des raisons, pas une complicité émotionnelle ? |
| Sobriété dramatique (53, d3) | Tricherie (887) | 340 Appel aux conséquences · 357 Conditionnement | Consequences | Les conséquences sont-elles présentées sans dramatisation ? |
| Sans chantage aux conséquences (54, d3) | Tricherie (887) | 340 Appel aux conséquences · 420 Jeu de pouvoir | Consequences | Les conséquences invoquées sont-elles sans menace ni chantage ? |
| Transparence (56, d3) | Tricherie (887) | 889 Mensonge · 953 Attention sélective | Commitment | Les intentions pertinentes sont-elles déclarées franchement ? |
| Neutralité bienveillante (57, d3) | Tricherie (887) | 420 Jeu de pouvoir · 511 Influence non verbale | Bias | La posture est-elle libre de stratégie d'emprise ? |
| Posture appropriée (58, d3) | Tricherie (887) | 511 Influence non verbale · 420 Jeu de pouvoir | Commitment | La posture soutient-elle loyalement la discussion ? |

**Scheme distribution (balanced, 7 distinct schemes):** Verbal Classification ×5, Commitment ×4, Values ×4, Bias ×3, Analogy ×2, Consequences ×2, Position to Know ×1. Présentation intègre spans presentation rhetoric — language choice, engagement/respect, value-aligned humor, analogy/figures, emotional neutrality, and consequence framing — so its scheme spread is broader than the single-family-dominant batches 1–3. The Values cluster (×4, all humor virtues opposing `Humour` 219) is the family's semantic signature: good humor is the correct holding of the `Humour` scheme, opposing its abuse into ridicule/derision. Contrast batches 1–3 (single-scheme dominant) — this broader profile confirms the pipeline tracks each family's genuine topical range rather than defaulting to one scheme.

Machine-readable: [`499-scaleup-phase2-presentation-annotations.csv`](499-scaleup-phase2-presentation-annotations.csv).

## 6. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Virtues - Taxonomy.csv`. Awaiting jsboige approval.
- ❌ Does **not** touch OWL, EPITA consumer, cards, or mindmaps.
- ❌ Does **not** claim the content is final — this is batch 4 (Présentation intègre) of the depth-3…7 layer; 3 families (120 nodes) remain for subsequent batches.

## 7. Proposed next steps (gated on jsboige)

1. **jsboige validates** the 21 Présentation-intègre rows (paradigm + content + depth-leaf granularity).
2. **Phase 2 remaining batches** — the 3 remaining families (120 depth-3…7 nodes), same method + anti-fab, in subsequent cron ticks. Argument pertinent (29) is next; then the 2 large ones — Raisonnement valide (51) and Échange enrichissant (40) — sub-batched per the Phase-1 precedent (batches of ~6).
3. On approval of the full Phase-2 set: the gated prod write (12-col Virtues CSV population) proceeds, same as the Phase-1 plan.

---

*GATED proposal. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
