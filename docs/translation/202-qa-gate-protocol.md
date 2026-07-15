# #202 — Post-Release EN Campaign — QA-Gate Protocol (per-tranche spot-check)

**Status:** PROTOCOL — docs-only companion to [202-en-campaign-plan.md](202-en-campaign-plan.md).
**Author:** po-2024 (dispatch `mf4nyb` [SECONDAIRE]).
**Audience:** ai-01 (QA verdict holder), jsboige (register decision).

> Defines the **byte-exact spot-check criteria** ai-01 applies per tranche before a bulk GO. The campaign (#809 plan) is **deferred post-v0.9.0**; this protocol prepares the gate so execution is turnkey once the register decision (A/B, jsboige) lands.

---

## 0. Why a gate, and who owns it

The campaign fills ~2725 empty cells via gpt-5.5 (`/v1/responses`, `reasoning.effort=low`, empty-only). gpt-5.5 can produce fluent-but-wrong output (false meaning, hallucinated proper nouns, register drift). A **pilot tranche** (20 cells) is QA'd before each bulk run; bulk is gated on pilot PASS. **Verdict QA = ai-01** (worker never self-approves bulk). This protocol pins *what* ai-01 checks so the bar is uniform across tranches.

---

## 1. Tranches + gate flow

| Tranche | Field | Pilot cells | Bulk cells | Gate owner | Blocks |
|---|---|---:|---:|---|---|
| T1 | `Simple_name_en` | 20 | 1327 | ai-01 spot-check | T2 |
| T2 | `Simple_name_en` | — | 1327 | (post-T1 GO) | regen |
| T3 | `political_example_en` | 20 | 1353 | ai-01 spot-check **+ register A/B ratified** | T4 |
| T4 | `political_example_en` | — | 1353 | (post-T3 GO) | regen |
| T5 | Virtues hierarchy residual | ~5 (all) | — | trivial | — |

**Flow:** T1 pilot (20) → ai-01 QA → [GO/NO-GO] → T2 bulk → regen Memo/Fallacies Face. Then T3 (requires register decision) → T4. T5 one-shot.

---

## 2. Spot-check checklist (per pilot, 20 cells)

For each pilot tranche, ai-01 runs these 6 checks on the **20 pilot cells** (worker delivers the 20-cell diff + a per-cell table):

### Check 1 — Empty-only invariant (byte-exact, HARD)
- **What:** every cell written was empty before; no filled cell changed.
- **Method:** `git diff` shows additions only in the target column; the 61 (Simple_name_en) / 35 (political_example_en) pre-filled cells are **byte-identical** before/after.
- **Pass:** 0 clobber. **Fail (auto-NO-GO):** any pre-filled cell altered.

### Check 2 — Meaning fidelity vs `text_fr`/`desc_fr` (semantic, per-cell)
- **What:** the EN output preserves the fallacy's *meaning*, not a transliteration.
- **Method:** ai-01 samples **≥5 cells** across families, compares EN output to `text_fr` + `desc_fr`. Flag any output that contradicts `desc_fr` or invents a different concept.
- **Pass:** 0 meaning contradiction in sample. **Fail:** ≥1 contradiction → investigate prompt, do not bulk.

### Check 3 — Register / style (per-cell, field-specific)
- **`Simple_name_en`:** ≤5 words, plain-English common name, card-facing. If `text_en` (formal) is already a good common name and was reused → OK. Flag over-long or jargon names.
- **`political_example_en`:** 1-3 sentences, **neutral + non-defamatory + factual**, clearly an *example* not an endorsement. **Register must match the ratified A/B decision** (A internationally-recognizable / B FR-context verbatim). Flag partisan/defamatory/endorsement tone.
- **Pass:** register uniform + matches A/B. **Fail:** register drift or defamatory content.

### Check 4 — MT-garbage / grammar sweep (mechanical, all 20)
- **What:** no fluent-but-broken English (the #803/#805 class: mistranslated headings, grammar tells, over-literal proper nouns).
- **Method:** the worker runs the 3-dimension sweep (flag tokens + FR-mistranslated headings + MT grammar patterns, count old→new per token, memo `mt-garbage-sweep-false-zero`) on the 20 outputs.
- **Pass:** 0 MT-garbage. **Fail:** ≥1 → investigate before bulk.

### Check 5 — Language purity (byte-exact, all 20)
- **What:** the EN cell contains English (no FR/RU/... leakage). Complement to #795 (which proved prose clean at the dataset level — here we re-check the *new* gpt-5.5 outputs).
- **Pass:** 0 leakage. **Fail:** ≥1 leakage → prompt leak, do not bulk.

### Check 6 — Encoding / round-trip (byte-exact, all 20)
- **What:** CRLF + BOM preserved, UTF-8 no-BOM on write, CsvHelper round-trip intact (no quoting drift — memo `csv-byte-exact-column-insertion`).
- **Method:** `git diff` shows no spurious whole-file re-encoding; only the target column changed.
- **Pass:** byte-exact outside the 20 target cells. **Fail:** encoding/quoting drift → re-run with field-segment splitter, not csv round-trip.

---

## 3. Pass/Fail thresholds

| Outcome | Condition | Action |
|---|---|---|
| **GO (bulk)** | Checks 1,5,6 byte-exact clean AND Checks 2,3,4 have 0 hard fails in the 20-cell pilot | Worker proceeds to bulk (T2/T4) |
| **REVISE** | 1-2 cells fail Checks 2/3/4 (meaning/register/MT) | Worker fixes prompt + re-runs the failing pilot cells; re-QA |
| **NO-GO** | Check 1 (clobber) OR Check 5/6 (leakage/encoding) fails, OR ≥3 cells fail meaning/register | Halt tranche; escalate to ai-01 + jsboige |

**Hard gates (auto-NO-GO, non-negotiable):** Check 1 clobber, Check 6 encoding drift. These are data-integrity, not quality.

---

## 4. Post-bulk regression gate (after T2/T4)

After each bulk run + card regen (Memo + Fallacies Face):
1. **Re-run #795 multilingual drift audit** — confirm 0 regression on the *other* 7 languages × all fields (the new EN writes must not have touched RU/PT/ES/AR/FA/ZH/FR).
2. **Byte-check ALL language columns** (memo `byte-check-multilang` — the zh regression #761 lesson: don't byte-check FR only).
3. **Rendered-source sanity** — confirm the regen consumed the updated CSV (memo `regen-success-without-clobber-is-stale-trap`: clobber harvests before regen; freshness proof = Chromium invoked, not count-identical).

---

## 5. Deliverables the worker provides per pilot

- The 20-cell `git diff` (target column only).
- A per-cell table: `pk | text_fr | text_en (formal) | <new EN output> | check-result`.
- The Check 4 sweep output (3-dim, per-token counts).
- A byte-exact confirmation (Check 1/5/6) for the pre-filled cells.

ai-01 then renders a verdict (GO/REVISE/NO-GO) on the tranche. Bulk never starts without an explicit GO.

---

## 6. Scope notes

- **#202 execution stays deferred post-v0.9.0** (jsboige arbitration). This protocol is turnkey prep.
- **T3/T4 blocked** on the register A/B decision (jsboige) — protocol applies once ratified.
- **T5 (Virtues hierarchy, ~5 cells)** is trivial: visual spot-check only, no pilot/bulk split.
- Out of scope: `link_*` (#192, separate), #804 Phase 4 regen (separate), #415 (INTERDICTED).

— po-2024 (dispatch `mf4nyb`, docs-only)
