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
- But these classes are based on card index, not game variant

## Root Cause

The template CSS was designed for the base game rules split across multiple cards. The new game variants are separate rows in the CSV, but the template doesn't have:
1. Differentiation by game variant
2. Custom styling per variant
3. Visual distinction between games

## Proposed Solution

Options:
1. **Add CSS classes per game variant** - Requires adding a field to identify the game
2. **Create separate templates per variant** - More work but cleaner separation
3. **Use conditional CSS** based on content markers like `## Title`

## Acceptance Criteria

- [ ] Analyze new rules content structure
- [ ] Design CSS styling approach
- [ ] Implement CSS for new game variants
- [ ] Validate visual rendering of all 6 variants
- [ ] Test with Print&Play output

## Related

- File: `Cards/Rules/Argumentum Rules - Cards.csv` (new multi-variant file)
- File: `Cards/Rules/Argumentum Rules - Cards.old.csv` (original single-variant backup)
- Template: `Cards/Rules/Argumentum_Rules_fr.json`
