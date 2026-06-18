# #499 — Virtues Scale-up, Phase 1: the 18 depth-2 sub-families (GATED proposal)

**Issue:** [#499 — Virtues parity with Fallacies relational + AIF layers](https://github.com/ArgumentumGames/Argumentum/issues/499)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-17
**Base:** master `fc8313b3`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Virtues CSV change until jsboige approves.** This document + the annotation CSV are the proposition.

**Calibration:** [`499-virtues-parity-pilot.md`](499-virtues-parity-pilot.md) + [`499-pilot-annotations.csv`](499-pilot-annotations.csv) — jsboige **validated interactively** ("nickel pour la parité", 2026-06-17) and merged as the few-shot exemplar.

---

## 1. Scope of Phase 1

The pilot annotated **10 structuring virtues** (7 family roots at depth 1 + 3 central depth-2 sub-virtues: pk 12, 88, 153). Phase 1 completes the **depth-2 structural backbone**: the **18 remaining depth-2 sub-families** (3 per Virtue family, minus the 3 already done). These are the nodes that anchor each family's three sub-branches before the depth-3+ leaves are refined.

| Virtue family | depth-2 pks (Phase 1) | prevented Fallacy family |
|---|---|---|
| Argument pertinent | 2, 28 | Insuffisance (1) |
| Présentation intègre | 35, 51, 55 | Tricherie (887) |
| Rigueur mathématique | 60, 64, 71 | Erreur mathématique (594) |
| Raisonnement valide | 80, 84 | Erreur de raisonnement (696) |
| Langage exact | 135, 142, 146 | Abus de langage (798) |
| Honnêteté intellectuelle | 159, 167 | Tricherie (887) |
| Échange enrichissant | 180, 199, 207 | Obstruction (1280) |

Remaining after Phase 1: **194 depth-3…7 leaf nodes** (Phase 2+, per family).

## 2. Schema — mirrors the validated pilot (10-col presentation), not the 12-col prod write

The pilot CSV uses a **10-column presentation schema** (what jsboige validated). Phase 1 reuses it verbatim:

```
virtue_pk, virtue_title, prevented_family_pk, prevented_family_name,
crossLink_Opposes, opposed_fallacies_readable,
AIF_skosDirectRef, AIF_skosMappingType, link_type, justification
```

Two naming notes carried over from the pilot (intentional, not errors):
- `AIF_skosDirectRef` holds the **Walton scheme name** the virtue honors.
- `AIF_skosMappingType` holds the **critical question (CQ) the virtue restores** (FR), not a mapping-type enum.

When jsboige approves and we write to the production Virtues CSV, these map into the **12-col Fallacies-mirror schema** (8 `crossLink_*` + 4 `AIF_skos*`, §2 of the pilot doc). Phase 1's `crossLink_Opposes` cell → the `crossLink_Opposes` column; `AIF_skosDirectRef`/`AIF_skosMappingType` carry straight over. That prod write is the **final gated step** (spot-check ai-01 + nod jsboige).

## 3. The inverse paradigm (honored per virtue)

A virtue is the **good holding of a scheme** — the **correct answer to the CQ** that the corresponding fallacy **violates**. So each row gives: the Fallacy family prevented, 1–2 specific named fallacies opposed (`crossLink_Opposes`, real depth-3 PKs), the Walton scheme, and the CQ the virtue restores. The same family mirror the pilot recovered independently is used here as a **hard constraint**.

## 4. Method & anti-fabrication guarantee

- **Model:** gpt-5.5 (OpenAI direct, key live 2026-06-17). `POST /v1/responses`, `reasoning:{effort:"low"}`, `max_output_tokens:4500`, no `temperature` (rejected). 3 batches of 6 virtues (smoke first → batch validated format before continuing). Answer read from `output[].content[].text` (the `output_text` top-level field is empty for this provider).
- **Grounding catalog:** the **7 Fallacy family PKs** (1, 175, 594, 696, 798, 887, 1280) + **63 named depth-3 fallacies** (9 per family), extracted via CSV-aware parsing (`csv.DictReader` — naive `awk -F','` corrupts on quoted multilingual fields). The prompt forbids referencing any PK outside this catalog.
- **Family mirror as hard constraint:** each Virtue family's `prevented_family_pk` is fixed (pilot §3), so the model cannot invent a family mapping.
- **Verification (three independent layers, all re-checked against the REAL corpus, not the prompt catalog):**
  1. *Catalog membership* — every opposed PK ∈ the 63 depth-3 set; `prevented_family_pk` ∈ the 7 families.
  2. *Ground-truth* — every PK re-verified against the real 1408-row Fallacies CSV; opposed-PK ↔ name and family-PK ↔ name cross-checked character-for-character.
  3. *Mirror consistency* — each virtue's `prevented_family_pk` equals the pilot §3 mirror for its Virtue family.
- **Result: 18/18 annotated, 0 violations across all three layers.** [PASS — structural integrity + mirror consistency verified.]

Generation/verify script: `tmp/499_gen.py` (ephemeral, not committed). Raw model output: `tmp/499_gen_batch{A,B,C}.json` (ephemeral).

## 5. Sample of the 18 rows (full set in the CSV)

| Virtue (pk) | → Fam. (PK) | Opposes (PK = fallacy) | Walton scheme | CQ restored |
|---|---|---|---|---|
| Généralisations justifiées (60) | Erreur math (594) | 596 Échantillon biaisé · 759 Conclusion hâtive | Argument from Example | L'échantillon est-il représentatif et suffisant ? |
| Causalités bien identifiées (80) | Erreur raisonn. (696) | 707 Inversion de causalité · 719 Effet cigogne | Argument from Cause to Effect | La relation causale est-elle établie sans confondre cause et corrélation ? |
| Comparaisons justifiées (142) | Abus langage (798) | 834 Comparaison abusive · 839 Fausse analogie | Argument from Analogy | Les ressemblances invoquées sont-elles pertinentes ? |
| Clarté linguistique (146) | Abus langage (798) | 847 Amphibologie · 855 Équivoque | Argument from Verbal Classification | Les termes sont-ils univoques ? |
| Respect de la personne (207) | Obstruction (1280) | 1398 Attaque personnelle · 1352 Empoisonnement du puits | Argument from Bias | La critique porte-t-elle sur l'argument, pas la personne ? |

Machine-readable: [`499-scaleup-phase1-annotations.csv`](499-scaleup-phase1-annotations.csv).

## 6. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Virtues - Taxonomy.csv` (no schema cols added, no rows changed). Awaiting jsboige approval.
- ❌ Does **not** touch OWL, EPITA consumer, cards, or mindmaps.
- ❌ Does **not** claim the content is final — Phase 1 is the depth-2 backbone; depth-3…7 refinement (Phase 2+) follows, then the gated prod write.

## 7. Proposed next steps (gated on jsboige)

1. **jsboige validates** the 18 Phase-1 rows (paradigm + content).
2. **Phase 2+** — refine the depth-3…7 leaves per family (Raisonnement valide = 55 nodes is the largest; Langage exact = 18 the smallest), same method + anti-fab.
3. On approval of the full set: add the 12 columns to the production Virtues CSV (empty), populate via DatasetUpdater gpt-5.5 using pilot + Phase 1 as calibration, then propagate to OWL + wire the EPITA consumer.

---

*GATED proposal. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
