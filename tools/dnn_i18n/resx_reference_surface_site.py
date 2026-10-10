#!/usr/bin/env python3
"""T3 dossier replay instrument — Argumentum-side reference surface of the 6 variant families.

Replays the Argumentum row of §2 of
docs/dnn-localization/457-t3-globalresources-arbitrage-dossier-2026-10-10.md (PR #1852):
which resx keys of the 6 variant families does the Argumentum-visible surface actually
reference? Published figure: 1 key (SharedResources 'Home.Text') over 538 files.

INSTRUMENT DEFINITION (this is what the published figures mean — the count is
instrument-dependent, see the dossier §7 addendum):
  scope      : DNNPlatform/Portals/1 + DNNPlatform/Portals/_default/Skins, recursive
  extensions : .cs .vb .ascx .aspx .cshtml .vbhtml .js .xml .config .master .htm .html
               (12 — a 6-extension variant of this set counts 537: the single extra
               file is Portals/1/2sxc/web.config)
  binding    : call-literal patterns —
                 Localization.GetString("KEY" ...      (C#/VB, with or without file arg)
                 LocalizeString("KEY"                  (Razor @Dnn.LocalizeString / helpers)
                 ResourceKey="KEY" / resourcekey="KEY" (markup attributes)
                 GetResource("KEY"
  exclusions : none by directory (the whole Portals/1 scope IS the Argumentum surface);
               DNN admin internals under DesktopModules are out of scope by construction.

Witness (fail-closed): the literal 'Privacy.Text' (a binding literal of the 2shineBS5
skin's LOCAL App_LocalResources — deliberately NOT one of the 6 measured families, so
the witness stays independent of the count it certifies) must be seen somewhere in the
scan, else the script exits 2 — an instrument that cannot see the positive case has no
count to report. (Witness repaired at commit time: the scratchpad original watched
'ui.fallacy.find_out_more', a CSV key that is no file literal — it always exited 2.
The 538-files/1-key figure itself was independently corroborated by the counter-review
of 10/10 08:48Z; see the dossier §7 addendum.)

USAGE
    python tools/dnn_i18n/resx_reference_surface_site.py
Exit 0 = scanned with witness seen; 2 = witness NOT seen (instrument suspect, count void).
"""
import re
import sys
import xml.etree.ElementTree as ET
from pathlib import Path

REPO = Path(__file__).resolve().parents[2]
ROOT = REPO / "DNNPlatform"
FAMILIES = ["Exceptions", "FileUpload", "GlobalResources",
            "List_Country", "SharedResources", "WebControls"]
SCAN = ["Portals/1", "Portals/_default/Skins"]
EXTS = (".cs", ".vb", ".ascx", ".aspx", ".cshtml", ".vbhtml", ".js", ".xml",
        ".config", ".master", ".htm", ".html")

PATTERNS = [
    re.compile(r'Localization\.GetString\(\s*"([^"]+)"'),
    re.compile(r'LocalizeString\(\s*"([^"]+)"'),
    re.compile(r'[Rr]esource[Kk]ey\s*=\s*"([^"]+)"'),
    re.compile(r'[Rr]esourcekey\s*=\s*"([^"]+)"'),
    re.compile(r'GetResource\(\s*"([^"]+)"'),
]

WITNESS = "Privacy.Text"  # 2shineBS5 local App_LocalResources key — a real binding literal in the scanned .ascx,
# deliberately NOT one of the 6 measured families (independent of the count it certifies)


def resx_keys(fam):
    root = ET.parse(ROOT / "App_GlobalResources" / f"{fam}.resx").getroot()
    return {d.get("name") for d in root.findall("data")}


def main():
    allkeys = {k: fam for fam in FAMILIES for k in resx_keys(fam)}
    hits = {}
    files_scanned = 0
    witness_seen = False
    for sub in SCAN:
        for p in (ROOT / sub).rglob("*"):
            if not p.is_file() or p.suffix.lower() not in EXTS:
                continue
            files_scanned += 1
            try:
                text = p.read_text(encoding="utf-8", errors="replace")
            except OSError:
                continue
            if WITNESS in text:
                witness_seen = True
            for pat in PATTERNS:
                for m in pat.finditer(text):
                    k = m.group(1)
                    if k in allkeys:
                        hits.setdefault(k, set()).add(str(p.relative_to(ROOT)))

    print(f"files scanned: {files_scanned} under {SCAN}")
    print(f"witness: {'SEEN' if witness_seen else 'NOT SEEN (instrument suspect)'}")
    print(f"distinct resx keys referenced by the Argumentum surface: {len(hits)}")
    by_fam = {}
    for k, srcs in hits.items():
        by_fam.setdefault(allkeys[k], []).append(k)
    for fam in sorted(by_fam):
        ks = by_fam[fam]
        n = len(resx_keys(fam))
        print(f"  {fam:<18}{len(ks):>4} / {n}")
    return 0 if witness_seen else 2


if __name__ == "__main__":
    sys.exit(main())
