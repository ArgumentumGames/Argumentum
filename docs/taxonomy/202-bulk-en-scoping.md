# #202 — Bulk-EN translation scoping (read-only)

**Author**: po-2024 (worker) · **Date**: 2026-07-01 · **Base**: master `18b4d023`
**Trigger**: ai-01 deep-queue supersede (`msg-...370u0q`, TERTIAIRE) — "caractérise le scope bulk EN restant (read-only, scoping only, pas d'exécution trad)".
**Method**: read-only census on the live taxonomy CSVs (code = truth). **0 write `Cards/`, 0 code change.** master stays `18b4d023`.
**Reproducibility**: the census snippets below re-run verbatim against `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`.

---

## TL;DR — #202's "bulk EN" is a **stale dispatch**

The issue [#202](https://github.com/ArgumentumGames/Argumentum/issues/202) (epic, written when the project was 4-lang) frames a large "Phase 2 bulk translation" backlog — e.g. *"Fill `Simple_name_en` (1348 empty), `political_example_en/ru/pt` (1373 empty each)"*, plus Scenarii/Virtues/Rules gaps. **A live census shows this bulk-translation work is ~done or non-existent:**

| #202 Phase-2 item | Live state on `18b4d023` | Verdict |
|---|---|---|
| Fallacies `Simple_name_en` (1348 empty) | 61/1408 filled, **EN > FR source** (FR `nom_vulgarisé` = 40) | not a translation gap — **source-empty** |
| Fallacies `political_example_en` (1373 empty) | 35/1408 filled, FR `exemple politique` = 39 | not a translation gap — **source-empty** |
| Scenarii bulk EN (77 rows + 7 cols + `baratineur` leak) | 167/167 EN-covered, **`baratineur` leak = 0/167** | ✅ **DONE** (CLAUDE.md) |
| Virtues EN/RU/PT (0%, blocked by #183) | 100% title/description/remark × 8 langs (PRs #218/#236/#246/#290/#295) | ✅ **DONE** |
| Rules PT apostrophe quality | fixed PR #306 cycle 47 | ✅ **DONE** |
| Core text (desc/example/title) × 8 langs | **100%** on Fallacies 1408 + Virtues 223 (census #609) | ✅ **DONE** |

**The genuinely-translatable EN residue is ~35 cells** (FR source set, EN empty), on secondary columns whose **FR source is itself ~97 % empty**. Bulk-translating them without first enriching the FR source would be acting on a stale dispatch — exactly the trap memory `i18n-coverage-gap-is-link-urls` warns against ("measure FR-relatively, never batch-translate without it").

→ **Recommendation**: #202 should be **closed or re-scoped**. The remaining real work is (a) FR source enrichment (editorial, human, lane #191 — not a translation task) and (b) a ~35-cell micro-translation pass, both **post-release**. This mirrors the #609 reframe (#141 text already done → real gap = AIF) and #618 (link_* = the real i18n gap).

---

## Census (measured on `18b4d023`, 2026-07-01)

### Fallacies taxonomy — 1408 rows, 102 columns

Column fill-rate distribution:

| Bucket | Count | Examples |
|--------|------:|----------|
| **FULL (≥99 %)** | 45 | `desc_*`/`example_*`/`text_*` × 8 langs (the core) |
| PARTIAL (50–99 %) | 18 | (link + structural metadata) |
| SPARSE (5–50 %) | 13 | `link_fr` 45 %, `Remarques` 17 %, `proverbe` 9 %, `link_*` 6–9 % |
| EMPTY (<5 %) | 26 | `Simple_name_en` 4 %, `political_example_en` 2 %, `crossLink_*` 0–1 %, `AIF_skos*` 0–5 % |

**The 6 `_en` columns** — all checked against their FR counterpart:

| FR source | fill | EN target | fill | FR-set & EN-empty (translatable) |
|-----------|-----:|-----------|-----:|---------------------------------:|
| `nom_vulgarisé` | 40 (3 %) | `Simple_name_en` | 61 (4 %) | **25** |
| `exemple politique` | 39 (3 %) | `political_example_en` | 35 (2 %) | **10** |

- **EN > FR** in both — the EN cells were **curated directly in English** (human), not derived from FR. So the "fill `Simple_name_en`" backlog is **not** a translation of existing FR content; it is missing EN content whose **FR source doesn't exist yet either**.
- **Genuinely translatable from FR** = 25 + 10 = **~35 cells** total (FR source set, EN empty). Trivial.

### Why the source is empty (not a bug)

`nom_vulgarisé` and `exemple politique` are **optional editorial columns** — a vulgarised everyday name and a political example, populated per-family as the taxonomy matured. Only ~3 % of nodes have them. **You cannot translate content that was never authored.** Filling EN here requires **authoring FR first** (lane #191, editorial, human decisions), then a translation pass — not a bulk EN job.

### Scenarii — baratineur leak (Phase 2 item)

| Check | Result |
|-------|--------|
| Scenarii rows with literal `baratineur` in an EN column | **0/167** ✅ |
| `suggestion_en` fill | 167/167 |

CLAUDE.md confirms Scenarii **167/167 records 100 % covered EN/RU/PT** (commits `7ed970a3`/`2a1b86bf`/`0dc838fb`, verified `7206f2f9`). The Phase-2 Scenarii backlog is **fully resolved**.

---

## What the "bulk EN" really decomposes into (post-release)

| Real residue | Nature | Lane | Pre/post-tag |
|---|---|---|---|
| FR source enrichment of `nom_vulgarisé` / `exemple politique` | **Editorial authoring** (human judgement per family) | #191 | post-tag |
| ~35 cells FR→EN (once source authored) | micro-translation (gpt-5.5, drift-free #595) | po-2024 | post-tag |
| `link_*` secondary columns (6–9 % non-FR) | **URL resolution** (human research / MediaWiki) | #600/#618 (done) ✅ | post-tag |
| `crossLink_*` / `AIF_skos*` (0–5 %) | **Argumentation-graph enrichment** (expert gate) | #141 (blocked: gpt-5.5 key) | post-tag |

None of these is a "bulk EN translation run". The closest (the ~35 cells) depends on FR authoring first.

## Why this is scoping, not execution

1. **Pre-tag freeze** — `Cards/` frozen; any translation write is post-release.
2. **Stale-dispatch protection** — #202's Phase-2 cell counts (1348, 1373) are **2-year-old** and contradict the live census. Acting on them = wasted bulk-translation of non-existent source (memory: *measure FR-relatively, never batch-translate without it*).
3. **The real i18n gap is `link_*`**, already addressed (#600/#618). Text i18n is 100 %.

## Scope of THIS PR

- ✅ `docs/taxonomy/202-bulk-en-scoping.md` — this read-only scoping doc.
- ✅ **0 write `Cards/`**, **0 AssetConverter code change** (pre-tag safe).
- ✅ Base `18b4d023`.

## Recommendation

- **Close or re-scope #202**: the bulk-EN framing is stale; the epic's concrete residue is editorial FR authoring (#191) + a ~35-cell micro-pass + the already-resolved link_*/AIF lanes.
- **Do NOT launch a bulk EN translation run** — there is no source content to translate on the secondary columns, and core text is already 100 %.

Relates to #202, #191 (FR source quality), #609 (#141 census), #600/#618 (link_*), memory `i18n-coverage-gap-is-link-urls`.
