# #415 — .git repo slimming (2.2 GB) : scoping & options

**Issue**: [#415 — réduction poids dépôt (.git 2,2 GB)](https://github.com/ArgumentumGames/Argumentum/issues/415)
**Track**: #458 Track 5 (dette technique / infra)
**Author**: po-2024 (worker backlog)
**Date**: 2026-06-14
**Base**: master `2e4f6735`
**Status**: Scoping (key-free analysis) — **execution gated on jsboige + 3-person coordination** (per #415 decision: requires backups + coordinated force-push)

> Scope of this document: **analysis only**. No history rewritten, no
> force-push. Execution is explicitly gated on jsboige (it rewrites shared
> history and affects every clone).

---

## 1. Measured reality (factual, from `2e4f6735`)

| Metric | Value |
|---|---|
| `.git` total | **2.1 GB** |
| Packed objects | 2.05 GiB in 3 packs |
| Objects in pack | 25 857 |
| Loose / garbage | 19.98 MiB loose, **0 garbage** (already `gc`-clean) |
| `git count-objects -vH` | confirms above |

**`.git` is already optimally packed** — a plain `git gc --aggressive` will
recover **~nothing** (0 garbage). The size is entirely **historical binary
content** committed before the gitignore rules existed.

## 2. What's eating the history (blob-size breakdown)

Computed via `git rev-list --objects --all | git cat-file --batch-check` over
**all unique blobs** in history (15 121 blobs, 5 982 MB unique content):

| Category | Blobs | MiB | % of unique |
|---|---:|---:|---:|
| **`Published/` build binaries (.zip/.exe/.dll)** | **154** | **4 361** | **73%** |
| other source/text | 11 423 | 906 | 15% |
| images (.png/.svg) | 3 215 | 363 | 6% |
| other .zip | 11 | 128 | 2% |
| Print&Play zip | 8 | 89 | 1.5% |
| other binaries (.exe/.dll) | 296 | 88 | 1.5% |
| .pdf | 14 | 47 | <1% |

**Single dominant target**: `Published/` self-contained build zips/exes/dlls
(the `dotnet publish` outputs) = **4.36 GB = 73% of all unique history content**.

Top individual blobs (all build artifacts):
```
90M  DNNPlatform/Portals/1/Downloads/Argumentum_Print&Play.zip
84M  .../Argumentum.AssetConverter/Published/v1.3/osx-x64.zip   (x5 versions)
81M  .../Published/v1.3/linux-x64.zip
80M  .../Published/v1.1/linux-x64/linux-x64.zip
80M  Cartes/.../Published/v1/win-x64/Argumentum.AssetConverter.exe
```

## 3. Current state (why it's safe to target `Published/`)

- **`.gitignore` already covers it**: lines 20 (`*.exe`), 243-244 (`**/Published/`),
  247 (`DNNPlatform/Portals/*/Downloads/*.zip`). **VÉRIFIÉ**.
- **`Published/` is NOT in the working tree** anymore — the gitignore was added
  *after* these binaries were committed, so they survive **only in history**.
- Removing them from history loses **no current functionality** — they are
  regenerable build outputs (`dotnet publish`), not source.

## 4. Options

### Option A — `git filter-repo` (recommended, modern)
Rewrite history to strip `**/Published/`, `**/Published/**`, and the DNN
`Downloads/*.zip` paths.

- **Tool**: `git filter-repo` (official successor to BFG/filter-branch; installed
  via `pip install git-filter-repo`, **not present on this machine today**).
- **Pro**: fast, path/glob-based, handles all refs/tags at once, produces clean
  commit map.
- **Con**: **rewrites every commit** → force-push required → every clone must
  re-clone. This is the #415 "3-person coordination" cost.
- **Expected gain**: ~4.36 GB → `.git` should drop to **~500 MB–1 GB** (the
  remaining source/images/text). Possibly more with `--strip-blobs-bigger-than`.

### Option B — BFG Repo-Cleaner
Java jar, deletes big blobs by size (`--strip-blobs-bigger-than 10M`).

- **Pro**: dead-simple, well-known.
- **Con**: size-based (blunt) rather than path-based; also rewrites history;
  requires Java. `filter-repo` is strictly better for path-targeted removal.

### Option C — Fresh re-init / "squash everything"
Archive current history as a tag, start a new repo with a single squashed
commit.

- **Pro**: smallest possible `.git`; clean slate.
- **Con**: **destroys all git history** (blame, archaeology, the investigation
  reports that cite commits). **Rejected** — Argumentum's pipeline recovery
  (golden master `0087f0ec`, regression archaeology) depends on history. Not
  recommended.

### Option D — Defer (status quo)
Keep the 2.1 GB repo; rely on shallow/partial clones for CI if clone time
matters.

- **Pro**: zero risk, zero coordination.
- **Con**: clone is slow (~2 GB every clone), fresh clones on po-2023 (the
  disk-critical machine) waste ~2 GB. Doesn't fix the root cause.

## 5. Recommendation

**Option A (`git filter-repo`)**, gated on the 3-person coordination already
flagged in #415, **after** v0.9.0 release (#134). Reasoning:
1. `Published/` = 73% of history, already gitignored, absent from working tree,
   fully regenerable → **zero source risk**.
2. `filter-repo` is path-precise (won't touch source/images accidentally).
3. But it **rewrites shared history** → every contributor's clone breaks →
   must be a **coordinated, announced force-push** with backups. This is exactly
   why #415 needs jsboige + 3 people, not an autonomous worker run.
4. **Timing**: don't do it mid-release-gate. Sequence it as the **first post-
   release cleanup** (Track 5), alongside tag v0.9.0 so the release itself is
   preserved on the pre-rewrite history if ever needed.

## 6. Execution plan (post-decision, for the coordinated run)

```bash
# 0. BACKUP first (coordination step, jsboige)
git clone --mirror <repo> argumentum.backup.$(date +%F).git

# 1. Install git-filter-repo (not present today)
pip install git-filter-repo

# 2. Rewrite — strip build artifacts from ALL history
git filter-repo \
  --path-glob '**/Published/**' \
  --path-glob 'Cartes/Generation/**/Published/**' \
  --path 'DNNPlatform/Portals/1/Downloads/Argumentum_Print&Play.zip' \
  --invert-paths

# 3. Repack + gc
git reflog expire --expire=now --all
git gc --prune=now --aggressive

# 4. Force-push (coordination: announce, all contributors re-clone)
git push --force origin --all
git push --force origin --tags
```

**Expected result**: `.git` 2.1 GB → ~0.5-1 GB. Every existing clone must be
re-cloned (the coordination cost).

## 7. Decisions needed from jsboige (the 3-person coordination)

1. **Go / no-go** on Option A post-v0.9.0? (recommendation: yes, after release)
2. **Who are the 3 people** to coordinate? (per #415: requires backups + everyone
   re-clones)
3. **Preserve pre-rewrite history as a tag/branch?** (recommend: yes — tag
   `v0.9.0` on current history before rewriting, so the release is recoverable)
4. **Should CI switch to shallow clones meanwhile?** (Option D partial, low-effort
   mitigation until the full cleanup)

---

_Generated by po-2024 (worker backlog), key-free scoping. No history rewritten,
no force-push, no code changed. Execution gated on jsboige + 3-person
coordination._
