#!/usr/bin/env python3
"""T3 dossier replay instrument — framework-source reference surface of the 6 variant families.

Replays the DNNPlatform-source row of §2 of
docs/dnn-localization/457-t3-globalresources-arbitrage-dossier-2026-10-10.md (PR #1852):
which resx keys of the 6 variant families does the greppable DNNPlatform source reference?
Published figure: 8 keys (5 GlobalResources + 3 SharedResources) over 970 files.

INSTRUMENT DEFINITION (this is what the published '8' means — the count is
instrument-DEPENDENT because DNN binds resource files via the Localization constant,
not a literal path; see the dossier §7 addendum for the counter-review's three
alternative instruments and their counts: literal-path 3, constant-bound 9,
raw-key-name upper bound 37):
  scope      : whole DNNPlatform tree, minus /obj/, /bin/ and App_GlobalResources itself
  extensions : .cs .vb .ascx .aspx .cshtml .vbhtml .master .config
  binding    : call-literal patterns ONLY —
                 Localization.GetString("KEY"
                 LocalizeString("KEY"
                 ResourceKey = "KEY"
  known gap (declared): keys bound via Localization.(Shared|Global)ResourceFile constants
  are missed by this instrument; the addendum's cross-instrument table bounds the effect
  (every instrument agrees: < 40 of 1 241 keys, and the constant-bound references are
  back-office UI — nothing a visitor renders).

Witness (fail-closed): the literal 'Privacy.Text' must be seen somewhere in the scan,
else the script exits 2 — an instrument that cannot see the positive case has no count
to report.

USAGE
    python tools/dnn_i18n/resx_reference_surface_source.py
Exit 0 = scanned with witness seen; 2 = witness NOT seen (instrument suspect, count void).
"""
import re
import sys
import xml.etree.ElementTree as ET
from collections import defaultdict
from pathlib import Path

REPO = Path(__file__).resolve().parents[2]
ROOT = REPO / "DNNPlatform"
FAM = ["Exceptions", "FileUpload", "GlobalResources",
       "List_Country", "SharedResources", "WebControls"]
PATS = [
    re.compile(r'Localization\.GetString\(\s*"([^"]+)"'),
    re.compile(r'LocalizeString\(\s*"([^"]+)"'),
    re.compile(r'[Rr]esource[Kk]ey\s*=\s*"([^"]+)"'),
]
EXTS = (".cs", ".vb", ".ascx", ".aspx", ".cshtml", ".vbhtml",
        ".master", ".config")


def main():
    keys = {}
    sizes = {}
    for fam in FAM:
        data = ET.parse(ROOT / "App_GlobalResources" / f"{fam}.resx").getroot().findall("data")
        sizes[fam] = len(data)
        for d in data:
            keys[d.get("name")] = fam

    hits = defaultdict(set)
    scanned = witness = 0
    for p in ROOT.rglob("*"):
        s = str(p).replace("\\", "/")
        if not p.is_file() or p.suffix.lower() not in EXTS:
            continue
        if "/obj/" in s or "/bin/" in s or "App_GlobalResources" in s:
            continue
        scanned += 1
        try:
            t = p.read_text(encoding="utf-8", errors="replace")
        except OSError:
            continue
        if "Privacy.Text" in t:
            witness += 1
        for pat in PATS:
            for m in pat.finditer(t):
                k = m.group(1)
                if k in keys:
                    hits[k].add(s)

    byf = defaultdict(list)
    for k, srcs in hits.items():
        byf[keys[k]].append((k, len(srcs)))
    print(f"scanned {scanned} source files; witness 'Privacy.Text' in {witness} files")
    print(f"distinct keys of the 6 families referenced in DNNPlatform source: {len(hits)}")
    for fam in FAM:
        got = byf.get(fam, [])
        print(f"  {fam:<18}{len(got):>4} / {sizes[fam]}")
    # where do the referenced keys live (top directories)
    topdirs = defaultdict(int)
    for k, srcs in hits.items():
        for s in srcs:
            topdirs[s.split("DNNPlatform/")[1].split("/")[0]] += 1
    print("by top directory:", dict(sorted(topdirs.items(), key=lambda x: -x[1])))
    return 0 if witness else 2


if __name__ == "__main__":
    sys.exit(main())
