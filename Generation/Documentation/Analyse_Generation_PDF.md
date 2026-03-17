# Rapport d'Analyse : Processus de Génération d'Assets

Ce document détaille le fonctionnement interne de l'outil `Argumentum.AssetConverter`, responsable de la génération des fichiers PDF "Print & Play" et d'autres assets visuels pour le projet Argumentum.

## 1. Architecture Générale

L'application est une console .NET 8 qui orchestre plusieurs bibliothèques et processus externes pour accomplir sa tâche. Le flux de travail principal peut être décomposé en trois grandes étapes :

1.  **Configuration** : Lecture et interprétation des fichiers de configuration.
2.  **Phase de "Harvesting" (Récolte)** : Génération et capture d'images de cartes via un navigateur web automatisé.
3.  **Phase de Génération PDF** : Assemblage des images récoltées dans un document PDF final.

Le point d'entrée de l'application est `Program.cs`, qui initialise la configuration et lance le processus principal via la classe `WebBasedGenerator`.

## 2. Le Moteur de Template CardPen

Le rendu visuel de chaque carte avant sa capture est géré par une application web interne nommée "Card-pen". Cette application utilise un puissant moteur de template, dont le fonctionnement est détaillé ci-dessous. Comprendre ce moteur est essentiel pour créer ou modifier des designs de cartes.

### 2.1 Principe de base : Mustache et Données CSV

Le moteur utilise une syntaxe inspirée de **Mustache/Handlebars**. Les templates sont définis dans des fichiers de configuration JSON (comme `Argumentum_Memo_Face_fr.json`) sous la clé `"mustache"`.

Le principe fondamental reste que chaque ligne d'un fichier de données (généralement un CSV) correspond à une carte à générer. Cependant, le moteur offre des fonctionnalités avancées qui vont au-delà de cette simple correspondance.

### 2.2 Accès aux Données

Les variables du fichier CSV sont directement accessibles dans le template.

-   **Variable de ligne** : Pour une ligne donnée, `{{NomDeLaColonne}}` insère la valeur de la colonne correspondante.
-   **Accès à l'ensemble des données (`rowset`)** : Le moteur rend la totalité du jeu de données accessible via un objet nommé `rowset`. Il s'agit d'un tableau de lignes. Cela permet des manipulations avancées, comme accéder à une ligne spécifique par son index : `{{rowset.0.Famille_camelCase}}`.

**Exemple (de `Argumentum_Memo_Face_fr.json`) :**
```html
<div class="{{rowset.0.Famille_camelCase}}">M</div>
<div class="{{rowset.70.Famille_camelCase}}">E</div>
```
Ici, le template accède aux données de la première ligne (`rowset.0`) et de la 71ème ligne (`rowset.70`) pour styliser les lettres du titre "MEMO".

### 2.3 Structures de Contrôle Avancées

Le moteur a été enrichi de helpers personnalisés pour permettre des logiques complexes.

-   **Boucles (`each`)** : Pour itérer sur l'ensemble des données, on utilise un bloc `{{#each}}`.
    **Syntaxe :**
    ```handlebars
    {{#each rowset}}
      <!-- Ce code est répété pour chaque ligne du CSV -->
      <p>{{nom_vulgarisé}}</p>
    {{/each}}
    ```
    Cela permet, par exemple, de générer une page récapitulative listant des éléments de plusieurs cartes, comme dans le fichier "Memo".

-   **Conditions (`ifCond`)** : Le moteur implémente un helper conditionnel personnalisé, `ifCond`, qui n'est pas standard. Il permet de comparer deux variables ou une variable et une valeur.
    **Syntaxe :**
    ```handlebars
    {{#ifCond variable1 "opérateur" variable2}}
      <!-- Contenu à afficher si la condition est vraie -->
    {{/ifCond}}
    ```
    **Exemple (de `Argumentum_Memo_Face_fr.json`) :**
    ```handlebars
    {{#ifCond Famille "==" text_fr}}
      <!-- Contenu affiché uniquement pour les lignes où la colonne 'Famille' est égale à la colonne 'text_fr' -->
    {{/ifCond}}
    ```
    Les opérateurs de comparaison disponibles incluent probablement `==`, `!=`, `<`, `>`, `<=`, `>=`.

### 2.4 Utilisation du Markdown

Le moteur interprète le **Markdown** contenu dans les champs du CSV. Le processus est le suivant :
1.  Le moteur récupère une valeur d'une colonne, par exemple `{{desc_fr}}`.
2.  Si cette valeur contient du Markdown (ex: `**gras**`, `*italique*`), il est converti en HTML (`<strong>gras</strong>`, `<em>italique</em>`).
3.  Le HTML résultant est ensuite injecté dans le template à l'endroit de la variable.

Cela signifie que les templates n'ont pas besoin de contenir de syntaxe Markdown, ils travaillent directement avec le HTML généré.

### 2.5 Recherche d'un Index d'Itération (Besoin Crucial)

**Aucune variable d'itération intégrée** (comme `{{@index}}` en Handlebars) n'a été trouvée lors de l'analyse. Dans une boucle `{{#each rowset}}`, il ne semble pas y avoir de moyen direct de connaître le numéro de la page ou l'index de la ligne en cours de traitement.

C'est une limitation importante à prendre en compte pour des fonctionnalités comme la numérotation automatique des pages ou des cartes. Une solution de contournement pourrait consister à ajouter une colonne d'index manuellement dans le fichier CSV source.

## 3. Étape 1 : Le Système de Configuration

Le cœur de l'outil `AssetConverter` réside dans son système de configuration flexible, mais puissant. Sa compréhension est cruciale pour utiliser l'outil efficacement et éviter les erreurs de production.

### 3.1 Le Rôle Central de `AssetConverterConfig.json`

Le fichier `AssetConverterConfig.json`, situé à la racine du projet de conversion, est le **point de contrôle principal** pour toutes les opérations. C'est ce fichier qui définit :
-   Quelles actions effectuer (génération d'images, de PDF, mise à jour de datasets, etc.).
-   Quelles sources de données utiliser (`CardSets`).
-   Quels documents finaux produire (`CardSetDocuments`).
-   Quels paramètres de localisation et de parallélisation appliquer.

En principe, **toute la configuration d'une session de travail devrait être définie dans ce fichier.**

### 3.2 La Configuration par Défaut : Source de Vérité et Mécanisme de Repli

Une caractéristique essentielle de l'application est son mécanisme de "repli" (fallback) qui garantit la robustesse et la prévisibilité du processus de génération.

-   **La classe `AssetConverterConfig.cs` comme source de vérité** : La configuration par défaut est entièrement définie dans la classe `Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs`. Ce fichier initialise toutes les propriétés avec des valeurs fonctionnelles, incluant les `DataSets` (pointant vers les fichiers CSV sur GitHub), la `LocalizationConfig`, et les configurations pour chaque mode de conversion.

-   **Génération automatique du `AssetConverterConfig.json`** : Si le fichier `AssetConverterConfig.json` **n'est pas trouvé** au démarrage, l'application ne se contente pas de charger les valeurs par défaut en mémoire. La méthode `GetConfig` dans `AssetConverterConfig.cs` **crée physiquement le fichier `AssetConverterConfig.json`** à la racine du projet, en le peuplant avec une sérialisation complète de la configuration par défaut.

-   **Le fichier JSON comme point de départ** : Une fois le fichier créé, c'est lui qui est lu et utilisé pour la suite du processus. Cela permet à l'utilisateur d'avoir immédiatement un exemple complet et fonctionnel comme base pour ses propres modifications, sans avoir à créer le fichier manuellement.

**Important :** Cette configuration par défaut est riche et complète. Elle contient les définitions nécessaires pour générer la quasi-totalité des assets de référence du projet (tous les sets de cartes, les PDFs Print & Play, les versions web, etc.). Elle sert de "configuration d'usine" et garantit que l'outil est toujours dans un état de fonctionnement connu.

### 3.3 Le Piège du "Repli Silencieux" - Historique d'une Régression

Une régression critique a été identifiée et corrigée. Lors d'une refactorisation, la configuration par défaut codée en dur avait été accidentellement **supprimée** et remplacée par des listes vides.

Cela a conduit à un comportement très problématique :
1.  En l'absence d'un `AssetConverterConfig.json` valide, l'application chargeait la configuration "par défaut".
2.  Cette configuration étant vide (aucune tâche `CardSetDocuments` définie), l'application s'exécutait, ne trouvait aucune tâche à accomplir, et **se terminait sans erreur ni avertissement**, ne produisant aucun fichier.

Ce "repli silencieux" vers une configuration vide rendait le débogage extrêmement difficile.

**La configuration par défaut complète a depuis été restaurée.** Le comportement attendu est maintenant le suivant : si `AssetConverterConfig.json` est manquant, l'application chargera la configuration d'usine et générera tous les documents de référence.

### 3.4 Recommandations et Bonnes Pratiques

1.  **Laissez l'outil générer le premier `AssetConverterConfig.json`** : La manière la plus sûre de commencer est de s'assurer qu'aucun `AssetConverterConfig.json` n'existe et de lancer l'outil. Il créera pour vous un fichier de configuration complet et valide, que vous pourrez ensuite modifier.
2.  **Utilisez le `AssetConverterConfig.json` généré comme source de vérité** : Une fois le fichier généré, personnalisez-le pour vos besoins spécifiques. N'hésitez pas à supprimer des sections entières (`Modes` de conversion, `CardSetDocuments`, `CardSets`) dont vous n'avez pas besoin pour alléger la configuration.
3.  **Consultez `AssetConverterConfig.cs` pour la structure de référence** : En cas de doute sur la structure, les types de données ou les valeurs par défaut d'une propriété, consultez directement la classe `AssetConverterConfig.cs` et les classes de configuration associées. C'est la référence ultime et la plus à jour.
4.  **Ne modifiez pas la configuration par défaut en C# à la légère** : Les modifications dans le code C# doivent être réservées à des changements permanents et globaux de la logique de génération du projet, car elles impacteront toute nouvelle configuration générée.
5.  **En cas de comportement inattendu** : Vérifiez la syntaxe de votre `AssetConverterConfig.json` et assurez-vous que les noms des `CardSetName` dans `CardSetDocuments` correspondent exactement aux noms (`Name`) définis dans `CardSets`.