# #847 — AIF structural homogeneity audit: **synthesis + verdict** (ai-01)

> **Status: VERDICT.** This is the synthesis the three measurement installments deferred to ai-01.
> Inputs (all merged, read-only, `MEASURE not verdict`): [#850](847-acompte-homogeneity.md) layers (a)+(b),
> [#853](847-layerc-reference-reachability.md) layer (c) reference-reachability,
> [#857](847-layerc-label-distortion.md) layer (c) label-distortion.
> Method + decision table: [`aif-structural-audit-method.md`](aif-structural-audit-method.md).
> **Any structural change remains gated on jsboige ratification. 0 prod-CSV write.**
> Author: ai-01 (synthesis/verdict lane). Compute: po-2024. Chantier: #498.

---

## 0. The question, and the answer

jsboige's question (framing of #847, after cluster #845 became the first to *qualify* the taxonomy rather
than force-fit it): **is the authored taxonomy structure now criticisable, and do we propose structural
adjustments?**

**Verdict: the tree survives the audit. No restructuring is proposed.**

Of **39 grouping nodes** measured on the 145 fully-modeled leaves:

| Reading | Nodes | What it means |
|---------|-------|---------------|
| mechanism-**coherent** | 4 | no action |
| **bridge node** (tradition divergence) | 1 | *Ad hominem* — annotate, **do not flatten** |
| **material resistance** (B / B+A) | 28 | the *material* resisted mechanistic modelling — leave the pedagogy, annotate the seam |
| **axis-A candidates** (genuine tree tension) | 6 | jsboige's to arbitrate — **but see §3: only 2 survive a confidence gate** |

The method doc's prior impression — *"changes at the margin"* — is **confirmed and now quantified**. The
dominant term is **layer (b)**: **65.5 %** of the 145 leaves carry no native AIF `_Inference` scheme token.
Most heterogeneity in this taxonomy is **material resistance**, not authorial grouping error. That is a
finding *in the tree's favour*, and it is the honest headline.

---

## 1. What layer (c) actually settled — and what it did not

Layer (c) was the fairness instrument: it existed so the audit could distinguish a **grouping defect
(axis A, jsboige's)** from a **cross-linguistic tradition artifact (axis C, nobody's fault)**. It has two
halves, and they came back with **asymmetric strength**. This asymmetry is the most consequential thing in
the three reports, and it must not be flattened into "layer (c) is done".

### 1.1 Settled: axis-C *lexical* pressure on the grouping layer is LOW — 1 node in 39

`borrowed` (raw Latin / anglicism, grounded in the taxonomy's own `Latin` column) fires on exactly **one**
grouping node: **`Ad hominem`**. Everything else is authored in French descriptive phrases or FR calques.

Two corroborating measurements make this a **structural fact about the tree**, not an artifact of a weak probe:

- **EN grouping labels are 1:1 with FR everywhere** (97 % coverage, every FR node → exactly one EN node).
  The EN `Family`/`Subfamily`/`Subsubfamily` columns are a **translation of the FR tree, not an independent
  anglophone tradition tree**. There is therefore **no cross-tradition structural divergence encoded at the
  grouping layer at all.**
- Axis-C divergence **does** exist, but it lives at the **leaf-reference** level (different Wikipedia articles
  per language — measured in #853), not in the groupings.

**Consequence for fairness:** the escape hatch the method hoped for is narrow. Layer (c) excuses **one** node.
The remaining heterogeneity cannot be attributed to tradition divergence at the grouping layer, because that
layer carries almost none.

### 1.2 NOT settled: `unflagged` ≠ "the label fits"

The §2.1 constraint has two bullets. Only **lexical availability** (borrowed) proved mechanizable.
**Distorted scope** (label stretched / narrowed vs its members) did **not** — and #857 is admirably explicit
that both proxies failed, with the numbers to prove it:

- `dispersion_fr` **saturates**: median **1.00** at d2 and d3 — essentially every leaf is its own Wikipedia
  article, so "distinct slugs ÷ leaves" ≈ 1 everywhere. It cannot separate a tight cluster from a sprawled one.
- EN-grouping divergence is **1:1 everywhere** → zero signal, for the reason in §1.1.

So for the 6 axis-A candidates, "no mechanical (c) flag" means **c-lexical is clear and c-scope is
unmeasured** — *not* "(c) is low". The decision rule's `(a)-high / (b)-low / (c)-low → genuine tree tension`
branch is therefore reached on **one half** of (c), not both.

**I am not treating that as a blocker**, and the reason is specific: for a node to be excused as a *tradition*
artifact it needs a cross-tradition signal, and §1.1 established that the grouping layer has none to give.
A stretched FR label is an **axis-A authoring choice** (jsboige's, legitimately arbitrable), not a tradition
divergence. So the unmeasured half would refine *why* a node is heterogeneous — it would not move it out of
his column. The verdict stands; the caveat is recorded because a future reader must not mistake `unflagged`
for a positive scope match.

### 1.3 Reference-reachability measures *evidential risk*, not tradition divergence

Worth stating plainly to prevent a category error downstream: #853 answers **"how solid is the evidence base
under this node?"** — a different question from **"is this heterogeneity a tradition artifact?"** It does not
feed the bridge-vs-defect test directly. Its proper use is as a **precondition on arbitration** (§3.3) and as
an independent **repair backlog** (§5). The taxonomy is **82.5 % Wikipedia-anchored** — the skeleton is sound.

---

## 2. `Ad hominem` — resolved as a bridge, and its `c-DEFER` is lifted

The acompte's headline A-candidate: a = 0.811, **b = 0.00** (all four leaves natively schemed — the cleanest
material in the whole measure), tension = `rebut`/CA (*tu quoque*, anchor 1361) vs `undermine`/I (direct
ad hominem, anchor 1398).

Three independent readings converge:

1. **It is the one `borrowed` node** (raw Latin, kept at d2 while the FR calque *Attaque personnelle* sits at d3).
2. Its Obstruction base is **reasonably anchored** (69 % Wikipedia, 15 % dictionary) → the heterogeneity is
   **not** an artifact of evidential decay.
3. The seam it spans is a documented cross-cultural one: *tu quoque* (Latin, scholastic) vs *whataboutisme*
   (Cold-War anglophone).

**Verdict: cross-cultural bridge node. Annotate the seam; do NOT flatten, do NOT bill as a grouping defect.**
This lifts the `c-DEFER` that #850 attached to it, and it ratifies the deferred modelling decision recorded
in #845 — that decision was correct and should stay deferred rather than be forced.

A node keeping a Latin label at d2 while its French calque sits at d3 is precisely what a bridge looks like:
the author declined to pick a side. That is a virtue of the tree, and the measurement now says so.

---

## 3. The 6 axis-A candidates — and why only **2** should reach jsboige now

### 3.1 A confidence gate the acompte's `n ≥ 3` threshold does not provide

Entropy is normalised **by the number of distinct attack types actually present**. Two consequences the
synthesis must apply before handing anyone an arbitration list:

- At **n = 3–4**, a merely uneven split reads as high heterogeneity almost by construction. A node with
  `{3 undermine, 1 rebut}` scores **0.811**; `{2, 2}` scores **1.00**. Small-n `a-high` is **thin evidence**.
- A node spanning **2** attack types can score 1.00 exactly like one spanning **3**, because the denominator
  adapts. `Refus du débat` (0/2/2) and `Saboter le débat` (1/1/1) both read 1.00, though the second is
  genuinely more heterogeneous.

Neither is an error in po-2024's instrument — both are documented normalisation choices. But a verdict that
ranks nodes for a human's scarce attention must weight them.

**Gate applied:** `n ≥ 8` = supported · `n = 5–7` = indicative · `n = 3–4` = thin.

### 3.2 The 6, gated

| # | Node | Lvl | n | a | b | evidence base | tier |
|---|------|-----|---|---|---|---------------|------|
| 1 | **Biais naturels** | d3 | **8** | **0.954** | 0.375 | not exposed | **supported** |
| 2 | **Pensée biaisée** | d2 | **13** | 0.890 | 0.462 | not exposed | **supported** |
| 3 | Argument bâclé | d2 | 7 | 0.863 | 0.429 | **46 % wiki · 24 % dict · 22 % long-tail** | indicative, **evidence-blocked** |
| 4 | Causalité douteuse | d2 | 5 | 0.722 | 0.200 | not exposed | indicative |
| 5 | Erreur de raisonnement | d1 | 14 | **0.592** | 0.357 | 67 % wiki · **25 % dict** | large n, **weakest tension** |
| 6 | Mauvaise interprétation | d2 | **4** | 0.811 | 0.250 | 71 % wiki · 27 % dict | **thin** |

### 3.3 Ranked recommendation to jsboige — **2 real items, not 6**

**Arbitrate now (2):**

- **`Biais naturels`** (d3, n=8) — highest heterogeneity in the whole A-set (0.954 = a near-even
  `3 undercut / 5 undermine` split), lowest fail-loud of the supported tier, evidence base not exposed.
  This is the single cleanest genuine tree-tension signal the audit produced. If exactly one node is worth
  his time, it is this one.
- **`Pensée biaisée`** (d2, n=13) — largest well-supported node, a = 0.890 (`4 undercut / 9 undermine`).
  Caveat to carry into the arbitration: **b = 0.462 sits just under the 0.50 threshold**, so part of this
  node's mix is material resistance, not grouping. Expect the answer to be "annotate the seam", not "regroup".

**Do not arbitrate yet (1) — fix the evidence first:**

- **`Argument bâclé`** (d2, n=7) — a = 0.863 looks arbitrable, but it is the **most evidentially exposed**
  node in the A-set: only **46 % Wikipedia**, 24 % dictionary, **22 % long-tail**. Deciding a grouping
  question on a base where half the citations are decaying or dead is deciding on sand. **Route to the
  reference-repair backlog (§5), then re-measure.** This is the one place where #853's evidential axis
  changes what should happen, and it is why that layer was worth instrumenting.

**Watch only (1):**

- **`Erreur de raisonnement`** (d1, n=14) — largest n but **a = 0.592**, barely over the 0.50 threshold, and
  `12 undercut / 2 undermine` is a dominated distribution (homogeneity ratio 0.857). Family-level, weak
  tension, on the only base at 25 % dictionary. **No arbitration warranted.**

**Drop from the list (2):** `Causalité douteuse` (n=5, indicative, b=0.20 — clean but modest a) and
`Mauvaise interprétation` (**n=4, thin**). Neither justifies a human decision at this evidence level.

### 3.4 Projected-only: `Mauvaise déduction`

Appears as A-candidate **only** in the projected set (n=10, a=0.88, b=0.20), i.e. it depends on the
docs-only Layer-C cluster propositions (#843 Inconsistance + accident #841). #853 reads it as *"genuine
tree-tension AND evidentially exposed (28 % dict)"* → real arbitration, not a tradition artifact. **Hold
until the underlying clusters are ratified**; arbitrating on unratified propositions would invert the
gating order.

---

## 4. The chantier measurably works — quantified endorsement

Folding the 8 native-rich Layer-C clusters into the base moves the numbers in the intended direction:

| metric | 145 (prod) | projected (191) | delta |
|--------|-----------|-----------------|-------|
| native-scheme rate | 34.5 % | 41.9 % | **+7.4 pp** |
| fail-loud (b) | 65.5 % | 58.1 % | **−7.4 pp** |

And two families **flip from `B+A` (material-resistance) to `A-candidate` (auditable tree tension)**:
**Obstruction** (b 0.67 → 0.49) and **Erreur mathématique** (b 0.50 → 0.48).

That is the strategy validating itself: **targeting native-rich clusters converts axis-B opacity into
axis-A auditability.** Before the chantier, those two families could not be assessed for grouping quality
at all — the material was too refractory. Now they can. This is the strongest argument for continuing #498
along the same targeting rule, and it should be read as an endorsement of po-2024's cluster selection, not
merely a statistic.

**Where the next clusters belong** (largest B+A families with no Layer-C coverage yet):

| family | n | a | b | why it is next |
|--------|---|---|---|----------------|
| **Abus de langage** | 57 | 0.67 | **0.88** | largest family, most refractory material — biggest auditability gain per unit of work |
| **Tricherie** | 22 | 0.95 | 0.59 | high heterogeneity locked behind material resistance |
| **Insuffisance** | 16 | 0.95 | 0.50 | at threshold; also the **highest-evidential-risk family** (63 % wiki, 17 % long-tail, H = 0.68) |

`Influence` needs nothing: the sole fully mechanism-coherent family (a = 0.414, mostly `undermine`) **and**
the best-anchored (89 % Wikipedia). Coherent and well-evidenced — leave it alone.

---

## 5. Reference-repair backlog — actionable **without** jsboige

Cleanly separable from every grouping question, and therefore executable now:

- **134 dead references (5.2 %)** — an **upper bound** (HEAD-probe over-counts; `rationalwiki.org` returns
  503 to HEAD but 200 to GET). Re-probe with GET before repairing, or the worklist will contain phantoms.
- Sharpest evidential shapes: **`Comparaison abusive`** (80 % dictionary, 20 % wiki) · **`Définition
  inconsistante`** (50 % long-tail, 17 % wiki) · **`Argument d'autorité`** (47 % long-tail, 31 % wiki) ·
  `Sophisme d'Explication` (40 % dict, 10 % wiki).
- **`Argument bâclé`** is the priority repair, because a grouping arbitration is **blocked** behind it (§3.3).
- Repair = repoint to a stable anchor (Wikipedia where one exists, `archive.org` otherwise). 39 refs are
  already archive-repointed, so the pattern is established.

Note the standing caveat: a `200` from a domain-squatted or repurposed page still reads `alive`. Mechanical
reachability is not evidential validity.

---

## 6. Verdict summary

1. **No restructuring proposed.** "Changes at the margin" is confirmed and quantified. The tree holds.
2. **Layer (b) dominates** (65.5 % fail-loud): most heterogeneity is **material resistance**, not authoring.
3. **`Ad hominem` is a bridge node** — `c-DEFER` lifted, annotate the seam, do **not** flatten. #845's
   deferred decision was right.
4. **Axis-C lexical pressure on the grouping layer is LOW (1/39)**, and the EN grouping tree is a 1:1 FR
   calque → tradition divergence lives at the **leaf-reference** level, not in the groupings.
5. **2 nodes reach jsboige**, not 6: **`Biais naturels`** (cleanest signal) and **`Pensée biaisée`** (largest
   supported, partly material). **`Argument bâclé` is evidence-blocked**; `Erreur de raisonnement` is
   watch-only; 2 more are too thin at n = 4–5.
6. **The chantier works, measurably** (−7.4 pp fail-loud; 2 families become auditable). Next clusters:
   **Abus de langage**, then **Tricherie**, then **Insuffisance**.
7. **A repair backlog exists that needs no arbitration** (§5) — start with `Argument bâclé`, which unblocks item 5.

### Method refinements earned by this pass (for the method doc, not blocking)

- Add a **minimum-n confidence tier** to `a-high` (≥8 supported / 5–7 indicative / 3–4 thin). Half the
  A-candidates sat at n = 4–5.
- Record **how many distinct attack types are present** alongside `entropy_norm`: a 2-type node and a 3-type
  node both reach 1.00 under present-type normalisation, but they are not equally heterogeneous.
- State that **`unflagged` is the absence of a flag, never a positive scope match** — the stretched/narrowed
  half of §2.1 is **unmeasured**, and mechanically unmeasurable without an LLM-assisted semantic pass.
- Keep **evidential risk (#853) as a precondition on arbitration**, not as an input to bridge-vs-defect.
  It changed the disposition of exactly one node (`Argument bâclé`) — and that alone justified the layer.

---

## Governance

- **Verdict, not application.** No CSV touched, no node reorganised. Every item in §3.3 is a **proposal for
  jsboige's ratification**; §5 is hygiene executable without him.
- **0 prod-CSV write** (post-T&A freeze). Docs-only.
- Inputs were held to `MEASURE not verdict` by po-2024 across all three installments, including explicit
  documentation of the two failed proxies (#857 §3) so no future worker re-tries them. That discipline is
  what made this synthesis possible; the honesty about `dead` being an upper bound and `unflagged` being a
  non-claim is load-bearing, not decorative.

## Refs

Inputs: [#850](847-acompte-homogeneity.md) · [#853](847-layerc-reference-reachability.md) ·
[#857](847-layerc-label-distortion.md) · method [`aif-structural-audit-method.md`](aif-structural-audit-method.md).
Tracking #847 · chantier #498 · qualifying cluster #845 · cross-cluster corroboration #846 · anchor audit #770 ·
OWL AIF wiring #763 · 0-fabrication #677.
