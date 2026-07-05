# 2026-07-05 — #684 Rule prose translation manifest (~30 entities × 7 langs) — prep

**Scope**: the **dominant cost** of the DNN i18n portage (#669 §3). Once the Rule content-type is
provisioned (#682), the view refactor validated (#674), and DNN serves cultures (#683), translate the
~30 rich-HTML prose entities into the 7 target languages and re-import to 2sxc. This doc is the
**manifest** (inventory + chunking plan + DatasetUpdater task sketch) — `Enabled = false`, no execution
until gates clear. Triggered by ai-01 dispatch `0hbg9t` (secondary). Prolongs #692.

**Repo reference**: master `d90ce613`. Issue: #684 (rule prose translation). Owner: worker
(translation + re-import prep) + jsboige (DB re-import). Blocked-by: #681 (export), #682 (field
provisioning), #674 (view refactor), #683 (DNN cultures).

> **⚠ Status = manifest/prep only.** No translation executed, no API calls, no DB write. The exact
> entity list lives in the prod DB (gated on #681 export); this manifest defines the *structure*,
> *volume estimate*, *chunking strategy*, and *task config* so execution is one-step once gates clear.

---

## TL;DR

- **Volume**: ~30 rich-HTML prose entities (6 fields × ~5 games) × **7 target langs** ≈ **210
  translation units**. Each entity ~500–2000 chars FR source → ~500–2000 chars target.
- **Method**: chunked **gpt-5.5** (Scenarii-style DatasetUpdater task), `SequentialChunks` with
  `ChunkSize ≥ 12`, OpenRouter path (`openai/gpt-5.5`). ~minutes-to-tens-of-minutes of API time per
  language once the task config is in place.
- **Source**: FR entities exported from the 2sxc Rule content-type (via #681, jsboige). **NOT the
  Rules CSV** (#692 confirmed CSV is a secondary reference, not the source — site content is richer
  HTML, DB-only, per #669 §2c).
- **Sink**: the 49 suffixed fields provisioned in #682 (Summary_en/Material_ru/…/Memo_zh).

---

## 1. Entity inventory (structure; exact list gated on #681 export)

Per #669 §2c + #682, the Rule content-type translatable prose fields are **6 rich-HTML fields**
(EntityTitle is short text, translated cheaply but treated separately):

| Field | Type | Content shape |
|-------|------|---------------|
| `Summary` | rich HTML | Rule summary paragraph(s) |
| `Material` | rich HTML | Material list |
| `Installation` | rich HTML | Setup/installation section |
| `Content` | rich HTML | Full rule content (the bulk) |
| `Variants` | rich HTML | Game variants |
| `Memo` | rich HTML | Memo card content |

× **~5 games** (the site hosts multiple games' rule pages — exact count gated on #681 export, #457
inventory content-type D) = **~30 prose entities** (+ ~5 EntityTitle short strings).

× **7 target langs** (en/ru/pt/es/ar/fa/zh) = **~210 translation units** (+ ~35 EntityTitle).

> **Honest uncertainty**: the exact entity count (is it 5 games? more? do all games have all 6
> fields populated?) **cannot be confirmed without the #681 export**. The ~30 figure is the
> authoritative estimate from #669 §3. The manifest structure holds regardless of the exact count.

---

## 2. Chunking strategy (Scenarii-style DatasetUpdater)

Per [[reference-datasetupdater-operational-lessons]] + [[reference-scenarii-chunking]]:

- **Chunk mode**: `SequentialChunks` (NOT `PKHierarchicalChar` — flat PKs, no deep hierarchy).
- **ChunkSize**: `≥ 12` (Scenarii-validated; smaller = more API overhead, larger = context-budget risk
  on rich HTML).
- **Per-language pass**: one language at a time (en, then ru, then pt, …). ~30 entities × 6 fields
  fits comfortably in one chunked pass per language.
- **HTML preservation**: the source is rich HTML — the prompt must instruct gpt-5.5 to **preserve
  HTML tags/structure** and translate only the text content (not the markup). This is the same
  concern as the Scenarii markdown-preservation pattern.

### gpt-5.5 API specifics (from [[reference-gpt55-reasoning-model-api]])

- **Endpoint**: `/v1/responses` with `reasoning: { effort: "low" }`.
- **REJECTS `temperature`** (HTTP 400) — omit it.
- **`max_completion_tokens`** = ~7000 (rich HTML entities can be long; the `Content` field is the
  bulk).
- **OpenRouter path** ([[reference-openrouter-gpt55-path]]): `Model = openai/gpt-5.5` +
  `openrouter-key.txt` + `BaseUrl = openrouter.ai/api/v1`, used when OpenAI direct is 429-exhausted.

---

## 3. DatasetUpdater task config sketch (`Enabled = false`)

The task reads from the **#681 export file** (FR source entities), translates to the target lang, and
writes to a re-import file. **Fields = raw export headers** ([[reference-datasetupdater-csv-dual-path]]
— property-name mismatch = silent no-op).

```csharp
new DatasetUpdaterTaskConfig {
    Name = "RulesProse_i18n",
    Enabled = false,                          // gated on #681 export + #682 provisioning
    Model = "openai/gpt-5.5",                 // OpenRouter path if OpenAI 429-exhausted
    BaseUrl = "https://openrouter.ai/api/v1", // alt: OpenAI direct
    ChunkMode = ChunkMode.SequentialChunks,
    ChunkSize = 12,
    SourceFilePath = "<from #681 export — FR entities>", // jsboige-gated
    OutputFilePath = "<re-import file — populated suffixed fields>",
    Fields = new[] {                           // RAW export headers (verify against #681 export)
        "EntityTitle", "Summary", "Material", "Installation", "Content", "Variants", "Memo"
    },
    TargetLanguages = new[] { "en", "ru", "pt", "es", "ar", "fa", "zh" },
    // gpt-5.5 specifics:
    //   no temperature, max_completion_tokens ~7000, reasoning effort low
    //   HTML-preservation prompt instruction
}
```

> **Prompt**: a new `Rules_i18n_preserve_html.txt` in `DatasetUpdater/Resources/` — instructs
> gpt-5.5 to translate text content only, preserve all HTML tags/attributes/structure, keep
> game-specific terminology consistent (use the Rules CSV #692 as terminology anchor).

---

## 4. Re-import plan (jsboige, post-translation)

1. **Entity count parity**: re-imported entity count = source count (no field loss, no row drop).
2. **Field mapping**: each translated value lands in its suffixed field (`Summary_en`, `Memo_zh`, …).
3. **0 FR-contaminated cell** guard (#216-style): post-import scan — for each non-FR cell, verify
   script matches the target lang (Cyrillic for ru, CJK for zh, RTL for ar/fa). The Rules CSV scan
   (#692) is the model for this.
4. **Harvest invariance**: if any rule content feeds an asset (harvest), verify image-count
   invariance (#216-style guard) — though RulesExplorer content is site-only (not harvested into
   PDFs), so this is likely a no-op. Confirm during #685 visual QA.

---

## 5. Effort estimate (per language)

| Sub-task | Volume | Est. effort |
|----------|--------|-------------|
| Rule prose (6 fields × ~5 games ≈ 30 entities) | ~30 × 7 langs | **dominant** — minutes-to-tens-of-minutes gpt-5.5 API per lang |
| EntityTitle (short) | ~5 × 7 | negligible |
| App Resources (`@Resources.*` FR values, DB-only) | ~8 × 7 | small-moderate (post-#681) |
| UI strings (`de X à Y joueurs`, 10 keys) | 10 × 7 | seconds (#487 rail, flip `Enabled`) |

**Per-language total**: one DatasetUpdater pass (comparable to a Scenarii 167-record × 8-field pass).
The **prep** (#681 export + #682 provisioning + #674 view + #683 config) is the real schedule
driver, not the translation itself (#669 §3).

---

## 6. DoD status

| DoD item (dispatch `0hbg9t` secondary) | Status |
|----------------------------------------|--------|
| Inventory of ~30 prose FR-only entities | ✅ Structure (6 fields × ~5 games), exact count gated #681 |
| gpt-5.5 chunking plan | ✅ SequentialChunks ≥ 12, OpenRouter path, HTML preservation |
| DatasetUpdater task config sketch (`Enabled = false`) | ✅ §3 |
| Prep for #684 | ✅ Manifest delivered; execution gated on #681/#682/#674/#683 |

---

## Gate boundaries (HARD — manifest only)

- ❌ No translation executed, no API calls, no DB write, no CSV mutation.
- ❌ No #674 merge, no régén launch.
- ✅ Manifest derived from merged investigations (#669, #692, #682) + DatasetUpdater operational
  lessons ([[reference-datasetupdater-operational-lessons]], [[reference-scenarii-chunking]]).
- ✅ Honest about the #681 gate for exact entity enumeration.

Relates: dispatch `0hbg9t` (secondary), #684 (this issue), #681 (export), #682 (provisioning), #674
(view), #683 (DNN config), #669 (mechanism), #692 (CSV coverage / terminology anchor), #487
(UI-strings rail), #458 (epic).
