# Virtues mindmap `.content.svg` FR-frozen — mechanism traced, root cause & fix path

**Author**: po-2024 (worker) · **Date**: 2026-06-25 · **Base**: master `bef3bc6c` (release-frozen)
**Scope**: read-only code investigation. **0 write under `Cards/`** (release freeze). master stays `bef3bc6c`.
**Related**: release dossier #591 §3.2 (gap documented as "deferred, non-blocking v0.9.0"); cross-verify #599 (orthogonality); memory `virtues-mindmap-content-svg-fr-frozen`.

---

## TL;DR

1. **The gap is real and verified**: Virtues `.content.svg` are FR-frozen (po-2023 observed `Argument valable` / `Échange enrichissant` identical fr=zh=ar), while Fallacies `.content.svg` are localized across 8 languages. **Non-blocking v0.9.0** — deferred to jsboige's GO (post-release).
2. **The localization mechanism is now TRACED** (was "untraced" as of the #591 dossier). It is **upstream of the render engine** — neither `TitleFunc` nor `ProcessSvgFilesAsync` applies the language. It is **expression rewriting** via reflection.
3. **Root cause is 2-layer** and fully bounded:
   - **Layer B (config)** — the Virtues localization table is a stale stub (translates only the tree-root literal `"Vertus"`, 4 langs).
   - **Layer A (entity)** — `Virtue.cs` has no `TitleAr/TitleFa/TitleZh` properties (the ar/fa/zh CSV columns added by #590/#595 are invisible to the mindmap pipeline).
4. **The fix is bounded** (3 steps, mirrors the Fallacies path) but **gated on jsboige** (post-release). One non-obvious prerequisite: the Virtues `DefaultTitleExpression` must change from `{item.Text}` to `{item.TitleFr}` before a config-only fix can fire — otherwise `Replace("TitleFr", ...)` is a silent no-op.
5. **For the WE 27/06 arbitrage** (ai-01's options a/b/c): see [§Decision input](#decision-input-for-we-2706--per-option-cost-for-jsboiges-arbitrage) — per-option technical cost. Short version: **(b) ship-as-is + known-limitation note is cheapest** (0 write, already the #591 §3.2 assumption); **(c) exclude-from-packs is dominated by (b)** (it's a Cards/ write that *removes* present content); **(a) fix-at-tag is most expensive** (freeze break + RDP régén). Worker reco = (b), jsboige decides.

---

## The mechanism — how Fallacies mindmaps get localized (traced)

Localization is **expression rewriting in-place**, applied **before** the `.mm`/SVG render:

1. `ParallelDocumentCreatorConfigBase.ProcessDocumentAsync` ([ParallelDocumentCreatorConfigBase.cs:58-62](../../Generation/Converters/Argumentum.AssetConverter/ParallelDocumentCreatorConfigBase.cs#L58-L62)) — for each target language, **clones** the document config (`currentTranslatedMap = mindMap.Clone()`), then applies **every** entry of `LocalizationConfig.MindMapLocalization` via `DocumentLocalization.DoReflectionTranslate(clone, lang)`.

2. `DocumentLocalization.DoReflectionTranslate` ([DocumentLocalization.cs:37-54](../../Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Localization/DocumentLocalization.cs#L37-L54)) — reflects over the config's `string` properties whose name is in the entry's `TargetProperties`. For each: reads the current value, runs `DoStaticConversions(value, lang)`, writes it back.

3. `DoStaticConversions` ([DocumentLocalization.cs:16-31](../../Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Localization/DocumentLocalization.cs#L16-L31)) — **naive `template.Replace(sourceText, destText)`** per conversion, filtered by language. No token-boundary awareness (same fragility class as #477 card-set mapping, #488 CSV contract).

**Net effect**: the config's *expression strings* are rewritten per language. E.g. Fallacies `TitleExpression = "{fallacy.TextFr}"` becomes `"{fallacy.TextZh}"` for `zh`. The interpolation at render time (`TitleFunc`) then produces localized text. The `.content.svg` is rebuilt from the in-memory Freemind model (`ProcessSvgFilesAsync`), which carries the already-localized expression.

The 4 production `MindMapLocalization` entries ([AssetConverterConfig.cs:207-261](../../Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs#L207-L261)):

| # | Targets | Does | Localizes Fallacies? | Localizes Virtues? |
|---|---|---|---|---|
| 1 | `FallacyMindMapDocumentConfig.{Title,Card,Description,Example,Link}Expression` | `TextFr→TextZh` (+6 langs), `DescFr→…`, `ExampleFr→…`, `LinkFrFallback→…` | ✅ **yes (text)** | n/a |
| 2 | `FallacyMindMapDocumentConfig.{Famille,SousFamille,SoussousFamille}Expression` | `Soussousfamille→SubsubfamilyZh`, etc. (most-specific-first ordering) | ✅ **yes (hierarchy)** | n/a |
| 3 | `VirtueMindMapDocumentConfig.TitleExpression` | `"Vertus"→"Virtues"/"Dobrodeteli"/"Virtudes"` (4 langs, **tree root only**) | n/a | ❌ **no (stub)** |
| 4 | `DocumentConfig.DocumentName` | `_fr.→_zh.` | filename only | filename only |

---

## Root cause — why Virtues stay FR (2 layers, both verified on `bef3bc6c`)

### Layer B — the Virtues conversion table is a stale stub (entry #3)

`AssetConverterConfig.cs:242-250` entry #3 targets `VirtueMindMapDocumentConfig.TitleExpression`, but its only `StaticConversion` is `("Vertus", [en→Virtues, ru→Dobrodeteli, pt→Virtudes, es→Virtudes])` — the **tree-root literal**, 4 languages only. The comment at l.241 reads *"data is FR-only, only tree root name changes"*.

That design intent is **stale**: the Virtues CSV is now translated ×7 langs (via #590 Phase 1 + #595/#583 harmonization). But the config was never updated to consume it.

Consequence: `VirtueMindMapDocumentConfig.DefaultTitleExpression = @"{item.Text}"` ([VirtueMindMapDocumentConfig.cs:37](../../Generation/Converters/Argumentum.AssetConverter/Mindmapper/VirtueMindMapDocumentConfig.cs#L37)). `"{item.Text}".Replace("Vertus", "Virtues")` is a **no-op** (the substring `"Vertus"` is not present) → the expression stays `{item.Text}` = `Virtue.Text` = `TitleFr` = **FR for all 8 languages**.

### Layer A — `Virtue` has no ar/fa/zh title properties (data exists, entity doesn't map it)

[Virtue.cs:25-68](../../Generation/Converters/Argumentum.AssetConverter/Entities/Virtue.cs#L25-L68) maps only `TitleFr/En/Ru/Pt/Es` (gap between l.68 and l.73 — no `TitleAr/TitleFa/TitleZh`). Compare `Fallacy.cs:134/146/158` which has `TextAr/TextFa/TextZh` (+ ClassMap at `:250/257/264`).

So even if Layer B were fixed, the ar/fa/zh CSV columns are **invisible to the mindmap pipeline** — there is no entity property to bind `{item.TitleZh}` to.

**Verified (2026-06-25)**: the **CSV data exists**. `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` carries `title_ar`/`title_zh`/`title_fa` (cols 39/51/57), plus `subfamily_{ar,zh,fa}`, `subsubfamily_{ar,zh,fa}`, `description_*`, `remark_*`, `family_*`, `link_*` for all three. So the gap is **purely the entity mapping**, not a missing-data problem — no upstream data task.

---

## Fix path (post-release, jsboige-gated — 3 bounded steps, mirrors Fallacies)

> ⚠️ **Gated**: touch the mindmap post-processing = judgment call (per ai-01 17:16 / po-2023 16:20, No-Pendulum). Do **not** code before jsboige's GO. The localization *content* exists in CSV; this is about making the mindmap pipeline consume it.

1. **Entity** ([Virtue.cs](../../Generation/Converters/Argumentum.AssetConverter/Entities/Virtue.cs)): add `TitleAr/TitleFa/TitleZh` (+ `SubfamilyAr/Fa/Zh`, `SubsubfamilyAr/Fa/Zh` for the hierarchy nodes) + `ClassMap` entries `.Optional()` mirroring `Fallacy.cs:250/257/264`. ✅ **CSV data verified present** (Layer A note above: `title_ar/zh/fa` + `subfamily_*`/`subsubfamily_*` exist in `Argumentum Virtues - Taxonomy.csv`) — no upstream data task, pure entity-mapping work.

2. **Expression** ([VirtueMindMapDocumentConfig.cs:37](../../Generation/Converters/Argumentum.AssetConverter/Mindmapper/VirtueMindMapDocumentConfig.cs#L37)): change `DefaultTitleExpression` from `{item.Text}` → **`{item.TitleFr}`** (parallel to Fallacies' `{fallacy.TextFr}`). **This is the non-obvious prerequisite**: `DoStaticConversions` is plain `Replace`, so `Replace("TitleFr","TitleZh")` only fires if the literal `TitleFr` is *in* the expression. `{item.Text}` contains no such literal → a config-only fix (step 3 alone) would be a **silent no-op**, repeating the SUPPOSÉ-error class. Changing the expression first is what unblocks the conversion.

3. **Config** ([AssetConverterConfig.cs:242-250](../../Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs#L242-L250)): replace the root-only entry #3 with an entry mirroring Fallacy text (#1) — target `VirtueMindMapDocumentConfig.TitleExpression` with `StaticConversions ("TitleFr",[en→TitleEn, ru→TitleRu, pt→TitlePt, es→TitleEs, ar→TitleAr, fa→TitleFa, zh→TitleZh])` — plus a hierarchy entry mirroring #2 for the family/subfamily/subsubfamily nodes.

### Validation

- **Regen 8-lang with clobbered harvests** (cf `regen-success-without-clobber-is-stale-trap`: exit 0 + identical count ≠ fresh content — must clobber `ImageHelper.cs:100` skips).
- Verify `Argumentation_Virtues_zh.content.svg` contains CJK glyphs, not `Échange enrichissant`; `…_ar.content.svg` RTL Arabic, not `Argument valable`.
- **Add a Virtues mirror of `MindMapLocalizationRegressionTests.cs`** — the existing regression suite covers Fallacies only (its own docstring, l.40-43, lists the 4 prod entries but the test asserts Fallacy expressions). Without a Virtues test, a future regression here is silent.

---

## Decision input for WE 27/06 — per-option cost (for jsboige's arbitrage)

ai-01 raised 3 options for this gap at the coordinator level. Technical cost per option — this investigation's contribution; **jsboige decides**:

**Fact (verified on `bef3bc6c`): mindmap SVGs are committed files**, not package-assembled. They live at `Cards/Fallacies/Mindmaps/{lang}/Argumentum_Virtues_MindMap_<lang>.{content,links}.svg` and ship as-is with the repo (no separate release-packaging/bundling step produces them). Mindmap regeneration runs in `Mindmapper` mode — the **only pipeline mode requiring an interactive RDP window** (FreeMind/Batik via SendKeys; #591 §3.2, #569; memory `regen-success-without-clobber-is-stale-trap`).

- **(a) Fix before tag** — cost = the 3-step fix above **+ a full mindmap régén** (RDP session, clobber Virtues harvests, re-commit 7 non-FR `.content.svg`/`.links.svg`) **+ CJK/RTL eyeball** + the Virtues regression test. **Breaks the release freeze** (Cards/ write) and re-touches the #591 byte-proven baseline (the `Fallacies_zh.svg` 5 451 309 B citation would need re-establishing). Multi-step, RDP-gated, delays the tag. → most expensive.
- **(b) Ship v0.9.0 with Virtues mindmaps FR-frozen + known-limitation note** — cost = **0 code, 0 Cards/ write**. The committed SVGs already exist (FR-frozen is the *current* state). Add a 1-line known limitation to `RELEASE-NOTES-v0.9.0.md`; apply the 3-step fix in v0.9.1. **This is what #591 §3.2 already assumes** ("non-blocking v0.9.0, deferred"). → cheapest.
- **(c) Exclude Virtues mindmaps from non-FR packs** — **dominated by (b)**. Because the SVGs are committed files (not assembled by packaging), "excluding" = `git rm`-ing 14 non-FR Virtues SVG files = a Cards/ write (same freeze-break class as (a)) **that removes present content** — an `ar`/`zh` user would get *no* Virtues mindmap rather than an FR-frozen one. Worse outcome at ≥ cost of (b).

**Worker recommendation (jsboige decides)**: **(b)** — ship v0.9.0 as-is (FR-frozen Virtues mindmaps are already committed and non-blocking; the *cards* are localized 8-lang PASS per ai-01's verdict), document the known limitation, fix in v0.9.1 via the 3-step path. **(c) can be dropped** from the decision (technically inferior to (b)). (a) only if jsboige wants the mindmaps localized *at tag* — accept the freeze break + RDP régén.

---

## Orthogonality note (vs #599 / #568)

This gap is **upstream of the render engine**. Whether the `.mm` is rendered by **Batik (FreeMind)** or **Freeplane (#599 opt-in)**, the node `TEXT` carries the same `{item.Text}` = FR for Virtues — the engine renders what it is given. The Freeplane breakthrough-refutation (#568 §3.8) and this finding are independent. (Cross-verify ACK #599 `4802039001`, po-2023.)

---

## Reproducibility

Read-only on master `bef3bc6c`. No code run. All line references verified against current source. Re-verify with:
- `grep -n "DefaultTitleExpression" Generation/Converters/Argumentum.AssetConverter/Mindmapper/VirtueMindMapDocumentConfig.cs`
- `grep -n "TitleAr\|TitleFa\|TitleZh" Generation/Converters/Argumentum.AssetConverter/Entities/Virtue.cs` → expect **no matches** (the gap)

---

*This doc converts the #591 §3.2 "deferred, mechanism untraced" finding into a "traced, bounded fix path" — so jsboige's post-release decision is informed, not open-ended.*
