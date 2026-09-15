# #498 — Layer C Generative Pilot — DESIGN (gated decision artefact for jsboige)

**Worker** po-2024 · **Date** 2026-07-12 · **Base** master `b32a4d7b` · **Status** **GATED DESIGN / SCOPE-ONLY — 0 write prod, 0 mapping generated, `Cards/` untouched.** This document is the decision artefact jsboige needs to GO/NO-GO a generative tranche-2. It is **not** the execution.

> ⚠️ **Reading grid.** Producing this design respects the standing rule "Layer C non entamé sans GO jsboige": designing the decision is not executing the pass. No skos/attack mapping is generated here. The generative write awaits an explicit jsboige GO on the methodology + timing this document proposes.

**Issue:** [#498 — modéliser les sophismes comme exceptions à des schemes (AIF/Walton)](https://github.com/ArgumentumGames/Argumentum/issues/498).
**Prior art (cited, not duplicated):**
- [`498-aif-generative-pilot.md`](498-aif-generative-pilot.md) (po-2023, 2026-06-16) — the 18-fallacy exception-structure pilot (4 fields: `walton_scheme`, `violated_critical_question`, `exception_mechanism`, `justification`). Method validated gated; **this is the source of the generative technique Layer C would scale.**
- [`498-reconciliation-p1-closure.md`](498-reconciliation-p1-closure.md) (po-2024, 2026-07-12) — the P1 skos-only back-fill (93 → 145, 0 token fabricated), the **back-fill** track Layer C is **not**.
- PR #760 — AIF Phase 1-3 Execute MODE 2 (the **75 typed-no-skos leaves**, the existing generative precedent — see §3).

---

## 0. TL;DR (for the GO/NO-GO decision)

| Question | Answer |
|---|---|
| What is Layer C? | The **1259 fallacy leaves that carry neither a skos signature nor an attackType** (89.6 % of leaves). They need a **generative** pass (Walton scheme + CQ + derived attackType), as opposed to the P1 **back-fill** (skos already present, attackType derived). |
| How many, by family? | Influence 411 · Tricherie 379 · Insuffisance 163 · Obstruction 118 · Erreur mathématique 94 · Erreur de raisonnement 91 · Abus de langage 77 · Argument fallacieux 1. |
| Is there a precedent? | **Yes — 75 leaves already typed without skos** (Phase 1-3, PR #760), modelled cluster-by-cluster with gpt-5.5 assist + human review. Layer C generalizes that to the long tail. |
| What is the fabrication risk? | **Higher than P1.** P1 derived attackType from an existing skos; Layer C must **generate the skos itself**, so a scheme can be invented. Mitigation: **fail-loud if no native Walton CQ** exists for the fallacy (do not fabricate a scheme to type it). |
| Effort? | **Massive** (~24× P1's 52 rows). P1 took 7 tranches over ~2 days; Layer C is ~1259 rows. Realistic as a multi-week, multi-tranche programme, not a single PR. |
| **Recommended timing** | **AFTER the v0.9.0 tag.** v0.9.0 already ships a clean AIF milestone (145 attack-typed, P1 complete, OWL 3-layer). Layer C is experimental (generation), scope-heavy, and risks delaying the tag. It is a better fit for v1.0. |

---

## 1. Périmètre exact (empirique, code=truth)

Computed from `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` at master `b32a4d7b`:

| Set | Count | Definition |
|---|---:|---|
| Total rows | 1408 | — |
| Leaves (no children) | 1401 | `decimal_path` is not a prefix of any other row's |
| Leaves WITH skos signature | 67 | any of `AIF_skos{Direct,Exception,Other}Ref` non-empty |
| Leaves typed (attackType) | 142 | 67 skos + 75 no-skos (Phase 1-3) |
| **Layer C target** | **1259** | leaves with **no skos AND no attackType** |
| Typed non-leaf (ancestors depth-1) | 3 | pk 1 / 696 / 1280 (family roots) |

Leaf coverage matrix (the 4 quadrants):

```
                 has skos     no skos
  typed            67           75      = 142 leaf-typed (+3 ancestor = 145 prod)
  not typed         0         1259      ← Layer C
```

**0 leaves with skos are untyped** → P1 is fully closed. The entire residual is Layer C (no skos at all).

### Layer C by family

| Famille | Layer C leaves | Share |
|---|---:|---:|
| Influence | 411 | 32.6 % |
| Tricherie | 379 | 30.1 % |
| Insuffisance | 163 | 12.9 % |
| Obstruction | 118 | 9.4 % |
| Erreur mathématique | 94 | 7.5 % |
| Erreur de raisonnement | 91 | 7.2 % |
| Abus de langage | 77 | 6.1 % |
| Argument fallacieux | 1 | 0.1 % |
| **Total** | **1259** | **100 %** |

Influence + Tricherie alone = **62.7 %** of Layer C. A pilot would start with the densest, most textbook-classic subsets of these two families (where Walton mappings are least ambiguous).

---

## 2. Méthodologie générative proposée (extends the 2026-06 pilot)

Layer C is a **change of nature** from P1:

| | P1 (done) | Layer C (proposed) |
|---|---|---|
| Input | skos signature **already present** | **no skos** — must be generated |
| attackType derived from | own skos + `desc_fr` | **generated** Walton scheme + violated CQ |
| Risk | low (skos is evidence) | **high (scheme can be invented)** |
| Discipline | 0 token fabrication (token exists) | **fail-loud if no native Walton CQ** |

### 2.1 Two-stage derivation (skos first, then attackType)

**Stage A — Exception-structure generation** (the 2026-06 pilot method, scaled):
For each Layer C leaf, generate the 4 pilot fields via gpt-5.5 (`/v1/responses`, `reasoning.effort=low`) + human review:
- `walton_scheme` — the canonical Walton scheme this fallacy is an exception to (**must be an exact name from the ~20 Conflict + 41 Inference/Scheme catalog already referenced in the CSV**, verified by grep — no invented scheme name);
- `violated_critical_question` — the specific CQ the fallacy evades (FR, Walton style);
- `exception_mechanism` — how it derails the scheme (FR);
- `justification` — one line linking them (FR).

The `walton_scheme` is then written to `AIF_skosExceptionRef` (or `DirectRef` for the textbook-classic mappings), giving the leaf a **real skos signature** — promoting it from Layer C to the P1 input set.

**Stage B — attackType derivation** (deterministic, from the CQ):
Once the leaf has a skos signature, the attackType is derived by a **deterministic rule on the violated CQ** (not a second generation):

| The violated CQ concerns… | attackType | node |
|---|---|---|
| the **inference rule** (the step from premise to conclusion is invalid) | `undercut` | `RA-node` |
| the **premise** (acceptability of a premise is contested) | `undermine` | `I-node` |
| the **conclusion** (an independent counter-conclusion is offered) | `rebut` | `CA-node` |

This is the same ASPIC+ Option (a) map as P1 (#707 §4), but the input is the **generated CQ** instead of an existing skos. The 2026-06 pilot already encodes this implicitly (e.g., Appeal to Consequences → CQ on the conclusion → `rebut/CA`; False Analogy → CQ on the rule → `undercut/RA`).

### 2.2 Anti-fabrication guarantee — fail-loud

**If gpt-5.5 cannot map a fallacy to a native Walton CQ, the leaf is left untyped and flagged.** No scheme is invented to force a verdict. This is the hard guardrail:

- the ~20 `*_Conflict` + 41 `*_Inference`/`*_Scheme` names already in the CSV are the **closed catalog** (verified: 56 distinct AIF IRIs in the OWL);
- a generated `walton_scheme` **must** match one of these (string-equal after normalization);
- anything else → the leaf is reported as "no native CQ — needs editorial decision" and excluded from the tranche.

This bounds the fabrication risk to the **reviewer's** call on whether the CQ mapping is genuine, exactly as the 18-row pilot and the 75-row Phase 1-3 already required.

---

## 3. Existing precedent — the 75 Phase 1-3 typed-no-skos leaves

Layer C is **not unprecedented**. PR #760 (AIF Phase 1-3 Execute MODE 2, 2026-07-09) already typed **75 leaves without a skos signature** (46 Abus de langage, 8 Tricherie, 5 Insuffisance, 5 Erreur mathématique, 4 Influence, 4 Obstruction, 3 Erreur de raisonnement). These were modelled **cluster-by-cluster** with the generative technique (Walton scheme + CQ → attackType), validated by jsboige/ai-01 (MODE 2, with overrides 834/847 → undercut).

| | Phase 1-3 (#760) | Layer C (proposed) |
|---|---|---|
| Leaves typed without skos | 75 | 1259 (residual) |
| Method | cluster modelling + gpt-5.5 | same, scaled |
| Review | jsboige/ai-01 gated | same |
| Outcome | merged, 0 fabrication flagged | — |

**Layer C is the long tail of exactly the work Phase 1-3 started.** The methodology is de-risked by that precedent; what is undecided is the **scope/timing** (1259 rows), not the technique.

---

## 4. Verification protocol (per tranche, mirrors P1)

Each Layer C tranche (e.g., a family subset of ~50-100 leaves) would follow the P1 serialization contract:

1. **Proposition gated** — annotation CSV (the 4 pilot fields + derived `AIF_skosExceptionRef` + `AIF_attackType` + `AIF_attackedNode`) + markdown rationale. **0 write prod.**
2. **ai-01 review** — verdict on the CQ→attackType derivations, MED flags on ambiguous cases (e.g., a fallacy whose CQ could be read as premise-level or rule-level).
3. **GO WRITE** — apply script (byte-exact cell fill of the 3 AIF columns + the promoted `AIF_skosExceptionRef`), disjoint PKs, baseline dynamic.
4. **Byte-exact 2× verify** — self-proof (0 mismatch) + independent backup-vs-prod (N PKs × 4 cols now, since the skos cell is also filled; 0 stray, 0 value mismatch).
5. **Catalog match check** — every generated `walton_scheme` ∈ the 56-IRI closed catalog (no invented scheme).

The tranche size is **smaller** than P1's (~50-100 vs ~7-16) because each row requires generated+reviewed content, not just a derived verdict.

---

## 5. Frontière gated (what this design does NOT authorize)

- ❌ **No mapping generated** by this document — Stage A/B are described, not executed.
- ❌ **No `Cards/` write** — `git diff b32a4d7b -- Cards/…/Taxonomy.csv` empty.
- ❌ **No Layer C leaf typed** until jsboige GOs the methodology + a first concrete tranche proposition.
- ✅ The first concrete output (after GO) would be a **single-family pilot tranche proposition** (e.g., ~30 Influence leaves), gated for ai-01 review — same shape as P1 tranche-1.

---

## 6. Effort estimation + fabrication risk

### Effort
- **1259 leaves**, at a tranche size of ~50-100 (constrained by the per-row generation+review cost).
- ≈ **13-25 tranches**, each ~1 worker-tick of proposition + ai-01 review + GO write.
- At the P1 cadence (7 tranches / ~2 days), Layer C is a **multi-week programme** (realistically 3-6 weeks of worker time, gating on review latency).
- gpt-5.5 cost: ~1259 calls × (Stage A generation), modest token-wise (the key resource is **human review bandwidth**, not API budget).

### Fabrication risk
- **Inherent to generation.** Unlike P1 (skos = evidence), Layer C generates the skos. A scheme could be invented or a CQ mis-attributed.
- **Mitigations:** (a) fail-loud on no-native-CQ (§2.2); (b) closed 56-IRI catalog match (§4 step 5); (c) per-row `justification` field surfacing the reviewer's reasoning; (d) ai-01 gated review per tranche with MED flags on ambiguous CQ→node mappings.
- **Residual risk:** a systematic mis-modelling (e.g., gpt-5.5 over-derives `undercut` for all Influence fallacies) would need a calibration pass — detectable on the first 1-2 tranches.

---

## 7. Recommended timing — AFTER v0.9.0 tag

**Recommendation: defer Layer C to post-v0.9.0 (target v1.0).**

1. **v0.9.0 already ships a clean AIF milestone** — 145 attack-typed (P1 complete), OWL 3-layer (#787), release notes + README (#789). Adding Layer C mid-release risks destabilizing that.
2. **Layer C is experimental** (generation, not back-fill). The tag should capture the stable, review-validated state, not an in-flight generative programme.
3. **Scope is massive** (1259 rows). Bundling it with v0.9.0 would delay the tag (critical path = jsboige visual GO, week of 2026-07-13) for no release-note value the user can perceive.
4. **v1.0 is the natural vehicle** for an ontology-extension feature; v0.9.0 is the "8 languages + pipeline recovery + print-ready" release.

If jsboige wants a **preview** in v0.9.0, the low-risk option is a **single pilot tranche** (one family, ~30 leaves) executed before the tag as a demonstrated capability — but the default recommendation is full deferral.

---

## 8. Decision needed from jsboige

| # | Decision | Options |
|---|---|---|
| 1 | **GO/NO-GO Layer C generative pass** | GO (post-tag v1.0 target) / NO-GO (stop at P1) / pilot-only (1 tranche in v0.9.0) |
| 2 | **Timing** | After v0.9.0 tag (recommended) / before (risky) |
| 3 | **Tranche size** | ~50 leaves (conservative) / ~100 (aggressive) |
| 4 | **Catalog policy** | closed 56-IRI (recommended) / allow new scheme names with jsboige veto |
| 5 | **First family** | Influence (densest, 411) / Tricherie (379) / a smaller family to calibrate (Abus de langage 77, already 46 Phase-1-3 typed) |

---

## 9. Gate boundaries (this design artefact)

- ✅ **0 write prod CSV** — `Cards/` untouched (`git diff b32a4d7b -- Cards/…/Taxonomy.csv` empty).
- ✅ **0 mapping generated** — Stage A/B described, not executed; no `walton_scheme`/CQ authored here.
- ✅ Périmètre empirique (1259 leaves, by family) computed from prod CSV, not estimated.
- ✅ Prior art cited (2026-06 pilot, Phase 1-3 #760), not duplicated.
- ❌ No Layer C execution without explicit jsboige GO on §8.
- ❌ Verdict QA = ai-01 for any future tranche; this design is a decision input, not a review verdict.

🤖 Worker po-2024 — Layer C pilot DESIGN (gated scope artefact for jsboige GO/NO-GO), 0 mapping generated.
