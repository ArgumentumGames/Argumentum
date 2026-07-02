# PDF CMYK + OutputIntent post-process (`PdfCmykPostProcess`) — #632

Converts the **RGB-300-lossless (FlateDecode)** PDFs produced by the QuestPDF stage into
**DeviceCMYK + OutputIntent (CGATS TR 001 / SWOP)** print-ready PDFs via a Ghostscript
post-pass. Runs as a standalone stage **after** PDF generation, so it can be invoked on an
existing bundle **without re-harvesting**.

## Why this exists

The per-image `ConvertToCmyk` pixel conversion (`ImageHelper.cs`) runs under `-c Release`, but
the image is then written as **PNG** (`DocumentConfig.ImageFormat = MagickFormat.Png`), and PNG
does not carry a CMYK profile — Magick silently re-encodes to RGB on the PNG write. QuestPDF has
no CMYK support either. So the Release bundle shipped as RGB-300-lossless, with **0 DeviceCMYK**
images (verified by ai-01 via `pdfimages -list`, 2026-07-01).

The real CMYK path is therefore a **post-process on the final PDF**, not on the source images.
Ghostscript's `pdfwrite` device converts the colorspace and embeds the OutputIntent.

## Scope (honest)

The output targets **CMYK colorspace + an OutputIntent profile**, NOT formal **PDF/X-3**
certification (no trim/bleed boxes added). This matches the print-ready objective validated in
the ai-01 POC (issue #632): `pdfimages -list` shows `0 rgb / 176 cmyk`, `OutputIntent =
GTS_PDFX / CGATS TR 001 (SWOP)`, PPI preserved, Flate lossless, ~23 s/PDF.

## Usage

Set the flag and run (Release build, after PDFs exist):

```
dotnet run -c Release --project Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj
# with AssetConverterConfig.json: "Mode": "QuestPdfGeneration, PdfCmykPostProcess"
# or standalone on an existing bundle: "Mode": "PdfCmykPostProcess"
```

Defaults: **OFF in Debug**, ON in Release (`EnabledDebug=false`, `EnabledRelease=true`).

## Configuration

| Property | Default | Notes |
|----------|---------|-------|
| `IsEnabled` | `true` | Master toggle. |
| `EnabledDebug` | `false` | OFF in Debug (preview-only). |
| `EnabledRelease` | `true` | ON in Release (printer quality). |
| `GhostscriptPath` | `"gswin64c"` | Binary name (PATH) or absolute path. Stage skips with a warning if absent. |
| `IccProfilePath` | `null` | Custom ICC. When null, extracted at runtime from `ImageMagick.ColorProfiles.USWebCoatedSWOP` — the same profile as the per-image `ConvertToCmyk` conversion, so the OutputIntent is color-consistent with the source pipeline (zero new licensing). |
| `TimeoutSeconds` | `180` | Per-PDF Ghostscript timeout. |

## Ghostscript install (no admin)

The official NSIS installer requires UAC elevation. A user-scope install works via conda-forge:

```
conda create -n gs -c conda-forge ghostscript
# then set GhostscriptPath to ~/miniconda3/envs/gs/Scripts/gswin64c.exe (or envs/gs/bin/gs)
```

If the binary is absent, the stage logs a warning and skips every PDF — it never crashes the
pipeline.

## Files

- `PdfCmykPostProcessConfig.cs` — config + `Apply` entry point (gated Release-only).
- `PdfCmykPostProcessor.cs` — discovers PDFs under `Target/`, extracts ICC, generates
  `PDFX_def.ps`, invokes Ghostscript, replaces the original atomically on success.
- Tests: `Argumentum.AssetConverter.Tests/PdfCmykPostProcess/PdfCmykPostProcessTests.cs` —
  gating, argument/PDFX_def contracts, ICC extraction, graceful-skip-when-absent (no GS required).

## Legacy note

The per-image `ConvertToCmyk` (`DocumentCardSet.ConvertToCmykRelease`) is a **no-op for the PNG
output path** (CMYK is lost at the PNG write). It is left in place for now (removing it is a
behavioral change — sRGB→CMYK→RGB round-trip color shift — that needs a visual verdict on the GS
bundle first). This GS post-process is the authoritative CMYK path. Flipping
`ConvertToCmykRelease=false` is deferred to a follow-up after the GS bundle is visually validated.
