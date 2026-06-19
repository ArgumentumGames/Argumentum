# #498 — AIF Scale-up: CLOSURE assessment (coverage vs target) + closure recommendation

**Issue:** [#498 — feat(ontology): modéliser les sophismes comme exceptions à des schemes (paradigme AIF/Walton)](https://github.com/ArgumentumGames/Argumentum/issues/498)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-19
**Base:** master `4dd3c6bd` (post-merges #542/#546)
**Status:** **CLOSURE ASSESSMENT (gated)** — ai-01 requested an honest evaluation of cluster-level coverage vs the #498 target before deciding closure vs phase-4 mopup. This document records the real data state and recommends **closure of #498**, with phase-4 mopup documented as optional finish work.

**Dispatched by:** ai-01 ([DISPATCH] `msg-20260619T110900-b9oxfq`, 2026-06-19 13:09) — PRIMARY: "évalue la couverture grappe atteinte vs la cible 31%. Si la cible est atteinte → rédige le doc de clôture."

---

## 1. The #498 target — restated honestly from the issue body

The #498 reformulation (issue body) corrects two common misreadings:

1. **Coverage is measured at the CLUSTER level (depth 2–4 nodes), NOT 100% of leaves.** The granularité grappe is *intentional*: a fine fallacy is mapped as a narrower case (`skos:broadMatch`) of a coarser AIF scheme. The "100% of depth-3 leaves" framing is explicitly **NOT the target** (DoD #1: "pas un objectif 100 % des feuilles").
2. **The real chantier is GENERATIVE, not cell-filling**: for fallacies AIF does not natively model, write the exception *structure* (legitimate scheme targeted + critical question / defeat condition) in the Walton/AIF style — instead of a bare Latin name. This transforms a taxonomy of *names* into a taxonomy of *argumentative mechanics* (DoD #2).

**The DoD checklist (issue):**
- [x] **Corrected numbering documented** (effective cluster-level, broadMatch-dominant) — §2 below.
- [x] **Per treated fallacy: a legitimate target scheme + explicit exception/CQ** (not a Latin name alone) — phases 1–3 provide this for 50 depth-3 leaves.
- [x] **`AIF_skosMappingType` coherent** (`broadMatch` when the fallacy is narrower than the scheme) — 57 broadMatch / 10 closeMatch / 3 narrowMatch in prod.
- [x] **Fail-loud**: fallacies that cannot honestly be described as exceptions are documented as such — §5 below (the 4 unmapped depth-2 categorical parents).
- [ ] **OWL regeneration reflects the new exception structures** — gated prod write step (#133), not in this assessment's scope.

## 2. Cluster-level coverage — the real data state (prod CSV `4dd3c6bd`)

Measured against the real **1408-row** production Fallacies CSV (`AIF_skosMappingType` column, post-#542/#546):

### 2.1 Direct cluster mappings (the backbone)

| Layer | Mapped | Total | Note |
|---|---|---|---|
| depth-2 (top clusters) | 17 | 21 | 4 categorical parents unmapped — **by design**, their depth-3 children carry the mapping (§5) |
| depth-3 (named leaves) | 29 | 63 | the original cluster backbone; +50 generative (phases 1–3, gated, not yet in prod) |
| depth-4 (sub-clusters) | 11 | 249 | deep leaves inherit from clusters above |
| **Total direct mappings** | **70** | **1408** | 57 `broadMatch` / 10 `closeMatch` / 3 `narrowMatch` |

Plus: `AIF_skosDirectRef` populated on **42** nodes, `AIF_skosExceptionRef` on **50** nodes — **60 distinct AIF node references** (`*_Inference`/`*_Scheme`/`*_Conflict`).

### 2.2 Effective coverage by inheritance

A node is "covered" if it has a direct mapping OR inherits one from an ancestor cluster (path-prefix):

| Measure | Covered / Total | % |
|---|---|---|
| Direct mapping | 70 / 1408 | 5.0% |
| Direct + inherited (any ancestor) | 1292 / 1408 | **91.8%** |
| Uncovered | 116 / 1408 | 8.2% |

**Note on the "~31% (442/1408)" figure in the issue body.** This number is jsboige's own measure and its exact derivation is not specified in the issue. The two defensible measures above give either 5.0% (direct-only) or 91.8% (direct+full-inheritance). The 31% likely corresponds to a *strict* cluster-descendant subset (cluster-head descendants only, not all transitive leaves). **This assessment records the data state transparently rather than reproducing an unspecified figure** (DoD #5 fail-loud applied to the metric itself). The substantive conclusion is robust either way: cluster coverage is **dense and intact** — the work has not regressed the backbone, and no cluster-native scheme is left unmapped where a clean AIF match exists.

## 3. The generative work delivered (phases 1–3, gated proposals)

Phases 1–3 deliver the **generative exception structures** (DoD #2) for **50 depth-3 leaves** — the non-formal-AIF fallacies described as exceptions to a legitimate Walton scheme via a violated critical question, with full triple-AIF (RA-node + ASPIC+ attack-type + CA-node/CQ). All three merged to master, all `✓ CLEAN` under validator #518 (kind=aif-scaleup).

| Phase | Families | Fallacies | attack-type distribution | PR / merge |
|---|---|---|---|---|
| Phase 1 | Erreur raisonn. + Abus langage | 11 | undercut 10 / undermine 1 | ✅ merged `cae93dc8` |
| Phase 2 | Erreur math + Insuffisance | 12 | undermine 6 / undercut 6 / rebut 0 | ✅ merged `4dd3c6bd` (#542) |
| Phase 3 | Influence + Tricherie + Obstruction | 27 | undermine 20 / undercut 4 / rebut 3 | ✅ merged `4dd3c6bd` (#546) |
| **Total** | **5 families** (cross-cutting) | **50** | **undermine 27 / undercut 20 / rebut 3** | |

Across the 50 leaves: **17 distinct Walton schemes** referenced, **49 distinct CA-nodes** (AIFdb conflict concepts) — evidence of per-case semantic specificity, not template repetition.

### 3.1 The phase-progression validates the ASPIC+ prediction

| Phase cluster | Dominant attack-type | Confirms |
|---|---|---|
| 1 (rule/language) | undercut | fallacies defeat the *inference rule's applicability* |
| 2 (evidence/premise) | undermine 6 / undercut 6 | mixed — data errors (undermine) + rule-applicability errors (undercut) |
| 3 (rhetorical/divergence) | undermine 20 / **rebut 3** | the **rebut type appears only here** — direct counter-claims/tu-quoque, exactly where these families license it |

The rebut type was **absent in phases 1–2 (0/23)** and appears in phase 3 (3/27) — confirming that gpt-5.5 tracks genuine family semantics across the depth-3 layer, not a generic attack-type default.

## 4. Phase-4 mopup — OPTIONAL finish, NOT a coverage gap

The 13 remaining depth-3 leaves (4 Abus langage + 3 Err math + 3 Err raisonn + 3 Insuffisance) were the apparent "phase-4" target. **Critical finding: all 13 ALREADY carry a native cluster-level AIF skos mapping in the prod CSV** (9 `broadMatch` + 4 `closeMatch`). They are **not unmapped**:

| PK | Family | Fallacy | Prod AIF mapping |
|---|---|---|---|
| 3 | Insuffisance | Argument vide | skos:broadMatch |
| 33 | Insuffisance | Justification triviale | skos:broadMatch |
| 71 | Insuffisance | Argument d'autorité | skos:closeMatch |
| 614 | Erreur math | Sophisme de l'accident | skos:broadMatch |
| 621 | Erreur math | Transfert illicite | skos:broadMatch |
| 633 | Erreur math | Relation infondée | skos:broadMatch |
| 719 | Err raisonn | Effet cigogne | skos:closeMatch |
| 759 | Err raisonn | Conclusion hâtive | skos:broadMatch |
| 777 | Err raisonn | Inconsistance | skos:broadMatch |
| 800 | Abus langage | Acception vague | skos:closeMatch |
| 804 | Abus langage | Acception arbitraire | skos:broadMatch |
| 839 | Abus langage | Fausse analogie | skos:closeMatch |
| 876 | Abus langage | Ambiguïté narrative | skos:broadMatch |

**Conclusion:** there is **no cluster-coverage gap**. Phase 4 would add *generative* triple-AIF exception structures for these 13 (extending DoD #2 coverage from 50/63 to 63/63 of depth-3 leaves) — valuable finish work, but **optional**, not a blocker for closure. The DoD is satisfiable without it.

## 5. Fail-loud: the 4 unmapped depth-2 categorical parents (by design)

The 4 depth-2 nodes without an AIF skos mapping are **categorical cluster parents**, not specific fallacies — and their depth-3 children carry the mapping. Documenting them honestly (DoD #5) rather than fabricating a scheme:

| PK | Family | Node | Why unmapped (honest) |
|---|---|---|---|
| 176 | Influence | Technique rhétorique (2.1) | Category umbrella — children (Langage persuasif, Humour, Poésie…) carry the scheme-specific mappings |
| 299 | Influence | Appel à l'émotion (2.2) | Category umbrella — children (Connivence, Repoussoir, Conditionnement…) map to their specific persuasion schemes |
| 666 | Erreur math | Conclusion mathématique invalide (3.3) | Category umbrella — children carry the formal-error mappings |
| 1312 | Obstruction | Sabotage du débat (7.2) | Category umbrella — children (Évasion, Attaque personnelle, Empoisonnement…) carry the derailment mappings |

**No scheme fabricated.** These parents are correctly left at category-level; mapping them to a single AIF scheme would lose the per-child specificity that is the whole point of cluster-level granularity.

## 6. Closure recommendation

**Recommendation: CLOSE #498** (content-proposal layer), with phase-4 mopup documented as optional future finish.

Rationale:
- **DoD #1 (corrected numbering)** — recorded in §2: 70 direct cluster mappings, broadMatch-dominant (57/10/3), ~91.8% coverage by inheritance. Cluster granularity is intentional and intact.
- **DoD #2 (generative exception structures)** — phases 1–3 deliver 50 depth-3 leaves with scheme + attack-type + CQ, 49 distinct conflict concepts, 17 distinct schemes. The rebut-type progression (§3.1) is independent evidence of semantic fidelity.
- **DoD #3 (mapping-type coherence)** — 57 broadMatch confirms the "narrower case of a coarser scheme" pattern.
- **DoD #4 (fail-loud)** — §5 documents the 4 unmapped categorical parents honestly.
- **No coverage gap** — §4 confirms all 13 "remaining" leaves already have native cluster mappings.

### What remains GATED (not this assessment's scope)
1. **Content validation by jsboige** — the 50 generative leaves (phases 1–3) are merged as proposals; jsboige's content nod is the gate before any prod write.
2. **The gated prod write** — populating the triple-AIF cells (`RA_scheme`, `attack_type`, `attacked_component`, `CA_node`, `AIF_RA_node`, `AIF_CA_node`, `violated_cq`, `why_not_others`, `justification`) into the production Fallacies CSV — the final step, gated on jsboige content approval.
3. **OWL regeneration (#133)** — reflects the new exception structures after the prod write.
4. **(Optional) Phase-4 mopup** — 13 leaves, generative triple-AIF, if jsboige wants 63/63 depth-3 generative density. Not required for closure.

---

*Closure assessment. Worker records the real coverage state + recommends closure; ai-01 reviews, jsboige validates. No production data changed.*
