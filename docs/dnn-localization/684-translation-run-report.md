# 2026-07-10 — #684 Game Rule prose translation — RUN REPORT (prep, 0 prod write)

**Status:** **PREP DELIVERED.** 196/196 cell-languages translated (28 cells × 7 langs), all DoD
gates **PASS** (exit 0). This is a **staging artifact**, not a prod mutation — the 2sxc re-import
is jsboige-gated (DB write). Triggered by ai-01 dispatch `l66yzc` (primaire). Base: origin/master
`7796c127`.

**Repo reference:** branch `feat/684-rule-prose-translation-7lang`. Issue: #684. Owner:
po-2023 (translation prep) + jsboige (DB re-import). Export source: read-only Method B SQL
(`myia-web1`, 2026-07-07), synced to shared-state.

> **⚠ Status = prep only.** No DNN/2sxc DB write, no `Cards/` CSV mutation, no re-import executed.
> The artifact is re-import-ready; the actual write is gated on jsboige.

---

## TL;DR — what was produced

- **Volume**: **196 translation units** = 28 FR source cells (6 prose fields × 5 games, +5 titles −
  7 structurally-empty prose cells) × **7 target langs** (en/ru/pt/es/ar/fa/zh).
- **Provider**: gpt-5.5 via **OpenAI direct** (`sk-proj-`, LIVE — 0× 429, 0× fallback to
  OpenRouter). `reasoning_effort=low`, no `temperature`, `max_completion_tokens` sized to field.
- **Quality gates**: **196/196 HTML tags preserved**, **196/196 script-correct** (0 FR-contamination,
  #216-style guard), **0 fabrication** (7 empty cells stay empty across all langs).
- **Wall time**: ~18 min of gpt-5.5 API (heaviest cells: e11378/e11389 `Content` @ 15.6k/7.8k chars,
  22–70 s/call; titles + short fields 1–5 s/call).

---

## §1 — DoD gates (all PASS, exit 0)

Verified by `tools/dnn_i18n/verify_game_rule_translations.py`:

| Gate | Rule | Result |
|------|------|--------|
| **G1 — Cell parity** | every populated FR cell → 7 langs present | ✅ 28/28 × 7 = 196/196 |
| **G2 — No fabrication** | the 7 structurally-empty prose cells absent | ✅ 0 present |
| **G3 — HTML preserved** | tag set(translated) == tag set(FR source) | ✅ 196/196 |
| **G4 — Script correct** | Cyrillic(ru) / CJK(zh) / Arabic-script(ar,fa) / Latin(en,pt,es); no FR-contamination | ✅ 196/196 |

**G4 note (honest):** the Arabic/Persian guard is two-sided — rejects FR diacritics (the #216
failure mode) OR zero Arabic letters. It is deliberately *lenient on ratio* for symbol/emoji-heavy
cells (e.g. e11378 Memo = a scoring diagram of 🥇👇➜🏆 where the only textual spans are the two
`<h3>` titles, correctly translated: شروط الفوز / شرایط پیروزی). A positive Arabic-letter *ratio*
would false-fail those; the contamination guard does not.

---

## §2 — Source → artifact mapping (re-import address)

The artifact `684-translations.json` mirrors the 2sxc EAV export shape so re-import is mechanical:

```json
{
  "_meta": { "source": "12-game-rule-content-items.json", "model": "gpt-5.5", ... },
  "entities": {
    "11378": {                               // EntityID (2sxc primary key)
      "title": "L'école des menteurs",       // FR title (reference)
      "fields": {
        "Summary": { "fr": "<p>...</p>", "en": "...", "ru": "...", "pt": "...",
                      "es": "...", "ar": "...", "fa": "...", "zh": "..." },
        ...
      }
    }, ...
  }
}
```

**Re-import address**: `EntityID` + `StaticName` (field) + target `DimensionID` (lang) = the exact
2sxc attribute-value coordinate. The export manifest names the dimension IDs:
`frFR_dimensionId=4` (source), `enUS_dimensionId=3`; the other 5 langs need their dimension
provisioned in 2sxc (Path A, #682) before re-import can land values.

| EntityID | FR title | Fields translated | Note |
|----------|----------|-------------------|------|
| 11378 | L'école des menteurs | Title + all 6 prose (Summary/Material/Installation/Content/Variants/Memo) | only game with Variants + Memo |
| 11380 | Le Bingo mixologie argumentative | Title + Summary/Material/Installation/Content | no Variants, no Memo |
| 11387 | Le dernier beau parleur | Title + Summary/Material/Installation/Content | no Variants, no Memo |
| 11388 | Le moulin à baratin | Title + Summary/Material/Installation/Content/Variants | Memo empty |
| 11389 | La parlote coinchée | Title + Summary/Material/Installation/Content | no Variants, no Memo |

---

## §3 — Reproduce

```bash
EXPORT="$ROOSYNC_SHARED_PATH/attachments/DNN-Argumentum-export-2026-07-07/12-game-rule-content-items.json"
# Translate (resume-capable; the output doubles as cache)
python tools/dnn_i18n/translate_game_rules.py --export "$EXPORT" \
    --out docs/dnn-localization/684-translations.json --all
# Verify (exit 0 = all DoD gates pass)
python tools/dnn_i18n/verify_game_rule_translations.py \
    --artifact docs/dnn-localization/684-translations.json --export "$EXPORT"
```

Stdlib only (no SDK); `.keys/openai-key.txt` primary, `.keys/openrouter-key.txt` automatic fallback.

---

## §4 — Open items for jsboige (re-import gate)

1. **2sxc dimension provisioning** (#682 Path A): the 5 non-FR/non-EN langs (ru/pt/es/ar/fa/zh) need
   their culture dimensions created before translated values can land. The artifact is ready; the
   sink is not.
2. **Title translation policy**: titles are translated (additive/reversible). If a FR brand is
   preferred for a game title (e.g. "L'école des menteurs" as a proper noun), keep FR at re-import —
   the artifact's translated titles are optional, not forced.
3. **RTL templates** (ar/fa): the localized HTML text is produced; page-level `<html dir="rtl">` +
   Arabic/Persian font stack is a separate DNN-template step (#457 §6).
4. **Re-import verifier** (post-write): a cell-by-cell scan mirroring the G4 guard confirms 0
   FR-contaminated cells in the live DB after jsboige's import.

---

## §5 — Gate boundaries (HARD)

- ❌ **Zero prod write** — neither script touches the live DNN DB, portal, or 2sxc entities.
- ❌ Does not modify `Cards/Rules/` CSV (game-content is a separate lane).
- ❌ Does not declare a QA verdict — that's ai-01. This report states DoD-gate facts, not a release PASS.

Relates: dispatch `l66yzc` (primaire), #684, #681 (export), #682 (dimension provisioning),
#669 (mechanism), #457 §6 (RTL templates), #216 (FR-contamination failure mode this guards against),
#458 (epic).
