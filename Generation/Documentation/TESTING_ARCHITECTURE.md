# Stratégie et Architecture de Test pour Argumentum.AssetConverter

## 1. Introduction

Ce document définit la stratégie et l'architecture de test complètes pour le pipeline de génération d'assets du projet `Argumentum.AssetConverter`. L'objectif est de mettre en place une fondation robuste pour un développement piloté par les tests (TDD), garantissant la fiabilité et la maintenabilité du pipeline.

## 2. Analyse de l'Existant

Une analyse de la structure du projet `Argumentum.AssetConverter` a été effectuée. Les points clés sont :

*   **Absence de framework de test formel :** Le fichier `.csproj` ne contient aucune dépendance à un framework de test standard (xUnit, NUnit, MSTest).
*   **Système de validation post-génération :** Le projet contient un répertoire `Tests/` avec des classes comme `CardGenerationValidationTests.cs`. Cependant, il ne s'agit pas de tests unitaires, mais de scripts de validation qui sont exécutés en tant qu'application console.
*   **Rapports manuels :** Les validations génèrent un rapport HTML et ne s'intègrent pas facilement dans un pipeline d'intégration continue (CI/CD).

**Conclusion :** Le système existant est un outil de contrôle qualité final, mais il n'est pas adapté à une approche TDD. Une nouvelle architecture de test est donc nécessaire.

## 3. Framework de Test Proposé : xUnit

Nous proposons l'adoption du framework **xUnit** pour tous les tests du projet.

### Justification

*   **Modernité et Alignement .NET :** xUnit est le standard de facto pour les projets .NET modernes et est utilisé par Microsoft pour tester le runtime .NET lui-même.
*   **Isolation des Tests :** xUnit crée une nouvelle instance de la classe de test pour chaque méthode de test. Cela garantit une isolation complète, ce qui est crucial pour les tests manipulant des fichiers ou des états, et prévient la pollution entre les tests.
*   **Exécution Parallèle :** Le projet montre déjà des optimisations pour le parallélisme. xUnit exécute les tests en parallèle par défaut, ce qui s'aligne avec la philosophie du projet et accélérera significativement le pipeline de build.
*   **Intégration et Outillage :** Il s'intègre parfaitement avec `dotnet test` et le Test Explorer de Visual Studio.

## 4. Architecture du Projet de Test

### 4.1. Nouveau Projet de Test

Un nouveau projet C# sera créé :

*   **Nom :** `Argumentum.AssetConverter.Tests`
*   **Emplacement :** `Generation/Converters/Argumentum.AssetConverter.Tests/`
*   **Type :** Bibliothèque de classes (.NET 8.0)

### 4.2. Dépendances NuGet

Le projet de test inclura les paquets suivants :

*   `Microsoft.NET.Test.Sdk`
*   `xunit`
*   `xunit.runner.visualstudio`
*   `FluentAssertions` : Pour des assertions plus lisibles et expressives.
*   `NSubstitute` : Pour la création de doublures de test (mocks et stubs).

### 4.3. Structure des Dossiers

L'organisation des dossiers suivra en miroir celle du projet principal pour une meilleure découvrabilité.

```
Argumentum.AssetConverter.Tests/
├── Assets/                 # Fichiers de données de test (CSV, templates, etc.)
│   ├── Csv/
│   │   ├── valid_fallacies.csv
│   │   └── malformed_fallacies.csv
│   └── HtmlTemplates/
│       └── simple_fallacy_template.scriban
├── Parsing/                # Tests pour l'analyse des données (CsvHelper)
│   └── CsvParserTests.cs
├── HtmlGeneration/         # Tests pour la génération HTML (Scriban)
│   └── ScribanGeneratorTests.cs
├── ImageConversion/        # Tests d'intégration pour HTML -> PNG
│   └── HtmlToPngConverterTests.cs
└── PdfAssembly/            # Tests d'intégration pour l'assemblage PDF
    └── PdfAssemblerTests.cs
```

## 5. Stratégie de Test Détaillée

### Étape 1 : Parsing des Données (CSV)

*   **Localisation :** `Parsing/CsvParserTests.cs`
*   **Objectif :** Valider que les fichiers CSV sont correctement lus et mappés aux objets C#.
*   **Tests Clés :**
    *   **Test de cas nominal :** Utiliser `valid_fallacies.csv` pour vérifier que le bon nombre d'objets est créé et que leurs propriétés correspondent exactement aux données du fichier.
    *   **Test de cas d'erreur :** Utiliser `malformed_fallacies.csv` (avec des en-têtes manquants ou des types incorrects) pour s'assurer que des exceptions appropriées (ex: `HeaderValidationException`) sont levées.
    *   **Test de cas limite :** Utiliser un fichier CSV vide ou ne contenant que les en-têtes pour vérifier que le parseur retourne une collection vide sans erreur.

### Étape 2 : Génération HTML (Scriban/Liquid)

*   **Localisation :** `HtmlGeneration/ScribanGeneratorTests.cs`
*   **Objectif :** Vérifier que les objets C# sont correctement injectés dans les templates HTML.
*   **Tests Clés :**
    *   **Test de rendu de propriété simple :** Créer un objet C# en mémoire, le passer au moteur de template avec un template simple (`<h1>{{ entity.Name }}</h1>`) et vérifier que la chaîne HTML de sortie est correcte.
    *   **Test de logique de template :** Tester les conditions (`if/else`) et les boucles (`for`) pour s'assurer qu'elles se comportent comme attendu.

### Étape 3 : Conversion HTML vers PNG

*   **Localisation :** `ImageConversion/HtmlToPngConverterTests.cs`
*   **Objectif :** S'assurer que le HTML peut être converti en une image PNG. Ces tests seront marqués comme des tests d'intégration.
*   **Tests Clés :**
    *   **Test de conversion réussie :** Fournir une chaîne HTML simple et vérifier que la méthode de conversion retourne un `byte[]` non nul.
    *   **Test de validation de l'image (optionnel) :** Utiliser une librairie comme `ImageSharp` pour lire le `byte[]` en mémoire et vérifier que les dimensions de l'image sont approximativement celles attendues.

### Étape 4 : Assemblage du PDF

*   **Localisation :** `PdfAssembly/PdfAssemblerTests.cs`
*   **Objectif :** Valider que plusieurs images PNG peuvent être assemblées en un document PDF.
*   **Tests Clés :**
    *   **Test de nombre de pages :** Fournir une liste de 3 `byte[]` (images PNG) à la méthode d'assemblage. Utiliser ensuite une librairie comme `PdfPig` pour lire le PDF résultant en mémoire et vérifier que le nombre de pages est bien de 3.

## 6. Stratégie pour les Tests d'Intégration du Pipeline

Les tests unitaires ci-dessus sont essentiels pour valider les composants individuels. Cependant, il est crucial de tester également le pipeline orchestré dans son ensemble. L'approche monolithique (tout regénérer à chaque fois) est trop lente. Nous allons donc adopter une stratégie de **tests d'intégration granulaires** qui exploite la logique de "skip" du pipeline.

### 6.1. Le Principe : Isoler une Étape en Contrôlant le Système de Fichiers

Le moteur de `AssetConverter` est conçu pour ne pas refaire un travail déjà accompli. Il vérifie systématiquement si les fichiers de sortie (`.png`, `.pdf`, etc.) existent avant de lancer une étape de génération coûteuse.

Notre stratégie de test tire parti de ce comportement : **pour tester une étape spécifique, nous allons préparer le système de fichiers pour que toutes les étapes *précédentes* soient considérées comme "déjà faites"**.

Chaque test d'intégration suivra donc ce modèle :
1.  **Setup (Arrange)** :
    *   Définir un répertoire de test unique et isolé.
    *   Copier ou créer les **artefacts d'entrée** nécessaires à l'étape testée (ex: les images PNG pour l'étape d'assemblage PDF).
    *   S'assurer que les **artefacts de sortie** de l'étape testée sont **absents**.
2.  **Action (Act)** :
    *   Exécuter le pipeline `AssetConverter` avec une configuration minimale pointant vers le répertoire de test.
3.  **Assertion (Assert)** :
    *   Vérifier que l'artefact de sortie a été correctement créé.
    *   Vérifier que les artefacts des autres étapes n'ont pas été modifiés (en comparant leurs timestamps par exemple).

### 6.2. Exemple Concret : Tester Uniquement l'Étape "Génération PDF"

**Objectif :** Valider que l'assemblage PDF fonctionne, sans régénérer les images PNG.

**Implémentation du test :**
1.  **Setup :**
    *   Créer un répertoire de test: `TestOutput/PdfOnlyTest/`.
    *   Dans `TestOutput/PdfOnlyTest/`, créer un sous-dossier `images/` contenant des fichiers PNG factices (`card-1.png`, `card-2.png`). Ces fichiers peuvent être des copies d'un jeu de référence.
    *   S'assurer que `TestOutput/PdfOnlyTest/MyDocument.pdf` n'existe pas.
2.  **Action :**
    *   Configurer `AssetConverterConfig.json` pour ne générer qu'un seul document (`MyDocument.pdf`) à partir des images du dossier `images/`.
    *   Lancer l'application `AssetConverter`.
3.  **Assertion :**
    *   Vérifier que `TestOutput/PdfOnlyTest/MyDocument.pdf` a bien été créé.
    *   (Optionnel mais recommandé) Vérifier que les timestamps des fichiers dans `images/` n'ont pas changé.
    *   Utiliser `PdfPig` pour inspecter le PDF et s'assurer qu'il contient le bon nombre de pages.

### 6.3. Exploiter les Flags de Forçage (`OverwriteExisting...`)

L'application possède des flags comme `OverwriteExistingDocs` ou `OverwriteExistingHtmlMaps`. Ces flags sont parfaits pour tester la logique de "skip" elle-même.

**Exemple de test pour la logique de "skip" :**
1.  **Setup :**
    *   Préparer une structure similaire à l'exemple précédent, mais cette fois-ci, **créer un fichier `MyDocument.pdf` factice (0-byte)** dans le répertoire de sortie.
2.  **Action :**
    *   Lancer `AssetConverter` avec le flag `OverwriteExistingDocs` à **`false`**.
3.  **Assertion :**
    *   Vérifier que le fichier `MyDocument.pdf` n'a **pas été modifié** (comparer sa taille ou son timestamp).
4.  **Action 2 :**
    *   Relancer `AssetConverter` avec le flag `OverwriteExistingDocs` à **`true`**.
5.  **Assertion 2 :**
    *   Vérifier que le fichier `MyDocument.pdf` a été **remplacé** par le nouveau document (la taille doit être supérieure à 0).

### 6.4. Gestion des Timeouts et de la Stabilité

Le pipeline `AssetConverter` peut parfois se bloquer, notamment lors des interactions avec le navigateur (Playwright). Pour éviter que le pipeline de CI/CD ne s'exécute indéfiniment, tous les tests d'intégration doivent être configurés avec un **timeout**.

xUnit permet de le faire facilement avec l'attribut `Fact` :
```csharp
[Fact(Timeout = 30000)] // Le test échouera s'il dure plus de 30 secondes
public void PdfAssembly_Should_Create_Valid_Pdf_From_Existing_Pngs()
{
    // ... logique du test
}
```
Ce filet de sécurité est indispensable pour garantir la robustesse de notre suite de tests.