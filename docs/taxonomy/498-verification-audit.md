# 2026-07-05 — #498 AIF chantier — verification audit (code=truth, adversarial)

**Scope**: independent code=truth verification of the 5 cluster propositions produced by po-2023
(PR-1 #699 False analogy, PR-2 #701 Faulty comparison, PR-3 #703 Association fallacy, PR-4 #705
Vague definition, PR-6 #708 Inconsistent definition). **Goal**: de-risk jsboige's ratification by
adversarially checking every claim against the CSV before any prod write.

**Author**: po-2024 (worker) · **Dispatch**: `awhj8g` (PRIMARY, ai-01 2026-07-05 19:11)
**Base**: master `34c7702c` (post-batch-merge) · **Read-only**: 0 write under `Cards/`, audit doc only.

> **Method.** A deterministic stdlib script (`498_audit_extract.py`, read-only) extracts — for every
> pk cited by the 5 docs (799, 800-803, 826-832, 833-845 = **25 pks**) — the columns `PK`,
> `Soussousfamille`, `Subsubfamily`, `text_fr`, `desc_fr`, `AIF_skosDirectRef`,
> `AIF_skosExceptionRef`, `AIF_skosOther`, `AIF_skosMappingType`, and builds the **native AIF
> vocabulary inventory** (every `*_Inference` / `*_Conflict` token appearing in any AIF column of
> any row — the "confirmed by existing usage" set the propositions restrict themselves to). The
> audit verdicts below are computed by crossing the docs' claims against this deterministic
> extraction (no LLM judgment on the data — only on the semantic legitimacy of FAIL-LOUD cases).

---

## TL;DR — verdict

**The 5 propositions are code=truth-clean.** Every pk exists, every cluster boundary matches
`Soussousfamille`/`Subsubfamily`, every anchor's AIF columns match byte-for-byte, every proposed
token is native (0 fabrication), every cited `desc_fr` matches byte-close, and every FAIL-LOUD case
is legitimate (the missing native token genuinely does not exist in the inventory). **1 FLAG**
raised (coverage-accounting consistency, not a data error) — see §6.

| Check | Result |
|-------|--------|
| 25 cited pks present in CSV | ✅ 25/25 |
| Cluster boundaries (`Soussousfamille`/`Subsubfamily`) match docs | ✅ 5/5 clusters |
| 4 mapped anchors — `text_fr`/DirectRef/ExceptionRef/MappingType match | ✅ 4/4 (byte-exact) |
| 10 proposed tokens — native (in existing inventory) | ✅ 10/10 (0 fabrication) |
| 21 cited `desc_fr` — match byte-close | ✅ 21/21 (1 apostrophe cosmetic diff on 841) |
| 6 FAIL-LOUD cases — legitimate | ✅ 6/6 (missing token genuinely absent) |
| Coverage accounting — uniform criterion across PRs | ⚠ **1 FLAG** (PR-1 vs PR-6, same shape counted differently) |

---

## 1. Anchor verification (the 4 mapped anchors the propositions lean on)

Each cluster borrows or reuses a mapped anchor whose AIF columns *must* match the doc's claim, or
the proposition's foundation is wrong. **All 4 match byte-exact.**

| pk | doc | text_fr (claim → CSV) | DirectRef | ExceptionRef | MappingType | Verdict |
|----|-----|-----------------------|-----------|--------------|-------------|---------|
| 799 | PR-6 | "Définition biaisée" → "Définition biaisée" | `BiasedClassification_Conflict` + `ArbitraryVerbalClassification_Inference` → exact | `VerbalClassification_Inference` → exact | `skos:broadMatch` → exact | ✅ PASS |
| 800 | PR-4 | "Acception vague" → "Acception vague" | `VagueVerbalClassification_Inference` → exact | (empty) → exact | `skos:closeMatch` → exact | ✅ PASS |
| 833 | PR-1/2/3 | "Comparaison fallacieuse" → "Comparaison fallacieuse" | `BiasedClassification_Conflict` + `ExceptionSimilarityCase_Conflict` → exact | (empty) → exact | `skos:broadMatch` → exact | ✅ PASS |
| 839 | PR-1 | "Fausse analogie" → "Fausse analogie" | `DifferencesUndermineSimilarity_Conflict` → exact | `Analogy_Inference` → exact | `skos:closeMatch` → exact | ✅ PASS |

**Finding**: the propositions' anchors are not invented or misquoted — they reproduce the CSV
exactly. This is the load-bearing check (a wrong anchor would invalidate the whole cluster pattern).

---

## 2. Cluster boundaries (`Soussousfamille` / `Subsubfamily_en` code=truth)

Each doc scopes its cluster to a `Subsubfamily` and a pk range. CSV verification:

| Cluster (doc) | Claimed pk range | CSV `Subsubfamily_en` (all rows in range) | CSV `Subfamily` | Verdict |
|---------------|------------------|-------------------------------------------|-----------------|---------|
| False analogy (PR-1) | 839-843 | `False analogy` ×5 | `Fallacious comparison` | ✅ PASS |
| Faulty comparison (PR-2) | 834-838 | `Faulty comparison` ×5 | `Fallacious comparison` | ✅ PASS |
| Association fallacy (PR-3) | 844-845 | `Association fallacy` ×2 | `Fallacious comparison` | ✅ PASS |
| Vague definition (PR-4) | 800-803 | `Vague definition` ×4 | `Inexact definition` | ✅ PASS |
| Inconsistent definition (PR-6) | 826-832 | `Inconsistent definition` ×7 | `Inexact definition` | ✅ PASS |

**Notable**: PR-1's "cluster-boundary correction" (§1 of PR-1 — pk 838 belongs to Faulty comparison,
not False analogy) is verified correct: 838's CSV `Subsubfamily_en` = `Faulty comparison`, so its
deferral to PR-2 is right. The earlier draft's misplacement was genuinely corrected.

---

## 3. Per-leaf verdict table (21 leaves)

For each leaf: `text_fr` + `desc_fr` provenance (CSV), proposed tokens (all native — see §4), and
verdict. **All 21 PASS on data integrity.** The FLAG in §6 is about coverage *accounting*, not data.

### PR-1 — False analogy (anchor 839 + 4 leaves)

| pk | text_fr (CSV) | Proposed ExceptionRef | Proposed DirectRef | MappingType | Verdict |
|----|---------------|----------------------|--------------------|-------------|---------|
| 840 | Pétition de principe analogique | `Analogy_Inference` | *(FAIL-LOUD: circularity, no native CA)* | `skos:closeMatch` | ✅ data-OK; FAIL-LOUD legit (see §5) |
| 841 | Analogie étendue | `Analogy_Inference` | `DifferencesUndermineSimilarity_Conflict` | `skos:narrowMatch` | ✅ PASS |
| 842 | Argument de la similarité fallacieuse | `Analogy_Inference` | `DifferencesUndermineSimilarity_Conflict` | `skos:broadMatch` | ✅ PASS |
| 843 | Fausse équivalence | `Analogy_Inference` | `DifferencesUndermineSimilarity_Conflict` | `skos:closeMatch` | ✅ PASS |

`desc_fr` of all 4 match the CSV byte-close (841 uses a typographic apostrophe `'` vs CSV `'` —
cosmetic, same text).

### PR-2 — Faulty comparison (borrowed anchor 833 + 5 leaves)

| pk | text_fr (CSV) | Proposed ExceptionRef | Proposed DirectRef | MappingType | Verdict |
|----|---------------|----------------------|--------------------|-------------|---------|
| 834 | Comparaison abusive | *(FAIL-LOUD: no native comparison scheme)* | `ExceptionSimilarityCase_Conflict` | `skos:broadMatch` | ✅ data-OK; FAIL-LOUD legit |
| 835 | Comparaison incomplète | *(FAIL-LOUD)* | `ExceptionSimilarityCase_Conflict` | `skos:broadMatch` | ✅ data-OK; FAIL-LOUD legit |
| 836 | Classification non exclusive | `VerbalClassification_Inference` | `BiasedClassification_Conflict` | `skos:closeMatch` | ✅ PASS |
| 837 | Comparaison incohérente | *(FAIL-LOUD)* | `ExceptionSimilarityCase_Conflict` | `skos:broadMatch` | ✅ data-OK; FAIL-LOUD legit |
| 838 | Distinction sans différence | `ArbitraryVerbalClassification_Inference` | `PropertyNotExistant_Conflict` | `skos:broadMatch` | ✅ PASS (scheme-divergence honest) |

### PR-3 — Association fallacy (borrowed anchor 833 + 2 leaves)

| pk | text_fr (CSV) | Proposed ExceptionRef | Proposed DirectRef | MappingType | Verdict |
|----|---------------|----------------------|--------------------|-------------|---------|
| 844 | Sophisme d'association | `VerbalClassification_Inference` | `BiasedClassification_Conflict` | `skos:closeMatch` | ✅ PASS |
| 845 | Amalgame | `ArbitraryVerbalClassification_Inference` | `ExceptionSimilarityCase_Conflict` | `skos:closeMatch` | ✅ PASS |

### PR-4 — Vague definition (anchor 800 + 3 leaves)

| pk | text_fr (CSV) | Proposed ExceptionRef | Proposed DirectRef | MappingType | Verdict |
|----|---------------|----------------------|--------------------|-------------|---------|
| 801 | Défaut d'élucidation | *(absent — direct-conflict)* | `VagueVerbalClassification_Inference` | `skos:narrowMatch` | ✅ PASS |
| 802 | Indéfinissabilité | *(absent)* | `VagueVerbalClassification_Inference` | `skos:broadMatch` | ✅ PASS |
| 803 | Concept essentiellement contesté | *(absent)* | `VagueVerbalClassification_Inference` | `skos:closeMatch` | ✅ PASS |

### PR-6 — Inconsistent definition (borrowed anchor 799 + 7 leaves)

| pk | text_fr (CSV) | Proposed ExceptionRef | Proposed DirectRef | MappingType | Verdict |
|----|---------------|----------------------|--------------------|-------------|---------|
| 826 | Définition incohérente | `VerbalClassification_Inference` | `Logical_Conflict` | `skos:closeMatch` | ✅ PASS |
| 827 | Conditions conflictuelles | *(absent — direct-conflict)* | `Logical_Conflict` | `skos:broadMatch` | ✅ PASS |
| 828 | Concept volé | *(absent)* | `InconsistentCommitment_Inference` | `skos:closeMatch` | ✅ PASS |
| 829 | Définition circulaire | `VerbalClassification_Inference` | *(FAIL-LOUD: circularity, no native CA)* | `skos:closeMatch` | ✅ data-OK; FAIL-LOUD legit |
| 830 | Argument du dictionnaire | `VerbalClassification_Inference` | `ArbitraryVerbalClassification_Inference` | `skos:narrowMatch` | ✅ PASS |
| 831 | Sophisme définiste | *(absent)* | `VagueVerbalClassification_Inference` (cross-cluster, PR-4 scheme) | `skos:narrowMatch` | ✅ PASS (cross-ref honest) |
| 832 | Sophisme philosophique | *(absent)* | *(FAIL-LOUD: vague meta-fallacy)* | *(none)* | ✅ data-OK; FAIL-LOUD legit |

---

## 4. Native vocabulary inventory — 0 fabrication (#677 discipline)

The propositions restrict themselves to AIF-native tokens "confirmed by existing usage". The audit
builds the inventory deterministically (every `*_Inference`/`*_Conflict` token in any AIF column of
any row) and checks each proposed token against it.

**Inventory size**: 54 distinct tokens (18 `*_Conflict` + 35 `*_Inference` + 1 edge case). **Every
one of the 10 distinct tokens proposed across the 5 docs is native:**

| Proposed token | Native? | Occurrences | Example pks (where already used) |
|----------------|---------|-------------|----------------------------------|
| `Analogy_Inference` | ✅ | 1 | 839 |
| `DifferencesUndermineSimilarity_Conflict` | ✅ | 1 | 839 |
| `BiasedClassification_Conflict` | ✅ | 2 | 799, 833 |
| `ExceptionSimilarityCase_Conflict` | ✅ | 2 | 614, 833 |
| `VerbalClassification_Inference` | ✅ | 1 | 799 |
| `ArbitraryVerbalClassification_Inference` | ✅ | 3 | 177, 799, 846 |
| `PropertyNotExistant_Conflict` | ✅ | 3 | 621, 633, 804 |
| `VagueVerbalClassification_Inference` | ✅ | 2 | 800, 856 |
| `Logical_Conflict` | ✅ | 1 | 696 |
| `InconsistentCommitment_Inference` | ✅ | 2 | 777, 1361 |

**Verdict: 0 fabricated tokens.** The #677 "0 fabrication" discipline is fully upheld. No proposition
invents a `*_Conflict` or `*_Inference` token; where a native token is missing, the proposition
FAIL-LOUDs (see §5).

> **Minor secondary note (not a FLAG)**: PR-2 §1 cites the inventory as "26 Conflict nodes / 36
> Inference schemes"; the empirical count on `34c7702c` is 18 Conflict + 35 Inference = 53. The
> delta is likely a counting-method difference (the doc's figure was from master `dc02e847` and may
> count case variants or sub-schemes differently). This does not affect the audit — the load-bearing
> claim (proposed tokens are native) holds 10/10. Worth a refresh of the cited count if the
> inventory doc is regenerated, but not a blocker.

---

## 5. FAIL-LOUD legitimacy (6 cases — all verified legitimate)

A FAIL-LOUD is legitimate only if the missing native token *genuinely does not exist* in the
inventory. Each of the 6 verified:

| pk | doc | FAIL-LOUD reason | Missing token | In inventory? | Verdict |
|----|-----|------------------|---------------|---------------|---------|
| 840 | PR-1 | circularity in analogy (CA-node missing) | a `Circularity_Conflict` / question-begging node | ❌ absent | ✅ legit |
| 829 | PR-6 | circularity in definition (CA-node missing) | same as 840 | ❌ absent | ✅ legit |
| 832 | PR-6 | vague meta-fallacy (RA-node missing) | any determinate scheme | ❌ no scheme fits the indeterminate desc | ✅ legit |
| 834 | PR-2 | pure comparison (RA-node missing) | `Comparison_Inference` | ❌ absent | ✅ legit |
| 835 | PR-2 | pure comparison (RA-node missing) | `Comparison_Inference` | ❌ absent | ✅ legit |
| 837 | PR-2 | pure comparison (RA-node missing) | `Comparison_Inference` | ❌ absent | ✅ legit |

**Two distinct FAIL-LOUD layers** (as the docs claim):
- **CA-missing** (840, 829): a legitimate scheme exists (RA-node present), but no native Conflict
  node captures circularity. These could be *typed* as undercuts if the schema evolves, but the
  CA-node token is absent today.
- **RA-missing** (832, 834, 835, 837): no native Inference scheme exists at all (pure comparison,
  or too-vague desc). Structurally prior — nothing to attack.

`Comparison_Inference` and any `Circularity_Conflict`/`QuestionBegging_Conflict` are genuinely
absent from the 54-token inventory → the FAIL-LOUDs are honest, not shortcuts.

---

## 6. ⚠ FLAG — coverage-accounting consistency (the one finding jsboige should settle)

**The data is clean; the *counting criterion* is applied inconsistently across PRs, making the
chantier headline number "86 fully-modeled" depend on which PR's convention you use.**

The two FAIL-LOUD layers (§5) are counted differently by different PRs:

| Leaf | Shape | RA-node | CA-node | Counting PR | Counted as "fully-modeled"? |
|------|-------|---------|---------|-------------|-----------------------------|
| **840** (PR-1) | CA-missing (circularity) | ✅ `Analogy_Inference` | ❌ absent | **PR-1** | **YES** (PR-1 §5: "+4 mapped leaves → 74") |
| **829** (PR-6) | CA-missing (circularity) | ✅ `VerbalClassification_Inference` | ❌ absent | **PR-6** | **NO** (PR-6 §4: "5 fully-modeled + 2 FAIL-LOUD") |

**840 and 829 are the same shape** (RA exists, CA missing — circularity, no native node), yet PR-1
counts 840 in its "+4 mapped" while PR-6 excludes 829 from its "+5 fully-modeled". The chantier
headline "86 fully-modeled" inherits this 1-leaf ambiguity:

- If PR-6's stricter criterion (CA-missing = NOT fully-modeled) is applied uniformly → 840 is also
  partial → chantier = **85 fully-modeled** (not 86).
- If PR-1's looser criterion (RA-exists = mapped) is applied uniformly → 829 is also mapped →
  chantier = **87 fully-modeled** (and 832/834/835/837, which are RA-missing, still excluded).

**Recommendation for jsboige**: ratify a single explicit "fully-modeled" criterion before the
headline number is committed to the release notes / OWL regen. The audit's reading is that the
**stricter criterion (85)** is more honest — a leaf with a missing CA-node has a documented gap
(the conflict is not typed), so calling it "fully-modeled" overstates the mapping. But this is
jsboige's call; the propositions themselves are internally honest either way (both layers are
documented, just counted differently).

**This FLAG does not block ratification of the *propositions*** — the 21 leaf proposals are clean
(§1-§5). It only asks jsboige to pick one counting convention so the coverage number is reproducible.

---

## 7. Ratification-ready summary (per dispatch SECONDARY)

Given the audit is clean (modulo the §6 accounting FLAG), the chantier is **ratification-ready**.
For jsboige's decision, the verified inventory:

**Native AIF tokens used by the 5 propositions** (all confirmed native, reuse-only):
- **Inference schemes (RA-nodes)**: `Analogy_Inference`, `VerbalClassification_Inference`,
  `ArbitraryVerbalClassification_Inference`, `VagueVerbalClassification_Inference`,
  `InconsistentCommitment_Inference`.
- **Conflict nodes (CA-nodes)**: `DifferencesUndermineSimilarity_Conflict`,
  `BiasedClassification_Conflict`, `ExceptionSimilarityCase_Conflict`, `PropertyNotExistant_Conflict`,
  `Logical_Conflict`.

**Two FAIL-LOUD layers documented (honest gaps, not fabricated)**:
- **CA-missing** (circularity): 840, 829 — would become `undercut` if a `Circularity_Conflict` /
  question-begging node is added to the AIF ontology (terminal decision for jsboige).
- **RA-missing** (no native scheme): 832 (vague meta-fallacy), 834/835/837 (pure comparison) —
  would require a `Comparison_Inference` scheme or a re-expression as `Example_Inference` /
  `Sign_Inference` (semantic stretch, lower fidelity).

**Schema decision (out of audit scope, flagged by #707 §4)**: the I-node/RA-node/CA-node
decomposition is recorded in every PR's §7 but **not serialized** — it awaits jsboige's call on
new columns (`AIF_attackType`, `AIF_attackedNode`). This audit is independent of that decision
(borrow-root modeling holds either way).

---

## Gate boundaries (HARD — read-only audit)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No change to the 5 proposition docs (this audit comments on them, does not edit).
- ✅ Audit derived code=truth from the taxonomy CSV via a deterministic read-only script
  (`498_audit_extract.py`, scratchpad-only, not committed — reproducible on `34c7702c`).
- ✅ Adversarial: 1 FLAG raised (§6), not a rubber-stamp.

Relates: dispatch `awhj8g` (PRIMARY), #498 (chantier), PR-1 #699, PR-2 #701, PR-3 #703, PR-4 #705,
PR-6 #708, coverage-status #707, #677 (0 fabrication), #133/#130 (existing OWL), #499 (inverse), #458.

---

# Addendum 2026-07-06 — PR-7/8/9 (Ambiguïté family) + chantier count reconciliation

**Scope**: independent code=truth verification of the 3 cluster propositions produced by po-2023
after the original audit — PR-7 #711 (Amphibologie, 8 rows), PR-8 #713 (Narrative ambiguity /
insinuation, 4 leaves), PR-9 #714 (Narrative ambiguity / deception, 6 leaves). 18 leaves + 2 anchors
(846, 876). Same adversarial method as §1-§7 (deterministic read-only extractor, no LLM judgment on
the data). Also delivers the **SECONDARY** count-reconciliation: one authoritative chantier headline
number (resolves the §6 FLAG, now compounded by po-2023's PR-8/9 "dual criterion").

**Author**: po-2024 (worker) · **Dispatch**: `ynv05a` (PRIMARY + SECONDARY, ai-01 2026-07-06 01:32)
**Base**: master `bdba45d8` · **Read-only**: 0 write under `Cards/`, audit addendum only.

---

## A. TL;DR — verdict (PR-7/8/9)

**The 3 Ambiguïté-family propositions are code=truth-clean**, consistent with the original 5. Every
pk exists, both anchors reproduce byte-exact, all 15 distinct proposed tokens are native (0
fabrication), all cited `desc_fr` match byte-close (2 cosmetic diffs), and the 3 FAIL-LOUD cases
(PR-7 847/848/853, syntactic-ambiguity gap) are legitimate. **Count reconciliation: the chantier
headline is 100 fully-modeled (strict, uniform) — po-2023's "inclusive 101" is non-uniform and should
be dropped (§H).**

| Check | Result |
|-------|--------|
| 20 cited pks present in CSV | ✅ 20/20 |
| 2 mapped anchors (846, 876) — text/DirectRef/ExceptionRef/MappingType byte-exact | ✅ 2/2 |
| 15 distinct proposed tokens — native (in existing inventory) | ✅ 15/15 (0 fabrication) |
| 18 cited `desc_fr` (where docs cite verbatim) — byte-close | ✅ 18/18 (2 cosmetic: apostrophe + 878 casing) |
| 3 FAIL-LOUD cases (PR-7 847/848/853) — legitimate | ✅ 3/3 (missing syntactic-ambiguity CA genuinely absent) |
| Native AIF inventory (re-derived on `bdba45d8`) | 54 tokens (35 `*_Inference` + 19 `*_Conflict`) |
| Chantier count — uniform strict criterion | **100 fully-modeled** (§H) |

---

## B. Anchor verification (the 2 mapped anchors the Ambiguïté propositions lean on)

PR-7 borrows the d2 subfamily root **846** (no in-sub-sub anchor in Amphibologie); PR-8 uses the
in-sub-sub anchor **876**. Both must match the docs byte-exact (load-bearing — a wrong anchor
invalidates the cluster pattern). **Both match.**

| pk | doc | text_fr (claim → CSV) | DirectRef | ExceptionRef | MappingType | Verdict |
|----|-----|-----------------------|-----------|--------------|-------------|---------|
| 846 | PR-7 | "Ambiguïté" → "Ambiguïté" | `ArbitraryVerbalClassification_Inference` + `OppositeConsequences_Conflict` + `SignFromOtherEvents_Conflict` → exact | (empty) → exact | `skos:broadMatch` → exact | ✅ PASS |
| 876 | PR-8/9 | "Ambiguïté narrative" → "Ambiguïté narrative" | `ConflictingGoals_Conflict` → exact | (empty) → exact | `skos:broadMatch` → exact | ✅ PASS |

---

## C. Cluster boundaries (`Soussousfamille` FR canonical — code=truth)

| Cluster (doc) | Claimed pk range | CSV `Soussousfamille` (FR, all rows in range) | Verdict |
|---------------|------------------|-----------------------------------------------|---------|
| Amphibologie (PR-7) | 847-854 | `Amphibologie` ×8 | ✅ PASS |
| Narrative ambiguity / insinuation (PR-8) | 877-880 | `Ambiguïté narrative` ×4 | ✅ PASS |
| Narrative ambiguity / deception (PR-9) | 881-886 | `Ambiguïté narrative` ×6 | ✅ PASS |

The pipeline hierarchy is driven by the FR `Soussousfamille` column (CLAUDE.md); all three clusters
are boundary-clean on it. **Side-note (not a boundary error)**: pk 882 and 885 carry an empty EN
`Subsubfamily` column while their FR `Soussousfamille` is correctly filled — this is the
**#712 i18n-propagation gap** (FR filled / EN empty, 48 rows), already inventoried; pk 882 appears in
the #712 spot-check. The Ambiguïté family thus intersects the #712 gap; the AIF propositions are
independent of it (AIF columns are language-agnostic metadata).

---

## D. Native vocabulary — 0 fabrication (#677 discipline, 15 tokens)

The inventory re-derived on `bdba45d8` holds at **54 distinct native tokens** (35 `*_Inference` +
19 `*_Conflict`). Every one of the 15 distinct tokens proposed across PR-7/8/9 is native:

| Proposed token | Native? | Occurrences | Example pks (where already used) | Used by |
|----------------|---------|-------------|----------------------------------|---------|
| `ArbitraryVerbalClassification_Inference` | ✅ | 3 | 177, 799, 846 | PR-7 anchor 846, 851, 852 |
| `OppositeConsequences_Conflict` | ✅ | 5 | 3, 697, 759, 846, 1090 | PR-7 anchor 846 |
| `SignFromOtherEvents_Conflict` | ✅ | 3 | 357, 846, 1371 | PR-7 anchor 846 |
| `VerbalClassification_Inference` | ✅ | 1 | 799 | PR-7 847/848/853 (RA-node) |
| `Deductive_Inference` | ✅ | 2 | 726, 758 | PR-7 849, 850 |
| `Logical_Conflict` | ✅ | 1 | 696 | PR-7 849, 850 |
| `VagueVerbalClassification_Inference` | ✅ | 2 | 800, 856 | PR-7 854 (cross-cluster PR-4) |
| `ConflictingGoals_Conflict` | ✅ | 3 | 356, 876, 973 | PR-8 anchor 876, 877, 878, 884, 886 |
| `OpposedCommitment_Conflict` | ✅ | 2 | 777, 1297 | PR-8 879 |
| `PositionToKnow_Inference` | ✅ | 2 | 70, 888 | PR-8 880, PR-9 882 |
| `LackOfCompleteKnowledge_Conflict` | ✅ | 1 | 133 | PR-8 880, PR-9 882 |
| `ExpertOpinion_Inference` | ✅ | 1 | 71 | PR-9 881, 883 |
| `ExpertiseInconsistency_Conflict` | ✅ | 1 | 71 | PR-9 881, 883 |
| `PracticalReasoning_Inference` | ✅ | 2 | 70, 697 | PR-9 884 |
| `Bias_Inference` | ✅ | 2 | 70, 1023 | PR-9 885 |

**Verdict: 0 fabricated tokens (15/15 native).** The #677 discipline holds across all 8 PR. PR-8/9
expand the demonstrated native palette from 10 (PR-1..6) to 21 distinct tokens (PR-8 adds
`OpposedCommitment_Conflict`, `LackOfCompleteKnowledge_Conflict`, `PositionToKnow_Inference`,
`ConflictingGoals_Conflict`; PR-9 adds `ExpertOpinion_Inference`,
`ExpertiseInconsistency_Conflict`, `PracticalReasoning_Inference`, `Bias_Inference`) — all native,
no fabrication.

> **Inventory-size note (not a FLAG)**: the original audit (§4) cited 54 tokens as "18 Conflict + 35
> Inference + 1 edge"; the re-derivation on `bdba45d8` gives 35 Inference + 19 Conflict = 54 (no
> edge-case remainder under the strict `*_Inference`/`*_Conflict` regex). The +1 Conflict vs the §4
> narrative count is a counting-method nuance, not a regression — the load-bearing claim (15/15
> proposed tokens native) is unaffected.

---

## E. Per-leaf verdict (18 leaves)

### PR-7 — Amphibologie (borrow-anchor 846 + 8 rows: 847 d3 anchor + 848-854 leaves)

| pk | text_fr (CSV) | Proposed ExceptionRef | Proposed DirectRef | MappingType | Verdict |
|----|---------------|----------------------|--------------------|-------------|---------|
| 847 | Amphibologie | `VerbalClassification_Inference` | *(FAIL-LOUD: syntactic ambiguity, no native CA)* | `skos:closeMatch` | ✅ data-OK; FAIL-LOUD legit (§G) |
| 848 | Ponctuation ambiguë | `VerbalClassification_Inference` | *(FAIL-LOUD: structural ambiguity)* | `skos:narrowMatch` | ✅ data-OK; FAIL-LOUD legit |
| 849 | Sophisme de portée modale | `Deductive_Inference` | `Logical_Conflict` | `skos:closeMatch` | ✅ PASS (scheme-divergence honest) |
| 850 | Glissement du quantificateur | `Deductive_Inference` | `Logical_Conflict` | `skos:narrowMatch` | ✅ PASS |
| 851 | Accent | *(absent — direct-conflict)* | `ArbitraryVerbalClassification_Inference` | `skos:closeMatch` | ✅ PASS (borrow-root reuse) |
| 852 | Contraste illicite | *(absent)* | `ArbitraryVerbalClassification_Inference` | `skos:narrowMatch` | ✅ PASS |
| 853 | Solécisme | `VerbalClassification_Inference` | *(FAIL-LOUD: grammar-induced ambiguity)* | `skos:narrowMatch` | ✅ data-OK; FAIL-LOUD legit |
| 854 | Barbarisme | *(absent)* | `VagueVerbalClassification_Inference` | `skos:narrowMatch` | ✅ PASS (cross-cluster PR-4) |

`desc_fr` 9/9 byte-close (PR-7 citations use a straight apostrophe `'` vs the CSV's curly `'` — same
cosmetic variant already noted for 841 in §3; no substantive diff).

### PR-8 — Narrative ambiguity / insinuation (in-sub-sub anchor 876 + 4 leaves)

| pk | text_fr (CSV) | Proposed ExceptionRef | Proposed DirectRef | MappingType | Verdict |
|----|---------------|----------------------|--------------------|-------------|---------|
| 877 | Fausse implication | *(absent — direct-conflict)* | `ConflictingGoals_Conflict` | `skos:narrowMatch` | ✅ PASS (anchor reuse) |
| 878 | Argument par l'insinuation | *(absent)* | `ConflictingGoals_Conflict` | `skos:closeMatch` | ✅ PASS |
| 879 | Compliment empoisonné | *(absent)* | `OpposedCommitment_Conflict` | `skos:closeMatch` | ✅ PASS (semantic-closer conflict) |
| 880 | Indiscrétion anonyme | `PositionToKnow_Inference` | `LackOfCompleteKnowledge_Conflict` | `skos:narrowMatch` | ✅ PASS (genuine exception) |

`desc_fr`: 876/877 cited byte-close; **878 cosmetic capitalization diff** (`Choses` in doc vs
`choses` in CSV — same cosmetic class as the apostrophe variant, no meaning change); 879/880 discuss
semantics without a verbatim citation, and the semantic readings (879 praise/discredit opposition,
880 anonymous-source epistemic gap) are faithful to the CSV descs. 0 FAIL-LOUD.

### PR-9 — Narrative ambiguity / deception (anchor 876 recap + 6 leaves)

| pk | text_fr (CSV) | Proposed ExceptionRef | Proposed DirectRef | MappingType | Verdict |
|----|---------------|----------------------|--------------------|-------------|---------|
| 881 | Propagande grise | `ExpertOpinion_Inference` | `ExpertiseInconsistency_Conflict` | `skos:narrowMatch` | ✅ PASS (concealment) |
| 882 | Campagne de murmures | `PositionToKnow_Inference` | `LackOfCompleteKnowledge_Conflict` | `skos:narrowMatch` | ✅ PASS (cross-PR-8 reuse) |
| 883 | Propagande noire | `ExpertOpinion_Inference` | `ExpertiseInconsistency_Conflict` | `skos:closeMatch` | ✅ PASS (falsification) |
| 884 | Interrogatoire clandestin | `PracticalReasoning_Inference` | `ConflictingGoals_Conflict` | `skos:narrowMatch` | ✅ PASS (anchor reuse) |
| 885 | Prêcher le faux pour savoir le vrai | *(absent — direct-conflict)* | `Bias_Inference` | `skos:narrowMatch` | ✅ PASS (scheme-as-defect) |
| 886 | Polytélie | *(absent)* | `ConflictingGoals_Conflict` | `skos:closeMatch` | ✅ PASS (anchor reuse, literal) |

`desc_fr` 6/6 byte-close (curly apostrophes match between doc and CSV). 0 FAIL-LOUD. The omission-
vs-commission distinction (881 concealment `narrowMatch` vs 883 falsification `closeMatch`, same
scheme+conflict pair) is a coherent MappingType grading, not a contradiction.

---

## F. FAIL-LOUD legitimacy (3 cases — all verified legitimate)

PR-7's 3 FAIL-LOUD cases (847/848/853) are all **CA-missing** (RA-node `VerbalClassification_Inference`
present, no native Conflict node captures syntactic / structural / grammar-induced ambiguity). Each
is legitimate only if a native syntactic-ambiguity Conflict token genuinely does not exist:

| pk | doc | FAIL-LOUD reason | Hypothetical missing token | In inventory? | Verdict |
|----|-----|------------------|----------------------------|---------------|---------|
| 847 | PR-7 | syntactic ambiguity (amphiboly, CA-node missing) | `SyntacticAmbiguity_Conflict` / `Amphiboly_Conflict` / `Ambiguity_Conflict` | ❌ all absent | ✅ legit |
| 848 | PR-7 | structural ambiguity via punctuation (CA-node missing) | `StructuralAmbiguity_Conflict` | ❌ absent | ✅ legit |
| 853 | PR-7 | grammar-induced ambiguity / solecism (CA-node missing) | `GrammarAmbiguity_Conflict` | ❌ absent | ✅ legit |

The 3 cases are the **same gap shape** as the §5 CA-missing circularity cases (840, 829): a legitimate
scheme exists (RA-node present), but no native Conflict node captures the defect. They form a
**fourth recurring AIF ontology gap — ambiguity** (after circularity PR-1/6, pure-comparison PR-2,
and the implicature-RA gap PR-8 flags in §7). All hypothetical tokens were checked absent against the
54-token inventory → the FAIL-LOUDs are honest, not shortcuts.

---

## G. ⚠ SECONDARY — chantier count reconciliation (the one authoritative number)

**The §6 FLAG is now compounded.** po-2023's PR-8/9 docs report a "dual criterion" of 100 strict /
101 inclusive, but the "inclusive 101" is itself **non-uniform**: it counts pk 840 (PR-1, CA-missing)
as fully-modeled while excluding pk 829 (PR-6) and pk 847/848/853 (PR-7) — **four leaves of the exact
same CA-missing shape** (RA present, CA absent). A criterion that includes one CA-missing leaf but
excludes four others is not a criterion; it is PR-1's original over-count preserved by inertia.

### The two uniform criteria

| Criterion | Definition | Count |
|-----------|-----------|------:|
| **Strict (uniform)** | fully-modeled = BOTH scheme (RA) AND conflict (CA) present | **100** |
| **Inclusive (uniform)** | fully-modeled = RA present (CA-missing also counted) | 105 |
| ~~po-2023 "inclusive"~~ | PR-1's original convention (840 only) | ~~101~~ — **non-uniform, drop** |

### Reconciliation table (uniform strict criterion, chantier-wide)

| PR | Cluster | Rows | Strict fully-modeled | CA-missing (RA✓/CA✗) | RA-missing (no scheme) |
|----|---------|-----:|---------------------:|----------------------|------------------------|
| baseline | — | — | 70 | — | — |
| PR-1 #699 | False analogy | 4 | 3 | 840 (circularity) | — |
| PR-2 #701 | Faulty comparison | 5 | 2 | — | 834, 835, 837 (pure comparison) |
| PR-3 #703 | Association fallacy | 2 | 2 | — | — |
| PR-4 #705 | Vague definition | 3 | 3 | — | — |
| PR-6 #708 | Inconsistent definition | 7 | 5 | 829 (circularity) | 832 (vague meta) |
| PR-7 #711 | Amphibologie | 8 | 5 | 847, 848, 853 (ambiguity) | — |
| PR-8 #713 | Narrative ambiguity / insinuation | 4 | 4 | — | — |
| PR-9 #714 | Narrative ambiguity / deception | 6 | 6 | — | — |
| **total** | **4 subfamilies** | **39** | **100** | **5** | **4** |

**Authoritative headline**: **100 fully-modeled (strict)** out of 109 addressed (70 baseline + 39
new) = 91.7%. The 9 partial leaves are honestly tracked as two gap families:

- **5 CA-missing** (would-be undercuts, conflict node gap): circularity ×2 (840, 829) + syntactic /
  structural / grammar ambiguity ×3 (847, 848, 853). These become fully-modeled the moment jsboige
  ratifies an ontology extension adding a `Circularity_Conflict` (question-begging) node and a
  syntactic-ambiguity Conflict node → **+5 undercuts, chantier → 105**.
- **4 RA-missing** (no native scheme): pure comparison ×3 (834, 835, 837) + vague meta-fallacy ×1
  (832). These need a `Comparison_Inference` scheme or a re-expression as `Example_Inference` /
  `Sign_Inference` (semantic stretch) → lower-fidelity, deferred.

**Recommendation for jsboige**: commit **100 fully-modeled (strict)** as the single chantier
headline for the release notes / OWL regen. Drop the "inclusive 101" — it is not a uniform criterion.
The two gap families (5 CA-missing, 4 RA-missing) are the agenda for the eventual ontology-extension
decision, which is more useful surfaced as 9 honest gaps than lumped into an inconsistent headline.

**This reconciliation does not block ratification of the 8 propositions** (PR-1..4/6..9, 39 rows) —
all 39 are data-clean (§1-§5 + §B-§F). It only fixes the headline number so the coverage claim is
reproducible.

---

## H. Ratification-ready summary (PR-7/8/9 + count)

The 3 Ambiguïté-family propositions are **ratification-ready** (data-clean, 0 fabrication, FAIL-LOUDs
legitimate). Combined with the original 5, the chantier now covers **4 subfamilies / 39 rows** with
**100 fully-modeled (strict)** + 9 documented gaps.

**Native AIF tokens added by PR-7/8/9** (all confirmed native, reuse-only):
- **Inference schemes (RA-nodes)**: `VerbalClassification_Inference`, `Deductive_Inference`,
  `VagueVerbalClassification_Inference`, `PositionToKnow_Inference`, `ExpertOpinion_Inference`,
  `PracticalReasoning_Inference`, `Bias_Inference`.
- **Conflict nodes (CA-nodes)**: `OppositeConsequences_Conflict`, `SignFromOtherEvents_Conflict`,
  `Logical_Conflict`, `ConflictingGoals_Conflict`, `OpposedCommitment_Conflict`,
  `LackOfCompleteKnowledge_Conflict`, `ExpertiseInconsistency_Conflict`.

**New FAIL-LOUD gap family (PR-7)**: ambiguity (3 CA-missing leaves) — joins circularity (PR-1/6),
pure-comparison (PR-2), implicature-RA (PR-8 §7) as the chantier's documented ontology gaps.

---

## Gate boundaries (HARD — read-only audit addendum)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted.
- ❌ No change to the 3 proposition docs (this addendum comments on them, does not edit).
- ✅ Audit derived code=truth from the taxonomy CSV via a deterministic read-only script
  (`498_audit_extract_pr789.py`, scratchpad-only, not committed — reproducible on `bdba45d8`).
- ✅ Adversarial: count-reconciliation FLAG resolved to one authoritative number (§G), not a
  rubber-stamp of po-2023's dual-criterion headline.

Relates: dispatch `ynv05a` (PRIMARY + SECONDARY), #498 (chantier), PR-7 #711, PR-8 #713, PR-9 #714,
PR-1 #699, PR-2 #701, PR-3 #703, PR-4 #705, PR-6 #708, coverage-status #707, family-gap #712,
#677 (0 fabrication), #133/#130 (existing OWL), #499 (inverse), #458. Base `bdba45d8`.
