# AIF attack-graph export — typed attack-edges + inter-fallacy relations

**Generated** : 2026-07-19 (po-2024, dispatch ai-01 grain msg-eltaoz)
**Sources** : `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` (master `c877c5ba`) + `docs/ontology/argumentum.owl`
**Posture** : read-only export of EXISTING AIF modelling. 0 new modelling. 0 write to sources.
**Consumer** : CoursIA ICT-series #7289 / uplift #5721/#6409 (strate-6 argumentation), Argumentum Layer C v1.0 (#790).

---

## Honest verdict (no fabrication)

**The AIF attack graph is BIPARTITE, not inter-fallacy.** Each fully-modeled fallacy is an **attacker** that targets an **abstract AIF node-TYPE** (`RA-node` / `I-node` / `CA-node`), NOT another fallacy. There is **no** "fallacy X attacks fallacy Y" adjacency in the source data — the columns `AIF_attackType` + `AIF_attackedNode` encode an attacker→node-type edge, not an attacker→fallacy edge.

A separate inter-fallacy graph DOES exist (in the OWL, via `isRelatedTo`, `mirrors`, etc.), but those are **generic semantic relations**, not typed AIF attack-edges. Exported separately and labelled as such.

### AIF / ASPIC+ semantics (attackType → attacked-node-type)

This correspondence is **axiomatic** in the data (145/145 rows respect it, 0 violations):

| `AIF_attackType` | `AIF_attackedNode` | Count | AIF meaning |
|---|---|---:|---|
| `undercut` | `RA-node` | 87 | attacks the **rule-application** link (the inference step) |
| `undermine` | `I-node` | 53 | attacks the **information/premise** node |
| `rebut` | `CA-node` | 5 | attacks via a **conflicting application** (counter-argument) |

---

## Artefacts

All CSVs are UTF-8, comma-delimited, header row. Regenerate via `python tools/aif-attack-graph-export.py`.

### `aif-attack-edges.csv` — 145 bipartite attack-edges (CSV-sourced, primary)

The typed AIF attack graph, PK-keyed. One row per fully-modeled fallacy.

| Column | Meaning |
|---|---|
| `attacker_pk` | Fallacy PK (primary key, stable across CSV versions). **Use this as the node id.** |
| `attacker_label` | Best-effort label (`nom_vulgarisé` else `carte`). Often empty on family-level rows — join the full taxonomy CSV for a richer label. |
| `family` / `subfamily` / `subsubfamily` | Taxonomy path (FR). |
| `decimal_path` | Hierarchical position (comma-separated, e.g. `1,11` = family 1, sub 11). |
| `attack_type` | `undercut` / `undermine` / `rebut`. |
| `attacked_node_type` | `RA-node` / `I-node` / `CA-node` (the abstract AIF node-TYPE attacked). |
| `skos_direct_ref` / `skos_exception_ref` | AIF canonical concept(s) referenced (see concepts file). 70/145 edges carry ≥1. |
| `skos_mapping_type` | `skos:broadMatch` / `skos:closeMatch` / etc. |
| `expected_node_type` / `node_type_axiom_ok` | Axiom cross-check (always `1` — 0 violations). |

**Adjacency semantics for a consumer**: build a bipartite graph `attacker_pk --[attack_type]--> attacked_node_type`, then optionally attach `skos_direct_ref` as the canonical-concept label on the edge. Do NOT try to build an edge between two `attacker_pk` values from these columns — that information is not present.

### `aif-canonical-concepts.csv` — 60 distinct AIF canonical concepts

The AIF ontology vocabulary referenced by the 145 attackers via skos.

| Column | Meaning |
|---|---|
| `concept_id` | AIF canonical concept (e.g. `OppositeConsequences_Conflict`, `Ignorance_Inference`). |
| `suffix` | Concept category: `Conflict` (19) / `Inference` (35) / `Scheme` (6). |
| `ref_count` | Number of attack-edges referencing this concept. |
| `sample_attacker_pks` | Up to 3 attacker PKs (join key). |

### `taxonomy-tree-edges.csv` — 1408 hierarchical edges

The full Fallacies taxonomy tree (all 1408 fallacies, not just the 145 fully-modeled). `parent_path` → `child_path` (child_pk). Useful as the backbone graph; the 145 attack-edges are a typed decoration on a subset of leaves.

### `aif-owl-attack-edges.csv` — 142 attack-edges from the OWL (cross-view)

Same AIF attack graph, extracted from `argumentum.owl` AnnotationAssertions (`aifAttackType` + `aifAttackedNode`). `attacker_iri_fragment` is a camelCase fallacy individual (e.g. `hastyGeneralization`), NOT a PK. **Not bijective with the CSV** (see granularity note below).

### `aif-relations-graph.csv` — 1734 inter-fallacy relations (NON-AIF)

`source_fragment --[relation]--> target_fragment`. These are the OWL's generic semantic relations between fallacy individuals. **NOT AIF attack-edges** — a different, complementary graph.

| Relation | Count | Semantics (loose) |
|---|---:|---|
| `isRelatedTo` | 606 | generic relatedness (bidirectional in source) |
| `mirrors` | 548 | structural mirror (bidirectional) |
| `leverages` | 399 | exploits / builds on |
| `allows` | 66 | enables / permits |
| `inverts` | 62 | logical inversion |
| `opposes` | 38 | direct opposition |
| `predatesOn` | 13 | historical precedence |
| `denounces` | 2 | explicit denunciation |

---

## CSV ↔ OWL granularity note (not a contradiction)

The two attack-edge files (CSV 145, OWL 142) are **not a bijective cross-check**:

- **CSV** encodes AIF at **taxonomy-row** granularity (PK-keyed) — one entry per fallacy row with populated `AIF_attackType`+`AIF_attackedNode`.
- **OWL** encodes AIF at **fallacy-individual** granularity (camelCase IRI, e.g. `hastyGeneralization`).

The CSV has no PK→IRI column, so a 1:1 row↔individual mapping cannot be established without a separate join key. Both views are internally consistent (0 axiom violations each) and are exported independently. `attackType` distribution is closely aligned (CSV 87u/53m/5r vs OWL ~85u/~52m/5r); the OWL lags the CSV by a few rows (post-tranche CSV modelling not yet regenerated into the OWL — expected, the OWL is a derived artefact).

---

## Re-run

```bash
python tools/aif-attack-graph-export.py
```

Idempotent, 0 external dependency (Python stdlib + `xml.etree`), runtime ~3s. Overwrites the 5 CSVs in this directory. 0 write to the source CSV/OWL.

---

## Out of scope

- ⛔ **New AIF modelling** (inter-fallacy attack edges). The data does not contain them; fabricating them would violate the DoD. If a typed inter-fallacy attack graph is needed, that is a separate modelling decision (new grain, gated).
- ⛔ **PK↔IRI join key** (would require a new CSV column mapping taxonomy PKs to OWL camelCase individuals — a schema change, not an export).
- ⛔ **Write to sources** (CSV taxonomy, argumentum.owl).

## Refs

- Dispatch: ai-01 → po-2024 (msg-eltaoz, 2026-07-19), grain non-bloquant post-tag-safe.
- AIF modelling history: tranches 1–1d (PRs #498/#753/#769/#776/#779), mémoire `aif-no-inherit-attacktype-from-anchor`.
- CoursIA consumers: ICT #7289, uplift #5721/#6409.
- Argumentum Layer C v1.0: #790 (deferred post-tag).
