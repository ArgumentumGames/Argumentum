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

## Coverage note (why this gate carries real weight)

The VisualTests suite covers properties of the **generated artefacts** that the unit-test project (`Tests.csproj`) does **not** cover: PDF dimensions (A0/A4/Poker/Tarot/Print&Play per language), page counts, minimum file sizes, footer-collision checks, and `Verify` snapshots. `Tests.csproj` covers the **assembly** logic (PdfAssembler, PdfDisposeContract, PrintAndPlayDocument). The two are complementary, not redundant — dropping VisualTests would lose the only release-gate on the dimensions and content of the actually-generated PDFs.

## Known gap (post-tag triage, #1046)

A self-audit of the test suite (#1046, 41 findings) flagged that the VisualTests cold-start guards cover *"`Target/` absent"* but **not uniformly** *"`Target/` present but a subtree is empty"* — 4 of the 6 `VisualQaHarness` detectors would pass green if their specific CardSet subtree produced no files. This is precisely the hole through which a false-green would return the day someone re-enables these tests in CI against a partial `Target/`. Named here rather than buried in the 41 findings, so it is addressed in the post-tag triage. It does **not** change the (b) decision above.
