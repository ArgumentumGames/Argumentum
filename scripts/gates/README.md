# scripts/gates — executable merge gate

`pr_merge_gate.py` mechanises the checklist a merger runs before `gh pr merge`.
Python 3, standard library only, reads GitHub through `gh api`. It is
**read-only**: it never merges, never comments, never writes to GitHub.

```bash
python scripts/gates/pr_merge_gate.py 1234                      # live read
python scripts/gates/pr_merge_gate.py 1234 --expect-head <sha>  # pin the head you reviewed
python scripts/gates/pr_merge_gate.py --self-test               # offline, no network
```

## Exit codes

| Code | Verdict | Meaning |
|---|---|---|
| `0` | PASS | No HOLD, no BLOCK. The identity-bound, head-pinned merge command is printed (never run). |
| `3` | HOLD | Something must be read, confirmed or waited for. Not a failure. |
| `2` | BLOCK | Do not merge. |
| `1` | blind | API failure, incomplete pagination, missing core data, bad arguments. **Never a verdict.** |

A failed read is never rendered as PASS nor as "nothing to report".

## Rules

Verdict = worst level. BLOCK > HOLD > INFO.

| ID | Checks | BLOCK | HOLD |
|---|---|---|---|
| R1 | PR state | not open, draft, base other than `master` | |
| R2 | pinned head | `--expect-head` differs from the head | |
| R3 | merge state | `dirty`, or GitHub says not mergeable | `unknown`; `blocked` when neither R4, R5 nor R6 shows why (`behind`, and `blocked` with its cause in view: INFO) |
| R4 | CI | any check failed, timed out, cancelled, action required | required check missing, running, or not conclusive |
| R5 | reviews | `CHANGES_REQUESTED` not lifted by the **same** author | blocking prose marker posted after the head |
| R6 | inline threads | unresolved and not outdated | (unresolved but outdated: INFO) |
| R7 | lane signature | | last body line is not a literal `*lane*` |
| R8 | counter-review | | none on the head, when the PR is from the merger's lane or `--require-counter-review`. Formal reviews keep their rule (attached to the head commit); a **comment** counts only when posted after the head, declaring itself counter-review (`contre-revue` / `counter-review`, case-insensitive) **and** naming the head (first 8 hex chars of its SHA). When R8 finds nothing, it names the post-head comments it saw and excluded, and any counter-review another lane declared on a **previous** head (the stacked-PR case after a base merge): it does not count, and the message says what to compare |
| R9 | closing keywords | | body, title or a commit message will close an issue: keyword + `#N`, `owner/repo#N`, `GH-N` or an issue/PR URL |
| R10 | diff hygiene (added lines) | token and private-key formats | secret-like assignment, profile path, mention of the `.keys` directory; any file the scan could not fully read |
| R11 | protected paths | printed reference (`Cards/*/Archive`, CardPen `examples`, the directories themselves included), env files (`.env`, `.env.*`, `.env-*`, `.envrc`), `.keys` directory, `*.pfx` | workflows, `.claude`, `AGENTS.md`, `CLAUDE.md`, `scripts/gates`, `web.config`, env models (`.env.example`, `.sample`, `.template`, `.dist`) |
| R12 | taxonomy CSV | another open PR touches the taxonomy CSV | (any CSV under `Cards/`: INFO, re-parse reminder) |

Required CI contexts are read live from the branch ruleset; if that read fails
the gate falls back to a named list and **says so** in its output.

R11 matches protected paths **whatever their case**: the repository tracks both
`Web.config` and `web.config`, and a case-exact rule let the first one through.

`blocked` is GitHub's word and it does not say why. When R4, R5 or R6 already
holds or blocks, the cause is in view and R3 only notes it. When none does, the
cause is somewhere the gate does not look (an approval rule of the ruleset, a
state not yet recomputed): a PASS there would print a command GitHub refuses, so
R3 holds.

### Acknowledgements

| Option | Lifts | Never lifts |
|---|---|---|
| `--ack-closes N[,N]` | R9, for exactly those issue numbers | |
| `--ack-path GLOB` (repeatable) | R11 HOLD, and the printed-reference BLOCK | env files, `.keys` directory, `*.pfx` |
| `--ack-read R5[,R10]` | heuristic HOLDs: R5 prose markers, R10 soft patterns | any BLOCK, a formal `CHANGES_REQUESTED`, a blind file |

An acknowledgement states that a human or agent **read** the thing. The gate
cannot check that.

`--ack-path` is matched **case-exact**, unlike the protection it lifts: the
acknowledgement has to name the path as the pull request writes it, so
`--ack-path '**/web.config'` does not lift a hold on `Web.config`.

## What the gate does not establish

- It does not read the substance for the merger: body, comments, reviews and
  diff are still to be read. The output ends with the reading surface.
- It does not re-parse a CSV (BOM, CRLF, columns): `tools/bytecheck-datasets.py`.
- It does not judge a visual verdict (card render, PDF, mind map).
- A lane signature is **declarative**. Several lanes write through the same
  GitHub account, and the gate does not authenticate who wrote a body or a
  comment: R7 and R8 catch a forgotten signature or a missing counter-review,
  not an impersonation.
- Of the branch ruleset it reads the required check contexts and nothing else.
  A required approval, or any other rule, shows only as `blocked` (R3).
- R5 (prose), R8 (dates: a comment counts when posted after the head commit's
  committer date, an approximation; and its declaration is matched by keyword,
  not read) and R10 (patterns) are heuristics. Their silence is not proof. R8
  establishes that a counter-review **exists**, not what it says.

## Size limits

GitHub lists at most 3000 files and 250 commits per pull request. Past either
cap the gate cannot read every path (R11) or every commit message (R9): it says
which cap was hit and exits `1`. Such a pull request is merged on a reading done
by hand, or split.

## Replaying a snapshot

`collect()` does all the I/O and returns a JSON snapshot; `evaluate()` is pure.

```bash
python scripts/gates/pr_merge_gate.py 1234 --snapshot-out snap.json
python scripts/gates/pr_merge_gate.py --from-snapshot snap.json          # same verdict, no network
python scripts/gates/pr_merge_gate.py --from-snapshot snap.json --json   # machine-readable
```

A snapshot is evidence of what the gate saw at `collected_at`; it goes stale as
soon as the PR moves. Merge on a live run, with `--expect-head`.

## Self-test

`--self-test` runs offline and is wired in `.github/workflows/python-tools-tests.yml`.
For every rule it injects the defect and expects the verdict, then **disables the
rule and expects PASS again** — a case that still fails with its rule disabled
does not discriminate. It also runs inverse controls (prose that must not
trigger, removed lines, signature not on the last line, a literal signature
followed by trailing text on the last line, an arbitration comment that must not
count as a counter-review, neighbours of a protected
name such as `docs/web.config.md`) and drives the collector through a fake `gh`
where each endpoint fails in turn (exit `1`, never `0`), where an endpoint
answers with **fewer items than it announces**, and where GraphQL returns an
error envelope next to complete data.

A guard is only worth the case that would go red without it. When adding one,
delete the guard and run `--self-test`: if it stays green, the case is missing.

Secret-shaped test strings are assembled at run time: none exists literally in
the file, so the gate does not block its own pull request.

## Adding a file here

`.gitignore` carries `Scripts/`, which matches `scripts/` on a case-insensitive
checkout. New files in this directory need `git add -f`.
