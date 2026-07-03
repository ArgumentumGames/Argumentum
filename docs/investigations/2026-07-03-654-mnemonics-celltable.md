# #654 — Mnemonics apply-ready cell table (refinement of #660, staged post-tag)

**Date:** 2026-07-03 (po-2024, base master `27442add`)
**Dispatch:** `29u572` (ai-01), idle task — "prépare la table exacte des cellules par langue prête à appliquer selon la décision"
**Type:** Analysis-only refinement of the #660 inventory. **0 CSV write. Staged post-tag.** Arbitration A/B/global = jsboige (ai-01 presents this cycle).

## What this adds vs #660

#660 gave the T/. presence matrix (which mnemonic-rows keep Latin per language). This doc extracts the **exact pks** of the non-conforming cells per language — the apply-ready set. **The cell set is decision-independent**; only the target value (Latin loanword vs native-script transliteration) depends on jsboige's direction choice. So once the decision lands, application is mechanical.

## Exact cell table (code=truth, `build_654_celltable.py`, read-only)

20 mnemonic rows (pks 106–127), 4 non-latin languages. Per language: which pks **keep Latin** vs are **transliterated/absent**.

### RU (6 kept-Latin / 14 transliterated) — majority = transliterated
- **KEPT-Latin (6):** pks 106, 107, 108, 109, 111, 120
- **Transliterated (14):** pks 112, 113, 114, 115, 116, 117, 118, 119, 121, 123, 124, 125, 126, 127

### AR (4 kept-Latin / 16 transliterated) — majority = transliterated
- **KEPT-Latin (4):** pks 106, 107, 111, 115
- **Transliterated (16):** pks 108, 109, 112, 113, 114, 116, 117, 118, 119, 120, 121, 123, 124, 125, 126, 127

### ZH (14 kept-Latin / 6 transliterated) — majority = Latin
- **KEPT-Latin (14):** pks 106, 107, 108, 109, 111, 112, 115, 118, 119, 120, 121, 123, 124, 125
- **Transliterated (6):** pks 113, 114, 116, 117, 126, 127

### FA (3 kept-Latin / 17 transliterated) — majority = transliterated
- **KEPT-Latin (3):** pks 106, 107, 111
- **Transliterated (17):** pks 108, 109, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 123, 124, 125, 126, 127

**Totals:** 27 kept-Latin cells + 53 transliterated cells = 80 non-latin cells (4 langs × 20 mnemonics).

## Decision scenarios (cells to change) — the SET per scenario

| Scenario | Direction | Cells to change | Which |
|---|---|---:|---|
| **S1 — per-lang min-churn** | Respect each lang's current majority | **19** | RU 6 (→translit) + AR 4 (→translit) + ZH 6 (→Latin) + FA 3 (→translit) |
| **S2 — global keep-Latin** | Revert all transliterated → Latin | **53** | all transliterated cells (RU 14 + AR 16 + ZH 6 + FA 17) |
| **S3 — global transliterate** | Convert all kept-Latin → native | **27** | all kept-Latin cells (RU 6 + AR 4 + ZH 14 + FA 3) |

**Min-churn = S1 (19 cells).** Most disruptive = S2 (53). Note pks 106/107/111 are KEPT-Latin in all 4 langs (the "anchor" mnemonics Barbara/Celarent/Cesare) — the most stable across scenarios.

## Application (post-tag, post-arbitration — NOT done here)

Once jsboige picks a direction, the application is a deterministic cell-by-cell edit:
- **S1:** for each lang, flip the minority-direction cells (the pks listed above) to the majority script.
- **S2/S3:** bulk-flip the listed pk-set per lang.
- The mnemonic Latin form (e.g. `Barbara`) is the search key; the transliteration per lang must be sourced from the existing transliterated cells (consistency with the lang's established rendering) — NOT machine-transliterated, to avoid script/term drift.

**gpt-5.5 is NOT appropriate here** — this is script-conversion (Latin ↔ Cyrillic/Arabic/CJK/Farsi) of a fixed technical term, not translation. The transliteration already exists in the transliterated cells (the lang's own rendering); S1/S3 just propagate it to the minority cells.

## Method / reproducibility

`build_654_celltable.py` (scratchpad) — read-only scan of `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv`, regex `\b(Barbara|Celarent|…|Bamalip)\b` per lang on title/description/remark. Output: `654_celltable.json` (scratchpad). 0 CSV write.

Relates #654, #660 (original inventory), #499 (depth-7 CQ rationale), #29u572 (dispatch). Base `27442add`.
