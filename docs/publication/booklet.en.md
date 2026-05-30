# Publication Booklet — *placeholder*

> ⏳ **Pending scope clarification** — ai‑01 dispatch 2026‑05‑30, `[ASK]` message posted on the workspace dashboard.

## Context

The initial dispatch mentioned a LaTeX‑compiled PDF booklet, titled "The Liars' School", hosted in a **separate repository** under the `ArgumentumGames` GitHub organisation.

## State‑of‑play inventory (2026‑05‑30)

| Verified source | Result |
|---|---|
| `gh repo list ArgumentumGames` | Only 2 repos: `Argumentum` (this one) and `Fallacies` (Python, last update 2017) |
| `gh search repos "argumentum" --owner=ArgumentumGames` | Same, 1 result |
| Search for `*.tex`, `Liar*`, `livret*`, `booklet*` in this repo | 0 results |
| Mention in `README.md`, `CLAUDE.md`, `docs/` | None |

## Hypotheses pending arbitration

1. **Personal repo.** The booklet could live under an individual maintainer (jsboige or another handle).
2. **Not yet created.** Track 1 = define a spec for a future booklet.
3. **Off‑Git.** The booklet exists in Drive/locally, and this doc should describe the integration workflow.
4. **Semantic renaming.** The word "booklet" may actually refer to the printed rules (`Argumentum_Rules.csv` → Rules cards in the Tarot deck).

## Action plan once scope is clarified

To be completed later; expected structure:

- **Identity.** Title, subtitle, author(s), possible ISBN.
- **Source.** Repo URL, canonical branch, root LaTeX file, bibliography files.
- **Build.** Required LaTeX distribution (TeX Live / MiKTeX), compile commands, graphical dependencies (TikZ, fonts).
- **Versions.** Versioning policy (git tag, publication date, possible ISSN/DOI).
- **Distribution.** Channels (site `argumentum.games`, GitHub Release, print‑on‑demand).
- **Localisation.** Multilingual policy (FR canonical, EN mirror, other languages TODO).
- **Link with cards.** How the booklet references the card catalog ([cards-catalog.en.md](cards-catalog.en.md)).

## Dispatch reference

- **Workspace dashboard** — `[ACK]` then `[ASK]` messages on 2026‑05‑30 by `po-2023`.
- **Associated doc-only PR** — this file.
