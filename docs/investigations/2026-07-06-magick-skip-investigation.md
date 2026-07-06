# 2026-07-06 — Magick.NET deferred-bug investigation (#719 flag, dispatch `h2utyb` SECONDARY)

**Scope**: root-cause analysis of the deferred-bug test skip flagged by #719
(`SvgConversionIntegrationTests.TryAutomateSvgConversion_WithValidMmFile...`,
`[Fact(Skip = "Temporairement désactivé pour isoler le crash Magick.NET")]`). Dispatched by ai-01
(`h2utyb` SECONDARY, 2026-07-06 07:09): "investigate/document the Magick.NET test skip: root cause,
minimal repro, fix option or tracking issue". **Docs/investigation — 0 write Cards/, 0 code change.**

**Author**: po-2024 (worker) · **Dispatch**: `h2utyb` (SECONDARY) · **Base**: master `7ebeda18`.

---

## TL;DR — verdict

**The Magick.NET crash is OBSOLETE — it is no longer reproducible.** The code path that crashed in
August 2025 was **replaced**: `TryAutomateSvgConversion` now delegates to `TryFreeMindSvgExport`
(FreeMind GUI process + SendKeys), and the production SVG rendering moved to **Batik** (PR #565,
April 2026). Magick.NET is no longer invoked anywhere in the mind-map SVG conversion path. The
2025-08-01 skip reason is stale; the test's premise (Magick-based SVG conversion) no longer matches
the implementation.

| Question (#719 flag) | Answer (code=truth on `7ebeda18`) |
|----------------------|-----------------------------------|
| (a) Is the Freeplane `TryAutomateSvgConversion` path dead code post-Batik? | **The Magick dependency in it is dead** — `TryAutomateSvgConversion` → `TryFreeMindSvgExport` (FreeMind GUI + SendKeys), **0 Magick calls**. |
| (b) If still a fallback, is the Magick crash latent? | N/A — Magick is not in this path at all. |
| (c) Test-isolation artifact? | No — the crash was real in 2025 (old path), but the path was since rewritten. |

**Followups surfaced** (post-tag, bundled with #710/#719 test-hygiene lane): the stale skip reason
must be replaced; a dead `using ImageMagick;` can be removed. See §3.

---

## 1. Root cause — the Magick crash lived in the OLD Freeplane+Magick SVG path (now replaced)

### What the skip said (2025-08-01)
`6bd802f4` ("Stabilisation complète de la suite de tests", 2025-08-01) skipped
`TryAutomateSvgConversion_WithValidMmFile_ShouldReturnTrueAndCreateSvgFile` with
`Skip = "Temporairement désactivé pour isoler le crash Magick.NET"`. At the time, the SVG conversion
path used **Magick.NET** to rasterize/process Freeplane SVG output, and a crash in that path was
destabilizing the test suite.

### What the code does now (code=truth on `7ebeda18`)
The path was rewritten in two stages:

1. **`TryAutomateSvgConversion` now delegates to `TryFreeMindSvgExport`** ([FallacyMindMapDocumentConfig.cs:439-442](Generation/Converters/Argumentum.AssetConverter/Mindmapper/FallacyMindMapDocumentConfig.cs#L439)):
   ```csharp
   private bool TryAutomateSvgConversion(string sourceMmPath, string destinationSvgPath, AssetConverterConfig config, bool isInteractive = true)
   {
       return TryFreeMindSvgExport(sourceMmPath, destinationSvgPath, config);
   }
   ```
2. **`TryFreeMindSvgExport` / `TryFreeMindSvgExportCore`** uses the **FreeMind GUI process + SendKeys**
   automation (commit `46d6cd9b`, CLAUDE.md "FreeMind GUI automation via SendKeys.SendWait —
   VALIDATED"), and the actual SVG rendering moved to **Batik** (PR #565 `55c6774e`, CLAUDE.md
   "Mind Maps & SVGs — April 2026 COMPLETE"). **No Magick.NET call anywhere in this path.**

### Evidence — Magick is absent from the SVG conversion path
```
$ grep -ncE "Magick" FallacyMindMapDocumentConfig.cs
1
$ grep -nE "Magick" FallacyMindMapDocumentConfig.cs
19:using ImageMagick;
```
**The only `Magick` token in the entire file is the `using` directive at line 19 — and it is dead**
(zero usages). The method body uses `Process.Start` (launch javaw/FreeMind) + `SendKeysSafe`
(`SendKeys.SendWait`), not `MagickImage`/`MagickImageCollection`.

**Conclusion**: the crash was real in 2025 against the old Freeplane+Magick path; that path was
replaced, so the crash is no longer reproducible. This is option (a) of the #719 flag.

---

## 2. Magick.NET is healthy and load-bearing — but ELSEWHERE

Magick.NET (14.14.0) is not removed from the project; it is alive and correct in the **image/PDF**
paths, which are unrelated to the mind-map SVG conversion:

- [ImageHelper.cs:23](Generation/Converters/Argumentum.AssetConverter/ImageHelper.cs#L23) — `new MagickImage(sourceFile)` (image loading).
- [ImageHelper.cs:38](Generation/Converters/Argumentum.AssetConverter/ImageHelper.cs#L38), [:50](Generation/Converters/Argumentum.AssetConverter/ImageHelper.cs#L50) — `new MagickImage(...)` (image read with settings).
- [PdfManager.cs:27](Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs#L27), [:53](Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs#L53), [:166](Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs#L166) — `MagickImageCollection` (PDF image assembly).

So the package itself is fine; only its (former) presence in the SVG conversion path was the problem, and that presence is gone.

---

## 3. Fix options for the post-tag test-hygiene lane

The dispatch asked for "fix option or tracking issue". The Magick crash needs **no fix** (obsolete);
what needs fixing is the **stale skip reason + the test's obsolete premise**. Three options, jsboige's
call (bundled with #710 nullable-cleanup + #719 test-skips post-tag lane):

1. **Reclassify as infra-gated** (recommended): change the skip to
   `Skip = "requires FreeMind GUI (machine-specific path ARGUMENTUM_FREEMIND_PATH)"`. The test now
   exercises the FreeMind GUI export (needs FreeMind installed, like the GSheet tests need OAuth), so
   it cannot run in CI anyway. This matches the established infra-gated pattern and gives an accurate
   reason. The test still has value as a jsboige-local smoke test of the GUI export.
2. **Delete the test**: its Magick premise is obsolete, and the SVG-generation path is already
   covered headlessly by the Batik tests + the `VirtueMindMap_GeneratesNativeScript_ForArFaZh` test
   (#715). If the GUI export is deemed sufficiently covered by manual régén runs, delete is clean.
3. **Retarget to a headless Batik assertion**: if a CI-runnable SVG-correctness test is wanted,
   retarget it to assert on a Batik-produced SVG (the production path) rather than the GUI export.
   This is the most work and overlaps #715; lowest priority.

**Recommendation**: option **(1)** — minimal, accurate, preserves the local smoke-test value. In all
three options, the stale `"Temporairement désactivé pour isoler le crash Magick.NET"` reason must be
replaced (it references a crash that no longer exists).

### Dead-code cleanup (minor, same post-tag lane)
The `using ImageMagick;` at [FallacyMindMapDocumentConfig.cs:19](Generation/Converters/Argumentum.AssetConverter/Mindmapper/FallacyMindMapDocumentConfig.cs#L19) is now unused (the only Magick token in the file). Removing it is a safe, trivial cleanup that prevents future confusion (a reader seeing the `using` would wrongly infer Magick is used here).

### Tracking
**No dedicated GitHub issue needed.** The finding folds into the #719 post-tag test-hygiene lane
(already documented in `docs/repo/test-skips-inventory.md` §2). When the lane runs (post-tag), apply
option (1) or (2) above + remove the dead `using`.

---

## 4. Reproducibility note (why no live repro was run)

A live reproduction was **not run** because it would not be informative: the test sets
`FreeMindPath = ""` / `FreeplanePath = ""` (disabled), so invoking `TryAutomateSvgConversion` now
hits the "FreeMind not found" early-return branch ([FallacyMindMapDocumentConfig.cs:559-562](Generation/Converters/Argumentum.AssetConverter/Mindmapper/FallacyMindMapDocumentConfig.cs#L559)) and
returns `false` without reaching any Magick code (there is none to reach). The current failure mode
of the test — if un-skipped — would be "FreeMind not found → returns false → assertion fails", **not**
a Magick crash. The code analysis in §1 (Magick token count = 1 = dead using; method delegates to
GUI+SendKeys) is conclusive and obviates a repro.

---

## Gate boundaries (HARD — docs/investigation, read-only)

- ❌ No code change, no test un-skip, no build config change (release freeze).
- ❌ No live test execution (the Magick path is gone; a repro would only show "FreeMind not found").
- ✅ Derived code=truth from source reads + git blame (provenance) + grep (Magick-token grounding).

Relates: dispatch `h2utyb` (SECONDARY), #719 (test-skips inventory — flagged this skip), #710
(nullable-cleanup post-tag lane — bundle target), #565 (Batik SVG migration — replaced the Magick
path), `46d6cd9b` (FreeMind GUI SendKeys automation — the replacement path), #133 (OWL known-fail,
the other non-skip test failure). Base `7ebeda18`.
