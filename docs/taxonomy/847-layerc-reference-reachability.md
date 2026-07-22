# AIF structural audit — Layer (c) reference-reachability acompte (#847)

> **Provenance.** Layer-(c) instrumentation of the audit method
> (`aif-structural-audit-method.md` §2.2, §4, §6). Dispatched by ai-01
> (`msg-gvpl31`, tick 87) as the *input the #847 acompte (#850) deferred* — the
> signal that turns each `A-candidate` / `c-DEFER` tag from the layers-(a)+(b)
> measure into a **bridge-vs-defect** reading. Computed by **po-2024** (worker,
> Cards/AIF lane). **MEASURE, not verdict. INPUT for ai-01 synthesis, GATED
> jsboige ratification. 0 prod-CSV write.** Companion to the layers-(a)+(b)
> acompte (`847-acompte-homogeneity.md`).

---

## 1. What layer (c) measures (method doc §2.2, §4)

> *"Wikipedia links are stable; fallacy-dictionary links are decaying (several
> already dead, repointed to archive.org). The Wikipedia-anchored nodes are the
> trustworthy skeleton; dictionary-only nodes are at evidential risk. The audit
> should therefore carry **reference reachability** as per-node metadata — a
> fail-loud leaf whose only anchor is a dead dictionary link is a different
> diagnosis than one with a live Wikipedia anchor."*

Layer (c) is the **cross-linguistic / evidential** axis. Where layer (a)
(attack-type heterogeneity) and layer (b) (fail-loud scheme rate) read the
**mechanism**, layer (c) reads the **evidence base**: is a node's membership
anchored on stable primary sources (Wikipedia / WMF), or does it lean on
decaying fallacy-dictionary and miscellaneous long-tail web pages? This is the
axis-C lens that lets the audit **distinguish a genuine grouping defect
(axis A) from a tradition-divergence artifact (axis C)** — the method's
central fairness principle ("the audit only bills jsboige for what is genuinely
his").

This document instruments **reference-reachability only**. The sibling
**label-distortion flag** (method doc §2.1) is a separate, lighter first-pass
noted in §5; it is mechanical-flag-only and never applied.

## 2. Instrument (operational, code=truth)

Source: `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`, the 8 `link_<lang>`
columns (fr/en/ru/pt/ar/es/zh/fa) — the external-reference anchor of each leaf.

Each unique URL is classified into one bucket:

| Bucket | How classified | Reading |
|--------|----------------|---------|
| `wikipedia-stable` | deterministic allowlist (`*.wikipedia.org`, wikisource, wiktionary, wikiquote — WMF) | trustworthy skeleton |
| `archive-repointed` | deterministic (`web.archive.org` / `archive.is` / `archive.today`) | already rescued — alive by construction |
| `dictionary-known` | deterministic allowlist of named fallacy dictionaries (logicallyfallacious, skepdic, fallacyfiles, don-lindsay-archive, ditext, …) | named evidential class — decaying, **not** auto-dead |
| `alive` | read-only **HEAD probe** (curl, polite UA, 8s timeout), 2xx/3xx final | live long-tail |
| `dead` | HEAD probe, 4xx/5xx final | dead link — direct §2.2 decay evidence |
| `unknown` | HEAD probe timeout / connection error / non-numeric | **conservative: never guessed** |
| `unparseable` | URL not parseable | malformed |

**Conservative by design:** deterministic allowlist covers ~93% of refs
(WMF+archive+dictionary); the **live HEAD probe runs only on the 186 long-tail
refs** (the evidential-risk class). Anything the probe cannot firmly classify
→ `unknown`, never inferred alive or dead.

## 3. Headline (all 2,579 reference cells)

| Bucket | n | % |
|--------|---|---|
| `wikipedia-stable` | 2,127 | **82.5%** |
| `dictionary-known` | 225 | 8.7% |
| `dead` (long-tail HEAD) | 134 | **5.2%** |
| `alive` (long-tail HEAD) | 51 | 2.0% |
| `archive-repointed` | 39 | 1.5% |
| `unknown` | 1 | 0.0% |
| `unparseable` | 2 | 0.1% |

**Reading.** The taxonomy is **overwhelmingly Wikipedia-anchored** (82.5%).
The method doc's §2.2 worry ("dictionary links decaying, several already dead")
is **confirmed and quantified**: 134 dead long-tail refs (5.2%) + a named
dictionary class (8.7%) that is decaying in place. The skeleton is trustworthy;
the evidential risk concentrates on the **long-tail + dictionary share**, which
the per-node rollup (§4) localizes.

## 4. Per-node reachability profile

Two artefacts (CSV):

- **`847-layerc-reference-reachability.csv`** — per parent node (family d1 /
  sub-family d2 / sub-sub d3, n≥3 referencing leaves): ref counts + `pct_wikipedia`,
  `pct_dictionary`, `pct_longtail`, Shannon-normalised bucket entropy.
- **`847-layerc-reference-reachability-refs.csv`** — every reference cell (pk,
  lang, host, url, bucket).

### 4.1 Family (d1)

| Family | n_refs | %wiki | %dict | %longtail | H |
|--------|-------|------|------|----------|---|
| Influence | 876 | **89%** | 3% | 5% | 0.26 |
| Tricherie | 803 | **91%** | 4% | 4% | 0.25 |
| Abus de langage | 142 | 72% | 17% | 9% | 0.48 |
| Obstruction | 193 | 69% | 15% | 12% | 0.57 |
| Erreur mathématique | 128 | 69% | 18% | 10% | 0.56 |
| Erreur de raisonnement | 138 | 67% | **25%** | 6% | 0.57 |
| Insuffisance | 291 | 63% | 15% | **17%** | **0.68** |

**Reading.** `Influence` and `Tricherie` are the **trustworthy skeleton**
(89-91% Wikipedia, low entropy). `Insuffisance` carries the **highest evidential
risk** (lowest wiki%, highest long-tail 17%, highest entropy 0.68). `Erreur de
raisonnement` is the only family at the dictionary threshold (25%).

### 4.2 Sub-family (d2) — where risk concentrates

Top dictionary-share sub-families (**MEASURE, not verdict**):

| Sub-family | %dict | %longtail | %wiki |
|------------|------|----------|------|
| Comparaison fallacieuse | **43.8%** | 0% | 50% |
| Mauvaise composition | 32.4% | 2.7% | 65% |
| Mauvaise déduction | 28.3% | 9.4% | 58% |
| Mauvaise interprétation | 26.7% | 2.2% | 71% |
| Argument bâclé | 24.4% | 22.2% | 46% |
| Changement de cap | 21.0% | **30.6%** | 45% |

### 4.3 Sub-sub (d3) — highest evidential-risk leaves

| Sub-sub | %dict | %longtail | %wiki |
|---------|------|----------|------|
| Comparaison abusive | **80%** | 0% | 20% |
| Définition inconsistance | 33% | **50%** | 17% |
| Argument d'autorité | 22% | **47%** | 31% |
| Justification triviale | 32% | 32% | 32% |
| Sophisme d'Explication | 40% | 20% | 10% |

**Reading.** The risk **concentrates at the leaves** (expected: the deepest
nodes cite the most specialized, least-stable sources). `Comparaison abusive`
(80% dictionary) and `Définition inconsistance` (50% long-tail, 17% wiki) are
the sharpest evidential-risk shapes.

## 5. Cross-layer reading (the point of layer c)

Layer (c) is read **against** layers (a)+(b) of the #850 acompte, never alone.
The method doc §4 decision rule wants `A-candidate (a-high/b-low)` nodes to be
checked for **cross-linguistic divergence / evidential risk** before being
billed as a grouping defect:

| Layer-(a)+(b) acompte node | Layer-(c) reading here | Synthesis input |
|----------------------------|------------------------|-----------------|
| **Mauvaise déduction** (a-high/b-low, b=0.20) | dictionary-heavy (28% dict) | genuine tree-tension candidate AND evidentially exposed → **real arbitration**, not tradition artifact |
| **Erreur de raisonnement** family (a-high) | only family at 25% dictionary | tension sits on a dictionary-leaning base |
| **Ad hominem** (a-high/b-low, c-DEFER in #850) | Obstruction family (69% wiki, 15% dict) — reasonably anchored | stays **bridge-node** reading — heterogeneity not explained by evidential decay |
| **Influence** (only fully coherent family) | 89% wiki skeleton | coherent **and** best-anchored — high confidence |

**This is the layer-(c) value:** it converts the acompte's `c-DEFER` tags from
uncheckable placeholders into a per-node evidential profile. A tree-tension
node sitting on a Wikipedia skeleton is a different arbitration than one sitting
on dead long-tail links. The synthesis (ai-01) can now read both.

## 6. Caveats (honest)

1. **HEAD-probe noise.** Some hosts return 4xx/5xx to HEAD but 200 to GET
   (e.g. `rationalwiki.org` returned 503 on HEAD — soft-blocked bots). These
   register as `dead`/`unknown`, slightly **over-counting** dead links. The
   5.2% dead figure is therefore an **upper bound** on true dead-link share.
   Re-running with GET (heavier, not read-only-HEAD) would refine it; deferred.
2. **Allowlist is opinionated.** The `dictionary-known` set is a named
   fallacy-dictionary allowlist; a host not on it falls to `longtail` and a live
   probe. Misclassification risk = a known dictionary lumped as long-tail
   (cosmetic, same evidential class) or vice-versa.
3. **No GET body validation.** A 200 from a domain-squatted / repurposed page
   still reads `alive`. Full evidential audit (does the page still say what it
   cited?) is human work, out of scope for a mechanical measure.
4. **Layer-(c) label-distortion flag (method §2.1) not computed here.** That is
   a separate first-pass (internal node = consecrated term? carries its own
   external ref? stretched/borrowed?). Mechanical-flag-only, **never applied**,
   deferred to a follow-on or human read.

## 7. Governance

- **MEASURE, not verdict.** 0 reorganisation wording. Synthesis + verdict = ai-01.
- **0 prod-CSV write** (post-T&A freeze). Artefacts are docs-only
  (`docs/taxonomy/847-layerc-*`).
- Conservative classification (unknown never guessed; dead is an upper bound).
- Read-only HEAD probes only (no write, no mutation).

## Refs

- Method: `aif-structural-audit-method.md` §2.2, §4, §6. Companion acompte:
  `847-acompte-homogeneity.md` (layers a+b, #850). Tracking: #847. Chantier: #498.
- Dispatch: ai-01 → po-2024 `msg-gvpl31` (tick 87).
