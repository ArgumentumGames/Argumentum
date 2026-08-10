# VisualTests — the release gate that runs outside CI (#1048)

> **Status:** validated (b) by ai-01 with a condition. The VisualTests project (`Argumentum.AssetConverter.VisualTests.csproj`) is **deliberately excluded from CI** — not as abandoned debt, but as a release gate **relocated** to the only machine where it can actually execute. This document is what makes that relocation honest: it records *why* CI cannot run them, *where* they run instead, and *what* is expected of them.

## Why they are not in CI (measured)

CI (`.github/workflows/build.yml`) compiles the whole solution but runs `dotnet test` **only** on `Argumentum.AssetConverter.Tests.csproj` (build.yml `Test` step). VisualTests is deliberately excluded — see the comment block in `build.yml` for the rationale, summarised here:

- VisualTests run **after** the pipeline, against a **populated `Target/`** directory: 9 000+ PNGs + generated PDFs, **5.30 GB** (measured). A fresh CI runner has none of this.
- Adding VisualTests to CI would therefore **fail every run** (no `Target/`) — red noise that hides real signal.
- Filtering with `continue-on-error` would be worse: a green check over tests that ran **zero** assertions on an empty `Target/` — the #909 *vert-pour-mauvaise-raison* failure mode, relocated to a separate project. Rejected deliberately (#963 / #957).
- The cold-start guards (`Assert.Fail("…test verified nothing…")`) make the harness **fail loud, never pass silent**, on a missing or incomplete `Target/`. Measured on a worker without a populated `Target/` (58 MB, 0 PDFs): `0 pass / ~37 fail-loud / 0 skip`. They are alive; CI is simply the wrong place for them.

## Where they run instead (the relocation)

On the **regeneration machine** — the one that produces the 5.30 GB `Target/` — **before tagging a release**. That is the only environment where the assertions have real artefacts to assert against. Concretely, as part of the release-validation dossier (see `docs/release-dossier/`):

1. After a full pipeline run produces a fresh, populated `Target/` (Debug or Release density directory).
2. Run locally:
   ```bash
   dotnet test "Generation/Converters/Argumentum.AssetConverter.VisualTests/Argumentum.AssetConverter.VisualTests.csproj" --verbosity normal
   ```
3. Record the **pass/fail/skip count** in the release-validation dossier (not merely "executed").

## What is expected of them

A **triplet** recorded in the release dossier:

> `dotnet test VisualTests` → N pass / N fail / N skip, run on `<machine>` against `Target/` dated `<YYYY-MM-DD>`, density `<N>`.

- **Not** "executed ✓" — a bare "executed" is the same hole as the #909 no-op. The count is what makes it a gate.
- A non-zero **skip** is acceptable *only* if each skipped test is named and the reason recorded (e.g. GUI/infrastructure dependency, dormant Magick.NET path). An unexplained skip count is treated as a failed gate.
- A non-zero **fail** blocks the tag until either fixed or triaged as a pre-existing known-fail (the one current known-fail is the OWLSharp round-trip bug tracked under #133, which is in `Tests`, not `VisualTests`).

## Licence gate — `Verify.ImageSharp` is AGPL-3.0-only, and this gate wakes it up

`Verify.ImageSharp 5.0.1` (referenced by `Argumentum.AssetConverter.VisualTests.csproj`) declares `<license type="expression">AGPL-3.0-only</license>` — strong copyleft. This was surfaced as a **pre-existing mislabel** (the doc previously read "MIT") by the #1051 licence re-verification; it was true at the prior version too. The correction is recorded in `docs/licensing/dependency-license-inventory.md` §2.

**State today (measured, #1051): dormant.** The only live `Verify` chain runs through `Verify.Xunit` (**MIT**) — `PdfSnapshotTests` serialises a metadata object to **text**, which produced the 12 committed `.verified.txt` baselines. The `Verify.ImageSharp` converter (PNG comparison) has exactly **one call site**, `FallacyCardTests.cs:108` (`await Verifier.Verify(imageBytes, "png")` in `[Fact] Render_NominalCard`), and **zero `.verified.png` baselines** (`git ls-files`): the AGPL converter has never compared an image. The package is never in CI.

**But this very gate changes that.** Relocating VisualTests to the release door (this document) means the first pre-tag run will execute `FallacyCardTests.Render_NominalCard` against a populated `Target/` — and for the **first time** the AGPL converter will compare a rendered card image, producing the first `.verified.png` to approve. Retiring the assertion now *because it is dormant* would delete the only pixel-level check at the exact moment we are deciding to wake it — the failure mode this project rejects everywhere else (#1051 → #1054).

**The licence decision therefore has a dated deadline, not a deferred one.** The moment to obtain jsboige's licence answer for `Verify.ImageSharp` is the **approval of the first `.verified.png`** — not before (the dependency costs nothing while dormant), and not after (approving the snapshot activates the AGPL dependency inside the release-validation chain). The operator who runs this gate the first time must, before approving that baseline, either (a) have a recorded jsboige decision to accept AGPL-3.0-only on a test-only path, (b) have swapped the converter for a permissive alternative, or (c) hold a commercial licence — and record which.

**Honest framing, not a reassuring one.** AGPL-3.0 attaches its obligations to **distribution of the work**. A test-only NuGet package that is never shipped is not distributed merely because the repository is public, so the risk is low — but "low" is not "none", which is why this section states a dated deadline rather than a closed conclusion. This does not block the (b) decision or the tag; it attaches a licence obligation to the first concrete artefact that triggers it.

## Coverage note (why this gate carries real weight)

The VisualTests suite covers properties of the **generated artefacts** that the unit-test project (`Tests.csproj`) does **not** cover: PDF dimensions (A0/A4/Poker/Tarot/Print&Play per language), page counts, minimum file sizes, footer-collision checks, and `Verify` snapshots. `Tests.csproj` covers the **assembly** logic (PdfAssembler, PdfDisposeContract, PrintAndPlayDocument). The two are complementary, not redundant — dropping VisualTests would lose the only release-gate on the dimensions and content of the actually-generated PDFs.

## Known gap (post-tag triage, #1046)

A self-audit of the test suite (#1046, 41 findings) flagged that the VisualTests cold-start guards cover *"`Target/` absent"* but **not uniformly** *"`Target/` present but a subtree is empty"* — 4 of the 6 `VisualQaHarness` detectors would pass green if their specific CardSet subtree produced no files. This is precisely the hole through which a false-green would return the day someone re-enables these tests in CI against a partial `Target/`. Named here rather than buried in the 41 findings, so it is addressed in the post-tag triage. It does **not** change the (b) decision above.
