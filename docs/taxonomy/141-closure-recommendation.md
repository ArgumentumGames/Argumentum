# #141 AIF cross-reference — closure recommendation

**Author**: po-2024 (worker) · **Date**: 2026-07-01 · **Base**: master `33b1c0bc`
**Status**: **RECOMMENDATION — close #141 after the expert gate (Stage-3)**. This is the synthesis
of Phases 1–3 of the AIF cross-reference work. It does **not** write anything; it states what is
delivered, what the gate owes, and the close criteria.

## What #141 set out to do

Map the Fallacies non-card taxonomy (1232 nodes) to (a) each other via cross-link verbs and (b) the
Walton AIF scheme set, using an LLM (gpt-5.5) under a **closed-set anti-fabrication design** so the
model can only pick from real nodes / real Walton tokens — it cannot invent scheme names or targets.
Output is a dry-run sidecar for expert ratification; nothing is auto-written to the taxonomy.

## Phase trace — all delivered

| Phase | Deliverable | PR | Status |
|---|---|---|---|
| Census | non-card gap + AIF cross-ref proposal | #609 | ✅ merged |
| Pilot (Stage-1, 28 nodes) | closed-set, 0 fabrication validated | #620 | ✅ merged |
| Full-scale generator (1232) | checkpoint/resume, anti-fab | #623 | ✅ merged |
| Full-scale results | 1232/1232, 0 fab, sidecar + report | #626 | ✅ merged |
| Stage-2 diff | vs existing AIF (12 nodes, 9/2/5) | #626 | ✅ merged |
| **Stage-3 adjudication** | expert gate package (this cycle) | _this PR_ | ⚖️ pending expert |

## Headline result — the anti-fab design scales

Across **1232 nodes**, the closed-set design produced **0 fabricated scheme names, 0 invented
decimal_paths, 0 out-of-vocabulary verbs**. The 87 WARNs are all `bad_map:*` — legitimate SKOS
predicates (`relatedMatch`/`exactMatch`/`none`/`noMatch`) outside the *observed* on-disk set; a
schema-extension decision for the gate, **not** fabrications. This was the core hypothesis of #141
and it held at full scale (cf. [full-scale report](141-aif-fullscale-report.md)).

## What the expert gate (Stage-3) owes

Two gated decisions block the final write and the closure:

1. **Adjudicate the 12 existing-AIF nodes** — the [Stage-3 package](141-aif-stage3-adjudication.md)
   surfaces 9 CONFLICTs + 5 SILENTs with advisory recommendations. The expert (jsboige) decides:
   keep existing / adopt proposed / field-swap / merge, per node. ~16 token-level decisions.
2. **Decide the schema extension** — accept `relatedMatch`/`exactMatch` (87 WARNs) into the observed
   set, or down-grade them to `closeMatch`; drop the `none`/`noMatch` hedges.
3. **Ratify the net-new high-confidence subset** — of the 1220 net-new nodes, prioritize confidence
   > 0.8 (the high-signal cluster) for the ratified write; the mid-tail (0.5–0.8) is genuine-but-
   generic and can defer.

Once ratified, the write itself is mechanical — via the `DatasetUpdater` prompt+config adaptation
(#141 original scope), drift-free (#595), skip-non-empty (won't clobber the 12 expert values).

## Close criteria for #141

**Recommend #141 be closed as *delivered, gated*** once the expert gate has:

- [ ] adjudicated the 12 existing-AIF nodes (Stage-3 package),
- [ ] decided the `relatedMatch`/`exactMatch` schema extension (87 WARNs),
- [ ] ratified (or deferred) the net-new high-confidence subset.

The generation + validation machinery (#623) and the reviewable sidecar (#626) are done and merged;
the remaining work is **judgment, not engineering**, and is owned by the expert gate, not by #141's
build lane. After the gate, #141 hands off to #130 (OWL export) / #136 (2sxc export) for the
ratified graph.

## What is explicitly NOT in scope for closure

- **No auto-write** of any candidate (anti-fab "Walton scheme = WARN" — every scheme match is
  adjudicated, never silently applied).
- **No claim of completeness** — the full-scale run is non-deterministic (`temperature=1.0`); the
  committed sidecar is the reproducible snapshot. Strong verbs (`Denounces`=1) remain sparse (cf.
  full-scale report limitations).
- **No Cards/ write, no AssetConverter change** — pre-tag freeze honored throughout.

Relates to #141, #609, #620, #623, #626, #130, #136, #192.
