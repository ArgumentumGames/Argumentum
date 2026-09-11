# VisualTests — release validation outside CI (#1048)

## Current execution policy

The build workflow compiles `Argumentum.AssetConverter.VisualTests.csproj` but does not execute it. The documented replacement is a local release-validation run on the regeneration machine before the tag. This records the existing coordination policy, not a new owner decision or an approval of the release.

The suite has two kinds of fixtures:

- **Generated artefacts:** PDF dimensions, content, bundle integrity, snapshots, card images and geometric detectors require a populated converter `Target/`. A fresh checkout does not supply these files. Missing required inputs must fail loudly, not become a successful empty run.
- **Committed wrappers:** `MindmapWrapperTests` and `MindmapWrapperCapabilitiesTests` use repository fixtures and Chromium. Their execution on a checkout without generated `Target/` was demonstrated in #1048, and they are now executed by the dedicated workflow `.github/workflows/mindmap-wrapper-behaviour.yml` on every pull request to `master` and every push to `master` — the #830 automation decision, implemented 2026-09-11. That job runs only this subset (`--filter "FullyQualifiedName~MindmapWrapper"`); the suites provision Chromium themselves, and the job carries no `continue-on-error`.

Baseline recorded when the workflow was wired (2026-09-11, master `2ece0eb8`): **38 cases, 0 failed, 0 skipped, 2 m 56 s**. A red run means the committed-wrapper behaviour regressed; a change to that count is a change of instrument and belongs in this document.

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
