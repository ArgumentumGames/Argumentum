# #847 AIF structural audit — Layer (c) label-distortion flag (read-only, 0 run)

> **Provenance.** Layer-(c) **second installment**, dispatched by ai-01
> (`msg-20260723T024907-2l19wg`, tick 89, PRIMARY). **Read-only — 0 run live, 0
> prod-CSV.** Companion to [#853](847-layerc-reference-reachability.md) (Layer-(c)
> reference-reachability, the §2.2 half). This completes the §2.1 half: the
> **internal-node labelling constraint**. Method doc
> [`aif-structural-audit-method.md`](aif-structural-audit-method.md) §2.1, §4, §5.
> **MEASURE, not verdict. INPUT for ai-01 synthesis. GATED jsboige ratification.**
> Scope: the **39 internal nodes** in [`847-acompte.csv`](847-acompte.csv) (the
> (a)+(b) layers were measured on the 145 fully-modeled subset; the
> `combined_reading` join only resolves there).

---

## TL;DR

`label_fit` is computed for the **39 grouping nodes** of the acompte. The §2.1
labelling constraint has **two bullets**; only one is mechanically computable:

| §2.1 bullet | What | Mechanically computable? | This instrument |
|-------------|------|--------------------------|-----------------|
| **(2) lexical availability** | a term is **borrowed** from another tradition where no FR term exists | **Yes** (lexical facts) | ✅ `borrowed_from_lang` |
| **(1) distorted scope** | label **stretched / narrowed** vs the cluster | **No** (semantic) | ⛔ proxies tested, non-discriminatory (§3) |

**Headline:** of 39 grouping nodes, **1 is `borrowed`** (`Ad hominem`, raw Latin —
confirmed against the taxonomy's own `Latin` column); **38 are `unflagged`** (no
mechanical distortion signal). `unflagged` means *no mechanical flag raised*, **not**
a verified scope match.

**Axis-C lexical pressure on the grouping layer is LOW** (1/39 raw-Latin): the
taxonomy is authored in FR descriptive phrases (or FR calques), importing raw Latin
**only** where no FR single-term equivalent exists — *Ad hominem* (the FR calque
*Attaque personnelle* sits at d3, but the d2 grouping keeps the Latin). This is a
real structural finding about the tree.

## The one priority candidate (distorted × heterogeneous)

| Node | Level | label_fit | acompte tag | Combined reading |
|------|-------|-----------|-------------|------------------|
| **Ad hominem** | sub-family (d2) | **borrowed (latin)** | **A-candidate** | **PRIORITY arbitration candidate** → **tradition-divergence → bridge-node, do NOT bill as grouping defect** |

This **resolves the `c-DEFER` on Ad hominem** from the acompte. Combined with #853
(Ad hominem's Obstruction base is **69% Wikipedia-anchored**, well-evidenced), the
layer-(c) reading is consistent: *Ad hominem*'s mechanism-heterogeneity is the
**tradition's** (a Latin scholastic term spanning cultures), **not** a jsboige
grouping error. The bridge holds.

There are **no `borrowed × B+A`** nodes (Ad hominem is the only borrowed node), so
no second-tier "label may have forced a bad merge under material-resistance" cases
arise from this mechanical pass.

## A-candidate resolution (the 7 tree-tension nodes)

| Node | Level | label_fit | Layer-(c) labelling read |
|------|-------|-----------|--------------------------|
| **Ad hominem** | d2 | **borrowed** | tradition-divergence → **bridge-node** |
| Erreur de raisonnement | d1 | unflagged | no label signal → genuine axis-A mix **or** human semantic read |
| Argument bâclé | d2 | unflagged | " |
| Causalité douteuse | d2 | unflagged | " |
| Mauvaise interprétation | d2 | unflagged | " |
| Pensée biaisée | d2 | unflagged | " |
| Biais naturels | d3 | unflagged | " |

**Reading for ai-01/jsboige:** of the 7 tree-tension (A-candidate) nodes, **only
Ad hominem carries a mechanical axis-C label signal** (borrowed Latin). The other 6
have **no mechanical labelling distortion** → their heterogeneity is either a
genuine axis-A grouping choice **or** a stretched/narrowed label that this
first-pass cannot detect mechanically (semantic; §3). Layer (c) therefore does
**not** bill any of the 6 as tradition artifacts — it leaves them as axis-A
candidates for jsboige, exactly per the method doc's "check axis C first; if low,
the grouping is genuinely his to arbitrate."

## Why stretched/narrowed are NOT mechanically derived (§3 — saves future work)

Two obvious mechanical proxies for §2.1 bullet (1) (distorted scope) were tested
and found **non-discriminatory**. They are reported in the CSV as contextual
evidence (`dispersion_fr`, `n_en_groupings`) but **not** used to derive `label_fit`.

1. **Concept-dispersion (within-language scope sprawl).** `dispersion_fr` =
   distinct FR Wikipedia slugs among a node's FR-referencing leaves / FR-referencing
   leaves. **Saturated**: median **0.94 (d1) / 1.00 (d2) / 1.00 (d3)** — i.e. at
   sub-family and below, essentially **every leaf is its own Wikipedia article**, so
   distinct-slugs ≈ leaves everywhere. It cannot distinguish a tight cluster from a
   sprawled one. *(Note: `dispersion_fr = 0.0` on a few small d3 nodes means their
   leaves carry **no FR Wikipedia reference** (dict-only / empty), not "perfectly
   clustered" — a coverage gap, flagged, not a distortion signal.)*

2. **EN-grouping divergence (cross-tradition structural signal).** For each FR
   grouping node, count distinct EN grouping labels (`Family`/`Subfamily`/`Subsubfamily`)
   among its leaves. **Result: 1:1 everywhere** (97% EN coverage; every FR node maps
   to exactly one EN node). The EN grouping columns are a **FR translation/calque,
   not an independent EN-tradition tree**. So no axis-C structural divergence is
   encoded at the grouping layer — the cross-linguistic divergence the method doc
   describes (§2 axis C) lives at the **leaf-reference** level (different Wikipedia
   articles per language, already measured in #853), not in the grouping labels.

**Consequence:** a defensible `stretched`/`narrowed` flag requires **human semantic
read** (compare each grouping label's consecrated scope to its members) — it is not
mechanizable from the taxonomy + #853 data without an LLM-assisted pass (heavier,
gated). This first-pass delivers the **mechanically-grounded** part (`borrowed`) and
documents the dead-end on the rest, so no future worker re-tries these proxies.

## Governance

- **MEASURE, not verdict.** 0 reorganisation wording. Synthesis + verdict = ai-01 /
  jsboige. Same governance as #850 / #853.
- **0 prod-CSV write** (T&A freeze). Docs-only artefact. DatasetUpdater untouched,
  `Enabled=false`.
- **`borrowed_from_lang` is grounded in lexical fact**, not semantic claim: a node
  is `borrowed` only if its label matches the taxonomy's own `Latin` column or Latin
  morphology (conservative). `unflagged` is explicitly *not* a positive "exact"
  verdict — it is the absence of a mechanical flag (mirrors #853's "dead = upper
  bound" honesty; a no-match is not a content claim).
- Calques (e.g. *Pétition de principe* ← *petitio principii*) are a **gray zone not
  flagged** by this conservative pass: the term was translated into FR (FR words),
  so by §2.1 bullet (2) it is not a raw borrow. Flagged for human read if a calque's
  scope is later found distorted.

## Caveats

1. **Coverage of `borrowed` is deliberately narrow** (raw Latin/anglicism only).
   A broader "compromise-label" notion (vernacular terms pressed into taxonomy
   service, e.g. *Tricherie*, *Humour*) is real but not mechanically detectable
   without a French frequency lexicon — out of scope, flagged.
2. **`unflagged` ≠ "label fits".** 38/39 nodes are unflagged; this instrument makes
   no positive scope-match claim for any of them. It only surfaces the 1 node where
   a mechanical distortion signal exists.
3. The join resolves **only** the 39 acompte nodes. Full-tree extension (all ~92
   grouping nodes) is the same heuristic but yields no `combined_reading` without
   the acompte (a)/(b) layers — out of scope here.

## Artefacts

- [`847-layerc-label-distortion.csv`](847-layerc-label-distortion.csv) — per
  internal node (39 rows): `level,node,n_leaves_full,n_modelled,label_fit,borrowed_lang,
  dispersion_fr,n_en_groupings,acompte_tag,combined_reading`.
- Compute script (scratchpad): `compute-847-layerc-label-distortion.py`.

## Refs

- Method: [`aif-structural-audit-method.md`](aif-structural-audit-method.md) §2.1,
  §4, §5. Companion Layer-(c) half: [`847-layerc-reference-reachability.md`](847-layerc-reference-reachability.md)
  (#853). Acompte (layers a+b): [`847-acompte-homogeneity.md`](847-acompte-homogeneity.md)
  (#850). Tracking: #847. Chantier: #498. Dispatch ai-01 `msg-20260723T024907-2l19wg`
  (tick 89). Base master `82a1e027`.
