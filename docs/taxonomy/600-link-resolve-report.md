# #600 — `link_*` candidate-URL resolution run (sidecar report)

**Author**: po-2024 (worker) · **Date**: 2026-07-01 · **Base**: master `18b4d023`
**Status**: **SNAPSHOT REPORT** — materializes the ~57 % resolvable ceiling measured by the
[#600](https://github.com/ArgumentumGames/Argumentum/issues/600) §5.1 probe into concrete candidate
URLs, for **human spot-validation** before any write.
**Scope**: docs + 2 sidecar CSVs. **0 write under `Cards/`** (pre-tag freeze). master stays `18b4d023`.
**Tool**: [`tools/link-langlinks-resolve.py`](../../tools/link-langlinks-resolve.py) (PR #610, merged).
**Reproducibility**: re-run `python tools/link-langlinks-resolve.py 0 fallacies --out <path>` (and `virtues`).

---

## TL;DR

Ran the resolver over the full Fallacies + Virtues taxonomy: **2934 candidate `link_<lang>` URLs**
produced from `link_en` via the MediaWiki `langlinks` API, **0 errors across 829 articles probed**.
This is **step 1 of #600 §6** (measure → resolve → ratify → apply) — the candidates now exist as a
reviewable snapshot. The apply step is **gated post-release** (drift-free `#595` + skip non-empty +
human spot-validation of the AR/FA/ZH residue).

| Dataset | Articles probed | Candidate fills | Errors |
|---------|----------------:|----------------:|-------:|
| Fallacies | 741 | **2754** | 0 |
| Virtues | 88 | **180** | 0 |
| **Total** | **829** | **2934** | **0** |

Probe ceiling (#600 §5.1) was **~2919 resolvable cells (57 %)**; this run **materializes 2934** —
the ceiling is met (Virtues adds a few the probe's sample under-counted).

## Sidecars (the reviewable snapshot)

| File | Rows | Purpose |
|------|-----:|---------|
| [`600-link-resolve-fallacies.csv`](600-link-resolve-fallacies.csv) | 2754 | candidate `link_<lang>` URLs for Fallacies nodes |
| [`600-link-resolve-virtues.csv`](600-link-resolve-virtues.csv) | 180 | candidate `link_<lang>` URLs for Virtues nodes |

Schema: `dataset,key,link_lang,resolved_url` (one row per candidate fill). UTF-8, no BOM, LF.

## Per-language breakdown

| Lang | Fallacies | Virtues | Total | Script |
|------|----------:|--------:|------:|--------|
| `ru` | 367 | 4 | 371 | Cyrillic |
| `pt` | 426 | 4 | 430 | Latin |
| `es` | 488 | 25 | 513 | Latin |
| `ar` | 547 | 56 | 603 | Arabic (RTL) |
| `fa` | 434 | 46 | 480 | Persian (RTL) |
| `zh` | 492 | 45 | 537 | CJK |

**Resolves FROM `link_en`** (Wikipedia URLs only). The 433 non-Wikipedia curated sources
(rationalwiki, yourlogicalfallacyis, …) are excluded + preserved as-is (#600 §6.2).

### Asymmetry observed (Virtues)

Virtues `ru`/`pt` = 4 each (very sparse) vs `ar`/`fa`/`zh` = 46–56. Abstract virtue concepts
(e.g. *impartiality*, *universalism*) have thinner RU/PT Wikipedia coverage than AR/FA/ZH. This is a
coverage reality, not a resolver bug — flagged for the spot-validation pass (low RU/PT count = high
value per cell, validate carefully).

## Quality findings

### URL encoding — ✅ correct per script

Spot-check (one resolved URL per language):

| Lang | Sample (decoded) |
|------|------------------|
| `ru` | `ru.wikipedia.org/wiki/Верую,_ибо_абсурдно` (`%D0%92…`) |
| `pt` | `pt.wikipedia.org/wiki/Argumentum_ad_ignorantiam` (Latin, unencoded) |
| `es` | `es.wikipedia.org/wiki/Argumento_ad_ignorantiam` |
| `ar` | `ar.wikipedia.org/wiki/احتكام_إلى_الجهل` (`%D8%A7%D8%AD…`) |
| `fa` | `fa.wikipedia.org/wiki/توسل_به_نادانی` (`%D8%AA%D9%88…`) |
| `zh` | `zh.wikipedia.org/wiki/訴諸無知` (`%E8%A8%B4%E8%AB%B8…`) |

### Homonym scan (#600 §6.4) — ✅ near-zero, no real leaks

Scanned all **1620 AR/FA/ZH candidates** for a Latin (English-homonym) path — the failure mode the
§6.4 warning describes (an English article leaking through for a non-Latin node):

| Lang | Candidates | Latin-path (suspect) | Real homonym? |
|------|-----------:|---------------------:|---------------|
| `ar` | 603 | **0** (0 %) | — |
| `fa` | 480 | **0** (0 %) | — |
| `zh` | 537 | **3** (0.6 %) | **0** — all legitimate loanwords/acronyms |

The 3 ZH Latin-path cases are **not** homonym errors — they are genuine cross-language article titles
that don't translate:

- `FUD` (fallacies key=338 & 920) — acronym, `zh.wikipedia.org/wiki/FUD` is the canonical title.
- `Creepypasta` (fallacies key=927) — loanword, used as-is in `zh.wikipedia.org/wiki/Creepypasta`.

**Conclusion**: the full run surfaces **0 real homonym leaks**. The "Engagement" homonym surfaced in
the PR #610 sample does **not** recur at scale — it was an isolated case (a node whose `link_en`
happened to share a name with an unrelated English article). Spot-validation remains non-optional
(defensive), but the residual risk is materially lower than the §6.4 ceiling suggested.

## Methodology (what the run did)

1. Load each dataset's taxonomy CSV (Fallacies, Virtues).
2. For every node with an `en.wikipedia.org/wiki/<Title>` `link_en` **missing** `link_<lang>`:
   query the MediaWiki `langlinks` API for the `<Title>` → capture the target-language title.
3. Build the resolved URL `https://<lang>.wikipedia.org/wiki/<url-encoded title>`.
4. Emit `dataset,key,link_lang,resolved_url` to the sidecar.

- **Public API**, no key, **0.3 s throttle** (MediaWiki best practice), descriptive User-Agent
  (default `urllib` UA is 403-forbidden — per #610).
- **Robust to network failure**: `try/except` + `continue` (skip on error, never abort). Result:
  0 errors / 829 probes.
- Target languages: `ru, pt, es, ar, fa, zh` (FR = source, EN = the resolution pivot).

## Next step (post-release, gated)

A follow-up PR **consumes** this sidecar:

1. **Apply cell-by-cell** to `Cards/Fallacies/` + `Cards/Virtues/` taxonomy CSVs, drift-free
   (`QUOTE_MINIMAL` + CRLF + UTF-8 no-BOM, method #595).
2. **Skip non-empty cells** (preserve curated links — never overwrite a human-authored `link_<lang>`).
3. **Human spot-validate** the AR/FA/ZH residue (~5 %, ~150 cells priority) before write. The
   native-ratification discipline (#192 `RATIFY` checklist) applies — same gate model.
4. Re-run the [#192 coverage probe](192-link-coverage-langlinks-probe.py) post-apply to confirm the
   gap closed from ~57 % → higher.

> **This is gated**: `Cards/` is frozen (release coupled to DNN #131). The apply PR lands post-tag,
> on jsboige GO, after spot-validation.

## Scope of THIS PR

- ✅ `docs/taxonomy/600-link-resolve-fallacies.csv` — sidecar snapshot (2754 candidates).
- ✅ `docs/taxonomy/600-link-resolve-virtues.csv` — sidecar snapshot (180 candidates).
- ✅ `docs/taxonomy/600-link-resolve-report.md` — this report.
- ✅ **0 write under `Cards/`**, **0 AssetConverter code change** (pre-tag safe).
- ✅ Base `18b4d023`.

Relates to #600, #606, #610 (resolver tool), memory `i18n-coverage-gap-is-link-urls`.
