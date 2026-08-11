# #1058 — `family_en` regen manifest (what the 48 filled EN cells touch)

**Issue:** [#1058 — derive the 48 empty EN `Family` cells](https://github.com/ArgumentumGames/Argumentum/pull/1058) (MERGED `ded70c81`)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-08-11
**Base:** master `ded70c81`
**Status:** **DOSSIER / docs-only, non-gated.** This is the deliverable ai-01's dispatch `ez05p8` (tertiaire) asked for: *"la liste précise des artefacts que `family_en` alimente et qui devront être régénérés avant le tag — cherche, ne te fie pas à ma liste."* It is a **document**, not an execution. **Zero artefact regen performed here** — the heavy regen is po-2023's lane (gel pré-tag v2.0.0).

> **Headline finding (counter to ai-01's working list):** ai-01 expected **mindmaps EN + OWL** to be in scope. The code says **only the EN card PNGs and the EN PDFs assembled from them** are. The mindmap EN is doubly isolated from `family_en` (stale data fork + read-only FR alias), and OWL does not consume `Family` at all. Details + line-level proof below.

---

## TL;DR — impact surface

| Artefact (EN-only) | Consumes `Family` (EN col 20)? | Regen before tag? | Proof (code = truth) |
|---|---|---|---|
| **Fallacies card PNGs** (Face 1/2/3, Web, Web Thumbnails, Memo Face+Back) | ✅ **YES** | ✅ **YES** | `AssetConverterConfig.cs:120` & `:133`; PapaParse reads the source CSV via `HarvestManager.cs:347` |
| **EN PDFs** assembled from those PNGs (TarotCards, Fallacies_Web A0/A4/Thumbnails, Print&Play, Memo) | ✅ **YES** (transitive) | ✅ **YES** | downstream of the PNGs above |
| **Fallacies mindmap EN** (`.mm` / `.svg` / HTML wrapper) | ❌ **NO** (double isolation) | ❌ **no** (pre-existing gap, post-tag) | `Fallacy.cs:15` `Family => Famille` (RO); `FallacyDocumentConfigBase.cs:10` reads stale fork |
| **OWL ontology** | ❌ **NO** | ❌ **no** | `OwlGeneratorConfig.cs:317-318` (prefLabel = TextFr/TextEn; hierarchy via `path`) |
| Mindmap/PDF/OWL **other 7 languages** | n/a | ❌ **no** | #1058 touched col 20 (EN) only |
| **Virtues** (any artefact) | n/a | ❌ **no** | #1058 touched the Fallacies taxonomy, not Virtues |

**Bottom line for po-2023's regen runbook:** regenerate the **EN Fallacies card PNGs** then the **EN PDFs**. Mindmap EN and OWL are out of scope for #1058.

---

## 1. What #1058 changed (recap)

48 empty `Family` (EN) cells in `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` (col idx 20) were **derived** from the nearest FR-carrying ancestor by walking the `path` column. Distribution: 42 Influence, 4 Cheating, 2 Misleading language. The other 7 language family columns were already 100% — **EN was the only hole**. No other column moved (byte-exact: 48 cells, all col 20, 104 columns, BOM+CRLF+multi-line cells preserved).

The downstream question: which artefacts read col 20 of that source CSV?

---

## 2. Method (triple grounding, SDDD)

- **Technical (code = truth):** targeted grep + reads of every consumer of `Family` / `Famille` under `Generation/Converters/Argumentum.AssetConverter/` (entities, mindmap, ontology, harvest, localization config). Plus a `sha256sum` diff of source vs the `Data/Mindmap/` data fork.
- **Conversational:** ai-01 dispatch `ez05p8` (tertiary): the working list named "mindmaps EN, OWL".
- **Semantic:** `codebase_search` over the workspace (surfaced the CardPen template JSONs that carry the `{{Family}}` placeholder — confirmed the PNG path).

ai-01's explicit bound — *"cherche, ne te fie pas à ma liste"* — is why the mindmap and OWL were re-derived from code rather than asserted from the dispatch.

---

## 3. The three consumption paths (with proof)

### Path A — CardPen harvest → card PNGs → PDFs  ✅ IMPACTED

This is the **#216 localization mechanism**. The Fallacies CardPen templates carry the placeholder `{{Famille}}` (FR native, per the comment at `AssetConverterConfig.cs:98`). For non-FR languages, `CardSetLocalization` rewrites the placeholder **before** PapaParse injects the row:

- `AssetConverterConfig.cs:120` (FrontFieldConversions): `("Famille", … ("en", "Family") …)` → `{{Famille}}` becomes `{{Family}}` for EN.
- `AssetConverterConfig.cs:133` (BackFieldConversions): same, for Memo Back.

PapaParse then resolves `{{Family}}` against the **CSV column `Family`** (EN col 20). Critically, PapaParse is JS inside CardPen and parses the **raw CSV string** injected by `cardSetDocument.csv = csvContent` (the Golden-Master path, `HarvestManager`). It does **not** depend on the C# `Fallacy` entity. The CSV fed to CardPen comes from the DataSet **named** `FallaciesTaxonomy` (`HarvestManager.cs:347`: `AssetConverterConfig.DataSets.FirstOrDefault(ds => ds.Name == cardSetInfo.DataSet)`), which points at the **source** `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` — i.e. the very file #1058 edited.

**Consequence:** the 48 EN cards whose family cell was empty now render the derived EN family (Influence / Cheating / Misleading language). **The PNG pixels change; therefore the PDFs that composite those PNGs change.**

CardSets affected (those carrying `{{Famille}}`, per `AssetConverterConfig.cs:98,111,126`):
- Fallacies Face (1/2/3), Fallacies Web, Fallacies Web Thumbnails, Memo Face + Memo Back.

→ EN PDFs that composite them: `Argumentum_TarotCards_en.pdf`, `Argumentum_Fallacies_Web_A4_en.pdf`, `Argumentum_Fallacies_Web_A0_en.pdf`, `Argumentum_Fallacies_Web_Thumbnails_A4_en.pdf`, `Argumentum_TarotCards_Print&Play_A4_en.pdf` (Memo ships inside TarotCards).

### Path B — Fallacies mindmap EN  ❌ NOT IMPACTED (double isolation)

Two independent reasons, **both pre-existing** (i.e. true before #1058, not caused by it):

1. **Stale data fork.** The mindmap production config does **not** read the source CSV. `FallacyDocumentConfigBase.cs:10`:
   ```csharp
   public string DataSet { get; set; } = @"..\..\..\Data\Mindmap\Argumentum Fallacies - Taxonomy.csv";
   ```
   `FallacyMindMapDocumentConfig : FallacyDocumentConfigBase` (`FallacyMindMapDocumentConfig.cs:30`) **does not override** `DataSet`, and `ParallelDocumentCreatorConfigBase.cs:50` consumes it (`var targetDataset = mindMap.DataSet;`). The fork is **stale** vs the source — measured:
   - source `Cards/Fallacies/…` sha256 `b91d1932a3…`
   - fork `Data/Mindmap/…`    sha256 `c8fc433dca…`
   - fork mtime `Jun 1`  (2+ months **before** #1058's `ded70c81`, 2026-08-11).
   The fork also carries `.REMOVED.git-id` sidecars (retired from git tracking by the #415 git-reduction) — it is a **local-only** file, not a tracked artefact.

2. **Read-only FR alias.** Even with a fresh fork, the EN column is never loaded into the entity. `Fallacy.cs:15`:
   ```csharp
   public string Family => Famille;   // expression-bodied, NO setter
   ```
   The `MindMapLocalization` StaticConversion (`AssetConverterConfig.cs:238`) rewrites the expression `{item.Famille}` → `{item.Family}` for EN, then `Interpolate` resolves `item.Family` by reflection — which returns `Famille` (FR), because the property has no setter and the CSV map at `Fallacy.cs:231` (`Map(m => m.Family).Name("Family").Optional()`) cannot write a read-only property. The sibling languages do **not** have this problem: `FamilyRu/Pt/Es/Ar/Fa/Zh` (`Fallacy.cs:93-154`) are real settable properties, so the RU/PT/ES/AR/FA/ZH mindmaps *do* show a localized family. **EN is the lone exception** — its mindmap family label is FR-frozen. (See the `item.Family` CSS-class use at `FallacyMindMapDocumentConfig.cs:1507` — same alias, same FR resolution; harmless because the CSS colour classes are keyed on the FR family name anyway.)

**Conclusion:** for #1058 to affect the mindmap EN, two gated (gel) changes are required — (a) re-sync the `Data/Mindmap/` fork from the source, and (b) add a settable `FamilyEn` property to `Fallacy.cs` + remap `Family` (or point the StaticConversion at `FamilyEn`). Both are **code/data changes, post-tag** — explicitly out of gel scope. The mindmap EN therefore **does not need regen for #1058**.

> Note for ai-01: this is a genuine latent gap worth a post-tag issue — *« le mindmap Fallacies EN affiche les familles en FR, contrairement aux 6 autres langues localisées »*. It predates #1058 and is orthogonal to it; #1058 merely made it visible by completing the EN column that nothing currently reads on the mindmap path.

### Path C — OWL ontology  ❌ NOT IMPACTED

`OwlGeneratorConfig.cs` consumes `fallacy.TextEn` / `TextFr` for concept IDs and `skos:prefLabel` (lines 128, 208, 311, 317-318). The concept hierarchy is built from `path`, not from `Family`. A grep of `Family|Famille` over the entire `Ontology/` folder returned **no matches**. The OWL adapter (`OwlAdapter.cs`) is fully generic (RDFResource + literals) and carries no family knowledge.

**Conclusion:** #1058 has zero effect on `docs/ontology/argumentum.owl`. No OWL regen needed for #1058.

---

## 4. Regen runbook for po-2023 (the actionable list)

Scope: **EN Fallacies card images + the EN PDFs that composite them.** Per the release-dossier §3 inventory, the 8 EN PDF types are the target set (only the Fallacies-bearing ones change; Virtues/Scenarii/Rules EN are unaffected but will be regenerated together in a normal full-EN run anyway).

**Before regen:**
- Confirm master is at `ded70c81` (or later) — the source CSV with the 48 filled EN cells must be the one the pipeline reads.
- ⚠️ **Clobber the cached EN harvests** (`Target/en/Harvest/*.harvest.json`) — per the [[regen-success-without-clobber-is-stale-trap]] lesson, an exit-0 regen with an identical image count is **not** proof of freshness; `ImageHelper` skips images that already exist. Forcing the harvest to re-run is the only proof the 48 cards are re-rendered.

**Regen mode:** the normal full-pipeline EN run (Debug params for the visual-QA cycle, then Release params for the shippable CMYK PDFs — see `CLAUDE.md` Debug/Release table). No new flag, no new config — #1058 is a data change, the existing pipeline picks it up.

**Post-regen proof (po-2023 reports back):**
- 48 EN card PNGs whose family cell was empty now show the derived EN family. A spot-check of ≥1 Influence / 1 Cheating / 1 Misleading-language card against the pre-regen PNG is the falsifiable signal.
- EN PDF page counts unchanged (the 48 cards already existed; only their family text changed — no layout shift expected).

**Visual QA verdict:** ai-01 only (Playwright + vision), never a worker. The release-dossier §4 gate already requires ≥1 PDF per language; #1058 raises the stakes on the **EN Fallacies** spot-check specifically.

---

## 5. Out of scope (measured, not assumed)

- **DNN 2sxc export.** `Dnn2sxcConfig.cs:24` reads its own fork `Data/Dnn2sxc/Argumentum Fallacies - Taxonomy.csv` and produces `2sxcContentExport_…xml`. The DNN export is **partial / keys excluded**, and issues #131/#132 are still open — DNN is not in the v2.0.0 cards-release scope. Noted for completeness; no regen action for the tag.
- **TaxonomyValidationTests / TranslationCoverageReport.** These read the source CSV for diagnostics and test assertions; #1058 *improves* them (EN family coverage went 1360/1408 → 1408/1408, already exercised in #1058's DoD). They are **not** artefacts to regenerate.
- **DatasetUpdater.** Writes the CSV (the inverse direction — it *produces* translations). It does not consume `family_en` as a rendered artefact.

---

## 6. Scope / gel

- **0 artefact regen · 0 CSV mutation · 0 code change · 0 mindmap wrapper touch · 0 dependabot** (~46 open).
- 1 net-new doc, this file. Branch + PR, no direct push to master.
- Heavy regen stays po-2023's lane; visual QA verdict stays ai-01's lane.

## Sources

- `Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs:98,111,120,126,133,228-239` (FrontFieldConversions / BackFieldConversions / MindMapLocalization StaticConversions)
- `Generation/Converters/Argumentum.AssetConverter/Entities/Fallacy.cs:15,93-154,231` (Family alias RO + FamilyRu..Zh setters + CSV map)
- `Generation/Converters/Argumentum.AssetConverter/Mindmapper/FallacyMindMapDocumentConfig.cs:30,68,73,1192-1194,1507` (class decl, FamilleExpression, localization comment, familyclass)
- `Generation/Converters/Argumentum.AssetConverter/FallacyDocumentConfigBase.cs:10` (DataSet = stale fork)
- `Generation/Converters/Argumentum.AssetConverter/ParallelDocumentCreatorConfigBase.cs:50` (mindMap.DataSet consumed)
- `Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Localization/DocumentLocalization.cs:37-54` (DoReflectionTranslate)
- `Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs:342-347` (PNG path reads source DataSet)
- `Generation/Converters/Argumentum.AssetConverter/Ontology/OwlGeneratorConfig.cs:128,208,311,317-318` (OWL uses TextEn/TextFr + path, not Family)
- `sha256sum` source vs `Data/Mindmap/` fork: `b91d1932…` vs `c8fc433d…`
- [`docs/release-dossier/README.md`](README.md) §3-§4 (release gate inventory + checklist convention)

🤖 po-2024 — ai-01 dispatch `ez05p8` (tertiaire), base `ded70c81`, docs-only
