# #141 — AIF Cross-Reference Stage-1 Candidate Pilot (gpt-5.5, dry-run)

**Author**: po-2024 (worker) · **Date**: 2026-07-01 · **Base**: master `18b4d023`
**Status**: **DRY-RUN PILOT / CANDIDATES** — gpt-5.5-proposed relationships for expert ratification. **NOT auto-written.**
**Scope**: docs + read-only scripts. **0 write under `Cards/`** (pre-tag freeze).
**Reproducibility**: [`141-aif-stage1.py`](141-aif-stage1.py) — `python docs/taxonomy/141-aif-stage1.py [N]`.

This is the **PRIMAIRE** of ai-01 deep-queue supersede (`msg-…370u0q`), unblocked once a gpt-5.5 key was available. It implements **Stage 1** of the method proposed in [`141-noncard-enrichment-census.md`](141-noncard-enrichment-census.md): gpt-5.5 *candidate generation* (assist only) behind an expert-ratification gate.

---

## TL;DR

Ran gpt-5.5 (`/v1/responses`, `reasoning.effort=low`) over a **28-node stratified pilot** spanning all 7 fallacy families, in **closed-set selection** mode: the model *picks* cross-link targets from an enumerated list of real nodes and AIF scheme tokens from the observed Walton vocabulary — it cannot invent a path or a scheme name.

| Metric | Value |
|---|---|
| Nodes processed | **28 / 28** (0 errors) |
| crossLink candidates | **93** |
| AIF scheme refs | **55** (38 DirectRef + 17 ExceptionRef) |
| Mean confidence | **0.75** (30 high ≥0.8 · 55 mid · 8 low) |
| Fabricated AIF tokens | **0** |
| Invalid cross-link targets | **0** |
| Invalid verbs | **0** |
| Anti-Fab WARNs | **1 kind** — `skos:relatedMatch` (legit SKOS, outside observed set) |

**The closed-set design fully contained the fabrication risk** the Anti-Fab Validator warns about ("Walton scheme = WARN"): every AIF token is in the 60-token observed vocabulary; every target resolves to a real taxonomy node.

---

## Method

### Schema grounding (code = truth)

Measured on `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` (1408 rows, 1232 non-card nodes):

- **`decimal_path`** uses a **comma** form `family,rest` where `rest` is 1 digit per level (`1,1111` = depth 5 = dotted `1.1.1.1.1`). `parent_of` drops the last digit of `rest`.
- **`crossLink_*`** (8 verbs) hold **dotted** decimal_paths (`7.1.2.3`). All 22 already-filled targets verified to exist.
- **`AIF_skos*`** hold **Walton AIF scheme** tokens with a `_Conflict`/`_Inference`/`_Scheme` suffix (`Ignorance_Inference`), plus a `skos:` mapping type.
- **60 distinct AIF tokens** observed in the live taxonomy (the closed vocabulary).

### Closed-set candidate generation

For each source node, gpt-5.5 receives:
- the node's FR/EN label + description + example (100 % populated — context),
- a **candidate target list** (siblings + parent + children + the 7 family roots — all REAL nodes, dotted paths verbatim),
- the **8 verb definitions**,
- the **60-token AIF vocabulary** + the 3 observed mapping types,
and returns strict JSON. It can only *select* — never *generate* — a target path or scheme token. This is the direct mitigation of the Walton-fabrication failure mode.

### Anti-Fab validation (post-generation)

Each candidate is checked: target ∈ taxonomy index · verb ∈ 8 · AIF token ∈ vocab · mappingType ∈ observed set. Violations are flagged `WARN` in the sidecar — none ratified.

---

## Results

### crossLink verb distribution (n=93)

| Verb | Count | % |
|---|---:|---:|
| IsRelatedTo | 43 | 46.2 % |
| Leverages | 32 | 34.4 % |
| Allows | 9 | 9.7 % |
| Mirrors | 7 | 7.5 % |
| Inverts | 2 | 2.2 % |
| **Opposes / Denounces / PredatesOn** | **0** | 0 % |

### AIF scheme tokens (top, n=55)

`Dialogue_Scheme` ×6 · `Bias_Inference` ×5 · `EvidenceToHypothesis_Inference` ×4 · `LackOfCompleteKnowledge_Conflict`/`InductiveInference_Scheme`/`OtherCausalFactorsInvolved_Conflict`/`VagueVerbalClassification_Inference`/`VerbalClassification_Inference` ×3. Mapping types: `skos:broadMatch` ×39, `skos:closeMatch` ×12, `skos:relatedMatch` ×4 (flagged).

### Quality spot-checks (defensible)

- *Appel à l'ignorance* `IsRelatedTo` *Preuve par absence de réfutation* (0.9) — genuine directional variant. `Mirrors` *Argument d'incrédulité* (0.55). DirectRef = `Ignorance_Inference` (correct Walton scheme).
- *Ignorance délibérée* `Leverages` *Appel à l'ignorance* (parent), `IsRelatedTo` *A priorisme* (sibling, 0.86) — structurally sound.

---

## Honest limitations (for the expert gate)

1. **Strong verbs under-explored.** The model never proposed `Opposes`/`Denounces`/`PredatesOn` — yet `PredatesOn` is the *most-filled* verb in the existing oracle (it links influence fallacies to the cognitive-bias nodes deep in family 6). The conservative posture is safe but means the richest existing relationship type is not reproduced by this pass. A follow-up could seed the prompt with the `PredatesOn`-to-bias pattern.
2. **Intra-family bias (84/93 = 90 %).** The candidate pool is sibling-heavy, so cross-family links (common in the oracle, e.g. `1,11 → 7.1.2.3`) are under-produced. A richer cross-family candidate set would surface them.
3. **Non-determinism.** `temperature=1.0`; re-runs vary (a smoke-test node proposed `skos:exactMatch`, the full run proposed `skos:relatedMatch`). Determinism is not required for *candidates* (a human ratifies), but reproducibility needs a seed/fixed snapshot — hence the committed sidecar.
4. **`skos:relatedMatch` / `exactMatch`** appear though absent from the observed oracle set ({broad,close,narrow}Match). Both are legitimate SKOS predicates — a schema-extension decision for the expert gate, **not** fabrications.

---

## Scope of THIS PR

- ✅ `docs/taxonomy/141-aif-explore.py` — read-only Stage-0 hierarchy + sample design (0 write).
- ✅ `docs/taxonomy/141-aif-stage1.py` — read-only Stage-1 generator (0 write; key read from scratchpad, never committed).
- ✅ `docs/taxonomy/141-aif-candidates-sample.csv` — the candidate sidecar (93 + 55 rows).
- ✅ `docs/taxonomy/141-aif-stage1-report.md` — this report.
- ✅ **0 write under `Cards/`**, **0 AssetConverter code change** (pre-tag safe).
- ✅ Base `18b4d023`. **No secret committed** (key stays in session scratchpad).

## Next (gated post-release)

- **Stage 2** — scale to all 1232 non-card nodes (batched, cost-bounded) once the candidate quality is ratified on this pilot.
- **Stage 3 — expert/jsboige ratification gate** (cf. #192): a human reviews candidate pairs per node; only ratified pairs are written to `Cards/`, post-release, via the `DatasetUpdater` pointed at `crossLink_*`/`AIF_skos*` (the prompt + config adaptation #141 asks for).
- **Stage 4** — ratified AIF graph flows into #130 (OWL/SKOS) and #136 (2sxc).

Relates to #141, #130, #136, #498, #192. Implements Stage 1 of the [`141-noncard-enrichment-census.md`](141-noncard-enrichment-census.md) method. Memory honored: `gpt55-responses-api-effort-low` (Responses API + effort=low), Anti-Fab "Walton scheme = WARN" (closed-set containment).
