# 2026-07-06 — #498 AIF chantier, PR-10 : cluster « Equivoque / Polysémie » (lexical multi-meaning)

**Scope**: tenth cluster of the #498 chantier — first half of the **Equivoque** sub-sub (Ambiguïté
subfamily, Abus de langage / Misleading language family). Models the **lexical polysemy mechanism**
(pk 855 anchor + 860/862/863/864 leaves, using 856 as mapped template). **Proposition only — GATED,
0 write to prod CSV.** Triggered by ai-01 dispatch `5ymo70` (primary: Equivoque sub-sub, split by
mechanism). **In-sub-sub anchor shape — independent of the I/RA/CA serialization decision.**

**Repo reference**: master `bdba45d8`. Issue: #498. Predecessors: PR-1 #699, PR-2 #701, PR-3 #703,
PR-4 #705, PR-6 #708, PR-7 #711 (Amphibologie), PR-8 #713, PR-9 #714 (**Ambiguïté subfamily complete
3/3**).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies →
> worker applies the CSV edits in a follow-up PR (gated).

---

## TL;DR

The **Equivoque** sub-sub (`Soussousfamille=Equivoque`) has **2 mapped rows** — pk 856 (d4 "Expression
vague", MAPPED `VagueVerbalClassification_Inference`, direct-conflict, `skos:broadMatch`) and pk 858
(d5 "Pente glissante sémantique", MAPPED `VerbalSlipperySlope_Inference` exception-ref, `skos:closeMatch`)
— and **18 unmapped leaves** (pk 855 anchor d3 + 857, 859-875). The sub-sub is the **largest in the
chantier** (21 rows) and semantically spans **three distinct mechanisms**, so it is split by mechanism
(this PR = lexical polysemy; PR-11 = continuum/sorite + misc; PR-12 = reification/magic-of-words).

**PR-10 scopes to the lexical-polysemy mechanism**: anchor pk 855 + template pk 856 (recap) + 4 leaves
(860 glissement polysémique, 862 homonymie, 863 hétérosémie, 864 jingle-jangle). All 5 modeled rows
honestly reuse the template 856's `VagueVerbalClassification_Inference` — the lexical-polysemy defect
*is* a vague verbal classification (the term's extension is not fixed, the arguer exploits one reading).
The only variation is the `MappingType`, encoding how tightly each leaf specializes the polysemy
mechanism.

**5 rows proposed, 5 fully-modeled, 0 FAIL-LOUD** — the third consecutive clean cluster (after PR-8,
PR-9). This PR opens the Equivoque sub-sub; the continuum (PR-11) and reification (PR-12) mechanisms
are deferred.

---

## 1. Cluster state (code=truth)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Abus de langage** (Misleading
language), subfamily **Ambiguïté**, sub-sub **Equivoque**:

| pk | text_fr | depth | DirectRef | ExceptionRef | MappingType | State |
|----|---------|-------|-----------|--------------|-------------|-------|
| 855 | Équivoque | d3 | — | — | — | unmapped (d3 anchor, **PR-10**) |
| **856** | **Expression vague** (mapped template) | d4 | `VagueVerbalClassification_Inference` | — | `skos:broadMatch` | **✅ MAPPED (PR-10 template)** |
| 860 | Glissement lexical polysémique | d4 | — | — | — | unmapped (**PR-10**) |
| 862 | Homonymie | d5 | — | — | — | unmapped (**PR-10**) |
| 863 | Hétérosémie | d6 | — | — | — | unmapped (**PR-10**) |
| 864 | Sophisme jingle-jangle | d5 | — | — | — | unmapped (**PR-10**) |

The mapped template (856) is the **paradigm**: an equivocal term = a `VagueVerbalClassification_Inference`
(a verbal classification where the term's extension is not fixed, so the arguer can exploit any reading).
The lexical-polysemy leaves (855, 860, 862, 863, 864) are all specializations of this — they vary by
*how* the multi-meaning arises (drift / unrelated-homonymy / context-dependence / bidirectional
confusion). The other mechanisms (continuum 858/857/859, reification 867-875) are deferred to PR-11/12.

---

## 2. The template model (856, the paradigm)

**Scheme**: `VagueVerbalClassification_Inference` (AIF native, confirmed 2x in existing usage — used as
PR-4 anchor too). A verbal classification (grouping/labeling items via a term) where the term's
extension is not fixed, allowing the arguer to shift reading mid-discussion.

**desc_fr** (856): "Vous utilisez des termes si vagues qu'ils ne permettent pas de comprendre
clairement ce que vous voulez dire."

**Pattern**: direct-conflict (the scheme is in DirectRef, no ExceptionRef — the equivocation *is* the
vague classification). This is the same shape as PR-4's anchor 800 (`VagueVerbalClassification_Inference`
direct-conflict) — the Equivoque sub-sub's lexical-polysemy mechanism is the term-level cousin of the
Inexact-definition subfamily's vagueness mechanism. The two sub-subs honestly share the scheme; the
distinction is *family* (ambiguity vs definition), not *scheme*.

---

## 3. Proposed AIF structure for the leaves

For each leaf: reuse the template 856's scheme `VagueVerbalClassification_Inference`, vary the
`MappingType` by the leaf's specialization. Vocabulary restricted to AIF-native tokens.

### pk 855 — Équivoque (Equivocation, d3 anchor) ✅
- **desc_fr**: "Vous exploitez les différents sens d'un même mot pour rendre votre propos ambigu."
- **Mechanism**: the *core* of the template — a term with multiple senses, exploited to keep the
  argument ambiguous. The anchor names the whole sub-sub's mechanism in general terms.
- **Proposal**: `DirectRef=VagueVerbalClassification_Inference`, `ExceptionRef=` *(direct-conflict
  pattern, mirroring 856)*, `MappingType=skos:closeMatch` (the anchor *is* the abstract case of the
  template 856 — direct sibling, framing the whole sub-sub).

### pk 860 — Glissement lexical polysémique (Polysemic drift) ✅
- **desc_fr**: "Vous employez un mot polysémique ou dont le sens a évolué, ce qui peut prêter à
  confusion."
- **Mechanism**: polysemy by **active drift** — the arguer slides between the senses of a polysemic
  term mid-argument. The *active* form of equivocation (the arguer exploits the drift), as opposed to
  862 (passive existence of homonymy).
- **Proposal**: `DirectRef=VagueVerbalClassification_Inference`, `ExceptionRef=`,
  `MappingType=skos:closeMatch` (a direct variant: polysemy exploited *via active drift mid-argument*,
  the canonical equivocation case — closest sibling of 855).

### pk 862 — Homonymie (Homonymy) ✅
- **desc_fr**: "Vous construisez votre argument en utilisant un terme à plusieurs sens, ce qui sème la
  confusion."
- **Mechanism**: polysemy by **unrelated-homonymy** — the term has multiple unrelated senses (e.g.
  "voler" = steal / fly), and the arguer trades on the coincidence. A *broader* phenomenon than 860's
  active drift: any homonymic term, not just actively-exploited drift.
- **Proposal**: `DirectRef=VagueVerbalClassification_Inference`, `ExceptionRef=`,
  `MappingType=skos:broadMatch` (a broader case: any homonymic term admitted, not just
  actively-drifted polysemy — the broadest lexical-polysemy leaf).

### pk 863 — Hétérosémie (Heterosemy) ✅
- **desc_fr**: "Vous basez votre argument sur un mot qui peut signifier plusieurs choses selon le
  contexte."
- **Mechanism**: polysemy by **context-dependence** — the term's meaning shifts with context, and the
  arguer suppresses the context shift. A *narrower* specialization than 862: the meanings are related
  (heterosemy, not homonymy), and the defect is the *suppressed context*, not the unrelated senses.
- **Proposal**: `DirectRef=VagueVerbalClassification_Inference`, `ExceptionRef=`,
  `MappingType=skos:narrowMatch` (a narrower case: polysemy *specifically via suppressed context*, the
  heterosemy specialization — narrower than 862's broad homonymy).

### pk 864 — Sophisme jingle-jangle (Jingle-jangle fallacy) ✅
- **⚠ Bidirectional confusion**: unlike 860/862/863 (one term, multiple meanings), the jingle-jangle
  fallacy goes *both ways* — treating different terms as the same idea (jingle) OR the same term as
  different ideas (jang). The desc says it explicitly: "termes différents pour désigner la même idée,
  ou le même terme pour désigner des idées différentes". This is the **mirror** of the others — it
  confuses the term↔idea mapping in both directions.
- **Mechanism**: the `VagueVerbalClassification_Inference` still applies (the classification is vague
  because the term↔idea mapping is inconsistent), but the bidirectional confusion is the
  *characteristic* specialization.
- **Proposal**: `DirectRef=VagueVerbalClassification_Inference`, `ExceptionRef=`,
  `MappingType=skos:closeMatch` (a direct variant: equivocation *via bidirectional term↔idea
  confusion*, a direct mirror-sibling of the anchor 855's unidirectional case).

---

## 4. Cluster summary (proposed)

| pk | text_fr | Proposed ExceptionRef | Proposed DirectRef | MappingType | Honest? |
|----|---------|----------------------|-------------------|-------------|---------|
| 855 | Équivoque | *(absent)* | `VagueVerbalClassification_Inference` | `skos:closeMatch` | ✅ (anchor) |
| 860 | Glissement lexical polysémique | *(absent)* | `VagueVerbalClassification_Inference` | `skos:closeMatch` | ✅ |
| 862 | Homonymie | *(absent)* | `VagueVerbalClassification_Inference` | `skos:broadMatch` | ✅ |
| 863 | Hétérosémie | *(absent)* | `VagueVerbalClassification_Inference` | `skos:narrowMatch` | ✅ |
| 864 | Sophisme jingle-jangle | *(absent)* | `VagueVerbalClassification_Inference` | `skos:closeMatch` | ✅ |

**5 rows proposed, 5 fully-modeled, 0 FAIL-LOUD.** The third consecutive clean cluster (after PR-8,
PR-9). All leaves reuse the template 856's `VagueVerbalClassification_Inference` (direct-conflict
pattern, mirroring 856/PR-4 800). The only variation is the `MappingType`, encoding the polysemy
specialization: 862 broad (any homonymy), 863 narrow (suppressed context heterosemy), 855/860/864
close (direct variants: abstract / active-drift / bidirectional-mirror).

### Polysemy-mechanism taxonomy (§5 lesson)
Within the single `VagueVerbalClassification_Inference` scheme, the lexical-polysemy leaves specialize
along **three orthogonal axes**:
- **Directionality**: unidirectional (855/860/862/863 — one term, many meanings) vs bidirectional
  (864 jingle-jangle — term↔idea confusion both ways).
- **Relation between meanings**: unrelated homonymy (862 — broad) vs related heterosemy (863 —
  narrow).
- **Activeness**: passive existence of multi-meaning (862/863) vs active drift mid-argument (860).

The MappingType encodes the *breadth* (broad/narrow), while the desc captures the *axis*. This gives
the consumer a graded notion of polysemy, not a binary one — same pattern as PR-4's vagueness-mechanism
taxonomy.

---

## 5. Method notes (additions for the chantier)

- **Mechanism-based sub-sub split (large sub-sub)**: the Equivoque sub-sub (21 rows) is the largest in
  the chantier. Splitting by AIF-relevant mechanism (lexical polysemy / continuum-sorite / reification)
  produces 3 coherent PRs rather than one incoherent 18-leaf PR. Each mechanism maps to a distinct
  AIF-scheme family (`VagueVerbalClassification_Inference` / `VerbalSlipperySlope_Inference` /
  reification-needs-new-CQ), so the split is semantically clean. This generalizes PR-8/9's
  Ambiguïté-narrative mechanism split to an even larger sub-sub.
- **Cross-subfamily scheme reuse (856 → PR-4)**: the template 856 `VagueVerbalClassification_Inference`
  is the same scheme as PR-4's anchor 800 (Vague definition). The Equivoque sub-sub (Ambiguïté
  subfamily) and the Vague-definition sub-sub (Inexact-definition subfamily) honestly share the scheme
  — the distinction is *family* (ambiguity = multiple readings, definition = unclear meaning), not
  *scheme*. This is cross-**subfamily** reuse (one level up from PR-6 831 / PR-7 854 cross-**sub-sub**
  reuse), documented.
- **Bidirectional-confusion specialization (864)**: the jingle-jangle fallacy is the chantier's first
  *bidirectional* equivocation (term↔idea both ways). The `VagueVerbalClassification_Inference` scheme
  captures it (the mapping is inconsistent), but the bidirectionality is the characteristic axis,
  recorded in the desc not the token. Same discipline as PR-4 §5 "intrinsic contestability is
  structural, captured in desc not token".

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not "100% leaves") | ✅ §4 (5 lexical-polysemy rows fully-modeled; Equivoque sub-sub split documented, ~13 leaves deferred to PR-11/12) |
| Per unmapped leaf: legitimate scheme + exception/CQ (not Latin alone) | ✅ §3 (5 leaves, scheme + mechanism each, 0 FAIL-LOUD) |
| `AIF_skosMappingType` coherent | ✅ §4 (close/broad/narrow encode the polysemy-breadth axis) |
| Fail-loud when no honest scheme fits | N/A this cluster (all leaves reuse the template's native scheme) |
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
| PR-9 #714 | Narrative ambiguity / deception | 6 | 6 | 104 (+6) |
| **PR-10 (this)** | **Equivoque / polysemy** | **5** | **5** | **109 (+5)** |
| **total (strict)** | **4 subfamilies touched** | **44** | **105** | **109** |
| **total (inclusive²)** | | **44** | **106** | **109** |

¹ PR-1 originally reported "+4 mapped" counting pk 840 (RA-exists/CA-missing) as mapped. Under the
strict criterion (CA must exist), PR-1 = +3. Flagged by po-2024 audit #709.
² Inclusive = count RA-exists/CA-missing rows (840) as fully-modeled (PR-1's original convention).
The 1-row discrepancy (105 strict vs 106 inclusive) is the accounting decision pending jsboige.

**This PR's +5 is criterion-independent** — all 5 leaves have both a scheme and a conflict.

**Ambiguïté subfamily: Equivoque sub-sub opened (5/~18 leaves done)**. Remaining Equivoque mechanisms:
continuum/sorite (858 template + 857/859, ~3 leaves → PR-11), reification/magic-of-words (867-875,
9 leaves → PR-12), plus misc (861 mondegreen, 865 changement sémantique, 866 métonymie → PR-11).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### pk 855 — Équivoque → **undercut** (degenerate)
- **RA-node**: `VagueVerbalClassification_Inference` — classifying via a term whose extension is not
  fixed.
- **CA-node**: the scheme itself names the defect (direct-conflict pattern) — the classification is
  vague because the term has multiple senses. Degenerate **undercut** (CA coincides with RA, as in
  PR-4 800).

### pk 860 — Glissement lexical polysémique → **undercut** (degenerate)
- **RA-node**: `VagueVerbalClassification_Inference`.
- **I-nodes**: premise P1 (at point A) "term T means M1"; premise P2 (at point B) "term T means M2";
  conclusion C "items classified via T at B are grouped as at A".
- **CA-node**: degenerate **undercut** — the inference (that T's classification transfers from A to B)
  is defective because P1 and P2 use different senses of T (active drift). The conclusion may hold;
  the *inference* is defective.

### pk 862 — Homonymie → **undercut** (degenerate)
- **RA-node**: `VagueVerbalClassification_Inference`.
- **CA-node**: degenerate **undercut** — T has unrelated senses, so the classification has no fixed
  extension.

### pk 863 — Hétérosémie → **undercut** (degenerate)
- **RA-node**: `VagueVerbalClassification_Inference`.
- **CA-node**: degenerate **undercut** — T's meaning shifts with context, suppressed by the arguer.

### pk 864 — Sophisme jingle-jangle → **undercut** (degenerate)
- **RA-node**: `VagueVerbalClassification_Inference`.
- **CA-node**: degenerate **undercut** — the term↔idea mapping is inconsistent (bidirectional).

### Cluster attack-type distribution
| pk | RA-node | CA-node | Attack type |
|----|---------|---------|-------------|
| 855 | `VagueVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |
| 860 | `VagueVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |
| 862 | `VagueVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |
| 863 | `VagueVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |
| 864 | `VagueVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |

**All 5 leaves are degenerate undercuts** — the direct-conflict pattern (scheme as its own defect), as
in PR-4's anchor 800 and PR-9's 885. The lexical-polysemy fallacies *are* defective
`VagueVerbalClassification_Inference`s; the inference (term ⟹ classification) is defective because the
term's extension is not fixed. No undermine, no rebut.

### Representation note
As in PR-1 to PR-9, the I-node/RA-node/CA-node decomposition is **recorded here but not serialized**
in the CSV — pending jsboige's schema decision (§4 of the coverage-status note #707). This PR is
**in-sub-sub template and independent of that decision**: the modeling (schemes + conflicts) holds
regardless of whether I/RA/CA is serialized in new columns.

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No #674/#596/#666/#686 merge, no régén launch.
- ✅ Proposition derived code=truth from the taxonomy CSV + AIF vocabulary confirmed by existing
  usage (the only token proposed — `VagueVerbalClassification_Inference` — is reused from the template
  856, no fabrication).
- ✅ Mechanism-based sub-sub split documented; cross-subfamily scheme reuse (856 → PR-4) noted;
  bidirectional-confusion specialization (864) documented; accounting transparency (dual criterion,
  §6) provided re: po-2024 audit #709.

Relates: dispatch `5ymo70` (primary), #498 (chantier), PR-1 #699, PR-2 #701, PR-3 #703, PR-4 #705,
PR-6 #708, PR-7 #711, PR-8 #713, PR-9 #714, coverage-status #707, verification-audit #709,
family-gap #712, #133/#130 (existing OWL), #499 (inverse), #677 (0 fabrication), #192, #458.
