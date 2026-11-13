# Consolidation — how a consolidation mission is finished

A consolidation takes something that exists in several places — text copied across files, a folder of dated findings, scripts that overlap, a state scattered over comments — and leaves **one** place that says what holds today. It has three steps, in this order:

1. **Analyse** every piece of the old material — each section, each function, each file. The list comes from an enumeration (`git ls-files`, a parser), never from memory.
2. **Merge** each piece into the target, with the proof that it arrived: the line where it now lives.
3. **Retire** the old copy — only after the proof.

Moving files to an archive folder without that proof is not a consolidation: it is a deletion that has not been noticed yet.

## Why these missions stay open

Each of these has happened here, more than once:

- the target is written, and the old copy stays — two live sources, which drift apart from the first edit;
- the content moved, and the pointers to it did not: issues, other files and tools still cite the old address;
- the proof is a sentence ("nothing was lost") that nobody can replay;
- the index lists the files its author remembered;
- the issue stays open, with a body that still describes the state before the work.

The definition below exists so that "done" can be checked by someone who only sees git.

## Definition of done

A consolidation is finished when **all** of these hold. The pull request states each one.

| # | Condition | How it is shown |
|---|---|---|
| 1 | **The inventory is complete** | the count of pieces comes from a command quoted in the pull request, and the result accounts for every one of them |
| 2 | **The proof is an instrument, at a commit** | a command, its exit code, replayable from a fresh clone — see the table of instruments below |
| 3 | **One source is left** | the old copy is deleted; or, when it is evidence that must stay, it carries a status and names what replaced it |
| 4 | **Inbound references resolve** | `git grep` for the old path or the old title returns nothing, or each remaining hit is justified in the pull request |
| 5 | **The limits are written** | what the proof does not cover, in the pull request and next to the instrument |
| 6 | **It cannot come undone in silence** | a contract in CI holds the result — or the pull request says that nothing holds it |
| 7 | **The issue says so** | closed with the proof, or its body updated with what remains and what blocks it |
| 8 | **Another lane tried to break it** | the reviewer removed a line, a file or a guard and re-ran the instrument: it went red |

Condition 8 is not a courtesy. An instrument written by the author, run by the author and read by the author has shown only what the author expected to see.

## The instrument for each kind

| What is consolidated | Instrument | What "green" means |
|---|---|---|
| Text moved from one file to others | `python tools/doc-move-preservation.py` with a manifest — see [`README.md`](README.md) | every non-blank line of the original is found word for word at the destination, or declared with its reason |
| A folder of dated findings | an index of record, held by `python tools/test_docs-index-contract.py` — next section | every tracked file of the folder is listed with its status; no dead link |
| Scripts merged into one | a table in the pull request — each function of the old script, the file and lines where it lives now — and a test that replays the old behaviour on a fixture | nothing generic holds this: the table **is** the proof, and the reviewer checks it line by line |
| A state scattered over comments | the body of the issue, rewritten: done, remaining, closing criterion, what blocks, next step | a reader who opens only the body knows what to do — `AGENTS.md` §5 |

## An index of record for a folder of findings

A folder of dated findings only grows. Its files are evidence — they are not merged into one, and they are not deleted because they are old. What can be finished is knowing, for **each** file, what it is worth today.

An index is a `README.md` that carries this marker, alone on a line:

```
<!-- index-of-record -->
```

By carrying it, the README commits to listing every tracked file of its folder, with exactly one status per line, in backticks:

| Status | Meaning | What a reader does with the file |
|---|---|---|
| `current` | says today's state, or governs a gesture still to come. Someone re-read it against `master` on the date of the index | relies on it — after re-measuring any count it quotes |
| `superseded` | replaced. The line names what replaced it: a second link, or an issue or pull request number | goes to the successor; never executes this one |
| `historical` | evidence of work that is closed: true at its date | cites it as a dated record; never executes it, never "corrects" it — correcting a dated record falsifies a journal |

A link to a subfolder covers the files under it (screenshots, data extracts): one line, one status. A subfolder that carries its own marker is held by its own index, and the parent only has to cite it — so a large folder is indexed piece by piece, one pull request per piece.

A README without the marker is bound to nothing. The contract applies only to folders that have been consolidated, and from then on a file added without its line turns the pull request red.

The worked example is [`docs/repo/README.md`](../repo/README.md).

### Procedure

1. `git ls-files docs/<area> | wc -l` — the count to account for. Quote it in the pull request.
2. Group the files by subject (most are named after their issue). For each subject, read the issue first: it says what is open.
3. For each file, read its header (date, base commit, declared status) and decide. `current` is the expensive status — it asserts a re-reading. When in doubt between `current` and `historical`, it is `historical`. The status follows what the file governs, never the state of its issue: a file that governs a gesture still to come stays `current` after its issue closes (its known defects go under the index's contradictions), and an open issue does not make a file `current` — a file not re-read against `master` is `historical`, and its line points to the issue, which holds the live state.
4. On top, a short dated paragraph per subject: where it stands, and which file governs. This paragraph is what the next reader will actually use.
5. Add the marker last, run the contract, open the pull request. Keep a pull request under about sixty files indexed: beyond that, nobody reviews the statuses.
6. The index changes no finding. A file that turns out to be wrong gets its own pull request.

### What the contract does not hold

That a status is *right*. The contract sees that each file has one; whether `current` is deserved is the judgement of whoever consolidates, and the reviewer samples it — open three files marked `current` and check them against `master`.

## Anti-patterns

- **Rewriting while moving.** The proof compares words; a reworded line reads as lost. Move verbatim in one commit, edit in the next.
- **Deleting a finding because it is old.** It is evidence. Give it a status.
- **Marking `current` what was not re-read.** A stale file labelled current is worse than an unlabelled one: it now carries an authority.
- **Quoting the retracted figure in the correction.** The next search finds it and reads it as live. Describe what was wrong; do not repeat it.
- **Raising a floor or a budget so that the addition passes.** That is a decision, argued in the pull request that takes it.
- **Declaring the mission done with the issue open and unchanged.** The issue is where the next agent looks first.
