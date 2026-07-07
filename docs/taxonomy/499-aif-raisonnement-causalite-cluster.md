# #499 AIF chantier — PR-1 (Virtues) : Raisonnement valide / Causalités bien identifiées cluster (proposition, gated)

**Issue:** [#499 — Virtues parity (relational + AIF + export)](https://github.com/ArgumentumGames/Argumentum/issues/499) (CLOSED — relational layer done; this advances the **ratified I/RA/CA serialization** #707§4(a))
**Author:** Claude Code @ myia-po-2024 (worker)
**Dispatch:** ai-01 `ompwhx` [primaire] #499 Virtues AIF (schéma #707§4(a) ratifié, CSV distinct Fallacies → 0 collision)
**Date:** 2026-07-07
**Base:** master `defd53d4`
**Status:** **PROPOSITION ONLY — GATED. 0 write to prod CSV.** Mirror of the po-2023 Fallacies chantier format (#498 PR-1 #699). This is the **pilot** for serializing the ratified `AIF_attackType` + `AIF_attackedNode` schema on the Virtues side, with the **inverse-paradigm adaptation** flagged for jsboige validation before any scale-up.

---

## TL;DR

- jsboige ratified schema **#707 §4 = Option (a)** on 2026-07-07 (interactive): two new CSV columns `AIF_attackType` (`undermine`/`undercut`/`rebut`) + `AIF_attackedNode` (`I-node`/`RA-node`/`CA-node`). po-2023 owns the Fallacies side (#498). This PR is the **Virtues mirror pilot**.
- The Virtues relational/AIF layer (12-col `crossLink_*` + `AIF_skos*`) is **already generated and validated CLEAN** (222/222 nodes, 9 PRs merged June 2026, validator #518 12/12 — see [`499-virtues-parity-closure.md`](499-virtues-parity-closure.md)). This PR does **not** redo it; it **extends** it with the I/RA/CA decomposition the ratified schema asks for.
- **One cluster modeled**: *Raisonnement valide* (family 4) / *Causalités bien identifiées* (subfamily 4.1) — anchor pk 80 + leaves 81/82/83. Chosen because it is the **cleanest inverse-paradigm mirror**: Fallacies subfamily *Causalité douteuse* sits at **path 4.1.x** (698 Pétition de principe, 707 Inversion de causalité, 719 Effet cigogne) — the Virtues subfamily *Causalités bien identifiées* sits at **path 4.1**. Same path, opposite tenor.
- **Inverse-paradigm adaptation (FLAGGED, not decided alone):** the ratified columns describe an *attack* (`attackType` on `attackedNode`). A Virtue is the *good holding* of a scheme — it does not attack. Three serialization options exist for the Virtues side (§3); this PR recommends **Option A — "attack resisted"** (the columns record the attack-type the virtue *prevents* on the node it *holds*). Native AIF vocab only (0 fabrication). **jsboige validates the choice** before the 222-row back-fill.

---

## 1. The cluster

**Raisonnement valide** family (Virtues root pk 79, path 4) / **Causalités bien identifiées** subfamily (pk 80, path 4.1). From `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv`:

| pk | depth | path | title_fr | description_fr (excerpt) |
|----|------:|------|----------|--------------------------|
| 80 | 2 | 4.1 | Causalités bien identifiées | Capacité à distinguer clairement les causes des effets… (anchor subfamily) |
| 81 | 3 | 4.1.1 | Indépendance des prémisses | Les prémisses doivent soutenir la conclusion sans la présupposer |
| 82 | 3 | 4.1.2 | Causalité bien orientée | Capacité à reconnaître la direction correcte d'une relation causale |
| 83 | 3 | 4.1.3 | Exclusion des causes alternatives | Analyse méthodique visant à écarter les explications causales concurrentes |

**Opposed Fallacies** (from `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, cross-checked by PK):

| fallacy pk | path | family | title_fr | mechanism (why it is an undercut) |
|-----------|------|--------|----------|-----------------------------------|
| 698 | 4.1.1 | Erreur de raisonnement | Pétition de principe | circularity — the premise presupposes the conclusion → undercuts the RA-node (Rule inference's own support link) |
| 707 | 4.1.2 | Erreur de raisonnement | Inversion de causalité | reverses causal direction → undercuts the RA-node (Cause-to-Effect inference) |
| 719 | 4.1.3 | Erreur de raisonnement | Effet cigogne | correlation≠causation → undercuts the RA-node (Cause-to-Effect inference) |
| 633 | 3.2.1 | Erreur mathématique | Relation infondée | posits a relation without basis → undercuts the RA-node (cross-family, honest scheme-divergence) |

> **Mirror proof.** The Fallacies *Causalité douteuse* subfamily and the Virtues *Causalités bien identifiées* subfamily share the **path prefix 4.1**. The taxonomy's path numbering encodes the inverse pairing at the structure level — independent corroboration of jsboige's inverse paradigm ("a Virtue is the good holding of the same scheme a Fallacy defeats"). The anchor and its 3 leaves carry the same path children (4.1.1/4.1.2/4.1.3) on both sides.

## 2. Reused grounding (no re-derivation)

The 12-col relational layer (merged June 2026) already established, per Virtue PK: the Walton scheme (`AIF_skosDirectRef`), the restored critical question (`AIF_skosMappingType`), and the opposed Fallacy PKs (`crossLink_Opposes`). This PR **reuses** that grounding and only adds the I/RA/CA decomposition:

| Virtue pk | scheme (reused) | restored CQ (reused) | opposed fallacies (reused) |
|----------|-----------------|----------------------|----------------------------|
| 81 | Argument from Rule | Les prémisses soutiennent-elles la conclusion sans la présupposer ? | 698; 1297 |
| 82 | Argument from Cause to Effect | La direction causale invoquée est-elle correctement orientée ? | 707; 719 |
| 83 | Argument from Cause to Effect | Les causes alternatives plausibles ont-elles été écartées ? | 633; 719 |

(source: `docs/taxonomy/499-scaleup-phase2-raisonnement-a-annotations.csv`, rows for pk 81/82/83.)

## 3. The inverse-paradigm adaptation (FLAGGED for jsboige)

The ratified columns are attack-shaped: `AIF_attackType` ∈ {`undermine`, `undercut`, `rebut`} and `AIF_attackedNode` ∈ {`I-node`, `RA-node`, `CA-node`}. A Virtue is not an attack — it is the **good holding** of a scheme. Three ways to serialize the columns on the Virtues side:

| Option | `AIF_attackType` (Virtue side) | `AIF_attackedNode` (Virtue side) | Pros | Cons |
|--------|-------------------------------|----------------------------------|------|------|
| **A — "attack resisted" (recommended)** | the attack-type the virtue **prevents** (inherited from the opposed fallacy: mostly `undercut`) | the node the virtue **holds/defends** (mostly `RA-node`) | Cross-dataset coherent (same native values); encodes the inverse relation; 0 fabrication; useful to the EPITA consumer (knows which attack the virtue is the antidote to) | "attackType" on a virtue is a semantic oxymoron at face value — needs the column doc to say "attack resisted/prevented" |
| B — structurally empty | empty | empty | Honest (a virtue attacks nothing) | Columns useless for 222 nodes; loses the I/RA/CA info the ratified schema exists to capture |
| C — AIF-native support encoding | n/a (AIF has no native "support" attack-type; RA-node *is* the support) | `RA-node` | Conceptually purest (a virtue = an RA-node that holds) | Cannot fill `AIF_attackType` without fabricating a `support` token → violates discipline #677 |

**Recommendation: Option A.** It preserves the ratified native vocabulary (`undercut` + `RA-node` are both native AIF), requires no fabrication, and the cross-dataset coherence is real: the row says "this virtue is the good holding of the RA-node that fallacy X undercuts." The column-level documentation (companion to the prod write) will state that on the Virtues side the pair means *attack resisted*, not *attack committed*.

**This is a judgment call on the inverse-paradigm semantics. It is FLAGGED for jsboige validation before the 222-row back-fill. The pilot below assumes Option A to make the proposition concrete; if jsboige picks B or C, only the last two columns of the table change.**

## 4. Proposition (per leaf, Option A)

I/RA/CA decomposition (Walton/AIF), assuming Option A:

| pk | title_fr | I-node (premises→conclusion, shape) | RA-node (scheme held) | CA-node resisted (attack prevented) | `AIF_attackType` | `AIF_attackedNode` |
|----|----------|-------------------------------------|-----------------------|-------------------------------------|------------------|--------------------|
| 80 | Causalités bien identifiées (anchor) | a causal claim is made and its direction + alternatives are examined | Argument from Cause to Effect | the causal RA-node is held against inversion/single-cause/correlation undercuts | `undercut` | `RA-node` |
| 81 | Indépendance des prémisses | premises are stated and checked for non-circular support of the conclusion | Argument from Rule | the rule RA-node is held against circularity (Pétition de principe 698) | `undercut` | `RA-node` |
| 82 | Causalité bien orientée | a causal relation is asserted and its direction is verified | Argument from Cause to Effect | the causal RA-node is held against direction inversion (Inversion de causalité 707) | `undercut` | `RA-node` |
| 83 | Exclusion des causes alternatives | a cause is identified after ruling out alternatives | Argument from Cause to Effect | the causal RA-node is held against single-cause / spurious-correlation undercuts (Effet cigogne 719, Relation infondée 633) | `undercut` | `RA-node` |

**Distribution:** 4/4 `undercut` on `RA-node` — consistent with jsboige's observation that "most fallacies live in the undercut" (the Virtue mirror correspondingly lives in holding the RA-node against undercuts). No `undermine`, no `rebut` in this cluster (those would surface in other families — e.g. *Honnêteté intellectuelle* resisting evidence-undermining).

## 5. Method (for the chantier's future Virtue PRs)

- **Cluster = one Virtue subfamily** (path X.Y) + its depth-3 leaves. Mirror the po-2023 Fallacies cluster shape (anchor + leaves), one subfamily per PR.
- **Prefer path-matched subfamilies** (like 4.1 here): when the Fallacies side has the same path prefix, the inverse pairing is structurally corroborated — strongest grounding. Otherwise use the `crossLink_Opposes` + `prevented_family_pk` already in the 12-col layer.
- **Reuse the 12-col grounding** (scheme, CQ, opposed PKs) — do not re-derive. The I/RA/CA decomposition only adds: which node the scheme is (RA-node), and which attack-type the opposed fallacy uses (read off the fallacy mechanism: circularity/inversion/correlation → `undercut`; evidence-denial → `undermine`; direct counter-conclusion → `rebut`).
- **Fail-loud** if a Virtue's opposed fallacy has no clean AIF attack-type (e.g. a Virtue opposing a purely presentational fallacy) — document in `AIF_skosOther`, never fabricate an `attackType` value outside {undermine, undercut, rebut}.
- **Honest scheme-divergence** (pk 83 opposes 633 *Relation infondée*, an Erreur mathématique, cross-family): kept; the scheme stays Cause-to-Effect on the Virtue side because that is the scheme the Virtue holds — the cross-family link is recorded in `crossLink_Opposes`, not forced into the scheme.

## 6. FAIL-LOUD / honest gaps (this cluster)

- **0 FAIL-LOUD in this cluster** — every leaf has a native scheme (Cause to Effect / Rule) and a native attack-type (`undercut`). No `AIF_skosOther` needed here.
- **Pending semantic decision** (not a gap, a gate): Option A vs B vs C (§3). The proposition is written under Option A; the back-fill of the 222 rows waits on jsboige's pick.

## 7. Coverage (per #499 + #707§4 DoD)

- This PR: **1 subfamily modeled** (Causalités bien identifiées, 4 nodes: 80 + 81/82/83).
- Virtues total: 222 real nodes across 7 families. At ~3-4 nodes/subfamily and ~1 subfamily/PR, the Virtues I/RA/CA chantier is ~15-20 PRs if done subfamily-by-subfamily — **but** if Option A is ratified, the back-fill is largely **mechanical** (attackType/attackedNode derivable from scheme + opposed-family via the existing 12-col annotations), so a single programmatic back-fill PR is plausible post-validation.
- OWL propagation remains downstream of the prod write (Virtues not yet in `OwlAdapter`) — unchanged from the June closure.

## 8. Gate boundaries (HARD)

- ❌ No prod CSV write, no DB write, no OWL regen, no `Cards/` write. **Proposition doc only.**
- ✅ Reuses the validated 12-col layer (June 2026, #518 CLEAN) — no re-derivation, no fabrication.
- ✅ All AIF tokens native (`undercut`, `RA-node`, `I-node`, `Cause to Effect`, `Rule`) — discipline #677 held.
- ✅ Mirror of po-2023 Fallacies format (proposition gated, 1 cluster, method + flag).

Relates: #499 (Virtues parity, inverse paradigm), #498 (Fallacies chantier, po-203 owner), #707 §4 (ratified I/RA/CA schema, Option a), #677 (0 fabrication), #518 (validator), #133/#130 (OWL), `499-virtues-parity-closure.md`, `499-virtues-prod-write-spec.md`, `498-coverage-status.md`.
