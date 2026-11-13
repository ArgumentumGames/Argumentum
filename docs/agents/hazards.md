# Hazards — gestures that destroy or mislead in silence

Each gesture below returned success, or nothing at all, while it destroyed work or produced a false reading. Read the theme before the gesture. What `AGENTS.md` §4 and §6 and `docs/agents/regeneration.md` already state is not repeated.

## Git index and working tree

**Never run `git read-tree --empty`.** It empties the index without a word, and the next commit records the deletion of every file. To test mergeability use `git merge-tree --write-tree <a> <b>` (exit code + conflict lines). Recover with `git reset --mixed origin/master`, never `--hard`, which also wipes the working tree. Before any commit, read the file count of `git diff --cached --stat`.

**Parallel agents writing git in one checkout collide.** Index, `HEAD` and working tree are shared: give each agent its own worktree, or serialise them. After the merge, remove the worktree and its branch.

**`.gitignore` does not protect a tracked file, and `git check-ignore` says "not ignored" for one.** A pattern that matches a tracked file has no effect on it. `skip-worktree` does not protect a file against `git clean`.

**`git diff --stat HEAD~1` compares a commit with your working tree.** To read what a commit did, use `git show --stat HEAD`.

## History, refs and recovery

**A commit cited as the source of truth may be reachable from no ref.** `git log` without `--all` answers only for your branch: an invisible branch was once read as "git never contained it". Check `git branch -a --contains`, `git tag --contains`, `git ls-remote` and `git reflog --all`, then protect the commit with a local tag — garbage collection removes an unreachable commit.

**A history rewrite needs three gates.** A mirror backup verified by count (`git clone --mirror`, then `git rev-list --all --count` on both sides); a dry run read as an inventory that names one file which must survive and one which must go; and a decision on the exact gesture, not on the intent. An inverted parameter in such a rewrite has already destroyed a repository.

## Pull requests and merges

**Merging the base of a stacked pull request with `--delete-branch` auto-closes the dependent one** (#1106/#1107). Merge the base without deleting its branch, retarget the dependent (`PATCH pulls/<n> base=master`), check CI on its exact head, then delete both branches. A retarget triggers no workflow; runs started by `workflow_dispatch` appear under `commits/<sha>/check-runs`, not in the pull request's rollup. Recover a lost head byte-exact with `git show <sha>:<path>` onto a new branch.

**After merges that rebased each other, compare `origin/master:<path>` to what you expected.** A merge can drop content that each pull request carried on its own.

**The displayed diff of a pull request is not its net effect** (#1428). A stale base makes files from later merges appear: `git rev-parse origin/master:<path>` against `pr<N>:<path>` tells identical blobs apart, and `git diff --stat origin/master pr<N>` gives what merging will change.

**`mergeStateStatus: CLEAN` does not say the tests passed.** After `gh run rerun` the run went back to queued with zero jobs and the state read clean (#1001); a `startup_failure` emits no check run at all (#1349). Assert that every expected context is present and successful.

## GitHub CLI, API and releases

**`--body @file` posts the literal path.** `gh` does no `@` expansion on `--body` and returns exit 0 with the comment URL. Always use `--body-file`, then re-read the posted `.body | length`. The body of an issue created that way is a third surface that re-reading comments does not catch.

**The contents API returns empty content above ~1 MB, with exit 0.** A comparison built on it went green on two empty files. Assert a non-zero size and a floor count on both inputs, and read large files with `git fetch origin 'refs/pull/N/head:prN'` then `git show prN:<path>`.

**A `gh` failure has three causes: identity, transport, GraphQL quota.** `gh api rate_limit` can show budget left while GraphQL calls are refused: probe with a real GraphQL call, and fall back to REST, whose fields differ (`.user.login`, `.created_at`, `.html_url`). Check the active account (`gh api user`) just before each write.

**A script that masks `gh` errors turns an outage into an empty result.** A triage script answered "0 requests" twice on a dead API until #1343 made it fail loudly. Never `2>/dev/null || echo "(none)"`; add an inverse control that fails if an outage passes for a clean backlog.

**Replacing a release asset has a radius.** Before `--clobber`, record the digest of what you replace; a 404 during `--clobber` was transient, retried file by file. After the upload, check `digest` and size through `gh api repos/O/R/releases/tags/<tag>`. The release notes and the notice to recipients must still be true of the bytes served; withdraw a known defect only once seen fixed in the served file.

**A tool under `tools/` must run from another checkout.** A tool carrying the path of the checkout it was written in failed before its first measure everywhere else (#1489), and CI stayed green because it executes none of the scripts under `tools/` beyond the ones wired into workflows. Derive the root from `__file__`.

## Shell and Python scripts

**`scan ; act` runs the act whatever the scan found.** A `grep` that crashed on its pattern still let the comment go; a scan that found matches still let the merge go. Write `n=$(grep -c … || true); [ "$n" = "0" ] && act`.

**`$?` after a pipeline is the status of its last command.** `r=$(cmd | head); ec=$?` gives `head`'s status. Use `set -o pipefail`, keep stderr visible, and read the process's own exit and summary rather than a wrapper line.

**A backslash does not survive a Bash heredoc.** Write scripts with a file-writing tool, or build the character with `chr(92)`; verify with `od -c`, not `grep`. For a template `css` edit: assert `raw.count(old) == 1`, replace, re-parse the JSON and assert that only `css` changed; write UTF-8 without BOM and with `newline=""`.

**`open(path, "w")` truncates before writing.** A failure after the open leaves a 0-byte file. Write to a temporary file and `os.replace` it; a repair script asserts its own effect afterwards.

## CSV and line endings

**No `.gitattributes` rule covers CSV files, and `core.autocrlf=true` rewrites them at commit.** The Fallacies taxonomy has 1409 CRLF lines: a commit through the working tree turns a 1-line diff into 1409. Write the blob yourself — `git hash-object -w --no-filters <file>` then `git update-index --cacheinfo 100644,<sha>,<path>`. `git -c core.autocrlf=… status` does not measure the option; compare with `git hash-object --path`.

**A text round-trip converts every line ending of a CRLF file.** Edit in binary with CRLF-anchored byte patterns; a byte-level cut uses `len(x.encode())`, not `len(x)`.

**A spreadsheet source can be ahead of the CSV or behind it.** Download, diff, pull request, re-diff, then upload — run the sync dry-run first.

**A prompt glossary regenerated from the CSV is consistent, not correct.** It freezes the corpus's drift, and CI then defends the frozen state. Fix glossary and CSV in the same pull request, against the printed reference.

## Regeneration inputs

**Faces render from the source CSV; backs render from the template.** The pipeline injects the dataset over the template's embedded `csv`. A content verdict on a face targets the CSV, never the copy embedded in a template.

**`dotnet clean` does not remove the runtime `.harvest.json` cache.** Delete the harvests of every language for the affected set; a post-fix render byte-identical to the previous delivery proves the cache was served. Check a cleanup by its effect: `find -delete` on a pattern matching nothing succeeds, and a hardcoded `bin/Debug` path missed the Release tree — count before and after, and fail on a zero delta (#1133).

**The rendering engine is a regeneration input too.** Templates and CSV follow the build mode, but the CardPen engine comes from the local site in both modes: a new template on a stale engine renders a silently wrong card (#1127/#1130). Guard against a distant reference — the copy GitHub Pages deploys through `static.yml` — and see the guard fail once (#1137). Merge no CSV or template change during a pass; preflight one control image.

**A fix that spans data and code dies in silence if only one half is refreshed.** A defect revealed by a regeneration was not introduced by it: rolling back re-arms it.

**Serve a browser check from the bytes you mean.** Extract the artefact with `git show origin/<branch>:<path>`, serve it on a fresh port, and check with `netstat` that one listener holds the port — a duplicate `http.server` on one port served an old file and produced a false FAIL.

**A Playwright run frozen at a flat 0 % CPU without a crash is synchronous I/O in an event handler** (#651), not a slow page.

## Tests and CI signals

**A test whose expected value is materialised at run time is green by construction** (#1112). So is a test declared but never executed (#1048). Read the test's input before accusing a diff, and make a new organ fail once on a planted defect before trusting its green.

**A CI status is a verdict, not a cause.** Read the log (#1134). An aggregate such as `UNSTABLE` is not terminal, and an empty `conclusion: ""` defeats jq's `//`: wait for `.status == "COMPLETED"` on each check.

**`dotnet test -v q` truncates assertion messages.** A zero read from it named nothing; rerun with `--logger "console;verbosity=detailed"`. Validate any output filter on a known red first.

**A negative assertion on a typed literal goes inert when the string changes.** Derive the watched string from the live source and prove it by mutation: N planted mutations must give exactly N reds, each naming its cell.
