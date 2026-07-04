# 2026-07-04 — Rules coverage: CSV (card-print) vs DNN site — #662 / #684 prep

**Scope**: precise cartography of what Rule prose **exists/missing per language** in the repo CSV
vs the DNN site, to prep the rule-prose translation step (#684, step 5 of the #669 portage plan).
Triggered by ai-01 dispatch `sde6s0` (secondary). Read-only analysis (no DB access needed — CSV is
in-repo; DNN site side reconciled from the #662 audit already merged).

**Repo reference**: master `21e2c666`. CSV coverage verified by direct parse (Python `csv`, handles
embedded newlines/quotes in the markdown `Text_*` fields).

---

## TL;DR

- **CSV (card-print) Rules = 100% covered, 0 leak on master.** 15 rows × 8 langs = **120/120 cells
  filled**, all native (verified Cyrillic/CJK/RTL script presence + spot-checks).
- **P&P Rules CSV = 100% covered.** 6 rows × 8 langs = **48/48 cells filled**.
- **DNN site Rules = FR-only** (5/5 jeux FR published, 7 langs unpublished — from #662 audit).
- **The CSV ≠ site content** (#669 §2c): the site Rule content is richer HTML (Summary/Material/
  Installation/Content/Variants/Memo × 5 games ≈ 30 prose entities), DB-only. The 8 CSV
  translations are a **secondary reference**, not directly reusable as-is for site portage (#684).

➡️ **The translation gap for #684 is NOT a CSV-coverage gap** (CSV is complete) — it's a **site-DB
content gap**: the ~30 rich HTML entities exist only in FR on the site and need chunked gpt-5.5
prose translation (Scenarii-style DatasetUpdater), then re-import to 2sxc (jsboige).

---

## 1. Two Rule surfaces (reconciliation target)

| Surface | Location | Content | Languages | Used by |
|---------|----------|---------|-----------|---------|
| **CSV (card-print)** | `Cards/Rules/Argumentum Rules - Cards.csv` (+ P&P variant) | Markdown prose, 15 entities | **8/8** (all filled) | AssetConverter → PDF/cards |
| **DNN site (2sxc DB)** | prod DB only (not in repo) | Rich HTML, ~30 entities × 5 games (Summary/Material/Installation/Content/Variants/Memo) | **1/8** (FR only) | RulesExplorer (`_RulesExplorer_Rule{List,Detail}.cshtml`) |

The CSV and the site DB are **different content models**: the site has more fields per rule and
more rules (5 games), authored manually as HTML. Per #669 §2c (merged), this was confirmed by
binary scan of the prod backup (#687): the 2sxc Rule content-type uses generic fields (`EntityTitle`,
`Summary`, `Material`, `MinNbPlayers`…) with **0 language-suffixed fields**, and the EAV is
dimensioned **FR+EN only**.

---

## 2. CSV coverage — `Argumentum Rules - Cards.csv` (15 rows)

Columns: `pk,Text,Text_en,Text_ru,Text_pt,print_and_play,Text_ar,Text_es,Text_zh,Text_fa`.

| Lang | Filled | Empty | Native script verified |
|------|--------|-------|------------------------|
| `fr` (canonical) | 15/15 | 0 | ✅ |
| `en` | 15/15 | 0 | ✅ |
| `ru` | 15/15 | 0 | ✅ Cyrillic (e.g. Rules_01 "Школа лжецов", Rules_15 "Ходы игры") |
| `pt` | 15/15 | 0 | ✅ |
| `es` | 15/15 | 0 | ✅ |
| `ar` | 15/15 | 0 | ✅ RTL |
| `fa` | 15/15 | 0 | ✅ RTL |
| `zh` | 15/15 | 0 | ✅ CJK (e.g. Rules_01 "说谎者学校") |

**0 empty cells, 0 FR-leak on master `21e2c666`.** Character counts per lang are coherent (fr ~1000–1500c, zh ~250–500c — the zh delta is CJK glyph compactness, not missing content; verified Rules_01 + Rules_15 native).

> **Note on a false positive**: an initial scan on the `dnn/sandbox-runtime-1032` working tree
> flagged `Rules_15 ru` as FR-contaminated ("…Dans une succession de 7 tours, les joueurs vont…").
> **Verified FALSE on master `21e2c666`**: Rules_15 ru is fully native Russian ("### 3. Ходы игры\n\n
> В течение 7 ходов каждый игрок сопоставляет 1 карту…", 0 FR markers). The leak existed in an older
> CSV revision on the sandbox branch and is already fixed on master. No action needed.

### P&P Rules CSV — `Argumentum Rules - Cards Print and Play.csv` (6 rows)

Same 8 language columns. **48/48 cells filled** (6 × 8), 0 empty, 0 leak suspect.

---

## 3. DNN site side (reconciled from #662 audit, merged)

From `docs/investigations/2026-07-03-dnn-prod-rules-coverage.md` (#662):
- **5/5 jeux FR présents** on the prod site (FR = canonical, published).
- **7 langs non publiées** (en/ru/pt/es/ar/fa/zh) — the site serves FR-only for Rules.
- Content is richer HTML than the CSV, DB-only (not derivable from the repo CSV or the Rules
  Markdown in `Cards/Rules/regles.md`/`rules.md`).

The #687 binary scan confirmed the field-model reason: the 2sxc Rule content-type has **generic,
language-neutral fields** (no `Summary_en`/`Summary_fr`), and the EAV is dimensioned FR+EN only.
The site therefore cannot serve the other 6 langs until either (Path A) lang-suffixed fields +
`loc()` cascade are added (PR #674 runtime-pending), or (Path B) 5 new EAV cultures are enabled.
#682 is forced toward **Path A**.

---

## 4. Reconciliation conclusion for #684

| Question | Answer |
|----------|--------|
| Is the CSV Rules coverage a blocker for #684? | **No** — CSV is 120/120 + 48/48, 0 leak on master. |
| Is the CSV reusable as-is for site portage? | **No** — site content is richer HTML (more fields, 5 games, ~30 entities), DB-only (#669 §2c). |
| What is the #684 translation gap then? | The **site-DB content** (~30 rich HTML entities) exists only in FR; needs chunked gpt-5.5 prose translation + re-import to 2sxc. |
| CSV role in #684? | **Secondary reference** for the 15 card-print rules (already localized) — useful as terminology anchor, not the translation source. |
| Pre-requisites before #684 can run? | #681 (portal/2sxc export, jsboige) → #682 (field-model Path A provisioning) → PR #674 (view refactor runtime-valid) → #683 (DNN serves cultures). |

---

## 5. DoD status

| DoD item (dispatch `sde6s0` secondary) | Status |
|----------------------------------------|--------|
| Precise cartography of Rule prose exists/missing per lang in CSV | ✅ 120/120 + 48/48, 0 leak on master (verified by parse + spot-checks) |
| Cartography vs DNN site | ✅ Reconciled from #662 (5/5 FR, 7 unpublished) + #687 field-model |
| Coverage table per language × Rule entity | ✅ Delivered (§2) |
| Prep for #684 | ✅ Gap identified = site-DB content (not CSV); pre-requisites mapped |

---

## Gate boundaries (HARD — read-only)

- ❌ No CSV mutation, no DB access, no site interaction.
- ❌ No #674 merge, no runtime verdict (ai-01 lane).
- ✅ All evidence read-only: CSV parse + reconciled #662/#687 findings.
- ✅ Corrected a false-positive FR-leak (sandbox-branch artifact) — master is clean.

Relates: dispatch `sde6s0` (secondary), #662 (DNN prod Rules audit), #669 (portage mechanism),
#681 (export unblocker), #682 (field-model), #674 (view refactor), #684 (translation DoD),
#458 (TRACK 2 epic).
