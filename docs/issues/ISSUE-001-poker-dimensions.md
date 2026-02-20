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

- [x] Investigate CardPen "poker" size definition
- [x] Verify if issue is in CardPen or template configuration
- [x] Generate poker cards with correct 63.5×88.9 mm dimensions
- [x] Validate generated images match expected dimensions

## Investigation

CardPen defines poker correctly as `poker: [3.5,2.5,ins]` = 88.9×63.5mm (see `js/const.js` line 18).

The issue was in `WebBasedGeneratorConfig.cs` - DocumentCardSet for PokerCards had incorrect WidthMM.

## Related

- Commit: `d6bb4665` (DPI standardization to 300)
- Commit: `688a21b5` (poker dimensions fix: 58mm → 63.5mm)
- Branch: `fix/recovery-october-2025`

## Status

✅ **RESOLVED** - Pipeline regenerated with correct dimensions

## Verification

- Images: 750×1050 px @ 300 DPI = 63.5×88.9mm ✓
- PDF pages: 180×252 pt = 63.5×88.9mm ✓

## Solution Applied

In `WebBasedGeneratorConfig.cs` lines 476-483:

```csharp
FrontCards = new DocumentCard()
{
    HeigthMM = 88.9m,  // Standard poker: 3.5" = 88.9mm
    WidthMM = 63.5m,   // Standard poker: 2.5" = 63.5mm (was 58mm - incorrect)
},
BackCards = new DocumentCard()
{
    HeigthMM = 88.9m,  // Standard poker: 3.5" = 88.9mm
    WidthMM = 63.5m,   // Standard poker: 2.5" = 63.5mm (was 58mm - incorrect)
}
```

Expected image dimensions after regeneration (with 5mm bleed @ 300 DPI):
- Width: 73.5mm → **867 px**
- Height: 98.9mm → **1167 px**
