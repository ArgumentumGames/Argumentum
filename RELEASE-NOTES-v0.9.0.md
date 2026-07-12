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

All CSV data (Fallacies, Virtues, Scenarii, Rules) is 100% translated across all 8 languages. The generation pipeline produces localized PDFs and card images for all 8 languages. MindMap SVGs (FreeMind/Batik) are committed for **all 8 languages** (PR #565), including RTL (ar/fa) and CJK (zh) rendering.

### 🃏 Generated Assets

| Asset Type | Languages | Count |
|------------|-----------|-------|
| Tarot PDFs (cards + Virtues + Print&Play A4) | 8 | 24 |
| Poker PDFs (cards + Print&Play A4) | 8 | 16 |
| Fallacies Web PDFs (A0 poster + A4 + Thumbnails) | 8 | 24 |
| **Total PDFs** | 8 | **64** |
| MindMap SVGs | 8 (FR/EN/RU/PT/ES/AR/FA/ZH) | 41 |
| Card Images (PNG) | 8 | ~9,834 |
| OWL Ontology | 1 (bilingual EN/FR) | 1 (~5.9 MB) |

### 🛠 Pipeline Recovery

After a series of regressions (May–September 2025), the entire .NET generation pipeline has been **fully restored and validated** against the April 2024 Golden Master:

- **HarvestManager**: Playwright-based card image generation restored with correct timeouts and CSV injection
- **PdfManager**: QuestPDF assembly working with thread-safe serialization
- **MindMapper**: FreeMind + Batik SVG generation automated (including FreePlane GUI)
- **Tests**: 595 automated tests (600 total: 595 pass / 1 known-fail / 5 skip), up from 0 at the start of recovery; zero-warning build

### 📊 Data Quality

- **Fallacies FR audit**: All 7 family roots reviewed cell-by-cell — bloated/MT-contaminated descriptions reverted to authoritative FR source
- **Deterministic i18n**: Translation consistency enforced across all 8 languages (no MT artefacts, correct scripts for RU/AR/FA/ZH)
- **Virtues**: 100% translated (title/description/remark × 4 languages, extended to 8)
- **Scenarii**: 167/167 records fully translated (was 54%)

### 🧠 AIF Argumentation Layer (#498/#499)

The taxonomy is now reconciled against the **ASPIC+ / AIF** framework — each typed fallacy and virtue carries a formal attack semantics:

- **145 fallacies** typed (undercut/RA-node ×87, undermine/I-node ×53, rebut/CA-node ×5), deterministic node map (ASPIC+ Option (a)). P1 reconciliation back-filled the 52 skos-only rows across 7 tranches (93 → 145), 0 token fabricated
- **222 / 223 Virtues** mirrored (#499 Option A "resisted attack"): 206 undercut, 13 undermine, 3 rebut
- **crossLink relational layer** (#763): 8 inter-fallacy verbs (predatesOn, denounces, leverages, allows, opposes, inverts, mirrors, isRelatedTo) — 1081 cells across 844 fallacies (59.9%)
- **OWL 3-layer artefact** (#787): the canonical `argumentum.owl` now serializes skos Walton (70) + crossLink (1985) + AIF attack (145) — freshly regenerated to match prod
- Full methodology in `docs/taxonomy/498-reconciliation-p1-closure.md`; Layer C (~1263 remaining leaves) deferred to a post-tag decision

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
5. **MindMap SVGs**: All 8 languages are committed (41 SVGs, PR #565). The generation requires an attended FreePlane GUI run (`SendKeys.SendWait` desktop automation), so SVGs are regenerated on demand rather than in CI.
6. **AIF Layer C**: 145 / 1408 fallacies carry an AIF attack type (the fully-reconciled skos-only subset). The remaining ~1263 leaves have no skos signature and require a generative Walton-mapping pass — out of scope for v0.9.0, tracked for a post-tag decision.

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
| Magick.NET-Q16-AnyCPU | 14.13.1 | Stable for SVG conversion (dependabot-tracked) |
| Microsoft.Playwright | 1.43.0 | Browser automation |
| OpenAI .NET SDK | 2.10.0 | New — translation pipeline |
| System.Linq.Dynamic.Core | 1.7.2 | Security upgrade (GHSA-4cv2-4hjh-77rx) |
| SkiaSharp.NativeAssets.Win32 | 2.88.6 | QuestPDF rendering backend |

---

## Contributors

- **jsboige** — Project lead, data curation, French source validation
- **Claude (ai-01)** — Pipeline recovery, code review, visual validation, documentation, ontology integration
- **Claude (po-2023)** — Heavy pipeline builds, data quality audits, DNN readiness, translation pipeline operations
- **Claude (po-2024)** — AIF reconciliation (#498/#499), OWL regeneration (#787), release-adjacent docs

---

## Next Steps (v1.0 Roadmap)

- [ ] #131/#132 — DNN site deployment + Stripe integration
- [ ] #134 — GitHub Release with packaged assets
- [ ] #415 — `git-filter-repo` history cleanup (3-person coordinated operation)
- [ ] #212 — Full Playwright visual regression test suite
- [ ] #133 — OWL ontology publication
- [ ] RTL-aware card templates for Arabic and Farsi
- [ ] CJK font optimization for Chinese cards
