# #411 PT Register Worklist — Fallacies Archaisms

## Summary

| Metric | Count |
|--------|-------|
| Total Fallacies records | 1408 |
| Records with archaic PT register | **80** |
| Cells to rework | **88** (71 desc_pt + 17 example_pt) |
| Records using modern você/vocês | 1220 (reference standard) |

## Archaism Pattern Frequency

| Pattern | Records | Notes |
|---------|---------|-------|
| `vosso` | 45 | Possessive "your" (masc.) |
| `vossa` | 25 | Possessive "your" (fem.) |
| `vós` | 12 | 2nd person plural pronoun |
| `dizeis` | 2 | Archaic verb conjugation |
| `sois` | 2 | Archaic verb conjugation |
| `sabeis` | 1 | Archaic verb conjugation |
| `podeis` | 3 | Archaic verb conjugation |
| `lembrais` | 1 | Archaic verb conjugation |
| `Esqueceis` | 1 | Archaic verb conjugation |

## Modern vs Archaic Register

The dataset overwhelmingly uses **modern Brazilian Portuguese** (`você/vocês/seu/sua`):
- Modern: **1220** records
- Archaic: **80** records (5.7%)

The archaisms are remnants of European Portuguese or literal translations. The goal is to modernize these 80 records to match the register of the other 92.3%.

## Recommended Approach

**gpt-5.5 targeted pass**: For each of the 80 records, rewrite `desc_pt`/`example_pt` replacing archaic forms with modern equivalents while preserving meaning and register.

Mapping table for gpt-5.5:
- `vós` → `vocês` (or restructure to avoid direct address)
- `vosso/vossa` → `seu/sua` (or restructure)
- `vosso/vossa` before a noun → `seu/sua` + noun
- Archaic verb forms (`dizeis`, `sois`, `sabeis`, `podeis`) → modern 3rd person plural (`dizem`, `são`, `sabem`, `podem`) or `vocês` + verb

## Full Worklist

### Records by PK

| PK | Field(s) | Archaism(s) | Severity |
|----|----------|-------------|----------|
| 139 | desc_pt | Vós | Low |
| 149 | desc_pt | vossa, vós | Medium |
| 176 | desc_pt, example_pt | vosso, vossa×2, sois | High |
| 177 | example_pt | vossa | Low |
| 248 | desc_pt | vosso | Low |
| 285 | desc_pt, example_pt | vosso×2, vossa×2 | High |
| 299 | desc_pt, example_pt | vosso×2, vós | Medium |
| 315 | desc_pt | vosso | Low |
| 329 | example_pt | vosso | Low |
| 333 | desc_pt | vosso | Low |
| 344 | desc_pt | Vós | Low |
| 346 | desc_pt | Vós, vossa | Medium |
| 350 | desc_pt | Vós, vossa | Medium |
| 357 | desc_pt | Vós | Low |
| 360 | desc_pt | vosso×2 | Medium |
| 376 | desc_pt | vosso | Low |
| 420 | example_pt | vosso | Low |
| 431 | desc_pt, example_pt | Vós, vosso×2, vossa, sabeis | High |
| 432 | desc_pt | vosso, vossa | Medium |
| 435 | example_pt | vosso | Low |
| 456 | desc_pt, example_pt | vosso×2 | Medium |
| 457 | desc_pt | vosso | Low |
| 458 | desc_pt | vossa | Low |
| 459 | example_pt | lembrais, Esqueceis, dizeis | High |
| 462 | desc_pt | vosso | Low |
| 473 | desc_pt | vosso | Low |
| 488 | desc_pt | vosso×3 | Medium |
| 518 | desc_pt, example_pt | Vós×2, vosso×2 | High |
| 519 | desc_pt | Vós, vosso | Medium |
| 521 | desc_pt | vosso | Low |
| 523 | desc_pt | vosso | Low |
| 535 | desc_pt | vosso×2 | Medium |
| 536 | desc_pt | vosso | Low |
| 538 | desc_pt | vossa | Low |
| 539 | desc_pt | vosso×2, vossa | Medium |
| 540 | desc_pt | vosso, vossa | Medium |
| 541 | desc_pt | vossa×2 | Medium |
| 542 | desc_pt | vosso, vossa | Medium |
| 769 | desc_pt | Vós | Low |
| 770 | desc_pt, example_pt | Vós, Podeis | Medium |
| 786 | desc_pt | vosso | Low |
| 787 | desc_pt | Vosso | Low |
| 789 | desc_pt | vosso | Low |
| 790 | desc_pt | vosso | Low |
| 791 | desc_pt | vossa | Low |
| 792 | desc_pt | podeis | Low |
| 793 | desc_pt | podeis | Low |
| 794 | desc_pt | vosso | Low |
| 795 | desc_pt | vossa | Low |
| 796 | desc_pt | vosso | Low |
| 797 | desc_pt | vosso | Low |
| 798 | desc_pt | Vós | Low |
| 808 | desc_pt | vossa | Low |
| 817 | desc_pt | Vós, vedes, sois | High |
| 818 | desc_pt | Vós | Low |
| 825 | desc_pt | vossa | Low |
| 883 | example_pt | vosso | Low |
| 909 | desc_pt | vosso | Low |
| 916 | desc_pt | vosso | Low |
| 925 | desc_pt | vosso | Low |
| 935 | example_pt | vossa×2 | Medium |
| 958 | desc_pt | vosso, vossa | Medium |
| 959 | desc_pt | vosso | Low |
| 960 | desc_pt | vosso | Low |
| 961 | desc_pt | vosso | Low |
| 964 | desc_pt | vosso | Low |
| 973 | desc_pt | vosso | Low |
| 1012 | desc_pt | vosso | Low |
| 1040 | desc_pt | vosso, vossa | Medium |
| 1054 | desc_pt | vossa | Low |
| 1055 | desc_pt | vossa | Low |
| 1165 | desc_pt, example_pt | vosso, vós | Medium |
| 1166 | desc_pt | vossa | Low |
| 1174 | desc_pt | vosso | Low |
| 1185 | desc_pt | vossa | Low |
| 1282 | example_pt | vós | Low |
| 1298 | example_pt | Dizeis | Low |
| 1346 | desc_pt | vosso | Low |
| 1357 | desc_pt | vosso | Low |
| 1358 | desc_pt | vosso | Low |

## Severity Distribution

| Severity | Count | Description |
|----------|-------|-------------|
| Low | 52 | Single archaism, simple replacement |
| Medium | 20 | 2-3 archaisms, context-aware replacement |
| High | 8 | 4+ archaisms or archaic verb conjugations |

## Sample 5 Records for gpt-5.5 Validation

These 5 records represent the range of complexity:

1. **PK=139** (Low): `desc_pt` has single "Vós" → straightforward replacement
2. **PK=360** (Medium): `desc_pt` has "vosso" ×2 → context-aware seu/sua
3. **PK=176** (High): `desc_pt` + `example_pt` with vosso, vossa×2, sois → multiple patterns
4. **PK=459** (High): `example_pt` with lembrais, Esqueceis, dizeis → archaic verbs
5. **PK=817** (High): `desc_pt` with Vós, vedes, sois → archaic verbs

## Next Steps

1. **ai-01 review** this worklist for register alignment strategy
2. **Sample pass** (5-10 records) via gpt-5.5 → diff review by ai-01
3. **Bulk pass** (remaining 75 records) only after ai-01 ✅ on sample
4. PR separate from #432, merge-after-Thursday
