# Correction Interface CardPen-Playwright - Appel automatique generateImages()

**Date:** 2025-10-16  
**Mode:** Debug Complex  
**Investigateur:** Roo Debug  
**Statut:** ✅ CORRECTION APPLIQUÉE - EN ATTENTE DE VALIDATION

---

## Résumé Exécutif

**Problème identifié:** Le timeout Playwright sur `#zipButton` était causé par l'absence d'appel à la fonction JavaScript `generateImages()` dans le contexte de l'iframe CardPen.

**Cause racine:** HarvestManager attendait passivement que le bouton `#zipButton` devienne visible, mais ce bouton ne pouvait devenir visible QUE si `generateImages()` était appelée - ce qui ne se produisait jamais en mode automatisé.

**Solution appliquée:** Ajout d'un appel explicite `await iframe.EvaluateAsync("generateImages()")` après détection du bouton `#generateButton`.

---

## Grounding SDDD - Découvertes Sémantiques

### 1. Architecture CardPen Identifiée

D'après [`docs/sddd/2025-08-11-stabilisation-pipeline-visuel.md`](docs/sddd/2025-08-11-stabilisation-pipeline-visuel.md):

> **Cause Racine Finale:** L'intégralité de l'interface de rendu des cartes, y compris le bouton "Generate Images", est créée dynamiquement à l'intérieur d'une `<iframe>` (avec l'ID `#cpOutput`).

> **Solution Implémentée:** La logique dans `HarvestManager.cs` adopte la séquence correcte :
> 1. Injecter les données et déclencher le rendu via `cardpen.form.set()` et `cardpen.write.generate()`
> 2. Obtenir une référence programmatique au contenu de l'iframe `#cpOutput`
> 3. **Appeler directement la fonction `generateImages()` dans le contexte de l'iframe**

### 2. Historique des Problèmes

D'après [`docs/investigations/2025-10-16-rapport-corrections-regressions-pipeline.md`](docs/investigations/2025-10-16-rapport-corrections-regressions-pipeline.md):

> **Régression #2: Injection Manuelle de frame.js**
> - Version avril: PAS D'INJECTION - frame.js chargé automatiquement par CardPen
> - Correction appliquée: Suppression de l'injection manuelle problématique

Le problème résiduel après ces corrections : **`generateImages()` n'était toujours pas appelée**.

---

## Analyse Technique Détaillée

### Workflow CardPen Identifié

1. **Génération HTML Iframe** ([`Generation/CardPen/js/main.js`](Generation/CardPen/js/main.js:1218-1220))
   ```javascript
   fullOutput += "<button id='generateButton' type='button' onclick='generateImages();'>Generate Images</button>\n";
   fullOutput += "<button id='zipButton' type='button' style='display: none;' onclick='zipper();'>Zip Images</button>\n";
   ```
   
   ➜ Le bouton `#zipButton` est créé **caché par défaut** (`display: none`)

2. **Fonction generateImages()** ([`Generation/CardPen/js/frame.js`](Generation/CardPen/js/frame.js:8-25))
   ```javascript
   async function generateImages() {
       var zipButton = document.getElementById('zipButton');
       if (zipButton) {
           zipButton.style.display = 'none'; // Cache le bouton ZIP pendant génération
       }
       // ... génération des images ...
       if (zipButton) {
           zipButton.style.display = 'block'; // ✅ REND LE BOUTON VISIBLE
       }
   }
   ```
   
   ➜ **Le bouton ZIP ne devient visible QUE si `generateImages()` est appelée**

3. **Problème HarvestManager** (version cassée)
   ```csharp
   // Ligne 370 - AVANT CORRECTION
   var zipButtonLocator = iframe.Locator("#zipButton");
   await zipButtonLocator.WaitForAsync(new() { 
       State = WaitForSelectorState.Visible, 
       Timeout = 120000 
   });
   ```
   
   ➜ **Attend un événement qui ne se produira JAMAIS** car `generateImages()` n'est pas appelée

---

## Solution Implémentée

### Modifications Appliquées

**Fichier:** [`Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs`](Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs:367-387)

**Lignes modifiées:** 367-387

```csharp
// AVANT (version cassée)
// Étape 4 : Attendre la fin de la génération des images dans l'iframe
Log("Waiting for image generation to finish...");
var zipButtonLocator = iframe.Locator("#zipButton");
await zipButtonLocator.WaitForAsync(new() { 
    State = WaitForSelectorState.Visible, 
    Timeout = 120000 
});

// APRÈS (version corrigée)
// Étape 4 : Attendre que l'iframe soit prêt et appeler generateImages()
Log("Waiting for iframe to be ready...");

// Attendre que le bouton Generate Images soit présent (signal que l'iframe est chargé)
var generateButtonLocator = iframe.Locator("#generateButton");
await generateButtonLocator.WaitForAsync(new() { 
    State = WaitForSelectorState.Visible, 
    Timeout = 30000 
});
Log("Generate button found, iframe is ready.");

// Appeler generateImages() dans le contexte de l'iframe pour démarrer la génération
Log("Calling generateImages() in iframe context...");
await iframe.EvaluateAsync("generateImages()");
Log("generateImages() called, waiting for completion...");

// Attendre que le bouton ZIP devienne visible (signal que la génération est terminée)
var zipButtonLocator = iframe.Locator("#zipButton");
await zipButtonLocator.WaitForAsync(new() { 
    State = WaitForSelectorState.Visible, 
    Timeout = 120000 
});
```

### Séquence Complète Après Correction

1. ✅ **HarvestManager injecte données** : `cardpen.form.set()` + `cardpen.write.generate()`
2. ✅ **Obtient référence iframe** : `#cpOutput`
3. ✅ **Attend iframe ready** : Détection `#generateButton` visible (nouveau)
4. ✅ **Appelle generateImages()** : `iframe.EvaluateAsync("generateImages()")` (nouveau)
5. ✅ **Attend fin génération** : `#zipButton` visible (signal de complétion)
6. ✅ **Télécharge images** : Extraction `#cpImages img`

---

## Points Clés de la Correction

### ✅ Compatibilité avec Corrections Précédentes

1. **Timeout 120s conservé** (correction précédente valide)
2. **Pas d'injection frame.js** (correction précédente valide)
3. **Ajoute l'appel manquant** de `generateImages()`

### ✅ Signaux de Synchronisation

- **Signal "Ready"** : `#generateButton` visible → iframe chargé et initialisé
- **Signal "Start"** : Appel `generateImages()` → démarre génération
- **Signal "Complete"** : `#zipButton` visible → génération terminée

### ✅ Timeouts Appropriés

- **Iframe ready** : 30s (suffisant pour chargement HTML/CSS/JS)
- **Génération images** : 120s (temps nécessaire pour rendering + domtoimage)

---

## Validation Prévue

### Tests à Effectuer

1. **Test compilation** : Vérifier que le code compile sans erreur
2. **Test données minimales** : 2-3 cartes Rules pour validation rapide
3. **Test config réduite** : 10-15 cartes avec config minimale
4. **Test complet** : Pipeline complet avec tous les CardSets

### Critères de Succès

✅ **SUCCÈS COMPLET** si :
- Aucune `TimeoutException` sur `#zipButton`
- Logs montrent : "Calling generateImages() in iframe context"
- Logs montrent : "Image generation process completed successfully"
- Fichiers `.harvest.json` créés avec images Base64
- PDFs générés correctement

⚠️ **SUCCÈS PARTIEL** si :
- `generateImages()` appelée mais erreurs JavaScript
- Images générées partiellement
- Timeout persiste mais durée réduite

❌ **ÉCHEC** si :
- Timeout persiste à 120s
- `generateImages()` non appelée
- Aucune image générée

---

## Script de Test

Créé : [`docs/investigations/scripts/2025-10-16-09-test-correction-generateimages.ps1`](docs/investigations/scripts/2025-10-16-09-test-correction-generateimages.ps1)

**Exécution :**
```powershell
cd D:\Dev\Argumentum
.\docs\investigations\scripts\2025-10-16-09-test-correction-generateimages.ps1
```

**Points de contrôle :**
- ✓ Iframe ready
- ✓ generateImages() appelée
- ✓ generateImages() confirmée
- ✓ Bouton ZIP visible
- ✓ Images téléchargées
- ✓ Harvest sauvegardé

---

## Prochaines Étapes

1. **Exécuter test de validation** avec script créé
2. **Analyser logs** pour confirmer séquence complète
3. **Vérifier fichiers générés** (.harvest.json, PDFs)
4. **Mettre à jour rapports** avec résultats
5. **Valider en production** si tests réussis

---

## Références Documentaires

### Documents Consultés (Grounding SDDD)

1. [`docs/sddd/2025-08-11-stabilisation-pipeline-visuel.md`](docs/sddd/2025-08-11-stabilisation-pipeline-visuel.md)
   - Architecture iframe CardPen
   - Séquence correcte d'initialisation
   
2. [`docs/investigations/2025-10-16-rapport-corrections-regressions-pipeline.md`](docs/investigations/2025-10-16-rapport-corrections-regressions-pipeline.md)
   - Historique régressions
   - Corrections timeout et frame.js
   
3. [`Generation/Documentation/ARCHITECTURE_PIPELINE.md`](Generation/Documentation/ARCHITECTURE_PIPELINE.md)
   - Flux détaillé Playwright
   - Phase harvesting
   
4. [`docs/sddd/2025-09-20-finalisation-pipeline-pdf.md`](docs/sddd/2025-09-20-finalisation-pipeline-pdf.md)
   - Validation pipeline end-to-end
   - Corrections JavaScript iframe

### Fichiers Modifiés

- [`Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs`](Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs) (lignes 367-387)

### Fichiers Analysés

- [`Generation/CardPen/index.html`](Generation/CardPen/index.html)
- [`Generation/CardPen/js/main.js`](Generation/CardPen/js/main.js) (lignes 1194-1260)
- [`Generation/CardPen/js/frame.js`](Generation/CardPen/js/frame.js) (lignes 8-25)

---

**Rapport généré le:** 2025-10-16 à 09:48 UTC+2  
**Auteur:** Roo Debug (Debug Complex Mode)  
**Statut:** ✅ CORRECTION APPLIQUÉE - PRÊT POUR TEST