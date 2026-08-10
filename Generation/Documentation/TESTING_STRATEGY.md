# Stratégie de Test pour le Pipeline de Génération d'Assets

## 1. Introduction

Ce document décrit la stratégie de test pour le pipeline de génération d'assets `Argumentum.AssetConverter`. Il s'appuie sur le document d'architecture existant (`ARCHITECTURE_PIPELINE.md`) et vise à fournir une approche pragmatique pour garantir la fiabilité et la maintenabilité du pipeline à travers des tests unitaires et d'intégration.

Notre approche est guidée par les principes du SDDD (Semantic-Documentation-Driven-Design), assurant que la stratégie de test est une extension logique et pratique de la documentation architecturale.

## 2. Philosophie de Test

La stratégie de test repose sur les principes suivants :

*   **Tests Granulaires et Isolés :** Chaque composant ou étape du pipeline doit être testé de manière aussi isolée que possible. Cela facilite l'identification rapide des régressions et réduit la complexité des tests.
*   **Utilisation de Configurations Partielles :** Pour chaque scénario de test, nous utiliserons des fichiers `AssetConverterConfig.test.json` spécifiques et minimaux. Ces configurations n'activeront que les modes et sections pertinents pour le test en cours, évitant ainsi l'exécution inutile d'autres parties du pipeline.
*   **Principe du "Skip" pour les Tests d'Intégration :** Le pipeline `AssetConverter` est conçu pour ne pas refaire un travail déjà accompli. Nous tirerons parti de ce comportement en préparant le système de fichiers pour que les étapes *précédentes* à celle testée soient considérées comme "déjà faites". Cela permet de cibler des étapes spécifiques du pipeline sans avoir à exécuter l'intégralité du processus.

## 3. Tests Unitaires

Les tests unitaires se concentreront sur la validation des composants individuels et des logiques métier, sans dépendances externes lourdes.

*   **Validation de la Taxonomie CSV :**
    *   **Objectif :** La validité des fichiers CSV de taxonomie est testée par une suite de tests unitaires dédiée. L'objectif est de garantir que les données sources, qui sont le fondement du pipeline, sont lues, parsées et mappées aux objets C# correspondants (tels que `Fallacy` ou `Rule`) de manière fiable et prévisible.
    *   **Validation :** Le processus de validation vérifie plusieurs aspects :
        *   L'intégralité du chargement des enregistrements depuis un fichier CSV valide.
        *   La correspondance exacte entre les colonnes du CSV et les propriétés des objets C#.
        *   La robustesse du système face à des données mal formées, notamment par la gestion correcte des exceptions lorsqu'un en-tête crucial (comme `PK`) est manquant.
    *   **Implémentation :** La couverture vit dans `Argumentum.AssetConverter.Tests/Parsing/`, répartie en trois familles qui recouvrent les trois aspects ci-dessus :
        *   `CsvParserTests.cs` — chargement intégral depuis un CSV valide, collection vide sur CSV vide, exception sur CSV mal formé.
        *   `CsvBaseStrictContractTests.cs` — le contrat strict d'en-tête : une colonne requise (non `.Optional()`) absente lève `HeaderValidationException`, tandis qu'une colonne optionnelle absente ne lève pas. C'est le garde contre la classe de régression #216/#477, où un `.Optional()` perdu vidait un champ requis en silence.
        *   `*ClassMapRegressionTests.cs` — la correspondance colonne CSV ↔ propriété C#, une classe par entité (`Fallacy`, `Virtue`, `Rule`, `Scenario`, `DnnUiString`, `TestFallacyCard`), y compris les en-têtes français accentués.
*   **Logique de Template CardPen :**
    *   **Objectif :** Valider le moteur de rendu HTML de CardPen de manière isolée.
    *   **Validation :** Fournir un jeu de données CSV, un template Mustache et du CSS, puis comparer le HTML généré à un snapshot de référence. Cela inclut la validation des helpers personnalisés (`ifCond`, `each`) et du rendu Markdown.
*   **Génération de Fichiers `.mm` (Mind Map) :**
    *   **Objectif :** S'assurer que la logique C# du `MindMapCreator` génère des fichiers `.mm` valides et bien formés.
    *   **Validation :** Vérifier la conformité XML du fichier généré et l'intégrité des données (le texte des nœuds correspond aux données sources).
*   **Génération d'Ontologie OWL :**
    *   **Objectif :** Valider que le `OwlManager` génère des ontologies OWL structurellement cohérentes et conformes aux spécifications.
    *   **Validation :** Vérifier la présence des classes, propriétés, instances et annotations multilingues attendues.

## 4. Tests d'Intégration et Dépendances (Chaînes de Tests)

Cette section est cruciale pour valider les interactions entre les composants et les étapes du pipeline. Nous définirons des "chaînes de tests" où la sortie d'un test devient l'entrée du suivant.

Chaque test d'intégration suivra le modèle : **Setup (Arrange) -> Action (Act) -> Assertion (Assert)**.

### 4.1. Chaîne de Test : Génération PDF Complète

Cette chaîne valide le flux complet de la génération d'images à l'assemblage PDF.

*   **Test 1 : `Test Harvest` (Génération d'Images de Cartes)**
    *   **Objectif :** Valider que le `HarvestManager` génère correctement les images PNG des cartes à partir des données CSV et des templates CardPen.
    *   **Setup :**
        *   Créer un répertoire de test isolé.
        *   Fournir un `AssetConverterConfig.test.json` qui active uniquement le mode `WebBasedImageGeneration` et pointe vers un jeu de données CSV de test.
        *   S'assurer que les fichiers `.harvest.json` et les images PNG de sortie n'existent pas.
    *   **Action :** Exécuter le pipeline `Argumentum.AssetConverter` avec cette configuration.
    *   **Assertion :**
        *   Vérifier que les fichiers `.harvest.json` ont été créés et contiennent les données attendues.
        *   Vérifier que les images PNG correspondantes ont été générées dans le répertoire de sortie.
        *   Utiliser des comparaisons de hash ou des outils d'audit d'image pour valider la qualité et le contenu des images.
    *   **Artefacts de Sortie :** Fichiers `.harvest.json` et images PNG.

*   **Test 2 : `Test PDF Assembly` (Assemblage PDF)**
    *   **Objectif :** Valider que le `PdfManager` assemble correctement les images PNG en un document PDF "Print & Play".
    *   **Dépendance :** Nécessite les images PNG générées par `Test Harvest`.
    *   **Setup :**
        *   Utiliser le même répertoire de test que `Test Harvest`.
        *   Copier les images PNG générées par `Test Harvest` dans un sous-dossier `images/` du répertoire de test.
        *   Créer un `AssetConverterConfig.test.json` qui active uniquement le mode `QuestPdfGeneration` et pointe vers ces images PNG.
        *   S'assurer que le fichier PDF de sortie n'existe pas.
    *   **Action :** Exécuter le pipeline `Argumentum.AssetConverter` avec cette configuration.
    *   **Assertion :**
        *   Vérifier que le fichier PDF a été créé et n'est pas vide.
        *   Utiliser le `PdfAuditor` pour valider que le PDF contient les images attendues et que leur ordre est correct.
        *   Vérifier que les images PNG d'entrée n'ont pas été modifiées.
    *   **Artefacts de Sortie :** Fichier PDF.

*   **Test 3 : `Test PDF Validation` (Validation du PDF)**
    *   **Objectif :** Valider la structure interne et le contenu du PDF généré.
    *   **Dépendance :** Nécessite le fichier PDF généré par `Test PDF Assembly`.
    *   **Setup :**
        *   Utiliser le même répertoire de test.
        *   Créer un `AssetConverterConfig.test.json` qui active uniquement le mode `PdfAuditor` et pointe vers le PDF généré.
    *   **Action :** Exécuter le pipeline `Argumentum.AssetConverter` avec cette configuration.
    *   **Assertion :**
        *   Vérifier que l'audit du PDF se termine avec succès.
        *   Valider le nombre de pages, la présence des images, et la conformité des métadonnées.

### 4.2. Chaîne de Test : Génération de Mind Map SVG

*   **Test 1 : `Test MM Generation` (Génération du Fichier .mm)**
    *   **Objectif :** Valider que la logique C# génère un fichier `.mm` valide.
    *   **Setup :** Répertoire de test, `AssetConverterConfig.test.json` activant le mode `MindMapGeneration` et pointant vers un CSV de test.
    *   **Action :** Exécuter le pipeline.
    *   **Assertion :** Vérifier l'existence et la validité XML du fichier `.mm`.
    *   **Artefacts de Sortie :** Fichier `.mm`.

*   **Test 2 : `Test MM to SVG Conversion` (Conversion .mm en SVG)**
    *   **Objectif :** Valider que le processus externe Freeplane convertit correctement le fichier `.mm` en SVG.
    *   **Dépendance :** Nécessite le fichier `.mm` généré par `Test MM Generation`.
    *   **Setup :** Répertoire de test, `AssetConverterConfig.test.json` activant le mode `MindMapGeneration` et pointant vers le `.mm` généré.
    *   **Action :** Exécuter le pipeline.
    *   **Assertion :** Vérifier l'existence et la validité minimale du fichier SVG (balise `<svg>`).
    *   **Artefacts de Sortie :** Fichier SVG.

*   **Test 3 : `Test SVG Post-Processing` (Post-Traitement SVG)**
    *   **Objectif :** Valider que la logique de post-traitement SVG (y compris la "disambiguation") fonctionne correctement.
    *   **Dépendance :** Nécessite le fichier SVG généré par `Test MM to SVG Conversion`.
    *   **Setup :** Répertoire de test, `AssetConverterConfig.test.json` activant le mode `MindMapGeneration` et pointant vers le SVG généré.
    *   **Action :** Exécuter le pipeline.
    *   **Assertion :** Utiliser des tests de snapshot pour comparer le SVG post-traité à un fichier de référence, garantissant la non-régression de la structure et des attributs.
    *   **Artefacts de Sortie :** Fichier SVG final.

## 5. Tests sur les Points de Fragilité

Nous allons cibler spécifiquement les risques identifiés dans le document d'architecture.

*   **Test de Non-Régression pour la Logique de "Disambiguation" SVG :**
    *   **Objectif :** Détecter toute régression dans la logique de liaison entre les données et les nœuds SVG des mind maps.
    *   **Stratégie :** Utiliser des tests de snapshot. Un fichier `.mm` et un fichier `.svg` de référence (générés par une version connue de Freeplane) seront utilisés. Le test exécutera la phase de post-traitement SVG et comparera le résultat au snapshot. Toute modification inattendue de la structure SVG entraînera un échec.
*   **Test de Validation du Comportement du `lock` pour la Génération PDF :**
    *   **Objectif :** S'assurer que le mécanisme de `lock` autour de `QuestPDF` fonctionne comme prévu, empêchant les *race conditions* et garantissant la génération séquentielle des PDF.
    *   **Stratégie :** Créer un test d'intégration qui tente de générer plusieurs documents PDF en parallèle (en simulant des appels concurrents au `PdfManager`). Le test devra vérifier que les documents sont générés correctement et qu'aucune corruption ou erreur liée à la concurrence ne se produit. Bien que le `lock` rende la génération séquentielle, ce test validera que le contournement est efficace.

## 6. Environnement de Test

Pour lancer ces tests, l'environnement doit être préparé comme suit :

*   **Répertoires de Test Isolés :** Chaque suite de tests (ou même chaque test d'intégration complexe) doit opérer dans un répertoire temporaire unique et isolé. Cela garantit que les artefacts générés par un test n'interfèrent pas avec les autres. Ces répertoires doivent être nettoyés après l'exécution des tests.
*   **Fichiers de Configuration de Test (`AssetConverterConfig.test.json`) :** Créer des fichiers de configuration spécifiques pour chaque scénario de test, activant uniquement les modes et sections nécessaires. Ces fichiers doivent être minimaux et pointer vers les données de test appropriées.
*   **Données de Test :**
    *   **CSV :** Fournir des jeux de données CSV représentatifs (cas nominaux, cas limites, cas d'erreur) pour les tests unitaires et d'intégration.
    *   **Templates CardPen :** Inclure des templates HTML/CSS/JS de référence pour CardPen.
    *   **Images PNG de Référence :** Pour les tests d'assemblage PDF, disposer d'un jeu d'images PNG pré-générées (simulant la sortie du `HarvestManager`).
    *   **Fichiers `.mm` et SVG de Référence :** Pour les tests de mind map et de "disambiguation" SVG, avoir des fichiers `.mm` et SVG de référence générés par une version stable de Freeplane.
*   **Outils Externes :** S'assurer que Playwright et Freeplane sont installés et configurés correctement sur l'environnement de CI/CD et de développement local. Le chemin vers Freeplane (`FreeplanePath`) doit être correctement défini dans les configurations de test.
*   **Framework de Test :** Utiliser xUnit avec des bibliothèques d'assertion comme FluentAssertions et de mocking comme NSubstitute.