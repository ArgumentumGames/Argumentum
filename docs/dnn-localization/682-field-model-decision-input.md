# 2026-07-05 — #682 Rule field-model decision-input + provisioning list (Path A)

> **📋 REVISED 2026-07-09 — see [`682-field-model-revision-2sxc21.md`](682-field-model-revision-2sxc21.md).**
> The Path A decision + 49 suffixed fields below are **confirmed correct** by the live 2sxc v21 export (#681). The revision corrects 3 deltas the binary scan #687 could not see: (1) schema family `ToSIC_EAV_*` v21 (not `ToSIC_SexyContent_*` 15.02), (2) app=60 not app=31, (3) attribute types **resolved** (all 7 translatable = String/Wysiwyg, no longer gated). The provisioning list (49 fields) is **unchanged**. This doc stays as the original decision record.

**Scope**: decision-input for #682 — the Rule field-model decision (step 2 of the #669 portage plan).
Produces the recommendation (Path A vs Path B), the rationale, and the **exact field-list to provision
× 8 languages** so jsboige's DB provisioning becomes a trivial mechanical execution. Triggered by ai-01
dispatch `0hbg9t` (primary).

**Repo reference**: master `d90ce613`. Issue: #682 (field-model decision). Owner: jsboige (DB) +
worker (analysis = this doc). Blocked-by: #681 (export) **for the exact SQL-type enumeration only**;
the decision + field-list below are derived from the known field model (#669 §2c + #687 binary scan +
PR #674 view refactor) and are **decidable now**.

> **⚠ Status**: analysis/decision-input only. **No DB write, no provisioning executed.** The exact
> 2sxc attribute *types* (String/Hyperlink/…) remain gated on the sysadmin unblocker (#687 §3) — but
> the field *names* × *languages* are known with high confidence and are what jsboige needs to execute.

---

## TL;DR — decision

**Recommendation: Path A — language-suffixed fields + `loc()` cascade** (mirror FallacyExplorer #490 /
PR #674). Provision **7 translatable fields × 7 non-FR languages = 49 new suffixed fields** on the
Rule content-type.

| | Path A (lang-suffixed + `loc()`) ✅ | Path B (2sxc EAV language dimensions) |
|---|---|---|
| **DB provisioning** | Add 49 suffixed fields (mechanical, jsboige) | Enable 5 new EAV cultures (ru/pt/es/ar/fa/zh) before any translation attaches |
| **Code** | PR #674 already implements `Loc()` cascade (runtime-pending) | View refactor needed anyway + dimension-wiring in 2sxc admin |
| **Risk** | Low — reuses a production-proven pattern (#490 live since FR/EN) | Higher — RTL/CJK EAV-dimension behavior, 5-culture enablement prerequisite |
| **Translation flow** | Field-by-field, graceful (`loc()` falls back to FR until a value is populated) | Blocked until all 5 cultures enabled, then value attaches per dimension |
| **EAV dimensioning needed?** | **No** — `loc()` reads suffixed fields regardless of EAV dimensions | **Yes** — the whole point of Path B |

➡️ **#687 forced this decision**: the prod EAV is dimensioned **FR+EN only** (en-us 6523, fr-fr 4184;
ru/ar/fa/zh = trace noise; pt/es = 0). Path B would block the entire portage on enabling 5 new DNN
content cultures + their 2sxc EAV dimensions. Path A needs **none of that** — `loc()` reads suffixed
fields independent of EAV dimensioning. **Decision is effectively forced.**

---

## 1. Current Rule content-type field model (code=truth)

From #669 §2c (view-template audit, merged) + #687 (binary scan, merged):

The Rule content-type uses **generic, language-neutral fields** — **0 language-suffixed fields**
(`Summary_en`/`Summary_fr` confirmed absent in the prod backup binary scan). Fields used by the two
RulesExplorer views:

| Field | Type (inferred) | Localizable? | Used in view |
|-------|-----------------|--------------|--------------|
| `EntityTitle` | String (text) | ✅ YES (rule title) | List + Detail (×3) |
| `Summary` | String (rich HTML) | ✅ YES | List + Detail |
| `Material` | String (rich HTML) | ✅ YES | Detail |
| `Installation` | String (rich HTML) | ✅ YES | Detail |
| `Content` | String (rich HTML) | ✅ YES | Detail |
| `Variants` | String (rich HTML) | ✅ YES | Detail (cond + raw) |
| `Memo` | String (rich HTML) | ✅ YES | Detail (cond + raw) |
| `UrlKey` | String (slug) | ❌ NO (SEO slug, non-localizable per #674 scope) | — |
| `MinNbPlayers` | Numeric (int) | ❌ NO | hardcoded FR "de X à Y" |
| `MaxNbPlayers` | Numeric (int) | ❌ NO | hardcoded FR "à Y joueurs" |

**7 translatable fields** (the prose that needs localization). The `de X à Y joueurs` hardcoded FR
(`MinNbPlayers`/`MaxNbPlayers`) is a **UI-string** issue, handled by the #457 L1 / #487 rail (separate
lane, out of #682 scope).

---

## 2. Path A provisioning — the exact field-list (jsboige execution input)

For each of the **7 translatable fields**, add **7 language-suffixed variants** (en/ru/pt/es/ar/fa/zh):

```
EntityTitle_en, EntityTitle_ru, EntityTitle_pt, EntityTitle_es, EntityTitle_ar, EntityTitle_fa, EntityTitle_zh
Summary_en,      Summary_ru,      Summary_pt,      Summary_es,      Summary_ar,      Summary_fa,      Summary_zh
Material_en,     Material_ru,     Material_pt,     Material_es,     Material_ar,     Material_fa,     Material_zh
Installation_en, Installation_ru, Installation_pt, Installation_es, Installation_ar, Installation_fa, Installation_zh
Content_en,      Content_ru,      Content_pt,      Content_es,      Content_ar,      Content_fa,      Content_zh
Variants_en,     Variants_ru,     Variants_pt,     Variants_es,     Variants_ar,     Variants_fa,     Variants_zh
Memo_en,         Memo_ru,         Memo_pt,         Memo_es,         Memo_ar,         Memo_fa,         Memo_zh
```

**Total: 7 fields × 7 langs = 49 new suffixed fields.**

### Suffix model (FR = canonical, unsuffixed)

- The **existing generic fields** (`Summary`, `Material`, …) **stay as the FR canonical** — no rename,
  no data migration. They keep holding the FR value.
- The **new suffixed fields** are **empty initially** (no translation yet — that's #684's job).
- `_fr` suffixed fields are **NOT provisioned** (FR = unsuffixed canonical). PR #674's `Loc()` cascade
  reflects this: `<field>_<lang>` → `<field>_en` → `<field>_fr` → `<field>` (generic).

> **Note on the `_fr` fallback in #674**: the `Loc()` cascade checks `<field>_fr` as 3rd fallback, but
> the 4th fallback (generic/unsuffixed) is what actually serves FR today (since `_fr` suffixed fields
> don't exist). This 4th fallback is **critical** — it preserves the current FR rendering during the
> DB-migration transition. If a `_fr` suffixed field is later provisioned, the cascade picks it up;
> either way FR renders. See PR #674 body ("The 4th fallback on the generic field").

### 2sxc attribute types (gated — best-effort inference)

The exact 2sxc attribute type per field (String / Hyperlink / Boolean / Entity) is **not enumerable
without the sysadmin unblocker** (#687 §3: `jsboi` has no server role + is not mapped in
`ArgumentumGames` DB → no SELECT on `ToSic_EAV_Attribute`). Best-effort inference from the view usage
(`@Html.Raw(field)` = rich HTML = **String** type):

| Field | Inferred 2sxc type | Confidence |
|-------|--------------------|------------|
| `EntityTitle` | String (plain) | High |
| `Summary`, `Material`, `Installation`, `Content`, `Variants`, `Memo` | String (rich HTML / Wysiwyg) | High (all rendered `@Html.Raw`) |

➡️ jsboige: when creating the suffixed fields in the 2sxc content-type editor, **match the type of the
existing generic field** (open the Rule content-type, note each field's Type, replicate it for the 7
suffixed variants). This is the only step that benefits from the live 2sxc admin UI (voie-1) or the
sysadmin-gated SQL enumeration.

---

## 3. How `loc()` consumes the provisioned fields (PR #674, runtime-pending)

PR #674 (`_RulesExplorer_Rule{List,Detail}.cshtml`) implements:

```csharp
var lang = (CmsContext.Culture.CurrentCode ?? "fr-fr").Split('-')[0].ToLowerInvariant();
string Loc(dynamic f, string field) {
    var primary = f.ContainsKey(field + "_" + lang) ? f[field + "_" + lang] : null;  // Summary_ru, Memo_zh…
    var en      = f.ContainsKey(field + "_en") ? f[field + "_en"] : null;
    var fr      = f.ContainsKey(field + "_fr") ? f[field + "_fr"] : null;
    return primary ?? en ?? fr ?? (f.ContainsKey(field) ? f[field] : "");             // generic = FR canonical
}
```

Fields routed through `Loc()` in #674: `EntityTitle` (List+Detail), `Summary` (List+Detail),
`Material`/`Installation`/`Content`/`Variants`/`Memo` (Detail). **This exactly matches the 7
translatable fields in §2.**

Once the 49 suffixed fields are provisioned + #684 populates them with translations, the cascade picks
up the localized value for the request culture (`CmsContext.Culture.CurrentCode`, set by #683 DNN
content-language enablement). No further view change needed — the `Loc()` is already designed for this.

---

## 4. Path B contrast (rejected, for completeness)

Path B = enable 2sxc EAV language dimensions on the Rule content-type. Per #687:
- Prod EAV is **FR+EN only**. Path B requires **enabling 5 new content cultures** (ru/pt/es/ar/fa/zh)
  in DNN Admin > Languages + their 2sxc EAV dimensions **before** any translation can attach.
- More fragile: RTL (ar/fa) and CJK (zh) EAV-dimension behavior is less battle-tested than suffixed
  fields; the FallacyExplorer production pattern (#490) uses suffixed fields precisely to avoid this.
- Higher prerequisite bar + more moving parts, for no functional gain over Path A.

➡️ **Reject Path B.** Path A is lower-risk, reuses a production-proven pattern, and needs no EAV
dimensioning. #687's finding (EAV FR+EN only) makes Path B strictly worse.

---

## 5. Decision + next actions

**DECISION (worker recommendation, for jsboige ratification)**: **Path A** — provision 49 suffixed
fields (§2 list) on the Rule content-type. FR stays canonical (unsuffixed). PR #674's `Loc()` cascade
consumes them at runtime.

**Next actions**:
1. **jsboige ratifies Path A** (this recommendation). *(Decision point — no code/DB yet.)*
2. **jsboige executes DB provisioning** — create the 49 suffixed fields via 2sxc content-type editor
   (voie-1) OR via SQL if sysadmin unblocked. Match each field's type to its generic counterpart.
3. **Worker (post-provisioning)**: PR #674 becomes **runtime-validable** — sandbox test: add one
   suffixed value (e.g. `Summary_en` = "test"), render RuleDetail with `?language=en-US`, confirm
   `Loc()` picks up the EN value. Smallest possible validation.
4. **#684** translates the ~30 rich-HTML prose entities into the 7 langs (chunked gpt-5.5) and
   populates the 49 fields × ~5 games.

---

## 6. DoD status

| DoD item (dispatch `0hbg9t` primary) | Status |
|----------------------------------------|--------|
| Recommendation Path A vs EAV with rationale | ✅ Path A, rationale §1+§4, forced by #687 |
| Enumeration of fields to provision × 8 langs | ✅ 7 translatable × 7 non-FR langs = 49 fields (§2), FR unsuffixed canonical |
| Comment on #682 (decisional) | ✅ To post (PR linked) |
| Doc `docs/dnn-localization/682-*.md` in PR | ✅ This doc |

**Open (gated)**: exact 2sxc attribute *types* (String/Hyperlink/…) — gated on sysadmin unblocker
(#687 §3). Best-effort inference provided (§2); jsboige can confirm via live 2sxc editor.

---

## Gate boundaries (HARD — analysis only)

- ❌ No DB write, no provisioning executed, no prod interaction.
- ❌ No #674 merge (runtime-pending, gated sandbox).
- ❌ No CSV mutation, no régén launch.
- ✅ Decision + field-list derived from merged investigations (#669 §2c, #687) + PR #674 view code.
- ✅ Honest about the sysadmin gate for exact-type enumeration (not papered over).

Relates: dispatch `0hbg9t` (primary), #682 (this issue), #681 (export unblocker), #669 (mechanism),
#687 (schema export, binary scan + sysadmin blocker), #674 (RulesExplorer `Loc()` refactor),
#490 (FallacyExplorer `loc()` prod pattern), #684 (translation), #683 (DNN content-language), #458.
