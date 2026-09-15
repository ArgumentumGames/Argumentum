# #192 — `link_*` i18n URL Scoping (residual gap isolation)

**Worker** po-2024 · **Date** 2026-07-13 · **Base** master `b736a808` · **READ-ONLY — 0 write prod CSV, `Cards/` untouched.**
Dispatch ai-01 `9zs45o` (PRIMAIRE, ungated). Companion to the multilingual drift audit #795 ([`multilingual-drift-audit-2026-07.md`](multilingual-drift-audit-2026-07.md)) which isolated `link_*` URL coverage as the **only material i18n residual** — prose content is 100% clean across all 4 CSVs × 7 languages.

## 0. Headline

| Class | Cells | Status |
|---|---:|---|
| NATIVE_URL (localized to a native article) | 2499 | ✅ correct |
| OTHER_URL (language-agnostic reference) | 452 | ✅ acceptable |
| **FR_URL (FR URL copied, not localized)** | **15** | ❌ gap |
| **EMPTY (FR has link, lang empty)** | **3807** | ❌ gap |
| **TRUE i18n URL GAP (FR_URL + EMPTY)** | **3822** | treatment candidate |


## Fallacies (1408 rows; FR link filled = 637)
| lang | NATIVE_URL ✅ | OTHER_URL ✅ | FR_URL ❌ | EMPTY ❌ | GAP total | gap % |
|---|---|---|---|---|---|---|
| en | 918 | 414 | 0 | 24 | 24 | 3.8% |
| ru | 117 | 3 | 0 | 533 | 533 | 83.7% |
| pt | 102 | 0 | 0 | 550 | 550 | 86.3% |
| es | 105 | 0 | 0 | 546 | 546 | 85.7% |
| ar | 102 | 0 | 0 | 550 | 550 | 86.3% |
| fa | 91 | 0 | 0 | 556 | 556 | 87.3% |
| zh | 88 | 0 | 0 | 561 | 561 | 88.1% |

<details><summary>FR_URL samples (URL copied from FR, not localized)</summary>


</details>

## Virtues (223 rows; FR link filled = 216)
| lang | NATIVE_URL ✅ | OTHER_URL ✅ | FR_URL ❌ | EMPTY ❌ | GAP total | gap % |
|---|---|---|---|---|---|---|
| en | 185 | 5 | 4 | 22 | 26 | 12.0% |
| ru | 172 | 18 | 6 | 20 | 26 | 12.0% |
| pt | 186 | 0 | 0 | 31 | 31 | 14.4% |
| es | 146 | 5 | 2 | 63 | 65 | 30.1% |
| ar | 92 | 0 | 0 | 124 | 124 | 57.4% |
| fa | 90 | 7 | 3 | 116 | 119 | 55.1% |
| zh | 105 | 0 | 0 | 111 | 111 | 51.4% |

<details><summary>FR_URL samples (URL copied from FR, not localized)</summary>

- **en**:
  - pk=29: `https://www.persee.fr/doc/comm_0588-8018_2005_num_78_1_3769`
  - pk=182: `https://www.caissedesdepots.fr/agir-ensemble-professionnels/une-explication-suffisante`
  - pk=183: `https://halshs.archives-ouvertes.fr/halshs-02193571v2/document`
  - pk=200: `http://www.communication-orthophonie.fr/la-communication/la-communication-verbale/le-dialo`
- **ru**:
  - pk=10: `https://www.service-public.fr/professionnels-entreprises/vosdroits/F33492`
  - pk=143: `https://www.persee.fr/doc/hel_0750-8069_2003_num_25_1_3148`
  - pk=183: `https://halshs.archives-ouvertes.fr/halshs-02193571v2/document`
  - pk=186: `https://www.leadership-toolbox.fr/les-10-attributs-de-latout-leader-numero-2-live-with-an-`
- **es**:
  - pk=183: `https://halshs.archives-ouvertes.fr/halshs-02193571v2/document`
  - pk=200: `http://www.communication-orthophonie.fr/la-communication/la-communication-verbale/le-dialo`
- **fa**:
  - pk=165: `https://www.ameli.fr/sites/default/files/Documents/911739/document/gestion-risque-methodes`
  - pk=173: `https://www.communicaid.fr/communication-interculturelle/relation-professionnelle-intercul`
  - pk=174: `https://www.franceculture.fr/societe/faut-il-etre-neutre-ou-partisan-sur-les-questions-de-`

</details>

## 1. Nature of the gap — why this is NOT a gpt-5.5 translation task

Translating a `link_*` URL is **not text translation** — it is **cross-language article resolution**: given a French Wikipedia/Wiktionary article (or a French source), find the *equivalent native-language article* on `{lang}.wikipedia.org` (or a comparable native source). The URL string itself is not 'translated'; the *target article* must be discovered.

**gpt-5.5 can help, but cannot close it alone:**
- ✅ gpt-5.5 can **propose candidate** native article titles given the fallacy's `text_fr` + `desc_fr` (e.g. 'Appel à l'ignorance' → ru:'Апелляция к незнанию'). This is a *suggestion pass*, ~1259 calls, cheap.
- ❌ gpt-5.5 **cannot verify the article exists** at that exact title on {lang}.wikipedia.org, nor that it covers the same fallacy — Wikipedia titles vary, articles may not exist, or may be disambiguation pages. **A human (or a Wikipedia API HEAD check) must confirm each candidate resolves.**
- ❌ For non-Wikipedia FR sources (cortecs.org, service-public.fr, village-justice.com, huffpost.com FR), there is **no translation path at all** — a native equivalent source must be researched manually or the link dropped.

This matches the standing memory `i18n-coverage-gap-is-link-urls`: link translation is **human research**, deliberately out of gpt-5.5's scope. The drift audit #795 confirmed it is the *only* residual (prose is 100% clean).

## 2. Treatment recommendation (decision input for ai-01 / jsboige)

| Option | What | Effort | Risk | Recommended? |
|---|---|---|---|---|
| **A — Defer (do nothing now)** | Ship v0.9.0 with current `link_*` state; native readers fall back to FR/reference URLs | 0 | Low (FR URL still works, just not localized) | ✅ **default** |
| **B — gpt-5.5 candidate pass + human verify** | gpt-5.5 proposes {lang} article title for each FR-URL/EMPTY cell; human (or Wikipedia API) confirms + fills | ~1259 gpt-5.5 calls + human verify | Medium (gpt-5.5 may hallucinate titles) | ⚠️ post-tag, if jsboige wants |
| **C — Wikipedia API auto-resolve** | Script queries `{lang}.wikipedia.org` API with the FR article title + langlinks to find the native equivalent; fills only API-confirmed matches | Script + spot-check | Low (API-confirmed = real article) | ✅ best ROI, post-tag |
| **D — Drop FR-specific non-Wikipedia links for non-FR langs** | For cortecs/service-public/village-justice URLs in non-FR columns, clear the cell (a dead FR link is worse than no link) | Small | Low | ⚠️ editorial decision |

**Recommendation:** **A (defer to v1.0)** for v0.9.0 — the gap is pre-existing, documented (#192, #795), and does not block the release (prose is clean, URLs are a secondary navigation aid). Post-tag, **Option C (Wikipedia langlinks API auto-resolve)** has the best ROI: it programmatically resolves the ~Wikipedia subset of FR_URL+EMPTY to native articles with zero hallucination risk (API-confirmed), leaving only the non-Wikipedia FR sources for Option D/manual.

## 3. Scope breakdown (Wikipedia vs non-Wikipedia FR sources)

The FR source links split into an **automatable** subset (Wikipedia/Wiktionary — Option C langlinks API can resolve these to native articles with zero hallucination) and a **manual-only** subset (French institutional/editorial sources — no auto-path):

| Dataset | FR links total | Wikipedia/Wiktionary (automatable) | Non-Wiki FR (manual/drop) |
|---|---:|---:|---:|
| Fallacies | 637 | 562 (88%) | 75 (11%) |
| Virtues | 216 | 161 (74%) | 55 (25%) |

The non-Wiki FR sources are: `cortecs.org`, `service-public.fr`, `village-justice.com`, `huffpost.com` (FR), `persee.fr`, `leadership-lavautoir.com` / `leadership-toolbox.fr`, `ameli.fr`, `communicaid.fr`, `franceculture.fr`, `halshs.archives-ouvertes.fr`, `communication-orthophonie.fr`, `caissedesdepots.fr` — all French-only institutional/editorial sites with **no native equivalent to translate to**. These are Option D candidates (clear the cell for non-FR langs: a dead FR link is worse than no link for a native reader).

**ROI read:** the Wikipedia subset (the majority of FR links in Fallacies) is the high-ROI automatable target; the non-Wiki subset is a smaller editorial drop.

## 4. Gate boundaries

- ✅ **READ-ONLY** — 0 write prod CSV; `git diff b736a808 -- Cards/` empty.
- ✅ Empirical — every count computed from the prod CSVs at master `b736a808`.
- ✅ Bounded + pre-tag-safe — scoping doc only, no treatment executed; does not touch the v0.9.0 critical path.
- ✅ Reusable idempotent script `tools/192-link-i18n-scoping.py` (`--json` / `--report`).
- ❌ No CSV write, no link_* cell modified — treatment is a separate gated decision.
- ❌ Verdict QA = ai-01 — this is a scoping input, not a treatment verdict.

🤖 Worker po-2024 — #192 link_* i18n URL scoping (gated doc, 0 write, gap isolated).
