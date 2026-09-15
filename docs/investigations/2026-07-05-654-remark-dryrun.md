# #654 mnemonics → Latin — `remark_*` dry-run (extension of the title-only #695)

**Source CSV:** `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` · **Base:** master `70bd1605`
**Author:** po-2024 (worker) · **Date:** 2026-07-05
**Dispatch:** `ka7vl5` (SECONDARY) · **Decision:** keep-Latin (VERIFIED jsboige 2026-07-04, dispatch q9xpks)
**Status:** DRY-RUN ONLY — `--apply` stays gated post-tag (0 write under `Cards/`).

Extends [`tools/mnemonics_to_latin.py`](../../tools/mnemonics_to_latin.py) (PR #695, merged) with a
`--fields` flag so it can scan `remark_<lang>` in addition to `title_<lang>`. This report is the
dry-run of `--fields remark`.

## Headline finding — remark is NOT title-shaped (token-strip does not apply)

**0 of 14 remark cells are auto-convertible by the title pipeline; all 14 are flagged ambiguous.**

The title pipeline (`extract_translit_token`) works by stripping the language's structural native
words (`Силлогизм` / `قياس` / `三段论` / `قیاس`) and recovering the mnemonic as an isolated residue.
That works for titles because the title *is* just `<structural-word> <mnemonic>`. For remarks, the
cell is a **full prose sentence** where the transliterated mnemonic is embedded mid-sentence
(typically at the start, sometimes after a short lead like « Силлогизм в форме Дисамис »):

- `remark_fr` always begins `<Mnemonic> relève de… / suit la forme…` (mnemonic at position 0).
- `remark_<lang>` mirrors that, but the mnemonic is transliterated AND followed by native prose, so
  the residue after structural-word stripping is multi-token → ambiguous guard correctly excludes it.

**This is the correct signal**: a remark conversion is a **token substitution inside a sentence**,
not an isolated-token swap. The deterministic title approach does not generalise to remark without a
different extraction strategy (regex-find the transliterated mnemonic by matching it to the
canonical Latin form known from `remark_fr`, then replace that substring).

## Inventory (14 transliterated remark cells, code=truth on `70bd1605`)

| pk | mnemonic | RU | AR | FA | ZH |
|---:|---|:--:|:--:|:--:|:--:|
| 116 | Darapti | ✗ | ✗ | ✗ | ✗ |
| 117 | Felapton | ✗ | ✗ | ✗ | ✗ |
| 118 | Disamis | ✗ | ✗ | ✗ | ✓ (kept-Latin) |
| 119 | Datisi | ✗ | ✗ | ✗ | ✓ (kept-Latin) |

**Totals**: 14 transliterated (RU 4 / AR 4 / FA 4 / ZH 2) + 2 ZH kept-Latin (pks 118, 119 — ZH
already renders those two as Latin). The 14 are on pks **116-119 only** (the 3rd-figure Darapti /
Felapton / Disamis / Datisi cluster); the other 15 mnemonic pks have no mnemonic in `remark_fr`
(remark is a prose example, only these 4 modes spell the form out by name).

## Options for jsboige (arbitration — not executed here)

- **Option R1 — defer remark (reco).** The title scope (#695, 52 cells) is already delivered and
  validated; the 14 remark cells are a *separate, smaller* surface with a different shape. Ship
  v0.9.0 with title-only #654, treat remark as a post-tag follow-up. **Lowest risk.**
- **Option R2 — extend the script with a remark-specific extractor.** Add a second code path: for
  remark cells, regex-find the transliterated mnemonic by anchoring on the canonical form in
  `remark_fr` (the sentence-initial token, or the token after a known lead phrase), and replace just
  that substring with the Latin form. ~30 lines of code + tests on synthetic prose. Medium effort.
- **Option R3 — manual edit.** 14 cells is small enough for a hand-edited CSV change reviewed by a
  native reader (the prose context matters for naturalness). Heavier to review, but no script
  fragility.

The dry-run below is the evidence for whichever option jsboige picks.

## Ambiguous cells (the 14, full content for manual review)

> Residue shown is the post-strip remainder — for remarks it is the whole sentence minus any
> structural word, hence multi-token. `reason: empty/multi-token/Latin-leak` = the ambiguity guard
> firing on the multi-token prose residue (NOT a real data problem — the data is well-formed).

- **pk 116 [ru] remark** `Darapti`: `Дарапти — силлогизм третьей фигуры, имеющий форму: все P есть M, все S есть M, следовательно, некоторые S есть P. Например: все врачи — профессионалы; все хирурги — врачи; следовательно, некоторые хирурги — профессионалы.`
- **pk 116 [ar] remark** `Darapti`: `ينتمي دارابتي إلى الشكل الثالث: كل M هو P، وكل M هو S، إذن بعض S هو P. مثال: كل الأطباء مهنيون؛ كل الأطباء حاصلون على شهادات؛ إذن بعض الحاصلين على شهادات مهنيون.`
- **pk 116 [zh] remark** `Darapti`: `达拉普蒂属于第三格：所有 M 都是 P，所有 M 都是 S，因此有些 S 是 P。例：所有医生都是专业人员；所有医生都有文凭；因此，有些有文凭者是专业人员。`
- **pk 116 [fa] remark** `Darapti`: `داراپتی به شکل سوم تعلق دارد: همهٔ Mها P هستند، همهٔ Mها S هستند، پس برخی Sها P هستند. مثال: همهٔ پزشکان حرفه‌مندند؛ همهٔ پزشکان دانش‌آموخته‌اند؛ پس برخی دانش‌آموختگان حرفه‌مندند.`
- **pk 117 [ru] remark** `Felapton`: `Фелаптон следует форме: ни один M не есть P, всякий M есть S, следовательно, некоторые S не есть P. Пример: ни одно животное не является духом; всякое животное является субстанцией; следовательно, некоторые субстанции не являются духами.`
- **pk 117 [ar] remark** `Felapton`: `يتبع فيلابتون الصيغة الآتية: لا شيء من M هو P، وكل M هو S، إذن بعض S ليس P. مثال: لا شيء من الحيوان بروح؛ كل حيوان جوهر؛ إذن بعض الجواهر ليست أرواحًا.`
- **pk 117 [zh] remark** `Felapton`: `费拉普顿遵循如下形式：没有 M 是 P，所有 M 都是 S，因此有些 S 不是 P。例：没有动物是精神；所有动物都是实体；因此，有些实体不是精神。`
- **pk 117 [fa] remark** `Felapton`: `فلاپتون از این صورت پیروی می‌کند: هیچ Mای P نیست، هر Mای S است، پس برخی Sها P نیستند. مثال: هیچ جانوری روح نیست؛ هر جانوری جوهر است؛ پس برخی جوهرها روح نیستند.`
- **pk 118 [ru] remark** `Disamis`: `Силлогизм в форме Дисамис состоит из высказывания следующего типа: некоторый M есть P, а всякий M есть S, следовательно, некоторый S есть P. …` *(mnemonic mid-sentence after « в формы »)*
- **pk 118 [ar] remark** `Disamis`: `يتبع ديساميس الصيغة الآتية: بعض M هي P، وكل M هي S، إذن بعض S هي P. …`
- **pk 118 [fa] remark** `Disamis`: `دیسامیس از این صورت پیروی می‌کند: برخی Mها P هستند؛ همهٔ Mها S هستند؛ پس برخی Sها P هستند. …`
- **pk 119 [ru] remark** `Datisi`: `Силлогизм Датиси — это силлогизм особой формы Барбара, в котором большая посылка является O, меньшая — I, …` *(note: also embeds « Барбара » — pk 106's mnemonic — which R2 must NOT touch unless 106 is in scope)*
- **pk 119 [ar] remark** `Datisi`: `يتبع داتيسي الصيغة الآتية: كل M هي P، وبعض M هي S، إذن بعض S هي P. …`
- **pk 119 [fa] remark** `Datisi`: `داتیسی از این صورت پیروی می‌کند: همهٔ Mها P هستند؛ برخی Mها S هستند؛ پس برخی Sها P هستند. …`

(ZH pks 118/119 are kept-Latin — `Disamis 遵循…` / `Datisi 遵循…` — so they are out of the conversion set.)

## Reproducibility

```bash
python tools/mnemonics_to_latin.py --fields remark       # dry-run (this report)
python tools/mnemonics_to_latin.py --fields title        # title-only (52 cells, #695 scope)
python tools/mnemonics_to_latin.py --fields title,remark # both
```

Relates to #654, #695 (title-only script), dispatch `ka7vl5` SECONDARY. Base `70bd1605`. 0 write
under `Cards/`, `--apply` gated post-tag.
