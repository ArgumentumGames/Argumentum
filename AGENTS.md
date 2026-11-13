# AGENTS.md — entry point for any agent working in this repository

This file is written for an agent (or a person) who sees **only this git repository**: no shared dashboard, no machine memory, no chat history. It is the **single source** for what follows. Claude Code loads it through the `@AGENTS.md` import at the top of [`CLAUDE.md`](CLAUDE.md); other tools read it natively. Do not copy its content elsewhere — link to it.

It covers what the repository is, how to build and test it, the invariants that have each already cost a defect, and how to contribute a change that can be merged. The deep pipeline knowledge (regeneration, mind maps, CMYK, release) lives in [`docs/agents/`](docs/agents/), one topic per file, read on demand and indexed in §8; [`CLAUDE.md`](CLAUDE.md) keeps the table of the pipeline's silent failures.

## 1. What this is

Argumentum is an educational card game about logical fallacies (sophisms). The repository contains:

- A .NET pipeline (`Argumentum.AssetConverter`) that transforms CSV data into print-ready PDFs, mind maps, and OWL ontologies
- A customized fork of CardPen (HTML card renderer using Mustache/Handlebars templates)
- Source data (CSV files) for fallacies taxonomy, game scenarios, rules, and virtues
- A DNN web platform export (partial, keys excluded)

The game ships in 8 languages (`fr` default, `en`, `ru`, `pt`, `es`, `ar`, `fa`, `zh`). The repository is **public** and dual-licensed: code under LGPL-3.0 ([`LICENSE`](LICENSE)), an enumerated list of corpus files under CC BY-SA 4.0 ([`LICENSE-CONTENT.md`](LICENSE-CONTENT.md)).

## 2. Map

| Path | Tracked files | What it holds |
|---|---:|---|
| `Cards/` | ≈ 800 | **Source of truth for content**: the 5 dataset CSVs, card templates (`*.json`), assets, committed mind maps (`Cards/Fallacies/Mindmaps/`), printed reference (`Cards/*/Archive/`) |
| `Generation/Converters/` | (in ≈ 830) | The pipeline: `Argumentum.AssetConverter` (main), `.Tests` (xUnit), `.VisualTests` (Playwright) |
| `Generation/CardPen/` | (in ≈ 830) | The CardPen fork. **A merge to `master` deploys it to GitHub Pages** (`static.yml`) |
| `tools/` | ≈ 150 | Python instruments, mostly named `<issue>-<purpose>.py`, some with a `test_*.py` |
| `docs/` | ≈ 860 | Dated findings, by area: `taxonomy/`, `corpus/`, `translation/`, `quality/`, `dnn*/`, `ontology/`, `release-dossier/`, `investigations/` |
| `DNNPlatform/` | ≈ 10 200 | Partial export of the website (DNN + 2sxc). Editing it deploys nothing |
| `.github/workflows/` | 6 | CI — see §6 |
| `.claude/skills/` | 5 | Claude Code session entry points (`coordinate`, `pipeline-*`); readable by anyone |
| `scripts/` | 3 | `triage/` (GitHub state instruments), `regen/` (preflight) |

Counts measured 2026-11-13 on `92076717`; re-measure with `git ls-files | cut -d/ -f1 | sort | uniq -c | sort -rn` rather than quoting them. The repository root also carries a few historical stray files (`LoggerTest.cs`, `Program.cs`, `yeh-*.png`…) that no solution references — do not add to them.

## 3. Build, test, run

```bash
# Run the full generation pipeline
dotnet run --project "Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj"

# Run tests (xUnit)
dotnet test "Generation/Converters/Argumentum.AssetConverter.Tests/Argumentum.AssetConverter.Tests.csproj"

# Run visual tests
dotnet test "Generation/Converters/Argumentum.AssetConverter.VisualTests/Argumentum.AssetConverter.VisualTests.csproj"

# Build all projects
dotnet build "Argumentum Converters.sln"
```

- **.NET 9 SDK, Windows.** The three projects target `net9.0-windows` (the converter uses WinForms). On a Linux or macOS sandbox, expect no local build (not measured here): **CI is your build** — both configurations run on `windows-latest` for every pull request.
- **What runs anywhere** (Python ≥ 3, std-lib only, offline, no artefact needed) — the three suites CI runs on every PR:

  ```bash
  python tools/test_1460-structure-charpente-pilot.py
  python tools/dnn_i18n/test_roundtrip.py
  python tools/test_458-eav-export-guards.py
  ```

- **Running the generation pipeline needs a maintainer workstation** (local CardPen host, Chromium, FreeMind for mind maps, Ghostscript for CMYK). From a sandbox, do not try: make the change, say in the PR which regeneration it calls for.
- `dotnet test` prints in the machine's language (`Réussi!` on French workstations): a `grep "Passed!"` can come back empty on a green run. Keep the exit code and the full output.

## 4. Invariants — each one has already cost a defect

**Configuration and code**

- **Code = truth.** The C# property initializers in `AssetConverterConfig.cs` *are* the configuration. `AssetConverterConfig.json` is generated, git-ignored and **not read** (`SkipConfigFile = true`, deliberate). **Never edit the JSON; regenerate it from correct C#; never "fix" the flag to `false`.**
- Every `DataSetInfo` needs its `CsvType`; never set `RowsetNb` when the template expects one CSV row per card; **never transform CSV content before it is injected into CardPen**. Each of these yields empty cards without an error.
- QuestPDF (`2022.12.12`) and AutoMapper (`14.0.0`) are **licence-pinned**: a Dependabot bump of either is closed, not merged. QuestPDF is also not thread-safe (global lock).
- Generated files go to `Generation/Converters/Argumentum.AssetConverter/bin/{Debug|Release}/net9.0-windows/Target/`, never to the repository root; the Debug and Release trees are independent.
- A **Release** render downloads card templates from `master`: a template fix living on a branch is invisible to it. Validate template fixes in Debug.

**Datasets (the 5 live CSVs under `Cards/`)**

| File | BOM | Row terminator | Note |
|---|---|---|---|
| `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` | yes | CRLF | bare LF **inside** quoted cells |
| `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` | yes | CRLF | |
| `Cards/Rules/Argumentum Rules - Cards.csv` | yes | CRLF | bare LF inside quoted cells |
| `Cards/Rules/Argumentum Rules - Cards Print and Play.csv` | yes | CRLF | bare LF inside quoted cells |
| `Cards/Scenarii/Argumentum Scenarii - Cards.csv` | **no** | CRLF | |

- **Edit surgically, byte-safe**: read and write with `newline=''`, touch only the cells you mean to change, keep BOM and terminators as found. Then `git diff --numstat -- <file>` must show **the number of rows you changed**. A figure close to the file's row count means the line endings were rewritten: redo the edit, do not ask a reviewer to read that diff.
- **One open pull request per dataset CSV at a time.** Check before starting (`gh pr list --state open`, then `gh pr diff <n> --name-only`).
- An invariant on a text field holds on **all 8 language columns**: check every one — a reported defect is a probe, not a perimeter. Column naming differs per file (`text_fr` … `text_zh` in Fallacies, `Text`, `Text_en` … in Rules, `titre` / `title` / `title_zh` in Scenarii): read the header, do not guess.
- The Fallacies file is a **taxonomy** (one row per node); the **deck** is the subset whose `carte` column is non-empty, pinned in CI by `PdfDeckCountContractTests`. A figure about "cards" and a figure about "nodes" are different objects.
- `Cards/*/Archive/` and `Generation/CardPen/examples/` are the **printed reference**: read them, never modify them.
- Three Virtues families were renamed in August 2026; old names survive in frozen artefacts. A search for an old name returning nothing is an instrument failure, not an absence — read the current labels from the CSV. The prompts under `…/DatasetUpdater/Resources/` carry the family names too: a second referent to update on any rename.

**Measuring**

- **A `0` is an absence only if the instrument could have seen a `1`.** Parse a card template with a JSON parser before applying a regex (its CSS is an escaped JSON string); mind-map thumbnails are inline base64, not paths; 424 tracked files match a `.gitignore` rule and are **silently skipped by ripgrep-based search** — use `git grep`.
- A sha256 taken on a working-tree file is a property of the machine that checked it out (line endings). Hash the blob: `git show <ref>:<path> | sha256sum`.
- A count found in a document is re-measured, never copied. In anything you publish, say what each figure is: **measured**, **derived**, or **assumed** — and name the command that re-measures it.

## 5. State lives in the issues, not in the documents

- The status tables and the "Prochaines étapes" list in `docs/agents/status-and-history.md` are **dated documentation**. Before acting on any item: `gh issue view <N> --json state,stateReason`, then read the body **and every comment** — the latest comments routinely supersede the body.
- `scripts/triage/dispatch-freshness.sh <N> [<N>…]` prints, per issue, the state and the last three comments. `scripts/triage/human-requests.sh` lists new requests written by humans. Both need `gh`.
- **`author.login` tells you nothing**: every maintainer lane writes through the same GitHub account. Who did what is carried by the **signature line** (§6), and a review arrives as a pull-request *comment* — `reviewDecision` stays empty by construction.
- #458 is the coordination roadmap. #2132 is reserved for the human reviewers: **agents do not post there.**

## 6. Contributing a change

Work is done by **lanes**: `ai-01` (coordination, merges), `po-2023` (site, regeneration, counter-reviews), `po-2024` (backlog, taxonomy), `web2` (production site operations). A cloud or one-off agent is its own lane and signs `cloud-<vendor>` (`cloud-codex`, `cloud-claude`…).

1. **Take one grain** — the task you were given, or one open issue. Do not widen it. Check for a collision first: open pull requests touching the same files, and the last comments of the issue.
2. **Branch** from the current `origin/master`: `<type>/<issue>-<slug>` (`fix/1471-ru-lot2`, `docs/2182-plan-sondes-execution`).
3. **Commit** in conventional form, the scope being the issue number or the component: `fix(1471): …`, `docs(2182): …`, `feat(i18n): …`.
4. **Before pushing**, read your own diff (`git diff --stat origin/master...HEAD`):
   - it contains only what the grain needs — no reformatting, no line-ending churn;
   - no secret, no machine path (`C:\…`, home directories), no IP address, no name of a private person or partner institution. Secrets live in `.env` / `.keys/` only, both git-ignored;
   - a durable finding goes to `docs/<area>/<issue>-<slug>-<YYYY-MM-DD>.md`, never to the repository root.
5. **Open the pull request** — one per grain. Title in conventional-commit form. Body in French (the maintainers' working language):
   - what changes and why, in a paragraph a maintainer can decide on;
   - the evidence: each figure with its nature (§4) and the command that reproduces it; for a fix, the control that **fails without it**;
   - what was **not** verified;
   - `Refs #N`. Write `Closes #N` only if the merge truly finishes the issue — the squash merge closes it on the spot;
   - **last line: the signature**, literally `*<lane>*`.
6. **You do not merge your own pull request.** The coordinator squash-merges once all of this holds on the **head commit**:
   - the four required checks are green — `build (Debug)`, `build (Release)`, `guard` (no new blob over 2 MiB outside the allow-list), `behaviour` (mind-map wrapper acceptance);
   - a lane **other than the author** has reviewed it, re-running the measurement against the source rather than trusting the description. A push after the review voids it: when you push again, comment what the delta changes and ask for the review again;
   - no unanswered change request or inline comment;
   - the body closes no issue it does not finish.

`master` is protected by a ruleset: pull request required, no force-push, no deletion, even for admins. A rejected push (`GH013`) means the protection works — open a pull request.

**Four readings this workflow has already made wrong**

- **The merge is a squash**: the commits of your branch never become ancestors of `master`, so `git merge-base --is-ancestor <your commit> origin/master` answers *no* for a change that landed. Check a landing with `gh pr view <N> --json state,mergeCommit` — or, for a file nobody else touched since, compare `git rev-parse origin/master:<path>` with `git rev-parse <your commit>:<path>`.
- **`gh pr checks <N>` exits `8` while checks are still running**: pending, not red. Wait for the run to end before you read the verdict.
- **A `gh` error `403` (rate limit) says nothing about the pull request**: the quota belongs to the account, and every lane draws on it. Do not retry in a loop — `gh api rate_limit` gives the reset time. Reading a pull request needs no API: `git fetch origin pull/<N>/head:refs/tmp/pr<N>` brings its head.
- **Reviewing a test, a gate or a guard by replaying the author's commands only shows what the author saw.** Disable the guard — or remove the line under test — and re-run: if the suite stays green, that is a finding.

## 7. Off-limits without an explicit maintainer decision

- Rewriting the **wording of the game rules**, or changing the content of an existing card. Typography (accents, quotation marks, spacing) is the exception: it is always corrected, in every language, without asking.
- Rebasing, closing or "fixing" someone else's open pull request. Long-lived open PRs are parked on purpose.
- Uploading, replacing or deleting **release assets**; tagging; publishing.
- Anything that acts on the live website or assumes a deployment. `DNNPlatform/web.config` on `master` is a placeholder template: never commit an expanded copy (`.githooks/pre-commit` guards it — activate with `git config core.hooksPath .githooks`).
- Any paid API call (translation runs through `DatasetUpdater` are gated on an owner decision and a named key).
- `@`-mentioning a human without a new fact for them; contacting anyone outside GitHub.

## 8. Where to read next

| You are about to… | Read |
|---|---|
| run any pipeline gesture | the table of silent failures in `CLAUDE.md` — one row per gesture, each naming its section |
| change pipeline configuration or a card set | `docs/agents/pipeline.md` § *Architecture*, `docs/agents/card-templates.md` § *Leçons Apprises* |
| regenerate cards or PDFs, one language or all | `docs/agents/regeneration.md` § *Regeneration runs*, § *Regenerating a SINGLE language* |
| touch Debug/Release behaviour or CMYK | `docs/agents/build-modes-and-cmyk.md` § *Debug vs Release Builds* |
| touch mind maps or their HTML wrappers | `docs/agents/pipeline.md` § *Pipeline Stages* (item 3), `docs/quality/visual-tests-release-gate.md` |
| edit a card template (`Cards/**/*.json`) | `docs/agents/regeneration.md` § *A branch fix to a card template…*, `docs/agents/card-templates.md` § *CSS Card Templates* |
| bump a dependency | `docs/agents/dependencies.md` § *Stable Dependency Versions* |
| quote a count, a status or a "next step" | `docs/agents/status-and-history.md` — dated snapshots: re-measure, and read the issue (§5) |
| publish a measure, review a pull request, dispatch work or close an issue | `docs/agents/method.md` — the rules, by theme; each one already cost a wrong conclusion here |
| run a git, `gh`, shell or CSV gesture you have not run here before | `docs/agents/hazards.md` — gestures that returned success while destroying work or misleading |
| move documentation from one file to another | `docs/agents/README.md` — the move is proven line by line with `tools/doc-move-preservation.py` |
| consolidate documents, a folder of findings or overlapping tools | `docs/agents/consolidation.md` § *Definition of done* — eight conditions, and a folder is held by an index of record |
| add a Python test to CI | header of `.github/workflows/python-tools-tests.yml`, `docs/quality/1460-charpente-suite-regression.md` |
| understand the pipeline end to end | `Generation/Documentation/ARCHITECTURE_PIPELINE.md` |
| run a coordination or worker session (Claude Code) | `.claude/skills/coordinate/SKILL.md`, `.claude/skills/pipeline-recovery/SKILL.md` |
