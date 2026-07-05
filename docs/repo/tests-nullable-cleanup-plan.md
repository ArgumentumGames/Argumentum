# Tests/ project — nullable-cleanup plan (post-v0.9.0-tag, GATED)

**Scope**: a plan-only doc for cleaning up the **96 nullable-annotation + ancillary compiler
warnings** in the `Argumentum.AssetConverter.Tests` project. **GATED: no execution during the
v0.9.0 release freeze** — this is a post-tag tech-debt lane. Recorded now so the work is scoped
and ready when the freeze lifts.

**Author**: po-2024 (worker) · **Dispatch**: `awhj8g` (TERTIAIRE — "dette read-only : documente le
plan nullable-cleanup Tests/ post-release en `docs/repo/`") · **Base**: master `34c7702c`.

> **Status: PLAN ONLY.** No code change in this PR. The freeze-safe rationale (why not now) is §3.

---

## 1. Empirical inventory (code=truth on `34c7702c`)

`dotnet build Argumentum.AssetConverter.Tests.csproj -c Debug --no-incremental` emits **96 source-level
warnings** (CS/SYSLIB, excluding NU1xxx NuGet-audit warnings which live on the main converter
project, not Tests/). Breakdown by code:

| Code | Count | Meaning | Fix shape |
|------|------:|---------|-----------|
| CS8625 | 22 | null literal passed to non-nullable reference type | `?` the parameter/return, or assert non-null |
| CS8618 | 20 | non-nullable property uninitialized in constructor | `?`, `= null!`, or `required` |
| CS8602 | 16 | possible-null dereference | null-guard or `!` |
| CS8620 | 14 | null cannot convert to generic type parameter | `?` constraint or default |
| CS8600 | 6 | null literal to non-nullable conversion | `?` or assert |
| CS8619 | 4 | nullable reference type flow mismatch | align annotations |
| CS8604 | 4 | possible-null argument to non-nullable param | null-guard or `?` |
| CS0108 | 4 | member hides inherited member | add `new` keyword |
| CS8603 | 2 | possible-null return | `?` the return |
| CS0219 | 2 | variable assigned but never used | delete the variable |
| CS8605 | 2 | unboxing a maybe-null value | null-guard |

**By category**: 90 nullable-flow (CS86xx), 4 member-hiding (CS0108), 2 dead-store (CS0219).

### Files most affected (top 8 of 20)

| File | Warnings |
|------|---------:|
| `UpdateTableFromRecordsTests.cs` | 20 |
| `PdfAlternateFaceAndBackContractTests.cs` | 14 |
| `EqualityComparerFactoryContractTests.cs` | 6 |
| `InterpolateCacheTests.cs` | 6 |
| `FallacyLinkFallbackTests.cs` | 6 |
| `MmGeneratorTests.cs` | 6 |
| `ScribanGeneratorTests.cs` | 4 |
| `CsvBaseStrictContractTests.cs` | 4 |

20 files total. The top 2 (`UpdateTableFromRecordsTests` + `PdfAlternateFaceAndBackContractTests`)
account for **34/96 (35%)** — start there for the biggest chunk.

---

## 2. Recommended approach (to execute post-tag)

**NOT one giant PR.** Three slices, smallest-blast-radius first, each a separate PR with the test
suite re-run after:

1. **Slice A — mechanical wins (CS0108 + CS0219, 6 warnings).** Add `new` keyword on the 4 hiding
   members (CS0108 — intention is always to hide in test doubles); delete the 2 dead stores
   (CS0219). Zero behavioral risk. ~10 min. 1 PR.

2. **Slice B — constructor-init non-nullable properties (CS8618, 20 warnings).** These are test
   fixture/entity classes where a property is set by the test harness or a serializer post-construction.
   Two honest fixes: `?` (property genuinely nullable) or `= null!` / `required` (property is
   logically required but set later — the test-fixture idiom). Case-by-case, but mostly
   `= string.Empty` for strings / `= null!` for objects. Low risk. 1 PR per ~2-3 files.

3. **Slice C — nullable-flow in assertions/arrange (CS86xx, 70 warnings).** The bulk. Test code
   routinely passes `null!` or `default` into a non-nullable param *on purpose* to test null-handling
   — the right fix is `?` on the SUT param (if the SUT genuinely accepts null) OR an explicit
   `null!` / `Assert.Throws<ArgumentNullException>` arrange (if the test is verifying null
   rejection). This needs the most judgment: each warning is a 30-second decision, but ×70.
   File-by-file, 1 PR per file (or per cohesive group). **Re-run the full test suite after each
   PR** — a wrong `?` annotation on an entity can silently change CsvHelper mapping behavior (the
   SUT-under-test), which is exactly the fragile area flagged in CLAUDE.md (CsvHelper ClassMaps).

**Do NOT** enable `<Nullable>enable</Nullable>` project-wide as a shortcut — that would surface
*hundreds* of new warnings across the SUT (non-test) code and explode the blast radius. Keep the
cleanup scoped to Tests/ via targeted annotations.

**Do NOT** use `#pragma warning disable CS86xx` as a blanket suppression — it hides the signal.
Reserve pragma for the rare genuinely-unsolvable case (e.g. reflection-driven init), documented
inline.

---

## 3. Why not during freeze (gate rationale)

- **Risk of silently changing test behavior.** A nullable annotation on a test entity (`Fallacy`,
  `Rule`, etc.) changes what CsvHelper considers a "missing field" vs a "null field" — the exact
  fragility CLAUDE.md documents (`MissingFieldFound` vs `HeaderValidated`). A wrong annotation
  during freeze could turn a passing test into a false-pass (warning suppressed, behavior changed)
  right when we need the suite most stable.
- **Low value during freeze.** The 96 warnings are pre-existing, do not block the build (the
  converter project, not Tests/, is the zero-warning target — #587), and do not affect generated
  assets. Cleaning them now buys nothing for v0.9.0.
- **Sequencing.** Best done *after* the tag, alongside or after the SGEN/MSB3073 tooling warning
  deferred from PR #706 — both are "test/build hygiene" lanes that can be batched in one post-tag
  tech-debt window.

---

## 4. Estimated effort (post-tag)

| Slice | Warnings | PRs | Effort |
|-------|---------:|----:|--------|
| A (CS0108 + CS0219) | 6 | 1 | ~15 min |
| B (CS8618 constructor-init) | 20 | ~3 | ~1 h |
| C (CS86xx flow) | 70 | ~8-10 | ~3-4 h |
| **Total** | **96** | **~12-14** | **~5 h** |

Spread across the post-tag window (one slice per idle tick), not a single session. The 96→0 target
makes the Tests/ project zero-warning, complementing #587 (converter project) and #706 (SYSLIB0014
fix, merged) — at which point the whole solution builds clean at `TreatWarningsAsErrors` if desired.

---

## 5. Companion: the SGEN/MSB3073 warning (deferred from PR #706)

Also a post-tag lane (flagged in PR #706's description but not fixed there):

```
MSB3073 + SGEN: "Failed to generate the serializer for Argumentum.AssetConverter.dll"
(from Microsoft.XmlSerializer.Generator)
```

This is a **build-time tooling warning**, not a source-level CS warning — it fires during the
`Microsoft.XmlSerializer.Generator` targets. Fix path: review whether the
`Microsoft.XmlSerializer.Generator` package is even needed (if no code uses `XmlSerializer` pre-
compilation, remove the package); if needed, fix the type that fails to serialize. Separate from
the Tests/ nullable cleanup above — bundle both in the same post-tag tech-debt window.

---

## Gate boundaries (HARD — plan only)

- ❌ No code change in this PR (plan doc only). No build, no test run.
- ❌ No `<Nullable>enable</Nullable>` project-wide flip (would explode blast radius — see §2).
- ❌ No execution during freeze (see §3).
- ✅ Plan is reproducible: `dotnet build Tests.csproj --no-incremental` re-derives the 96-warning
  inventory on any checkout at `34c7702c`.

Relates: dispatch `awhj8g` (TERTIAIRE), #587 (converter zero-warning), #706 (SYSLIB0014 fix +
SGEN deferral), #654 lane (SYSLIB0014 was the freeze-safe tech-debt lane this tick). Base `34c7702c`.
