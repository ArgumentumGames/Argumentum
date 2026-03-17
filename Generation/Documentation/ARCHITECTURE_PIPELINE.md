# Architecture du Pipeline de Génération d'Assets

## 1. Introduction

Ce document fournit une description détaillée de l'architecture technique du pipeline `Argumentum.AssetConverter`. Son objectif est de servir de référence pour la maintenance, l'évolution et la création de tests unitaires et d'intégration.

Le pipeline est une application console .NET 8 qui orchestre un ensemble de processus et de bibliothèques pour transformer des données brutes (principalement des fichiers CSV) en assets finaux (PDF "Print & Play", cartes mentales SVG, ontologies OWL, etc.).

### Diagramme de Flux Global

```mermaid
graph TD
    subgraph " "
        direction LR
        A[Données Sources<br>(CSVs, Google Sheets)] --> B{Argumentum.AssetConverter}
    end

    subgraph "Pipeline de Génération"
        direction TB
        B --> C{1. Configuration<br>AssetConverterConfig.json}
        C --> D{2. Harvesting<br>Playwright & CardPen}
        D --> E[Images de cartes (.png)]
        E --> F{3. Assemblage}
    end

    subgraph "Sorties"
        direction TB
        F --> G[PDFs Print & Play]
        F --> H[Cartes Mentales SVG]
        F --> I[Ontologies OWL]
        F --> J[Rapports de Validation]
    end

    subgraph "Validation Continue"
        G --> K{PdfAuditor}
        A --> L{Validateur de Taxonomie}
        K --> J
        L --> J
    end

```

## 2. Étape 1 : Le Système de Configuration (Le Cerveau)

Le cœur du pipeline est son système de configuration. Une mauvaise compréhension de ce système est la source de nombreuses erreurs historiques.

### 2.1. Le Rôle Central de `AssetConverterConfig.json`

Le fichier `AssetConverterConfig.json` est le point de contrôle unique pour une exécution donnée. Il définit :
- **Les modes actifs** (`Mode`) : Quelles actions effectuer (`WebBasedImageGeneration`, `QuestPdfGeneration`, `MindMapGeneration`, `PdfAuditor`, etc.).
- **Les sources de données** (`DataSets`) : Pointeurs vers les fichiers CSV.
- **Les ensembles de cartes** (`CardSets`) : Définit les templates (HTML/CSS/JS via CardPen) pour chaque type de carte.
- **Les documents à produire** (`CardSetDocuments`) : Décrit les documents PDF à assembler à partir des images générées.
- Les paramètres de **localisation** (`LocalizationConfig`) : Gère les traductions des champs de données et des templates.
- Les paramètres de **parallélisation** (`WebBasedGeneratorConfig.MaxDegreeOfParallelism*`) : Contrôle le nombre d'instances de navigateur ou de threads pour la génération d'images et de documents.
- Les configurations spécifiques à chaque **mode** (ex: `WebBasedGeneratorConfig`, `FallacyMindMapCreatorConfig`, `PdfAuditorConfig`) : Chaque mode a sa propre section de configuration détaillée, permettant un contrôle granulaire de son comportement.

### 2.2. La Configuration par Défaut : Source de Vérité et Mécanisme de Repli

Pour garantir la robustesse, le système implémente un mécanisme de configuration par défaut puissant :

1.  **Source de Vérité** : La classe `AssetConverterConfig.cs` contient la configuration "d'usine" complète et fonctionnelle.
2.  **Génération Automatique** : Si le fichier `AssetConverterConfig.json` est **absent** au démarrage, l'application le **crée physiquement** en sérialisant la configuration par défaut depuis `AssetConverterConfig.cs`.
3.  **Point de départ** : L'application utilise ensuite ce fichier JSON nouvellement créé (ou un fichier existant) pour son exécution.

Ce mécanisme assure que l'utilisateur dispose toujours d'un fichier de configuration complet et valide comme point de départ.

### 2.4. Structure Détaillée de `AssetConverterConfig.json`

Le fichier `AssetConverterConfig.json` est structuré en plusieurs sections clés, chacune gérant un aspect spécifique du pipeline :

-   **`Mode`** : Une chaîne de caractères (ou un tableau) qui spécifie les modes de conversion actifs. Les modes sont des drapeaux (`flags`) qui peuvent être combinés (ex: `"WebBasedImageGeneration, QuestPdfGeneration, Mindmapper"`). Si un mode n'est pas listé, il ne sera pas exécuté.
-   **`DataSets`** : Une liste d'objets `DataSetInfo` qui définissent les sources de données brutes. Chaque `DataSet` spécifie :
    -   `Name` : Nom unique du jeu de données (utilisé pour référencer ce jeu dans les `CardSets`).
    -   `ReleaseFilePath` / `DebugFilePath` : Chemins vers les fichiers CSV (ou répertoires) en fonction du mode d'exécution (Release/Debug). Supporte les URLs GitHub pour les fichiers distants.
    -   `IsDirectory` : Indique si le chemin pointe vers un répertoire de fichiers (ex: pour les fichiers Markdown).
    -   `CsvType` : Le type C# complet de l'entité de données correspondante (ex: `"Argumentum.AssetConverter.Entities.Rule, Argumentum.AssetConverter"`). Crucial pour le parsing et la réflexion.
-   **`LocalizationConfig`** : Gère les paramètres de localisation et de traduction.
    -   `Enabled` : Active ou désactive la localisation.
    -   `DefaultLanguage` : Langue par défaut (ex: `"fr"`).
    -   `CardSetLocalizations` : Règles de conversion pour les champs de cartes spécifiques à chaque langue.
    -   `MindMapLocalization` : Règles de conversion pour les champs des mind maps.
-   **`WebBasedGeneratorConfig`** : Paramètres pour la phase de "Harvesting" via Playwright et CardPen.
    -   `HeadLessBrowser` : Exécute le navigateur en mode sans tête (sans interface graphique).
    -   `EnableParallelism` : Active la parallélisation pour la génération d'images.
    -   `MaxDegreeOfParallelismCardpen` / `MaxDegreeOfParallelismCardpenTranslations` / `MaxDegreeOfParallelismImages` / `MaxDegreeOfParallelismImageTranslations` / `MaxDegreeOfParallelismDocuments` : Contrôlent le nombre maximal d'opérations parallèles pour chaque sous-étape.
    -   `ReleaseCardpenUrl` / `LocalCardpenUrl` / `UseLocalCardpen` : Définissent l'URL de l'application CardPen (distante ou locale).
    -   `CardSets` : Liste des `CardSet` à générer. Chaque `CardSet` contient :
        -   `Name` : Nom unique du set de cartes.
        -   `IsEnabled` : Active ou désactive la génération de ce set.
        -   `FaceCardSetInfo` / `BackCardSetInfo` : Configurations pour le recto et le verso des cartes, incluant le `DataSet` source, les filtres CSV (`CsvFilterField`, `CsvFilterValues`), le chemin vers le template JSON (`JsonFilePathRelease`/`Debug`), le DPI (`Dpi`) et le nombre de lignes (`RowsetNb`).
-   **`CardSetDocuments`** : Liste des documents PDF finaux à assembler. Chaque `CardSetDocument` spécifie :
    -   `DocumentName` : Nom du fichier PDF de sortie.
    -   `CardSets` : Liste des `CardSet` à inclure dans ce document, avec le nombre de copies (`NbCopies`) et des options de conversion (ex: `ConvertToCmyk`).
    -   `DocumentFormat` : Format de mise en page (ex: `"PrintAndPlay"`, `"BackFirstOneDocPerBack"`).
    -   `PageSize` : Taille de la page (ex: `"A4"`).
    -   `Header` : Image d'en-tête.
-   **`FallacyMindMapCreatorConfig` / `VirtueMindMapCreatorConfig`** : Configurations spécifiques pour la génération des mind maps.
    -   `DocumentConfigs` : Liste des configurations pour chaque mind map, incluant le format (`Format`), les expressions pour les champs (`TitleExpression`, `DescriptionExpression`, etc.), les tailles de police (`FontSizes`), les couleurs (`Colors`), et les configurations SVG (`SVGMaps`).
    -   `InsertCardsThumbnails` : Indique si des miniatures de cartes doivent être insérées dans la mind map.
-   **`OwlGeneratorConfig`** : Configuration pour la génération des ontologies OWL.
-   **`TaxonomyValidatorConfig` / `OwlValidatorConfig` / `CardValidatorConfig` / `ContinuousValidationConfig` / `TranslationCoverageConfig`** : Configurations pour les différents modules de validation et de rapport.
-   **`ParallelismOptimizerConfig`** : Gère l'optimisation dynamique du parallélisme.
-   **`PdfAuditorConfig`** : Configuration pour l'audit des PDF générés.
-   **`FreeplanePath`** : Chemin vers l'exécutable de Freeplane (pour la génération des mind maps).
-   **`OverwriteExistingDocs` / `OverwriteExistingHtmlMaps`** : Drapeaux pour forcer l'écrasement des fichiers existants.
-   **`EnableSVGPrompt` / `AsynchronousPipeline`** : Options avancées pour le pipeline.

### 2.5. Impact des Configurations Partielles et Scénarios de Test

La flexibilité de `AssetConverterConfig.json` permet de définir des configurations partielles pour des scénarios spécifiques, ce qui est crucial pour les tests :

-   **Exécution Ciblée** : Pour tester uniquement la génération d'images, on peut créer un `AssetConverterConfig.json` qui ne contient que le mode `WebBasedImageGeneration` et les `CardSets` pertinents. Tous les autres modes et sections non nécessaires peuvent être omis.
-   **Isolation des Composants** : Cette approche permet d'isoler les composants du pipeline. Par exemple, pour tester le `PdfManager`, on peut désactiver le `WebBasedImageGeneration` et fournir des images pré-générées, en configurant uniquement la section `CardSetDocuments`.
-   **Débogage Facilité** : En réduisant la portée de l'exécution, les configurations partielles facilitent le débogage en éliminant les interférences des autres étapes du pipeline.
-   **Tests d'Intégration Granulaires** : Comme mentionné dans la section 5, cette capacité est exploitée pour créer des tests d'intégration qui valident des étapes spécifiques du pipeline sans avoir à exécuter l'intégralité du processus.

### 2.3. Instabilité Historique et Points de Vigilance

L'analyse des commits a révélé une instabilité chronique de ce système. Les développeurs doivent être conscients des points suivants :

-   **Le piège du "Repli Silencieux"** : Une régression passée a vu la configuration par défaut en C# remplacée par des listes vides. En l'absence d'un `AssetConverterConfig.json`, l'application chargeait une configuration vide, ne faisait rien, et se terminait "avec succès" sans produire d'erreur. Ce problème est corrigé, mais il illustre la criticité de la configuration par défaut.
-   **Changements de Librairies de Sérialisation** : Le projet a historiquement utilisé `Utf8Json`, `System.Text.Json` et implicitement `Newtonsoft.Json`. Bien que stabilisé sur `System.Text.Json`, des incompatibilités subtiles pourraient réapparaître lors de mises à jour majeures.

**Recommandation :** Pour une exécution de test ou de développement, toujours partir d'un `AssetConverterConfig.json` généré par l'application, puis l'épurer pour ne conserver que les sections nécessaires.

## 3. Étape 2 : La Phase de "Harvesting" (Les Yeux & les Mains)

Cette phase est responsable de la création des images individuelles pour chaque carte. Elle utilise une combinaison d'outils web et d'automatisation.

### 3.1. CardPen : Le Moteur de Rendu HTML

Le rendu visuel de chaque carte est délégué à **CardPen**, une application web interne qui fonctionne comme un moteur de template.

-   **Templates** : La structure HTML est définie dans les fichiers de configuration JSON (ex: `Argumentum_Fallacies_Face_fr.json`) via une clé `"mustache"`.
-   **Moteur** : Il utilise une syntaxe **Mustache/Handlebars** enrichie de helpers personnalisés (`ifCond`, `each`) pour permettre des logiques complexes directement dans le template.
-   **Données** : Il prend en entrée des données CSV et les rend accessibles dans les templates, soit ligne par ligne (`{{NomDeLaColonne}}`), soit en tant qu'ensemble complet de données (`{{#each rowset}}`).
-   **Markdown** : Il interprète le Markdown présent dans les données CSV et le convertit en HTML avant de l'injecter dans le template.

### 3.2. Playwright : L'Automatisation du Navigateur et le Flux de Données

Le pipeline utilise **Playwright** pour automatiser un navigateur (Chromium) et orchestrer la génération d'images. Le `HarvestManager.cs` est le composant central de cette étape.

**Flux de Données Détaillé :**

1.  **Initialisation du Navigateur** : Le `HarvestManager` (via sa propriété `Browser`) démarre une instance de navigateur Chromium en mode headless (par défaut, configurable via `HeadLessBrowser` dans `AssetConverterConfig.json`). Playwright est automatiquement téléchargé et installé si nécessaire.
2.  **Préparation des Données de Cartes** : Pour chaque `CardSet` défini dans la configuration (`WebBasedGeneratorConfig.CardSets`), le `HarvestManager` :
    *   Récupère les données brutes depuis le `DataSet` CSV correspondant (local ou distant via URL GitHub).
    *   Applique des filtres si `CsvFilterField` et `CsvFilterValues` sont spécifiés dans `CardSetInfo`.
    *   Charge le template JSON (`JsonFilePathRelease`/`Debug`) pour le recto (`FaceCardSetInfo`) et/ou le verso (`BackCardSetInfo`) de la carte.
    *   Si la localisation est activée, les données sont traduites en fonction des `CardSetLocalizations` définies dans `LocalizationConfig`.
    *   Les données CSV sont injectées dans le template JSON du CardPen.
3.  **Chargement de CardPen et Injection des Données** :
    *   Une nouvelle page de navigateur est ouverte (ou réutilisée depuis un pool de pages pour la parallélisation).
    *   Le navigateur navigue vers l'URL de CardPen (configurable via `ReleaseCardpenUrl`/`LocalCardpenUrl`).
    *   Le `HarvestManager` sérialise le `CardSetDocument` (contenant le template HTML/CSS/JS et les données CSV) en JSON.
    *   Ce JSON est ensuite "uploadé" dans CardPen via une interaction simulée avec un champ d'entrée de fichier (`#import` dans CardPen).
4.  **Rendu et Capture d'Image** :
    *   CardPen, côté client (dans le navigateur), prend le JSON injecté, le dé-sérialise, et utilise son moteur Mustache/Handlebars pour rendre le HTML de la carte.
    *   Le `HarvestManager` attend que l'élément `<card>` soit rendu dans l'iframe de CardPen (`#cpOutput`).
    *   Une fois le rendu stable, le `HarvestManager` clique sur le bouton "Generate" de CardPen (`#generateButton`).
    *   CardPen génère les images des cartes et les encode en Base64.
    *   Le `HarvestManager` extrait les attributs `src` (contenant les images Base64) des balises `<img>` générées par CardPen.
    *   Ces images Base64 sont décodées et stockées en mémoire dans un objet `CardPenHarvest`.
5.  **Sauvegarde du Harvest** : L'objet `CardPenHarvest` (contenant les images de toutes les cartes du `CardSet`) est sérialisé en un fichier `.harvest.json` local (ex: `Target/Harvest/Rules_fr.harvest.json`). Ce fichier sert de cache pour les exécutions futures et de point d'entrée pour la phase d'assemblage PDF.

**Gestion des Templates et Points de Défaillance Potentiels :**

-   **Templates CardPen (`.json` avec `mustache`):**
    -   **Structure :** Ces fichiers JSON définissent la structure HTML/CSS/JS de chaque type de carte. La clé `"mustache"` contient le template principal.
    -   **Helpers Personnalisés :** CardPen utilise des helpers Mustache/Handlebars (ex: `{{#ifCond}}`, `{{#each rowset}}`) pour des logiques conditionnelles et des boucles. Une erreur dans ces helpers ou une incompatibilité de syntaxe peut entraîner un rendu incorrect ou incomplet.
    -   **Markdown :** Le Markdown dans les données CSV est converti en HTML par CardPen. Des syntaxes Markdown non supportées ou des erreurs peuvent briser le rendu.
    -   **Dépendances CSS/JS :** Les templates peuvent dépendre de fichiers CSS ou JavaScript externes. Des chemins incorrects ou des erreurs dans ces fichiers peuvent empêcher le rendu correct des cartes.
    -   **DPI Inapproprié :** Un DPI mal configuré dans `CardSetInfo.Dpi` peut entraîner des images de mauvaise qualité ou des problèmes de performance.

### 3.3. Problèmes de Concurrence et Résolution

L'historique du projet montre que cette phase a été une source majeure de bugs de concurrence, principalement en raison de l'automatisation du navigateur et de la parallélisation.

-   **`ImageFileGenerator` Stateful (Corrigé)** : Une version antérieure de `ImageFileGenerator` (un composant interne du `HarvestManager`) utilisait une variable partagée (`currentCard`) dans une boucle parallèle. Cela provoquait des *race conditions* et des `KeyNotFoundException` intermittentes, car plusieurs threads tentaient de modifier la même ressource simultanément.
    -   **Solution (Commit `fc70138d`)** : La logique a été réécrite pour être **stateless**. Chaque carte est désormais une unité de travail atomique, traitée indépendamment. Le `Logger` a également été rendu thread-safe (`lock`) pour faciliter le diagnostic des problèmes de concurrence.
-   **`HarvestManager` Massivement Parallèle** : Le `HarvestManager` utilise `Parallel.ForEachAsync` avec un pool de pages de navigateur (`ConcurrentStack<IPage> Freepages`) pour paralléliser la génération d'images (`MaxDegreeOfParallelismCardpen`, `MaxDegreeOfParallelismCardpenTranslations`). Bien que cette approche soit plus stable après la correction de `ImageFileGenerator`, elle reste sensible aux problèmes de ressources système (mémoire, CPU) et aux instabilités de Playwright.
    -   **Points de Vigilance :**
        -   **Fuites de Mémoire Navigateur :** Chaque instance de navigateur (même headless) consomme de la mémoire. Un grand nombre de pages parallèles ou des sessions de longue durée peuvent entraîner des fuites de mémoire et des plantages.
        -   **Timeouts Playwright :** Les opérations Playwright (navigation, attente d'éléments) peuvent échouer en raison de timeouts si le rendu de CardPen est lent ou si le réseau est instable.
        -   **Dépendances Externes :** La stabilité de cette phase dépend de la robustesse de Playwright et de Chromium. Des mises à jour de ces outils peuvent introduire des régressions.

## 4. Étape 3 : La Phase de Génération d'Assets (L'Atelier)

Une fois les images individuelles récoltées, cette phase les assemble dans les différents formats de sortie finaux.

### 4.1. Générateur de PDF (`PdfManager`)

Ce composant assemble les images PNG en documents PDF "Print & Play".

-   **Logique de Mise en Page Complexe** : Le `PdfManager` contient une logique de mise en page manuelle très complexe pour calculer le positionnement des cartes, gérer les marges, les fonds perdus (bleed) et surtout l'inversion des pages pour l'impression recto-verso (pour l'impression recto-verso des cartes). Cette logique est encapsulée dans la classe `PrintAndPlayDocument` et est hautement spécifique aux dimensions des cartes et aux formats d'impression.
-   **Bug de Concurrence avec QuestPDF (Commit `6723d628`)** : L'analyse historique a révélé que la bibliothèque `QuestPDF`, utilisée pour la génération des documents PDF, n'était **pas thread-safe** dans les versions utilisées précédemment. Les tentatives de paralléliser la génération de plusieurs documents PDF simultanément provoquaient des plantages aléatoires et des corruptions de fichiers, en raison de *race conditions* internes à la bibliothèque.
    -   **Solution (Contournement Actuel)** : Pour résoudre ce problème critique, un mécanisme de `lock` (`lock(pdfLock)`) a été implémenté autour de l'appel à la méthode `GenerateDocument` de `QuestPDF` dans le `PdfManager`. Ce `lock` garantit qu'une seule opération de génération de document QuestPDF peut s'exécuter à la fois, rendant le processus séquentiel.
    -   **Implications pour les Tests et la Performance** :
        -   **Suppression de la Parallélisation** : Ce `lock` annule de fait tout bénéfice de parallélisation pour la phase de génération PDF, même si le `HarvestManager` produit des images en parallèle. La génération de plusieurs documents PDF est désormais une étape séquentielle, ce qui représente une **régression de performance** significative pour les gros volumes de documents.
        -   **Dette Technique** : Il s'agit d'un **contournement temporaire** et non d'une solution architecturale propre. La dépendance à ce `lock` est une dette technique importante qui doit être remboursée.
        -   **Impact sur les Tests** : Lors de l'écriture de tests, il est crucial de se rappeler que la génération PDF est séquentielle. Les tests d'intégration qui impliquent la création de plusieurs PDF ne bénéficieront pas de la parallélisation et devront être conçus en conséquence (ex: en limitant le nombre de documents générés par test ou en augmentant les timeouts).
        -   **Surveillance des Mises à Jour QuestPDF** : Il est impératif de surveiller les futures versions de `QuestPDF` pour vérifier si le problème de thread-safety a été résolu. Si c'est le cas, le `lock` pourra être retiré, permettant de restaurer la parallélisation et d'améliorer les performances.
-   **Dépendance à une Ancienne Version** : Le projet a dû revenir à une version plus ancienne de `QuestPDF` (`2023.12.0`) pour résoudre des problèmes de compatibilité et de stabilité. Cette dépendance à une version non à jour représente un risque de sécurité (vulnérabilités non corrigées) et de maintenance (difficulté à intégrer de nouvelles fonctionnalités ou à résoudre des bugs).
    -   **Action Recommandée** : Planifier une migration vers la dernière version stable de `QuestPDF` dès que possible, en s'assurant que les problèmes de thread-safety sont résolus ou en mettant en place une stratégie de gestion de la concurrence plus robuste.

### 4.2. Générateur de Mind Map (`MindMapCreator`, `freeplane.bat`)

Ce composant génère des cartes mentales au format SVG à partir de fichiers `.mm` (Freeplane/Freemind).

-   **Processus Externe** : La génération des fichiers `.mm` (format natif de Freeplane) est gérée par la logique C# du `MindMapCreator`. La conversion de ces fichiers `.mm` en SVG s'appuie sur l'exécution d'un **processus externe** via le script `freeplane.bat` (ou `freeplane.sh` sur Linux/macOS), qui lance l'application Freeplane en ligne de commande pour effectuer la conversion. Le chemin vers cet exécutable est configuré via `FreeplanePath` dans `AssetConverterConfig.json`.
-   **Logique de "Disambiguation" SVG (Point de Fragilité Majeur)** : Après la conversion initiale du `.mm` en SVG par Freeplane, le pipeline tente de post-traiter le fichier SVG généré pour y injecter des métadonnées supplémentaires ou modifier des attributs. Cette étape est cruciale pour rendre le SVG interactif ou pour lier les éléments graphiques aux données sources.
    -   **Le Problème de la "Magie"** : Pour établir le lien entre un élément de donnée (ex: une `Fallacy` avec son `Path` unique) et son nœud graphique correspondant dans le fichier SVG, le code utilise une **heuristique de "disambiguation" extrêmement complexe et fragile**. Cette logique repose sur des suppositions implicites concernant la structure interne du SVG généré par Freeplane, notamment :
        -   **Contenu Textuel** : Elle tente de faire correspondre le texte des nœuds SVG avec les titres des entités de données.
        -   **Positionnement** : Elle peut se baser sur des coordonnées ou des relations spatiales entre les éléments SVG.
        -   **Absence d'ID Stables** : Le problème fondamental est que Freeplane n'exporte pas les mind maps avec des identifiants uniques et stables pour chaque nœud qui pourraient être directement liés aux IDs des données sources. Le code doit donc "deviner" ces correspondances.
    -   **Dépendances et Fragilité** :
        -   **Dépendance à Freeplane** : La logique de post-traitement est intrinsèquement liée à la version et au comportement d'export de Freeplane. Toute mise à jour de Freeplane qui modifie la structure interne du SVG (ex: changement de balises, d'attributs, d'ordre des éléments) cassera cette logique.
        -   **Retouches Manuelles** : Si un utilisateur modifie manuellement un fichier `.mm` dans Freeplane, cela peut altérer la structure du SVG exporté et rendre la logique de "disambiguation" inopérante.
        -   **Risque (Commit `fc62618c`)** : Ce mécanisme est une **bombe à retardement**. Il a déjà été la source de bugs difficiles à diagnostiquer (comme le montre le commit `fc62618c`). La moindre déviation par rapport au format SVG attendu entraînera des erreurs silencieuses ou des SVG corrompus. C'est un "code smell" majeur.
    -   **Solution à Long Terme** : Idéalement, il faudrait trouver un moyen d'exporter les mind maps avec des identifiants stables et programmatiques depuis Freeplane/Freemind, ou envisager un autre outil de cartographie mentale qui offre cette fonctionnalité. Cela permettrait une liaison déterministe entre les données et le SVG.
    -   **Solution à Court Terme pour les Tests** : Isoler cette logique de "disambiguation" dans un composant séparé et la couvrir avec des tests de caractérisation robustes. Ces tests devraient utiliser un fichier `.mm` et un fichier `.svg` de référence (générés par une version connue de Freeplane) pour détecter immédiatement toute régression due à des changements dans Freeplane ou dans la logique de post-traitement.

### 4.3. Générateur d'Ontologie OWL (`OwlManager`)

Ce composant est responsable de la génération des ontologies au format OWL (Web Ontology Language) à partir des données sources. Il utilise la bibliothèque `OWLSharp`.

-   **Objectif** : Créer une représentation formelle des connaissances du domaine (ex: les sophismes, les vertus) sous forme d'ontologie, permettant des requêtes sémantiques et des inférences logiques.
-   **Dépendance** : S'appuie sur la bibliothèque `OWLSharp` pour la manipulation et la sérialisation des ontologies.
-   **Configuration** : La section `OwlGeneratorConfig` dans `AssetConverterConfig.json` définit les paramètres de génération, tels que le namespace de l'ontologie (`OntologyNamespace`), les références à des ontologies externes (`ExternalReferenceOntologyNamespaceURI`, `ExternalReferenceOntologyUri`), et les métadonnées (commentaires, créateur, version).
-   **Flux de Données** : Le `OwlManager` lit les données structurées (principalement les taxonomies de sophismes et de vertus) et les transforme en classes, propriétés et instances OWL, en respectant la hiérarchie définie dans les données sources.
-   **Validation** : L'ontologie générée peut être validée par le `OwlValidator` pour s'assurer de sa cohérence structurelle, de la présence des annotations multilingues et de la conformité aux mappings AIF (Argument Interchange Format).

### 4.4. Auditeur de PDF (`PdfAuditor`)

Ce module de validation est un "client" du `PdfManager`.
- **Objectif** : Vérifier que le contenu visuel d'un PDF généré correspond exactement aux images sources attendues.
- **Fonctionnement** : Il extrait les images du PDF, recalcule l'ordre attendu, et compare les hashs SHA256 des images extraites avec les images sources.
- **Activation** : Se fait via le `Mode` et la section `PdfAuditorConfig` dans `AssetConverterConfig.json`.

## 5. Architecture pour les Tests Unitaires et d'Intégration

L'objectif de cette section est de fournir une stratégie claire pour tester les différentes briques du pipeline de manière isolée et fiable.

### 5.1. Tester chaque brique séparément

Le principal défi du test de ce pipeline est son caractère monolithique et ses dépendances à des processus externes (Playwright, Freeplane). La stratégie consiste à découpler les composants.

### 5.2. Tests d'Intégration Granulaires et le Principe du "Skip"

En complément des tests unitaires, il est crucial de tester les interactions entre les composants et les étapes du pipeline. L'approche monolithique (tout regénérer à chaque fois) est trop lente et inefficace pour le développement et la CI/CD. Le pipeline `AssetConverter` est conçu pour ne pas refaire un travail déjà accompli, en vérifiant systématiquement si les fichiers de sortie intermédiaires (ex: `.harvest.json`, images PNG) existent avant de lancer une étape coûteuse.

Notre stratégie de test tire parti de ce comportement : **pour tester une étape spécifique, nous allons préparer le système de fichiers pour que toutes les étapes *précédentes* soient considérées comme "déjà faites"**.

Chaque test d'intégration suivra donc ce modèle :

1.  **Setup (Arrange)** :
    *   Définir un répertoire de test unique et isolé pour éviter les interférences.
    *   Copier ou créer les **artefacts d'entrée** nécessaires à l'étape testée (ex: les fichiers `.harvest.json` pour l'étape d'assemblage PDF, ou les images PNG pour le `PdfManager`). Ces artefacts peuvent être des fichiers de référence pré-générés.
    *   S'assurer que les **artefacts de sortie** de l'étape testée sont **absents** (pour forcer leur génération).
    *   Créer un fichier `AssetConverterConfig.test.json` minimal qui active uniquement le mode de conversion pertinent pour le test et pointe vers les données et artefacts d'entrée du répertoire de test.
2.  **Action (Act)** :
    *   Exécuter le pipeline `Argumentum.AssetConverter` en lui passant le chemin vers le fichier `AssetConverterConfig.test.json` spécifique au test.
3.  **Assertion (Assert)** :
    *   Vérifier que l'artefact de sortie a été correctement créé dans le répertoire de test.
    *   Vérifier que les artefacts des autres étapes (non testées) n'ont pas été modifiés ou générés inutilement.
    *   Utiliser des outils d'audit (comme le `PdfAuditor` pour les PDF) ou des comparaisons de hash pour valider le contenu des fichiers générés.

**Exemple Concret : Tester Uniquement l'Étape "Génération PDF"**

**Objectif :** Valider que l'assemblage PDF fonctionne correctement, sans régénérer les images PNG via Playwright.

**Implémentation du test :**

1.  **Setup :**
    *   Créer un répertoire de test temporaire : `TestOutput/PdfOnlyTest/`.
    *   Dans `TestOutput/PdfOnlyTest/`, simuler la phase de "Harvesting" en plaçant des fichiers `.harvest.json` et les images PNG correspondantes (ex: `Target/Harvest/card-1.png`, `card-2.png`) qui seraient normalement générés par le `HarvestManager`. Ces images peuvent être des copies d'un jeu de référence.
    *   S'assurer que le fichier PDF de sortie attendu (ex: `TestOutput/PdfOnlyTest/MyDocument.pdf`) n'existe pas.
    *   Créer un `AssetConverterConfig.test.json` qui :
        -   Active uniquement le mode `QuestPdfGeneration`.
        -   Définit un `CardSetDocument` qui pointe vers les images PNG du répertoire `TestOutput/PdfOnlyTest/`.
        -   Désactive les modes `WebBasedImageGeneration` et `Mindmapper`.
2.  **Action :**
    *   Lancer `dotnet run --project "Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj" --config "TestOutput/PdfOnlyTest/AssetConverterConfig.test.json"`.
3.  **Assertion :**
    *   Vérifier que `TestOutput/PdfOnlyTest/MyDocument.pdf` a été créé et n'est pas vide.
    *   Utiliser le `PdfAuditor` pour valider que le PDF contient les images attendues et que leur ordre est correct.
    *   Vérifier que les fichiers `.harvest.json` et les images PNG d'entrée n'ont pas été modifiés.

Cette stratégie permet des tests rapides et ciblés, essentiels pour un pipeline complexe avec des dépendances externes.

-   **Tests du Moteur de Template (CardPen)** :
    -   **Stratégie** : Le moteur Javascript de CardPen peut être testé de manière totalement isolée. Il suffit de lui fournir un jeu de données CSV, un template Mustache et du CSS, puis de valider le HTML généré.
    -   **Implémentation** : Créer des tests unitaires qui appellent la fonction de rendu de CardPen et comparent le HTML de sortie à un snapshot de référence. Cela valide la non-régression des helpers (`ifCond`, `each`) et du rendu Markdown.

-   **Tests de la Phase de "Harvesting" (sans navigateur)** :
    -   **Stratégie** : Il n'est pas nécessaire de lancer un navigateur complet pour tester la logique de `HarvestManager`. On peut mocker l'étape de capture d'écran.
    -   **Implémentation** :
        1.  Créer un test qui exécute la première partie du `HarvestManager` (préparation des données, etc.).
        2.  Mocker l'appel à Playwright qui prend le screenshot. Le mock retournera simplement une image pré-générée depuis les ressources de test.
        3.  Valider que le `HarvestManager` traite correctement l'image mockée (nommage, sauvegarde au bon endroit).

-   **Tests du `PdfManager` (Générateur PDF)** :
    -   **Stratégie** : Le `PdfManager` peut être testé en isolation en lui fournissant un ensemble d'images d'entrée.
    -   **Implémentation** :
        1.  Préparer un jeu d'images de test (ex: 10 images 300x400px).
        2.  Appeler `PdfManager.GenerateDocument` avec ces images et une configuration de document simple.
        3.  Valider le PDF de sortie. La validation peut se faire de deux manières :
            -   **Snapshot Testing** : Comparer le hash du PDF généré avec un hash de référence. Simple mais fragile aux moindres changements.
            -   **Audit Programmatique** : Utiliser le `PdfAuditor` (ou directement `PdfPig`) pour ouvrir le PDF généré et valider ses propriétés (ex: a-t-il le bon nombre de pages ? contient-il 10 images ?). C'est plus robuste.

### 5.2. Utilisation de Configurations Partielles

Pour les tests, il est fondamental de ne **pas** utiliser la configuration globale du projet.
-   **Principe** : Créer des fichiers `AssetConverterConfig.test.json` spécifiques à chaque scénario de test.
-   **Exemple** : Pour tester le `PdfManager`, on créera une configuration qui ne contient **que** le mode `QuestPdfGeneration` et la définition d'un seul `CardSetDocument` pointant vers nos images de test. Tous les autres modes (`WebBasedImageGeneration`, `MindMapGeneration`, etc.) sont omis.

Cela garantit que le test est focalisé sur un seul composant et n'est pas pollué par l'exécution d'autres parties du pipeline.

## 6. Synthèse des Risques et Dette Technique

Cette architecture, bien que fonctionnelle, porte le poids de son histoire. Voici les principaux points de risque et la dette technique à surveiller ou à rembourser.

1.  **Fragilité de la Mise en Page PDF** : La logique manuelle dans `PdfManager` est une source de bugs visuels. La moindre modification peut avoir des effets de bord imprévisibles. **Solution à long terme :** Remplacer cette logique par des fonctionnalités natives de `QuestPDF` si possible, ou la documenter de manière exhaustive.

2.  **Contournement du Bug de Concurrence de QuestPDF** : Le `lock` sur la génération de PDF est un contournement, pas une solution. Il a un impact direct sur les performances. **Solution à long terme :** Surveiller les nouvelles versions de `QuestPDF` pour voir si le problème de thread-safety est résolu et retirer le `lock`.

3.  **Dépendances à des Versions Anciennes** : Le projet est épinglé à des versions spécifiques de `QuestPDF` et `Magick.NET`. C'est un risque de sécurité et de maintenance. **Action à court terme :** Créer des tests de caractérisation robustes autour des fonctionnalités qui utilisent ces librairies. **Action à long terme :** Planifier une migration vers des versions plus récentes, ce qui nécessitera une phase de test et de validation importante.

4.  **La "Disambiguation" SVG Magique** : La liaison entre les données et les nœuds SVG est le point le plus fragile de tout le pipeline. **Solution à long terme :** Idéalement, trouver un moyen d'exporter les mind maps avec des ID stables depuis Freeplane/Freemind, ou trouver un autre outil de cartographie qui le permet. **Solution à court terme :** Isoler cette logique et la couvrir avec des tests de caractérisation utilisant un fichier `.mm` et un fichier `.svg` de référence pour détecter immédiatement toute régression.