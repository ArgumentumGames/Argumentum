# Rules i18n Coverage Audit (15 records × 8 languages) — data pendant of verdict #140

**Date:** 2026-07-03 (po-2024, base master `9c19e51a`)
**Scope:** `Cards/Rules/Argumentum Rules - Cards.csv` — cell-by-cell coverage audit, **read-only** on the CSV.
**Dispatch:** ai-01 deep-queue (msg `kp26j6`) — data pendant of the visual verdict #140 (5 game sets confirmed visually on the FR TarotCards bundle).

## TL;DR

- **Primaire (résidus trad Fallacies EN) = NO-OP.** 0 empty `desc_en` cell across all 1408 Fallacies rows; PK 371 and 607 (cited as empty) are **already populated** (120 / 79 chars). Scanner #647 TOTAL = 0; 0 EN cell identical-to-FR; 0 EN cell reading as French. The #351/#308 drift (40 PK) was resolved in an earlier cycle. Nothing to fill, no gpt-5.5 call warranted.
- **Secondaire (audit Rules coverage) = 120/120 OK.** All 15 records × 8 languages are populated, in the correct script, with 0 residual contamination. Data-side confirmation of the visual verdict #140 (5 game sets intact).

## Rules audit — coverage matrix (code=truth)

15 records (`Rules_01`…`Rules_15`), 8 language columns (`Text`/`Text_en`/`Text_ru`/`Text_pt`/`Text_ar`/`Text_es`/`Text_zh`/`Text_fa`). For each cell: populated? + script matches expected (latin for fr/en/es/pt, cyrillic for ru, arabic for ar/fa, cjk for zh)?

| pk | fr | en | ru | pt | ar | es | zh | fa |
|---|---|---|---|---|---|---|---|---|
| Rules_01 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules_02 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules_03 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules_04 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules_05 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules_06 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules_07 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules_08 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules_09 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules_10 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules_11 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules_12 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules_13 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules_14 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules_15 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |

**Empty cells: 0. Script mismatches: 0.** All 120 cells populated with the correct script for their language.

### False-positive probes (verified legitimate, not contamination)

Two flags from the initial heuristic scan were investigated and **cleared**:

- **Rules_01 `Text_zh`** flagged "latin?" by the script heuristic. Real content: `# Argumentum\n## 说谎者学校` — "说谎者学校" (5 hanzi, "School of Liars"). The latin-script word "Argumentum" (the brand name, kept in latin across all languages) inflated the latin count past the threshold; the CJK content is present and correct. ✅ Not a defect.
- **`embromador` (PT, Rules_04/05/06/12)** flagged by a garbage probe. Real meaning: the **canonical PT term for "baratineur / smooth talker"** (the player role), used 77× across `Cards/Scenarii/...Cards.csv` PT. It is the correct game-term translation, not MT garbage. ✅ Not a defect. (The probe was over-broad — `embromador` was incorrectly borrowed from the #642 worklist garbage list.)

### Post-#640 contamination re-check

Scanner #647 on `Cards/Rules/Argumentum Rules - Cards.csv` = **0 finding**. The #633 garbage ("English Channel", "picareta", etc.) purged by #640 is confirmed gone — no residual MT contamination in any language column.

## Primaire (Fallacies EN residuals) — NO-OP evidence

The dispatch cited "2 empty `desc_en` (PK 371 and 607)" + "re-scan drift EN/RU/PT (#351/#308 = 40 PK)". Verified empirically (code=truth, per [[matcher-no-match-is-not-content-absent]] / [[test-counter-empirical-dotnet-test]]):

| Check | Method | Result |
|---|---|---|
| Empty `desc_en` (FR canon non-empty) | `csv` scan, 1408 rows | **0** |
| PK 371 `desc_en` | direct read | **120 chars** (populated) |
| PK 607 `desc_en` | direct read | **79 chars** (populated) |
| Scanner #647 drift | committed scanner | **TOTAL: 0** (all datasets) |
| EN cell identical-to-FR | accent-normalised compare | **0** |
| EN cell reading as French | stopword-density heuristic | **0** |

The PKs cited exist (PK range 0–1407), but their `desc_en` are **already filled**. The #351/#308 drift (40 PK) was resolved in an earlier cycle. **No fill, no gpt-5.5 call warranted** — reporting the no-op rather than fabricating work.

## Method / reproducibility

- Rules audit: `csv` read of `Cards/Rules/Argumentum Rules - Cards.csv`, script classification per cell (latin/cyrillic/arabic/cjk by Unicode block counts), garbage probe + manual verification of the 2 flags.
- Fallacies no-op: `csv` read of `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, direct PK reads, scanner #647 run.
- Read-only on both CSVs. 0 write.

Relates #640 (Rules i18n refonte), #647 (scanner), #140 (visual verdict), #351/#308 (drift, resolved). Base master `9c19e51a`.
