# Investigations & Debug Archive — Index of record (tranches 1–2, measured 2026-11-24)

<!-- index-of-record -->

> **Entry point for `docs/investigations`.**
> The folder holds **178 tracked files** (`git ls-files docs/investigations`, re-measured 2026-11-26 on this
> branch: `5e02dfae` plus the nine files of PR #2388 and the twenty-two of PR #2394): **83 root files**, **this README**, **94 files in eleven subfolders**. It is the index of record,
> written in two tranches. Tranche 1 (PR #2364) indexed the **49 root files of the 2025-10 → 2026-06 era** and
> the **nine subfolders**; tranche 2 indexes the **34 remaining root files (2026-07 → 2026-11)**; later
> additions index the `2026-11-25-1369-sortie-de-repli/` and `2026-11-26-1485-polices-avant-apres/` subfolders (§11). Coverage:
> **83/83 root files**, **11/11 subfolders**, the subfolders by folder link.
> The `<!-- index-of-record -->` mark is posed since tranche 2: `tools/test_docs-index-contract.py` holds this
> file to completeness, one status per row, and no dead link (see §13).

**Status vocabulary** (`docs/agents/consolidation.md`): `current` = says today's state, or governs a
gesture still to come — re-read against master on 2026-11-24; `superseded` = a later document replaces it
(the successor is named on the same line); `historical` = dated record, not governing. Doubt between
current and historical resolves to **historical**. For data files (CSV/JSON/TXT), the date column is the
last commit that touched them. A row whose file carries a defect that a later file corrects says so
in its last column: the index changes no finding (`docs/agents/consolidation.md`, step 6).

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
| [2026-06-14-415-git-repo-slimming-scoping.md](2026-06-14-415-git-repo-slimming-scoping.md) | 2026-06-14 | `current` | #415 `.git` slimming: scoping, execution gated (3-person coordination). **Not executed** — the proof is #415 being **open**, not the pack size: `size-pack` is a property of the clone that measures it (`git count-objects -vH`, 2026-11-24: **6.22 GiB** on po-2023; **2.30 GiB** on ai-01, as quoted in #458). The doc's 2.2 GB is its figure of 2026-06-14. |
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

## §6 — July 2026 · Rules, translations, datasets

**Where it stands on 2026-11-24.** Every subject of this section is closed: #642, #654 and #804 are closed
completed. Each figure is the figure of that file's base commit. The translation coverage they measure was
re-measured afterwards — 2026-09-13 for `es`/`ar`/`fa`/`zh`
([`../translation/i18n-remesure-es-ar-fa-zh.md`](../translation/i18n-remesure-es-ar-fa-zh.md)) — and the
printed deck's `link` column was settled separately (#1435 is closed; project `CLAUDE.md`). No file here
governs a gesture still to come: the canon is the CSV under `Cards/`.

| File | Date | Status | What it is |
|---|---|---|---|
| [2026-07-02-gdrive-rules-642-structural-analysis.md](2026-07-02-gdrive-rules-642-structural-analysis.md) | 2026-07-02 | `historical` | #642, GDrive Rules ↔ repo master: structural analysis and machine-translation garbage inventory (base `99145fab`). Concludes that a row-level paste-ready CSV is structurally impossible, and delivers the decision analysis instead. #642 is **closed completed**. |
| [2026-07-02-scanner-fp-arbitration.md](2026-07-02-scanner-fp-arbitration.md) | 2026-07-02 | `historical` | Arbitration of the 7 residual findings of the i18n scanner after the #640 Rules rework: all 7 are legitimate — French itself keeps several of the terms — and are kept as they are. |
| [2026-07-03-gdrive-rules-642-migration-table.md](2026-07-03-gdrive-rules-642-migration-table.md) | 2026-07-03 | `historical` | Audit trail of the #642 GDrive migration: the legacy 24-row `Cards` tab archived under a reversible name, replaced by `Rules main` (16×10) and `Rules PP` (7×10) populated from repo master. |
| [2026-07-03-polish-trad-sweep-v090-confirm.md](2026-07-03-polish-trad-sweep-v090-confirm.md) | 2026-07-03 | `historical` | Final pre-release translation sweep of the 4 datasets (scanner #647): 0 drift, 0 machine-translation contamination. True of master `27442add`. |
| [2026-07-03-rules-i18n-coverage-audit.md](2026-07-03-rules-i18n-coverage-audit.md) | 2026-07-03 | `historical` | Cell-by-cell coverage audit of the Rules CSV (15 records × 8 languages): 120/120 populated, expected script, 0 residual contamination — the data pendant of the visual verdict #140. |
| [2026-07-03-654-virtues-mnemonics-inventory.md](2026-07-03-654-virtues-mnemonics-inventory.md) | 2026-07-03 | `historical` | #654: verified inventory of the syllogistic mnemonics in the Virtues taxonomy — 20 rows, 19 classical mnemonics — replacing the estimated range the issue carried. #654 is **closed completed**. |
| [2026-07-03-654-mnemonics-celltable.md](2026-07-03-654-mnemonics-celltable.md) | 2026-07-03 | `historical` | #654: the apply-ready cell table per language (pks 106–127, the 4 non-Latin languages), refining the presence matrix of the #660 inventory. Analysis only, no CSV write. |
| [2026-07-05-654-mnemonics-dryrun.md](2026-07-05-654-mnemonics-dryrun.md) | 2026-07-05 | `historical` | #654 mnemonics → Latin: dry-run of the title-only conversion (52 cells) after the keep-Latin decision of 2026-07-04. Dry-run only — the apply stayed gated before the tag. |
| [2026-07-05-654-remark-dryrun.md](2026-07-05-654-remark-dryrun.md) | 2026-07-05 | `historical` | #654, the same tool extended to `remark_*` (PR #695): remark is not title-shaped — 0 of 14 cells auto-convertible, all 14 flagged ambiguous. |
| [2026-07-16-bytecheck-datasets-non-couverts.md](2026-07-16-bytecheck-datasets-non-couverts.md) | 2026-07-16 | `historical` | Encoding inventory across the production Cards CSVs, with the correction of its own first pass (end-of-line read on the first 64 bytes only) and the extension to the two taxonomies. |
| [2026-07-16-link-i18n-prep-sample-30.md](2026-07-16-link-i18n-prep-sample-30.md) | 2026-07-16 | `historical` | #804 preparation: 30 `fr.wikipedia` links probed through the MediaWiki langlinks API for a localized article. #804 is **closed completed**. |

---

## §7 — July 2026 · DNN: coverage, mechanism, schema

**Where it stands on 2026-11-24.** Four read-only readings of the site and of the export of July 2026. They
predate the generation that went live on 2026-11-24: today's state is §0 of
[`../dnn-localization/README.md`](../dnn-localization/README.md), and the procedure that took the site there
is [`../quality/1066-cutover-runbook-2026-11-04.md`](../quality/1066-cutover-runbook-2026-11-04.md). #681 is
closed completed; #684 is closed **not planned**. No file here governs a gesture still to come.

| File | Date | Status | What it is |
|---|---|---|---|
| [2026-07-03-dnn-prod-rules-coverage.md](2026-07-03-dnn-prod-rules-coverage.md) | 2026-07-03 | `historical` | Read-only audit of the production DNN site: the 4 variants and the main game were on prod in French, and the 7 translated languages were not published on the site. |
| [2026-07-03-dnn-i18n-porting-mechanism.md](2026-07-03-dnn-i18n-porting-mechanism.md) | 2026-07-03 | `historical` | How DNN/2sxc localisation worked on the 2026-07 export, with an effort estimate per language and a step plan. Read-only. |
| [2026-07-04-dnn-2sxc-rule-schema-export.md](2026-07-04-dnn-2sxc-rule-schema-export.md) | 2026-07-04 | `historical` | Read-only export of the 2sxc Rule content-type schema from the production backup: generic, language-neutral fields, zero language-suffixed ones — the unblocker #681 asked for. #681 is **closed completed**. |
| [2026-07-04-rules-coverage-csv-vs-dnn.md](2026-07-04-rules-coverage-csv-vs-dnn.md) | 2026-07-04 | `historical` | Rule prose present and missing per language, repo CSV against the DNN site, to prepare the rule-prose translation step. That step's issue, #684, is **closed, not planned**. |

---

## §8 — July 2026 · Regen readiness and mind maps

**Where it stands on 2026-11-24.** The fresh 8-language regen that the two readiness documents prepared has
been run since; the working recipe lives in the project `CLAUDE.md` (*Regeneration runs*). The mind-map
subjects are closed completed, and their findings are held by tests —
`MindmapWrapperGoldenMasterTests`, `MindmapWrapperCapabilitiesTests`, `MindmapWrapperFreshnessGateTests` —
except one: the stale test skip analysed on 2026-07-06 is **still in the tree** (its row carries the measured
line). No file here is the reference to execute from; the tests are.

| File | Date | Status | What it is |
|---|---|---|---|
| [2026-07-04-regen-readiness-checklist-8lang.md](2026-07-04-regen-readiness-checklist-8lang.md) | 2026-07-04 | `historical` | Regen-readiness checklist for the fresh 8-language regen required before the v0.9.0 tag — **PREP only**, anchored on `21e2c666`. Its readiness figures are re-anchored by the 07-11 refresh, and the regens it prepared have been run since. |
| [2026-07-04-retry-serial-smoke-test.md](2026-07-04-retry-serial-smoke-test.md) | 2026-07-04 | `historical` | Static analysis of the retry-serial path (#613 option C), the rationale for holding its runtime smoke-test before the tag, and the post-tag validation plan. A metric signal, not a verdict. |
| [2026-07-06-fallacies-enru-svg-freshness-verify.md](2026-07-06-fallacies-enru-svg-freshness-verify.md) | 2026-07-06 | `historical` | Verification that gap #1 of #636 was already closed: the Fallacies EN/RU SVGs were current at `d4484679` (PR #664). #636 is **closed completed**. |
| [2026-07-06-freeplane-headless-export-spike.md](2026-07-06-freeplane-headless-export-spike.md) | 2026-07-06 | `historical` | Feasibility spike of a headless Freeplane `c.export()` SVG path: blocked at production scale, negative adoption verdict. The goal of #568 — no RDP-foreground dependency — was already met by the OS-level fix. #568 is **closed completed**. |
| [2026-07-06-magick-skip-investigation.md](2026-07-06-magick-skip-investigation.md) | 2026-07-06 | `historical` | Root cause of the deferred test skip flagged by #719 (test-skips inventory): the 2025-08-01 reason is stale — Magick.NET left the mind-map SVG path with the Batik migration (PR #565). ⚠️ Its follow-ups are **not applied** on master at 2026-11-24: the skip and its reason are still at `Generation/Converters/Argumentum.AssetConverter.Tests/MindmapGeneration/SvgConversionIntegrationTests.cs:40`. |
| [2026-07-07-issue-725-virtues-wrapper-stale-not-code-bug.md](2026-07-07-issue-725-virtues-wrapper-stale-not-code-bug.md) | 2026-07-07 | `historical` | #725: the FR-frozen Virtues HTML wrappers were a **stale file, not a code bug** — the wrapper had not been regenerated after the i18n wiring. #725 is **closed completed**. |
| [2026-07-11-regen-readiness-refresh-c1ed77d2.md](2026-07-11-regen-readiness-refresh-c1ed77d2.md) | 2026-07-11 | `historical` | Refresh of that checklist against `c1ed77d2` — a delta over the 07-04 harness — with a micro-refresh of 2026-07-13 (`b736a808`). Read the two together. |
| [2026-07-16-mindmap-html-wrapper-encoding.md](2026-07-16-mindmap-html-wrapper-encoding.md) | 2026-07-16 | `historical` | Read-only encoding check of the 16 integrated HTML mindmap wrappers that PR #819 regenerated — the vehicle the reviewers open. |

---

## §9 — July 2026 · Ontology, CardPen, release body

**Where it stands on 2026-11-24.** #499, #629 and #812 are closed completed. The audit of 2026-07-28 reads
the body of the v0.9.0 release-validation dossier, which has since been replaced: the release index of record
is [`../release-dossier/README.md`](../release-dossier/README.md), and #134 is still **open** — the tag is not
posed.

| File | Date | Status | What it is |
|---|---|---|---|
| [2026-07-03-629-cardpen-pages-cards-404-diagnostic.md](2026-07-03-629-cardpen-pages-cards-404-diagnostic.md) | 2026-07-03 | `historical` | #629 `/Cards/` 404 on GitHub Pages: diagnosis and option analysis — the root cause is wider than the issue body said (relative `/Cards` paths in three card types). #629 is **closed completed**; the workaround in force is `UseLocalCardpen = true` (project `CLAUDE.md`). |
| [2026-07-04-499-virtues-owl-phase2-confirm.md](2026-07-04-499-virtues-owl-phase2-confirm.md) | 2026-07-04 | `historical` | #499: Virtues OWL phase 2 confirmed done, freshness-verified against the CSV (base `a41cbda6`). #499 is **closed completed**. |
| [2026-07-16-audit-template-stale-csv.md](2026-07-16-audit-template-stale-csv.md) | 2026-07-16 | `historical` | #812: the `csv` embedded in the 27 CardPen templates × 15 CardSets is stale against the source CSV — but dead for Face cards, since the harvester injects the source at run time. #812 is **closed completed**. |
| [2026-07-19-aif-attack-graph-export.md](2026-07-19-aif-attack-graph-export.md) | 2026-07-19 | `historical` | Read-only export of the typed AIF attack graph (undercut / undermine / rebut) from the CSV and the OWL, with its empirical findings and verdict. No new modelling. |
| [2026-07-28-release-validation-v0.9.0-audit.md](2026-07-28-release-validation-v0.9.0-audit.md) | 2026-07-28 | `historical` | Audit of the body of the v0.9.0 release-validation dossier (§1–§8), each figure re-measured on `3094d42c` for the #134 gate. The dossier it audits has since been replaced: the release index of record is [`../release-dossier/README.md`](../release-dossier/README.md). |

---

## §10 — August → November 2026

**Where it stands on 2026-11-24.** #830, #2025 and #2138 are closed completed; the XSLT grain and the FreeMind
pass are delivered. The two 2sxc reports measure the **preprod base of 30/10**, before the go-live of
2026-11-24 — read them as the record of that date, with §0 of
[`../dnn-localization/README.md`](../dnn-localization/README.md) for today's state.

| File | Date | Status | What it is |
|---|---|---|---|
| [2026-08-23-mindmap-golden-master-instrument.md](2026-08-23-mindmap-golden-master-instrument.md) | 2026-08-23 | `historical` | #830: the design of the mindmap verdict instrument — a behavioural barème for the HTML wrappers, to be re-checked at every regen. Delivered as `MindmapWrapperCapabilitiesTests`; #830 is **closed completed**. |
| [2026-10-04-xslt-fallback-visible.md](2026-10-04-xslt-fallback-visible.md) | 2026-10-04 | `historical` | Pool #458 grain 4: the XSLT fallback made visible — when the path is taken, what the log says, the warning counted in the end-of-pass summary. Delivered with `MindmapXsltFallbackSignalTests`. |
| [2026-10-30-passe-freemind-mindmaps-2025.md](2026-10-30-passe-freemind-mindmaps-2025.md) | 2026-10-30 | `historical` | Measure of the #2025 FreeMind pass (one language per run, in the build tree of a detached worktree): what the re-derivation really changes, and the explicit FR-only transfer to the committed tree. #2025 is **closed completed**. |
| [2026-10-30-sxc-data-surface-measured.md](2026-10-30-sxc-data-surface-measured.md) | 2026-10-30 | `historical` | Measured 2sxc data surface on the preprod site (read-only, PR #2073). ⚠️ Its announcement that the Rules content was already in 2sxc in 8 languages is **inexact** — a confusion of granularity (game-level `Game Rule` ×5, not the CSV-line `Rule` type); the correction is in [2026-10-30-sxc-rules-import-state.md](2026-10-30-sxc-rules-import-state.md). |
| [2026-10-30-sxc-rules-import-state.md](2026-10-30-sxc-rules-import-state.md) | 2026-10-30 | `historical` | State of the Rules import into 2sxc and of its gate (pool #458, item ②): the payload is ready and its DoD already automated; the row-level `Rule` content was not in the base on 30/10. Completes and corrects the data-surface report of the same day. |
| [2026-11-05-2138-instrument-diagnostic-timeout.md](2026-11-05-2138-instrument-diagnostic-timeout.md) | 2026-11-05 | `historical` | #2138, condition 1: an instrument that says, at a `git cat-file` timeout, whether the exchange had started and where it stood. #2138 is **closed completed**. |

---

## §11 — Subfolders

| Folder | Files | Status | What it is |
|---|---:|---|---|
| [2026-06-04-memo-435-i18n-gap/](2026-06-04-memo-435-i18n-gap/) | 10 | `historical` | #435 Memo Tarot i18n regression: symptom by render, diagnostic, fix. #435 is **closed completed**. |
| [2026-06-04-rules-250-validation/](2026-06-04-rules-250-validation/) | 7 | `historical` | Visual validation of the Rules editorial redesign (#250, PR #438): 15-card layout, CSS indexed by card number. #250 is **closed completed**. |
| [2026-09-30-1485-substitution-polices/](2026-09-30-1485-substitution-polices/) | 8 | `current` | Font substitution table (DINPro → permissive, + TrendSlab) with comparative renders on real faces; feeds the **open** #1485 licence decision (item 3.a, owner). Changes no template. |
| [2026-10-22-gcc-scenarii-footer-dollar/](2026-10-22-gcc-scenarii-footer-dollar/) | 5 | `historical` | Grain #458 G-CC: removal of the `$` pictogram from the Scenarii card footer, with its escape. |
| [2026-10-22-news5-mobile-overflow/](2026-10-22-news5-mobile-overflow/) | 3 | `historical` | Mobile horizontal overflow of the News5 overlay (grain #458 G-CC-bis): `scrollWidth` 539 for `clientWidth` 375 at 390 px, before deferred vignettes load. |
| [2026-11-25-1369-sortie-de-repli/](2026-11-25-1369-sortie-de-repli/) | 9 | `current` | Grain #1369 (P23-08): the Rules_07 pilot illustration removed, with its A/B pixel comparison; the visual verdict is ai-01's and #2132 stays open. |
| [2026-11-26-1485-polices-avant-apres/](2026-11-26-1485-polices-avant-apres/) | 22 | `current` | Grain #1485 (P23-09): the eleven card fonts embedded in the harvest payload and the ten card templates moved from DINPro to Barlow/Rubik, with before/after renders of eleven faces in `fr` and `ru` and the injection artefact isolated on the Memo back; the visual verdict is ai-01's and the merge waits for #2132. Changes no data. |
| [archeologie-git/](archeologie-git/) | 21 | `historical` | Git archaeology: reference copies of the functional config plus ~15 historical versions for comparison. |
| [diffs-git/](diffs-git/) | 3 | `historical` | Six-month diffs of `AssetConverterConfig`, `CardSetDocument` and `WebBasedGeneratorConfig`. |
| [pdf-validation/](pdf-validation/) | 3 | `historical` | Node harness that validated the generated PDFs (`package.json` + `validate-pdfs.js`). |
| [scripts/](scripts/) | 3 | `historical` | One-off diagnostic scripts: git-status analysis, translation scan, mindmap interactive session. |

The eleven rows link the **folder**: a link to a subfolder's `README.md` covers only that README, and the other files would read as uncited (§13).

---

## §12 — How to read this folder

These are **dated measures**, not living references. A number quoted here is the number of its date: the
history is full of superseded counts (PDF totals, image counts, family names) that were true when written.
Before quoting one in an issue or a devis, re-measure it against the tree — and prefer the latest report of
the same subject, not the most detailed one. When two reports disagree, the later one measured; the earlier
one recorded an intention.

---

## §13 — The contract, from this tranche on

This README carries the `<!-- index-of-record -->` mark since tranche 2. `python tools/test_docs-index-contract.py`
holds it to three things: every tracked file of `docs/investigations` is cited, directly or through a folder
link; every row that links a file of this folder carries **exactly one** status in backticks; no link is dead,
case included. A file added under this folder without its row turns the pull request red, naming the file.

A subfolder row links the **folder** (`name/`), never its `README.md`: a link to a subfolder's README covers
only that README, and the other files of the subfolder would read as uncited. That is why the eleven rows of §11
use `name/`.

The mark was posed with the proof the DoD asks for: one row of a file cited only once was removed from a copy
of this file, the contract went red naming that file, and the row was restored (tranche-2 pull request). The
count this file accounts for moves when a file is added — `git ls-files docs/investigations | wc -l` is the
instrument, and the figure above is dated.

*po-2023 — pool #458, P23-35, tranches 1–2 · 24/11/2026.*
