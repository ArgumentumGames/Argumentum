# #498 — AIF Scale-up, Phase 3: Influence + Tricherie + Obstruction depth-3 (27 fallacies, triple-AIF) (GATED proposal)

**Issue:** [#498 — AIF generative scale-up: triple-AIF (RA-node + attack-type + CA-node/CQ) for all depth-3 fallacies](https://github.com/ArgumentumGames/Argumentum/issues/498)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-06-19
**Base:** master `909d04c3`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Fallacies CSV change until jsboige approves.** This document + the annotation CSV are the proposition.

**Calibration:** [`498-aif-scaleup-phase1.md`](498-aif-scaleup-phase1.md) — Phase 1 (11 fallacies, Erreur de raisonnement + Abus de langage) **merged** (`cae93dc8`). [`498-aif-scaleup-phase2.md`](498-aif-scaleup-phase2.md) — Phase 2 (12 fallacies, Erreur mathématique + Insuffisance) **OPEN** (PR #542, CLEAN). This is **Phase 3: the rhetorical/divergence families** (Influence + Tricherie + Obstruction, 27 fallacies) — the rebut-bearing cluster. With phases 1+2 (23 fallacies) and phase 3 (27), AIF depth-3 coverage reaches **50/63 (79%)**.

---

## 1. Scope of Phase 3

Phase 3 extends the triple-AIF annotation to the **rhetorical/divergence families** — the fallacies of persuasion, deception, and discussion-derailment. These are the **rebut-bearing** families: where phases 1–2 (evidence/premise + rule/language families) were undermine/undercut-dominant, phase 3 is where the **rebut** attack-type (absent in phases 1–2) finally appears — for the direct-counter-claim and tu-quoque cases of Obstruction (`Relativisme abusif`, `Évasion`, `Procès en incohérence`).

**27 fallacies** = the 9 depth-3 leaves of each of the 3 families (Influence pks 177–511, Tricherie pks 889–1242, Obstruction pks 1282–1398). All 27 carry no AIF yet — these three families are entirely unannotated before this batch.

| AIF phase | Families | Fallacies | attack-type signature | status |
|---|---|---|---|---|
| Phase 1 | Erreur raisonn. + Abus langage | 11 | undercut-dominant | ✅ merged `cae93dc8` |
| Phase 2 | Erreur math + Insuffisance | 12 | undermine 6 / undercut 6 / rebut 0 | ⏳ PR #542 OPEN CLEAN |
| **Phase 3 (this)** | **Influence + Tricherie + Obstruction** | **27** | **undermine 20 / undercut 4 / rebut 3** | **⏳ this batch** |
| Phase 4 (mopup) | stragglers (4 Abus lang + 3 Err raisonn + 3 Err math + 3 Insuff) | 13 | — | next |
| **Total depth-3** | 7 families | 63 | — | **50/63 after phase 3 (79%)** |

## 2. Schema — identical 12-col triple-AIF schema (phases 1–2, unchanged)

```
fallacy_pk, family, fallacy_name, RA_scheme, attack_type, attacked_component,
CA_node, AIF_RA_node, AIF_CA_node, violated_cq, why_not_others, justification
```

Same conventions as phases 1–2: `RA_scheme` = exact Walton scheme (24-catalog); `attack_type` ∈ {undermine→premise, undercut→inference_rule, rebut→conclusion} (ASPIC+ coherence, attack_type ↔ attacked_component); `CA_node` = CamelCase AIFdb conflict concept; `AIF_RA_node` = scheme + `_Inference`; `AIF_CA_node` = CA_node + `_Conflict`; `violated_cq`/`why_not_others`/`justification` = FR. The eventual prod write (AIF cells to the production Fallacies CSV) is the separate final gated step.

## 3. The triple-AIF paradigm (honored per fallacy, same as phases 1–2)

A fallacy is a **defeasible exception** to a legitimate Walton scheme. The triple-AIF encodes HOW it defeats the scheme's legitimate inference: (1) **RA_scheme** = the canonical Walton scheme the fallacy mimics/abuses (the legitimate move it counterfeits); (2) **attack_type** = the ASPIC+ defeat mode — *undermine* attacks a premise's truth/acceptability, *undercut* attacks the inference rule's applicability, *rebut* raises a counter-conclusion; (3) **CA_node** = the specific critical question violated. The `why_not_others` field forces active per-case discrimination, preventing collapse to a single attack type.

## 4. Method & anti-fabrication guarantee (identical to phases 1–2)

- **Model:** gpt-5.5 (OpenAI direct, key live 2026-06-19). `POST /v1/responses`, `reasoning:{effort:"low"}`, `max_output_tokens:7000`, no `temperature`. Answer read from `output[].content[].text`.
- **Grounding catalog:** the **24 canonical Walton schemes** (Walton/Reed/Macagno 2008, reused verbatim from the phase-1 dataset). The prompt forbids referencing any scheme outside this catalog.
- **ASPIC+ coherence as hard constraint:** every row's `attack_type` ↔ `attacked_component` pair coheres per the undermine→premise / undercut→inference_rule / rebut→conclusion map.
- **Verification (three independent layers, all re-checked against the REAL corpus):**
  1. *Catalog membership* — every `RA_scheme` ∈ the 24 Walton set; `attack_type` ∈ the 3 types; node suffixes `_Inference`/`_Conflict`.
  2. *Ground-truth* — every PK re-verified against the real **1408-row** Fallacies CSV (`PK` + `text_fr` + `Famille` + `depth`); pk ↔ name ↔ family ↔ depth=3 cross-checked; all 27 present, unique, no missing/extra.
  3. *ASPIC+ coherence* — attack_type ↔ attacked_component per the map.
- **Result: 27/27 annotated, 0 violations across all three layers.** [PASS]
- **Anti-fab validator #518** (`tools/validate_taxonomy_annotations.py`): **`✓ CLEAN — 27 rows, 0 HARD, 0 WARN`** (kind=aif-scaleup).

Generation/verify script: `tmp/498_phase3_gen.py` (ephemeral, not committed — derived from the phase-2 `tmp/498_phase2_gen.py`). Dataset: `tmp/498_phase3_dataset.json` (ephemeral). Raw model output: `tmp/498_phase3_gen_result.json` (ephemeral).

## 5. The 27 rows (full set in the CSV)

### 5.1 Distribution: undermine 20 / undercut 4 / rebut 3

| Family | undermine | undercut | rebut | signature |
|---|---|---|---|---|
| Influence (9) | 9 | 0 | 0 | pure persuasion = bad/loaded premise |
| Tricherie (9) | 6 | 3 | 0 | deception = false premise + double-standard rule shifts |
| Obstruction (9) | 5 | 1 | **3** | derailment = source attacks + counter-claims/tu-quoque |

**Phase progression confirms the ASPIC+ prediction.** Phases 1–2 (evidence/premise + rule families) produced **rebut 0/23** — no fallacy raised a direct counter-conclusion; they were all bad premises (undermine) or inapplicable rules (undercut). Phase 3 introduces the **rebut type (3 cases)**, exactly where the rhetorical/divergence families license it: `Relativisme abusif` 1282 (a competing-truth counter-claim), `Évasion` 1313 (substituting a different conclusion), `Procès en incohérence` 1361 (tu-quoque counter). The undermine dominance (20/27) is the Tricherie/Obstruction signature — these families counterfeit legitimate schemes via false/biased premises (Mensonge, fausse attribution, attention sélective, biais, attaque personnelle, empoisonnement du puits) and bare assertions (Preuve par assertion).

### 5.2 The 27 rows

| Fallacy (pk) | Family | Walton scheme | attack → component | CA_node | violated CQ (FR) |
|---|---|---|---|---|---|
| Langage persuasif (177) | Influence | Verbal Classification | undermine → premise | LoadedLanguage | Les termes qui cadrent le débat sont-ils neutres, acceptables et non chargés de présupposés contestables ? |
| Humour (219) | Influence | Popular Opinion | undermine → premise | EmotionalAppeal | La sympathie ou l’amusement suscité par l’orateur repose-t-il sur une raison pertinente et non sur une simple séduction émotionnelle ? |
| Poésie (247) | Influence | Values | undermine → premise | RhetoricalEmbellishment | Les valeurs invoquées sont-elles appuyées par des raisons pertinentes plutôt que par un embellissement stylistique destiné à séduire ? |
| Connivence (300) | Influence | Values | undermine → premise | EmotionalAlliance | L’appel à une valeur commune repose-t-il sur une raison partageable plutôt que sur une alliance émotionnelle avec l’auditoire ? |
| Repoussoir (322) | Influence | Bias | undermine → premise | DerogatoryPortrayal | La caractérisation négative de l’idée ou de ses défenseurs est-elle pertinente et étayée plutôt que simplement repoussante ? |
| Appel aux conséquences (340) | Influence | Consequences | undermine → premise | RelevanceOfConsequences | Les conséquences invoquées sont-elles réelles, pertinentes et suffisantes pour juger la thèse elle-même ? |
| Conditionnement (357) | Influence | Sign | undermine → premise | AssociativeConditioning | Le signe ou l’association invoqué indique-t-il réellement la propriété conclue plutôt qu’une émotion artificiellement conditionnée ? |
| Jeu de pouvoir (420) | Influence | Commitment | undermine → premise | CoercedCommitment | L’engagement attribué à l’interlocuteur est-il librement accepté plutôt qu’imposé par une pression affective ou psychologique ? |
| Influence non verbale (511) | Influence | Sign | undermine → premise | NonVerbalManipulation | Les signes non verbaux utilisés sont-ils des indices pertinents de la thèse plutôt que des moyens de déstabilisation ou de pression ? |
| Mensonge (889) | Tricherie | Position to Know | undermine → premise | PremiseTruth | L’assertion présentée comme connue est-elle vraie et sincèrement soutenue par quelqu’un en position de la savoir ? |
| Fausse attribution (942) | Tricherie | Expert Opinion | undermine → premise | SourceCredibility | La source citée existe-t-elle, est-elle correctement identifiée et possède-t-elle l’expertise pertinente ? |
| Attention sélective (953) | Tricherie | Example | undermine → premise | SelectiveEvidence | Les exemples ou faits retenus sont-ils représentatifs de l’ensemble des données pertinentes, y compris celles qui contredisent la thèse ? |
| Exigence renforcée (974) | Tricherie | Rule | undercut → inference_rule | MovingGoalposts | Le critère ou la règle d’évaluation reste-t-il stable une fois que l’interlocuteur y a satisfait ? |
| Vouloir le beurre et l’argent du beurre (992) | Tricherie | Commitment | undercut → inference_rule | EquivocalCommitment | La position ou l’engagement invoqué est-il suffisamment clair pour permettre d’en tirer une conséquence argumentative contrôlable ? |
| Exigence relâchée (1011) | Tricherie | Rule | undercut → inference_rule | LoweredStandards | Le critère d’évaluation appliqué est-il maintenu au même niveau au lieu d’être abaissé pour sauver la position défendue ? |
| Biais naturels (1024) | Tricherie | Cause to Effect | undermine → premise | AnthropocentricBias | La relation causale ou téléologique invoquée repose-t-elle sur des faits plutôt que sur une projection anthropocentrique ? |
| Biais culturels (1174) | Tricherie | Values | undermine → premise | CulturalBias | L’évaluation proposée repose-t-elle sur des valeurs justifiées plutôt que sur les présupposés particuliers d’un contexte culturel ? |
| Biais théoriques (1242) | Tricherie | Rule | undermine → premise | TheoreticalBias | La règle ou le concept théorique appliqué repose-t-il sur une prémisse adaptée au cas plutôt que sur un cadre rigide et déformant ? |
| Relativisme abusif (1282) | Obstruction | Commitment | **rebut → conclusion** | CounterConclusionConflict | L’affirmation relativiste contredit-elle indûment la conclusion commune en niant qu’une vérité discutable puisse valoir au-delà d’une perspective individuelle ? |
| Pseudo-explication (1287) | Obstruction | Cause to Effect | undermine → premise | PseudoExplanation | L’explication proposée identifie-t-elle réellement des causes pertinentes plutôt que de reformuler ou d’escamoter ce qui doit être expliqué ? |
| Preuve par assertion (1297) | Obstruction | Position to Know | undermine → premise | BareAssertion | L’assertion répétée est-elle appuyée par des raisons ou des connaissances indépendantes plutôt que par sa seule insistance ? |
| Évasion (1313) | Obstruction | Practical Reasoning | **rebut → conclusion** | IssueAvoidance | La réponse proposée traite-t-elle la question à résoudre au lieu de substituer une autre conclusion ou un autre agenda de discussion ? |
| Complication exagérée (1345) | Obstruction | Practical Reasoning | undercut → inference_rule | ExcessiveComplexity | La complexité invoquée empêche-t-elle réellement l’application d’un raisonnement pratique ou sert-elle à rendre indécidable une question traitable ? |
| Empoisonnement du puits (1352) | Obstruction | Bias | undermine → premise | PoisoningTheWell | L’information préjudiciable invoquée établit-elle un biais pertinent de la source plutôt qu’un discrédit préalable et non probant ? |
| Procès en incohérence (1361) | Obstruction | Commitment | **rebut → conclusion** | TuQuoque | L’incohérence alléguée de l’interlocuteur réfute-t-elle réellement sa conclusion plutôt que de lui opposer seulement son comportement ou ses engagements ? |
| Sophisme génétique (1371) | Obstruction | Bias | undermine → premise | GeneticAdHominem | L’origine de l’idée ou de la personne établit-elle un biais pertinent qui affecte le contenu de la thèse ? |
| Attaque personnelle (1398) | Obstruction | Bias | undermine → premise | PersonalAttack | Le trait personnel attaqué établit-il un biais ou une incompétence pertinente relativement à l’argument présenté ? |

### 5.3 Semantic-fidelity highlights

- **Influence (9/9 undermine)**: every Influence fallacy counterfeits a persuasion scheme via a **loaded or emotion-laden premise** — not a false rule, not a counter-conclusion, but a premise whose acceptability is corrupted (loaded language, emotional appeal, derogatory portrayal, coerced commitment, associative conditioning). The CA-nodes carry this specificity: `LoadedLanguage`, `EmotionalAppeal`, `DerogatoryPortrayal`, `CoercedCommitment`, `AssociativeConditioning`. The schemes disperse across the persuasion catalog (Verbal Classification, Popular Opinion, Values ×2, Bias, Consequences, Sign ×2, Commitment) — Influence abuses *every* persuasion scheme, always at the premise layer.
- **Tricherie (undermine 6 / undercut 3)**: deception splits into **false/biased premises** (Mensonge = PremiseTruth, fausse attribution = SourceCredibility, attention sélective = SelectiveEvidence, biais ×3) and **double-standard rule shifts** (Exigence renforcée = MovingGoalposts, Exigence relâchée = LoweredStandards, Vouloir le beurre = EquivocalCommitment) — the undercut cluster is precisely the *special-pleading* trio where the rule of evaluation itself is the problem.
- **Obstruction (undermine 5 / rebut 3 / undercut 1)**: the **only rebut-bearing family**. The 3 rebut cases are the genuine counter-conclusion fallacies — `Relativisme abusif` (competing-truth), `Évasion` (issue substitution), `Procès en incohérence` (tu-quoque) — each raising a conclusion that conflicts with the RA rather than merely denying a premise. The undermine cases are the credibility attacks (`Attaque personnelle` = PersonalAttack, `Sophisme génétique` = GeneticAdHominem, `Empoisonnement du puits` = PoisoningTheWell) and bare assertions (`Preuve par assertion` = BareAssertion). `Complication exagérée` is the lone undercut (ExcessiveComplexity = the practical-reasoning rule becomes inapplicable).

Machine-readable: [`498-scaleup-phase3-annotations.csv`](498-scaleup-phase3-annotations.csv).

## 6. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Fallacies - Taxonomy.csv`. Awaiting jsboige approval.
- ❌ Does **not** touch OWL, EPITA consumer, cards, or mindmaps.
- ❌ Does **not** complete the depth-3 AIF coverage — **13 stragglers remain** (phase 4 mopup: 4 Abus de langage + 3 Erreur de raisonnement + 3 Erreur mathématique + 3 Insuffisance depth-3 leaves not yet annotated).

## 7. Proposed next steps (gated on jsboige)

1. **jsboige validates** the 27 phase-3 rows (paradigm + content + the rebut-type cluster in Obstruction). Note: phase 3 is independent of phase 2's merge — it bases on master and can be reviewed on its own merits.
2. **Phase 4 mopup** (13 stragglers) — the remaining depth-3 fallacies, completing AIF depth-3 coverage to 63/63.
3. On approval of the full AIF set: the **gated prod write** (AIF cells to the production Fallacies CSV) proceeds — writing `RA_scheme`, `attack_type`, `attacked_component`, `CA_node`, `AIF_RA_node`, `AIF_CA_node`, `violated_cq`, `why_not_others`, `justification` to the production CSV.

---

*GATED proposal. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
