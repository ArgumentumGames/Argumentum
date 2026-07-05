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
