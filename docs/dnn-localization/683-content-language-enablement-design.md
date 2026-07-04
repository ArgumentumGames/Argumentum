# 2026-07-04 — DNN content-language enablement design (7 cultures + routing + switcher) — #683

**Scope**: design sketch (no prod execution) for step 4 of the #669 DNN i18n portage plan — enable
7 content languages on the DNN portal, wire URL routing so culture is reflected, and populate the
language switcher skin object so users can switch. Triggered by ai-01 dispatch `sde6s0` (tertiary).

**Issue**: #683 (jsboige-owned, parallel/non-blocked — does not gate on #681 export). Links to epic
#458 (TRACK 2 — Site DNN).

**Platform**: DNN 10.3.2 + 2sxc 21.07, .NET Framework 4.8. Portal: `dnn.argumentum.myia.io` (live,
full-IIS, ACME bypass active for renew). FR is canonical.

> **⚠ DESIGN ONLY.** No prod write, no DNN admin UI changes, no web.config mutation. This doc
> sketches the approach + decision points for jsboige to execute (or dispatch) when ready.

---

## TL;DR

Three config layers must be enabled for the site to serve 8 cultures:

1. **DNN content languages** — Admin > Site > Languages: enable 7 cultures (en/ru/pt/es/ar/fa/zh);
   FR already canonical. One-time, jsboige admin UI.
2. **URL routing** — decide path-based (`/en-US/<page>`) vs query-string (`?language=en-US`). Today
   both are broken (path = 404, query = ignored, `<html lang>` stays `fr-FR`).
3. **Language switcher** — populate the empty `<div class="language">` skin object (shell shipped by
   #490, currently unpopulated). Wire it to the routing mode chosen.

2sxc side: `CmsContext.Culture.CurrentCode` already reflects the DNN request culture — the `loc()`
cascade (lang→en→fr) in FallacyExplorer (post-#490) and RulesExplorer (post-#674) picks it up
automatically once DNN serves the culture. So this step is **upstream of** the view refactor — it
makes `CmsContext.Culture` return the right value.

---

## 1. Current state (code=truth from #669 §3)

| Aspect | Current | Source |
|--------|---------|--------|
| Canonical culture | `fr-FR` (portal default) | DNN portal config |
| `<html lang>` | stays `fr-FR` regardless of request | #669 §3 (query-string ignored) |
| Path-based routing (`/en-US/Règles`) | **404** (no localized pages provisioned) | #669 §3 |
| Query-string (`?language=`) | **ignored** (culture not applied) | #669 §3 |
| Language switcher skin object | `<div class="language">` shell shipped by #490, **unpopulated** | #669 §3 |
| FallacyExplorer view | `loc()` cascade **code-ready** post-#490 (reads `CmsContext.Culture`) | #669 §2a |
| RulesExplorer view | FR-only, no culture logic (PR #674 runtime-pending fixes) | #669 §2b |

➡️ The views are (or will be, post-#674) ready to consume a culture — the gap is **DNN is not
serving one**. This step fixes that.

---

## 2. DNN content-language enablement (Admin UI)

**Owner**: jsboige (DNN admin). **Effort**: one-time, ~minutes.

In DNN 10.3.2: **Admin > Site > Languages** (or Settings > Site Settings > Languages). For each of
the 7 target cultures:

- **Enable** the language (check Active).
- Mark localized page behaviour (DNN creates localized page variants when enabled — see §3 routing
  implications).

Cultures to enable (DNN culture codes map to the 8 release langs):

| Release lang | DNN culture code | Notes |
|--------------|------------------|-------|
| en | `en-US` (or `en-GB`) | secondary |
| ru | `ru-RU` | Cyrillic |
| pt | `pt-PT` (or `pt-BR`) | |
| es | `es-ES` | |
| ar | `ar-...` (e.g. `ar-SA`/`ar`) | **RTL** — `dir=rtl` |
| fa | `fa-IR` | **RTL** — `dir=rtl` |
| zh | `zh-CN` | CJK |

`fr-FR` remains the canonical/default portal language.

> **2sxc note (#687)**: the 2sxc EAV is currently dimensioned FR+EN only. Enabling the DNN cultures
> does **not** automatically dimension 2sxc EAV for them — but **Path A (lang-suffixed fields +
> `loc()`, PR #674/#682) does not need EAV dimensions**. The `loc()` cascade reads suffixed fields
> regardless of EAV dimensioning. So enabling DNN cultures is sufficient for the lang-suffixed path.

---

## 3. URL routing — decision needed (jsboige)

DNN supports three routing modes for multilingual (Settings > Site Settings > Site Behavior >
Language Settings):

### Option 3a — Path-based (`/en-US/<page>`)
- URLs: `dnn.argumentum.myia.io/en-US/Règles`, `.../ru-RU/Règles`, …
- **Pro**: SEO-friendly, culture visible in URL, bookmarkable.
- **Con**: DNN creates **localized page variants** per enabled language → must manage/translate page
  names per lang, or pages 404 (today's state). More page-admin overhead.
- **Current**: 404 (localized pages not provisioned).

### Option 3b — Query-string (`?language=en-US`)
- URLs: `dnn.argumentum.myia.io/Règles?language=en-US`
- **Pro**: single page tree (no localized variants), less admin.
- **Con**: less SEO-friendly; culture in query string.
- **Current**: ignored (culture not applied to request).

### Option 3c — Domain/cookie (less common for DNN)
- Per-language subdomains (`en.dnn.argumentum.myia.io`) or cookie-based.
- **Con**: extra DNS/TLS per subdomain; not standard DNN. Likely overkill.

**Recommendation**: **Option 3b (query-string)** for fastest go-live (single page tree, no localized
variant admin) OR **Option 3a (path-based)** if SEO/per-lang URLs matter for an educational product.
**Decision = jsboige.** Either way, the view layer (`loc()` via `CmsContext.Culture`) is agnostic —
it reads whatever culture DNN assigns to the request.

---

## 4. Language switcher — populate the skin object

The #490 merge shipped a shell `<div class="language">` in the skin (currently empty). Populate it
with a DNN **language selector skin object**:

- DNN ships a built-in `LanguageSelector` / `LanguageEnabler` skin object (`<dnn:LANGUAGE runat=
  "server" />` or the 2sxc-equivalent).
- Wire it to render a flag/name dropdown for the 8 cultures, emitting the routing form chosen in §3
  (path or query-string).
- On switch: set the request culture → `CmsContext.Culture.CurrentCode` updates → `loc()` cascade
  re-resolves → page re-renders in the new lang.

Skin object location: the active skin's `.ascx` (DNN skin). jsboige to locate the active skin and
add the object where the `<div class="language">` placeholder sits.

---

## 5. `<html lang>` / `dir` reflection

Once DNN serves the request culture, ensure:

- `<html lang="<culture-code>">` reflects the culture (DNN sets this from the request language when
  routing is wired — today it's stuck `fr-FR` because query-string is ignored).
- **RTL**: for `ar` and `fa`, `<html dir="rtl">` (or `dir="rtl"` on the content container). DNN
  flips `dir` automatically when the culture is marked RTL in its language config; verify in the
  Admin > Languages RTL flag per culture.
- **CJK/Cyrillic**: no `dir` change needed; verify fonts render (tofu check in #685 visual QA).

> This is the #685 (visual validation) entry condition: without `<html lang>`/`dir` correct,
> per-language visual QA cannot pass. So #683 is on the critical path of the coupled release
> (#683 → #684 → #685 → #134 coupled sign-off).

---

## 6. Wiring with the 2sxc view layer (no extra work here)

The `loc()` cascade (FallacyExplorer post-#490, RulesExplorer post-#674):

```csharp
var lang = (CmsContext.Culture.CurrentCode ?? "fr-fr").Split('-')[0].ToLowerInvariant();
string loc(dynamic f, string field) {
    // try lang-suffixed field, cascade lang → en → fr
}
```

Once #683 makes `CmsContext.Culture.CurrentCode` return the right culture, **the views pick it up
with no further change**. So #683 is purely the DNN-config layer; the 2sxc code layer is separate
(#674) and already designed to consume it.

---

## 7. Decision points for jsboige

1. **Routing mode**: path-based (3a) vs query-string (3b)? (Recommendation: 3b for speed, 3a for
   SEO.)
2. **Localized page variants**: if 3a, are page names translated per lang, or left FR with localized
   content? (Affects page-admin volume.)
3. **Culture codes**: confirm exact DNN codes (`en-US` vs `en-GB`, `pt-PT` vs `pt-BR`, `zh-CN`).
4. **Switcher UX**: flag icons, native names, or codes? (Brand decision.)
5. **Timing**: parallel to #681 export (non-blocked) — can be staged on sandbox first
   (`docs/dnn-localization/131-step2-smoke-test-checklist.md` pattern) before prod.

---

## 8. Sequencing

```
#683 (this design → jsboige DNN config) ──┐
#681 (export) ──► #682 (field-model) ──► #674 (view refactor runtime) ──┼──► #684 (translate) ──► #685 (visual QA) ──► #134
                                          ──────────────────────────────┘
```

#683 is **parallel/non-blocked** (does not need the export). It can be staged on the sandbox
(IIS Express :8090, per the Phase B work) before prod go-live.

---

## Gate boundaries (HARD — design only)

- ❌ No prod write, no DNN admin change, no web.config mutation, no skin edit.
- ❌ No #674/#596 merge (runtime-pending / garde-fou).
- ✅ Design sketch only; all routing/state claims cited from #669 (merged) + #687 (merged).
- ✅ Decision points surfaced for jsboige — not pre-decided unilaterally.

Relates: dispatch `sde6s0` (tertiary), #683 (this issue), #458 (TRACK 2 epic), #669 (mechanism,
step 4), #490 (FallacyExplorer loc shell + switcher shell), #674 (RulesExplorer view refactor),
#681 (export), #682 (field-model Path A), #685 (visual QA — downstream), #134 (coupled release).
