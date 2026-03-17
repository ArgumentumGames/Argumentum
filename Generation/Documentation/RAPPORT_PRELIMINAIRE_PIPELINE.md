# Rapport Préliminaire : Analyse Sémantique du Pipeline de Génération

Ce document synthétise les 5 découvertes fondamentales issues de la phase de recherche sémantique. Ces points constituent le socle de la compréhension du système de génération d'assets.

### 1. Architecture Tripartite du Pipeline Principal

Le processus de génération des cartes et des PDF est orchestré par l'application console .NET `Argumentum.AssetConverter`. Il suit un pipeline clair en trois étapes distinctes :
   - **1. Configuration :** Lecture et interprétation de fichiers de configuration JSON qui définissent l'ensemble du processus.
   - **2. "Harvesting" (Récolte d'images) :** Utilisation d'un navigateur web automatisé pour générer des images de cartes individuelles.
   - **3. Assemblage PDF :** Combination des images récoltées pour créer les documents PDF finaux prêts à l'impression.

### 2. Rôle Central et Automatisation de "CardPen"

L'outil `CardPen` est une application web (HTML/JS) qui agit comme un moteur de templating. Il n'est pas utilisé manuellement. Le composant C# `HarvestManager.cs` automatise un navigateur (via la bibliothèque Playwright) pour :
   - Charger l'application `CardPen`.
   - Injecter les données d'une carte (issues des CSV) dans les gabarits Mustache/Handlebars.
   - Capturer l'image HTML résultante directement en Base64, sans passer par une capture d'écran traditionnelle.

### 3. Criticité et Complexité de la Configuration JSON

Le comportement du pipeline est entièrement dicté par des fichiers de configuration, notamment `AssetConverterConfig.json` et les fichiers de définition de `CardSet` (par ex. `Argumentum_Rules_fr.json`). Ces fichiers contrôlent :
   - Les **sources de données** (`DataSets`), qui peuvent être des fichiers CSV ou des répertoires de fichiers (`.md`).
   - Les **ensembles de cartes** (`CardSets`), qui lient les données aux gabarits.
   - Les **documents de sortie** (`CardSetDocuments`), qui définissent les PDF à générer.
   
Une découverte critique est l'existence d'un **"repli silencieux"** : une configuration incorrecte ou manquante ne génère pas nécessairement d'erreur, mais peut entraîner un comportement par défaut inattendu, ce qui constitue un risque de bug majeur.

### 4. Chaîne d'Outils Spécifique pour la Génération PDF

La phase finale d'assemblage des PDF est gérée par le `PdfManager.cs` et repose sur une combinaison de deux bibliothèques externes clés :
   - **ImageMagick :** Utilisée pour le traitement des images, notamment pour décoder les données Base64 et pour la conversion colorimétrique (RVB vers CMJN).
   - **QuestPDF :** Utilisée pour la composition des documents PDF, la mise en page des cartes sur les planches (ex: format A4 "Print & Play"), et la génération du fichier final.

### 5. Existence d'un Pipeline Parallèle pour les Mindmaps

Un pipeline de génération distinct et non-trivial existe pour les cartes mentales (Mindmaps). Son flux est le suivant :
   - **Données sources (CSV) -> Logique C# -> Fichier XML (`.mm`)**
   - Le fichier `.mm` est ensuite traité par un outil externe, **Freeplane** (via `freeplane.bat`), pour être converti en **SVG**.

Ce pipeline a connu des problèmes historiques significatifs dus à une logique complexe de "devinette" (`disambiguation`) pour faire correspondre les nœuds SVG aux données originales. Une refactorisation a été entreprise pour utiliser des identifiants (`ID`) stables de bout en bout, ce qui est une information cruciale pour comprendre sa conception actuelle et sa fragilité passée.