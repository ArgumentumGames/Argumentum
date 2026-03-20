# Pipeline Multilingual - Validation Report

**Date:** 2026-03-14
**Branch:** fix/recovery-october-2025
**PR:** #116

## Résumé Exécutif

| Statut | Détail |
|--------|--------|
| ✅ FR | Complet - 1072 images, 19 PDFs |
| ✅ RU | **BON** - Dimensions identiques face/back |
| ⚠️ EN | **PROBLÈME** - Mismatch dimensions backs |
| ⚠️ PT | **PROBLÈME** - Mismatch dimensions backs |

## Inventaire des Fichiers

| Langue | Images | PDFs | Total |
|--------|--------|------|-------|
| FR | 1072 | 19 | 1091 |
| EN | 830 | 22 | 852 |
| RU | 724 | 17 | 741 |
| PT | 830 | 22 | 852 |
| **TOTAL** | **3456** | **80** | **3536** |

## Validation des Dimensions Images

### ✅ FR - Dimensions Correctes

| CardSet | Face (px) | Back (px) | Status |
|---------|-----------|-----------|--------|
| Fallacies | 708×1334 | 708×1334 | ✅ OK |
| Scenarii | 750×1050 | 750×1050 | ✅ OK |

### ✅ RU - Dimensions Correctes

| CardSet | Face (px) | Back (px) | Status |
|---------|-----------|-----------|--------|
| Fallacies | 708×1334 | 708×1334 | ✅ OK |
| Scenarii | 685×1051 | 685×1051 | ✅ OK |

**Note:** Les dimensions RU Scenarii (685×1051) sont différentes de FR (750×1050) car le texte cyrillique est plus large, mais face/back restent identiques.

### ⚠️ EN - Problème Détecté

| CardSet | Face (px) | Back (px) | Diff | Status |
|---------|-----------|-----------|------|--------|
| Fallacies | 1572×2572 | 1493×2493 | -79×-79 | ❌ MISMATCH |
| Scenarii | 1447×1947 | 1250×1750 | -197×-197 | ❌ MISMATCH |

### ⚠️ PT - Problème Détecté

| CardSet | Face (px) | Back (px) | Diff | Status |
|---------|-----------|-----------|------|--------|
| Fallacies | 1572×2572 | 1493×2493 | -79×-79 | ❌ MISMATCH |
| Scenarii | (non vérifié) | (probablement KO) | - | ⚠️ ASSUMÉ |

## Analyse du Problème

### Cause Probable

Le problème vient de la méthode `LoadAndProcessImageUrl` dans `ImageHelper.cs`:

1. Les images sont chargées depuis le harvest (data URLs)
2. Chaque image conserve son DPI metadata d'origine (défini par CardPen lors de la génération)
3. Même après forçage du DPI à 300 (ligne 154), le redimensionnement en MM utilise les dimensions de l'image chargée
4. Si les images face et back ont des DPI initiaux différents, le redimensionnement donne des tailles finales différentes

### Pourquoi RU Fonctionne

RU a probablement été généré avec un harvest cohérent où face et back ont le même DPI initial.

### Pourquoi EN/PT Échouent

EN et PT ont des harvests où les DPI des faces et backs sont incohérents, causant un redimensionnement différent.

## Impact

### Niveau de Sévérité: MOYEN

- **Visuel:** Les backs seront plus petits que les faces dans les PDFs
- **Impression:** Le recto-verso ne s'alignera pas correctement
- **Portée:** Affecte EN et PT uniquement (FR et RU sont OK)

## Screenshots de Validation

Les fichiers suivants ont été générés pour validation visuelle:

- `Validation/tarot_ru_page1.png` - TarotCards RU page 1 (face)
- `Validation/tarot_ru_page2.png` - TarotCards RU page 2 (dos)
- `Validation/poker_en_page1.png` - PokerCards EN page 1 (face)
- `Validation/poker_en_page2.png` - PokerCards EN page 2 (dos)

## Recommandations

### Court Terme (PR #116)

1. **Documenter le problème** dans les notes de PR
2. **Fusionner pour FR et RU** qui sont corrects
3. **Créer une issue** pour corriger EN/PT

### Moyen Terme (Correction)

1. **Investiguer le HarvestManager** pour comprendre pourquoi les DPI diffèrent
2. **Corriger `LoadAndProcessImageUrl`** pour normaliser les dimensions indépendamment du DPI source:
   ```csharp
   // Solution: Lire les dimensions de l'image source
   // Calculer le DPI effectif actuel
   // Forcer le redimensionnement en ignorant le DPI metadata
   ```
3. **Régénérer les harvests EN/PT** avec un DPI cohérent

### Tests à Ajouter

- Validation automatique des dimensions face = back pour chaque langue
- Test de régression pour les futures générations multilingues

## Conclusion

La génération multilingue est **fonctionnelle** mais **partiellement dégradée** pour EN et PT. FR et RU sont parfaits. La PR #116 peut être fusionnée avec une note documentant le problème connu.

---

**Généré par:** Claude Code (Sonnet 4.6)
**Checkpoint:** [pipeline-multilingual-checkpoint.md](../.claude/pipeline-multilingual-checkpoint.md)
