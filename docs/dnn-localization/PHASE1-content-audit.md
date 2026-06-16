# DNN Site Localization — Phase 1 Content Audit (#457)

**Status:** Phase 1 (Content Extraction) — repo-side audit complete
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-13
**Scope:** v0.9.0 = 8 languages (FR/EN/RU/PT/ES/AR/FA/ZH). FR is the canonical source.
**Base:** master `7b57251e`

> This document is the deliverable for Epic #457 **Phase 1 — Content Extraction**, sub-tasks
> #N1 (audit pages → inventory) and the repo-extractable slice of #N2–#N5 (export to CSV).
> It is **pure prep** — no release artifact, no tag, no DNN write. The release gate is untouched.

---

## TL;DR — what is and isn't extractable from the repo

The DNN export checked into this repo contains **application templates** (2sxc Razor `.cshtml`,
static HTML, framework `.resx`) but **not the portal content database**. In 2sxc, the actual
content items (glossary terms, FAQ, homepage/about text, per-rule content, and the App's
**resource dictionary values**) live in SQL — they are **not** in the repo.

| Bucket | In repo? | Phase 1 action |
|--------|----------|----------------|
| Custom template **hardcoded UI strings** | ✅ Yes | Extracted → `dnn-ui-strings.csv` |
| Custom template **`@Resources.*` keys** | ⚠️ Keys yes, **values DB-only** | Key inventory + inferred FR scaffold → CSV (marked `INFERRED`) |
| **Static HTML pages** (`fallacies/*.html`) | ✅ Yes (2 files) | Inventoried below; full extraction deferred pending live-nav confirmation |
| 2sxc **content items** (glossary, FAQ, homepage, rules, downloads) | ❌ DB-only | **Blocked** — needs DNN/2sxc export (jsboige access, Phase 4 dependency) |
| **DNN page settings** (titles, SEO meta, nav labels) | ❌ DB-only | **Blocked** — needs portal export |
| Framework `.resx` (Exceptions, FileUpload, Country…) | ✅ Yes | **Out of scope** — DNN ships official language packs |

**Net correction to the epic's estimates:** the optimistic "~30 rules pages / ~50 glossary
entries / ~100 UI strings → CSV" assumes the content is in the repo. It is **not**. The
repo yields a *small, precise* extractable set (below). The **bulk requires a portal/2sxc
data export** before any CSV-driven translation can run. That export is the true unblocker
for Phases 2–3.

---

## 1. Custom Argumentum 2sxc app — templates

Path: `DNNPlatform/Portals/1/2sxc/Argumentum/`

Four custom Razor templates. Three carry Argumentum-specific display strings; one is a stock
landing-page builder template (no Argumentum content).

| Template | Role | Translatable strings |
|----------|------|----------------------|
| `_FallacyExplorer_Root.cshtml` | Fallacy list (reads `App.Query["FallaciesFromCSV"]`) | 1 hardcoded: `find out more` |
| `_RulesExplorer_RuleList.cshtml` | Rules list | 1 hardcoded: `de {0} à {1} joueurs` |
| `_RulesExplorer_RuleDetail.cshtml` | Rule detail + Memo card | same players string + **8 `@Resources.*` keys** |
| `_Album List.cshtml` | Stock landing-page builder | none (generic app, skip) |

### 1a. Hardcoded UI strings (fully extractable)

| key | source | FR (canonical) | note |
|-----|--------|----------------|------|
| `ui.fallacy.find_out_more` | `_FallacyExplorer_Root.cshtml:20` | en savoir plus | Template **hardcodes the EN value** *and* reads `text_en`/`desc_en`/`link_en` regardless of culture → **i18n bug** (see §4). |
| `ui.rules.players_range` | `_RulesExplorer_RuleList.cshtml:15`, `_RulesExplorer_RuleDetail.cshtml:36` | de {0} à {1} joueurs | Hardcoded FR (uses `&agrave;` entity). |

### 1b. `@Resources.*` keys — keys in repo, **values DB-only**

`@Resources.X` in 2sxc Razor14 resolves against the App's **Resources** dictionary, which is
stored in the portal DB (the Argumentum app folder has **no** `App_Data/` in the repo, so the
values are not checked in). The FR values below are **inferred from key + surrounding markup**
and must be **verified against a DNN/2sxc export** before being treated as source-of-truth.

| key | used at | inferred FR | confidence |
|-----|---------|-------------|------------|
| `res.RuleSummary` | `_RulesExplorer_RuleDetail.cshtml:37` (`<h2>`) | Résumé | high |
| `res.RuleMaterial` | `:41` (`<h2>`) | Matériel | high |
| `res.RuleInstallation` | `:43` (`<h2>`) | Installation | high |
| `res.RuleVariants` | `:50` (`<h2>`) | Variantes | high |
| `res.RuleMemoCard` | `:58` (`<h2>`) | Carte mémo | medium |
| `res.RuleMemoInstructions` | `:59` (`Html.Raw`) | *(multi-sentence instructions — DB-only)* | low — placeholder only |
| `res.RuleMemoCardFileNamePrefix` | `:67` (card name) | Mémo | medium |
| `res.RuleMemoCardDownload` | `:89` (button) | Télécharger la carte | high |

---

## 2. Static HTML pages

Path: `DNNPlatform/fallacies/`

| File | `<title>` | Size | Assessment |
|------|-----------|------|------------|
| `fallacies.html` | "Charte Html pour l'identification des arguments fallacieux" | 81 KB | **Legacy developer page** (HTML/PhpBB integration guide for the fallacy infographic). Almost certainly **not** in v0.9.0 site navigation. Recommend **exclude** unless jsboige confirms it is live. |
| `MariagePourTous.html` | "L'analyse rhétologique pour tous" | 38 KB | Worked-example essay (rhetological analysis of a real debate). **Candidate** user-facing content — but verify it is in live nav before scoping a full-page translation. |

Full text extraction of these pages is **deferred**: dumping 81 KB / 38 KB of raw HTML into a
translation CSV unverified is exactly the kind of bulk that creates noise. They are inventoried
here; a translate/exclude decision per page is a small, explicit step once live-nav status is
confirmed.

---

## 3. DB-only content (blocked — needs portal export)

None of the following is in the repo; all require a DNN/2sxc data export (jsboige DNN access,
already flagged as the **Phase 4** dependency in #457). Counts are the epic's estimates, to be
confirmed *by the export itself*:

| Content type | Source app/module | Est. records | Extraction method (when export available) |
|--------------|-------------------|--------------|-------------------------------------------|
| App Resources values (Rules UI, §1b) | Argumentum app Resources | ~8+ | 2sxc App export → `Resources` content-type JSON |
| Rules content (Summary/Material/Installation/Variants/Memo) | 2sxc content items | ~24 rules × 5 fields | 2sxc query export per rule entity |
| Glossary entries | `Glossary3` app | ~50 | 2sxc content export |
| FAQ entries | `Faq4` app | ~? | 2sxc content export |
| Homepage / About / landing content | `Content` app + page modules | ~10 pages | 2sxc content export per module |
| Navigation menu labels | DNN tabs | ~? | DNN page/tab export |
| SEO meta descriptions / titles | DNN page settings | ~40 | DNN page settings export |

> **Note — Fallacies Explorer is already localized.** It reads the taxonomy CSV via
> `App.Query["FallaciesFromCSV"]`, so fallacy content rides the existing 8-language card CSV.
> (Caveat: the *template* currently pins `_en` fields — see §4.) No separate extraction needed
> for fallacy bodies.

---

## 4. Bug found during audit — FallacyExplorer not culture-aware

> **✅ STATUS UPDATE (2026-06-16):** This bug was subsequently **FIXED in PR #464** (commit
> `c9197f15` — "@fix(dnn): make FallacyExplorer culture-aware"). `_FallacyExplorer_Root.cshtml` now
> resolves `field_{lang}` → `field_en` → `field_fr` via a `loc()` cascade + a localized
> `findOutMore` dictionary (8 langs). The finding below is preserved as the original Phase-1 record.

`_FallacyExplorer_Root.cshtml` hardcodes English fields and an English label:

```razor
@fallacy.text_en (#@fallacy.path)
...
@fallacy.desc_en
(<a href="@fallacy.link_en" target="_blank">find out more</a>)
```

Regardless of the visitor's language, it renders `text_en` / `desc_en` / `link_en` and the
literal "find out more". For an 8-language site this must resolve the **current-culture** field
(`text_{lang}` with EN/FR fallback, mirroring the card-pipeline `LinkXxFallback` cascade added
in PR #454) and use a localized label key (`ui.fallacy.find_out_more`).

**Not fixed here** (Phase 1 is audit/extraction only). Logged as a Phase 2/4 implementation
item; flagged to ai-01.

---

## 5. Phase 1 deliverables (this PR)

1. **`dnn-ui-strings.csv`** — DatasetUpdater-compatible. Columns:
   `key, context, source_file, fr, en, ru, pt, es, ar, fa, zh, notes`.
   - PrimaryField = `key`; source = `fr`; FieldsToUpdate = `en,ru,pt,es,ar,fa,zh`.
   - Rows: 2 hardcoded UI strings (real FR) + 8 resource keys (FR `INFERRED`, verify vs DB).
2. **This audit** (`PHASE1-content-audit.md`).

## 6. Recommended next steps (for ai-01 / jsboige)

1. **Unblock the bulk:** produce a DNN/2sxc **data export** of the Argumentum portal (Resources
   dictionary, Rules/Glossary/FAQ/Content items, page settings). This is the real gate for
   Phases 2–3 — without it, only the 10 repo strings above are translatable.
2. Confirm live-nav status of the 2 static HTML pages (§2) → translate-or-exclude.
3. Verify the 8 inferred FR resource values (§1b) against the export.
4. Schedule the FallacyExplorer culture-fix (§4) into Phase 2/4 implementation.

---

*Generated as Phase 1 prep — release gate untouched. Worker signals; visual/QA verdict and
merge are ai-01's.*
