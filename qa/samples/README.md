# QA Samples — Release 8 Languages

**Generated**: 2026-06-01  
**Master**: `7c8e8bd4` (PR #408 taxonomy labels + #407 Virtues i18n + #406 timeout fix)  
**Config**: Release build (`-c Release`), CMYK + PNG lossless, print-ready  

## Sample Coverage

| Sample | Language | CardSet | Card ID | Purpose |
|--------|----------|---------|---------|---------|
| `ar_fallacies_1.1.1_face.png` | AR | Fallacies | 1.1.1 | RTL Arabic rendering, joined glyphs |
| `ar_scenarii_1.1.1_face.png` | AR | Scenarii | 1.1.1 | Arabic scenario card, text direction |
| `ar_virtues_*_face.png` | AR | Virtues | root | RTL + propagated hierarchy labels |
| `fa_fallacies_1.1.1_face.png` | FA | Fallacies | 1.1.1 | Persian RTL, glyph rendering (پچژگ) |
| `fa_virtues_*_face.png` | FA | Virtues | root | Persian RTL + propagated labels |
| `zh_rules_01_face.png` | ZH | Rules | 01 | CJK sinograms, no tofu boxes |
| `zh_virtues_*_face.png` | ZH | Virtues | root | CJK + propagated hierarchy labels |
| `ru_fallacies_long_title_face.png` | RU | Fallacies | 1.1.2.3 | Cyrillic, overflow test (#316 auto-shrink) |
| `ru_fallacies_2.1.1_hier.png` | RU | Fallacies | 2.1.1 | Cyrillic + propagated Family/Subfamily labels |
| `pt_fallacies_1.1_face.png` | PT | Fallacies | 1.1 | Latin, propagated taxonomy labels |

## QA Checklist for ai-01

- [ ] RTL direction correct (AR/FA) — text flows right-to-left
- [ ] Arabic glyphs properly joined (not disconnected)
- [ ] Persian glyphs correct (پچژگ visible, no tofu)
- [ ] CJK sinograms rendering (no tofu boxes, correct stroke order)
- [ ] Cyrillic content correct, no overflow (#316 auto-shrink validated)
- [ ] Hierarchy labels (Family/Subfamily/Subsubfamily) visible on:
  - AR/FA/ZH Virtues root cards
  - RU/PT Fallacies cards (propagated via PR #408)
- [ ] Card layout intact (colors, backgrounds, borders)
- [ ] No FR contamination on translated cards
