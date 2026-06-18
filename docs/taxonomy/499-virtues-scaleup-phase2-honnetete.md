# #499 — Virtues Scale-up, Phase 2 (batch 3): Honnêteté intellectuelle depth-3…5 leaves (GATED proposal)

**Issue:** [#499 — Virtues parity with Fallacies relational + AIF layers](https://github.com/ArgumentumGames/Argumentum/issues/499)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-18
**Base:** master `fc2013fc`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Virtues CSV change until jsboige approves.** This document + the annotation CSV are the proposition.

**Calibration:** [`499-virtues-scaleup-phase2.md`](499-virtues-scaleup-phase2.md) — batch 1 (Langage exact, 14 nodes, **merged** `fc2013fc`/PR #530) validated the Phase-2 leaf method. Batch 3 uses the **same method, same schema, same anti-fab guarantee**, extending the annotation to the Honnêteté intellectuelle family.

---

## 1. Scope of batch 3

Phase 2 annotates the **depth-3…7 leaf layer** — the 194 remaining Virtue nodes that carry no annotation yet. This is **batch 3: the `Honnêteté intellectuelle` family** (segment 6, 23 nodes: 9 depth-3 + 12 depth-4 + 2 depth-5). It mirrors `Tricherie` (Fallacy family PK 887).

| Virtue family | depth-3…7 leaves (Phase 2 scope) | status |
|---|---|---|
| Langage exact | 14 | ✅ batch 1 merged (`fc2013fc`) |
| Rigueur mathématique | 16 | ⏳ batch 2 (PR #532, ai-01 review) |
| **Honnêteté intellectuelle** | **23** | ✅ **this batch** |
| Raisonnement valide | 51 | next (largest, sub-batched) |
| Échange enrichissant | 40 | next (sub-batched) |
| Argument pertinent | 29 | next |
| Présentation intègre | 21 | next |
| **Total remaining** | **194** | |

## 2. Schema — identical 10-col presentation schema (Phase 1, unchanged)

```
virtue_pk, virtue_title, prevented_family_pk, prevented_family_name,
crossLink_Opposes, opposed_fallacies_readable,
AIF_skosDirectRef, AIF_skosMappingType, link_type, justification
```

Same naming notes as Phase 1 (`AIF_skosDirectRef` = Walton scheme; `AIF_skosMappingType` = CQ restored; `link_type` = `crossLink_Opposes`). The eventual 12-col prod write is the separate final gated step.

## 3. The inverse paradigm (honored per leaf, same as Phase 1)

A virtue is the **good holding of a scheme** — the **correct answer to the CQ** the corresponding fallacy violates. The leaves inherit their family's mirror (`Honnêteté intellectuelle` → `Tricherie` 887) and each opposes 1–2 specific real depth-3 fallacies. Depth-leaf granularity: a depth-4/5 virtue opposes the *most specific* applicable fallacy under the family (e.g. "Considération équilibrée" → `Attention sélective` 953, not a broad bias).

## 4. Method & anti-fabrication guarantee (identical to Phase 1)

- **Model:** gpt-5.5 (OpenAI direct, key live 2026-06-18). `POST /v1/responses`, `reasoning:{effort:"low"}`, `max_output_tokens:4500`, no `temperature`. Answer read from `output[].content[].text`.
- **Grounding catalog:** the **7 Fallacy family PKs** + **63 named depth-3 fallacies** (9 per family), reused verbatim from the Phase-1 dataset. The prompt forbids referencing any PK outside this catalog.
- **Family mirror as hard constraint:** every `Honnêteté intellectuelle` leaf's `prevented_family_pk` is fixed to **887**.
- **Verification (three independent layers, all re-checked against the REAL corpus, not the prompt catalog):**
  1. *Catalog membership* — every opposed PK ∈ the 63 depth-3 set; `prevented_family_pk` ∈ the 7 families.
  2. *Ground-truth* — every PK re-verified against the real **1408-row** Fallacies CSV; opposed-PK ↔ `text_fr` and family-PK ↔ name cross-checked character-for-character. (The Fallacies corpus uses `text_fr` as its title field.)
  3. *Mirror consistency* — every row's `prevented_family_pk` equals the pilot §3 mirror for `Honnêteté intellectuelle` (= 887).
- **Result: 23/23 annotated, 0 violations across all three layers.** [PASS]
- **Anti-fab validator #518** (`tools/validate_taxonomy_annotations.py`, kind=virtues): **`✓ CLEAN — 23 rows, 0 HARD, 0 WARN`.**

Generation/verify script: `tmp/499_phase2_gen6.py` (ephemeral, not committed — derived from the Phase-1 `tmp/499_gen.py`). Dataset: `tmp/499_phase2_dataset_6.json` (ephemeral). Raw model output: `tmp/499_phase2_gen_result.json` (ephemeral).

## 5. The 23 rows (full set in the CSV)

| Virtue (pk, depth) | → Fam. (PK) | Opposes (PK = fallacy) | Walton scheme | CQ restored |
|---|---|---|---|---|
| Vérité des faits (154, d3) | Tricherie (887) | 889 Mensonge · 1297 Preuve par assertion | Witness Testimony | Les faits rapportés sont-ils exacts, vérifiés et fidèles ? |
| Attribution juste (155, d3) | Tricherie (887) | 942 Fausse attribution · 889 Mensonge | Witness Testimony | La source citée est-elle correctement identifiée ? |
| Considération équilibrée (156, d4) | Tricherie (887) | 953 Attention sélective · 177 Langage persuasif | Bias | La présentation des faits est-elle exempte de sélection partiale ? |
| Considération nuancée (157, d4) | Tricherie (887) | 953 Attention sélective · 1282 Relativisme abusif | Bias | Les points de vue pertinents sont-ils pris en compte ? |
| Objectivité (158, d3) | Tricherie (887) | 953 Attention sélective · 889 Mensonge | Witness Testimony | L'énoncé est-il fidèle à la réalité observée ? |
| Rigueur des critères (160, d4) | Tricherie (887) | 974 Exigence renforcée · 1011 Exigence relâchée | Rule | Les critères sont-ils appliqués uniformément ? |
| Cohérence des positions (161, d4) | Tricherie (887) | 1297 Preuve par assertion · 698 Pétition de principe | Commitment | Les positions sont-elles libres de pétition de principe ? |
| Uniformité d'application (162, d4) | Tricherie (887) | 974 Exigence renforcée · 1011 Exigence relâchée | Rule | La règle est-elle appliquée sans double standard ? |
| Clarté des engagements (163, d3) | Tricherie (887) | 876 Ambiguïté narrative · 855 Équivoque | Commitment | L'engagement est-il exprimé sans ambiguïté ? |
| Stabilité des positions (164, d4) | Tricherie (887) | 992 Vouloir le beurre et l’argent du beurre · 1011 Exigence relâchée | Rule | La position est-elle stable dans le temps ? |
| Prudence face aux conséquences (165, d3) | Tricherie (887) | 340 Appel aux conséquences · 1345 Complication exagérée | Danger | Les conséquences sont-elles évaluées sans alarmisme ? |
| Refus de la manipulation (166, d4) | Tricherie (887) | 420 Jeu de pouvoir · 953 Attention sélective | Bias | L'argument repose-t-il sur la raison, pas sur le pouvoir ? |
| Ouverture d'esprit (168, d5) | Tricherie (887) | 1174 Biais culturels · 1242 Biais théoriques | Values | Les cadres étrangers sont-ils considérés avec fairness ? |
| Reconnaissance des biais (169, d4) | Tricherie (887) | 1024 Biais naturels · 953 Attention sélective | Bias | Les biais cognitifs sont-ils reconnus et corrigés ? |
| Humilité épistémique (170, d4) | Tricherie (887) | 1024 Biais naturels · 1352 Empoisonnement du puits | Bias | Les limites du savoir sont-elles reconnues ? |
| Respect des cultures (171, d4) | Tricherie (887) | 1174 Biais culturels · 1282 Relativisme abusif | Bias | Le jugement évite-t-il l'ethnocentrisme ? |
| Tolérance intellectuelle (172, d4) | Tricherie (887) | 1174 Biais culturels · 953 Attention sélective | Bias | Les vues divergentes sont-elles écoutées ? |
| Indépendance de jugement (173, d4) | Tricherie (887) | 1174 Biais culturels · 1371 Sophisme génétique | Bias | Le jugement est-il libre de l'origine de l'idée ? |
| Remise en question (174, d4) | Tricherie (887) | 1242 Biais théoriques · 953 Attention sélective | Bias | Les présupposés sont-ils périodiquement réexaminés ? |
| Prudence catégorielle (175, d4) | Tricherie (887) | 1242 Biais théoriques · 844 Sophisme d'association | Bias | La généralisation évite-t-elle l'association abusive ? |
| Vigilance argumentative (176, d4) | Tricherie (887) | 1242 Biais théoriques · 1352 Empoisonnement du puits | Bias | L'argument est-il évalué sans préjugé hostile ? |
| Probité statistique (177, d5) | Tricherie (887) | 644 Probabilités faussées · 658 Infini fallacieux | Sign | Les chiffres sont-ils utilisés sans tromperie ? |
| Honnêteté causale (178, d3) | Tricherie (887) | 596 Échantillon biaisé · 633 Relation infondée | Cause to Effect | La relation causale est-elle établie sans foriture ? |

**Scheme distribution (11 of 23 = Bias-dominant):** Argument from Bias ×11, Witness Testimony ×3, Rule ×3, Commitment ×2, Danger / Values / Sign / Cause to Effect ×1 each. This is the expected profile for Honnêteté intellectuelle — the family of intellectual honesty virtues whose opposite is cognitive bias and selective attention, not a single logical fallacy. Contrast batch 1 (Langage exact, Verbal Classification dominant) and batch 2 (Rigueur math, Sign dominant): the scheme distribution shifts per family, confirming semantic fidelity rather than generic pattern-matching.

Machine-readable: [`499-scaleup-phase2-honnetete-annotations.csv`](499-scaleup-phase2-honnetete-annotations.csv).

## 6. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Virtues - Taxonomy.csv`. Awaiting jsboige approval.
- ❌ Does **not** touch OWL, EPITA consumer, cards, or mindmaps.
- ❌ Does **not** claim the content is final — this is batch 3 (Honnêteté intellectuelle) of the depth-3…7 layer; 4 families (142 nodes) remain for subsequent batches.

## 7. Proposed next steps (gated on jsboige)

1. **jsboige validates** the 23 Honnêteté-intellectuelle rows (paradigm + content + depth-leaf granularity).
2. **Phase 2 remaining batches** — the 4 remaining families (142 depth-3…7 nodes), same method + anti-fab, in subsequent cron ticks. Raisonnement valide (51) and Échange enrichissant (40) are the largest and will be sub-batched.
3. On approval of the full Phase-2 set: the gated prod write (12-col Virtues CSV population) proceeds, same as the Phase-1 plan.

---

*GATED proposal. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
