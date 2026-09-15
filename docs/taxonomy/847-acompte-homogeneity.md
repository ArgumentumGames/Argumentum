# 2026-07-22 — #847 AIF structural-homogeneity acompte (layers a+b) — MEASURE, not verdict

> **⚠ STATUS = MEASURE ONLY, not a verdict.** This document is an **INPUT for ai-01's synthesis** and the
> post-tag batch ratification. It carries **numbers + the mechanical decision-rule tag only** — no
> "reorganise this" wording, no structural proposal. All interpretation/synthesis/verdict is ai-01's;
> all structural change is **gated on jsboige ratification**. **0 prod-CSV write** (post-T&A regime).
> Method: [`aif-structural-audit-method.md`](aif-structural-audit-method.md) (3 axes A/B/C, decision table §4).
> Tracking issue : #847. Chantier : #498. Author : po-2024 (compute). Synthesis/verdict : ai-01.

---

## TL;DR (numbers only)

- **145 fully-modeled** (prod CSV, code=truth) : attack-type `87 undercut / 53 undermine / 5 rebut` ;
  **native-scheme 34.5 %**, fail-loud-scheme **65.5 %** ; overall entropy-norm **0.720**.
- **Projected** (145 + 46 leaf propositions from the 8 merged Layer-C clusters, docs-only) : n=191,
  `107 / 72 / 12` ; native-scheme **41.9 %**, fail-loud **58.1 %** ; entropy-norm **0.788**.
- **Layer (b) dominates.** The base material is materially resistant (65.5 % fail-loud on the scheme layer)
  — most family-level heterogeneity is **axis-B (material)**, not axis-A (grouping). This *quantifies*
  the method-doc impression ("changes at the margin").
- **The chantier measurably converts B → A.** Folding the 8 native-rich Layer-C clusters drops fail-loud
  65.5 % → 58.1 % and flips **Obstruction** and **Erreur mathématique** from "material-resistance (B+A)"
  to "tree-tension candidate (A)". *Validates the chantier's native-rich targeting strategy.*
- **Genuine tree-tension (a-high / b-low) concentrates in** : sub-family **Ad hominem** (b=0.00 → 0.21 projected,
  the #844/#845/#846 cluster), **Mauvaise déduction** (b=0.20 projected, the #843 cluster),
  **Erreur de raisonnement** family (b=0.36 → 0.27). **All tagged c-DEFER** (may be cross-linguistic bridge
  nodes, not grouping defects — see §5).
- **Influence** is the only fully mechanism-coherent family (a-low, mostly undermine).

---

## 1. Operational definitions (what was measured)

Per the method doc §4, two layers are computed per parent node ; layer (c) is **deferred** (not instrumented).

| Layer | Operational definition (code=truth, this acompte) |
|-------|---------------------------------------------------|
| **(a) attack-type heterogeneity** | Shannon entropy of the `undercut`/`undermine`/`rebut` distribution across the node's modeled leaves, **normalised to [0,1]** (÷ log₂ of the number of distinct present types). Computed at 3 granularities : family (d1), sub-family (d2), sub-sub (d3). Also reports the **homogeneity ratio** (largest class / n). **a_high = entropy-norm ≥ 0.50.** |
| **(b) fail-loud rate** | Fraction of the node's modeled leaves with **no native `_Inference` scheme token** in `AIF_skosDirectRef` **OR** `AIF_skosExceptionRef` (prod) / the annotation scheme columns (cluster CSVs). A leaf carrying only a `_Conflict` token (no `_Inference`) counts as fail-loud for (b) — it resisted *scheme* modelling. **b_high = fail-loud ≥ 0.50.** |
| **(c) cross-linguistic divergence** | **DEFER — not instrumented.** Requires reference-reachability metadata (Wikipedia-stable vs dictionary/archive.org) + a label-distortion flag on internal nodes (method doc §2.1/§2.2/§6). Flagged "à instrumenter" ; best-effort only if a signal is obvious. |

**Decision-rule tag** (mechanical, from the method-doc table ; c unknown so a-high/b-low reads as *candidate*) :

| (a) | (b) | tag (this acompte) |
|-----|-----|--------------------|
| low | low | `coherent` — mechanism-coherent (no action) |
| high | low | `A-candidate` — tree-tension **candidate** (c DEFER — may be a bridge node ; **do not bill as defect**) |
| high | high | `B+A` — material-resistance + type-mix (annotate the seam ; c DEFER) |
| low | high | `B` — material-resistance, homogeneous type (leave the pedagogy) |

> **Why (b) reads from ExceptionRef too.** An initial scan of `skosDirectRef` alone reported 92 % fail-loud —
> **wrong** : the tranche-1 serialisation placed the `_Inference` token in `AIF_skosExceptionRef` (43 leaves)
> not `DirectRef` (10). The correct denominator unions both columns → 50/145 native-scheme, 65.5 % fail-loud.
> Recorded here so the trap is not re-stepped.

**Coverage threshold.** Only parent nodes with **≥ 3 modeled leaves** are reported (entropy is meaningless
below 3). Nodes below threshold are omitted from the tables, not tagged.

---

## 2. Headline — 145 fully-modeled vs projected (145 + 8 Layer-C clusters)

| metric | 145-only (prod) | projected (145 + clusters) | delta |
|--------|-----------------|----------------------------|-------|
| n leaves | 145 | 191 | +46 (cluster propositions) |
| undercut / undermine / rebut | 87 / 53 / 5 | 107 / 72 / 12 | rebut +7 (Ad-hominem clusters) |
| native-scheme rate | 34.5 % | 41.9 % | **+7.4 pp** (chantier improves fit) |
| fail-loud-scheme rate | 65.5 % | 58.1 % | **−7.4 pp** |
| overall entropy-norm (a) | 0.720 | 0.788 | +0.068 |

**Reading.** The chantier's 8 clusters are native-rich by design ; folding them in lifts native-scheme
coverage and *lowers* fail-loud — i.e. the projected picture is more tractable than the raw 145. The
entropy rise is modest because the clusters are themselves mechanism-coherent (uniform within cluster) ;
they add leaves without scrambling the global type balance.

---

## 3. Family-level (d1) — 145-only vs projected (the B→A conversion)

| family | 145-only (n / a / b / tag) | projected (n / a / b / tag) | shift |
|--------|---------------------------|----------------------------|-------|
| Abus de langage | 57 / 0.67 / 0.88 / B+A | 57 / 0.67 / 0.88 / B+A | unchanged (no Layer-C cluster here) |
| **Erreur de raisonnement** | 14 / 0.59 / 0.36 / **A-cand** | 26 / 0.89 / 0.27 / **A-cand** | b drops 0.36→0.27 (more tractable) |
| **Erreur mathématique** | 12 / 0.92 / 0.50 / B+A | 23 / 0.76 / 0.48 / **A-cand** | **B+A → A-cand** (accident/cigogne/operation clusters) |
| Influence | 12 / 0.41 / 0.42 / **coherent** | 12 / 0.41 / 0.42 / **coherent** | unchanged (only coherent family) |
| Insuffisance | 16 / 0.95 / 0.50 / B+A | 16 / 0.95 / 0.50 / B+A | unchanged |
| **Obstruction** | 12 / 0.84 / 0.67 / B+A | 35 / 0.90 / 0.49 / **A-cand** | **B+A → A-cand** (Ad-hominem clusters #844/#845/#846 + complication #837) |
| Tricherie | 22 / 0.95 / 0.59 / B+A | 22 / 0.95 / 0.59 / B+A | unchanged |

**Key finding (measure, not verdict).** Two families flip from `B+A` (material-resistance) to `A-candidate`
(tree-tension) once the chantier's native-rich clusters are folded in : **Obstruction** (b 0.67→0.49) and
**Erreur mathématique** (b 0.50→0.48). This *quantifies that the chantier's strategy works* — targeting
native-rich clusters makes previously-refractory nodes tractable for the (a) grouping review. Conversely,
**Abus de langage / Insuffisance / Tricherie** stay B+A : no Layer-C clusters there yet, material still
resists. **Influence** is the sole fully-coherent family (a-low).

---

## 4. A-candidate sub-families (a-high / b-low — genuine tree-tension, material tractable)

These are the nodes where the decision-rule says *"arbitrage worth it"* **pending the (c) check**. Listed
as candidates only — **none is billed as a defect** (c deferred).

| sub-family | 145-only (n / a / b) | projected (n / a / b) | note |
|------------|----------------------|-----------------------|------|
| **Ad hominem** | 4 / 0.81 / **0.00** | 14 / 0.86 / 0.21 | cleanest signal ; the #844/#845/#846 cluster. Mixes `undermine` (direct) + `rebut` (tu quoque) — the method-doc §4 "live specimen" (tu quoque/whataboutisme), now a number. b=0.00 → 0.21 (still low). |
| Mauvaise déduction | (below th. / b=0.50) | 10 / 0.88 / **0.20** | appears as A-cand only projected ; the #843 Inconsistance cluster (6 undermine) + accident #841. |
| Argument bâclé | 7 / 0.86 / 0.43 | 7 / 0.86 / 0.43 | stable. |
| Mauvaise interprétation | 4 / 0.81 / 0.25 | 4 / 0.81 / 0.25 | stable. |
| Pensée biaisée | 13 / 0.89 / 0.46 | 13 / 0.89 / 0.46 | stable. |
| Causalité douteuse | 5 / 0.72 / 0.20 | (5 / 0.72 / 0.20) | stable (145-only list ; projected unchanged). |

**Ad hominem** is the headline candidate : highest heterogeneity with the lowest fail-loud (all native-scheme).
Its tension is the `rebut`/CA (tu quoque, anchor 1361) vs `undermine`/I (direct ad hominem, anchor 1398)
split — exactly the modelling tension flagged in #845 and ratified as a deferred decision. The (c) check
matters most here : tu quoque (Latin, scholastic) vs whataboutisme (Cold-War anglophone) is a **joint
axis-B + axis-C seam** — if (c) reads high, this node is a *bridge*, not a defect.

---

## 5. Layer (c) — DEFER, and what instrumenting it needs

(c) is **not computed** this acompte. To instrument it (method doc §6) :

1. **Reference-reachability metadata** per leaf : Wikipedia-stable (trustworthy skeleton) vs
   fallacy-dictionary (decaying, several dead → archive.org). A fail-loud leaf whose only anchor is a
   dead dictionary link is a *different diagnosis* than one with a live Wikipedia anchor.
2. **Label-distortion flag** on internal (grouping) nodes : is the node's consecrated term stretched/narrowed
   (§2.1) ? A distorted node that is mechanism-homogeneous is *vindicated* ; one that is mechanism-heterogeneous
   is a candidate where the labelling constraint may have forced a bad merge — **but check (c) first**.

Both are cheap additions to the chantier's per-leaf record ; deferred until the chantier's coverage advances
or jsboige asks for the full audit. Until then, **every `A-candidate` tag carries "c DEFER — may be a bridge
node"** — none is actionable as a grouping defect without the (c) read.

---

## 6. What this acompte is NOT (governance)

- **Not a verdict.** No node is labelled "to reorganise". Tags are mechanical outputs of the (a)+(b) rule.
- **Not a proposal.** No structural change is suggested. Synthesis + verdict + any jsboige presentation = ai-01.
- **Not prod.** 0 prod-CSV write. The two CSVs (`847-acompte.csv`, `847-acompte-projected.csv`) are read-only
  artefacts in `docs/taxonomy/`.
- **Not the full tree.** Coverage = 145 fully-modeled (+ 46 cluster propositions). The whole-tree per-node
  number waits for Layer-C coverage to advance (method doc §6).
- **(c) is absent.** Any `A-candidate` could still be a tradition-bridge (axis C), not a grouping defect.
  The measure is deliberately blind to that axis until instrumented.

---

## Artefacts

- **`docs/taxonomy/847-acompte.csv`** — per-node (a)+(b)+tag, **145 fully-modeled** (prod CSV, code=truth).
  42 rows (family + sub-family + sub-sub, n≥3).
- **`docs/taxonomy/847-acompte-projected.csv`** — same, **projected = 145 + 8 Layer-C cluster leaf
  propositions** (docs-only, illustrative). 51 rows.
- Compute script (reproducible) : `scratchpad/compute-847-acompte-v2.py` (not committed — scratch).

Relates : #847 (tracking), #498 (chantier), [`aif-structural-audit-method.md`](aif-structural-audit-method.md)
(method + decision table), #845 (first qualifying cluster), #846 (cross-cluster straw-man corroboration),
#770 (anchor audit), #763 (OWL AIF wiring), #677 (0 fabrication). Base master `2becf12a`.
