# Build modes and CMYK

> **Read on demand — not loaded by default.** Read this before touching a Debug/Release pair, or anything about print colour.
> Entry point and invariants: [`AGENTS.md`](../../AGENTS.md) · silent failures and the map of section titles: [`CLAUDE.md`](../../CLAUDE.md) · about this folder: [`README.md`](README.md).
> The sections below were moved here **verbatim** from `CLAUDE.md` on 2026-11-13 — headings and wording unchanged, so a citation such as *CLAUDE.md § Debug vs Release Builds* names a heading that exists here.
> In the table, *see #629 note below* points to *Local CardPen is mandatory for every regen…*, now in [`regeneration.md`](regeneration.md).

### Debug vs Release Builds

The pipeline uses `UseDebugParams` / `UseReleaseParams` (the `AssetConverterConfig.UseDebugParams` / `UseReleaseParams` properties) to control output quality. Convention: each config property has a `XxxDebug`/`XxxRelease` pair with a `GetXxx(config)` helper that resolves based on build mode.

| Aspect | Debug (`dotnet run`) | Release (`-c Release`) |
|--------|----------------------|------------------------|
| Print&Play image format | JPEG Q=85 (~71 MB Tarot) | PNG lossless (~222 MB) |
| Per-image CMYK conversion | Disabled (RGB) | **Disabled since #1111** (RGB — round-trip retired, see oxymore below) |
| CardPen source | Local IIS (`UseLocalCardpen=true` — default for BOTH modes) | same local IIS — ⚠️ flipping to `false` (GitHub Pages) breaks regen, see #629 note below |
| Template paths | `JsonFilePathDebug` | `JsonFilePathRelease` |
| Harvest output | Debug density directory | Release density directory |
| **PDF CMYK+OutputIntent post-process** (`PdfCmykPostProcess`, #632) | **OFF** | **OFF too** — not driven by build config, see below |

**⚠️ The CMYK post-process is NOT reached by `-c Release`.** It sits behind **two gates in series**, and only the inner one is a Debug/Release pair:

1. **Outer gate — the `Mode` flag.** The stage runs only `if (Mode.HasFlag(ConverterMode.PdfCmykPostProcess))` (the gate in `AssetConverterConfig.Apply()`). The default `Mode` is `WebBasedImageGeneration | QuestPdfGeneration` (the `AssetConverterConfig.Mode` initializer) — the flag is **absent**, and no code path derives `Mode` from the build configuration. The only place it is set is the standalone `--pdf-cmyk` entry point (the `--pdf-cmyk` handler in `Program.cs`, which assigns `cmykConfig.Mode`).
2. **Inner gate — `EnabledDebug=false` / `EnabledRelease=true`** in `PdfCmykPostProcessConfig`. This is the pair the `PdfCmykPostProcess/README.md` describes as "OFF in Debug, ON in Release" — true, but *conditional on gate 1 already being open*.

⇒ A plain `dotnet run -c Release` regeneration ships **RGB-300-lossless**, never CMYK. To get the printer bundle, run the dedicated pass **on the PDFs already generated** (it discovers them under `Target/`, converts in place, no re-harvest and no PDF regeneration — so a CMYK bundle never requires re-running the pipeline):

```bash
dotnet run -c Release --project Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj -- --pdf-cmyk
```

Ghostscript must be resolvable on `PATH`; if it is not, the stage skips every PDF **with a warning rather than crashing** — a silent-RGB failure mode, so check the log, not just the exit code.

**⚠️ CMYK oxymore (resolved by #632, round-trip retired by #1111)**: the per-image `ConvertToCmyk` (`DocumentCardSet.cs`) used to run under Release, but the image is then written as **PNG** which cannot carry CMYK — Magick re-encodes to RGB on the write, so the per-image conversion was effectively a no-op for the PNG path that still shifted pixels through the sRGB→CMYK→RGB round-trip. Since #1111, `ConvertToCmykRelease` defaults to **false** and a standard Release run writes RGB PNGs at every stage; the pixel delta vs the GO v0.9.0 bundle is covered by a fresh visual verdict on the next regen. The **authoritative CMYK path is the Ghostscript post-process** (`PdfCmykPostProcess`, new flag `ConverterMode.PdfCmykPostProcess = 1<<15`): it converts the final PDF to DeviceCMYK and embeds the SWOP OutputIntent. See `PdfCmykPostProcess/README.md`.

**Override**: Set `ForceReleaseParams = true` in JSON config to use Release params in Debug builds.

**Key files with Debug/Release pairs**: `DocumentCardSet.cs` (CMYK), `PdfManager.cs` (JPEG), `WebBasedGeneratorConfig.cs` (CardPen URL, template paths), `MindMapDocumentConfig.cs` (paths), `HarvestManager.cs` (URLs), `PdfCmykPostProcessConfig.cs` (GS post-process enable, #632).
