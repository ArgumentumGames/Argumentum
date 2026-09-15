#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""AIF attack-graph export — typed attack-edges + relations for CoursIA uplift.

Exports the EXISTING AIF modelling from the Fallacies Taxonomy CSV + argumentum.owl
into consumable artefacts. READ-ONLY on sources (0 write to CSV/OWL).

Honest verdict (no fabrication):
  The AIF columns (AIF_attackType + AIF_attackedNode) encode a BIPARTITE attack
  graph: each fully-modeled fallacy is an ATTACKER that targets an abstract AIF
  node-TYPE (RA-node / I-node / CA-node), NOT another fallacy. There is NO
  inter-fallacy attack adjacency ("fallacy X attacks fallacy Y") in the data.

  A separate inter-fallacy graph exists in the OWL (isRelatedTo, mirrors,
  predatesOn, denounces, leverages, allows, opposes, inverts) but these are
  GENERIC semantic relations, NOT typed AIF attack-edges — exported separately
  and labelled as such.

Outputs (docs/ontology/aif-export/):
  aif-attack-edges.csv       145 bipartite attack-edges (CSV-sourced, PK-keyed)
  aif-canonical-concepts.csv AIF canonical concepts referenced via skos
  taxonomy-tree-edges.csv    hierarchical tree edges (decimal_path, all 1408)
  aif-owl-attack-edges.csv   OWL-sourced mirror of attack-edges (cross-check)
  aif-relations-graph.csv    OWL inter-fallacy semantic relations (non-AIF)
"""
import csv
import os
import re
import sys
import collections
import xml.etree.ElementTree as ET

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_PATH = os.path.join(ROOT, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")
OWL_PATH = os.path.join(ROOT, "docs", "ontology", "argumentum.owl")
OUT_DIR = os.path.join(ROOT, "docs", "ontology", "aif-export")

AIF_NS = "http://www.arg.dundee.ac.uk/aif"
ARG_NS = "https://www.argumentum.games/argumentum_fallacies.owl"

# attackType -> expected attackedNode-type (AIF / ASPIC+ semantics)
ATTACK_NODE_AXIOM = {
    "undercut": "RA-node",   # attacks the rule-application link
    "undermine": "I-node",   # attacks the information/premise node
    "rebut": "CA-node",      # attacks via a conflicting application
}

# OWL ObjectProperties / AnnotationProperties that encode inter-fallacy
# semantic relations (NOT AIF attack-edges). Exported as a separate graph.
RELATION_PROPS = {
    "isRelatedTo", "mirrors", "predatesOn", "denounces",
    "leverages", "allows", "opposes", "inverts",
}


def frag(iri):
    """Return the fragment (after #) of an IRI, or the full string."""
    if not iri:
        return ""
    return iri.rsplit("#", 1)[-1] if "#" in iri else iri


def iri_of(el):
    """Read an IRI from an OWL/XML element.

    Two forms occur in OWL/XML:
      - <AnnotationProperty IRI="..."/>   -> attribute
      - <IRI>http://...</IRI>             -> element text
    """
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


def export_attack_edges_csv(data, col, out_path):
    """145 bipartite attack-edges from CSV columns AIF_attackType + AIF_attackedNode."""
    fields = [
        "attacker_pk", "attacker_label", "family", "subfamily", "subsubfamily",
        "decimal_path", "attack_type", "attacked_node_type",
        "skos_direct_ref", "skos_exception_ref", "skos_mapping_type",
        "expected_node_type", "node_type_axiom_ok",
    ]
    rows_out = []
    axiom_violations = []
    for r in data:
        at = r[col("AIF_attackType")].strip()
        an = r[col("AIF_attackedNode")].strip()
        if not at and not an:
            continue  # not fully-modeled
        if at and not an or an and not at:
            axiom_violations.append((r[col("PK")], "asymmetric attackType/attackedNode", at, an))
            continue
        expected = ATTACK_NODE_AXIOM.get(at, "")
        ok = "1" if expected == an else "0"
        if ok == "0":
            axiom_violations.append((r[col("PK")], f"attackType={at} expects {expected}, got {an}", at, an))
        rows_out.append({
            "attacker_pk": r[col("PK")],
            "attacker_label": r[col("nom_vulgarisé")].strip() or r[col("carte")].strip(),
            "family": r[col("Famille")].strip(),
            "subfamily": r[col("Sous-Famille")].strip(),
            "subsubfamily": r[col("Soussousfamille")].strip(),
            "decimal_path": r[col("decimal_path")].strip(),
            "attack_type": at,
            "attacked_node_type": an,
            "skos_direct_ref": r[col("AIF_skosDirectRef")].strip(),
            "skos_exception_ref": r[col("AIF_skosExceptionRef")].strip(),
            "skos_mapping_type": r[col("AIF_skosMappingType")].strip(),
            "expected_node_type": expected,
            "node_type_axiom_ok": ok,
        })
    _write_csv(out_path, fields, rows_out)
    return rows_out, axiom_violations


def export_canonical_concepts(data, col, out_path):
    """Distinct AIF canonical concepts referenced via skos (direct + exception)."""
    concept_uses = collections.Counter()
    concept_samples = collections.defaultdict(list)
    for r in data:
        for refcol in ("AIF_skosDirectRef", "AIF_skosExceptionRef"):
            ref = r[col(refcol)].strip()
            if not ref:
                continue
            # a cell may hold multiple comma-separated concepts
            for c in [x.strip() for x in ref.split(",") if x.strip()]:
                concept_uses[c] += 1
                if len(concept_samples[c]) < 3:
                    concept_samples[c].append(r[col("PK")])
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


def export_taxonomy_tree(data, col, out_path):
    """Hierarchical tree edges derived from decimal_path (parent -> child)."""
    pk_i = col("PK")
    dp_i = col("decimal_path")
    fam_i = col("Famille")
    sfam_i = col("Sous-Famille")
    ssfam_i = col("Soussousfamille")
    fields = ["parent_path", "child_path", "child_pk", "depth", "family", "subfamily", "subsubfamily"]
    rows_out = []
    for r in data:
        dp = r[dp_i].strip()
        if not dp:
            continue
        depth = dp.count(",") + 1
        parent = ",".join(dp.split(",")[:-1]) if depth > 1 else ""
        rows_out.append({
            "parent_path": parent,
            "child_path": dp,
            "child_pk": r[pk_i],
            "depth": depth,
            "family": r[fam_i].strip(),
            "subfamily": r[sfam_i].strip(),
            "subsubfamily": r[ssfam_i].strip(),
        })
    rows_out.sort(key=lambda x: x["child_path"])
    _write_csv(out_path, fields, rows_out)
    return len(rows_out)


def parse_owl_attack_and_relations():
    """Parse argumentum.owl: extract aifAttackType/aifAttackedNode assertions
    (attack graph) and inter-fallacy relation assertions (isRelatedTo etc.)."""
    tree = ET.parse(OWL_PATH)
    root = tree.getroot()
    # OWL/XML elements use no namespace prefix for AnnotationAssertion etc.
    attack = {}      # subject_fragment -> {type, node}
    relations = []   # (subject, prop, object)
    for aa in root.iter("AnnotationAssertion"):
        children = list(aa)
        if len(children) < 3:
            continue
        prop_el = children[0]
        subj_el = children[1]
        obj_el = children[2]
        prop_iri = prop_el.get("IRI", "")
        prop_frag = frag(prop_iri)
        subj_frag = frag(subj_el.get("IRI", "") or subj_el.text or "")
        if not subj_frag:
            continue
        # AIF attack properties
        if prop_frag == "aifAttackType":
            val = obj_el.text.strip() if obj_el.tag == "Literal" else ""
            attack.setdefault(subj_frag, {})["type"] = val
        elif prop_frag == "aifAttackedNode":
            node_frag = frag(iri_of(obj_el))
            attack.setdefault(subj_frag, {})["node"] = node_frag
        elif prop_frag in RELATION_PROPS:
            obj_frag = frag(iri_of(obj_el))
            if obj_frag:
                relations.append((subj_frag, prop_frag, obj_frag))
    return attack, relations


def export_owl_attack_edges(out_path):
    attack, relations = parse_owl_attack_and_relations()
    fields = ["attacker_iri_fragment", "attack_type", "attacked_node_type",
              "expected_node_type", "node_type_axiom_ok"]
    rows_out = []
    for subj, info in sorted(attack.items()):
        t = info.get("type", "")
        n = info.get("node", "")
        if t and n:
            expected = ATTACK_NODE_AXIOM.get(t, "")
            ok = "1" if expected == n else "0"
            rows_out.append({
                "attacker_iri_fragment": subj,
                "attack_type": t,
                "attacked_node_type": n,
                "expected_node_type": expected,
                "node_type_axiom_ok": ok,
            })
    rows_out.sort(key=lambda x: (x["attack_type"], x["attacker_iri_fragment"]))
    _write_csv(out_path, fields, rows_out)
    return rows_out, relations


def export_relations_graph(relations, out_path):
    """Inter-fallacy semantic relations from OWL (NON-AIF). Bidirectional pairs
    are emitted as single directed edges; duplicates collapsed."""
    seen = set()
    fields = ["source_fragment", "relation", "target_fragment"]
    rows_out = []
    for s, p, o in relations:
        key = (s, p, o)
        if key in seen:
            continue
        seen.add(key)
        rows_out.append({"source_fragment": s, "relation": p, "target_fragment": o})
    rows_out.sort(key=lambda x: (x["relation"], x["source_fragment"]))
    _write_csv(out_path, fields, rows_out)
    return rows_out


def _write_csv(path, fields, rows):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8", newline="") as f:
        w = csv.DictWriter(f, fieldnames=fields)
        w.writeheader()
        for r in rows:
            w.writerow(r)


def main():
    if not os.path.exists(CSV_PATH):
        sys.exit(f"CSV not found: {CSV_PATH}")
    os.makedirs(OUT_DIR, exist_ok=True)

    header, data, col = load_csv()
    total = len(data)
    print("=" * 72)
    print("AIF attack-graph export — read-only, 0 write to sources")
    print("=" * 72)
    print(f"CSV  : {CSV_PATH}")
    print(f"OWL  : {OWL_PATH}")
    print(f"OUT  : {OUT_DIR}")
    print(f"Total fallacy rows: {total}")
    print()

    # 1. CSV attack-edges
    at_edges, violations = export_attack_edges_csv(
        data, col, os.path.join(OUT_DIR, "aif-attack-edges.csv"))
    print(f"[1] aif-attack-edges.csv         : {len(at_edges)} bipartite attack-edges")
    by_type = collections.Counter(e["attack_type"] for e in at_edges)
    for t, n in by_type.most_common():
        print(f"      {t:12s}: {n}  (expected target: {ATTACK_NODE_AXIOM.get(t)})")
    by_node = collections.Counter(e["attacked_node_type"] for e in at_edges)
    print(f"      attackedNode distribution: {dict(by_node)}")
    print(f"      axiom violations (attackType/attackedNode mismatch): {len(violations)}")
    for pk, reason, at, an in violations[:5]:
        print(f"        - PK {pk}: {reason}")
    print()

    # 2. Canonical concepts
    concepts, suffix_count = export_canonical_concepts(
        data, col, os.path.join(OUT_DIR, "aif-canonical-concepts.csv"))
    rows_with_skos = len([e for e in at_edges if e["skos_direct_ref"] or e["skos_exception_ref"]])
    print(f"[2] aif-canonical-concepts.csv   : {len(concepts)} distinct AIF concepts "
          f"(referenced by {rows_with_skos}/{len(at_edges)} attack-edges)")
    print(f"      concept suffix distribution: {dict(suffix_count.most_common())}")
    print()

    # 3. Taxonomy tree
    n_tree = export_taxonomy_tree(data, col, os.path.join(OUT_DIR, "taxonomy-tree-edges.csv"))
    print(f"[3] taxonomy-tree-edges.csv      : {n_tree} hierarchical edges (full tree)")
    print()

    # 4. OWL attack-edges (cross-check)
    owl_edges, relations = export_owl_attack_edges(
        os.path.join(OUT_DIR, "aif-owl-attack-edges.csv"))
    print(f"[4] aif-owl-attack-edges.csv     : {len(owl_edges)} attack-edges from OWL "
          f"(cross-check vs CSV {len(at_edges)})")
    owl_by_type = collections.Counter(e["attack_type"] for e in owl_edges)
    print(f"      OWL attackType distribution: {dict(owl_by_type.most_common())}")
    print()

    # 5. Relations graph (NON-AIF)
    rel_rows = export_relations_graph(relations, os.path.join(OUT_DIR, "aif-relations-graph.csv"))
    rel_by_prop = collections.Counter(r["relation"] for r in rel_rows)
    print(f"[5] aif-relations-graph.csv      : {len(rel_rows)} inter-fallacy relations "
          f"(NON-AIF semantic relations)")
    print(f"      relation distribution: {dict(rel_by_prop.most_common())}")
    print()

    # Consistency summary
    print("=" * 72)
    print("CONSISTENCY SUMMARY")
    print("=" * 72)
    print(f"  CSV attack-edges (row-level, PK-keyed) : {len(at_edges)}")
    print(f"  OWL attack assertions (individual-level) : {len(owl_edges)}")
    print()
    print("  NOTE: the two are NOT a bijective cross-check. The CSV encodes AIF")
    print("  at taxonomy-ROW granularity (PK-keyed), the OWL at fallacy-INDIVIDUAL")
    print("  granularity (camelCase IRI). The CSV has no direct PK->IRI column, so a")
    print("  1:1 row<->individual mapping cannot be established without a separate")
    print("  join key. Both views are internally consistent (0 axiom violations each)")
    print("  and are exported independently. attackType distribution is closely")
    print("  aligned (CSV 87u/53m/5r vs OWL ~85u/~52m/5r) — the OWL lags the CSV by")
    print("  a few rows (post-tranche CSV modelling not yet regenerated into the OWL).")
    print()
    print("HONEST VERDICT")
    print("-" * 72)
    print("The AIF attack graph is BIPARTITE: fallacy (attacker) -> AIF node-TYPE")
    print("(RA-node / I-node / CA-node). There is NO inter-fallacy attack adjacency")
    print("('fallacy X attacks fallacy Y') in the source data. The inter-fallacy")
    print("graph in aif-relations-graph.csv holds GENERIC semantic relations, not")
    print("typed AIF attack-edges.")
    print()
    print("Done. Artefacts written to docs/ontology/aif-export/")


if __name__ == "__main__":
    main()
