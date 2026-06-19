# #499 — Virtues Scale-up, Phase 2 (batch 7a): Raisonnement valide depth-3…6 leaves — sub-batch A (pks 81–104) (GATED proposal)

**Issue:** [#499 — Virtues parity with Fallacies relational + AIF layers](https://github.com/ArgumentumGames/Argumentum/issues/499)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-19
**Base:** master `f17d8e6e`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Virtues CSV change until jsboige approves.** This document + the annotation CSV are the proposition.

**Calibration:** [`499-virtues-scaleup-phase2-echange-a.md`](499-virtues-scaleup-phase2-echange-a.md) — Échange enrichissant (40 leaves) was completed via sub-batching (6a + 6b). Batch 7 is the **last and largest family**: Raisonnement valide (51 leaves, the deepest — 19 nodes at depth-7). Split into **7a (pks 81–104, 22 leaves, premises/causality/formal-validity + inference rules)**, **7b (pks 105–127, 23 leaves, syllogistic modes)**, **7c (pks 128–133, 6 leaves, informal/analytical reasoning)**. Same method, same schema, same anti-fab guarantee.

---

## 1. Scope of batch 7a

Phase 2 annotates the **depth-3…7 leaf layer** — the 194 remaining Virtue nodes that carry no annotation yet. This is **batch 7a: the first sub-batch of the `Raisonnement valide` family** (segment 4, pks 81–104, 22 nodes: 9 depth-3 + 4 depth-4 + 2 depth-5 + 7 depth-6). It mirrors `Erreur de raisonnement` (Fallacy family PK 696) — the family of reasoning-error fallacies (petitio principii, causal inversion, propositional-logic error, invalid syllogism, inconsistency).

| Virtue family | depth-3…7 leaves | status |
|---|---|---|
| Langage exact | 14 | ✅ batch 1 merged |
| Rigueur mathématique | 16 | ✅ batch 2 merged |
| Honnêteté intellectuelle | 23 | ✅ batch 3 merged |
| Présentation intègre | 21 | ✅ batch 4 merged |
| Argument pertinent | 29 | ✅ batch 5 merged |
| Échange enrichissant | 40 | ✅ batch 6 (6a+6b) merged |
| **Raisonnement valide** | **51** (7a=22 + 7b=23 + 7c=6) | **7a this batch · 7b + 7c next** |
| **Total remaining** | **194** | |

## 2. Schema — identical 10-col presentation schema (Phase 1, unchanged)

```
virtue_pk, virtue_title, prevented_family_pk, prevented_family_name,
crossLink_Opposes, opposed_fallacies_readable,
AIF_skosDirectRef, AIF_skosMappingType, link_type, justification
```

Same naming notes as Phase 1 (`AIF_skosDirectRef` = Walton scheme; `AIF_skosMappingType` = CQ restored; `link_type` = `crossLink_Opposes`). The eventual 12-col prod write is the separate final gated step.

## 3. The inverse paradigm (honored per leaf, same as Phase 1)

A virtue is the **good holding of a scheme** — the **correct answer to the CQ** the corresponding fallacy violates. The leaves inherit their family's mirror (`Raisonnement valide` → `Erreur de raisonnement` 696) and each opposes 1–2 specific real depth-3 fallacies. Batch 7a is the **formal-validity** cluster (pks 81–104: independence of premises, causal orientation, formal validity, and the inference rules — modus ponens, modus tollens, conjunction, reductio, resolution). These virtues are the *good holding of the Rule scheme* — applying valid inferential rules — opposing the formal-logic errors (`Erreur de logique propositionnelle`, `Syllogisme invalide`, `Inconsistance`).

## 4. Method & anti-fabrication guarantee (identical to Phase 1)

- **Model:** gpt-5.5 (OpenAI direct, key live 2026-06-19). `POST /v1/responses`, `reasoning:{effort:"low"}`, `max_output_tokens:7000`, no `temperature`. Answer read from `output[].content[].text`.
- **Grounding catalog:** the **7 Fallacy family PKs** + **63 named depth-3 fallacies** (9 per family), reused verbatim from the Phase-1 dataset. The prompt forbids referencing any PK outside this catalog.
- **Family mirror as hard constraint:** every `Raisonnement valide` leaf's `prevented_family_pk` is fixed to **696**.
- **Verification (three independent layers, all re-checked against the REAL corpus, not the prompt catalog):**
  1. *Catalog membership* — every opposed PK ∈ the 63 depth-3 set; `prevented_family_pk` ∈ the 7 families.
  2. *Ground-truth* — every PK re-verified against the real **1408-row** Fallacies CSV; opposed-PK ↔ `text_fr` and family-PK ↔ name cross-checked character-for-character.
  3. *Mirror consistency* — every row's `prevented_family_pk` equals the pilot §3 mirror for `Raisonnement valide` (= 696).
- **Result: 22/22 annotated, 0 violations across all three layers.** [PASS]
- **Anti-fab validator #518** (`tools/validate_taxonomy_annotations.py`, kind=virtues): **`✓ CLEAN — 22 rows, 0 HARD, 0 WARN`.**

Generation/verify script: `tmp/499_phase2_gen4.py` (ephemeral, not committed — derived from the Phase-1 `tmp/499_gen.py`). Dataset: `tmp/499_phase2_dataset_4.json` (ephemeral). Raw model output: `tmp/499_phase2_gen_7a.json` (ephemeral).

## 5. The 22 rows (full set in the CSV)

| Virtue (pk, depth) | → Fam. (PK) | Opposes (PK = fallacy) | Walton scheme | CQ restored |
|---|---|---|---|---|
| Indépendance des prémisses (81, d3) | Erreur raisonn. (696) | 698 Pétition de principe · 1297 Preuve par assertion | Rule | Les prémisses soutiennent-elles la conclusion sans la présupposer ? |
| Causalité bien orientée (82, d3) | Erreur raisonn. (696) | 707 Inversion de causalité · 719 Effet cigogne | Cause to Effect | La direction causale invoquée est-elle correctement établie ? |
| Exclusion des causes alternatives (83, d3) | Erreur raisonn. (696) | 633 Relation infondée · 719 Effet cigogne | Cause to Effect | Les causes alternatives plausibles ont-elles été examinées et écartées ? |
| Énoncés propositionnels (85, d3) | Erreur raisonn. (696) | 727 Erreur de logique propositionnelle · 847 Amphibologie | Rule | Les énoncés propositionnels sont-ils formulés de façon claire et logique ? |
| Énoncés quantifiés (86, d3) | Erreur raisonn. (696) | 735 Erreur de quantification · 759 Conclusion hâtive | Verbal Classification | La portée des quantificateurs est-elle correctement déterminée ? |
| Modalités adéquates (87, d3) | Erreur raisonn. (696) | 750 Erreur de modalité · 727 Erreur de logique propositionnelle | Rule | La modalité invoquée est-elle logiquement adéquate à l'énoncé ? |
| Raisonnement jalonné (89, d3) | Erreur raisonn. (696) | 658 Infini fallacieux · 727 Erreur de logique propositionnelle | Rule | Chaque étape intermédiaire suit-elle validement de l'étape précédente ? |
| Démonstration cohérente (90, d3) | Erreur raisonn. (696) | 777 Inconsistance · 727 Erreur de logique propositionnelle | Rule | L'enchaînement démonstratif reste-t-il cohérent à toutes ses étapes ? |
| Cohérence des prémisses (91, d4) | Erreur raisonn. (696) | 777 Inconsistance · 698 Pétition de principe | Commitment | Les prémisses sont-elles compatibles entre elles et vérifiables ? |
| Cohérence interne (92, d4) | Erreur raisonn. (696) | 777 Inconsistance · 992 Vouloir le beurre et l’argent du beurre | Commitment | Les prémisses et la conclusion sont-elles mutuellement compatibles ? |
| Raisonnement concluant (93, d3) | Erreur raisonn. (696) | 784 Syllogisme invalide · 759 Conclusion hâtive | Rule | La conclusion découle-t-elle nécessairement des prémisses ? |
| Validité formelle (94, d4) | Erreur raisonn. (696) | 784 Syllogisme invalide · 727 Erreur de logique propositionnelle | Rule | La forme logique de l'argument est-elle valide indépendamment de son contenu ? |
| Inférence simple (95, d5) | Erreur raisonn. (696) | 727 Erreur de logique propositionnelle · 759 Conclusion hâtive | Rule | La règle permet-elle bien de tirer cette conclusion à partir de cette seule prémisse ? |
| Inférence : modus ponens (96, d6) | Erreur raisonn. (696) | 727 Erreur de logique propositionnelle · 784 Syllogisme invalide | Rule | L'antécédent de l'implication est-il établi de sorte que le conséquent suive ? |
| Inférence : modus tollens (97, d6) | Erreur raisonn. (696) | 727 Erreur de logique propositionnelle · 784 Syllogisme invalide | Rule | La négation du conséquent autorise-t-elle bien la négation de l'antécédent ? |
| Inférence : conjonction (98, d6) | Erreur raisonn. (696) | 727 Erreur de logique propositionnelle · 777 Inconsistance | Rule | Les deux propositions conjointes sont-elles chacune établies comme vraies ? |
| Inférence : élimination de la conjonction (99, d6) | Erreur raisonn. (696) | 727 Erreur de logique propositionnelle · 784 Syllogisme invalide | Rule | La proposition conjointe autorise-t-elle bien l'affirmation de chaque composante ? |
| Inférence : double négation (100, d6) | Erreur raisonn. (696) | 727 Erreur de logique propositionnelle · 750 Erreur de modalité | Rule | La double négation est-elle logiquement équivalente à l'affirmation correspondante ? |
| Inférence : reductio ad absurdum (101, d6) | Erreur raisonn. (696) | 777 Inconsistance · 727 Erreur de logique propositionnelle | Rule | La supposition contraire mène-t-elle réellement à une contradiction ? |
| Inférence : résolution unitaire (102, d6) | Erreur raisonn. (696) | 727 Erreur de logique propositionnelle · 784 Syllogisme invalide | Rule | Le littéral unitaire impose-t-il correctement la simplification de la clause ? |
| Inférence : résolution (103, d6) | Erreur raisonn. (696) | 727 Erreur de logique propositionnelle · 784 Syllogisme invalide | Rule | Les deux clauses contiennent-elles bien des littéraux opposés permettant la résolution ? |
| Syllogisme valide (104, d5) | Erreur raisonn. (696) | 784 Syllogisme invalide · 735 Erreur de quantification | Rule | La conclusion syllogistique découle-t-elle validement des deux prémisses quantifiées ? |

**Scheme distribution (Rule-dominant — the strongest single-scheme dominance of all batches):** Rule ×17, Cause to Effect ×2, Commitment ×2, Verbal Classification ×1. The **Rule dominance (17/22)** is the formal-logic signature: these virtues are the *good holding of inferential Rule schemes* — modus ponens, modus tollens, conjunction, reductio, resolution — applying valid rules of inference. The most-opposed fallacies form the **formal-validity cluster**: `Erreur de logique propositionnelle` 727 (×14), `Syllogisme invalide` 784 (×8), `Inconsistance` 777 (×5) — together the exact mirror of `Erreur de raisonnement` 696: a valid reasoning applies correct rules (not `Erreur de logique propositionnelle`), in valid syllogistic form (not `Syllogisme invalide`), consistently (not `Inconsistance`). The d6 inference-rule nodes (pk 96–103: modus ponens/tollens, conjunction, reductio ad absurdum, resolution) carry highly specific CQs naming the exact inferential step — semantic fidelity at the deepest technical layer. Depth reaches d6; 7b will reach d7 (the syllogistic modes).

Machine-readable: [`499-scaleup-phase2-raisonnement-a-annotations.csv`](499-scaleup-phase2-raisonnement-a-annotations.csv).

## 6. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Virtues - Taxonomy.csv`. Awaiting jsboige approval.
- ❌ Does **not** touch OWL, EPITA consumer, cards, or mindmaps.
- ❌ Does **not** complete the Raisonnement valide family — **7b (pks 105–127, 23 leaves, syllogistic modes, depth-6/7) and 7c (pks 128–133, 6 leaves) follow** in subsequent ticks.

## 7. Proposed next steps (gated on jsboige)

1. **jsboige validates** the 22 Raisonnement-A rows (paradigm + content + depth-leaf granularity).
2. **Batch 7b** (pks 105–127, 23 syllogistic-mode leaves incl. all the d7 Barbara/Celarent/etc.) next tick — same method + anti-fab. Then **7c** (pks 128–133, 6 leaves) completes Phase 2.
3. On approval of the full Phase-2 set: the gated prod write (12-col Virtues CSV population) proceeds, same as the Phase-1 plan.

---

*GATED proposal. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
