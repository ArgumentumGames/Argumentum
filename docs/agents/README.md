# docs/agents — detail files for agents, read on demand

[`AGENTS.md`](../../AGENTS.md), at the repository root, is the entry point: what the repository is, how to build and test it, the invariants, the contribution protocol. [`CLAUDE.md`](../../CLAUDE.md) adds what must be in context before a pipeline gesture — the silent failures — and maps every section title it used to carry to a file of this folder.

**Neither of them carries detail.** The detail is here, one topic per file, and no tool loads it by default: an agent opens the file its task names. `AGENTS.md` §8 says which file for which task.

## Rule for adding

- A fact that changes **what a contributor must do** → one line in `AGENTS.md`, its detail here.
- A pipeline failure that is **silent** → one row in the table of `CLAUDE.md`, its detail here.
- A measure, an incident, a dated finding → `docs/<area>/<issue>-<slug>-<YYYY-MM-DD>.md`, with a one-line pointer from the file here that it qualifies.
- Section titles are **addresses**: issues, pull requests and other files cite them. Rename one only together with **every** pointer to it — the two tables of `CLAUDE.md` (the silent failures, and *Where the former sections went*) and the table of `AGENTS.md` §8. `python tools/test_agent-docs-contract.py` goes red on each pointer left behind.
- A lesson that cost a cycle and belongs to no single pipeline gesture → one paragraph under its theme in [`method.md`](method.md) (how to measure, verify, report, review, dispatch, close) or [`hazards.md`](hazards.md) (a gesture that destroys or misleads in silence). The rule comes first, in bold; then the incident, with its issue number. It names no person, machine, account or price — the repository is public.
- A folder of dated findings that has been consolidated carries an **index of record** — a `README.md` listing every file with its status. Add a file there, add its line: [`consolidation.md`](consolidation.md).
- These files describe a state at a date. A count, a status or a "next step" read here is re-measured before it is quoted or acted on (`AGENTS.md` §4 and §5).

`tools/test_agent-docs-contract.py` holds the shape: every relative link of these documents resolves, at the exact case; every section pointer (a file name in backticks, then § and the title in italics) designates **one** heading of that file — the whole title, or its beginning cut at a word boundary, since titles carry issue numbers and dates a pointer need not copy; nothing but `AGENTS.md` is imported, on a line of its own or inside a sentence; and the two files loaded by default stay under a byte budget (61 KB before the move, 23 KB after). What sits in a fenced code block is neither a heading, nor a link, nor an import. Raising the budget is a decision to argue in the pull request that raises it — not a number to bump so that an addition passes.

## The move of 2026-11-13, and how it was proven lossless

Until that date `CLAUDE.md` held all of this — 643 lines at `92076717` — and every Claude Code session loaded it whole. It was split in two steps: the entry-point content went to `AGENTS.md`, then the pipeline detail came here. Sections moved whole; headings and wording were left as they were.

"Nothing was lost" is not something to take on trust. `tools/doc-move-preservation.py` reads the original file at its git ref and requires **every non-blank line** to be found, word for word, in the destination files — or to be declared, with its reason, in [`move-manifest.tsv`](move-manifest.tsv):

```bash
python tools/doc-move-preservation.py --base 92076717 --source CLAUDE.md \
    --into CLAUDE.md --into AGENTS.md --into docs/agents \
    --manifest docs/agents/move-manifest.tsv --head <commit of the move>
```

Exit `0`: preserved. Exit `1`: a line is missing, or a declaration no longer covers anything. Exit `2`: the instrument could not measure (unknown ref, a source that is missing or is a folder, an `--into` that designates no file, an unreadable manifest) — never a green.

The proof holds **at the commit of the move** — `git log --diff-filter=A --format=%H -- docs/agents/move-manifest.tsv` names it. The files evolve afterwards, as they should; a later run against the working tree will rightly list the lines edited since, and that is not a defect.

What the proof does not cover: the order of the lines (the tool prints the runs it found; they are not the verdict), whether the moved text is still *true*, indentation, and markdown structure — a line found inside a code block counts as found. Short lines and pure markup (`---`, a table rule, a closing fence) are found almost anywhere, so finding them proves little: the tool counts them apart, as `FAIBLES`. A line that a destination file already carried at the base ref does not count either — only what a destination has *gained* does. The same tool serves any later move of documentation; its own contract is `tools/test_doc-move-preservation.py`.
