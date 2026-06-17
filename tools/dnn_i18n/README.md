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
