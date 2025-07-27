# Architecture de Test pour le Pipeline de Génération de Mindmap

**Auteur:** Roo, Architecte Technique
**Date:** 25/07/2025
**Statut:** En cours de validation

## 1. Contexte et Objectifs

Ce document décrit la stratégie de test et l'architecture retenue pour fiabiliser le pipeline de génération des mindmaps (`.mm`) et leur conversion en SVG. Le pipeline actuel, bien que fonctionnel, présente des fragilités notables, notamment lors de la conversion par l'outil externe Freeplane/Freemind et le post-traitement des fichiers SVG.

L'objectif est d'implémenter une suite de tests automatisés pragmatiques qui garantissent la **prévisibilité**, la **robustesse** et la **maintenabilité** du pipeline, en se concentrant sur des validations concrètes plutôt que sur une validation esthétique subjective.

## 2. Analyse du Pipeline Existant

Le pipeline peut être schématisé comme suit :

```mermaid
graph TD
    A[Source de données (CSV)] --> B{Logique C#};
    B --> |Génération XML| C[Fichier `.mm`];
    C --> D{Processus Externe (Freeplane)};
    D --> |Conversion| E[Fichier `.svg`];
    E --> F{Logique C# de "Disambiguation"};
    F --> |Modification| G[Fichier `.svg` final];

    style D fill:#f9f,stroke:#333,stroke-width:2px;
    style F fill:#f9f,stroke:#333,stroke-width:2px;
```

Les points de fragilité identifiés sont :
- **D. Processus Externe :** L'appel à Freeplane, bien que maintenant documenté (voir [`EXTERNAL_PROCESS_HANDLING.md`](./EXTERNAL_PROCESS_HANDLING.md)), reste une interaction avec un système tiers. Les risques d'échec sont gérés par les tests d'intégration.
- **F. Logique de "Disambiguation" :** Le post-traitement du SVG est basé sur une heuristique fragile ("devinette" du contenu textuel) pour lier les données aux nœuds graphiques. C'est le risque le plus élevé du pipeline actuel.

## 3. Stratégie de Test Granulaire

La stratégie consiste à tester chaque segment du pipeline de manière isolée et adaptée.

### 3.1. Tests de la Génération du Fichier `.mm` (Étape A -> C)

Cette partie est entièrement contrôlée par le code C# et doit être couverte par des **tests unitaires rapides**.

- **Acteurs Clés :** Les méthodes `CreateFreemindmap` et `SerializeMindMapAsync` dans les classes `FallacyMindMapDocumentConfig` et `VirtueMindMapDocumentConfig`.

- **Type de Tests :** Tests unitaires.

- **Points de Validation :**
    1.  **Conformité du Schéma XML :**
        -   **Objectif :** S'assurer que le fichier `.mm` généré est un document XML valide et bien formé.
        -   **Mise en œuvre :** Un test qui génère le `.mm` et le charge via `System.Xml.Linq.XDocument.Parse()`. Le test passe si aucune exception n'est levée.
    2.  **Intégrité des Données :**
        -   **Objectif :** S'assurer que les données du CSV source sont correctement écrites dans les attributs des nœuds XML.
        -   **Mise en œuvre :** Un test qui, pour une entrée CSV connue, génère le `.mm` et utilise XPath ou LINQ to XML pour vérifier la présence et le contenu des nœuds attendus (ex: `//node[@TEXT='Titre Attendu']` et la valeur de ses enfants).
    3.  **Présence de l'ID Unique (Après Refactoring) :**
        -   **Objectif :** Valider que l'identifiant unique et stable de la donnée source est bien écrit comme attribut sur le nœud XML correspondant.
        -   **Mise en œuvre :** Mettre à jour le test précédent pour vérifier la présence d'un attribut `ID` ou `UID` sur le nœud.

### 3.2. Tests de la Conversion `.mm` vers `.svg` (Étape C -> E)

Cette partie teste l'interaction avec un système externe et doit être couverte par des **tests d'intégration**. L'implémentation de référence pour la gestion de ce processus est détaillée dans le document [`EXTERNAL_PROCESS_HANDLING.md`](./EXTERNAL_PROCESS_HANDLING.md).

- **Acteurs Clés :** La méthode `TryAutomateSvgConversion` qui encapsule l'appel à `freeplane.bat`.

- **Type de Tests :** Tests d'intégration.

- **Points de Validation :**
    1.  **Succès de l'Exécution :**
        -   **Objectif :** Confirmer que le processus externe s'est terminé sans erreur.
        -   **Mise en œuvre :** Un test qui exécute le processus et vérifie que le **code de sortie est `0`**.
    2.  **Création de l'Artefact :**
        -   **Objectif :** S'assurer qu'un fichier SVG a bien été créé.
        -   **Mise en œuvre :** Le test doit vérifier l'existence du fichier SVG de sortie et s'assurer qu'il **n'est pas vide** (taille > 0).
    3.  **Validation Minimale du Contenu SVG :**
        -   **Objectif :** S'assurer que le fichier est bien un SVG.
        -   **Mise en œuvre :** Le test doit lire le début du fichier et vérifier la présence de la balise `<svg`.
    4.  **Présence de l'ID Unique dans le SVG (Après Refactoring) :**
        -   **Objectif :** Valider que l'ID unique a été préservé par l'outil externe.
        -   **Mise en œuvre :** Le test doit parser le SVG et vérifier que des éléments (probablement `<g>`) possèdent l'attribut `id` avec la valeur attendue.

### 3.3. Tests du Post-Traitement SVG via Snapshot (Étape E -> G)

Cette étape est cruciale pour garantir la non-régression du post-traitement appliqué aux fichiers SVG. La logique, bien que simplifiée, modifie le DOM du SVG (nettoyage, ajout de styles, etc.).

- **Acteurs Clés :** La méthode `ProcessSvgFilesAsync` dans les configurations de mindmap.
- **Type de Tests :** Tests de Snapshot.
- **Principe :**
    1.  Un fichier SVG source (`.svg`) est traité par la méthode.
    2.  Le contenu du SVG résultant est comparé, au caractère près, à un fichier de référence stocké dans le projet (`.snapshot.svg`). Ce fichier de référence représente l'état "approuvé" de la sortie.
    3.  Le test échoue si la moindre différence est détectée entre la sortie actuelle et le snapshot.
- **Avantages :**
    - **Exhaustivité :** Capture la moindre modification inattendue (espaces, ordre des attributs, etc.).
    - **Stabilité :** Verrouille le comportement de la sortie et empêche les régressions visuelles ou structurelles.
    - **Documentation implicite :** Le fichier `.snapshot.svg` documente concrètement la sortie attendue.
- **Points de Validation :**
    - **Identité de la sortie :** Le test valide que pour une entrée donnée, la sortie est toujours rigoureusement identique à la version approuvée.

### 3.4. Abandon de la Logique de "Disambiguation" (Étape F)

Conformément aux conclusions des analyses précédentes (voir `Refactoring_SVG_Data_Binding.md`), la stratégie de test n'est pas de tester cette logique, mais de **supporter sa suppression**. Elle est une source de dette technique trop importante.

La nouvelle architecture **supprime cette étape** en s'assurant que l'identifiant unique est propagé de bout en bout, rendant toute forme de "devinette" inutile. Les tests décrits en 3.1.3 et 3.2.4 sont conçus pour valider cette nouvelle approche robuste.

## 4. Conclusion

Cette architecture de test en plusieurs niveaux (unitaire et intégration) permet de valider chaque segment du pipeline de génération de mindmap de manière indépendante et pragmatique. Elle se concentre sur des validations techniques objectives (conformité XML, codes de sortie, présence d'ID) et contourne consciemment le problème de la validation esthétique, tout en promouvant l'abandon des logiques les plus fragiles au profit d'une architecture basée sur des identifiants stables.

## 5. Implémentation et Statut

Cette section documente l'état d'avancement de l'implémentation de la stratégie de test décrite ci-dessus.

### 5.1. Tests de la Génération du Fichier `.mm` (Étape A -> C)

-   **Statut :** **TERMINÉ**
-   **Description :** Une suite de tests unitaires a été implémentée dans la classe `MmGeneratorTests` au sein du projet `Argumentum.AssetConverter.Tests`. Un test de sécurité (`CreateFreemindmap_ViaPublicApi_ShouldCreateNonEmptyFile`) a été ajouté pour garantir que la méthode `CreateFreemindmap` (appelée via son point d'entrée public) produit systématiquement un fichier valide et non-vide, agissant comme un filet de sécurité contre les régressions de la logique de base de génération.
-   **Couverture des Validations :**
    -   **Conformité XML :** Le test charge le fichier `.mm` généré dans un `XDocument`, ce qui valide sa bonne formation.
    -   **Intégrité des Données :** Le test vérifie que le texte des nœuds correspond aux données sources du fichier CSV.
    -   **Présence de l'ID Unique :** Le test principal valide que l'attribut `ID` de chaque nœud XML correspond à l'identifiant stable provenant des données sources (sauf pour le nœud racine qui a un ID hardcodé).
-   **Fichiers Clés :**
    -   Test : `Generation/Converters/Argumentum.AssetConverter.Tests/MindmapGeneration/MmGeneratorTests.cs`
    -   Données de test : `Generation/Converters/Argumentum.AssetConverter.Tests/Assets/simple-fallacies.csv`

### 5.2. Tests de la Conversion `.mm` vers `.svg` (Étape C -> E)

-   **Statut :** **TERMINÉ**
-   **Description :** Un test d'intégration a été ajouté pour valider le processus de conversion de `.mm` en `.svg` via l'outil externe `freeplane.bat`. Ce test sécurise l'appel à la méthode privée `TryAutomateSvgConversion` dans les configurations de mindmap. Il s'assure que le processus externe est correctement lancé, qu'il se termine avec succès et qu'il produit un fichier `.svg` non vide.
-   **Couverture des Validations :**
    -   **Succès de l'Exécution :** Le test vérifie que la méthode retourne `true`.
    -   **Création de l'Artefact :** Le test valide que le fichier `.svg` de sortie existe et a une taille supérieure à zéro.
-   **Fichiers Clés :**
    -   Test : `Generation/Converters/Argumentum.AssetConverter.Tests/MindmapGeneration/SvgConversionIntegrationTests.cs`

### 5.3. Tests du Post-Traitement SVG (Étape E -> G)

-   **Statut :** **INCOMPLET - Le fichier snapshot de référence est manquant.**
-   **Description :** Pour verrouiller le comportement du post-traitement SVG et éviter toute régression, un test de snapshot a été implémenté. Ce test compare la sortie de la méthode `ProcessSvgFilesAsync` à une version "approuvée" du fichier (`.snapshot.svg`). Toute modification, même mineure, dans la sortie SVG provoquera l'échec du test, assurant ainsi une grande stabilité du rendu final.
-   **Couverture des Validations :**
    -   **Non-régression de la sortie :** Le test garantit que le code produit un SVG strictement identique à la version de référence validée.
-   **Fichiers Clés :**
    -   Test : `Generation/Converters/Argumentum.AssetConverter.Tests/MindmapGeneration/SvgPostProcessingTests.cs`
    -   **Note sur le snapshot :** Le code de test s'attend à trouver un fichier `sample_fallacy_map.snapshot.svg` dans un répertoire `snapshots` situé à côté de l'assembly de test (ex: `bin/Debug/netcoreapp3.1/snapshots/`). **Ce fichier est actuellement manquant.**