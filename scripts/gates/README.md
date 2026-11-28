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
| R10 | diff hygiene (added lines) | token and private-key formats | secret-like assignment, profile path, mention of the `.keys` directory; any file the scan could not fully read (the count comes from git, not from the API's announcement — see below) |
| R11 | protected paths | printed reference (`Cards/*/Archive`, CardPen `examples`, the directories themselves included), env files (`.env`, `.env.*`, `.env-*`, `.envrc`), `.keys` directory, `*.pfx` | workflows, `.claude`, `AGENTS.md`, `CLAUDE.md`, `scripts/gates`, `web.config`, env models (`.env.example`, `.sample`, `.template`, `.dist`), `tools/test_docs-index-contract.py` (the contract R13 re-executes — a pull request that weakens it would be replayed with its own weakened instrument) |
| R12 | taxonomy CSV | another open PR touches the taxonomy CSV | (any CSV under `Cards/`: INFO, re-parse reminder) |
| R13 | docs index of record | | `tools/test_docs-index-contract.py` returns **red** when replayed on the PR's **merged tree** (`git merge-tree` → `commit-tree` → a detached worktree → `python -B … DocsIndexContract`). The message quotes the contract's own `AssertionError`, never a re-read of it. A replay that cannot see (no local repo or no `origin`, an `origin` that names another repository, base that cannot be refreshed, head missing, merge conflict, contract absent from the tree, unrecognised output) is **blind**: exit 1, never a PASS. A replay worktree that could not be removed is **named** (path + removal command) in the R13 line — the verdict stays the contract's |

Required CI contexts are read live from the branch ruleset; if that read fails
the gate falls back to a named list and **says so** in its output.

### R10 counts with git, not with the API's announcement

The API `pulls/N/files` announces, for every file, how many lines it adds. Past
a diff budget it **truncates that announcement**: the last files come announced
`additions: 0`, `changes: 0`, and without a `patch` — while the unified diff,
downloaded separately, carries their lines. **Measured** on #2403 (2026-11-28):
the API announces 0 for four files whose `git diff --numstat` counts 278, 278,
1512 and 4 added lines; the unified diff carries all 38 files, and the number
of lines the gate reads matches `--numstat` on the 38 (0 discrepancy).

Taking the announcement as the reference therefore produced a false HOLD —
"scan AVEUGLE on 4 files", for files that had been read in full. The reference
is now **git's count**:

```bash
git fetch --quiet --no-tags origin +refs/heads/<base_ref>:refs/remotes/origin/<base_ref>
git fetch --quiet --no-tags origin refs/pull/<N>/head
git diff --numstat -z <merge-base> <head>
```

Git is asked only when its answer can change the verdict: a file read with a
count different from the announcement, or a file announced 0 without a `patch`.
Where the announcement and the lines read agree, git is not asked and the gate
behaves exactly as before — the cross-check never invents a HOLD.

A file whose **read** count differs from git's count stays blind: HOLD. A file
read in full whose announcement git contradicts is reported (INFO) — the file is
named, the announcement is named, the verdict does not change. A cross-check
that cannot read (no local repository, an `origin` naming another repository, a
base that cannot be refreshed, a head that is absent) never lifts anything: the
HOLD stays and names the failure.

The reads happen in the local clone of the pull request's own repository, like
R13's replay: the origin's URL is checked against it, and the only writes are
the ones a `git fetch` makes.

### R13 replays the contract, it does not re-read the indexes

`tools/test_docs-index-contract.py` is the source of truth for "an index of
record cites every tracked file of its folder". R13 runs **that file**, on the
tree the squash will produce:

```bash
git fetch --quiet --no-tags origin +refs/heads/<base_ref>:refs/remotes/origin/<base_ref>
git merge-tree --write-tree refs/remotes/origin/<base_ref> <head>   # merged tree
git commit-tree <tree> -p <base> -p <head> -m gate-docs-replay  # identified
git worktree add --detach <tmp> <commit>
python -B <tmp>/tools/test_docs-index-contract.py DocsIndexContract
git worktree remove <tmp>
```

`<base_ref>` is the pull request's own base branch, read from GitHub — it is
**not** `master` in general: the example above writes `master` because the
pull request it describes targets `master`, and the code takes the base the
pull request declares.

The base is **refreshed first**, never presumed current. **Measured** on
2026-11-27: on a clone whose `origin/master` is behind, the replay said green
where the squash is red. `master` creates an index of record above a folder
already tracked (the #2252/#2263 form); a pull request branched before adds a
file under it — covered by the old base's folder link, owed to the new index
after the squash. The old base plus the head is not the tree the squash will
produce, so reading it is reading the wrong thing. A fetch that fails (no
`origin`, unreachable, no such branch) is blind: exit 1.

The writes in the local clone are the ones a `git fetch` makes — the
remote-tracking ref **and** `FETCH_HEAD` — plus the **temporary registration**
of the replay worktree under `.git/worktrees/`, which `git worktree remove`
takes back. Nothing else is written: the replay reads a tree, it does not
publish it.

**The replay runs in the local clone of the pull request's own repository.**
The origin's URL is checked against the repository the pull request belongs to;
if the local `origin` names another repository, the replay goes **blind** (exit
1) rather than replaying a contract against the wrong tree.

**A merge conflict is blind too.** `git merge-tree --write-tree` cannot compute
the merged tree of a base and a head that touch the same lines; the replay says
so and exits 1. It never renders a verdict about a tree it could not build.

**A worktree that could not be removed is named, not hidden.** If
`git worktree remove` fails, the checkout stays on disk and its registration
stays under `.git/worktrees/`. The R13 line then carries the path and the
removal command (`git worktree remove <path>`), and the verdict stays the
contract's own — the leak is reported, the verdict is not invented.

The contract stops at the **first** failing index (`assertEqual` inside its
loop), so does the message: it names that index's defects, and a second failing
index shows on the next run.

The quoted message is the assertion's own first lines — cut at the blank line
(the diff), at unittest's separator, or at 300 characters — so the bilan is
never reported as a defect. **Measured** on 2026-11-27: the contract's floor
(`aucun README.md ne porte la marque`) had its rule and `Ran 2 tests … FAILED`
pulled into the quoted defect before this cut.

The class is run because the contract's inverse controls belong to its own CI
job (`agent-docs-contracts`); `-B` keeps the worktree free of `__pycache__` so it
removes without `--force`.

The replay checks out the **whole** merged tree, and that is what it costs.
**Measured** on 2026-11-27 on one worker machine shared with other lanes, nine
passes of `_docs_replay(2410, "master", b99aa59a)` before the base fetch was
added (16 s to 37 s per replay) and three after (19.9 s, 23.7 s, 35.6 s): the
fetch 1.0 s to 1.6 s, the checkout 11 s to 17 s, its removal 3 s to 8 s, the
contract itself 2 s to 12 s. The spread between two passes is attributed to
load on that machine, not isolated. The cost on another machine, and on a CI
runner, was not measured.

A sparse checkout is not a shortcut. **Measured** the same day:
`sparse-checkout set docs tools` makes the contract red where the full tree is
green — `lien mort : ../../.github/workflows/static.yml`, on the merged tree and
on the pull request's head alike — because an index of `docs/` links a file
outside `docs/`, and the contract reads the filesystem. For the same reason the
gate does not skip the replay when a pull request touches neither `docs/` nor
`tools/`: **derived** from that mechanism, deleting the linked file would break
an index without touching either.

A locally re-implemented reader came first and was wrong where it mattered: on
the ten index forms measured in po-2023's counter-review (2026-11-27), two
agreed with the contract; six passed where the contract refuses — an index the
pull request creates above an already-tracked file, a rename, a copy, a `./`
link, a link to the parent folder, a citing line inside a code block; two held
where it does not — a sub-index cited by `sub/README.md`, a `%20` target.
Replaying the contract also settles the form the reader could not see: an index
**created** above an already-tracked file (#2252 created the three indexes and
omitted `articles/README.md`, #2263 having added it four minutes earlier).

A `git archive` extract was the first form tried, and it cannot see in either
placement — **measured** on 2026-11-27. Outside any repository, the contract
raises `RuntimeError: git ls-files a échoué` (`FAILED (errors=1)`): the gate
reads a blind instrument, exit 1. Nested inside the enclosing repository,
`git ls-files` lists nothing under the ignored extract, the contract's floor
fires (`aucun README.md ne porte la marque`) and the gate reads a **red** — about
the extract, not about the pull request. The merged tree is therefore replayed
as a detached worktree of the local repository, where `git ls-files` answers
about the merged tree itself.

Green means the two real assertions ran and passed on the merged tree: a
`Ran 1 test … OK` is not a green, it is an instrument that did not see
everything. A traceback or a `RuntimeError: git ls-files a échoué` is the
contract saying it cannot see — a blind instrument, not a defect.

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
