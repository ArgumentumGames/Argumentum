# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Argumentum is an educational card game about logical fallacies (sophisms). The repository contains:
- A .NET pipeline (`Argumentum.AssetConverter`) that transforms CSV data into print-ready PDFs, mind maps, and OWL ontologies
- A customized fork of CardPen (HTML card renderer using Mustache/Handlebars templates)
- Source data (CSV files) for fallacies taxonomy, game scenarios, rules, and virtues
- A DNN web platform export (partial, keys excluded)

## Build & Run Commands

```bash
# Run the full generation pipeline
dotnet run --project "Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj"

# Run tests (xUnit)
dotnet test "Generation/Converters/Argumentum.AssetConverter.Tests/Argumentum.AssetConverter.Tests.csproj"

# Run visual tests
dotnet test "Generation/Converters/Argumentum.AssetConverter.VisualTests/Argumentum.AssetConverter.VisualTests.csproj"

# Build all projects
dotnet build "Argumentum Converters.sln"
```

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
   - Converts to SVG via Freeplane external process
   - **WARNING**: SVG post-processing uses fragile heuristics ("disambiguation") dependent on Freeplane's output structure

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

Languages: French (default), English, Russian, Portuguese

Localization is handled via `LocalizationConfig` in the main config:
- `CardSetLocalizations`: Field mappings per language
- `MindMapLocalization`: Mind map field translations

CSV fields use language suffixes: `Title`, `Title_en`, `Title_ru`, `Title_pt`

## Output Directories

Generated files go to:
```
Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0/Target/
├── {lang}/
│   ├── Documents/              # Final PDFs
│   └── Harvest/                # Cached .harvest.json files
└── Images/                     # Generated card PNGs
```

## Historical Context & Known Issues

### Golden Master: April 2024 (Commit `0087f0ec`)

The pipeline worked correctly before May 2025. A series of "vibecodés" commits introduced regressions.

### Critical Regression - `SkipConfigFile` (Commit `d324bd3b`, Aug 2025)

**The #1 cause of pipeline failures**: If `SkipConfigFile = true` in `AssetConverterConfig.cs` (line 31), the JSON config is completely ignored, causing the pipeline to use an incomplete default config.

**ALWAYS verify**: `SkipConfigFile = false`

### Stable Dependency Versions

| Package | Version | Notes |
|---------|---------|-------|
| QuestPDF | 2022.12.12 | MIT free license, thread-safe issues above this |
| Magick.NET | 13.5.0 | SVG conversion stability |
| SkiaSharp.NativeAssets.Win32 | 2.88.6 | Required for QuestPDF |
| Microsoft.Playwright | 1.43.0 | Browser automation |

### Applied Corrections (Oct-Dec 2025)

1. **HarvestManager.cs - Timeout**: Restored from 60s to 120s (CardPen needs 90-120s)
2. **HarvestManager.cs - frame.js**: Removed manual injection (CardPen loads it)
3. **HarvestManager.cs - generateImages()**: Added explicit call (no UI click in automation)
4. **PdfManager.cs - Lock**: Global lock on QuestPDF (NOT thread-safe)
5. **WebBasedGeneratorConfig.cs - CardSet Memo**: Reintegrated (critical for Print&Play Tarot)

### Known CSV Mapping Issues

These issues were identified but may not be fully resolved:

| File | Issue | Impact |
|------|-------|--------|
| `Rule.cs` | `GetId()` returns `string.Empty` | Empty IDs everywhere |
| `RuleClassMap.cs` | `print_and_play` column not mapped | Column ignored |
| `ArgumentVirtueClassMap.cs` | `Id` not mapped | Virtues without ID |
| `CsvBase.cs` | `MissingFieldFound = null` | Silent failures |

### Critical Config Values to Verify

In `WebBasedGeneratorConfig.cs`:
- `UseLocalCardpen = true` (use local IIS, not GitHub Pages)
- `LocalCardpenUrl = "http://argumentum.myia.io"` (local IIS site)
- FallaciesWeb DPI = 400 (not 72)
- Virtues `Enabled = true`
- Format A0: `NbColumns = 11`

## Diagnostic Resources

### Investigation Reports

Located in `docs/investigations/`:
- `2025-10-15-investigation-historique-pipeline-pdf.md` - Complete regression history
- `2025-10-21-rapport-archeologie-git-final.md` - Git archaeology (conclusion: C# code is intact)
- `2025-12-13-rapport-cloture-mission.md` - Final mission report with validation metrics

### PowerShell Diagnostic Scripts

Located in `docs/investigations/scripts/`:
- `2025-10-16-XX-test-*.ps1` - Pipeline validation tests
- `2025-10-21-XX-*.ps1` - Git archaeology scripts
- `2025-10-23-01-validation-memo-restaure.ps1` - Memo CardSet validation

### Reference Files (Git Archaeology)

Located in `docs/investigations/archeologie-git/`:
- `WebBasedGeneratorConfig_REFERENCE_FUNCTIONAL_*.cs` - Known working config (20KB)
- 15 historical versions for comparison

## Golden Rule: Code = Truth

- **NEVER edit JSON directly** - Always modify C# source (single source of truth)
- **Regenerate JSON** from correct C# code to ensure consistency
- `AssetConverterConfig.json` is in `.gitignore` (generated, not tracked)

## Leçons Apprises (Pipeline Recovery 2025-2026)

### CardPen Template Configuration

| Parameter | Description | Example |
|-----------|-------------|---------|
| `rscount` | Nombre de lignes CSV groupées par carte | Memo=200 (1 carte avec toutes les données) |
| `rsstyle` | Mode de groupement | "bunch", "cycle", "random" |
| `{{cardIndex}}` | Variable auto-injectée (1-based) | Utilisable pour numérotation de page |

**Règle critique**: Ne JAMAIS forcer `rscount=0` dans le code C# - préserver la valeur du template JSON.

### Calcul du nombre d'images attendues

Avec `rsstyle="bunch"` et `rscount >= N`:
```
expectedImageCount = ceil(cardIds.Count / rscount)
```

Exemple: Memo avec 200 lignes CSV et `rscount=200` → génère 1 seule image.

### CSS Card Templates

```css
/* TOUJOURS inclure pour éviter problèmes d'affichage des caractères */
-webkit-font-variant-ligatures: no-common-ligatures;
font-variant-ligatures: no-common-ligatures;
```

**À éviter**: `flex-flow: column wrap` - cause des débordements de texte. Préférer `column nowrap` avec gestion overflow.

### domtoimage (frame.js)

Configuration Golden Master restaurée:
```javascript
const options = {
    height: height,
    width: width,
    scale: dpi / 96,      // Important pour résolution correcte
    cachedFonts: true,    // Utilise polices préchargées
    imagePlaceholder: "..." // Fallback pour images manquantes
};

// Appeler AVANT génération:
domtoimage.getFontsBefore();
```

### Chemins d'assets relatifs

Les chemins `../../Cards/...` ne fonctionnent pas avec CardPen local (IIS).

**Solutions**:
1. Réécrire en URLs absolues GitHub: `https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/...`
2. Ou configurer IIS pour servir `/Cards/` depuis le répertoire local

### Debugging Tips

| Symptôme | Cause probable | Solution |
|----------|---------------|----------|
| Harvest vide (`Images: {}`) | Erreur JS dans CardPen | Vérifier console Playwright |
| Images blanches/vides | Chemins assets relatifs | Réécrire en URLs absolues |
| `data:,` (empty dataUrl) | Timeout ou erreur fonts | Augmenter timeout à 120s |
| Mismatch image count | rscount mal calculé | Vérifier formule expectedImageCount |

### Dimensions FallaciesWeb A0

Pour tenir sur 1 page A0 (841×1189mm):
- HeightMM = 69mm (pas 72mm)
- WidthMM = 69mm
- NbColumns = 11
- Padding = 2mm

## Related Documentation

- [ARCHITECTURE_PIPELINE.md](Generation/Documentation/ARCHITECTURE_PIPELINE.md) - Detailed pipeline architecture
- [docs/sddd/](docs/sddd/) - SDDD methodology and investigation reports
- [docs/investigations/](docs/investigations/) - Debug/archaeology reports (37 reports + scripts)
