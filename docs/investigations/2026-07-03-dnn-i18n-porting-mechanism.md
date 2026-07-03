# 2026-07-03 — DNN multilingual porting — mechanism investigation (read-only)

**Scope**: Read-only investigation of the **mechanism** for publishing the Argumentum DNN site
content in the 7 non-FR languages (EN/RU/PT/ES/AR/FA/ZH). Triggered by ai-01 dispatch
`msg-20260703T171820-8b3ymj` (coupled-release critical path), building on the prod coverage
audit ([2026-07-03-dnn-prod-rules-coverage.md](2026-07-03-dnn-prod-rules-coverage.md), PR #662:
site is FR-only, 7 translations not published).

**Deliverable (DoD)**: the mechanism (how DNN/2sxc content localization works here), an
**effort estimate per language**, and a step plan. **Read-only — 0 prod write, 0 DB mutation.**

**Repo reference**: DNN export at `DNNPlatform/` (master `27442add`). App templates at
`DNNPlatform/Portals/1/2sxc/Argumentum/`.

---

## TL;DR — verdict

> The porting is **mechanically understood and feasible**, but it is **NOT a config flip**.
> It spans **3 independent content layers**, one of which (the rule *content*) has **no
> localization plumbing in its view at all** and its source values are **DB-only** (not in the
> repo, not derivable from the Rules CSV). The single hard unblocker is a **portal/2sxc DB
> export (jsboige-gated)** — without it, the bulk of the work cannot even be *seen*, let alone
> translated.

Two bespoke content apps sit at **different i18n maturity levels**:

| App | View | i18n plumbing in code? | Content values |
|-----|------|------------------------|----------------|
| **FallacyExplorer** (fallacies) | `_FallacyExplorer_Root.cshtml` | ✅ **YES** (post-#490) — `loc()` helper, `CmsContext.Culture` detection, cascade `lang→en→fr`, 8-lang dictionary | lang-suffixed fields (`text_fr`/`text_en`/…/`desc_*`/`link_*`), CSV-driven (`App.Query["FallaciesFromCSV"]`) |
| **RulesExplorer** (rules) | `_RulesExplorer_Rule{List,Detail}.cshtml` | ❌ **NO** — generic fields (`ruleEntity.Summary`), no culture logic, hardcoded FR ("de X à Y joueurs") | generic fields (`Summary`/`Material`/`Installation`/`Content`/`Variants`/`Memo`), **DB-only, manually authored** |

**The Rules gap is exactly why audit #662 found the rules page FR-only.** FallacyExplorer was
fixed by #490; RulesExplorer was not.

---

## 1. The 3 localization layers (from #457 inventory + this audit)

The #457 epic inventory ([457-site-content-type-inventory.md](../dnn-localization/457-site-content-type-inventory.md),
po-2023 2026-06-17) established 5 content-types. Mapped to the porting work:

| Layer | Content-types | Where | Repo? | Porting mechanism | Status |
|-------|---------------|-------|-------|-------------------|--------|
| **L1 — UI strings** | A (hardcoded) + B (`@Resources.*`) | `.cshtml` templates | ✅ keys; ⚠️ B values DB-only | gpt-5.5 rail **#487** (`Enabled=false`, 8-lang) — **built, gated on portal export** for B values | 🟡 near-ready |
| **L2 — static HTML** | C (2 prose pages, ~37K chars) | `DNNPlatform/fallacies/*.html` | ✅ Yes | chunked prose translation (separate DatasetUpdater task shape, **not wired**) | 🔴 not wired |
| **L3 — content items** | D (rules/fallacies/Resources/glossary VALUES) + E (page settings) | SQL (2sxc EAV) | ❌ DB-only | **the bulk** — needs portal export + (for Rules) view refactor + prose translation | 🔴 blocked |

**L3 is the dispatch focus and the dominant cost.** L1/L2 are scoped and partly built.

---

## 2. The mechanism — two apps, two models

### 2a. FallacyExplorer — the multilingual-ready pattern (post-#490)

`DNNPlatform/Portals/1/2sxc/Argumentum/_FallacyExplorer_Root.cshtml`:

```csharp
var query = App.Query["FallaciesFromCSV"];          // 2sxc Visual Query → CSV-backed entity set
var lang = (CmsContext.Culture.CurrentCode ?? "fr-fr").Split('-')[0].ToLowerInvariant();
var supported = new HashSet<string> { "fr","en","ru","pt","es","ar","fa","zh" };
if (!supported.Contains(lang)) { lang = "fr"; }

string loc(dynamic f, string field) {               // culture-aware field selection
    var primary = f.ContainsKey(field + "_" + lang) ? f[field + "_" + lang] : null;  // text_en, desc_en…
    if (!empty(primary)) return primary;
    var en = f.ContainsKey(field + "_en") ? f[field + "_en"] : null;                 // fallback EN
    if (!empty(en)) return en;
    return (f.ContainsKey(field + "_fr") ? f[field + "_fr"] : null) ?? "";           // fallback FR
}
```

- **Model**: language-**suffixed** fields (`text_fr`, `text_en`, …, `desc_*`, `link_*`), **not**
  2sxc EAV language dimensions. The 2sxc Visual Query `FallaciesFromCSV` surfaces a flat entity
  set with one field per language.
- **Resolution**: explicit `loc()` helper driven by `CmsContext.Culture.CurrentCode` (the DNN
  request culture), with a `lang → en → fr` cascade.
- **History note**: PHASE1 audit (2026-06-13, line 57) flagged this template as an "i18n bug"
  ("hardcodes EN value, reads `text_en` regardless of culture"). **That bug is fixed in the
  current revision** — the `loc()` cascade + culture detection + 8-lang `findOutMore`
  dictionary are present. This is the #490 DNN i18n infrastructure work.
- **Implication**: the fallacy content is **code-ready** for 7 languages. If the
  `FallaciesFromCSV` source carries the 8 language columns (the Fallacies CSV does), the
  content may already be multilingual in the data — **DB verification needed** (gated).

### 2b. RulesExplorer — the FR-only gap (the #662 finding)

`_RulesExplorer_RuleList.cshtml` (l.15) and `_RulesExplorer_RuleDetail.cshtml` (l.34-71):

```csharp
<h2>@ruleEntity.EntityTitle</h2>
<h5>de @ruleEntity.MinNbPlayers &agrave; @ruleEntity.MaxNbPlayers joueurs </h5>   <!-- hardcoded FR -->
@Html.Raw(ruleEntity.Summary)                                                       <!-- generic field, no loc() -->
… @Html.Raw(ruleEntity.Material) … @Html.Raw(ruleEntity.Installation) …
… @Html.Raw(ruleEntity.Content) … @Html.Raw(ruleEntity.Variants) … @Html.Raw(ruleEntity.Memo) …
```

- **Model**: **generic** fields (`Summary`, `Material`, `Installation`, `Content`, `Variants`,
  `Memo`, `EntityTitle`, `UrlKey`, `MinNbPlayers`, `MaxNbPlayers`). No language suffix, no
  `loc()`, no `CmsContext.Culture` reference.
- **No culture logic at all**: whatever culture DNN serves, the view emits the single (FR)
  value of each field. This is the direct cause of the FR-only verdict in audit #662.

### 2c. Critical: the site Rules content is NOT the Rules CSV

The repo Rules CSV (`Cards/Rules/Argumentum Rules - Cards.csv`) has columns
`pk, Text, Text_en, Text_ru, Text_pt, Text_ar, Text_es, Text_zh, Text_fa, print_and_play`.

The 2sxc Rule entity fields are `Summary, Material, Installation, Content, Variants, Memo, …`.

**These do not correspond.** The CSV `Text*` columns are the print/PDF rules (15 records, the
5 games). The 2sxc site Rule fields are **richer, manually-authored HTML content** (a Summary,
a Material list, an Installation section, the full rule Content, Variants, a Memo card) that
exists **only in the portal DB** (content-type D, per #457 inventory).

➡️ **The 8 existing CSV translations (`Text_en`, …) are NOT reusable as-is for the site** —
the site content is a different (richer) artifact living only in SQL. Porting the rules to 7
languages is a **prose translation of DB content**, not a CSV field remap.

---

## 3. Effort estimate (per language, 7 non-FR targets)

The work decomposes into a **fixed one-time cost** + a **per-language variable cost**.

### Fixed cost (once, jsboige/RDP-gated)
1. **Portal / 2sxc App export** — the hard unblocker. Exposes content-types D + E (rule
   content values, Resources values, page settings). Without it, nothing downstream is
   verifiable. (~jsboige, RDP/DB access)
2. **DNN content-language enablement** — Admin > Languages: enable the 7 cultures. One-time.
3. **URL routing + switcher** — either path-based (`/en-US/Règles`, currently all 404) or
   query-string (`?language=`, currently ignored — `<html lang>` stays `fr-FR`). Populate the
   empty `<div class="language">` skin object. (#490 shipped the shell; it is unpopulated.)
4. **RulesExplorer view refactor** — port the `loc()` pattern from FallacyExplorer into the
   two Rule views, OR enable 2sxc EAV language dimensions on the Rule content-type (requires
   knowing the content-type's field model — **DB-gated**, see §2c). Code change, worker-able.

### Variable cost (per language)
| Sub-task | Volume | Method | Est. effort |
|----------|--------|--------|-------------|
| **Rule content** (Summary/Material/Installation/Content/Variants/Memo × 5 rules ≈ 30 prose entities) | ~30 rich-HTML entities × 7 | gpt-5.5 chunked prose (Scenarii-style task) | **dominant** — the bulk |
| **Fallacy content** (text/desc × N) | unknown — **DB-gated**: if `FallaciesFromCSV` already carries 8 langs, ~0; if FR-only, large | verify first, then translate if needed | 0-to-large (uncertain) |
| **UI strings** (10 keys: 2 `ui.*` + 8 `res.*`) | 10 × 7 | gpt-5.5 rail **#487** (flip `Enabled=true`) | seconds (rail built) |
| **HTML pages** (2 files, ~37K chars) | ~37K chars × 7 | gpt-5.5 chunked prose (separate lane, not wired) | moderate |
| **App Resources values** (`@Resources.*` FR values, DB-only) | ~8 entities × 7 | gpt-5.5, post-export | small-moderate |

**Per-language rough order**: the rule-content prose dominates. A single language's full port
(rule prose + HTML + Resources + UI strings) is on the order of **a DatasetUpdater run**
(comparable to a Scenarii 167-record × 8-field pass), i.e. minutes-to-tens-of-minutes of
gpt-5.5 API time **once the export + view refactor + task config are in place**. The
preparation (export, refactor, config) is the real schedule driver, not the translation itself.

**Honest uncertainty**: the fallacy-content branch (0-to-large) and the exact Rule
content-type field model (lang-suffixed vs EAV dimensions) **cannot be resolved without the
portal export**. The estimate above assumes the Rule content-type is ported to the
lang-suffixed + `loc()` model (mirroring FallacyExplorer), which is the lower-risk path since
it reuses a pattern already proven in production by #490.

---

## 4. Step plan (recommended sequence)

1. **[jsboige] Portal / 2sxc App export** — dump the Argumentum app content-types + content
   items + Resources. This unblocks everything. (Gated; the single prerequisite.)
2. **[analysis] Inspect the exported content-types** — determine the Rule content-type field
   model (generic single-value today). Decide: adopt lang-suffixed fields + `loc()` (mirror
   FallacyExplorer, recommended) OR enable 2sxc EAV language dimensions.
3. **[worker, code] RulesExplorer view refactor** — port the `loc()` culture cascade into
   `_RulesExplorer_RuleList.cshtml` + `_RulesExplorer_RuleDetail.cshtml`; extract the
   hardcoded "de X à Y joueurs" into the UI-strings rail. (Mirror the tested #490 pattern.)
4. **[jsboige] DNN config** — enable 7 content languages, wire URL routing, populate the
   language switcher skin object.
5. **[worker, translation] Localize content** — for each non-FR language: rule prose (chunked
   gpt-5.5), Resources values, UI strings (#487 flip), HTML pages (new task). Re-import to 2sxc.
6. **[ai-01] Visual validation** — per-language render check (RTL AR/FA, CJK ZH, Cyrillic RU)
   on the live/staging site. (Visual verdict = ai-01 lane, per project rules.)

---

## 5. Gate boundaries (HARD — this investigation is read-only)

- ❌ No prod write, no DB mutation, no portal export attempted (jsboige-gated).
- ❌ Does not modify the Argumentum app templates (the §4 refactor is a *proposal*, not
  executed here — it is worker-able but is a separate code PR).
- ❌ Does not enable or modify the #487 DatasetUpdater rail.
- ❌ Does not declare a QA verdict (ai-01 only).
- ✅ All evidence is source-level (`code=truth`): `.cshtml` templates read directly, #457/#487/
  #490/#662 referenced by merged PR/commit.

---

## 6. Recommendation

- **Consistent with audit #662**: the multilingual porting is a **post-tag candidate** for
  v0.9.0 *assets* (decisions #27 RTL/CJK deferred to native validation, #16 DNN go-live
  decoupled from print assets). It does **not** block the print/PDF release.
- **BUT it is on the critical path of the "coupled release"** (release coupled to DNN
  go-live multilingual, per dispatch `8b3ymj`). If the coupled release must ship multilingual,
  the **portal export (step 1) is the schedule-critical prerequisite** and is jsboige's lane.
- The **highest-leverage worker-able prep** (doable now, before any export) is the
  **RulesExplorer view refactor** (step 3) — it mirrors the proven FallacyExplorer pattern and
  unblocks the code side independently of the DB export. Recommended as the first concrete
  code step once jsboige confirms the coupled-release scope.

---

## 7. Sources / reproducibility

- Templates read: `DNNPlatform/Portals/1/2sxc/Argumentum/_FallacyExplorer_Root.cshtml`,
  `_RulesExplorer_RuleList.cshtml`, `_RulesExplorer_RuleDetail.cshtml`.
- Repo CSV: `Cards/Rules/Argumentum Rules - Cards.csv` (15 records, 10 cols — field mismatch
  with site Rule content-type documented in §2c).
- Prior art: [457-site-content-type-inventory.md](../dnn-localization/457-site-content-type-inventory.md),
  [PHASE1-content-audit.md](../dnn-localization/PHASE1-content-audit.md),
  [2026-07-03-dnn-prod-rules-coverage.md](2026-07-03-dnn-prod-rules-coverage.md) (#662).
- Merged infra: #490 (DNN i18n culture plumbing + FallacyExplorer `loc()` fix), #487 (gpt-5.5
  UI-strings rail, `Enabled=false`), `tools/dnn_i18n/` (extractor + dry-run re-import).
- Read-only: no requests against `dnn.argumentum.myia.io` beyond the #662 audit; no DNN admin
  or DB access. All file evidence at master `27442add`.

Relates: #457, #487, #490, #662, #134, #140, dispatch `msg-20260703T171820-8b3ymj`.
