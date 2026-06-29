# #192 — Native Ratification Checklist (post-#607 deferred residue)

**Author**: po-2024 (worker) · **Date**: 2026-06-28 · **Base**: master `44c37fa2` (post-#607)
**Status**: **TURNKEY CHECKLIST** — ratifiable in ~10 min by a native speaker / jsboige, **post-release**.
**Scope**: docs only. **0 write under `Cards/`**. master stays `44c37fa2`.
**Companion**: [`192-terminology-glossary-register.md`](192-terminology-glossary-register.md) (full proposal) · [`192-terminology-apply.py`](192-terminology-apply.py) (the applier).

---

## TL;DR — what this is

#607 applied the **6 ratifiable Scenarii PT groups** (S1, S4, S5, S6, S7, S8 — 39 cells, drift-free, merged `44c37fa2`). **8 groups were correctly left PENDING** — exactly jsboige's ratification "*RTL/CJK + BR/PT deferred to native*". These 8 are **non-blocking v0.9.0** (the tag ships with the majority variant on each). This doc turns them into a ~10-min ✅-checklist so a native speaker can close #192 cleanly after release.

**Re-verified on `44c37fa2`** via `python docs/taxonomy/192-terminology-apply.py` (dry-run): `0 would-change, 8 pending groups`. The 6 applied groups are gone from the queue; only the 8 below remain.

---

## The 8 groups (ratify the **RATIFY** column)

> **Confidence**: LOW = near-tie / RTL/CJK nuance → native confirmation needed · MED = majority defensible.
> **RATIFY**: write `✅ <chosen variant>` or `keep majority <variant>` or `defer` in the last column. Re-run the applier (§"After ratification") to apply.

### Virtues — 6 RTL/CJK near-ties (V1–V6)

| # | field.lang | FR source | Variants (count) | Recommended | Conf | RATIFY |
|---|---|---|---|---|---|---|
| **V1** | `family.fa` | Raisonnement valide | `استنتاج معتبر`(32) · `استدلال معتبر`(23) | majority `استنتاج معتبر` (58%) — near-tie (inference vs argument nuance) | LOW → native | |
| **V2** | `subsubfamily.ru` | Objectif non complaisant | `Некомплаентная объективность`(2) · `Непотворствующая цель`(1) | paraphrase `Непотворствующая цель` likely — calque `комплаентная` is an awkward loanword; n=3 too small to assert | LOW → native | |
| **V3** | `subsubfamily.ar` | Mise à distance des idéologies | `إبعاد الأيديولوجيات`(3) · `النأي عن الأيديولوجيات`(2) | `النأي عن الأيديولوجيات` (more idiomatic) — but minority | LOW → native | |
| **V4** | `subsubfamily.zh` | Raisonnement concluant | `有结论力的推理`(19) · `结论性推理`(14) · `结论成立的推理`(8) | `结论性推理` (concise, parallel "conclusive reasoning") or majority `有结论力的推理` | LOW → native | |
| **V5** | `subsubfamily.fa` | Mise à distance des idéologies | `فاصله‌گذاری از ایدئولوژی‌ها`(3) · `فاصله‌گیری از ایدئولوژی‌ها`(2) | majority `فاصله‌گذاری…` — near-synonym, low-stakes | LOW-MED → native optional | |
| **V6** | `subsubfamily.fa` | Raisonnement concluant | `استدلال نتیجه‌بخش`(26) · `استدلال منتج`(8) · `استدلال قاطع`(7) | majority `استدلال نتیجه‌بخش` (63%) | MED | |

### Scenarii PT — 2 (S2, S3)

| # | field | FR source | Variants (count) | Recommended | Conf | RATIFY |
|---|---|---|---|---|---|---|
| **S2** | `sous-catégorie.pt` | contes | `Contos e literatura`(7) · `contos`(3) | **scope decision** — FR "contes" = tales only; PT majority expanded to "tales + literature". If FR is authoritative → retract PT to `Contos`; if broader scope preferred → expand FR to "Contes et littérature" (1-cell FR edit). | MED → jsboige | |
| **S3** | `sous-catégorie.pt` | drague et séduction | `Paquera e sedução`(6) · `engate e sedução`(3) | **regional (BR vs PT-PT)** — `Paquera` = BR (common, informal flirting); `engate` = Portugal-PT. Recommend **BR `Paquera`** (de-facto default for digital PT) unless jsboige targets Portugal. Apply the same choice to every PT register-dependent label. | FLAG → jsboige | |

---

## Factual rationale (to ratify fast)

**Portuguese (S2, S3) — ratifiable from these facts:**

- **S3 regional** (`Paquera` BR 6 vs `engate` PT-PT 3): a **dialect/register** split, not correctness. **Decision = target audience**: Brazil (largest PT readership) → `Paquera e sedução`; Portugal / neutral international PT → `engate e sedução`. Default recommendation: **BR (`Paquera`)**.
- **S2 scope** (`Contos e literatura` 7 vs `contos` 3): a **scope expansion** crept into PT. FR source = "contes" (tales only); PT majority = "tales *and* literature". **Decision = is the category tales-only or tales+literature?** FR authoritative → PT retracts to `Contos` (3 outliers right). Default recommendation: **align PT → `Contos`** (FR is the source of truth).

**RTL/CJK (V1–V6) — native-required, gpt-5.5 assist optional:**

These are near-ties where the raw majority is a weak default. A gpt-5.5 candidate pass (`/v1/responses` + `reasoning.effort=low`, per memory `gpt55-responses-api-effort-low`) can surface the nuance (e.g. V4 `结论性推理` "conclusive-natured" vs `有结论力的推理` "having-conclusive-force" — a native picks the natural one) but **must be confirmed by a native speaker** — do **not** ratify an LLM pick as authoritative. If no native is available → `defer` (already non-blocking v0.9.0; the ship carries the majority variant).

- **V2** (ru): the calque `комплаентная` is an awkward loanword — likely the paraphrase `Непотворствующая цель` wins, but n=3 is too small to assert.
- **V3/V5** (ar/fa): near-synonyms, low-stakes — both readable. Majority is a safe interim default; native optional.

---

## After ratification — how po-2024 applies

1. For each group ratified `✅ <variant>` (or `keep majority`), add/confirm an `EXCEPTIONS` entry in [`192-terminology-apply.py`](192-terminology-apply.py):
   - `("Virtues", "family_fr", "fa", "Raisonnement valide"): ("OVERRIDE", "<ratified variant>")` (change `FLAG` → `OVERRIDE`), **or** leave as-is for `keep majority` (the applier picks majority by default).
2. Run `python docs/taxonomy/192-terminology-apply.py --verify-drift` first (assert dialect safe).
3. Run `python docs/taxonomy/192-terminology-apply.py --apply` (writes only ratified cells).
4. Re-run `--verify-drift` + dry-run → expect `0 would-change, 0 pending` on ratified groups.
5. PR → ai-01 merge. **#192 fully closed**.

**Scope precision** (lesson `192-apply-script-post-ratification`): groups are scoped by FR source label — no cross-family row leak. Verified again on `44c37fa2` dry-run.

---

## Coverage reminder

#192 was never a coverage gate — text translation is **100 %** across 7 languages × 4 datasets. The only coverage gap is `link_*` URLs (human research, separate track, **out of #192 LLM scope** per memory `i18n-coverage-gap-is-link-urls`). Once these 8 are ratified, #192 passes 2-4 are fully DONE.

---

*This checklist converts the 8 deferred near-ties into a ~10-min ratifiable task. It does not pre-empt the native decision — it pre-digests it.*
