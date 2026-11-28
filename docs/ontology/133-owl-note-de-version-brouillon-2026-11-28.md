# Note de version de l'ontologie — BROUILLON (2026-11-28)

> **BROUILLON — aucune version n'est publiée par ce document.** Il n'y a ni tag, ni
> release, ni annonce dans une note de version publique : c'est un brouillon de
> travail, committé dans le dossier du dépôt pour être relu. Le dossier
> `docs/ontology/` est monté en entier par `static.yml` : ce fichier est donc servi
> comme fichier statique — ce n'est pas une publication de version.
> Issue : #133 · file : #458 (P24-47, reprend CL-06) · lane : po-2024.
> Instrument : [`tools/133-owl-release-note.py`](../../tools/133-owl-release-note.py).

## 1. Ce que ce brouillon compare

| | Blob servi (avant) | Blob de master (après) |
|---|---|---|
| Fichier | `docs/ontology/v1.0.0/argumentum_virtues.owl` | `docs/ontology/argumentum_virtues.owl` |
| sha256 | `d4099c79d4abfd139dc622b920938005d67b54727d86c25fa37cf664507bf212` | `884be0f3cea4d3666a6778e3910a38de09648af91f833cc9dde3d4249552a9e1` |
| Octets | 1094832 | 1088889 |
| CSV source | `c1491de9` | `HEAD` |

Les deux blobs sont **ceux du dépôt** : aucun artefact de build n'est requis, la
comparaison est rejouable hors ligne.

## 2. Le changement en une ligne

**114 clés de l'OWL ont changé, sur 89 IRI.** Chaque changement porte son commit ;
aucun n'est sans cause : le script échoue plutôt que de produire une note qui
n'explique pas un changement — il **propage le code de sortie de l'instrument de
delta**, 4 pour un changement d'OWL sans cause dans le CSV.

| Champ | Changements | IRI touchées |
|---|---:|---:|
| `skos:definition@en` | 17 | 17 |
| `skos:prefLabel@en` | 2 | 2 |
| `rdfs:seeAlso@en` | 21 | 21 |
| `rdfs:seeAlso@fr` | 74 | 74 |

## 3. Les changements par IRI

Une IRI par section ; une ligne par champ modifié ; le commit est celui qui a
changé la cellule du CSV responsable.

### `https://www.argumentum.games/argumentum_virtues.owl#groundedArgument`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | Well-structured argument, based on facts or solid reasoning | Well-structured argument, based on facts or solid reasoning. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |

### `https://www.argumentum.games/argumentum_virtues.owl#realArgument`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | Argument based on concrete and verifiable elements, rather than on opinions or assumptions | Argument based on concrete and verifiable elements, rather than on opinions or assumptions. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |

### `https://www.argumentum.games/argumentum_virtues.owl#deductiveArgument`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | Argument that logically derives a conclusion from premises accepted as true | Argument that logically derives a conclusion from premises accepted as true. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |

### `https://www.argumentum.games/argumentum_virtues.owl#inductiveArgument`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | Argument that proposes a probable conclusion from specific examples, while remaining open to revision | Argument that proposes a probable conclusion from specific examples, while remaining open to revision. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Raisonnement_inductif | https://fr.wikipedia.org/wiki/Induction_(logique) | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#tangibleEvidence`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | Evidence supporting the argument through concrete examples, testimonies, or verifiable scientific facts | Evidence supporting the argument through concrete examples, testimonies, or verifiable scientific facts. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Preuve_(logique) | https://fr.wikipedia.org/wiki/Preuve | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#clearObjective`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | Precise establishment of the debate objective and the parties' positions | Precise establishment of the debate objective and the parties' positions. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |

### `https://www.argumentum.games/argumentum_virtues.owl#providingExamples`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | Support for argumentation through specific and relevant examples to make it more understandable | Support for argumentation through specific and relevant examples to make it more understandable. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |

### `https://www.argumentum.games/argumentum_virtues.owl#citingSources`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Référence_bibliographique | `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#unbiasedReasoning`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | Reasoning based on rational logic and facts that avoids preconceived conclusions | Reasoning based on rational logic and facts that avoids preconceived conclusions. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |

### `https://www.argumentum.games/argumentum_virtues.owl#reliablePremises`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | Bases of an argument considered solid and credible | Bases of an argument considered solid and credible. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |

### `https://www.argumentum.games/argumentum_virtues.owl#credibleSources`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Fiabilit%C3%A9_des_sources | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#wellEvaluatedSource`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Fiabilit%C3%A9_des_sources#M%C3%A9thodes_d'%C3%A9valuation_de_la_fiabilit%C3%A9_des_sources | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#competentAuthority`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Autorit%C3%A9_cognitive | https://fr.wikipedia.org/wiki/Crédibilité | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#validatedConclusion`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Preuve_science | https://fr.wikipedia.org/wiki/Preuve | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#confirmedInformation`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Journalisme | `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#empiricalEvidence`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Preuve_empirique | `2bb7958b` (2026-10-14) fix(corpus): #458 virtues linkless sourcing — fill 7 of 8 #1851-newly-linkless nodes (#1880) |
| `rdfs:seeAlso@en` | — | https://en.wikipedia.org/wiki/Empirical_evidence | `2bb7958b` (2026-10-14) fix(corpus): #458 virtues linkless sourcing — fill 7 of 8 #1851-newly-linkless nodes (#1880) |

### `https://www.argumentum.games/argumentum_virtues.owl#impartialSources`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | Selection of objective sources, free of personal interest, for an argumentation grounded in truth rather than opinion. | Objective sources, free of personal interest, for argumentation grounded in facts rather than opinion. | `da1251e3` (2026-10-21) fix(virtues): #458 Sonnet pass 3 — 23/24 kept cells (cascade drift, pk 0-33) (#1928) |
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Impartialité | `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#sourcesFreeOfSocioEconomicBias`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Source_impartiale | https://fr.wikipedia.org/wiki/Impartialité | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#fairInterpretation`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Interpr%C3%A9tation_(philosophie) | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#nonPartisanInterpretation`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Objectivit%C3%A9_(philosophie) | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#justifiedTechnicalTerminology`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Terme_technique | https://fr.wikipedia.org/wiki/Jargon | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#emotionalBalance`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Distanciation_(psychologie) | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#emotionalNeutrality`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Connivence_(psychologie) | https://fr.wikipedia.org/wiki/Équanimité | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `6ddac169` (2026-10-16) fix(corpus): #458 virtues re-curation pk 52 + pk 102 — fix wrong source referents, 12 cells via langlinks (#1887) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Collusion | https://en.wikipedia.org/wiki/Equanimity | `6ddac169` (2026-10-16) fix(corpus): #458 virtues re-curation pk 52 + pk 102 — fix wrong source referents, 12 cells via langlinks (#1887) |

### `https://www.argumentum.games/argumentum_virtues.owl#noBlackmailThroughConsequences`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | Manipulating one's interlocutor by resorting to threats or promises constitutes an affront to honest and ethical exchange. | Refusing to manipulate one's interlocutor through threats or promises linked to consequences. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Chantage_émotionnel | `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#quantitativeReasoning`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/M%C3%A9thode_math%C3%A9matique | https://fr.wikipedia.org/wiki/Numératie | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Mathematical_method | https://en.wikipedia.org/wiki/Numeracy | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `2bb7958b` (2026-10-14) fix(corpus): #458 virtues linkless sourcing — fill 7 of 8 #1851-newly-linkless nodes (#1880) |

### `https://www.argumentum.games/argumentum_virtues.owl#justifiedGeneralizations`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/G%C3%A9n%C3%A9ralisation_%28logique%29 | https://fr.wikipedia.org/wiki/Généralisation | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#representativeSampling`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/%C3%89chantillonnage_statistique | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#legitimateTransfer`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Transfert_(logique) | https://fr.wikipedia.org/wiki/Analogie | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `2bb7958b` (2026-10-14) fix(corpus): #458 virtues linkless sourcing — fill 7 of 8 #1851-newly-linkless nodes (#1880) |
| `rdfs:seeAlso@en` | — | https://en.wikipedia.org/wiki/Argument_from_analogy | `2bb7958b` (2026-10-14) fix(corpus): #458 virtues linkless sourcing — fill 7 of 8 #1851-newly-linkless nodes (#1880) |

### `https://www.argumentum.games/argumentum_virtues.owl#establishedCorrelations`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Corr%C3%A9lation_et_d%C3%A9pendance#Relation_de_causalit%C3%A9 | https://fr.wikipedia.org/wiki/Corrélation_n'implique_pas_causalité | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#methodicalEstimation`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Estimation_statistique | https://fr.wikipedia.org/wiki/Théorie_de_l'estimation | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#finiteSupport`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Espace_probabilis%C3%A9_fini | https://fr.wikipedia.org/wiki/Univers_(probabilit%C3%A9s) | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `2bb7958b` (2026-10-14) fix(corpus): #458 virtues linkless sourcing — fill 7 of 8 #1851-newly-linkless nodes (#1880) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Finite_probability_space | https://en.wikipedia.org/wiki/Sample_space | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `2bb7958b` (2026-10-14) fix(corpus): #458 virtues linkless sourcing — fill 7 of 8 #1851-newly-linkless nodes (#1880) |

### `https://www.argumentum.games/argumentum_virtues.owl#clearPresentationOfData`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Qualit%C3%A9_de_la_donn%C3%A9e | https://fr.wikipedia.org/wiki/Qualité_des_données | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#exactCalculations`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Pr%C3%A9cision_num%C3%A9rique | https://fr.wikipedia.org/wiki/Précision | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#numericalAccuracy`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Pr%C3%A9cision_num%C3%A9rique | https://fr.wikipedia.org/wiki/Précision | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#inferentialMastery`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Raisonnement_valide | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Valid_reasoning | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#independenceOfPremisesFromTheConclusion`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Raisonnement_valide | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Valid_reasoning | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#rigorousConstructionOfStatements`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Raisonnement_valide#Structure_logique_de_l'argument_valide | https://fr.wikipedia.org/wiki/Validité_(logique) | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#wellFormedPropositionalStatements`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Proposition_(logique) | https://fr.wikipedia.org/wiki/Théorème | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#correctDeductions`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Raisonnement_valide | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Valid_reasoning | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#stepwiseReasoning`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | Progressive reasoning method using a series of intermediate inferences to reach a rigorous conclusion | Progressive reasoning method using a series of intermediate inferences to reach a rigorous conclusion. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Raisonnement_conjectural | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Conjectural_reasoning | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#coherentDemonstration`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | Rigorous and logical sequence of arguments that does not reveal inconsistencies between its different parts | Rigorous and logical sequence of arguments that does not reveal inconsistencies between its different parts. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |

### `https://www.argumentum.games/argumentum_virtues.owl#absenceOfInternalContradictions`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Coh%C3%A9rence_(philosophie) | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Coherence_(philosophy) | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#inference:ConjunctionIntroduction`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Conjonction_(logique) | https://fr.wikipedia.org/wiki/Conjonction_logique | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#inference:ConjunctionElimination`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Disjonction_(logique) | https://fr.wikipedia.org/wiki/Disjonction_logique | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#inference:DoubleNegation`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/N%C3%A9gation_(logique)#Double_n.C3.A9gation | https://fr.wikipedia.org/wiki/N%C3%A9gation_logique | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#inference:UnitResolution`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Raisonnement_synth%C3%A9tique | https://fr.wikipedia.org/wiki/Règle_de_résolution | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `6ddac169` (2026-10-16) fix(corpus): #458 virtues re-curation pk 52 + pk 102 — fix wrong source referents, 12 cells via langlinks (#1887) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Synthetic_reasoning | https://en.wikipedia.org/wiki/Resolution_(logic) | `6ddac169` (2026-10-16) fix(corpus): #458 virtues re-curation pk 52 + pk 102 — fix wrong source referents, 12 cells via langlinks (#1887) |

### `https://www.argumentum.games/argumentum_virtues.owl#datisiSyllogism`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Syllogisme_Datisi | https://fr.wikipedia.org/wiki/Syllogisme#Les_variantes_du_syllogisme_standard | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `2bb7958b` (2026-10-14) fix(corpus): #458 virtues linkless sourcing — fill 7 of 8 #1851-newly-linkless nodes (#1880) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Datisi | https://en.wikipedia.org/wiki/Syllogism#Standard-form_syllogisms | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `2bb7958b` (2026-10-14) fix(corpus): #458 virtues linkless sourcing — fill 7 of 8 #1851-newly-linkless nodes (#1880) |

### `https://www.argumentum.games/argumentum_virtues.owl#camenesSyllogism`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Syllogisme_cam%C3%A9nien | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Syllogism_caménien | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#dimatisSyllogism`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Syllogisme_dimaris | https://fr.wikipedia.org/wiki/Syllogisme | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#fesapoSyllogism`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Syllogisme_de_mode_permis | https://fr.wikipedia.org/wiki/Logique_traditionnelle | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#fresisonSyllogism`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Syllogisme_de_mode_permis | https://fr.wikipedia.org/wiki/Logique_traditionnelle | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#bamalipSyllogism`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Syllogisme_de_mode_permis | https://fr.wikipedia.org/wiki/Logique_traditionnelle | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#acceptableInformalLogic`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:prefLabel@en` | Acceptable informal logic | Solid informal logic | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |

### `https://www.argumentum.games/argumentum_virtues.owl#correctAnalyticalReasoning`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Raisonnement_analytique | https://fr.wikipedia.org/wiki/Raison | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#correctSyntheticReasoning`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Raisonnement_synth%C3%A9tique | https://fr.wikipedia.org/wiki/Distinction_analytique-synthétique | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#correctAbductiveReasoning`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Raisonnement_abductif | https://fr.wikipedia.org/wiki/Abduction_(logique) | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#lexicalPrecision`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Langage_rigoureux | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Rigorous_language | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#conformingDefinition`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Terminologie_technique | https://fr.wikipedia.org/wiki/Jargon | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#appropriateTechnicalTerminology`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Terminologie_technique | https://fr.wikipedia.org/wiki/Jargon | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#consistentDefinition`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Définition | `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#justifiedComparisons`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Analogie | `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#adequateComparison`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Analogie | `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#relevantAssociation`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Sophisme_d%27association | https://fr.wikipedia.org/wiki/Sophisme_par_association | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#clearSyntax`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Syntaxe | `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#unambiguousExpressions`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Ambigu%C3%AFt%C3%A9_(linguistique) | https://fr.wikipedia.org/wiki/Ambiguïté | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#intellectualHonesty`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Honn%C3%AAtet%C3%A9_intellectuelle | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#truthOfFacts`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Vérité | `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#balancedConsideration`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Objectivit%C3%A9_(presse) | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#fullDisclosure`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Transparence_(droit) | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#clarityOfIssues`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Cadrage_(sciences_sociales) | `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#appropriateLevelOfProof`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | The expected objective for the opponent must be attainable and fair, with measurable, clear and well-defined criteria for assessing success. | Requirement of clear, measurable and fair criteria for evaluating the success of an argumentation. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |

### `https://www.argumentum.games/argumentum_virtues.owl#clearPositioning`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Honn%C3%AAtet%C3%A9_intellectuelle | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#nonComplacentObjective`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Objectif_SMART | https://fr.wikipedia.org/wiki/Objectifs_et_indicateurs_SMART | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#objectivityEffort`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Objectivit%C3%A9_(philosophie) | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#interculturalAwareness`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Ouverture_culturelle | https://fr.wikipedia.org/wiki/Comp%C3%A9tence_interculturelle | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `2bb7958b` (2026-10-14) fix(corpus): #458 virtues linkless sourcing — fill 7 of 8 #1851-newly-linkless nodes (#1880) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Cultural_open-mindedness | https://en.wikipedia.org/wiki/Intercultural_competence | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `2bb7958b` (2026-10-14) fix(corpus): #458 virtues linkless sourcing — fill 7 of 8 #1851-newly-linkless nodes (#1880) |

### `https://www.argumentum.games/argumentum_virtues.owl#recognitionOfPersonalCulturalBiases`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Biais_culturel | `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#distancingFromIdeologies`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Neutralité_(relations_internationales) | `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#takingTheOpponentsIdeologicalBiasesIntoAccount`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:prefLabel@en` | Taking the opponent's ideological biases into account | Taking the parties' ideological biases into account | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |

### `https://www.argumentum.games/argumentum_virtues.owl#enrichingExchange`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Discussion_constructive | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Constructive_dialogue | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#positiveContribution`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Discussion_constructive | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Constructive_discussion | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#recognitionOfAnObjectiveReality`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Constructive_discussion | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#identificationOfOnesBiases`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Biais_cognitif | `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#opennessToChange`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | — | https://fr.wikipedia.org/wiki/Ouverture_d'esprit | `8253cd3e` (2026-10-15) fix(corpus): #458 virtues link_fr — fill 41 empty cells from fr langlinks, 140 → 181 (#1885) |

### `https://www.argumentum.games/argumentum_virtues.owl#focusOnTheEssentials`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/M%C3%A9thode_PRIOR | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/PRIOR_method | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#takingAllottedTimeIntoAccount`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/M%C3%A9thode_PRIOR | https://fr.wikipedia.org/wiki/Gestion_du_temps | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `2bb7958b` (2026-10-14) fix(corpus): #458 virtues linkless sourcing — fill 7 of 8 #1851-newly-linkless nodes (#1880) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/METHOD_PRIOR | https://en.wikipedia.org/wiki/Time_management | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) → `2bb7958b` (2026-10-14) fix(corpus): #458 virtues linkless sourcing — fill 7 of 8 #1851-newly-linkless nodes (#1880) |

### `https://www.argumentum.games/argumentum_virtues.owl#principleOfCharity`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | An intellectually honest approach that adopts the strongest and most coherent interpretation of the interlocutor's arguments. | Adopt the strongest and most coherent interpretation of the interlocutor's arguments. | `edb39554` (2026-10-08) fix(virtues): #458 pk 205 — cadrage "honnête intellectuellement" retiré (EN/ar/fa) (#1806) |

### `https://www.argumentum.games/argumentum_virtues.owl#respectForThePerson`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `rdfs:seeAlso@fr` | https://fr.wikipedia.org/wiki/Valeurs_de_la_R%C3%A9publique_fran%C3%A7aise#Respect_de_la_personne_humaine | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |
| `rdfs:seeAlso@en` | https://en.wikipedia.org/wiki/Respect_for_personhood | — | `059f219f` (2026-10-10) fix(corpus): #458 grain ③ — Virtues Wikipedia : 204 liens morts (26,4 %), fabriqués à la génération, 246 cellules traitées (#1851) |

### `https://www.argumentum.games/argumentum_virtues.owl#doNotInterrupt`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | It is important to let each participant speak without interruption in order to allow complete expression and attentive listening | It is important to let each participant speak without interruption in order to allow complete expression and attentive listening. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |

### `https://www.argumentum.games/argumentum_virtues.owl#respectfulTone`

| Champ | Avant | Après | Commit |
|---|---|---|---|
| `skos:definition@en` | It is important to use a cordial and non-aggressive tone to express one's point of view | It is important to use a cordial and non-aggressive tone to express one's point of view. | `371dd730` (2026-10-07) fix(virtues): #458 grain 10 — 28 cellules de fidelite (pk 54, 162, 128, 176 + 13 points finaux EN) (#1795) |

## 4. Ce qui n'a pas changé

- **Aucune IRI perdue** : la surface d'IRI passe de 224 à 224 (0 perdue, 0 gagnée).
- **Aucun changement d'OWL sans cause dans le CSV** (0), aucune cause sans effet (0), aucune clé hors oracle (0).
- **Aucun libellé, définition ni commentaire en français n'a changé** : les changements français sont tous des liens (`rdfs:seeAlso`).
- **L'autre artefact du dossier est identique octet pour octet** (`docs/ontology/argumentum.owl`, sha256 `e6da4eb8…`) : rien à y raconter.

## 5. Comment le rejouer

```bash
# le delta, seul (l'instrument écrit son rapport JSON)
python tools/133-owl-regen-delta.py --repo . \
  --old docs/ontology/v1.0.0/argumentum_virtues.owl --csv-old c1491de9 \
  --new docs/ontology/argumentum_virtues.owl --csv-new HEAD \
  --attribute --json <delta.json>

# le brouillon (appelle le delta ci-dessus, puis rattache chaque changement à son commit)
python tools/133-owl-release-note.py --repo . \
  --old docs/ontology/v1.0.0/argumentum_virtues.owl --csv-old c1491de9 \
  --new docs/ontology/argumentum_virtues.owl --csv-new HEAD \
  --other-old docs/ontology/v1.0.0/argumentum.owl --other-new docs/ontology/argumentum.owl \
  --date 2026-11-28 --out docs/ontology/133-owl-note-de-version-brouillon-2026-11-28.md

# le brouillon est-il à jour ? LA MÊME COMMANDE, --check en plus.
python tools/133-owl-release-note.py --repo . \
  --old docs/ontology/v1.0.0/argumentum_virtues.owl --csv-old c1491de9 \
  --new docs/ontology/argumentum_virtues.owl --csv-new HEAD \
  --other-old docs/ontology/v1.0.0/argumentum.owl --other-new docs/ontology/argumentum.owl \
  --date 2026-11-28 --out docs/ontology/133-owl-note-de-version-brouillon-2026-11-28.md --check
```

Ces commandes se lancent **depuis la racine du dépôt** : `--old` et `--new` sont
ouverts relativement au répertoire courant, pas à `--repo`. Sans
`--other-old`/`--other-new`, la section « ce qui n'a pas changé » perd sa ligne sur
l'autre artefact — et `--check` répond alors « brouillon perime » sur un arbre
correct.

Le delta vient de [`tools/133-owl-regen-delta.py`](../../tools/133-owl-regen-delta.py) :
l'attendu est **calculé depuis le CSV**, la valeur constatée est lue dans le XML —
l'instrument ne peut pas se donner raison à lui-même. Le rattachement à un commit
relit le CSV à **chaque commit** qui l'a touché dans la plage : le commit d'une
transition est celui qui a changé cette cellule, pas une recherche de chaîne qui
tomberait sur la même valeur dans une autre cellule.

**Ce contrôle n'est pas câblé en CI** : il lit l'historique du CSV (`c1491de9`), qu'un
checkout superficiel ne contient pas. Le câbler suppose un job à `fetch-depth: 0` —
à faire, ce n'est pas fait ici.
