# Pipeline — architecture, configuration, outputs

> **Read on demand — not loaded by default.** Read this before changing the converter's configuration, a pipeline stage, the mind-map generation, or anything that reads the output tree.
> Entry point and invariants: [`AGENTS.md`](../../AGENTS.md) · silent failures and the map of section titles: [`CLAUDE.md`](../../CLAUDE.md) · about this folder: [`README.md`](README.md).
> The sections below were moved here **verbatim** from `CLAUDE.md` on 2026-11-13 — headings and wording unchanged, so a citation such as *CLAUDE.md § Pipeline Stages* names a heading that exists here.
> Re-homed, same wording: *`SkipConfigFile`* and *Critical Config Values to Verify* sat under *Historical Context & Known Issues*; they are live configuration guidance and now follow *Configuration System*.
> The other sections of *Key Technical Considerations* are in [`build-modes-and-cmyk.md`](build-modes-and-cmyk.md) and [`regeneration.md`](regeneration.md).

## Architecture

### Configuration System (Critical)

The pipeline uses a **hierarchical configuration with C# default values**:

1. `AssetConverterConfig.cs` contains the "factory default" configuration as property initializers
2. On first run, if `AssetConverterConfig.json` is missing, it's auto-generated from C# defaults
3. The JSON file can be edited to override defaults
4. Set `SkipConfigFile = true` to always use compiled defaults (ignore JSON)

**Key config sections:**
- `Mode`: Flags enum controlling which pipeline stages run (`WebBasedImageGeneration`, `QuestPdfGeneration`, `Mindmapper`, `PdfAuditor`, etc.)
- `DataSets`: CSV source definitions with paths and C# entity types
- `WebBasedGeneratorConfig.CardSets`: Card template configurations
- `CardSetDocuments`: PDF assembly definitions

### `SkipConfigFile` — deliberate `true` (NOT a regression)

`SkipConfigFile` is **deliberately `true`** (`AssetConverterConfig.SkipConfigFile`): the C# property initializers are the single source of truth, and the JSON config file is **ignored** because `List<(string,string)>` tuples (the `Translations` field) are not correctly serialized to JSON — the JSON round-trip would silently drop localization data. The JSON file is still auto-generated on first run for reference, but it is not read.

Historical note (commit `d324bd3b`, Aug 2025): `SkipConfigFile = true` was *briefly* a regression when the JSON config was the authoritative source. Since then the C# defaults were made complete and authoritative, so `true` is now correct. **Do not "fix" it to `false`** — editing C# defaults is the way to change config.

### Critical Config Values to Verify

In `WebBasedGeneratorConfig.cs`:
- `UseLocalCardpen = true` (use local IIS, not GitHub Pages)
- `LocalCardpenUrl = "http://argumentum.myia.io"` (local IIS site)
- FallaciesWeb DPI = 400 (not 72)
- Virtues `Enabled = true`
- Format A0: `NbColumns = 12`

### Pipeline Stages

```
CSV Data → [Harvesting] → PNG Images → [PDF Assembly] → Print-ready PDFs
                ↓
         [MindMapper] → SVG Mind Maps
                ↓
         [OWL Generator] → Ontologies
```

1. **Harvesting** (`HarvestManager.cs`)
   - Uses Playwright to automate Chromium
   - Loads CardPen, injects data, captures card images
   - Outputs `.harvest.json` files (cached)

2. **PDF Assembly** (`PdfManager.cs`, `PrintAndPlayDocument.cs`)
   - Uses QuestPDF library
   - **IMPORTANT**: QuestPDF is NOT thread-safe - a global lock serializes PDF generation
   - Complex manual layout logic for recto-verso printing

3. **Mind Maps** (`MindMapCreator`)
   - Generates `.mm` files (Freemind format)
   - Converts to SVG via **FreeMind GUI** (`FreeMind.exe` driven by `SendKeys` automation, resolved from `ARGUMENTUM_FREEMIND_PATH` env var). A `MindMapFormat.Freeplane` path exists in code but is non-default; the validated production path (PR #565) is FreeMind — `MindMapFormat.Freemind` is the default (`FallacyMindMapDocumentConfig.cs:32`). ⚠️ **Inventory (re-measured 05/09 on master `ceb572c8`, after #1269/#1285): the committed tree holds 43 SVG + 34 HTML wrappers + 2 root templates, NOT "20 per language"** — the historical "20 SVGs" was the file count of commit `55c6774e`, not a per-language total.

   | | Per language (×7) | `fr` | Total |
   |---|---|---|---|
   | Fallacies SVG | 3 — `Fallacies_<lang>{,.content,.links}.svg` | 3 | 24 |
   | Virtues SVG | 2 — `Argumentum_Virtues_MindMap_<lang>{.content,.links}.svg` | 2 | 16 |
   | Fallacies **cards** SVG | 0 — FR-only by config (the `Argumentum_Fallacies_MindMap_cards_fr` document in `FallacyMindMapCreatorConfig`) | 3 | 3 |
   | HTML wrappers | 4 — {Fallacies, Virtues} × {main, `_ext`} | 6 | 34 |

   Two asymmetries make a naive count go red: **Virtues ships a pair, Fallacies a triplet** (Virtues has no base `.svg`, only `.content`/`.links`), and the FR-only `cards` variant carries a **full triplet since #1269** (it was a lone file before — the viewer was the fix). The 2 root templates (`external.html`, `included.html`) sit at `Mindmaps/` root, **outside** the `<lang>/` directories: a pathspec written `Mindmaps/**/*.html` **excludes them** (34), one written `Mindmaps/*.html` includes them (36) — git glob matches `/`. Do not hardcode any of these numbers in a test: `MindmapWrapperGoldenMasterTests` **derives** the expectation from the creator configs and keeps only an anti-collapse floor.

   ⚠️ **The 34 `<lang>/` wrappers split 17/17, and only one half carries anything** (measured 2026-09-20 on `c56ec6d6`, by counting `<svg` file by file). The **17 inlining** wrappers (`Fallacies_<lang>.html`, `Argumentation_Virtues_<lang>.html`, plus FR-only `Fallacies_cards_fr.html`) embed the SVG **verbatim** — 508 KB to 5.9 MB each, and they are what a reviewer opens. The **17 `_ext`** wrappers reference it through `<object data="…">` and are ~91 KB, therefore carrying **zero** URLs, zero thumbnails, zero node text *by construction* — an instrument that counts them as defect surface will read a structural zero as a clean bill. The 2 root templates likewise. ⇒ **An anti-collapse floor belongs on the inlining subset (17), never on the total (34)**: a floor of 34 is red on a healthy tree.

   ⚠️ **A re-derivation does NOT refresh the wrappers unless a flag is explicitly forced — and the skip is silent** (measured 2026-09-20, [#1438 c.5750260550](https://github.com/ArgumentumGames/Argumentum/issues/1438#issuecomment-5750260550)). `OverwriteExistingHtmlMaps` (`AssetConverterConfig.OverwriteExistingHtmlMaps`) has **no initializer** ⇒ `false`, and since #1614 the guard lives in **one** place, shared by both pipelines (`MindMapSvgWrapperWriter.GenerateHtmlSvgWrappers`):

   ```csharp
   if (File.Exists(htmlFileName) && !config.OverwriteExistingHtmlMaps)
       Logger.Log($"Skip existing Html SVG Wrapper: {htmlFileName}");   // Log, NOT LogWarning
   ```

   Every committed wrapper exists ⇒ every one is skipped. A pass run without the named gesture repairs the SVGs and ships wrappers still inlining the **pre-repair** SVG, with only an INFO line to show for it — the same family of silent-green as the XSLT fallback below. The two `--regen-*` entry points (`--regen-fallacy-mindmap-nodes`, the only one that forces the flag — `regenConfig.OverwriteExistingHtmlMaps = true` at `Program.cs:429`; and `--regen-virtue-mindmap-html`, which never consults the guard and rewrites each wrapper itself) are the only places that touch it — but **neither is the re-derivation** — the first states in its own comment that it *"NEVER runs the normal Mindmapper pipeline"*. ⇒ On a re-derivation pass, set `OverwriteExistingHtmlMaps = true`, run in **Debug** (`TemplatePathRelease` is a `raw.githubusercontent.com/.../master/` URL — same trap as `JsonFilePathRelease`, #1225/#1228), and verify **in delta**: `grep -c "Skip existing Html SVG Wrapper"` = 0 and `grep -c "wrapper .* successfully saved"` = 34 (= **17 `content.svg` × 2 wrappers each** — the config rewrites the 17 `_ext` too, same loop, which the writer walks over both `HtmlWrappers` lists (`FallacyMindMapCreatorConfig` main + `cards`, `VirtueMindMapCreatorConfig`); this 34 is the **rewrite** surface, not the defect surface forbidden above). `MindmapWrapperFreshnessGateTests` (#1446) catches the skip *on the tree* afterwards, by flagging any inlining wrapper committed older than the `.content.svg` it embeds.

   ⚠️ Corollary for any conclusion drawn from a config default: **a default describes what is possible, not what is practised.** Reading `false` here yields "the wrappers are never rewritten" — which the artefact history refutes (`4bd53c81` touches 9 `.svg` + 3 `.html`; `f5acc7be` 27 + 9 — one inlining wrapper per language touched). Confront a default with `git show --name-only` on the commits that performed the gesture before concluding.

   If `FreeMind.exe` is not found, `TryFreeMindSvgExportCore` logs a warning and skips GUI export → falls back to XSLT (dead, #184) → DoD SHA-diff fails **silently**
   - **WARNING**: SVG post-processing uses fragile heuristics ("disambiguation") dependent on FreeMind's output structure

### CardPen (Custom Fork)

Located in `Generation/CardPen/`. Key customizations:
- Custom Handlebars helpers: `ifCond`, `each`, `markdown`
- Support for both Mustache and Handlebars modes (`useMustache` flag)
- Markdown rendering via marked.js
- Template files are JSON with embedded `mustache` key containing HTML/CSS

## Project Structure

```
Cards/                          # Source data (CSV files)
├── Fallacies/                  # Fallacy taxonomy + card assets
├── Scenarii/                   # Game scenarios
├── Rules/                      # Game rules (CSV + Markdown)
└── Memo/                       # Memo cards

Generation/
├── CardPen/                    # Customized HTML card renderer
│   └── js/main.js              # Main CardPen logic
├── Converters/
│   ├── Argumentum.AssetConverter/     # Main pipeline
│   │   ├── Entities/                  # CSV entity classes (Fallacy, Scenario, Rule, etc.)
│   │   ├── WebBasedGenerator/         # Harvesting & PDF generation
│   │   │   ├── HarvestManager.cs      # Playwright orchestration
│   │   │   ├── PdfManager.cs          # PDF assembly
│   │   │   └── Cardpen/               # CardSet configurations
│   │   ├── Mindmapper/                # Mind map generation
│   │   ├── Ontology/                  # OWL generation
│   │   └── Tests/                     # Validation modules
│   └── Argumentum.AssetConverter.Tests/  # Unit tests (xUnit)

docs/
├── sddd/                       # SDDD methodology documentation
└── investigations/             # Debug/archaeology reports
```

## Key Technical Considerations

### CsvHelper Mapping
CSV parsing uses CsvHelper. Entity classes in `Entities/` have inner `ClassMap` classes. Common issues:
- Missing `.Optional()` on nullable columns causes silent failures
- Column name mismatches result in 0 records loaded

### Thread Safety
- QuestPDF PDF generation has a global `lock` - cannot parallelize
- Playwright harvesting uses a page pool (`ConcurrentStack<IPage>`)
- `Logger` is thread-safe (uses locks)

### Known Fragile Areas
1. SVG disambiguation in mind map generation
2. Manual PDF layout calculations in `PrintAndPlayDocument.cs`
3. CardPen Handlebars/Markdown rendering when data contains special characters

## Multilingual Support

Languages: French (default), English, Russian, Portuguese, Spanish, Arabic, Farsi, Chinese (8 languages)

Localization is handled via `LocalizationConfig` in the main config:
- `CardSetLocalizations`: Field mappings per language
- `MindMapLocalization`: Mind map field translations

CSV fields carry a language suffix, **named differently in each file**: `text_fr` … `text_zh` in Fallacies, `title_fr` … `title_zh` in Virtues, `Text`, `Text_en` … `Text_zh` in Rules, `titre` / `title` / `title_ru` … `title_zh` in Scenarii. Read the header, do not guess (`AGENTS.md` §4).

## Output Directories

Generated files go to the **converter's build output**, not the repository root (`<repoRoot>/Target/` does not exist — a test anchored there is red on every machine, cf. #1072):

```
Generation/Converters/Argumentum.AssetConverter/bin/{Debug|Release}/net9.0-windows/Target/
└── {lang}/                             # ar en es fa fr pt ru zh
    ├── Documents/                      # Final PDFs
    ├── Harvest/                        # Cached .harvest.json files
    └── Images/
        ├── density-{n}/                # n = density index (0 in practice)
        │   ├── Fallacies/              # 175 × …_face.png + card_001.png (the single shared back — per language)
        │   ├── Fallacies-Web/
        │   ├── Fallacies-Print&Play/
        │   ├── Memo/  Rules/  …        # one directory per CardSet
        └── original/                   # pre-resize source PNGs (Fallacies-Web)
```

Two consequences that have each already cost a defect: **`Images/` is under `{lang}/`**, and the card PNGs are **two levels below it** (`density-{n}/<CardSet>/`) — enumerating `Images/` with `TopDirectoryOnly` can only ever see the `density-*` directories, never a PNG. Debug and Release hold **independent** trees: a regeneration run in Release (the CMYK/print path) leaves the Debug tree stale, so anything reading artefacts must pick by modification time rather than assume a configuration.
