# Investigations & Debug Archive — Index of record (tranche 1/2, measured 2026-11-24)

> **Entry point for `docs/investigations`.**
> The folder holds **146 tracked files** (`git ls-files docs/investigations`, measured 2026-11-24): **83 at the
> root**, **63 in nine subfolders**. No README existed before this one; it is created as an index of record,
> by tranches. Tranche 1 (this file) indexes the **49 root files of the 2025-10 → 2026-06 era** and the
> **nine subfolders** — 58 rows. Tranche 2 will index the **34 remaining root files (2026-07 → 2026-11)**.
> ⚠️ The `<!-- index-of-record -->` mark is posed at the **last** tranche: this file does not carry it yet,
> and `tools/test_docs-index-contract.py` holds only a marked README to completeness.

**Status vocabulary** (`docs/agents/consolidation.md`): `current` = says today's state, or governs a
gesture still to come — re-read against master on 2026-11-24; `superseded` = a later document replaces it
(the successor is named on the same line); `historical` = dated record, not governing. Doubt between
current and historical resolves to **historical**. For data files (CSV/JSON/TXT), the date column is the
last commit that touched them.

---

## §1 — October 2025: the PDF pipeline recovery

The founding block: what broke, what was corrected, what failed again. Read it as a sequence — several
reports contradict each other because they were written hours apart, and the later one is the one that
measured.

| File | Date | Status | What it is |
|---|---|---|---|
| [2025-10-15-investigation-historique-pipeline-pdf.md](2025-10-15-investigation-historique-pipeline-pdf.md) | 2025-10-15 | `historical` | Complete history of the PDF pipeline (April–October 2025): where the regressions came from. |
| [2025-10-16-analyse-etape1-validation.md](2025-10-16-analyse-etape1-validation.md) | 2025-10-16 | `historical` | Step 1 analysis: validation of the `generateImages()` correction. |
| [2025-10-16-correction-architecture-cardpen.md](2025-10-16-correction-architecture-cardpen.md) | 2025-10-16 | `historical` | CardPen routing architecture correction. |
| [2025-10-16-correction-cardpen-playwright.md](2025-10-16-correction-cardpen-playwright.md) | 2025-10-16 | `historical` | CardPen–Playwright interface fix: automatic `generateImages()` call. Correction applied, awaiting validation. |
| [2025-10-16-correction-chargement-csv.md](2025-10-16-correction-chargement-csv.md) | 2025-10-16 | `historical` | Rules card CSV loading fix. Applied, awaiting user test. |
| [2025-10-16-echec-correction-bug-csv.md](2025-10-16-echec-correction-bug-csv.md) | 2025-10-16 | `historical` | Failed CSV fix report: the bug persisted despite the C# correction. |
| [2025-10-16-rapport-corrections-regressions-pipeline.md](2025-10-16-rapport-corrections-regressions-pipeline.md) | 2025-10-16 | `historical` | Corrections to the PDF pipeline regressions. Applied, awaiting validation. |
| [2025-10-16-rapport-echec-generation-complete.md](2025-10-16-rapport-echec-generation-complete.md) | 2025-10-16 | `historical` | Complete generation failure: no PDF produced. |
| [2025-10-16-rapport-execution-pipeline-pdf.md](2025-10-16-rapport-execution-pipeline-pdf.md) | 2025-10-16 | `historical` | Execution report of the PDF pipeline, 16 October 2025: critical failure. |
| [2025-10-16-rapport-final-corrections-pipeline.md](2025-10-16-rapport-final-corrections-pipeline.md) | 2025-10-16 | `historical` | Final report of the five corrections, ready for user test. |
| [2025-10-16-rapport-regression-cardpen.md](2025-10-16-rapport-regression-cardpen.md) | 2025-10-16 | `historical` | CardPen pipeline regression investigation. |
| [2025-10-16-rapport-validation-finale-pipeline.md](2025-10-16-rapport-validation-finale-pipeline.md) | 2025-10-16 | `historical` | Final validation report after corrections: failure on a critical configuration problem. |
| [2025-10-17-pdfs-generes-final.csv](2025-10-17-pdfs-generes-final.csv) | 2025-10-17 | `historical` | Inventory of the PDFs generated at that date. |
| [2025-10-18-analyse-commits-profonde.json](2025-10-18-analyse-commits-profonde.json) | 2025-10-18 | `historical` | Deep commit analysis, one record per commit with recoverability verdicts. |
| [2025-10-18-diagnostic-echecs.csv](2025-10-18-diagnostic-echecs.csv) | 2025-10-18 | `historical` | Failure diagnostic table. |
| [2025-10-18-rapport-historique-png.txt](2025-10-18-rapport-historique-png.txt) | 2025-10-18 | `historical` | Raw PNG history report. |
| [2025-10-21-RÉSUMÉ-EXÉCUTIF-ARCHÉOLOGIE.md](2025-10-21-RÉSUMÉ-EXÉCUTIF-ARCHÉOLOGIE.md) | 2025-10-21 | `historical` | Executive summary of the git archaeology on the DocumentConfigurations regression: investigation complete. |
| [2025-10-21-investigation-regression-cardsets.md](2025-10-21-investigation-regression-cardsets.md) | 2025-10-21 | `historical` | Investigation of the PDF pipeline CardSet configuration regression. |
| [2025-10-21-rapport-archeologie-git-final.md](2025-10-21-rapport-archeologie-git-final.md) | 2025-10-21 | `historical` | Final git archaeology report; major conclusion: **no git regression** — the C# code was intact. |
| [2025-10-21-rapport-rebuild-et-validation.md](2025-10-21-rapport-rebuild-et-validation.md) | 2025-10-21 | `historical` | Full rebuild and validation of the auto-generated JSON. |
| [2025-10-22-rapport-final-generation-pdfs-avec-scenarii.md](2025-10-22-rapport-final-generation-pdfs-avec-scenarii.md) | 2025-10-22 | `historical` | Final PDF generation with Scenarii validation: partially successful, incomplete-Scenarii bug identified. |
| [2025-10-23-investigation-regression-complete-configuration-pdfs.md](2025-10-23-investigation-regression-complete-configuration-pdfs.md) | 2025-10-23 | `historical` | Complete investigation of the PDF configuration regression. |
| [2025-10-23-validation-memo-pdfs.csv](2025-10-23-validation-memo-pdfs.csv) | 2025-10-23 | `historical` | Memo CardSet PDF validation data. |
| [2025-10-23-validation-memo-restaure.md](2025-10-23-validation-memo-restaure.md) | 2025-10-23 | `historical` | Validation of the restored Memo CardSet: success. |

---

## §2 — December 2025: git stabilisation and mission closure

| File | Date | Status | What it is |
|---|---|---|---|
| [2025-12-11-analyse-etat-git.md](2025-12-11-analyse-etat-git.md) | 2025-12-11 | `historical` | Phase 1 git-state analysis. |
| [2025-12-11-audit-etat-git.md](2025-12-11-audit-etat-git.md) | 2025-12-11 | `historical` | Git-state audit; verdict: do **not** `git reset --hard`. |
| [2025-12-11-phase2-critical-differences-analysis.md](2025-12-11-phase2-critical-differences-analysis.md) | 2025-12-11 | `historical` | Phase 2: critical differences analysis. |
| [2025-12-11-rapport-generation-complete.md](2025-12-11-rapport-generation-complete.md) | 2025-12-11 | `historical` | Final PDF generation report: technically successful, scope incomplete. |
| [2025-12-11-rapport-stabilisation-git.md](2025-12-11-rapport-stabilisation-git.md) | 2025-12-11 | `historical` | Git stabilisation report: validated. |
| [2025-12-11-rapport-validation-finale.md](2025-12-11-rapport-validation-finale.md) | 2025-12-11 | `historical` | Final validation report of the October 2025 recovery release. |
| [2025-12-13-rapport-cloture-mission.md](2025-12-13-rapport-cloture-mission.md) | 2025-12-13 | `historical` | Mission closure: Poker & Tarot asset regeneration, success. |
| [2025-12-14-rapport-archeologie-config-avril-2024.md](2025-12-14-rapport-archeologie-config-avril-2024.md) | 2025-12-14 | `historical` | Git archaeology restoring the April 2024 Golden Master configuration (April 2025 snapshot). |

---

## §3 — March / May 2026

| File | Date | Status | What it is |
|---|---|---|---|
| [2026-03-24-plan-po2023-svg-rules.md](2026-03-24-plan-po2023-svg-rules.md) | 2026-03-24 | `historical` | Plan: SVG export + Rules re-harvest under the po-2023 lane. |
| [2026-05-01-formulas-per-spreadsheet.md](2026-05-01-formulas-per-spreadsheet.md) | 2026-05-01 | `historical` | Formula audit across all GSheet spreadsheets. |

---

## §4 — June 2026: scopings and audits

| File | Date | Status | What it is |
|---|---|---|---|
| [2026-06-14-140-qa-verification-matrix.md](2026-06-14-140-qa-verification-matrix.md) | 2026-06-14 | `historical` | Multi-axis multilingual QA verification matrix (#140). |
| [2026-06-14-188-rules-cover-extension-scoping.md](2026-06-14-188-rules-cover-extension-scoping.md) | 2026-06-14 | `historical` | #188 Rules cover extension (game variants): scoping. Phase 1 resolved (PR #194), Phase 2 key-gated; #188 is **closed completed**. |
| [2026-06-14-189-print-play-complete-scoping.md](2026-06-14-189-print-play-complete-scoping.md) | 2026-06-14 | `historical` | #189 Print & Play "complete" alternative: scoping. #189 is **closed completed**; the full/light pair ships as the download labels (`download-cards-canon.csv`, DNN side). |
| [2026-06-14-29-memory-perf-hotspots.md](2026-06-14-29-memory-perf-hotspots.md) | 2026-06-14 | `historical` | Memory / performance hotspot audit (#29). |
| [2026-06-14-415-git-repo-slimming-scoping.md](2026-06-14-415-git-repo-slimming-scoping.md) | 2026-06-14 | `current` | #415 `.git` slimming: scoping, execution gated (3-person coordination). **Not executed** — measured 2026-11-24: `git count-objects -vH` size-pack **6.22 GiB** (the doc quotes 2.2 GB); #415 is **open**. |
| [2026-06-14-llm-model-quality-datasetupdater.md](2026-06-14-llm-model-quality-datasetupdater.md) | 2026-06-14 | `historical` | LLM model quality for the DatasetUpdater translations (#299). #299 is closed completed. |
| [2026-06-21-mindmap-reliability-without-rdp.md](2026-06-21-mindmap-reliability-without-rdp.md) | 2026-06-21 | `historical` | Making MindMap SVG generation reliable without an active RDP session: OS-level fix delivered; native Freeplane path traced in #568. |
| [2026-06-23-prod-csv-hygiene-audit.md](2026-06-23-prod-csv-hygiene-audit.md) | 2026-06-23 | `historical` | Production CSV / Memo hygiene audit. |
| [2026-06-25-virtues-mindmap-fr-frozen-mechanism.md](2026-06-25-virtues-mindmap-fr-frozen-mechanism.md) | 2026-06-25 | `historical` | Virtues mindmap `.content.svg` FR-frozen: mechanism traced, root cause and fix path. |
| [2026-06-29-issues-28-29-scoping.md](2026-06-29-issues-28-29-scoping.md) | 2026-06-29 | `historical` | Read-only scoping of issues #28/#29: closures justified, fixes verified in code. |

---

## §5 — Undated artifacts (date = last commit)

| File | Date | Status | What it is |
|---|---|---|---|
| [411-pt-register-worklist.md](411-pt-register-worklist.md) | 2026-06-05 | `historical` | #411 PT register worklist: 80 archaic-register records, 88 cells to rework. #411 is **closed completed**. |
| [411-pt-register-sample.py](411-pt-register-sample.py) | 2026-06-05 | `historical` | Sampling script for the #411 worklist. |
| [411-pt-sample-results.json](411-pt-sample-results.json) | 2026-06-05 | `historical` | Sampling results for #411. |
| [virtues_minority_verdicts.json](virtues_minority_verdicts.json) | 2026-06-05 | `historical` | Minority-verdict data captured during the June audits. |
| [git_status_report.txt](git_status_report.txt) | 2025-12-14 | `historical` | Raw git status capture from the December stabilisation. |

---

## §6 — Subfolders

| Folder | Files | Status | What it is |
|---|---:|---|---|
| [2026-06-04-memo-435-i18n-gap/](2026-06-04-memo-435-i18n-gap/README.md) | 10 | `historical` | #435 Memo Tarot i18n regression: symptom by render, diagnostic, fix. #435 is **closed completed**. |
| [2026-06-04-rules-250-validation/](2026-06-04-rules-250-validation/README.md) | 7 | `historical` | Visual validation of the Rules editorial redesign (#250, PR #438): 15-card layout, CSS indexed by card number. #250 is **closed completed**. |
| [2026-09-30-1485-substitution-polices/](2026-09-30-1485-substitution-polices/README.md) | 8 | `current` | Font substitution table (DINPro → permissive, + TrendSlab) with comparative renders on real faces; feeds the **open** #1485 licence decision (item 3.a, owner). Changes no template. |
| [2026-10-22-gcc-scenarii-footer-dollar/](2026-10-22-gcc-scenarii-footer-dollar/README.md) | 5 | `historical` | Grain #458 G-CC: removal of the `$` pictogram from the Scenarii card footer, with its escape. |
| [2026-10-22-news5-mobile-overflow/](2026-10-22-news5-mobile-overflow/README.md) | 3 | `historical` | Mobile horizontal overflow of the News5 overlay (grain #458 G-CC-bis): `scrollWidth` 539 for `clientWidth` 375 at 390 px, before deferred vignettes load. |
| [archeologie-git/](archeologie-git/) | 21 | `historical` | Git archaeology: reference copies of the functional config plus ~15 historical versions for comparison. |
| [diffs-git/](diffs-git/) | 3 | `historical` | Six-month diffs of `AssetConverterConfig`, `CardSetDocument` and `WebBasedGeneratorConfig`. |
| [pdf-validation/](pdf-validation/) | 3 | `historical` | Node harness that validated the generated PDFs (`package.json` + `validate-pdfs.js`). |
| [scripts/](scripts/) | 3 | `historical` | One-off diagnostic scripts: git-status analysis, translation scan, mindmap interactive session. |

---

## §7 — How to read this folder

These are **dated measures**, not living references. A number quoted here is the number of its date: the
history is full of superseded counts (PDF totals, image counts, family names) that were true when written.
Before quoting one in an issue or a devis, re-measure it against the tree — and prefer the latest report of
the same subject, not the most detailed one. When two reports disagree, the later one measured; the earlier
one recorded an intention.

---

## §8 — Remaining tranche

Tranche 2 will index the **34 root files dated 2026-07 → 2026-11** (the 2026-07 block alone is 28 files:
regen-readiness, mindmap instruments, DNN exports, coverage audits). It will be branched from master after
this PR merges — never stacked. The `<!-- index-of-record -->` mark is posed on **that** PR, together with
the DoD mutant ("a row of a file cited only once removed → contract red and named"). Until then this file is
a navigation index that carries statuses, not a contract.