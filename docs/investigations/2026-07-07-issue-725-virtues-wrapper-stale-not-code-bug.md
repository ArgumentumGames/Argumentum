# Issue #725 — Virtues HTML wrapper FR-frozen: **stale file, not a code bug**

**Author**: po-2024 (worker) · **Date**: 2026-07-07 · **Base**: master `6ce91ef8`
**Scope**: read-only code/data investigation + CI regression test. **0 write under `Cards/`** (release freeze). master untouched.
**Dispatch**: ai-01 `3i1ie4` [PRIMAIRE] #725 — "corrige l'assemblage du wrapper (quel `content.svg` par-langue est inliné) ; code + test headless gated, 0 régén".
**Related**: #725 (this issue) · #665 / `27442add` (Virtues mindmap i18n wiring — the actual fix) · #715 (Virtue ar/fa/zh localization) · #724 (Virtue ar/fa/zh native-script SVGs) · #686 / `204adc47` (regen Virtues Batik SVGs) · `2026-06-25-virtues-mindmap-fr-frozen-mechanism.md` (the FR-frozen mechanism, now **superseded** for the `.content.svg`) · memory `virtues-mindmap-content-svg-fr-frozen` (**stale** — see §5).

---

## TL;DR

1. **The issue's root-cause hypothesis is incorrect on the mechanism.** #725 hypothesizes the wrapper "inlines the raw FreeMind `.svg` (French source) rather than the post-localization `content.svg`". Code reading proves the opposite: `GenerateHtmlSvgWrappers` inlines the **post-localization `content.svg`** (the `svgLoader` produced after `UpdateSvgWithItems`), never the raw render.
2. **The committed wrappers are stale, not mis-assembled.** `Argumentation_Virtues_{en,ru,…}.html` were last touched `2026-05-24` by `df3c769e` (#312) — **before** the Virtues i18n wiring landed in `27442add` (#665). At that time the Virtues `content.svg` was genuinely FR-frozen (per the 2026-06-25 investigation). The wrapper was generated FR, then never regenerated.
3. **The `content.svg` (the input to the wrapper) IS now localized** for all 8 languages (regenerated `2026-07-06` by `204adc47` / #686, after #665/#715/#724). The wrapper just wasn't regenerated alongside it.
4. **Why the wrapper was skipped during every later SVG regen:** `AssetConverterConfig.OverwriteExistingHtmlMaps` defaults to `false` (no initializer, `AssetConverterConfig.cs:380`) while `OverwriteExistingDocs` is `true` (`:361`). So a regen clobbers the `.content.svg` (docs) but silently skips the existing `.html` wrappers. This is the operational root cause of the staleness.
5. **No code fix is needed for #725.** The assembly code is correct (proven 4 ways below). The fix is a **post-tag regen of the wrappers with `OverwriteExistingHtmlMaps=true`** (po-2023 lane, RDP/FreeMind-gated). This investigation + a CI regression test are the gated, régén-free deliverable for this tick.
6. **Memory `virtues-mindmap-content-svg-fr-frozen` is now stale for the `.content.svg`** — #665/#715/#724 fixed it. Updated in §5.

---

## 1. Evidence — the assembly code inlines the localized `content.svg`

`VirtueMindMapDocumentConfig.ProcessSvgFilesAsync` ([VirtueMindMapDocumentConfig.cs:531-605](../../Generation/Converters/Argumentum.AssetConverter/Mindmapper/VirtueMindMapDocumentConfig.cs#L531-L605)):

- `svgFilePath` = the raw FreeMind render (`…_{lang}.svg`, FR tree structure as emitted by FreeMind).
- For each `SVGFreemindMap` (here `content.svg`): `svgSavedFilePath = …_{lang}.content.svg`.
- Either branch of the exist-check resolves `svgLoader` to the **localized** SVG:
  - File-exists branch (`:541-545`): reads the saved `…_{lang}.content.svg` (which is localized post-#686).
  - Regen branch (`:546-592`): loads the raw render, applies `UpdateSvgWithItems(svgFreemindMap, mindMapItems, …)` (`:583`) — this is the post-processing that rewrites node text from the **localized** `mindMapItems` — then `svgLoader = () => GetSvgContent(svgDoc)`.
- `GenerateHtmlSvgWrappers(svgFreemindMap, …, svgLoader, language)` (`:594`) consumes that localized `svgLoader`.

`GenerateHtmlSvgWrappers` ([VirtueMindMapDocumentConfig.cs:869-901](../../Generation/Converters/Argumentum.AssetConverter/Mindmapper/VirtueMindMapDocumentConfig.cs#L869-L901)):

```csharp
var languageAwareDocName = htmlSvgWrapper.DocumentName.Replace("[LANGUAGE]", language);   // :881
…
htmlTemplate = MindMapHtmlWrapper.FormatWrapper(htmlTemplate, svgRelativePath, await svgContent());  // :895
File.WriteAllText(htmlFileName, htmlTemplate, Encoding.UTF8);                              // :897
```

`svgContent` is the localized `svgLoader` from above. The wrapper name is correctly per-language (`Argumentation_Virtues_{lang}.html`). **At no point is the raw FreeMind render inlined** — #725's hypothesis is wrong on the mechanism.

The `included.html` template carries only the `[SVGCONTENT]` placeholder ([MindMapHtmlWrapper.cs:5-21](../../Generation/Converters/Argumentum.AssetConverter/Mindmapper/MindMapHtmlWrapper.cs#L5-L21)); it contributes no language of its own.

## 2. Evidence — empirical state on master `6ce91ef8`

Codepoint scan of the committed files (visible `<text>` content, not `family=` attributes):

| Lang | `…content.svg` native script | `…content.svg` `"Honnêteté intellectuelle"` | `Argumentation_Virtues_{lang}.html` native script | wrapper `"Honnêteté intellectuelle"` |
|------|------------------------------|---------------------------------------------|---------------------------------------------------|--------------------------------------|
| fr   | — (Latin)                    | **27×** (correct)                           | — (Latin)                                         | **133× FR diacritics** (correct for FR) |
| en   | `Intellectual honesty` ×28   | **0×** ✅ localized                          | **0 EN labels, 0 Cyrillic/CJK/Arabic**            | FR only ❌                            |
| ru   | Cyrillic ×40066              | **0×** ✅                                    | **0 Cyrillic**                                    | FR only ❌                            |
| zh   | CJK ×10189                   | **0×** ✅                                    | wrapper file not committed for zh                 | —                                    |
| ar   | Arabic ×23980                | **0×** ✅                                    | wrapper file not committed for ar                 | —                                    |
| fa   | Arabic ×25576                | **0×** ✅                                    | wrapper file not committed for fa                 | —                                    |

- The `content.svg` is localized across the board (post-#686). The `fr` residual of `"Honnêteté"` (without `intellectuelle`) at 27× in every language is a separate, constant, structural residue (untranslated link/root nodes) — out of #725's scope, constant across languages, not the reported regression.
- The committed wrapper for `en` and `ru` is **100% French** — including `ru` having **zero** Cyrillic. That is impossible to produce from the *current* `content.svg` (which is Cyrillic), so the wrapper must predate the localized `content.svg`. Confirmed in §3.

## 3. Evidence — git history: wrapper predates the i18n fix; `content.svg` was regenerated after

```
Argumentation_Virtues_en.html        last: df3c769e  2026-05-24  feat(mindmaps): … zoom/pan (#312)
Argumentum_Virtues_MindMap_en.content.svg  last: 204adc47  2026-07-06  regen(mindmaps): refresh Virtues Batik SVGs (#686)
Argumentation_Virtues_ru.html        last: df3c769e  2026-05-24  (#312)
```

And the Virtues i18n **code** wiring landed between the two:

```
27442add  fix(mindmap): #636 §2 — wire Virtues mindmap i18n for En/Ru/Pt/Es (#665)
```

So: on 2026-05-24, the Virtues `content.svg` was still FR-frozen (per the 2026-06-25 investigation, base `bef3bc6c`); the wrapper generated that day was correctly FR **relative to its FR input**. `#665` then wired i18n; `#715`/`#724` added ar/fa/zh; `#686` regenerated the `content.svg` localized — but the wrappers were skipped (`OverwriteExistingHtmlMaps=false`, §4) and stayed FR.

## 4. Evidence — the Fallacies wrapper (same code path) IS localized

The Fallacies wrapper uses the identical `GenerateHtmlSvgWrappers` path (`FallacyMindMapDocumentConfig.cs:1499-1526`). The committed Fallacies wrapper is localized:

```
Fallacies_en.html:  EN-hits=188  FR-hits=12   text nodes: 'Fallacy', 'Ad hominem', 'Name Calling', 'Defamation' …
```

Same assembly code, correct result for Fallacies ⇒ the assembly code is correct; the Virtues wrapper is a stale-file artifact, not a code defect.

## 5. Operational root cause & post-tag fix

- **Root cause (staleness):** `OverwriteExistingHtmlMaps` defaults to `false` (`AssetConverterConfig.cs:380`, no initializer) while `OverwriteExistingDocs = true` (`:361`). Any SVG regen clobbers the `.content.svg`/`.links.svg` (docs) but skips existing `.html` wrappers (`VirtueMindMapDocumentConfig.cs:886`, `FallacyMindMapDocumentConfig.cs:1514`, `MindMapDocumentConfig.cs:772`).
- **Post-tag fix (po-203 lane, gated):** regen the Virtues (and, defensively, Fallacies) mind maps with `OverwriteExistingHtmlMaps=true` so the wrappers are rebuilt from the now-localized `content.svg`. Then commit the regenerated `Argumentation_Virtues_{lang}.html` / `_ext.html`. Clobber the harvest/SVG cache first (lesson `regen-success-without-clobber-is-stale-trap`). Add zh/ar/fa wrappers (none committed today — the pipeline will emit them once the regen runs; they are not in `git ls-files`).
- **Optional hardening (judgment call, separate PR, not this tick):** couple wrapper regen to SVG regen, or emit a warning when a wrapper is skipped but its sibling `content.svg` is newer. Out of scope here (mindmap post-processing = judgment call per No-Pendulum).

## 6. Deliverable this tick (gated, régén-free)

- **CI regression test** `Argumentum.AssetConverter.Tests/MindmapGeneration/VirtuesMindmapWrapperLocalizationTests.cs` — a headless (no Playwright) Theory over fr/en/ru/zh/ar/fa that feeds the committed localized `content.svg` through `MindMapHtmlWrapper.FormatWrapper` and asserts the wrapper matches the target language (native script / EN label present, FR-frozen marker absent for non-FR). Mirrors `VisualTests/MindmapWrapperTests.cs` for Fallacies but runs in the unit-test CI suite. Pins the assembly contract so a future regression in the helper or the i18n wiring surfaces without RDP/FreeMind.
- **This investigation** documenting the stale-file verdict.

## 7. Memory correction

`virtues-mindmap-content-svg-fr-frozen` (written 2026-06-25 on base `bef3bc6c`) asserted the Virtues `.content.svg` is FR-frozen. That was true then; it is **false now** — #665/#715/#724/#686 localized the `.content.svg` (Cyrillic/CJK/Arabic verified present, §2). The memory is updated to scope the FR-frozen claim to the **pre-#665** era and point the remaining gap at the **stale wrapper** (this issue), not the `content.svg`.
