# #642 — GDrive Rules Sync: Migration Table (24 → 21) Audit Trail

**Generated:** 2026-07-03 (po-2024, base master `cb989051`)  
**Companion to:** [`2026-07-02-gdrive-rules-642-structural-analysis.md`](2026-07-02-gdrive-rules-642-structural-analysis.md) (PR #646)

## Context

Google Sheet **"Argumentum Rules"** (`1jnhlod6…`, partner juliadespb) had a legacy `Cards` tab with **24 rows × 5 cols** (pk + Text/Text_en/Text_ru/Text_pt). The repo master has **21 canonical Rules records** (15 main + 6 Print&Play), each with **8 language columns** (FR/EN/RU/PT + AR/ES/ZH/FA). Option 3 (jsboige) was applied:

- **2 clean sheets created & populated byte-perfect** from repo master:
  - `Rules main` (16×10 = header + Rules_01..15)
  - `Rules PP` (7×10 = header + RulesPP_01..06)
- **Legacy `Cards` tab archived** under neutral name `Cards (archive 24x5)` (content intact, reversible).

This table records, for each legacy row, the repo record it maps to and the migration verdict — the audit trail proving no authentic partner content was silently lost.

## Summary

| Verdict | Count | Meaning |
|---|---|---|
| IDENTICAL | 1 | Legacy FR text == repo record FR (already in master) |
| PRESERVED | 0 | Legacy ≈ repo record (cov ≥ 80%): content lives in the clean sheet |
| FRAGMENT | 1 | Legacy row = part of a repo record (markdown split across GDrive rows) |
| GARBAGE-MT | 10 | Machine-translation garbage ("English Channel", "picareta"…) — superseded by clean gpt-5.5 translations |
| NO-MATCH | 12 | No repo record matched |
| **Total legacy rows** | **24** | |

**Partner-edited cells (authentic juliadespb content differing from repo, non-garbage):** 25 across 24 rows. All are preserved verbatim in the archived `Cards (archive 24x5)` tab — nothing was overwritten destructively.

> **Note on NO-MATCH (12 rows):** these are **variant game rules** not present in the repo `Cards/Rules/` CSV (which holds only the base game, Rules_01..15 + RulesPP_01..06). The legacy sheet bundled multiple game variants in one tab — "Le Bingo mixologie argumentative" (row 7), "Le dernier beau parleur" (row 10), "Le moulin à baratin" (row 15), "La parlote coinchée" (row 20), and their sub-rows. **They are preserved verbatim in `Cards (archive 24x5)`** — NO-MATCH means "no repo counterpart to mirror", not data loss. A future scope (out of #642) could promote these variants into the repo if the base-game-only CSV is to be extended.

## Why row-to-row mapping is lossy

The legacy sheet used a **24-row linear layout** that splits multi-section markdown rules across rows (e.g. GDrive row 4 ends mid-rule with "Le baratineur expose…" and row 5 begins "### 4. Le jury"), whereas the repo stores each rule as a **single cell with embedded `\n` line breaks**. A 1:1 row mapping is therefore structurally impossible (proven in the companion analysis). The clean-sheet approach (mirror the repo's own record granularity) is the lossless resolution; this table documents the *best-effort* per-row mapping for traceability.

## Migration table (24 legacy rows → 21 repo records)

| # | Legacy FR preview | Best repo match | Verdict |
|---|---|---|---|
| 1 | # Argumentum ## L'école des menteurs | Rules_01 (main, cov=100%) | IDENTICAL |
| 2 | *Règles du jeu : de 4 à 8 joueurs* ## Matériel * 1 paquet de cartes d’… | RulesPP_02 (pp, cov=67%) | GARBAGE-MT |
| 3 | ## Installation Selon le nombre de joueurs et le niveau de difficulté … | RulesPP_03 (pp, cov=55%) | GARBAGE-MT |
| 4 | ## Déroulé de la manche ### 1. Le piocheur Le piocheur tire une carte … | RulesPP_04 (pp, cov=89%) | GARBAGE-MT |
| 5 | Le baratineur expose ses arguments et le piocheur lui donne la répliqu… | RulesPP_05 (pp, cov=33%) | GARBAGE-MT |
| 6 | En cas d’égalité (🥈👇👇≟🥈👇👇), le baratineur l'emporte (✅🎭➜🎭🏆), et à défa… | Rules_06 (main, cov=29%) | GARBAGE-MT |
| 7 | # Argumentum ## Le Bingo mixologie argumentative | — | NO-MATCH |
| 8 | *Règles du jeu : de 1 à 20 joueurs* ## Matériel * 1 débat ou 1 discour… | — | NO-MATCH |
| 9 | ## Pendant le débat Chaque joueur écoute les arguments formulés. S'il … | Rules_08 (main, cov=30%) | FRAGMENT |
| 10 | # Argumentum ## Le dernier beau parleur | — | NO-MATCH |
| 11 | *Règles du jeu : de 1 à 8 joueurs* ## Matériel * 1 paquet de cartes d’… | — | NO-MATCH |
| 12 | ## Installation Selon le nombre de joueurs et le niveau de difficulté … | RulesPP_03 (pp, cov=18%) | GARBAGE-MT |
| 13 | ## Déroulé de la manche ### 1. Le piocheur Le piocheur tire une carte … | Rules_10 (main, cov=20%) | GARBAGE-MT |
| 14 | ## Fin de partie et décompte Si un joueur arrive à se débarasser de to… | — | NO-MATCH |
| 15 | # Argumentum ## Le moulin à baratin | — | NO-MATCH |
| 16 | *Règles du jeu : de 2 à 8 joueurs* ## Matériel * 1 paquet de cartes d’… | — | NO-MATCH |
| 17 | ## Installation Selon le nombre de joueurs et le niveau de difficulté … | RulesPP_03 (pp, cov=9%) | GARBAGE-MT |
| 18 | ## Déroulé de la manche ### 1. Le piocheur Le piocheur tire une carte … | Rules_12 (main, cov=21%) | GARBAGE-MT |
| 19 | ## Fin de partie Le premier joueur arrivant à 20 points emporte la par… | — | NO-MATCH |
| 20 | # Argumentum ## La parlote coinchée | — | NO-MATCH |
| 21 | *Règles du jeu : 4 joueurs* ## Matériel * 1 paquet de cartes d’argumen… | — | NO-MATCH |
| 22 | ## Installation On sélectionne 28 cartes d'arguments fallacieux pour l… | — | NO-MATCH |
| 23 | ## Début de la manche ### 1. Le piocheur Le piocheur tire une carte de… | Rules_14 (main, cov=25%) | GARBAGE-MT |
| 24 | ### 3. Décompte de la manche La manche s'arrête quand tous les plis on… | — | NO-MATCH |

## Verification of the clean sheets (post-upload)

- `Rules main` readback vs repo matrix: **EXACT MATCH byte-for-byte** (16 rows, 160 cells)
- `Rules PP` readback vs repo matrix: **EXACT MATCH byte-for-byte** (7 rows, 70 cells)
- Method: REST `values:batchUpdate` (`valueInputOption=RAW`), `\n` preserved as in-cell line breaks
- Triple-verified: REST diff + MCP `read_sheet_values` + dimension check (16/7 exact rows, no split)

## Closes

Relates to #642. Base master `cb989051`. Companion: structural analysis (#646), scanner anti-FP (#647).
