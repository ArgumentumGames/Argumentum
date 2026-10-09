# #457 DNN site localization — i18n tooling (bricks 1 + 3 + round-trip)

Reusable tooling around the **already-wired** DNN UI-strings translation rail (DatasetUpdater
Option C, config merged in #487, `Enabled=false`). These tools do the work *around* the config
that the config can't do for itself: extract the content-type set from the templates, and
dry-run-verify the re-import — all **fixture/dry-run**, zero prod mutation.

## The 3 bricks (and which already existed)

| Brick | What | Status |
|-------|------|--------|
| **(2) DatasetUpdater config (Option C)** | gpt-5.5 task for `ui.*`/`res.*`, 8-language | ✅ **Already done** — #487 (`ca9a8640`), `DatasetUpdaterRootConfig.cs:2636-2696`, `Enabled=false` |
| **(1) content-type → CSV extractor** | reusable, codifies the PHASE1 manual audit | 🆕 net-new — `extract_dnn_ui_strings.py` |
| **(3) CSV → DNN re-import dry-run verifier** | key-set diff + payload render (no write) | 🆕 net-new — `reimport_dnn_ui_strings.py` |

**Brick 2 was NOT rebuilt** — the investigation confirmed the config-only gpt-5.5 path is already
canonical (entity `DnnUiString`, `KnownDataSets.DnnUiStrings`, prompts, task config all merged,
`Enabled=false`). Re-adding it would duplicate. This PR adds the missing tooling (bricks 1 + 3).

## Files

- `extract_dnn_ui_strings.py` — parse 2sxc `.cshtml`, emit `dnn-ui-strings.csv` dialect.
- `reimport_dnn_ui_strings.py` — `verify` (key-set diff vs reference) + `reimport` (render payload).
- `test_roundtrip.py` — DoD proof: extract → verify → reimport on a fixture, zero prod mutation.
- `fixtures/sample_templates/*.cshtml` — miniature audit-anchored fixture (NOT production).
- `fixtures/reference_snapshot.csv` — committed golden snapshot for the round-trip test.

## Quick start

```bash
# Round-trip DoD test (stdlib only, writes nothing outside temp):
python tools/dnn_i18n/test_roundtrip.py

# Extract from the REAL prod templates (standalone output — does NOT touch dnn-ui-strings.csv):
python tools/dnn_i18n/extract_dnn_ui_strings.py \
    --templates-root DNNPlatform/Portals/1/2sxc/Argumentum \
    --out /tmp/prod_extract.csv

# Verify the extraction's key set vs the reference CSV (HARD contract on key set):
python tools/dnn_i18n/reimport_dnn_ui_strings.py verify \
    --extracted /tmp/prod_extract.csv \
    --reference docs/dnn-localization/dnn-ui-strings.csv

# Render the re-import payload to stdout (dry-run — never writes anywhere):
python tools/dnn_i18n/reimport_dnn_ui_strings.py reimport --csv /tmp/prod_extract.csv
```

## Anti-fabrication guarantees

- **`ui.*` extraction is anchor-based, not free-text.** Each `ui.*` entry declares a verbatim
  anchor that must exist in the named source. If a refactor removes the string, the extractor
  **fails loud (exit 2)** instead of silently dropping the row. A free-text scanner would
  fabricate "translatable strings" out of every template literal.
- **`res.*` extraction is honest about DB-only values.** The `@Resources.<Key>` reference is in
  the repo; the canonical FR *value* lives in SQL (2sxc App Resources). The extractor leaves
  `fr` empty + flags `DB-only`. INFERRED FR scaffolds (PHASE1 §1b) are a human curation step,
  intentionally NOT regenerated.
- **Negative test proven:** breaking an anchor → exit 2 (verified).
- **Cross-validated vs prod:** the extractor run on the real `DNNPlatform/.../Argumentum/`
  templates yields **10/10 keys** matching `dnn-ui-strings.csv` (the only delta is `res.*` fr,
  empty-in-extract by design).
- **Round-trip on fixture: PASS** (extract → verify → reimport, zero prod mutation).

## Gate boundaries (HARD)

- ❌ Does **not** touch the live DNN DB, portal, or 2sxc App Resources — live extract/re-import
  is **DB/RDP-gated (jsboige)**. The `reimport` subcommand only *renders* the payload to stdout.
- ❌ Does **not** modify `docs/dnn-localization/dnn-ui-strings.csv` (that file is worker po-2024's
  lane, #490) — the extractor writes to a user-supplied `--out` path only.
- ❌ Does **not** enable or modify the DatasetUpdater task config (#487's `Enabled=false` rail).
- ❌ Does **not** run any translation — gpt-5.5 translation is the config's job (#487), gated on
  the source FR being complete (which needs the portal export, jsboige).
- ❌ Does **not** declare a QA verdict — that's ai-01.

## What unblocks next

When jsboige exports the 2sxc App Resources (the DB-only `res.*` values), the FR column can be
populated, at which point the existing config (#487) can be flipped `Enabled=true` to run gpt-5.5
across the 7 target languages. This tooling feeds that rail; it doesn't replace it.

---

# #684 Game Rule prose translation — document-tier tooling

A **parallel lane** to the string-tier (#457 above). Where #457 translates short UI labels
(`ui.*`/`res.*` key→value), #684 translates the **rich-HTML game-rule prose** of the 5 published
2sxc Game Rule entities (Summary/Material/Installation/Content/Variants/Memo + Title). See the
manifest [`docs/dnn-localization/684-translation-manifest.md`](../../docs/dnn-localization/684-translation-manifest.md)
and scope correction [`684-scope-correction.md`](../../docs/dnn-localization/684-scope-correction.md)
(23 populated prose cells + 5 titles, 161 / +35 translation units).

## Files

- `translate_game_rules.py` — read the 2sxc Game Rule export, translate each populated FR cell to
  the 7 target langs via gpt-5.5 (HTML preserved), write a re-import-ready JSON. Resume-capable
  (the output doubles as a cache; re-running skips done cell-langs).
- `verify_game_rule_translations.py` — DoD gate: cell parity, no-fabrication, HTML-tag preservation,
  script correctness (#216-style FR-contamination guard).

## Quick start

```bash
# Export is delivered on-box (Method B read-only) and synced to shared-state; pass its path:
EXPORT="$ROOSYNC_SHARED_PATH/attachments/DNN-Argumentum-export-2026-07-07/12-game-rule-content-items.json"

# Smoke (1 cell x 7 langs) before a full pass:
python tools/dnn_i18n/translate_game_rules.py --export "$EXPORT" \
    --out docs/dnn-localization/684-translations.json --smoke

# Full pass (28 cells x 7 langs = 196 cell-langs; ~10-15 min of gpt-5.5 API):
python tools/dnn_i18n/translate_game_rules.py --export "$EXPORT" \
    --out docs/dnn-localization/684-translations.json --all

# Verify the artifact (exit 0 = all DoD gates pass):
python tools/dnn_i18n/verify_game_rule_translations.py \
    --artifact docs/dnn-localization/684-translations.json --export "$EXPORT"
```

## gpt-5.5 API specifics (reasoning model)

- OpenAI Chat Completions; **no `temperature`** (HTTP 400); `max_completion_tokens` (not
  `max_tokens`), sized to field length; `reasoning_effort=low`.
- OpenAI direct key (`.keys/openai-key.txt`, `sk-proj-`) primary; OpenRouter (`openai/gpt-5.5`,
  `.keys/openrouter-key.txt`) automatic fallback on 401/429. See memory
  `reference-gpt55-reasoning-model-api`, `reference-openrouter-gpt55-path`.

## DoD gates (enforced by the verifier)

1. **Cell parity** — every populated FR cell has all 7 target langs present.
2. **Empty stays empty** — the 7 structurally-empty prose cells are ABSENT (no fabrication).
3. **HTML preserved** — the tag set of each translation == the FR source tag set.
4. **Script correctness** — Cyrillic (ru), CJK (zh), Arabic-script (ar/fa), Latin (en/pt/es); no
   FR-contaminated cells (#216-style).

## Gate boundaries (HARD)

- ❌ **Zero prod write** — neither script touches the live DNN DB, portal, or 2sxc entities. The
  artifact is a **re-import-ready staging file**; the actual 2sxc re-import is jsboige-gated (DB
  write). See `684-translation-run-report.md` for the re-import mapping.
- ❌ Does not modify `Cards/Rules/` CSV (game-content is a separate lane).
- ❌ Does not declare a QA verdict — that's ai-01.

---

# #457 T1 — site-content pipeline (2sxc v21 export ⇄ CSV ⇄ import XML with dimensions)

Bricks 1 and 2 of the site chain already existed (the committed export, and
`translate_game_rules.py`). What did **not** exist was the **CSV pivot** and the **import XML
carrying language dimensions** — the two the dispatch asks for. Both live in
`site_content_pipeline.py`. Full detail, measurements and open items:
[`docs/dnn-localization/457-t1-site-content-pipeline.md`](../../docs/dnn-localization/457-t1-site-content-pipeline.md).

| Brick | What | Status |
|---|---|---|
| (1) 2sxc → JSON | committed export, app 60 | ✅ `release-validation/exports/DNN-Argumentum-export-2026-07-07/` |
| (2) JSON → translations | `translate_game_rules.py` → `684-translations.json` | ✅ DoD-verified 0 violations (28 cells × 7 langs) |
| **(3) JSON → CSV** | the pivot — a `key` plus `fr` plus the 7 target columns | 🆕 `site_content_pipeline.py to-csv` |
| **(4) CSV → import XML** | one `<Entity>` per (Guid, Language) | 🆕 `site_content_pipeline.py to-xml` |

## Files

- `site_content_pipeline.py` — `to-csv` / `to-xml` / `self-test` / `list-cultures`.
- `fixtures/one-entity-game-rule-import.xml` — the *"one entity before volume"* payload
  ("L'école des menteurs", 8 culture blocks), consumed by
  `Argumentum.AssetConverter.Tests/SiteContentImportXmlInteropTests.cs`.

## Quick start

```bash
# export + committed translations -> pivot CSV (52 rows, 28 translated)
python site_content_pipeline.py to-csv --translations \
    ../../docs/dnn-localization/684-translations.json --out site-content.csv

# CSV -> import XML for ONE entity (the dispatched proof), PROVISIONAL cultures
python site_content_pipeline.py to-xml --csv site-content.csv \
    --entity ae1edefa-6f1b-4593-8230-97fa1edf4f78 --unconfirmed-ok --out one-entity.xml

# offline proof: 2 round-trips + controls, zero network, zero prod write
python site_content_pipeline.py self-test        # also step [5/5] of test_roundtrip.py
```

## The dimension axis — attested vs unconfirmed

The dialect expresses a language as one `<Entity>` block per (Guid, `Language`). Attested,
**measured on the live DB** (I2, 2026-10-09 — dimensions are zone-scoped and app 60 lives in
zone 3): **fr-FR** (dimensionId **6**) and **en-US** (dimensionId **7**). The first version of
this table said 4/3, copied from the export's `manifest.json` — those are **zone 2's** IDs (the
dead portal 0), and the same manifest declares `argumentumAppZoneId: 3` (see
`1781-i2-zone3-inventory.md` §1). The other six culture codes have **no row anywhere** in
`TsDynDataDimension` (#682 Path A, now measured). `to-xml` therefore **refuses** to emit them
unless `--unconfirmed-ok` is passed, so a file whose culture codes were invented cannot ship
silently. `list-cultures` prints the table.

## What is proven, and what is NOT

- ✅ **Proven offline**: the CSV carries the FR source byte-exactly (RT1), and the XML carries
  every language cell byte-exactly across the dimension axis (RT2) — 17 checks, all named.
- ❌ **Not proven, and claimed nowhere**: that 2sxc v21 *accepts* this XML on import. That is
  the T1+I3 unknown and it is settled on the live portal, on one entity, before any volume.
