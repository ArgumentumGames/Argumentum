# #189 — Print & Play « complete » alternative: scoping & options

**Issue**: [#189 — feat(print&play): add complete alternative for TarotCards Print&Play](https://github.com/ArgumentumGames/Argumentum/issues/189)
**Track**: #458 Track 4 (éditorial / features cartes)
**Author**: po-2024 (worker backlog)
**Date**: 2026-06-14
**Status**: Scoping (key-free analysis) — **design decisions pending jsboige**

> Scope of this document: **analysis only**. No pipeline code touched, no
> regeneration. Implementation (new `CardSetDocument` / flags) is gated on the
> design decisions at the end and on the v0.9.0 release gate being lifted.

---

## 1. What the "light" version currently is (factual, from `0dab862b`)

The current TarotCards Print & Play is defined in
`WebBasedGeneratorConfig.cs:505-598`:

| Aspect | Value |
|---|---|
| Document | `Argumentum_TarotCards_Print&Play_A4_fr.pdf` |
| Page | A4, `CardDocumentFormat.PrintAndPlay` |
| Languages | FR source + 7 translations (en/ru/pt/es/ar/fa/zh) |
| Card sets | Rules ×1, Fallacies ×1, Virtues ×1, Memo ×5 |
| Card size | 60 × 113 mm (recto **and** verso) |
| `BorderMM` | **0** on every front **and** back card |

And the rendering engine `PrintAndPlayDocument.cs` has **no** handling of any of:
crop marks, bleed (fond perdu), inter-card gap/spacing, or alignment marks
(grep for `Crop|bleed|Bleed|mark|Mark|gap|Gap|spacing|margin` → 0 hits).

**Conclusion**: the "light" PDF is cards laid out **edge-to-edge** (`BorderMM=0`,
no gaps), front+back, no cut guides, no bleed. Optimised for cheap/fast home
printing and guillotine cutting by eye.

## 2. Gap analysis — what "complete/premium" would add

Based on #189's open questions and standard Print & Play practice:

| Feature | Light (now) | Complete (proposed) | Where it lives |
|---|---|---|---|
| **Cut/bleed margin** | 0 mm (`BorderMM=0`) | 3–5 mm bleed + safe margin | `DocumentCard.BorderMM` (already a field, unused) + `PrintAndPlayDocument` layout |
| **Crop/alignment marks** | none | registration ticks at card corners | `PrintAndPlayDocument.cs` (new logic) |
| **Inter-card gap** | none (edge-to-edge) | 2–3 mm gutter (cutting tolerance) | `PrintAndPlayDocument.cs` (new logic) |
| **Full backs** | backs present (uniform) | family-coloured backs verified at scale | verification, not new code |
| **Rules face+dos** | already front+back | (already covered — Rules ×1 FR+back) | n/a |
| **Memo integral** | Memo ×5 | possible × full deck (optional) | `NbCopies` field |
| **Print-shop ready** | no (home only) | yes (CMYK, bleed, marks) | `ConvertToCmyk` already exists |

**The two real, new pieces of work** are (a) make `BorderMM`/gap actually do
something in `PrintAndPlayDocument`, and (b) add crop-mark drawing. Everything
else is config (`NbCopies`, `ConvertToCmyk`) that already exists.

## 3. Options

### Option A — Second distinct `CardSetDocumentConfig` (recommended by #189)
Add a second document `Argumentum_TarotCards_Print&Play_Full_A4_{lang}.pdf`
alongside the light one, with `BorderMM>0`, CMYK on all sets, and crop marks.

- **Pro**: both PDFs coexist; user chooses; light stays untouched (no
  regression risk on the validated light version); per-document flags are clean.
- **Con**: needs crop-mark + bleed logic in `PrintAndPlayDocument.cs` (the real
  work); doubles Print & Play generation time/size (Disk-risky on po-2023 — see
  risks below).

### Option B — Config flag for "level"
A single document, toggled by a config flag (`PrintAndPlayLevel = Light|Full`).

- **Pro**: one document, one code path.
- **Con**: can't ship both in one run; flag proliferation; harder to QA both at
  once. **Not recommended.**

### Option C — Defer / won't-do for v0.9.0
Track 4 is explicitly **low priority** (release + DNN are the critical path).
Ship light-only for v0.9.0, revisit for v1.0.

- **Pro**: zero risk to the gated release; no disk pressure.
- **Con**: defers the feature.

## 4. Recommendation

**Option C now → Option A for post-v1.0**, because:
1. The cluster is **release-gated** (jsboige validation pending, today).
   Adding a 2nd large P&P PDF multiplies Release output size and regeneration
   cost — disk-risky given po-2023's 488 GB Docker pressure.
2. The "light" version is already validated/complete for home use; the gap to
   "complete" is real but cosmetic/premium, not blocking.
3. Option A is the right *eventual* shape (coexisting PDFs), but its real cost
   is the new crop-mark/bleed logic in `PrintAndPlayDocument.cs` — worth doing
   once, properly, after v0.9.0 ships.

If jsboige wants it **in** v0.9.0, Option A is implementable but must be
scheduled before the release regeneration, not after.

## 5. Implementation sketch (post-decision, for the eventual PR)

```
PrintAndPlayDocument.cs  (new)
  - honor DocumentCard.BorderMM as real bleed margin
  - draw crop/registration marks at card corners when BorderMM > 0
  - optional inter-card gutter

WebBasedGeneratorConfig.cs  (new doc, Option A)
  new CardSetDocumentConfig { DocumentName = "..._Full_A4_{lang}.pdf",
    DocumentFormat = PrintAndPlay,
    CardSets = [ Rules/Fallacies/Virtues/Memo with BorderMM=4, ConvertToCmyk=true ] }
```

Estimated effort: ~1 day (layout math + crop-mark drawing + per-language regen
QA). **Visual verdict = ai-01** (worker signals, does not declare PASS).

## 6. Decisions needed from jsboige

1. **In or out of v0.9.0?** (recommendation: out — Option C now)
2. If in: **what exactly distinguishes light from complete?** Confirm the
   table in §2 (bleed margin mm? crop marks yes/no? CMYK all sets? Memo copies?).
3. If out: ack to close/defer #189 to a v1.0 milestone?

---

_Generated by po-2024 (worker backlog), key-free scoping. No code or pipeline
artefact modified. Implementation gated on decisions above + release gate._
