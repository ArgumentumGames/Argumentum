# AIF structural audit — method & governance record

> **Provenance.** This document records a design conversation between **jsboige**
> (author of the fallacy taxonomy, 5+ years of incremental work) and the **ai-01
> coordinator**, 2026-07-22, on how the AIF/Walton mechanistic lens (chantier
> [#498](https://github.com/ArgumentumGames/Argumentum/issues/498)) should relate
> to the *authored structure* of the fallacy taxonomy. It is committed to the repo
> deliberately so the reasoning survives context/dashboard condensation. It is a
> **method + decision record**, not ratified policy — wording is open to jsboige's
> revision. Tracking issue: see the "Actionable" section.

---

## 1. Why this exists

The AIF Layer-C chantier (#498) annotates each fallacy with an **attack-type**
(`undercut` → RA-node, `undermine` → I-node, `rebut` → CA-node — see
`docs/ontology/aif-export/README.md`). Cluster [#845](https://github.com/ArgumentumGames/Argumentum/pull/845)
was the **first** to cross from *serializing* the taxonomy (annotating what is
there) to *qualifying* it (finding that a node's grouping does not hold under the
mechanistic lens — 6 leaves flagged as out-of-mechanism, **flagged not
force-fitted**).

That inflection raised jsboige's real question: **are we entering a phase where
the detail of the authored taxonomy becomes criticisable, and structural
adjustments get proposed?** The answer is *yes, partly* — and this document
frames how to do it **fairly to the tree**, without letting an automated lens
flatten five years of deliberate nuance.

## 2. Two axes of arbitrariness (jsboige's framing)

The taxonomy carries **two distinct kinds of arbitrariness**, and only one is
revisable:

| Axis | What | Status |
|------|------|--------|
| **A — the groupings** | The tree structure jsboige built: consolidating clusters, moving grappes, integrating large blocks (codex of cognitive biases, the mental-manipulation / psychological-games grappe). Built with, in his own words, *"as much rigour as I could, i.e. relatively little if I'm honest."* One explicit criterion, kept since the card game: **balancing the tree** — necessarily arbitrary, essentially pedagogical. | **Revisable** |
| **B — the base material** | The fallacies *themselves* are heterogeneous objects. Some carry Latin names and 2000 years of scholastic history (*petitio principii*); others are decades-old coinages for modern phenomena (*whataboutisme*, ~30 yrs). The tree is simultaneously **a classification of argument structures AND a museum of the history of how humans named their bad arguments.** No protocol makes these commensurable. | **Irreducible** |

**The AIF attack-type is a lens orthogonal to axis A.** A leaf can be correctly
placed (pedagogically) under a node *and* have a different attack-mechanism than
its siblings. **"Misgrouped under the AIF lens" ≠ "misplaced in the taxonomy."**

## 3. Preliminary read (ai-01 impression — to be replaced by measurement)

Not a verdict, an **impression** grounded in artifacts, explicitly awaiting the
audit below:

- **The tree is not decorative. Intuitions sound; changes at the margin.**
- **Strongest evidence:** it took **7 Layer-C clusters** before the first
  mechanism-heterogeneity surfaced (#845). Had the tree been arbitrary,
  heterogeneity would appear at cluster 1–2 (attack-type distributed at random
  under the nodes). The first six came out mechanism-homogeneous — even where a
  *scheme* token was missing (fail-loud), the *attack-type* stayed consistent —
  although jsboige did **not** build them on the AIF axis. A pedagogical tree that
  turns out *also* mechanism-coherent means intuition supplied the formal rigour
  that was not consciously applied.
  *(Qualified: the "7 clusters / first heterogeneous" framing is RAPPORTÉ by the
  chantier and consistent with the merge sequence; the audit turns it into a number.)*
- **Where it will bite:** jsboige's **tree-balancing criterion** — the one
  criterion genuinely orthogonal to mechanism. Wherever a grappe was split or
  moved to balance branch sizes (card-game constraint), AIF may want to regroup or
  re-split. The *"painful arbitrations"* concentrate **there**, plus on **hub
  nodes** (e.g. *homme de paille*, central from several angles — also a hub in the
  #838 84-pair near-dup set). Not everywhere — on those specific points.

## 4. The instrument — two-layer homogeneity audit

The point of the audit is to **replace impression with a per-node number**, and to
be **fair to the tree** by separating tree-tension from material-resistance.

For each parent node, compute two independent layers:

- **(a) Attack-type heterogeneity** — distribution of `undercut`/`undermine`/`rebut`
  across the node's leaves (a homogeneity ratio or Shannon entropy). **High (a) =
  potential tree tension** (the node mixes attack-mechanisms).
- **(b) Fail-loud rate** — fraction of the node's leaves with **no native AIF
  scheme token** (the material resisted mechanistic modelling). This is an
  *imperfect proxy* for **material resistance** (axis B intruding). The chantier
  already records this per leaf (`native token` vs `fail-loud`, e.g. #839).

**Decision rule:**

| (a) attack-type heterogeneity | (b) fail-loud rate | Reading | Action |
|---|---|---|---|
| high | low | Genuine **tree tension** — the grouping mixes mechanisms and the material is tractable | Arbitrage worth it (jsboige) |
| any | high | **Material resistance** — axis B, the atoms are just refractory here | Leave the pedagogy alone; annotate the seam, don't regroup |
| low | low | Node is mechanism-coherent | No action — evidence the grouping tracks mechanism |

This makes the audit **only bill jsboige for what is genuinely his** (grouping
choices), never for the irreducible messiness of the source material.

**Live specimen already on the table:** *tu quoque* (ancient, Latin) and
*whataboutisme* (modern) — the **same pragmatic move at two eras** — surfaced as
the `rebut`/CA vs `undermine`/I modelling tension in #845. The ancient/modern seam
(axis B) reads *directly* in a modelling tension, which is exactly why layer (b) is
needed to avoid misreading it as an axis-A defect.

## 5. Governance principle (decision record)

1. **AIF may *qualify* (critique) the structure — but findings are FLAGS, never
   auto-reorganisations.** Workers surface "mechanism-heterogeneity here"; jsboige
   alone decides whether it means anything.
2. **All findings gated on jsboige ratification. 0 prod-CSV write** until ratified
   (post-tag regime, #499 staging method — accumulate in `docs/taxonomy/`, batch
   ratify, then apply).
3. **The pedagogical-balance criterion is legitimate**, orthogonal to mechanism.
   Heterogeneity under AIF is **not automatically a defect**.
4. **Preserve rich taxonomy nuance** (jsboige's standing value): where
   heterogeneity is material-driven (layer b high), **keep the pedagogical grouping
   and annotate the seam** — do not flatten a multi-axis tree onto one axis.

## 6. Status & actionable

- **The instrument is half-built — it *is* the AIF chantier (#498).** Current
  coverage: 145 fully-modeled fallacies + ~7 Layer-C clusters. The full-tree
  per-node number waits for Layer-C coverage to advance.
- **Down-payment ("acompte") available now:** a preliminary homogeneity read on
  the already-annotated material (145 fully-modeled + the merged clusters) — a
  first per-family homogeneity rate + the visible heterogeneous nodes — enough for
  jsboige to sanity-check the "changes at the margin" impression on data.
- Read-only, staged, feeds the post-tag batch ratification. Companion to #497
  (crosslinks), #498 (AIF modelling), #790 (Layer-C pilot design).

**Tracking issue:** the two-layer audit + acompte are tracked as a dedicated
GitHub issue (extends #498). See #458 (coordination roadmap) for the pointer.
