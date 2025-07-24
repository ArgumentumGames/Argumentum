# Analyse des Commits Git

Ce document détaille l'analyse de l'historique des commits du projet Argumentum, avec un focus sur la détection de régressions potentielles, de mauvaises pratiques ou d'instabilités dans le système de configuration et de génération d'assets.

---

## Commit `5107e322` - fix(build): Resolve JsonSerializer ambiguity in HarvestManager

*   **Fichier(s) modifié(s) :** `Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs`
*   **Analyse Technique :**
    *   Le code force désormais l'utilisation de `System.Text.Json.JsonSerializer` pour résoudre une ambiguïté de namespace (probablement avec `Newtonsoft.Json`).
    *   Correction d'une erreur de type où une `string` JSON était assignée à un `byte[]` (`Buffer`). Le code ajoute maintenant l'encodage `UTF8.GetBytes()` nécessaire.
*   **Risque de Régression (Moyen) :**
    *   **Incompatibilité de Format :** Si les systèmes en amont ou en aval s'attendent à un format JSON spécifique à `Newtonsoft.Json`, le changement de sérialiseur peut introduire des bugs subtils (gestion des `null`, casse des noms de propriétés, etc.).
    *   **Corruption de la Configuration :** La désérialisation de fichiers de configuration existants pourrait échouer s'ils utilisent des fonctionnalités spécifiques à `Newtonsoft.Json` (ex: commentaires). Cela pourrait mener à des configurations vides ou partiellement chargées.

---

## Commit `149386d5` - chore(gitignore): Ignore Logs directory

*   **Fichier(s) modifié(s) :** `.gitignore`
*   **Analyse Technique :**
    *   Ajout simple du répertoire `Logs/` au fichier `.gitignore`. C'est une opération de maintenance standard pour empêcher le versionnement des fichiers de log.
*   **Risque de Régression (Nul) :**
    *   Aucun impact sur le code exécutable.

---
## Commit `082073ec` - feat(generation): Enhance asset converter and add PDF auditor

Ce commit contient des changements significatifs et hétérogènes.

*   **Fichier(s) modifié(s) :** Multiples, incluant `.csproj`, `*.js`, `*Config.cs`, et la logique de génération.
*   **Analyse Technique :**
    *   **Dépendance et Rendu :** Ajout de la dépendance `UglyToad.PdfPig` pour l'audit de PDF et modification du rendu Markdown pour forcer les sauts de ligne.
    *   **Externalisation de la Configuration (RISQUE ÉLEVÉ) :** Le changement le plus critique est dans `AssetConverterConfig.cs`. Les configurations `DataSets`, `CardSets`, `LocalizationConfig`, etc., ne sont plus initialisées avec des valeurs par défaut en dur. Elles sont désormais des listes vides. Toute la configuration doit impérativement provenir du fichier externe `AssetConverterConfig.json`.
    *   **Refactorisation de la Génération PDF :** La logique a été inversée. Ce n'est plus `PdfManager` qui cherche les images, mais `WebBasedGenerator` qui les génère, les collecte, et les fournit au `PdfManager`. C'est un meilleur design (meilleure séparation des responsabilités).
    *   **Parallélisme Désactivé :** La parallélisation pour `Cardpen` a été désactivée par défaut (`MaxDegreeOfParallelismCardpen` passe de 3 à 1). C'est souvent le signe de problèmes de concurrence non résolus.
    *   **Artefacts de développement :** Des fichiers de logs et un `Recovery_Plan.md` ont été inclus dans ce commit, ce qui indique que le développement s'est fait dans un contexte de crise/réparation.
*   **Risque de Régression (Très Élevé) :**
    *   **Configuration Vide :** Le risque principal et le plus probable est qu'un `AssetConverterConfig.json` manquant ou incomplet conduise à une exécution qui **ne fait rien et ne lève pas d'erreur**. L'application démarre, lit une config vide, n'a aucune tâche à effectuer, et se termine "avec succès". C'est un scénario de bug très pernicieux.
    *   **Performance :** La désactivation du parallélisme va ralentir considérablement la génération des cartes.
    *   **Mauvais Contenu des PDF :** La nouvelle logique de mapping entre les images générées et les documents PDF est plus complexe et peut contenir des erreurs, menant à des PDF mal assemblés.

---
## Commit `4eaab936` - refactor(core): Restore core files to stable state post-refactoring

*   **Fichier(s) modifié(s) :** `Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs`
*   **Analyse Technique :**
    *   **MESSAGE DE COMMIT TROMPEUR :** Le message annonce une "restauration" alors que le `diff` montre une **refactorisation en profondeur** du fichier. Ce n'est en aucun cas un retour à un état précédent.
    *   **Changement de Bibliothèque de Sérialisation :** Le code de sauvegarde JSON passe de `Utf8Json` à `System.Text.Json`, écrivant directement dans un `FileStream`. C'est une correction de bug légitime pour éviter la corruption de fichiers JSON, mais c'est un changement significatif, pas une "restauration".
    *   **Assouplissement de la Validation :** La logique qui vérifie la correspondance entre le nombre de cartes attendues et le nombre d'images générées est changée. Une simple égalité `!=` est remplacée par une logique complexe qui tolère des cas de "désynchronisation" (0 image pour un dos de carte, 1 image pour 0 nom de carte).
*   **Risque de Régression (Très Élevé) :**
    *   **Manque de Fiabilité :** Le décalage entre le message de commit et la réalité du code signale une grande confusion lors du développement. Il est impossible de se fier au message pour comprendre l'intention.
    *   **Masquage de Bugs :** L'assouplissement de la validation est dangereux. Au lieu de corriger la source du problème (pourquoi la génération produit-elle un nombre inattendu d'images ?), le code est modifié pour accepter ces résultats anormaux. Cela peut mener à des générations d'assets qui semblent réussir mais sont silencieusement incomplètes, ce qui se répercutera sur les PDF finaux.
    *   **Incompatibilité de Sérialisation :** Comme pour les commits précédents, le changement de `Utf8Json` à `System.Text.Json` comporte des risques d'incompatibilité de format.

---
## Commit `0a89cd8c` - chore(housekeeping): Ignore imageflow cache directory

*   **Fichier(s) modifié(s) :** `.gitignore`
*   **Analyse Technique :**
    *   Ajout de la règle `**/imageflow_hybrid_cache/` pour ignorer les répertoires de cache du processeur d'images `imageflow` partout dans le projet.
*   **Risque de Régression (Nul) :**
    *   Aucun impact sur le code exécutable. Opération de maintenance standard.

---
## Commit `365e4c6b` - fix(converter): Repair cardpen and asset converter logic

Il s'agit d'un "commit de réparation" massif et tentaculaire, touchant de nombreux aspects du projet.

*   **Fichier(s) modifié(s) :** `.gitignore`, `*.json`, `package.json`, `.csproj`, logique de configuration et de génération.
*   **Analyse Technique :**
    *   **Downgrade de Dépendances Critiques (RISQUE TRÈS ÉLEVÉ) :** Dans le `.csproj`, `Magick.NET` (pour l'image) et `QuestPDF` (pour le PDF) sont **downgradés** à des versions antérieures. C'est un symptôme de problèmes graves d'incompatibilité que le développeur n'a pas pu résoudre autrement. Cela bloque le projet sur de vieilles versions, créant une dette technique importante et un risque pour la maintenance future.
    *   **Stratégie de Configuration Instable :** Dans `AssetConverterConfig.cs`, le comportement de `SkipConfigFile` est inversé (passant à `false` par défaut), forçant la lecture du fichier de configuration externe. Ce "va-et-vient" par rapport aux commits précédents montre une grande incertitude sur la manière de gérer la configuration du projet.
    *   **Refactorisation Complexe de la Génération PDF :** La logique dans `PdfManager.cs` est entièrement réécrite pour tenter de gérer l'impression recto-verso ("print and play"). La nouvelle méthode est très complexe (inversion manuelle de l'ordre des cartes pour le dos, etc.) et constitue une source de bugs potentiels (cartes mal positionnées, ordre incorrect).
    *   **Hardcoding de la Configuration :** Dans `HarvestManager.cs`, le mode du navigateur (`Headless`) est forcé à `true`, ignorant la valeur qui venait du fichier de configuration. Cela réduit la flexibilité et peut indiquer que le mode non-headless était instable.
*   **Risque de Régression (Très Élevé) :**
    *   **Fragilité due aux Dépendances :** Le projet est maintenant dépendant de versions spécifiques et anciennes de bibliothèques majeures. Toute tentative de mise à jour future risque de tout casser à nouveau.
    *   **Erreurs de Mise en Page Silencieuses :** La nouvelle logique de génération de PDF est si complexe qu'elle est susceptible de contenir des erreurs de mise en page subtiles. Le `try-catch` ajouté peut empêcher le système de build de détecter une erreur critique, produisant un PDF corrompu mais un build "réussi".
    *   **Chaos Global :** Ce commit mélange des corrections de bugs, des ajouts de fonctionnalités, des mises à jour de dépendances (à la baisse !) et des changements de configuration, ce qui est une très mauvaise pratique et rend l'historique difficile à suivre.

---
## Commit `5d298b9d` - fix(deps): Revert QuestPDF to 2023.12.0 to fix PDF generation

*   **Fichier(s) modifié(s) :** `Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs`
*   **Analyse Technique :**
    *   **Contradiction Message/Code :** Le message annonce un retour en arrière de la version de `QuestPDF`. Cependant, le `diff` de ce commit montre uniquement la **suppression de blocs `try-catch`** dans la logique de rendu des PDF. La gestion des erreurs a été retirée.
    *   **Fragilisation du Code :** En supprimant la gestion des exceptions, le processus de génération de PDF devient moins robuste. Désormais, la moindre erreur sur une seule image (ex: corrompue, format non supporté) fera planter la génération complète du document, alors qu'auparavant, l'erreur aurait pu être simplement enregistrée.
*   **Risque de Régression (Élevé) :**
    *   **Instabilité accrue :** Le code est maintenant plus susceptible de planter sur des erreurs qui étaient auparavant gérées.
    *   **Mauvaise Pratique :** Le fait que le message de commit ne corresponde pas au changement de code réel est un signe majeur de mauvaise gestion de version. Le "revert" du package a probablement été fait, mais commité avec un changement de logique non lié et non documenté. Il est impossible de savoir pourquoi la gestion d'erreur a été supprimée, ce qui rend la maintenance future très difficile.

---
## Commit `6723d628` - feat: Clean up repository and update documentation

Ce commit est un grand nettoyage du référentiel qui révèle un problème de concurrence majeur.

*   **Fichier(s) modifié(s) :** `README.md`, `AssetConverterConfig.cs`, `WebBasedGenerator.cs`, et de nombreux fichiers supprimés.
*   **Analyse Technique :**
    *   **Découverte d'un Problème de Concurrence (RISQUE MAJEUR) :** Dans `WebBasedGenerator.cs`, un `lock (pdfLock)` a été ajouté autour de l'appel au `PdfManager`. C'est la preuve irréfutable que la génération de PDF avec `QuestPDF` n'était **pas thread-safe**. Les tentatives précédentes de paralléliser la création de plusieurs documents PDF devaient causer des plantages ou des corruptions de fichiers. Le `lock` résout le plantage, mais au prix de la performance, car il force la génération de PDF à se faire en série.
    *   **Nettoyage du Dépôt :** De nombreux fichiers d'archives, de documentation obsolète et d'artefacts de build (`.git-id`) ont été supprimés. C'est une excellente pratique de nettoyage.
    *   **Changement de Comportement par Défaut :** Dans la configuration, `OverwriteExistingDocs` est maintenant activé par défaut. L'application va donc écraser les PDF existants sans confirmation, ce qui peut être un comportement inattendu pour l'utilisateur.
*   **Risque de Régression (Moyen) :**
    *   **Régression de Performance :** L'ajout du `lock` constitue une régression de performance majeure pour la génération de plusieurs documents, car l'avantage de la parallélisation est perdu.
    *   **Dette Technique :** Le problème de fond (le manque de thread-safety de la génération PDF) n'est pas réglé, il est seulement contourné. C'est une bombe à retardement pour les futures évolutions.
    *   **Perte de Données Potentielle :** Le fait d'écraser les fichiers par défaut peut entraîner la perte de documents si l'utilisateur n'est pas attentif.

---
## Commit `6edf683c` - feat: Refactor MindMap generation for Virtues and Fallacies

Le message de commit est extrêmement réducteur. Il s'agit en réalité d'une **refactorisation massive et disruptive** de tout le système de génération, bien au-delà des cartes mentales. Ce commit est la source principale de l'instabilité observée dans les commits suivants.

*   **Fichier(s) modifié(s) :** `AssetConverterConfig.cs`, `WebBasedGenerator.cs`, `PdfManager.cs` (nouvellement créé/refactorisé), etc.
*   **Analyse Technique :**
    *   **Introduction d'un Bug de Concurrence :** Dans `WebBasedGenerator.cs`, le code introduit une boucle `Parallel.ForEach` pour générer les documents PDF. Cependant, la génération de PDF (via `QuestPDF`) n'est pas thread-safe. C'est ce commit qui **crée le bug de concurrence** qui ne sera "corrigé" que bien plus tard par l'ajout d'un `lock` (voir commit `6723d628`).
    *   **Refactorisation à Très Haut Risque :** Toute la logique de génération de PDF est extraite et réécrite dans un nouveau `PdfManager`. Cette nouvelle version contient une logique de mise en page manuelle, complexe et fragile, pour calculer le positionnement des cartes, gérer les marges et l'inversion des pages pour le recto-verso.
    *   **Spécialisation de la Configuration :** La configuration des cartes mentales (`MindMap...Config`) est scindée en deux classes plus spécifiques (`Fallacy...` et `Virtue...`). C'est un changement de design probablement positif, mais il est noyé au milieu d'une refactorisation dangereuse.
    *   **Suppression de `NuGet.Config` :** La suppression de ce fichier rend le build dépendant de la configuration globale de la machine du développeur, ce qui est une mauvaise pratique pour la reproductibilité des builds.
*   **Risque de Régression (Catastrophique) :**
    *   **Source de l'Instabilité :** Ce commit est la cause racine de la plupart des problèmes qui suivent. Il a introduit un bug de concurrence critique et une logique de mise en page fragile.
    *   **Complexité Incontrôlée :** La nouvelle logique dans `PdfManager` est un exemple de complexité accidentelle. Tenter de gérer manuellement la mise en page à ce niveau de détail est une source infinie de bugs visuels et d'erreurs d'impression.
    *   **"Refactorisation Big Bang" :** Tenter de changer l'architecture, la logique, la configuration et les dépendances en un seul commit est une anti-pattern classique qui mène presque toujours à ce genre de situation instable.

---
## Commit `fc62618c` - Fix(Converter): Resolve ParseException and enable Virtue generation

Ce commit est une tentative de stabiliser et de généraliser la refactorisation commencée dans `6edf683c`.

*   **Fichier(s) modifié(s) :** `AssetConverterConfig.cs`, `MindMap...Config.cs`, et de nombreuses classes de génération.
*   **Analyse Technique :**
    *   **Généricité de la Génération :** L'objectif principal est de rendre la génération de cartes mentales générique pour qu'elle fonctionne à la fois pour les `Fallacies` et les `Virtues`. Pour ce faire, une nouvelle interface `IMindMapItem` est créée, et la logique est modifiée pour opérer sur cette abstraction plutôt que sur des classes concrètes.
    *   **Logique de "Disambiguation" SVG (RISQUE TRÈS ÉLEVÉ) :** Pour faire le lien entre un item de donnée (une `Fallacy` par exemple) et son équivalent graphique dans le fichier SVG exporté par Freemind, le code introduit une logique de "disambiguation" extrêmement complexe. Elle tente de deviner quel noeud SVG correspond à quel item en se basant sur le contenu textuel, la proximité d'autres noeuds, etc.
    *   **Instabilité Continue de la Configuration :** Dans `AssetConverterConfig.cs`, le booléen `SkipConfigFile` est à nouveau inversé (passant à `true`), montrant une fois de plus l'absence de stratégie de configuration stable. Le projet oscille entre une configuration en dur dans le code et une configuration externe.
*   **Risque de Régression (Très Élevé) :**
    *   **Fragilité Extrême du Traitement SVG :** La logique de "disambiguation" est une bombe à retardement. Elle est incroyablement fragile et dépend fortement de la structure interne du fichier SVG généré par un outil tiers. La moindre modification de ce fichier par un graphiste, ou une mise à jour de Freemind qui changerait son export SVG, cassera toute la chaîne de génération sans avertissement clair.
    *   **"Code Smell" Fort :** Une telle logique est un "code smell" majeur. Elle indique que le flux de données entre les outils est cassé (Freemind ne conserve pas d'ID stables lors de l'export), et au lieu de trouver une solution robuste, le code tente de "deviner" la correspondance. Ceci est une source de bugs non déterministes et de maintenance impossible.
    *   **Chaos Persistant :** Le projet est toujours dans un état de flux, avec des changements constants de stratégie de configuration et des refactorisations qui ajoutent plus de complexité qu'elles n'en retirent.

---
## Commit `fc70138d` - fix: Refactor card generation to be stateless

Ce commit est un exemple de correction de bug de haute qualité, qui s'attaque à la cause racine d'un problème d'instabilité.

*   **Fichier(s) modifié(s) :** `ImageFileGenerator.cs`, `Logger.cs`, `OwlAdapter.cs`.
*   **Analyse Technique :**
    *   **Correction d'un Bug de Concurrence Critique :** Le message de commit l'explique parfaitement. L'ancienne logique de `ImageFileGenerator` utilisait un objet `currentCard` partagé et modifié sur plusieurs itérations d'une boucle parallèle. Cela créait des "race conditions" où plusieurs threads se marchaient sur les pieds, menant à des `KeyNotFoundException` intermittentes. La logique a été réécrite pour être **stateless** : chaque carte est maintenant assemblée atomiquement et indépendamment, éliminant la cause du bug.
    *   **Amélioration du Logging :** Pour débugger ce problème, le `Logger` a été grandement amélioré. Il est maintenant **thread-safe** (grâce à un `lock`) et écrit dans un fichier de log persistant. C'est une amélioration majeure de la robustesse et de la capacité de diagnostic de l'application.
    *   **Suppression de Dette Technique :** La méthode de chargement des ontologies dans `OwlAdapter` a été nettoyée, remplaçant une utilisation fragile de la réflexion par des appels directs à l'API de la bibliothèque `OWLSharp`.
*   **Risque de Régression (Très Faible) :**
    *   Ce commit ne présente quasiment aucun risque. Au contraire, il **augmente massivement la stabilité** de la génération d'images. C'est un pas très important vers la fiabilisation du processus. La seule petite fragilité restante est la logique de matching des noms de fichiers entre recto et verso, mais elle est maintenant bien mieux isolée et journalisée.

---
## Commit `e8482fe5` - fix(asset-converter): Repair OWL generation after OwlSharp library upgrade

Ce commit vise à corriger un problème de compatibilité avec une bibliothèque, mais le développeur en profite pour lancer une autre refactorisation majeure et risquée.

*   **Fichier(s) modifié(s) :** `.csproj`, `HarvestManager.cs`, `AssetConverterConfig.json` (supprimé).
*   **Analyse Technique :**
    *   **Mise à jour de Dépendances :** Plusieurs paquets NuGet sont mis à jour, dont `OWLSharp`. Le message et le code indiquent que cette mise à jour a cassé la génération d'ontologies et a nécessité des corrections.
    *   **Refactorisation Massive de `HarvestManager` :** La classe `HarvestManager`, responsable de l'interaction avec le navigateur, est entièrement réécrite pour être massivement parallèle. Elle utilise maintenant `Parallel.ForEachAsync` et un "pool" de pages de navigateur pour maximiser les performances.
    *   **Suppression de la Configuration Versionnée :** Le fichier `AssetConverterConfig.json`, qui contenait une configuration complète, est supprimé du contrôle de version. Le projet dépend maintenant entièrement d'un fichier de configuration généré localement ou fourni par l'utilisateur.
    *   **Logique "Stateful" Boguée Toujours Présente :** À ce stade de l'historique, la classe `ImageFileGenerator` contient toujours la logique "stateful" (avec la variable partagée `currentCard`) qui a été identifiée plus tard comme la source de bugs de concurrence.
*   **Risque de Régression (Très Élevé) :**
    *   **Introduction Probable du Bug de Concurrence :** La nouvelle architecture massivement parallèle de `HarvestManager`, combinée à la logique "stateful" et boguée de `ImageFileGenerator`, est très probablement la cause directe de la `KeyNotFoundException` intermittente corrigée dans le commit `fc70138d`. La complexité parallèle a probablement exacerbé ce bug latent.
    *   **Complexité de la parallélisation :** La gestion d'un pool d'objets (les pages du navigateur) dans un contexte fortement parallèle est notoirement difficile et une source fréquente de bugs subtils (deadlocks, race conditions, etc.).
    *   **Fragilisation du Démarrage :** La suppression du fichier de configuration par défaut rend le premier démarrage de l'application plus fragile et dépendant de la génération correcte de ce fichier.

---
## Commits de Maintenance (`ceb86b06`, `b09d4522`, `3cc8af23`)

Ces trois commits anciens concernent exclusivement des modifications du fichier `.gitignore`.

*   **Fichier(s) modifié(s) :** `.gitignore`
*   **Analyse Technique :**
    *   **Artefacts d'Outils :** Ajout des rapports et du `.jar` de l'outil BFG (utilisé pour nettoyer l'historique Git).
    *   **Sorties de Génération :** Ajout des répertoires `Output/` (où sont générés les assets) et des dossiers de cartes par langue (`Cards/en/`, `Cards/fr/`, etc.), indiquant que ces derniers sont considérés comme des produits de la génération et non comme des sources.
*   **Risque de Régression (Nul) :**
    *   Aucun impact sur le code exécutable. Ces changements améliorent la propreté du contrôle de version.

---
# Synthèse Finale et Points de Vulnérabilité

L'analyse archéologique de l'historique des commits révèle un projet qui a traversé plusieurs phases de grande instabilité, principalement centrées autour de la **génération d'assets** (`AssetConverter`). Les problèmes les plus critiques ne sont pas des bugs isolés, mais des problèmes systémiques liés à l'architecture, aux dépendances et aux pratiques de développement.

## Principaux Points de Risque

1.  **Instabilité Chronique de la Configuration :** Le projet oscille constamment entre une configuration "en dur" dans le code C# et une configuration externe via `AssetConverterConfig.json`. Cette indécision est une source majeure de bugs, notamment des scénarios où **l'application s'exécute sans rien faire** car elle lit une configuration vide (voir commit `082073ec`). Le changement fréquent de bibliothèques de sérialisation JSON (`Utf8Json`, `System.Text.Json`, `Newtonsoft.Json` implicite) aggrave ce problème en créant des risques d'incompatibilité de format.

2.  **Refactorisations "Big Bang" Mal Maîtrisées :** Le commit `6edf683c` est l'exemple parfait d'une refactorisation trop ambitieuse qui a déstabilisé le projet. Elle a remplacé une logique simple par une logique de génération PDF complexe et fragile, et a introduit un **bug de concurrence majeur** en parallélisant une opération non thread-safe (`QuestPDF`). Les commits suivants sont une longue suite de tentatives pour réparer les dégâts causés par celui-ci.

3.  **Gestion Fragile des Dépendances :** Le projet a dû **revenir à des versions antérieures** de bibliothèques critiques (`QuestPDF`, `Magick.NET`), ce qui est un signe de problèmes d'intégration profonds (commit `365e4c6b`). Cela crée une dette technique importante et rend le projet vulnérable et difficile à maintenir.

4.  **Logiques "Magiques" et Fragiles :** L'approche pour lier les données (`Fallacy`, `Virtue`) à leur représentation graphique dans les fichiers SVG (commit `fc62618c`) est extrêmement fragile. La logique de "disambiguation" qui tente de "deviner" les correspondances est une bombe à retardement qui cassera à la moindre modification du fichier SVG.

5.  **Manque de Fiabilité de l'Historique Git :** De nombreux messages de commit sont trompeurs ou contredisent les changements réels du code (ex: `4eaab936` qui annonce une "restauration" mais effectue une refactorisation majeure). Cela rend l'historique difficile à exploiter pour comprendre l'évolution du code et les intentions du développeur.

## Conclusion

Le principal risque de régression ne vient pas d'un commit en particulier, mais de la **cascade d'événements initiée par la refactorisation `6edf683c`**. Cette dernière a introduit une complexité et une instabilité que les commits suivants ont tenté, avec plus ou moins de succès, de contenir. Le bug de concurrence le plus grave a finalement été corrigé proprement (`fc70138d`), mais le système reste fragile, notamment à cause de sa dépendance à des versions de bibliothèques anciennes et de sa logique de traitement SVG complexe.

Toute intervention future sur le `AssetConverter` doit être faite avec une extrême prudence, en particulier sur le `PdfManager` et la configuration.