# Downloads page — Argumentum v2.0.0 (EN mirror of the canonical FR draft)

> **Status: DRAFT (prep #135 §Downloads, pool #458 po-2023 grain; aligned with #2234 and the settled
> correspondence [c.6650903268](https://github.com/ArgumentumGames/Argumentum/issues/135#issuecomment-6650903268)
> by P24-03, 21 Nov 2026).** Preparation of the DNN site **Downloads** page for the **v2.0.0** release
> (re-scope v0.9.0 → v2.0.0, jsboige 2026-08-06, #999). Canonical version:
> [downloads-v2.0.0.fr.md](downloads-v2.0.0.fr.md). **Publication is GATED** on #134 (GitHub Release tag),
> #132 (prod deployment) and #131 (DNN 10.3.2 live) — see "Publish checklist". **jsboige review required
> before any publish.** This file prepares the text; it publishes nothing.
>
> **Source of truth:** [RELEASE-NOTES-v2.0.0.md](../RELEASE-NOTES-v2.0.0.md) (reader notes, re-measured
> figures + qualification annex) · [cards-catalog.en.md](cards-catalog.en.md) (formats, volumes) ·
> [news-article-v2.0.0.en.md](news-article-v2.0.0.en.md) (the announcement article points to this page) ·
> **[135-downloads-assets-table.md](135-downloads-assets-table.md) — the 80 published-name table, DERIVED
> from `WebBasedGeneratorConfig.cs` and confronted with the release (PR #2234)** · `manifest-v2.0.0.md`
> of bundle `review-v2.0.0-regen-20260912` (80 sha256 fingerprints, `Status: FINAL`, base `65dd4742`).
> `[PLACEHOLDER]` fields are filled at tag time.
>
> **Naming SETTLED**: `Print.Play` is the **published form** (GitHub rewrites `&` → `.`; 32/32 published
> P&P PDFs measured `Print.Play`, zero `&` — #1491 dossier). Links point to the **final release** (#134),
> **never** to a pre-release (`v0.9.0-review`, `v2.1.0-review`).
>
> **Sizes**: all PDF sizes below are **MEASURED 16/09** on the final 12/09 bundle (qualification in
> Annex A). **Zip archive** sizes will be re-measured at packaging (a PDF is already Flate-compressed;
> the zip will not change the order of magnitude, only the values).

---

## Pre-release warning (remove at tag time)

⚠️ **Until the `v2.0.0` tag**, do not redistribute pre-release assets: `v0.9.0-review` dates from
Aug 24-25 and predates the September corrections (detailed state in the
[drift dossier](../release-dossier/134-release-assets-drift-2026-09-15.md), PR #1383); `v2.1.0-review`
is not the approved lot. The page must reference **only** the fresh assets uploaded at the tag (checklist).

## Browse by language — the complete material, 8 times over

Every language ships the **complete** material: **10 PDF documents** (published names in the
[derived table #2234](135-downloads-assets-table.md) — 10 stems × 8 languages = 80 assets), plus **two
packages** built at tag time by [`tools/135-build-download-zips.py`](../../tools/135-build-download-zips.py)
and attached to the final release:

- **Digital cards** — `Argumentum_Cards_PNG_<lang>.zip`: the **175** cards as PNG, rebuilt from the same
  generation tree as the release (the PNGs currently served exist in French only and count 157 cards —
  doubly stale);
- **Mind maps** — `Argumentum_MindMaps_<lang>.zip`: the Fallacies **and** Virtues SVGs of the language,
  taken from the repo (`Cards/Fallacies/Mindmaps/<lang>/`), plus a link to the site page displaying them.

| Language | Complete material (10 PDFs) | of which Print & Play only | Documents |
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
There is **no "complete language" zip**: every PDF is served individually from the release.

## Browse by format — the 10 documents

Names from the derived table #2234 (**published** form, `Print.Play`):

| Document | Contents | Pages | Size (min..max across 8 languages) | Use |
|---|---|---:|---|---|
| `Argumentum_TarotCards` | Main deck: 15 rules + memo ×7 + 175 fallacies, double-sided **CMYK** | 379 | 57.9..66.9 MB | print shop |
| `Argumentum_TarotCards_Virtues` | Virtues expansion: 131 cards, double-sided CMYK | 262 | 83.1..88.0 MB | print shop |
| `Argumentum_PokerCards` | Scenario deck: 167 cards ×2, 7 backs, CMYK | 334 | 25.9..32.7 MB | print shop |
| `Argumentum_TarotCards_Print.Play_A4` | Tarot Print & Play (A4, double-sided) | 105 | 136.1..147.1 MB | home |
| `Argumentum_TarotCards_Print.Play_Light_A4` | Tarot P&P **Light** (ink-saving) | 21 | 29.7..31.4 MB | home |
| `Argumentum_PokerCards_Print.Play_A4` | Scenario P&P | 38 | 25.7..32.5 MB | home |
| `Argumentum_PokerCards_Print.Play_Light_A4` | Scenario P&P Light | 6 | 6.3..7.4 MB | home |
| `Argumentum_Fallacies_Web_A0` | A0 poster of the 175 fallacies | 1 | 22.6..28.1 MB | poster |
| `Argumentum_Fallacies_Web_A4` | A4 poster | 15 | 22.0..27.5 MB | poster |
| `Argumentum_Fallacies_Web_Thumbnails_A4` | Thumbnail sheet | 9 | 27.0..29.0 MB | reference |

Each document declines as `<stem>_<lang>.pdf` (e.g. `Argumentum_TarotCards_Print.Play_A4_en.pdf`).

Box volumes: **197 / 364 / 495** cards (manufacturing decision #1187 — detail and qualification in the
release notes). Pagecounts = measured contract 80/80 (dossier-validation-mesures, PR #1409 §C2) — without
conflating the two decks: tarot **379** pages, poker **334**.

## Browse by package type

Correspondence **settled** ([c.6650903268](https://github.com/ArgumentumGames/Argumentum/issues/135#issuecomment-6650903268),
Nov 5) — what the page offers today keeps its nature, updated and declined over the 8 languages:

| Package | Contents | Size | Availability |
|---|---|---|---|
| **Complete** | 80 PDFs, all 8 languages — no global zip | **3,745.6 MB** MEASURED | per-language links above (each PDF on the final release) |
| **Per language** | 10 PDFs + cards zip + mind-maps zip of the language | 449.3..481.4 MB (PDFs alone) | `[PLACEHOLDER — final release #134 URLs]` |
| **Print & Play** | 4 P&P PDFs per language (standard + Light) | 204.7..217.3 MB | `Argumentum_{Tarot,Poker}Cards_Print.Play[_Light]_A4_<lang>.pdf` |
| **A0 poster** | poster of the 175 fallacies | 22.6..28.1 MB | `Argumentum_Fallacies_Web_A0_<lang>.pdf` (replaces the old French-only zip) |
| **A4 sheets** | A4 poster + thumbnail sheet | 22.0..29.0 MB | `Argumentum_Fallacies_Web_{A4,Thumbnails_A4}_<lang>.pdf` |
| **Digital cards** | 175 PNG cards per language | `[PLACEHOLDER — measure at packaging]` | `Argumentum_Cards_PNG_<lang>.zip` (built at tag, attached to the release) |
| **Light cards** | 175 light-density PNGs | `[PLACEHOLDER — to measure]` | `Argumentum_Cards_PNG_light_<lang>.zip` **if** the release tree carries that density; otherwise the package disappears, replaced by the `Argumentum_Fallacies_Web_Thumbnails_A4_<lang>.pdf` sheet |
| **Mind maps** | Fallacies + Virtues SVGs per language | `[PLACEHOLDER — measure at packaging (repo SVGs)]` | `Argumentum_MindMaps_<lang>.zip` + link to the site page displaying them |
| **Ontology — fallacies** | OWL of the 175 fallacies (SKOS + AIF), **served by the site** | `[PLACEHOLDER — size to measure]` | `/argumentum_fallacies.owl` (site root) |
| **Ontology — virtues** | OWL of the virtues (SKOS + AIF), **served by the site** | `[PLACEHOLDER — size to measure]` | `/argumentum_virtues.owl` (site root) |

> OWL link (#133 option A, rehearsed on préprod Nov 5 — runbook §6): the served name comes from the
> **IRI** declared by the blob, not the repo filename (`argumentum.owl` → served `argumentum_fallacies.owl`).
> Version and date stay blank until the tag.
>
> The 6 old zips of the downloads directory (18/08, French) are no longer linked; ⛔ none is deleted
> without a withdrawal decision backed by a preservation proof (c.6650903268). The "Argmentum" typo
> disappears with the new names.

Professional-print PDFs are converted to **DeviceCMYK with OutputIntent** (criteria `/GTS_PDFX` +
`CGATS TR 001` of the validation guide ed. 3, #1380); cards at native 300 dpi.

## Tracking & SEO (set at publish)

- **Download analytics**: the page is served by the DNN portal — the GTM/GA carrier is the **DB column
  `Tabs.PageHeadText`** (#1399), which migrates with the database; the probe renders **×2 per page**
  (NanoClaw observation + counter-review, propagated to pools 16/09). Download clicks must be instrumented
  `[PLACEHOLDER — GTM download_click event or DNN equivalent]`.
- **Meta description**: `[PLACEHOLDER — ~150 chars, echoing "8 languages, 80 PDFs, free Print & Play"]`.
- **Sitemap**: add the page URL to the DNN sitemap at publish. Share tags (`og:`, `twitter:card`)
  currently absent (measured baseline 19/11, #135 P23-04) — gesture to be arbitrated in thread #135.

## Publish checklist (gates #134 / #132 / #131)

- [ ] **#134** — `v2.0.0` tag + GitHub Release with **80 fresh assets** (not a pre-release;
      `Print.Play` naming, settled).
- [ ] **Zips built at tag** (`tools/135-build-download-zips.py`): `Argumentum_Cards_PNG_<lang>.zip`
      ×8 (+ `light` ×8 **if** the tree carries it) and `Argumentum_MindMaps_<lang>.zip` ×8, attached to
      the final release (po-2023 builds, ai-01 attaches — c.6650903268), then their sizes recorded.
- [ ] Replace every `[PLACEHOLDER]`: package URLs, analytics event, meta description, cards + mind-maps +
      ontology package sizes (measured at packaging), re-measure zip sizes.
- [ ] Remove the pre-release warning (section above) once v2.0.0 links are live.
- [ ] Update the portal **Downloads** page with these tables (DNN module concerned:
      `[PLACEHOLDER — module/tabs of the Downloads page]`).
- [ ] **Control** (c.6650903268): the page no longer serves any zip from the old directory; every link
      resolves 200 to a published asset of the final release; the served size equals the asset size.
- [ ] **Final visual verdict** = jsboige / ai-01 (the worker reports, it does not declare PASS).
- [ ] jsboige review of this draft (#999 DoD 3 condition extended to the Downloads page), including
      human RTL/CJK validation of the 6 mirrors.

## Translations (mirrors)

FR canonical: [downloads-v2.0.0.fr.md](downloads-v2.0.0.fr.md). The **6 other mirrors exist**
(P24-03, 21 Nov): [ru](downloads-v2.0.0.ru.md) · [pt](downloads-v2.0.0.pt.md) · [es](downloads-v2.0.0.es.md) ·
[ar](downloads-v2.0.0.ar.md) · [fa](downloads-v2.0.0.fa.md) · [zh](downloads-v2.0.0.zh.md). Asset names
are identical across the 8 files (proper nouns); human RTL/CJK validation before publish (same discipline
as #192).

## Sources

- [RELEASE-NOTES-v2.0.0.md](../RELEASE-NOTES-v2.0.0.md) — reader-facing notes + figure qualification annex.
- [cards-catalog.en.md](cards-catalog.en.md) — formats, physical dimensions, volumes (15/09 erratum).
- [news-article-v2.0.0.en.md](news-article-v2.0.0.en.md) — announcement article (aligned checklist).
- [135-downloads-assets-table.md](135-downloads-assets-table.md) — **the 80 published names + 24 zips
  table, derived from the config and confronted** (PR #2234).
- `manifest-v2.0.0.md` (bundle `review-v2.0.0-regen-20260912`) — 80 fingerprints, `Status: FINAL`, base `65dd4742`.
- Issue [#135](https://github.com/ArgumentumGames/Argumentum/issues/135) — brief §Downloads (original body
  stale "4 languages"; actual scope = 8, see the announcement article; correspondence settled c.6650903268).
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
| Published names (80 PDFs + 24 zips) | **DERIVED** | PR #2234 (`WebBasedGeneratorConfig.cs` + tool code, `Print.Play` form) |

---

*po-2023 (worker lane) · pool #458 c.`5666259809` grain #999/#135 · P24-03 alignment (po-2024, 21 Nov) ·
preparation only — no tag, no upload, no publication · visual verdict: ai-01 · review: jsboige.*
