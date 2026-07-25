# #684 — DNN Rule prose translation harness (Option C pilot-ready)

> **Status:** DESIGN / prep — **0 run, 0 prod CSV, 0 DB mutation**. This is the worker-side
> harness prepared ahead of the `#681` (2sxc App export) artefact, per ai-01 dispatch
> (`msg-20260725T102303-rn9kpj`, `msg-20260725T083424-t6wvjv`). It turns the export wait into
> concrete readiness: when the export drops, translation starts from a calibrated harness, not zero.
>
> **Owner:** po-2024 (translation + re-import prep). **Gated by:** `#681` (export, web1),
> `#682` (Rule field-model decision), PR `#674` (view refactor runtime-valid), `#683`
> (DNN serving cultures). **Tracking issue:** [#684](https://github.com/ArgumentumGames/Argumentum/issues/684).
> **Mechanism source:** [#669](https://github.com/ArgumentumGames/Argumentum/pull/669) §3 (variable-cost table, step 5).

---

## 1. Why this harness exists

`#684` is step 5/6 of the DNN i18n portage and the **dominant cost**: translate the site Rule
prose (~30 rich entities × 7 languages) and re-import to 2sxc. The source is **DB-only** — it
comes from the `#681` export, **not** from any in-repo CSV. The 8 existing Rules CSV translations
are **not reusable as-is** (different field model, richer HTML, manually authored) — see `#669` §2c.

Two things must be ready the instant the export lands, or the campaign stalls:

1. **The Option C routing** — every gpt-5.5 translation task in the fleet is currently wired to
   legacy Chat Completions (`UseResponsesApi` unset on 0/48 tasks, per `#855`). For reasoning
   models, Chat Completations burns the budget on hidden reasoning and returns **empty Content**.
   The Responses-API path exists (`#852`, merged, default off) but is dormant. The first
   `Enabled=true` flip without Option C = silent empty output. This harness closes that gap for
   the Rule-prose task specifically.
2. **The verification protocol** — reasoning-model output must be checked **cell-by-cell**, not
   by row-count or matcher hit-rate (a no-match means the algorithm didn't align, not that content
   is absent — see `#656`; a token-only sweep gives a false 0 — see `#803` r7 residual).

This document specifies the task config (Option C knobs decided; field model pending `#682`),
the 5-record pilot, the cell-by-cell protocol, the 7-language acceptance grid, and the report
format. **Nothing here runs until the export exists.**

---

## 2. Decided vs pending

| Knob | Value | Status |
|------|-------|--------|
| `Model` | `gpt-5.5` | **Decided** (mandated; no lower tier — `#684` stage msg) |
| `UseResponsesApi` | `true` | **Decided** (Option C, `#855` reco / `#852` plumbing) |
| `ReasoningEffort` | `"low"` | **Decided** (dashboard metric; caps reasoning so Content is non-empty) |
| `MaxOutputTokens` | `7000` | **Decided** (dashboard metric) |
| `UseFunctionCalling` | `true` | **Decided** (all gpt-5.5 tasks; `UpdateRecord` parallel tool calls) |
| `DivisionMode` / `ChunkSize` | `SequentialChunks` / `3` | **Decided** (matches Rules/Scenarii tasks) |
| `Enabled` | `false` | **Decided** (held until export + `#682` + pilot PASS) |
| `SystemPromptPath` | `PromptGeneralSystem.txt` (shared) | **Decided** |
| Field model (lang-suffixed vs EAV) | lang-suffixed **recommended** | **Pending `#682`** |
| Source field names (`Summary`/`Material`/…) | from `#669` §2c | **Pending export verification** |
| `SourceDataset` / `TargetPath` | n/a today (DB-only) | **Pending `#681` export** |

**Why no compiled config entry yet:** the source prose lives in the 2sxc DB and arrives via the
`#681` export — there is no `KnownDataSets.DnnRuleProse` today, and the field model is undecided
(`#682`). Adding a compiled entry with guessed field names would be dead code that *looks* ready.
The snippet below is the ready-to-paste form; it is instantiated once the export format is known.

---

## 3. Task config (Option C, ready-to-paste)

Pattern = the existing `Translate Rules to Portuguese by chunk` task (`DatasetUpdaterRootConfig.cs`
L772-814) **plus the Option C knobs it is missing**. One task **per language** (7 total), matching
the fleet convention (Fallacies→EN / →RU / →PT are separate tasks). Below is the **EN** variant;
replicate per language by swapping the `_en` suffix and the prompt paths.

```csharp
new DatasetUpdaterConfig()
{
    Enabled = false,                          // HARD: held until #681 export + #682 + pilot PASS
    Name = "Translate DNN Rule prose to English by chunk 0-shot",
    // SourceDataset = KnownDataSets.DnnRuleProse,   // TODO #681: register once export lands
    // TargetPath   = @".\Target\Datasets\DnnRuleProse.csv", // TODO #681: export path
    FieldsToInclude = new List<string>()
    {
        "pk",
        "Summary", "Material", "Installation", "Content", "Variants", "Memo",   // FR source (#669 §2c)
        "Summary_en", "Material_en", "Installation_en", "Content_en", "Variants_en", "Memo_en",
    },
    FieldsToUpdate = new List<string>()
    {
        "Summary_en", "Material_en", "Installation_en", "Content_en", "Variants_en", "Memo_en",
    },
    PrimaryField = "pk",
    SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
    DialogPrompts = new List<PromptExample>()
    {
        new PromptExample()
        {
            UserPromptPath = PromptsRootPath + "PromptDnnRuleProseTranslateEnUser.txt",
            AssistantAnswerPath = PromptsRootPath + "PromptDnnRuleProseTranslateEnAssistant.txt",
        },
    },
    Model = "gpt-5.5",                        // mandated, no lower tier
    UseResponsesApi = true,                   // Option C (#855 reco) — closes the empty-Content gap
    ReasoningEffort = "low",                  //   caps reasoning so output Content is non-empty
    MaxOutputTokens = 7000,                   //   dashboard metric
    UseFunctionCalling = true,                // UpdateRecord parallel tool calls
    MaxTokensPerMinute = 70000,
    DivisionMode = DivisionMode.SequentialChunks,
    ChunkSize = 3,
    NbMessageCalls = 1,
    SkipChunkNb = 0,
    TakeChunkNb = -1,                         // pilot: set to 2 (ChunkSize 3 → ~5-6 records)
    SelectEmptyTargets = true,                // only rows missing the target lang
    RandomizeChunks = false,
    MaxDegreeOfParallelismWebService = 3,
    CompareMode = false,
    AutoCompare = false,
    MaxGroupItemNb = 12,
    WriteOneTargetFileByField = false,
    MaxChildren = 8,
},
```

**Per-language replication:** duplicate the block for `ru`/`pt`/`es`/`ar`/`fa`/`zh`, swapping the
`_<lang>` suffix in `FieldsToUpdate`/`FieldsToInclude` and the prompt paths. The
`GetRecordsUpdaterToolDefinitions` helper auto-derives `langSuffix` from the first
`FieldsToUpdate` entry (`Split('_').LastOrDefault()`), so the `UpdateRecord` tool description
names the right target field automatically.

**If `#682` decides EAV instead of lang-suffixed:** `FieldsToUpdate` becomes the EAV dimension
keys (e.g. `[ "en" ]` against an entity-value table) and the prompt is adjusted — the Option C
knobs are unaffected.

---

## 4. Option C 5-record pilot (`#855` reco)

**Goal:** before the ~30 × 7 campaign, confirm that Option C (Responses API + `effort=low`) yields
**usable, non-empty Content** for this content class — Option C could not be tested by `#299`
(the Responses path didn't exist then).

**Protocol:**
1. Take the **first 5 Rule entities** from the `#681` export (deterministic order — no randomization).
2. Set `TakeChunkNb = 2`, `ChunkSize = 3` → processes 2 chunks (~5-6 records).
3. Run **EN only** (one language — the richest reference source after FR).
4. Inspect the output **cell-by-cell** against §5.
5. **Acceptance:** 0 empty Content cells, 0 FR-contaminated cells, all 6 prose fields populated
   per entity, HTML structure preserved.

**Abort conditions:**
- Any empty-Content cell → Option C knob wrong (verify `UseResponsesApi`/`ReasoningEffort` wired);
  do **not** scale to other languages.
- FR contamination in >1 cell → prompt insufficient; revise `PromptDnnRuleProseTranslate*` before
  scaling.
- HTML structure broken (dropped tags, unbalanced) → add an explicit HTML-preservation clause to
  the user prompt.

**On PASS:** scale to the remaining 6 languages, then the full ~30 entities. Log the pilot result
in the report (§7) and post to `#684`.

---

## 5. Cell-by-cell verification protocol

Re-runs are verified **cell-by-cell**, never by row-count or matcher hit-rate. Three dimensions
must all pass (lesson `#803` r7: a token-only sweep gives a false 0; lesson `#656`: a matcher
no-match ≠ content absent).

For each `(entity, field, lang)` cell:

| Dimension | Check | Fail signal |
|-----------|-------|-------------|
| **D1 — non-empty** | target cell is non-empty, non-whitespace | empty Content (Option C knob failure) |
| **D2 — not FR-contaminated** | no French lexeme heading the cell; no untranslated FR sentence. Sweep 3 signals: (a) FR flag tokens, (b) FR headings (e.g. a cell whose `Summary_en` starts with a FR word), (c) FR grammar in an EN/ES/PT cell | any hit = contamination |
| **D3 — language-correct** | `ru` = Cyrillic, `ar`/`fa` = the correct script + RTL-safe content, `zh` = CJK (no tofu / no Latin substitution), `en`/`pt`/`es` = Latin with correct diacritics | wrong script, tofu, or Latin-where-CJK |

**RTL/CJK note:** the translation *produces* the text; rendering direction is a view-layer concern
(PR `#674` / `#683`). But the cell content itself must be the natural-script text (e.g. `ar` cells
contain Arabic script, not transliterated Latin). RTL bidi marks are **not** added at translation
time — they are a rendering concern.

**HTML preservation:** source prose is richer HTML than CSV. Verify tags survive translation:
`<strong>`, `<em>`, `<a href>`, lists — the translator must not drop, unbalance, or translate
inside tag names. Placeholder markers `{0}`/`{1}` (player-count ranges, counts) are preserved
**exactly** at the equivalent position (lesson from `#487` UI-strings rail).

**FR-relative measure:** before flagging a cell "untranslated", compare it to the FR source — a
legitimate proper noun (Sherlock, Jeanne d'Arc, Ergo sum) legitimately overlaps across languages
(lesson from Scenarii `Title=FR` overlaps). Do not batch-replace overlaps without this measure.

---

## 6. 7-language acceptance grid (DoD, from `#684`)

Per-language checklist. A language is **done** when every cell passes D1+D2+D3 AND the re-import
is verified (entity-count parity, no field loss).

| Lang | Script | Extra check |
|------|--------|-------------|
| `en` | Latin | richest reference; pilot runs here first |
| `ru` | Cyrillic | Cyrillic renders (no Latin substitution) |
| `pt` | Latin | diacritics (ã/õ/ç) correct |
| `es` | Latin | diacritics (ñ/¿/¡) correct |
| `ar` | Arabic | RTL content; modern standard Arabic (فصحى) |
| `fa` | Persian | RTL content; Persian script (not Arabic-fit) |
| `zh` | CJK | simplified Chinese; no tofu, no Latin substitution |

Plus (post-translation, `#684` DoD):
- [ ] App Resources values translated (`res.Rule*`, incl. `res.RuleMemoInstructions` skipped by `#490`).
- [ ] Re-import to 2sxc DB verified (entity-count parity, no field loss).
- [ ] If rule content feeds any asset: harvest regenerated + image-count invariance (`#216`-style guard).

---

## 7. Report format

Each run produces a markdown report (staged in `docs/dnn-localization/`, not the repo root),
one section per language:

```
## #684 run — <lang> — <date>
- Model: gpt-5.5 / /v1/responses / reasoning.effort=low / max_output_tokens=7000
- Scope: <n> entities × 6 fields, chunk size 3, <n_chunks> chunks
- D1 non-empty:    <pass>/<total>   (failures listed by pk.field)
- D2 not-FR-contam:<pass>/<total>   (failures listed, with the contaminated lexeme)
- D3 lang-correct: <pass>/<total>   (failures listed, with the wrong-script evidence)
- HTML preserved:  yes/no           (any unbalanced/dropped tags listed)
- Placeholders {0}/{1}: preserved yes/no
- Re-import parity: entities <n_in> == <n_out> yes/no
- Verdict: PILOT-PASS / PILOT-FAIL / SCALE / BLOCKED
- Next: <action>
```

**No `*_TEST_REPORT.md` is pushed to the CoursIA repo** (governance); Argumentum docs only.

---

## 8. Serialization & gates (HARD)

- **`Enabled = false`** until: (a) `#681` export artefact exists, (b) `#682` field-model decided,
  (c) Option C 5-record pilot PASS, (d) jsboige T&A lift on `#802`.
- **Do NOT start translation before the `#681` export.** The source prose comes from that export,
  not from CSV. Translating without it = translating the wrong material (ai-01 stage msg
  `msg-20260725T083424-t6wvjv`).
- **0 prod CSV, 0 DB mutation, 0 live re-import** from this harness. Re-import is jsboige-side.
- Visual QA / PASS verdict = **ai-01 only**. This harness reports measurements, never a verdict.

---

## 9. Draft prompt (companion file)

`Resources/PromptDnnRuleProseTranslateEnUser.txt` (this branch) — adapts the `#487` DNN UI-strings
prompt pattern to Rule prose: per-language, function-calling, placeholder + HTML preservation,
RTL/CJK, game-term rules. Marked DRAFT pending the export's concrete field names.

---

## 10. Dependencies / open questions

- `#681` export format (field names, entity count, HTML richness) — resolves the TODOs in §3.
- `#682` field-model decision (lang-suffixed vs EAV) — may rewrite `FieldsToUpdate`.
- Whether Rule prose feeds any regenerated asset (determines the `#216` image-count guard).
- Pilot host: who runs the first `Enabled=true` 5-record pilot (po-2024, once export lands).
