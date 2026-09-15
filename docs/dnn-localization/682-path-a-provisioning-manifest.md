# 2026-07-11 — #682 Path A provisioning manifest (49 suffixed fields, gated ops)

**Scope**: the **concrete provisioning list** jsboige/ops applies on the live DNN instance to make the
Game Rule content-type (app=60, AttributeSet 377) **Path-A multilingual** — i.e. create the **49
lang-suffixed attributes** (7 translatable fields × 7 non-FR langs) so PR #674's `Loc()` cascade can
resolve them at runtime. This is the **ops-gated artifact**; no DB write from this repo.

**Repo reference**: branch `feat/682-path-a-provisioning-manifest`, base master `7406bb8e`. Issue: #682.
Owner: jsboige (DB provisioning). Triggered by ai-01 dispatch `bzkvnh` (2026-07-11, primary).

> ⚠ **Status = manifest only.** No DB write is executed from this repo. The 49 attribute creations
> below are **staged for prod apply** (gated jsboige/ops) on the live DNN Argumentum instance (app=60).

---

## Ground truth (from the live 2sxc v21 export, #774 / #681)

Source: [`exports/DNN-Argumentum-export-2026-07-07/11-game-rule-schema.json`](release-validation/exports/DNN-Argumentum-export-2026-07-07/11-game-rule-schema.json).

- **Content-type**: `Game Rule`, AttributeSet **id=377**, app **60** (ZoneID=3), StaticName `a9d3420a-3644-4be1-8df1-12c27ced8c19`.
- **Schema family**: `ToSIC_EAV_*` (2sxc **v21**) — NOT `ToSIC_SexyContent_*`.
- **15 attributes** total. **7 translatable String fields** (the provisioning scope):

| # | StaticName | AttributeID | Type | IsTitle |
|---|------------|-------------|------|---------|
| 1 | `Title` | 1933 | String | **yes** |
| 2 | `Summary` | 1934 | String | no |
| 3 | `Material` | 1935 | String | no |
| 4 | `Installation` | 1938 | String | no |
| 5 | `Content` | 1939 | String | no |
| 6 | `Variants` | 1940 | String | no |
| 7 | `Memo` | 2113 | String | no |

- **8 non-localizable attrs** (out of scope): `Parent` (Entity), `MinNbPlayers`/`MaxNbPlayers` (Number), `Date` (DateTime), `Author`/`Licence`/`Original` (Entity), `UrlKey` (SEO slug).

---

## Path A — the 49 suffixed fields to provision

Decision (confirmed by [`682-field-model-revision-2sxc21.md`](682-field-model-revision-2sxc21.md) §4):
**Path A** = lang-suffixed attributes + `Loc()` cascade (PR #674). FR stays canonical (unsuffixed). The
7 target non-FR langs (matching the `dnn-ui-strings.csv` / DatasetUpdater target set):

`en` · `ru` · `pt` · `es` · `ar` · `fa` · `zh`

### The 49 attributes (7 fields × 7 langs)

Each new attribute = `<Field>_<lang>`, type **String (Wysiwyg/rich-HTML)** matching its generic
counterpart (confirmed String, §1 of the revision doc), added to AttributeSet 377.

| Field (generic) | en | ru | pt | es | ar | fa | zh |
|-----------------|----|----|----|----|----|----|-----|
| `Title` (1933) | `Title_en` | `Title_ru` | `Title_pt` | `Title_es` | `Title_ar` | `Title_fa` | `Title_zh` |
| `Summary` (1934) | `Summary_en` | `Summary_ru` | `Summary_pt` | `Summary_es` | `Summary_ar` | `Summary_fa` | `Summary_zh` |
| `Material` (1935) | `Material_en` | `Material_ru` | `Material_pt` | `Material_es` | `Material_ar` | `Material_fa` | `Material_zh` |
| `Installation` (1938) | `Installation_en` | `Installation_ru` | `Installation_pt` | `Installation_es` | `Installation_ar` | `Installation_fa` | `Installation_zh` |
| `Content` (1939) | `Content_en` | `Content_ru` | `Content_pt` | `Content_es` | `Content_ar` | `Content_fa` | `Content_zh` |
| `Variants` (1940) | `Variants_en` | `Variants_ru` | `Variants_pt` | `Variants_es` | `Variants_ar` | `Variants_fa` | `Variants_zh` |
| `Memo` (2113) | `Memo_en` | `Memo_ru` | `Memo_pt` | `Memo_es` | `Memo_ar` | `Memo_fa` | `Memo_zh` |

**= 49 new attributes.** Naming convention follows `Loc()` expectations (PR #674 `<Field>_<lang>`
resolution — verify the exact suffix spelling `_en`/`_ru`/… matches the `Loc()` implementation before
provisioning; if #674 uses `_en-US`/`_ru-RU` culture names, align accordingly).

> **Memo caveat**: `Memo` (AttributeID 2113) has a non-contiguous ID (vs 1932-1941) — it was added
> later. Confirm it is on AttributeSet 377 (not a sibling set) before provisioning its 7 suffixes.

---

## Apply procedure (jsboige / ops, gated)

### Path A-1 — 2sxc admin UI (lowest risk, visual)
1. Log into DNN as host/admin → 2sxc app **60** (Argumentum).
2. Content-type `Game Rule` (AttributeSet 377) → **manage fields**.
3. For each of the 49 rows above: **add field** → name `<Field>_<lang>`, type **String** (Wysiwyg to
   match the rich-HTML generic counterpart), group `Default`. Save.
4. Verify the 49 fields appear + PR #674's `Loc()` cascade resolves them per request culture.

### Path A-2 — Method B write script (on-box, gated)
A SQL write against the live DNN DB via 2sxc v21 EAV tables (`ToSIC_EAV_Attributes` +
`ToSIC_EAV_AttributesInSet`). **Requires write credentials** (the export was read-only SELECT). The
2sxc admin UI (Path A-1) is strongly preferred — direct EAV inserts risk metadata inconsistencies
(SortOrder, AttributeID assignment, AppId/AssignmentType). A SQL path is feasible but **must be
validated against the live `ToSIC_EAV_*` v21 schema** before execution; the script below is a
**draft skeleton**, not apply-ready.

```sql
-- DRAFT SKELETON — validate table/column names vs live ToSIC_EAV_* v21 schema before executing.
-- Adds the 49 suffixed attributes to AttributeSet 377 (Game Rule, app=60).
-- 2sxc v21 stores attribute metadata across ToSIC_EAV_Attributes + ToSIC_EAV_AttributesInSet.

DECLARE @AttributeSetId INT = 377;
DECLARE @AppId INT = 60;

-- Repeat for each (Field, lang) pair in the 49-grid above.
-- Example: Title_en
INSERT INTO ToSIC_EAV_Attributes (AttributeSetId, StaticName, Type, IsTitle, AppId)
VALUES (@AttributeSetId, 'Title_en', 'String', 0, @AppId);
-- + corresponding ToSIC_EAV_AttributesInSet row (AttributeId, AttributeSetId, SortOrder, GroupId)
-- + dimension config if the App uses dimensioned values (Path A uses suffixed attrs, so NOT dimensioned)
```

> ⚠ **Do NOT run the SQL skeleton as-is.** The column names (`AttributeSetId`, `StaticName`, `Type`,
> `IsTitle`, `AppId`) and the `ToSIC_EAV_AttributesInSet` companion insert are inferred from the v21
> EAV model and **must be confirmed against the live schema** (cf the export `13-app60-resources.json`
> raw shape + a `sp_columns ToSIC_EAV_Attributes` probe). Path A-1 (2sxc admin UI) avoids this entirely.

### Post-provisioning verification
1. Re-export AttributeSet 377 (Method B read-only) → confirm the 15 → 64 attribute count (15 + 49).
2. PR #674 becomes **runtime-validable**: the `Loc()` cascade should resolve `<Field>_<lang>` per
   request culture. Smoke-test on `/Règles` detail page switching cultures.
3. **#684** (Game Rule prose translation, PR #767 already merged) then populates the 49 fields with the
   7-lang translated values from `docs/dnn-localization/684-translations.json` (re-import gated).

---

## Gate boundaries (HARD)

- ❌ **Zero DB write from this repo** — the manifest is a staging artifact; apply is gated jsboige/ops.
- ❌ Does not provision the suffixed fields unilaterally — jsboige ratifies + executes (decision confirmed).
- ❌ Does not modify `Cards/` CSV (game-content is po-2024's lane).
- ❌ Does not declare a QA verdict — that's ai-01.
- ❌ Does not merge #674 (runtime-pending, gated sandbox — #596 garde-fou).

## What this PR also ships (sibling doc correction)

This branch also corrects the **2sxc 15.02 → v21 staleness** in
[`release-validation/2sxc-export-spec.md`](release-validation/2sxc-export-spec.md) (§3/§4/§7) — the
spec was written for 2sxc 15.02 (`ToSIC_SexyContent_*`, app=31, "FR inféré"), now superseded by the
live v21 export (`ToSIC_EAV_*`, app=60, FR verified). See the SUPERSEDED banner + corrected sections.

Relates: dispatch `bzkvnh`, #682 (this issue), #774/#681 (live export), #694 (original decision-input),
#674 (RulesExplorer `Loc()` refactor), #684 (translation, populates the 49 fields post-provisioning),
#490 (res.* rail — separate), #458 (epic).
