# Polish trad sweep v0.9.0 — pre-release contamination/drift = 0 (confirmed)

**Date:** 2026-07-03 (po-2024, base master `27442add`)
**Dispatch:** `29u572` (ai-01), secondaire — "sweep de vérification finale pré-release"
**Tool:** scanner #647 (`docs/investigations/scripts/2026-07-02-scan-translations.py`, merged) — 3 anti-FP heuristics
**Result:** **TOTAL: 0 finding** across all 4 datasets. Expected (audits antérieurs 100%).

## TL;DR

Final pre-release translation-quality sweep confirms the 4 datasets are clean at tag time: **0 drift**, **0 MT-contamination residual**, **0 EN-identical-to-FR**. No gpt-5.5 fill warranted. This is the release audit-trail proof that the i18n surface is ship-ready.

## Sweep result (code=truth, master `27442add`)

| Dataset | Rows | Drift | Contamination | EN=FR | Verdict |
|---|---:|---:|---:|---:|---|
| Fallacies (`Argumentum Fallacies - Taxonomy.csv`) | 1408 | 0 | 0 | 0 | ✅ clean |
| Virtues (`Argumentum Virtues - Taxonomy.csv`) | 223 | 0 | 0 | 0 | ✅ clean |
| Scenarii (`Argumentum Scenarii - Cards.csv`) | 167 | 0 | 0 | 0 | ✅ clean |
| Rules (`Argumentum Rules - Cards.csv`) | 15 | 0 | 0 | 0 | ✅ clean |
| **TOTAL** | **1813** | **0** | **0** | **0** | ✅ |

**Scanner #647 TOTAL: 0** (re-run on `27442add`). Unmapped columns reported by the scanner are structural metadata (pk/path/depth/crossLinks/AIF_*/print_and_play), not translatable content — expected, not findings.

## Context — what "clean" rests on

The 0-finding state is the cumulative result of the i18n work landed this cycle:

- **#640** Rules i18n refonte (gpt-5.5) — purged the 23 "English Channel" HIGH occurrences; 0 HIGH résiduel.
- **#653** Johnny 6.1.3 duplication fix — 6 languages; scanner TOTAL: 0 after.
- **#218/#236/#246/#290/#295** Virtues i18n (title/desc/remark × 8 langs) — 100% coverage.
- **Scenarii EN/RU/PT** — 167/167 records 100% covered (verified cell-by-cell `7206f2f9`, 2026-05-24).
- **Fallacies** — 1408 rows; #351/#308 drift (40 PK) resolved in an earlier cycle.

The #642 GDrive Rules sync (2 tabs byte-perfect) and the GDrive migration audit (#656) confirmed Rules parity repo↔GSheet (24→21, 0 char lost in the 24→15 restructure).

## Method / reproducibility

```
python docs/investigations/scripts/2026-07-02-scan-translations.py
# → TOTAL: 0
```

Scanner #647 = 3 anti-false-positive heuristics (accent-normalised EN=FR compare, FR-stopword-density "EN reading as French" probe, contamination dictionary). Read-only on the 4 CSVs. 0 write.

## Conclusion

No translation work warranted pre-tag — the i18n surface is confirmed clean at `27442add`. Per the "no fabrication" discipline: reporting the 0-finding state rather than generating work. **QA verdict on rendered output remains ai-01's lane** (verdict #140 content + #632 CMYK); this doc is the data-side pendant.

Relates #647 (scanner), #640 (Rules refonte), #653 (Johnny fix), #134 (release), #140 (QA), #29u572 (dispatch). Base `27442add`.
