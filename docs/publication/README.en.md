# Argumentum Publication — Documentation

> **Audience.** Maintainers, contributors, printing/distribution partners.
> **Purpose.** Describe what is published, in what form and for which channel.

## Bilingual status

| Language | Status |
|---|---|
| French (`*.fr.md`) | ✅ Canonical (cycle 2026‑05) |
| English (`*.en.md`) | ✅ Mirror |
| Other languages (ru/pt/es/ar/fa/zh) | ⏳ TODO — see [Argumentum CLAUDE.md §8‑Language Extension](../../CLAUDE.md) |

Any substantive update to an FR document must be mirrored in the EN counterpart within the same PR. Non‑FR/EN languages are handled separately via the translation pipelines (`DatasetUpdater`).

## Document inventory

### 1. Card catalog and formats

- **[cards-catalog.en.md](cards-catalog.en.md)** — exhaustive inventory of `CardSetDocuments` (Tarot, Poker, Print&Play, Web A0/A4/Thumbnails), physical dimensions, recommendations by audience.
- **[cards-catalog.fr.md](cards-catalog.fr.md)** — French source.

### 2. Game rules

The rules ("booklet") ship as **Rules cards** in the Tarot deck (and the Print&Play A4) — there is no separate document. See the "Game rules" section of [cards-catalog.en.md](cards-catalog.en.md).

### 3. Release announcement article

- **[news-article-v0.9.0.en.md](news-article-v0.9.0.en.md)** — EN mirror draft of the v0.9.0 release announcement (DNN News5 module); publication gated on #134/#131/#132 (issue #135).
- **[news-article-v0.9.0.fr.md](news-article-v0.9.0.fr.md)** — French canonical.

## Conventions

- Documents take the ISO language code as suffix: `*.fr.md`, `*.en.md`.
- Dimensions are expressed in **millimetres** (consistent with `WebBasedGeneratorConfig.cs`).
- Card names (`Tarot`, `Poker`, `Print&Play`, `Web A0`, `Thumbnails`) mirror the `DocumentName` entries in the C# configuration — single source of truth.
- To modify dimensions, formats or add a deliverable, **edit the C# code** first (`AssetConverterConfig.cs` or `WebBasedGeneratorConfig.cs`), never the generated JSON.

## Related resources

- [CLAUDE.md](../../CLAUDE.md) — pipeline instructions and recovery history.
- [ARCHITECTURE_PIPELINE.md](../../Generation/Documentation/ARCHITECTURE_PIPELINE.md) — detailed technical pipeline.
- [README.md](../../README.md) — repository overview.
