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

## 2. Three axes of arbitrariness (jsboige's framing)

The taxonomy carries **three distinct kinds of arbitrariness**, and only one is
freely revisable:

| Axis | What | Status |
|------|------|--------|
| **A — the groupings** | The tree structure jsboige built: consolidating clusters, moving grappes, integrating large blocks (codex of cognitive biases, the mental-manipulation / psychological-games grappe). Built with, in his own words, *"as much rigour as I could, i.e. relatively little if I'm honest."* One explicit criterion, kept since the card game: **balancing the tree** — necessarily arbitrary, essentially pedagogical. | **Revisable** |
| **B — the base material** | The fallacies *themselves* are heterogeneous objects. Some carry Latin names and 2000 years of scholastic history (*petitio principii*); others are decades-old coinages for modern phenomena (*whataboutisme*, ~30 yrs). The tree is simultaneously **a classification of argument structures AND a museum of the history of how humans named their bad arguments.** No protocol makes these commensurable. | **Irreducible** |
| **C — the cross-linguistic tradition** | The taxonomy is authored primarily in **French**, with **English the richest** reference source; the other six languages are unevenly populated. But the divergence runs deeper than coverage: **traditions consecrated different visions across cultures, so the branches are not always aligned** — a grouping natural in one language has no clean counterpart in another. The tree is therefore a **French-anchored *projection* of a family of partially-divergent language-specific trees**, not a culture-neutral structure. | **Partly irreducible** (tradition-bound; but *which projection to privilege* is a real editorial arbitration) |

**The AIF attack-type is a lens orthogonal to axes A *and* C** — it names a
*pragmatic move*, invariant both to how a node is grouped and to which language
named it. A leaf can be correctly placed (pedagogically) under a node *and* have a
different attack-mechanism than its siblings: **"misgrouped under the AIF lens" ≠
"misplaced in the taxonomy."** But that same invariance makes AIF a
**cross-linguistic solvent**: two non-aligned consecrated labels (FR vs EN) can be
tested for *"same fallacy?"* by comparing attack-type + scheme — so the lens does
not merely *audit* axis A, it can **diagnose** axis C.

### 2.1 The internal-node labelling constraint (straddles A and C)

A self-imposed rule with a hard external teeth: **internal (grouping) nodes must be
labelled with a consecrated term that carries an external reference** — one may not
coin a neologism for a node just to make the tree balance. Leaves are specific
named fallacies; internal nodes must borrow an *existing* term. Two consequences,
both arbitrations jsboige has already had to make:

- **Distorted labels** — the nearest consecrated term's scope doesn't exactly match
  the intended cluster → the term is **stretched or narrowed**, giving the node a
  slightly shifted meaning to align the schema.
- **Grouping gated by lexical availability** — where no consecrated term exists for
  a natural grouping, either the grouping isn't made, or a term is **borrowed from
  the one language/tradition that has it**, importing that tradition's framing.

This mostly lives on **axis A** (a structural/pedagogical rule), but it is
**genuinely arbitrary** (the term must exist and be referenced) and it **surfaces
axis C**: whether a consecrated label is even *available* is language-dependent, so
the tree's shape is partly dictated by which culture happened to name a given
meta-concept. **Refinement for the audit:** a distorted/compromise-labelled node
that is *mechanism-homogeneous* is **vindicated** (the stretch was sound); one that
is *mechanism-heterogeneous* is a candidate where the labelling constraint may have
**forced a bad merge** — but check axis C first (below) before billing it as a
grouping defect.

### 2.2 The evidential base is uneven and decaying

External anchors split by stability: **Wikipedia links are stable; fallacy-dictionary
links are decaying** (several already dead, repointed to archive.org). The
Wikipedia-anchored nodes are the trustworthy skeleton; dictionary-only nodes are at
evidential risk. The audit should therefore carry **reference reachability** as
per-node metadata — a fail-loud leaf whose only anchor is a dead dictionary link is
a *different* diagnosis than one with a live Wikipedia anchor.

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

For each parent node, compute three independent layers:

- **(a) Attack-type heterogeneity** — distribution of `undercut`/`undermine`/`rebut`
  across the node's leaves (a homogeneity ratio or Shannon entropy). **High (a) =
  potential tree tension** (the node mixes attack-mechanisms).
- **(b) Fail-loud rate** — fraction of the node's leaves with **no native AIF
  scheme token** (the material resisted mechanistic modelling). This is an
  *imperfect proxy* for **material resistance** (axis B intruding). The chantier
  already records this per leaf (`native token` vs `fail-loud`, e.g. #839).
- **(c) Cross-linguistic divergence** — do the node's leaves' cross-language
  reference sets and labels **align**, or does the node's membership/shape shift
  with the source language? Plus a flag: is the node's own label a
  *distorted/compromise* consecrated term (§2.1)? **High (c) = the heterogeneity may
  be a tradition-divergence artifact** (axis C), not a grouping defect. Weakest of
  the three proxies today — reference-set overlap + label-distortion flag are the
  first-pass signals; refine with human read.

**Decision rule:**

| (a) heterogeneity | (b) fail-loud | (c) ling. divergence | Reading | Action |
|---|---|---|---|---|
| high | low | low | Genuine **tree tension** — grouping mixes mechanisms, material tractable, traditions aligned | Arbitrage worth it (jsboige) |
| high | low | **high** | **Tradition divergence** (axis C) — a cross-cultural **bridge node** whose heterogeneity does legitimate work | Annotate as a bridge; **don't flatten** — the misalignment is the tradition's, not jsboige's |
| any | high | any | **Material resistance** (axis B) — the atoms are refractory here | Leave the pedagogy alone; annotate the seam, don't regroup |
| low | low | any | Node is mechanism-coherent | No action — the grouping tracks mechanism |

This makes the audit **only bill jsboige for what is genuinely his** (grouping
choices), never for the irreducible messiness of the source material (axis B) or
the divergence of traditions the tree must bridge (axis C).

**Live specimen already on the table:** *tu quoque* (ancient, Latin, scholastic) and
*whataboutisme* (modern, anglophone, Cold-War) — the **same pragmatic move at two
eras *and* two traditions** — surfaced as the `rebut`/CA vs `undermine`/I modelling
tension in #845. It is a **joint axis-B *and* axis-C seam**: the ancient/modern
divide (B) and the Latin-scholastic / Cold-War-anglophone divide (C) both read
*directly* in a modelling tension — exactly why layers (b) and (c) are needed to
avoid misreading it as an axis-A grouping defect.

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
  jsboige to sanity-check the "changes at the margin" impression on data. Layers (a)
  and (b) are computable today; **layer (c)** needs two cheap additions to the
  chantier: **reference-reachability metadata** (Wikipedia-stable vs
  dictionary/archive.org) and a **label-distortion flag** on internal nodes.
- Read-only, staged, feeds the post-tag batch ratification. Companion to #497
  (crosslinks), #498 (AIF modelling), #790 (Layer-C pilot design).

**Tracking issue:** the three-layer audit + acompte are tracked as a dedicated
GitHub issue (extends #498). See #458 (coordination roadmap) for the pointer.
