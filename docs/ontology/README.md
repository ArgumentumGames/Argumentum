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

> **⚠️ Count freshness — measured 2026-09-15 on master `5bdbdfd1`.** The layer tables in §2 and the
> coverage table in §3 are the **2026-07-12 validation snapshot** (#787), and the relational layers have
> moved since. Against the committed artefact today: `skos` Walton mappings **70 → 116**
> (`broadMatch` 64 / `closeMatch` 33 / `narrowMatch` 19), the crossLink layer restructured by
> #1247/#1286 (**1 985 raw assertions → 674 unique relation triples**), and Virtues attack-typed
> **222 → 142** (#1242 — the virtues without a measured attack are no longer emitted). The structural
> rows still reproduce exactly (`skos:broader` 1 407 · `skos:inScheme` 1 408 · `skos:prefLabel` 2 816).
> Current measured values live in [`aif-export/README.md`](aif-export/README.md); the tables below are
> kept as the dated snapshot they are, not silently rewritten.

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
- **`versionInfo` = 1.0.0** — bump to the release tag at publication time (**v2.0.0**, ex-v0.9.0 — re-scoped 2026-08-06, #999).

---

## 6. Publication — endpoint (#133)

These artefacts are published as static files by
[`.github/workflows/static.yml`](../../.github/workflows/static.yml), which mounts `docs/ontology/` into
the GitHub Pages artifact **on every push to `master`**. Publishing is therefore not a recurring manual
step: regenerating the OWL, committing it and merging republishes it.

| Served URL | Source (committed blob) |
|---|---|
| `https://argumentumgames.github.io/Argumentum/docs/ontology/argumentum.owl` | `docs/ontology/argumentum.owl` |
| `https://argumentumgames.github.io/Argumentum/docs/ontology/argumentum_virtues.owl` | `docs/ontology/argumentum_virtues.owl` |

CardPen keeps its position at the artifact **root**, so its served URLs are unchanged.

### Post-merge smoke

```bash
tools/133-pages-owl-smoke.sh              # base URL and oracle ref overridable
```

Checks status, `Content-Type` and **sha256 of the served body against the committed blob**, with a live
control (`/` must stay `200`) and a dead control (a missing path must stay `404`). The hash is the point:
before this workflow landed, both OWL URLs returned the Pages "file not found" page — a 9 379-byte HTML
body, the same size as any other missing path on that host — so neither size nor "does it look like XML"
discriminates a served artefact from an error page.

### Honest limitations

- **No content negotiation.** GitHub Pages is a static host: no `Accept`-header dispatch and no `303`
  redirect from the namespace IRI. Distinct URLs are used instead (standard static practice). A
  W3C-style negotiated endpoint needs the `argumentum.myia.io` server and is deferred to #132.
- **`Content-Type` is the host's, not ours — and the host answers `application/rdf+xml`.** This bullet
  originally predicted `application/octet-stream` (`.owl` assumed absent from the host's MIME map); the
  live measurement refuted that prediction: both OWL URLs are served `application/rdf+xml` (#133,
  2026-09-15 — pre-pose 404 / post-pose 200 + sha256 = committed blob; re-measured 2026-09-15 by the
  counter-review, both URLs `200 application/rdf+xml`). The smoke still reports the header instead of
  asserting it — the host remains ours to verify, not to dictate. *(Erratum 2026-09-15 of the pre-merge
  prediction.)*
- **The namespace IRIs do not resolve, and nothing in the artefact bridges them.** `ontologyIRI` is
  `https://www.argumentum.games/argumentum_fallacies.owl#` (decided 2026-06-02). The file's
  `rdfs:seeAlso` assertions point to **external** references (e.g. Wikipedia) — measuring the committed
  artefact shows **no** assertion pointing at this endpoint, so the served URL and the namespace IRI are
  currently unlinked. Adding that bridge is a generator change, deliberately out of scope here.
- **This endpoint and the live DNN copy are two different artefacts.** The DNN webroot serves an
  **untraced March-2024** generation (1 405 terms, sha256 `258a9e43…`); this endpoint serves the
  committed generation. The gap between them — 1 264 common, 141 published-only, 175 committed-only — is
  measured in [`../quality/133-iri-impact-before-owl-publication.md`](../quality/133-iri-impact-before-owl-publication.md)
  and is **documented, not repaired** by the Pages change.
- **Publishing pins a surface that already moves.** `IRI = Camelize(text_en)`: any English label edit —
  including punctuation alone — relocates the IRI, so the "stable endpoint" asks for a naming policy the
  name itself cannot provide. The three options (decouple the IRI from the label / resolve by version /
  accept the move) remain open on #133; the owner has exercised the third once, for PK 511 (§3.1 of the
  dossier above).

### Rollback

Bounded and complete: `git revert` the workflow commit → Pages rebuilds → both URLs return 404 again.
The `.owl` files stay tracked, so nothing is lost. No database, no server, no CSV or OWL write involved.

### Still staged (not done here)

`owl:versionInfo` stays `1.0.0`; bump it to the release tag at publication time (v2.0.0, ex-v0.9.0 —
re-scoped 2026-08-06, #999). Snapshot copies under `v{semver}/` at release tags, the download link in the
root `README.md`, a WebVOWL index page, and cross-linking from the Walton AIF portal / LOV remain
follow-ups.

---

## 7. Provenance

- **Generated by:** `Argumentum.AssetConverter` — `Ontology/OwlGeneratorConfig.cs` (Fallacies) and `Ontology/VirtueOwlGeneratorConfig.cs` (Virtues), using the [OWLSharp](https://github.com/mdesalvo/owlsharp) serializer.
- **Source data:** `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` (1408 rows × 104 cols) and `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` (223 rows × 81 cols).
- **Methodology:** AIF reconciliation tier-par-confiance documented in [`docs/taxonomy/498-reconciliation-p1-closure.md`](../taxonomy/498-reconciliation-p1-closure.md) (P1 skos-only back-fill, 7 tranches, 0 token fabricated, deterministic ASPIC+ node map #707 §4).
- **Last change to the artefacts:** `argumentum.owl` 2026-09-15 (`2b572787`, #1379 — PK 511 IRI
  `nonverbalCommunication`, PK 598 `hastyInduction`); `argumentum_virtues.owl` 2026-09-01 (`97c4b3e3`,
  #1252 — 14 Walton scheme IRIs sanitized). The layer counts in §2/§3 are the older 2026-07-12
  snapshot — see the freshness note in §1.
