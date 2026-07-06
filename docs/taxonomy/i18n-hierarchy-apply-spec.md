# Unified gated apply-spec — Fallacies i18n-hierarchy gap (48 rows × 3 EN columns)

**Scope**: consolidates the two data-quality inventories — #712 `family-gap-inventory.md`
(Family/Subfamily empty) + #716 `subsubfamily-gap-inventory.md` (Subsubfamily empty, same 48 rows) —
into **ONE post-tag apply spec** with a gated, byte-targeted script. Dispatched by ai-01
(`h2utyb` PRIMARY, 2026-07-06 07:09) after the inventories were ratified via batch merge.

**GATED: 0 write to the prod CSV in this PR** (release freeze on `Cards/` until the v0.9.0 tag).
The script + unit tests ship and run (synthetic + grounding), but `--apply` on the prod CSV is a
**post-tag** operation.

**Author**: po-2024 (worker) · **Dispatch**: `h2utyb` (PRIMARY) · **Base**: master `7ebeda18`.

> **Status: SPEC + GATED SCRIPT, NOT EXECUTED.** Same discipline as #654 `mnemonics_to_latin.py`:
> re-derive the mapping at apply time, abort on any conflict/novel key, BOM+CRLF+QUOTE_MINIMAL
> byte-preserved. jsboige ratifies + tags → worker runs `--apply` in a post-tag follow-up.

---

## TL;DR

The 48 Fallacies-taxonomy rows flagged by #712 (empty `Family`/`Subfamily` EN) are the **exact same
48 rows** #716 found with empty `Subsubfamily` EN (overlap 48/48, 0 difference). Each row is a pure
i18n-propagation gap: its French `Soussousfamille` is filled but **all three** English hierarchy
columns are empty. The fix is **100% mechanical and deterministic** — each row's FR key maps to
exactly one canonical `(Family, Subfamily, Subsubfamily)` EN tuple (100% consensus, zero conflict).

**This PR unifies the two inventories into one apply** (the #712 apply would have filled 2 columns
and missed the third; doing all three in one pass avoids a second apply round):

- **Script**: `tools/fallacies_i18n_hierarchy_apply.py` (+ `tools/test_fallacies_i18n_hierarchy_apply.py`).
- **Dry-run on master `7ebeda18`**: **144 cells** (48 rows × 3 EN columns), **0 conflict**, **0 novel key**, canonical map 63 FR keys (consensus). 11/11 unit tests pass.
- **Post-tag apply**: one command — `python tools/fallacies_i18n_hierarchy_apply.py --csv <PATH> --apply`.

---

## 1. The gap (code=truth on `7ebeda18`)

| Criterion | Count |
|-----------|------:|
| Rows with `Soussousfamille` (FR) filled AND `Family` (EN) empty | **48** |
| … of which also `Subfamily` (EN) empty | 48 |
| … of which also `Subsubfamily` (EN) empty | 48 |
| Family-header rows (FR `Soussousfamille` empty — pk 0 + 7 family roots) | 8 (legitimate empties, excluded) |
| Total CSV rows | 1408 |

The 48 rows are **not orphans, not headers, not ambiguous**. They are a pure i18n-propagation gap.

---

## 2. The canonical FR → EN map (re-derived at apply time)

Built deterministically from the **filled** rows: for each distinct `Soussousfamille` (FR) value, the
`(Family, Subfamily, Subsubfamily)` EN tuple carried by the rows where all three EN cols are filled.
**100% consensus — no conflicting tuple for any FR key.** The 6 FR keys present in the gap:

| `Soussousfamille` (FR) | Rows in gap | `Family` (EN) | `Subfamily` (EN) | `Subsubfamily` (EN) |
|------------------------|------------:|---------------|-------------------|----------------------|
| Jeu de pouvoir | 33 | Influence | Psychological manipulation | Power games |
| Conditionnement | 6 | Influence | Psychological manipulation | Conditioning |
| Mensonge | 4 | Cheating | Spin doctoring | Lying |
| Langage persuasif | 2 | Influence | Rhetorical device | Loaded language |
| Ambiguïté narrative | 2 | Misleading language | Ambiguity | Narrative ambiguity |
| Repoussoir | 1 | Influence | Appeal to emotion | Foil |
| **Total** | **48** | | | |

**6 distinct FR sub-sub-families → 6 canonical tuples.** No judgment call. The script re-derives
this map from the CSV at apply time (never hardcodes it) — if the map drifts between now and apply,
the gates (§4) catch it.

---

## 3. The gated script

`tools/fallacies_i18n_hierarchy_apply.py` — Python 3 stdlib, same byte-targeted discipline as
`tools/mnemonics_to_latin.py` (#654):

```
python tools/fallacies_i18n_hierarchy_apply.py --csv "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
    # default: dry-run, prints the 144-cell plan + canonical map, writes no file.

python tools/fallacies_i18n_hierarchy_apply.py --csv <PATH> --report docs/<NAME>.md
    # dry-run + markdown report (the plan + map + any conflict/novel).

python tools/fallacies_i18n_hierarchy_apply.py --csv <PATH> --apply
    # WRITES the CSV in place (byte-targeted). GATED post-tag.
```

**Byte-targeting** (`apply_plan`): reads raw bytes, detects BOM, decodes utf-8-sig, `csv.reader`
preserves column order, fills ONLY the empty target cells, reserialises `QUOTE_MINIMAL` + CRLF,
re-prepends BOM. Round-trip fidelity: the diff between input and written file is exactly the 144
newly-filled cells, nothing else (verified by the apply round-trip unit test).

**Unit tests** (`tools/test_fallacies_i18n_hierarchy_apply.py`, 11 tests, all PASS):
- canonical-map derivation (consensus, conflict detection, empty-key exclusion)
- plan building (3-col fill, partial gap, novel-key abort, no-FR-key skip, full-row-not-a-gap)
- apply round-trip (BOM+CRLF preserved, empty cells filled, filled cells never overwritten)
- **real-CSV grounding**: asserts 48 rows × 3 cols = 144 cells, 0 conflict, 0 novel on master.

---

## 4. Validation gates (enforced in code — abort with exit 2)

The `--apply` is refused (exit 2) if ANY gate fails. The dry-run surfaces the same signals.

1. **Re-derive mapping at apply time** — no hardcoded tuple. The canonical map is rebuilt from the
   CSV read in the same run.
2. **Conflict** — a FR key value maps to >1 distinct EN tuple among filled rows → abort (a real
   semantic drift; do not guess which tuple wins). Currently 0 conflicts on `7ebeda18`.
3. **Novel key** — a gap row's FR value is absent from the canonical map (no filled example to derive
   from) → abort. Currently 0 novel keys.
4. **Row count preserved** — the apply touches cells only; it never adds/removes/reorders rows
   (`csv.reader`/`csv.writer` preserve order; rows are mutated in place by pk).
5. **BOM + CRLF + QUOTE_MINIMAL byte-preserved** — only the 144 empty target cells change; the rest
   byte-identical (round-trip unit test grounds this).

---

## 5. Post-tag apply procedure (for the worker, after jsboige tags v0.9.0)

1. `git checkout master && git pull` (ensure on the tagged commit).
2. `python tools/fallacies_i18n_hierarchy_apply.py --csv "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv" --report docs/taxonomy/i18n-hierarchy-apply-<DATE>.md`
   — dry-run; confirm the report shows **144 cells, 0 conflict, 0 novel**. If not, STOP and surface.
3. `python tools/test_fallacies_i18n_hierarchy_apply.py` — confirm 11/11 still pass.
4. `python tools/fallacies_i18n_hierarchy_apply.py --csv <PATH> --apply` — writes the CSV in place.
5. `git diff --stat "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"` — confirm only 48 rows touched.
6. Commit on a branch `fix/fallacies-i18n-hierarchy-apply`, PR, merge. Then the OWL/PDF regen window
   picks up the corrected EN hierarchy (post-tag regen, coordinated with ai-01).

---

## 6. Impact (what consumes the EN hierarchy columns)

The `Family` / `Subfamily` / `Subsubfamily` (EN) columns are the **English localization of the
hierarchy** — they mirror the FR `Soussousfamille` structure for the EN rendered assets (mind maps,
PDFs, OWL). Filling the 48 rows' 3 EN columns:

- **Correct**: 48 fallacy nodes currently render with a blank/misplaced hierarchy label in EN assets
  (they fall back to the FR `Soussousfamille` or render empty in the EN hierarchy view).
- **Low risk**: no code reads these EN columns for control flow — they are render metadata. The
  pipeline hierarchy is driven by the FR `Soussousfamille` (CLAUDE.md). An EN column fill cannot
  change CsvHelper mapping behavior (keys on column names, not values).
- **Caveat to verify at apply time**: spot-check 2-3 filled rows in a regenerated EN mind map / PDF
  (the post-tag regen window).

---

## 7. Relationship to the other i18n coverage gap (link_fr → link_en, 24 rows)

The all-pairs scan (dispatch `ynv05a` TERTIAIRE, `i18n_gap_scan.py`) also found **24 rows with
`link_fr` filled / `link_en` empty**. These are **URLs, not hierarchy labels** — they are not
mechanically derivable from FR (each is a distinct external resource link) and are explicitly a
**human-research lane** (memory `[[i18n-coverage-gap-is-link-urls]]`). **This apply does NOT touch
them.** They remain a separate, deferred coverage item.

## 8. Scenarii + Rules scan — 0 gap (clean negative, dispatch TERTIAIRE)

The same i18n-propagation-gap method was applied to the other two `Cards/` datasets (dispatch
`h2utyb` TERTIAIRE) to check whether the Fallacies gap is unique. These datasets use **semantic name
pairs** (not the Fallacies `_fr`/`_en` suffix convention), so the FR↔EN pairs are enumerated
explicitly. **Result: 0 derivable gap in either dataset.** Fallacies is the only `Cards/` dataset with
an i18n-propagation hierarchy gap.

**Scenarii** (`Cards/Scenarii/Argumentum Scenarii - Cards.csv`, 167 rows) — 8 FR→EN pairs scanned:

| FR → EN | gap rows | derivable | conflict | novel |
|---------|---------:|----------:|---------:|------:|
| catégorie → category | 0 | — | — | — |
| sous-catégorie → subcategory | 0 | — | — | — |
| titre → title | 0 | — | — | — |
| baratineur → smoothTalker | 0 | — | — | — |
| piocheur → drawer | 0 | — | — | — |
| contexte → context | 0 | — | — | — |
| enjeu → issue | 0 | — | — | — |
| suggestion → suggestion_en | 0 | — | — | — |

Every FR-filled row has its EN counterpart filled (0 gaps across all 8 pairs). This confirms the
prior coverage finding (memory: Scenarii 100% translated, 8 fields × 4 langs, verified cell-by-cell).

**Rules** (`Cards/Rules/Argumentum Rules - Cards.csv`, 15 rows) — single FR→EN pair: `Text` →
`Text_en` = **0 gap** (all 15 EN cells filled; consistent with #306 fixing the PT row-1 cover).

**Reproducibility**: `scenarii_rules_gap_scan.py` (scratchpad, read-only, stdlib) re-derives this
negative result on any checkout at `7ebeda18`. This closes the TERTIAIRE question: the Fallacies
hierarchy gap is the **only** i18n-propagation gap in `Cards/` — Scenarii and Rules are complete.

---

## Gate boundaries (HARD — spec + gated script, NOT executed)

- ❌ No prod CSV write, no build, no test run on prod, no OWL regen in this PR.
- ❌ No `--apply` execution during release freeze (`Cards/` frozen pre-tag).
- ✅ Script + unit tests ship (synthetic + grounding tests pass); dry-run on the real CSV produces
  the 144-cell / 0-conflict report.
- ✅ Fix is mechanical (6-tuple canonical map, 100% consensus, zero ambiguity) — gated post-tag.

Relates: dispatch `h2utyb` (PRIMARY + TERTIAIRE), #712 (Family/Subfamily gap inventory), #716
(Subsubfamily gap inventory + count reconcile), #654 (same byte-targeted apply discipline), #587
(CSV hygiene lane), #707 §1 (gap flagged). Base `7ebeda18`.
