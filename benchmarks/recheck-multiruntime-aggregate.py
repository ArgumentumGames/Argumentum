#!/usr/bin/env python3
"""Aggregation du controle multilangage .NET 9 / 10 / 11 (banc Argumentum).

Produit, a partir des rapports BenchmarkDotNet ecrits par
``benchmarks/recheck-multiruntime.ps1`` :

  - le CONTROLE D'ATTESTATION : la ligne ``[Host]`` de chaque jambe doit
    nommer le runtime attendu (une jambe non attestee invalide la matrice) ;
  - les MOYENNES PAR POSITION, qui mesurent le biais de passage ;
  - les statistiques par runtime (moyenne, mediane, min, max) ;
  - le TEST DE SEPARABILITE : deux groupes ne sont distinguables que si
    ``max(A) < min(B)``. Deux groupes qui se recouvrent ne sont PAS separes --
    c'est une absence de separation, pas une preuve d'egalite.

Lecture des nombres : BenchmarkDotNet ecrit au format **invariant**
(``1,337.3`` = virgule de milliers, point decimal). Un parseur au format
francais divise silencieusement par 1000.

Utilisation :
    python benchmarks/recheck-multiruntime-aggregate.py <dossier de sortie>
"""
import json
import os
import re
import sys
from collections import defaultdict

# Version majeure attendue de chaque runtime, pour le controle d'attestation.
EXPECTED_MAJOR = {"net9": "9.", "net10": "10.", "net11": "11."}

# Ordre canonique des axes.
AXES = ("timestamping", "load50", "load500")


def parse_bdn_number(text):
    """Parse un nombre au format invariant de BenchmarkDotNet.

    ``1,337.3`` -> 1337.3 ; ``57.59`` -> 57.59 ; ``162.54 KB`` -> 162.54 (en Ko).
    """
    t = text.strip().replace(" ", "").replace(" ", "")
    if not t:
        return None
    m = re.match(r"^([0-9][0-9,]*(?:\.[0-9]+)?)([A-Za-zμs]*)$", t)
    if not m:
        return None
    num, unit = m.group(1), m.group(2)
    if "," in num and "." in num:
        num = num.replace(",", "")          # 1,337.3 -> 1337.3
    elif "," in num:
        head, _, tail = num.partition(",")
        # 1,337 -> virgule de milliers ; 1,5 -> virgule decimale
        num = head + tail if len(tail) == 3 else num.replace(",", ".")
    value = float(num)
    scale = {
        "ns": 1e-3, "μs": 1.0, "us": 1.0, "ms": 1e3, "s": 1e6,
        "B": 1.0 / 1024, "KB": 1.0, "MB": 1024.0, "GB": 1024.0 * 1024,
    }
    return value * scale.get(unit, 1.0)


def classify(method):
    """Nom court d'un benchmark, a partir de la cellule 'Method'."""
    m = re.sub(r"<[^>]+>", "", method).replace("&#39;", "'")
    if "Timestamping" in m:
        return "timestamping"
    if "500" in m:
        return "load500"
    if "50" in m:
        return "load50"
    return None


def parse_report(path):
    """Extrait le runtime attesté et les moyennes par benchmark d'un rapport BDN."""
    host = None
    values = {}
    allocated = {}
    alloc_index = None

    with open(path, "r", encoding="utf-8", errors="replace") as handle:
        for line in handle:
            if "[Host]" in line and host is None:
                m = re.search(r"\[Host\]\s*:\s*\.NET\s+([0-9.]+)", line)
                if m:
                    host = m.group(1)
            if not line.startswith("|"):
                continue
            cells = [c.strip() for c in line.strip().strip("|").split("|")]
            if alloc_index is None:
                header = [re.sub(r"<[^>]+>", "", c).strip() for c in cells]
                # BDN ajoute une colonne 'Median' sur certaines jambes : l'index
                # de 'Allocated' se calcule sur l'en-tete, jamais en dur.
                if "Allocated" in header:
                    alloc_index = header.index("Allocated")
                continue
            key = classify(cells[0])
            if key is None or len(cells) < 2:
                continue
            value = parse_bdn_number(cells[1])
            if value is not None:
                values[key] = value
            if alloc_index is not None and len(cells) > alloc_index:
                allocated[key] = cells[alloc_index]

    return host, values, allocated


def collect(root):
    legs = []
    for tag in sorted(os.listdir(root)):
        leg_dir = os.path.join(root, tag)
        if not os.path.isdir(leg_dir) or tag == "warmup":
            continue
        report = None
        for dirpath, _, files in os.walk(leg_dir):
            for name in files:
                if name.endswith("-report-github.md"):
                    report = os.path.join(dirpath, name)
        if not report:
            continue
        host, values, allocated = parse_report(report)
        if not values:
            continue
        m = re.match(r"rep(\d+)-(net\d+)-p(\d+)", tag)
        legs.append({
            "tag": tag,
            "runtime": m.group(2) if m else None,
            "position": int(m.group(3)) if m else None,
            "host": host,
            "values": values,
            "allocated": allocated,
        })
    return legs


def main():
    root = sys.argv[1] if len(sys.argv) > 1 else "."
    legs = collect(root)
    if not legs:
        print("Aucune jambe trouvee sous %s" % root)
        return 1

    print("=== ATTESTATION (ligne [Host] de chaque jambe) ===")
    bad = 0
    for leg in legs:
        expected = EXPECTED_MAJOR.get(leg["runtime"], "")
        if not leg["host"] or not leg["host"].startswith(expected):
            bad += 1
            print("  MISMATCH %s : attendu %s, [Host]=%s"
                  % (leg["tag"], leg["runtime"], leg["host"]))
    print("  %d jambe(s) mesuree(s), %d ecart(s) d'attestation" % (len(legs), bad))
    if bad:
        print("  ATTENTION : une jambe non attestee invalide la comparaison.")

    for axis in AXES:
        subset = [l for l in legs if axis in l["values"]]
        if not subset:
            continue
        print("\n=== AXE %s ===" % axis)
        by_runtime = defaultdict(list)
        for leg in subset:
            by_runtime[leg["runtime"]].append(leg["values"][axis])
        runtimes = sorted(by_runtime)

        print("  moyennes par position (biais de passage) :")
        print("    runtime  " + "  ".join("p%d" % p for p in (1, 2, 3)))
        for rt in runtimes:
            cells = []
            for p in (1, 2, 3):
                vals = [l["values"][axis] for l in subset
                        if l["runtime"] == rt and l["position"] == p]
                cells.append("%8.1f" % (sum(vals) / len(vals)) if vals else "     n/a")
            print("    %-7s " % rt + "  ".join(cells))
        for rt in runtimes:
            positions = sorted(l["position"] for l in subset if l["runtime"] == rt)
            print("    %-7s positions rencontrees : %s" % (rt, positions))

        print("  statistiques :")
        for rt in runtimes:
            vals = sorted(by_runtime[rt])
            print("    %-7s n=%d min=%8.1f moy=%8.1f max=%8.1f"
                  % (rt, len(vals), vals[0], sum(vals) / len(vals), vals[-1]))

        print("  separabilite (max d'un groupe < min de l'autre) :")
        for i in range(len(runtimes)):
            for j in range(i + 1, len(runtimes)):
                a, b = runtimes[i], runtimes[j]
                va, vb = by_runtime[a], by_runtime[b]
                if max(va) < min(vb):
                    verdict = "SEPARES : %s < %s" % (a, b)
                elif max(vb) < min(va):
                    verdict = "SEPARES : %s < %s" % (b, a)
                else:
                    verdict = "CHEVAUCHEMENT (absence de separation)"
                print("    %-6s vs %-6s : %-45s [%s %.1f-%.1f | %s %.1f-%.1f]"
                      % (a, b, verdict, a, min(va), max(va), b, min(vb), max(vb)))

    allocations = sorted({l["allocated"].get("load50") for l in legs if l.get("allocated")})
    print("\n=== ALLOCATIONS (load50) ===")
    print("  valeurs distinctes : %s" % allocations)

    out = os.path.join(root, "aggregation.json")
    with open(out, "w", encoding="utf-8") as handle:
        json.dump(legs, handle, indent=2, ensure_ascii=False)
    print("\n(jambes brutes : %s)" % out)
    return 0


if __name__ == "__main__":
    sys.exit(main())
