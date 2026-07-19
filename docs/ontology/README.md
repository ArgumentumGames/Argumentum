# Argumentum Ontology

**OWL ontologies for the Argumentum fallacy & virtue taxonomy**, reconciled against the [ASPIC+ / AIF (Argument Interchange Format)](http://www.argumentationresearch.org/aif.html) framework. This directory holds the canonical artefacts for publication (#133).

| Artefact | Namespace IRI | Size | Subjects |
|---|---|---:|---:|
| [`argumentum.owl`](argumentum.owl) | `https://www.argumentum.games/argumentum_fallacies.owl#` | ~5.9 MB | 1408 fallacies |
| [`argumentum_virtues.owl`](argumentum_virtues.owl) | `https://www.argumentum.games/argumentum_virtues.owl#` | ~863 KB | 223 virtues |

**Version:** 1.0.0 · **License:** LGPL-3.0 (see root [`LICENSE`](../../LICENSE)) · **Format:** OWL/XML.

---

## 1. What the ontology models

Each fallacy and virtue is published as an OWL **named individual** (`#fallacy` / `#virtue` class hierarchy) with:

- a stable IRI (`https://www.argumentum.games/argumentum_fallacies.owl#<camelCaseName>`);
- **bilingual preferred labels and descriptions** (FR canonical + EN secondary) via `skos:prefLabel` / `rdfs:comment`;
- a **full hierarchy** via `skos:broader` (1407 `broader` assertions in the Fallacies ontology);
- links to the **Walton AIF scheme ontology** and the **relational / attack layers** (below).

### Scope note (honest)
The OWL generator is **bilingual (FR + EN)**. It does **not** carry the 6 other release languages (RU/PT/ES/AR/FA/ZH). The 8-language claim of v0.9.0 applies to the CSV / PDF / SVG assets, **not** to the OWL artefact.

---

## 2. The three relational layers

The ontology serializes three distinct relational layers, each grounded in a different CSV column family:

### Layer 1 — skos Walton mappings (native scheme classification)
Each typed fallacy is mapped to one or more **Walton AIF conflict / inference schemes** via `skos:{broadMatch, closeMatch, narrowMatch, exactMatch, relatedMatch}`. Source columns: `AIF_skosDirectRef`, `AIF_skosExceptionRef`, `AIF_skosOther` (#753).

| Predicate | Count |
|---|---:|
| `skos:broadMatch` | 57 |
| `skos:closeMatch` | 10 |
| `skos:narrowMatch` | 3 |
| **Total** | **70** |

The ontology references **56 distinct AIF scheme classes** from `http://www.arg.dundee.ac.uk/aif#` (Conflict, Inference, Deductive, Analogy, Bias, CauseToEffect, ConflictingGoals, Waste, PositionToKnow, …).

### Layer 2 — crossLink inter-fallacy relations (#763)
Eight **inter-fallacy relational verbs** capture how fallacies relate to each other, emitted as OWL annotation/object properties with symmetric-flag handling. Source columns: `crossLink_*`. Emitted by `OwlGeneratorConfig.cs:242`.

| Verb | Count | Symmetric |
|---|---:|---|
| `predatesOn` | 14 | no |
| `denounces` | 3 | no |
| `leverages` | 403 | no |
| `allows` | 67 | no |
| `opposes` | 51 | yes |
| `inverts` | 83 | yes |
| `mirrors` | 721 | yes |
| `isRelatedTo` | 643 | yes |
| **Total** | **1985** | |

Coverage: 844 fallacies (59.9 %) carry at least one crossLink relation.

### Layer 3 — AIF attack (#498 Fallacies, #499 Virtues)
Each attack-typed fallacy/virtue carries a formal **ASPIC+ attack semantics**: an `attackType` (`undercut` / `undermine` / `rebut`) and the `attackedNode` it defeats (`RA-node` = inference/rule, `I-node` = premise, `CA-node` = conclusion). The node map is **deterministic** (ASPIC+ Option (a), [#707](../../docs/taxonomy/498-reconciliation-p1-closure.md) §4). Emitted by `OwlGeneratorConfig.cs:262-300`.

| attackType | attackedNode | Fallacies | Virtues | Attacks… |
|---|---|---:|---:|---|
| `undercut` | `RA-node` | 87 | 206 | the inference / rule |
| `undermine` | `I-node` | 53 | 13 | the premise |
| `rebut` | `CA-node` | 5 | 3 | the conclusion |
| **Total** | | **145** | **222** | |

Coverage: 145 / 1408 fallacies (10.3 %) — the fully-reconciled skos-only subset (P1 complete, 93 → 145, 0 residual, 0 token fabricated). The remaining ~1263 leaves have no skos signature (Layer C, deferred to a post-v0.9.0 decision).

**Rebut rarity:** relational fallacies (personal attack, genetic fallacy, moving the goalposts) are modelled as `undermine` / `undercut`, not `rebut`, because they reject without presenting an independent counter-conclusion. Rebut is structurally localized to appeal-to-consequences (~3 %).

---

## 3. Coverage summary (regenerated 2026-07-12, master `95b4210b`)

| Metric | Fallacies | Virtues |
|---|---:|---:|
| Named individuals | 1408 | 223 |
| `skos:prefLabel` | 2816 (FR + EN) | — |
| `skos:broader` (hierarchy) | 1407 | — |
| EN literals | 5558 | — |
| FR literals | 4861 | — |
| AIF attack-typed | 145 | 222 |
| crossLink relations | 1985 | — |
| skos Walton mappings | 70 | — |

See [`argumentum-owl-validation-2026-07-12.md`](argumentum-owl-validation-2026-07-12.md) for the layer-by-layer validation report.

### Consumable CSV exports (Layer 3, for downstream consumers)

The AIF attack-graph (Layer 3 above) is also exported as **consumable CSV** in [`aif-export/`](aif-export/README.md) — bipartite typed attack-edges (Fallacies 145 + Virtues 222), OWL good-tenor edges (Virtues 222 → 14 schemes), canonical concepts, the taxonomy tree, and inter-fallacy semantic relations. Reproducible via `python tools/aif-attack-graph-export.py` + `python tools/aif-virtues-export.py` (read-only on sources). Consumers: CoursIA ICT #7289 / uplift #5721, Layer C v1.0 #790.

---

## 4. Regeneration

The OWL is **regenerable** from the CSV taxonomy via a single-stage entry-point (no harvest / PDF / mindmap):

```bash
dotnet run --project Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj -- --generate-owl
```

`Program.cs:397` routes `--generate-owl` to `Mode = ConverterMode.OwlGenerator`, which reads the Fallacies + Virtues CSV datasets and emits both OWL files to `Target/<lang>/Ontology/`. The committed artefacts in this directory are the canonical copies staged from that run.

---

## 5. Known limitations

- **Bilingual only** (FR + EN) — see §1 scope note.
- **Layer C not covered** — ~1263 fallacy leaves without a skos signature are not AIF-typed (generative pass deferred). See the P1 closure report.
- **OWLSharp round-trip bug** (#133): the `rdf:type` / `skos:inScheme` assertions are dropped by the OWL/XML round-trip. Readers are scoped on the surviving annotations (`prefLabel`, `broader`, the 3 relational layers). This does not affect the generated artefact.
- **`versionInfo` = 1.0.0** — bump to the v0.9.0 release tag at publication time.

---

## 6. Publication procedure (staged — #133)

The artefact is **publication-ready**. The actual publish requires infra / `jsboige` and is tracked in #133:

1. Final regen from the tagged master (`--generate-owl`), bump `versionInfo` to the release tag.
2. Decide the canonical hosting IRIs (`https://www.argumentum.games/argumentum_*.owl#` is the current `ontologyIRI`); set up the PURL / content-negotiation redirect if a persistent PURL is desired.
3. Upload the two `.owl` files to the chosen host; verify the IRIs resolve.
4. Add the download link to the root `README.md` (the v0.9.0 release notes already reference the ontology package).
5. Cross-link from the Walton AIF portal / LOV if desired.

---

## 7. Provenance

- **Generated by:** `Argumentum.AssetConverter` — `Ontology/OwlGeneratorConfig.cs` (Fallacies) and `Ontology/VirtueOwlGeneratorConfig.cs` (Virtues), using the [OWLSharp](https://github.com/mdesalvo/owlsharp) serializer.
- **Source data:** `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` (1408 rows × 104 cols) and `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` (223 rows × 81 cols).
- **Methodology:** AIF reconciliation tier-par-confiance documented in [`docs/taxonomy/498-reconciliation-p1-closure.md`](../taxonomy/498-reconciliation-p1-closure.md) (P1 skos-only back-fill, 7 tranches, 0 token fabricated, deterministic ASPIC+ node map #707 §4).
- **Last regeneration:** 2026-07-12 from master `95b4210b` (#787).
