# Fallacies EN/RU mindmap SVG — freshness verification (read-only)

**Date:** 2026-07-06 · **Author:** po-2024 · **Dispatch:** `x0ykyn` TERTIAIRE [3] / #636 §1
**Scope:** read-only investigation — 0 write `Cards/`, 0 régén, 0 CSV/DB/OWL mutation.

## TL;DR — gap #1 (#636 §1) is already CLOSED

The Fallacies EN/RU mindmap SVGs are **current as of 2026-07-03** (commit `d4484679`, PR #664,
merged by ai-01). **Zero commits touch the Fallacies taxonomy CSV after that date.** The
"~34 commits stale" premise in #636's title (and restated in ai-01's 2026-07-06 #636 comment)
was true when #636 was opened, but was resolved by #664 three days before that comment. **No
regeneration is needed; no manifest of pending changes exists because there are none.**

This document is the code=truth verification of that closure.

## What was asked

Dispatch `x0ykyn` TERTIAIRE [3] (#636 §1): produce a read-only manifest of the Fallacies EN/RU
régénération — list the PKs/nodes actually changed in the taxonomy CSV since the committed SVGs
(then dated 2026-04-06), to scope a turnkey régén for the FreeMind side.

## What code=truth shows

### 1. The SVGs were regenerated 2026-07-03, not left at 2026-04-06

```
$ git log -1 --format="%h %ai %s" -- Cards/Fallacies/Mindmaps/en/Fallacies_en.content.svg
d4484679 2026-07-03 15:42:45 +0200 fix(mindmap): #636 refresh Fallacies EN/RU mindmap SVGs to current taxonomy (#664)
```

Same commit `d4484679` for all four artifacts:
- `Cards/Fallacies/Mindmaps/en/Fallacies_en.content.svg`
- `Cards/Fallacies/Mindmaps/en/Fallacies_en.links.svg`
- `Cards/Fallacies/Mindmaps/ru/Fallacies_ru.content.svg`
- `Cards/Fallacies/Mindmaps/ru/Fallacies_ru.links.svg`

PR #664 title: *"fix(mindmap): #636 §1 — refresh Fallacies EN/RU mindmap SVGs"* — merged
`2026-07-03T13:42:45Z`, author ai-01.

### 2. Zero commits on the taxonomy CSV since the régén

```
$ git log --oneline d4484679..HEAD -- "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
(empty — 0 commits)
```

The last commit touching the Fallacies taxonomy CSV before the régén was `2079f0cc`
(2026-07-02, PR #640). The régén (`d4484679`, 2026-07-03) is **one day later** — the SVGs are
strictly newer than the CSV they render.

### 3. #664's own scope proof (authoritative, authored by ai-01)

The PR #664 body documents the freshness delta against the 2026-04-06 baseline (`55c6774e`):

- **Stale labels purged** (absent from current CSV, were in the old SVG):
  `All-or-Nothing Thinking`, `Appeal to desperation`, `Appeal to outrage`.
- **Fresh labels present** (in current CSV, now in the regenerated SVG):
  `Abusive relativism`, `Appeal to custom`, `Appeal to moral elevation`, `Appeal to pragmatic urgency`.
- **RU refreshed** with 1447 Cyrillic text nodes (`Софизмы`, `Обструкция`…).

Spot-check (this investigation): `grep -c "All-or-Nothing Thinking" …/en/Fallacies_en.content.svg`
→ **0** — confirms the stale label is gone from the committed EN SVG.

## Conclusion

| Question | Answer |
|----------|--------|
| Are the Fallacies EN/RU SVGs stale relative to the CSV? | **No.** SVGs 2026-07-03 ≥ last CSV commit 2026-07-02. |
| How many CSV commits pending a régén? | **0** (`git log d4484679..HEAD -- <csv>` empty). |
| Does #636 §1 still need work? | **No.** Closed by #664 (2026-07-03). |
| Should a régén be scheduled? | **No.** Nothing to catch up to. |

## Recommendation

**Close #636 entirely.** §2 (Virtues clones) was resolved separately on 2026-07-06 by #724
(ai-01 — 8/8 languages localized). §1 (Fallacies EN/RU stale) was resolved 2026-07-03 by #664.
Both gaps named in #636's title are now closed; the issue can be closed.

The "~34 commits stale / dated 06-04" wording in ai-01's 2026-07-06 #636 comment appears to
predate #664 (it restates #636's original opening premise verbatim). This is not a live
regression — #664 is on master and the CSV has not moved since.

## Non-goals (respectés)

- 0 write under `Cards/`. 0 production / rendering code change. 0 CSV/DB/OWL mutation. 0 régén.
- Read-only: `git log`, `git show`, `gh pr view`, one `grep` on a committed SVG.

Relates: #636, #664, #724, dispatch `x0ykyn` TERTIAIRE [3].
