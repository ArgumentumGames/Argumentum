# Issue: [P1] Poker card dimensions incorrect - 58mm width instead of 63.5mm

## Problem

The poker-sized cards (Scenarii) are generated with incorrect dimensions:
- **Current**: 685×1051 px @ 300 DPI = **58×89 mm**
- **Expected**: 63.5×88.9 mm (standard poker size)
- **Gap**: 5.5mm missing in width

## Root Cause

The Scenarii template uses `"csize": "poker"` in CardPen, but the "poker" format definition appears to be incorrect or using different dimensions than the standard.

## Template Configuration

File: `Cards/Scenarii/Argumentum_Scenarii_Face_fr.json`
```json
{
  "csize": "poker",
  "cori": "portrait",
  "blsize": 5,
  "blunit": "mm",
  "dpi": 300
}
```

## Expected Dimensions

Standard poker card: 2.5×3.5 inches = 63.5×88.9 mm
With 5mm bleed:
- Width: 63.5 + 2×5 = 73.5 mm → 867 px @ 300 DPI
- Height: 88.9 + 2×5 = 98.9 mm → 1167 px @ 300 DPI

## Acceptance Criteria

- [ ] Investigate CardPen "poker" size definition
- [ ] Verify if issue is in CardPen or template configuration
- [ ] Generate poker cards with correct 63.5×88.9 mm dimensions
- [ ] Validate generated images match expected dimensions

## Related

- Commit: `d6bb4665` (DPI standardization to 300)
- Branch: `fix/recovery-october-2025`
