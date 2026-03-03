# Skill: Pipeline Fix

Corrige un problème spécifique identifié dans un rapport de validation, régénère les médias affectés, et valide jusqu'à résolution.

## Usage

```
/pipeline-fix <problem-description> [validation-report-path]
```

**Exemples:**
- `/pipeline-fix "Virtues CSS argumentsVertueux manquant" Target/fr/validation-report-20260202.md`
- `/pipeline-fix "TarotCards_fr-2.pdf page 12 blanche"`
- `/pipeline-fix "Rules sans styles CSS"`

## Workflow Automatique

### Phase 1: Analyse du Problème

**Entrées:**
- Description du problème (texte ou extrait de rapport)
- Rapport de validation (optionnel)

**Actions:**

1. **Lis le rapport de validation** (si fourni) avec Read
2. **Identifie les paramètres:**
   - CardSet affecté (ex: Virtues, Fallacies, Rules)
   - Type de problème (CSS, dimension, asset, synchronisation)
   - Fichiers source à modifier (template JSON, config C#, CSV)
   - Impact (images, PDFs, ou les deux)

3. **Détermine la stratégie de correction:**

| Type Problème | Fichiers à Modifier | Régénération Requise |
|---------------|---------------------|---------------------|
| CSS manquant | Template JSON CardPen | Images + PDFs |
| Dimension incorrecte | WebBasedGeneratorConfig.cs | Images + PDFs |
| Asset non chargé | Template JSON (URLs) | Images + PDFs |
| Sync dos/faces | PrintAndPlayDocument.cs | PDFs seulement |
| Nombre colonnes PDF | CardSetDocumentConfig | PDFs seulement |
| CSV contenu | Fichier CSV source | Images + PDFs |

### Phase 2: Application de la Correction

**Utilise les outils appropriés:**

1. **Lis les fichiers concernés** avec Read
2. **Applique la correction** avec Edit
3. **Vérifie la syntaxe:**
   - JSON: Valide avec parser
   - C#: `dotnet build` pour vérifier compilation
   - CSV: Pas de validation auto (trop fragile)

4. **Commit la correction** si demandé:
   ```bash
   git add [fichiers]
   git commit -m "fix([scope]): [description]

   [détails du problème]
   [solution appliquée]

   Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>"
   ```

### Phase 3: Régénération Ciblée

**Selon l'impact identifié:**

#### Si Images + PDFs à régénérer

```bash
cd "Generation/Converters/Argumentum.AssetConverter"

# Option 1: Régénération complète (plus sûr)
dotnet run --project Argumentum.AssetConverter.csproj

# Option 2: Régénération partielle (si supportée)
# TODO: Implémenter flag --cardset={name} dans le pipeline
```

**Surveillance:**
- Capture logs pour le CardSet affecté
- Vérifie que le problème ne se reproduit pas
- Note durée de régénération

#### Si PDFs seulement à régénérer

**Modifier temporairement le Mode dans AssetConverterConfig.cs:**

```csharp
// Désactiver harvesting, garder seulement PDF
Mode = OperationMode.QuestPdfGeneration | OperationMode.PdfAuditor
```

Puis:
```bash
dotnet run --project Argumentum.AssetConverter.csproj
```

**IMPORTANT:** Restaurer le Mode après régénération.

### Phase 4: Validation de la Correction

**Lis visuellement les fichiers régénérés:**

1. **Images concernées** (échantillon de 5-10):
   - ✅ Problème corrigé?
   - ✅ Pas de régression visuelle?
   - ✅ Autres cartes non affectées?

2. **PDFs concernés** (lecture complète):
   - ✅ Problème corrigé dans toutes les pages?
   - ✅ Pas de nouvelles pages blanches?
   - ✅ Qualité visuelle maintenue?

3. **Logs de génération:**
   - Grep pour warnings liés au CardSet
   - Pas de nouvelles erreurs?

### Phase 5: Itération si Nécessaire

**Si problème persiste:**

1. **Analyse approfondie:**
   - Lis template JSON complet
   - Lis config C# complet
   - Compare avec Golden Master (commit 0087f0ec)
   - Cherche CLAUDE.md pour leçons apprises similaires

2. **Hypothèse alternative:**
   - Documente pourquoi première correction a échoué
   - Propose nouvelle stratégie
   - Applique correction #2

3. **Limite d'itérations:** 3 tentatives max
   - Si échec après 3 tentatives → Demande intervention humaine
   - Documente tout le contexte pour debug manuel

**Si problème résolu:**

1. **Créer rapport de correction:**

```markdown
# Fix Report: [Problème]

**Date:** 2026-02-02 15:30:00
**CardSet:** Virtues Tarot FR
**Problème:** Classe CSS `argumentsVertueux` manquante

## Diagnostic

- Fichier source: `Generation/CardPen/templates/Argumentum_Virtues_Face_fr.json`
- Cause: Classe CSS non définie dans section `<style>`
- Impact: 113 cartes avec fond blanc au lieu de gris #555555

## Correction Appliquée

**Fichier:** `Generation/CardPen/templates/Argumentum_Virtues_Face_fr.json`

**Changement:**
```diff
+ .argumentsVertueux { background-color: #555555; }
```

**Commit:** `a1b2c3d4 - fix(virtues): add argumentsVertueux CSS class`

## Régénération

**Commande:** `dotnet run --project Argumentum.AssetConverter.csproj`
**Durée:** 8m 15s
**Fichiers régénérés:** 113 images, 1 PDF (37MB)

## Validation Visuelle

✅ **Images:** 10 échantillons vérifiés
- Carte racine "Arguments vertueux" → fond gris #555555 ✅
- 7 familles → couleurs conservées ✅
- Texte lisible ✅

✅ **PDF:** TarotCards_Virtues_fr-FacesOnly.pdf
- 15 pages ✅
- Fond gris page 1 ✅
- Pas de régression ✅

## Résultat

**Statut:** ✅ RÉSOLU
**Itérations:** 1/3
**Qualité:** 100% (problème éliminé)

## Leçons Apprises

- Template Virtues nécessite 8 classes CSS (1 racine + 7 familles)
- Couleur racine doit être gris #555555 (pas blanc)
- Valider TOUTES les classes CSS lors de modification templates
```

2. **Mettre à jour checkpoint:**

Ajoute entrée dans `.claude/pipeline-recovery-checkpoint.md`:

```markdown
### Session 2026-02-02 (Fix Virtues CSS)

- **Problème:** Classe CSS `argumentsVertueux` manquante
- **Correction:** Ajouté dans template Virtues_Face_fr.json
- **Validation:** 113 images + 1 PDF régénérés et vérifiés
- **Commit:** `a1b2c3d4`
```

## Utilisation dans le Workflow

Cet agent est conçu pour être appelé:

1. **Après un pipeline-validate** avec problèmes identifiés
2. **Par pipeline-iterate** pour correction automatique
3. **Manuellement** pour fix ciblé d'un problème connu

## Sorties Réutilisables

- **Fix Report MD:** Traçabilité complète de la correction
- **Commit Git:** Correction versionnée
- **Logs de régénération:** Preuve de non-régression
- **Validation visuelle:** Screenshots avant/après

## Rappels Critiques

- **Toujours lire avant modifier** - Ne jamais deviner le contenu d'un fichier
- **Valider visuellement** - Pas de "ça devrait marcher", confirme avec Read (vision)
- **Documenter pourquoi** - Capture la cause racine, pas juste le symptôme
- **Limiter les itérations** - 3 max, sinon intervention humaine nécessaire
- **Ne pas casser ce qui marche** - Vérifie absence de régression sur autres CardSets
