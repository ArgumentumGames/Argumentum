# Catalogue des cartes Argumentum — formats publiables

> **Source de vérité.** [`WebBasedGeneratorConfig.cs`](../../Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGeneratorConfig.cs) — section `CardSetDocuments`.
> **Date de l'inventaire.** 2026‑05‑30 (branche `master`, commit `1811afc4`).

Argumentum est un jeu **à double deck** : un deck de grandes cartes (sophismes/vertus) et un deck de petites cartes (scénarios). La génération du dépôt produit **huit livrables PDF** activés par défaut, à partir des mêmes sources CSV (`Cards/Fallacies/`, `Cards/Scenarii/`, `Cards/Rules/`).

Tous les formats sont déclinés en **8 langues** : `fr` (canonique) + `en`, `ru`, `pt`, `es`, `ar`, `fa`, `zh` (traductions).

## Tableau récapitulatif

| Livrable | Cartes incluses | Dimensions (mm) | CMYK ? | Page | Public cible |
|---|---|---|---|---|---|
| **Tarot professionnel** (`Argumentum_TarotCards_fr.pdf`) | Rules + Memo×7 + Fallacies | 60×113 | ✅ | Découpe Tarot | Print pro / éditeur |
| **Tarot Vertus** (`Argumentum_TarotCards_Virtues_fr.pdf`) | Virtues | 60×113 | ✅ | Découpe Tarot | Print pro / éditeur |
| **Poker scénarios** (`Argumentum_PokerCards_fr.pdf`) | Scenarii | 63.5×88.9 (2.5″×3.5″ standard) | ✅ | Découpe Poker | Print pro / éditeur |
| **Print&Play Tarot A4** (`Argumentum_TarotCards_Print&Play_A4_fr.pdf`) | Rules + Fallacies + Virtues + Memo×5 | 60×113 | RGB | A4 | Joueur, imprimante perso |
| **Print&Play Poker A4** (`Argumentum_PokerCards_Print&Play_A4_fr.pdf`) | Scenarii | 63.5×88.9 | RGB | A4 | Joueur, imprimante perso |
| **Web A4 — Fallacies** (`Argumentum_Fallacies_Web_A4_fr.pdf`) | FallaciesWeb (sans dos) | 66×66 | RGB | A4 | Découverte / pédagogie |
| **Poster A0 — Fallacies** (`Argumentum_Fallacies_Web_A0_fr.pdf`) | FallaciesWeb (sans dos) | 69×69 — 12 colonnes — entête logo+QR | ✅ | A0 (841×1189) | Affichage mural / éducation |
| **Vignettes A4** (`Argumentum_Fallacies_Web_Thumbnails_A4_fr.pdf`) | FallaciesWebThumbnails | 50×50 (face) / 72×72 (dos) | RGB | A4 | Index visuel / référence |

> Deux livrables additionnels (`TarotCards_2`, `TarotCards_3`) existent dans le code mais sont **désactivés** (`Enabled = false`). Ils correspondent à des extensions futures de la taxonomie (`Fallacies2`, `Fallacies3`).

## Détails par livrable

### 1. Tarot professionnel — `Argumentum_TarotCards_fr.pdf`

- **Cartes** : Rules (24) + Memo (×7 copies) + Fallacies (taxonomie complète).
- **Dimensions** : 60×113 mm (format tarot standard, sans marge de découpe).
- **Profil couleur** : CMYK (épreuve imprimeur).
- **Volume FR** : ~177 cartes Fallacies + 24 Rules + ~7 Memo ≈ **208 cartes**.
- **Usage** : remise à un imprimeur professionnel pour fabrication d'un deck Tarot.

### 2. Tarot Vertus — `Argumentum_TarotCards_Virtues_fr.pdf`

- **Cartes** : Virtues (223 records).
- **Dimensions** : 60×113 mm — CMYK.
- **Usage** : deck complémentaire dédié aux vertus argumentatives.

### 3. Poker scénarios — `Argumentum_PokerCards_fr.pdf`

- **Cartes** : Scenarii (167 records).
- **Dimensions** : 63.5×88.9 mm (standard poker, 2.5″×3.5″) — CMYK.
- **Usage** : petit deck de scénarios, format poker traditionnel.

### 4. Print&Play Tarot A4 — `Argumentum_TarotCards_Print&Play_A4_fr.pdf`

- **Cartes** : Rules (P&P) + Fallacies (P&P) + Virtues + Memo (×5 P&P).
- **Dimensions cartes** : 60×113 mm, **RGB** sans conversion CMYK.
- **Page** : A4.
- **Usage** : imprimer chez soi (laser/jet d'encre), découper et jouer.

### 5. Print&Play Poker A4 — `Argumentum_PokerCards_Print&Play_A4_fr.pdf`

- **Cartes** : Scenarii (P&P).
- **Dimensions cartes** : 63.5×88.9 mm, RGB.
- **Page** : A4.

### 6. Web A4 — Fallacies — `Argumentum_Fallacies_Web_A4_fr.pdf`

- **Cartes** : FallaciesWeb (variante web sans dos).
- **Dimensions** : 66×66 mm, RGB, sans dos (`NoBack = true`).
- **Usage** : support pédagogique imprimable sur A4, format carré, à distribuer en classe ou en formation.

### 7. Poster A0 — Fallacies — `Argumentum_Fallacies_Web_A0_fr.pdf`

- **Cartes** : FallaciesWeb (poster monté).
- **Dimensions** : 69×69 mm — **12 colonnes** — CMYK — sans dos — entête `Logo_Argumentum & QRCode.png` — padding 2 mm.
- **Page** : A0 (841×1189 mm — 1 page).
- **Usage** : poster d'affichage (salle de classe, événement, exposition). Le QR code en entête redirige vers le site.

### 8. Vignettes A4 — `Argumentum_Fallacies_Web_Thumbnails_A4_fr.pdf`

- **Cartes** : FallaciesWebThumbnails (mini‑vignettes).
- **Dimensions** : 50×50 mm (face), 72×72 mm (dos), RGB, sans dos publié.
- **Usage** : index visuel compact, fiche de référence à glisser dans un classeur.

## Recommandation par audience et canal

| Audience | Canal | Format recommandé | Raison |
|---|---|---|---|
| Joueur curieux | Web / téléchargement | **Print&Play Tarot A4** + **Print&Play Poker A4** | Imprimable maison, RGB, A4 standard |
| Enseignant / formateur | Salle | **Web A4 Fallacies** + **Vignettes A4** | Pédagogique, distribuable, sans dos |
| Établissement scolaire | Mur / salle | **Poster A0 Fallacies** | Référence permanente, QR vers ressources |
| Éditeur / partenaire commercial | Industrie | **Tarot** + **Tarot Vertus** + **Poker scénarios** | CMYK, dimensions print pro |
| Contributeur traduction | Référence | **Vignettes A4** | Vue d'ensemble pour relecture |
| Bibliothèque associative | Prêt | **Print&Play Tarot A4** + **Poker A4** assemblés et plastifiés | Reproductible à coût marginal |

## Artefacts complémentaires versionnés

Au‑delà des PDF générés, le dépôt versionne plusieurs artefacts directement publiables :

### Mind maps (4 langues × 5 fichiers)

- `Cards/Fallacies/Mindmaps/{fr,en,ru,pt}/Fallacies_*.svg` (3 vues : `.svg`, `.content.svg`, `.links.svg`)
- `Cards/Fallacies/Mindmaps/{fr,en,ru,pt}/Argumentum_Virtues_MindMap_*.{content,links}.svg`
- Source FreeMind : `Cards/Fallacies/Mindmaps/fallacy_map.mm`
- Wrappers HTML interactifs : `Cards/Fallacies/Mindmaps/*/Fallacies_*.html`

### Ontologie OWL

- Cible : ontologie OWL avec annotations SKOS (cf. [CLAUDE.md §Mind Maps & SVGs](../../CLAUDE.md)).

### Packaging boîte

- `Cards/Packaging/FCPM_065 - CLOCHE - 121x126x26mm.svg` — gabarit physique boîte (cloche).
- `Cards/Packaging/FCPM_065 - FOND - 117x122x28mm.svg` — gabarit physique boîte (fond).

## Comment régénérer

Voir [README.md §Generating Cards images and documents](../../README.md) pour les commandes complètes. Résumé :

```bash
dotnet run --project "Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj"
```

Les PDF générés apparaissent dans `Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0/Target/{lang}/Documents/`.

> **Note.** Le pipeline saute les fichiers existants — pour régénérer un livrable précis, supprimer le PDF cible.

## Pour ajouter ou modifier un format

1. **Éditer** `WebBasedGeneratorConfig.cs` (jamais le JSON généré).
2. **Ajouter** une entrée `CardSetDocumentConfig` dans la liste `CardSetDocuments`.
3. **Tester** la génération sur un sous‑ensemble.
4. **Mettre à jour** ce catalogue (`cards-catalog.fr.md` + `cards-catalog.en.md` simultanément).
5. **PR** avec capture d'écran d'une page représentative.
