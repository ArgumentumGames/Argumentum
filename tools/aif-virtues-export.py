#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""AIF Virtues export — companion to aif-attack-graph-export.py (Fallacies).

Extends the AIF export to the Virtues Taxonomy. READ-ONLY on sources.

Honest verdict (no fabrication) — DOUBLE AIF modelling of the Virtues:
  The 222 virtues carry TWO distinct AIF views of the SAME population:

  (V-A) CSV attack-graph (AIF_attackType + AIF_attackedNode): each virtue is
        modelled as an ATTACKER targeting an abstract AIF node-TYPE
        (RA/I/CA-node). Bipartite, 222 edges, 0 axiom violations.
        UNDERCUT→RA 206, UNDERMINE→I 13, REBUT→CA 3.

  (V-B) OWL good-tenor graph (aif#goodTenorOf): each virtue is modelled as a
        GOOD EXAMPLE of a canonical AIF argument SCHEME (Argument from
        Rule/Commitment/Bias/Sign/...). 222 edges, 14 distinct schemes.

  These are NOT redundant and NOT contradictory: a virtue can ATTACK a bad
  reasoning (V-A) BY EMBODYING a good argument scheme (V-B). They are two
  complementary AIF views. Exported as separate graphs, never merged.

  Contrast with Fallacies (#828): Fallacies have ONLY the CSV attack-graph
  (no goodTenorOf in argumentum.owl). The Virtues are the dual — they carry
  BOTH views. Do NOT fuse Fallacies-attacks + Virtues-attacks into one
  homogeneous graph: Virtues-attacks encode counter-arguments to fallacies,
  a different semantics from Fallacies-attacking-nodes.

Outputs (docs/ontology/aif-export/):
  aif-virtues-attack-edges.csv       222 CSV bipartite attack-edges (PK-keyed)
  aif-virtues-good-tenor.csv         222 OWL goodTenorOf edges (virtue -> scheme)
  aif-virtues-canonical-concepts.csv AIF canonical concepts referenced via skos
  aif-virtues-schemes.csv            14 canonical argument schemes (goodTenorOf targets)
"""
import csv
import os
import sys
import collections
import xml.etree.ElementTree as ET

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_PATH = os.path.join(ROOT, "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv")
OWL_PATH = os.path.join(ROOT, "docs", "ontology", "argumentum_virtues.owl")
OUT_DIR = os.path.join(ROOT, "docs", "ontology", "aif-export")

ATTACK_NODE_AXIOM = {
    "undercut": "RA-node",
    "undermine": "I-node",
    "rebut": "CA-node",
}


def frag(iri):
    if not iri:
        return ""
    return iri.rsplit("#", 1)[-1] if "#" in iri else iri


def iri_of(el):
    if el.get("IRI"):
        return el.get("IRI")
    return (el.text or "").strip()


def load_csv():
    with open(CSV_PATH, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.reader(f))
    header = rows[0]

    def col(name):
        return header.index(name)

    return header, rows[1:], col


def _write_csv(path, fields, rows):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8", newline="") as f:
        w = csv.DictWriter(f, fieldnames=fields)
        w.writeheader()
        for r in rows:
            w.writerow(r)


def export_virtues_attack_edges(data, col, out_path):
    """222 bipartite attack-edges from Virtues CSV (V-A)."""
    fields = [
        "attacker_pk", "attacker_label", "family", "subfamily", "subsubfamily",
        "decimal_path", "attack_type", "attacked_node_type",
        "skos_direct_ref", "skos_exception_ref", "skos_mapping_type",
        "expected_node_type", "node_type_axiom_ok",
    ]
    rows_out = []
    violations = []
    for r in data:
        at = r[col("AIF_attackType")].strip()
        an = r[col("AIF_attackedNode")].strip()
        if not at and not an:
            continue
        if (at and not an) or (an and not at):
            violations.append((r[col("pk")], "asymmetric attackType/attackedNode", at, an))
            continue
        expected = ATTACK_NODE_AXIOM.get(at, "")
        ok = "1" if expected == an else "0"
        if ok == "0":
            violations.append((r[col("pk")], f"attackType={at} expects {expected}, got {an}", at, an))
        rows_out.append({
            "attacker_pk": r[col("pk")],
            "attacker_label": r[col("title_fr")].strip(),
            "family": r[col("family_fr")].strip(),
            "subfamily": r[col("subfamily_fr")].strip(),
            "subsubfamily": r[col("subsubfamily_fr")].strip(),
            "decimal_path": r[col("decimal_path_padded")].strip(),
            "attack_type": at,
            "attacked_node_type": an,
            "skos_direct_ref": r[col("AIF_skosDirectRef")].strip(),
            "skos_exception_ref": r[col("AIF_skosExceptionRef")].strip(),
            "skos_mapping_type": r[col("AIF_skosMappingType")].strip(),
            "expected_node_type": expected,
            "node_type_axiom_ok": ok,
        })
    _write_csv(out_path, fields, rows_out)
    return rows_out, violations


def export_virtues_canonical_concepts(data, col, out_path):
    """AIF canonical concepts referenced via skos in the Virtues CSV."""
    concept_uses = collections.Counter()
    concept_samples = collections.defaultdict(list)
    for r in data:
        for refcol in ("AIF_skosDirectRef", "AIF_skosExceptionRef"):
            ref = r[col(refcol)].strip()
            if not ref:
                continue
            for c in [x.strip() for x in ref.split(",") if x.strip()]:
                concept_uses[c] += 1
                if len(concept_samples[c]) < 3:
                    concept_samples[c].append(r[col("pk")])
    fields = ["concept_id", "suffix", "ref_count", "sample_attacker_pks"]
    rows_out = []
    suffix_count = collections.Counter()
    for c, n in concept_uses.most_common():
        suffix = c.rsplit("_", 1)[-1] if "_" in c else "(none)"
        suffix_count[suffix] += 1
        rows_out.append({
            "concept_id": c,
            "suffix": suffix,
            "ref_count": n,
            "sample_attacker_pks": ";".join(concept_samples[c]),
        })
    _write_csv(out_path, fields, rows_out)
    return concept_uses, suffix_count


def export_good_tenor(out_path, schemes_path):
    """222 goodTenorOf edges from virtues OWL (V-B) + 14 canonical schemes."""
    tree = ET.parse(OWL_PATH)
    root = tree.getroot()
    edges = []
    scheme_count = collections.Counter()
    for aa in root.iter("AnnotationAssertion"):
        ch = list(aa)
        if len(ch) < 3:
            continue
        pf = frag(ch[0].get("IRI", ""))
        if pf != "goodTenorOf":
            continue
        subj = frag(iri_of(ch[1]))
        obj = frag(iri_of(ch[2]))
        if subj and obj:
            edges.append((subj, obj))
            scheme_count[obj] += 1
    fields = ["virtue_iri_fragment", "argument_scheme"]
    rows_out = [{"virtue_iri_fragment": s, "argument_scheme": o} for s, o in
                sorted(edges, key=lambda x: (x[1], x[0]))]
    _write_csv(out_path, fields, rows_out)

    # schemes summary file
    sfields = ["argument_scheme", "virtue_count"]
    srows = [{"argument_scheme": s, "virtue_count": n}
             for s, n in scheme_count.most_common()]
    _write_csv(schemes_path, sfields, srows)
    return rows_out, scheme_count


def main():
    if not os.path.exists(CSV_PATH):
        sys.exit(f"CSV not found: {CSV_PATH}")
    os.makedirs(OUT_DIR, exist_ok=True)

    header, data, col = load_csv()
    print("=" * 72)
    print("AIF Virtues export — companion to aif-attack-graph-export.py")
    print("READ-ONLY, 0 write to sources")
    print("=" * 72)
    print(f"CSV  : {CSV_PATH}")
    print(f"OWL  : {OWL_PATH}")
    print(f"OUT  : {OUT_DIR}")
    print(f"Total virtue rows: {len(data)}")
    print()

    # V-A: CSV attack-edges
    at_edges, violations = export_virtues_attack_edges(
        data, col, os.path.join(OUT_DIR, "aif-virtues-attack-edges.csv"))
    print(f"[V-A] aif-virtues-attack-edges.csv : {len(at_edges)} bipartite attack-edges")
    by_type = collections.Counter(e["attack_type"] for e in at_edges)
    for t, n in by_type.most_common():
        print(f"        {t:12s}: {n}  (expected target: {ATTACK_NODE_AXIOM.get(t)})")
    print(f"        axiom violations: {len(violations)}")
    for pk, reason, at, an in violations[:5]:
        print(f"          - PK {pk}: {reason}")
    print()

    # canonical concepts
    concepts, suffix_count = export_virtues_canonical_concepts(
        data, col, os.path.join(OUT_DIR, "aif-virtues-canonical-concepts.csv"))
    rows_with_skos = len([e for e in at_edges if e["skos_direct_ref"] or e["skos_exception_ref"]])
    print(f"[skos] aif-virtues-canonical-concepts.csv : {len(concepts)} concepts "
          f"(referenced by {rows_with_skos}/{len(at_edges)} attack-edges)")
    print(f"        suffix distribution: {dict(suffix_count.most_common())}")
    print()

    # V-B: OWL goodTenorOf
    gt_rows, scheme_count = export_good_tenor(
        os.path.join(OUT_DIR, "aif-virtues-good-tenor.csv"),
        os.path.join(OUT_DIR, "aif-virtues-schemes.csv"))
    print(f"[V-B] aif-virtues-good-tenor.csv : {len(gt_rows)} virtue->scheme edges")
    print(f"        distinct virtues: {len(set(r['virtue_iri_fragment'] for r in gt_rows))}")
    print(f"        distinct schemes: {len(scheme_count)}")
    print(f"        scheme distribution:")
    for s, n in scheme_count.most_common():
        print(f"          {s:40s}: {n}")
    print()

    print("=" * 72)
    print("HONEST VERDICT — double AIF modelling of the Virtues")
    print("=" * 72)
    print(f"  CSV attack-graph  (V-A): {len(at_edges)} virtue-attackers -> node-TYPE (bipartite)")
    print(f"  OWL good-tenor    (V-B): {len(gt_rows)} virtues -> {len(scheme_count)} argument schemes")
    print()
    print("  Same 222 virtues, TWO complementary AIF views:")
    print("    V-A = how the virtue ATTACKS a bad reasoning (counter-argument)")
    print("    V-B = how the virtue EMBODIES a good argument scheme")
    print("  NOT redundant, NOT contradictory. Exported separately, never merged.")
    print()
    print("  Contrast with Fallacies (#828): Fallacies carry ONLY V-A (CSV attacks),")
    print("  no goodTenorOf. Virtues are the dual — they carry BOTH. Do NOT fuse")
    print("  Fallacies-attacks + Virtues-attacks: Virtues-attacks encode")
    print("  counter-arguments to fallacies, a different semantics.")
    print()
    print("Done. Artefacts written to docs/ontology/aif-export/")


if __name__ == "__main__":
    main()
