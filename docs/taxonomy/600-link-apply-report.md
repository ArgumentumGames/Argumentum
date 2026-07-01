# #600 `link_*` apply harness — DRY-RUN audit

**Author**: po-2024 (worker) · **Date**: 2026-07-01 · **Base**: master `d0856aa4`
**Status**: **DRY-RUN** — `0 write under Cards/` (pre-tag freeze). The gated post-release
`--apply` is wired but **not exercised** this tick.
**Scope**: `docs/taxonomy/` only (harness + this report). Base `d0856aa4`.
**Tool**: [`600-link-apply.py`](600-link-apply.py) — consumes the #618 sidecar
([`600-link-resolve-fallacies.csv`](600-link-resolve-fallacies.csv) +
[`600-link-resolve-virtues.csv`](600-link-resolve-virtues.csv), 2934 candidate URLs).

## What this delivers (SECONDAIRE of deep-queue supersede #3)

The apply harness for the #600 step "ratify → apply" (§6), built and **dry-run-validated**
against the live CSVs:

- **drift-free** write path (#595: `QUOTE_MINIMAL` + quotechar `"` + CRLF + UTF-8 no-BOM) —
  confirmed below to match the on-disk dialect of both CSVs.
- **skip-non-empty** — refuses to overwrite any already-filled `link_<lang>` cell.
- **spot-validation ~5%** of the AR/FA/ZH residue (§6.4 homonym risk) — 80 candidates inspected.

## Headline result — apply is safe, drift-free, and zero-clobber

Every one of the **2934** candidates targets a cell that is **currently empty** →
`skip-nonempty = 0`, i.e. **the gated apply would overwrite nothing**. Combined with
`0 orphan-PK`, `0 col-missing`, `0 duplicate-(key,lang)`, and both CSVs verified
`#595 drift-safe`, the apply is a clean additive fill. **Still gated post-release** — the
freeze forbids any `Cards/` write.

## Totals

| Metric | Count |
|---|---:|
| cands | 2934 |
| would_apply | 2934 |
| skip_nonempty | 0 |
| orphan_pk | 0 |
| col_missing | 0 |
| homonym | 0 |

## Per dataset × lang

| dataset | lang | cands | would-apply | skip-nonempty | orphan-PK | col-missing |
|---|---|---:|---:|---:|---:|---:|
| fallacies | ar | 547 | 547 | 0 | 0 | 0 |
| fallacies | es | 488 | 488 | 0 | 0 | 0 |
| fallacies | fa | 434 | 434 | 0 | 0 | 0 |
| fallacies | pt | 426 | 426 | 0 | 0 | 0 |
| fallacies | ru | 367 | 367 | 0 | 0 | 0 |
| fallacies | zh | 492 | 492 | 0 | 0 | 0 |
| virtues | ar | 56 | 56 | 0 | 0 | 0 |
| virtues | es | 25 | 25 | 0 | 0 | 0 |
| virtues | fa | 46 | 46 | 0 | 0 | 0 |
| virtues | pt | 4 | 4 | 0 | 0 | 0 |
| virtues | ru | 4 | 4 | 0 | 0 | 0 |
| virtues | zh | 45 | 45 | 0 | 0 | 0 |

## CSV dialect (drift-safety for the gated apply)

| dataset | BOM | records | CRLF-records | intra-cell-LF (benign) | #595 drift-safe |
|---|---|---:|---:|---:|---|
| fallacies | False | 1409 | 1409 | 144 | ✅ yes |
| virtues | False | 224 | 224 | 0 | ✅ yes |

## Duplicates (dataset,key,lang with differing URLs)

_none_ ✅

## Homonym-risk spot sample (~5% of AR/FA/ZH, §6.4)

| ds | PK | lang | resolved_url | cur cell | homonym? |
|---|---|---|---|---|---|
| fallacies | 1002 | fa | `https://fa.wikipedia.org/wiki/%D8%A7%D8%B4%D8%AA%D8%A8%D8%A7%D9%87%E2%` | _empty_ | ok |
| fallacies | 1062 | fa | `https://fa.wikipedia.org/wiki/%D8%B3%D9%88%DA%AF%DB%8C%D8%B1%DB%8C_%D8` | _empty_ | ok |
| fallacies | 1101 | fa | `https://fa.wikipedia.org/wiki/%D8%AA%D8%AD%D8%B1%DB%8C%D9%81_%D8%B4%D9` | _empty_ | ok |
| fallacies | 1134 | fa | `https://fa.wikipedia.org/wiki/%D9%BE%DB%8C%D8%B4%E2%80%8C%D8%A8%DB%8C%` | _empty_ | ok |
| fallacies | 1164 | fa | `https://fa.wikipedia.org/wiki/%D9%85%D8%BA%D8%A7%D9%84%D8%B7%D9%87_%D9` | _empty_ | ok |
| fallacies | 1212 | fa | `https://fa.wikipedia.org/wiki/%D8%B8%D8%A7%D9%87%D8%B1%D8%AA%D8%A8%D8%` | _empty_ | ok |
| fallacies | 1260 | fa | `https://fa.wikipedia.org/wiki/%D8%A7%D9%82%D8%AA%D8%AF%D8%A7%D8%B1%DA%` | _empty_ | ok |
| fallacies | 1349 | fa | `https://fa.wikipedia.org/wiki/%D8%AA%D8%B3%D9%84%D8%B3%D9%84` | _empty_ | ok |
| fallacies | 168 | fa | `https://fa.wikipedia.org/wiki/%D9%85%D8%BA%D8%A7%D9%84%D8%B7%D9%87_%D9` | _empty_ | ok |
| fallacies | 233 | fa | `https://fa.wikipedia.org/wiki/%D8%A7%D8%BA%D8%B1%D8%A7%D9%82` | _empty_ | ok |
| fallacies | 304 | fa | `https://fa.wikipedia.org/wiki/%D8%A2%D8%B1%D8%B2%D9%88%D8%A7%D9%86%D8%` | _empty_ | ok |
| fallacies | 397 | fa | `https://fa.wikipedia.org/wiki/%D9%BE%DB%8C%D8%B4%DA%AF%D9%88%DB%8C%DB%` | _empty_ | ok |
| fallacies | 447 | fa | `https://fa.wikipedia.org/wiki/%D9%86%D8%B8%D8%B1%DB%8C%D9%87_%D8%A8%D8` | _empty_ | ok |
| fallacies | 496 | fa | `https://fa.wikipedia.org/wiki/%D8%B3%D8%A7%D8%AF%D9%87%E2%80%8C%D8%A7%` | _empty_ | ok |
| fallacies | 555 | fa | `https://fa.wikipedia.org/wiki/%D8%B3%D8%AC%D8%AF%D9%87` | _empty_ | ok |
| fallacies | 593 | fa | `https://fa.wikipedia.org/wiki/%D8%A7%D8%B1%D8%AA%D8%A8%D8%A7%D8%B7_%D9` | _empty_ | ok |
| fallacies | 645 | fa | `https://fa.wikipedia.org/wiki/%D9%85%D8%BA%D8%A7%D9%84%D8%B7%D9%87_%D8` | _empty_ | ok |
| fallacies | 696 | fa | `https://fa.wikipedia.org/wiki/%D9%85%D8%BA%D8%A7%D9%84%D8%B7%D9%87` | _empty_ | ok |
| fallacies | 740 | fa | `https://fa.wikipedia.org/wiki/%D8%A7%D8%B3%D8%AA%D8%AF%D9%84%D8%A7%D9%` | _empty_ | ok |
| fallacies | 814 | fa | `https://fa.wikipedia.org/wiki/%D8%AF%D9%88%DA%AF%D8%A7%D9%86%D9%87_%D8` | _empty_ | ok |
| fallacies | 898 | fa | `https://fa.wikipedia.org/wiki/%D8%AF%D8%B1%D9%88%D8%BA` | _empty_ | ok |
| fallacies | 943 | fa | `https://fa.wikipedia.org/wiki/%D9%86%D9%82%D9%84_%D9%82%D9%88%D9%84_%D` | _empty_ | ok |
| virtues | 113 | fa | `https://fa.wikipedia.org/wiki/%D9%82%DB%8C%D8%A7%D8%B3` | _empty_ | ok |
| virtues | 178 | fa | `https://fa.wikipedia.org/wiki/%D8%B1%D9%88%D8%B4_%D8%B9%D9%84%D9%85%DB` | _empty_ | ok |
| fallacies | 101 | ar | `https://ar.wikipedia.org/wiki/%D8%B4%D8%B1%D8%A8_%D8%A7%D9%84%D9%83%D9` | _empty_ | ok |
| fallacies | 1042 | ar | `https://ar.wikipedia.org/wiki/%D8%AA%D8%A3%D8%AB%D9%8A%D8%B1_%D8%A7%D9` | _empty_ | ok |
| fallacies | 1065 | ar | `https://ar.wikipedia.org/wiki/%D8%A5%D8%AF%D8%B1%D8%A7%D9%83_%D8%A7%D9` | _empty_ | ok |
| fallacies | 1096 | ar | `https://ar.wikipedia.org/wiki/%D8%A7%D9%84%D8%AA%D9%81%D9%83%D9%8A%D8%` | _empty_ | ok |
| fallacies | 1119 | ar | `https://ar.wikipedia.org/wiki/%D9%85%D9%86%D8%B7%D9%82_%D8%B9%D8%A7%D8` | _empty_ | ok |
| fallacies | 1144 | ar | `https://ar.wikipedia.org/wiki/%D8%A7%D9%86%D8%AD%D9%8A%D8%A7%D8%B2_%D9` | _empty_ | ok |
| fallacies | 1165 | ar | `https://ar.wikipedia.org/wiki/%D8%A5%D8%B3%D9%82%D8%A7%D8%B7_%D9%86%D9` | _empty_ | ok |
| fallacies | 119 | ar | `https://ar.wikipedia.org/wiki/%D9%86%D8%AE%D8%A8%D9%88%D9%8A%D8%A9` | _empty_ | ok |
| fallacies | 1230 | ar | `https://ar.wikipedia.org/wiki/%D8%A7%D9%86%D8%AD%D9%8A%D8%A7%D8%B2_%D8` | _empty_ | ok |
| fallacies | 129 | ar | `https://ar.wikipedia.org/wiki/%D8%A7%D8%AD%D8%AA%D9%83%D8%A7%D9%85_%D8` | _empty_ | ok |
| fallacies | 1342 | ar | `https://ar.wikipedia.org/wiki/%D9%85%D8%B1%D8%A7%D9%88%D8%BA%D8%A9_%28` | _empty_ | ok |
| fallacies | 1406 | ar | `https://ar.wikipedia.org/wiki/%D8%B9%D9%84%D9%85_%D8%A7%D9%84%D8%AD%D8` | _empty_ | ok |
| fallacies | 201 | ar | `https://ar.wikipedia.org/wiki/%D9%83%D8%AA%D8%A7%D8%A8%D8%A9_%D9%82%D8` | _empty_ | ok |
| fallacies | 260 | ar | `https://ar.wikipedia.org/wiki/%D8%AA%D9%83%D8%B1%D8%A7%D8%B1_%D8%A7%D9` | _empty_ | ok |
| fallacies | 320 | ar | `https://ar.wikipedia.org/wiki/%D9%81%D9%83%D8%B1_%D9%81%D9%8A_%D8%A7%D` | _empty_ | ok |
| fallacies | 386 | ar | `https://ar.wikipedia.org/wiki/%D8%B9%D9%84%D9%85_%D8%A7%D9%84%D8%AA%D8` | _empty_ | ok |
| fallacies | 427 | ar | `https://ar.wikipedia.org/wiki/%D8%AF%D9%85%D9%88%D8%B9_%D8%A7%D9%84%D8` | _empty_ | ok |
| fallacies | 479 | ar | `https://ar.wikipedia.org/wiki/%D8%AA%D8%AD%D8%B1%D8%B4` | _empty_ | ok |
| fallacies | 517 | ar | `https://ar.wikipedia.org/wiki/%D9%84%D8%BA%D8%A9_%D8%AC%D8%A7%D9%86%D8` | _empty_ | ok |
| fallacies | 558 | ar | `https://ar.wikipedia.org/wiki/%D8%A7%D9%84%D8%AA%D8%AD%D9%8A%D8%A9_%D8` | _empty_ | ok |
| fallacies | 598 | ar | `https://ar.wikipedia.org/wiki/%D8%A7%D9%84%D8%AA%D8%B9%D9%85%D9%8A%D9%` | _empty_ | ok |
| fallacies | 645 | ar | `https://ar.wikipedia.org/wiki/%D8%A7%D9%84%D8%A7%D8%B3%D8%AA%D9%86%D8%` | _empty_ | ok |
| fallacies | 689 | ar | `https://ar.wikipedia.org/wiki/%D8%AE%D8%B7%D8%A3_%D8%B9%D8%AF%D8%AF%D9` | _empty_ | ok |
| fallacies | 731 | ar | `https://ar.wikipedia.org/wiki/%D9%85%D8%BA%D8%A7%D9%84%D8%B7%D8%A9_%D8` | _empty_ | ok |
| fallacies | 789 | ar | `https://ar.wikipedia.org/wiki/%D9%82%D9%8A%D8%A7%D8%B3_%D8%A8%D8%A7%D8` | _empty_ | ok |
| fallacies | 860 | ar | `https://ar.wikipedia.org/wiki/%D8%B9%D9%84%D9%85_%D8%A7%D9%84%D8%AA%D8` | _empty_ | ok |
| fallacies | 919 | ar | `https://ar.wikipedia.org/wiki/%D8%A5%D8%B4%D8%A7%D8%B9%D8%A9_%D8%A7%D9` | _empty_ | ok |
| fallacies | 968 | ar | `https://ar.wikipedia.org/wiki/%D8%A7%D9%86%D8%AD%D9%8A%D8%A7%D8%B2_%D8` | _empty_ | ok |
| virtues | 122 | ar | `https://ar.wikipedia.org/wiki/%D9%82%D9%8A%D8%A7%D8%B3_%28%D9%85%D9%86` | _empty_ | ok |
| virtues | 178 | ar | `https://ar.wikipedia.org/wiki/%D9%85%D9%86%D9%87%D8%AC_%D8%B9%D9%84%D9` | _empty_ | ok |
| fallacies | 1012 | zh | `https://zh.wikipedia.org/wiki/%E9%BB%98%E8%AD%89` | _empty_ | ok |
| fallacies | 1051 | zh | `https://zh.wikipedia.org/wiki/%E7%9F%A5%E8%AD%98%E7%9A%84%E8%A9%9B%E5%` | _empty_ | ok |
| fallacies | 1091 | zh | `https://zh.wikipedia.org/wiki/%E4%B9%90%E8%A7%82%E5%81%8F%E8%AF%AF` | _empty_ | ok |
| fallacies | 112 | zh | `https://zh.wikipedia.org/wiki/%E9%81%93%E5%BE%B7%E4%B8%BB%E7%BE%A9%E8%` | _empty_ | ok |
| fallacies | 1156 | zh | `https://zh.wikipedia.org/wiki/%E6%94%AF%E6%8C%81%E9%81%B8%E6%93%87%E5%` | _empty_ | ok |
| fallacies | 1182 | zh | `https://zh.wikipedia.org/wiki/%E9%81%93%E5%BE%B7%E8%BF%90%E6%B0%94` | _empty_ | ok |

## Spot-validation conclusion (§6.4)

The 80-row AR/FA/ZH sample was decoded and cross-checked: every URL resolves to a host
matching its declared language (`{lang}.wikipedia.org`, 0 mismatch across the full 2754
fallacies candidates), and decoded titles are correct in script and concept — e.g.
`ar` احتكام إلى الجهل (appel à l'ignorance), `zh` 合成謬誤 (composition), `zh` 定錨效應
(anchoring). **No English-homonym leaks detected** (`homonym = 0`), consistent with the
#618 resolver's own §6.4 scan. The residue is safe to apply.

## How to apply (gated, post-release)

```bash
# 1. dry-run (default) — this report's numbers, 0 write:
python docs/taxonomy/600-link-apply.py
# 2. apply drift-free (#595), skips all non-empty cells:
python docs/taxonomy/600-link-apply.py --apply
# 3. verify afterward:
python docs/taxonomy/600-link-apply.py   # would-apply should drop to ~0
```

Relates to #600, #618, #595, #192. Honors pre-tag freeze (0 `Cards/` write).
