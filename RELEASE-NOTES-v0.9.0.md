# Release Notes — Argumentum v0.9.0

**Release date**: TBD (pending DNN deployment, see #134)
**Scope**: 8-language support (FR, EN, RU, PT, ES, AR, FA, ZH)

---

## Highlights

### 🌍 8-Language Support

Argumentum now generates its entire educational card game in **8 languages**:

| Language | Code | Script | Status |
|----------|------|--------|--------|
| Français | `fr` | Latin | ✅ Complete (source) |
| English | `en` | Latin | ✅ Complete |
| Русский | `ru` | Cyrillic | ✅ Complete |
| Português | `pt` | Latin | ✅ Complete |
| Español | `es` | Latin | ✅ Complete |
| العربية | `ar` | Arabic (RTL) | ✅ Complete |
| فارسی | `fa` | Persian (RTL) | ✅ Complete |
| 中文 | `zh` | CJK | ✅ Complete |

All CSV data (Fallacies, Virtues, Scenarii, Rules) is 100% translated across all 8 languages. The generation pipeline produces localized PDFs and card images for all 8 languages. MindMap SVGs are currently committed for FR/EN/RU/PT; the pipeline is configured for ES/AR/FA/ZH with SVG regeneration pending (see Known Limitations).

### 🃏 Generated Assets

| Asset Type | Languages | Count |
|------------|-----------|-------|
| Tarot PDFs (cards + Virtues + Print&Play A4) | 8 | 24 |
| Poker PDFs (cards + Print&Play A4) | 8 | 16 |
| Fallacies Web PDFs (A0 poster + A4 + Thumbnails) | 8 | 24 |
| **Total PDFs** | 8 | **64** |
| MindMap SVGs | 4 (FR/EN/RU/PT) | 21 |
| Card Images (PNG) | 8 | ~9,834 |
| OWL Ontology | 1 (FR) | 1 (664 KB) |

### 🛠 Pipeline Recovery

After a series of regressions (May–September 2025), the entire .NET generation pipeline has been **fully restored and validated** against the April 2024 Golden Master:

- **HarvestManager**: Playwright-based card image generation restored with correct timeouts and CSV injection
- **PdfManager**: QuestPDF assembly working with thread-safe serialization
- **MindMapper**: FreeMind + Batik SVG generation automated (including FreePlane GUI)
- **Tests**: 155 automated tests (up from 0)

### 📊 Data Quality

- **Fallacies FR audit**: All 7 family roots reviewed cell-by-cell — bloated/MT-contaminated descriptions reverted to authoritative FR source
- **Deterministic i18n**: Translation consistency enforced across all 8 languages (no MT artefacts, correct scripts for RU/AR/FA/ZH)
- **Virtues**: 100% translated (title/description/remark × 4 languages, extended to 8)
- **Scenarii**: 167/167 records fully translated (was 54%)

---

## New Modules

### DatasetUpdater (Translation Pipeline)

Automated CSV translation using OpenAI GPT with structured output:

- **SDK**: Official OpenAI .NET SDK v2.10.0
- **Multi-provider**: OpenAI + alternative providers configurable
- **Tasks**: 7 task configs (all disabled by default — enable for on-demand translation)
- **Prompts**: 29 prompt files with function calling + JSON schema validation

### GSheet ↔ CSV Sync

Bidirectional synchronization between Google Sheets and local CSV files:

- **6-layer safety**: dry-run → diff report → threshold check → user confirmation → backup → verify
- **4 spreadsheet configs**: Fallacies, Scenarii, Virtues, Rules
- **Formula protection**: Preserves Google Sheets formulas during sync

### Visual QA Harness

Automated visual regression testing for generated assets:

- **Pixel detectors**: Footer-collision, text-overflow, margin calibration
- **Per-CardSet thresholds**: Custom calibration for each card type
- **8-language validation**: Automated checks across all supported languages

---

## Known Limitations

1. **RTL languages (AR, FA)**: Card layouts assume LTR — RTL-aware CSS may be needed for optimal rendering
2. **CJK fonts (ZH)**: Requires system-installed CJK fonts for correct rendering in PDFs and card images
3. **DNN site**: Deployment pending (#131/#132) — release coupled with site update (Decision 2)
4. **OWLOntology**: Published in French only — multilingual ontology planned for future release
5. **MindMap SVGs for ES/AR/FA/ZH**: Only FR/EN/RU/PT SVGs are committed (21 files). The pipeline is configured for ES/AR/FA/ZH (`StaticConversions` + `MindMapLocalization` in `AssetConverterConfig.cs`), but SVG regeneration is pending — it requires an attended FreePlane GUI run (`SendKeys.SendWait` desktop automation, skipped by the test suite as "requires interactive session"). Tracked as #458 Track 1a.

---

## Breaking Changes

- **SkipConfigFile = true**: The JSON config file is now ignored by default. C# source code is the single source of truth. This was necessary because `System.Text.Json` cannot serialize value tuples used in `Translations` lists.
- **`_supportedLanguages` expanded**: Test classes that validate language coverage now check 8 languages (was 4). Any custom test fixtures must be updated accordingly.
- **Fallacy entity**: 21 new properties added (AR/FA/ZH). CSV files must include corresponding columns or use `.Optional()` mappings.

---

## Dependencies

| Package | Version | Change |
|---------|---------|--------|
| QuestPDF | 2022.12.12 | Pinned (MIT free license, thread-safety boundary) |
| Magick.NET | 13.5.0 | Stable for SVG conversion |
| Microsoft.Playwright | 1.43.0 | Browser automation |
| OpenAI .NET SDK | 2.10.0 | New — translation pipeline |
| System.Linq.Dynamic.Core | 1.7.2 | Security upgrade (GHSA-4cv2-4hjh-77rx) |
| SkiaSharp.NativeAssets.Win32 | 2.88.6 | QuestPDF rendering backend |

---

## Contributors

- **jsboige** — Project lead, data curation, French source validation
- **Claude (ai-01)** — Pipeline recovery, code review, visual validation, documentation
- **Claude (po-2023)** — Heavy pipeline builds, data quality audits, translation pipeline operations

---

## Next Steps (v1.0 Roadmap)

- [ ] #131/#132 — DNN site deployment + Stripe integration
- [ ] #134 — GitHub Release with packaged assets
- [ ] #415 — `git-filter-repo` history cleanup (3-person coordinated operation)
- [ ] #212 — Full Playwright visual regression test suite
- [ ] #133 — OWL ontology publication
- [ ] RTL-aware card templates for Arabic and Farsi
- [ ] CJK font optimization for Chinese cards
