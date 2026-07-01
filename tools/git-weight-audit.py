#!/usr/bin/env python3
"""#415 repo-weight audit — read-only, reproducible.

Measures .git weight: pack size, top history blobs (deduped by path),
top HEAD files, and aggregates by zone with a regenerability flag.
0 write (read-only git plumbing). Run: python tools/git-weight-audit.py
"""
import subprocess, sys, re
from collections import defaultdict

TOP = 15  # per-listing limit

def git(*args):
    r = subprocess.run(["git"]+list(args), capture_output=True, text=True, errors="replace")
    return r.stdout

def human(n):
    for u in ("B","KB","MB","GB"):
        if n < 1024: return f"{n:.1f} {u}"
        n /= 1024
    return f"{n:.1f} TB"

# --- zones (path-prefix -> (name, regenerable, rationale)) ---
ZONES = [
 (r"Generation/Converters/Argumentum.AssetConverter/Published/", "Published/ .NET builds", True,  "dotnet publish (regenerable)"),
 (r"Cartes/Generation/Converters/.*Published/",                   "Legacy Cartes/ builds", True, "legacy tree, deleted at HEAD"),
 (r"DNNPlatform/Portals/.*/Downloads/.*\.zip",                    "DNN Downloads zips",   True,  "pipeline output (Print&Play)"),
 (r"DNNPlatform/.*/(Install|ExtensionPackages)/.*\.resources",    "2sxc/DNN .resources",  True,  "re-downloadable install pkgs"),
 (r"Generation/Sketch/.*\.sketch",                                "Sketch design source", False, "DESIGN SOURCE — preserve/LFS"),
 (r"Cards/Packaging/",                                            "Packaging design",     False, "DESIGN SOURCE — preserve/LFS"),
 (r"Cards/Fallacies/Assets/.*\.(png|jpg)",                        "Card PNG assets",      "part", "pipeline + curated"),
 (r"Data/Mindmap/.*\.svg",                                        "Mindmap SVGs",          True,  "FreeMind Batik (regenerable)"),
]

def zone_of(path):
    for pat, name, regen, _ in ZONES:
        if re.search(pat, path): return name, regen
    return "other (text/code)", None

def main():
    print("="*70); print(" #415 REPO-WEIGHT AUDIT"); print("="*70)
    # 1. count-objects
    co = git("count-objects","-vH")
    pack = {l.split(": ")[0]: l.split(": ")[1] for l in co.splitlines() if ": " in l}
    print(f"\n[pack] size-pack = {pack.get('size-pack','?')} | in-pack = {pack.get('in-pack','?')} | packs = {pack.get('packs','?')}")

    # 2. history blobs (deduped by path, keep largest)
    raw = git("rev-list","--objects","--all")
    batch_in = "\n".join(l for l in raw.splitlines() if l)
    # batch-check needs object list; feed rev-list objects via stdin
    objs = subprocess.run(["git","cat-file","--batch-check=%(objecttype) %(objectname) %(objectsize) %(rest)"],
                          input=raw, capture_output=True, text=True, errors="replace").stdout
    hist_path_max = {}  # path -> max bytes
    for line in objs.splitlines():
        p = line.split(" ",3)
        if len(p)<4 or p[0]!="blob": continue
        try: sz=int(p[2])
        except: continue
        path=p[3].strip()
        if path and sz>hist_path_max.get(path,0): hist_path_max[path]=sz

    # 3. HEAD files
    head = git("ls-tree","-r","-l","HEAD")
    head_files=[]  # (bytes, path)
    for line in head.splitlines():
        p=line.split(None,4)
        if len(p)<5: continue
        try: sz=int(p[3])
        except: continue
        head_files.append((sz, p[4].strip()))

    # 4. zone aggregation
    print(f"\n[top {TOP} history blobs (deduped by path, largest version)]")
    for path,sz in sorted(hist_path_max.items(), key=lambda x:-x[1])[:TOP]:
        print(f"  {human(sz):>11}  {path}")
    print(f"\n[top {TOP} HEAD files]")
    for sz,path in sorted(head_files, reverse=True)[:TOP]:
        print(f"  {human(sz):>11}  {path}")

    # zone totals: history (sum of unique-path max) + HEAD (sum of present)
    zh = defaultdict(int); zh_n=defaultdict(int)   # history bytes + count
    zhead=defaultdict(int); zhead_n=defaultdict(int)
    regen={}
    for path,sz in hist_path_max.items():
        z,r = zone_of(path); zh[z]+=sz; zh_n[z]+=1; regen[z]=r
    for sz,path in head_files:
        z,r = zone_of(path); zhead[z]+=sz; zhead_n[z]+=1; regen[z]=r

    print("\n[zone aggregation]")
    print(f"  {'zone':30s} {'history':>12} {'(files)':>9} {'at HEAD':>12} {'(files)':>9}  regenerable")
    for z in sorted(set(list(zh)+list(zhead)), key=lambda z:-zh.get(z,0)):
        rh = zh.get(z,0); rhn=zh_n.get(z,0); rhd=zhead.get(z,0); rhdn=zhead_n.get(z,0)
        print(f"  {z:30s} {human(rh):>12} {rhn:>9} {human(rhd):>12} {rhdn:>9}  {regen.get(z,'?')}")

    total_hist = sum(zh.values()); total_head=sum(sz for sz,_ in head_files)
    print(f"\n[totals] history unique-path bytes (largest ver) ≈ {human(total_hist)} | HEAD checkout ≈ {human(total_head)}")

    # proof-of-preservation summary
    print("\n[proof-of-preservation — regenerable zones]")
    for pat,name,regen,why in ZONES:
        if regen is True:
            n=zh_n.get(name,0)+zhead_n.get(name,0)
            print(f"  {name:30s} {n:4d} files  -> {why}")

if __name__=="__main__":
    main()
