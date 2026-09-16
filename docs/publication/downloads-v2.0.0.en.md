# Downloads page — Argumentum v2.0.0 (EN mirror draft)

> **Status: DRAFT (#135 §Downloads prep, pool #458 grain po-2023).** Prepares the DNN site **Downloads**
> page content for the **v2.0.0** release (re-scoped v0.9.0 → v2.0.0, jsboige 2026-08-06, #999). **English
> mirror** of the FR canonical ([downloads-v2.0.0.fr.md](downloads-v2.0.0.fr.md)) — the DNN portal serves FR
> as primary. **Publication is GATED** on #134 (GitHub Release tag), #132 (prod deployment) and #131
> (DNN 10.3.2 live) — see "Publish checklist". **jsboige review required before any publish.** This file
> prepares the copy; it publishes nothing.
>
> **Source of truth:** [RELEASE-NOTES-v2.0.0.md](../RELEASE-NOTES-v2.0.0.md) (reader-facing notes +
> qualification annex) · [cards-catalog.en.md](cards-catalog.en.md) (formats, volumes) ·
> [news-article-v2.0.0.en.md](news-article-v2.0.0.en.md) (the announcement links here) ·
> `manifest-v2.0.0.md` of bundle `review-v2.0.0-regen-20260912` (80 sha256 fingerprints, `Status: FINAL`,
> base `65dd4742`). `[PLACEHOLDER]` fields are filled at tag time.
>
> **Sizes:** every PDF size below is **MEASURED on 2026-09-16** on the final 12/09 bundle (qualification in
> annex A). **Zip archive** sizes will be re-measured at packaging (a PDF is already Flate-compressed; the
> zip changes the order of magnitude of nothing).

---

## Pre-release warning (remove at tag time)

⚠️ **Until the `v2.0.0` tag**, do not redistribute the `v0.9.0-review` pre-release assets: they date from
Aug 24-25 and predate the September corrections (detailed state in the
[drift dossier](../release-dossier/134-release-assets-drift-2026-09-15.md), PR #1383). The page must
reference **only** the fresh assets uploaded at the tag (checklist).

## Browse by language — the complete material, 8 times over

Every language ships the **complete** material (10 documents). Per-language package link:
`[PLACEHOLDER — asset URL argumentum-{lang}-v2.0.0.zip, scheme #999 DoD 4; `Print.Play` vs `Print&Play`
naming to be settled at packaging]`.

| Language | Complete material | of which Print & Play only | Documents |
|---|---:|---:|---:|
| العربية (ar) | **459.9 MB** | 205.6 MB | 10 |
| English (en) | **469.0 MB** | 211.6 MB | 10 |
| Español (es) | **472.2 MB** | 212.9 MB | 10 |
| فارسی (fa) | **470.3 MB** | 211.4 MB | 10 |
| Français (fr) — source language | **465.5 MB** | 210.2 MB | 10 |
| Português (pt) | **481.4 MB** | 217.3 MB | 10 |
| Русский (ru) | **449.3 MB** | 204.7 MB | 10 |
| 中文 (zh) | **478.0 MB** | 213.4 MB | 10 |
| **Total** | **3,745.6 MB** | 1,687.1 MB | **80** |

"Print & Play only" = the 4 home-printable PDFs (tarot standard + Light, poker standard + Light).

## Browse by format — the 10 documents

| Document | Contents | Pages | Size (min..max across 8 languages) | Use |
|---|---|---:|---|---|
| `TarotCards` | Main deck: 15 rules + memo ×7 + 175 fallacies, double-sided **CMYK** | 379 | 57.9..66.9 MB | print shop |
| `TarotCards_Virtues` | Virtues expansion: 131 cards, double-sided CMYK | 262 | 83.1..88.0 MB | print shop |
| `PokerCards` | Scenario deck: 167 cards ×2, 7 backs, CMYK | 334 | 25.9..32.7 MB | print shop |
| `TarotCards_Print&Play_A4` | Tarot Print & Play (A4, double-sided) | 105 | 136.1..147.1 MB | home |
| `TarotCards_Print&Play_Light_A4` | Tarot P&P **Light** (ink-saving) | 21 | 29.7..31.4 MB | home |
| `PokerCards_Print&Play_A4` | Scenario P&P | 38 | 25.7..32.5 MB | home |
| `PokerCards_Print&Play_Light_A4` | Scenario P&P Light | 6 | 6.3..7.4 MB | home |
| `Fallacies_Web_A0` | A0 poster of the 175 fallacies | 1 | 22.6..28.1 MB | poster |
| `Fallacies_Web_A4` | A4 poster | 15 | 22.0..27.5 MB | poster |
| `Fallacies_Web_Thumbnails_A4` | Thumbnail sheet | 9 | 27.0..29.0 MB | reference |

Box volumes: **197 / 364 / 495** cards (manufacturing decision #1187 — detail and qualification in the
release notes). Pagecounts = measured contract 80/80 (dossier-validation-mesures, PR #1409 §C2) — without
conflating the two decks: tarot **379** pages, poker **334**.

## Browse by package type

| Package | Contents | Size | Availability |
|---|---|---|---|
| **Complete** | 80 PDFs, all 8 languages | **3,745.6 MB** MEASURED | `[PLACEHOLDER — URL]` |
| **Per language** | 10 PDFs of one language | 449.3..481.4 MB (table above) | `[PLACEHOLDER — URL ×8]` |
| **Print & Play** | 4 P&P PDFs per language (standard + Light) | 204.7..217.3 MB | `[PLACEHOLDER — URL]` |
| **Mind maps** | Fallacies + Virtues SVGs, 8 languages | `[PLACEHOLDER — measure at packaging (repo SVGs, outside the PDF bundle)]` | `[PLACEHOLDER — URL]` |
| **Ontology** | `argumentum.owl` + `argumentum_virtues.owl` (SKOS + AIF) | `[PLACEHOLDER — measure at packaging]` | `[PLACEHOLDER — URL]` |

Professional-print PDFs are converted to **DeviceCMYK with OutputIntent** (criteria `/GTS_PDFX` +
`CGATS TR 001` of the validation guide ed. 3, #1380); cards at native 300 dpi.

## Tracking & SEO (set at publish)

- **Download analytics**: the page is served by the DNN portal — the GTM/GA carrier is the **DB column
  `Tabs.PageHeadText`** (#1399), which migrates with the database; the probe renders **×2 per page**
  (NanoClaw observation + counter-review, propagated to pools 16/09). Download clicks must be instrumented
  `[PLACEHOLDER — GTM download_click event or DNN equivalent]`.
- **Meta description**: `[PLACEHOLDER — ~150 chars, echoing "8 languages, 80 PDFs, free Print & Play"]`.
- **Sitemap**: add the page URL to the DNN sitemap at publish.

## Publish checklist (gates #134 / #132 / #131)

- [ ] **#134** — `v2.0.0` tag + GitHub Release with **80 fresh assets** (not the Aug 24 pre-release;
      `Print.Play` vs `Print&Play` naming settled).
- [ ] Replace every `[PLACEHOLDER]`: package URLs, analytics event, meta description, mind-maps +
      ontology package sizes (measured at packaging), re-measure zip sizes.
- [ ] Remove the pre-release warning (section above) once v2.0.0 links are live.
- [ ] Update the portal **Downloads** page with these tables (DNN module concerned:
      `[PLACEHOLDER — module/tabs of the Downloads page]`).
- [ ] **Final visual verdict** = jsboige / ai-01 (the worker reports, it does not declare PASS).
- [ ] jsboige review of this draft (#999 DoD 3 condition extended to the Downloads page).

## Translations (mirrors)

FR canonical: [downloads-v2.0.0.fr.md](downloads-v2.0.0.fr.md). The 6 other languages follow at publish
time via the `DatasetUpdater` pipeline (same discipline as #192 — translation then human RTL/CJK validation).

## Sources

- [RELEASE-NOTES-v2.0.0.md](../RELEASE-NOTES-v2.0.0.md) — reader-facing notes + figure qualification annex.
- [cards-catalog.en.md](cards-catalog.en.md) — formats, physical dimensions, volumes (15/09 erratum).
- [news-article-v2.0.0.en.md](news-article-v2.0.0.en.md) — announcement article (aligned checklist).
- `manifest-v2.0.0.md` (bundle `review-v2.0.0-regen-20260912`) — 80 fingerprints, `Status: FINAL`, base `65dd4742`.
- Issue [#135](https://github.com/ArgumentumGames/Argumentum/issues/135) — brief §Downloads (original body
  stale "4 languages"; actual scope = 8, see the announcement article).
- [`134-dossier-validation-mesures.md`](../release-dossier/134-dossier-validation-mesures.md) (PR #1409) —
  pagecount contract C2 + C1 totals (independent corroboration of the sizes).

---

## Annex A — Size qualification (re-measure of 2026-09-16)

| value | status | source |
|---|---|---|
| Per-PDF sizes (80) and per-language totals | **MEASURED** | `find -printf '%s'` on bundle `review-v2.0.0-regen-20260912` (16/09, po-2023 machine, `G:` mount); totals corroborated by dossier-validation-mesures PR #1409 §C1 (3,745.6 MB = manifest to 0.1 MB) |
| Pages per document (contract) | **MEASURED** | dossier-validation-mesures PR #1409 §C2 (80/80, `gswin64c pdfpagecount`) |
| **Zip** sizes | **TO MEASURE** | packaging has not run; sizes above are **bare PDFs** (already Flate-compressed — the expected zip is the same order) |
| Mind maps / ontology sizes | **TO MEASURE** | repo artifacts (outside the PDF bundle), measured at packaging |
| 197 / 364 / 495 | **DECIDED** + arithmetic verified | #1187 c.`5665864605` (carried from the release notes, not re-measured here) |

---

*po-2023 (worker lane) · pool #458 c.`5666259809` grain #999/#135 · preparation only — no tag, no upload,
no publication · visual verdict: ai-01 · review: jsboige.*
