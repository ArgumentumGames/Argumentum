# #415 Phase-2 — `.git` reduction proposal (READ-ONLY, no execution)

**Author**: po-2024 (worker) · **Date**: 2026-07-01 · **Base**: master `33b1c0bc`
**Status**: **PROPOSAL ONLY — no execution.** This surfaces the go/no-go and a costed plan for
jsboige to decide. **History rewriting is a jsboige decision** + a coordinated cluster force-push.
Nothing below has been run.

Builds on the Phase-1 audit ([#621](https://github.com/ArgumentumGames/Argumentum/pull/621),
[`tools/git-weight-audit.py`](../../tools/git-weight-audit.py), merged). All numbers in this doc are
reproduced by re-running that tool — this is its Phase-2 *interpretation*, not a second measurement.

## Measured weight (Phase-1 audit, reproduced this cycle)

- **`.git` pack**: **2.05 GiB** (3 packs, 25 857 in-pack objects).
- **History unique-path bytes** (largest version per path): **≈ 2.6 GB**.
- **HEAD checkout**: ≈ 780 MB.
- **No submodules** (DNNPlatform is a regular committed tree); single remote (`origin` → ArgumentumGames/Argumentum).

### History weight by zone (regenerability flagged)

| Zone | History | Files | At HEAD | Regenerable? |
|---|---:|---:|---:|---|
| **Published/ .NET builds** | **1.2 GB** | 42 | 0 B | ✅ `dotnet publish` |
| other (text/code + mixed) | 882.8 MB | 12 427 | 514 MB | mixed |
| **DNN Downloads zips** | **205.5 MB** | 12 | 0 B | ✅ pipeline output |
| **2sxc/DNN `.resources`** | **151.5 MB** | 197 | 0 B | ✅ re-downloadable |
| Packaging design | 96.9 MB | 20 | 96.9 MB | ❌ **DESIGN SOURCE — keep/LFS** |
| Card PNG assets | 49.9 MB | 143 | 76 MB | partial (pipeline) |
| Mindmap SVGs | 47.7 MB | 10 | 47.7 MB | ✅ FreeMind Batik (but committed-of-record #565) |
| Sketch design source | 45.1 MB | 2 | 45.1 MB | ❌ **DESIGN SOURCE — keep/LFS** |

### Top history blobs (largest version, deduped by path)

- `89.3 MB` — `DNNPlatform/Portals/1/Downloads/Argumentum_Print&Play.zip`
- `83.4 MB` — `Published/v1.3/osx-x64.zip`
- `80.6 MB` — `Published/v1.3/linux-x64.zip`
- `79.7 MB` — `Published/v1.1/linux-x64/linux-x64.zip`
- `79.5 MB` — `Cartes/.../Published/v1/win-x64/Argumentum.AssetConverter.exe` (legacy tree, gone at HEAD)
- `72.5 / 71.6 / 71.1 / 71.1 / 64.0 / 64.0 / 57.5 / 57.5 / 50.0 / 50.0 MB` — more `Published/` zips + legacy `Cartes/` builds

The `Published/` .NET build artifacts alone are **~1.2 GB of the 2.05 GiB** and are **100 % regenerable**
(0 at HEAD — they were committed then removed, but linger in history).

## The reduction opportunity (costed)

Targeting the **fully-regenerable zones** only (build artifacts, DNN zips, .resources):

| Target | Strippable history | Notes |
|---|---:|---|
| `Published/` .NET builds | **~1.2 GB** | `dotnet publish` output; 0 at HEAD |
| `DNNPlatform/.../Downloads/*.zip` | **~205 MB** | Print&Play pipeline output; 0 at HEAD |
| `DNNPlatform/.../{Install,ExtensionPackages}/*.resources` | **~150 MB** | re-downloadable 2sxc/DNN pkgs; 0 at HEAD |
| **Total regenerable** | **≈ 1.55–1.6 GB** | would take the repo from **2.05 GiB → ~0.45–0.5 GiB** |

**Out of scope for stripping** (design sources — preserve, or migrate to Git LFS separately):
- `Cards/Packaging/` (96.9 MB), `Generation/Sketch/argumentum.sketch` (45.1 MB) — these are
  **source-of-truth design assets**, not build output. LFS migration is a separate, lower-risk
  decision (does not require history rewrite for future files).

## Plan (if green-lit) — `git filter-repo`

`git filter-repo` is the current recommended tool (BFG is legacy). Concretely:

```bash
# 1. Fresh clone (filter-repo refuses a dirty/non-fresh clone by default)
git clone --no-local https://github.com/ArgumentumGames/Argumentum arg-reduce
cd arg-reduce

# 2. Strip regenerable build artifacts from ALL history
pip install git-filter-repo
git filter-repo \
  --path DNNPlatform/Portals \
  --path "Generation/Converters/Argumentum.AssetConverter/Published" \
  --path "Cartes/Generation/Converters" \
  --path-glob "*.resources" \
  --invert-paths

# 3. Re-mirror
git remote add origin https://github.com/ArgumentumGames/Argumentum
git push --force origin master   # ⚠️ coordinated (see risks)
```

**Estimated gain**: `.git` 2.05 GiB → **~0.45–0.5 GiB** (~75 % reduction), dominated by the `Published/` strip.

## Risks (explicit — this is why it's a decision, not a chore)

1. **History rewrite = force-push to `master`.** Every existing clone (ai-01, po-2023, po-2024, any
   contributor) is invalidated and must **re-clone**. Worker downtime during the window. The 3
   cluster machines all pull from `origin` — a coordinated re-clone is required or they diverge.
2. **Open PRs break.** Any PR open at rewrite time will not rebase cleanly (its base commits no
   longer exist). **8 PRs are currently open** (#627, #603, #596, #597, #599, #600, #601, #627-area).
   They must be merged or closed **before** the rewrite.
3. **Tag / release integrity.** If `v0.9.0` is tagged on the **old** history, the rewrite invalidates
   it (the tag points at a commit that no longer exists in the new chain). Either (a) tag **after**
   the rewrite, or (b) accept re-tagging. **Strong reason to either rewrite before the v0.9.0 tag, or
   defer entirely until after the release is shipped.**
4. **SHA shift cascades.** roosync conversation archives, dashboard references, GDrive bundles, and
   investigation reports cite commit SHAs that **will all change**. These become stale-but-harmless
   (they're documentation, not code), but it's noise.
5. **`filter-repo` removes the `origin` remote** by design after a run (anti-foot-gun) — must be
   re-added before push.
6. **Mindmap SVGs** (47.7 MB) are regenerable (#565) but are the committed-of-record; including them
   in the strip is optional and should be confirmed with the mindmap lane (ai-01) — **left OUT of
   the proposed `--path` set above** to be conservative.

## Recommendation

**DEFER to post-v0.9.0-tag.** Concretely:

- **Now (pre-tag freeze): do nothing.** Rewriting history during the freeze — with 8 open PRs and a
  pending tag — maximizes risk for ~zero release benefit (clone size doesn't affect the shipped
  artifacts).
- **Post-tag (Phase-3, when green-lit):** execute the `git filter-repo` plan above in a coordinated
  window: (1) close/merge all PRs, (2) tag v0.9.0 on the old history **or** agree to tag after,
  (3) rewrite + force-push, (4) announce the re-clone on the dashboard, (5) each worker re-clones.

**Go/no-go = jsboige.** This proposal surfaces the ~1.55 GB gain and the 6 risks; it does not execute.

## What this doc does NOT do

- **No `git filter-repo` / BFG run.** No clone, no force-push, no history mutation.
- **No deletion of design sources** (Packaging, Sketch) — those are flagged preserve/LFS, a separate
  decision.
- **No `.gitignore` change** for future artifacts — that is a worthwhile companion PR (prevent
  re-committing `Published/`), but is **orthogonal** to the history rewrite and can proceed
  independently and safely now. Flagged as a follow-up, not included here.

Relates to #415, #621, #565. Honors pre-tag freeze (0 `Cards/` write, 0 history mutation, read-only
analysis). Reproducible via `python tools/git-weight-audit.py`.
