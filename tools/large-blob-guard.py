#!/usr/bin/env python3
"""#415 Phase 3 — CI ratchet against NEW oversized blobs entering the repo.

Why (#415): the clone weighs ~2.2 GB because build artifacts and vendored
binaries were committed historically (Published/ .NET builds, DNN Downloads
zips, .resources packages — see #621/#628). Those are already absent from HEAD;
this guard makes sure the class of mistake does not come back: any blob NEWER
than the comparison base that exceeds the threshold and sits outside the
allow-list fails the build.

Semantics (blob-level, rename-safe):
    git rev-list --objects <head> --not <base>
lists exactly the objects reachable from head but not from base. A renamed
file keeps its blob oid, so pure renames never trip the guard; a modified
file gets a new oid and is checked; deletions add nothing. Git-LFS pointer
files are ~130-byte text blobs, far below the threshold, and pass by
construction.

Threshold: 2 MiB (2 097 152 bytes), strictly greater rejects. Chosen from the
measured HEAD distribution on master `1160be83` (2026-09-11): 116 files > 1 MiB,
72 > 2 MiB, 27 > 5 MiB, 5 > 10 MiB — every > 2 MiB file falls in an allow-listed
family below. 2 MiB is also the no-drift line the #415 dossier itself has been
tracking since 2026-07 ("0 new blob > 2 MB committed since 2026-07-01").

Allow-list (validated by `--audit-head`: 0 unlisted blob > 2 MiB at HEAD):
    Cards/Fallacies/Mindmaps/                                       regenerated mindmap SVG/HTML, committed-of-record (29 files > 2 MiB)
    Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv             live editorial taxonomy CSV (4.1 MB, touched by every editorial PR)
    Cards/Virtues/Argumentum Virtues - Taxonomy.csv                 editorial data of record (currently < 2 MiB, pre-allowed for symmetry)
    Cards/Fallacies/Archive/                                        archives of record
    Cards/Fallacies/Assets/                                         card input art (PNG)
    Cards/Packaging/                                                design sources — preserve/LFS per #628
    Generation/Sketch/                                              design source — preserve/LFS per #628
    Generation/CardPen/images/                                      vendored CardPen images
    Generation/Converters/Argumentum.AssetConverter/Data/Mindmap/   mindmap input data SVGs (incl. one 26.7 MB)
    docs/ontology/                                                  generated OWL
    DNNPlatform/                                                    vendored DNN platform export (bulk platform updates)
Adding an entry is a deliberate, reviewable act: extend ALLOWED below with a
one-line rationale and cite the issue.

Usage:
    python tools/large-blob-guard.py --base origin/master [--head HEAD]
        Fail (exit 1) if any NEW blob > threshold is outside the allow-list.
    python tools/large-blob-guard.py --audit-head [--head HEAD]
        Fail if any blob > threshold at HEAD is outside the allow-list
        (proves the allow-list still covers the existing tree).
    python tools/large-blob-guard.py --self-test
        Build a scratch repo and exercise addition / rename / deletion /
        modification / LFS-pointer / exact-threshold cases.

Known limitation: rev-list reports one path per new blob, so a new blob placed
simultaneously at an allowed and a non-allowed path is judged by its reported
path only. Content already present at base never re-triggers (rename-safe by
design).
"""
import argparse
import os
import subprocess
import sys
import tempfile

DEFAULT_THRESHOLD = 2 * 1024 * 1024  # 2 MiB — see module docstring

# Directory prefixes must end with "/". Bare paths are exact file matches.
ALLOWED = [
    "Cards/Fallacies/Mindmaps/",                                      # regenerated mindmap SVG/HTML (#415 status-checks)
    "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv",            # live editorial taxonomy (4.1 MB)
    "Cards/Virtues/Argumentum Virtues - Taxonomy.csv",                # editorial data of record (symmetry)
    "Cards/Fallacies/Archive/",                                       # archives of record
    "Cards/Fallacies/Assets/",                                        # card input art
    "Cards/Packaging/",                                               # design sources — preserve/LFS (#628)
    "Generation/Sketch/",                                             # design source — preserve/LFS (#628)
    "Generation/CardPen/images/",                                     # vendored CardPen images
    "Generation/Converters/Argumentum.AssetConverter/Data/Mindmap/",  # mindmap input data
    "docs/ontology/",                                                 # generated OWL
    "DNNPlatform/",                                                   # vendored DNN platform export
]


def is_allowed(path, allow_list):
    return any(path == e or (e.endswith("/") and path.startswith(e)) for e in allow_list)


def git(*args, cwd=None, check=True):
    r = subprocess.run(["git"] + list(args), capture_output=True, text=True,
                       encoding="utf-8", errors="replace", cwd=cwd)
    if check and r.returncode != 0:
        raise RuntimeError(f"git {' '.join(args)} failed: {r.stderr.strip()}")
    return r.stdout


def objects_with_paths(rev_args, cwd=None):
    """[(oid, path_or_None)] from `git rev-list --objects ...` (one path per object)."""
    out = git("rev-list", "--objects", *rev_args, cwd=cwd)
    entries = []
    for line in out.splitlines():
        line = line.strip()
        if not line:
            continue
        parts = line.split(" ", 1)
        entries.append((parts[0], parts[1] if len(parts) > 1 else None))
    return entries


def blob_sizes(oids, cwd=None):
    """{oid: size} via cat-file batch-check."""
    if not oids:
        return {}
    stdin = "\n".join(oids) + "\n"
    r = subprocess.run(["git", "cat-file", "--batch-check=%(objectname) %(objecttype) %(objectsize)"],
                       input=stdin, capture_output=True, text=True, encoding="utf-8",
                       errors="replace", cwd=cwd)
    if r.returncode != 0:
        raise RuntimeError(f"cat-file failed: {r.stderr.strip()}")
    sizes = {}
    for line in r.stdout.splitlines():
        parts = line.split()
        if len(parts) == 3 and parts[1] == "blob":
            sizes[parts[0]] = int(parts[2])
    return sizes


def oversized_new_blobs(base, head, threshold, allow_list, cwd=None):
    """[(path, oid, size)] for NEW blobs > threshold outside the allow-list."""
    entries = objects_with_paths([head, "--not", base], cwd=cwd)
    sizes = blob_sizes([oid for oid, _ in entries], cwd=cwd)
    violations = []
    for oid, path in entries:
        size = sizes.get(oid)
        if size is None or size <= threshold:
            continue
        if path is not None and not is_allowed(path, allow_list):
            violations.append((path, oid, size))
    return sorted(violations, key=lambda v: -v[2])


def oversized_head_blobs(head, threshold, allow_list, cwd=None):
    """[(path, oid, size)] for blobs > threshold in the CURRENT TREE of head
    (ls-tree snapshot — NOT the full reachable history) outside the allow-list."""
    out = git("ls-tree", "-r", "-l", head, cwd=cwd)
    violations = []
    for line in out.splitlines():
        # <mode> SP <type> SP <oid> SP <size> TAB <path>
        meta, _, path = line.partition("\t")
        parts = meta.split()
        if len(parts) != 4 or parts[1] != "blob":
            continue
        oid, size = parts[2], int(parts[3])
        if size > threshold and not is_allowed(path, allow_list):
            violations.append((path, oid, size))
    return sorted(violations, key=lambda v: -v[2])


def report(violations, threshold, mode_desc):
    if not violations:
        print(f"large-blob-guard: OK — 0 unlisted blob > {threshold} bytes ({mode_desc})")
        return 0
    print(f"large-blob-guard: FAIL — {len(violations)} new oversized blob(s) "
          f"> {threshold} bytes outside the allow-list ({mode_desc}):")
    for path, oid, size in violations:
        print(f"  {size:>12,} B  {oid[:12]}  {path}")
    print("\nRemediation: shrink/split the file, store it via Git LFS or a GitHub "
          "Release, or extend ALLOWED in tools/large-blob-guard.py with a rationale "
          "citing the issue (see module docstring).")
    return 1


# ── self-test ────────────────────────────────────────────────────────────────

def _write(path, size, repo):
    os.makedirs(os.path.dirname(os.path.join(repo, path)) or repo, exist_ok=True)
    with open(os.path.join(repo, path), "wb") as f:
        f.write(os.urandom(size))


def _commit(repo, msg):
    git("add", "-A", cwd=repo)
    git("commit", "-m", msg, cwd=repo)


def self_test():
    """Scratch-repo test: addition, rename, deletion, modification, LFS pointer,
    exact-threshold boundary. Returns process exit code."""
    cases = []
    with tempfile.TemporaryDirectory(prefix="lbg-selftest-") as repo:
        def sgit(*args):
            return git(*args, cwd=repo)

        sgit("init", "-q")
        # Repo-level identity: CI runners have no global git config, and the
        # scenario commits go through _commit() → module-level git() without -c.
        sgit("config", "user.name", "t")
        sgit("config", "user.email", "t@t")
        sgit("commit", "-q", "--allow-empty", "-m", "base0")
        # base: existing oversized blob at a NON-listed path (must never re-trigger)
        _write("data/keep.bin", 2_500_000, repo)
        _commit(repo, "base: keep.bin")
        base_sha = sgit("rev-parse", "HEAD").strip()

        # 1) addition at non-listed path → violation; listed path → allowed;
        #    exact-threshold file → allowed (strictly greater rejects)
        _write("build/out.zip", 3_000_000, repo)
        _write("Cards/Packaging/new.ai", 3_000_000, repo)
        _write("assets/at-threshold.png", DEFAULT_THRESHOLD, repo)
        _commit(repo, "c1 additions")
        cases.append(("addition flagged", oversized_new_blobs("HEAD~1", "HEAD", DEFAULT_THRESHOLD, ALLOWED, cwd=repo),
                      ["build/out.zip"]))

        # 2) rename of an existing >threshold blob to a non-listed path → no new violation
        os.rename(os.path.join(repo, "data/keep.bin"), os.path.join(repo, "build/renamed.bin"))
        _commit(repo, "c2 rename")
        cases.append(("rename passes", oversized_new_blobs("HEAD~1", "HEAD", DEFAULT_THRESHOLD, ALLOWED, cwd=repo), []))

        # 3) deletion → nothing new
        os.remove(os.path.join(repo, "build/renamed.bin"))
        _commit(repo, "c3 delete")
        cases.append(("deletion passes", oversized_new_blobs("HEAD~1", "HEAD", DEFAULT_THRESHOLD, ALLOWED, cwd=repo), []))

        # 4) LFS-pointer-style small text at a big-file extension → under threshold
        with open(os.path.join(repo, "assets/video.mp4"), "wb") as f:
            f.write(b"version https://git-lfs.github.com/spec/v1\noid sha256:" + b"a" * 64 + b"\n")
        _commit(repo, "c4 lfs pointer")
        cases.append(("lfs pointer passes", oversized_new_blobs("HEAD~1", "HEAD", DEFAULT_THRESHOLD, ALLOWED, cwd=repo), []))

        # 5) modification of an existing oversized file → NEW blob at non-listed path → flagged
        _write("data/keep.bin", 2_500_000, repo)
        _commit(repo, "c5 modify keep.bin")
        cases.append(("modification flagged", oversized_new_blobs("HEAD~1", "HEAD", DEFAULT_THRESHOLD, ALLOWED, cwd=repo),
                      ["data/keep.bin"]))

        # 6) full-range view from the base commit (which already holds the original
        #    keep.bin blob): exactly the two flagged additions — the renamed copy of
        #    the base blob stays excluded because the blob is reachable from base
        got = sorted(p for p, _, _ in oversized_new_blobs(base_sha, "HEAD", DEFAULT_THRESHOLD, ALLOWED, cwd=repo))
        cases.append(("cumulative range", got, ["build/out.zip", "data/keep.bin"]))

    failed = 0
    for name, got, want in cases:
        got_paths = sorted(p for p, _, _ in got) if isinstance(got, list) and got and isinstance(got[0], tuple) else got
        ok = got_paths == want
        print(f"  [{'PASS' if ok else 'FAIL'}] {name}: got {got_paths}, want {want}")
        failed += 0 if ok else 1
    print(f"self-test: {len(cases) - failed}/{len(cases)} passed")
    return 1 if failed else 0


def main():
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--base", help="comparison base ref (check mode)")
    ap.add_argument("--head", default="HEAD", help="head ref (default HEAD)")
    ap.add_argument("--threshold", type=int, default=DEFAULT_THRESHOLD,
                    help=f"max allowed blob size in bytes (default {DEFAULT_THRESHOLD})")
    ap.add_argument("--audit-head", action="store_true",
                    help="scan ALL blobs at --head against the allow-list")
    ap.add_argument("--self-test", action="store_true", help="run the scratch-repo test suite")
    args = ap.parse_args()

    if args.self_test:
        sys.exit(self_test())

    if args.audit_head:
        sys.exit(report(oversized_head_blobs(args.head, args.threshold, ALLOWED),
                        args.threshold, f"audit of {args.head}"))

    if not args.base:
        ap.error("--base is required in check mode (or use --audit-head / --self-test)")
    if set(args.base) == {"0"}:  # push event with no previous commit (branch creation)
        print("large-blob-guard: SKIP — base is the zero sha (no previous commit to compare)")
        sys.exit(0)
    sys.exit(report(oversized_new_blobs(args.base, args.head, args.threshold, ALLOWED),
                    args.threshold, f"{args.head} not reachable from {args.base}"))


if __name__ == "__main__":
    main()
