# Rapport d'Exécution du Pipeline PDF Argumentum - 16 Octobre 2025

**Date:** 2025-10-16 03:35 UTC+2  
**Durée d'Exécution:** ~1min 42s  
**Statut:** ❌ **ÉCHEC CRITIQUE**

---

## 📋 Résumé Exécutif

L'exécution complète du pipeline de génération PDF Argumentum a **ÉCHOUÉ** malgré un serveur CardPen fonctionnel. Trois erreurs critiques majeures ont été identifiées empêchant la génération de tout PDF.

### Résultat Final
- **PDFs Générés:** 0/4 critiques (0%)
- **Fichiers Harvest:** Partiels (3 existants chargés)
- **Images Générées:** Échec total
- **Cause Principale:** Erreurs de timeout et désynchronisation CardPen

---

## ✅ Prérequis Validés

### 1. Serveur CardPen
```
URL: http://localhost:5258/index.html
Statut: ✅ ACTIF ET RÉPONDANT
Réponse: 200 OK avec contenu HTML complet
```

### 2. Configuration
```
Fichier: AssetConverterConfig.json
Modes Activés: 
  - WebBasedImageGeneration
  - QuestPdfGeneration
UseLocalCardpen: true
HeadLessBrowser: false
EnableParallelism: true
```

### 3. Environnement
```
.NET Runtime: net9.0
Compilateur: OK (warnings de vulnérabilités NuGet non bloquants)
Workspace: D:\Dev\Argumentum
```

---

## 🔴 Erreurs Critiques Identifiées

### Erreur 1: Timeout CardPen Interface (BLOQUANT)

**Type:** `TimeoutException`  
**Localisation:** `HarvestManager.cs:378`

```
TimeoutException: Timeout 60000ms exceeded.
Call log:
  - waiting for Locator("#zipButton") to be visible
```

**Analyse:**
- Le pipeline attend que le bouton "ZIP" apparaisse dans l'interface CardPen
- Délai d'attente: 60 secondes dépassé
- L'interface CardPen ne parvient pas à générer ou afficher ce bouton
- Aucune image n'a pu être téléchargée

**Impact:** Bloque la génération de toutes les images de cartes

---

### Erreur 2: Désynchronisation Comptage Images (CRITIQUE)

**Type:** `ApplicationException`  
**Localisation:** `HarvestManager.cs:420`

```
ApplicationException: Mismatch between generated image count (1) 
and expected card count (0). Card IDs: []
```

**Analyse:**
- CardPen a généré 1 image
- Le système attendait 0 carte (liste vide: `[]`)
- Désynchronisation totale entre l'interface et le backend
- Les IDs de cartes ne sont pas correctement propagés

**Impact:** Impossibilité de télécharger et associer les images générées

---

### Erreur 3: Reflection CSV Oversize (NON BLOQUANT)

**Type:** `TargetInvocationException`  
**Localisation:** Traitement CSV des Fallacies

```
Exception: An item with the same key has already been added. 
Key: Type:<>f__AnonymousType<...>
```

**Analyse:**
- Le fichier CSV `Argumentum Fallacies - Taxonomy.csv` contient trop de colonnes
- Génération d'un type anonyme dépassant les limites du compilateur C#
- Erreur lors de la création dynamique du modèle de données
- Plus de **1362 lignes de définition de propriétés** générées

**Impact:** Potentiellement empêche le chargement correct des données Fallacies

---

## 📊 Chronologie Détaillée de l'Exécution

### Phase 1: Initialisation (00:00:00 - 00:00:03)
```
✅ Configuration chargée: AssetConverterConfig.json
✅ 6 CardSets cibles identifiés
✅ 3 harvests existants chargés:
   - Fallacies_harvest_fr.json
   - FallaciesWeb_harvest_fr.json  
   - FallaciesPrintAndPlay_harvest_fr.json
```

### Phase 2: Traitement CSV (00:00:03 - 00:00:10)
```
⚠️ Chargement fichiers CSV
⚠️ Parsing colonnes multiples
❌ ERREUR: Reflection oversize sur Fallacies CSV
```

### Phase 3: Génération Images CardPen (00:00:10 - 00:01:42)
```
🔄 Ouverture navigateur Playwright
🔄 Navigation vers http://localhost:5258/index.html
🔄 Chargement template JSON dans CardPen
⏳ Attente génération cartes...
⏳ Recherche bouton "#zipButton"...
⏳ 10s... 20s... 30s... 40s... 50s... 60s...
❌ TIMEOUT: Bouton ZIP jamais apparu
❌ ÉCHEC: Impossible de télécharger les images
```

### Phase 4: Validation Images (00:01:42)
```
❌ Vérification comptage: 1 générée vs 0 attendue
❌ Liste CardIDs vide: []
❌ ÉCHEC FATAL: Désynchronisation totale
🛑 ARRÊT DU PIPELINE
```

---

## 📁 Fichiers Générés

### ❌ PDFs Critiques Manquants

| Fichier Attendu | Statut | Taille | Commentaire |
|-----------------|--------|--------|-------------|
| `Argumentum_TarotCards_fr.pdf` | ❌ ABSENT | 0 B | Non généré |
| `Argumentum_TarotCards_Print&Play_A4_fr.pdf` | ❌ ABSENT | 0 B | Non généré |
| `Argumentum_PokerCards_fr.pdf` | ❌ ABSENT | 0 B | Non généré |
| `Argumentum_PokerCards_Print&Play_A4_fr.pdf` | ❌ ABSENT | 0 B | Non généré |

### ✅ Fichiers Harvest Existants (Anciens)

```powershell
# Ces fichiers datent d'exécutions précédentes:
Fallacies_harvest_fr.json
FallaciesWeb_harvest_fr.json
FallaciesPrintAndPlay_harvest_fr.json
```

**Note:** Aucun nouveau harvest généré lors de cette exécution

---

## 🔍 Diagnostic Détaillé

### Hypothèse 1: Problème d'Interface CardPen ⭐ PROBABLE

**Symptômes:**
- Bouton ZIP n'apparaît jamais
- Timeout systématique à 60s
- Interface CardPen ne finalise pas la génération

**Causes Possibles:**
1. **JavaScript non exécuté correctement** dans l'iframe CardPen
2. **Données JSON malformées** envoyées à CardPen
3. **Erreurs de rendering** des cartes bloquant le workflow
4. **Sélecteur CSS obsolète** (`#zipButton` inexistant/modifié)

**Tests Recommandés:**
```powershell
# Test 1: Vérifier bouton ZIP en mode manuel
# Ouvrir http://localhost:5258/index.html
# Charger un template simple
# Observer si le bouton ZIP apparaît

# Test 2: Console JavaScript
# F12 > Console
# Rechercher erreurs JavaScript
```

---

### Hypothèse 2: Désynchronisation Données CSV/JSON ⭐⭐ TRÈS PROBABLE

**Symptômes:**
- Expected card count = 0
- Liste CardIDs vide: `[]`
- Type anonyme oversize

**Causes Possibles:**
1. **CSV trop complexe** (trop de colonnes) → parsing échoue
2. **Filtres CSV défectueux** → aucune carte sélectionnée
3. **Mapping JSON → CardPen incomplet** → IDs perdus
4. **Référence circulaire** dans les données

**Exemple Problématique:**
```json
// Config filtrant les cartes:
"CsvFilterField": "carte",
"CsvFilterValues": ["1", "2"]

// Si le CSV n'a pas de colonne "carte" ou valeurs incorrectes
// → Résultat: 0 carte
```

---

### Hypothèse 3: Regression Code Pipeline 🔧 POSSIBLE

**Symptômes:**
- Code compile mais runtime errors
- Gestion d'erreurs inadéquate

**Indices:**
- Stack trace mentionne lignes spécifiques:
  - `HarvestManager.cs:378` (timeout)
  - `HarvestManager.cs:420` (mismatch count)
  - `HarvestManager.cs:390` (download images)

**Action Recommandée:**
```bash
# Comparer version actuelle vs dernière version fonctionnelle
git diff HEAD~10 Generation/Converters/Argumentum.AssetConverter/HarvestManager.cs
```

---

## 🛠️ Solutions Proposées

### Solution 1: Mode Debug Renforcé ⚡ PRIORITÉ 1

**Objectif:** Capturer visuellement ce qui se passe dans CardPen

```json
// AssetConverterConfig.json
{
  "WebBasedGeneratorConfig": {
    "HeadLessBrowser": false,  // ✅ Déjà fait
    "ShowInfoLogs": true,       // ✅ Déjà fait
    
    // AJOUTER:
    "SaveScreenshotsOnError": true,
    "SlowMo": 1000,  // Ralentir Playwright de 1s par action
    "DevTools": true  // Ouvrir DevTools automatiquement
  }
}
```

**Exécution:**
```powershell
# Relancer en observant la fenêtre navigateur
cd Generation/Converters/Argumentum.AssetConverter
dotnet run
```

---

### Solution 2: Test Manuel CardPen 🧪 PRIORITÉ 1

**Objectif:** Vérifier si CardPen fonctionne indépendamment

**Étape 1: Charger Template Minimal**
```powershell
# Ouvrir navigateur sur:
Start-Process "http://localhost:5258/index.html"

# Charger manuellement:
Cards/Rules/Argumentum_Rules_fr.json
```

**Étape 2: Observer Interface**
- Le template se charge-t-il correctement ?
- Les cartes s'affichent-elles ?
- Le bouton "Images" ou "ZIP" apparaît-il ?
- Des erreurs JavaScript dans la console ?

**Étape 3: Télécharger Manuellement**
- Cliquer sur le bouton de génération
- Observer le processus
- Télécharger le ZIP
- Vérifier les images générées

---

### Solution 3: Simplifier Configuration 📝 PRIORITÉ 2

**Objectif:** Isoler le problème en testant un seul CardSet

```json
// AssetConverterConfig.json - Version Test Minimal
{
  "WebBasedGeneratorConfig": {
    "CardSets": [
      {
        "Name": "Rules",  // UN SEUL CardSet simple
        "FaceCardSetInfo": {
          "DataSet": "Rules",
          "JsonFilePathDebug": "..\\..\\..\\..\\..\\..\\Cards\\Rules\\Argumentum_Rules_fr.json"
        }
      }
      // SUPPRIMER TOUS LES AUTRES
    ],
    "CardSetDocuments": [
      {
        "DocumentName": "Test_Rules_Only.pdf",
        "Enabled": true,
        "CardSets": [
          {
            "CardSetName": "Rules",
            "NbCopies": 1,
            "FrontCards": {
              "HeigthMM": 113,
              "WidthMM": 60
            }
          }
        ]
      }
      // SUPPRIMER TOUS LES AUTRES
    ]
  }
}
```

---

### Solution 4: Désactiver Parallélisme 🔄 PRIORITÉ 2

**Objectif:** Éviter les race conditions

```json
{
  "WebBasedGeneratorConfig": {
    "EnableParallelism": false,  // ← Changer à false
    "MaxDegreeOfParallelismCardpen": 1,
    "MaxDegreeOfParallelismImages": 1
  }
}
```

---

### Solution 5: Augmenter Timeouts ⏱️ PRIORITÉ 3

**Objectif:** Laisser plus de temps à CardPen

```csharp
// À modifier dans HarvestManager.cs ligne ~378
// Remplacer:
await page.Locator("#zipButton").WaitForAsync(new() { Timeout = 60000 });

// Par:
await page.Locator("#zipButton").WaitForAsync(new() { Timeout = 300000 }); // 5 minutes
```

---

### Solution 6: Corriger CSV Oversize 📊 PRIORITÉ 2

**Objectif:** Réduire la complexité du CSV Fallacies

**Option A: Nettoyer CSV**
```powershell
# Supprimer colonnes inutilisées du CSV
# Garder seulement les colonnes essentielles
```

**Option B: Désactiver Fallacies Temporairement**
```json
{
  "WebBasedGeneratorConfig": {
    "CardSets": [
      // COMMENTER les CardSets Fallacies:
      // { "Name": "Fallacies", ... },
      // { "Name": "FallaciesPrintAndPlay", ... },
    ]
  }
}
```

---

## 📋 Plan d'Action Recommandé

### Étape 1: Investigation Immédiate ⚡
1. **Test Manuel CardPen** (Solution 2)
   - Durée: 10 minutes
   - Confirme si problème = Pipeline ou CardPen
   
2. **Mode Debug Visual** (Solution 1)
   - Durée: 5 minutes setup + exécution
   - Capture screenshots d'erreur

### Étape 2: Correction Rapide 🔧
3. **Configuration Minimale** (Solution 3)
   - Durée: 15 minutes
   - Test avec 1 seul CardSet simple
   
4. **Désactiver Parallélisme** (Solution 4)
   - Durée: 2 minutes
   - Évite race conditions

### Étape 3: Corrections Profondes 🛠️
5. **Nettoyer CSV Fallacies** (Solution 6)
   - Durée: 30 minutes
   - Résout l'oversize reflection
   
6. **Augmenter Timeouts** (Solution 5)
   - Durée: 10 minutes
   - Fallback si CardPen lent

---

## 📈 Métriques d'Échec

| Métrique | Valeur | Cible | Écart |
|----------|--------|-------|-------|
| PDFs Générés | 0 | 4 | -100% |
| Temps Exécution | 1m 42s | ~10m | N/A (échec avant) |
| Images Téléchargées | 0 | ~200 | -100% |
| Erreurs Critiques | 3 | 0 | +∞ |
| Taux de Succès | 0% | 100% | -100% |

---

## 🔗 Références

### Fichiers Impliqués
- [`AssetConverterConfig.json`](Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.json)
- `HarvestManager.cs:378` (timeout)
- `HarvestManager.cs:420` (mismatch)
- [`Argumentum Fallacies - Taxonomy.csv`](Cards/Fallacies/Argumentum%20Fallacies%20-%20Taxonomy.csv)

### Documentation SDDD
- [`2025-09-20-finalisation-pipeline-pdf.md`](docs/sddd/2025-09-20-finalisation-pipeline-pdf.md) - Spécifications attendues
- [`2025-10-16-rapport-regression-cardpen.md`](docs/investigations/2025-10-16-rapport-regression-cardpen.md) - Investigation précédente

### Serveur CardPen
- URL Locale: http://localhost:5258/index.html
- Port: 5258
- Statut: ✅ Actif

---

## ⚠️ Conclusion Critique

**Le pipeline PDF Argumentum est actuellement NON FONCTIONNEL** malgré:
- ✅ Serveur CardPen actif
- ✅ Configuration apparemment correcte
- ✅ Compilation sans erreur

**Bloqueurs Majeurs:**
1. CardPen ne finalise pas la génération (timeout bouton ZIP)
2. Désynchronisation données CSV → JSON → CardPen
3. CSV Fallacies trop complexe (oversize type anonyme)

**Recommandation Immédiate:**
Exécuter **Solution 2 (Test Manuel CardPen)** pour déterminer si le problème vient du code Pipeline ou de l'intégration CardPen elle-même.

**Estimation Correction:**
- Si problème CardPen: 2-4 heures (investigation + fix interface)
- Si problème Pipeline: 4-8 heures (refactoring logique harvest)
- Si problème CSV: 1-2 heures (nettoyage colonnes)

---

**Rapport généré le:** 2025-10-16 à 03:38 UTC+2  
**Auteur:** Roo Code  
**Statut:** ❌ ÉCHEC VALIDÉ - INVESTIGATION REQUISE