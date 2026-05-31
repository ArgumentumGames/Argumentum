# Publication Argumentum — Documentation

> **Audience.** Mainteneurs, contributeurs, partenaires impression/distribution.
> **Objet.** Décrire ce qui est publié, sous quelle forme et pour quel canal.

## Statut bilingue

| Langue | Statut |
|---|---|
| Français (`*.fr.md`) | ✅ Canonique (cycle 2026‑05) |
| English (`*.en.md`) | ✅ Miroir |
| Autres langues (ru/pt/es/ar/fa/zh) | ⏳ TODO — voir [Argumentum CLAUDE.md §8‑Language Extension](../../CLAUDE.md) |

Toute mise à jour matérielle d'un document FR doit être répliquée dans le miroir EN dans le même PR. Les langues hors FR/EN sont gérées séparément via les pipelines de traduction (`DatasetUpdater`).

## Inventaire des documents

### 1. Catalogue des cartes et formats

- **[cards-catalog.fr.md](cards-catalog.fr.md)** — inventaire exhaustif des `CardSetDocuments` (Tarot, Poker, Print&Play, Web A0/A4/Thumbnails), dimensions physiques, recommandations par audience.
- **[cards-catalog.en.md](cards-catalog.en.md)** — English mirror.

### 2. Règles du jeu

Les règles (« livret ») sont livrées sous forme de **cartes Rules** dans le deck Tarot (et le Print&Play A4) — il n'existe pas de document séparé. Voir la section « Règles du jeu » de [cards-catalog.fr.md](cards-catalog.fr.md).

## Conventions

- Les documents prennent le code de langue ISO en suffixe : `*.fr.md`, `*.en.md`.
- Les dimensions sont exprimées en **millimètres** (cohérent avec `WebBasedGeneratorConfig.cs`).
- Les noms de cartes (`Tarot`, `Poker`, `Print&Play`, `Web A0`, `Thumbnails`) reprennent les `DocumentName` de la configuration C# — source de vérité unique.
- Pour modifier les dimensions, formats ou ajouter un livrable, **éditer le code C#** d'abord (`AssetConverterConfig.cs` ou `WebBasedGeneratorConfig.cs`), jamais le JSON généré.

## Ressources liées

- [CLAUDE.md](../../CLAUDE.md) — instructions pipeline et historique de récupération.
- [ARCHITECTURE_PIPELINE.md](../../Generation/Documentation/ARCHITECTURE_PIPELINE.md) — pipeline technique détaillé.
- [README.md](../../README.md) — vue d'ensemble du dépôt.
