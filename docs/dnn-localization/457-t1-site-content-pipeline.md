# #457 T1 — the site-content pipeline (2sxc export ⇄ CSV ⇄ import XML)

**Status: T1 first brick delivered — the pipeline and its offline proof.**
The **live** import proof (does 2sxc v21 accept this XML with dimensions?) is **I3** and stays
jsboige-gated. Nothing in this brick writes to DNN, and nothing in it calls a paid API.

Owner decision 2026-10-08 (dispatch `ai01-20261008-i18n-po2024`, plan
[#457 c.6056530803][plan], grains [#458 c.6056545021][grains]):
> *"Démarre T1 sur l'export app 60 (#774), avec une preuve sur une entité avant le volume."*

---

## 1. What T1 had to produce, and what already existed

The dispatched chain is four bricks. Two existed before this work; two did not.

| # | Brick | State before | State now |
|---|-------|--------------|-----------|
| 1 | 2sxc export → JSON | ✅ committed (#681/#774) | unchanged |
| 2 | JSON → translations | ✅ `translate_game_rules.py` → `684-translations.json`, DoD-verified | unchanged |
| 3 | JSON → **CSV** (the pivot) | ❌ missing | 🆕 `site_content_pipeline.py to-csv` |
| 4 | CSV → **import XML with dimensions** | ❌ missing | 🆕 `site_content_pipeline.py to-xml` |

Brick 2 exists as a **standalone Python translator calling OpenAI directly**, not as the
DatasetUpdater task the plan names ("tâche DatasetUpdater « contenus du site »"). That is a
deliberate **open item**, not an oversight — see §8. This brick is built so the middle step is
**pluggable**: the CSV is the pivot, and either translator can fill its 7 target columns. No
paid call was made and none is required to prove the pipeline.

---

## 2. The v21 export shape — measured, not assumed

`release-validation/exports/DNN-Argumentum-export-2026-07-07/12-game-rule-content-items.json`
is **not** the 2sxc-15 `SexyContentData` dialect. It is a flat triple store:

```json
{ "contentType": "Game Rule", "appId": 60, "attributeSetId": 377,
  "schemaAttributes": [ { "StaticName": "Parent", "Type": "Entity", ... }, ... ],
  "entities": [ { "EntityID": 11378, "EntityGUID": "ae1edefa-…", "IsPublished": true } ],
  "values":   [ { "EntityID": 11378, "StaticName": "Title", "Value": "…",
                  "Type": "String", "Lang": null, "DimensionID": null } ] }
```

Measured 2026-10-08 on the committed file:

| Observation | Value |
|---|---|
| Game Rule entities | **5** (11378, 11380, 11387, 11388, 11389) |
| Schema attributes | **15** |
| Populated values | **52** |
| Values carrying `Lang != null` or `DimensionID != null` | **0 of 52** |
| Schema slots never emitted (`Parent`, `Author`, `Licence`, `Original` ×5, `Memo` ×3) | **23 of 75** |
| Stored **but empty** values (3 `Variants` + 1 `Memo`) | **4** |

⚠️ **Two consequences that a naive reader gets wrong.**

1. **The export carries only the dimensionless default, which the site treats as FR**
   (`manifest.json: dimensions.note`). No translated value is in the export; the 7 target
   languages come from `684-translations.json` (28 cells × 8 languages, DoD-verified:
   **196/196 tag-preserving and script-correct, 0 violations** by
   `verify_game_rule_translations.py`).
2. **"Absent" and "present but empty" are different facts** — 23 slots vs 4 cells. The v21
   export omits an unset attribute entirely; it emits a value row with `Value: ""` when the
   attribute is set but empty (`Variants` on 3 rules, `Memo` on 1). A reader that iterates the
   **schema** instead of the export's **value list** turns the 23 absent slots into 23 invented
   values. The pipeline iterates the value list, and the self-test pins both counts (control
   IC10).

---

## 3. The pipeline

`tools/dnn_i18n/site_content_pipeline.py` — stdlib only, four subcommands.

```
to-csv        export JSON (+ optional translations) -> pivot CSV
to-xml        pivot CSV -> <SexyContentData> with one <Entity> per (Guid, Language)
self-test     2 round-trips + 15 controls, offline
list-cultures the culture table with attestation status
```

**The pivot CSV** — one row per (entity, attribute) actually stored, columns
`key, app, content_type, guid, attribute, fr, en, ru, pt, es, ar, fa, zh`:

```csv
key,app,content_type,guid,attribute,fr,en,ru,pt,es,ar,fa,zh
60|Game Rule|0167188c-…|Content,60,Game Rule,0167188c-…,Content,"<h2>D&eacute;roul&eacute;…",…
```

52 rows, of which **28 carry at least one translation** (the translatable set is
`TRANSLATE_FIELDS` = Title + 6 prose fields, **imported from the sibling translator** so the two
cannot drift; control IC6). The remaining 24 rows (`MinNbPlayers`, `MaxNbPlayers`, `Date`,
`UrlKey`) carry `fr` only — they are data, not prose, and are never sent for translation.

**The import XML** — the dialect the repository already reads and writes
(`Argumentum.AssetConverter/Dnn2sxc/SexyContentData.cs`, `Entity.cs`): one `<Entity Type="Game
Rule">` per **(Guid, Language)** pair, the same Guid repeated once per language, the dimension
carried by the `<Language>` element. Measured on the one-entity payload: **8 blocks, 1 Guid,
8 distinct cultures**, 11 attributes in the `fr-FR` block (what this entity actually stores) and
7 in each target block (the translatable set).

**Emission rule — uniform minimal.** An attribute element is emitted only where a value is
actually carried, in *every* language including the source. No empty element is ever written,
so the import can never be read as *"clear this attribute"* (control IC9). `--uniform-attributes`
switches to the legacy dialect shape (every schema attribute in every block, empty ones as
`<Attr />`): 600 slots vs 244 (control IC8).

---

## 4. Round-trip proof and controls

`self-test` — **17 checks, all named**, zero network, zero prod write, runnable as step `[5/5]`
of the existing `test_roundtrip.py` runner.

**Two round-trip axes** (both byte-exact):

* **RT1 — source axis**: export → CSV → reconstructed v21 `values`. 52/52 identical, including
  HTML entities, embedded newlines and the non-Latin scripts.
* **RT2 — dimension axis**: CSV → XML → parsed back into `(guid, attribute, culture) → value`.
  244/244 cells identical.

**Fifteen controls**, each aimed at a failure this pipeline could actually have:

| Control | What it pins |
|---|---|
| IC1 | a mutated FR cell is **detected** by RT1 (the round-trip is not vacuous) |
| IC2 | a dropped XML cell diverges (RT2 is not vacuous) |
| IC3 | the artifact's spurious `_meta` / `entities` keys do **not** become entities |
| IC4 | unconfirmed cultures are **refused** by default (fail-closed) |
| IC5 | a value for an out-of-schema attribute is refused |
| IC6 | `TRANSLATE_FIELDS` really comes from the sibling tool |
| IC7 | 8 distinct culture codes are emitted (the axis is not collapsed) |
| IC8 | `--uniform-attributes` emits all 600 slots; minimal emits 244 |
| IC9 | minimal mode emits **zero** empty elements |
| IC10 | present-but-empty (4) is distinct from absent (23) |
| IC11 | the committed fixture **is exactly** what the tool regenerates from committed inputs |

**Contre-épreuve** (mutation of the corpus, restoration by `cp`, **never** `git checkout --`),
run against the C# interop suite: mutating the `ru-RU` title to Latin and corrupting one `<Guid>`
produced **2 named reds**, exactly the two predicted tests. IC11 was proven to bite separately
(one accent removed from the fixture → `177155` vs `177156` bytes → red). Restoration verified by
`sha256` (`789bb0a762bcdd24…`).

⚠️ IC11's first version was **near-vacuous** and the contre-épreuve is what exposed it: a
script check written as *"contains at least one Cyrillic character"* survives any single-character
mutation by construction. It was re-calibrated to the sibling verifier's rule (≥3 characters **and**
ratio > 0.1) — see §6 for the declared scope of that gate.

---

## 5. The dimension axis — attested vs unconfirmed

The import dialect encodes a language as a **culture code** on a repeated Guid. Measured on the
live DB (I2, 2026-10-09, `TsDynDataDimension` — dimensions are **zone-scoped** and app 60 lives
in **zone 3**):

| lang | culture | dimensionId | status |
|------|---------|------------:|--------|
| fr | `fr-FR` | **6** | ✅ measured — `TsDynDataDimension` zone 3 |
| en | `en-US` | **7** | ✅ measured — `TsDynDataDimension` zone 3 |

⚠️ **Corrected 2026-10-09** — this table first said 4/3, copied from the export's
`manifest.json: dimensions`. Those are **zone 2's** IDs (the dead portal 0), while the same
manifest declares `argumentumAppZoneId: 3` — it contradicts itself, and the correction is
measured in [`1781-i2-zone3-inventory.md`](1781-i2-zone3-inventory.md) §1. Only the culture
code is ever emitted into the XML, so nothing shipped wrong; the IDs are documentation.
| ru | `ru-RU` | — | ⚠ **unconfirmed** |
| pt | `pt-PT` | — | ⚠ **unconfirmed** |
| es | `es-ES` | — | ⚠ **unconfirmed** |
| ar | `ar-SA` | — | ⚠ **unconfirmed** |
| fa | `fa-IR` | — | ⚠ **unconfirmed** |
| zh | `zh-CN` | — | ⚠ **unconfirmed** |

The six unconfirmed cultures are **not attested anywhere in this repository** — their 2sxc
dimensions are not provisioned (#682 Path A, recorded in `684-translation-run-report.md` lines
70-71 and 102-103). The candidate codes above are **proposals**, not measurements, and the
provenance field says so.

⇒ `to-xml` **refuses** to emit them unless `--unconfirmed-ok` is passed. This is the point: a
file whose culture codes were invented must not ship silently. Confirming them is a live step,
one query:

```sql
SELECT DimensionID, ZoneId, Name, CultureCode FROM TsDynDataDimension
```

⚠️ The existing report says *"the other 5 langs"* then lists **six** codes (ru/pt/es/ar/fa/zh).
The list is right and the count is a slip; recorded here so the next reader does not re-derive it.

---

## 6. What the C# interop test adds — and the loss it measures

`Argumentum.AssetConverter.Tests/SiteContentImportXmlInteropTests.cs` (4 facts) puts the
pipeline's output through the **repository's own 2sxc reader** (`XmlSerializer` over
`SexyContentData` / `Entity`, the one `Dnn2sxcConfig.Apply()` uses): 8 blocks parsed, one Guid,
8 cultures, `Type="Game Rule"`, no XML namespaces, and the translated titles present in their own
scripts (Cyrillic / Arabic / CJK) — calibrated to the same barème as
`verify_game_rule_translations.py check_script()`.

Its second fact is the one that **justifies the design**: `Entity.cs` is hard-wired to the
*Fallacy* attribute set, and `XmlSerializer` silently ignores elements it does not map. Measured
by counting `UnknownNode` events — **10 of the 15 Game Rule attributes are dropped**
(Title, Summary, Material, MinNbPlayers, MaxNbPlayers, Installation, Content, Variants, Memo,
UrlKey), while the 5 in the intersection survive (Parent, Author, Original, Licence, Date).
Writing the import through that class would have produced an amputated XML **with no warning at
all**. That is why the pipeline is schema-driven.

**Declared scope of the script gate**: it is a **contamination detector** (the model answered in
Latin in a non-Latin cell — the #216 failure mode), not a fidelity detector. A ratio-based gate
is insensitive to single-character corruption *by construction*; cell-level fidelity is carried
by RT1/RT2, which are byte-exact. Stated in the test itself so the next reader does not mistake
it for a stronger instrument than it is.

---

## 7. Declared deviations

| Deviation | Why |
|---|---|
| CSV `key` uses an ASCII `\|` separator (`60\|Game Rule\|<guid>\|<attr>`), not the plan's typographic `·` | the key is a `PrimaryField` and may become a filename or a prompt token; ASCII avoids an encoding dependency. The plan's `·` reads as an enumeration of components, not a literal separator. |
| The emitted XML **omits** the unused `xmlns:xsi` / `xmlns:xsd` declarations | they are `XmlSerializer` noise in the attested file and carry no information; asserted absent by the C# test, which also proves the repo's reader accepts the result. |
| Default emission is **minimal**, not dialect-matching | non-destructive: an empty element must never be readable as *"clear this"*. The dialect shape is one flag away. |
| The `fr-FR` block represents the DB's **dimensionless** default | the legacy import XML does exactly this, so it is the attested way to say *"the source language"*. **Unverified against v21** — a question for I3. |

---

## 8. Open items (none of them blocks the next grain)

1. **I3 — the live proof.** Does 2sxc v21 accept this XML, and does it create/find the language
   dimensions? One entity, then volume. jsboige-gated (DB write). The ready payload is
   `tools/dnn_i18n/fixtures/one-entity-game-rule-import.xml`.
2. **Which translator drives the CSV?** The plan names a DatasetUpdater task ("contenus du
   site"); the lane has a working, DoD-gated standalone translator. The CSV pivot is agnostic —
   this is a decision to take, not a technical blocker.
3. **Empty-element semantics.** Does `<Attr />` in a non-default dimension clear the value or
   leave it to fall back? The minimal default sidesteps the question; `--uniform-attributes`
   raises it. To be settled on the live site.
4. **Confirm the six culture codes** (§5) — one query.
5. **CI wiring.** Neither `tools/dnn_i18n/` runner nor this self-test runs in CI today. Already
   tracked as its own pool item (*"self-test en CI, offline part"*), so this brick joins that
   grain rather than opening a second front.

---

## Sources

- `docs/dnn-localization/release-validation/exports/DNN-Argumentum-export-2026-07-07/` — the
  committed export (`12-game-rule-content-items.json`, `manifest.json`)
- `docs/dnn-localization/684-translations.json` — the DoD-verified 8-language artifact
- `docs/dnn-localization/684-translation-run-report.md` — the dimension IDs and the #682 Path A
  provisioning gap
- `Generation/Converters/Argumentum.AssetConverter/Dnn2sxc/` — `SexyContentData`, `Entity`,
  `Dnn2sxcConfig`
- `tools/dnn_i18n/site_content_pipeline.py`, `tools/dnn_i18n/README.md`,
  `Generation/Converters/Argumentum.AssetConverter.Tests/SiteContentImportXmlInteropTests.cs`

[plan]: https://github.com/ArgumentumGames/Argumentum/issues/457#issuecomment-6056530803
[grains]: https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-6056545021
