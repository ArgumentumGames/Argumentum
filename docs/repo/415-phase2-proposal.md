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

## Affinement (2026-07-03, base `9c19e51a`) — Mindmapper gap + refreshed pack

Re-measured on the current master (`9c19e51a`, was `33b1c0bc` at proposal time). Two findings sharpen
the plan:

**1. Pack grew, regenerable zones stable.** `size-pack` 2.05 → **2.08 GiB**, 3 → **4 packs**, in-pack
objects 25 857 → **29 725** (+3 868). The growth is **text/code only** (`other` zone 882.8 → 946.7 MB);
the regenerable binary zones (Published/, Downloads, .resources) are byte-for-byte unchanged. **Conclusion:
the bloat source is not recurring** — no new build artifacts were committed; the pack drift is ordinary
history growth. The strip target is unchanged in nature.

**2. GAP in the proposed `--path` set — 186.7 MB of Mindmapper build binaries missed.** The proposal's
`git filter-repo --path` prefixes (`.../AssetConverter/Published`, `Cartes/Generation/Converters`) are
**exact-prefix** matches. They do **not** catch two sibling build-output trees:

| Missed tree | History | Why missed |
|---|---:|---|
| `Generation/Converters/Argumentum.AssetConverter/Mindmapper/Published/` | **129.2 MB** | `.../AssetConverter/Mindmapper/Published` ≠ prefix `.../AssetConverter/Published` |
| `Cartes/Generation/Mindmap/Mindmapper/Published/` | **57.5 MB** | `Cartes/Generation/Mindmap` ≠ prefix `Cartes/Generation/Converters` |
| **Gap total** | **186.7 MB** | would survive the proposed rewrite |

Top missed blobs: `Mindmapper` osx-x64 (71.6 MB), `Mindmapper.exe` win-x64 (57.5 MB ×2 trees). These are
**regenerable `dotnet publish` output** — same class as the main `Published/`: the Mindmapper project
source (`.csproj`, `.sln`, `MindMapConfig.cs`) is committed in history, the binaries are deleted at HEAD
(0 at HEAD), and `.REMOVED.git-id` sentinel files alongside them confirm the team already un-tracked them
as build artifacts. **They belong in the strip.** (They were misclassified into the `other` zone by the
audit's path regex — which is why the proposal's 1.2 GB Published figure under-counted the true build-
binary weight.)

**Corrected strip target: ~1.8 GB regenerable** (proposal said ~1.557 GB; the +186.7 MB Mindmapper gap
was the difference). Net: a rewrite that ignores this gap leaves 186.7 MB on the table.

## The reduction opportunity (costed)

Targeting the **fully-regenerable zones** only (build artifacts, DNN zips, .resources):

| Target | Strippable history | Notes |
|---|---:|---|
| `Published/` .NET builds | **~1.2 GB** | `dotnet publish` output; 0 at HEAD |
| **Mindmapper/Published/ (2 trees)** | **~187 MB** | `dotnet publish` output; 0 at HEAD — **was missed, see affinement** |
| `DNNPlatform/.../Downloads/*.zip` | **~205 MB** | Print&Play pipeline output; 0 at HEAD |
| `DNNPlatform/.../{Install,ExtensionPackages}/*.resources` | **~150 MB** | re-downloadable 2sxc/DNN pkgs; 0 at HEAD |
| **Total regenerable** | **≈ 1.8 GB** | would take the repo from **2.08 GiB → ~0.45–0.5 GiB** |

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
#    NOTE (2026-07-03 affinement): the two Mindmapper/Published --path entries are REQUIRED —
#    the original 3-path set missed 186.7 MB of regenerable dotnet-publish output (see affinement).
pip install git-filter-repo
git filter-repo \
  --path DNNPlatform/Portals \
  --path "Generation/Converters/Argumentum.AssetConverter/Published" \
  --path "Generation/Converters/Argumentum.AssetConverter/Mindmapper/Published" \
  --path "Cartes/Generation/Converters" \
  --path "Cartes/Generation/Mindmap" \
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
   longer exist). **3 PRs are currently open** (#596, #661, #662 — down from 8 at proposal time;
   #627/#603/#597/#599/#600/#601 have since merged or closed). They must be merged or closed
   **before** the rewrite. *Lower risk than at proposal time.*
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
