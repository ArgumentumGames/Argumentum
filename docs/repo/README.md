# docs/repo — repository hygiene: the weight of `.git`, test-suite debt

<!-- index-of-record -->

This folder is an **index of record**: every file in it is listed below with what it is worth today. Add a file here and you add its line — `python tools/test_docs-index-contract.py` goes red otherwise. The three statuses and the method are in [`docs/agents/consolidation.md`](../agents/consolidation.md).

## Where the two subjects stand

Measured 2026-11-13 on `47d70f15`. Re-measure before quoting; the issue says what is open.

- **Weight of the repository (#415).** Phase 1 is done: no regenerable build binary is tracked at `HEAD` any more (#416, #501, #858), and a CI guard refuses any new blob above 2 MiB outside its allow-list (#1325). Phase 2 — rewriting history — is prepared and **has never been run**. It waits for two things: the `v2.0.0` tag, then the owner's go on that exact gesture, the same day. What governs it is [`docs/quality/415-filter-repo-runbook-post-tag.md`](../quality/415-filter-repo-runbook-post-tag.md) with its read-only preflight [`tools/415-filter-repo-phase2-preflight.py`](../../tools/415-filter-repo-phase2-preflight.py) — not the two plans listed below, which it replaced.
- **Test-suite debt.** The nullable clean-up is done: since #1676 a nullable warning is an error in both test projects. Current test and skip counts are in [`docs/agents/status-and-history.md`](../agents/status-and-history.md) § *Test Coverage* — a dated snapshot, like every count.

## The files

| File | Dated | Status | What it is |
|---|---|---|---|
| [`415-pack-growth-explained.md`](415-pack-growth-explained.md) | 2026-09-14 | `current` | Why "2.05 → 5.57 GiB" compares two different clones and measures no growth; where the weight actually sits. Read this before quoting any size of the repository |
| [`415-git-weight-audit.md`](415-git-weight-audit.md) | 2026-07-01 | `historical` | The audit that established phase 1 was already done while the issue still listed it as to do. Its sizes are those of July |
| [`415-phase2-proposal.md`](415-phase2-proposal.md) | 2026-07-03 | `superseded` | First costing of the history rewrite. Replaced by the [runbook](../quality/415-filter-repo-runbook-post-tag.md): the object list changed (a legacy **source** tree it proposed to purge stays) and the expected gain was re-computed |
| [`415-git-reduction-execution-plan.md`](415-git-reduction-execution-plan.md) | 2026-07-05 | `superseded` | Step-by-step plan built on the proposal above. Replaced by the same [runbook](../quality/415-filter-repo-runbook-post-tag.md) — do not execute this one |
| [`415-allowlist-options-2026-09-15.md`](415-allowlist-options-2026-09-15.md) | 2026-09-15 | `historical` | What the large-blob guard's allow-list lets through, and the options for shrinking it. The choice was made — next row |
| [`415-feuille-decision-allowlist-2026-09-15.md`](415-feuille-decision-allowlist-2026-09-15.md) | 2026-09-15 | `historical` | The three-question decision sheet drawn from the options. Answered on 2026-09-17, as recorded on [#458](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5728827430): the website export stays as it is until the site is deployed, the mind maps until the tag, then both move out (extraction or LFS); the design sources go to LFS in a quiet slot after the tag. Nothing of this is executed yet |
| [`test-skips-inventory.md`](test-skips-inventory.md) | 2026-07-06 | `historical` | The 5 skipped tests of July, each classified (infrastructure-gated or coverage gap). The suite has moved since: re-list the skips before relying on it |
| [`tests-nullable-cleanup-plan.md`](tests-nullable-cleanup-plan.md) | 2026-07-06 | `superseded` | Plan for the 96 nullable warnings of the test project. Done and locked by #1676; the main project is still outside the nullable context, which this plan never covered |
