# `mnemonics_to_latin.py` — Virtues syllogistic-mnemonic normaliser (#654, Option B)

A stdlib-only Python tool that reverts the transliterated syllogistic mnemonics in the Virtues
taxonomy to their canonical Latin form for the 4 non-Latin languages (RU/AR/ZH/FA), so all 8
languages are consistent with FR/EN/ES/PT (which already keep the Latin technical term).

**Decision (VERIFIED jsboige 2026-07-04, dispatch `q9xpks`):** Option B "keep-Latin" — Scenario
S2 of [`docs/investigations/2026-07-03-654-mnemonics-celltable.md`](../docs/investigations/2026-07-03-654-mnemonics-celltable.md).
The 19 classical mnemonics (Barbara, Celarent, … Bamalip) are Latin canonical terms; the
languages that transliterated some of them revert to Latin.

**This is NOT translation.** It is a deterministic script-conversion of a fixed Latin technical
term already present in `title_fr`. gpt-5.5 is inappropriate (would hallucinate transliterations
/ drift). The transliteration per language is detected in the current cell (the language's own
established rendering) and swapped for the canonical Latin form — never invented.

## Run

```bash
# Dry-run (default): print the 52-cell plan, write nothing.
python tools/mnemonics_to_latin.py

# Dry-run + write a markdown report (the 52 cells before/after).
python tools/mnemonics_to_latin.py --report docs/investigations/2026-07-05-654-mnemonics-dryrun.md

# Apply in place (GATED — do NOT run on prod CSV until #654 is unblocked post-tag).
python tools/mnemonics_to_latin.py --apply
```

**Exit code:** `0` = plan built / applied cleanly; `1` = CSV not found; `2` = ambiguous cells
excluded from the plan (`--apply` is refused until they are resolved in
`STRUCTURAL_TOKENS` / `extract_translit_token`).

No dependencies beyond the Python 3 standard library (`csv`, `argparse`, `os`, `sys`, `re`).

## Scope (code=truth on master `d90ce613`)

| Field | Cells transliterated | In scope? |
|-------|---------------------|-----------|
| `title_<lang>` | **52** (RU 14 / AR 16 / ZH 6 / FA 16) | ✅ converted |
| `description_<lang>` | 0 (descriptions carry no mnemonic) | N/A |
| `remark_<lang>` | ~14 (RU 4 / AR 4 / ZH 2 / FA 4) | ⛔ out of scope (title-only per dispatch) |

The 14 `remark_*` cells are surfaced in the dry-run report as a follow-up flag — extending the
script to `remark` is a one-line change if jsboige decides #654 covers it.

## How it works

1. For each mnemonic pk (the 19 depth-7 CQ rows whose `title_fr` = `Syllogisme <M>`), read the
   canonical Latin mnemonic `M` from `title_fr`.
2. For each non-Latin language, if `title_<lang>` already contains `M` → kept-Latin, SKIP.
3. Otherwise (transliterated): strip the language's structural native words
   (`Силлогизм` / `قياس` / `المنطقي` / `三段论` / `式` / `قیاس`) — the residue is the
   transliterated mnemonic token. Guard: if the residue is empty, multi-token, or contains
   Latin letters, the cell is flagged **ambiguous** and excluded (surfaced for manual review).
4. Plan: replace the transliterated token with `M` inside `title_<lang>`.
5. `--apply`: parse the CSV with `csv.reader`, modify **only** the `title_<lang>` field, reserialise with `QUOTE_MINIMAL` + CRLF + BOM. The diff vs the input is exactly the 52 swapped
   tokens — quoting, delimiters, line endings and the BOM are byte-preserved.

### The CJK-boundary fix (why not `\b`)

Latin-mnemonic detection uses `(?<![A-Za-z])…(?![A-Za-z])`, NOT `\b…\b`. Under `re.UNICODE`
(default for `str`), `\w` includes CJK letters, so `\b` does **not** fire at the Latin→CJK
boundary — `"Festino三段论"` would be missed (the ZH cells would be spuriously flagged
ambiguous). The Latin-letter lookaround is the contract pinned by `test_cjk_glued_boundary`.

## Tests

```bash
python tools/test_mnemonics_to_latin.py
```

26 contract tests (stdlib `unittest`): mnemonic extraction, the CJK-boundary regression,
translit-token extraction per language with ambiguity guards, plan-building on synthetic CSV,
`--apply` round-trip (title-only, BOM/CRLF preserved, idempotent), and a **grounding test** on
the real Virtues CSV asserting the plan is exactly 52 cells / 0 ambiguous. If the corpus drifts,
the grounding test surfaces it before a worker re-runs `--apply`.

## Sources

- [`docs/investigations/2026-07-03-654-virtues-mnemonics-inventory.md`](../docs/investigations/2026-07-03-654-virtues-mnemonics-inventory.md) — analysis-only inventory (#660).
- [`docs/investigations/2026-07-03-654-mnemonics-celltable.md`](../docs/investigations/2026-07-03-654-mnemonics-celltable.md) — apply-ready cell table (#668).
- [`docs/investigations/2026-07-05-654-mnemonics-dryrun.md`](../docs/investigations/2026-07-05-654-mnemonics-dryrun.md) — the 52-cell dry-run report produced by this script.
- Issue #654, dispatch `q9xpks`. Base `d90ce613`.
