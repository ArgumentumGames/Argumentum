# Regeneration — launching, scoping, validating

> **Read on demand — not loaded by default.** Read this before launching a regeneration, scoping it to one language, handing its output to a reviewer, or validating a card-template fix.
> Entry point and invariants: [`AGENTS.md`](../../AGENTS.md) · silent failures and the map of section titles: [`CLAUDE.md`](../../CLAUDE.md) · about this folder: [`README.md`](README.md).
> The sections below were moved here **verbatim** from `CLAUDE.md` on 2026-11-13 — headings and wording unchanged, so a citation such as *CLAUDE.md § Regenerating a SINGLE language* names a heading that exists here.
> In *Local CardPen is mandatory…*, *the table row above* is the `CardPen source` row of the Debug/Release table, now in [`build-modes-and-cmyk.md`](build-modes-and-cmyk.md).

### Regeneration runs — launch shell and path length (#1179, second bite of #1121)

Pipeline runs must launch from the **short junction `D:\A1114`** (→ `.prep-1114-worktree`), and only from **PowerShell or cmd**. Launched from **Git-Bash/MSYS2, the junction is resolved at process spawn**: the child's working directory becomes the full worktree path (~30 chars longer), and image writes whose path crosses the ImageMagick native buffer (MAX_PATH = 260) fail as `MagickCoderErrorException: WriteBlob Failed`. Two "identical" invocations therefore do NOT have the same effective path length — the shell is part of the repro. `ImageHelper.EnsurePathWithinLimit` now fails the run at 250 chars (before the native failure), naming the path and its length. Related hardening (#1179): a document×language couple producing zero images fails the run instead of silently skipping PDF generation (the #1177 defect), and the logger archives the previous run's `file_logger.log` to `file_logger-<timestamp>.log` instead of deleting it — the CMYK pass no longer erases the generation log.

### Regenerating a SINGLE language — the `LocalizationConfig.Enabled` lever (#1469)

`LocalizationConfig.Enabled = false` is the **only** lever needed to scope a regen to one language — nothing else has to be touched. `BuildLanguageList` then returns `[DefaultLanguage]` alone (`LocalizationConfig.cs:47-55`), which is consumed by `HarvestManager` (`WebBasedGenerator/Cardpen/HarvestManager.cs`, the `BuildLanguageList` call) and mirrored on the PDF side by `ImageFileGenerator`, which rebuilds the same list from `LocalizationConfig.DefaultLanguage` plus each document's `Translations`; the `CardSetDocuments` `DocumentName`s already carry the language suffix (`WebBasedGeneratorConfig.cs:451…`), so **all 10 `fr` documents come out without scoping any CardSet, document or dataset**. Restore `true` afterwards — this edit is a **recipe, never committed**.

Validated 2026-09-21 (#1469): throwaway worktree (`--detach origin/master`) reached through a **short junction** + **PowerShell**, `-c Release`, then `-- --pdf-cmyk` **on the same tree** — its `Target/` holds that language only, so the CMYK pass discovers only those PDFs.

- ⚠️ **Runs unattended** — no FreeMind, no GUI. Unlike the mindmap pass this needs no attended desktop, which is what makes it schedulable at all.
- ⚠️ **First run in a virgin `bin/Release`** auto-generates `AssetConverterConfig.json` → `Console.ReadKey()` → immediate death on a redirected console. Relaunch (the crash is what wrote the JSON); the control is the **log**, never the exit code.
- **Keep every document of the language.** The untouched ones are the **control pair** that *measures* a declared "no rendered impact" edit instead of asserting it — compare page signatures (`docs`/scratchpad instrument), not whole-file hashes.

**Measured barème** (#1469, 2026-09-21) — quote these with their nature:

| Poste | Mesure | Nature |
|---|---|---|
| Full single-language pass (harvest + images + assembly + PDF) | **23 min 15 s** (`fr`, 10 docs) | measured |
| Assembly only, 10 docs | 3 min 12 s | measured |
| Assembly only, 80 docs | 25 min 50 s | measured |
| CMYK post-process | ≈ 70 s/PDF (1 h 33 for 80) | measured |
| Harvest + images | ≈ 10.5 min/language (1 h 24 / 8) | **DERIVED** — not directly timed |

⇒ one language ≈ 30 min; all eight ≈ 3 h 25. A targeted pass therefore costs ~6× less than a full one, and the full pass is what buys the control pair.

⚠️ **The recipe above ends at the bundle, and the bundle is NOT what a reviewer opens.** A regen produces **three** objects, and the third one does not move on its own:

| Objet | Où | Qui l'écrit |
|---|---|---|
| Build tree | `bin/{Debug,Release}/net9.0-windows/Target/` | the run |
| Review bundle | `G:\Mon Drive\Argumentum\review-<tag>-<date>\` | the operator, by copy |
| **Published assets** | **release `v2.0.0-review`, 80 files** | **nobody — unless it is done explicitly** |

Measured 2026-09-21: the three `fr` PDFs fixed by #1469 landed in the bundle at 17:00, and **four hours later the published assets still carried the pre-fix bytes** (`updatedAt` 18/09). A reviewer opening the kit in that window read the defect the fix had already removed. Remedy in one line, from **PowerShell** (`G:` is invisible to MSYS/Git-Bash — a bash `ls` shows the directory and none of its files):

```powershell
gh release upload v2.0.0-review "<bundle>\<lang>\<file>.pdf" --clobber   # 244 MB → 19 s
```

⚠️ GitHub **rewrites `&` to `.`** in asset names: `Print&Play` on disk is `Print.Play` once published. `--clobber` matches the **published** name, so the replacement lands correctly — but a script that greps the on-disk name against the asset list finds nothing and reads it as "asset absent".

**The control is the byte size, not the timestamp** — the corrected and pre-fix PDFs differ by a few hundred bytes, so `gh release view --json assets` compared against the bundle is a one-call discriminator. ⭐ And the general form, which has now cost two cycles: *a validated artefact and the channel that serves it are two different objects* — the verdict is only worth what the **recipient's link** shows.

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

⚠️ **"Extract the `css` key" means PARSE the JSON, not grep the file** — and the difference silently inverts results (measured 2026-09-21, [#1485 c.5767568615](https://github.com/ArgumentumGames/Argumentum/issues/1485#issuecomment-5767568615)). A template's CSS lives inside a JSON **string**, so on disk every quote is escaped: a rule written `src:url("…/BigJohnPRO-Bold.otf")` is stored as `src:url(\"…\")`. A regex run over the **raw bytes** stops at the backslash and yields an **empty capture** — which reads as *"this declaration is empty"* when the declaration is in fact full. One font declaration was published as measured-empty on that basis and had to be retracted; parsed correctly it carries three `url()` entries. ⛔ The retracted figures are deliberately not repeated here — a remedy that quotes the defect re-indexes it; #1485 holds the detail. The rule generalises: ⭐ *a `0` is only an absence if the instrument could have seen a `1`.* Parse with `json.load`, walk to the `css` key, **then** apply the regex — and commit an inverse control that injects the pattern you claim is absent and asserts it is found.
