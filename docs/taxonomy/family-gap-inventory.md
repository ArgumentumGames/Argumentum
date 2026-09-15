# Data-quality gap — 48 rows with empty `Family` column (i18n propagation gap)

**Scope**: inventory + gated fix proposal for the data-quality gap flagged by #707 §1 — **48 rows
in `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` with an empty `Family` (EN) column**.
**GATED: 0 write to the prod CSV in this PR** (release freeze on `Cards/`). This is a proposition,
derived code=truth, ready for a post-tag apply.

**Author**: po-2024 (worker) · **Dispatch**: `awhj8g` (idle-de-secours: « Pioche une autre vérif
code=truth freeze-safe (ex : le data-quality gap '48 rows Family vide' — inventorie-les, propose un
fix gated) ») · **Base**: master `34c7702c`.

> **Status: INVENTORY + GATED FIX PROPOSITION.** No CSV write, no build, no regen. jsboige ratifies
> → worker applies the CSV edits in a post-tag follow-up (gated, like #654 `--apply`).

---

## TL;DR — finding

The 48 rows are **not** orphans, **not** structural headers, and **not** ambiguous. They are a
**pure i18n-propagation gap**: each row has its French hierarchy column `Soussousfamille` (FR)
filled but its English columns `Family` (EN) + `Subfamily` (EN) empty. The fix is **100% mechanical
and deterministic** — each of the 48 rows' `Soussousfamille_fr` maps to exactly one canonical
`(Family_en, Subfamily_en)` pair (100% consensus across the filled rows, zero conflict).

**Categorization (code=truth on `34c7702c`):**

| Category | Count | Verdict |
|----------|------:|---------|
| `Soussousfamille_fr` filled, `Family_en` empty | **48** | the gap (i18n propagation) |
| `Soussousfamille_fr` empty, `Family_en` empty | 0 | (no orphans) |
| `text_fr` empty | 0 | (no blank rows) |
| Header rows (`Subfamily_en` empty + `Family_en` filled) | 8 | **not a gap** — these are the 8 family-header rows themselves (pk 0 root + 7 family-roots: Insufficiency, Influence, Mathematical error, Faulty logics, Misleading language, Cheating, Obstruction). A family-header has no parent subfamily → empty `Subfamily_en` is correct. |

---

## 1. The canonical FR → EN mapping (the fix table)

Built deterministically from the **filled** rows: for each distinct `Soussousfamille_fr` value,
the `(Family_en, Subfamily_en)` pair that the filled rows carry (100% consensus — no conflicting
mappings exist for any FR value).

| `Soussousfamille_fr` | Rows in the 48-gap | Canonical `Family_en` | Canonical `Subfamily_en` |
|----------------------|-------------------:|-----------------------|--------------------------|
| Jeu de pouvoir | 33 | Influence | Psychological manipulation |
| Conditionnement | 6 | Influence | Psychological manipulation |
| Mensonge | 4 | Cheating | Spin doctoring |
| Langage persuasif | 2 | Influence | Rhetorical device |
| Ambiguïté narrative | 2 | Misleading language | Ambiguity |
| Repoussoir | 1 | Influence | Appeal to emotion |
| **Total** | **48** | | |

**6 distinct FR subfamilies, each mapping to exactly one (Family, Subfamily) pair.** No ambiguity,
no judgment call — the fix is a lookup-table fill.

### Spot-check (first/last of the 48)

| pk | text_fr | Soussousfamille_fr | → Family_en (to fill) | → Subfamily_en (to fill) |
|----|---------|--------------------|-----------------------|--------------------------|
| 200 | Fedspeak | Langage persuasif | Influence | Rhetorical device |
| 202 | Obscurantisme | Langage persuasif | Influence | Rhetorical device |
| 338 | Appel au doute et à la crainte | Repoussoir | Influence | Appeal to emotion |
| 414 | Ingénierie sociale (politique) | Conditionnement | Influence | Psychological manipulation |
| 457 | Déstabilisation | Jeu de pouvoir | Influence | Psychological manipulation |
| 882 | Campagne de murmures | Ambiguïté narrative | Misleading language | Ambiguity |
| 918 | Campagne de murmures | Mensonge | Cheating | Spin doctoring |
| 921 | Stratégie de la tension | Mensonge | Cheating | Spin doctoring |

---

## 2. The gated fix (proposition — NOT executed)

**One-liner**: for each of the 48 rows with empty `Family_en`, look up its `Soussousfamille_fr` in
the table above and fill `Family_en` + `Subfamily_en` from the canonical mapping. Read the CSV,
write the CSV in place (UTF-8 BOM + CRLF preserved, byte-targeted fill — same discipline as the
#654 `mnemonics_to_latin.py` apply path).

**Validation gates the apply must pass before running** (post-tag):
1. **Re-derive the mapping on the current master** — the 6-row table must still hold (no conflicting
   `(Family, Subfamily)` pair introduced for any of the 6 FR values since `34c7702c`). If a conflict
   appears, abort and surface — do not guess.
2. **Row count unchanged** — the apply must touch exactly 48 rows' `Family`/`Subfamily` cells, no
   row added/removed/reordered.
3. **BOM + CRLF + quoting byte-preserved** — only the 96 cells (48 rows × 2 cols) change; the rest
   of the file is byte-identical (round-trip fidelity, same contract as #654's `apply_plan`).
4. **No `Soussousfamille_fr` drift** — the apply reads `Soussousfamille_fr` to key the lookup; if a
   row's FR value changed since the inventory, it must hit the table cleanly or abort.

**Why not during freeze**: the CSV is under `Cards/` → release freeze applies (`0 write sous Cards/`
pre-tag). Same gate as #654 `--apply`. Low value during freeze (the gap is pre-existing, 48/1408 =
3.4%, does not block generation — the pipeline uses the FR `Soussousfamille` column for hierarchy,
the EN `Family`/`Subfamily` are secondary metadata). Fix it in the post-tag CSV-hygiene window.

---

## 3. Impact (what consumes `Family_en`)

The `Family` (EN) / `Subfamily` (EN) / `Subsubfamily` (EN) columns are the **English localization
of the hierarchy** — they mirror the FR `Soussousfamille` structure for the EN rendered assets
(mind maps, PDFs, OWL). Filling the 48 rows' EN columns:

- **Correct**: 48 fallacy nodes currently render with a blank/misplaced family label in EN assets
  (they fall back to the FR `Soussousfamille` or render empty in the EN hierarchy view). Filling
  them aligns EN output with the FR hierarchy.
- **Low risk**: no code reads `Family_en` for control flow — it is render metadata. The pipeline's
  hierarchy is driven by the FR column (CLAUDE.md: hierarchy is in `Soussousfamille`). An EN column
  fill cannot change CsvHelper mapping behavior (which keys on column names, not values).
- **Caveat to verify at apply time**: spot-check 2-3 of the filled rows in a regenerated EN mind
  map / PDF to confirm the family label renders correctly (the post-tag regen window is the place
  for this, not freeze).

---

## 4. Relationship to the 56 `Subfamily_en`-empty rows

#707 §1 cites "48 rows `Family` vide". The extraction also counts **56 rows `Subfamily_en` vide**.
The delta (8 rows) is **not a separate gap** — it is the 8 family-header rows (pk 0 + 7 family
roots, see TL;DR) where `Subfamily_en` is legitimately empty (a family-header has no parent
subfamily). So:

- **Real gap**: 48 rows (`Family_en` empty + `Subfamily_en` empty, both fillable from the FR key).
- **Legitimate empties**: 8 rows (`Subfamily_en` empty because they ARE the family headers; their
  `Family_en` is correctly filled).
- **Total `Subfamily_en`-empty = 48 + 8 = 56** ✓ (matches the raw count, accounted for).

No additional gap beyond the 48.

---

## 5. Reproducibility

The inventory re-derivable via the read-only extractor (scratchpad, ~30 LOC stdlib):
`python family_gap_extract.py` on any checkout at `34c7702c` reproduces the 48-row list + the 6-row
canonical mapping. The apply script (gated, post-tag) would use the same mapping, re-derived at
apply time (never trust the inventory blindly — re-verify the canonical mapping holds before
writing).

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no build, no test run, no regen in this PR.
- ❌ No execution during release freeze (`Cards/` is frozen pre-tag).
- ✅ Inventory derived code=truth from the taxonomy CSV (read-only deterministic extractor).
- ✅ Fix is mechanical (6-row lookup table, 100% canonical consensus, zero ambiguity) — gated post-tag.

Relates: dispatch `awhj8g` (idle-de-secours), #707 §1 (gap flagged), #654 (same gated-apply
discipline on the same CSV), #587 (CSV hygiene lane). Base `34c7702c`.
