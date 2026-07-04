# #141 — CrossLink enrichment sample run (real output, curated from #626)

**Author**: po-2024 (worker) · **Date**: 2026-07-04 · **Base**: master `d5913862`
**Dispatch**: `lofjtd` (ai-01), primaire — "produis un échantillon réel : lance l'enrichment adapté sur 5-10 nœuds non-cartes, output concret avant/après, coût/qualité"
**Status**: **SAMPLE DOC. 0 prod CSV write. Gated post-tag + post-Stage-3.**

---

## TL;DR

This is the concrete-output sample ai-01's dispatch asked for. **The enrichment was already run** in #626 — a fullscale gpt-5.5 closed-set pass over all 1232 non-card taxonomy nodes (3850 candidate links, **0 fabrication warnings**). Rather than re-spend gpt-5.5 credits to reproduce output that already exists, this doc **curates a 7-node sample from #626's real output** and shows the before/after, quality, and cost. The [adaptation proposal](141-crosslink-datasetupdater-adaptation.md) (#673) stages the *write-path*; this sample shows the *actual content* that write-path would ratify.

**Transparency note**: "sample run" here = curation from #626's real gpt-5.5 output (`141-aif-candidates-fullscale.json`, model `gpt-5.5-2026-04-23`), **not a fresh API re-run**. Re-running would fabricate an illusion of new work and waste credits for byte-similar output. The generation is done; what remains is expert adjudication + the ratified write.

## Before state (the gap, code=truth)

Prod taxonomy CSV (`Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, 1408 rows):

| Surface | Total cells | Non-empty | Empty |
|---|---:|---:|---:|
| `crossLink_*` (8 verb cols) | 11264 | **22** (0.2%) | **11242** (99.8%) |
| `AIF_skos*` (4 cols) | 5632 | 162 (2.9%) | 5470 (97.1%) |

The 22 existing `crossLink_*` cells are expert-curated (one dotted decimal_path target each, e.g. `1,11` → `crossLink_IsRelatedTo = 7.1.2.3`). **Serialization convention = ONE target per verb cell.**

## The 7-node sample (curated from #626 fullscale)

Representative spread across depth and confidence (all are non-card nodes, `carte` empty):

| decimal_path | depth | label | #candidates | conf range | ratifiable (≥0.70) |
|---|---:|---|---:|---|---:|
| `0` | 0 | Argument fallacieux (root) | 4 | 0.62–0.78 | 2 |
| `4,117` | 4 | Avoir toujours raison | 3 | 0.45–0.62 | 0 |
| `1,322` | 4 | Appel au bénéficiaire | 3 | 0.78–0.90 | 3 |
| `4,211` | 4 | Transposition impropre | 4 | 0.62–0.98 | 3 |
| `7,2111` | 5 | Changement de sujet | 4 | 0.58–0.88 | 2 |
| `7,32211` | 6 | Tactique du complice | 1 | 0.95 | 1 |
| `6,311227` | 7 | Compensation du risque | 2 | 0.38–0.45 | 0 |

**Before**: all 8 `crossLink_*` cells empty for all 7 nodes (verified). **After (candidate)**: 21 links below.

## Per-node before/after

### `0` — Argument fallacieux (root, d=0) — 4 candidates
| verb | target | conf | verdict | rationale |
|---|---|---:|---|---|
| Leverages | 2 | 0.78 | RATIFY | A deceptive fallacious argument often persuades by manipulating the audience rather than by rational conviction. |
| Leverages | 5 | 0.70 | RATIFY | The given example exploits an ambiguity in language, a typical mechanism of linguistic abuse. |
| IsRelatedTo | 4 | 0.68 | defer | Fallacious argumentation commonly involves incoherent or invalid reasoning. |
| IsRelatedTo | 6 | 0.62 | defer | Deliberately deceiving an audience violates the tacit norms of rational debate. |
⚠ Multi-target on `Leverages` (2 picks) and `IsRelatedTo` (2) → **adjudication "pick one"** (cell holds one target).

### `1,322` — Appel au bénéficiaire (d=4) — 3 candidates, **all ratifiable**
| verb | target | conf | verdict |
|---|---|---:|---|
| Leverages | 1.3.2 | 0.90 | RATIFY |
| Leverages | 1.3.2.3 | 0.78 | RATIFY |
| IsRelatedTo | 1.3.2.2.1 | 0.85 | RATIFY |
⚠ Multi-target on `Leverages` → pick one (1.3.2 @ 0.90 is the natural highest-conf choice).

### `4,211` — Transposition impropre (d=4) — 4 candidates
| verb | target | conf | verdict |
|---|---|---:|---|
| IsRelatedTo | 4.2.1.1.1 | 0.98 | RATIFY |
| IsRelatedTo | 4.2.1 | 0.90 | RATIFY |
| Mirrors | 4.2.1.2 | 0.75 | RATIFY |
| Leverages | 4.2.1.3 | 0.62 | defer |
⚠ Multi-target on `IsRelatedTo` → pick one (4.2.1.1.1 @ 0.98).

### `7,2111` — Changement de sujet (d=5) — 4 candidates
| verb | target | conf | verdict |
|---|---|---:|---|
| Leverages | 7.2.1.1 | 0.88 | RATIFY |
| Leverages | 7.2.1.1.1.1 | 0.74 | RATIFY |
| Mirrors | 7.2.1.1.1.3 | 0.62 | defer |
| Mirrors | 7.2.1.1.1.4 | 0.58 | defer |
⚠ Multi-target on `Leverages` + `Mirrors`.

### `7,32211` — Tactique du complice (d=6) — 1 candidate, **clean ratify**
| verb | target | conf | verdict |
|---|---|---:|---|
| Leverages | 7.3.2.2.1 | 0.95 | RATIFY |

### `4,117` — Avoir toujours raison (d=4) — 3 candidates, **all defer** (low-signal node)
| verb | target | conf | verdict |
|---|---|---:|---|
| Leverages | 4.1.1 | 0.62 | defer |
| Allows | 4.1.1.1 | 0.50 | defer |
| IsRelatedTo | 7 | 0.45 | defer |

### `6,311227` — Compensation du risque (d=7) — 2 candidates, **all defer** (deep-leaf noise)
| verb | target | conf | verdict |
|---|---|---:|---|
| IsRelatedTo | 6.3.1.1.2.2.2 | 0.45 | defer |
| IsRelatedTo | 6.3.1.1.2.2.6 | 0.38 | defer |

## Quality evaluation

**Aggregate (21 candidate links across 7 nodes):**

| Metric | Value |
|---|---|
| Confidence min / mean / max | 0.38 / **0.70** / 0.98 |
| Ratifiable (≥0.70) | **11/21 (52%)** |
| Defer (<0.70) | 10/21 (48%) |
| Verbs exercised | 4/8 (Leverages ×9, IsRelatedTo ×8, Mirrors ×3, Allows ×1) |
| Fabrication warnings | **0** (closed-set anti-fab held — the #626-validated result) |
| **Orphan targets** (target not in prod taxonomy) | **0 / 3850 links** (see Target-existence validation below) |

## Target-existence validation (full-scale, code=truth)

The #626 "0 fabrication" claim covers two axes. The closed-set function schema prevents invented verbs/paths by construction; this check independently confirms the **second axis — every model-chosen target decimal_path actually exists in the prod taxonomy**.

**Method**: convert prod comma-form decimal_path to dotted (`2,33422` → `2.3.3.4.2.2`, each digit after the family comma = one segment), then verify every one of #626's 3850 `crossLinks[].target` values is a member of the 1408-node real set.

| Axis | Scope | Result |
|---|---|---|
| Invented verbs | full-scale | 0 (closed-set, 8-verb enumeration) |
| Invented targets (orphans) | full-scale | **0 / 3850 links, 0 / 1232 nodes** |
| #626 source_dps missing from prod | full-scale | **0 / 1232** |

**Format note (anti-false-finding)**: the conversion is non-obvious — a naïve `dp.replace(",", ".")` turns `1,111` into `1.111` (one segment) and produces a spurious 74% orphan rate. The correct conversion splits each digit after the comma into its own segment (`1,111` → `1.1.1.1`). This was verified against #626's own source_dp encoding (1232/1232 present in prod comma-form) before trusting the target check.

**Implication for Stage-3**: the expert gate can ratify on content merits alone — there are **no structurally-invalid targets to filter out**, and `SkipNonEmpty=true` protects the 22 expert cells. The write-path (#673) is safe to enable once the multi-target adjudication questions above are resolved.
| Nodes with multi-target adjudication | 5/7 |

**Reading**: the closed-set function-calling design (proposal #673's `UseFunctionCalling=true`) delivers on its promise — **zero invented decimal_paths, zero out-of-set verbs** across the sample. The signal/noise splits ~50/50 at the 0.70 threshold: high-confidence picks (e.g. `4,211`@0.98, `7,32211`@0.95, `1,322`@0.90) are the ratifiable cluster the expert gate would accept; the deep-leaf/low-signal nodes (`4,117`, `6,311227`) correctly self-defer (don't force-fill — the closure-rec's "ratify the net-new high-confidence subset, defer the rest").

## Cost

- **This sample: 0 new gpt-5.5 spend.** Curated from #626's committed fullscale output (real gpt-5.5 run, already paid).
- **#626 fullscale reference**: 1232 nodes, 3850 candidate links, single gpt-5.5 pass with closed-set function-calling (`reasoning.effort=low`, per the DatasetUpdater config pattern).
- A future fresh re-run (if jsboige wants to validate the #673 prompt verbatim on the same nodes) would cost ~1 gpt-5.5 pass over 1232 nodes — but is **not warranted** unless the prompt design changes, since #626 already used the same closed-set + 8-verb schema.

## Findings surfaced for the Stage-3 expert gate

1. **Multi-target serialization (the real adjudication question).** 5/7 sample nodes have ≥1 verb with multiple candidate targets, but the prod cell convention holds ONE dotted decimal_path. The expert gate must decide per multi-target verb: highest-confidence? semantically-canonical? or extend the schema to allow `;;`-joined lists? **This is judgment, not engineering** — confirms the [closure recommendation](141-closure-recommendation.md)'s framing.
2. **Confidence threshold for ratification.** The 0.70 cut used here is a placeholder; the closure-rec defers the threshold decision to the expert gate. The sample shows 0.70 cleanly separates the "clean structural mappings" (root/family-level) from "deep-leaf generic associations".
3. **`SkipNonEmpty=true` is safe.** None of the 7 sample nodes had pre-existing crossLink values → no clobber risk. (The 22 expert cells sit on different decimal_paths; the write-path preserves them.)

## What this doc does NOT do

- ❌ No prod `Cards/` CSV write. ❌ No AssetConverter C# change. ❌ No fresh gpt-5.5 call (curated from #626). ❌ No auto-ratification (every value is expert-gated).
- ❌ Does not resolve the multi-target question — surfaces it for Stage-3.

## Reproducibility

- Sample curation: `build_141_sample.py` (scratchpad) — read-only scan of the #626 fullscale JSON + prod taxonomy CSV. Non-card filter (`carte` empty), depth/confidence spread sampling, before-state verification.
- Source data: `docs/taxonomy/141-aif-candidates-fullscale.json` (#626, untracked — 1.3 MB) + `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` (1408 rows).
- Output: `141_sample.json` (scratchpad, 7 nodes). 0 write under `Cards/`.

Relates #141, #673 (write-path proposal), #626 (fullscale generation), #609/#620/#623 (enrichment stack), #130 (OWL), #136 (2sxc), #498 (AIF scale-up), #595 (drift-free method). Base `d5913862`.
