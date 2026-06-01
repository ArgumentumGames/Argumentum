# QA Samples — Release 8 Languages

**Generated**: 2026-06-01  
**Master**: `7c8e8bd4` (PR #408 taxonomy labels + #407 Virtues i18n + #406 timeout fix)  
**Config**: Release build (`-c Release`), CMYK + PNG lossless, print-ready  

## Sample Coverage

| Sample | Language | CardSet | Card ID | Purpose |
|--------|----------|---------|---------|---------|
| `ar_fallacies_1.1.1_face.png` | AR | Fallacies | 1.1.1 | RTL Arabic rendering, joined glyphs |
| `ar_scenarii_1.1.1_face.png` | AR | Scenarii | 1.1.1 | Arabic scenario card, text direction |
| `ar_virtues_argumentum_virtues_0.._face.png` | AR | Virtues | root (0.) | RTL + root card layout |
| `ar_virtues_1.1_hier_face.png` | AR | Virtues | 1.1 | **Deep — RTL + propagated hierarchy labels (PR #408)** |
| `fa_fallacies_1.1.1_face.png` | FA | Fallacies | 1.1.1 | Persian RTL, glyph rendering (پچژگ) |
| `fa_virtues_argumentum_virtues_0.._face.png` | FA | Virtues | root (0.) | Persian RTL + root card layout |
| `zh_rules_01_face.png` | ZH | Rules | 01 | CJK sinograms, no tofu boxes |
| `zh_virtues_argumentum_virtues_0.._face.png` | ZH | Virtues | root (0.) | CJK root card |
| `zh_virtues_1.1_hier_face.png` | ZH | Virtues | 1.1 | **Deep — CJK + propagated hierarchy labels (PR #408)** |
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
  - **AR Virtues 1.1 (deep)** + ZH Virtues 1.1 (deep) — primary #408 label-propagation witnesses
  - AR/FA/ZH Virtues root cards (layout sanity)
  - RU/PT Fallacies cards (propagated via PR #408)
- [ ] Card layout intact (colors, backgrounds, borders)
- [ ] No FR contamination on translated cards
