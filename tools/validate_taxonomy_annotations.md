# `validate_taxonomy_annotations.py` — anti-fabrication validator

A stdlib-only Python tool that encodes the manual anti-fabrication passes done for
each taxonomy scale-up phase, so a new proposition CSV can be validated
**automatically before its PR**. Dispatched by ai-01 ([`msg-20260617T051231-o6izvl`](../docs/taxonomy)) — built by worker po-2024.

## Run

```bash
# Validate every proposition CSV currently in docs/taxonomy/ (default):
python tools/validate_taxonomy_annotations.py

# Validate a specific phase CSV (e.g. on your branch, before opening the PR):
python tools/validate_taxonomy_annotations.py docs/taxonomy/499-scaleup-phase1-annotations.csv

# If Cards/Fallacies/ is not auto-detected, pass the repo root:
python tools/validate_taxonomy_annotations.py --taxonomy-root /path/to/Argumentum
```

**Exit code:** `0` = no HARD violations; `1` = HARD violations present; `2` = usage/IO error.
Warnings never fail the run.

No dependencies beyond the Python 3 standard library (`csv`, `argparse`, `os`, `sys`).

## What it checks

The tool detects the schema kind from the CSV header and runs the applicable
checks. **Ground-truth checks are HARD** (fail the run); checks we cannot
auto-verify canonically are **WARNINGs** that surface for human review.

| Check | Applies to | Level | Rule |
|-------|-----------|-------|------|
| (a) PK membership | all | HARD | every `*_pk` / opposed PK / family PK / source+target PK is a real PK in the corresponding corpus CSV; family PKs are depth 1 |
| (b) link type | virtues, crosslinks | HARD | `link_type` ∈ the 8 `crossLink_*` types |
| (c) Walton scheme | virtues (`AIF_skosDirectRef`), AIF (`RA_scheme`/`walton_scheme`) | WARN | scheme ∈ the union used by validated pilots (drift detection — see below) |
| (d) attack type | aif-scaleup | HARD | `attack_type` ∈ {`undermine`, `undercut`, `rebut`} |
| (e) attack coherence | aif-scaleup | WARN | `attacked_component` coherent with `attack_type` (data-grounded map: undermine→premise, undercut→inference_rule, rebut→conclusion) |
| (f) symmetric reciprocity | crosslinks | HARD | when `symmetric=True`, the reverse edge `(target, link_type, source)` is encoded |

**Schema kinds detected:** `virtues` (`virtue_pk`+`prevented_family_pk`) ·
`aif-scaleup` (`fallacy_pk`+`attack_type`, triple AIF) · `aif-pilot` (`fallacy_pk`+`walton_scheme`,
legacy) · `crosslinks` (`source_pk`+`target_pk`+`symmetric`).

## Why check (c) is a WARNING, not HARD (anti-fab design)

The dispatch asked for `RA_scheme`/`AIF_skosDirectRef` ∈ "Walton 24". **No
machine-readable canonical Walton list exists in this repo** — the AIF lane was
generated via ad-hoc scripts, and the `AIF_skosDirectRef` column of the Fallacies
corpus holds AIF **node IDs** (`Bias_Inference`, `Commitment_Conflict`…), not
human scheme names. Hardcoding Walton's 24 from memory would risk false failures
(a real scheme with a slightly different spelling) or false passes — a fabrication
risk. Instead, the catalog is the **union of scheme names already used in the
merged, validated pilots**; a scheme a new phase uses that isn't in that union is
flagged as **NOVEL** for human verification. This catches drift/typos honestly
without inventing a canonical list.

## Validated corpus (DoD, 2026-06-17)

| CSV | kind | rows | HARD | WARN |
|-----|------|------|------|------|
| `497-pilot-crosslinks.csv` | crosslinks | 24 | 0 | 0 |
| `498-pilot-annotations.csv` | aif-pilot | 18 | 0 | 0 |
| `499-pilot-annotations.csv` | virtues | 10 | 0 | 8 (diacritic-stripped titles vs corpus) |
| `498-scaleup-phase1-annotations.csv` (#509) | aif-scaleup | 11 | 0 | 1 (novel valid scheme) |
| `499-scaleup-phase1-annotations.csv` (#510) | virtues | 18 | 0 | 0 |

**0 HARD violations across all 5 files.** A negative test (deliberately corrupted
CSVs) confirms every HARD check fires on the corresponding error (bad PK, bad
link type, wrong family depth, bad attack type, missing reciprocal edge).

## Limitations (honest scope)

- (c) is drift-detection only, not canonical-Walton verification (by design, above).
- (e) coherence map is grounded in the #509 phase-1 vocabulary + AIF standard; a
  genuinely new but legitimate attack/component pair will WARN — review it.
- Name-field warnings use exact string match (the merged 499 pilot strips French
  diacritics → 8 expected warnings; this surfaces real drift, not validator bugs).
