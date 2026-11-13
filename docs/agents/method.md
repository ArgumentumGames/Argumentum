# Working method — rules that each already cost a cycle

Each rule below was paid for by at least one wrong conclusion, dispatch or publication in this repository. It extends `AGENTS.md` §4 *Measuring*, §5 and §6, and does not repeat them.

## Measuring — suspect the instrument, then open it

**An unexpected measurement indicts the instrument before the artefact.** A stale cache, a download killed by a timeout, a browser cache and an incremental build have each produced a "finding" here, as many false regressions as false passes. Confirm the read completed (bytes received against `Content-Length`; a size that is an exact multiple of a power of two signals truncation) and restate the full identifier — host and path, absolute file — in every pass of a before/after comparison.

**Suspecting the instrument is not a diagnosis; opening it is.** "CI does not run the tests" was published while the workflow had run `dotnet test` since the fix of #909; the cause was test parallelism following the core count (#1194). Read the workflow, config or script before saying why it is blind; when two measurements diverge, look first for the environment that differs (cores, shell, path, locale).

**Calibrate a control on a case whose answer you know.** A zero, a perfect symmetry, or exactly the value that confirms the suspicion are the three most seductive results. `pdftotext` returns 0 on these PDFs because they carry no text layer: the probe cannot perceive the object. An uncalibrated zero is not published, even as a note.

**An inverse control that returns the same number as its witness is inert.** Read a control as a delta against its witness, never as "≠ 0 so OK". Re-typing a calibrated command builds a new, uncalibrated instrument — a trailing slash in a git pathspec turned 34 into 0 at the merge of #1453. Replay a witness by its exact command, and derive search patterns from the source (`git show <commit>~1`), never from memory.

**Prove you compare the same objects before reporting that they differ.** A join on `path` reported 308 Scenarii "rewrites" that were re-keyed rows: category matched 63/63 while sub-category matched 16/63. Require an invariant that survives a rewrite but not a re-key, and that documented renames cannot touch. When two keys disagree, the answer is "not measurable with this instrument", never an estimate.

**Measure at the output of the reader, not in the source column.** A `link` column 4–7 % filled was 92 % effective on the deck, through the fallback cascade declared in `Fallacy.cs` (#1435). Populated is not rendered either: 1081 cross-links existed and none reached a card or a mind map (#497) — `git grep "<Property> ="` returning nothing means the dependent code never runs.

**A count without an expected value cannot detect an absence.** "79 PDFs" hid a missing language in plain sight (`ru 9`), and an existing file held 2 pages instead of 334. Write the expected figure first (8 languages × 10 documents), compare variants with each other, and measure content (pages per language), not only files.

**A language-model judge fed truncated input invents defects.** Cells cut at 900 characters came back as six false "truncated cell" findings. Reduce the number of items per call, never their length. A positive witness proves sensitivity, not specificity.

## Verifying a deliverable

**Structure and localisation are orthogonal.** Translated text passed 8/8 while the Memo back grid was broken in all eight languages. Check geometry explicitly (headers per family, items per cell), inspect every card, and judge a live render — real engine, real data — loaded in a browser.

**A control must be able to see the defect it rules out.** In the recto-verso PDFs odd pages are backs; a "French text copied" defect is invisible between `fr` and `ar`, which differ anyway. Ask "if the defect were still there, would this measure change?", then pick the page by its role and the languages by their ability to show the defect.

**Measure a shape, not a size.** "The card comes out at 60 × 113" stayed true over a +15 % distortion, because the resize ignored the aspect ratio (#1250). Feed a square probe and require a square.

**A rendered rule is judged on the rendered pair element × background.** A harmless-looking `color:` put a page number white on white (#1429). Render before, after, and a witness card without the feature; measure overflow against the inner edge of the coloured border (#1607); accept on the real pipeline render, not a harness (#1485).

**A defect in one document of a language calls for a sweep of that language.** A witness pair proves a change did not leak; it is blind to a per-language harvest defect such as unloaded fonts. Compare language against language: a stable defect passes every before/after check.

**A fix follows the shared template, not the reported card set.** Three other card sets reused the back template fixed for one (#1141). After a fix, measure the same defect one notch below and one notch beside; a "last occurrence" is a verification task, not a conclusion.

**When a report names a gesture, the object is what the gesture produces.** "When one clicks" cannot be about a static file in the repository. A re-worded report means the first answer missed: reproduce the gesture (a real click), then measure.

**A visual impression is refuted by the reference object, not by a second look.** A "defect" can be faithful to an asset already shipped. For a corpus that was printed, the reference is the printed object, not the first commit: the repository starts after the print.

## Reporting what you know

**Do not publish an inference in the voice of a measurement.** A GO issued does not prove a run started; a merge does not move your local branch. Never announce both sides of a delta when one was measured, nor relay another lane's count as your own measure.

**"Verbatim" is a claim too.** Use it only for the string from the source closest to the speaker; a relay that summarises is a summary. When two channels carry one decision, the more detailed one decides the wording.

**Scope every negative.** Write the real scope ("0 in the active window"), not "never". An absence claim names every channel searched; a timeout or a cold start is inconclusive.

**Describe a defect; never quote it inside its remedy.** A quoted wrong string stays indexable and the next `grep` reads it as current (#1458). A struck value beside its replacement is fine; the defect is an old value standing far from what cancels it.

## Reviewing

**A diff is read against the merge-base.** Reviewing #1477 against the current `master` showed 4 cells "reverted" that were `master`'s own later commits, sign inverted; at the merge-base it was 1 cell. Write "base = X, target = Y".

**An organ passes two trials: injected defect → red, real artefact → green.** The injected witness proves sensitivity only; an exemption written from an untested sentence made a new test red on a legitimate page (#1178). Demand the measure behind every exemption.

**Ask on which machine a guard can fail.** A guard comparing a served file with the local checkout is equal by construction on the serving machine (#1137): compare to a distant reference and watch the guard fail once. A recipe is validated only once replayed on another machine.

**A gate figure is relative to a tree.** A gate green 11/11 on a base lacking a merged commit was 13/13 on the merged tree. Check `git merge-base --is-ancestor <commit> <tree>` before trusting a figure, CI green included.

**Test the premise of a written position, not its judgement.** Before recommending the opposite of a documented decision, test one of its factual claims; a deadline tied to an event is real only if the event can occur. A refuted premise does not cancel the conclusion: find what it actually rests on.

**Existing working code is load-bearing.** Before replacing a component as a side effect, diff it against the version that worked and account for each capability removed — the mind-map viewer was lost that way and restored by #826. A commit titled "restore" proves nothing.

## Dispatching and deciding

**Measure before you dispatch.** An open issue is bookkeeping, not code state; prescribing a state already reached pushes the worker to change something anyway. Run `git log --oneline origin/master --grep=<issue>` first, and check whether a proposal "awaiting a GO" was already applied.

**A doc line, a pool line, or your own note is a premise.** A stale line in a repository document gets executed, not ignored: fix it at the source, then audit the batch it came from — repairing the reported instance leaves its twins.

**A GO authorises an intent on an object, not a number.** At application time, re-measure the object and report the gap (a "38 cells" GO met 24). Content written into a dispatch is checked like a deliverable.

**A gesture presupposes a right; measure that right.** Existence and completeness checks never answer "may we?" — license, publish, merge, delete. `git blame` names the last editor, not the author, and sees nothing outside the repository: it narrows a perimeter, never widens it.

**An order lives where the worker reads orders.** "Awaiting X" at the end of a report is not a dispatch, and silence measures nothing: look where work lands (pushes, pull requests, comments). Give each lane a pool of independent grains, one pull request each; a blocked grain is excluded until a named event, which lifts the exclusion without validating the action.

**A question for a maintainer must be decidable by reading it alone.** Name the card and quote the texts, never a bare identifier; frame each item as "keep X / revert to Y" with one reason; first ask whether the question needs to exist. A review guide states what agents already checked, names the exact directory to open, and asks only what a human alone can judge. Before handing anything over, a defect visible in under 30 seconds is ours to find.

## Closing issues

**Each pass starts with closure.** An issue that is settled or superseded is closed with its measure and its inverse control in the comment. One that cannot close carries, at the top of its body, the path to its closure: what is done, what remains, the closing criterion, what blocks, and the next step with its lane — kept current by whoever merges a change that touches it.

**A roadmap's negative claims are perishable.** "No GO yet" or "blocked on X" is re-measured like an unchecked box; record a decision on the issue and the roadmap in the same gesture.

**When a merge auto-closes an issue it did not exhaust, reopen it with the measure of what remains.** Compare the issue's statement with the pull request's surface; the tell is an "and…" in the issue that moves the defect from cause to effect.

**Close a verification incident with an organ, not a note.** Ask "which test now fails if this recurs?" — the lessons that held are those that became tests (#830, #1133, #1112). The organ is red when written, and a missing input fails it loudly, never a silent skip.

## Consolidating

**Consolidating is analyse, merge with proof, then archive.** A digest is not a concatenation: keep a provenance table (source → where kept → what was dropped and why), prove every anchor before moving text, audit the whole link graph at the end. Exclude by rewritten pattern, never by whole file, and never fold away a constraint that has not expired. The definition of done, the instrument for each kind of consolidation and the index of record that holds a folder of findings are in [`consolidation.md`](consolidation.md).
