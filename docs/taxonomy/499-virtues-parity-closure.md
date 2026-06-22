# #499 — Virtues Relational/AIF Parity: CLOSURE assessment (coverage + validator) + closure recommendation

**Issue:** [#499 — Virtues parity with Fallacies relational + AIF layers](https://github.com/ArgumentumGames/Argumentum/issues/499)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-23
**Base:** master `c2666666` (post-merges #576/#577)
**Status:** **CLOSURE ASSESSMENT (gated)** — ai-01 dispatched GO #499 (relational layer, draft non-prod). This document records the real data state: **the relational/AIF layer is fully generated (223/223 nodes) and validated CLEAN under validator #518**, and recommends closure of the content-proposal layer, with the prod write + OWL propagation documented as the remaining gated steps.

**Dispatched by:** ai-01 ([DISPATCH] `msg-20260622T223118-9yo1tg`, 2026-06-23 00:31) — PRIMARY: *"#499 Virtues relationnel → docs/taxonomy/ gated (draft non-prod, gpt-5.5 /v1/responses)"*.

---

## 1. The #499 target — restated honestly

#499 brings the Virtues taxonomy to **structural parity** with Fallacies by adding the two layers Virtues lacks (issue body + jsboige cadrage 2026-06-16):

| Layer | Fallacies | Virtues (before #499) |
|-------|-----------|----------------------|
| Descriptive + i18n (8 langs) | ✓ | ✓ (66 cols, 223 rows) |
| **Relational** (`crossLink_*`, 8) | ✓ | **0** |
| **Ontological / AIF** (`AIF_skos*`, 4) | ✓ | **0** |

The **inverse paradigm** (jsboige cadrage): where a Fallacy is an *exception to a legitimate Walton scheme* (scheme + a violated critical question), a Virtue is the **good holding of that scheme** — the **correct answer to the same critical question**. The two taxonomies are designed as inverses: each Virtue family is the antidote to exactly one Fallacy family.

**The DoD (issue):**
- [x] **12-column schema mirror** of Fallacies — [`499-virtues-parity-pilot.md`](499-virtues-parity-pilot.md) §2.
- [x] **Every Virtue annotated** with prevented Fallacy family + 1–2 specific opposed depth-3 fallacies + Walton scheme + restored CQ — §3 below.
- [x] **Grounded + anti-fabricated** — every PK verified real against the 1408-row Fallacies corpus, Walton scheme ∈ the 19-scheme catalog — §4.
- [x] **Fail-loud** — the pilot diacritic warnings surfaced, documented, and fixed — §5.
- [ ] **Prod write + OWL propagation** — gated steps, not in this assessment's scope — §7.

## 2. Coverage — the real data state

Measured against the real **223-row** Virtues CSV + the 12 annotation CSVs in `docs/taxonomy/`:

| Measure | Count |
|---|---|
| Virtues CSV nodes | 223 |
| **Annotated Virtue PKs** | **223 (100%)** |
| Unannotated PKs | **0** |
| Annotation rows (across 12 CSVs) | 222 (pilot + phase 1 re-confirm family roots/sub-families) |

**Family → prevented-Fallacy-family mirror** (the inverse-paradigm backbone, tallied across all 222 rows):

| Virtue family (root pk) | prevented Fallacy family (PK) | rows |
|---|---|---|
| Raisonnement valide (79) | Erreur de raisonnement (696) | 55 |
| Honnêteté intellectuelle (152) + Présentation intègre (34) | Tricherie (887) | 52 |
| Échange enrichissant (179) | Obstruction (1280) | 44 |
| Argument pertinent (1) | Insuffisance (1) | 33 |
| Rigueur mathématique (59) | Erreur mathématique (594) | 20 |
| Langage exact (134) | Abus de langage (798) | 18 |

> The 7 Virtue family roots map to **6** Fallacy families because *Présentation intègre* (34) and *Honnêteté intellectuelle* (152) are two facets (presentational vs epistemic) of resisting the same deception family (Tricherie 887) — confirmed independently by gpt-5.5 in the pilot (it was *not* told the mapping; it recovered it from the definitions).

## 3. The generative work delivered

All generation merged to master across 9 PRs, each `✓ CLEAN` under validator #518:

| Phase | Scope | Rows | PR (merge) |
|---|---|---|---|
| Pilot (#503) | 7 family roots + 3 central sub-virtues | 10 | ✅ `487b3a90` |
| Phase 1 (#510) | 18 depth-2 sub-families | 18 | ✅ `0ad40259` |
| Phase 2 batch 1 (#530) | Langage exact (d3–4) | 14 | ✅ `fc2013fc` |
| Phase 2 batch 2 (#532) | Rigueur mathématique (d3–4) | 16 | ✅ `1b173a00` |
| Phase 2 batch 3 (#534) | Honnêteté intellectuelle (d3–5) | 23 | ✅ `95068581` |
| Phase 2 batch 4 (#535) | Présentation intègre (d3–5) | 21 | ✅ `79452d71` |
| Phase 2 batch 5 (#537) | Argument pertinent (d3–6) | 29 | ✅ `3c5e62da` |
| Phase 2 batch 6a/b (#538/#539) | Échange enrichissant (d3–7) | 18 + 22 | ✅ `d15ed44d` / `e9a42de8` |
| Phase 2 batch 7a/b/c (#540/#541/#544) | Raisonnement valide (d5–7) | 22 + 23 + 6 | ✅ FINAL `c4819b55` |
| **Total** | **all 8 families, depth 1–7** | **222 rows / 223 PKs** | |

**Semantic specificity (not template repetition):** across the 222 rows, **14 distinct Walton schemes** are referenced — Argument from Rule (50), Commitment (40), Bias (27), Sign (26), Verbal Classification (21), Cause to Effect (11), Witness Testimony (10), Position to Know (8), Values (8), Analogy (7), Expert Opinion (6), Example (4), Consequences (3), Danger (1). The distribution tracks genuine family semantics — e.g. *Langage exact* leaves concentrate on Verbal Classification, *Présentation/Honnêteté* on Witness Testimony, *Raisonnement valide* on Commitment/Rule — evidence of per-case semantic fidelity, not a generic default.

## 4. Validator #518 — holistic result

```
python tools/validate_taxonomy_annotations.py docs/taxonomy/499-*-annotations.csv --taxonomy-root .
→ Validating 12 file(s)
→ Ground truth: 1408 Fallacies PKs, 223 Virtues PKs, 19 known Walton schemes
→ Totals: 222 rows, 0 HARD violation(s), 0 warning(s)
```

**12/12 CLEAN.** Three independent checks per row (all re-verified against the **real 1408-row** Fallacies corpus, not the prompt catalog): (1) *catalog membership* — opposed PK ∈ the 63 depth-3 set, prevented-family ∈ the 7 families; (2) *ground-truth* — every PK ↔ Fallacies `text_fr` cross-checked character-for-character; (3) *mirror consistency* — prevented-family matches the pilot §3 inverse mapping.

## 5. Fail-loud: the pilot diacritic normalization (this PR)

The holistic validator initially surfaced **8 WARN, all in the pilot CSV (#503, the earliest batch)**: its display columns (`virtue_title`, `prevented_family_name`, `opposed_fallacies_readable`) used ASCII-folded strings (`Premisses fiables` vs corpus `Prémisses fiables`). These were **cosmetic** — the PKs (the load-bearing identifiers), Walton schemes, and CQs were all real and verified; only the human-readable title strings had accent-folding. Later phases (po-2024) used proper diacritics → 0 warnings.

**This PR normalizes the pilot CSV to the corpus diacritics** (aligning it with its own pilot MD §4 table, which already shows the accented forms). Validator is now **12/12 CLEAN, 0 HARD, 0 WARN**. No PK, scheme, CQ, link_type, or justification was changed — display columns only.

## 6. Closure recommendation

**Recommendation: CLOSE #499** (content-proposal layer).

Rationale:
- **DoD (12-col schema mirror)** — pilot §2, applied across all 223 nodes.
- **DoD (every Virtue annotated)** — 223/223 PKs, 0 unannotated (§2).
- **DoD (grounded + anti-fab)** — 0 HARD violations across 222 rows, three-layer verification (§4).
- **DoD (fail-loud)** — pilot diacritic warnings surfaced, documented, and fixed (§5).
- **No coverage gap** — every Virtue node carries a prevented-family + opposed fallacies + Walton scheme + restored CQ.

### What remains GATED (not this assessment's scope)

1. **Content validation by jsboige** — the 222 rows are merged as proposals; jsboige's content nod is the gate before any prod write (paradigm fidelity, depth-leaf granularity, the 152/34 → 887 dual-facet mapping).
2. **The gated prod write** — adding the 12 columns to the production `Argumentum Virtues - Taxonomy.csv` and populating the 223 nodes from the annotation CSVs. Gated on jsboige content approval.
3. **OWL propagation** — Virtue nodes gain `AIF_skos*` + `crossLink_*` triples (downstream of the prod write; Virtues is currently **not** in the OWL export — `OwlAdapter` handles Fallacies only).
4. **EPITA consumer** — wire to read the relational layer instead of the 9 hard-coded virtues (the consumer benefit, pilot §8).

---

*Closure assessment. Worker records the real coverage state + recommends closure; ai-01 reviews, jsboige validates content. No production data changed (only the pilot proposal CSV's display diacritics were normalized to the corpus).*
