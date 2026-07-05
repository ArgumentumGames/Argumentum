# 2026-07-05 — #498 AIF chantier, PR-7 : cluster « Amphibologie » (Ambiguïté, nouvelle subfamily)

**Scope**: seventh cluster of the #498 chantier — first cluster of a **new subfamily**
(**Ambiguïté**, Abus de langage / Misleading language family). Models the **Amphibologie** sub-sub
(pk 847 anchor + 848-854 leaves, 8 rows total). **Proposition only — GATED, 0 write to prod CSV.**
Triggered by ai-01 dispatch `ih617l` (primary: « prochain cluster borrow-root schema-independent »).
Borrow-root shape — **independent of the I/RA/CA serialization decision** (no schema dependency).

**Repo reference**: master `34c7702c`. Issue: #498. Predecessors: PR-1 #699, PR-2 #701, PR-3 #703
(Fallacious comparison complete), PR-4 #705 (Vague definition), PR-6 #708 (Inconsistent definition).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies →
> worker applies the CSV edits in a follow-up PR (gated).

---

## TL;DR

The **Amphibologie** sub-sub has **no in-sub-sub mapped anchor** (pk 847 d3 is unmapped) → it borrows
the d2 subfamily root **pk 846** ("Ambiguïté", MAPPED, direct-conflict pattern:
`ArbitraryVerbalClassification_Inference` + `OppositeConsequences_Conflict` +
`SignFromOtherEvents_Conflict`, `skos:broadMatch`). The sub-sub has **8 rows** (pk 847 d3 anchor +
7 leaves, depth 4-5) — PR-2-sized.

The cluster is **structurally coherent** (all leaves are syntactic / structural / prosodic ambiguity
mechanisms — sentence-level rather than term-level), which sets it apart from the *Equivoque* sub-sub
(term-level ambiguity). This coherence drives an **honest scheme-divergence from the borrow-root**:
amphiboly is syntactic, while the borrow-root's `ArbitraryVerbalClassification_Inference` is
semantic/term-level — so the leaves split across **native schemes that honestly fit their mechanism**
(`Deductive_Inference`+`Logical_Conflict` for scope, `ArbitraryVerbalClassification_Inference` for
prosody, `VagueVerbalClassification_Inference` for undefined terms) rather than forcing the
borrow-root's classification framing onto syntactic defects.

**8 rows proposed: 5 fully-modeled + 3 FAIL-LOUD** (847/848/853 structural ambiguity — **third
recurring AIF ontology gap** after circularity: no native syntactic-ambiguity CQ). This PR opens the
**Ambiguïté** subfamily and flags the ambiguity gap for the eventual ontology-extension decision.

---

## 1. Cluster state (code=truth)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Abus de langage** (Misleading
language), subfamily **Ambiguïté**, sub-sub **Amphibologie**:

| pk | text_fr | depth | DirectRef | ExceptionRef | MappingType | State |
|----|---------|-------|-----------|--------------|-------------|-------|
| **846** | **Ambiguïté** (borrowed anchor) | d2 | `ArbitraryVerbalClassification_Inference`, `OppositeConsequences_Conflict`, `SignFromOtherEvents_Conflict` | — | `skos:broadMatch` | **✅ MAPPED (d2 subfamily root)** |
| 847 | Amphibologie | d3 | — | — | — | unmapped (d3 anchor, **PR-7**) |
| 848 | Ponctuation ambiguë | d4 | — | — | — | unmapped (**PR-7**) |
| 849 | Sophisme de portée modale | d4 | — | — | — | unmapped (**PR-7**) |
| 850 | Glissement du quantificateur | d5 | — | — | — | unmapped (**PR-7**) |
| 851 | Accent | d4 | — | — | — | unmapped (**PR-7**) |
| 852 | Contraste illicite | d5 | — | — | — | unmapped (**PR-7**) |
| 853 | Solécisme | d4 | — | — | — | unmapped (**PR-7**) |
| 854 | Barbarisme | d4 | — | — | — | unmapped (**PR-7**) |

The borrowed anchor (846) is the **template**: a direct-conflict pattern carrying **three** native
tokens (the fallacy *is* an ambiguous classification / a sign from ambiguous events / opposite
consequences of an ambiguous statement). As in PR-3 (anchor 833 split across 2 leaves), the
Amphibologie leaves honestly draw on the borrow-root's tokens where they fit (851/852 →
`ArbitraryVerbalClassification_Inference`) and diverge to other native schemes where the mechanism is
genuinely different (849/850 → deductive scope; 854 → vagueness).

---

## 2. The borrowed anchor (846, recap)

**Pattern**: direct-conflict (no ExceptionRef).

**Conflict/scheme nodes** (DirectRef):
- `ArbitraryVerbalClassification_Inference` — the ambiguous statement is classified under one
  arbitrarily-chosen reading.
- `OppositeConsequences_Conflict` — the ambiguity admits readings with opposite consequences.
- `SignFromOtherEvents_Conflict` — the ambiguous statement is taken as a sign, but the sign relation
  is ambiguous.

**desc_fr** (846): "Votre argumentation repose sur des énoncés qui peuvent avoir plusieurs sens."

The Amphibologie leaves specialize the *ambiguity mechanism*: sentence-structure (847/848),
logical scope (849/850), prosody (851/852), grammar (853), lexicon (854). The borrow-root's
*classification* framing fits the prosodic and lexical leaves but **not** the structural and
scope leaves — those honestly target deductive/logical schemes instead (PR-2 §5 scheme-divergence).

---

## 3. Proposed AIF structure for the leaves

For each leaf: **(a) legitimate scheme (or FAIL-LOUD), (b) exception/CQ or direct conflict,
(c) `AIF_skosMappingType`**. Vocabulary restricted to AIF-native tokens confirmed by existing usage.

### pk 847 — Amphibologie (Amphiboly, d3 anchor) ⚠ FAIL-LOUD
- **desc_fr**: "Vous construisez votre argumentation sur des phrases dont la structure peut prêter à
  différentes interprétations."
- **⚠ FAIL-LOUD at the CA-node level**: amphiboly is a **syntactic** ambiguity — the sentence
  *structure* (not term choice) admits two readings, and the arguer rides the one that supports their
  conclusion. The defect defeats an inference (the premise is equivocal), but **no native AIF Conflict
  node captures syntactic ambiguity**. The borrow-root's `ArbitraryVerbalClassification_Inference` is
  term-level (semantic), not sentence-level (syntactic) — forcing it would conflate the two (PR-2 §5
  scheme-divergence discipline). `Logical_Conflict` does not fit (the two readings need not be
  contradictory; they are simply different parses).
- **Proposal**: `DirectRef=` *(absent — FAIL LOUD)*, `ExceptionRef=VerbalClassification_Inference`
  (the statement-as-classification RA-node exists), `AIF_skosOther="Syntactic ambiguity
  (amphiboly): sentence structure admits multiple parses (no native AIF Conflict node —
  ArbitraryVerbalClassification_Inference is term-level, does not fit syntactic ambiguity)"`,
  `MappingType=skos:closeMatch`. Do **not** fabricate an `Ambiguity_Conflict` or `Amphiboly_Conflict`
  token.

### pk 848 — Ponctuation ambiguë (Ambiguous punctuation) ⚠ FAIL-LOUD
- **desc_fr**: "Vous utilisez une ponctuation imprécise, créant ainsi des phrases à double sens."
- **⚠ FAIL-LOUD at the CA-node level**: same gap as 847 — punctuation is a **structural** ambiguity
  mechanism (the parse tree depends on comma/period placement), not a term-level classification
  defect. No native AIF CQ captures it.
- **Proposal**: `DirectRef=` *(absent — FAIL LOUD)*, `ExceptionRef=VerbalClassification_Inference`,
  `AIF_skosOther="Structural ambiguity via punctuation (no native AIF CQ — same gap as 847)"`,
  `MappingType=skos:narrowMatch` (a narrower case: syntactic ambiguity *specifically via punctuation*,
  narrower than 847's general amphiboly).

### pk 849 — Sophisme de portée modale (Modal scope fallacy) ✅
- **desc_fr**: "Vous modifiez le sens logique en confondant la portée d'un terme modal (nécessaire,
  possible, certain)."
- **⚠ Honest scheme-divergence from the borrow-root**: the modal scope fallacy is a **deductive /
  logical** defect (conflating ◻(A→B) with A→◻B, or wide vs narrow scope of a modal operator), not a
  classification defect. It honestly targets `Deductive_Inference` (native, confirmed 2x in existing
  usage), not the borrow-root's `ArbitraryVerbalClassification_Inference`.
- **Exception/CQ**: the wide-scope and narrow-scope readings are **logically incompatible** (they
  license different conclusions) → `Logical_Conflict` (native, confirmed 1x). The deductive inference
  is defective because the operator's scope is conflated.
- **Proposal**: `ExceptionRef=Deductive_Inference`, `DirectRef=Logical_Conflict`,
  `MappingType=skos:closeMatch` (a direct logical-inference variant of the ambiguity family).

### pk 850 — Glissement du quantificateur (Quantifier shift) ✅
- **desc_fr**: "Vous modifiez le sens d'une phrase en déplaçant les quantificateurs."
- **Mechanism**: the quantifier-shift fallacy — confusing ∀x∃y with ∃y∀x (e.g. "Everyone loves
  someone" → "There is someone whom everyone loves"). A direct sibling of 849 (scope conflation), but
  for **quantifiers** rather than modal operators.
- **Legitimate scheme**: `Deductive_Inference` (same scheme-divergence as 849 — a deductive/logical
  defect).
- **Exception/CQ**: the two quantifier orderings are logically incompatible → `Logical_Conflict`.
- **Proposal**: `ExceptionRef=Deductive_Inference`, `DirectRef=Logical_Conflict`,
  `MappingType=skos:narrowMatch` (a narrower case: scope conflation *specifically via quantifier
  order*, narrower than 849's general modal scope).

### pk 851 — Accent (Fallacy of accent) ✅
- **desc_fr**: "Vous utilisez l'intonation ou l'accentuation pour suggérer un message différent des
  mots employés."
- **Legitimate scheme**: `ArbitraryVerbalClassification_Inference` (native, **borrowed from anchor
  846** — the accent/stress picks out one reading of the utterance, an arbitrary classification of its
  meaning). Unlike 847/848 (syntactic), accent is **prosodic / term-level** (which word is stressed) →
  the borrow-root's classification framing honestly fits.
- **Pattern**: direct-conflict (mirrors 846).
- **Proposal**: `DirectRef=ArbitraryVerbalClassification_Inference`, `ExceptionRef=`,
  `MappingType=skos:closeMatch` (a direct prosodic variant of 846's classification defect).

### pk 852 — Contraste illicite (Illicit contrast) ✅
- **desc_fr**: "Vous attribuez à autrui une intonation qui modifie le sens perçu de ses mots."
- **Mechanism**: the illicit-contrast fallacy — **misattributing** stress/prosody to someone else's
  words to distort their meaning (the mirror of 851, applied to others' speech). Same prosodic /
  term-level mechanism → same native scheme.
- **Legitimate scheme**: `ArbitraryVerbalClassification_Inference` (borrowed from 846).
- **Proposal**: `DirectRef=ArbitraryVerbalClassification_Inference`, `ExceptionRef=`,
  `MappingType=skos:narrowMatch` (a narrower case: arbitrary classification *via misattributed
  prosody to another's speech*, narrower than 851's own-accent case).

### pk 853 — Solécisme (Solecism) ⚠ FAIL-LOUD
- **desc_fr**: "Une erreur de grammaire dans votre phrase crée une ambiguïté, même si les mots
  employés sont corrects."
- **⚠ FAIL-LOUD at the CA-node level**: the desc explicitly states the grammar error **creates an
  ambiguity** — but the ambiguity mechanism is **structural/grammatical** (the parse is ill-formed),
  not term-level classification. No native AIF CQ captures grammar-induced ambiguity (same gap as
  847/848). `VagueVerbalClassification_Inference` does not fit (the words are correct, not vague —
  the *grammar* is the defect).
- **Proposal**: `DirectRef=` *(absent — FAIL LOUD)*, `ExceptionRef=VerbalClassification_Inference`,
  `AIF_skosOther="Grammar-induced ambiguity (solecism): ill-formed parse creates ambiguity (no native
  AIF CQ — same structural-ambiguity gap as 847/848)"`, `MappingType=skos:narrowMatch` (a narrower
  case: ambiguity *specifically via grammar error*).

### pk 854 — Barbarisme (Barbarism) ✅
- **desc_fr**: "Vous employez un mot inventé ou emprunté à une autre langue, rendant le propos
  incertain."
- **⚠ Semantic link to PR-4**: unlike 847/848/853 (structural ambiguity), barbarism makes the
  statement **uncertain because the term is undefined** — this is **vagueness** (no determinate
  meaning), not ambiguity (two determinate readings). The desc "incertain" signals vagueness. It
  honestly reuses PR-4's anchor scheme `VagueVerbalClassification_Inference` (cross-cluster reuse,
  like PR-6 831 → PR-4).
- **Legitimate scheme**: `VagueVerbalClassification_Inference` (PR-4 anchor scheme, native).
- **Pattern**: direct-conflict (the vagueness *is* the scheme — the term is undefined).
- **Proposal**: `DirectRef=VagueVerbalClassification_Inference`, `ExceptionRef=`,
  `MappingType=skos:narrowMatch` (a narrower case: vagueness *via an invented/borrowed term*,
  narrower than PR-4 802's plain absence of definition).

---

## 4. Cluster summary (proposed)

| pk | text_fr | Proposed ExceptionRef | Proposed DirectRef | MappingType | Honest? |
|----|---------|----------------------|-------------------|-------------|---------|
| 847 | Amphibologie | `VerbalClassification_Inference` | *(absent — FAIL LOUD)* | `skos:closeMatch` | ⚠ FAIL LOUD (syntactic ambiguity) |
| 848 | Ponctuation ambiguë | `VerbalClassification_Inference` | *(absent — FAIL LOUD)* | `skos:narrowMatch` | ⚠ FAIL LOUD (structural ambiguity) |
| 849 | Sophisme de portée modale | `Deductive_Inference` | `Logical_Conflict` | `skos:closeMatch` | ✅ |
| 850 | Glissement du quantificateur | `Deductive_Inference` | `Logical_Conflict` | `skos:narrowMatch` | ✅ |
| 851 | Accent | *(absent)* | `ArbitraryVerbalClassification_Inference` | `skos:closeMatch` | ✅ |
| 852 | Contraste illicite | *(absent)* | `ArbitraryVerbalClassification_Inference` | `skos:narrowMatch` | ✅ |
| 853 | Solécisme | `VerbalClassification_Inference` | *(absent — FAIL LOUD)* | `skos:narrowMatch` | ⚠ FAIL LOUD (grammar-induced ambiguity) |
| 854 | Barbarisme | *(absent)* | `VagueVerbalClassification_Inference` | `skos:narrowMatch` | ✅ |

**8 rows proposed: 5 fully-modeled (849, 850, 851, 852, 854) + 3 FAIL-LOUD (847, 848, 853).** The
cluster spans **three patterns**: exception (849, 850 — scheme + conflict), direct-conflict (851, 852,
854 — scheme as defect), FAIL-LOUD (847, 848, 853 — structural-ambiguity gap). It demonstrates
**scheme-divergence from the borrow-root** (849/850 → deductive, 854 → vagueness) and **cross-cluster
scheme reuse** (854 → PR-4).

---

## 5. Method notes (additions for the chantier)

- **Syntactic vs term-level ambiguity (honest scheme-divergence from borrow-root)**: the Ambiguïté
  borrow-root (846) frames ambiguity as *verbal classification* (term-level / semantic). But
  amphiboly (847/848/853) is *syntactic* (sentence-structure / grammar) — a genuinely different
  mechanism. Forcing the borrow-root's `ArbitraryVerbalClassification_Inference` onto syntactic
  defects would conflate the two layers (the exact anti-pattern PR-2 §5 warns against). The honest
  move: FAIL-LOUD the structural leaves (no native syntactic-ambiguity CQ) and model only the leaves
  whose mechanism is genuinely term-level (851/852 prosody, 854 lexicon) with the borrow-root's
  scheme.
- **Third recurring AIF ontology gap — ambiguity**: PR-1/PR-6 documented circularity as a recurring
  gap (no native circularity CQ). This cluster documents **ambiguity** as a third gap (no native
  syntactic-ambiguity CQ), with **3 FAIL-LOUD leaves** (847/848/853) — the most FAIL-LOUD in a single
  cluster so far. The chantier now has recurring gaps in **3 of 7 PR** (circularity PR-1/PR-6,
  pure-comparison PR-2, ambiguity PR-7) — all worth flagging for the eventual ontology-extension
  decision.
- **Logical-scope leaves (849/850) — scheme-divergence to Deductive_Inference**: modal-scope and
  quantifier-shift fallacies are genuinely *logical* defects (scope conflation of operators), not
  classification defects. They honestly target `Deductive_Inference` + `Logical_Conflict` (both
  native). This is the first chantier use of `Deductive_Inference` for a *fallacy* (vs its existing
  use for valid deductions) — legitimate because the fallacy *is* a defective deductive inference.
- **Vagueness-vs-ambiguity distinction (854)**: barbarism produces **uncertainty** (the term is
  undefined → vague), not ambiguity (two readings). The desc word "incertain" is the signal. This
  distinguishes 854 (→ `VagueVerbalClassification_Inference`, PR-4) from 847/848/853 (→ FAIL-LOUD
  ambiguity). The distinction is philosophically real (vagueness ≠ ambiguity) and the AIF modeling
  respects it.

---

## 6. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented (not "100% leaves") | ✅ §4 (86→91 fully-modeled / 89→94 DirectRef-loose; new subfamily Ambiguïté opened) |
| Per unmapped leaf: legitimate scheme + exception/CQ (not Latin alone) | ✅ §3 — 5 leaves fully-modeled; 3 FAIL-LOUD (847/848/853 structural-ambiguity gap) |
| `AIF_skosMappingType` coherent | ✅ §4 (close/narrow per leaf semantics; narrow encodes the specialization mechanism) |
| Fail-loud when no honest scheme fits | ✅ §3 pk 847/848/853 (structural ambiguity) — documented, not fabricated |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies, then CSV edits, then OWL regen (#133) |

### Coverage accounting (cumulative, chantier-wide)
| PR | Cluster | Leaves | Fully-modeled (DoD) | DirectRef-loose |
|----|---------|--------|---------------------|-----------------|
| baseline | — | — | 70 | 70 |
| PR-1 #699 | False analogy | 4 | 74 (+4) | 74 (+4) |
| PR-2 #701 | Faulty comparison | 5 | 76 (+2) | 79 (+5) |
| PR-3 #703 | Association fallacy | 2 | 78 (+2) | 81 (+2) |
| PR-4 #705 | Vague definition | 3 | 81 (+3) | 84 (+3) |
| PR-6 #708 | Inconsistent definition | 7 | 86 (+5) | 89 (+5) |
| **PR-7 (this)** | **Amphibologie** | **8** | **91 (+5)** | **94 (+5)** |
| **total** | **4 subfamilies touched** | **29** | **91** | **94** |

**Ambiguïté subfamily: 1/3 sub-subs opened** (Amphibologie PR-7). Remaining: Equivoque (2 mapped +
18 unmapped — large, needs splitting by depth), Ambiguïté narrative (1 mapped anchor + 10 unmapped,
in-sub-sub anchor — cleanest next candidate).

---

## 7. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### pk 847 — Amphibologie → FAIL-LOUD at **CA-node**
- **RA-node**: `VerbalClassification_Inference` (exists — classifying items via the statement).
- **I-nodes**: premise P "statement S means M1"; the sentence structure *also* admits M2.
- **CA-node**: ⚠ **absent (FAIL LOUD)** — amphiboly would be an **undercut** (the inference that S
  licenses classification under M1 is defective because S is syntactically equivocal), but no native
  AIF Conflict node captures syntactic ambiguity (same gap shape as PR-1 840 circularity).

### pk 848 — Ponctuation ambiguë → FAIL-LOUD at **CA-node**
- **RA-node**: `VerbalClassification_Inference`.
- **CA-node**: ⚠ **absent (FAIL LOUD)** — punctuation-induced structural ambiguity, no native CQ
  (same gap as 847).

### pk 849 — Sophisme de portée modale → **undercut**
- **RA-node**: `Deductive_Inference` — a deductive step relying on a modal premise.
- **I-nodes**: premise P "◻(A→B)" (wide scope); the fallacy concludes "A→◻B" (narrow scope).
- **CA-node**: `Logical_Conflict`, applied as an **undercut** on the RA-node — the deductive inference
  is defective because the wide-scope and narrow-scope readings are logically incompatible (they
  license different conclusions). The conclusion may still hold; the *inference* is defective.

### pk 850 — Glissement du quantificateur → **undercut**
- **RA-node**: `Deductive_Inference`.
- **I-nodes**: premise P "∀x∃y L(x,y)"; the fallacy concludes "∃y∀x L(x,y)".
- **CA-node**: `Logical_Conflict`, applied as an **undercut** — the two quantifier orderings are
  logically incompatible (same shape as 849).

### pk 851 — Accent → **undercut**
- **RA-node**: `ArbitraryVerbalClassification_Inference` — classifying the utterance's meaning via
  stress.
- **I-nodes**: premise P "utterance U, stressed on word w, means M".
- **CA-node**: the scheme itself names the defect (direct-conflict pattern) — the stress-driven
  classification is arbitrary. Degenerate **undercut** (CA coincides with RA, as in PR-4).

### pk 852 — Contraste illicite → **undercut**
- **RA-node**: `ArbitraryVerbalClassification_Inference` — classifying another's meaning via
  misattributed stress.
- **CA-node**: degenerate **undercut** (direct-conflict, scheme as defect).

### pk 853 — Solécisme → FAIL-LOUD at **CA-node**
- **RA-node**: `VerbalClassification_Inference`.
- **CA-node**: ⚠ **absent (FAIL LOUD)** — grammar-induced ambiguity, no native CQ (same gap as
  847/848).

### pk 854 — Barbarisme → **undercut**
- **RA-node**: `VagueVerbalClassification_Inference` — classifying via an undefined term.
- **CA-node**: degenerate **undercut** (direct-conflict — the vagueness is the scheme).

### Cluster attack-type distribution
| pk | RA-node | CA-node | Attack type |
|----|---------|---------|-------------|
| 847 | `VerbalClassification_Inference` | *(absent — FAIL LOUD)* | would be undercut (syntactic ambiguity) — untyped |
| 848 | `VerbalClassification_Inference` | *(absent — FAIL LOUD)* | would be undercut (structural ambiguity) — untyped |
| 849 | `Deductive_Inference` | `Logical_Conflict` | undercut |
| 850 | `Deductive_Inference` | `Logical_Conflict` | undercut |
| 851 | `ArbitraryVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |
| 852 | `ArbitraryVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |
| 853 | `VerbalClassification_Inference` | *(absent — FAIL LOUD)* | would be undercut (grammar ambiguity) — untyped |
| 854 | `VagueVerbalClassification_Inference` | *(scheme as direct conflict)* | undercut (degenerate) |

**All typed leaves are undercuts** — consistent with jsboige's "most fallacies live in the undercut":
ambiguity fallacies defeat the *inference* (the statement→classification or premise→conclusion step),
they do not deny the conclusion. No undermine, no rebut in this cluster.

### Representation note
As in PR-1 to PR-6, the I-node/RA-node/CA-node decomposition is **recorded here but not serialized**
in the CSV — pending jsboige's schema decision (§4 of the coverage-status note #707). This PR is
**borrow-root and independent of that decision**: the modeling (schemes + conflicts) holds regardless
of whether I/RA/CA is serialized in new columns.

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No #674/#596/#666 merge, no régén launch.
- ✅ Proposition derived code=truth from the taxonomy CSV + AIF vocabulary confirmed by existing
  usage (all proposed tokens native: `VerbalClassification_Inference`, `Deductive_Inference`,
  `Logical_Conflict`, `ArbitraryVerbalClassification_Inference`, `VagueVerbalClassification_Inference`,
  + the borrow-root's `OppositeConsequences_Conflict`, `SignFromOtherEvents_Conflict`). No fabrication
  — 847/848/853 FAIL-LOUD with documented structural-ambiguity gap.
- ✅ Scheme-divergence from borrow-root (849/850 → Deductive, 854 → Vague) documented; cross-cluster
  reuse (854 → PR-4) noted; third recurring ontology gap (ambiguity) flagged.

Relates: dispatch `ih617l` (primary), #498 (chantier), PR-1 #699, PR-2 #701, PR-3 #703, PR-4 #705,
PR-6 #708, coverage-status #707, verification-audit #709, #133/#130 (existing OWL), #499 (inverse),
#677 (0 fabrication), #192, #458.
