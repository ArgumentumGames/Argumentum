# 2026-07-22 — #498 AIF chantier : cluster « Attaque personnelle » (Obstruction > Ad hominem) — **second undermine-dominant, the most uniform cluster**

**Scope** : dedicated cluster doc for the **Attaque personnelle** sub-sub (family **Obstruction**, sub-family
**Ad hominem**, sub-sub **Attaque personnelle**) — **7 unmapped leaves** (1399-1405) under the anchor **1398**
(serialized `undermine`/`I-node` **with native skos**). **Proposition only — GATED, 0 write to prod CSV.**
Continues the cluster-docs backlog (ai-01 dispatch `msg-9akcg4` strate-6 deep-queue idle lane + ai-01 ACK
`msg-6uufhx` naming this exact candidate, post-T&A regime). Sixth cluster of the dispatch
(#837 + #839 + #840 + #841 + #843 + this).

**Repo reference** : master `eebfea73`. Issue : #498. Predecessors : #770 (anchor audit), #837/#839 (mixed),
#840 (cigogne), #841 (accident), #843 (inconsistance — first undermine-dominant).

> **⚠ STATUS = PROPOSITION ONLY.** No prod CSV write, no DB write, no OWL regen. jsboige ratifies → worker applies
> the CSV edits in a follow-up PR (gated), same flow as #837/#839/#840/#841/#843. Serialization deferred to gated apply.

---

## TL;DR

- **Attaque personnelle** (Obstruction > Ad hominem, sub-sub, 8 rows) : anchor **1398** serialized
  `undermine`/`I-node` **with native skos** (`skosDirectRef=DirectAdHominem_Inference`, `skos:broadMatch`).
  **7 unmapped leaves** — 1399 Argument tonal, 1400 Dérision, 1401 Raillerie, 1402 Diffamation, 1403 Injure,
  1404 Insulte, 1405 Lancer de soulier.
- **Second undermine-dominant cluster of the chantier** (after #843 Inconsistance), and **the most uniform**:
  all 7 leaves share the same attack-type (`undermine`/I-node), the same scheme
  (`DirectAdHominem_Inference`), and the same mechanism (discredit the *source* rather than refute the *position*).
  The sub-sub is a pure **rhetorical-vector family** — the *channel* of attack varies (tone / mockery /
  reputation / abuse / gesture), the AIF mechanism does not.
- **7/7 native-fit (100 %)** — the highest native-fit rate in the chantier, tied with accident #841. The
  `DirectAdHominem_Inference` token (natively attested, the anchor's own scheme) captures *exactly* the
  "attack the person, not the position" mechanism. No fail-loud needed. No fabrication (#677) — legitimate
  scheme inheritance (leaves share the anchor's mechanism, distinct from the attack-type-inheritance warning).
- **One boundary leaf (1405)** : the only *non-verbal* leaf (a contempt gesture — shoe-throwing). Modelled
  undermine/I-node honestly (the gesture discredits the source), flagged for ratification.
- **7 attack-typed leaves proposed, 0 fabrication, axiom 0 violation** (7 undermine→I). Projected coverage
  **145 → 152 / 1 408** once applied (live rescan master `eebfea73` = **145/1 408** — propositions are docs-only,
  not yet written to prod CSV).

---

## 1. Cluster state (code=truth, master `eebfea73`)

From `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, family **Obstruction**, sub-family **Ad hominem**,
sub-sub **Attaque personnelle** (8 rows) :

| pk | text_fr | depth | AIF_attackType | AIF_attackedNode | skos cols | State |
|----|---------|-------|----------------|------------------|-----------|-------|
| **1398** | **Attaque personnelle** (anchor) | d3 | `undermine` | `I-node` | direct=`DirectAdHominem_Inference` (`skos:broadMatch`) | **✅ serialized (skos)** |
| 1399 | Argument tonal | d4 | — | — | — | unmapped (**this cluster**) |
| 1400 | Dérision | d4 | — | — | — | unmapped (**this cluster**) |
| 1401 | Raillerie | d4 | — | — | — | unmapped (**this cluster**) |
| 1402 | Diffamation | d4 | — | — | — | unmapped (**this cluster**) |
| 1403 | Injure | d4 | — | — | — | unmapped (**this cluster**) |
| 1404 | Insulte | d5 | — | — | — | unmapped (**this cluster**) |
| 1405 | Lancer de soulier | d5 | — | — | — | unmapped (**this cluster**) |

**Leaf content (code=truth `desc_fr` / `example_fr`)** :
- **1398 Attaque personnelle** — « Au lieu de réfuter les arguments, vous attaquez directement la personne qui les
  présente. » / « Ne me dites pas comment hacher l'ail. Un toxicomane ne va pas m'apprendre à me nourrir. »
- **1399 Argument tonal** — « Vous refusez un argument en critiquant la manière dont il est présenté plutôt que son
  contenu. » / « Tant que vous continuez avec cette attitude condescendante, je refuse de discuter avec vous. »
- **1400 Dérision** — « Vous ridiculisez votre interlocuteur au lieu de répondre à son argumentation. » /
  « Oh, donc maintenant vous êtes l'expert, c'est ça ? Expliquez-moi encore comment cela fonctionne ! »
- **1401 Raillerie** — « Vous raillez votre interlocuteur avec des moqueries au lieu de discuter son idée. » /
  « Au moins, nous saurons qui blâmer quand il n'y aura plus de café à cause du réchauffement climatique. »
- **1402 Diffamation** — « Vous cherchez à discréditer votre adversaire en lui imputant publiquement, et à tort, un
  fait susceptible de porter atteinte à sa réputation » / « Il est connu que cette société verse des pots-de-vin… »
- **1403 Injure** — « Sans même examiner sa proposition, vous adressez à votre adversaire des paroles grossières
  visant à le blesser personnellement. » / « Seul un idiot peut se préoccuper de telles futilités. »
- **1404 Insulte** — « Sans même examiner sa proposition, vous adressez à votre adversaire des gestes ou des paroles
  visant à l'offenser. » / « Seule une personne aussi ignorante que vous pourrait penser que c'est une bonne idée. »
- **1405 Lancer de soulier** — « Vous manifestez un profond manque de respect par un geste offensant, ici le lancer
  de soulier. » / « Au milieu de notre débat, il a subitement retiré sa chaussure et l'a jetée sur la table… »

The sub-sub is **semantically coherent around one mechanism** — *attack the person, not the position* — which is why
the cluster is uniform undermine/I-node. The 7 leaves are all *rhetorical channels* of the same direct ad-hominem
attack; they differ by the **vector** (tone / mockery / reputation / abuse / gesture), not by the AIF mechanism.

---

## 2. The anchor model (1398, serialized) — direct ad-hominem

`1398` is serialized `AIF_attackType=undermine`, `AIF_attackedNode=I-node`, **`AIF_skosDirectRef=
DirectAdHominem_Inference`**, `AIF_skosMappingType=skos:broadMatch`, ExceptionRef empty. The fallacy attacks a
**premise-source** (I-node) — the speaker — by discrediting the person rather than refuting the position. This is
the canonical AIF/ASPIC+ shape of an ad hominem: the attacked node is the *source* (an I-node carrying the
speaker's credibility), defeated by an attack on the person (undermine). The two other Ad-hominem sub-subs
(Procès en inconsistance, Sophisme génétique) use the circumstantial / bias / genetic variants — this sub-sub is
the *direct/abusive* variant.

**Native ad-hominem / source-credibility token inventory (code=truth)** : `DirectAdHominem_Inference` (×1),
`CircumstantialAdHominem_Inference` (×2), `PositionToKnow_Inference` (×2), `Bias_Inference` (×2),
`ExpertOpinion_Inference` (×1), `ExpertiseInconsistency_Conflict` (×1) — **all natively attested**. The family is
native-rich; the cluster's leaves inherit `DirectAdHominem_Inference` (the direct/abusive scheme).

---

## 3. Proposed AIF structure for the leaves

Per-leaf derivation. `attackType` + `attackedNode` follow the AIF/ASPIC+ axiom (**0 violation**, all undermine→I).
`skosDirectRef` carries the native `DirectAdHominem_Inference` (7/7 native-fit). **0 fabrication (#677)** —
legitimate scheme inheritance (leaves share the anchor's mechanism, distinct from the attack-type-inheritance
warning `[[aif-no-inherit-attacktype-from-anchor]]`).

### All leaves → **undermine / I-node** + `DirectAdHominem_Inference` (matches anchor)

The 7 leaves are all **direct/abusive** ad-hominem attacks (attack the person's character / tone / reputation, not
the circumstantial bias). They inherit the anchor's `DirectAdHominem_Inference` scheme — the Walton family provides
no more specific native token for "injure / dérision / diffamation" individually, and `DirectAdHominem_Inference`
captures *exactly* their shared mechanism. The variety is in the **attack vector** (the rhetorical channel), which
the scheme accommodates without forking.

| pk | text_fr | attack vector | Proposal |
|----|---------|---------------|----------|
| 1399 | Argument tonal | tonal (manner of presentation) | `undermine`/I, `DirectAdHominem_Inference`, narrowMatch |
| 1400 | Dérision | mockery (ridicule) | `undermine`/I, `DirectAdHominem_Inference`, narrowMatch |
| 1401 | Raillerie | mockery (taunting) | `undermine`/I, `DirectAdHominem_Inference`, narrowMatch |
| 1402 | Diffamation | reputation (poisoning the well) | `undermine`/I, `DirectAdHominem_Inference`, narrowMatch |
| 1403 | Injure | abuse (hurtful language) | `undermine`/I, `DirectAdHominem_Inference`, narrowMatch |
| 1404 | Insulte | abuse (offensive language/gesture) | `undermine`/I, `DirectAdHominem_Inference`, narrowMatch |
| 1405 | Lancer de soulier | **contempt gesture (non-verbal)** ⚠ boundary | `undermine`/I, `DirectAdHominem_Inference`, narrowMatch |

### Cluster summary (proposed)

| pk | text_fr | attackType | attackedNode | DirectRef | ExceptionRef | MappingType | attack vector |
|----|---------|-----------|--------------|-----------|--------------|-------------|---------------|
| 1398 | Attaque personnelle (anchor) | undermine | I-node | `DirectAdHominem_Inference` (native) | *(empty)* | broadMatch | (umbrella) |
| 1399 | Argument tonal | undermine | I-node | `DirectAdHominem_Inference` (native) | *(empty)* | narrowMatch | tonal |
| 1400 | Dérision | undermine | I-node | `DirectAdHominem_Inference` (native) | *(empty)* | narrowMatch | mockery |
| 1401 | Raillerie | undermine | I-node | `DirectAdHominem_Inference` (native) | *(empty)* | narrowMatch | mockery |
| 1402 | Diffamation | undermine | I-node | `DirectAdHominem_Inference` (native) | *(empty)* | narrowMatch | reputation |
| 1403 | Injure | undermine | I-node | `DirectAdHominem_Inference` (native) | *(empty)* | narrowMatch | abuse |
| 1404 | Insulte | undermine | I-node | `DirectAdHominem_Inference` (native) | *(empty)* | narrowMatch | abuse |
| 1405 | Lancer de soulier | undermine | I-node | `DirectAdHominem_Inference` (native) | *(empty)* | narrowMatch | gesture (boundary) |

**7 attack-typed leaves proposed, 0 fabrication.** Axiom **0 violation** (7 undermine→I). **Uniform undermine/I-node**
+ **uniform scheme** (`DirectAdHominem_Inference`) — the most uniform cluster in the chantier. **7/7 native-fit (100 %)**.
Variety is in the **attack vector** (the rhetorical channel), not the attack-type or scheme.

---

## 4. Method notes (additions for the chantier)

- **Second undermine-dominant cluster — the rhetorical-vector pole.** #843 (Inconsistance) was the first
  undermine-dominant, with variety in the **contradiction shape** (pair / self-reference / cumulative / n-ary /
  premise↔conclusion). This cluster is the second undermine-dominant, with variety in the **attack vector** (the
  rhetorical channel of the ad-hominem attack). Together they bracket the mono-scheme undermine space: the scheme
  (`InconsistentCommitment_Inference` / `DirectAdHominem_Inference`) stays constant, and the leaves specialize the
  *how* of the premise-attack. The chantier now documents the two undermine-poles (contradiction-shape variety +
  attack-vector variety) alongside the two undercut-poles (scheme-diverse accident #841 + native-rich cigogne #840)
  and the rebut pole (#760) — the AIF spectrum, fully covered.
- **The most uniform cluster — 100 % native-fit, single scheme, single mechanism.** Unlike accident #841 (4
  schemes) or cigogne #840 (mixed native-fit / fail-loud), this cluster has a single scheme inherited by all 7
  leaves with zero fail-loud. This is because the sub-sub is a pure *rhetorical-channel* family: the fallacies are
  *forms* of the same direct ad-hominem attack, differing only in the channel. The AIF scheme vocabulary is rich
  enough to cover them all with one token (`DirectAdHominem_Inference`), so no forking and no fabrication.
- **1402 Diffamation = poisoning the well.** The defamation example (« Il est connu que cette société verse des
  pots-de-vin… ») is the textbook *poisoning the well* (a pre-emptive reputation attack to discredit the source
  before they speak). This is a canonical form of the direct ad-hominem — `DirectAdHominem_Inference` fits, and it
  bridges to the separate "Empoisonner le puits" cluster (queue candidate) which is the *generic* pre-emptive
  version.
- **1405 Lancer de soulier boundary — the only non-verbal leaf.** Shoe-throwing is a *contempt gesture*, not a
  verbal argument. It is the only leaf in the cluster whose attack vector is non-verbal (physical gesture of
  disdain). Modelled undermine/I-node honestly: the gesture *does* discredit the source (it signals contempt, which
  is the ad-hominem move), and the sub-sub 1398 includes it. Flagged for ratification — ai-01/jsboige may prefer to
  read it as outside the AIF argument-space (a pure physical act), but the canonical reading (contempt-as-discredit)
  keeps it undermine/I-node.
- **Legitimate skos inheritance, again.** As in cigogne (#840), accident (#841), and inconsistance (#843), the
  native scheme is inherited because the leaves genuinely share the anchor's mechanism — distinct from the
  attack-type-inheritance warning `[[aif-no-inherit-attacktype-from-anchor]]` (which concerns *attack-type*
  inheritance; skos inheritance is legitimate when the leaf shares the anchor's *scheme*). The cluster is a clean
  illustration: the anchor's *attack-type* (undermine) and *scheme* (`DirectAdHominem_Inference`) both transfer,
  because every leaf is a direct ad-hominem.

---

## 5. DoD status

| DoD item (#498) | Status |
|-----------------|--------|
| Effective cluster-level coverage documented | ✅ §1/§3 (7 leaves attack-typed ; sub-sub Attaque personnelle fully opened, 8/8 rows addressed incl. anchor) |
| Per unmapped leaf : legitimate decomposition | ✅ §3 (attack-type + attacked-node + native scheme, 0 fabrication) |
| `AIF_skosMappingType` coherent | ✅ §3 (narrowMatch throughout — all leaves specialize the anchor's direct-ad-hominem mechanism) |
| Fail-loud when no native token fits | ✅ 7/7 native-fit (no fail-loud needed — `DirectAdHominem_Inference` covers the direct/abusive family) ; boundary on 1405 documented |
| OWL regen reflects new structures | ⏳ Deferred — jsboige ratifies → CSV edits (gated) → OWL regen (#763 wiring ready) |

### Coverage accounting (code=truth)

- **Master coverage (rescan, `eebfea73`)** : **145/1 408 mapped** (`attackType` non-empty). **Axiom 0 violation.**
  Undermine population rises to 52 (45 + 7) once applied — the second batch of undermine/I-node modelling after
  #843 (Inconsistance). *(Note : propositions #841/#843 are docs-only, not yet in prod CSV — the live count is 145,
  the strict "fully-modeled" figure. The +6/+6/+7 are *projected* on ratification + apply.)*
- **This cluster (proposed)** : **+7 attack-typed** (7 undermine/I) → **152/1 408** projected once applied.
- **Obstruction family footprint** : the cluster opens the Ad-hominem > Attaque personnelle sub-sub (direct/abusive
  variant). The two sibling sub-subs (Procès en inconsistance = circumstantial/tu-quoque variant ; Sophisme
  génétique = genetic/bias variant) remain queue candidates.

**Attaque personnelle sub-sub : fully opened** (anchor 1398 serialized + 7 leaves proposed = 8/8 addressed).

---

## 6. Mapping to jsboige's enriched DoD (I-node / RA-node / CA-node decomposition)

### All leaves → undermine / I-node (source discrediting)

The attacked node is the **source-premise** (I-node) — the speaker whose credibility carries the argument. The
attack discredits the *person* (via tone / mockery / reputation / abuse / gesture), so the source-premise is
undermined rather than the inference rule (undercut) or the conclusion (rebut). No leaf invokes an RA-node (the
argument's rule is not challenged) or a CA-node (no counter-conclusion is offered).

#### pk 1399 — Argument tonal → **undermine** (tonal vector)
- **I-nodes** : the source, attacked via the *tone* of presentation. The premise-source is discredited by tone-policing.
- **RA-node** : *(none — the rule is not challenged)*.
- **CA-node** : none.
- **Attack type** : **undermine** (the source-premise is attacked via its manner of presentation).

#### pk 1400 — Dérision → **undermine** (mockery vector)
- **I-nodes** : the source, ridiculed. The premise-source is discredited by ridicule.
- **RA-node / CA-node** : none.
- **Attack type** : **undermine**.

#### pk 1401 — Raillerie → **undermine** (mockery vector)
- **I-nodes** : the source, taunted. The premise-source is discredited by mockery.
- **RA-node / CA-node** : none.
- **Attack type** : **undermine**.

#### pk 1402 — Diffamation → **undermine** (reputation vector)
- **I-nodes** : the source, attacked via a false imputation to their reputation (poisoning the well). The
  premise-source is discredited by reputation damage.
- **RA-node / CA-node** : none.
- **Attack type** : **undermine**.

#### pk 1403 — Injure → **undermine** (abuse vector)
- **I-nodes** : the source, addressed hurtful language. The premise-source is discredited by personal abuse.
- **RA-node / CA-node** : none.
- **Attack type** : **undermine**.

#### pk 1404 — Insulte → **undermine** (abuse vector)
- **I-nodes** : the source, addressed offensive language/gesture. The premise-source is discredited by offense.
- **RA-node / CA-node** : none.
- **Attack type** : **undermine**.

#### pk 1405 — Lancer de soulier → **undermine** (contempt-gesture vector, boundary)
- **I-nodes** : the source, shown contempt by an offensive gesture. The premise-source is discredited by disdain.
- **RA-node / CA-node** : *(none — the gesture is non-verbal, but the canonical reading is contempt-as-discredit)*.
- **Attack type** : **undermine** (boundary — non-verbal gesture).

### Cluster attack-type distribution
| pk | attack vector | I-node (attacked source) | RA-node | Attack type |
|----|---------------|--------------------------|---------|-------------|
| 1398 | (umbrella) | the speaker | *(none)* | undermine (serialized) |
| 1399 | tonal | speaker (tone-policing) | *(none)* | undermine |
| 1400 | mockery | speaker (ridicule) | *(none)* | undermine |
| 1401 | mockery | speaker (taunt) | *(none)* | undermine |
| 1402 | reputation | speaker (poisoning the well) | *(none)* | undermine |
| 1403 | abuse | speaker (hurtful language) | *(none)* | undermine |
| 1404 | abuse | speaker (offense) | *(none)* | undermine |
| 1405 | gesture (boundary) | speaker (contempt) | *(none)* | undermine |

**7/7 undermine / I-node** — uniform attack-type, **one native scheme** (`DirectAdHominem_Inference`), variety in
the attack vector (the rhetorical channel). Serialization = `AIF_attackType` + `AIF_attackedNode` per the table ;
native skos on all 7.

---

## Gate boundaries (HARD — proposition only)

- ❌ No prod CSV write, no DB write, no OWL regen, no `aif:` assertion emitted. Post-T&A regime.
- ❌ No self-merge. No régén launch. Verdict QA = ai-01.
- ✅ Proposition derived code=truth from the taxonomy CSV (master `eebfea73`) + AIF vocabulary verified native on
  master (`DirectAdHominem_Inference` attested on the anchor 1398) ; 0 fabricated token (0 fabrication #677).
  Native ad-hominem scheme inherited legitimately (leaves share the anchor's direct/abusive mechanism).
- ✅ Sixth cluster documented (second undermine-dominant — the rhetorical-vector pole of the undermine space) ;
  boundary leaf 1405 flagged (non-verbal contempt gesture) ; machine-readable annotation CSV
  `498-aif-attaque-personnelle-annotations.csv` provided for the gated serialization.

Relates : #498 (chantier), #837 (mixed 1 — Complication exagérée), #839 (mixed 2 — Opération inappropriée), #840
(native-rich cigogne, undercut), #841 (native-rich accident, undercut), #843 (first undermine-dominant — Inconsistance),
#760 (rebut cluster — Relativisme abusif), #770 (anchor audit), #763 (OWL AIF wiring), #677 (0 fabrication),
`[[aif-no-inherit-attacktype-from-anchor]]` (discipline — this cluster's skos inheritance is legitimate, leaves share
the anchor's scheme). Base master `eebfea73`.
