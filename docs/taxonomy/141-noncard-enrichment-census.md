# #141 — Non-card Taxonomy Node Census & Enrichment Proposal

**Author**: po-2024 (worker) · **Date**: 2026-06-29 · **Base**: master `ba8e4a6c`
**Status**: **PROPOSAL / CENSUS** — grounds #141 in the *current* taxonomy state (the issue text is ~2 years old; the DatasetUpdater + translation PRs have moved the state considerably since).
**Scope**: docs + read-only script. **0 write under `Cards/`** (pre-tag freeze). master stays `ba8e4a6c`.
**Reproducibility**: [`141-noncard-census.py`](141-noncard-census.py) — re-run anytime: `python docs/taxonomy/141-noncard-census.py`.

---

## TL;DR — #141 reframed by the census

The issue's **original enrichment target (scope item 3: descriptions + examples + translations on non-card nodes) is DONE** — 100 % across 8 languages on both Fallacies and Virtues non-card nodes. The "GPT-4 enrichment script" it asks to adapt **is already adapted** — it is the modern `DatasetUpdater` (OpenAI SDK v2.10.0 via PR #210, gpt-5.5 primary, multi-provider).

**The genuinely-open residue of #141 is the AIF cross-reference graph** (`crossLink_*` relationship columns + `AIF_skos*` semantic mappings): schema is **ready** (columns exist) but content is **~0 % populated** on non-card nodes. This is exactly scope item 3's "cross-references between related fallacies", and it feeds the dependencies #141 declares (#130 OWL, #136 2sxc). It overlaps the **#498 AIF scale-up** pilots, which produced the few filled cells that exist.

→ **Recommendation**: re-scope #141 from "enrich empty descriptions" (done) to "**populate the AIF cross-reference graph**", via gpt-5.5 candidate generation + an expert validation gate (AIF/Walton mappings are specialised — not auto-writable).

---

## Census (measured on `ba8e4a6c`, 2026-06-29)

### Non-card node counts

| Dataset | Total rows | Cards | **Non-card nodes** | Card flag |
|---|---|---|---|---|
| Fallacies | 1408 | 176 | **1232** | `carte` ∈ {1, 2} |
| Virtues | 223 | 113 | **110** | `card` = 1 |

Non-card = family / subfamily / subsubfamily headers + order groupings (rows whose card flag is empty). Scenarii and Rules have no family hierarchy nor crossLink/AIF columns → out of #141 scope.

### Text enrichment on non-card nodes (original #141 scope item 3) — ✅ DONE

| Dataset | desc/description ×8 langs | example/remark ×8 langs |
|---|---|---|
| Fallacies | **100 %** (1232/1232) | 97–100 % (min 1192/1232 `example_fr`) |
| Virtues | **100 %** (110/110) | **100 %** (110/110) |

Titles, descriptions, examples and remarks are fully populated across fr/en/ru/pt/es/ar/zh/fa. **There is no text-enrichment gap to script.**

### Cross-references (`crossLink_*` + `AIF_skos*`) — the REAL open gap

Format observed: `crossLink_*` holds **`decimal_path` references** to other nodes (e.g. `5.1.2.2.2`); `AIF_skos*` holds **AIF-scheme refs + SKOS mapping types** (e.g. `GeneralAcceptanceDoubt_Conflict`, `skos:broadMatch`).

| Dataset | `crossLink_*` (8 cols) | `AIF_skos*` (4 cols) | Notes |
|---|---|---|---|
| Fallacies non-card | **15 / 9856 (0.2 %)** | 26 / 4928 (0.5 %) | AIF barely started on non-cards |
| Virtues non-card | 110 / 880 (12.5 %) | 220 / 440 (50 %) | Partial via #498 pilots |

**Virtues detail** (asymmetry from #498): `crossLink_Opposes` 100 %, `AIF_skosDirectRef` 100 %, `AIF_skosMappingType` 100 % — but the **7 other `crossLink_*` relationship types are 0 %**. So even where #498 ran, only a slice of the relationship graph is populated.

**Fallacies detail**: `crossLink_PredatesOn` 8, `Leverages`/`Opposes` 2, `IsRelatedTo`/`Allows`/`Denounces` 1, `Inverts`/`Mirrors` 0 — effectively unstarted on non-cards.

**Aggregate**: ~16700 empty cross-reference cells across the two datasets where the schema is ready and waiting.

---

## Why this is a proposal, not an auto-write

1. **Pre-tag freeze** — `Cards/` is frozen (release coupled to DNN). Any cross-reference write is post-release.
2. **AIF/Walton is specialised** — the 8 `crossLink_*` verbs (`Inverts`, `Mirrors`, `Allows`, `Denounces`, `IsRelatedTo`, `Leverages`, `Opposes`, `PredatesOn`) and the AIF scheme refs encode an argumentation-theory judgement. An LLM can propose candidates but **must not be ratified as authoritative without expert/native review** (cf. project memory: *"Anti-Fab Validator: Check 'Walton scheme' = WARN"* — LLM-fabricated AIF mappings are a known failure mode). This mirrors the #192 native-ratification discipline.
3. **Schema decisions still open** — should `crossLink_*` hold one `decimal_path`, a list, or a structured `{target, note}`? The few filled cells use bare `decimal_path`, but a richer encoding may serve #136 (2sxc entities) better.

---

## Proposed enrichment method (gpt-5.5 assist + expert gate)

**Stage 0 — ground truth (done by this census).** Confirm text is complete; isolate the cross-reference gap.

**Stage 1 — gpt-5.5 candidate generation (assist only).** For each non-card node, give the model:
- the node's FR/EN description + example (already 100 % populated — the context),
- the list of sibling/cousin nodes (same parent's children) with their `decimal_path` + label,
- the AIF scheme reference list (external, academic — Walton's argumentation schemes),
- the 8 `crossLink_*` verb definitions,
and ask it to **propose** 0–N candidate relationships as `{verb, target_decimal_path, confidence, one-line rationale}`. Use `/v1/responses` + `reasoning.effort=low` (memory `gpt55-responses-api-effort-low`); `max_output_tokens=7000`.

**Stage 2 — dry-run + diff review (drift-free method #595).** Write candidates to a **sidecar** report (not `Cards/`). Re-runs verified cell-by-cell (no drift).

**Stage 3 — expert/jsboige ratification gate.** A human reviews the candidate pairs per node (like the #192 `RATIFY` checklist). Only ratified pairs are written to `Cards/`, post-release, via the `DatasetUpdater` (point it at the `crossLink_*` fields with a dedicated prompt + JSON schema — the adaptation the issue asked for).

**Stage 4 — OWL / 2sxc export.** Once ratified, the AIF graph flows into #130 (OWL/SKOS `skos:related`, `skos:broadMatch`) and #136 (2sxc entities) — the dependencies #141 declares.

---

## What "adapt the GPT-4 enrichment script" means now

The original ~2-year-old GPT-4 script's **modern counterpart already exists**: [`DatasetUpdater`](../../Generation/Converters/Argumentum.AssetConverter/DatasetUpdater) (8 C# files, 35+ prompts, OpenAI SDK v2.10.0, function-calling via manual `FunctionToolDef` + JSON schema, gpt-5.5 primary). The adaptation the issue asks for is **not** a rewrite — it is:

1. **Add a `crossLink` prompt** (new `PromptCrossLink*User/Assistant/System` in `DatasetUpdater/Resources/`) that frames the relationship-proposal task above.
2. **Add a `crossLink` task config** in `DatasetUpdaterRootConfig.cs` (7 task configs exist; all `Enabled = false` — same gated pattern).
3. **Point it at the `crossLink_*` + `AIF_skos*` columns** with the Stage 1–2 assist-then-gate flow.

This is **post-release work** (touches `Cards/` content + the AssetConverter project → pre-tag regression risk). The deliverable *now* is this proposal + the census script; the prompt/config adaptation is gated on (a) jsboige GO and (b) the schema decision (§"Why this is a proposal").

---

## Dependencies & overlaps

- **#498 (AIF scale-up)** — phases 1–3 + generative pilot (branches `feat/498-aif-scaleup-phase{1,2,3}`, `docs/498-aif-closure`) produced the few filled AIF/crossLink cells. #141 cross-reference work should **resume from #498's results**, not restart.
- **#130 (OWL ontology)** — AIF graph → SKOS semantic relations. Dependency declared by #141.
- **#136 (2sxc taxonomy entities)** — AIF graph → website entity relationships. Dependency declared by #141.
- **#192** — i18n harmonisation (closed for the ratifiable subset; native-ratification residue tracked separately). Orthogonal: #192 is *labels*, #141 is *relationships*.

---

## Scope of THIS PR

- ✅ `docs/taxonomy/141-noncard-census.py` — read-only, reproducible census (0 write).
- ✅ `docs/taxonomy/141-noncard-enrichment-census.md` — this proposal.
- ✅ **0 write under `Cards/`**, **0 AssetConverter code change** (pre-tag safe).
- ✅ Base `ba8e4a6c`.

Relates to #130, #136, #498. Reframes #141: the text-enrichment scope is complete; the cross-reference graph is the open work, gated post-release on jsboige GO + schema decision.
