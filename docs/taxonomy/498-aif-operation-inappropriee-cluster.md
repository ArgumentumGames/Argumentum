# 2026-07-22 — #498 AIF chantier : cluster « Opération inappropriée » (Erreur mathématique) — **second mixed cluster (math-inference errors)**

**Scope** : dedicated cluster doc for the **Opération inappropriée** sub-sub (family **Erreur mathématique**,
sub-family **Résultat invalide**) — **5 unmapped leaves** (691-695) under the anchor **690** (already serialized
`undercut`/`RA-node`, attack-columns-only). **Proposition only — GATED, 0 write to prod CSV.** Continues the
cluster-docs authoring backlog (ai-01 dispatch `msg-9akcg4` strate-6 deep-queue, post-T&A regime). Second mixed
undercut/undermine cluster after #837 (Complication exagérée).

**Repo reference** : master `3a87eb0e`. Issue : #498. Predecessors : #770 (anchor audit), #837 (first mixed
cluster — Complication exagérée, MERGED `3a87eb0e`), #760 (rebut cluster — Relativisme abusif). Layer C =
**1 263 unmapped leaves** remain (this doc opens 5 more of them).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies → worker applies
> the CSV edits in a follow-up PR (gated), same flow as #753/#760/#837. Serialization to the 2 attack columns
> is deferred to the gated apply.

---

## TL;DR

- **Opération inappropriée** (Erreur mathématique > Résultat invalide, sub-sub, 6 rows) : anchor **690** already
  serialized `undercut`/`RA-node` (attack-columns-only, no skos) ; **5 unmapped leaves** — 691 Sophisme
  mathématique, 692 Division par zéro, 693 Pseudo démonstration d'égalité, 694 Détournement de symbole,
  695 Pseudo-science.
- **Second mixed cluster (after #837).** The umbrella « invalid mathematical operation » forks into two honest
  sub-mechanisms :
  - **Math-inference corruption** (691, 692, 693, 694) — a hidden calculation error / division-by-zero / masked
    error / symbol equivocation *invalidates the inference step* (the RA-node / rule-application) → **undercut /
    RA-node** (matches the anchor 690).
  - **Epistemic-premise fabrication** (695) — pseudo-science *fabricates the premise* of scientific status (an
    I-node) → **undermine / I-node** (genuine divergence from the anchor — odd-one-out of the cluster, an
    epistemic-misrepresentation move rather than an operational one).
- **All 5 are attack-columns-only (fail-loud on skos).** No native Walton scheme honestly fits « hidden
  calculation error », « division by zero », « symbol equivocation », or « fabricated scientific status » —
  these are not Walton argument schemes. Consistent with the anchor 690 (also attack-columns-only). 0 fabrication
  (#677).
- **5 attack-typed leaves proposed, 0 fabrication, axiom 0 violation** (4 undercut→RA + 1 undermine→I).
  Projected coverage **145 → 150 / 1 408** once applied (cluster run on master `3a87eb0e` rescan = 145/1 408,
  10.3 %).

---

## 1. Cluster state (code=truth, master `3a87eb0e`)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Erreur mathématique**, sub-family
**Résultat invalide**, sub-sub **Opération inappropriée** (6 rows) :

| pk | text_fr | depth | AIF_attackType | AIF_attackedNode | skos cols | State |
|----|---------|-------|----------------|------------------|-----------|-------|
| **690** | **Opération inappropriée** (anchor) | d3 | `undercut` | `RA-node` | *(all empty)* | **✅ serialized** (attack-columns-only) |
| 691 | Sophisme mathématique | d4 | — | — | — | unmapped (**this cluster**) |
| 692 | Division par zéro | d5 | — | — | — | unmapped (**this cluster**) |
| 693 | Pseudo démonstration d'égalité | d5 | — | — | — | unmapped (**this cluster**) |
| 694 | Détournement de symbole | d4 | — | — | — | unmapped (**this cluster**) |
| 695 | Pseudo-science | d4 | — | — | — | unmapped (**this cluster**) |

**Leaf content (code=truth `desc_fr` / `example_fr`)** :
- **690 Opération inappropriée** — « Vous utilisez un type de raisonnement mathématique qui n'est pas valide
  dans cette situation. » / « chaque fois que le lièvre avance, la tortue avance aussi ; cela empêcherait donc
  le lièvre de combler l'écart »
- **691 Sophisme mathématique** — « Vous cachez ou ignorez une erreur dans votre calcul pour appuyer un
  argument incorrect. » / « J'ai divisé toutes les quantités par deux… ainsi que le temps de cuisson.
  Résultat : le gâteau est… »
- **692 Division par zéro** — « Vous utilisez un raisonnement qui implique, de manière cachée, une division par
  zéro. » / « En simplifiant les deux membres de cette égalité par leur facteur commun, nous avons donc
  démontré que 1 = 2. »
- **693 Pseudo démonstration d'égalité** — « Vous prétendez à tort que deux nombres différents sont égaux en
  masquant une erreur de calcul. » / « multiplier un nombre par un million revient au même que le diviser par… »
- **694 Détournement de symbole** — « Vous utilisez un symbole mathématique dans un sens autre que celui qui est
  établi pour induire en erreur. » / « utilise le symbole '%' pour signifier 'pour cent'… mais subtilement
  l'utilise comme un… »
- **695 Pseudo-science** — « Vous faites passer des théories non fondées pour des faits scientifiques. » /
  « disposer les meubles selon les principes du feng shui améliorera votre santé… car cette méthode… »

The sub-sub is **semantically coherent around one umbrella** — *invalid mathematical operation* — but that
umbrella forks into two distinct AIF targets (inference corruption vs premise fabrication), which is why this is
a **mixed** cluster (like #837). §3 derives each leaf's target honestly.

---

## 2. The anchor model (690, serialized) — attack-columns-only

`690` is serialized `AIF_attackType=undercut`, `AIF_attackedNode=RA-node`, **all skos relational columns empty**.
This is #677-honest : the anchor models an *invalid mathematical operation* — it attacks the **rule-application**
(RA-node) by using a type of reasoning that does not validly apply in the situation. There is **no native Walton
CQ-conflict token** for « inappropriate mathematical operation ». The load-bearing modelling is the 2 attack
columns ; the skos layer is honestly empty.

**Native math-family token inventory (code=truth)** : `EvidenceToHypothesis_Inference` (×2),
`PropertyNotExistant_Conflict` (×2), `Example_Inference`, `InductiveInference_Scheme`, `ExceptionalCase_Inference`,
`Sign_Inference`, `Gradualism_Inference`, `WeakestLink_Conflict`, plus 3 slippery-slope variants. **None of these
honestly fits** « hidden calculation error », « division by zero », « symbol equivocation », or « fabricated
scientific status » — these are operational/epistemic defects, not Walton argument schemes. All 5 leaves are
therefore attack-columns-only (fail-loud), consistent with the anchor.

---

## 3. Proposed AIF structure for the leaves

Per-leaf derivation. `attackType` + `attackedNode` follow the AIF/ASPIC+ axiom (undercut→RA, undermine→I,
rebut→CA — **0 violation**). `skosDirectRef` / `skosExceptionRef` stay **empty** for all 5 (no honest native
fit) with a fail-loud note. **0 fabrication (#677).**

### Sub-mechanism A — math-inference corruption → **undercut / RA-node** (matches anchor)

#### pk 691 — Sophisme mathématique (hidden calculation error) ✅ undercut
- **Mechanism** : hides or ignores an error in the calculation that underpins the argument. The error corrupts
  the inference *step* (the calculation is the rule-application) → the RA-node is attacked.
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef` **empty**, `ExceptionRef` **empty**,
  fail-loud note in `AIF_skosOther`, `MappingType=skos:narrowMatch` (d4 specialization).
- **Why not undermine** : the factual premise is not contested — the *calculation itself* is corrupted. Why no
  scheme : no native Walton token for « hidden calculation error ».

#### pk 692 — Division par zéro (hidden division by zero) ✅ undercut
- **Mechanism** : a hidden division by zero breaks the algebraic step (the classic 1=2 « proof »). The rule-
  application (the simplification step) is invalid → RA-node attacked. Textbook ASPIC+ undercut of an algebraic
  inference.
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef` **empty**, `ExceptionRef` **empty**,
  fail-loud note, `MappingType=skos:narrowMatch` (d5).
- **Why not undermine** : no premise contested — the algebraic *operation* is invalid. Why no scheme : no native
  token for « division by zero ». Cleanest ratification test case in the cluster (the 1=2 inference demonstrably
  fails at the simplification step).

#### pk 693 — Pseudo démonstration d'égalité (masked error) ✅ undercut
- **Mechanism** : masks a calculation error to falsely « prove » two different numbers are equal. Same as 691/692
  — a hidden error invalidates the inference step (RA-node).
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef` **empty**, `ExceptionRef` **empty**,
  fail-loud note, `MappingType=skos:narrowMatch` (d5).
- **Why not undermine** : the calculation is corrupted, not a premise. Why no scheme : no native token for
  « masked calculation error ».

#### pk 694 — Détournement de symbole (symbol equivocation) ⚠ undercut (boundary: undermine)
- **Mechanism** : uses a mathematical symbol in a non-established sense to mislead. The equivocation breaks the
  rule-application (the inference assumes a univocal symbol meaning) → RA-node attacked.
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef` **empty**, `ExceptionRef` **empty**,
  fail-loud note, `MappingType=skos:narrowMatch` (d4).
- **Documented boundary** : close to an *undermine* (the symbol's meaning, an information premise, is corrupted),
  but the charge breaks the inference step that relies on a fixed meaning, so **undercut**. Flagged for
  ai-01/jsboige ratification. Why no scheme : no native Walton token for « mathematical symbol equivocation ».

### Sub-mechanism B — epistemic-premise fabrication → **undermine / I-node** (diverges from anchor)

#### pk 695 — Pseudo-science (fabricated scientific status) ⚠ undermine (diverges from anchor)
- **Mechanism** : passes unfounded theories off as scientific facts. The attack is on the **premise** — the
  scientific status presented as established (an I-node), not on the inference operation. This is an *epistemic
  misrepresentation*, not an operational defect — the **odd-one-out** of the cluster.
- **Proposal** : `attackType=undermine`, `attackedNode=I-node`, `DirectRef` **empty**, `ExceptionRef` **empty**,
  fail-loud note, `MappingType=skos:narrowMatch` (d4).
- **Why undermine (honest divergence from the anchor)** : the anchor (690) attacks the *operation* (RA-node) ;
  695 attacks a *premise* (the false scientific status = I-node). Per-leaf derivation, NOT anchor inheritance
  (`[[aif-no-inherit-attacktype-from-anchor]]`). Why no scheme : no clean native fit — `ExpertOpinion_Inference`
  is about appeal-to-authority (not fabrication of status), `EvidenceToHypothesis_Inference` is about the
  evidence→hypothesis inference (not the false-premise claim). Fail-loud.

---

## 4. Cluster summary (proposed)

| pk | text_fr | attackType | attackedNode | DirectRef | ExceptionRef | MappingType | Honest? |
|----|---------|-----------|--------------|-----------|--------------|-------------|---------|
| 690 | Opération inappropriée (anchor) | undercut | RA-node | *(empty)* | *(empty)* | — | ✅ serialized |
| 691 | Sophisme mathématique | undercut | RA-node | *(empty — fail-loud)* | *(empty — fail-loud)* | narrowMatch | ✅ math-inference |
| 692 | Division par zéro | undercut | RA-node | *(empty — fail-loud)* | *(empty — fail-loud)* | narrowMatch | ✅ math-inference |
| 693 | Pseudo démonstration d'égalité | undercut | RA-node | *(empty — fail-loud)* | *(empty — fail-loud)* | narrowMatch | ✅ math-inference |
| 694 | Détournement de symbole | undercut | RA-node | *(empty — fail-loud)* | *(empty — fail-loud)* | narrowMatch | ⚠ boundary undermine |
| 695 | Pseudo-science | undermine | I-node | *(empty — fail-loud)* | *(empty — fail-loud)* | narrowMatch | ⚠ diverges (epistemic premise) |

**5 attack-typed leaves proposed, 0 fabrication.** Axiom **0 violation** (4 undercut→RA + 1 undermine→I).
- **5/5 attack-columns-only** (fail-loud on both skos layers) — no native Walton scheme fits the math-operational
  / epistemic defects. Consistent with the anchor 690 (cleanest cluster in the chantier on this dimension —
  every leaf is attack-columns-only, no token fits).
- **1/5 diverges from the anchor's attack-type** (695 undermine vs anchor undercut) — honest per-leaf
  derivation, flagged for ratification. **Odd-one-out** : pseudo-science is epistemic misrepresentation, not an
  operational math error — arguably a cataloguing edge case (it lives in « Opération inappropriée » but its
  mechanism is premise-fabrication).

---

## 5. Method notes (additions for the chantier)

- **Second mixed cluster → the math-inference-corruption pattern.** Where #837 (Complication exagérée) split
  process-burden (undercut) from source-discredit (undermine) along a PTK axis, this cluster splits
  inference-corruption (undercut) from premise-fabrication (undermine) along an operational-vs-epistemic axis.
  Both confirm the `[[aif-no-inherit-attacktype-from-anchor]]` discipline : a mixed result is honest when the
  sub-sub is not target-uniform.
- **All-fail-loud cluster = cleanest skos discipline.** Unlike #837 (3/6 carried a native scheme), this cluster
  has **0/5 native schemes** — math-operational defects (hidden error, div-by-zero, equivocation) and epistemic
  fabrication are not Walton argument schemes. This is the strongest demonstration of the attack-columns-only
  pattern : the load-bearing modelling is the 2 attack columns, and the skos layer is honestly empty across the
  board.
- **692 Division par zéro = ratification test case.** The 1=2 « proof » is a textbook ASPIC+ undercut : the
  inference demonstrably fails at the simplification-by-zero step (RA-node). If ai-01/jsboige want a single
  leaf to ratify the cluster's undercut/RA modelling, 692 is the cleanest.
- **695 Pseudo-science = cataloguing edge case.** Its mechanism (epistemic misrepresentation) does not match the
  cluster's umbrella (invalid math operation). It is modelled honestly (undermine/I-node, the false-premise
  reading) but flagged as a potential cataloguing question for jsboige — it may belong more naturally in an
  « Abus d'autorité » cluster than in « Opération inappropriée ». Not reclassified here (out of scope —
  proposition only, no taxonomy restructuring).

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not « 100 % leaves ») | ✅ §1/§4 (5 leaves attack-typed ; sub-sub Opération inappropriée fully opened, 6/6 rows addressed incl. anchor) |
| Per unmapped leaf : legitimate decomposition | ✅ §3 (attack-type + attacked-node, 0 fabrication) |
| `AIF_skosMappingType` coherent | ✅ §4 (narrowMatch throughout — all leaves specialize the anchor's invalid-operation mechanism, except 695 which diverges in target but still specializes the umbrella) |
| Fail-loud when no native token fits | ✅ 5/5 double fail-loud (both skos layers empty for all leaves) — documented in `AIF_skosOther` / annotation CSV |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies → CSV edits (gated) → OWL regen (#763 wiring ready) |

### Coverage accounting (code=truth)

- **Master coverage (rescan, `3a87eb0e`)** : **145/1 408 mapped** (`AIF_attackType` filled, 10.3 %). Dist :
  {undermine:53, undercut:87, rebut:5}. **Axiom 0 violation.**
- **This cluster (proposed)** : **+5 attack-typed** (4 undercut/RA + 1 undermine/I) → **150/1 408** projected
  once applied.
- **Erreur mathématique family footprint** : the cluster adds 4 undercut + 1 undermine to the math family's AIF
  modelling. Combined with #837's Obstruction cluster, the chantier now covers mixed undercut/undermine splits
  in two distinct families (Obstruction + Erreur mathématique).

**Opération inappropriée sub-sub : fully opened** (anchor 690 serialized + 5 leaves proposed = 6/6 addressed).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### Sub-mechanism A — math-inference corruption → undercut / RA-node

#### pk 691 — Sophisme mathématique → **undercut** (hidden calc error)
- **I-nodes** : the stated quantities + the calculation (the calculation is corrupted, not the quantities).
- **RA-node** : the rule-application (calculation → conclusion) rests on an erroneous computation.
- **CA-node** : none.
- **Attack type** : **undercut** (the inference *step* is attacked).

#### pk 692 — Division par zéro → **undercut** (div-by-zero)
- **I-nodes** : the equality's two members + the « common factor » (the factor is zero, hidden).
- **RA-node** : the simplification step is invalid (division by zero) → the 1=2 inference fails here.
- **CA-node** : none.
- **Attack type** : **undercut** (the algebraic rule-application is invalid).

#### pk 693 — Pseudo démonstration d'égalité → **undercut** (masked error)
- **I-nodes** : the two numbers claimed equal + the masked calculation.
- **RA-node** : the rule-application (masked calc → false equality) is corrupted.
- **CA-node** : none.
- **Attack type** : **undercut** (same as 691/692 — hidden error → invalid step).

#### pk 694 — Détournement de symbole → **undercut** (symbol equivocation)
- **I-nodes** : the symbol's established meaning (an information premise, corrupted by the equivocation).
- **RA-node** : the rule-application assumes a univocal symbol meaning ; equivocation breaks it.
- **CA-node** : none.
- **Attack type** : **undercut** (the inference step breaks), boundary *undermine* documented (symbol-meaning as
  premise).

### Sub-mechanism B — epistemic-premise fabrication → undermine / I-node

#### pk 695 — Pseudo-science → **undermine** (fabricated scientific status)
- **I-nodes** : the premise « these are scientific facts » (the attacked premise — fabricated, false).
- **RA-node** : *(no clean native scheme — the fallacy is the false premise, not the inference)*.
- **CA-node** : none.
- **Attack type** : **undermine** (a *premise* — the scientific status — is attacked). Diverges from the anchor
  honestly (epistemic misrepresentation, not operational).

### Cluster attack-type distribution
| pk | sub-mechanism | I-node (attacked premise) | RA-node (scheme) | Attack type |
|----|---------------|---------------------------|------------------|-------------|
| 690 | math-inference (anchor) | *(none — operation target)* | *(none — generic invalid op)* | undercut (serialized) |
| 691 | math-inference | *(none — calc corrupted)* | *(none — fail-loud)* | undercut |
| 692 | math-inference | *(none — div-by-zero)* | *(none — fail-loud)* | undercut |
| 693 | math-inference | *(none — masked error)* | *(none — fail-loud)* | undercut |
| 694 | math-inference | symbol meaning (boundary) | *(none — fail-loud)* | undercut (boundary undermine) |
| 695 | epistemic-fabrication | scientific status | *(none — fail-loud)* | **undermine** |

**4 math-inference → undercut/RA-node ; 1 epistemic-fabrication → undermine/I-node.** Serialization =
`AIF_attackType` + `AIF_attackedNode` per the table ; native skos empty for all 5 (fail-loud, documented).

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted. Post-T&A regime.
- ❌ No self-merge. No régén launch. Verdict QA = ai-01.
- ✅ Proposition derived code=truth from the taxonomy CSV (master `3a87eb0e`) + AIF vocabulary verified : no
  native token honestly fits any of the 5 leaves (math-operational / epistemic defects are not Walton schemes) →
  all 5 attack-columns-only, 0 fabricated token (0 fabrication #677).
- ✅ Second mixed cluster documented (math-inference vs epistemic-premise split) ; per-leaf boundary nuances
  flagged (694 boundary undermine, 695 diverges + cataloguing edge case) ; machine-readable annotation CSV
  `498-aif-operation-inappropriee-annotations.csv` provided for the gated serialization.

Relates : #498 (chantier), #837 (first mixed cluster — Complication exagérée, MERGED), #770 (anchor audit),
#760 (rebut cluster precedent), #763 (OWL AIF wiring), #677 (0 fabrication),
`[[aif-no-inherit-attacktype-from-anchor]]` (anchor-type-inheritance warning — this cluster's 1 divergent leaf
is the honest-derivation case), #499 (inverse : virtues), #133/#130 (OWL). Base master `3a87eb0e`.
