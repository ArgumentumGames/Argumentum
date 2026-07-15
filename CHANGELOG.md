# Changelog

All notable changes to the Argumentum project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.9.0] — 2026-07-XX (tag pending jsboige visual GO, week of 2026-07-13)

### Added — Multilingual Support (8 Languages)

The entire pipeline now generates assets in **8 languages**: French, English, Russian, Portuguese, Spanish, Arabic, Farsi, and Chinese.

- **CSV data**: 100% translation coverage for Fallacies, Virtues, Scenarii, and Rules across all 8 languages (title, description, example, family hierarchy, links)
- **PDF generation**: CardSets (Tarot, Poker, A0 posters, Print&Play) produce localized PDFs for all 8 languages via `CardSetLocalizations` field mapping
- **MindMap SVGs**: FreeMind/Batik mind maps committed for all 8 languages (FR/EN/RU/PT refreshed + ES/AR/FA/ZH added, PR #565) with localized node text, family hierarchy, and descriptions, including RTL (ar/fa) and CJK (zh) rendering via Tahoma-capable fonts
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

### Added — Print Production (CMYK & Print&Play)

The v0.9.0 release bundle is **print-ready**: 80 PDFs (10 document types × 8 languages), DeviceCMYK + SWOP OutputIntent, verified by the Ghostscript post-process (#632/#652).

- **CMYK post-process** (#632): Standalone Ghostscript pass converts the final PDFs to DeviceCMYK and embeds the SWOP OutputIntent (ICC profile auto-extracted via Magick.NET). Resolves the per-image CMYK oxymore — `DocumentCardSet.cs` converts to CMYK but writes PNG, which cannot carry CMYK (Magick re-encodes to RGB); the authoritative CMYK path is therefore the GS post-process on the final PDF, not per-image conversion. Entry-point `--pdf-cmyk` (#652) runs the pass standalone on an existing bundle (`Mode=PdfCmykPostProcess` alone, `ForceReleaseParams`)
- **Bundle v3** (regenerated 2026-07-03): 80 PDFs (10 types × 8 languages), 6.18 GB RGB → **5.30 GB CMYK**, PNG-300-lossless source. 80/80 converted DeviceCMYK + OutputIntent SWOP (proof: `TarotCards_fr` = DeviceCMYK 195 / DeviceRGB 0 / OutputIntent 3 / ICC SWOP 2)
- **Print&Play Standard + Light** (#645/#648-650): The digital edition is entirely free. **Light** = cards flagged in the `print_and_play` column (+ Virtues families overview subset). **Standard** = all cards. Expanded the bundle from 64 → 80 PDFs (+2 document types). Virtues Light subset broadened to depth ≤ 2 (24 cards)
- **GS timeout hardening** (#670): Raised per-PDF Ghostscript timeout 180s → 900s (23 PDFs initially timed out at 180s on dense A0)

### Added — OWL Ontology (Bilingual EN/FR)

- **Fallacies OWL** (`docs/ontology/argumentum.owl`, 5.07 MB): SKOS + AIF, 1408 fallacies, bilingual literals (EN 5558 + FR 4861), 2816 `prefLabel`, 1408 `broader` (full hierarchy). Freshly regenerated 2026-07-02 (#634 — previous commit `d206e59c` was ~3 months stale)
- **Virtues OWL** (#592/#499 Phase 2, `docs/ontology/argumentum_virtues.owl`, 842 KB): 223 Virtues, 223 `aif:goodTenorOf` assertions, 7 families, bilingual literals (FR 884 + EN 641). Relational prod-write Phase 1 (#499, 66 → 78 columns, 12 additive)
- **Scope note (honest)**: the OWL generator is bilingual (FR canonical + EN secondary) — it does **not** carry the 6 other release languages (RU/PT/ES/AR/FA/ZH). The 8-language claim applies to CSV/PDF/SVG, **not** to OWL
- **Three relational layers now serialized in `argumentum.owl`** (#787, freshly regenerated from master `95b4210b`): (1) **skos Walton** — 70 native scheme mappings (`AIF_skos*Ref`, #753); (2) **crossLink** — 1985 inter-fallacy relational verbs across 8 predicates (#763); (3) **AIF attack** — 145 attack-typed fallacies with deterministic ASPIC+ node mapping (#498, see below). The committed OWL had drifted to 93 AIF assertions before the P1 reconciliation regen; #787 realigns it to the 145 prod value
- #133 (OWL publication) certified **publication-safe** (#793, 2026-07-12): `skos:inScheme` survives the OWLSharp XML round-trip (1408 assertions); only `rdf:type` drops on reload, which is asserted-as-expected and worked around by the survivor-fallback read path — the generated ontology is otherwise complete (SKOS + AIF)

### Added — AIF Argumentation Layer (#498 Fallacies, #499 Virtues)

A two-layer reconciliation of the taxonomy against the **ASPIC+ / AIF (Argument Interchange Format)** framework, giving each fallacy and virtue a formal attack semantics. Contract columns added in #753 (`AIF_attackType`, `AIF_attackedNode`, plus the `AIF_skos*Ref` mapping columns).

- **Fallacies attack layer (#498)** — **145 / 1408 fallacies** now carry a typed AIF attack (10.3%), with a **deterministic node map** (ASPIC+ Option (a), #707 §4): `undercut → RA-node` (attacks the inference/rule, 87), `undermine → I-node` (attacks the premise, 53), `rebut → CA-node` (attacks the conclusion, 5). Rebut is a structural tail case (~3%), localized to appeal-to-consequences — relational fallacies (personal attack, genetic fallacy) are modelled as undermine/undercut, not rebut, because they reject without an independent counter-conclusion
- **P1 skos-only reconciliation complete** (#498 P1, 93 → 145): the 52 fallacies that carried a skos signature but no attackType were back-filled across 7 tranches (tranche-1 #769/#771, 1b #773/#776, 1c #775/#779, 1d #778, 1e #780, 1f #781, 1g #784/#785; batched write 1d+1e+1f #783). Tiering by confidence: 14 PRECEDENT (exact-token precedent in-set) + 2 PREC-TIE (competing precedents, tie broken by majority + desc) + 36 SUFFIX-ONLY (skos token present, no typed precedent). **0 residual** skos-only rows; **0 token fabricated** — every attackType derived from the row's own skos signature + `desc_fr`, never inherited from the anchor (anchor audit #770: 16 CLEAN / 2 SOFT / 0 error; 0/19 anchors align with their sub-sub-leaves, proving inheritance would be fabrication)
- **crossLink relational layer (#763)** — **8 inter-fallacy relational verbs** (`predatesOn`, `denounces`, `leverages`, `allows`, `opposes`, `inverts`, `mirrors`, `isRelatedTo`) filling **1081 cells across 844 fallacies (59.9%)**, emitted as OWL object properties with symmetric-flag handling. Mirrors (#721) and isRelatedTo are the densest predicates
- **Virtues mirror (#499)** — **222 / 223 Virtues** carry an AIF attack type (Option A "resisted attack" ratified by ai-01 under jsboige delegation): 206 `undercut/RA-node`, 13 `undermine/I-node`, 3 `rebut/CA-node`. Prod-write Phase 1 (#755, script #754) extended the Virtues schema 66 → 78 columns (12 additive attack-shaped columns)
- **Anchor audit (#770)** and **tiering methodology** documented in `docs/taxonomy/498-reconciliation-p1-closure.md`; the same-token divergence principle (desc-driven, token non-decisif) is validated by two documented cases (pk808/pk33, pk888/pk2)
- **Layer C** (~1263 leaves without any skos signature) is **out of scope for v0.9.0** — a generative Walton-mapping pass is a change of nature (generation vs back-fill) requiring its own gated pilot; escalated to jsboige for a post-tag decision

### Fixed — Pipeline Recovery (Oct 2025 — Jun 2026)

Complete restoration of the generation pipeline from the April 2024 Golden Master state:

- **HarvestManager**: Restored 120s timeout (from 60s), removed manual frame.js injection, added explicit `generateImages()` call, raised Playwright timeout to 300s for heavy CardSets, fixed CardPen global race condition in Release mode
- **PdfManager**: Restored global QuestPDF lock (NOT thread-safe), fixed Rules card positioning (#119)
- **CardPen templates**: Restored `argumentsVertueux` CSS class, fixed Scenarii asset paths to GitHub URLs, auto-shrink overflowing card titles (#316), auto-shrink Virtues body text overflow (#420)
- **CSV injection**: Restored Golden Master CSV injection (no `Replace("\n", "\\n")` — PapaParse handles newlines correctly)
- **Configuration**: Removed erroneous `RowsetNb=14` for Scenarii CardSet, restored Virtues CardSet (critical for Print&Play Tarot)
- **SVG generation**: Replaced XSLT-based SVGs with FreeMind Batik SVGs across all 8 languages (FR/EN/RU/PT + ES/AR/FA/ZH, PR #565), automated FreePlane GUI via `SendKeys.SendWait`
- **Mémo Back cards**: Language-invariant control-break grouping (#449), localized taxonomy labels for all 8 languages (#446), cyrillic font fallbacks + vertical grid auto-fit for RU (#452)

### Fixed — Pipeline Stability (Jun — Jul 2026)

Late-cycle hardening of the harvest and rendering pipeline, post-recovery:

- **Harvest deadlock** (#651): Root-caused a flat 0-CPU freeze (no crash) — the `page.Console` handler logged 5 synchronous lines per message (File I/O + lock + AnsiConsole non-thread-safe) on the Playwright event-dispatch thread, drowning the transport under CardPen's console flood. Fix = minimal capture (`error` only) drained on the main thread. The "Fallacies freeze" was a false-positive of watcher-v1 (parent .NET CPU only; Chromium was rendering at 1.25 cores)
- **Serial retry for failed harvest sets** (#613/#676, Option C): Failed CardSet harvests are now retried serially with backoff (`HarvestSetRetryAttempts`, `HarvestSetRetryBackoffSeconds`) after the parallel drain — additive, gated, does not touch `ParallelismOptimizer`. `RetryAsync` contract pinned by 8 unit tests (#678)
- **Logger Spectre markup** (#630/#655): Fixed Spectre.Console escape asymmetry in `Logger.cs` + made the `[HARVEST-FAILURE]` path non-throwing (it previously short-circuited the #614 resilience path on set failure)
- **CMYK oxymore resolved** (#632): The per-image `ConvertToCmyk` in `DocumentCardSet.cs` was effectively a no-op for PNG output (PNG cannot carry CMYK; Magick re-encodes to RGB on write). Authoritative CMYK now applied via Ghostscript post-process on the final PDF — see *Print Production* above
- **Scenarii "Johnny Johnny" duplication** (#653, 6.1.3): Fixed across 6 languages (corrects #644)
- **Rules i18n refonte** (#640): Eliminated 23 HIGH "English Channel" mistranslations (homonyme "Manche" round→geography) via gpt-5.5 rewrite — 0 HIGH residual
- **Taxonomy CSV hygiene** (#579/#581/#584): Repaired systemic `%C3→A13` encoding corruption in JSON templates (Fallacies, Memo, 4 live Fallacies templates)
- **OWL staleness** (#634): Regenerated the stale Fallacies OWL (previous commit `d206e59c` dated 2026-03-28, ~3 months stale) — was a release blocker

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
- **Dependencies**: System.Linq.Dynamic.Core 1.3.12→1.7.2 (security), Spectre.Console→0.50.0, QuestPDF pinned to 2022.12.12 (MIT free license), Magick.NET-Q16-AnyCPU 14.14.0 (dependabot-tracked), SkiaSharp.NativeAssets.Win32 2.88.6
- **Build artifacts**: Stopped tracking 968 MB of regenerable build outputs (#415 Phase 1)
- **Mémo Back taxonomy**: Grouping selector now language-invariant (CSS class-based, not text-based)

### Test Coverage

- **596 tests pass** (`dotnet test` on `Argumentum.AssetConverter.Tests`, 2026-07-14, .NET 9 — 601 total: 596 pass / 0 fail / 5 skip), 5 skips (GUI/infrastructure). The last known-fail (#133, OWLSharp XML round-trip) was cleared in #793: `skos:inScheme` now survives the round-trip (1408 assertions); the residual `rdf:type` reload-drop is asserted-as-expected and worked around by the survivor-fallback read path — does not affect generated assets. Zero-warning build (CS compiler + NuGet audit, #587)
- Coverage includes: CsvDiffEngine, SyncSafetyChecker, DiffReport, CsvToGrid, MindMapHtmlWrapper, FallaciesLocalizationTests, TaxonomyValidationTests, Memo_Back localization, HarvestManager `RetryAsync` contract (#678), Virtues mindmap wrapper localization (#738), Playwright visual tests

### Migration Notes

- **SkipConfigFile** must be `true` in `AssetConverterConfig.cs` — tuple serialization breaks `Translations` in JSON config
- **CSV data is read-only** — never modify before CardPen injection
- **QuestPDF global lock** — do not remove or parallelize PDF generation
- **Playwright timeout** — minimum 120s for harvesting, 300s for heavy CardSets
