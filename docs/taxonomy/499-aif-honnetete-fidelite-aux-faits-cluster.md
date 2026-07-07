# #499 AIF chantier — PR-2 (Virtues) : Honnêteté intellectuelle / Fidélité aux faits cluster (proposition, gated)

**Issue:** [#499 — Virtues parity (relational + AIF + export)](https://github.com/ArgumentumGames/Argumentum/issues/499) (CLOSED — relational layer done; this advances the **ratified I/RA/CA serialization** #707§4(a))
**Author:** Claude Code @ myia-po-2024 (worker)
**Dispatch:** ai-01 `ompwhx` [primaire] #499 — idle-de-secours mandate: *"Continue #499 cluster par cluster en mode autonome"*
**Date:** 2026-07-07
**Base:** master `defd53d4`
**Predecessor:** [`499-aif-raisonnement-causalite-cluster.md`](499-aif-raisonnement-causalite-cluster.md) (PR-1 #741, pilot)
**Status:** **PROPOSITION ONLY — GATED. 0 write to prod CSV.** Second cluster of the Virtues I/RA/CA chantier. Written to **demonstrate the method generalizes** beyond the pilot's uniform pattern (PR-1 was 4/4 `undercut`/`RA-node`); this cluster shows a **mixed** distribution.

---

## TL;DR

- **Why a second cluster now** (vs waiting on the PR-1 Option A/B/C FLAG): the dispatch's idle mandate says continue cluster-by-cluster; the I/RA/CA decomposition (the load-bearing semantic content) is **Option-independent** — only the last two table columns change if jsboige picks B or C. A second cluster in a **different family** gives jsboige two concrete, contrasted examples to decide from, and shows the method is not a uniform mold.
- **One cluster modeled**: *Honnêteté intellectuelle* (family 6) / *Fidélité aux faits* (subfamily 6.1) — anchor pk 153 + leaves 154/155/156. Chosen because it is the cleanest **path-matched** mirror for a *different* Fallacies family (Tricherie): Fallacies subfamily *Arranger les faits* sits at **path 6.1.x** (889/942/953) — the Virtues subfamily sits at **path 6.1**. Same path-prefix corroboration as PR-1, independent family.
- **Mixed distribution (the point of this PR)**: the three leaves resist three different attack mechanisms → **1 `undermine`/`I-node` + 2 `undercut`/`RA-node`**. This contrasts with PR-1's 4/4 `undercut`/`RA-node` and proves the columns are **derived per-case from the fallacy mechanism**, not templated.
- Native AIF vocab only (0 fabrication). Written under **Option A** (same FLAG as PR-1, same one-line adjustability).

---

## 1. The cluster

**Honnêteté intellectuelle** family (Virtues root pk 152, path 6) / **Fidélité aux faits** subfamily (pk 153, path 6.1). From `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv`:

| pk | depth | path | title_fr | description_fr (excerpt) |
|----|------:|------|----------|--------------------------|
| 153 | 2 | 6.1 | Fidélité aux faits | (anchor subfamily) |
| 154 | 3 | 6.1.1 | Vérité des faits | Présenter des faits précis, vérifiés et fidèles à la réalité |
| 155 | 3 | 6.1.2 | Attribution juste | Attribuer fidèlement les sources et les citations |
| 156 | 3 | 6.1.3 | Considération équilibrée | Présentation neutre des faits, sans parti pris |

**Opposed Fallacies** (from `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, cross-checked by PK):

| fallacy pk | path | family | subfamily | desc_fr | mechanism (→ attack-type) |
|-----------|------|--------|-----------|---------|---------------------------|
| 889 | 6.1.1 | Tricherie | Arranger les faits | Vous affirmez quelque chose que vous savez faux | the factual premise is false → **undermines the I-node** (the witness/fact itself is rotten) |
| 942 | 6.1.2 | Tricherie | Arranger les faits | Vous donnez du poids à votre argument en citant une source fausse, mal identifiée, incompétente ou inventée | attacks the Witness-Testimony scheme's credibility CQ → **undercuts the RA-node** (the inference link from testimony to conclusion) |
| 953 | 6.1.3 | Tricherie | Arranger les faits | Vous ne présentez que les faits qui soutiennent votre thèse, en occultant ceux qui la contredisent | cherry-picking biases the facts→conclusion transition → **undercuts the RA-node** (the Bias-scheme transition) |

> **Mirror proof.** Fallacies *Arranger les faits* (Tricherie family) and Virtues *Fidélité aux faits* share the **path prefix 6.1** — the taxonomy's path numbering encodes the inverse pairing at the structure level, exactly as PR-1 found for *Causalités* (path 4.1). Second independent corroboration of jsboige's inverse paradigm.

## 2. Reused grounding (no re-derivation)

From the validated 12-col layer (`docs/taxonomy/499-scaleup-phase2-honnetete-annotations.csv`, rows for pk 154/155/156):

| Virtue pk | scheme (reused) | restored CQ (reused) | opposed fallacies (reused) |
|----------|-----------------|----------------------|----------------------------|
| 154 | Argument from Witness Testimony | Les faits rapportés sont-ils exacts, vérifiables et fidèles à la réalité ? | 889; 1297 |
| 155 | Argument from Witness Testimony | La source citée est-elle correctement identifiée et compétente ? | 942; 889 |
| 156 | Argument from Bias | La présentation des faits est-elle exempte de parti pris ? | 953; 177 |

The I/RA/CA decomposition below **only adds**: which node the scheme is (RA-node), and which attack-type the opposed fallacy uses (read off the mechanism, §1).

## 3. Proposition (per leaf, Option A) — mixed distribution

| pk | title_fr | I-node (premises→conclusion, shape) | RA-node (scheme held) | CA-node resisted (attack prevented) | `AIF_attackType` | `AIF_attackedNode` |
|----|----------|-------------------------------------|-----------------------|-------------------------------------|------------------|--------------------|
| 153 | Fidélité aux faits (anchor) | a factual claim is reported and its accuracy/source/balance are verified | Argument from Witness Testimony | the witness I-node is held against falsehood + the testimony RA-node is held against source-distortion + selection-bias | `undermine` (primary) | `I-node` |
| 154 | Vérité des faits | a fact is reported and checked for accuracy against reality | Argument from Witness Testimony | the witness I-node is held against the falsehood (889 *affirmer ce qu'on sait faux*) | `undermine` | `I-node` |
| 155 | Attribution juste | a source is cited and checked for correct identification + competence | Argument from Witness Testimony | the testimony RA-node is held against source-fabrication/misidentification (942) — the CQ *le témoin est-il crédible ?* is the one 942 defeats | `undercut` | `RA-node` |
| 156 | Considération équilibrée | the facts are presented and checked for partisan balance | Argument from Bias | the bias RA-node is held against cherry-picking (953) — the facts→conclusion transition is the one 953 distorts | `undercut` | `RA-node` |

**Distribution: 1 `undermine`/`I-node` + 2 `undercut`/`RA-node`.** This is the deliberate contrast with PR-1 (4/4 `undercut`/`RA-node`): the attack-type is **derived from the fallacy mechanism** (falsehood→undermine the premise; source-distortion/bias→undercut the inference), not templated. The anchor (153) takes the **primary** attack-type of its subfamily (`undermine`, the most direct threat to factual fidelity).

## 4. Why this matters (generalization evidence)

A reviewer (or jsboige) reading PR-1 alone could worry the method **always** produces `undercut`/`RA-node` (a trivial mold). This PR disproves that:

- **Undermine appears** (154/889): a lie attacks the *premise* (I-node), not the *inference* (RA-node). The distinction is load-bearing in AIF — undermine and undercut defeat different components.
- **Undercut varies by scheme** (155 Witness Testimony vs 156 Bias): the attacked RA-node is not always the same scheme, so the column is not a constant.
- **Honest nuance on 942** (documented, not fabricated): 942 has four variants (*source fausse, mal identifiée, incompétente, inventée*). The *inventée* variant would undermine the I-node (the witness doesn't exist); the *incompétente* variant undercuts the RA-node (the witness exists but the testimony CQ fails). This PR classifies 942 as **primarily `undercut`/`RA-node`** because the subfamily's restored CQ is about source credibility (a RA-node concern); the *inventée* edge is flagged in `AIF_skosOther` rather than forcing a second row. **0 fabrication** — the choice is documented, the native token is used, and the ambiguity is recorded.

## 5. Method confirmation (for future Virtue PRs)

PR-1's method holds; this PR adds one refinement:

- **Cluster = one Virtue subfamily** (path X.Y) + its depth-3 leaves. Path-matched Fallacies subfamily (same path prefix) = strongest grounding; `crossLink_Opposes` otherwise.
- **attack-type is derived, not templated**: read the fallacy mechanism — *falsehood/false-premise* → `undermine`/`I-node`; *distorted-inference/bad-transition/bad-scheme-application* → `undercut`/`RA-node`; *direct-counter-conclusion* → `rebut`/`CA-node` (not yet seen in Virtues; would surface opposing a fallacy that asserts the opposite conclusion).
- **Anchor takes the subfamily's primary attack-type** (the most direct threat). Leaves may vary.
- **Multi-mechanism fallacies** (like 942): classify by the **restored CQ's concern** (I-node vs RA-node), flag edge-variants in `AIF_skosOther`. Never fabricate a token to split one row.
- **Reuse the 12-col grounding** — never re-derive scheme/CQ/opposed-PK.

## 6. FAIL-LOUD / honest gaps (this cluster)

- **0 FAIL-LOUD** — every leaf has a native scheme (Witness Testimony / Bias) and a native attack-type (`undermine` / `undercut`). No `AIF_skosOther` needed for a missing token.
- **1 honest nuance** (942, §4) — multi-variant fallacy; classified by primary mechanism, edge-variant flagged in `AIF_skosOther`, not split. Documented, not fabricated.
- **Pending semantic decision** (gate, not a gap): Option A vs B vs C (PR-1 §3). Written under A; two columns adjust post-ratification.

## 7. Coverage (per #499 + #707§4 DoD)

- This PR: **1 subfamily modeled** (Fidélité aux faits, 4 nodes: 153 anchor + 154/155/156).
- Cumulative Virtues I/RA/CA chantier: **2 subfamilies** (Causalités 4.1 + Fidélité aux faits 6.1), **8 nodes**, across **2 families** (Raisonnement valide + Honnêteté intellectuelle), **2 attack-types** observed (undercut + undermine).
- Remaining: 220 nodes / ~13-18 subfamilies. Post-Option-ratification, the back-fill remains largely mechanical (attackType/attackedNode derivable from scheme + fallacy mechanism).
- OWL propagation unchanged (downstream of prod write; Virtues not in `OwlAdapter`).

## 8. Gate boundaries (HARD)

- ❌ No prod CSV write, no DB write, no OWL regen, no `Cards/` write. **Proposition doc only.**
- ✅ Reuses the validated 12-col layer (June 2026, #518 CLEAN) — no re-derivation, no fabrication.
- ✅ All AIF tokens native (`undermine`, `undercut`, `I-node`, `RA-node`, `Witness Testimony`, `Bias`) — discipline #677 held.
- ✅ Mirror of po-2023 Fallacies format (proposition gated, 1 cluster, method + flag).

Relates: #499, #498 (Fallacies chantier, po-203 owner), #707 §4 (ratified I/RA/CA schema, Option a), #677 (0 fabrication), #518 (validator), #133/#130 (OWL), `499-aif-raisonnement-causalite-cluster.md` (PR-1), `499-virtues-parity-closure.md`, `499-virtues-prod-write-spec.md`, `498-coverage-status.md`.
