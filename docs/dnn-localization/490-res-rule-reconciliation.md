# 2026-07-09 — #490 §6 `res.Rule*` reconciliation (live 2sxc v21 export #681)

**Scope**: reconciliation of the 8 `res.Rule*` App-Resource keys (consumed by
`_RulesExplorer_RuleDetail.cshtml` via `@Resources.X`) against the **live 2sxc v21 export** (#681,
delivered 2026-07-08 by web1, read-only SQL Method B). This **lifts the #490 HOLD** — the issue was a
HOLD-prep because the canonical FR lived "DB-only" and the 7 `res.*` FR values were *inferred* from the
resource-key names. The export now provides the ground truth (entity id=10340, FR dimensionless default),
so the FR column is **verified, no longer inferred**.

**Repo reference**: master `a39533eb`. Issue: #490 (res.* rail). Triggered by ai-01 dispatch `k0z7uj`
(2026-07-09, picking up po-2024's HOLD-prep lane now that export #681 is available). Owner: jsboige (DB
provisioning) + worker (analysis = this doc + CSV notes). **Analysis-only — no DB write, no prod
interaction** (export = Method B read-only SELECT).

**Export source**: `.shared-state/attachments/DNN-Argumentum-export-2026-07-07/13-app60-resources.csv`
(11 App-Resources on eid=10340, app=60, FR dimensionless default).

---

## TL;DR — HOLD lifted, 5/8 verified, 3 FR deltas + 1 orphan finding

The export resolves the #490 verification gate: **5 of 8 referenced `res.Rule*` keys match the CSV FR
exactly**, confirming those rows. **3 FR deltas** need jsboige arbitration (none blocks the verification
itself — the rows are reliable, the deltas are about *which* FR is canonical). **1 finding**: `RuleContent`
is **not** a "missing key to add" (as dispatch `k0z7uj` framed it) — it is an **orphan heading resource**
tied to a **view-template bug** (the Content section at `_RulesExplorer_RuleDetail.cshtml:47` renders its
body `@Html.Raw(ruleEntity.Content)` with **no `<h2>` heading**, unlike every sibling section). Resolving
that is a view fix, not a localization act.

| key | CSV `fr` | live DB `fr` (eid=10340) | verdict |
|-----|----------|--------------------------|---------|
| `res.RuleSummary` | `Résumé` | `Résumé` | ✅ MATCH |
| `res.RuleMaterial` | `Matériel` | `Materiel` | ⚠ **DELTA** — DB missing accent (typo); CSV correct |
| `res.RuleInstallation` | `Installation` | `Installation` | ✅ MATCH |
| `res.RuleVariants` | `Variantes` | `Variantes` | ✅ MATCH |
| `res.RuleMemoCard` | `Carte mémo` | `Carte Mémo` | ⚠ **DELTA** — case (sentence vs title) |
| `res.RuleMemoInstructions` | `Vous pouvez télécharger la carte de mémo suivante…` | `<p>Vous pouvez t&eacute;l&eacute;charger la carte de m&eacute;mo suivante…</p>` | ✅ MATCH (CSV = plain canonical; DB = HTML-encoded rendered — same text) |
| `res.RuleMemoCardFileNamePrefix` | `Mémo` | `Argumentum - Règle` | ⚠ **DELTA** — CSV wrong; DB is the real brand prefix |
| `res.RuleMemoCardDownload` | `Télécharger la carte mémo` | `Télécharger la carte mémo` | ✅ MATCH |
| `res.RuleContent` | *(not in CSV)* | `Contenu` | ❓ **ORPHAN** — DB has it, **no view references it** |

➡️ **#490 HOLD lifted.** All 8 referenced keys are now verified against ground truth. The 3 deltas +
RuleContent finding are documented below for jsboige arbitration; they do not block the verification
gate (which was: "is the inferred FR reliable?"). Answer: yes, modulo the 3 deltas.

---

## §1 — Verified keys (5/8, no action needed)

These 5 keys have CSV `fr` == DB `fr` (eid=10340). The inferred FR was **correct**. Their `notes` column
in `dnn-ui-strings.csv` is updated from `INFERRED FR; … verify vs export` →
`VERIFIED FR via export #681 (eid=10340); CSV=DB match`.

- `res.RuleSummary` = `Résumé`
- `res.RuleInstallation` = `Installation`
- `res.RuleVariants` = `Variantes`
- `res.RuleMemoInstructions` = `Vous pouvez télécharger la carte de mémo suivante pour retrouver facilement les règles.` (CSV plain UTF-8 = canonical translation source; DB stores the HTML-encoded `<p>…&eacute;…</p>` rendered form — same text, different encoding for their respective roles)
- `res.RuleMemoCardDownload` = `Télécharger la carte mémo`

---

## §2 — The 3 FR deltas (jsboige arbitration)

### Δ1 — `res.RuleMaterial`: DB typo (missing accent)

- **CSV**: `Matériel` (correct French, with accent)
- **DB**: `Materiel` (no accent — **typo**)
- **Verdict**: CSV is correct; DB has a typo. The rendered `<h2>` currently shows "Materiel" (wrong).
- **Recommended action (jsboige)**: fix the DB FR on eid=10340 from `Materiel` → `Matériel`. No CSV change
  (CSV already correct). The 7 non-FR translations (`Materials`/`Материалы`/…) are unaffected (they were
  translated from the *correct* FR, so they remain valid).

### Δ2 — `res.RuleMemoCard`: case style (sentence vs title)

- **CSV**: `Carte mémo` (sentence case — lowercase `m`)
- **DB**: `Carte Mémo` (title case — capital `M`)
- **Verdict**: both are valid French for a `<h2>` heading. This is a **style** choice, not an error.
- **Recommended action (jsboige)**: arbitrate the intended case. Options:
  - **(a) Title case** `Carte Mémo` (matches current DB render) → align CSV `fr` → `Carte Mémo`, keep the 7
    translations (they're case-insensitive equivalents: `Memo card`/`Памятка`/…).
  - **(b) Sentence case** `Carte mémo` (matches CSV, more conventional for FR headings) → fix DB → `Carte mémo`.
- **Default (no signal)**: keep CSV `Carte mémo`; flag DB for alignment. Non-blocking.

### Δ3 — `res.RuleMemoCardFileNamePrefix`: CSV value is wrong

- **CSV**: `Mémo`
- **DB**: `Argumentum - Règle`
- **Why the DB is right**: the view (`:67`) renders `<div class="cardName">@Resources.RuleMemoCardFileNamePrefix - @ruleEntity.EntityTitle</div>`.
  With the DB value, the memo-card name reads `Argumentum - Règle - {EntityTitle}` (e.g. "Argumentum - Règle -
  Fallacies"). With the CSV `Mémo`, it would read `Mémo - {EntityTitle}` — not the intended branded name.
- **Verdict**: the CSV `fr` was **wrong** (inferred as the bare word "memo"). The real value is the
  **brand compound** `Argumentum - Règle`.
- **Recommended action (jsboige)** — **two sub-questions**:
  1. **FR fix**: update CSV `fr` `Mémo` → `Argumentum - Règle` (to match DB canonical).
  2. **Translation policy**: is the prefix **brand-fixed** (`Argumentum - Règle` kept verbatim in all
     langs, since "Argumentum" is the brand) or **partially translated** (translate "Règle" →
     `Argumentum - Rule` / `Argumentum - Правило` / …)? The current 7 CSV translations
     (`Memo`/`Памятка`/`Memorando`/…) translate the *bare word* "memo" and are **stale** once the FR is
     corrected to the brand compound — they need re-translation under the chosen policy. This row's 7
     non-FR values are therefore **HOLD-pending** this arbitration.

> ⚠ **Δ3 is the only delta with translation-blast radius.** Δ1 and Δ2 leave the 7 translations valid; Δ3
> invalidates them pending the brand-fixed-vs-translated decision. Recommend jsboige arbitrate Δ3 before
> any re-import of this row's 7 langs.

---

## §3 — The `RuleContent` finding (orphan heading resource — NOT a new localization key)

Dispatch `k0z7uj` framed `RuleContent` as "nouvelle clé absente du CSV". The read-body finding corrects
this: **`RuleContent` is not missing from the CSV — it should not be in the CSV at all (yet), because no
view consumes it.**

### Evidence

- The export (`13-app60-resources.csv`) lists `RuleContent` = `Contenu` on eid=10340. So the resource
  **exists in the DB**.
- A grep of every `.cshtml` under `DNNPlatform/Portals/1/2sxc/Argumentum/` finds **8** `@Resources.Rule*`
  references — `RuleContent` is **not among them**.
- The view `_RulesExplorer_RuleDetail.cshtml` renders **7 content sections**, each with a
  `<h2>@Resources.RuleXxx</h2>` heading followed by `@Html.Raw(ruleEntity.Xxx)` body — **except Content**:

  | line | heading | body |
  |------|---------|------|
  | 37 | `<h2>@Resources.RuleSummary</h2>` | `@Html.Raw(ruleEntity.Summary)` |
  | 41 | `<h2>@Resources.RuleMaterial</h2>` | `@Html.Raw(ruleEntity.Material)` |
  | 43 | `<h2>@Resources.RuleInstallation</h2>` | `@Html.Raw(ruleEntity.Installation)` |
  | **47** | **(no `<h2>` heading)** | `@Html.Raw(ruleEntity.Content)` |
  | 50 | `<h2>@Resources.RuleVariants</h2>` | `@Html.Raw(ruleEntity.Variants)` |

  Line 47 renders the Content body **without a heading**, breaking the section pattern. The `res.RuleContent`
  = `Contenu` resource in the DB is almost certainly **the intended heading for this section** — the
  `<h2>@Resources.RuleContent</h2>` line was dropped (or never added).

### Two resolution scenarios (jsboige arbitrate)

- **(a) View bug** (likely): add `<h2>@Resources.RuleContent</h2>` before line 47 (matching the sibling
  pattern). Then `RuleContent` becomes a **real, referenced** resource → add it to `dnn-ui-strings.csv`
  with `Contenu` + 7 translations, and localize it. **Candidate translations ready** (§4 manifest) if this
  path is chosen: `Contenu` / `Content` / `Содержание` / `Conteúdo` / `Contenido` / `المحتوى` / `محتوا` / `内容`.
- **(b) Intentional**: the Content section deliberately has no heading → `res.RuleContent` is a **dead
  resource** → clean it up from eid=10340 (DB housekeeping, jsboige), and **do not** add it to the CSV.

➡️ **Do NOT add `RuleContent` to the CSV in this PR** (pending the view-bug arbitration). Adding it
speculatively would either (a) localize a key that scenario (b) would delete, or (b) commit translations
before the view wires the heading. The CSV stays at 8 referenced keys. The candidate translations are
documented here so scenario (a) is a 1-step follow-up.

---

## §4 — Provisioning manifest (8 keys × 8 langs, ready to push into eid=10340)

This is the "provisioning list" for jsboige: the per-language values to attach to the App-Resource entity
(eid=10340) so `@Resources.X` resolves per request culture. The FR values are the verified canonical; the
7 non-FR values come from the existing `dnn-ui-strings.csv` translations (gpt-5.5), **except Δ3
(RuleMemoCardFileNamePrefix) which is HOLD-pending brand-policy arbitration**.

> **Mechanism note — App-Resources use 2sxc language dimensions, NOT lang-suffixed attributes.** Unlike
> the #682 content-type decision (Path A = suffixed attrs + `loc()` cascade), the `res.*` rail is consumed
> via `@Resources.X`, which the 2sxc framework resolves against the **current culture dimension**
> natively. The lowest-risk provisioning is therefore **enabling language dimensions on the Resources
> content-type** (app=60) and attaching one value per dimension — **no view change needed**. Lang-suffixed
> resource keys (`RuleSummary_en` etc.) would require rewriting every `@Resources.X` call and is rejected.
> (The dispatch's "Path A lang-suffixed" wording applies to the #682 content-type attrs, not the res.*
> rail — see [682-field-model-revision-2sxc21.md](682-field-model-revision-2sxc21.md) §2 for the
> two-rails distinction.)

| key | fr (canonical) | en | ru | pt | es | ar | fa | zh |
|-----|----------------|----|----|----|----|----|----|-----|
| `RuleSummary` | Résumé | Summary | Краткое описание | Resumo | Resumen | ملخص | خلاصه | 摘要 |
| `RuleMaterial` | **Matériel** ⚠Δ1 (DB typo) | Materials | Материалы | Material | Material | المواد | محتویات بازی | 材料 |
| `RuleInstallation` | Installation | Setup | Подготовка | Preparação | Preparación | الإعداد | آماده‌سازی | 设置 |
| `RuleVariants` | Variantes | Variants | Варианты | Variantes | Variantes | الأنواع | گونه‌ها | 变体 |
| `RuleMemoCard` | Carte mémo ⚠Δ2 (case) | Memo card | Памятка | Cartão de resumo | Tarjeta de resumen | بطاقة تذكيرية | کارت یادآوری | 备忘卡 |
| `RuleMemoInstructions` | Vous pouvez télécharger la carte de mémo suivante pour retrouver facilement les règles. | You can download the following memo card to refer back to the rules easily. | Вы можете скачать следующую карточку-памятку, чтобы легко вернуться к правилам. | Você pode baixar o cartão-memo a seguir para consultar as regras facilmente. | Puedes descargar la siguiente tarjeta memo para consultar fácilmente las reglas. | يمكنك تنزيل بطاقة التذكير التالية للرجوع إلى القواعد بسهولة. | می‌توانید کارت یادآوری زیر را دانلود کنید تا به‌راحتی به قوانین مراجعه کنید. | 你可以下载以下备忘卡，以便轻松查阅规则。 |
| `RuleMemoCardFileNamePrefix` | **HOLD ⚠Δ3** (`Argumentum - Règle`? brand policy TBD) | HOLD | HOLD | HOLD | HOLD | HOLD | HOLD | HOLD |
| `RuleMemoCardDownload` | Télécharger la carte mémo | Download memo card | Скачать карточку-памятку | Baixar cartão-memo | Descargar tarjeta memo | نزّل بطاقة التذكير | دانلود کارت یادآوری | 下载备忘卡 |

**`RuleMemoInstructions`** carries rich HTML in the DB (`<p>…</p>`, HTML entities). The values above are
the plain-UTF-8 canonical text; on re-import, the DatasetUpdater/toolchain wraps them in the `<p>`
container the view expects (`@Html.Raw`). Confirm the wrap policy with the `tools/dnn_i18n/` re-import
verifier before pushing.

### Additional App-Resources on eid=10340 (out of #490 scope, noted for completeness)

The export shows **2 more** App-Resources on eid=10340 that are **not `res.Rule*`** and **not referenced
by any RuleExplorer view**: `Author` = `Auteur`, `Licence` = `Licence`. These likely back other
content-types (entity-ref display labels) or are themselves orphan. **Not in #490 scope** (the RuleExplorer
`res.*` rail only) — flagged for a future App-Resources-wide audit if jsboige wants one.

---

## §5 — CSV mutation in this PR (`dnn-ui-strings.csv`)

**Minimal, non-speculative**: the `notes` column of the 8 referenced `res.*` rows is updated from
`INFERRED FR; … verify vs export` → verification status, with deltas documented inline. **No FR cell is
mutated** (the 3 deltas await jsboige arbitration — mutating speculatively would pre-empt the decision).
**`RuleContent` is NOT added** (pending §3 view-bug arbitration).

| line | key | new `notes` (from → to) |
|------|-----|-------------------------|
| 4 | RuleSummary | `INFERRED…` → `VERIFIED FR via export #681 (eid=10340); CSV=DB match` |
| 5 | RuleMaterial | → `VERIFIED FR via export #681; DELTA Δ1 — DB "Materiel" (typo, no accent); CSV "Matériel" correct; DB fix flagged jsboige` |
| 6 | RuleInstallation | → `VERIFIED FR via export #681; CSV=DB match` |
| 7 | RuleVariants | → `VERIFIED FR via export #681; CSV=DB match` |
| 8 | RuleMemoCard | → `VERIFIED FR via export #681; DELTA Δ2 — DB "Carte Mémo" (title) vs CSV "Carte mémo" (sentence); case arbitration jsboige` |
| 9 | RuleMemoInstructions | → `VERIFIED FR via export #681 (HTML-encoded <p>…</p> in DB); CSV = canonical translation source; en/ru/pt/es/ar/fa/zh via gpt-5.5` |
| 10 | RuleMemoCardFileNamePrefix | → `VERIFIED FR via export #681; DELTA Δ3 — DB "Argumentum - Règle" (real brand prefix) vs CSV "Mémo" (wrong); brand-policy arbitration jsboige (7 langs HOLD)` |
| 11 | RuleMemoCardDownload | → `VERIFIED FR via export #681; CSV=DB match` |

---

## §6 — DoD status (dispatch `k0z7uj` primaire)

| DoD item | Status |
|----------|--------|
| Reconcile 4 divergences | ✅ 3 FR deltas documented (§2) + 1 finding re-classified (§3 — RuleContent is orphan, not a new key) |
| Provisioning list lang-suffixed × 7 non-FR (Path A) | ✅ §4 manifest (8 keys × 8 langs); mechanism = 2sxc language dimensions (res.* rail ≠ content-type Path A — documented) |
| CSV/doc `res.*` reconciled | ✅ notes column updated (§5); no speculative FR mutation; RuleContent not added (pending §3) |
| 0 write DNN prod | ✅ export = Method B read-only SELECT; no DB/IIS/CSV-content mutation |
| PR gated review ai-01 | ✅ this PR |

**Open (jsboige arbitration, non-blocking)**: Δ1 (Material DB typo fix), Δ2 (MemoCard case), Δ3
(FileNamePrefix brand policy + 7-lang re-translation), §3 (RuleContent view-bug: add `<h2>` heading vs
drop orphan resource).

---

## §7 — Gate boundaries (HARD — analysis only)

- ❌ No DB write, no provisioning executed, no prod interaction (export = read-only Method B SELECT).
- ❌ No CSV *content* mutation (FR cells untouched — deltas await arbitration; only the `notes` column
  records verification status).
- ❌ `RuleContent` NOT added to CSV (pending §3 view-bug arbitration — adding speculatively = pre-empting
  the decision).
- ❌ No view-template change (the line-47 heading gap is documented as a finding, not fixed in this PR —
  view lane, separate scope).
- ✅ HOLD lifted on verified evidence (live export #681 = ground truth for the 8 referenced keys).
- ✅ Honest about the dispatch's RuleContent mis-framing (re-classified as orphan/view-bug, not a new key).

Relates: dispatch `k0z7uj` (primary), #490 (this issue, HOLD-prep by po-2024), #681 (export, ground
truth), #682 (content-type Path A — distinct rail), #487 (DatasetUpdater res.* config, `Enabled=false`),
#457 (document-tier rail), `tools/dnn_i18n/` (extractor + re-import verifier), #669 (mechanism).
