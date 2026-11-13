# docs/agents — detail files for agents, read on demand

[`AGENTS.md`](../../AGENTS.md), at the repository root, is the entry point: what the repository is, how to build and test it, the invariants, the contribution protocol. [`CLAUDE.md`](../../CLAUDE.md) adds what must be in context before a pipeline gesture — the silent failures — and maps every section title it used to carry to a file of this folder.

**Neither of them carries detail.** The detail is here, one topic per file, and no tool loads it by default: an agent opens the file its task names. `AGENTS.md` §8 says which file for which task.

## Rule for adding

- A fact that changes **what a contributor must do** → one line in `AGENTS.md`, its detail here.
- A pipeline failure that is **silent** → one row in the table of `CLAUDE.md`, its detail here.
- A measure, an incident, a dated finding → `docs/<area>/<issue>-<slug>-<YYYY-MM-DD>.md`, with a one-line pointer from the file here that it qualifies.
- Section titles are **addresses**: issues, pull requests and other files cite them. Rename one only together with the map in `CLAUDE.md` (*Where the former sections went*).
- These files describe a state at a date. A count, a status or a "next step" read here is re-measured before it is quoted or acted on (`AGENTS.md` §4 and §5).

`tools/test_agent-docs-contract.py` holds the shape: every relative link and every section pointer of these documents (a file name in backticks, then § and the title in italics) resolves, nothing but `AGENTS.md` is imported, and the two files loaded by default stay under a byte budget (61 KB before the move, 22 KB after). Raising the budget is a decision to argue in the pull request that raises it — not a number to bump so that an addition passes.

## The move of 2026-11-13, and how it was proven lossless

Until that date `CLAUDE.md` held all of this — 643 lines at `92076717` — and every Claude Code session loaded it whole. It was split in two steps: the entry-point content went to `AGENTS.md`, then the pipeline detail came here. Sections moved whole; headings and wording were left as they were.

"Nothing was lost" is not something to take on trust. `tools/doc-move-preservation.py` reads the original file at its git ref and requires **every non-blank line** to be found, word for word, in the destination files — or to be declared, with its reason, in [`move-manifest.tsv`](move-manifest.tsv):

```bash
python tools/doc-move-preservation.py --base 92076717 --source CLAUDE.md \
    --into CLAUDE.md --into AGENTS.md --into docs/agents \
    --manifest docs/agents/move-manifest.tsv --head <commit of the move>
```

Exit `0`: preserved. Exit `1`: a line is missing, or a declaration no longer covers anything. Exit `2`: the instrument could not measure (unknown ref, missing source, no destination file) — never a green.

The proof holds **at the commit of the move** — `git log --diff-filter=A --format=%H -- docs/agents/move-manifest.tsv` names it. The files evolve afterwards, as they should; a later run against the working tree will rightly list the lines edited since, and that is not a defect.

What the proof does not cover: the order of the lines (the tool prints the runs it found; they are not the verdict), whether the moved text is still *true*, and indentation. The same tool serves any later move of documentation; its own contract is `tools/test_doc-move-preservation.py`.
