# Argumentum Ontology

**OWL ontologies for the Argumentum fallacy & virtue taxonomy**, reconciled against the [ASPIC+ / AIF (Argument Interchange Format)](http://www.argumentationresearch.org/aif.html) framework. This directory holds the canonical artefacts for publication (#133).

<!-- index-of-record -->

Ce README est un **index of record** : chaque fichier suivi du dossier est cité avec son statut (`current`, `superseded`, `historical`) — la table des artefacts ci-dessous et la §8 en fin de page. Ajouter un fichier ici sans sa ligne rougit la CI (`python tools/test_docs-index-contract.py`) ; la méthode est dans [`docs/agents/consolidation.md`](../agents/consolidation.md).

| Artefact | Namespace IRI | Size | Subjects | Statut |
|---|---|---|---:|---|
| [`argumentum.owl`](argumentum.owl) | `https://www.argumentum.games/argumentum_fallacies.owl#` | 6 019 161 B (mesurés 2026-11-19) | 1408 fallacies | `current` |
| [`argumentum_virtues.owl`](argumentum_virtues.owl) | `https://www.argumentum.games/argumentum_virtues.owl#` | 1 088 889 B (mesurés 2026-11-24) | 223 virtues | `current` |

L'OWL Vertus a été **régénéré le 24/11** (P24-37) : le gel institué le 17/11 (révisé le 21/11) tombe parce que son motif a disparu — le remède d'épinglage des IRI (P24-34, §6.1) tient sur le corpus réel, la régénération ne déplace **aucune** IRI servie (224 → 224 classes, 0 perdue) et ses 114 clés changées sont toutes rattachées à un changement du CSV, `seeAlso` compris (78 retirés / 61 ajoutés, solde −17) — constat [`133-owl-virtues-regen-2026-11-24.md`](133-owl-virtues-regen-2026-11-24.md), empreinte `884be0f3…` ; la prédiction inverse (rupture d'IRI) reste lisible dans [`133-owl-virtues-peremption-mesuree-2026-10-09.md`](133-owl-virtues-peremption-mesuree-2026-10-09.md). L'OWL Fallacies avait été régénéré le 17/11 (#2295).

**Version:** 1.0.0 · **License:** CC BY-SA 4.0 (see root [`LICENSE-CONTENT.md`](../../LICENSE-CONTENT.md) §1 — derived artefacts of the CC BY-SA taxonomies, decision Q-15a 2026-09-24; the C# generator itself remains LGPL-3.0) · **Format:** OWL/XML.

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
- **`versionInfo` = 1.0.0** — bump to the release tag at publication time (**v2.0.0**, ex-v0.9.0 — re-scoped 2026-08-06, #999). The bump is a **config diff** (recipe in §6.1), pinned by `OwlVersionInfoContractTests` — nothing else may move this value.

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

**The `v1.0.0/` snapshot is served too, and it declares the same `ontologyIRI`.** `static.yml` mounts
all of `docs/ontology/`, so `…/docs/ontology/v1.0.0/argumentum.owl` and `…/argumentum_virtues.owl`
answer as well — measured 2026-11-28: `200`, `application/rdf+xml`, body byte-identical to the
committed blobs (sha256 `e6da4eb8…` / `d4099c79…`, 6 019 161 and 1 094 832 bytes). Both declare the
`ontologyIRI` of their canonical counterpart (`https://www.argumentum.games/argumentum_fallacies.owl#`
/ `…_virtues.owl#`). **Accepted** (ai-01 arbitration, 2026-11-27): these are the bytes the site served
until the 24/11 Virtues regeneration, and a readable versioned snapshot is normal use — the canonical
path serves the **latest** generation and the version lives in `owl:versionInfo` (§6.1). The snapshot
is frozen: [`tools/test_133-owl-snapshot-pin.py`](../../tools/test_133-owl-snapshot-pin.py) pins its two
oids (`6ae37077…` / `a9f5b7ec…`) and the shared `ontologyIRI`, so one changed byte reddens the CI.

A readable page sits at the entry of the folder: [`index.html`](index.html) is the human-readable
index of what is served here — which file, under which namespace IRI, at which version, with which
bytes (sha256). It is **generated** from the files of the folder by
[`tools/133-ontology-index-page.py`](../../tools/133-ontology-index-page.py) — every figure on it is
read from the `.owl` headers or computed on the bytes, never copied from a document. It carries **no
date**, so replaying the script yields the same bytes: `python tools/133-ontology-index-page.py --check`
exits 0 only while the page still matches the files it describes, and the contract
[`tools/test_133-ontology-index-page.py`](../../tools/test_133-ontology-index-page.py) runs that check in
CI, so one changed byte in an `.owl` reddens the pull request. The folder being mounted as a whole by
`static.yml`, the page is served at `…/docs/ontology/index.html` — **derived** from the mount, not
measured (the served URLs of the artefacts are the ones measured above).

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

### 6.1 Version bump and `v{semver}/` snapshot recipe (P24-05, 2026-11-21)

**Doctrine** (issue #133, 2026-10) : the canonical URL always serves the **latest** generation; the
version lives **in the file** via `owl:versionInfo`; snapshot copies under `v{semver}/` are made at
**release tags only**. Nothing freezes between updates.

**The bump is a config diff, not a code edit.** The version is a property of the two document configs —
`OwlGeneratorConfig.cs` and `VirtueOwlGeneratorConfig.cs`, `Version = new Version(1,0,0)` (the C#
initializers **are** the configuration, `SkipConfigFile = true`). At the tag:

1. Bump `Version` in **both** initializers (e.g. `new Version(2,0,0)` for #134) — one reviewed commit;
2. Regenerate: `--generate-owl` (§4) and commit both artefacts — the emitted `owl:versionInfo` follows
   the config (pinned by `OwlVersionInfoContractTests`: defaults at 1.0.0 until this diff, emission
   carries the configured value, literal survives the OWL2XML reload, **no Argumentum-namespace IRI
   moves** — A/B control);
3. Copy each artefact to `v{semver}/argumentum[_virtues].owl` (the versioned snapshot — a new tracked
   directory per tag; its line in the §8 index-of-record comes with the copy). The canonical path is
   left untouched: latest stays latest.

**Why the Virtues freeze outlives this recipe** (décision #133 du 17/11, **révisée le 21/11**,
c.7062194283) : regenerating `argumentum_virtues.owl` renames **2 already-served IRIs**
(`acceptableInformalLogic`→`solidInformalLogic`, `takingTheOpponentsIdeas…`→`takingTheParties…` —
cause measured in
[`133-owl-virtues-peremption-mesuree-2026-10-09.md`](133-owl-virtues-peremption-mesuree-2026-10-09.md)).
The first version of the decision held the freeze « until this recipe is in place » — **withdrawn**:
a `v{semver}/` snapshot carries the **same IRIs** as the canonical file, the canonical path serves
the **latest** generation, and the version served today is copied nowhere, so the renaming breaks
what the canonical URL serves whatever the snapshot. The freeze now holds until the **IRI-pinning
remedy** (grain P24-34, #458). **Fait le 2026-11-24 (P24-37)** : les artefacts gelés ont été copiés
dans [`v1.0.0/`](v1.0.0/) avant la régénération — à l'octet les blobs committés, eux-mêmes les
octets que le site servait (mesuré) — la génération d'avant reste donc récupérable.

**Structural remedy = grain P24-34** (17/11, root cause of the same measure; numbered in #458 by the
revised decision): the IRI is minted from the English label (`GetId(TextEn)`), so **any** label
rename relocates a served IRI — the « stable endpoint » asks a naming policy the label cannot
provide (§6 « Publishing pins a surface that already moves »). Remedy: **pin the existing IRIs** —
decouple minting from the label so a rename never breaks a served IRI again, in both generators.
Done when a Virtues regeneration on a branch makes **0 served IRI disappear** (gate: P24-05 merged;
counter-review po-2023). This section is where the decision lives; the implementation is P24-34.

**Implemented (21/11, P24-34)**: fragments now resolve through the pk-keyed `OwlIriRegistry`
(`Ontology/OwlIriRegistry.cs`, frozen from the committed artefacts — Fallacies 1408/1408, Virtues
223/223), both generators call `Resolve{Fallacy,Virtue}IriFragment` at every mint site, and an
unknown pk **refuses loudly** (a new corpus row extends the registry in the same reviewed diff —
the label never mints a served IRI). Measured on the branch regen (`--generate-owl`): **0 served
IRI disappears** (Fallacies 1445/1445 fragments, Virtues 227/227), both regenerated artefacts
carry `owl:versionInfo` 1.0.0, and the two renamed titles (pk 128/176) keep their served IRIs —
pinned by `OwlIriPinningContractTests` (8 facts; 5 mutation controls redden per generator, call
site included). **Exécutée le 2026-11-24 (P24-37)** : la régénération des artefacts committés
était une **décision séparée** (ai-01/owner), ouverte par le dispatch ai-01 du 24/11 ; le remède
étant en place, elle ne rompt plus rien — **0 IRI servie disparaît** sur le corpus réel (224 → 224 ;
les deux titres renommés ci-dessus gardent leur IRI), et l'instantané `v1.0.0/` porte désormais la
génération d'avant — mesures et rejeu dans
[`133-owl-virtues-regen-2026-11-24.md`](133-owl-virtues-regen-2026-11-24.md).

### Rollback

Bounded and complete: `git revert` the workflow commit → Pages rebuilds → both URLs return 404 again.
The `.owl` files stay tracked, so nothing is lost. No database, no server, no CSV or OWL write involved.

### Still staged (not done here)

`owl:versionInfo` stays `1.0.0`; the bump and snapshot gesture at the release tag (v2.0.0, ex-v0.9.0 —
re-scoped 2026-08-06, #999) now have their **recipe** (§6.1, P24-05) — the execution belongs to the tag.
The download link in the root `README.md`, a WebVOWL index page, and cross-linking from the Walton AIF
portal / LOV remain follow-ups.

---

## 7. Provenance

- **Generated by:** `Argumentum.AssetConverter` — `Ontology/OwlGeneratorConfig.cs` (Fallacies) and `Ontology/VirtueOwlGeneratorConfig.cs` (Virtues), using the [OWLSharp](https://github.com/mdesalvo/owlsharp) serializer.
- **Source data:** `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` (1408 rows × 104 cols) and `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` (223 rows × 81 cols).
- **Methodology:** AIF reconciliation tier-par-confiance documented in [`docs/taxonomy/498-reconciliation-p1-closure.md`](../taxonomy/498-reconciliation-p1-closure.md) (P1 skos-only back-fill, 7 tranches, 0 token fabricated, deterministic ASPIC+ node map #707 §4).
- **Last change to the artefacts** (re-mesuré 2026-11-24) : `argumentum.owl` 2026-11-17 (`7a39d9ad`,
  #2295 — régénération : schéma imité (a) + questions critiques (c) + `seeAlso`, contrôle « 0 IRI
  disparue ») ; avant cela 2026-09-15 (`2b572787`, #1379 — PK 511 IRI `nonverbalCommunication`,
  PK 598 `hastyInduction`). `argumentum_virtues.owl` **2026-11-24 (P24-37 — régénération sur corpus
  à jour : 0 IRI perdue, 114 clés changées toutes rattachées au CSV, `884be0f3…`, 1 088 889 o ;
  constat [`133-owl-virtues-regen-2026-11-24.md`](133-owl-virtues-regen-2026-11-24.md))** ; avant
  cela 2026-09-30 (`c1491de9`, #1681 — reconcile après regen mindmaps x8). Le layer counts in
  §2/§3 are the older 2026-07-12 snapshot — see the freshness note in §1.

---

## 8. Index of record — les fichiers du dossier

Mesuré le 2026-11-24 sur master `0543ab69`. Le dossier comptait **24 fichiers suivis**
(`git ls-files docs/ontology | wc -l`) : les deux artefacts (table ci-dessus), ce README, **neuf
constats datés**, le sous-dossier [`aif-export/`](aif-export/) (10 fichiers couverts par leur
ligne ci-dessous — son propre README ne porte pas la marque, il n'engage que lui) et le
sous-dossier [`v1.0.0/`](v1.0.0/) (2 fichiers, couverts par leur ligne ci-dessous). La page
d'accueil [`index.html`](index.html), ajoutée le 2026-11-28 (P24-46), porte le compte à **25**.

### Où en sont les deux sujets

- **Publication #133 (ouvert).** Re-mesuré le 19/11 : les deux URL répondent 200 et le corps servi
  est à l'octet près le blob committé (smoke [`tools/133-pages-owl-smoke.sh`](../../tools/133-pages-owl-smoke.sh) —
  sha256 `e6da4eb8…` / `d4099c79…`). **P24-37 (24/11) change le blob Vertus en `884be0f3…` : au
  merge, Pages republie et le sha servi doit suivre — le smoke est la mesure, à rejouer après le
  merge.** Anomalie concomitante : le témoin mort (chemin absent) reçoit
  **503** au lieu de 404, deux exécutions de suite — comportement de l'hôte, sans effet sur
  l'identité des artefacts, à re-vérifier avant de citer un « 404 propre » n'importe où. L'état
  vivant (bump `versionInfo` v2.0.0, snapshots `v{semver}/`, pont IRI↔endpoint) vit sur
  [#133](https://github.com/ArgumentumGames/Argumentum/issues/133).
- **Chantier AIF #498 (clos le 17/11/2026).** Les constats ci-dessous sont les enregistrements
  datés du chantier ; la méthodologie de réconciliation citée en §7 vit dans
  [`docs/taxonomy/`](../taxonomy/), dont la tranche #498 est déjà indexée (`91707a7e`).

### Les fichiers

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`index.html`](index.html) | 2026-11-28 | `current` | Page d'accueil lisible du dossier (P24-46) : quel fichier, sous quel IRI d'espace de noms, en quelle version, avec quels octets (sha256). Elle est **générée** par [`tools/133-ontology-index-page.py`](../../tools/133-ontology-index-page.py) à partir des fichiers eux-mêmes, porte **aucune date** (rejeu ⇒ octets identiques) et son contrôle `--check` échoue dès qu'un `.owl` du dossier change — garde [`tools/test_133-ontology-index-page.py`](../../tools/test_133-ontology-index-page.py) |
| [`133-etat-servi-2026-11-05.md`](133-etat-servi-2026-11-05.md) | 2026-11-05 | `historical` | Mesure HTTP lecture-seule de l'état servi, canal par canal (05/11) — l'état courant de la publication vit sur [#133](https://github.com/ArgumentumGames/Argumentum/issues/133) et dans le smoke ci-dessus |
| [`133-owl-publication-certificate.md`](133-owl-publication-certificate.md) | 2026-07-12 | `historical` | Certificat « publication-safe » de l'artefact de juillet — l'OWL a été régénéré depuis (15/09 puis 17/11) sans nouveau certificat |
| [`133-owl-virtues-peremption-mesuree-2026-10-09.md`](133-owl-virtues-peremption-mesuree-2026-10-09.md) | 2026-10-09 | `historical` | Péremption mesurée de l'OWL Vertus et coût caché d'une régénération (rupture d'IRI) — la décision vive est le gel, pris le 17/11 sur [#133](https://github.com/ArgumentumGames/Argumentum/issues/133) |
| [`133-owl-virtues-regen-2026-11-24.md`](133-owl-virtues-regen-2026-11-24.md) | 2026-11-24 | `current` | Régénération de l'OWL Vertus (P24-37) : 0 IRI perdue, 114 clés changées toutes rattachées au CSV, `seeAlso` 78 retirés / 61 ajoutés, empreinte `884be0f3…` — la mesure vive, rejouable par [`tools/133-owl-regen-delta.py`](../../tools/133-owl-regen-delta.py) |
| [`v1.0.0/`](v1.0.0/) | 2026-11-24 | `superseded` | Instantané de la génération **servie avant** la régénération P24-37 (2 fichiers, blobs `e6da4eb8…` / `d4099c79…` — les octets que le site servait, mesuré) — recours de comparaison ; le successeur est la génération canonique courante, décidée sur [#133](https://github.com/ArgumentumGames/Argumentum/issues/133) |
| [`498-cadrage-exceptions-schemes-2026-09-21.md`](498-cadrage-exceptions-schemes-2026-09-21.md) | 2026-09-21 | `historical` | Cadrage « sophismes comme exceptions à des schemes » — **ses chiffres d'héritage sont retirés depuis le 06/11** (avertissement en tête du fichier : l'instrument découpait `decimal_path` aux virgules) ; pièce à citer, jamais chiffre à reprendre |
| [`498-instrument-path-2026-11-06.md`](498-instrument-path-2026-11-06.md) | 2026-11-06 | `historical` | Correctif d'instrument : la hiérarchie se lit dans `path`, pas `decimal_path` — l'instrument corrigé ([`tools/498-aif-cadrage-measure.py`](../../tools/498-aif-cadrage-measure.py)) reste en usage |
| [`498-grain9-cadrage-scheme-imite-2026-11-10.md`](498-grain9-cadrage-scheme-imite-2026-11-10.md) | 2026-11-10 | `historical` | Cadrage du schéma imité (a) pour les 7 groupes restants — a préparé les tranches P3 mergées du 14 au 16/11 ; #498 clos le 17/11 |
| [`498-grain10-mappingtype-mesure-2026-11-10.md`](498-grain10-mappingtype-mesure-2026-11-10.md) | 2026-11-10 | `historical` | Verdicts du DoD 2 sur les 116 correspondances (partition 60 inchangées + 56 reclassées, revue ai-01 du 11/11) |
| [`argumentum-owl-validation-2026-07-12.md`](argumentum-owl-validation-2026-07-12.md) | 2026-07-12 | `historical` | Validation layer-par-layer de l'OWL de juillet — c'est le snapshot que reproduisent les tables §2/§3 (voir l'avertissement de fraîcheur en §1) |
| [`aif-export/`](aif-export/) | 2026-10-22 | `current` | Exports CSV consommables (10 fichiers, README compris) — la couche attaque est inchangée par la regen du 17/11 (#2295 : ajouts seulement) ; la ligne de provenance du README interne cite encore l'OWL de septembre |
