# #141 AIF Stage-3 — expert adjudication package (DRY-RUN, no auto-apply)

**Author**: po-2024 (worker) · **Date**: 2026-07-01 · **Base**: master `33b1c0bc`
**Source of truth**: [`141-aif-stage2-diff.csv`](141-aif-stage2-diff.csv) (16 rows, reproducible via
[`141-aif-stage2-diff.py`](141-aif-stage2-diff.py)) · derived from the
[full-scale Stage-1 sidecar](141-aif-candidates-fullscale.csv) (#626, merged).
**Status**: **ADVISORY PACKAGE FOR THE EXPERT GATE** — recommendations only. **Nothing here is
auto-applied.** Every row marked ⚖️ is a human (jsboige / argumentation expert) call.

This is the one-pass adjudication table for the **12 non-card nodes** that already carry an AIF
value on the taxonomy (0.97 % of 1232). For those 12, the full-scale generator (gpt-5.5,
closed-set) was compared against the existing expert annotation. The other **1220 nodes are entirely
net-new** — no existing value to conflict with, so they are out of this adjudication (they go to the
general Stage-3 ratification, prioritized by confidence > 0.8).

## Verdict summary

| Verdict | Rows | Nodes | Action |
|---|---:|---:|---|
| CONFIRM | 2 | 2 | gpt-5.5 re-derived the **exact** expert token — **ratify, no change** |
| CONFLICT | 9 | 6 | gpt-5.5 proposed a different token/field — **adjudicate below** |
| SILENT | 5 | 5 | gpt-5.5 stayed silent on a filled field — **preserve existing** |

## CONFIRM — ratify as-is (2)

| dp | label | field | token (existing = proposed) |
|---|---|---|---|
| `1.111` | Appel à l'ignorance | DirectRef | `Ignorance_Inference` |
| `5.321` | Expression vague | DirectRef | `VagueVerbalClassification_Inference` |

Both are independent corroboration: the closed-set generator reproduced the expert's exact Walton
token with no prompt priming. Highest-confidence signal in the whole run.

## CONFLICT — adjudicate (9 rows, 6 nodes)

Each node below: existing annotation vs gpt-5.5 proposal, an **advisory recommendation** (⚖️ expert
decides), and a one-line Walton rationale. A cross-cutting convention underpins several recos
(see [Convention](#convention)).

### `4.116` Pente glissante — field-swap + token churn (4 rows)

| field | existing | gpt-5.5 proposed |
|---|---|---|
| DirectRef | `RequiredSteps_Conflict` | `CausalSlipperySlope_Inference; NegativeConsequences_Inference` |
| ExceptionRef | `CausalSlipperySlope_Inference` | `RequiredSteps_Conflict` |
| ExceptionRef | `FullSlipperySlope_Inference` | `RequiredSteps_Conflict` |
| ExceptionRef | `PrecedentSlipperySlope_Inference` | `RequiredSteps_Conflict` |

⚖️ **Reco: MERGE — adopt the field-swap, preserve the dropped tokens.**
- **Adopt**: `_Inference` schemes as **DirectRef** (primary reasoning scheme), `RequiredSteps_Conflict`
  as the conflict/structure side — gpt-5.5's field allocation aligns with Walton's scheme-vs-conflict
  distinction (see [Convention](#convention)).
- **Preserve** (gpt-5.5 dropped these, but they are legitimate Walton slippery-slope subtypes):
  `FullSlipperySlope_Inference`, `PrecedentSlipperySlope_Inference` → keep in ExceptionRef.
- **Adopt the addition**: `NegativeConsequences_Inference` (valid subtype gpt-5.5 surfaced).
- Net = **union of both sets under the swapped field allocation**, not swap-and-drop.

### `6.31123` Apophénie (1 row)

| field | existing | gpt-5.5 proposed |
|---|---|---|
| ExceptionRef | `Sign_Inference` | `OtherCausalFactorsInvolved_Conflict; SignFromOtherEvents_Conflict` |

⚖️ **Reco: PROMOTE `Sign_Inference` to DirectRef; absorb the proposed `_Conflict` tokens as
ExceptionRef critical questions.** Apophénie's core is a *sign* mistaken for a real relationship —
`Sign_Inference` is the primary scheme, arguably mis-located in ExceptionRef. The proposed
`OtherCausalFactorsInvolved` / `SignFromOtherEvents` are well-formed critical questions (alternative
causes / alternative signs) and enrich the exception structure. Expert: confirm the field promotion.

### `6.311234` Preuve anecdotique (1 row)

| field | existing | gpt-5.5 proposed |
|---|---|---|
| ExceptionRef | `InductiveInference_Scheme` | `WeakestLink_Conflict` |

⚖️ **Reco: KEEP `InductiveInference_Scheme` (canonical).** Anecdotal evidence = hasty induction
from a single case — `InductiveInference_Scheme` is the textbook Walton fit. `WeakestLink_Conflict`
is a critical question about the weakest link in a chain; it can be added as a *companion*
critical question but does **not** replace the scheme. gpt-5.5 is over-conservative here (dropping
the scheme). Expert: keep scheme, optionally add `WeakestLink_Conflict` as a critical question.

### `6.312` Biais émotionnels (1 row)

| field | existing | gpt-5.5 proposed |
|---|---|---|
| DirectRef | `OppositeConsequences_Conflict` | `Bias_Inference` |

⚖️ **Reco: ADOPT `Bias_Inference` as DirectRef; demote `OppositeConsequences_Conflict` to
ExceptionRef.** The fallacy *is* a bias — `Bias_Inference` (gpt-5.5) is the stronger primary scheme.
`OppositeConsequences_Conflict` is a critical question (consequences going the other way) that fits
the exception structure, not the primary scheme. gpt-5.5 is right; existing DirectRef is mis-tiered.

### `6.321` Biais d'attribution (1 row)

| field | existing | gpt-5.5 proposed |
|---|---|---|
| ExceptionRef | `Example_Inference` | `OtherCausalFactorsInvolved_Conflict` |

⚖️ **Reco: ADOPT proposed; flag both as weak DirectRef fits.** Attribution bias = misattributing a
cause. `Example_Inference` (existing) = generalizing from an example — weak fit (attribution isn't
example-based). `OtherCausalFactorsInvolved_Conflict` (gpt-5.5) better captures the error
(*ignoring alternative causes*) but is a `_Conflict` token (critical question, not a scheme).
**Expert**: check whether a cleaner `Causal_Inference` / `CauseToEffect_Inference` scheme exists in
the Walton vocabulary for the primary DirectRef; absent that, adopt gpt-5.5's token as the exception
structure and keep the field flagged.

### `6.322` Essentialisme (1 row)

| field | existing | gpt-5.5 proposed |
|---|---|---|
| ExceptionRef | `Preference_Scheme` | `BiasedClassification_Conflict` |

⚖️ **Reco: ADOPT proposed; existing looks mislabeled.** Essentialism = treating categories as
innate/fixed. `Preference_Scheme` (existing) is an odd fit (preference ≠ essence).
`BiasedClassification_Conflict` (gpt-5.5) captures the classification-bias core, but is a `_Conflict`
token. **Expert**: check for a `Classification_Inference` / `VerbalClassification_Inference` scheme
for a cleaner DirectRef; the existing `Preference_Scheme` appears mislabeled and should not be
preserved as-is.

## SILENT — preserve existing (5)

gpt-5.5 did **not** re-propose a token for these already-filled fields. The correct action is to
**preserve the existing expert value** — silence is not a signal to drop.

| dp | label | field | existing (preserve) |
|---|---|---|---|
| `5.3212` | Pente glissante sémantique | ExceptionRef | `VerbalSlipperySlope_Inference` |
| `6.31121` | Effet de simple exposition | ExceptionRef | `Example_Inference` |
| `6.312` | Biais émotionnels | ExceptionRef | `Preference_Scheme` |
| `6.31212` | Biais d'autocomplaisance | ExceptionRef | `PositiveConsequences_Inference` |
| `6.312311` | Justification de l'effort | ExceptionRef | `Waste_Inference` |

> Note: `6.312` Biais émotionnels appears in both CONFLICT (DirectRef) and SILENT (ExceptionRef
> `Preference_Scheme`). The DirectRef conflict is adjudicated above; its ExceptionRef
> `Preference_Scheme` is preserved here (gpt-5.5 was silent on that field).

## Convention

The recurring thread across the 9 CONFLICTs is a **scheme-vs-conflict tier distinction** in Walton's
AIF:

- **`*_Inference` / `*_Scheme` tokens** name a *reasoning scheme* (the pattern of inference) →
  natural **DirectRef** (primary scheme the fallacy instantiates).
- **`*_Conflict` tokens** name a *conflict/decision structure* or a *critical question* → natural
  **ExceptionRef** (the exception/critical-question side).

gpt-5.5's closed-set picks respect this distinction more consistently than some existing annotations
(which occasionally put `_Conflict` tokens in DirectRef or `_Scheme` tokens in ExceptionRef). Four of
the six conflict nodes (`4.116`, `6.31123`, `6.312`, `6.322`) improve under the convention; two
(`6.311234`, `6.321`) are weak on both sides and need a cleaner scheme token. **Adopting the
convention is itself a gate decision** — it is surfaced here, not imposed.

## What this package does NOT do

- **No auto-apply.** Every ⚖️ row awaits the expert gate. No `Cards/` write, no DatasetUpdater run.
- **No verdict on the 1220 net-new nodes.** Those go to general Stage-3 ratification, prioritized by
  confidence > 0.8 (the high-signal subset, cf. the [full-scale report](141-aif-fullscale-report.md)).
- **No schema-extension decision** (the 87 `bad_map:*` WARNs — `relatedMatch`/`exactMatch`/`none`/
  `noMatch`) — that is a separate gate call, already surfaced in the full-scale report.

## Reproducibility

The data in this package is regenerated deterministically from
[`141-aif-stage2-diff.py`](141-aif-stage2-diff.py) (reads the taxonomy + the Stage-1 sidecar). The
recommendations are curated prose over that data; re-running the diff after any taxonomy change will
refresh the table, at which point the recos should be re-checked.

Relates to #141, #609, #620, #626, #130, #136, #192. Honors pre-tag freeze (0 `Cards/` write, 0
AssetConverter code change). Memory honored: Anti-Fab "Walton scheme = WARN" — this package treats
every Walton-scheme mismatch as adjudication, never silent overwrite.
