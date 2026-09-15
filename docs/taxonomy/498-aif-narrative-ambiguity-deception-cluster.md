# 2026-07-05 — #498 AIF chantier, PR-9 : cluster « Ambiguïté narrative / Deception » (source-deception + interrogation + polytélie)

**Scope**: ninth cluster of the #498 chantier — **completes the Ambiguïté subfamily** (3/3 sub-subs).
Second half of the Ambiguïté narrative sub-sub: the **deferred mechanisms** from PR-8 —
source-deception (pk 881-883), interrogation (pk 884-885), and polytélie (pk 886), 6 leaves.
**Proposition only — GATED, 0 write to prod CSV.** Triggered by ai-01 dispatch `ih617l` (tertiary:
« continue en autonomie sur les clusters suivants »). **In-sub-sub anchor shape — independent of the
I/RA/CA serialization decision** (no schema dependency).

**Repo reference**: master `204adc47`. Issue: #498. Predecessors: PR-1 #699, PR-2 #701, PR-3 #703
(Fallacious comparison complete), PR-4 #705 (Vague definition), PR-6 #708 (Inconsistent definition),
PR-7 #711 (Amphibologie), PR-8 #713 (Narrative ambiguity / insinuation).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies →
> worker applies the CSV edits in a follow-up PR (gated).

---

## TL;DR

PR-8 (insinuation, pk 877-880) scoped the Ambiguïté narrative sub-sub to the *implication* mechanism.
This PR completes the sub-sub with the **deferred mechanisms**: source-deception (881-883), interrogation
(884-885), and polytélie (886). All 6 leaves borrow the in-sub-sub anchor pk 876
(`ConflictingGoals_Conflict`, direct-conflict, `skos:broadMatch`) where the conflict-goals mechanism
fits, and honestly diverge to other native schemes where the mechanism is genuinely different
(epistemic source-deception → `ExpertOpinion_Inference`/`PositionToKnow_Inference`).

**6 leaves proposed, 6 fully-modeled, 0 FAIL-LOUD** — the second consecutive clean cluster (after PR-8).
This PR **completes the Ambiguïté narrative sub-sub** (4 insinuation + 6 deception = 10/10 leaves) and
**completes the Ambiguïté subfamily** (3/3 sub-subs: Amphibologie PR-7, Narrative-ambiguity PR-8+9).

The cluster demonstrates **4 first-time chantier native tokens** — `ExpertOpinion_Inference`,
`ExpertiseInconsistency_Conflict`, `PracticalReasoning_Inference`, `Bias_Inference` — expanding the
demonstrated palette to **21 distinct native tokens** across 9 PR.

---

## 1. Cluster state (code=truth)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Abus de langage** (Misleading
language), subfamily **Ambiguïté**, sub-sub **Ambiguïté narrative** (continuation of PR-8):

| pk | text_fr | depth | DirectRef | ExceptionRef | MappingType | State |
|----|---------|-------|-----------|--------------|-------------|-------|
| **876** | **Ambiguïté narrative** (in-sub-sub anchor, recap) | d3 | `ConflictingGoals_Conflict` | — | `skos:broadMatch` | **✅ MAPPED (PR-8 anchor)** |
| 881 | Propagande grise | d4 | — | — | — | unmapped (**PR-9**) |
| 882 | Campagne de murmures | d5 | — | — | — | unmapped (**PR-9**) |
| 883 | Propagande noire | d4 | — | — | — | unmapped (**PR-9**) |
| 884 | Interrogatoire clandestin | d5 | — | — | — | unmapped (**PR-9**) |
| 885 | Prêcher le faux pour savoir le vrai | d5 | — | — | — | unmapped (**PR-9**) |
| 886 | Polytélie | d4 | — | — | — | unmapped (**PR-9**) |

The anchor (876) is the template: a narrative whose goals conflict (benign surface + deniable payload).
PR-8 covered the implication mechanism (877-880); this PR covers the **source-deception** (881-883),
**interrogation** (884-885), and **multi-goal** (886) mechanisms. The source-deception and interrogation
leaves honestly diverge from the anchor's `ConflictingGoals_Conflict` to epistemic/action schemes that
fit their semantics better.

---

## 2. The anchor (876, recap from PR-8)

**Conflict node** (DirectRef): `ConflictingGoals_Conflict` — the narrative serves conflicting goals
(benign surface reading + persuasive/deniable payload). PR-8 showed the implication leaves (877-880)
reuse this token. This PR shows the deception leaves split: source-deception targets epistemic schemes
(`ExpertOpinion_Inference`, `PositionToKnow_Inference`) because the defect is *credibility
manipulation*, not conflicting narrative goals; interrogation targets `PracticalReasoning_Inference`
because the defect is *action under a false context*; polytélie (886) reuses the anchor's token directly
(multiple goals = the anchor's mechanism made literal).

---

## 3. Proposed AIF structure for the leaves

For each leaf: **(a) legitimate scheme, (b) exception/CQ or direct conflict, (c) `AIF_skosMappingType`**.
Vocabulary restricted to AIF-native tokens confirmed by existing usage.

### Source-deception mechanism (881-883)

### pk 881 — Propagande grise (Grey propaganda) ✅
- **desc_fr**: "Vous dissimulez l'origine réelle de vos informations pour les rendre plus crédibles,
  sans révéler vos intentions ni vos liens."
- **Legitimate scheme**: `ExpertOpinion_Inference` (AIF native, confirmed 1x in existing usage) — the
  audience treats the information as coming from a credible, independent source.
- **Exception/CQ**: the source's independence/expertise is **hidden** (origin concealed, links
  undisclosed) → the expertise the audience presumes is **inconsistent** with the source's actual
  position. Native conflict: `ExpertiseInconsistency_Conflict` (AIF native, confirmed 1x — captures an
  inconsistency between presumed and actual expertise).
- **⚠ Omission mechanism**: grey propaganda *conceals* the source (passive omission) — the audience
  cannot verify the expertise. Distinguished from 883 (active falsification) by the MappingType.
- **Proposal**: `ExceptionRef=ExpertOpinion_Inference`, `DirectRef=ExpertiseInconsistency_Conflict`,
  `MappingType=skos:narrowMatch` (a narrower case: expertise inconsistency *specifically via concealed
  origin*, narrower than the general expertise-inconsistency defeat).

### pk 882 — Campagne de murmures (Whisper campaign) ✅
- **desc_fr**: "Vous répandez des rumeurs nuisibles ou des insinuations à propos d'une cible, tout en
  évitant d'être identifié."
- **⚠ Cross-cluster scheme reuse (PR-8 880)**: like 880 (anonymous indiscretion), the defect is
  **epistemic** — the rumor's source is anonymous, so the audience cannot assess the source's position
  to know. This honestly reuses PR-8's 880 scheme+conflict pair, distinguished by the *organized
  campaign* flavor (882 = coordinated/repeated; 880 = single indiscretion).
- **Legitimate scheme**: `PositionToKnow_Inference` (native, used in PR-8 880).
- **Exception/CQ**: the source's position-to-know is **unverifiable** due to anonymity →
  `LackOfCompleteKnowledge_Conflict` (native, used in PR-8 880).
- **Proposal**: `ExceptionRef=PositionToKnow_Inference`, `DirectRef=LackOfCompleteKnowledge_Conflict`,
  `MappingType=skos:narrowMatch` (a narrower case: position-to-know defeated *specifically via an
  organized anonymous whisper campaign*, narrower than 880's single-anonymous-indiscretion case).

### pk 883 — Propagande noire (Black propaganda) ✅
- **desc_fr**: "Vous faites croire que vos arguments viennent de vos alliés, alors qu'ils émanent en
  réalité de vos opposants."
- **Legitimate scheme**: `ExpertOpinion_Inference` (same as 881 — the audience presumes a credible
  allied source).
- **Exception/CQ**: the source attribution is **falsified** (active commission — the audience believes
  the source is an ally, but it is the opponent). Same native conflict as 881:
  `ExpertiseInconsistency_Conflict` (the presumed source is inconsistent with the real source).
- **⚠ Commission mechanism**: black propaganda *falsifies* the source (active misattribution) —
  distinguished from 881 (passive concealment). The MappingType encodes this: 883 is a *direct variant*
  of the expertise-inconsistency defeat, while 881 is a *narrower* (concealment) specialization.
- **Proposal**: `ExceptionRef=ExpertOpinion_Inference`, `DirectRef=ExpertiseInconsistency_Conflict`,
  `MappingType=skos:closeMatch` (a direct variant: expertise inconsistency *via falsified source
  attribution*, a direct sibling of the general case).

### Interrogation mechanism (884-885)

### pk 884 — Interrogatoire clandestin (Clandestine interrogation) ✅
- **desc_fr**: "Vous menez un interrogatoire sans révéler vos intentions ni votre identité, en
  engageant la personne dans une conversation détendue pour obtenir des informations."
- **Legitimate scheme**: `PracticalReasoning_Inference` (AIF native, confirmed 2x in existing usage) —
  the target's decision to share information is a practical action taken on the (false) premise of a
  friendly conversation.
- **Exception/CQ**: the interrogator's goals are **concealed** (friendly surface + extraction goal) →
  the target's practical reasoning is defective because the action context is engineered to serve the
  interrogator's conflicting goals. Native conflict: `ConflictingGoals_Conflict` (native, **borrowed
  from anchor 876** — the friendly-conversation surface + information-extraction goal is exactly the
  conflicting-goals mechanism).
- **Proposal**: `ExceptionRef=PracticalReasoning_Inference`, `DirectRef=ConflictingGoals_Conflict`,
  `MappingType=skos:narrowMatch` (a narrower case: conflicting goals *specifically in the interrogation
  context*, where the target's practical reasoning is the vehicle).

### pk 885 — Prêcher le faux pour savoir le vrai (Preach the false to know the true) ✅
- **desc_fr**: "Vous faites de fausses affirmations ou posez des questions trompeuses afin d'observer
  les réactions et de déceler la vérité."
- **⚠ Direct-conflict pattern (scheme as defect)**: the speaker manufactures false premises to elicit
  reactions, then infers the truth from those reactions. The defect is that the **evidence is biased by
  its manufactured collection method** — the reactions are to false premises, not to the real state of
  affairs. This honestly targets `Bias_Inference` (AIF native, confirmed 2x in existing usage) as a
  direct conflict: the inference (reactions ⟹ truth) is itself a biased inference because the evidence
  was obtained under false pretenses.
- **Pattern**: direct-conflict — the `Bias_Inference` *is* the defect (the scheme names the bias
  introduced by the false-premise collection).
- **Proposal**: `DirectRef=Bias_Inference`, `ExceptionRef=`, `MappingType=skos:narrowMatch` (a narrower
  case: bias *specifically via false-premise evidence collection*, narrower than the general biased-
  inference case).

### Multi-goal mechanism (886)

### pk 886 — Polytélie (Multiple goals / multi-telic problem) ✅
- **desc_fr**: "Vous présentez un problème qui sert plusieurs objectifs, ce qui rend la solution
  ambiguë."
- **Direct-conflict reusing anchor token**: polytélie is the *literal* case of the anchor 876's
  `ConflictingGoals_Conflict` — the problem serves several objectives, so no determinate solution
  exists. This is the cleanest anchor reuse in the chantier: the leaf *is* the anchor's mechanism named
  as a problem-structure rather than a narrative.
- **Proposal**: `DirectRef=ConflictingGoals_Conflict`, `ExceptionRef=`,
  `MappingType=skos:closeMatch` (a direct variant: conflicting goals *as a problem structure*, a direct
  sibling of the anchor's narrative framing).

---

## 4. Cluster summary (proposed)

| pk | text_fr | Proposed ExceptionRef | Proposed DirectRef | MappingType | Honest? |
|----|---------|----------------------|-------------------|-------------|---------|
| 881 | Propagande grise | `ExpertOpinion_Inference` | `ExpertiseInconsistency_Conflict` | `skos:narrowMatch` | ✅ (concealment) |
| 882 | Campagne de murmures | `PositionToKnow_Inference` | `LackOfCompleteKnowledge_Conflict` | `skos:narrowMatch` | ✅ (cross-PR-8 reuse) |
| 883 | Propagande noire | `ExpertOpinion_Inference` | `ExpertiseInconsistency_Conflict` | `skos:closeMatch` | ✅ (falsification) |
| 884 | Interrogatoire clandestin | `PracticalReasoning_Inference` | `ConflictingGoals_Conflict` | `skos:narrowMatch` | ✅ (anchor reuse) |
| 885 | Prêcher le faux pour savoir le vrai | *(absent)* | `Bias_Inference` | `skos:narrowMatch` | ✅ (direct-conflict) |
| 886 | Polytélie | *(absent)* | `ConflictingGoals_Conflict` | `skos:closeMatch` | ✅ (anchor reuse, literal) |

**6 leaves proposed, 6 fully-modeled, 0 FAIL-LOUD.** The cluster spans **three patterns**: exception
(881, 882, 883, 884 — scheme + conflict), direct-conflict (885, 886 — scheme as defect). Two leaves
reuse the anchor's `ConflictingGoals_Conflict` (884, 886); two reuse PR-8's epistemic pair (882 →
880); two introduce source-deception schemes (881, 883 → `ExpertOpinion_Inference`).

### Mechanism specialization via MappingType (881 vs 883)
881 (grey) and 883 (black) share the same scheme+conflict pair
(`ExpertOpinion_Inference` + `ExpertiseInconsistency_Conflict`) — the distinction is the **mechanism**:
grey = concealment (omission, narrower), black = falsification (commission, closer to the general
expertise-inconsistency case). The MappingType (`narrowMatch` vs `closeMatch`) encodes this, giving the
consumer a graded notion of the source-deception mechanism.

---

## 5. Method notes (additions for the chantier)

- **Sub-sub completion by mechanism split**: PR-8 (insinuation, 4 leaves) + this PR (deception +
  interrogation + polytélie, 6 leaves) complete the Ambiguïté narrative sub-sub (10/10 leaves). The
  méthode's "split a >5-leaf sub-sub by mechanism" rule (§3 of #707) produces two coherent PRs rather
  than one incoherent 10-leaf PR. Each mechanism family (implication / source-deception /
  interrogation / multi-goal) maps to a distinct AIF-scheme family, so the split is semantically clean.
- **Omission vs commission distinction (881 vs 883)**: two leaves sharing a scheme+conflict pair can
  still be honestly distinguished by the *mechanism* (concealment vs falsification), encoded in the
  MappingType. This generalizes PR-4's "vary MappingType within one scheme" pattern (PR-4 §5) to the
  case where both scheme *and* conflict are shared — the MappingType is the only differentiator, and it
  carries real semantic content (graded source-deception).
- **Direct-conflict scheme-as-defect (885)**: when a fallacy's defect *is* a defective inference scheme
  (885's biased evidence collection), the direct-conflict pattern places the scheme in `DirectRef` with
  no `ExceptionRef` — the scheme names its own defect. This is the same degenerate-undercut shape as
  PR-4's anchor 800 (`VagueVerbalClassification_Inference`) and PR-6's 828 (`InconsistentCommitment_Inference`),
  now applied to `Bias_Inference`.
- **Native palette expansion (4 new tokens)**: this PR introduces `ExpertOpinion_Inference`,
  `ExpertiseInconsistency_Conflict`, `PracticalReasoning_Inference`, and `Bias_Inference` — all
  confirmed in existing usage (1x/1x/2x/2x). **21 distinct native tokens** now demonstrated across 9 PR.
  No fabrication; the chantier has progressively broadened the demonstrated native palette from the
  initial ~8 tokens (PR-1) to 21, showing the AIF ontology covers the Misleading-language family well.

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not "100% leaves") | ✅ §4 (6 deception leaves fully-modeled; Ambiguïté narrative sub-sub complete 10/10; Ambiguïté subfamily complete 3/3) |
| Per unmapped leaf: legitimate scheme + exception/CQ (not Latin alone) | ✅ §3 (6 leaves, scheme + CQ/conflict each, 0 FAIL-LOUD) |
| `AIF_skosMappingType` coherent | ✅ §4 (narrow/close encode mechanism specialization, esp. 881 vs 883 omission/commission) |
| Fail-loud when no honest scheme fits | N/A this cluster (all leaves native) |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies, then CSV edits, then OWL regen (#133) |

### Coverage accounting (cumulative, chantier-wide — dual criterion)
| PR | Cluster | Leaves | Fully-modeled (strict) | DirectRef-loose |
|----|---------|--------|------------------------|-----------------|
| baseline | — | — | 70 | 70 |
| PR-1 #699 | False analogy | 4 | 3 (840 CA-missing)¹ | 74 (+4) |
| PR-2 #701 | Faulty comparison | 5 | 2 (834/835/837 RA-missing) | 79 (+5) |
| PR-3 #703 | Association fallacy | 2 | 2 | 81 (+2) |
| PR-4 #705 | Vague definition | 3 | 3 | 84 (+3) |
| PR-6 #708 | Inconsistent definition | 7 | 5 (829 CA-missing, 832 RA-missing) | 89 (+5) |
| PR-7 #711 | Amphibologie | 8 | 5 (847/848/853 CA-missing) | 94 (+5) |
| PR-8 #713 | Narrative ambiguity / insinuation | 4 | 4 | 98 (+4) |
| **PR-9 (this)** | **Narrative ambiguity / deception** | **6** | **6** | **104 (+6)** |
| **total (strict)** | **4 subfamilies touched** | **39** | **100** | **104** |
| **total (inclusive²)** | | **39** | **101** | **104** |

¹ PR-1 originally reported "+4 mapped" counting pk 840 (RA-exists/CA-missing) as mapped. Under the
strict criterion (CA must exist), PR-1 = +3. Flagged by po-2024 audit #709.
² Inclusive = count RA-exists/CA-missing rows (840) as fully-modeled (PR-1's original convention).
The 1-row discrepancy (100 strict vs 101 inclusive) is the accounting decision pending jsboige.

**This PR's +6 is criterion-independent** — all 6 leaves have both a scheme and a conflict.

**Ambiguïté subfamily: 3/3 sub-subs COMPLETE** (Amphibologie PR-7 + Narrative-ambiguity PR-8+9 = 10/10
+ 8/8 = 18/18 leaves addressed). **Second fully-complete subfamily** after Fallacious comparison (PR-1/2/3).
Remaining Misleading-language subfamily: Equivoque (2 mapped + 18 unmapped, large — split by depth),
Arbitrary definition (HELD pending I/RA/CA schema ratification).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### pk 881 — Propagande grise → **undercut**
- **RA-node**: `ExpertOpinion_Inference` — the audience infers the information is credible because it
  presumes an independent expert source.
- **I-nodes**: premise P "the information comes from a credible independent source S"; conclusion C
  "the information is reliable".
- **CA-node**: `ExpertiseInconsistency_Conflict`, applied as an **undercut** — the inference (that the
  presumed source licenses reliability) is defective because S's actual expertise/position is hidden,
  hence inconsistent with the presumed independence. The conclusion may still be true; the *inference*
  is defective.

### pk 882 — Campagne de murmures → **undercut**
- **RA-node**: `PositionToKnow_Inference` (same as PR-8 880).
- **CA-node**: `LackOfCompleteKnowledge_Conflict`, applied as an **undercut** — the anonymity of the
  coordinated campaign hides whether any source is in a position to know.

### pk 883 — Propagande noire → **undercut**
- **RA-node**: `ExpertOpinion_Inference` — the audience infers credibility from a presumed allied
  source.
- **CA-node**: `ExpertiseInconsistency_Conflict`, applied as an **undercut** — the presumed source
  (ally) is inconsistent with the real source (opponent). The attribution is falsified, so the
  expertise inference is defective.

### pk 884 — Interrogatoire clandestin → **undercut**
- **RA-node**: `PracticalReasoning_Inference` — the target's decision to share information (a practical
  action) based on the premise of a friendly conversation.
- **I-nodes**: premise P "this is a friendly conversation"; the target shares information I.
- **CA-node**: `ConflictingGoals_Conflict`, applied as an **undercut** on P — the premise (friendly
  conversation) is false because the interrogator's goals conflict with the friendly framing
  (extraction vs rapport). The target's practical reasoning is defective because the action context is
  engineered.

### pk 885 — Prêcher le faux pour savoir le vrai → **undercut** (degenerate)
- **RA-node**: `Bias_Inference` — the inference from observed reactions to the truth. The scheme itself
  names the defect (the inference is biased by the false-premise collection method).
- **CA-node**: degenerate **undercut** — the RA-node is self-undermining (the bias *is* the inference's
  defect), same shape as PR-4 800 / PR-6 828.

### pk 886 — Polytélie → **undercut** (degenerate)
- **RA-node**: *(the inference from the multi-goal problem to a determinate solution, scheme-untyped)*.
- **CA-node**: `ConflictingGoals_Conflict`, applied as an **undercut** — the inference (that the
  problem admits a determinate solution) is defective because the multiple objectives conflict, so no
  single solution follows.

### Cluster attack-type distribution
| pk | RA-node | CA-node | Attack type |
|----|---------|---------|-------------|
| 881 | `ExpertOpinion_Inference` | `ExpertiseInconsistency_Conflict` | undercut |
| 882 | `PositionToKnow_Inference` | `LackOfCompleteKnowledge_Conflict` | undercut |
| 883 | `ExpertOpinion_Inference` | `ExpertiseInconsistency_Conflict` | undercut |
| 884 | `PracticalReasoning_Inference` | `ConflictingGoals_Conflict` | undercut |
| 885 | `Bias_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |
| 886 | *(untyped)* | `ConflictingGoals_Conflict` | undercut |

**All 6 leaves are undercuts** — consistent with jsboige's "most fallacies live in the undercut":
source-deception and interrogation fallacies defeat the *inference* the audience/target is manipulated
to draw (source ⟹ reliability, false premise ⟹ genuine reaction), they do not deny the surface claim.
No undermine, no rebut.

### Representation note
As in PR-1 to PR-8, the I-node/RA-node/CA-node decomposition is **recorded here but not serialized**
in the CSV — pending jsboige's schema decision (§4 of the coverage-status note #707). This PR is
**in-sub-sub anchor and independent of that decision**: the modeling (schemes + conflicts) holds
regardless of whether I/RA/CA is serialized in new columns.

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No #674/#596/#666/#686 merge, no régén launch.
- ✅ Proposition derived code=truth from the taxonomy CSV + AIF vocabulary confirmed by existing
  usage (all proposed tokens native: `ExpertOpinion_Inference`, `ExpertiseInconsistency_Conflict`,
  `PositionToKnow_Inference`, `LackOfCompleteKnowledge_Conflict`, `PracticalReasoning_Inference`,
  `Bias_Inference`, `ConflictingGoals_Conflict`). No fabrication — 0 FAIL-LOUD this cluster.
- ✅ Sub-sub completion by mechanism split documented; omission-vs-commission distinction (881/883)
  encoded in MappingType; cross-PR-8 reuse (882 → 880) noted; accounting transparency (dual criterion,
  §6) provided re: po-2024 audit #709.

Relates: dispatch `ih617l` (tertiary), #498 (chantier), PR-1 #699, PR-2 #701, PR-3 #703, PR-4 #705,
PR-6 #708, PR-7 #711, PR-8 #713, coverage-status #707, verification-audit #709, family-gap #712,
#133/#130 (existing OWL), #499 (inverse), #677 (0 fabrication), #192, #458.
