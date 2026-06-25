# #192 — `link_*` Coverage Research Track

**Author**: po-2024 (worker) · **Date**: 2026-06-25 · **Base**: master `bef3bc6c` (release-frozen)
**Dispatch**: ai-01 2026-06-25 18:58 (`msg-…jp3hx2`, deep-queue v2) — own & research the `link_*` URL coverage gap (non-LLM, non-owned).
**Scope**: research/docs only. **0 write under `Cards/`** (release freeze). master stays `bef3bc6c`.

---

## TL;DR for jsboige

1. **What `link_*` actually is**: every `link_<lang>` cell is a **per-language Wikipedia article URL** (`https://<lang>.wikipedia.org/wiki/…`) — quasi-exclusively. It is the "learn more" link on each card, localized to the reader's language.
2. **The gap is NOT translation and NOT pure human research** — it is **cross-language article resolution**. The gap exists because many fallacies/virtues simply have no dedicated article in the target Wikipedia, or that article isn't Wikidata-linked to the EN one.
3. **Semi-automatable via the MediaWiki `langlinks` API** (no API key, rate-limited). For every node that has `link_en`, the API can return the equivalent article in `<lang>` *if it exists*. This refines memory `i18n-coverage-gap-is-link-urls` ("human research, not gpt-5.5") → it's **neither LLM nor purely human**; it's an API lookup with human validation of the residue.
4. **Measured fillable cells (full census, 2026-06-25)**: of the Wikipedia-type candidate pool, **Fallacies 2 739 / 4 823 (57 %)** and **Virtues 180 / 322 (56 %)** are confirmed resolvable via langlinks — **~2 919 cells total**. The earlier 8 110 figure was a *theoretical ceiling* that over-counted (it included 433 non-Wikipedia `link_en` URLs that langlinks cannot resolve); the measured number is the real, decision-grade value. Residual ~5 % needs human spot-validation (RTL/CJK homonym risk). See §5.1.
5. **Proposal only here** — no CSV write. A bounded follow-up PR (post-release) can run the langlinks enrichment script + human spot-validation. This doc is the methodology + scope so jsboige can decide priority/effort.

---

## 1. Measured state (read-only, re-run via [`192-coverage-report.py`](192-coverage-report.py))

| Dataset | Rows | `link_*` coverage (filled %, where ≠ 100 %) |
|---|---|---|
| **Virtues** | 223 | fr 97 %, en 87 %, ru 88 %, pt 83 %, **es 41 %, ar 69 %, zh 47 %, fa 45 %** |
| **Fallacies** | 1408 | fr 45 %, en 95 %, **ru 9 %, pt 7 %, es 7 %, ar 7 %, zh 6 %, fa 6 %** |
| Scenarii / Rules | — | no `link_*` field (N/A) |

(Text fields are 100 % across 7 langs × 4 datasets — the `link_*` gap is the *only* coverage gap; confirmed by the same coverage report.)

---

## 2. Pattern observed — what `link_*` actually contains

Domain analysis of every *filled* `link_<lang>` cell:

| Field | Dominant content | Notable |
|---|---|---|
| Fallacies `link_fr` | 547/637 `wikipedia-fr` | 79 other-url, 7 fallacy-sites |
| Fallacies `link_en` | 900/1333 `wikipedia-en` | 141 `yourlogicalfallacyis…`, 291 other-url |
| Fallacies `link_{ru,pt,es,ar,zh,fa}` | **100 % `wikipedia-<lang>`** | e.g. `https://ar.wikipedia.org/wiki/مغالطة` |
| Virtues `link_fr` | 161/216 `wikipedia-fr` | 55 other-url |
| Virtues `link_{en,ru,es,fa}` | mostly `wikipedia-<lang>` | small other-url tail |
| Virtues `link_{pt,ar,zh}` | **100 % `wikipedia-<lang>`** | — |

**Conclusion**: `link_<lang>` = the localized Wikipedia article URL. The few `other-url` / `fallacy-site` entries are curated exceptions (e.g. `yourlogicalfallacyis.com` for popular EN fallacies). The model is consistent across both datasets and 7 languages.

---

## 3. Why the gaps differ by language (root cause, not a data-entry bug)

- **EN ~95 %** (Fallacies) / 87 % (Virtues): English Wikipedia is the reference corpus — most fallacies have an EN article. This is why `link_en` is the densest.
- **FR 45 %** (Fallacies): many sub-families / variants have no dedicated FR article (only the parent term does).
- **RU/PT/ES/AR/ZH/FA 6–9 %** (Fallacies): these Wikipedias have far fewer articles for these specific fallacies. The gap is a **Wikipedia content gap**, not a translation gap.

→ This is why an LLM cannot fill it: there is nothing to *translate* — we need the *article that already exists* in the target language, or nothing.

---

## 4. Insight — semi-automation via MediaWiki `langlinks` API

The MediaWiki API exposes cross-language article links (backed by Wikidata):

```
GET https://en.wikipedia.org/w/api.php?action=query&format=json&prop=langlinks
    &titles=Argument_from_ignorance&lllang=fr&lllimit=1
→ { "query": { "pages": { …: { "langlinks": [{"lang":"fr","*":"Appel à l'ignorance"}] } } } }
→ resolve → https://fr.wikipedia.org/wiki/Appel_%C3%A0_l%27ignorance
```

- **No API key required** (public, rate-limited — use a descriptive User-Agent, throttle ~10 req/s).
- Returns the localized article **only if it exists AND is Wikidata-linked**. Otherwise → no link (genuine gap).
- Batchable: `titles=A|B|C` (up to 50/titles per request via `generator`/`titles` piped).

This means the bulk of the "missing `link_<lang>`" cells where `link_en` exists are **machine-resolvable candidates**, not human-research items. Human effort shifts to **validating the residue** (ambiguous matches, disambiguation pages, wrong article).

---

## 5. Fillable-candidate upper bounds (have `link_en`, missing `link_<lang>`)

| Dataset | Nodes with `link_en` | Missing-candidate cells (upper bound) |
|---|---|---|
| **Virtues** | 194 / 223 | **364 total** (ru 8, pt 17, es 49, ar 103, zh 90, fa 97) |
| **Fallacies** | 1333 / 1408 | **8 110 total** (fr 720, ru 1213, pt 1231, es 1228, ar 1231, zh 1245, fa 1242) |

Fallacies nodes **without `link_en` at all**: 75 (not fillable via langlinks — need a different source or stay empty).

**These are ceilings.** Realistic fill ≈ a fraction: langlinks returns nothing when the target-language article doesn't exist. The irreducible remainder is a real content gap in target Wikipedias (out of our control).

---

## 5.1 Measured fill-rate — full census via langlinks (2026-06-25, decision-grade)

The §5 ceilings were theoretical. This section **measures** the real resolvable count by probing every unique candidate article against the MediaWiki `langlinks` API. Only `en.wikipedia.org/wiki/<Title>` URLs qualify (433 non-Wikipedia `link_en` URLs — rationalwiki, logicallyfallacious, etc. — are excluded; they are a curated category preserved as-is).

Script: [`192-link-coverage-langlinks-probe.py`](192-link-coverage-langlinks-probe.py) — read-only, no API key, ~0.3 s throttle, descriptive User-Agent (MediaWiki 403s the default urllib UA). Census run = 0 errors on 741 (Fallacies) + 88 (Virtues) articles.

### Fallacies — 1 408 rows

| `link_<lang>` | candidate cells missing | confirmed resolvable | rate |
|---|---|---|---|
| ru | 790 | 366 | 46 % |
| pt | 803 | 426 | 53 % |
| es | 800 | 488 | 61 % |
| ar | 803 | 533 | 66 % |
| fa | 813 | 434 | 53 % |
| zh | 814 | 492 | 60 % |
| **total** | **4 823** | **2 739** | **57 %** |

`link_en` categorization: 900 Wikipedia URLs (resolvable pool) · 433 non-Wikipedia (excluded) · 75 empty.

### Virtues — 223 rows

| `link_<lang>` | candidate cells missing | confirmed resolvable | rate |
|---|---|---|---|
| ru | 5 | 4 | 80 % |
| pt | 9 | 4 | 44 % |
| es | 44 | 25 | 57 % |
| ar | 94 | 56 | 60 % |
| fa | 89 | 46 | 52 % |
| zh | 81 | 45 | 56 % |
| **total** | **322** | **180** | **56 %** |

`link_en` categorization: 185 Wikipedia URLs · 9 non-Wikipedia · 29 empty.

### Decision readout

- **Combined measured fillable ≈ 2 919 cells (57 % of the Wikipedia-type candidate pool).** This is the real number — it supersedes the 8 110 ceiling for prioritization.
- **Per-lang signal**: AR/ZH are the densest (60-66 %) — highest return on a fill pass; RU/PT/FA mid (44-53 %). The gap is not uniform; a fill pass yields more for RTL/CJK than for Cyrillic.
- **Why it's "57 %, not 100 %"**: the missing 43 % is a genuine *Wikipedia content gap* — those fallacies simply have no article in the target language, or it isn't Wikidata-linked. Unfixable by us. langlinks reports it honestly (no match).
- **Honesty caveat**: the 2 919 (2 739 Fallacies + 180 Virtues) are *confirmed resolvable*; writing them still needs the §6 method (skip non-empty cells, preserve curated sources, QUOTE_MINIMAL + CRLF) + ~5 % human spot-validation for AR/FA/ZH homonym risk. So **~2 770 cells are realistically auto-fillable** after validation attrition.

---

## 6. Proposed fill methodology (for the follow-up PR, post-release)

1. **Resolve via `langlinks`**: for each node with `link_en`, query the API for each target `<lang>`; if an equivalent exists, write `https://<lang>.wikipedia.org/wiki/<resolved_title>`.
2. **Curated sources fallback** (for the EN-dense cases already using non-Wikipedia): preserve existing `other-url` / `fallacy-site` entries — don't overwrite curated links with a generic Wikipedia link.
3. **Nodes without `link_en`**: leave empty or fall back to a parent-term article (judgment call per node) — flag for human curation.
4. **Human spot-validation**: sample ~5 % of filled cells, verify the resolved article is the *right* concept (not a disambiguation page or a homonym). Critical for AR/FA/ZH where title matching can misfire.
5. **CSV safety**: cell-level `QUOTE_MINIMAL` + CRLF, UTF-8 no-BOM — same drift-free method as #595. Re-run coverage report → confirm fill rose.

**Effort estimate**: script is bounded (~1 day incl. rate-limit handling + validation harness); execution ~minutes for the ~2 919 measured resolvable cells. Human validation of the ~5 % residue = the real cost (~150 cells to eyeball, AR/FA/ZH priority).

---

## 7. Authoritative source candidates (per need)

| Need | Source |
|---|---|
| Localized article (primary) | `<lang>.wikipedia.org` via `langlinks` |
| Cross-language hub (canonical concept → all langs) | **Wikidata** (`wbgetentities`, `sitelinks`) — more complete than langlinks for rare terms |
| EN popular-fallacy depth | `yourlogicalfallacyis.com` (already used for 141 EN cells) |
| Academic depth (virtues, rare fallacies) | **Stanford Encyclopedia of Philosophy** (`plato.stanford.edu`) — EN, but authoritative for concepts lacking a Wikipedia article |
| Disambiguation aid | Wikidata labels/descriptions per lang |

---

## 8. Honesty / caveats

- **Upper bounds, not guarantees**: the 364 / 8 110 figures count *candidates* (have `link_en`, missing target). Many will return no langlink (article doesn't exist in target Wikipedia) → stay empty. The doc must not be read as "we can fill 8 110 cells".
- **Disambiguation / homonym risk**: langlinks returns *a* title; for AR/FA/ZH it can be a homonym or disambiguation page. Human spot-validation (§6.4) is non-optional, not nice-to-have.
- **No overwrite of curated links**: the 141 `yourlogicalfallacyis` + `other-url` EN entries are intentional. The fill script must skip non-empty cells.
- **Not in #192 LLM scope**: this refines memory `i18n-coverage-gap-is-link-urls` — the gap is *API-resolvable + human-validated*, not gpt-5.5 batch translation. Separate track from the #192 terminology harmonization (PR #598).
- **Release-safe**: this is a proposal only. 0 CSV written, master stays `bef3bc6c`.

---

## 9. Reproducibility

Re-run the measurement anytime from repo root:
- `python docs/taxonomy/192-coverage-report.py` — per-field × per-lang fill + `link_*` gaps.
- `python docs/taxonomy/192-link-coverage-langlinks-probe.py` — **the measured fill-rate census** (§5.1). Default = full probe of all candidate articles (Fallacies 741 + Virtues 88); pass `50` for a strided sample, `0 virtues` / `0 fallacies` for one dataset. ~0.3 s throttle, no API key, descriptive User-Agent required (MediaWiki 403s the default urllib UA).

---

*This track owns the `link_*` gap so it's no longer "known but unowned". It converts an open-ended "fill the URLs" into a bounded methodology (API resolve → human validate residue) with a measured scope — ready for jsboige to prioritize relative to the WE release decisions.*
