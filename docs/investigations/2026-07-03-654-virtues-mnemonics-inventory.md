# #654 — Virtues Syllogistic Mnemonics: Inventory (analysis-only, feeds A/B scope arbitration)

**Date:** 2026-07-03 (po-2024, base master `ca5db818`)
**Issue:** #654 — mnémoniques Virtues normalisation (DEFER post-tag, scope A vs B à arbitrer par jsboige)
**Type:** Analysis-only. **0 CSV write.** Produces the exact inventory the #654 DoD asks for, replacing the "~40-60 estimé".

## What "mnémoniques" means here

Clarified from [`docs/taxonomy/499-virtues-scaleup-phase2-raisonnement-b.md`](../taxonomy/499-virtues-scaleup-phase2-raisonnement-b.md) §86: the "colonnes mnémoniques" are the **classical syllogistic mnemonics** (Barbara, Celarent, Darii, Ferio, … Bamalip) carried by the **depth-7 CQ rows** of the Virtues taxonomy. Each names the exact premise quantity/quality of a syllogistic mode. The "incohérences de forme entre langues" in #654 = **how these Latin terms are handled across the 8 languages** — kept as Latin loanwords vs transliterated to native script.

## Verified inventory (code=truth)

Source: `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv`. **20 rows** carry a syllogistic mnemonic in their FR cell, spanning all **19 classical mnemonics** (Barbara, Celarent, Darii, Ferio, Cesare, Camestres, Festino, Baroco, Darapti, Felapton, Disamis, Datisi, Bocardo, Ferison, Camenes, Dimatis, Fesapo, Fresison, Bamalip). Pks 106–127 (depth-7 CQs).

### Per-language: is the Latin mnemonic kept (Latin chars) or transliterated?

| Language | Latin-mnemonic kept | Verdict |
|---|---|---|
| FR | 20 / 20 | Uniform — KEPT-Latin ✅ |
| EN | 20 / 20 | Uniform — KEPT-Latin ✅ |
| ES | 20 / 20 | Uniform — KEPT-Latin ✅ |
| PT | 20 / 20 | Uniform — KEPT-Latin ✅ |
| RU | 6 / 20 | **Inconsistent** — 6 kept Latin, 14 transliterated to Cyrillic |
| AR | 4 / 20 | **Inconsistent** — 4 kept Latin, 16 in Arabic script |
| ZH | 14 / 20 | **Inconsistent** — 14 kept Latin, 6 in CJK |
| FA | 3 / 20 | **Inconsistent** — 3 kept Latin, 17 in Farsi script |

**Total transliterated-away cells (the inconsistency surface): 53** — RU 14 + AR 16 + ZH 6 + FA 17. This is the verified count replacing the "~40-60 estimé".

### Per-row presence matrix (T = Latin mnemonic kept)

| pk | mnemonic | fr | en | es | pt | ru | ar | zh | fa |
|---|---|---|---|---|---|---|---|---|---|
| 106 | Barbara | T | T | T | T | T | T | T | T |
| 107 | Celarent | T | T | T | T | T | T | T | T |
| 108 | Darii | T | T | T | T | T | . | T | . |
| 109 | Ferio | T | T | T | T | T | . | T | . |
| 111 | Cesare | T | T | T | T | T | T | T | T |
| 112 | Camestres | T | T | T | T | . | . | T | . |
| 113 | Festino | T | T | T | T | . | . | . | . |
| 114 | Baroco | T | T | T | T | . | . | . | . |
| 115 | Darapti | T | T | T | T | . | T | T | . |
| 116 | Darapti | T | T | T | T | . | . | . | . |
| 117 | Felapton | T | T | T | T | . | . | . | . |
| 118 | Disamis | T | T | T | T | . | . | T | . |
| 119 | Datisi | T | T | T | T | . | . | T | . |
| 120 | Bocardo | T | T | T | T | T | . | T | . |
| 121 | Ferison | T | T | T | T | . | . | T | . |
| 123 | Camenes | T | T | T | T | . | . | T | . |
| 124 | Dimatis | T | T | T | T | . | . | T | . |
| 125 | Fesapo | T | T | T | T | . | . | T | . |
| 126 | Fresison | T | T | T | T | . | . | . | . |
| 127 | Bamalip | T | T | T | T | . | . | . | . |

## Scope arbitration input (for jsboige)

**Scope A — FR/EN/ES/PT only: 0 cells.** These are already uniform (all KEPT-Latin, 20/20). Normalising scope A is a **no-op** — there is nothing to fix. If the goal is "ensure Latin-script langs keep the Latin mnemonic", the inventory confirms they already do.

**Scope B — + RU/AR/ZH/FA: 53 cells of genuine inconsistency.** This is where the work is. For each of the 4 non-latin languages, the per-row matrix above shows exactly which mnemonics are kept-Latin vs transliterated. The arbitration is **per-language**: should that language **keep all 20 as Latin loanwords** (consistent with FR/EN, foreign term in native text) or **transliterate all 20 to native script** (consistent native reading, but the technical Latin term loses recognisability)?

- **RU (14 transliterated / 6 Latin):** currently leans transliterated. Decision: finish transliterating the 6, or revert the 14 to Latin.
- **AR (16 Arabic / 4 Latin):** currently leans Arabic. Decision: finish the 4, or revert the 16.
- **ZH (6 CJK / 14 Latin):** currently leans Latin. Decision: finish the 6 to Latin, or transliterate the 14 to CJK.
- **FA (17 Farsi / 3 Latin):** currently leans Farsi. Decision: finish the 3, or revert the 17.

### Recommendation (analysis, not verdict)

The 4 non-latin languages currently lean **in opposing directions** (RU/AR/FA lean transliterated; ZH leans Latin). A single global rule would be cleanest but touches all 53 cells + reverses recent transliterations. A per-language rule (respect each language's current majority direction) minimises churn: finish RU/AR/FA toward transliterated (+6+4+3 = 13 cells), finish ZH toward Latin (+6 cells) = **19 cells** to converge, vs 53 for a global flip. **Final verdict = jsboige.**

## Method / reproducibility

Inventory generated by `build_654_mnemonic_inventory.py` (scratchpad) — read-only, scans `title_*`/`description_*`/`remark_*` per language for the 19 classical mnemonics (regex word-boundary match), classifies Latin-presence. Script + JSON output persisted in scratchpad. No CSV modified.

Relates #654, #647 (scanner), #499 (depth-7 CQ rationale), #202 (epic finalization). Base master `ca5db818`.
