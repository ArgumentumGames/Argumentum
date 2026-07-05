# 2026-07-05 — #498 AIF chantier, PR-1 : cluster « Fallacious comparison » (Misleading language)

**Scope**: first cluster of the #498 chantier — model the **Fallacious comparison** subtree (Misleading
language family) as **defeasible exceptions to legitimate argument schemes**, in the Walton/AIF
paradigm. **Proposition only — GATED, 0 write to prod CSV.** Triggered by ai-01 dispatch `yj7u3j`
(primary). This is a multi-session chantier; this PR advances one coherent cluster.

**Repo reference**: master `dc02e847`. Issue: #498 (reformulated, GO jsboige 2026-06-17 verified).
Linked: #133/#130 (existing OWL ontology, regenerated to reflect new structures — not created).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. This doc proposes
> the AIF structure (scheme + exception/CQ) for the cluster's unmapped leaves, derived code=truth
> from the CSV. jsboige ratifies → worker applies the CSV edits in a follow-up PR (gated).

---

## TL;DR

The **Fallacious comparison** subfamily (`Subfamily=Fallacious comparison`, Misleading language
family) has **2 mapped nodes** — pk 833 (d2 subfamily root "Comparaison fallacieuse",
`BiasedClassification_Conflict` directRef, `skos:broadMatch`) and pk 839 (d3 "Fausse analogie",
`Analogy_Inference` defeated by `DifferencesUndermineSimilarity_Conflict`, `skos:closeMatch`) —
across 3 sub-sub clusters: **Faulty comparison** (834-838), **False analogy** (839-843), and
**Association fallacy** (844-845). **This PR scopes to the False analogy sub-sub**: anchor pk 839 +
**4 unmapped leaves** (pk 840, 841, 842, 843). The anchor establishes the cluster's pattern —
**exception to `Analogy_Inference` via the `DifferencesUndermineSimilarity_Conflict` critical
question** — and the 4 leaves are specialized variants of the same defeasible structure. This PR
proposes the AIF exception-modeling for the 4 leaves, reusing the anchor's scheme, and **failing
loud** where a leaf does not honestly fit the Analogy_Inference scheme (documenting it as such
rather than fabricating).

> **⚠ Cluster-boundary correction (code=truth re-verification).** An earlier draft of this
> proposition incorrectly placed pk 838 ("Distinction sans différence") in the False analogy
> sub-sub and labeled pk 834 as the subfamily root. Re-verification against the CSV `Soussousfamille`
> column shows: (a) the true subfamily root is **pk 833** (d2, MAPPED — missing from the earlier
> draft); (b) **pk 838 belongs to Faulty comparison**, not False analogy. The False analogy sub-sub
> is strictly pk 839-843 (anchor + 4 leaves). pk 838 (and its honest scheme-divergence modeling) is
> deferred to **PR-2** (Faulty comparison cluster). This correction strengthens PR-1's methodology:
> all 4 False-analogy leaves now honestly reuse `Analogy_Inference`, with no scheme-divergence
> needed in this cluster.

---

## 1. Cluster state (code=truth)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Misleading language**, subfamily
**Fallacious comparison**:

| pk | text_fr | Subsubfamily | DirectRef | ExceptionRef | MappingType | State |
|----|---------|--------------|-----------|--------------|-------------|-------|
| **833** | **Comparaison fallacieuse** (d2) | *(root)* | `BiasedClassification_Conflict` | — | `skos:broadMatch` | **✅ MAPPED (subfamily root)** |
| 834 | Comparaison abusive (d3) | Faulty comparison | — | — | — | unmapped (d3 anchor → PR-2) |
| 835 | Comparaison incomplète | Faulty comparison | — | — | — | unmapped (→ PR-2) |
| 836 | Classification non exclusive | Faulty comparison | — | — | — | unmapped (→ PR-2) |
| 837 | Comparaison incohérente | Faulty comparison | — | — | — | unmapped (→ PR-2) |
| 838 | Distinction sans différence | Faulty comparison | — | — | — | unmapped (→ PR-2) |
| **839** | **Fausse analogie** (d3) | False analogy | `DifferencesUndermineSimilarity_Conflict` | `Analogy_Inference` | `skos:closeMatch` | **✅ MAPPED (PR-1 anchor)** |
| 840 | Pétition de principe analogique | False analogy | — | — | — | unmapped (**PR-1**) |
| 841 | Analogie étendue | False analogy | — | — | — | unmapped (**PR-1**) |
| 842 | Argument de la similarité fallacieuse | False analogy | — | — | — | unmapped (**PR-1**) |
| 843 | Fausse équivalence | False analogy | — | — | — | unmapped (**PR-1**) |
| 844 | Sophisme d'association (d3) | Association fallacy | — | — | — | unmapped (d3 anchor → PR-3) |
| 845 | Amalgame | Association fallacy | — | — | — | unmapped (→ PR-3) |

The mapped anchor (839) is the **template**: a fallacious comparison = an `Analogy_Inference` scheme
**defeated** because the critical question "do the differences between the compared items undermine
the similarity?" (`DifferencesUndermineSimilarity_Conflict`) is violated. The 4 leaves are
specialized ways that analogy goes wrong.

**This PR scopes to the `False analogy` sub-subfamily (pk 839-843)** — anchor 839 + 4 leaves — the
tightest coherent cluster with an in-sub-sub mapped anchor. The `Faulty comparison` sub-subfamily
(834-838, no in-sub-sub anchor → anchored by the d2 root 833) is **PR-2**; `Association fallacy`
(844-845) is **PR-3**.

---

## 2. The parent model (839, the template)

**Legitimate scheme**: `Analogy_Inference` (AIF native) — "A is like B in property P; B has property
Q; therefore A has property Q." (Walton's argument from analogy.)

**Critical question violated** (the defeaser): `DifferencesUndermineSimilarity_Conflict` — "Are there
relevant differences between A and B that undermine the transfer of Q?" The fallacy ignores these.

**Mapping type**: `skos:closeMatch` — "Fausse analogie" is the informal-logic name for the violated-CQ
case of `Analogy_Inference`.

This is **exactly the paradigm #498 asks for**: a fallacy = a defeasible exception to a legitimate
scheme, named by the violated critical question, not by a Latin label alone.

---

## 3. Proposed AIF structure for the unmapped leaves

For each leaf, the proposal: **(a) legitimate scheme targeted, (b) exception/CQ that defeats it,
(c) `AIF_skosMappingType`**. Reuse the parent's scheme where the leaf is a specialized analogy-failure;
**fail loud** where a leaf targets a different scheme honestly.

### pk 840 — Pétition de principe analogique (Analogical begging the question)
- **desc_fr**: "Vous utilisez une analogie qui suppose déjà la conclusion que vous cherchez à
  démontrer, en assimilant de manière discutable deux situations."
- **Legitimate scheme**: `Analogy_Inference`
- **Exception/CQ**: the analogy **begs the question** — the disputed conclusion (A and B are
  assimilable) is smuggled into the analogy's premise. This is **not** the same CQ as 839
  (differences undermining similarity) — it's a circularity defeater. The closest AIF native conflict
  node is a **question-begging / circularity** CQ.
- **Proposal**: `ExceptionRef=Analogy_Inference`, `DirectRef=` *(no clean AIF native circularity
  Conflict node exists for analogy)* → **FAIL LOUD**: document that no AIF native CQ cleanly
  captures "the analogy presupposes its conclusion"; propose `AIF_skosOther="Circularity:
  analogy presupposes the disputed assimilation (no native AIF Conflict node)"`,
  `MappingType=skos:closeMatch`. Do **not** fabricate a `*_Conflict` token.

### pk 841 — Analogie étendue (Extended/Transitive analogy)
- **desc_fr**: "À partir d'une chose qui ressemble à une deuxième et partage une propriété avec une
  troisième, vous concluez que la deuxième et la troisième se ressemblent."
- **Legitimate scheme**: `Analogy_Inference` (transitive chain: A~B, A~C ⟹ B~C)
- **Exception/CQ**: transitivity of similarity is not guaranteed — similarity is not transitive.
  This **is** a clean AIF conflict: the differences between B and C undermine the inferred similarity.
- **Proposal**: `ExceptionRef=Analogy_Inference`, `DirectRef=DifferencesUndermineSimilarity_Conflict`
  (same CQ as 839, applied to the B-C link rather than A-B), `MappingType=skos:narrowMatch` (a
  narrower, transitive-chain variant of 839).

### pk 842 — Argument de la similarité fallacieuse (Fallacious similarity)
- **desc_fr**: "Vous considérez que deux choses qui se ressemblent sont liées ou similaires."
- **Legitimate scheme**: `Analogy_Inference` (or more loosely `Sign_Inference` — resemblance as a
  sign of relatedness)
- **Exception/CQ**: superficial resemblance does not establish a real link/causal relation. The CQ:
  "is the resemblance merely superficial, without an underlying connection?" Closest AIF:
  `DifferencesUndermineSimilarity_Conflict` (the resemblance is too thin to transfer any property).
- **Proposal**: `ExceptionRef=Analogy_Inference`, `DirectRef=DifferencesUndermineSimilarity_Conflict`,
  `MappingType=skos:broadMatch` (the fallacy is broader — covers mere-resemblance-without-property-
  transfer, a looser case than 839's property-transfer analogy).

### pk 843 — Fausse équivalence (False equivalence)
- **desc_fr**: "Vous faites un parallèle impropre entre deux arguments distincts."
- **Legitimate scheme**: `Analogy_Inference` (two arguments treated as equivalent)
- **Exception/CQ**: the two arguments differ in a **relevant** respect (strength, structure,
  evidence) that defeats their equivalence. This is again `DifferencesUndermineSimilarity_Conflict`,
  but applied to **arguments** rather than objects — a well-known specialized case.
- **Proposal**: `ExceptionRef=Analogy_Inference`, `DirectRef=DifferencesUndermineSimilarity_Conflict`,
  `MappingType=skos:closeMatch` (a direct, argument-level variant of 839).

> **Note on pk 838** ("Distinction sans différence"): this leaf sits in the **Faulty comparison**
> sub-sub (`Soussousfamille=Faulty comparison`), **not** False analogy — see §1 boundary correction.
> It is the mirror fallacy (claiming a distinction where none exists) and honestly targets a
> **classification** scheme (`ArbitraryVerbalClassification_Inference` +
> `PropertyNotExistant_Conflict`) rather than `Analogy_Inference`. Its full modeling is deferred to
> **PR-2** (Faulty comparison cluster), to keep this PR scoped to a single coherent sub-sub.

---

## 4. Cluster summary (proposed)

| pk | text_fr | Proposed ExceptionRef | Proposed DirectRef | MappingType | Honest? |
|----|---------|----------------------|-------------------|-------------|---------|
| 839 | Fausse analogie | `Analogy_Inference` | `DifferencesUndermineSimilarity_Conflict` | `skos:closeMatch` | ✅ (existing anchor) |
| 840 | Pétition de principe analogique | `Analogy_Inference` | *(Other: circularity — no native CQ)* | `skos:closeMatch` | ⚠ FAIL LOUD (no native node) |
| 841 | Analogie étendue | `Analogy_Inference` | `DifferencesUndermineSimilarity_Conflict` | `skos:narrowMatch` | ✅ |
| 842 | Argument de la similarité fallacieuse | `Analogy_Inference` | `DifferencesUndermineSimilarity_Conflict` | `skos:broadMatch` | ✅ |
| 843 | Fausse équivalence | `Analogy_Inference` | `DifferencesUndermineSimilarity_Conflict` | `skos:closeMatch` | ✅ |

**4 leaves proposed (840-843), 1 FAIL-LOUD (840)** — documents that AIF has no native circularity-CQ
for analogy rather than fabricating one. All 4 leaves honestly reuse the anchor's
`Analogy_Inference` scheme; the only variation is the violated-CQ specialization (840) and the
`MappingType` granularity. The honest scheme-divergence case (pk 838, classification fallacy)
moves to **PR-2** (Faulty comparison cluster), keeping this PR a single coherent sub-sub.

---

## 5. Method notes (for the chantier's后续 PRs)

- **Cluster = a sub-sub-family with an in-sub-sub mapped anchor + its unmapped sibling leaves.**
  This PR's cluster (False analogy, anchor 839 + 4 leaves) is the unit of work per PR. Where a
  sub-sub has no in-sub-sub anchor (e.g. Faulty comparison 834-838), it is anchored by the parent
  d2/d3 node — that is PR-2's shape.
- **Reuse the anchor's scheme + CQ** where the leaf is a specialized violation of the same scheme;
  vary only the `MappingType` (`narrowMatch` = more specific, `broadMatch` = looser, `closeMatch` =
  direct variant).
- **Fail loud** when no native AIF Conflict node captures the leaf's defeater (840) — use
  `AIF_skosOther` to document, never fabricate a `*_Conflict` token.
- **Honest scheme-divergence** — when a leaf sits in a subfamily but actually targets a different
  scheme (pk 838: a comparison-subfamily leaf that is genuinely a *classification* fallacy), say so
  and use the right scheme; don't force the anchor's scheme. (838 lives in the Faulty comparison
  sub-sub → its modeling is in PR-2, not here.)
- **Coverage accounting** (#498 DoD): this PR adds **4 mapped leaves** to the 70 existing → 74/1408.
  At cluster-level, the False-analogy sub-sub goes from 1/5 mapped to 5/5. Effective cluster
  coverage of the comparison subfamily improves; leaf-level "100%" is explicitly **not** the goal.

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective ~31% cluster-level coverage documented (not "100% leaves") | ✅ §5 (70→74 mapped, cluster-level) |
| Per unmapped leaf: legitimate scheme + exception/CQ (not Latin alone) | ✅ §3 (4 leaves, scheme + CQ each) |
| `AIF_skosMappingType` coherent (broadMatch when narrower) | ✅ §4 (varied per leaf) |
| Fail-loud when no honest scheme fits | ✅ §3 pk 840 (no native circularity CQ — documented, not fabricated) |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies this proposition first, then CSV edits applied, then OWL regen (#133) — not in this PR |

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No #674/#596 merge, no régén launch.
- ✅ Proposition derived code=truth from the taxonomy CSV (cluster + AIF columns) + AIF scheme
  semantics (Walton/`Analogy_Inference` + `DifferencesUndermineSimilarity_Conflict` are AIF native).
- ✅ Fail-loud on 840 (no fabricated Conflict node); pk 838's honest scheme-divergence deferred to
  PR-2 (cluster-boundary correction §1 — 838 is in Faulty comparison, not this cluster).

Relates: dispatch `yj7u3j` (primary), #498 (chantier), #133/#130 (existing OWL), #499 (inverse:
virtue = good tenor of a scheme), #192 (terminology), #458.
