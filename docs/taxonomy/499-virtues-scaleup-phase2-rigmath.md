# #499 — Virtues Scale-up, Phase 2 (batch 2): Rigueur mathématique depth-3…4 leaves (GATED proposal)

**Issue:** [#499 — Virtues parity with Fallacies relational + AIF layers](https://github.com/ArgumentumGames/Argumentum/issues/499)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-18
**Base:** master `fc2013fc`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Virtues CSV change until jsboige approves.**

**Calibration:** [Phase 2 batch 1 — `Langage exact`](499-virtues-scaleup-phase2.md) (`499-scaleup-phase2-annotations.csv`, 14 leaves, **merged** `fc2013fc` via #530) + [Phase 1](499-virtues-scaleup-phase1.md) (`0ad40259`). This batch 2 uses the **same method, same schema, same anti-fab guarantee**, on the second family.

---

## 1. Scope

Phase 2 batch 2: the **`Rigueur mathématique` family** (16 nodes: 9 depth-3 + 7 depth-4). Same leaf-layer annotation as batch 1; confirms the method reproduces across families.

| Virtue family | leaves | batch | prevented Fallacy family |
|---|---|---|---|
| ~~Langage exact~~ | ~~14~~ | ✅ batch 1 (#530 merged) | Abus de langage (798) |
| **Rigueur mathématique** | **16** | ✅ **this batch** | Erreur mathématique (594) |
| Raisonnement valide | 51 | next | Erreur de raisonnement (696) |
| Échange enrichissant | 40 | next | Obstruction (1280) |
| Argument pertinent | 29 | next | Insuffisance (1) |
| Honnêteté intellectuelle | 23 | next | Tricherie (887) |
| Présentation intègre | 21 | next | Tricherie (887) |
| **Remaining after this batch** | **164** | | |

## 2. Schema — identical 10-col presentation schema (Phase 1, unchanged)

```
virtue_pk, virtue_title, prevented_family_pk, prevented_family_name,
crossLink_Opposes, opposed_fallacies_readable,
AIF_skosDirectRef, AIF_skosMappingType, link_type, justification
```

## 3. Method & anti-fabrication guarantee (identical to Phase 1 / batch 1)

- **gpt-5.5** `/v1/responses` `reasoning:low`, grounding catalog of **7 family PKs + 63 real depth-3 fallacies** (forbidden to use any PK outside it).
- **Family mirror hard constraint**: every `Rigueur mathématique` leaf → `Erreur mathématique` (PK 594).
- **Three independent verification layers**, all re-checked against the REAL corpus:
  1. *Catalog membership* — every opposed PK ∈ the 63 depth-3 set.
  2. *Ground-truth* — every PK re-verified against the real **1408-row** Fallacies CSV, opposed-PK ↔ `text_fr` character-for-character.
  3. *Mirror consistency* — every row's `prevented_family_pk` = 594.
- **Result: 16/16 annotated, 0 violations across all 3 layers.** [PASS]
- **Anti-fab validator #518** (kind=virtues): **`✓ CLEAN — 16 rows, 0 HARD, 0 WARN`**.

Generation/verify script: `tmp/499_phase2_gen3.py` (ephemeral, not committed). Dataset: `tmp/499_phase2_dataset_3.json` (ephemeral). Raw model output: `tmp/499_phase2_gen_result.json` (ephemeral).

## 4. The 16 rows (full set in the CSV)

| Virtue (pk, depth) | → Fam. (PK) | Opposes (PK = fallacy) | Walton scheme |
|---|---|---|---|
| Échantillonnage représentatif (61, d3) | Erreur math (594) | 596 Échantillon biaisé · 759 Conclusion hâtive | Example |
| Prise en compte des exceptions (62, d3) | Erreur math (594) | 614 Sophisme de l'accident · 759 Conclusion hâtive | Rule |
| Transfert licite (63, d3) | Erreur math (594) | 621 Transfert illicite · 839 Fausse analogie | Analogy |
| Corrélations avérées (65, d3) | Erreur math (594) | 633 Relation infondée · 719 Effet cigogne | Sign |
| Estimation méthodique (66, d4) | Erreur math (594) | 633 Relation infondée · 644 Probabilités faussées | Sign |
| Probabilités maîtrisées (67, d3) | Erreur math (594) | 644 Probabilités faussées · 681 Erreur de calcul | Sign |
| Indépendance des événements (68, d4) | Erreur math (594) | 644 Probabilités faussées · 633 Relation infondée | Cause to Effect |
| Raisonnement bayésien (69, d4) | Erreur math (594) | 644 Probabilités faussées · 707 Inversion de causalité | Sign |
| Support fini (70, d3) | Erreur math (594) | 658 Infini fallacieux · 644 Probabilités faussées | Rule |
| Mesures précises (72, d3) | Erreur math (594) | 667 Imprécision · 681 Erreur de calcul | Sign |
| Données claires (73, d4) | Erreur math (594) | 667 Imprécision · 804 Acception arbitraire | Sign |
| Calculs exacts (74, d3) | Erreur math (594) | 681 Erreur de calcul · 690 Opération inappropriée | Sign |
| Exactitude numérique (75, d4) | Erreur math (594) | 681 Erreur de calcul · 667 Imprécision | Sign |
| Justesse géométrique (76, d4) | Erreur math (594) | 681 Erreur de calcul · 690 Opération inappropriée | Sign |
| Exactitude topologique (77, d4) | Erreur math (594) | 690 Opération inappropriée · 667 Imprécision | Verbal Classification |
| Opérations valides (78, d3) | Erreur math (594) | 690 Opération inappropriée · 681 Erreur de calcul | Rule |

Machine-readable: [`499-scaleup-phase2-rigmath-annotations.csv`](499-scaleup-phase2-rigmath-annotations.csv).

**Granularity note** (leaf-level, consistent with batch 1): depth-4 leaves oppose the *most specific* applicable math fallacy — e.g. "Exactitude numérique" (pk 75) → `Erreur de calcul` (681) + `Imprécision` (667), not a broad sampling fallacy. The "Sign" scheme dominance reflects the probabilistic/correlational nature of most math-rigor virtues.

## 5. What this does NOT do (gate boundaries)

- ❌ No production `Argumentum Virtues - Taxonomy.csv` change — `docs/taxonomy/` only.
- ❌ No OWL / EPITA consumer / cards / mindmaps touched.
- ❌ Generation script + raw model output kept **ephemeral** (`tmp/`, not committed).
- ❌ Batch 2 of 7; 5 families (164 nodes) remain for subsequent batches.

## 6. Proposed next steps (gated on jsboige)

1. **jsboige validates** the 16 Rigueur-mathématique rows (and the batch-1 Langage-exact rows if not yet reviewed).
2. **Phase 2 batches 3+** — the 5 remaining families (164 nodes). Raisonnement valide (51) and Échange enrichissant (40) are the largest and will be sub-batched.
3. On approval of the full Phase-2 set: the gated prod write (12-col Virtues CSV population).

---

*GATED proposal. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
