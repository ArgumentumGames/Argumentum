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
   - Converts to SVG via **FreeMind GUI** (`FreeMind.exe` driven by `SendKeys` automation, resolved from `ARGUMENTUM_FREEMIND_PATH` env var). A `MindMapFormat.Freeplane` path exists in code but is non-default; the validated production path (PR #565) is FreeMind — `MindMapFormat.Freemind` is the default (`FallacyMindMapDocumentConfig.cs:32`). ⚠️ **Inventory (measured 23/08, #830 QA): the committed tree holds 41 SVG total (5/lang + 6 for `fr` — the `cards` variant is FR-only by config, `FallacyMindMapCreatorConfig.cs:106`) + 32 HTML wrappers (2 mindmaps × {main, `_ext`} × 8), NOT "20 per language"** — the historical "20 SVGs" was the file count of commit `55c6774e`, not a per-language total. If `FreeMind.exe` is not found, `TryFreeMindSvgExportCore` logs a warning and skips GUI export → falls back to XSLT (dead, #184) → DoD SHA-diff fails **silently**
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

### Debug vs Release Builds

The pipeline uses `UseDebugParams` / `UseReleaseParams` (in `AssetConverterConfig.cs:363-376`) to control output quality. Convention: each config property has a `XxxDebug`/`XxxRelease` pair with a `GetXxx(config)` helper that resolves based on build mode.

| Aspect | Debug (`dotnet run`) | Release (`-c Release`) |
|--------|----------------------|------------------------|
| Print&Play image format | JPEG Q=85 (~71 MB Tarot) | PNG lossless (~222 MB) |
| Per-image CMYK conversion | Disabled (RGB) | Enabled (but see oxymore below) |
| CardPen source | Local IIS (`UseLocalCardpen=true` — default for BOTH modes) | same local IIS — ⚠️ flipping to `false` (GitHub Pages) breaks regen, see #629 note below |
| Template paths | `JsonFilePathDebug` | `JsonFilePathRelease` |
| Harvest output | Debug density directory | Release density directory |
| **PDF CMYK+OutputIntent post-process** (`PdfCmykPostProcess`, #632) | **OFF** | **OFF too** — not driven by build config, see below |

**⚠️ The CMYK post-process is NOT reached by `-c Release`.** It sits behind **two gates in series**, and only the inner one is a Debug/Release pair:

1. **Outer gate — the `Mode` flag.** The stage runs only `if (Mode.HasFlag(ConverterMode.PdfCmykPostProcess))` (`AssetConverterConfig.cs:644`). The default `Mode` is `WebBasedImageGeneration | QuestPdfGeneration` (`AssetConverterConfig.cs:37`) — the flag is **absent**, and no code path derives `Mode` from the build configuration. The only place it is set is the standalone `--pdf-cmyk` entry point (`Program.cs:391`).
2. **Inner gate — `EnabledDebug=false` / `EnabledRelease=true`** in `PdfCmykPostProcessConfig`. This is the pair the `PdfCmykPostProcess/README.md` describes as "OFF in Debug, ON in Release" — true, but *conditional on gate 1 already being open*.

⇒ A plain `dotnet run -c Release` regeneration ships **RGB-300-lossless**, never CMYK. To get the printer bundle, run the dedicated pass **on the PDFs already generated** (it discovers them under `Target/`, converts in place, no re-harvest and no PDF regeneration — so a CMYK bundle never requires re-running the pipeline):

```bash
dotnet run -c Release --project Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj -- --pdf-cmyk
```

Ghostscript must be resolvable on `PATH`; if it is not, the stage skips every PDF **with a warning rather than crashing** — a silent-RGB failure mode, so check the log, not just the exit code.

**⚠️ CMYK oxymore (resolved by #632)**: the per-image `ConvertToCmyk` (`DocumentCardSet.cs`) runs under Release, but the image is then written as **PNG** which cannot carry CMYK — Magick re-encodes to RGB on the write, so the per-image conversion is effectively a no-op for the PNG path. The bundle therefore ships **RGB-300-lossless** (FlateDecode, 0 DeviceCMYK — verified via `pdfimages -list`). The **authoritative CMYK path is the Ghostscript post-process** (`PdfCmykPostProcess`, new flag `ConverterMode.PdfCmykPostProcess = 1<<15`): it converts the final PDF to DeviceCMYK and embeds the SWOP OutputIntent. See `PdfCmykPostProcess/README.md`.

**Override**: Set `ForceReleaseParams = true` in JSON config to use Release params in Debug builds.

**Key files with Debug/Release pairs**: `DocumentCardSet.cs` (CMYK), `PdfManager.cs` (JPEG), `WebBasedGeneratorConfig.cs` (CardPen URL, template paths), `MindMapDocumentConfig.cs` (paths), `HarvestManager.cs` (URLs), `PdfCmykPostProcessConfig.cs` (GS post-process enable, #632).

### Known Fragile Areas
1. SVG disambiguation in mind map generation
2. Manual PDF layout calculations in `PrintAndPlayDocument.cs`
3. CardPen Handlebars/Markdown rendering when data contains special characters

### Regeneration runs — launch shell and path length (#1179, second bite of #1121)

Pipeline runs must launch from the **short junction `D:\A1114`** (→ `.prep-1114-worktree`), and only from **PowerShell or cmd**. Launched from **Git-Bash/MSYS2, the junction is resolved at process spawn**: the child's working directory becomes the full worktree path (~30 chars longer), and image writes whose path crosses the ImageMagick native buffer (MAX_PATH = 260) fail as `MagickCoderErrorException: WriteBlob Failed`. Two "identical" invocations therefore do NOT have the same effective path length — the shell is part of the repro. `ImageHelper.EnsurePathWithinLimit` now fails the run at 250 chars (before the native failure), naming the path and its length. Related hardening (#1179): a document×language couple producing zero images fails the run instead of silently skipping PDF generation (the #1177 defect), and the logger archives the previous run's `file_logger.log` to `file_logger-<timestamp>.log` instead of deleting it — the CMYK pass no longer erases the generation log.

### Local CardPen is mandatory for every regen, Debug AND Release (#629, option 3)

`UseLocalCardpen` (`WebBasedGeneratorConfig.cs:84`) is a **single flag defaulting to `true` for both build modes** — it is NOT a Debug/Release pair (the table row above says so explicitly). Keep it `true`. GitHub Pages publishes **only the CardPen site**, not the repo's `/Cards/` tree: a Release run with `UseLocalCardpen=false` resolves Scenarii/asset URLs to `argumentumgames.github.io/Cards/` → **HTTP 404 → 0 images → 0 PDF** (silent set failure, discovered 2026-07-01). The workaround — keep the default `true` — was validated 01/07 (64 PDFs complete) and has been in force on every regen since (22/08, 28/08). The durable fix (Option 1: absolute raw-master URLs, PR #666) is HOLD post-tag.

### A branch fix to a card template is NOT verifiable by a Release render (#1225/#1228)

`JsonFilePathRelease` is an **absolute URL pinned to `master`** — e.g. Rules resolves to `https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Rules/Argumentum_Rules_fr.json` (`WebBasedGeneratorConfig.cs:110`). `CardSetInfo.GetJsonFilePath` picks it whenever `UseDebugParams` is false, so **a Release run downloads the template from master and never reads the worktree**. A template fix living on a feature branch is therefore invisible to a Release render, no matter which branch is checked out.

The failure mode is silent and looks like non-determinism: the render is byte-identical across runs (same code, same master template), the fix "does not take", and a cache-clobber changes nothing. Discovered 2026-09-01 after three identical deliveries on #1228 — the discriminator is already in the log, two lines per CardSet:

```
[CardSetInfo] JsonFilePathDebug='…', JsonFilePathRelease='…'
[CardSetInfo] UseDebugParams=<bool>, Selected path: '<path>'
```

**Validate template fixes in Debug** (local path → worktree), or merge first and validate after. Demanding a Release proof before merge asks for the fix to be on master before it is on master.

Corollary for reviewers: the CSS cascade itself can be measured without the pipeline — extract the `css` and `mustache` keys from the template JSON, apply the language-class rewrite the pipeline performs (`ARGU_LANG_MARKER`, `AssetConverterConfig.cs`), reconstruct the markdown render, and read `getComputedStyle` in a browser. That verdict is independent of which template the pipeline happens to load.

## Multilingual Support

Languages: French (default), English, Russian, Portuguese, Spanish, Arabic, Farsi, Chinese (8 languages)

Localization is handled via `LocalizationConfig` in the main config:
- `CardSetLocalizations`: Field mappings per language
- `MindMapLocalization`: Mind map field translations

CSV fields use language suffixes: `Title`, `Title_en`, `Title_ru`, `Title_pt`, `Title_es`, `Title_ar`, `Title_fa`, `Title_zh`

## Output Directories

Generated files go to the **converter's build output**, not the repository root (`<repoRoot>/Target/` does not exist — a test anchored there is red on every machine, cf. #1072):

```
Generation/Converters/Argumentum.AssetConverter/bin/{Debug|Release}/net9.0-windows/Target/
└── {lang}/                             # ar en es fa fr pt ru zh
    ├── Documents/                      # Final PDFs
    ├── Harvest/                        # Cached .harvest.json files
    └── Images/
        ├── density-{n}/                # n = density index (0 in practice)
        │   ├── Fallacies/              # 176 × …_face.png + card_001.png (the single shared back)
        │   ├── Fallacies-Web/
        │   ├── Fallacies-Print&Play/
        │   ├── Memo/  Rules/  …        # one directory per CardSet
        └── original/                   # pre-resize source PNGs (Fallacies-Web)
```

Two consequences that have each already cost a defect: **`Images/` is under `{lang}/`**, and the card PNGs are **two levels below it** (`density-{n}/<CardSet>/`) — enumerating `Images/` with `TopDirectoryOnly` can only ever see the `density-*` directories, never a PNG. Debug and Release hold **independent** trees: a regeneration run in Release (the CMYK/print path) leaves the Debug tree stale, so anything reading artefacts must pick by modification time rather than assume a configuration.

## Historical Context & Known Issues

### Golden Master: April 2024 (Commit `0087f0ec`)

The pipeline worked correctly before May 2025. A series of "vibecodés" commits introduced regressions.

### `SkipConfigFile` — deliberate `true` (NOT a regression)

`SkipConfigFile` is **deliberately `true`** in `AssetConverterConfig.cs` (line 34): the C# property initializers are the single source of truth, and the JSON config file is **ignored** because `List<(string,string)>` tuples (the `Translations` field) are not correctly serialized to JSON — the JSON round-trip would silently drop localization data. The JSON file is still auto-generated on first run for reference, but it is not read.

Historical note (commit `d324bd3b`, Aug 2025): `SkipConfigFile = true` was *briefly* a regression when the JSON config was the authoritative source. Since then the C# defaults were made complete and authoritative, so `true` is now correct. **Do not "fix" it to `false`** — editing C# defaults is the way to change config.

### Stable Dependency Versions

| Package | Version | Notes |
|---------|---------|-------|
| QuestPDF | 2022.12.12 | **Licence-pinned** — MIT free license; also thread-safe issues above this |
| Magick.NET-Q16-AnyCPU | 14.15.0 | Image processing (per-image CMYK conversion is a no-op for PNG output; CMYK for print is applied via Ghostscript post-process on the final PDFs, see #632). Bumped from 14.14.0 via #871 (2026-07-25): 14.15.0 is the declared first-patched version for 5 advisories (4 medium + 1 low) that had accumulated against 14.14.0 — licence unchanged (Apache-2.0) |
| AutoMapper | 14.0.0 | **Licence-pinned — do NOT bump.** 14.0.0 is the last MIT release; 15.0.0+ is RPL-1.5 / commercial dual-licensed (Lucky Penny), `requireLicenseAcceptance: true`. Decision jsboige 2026-06-23, implemented in #588 (`6caf5833`): stay MIT-pure + targeted `NuGetAuditSuppress` for GHSA-rvv3-g6hj-g44x + `MaxDepth(1)` guard in `Entities/MappingProfile.cs` (the only Profile is flat and acyclic, so the vulnerable recursion path is unreachable). No patched 14.x exists. Dependabot re-proposes 15.x periodically — close it (see #887) |
| SkiaSharp.NativeAssets.Win32 | 2.88.6 | Required for QuestPDF |
| Microsoft.Playwright | 1.43.0 | Browser automation |

> **NuGet audit warnings surface on `restore`, not on incremental `build`.** An incremental `dotnet build` can report 0 warnings while `dotnet restore --force` reports dozens of `NU1901`/`NU1902` advisories. To check the zero-warning invariant (#587) honestly, force a restore. Advisory sets drift over time against a pinned version, so this can go red without any code change.

### Applied Corrections (Oct-Dec 2025)

1. **HarvestManager.cs - Timeout**: Restored from 60s to 120s (CardPen needs 90-120s)
2. **HarvestManager.cs - frame.js**: Removed manual injection (CardPen loads it)
3. **HarvestManager.cs - generateImages()**: Added explicit call (no UI click in automation)
4. **PdfManager.cs - Lock**: Global lock on QuestPDF (NOT thread-safe)
5. **WebBasedGeneratorConfig.cs - CardSet Memo**: Reintegrated (critical for Print&Play Tarot)
6. **PdfManager.cs - Issue #119**: Rules cards positioning (lines 43-72) - preserves CardSet order so Rules appear first in TarotCards PDFs

### Known CSV Mapping Issues — ALL RESOLVED ✅

| File | Original Issue | Resolution |
|------|---------------|------------|
| `Rule.cs` | `GetId()` returned `string.Empty` | Now returns `Rules_01`, `Rules_02` etc. via RowIndex |
| `RuleClassMap.cs` | `print_and_play` not mapped | Mapped with `.Optional()` to `PrintAndPlay` property |
| `ArgumentVirtueClassMap.cs` | `Id` not mapped | Uses `Pk` property mapped to CSV `pk` column |
| `CsvBase.cs` | `MissingFieldFound = null` | Now logs warnings via `Logger.Log()` instead of silent |

### Critical Config Values to Verify

In `WebBasedGeneratorConfig.cs`:
- `UseLocalCardpen = true` (use local IIS, not GitHub Pages)
- `LocalCardpenUrl = "http://argumentum.myia.io"` (local IIS site)
- FallaciesWeb DPI = 400 (not 72)
- Virtues `Enabled = true`
- Format A0: `NbColumns = 12`

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
| Harvest vide + Dpi=0 | CsvType manquant dans DataSet | Ajouter `CsvType = typeof(Entity)` |
| Images blanches/vides | Chemins assets relatifs | Réécrire en URLs absolues |
| `data:,` (empty dataUrl) | Timeout ou erreur fonts | Augmenter timeout à 120s |
| Mismatch image count | rscount mal calculé | Vérifier formule expectedImageCount |

### CsvType et DataSets

**CRITIQUE**: Chaque DataSet doit avoir un `CsvType` défini dans `AssetConverterConfig.cs` pour que le harvesting fonctionne.

```csharp
// ✅ CORRECT - CsvType défini
new DataSetInfo() {
    Name = KnownDataSets.Scenarii,
    CsvType = typeof(Scenario),  // ← OBLIGATOIRE
    DebugFilePath = @"..\..\Cards\Scenarii\..."
}

// ❌ INCORRECT - génère harvest vide
new DataSetInfo() {
    Name = KnownDataSets.Scenarii,
    // CsvType manquant → early return dans HarvestManager ligne ~494
    DebugFilePath = @"..\..\Cards\Scenarii\..."
}
```

### Dimensions FallaciesWeb A0

Pour tenir sur 1 page A0 (841×1189mm):
- HeightMM = 69mm (pas 72mm)
- WidthMM = 69mm
- NbColumns = 12
- Padding = 2mm

### RowsetNb vs rscount (CRITIQUE)

**Règle**: Si le template JSON attend 1 ligne CSV par carte (variables simples comme `{{titre}}`), ne PAS définir `RowsetNb` dans la config C#.

```csharp
// ✅ CORRECT - template avec rscount=1 utilise 1 ligne par carte
FaceCardSetInfo = new CardSetInfo()
{
    DataSet = KnownDataSets.Scenarii,
    JsonFilePathDebug = @"...\Argumentum_Scenarii_Face_fr.json",
    // RowsetNb non défini → utilise rscount du template JSON
}

// ❌ INCORRECT - force 14 lignes par carte mais template attend 1 ligne
FaceCardSetInfo = new CardSetInfo()
{
    ...
    RowsetNb = 14  // Casse le template !
}
```

**Symptôme si RowsetNb incorrect**: Cartes générées avec contenu vide (seuls éléments statiques visibles).

### CSV Injection dans CardPen (CRITIQUE)

**Règle absolue**: NE JAMAIS modifier le contenu CSV avant injection dans CardPen.

```csharp
// ✅ CORRECT - Golden Master (avril 2024)
cardSetDocumentWrapper.CardSetDocument.csv = csvContent;

// ❌ INCORRECT - Casse le parsing PapaParse
cardSetDocumentWrapper.CardSetDocument.csv = csvContent.Replace("\n", "\\n");
```

**Pourquoi**: PapaParse gère correctement les newlines dans les cellules CSV entre guillemets.
L'échappement transforme les vrais newlines en chaînes littérales "\\n", cassant le parsing.

**Symptôme**: Cartes générées avec contenu vide (seules les icônes statiques visibles).

### Classes CSS Familles (Virtues/Fallacies)

La classe racine de la carte est **la valeur de la colonne CSV** désignée par `cardClass` dans le template — `family_fr_camelcase` pour Virtues, `Famille_camelCase` pour Fallacies. Chaque valeur doit avoir sa règle `card.<valeur>` dans la clé `css` du template JSON (⚠️ pas dans `mustache`, qui ne contient que le HTML).

Liste complète pour Virtues, **vérifiée contre `Argumentum Virtues - Taxonomy.csv` + la clé `css` du template** (master `c99fc4b3`, 2026-08-17) — 8 familles, 223 nœuds :

| Classe CSS (= `family_fr_camelcase`) | Famille (`family_fr`) | Nœuds | Couleur | Alias hérités encore déclarés |
|------------|---------|---:|---------|---------|
| `argumentValable` | Argument valable (racine) | 1 | Gris #555555 | `argumentsVertueux` |
| `argumentPertinent` | Argument pertinent | 33 | Violet #811da3 | — |
| `présentationIntègre` | Présentation intègre | 25 | Rose #ff66eb | — |
| `sensQuantitatif` | Sens quantitatif | 20 | Turquoise #08af93 | `rigueurMathématique`, `exactitudeMathématique` |
| `inférenceMaîtrisée` | Inférence maîtrisée | 55 | Vert #8dc801 | `raisonnementValide` |
| `justesseLexicale` | Justesse lexicale | 18 | Bleu #0054a4 | `langageExact`, `langageRigoureux` |
| `honnêtetéIntellectuelle` | Honnêteté intellectuelle | 27 | Jaune #ffc307ff | — |
| `échangeEnrichissant` | Échange enrichissant | 44 | Rouge #dc0f0a | `débatRespectueux` |

> ⚠️ **Trois familles ont été renommées les 6-7 août 2026** (#981/#982a → `6d22f79a`, #998 → `5631bf3c`, #1002 → `19b9c9d1`) : `Raisonnement valide` → **`Inférence maîtrisée`**, `Rigueur mathématique` → **`Sens quantitatif`**, `Langage exact` → **`Justesse lexicale`**. Les anciens noms **survivent partout dans les artefacts figés** — bodies d'issues, rapports, commentaires. Une recherche sur un ancien nom rend **0 résultat** dans le CSV, ce qui se lit à tort comme « rien à faire » : c'est une panne d'instrument, pas une absence. **Toujours re-mesurer le libellé courant contre le CSV avant d'agir sur une famille.** (Le dossier #985 a ainsi coûté un cycle : son tableau porte encore l'ancien nom *et* des comptes de cartes périmés.)

**Renommage additif** — les renommages ci-dessus ont été faits selon ce motif, et le template le conserve : tous les noms successifs cohabitent **dans le même bloc**, séparés par des virgules (`card.exactitudeMathématique, card.rigueurMathématique, card.sensQuantitatif { … }` — trois générations). Pour renommer une famille : ajouter le nouveau nom au bloc existant **d'abord**, renommer le CSV **ensuite**. L'ordre inverse produit des cartes sans couleur de famille entre les deux merges.

**Second référentiel à synchroniser** — les glossaires en dur des prompts `DatasetUpdater/Resources/` portent aussi les noms de familles : un renommage CSV non répercuté y est **ré-injecté à la passe de traduction suivante**. Les 8 `PromptVirtues*User.txt` sont additifs (ancien + nouveau) et donc corrects ; vérifier ce point à chaque renommage.

**Symptôme si classe manquante**: Carte avec fond blanc au lieu de la couleur de famille.

## Pipeline Recovery Status (Mars 2026)

### Validation Multilingue - 17 Mars 2026 (dimensions OK, contenu CASSÉ)

**P1 - Pipeline multilingue validé en DIMENSIONS uniquement** : 79 PDFs générés, 4209 images, dimensions correctes.

| Langue | PDFs | Images | Status |
|--------|------|--------|--------|
| FR (Français) | 18 | 620 | ✅ Golden Master |
| EN (English) | 22 | 1781 | ⚠ Structure OK, contenu FR (voir #216) |
| RU (Русский) | 17 | 1270 | ⚠ Structure OK, contenu FR (voir #216) |
| PT (Português) | 22 | 538 | ⚠ Structure OK, contenu FR (voir #216) |

**Issue #119 validée** : Rules cards apparaissent en premier dans tous les TarotCards multilingues.

**Bug #216 découvert 2026-04-22 + corrigé 2026-04-23** : `LocalizationConfig.FrontFieldConversions` pour Fallacies référençait des noms de champs (`Titre`, `Definition`, `Exemple`, `Contre-Exemple`) qui n'existaient PAS dans les templates Mustache (qui utilisent `{{text_fr}}`, `{{desc_fr}}`, `{{example_fr}}`, `{{Famille}}`, `{{Sous-Famille}}`, `{{Soussousfamille}}`). Résultat : `template.Replace()` ne trouvait rien à remplacer → tous les PDFs EN/RU/PT contenaient du contenu français. Corrigé en restaurant le mapping Golden Master (avril 2024) + ajout de Rules (absent) + tests de régression `FallaciesLocalizationTests`. **Regénération pipeline complète requise** pour produire les PDFs réellement multilingues.

**Mise à jour cycle 56+59 (mai 2026)** : Bug #216 corrigé avril 2026, pipeline régen post-merges #301/#302 auditée cycle 56 (Stratégie D recommandée : spot-check 5 cartes pixel diff ~15 min avant décision régen complète A ou aucune C). Tableau ci-dessus reste snapshot 17 mars 2026 (pré-fix).

### État actuel par CardSet — **mesuré 2026-08-28 sur l'arbre Release** (régén du 22/08), identique sur **les 8 langues**

⚠️ Les trois chiffres barrés ci-dessous étaient périmés dans ce fichier et auraient produit des devis d'impression faux (#1187). Un devis se chiffre **à la carte** : ne pas citer ce tableau sans re-mesurer.

| CardSet | Faces | Dos | Images | PDFs |
|---------|------:|----:|-------:|------|
| Fallacies Tarot | 176 | 1 | 177 | TarotCards_{lang}-1/2.pdf |
| FallaciesWeb | 176 | — | 176 | A0, A4, Thumbnails |
| Rules Tarot | **15** ~~24~~ | — | 15 | dans TarotCards — réduit par #438 |
| Memo Tarot | 1 | 1 | 2 | dans TarotCards |
| Scenarii Poker | **167** ~~97~~ | **7** | 174 | PokerCards_{lang}-1.pdf |
| Virtues Tarot | **131** ~~113~~ | 1 | 132 | TarotCards_Virtues_{lang}-FacesOnly.pdf |

**Totaux fabrication** (#1187, une boîte par langue) : deck **Tarot = 192 cartes** (176 + 15 + 1) · deck **Scenarii = 167 cartes** · boîte **sans** Virtues = **359 cartes**, **avec** = **490**.

⚠️ **Le deck Scenarii a 7 dos distincts**, un par catégorie (`histoire`, `mythologie`, `politique`, `pop_culture`, `relation_intime`, `vie_personnelle`, `vie_professionnelle`) — contrairement au Tarot qui partage **un** dos. C'est une contrainte de façonnage, pas un détail d'asset.

⚠️ **L'arbre Debug est périmé** (Rules 24 = pré-#438, Virtues 1) : Debug et Release sont indépendants — mesurer sur celui dont la date correspond, jamais « le premier trouvé ».

### Mind Maps & SVGs — April 2026 ✅ COMPLETE

- FreeMind Batik SVGs generated and committed across all 8 languages (FR/EN/RU/PT + ES/AR/FA/ZH, PR #565)
- FreeMind GUI automation via `SendKeys.SendWait` — VALIDATED (commit `46d6cd9b`)
- Issues #127, #128, #129 closed
- OWL ontology with SKOS — committed (PR #161), issue #130 closed

### Translation Pipeline (DatasetUpdater)

- **SDK**: Official OpenAI .NET SDK v2.10.0 (PR #210 merged)
- **Models**: `gpt-5.5` (EN translations primary, PR #302), `gpt-4.1`/`gpt-4.1-mini` (fallback + RU/PT)
- **Multi-provider**: API support added via PR #302 (OpenAI + alternative providers configurable)
- **Config**: `DatasetUpdater/DatasetUpdaterRootConfig.cs` — 7 task configs (all `Enabled = false`)
- **Prompts**: 29 files in `DatasetUpdater/Resources/`
- **Function calling**: Manual `FunctionToolDef` + JSON schema + `BinaryData.FromString()`
- **Virtues CSV**: 100% translated (title/description/remark × fr/en/ru/pt), via PRs #218, #236, #246, #290, #295
- **Issue #183** DONE — merged via PR #210

### GSheet ↔ CSV Sync (PR #200 merged)

- **Module**: `GSheetSync/` (9 files: CsvDiffEngine, DiffReport, SyncSafetyChecker, Auth, Service, Runner, configs)
- **Mode flag**: `ConverterMode.GSheetSync = 1 << 14` (16384)
- **Safety**: 6-layer upload protection (dry-run, diff, thresholds, confirmation, backup, verify)
- **4 spreadsheet configs**: Fallacies, Scenarii, Virtues, Rules (all `Enabled = false`)
- **Pending**: OAuth credentials for end-to-end testing
- **Tests**: 77 pass / 0 fail / 1 skip (includes CsvDiffEngine, SyncSafetyChecker, DiffReport, CsvToGrid tests)

### Test Coverage (July 2026)

- **578 tests pass** (`dotnet test` on `Argumentum.AssetConverter.Tests`, 2026-07-05, .NET 9 — 584 total: 578 pass / 1 fail / 5 skip), 5 skips (GUI/infrastructure), 1 known-fail (`OwlE2EGenerationValidationTests.LoadedOntology_RdfTypeAndInScheme_DroppedByOwl2XmlRoundTrip` — OWLSharp round-trip bug, pre-existing, tracked #133 — does not affect generated assets)
- Coverage includes: CsvDiffEngine, SyncSafetyChecker, DiffReport, CsvToGrid, MindMapHtmlWrapper, FallaciesLocalizationTests, TaxonomyValidationTests, Memo_Back localization, Playwright visual tests
- Build is zero-warning (CS compiler warnings + NuGet audit, #587)
- Issue #212 tracks Playwright visual regression tests for generated PDFs

### Prochaines étapes

1. ~~Valider génération images après corrections CSV/CSS~~ FAIT
2. ~~Tester génération PDFs (QuestPDF)~~ FAIT
3. ~~Activer et tester génération multilingue~~ FAIT (17 Mars 2026)
4. ~~Valider formats: Tarot, Poker, A0, Print&Play~~ FAIT
5. ~~Mind Maps + SVGs (Batik)~~ FAIT (6 Avril 2026)
6. ~~#183 — Upgrade SDK traduction~~ FAIT (PR #210 merged, avril 2026)
7. ~~#193 — GSheet ↔ CSV sync~~ FAIT (PR #200 merged, avril 2026)
8. ~~#202 Phase 1 — CSV text micro-fixes~~ FAIT (PR #203 + #213 merged)
9. Valider DatasetUpdater round-trip avec OpenAI API (3-5 records, Enabled=true) — smoke test gpt-5.5 en cours (po-2023, cycle 61bis)
10. ~~#211 — Retraduction PT Rules~~ ✅ DONE (closed 2026-05-17), Rules PT 100% sauf row 1 cover (fix PR #306 cycle 47)
11. #212 — Playwright visual regression tests pour PDFs générés
12. ~~Virtues i18n — ajouter colonnes _en/_ru/_pt~~ ✅ DONE (April-May 2026, PRs #218/#236/#246/#290/#295) — 100% coverage title/description/remark
13. ~~Scenarii EN/RU/PT — 76/167 records missing (~46%)~~ ✅ DONE — 167/167 records 100% covered EN/RU/PT (verified cell-by-cell on master `7206f2f9`, 2026-05-24) across all 8 translatable fields; filled via commits `7ed970a3` (EN), `2a1b86bf` (RU), `0dc838fb` (PT) + contamination/BOM fixes
14. #134 — GitHub Release v0.9.0 (en attente validation docs)
15. #133 — Publication OWL
16. #131/#132 — DNN site + déploiement

### Commits clés de la recovery

| Commit | Description |
|--------|-------------|
| `37600e4a` | fix(harvest): restore Golden Master CSV injection |
| `f0b1cd35` | fix(templates): add argumentsVertueux CSS class |
| `09b427ef` | fix(templates): Scenarii asset paths to GitHub URLs |
| `30483257` | fix(templates): Virtues CSS and Rules naming |
| `9b19d5e8` | fix(config): remove RowsetNb=14 for Scenarii CardSet |
| `75a049d3` | fix(mindmap): restore validated FreeMind SendKeys automation |
| `55c6774e` | feat(assets): replace XSLT SVGs with FreeMind Batik SVGs |
| `fd2aef10` | feat(dataset-updater): migrate to official OpenAI SDK v2.10.0 (#183) |
| `e24cbd17` | fix(prompt): enable #nullable context and guard null param.Name |
| `092d4639` | Merge PR #200 — bidirectional GSheet ↔ CSV sync (#193) |

### Data Quality Issues (April-May 2026)

| Issue | Description | Status |
| ------- | ----------- | ------ |
| ~~Fallacies duplicate PKs 520, 1000~~ | Was reported during GSheet sync; **not reproducible on master** — 1408/1408 PKs unique, PK 520 & 1000 appear once each (verified `7206f2f9`, 2026-05-24). Stale warning or GSheet-view artefact | ✅ N/A |
| ~~Scenarii 54% translated~~ | 167/167 records now 100% covered EN/RU/PT, all 8 fields; substantive fields (context/issue) 0% FR-contaminated, RU 165/167 Cyrillic. Title=FR overlaps (21 EN/11 PT) = legitimate proper nouns (Sherlock, Jeanne d'Arc, Ergo sum…). Verified `7206f2f9`, 2026-05-24 | ✅ DONE |
| ~~Virtues 0% translated~~ | ✅ Resolved via PRs #218, #236, #246, #290, #295 (April-May 2026) — 100% coverage title/description/remark × 4 languages | DONE |
| PT Rules row 1 EN contamination | Rules cover showed "Liars 'School" instead of "A Escola dos Mentirosos" | ✅ Fix PR #306 cycle 47 (1 cell CSV, native PT validated po-2023) |

## Related Documentation

- [ARCHITECTURE_PIPELINE.md](Generation/Documentation/ARCHITECTURE_PIPELINE.md) - Detailed pipeline architecture
- [docs/sddd/](docs/sddd/) - SDDD methodology and investigation reports
- [docs/investigations/](docs/investigations/) - Debug/archaeology reports (37 reports + scripts)
