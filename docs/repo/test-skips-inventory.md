# Test-suite debt — 5 skipped tests inventory (#133-adjacent, read-only)

**Scope**: read-only inventory of the **5 skipped tests** in the AssetConverter test suite, dispatched
as idle-de-secours (`ynv05a`: "review #133-adjacent test skips"). Classifies each skip as legitimate
(infrastructure-gated) vs deferred-coverage-gap, and flags the one deferred-bug skip for post-tag
investigation. **GATED: no code change in this PR** (release freeze; this is an inventory, not a fix).

**Author**: po-2024 (worker) · **Dispatch**: `ynv05a` (idle-de-secours, ai-01 2026-07-06 01:32)
**Base**: master `bdba45d8` · **Read-only**: `dotnet test` + read test sources, 0 write.

---

## TL;DR — classification

Empirical `dotnet test` on `bdba45d8`: **593 total = 587 pass / 1 fail / 5 skip**. The 1 fail is the
permanent known-fail `OwlE2EGenerationValidationTests...XmlRoundTrip` (OWLSharp round-trip bug, tracked
#133 — does not affect generated assets). The **5 skips** split cleanly:

| # | Test (FQN) | Skip reason | Class | Verdict |
|---|-----------|-------------|-------|---------|
| 1 | `GSheetSync.FormulaAuditTests.Audit_All_Spreadsheets_Formulas` | `"Manual only — requires OAuth + network access"` | infra-gated | ✅ legitimate |
| 2 | `GSheetSync.FallaciesPullTests.Compare_Fallacies_GDrive_vs_Local` | same | infra-gated | ✅ legitimate |
| 3 | `GSheetSync.RulesPullTests.Pull_Rules_GDrive_Baseline` | same | infra-gated | ✅ legitimate |
| 4 | `GSheetSync.RulesPullTests.DryRun_Rules_GDrive_Baseline` | same | infra-gated | ✅ legitimate |
| 5 | `MindmapGeneration.SvgConversionIntegrationTests.TryAutomateSvgConversion_WithValidMmFile...` | `"Temporairement désactivé pour isoler le crash Magick.NET"` | **deferred-bug** | ⚠ **flag — post-tag** |

**4/5 are legitimate** (OAuth + network, see §1). **1/5 is a deferred-bug skip hiding a Magick.NET
crash** in the SVG-conversion test path — skipped 11 months ago under a "temporary" label, see §2.

> **Test-count drift note**: CLAUDE.md snapshots "584 total (578 pass / 1 fail / 5 skip)" at
> 2026-07-05; the empirical count on `bdba45d8` is **593 total (587 pass / 1 fail / 5 skip)** — 9 new
> passing tests added by recent merges (#715 VirtueMindMap native-script test + others). The skip
> count (5) and fail count (1) are unchanged. (Memory `[[test-counter-empirical-dotnet-test]]`:
> counters are empirical, never copied from a doc.)

---

## 1. The 4 GSheetSync skips — legitimate (infra-gated)

All four are hardcoded `[Fact(Skip = "Manual only — requires OAuth + network access")]` and read
OAuth credentials from a **jsboige-machine-specific path**:
`G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\GSheet-OAuth-Credentials.txt` (+ refresh token). They
exercise the live Google Sheets sync (PR #200, #193) — end-to-end network calls against the real
spreadsheets. CLAUDE.md already records this as "Pending: OAuth credentials for end-to-end testing".

**Why legitimate**: these cannot run in CI (no credentials, no network to GDrive) and intentionally
so — they are manual smoke-tests for the GSheet sync, run by jsboige locally when validating #193.
They are not coverage gaps; the GSheet sync logic is covered by 77 unit tests (CsvDiffEngine,
SyncSafetyChecker, DiffReport, CsvToGrid) that DO run in CI.

**Minor post-tag suggestion (NOT a fix)**: the four use *unconditional* `[Fact(Skip=...)]`, so they
never run even when the credentials file is present on jsboige's machine via `dotnet test`. A
conditional skip (run iff `File.Exists(CredentialsPath)`) would let them execute locally while staying
skipped in CI — same pattern as the standard `RequiresX()` test-gating. Low priority; flagged for the
post-tag test-hygiene window alongside #710.

---

## 2. ⚠ The Magick.NET deferred-bug skip — flag for post-tag investigation

`SvgConversionIntegrationTests.TryAutomateSvgConversion_WithValidMmFile_ShouldReturnTrueAndCreateSvgFile`
is the only skip that is **not** infra-gated. Its skip reason — `"Temporairement désactivé pour isoler
le crash Magick.NET"` ("temporarily disabled to isolate the Magick.NET crash") — hides a **real bug**:

- **Provenance**: skipped in commit `6bd802f4` (2025-08-01, "Stabilisation complète de la suite de
  tests") — **~11 months ago**, under a "temporary" label that is now stale.
- **What it tests**: `FallacyMindMapDocumentConfig.TryAutomateSvgConversion` (a private method,
  invoked via reflection) — the **Freeplane-process SVG conversion** path. It writes a minimal valid
  `.mm` file and asserts the conversion produces a non-empty `.svg`.
- **The crash**: the test isolates a **Magick.NET crash** somewhere in the SVG-conversion path.
  Magick.NET (14.14.0) is the image-processing lib; the SVG conversion uses it (likely for
  rasterization / post-processing of the Freeplane output).

### Why this deserves post-tag attention

The production mind-map SVG path **switched to FreeMind Batik** in April 2026 (PR #565 `55c6774e`,
CLAUDE.md "Mind Maps & SVGs — April 2026 COMPLETE"). This raises a real question the skip is hiding:

- **(a) Is the Freeplane `TryAutomateSvgConversion` path still used in production?** If Batik fully
  superseded it, the test may be **dead code** (testing a path no longer on the critical path) — in
  which case the fix is to delete or retarget the test to the Batik path, not to re-enable it.
- **(b) If the Freeplane path is still a fallback** (CLAUDE.md notes Freeplane GUI automation via
  `SendKeys.SendWait` is "VALIDATED", commit `46d6cd9b`), the Magick.NET crash is a **latent bug**
  that should be reproduced and fixed before it bites a production régéneration.
- **(c) Test-isolation artifact?** The crash may be a test-harness issue (Magick initialization under
  the reflection-invoked private method) rather than a production bug — only reproducing it settles
  this.

**Recommendation for the post-tag window** (bundled with #710 nullable-cleanup + SGEN/MSB3073 lane):
un-skip the test, reproduce the Magick.NET crash, and resolve per (a)/(b)/(c) above. This is the
single actionable item from the skip inventory — it converts an 11-month "temporary" skip into either
a fixed test, a retargeted (Batik) test, or a documented deleted test.

---

## 3. Reproducibility

`dotnet test Argumentum.AssetConverter.Tests.csproj` on any checkout at `bdba45d8` reproduces the
5-skip / 1-fail / 587-pass breakdown. The skip reasons are visible in the test runner output
(`Ignoré <FQN>` lines) and in the source (`[Fact(Skip = "...")]` attributes at the paths cited in §1/§2).

---

## Gate boundaries (HARD — read-only inventory)

- ❌ No code change, no test un-skip, no build config change in this PR (release freeze).
- ❌ No execution of the skipped tests (the Magick.NET crash is **referenced**, not reproduced here).
- ✅ Inventory derived code=truth from `dotnet test` output + test source reads + git blame (provenance).
- ✅ The single actionable finding (Magick.NET deferred-bug) is **flagged** for the post-tag window,
  not fixed here.

Relates: dispatch `ynv05a` (idle-de-secours), #133 (OWL known-fail — the only test fail, distinct from
the 5 skips), #710 (Tests/ nullable-cleanup — same post-tag lane), #565 (Batik SVG migration — context
for the Magick skip's (a)/(b)/(c) question), #193/#200 (GSheet sync — the 4 infra-gated skips).
Base `bdba45d8`.
