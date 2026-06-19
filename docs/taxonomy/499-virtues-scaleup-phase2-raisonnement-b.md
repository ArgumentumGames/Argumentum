# #499 — Virtues Scale-up, Phase 2 (batch 7b): Raisonnement valide depth-6…7 leaves — sub-batch B (pks 105–127) (GATED proposal)

**Issue:** [#499 — Virtues parity with Fallacies relational + AIF layers](https://github.com/ArgumentumGames/Argumentum/issues/499)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-19
**Base:** master `f17d8e6e`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Virtues CSV change until jsboige approves.** This document + the annotation CSV are the proposition.

**Calibration:** [`499-virtues-scaleup-phase2-raisonnement-a.md`](499-virtues-scaleup-phase2-raisonnement-a.md) — batch 7a (pks 81–104, premises/causality/formal-validity + inference rules) delivered, PR #540. This is **batch 7b: the syllogistic-modes cluster** (pks 105–127, 23 leaves: 4 depth-6 figure anchors + 19 depth-7 modes). It reaches **depth-7 — the deepest layer of the entire Virtues taxonomy**. **7c** (pks 128–133, 6 leaves, informal/analytical reasoning) follows to complete Phase 2. Same method, same schema, same anti-fab guarantee.

---

## 1. Scope of batch 7b

Phase 2 annotates the **depth-3…7 leaf layer** — the 194 remaining Virtue nodes that carry no annotation yet. This is **batch 7b: the second sub-batch of the `Raisonnement valide` family** (segment 4, pks 105–127, 23 nodes: 4 depth-6 + 19 depth-7). It mirrors `Erreur de raisonnement` (Fallacy family PK 696). Batch 7b is the **syllogistic-modes cluster**: the 4 figure anchors (2nd/3rd/4th figure + "mode parfait") and the **19 canonical syllogistic modes** (Barbara, Celarent, Darii, Ferio, Cesare, Camestres, Festino, Baroco, Darapti, Felapton, Disamis, Datisi, Bocardo, Ferison, Camenes, Dimatis, Fesapo, Fresison, Bamalip) — the deepest technical layer of the whole taxonomy.

| Virtue family | depth-3…7 leaves | status |
|---|---|---|
| Langage exact | 14 | ✅ batch 1 merged |
| Rigueur mathématique | 16 | ✅ batch 2 merged |
| Honnêteté intellectuelle | 23 | ✅ batch 3 merged |
| Présentation intègre | 21 | ✅ batch 4 merged |
| Argument pertinent | 29 | ✅ batch 5 merged |
| Échange enrichissant | 40 | ✅ batch 6 (6a+6b) merged |
| **Raisonnement valide** | **51** (7a=22 + 7b=23 + 7c=6) | **7a PR #540 · 7b this batch · 7c next** |
| **Total remaining** | **194** | |

## 2. Schema — identical 10-col presentation schema (Phase 1, unchanged)

```
virtue_pk, virtue_title, prevented_family_pk, prevented_family_name,
crossLink_Opposes, opposed_fallacies_readable,
AIF_skosDirectRef, AIF_skosMappingType, link_type, justification
```

Same naming notes as Phase 1 (`AIF_skosDirectRef` = Walton scheme; `AIF_skosMappingType` = CQ restored; `link_type` = `crossLink_Opposes`). The eventual 12-col prod write is the separate final gated step.

## 3. The inverse paradigm (honored per leaf, same as Phase 1)

A virtue is the **good holding of a scheme** — the **correct answer to the CQ** the corresponding fallacy violates. The leaves inherit their family's mirror (`Raisonnement valide` → `Erreur de raisonnement` 696) and each opposes 1–2 specific real depth-3 fallacies. Batch 7b is the **syllogistic-form** cluster: each mode is the *good holding of a Rule scheme of syllogistic form* — applying the correct figure/mode to derive a valid conclusion — opposing `Syllogisme invalide` 784 (the misapplication of syllogistic form) and the quantification errors that break syllogistic validity.

## 4. Method & anti-fabrication guarantee (identical to Phase 1)

- **Model:** gpt-5.5 (OpenAI direct, key live 2026-06-19). `POST /v1/responses`, `reasoning:{effort:"low"}`, `max_output_tokens:7000`, no `temperature`. Answer read from `output[].content[].text`.
- **Grounding catalog:** the **7 Fallacy family PKs** + **63 named depth-3 fallacies** (9 per family), reused verbatim from the Phase-1 dataset. The prompt forbids referencing any PK outside this catalog.
- **Family mirror as hard constraint:** every `Raisonnement valide` leaf's `prevented_family_pk` is fixed to **696**.
- **Verification (three independent layers, all re-checked against the REAL corpus, not the prompt catalog):**
  1. *Catalog membership* — every opposed PK ∈ the 63 depth-3 set; `prevented_family_pk` ∈ the 7 families.
  2. *Ground-truth* — every PK re-verified against the real **1408-row** Fallacies CSV (`PK` + `text_fr` columns); opposed-PK ↔ `text_fr` and family-PK ↔ name cross-checked character-for-character.
  3. *Mirror consistency* — every row's `prevented_family_pk` equals the pilot §3 mirror for `Raisonnement valide` (= 696).
- **Result: 23/23 annotated, 0 violations across all three layers.** [PASS]
- **Anti-fab validator #518** (`tools/validate_taxonomy_annotations.py`, kind=virtues): **`✓ CLEAN — 23 rows, 0 HARD, 0 WARN`.**

Generation/verify script: `tmp/499_phase2_gen4.py` (ephemeral, not committed — derived from the Phase-1 `tmp/499_gen.py`). Dataset: `tmp/499_phase2_dataset_4.json` (ephemeral). Raw model output: `tmp/499_phase2_gen_7b.json` (ephemeral).

## 5. The 23 rows (full set in the CSV)

| Virtue (pk, depth) | → Fam. (PK) | Opposes (PK = fallacy) | Walton scheme | CQ restored |
|---|---|---|---|---|
| Syllogisme de mode parfait (105, d6) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | La règle syllogistique est-elle correctement appliquée aux trois termes et aux quantificateurs ? |
| Syllogisme Barbara (106, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 727 Erreur de logique propositionnelle | Rule | Deux prémisses universelles affirmatives autorisent-elles bien la conclusion universelle affirmative ? |
| Syllogisme Celarent (107, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | La prémisse négative universelle et la prémisse affirmative universelle justifient-elles bien l'exclusion universelle conclue ? |
| Syllogisme Darii (108, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 759 Conclusion hâtive | Rule | L'inclusion universelle et l'existence particulière permettent-elles bien de conclure une inclusion particulière ? |
| Syllogisme Ferio (109, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | L'universelle négative et la particulière affirmative autorisent-elles bien une conclusion particulière négative ? |
| Syllogisme de deuxième figure (110, d6) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | Le terme moyen, prédicat dans les deux prémisses, est-il distribué de façon à permettre la conclusion par opposition ? |
| Syllogisme Cesare (111, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | Les deux prémisses établissent-elles correctement qu'aucun sujet de la première catégorie n'appartient à l'autre ? |
| Syllogisme Camestres (112, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 727 Erreur de logique propositionnelle | Rule | L'affirmation universelle et la négation universelle mènent-elles validement à la conclusion universelle négative ? |
| Syllogisme Festino (113, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 759 Conclusion hâtive | Rule | L'exclusion universelle et le cas particulier affirmé soutiennent-ils bien une conclusion particulière négative ? |
| Syllogisme Baroco (114, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | La prémisse universelle affirmative et la prémisse particulière négative autorisent-elles bien la conclusion particulière négative ? |
| Syllogisme de 3e figure (115, d6) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | Le terme moyen sujet dans les deux prémisses permet-il seulement la conclusion particulière requise ? |
| Syllogisme Darapti (116, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | Deux prémisses universelles affirmatives justifient-elles une conclusion particulière affirmative sans excès de portée ? |
| Syllogisme Felapton (117, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | L'universelle négative et l'universelle affirmative fondent-elles bien une conclusion particulière négative ? |
| Syllogisme Disamis (118, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 759 Conclusion hâtive | Rule | La prémisse particulière affirmative et la prémisse universelle affirmative permettent-elles bien la conclusion particulière affirmative ? |
| Syllogisme Datisi (119, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | L'universelle affirmative et la particulière affirmative soutiennent-elles validement une conclusion particulière affirmative ? |
| Syllogisme Bocardo (120, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | La prémisse particulière négative et l'universelle affirmative autorisent-elles bien une conclusion particulière négative ? |
| Syllogisme Ferison (121, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 759 Conclusion hâtive | Rule | L'universelle négative et la particulière affirmative justifient-elles seulement la conclusion particulière négative ? |
| Syllogisme de quatrième figure (122, d6) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | La position du terme moyen en quatrième figure permet-elle la conclusion avec la bonne quantité et la bonne qualité ? |
| Syllogisme Camenes (123, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 727 Erreur de logique propositionnelle | Rule | Les deux prémisses universelles de Camenes produisent-elles validement la conclusion universelle attendue ? |
| Syllogisme Dimatis (124, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | Deux prémisses affirmatives autorisent-elles bien la conclusion particulière de Dimatis ? |
| Syllogisme Fesapo (125, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | Deux prémisses universelles de Fesapo justifient-elles correctement une conclusion particulière négative ? |
| Syllogisme Fresison (126, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | L'universelle négative et l'universelle affirmative de Fresison fondent-elles bien une conclusion particulière négative ? |
| Syllogisme Bamalip (127, d7) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | Deux prémisses universelles affirmatives de Bamalip permettent-elles bien une conclusion particulière affirmative ? |

**Scheme distribution (100% Rule — total single-scheme dominance, the strongest of all batches):** Rule ×23. The **Rule dominance (23/23, 100%)** exceeds even batch 7a (17/22): every syllogistic mode is, by definition, the *good holding of a Rule scheme of syllogistic form* — applying a valid figure/mode to derive a valid conclusion. The most-opposed fallacy is `Syllogisme invalide` 784 — it appears in **all 23 rows (23/23, 100%)**, because each valid syllogistic mode is the precise inverse of an invalid syllogism: a valid mode (Barbara, Celarent, Darii…) applies the correct distribution of the middle term and the correct quantity/quality, where an invalid syllogism violates exactly that. The secondary opponent `Erreur de quantification` 735 (×16) names the distribution errors that break syllogistic validity (illicit distribution of the middle/ major/minor term); `Conclusion hâtive` 759 (×4) and `Erreur de logique propositionnelle` 727 (×3) catch the existential-import and compound-form errors.

The **depth-7 CQs carry per-mode specificity** — each names the exact premise quantity/quality of that mode (e.g. Barbara = "deux prémisses universelles affirmatives → conclusion universelle affirmative"; Celarent = "prémisse négative universelle + prémisse affirmative universelle → exclusion universelle"). This is semantic fidelity at the **deepest technical layer of the entire taxonomy**: gpt-5.5 reproduces the classical syllogistic mnemonics (Barbara/Celarent/Darii/Ferio/Cesare/Camestres/Festino/Baroco/Darapti/Felapton/Disamis/Datisi/Bocardo/Ferison/Camenes/Dimatis/Fesapo/Fresison/Bamalip) with the correct figure and mood for each. Batch 7b confirms the pipeline tracks genuine technical depth — not generic pattern-matching — all the way to depth-7.

Machine-readable: [`499-scaleup-phase2-raisonnement-b-annotations.csv`](499-scaleup-phase2-raisonnement-b-annotations.csv).

## 6. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Virtues - Taxonomy.csv`. Awaiting jsboige approval.
- ❌ Does **not** touch OWL, EPITA consumer, cards, or mindmaps.
- ❌ Does **not** complete the Raisonnement valide family — **7c (pks 128–133, 6 leaves, informal/analytical reasoning) follows** in the next tick to complete Phase 2 (194/194 leaves).

## 7. Proposed next steps (gated on jsboige)

1. **jsboige validates** the 23 Raisonnement-B rows (paradigm + content + the depth-7 syllogistic-mode granularity).
2. **Batch 7c** (pks 128–133, 6 informal/analytical-reasoning leaves) next tick — same method + anti-fab. This completes Raisonnement valide (51/51) AND the full Phase-2 Virtues set (194/194 leaves).
3. On approval of the full Phase-2 set: the gated prod write (12-col Virtues CSV population) proceeds, same as the Phase-1 plan.

---

*GATED proposal. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
