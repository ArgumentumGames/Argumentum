# Plan po-2023 — SVG Export + Rules Re-harvest (2026-03-24)

## Context

3 commits pushed on `fix/recovery-october-2025`:
- `46d6cd9b` — FreeMind GUI automation for SVG export (working, validated FR)
- `5830cba9` — Fix ProcessSvgFilesAsync disambiguation (.First() bug)
- `5f60e908` — Restore Rules CSV (24 cards matching CSS color scheme)

## Task 1: Re-harvest Rules cards (PRIORITY)

The Rules CSV was restored from backup (24 records aligned with CSS color scheme).
The card images need to be re-generated.

### Steps
1. `git pull origin fix/recovery-october-2025`
2. Edit `AssetConverterConfig.cs`:
   ```csharp
   public ConverterMode Mode { get; set; } = ConverterMode.WebBasedImageGeneration | ConverterMode.QuestPdfGeneration;
   ```
   Keep `Enabled = true` for Localization (multilingual).
3. In `WebBasedGeneratorConfig.cs`, temporarily disable all CardSets EXCEPT Rules:
   - Set `Enabled = false` on Fallacies, Virtues, Scenarii, Memo, FallaciesWeb, etc.
   - Keep `Enabled = true` only on `KnownCardSets.Rules`
4. Run: `dotnet run -- --non-interactive`
5. Verify the generated images in `Target/fr/Harvest/` — should be 24 Rules card images
6. Check colors: cards 1-6 purple, 7-9 orange, 10-14 blue, 15-19 green, 20-24 violet
7. Re-enable all CardSets, regenerate PDFs

### Page break issues to check
After re-harvest, verify these specific problems in the PDF:
- **Le Dernier Beau Parleur**: "Déroulé de la manche" should start on its own card
- **Le Moulin à Baratin**: Installation on first page, Fin de partie on same card as Déroulé
- **La Parlote Coinchée**: Page break before Installation, page break before Tours de jeu

If pagination is wrong, the Markdown content in the CSV needs to be redistributed across cards.
Each CSV row = one card face. To move content to a new card, split the Markdown `Text` field.

## Task 2: FreeMind SVG export (multilingual)

### Prerequisites
- **Interactive session required** (RDP unlocked, screen visible)
- **Do not touch keyboard/mouse** during export (~30s per .mm file)
- FreeMind 1.0.1 at `C:\Program Files (x86)\FreeMind\FreeMind.exe`

### Steps
1. Edit `AssetConverterConfig.cs`:
   ```csharp
   public ConverterMode Mode { get; set; } = ConverterMode.Mindmapper;
   ```
   Keep `Localization.Enabled = true`
2. Clean recovery files: `del C:\Users\MYIA\.freemind\FM_*.mm`
3. Run: `dotnet run -- --non-interactive`
4. **DO NOT TOUCH keyboard/mouse** — FreeMind will open repeatedly
5. Expected: 3 .mm files × (FR + EN + RU + PT languages) = up to 12 sequential FreeMind exports
6. Each export takes ~30s. Total ~6-8 minutes.

### Known issues
- `Argumentum_Fallacies_MindMap_cards_fr.mm` (1.2MB with thumbnails): may take longer to load
- If FreeMind hangs: kill javaw, delete `C:\Users\MYIA\.freemind\FM_*.mm`, rerun
- SendKeys requires foreground focus — don't click anywhere during export

### Keystroke sequence (for reference)
`Alt+F` → 8×DOWN → RIGHT → 12×DOWN → ENTER (SVG) → ENTER (save) → ENTER (overwrite)

## Task 3: Full pipeline validation

After Tasks 1 and 2:
1. Set Mode to full: `WebBasedImageGeneration | QuestPdfGeneration | Mindmapper`
2. Run full pipeline
3. Verify:
   - Rules cards: correct colors, correct page breaks
   - All PDFs: TarotCards, PokerCards, Print&Play
   - SVG mind maps: Fallacies + Virtues × 4 languages
   - OWL ontology (if OwlGenerator in Mode)
