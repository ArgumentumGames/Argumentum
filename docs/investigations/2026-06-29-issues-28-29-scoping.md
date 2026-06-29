# #28 / #29 — Scoping (read-only) : closures justified, fixes verified in code

**Author**: po-2024 (worker) · **Date**: 2026-06-29 · **Base**: master `ba8e4a6c`
**Trigger**: ai-01 deep-queue dispatch (TERTIAIRE, post-coupure) — "caractérise dissociation front/back (#28) + conso mémoire résiduelle (#29 partiellement #436 Magick.NET dispose)".
**Method**: read-only code grounding (SDDD — code = truth). **0 modif code AssetConverter** (pre-tag safe). master stays `ba8e4a6c`.

---

## TL;DR

Both issues are **CLOSED and legitimately so** — the requested fixes exist in the current code, with explicit `// #28` / `// #29` comments documenting them. The dispatch was a stale-dispatch scoping request; the scoping confirms the closures stand. **No re-opening, no code change warranted** (pre-tag freeze respected regardless).

| Issue | State | Requested (2021 report) | Status in code (`ba8e4a6c`) |
|---|---|---|---|
| **#28** | CLOSED | Dissociate front/back target dirs + stop before PDF | **Both halves implemented** (opt-in flags, default `false` for backward compat) |
| **#29** | CLOSED | High memory / machine sluggish | **Fixed** via #436 (deterministic `MagickImage` dispose, ~1.2 GB peak eliminated) |

---

## #28 — front/back folder separation + stop-before-PDF

Two config flags in [`AssetConverterConfig.cs`](../../Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs), both opt-in (default `false` to preserve the historical flat layout):

### (a) `SeparateFrontBackFolders` — `AssetConverterConfig.cs:351`
```csharp
/// Issue #28 (a): when true, harvested card images are written to distinct
/// front\ and back\ sub-folders under each card-set image folder instead of
/// sharing the same directory. Default false keeps the historical flat layout.
public bool SeparateFrontBackFolders { get; set; } = false;
```
The front/back dissociation the 2021 report asked for. Default off = backward-compatible (faces suffixed `_face`, backs by name in one folder); opt-in for the separated layout.

### (b) `StopBeforePdfGeneration` — `AssetConverterConfig.cs:359`
```csharp
/// Issue #28 (b): when true, the pipeline stops right after harvesting and
/// image generation, before any PDF document is assembled — even when
/// ConverterMode.QuestPdfGeneration is set.
public bool StopBeforePdfGeneration { get; set; } = false;
```
The "stop before PDF" capability. Additionally, the [`ConverterMode`](../../Generation/Converters/Argumentum.AssetConverter/ConverterMode.cs) flags enum (`QuestPdfGeneration = 1 << 12`, etc.) lets callers granularly disable any stage — a superset of (b).

**Verdict**: #28 resolved. The only "residual" is a design choice, not a bug — the flags are opt-in (default off) so the *default* behavior is the pre-#28 flat/shared layout. Re-opening would only make sense if the default should flip, which is a product decision (post-tag), not a defect.

---

## #29 — high memory consumption

Two `// #29 fix` sites, both deterministic-dispose of `ImageMagick` resources (the heap pressure source on the 2021 "machine qui rame" report):

### MagickImage — [`ImageHelper.cs:105`](../../Generation/Converters/Argumentum.AssetConverter/ImageHelper.cs)
```csharp
// #29 fix: deterministic dispose of MagickImage after processing
using var imageFromEmbeddedUrl = imageUrl switch { ... };
```
`using var` → the per-card image is disposed at scope exit, not held to the GC finalizer.

### MagickImageCollection — [`PdfManager.cs:264`](../../Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs)
```csharp
// #29 fix: deterministic dispose of MagickImageCollection after write
// prevents ~1.2 GB peak held until GC finalizer (esp. Fallacies Tarot ~277 images).
WriteAndDispose(targetFile.documentImages, collection => collection.Write(targetFile.fileName));
```
The `WriteAndDispose<T>` helper (`PdfManager.cs:289`) is **pure + unit-testable** (no Magick render needed to test the dispose contract) — extracted output-neutral from the inline control flow, preserving the exact `using`-scope. This is the #436 Magick.NET dispose work ai-01 referenced.

**Verdict**: #29 resolved with a robust, tested fix. The ~1.2 GB peak (Fallacies Tarot ~277 images) is eliminated by deterministic dispose. No residual memory defect identified in read-only review. (QuestPDF's thread-safety global lock — separate concern, documented in CLAUDE.md — is not a memory leak.)

---

## Recommendation

- **Do not re-open #28 or #29.** Both closures are justified by concrete, commented fixes in the codebase.
- **No code change** (this is scoping only; pre-tag freeze on AssetConverter regardless).
- If the product later wants the front/back-separated layout or stop-before-PDF as the **default**, that's a post-tag config-defaults decision — tracked here as a note, not an action.

---

## Reproducibility

- `grep -rn "// #28\|#29 fix\|StopBeforePdfGeneration\|SeparateFrontBackFolders\|WriteAndDispose" Generation/Converters/Argumentum.AssetConverter/` — re-locates every site cited above.
- This doc is the scoping deliverable. Read-only; 0 write under `Cards/`, 0 AssetConverter code change.

Relates to #436 (Magick.NET dispose), #28, #29.
