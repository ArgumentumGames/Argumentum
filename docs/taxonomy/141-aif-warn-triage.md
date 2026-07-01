# #141 AIF WARN `bad_map:*` triage (DRY-RUN, no auto-apply)

**Author**: po-2024 (worker) · **Date**: 2026-07-01 · **Base**: master `33b1c0bc`
**Source of truth**: [`141-aif-candidates-fullscale.json`](141-aif-candidates-fullscale.json)
(structured sidecar, #626) · triage reproduced by
[`141-aif-warn-triage.py`](141-aif-warn-triage.py) → reviewable
[`141-aif-warn-triage.csv`](141-aif-warn-triage.csv) (87 rows).
**Status**: **ADVISORY — the schema-extension decision for the expert gate.** Nothing is auto-applied.

This completes the expert-gate package for #141 (alongside the [Stage-3
adjudication](141-aif-stage3-adjudication.md) of the 12 existing-AIF nodes and the
[closure recommendation](141-closure-recommendation.md)). It addresses the **other** gate
decision surfaced in the [full-scale report](141-aif-fullscale-report.md): the 87 WARN nodes.

## What the 87 WARNs are (and are not)

Each WARN is `bad_map:*` — the generator proposed an AIF `mappingType` **outside the *observed*
on-disk set** `{broadMatch, closeMatch, narrowMatch}`. The validator is strict-by-construction: it
flags anything not in the observed set. **These are legitimate SKOS predicates or explicit hedges —
not fabrications.** There are **zero** fabricated scheme names and **zero** invented targets across
all 1232 nodes; the closed-set design contains fabrication completely. The 87 are a **schema call**:
extend the observed set, or down-grade.

## Triage (87 nodes, 7 % of 1232)

| mappingType | Nodes | Recommendation |
|---|---:|---|
| `skos:relatedMatch` | 78 | **ADOPT** — extend observed set |
| `skos:exactMatch` | 2 | **ADOPT** — extend observed set (but ⚠️ strong claim — verify per-node) |
| `none` | 5 | **DROP** — weak hedge, no defensible mapping |
| `skos:noMatch` | 2 | **DROP** — explicit no-match hedge |
| **Total** | **87** | **80 ADOPT / 7 DROP** |

### ADOPT — extend the observed set (80 nodes)
`skos:relatedMatch` (78) and `skos:exactMatch` (2) are standard SKOS mapping predicates.
`relatedMatch` is the natural fit for "genuine-but-weaker" AIF cross-references (analogous to the
`IsRelatedTo` cross-link verb that dominates the 3850 crossLinks). Adopting them **enriches the
mapping semantics** beyond the 3 observed predicates at zero fabrication risk — they are real SKOS.
- **Action**: extend the observed `MAP_TYPES` set to include `relatedMatch` (and optionally
  `exactMatch`), then these 80 nodes' mappingType becomes valid (no rewrite needed — the value is
  already the correct predicate).
- ⚠️ `exactMatch` (2 nodes) is a **strong** claim (the source *is* the scheme); the gate should
  spot-verify those 2 rather than rubber-stamp.

### DROP — weak hedges (7 nodes)
`none` (5) and `skos:noMatch` (2) are the generator hedging on weak fits — proposing a mappingType
without committing to a real predicate. These are candidates to **drop during ratification** rather
than salvage (no mappingType > a fake `none`). The 7 nodes: `2,131` Jeu de sonorités, `2,1312`
Assonance, `2,132` Répétition, `2,1322` Anaphore, `2,13222` Symploque (the 5 `none`), + 2 `noMatch`.
- **Action**: on ratification, leave these nodes' `mappingType` empty (or unset), not `none`.

## Reproducibility note

The structured JSON is the source of truth for WARNs (the flat CSV loses the warn for the ~23 nodes
that have a `mappingType` but 0 `DirectRef`/`ExceptionRef`, since no AIF row is emitted for them —
hence reading WARNs from the flat CSV undercounts by ~23). The JSON is regenerable via
`python docs/taxonomy/141-aif-fullscale.py --finalize` (from the `tmp/` checkpoint). The committed
**output** of this triage is the 87-row CSV — a reviewer reads it directly without re-running.

## What this does NOT do

- **No schema change**, no `MAP_TYPES` edit, no taxonomy write. Every row is a gate call.
- **No claim the 80 ADOPT nodes are correct mappings** — only that their *predicate* is valid SKOS;
  whether each mapping itself holds is part of the general Stage-3 ratification (confidence > 0.8
  first).
- **No `Cards/` write, no AssetConverter change** — pre-tag freeze honored.

Relates to #141, #609, #620, #623, #626, #130, #136, #192. Memory honored: Anti-Fab "Walton scheme
= WARN" — these WARNs are flagged for the gate precisely because the closed-set validator is strict;
none are silently accepted.
