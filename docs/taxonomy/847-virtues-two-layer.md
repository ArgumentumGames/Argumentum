# 2026-08-28 — #847 two-layer homogeneity, release re-run — Fallacies drift check + FIRST Virtues pass

> **⚠ STATUS = MEASURE ONLY, not a verdict.** Input for ai-01's synthesis and the jsboige ratification queue. No
> reorganisation proposal, no structural claim beyond the mechanical decision-rule tags. **0 prod-CSV write** —
> this pass only reads the prod CSVs at `bf58c275` and writes docs. Method: [`aif-structural-audit-method.md`](aif-structural-audit-method.md).
> Prior art: acompte [`847-acompte-homogeneity.md`](847-acompte-homogeneity.md) (2026-07, layers a+b), synthesis
> [`847-synthesis-verdict.md`](847-synthesis-verdict.md) (#904). Author: po-2024 (compute). Verdict: ai-01. Tracking: #847.
>
> Trigger for this re-run: the 25/08 release GO unfroze the chantier, and #989 branch B (PR #1216) just published the
> Virtues AIF attack layer — the Virtues tree became measurable by this instrument **for the first time**.

---

## TL;DR (numbers only)

- **Fallacies: ZERO drift.** 145 typed leaves, `87 undercut / 53 undermine / 5 rebut` — **identical** to the July
  acompte. The acompte's tables, the verdict §3.3 nominations (`Biais naturels`, `Pensée biaisée`, evidence-blocked
  `Argument bâclé`) and the §5 repair backlog all stand unchanged. Nothing to re-arbitrate on the Fallacies side.
- **Virtues (first pass):** 222 typed leaves `206 / 13 / 3`, corpus entropy-norm **0.267**, homogeneity **0.928** —
  globally mechanism-coherent. **4 of 7 families are `coherent`**; 3 read `A-candidate` under the mechanical tag
  (all `supported`, n ≥ 8): **Présentation intègre** (a = 0.692), **Justesse lexicale** (0.650),
  **Honnêteté intellectuelle** (0.521).
- **Layer (b) does not transplant to Virtues as written** — both readings reported in §3: the strict July definition
  (`_Inference` scheme tokens) gives fail-loud **1.000 everywhere** (degenerate: the Virtues columns carry Walton
  scheme *names*, not AIF tokens); the transposed reading (any Walton scheme in `AIF_skosDirectRef`) gives **0.000**
  (222/222 carry one). So the mechanical tags below all read `A-candidate`/`coherent` — **there is no
  material-resistance axis on the Virtues side at this instrument's resolution.**
- **The derivation caveat governs everything below:** the Virtues attack types are **100 % script-derived** from
  `crossLink_Opposes` (plan #750 v2, published WITH its derivation by #989-B — 206/13/3 is the 1:1 fingerprint).
  Layer (a) here measures **where the virtues that oppose fallacies 889/804 (→ undermine) or 340 (→ rebut) sit in
  the tree** — the interaction of two authored layers (grouping × opposes-network), *not* independent per-leaf
  editorial attack judgments as on the Fallacies side.

---

## 1. Fallacies — drift check vs July

Re-ran the exact acompte instrument (same entropy normalisation, same `_Inference` scheme test, same 0.50 thresholds,
same d1/d2/d3 granularities) on `bf58c275`:

| metric | July acompte | 2026-08-28 | delta |
|---|---|---|---|
| typed leaves (prod) | 145 | **145** | 0 |
| type split | 87 / 53 / 5 | **87 / 53 / 5** | 0 |
| nodes measured (d1+d2+d3) | 39 grouping / 91 total | **91** (32 `A-candidate`\|`B+A`) | same structure |

The projected set (n=191, the 8 docs-only Layer-C clusters) remains **unratified and unserialized** — unchanged since
July. Consequence: **the July synthesis verdict needs no refresh for the release.** Its §3.3 queue for jsboige
(`Biais naturels` d3 n=8 · `Pensée biaisée` d2 n=13 · `Argument bâclé` evidence-blocked pending §5 repair) is still
the live queue. Full table: [`847-fallacies-rerun.csv`](847-fallacies-rerun.csv).

## 2. Virtues — first pass, corpus level

| metric | value |
|---|---|
| typed leaves | **222 / 223** (root `Argument valable`, pk 0, empty by design) |
| type split | `206 undercut / 13 undermine / 3 rebut` (the #989-B script fingerprint, re-verified live) |
| corpus entropy-norm | **0.267** |
| homogeneity ratio | **0.928** |
| (b) strict-July (`_Inference` tokens) | native 0/222 → fail-loud **1.000** — *degenerate, definition does not transplant* |
| (b) transposed (Walton scheme in DirectRef) | native **222/222** → fail-loud **0.000** |

## 3. Virtues — per-node, mechanical tags

Full table: [`847-virtues-pass.csv`](847-virtues-pass.csv). Non-`coherent` nodes (16), with the confidence gate:

| level | node | n | u/m/r | a | tag | gate |
|---|---|---:|---|---:|---|---|
| d1 | **Présentation intègre** | 25 | 18/5/2 | **0.692** | A-candidate | supported |
| d1 | **Justesse lexicale** | 18 | 15/3/0 | **0.650** | A-candidate | supported |
| d1 | **Honnêteté intellectuelle** | 27 | 22/4/1 | **0.521** | A-candidate | supported |
| d2 | Fidélité aux faits | 6 | 2/4/0 | 0.918 | A-candidate | indicative |
| d2 | Définitions recevables | 7 | 4/3/0 | 0.985 | A-candidate | indicative |
| d2 | Rhétorique acceptable | 16 | 14/2/0 | 0.544 | A-candidate | supported |
| d2 | Clarté des enjeux · Résultats valides | 8 · 8 | 7/0/1 · 7/1/0 | 0.544 | A-candidate | supported |
| d2 | Communication authentique · Equilibre émotionnel | 4 · 4 | 2/2/0 · 2/0/2 | 1.000 | A-candidate | **thin** |
| d3 | 6 nodes (n=2–7) | 2–7 | — | 0.592–1.000 | A-candidate | thin/indicative |

`coherent` (a < 0.50): **Argument pertinent** (33, a=0), **Inférence maîtrisée** (55, a=0), **Échange enrichissant**
(44, a=0), **Sens quantitatif** (20, a=0.286) — the four families whose virtues oppose no override fallacy.

## 4. Observation for the synthesis (flagged, not concluded)

Every non-default attack type traces mechanically to `crossLink_Opposes` membership: **undermine ⇔ opposes 889
*Mensonge* (Tricherie) or 804 *Acception arbitraire* (Abus de langage); rebut ⇔ opposes 340 *Appel aux conséquences*
(Influence)**. The three A-candidate families are therefore exactly the ones containing the virtues that oppose
those three fallacies:

- **Fidélité aux faits** (2/4, undermine *dominant*) = the anti-*Mensonge* / anti-*Attention sélective* cluster;
- **Définitions recevables** (4/3) = the anti-*Acception arbitraire* / *Équivoque* / *Acception vague* cluster;
- **Equilibre émotionnel / Clarté des enjeux** carry the two *rebut* (anti-340) nodes.

Read naively, the tags say "tree tension". Read with the derivation caveat, they say something else: **the
heterogeneity IS the virtues-mirror-of-fallacies structure** — honesty-to-facts virtues oppose *lying*
(undermine: attack the source/premise), definitional-exactness virtues oppose *arbitrary redefinition*, and they
cluster in the tree exactly where their semantics puts them. That reading would make these flags **corroboration of
the cross-corpus mirror** (axis-C solvent working as designed), not grouping defects — but that is a synthesis
call, not this document's. Recorded here so the verdict can make it explicitly rather than by omission.

**What would falsify the mirror reading:** a family mixing anti-889 and anti-804 virtues *without* semantic unity,
or the override fallacies (889/804/340) themselves being revised — both measurable on request.

## 5. Provenance & reproducibility

- Script: [`847-virtues-two-layer.py`](847-virtues-two-layer.py) (reads the prod CSVs at repo root; emits both CSVs).
- Measured on master `bf58c275` (2026-08-28) by po-2024, dispatch ai-01 `t5p2ia` [secondaire].
- CSVs: [`847-fallacies-rerun.csv`](847-fallacies-rerun.csv) (91 nodes), [`847-virtues-pass.csv`](847-virtues-pass.csv) (91 nodes).
- **0 prod-CSV write. 0 structural proposal. Tags are mechanical.** Synthesis/verdict: ai-01. Ratification: jsboige.
