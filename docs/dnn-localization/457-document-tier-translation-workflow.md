# #457 — Document-Tier Translation Workflow (content-type C: static HTML pages)

**Status:** **DOC / design / non-gated.** The Phase 2–4 workflow design for translating the **document-tier** DNN content (the 2 standalone FR HTML pages identified in the Phase 1 inventory). **No prod mutation, no live DNN touch, no extraction executed.** This is the *how* that the [Phase 1 inventory](457-site-content-type-inventory.md) §3 deferred: the inventory names *what* to translate; this doc designs *how* the extraction → translation → re-import pipeline works for the document tier specifically.

**Why a separate doc from the string tier:** the string-tier content-types A/B (`ui.*`/`res.*`) already have a complete toolchain (extractor + DatasetUpdater config + re-import verifier, PRs #524/#487, see [tools/dnn_i18n/README.md](../../tools/dnn_i18n/README.md)). Content-type C is **prose**, not key-value — it needs a different extraction shape, a different DatasetUpdater task shape, and a different re-import path. Lumping them together would mis-spec both.

---

## §1 — What "document-tier" means here (scope)

From the Phase 1 inventory §3, the document-tier content is **2 standalone FR HTML pages**, not CSV-driven, not wired to any 2sxc query:

| File | Size | ~Visible text | Shape | Role |
|------|------|---------------|-------|------|
| `DNNPlatform/fallacies/fallacies.html` | 81 KB | ~24,200 chars | 2 `<h2>` + 2 `<h3>` + 10 `<li>` — a structured **integration charte** ("Charte Html pour l'identification des arguments fallacieux") | Educational integration doc with worked fallacy examples |
| `DNNPlatform/fallacies/MariagePourTous.html` | 38 KB | ~13,100 chars | Prose, no semantic headings | Standalone analysis article ("L'analyse rhétologique pour tous") |

Total: ~37,300 chars of FR prose across 2 files. These are **standalone FR content** — translating them is a document/prose task (like Scenarii narrative fields), **not** a key-value dictionary task (like `ui.*`/`res.*`).

**Out of scope of this doc (covered elsewhere):**
- String tier (`ui.*`/`res.*`) → [tools/dnn_i18n/README.md](../../tools/dnn_i18n/README.md) + DatasetUpdater config #487.
- Fallacies Explorer / Rules Explorer → already CSV-driven via 2sxc queries (taxonomy CSVs already localized 8-lang).
- Glossary / nav / SEO meta → Phase 1 inventory §2, separate lanes, not document-tier.

---

## §2 — Phase 2: CSV schema (document-tier ≠ string-tier)

The string-tier `dnn-ui-strings.csv` dialect (one row per key) does **not** fit prose. The document tier needs a **segment-oriented** schema, one row per translatable segment, so the LLM gets manageable chunks and re-import can map segments back to their source location.

### Proposed schema

```csv
segment_id,source_path,selector,field_fr,field_en,field_ru,field_pt,field_es,field_ar,field_fa,field_zh
fallacies.html#h2-1,DNNPlatform/fallacies/fallacies.html,h2:nth-of-type(1),"Identification des arguments fallacieux","","","","","","",""
fallacies.html#li-3,DNNPlatform/fallacies/fallacies.html,li:nth-of-type(3),"<full FR text of that list item>","","","","","","",""
MariagePourTous.html#p-7,DNNPlatform/fallacies/MariagePourTous.html,p:nth-of-type(7),"<full FR paragraph>","","","","","","",""
```

- **`segment_id`** = `<file>#<element>-<index>` — the `PrimaryField` for DatasetUpdater, stable across regenerations.
- **`source_path`** + **`selector`** = the re-import address (which element in which file). Stored so re-import is mechanical, not heuristic.
- **`field_fr`** = the source text (extracted verbatim, HTML inner-text or outer-HTML per segment type — decision in §4).
- **`field_{lang}`** = the 7 translation targets (empty, to be filled by DatasetUpdater).

### Why segment-oriented, not whole-document

- gpt-5.5 translation quality degrades on very long single prompts; chunked segments give better fidelity and parallelizable API calls.
- Re-import becomes a deterministic selector-based patch (find element → replace inner content), not a fragile full-file diff.
- Segments can be reviewed individually (jsboige QA gate, AR/FA RTL especially).

---

## §3 — Phase 2: DatasetUpdater task shape

The existing DatasetUpdater config pattern (see `DatasetUpdaterRootConfig.cs`, string-tier task #487 at lines ~2636–2696) is reusable but needs a **document-tier task** variant. Key differences from the string-tier task:

| Aspect | String-tier (`ui.*/res.*`) | Document-tier (this lane) |
|--------|----------------------------|---------------------------|
| Source | `dnn-ui-strings.csv` (key→value) | `dnn-document-segments.csv` (segment→prose) |
| `FieldsToUpdate` | the `value_*` cells | `field_en,field_ru,...,field_zh` (prose) |
| Prompt | "translate this UI label, keep concise, UI-context" | "translate this educational prose, preserve structure/HTML inline, register = pedagogical, keep fallacy names consistent with the taxonomy CSV" |
| Chunking | by row count (`ChunkSize`) | by **character budget** (segments grouped to stay under token limit; the 81KB charte must be split across many chunks) |
| QA burden | Low (short labels) | High (prose, cultural register, AR/FA RTL, consistency with taxonomy FR→XX glossary) |

**Config decisions to make when enabling (jsboige gate):**
- `Model` = `gpt-5.5` (quality prose) — note the OpenAI key is live (decision #11, recharge); OpenRouter path is the proven fallback if OpenAI 429s ([reference-openrouter-gpt55-path](../../tools/dnn_i18n/) memory).
- `reasoning: { effort: "low" }` + `max_output_tokens` ≥ 7000 for chunked batches (gpt-5.5 gotcha, [reference-gpt55-reasoning-model-api]).
- `Enabled = false` until jsboige GO — same discipline as #487.

---

## §4 — Phase 1.5: the extractor (net-new, the real blocker)

There is **no extractor** for the document tier today — `extract_dnn_ui_strings.py` is scoped to `.cshtml` anchors (string tier A/B). The document-tier extractor must:

1. **Parse the HTML** (stdlib `html.parser` to match the zero-dependency discipline of the existing tools, no BeautifulSoup dependency).
2. **Select translatable elements** per file — for `fallacies.html`: `h2, h3, li` (structured charte); for `MariagePourTous.html`: `p, h2, h3` (prose). The selector set is **file-specific** (codified in the inventory §3), not heuristic.
3. **Extract inner-text** (translate the text, not the HTML wrappers) OR **outer-HTML** (if structure must travel). **Recommendation: inner-text for prose elements, and store the selector so re-import re-wraps** — keeps translation clean of HTML noise.
4. **Emit the §2 CSV** with stable `segment_id`s.

This is the **net-new tool** (analogous to `extract_dnn_ui_strings.py` for brick 1). It is the gating deliverable before any translation can run.

---

## §5 — Phase 4: re-import (selector-based patch, deterministic)

Re-import is the inverse of extraction: for each segment, find the element via `source_path` + `selector`, replace its inner-text with the translated `field_{lang}`, write the localized HTML file.

- A dry-run verifier (analogous to `reimport_dnn_ui_strings.py verify`) should confirm: (a) every `segment_id` resolves to exactly one element, (b) the translation fits the element without breaking surrounding structure, (c) zero prod write (fixture-first, same DoD as the string tier round-trip test).
- **Output per language**: a localized HTML file per source page (`fallacies.en.html`, `fallacies.ar.html`, …). How these are **served** by DNN (URL routing `/en/`, RTL `dir="rtl"` for ar/fa) is a DNN-config step gated on jsboige — documented here, not executed.

---

## §6 — RTL / CJK considerations (AR/FA/ZH specific to prose)

The string-tier RTL/CJK handling (font fallbacks Noto Naskh Arabic / Noto Sans SC / Vazirmatn, `dir="auto"`) was solved for cards in PR #359. For **document-tier HTML**, the same applies but at the page level:

- AR/FA: the localized HTML page needs `<html dir="rtl" lang="ar">` and the Arabic/Persian font stack in its CSS.
- ZH: `lang="zh"` + Noto Sans SC font stack.
- These are **template/render concerns**, not translation concerns — the re-import step produces the text; a separate template variant per RTL/CJK language wraps it. Decision for jsboige: one HTML template with dynamic `dir`/`lang`, or one file per language.

---

## §7 — Sequencing (what unblocks what)

```
[Phase 1.5] extractor (§4, net-new tool)
      │  ← gating deliverable (nothing translates until this exists)
      ▼
[Phase 2] CSV extracted + DatasetUpdater document-tier task enabled (§3, jsboige GO)
      ▼
[Phase 3] translation EN/RU/PT first → then ES/AR/FA/ZH (QA gate per language)
      ▼
[Phase 4] re-import dry-run verify (§5) → localized HTML files
      ▼
[Phase 5] DNN serve (URL routing, RTL/CJK templates, jsboige live DNN step)
```

The extractor (§4) is the critical path. Everything downstream is config + API calls + jsboige gates.

---

## §8 — Open questions for jsboige (block the live portion)

1. **Inner-text vs outer-HTML** extraction (§4): translate clean text and re-wrap on import (recommended), or translate full HTML fragments? (Outer-HTML risks the LLM mangling markup.)
2. **One template + dynamic dir/lang, or one file per language** (§6)?
3. **These 2 pages — are they still served on the prod site?** (If one is deprecated, drop it from scope before translating 8×.)
4. **GO to build the extractor** (Phase 1.5)? — doc/design only until this GO.

---

## §9 — Out of scope (explicit)

- Building the extractor now (§4) — design only; the tool is the gating deliverable once jsboige GOs.
- Enabling the DatasetUpdater task (§3) — `Enabled=false` until GO.
- Any live DNN mutation / serving / routing config.
- Touching the string-tier toolchain (already complete).

## Sources

- [457-site-content-type-inventory.md](457-site-content-type-inventory.md) §3 (the 2 HTML pages, the "separate translation lane" recommendation)
- [tools/dnn_i18n/README.md](../../tools/dnn_i18n/README.md) (the string-tier toolchain this parallels)
- Issue #457 body (Phase 2–4 sub-tasks #N6–#N18, DatasetUpdater CSV format reference)
- `DNNPlatform/fallacies/fallacies.html` (81 KB), `DNNPlatform/fallacies/MariagePourTous.html` (38 KB)
- PR #359 (8-language card RTL/CJK font handling, reusable for document tier)
- Memory: [reference-gpt55-reasoning-model-api], [reference-openrouter-gpt55-path] (gpt-5.5 / OpenRunner config for the translation task)
