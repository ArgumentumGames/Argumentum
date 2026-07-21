# 2026-07-21 — #498 AIF chantier : cluster « Complication exagérée » (Obstruction) — **first mixed undercut/undermine cluster doc**

**Scope** : dedicated cluster doc for the **Complication exagérée** sub-sub (family **Obstruction**, sub-family
**Saboter le débat**) — **6 unmapped leaves** (1346-1351) under the anchor **1345 « Couper les cheveux en
quatre »** (already serialized `undercut`/`RA-node`, attack-columns-only). **Proposition only — GATED, 0 write
to prod CSV.** Continues the cluster-docs authoring backlog (ai-01 dispatch `ucdwi7`, strate-6 deep-queue,
post-T&A regime).

**Repo reference** : master `b442c658`. Issue : #498. Predecessors : #770 (anchor audit), tranches 1-1d
(#753/#769/#776/#779 — serialisation 93→121), tranches p1e/p1f/p1g (Reconciliation-A back-fill → **145/145
attack-typed, P1 complete, 0 residual**). Ontology wiring : #763 (OWL emits `AIF_attack*`). Layer C =
**1263 unmapped leaves** remain (the present doc opens 6 of them).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies → worker
> applies the CSV edits in a follow-up PR (gated), same flow as #753/#760. The attack-type decomposition is
> recorded here + in the machine-readable annotation CSV `498-complication-exageree-annotations.csv` ;
> serialisation to the 2 columns (`AIF_attackType`, `AIF_attackedNode`) is deferred to the gated apply.

---

## TL;DR

- **Complication exagérée** (Obstruction > Saboter le débat, sub-sub, 7 rows total) : anchor **1345** already
  serialized `undercut`/`RA-node` (attack-columns-only, no skos) ; **6 unmapped leaves** — 1346 Méthode
  hypercritique, 1347 Appel à l'expertise, 1348 Sophisme de l'examen sur-le-champ, 1349 Justification infinie,
  1350 Solution parfaite, 1351 Objection triviale.
- **First mixed undercut/undermine cluster.** Unlike the Relativisme abusif cluster (all `rebut`, one
  mechanism), this sub-sub splits into **two honest sub-mechanisms** :
  - **Process-burdening** (1346, 1349, 1350, 1351) — overwhelms the *inference process* (the RA-node /
    rule-application) to prevent closure → **undercut / RA-node** (matches the anchor 1345).
  - **Source-discrediting** (1347, 1348) — attacks the opponent's *competence as an information source* (an
    I-node / premise) → **undermine / I-node** (genuine divergence from the anchor, NOT inheritance — derived
    per-leaf from the PTK target).
- **Native-scheme identification varies.** 3/6 leaves carry a **native, code=truth-attested** scheme : 1347 +
  1348 (Position-To-Know family : `LackOfPTKReliability_Scheme` / `LackOfCompleteKnowledge_Conflict` /
  `PositionToKnow_Inference`) and 1350 (`PracticalReasoning_Inference` — the nirvana-fallacy CQ on practical
  means). The other 3/6 are **attack-columns-only** with a fail-loud note (no native CQ-conflict token for a
  generic procedural burden — same #677 honesty as the rebut anchor 1282).
- **6 attack-typed leaves proposed, 0 fabrication, axiom 0 violation** (4 undercut→RA + 2 undermine→I).
  Projected coverage **145 → 151 / 1408** once applied.

---

## 1. Cluster state (code=truth, master `b442c658`)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Obstruction**, sub-family
**Saboter le débat**, sub-sub **Complication exagérée** (7 rows) :

| pk | text_fr | depth | AIF_attackType | AIF_attackedNode | skos cols | State |
|----|---------|-------|----------------|------------------|-----------|-------|
| **1345** | **Couper les cheveux en quatre** (anchor, sub-sub = « Complication exagérée ») | d3 | `undercut` | `RA-node` | *(all empty)* | **✅ serialized** (attack-columns-only) |
| 1346 | Méthode hypercritique | d4 | — | — | — | unmapped (**this cluster**) |
| 1347 | Appel à l'expertise | d5 | — | — | — | unmapped (**this cluster**) |
| 1348 | Sophisme de l'examen sur-le-champ | d6 | — | — | — | unmapped (**this cluster**) |
| 1349 | Justification infinie | d5 | — | — | — | unmapped (**this cluster**) |
| 1350 | Solution parfaite | d4 | — | — | — | unmapped (**this cluster**) |
| 1351 | Objection triviale | d4 | — | — | — | unmapped (**this cluster**) |

**Leaf content (code=truth `desc_fr` / `example_fr`)** :
- **1345 Couper les cheveux en quatre** — « Vous complexifiez volontairement le débat afin d'empêcher toute
  résolution claire. » / « Je ne vois pas comment on pourrait discuter de ce sujet, il y a de toute façon
  beaucoup trop de variables à prendre en compte. »
- **1346 Méthode hypercritique** — « Vous critiquez de manière disproportionnée pour embarrasser votre
  interlocuteur et éviter le fond du problème. » / « Comment pouvez-vous être absolument certain que cela
  s'est passé exactement comme vous le décrivez ? »
- **1347 Appel à l'expertise** — « Vous exigez une expertise spécifique pour discréditer l'avis de tout
  interlocuteur non spécialiste. » / « À moins que vous ne soyez un climatologue, votre opinion ne compte pas
  sur ces questions très complexes. »
- **1348 Sophisme de l'examen sur-le-champ** — « Vous jugez votre interlocuteur incompétent s'il ne peut pas
  citer immédiatement des données précises ou des détails techniques sur un sujet. » / « Si vous ne pouvez pas
  me citer les dernières statistiques sur la croissance économique, alors votre point de vue sur la politique
  économique n'est pas valable. »
- **1349 Justification infinie** — « Vous demandez sans cesse davantage d'explications, tout en évitant
  d'arriver à une conclusion claire. » / « Mais pourquoi ? Et d'où vient cette information ? Et pourquoi
  devrais-je croire cette source ? »
- **1350 Solution parfaite** — « Vous rejetez les solutions réalistes en demandant un idéal inatteignable. » /
  « Je suis opposée au port obligatoire de la ceinture de sécurité, car cette mesure n'empêche pas tous les
  accidents de la route. »
- **1351 Objection triviale** — « Vous soulevez des objections mineures pour affaiblir les arguments avancés. »
  / « Vous me dites que marcher jusqu'au supermarché est bon pour la santé, mais cela ne m'intéresse pas, car
  je ne vais jamais au supermarché. »

The sub-sub is **semantically coherent around one umbrella mechanism** — *procedural obstruction by
burdening* — but that umbrella forks into two distinct AIF targets (process vs source), which is why this is a
**mixed** cluster rather than a uniform one. §3 derives each leaf's target honestly.

---

## 2. The anchor model (1345, serialized) — attack-columns-only

`1345` is serialized `AIF_attackType=undercut`, `AIF_attackedNode=RA-node`, **all skos relational columns
empty**. This is #677-honest : the anchor models a *generic procedural burden on the inference process* — it
attacks the **rule-application** (RA-node) by deliberate over-complication, but there is **no native Walton
CQ-conflict token** for « deliberate complication ». The load-bearing modelling is the 2 attack columns ; the
skos layer is honestly empty.

**Verification (code=truth, native-token scan of prod CSV)** :

| candidate token | native? (in a filled skos column) |
|-----------------|-----------------------------------|
| `LackOfPTKReliability_Scheme` | ✅ native (attested) |
| `LackOfCompleteKnowledge_Conflict` | ✅ native (attested) |
| `PositionToKnow_Inference` | ✅ native (attested) |
| `PracticalReasoning_Inference` | ✅ native (attested) |
| *(any « Complication » / « Burden » / « Regress » conflict token)* | ❌ NOT native (absent — fail-loud) |

The 4 process-burdening leaves (1346, 1349, 1350, 1351) share the anchor's *generic-burden* character. Of
these, only **1350** has a clean native rebutted scheme (`PracticalReasoning_Inference` — the nirvana-fallacy
CQ) ; the other 3 are attack-columns-only. The 2 source-discrediting leaves (1347, 1348) leave the
process-burden family entirely and carry native PTK schemes.

---

## 3. Proposed AIF structure for the leaves

Per-leaf derivation. `attackType` + `attackedNode` follow the AIF/ASPIC+ axiom (undercut→RA, undermine→I,
rebut→CA — **0 violation**). `skosDirectRef` / `skosExceptionRef` carry a **native** token only where honest ;
otherwise empty with a fail-loud note. **0 fabrication (#677).**

### Sub-mechanism A — process-burdening → **undercut / RA-node** (matches anchor)

#### pk 1346 — Méthode hypercritique (disproportionate criticism) ✅ undercut
- **Mechanism** : overwhelms the inference *process* with disproportionate demands (« comment pouvez-vous
  être *absolument* certain ? ») to prevent the conclusion from settling. The target is the rule-application
  (RA-node), not a premise.
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef` **empty**, `ExceptionRef`
  **empty**, fail-loud note in `AIF_skosOther`, `MappingType=skos:narrowMatch` (d4 specialization of the
  anchor's generic complication).
- **Why not undermine** : no specific premise is contested — the criticism is *disproportionate* (process
  burden), not *factual* (premise attack). Why no scheme : no native CQ-conflict token for « hypercritical
  burden » honestly applies.

#### pk 1349 — Justification infinie (infinite regress) ✅ undercut
- **Mechanism** : endless « mais pourquoi ? » regress that prevents the rule-application from closing. Target
  = RA-node (the inference never reaches a settled conclusion).
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef` **empty**, `ExceptionRef`
  **empty**, fail-loud note, `MappingType=skos:narrowMatch` (d5).
- **Why not undermine** : no single premise is attacked — the move is procedural regress (process). Why no
  scheme : AIF has no native Walton CQ-conflict token for « infinite regress ». Latin `ad infinitum` is a
  label, not a decomposition (DoD : legitimate decomposition, not Latin alone).

#### pk 1350 — Solution parfaite (nirvana / perfect-solution fallacy) ✅ undercut
- **Mechanism** : rejects a realistic solution (ceinture de sécurité) for not being *perfect* (n'empêche pas
  *tous* les accidents). Attacks the **practical-reasoning rule-application** by imposing an unreachable
  standard — violates the CQ « are the proposed means sufficient for the goal ? ».
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef` **empty** (no native CQ-conflict
  token for « impossible standard »), `ExceptionRef=PracticalReasoning_Inference` (native — the scheme whose
  sufficiency CQ is violated), `MappingType=skos:narrowMatch` (d4).
- **Why undercut** : the attack is on the *rule application* (the practical means → goal inference is declared
  insufficient), not on a premise. The rebutted scheme (`PracticalReasoning_Inference`) is native → the
  ExceptionRef is serializable ; the DirectRef stays empty (no conflict token).

#### pk 1351 — Objection triviale (quibbling) ✅ undercut
- **Mechanism** : raises a minor, tangential objection (« je ne vais jamais au supermarché ») to weaken the
  argument's force. Target = RA-node (the objection burden the rule-application rather than refute a premise).
- **Proposal** : `attackType=undercut`, `attackedNode=RA-node`, `DirectRef` **empty**, `ExceptionRef`
  **empty**, fail-loud note, `MappingType=skos:narrowMatch` (d4).
- **Why not undermine** : the objection is tangential/minor (process noise), not a premise refutation. Why no
  scheme : no native CQ-conflict token for « trivial objection ».

### Sub-mechanism B — source-discrediting → **undermine / I-node** (diverges from anchor)

#### pk 1347 — Appel à l'expertise (demand expertise) ⚠ undermine (diverges from anchor)
- **Mechanism** : discredits the opponent's view by demanding specialist expertise (« à moins que vous ne
  soyez un climatologue »). The target is the opponent's **reliability as an information source** — an
  I-node (premise), not the rule-application.
- **Proposal** : `attackType=undermine`, `attackedNode=I-node`, `DirectRef=LackOfPTKReliability_Scheme`
  (native — the source's position-to-know reliability is the attacked premise), `ExceptionRef=PositionToKnow_Inference`
  (native — the Argument from Position to Know, whose source-reliability CQ is exploited), `MappingType=skos:narrowMatch`
  (d5).
- **Why undermine, not undercut (honest divergence from the anchor)** : the anchor (1345) burdens the
  *process* (RA-node) ; 1347 attacks a *premise* (the opponent's competence as a source = I-node). This is
  **per-leaf derivation, NOT anchor inheritance** — the memory `[[aif-no-inherit-attacktype-from-anchor]]`
  warns against inheriting the anchor's type, and here the divergence is the honest result : a PTK-premise
  attack is `undermine/I-node` by the AIF axiom. Flagged for ai-01/jsboige ratification.

#### pk 1348 — Sophisme de l'examen sur-le-champ (demand immediate recall) ⚠ undermine (diverges from anchor)
- **Mechanism** : judges the opponent incompetent for not citing data *immediately* (« si vous ne pouvez pas
  me citer les dernières statistiques »). Target = the opponent's **knowledge completeness** as a source — an
  I-node (premise).
- **Proposal** : `attackType=undermine`, `attackedNode=I-node`, `DirectRef=LackOfCompleteKnowledge_Conflict`
  (native — the source's incomplete knowledge is the attacked premise), `ExceptionRef=PositionToKnow_Inference`
  (native), `MappingType=skos:narrowMatch` (d6).
- **Why undermine (honest divergence)** : same PTK-premise logic as 1347 — attacks a premise (knowledge
  completeness), not the rule-application. Native tokens attested → serializable.

---

## 4. Cluster summary (proposed)

| pk | text_fr | attackType | attackedNode | DirectRef | ExceptionRef | MappingType | Honest? |
|----|---------|-----------|--------------|-----------|--------------|-------------|---------|
| 1345 | Couper les cheveux en quatre (anchor) | undercut | RA-node | *(empty)* | *(empty)* | — | ✅ serialized |
| 1346 | Méthode hypercritique | undercut | RA-node | *(empty — fail-loud)* | *(empty — fail-loud)* | narrowMatch | ✅ process-burden |
| 1347 | Appel à l'expertise | undermine | I-node | `LackOfPTKReliability_Scheme` (native) | `PositionToKnow_Inference` (native) | narrowMatch | ⚠ diverges (PTK premise) |
| 1348 | Sophisme de l'examen sur-le-champ | undermine | I-node | `LackOfCompleteKnowledge_Conflict` (native) | `PositionToKnow_Inference` (native) | narrowMatch | ⚠ diverges (PTK premise) |
| 1349 | Justification infinie | undercut | RA-node | *(empty — fail-loud)* | *(empty — fail-loud)* | narrowMatch | ✅ process-burden |
| 1350 | Solution parfaite | undercut | RA-node | *(empty — fail-loud)* | `PracticalReasoning_Inference` (native) | narrowMatch | ✅ process-burden |
| 1351 | Objection triviale | undercut | RA-node | *(empty — fail-loud)* | *(empty — fail-loud)* | narrowMatch | ✅ process-burden |

**6 attack-typed leaves proposed, 0 fabrication.** Axiom **0 violation** (4 undercut→RA + 2 undermine→I).
- **3/6 carry a native scheme** (1347 PTK, 1348 PTK, 1350 PracticalReasoning) — all 4 tokens verified native
  on master `b442c658`.
- **3/6 are attack-columns-only** (1346, 1349, 1351) with a documented fail-loud on both skos layers — no
  native CQ-conflict token for a generic procedural burden (consistent with the anchor 1345).
- **2/6 diverge from the anchor's attack-type** (1347/1348 undermine vs anchor undercut) — honest per-leaf
  derivation, flagged for ratification.

---

## 5. Method notes (additions for the chantier)

- **First mixed undercut/undermine cluster → honest sub-mechanism split.** Uniform clusters (Relativisme
  abusif = all `rebut`) inherit the anchor's attack-type cleanly. This cluster demonstrates the **mixed** case
  where one umbrella mechanism (« procedural obstruction ») forks into two AIF targets : process-burden
  (undercut/RA) and source-discredit (undermine/I). The split is **derived per leaf**, not imposed — 4/6 match
  the anchor (process-burden), 2/6 diverge (PTK premise attack). This complements the `[[aif-no-inherit-
  attacktype-from-anchor]]` discipline : you neither inherit blindly *nor* diverge arbitrarily — you derive,
  and a mixed result is the honest outcome when the sub-sub is not target-uniform.
- **Native PTK vocabulary is the cluster's richest seam.** The Position-To-Know family
  (`LackOfPTKReliability_Scheme` / `LackOfCompleteKnowledge_Conflict` / `PositionToKnow_Inference`) is
  natively attested and gives 1347 + 1348 a full DirectRef + ExceptionRef pair — the only leaves in this
  cluster with both skos layers filled. The process-burden leaves have no equivalent native vocabulary
  (deliberate complication / regress / quibbling are not Walton CQ-conflicts) → they stay attack-columns-only.
- **Nirvana fallacy = practical-reasoning undercut.** 1350 is a textbook ASPIC+ undercut of the
  practical-reasoning scheme (reject means→goal because the means are not *perfect*). `PracticalReasoning_Inference`
  is native → the ExceptionRef is serializable. This is the cleanest single-scheme identification in the
  cluster and a good ratification test case.
- **Latin is a label, not a decomposition.** 1349 carries `Latin=ad infinitum` — recorded, but the AIF
  decomposition is `undercut/RA-node` (regress prevents rule-application closure), not the Latin term. DoD :
  legitimate decomposition, not Latin alone.

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not « 100% leaves ») | ✅ §1/§4 (6 leaves attack-typed ; sub-sub Complication exagérée fully opened, 7/7 rows addressed incl. anchor) |
| Per unmapped leaf : legitimate decomposition (not Latin alone) | ✅ §3 (attack-type + attacked-node + native scheme where honest, 0 fabrication) |
| `AIF_skosMappingType` coherent | ✅ §4 (narrowMatch throughout — all leaves are specializations of the anchor's obstruction mechanism) |
| Fail-loud when no native token fits | ✅ 3/6 double fail-loud (1346/1349/1351 — both skos layers empty) + 3/6 single fail-loud (1350 DirectRef empty) ; documented in `AIF_skosOther` / annotation CSV |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies → CSV edits (gated) → OWL regen (#763 wiring ready) |

### Coverage accounting (code=truth)

- **Master coverage (rescan, `b442c658`)** : **145/1408 mapped** (`AIF_attackType` filled, 10.3 %). Dist :
  {undermine:53, undercut:87, rebut:5}. **Axiom 0 violation.**
- **This cluster (proposed)** : **+6 attack-typed** (4 undercut/RA + 2 undermine/I) → **151/1408** projected
  once applied.
- **Obstruction family footprint** : the 2 proposed undermine leaves (1347/1348) are the first PTK-premise
  attacks in the Saboter le débat sub-family — they connect this obstruction cluster to the broader
  Position-To-Know modelling (cf. `LackOfPTKReliability_Scheme` already used in Influence/Obstruction rows).

**Complication exagérée sub-sub : fully opened** (anchor 1345 serialized + 6 leaves proposed = 7/7 addressed).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### Sub-mechanism A — process-burdening → undercut / RA-node

#### pk 1346 — Méthode hypercritique → **undercut** (process burden)
- **I-nodes** : the opponent's stated claim + supporting data (not contested as false — contested as
  *insufficient by an unreachable standard*).
- **RA-node** : the rule-application from claim+data → conclusion is burdened by disproportionate criticism
  (« êtes-vous *absolument* certain ? »).
- **CA-node** : none (not a counter-conclusion).
- **Attack type** : **undercut** (the inference *process* is attacked, not a premise or a conclusion).

#### pk 1349 — Justification infinie → **undercut** (regress)
- **I-nodes** : each offered explanation becomes a new premise demanded of the next « mais pourquoi ? ».
- **RA-node** : the rule-application never closes — regress prevents the conclusion from settling.
- **CA-node** : none.
- **Attack type** : **undercut** (procedural regress on the process).

#### pk 1350 — Solution parfaite → **undercut** (nirvana)
- **I-nodes** : the realistic solution (ceinture de sécurité reduces harm).
- **RA-node** : `PracticalReasoning_Inference` — the means→goal rule-application is declared insufficient
  because the means are not *perfect* (n'empêche pas *tous* les accidents). The sufficiency CQ is violated.
- **CA-node** : none.
- **Attack type** : **undercut** (the practical-reasoning rule is attacked, not the premise that seatbelts
  reduce harm).

#### pk 1351 — Objection triviale → **undercut** (quibble)
- **I-nodes** : the opponent's argument (marching is healthy) + the tangential objection (« je ne vais jamais
  au supermarché »).
- **RA-node** : the rule-application is burdened by a tangential objection that does not refute the premise.
- **CA-node** : none.
- **Attack type** : **undercut** (process noise, not premise refutation).

### Sub-mechanism B — source-discrediting → undermine / I-node

#### pk 1347 — Appel à l'expertise → **undermine** (PTK reliability)
- **I-nodes** : the opponent's **reliability as an information source** (the attacked premise — « votre avis
  ne compte pas si vous n'êtes pas climatologue »).
- **RA-node** : `PositionToKnow_Inference` — the Argument from Position to Know, whose source-reliability CQ
  is exploited (the fallacy *abuses* the legitimate CQ « is the source in a position to know ? »).
- **CA-node** : none.
- **Attack type** : **undermine** (a *premise* — the source's PTK reliability — is attacked). Diverges from
  the anchor honestly.

#### pk 1348 — Sophisme de l'examen sur-le-champ → **undermine** (knowledge completeness)
- **I-nodes** : the opponent's **knowledge completeness** as a source (the attacked premise — incompétent
  faute de citer les statistiques *immédiatement*).
- **RA-node** : `PositionToKnow_Inference` — same PTK scheme, CQ « does the source have complete knowledge ? »
  abused into an on-the-spot recall demand.
- **CA-node** : none.
- **Attack type** : **undermine** (a *premise* — the source's knowledge completeness — is attacked). Diverges
  from the anchor honestly.

### Cluster attack-type distribution
| pk | sub-mechanism | I-node (attacked premise) | RA-node (scheme) | Attack type |
|----|---------------|---------------------------|------------------|-------------|
| 1345 | process-burden (anchor) | *(none — process target)* | *(none — generic burden)* | undercut (serialized) |
| 1346 | process-burden | *(none)* | *(none — fail-loud)* | undercut |
| 1347 | source-discredit | opponent's PTK reliability | `PositionToKnow_Inference` | **undermine** |
| 1348 | source-discredit | opponent's knowledge completeness | `PositionToKnow_Inference` | **undermine** |
| 1349 | process-burden | *(none)* | *(none — regress, fail-loud)* | undercut |
| 1350 | process-burden | *(none — process target)* | `PracticalReasoning_Inference` | undercut |
| 1351 | process-burden | *(none)* | *(none — quibble, fail-loud)* | undercut |

**4 process-burden → undercut/RA-node ; 2 source-discredit → undermine/I-node.** Serialization =
`AIF_attackType` + `AIF_attackedNode` per the table ; native skos only where attested (1347/1348/1350) ;
attack-columns-only otherwise (1346/1349/1351), fail-loud documented.

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted. Post-T&A regime.
- ❌ No self-merge. No régén launch. Verdict QA = ai-01.
- ✅ Proposition derived code=truth from the taxonomy CSV (master `b442c658`) + AIF vocabulary verified native
  on master (`LackOfPTKReliability_Scheme`, `LackOfCompleteKnowledge_Conflict`, `PositionToKnow_Inference`,
  `PracticalReasoning_Inference` all attested ; no fabricated token introduced — 0 fabrication #677).
- ✅ First mixed undercut/undermine cluster documented (honest sub-mechanism split) ; per-leaf boundary
  nuances flagged for ai-01/jsboige ratification ; machine-readable annotation CSV
  `498-complication-exageree-annotations.csv` provided for the gated serialization.

Relates : #498 (chantier), #770 (anchor audit), #753/#769/#776/#779 (serialisation tranches 1-1d),
#760 (rebut cluster precedent — Relativisme abusif), #763 (OWL AIF wiring), #677 (0 fabrication),
`[[aif-no-inherit-attacktype-from-anchor]]` (anchor-type-inheritance warning — this cluster's 2 divergent
leaves are the honest-derivation counter-example), #499 (inverse : virtues), #133/#130 (OWL).
Base master `b442c658`.
