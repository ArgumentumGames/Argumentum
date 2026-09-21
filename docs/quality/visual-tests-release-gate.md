# VisualTests — release validation outside CI (#1048)

## Current execution policy

The build workflow compiles `Argumentum.AssetConverter.VisualTests.csproj` but does not execute it. The documented replacement is a local release-validation run on the regeneration machine before the tag. This records the existing coordination policy, not a new owner decision or an approval of the release.

The suite has two kinds of fixtures:

- **Generated artefacts:** PDF dimensions, content, bundle integrity, snapshots, card images and geometric detectors require a populated converter `Target/`. A fresh checkout does not supply these files. Missing required inputs must fail loudly, not become a successful empty run.
- **Committed wrappers:** `MindmapWrapperTests` and `MindmapWrapperCapabilitiesTests` use repository fixtures and Chromium. Their execution on a checkout without generated `Target/` was demonstrated in #1048, and they are now executed by the dedicated workflow `.github/workflows/mindmap-wrapper-behaviour.yml` on every pull request to `master` and every push to `master` — the #830 automation decision, implemented 2026-09-11. That job runs only this subset (`--filter "FullyQualifiedName~MindmapWrapper"`); the suites provision Chromium themselves, and the job carries no `continue-on-error`.

Baseline recorded when the workflow was wired (2026-09-11, master `2ece0eb8`): **38 cases, 0 failed, 0 skipped, 2 m 56 s**. A red run means the committed-wrapper behaviour regressed; a change to that count is a change of instrument and belongs in this document.

Count changed 38 → **39** on 2026-09-11 (#830): `Cap8_ClickNode_AppliesFamilyClassAndColoursOverlay` instruments capability #8 (family colours — class applied to the overlay at click + computed non-white background, no colour guessed). Measured locally before push: **39 cases, 0 failed, 0 skipped, 1 min 14 s** (warm Chromium; a cold CI runner pays the install, as the wired baseline did).

Count changed 39 → **43** on 2026-09-14 (#830): two capabilities that had no direct assertion became falsifiable. `Cap5_ControlIcons_PresentClickableAndEffective` (2 cases, FR × 2 families) asserts the three control icons are not merely **present and clickable** but **effective** — each one moves the viewport; an inert control is present, visible, clickable and useless, so presence alone proves nothing. `Cap9_ResetAndZoomOutMaxAreDistinctStates` (2 cases) asserts that reset and the maximal zoom-out are **two distinct, strictly ordered states** whose ratio equals the template's **declared `minZoom`** (read from the fixture, not from the observation), refuting the golden-master's "reset = zoom-out max" equivalence. Falsifiability was proven by mutation: setting `minZoom: 0.15` to `1` turns Cap 9 red, after which `git checkout --` restores the fixture. Measured locally before push, on the exact CI filter (`--filter "FullyQualifiedName~MindmapWrapper"`): **43 cases, 0 failed, 0 skipped, 59 s** (warm Chromium).

Count changed 43 → **44** on 2026-09-20 (#1441): capability #2 (`Cap2_Recentring_CentersRootNode_InViewport`) was extended to cover a **right-to-left** language — its `InlineData` went from `fr, zh` to `fr, ar, zh`, so RTL is no longer carried by the live verdict pass alone. That merge (`866d65fa`) **did not update this register**, and the line above still read 43; the drift was found by measurement on 2026-09-21, not by reading. Attribution was mechanical, not inferred: `MindmapWrapperTests.cs` has not been written since 2026-09-11 (`ad7ceddf`), so its share of the count is unchanged, and the filter selects exactly two classes — `MindmapWrapperTests` (28 cases) and `MindmapWrapperCapabilitiesTests` (16). The +1 is the new `ar` row. Measured on 2026-09-21 (master `6694d702`, warm Chromium), on the exact CI filter: **44 cases, 0 failed, 0 skipped, 1 min 20 s**. The numeric reference for the 9 capabilities on that tree is in [`830-reference-numerique-9-capacites-2026-09-21.md`](830-reference-numerique-9-capacites-2026-09-21.md).

⚠️ **Cap 9 does not establish the fit.** Renamed from `…ResetLandsOnFit…` on review: the post-reset scale is the observed value, and no independent reference confirms it *is* the fit-to-viewport scale. A geometric derivation (viewport `getBBox()` against the container box) misses it by 1,0–1,5 % because svg-pan-zoom snapshots its sizes at construction; `getSizes().realZoom` would settle it but the wrapper keeps its instance in a closure. The claim is the distinction, not the landing point.

⚠️ **The "deferred reset" (~1 s) is not a measurement of this suite.** That figure comes from another harness (ai-01, #830 c.5651920638). Re-measured here on 2026-09-14 (fr, both fixtures, click → first CTM change) the delay is **12 ms** and **57 ms**, reset landing directly on its final value — which is also why the repo CI's previous fixed 300 ms read stayed green. `WaitForSettledAfterActionAsync`'s change-gate is kept as robustness against the failure class, not because this suite observes a ~1 s deferral.

Do not apply `continue-on-error` to make missing artefacts look like a passing release gate. Compilation alone does not establish that these assertions passed.

## Run against the intended artefacts

After the grouped regeneration, run the suite locally:

```bash
dotnet test "Generation/Converters/Argumentum.AssetConverter.VisualTests/Argumentum.AssetConverter.VisualTests.csproj" --verbosity normal --logger trx
```

Record the actual paths selected by the tests. The converter writes under `Generation/Converters/Argumentum.AssetConverter/bin/{Debug|Release}/net9.0-windows/Target/`, not `<repoRoot>/Target/`. A build configuration alone does not prove which artefacts a test reads. Debug and Release outputs are independent and may have different ages.

The dossier must identify the machine, code revision, generated inputs and run, artefact paths and dates, density, test filter if any, and the actual pass/fail/skip counts. A filtered run is evidence only for that subset, never the full suite.

Capture the test process's own exit code. In Bash, an unguarded `dotnet test ... | tail` reports the final command's exit code instead. Prefer no pipe, or capture `PIPESTATUS[0]` immediately. Retain the test report and read its counts rather than inferring them from an exit code.

## Release acceptance

- Record the real **pass/fail/skip triplet**, not merely “executed”. A crash or timeout without a complete report is an incomplete gate.
- Name every skipped test and its reason. Unexplained skips leave the gate unqualified.
- Failures block acceptance until corrected or explicitly dispositioned with their evidence and authority. Historical known-failure lists are not current exemptions.
- **Never approve baselines automatically.** Explain and measure a snapshot difference before accepting it. Compare PDF pagination with the current `PdfDeckCountContractTests` contract and establish composition separately. An old baseline is not sufficient authority for a new expected value.
- Detector output does not replace the coordinator's visual examination or the final human sign-off. The release dossier remains tracked in [#134](https://github.com/ArgumentumGames/Argumentum/issues/134).

The artefact checks complement the main suite's generation and assembly tests. Behavioural wrapper checks exercise the delivered viewer. Neither a passing build nor a source-text check substitutes for these executions.

## Historical evidence and resolved prerequisites

Measurements below describe their recorded runs, not the current suite size, runtime or release status. Re-measure before planning execution or quoting coverage.

| Date | Evidence recorded in #1048 | Scope |
|---|---|---|
| 2026-08-10 | Generated inputs measured at 5.30 GB | One historical output tree, not a fixed requirement |
| 2026-08-11 | 98 cases, 85 pass, 13 fail, about 1 h 33 | Before subsequent harness corrections |
| 2026-08-16 | 98 cases, 93 pass, 5 fail, 3 min 11 | Complete run after harness changes; not a release PASS |
| 2026-08-24 report | 38 pass, 72 fail, 0 skip on checkout without generated artefacts | 12 wrapper declarations passed; 30 artefact-dependent declarations failed loudly |
| 2026-08-30 | 47 declarations and 118 executable cases reported | Later inventory, not a new execution or current count |

- [PR #1069](https://github.com/ArgumentumGames/Argumentum/pull/1069), merged on 2026-08-12, replaced `Verify.ImageSharp 5.0.1` with a direct `SixLabors.ImageSharp 3.1.12` reference. The old conditional AGPL gate for the removed package is no longer applicable. This is removal, not approval of AGPL or a blanket licence clearance. Consult `docs/licensing/dependency-license-inventory.md` for the remaining dependencies.
- `FallacyCardTests` no longer performs the historical live CardPen harvest. The current source probes `bin/{Release,Debug}/net9.0-windows/Target/fr/Images/density-*/Fallacies`. The incorrect repository-root anchor introduced by #1072 was subsequently corrected.
- [Audit #1046](https://github.com/ArgumentumGames/Argumentum/issues/1046#issuecomment-5393124594) closed on 2026-08-24. The previously missing zero-scan guards are present in `VisualQaHarness`: each relevant detector fails on zero scanned images, and the full-grid test fails on zero total images. This resolves the documented empty-scan gap, not every possible incomplete-dataset defect.

The four measurement questions in #1048 are separate from final release acceptance. The decision to automate the committed-wrapper subset in CI was taken in #830 and is implemented by `.github/workflows/mindmap-wrapper-behaviour.yml`; it replaces neither the artefact-dependent release gate above nor the coordinator's visual verdict.
