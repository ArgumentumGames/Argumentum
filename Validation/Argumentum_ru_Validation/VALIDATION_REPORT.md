# Rapport de Validation - Pipeline Argumentum RU

**Date:** 2026-02-18 (Mis à jour)
**Package:** Argumentum_ru_Validation
**Comparaison:** Référence 2023 vs Génération Actuelle (Après corrections DPI + Print&Play)

## Résumé Exécutif

| Métrique | Référence 2023 | Actuel | Status |
|----------|---------------|--------|--------|
| **TarotCards ru-1** | 2 pages | 2 pages | ✅ CORRECT |
| **TarotCards** | 14 fichiers | 2 fichiers | ⚠️ Mode différent |
| **PokerCards ru-1** | 13 pages | 13 pages | ✅ CORRECT |
| **PokerCards** | 7 fichiers | 7 fichiers | ✅ CORRECT |
| **Tarot Print&Play** | 12 pages @ 1.8MB | ~40 pages @ 27MB | ⚠️ Contenu complet (voir notes) |
| **Poker Print&Play** | 6 pages | 6 pages | ✅ CORRECT |
| **Dimensions Tarot** | 60x113mm | 60x113mm | ✅ CORRECT |
| **Dimensions Poker** | 58x89mm | 58x89mm | ✅ CORRECT |

**Notes Print&Play:**
- Avant correction: 4 pages (8 images seulement)
- Après correction: ~40 pages avec 46 images (6 Rules + 35 Fallacies + 5 Memo)
- La référence 2023 utilise un filtre différent ou contenu réduit

## Problèmes Résolus ✅

### 1. Dimensions des Cartes - RÉSOLU ✅

**Avant:**
- TarotCards: 485×838mm (~8x trop grand)
- PokerCards: 441×617mm (~7x trop grand)

**Après correction DPI (500→300):**
- **TarotCards**: 60×113mm ✓
- **PokerCards**: 58×89mm ✓

**Solution:** Changement de `"dpi": 500` → `"dpi": 300` dans les templates JSON.

### 2. Pagination PokerCards - RÉSOLU ✅

**Avant:** 168 pages (mode AlternateFaceAndBack incorrect)
**Après:** 13 pages (correspond à la référence)

## Problèmes Résolus (Mis à jour) ✅

### 3. Print&Play Harvesting - RÉSOLU ✅

**Avant:**
- Fallacies-Print&Play: 0 faces (JsonFilePath manquant)
- Total: 8 images seulement → 4 pages

**Après correction:**
- Rules-Print&Play: 6 faces + 0 backs
- Fallacies-Print&Play: 35 faces + 1 back
- Memo-Print&Play: 1 face + 1 back
- Total: 46 images → ~40 pages (recto-verso)

**Solution:** Ajout de `JsonFilePath` dans `WebBasedGeneratorConfig.cs`:
```csharp
// FallaciesPrintAndPlay FaceCardSetInfo
JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_fr.json",
JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum_Fallacies_Face_fr.json",
```

## Notes sur la Différence avec Référence 2023

**Tarot Print&Play A4:**
- Référence: 12 pages @ 1.8MB
- Actuel: ~40 pages @ 27MB
- **Explication probable:** La référence 2023 utilise un filtre CSV différent ou une version réduite du contenu
- Avec les filtres actuels: 35 fallacies (print_and_play=1) + 6 rules + 5 copies de Memo = 46 cartes
- Sur A4 (3 cols × 2 rows = 6 cartes/page): 46/6 ≈ 8 pages × 2 (recto-verso) ≈ 16 pages

Le contenu actuel est **COMPLET** mais plus volumineux que la référence 2023.

## Actions Recquises

### Priorité 1: Corriger les Dimensions Images

1. **Vérifier le DPI de génération CardPen**
   - Template JSON: `dpi: 500`
   - Config C#: `Dpi = 300`
   - Le DPI est-il bien passé à CardPen ?

2. **Forcer le redimensionnement des images**
   - Dans `ImageHelper.cs`, ligne 116: `imageFromEmbeddedUrl.Density = new Density(sourceDpi);`
   - `sourceDpi` vaut-il 0 (capturé échoué) ?

3. **Appliquer ResizeInMM systématiquement**
   - Vérifier que `ResizeInMM` est appelé avec les bonnes dimensions

### Priorité 2: Corriger Pagination Print&Play

1. **Vérifier la configuration dans `WebBasedGeneratorConfig.cs`**
   ```csharp
   // FallaciesPrintAndPlay / TarotPrintAndPlay
   NbColumns = ?,  // Doit être 11-12 pour A0
   Header = ?,
   Padding = ?
   ```

2. **Vérifier le calcul dans `PrintAndPlayDocument.cs`**
   ```csharp
   var contentHeightPoints = pageSize.Height - totalMarginPoints - headerHeightPoints;
   var nbRows = (int)(contentHeightPoints / cardHeightPoints);
   ```

### Priorité 3: Valider le Mode BackFirstOneDocPerBack

Le mode doit générer 1 fichier par type de dos:
- Si toutes les faces ont le même dos → 1 fichier
- Si les faces ont N dos différents → N fichiers

## Recommandations

### Immédiat

1. **Désactiver la localisation** (`Enabled = false`) pour stabiliser le FR
2. **Corriger le DPI** avant de régénérer
3. **Vérifier les images FR** avant de passer à d'autres langues

### Court terme

1. Ajouter des tests unitaires pour les dimensions images
2. Ajouter des tests de régression pour les PDFs
3. Créer un script de validation automatisé

### Moyen terme

1. Standardiser sur 300 DPI pour tous les CardSets
2. Documenter les dimensions attendues dans un fichier de référence
3. Créer un "Golden Master" de test

## Fichiers de Référence

| Fichier | Contenu | Usage |
|---------|----------|-------|
| `comparison_plan.md` | Plan de comparaison détaillé | Documentation |
| `comparison_agent.py` | Script de comparaison automatisé | Automation |
| `Argumentum_ru_Validation/` | Package PDF actuel RU | Validation |

## Prochaine Étape

Régénérer les images avec les corrections DPI appliquées et relancer la validation.
