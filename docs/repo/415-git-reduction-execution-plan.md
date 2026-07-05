# #415 Phase-2 — `.git` reduction EXECUTION PLAN (read-only, no execution)

**Author**: po-2024 (worker) · **Date**: 2026-07-05 · **Base**: master `70bd1605`
**Status**: **EXECUTION PLAN ONLY — no run.** This turns the [Phase-2 proposal](415-phase2-proposal.md)
(analysis + costing) into a concrete, step-by-step, cluster-coordinated *execution* playbook.
**History rewriting is a jsboige decision** + a coordinated cluster force-push.
**Nothing below has been run. 0 history mutation, 0 force-push, 0 `Cards/` write.**

Builds on:
- [Phase-1 audit](415-git-weight-audit.md) + [`tools/git-weight-audit.py`](../../tools/git-weight-audit.py) (merged #621)
- [Phase-2 proposal](415-phase2-proposal.md) (analysis + costing + the `filter-repo` strip set)
- [2026-07-03 affinement](415-phase2-proposal.md#affinement-2026-07-03-base-9c19e51a--mindmapper-gap--refreshed-pack) (Mindmapper +186.7 MB gap fix)

---

## 1. Empirical inventory (re-measured this cycle, master `70bd1605`, read-only)

| Metric | Value | How measured |
|---|---:|---|
| `.git` total | **2.2 GB** | `du -sh .git` |
| `size-pack` | **~2.1 GB** (4 packs) | `git count-objects -vH` equivalent |
| In-pack objects | ~29 700 | `git verify-pack -v` aggregate |
| Open PRs (risk §2) | **4** (#596, #666, #674, #686) | `gh pr list` |

### Top-5 history blobs (largest version per path, read-only `git verify-pack` + `rev-list --objects`)

| Bytes | Path | Class |
|---:|---|---|
| 93.6 MB | `DNNPlatform/Portals/1/Downloads/Argumentum_Print&Play.zip` | DNN zip (regenerable) |
| 86.4 MB | `Generation/Converters/Argumentum.AssetConverter/Published/v1.3/osx-x64.zip` | .NET build (regenerable) |
| 83.6 MB | `.../Published/v1.1/linux-x64/linux-x64.zip` | .NET build (regenerable) |
| 83.5 MB | `.../Published/v1.3/linux-x64.zip` | .NET build (regenerable) |
| 83.2 MB | `.../Published/v1.3/linux-x64.zip` | .NET build (regenerable) |

**Confirmation**: the bloat source is unchanged since the proposal — `Published/` .NET builds (~1.2 GB) +
`DNNPlatform/.../Downloads/*.zip` (~205 MB) + `.resources` (~150 MB) + `Mindmapper/Published/` (~187 MB) dominate.
All are **regenerable** (0 at HEAD, `dotnet publish` / pipeline output / re-downloadable 2sxc packages).

### Strip target (corrected, post-affinement)

| Target | Strippable history | Status at HEAD | Regenerable? |
|---|---:|---:|---|
| `Published/` .NET builds | ~1.2 GB | 0 B | ✅ `dotnet publish` |
| `Mindmapper/Published/` (2 trees) | ~187 MB | 0 B | ✅ `dotnet publish` |
| `DNNPlatform/.../Downloads/*.zip` | ~205 MB | 0 B | ✅ pipeline output |
| `DNNPlatform/.../{Install,ExtensionPackages}/*.resources` | ~150 MB | 0 B | ✅ re-downloadable |
| **Total regenerable** | **≈ 1.8 GB** | | |

**Out of scope** (design sources — preserve, LFS is a separate non-rewrite decision):
`Cards/Packaging/` (96.9 MB), `Generation/Sketch/argumentum.sketch` (45.1 MB), committed Mindmap SVGs (47.7 MB, left OUT conservatively per proposal risk §6).

---

## 2. Projection post-purge (estimated)

| | Before | After (est.) | Gain |
|---|---:|---:|---:|
| `.git` total | 2.2 GB | **~0.45–0.5 GB** | **~75 %** |
| Fresh clone weight | ~2.2 GB | ~0.5 GB | ~1.7 GB saved per clone |
| `size-pack` | 2.1 GB | ~0.4 GB | dominated by `Published/` strip |

Projection method: the strip removes 1.8 GB of history bytes; the residual ~0.45 GB is text/code + the preserved design sources + Mindmap SVGs. Consistent with the proposal's estimate (proposal said ~0.45–0.5 GiB on a 2.05 GiB base; the +0.15 GB base drift since is text/code, also stripped proportionally by the rewrite's dedup).

---

## 3. Execution playbook (cluster-coordinated, **GATED — jsboige go/no-go**)

### Pre-flight (T-1, jsboige decision first)
- [ ] **jsboige green-lights** the rewrite (history rewrite + force-push to `master`).
- [ ] **Timing**: either (a) **before** the `v0.9.0` tag (tag on the new clean history), or (b) **defer to post-release** (proposal reco = DEFER). Decide explicitly.
- [ ] **All 4 open PRs** (#596, #666, #674, #686) are **merged or closed** first — they will not rebase after the rewrite (base commits cease to exist).
- [ ] Announce a **cluster re-clone window** on the dashboard (workers will be down during it).

### Execution (T0, single operator — jsboige or one delegated worker on a fresh clone)
```bash
# 1. Fresh clone (filter-repo refuses a dirty/non-fresh clone by default)
git clone --no-local https://github.com/ArgumentumGames/Argumentum arg-reduce
cd arg-reduce

# 2. Install git-filter-repo (current recommended tool; BFG is legacy)
pip install git-filter-repo

# 3. Strip the regenerable build artifacts from ALL history.
#    --invert-paths = remove the listed paths, keep everything else.
#    The two Mindmapper/Published entries are REQUIRED (2026-07-03 affinement: +187 MB missed
#    by the original 3-path set).
git filter-repo \
  --path DNNPlatform/Portals \
  --path "Generation/Converters/Argumentum.AssetConverter/Published" \
  --path "Generation/Converters/Argumentum.AssetConverter/Mindmapper/Published" \
  --path "Cartes/Generation/Converters" \
  --path "Cartes/Generation/Mindmap" \
  --path-glob "*.resources" \
  --invert-paths

# 4. Verify the result LOCALLY before any push
du -sh .git                           # expect ~0.45–0.5 GB
git log --oneline | wc -l             # commit count preserved (only tree contents change)
git fsck --full                       # integrity check
python tools/git-weight-audit.py      # confirm zones stripped, design sources intact

# 5. Re-add origin (filter-repo removes it by design as an anti-foot-gun)
git remote add origin https://github.com/ArgumentumGames/Argumentum

# 6. COORDINATED force-push (announce on dashboard immediately before)
git push --force origin master
```

### Post-rewrite (T+1, every cluster machine + contributor)
- [ ] **Dashboard announce**: "master rewritten at <new-SHA>; every clone invalidated, re-clone required".
- [ ] **ai-01, po-2023, po-2024**: each re-clone (delete local checkout, `git clone origin`). No `git pull` will work — histories diverged.
- [ ] **Tag v0.9.0** on the new history (if option (a) chosen), or confirm the existing tag strategy.
- [ ] **Verify** the 4 closed/merged PRs' content survives (re-open equivalent if any was force-closed prematurely).
- [ ] **Follow-up PR** (orthogonal, can proceed now): tighten `.gitignore` to prevent re-committing `Published/` (proposal §"What this doc does NOT do").

---

## 4. Risks (carried from the proposal, refreshed)

1. **History rewrite = force-push to `master`** → every existing clone invalidated, coordinated re-clone required or workers diverge.
2. **Open PRs break** → 4 currently open (#596/#666/#674/#686) must merge/close first.
3. **Tag/release integrity** → if `v0.9.0` is tagged on old history, rewrite invalidates it. Decide tag-before vs tag-after explicitly.
4. **SHA shift cascades** → roosync archives, dashboard refs, GDrive bundles, investigation reports cite SHAs that all change (stale-but-harmless docs noise).
5. **`filter-repo` removes `origin`** by design → re-add before push.
6. **Mindmap SVGs** (47.7 MB) — left OUT of the strip conservatively (committed-of-record #565); confirm with mindmap lane before including.

---

## 5. Gate (explicit)

**EXECUTION OF THIS PLAN IS GATED ON jsboige.** This document:
- ❌ Does **not** run `git filter-repo` / BFG, no clone, no force-push, no history mutation.
- ❌ Does **not** delete design sources (Packaging, Sketch) — LFS is a separate non-rewrite decision.
- ❌ Does **not** change `.gitignore` (companion PR, orthogonal, can proceed independently).
- ✅ Provides a **concrete, cluster-coordinated playbook** ready to execute when jsboige green-lights, with the pre-flight checklist, the exact strip set, the verification steps, and the post-rewrite re-clone coordination.

**Recommendation (unchanged from proposal): DEFER to post-v0.9.0-tag** (rewrite during the freeze, with 4 open PRs and a pending tag, maximizes risk for ~zero release benefit — clone size does not affect shipped artifacts).

---

Relates to #415, #621. Honors release freeze (0 `Cards/` write, 0 history mutation, read-only analysis). Reproducible via `python tools/git-weight-audit.py`. Base `70bd1605`.
