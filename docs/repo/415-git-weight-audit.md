# #415 Repo-Weight Audit — verified state on `18b4d023`

**Author**: po-2024 (worker) · **Date**: 2026-07-01 · **Base**: master `18b4d023`
**Status**: **AUDIT / STALE-ISSUE REFRESH**. Read-only. 0 write.
**Reproducibility**: [`tools/git-weight-audit.py`](../../tools/git-weight-audit.py) — `python tools/git-weight-audit.py`.

## TL;DR — #415 Phase 1 is CLOSED; the issue body is stale on that point

The #415 issue body (measured on `0bf77852`, 2026-06-01) states *"~1,4 GB de binaires encore trackés au HEAD"* and proposes Phase 1 (gitignore + `git rm --cached`). **Phase 1 has since been executed and merged**:

- **PR #416** `94b43712` — *"stop tracking 968 MB of regenerable build artifacts (#415 Phase 1)"*
- **PR #501** `ff031470` — *"#415 untrack 10.5 MB 2sxc install module + gitignore"*

The audit confirms it on current master: the regenerable zones are **0 bytes at HEAD** (untracked) — they survive only in history.

| Regenerable zone | In history | At HEAD |
|---|---:|---:|
| `Published/` .NET builds (osx/linux/win zips, .exe) | **1.2 GB** (42 paths) | **0** ✅ untracked |
| `DNNPlatform/.../Downloads/*.zip` (Print&Play) | 206 MB (12) | **0** ✅ |
| `2sxc/DNN *.resources` install pkgs | 152 MB (197) | **0** ✅ |
| Mindmap SVGs | 48 MB (10) | 48 MB (still tracked — see note) |

→ **Phase 1 is done.** The remaining `.git` weight is **100 % historical** — only a history rewrite (Phase 2) reduces the clone.

## Current measurements (`git count-objects -vH`)

- **`size-pack` = 2.05 GiB** (3 packs, 25 857 objects) — ≈ unchanged from the issue's 2.01 GiB. **Phase 1 does not reduce the pack** (it only stops re-adding at HEAD); this is expected and stated in the issue.
- HEAD checkout ≈ **778 MB** (down from the issue's higher figure once Published/ was untracked).

## What Phase 2 (`git filter-repo`) would reclaim

The top history blobs (deduped by path, largest version) are all in the now-untracked regenerable zones:

- `DNNPlatform/Portals/1/Downloads/Argumentum_Print&Play.zip` — **89 MB**
- `Published/v1.3/osx-x64.zip` — 80–83 MB (multiple versions)
- `Published/v1.3/linux-x64.zip` — 79–81 MB (multiple)
- `Cartes/.../Published/v1/win-x64/Argumentum.AssetConverter.exe` — 79 MB (legacy tree)
- `Published/v1.3/win-x64.zip` + `.001/.002` splits — 50–72 MB

Purging `Published/`, `Cartes/.../Published/`, `Downloads/*.zip`, `*.resources` from history would reclaim **≈ 1.5–1.6 GB** → projected clone **< 200 MB**, matching the issue's Phase-2 estimate.

**Phase 2 is DESTRUCTIVE and gated on jsboige GO** (rewrites all SHAs → backup + coordinated force-push + all machines re-clone + old PR refs invalidated). Not actionable autonomously.

## Sources to PRESERVE (regenerable = False)

| At HEAD | Size | Nature |
|---|---:|---|
| `Cards/Packaging/*.ai/*.pdf/*.svg` (master box, FCPM, box designs) | **97 MB** | Illustrator/PDF design sources — not regenerable |
| `Generation/Sketch/argumentum.sketch` | **45 MB** | Sketch design source |
| `Cards/Fallacies/Assets/*.png` | 76 MB | Card art — pipeline + curated |

These are **Phase 3** candidates (Git LFS or external GDrive storage), again a jsboige decision. They must NOT be touched by a blind `filter-repo`.

## Note: Mindmap SVGs still tracked at HEAD (48 MB)

`Data/Mindmap/*.svg` (e.g. `Argumentum_Fallacies_MindMap_Fr_4.svg` 26 MB) are FreeMind-Batik output (regenerable) yet remain tracked. They were not covered by the Phase-1 untrack (PR #416). A small, safe follow-up could `git rm --cached` them — but they are byte-stable deliverables (#565) and some are referenced; leaving them is the conservative choice. Flagged, not actioned.

## Interim onboarding mitigation (non-destructive, actionable NOW)

The issue's original pain — *clone is very long on a new machine* — is **not blocked on Phase 2**. Configure/document a partial clone (keeps full history on demand):

```bash
# recommended: full history, blobs fetched lazily
git clone --filter=blob:none https://github.com/ArgumentumGames/Argumentum.git
# or fastest (no history):
git clone --depth=1 https://github.com/ArgumentumGames/Argumentum.git
```

No `partialclone.filter` default is currently set on the repo — new clones still fetch all 2 GB. Documenting the `--filter=blob:none` command in the README onboarding section (or setting a server-side default) unblocks new machines immediately, independent of the Phase-2 decision. **This is the one concrete, release-safe action this audit recommends.**

## Verdict

- **Phase 1**: ✅ DONE (#416 + #501). Close that section of the issue or mark it resolved.
- **Phase 2**: ⏸️ the only remaining weight-reducer; ~1.5 GB reclaimable; **jsboige-gated** (destructive).
- **Phase 3**: design sources (142 MB) → LFS/external; jsboige decision.
- **Interim**: document `--filter=blob:none` clone for onboarding (non-destructive).

Relates to #415, #416, #501, #134 (releases as the distributables home). Implements the "stale-dispatch" discipline: verify live state before acting on issue text.
