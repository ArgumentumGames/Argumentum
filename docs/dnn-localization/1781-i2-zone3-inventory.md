# #1781 I2 — zone-3 2sxc inventory: what the live portal's visible text is made of (measured)

**Issue:** [#1781 — site multilingue](https://github.com/ArgumentumGames/Argumentum/issues/1781) (grain I2)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-10-09
**Base:** master `c303fc23`
**Method:** read-only SQL against the préprod DB (`ArgumentumGames`, Method B — SELECT only, zero
mutation, zero network). **Zero personal data exported**: this document carries attribute names,
counts and classifications — no cell values leave the DB except the two named public strings
already visible on the site.

This closes the hole the #457 scope-map left open: content-types **D** (2sxc content items) and
**E** were marked *"unknown (the bulk)"*. They are no longer unknown — the bulk is **one content
type** (app 60 `Fallacy`, 1 013 entities × 3 prose fields), and it is **already translated in the
DB, in the wrong zone**.

---

## 1. The schema family changed name — and the T1 correction that follows

2sxc v21 renamed its storage: the `ToSIC_EAV_*` tables are gone (only `ToSIC_EAV_Attachments`
survives); the EAV store now lives in **`TsDynData*`** (`TsDynDataApp`, `TsDynDataContentType`,
`TsDynDataAttribute`, `TsDynDataEntity`, `TsDynDataValue`, `TsDynDataValueDimension`,
`TsDynDataDimension`, `TsDynDataZone`). Any doc or tool that names `ToSIC_EAV_Dimensions` queries
a table that no longer exists.

The **dimension axis, measured** (`SELECT * FROM TsDynDataDimension`), is zone-scoped — this is
the point T1 got wrong by trusting the export's manifest:

| Zone | Name | Dimensions |
|-----:|------|------------|
| 1 | Default | 1 = Culture Root |
| 2 | Argumentum Games (**Portal 0**, the dead one) | 2 = root, **3 = en-US**, **4 = fr-FR** |
| **3** | **Argumentum (Portal 1, the live one)** | 5 = root, **6 = fr-FR**, **7 = en-US** |

⇒ **T1's `CULTURES` table carries zone-2 dimension IDs for a zone-3 app.** The committed export's
manifest (`manifest.json: dimensions`) declares `frFR_dimensionId: 4` / `enUS_dimensionId: 3`
while *also* declaring `argumentumAppZoneId: 3` — internally contradictory, and the pipeline
copied it faithfully. The **culture codes** (`fr-FR`, `en-US`) are right and are what the import
XML actually carries, so nothing shipped wrong; the dimension IDs were documentation-tier wrong.
Corrected in this PR: `fr→6`, `en→7`, provenance `TsDynDataDimension` measured 2026-10-09.

Also measured: **the six other target cultures (ru/pt/es/ar/fa/zh) are not provisioned anywhere**
— 7 dimension rows total, all listed above. This upgrades #682 Path A from "reported" to
"measured": I3 in anything beyond EN requires provisioning dimensions first.

## 2. Which 2sxc apps actually render on public pages

65 live 2sxc module instances (2 desktop modules: `2sxc` Content / `2sxc-app` App), spread over
public pages, admin pages and per-user pages. Resolving each instance's app binding
(`ModuleSettings.TsDynDataApp` / `TsDynDataContentGroup` → app) and the page's anonymous VIEW
permission:

| App | Folder | Bound modules (public pages) | Carries |
|----:|--------|------------------------------|---------|
| **33** | `Content` | 575 + 583 (home), 587 « Retrouvez nous » (all pages), 593 (Téléchargements), 603 (Règles intro), 604 (Amis), 637 (Argumentation) | homepage prose, link blocks, contact/location |
| **35** | `Accordion4` | 578 (home), 600 « Contactez-nous » (all pages) | accordion titles |
| **52** | `News5` | 588 (Actus), 590 (home) | news items + labels |
| **60** | `Argumentum` | 602 (Règles — RulesExplorer) | **the game content** |
| 47 | `QrCode2` | 605 — page **QRCodes is not anonymously viewable** (nor is its parent Tests) → placed, not public | excluded from I3 scope |

The other ~22 zone-3 apps (34–61) are installed but **not placed on any page** — no module
binding exists for them. They are stock 2sxc apps whose entities are seed/demo data; not
translation surface.

## 3. The translatable surface, per app (attribute-level)

Classification: **prose** (human-visible sentences), **label** (short UI string), **tech**
(enum-like/color/filename — String-typed but not translatable), **data** (Number/DateTime/
Boolean/Entity/Hyperlink). Counts are stored values (`TsDynDataValue`), i.e. cells that exist —
absent attributes store nothing (same convention as the v21 export).

### App 60 `Argumentum` — 3 132 values, of which 3 107 String/Hyperlink

| Content type | Entities | Attribute | Type | Class | Cells |
|--------------|---------:|-----------|------|-------|------:|
| **Fallacy** | **1 013** | Name | String (isTitle) | prose | **1 013** |
| | | Description | String | prose | **1 013** |
| | | Example | String | prose | **1 013** |
| | | Difficulty/Card/Date | Number/Bool/DateTime | data | 9 |
| Game Rule | 5 | Title, Summary, Material, Installation, Content, Variants, Memo | String | prose | 32 (28 non-empty — the #684 artifact) |
| | | MinNbPlayers, MaxNbPlayers, Date, UrlKey | Number/DateTime/String | data | 20 |
| App-Resources | 2 | 11 × `res.Rule*`/Licence/Author | String | label (2 tech: `RuleMemoCardFileNamePrefix`, URLs) | 11 |
| Album / License / Contributor / ImageMetadata / AlbumPresentation | 8 | titles, introductions, names | String | prose/label | ~15 |
| Scenario, Scenario Category, Comment, Argumentum Settings | 0 | — | — | — | 0 |

### App 33 `Content` — 461 values (307 String/Hyperlink)

| Content type | Entities | Translatable attributes | Class | Cells |
|--------------|---------:|------------------------|-------|------:|
| Content | 31 | Title, Text, ImageCaption | prose | 87 |
| Link | 8 | Title, Description, LinkText | prose | 24 |
| Video | 8 | Title, Text | prose | 15 |
| Person | 1 | FullName, Position, Description | prose (public contact block — see §5) | 3 |
| Location | 2 | Company, Description, Street, ZipCode, City, Country, Tel | prose (public contact block) | ~14 |
| App-Resources | 1 | MapsLabelDirections | label | 1 |
| PresSetText / *ViewSettings / PresSet* | 82 | HeadingType, alignments, columns | **tech** | 0 |

### App 52 `News5` — 102 values (64 String)

| Content type | Entities | Translatable attributes | Class | Cells |
|--------------|---------:|------------------------|-------|------:|
| News | 9 (5 published titles) | Title, Teaser, Content | prose | 15 |
| Category | 6 | Name, PageTitle | label | 10 |
| App-Resources | 2 | LabelReadMore, LabelBackToList, LabelCategoryAll, LabelEventNotExists(+Text), LabelShowFrom/ToPill, 2 × LabelAdmin* | label | 10 |

### App 35 `Accordion4` — 62 values (43 String)

| Content type | Entities | Translatable attributes | Class | Cells |
|--------------|---------:|------------------------|-------|------:|
| Accordion | 19 | Title | prose | 19 |
| App-Resources | 2 | DemoItemMessage | label | 1 |

**Total translation surface ≈ 3 270 cells** — of which **3 039 (93 %) are the Fallacy triplet**.

## 4. The two headline findings

**(a) Zone 3 carries ZERO translated values.** `TsDynDataValueDimension` has no row for
dimensions 6 or 7 — every value in the live portal is the dimensionless default (= FR). Whatever
I3 injects, it injects from scratch; there is nothing to merge against.

**(b) The English translation of the entire Fallacy explorer already exists — in zone 2.** The
dead portal-0 copy of the same app (app 29, **same app GUID** `bbc87873-…` as app 60) carries:

- **3 046 en-US values: exactly Fallacy Name × 1 013 + Description × 1 013 + Example × 1 013**
  (+ 7 technical), dimension 3;
- 2 027 fr-FR values (dimension 4);
- **1 126 of app 60's 1 128 entities exist in app 29 by identical EntityGuid** — the mapping to
  reinject on is the GUID, not the name.

⚠️ **Reuse only after diffing, not by trust**: zone 2's Fallacy list holds **3 037 entities vs
zone 3's 1 013** — a different, larger cut (likely an older corpus state). The EN cells attach to
entities by GUID, so the diff is mechanical: for each of the 1 013 shared Fallacy GUIDs, compare
zone-2 EN against the zone-3 FR it would sit beside; whatever diverges beyond translation drift
re-enters the normal translation lane. This turns I3's biggest block (3 039 cells) from
"translate" into "verify + inject" — a measurement task, not an API spend.

## 5. Personal data — none in scope

- No form app is placed on any page: the « Contactez-nous » module (600, on every page) is bound
  to **app 35 (Accordion4)** — there are no form-submission content types anywhere in zone 3.
  `MobiusForms5` exists only as an unbound stock folder.
- `Person` (1 entity) and `Location` (2 entities) in app 33 are the association's **public
  contact block** (already rendered on the site). They are excluded from any bulk translation
  export by default; if I3 wants them translated, they are 3 entities to handle by hand.
- This inventory itself exports **counts and attribute names only**.

## 6. What I3 needs from here (pivot keys)

1. **Export key = EntityGuid**, never Name (names are not unique across the corpus and the label
   splits across `tspan` in some views). Zone 2 → zone 3 join measured at 1 126/1 128.
2. **Injection dialect**: one `<Entity>` per (Guid, culture) — the T1 XML shape, whose culture
   codes are confirmed valid in zone 3 (`fr-FR`, `en-US` both Active). The six other cultures
   need `TsDynDataDimension` provisioning first (#682 Path A).
3. **One entity first**: the T1 fixture (`one-entity-game-rule-import.xml`) remains the right
   first payload; the live proof is still the open unknown T1 declared.
4. **Do not translate what is tech**: `HeadingType` (45 cells), colors, filename prefixes,
   UrlKeys are String-typed but must ride the `fr` column unchanged.

## 7. Gates

- ❌ No DB write, no portal mutation, no API call, zero personal data.
- ❌ No QA verdict (ai-01 only). The T1 `CULTURES` correction is documentation-tier: culture
  codes unchanged, fixture byte-identical (IC11 still passes), self-test re-run green.

## Sources

- Préprod DB `ArgumentumGames` (read-only): `TsDynDataZone/App/Dimension/ContentType/Attribute/
  Entity/Value/ValueDimension`, `Modules/TabModules/Tabs/TabPermission/ModuleSettings`,
  measured 2026-10-09.
- `docs/dnn-localization/457-t1-site-content-pipeline.md` §5 (the attested-vs-unconfirmed table
  this corrects), `release-validation/exports/DNN-Argumentum-export-2026-07-07/manifest.json`
  (the source of the zone-2 IDs), `457-site-content-type-inventory.md` (the "unknown (the
  bulk)" hole this fills), `684-translations.json` (28-cell artifact, unaffected).
