# #202 — EN Post-Release Translation Campaign — Plan (planning-only, 0 CSV write)

**Status:** PLAN — deferred post-v0.9.0 (arbitrage jsboige). No CSV write in this doc.
**Author:** po-2024 (dispatch `y20p0t`, idle GO).
**Base:** master `cf8cb0d8` (2026-07-15).
**Empirical inventory:** computed live from the prod CSVs at `cf8cb0d8`.

> Companion: this scopes the *remaining* EN translation margin after the core-translation campaign (PRs #210/#218/#236/#246/#290/#295, #795 drift audit). It is **not** a re-translation of already-translated content — `#795` proved prose is 100% clean. The margin is in two under-filled **Fallacies** fields. All other datasets are at/near 100%.

---

## 0. Why this is a campaign (and what is NOT in it)

| What | Status | In this campaign? |
|---|---|---|
| Core prose (text/desc/example) EN/RU/PT/ES/AR/FA/ZH | 100% clean (#795) | ❌ done |
| Virtues i18n (title/desc/remark + hierarchy) | 100% core, 86-96% hierarchy | ❌ done |
| Scenarii EN (`suggestion_en`) | 167/167 (100%) | ❌ done |
| Rules EN (`Text_en`) | 15/15 (100%) | ❌ done |
| **Fallacies `Simple_name_en`** | **61/1408 (4%)** | ✅ **scope** |
| **Fallacies `political_example_en`** | **35/1408 (2%)** | ✅ **scope** |
| Fallacies `link_en` | 1333/1408 (94%) | ❌ → #192 (Wikipedia-API, separate) |

**The only material EN margin is Fallacies `Simple_name_en` + `political_example_en`.** Both are **used in active templates** (`Cards/Fallacies/Argumentum_Fallacies_Face_*.json`, `Cards/Memo/Argumentum_Memo_*.json` — confirmed via grep), so filling them has real rendered-card impact (a localized "simple name" on the Memo + an English political example on Fallacies cards).

---

## 1. Empirical inventory (measured FR-relative, not "empty")

| Dataset / field | Filled | Empty | Margin | Used in templates? |
|---|---:|---:|---:|---|
| Fallacies `Simple_name_en` | 61 | **1347** | 96% | ✅ Memo + Fallacies Face |
| Fallacies `political_example_en` | 35 | **1373** | 98% | ✅ Fallacies Face (political variant) |
| Virtues `subsubfamily_en` | 194 | 29 | 13% | ✅ hierarchy labels (marginal) |
| Virtues `subfamily_en` | 215 | 8 | 4% | ✅ hierarchy labels (marginal) |

**Total EN cells to translate: ~2725** (1347 + 1373 + ~5 Virtues hierarchy residual). The Virtues hierarchy residual is tiny and low-priority — the campaign is essentially the two Fallacies fields.

### Field semantics (what gpt-5.5 is generating)

- **`Simple_name_en`** — a short, plain-English common name for the fallacy (e.g. `text_fr` "Appel à l'autorité" → `Simple_name_en` "Appeal to authority"). Distinct from `text_en` (the formal taxonomy label) — it's the accessible, card-facing name. Must preserve the Fallacy's *meaning*, not transliterate.
- **`political_example_en`** — a real-world political example illustrating the fallacy. The 35 existing ones reference FR political context; the EN ones should use internationally-recognizable examples (or keep the FR-context example translated, per a register decision — see §3).

---

## 2. Approach — gpt-5.5 re-run, verified cell-by-cell (never bulk)

**Hard rules (from standing constraints):**
- Model: **gpt-5.5 only** via `/v1/responses`, `reasoning.effort=low` (memory `gpt55-responses-api-effort-low`). Never a lower tier.
- **Empty-only** (fill only the 1347+1373 empty cells; never overwrite a filled cell — byte-exact, no clobber).
- Key `.keys/openai-key.txt` (gitignored); BaseUrl = OpenAI direct, NOT OpenRouter.
- **Cell-by-cell verified** (re-read after write, byte-check the column).
- CRLF + BOM preserved; UTF-8 no-BOM on write.

### Reuse the existing DatasetUpdater scaffold

The repo already has `DatasetUpdaterRootConfig.cs` with **"Translate Fallacies to English by branch empty-only 0-shot"** (task #4, `Enabled=false`). The campaign **adds two sibling tasks** targeting `Simple_name_en` and `political_example_en` — same `empty-only 0-shot` pattern, new `TargetField` + a dedicated prompt. This keeps the toolchain (CsvHelper round-trip protection, FunctionToolDef structured output, verify-after-write) consistent.

---

## 3. Prompt design (gpt-5.5)

Two prompts, one per field, both following the `CsvPromptSystemEn.txt` style + `PromptCosmeticPolish*` register precedent:

### 3a. `Simple_name_en` prompt
- **Input:** `text_fr`, `text_en` (formal), `desc_fr`.
- **Task:** produce a short (≤ 5 words) plain-English common name for the fallacy.
- **Constraint:** must match the *meaning* of `text_fr`; if `text_en` is already a good common name, reuse it; never invent a name that contradicts `desc_fr`.
- **Function-call schema:** `{ "Simple_name_en": "<string>" }`.

### 3b. `political_example_en` prompt
- **Input:** `text_fr`, `desc_fr`, the existing `example_fr` (if any).
- **Task:** produce a real-world political example (1-3 sentences) illustrating the fallacy for an English-speaking reader.
- **Register decision (needs jsboige):** (A) internationally-recognizable examples (broaden from FR politics) — recommended; (B) translate the FR-context example verbatim.
- **Constraint:** neutral, non-defamatory, factual; clearly an *example* of the pattern, not an endorsement.
- **Function-call schema:** `{ "political_example_en": "<string>" }`.

---

## 4. Tranching + gating

| Tranche | Field | Cells | Est. gpt-5.5 calls | Gate |
|---|---|---:|---:|---|
| T1 | `Simple_name_en` pilot | 20 | 20 | Smoke (ai-01 QA + spot-check register) |
| T2 | `Simple_name_en` bulk | 1327 | 1327 | Post-T1 GO |
| T3 | `political_example_en` pilot | 20 | 20 | Smoke (register decision A/B) |
| T4 | `political_example_en` bulk | 1353 | 1353 | Post-T3 GO |
| T5 | Virtues hierarchy residual | ~5 | ~5 | Trivial |

**Total: ~2725 gpt-5.5 calls.** Each pilot is a 20-cell tranche with human/QA spot-check before the bulk commit. `empty-only` means re-runs are idempotent.

---

## 5. Execution order (post-tag)

1. **jsboige register decision** on `political_example_en` (A international vs B FR-context) — blocks T3/T4 only.
2. T1 pilot → QA → T2 bulk (the higher-value field; card-facing).
3. T3 pilot → QA → T4 bulk.
4. T5 trivial Virtues hierarchy (5 cells, one-shot).
5. Regenerate affected card sets (Memo + Fallacies Face) with the new localized `Simple_name_en`/`political_example_en`.
6. Re-run the `#795` multilingual drift audit to confirm 0 regression on the other fields.

---

## 6. Out of scope (explicit)

- **No bulk re-translation of EN core prose** — #795 proved it's clean; re-running gpt-5.5 on filled cells risks regression for no gain.
- **No `link_*` work** — that's #192 (Wikipedia langlinks API, `#799` Option C), a cross-language *article-resolution* task, not text translation.
- **No #804 Phase 4** (Virtues `links.svg` regen) — code fix merged (#808); regen gated separately (FreeMind box).
- **History rewrite / `.git` shrink (#415)** — INTERDICTED (ai-01 `y20p0t`): force-push on shared repo, jsboige arbitration only.

---

## 7. Gate

This is a **plan doc only** — 0 CSV write, 0 code change. The campaign itself is **deferred post-v0.9.0** (jsboige arbitration). When greenlit, execute §5 in order.

— po-2024 (dispatch `y20p0t`, planning-only GO)
