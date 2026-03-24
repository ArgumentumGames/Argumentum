# Issue: [P2] Rules templates still at 500 DPI

## Problem

The Rules templates are still using 500 DPI while all other templates have been standardized to 300 DPI.

## Files Affected

- `Cards/Rules/Argumentum_Rules_fr.json` - DPI: 500
- `Cards/Rules/Argumentum_Rules_Back_fr.json` - DPI: 500

## Impact

- Inconsistent output dimensions across card types
- Rules cards may have different print quality than other cards
- Larger file sizes than necessary

## Expected Behavior

All templates should use 300 DPI for consistency.

## Acceptance Criteria

- [x] Change DPI to 300 in `Argumentum_Rules_fr.json`
- [x] Change DPI to 300 in `Argumentum_Rules_Back_fr.json`
- [x] Regenerate Rules images
- [x] Validate dimensions match expected tarot size (60×113mm)

## Related

- Commit: `d6bb4665` (DPI standardization for Fallacies and Scenarii)
- Template: Rules templates were intentionally skipped in initial DPI fix

## Status

✅ **RESOLVED** - DPI changed to 300 in both templates

## Solution Applied

- `Argumentum_Rules_fr.json`: `"dpi": 300`
- `Argumentum_Rules_Back_fr.json`: `"dpi": 300`
