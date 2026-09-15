# Taxonomy cross-language terminological coherence scan (release sign-off)

**Author**: po-2024 (worker) · **Date**: 2026-07-04 · **Base**: master `d5913862`
**Dispatch**: `lofjtd` (ai-01), secondaire — "une passe de cohérence terminologique cross-langue non encore couverte"
**Status**: **Read-only scan, 0 finding. Release sign-off.** 0 CSV write, 0 gpt-5.5 call.

---

## TL;DR

A cross-language **terminological coherence** scan of the Fallacies taxonomy family/subfamily labels (7 non-FR languages × 3 hierarchical levels). The angle: does the same FR source label map to a **consistent** localized rendering, or do rows diverge? **Result: 0 inconsistencies** at every meaningful granularity. The localized family tree branches intentionally (a sub-family may localize differently than FR's umbrella), but within each (Famille, Sous-Famille, Soussousfamille) group, every language is internally consistent.

This complements the [polish trad sweep #667](../investigations) (scanner #647 = FR-contamination, 0 finding) and #192 (FR-relative terminology apply): those check *is it translated / is the FR term harmonized*; this checks *is each translation internally consistent across rows*. The release i18n surface is coherent on all three axes.

## Why this scan (the gap not yet covered)

| Prior pass | Axis | Coverage |
|---|---|---|
| Scanner #647 (#667) | FR-contamination (untranslated content) | ✅ 0 finding |
| #192 terminology apply | FR-relative term harmonization | ✅ applied (post-ratification) |
| **This scan** | **Intra-lang consistency: same FR source → same localized label across all rows** | ✅ **0 finding (this doc)** |

The scanner #647 catches "this EN cell still contains FR text". It does NOT catch "this EN cell says 'Fallacy' but another row with the same FR `Famille` says 'Sophism'". That intra-lang divergence is what this scan checks.

## Method (code=truth, read-only)

Source: `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` (1408 rows). For each FR grouping key, collect the set of localized renderings per language and flag any group where a language has >1 distinct value.

**Naïve check** (group by `Famille` only): 7/8 top-level FR families show >1 rendering in some language (e.g. FR `Tricherie` → ZH renders as `人性偏见` / `作弊` / `偏见思维`). **This is expected, not a defect** — it reflects that the localized taxonomy tree branches at the sub-family level differently than FR's umbrella.

**Tight check** (the meaningful one): group by the *full* FR key and verify each language is consistent within the group:

| Granularity | Groups | Inconsistent groups | (group, lang) cases |
|---|---:|---:|---:|
| `Family_<lang>` by (Famille, Sous-Famille) | 29 | **0** | **0** |
| `Subfamily_<lang>` by (Famille, Sous-Famille) | 21 | **0** | **0** |
| `Subsubfamily_<lang>` by (Famille, Sous-Famille, Soussousfamille) | 63 | **0** | **0** |
| FR `Sous-Famille` → consistent `Subfamily_<lang>` | 21 | **0** | **0** |

**Verdict**: at every meaningful granularity, each FR source label maps to exactly one localized rendering per language. The localized family/subfamily/subsubfamily trees are internally consistent across all 1408 rows × 7 languages (EN/RU/PT/ES/AR/FA/ZH).

## What this scan does NOT cover (scope, not findings)

- **Proper-noun consistency** (Sherlock, Jeanne d'Arc, Ergo sum…) lives in the **Scenarii** CSV, not the Fallacies taxonomy — out of this scan's scope (those are legitimate per-language proper-noun renderings, per MEMORY [[i18n-coverage-gap-is-link-urls]]).
- **`link_*` URL consistency** — the only known i18n gap (~2919 resolvable cells, human-research not gpt-5.5), tracked under #600/#606. Not a terminological axis.
- **Content prose** (`text_*`/`desc_*`/`example_*`) translation quality — covered by #667's scanner (FR-contamination) + native-speaker validation deferred per decision #27.

## Release implication

For v0.9.0, the Fallacies taxonomy i18n surface is **coherent** on the terminological axis (this scan) + **uncontaminated** on the FR-leak axis (#667). Both = 0 finding. The 8-language bundle ships with consistent family/subfamily labeling.

## Reproducibility

`build_coherence_scan.py` (scratchpad) — read-only grouping scan of the prod taxonomy CSV. Deterministic. 0 write.

Relates #141 (i18n taxonomy lane), #192 (terminology apply), #667 (polish sweep = scanner #647), #600/#606 (link_* gap, separate axis). Base `d5913862`.
