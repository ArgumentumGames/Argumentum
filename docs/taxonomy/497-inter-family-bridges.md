# 2026-07-22 — #497 inter-family bridges — **84 cross-family near-duplicate leaf pairs, all unlinked**

**Scope** : primary deliverable of ai-01 dispatch `msg-9akcg4` (strate-6 deep-queue, post-T&A regime).
Drills my own #834 finding (densification saturated INTRA-grappe → the ROI is INTER-family bridges) down to the
**leaf level**. Result reframes the gap : it is not « a few sparse lexical leads » — it is a **systematic
structural gap of 84 cross-family near-duplicate leaf pairs, all currently UNLINKED inter-family**.
**Proposition only — GATED, 0 write to prod CSV.**

**Repo reference** : master `3a87eb0e`. Issue : #497. Companion to #834 (substrate refresh, MERGED `15c9fd22`),
#833 (spectral consumption contract, MERGED `b442c658`), #837 (#498 cluster, MERGED `3a87eb0e`).
Downstream consumer : CoursIA ICT #7289 strate-6 Phase-B (the typed graph these `Mirrors` edges densify).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies → worker applies
> the `crossLink_Mirrors` edits in a gated follow-up PR, same flow as #753/#760. The 84 proposed bridges are
> recorded here + in the machine-readable `497-inter-family-bridges-annotations.csv` (85 rows, all 84 pairs).

---

## TL;DR

- **The #497 inter-family gap is systematic, not sparse.** A leaf-level Jaccard scan (desc_fr distinctive
  vocabulary, ≥ 4 shared distinctive terms, J ≥ 0.45) surfaces **84 cross-family leaf pairs** that are
  **near-duplicates** — the *same fallacy catalogued under two (or more) different families*, with identical or
  word-for-word descriptions. **0 of the 84 are currently linked inter-family.**
- **All 84 are `Mirrors`** — the taxonomy's native structural-mirror relation (304 existing uses, code=truth
  attested). No fabricated relation type, no coined token (0 fabrication #677).
- **My #834 lexical leads converge / diverge honestly :**
  - **Causal lead (Interprétation-quant ↔ Causalité-douteuse)** → **converges** : P635 ↔ P725 *is* one of the 84
    (identical desc + identical example « le merle chante… le soleil se lève »). The single strongest bridge in
    the set.
  - **Attaque-affect lead (Appel-émotion ↔ Ad-hominem)** → **diverges** : the top candidates (P324 Appel au
    ridicule ↔ P1403 Injure ; P342 Appel à la véhémence ↔ P1401 Raillerie) are **lexical-affinity** bridges
    (related concepts, NOT duplicates). Reported separately at moderate confidence, fail-loud flagged.
- **The 2.3 ↔ 6.3 PredatesOn anchor is already populated at leaf level** (7+ edges 2.3* → 6.3*, e.g. P358/P369/
  P377/P383/P390/P439/P440) — it is **not a gap**, and it is a *different* relation type (semantic/evolutionary
  `PredatesOn`, not structural `Mirrors`). Documented as the structural backbone, left untouched.
- **Coverage impact** : +84 inter-family `Mirrors` edges on the typed graph. This is the highest-ROI, most
  defensible inter-family densification possible — identical descriptions are an undeniable signal, not a
  heuristic guess.

---

## 1. Cluster state (code=truth, master `3a87eb0e`)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` (1 408 fallacies, 1 379 leaves with `desc_fr`) :

| Metric | Value |
|--------|------:|
| Leaves with `desc_fr` (analysis population) | 1 379 |
| Inter-family near-duplicate pairs (J ≥ 0.45, ≥ 4 shared distinctive terms) | **84** |
| … of which already linked inter-family | **0** |
| Distinct duplicate-name groups (a concept duplicated ≥ 1×) | 70 |
| … groups with ≥ 3 copies (triples/quadruples) | 8 |
| Attested crossLink relation vocabulary (native, no coinage) | 8 types (Leverages 350, Mirrors 304, IsRelatedTo 286, Allows 62, Inverts 41, Opposes 23, PredatesOn 13, Denounces 2) |

**Why these 84 are ironclad.** A near-duplicate pair is the same *named fallacy* (e.g. « Homme de paille »,
« Rationalisation », « Preuve anecdotique ») appearing in two families with descriptions that share ≥ 4
distinctive content terms at Jaccard ≥ 0.45 (most are J = 1.00 — word-for-word identical). This is not lexical
affinity (shared genre vocabulary) — it is **content identity**. Semantic verification of a sample (§3) confirms
every checked pair is a genuine cross-family duplicate.

### Family-pair distribution (the 84 bridges span all inter-family pairs, but cluster on Tricherie)

| Family pair | bridges | | Family pair | bridges |
|---|---:|---|---|---:|
| Influence ↔ Tricherie | 11 | | Abus de langage ↔ Influence | 4 |
| Obstruction ↔ Tricherie | 10 | | Erreur mathématique ↔ Tricherie | 4 |
| Insuffisance ↔ Tricherie | 9 | | Erreur de rais. ↔ Obstruction | 4 |
| Influence ↔ Obstruction | 7 | | Abus de langage ↔ Tricherie | 4 |
| Erreur de rais. ↔ Insuffisance | 4 | | Abus de langage ↔ Obstruction | 4 |
| Insuffisance ↔ Obstruction | 4 | | *(+ 9 other pairs, ≤ 3 each)* | … |

**Insuffisance and Tricherie are the duplication hubs.** Insuffisance (the root cognitive-bias family) is the
origin of most duplicates — its members are re-catalogued under the more specific error families (Erreur de
raisonnement, Erreur mathématique) and under Tricherie/Obstruction where the bias manifests as a tactic. This is
a genuine structural property of the taxonomy's construction, not an artefact.

### Triples / quadruples (concepts in ≥ 3 families)

| concept | copies | families |
|---|---:|---|
| Homme de paille | 4 | Insuffisance, Tricherie, Obstruction (+1) |
| Piège de gratitude | 4 | (4 families) |
| Gish gallop | 4 | (4 families) |
| *(+ 5 other triples)* | 3 each | … |

---

## 2. The relation : `Mirrors` (native, 0 fabrication #677)

`crossLink_Mirrors` is the taxonomy's structural-mirror relation — **304 existing uses** (code=truth on master),
the second-most-used relation after `Leverages` (350). A cross-family near-duplicate *is* the canonical
`Mirrors` case : two nodes that are structurally the same fallacy, catalogued separately.

**Discipline applied (consistent with #677 / `[[aif-no-inherit-attacktype-from-anchor]]`) :**
- **No coined relation type.** Only the 8 attested relations are used ; `Mirrors` fits the near-duplicate
  semantics exactly.
- **No AIF token fabricated.** These are `crossLink_*` (game-content relations), not AIF attack-columns — a
  different layer from the #498 AIF modelling. No `attackType` / `skos` token is touched.
- **Bidirectional emission.** `Mirrors` is a symmetric relation (emitted as two distinct triples
  `(A,p,B)` + `(B,p,A)` in the OWL, per #828 README) — each proposed bridge is one logical mirror, two directed
  triples at serialization (handled by the OWL serializer, not hand-authored).

---

## 3. Semantic verification (sample — discipline : read the real text, not the score)

A Jaccard score is a *generator of candidates, not proof* (my own #834 finding warned about heuristic noise —
second-person verbs « faites / utilisez / votre »). I read the actual `desc_fr` of a sample to confirm the
near-duplicates are genuine cross-family duplicates, not false positives :

| pair | families | P-A desc_fr | P-B desc_fr | verdict |
|---|---|---|---|---|
| P168 ↔ P894 ↔ P1365 (Homme de paille) | Insuff / Tricherie / Obstruction | « caricaturez l'argument… pour réfuter » | « caricaturez l'argument… afin de réfuter » | ✅ identical (trivial « pour/afin de » variant) |
| P62 ↔ P762 (Rationalisation) | Insuff / Err.rais. | « présentez des justifications peu sincères… » | *word-for-word identical* | ✅ identical |
| P34 ↔ P1087 (Preuve anecdotique) | Insuff / Tricherie | « basez votre argumentation sur un événement isolé… » | *word-for-word identical* | ✅ identical |
| P179 ↔ P701 (Question piège) | Influence / Err.rais. | « posez une question qui suppose subtilement une prémisse… » | *word-for-word identical* | ✅ identical |
| P635 ↔ P725 (Confusion antériorité/causalité) | Err.math / Err.rais. | « supposez qu'un événement est la cause… uniquement parce qu'il l'a précédé » | *word-for-word identical* + identical example (merle/soleil) | ✅ identical — **the #834 causal lead** |

**Every checked pair is a genuine cross-family duplicate.** The ≥ 4-shared-distinctive-terms threshold plus the
J ≥ 0.45 filter exclude short-description coincidences ; the verification confirms content identity.

**Current link state (why these are NEW bridges) :** the checked pairs carry **no inter-family crossLink**
between the duplicate leaves. P168 (Homme de paille) has *no crossLink at all* ; P894 links only to the
family-1 root (`IsRelatedTo -> 1`), not to its Insuffisance twin P168. The inter-family mirror is missing in
every case — these are genuinely unmodelled bridges.

---

## 4. The lexical leads from #834 — convergence and honest divergence

My #834 grappe-level finding flagged 2 lexical leads (Jaccard on the grappes' distinctive vocabulary). The
leaf-level drill resolves each honestly :

### 4a. Causal lead — **CONVERGES** (in the near-duplicate set) ✅
- **P635 (Confusion entre antériorité et causalité, Err.math 3.2) ↔ P725 (Appel à la temporalité comme cause,
  Err.rais. 4.1)** : J = 1.00, 14 shared distinctive terms, **identical description + identical example**. This
  is one of the 84 near-duplicates (§3 last row). Proposed : `Mirrors`.
- The other causal grappe-pair candidates (P661↔P699, P652↔P708, P638↔P718, P636↔P721) scored J 0.09–0.11 on
  generic shared terms (*argument, chaque, après, toujours, compte*) → **fail-loud, not proposed** (heuristic
  noise, no content identity).

### 4b. Attaque-affect lead — **DIVERGES** (lexical affinity, not duplicates) ⚠
- The top attaque-affect candidates (P324 Appel au ridicule ↔ P1403 Injure ; P342 Appel à la véhémence ↔ P1401
  Raillerie) are **NOT** in the near-duplicate set — they are *related but distinct* concepts (ridicule vs
  insult, vehemence vs mockery) sharing genre vocabulary (*adversaire, interlocuteur, discréditer*).
- These are genuine **lexical-affinity bridges** at moderate confidence. Proposed relation : `Leverages` (the
  Influence affect-appeal *leverages* the Obstruction discredit move) or `IsRelatedTo`. **Fail-loud flagged** —
  weaker signal than the near-duplicates, deferred to a separate curation pass (reported here for completeness,
  not in the 84-pair CSV).

### 4c. The 2.3 ↔ 6.3 PredatesOn anchor — **already populated, not a gap** ✅
- My #834 grappe-bubble showed the 2.3 ↔ 6.3 `PredatesOn` edge (poids 7) as the inter-family anchor. Leaf-level
  inspection confirms it is **already well-developed** : 7+ leaf-level `PredatesOn` edges run 2.3* → 6.3* (P358,
  P369, P377, P383, P390, P439, P440) — the grappe-bubble weight was the aggregated view of these real edges.
- It is a **different relation type** (`PredatesOn` = semantic/evolutionary precedence, 13 uses) from the
  `Mirrors` near-duplicates. It is the structural backbone, **left untouched** (no gap, no proposal).

---

## 5. Proposed bridge set (summary)

| set | count | relation | confidence | source |
|-----|------:|----------|------------|--------|
| Cross-family near-duplicate leaf pairs | **84** | `Mirrors` | **ironclad** (content identity) | §1 / CSV |
| Attaque-affect lexical-affinity bridges | 2 | `Leverages` / `IsRelatedTo` | moderate (fail-loud) | §4b |
| Causal lexical lead (subset of the 84) | 1 (in 84) | `Mirrors` | ironclad | §4a |
| 2.3 ↔ 6.3 PredatesOn anchor | 7+ (exist) | `PredatesOn` | — (already modelled) | §4c |

**Net proposed : +84 `Mirrors` inter-family edges** (the machine-readable CSV), +2 lexical-affinity bridges
reported for a separate pass. The 84 are the deliverable ; the lexical leads are documented for traceability
(the causal one is absorbed into the 84, the attaque-affect two are deferred).

---

## 6. Method notes

- **Leaf-level, not grappe-level.** My #834 finding operated at the grappe (depth-2) level — necessary to see
  the macro density, but it averaged away the leaf-level structure. The inter-family gap is visible *only* at
  leaf level : the grappe-bubble showed 2.3 ↔ 6.3 as the lone anchor because it aggregated the leaf-level
  `PredatesOn` edges, while the 84 `Mirrors` near-duplicates were invisible (they are inter-family at leaf
  level but do not bubble up as a single grappe-pair). **The leaf-level pass is what surfaces the real ROI.**
- **Near-duplicate detection > lexical affinity for bridge proposals.** Lexical affinity (Jaccard on
  distinctive vocabulary) is a *candidate generator* with documented noise (genre verbs, second-person
  imperatives). Near-duplicate detection (≥ 4 shared distinctive terms + J ≥ 0.45 + manual verification) is
  *content identity* — an undeniable signal. Where the two methods agree (the causal lead), confidence is
  maximal ; where they diverge (attaque-affect), the near-duplicate method's absence is the honest fail-loud.
- **Insuffisance/Tricherie as hubs is a construction property.** The duplication is not random — it reflects
  how the taxonomy was built (a cognitive bias re-catalogued under each family where it manifests). The
  `Mirrors` bridges make this latent structure explicit in the graph. A spectral consumer (#7289 Phase-B) gains
  inter-family edges that the intra-saturated grappes (2.3, 6.3) could not provide — exactly the ROI my #834
  finding predicted.
- **No self-merge, no prod write.** Verdict QA = ai-01. These 84 edges are staged ; serialization to
  `crossLink_Mirrors` is deferred to a gated apply PR after jsboige ratifies.

---

## 7. DoD status

| DoD item (dispatch `msg-9akcg4`) | Status |
|---|---|
| Doc staged `docs/taxonomy/` listing N inter-family bridges with defensible justification | ✅ §1/§3/§5 + `497-inter-family-bridges-annotations.csv` (84 bridges) |
| Fail-loud where no real signal | ✅ §4a (causal noise 0.09–0.11 fail-loud), §4b (attaque-affect moderate, flagged) |
| 0 prod CSV | ✅ PROPOSITION ONLY, 0 write |
| Lexical leads curated (causal + attaque-affect) | ✅ §4a (converges) / §4b (diverges, deferred) |
| Anchor 2.3 ↔ 6.3 documented | ✅ §4c (already populated, different relation, untouched) |

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen. Post-T&A regime.
- ❌ No self-merge. No régén launch. Verdict QA = ai-01.
- ✅ Proposition derived code=truth from the taxonomy CSV (master `3a87eb0e`) ; relation vocabulary verified
  native on master (8 attested types, `Mirrors` 304 uses) ; 0 fabricated relation, 0 coined AIF token
  (0 fabrication #677) ; sample semantic verification confirms every checked pair is a genuine duplicate.
- ✅ Machine-readable annotation CSV `497-inter-family-bridges-annotations.csv` (85 rows : header + 84 bridges)
  provided for the gated serialization.

Relates : #497 (chantier), #834 (substrate refresh — MERGED, this is the leaf-level drill of its finding),
#833 (spectral consumption contract — MERGED, these `Mirrors` edges densify the typed graph it contracts),
#837 (#498 cluster — MERGED), #7289 Phase-B (downstream consumer), #677 (0 fabrication),
`[[aif-no-inherit-attacktype-from-anchor]]` (discipline — derive, don't impose). Base master `3a87eb0e`.
