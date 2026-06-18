# #499 — Virtues Relational/AIF Parity Pilot (GATED proposal)

**Issue:** [#499 — Virtues parity with Fallacies relational + AIF layers](https://github.com/ArgumentumGames/Argumentum/issues/499)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-16
**Base:** master `50247b77`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Virtues CSV change until jsboige approves.** This document + the pilot CSV are the proposal.

---

## 1. The gap (confirmed concretely)

The Fallacies taxonomy carries two structural layers the Virtues taxonomy **does not have at all** — the columns are absent from the schema, not merely empty:

| Layer | Fallacies cols | Virtues cols |
|-------|---------------|--------------|
| Descriptive + i18n (8 langs) | ✓ | ✓ (66 cols, 223 rows) |
| **Relational** (`crossLink_*`) | **8** (PredatesOn, Denounces, Levarages, Allows, Opposes, Inverts, Mirrors, IsRelatedTo) | **0** |
| **Ontological / AIF** (`AIF_skos*`) | **4** (DirectRef, ExceptionRef, Other, MappingType) | **0** |
| **Total cols** | 102 | 66 |

Without these layers, Virtues cannot express *which fallacies a virtue prevents*, nor anchor itself to the **Argument Interchange Framework** (Walton schemes + critical questions) that Fallacies already uses. The EPITA consumer currently hard-codes 9 virtues because the taxonomy cannot answer "what does this virtue protect against?"

## 2. Proposed schema (mirror of Fallacies, no invention)

Add to `Argumentum Virtues - Taxonomy.csv` the **same 12 columns** Fallacies uses, verbatim names:

```
crossLink_PredatesOn, crossLink_Denounces, crossLink_Leverages, crossLink_Allows,
crossLink_Opposes, crossLink_Inverts, crossLink_Mirrors, crossLink_IsRelatedTo,
AIF_skosDirectRef, AIF_skosExceptionRef, AIF_skosOther, AIF_skosMappingType
```

Convention (matches existing Fallacies usage): a `crossLink_*` cell holds one or more **target PKs** (Fallacies PKs, since the relation is cross-taxon Virtue→Fallacy). `AIF_skosMappingType` holds the Walton scheme + the critical question in a compact form. This is structural parity only — *populating* it is the pilot below.

## 3. The conceptual backbone — symmetric Virtue ↔ Fallacy family mirror

The two taxonomies are designed as **inverses**: each Virtue family is the antidote to exactly one Fallacy family. The pilot confirms this 1:1 mirror independently (gpt-5.5 was not told the mapping; it recovered it from the definitions):

| Virtue family (pk) | prevents Fallacy family (PK) | relation |
|--------------------|------------------------------|----------|
| Argument pertinent (1) | Insuffisance (1) | pertinence ↔ insufficient/irrelevant support |
| Présentation intègre (34) | Tricherie (887) | honest presentation ↔ cheating/misrepresentation |
| Rigueur mathématique (59) | Erreur mathématique (594) | exact mirror |
| Raisonnement valide (79) | Erreur de raisonnement (696) | exact mirror |
| Langage exact (134) | Abus de langage (798) | exact mirror |
| Honnêteté intellectuelle (152) | Tricherie (887) | honesty ↔ factual cheating |
| Échange enrichissant (179) | Obstruction (1280) | constructive exchange ↔ debate obstruction |

> Note on 152/34 → both map to Tricherie (887). This is correct: *Présentation intègre* and *Honnêteté intellectuelle* are two facets (presentational vs epistemic) of resisting the same family of deception. Influence (175) is reached at the **specific-fallacy** level (e.g. appeal to consequences, conditioning) rather than the family level — exercised in the pilot's `opposed_fallacy_pks`.

## 4. The pilot — 10 structuring virtues, fully annotated

**Selection:** the 7 family roots (depth 1) + 3 structurally central sub-virtues (depth 2): *Prémisses fiables* (12), *Déductions correctes* (88), *Fidélité aux faits* (153).

For each: the Fallacy family prevented, 1–2 **specific named fallacies** (real Fallacies depth-3 PKs) it directly opposes, the canonical **Walton argumentation scheme**, the **critical question** the fallacy violates and the virtue restores, the link type, and a one-line justification.

| Virtue (pk) | → Fam. (PK) | Opposes (PK = fallacy) | Walton scheme | Critical question restored (FR) |
|---|---|---|---|---|
| Argument pertinent (1) | Insuffisance (1) | 153 Argument des mauvaises raisons · 3 Argument vide | Argument from Sign | L'élément invoqué est-il un signe pertinent et fiable de la conclusion ? |
| Prémisses fiables (12) | Insuffisance (1) | 3 Argument vide · 33 Justification triviale | Argument from Position to Know | La prémisse est-elle suffisamment fiable pour soutenir la conclusion ? |
| Présentation intègre (34) | Tricherie (887) | 889 Mensonge · 942 Fausse attribution | Argument from Witness Testimony | L'orateur dit-il la vérité et attribue-t-il correctement les propos ? |
| Rigueur mathématique (59) | Erreur math (594) | 644 Probabilités faussées · 681 Erreur de calcul | Argument from Sign | Le signe quantitatif est-il calculé correctement et fiable ? |
| Raisonnement valide (79) | Erreur raisonn. (696) | 727 Erreur de logique propositionnelle · 784 Syllogisme invalide | Argument from Commitment | La conclusion découle-t-elle des prémisses par une inférence valide ? |
| Déductions correctes (88) | Erreur raisonn. (696) | 727 Erreur de logique propositionnelle · 784 Syllogisme invalide | Argument from Commitment | La conclusion découle-t-elle effectivement des prémisses ? |
| Langage exact (134) | Abus langage (798) | 800 Acception vague · 855 Équivoque | Argument from Verbal Classification | Les termes sont-ils définis et employés dans le même sens précis ? |
| Honnêteté intellectuelle (152) | Tricherie (887) | 942 Fausse attribution · 953 Attention sélective | Argument from Witness Testimony | La source est-elle rapportée fidèlement, sans sélection biaisée ? |
| Fidélité aux faits (153) | Tricherie (887) | 889 Mensonge · 953 Attention sélective | Argument from Witness Testimony | L'énoncé factuel est-il exact, complet et corroboré ? |
| Échange enrichissant (179) | Obstruction (1280) | 1313 Évasion | Argument from Commitment | L'interlocuteur répond-il à la question en discussion plutôt que de l'éviter ? |

Machine-readable version: [`499-pilot-annotations.csv`](499-pilot-annotations.csv).

## 5. Method & anti-fabrication guarantee

- **Model:** gpt-5.5 (OpenAI direct, key live 2026-06-16). One call per virtue, `max_completion_tokens=1500`, no `temperature` (reasoning models reject it — learned this tick).
- **Grounding catalog:** the **7 Fallacy family PKs + 63 named depth-3 fallacies (9 per family)**, all extracted directly from `Argumentum Fallacies - Taxonomy.csv`. The prompt forbids referencing any PK outside this catalog.
- **Verification (two layers):**
  1. *Catalog check* — every returned PK must be in the catalog (script `verify()`).
  2. *Ground-truth check* — every returned PK re-verified against the **real 1408-row Fallacies CSV**, not the catalog.
- **Result:** **29 PK references checked, 0 violations.** All family PKs confirmed depth-1. [PASS — all PKs verified real.]

Generation script: `tmp/499_pilot_gen.py` (not committed — ephemeral). Raw model output: `tmp/499_pilot_annotations.json` (not committed — `_usage` tokens included for audit; can be committed on request).

## 6. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Virtues - Taxonomy.csv` (no schema cols added, no rows changed). Awaiting jsboige approval of the column set + the 10 pilot mappings.
- ❌ Does **not** touch the OWL ontology or any consumer (EPITA harness, cards, mindmaps).
- ❌ Does **not** claim the pilot is the final content — it is the **calibration exemplar** for the scale-up to all 223 nodes.

## 7. Proposed next steps (gated on jsboige)

1. **jsboige validates** (a) the 12-column schema, (b) the family mirror in §3, (c) the 10 pilot rows in §4.
2. On approval: add the 12 columns to the production Virtues CSV (empty), then populate the **223 nodes** via a DatasetUpdater gpt-5.5 task using this pilot as few-shot calibration + the same catalog-grounding + anti-fabrication verify.
3. Propagate to the OWL ontology (Virtue nodes gain `AIF_skos*` + `crossLink_*` triples), and wire the EPITA consumer to read the relational layer instead of the hard-coded 9.

## 8. Consumer benefit (EPITA)

Once populated, the relational layer lets the consumer answer, for any virtue, *"which fallacy does this protect against, and which critical question does it enforce?"* — replacing the 9 hard-coded virtues with the full 223-node backbone, and giving every virtue a traceable Walton grounding.

---

*GATED proposal. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
