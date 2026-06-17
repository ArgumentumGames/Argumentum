# #498 — AIF Generative Layer Pilot (GATED proposal)

**Issue:** [#498 — AIF génératif: structure d'exception pour sophismes sans analogue AIF](https://github.com/ArgumentumGames/Argumentum/issues/498)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-16
**Base:** master `d780774e`
**Status:** **GATED PROPOSAL** — ai-01 reviews structure, jsboige validates content. **No production Fallacies CSV change until jsboige approves.** This document + the pilot CSV are the proposal.

---

## 1. The gap & the reformulation (per ai-01)

Only **42/1408** Fallacies nodes carry a direct `AIF_skosDirectRef` (2.9% direct; ~31% effective via grappe-level inheritance). ai-01's reformulation (verified read-only by po-2024) settled the scope:

> *"Le chantier n'est PAS « augmenter la couverture » (impossible depuis AIF, ~20 schemes natifs), mais **génératif** — pour les sophismes sans analogue AIF, rédiger la **structure d'exception** (scheme + CQ) au lieu d'un nom latin."*

AIF's value is **not** its ~20-node catalog — it's the **exception structure**: a fallacy is a *defeasible exception to a legitimate Walton scheme*, escaping one of the scheme's critical questions (CQ). A latin name is consumable by any LLM 0-shot; the *« exception to scheme S / violated CQ Q »* mechanism is not — that's the generative layer this pilot produces.

## 2. Approach — exception-structure annotation

For each pilot fallacy, generate four fields:

| Field | Meaning | Grounding |
|-------|---------|-----------|
| `walton_scheme` | The canonical Walton scheme this fallacy is an exception to | Must be an exact name from the canonical catalog (§4) |
| `violated_critical_question` | The specific CQ of that scheme the fallacy violates | Articulated in the style of Walton's CQs (FR) |
| `exception_mechanism` | How the fallacy derails the scheme by evading that CQ | Generated, FR |
| `justification` | One line linking them | Generated, FR |

This **deliberately does not** invent new AIF corpus node names. It documents the generative exception structure directly — the high-value, non-trivial layer.

## 3. The pilot — 18 fallacies (no prior AIF, all 7 families)

**Selection:** 18 well-known depth-3 fallacies that currently have **no** `AIF_skosDirectRef`, ~2-3 per family, chosen where the Walton exception mapping is demonstrable.

| PK | Fallacy | Walton scheme (exception to) | Violated critical question |
|----|---------|------------------------------|----------------------------|
| 55 | Sauvetage ad hoc | Argument from Rule | L'exception invoquée est-elle prévue par des raisons indépendantes, ou introduite seulement pour sauver la conclusion ? |
| 96 | Appel à la nature | Argument from Values | La valeur « naturel » est-elle pertinente pour établir que la chose est meilleure, malgré sécurité/efficacité/justice ? |
| 112 | Sophisme moraliste | Argument from Values | La valeur morale invoquée est-elle pertinente pour la vérité factuelle, ou seulement pour l'évaluer/condamner ? |
| 322 | Repoussoir | Argument from Values | La valeur négative rendant l'idée repoussante est-elle pertinent pour l'évaluer rationnellement ? |
| 340 | Appel aux conséquences | Argument from Consequences | Les conséquences invoquées sont-elles pertinentes pour la vérité/falsité de la proposition ? |
| 596 | Échantillon biaisé | Argument from Example | L'échantillon est-il représentatif de la population cible de la conclusion ? |
| 644 | Probabilités faussées | Argument from Sign | La probabilité est-elle fiable, correctement calculée et pertinente ? |
| 698 | Pétition de principe | Argument from Commitment | La prémisse-engagement est-elle acceptée indépendamment de la conclusion à prouver ? |
| 707 | Inversion de causalité | Argument from Cause to Effect | La direction causale est-elle correctement établie, plutôt qu'inversée ? |
| 784 | Syllogisme invalide | Argument from Rule | La règle + le cas autorisent-ils réellement la conclusion tirée ? |
| 834 | Comparaison abusive | Argument from Analogy | Existe-t-il des différences pertinentes rendant l'analogie trompeuse ? |
| 847 | Amphibologie | Argument from Verbal Classification | La formulation attribue-t-elle clairement, sans ambiguïté, le prédicat au cas ? |
| 855 | Équivoque | Argument from Verbal Classification | Le terme a-t-il le même sens dans les prémisses et la conclusion ? |
| 889 | Mensonge | Argument from Witness Testimony | Le témoin est-il sincère et dit-il la vérité telle qu'il la connaît ? |
| 942 | Fausse attribution | Argument from Expert Opinion | La source a-t-elle affirmé ceci et possède-t-elle l'expertise pertinente ? |
| 974 | Exigence renforcée | Argument from Commitment | L'engagement est-il stable, explicite et cohérent tout au long de l'échange ? |
| 1352 | Empoisonnement du puits | Argument from Bias | Le biais allégué est-il pertinent pour la fiabilité sur *cette* affirmation précise ? |
| 1361 | Procès en incohérence | Argument from Commitment | Les engagements attribués impliquent-ils réellement la contradiction alléguée ? |

Several are textbook-classic mappings (Appeal to Consequences→Consequences, False Analogy→Analogy, Equivocation/Amphiboly→Verbal Classification, Poisoning the Well→Bias, Causal Inversion→Cause to Effect, Biased Sample→Example). Full `exception_mechanism` + `justification` per row: [`498-pilot-annotations.csv`](498-pilot-annotations.csv).

## 4. Method & anti-fabrication guarantee

- **Grounding catalog (canonical Walton schemes, Walton/Reed/Macagno 2008):** Argument from Position to Know · Expert Opinion · Witness Testimony · Popular Opinion · Example · Analogy · Correlation to Cause · Cause to Effect · Sign · Consequences · Slippery Slope · Rule · Practical Reasoning · Commitment · Values · Verbal Classification · Bias · Danger · Need for Help · Ignorance · Composition/Division · Precedent · Gradualism · Waste. The model must pick an **exact** name.
- **Model / endpoint:** gpt-5.5 via **`/v1/responses`** with `reasoning:{effort:"low"}` — the cluster's proven method for reliable content output (chat/completions burns the budget in reasoning_tokens and can return empty; effort:low yielded clean JSON at **~68 reasoning tokens/call**). One call per fallacy.
- **Verification (3 layers):**
  1. `walton_scheme` ∈ catalog (reject fabricated scheme names),
  2. `fallacy_pk` ∈ real 1408-row CSV (reject hallucinated PKs),
  3. all 18 confirmed **no prior AIF** in the source CSV (the generative-layer claim holds).
- **Result: 0 PK violations, 0 scheme violations. PASS — all schemes canonical, all PKs real.** Scheme distribution is healthy: **12 distinct schemes** across 18 fallacies (no over-concentration).

Generation script: `tmp/498_pilot_gen.py` (ephemeral). Raw output: `tmp/498_pilot_annotations.json` (available on request).

## 5. What this does NOT do (gate boundaries)

- ❌ Does **not** edit the production `Argumentum Fallacies - Taxonomy.csv` (no `AIF_skos*` cells filled, no rows changed).
- ❌ Does **not** invent AIF corpus node names — it documents the exception structure, sidestepping AIF's ~20-node ceiling per ai-01's directive.
- ❌ Does **not** touch the OWL ontology, cards, mindmaps, or any consumer.
- ❌ Does **not** claim final content — it is the **calibration exemplar** for scaling to the ~1366 no-AIF fallacies.

## 6. Proposed next steps (gated on jsboige)

1. **jsboige validates** (a) the exception-structure framing (scheme + CQ, not AIF-node mapping), (b) the 18 scheme assignments in §3, (c) the CQ phrasings.
2. On approval: populate the ~1366 no-AIF Fallacies nodes via a DatasetUpdater gpt-5.5 task, with this pilot as few-shot calibration + the same catalog-grounding + 3-layer verify. The `AIF_skosMappingType` cell would hold the scheme + violated CQ (compact form), `AIF_skosDirectRef` left to a curated AIF-corpus match where one genuinely exists.
3. Propagate to the OWL ontology (AIF annotations on fallacy nodes) → feeds the **EPITA Restitution Epic #1134** (the readable report that *narrates* the exception rather than citing a latin name).

## 7. Consumer benefit

The exception structure is the layer with the highest downstream value: it lets a consumer (EPITA harness, CoursIA pedagogy, the restitution report) explain *why* a fallacy fails — "it is an appeal to consequences that evades CQ: *are the consequences relevant to the proposition's truth?*" — instead of naming it « argumentum ad consequentiam ». ai-01's cross-workspace synchro (EPITA × Argumentum × CoursIA) frames this as the readable debouché of the formal verification EPITA runs.

---

*GATED proposal. Worker signals structure + grounding; ai-01 reviews, jsboige validates content. No production data changed.*
