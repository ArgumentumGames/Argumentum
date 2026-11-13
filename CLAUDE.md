# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

**Start with [`AGENTS.md`](AGENTS.md)** — the cross-vendor entry point, and the single source for what the repository is, its map, the build and test commands, the index of invariants and the contribution protocol. The line below imports it, so Claude Code loads it together with this file; nothing it states is repeated here.

@AGENTS.md

## What this file holds

Two things, and only these: the **silent failures** of the pipeline — what has to be in context *before* the gesture, because nothing will warn afterwards — and the **map** from every section this file used to carry to the file that carries it now.

The detail itself (≈ 600 lines until 2026-11-13) lives in [`docs/agents/`](docs/agents/). It was moved **verbatim** and is **not loaded by default**: open the file the table or the map names when the task calls for it.

## Silent failures — read the section before the gesture

Most of these raise no error at all: a green run, an exit code 0, an empty result that reads as "nothing to do". The others fail far from their cause. Files are under `docs/agents/`.

| Before you… | What happens | Read first |
|---|---|---|
| re-derive mind maps | every committed HTML wrapper is **skipped** unless `OverwriteExistingHtmlMaps = true`: the SVGs are repaired, the wrappers still inline the old ones, and one INFO line is all there is to show for it | `pipeline.md` § *Pipeline Stages*, item 3 |
| export mind-map SVGs | `FreeMind.exe` not found ⇒ warning, XSLT fallback (dead) ⇒ the SHA-diff check fails silently | same |
| count mind-map files, wrappers or thumbnails | the tree holds 43 SVG and 34 wrappers, not "20 per language"; half of the wrappers carry nothing by construction; thumbnails are inline base64, so a search for `.png` returns 0 | same, and `status-and-history.md` § *État actuel par CardSet* |
| expect CMYK from `-c Release` | it ships RGB: the CMYK pass sits behind the `Mode` flag and only runs through `--pdf-cmyk`; without Ghostscript on `PATH` it skips every PDF with a warning | `build-modes-and-cmyk.md` |
| validate a card-template fix with a Release render | Release downloads the template from `master`: the branch fix is never read, and the render is byte-identical run after run | `regeneration.md` § *A branch fix to a card template…* |
| read a template's CSS with a regex | the CSS is an escaped JSON string: a regex over the raw bytes captures nothing, which reads as "this declaration is empty" | same section, last paragraph |
| set `UseLocalCardpen = false` | asset URLs resolve to GitHub Pages ⇒ HTTP 404 ⇒ 0 images ⇒ 0 PDF | `regeneration.md` § *Local CardPen is mandatory…* |
| launch a regeneration | from Git-Bash the short junction is resolved and image paths cross the 260-character limit; a first run in a virgin `bin/Release` dies on `Console.ReadKey()` — the control is the log, never the exit code | `regeneration.md` § *Regeneration runs*, § *Regenerating a SINGLE language* |
| hand a regenerated bundle to a reviewer | the build tree, the review bundle and the published release assets are three objects, and the third does not move on its own (GitHub also rewrites `&` to `.` in asset names) | `regeneration.md` § *Regenerating a SINGLE language* |
| add or change a card set or a dataset | a missing `CsvType`, a forced `RowsetNb`, or CSV content altered before injection ⇒ empty cards, no error | `card-templates.md` |
| rename or search a Virtues family | a missing CSS class gives a white card; an old family name returns 0 rows — an instrument failure, not an absence | `card-templates.md` § *Classes CSS Familles* |
| bump a NuGet package | QuestPDF and AutoMapper are licence-pinned; audit warnings appear on `restore --force`, not on an incremental build | `dependencies.md` |
| read artefacts from `Target/` | the Debug and Release trees are independent, so one of them is stale; the PNGs sit two levels under `{lang}/Images/` | `pipeline.md` § *Output Directories* |
| quote a card count or a test count, or pick a "next step" | the status tables are dated snapshots; two unstruck list items pointed at closed issues, and one produced a false dispatch | `status-and-history.md`, then the issue itself (`AGENTS.md` §5) |

## Where the former sections went

Issues, pull requests and other files cite this file by section title (*CLAUDE.md § Stable Dependency Versions*). The titles did not change; each one now lives in a single file under `docs/agents/`:

- [`pipeline.md`](docs/agents/pipeline.md) — Architecture: Configuration System (Critical), Pipeline Stages, CardPen (Custom Fork) · Project Structure · Key Technical Considerations: CsvHelper Mapping, Thread Safety, Known Fragile Areas · Multilingual Support · Output Directories · `SkipConfigFile` — deliberate `true` (NOT a regression) · Critical Config Values to Verify
- [`build-modes-and-cmyk.md`](docs/agents/build-modes-and-cmyk.md) — Debug vs Release Builds
- [`regeneration.md`](docs/agents/regeneration.md) — Regeneration runs — launch shell and path length · Regenerating a SINGLE language — the `LocalizationConfig.Enabled` lever · Local CardPen is mandatory for every regen, Debug AND Release · A branch fix to a card template is NOT verifiable by a Release render
- [`card-templates.md`](docs/agents/card-templates.md) — Leçons Apprises (Pipeline Recovery 2025-2026): CardPen Template Configuration, Calcul du nombre d'images attendues, CSS Card Templates, domtoimage (frame.js), Chemins d'assets relatifs, Debugging Tips, CsvType et DataSets, Dimensions FallaciesWeb A0, RowsetNb vs rscount (CRITIQUE), CSV Injection dans CardPen (CRITIQUE), Classes CSS Familles (Virtues/Fallacies)
- [`dependencies.md`](docs/agents/dependencies.md) — Stable Dependency Versions
- [`status-and-history.md`](docs/agents/status-and-history.md) — Historical Context & Known Issues: Golden Master: April 2024, Applied Corrections (Oct-Dec 2025), Known CSV Mapping Issues — ALL RESOLVED · Diagnostic Resources: Investigation Reports, PowerShell Diagnostic Scripts, Reference Files (Git Archaeology) · Pipeline Recovery Status (Mars 2026): Validation Multilingue, État actuel par CardSet, Mind Maps & SVGs, Translation Pipeline (DatasetUpdater), GSheet ↔ CSV Sync, Test Coverage, Prochaines étapes, Commits clés de la recovery, Data Quality Issues

Stated once in `AGENTS.md` since the same date: Project Overview (§1) · Build & Run Commands (§3) · Golden Rule: Code = Truth (§4).

## Adding to this file

A fact earns a row here only if it must be in context **before** a gesture and fails silently otherwise: one row in the table, its detail under `docs/agents/`. How to contribute belongs to `AGENTS.md`. Measures, incidents and history belong to `docs/` — never here. [`docs/agents/README.md`](docs/agents/README.md) states the rule, and how the move was proven lossless.

## Related Documentation

- [ARCHITECTURE_PIPELINE.md](Generation/Documentation/ARCHITECTURE_PIPELINE.md) - Detailed pipeline architecture
- [docs/sddd/](docs/sddd/) - SDDD methodology and investigation reports
- [docs/investigations/](docs/investigations/) - Debug/archaeology reports (37 reports + scripts)
