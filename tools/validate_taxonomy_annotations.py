#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
validate_taxonomy_annotations.py — reusable anti-fabrication validator for the
Argumentum taxonomy scale-up propositions.

Dispatched by ai-01 (msg-20260617T051231-o6izvl) to po-2024. Encodes the manual
anti-fab passes done per phase (#503/#505/#508 pilots, #509/#510 phase-1) so each
new scale-up CSV can be validated automatically before its PR.

WHY A SCRIPT (not a C# test)
  The proposition CSVs in docs/taxonomy/ have HETEROGENEOUS schemas (virtues,
  AIF-triple, cross-links) that are NOT C# entities, and the generation/anti-fab
  tooling for this lane is Python. A stdlib-only script needs no runtime the repo
  does not already imply, and accepts arbitrary CSV paths so a worker can validate
  a phase file on its own branch before opening the PR.

WHAT IT CHECKS (per detected schema kind)
  Ground truth (PK membership) is HARD-fail; semantics we cannot auto-verify
  canonically (Walton scheme spelling, name-field drift) are WARNINGs that surface
  for human eyes. This is deliberate anti-fab discipline: we do NOT fabricate a
  canonical "Walton 24" list (none exists machine-readably in-repo) — we detect
  DRIFT against the schemes already used in the validated pilots.

  (a) PK membership — every *_pk / opposed PK / family_pk / source+target pk is a
      real PK in the corresponding ground-truth taxonomy CSV; family PKs are depth 1.
  (b) link_type / crossLink_* in the 8 known cross-link types.
  (c) Walton scheme (RA_scheme / walton_scheme / AIF_skosDirectRef) — WARNING if not
      in the union of schemes used by merged proposition CSVs (drift detection).
  (d) attack_type in {undermine, undercut, rebut}.
  (e) attacked_component coherent with attack_type (WARNING — data-grounded map).
  (f) symmetric cross-links: the reverse edge exists with the same link_type.

USAGE
  python tools/validate_taxonomy_annotations.py [PROP_CSV ...] [--taxonomy-root DIR]
  No args  -> validate every docs/taxonomy/*-annotations.csv + *-crosslinks.csv found.
  Exit code: 0 if no HARD violations, 1 otherwise. Warnings never fail the run.

DEPENDENCIES: Python 3 stdlib only (csv, argparse, os, sys, collections).
"""

import argparse
import csv
import os
import sys
from collections import defaultdict

# ─────────────────────────────────────────────────────────────────────────────
# Constants — grounded in the corpus / validated pilots, never from memory.
# ─────────────────────────────────────────────────────────────────────────────

CROSSLINK_TYPES = {
    "crossLink_PredatesOn", "crossLink_Denounces", "crossLink_Leverages",
    "crossLink_Allows", "crossLink_Opposes", "crossLink_Inverts",
    "crossLink_Mirrors", "crossLink_IsRelatedTo",
}

ATTACK_TYPES = {"undermine", "undercut", "rebut"}

# attacked_component <-> attack_type coherence (data-grounded from #509 phase-1 +
# AIF standard semantics). A pair not in this map is flagged for review.
ATTACK_COMPONENT_OK = {
    ("undermine", "premise"),
    ("undercut", "inference_rule"),
    ("rebut", "conclusion"),
}

TRUTHY = {"true", "1", "yes", "y", "oui", "vrai"}


# ─────────────────────────────────────────────────────────────────────────────
# Ground-truth loading
# ─────────────────────────────────────────────────────────────────────────────

def _repo_root(start):
    """Walk up from `start` until a `Cards/Fallacies/` dir exists (the corpus)."""
    cur = os.path.abspath(start)
    for _ in range(8):
        if os.path.isdir(os.path.join(cur, "Cards", "Fallacies")):
            return cur
        parent = os.path.dirname(cur)
        if parent == cur:
            break
        cur = parent
    return None


def _read_csv(path):
    """RFC-4180 CSV read (handles quoted multilingual fields + BOM)."""
    with open(path, encoding="utf-8-sig", newline="") as f:
        return list(csv.DictReader(f))


def load_ground_truth(repo_root):
    """Load Fallacies + Virtues PK maps from the real corpus CSVs."""
    fall_path = os.path.join(repo_root, "Cards", "Fallacies",
                             "Argumentum Fallacies - Taxonomy.csv")
    virt_path = os.path.join(repo_root, "Cards", "Fallacies",
                             "Argumentum Virtues - Taxonomy.csv")
    if not os.path.isfile(fall_path):
        raise FileNotFoundError(f"Fallacies corpus not found: {fall_path}")
    if not os.path.isfile(virt_path):
        raise FileNotFoundError(f"Virtues corpus not found: {virt_path}")

    fallacies = {}  # pk -> {"text_fr", "famille", "depth", "path"}
    for row in _read_csv(fall_path):
        pk = (row.get("PK") or "").strip()
        if not pk:
            continue
        fallacies[pk] = {
            "text_fr": (row.get("text_fr") or "").strip(),
            "famille": (row.get("Famille") or "").strip(),
            "depth": (row.get("depth") or "").strip(),
            "path": (row.get("path") or "").strip(),
        }

    virtues = {}  # pk -> {"title_fr", "family_fr", "depth"}
    for row in _read_csv(virt_path):
        pk = (row.get("pk") or "").strip()
        if not pk:
            continue
        virtues[pk] = {
            "title_fr": (row.get("title_fr") or "").strip(),
            "family_fr": (row.get("family_fr") or "").strip(),
            "depth": (row.get("depth") or "").strip(),
        }

    return {"fallacies": fallacies, "virtues": virtues}


def split_pks(cell):
    """Split a ';'- or ','-separated PK cell into stripped non-empty PKs.
    Opposed-PK cells in the propositions use ';' as separator (e.g. '153;3')."""
    if not cell:
        return []
    out = []
    for tok in str(cell).replace(",", ";").split(";"):
        tok = tok.strip()
        if tok:
            out.append(tok)
    return out


# ─────────────────────────────────────────────────────────────────────────────
# Schema detection
# ─────────────────────────────────────────────────────────────────────────────

def detect_kind(header):
    h = set(header)
    if {"source_pk", "target_pk", "symmetric"} <= h:
        return "crosslinks"
    if {"virtue_pk", "prevented_family_pk"} <= h:
        return "virtues"
    if {"fallacy_pk", "attack_type"} <= h:
        return "aif-scaleup"           # triple AIF (#509): RA + attack_type + CA
    if {"fallacy_pk", "walton_scheme"} <= h:
        return "aif-pilot"             # legacy generative pilot (#505)
    return "unknown"


# ─────────────────────────────────────────────────────────────────────────────
# Per-kind validators
# ─────────────────────────────────────────────────────────────────────────────

def validate_virtues(rows, gt, walton_catalog):
    """#503 pilot + #510 phase-1: virtue -> prevented family + opposed fallacies."""
    V, F = gt["virtues"], gt["fallacies"]
    hard, warn = [], []
    for i, r in enumerate(rows, 2):  # 2 = first data row (1-based, header=1)
        vpk = (r.get("virtue_pk") or "").strip()
        fam = (r.get("prevented_family_pk") or "").strip()
        ltype = (r.get("link_type") or "").strip()
        scheme = (r.get("AIF_skosDirectRef") or "").strip()

        # (a) PK membership — HARD
        if vpk and vpk not in V:
            hard.append(f"L{i}: virtue_pk {vpk} not in Virtues corpus")
        if fam:
            if fam not in F:
                hard.append(f"L{i}: prevented_family_pk {fam} not in Fallacies corpus")
            elif F[fam]["depth"] != "1":
                hard.append(f"L{i}: prevented_family_pk {fam} depth={F[fam]['depth']} (expected family depth 1)")
        for pk in split_pks(r.get("crossLink_Opposes")):
            if pk not in F:
                hard.append(f"L{i}: opposed fallacy PK {pk} not in Fallacies corpus")

        # (b) link_type — HARD
        if ltype and ltype not in CROSSLINK_TYPES:
            hard.append(f"L{i}: link_type '{ltype}' not one of the 8 cross-link types")

        # (c) Walton scheme — WARN (drift vs validated pilots)
        if scheme and walton_catalog and scheme not in walton_catalog:
            warn.append(f"L{i}: AIF_skosDirectRef '{scheme}' is a NOVEL scheme "
                        f"(not in validated pilots) — verify spelling")

        # name drift — WARN
        if vpk in V and (r.get("virtue_title") or "").strip():
            if (r["virtue_title"]).strip() != V[vpk]["title_fr"]:
                warn.append(f"L{i}: virtue_title '{r['virtue_title'].strip()}' "
                            f"!= corpus title_fr '{V[vpk]['title_fr']}'")
        if fam in F and (r.get("prevented_family_name") or "").strip():
            fn = r["prevented_family_name"].strip()
            if fn not in (F[fam]["text_fr"], F[fam]["famille"]):
                warn.append(f"L{i}: prevented_family_name '{fn}' != corpus "
                            f"('{F[fam]['text_fr']}' / Famille '{F[fam]['famille']}')")
    return hard, warn


def validate_aif_scaleup(rows, gt, walton_catalog):
    """#509 phase-1 triple AIF: RA scheme + attack_type + attacked_component + CA."""
    F = gt["fallacies"]
    hard, warn = [], []
    for i, r in enumerate(rows, 2):
        fpk = (r.get("fallacy_pk") or "").strip()
        at = (r.get("attack_type") or "").strip()
        comp = (r.get("attacked_component") or "").strip()
        scheme = (r.get("RA_scheme") or "").strip()

        # (a) PK membership — HARD
        if fpk and fpk not in F:
            hard.append(f"L{i}: fallacy_pk {fpk} not in Fallacies corpus")

        # (d) attack_type — HARD
        if at and at not in ATTACK_TYPES:
            hard.append(f"L{i}: attack_type '{at}' not in {{undermine, undercut, rebut}}")

        # (e) coherence — WARN (data-grounded map; flag novel pairs for review)
        if at and comp and (at, comp) not in ATTACK_COMPONENT_OK:
            warn.append(f"L{i}: attack_type '{at}' + attacked_component '{comp}' "
                        f"is a NOVEL pair — verify AIF coherence")

        # (c) Walton scheme — WARN
        if scheme and walton_catalog and scheme not in walton_catalog:
            warn.append(f"L{i}: RA_scheme '{scheme}' NOVEL — verify spelling")

        # name drift — WARN
        if fpk in F and (r.get("fallacy_name") or "").strip():
            if r["fallacy_name"].strip() != F[fpk]["text_fr"]:
                warn.append(f"L{i}: fallacy_name '{r['fallacy_name'].strip()}' "
                            f"!= corpus text_fr '{F[fpk]['text_fr']}'")
    return hard, warn


def validate_aif_pilot(rows, gt, walton_catalog):
    """#505 legacy generative pilot: fallacy_pk + walton_scheme."""
    F = gt["fallacies"]
    hard, warn = [], []
    for i, r in enumerate(rows, 2):
        fpk = (r.get("fallacy_pk") or "").strip()
        scheme = (r.get("walton_scheme") or "").strip()
        if fpk and fpk not in F:
            hard.append(f"L{i}: fallacy_pk {fpk} not in Fallacies corpus")
        if scheme and walton_catalog and scheme not in walton_catalog:
            warn.append(f"L{i}: walton_scheme '{scheme}' NOVEL — verify spelling")
        if fpk in F and (r.get("fallacy_name") or "").strip():
            if r["fallacy_name"].strip() != F[fpk]["text_fr"]:
                warn.append(f"L{i}: fallacy_name drift vs corpus")
    return hard, warn


def validate_crosslinks(rows, gt, walton_catalog):
    """#508 cross-links pilot: source->target with link_type + symmetry."""
    F = gt["fallacies"]
    hard, warn = [], []
    # Build edge set: {(source_pk, link_type, target_pk)} for reciprocity check (f)
    edges = set()
    for r in rows:
        s = (r.get("source_pk") or "").strip()
        t = (r.get("target_pk") or "").strip()
        lt = (r.get("link_type") or "").strip()
        if s and t and lt:
            edges.add((s, lt, t))

    for i, r in enumerate(rows, 2):
        s = (r.get("source_pk") or "").strip()
        t = (r.get("target_pk") or "").strip()
        lt = (r.get("link_type") or "").strip()
        sym = (r.get("symmetric") or "").strip().lower() in TRUTHY

        # (a) PK membership — HARD
        if s and s not in F:
            hard.append(f"L{i}: source_pk {s} not in Fallacies corpus")
        if t and t not in F:
            hard.append(f"L{i}: target_pk {t} not in Fallacies corpus")

        # (b) link_type — HARD
        if lt and lt not in CROSSLINK_TYPES:
            hard.append(f"L{i}: link_type '{lt}' not one of the 8 cross-link types")

        # (f) symmetry reciprocity — HARD. The row's symmetric flag is the source
        # of truth: when True, the reverse edge (target, link_type, source) MUST be
        # encoded in the same CSV (validated pilots encode both directions).
        if sym and s and t and lt:
            if (t, lt, s) not in edges:
                hard.append(f"L{i}: symmetric link ({s}->{t}, {lt}) has NO reverse "
                            f"edge ({t}->{s}, {lt})")
    return hard, warn


VALIDATORS = {
    "virtues": validate_virtues,
    "aif-scaleup": validate_aif_scaleup,
    "aif-pilot": validate_aif_pilot,
    "crosslinks": validate_crosslinks,
}


# ─────────────────────────────────────────────────────────────────────────────
# Walton catalog (union of schemes used in merged proposition CSVs)
# ─────────────────────────────────────────────────────────────────────────────

def build_walton_catalog(taxonomy_dir):
    """Collect scheme names already used in merged docs/taxonomy/ propositions.
    This is the only authoritative in-repo source for scheme SPELLING (the
    generation prompts do not enumerate the canonical Walton list). Used as a
    drift detector — novel schemes are WARNINGs, not hard fails."""
    scheme_cols = ("AIF_skosDirectRef", "RA_scheme", "walton_scheme")
    catalog = set()
    if not os.path.isdir(taxonomy_dir):
        return catalog
    for name in sorted(os.listdir(taxonomy_dir)):
        if not (name.endswith("-annotations.csv") or name.endswith("-crosslinks.csv")):
            continue
        path = os.path.join(taxonomy_dir, name)
        try:
            rows = _read_csv(path)
        except Exception:
            continue
        for r in rows:
            for col in scheme_cols:
                v = (r.get(col) or "").strip()
                if v and " " in v and v[0].isupper():  # scheme names read "Argument from X"
                    catalog.add(v)
    return catalog


# ─────────────────────────────────────────────────────────────────────────────
# Driver
# ─────────────────────────────────────────────────────────────────────────────

def validate_one(path, gt, walton_catalog):
    rows = _read_csv(path)
    if not rows:
        return "unknown", [], [f"empty CSV (no data rows)"]
    kind = detect_kind(rows[0].keys())
    validator = VALIDATORS.get(kind)
    if validator is None:
        return kind, [], [f"unrecognized schema — cannot validate (cols: "
                          f"{', '.join(list(rows[0].keys())[:6])}...)"]
    hard, warn = validator(rows, gt, walton_catalog)
    return kind, hard, warn


def main(argv=None):
    ap = argparse.ArgumentParser(description="Anti-fab validator for taxonomy scale-up CSVs.")
    ap.add_argument("csvs", nargs="*", help="proposition CSVs to validate (default: all in docs/taxonomy/)")
    ap.add_argument("--taxonomy-root", default=None,
                    help="repo root containing Cards/Fallacies/ (default: auto-detect)")
    ap.add_argument("--quiet", action="store_true", help="suppress per-file detail on success")
    args = ap.parse_args(argv)

    # Resolve repo root + default CSV set
    start = args.csvs[0] if args.csvs else os.getcwd()
    repo_root = args.taxonomy_root or _repo_root(start) or _repo_root(os.path.dirname(__file__))
    if not repo_root or not os.path.isdir(os.path.join(repo_root, "Cards", "Fallacies")):
        print("ERROR: could not locate repo root (Cards/Fallacies/). "
              "Pass --taxonomy-root.", file=sys.stderr)
        return 2

    taxonomy_dir = os.path.join(repo_root, "docs", "taxonomy")
    csvs = args.csvs or sorted(
        os.path.join(taxonomy_dir, n) for n in os.listdir(taxonomy_dir)
        if n.endswith("-annotations.csv") or n.endswith("-crosslinks.csv")
    ) if os.path.isdir(taxonomy_dir) else []

    if not csvs:
        print("No proposition CSVs found to validate.", file=sys.stderr)
        return 2

    print(f"Repo root : {repo_root}")
    print(f"Validating {len(csvs)} file(s)\n")

    gt = load_ground_truth(repo_root)
    walton_catalog = build_walton_catalog(taxonomy_dir)
    print(f"Ground truth: {len(gt['fallacies'])} Fallacies PKs, "
          f"{len(gt['virtues'])} Virtues PKs, "
          f"{len(walton_catalog)} known Walton schemes (from validated pilots)\n")

    total_hard, total_warn, total_rows = 0, 0, 0
    for path in csvs:
        try:
            kind, hard, warn = validate_one(path, gt, walton_catalog)
        except Exception as e:
            print(f"  ✗ {os.path.basename(path)}: READ ERROR — {e}")
            total_hard += 1
            continue
        nrows = len(_read_csv(path))
        total_rows += nrows
        total_hard += len(hard)
        total_warn += len(warn)
        status = "✓ CLEAN" if not hard else "✗ VIOLATIONS"
        detail = "" if (args.quiet and not hard) else f" [{nrows} rows, kind={kind}]"
        print(f"  {status}  {os.path.basename(path)}{detail}")
        for m in hard:
            print(f"      HARD  {m}")
        for m in warn:
            print(f"      warn  {m}")

    print(f"\nTotals: {total_rows} rows, {total_hard} HARD violation(s), "
          f"{total_warn} warning(s)")
    return 0 if total_hard == 0 else 1


if __name__ == "__main__":
    sys.exit(main())
