# 2026-07-09 — #682 Rule field-model REVISION (2sxc v21 export #681)

**Scope**: revision of [`682-field-model-decision-input.md`](682-field-model-decision-input.md) (PR #694, 2026-07-05) in light of the **live 2sxc v21 export** (#681, delivered 2026-07-08 by web1, read-only SQL Method B). This is an **addendum / supersede-note**, not a rewrite — the original decision (Path A, 49 suffixed fields) **stands and is confirmed**; this doc corrects the deltas that the binary scan #687 could not see.

**Repo reference**: master `b666ed6e`. Issue: #682. Triggered by ai-01 dispatch `22w5oc` (2026-07-09). Owner: jsboige (DB) + worker (analysis). **Analysis-only — no DB write, no provisioning executed.**

**Export source**: `.shared-state/attachments/DNN-Argumentum-export-2026-07-07/` (8 files), GDrive-synced. Key files consumed here: `manifest.json`, `11-game-rule-schema.json` (Game Rule content-type schema, id=377), `14-res-rule-dictionary.json` (9 res.Rule* keys).

---

## TL;DR — the decision is unchanged, 3 deltas corrected

The original Path A recommendation (49 suffixed fields, FR canonical) is **confirmed correct** by the live export. What changes:

| Delta | Original (#694, binary scan #687) | Revised (live export #681) |
|---|---|---|
| **Schema family** (spec §4) | `ToSIC_SexyContent_*` (2sxc 15.02) | **`ToSIC_EAV_*` (2sxc v21)** — spec §4 outdated |
| **app-id** | app=31 (debate/fallacies, ZoneID=2) probed | **app=60** (Argumentum game, ZoneID=3) — wrong-app files removed |
| **Attribute types** | gated on sysadmin unblocker (best-effort inference) | **resolved** — all 15 types enumerated (§1 below) |

The **7 translatable fields × 7 non-FR langs = 49 suffixed fields** count is **unchanged** (see §2 — the manifest caveat confirms 49 = res.* label resources scope, not content-type attr count).

---

## 1. Game Rule content-type — the real 15 attributes (code=truth, from `11-game-rule-schema.json`)

The binary scan #687 saw only the subset consumed by the view templates. The live DB export reveals the **full AttributeSet id=377 (15 attributes)** with exact types:

| # | StaticName | Type | IsTitle | Localizable? | In #694 list? |
|---|------------|------|---------|--------------|----------------|
| 0 | `Parent` | Entity | no | ❌ NO (structural ref) | — |
| 1 | `Title` | String | **yes** | ✅ YES | ✅ (as EntityTitle) |
| 2 | `Summary` | String | no | ✅ YES | ✅ |
| 3 | `Material` | String | no | ✅ YES | ✅ |
| 4 | `MinNbPlayers` | Number | no | ❌ NO | ✅ (non-localizable) |
| 5 | `MaxNbPlayers` | Number | no | ❌ NO | ✅ (non-localizable) |
| 6 | `Installation` | String | no | ✅ YES | ✅ |
| 7 | `Content` | String | no | ✅ YES | ✅ |
| 8 | `Variants` | String | no | ✅ YES | ✅ |
| 9 | `Memo` | String | no | ✅ YES | ✅ |
| 10 | `Date` | DateTime | no | ❌ NO (metadata) | NEW (not in #694) |
| 11 | `Author` | Entity | no | ❌ NO (ref) | NEW |
| 12 | `Licence` | Entity | no | ❌ NO (ref) | NEW |
| 13 | `Original` | Entity | no | ❌ NO (ref) | NEW |
| 14 | `UrlKey` | String | no | ❌ NO (SEO slug) | ✅ (non-localizable) |

**Translatable String fields: still exactly 7** (Title/Summary/Material/Installation/Content/Variants/Memo). The 8 additional attrs the scan missed (Parent/Date/Author/Licence/Original + the 2 Number + UrlKey) are all **non-localizable** (Entity refs, DateTime, Number, SEO slug). **The provisioning list (49 suffixed fields) does not change.**

> **Resolution of the "7→15" dispatch wording**: ai-01's dispatch `22w5oc` spoke of a "delta 7→15 attrs". That is **15 = total attrs on the content-type**, not 15 *translatable*. The translatable count stays **7**. This addendum documents the full 15 for completeness + corrects the type gate, but the provisioning scope is unchanged.

### Exact 2sxc types — GATE RESOLVED

#694 §2 listed the attribute types as "gated on sysadmin unblocker". The export resolves this: all 7 translatable fields are **`Type: String`** (rendered via `@Html.Raw` = rich HTML / Wysiwyg in the view). jsboige: when provisioning the 49 suffixed fields, create them as **String (Wysiwyg/rich-HTML)** matching their generic counterpart — confirmed, no longer inferred.

---

## 2. The "49 suffixés" clarified — res.* labels vs content-type attrs (manifest caveat)

The manifest (`manifest.json` → `caveat_fieldCount`) flags a terminology ambiguity worth nailing down, because two different "fields" are in play:

- **Content-type attributes** (§1 above): the 15 columns on the Game Rule entity. **7 are translatable** → 7 × 7 non-FR langs = **49 suffixed attributes** to provision. ← **This is the #682 provisioning scope.**
- **App-Resources `res.*` labels** (`14-res-rule-dictionary.json`): UI string keys consumed by the view templates via `@Resources.X`. Currently **9 `res.Rule*` keys** provisioned (RuleContent/RuleInstallation/RuleMaterial/RuleMemoCard/RuleMemoCardDownload/RuleMemoCardFileNamePrefix/RuleMemoInstructions/RuleSummary/RuleVariants), stored as attributes of a single resource entity (eid=10340), FR dimensionless default. These are a **separate localization rail** (#457 L1 / `dnn-ui-strings.csv`), **not** the #682 content-type provisioning.

➡️ The dispatch's "49 suffixés" = the **content-type attributes** (§1), **not** the res.* labels. Both rails need localization for full i18n, but they are provisioned differently (suffixed attrs vs res.* values). #490 §6 CSV (dispatch `22w5oc` [secondaire]) covers the res.* rail; #682 covers the content-type attrs.

### res.Rule* — the 9 confirmed FR values (from `14-res-rule-dictionary.json`)

For #490 (next in deep-queue), the live FR values to verify against `dnn-ui-strings.csv`:

| key | FR (live, eid=10340) | `dnn-ui-strings.csv` FR (line 4-11) | Match? |
|-----|----------------------|--------------------------------------|--------|
| RuleSummary | Résumé | Résumé | ✅ |
| RuleMaterial | Materiel | Matériel | ⚠ accent (Materiel vs Matériel) |
| RuleInstallation | Installation | Installation | ✅ |
| RuleVariants | Variantes | Variantes | ✅ |
| RuleMemoCard | Carte Mémo | Carte mémo | ⚠ case (Mémo vs mémo) |
| RuleMemoInstructions | (multi-sentence HTML) | Vous pouvez télécharger… | ✅ (live-captured 2026-06-16) |
| RuleMemoCardFileNamePrefix | Argumentum - Règle | Mémo | ⚠ divergent (FileNamePrefix ≠ display) |
| RuleMemoCardDownload | Télécharger la carte mémo | Télécharger la carte mémo | ✅ |
| RuleContent | Contenu | (not in dnn-ui-strings.csv — gap) | ⚠ NEW key, not in CSV |

→ Lifts the #490 HOLD with **5 confirmed + 4 divergent/gap** to reconcile. Detail in #490 work (next tick).

---

## 3. Spec §4 correction — `ToSIC_SexyContent_*` → `ToSIC_EAV_*`

[`2sxc-export-spec.md`](release-validation/2sxc-export-spec.md) §4 (Method B SQL) was written for 2sxc **15.02** and references the legacy `ToSIC_SexyContent_*` table family. The live site runs **2sxc v21** (`ToSIC_EAV_*` family, confirmed by web1 export `schemaFamily`). The SQL in §4 must be updated when re-used — the table names in the JOIN are wrong for v21. **Correction scope: spec §4 only** (a sibling doc, separate from this #682 revision). web1's Method B export already uses the correct `ToSIC_EAV_*` names; this is a doc-staleness fix, not a data issue.

Also: the spec probes **app=31** (debate/fallacies app) — the **Argumentum game is app=60** (ZoneID=3). The manifest `note_wrongApp` documents this correction. Any re-run of Method B must target app=60.

---

## 4. Decision (unchanged) + revised next actions

**DECISION (confirmed)**: **Path A** — provision **49 suffixed fields** (§2 of #694, unchanged) on the Game Rule content-type (AttributeSet 377, app=60). FR stays canonical (unsuffixed). PR #674's `Loc()` cascade consumes them at runtime.

**Revised next actions** (vs #694 §5):
1. **jsboige ratifies Path A** (unchanged recommendation). *(Decision point.)*
2. **jsboige executes DB provisioning** — create the 49 suffixed fields via 2sxc content-type editor, **type = String (Wysiwyg)** matching each generic counterpart (types now **confirmed**, §1 — no longer gated). Target **app=60 / AttributeSet 377**.
3. **Worker (post-provisioning)**: PR #674 becomes runtime-validable (unchanged).
4. **#684** translates the ~5 games' prose into 7 langs and populates the 49 fields. *(Note: export confirms **5 published rules**, not 24-30 — DB in seed/pre-migration state. #684 scope is smaller than estimated.)*

---

## 5. Gate boundaries (HARD — analysis only)

- ❌ No DB write, no provisioning executed, no prod interaction (export = Method B read-only SELECT).
- ❌ No #674 merge (runtime-pending, gated sandbox).
- ❌ No CSV mutation, no régén launch.
- ✅ Decision + field-list **confirmed** by live 2sxc v21 export (#681). Type gate **resolved** (all String).
- ✅ Honest about the 2 deltas still open for sibling docs (spec §4 staleness, app-id correction) — flagged, not papered over.

Relates: dispatch `22w5oc` (primary), #682 (this issue), #681 (export, delivered), #694 (original decision-input, confirmed), #687 (binary scan, superseded by #681 for types), #674 (RulesExplorer `Loc()` refactor), #490 (res.* rail, next), #684 (translation, 5 games not 30), #458.
