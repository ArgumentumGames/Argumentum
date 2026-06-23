# Prod CSV / Memo Hygiene Audit — 2026-06-23

**Dispatched by:** ai-01 (cycle 03:13 UTC, `msg-…` post-merge #579/#580): *"scan data-hygiene autres CSVs prod (Scenarii/Virtues/Rules/Memo)"*.
**Worker:** po-2024.
**Method:** apply the **full #579 defect lens** (not just the narrow %C3 scan done in #580) — percent-encoding corruption, scheme-less URLs, leaked dev-notes, HTML entities, mojibake — cell-level + raw-byte, across all active prod tabular CSVs and the Memo templates.

## TL;DR

| Asset | Result |
|---|---|
| **Memo JSON** (`Argumentum_Memo_Back_fr.json`, `Argumentum_Memo_Face_fr.json`) | **6 `%C3`→`A13` URL corruptions per file (12 total) — FIXED in this PR.** Identical signature to #579. |
| **Scenarii** CSV | **CLEAN.** (24 `todo` matches = the Spanish word *"todo"* = "all", in correct translations — **false positives**, see §3.) |
| **Virtues** CSV | **CLEAN.** (31 `todo` = Spanish/Portuguese *"all/every"*, mostly syllogism forms *"Todo M es P"*. **i18n 100% claim holds.**) |
| **Rules** / **RulesPP** CSV | **CLEAN.** (4 `todo` = *"all"* in translated rules: *"Todos os outros jogadores"*.) |

The `%C3`→`A13` corruption is **Fallacies + Memo specific** (both embed the same French-wiki URL catalog with the same encoding mishap). The three other prod CSVs have **zero** percent-encoding corruption, zero mojibake, zero HTML entities, zero scheme-less URLs, zero real dev-note leaks.

**Class (c) verbatim-dup scan** (§4): 0 misfiled URLs; cross-lang identical confined to short fields (proper nouns / Ibero-Romance cognates / shared wiki links) — **0 long-prose contamination** (rules out #216). **One content-level finding flagged for jsboige**: 9 RU cells in the "Mathematical accuracy" family (pk 71–78) hold untranslated English "Valid results" (§4).

---

## 1. The one real defect — Memo JSON `%C3`→`A13` (FIXED)

Same bug as #579: the UTF-8 lead-byte percent-encoding `%C3` was corrupted to `A13` in French Wikipedia URLs embedded in the Memo card's fallacy catalog (line 30 of each template). **Twin-proof holds** — each file carries **28 correct `%C3%`** sequences alongside the **6 corrupted `A13%`** twins; if `%C3` were not the intended encoding, those 28 correct forms would not exist.

Restored (per file, ×2 files = 12 total):

| corrupted | restored | decodes to |
|---|---|---|
| `autoritA13%A9` | `autorit%C3%A9` | autorité |
| `(dA13%A9cision)` | `(d%C3%A9cision)` | décision |
| `PA13%A9tition` | `P%C3%A9tition` | Pétition |
| `consA13%A9quent` | `cons%C3%A9quent` | conséquent |
| `vrai_A13%89cossais` | `vrai_%C3%89cossais` | Écossais |
| `A13%89quivoque` | `%C3%89quivoque` | Équivoque |

**Safety proof:** every `A13` in both files is `A13%` inside a wiki-URL context; **0 bare `A13`** anywhere (so replacing `A13%`→`%C3%` has zero false-positive risk). Post-fix: `A13` count = 0; `%C3%` = 34 (28+6); byte count unchanged (4-char→4-char ASCII in-place); CRLF preserved; JSON parses valid; `git diff --stat` = 2 files / 2 ins / 2 del (surgical, 1 line/file — all 6 restorations sit on line 30).

**Scope:** only the **2 active prod** Memo templates fixed. The **Archive/2022** Memo files (`English`/`Francais`/`Print_and_Play` variants) carry the same corruption but are **frozen historical snapshots** — left untouched (No-Pendulum: don't rewrite history without a GO).

---

## 2. Method — the full #579 defect lens

Six defect classes scanned (raw-byte + cell-level with row PK + column):

| Class | Pattern |
|---|---|
| `%C3`→`A13` corruption | literal `A13` / `A13%` |
| Other garbled percent-encoding | `(?<!%)A13%` / orphan `%C3` |
| Scheme-less URL | `www\.` not preceded by `http(s)://` |
| Leaked dev-note | `XXX|TODO|FIXME|HACK|ptet|ptdr|à vérifier|???|<placeholder>` |
| HTML entity | `&word;` / `&#NNN;` |
| Mojibake | `Ã©/Ã¨/â€™/ï¿½/Â¦/Ã¢` |

Scanned on: `Cards/Scenarii/Argumentum Scenarii - Cards.csv` (167 rows × 70 cols), `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` (223 × 66), `Cards/Rules/Argumentum Rules - Cards.csv` (15 × 10), `Cards/Rules/Argumentum Rules - Cards Print and Play.csv` (6 × 10), `Cards/Memo/Argumentum_Memo_{Back,Face}_fr.json`.

**Result on the 4 CSVs: 0 real hits on every class.** BOM: Scenarii/Virtues/RulesPP = no-BOM, Rules = BOM (all pre-existing, unchanged). No %C3 corruption, no mojibake, no entities, no scheme-less URLs.

---

## 3. The false-positive trap — `todo` ≠ placeholder (CRITICAL for future scans)

The dev-note scan surfaced **59 `todo` matches** across the three CSVs (Scenarii 24, Virtues 31, Rules 4). A naive reader would flag these as **leaked placeholder strings** (like #579's `XXXptet inverser` dev-note). **They are not.** Every one is the **Spanish / Portuguese word *"todo"*** (= "all / every / whole") inside a fully-correct, complete translation. Verified by reading the full cell values:

- **Scenarii** — `"todo va bien"` (everything is fine), `"todo el poder"` (all the power), `"todo o resto"` (all the rest).
- **Virtues** — syllogism forms are the bulk: `"Todo M es P; todo S es M; por lo tanto todo S es P"` (= "All M is P…", the Barbara syllogism), plus `"todo o raciocínio"` (all reasoning), `"compreender um todo"` (understand a whole). These are **high-quality complete translations** of the syllogistic-virtue family.
- **Rules** — `"Todos os outros jogadores"` (all other players), `"por todo o júri"` (by all the jury) — correct rule translations.

**Consequence:** the CLAUDE.md claim *"Virtues i18n 100% coverage (title/description/remark × fr/en/ru/pt)"* **HOLDS** — the 31 `todo` hits are legitimate vocabulary, not gaps. Cross-check confirmed: for every affected Virtue PK, the `fr`/`en`/`ru` remark columns are also populated (no row is half-translated).

**Lesson for future hygiene scans:** a case-insensitive `\bTODO\b` regex is a **trap on multilingual corpora** — it matches the common Romance word. Always read the full cell value (or scope the pattern to isolated `^todo$` / uppercase `TODO` / `XXX`-style markers) before flagging. Bulk-deleting these 59 cells would have been a catastrophic regression — exactly the "vibecodé" failure mode this project suffered pre-recovery.

---

## 4. Class (c) — verbatim duplicate scan (completes the dispatch DoD)

The dispatch explicitly listed 4 defect classes; §2 covered encoding/dev-notes/entities/mojibake. This section closes class (c): **verbatim duplicates between columns**. Two sub-checks:

**(c1) URL-like value in a non-link column** (the #579 PK-61 pattern — `Simple_name_en` duplicated `link_en`):
- Scenarii: **0** · Virtues: **0** · Rules: **0**. Clean — no misfiled URLs.

**(c2) Cross-language byte-identical cells** (contamination signal, cf. the #216 FR-leak bug):
- Scenarii 53 hits, Virtues 154 hits — **but all in SHORT fields**: proper nouns (character names *Sócrates / Hades / Penélope / Scheherazade* shared across PT/ES/ar/fa), Ibero-Romance cognates (Virtues *Argumento válido*, PT==ES), and shared wiki links (legitimate fallback where a language has no native Wikipedia article).
- **Long-prose contamination test (len ≥ 30, any `description_*`/`remark_*`/`context_*`/`issue_*`/`suggestion_*` byte-identical across languages): 0 in both CSVs.** Long prose diverges across every language → **rules out #216-style contamination.** The cross-lang identity is confined to short fields where it is linguistically legitimate.

### ⚠️ Flagged for jsboige — 9-cell RU "Valid results" gap (content decision, not auto-fixed)

One real content-level finding surfaced by (c2): the **"Mathematical accuracy" family** (pk 71–78) has `subfamily_ru` = English **"Valid results"** (untranslated), while the same field is translated in every other language:

| pk | family_en | subfamily_fr | subfamily_en | **subfamily_ru** | subfamily_es/pt |
|---|---|---|---|---|---|
| 71–78 | Mathematical accuracy | Résultats valides | Valid results | **Valid results (English!)** | Resultados válidos |

Plus pk 71 `title_ru` = "Valid results" (English). **9 RU cells total** hold untranslated English. This is an isolated oversight (the rest of `subfamily_ru`/`title_ru` is correctly Cyrillic — e.g. pk 72 `title_ru`="Точные измерения", pk 75="Числовая точность"), **not** mass contamination.

**Not auto-fixed** because the correct Russian term is a content judgment: *Достоверные результаты* / *Правильные результаты* / *Корректные результаты* — needs native/PO judgment (or a gpt-5.5 pass scoped to this family). Flagged here per the dispatch's "flag content-judgment items for jsboige" instruction (same handling as #579's PK 816 EN-slug and PK 39 swap).

## 5. Verification

- Memo fix: `A13` 6→0, `%C3%` 28→34 per file, bytes unchanged, CRLF preserved, `ConvertFrom-Json` VALID, `git diff` surgical (no non-`A13` deletions).
- CSVs: read-only scan, no writes (no `git diff` on CSVs this PR).
- Audit scripts: `tmp/_hygiene_audit.ps1`, `tmp/_hygiene_deep.ps1`, `tmp/_todo_verify.ps1`, `tmp/_memo_fix.ps1` (ephemeral, `tmp/` gitignored).

## Context

Cluster: ai-01 + po-2023 (#568 in flight) + po-2024 (this audit). Follows #579 (Fallacies %C3→A13, merged `ed66f800`) and #580 (#499 prod-write spec, merged `9db76d91`). This PR closes the "extend hygiene scan to other prod CSVs" dispatch by proving the three tabular CSVs are clean and fixing the one real defect found (Memo, same bug class as #579).

🤖 Worker po-2024 · prod-CSV hygiene audit · Memo %C3 fix (12 URLs) · Scenarii/Virtues/Rules clean · 59 false-positive `todo` documented
