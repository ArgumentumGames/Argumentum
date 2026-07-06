# Data-quality gap — 48 rows with empty `Subsubfamily` column (third column on the #712 rows)

**Scope**: inventory + gated fix proposition for the **second** i18n-propagation gap found by applying
the #712 method Fallacies-CSV-wide (dispatch `ynv05a` TERTIAIRE). **48 rows in
`Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` with an empty `Subsubfamily` (EN) column** —
the **exact same 48 rows** as #712's `Family` gap, but a **third EN column** (`Subsubfamily_en`) that
#712 did not fill.
**GATED: 0 write to the prod CSV in this PR** (release freeze on `Cards/`). Proposition, derived
code=truth, ready for a post-tag apply.

**Author**: po-2024 (worker) · **Dispatch**: `ynv05a` (TERTIAIRE, ai-01 2026-07-06 01:32)
**Base**: master `bdba45d8`.

> **Status: INVENTORY + GATED FIX PROPOSITION.** No CSV write, no build, no regen. jsboige ratifies
> → worker applies the CSV edit in a post-tag follow-up (gated, same window as #712).

---

## TL;DR — finding

The #712 family-gap inventory identified 48 rows with empty `Family` (EN) + `Subfamily` (EN) and
proposed a gated lookup-table fill of those two columns. **The same 48 rows also have an empty
`Subsubfamily` (EN) column**, which #712 did not address. Applying the #712 method Fallacies-CSV-wide
(scan every FR/EN column pair for FR-filled/EN-empty) confirms `Subsubfamily_en` is the **only other
hierarchy i18n-propagation gap** — and it is 100% mechanically derivable (each gap row's FR
`Soussousfamille` maps to exactly one canonical `Subsubfamily_en`, 0 conflict).

| Scan (FR filled / EN empty) | Gap rows | Mechanically derivable? |
|-----------------------------|---------:|-------------------------|
| `Famille` → `Family` (EN) | 48 | ✅ (#712 — already inventoried) |
| **`Soussousfamille` → `Subsubfamily` (EN)** | **48** | **✅ 48/48 1-to-1, 0 conflict (THIS gap)** |
| `Sousfamille` → `Subfamily` (EN) | (covered by #712's 2-column fill) | ✅ |
| `text_fr` → `text_en` | 0 | — (no gap) |
| `desc_fr` → `desc_en` | 0 | — (no gap) |
| `example_fr` → `example_en` | 0 | — (no gap) |
| `link_fr` → `link_en` | 24 | ❌ URLs — human-research lane, not FR→EN translatable (memory `[[i18n-coverage-gap-is-link-urls]]`) |

**Row overlap with #712**: the 48 `Subsubfamily_en`-empty rows are the **exact same 48 rows** as
#712's `Family_en`-empty set (overlap 48/48, 0 in either set difference). So this is **not a new batch
of rows** — it is a **third column on the #712 rows** that #712's 2-column fill missed.

---

## 1. The canonical FR → EN mapping (the fix table)

Built deterministically from the **filled** rows: for each distinct `Soussousfamille` (FR) value,
the `Subsubfamily` (EN) value that the filled rows carry (100% consensus — no conflicting mappings).

| `Soussousfamille` (FR) | Rows in the 48-gap | Canonical `Subsubfamily` (EN) |
|------------------------|-------------------:|-------------------------------|
| Jeu de pouvoir | 33 | Power games |
| Conditionnement | 6 | Conditioning |
| Mensonge | 4 | Lying |
| Langage persuasif | 2 | Loaded language |
| Ambiguïté narrative | 2 | Narrative ambiguity |
| Repoussoir | 1 | Foil |
| **Total** | **48** | |

**6 distinct FR sub-sub-families, each mapping to exactly one EN value.** No ambiguity, no judgment
call — the fix is a lookup-table fill, identical in shape to #712.

---

## 2. The gated fix (proposition — NOT executed)

**One-liner**: for each of the 48 rows with empty `Subsubfamily_en`, look up its `Soussousfamille`
(FR) in the table above and fill `Subsubfamily_en` from the canonical mapping. **This is a natural
extension of the #712 apply path** — the #712 apply should be amended to fill **all three** EN
hierarchy columns (`Family_en` + `Subfamily_en` + `Subsubfamily_en`) on the same 48 rows in one pass,
rather than two separate applies.

**Validation gates the apply must pass before running** (post-tag, same as #712):
1. **Re-derive the mapping on the current master** — the 6-row table must still hold (no conflicting
   `Subsubfamily_en` introduced for any of the 6 FR values since `bdba45d8`). Abort if a conflict
   appears.
2. **Row count unchanged** — the apply touches exactly 48 rows' `Subsubfamily` cells.
3. **BOM + CRLF + quoting byte-preserved** — only the 48 cells change; the rest byte-identical (same
   contract as #654 / #712).
4. **No `Soussousfamille` drift** — the apply keys on `Soussousfamille`; if a row's FR value changed
   since the inventory, it must hit the table cleanly or abort.

**Why not during freeze**: same as #712 — `Cards/` is under release freeze (0 write pre-tag). The gap
is pre-existing (3.4% of rows), does not block generation (the pipeline hierarchy is driven by the FR
`Soussousfamille` column; the EN `Subsubfamily` is secondary render metadata). Fix it in the post-tag
CSV-hygiene window, ideally **merged into the #712 apply** as a third column.

---

## 3. Impact (what consumes `Subsubfamily_en`)

The `Subsubfamily` (EN) column is the **English localization of the finest hierarchy granularity** —
it mirrors the FR `Soussousfamille` for the EN rendered assets (mind maps, PDFs, OWL). Filling the 48
rows' `Subsubfamily_en`:

- **Correct**: 48 fallacy nodes currently render with a blank/misplaced sub-sub-family label in EN
  assets (they fall back to the FR `Soussousfamille` or render empty in the EN hierarchy view).
- **Low risk**: no code reads `Subsubfamily_en` for control flow — it is render metadata. An EN column
  fill cannot change CsvHelper mapping behavior (keys on column names, not values).
- **Caveat to verify at apply time**: spot-check 2-3 filled rows in a regenerated EN mind map / PDF
  (post-tag regen window, not freeze).

---

## 4. Relationship to #712 (and the 56 `Subfamily_en`-empty count)

#712 clarified that the 56 `Subfamily_en`-empty count = 48 gap + 8 family-header rows. The
`Subsubfamily_en`-empty count is **also 48 + 8**:

- **48 gap rows** = the same rows as #712's Family gap (this proposition's fix).
- **8 family-header rows** (pk 0 + 7 family roots) where `Subsubfamily_en` is legitimately empty (a
  family root has no parent sub-sub-family; their FR `Soussousfamille` is also empty, so they are
  **not** in the derivable gap — confirmed by the scan: 0 family-header in the gap set).

So the `Subsubfamily_en`-empty raw count (56) is fully accounted for: 48 mechanical gap + 8
legitimate empties. No additional gap beyond the 48.

---

## 5. Reproducibility

Re-derivable via the read-only scanner (scratchpad, ~60 LOC stdlib):
`python i18n_gap_scan.py` on any checkout at `bdba45d8` reproduces the 48-row `Subsubfamily` gap +
the 6-row canonical mapping + the all-pairs scan (confirming no other hierarchy gap). The apply
script (gated, post-tag) would use the same mapping, re-derived at apply time.

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no build, no test run, no regen in this PR.
- ❌ No execution during release freeze (`Cards/` is frozen pre-tag).
- ✅ Inventory derived code=truth from the taxonomy CSV (read-only deterministic all-pairs scanner).
- ✅ Fix is mechanical (6-row lookup table, 100% canonical consensus, zero ambiguity) — gated
  post-tag, ideally merged into the #712 apply as a third column.

Relates: dispatch `ynv05a` (TERTIAIRE), #712 (Family/Subfamily gap — same 48 rows, 2-column fill),
#707 §1 (gap flagged), #654 (same gated-apply discipline), #587 (CSV hygiene lane),
#192/#458 (i18n coverage). Base `bdba45d8`.
