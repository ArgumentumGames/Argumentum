# #141 — CrossLink DatasetUpdater adaptation (additive proposal, gated)

**Author**: po-2024 (worker) · **Date**: 2026-07-04 · **Base**: master `a41cbda6`
**Dispatch**: `l0wt63` (ai-01), secondaire — "#141 taxonomy non-card node iteration; adaptation du script d'enrichment; Proposition additive gated (même méthode que #497-499). DoD : proposal doc + échantillon, PR."
**Status**: **PROPOSAL + SAMPLE. Gated post-tag.** 0 write under `Cards/`, 0 AssetConverter C# change (the prompt/config are staged *as text* here; materializing them in code is the gated post-tag step).

---

## TL;DR

#141's original "adapt the GPT-4 enrichment script" is **already adapted** — the modern counterpart is `DatasetUpdater` (OpenAI SDK v2.10.0, gpt-5.5, function-calling). The text-enrichment scope is **DONE** (100% × 8 langs). The AIF cross-reference graph is **delivered through Stage-3** (1232/1232, 0 fabrication) and awaits expert adjudication. The single remaining engineering artifact is the **`crossLink` DatasetUpdater task** — a prompt + config that lets the expert-gate output flow into `Cards/` drift-free. **This doc stages that artifact as a sample, gated post-tag.**

This is additive to the [census](141-noncard-enrichment-census.md) + [closure recommendation](141-closure-recommendation.md); it does not re-scope them.

## Current state (code=truth, `a41cbda6`)

| #141 scope item | Status | Evidence |
|---|---|---|
| Text enrichment (desc/example × 8 langs, non-card) | ✅ DONE | census: 100% Fallacies + Virtues |
| "Adapt the GPT-4 script" | ✅ DONE | `DatasetUpdater` (99 prompts, 7+ task configs, SDK v2.10.0) |
| AIF cross-ref generation | ✅ DELIVERED | #626: 1232/1232, 0 fab, sidecar committed |
| AIF cross-ref expert adjudication | ⚖️ PENDING | [Stage-3 package](141-aif-stage3-adjudication.md) — expert gate, not build lane |
| **`crossLink` DatasetUpdater task (this proposal)** | 📝 STAGED | prompt + config skeleton below, gated post-tag |

The [closure recommendation](141-closure-recommendation.md) is explicit: remaining #141 work is "judgment, not engineering, owned by the expert gate." This proposal stages the **one engineering artifact** that follows the gate, so the handoff is mechanical once jsboige adjudicates.

## The proposed artifact — `crossLink` DatasetUpdater task

### Task config skeleton (mirrors the 7 existing `Enabled=false` configs)

```csharp
new DatasetUpdaterConfig()
{
    Enabled = false,   // GATED — post-tag + post-Stage-3-adjudication only
    Name = "Populate CrossLink + AIF graph (ratified subset)",
    SourceDataset = KnownDataSets.FallaciesTaxonomy,
    FieldsToInclude = new List<string>()
    {
        "decimal_path", "title_fr", "title_en",
        "description_fr", "description_en",
        // the 8 crossLink_* + 4 AIF_skos* target columns:
        "crossLink_PredatesOn", "crossLink_Denounces", "crossLink_Leverages",
        "crossLink_Allows", "crossLink_Opposes", "crossLink_Inverts",
        "crossLink_Mirrors", "crossLink_IsRelatedTo",
        "AIF_skosDirectRef", "AIF_skosExceptionRef", "AIF_skosOther", "AIF_skosMappingType"
    },
    FieldsToUpdate = new List<string>()
    {
        "crossLink_PredatesOn", /* ...the 8 verbs... */ "crossLink_IsRelatedTo",
        "AIF_skosDirectRef", /* ...the 4 AIF cols... */ "AIF_skosMappingType"
    },
    PrimaryField = "decimal_path",
    TargetPath = @".\Target\Datasets\Argumentum Fallacies - Taxonomy.csv",
    SystemPromptPath = PromptsRootPath + "PromptCrossLinkSystem.txt",
    DialogPrompts = new List<PromptExample>()
    {
        new PromptExample()
        {
            UserPromptPath = PromptsRootPath + "PromptCrossLinkSampleUser.json",
            AssistantAnswerPath = PromptsRootPath + "PromptCrossLinkSampleAssistant.json"
        }
    },
    Model = "gpt-5.5",
    MaxTokensPerMinute = 70000,
    DivisionMode = DivisionMode.SequentialChunks,
    ChunkSize = 1,                 // one node at a time (relationship judgement)
    UseFunctionCalling = true,     // closed-set: function schema enumerates real verbs + real decimal_paths
    SkipChunkNb = 0,
    TakeChunkNb = -1,
    SkipNonEmpty = true            // CRITICAL: won't clobber the 12 expert-adjudicated existing-AIF nodes
}
```

**Key design points:**
- **`UseFunctionCalling = true`** — the closed-set anti-fabrication design validated at full scale (#626): the function schema enumerates the 8 real `crossLink_*` verbs + the real `decimal_path` set, so the model can only pick real targets (0 fabrication, 0 invented paths — the #626 result).
- **`SkipNonEmpty = true`** — preserves the 12 existing-AIF nodes (Stage-3 adjudicated values); only fills empty cells.
- **`ChunkSize = 1`** — relationship judgement is per-node (siblings/cousins context), not batch-translatable.
- **`Enabled = false`** — same gated pattern as the 7 existing configs. Dead code until jsboige GO + Stage-3 ratified.

### Prompt design (`PromptCrossLinkSystem.txt`, sample framing)

```
You map a Fallacies taxonomy node to its relatives via argumentation-theory verbs.

For the node {{decimal_path}} ({{title_fr}} / {{title_en}}):
- description: {{description_fr}}
- siblings/cousins available as targets: {{sibling_decimal_paths_with_labels}}

Pick 0-N relationships from the CLOSED SET:
  verbs = [PredatesOn, Denounces, Leverages, Allows, Opposes, Inverts, Mirrors, IsRelatedTo]
  targets = the sibling decimal_paths above (NO other paths — inventing a path is forbidden)

For each pick, return {verb, target_decimal_path, confidence ∈ [0,1], one-line rationale}.
Only return relationships you can justify from the descriptions. "No relationship" is a valid answer.
DO NOT invent decimal_paths. DO NOT use verbs outside the closed set.
```

This is the **same closed-set anti-fab pattern** that delivered 0 fabrication across 1232 nodes (#626) — repurposed for the assisted-propose phase of the expert gate.

## Échantillon (sample — ratified-output shape)

Drawn from the committed sidecar [`141-aif-candidates-sample.csv`](141-aif-candidates-sample.csv) (#626), this is what the expert gate ratifies and the `crossLink` task would then write:

| source_dp | source_label | verb | target | target_label | conf | rationale |
|---|---|---|---|---|---|---|
| 1,111 | Appel à l'ignorance | IsRelatedTo | 1.1.1.1.4 | Preuve par absence de réfutation | 0.95 | forme particulièrement nette de l'appel à l'ignorance |
| 1,111 | Appel à l'ignorance | IsRelatedTo | 1.1.1.1.3 | Argument d'incrédulité | 0.72 | conclusion indue d'une limite cognitive |
| 1,111 | Appel à l'ignorance | Allows | 1.1.1.1.1 | Ignorance délibérée | 0.55 | ouvre la voie quand l'absence de preuve est entretenue |

The high-confidence row (0.95) is the ratifiable cluster; the mid-tail (0.55) is genuine-but-generic and defers per the [closure recommendation](141-closure-recommendation.md) §"ratify the net-new high-confidence subset".

## Staged flow (gated, post-tag)

1. **Stage-3 expert adjudication** (jsboige, pending) — adjudicate the 12 existing-AIF nodes + decide the `relatedMatch`/`exactMatch` schema extension (87 WARNs) + ratify the high-confidence subset.
2. **Materialize this proposal** (post-tag, post-ratification) — add the 3 prompt files (`PromptCrossLinkSystem.txt` + sample User/Assistant) to `DatasetUpdater/Resources/`, add the task config block above to `DatasetUpdaterRootConfig.cs`.
3. **Dry-run** (Enabled=true, small TakeChunkNb) on the ratified subset → diff review (drift-free method #595).
4. **Write** — Enable, run, write the ratified `crossLink_*`/`AIF_skos*` cells to `Cards/` (SkipNonEmpty preserves experts).
5. **Export** — graph flows to #130 (OWL/SKOS) + #136 (2sxc entities).

## Why a proposal now, not the code

- **Pre-tag freeze** — the census + closure-rec both flag `Cards/` + AssetConverter changes as post-release. Materializing the C# now is pre-tag regression risk for zero release benefit.
- **Expert gate precedes it** — Stage-3 adjudication owns the content decisions; the `crossLink` task only mechanizes the ratified write. Building it before the gate is built-out-of-order.
- **Same method as #497-499** — those scale-ups shipped proposal docs + pilots first; the prod-write was a separate gated step. This mirrors that.

## What this PR does NOT do

- ❌ No `Cards/` write. ❌ No AssetConverter C# change (prompt/config are *text in this doc*, not materialized files).
- ❌ No gpt-5.5 call (the generation machinery #623 is done; this stages the write-path, not the generation).
- ❌ No auto-ratification (every crossLink/AIF value is expert-gated — anti-fab "Walton scheme = WARN").

## Reproducibility

Sample drawn from the committed sidecar `docs/taxonomy/141-aif-candidates-sample.csv` (#626). Task-config skeleton mirrors `DatasetUpdaterRootConfig.cs:23-72` (the Virtues config block). Read-only; 0 write.

Relates #141, #609 (census), #620 (pilot), #623 (generator), #626 (fullscale + sidecar), #130 (OWL), #136 (2sxc), #498 (AIF scale-up), #595 (drift-free method). Base `a41cbda6`.
