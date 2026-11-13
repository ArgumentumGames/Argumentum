# docs/translation — translating the corpus: native links, fidelity reviews, the translation model

<!-- index-of-record -->

This folder is an **index of record**, as defined in [`docs/agents/consolidation.md`](../agents/consolidation.md): every tracked file in it is listed below with what it is worth today. Add a file here and you add its line — `python tools/test_docs-index-contract.py` goes red otherwise. A file marked current was re-read against `master` on the date below; a superseded one names its successor on its line; a historical one was true at its date and is neither executed nor "corrected" now.

## Where the subjects stand

Measured 2026-11-13 on `47d70f15`; issue states read the same day in one bulk call. Re-measure before quoting a count; the issue says what is open.

- **Native links on the Fallacies deck (#1471, open).** The deck is 175 cards; 165 carry at least one link, and the 10 without any are exactly the justified ones of #1435 (closed). Native `link_*` cells, measured: fr 125 · en 163 · ru 85 · pt 91 · es 100 · ar 110 · fa 88 · zh 120 — 438 cells still to produce outside the justified ten. These are the figures of the issue's [closure dossier of 12/11](https://github.com/ArgumentumGames/Argumentum/issues/1471#issuecomment-6836442791), and that dossier is what governs the next lots: no file of this folder does any more. The `1471-*` and `1497-*` files (#1497 closed 2026-10-19) are the dated record of how the classes and the write instruments were established; the instruments themselves are under `tools/1471-*` and `tools/1497-*`.
- **Fidelity of the translations (#458 roadmap, open; October campaign).** Scenarii (seven languages plus the printed English axis), Rules (the two sources, then the six target languages) and Virtues (seven languages) were each read with zero writes, then corrected by their own pull requests: Scenarii #1760, #1763, #1769, #1774, #1776, #1784, #1788, #1790, #1792; Rules #1764, #1768, #1778, #1929, #1941; Virtues #1795, #1798, #1806, #1807, #1808, #1814, #1834. The roadmap lists no fidelity grain today. Two residues stay open, both pinned: the Persian term for the trump is still split between two Rules rows (measured; held as an observation by `RulesTerminologyUnityGuardTests`, see the Rules table); and the Virtues OWL still carries the identifier of a renamed English label (measured: `acceptableInformalLogic` present, `solidInformalLogic` absent; held by `IdentityLag` in `VirtueOwlTaxonomyDivergenceCensusTests`), governed by [`docs/ontology/133-owl-virtues-peremption-mesuree-2026-10-09.md`](../ontology/133-owl-virtues-peremption-mesuree-2026-10-09.md) and #133. The Virtues mind maps no longer lag (measured: the new English label is in the three English Virtues mind-map files, the old one nowhere under `Cards/`).
- **Rules_06 owner lot (#1502, open).** The lot was written in eight languages by #1862, the Persian digits by #1863; the issue now waits on the site going live (REPORTED from the issue). `pivot-rules06-phrases.csv` is read by the generator config (DataSet `Rules06LotPhrases`) and by the DatasetUpdater task of the lot: keep its path.
- **Translation model and DatasetUpdater (#202, closed 2026-09-09).** Measured in `DatasetUpdaterRootConfig.cs`: 53 literals `"gpt-5.6-sol"`, none of `gpt-5.5` or `gpt-5.4`, no task with `Enabled = true`. The model migration is done (#1426, comments by #1431). What remains is the round-trip smoke run, gated on an owner GO and a named key: the smoke protocol governs it, and it is still item 9 of the next steps in [`docs/agents/status-and-history.md`](../agents/status-and-history.md).
- **English post-release campaign (#202, closed).** The plan and its QA gate were written in July and never executed. Measured: `Simple_name_en` is filled on 61 of 1408 nodes and `political_example_en` on 35, as when the plan was written, and both columns are still read by six Fallacies and Memo card templates. No open issue carries the campaign (searched; a search can miss) — see the statuses to counter-review.
- **Typography (#994, closed 2026-10-19), es/ar/fa/zh coverage, translation-defect detection trial (#1520, closed 2026-11-11).** Dated records; nothing to execute. No issue for reviewing the trial's candidate cells was found (searched; a search can miss).
- **Not in this folder.** The site's translation chain — language-pack residue (#457, open), articles and site body (#2182, open) — is followed in [`docs/dnn-localization/`](../dnn-localization/), for instance [`2182-plan-injection-147-cellules-2026-11-11.md`](../dnn-localization/2182-plan-injection-147-cellules-2026-11-11.md).

Several files here are cited by path from guard tests, from the generator config and from `docs/agents/status-and-history.md` (the smoke protocol, the es/ar/fa/zh re-measure, the Rules and Scenarii dossiers, the Virtues corrections, the Rules_06 pivot). Moving one breaks those references.

## The files

### Native links — #1471, #1497

| File | Dated | Status | What it is |
|---|---|---|---|
| [`1471-rendement-reel-langlinks-2026-09-21.md`](1471-rendement-reel-langlinks-2026-09-21.md) | 2026-09-21 | `historical` | Grain ①: the real yield of the Wikipedia `langlinks` API on the deck, and why the cost structure is inverted (#1475). Sized against the epic's opening count; the residual is on the issue now |
| [`1471-caracterisation-778-cellules.md`](1471-caracterisation-778-cellules.md) | 2026-09-21 | `superseded` | Classification of the empty `link_*` cells left after #1487 into classes A, B and E (the name says 778; the body measures 858 and explains the gap). Its class B and E sizes were replaced by [`1497-tiering-B-2026-09-25.md`](1497-tiering-B-2026-09-25.md) and by the two class-E audits below |
| [`1471-resondage-classe-E-2026-09-22.md`](1471-resondage-classe-E-2026-09-22.md) | 2026-09-22 | `historical` | Re-survey of class E with a distinct instrument: nearly every card has a hit by native title (#1492). A hit is not a relevant article — the relevance audit below overturned what was drawn from it |
| [`1471-audit-qualite-A-2026-09-22.md`](1471-audit-qualite-A-2026-09-22.md) | 2026-09-22 | `historical` | Quality audit of the 24 class-A cells: 22 on subject, 2 off subject (#1494) |
| [`1471-write-22A-2026-09-22.md`](1471-write-22A-2026-09-22.md) | 2026-09-22 | `historical` | Writing of the 22 on-subject class-A cells under the owner GO of 22/09 (#1508) |
| [`1471-audit-pertinence-E-2026-09-22.md`](1471-audit-pertinence-E-2026-09-22.md) | 2026-09-22 | `historical` | Relevance audit of the class-E hits, second version after a human cross-check: about one hit in ten is a relevant article (#1496). The owner decision of 22/09 that followed was executed by #1554 |
| [`1497-tiering-B-2026-09-25.md`](1497-tiering-B-2026-09-25.md) | 2026-09-25 | `historical` | Tiering of the class-B cells: the opening class-B count was an instrument artefact; part are interwiki A cells, the true B have no relevant hit (#1555). #1497 is closed |
| [`1497-W-rejouabilite-2026-09-25.md`](1497-W-rejouabilite-2026-09-25.md) | 2026-09-25 | `historical` | Replayability of the `link_*` write instrument — base derived from history, mutation test (#1561) — behind the interwiki write of #1557 |

### Rules_06 owner lot — #1502

| File | Dated | Status | What it is |
|---|---|---|---|
| [`1502-rules06-owner-lot/pivot-rules06-phrases.csv`](1502-rules06-owner-lot/pivot-rules06-phrases.csv) | 2026-10-11 | `current` | Pivot of the Rules_06 / RulesPP_06 phrases, read by the generator config as the DataSet `Rules06LotPhrases` and by the DatasetUpdater task of the lot. Keep the path |
| [`1502-rules06-owner-lot/pivot-rules06-phrases-filled.csv`](1502-rules06-owner-lot/pivot-rules06-phrases-filled.csv) | 2026-10-11 | `historical` | The same pivot filled in eight languages: the output applied to the corpus by #1862 |

### Translation model, DatasetUpdater, English campaign — #202

| File | Dated | Status | What it is |
|---|---|---|---|
| [`202-smoke-datasetupdater-protocole-2026-09-15.md`](202-smoke-datasetupdater-protocole-2026-09-15.md) | 2026-09-15, updated 09-21 | `current` | Protocol for a three-record round-trip smoke run of DatasetUpdater on a scratch copy, gated on an owner GO and a named key; never executed. Its model prerequisite is settled (#1426); its line numbers have moved and one claim no longer holds — see Contradictions |
| [`gpt55-modele-courant-inventaire-2026-09-15.md`](gpt55-modele-courant-inventaire-2026-09-15.md) | 2026-09-15, erratum 09-17 | `superseded` | Inventory showing that the compiled config still named the previous model after the decision. Replaced by #1426, which migrated the config |
| [`202-en-campaign-plan.md`](202-en-campaign-plan.md) | 2026-07-15 | `historical` | Plan, with zero CSV writes, of a post-release English campaign on `Simple_name_en` and `political_example_en` (#809). Never executed; #202 closed |
| [`202-qa-gate-protocol.md`](202-qa-gate-protocol.md) | 2026-07-16 | `historical` | Per-tranche byte-exact spot-check criteria for that campaign, waiting on a register decision (#810). Never executed |

### Coverage and typography

| File | Dated | Status | What it is |
|---|---|---|---|
| [`i18n-remesure-es-ar-fa-zh.md`](i18n-remesure-es-ar-fa-zh.md) | 2026-09-13 | `historical` | Coverage and alignment re-measure of es, ar, fa and zh over five corpora (#1354): full coverage of the substantive fields and no French contamination at its date. Its `link` figures predate the #1471 and #458 fills |
| [`994-typographie-BC-balayage-2026-09-25.md`](994-typographie-BC-balayage-2026-09-25.md) | 2026-09-25 | `historical` | Typographic norm per language — branches B and C and the closing sweep — applied by #1562. #994 is closed |

### Fidelity campaign — Scenarii (#458, October)

| File | Dated | Status | What it is |
|---|---|---|---|
| [`458-fidelite-scenarii-zh-2026-10-04.md`](458-fidelite-scenarii-zh-2026-10-04.md) | 2026-10-04 | `historical` | Fidelity reading of the Scenarii zh columns, zero writes (#1746): full coverage, internal inconsistencies. Consolidated in the matrix below |
| [`458-fidelite-scenarii-ar-2026-10-04.md`](458-fidelite-scenarii-ar-2026-10-04.md) | 2026-10-04 | `historical` | Three-way reading (FR, EN, ar) of the Scenarii ar columns, zero writes (#1749). Consolidated in the matrix below |
| [`458-fidelite-scenarii-fa-2026-10-04.md`](458-fidelite-scenarii-fa-2026-10-04.md) | 2026-10-04 | `historical` | Three-way reading of the Scenarii fa columns, zero writes (#1750). Consolidated in the matrix below |
| [`458-fidelite-scenarii-es-2026-10-04.md`](458-fidelite-scenarii-es-2026-10-04.md) | 2026-10-04 | `historical` | Full three-way reading of the Scenarii es columns, zero writes (#1751). Consolidated in the matrix below |
| [`458-fidelite-scenarii-ru-2026-10-04.md`](458-fidelite-scenarii-ru-2026-10-04.md) | 2026-10-04 | `historical` | Full three-way reading of the Scenarii ru columns, zero writes (#1752). Consolidated in the matrix below |
| [`458-fidelite-scenarii-pt-2026-10-04.md`](458-fidelite-scenarii-pt-2026-10-04.md) | 2026-10-04 | `historical` | Full three-way reading of the Scenarii pt columns, zero writes (#1753); its pt corrections were written by #1769 |
| [`458-fidelite-scenarii-en-2026-10-04.md`](458-fidelite-scenarii-en-2026-10-04.md) | 2026-10-04 | `historical` | Two-way reading (FR, EN) of the Scenarii en columns, the last of the seven, zero writes (#1754) |
| [`458-fidelite-scenarii-en-axe-imprime-2026-10-04.md`](458-fidelite-scenarii-en-axe-imprime-2026-10-04.md) | 2026-10-05 | `historical` | Companion to the en dossier: its defects confronted with the printed English text (#1765), which split printed from never-printed cells |
| [`458-matrice-inter-langues-2026-10-04.md`](458-matrice-inter-langues-2026-10-04.md) | 2026-10-05 | `historical` | Cross-language matrix consolidating the seven Scenarii dossiers into defects and observations, zero writes (#1766). Worked through by the grains up to #1792, which wrote the deferred compositions |
| [`458-soumission-owner-texte-imprime-2026-10-05.md`](458-soumission-owner-texte-imprime-2026-10-05.md) | 2026-10-05 | `historical` | Submission to the owner of the printed text the campaign proposed to change, Rules and Scenarii EN (#1773). Answered yes to all on 05/10; executed by #1778, #1788 and #1790 |
| [`458-v22-g6-titres-ru-cas-par-cas-2026-10-04.md`](458-v22-g6-titres-ru-cas-par-cas-2026-10-04.md) | 2026-10-04 | `historical` | Scenarii ru titles case by case: two restorations written (#1760), the others sent to arbitration, then written by #1774, #1776 and #1784 or kept as they were |

### Fidelity campaign — Rules (#458, October)

| File | Dated | Status | What it is |
|---|---|---|---|
| [`458-fidelite-regles-sources-fr-en-2026-10-04.md`](458-fidelite-regles-sources-fr-en-2026-10-04.md) | 2026-10-04 | `historical` | The two Rules sources read first, FR and EN, zero writes (#1757). Its EN defects went to the owner submission and were executed by #1778 |
| [`458-fidelite-regles-6-langues-2026-10-04.md`](458-fidelite-regles-6-langues-2026-10-04.md) | 2026-10-04 | `current` | The six Rules target languages read against the reference, zero writes (#1758). Still true: the Persian trump term is split between two rows, held as an observation by `RulesTerminologyUnityGuardTests`. Its other defects were worked by #1764, #1768, #1929 and #1941 — not re-measured one by one |

### Fidelity campaign — Virtues (#458, October)

| File | Dated | Status | What it is |
|---|---|---|---|
| [`458-fidelite-virtues-en-2026-10-07.md`](458-fidelite-virtues-en-2026-10-07.md) | 2026-10-07 | `historical` | Two-way reading (FR, EN) of the Virtues en columns, zero writes (#1793; erratum in #1794); the reference for the other six |
| [`458-fidelite-virtues-ru-2026-10-07.md`](458-fidelite-virtues-ru-2026-10-07.md) | 2026-10-07 | `historical` | Three-way reading of the Virtues ru columns, zero writes (#1794) |
| [`458-fidelite-virtues-pt-2026-10-07.md`](458-fidelite-virtues-pt-2026-10-07.md) | 2026-10-07 | `historical` | Reading of the Virtues pt columns, zero writes (#1796): substantially faithful. Its two certain defects were fixed by #1871 and #1808, and its isolated encoding defect no longer shows (measured: the pt cells are NFC) |
| [`458-fidelite-virtues-es-2026-10-07.md`](458-fidelite-virtues-es-2026-10-07.md) | 2026-10-07 | `historical` | Reading of the Virtues es columns, zero writes (#1797): faithful and clean; the defects it saw already in flight were left to #1795 |
| [`458-fidelite-virtues-ar-2026-10-07.md`](458-fidelite-virtues-ar-2026-10-07.md) | 2026-10-07 | `historical` | Three-way reading of the Virtues ar columns, zero writes (#1800) |
| [`458-fidelite-virtues-fa-2026-10-07.md`](458-fidelite-virtues-fa-2026-10-07.md) | 2026-10-07 | `historical` | Three-way reading of the Virtues fa columns, zero writes (#1801) |
| [`458-fidelite-virtues-zh-2026-10-07.md`](458-fidelite-virtues-zh-2026-10-07.md) | 2026-10-07 | `historical` | The last Virtues dossier, zh, with the cross-language synthesis of the campaign in its §6 (#1802). The synthesis items were corrected by #1795, #1798, #1806, #1807 and #1808 |
| [`458-corrections-virtues-g10-2026-10-07.md`](458-corrections-virtues-g10-2026-10-07.md) | 2026-10-07 | `historical` | The 28 Virtues fidelity cells written by #1795. The mind-map lag of its §4 is closed (measured); the OWL lag of its §4 bis is still open and governed by the ontology file and #133 named above |

### Translation-defect detection trial — #1520

| File | Dated | Status | What it is |
|---|---|---|---|
| [`luna6-essai-2026-11-10/`](luna6-essai-2026-11-10/) | 2026-11-10 / 11 | `historical` | A model trial detecting translation defects on Fallacies clusters (#1520, closed; #2231): the dossier `README.md`, the seven files of its instruments (scripts and prompts), the planted-defect control and the raw and aggregated results — sixteen files, zero writes under `Cards/`. Its first draft was corrected on review and is kept in its annex C: the corrected §1 holds |

## Contradictions

None is corrected in the files: an index changes no finding. Each line says which side holds.

- The Scenarii `en` dossier (§5) and the cross-language matrix (§7) count the lines where the English is the source differently: the matrix, which consolidates all seven dossiers, holds.
- `1471-caracterisation-778-cellules.md` dates its class-A count after #1487, `1471-write-22A-2026-09-22.md` before it: what holds is the 24 cells re-measured by the class-A quality audit, 22 of which were written (#1508).
- The class B and E sizes of the characterisation against the tiering of class B: the tiering holds — it re-measures through interwiki and names the artefact.
- The class-E re-survey (class E massively over-estimated) against the class-E relevance audit (about one hit in ten relevant): the relevance audit holds; the owner decision of 22/09 and #1554 rest on it.
- The title and §0 of the model inventory against today's config: #1426 holds (measured: 53 literals of the decided model, none of the previous one).
- The smoke protocol cites mechanism lines that have moved (for instance `TakeChunkNb`, line 72 at its base, line 92 today) and says the `SelectEmptyTargets` usage block is commented out, while an active group filter on that flag exists (line 240 at its base, line 266 today); its occurrence count of the model has grown from 48 to 53. Whether the recipe of its §2 changes: undetermined — needs the owning lane.
- The es/ar/fa/zh re-measure concludes that no editorial question remains, yet the `Campaign` sub-category was a false friend, fixed by #1434 six days later; its `link` figures are also older than the #1471 and #458 fills. For coverage, re-measure; for links, the #1471 issue holds.
- The yield grain and the title of #1471 size the epic by its opening count; the closure dossier of 12/11 re-measures the residual at 438, as does this index: the dossier holds.

## Statuses to counter-review

The owning lane should sample these first: the judgement was closest to the line.

- The smoke protocol as `current`: its issue #202 is closed, but the protocol still governs a gesture to come (item 9 of the next steps), and one of its claims does not hold today.
- The six-language Rules dossier as `current`: only the Persian trump-term observation was re-measured; its other defects were not re-checked one by one.
- The English campaign plan and its QA gate as `historical`: never executed, the gap they target is unchanged, and the release they were waiting for has not shipped. If the campaign is still wanted, they are current and need an open issue.
- The Scenarii `es`, `pt` and `en` dossiers as `historical`: the corrections are traced through the matrix and the pull requests, not re-measured observation by observation.
- The es/ar/fa/zh re-measure as `historical`: `docs/agents/status-and-history.md` still cites it for coverage, which no later file re-measured in full.
