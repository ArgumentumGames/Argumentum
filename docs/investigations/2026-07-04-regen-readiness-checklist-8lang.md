# 2026-07-04 — Régén-readiness checklist 8-lang (PREP, pas exécution) — #sde6s0 PRIMARY

**Scope**: cold-executable prep checklist for the **full 8-language fresh regen** required before
tag v0.9.0 (decision #26: régén fraîche 8 langues exigée avant publication). Triggered by ai-01
dispatch `sde6s0` (HIGH, deep-queue primary). Worker: po-2023 (heavy-regen lane).

**Repo reference**: master `21e2c666`. All file:line refs below verified on this base.

> **⚠ STATUS = PREP ONLY.** This doc is the harness + checklist. **Do NOT launch the regen yet** —
> two holds are active: (1) desktop is currently **non-interactive** (FreeMind GUI parts will fail /
> fall back to XSLT which is proscribed #184); (2) regen is **HOLD until final content is locked**
> (post all jsboige arbitrages). This doc prepares so the regen is a one-shot when the window opens.

---

## TL;DR — what a full 8-lang regen produces

A fresh regen rebuilds, for each of the 8 languages (fr/en/ru/pt/es/ar/fa/zh):
- **Card PNGs** (Playwright harvest of CardPen) → `Target/<lang>/Harvest/`
- **Print PDFs** (QuestPDF assembly) → `Target/<lang>/Documents/` (10 doc types × 8 langs = **80 PDFs**)
- **Mind-map SVGs** (FreeMind `.mm` → Batik SVG, GUI-automated) → `Cards/Fallacies/Mindmaps/<lang>/` + Virtues
- **OWL ontology** (mono-language caveat — see §6) → `Target/<lang>/Ontology/argumentum.owl`
- **CMYK post-process** (Ghostscript, runs LAST on the final bundle) → DeviceCMYK + SWOP OutputIntent

Total expected (per `docs/v0.9.0-readiness-and-disk-recovery.md` + bundle v3): **80 PDFs, ~4500
images, 20 mind-map SVGs (4 langs × 5 types historical → 8 langs target), OWL 5 MB**.

---

## 1. Languages (8) — code=truth

| Code | Language | Script | Notes |
|------|----------|--------|-------|
| `fr` | French | Latin | **Canonical / default** (`LocalizationConfig.cs:12` `DefaultLanguage = "fr"`) |
| `en` | English | Latin | Secondary (OWL includes EN — see §6) |
| `ru` | Russian | Cyrillic | |
| `pt` | Portuguese | Latin | |
| `es` | Spanish | Latin | |
| `ar` | Arabic | RTL | Direction-sensitive |
| `fa` | Farsi | RTL | Direction-sensitive |
| `zh` | Chinese | CJK | Font/tofu-sensitive; glyphs are compact (shorter char counts ≠ missing) |

- **Language list construction**: `LocalizationConfig.BuildLanguageList` (`LocalizationConfig.cs:47-55`)
  seeds with `DefaultLanguage`, then appends each card-set's `translations.targetLanguage`.
- **Field suffixes**: `_en`, `_ru`, `_pt`, `_es`, `_ar`, `_fa`, `_zh` — mapping tables at
  `AssetConverterConfig.cs:116-205` (e.g. `text_fr`→`text_en/.../text_zh` at :121).
- **Per-language iteration in harvest**: `HarvestManager.cs:88` (`BuildLanguageList`), `:90`
  (`Parallel.ForEachAsync` over `targetLanguages`).
- ⚠ **Stale comment** at `AssetConverterConfig.cs:94` lists only "FR, EN, RU, PT" — the tuples at
  :118-198 actually encode all 7 translation targets. The comment is stale, not the config.

---

## 2. Pipeline stages — ConverterMode flags (all MERGED on master)

`ConverterMode.cs:5-24` — 16 flags. Default Mode (`AssetConverterConfig.cs:36`):
`WebBasedImageGeneration | QuestPdfGeneration` (harvest + PDF only). A **full regen** sets
`Mindmapper | OwlGenerator` additionally. `SkipConfigFile = true` (`:34`) → **C# defaults are the
single source of truth; the JSON config file is ignored** (tuple serialization breaks Translations).

| Flag | Bit | Value | What it runs | Apply ref |
|------|-----|-------|--------------|-----------|
| `BatchImageProcessor` | 1<<0 | 1 | Batch image ops | `:456` |
| `WebBasedImageGeneration` | 1<<1 | 2 | **Playwright card harvest → PNGs** | `:485` |
| `Mindmapper` | 1<<2 | 4 | **FreeMind `.mm`→SVG (GUI, RDP-required)** | `:499` |
| `Dnn2sxc` | 1<<3 | 8 | 2sxc export | `:516` |
| `DatasetUpdater` | 1<<4 | 16 | Translation runs (gpt-5.5) | `:471` |
| `OwlGenerator` | 1<<5 | 32 | **OWL ontology (mono-lang)** | `:530` |
| `TaxonomyValidator` | 1<<6 | 64 | Taxonomy validation | `:544` |
| `OwlValidator` | 1<<7 | 128 | OWL validation | `:556` |
| `CardValidator` | 1<<8 | 256 | Card validation | `:568` |
| `ContinuousValidator` | 1<<9 | 512 | Continuous validation | `:580` |
| `TranslationCoverage` | 1<<10 | 1024 | Coverage report | `:592` |
| `ParallelismOptimizer` | 1<<11 | 2048 | Parallelism tuning | `:604` |
| `QuestPdfGeneration` | 1<<12 | 4096 | **PDF assembly** (consumed inside WebBasedGenerator) | — |
| `PdfAuditor` | 1<<13 | 8192 | PDF audit | `:616` |
| `GSheetSync` | 1<<14 | 16384 | GSheet ↔ CSV sync | `:628` |
| `PdfCmykPostProcess` | 1<<15 | 32768 | **Ghostscript CMYK+OutputIntent** (runs LAST) | `:641` |

> **Note**: an earlier reconnaissance (sub-agent) reported CMYK as "worktree-only / pending merge".
> **Verified FALSE on master `21e2c666`**: `ConverterMode.cs:24`, the `PdfCmykPostProcess/` folder,
> `--pdf-cmyk` (`Program.cs:373`), and `PdfCmykPostProcessConfig.Apply` (`:643`) are all present. The
> sub-agent had inspected the `dnn/sandbox-runtime-1032` working tree (behind master on CMYK). CMYK
> is merged.

---

## 3. Regen sequence — order of operations

`AssetConverterConfig.Apply` runs stages as sequential `if (Mode.HasFlag(...))` blocks. Most stage
work is collected into a `tasks` list, then **`await Task.WhenAll(tasks)`** at `:635` (a barrier),
and **`PdfCmykPostProcess` runs AFTER the barrier** (`:641-643`) — i.e. on the fully-built bundle.

**Recommended full-regen Mode** (additive over default):
```
WebBasedImageGeneration | QuestPdfGeneration | Mindmapper | OwlGenerator
```
Then a **separate CMYK pass** via `--pdf-cmyk` on the finished bundle (or add `PdfCmykPostProcess`
to Mode — same effect, runs last).

**Sequence for a one-shot fresh regen**:
1. **Clobber harvest cache** (§4) — mandatory, else stale `.harvest.json` is reused as-authoritative.
2. **Clobber FreeMind `auto.properties` tabs** (§5) — else FreeMind reopens stale maps.
3. **Confirm RDP live foreground** (§5) — required for Mindmapper stage; else XSLT fallback (proscribed).
4. **Set Mode** in C# defaults (`AssetConverterConfig.cs:36`) to include `Mindmapper | OwlGenerator`.
5. **`dotnet run --project ...AssetConverter.csproj`** — runs harvest → PDF → mindmaps → OWL.
6. **CMYK post-process**: `dotnet run ... -- --pdf-cmyk` (or include flag in Mode). Operates on the
   existing bundle, no re-harvest. Cwd must be the exe dir (`bin/.../net9.0-windows/`).
7. **Per-language verification** (§7) — geometry, content, RTL/CJK, no FR leak.
8. **Visual verdict = ai-01** (po-2023 signals counts/hashes, never declares PASS).

**ONE `dotnet run` at a time** + `kill javaw` between runs (FreeMind mutex; `KillAllFreeMind`
deliberately kills all `javaw`).

---

## 4. Cache-clobber — MANDATORY before fresh harvest (stale-harvest lesson)

**There is NO `--clean` CLI flag** (zero matches in `Generation/`). Cleaning is PowerShell-only.

- **Cache location**: `<cwd>/Target/<lang>/Harvest/<CardSetName>_harvest_<lang>.json`
  (`CardSetConfig.GetHarvestSerializationName`, `CardSetConfig.cs:26-29`; `HarvestDirectoryName =
  @"Harvest\"` at `AssetConverterConfig.cs:307`).
- **Cache-skip logic**: `HarvestManager.LoadHarvestsAsync` (`HarvestManager.cs:114-135`) — if the
  `.harvest.json` exists, it short-circuits with a lazy loader; `ProcessLocalizedHarvest`
  (`:147-168`) early-returns on existing key. **An existing harvest is authoritative → Playwright
  re-harvest is skipped entirely.**
- **`dotnet clean` ≠ clean regen** — `dotnet clean` only clears build artifacts, NOT harvest cache
  (lesson [[feedback-stale-harvest-regen]]). You MUST clobber harvests explicitly.

**Clobber commands** (from existing scripts):
- `prepare-environment.ps1:41-49` deletes `*.harvest.json`; `:51-55` clears `Harvest\`.
- `cleanup-output.ps1:5` deletes both `*.pdf` and `*.harvest.json`.

**Pre-regen clobber recipe**:
```powershell
# From the exe dir (bin/.../net9.0-windows/)
Get-ChildItem -Recurse -Filter *.harvest.json Target | Remove-Item -Force
# Optionally clear whole Harvest dirs:
Get-ChildItem -Recurse -Directory -Filter Harvest Target | Remove-Item -Recurse -Force
```

---

## 5. FreeMind GUI — RDP live foreground (the fragile part)

All in `Mindmapper/FallacyMindMapDocumentConfig.cs`. This stage **fails on a non-interactive
desktop**.

- **Path resolution**: `config.FreeMindPath` or env `ARGUMENTUM_FREEMIND_PATH`, else skip with
  warning (`:559-568`). Default empty (`AssetConverterConfig.cs:280`) → **must be set**.
- **Process launch** (`:587-592`): `Process.Start(UseShellExecute=true)` on the `.mm` file.
- **Desktop attachment**: `TryAttachToInteractiveDesktop` (`:360-390`) — `OpenInputDesktop` +
  `SetThreadDesktop` to reach `WinSta0\Default`. Needed because `Service-0x0-3e7$\Default` has NULL
  foreground.
- **Foreground forcing**: `ForceForeground` (`:406-434`) — `AttachThreadInput` + `BringWindowToTop`
  + `SetForegroundWindow`, 5 attempts, verifies `GetForegroundWindow() == hWnd`.
- **Menu nav** (`:642-669`): `{ESC}` → `%f` → 8×`{DOWN}` → `{RIGHT}` → 12×`{DOWN}` → `{ENTER}`×3.
- **Cleanup between runs**: `KillAllFreeMind` (`:535-551`) kills ALL `javaw`;
  `ClearFreeMindAutoOpenedTabs` (`:502-533`) wipes `lastOpened=` and
  `mindmap_last_state_map_storage=` in `auto.properties` so FreeMind opens only the target `.mm`.

**RDP prerequisite** (authoritative: `docs/investigations/2026-06-21-mindmap-reliability-without-rdp.md`):
- `quser` showing "Actif" is **NOT sufficient** — the RDP client must keep foreground focus.
- Recipe: OS persistent-session via `tscon <session> /dest:console` (PR #569) keeps the desktop
  interactive after the RDP client disconnects.
- **XSLT fallback is PROSCRIBED** (#184) — if `TryAttachToInteractiveDesktop` fails, the pipeline
  silently produces XSLT SVGs; these must NOT be committed. Only Batik SVGs are acceptable.

**Current coverage gap**: historically 4 langs (FR/EN/RU/PT, 21 SVGs). PR #686 (`c2e51192`) made
**code 8/8 complete** (ar/fa/zh wiring) + delivered 5/8 Batik SVGs; **ar/fa/zh SVGs still need a
live RDP foreground window** (~3 min warm regen). Status: gated on jsboige RDP window + #686 merge
arbitrage.

---

## 6. OWL — mono-language caveat (be honest in release notes)

- **Entry**: `OwlGeneratorConfig` (`Ontology/OwlGeneratorConfig.cs:18`), dispatched at
  `AssetConverterConfig.cs:539` (Fallacy) + `:540` (Virtue). Default reads
  `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`.
- **Output**: `<Target>/<lang>/Ontology/argumentum.owl` (deletes old file first, `:89-92`).
- **⚠ Mono-language limitation**: `GetFallacyConcept` (`:253-260`) hardcodes **only `fr` and `en`
  literals** (`TextFr/TextEn`, `DescFr/DescEn`, `ExampleFr/ExampleEn`). The `language` param
  (`:76`) only changes the output *directory*, not the concept annotations. **RU/PT/ES/AR/FA/ZH
  are never read by the OWL generator even when present in the CSV.**
- **Release-notes implication**: OWL is **EN+FR bilingue only**, NOT 8-lang. This honest scoping
  must appear in release notes (per #688 index finding, reflected in po-2024's #690 coherence PR).

---

## 7. Per-language verification points

For each of the 8 languages, after regen:

| Check | How | Owner |
|-------|-----|-------|
| **PDF count** = 10 doc types | `ls Target/<lang>/Documents/` | worker (count) |
| **Image count** matches expectation | `ls Target/<lang>/Harvest/*.json` image keys | worker |
| **Geometry** (A0/A4 dimensions, Tarot/Poker sizes) | Playwright + vision | **ai-01** |
| **Content localized** (no FR leak, #216 guard) | spot-check cards | **ai-01** |
| **Cyrillic renders** (ru) | visual, no tofu | **ai-01** |
| **CJK renders** (zh) | visual, no tofu | **ai-01** |
| **RTL direction + mirror** (ar/fa) | visual, `dir` attribute | **ai-01** |
| **Mind-map SVG native** (not FR-clone) | content check | worker + ai-01 |
| **CMYK applied** (DeviceCMYK + OutputIntent) | `pdfimages -list`, Ghostscript verify | worker |

**Visual verdict = ai-01 lane only** (project rule). po-2023 signals hashes/counts, never PASS.

---

## 8. Entry points / how to invoke

- **Main**: `Program.cs:18` `Main(string[] args)`. Sets cwd to exe dir (`:23-24`), UTF-8 console.
- **Full regen** (no args): loads `AssetConverterConfig.json` (but `SkipConfigFile=true` → C#
  defaults win), calls `config.Apply()`. Runs whatever `Mode` is set.
- **Single stage**: no CLI flag per-stage — set `Mode` in C# defaults (`AssetConverterConfig.cs:36`)
  or JSON `Mode` field.
- **CMYK-only**: `--pdf-cmyk` (`Program.cs:373-396`) — sets `Mode = PdfCmykPostProcess` +
  `ForceReleaseParams = true`, no harvest, no PDF regen (operates on existing bundle).
- **Validators** (optional, pre-regen sanity): `--validate-taxonomy`, `--validate-owl`,
  `--validate-cards`, `--translation-coverage` (see `Program.cs:53-269`).

**Invoke (Release params for print-quality)**:
```bash
dotnet run -c Release --project "Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj"
# then CMYK pass:
dotnet run -c Release --project ... -- --pdf-cmyk
```
Release: PNG lossless, per-image CMYK conversion ON (but see CLAUDE.md oxymore — per-image CMYK is
a no-op for PNG; authoritative CMYK = the Ghostscript post-process). Force release params in Debug
via `ForceReleaseParams = true`.

---

## 9. Tests (verify no regression pre/post regen)

The `Argumentum Converters.sln` references only **AssetConverter** + **VisualTests**. The unit-test
project (`Argumentum.AssetConverter.Tests`) is **NOT in the .sln** → test by path.

```bash
# Unit tests (must test by path — not in sln)
dotnet test "Generation/Converters/Argumentum.AssetConverter.Tests/Argumentum.AssetConverter.Tests.csproj"
# Visual tests (Playwright + Verify snapshots)
dotnet test "Generation/Converters/Argumentum.AssetConverter.VisualTests/Argumentum.AssetConverter.VisualTests.csproj"
```

**Current baseline** (empirical, po-2024 2026-07-04): **578 pass / 1 known-fail (#133 OWLSharp
round-trip) / 5 skip / 584 total**, build zéro-warning (CS + NuGet audit, #587). Test counts must be
re-confirmed empirically (`dotnet test`), never copied from a report ([[test-counter-empirical]]).

---

## 10. Pre-regen readiness gate (checklist before launching)

- [ ] All jsboige content arbitrages locked (tag content = final).
- [ ] RDP live foreground window confirmed (§5) — `tscon /dest:console` or jsboige present.
- [ ] `FreeMindPath` / `ARGUMENTUM_FREEMIND_PATH` set to a working FreeMind install.
- [ ] Harvest cache clobbered (§4) — `*.harvest.json` removed from `Target/<lang>/Harvest/`.
- [ ] FreeMind `auto.properties` tabs cleared (§5) — `lastOpened=` / `mindmap_last_state_map_storage=`.
- [ ] `Mode` includes `Mindmapper | OwlGenerator` (§3).
- [ ] CardPen local IIS up (`UseLocalCardpen=true`, `http://argumentum.myia.io`) — Pages cannot
      serve `/Cards/` (#629/#657, post-tag).
- [ ] Unit tests green (§9) — no regression baseline.
- [ ] ONE `dotnet run` at a time; `kill javaw` between runs.
- [ ] Post-regen: reject any XSLT SVG (proscribed #184); only Batik committed.
- [ ] Per-language verification (§7) + visual verdict ai-01.

---

## Gate boundaries (HARD — PREP only)

- ❌ No regen launched in this dispatch (desktop non-interactive + content HOLD).
- ❌ No CSV mutation, no config mutation beyond documenting the harness.
- ❌ No visual PASS verdict declared (ai-01 lane only).
- ❌ Held PRs untouched: #674 `[runtime pending]`, #666 `[HOLD post-tag]`, #596 garde-fou — NE PAS
  merger.
- ✅ All evidence is read-only code=truth (file:line refs verified on master `21e2c666`).

Relates: dispatch `sde6s0` (primary), #134 (release), #26 (régén fraîche decision), #632/#652/#641
(CMYK), #636 §1/#686 (mindmap RDP/ar-fa-zh), #184 (XSLT proscribed), #216 (FR-leak guard),
[[feedback-stale-harvest-regen]], [[feedback-assetconverter-background-runkey]],
[[feedback-dotnet-run-buildserver-deadlock]].
