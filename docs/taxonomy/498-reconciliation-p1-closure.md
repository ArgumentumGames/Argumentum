# #498 AIF Reconciliation — P1 (skos-only) Closure Report

**Worker** po-2024 · **Date** 2026-07-12 · **Base** master `474607c9` · **Status** ✅ **P1 COMPLETE** (93 → 145 attack-typed, 0 skos-only residual). GO ai-01 `gc40e5` (write 1g) + dispatch `ti19qu` (closure doc).

> This document closes the **P1 reconciliation phase** of #498: back-filling the two AIF attack columns (`AIF_attackType`, `AIF_attackedNode`) for the 52 rows that carried a skos signature but had no attackType. It consolidates the methodology, the tiering, and the per-tranche serialization record for release notes and #133 OWL publication.

---

## 0. TL;DR

- **52 skos-only rows back-filled** → attack-typed total **93 → 145** (10.3% of 1408 leaves).
- **0 residual**: rows with skos but no attackType = **0**.
- **0 fabrication token** across all of P1 — every typed row derived from its own skos signature + `desc_fr`.
- **Deterministic node map** #707 §4 Option (a): `undercut→RA-node` (rule), `undermine→I-node` (premise), `rebut→CA-node` (conclusion). **0 node/type inconsistency** in prod.
- Final prod distribution: **87 undercut / 53 undermine / 5 rebut** (rebut is a structural tail case ~3%, localized to appeal-to-consequences).

---

## 1. Tiering methodology (tier-par-confiance > ordre-famille strict)

The 52 skos-only rows are not homogeneous. They were tiered by empirical confidence rather than by family order:

| Tier | Count | Definition | Serialization |
|---|---:|---|---|
| **PRECEDENT** | 14 | Has an exact-token precedent already typed in-set | tranche-1 (#771, 93→107) |
| **PREC-TIE** | 2 | Competing precedents; tie broken by majority + desc | tranche-1b (#776, 107→114) |
| **SUFFIX-ONLY** | 36 | skos token present but no typed precedent (novel or semantic parallel) | 1b+1c+1d+1e+1f+1g |
| **TOTAL** | **52** | | **93 → 145** |

**Key principle (memory `aif-no-inherit`)**: `attackType` is a **fresh judgment** derived from the row's own skos signature + `desc_fr` ("what does the Walton CQ defeat"). It is **never inherited from the anchor** — anchor audit (#770) showed 0/19 fully-modeled anchors align with their sub-sub-leaves on attackType. Inheriting would be fabrication.

---

## 2. Serialization record (7 tranches)

| Tranche | Family | Rows | Distribution | PR | Δ |
|---|---|---:|---|---|---|
| **tranche-1** | PRECEDENT (cross-family) | 14 | 9 undercut / 4 undermine / 1 rebut | #771 (write), #769 (prop) | 93 → 107 |
| **tranche-1b** | PREC-TIE + SUFFIX (2 + 5) | 7 | 4 undermine / 2 undercut / 1 rebut | #776 (write), #773 (prop) | 107 → 114 |
| **tranche-1c** | Erreur de raisonnement | 7 | 7 undercut (inference-uniform) | #779 (write), #775 (prop) | 114 → 121 |
| **tranche-1d** | Erreur mathématique | 4 | 4 undercut (inference-uniform) | #783 (batched write), #778 (prop) | 121 → 125 |
| **tranche-1e** | Insuffisance | 5 | 5 undercut (inference-uniform) | #783 (batched write), #780 (prop) | 125 → 130 |
| **tranche-1f** | Abus de langage | 7 | 6 undermine / 1 undercut (MIXTE) | #783 (batched write), #781 (prop) | 130 → 137 |
| **tranche-1g** | Tricherie (LAST FAMILY) | 8 | 3 undermine / 5 undercut (MIXTE) | #785 (write), #784 (prop) | 137 → 145 |
| **TOTAL** | | **52** | 87 undercut / 53 undermine / 5 rebut | | **+52** |

Every write was **byte-exact 2× verified**: self-proof (0 byte-preservation mismatch, 104 cols, CRLF+BOM) + independent backup-vs-prod (exactly N PKs changed in `AIF_attackType`/`AIF_attackedNode` only, 0 stray cells, empty→value).

---

## 3. Defect-nature modeling (Walton CQ → ASPIC+ node)

The 3-family MIXTE tranches (1f, 1g) surfaced three distinct defect natures, each mapping deterministically to a node:

| Defect nature | What is attacked | Node | Example |
|---|---|---|---|
| **Relational manipulation** (deceptive arrangement, goalpost-moving, biased reasoning) | the **premise** (acceptability contested) | `undermine / I-node` | 888, 973, 1023 (1g); 800, 814, 833 (1f) |
| **Sunk-cost / effort-justification** | the **inference rule** (practical-reasoning/waste) | `undercut / RA-node` | 1020, 1148 (Waste_Inference pk432) |
| **Weak induction** (mere exposure, anecdotal, attribution) | the **inference** (induction too weak) | `undercut / RA-node` | 1066, 1087, 1175 |

**Rebut rarity (memory `rebut-rarity`)**: relational fallacies (personal attack, genetic fallacy, moving goalposts) are **undermine/undercut**, not rebut — they reject without presenting an independent counter-conclusion. Rebut is structurally localized to appeal-to-consequences (5 rows, ~3%).

---

## 4. Same-token divergence principle (desc-driven, token non-décisif)

Two occurrences tested and confirmed the principle that **the desc drives the verdict, not the token**:

| Pair | Shared token | Divergent verdicts | Justification |
|---|---|---|---|
| **pk808 / pk33** (tranche-1f) | `AlternativeMeans_Conflict` | pk33→undercut, pk808→undermine | pk33 desc = inferential; pk808 desc = definitional |
| **pk888 / pk2** (tranche-1g) | `LackOfPTKReliability_Scheme` | pk2→undercut, pk888→undermine | pk2 desc = weak induction; pk888 desc = deceptive arrangement (premise), reinforced by `PositionToKnow_Inference`→undermine (pk70) |

ai-01 concurred on both. This validates the core anti-fabrication discipline: a token does not mechanically determine a verdict; the desc_fr does.

---

## 5. Anchor audit foundation (#770)

The reconciliation rests on the anchor audit (#770, merged):
- **18 fully-modeled anchors audited**: 16 CLEAN / 2 SOFT / 0 error.
- **Suffix plurality prior rejected**: pk804 (`_Conflict` suffix → undercut) proves the suffix is non-decisive. The suffix prior is **proscribed**.
- **0/19 anchors align with their sub-sub-leaves** on attackType → inheritance = fabrication, confirmed empirically.

---

## 6. Coverage after P1

| Metric | Value |
|---|---:|
| Fallacies leaves total | 1408 |
| attack-typed (prod) | **145** (10.3%) |
| — `undercut / RA-node` | 87 |
| — `undermine / I-node` | 53 |
| — `rebut / CA-node` | 5 |
| skos-only residual | **0** |
| crossLink relations (#763) | 1985 emitted, carried by 844 leaves (**59.9%** of 1408) |
| skos Walton mappings | 70 |

The attack layer now covers **all 52 rows that previously had skos without attackType**. The remaining ~1263 untyped leaves are **Layer C** (no skos at all) — see §7.

---

## 7. Out of scope — Layer C (generative pass)

The ~1263 remaining leaves have **no skos signature** and require a **generative** pass (Walton CQ mapping ex-nihilo) to receive an attackType. This is a **change of nature** from the P1 back-fill (generation vs back-fill → real fabrication risk) and is **out of scope for the P1 tranche**. It has been escalated to `jsboige` by ai-01 as a scope + timing decision (own gated pilot, before/after v0.9.0 tag). **Not started without a dedicated GO.**

---

## 8. Release notes feed

For the v0.9.0 release notes and #133 OWL publication:
- **AIF attack layer** reached **145 fallacies** with deterministic ASPIC+ node mapping (Option a);
- **0 fabrication token** — every typed fallacy derived from its own evidence;
- **3-layer relational ontology** now fully serialized in `docs/ontology/argumentum.owl` (skos 70 + crossLink 1985 + AIF attack 145);
- **Methodology reproducible** via the tier-par-confiance framework documented here.

---

## 9. PRs merged (P1)

| PR | Title |
|---|---|
| #769 | P1 tranche-1 proposition (PRECEDENT, 14) |
| #770 | Audit 18 anchors (16 CLEAN / 2 SOFT) |
| #771 | P1 tranche-1 write prod (93→107) |
| #773 | tranche-1b proposition (2 PREC-TIE + 5 SUFFIX) |
| #775 | tranche-1c proposition (Erreur raisonnement) |
| #776 | tranche-1b write prod (107→114) |
| #778 | tranche-1d proposition (Erreur mathématique) |
| #779 | tranche-1c write prod (114→121) |
| #780 | tranche-1e proposition (Insuffisance) |
| #781 | tranche-1f proposition (Abus de langage MIXTE) |
| #783 | batched write 1d+1e+1f (121→137) |
| #784 | tranche-1g proposition (Tricherie MIXTE, LAST) |
| #785 | tranche-1g write prod (137→145) |

🤖 Worker po-2024 — P1 skos-only 100% complete, closure documented.
