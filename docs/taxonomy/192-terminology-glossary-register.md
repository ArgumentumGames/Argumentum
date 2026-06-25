# #192 — Terminology Glossary & Register Proposal

**Author**: po-2024 (worker) · **Date**: 2026-06-25 · **Base**: master `bef3bc6c` (release-frozen)
**Status**: **PROPOSAL** — awaits jsboige / native ratification at WE 27/06
**Dispatch**: ai-01 2026-06-25 14:05 (`msg-…o0vg3h`) — pre-instruct the glossary/register gate so #192 passes 2-4 become ratifiable.
**Scope**: docs proposal only. **0 write under `Cards/`** (release freeze). master stays `bef3bc6c`.

---

## TL;DR for jsboige (ratify in ~10 min)

1. **Text translation is DONE**: 100 % across 7 languages × 4 datasets (title/description/remark, text/desc/example, scenario fields, rules). Coverage is **not** the gate. *(see §5 appendix)*
2. **The gate = terminological consistency** on shared taxonomy/category labels. Audit found **14 multi-variant groups** where the same FR label carries divergent translations across rows:
   - **1 OBVIOUS** (≥80 % majority, mechanical) + **13 ARBITRARY** (<80 %, judgment).
   - Distribution: **Virtues 6** (residue of #595), **Scenarii PT 8**, **Fallacies 0**, **Rules N/A**.
3. **Two decisions unblock everything**:
   - **Decision A — Register convention** for category labels (sentence-case vs title-case). Resolves ~5 cases **mechanically** (pure capitalization).
   - **Decision B — Lexical pick** per remaining case (standardize on majority, or flag for native). 4 of these are true judgment calls (regional register, number concordance, meaning disambiguation).
4. **To ratify**: annotate the **RATIFY** column per row below (✅ `<choice>` or `native`), or reply with global defaults. po-2024 then executes the harmonization PR(s) — cell-level drift-free (QUOTE_MINIMAL + CRLF, same method as #595). → **#192 passes 2-4 = DONE.**

---

## ⚠️ Honesty correction vs my earlier audit

My #595-prep audit under-reported: it built non-EN target columns as `<FR-stem>_<lang>` (e.g. `catégorie_pt`), but Fallacies & Scenarii store translated columns as `<EN-stem>_<lang>` (e.g. `category_pt`). The mismatch silently **skipped every non-EN language for Scenarii & Fallacies** → I reported "Scenarii = 0" wrongly. The audit behind this doc (`audit_arbitrary_dump.py`, uniform `<en-base>_<lang>` rule) corrects that: **Scenarii PT residue (8 groups) was always present**, now surfaced. Fallacies is genuinely 0 (re-verified both ways). This is the same cross-verify rigor applied to release dossier #591 — a 2nd pass caught what the 1st missed.

---

## Decision A — Register / capitalization convention (cross-cutting)

Category/subcategory labels appear on cards. Current Scenarii PT mixes sentence-case and title-case for the *same* label across rows. Pick one convention → ~5 cases resolve mechanically (no judgment).

| Option | Effect | Cases resolved |
|---|---|---|
| **Sentence-case** (recommended) — first word capitalized, proper nouns excepted; matches FR source ("relation intime", "contes", "religions") | Lightest change, consistent with FR | Scenarii #4, #5, #6, cap-portion of #3, #8 |
| Title-case — every content word capitalized | Heavier, more formal | (same cases, opposite normalization) |

**RATIFY Decision A**: `sentence-case ✅` / `title-case` / `keep-as-is`

---

## Decision B — Lexical picks (per case)

> **Confidence legend**: HIGH = mechanical/obvious · MED = majority is defensible · LOW = near-tie or RTL/CJK nuance → **native confirmation recommended**.
> **RATIFY**: write `✅ <chosen variant>` or `native` in the last column.

### Virtues — 6 ARBITRARY (residue of #595)

| # | field.lang | FR source | Variants (count) | Class | Recommended default | Conf | RATIFY |
|---|---|---|---|---|---|---|---|
| V1 | `family.fa` | Raisonnement valide | `استنتاج معتبر`(32) · `استدلال معتبر`(23) | lexical (inference vs argument) | majority `استنتاج معتبر` (58%) — near-tie | LOW → native | |
| V2 | `subsubfamily.ru` | Objectif non complaisant | `Некомплаентная объективность`(2) · `Непотворствующая цель`(1) | lexical (calque vs paraphrase) | flag — calque `комплаентная` is awkward; n=3 tiny | LOW → native | |
| V3 | `subsubfamily.ar` | Mise à distance des idéologies | `إبعاد الأيديولوجيات`(3) · `النأي عن الأيديولوجيات`(2) | lexical (literal vs idiomatic) | `النأي عن الأيديولوجيات` (more idiomatic) — but minority | LOW → native | |
| V4 | `subsubfamily.zh` | Raisonnement concluant | `有结论力的推理`(19) · `结论性推理`(14) · `结论成立的推理`(8) | lexical (3-way nuance) | `结论性推理` (concise, parallel to "conclusive reasoning") or majority | LOW → native | |
| V5 | `subsubfamily.fa` | Mise à distance des idéologies | `فاصله‌گذاری از ایدئولوژی‌ها`(3) · `فاصله‌گیری از ایدئولوژی‌ها`(2) | lexical (near-synonym, low-stakes) | majority `فاصله‌گذاری…` | LOW-MED → native optional | |
| V6 | `subsubfamily.fa` | Raisonnement concluant | `استدلال نتیجه‌بخش`(26) · `استدلال منتج`(8) · `استدلال قاطع`(7) | lexical (3-way register) | majority `استدلال نتیجه‌بخش` (63%) | MED | |

### Scenarii PT — 7 ARBITRARY + 1 OBVIOUS

| # | field | FR source | Variants (count) | Class | Recommended default | Conf | RATIFY |
|---|---|---|---|---|---|---|---|
| S1 | `catégorie.pt` | relation intime | `relação íntima`(20) · `Relacionamento íntimo`(16) | lexical + cap | `relação íntima` (matches FR "relation") + sentence-case | MED | |
| S2 | `sous-catégorie.pt` | contes | `Contos e literatura`(7) · `contos`(3) | **scope mismatch** vs FR | `Contos e literatura` (majority) — *flag: should FR "contes" expand too?* | MED → jsboige | |
| S3 | `sous-catégorie.pt` | drague et séduction | `Paquera e sedução`(6) · `engate e sedução`(3) | **regional (BR vs PT)** | depends on game's PT target — majority = BR `Paquera` | **FLAG → jsboige** | |
| S4 | `sous-catégorie.pt` | gestion et administration | `gestão e administração`(6) · `Gestão e administração`(3) | **cap only** → Decision A | `gestão e administração` (sentence-case) | HIGH | |
| S5 | `sous-catégorie.pt` | moyen-âge et temps modernes | `Idade Média e era moderna`(5) · `Idade Média e Tempos Modernos`(1) | **cap (OBVIOUS 83%)** → Decision A | `Idade Média e era moderna` ("Idade Média" stays capped = proper noun) | HIGH | |
| S6 | `sous-catégorie.pt` | relations au travail | `Relações de trabalho`(6) · `relações no trabalho`(2) | **meaning difference** (labor vs at-work) | depends on intent — majority = labor `Relações de trabalho` | **FLAG → jsboige** | |
| S7 | `sous-catégorie.pt` | religions | `Religião`(7) · `Religiões`(4) | **number (sing vs plural)** | `Religiões` (plural = matches FR source) — *contra majority* | MED-HIGH | |
| S8 | `sous-catégorie.pt` | vie de couple | `vida de casal`(10) · `Vida a dois`(5) · `vida a dois`(1) | lexical + cap | `vida de casal` (majority, literal to FR) + sentence-case | MED | |

**Fallacies**: 0 multi-variant groups (consistent — re-verified both column-naming ways).
**Rules**: N/A — `Text` × 8 langs, no shared taxonomy grouping labels (each rule row is standalone).

### Decisions that genuinely need jsboige (not mechanical)

- **S3 regional**: BR-PT (`Paquera`) vs PT-PT (`engate`) — what is the game's Portuguese target audience?
- **S6 meaning**: "relations au travail" = workplace interpersonal dynamics (`relações no trabalho`) vs labor/employment relations (`Relações de trabalho`)?
- **S7 number**: match FR plural ("religions" → `Religiões`, recommended) or keep majority singular?
- **S2 scope**: does category "contes" include "literature" (expand FR) or not (PT → bare `contos`)?
- **V1–V6 (RTL/CJK)**: recommend native confirmation; majority is a safe interim default if no native available at WE.

---

## Fluency / Register rubric per language (pass 3)

Guiding tone per language — ratify or amend. Category labels follow **Decision A**; body text (card descriptions) follows this register.

| Lang | Register guidance (current state → target) |
|---|---|
| **fr** | Reference source. Academic-accessible, **« Vous »** form (per jsboige directive). ✓ stable. |
| **en** | **Impersonal** (per jsboige directive, no "you"). Plain-English, academic-accessible. ✓ stable. |
| **ru** | Formal-but-accessible; avoid loanword calques where a native term exists (cf V2 `комплаентная`). |
| **pt** | **Regional register TBD (BR vs PT)** — see S3. Decide once, apply globally. |
| **es** | Neutral peninsular + Latin-American accessible. |
| **ar** | MSA (Modern Standard Arabic), formal. |
| **fa** | Formal Persian. |
| **zh** | Simplified Chinese, formal; prefer concise 4-char compounds where natural (cf V4). |

**RATIFY register rubric**: `✅ as-is` / amend `<lang>: <note>`.

---

## §5 — Coverage appendix (read-only, measured 2026-06-25)

Confirms text translation is complete; the only coverage gap is `link_*` URLs (human research, **out of #192 LLM scope** per memory `i18n-coverage-gap-is-link-urls`).

| Dataset | Rows | Text fields | link_* gap |
|---|---|---|---|
| Virtues | 223 | title/desc/remark **100 %** ×7 langs | es 41 %, zh 47 %, fa 45 % (URLs) |
| Fallacies | 1408 | text/desc/example **99-100 %** ×7 langs | ru/pt/es/ar/zh/fa 6-9 % (URLs); example −7..−9 rows |
| Scenarii | 167 | 6 fields **100 %** ×7 langs | — |
| Rules | 15 | Text **100 %** ×7 langs | — |

**Implication**: #192 passes 2-4 are about **harmonization decisions**, not coverage. Once Decision A + B are ratified, the work is a bounded mechanical harmonization (≈65 Virtues + ≈30 Scenarii PT cells), not a re-translation.

---

## After ratification — what po-2024 executes

1. Harmonization PR(s) applying ratified choices: **Virtues** (closes #595 residue) + **Scenarii PT**.
2. Method: cell-level, `QUOTE_MINIMAL` + CRLF (byte-identical round-trip, same as #595). Re-run consistency audit → 0 ARBITRARY remain on ratified groups.
3. RTL/CJK groups ratified `native` stay deferred until native review (documented, not blocking).
4. → **#192 passes 2-4 = DONE.** Remaining #192 item = `link_*` URL research (human, separate track, post-release).

---

## Reproducibility

Both audit scripts are read-only (0 CSV write), committed alongside this doc:
- [`192-terminology-audit.py`](192-terminology-audit.py) — full variant breakdown, 4 datasets, classifies OBVIOUS/ARBITRARY.
- [`192-coverage-report.py`](192-coverage-report.py) — per-field × per-lang fill rate + link_* gaps.

Re-run anytime from repo root: `python docs/taxonomy/192-terminology-audit.py` / `python docs/taxonomy/192-coverage-report.py`.

---

*This proposal pre-instructs — does not pre-empt — jsboige's decision. It converts an open gate into a ratifiable checklist so the WE review takes minutes, not an open-ended translation debate.*
