# Argumentum Card Catalog — Publishable Formats

> **Source of truth.** [`WebBasedGeneratorConfig.cs`](../../Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGeneratorConfig.cs) — `CardSetDocuments` section.
> **Inventory date.** 2026‑05‑30 (branch `master`, commit `1811afc4`).

Argumentum is a **dual‑deck** game: a large‑card deck (fallacies/virtues) and a small‑card deck (scenarios). The repository's generation produces **eight PDF deliverables** enabled by default, from the same CSV sources (`Cards/Fallacies/`, `Cards/Scenarii/`, `Cards/Rules/`).

All formats are localized in **8 languages**: `fr` (canonical) + `en`, `ru`, `pt`, `es`, `ar`, `fa`, `zh` (translations).

## Summary table

| Deliverable | Cards included | Dimensions (mm) | CMYK? | Page | Target audience |
|---|---|---|---|---|---|
| **Professional Tarot** (`Argumentum_TarotCards_fr.pdf`) | Rules + Memo×7 + Fallacies | 60×113 | ✅ | Tarot trim | Pro print / publisher |
| **Virtues Tarot** (`Argumentum_TarotCards_Virtues_fr.pdf`) | Virtues | 60×113 | ✅ | Tarot trim | Pro print / publisher |
| **Scenarios Poker** (`Argumentum_PokerCards_fr.pdf`) | Scenarii | 63.5×88.9 (2.5″×3.5″ standard) | ✅ | Poker trim | Pro print / publisher |
| **Print&Play Tarot A4** (`Argumentum_TarotCards_Print&Play_A4_fr.pdf`) | Rules + Fallacies + Virtues + Memo×5 | 60×113 | RGB | A4 | Player, home printer |
| **Print&Play Poker A4** (`Argumentum_PokerCards_Print&Play_A4_fr.pdf`) | Scenarii | 63.5×88.9 | RGB | A4 | Player, home printer |
| **Web A4 — Fallacies** (`Argumentum_Fallacies_Web_A4_fr.pdf`) | FallaciesWeb (no back) | 66×66 | RGB | A4 | Discovery / pedagogy |
| **A0 Poster — Fallacies** (`Argumentum_Fallacies_Web_A0_fr.pdf`) | FallaciesWeb (no back) | 69×69 — 12 columns — header logo+QR | ✅ | A0 (841×1189) | Wall display / education |
| **A4 Thumbnails** (`Argumentum_Fallacies_Web_Thumbnails_A4_fr.pdf`) | FallaciesWebThumbnails | 50×50 (front) / 72×72 (back) | RGB | A4 | Visual index / reference |

> Two additional deliverables (`TarotCards_2`, `TarotCards_3`) exist in code but are **disabled** (`Enabled = false`). They correspond to future taxonomy extensions (`Fallacies2`, `Fallacies3`).

## Per‑deliverable details

### 1. Professional Tarot — `Argumentum_TarotCards_fr.pdf`

- **Cards**: Rules (24) + Memo (×7 copies) + Fallacies (full taxonomy).
- **Dimensions**: 60×113 mm (standard Tarot trim, no bleed).
- **Color profile**: CMYK (printer proof).
- **FR volume**: ~177 Fallacies + 24 Rules + ~7 Memo ≈ **208 cards**.
- **Use**: hand off to a professional printer for Tarot deck production.

### 2. Virtues Tarot — `Argumentum_TarotCards_Virtues_fr.pdf`

- **Cards**: Virtues (223 records).
- **Dimensions**: 60×113 mm — CMYK.
- **Use**: companion deck dedicated to argumentative virtues.

### 3. Scenarios Poker — `Argumentum_PokerCards_fr.pdf`

- **Cards**: Scenarii (167 records).
- **Dimensions**: 63.5×88.9 mm (standard poker, 2.5″×3.5″) — CMYK.
- **Use**: small scenario deck, traditional poker format.

### 4. Print&Play Tarot A4 — `Argumentum_TarotCards_Print&Play_A4_fr.pdf`

- **Cards**: Rules (P&P) + Fallacies (P&P) + Virtues + Memo (×5 P&P).
- **Card dimensions**: 60×113 mm, **RGB** with no CMYK conversion.
- **Page**: A4.
- **Use**: print at home (laser/inkjet), cut, play.

### 5. Print&Play Poker A4 — `Argumentum_PokerCards_Print&Play_A4_fr.pdf`

- **Cards**: Scenarii (P&P).
- **Card dimensions**: 63.5×88.9 mm, RGB.
- **Page**: A4.

### 6. Web A4 — Fallacies — `Argumentum_Fallacies_Web_A4_fr.pdf`

- **Cards**: FallaciesWeb (web variant, no back).
- **Dimensions**: 66×66 mm, RGB, no back (`NoBack = true`).
- **Use**: pedagogical printable on A4, square format, for handing out in class or training.

### 7. A0 Poster — Fallacies — `Argumentum_Fallacies_Web_A0_fr.pdf`

- **Cards**: FallaciesWeb (poster‑assembled).
- **Dimensions**: 69×69 mm — **12 columns** — CMYK — no back — header `Logo_Argumentum & QRCode.png` — 2 mm padding.
- **Page**: A0 (841×1189 mm — single page).
- **Use**: display poster (classroom, event, exhibition). The header QR code links to the website.

### 8. A4 Thumbnails — `Argumentum_Fallacies_Web_Thumbnails_A4_fr.pdf`

- **Cards**: FallaciesWebThumbnails (mini thumbnails).
- **Dimensions**: 50×50 mm (front), 72×72 mm (back), RGB, back not published.
- **Use**: compact visual index, reference sheet for a binder.

## Recommendation by audience and channel

| Audience | Channel | Recommended format | Reason |
|---|---|---|---|
| Curious player | Web / download | **Print&Play Tarot A4** + **Print&Play Poker A4** | Home‑printable, RGB, standard A4 |
| Teacher / trainer | Classroom | **Web A4 Fallacies** + **A4 Thumbnails** | Pedagogical, hand‑outable, no back |
| Educational institution | Wall / room | **A0 Poster Fallacies** | Persistent reference, QR to resources |
| Publisher / commercial partner | Industry | **Tarot** + **Virtues Tarot** + **Scenarios Poker** | CMYK, pro print dimensions |
| Translation contributor | Reference | **A4 Thumbnails** | Overview for proofreading |
| Library / non‑profit | Loan | **Print&Play Tarot A4** + **Poker A4** assembled and laminated | Reproducible at marginal cost |

## Additional versioned artifacts

Beyond the generated PDFs, the repository versions several directly publishable artifacts:

### Mind maps (4 languages × 5 files)

- `Cards/Fallacies/Mindmaps/{fr,en,ru,pt}/Fallacies_*.svg` (3 views: `.svg`, `.content.svg`, `.links.svg`)
- `Cards/Fallacies/Mindmaps/{fr,en,ru,pt}/Argumentum_Virtues_MindMap_*.{content,links}.svg`
- FreeMind source: `Cards/Fallacies/Mindmaps/fallacy_map.mm`
- Interactive HTML wrappers: `Cards/Fallacies/Mindmaps/*/Fallacies_*.html`

### OWL ontology

- Target: OWL ontology with SKOS annotations (cf. [CLAUDE.md §Mind Maps & SVGs](../../CLAUDE.md)).

### Box packaging

- `Cards/Packaging/FCPM_065 - CLOCHE - 121x126x26mm.svg` — physical box template (lid).
- `Cards/Packaging/FCPM_065 - FOND - 117x122x28mm.svg` — physical box template (base).

## How to regenerate

See [README.md §Generating Cards images and documents](../../README.md) for the full commands. Summary:

```bash
dotnet run --project "Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj"
```

Generated PDFs appear in `Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0/Target/{lang}/Documents/`.

> **Note.** The pipeline skips existing files — to regenerate a specific deliverable, delete the target PDF.

## How to add or modify a format

1. **Edit** `WebBasedGeneratorConfig.cs` (never the generated JSON).
2. **Add** a `CardSetDocumentConfig` entry to the `CardSetDocuments` list.
3. **Test** generation on a subset.
4. **Update** this catalog (`cards-catalog.fr.md` + `cards-catalog.en.md` simultaneously).
5. **PR** with a screenshot of a representative page.
