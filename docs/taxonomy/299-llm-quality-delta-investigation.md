# #299 LLM translation quality — delta investigation (read-only, 0 run live)

> **Provenance.** Investigation dispatched by ai-01 (`msg-gvpl31`, tick 88,
> SECONDARY). **Read-only — 0 run live** (`DatasetUpdater.Enabled=false` on all
> 48 task configs, T&A freeze). This doc does NOT redo the #299 benchmark; it
> characterizes the **delta** between the closed #299 benchmark (2026-05-17) and
> the current state (gpt-5.5 pivot + the now-plumbed-but-dormant
> `/v1/responses` path from #852). **INPUT for the post-T&A activation
> decision. GATED jsboige ratification. 0 prod-CSV write.**

---

## 1. What #299 already settled (closed, do not redo)

#299 is **CLOSED** with a complete provider benchmark by jsboige
(2026-05-17, 3 Scenarii records FR→PT, fixed PT prompt):

| Provider | Model | baratineur→PT | Time | Verdict |
|----------|-------|---------------|------|---------|
| OpenAI | gpt-4.1-mini | "O convencedor" | 13s | solid baseline, reliable |
| OpenRouter | claude-sonnet-4 | "O persuasor" | 7s | **best quality, fastest** |
| Myia | qwen3.6-35b-a3b | — | **120s TIMEOUT** | unusable for batch |
| ZAI | glm-5.1 | "O persuasor" | 69s | good but **truncated** |

**jsboige's recommendation (2026-05-17):** claude-sonnet-4 primary (best +
fastest), gpt-4.1-mini fallback, skip Myia (reasoning overhead), ZAI cautious.

**Crucially, jsboige already measured the reasoning-overhead failure mode** —
the comment is explicit:

> *"Reasoning model uses tokens for thinking before generating content. Even
> with 4000 max_tokens, the reasoning consumed all tokens before producing
> output."* (Myia qwen) — and *"Reasoning consumed 3751 of 4000 tokens,
> truncating output"* (ZAI glm-5.1).

This is the **same root cause** later formalized for gpt-5.5 in memory
`[[gpt55-responses-api-effort-low]]`: reasoning models on Chat Completions burn
the token budget on hidden reasoning and return empty/truncated Content.
jsboige observed it empirically 2 months before the gpt-5.5 pivot.

## 2. What changed since (the delta)

The #299 benchmark is **stale relative to two subsequent changes**:

### 2.1 The gpt-5.5 pivot

The benchmark tested gpt-4.1-mini (and claude-sonnet-4/glm-5.1/qwen). Today
(`DatasetUpdaterRootConfig.cs`, master `f70b20dc`):

| Model configured | n tasks |
|------------------|---------|
| **gpt-5.5** | **44** |
| gpt-5.4 | 4 |
| **Total** | **48** |

**Neither benchmark winner is configured:** gpt-4.1-mini = 0 tasks,
claude-sonnet-4 (OpenRouter) = 0 tasks. The fleet pivoted to gpt-5.5 — a
**reasoning model**, precisely the class jsboige flagged as over-budget on Chat
Completions in the #299 comment.

### 2.2 The `/v1/responses` plumbing landed but is DORMANT

PR #852 (merged `f70b20dc`) added the Responses-API path (`UseResponsesApi` +
`ReasoningEffort` config props, `SendViaResponses` in `Prompt.cs`) alongside the
legacy `CompleteChatAsync` — **reversible, default off**. Empirical config scan:

- `UseResponsesApi` set on task configs: **0 / 48**
- `ReasoningEffort` set on task configs: **0 / 48**
- `UseFunctionCalling = true`: 47 / 48 (function-calling is the dominant mode)

**Consequence:** all 44 gpt-5.5 tasks are wired to the **legacy Chat
Completions** path. If `Enabled` were flipped to `true` today, gpt-5.5 would run
on Chat Completions → reasoning consumes the budget → **empty/truncated Content**
(the exact failure mode `Prompt.Send` returns `""` at `:132`, and the exact
root cause jsboige measured on glm-5.1/qwen). The plumbing fix exists but is
not activated on any task.

## 3. The current decision space (input for ai-01 / jsboige post-T&A)

The post-T&A activation now has **three** coherent options (the #299 benchmark
could not consider option C — `/v1/responses` did not exist then):

| Option | Path | Model | Status | Trade-off |
|--------|------|-------|--------|-----------|
| **A** | Chat Completions | gpt-5.5 (current config) | ❌ would empty-Content | reasoning burns budget — **don't activate as-is** |
| **B** | Chat Completions | revert to gpt-4.1-mini / claude-sonnet-4 (#299 winners) | not configured | proven by #299, but discards gpt-5.5 reasoning value |
| **C** | `/v1/responses` + `reasoning.effort=low` (#852, OFF today) | gpt-5.5 | plumbing ready, 0 tasks opted in | **untested** — keeps gpt-5.5 but caps reasoning spend; the intended config per dashboard metric |

**Recommendation (for ai-01 synthesis, not a verdict):** Option C is the
intended config (it is literally what the dashboard API metric records:
`gpt-5.5 /v1/responses, reasoning:{effort:"low"}, max_output_tokens=7000`).
Before any post-T&A campaign, flip `UseResponsesApi=true` + `ReasoningEffort="low"`
on the gpt-5.5 task configs and run a **5-record pilot** (not a full Phase 1) to
confirm Option C returns usable Content — the #299 benchmark could not test it.

## 4. What this investigation did NOT do (governance)

- **0 run live.** All 48 task configs are `Enabled=false` (T&A freeze). No API
  call was made. This is a config + code + doc read.
- **0 prod-CSV write.**
- **Did not redo the #299 benchmark** (it is closed and valid for its era).
- **Did not change any config.** `UseResponsesApi`/`ReasoningEffort` stay at 0 —
  activation is a post-T&A decision (gated jsboige), not a worker action.
- **Did not reopen #299.** The benchmark stands; this doc characterizes the
  delta, it does not dispute the closed verdict.

## 5. Caveats

1. **Option C is untested empirically.** The dashboard metric states it as the
   intended config, but no run has confirmed gpt-5.5-via-Responses returns
   non-empty Content at `effort:low`. The 5-record pilot (§3) is the
   verification — deferred to post-T&A, gated jsboige.
2. The benchmark's qualitative PT finding ("persuasor" vs "convencedor" for
   *baratineur*) may still favour claude-sonnet-4 on idiomatic grounds — Option C
   (gpt-5.5) vs Option B (claude-sonnet-4) is a genuine quality-vs-cost
   arbitration the pilot should inform.
3. `gpt-5.4` (4 tasks) is not a reasoning model in the same way as gpt-5.5; its
   behaviour on Chat Completions may differ. Out of scope here — flagged only.

## Refs

- #299 (closed benchmark, 2026-05-17). #852 (`/v1/responses` plumbing, merged
  `f70b20dc`). Memory `[[gpt55-responses-api-effort-low]]`. Dispatch ai-01 →
  po-2024 `msg-gvpl31` (tick 88, SECONDARY).
