# Changelog

All notable changes to the Argumentum project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.9.0] — 2026-06-XX

### Added — Multilingual Support (8 Languages)

The entire pipeline now generates assets in **8 languages**: French, English, Russian, Portuguese, Spanish, Arabic, Farsi, and Chinese.

- **CSV data**: 100% translation coverage for Fallacies, Virtues, Scenarii, and Rules across all 8 languages (title, description, example, family hierarchy, links)
- **PDF generation**: CardSets (Tarot, Poker, A0 posters, Print&Play) produce localized PDFs for all 8 languages via `CardSetLocalizations` field mapping
- **MindMap SVGs**: FreeMind mind maps generated for all 8 languages with localized node text, family hierarchy, and descriptions
- **Mémo cards**: Front/back cards with language-invariant grouping selectors and localized taxonomy labels
- **Rules cards**: Translated rule content for all 8 languages including AR/ES/FA/ZH additions
- **Entity layer**: Added AR/FA/ZH properties to `Fallacy.cs` (21 properties + 21 CsvHelper mappings) and `Rule.cs` (4 properties)
- **MindMap localization**: Extended `StaticConversions` in `AssetConverterConfig.cs` for AR/FA/ZH text, description, example, link, and family hierarchy fields
- **Taxonomy validation**: Test suite updated to validate all 8 languages (`_supportedLanguages: fr/en/ru/pt/es/ar/fa/zh`)

### Added — Pipeline Infrastructure

- **DatasetUpdater** (PR #210): Migration to official OpenAI .NET SDK v2.10.0 with multi-provider support (OpenAI + alternative providers). 7 task configs, 29 prompt files, function calling with structured output
- **GSheet ↔ CSV Sync** (PR #200): Bidirectional synchronization module with 6-layer upload protection (dry-run, diff, thresholds, confirmation, backup, verify). 4 spreadsheet configs for Fallacies, Scenarii, Virtues, Rules
- **Visual QA harness** (#412): Mechanical pixel-detector system for automated visual regression testing of generated PDFs and cards
- **Footer-collision detector** (#412 follow-up): Per-CardSet calibrated thresholds for Rules overflow detection
- **Playwright visual regression tests** (#212 scaffold): Test infrastructure for screenshot-based PDF verification

### Added — Documentation

- **Publication validation guide** (#140): Comprehensive per-language visual verification guide with scenarios and acceptance criteria
- **Architecture documentation**: `ARCHITECTURE_PIPELINE.md` with pipeline stage descriptions and data flow
- **SDDD methodology docs**: Investigation methodology, conversation grounding, and triple-grounding protocol
- **37 investigation reports** (2025-2026): Complete regression history, git archaeology, and closure reports in `docs/investigations/`

### Fixed — Pipeline Recovery (Oct 2025 — Jun 2026)

Complete restoration of the generation pipeline from the April 2024 Golden Master state:

- **HarvestManager**: Restored 120s timeout (from 60s), removed manual frame.js injection, added explicit `generateImages()` call, raised Playwright timeout to 300s for heavy CardSets, fixed CardPen global race condition in Release mode
- **PdfManager**: Restored global QuestPDF lock (NOT thread-safe), fixed Rules card positioning (#119)
- **CardPen templates**: Restored `argumentsVertueux` CSS class, fixed Scenarii asset paths to GitHub URLs, auto-shrink overflowing card titles (#316), auto-shrink Virtues body text overflow (#420)
- **CSV injection**: Restored Golden Master CSV injection (no `Replace("\n", "\\n")` — PapaParse handles newlines correctly)
- **Configuration**: Removed erroneous `RowsetNb=14` for Scenarii CardSet, restored Virtues CardSet (critical for Print&Play Tarot)
- **SVG generation**: Replaced XSLT-based SVGs with FreeMind Batik SVGs (20 SVGs, 4 languages × 5 types), automated FreePlane GUI via `SendKeys.SendWait`
- **Mémo Back cards**: Language-invariant control-break grouping (#449), localized taxonomy labels for all 8 languages (#446), cyrillic font fallbacks + vertical grid auto-fit for RU (#452)

### Fixed — Data Quality

- **Fallacies FR audit**: Cell-by-cell review of all 7 family roots — selective revert of bloated/MT-contaminated descriptions (135+ cells audited, ~30 reverts)
- **Fallacies i18n**: Deterministic translation consistency fixes across all 8 languages (#432), cosmetic polish for PT/FA/ZH (#411, #424, #447)
- **Scenarii i18n**: 167/167 records 100% covered for EN/RU/PT across all 8 translatable fields (was 54% — 76 records missing)
- **Virtues i18n**: 100% coverage for title/description/remark × 4 languages (was 0% — added via PRs #218/#236/#246/#290/#295)
- **Rules i18n**: PT retranslation (#211), AR/ES/FA/ZH P&P content added, cover row EN contamination fixed
- **Fallacies taxonomy**: Propagated hierarchy labels to true 100% across 8 languages (#408)
- **Virtues FA blank-fill**: 100% i18n coverage across all 8 languages (#407)
- **PT Scenarii**: Deterministic enclitic-hyphen + MT-artefact cleanup (#383)

### Changed

- **DNN platform**: Upgraded 2sxc 15→21.07 with Razor14 migration + IRenderService fix (#418)
- **Dependencies**: System.Linq.Dynamic.Core 1.3.12→1.7.2 (security), Spectre.Console→0.50.0, QuestPDF pinned to 2022.12.12 (MIT free license), Magick.NET 13.5.0, SkiaSharp.NativeAssets.Win32 2.88.6
- **Build artifacts**: Stopped tracking 968 MB of regenerable build outputs (#415 Phase 1)
- **Mémo Back taxonomy**: Grouping selector now language-invariant (CSS class-based, not text-based)

### Test Coverage

- **155 tests** pass (up from 0 in April 2024)
- Coverage includes: CsvDiffEngine, SyncSafetyChecker, DiffReport, CsvToGrid, MindMapHtmlWrapper, FallaciesLocalizationTests, TaxonomyValidationTests, Memo_Back localization, Playwright visual tests
- 1 skip (Freeplane GUI — requires interactive session)
- 5 skips (visual/infrastructure-dependent)

### Migration Notes

- **SkipConfigFile** must be `true` in `AssetConverterConfig.cs` — tuple serialization breaks `Translations` in JSON config
- **CSV data is read-only** — never modify before CardPen injection
- **QuestPDF global lock** — do not remove or parallelize PDF generation
- **Playwright timeout** — minimum 120s for harvesting, 300s for heavy CardSets
