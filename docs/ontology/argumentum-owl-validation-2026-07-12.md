# #133 OWL Publication Prep — Validation Report (2026-07-12)

**Worker** po-2024 · **Date** 2026-07-12 · **Base** master `474607c9` · **GO** ai-01 dispatch `ti19qu` (interim #133). Companion to the regenerated `docs/ontology/argumentum.owl`.

> Scope: refresh the committed Fallacies OWL after the AIF reconciliation P1 (#498, 93→145 attack-typed) and validate the 3 relational layers serialize correctly. **0 write prod CSV** — `--generate-owl` reads CSV + emits OWL only.

---

## 0. TL;DR

The committed `argumentum.owl` was **stale (pre-P1)**: its AIF attack layer counted **93** annotation assertions while prod CSV had advanced to **145**. Regenerating from master `474607c9` brings the OWL AIF layer to **145** = exact match with prod. The other two relational layers (skos Walton + crossLink #763) were already current and are **byte-stable** in the regen.

| Layer | Source | Committed (pre-regen) | Fresh (master `474607c9`) | Δ |
|---|---|---:|---:|---:|
| **1. skos Walton** (mapping types) | `AIF_skosDirectRef/ExceptionRef/Other` (#753) | 70 | 70 | **0** (stable) |
| **2. crossLink** (relational verbs) | `CrossLink_*` (#763) | 1985 | 1985 | **0** (stable) |
| **3. AIF attack** (#498) | `AIF_attackType` + `AIF_attackedNode` | 93 | **145** | **+52** ✅ |

The +52 is exactly the P1 skos-only back-fill (14 tranche-1 PRECEDENT + 2 PREC-TIE + 36 SUFFIX-ONLY). `argumentum_virtues.owl` is **byte-identical** (not re-committed).

---

## 1. Regeneration (entry-point `--generate-owl`)

`Program.cs:397` routes `--generate-owl` to a single-stage run with `Mode = ConverterMode.OwlGenerator` — no harvest, no PDF, no mindmap. Reads the Fallacies + Virtues CSV datasets, emits two OWL documents to `Target/<lang>/Ontology/`.

```
Mode génération OWL (ontologies Fallacies + Virtues)
Loading csv from dataSet Fallacies - Taxonomy (1408 rows)
Owl document Target/fr/Ontology/argumentum.owl successfully saved       [1.67s]
Loading csv from dataSet Fallacies - Virtues (223 rows)
Virtue Owl document Target/fr/Ontology/argumentum_virtues.owl saved     [1.75s]
```

Exit 0. Output: `argumentum.owl` 6 030 505 chars (+138 KB vs committed 5 892 331), `argumentum_virtues.owl` 862 709 chars (identical).

---

## 2. Layer-by-layer validation (grep/count XML)

### Layer 1 — skos Walton (native scheme mappings)
Emitted by `OwlAdapter.cs` via `SKOSVocabulary.{ExactMatch,CloseMatch,BroadMatch,NarrowMatch,RelatedMatch}`.

| Predicate | Committed | Fresh |
|---|---:|---:|
| `skos:broadMatch` | 57 | 57 |
| `skos:closeMatch` | 10 | 10 |
| `skos:narrowMatch` | 3 | 3 |
| `skos:exactMatch` | 0 | 0 |
| `skos:relatedMatch` | 0 | 0 |
| **TOTAL** | **70** | **70** |

Stable: the P1 reconciliation only added `attackType` to skos-only rows; it did not touch the skos mapping columns.

### Layer 2 — crossLink relational verbs (#763)
Emitted by `OwlGeneratorConfig.cs:242` (8 verbs, symmetric flags honored).

| Verb | Committed | Fresh | Symmetric |
|---|---:|---:|---|
| `predatesOn` | 14 | 14 | no |
| `denounces` | 3 | 3 | no |
| `leverages` | 403 | 403 | no |
| `allows` | 67 | 67 | no |
| `opposes` | 51 | 51 | yes |
| `inverts` | 83 | 83 | yes |
| `mirrors` | 721 | 721 | yes |
| `isRelatedTo` | 643 | 643 | yes |
| **TOTAL** | **1985** | **1985** | |

Stable: #763 crosslinks were merged before the P1 reconciliation and are unchanged.

### Layer 3 — AIF attack (#498) ← the gap this regen closes
Emitted by `OwlGeneratorConfig.cs:262-300`:
- `aifAttackType` = annotation property + plain literal (`undercut`/`undermine`/`rebut`);
- `aifAttackedNode` = object property → `AifNode()` maps to `RA-node` / `I-node` / `CA-node`.

| Predicate | Committed (pre-P1) | Fresh (master `474607c9`) | Δ |
|---|---:|---:|---:|
| `aifAttackType` | 93 | **145** | **+52** ✅ |
| `aifAttackedNode` | 94 | 146 | +52 (+1 = the ObjectProperty declaration) |

**Validation: fresh `aifAttackType` count (145) == CSV prod attack-typed total (145) → PASS ✅.**

Prod distribution (CSV) reflected in the OWL literal values:

| attackType | attackedNode | CSV count | OWL role |
|---|---|---:|---|
| `undercut` | `RA-node` | 87 | attacks the inference/rule |
| `undermine` | `I-node` | 53 | attacks the premise |
| `rebut` | `CA-node` | 5 | attacks the conclusion |
| **TOTAL** | | **145** | |

Node/type map is the deterministic ASPIC+ Option (a) (#707 §4); **0 node-type inconsistency** in the CSV (verified: every typed row's `(attackType, attackedNode)` pair ∈ {undercut/RA, undermine/I, rebut/CA}).

---

## 3. Diff scope — AIF attack layer only

`git diff docs/ontology/argumentum.owl` = 530 insertions / 10 deletions. Every added line belongs to an `<AnnotationAssertion>` block asserting an AIF attack triple (subject fallacy IRI + `aifAttackType`/`aifAttackedNode` property + literal/resource value). Sample:

```xml
  <AnnotationAssertion>
    <AnnotationProperty IRI="...#aifAttackType" />
    <IRI>...#insufficiency</IRI>           <!-- tranche-1e -->
    <Literal>undermine</Literal>
  </AnnotationAssertion>
  <AnnotationAssertion>
    <AnnotationProperty IRI="...#aifAttackedNode" />
    <IRI>...#insufficiency</IRI>
    <IRI>http://www.arg.dundee.ac.uk/aif#I-node</IRI>
  </AnnotationAssertion>
```

0 change to skos Layer 1, 0 change to crossLink Layer 2, 0 change to concept declarations / class hierarchy / multilingual labels. The regen is **additive and scoped** to the AIF layer.

---

## 4. Virtues OWL

`argumentum_virtues.owl` (223 Virtues, #499 mirrored) regenerated to **byte-identical** content (862 709 chars, `a==b` byte check). Not re-committed — the Virtues taxonomy was unchanged since the last regen.

---

## 5. Publication readiness (#133)

The regenerated `argumentum.owl` now faithfully reflects the 3-layer relational taxonomy in prod:
1. **skos Walton** — 70 scheme mappings to the AIF/Walton ontology;
2. **crossLink** — 1985 inter-fallacy relational verbs (PR #763);
3. **AIF attack** — 145 attack-typed fallacies with deterministic ASPIC+ node mapping (#498 P1 complete, 0 fabrication token).

This OWL is the canonical artefact for #133 (OWL publication). It is IRI-stable (`https://www.argumentum.games/argumentum_fallacies.owl#`), OWL/XML well-formed, and self-consistent (node-type map verified). Ready for publication / release notes; staging the artefact in this PR.

---

## 6. Gate boundaries

- ✅ Regeneration via `--generate-owl` (single-stage, 0 harvest/PDF/mindmap).
- ✅ **0 write prod CSV** — `--generate-owl` reads CSV, emits OWL; `Cards/` untouched.
- ✅ Fresh AIF layer == prod (145); skos + crossLink byte-stable; virtues identical.
- ✅ AIF-only diff scope verified (530/10, every line is an AIF annotation assertion).
- ❌ No Layer C (~750 leaf rows) — gated jsboige decision (not this PR).
- ❌ No self-merge — verdict QA = ai-01.

🤖 Worker po-2024 — #133 OWL prep, regen from master `474607c9`, 3-layer validated.
