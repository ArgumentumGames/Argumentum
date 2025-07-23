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

## 3. Étape 1 : Configuration

*(Cette section est un placeholder pour le contenu existant du document. Vous ne devez insérer que le contenu ci-dessus)*

## 4. La structure-clef de AssetConverterConfig.json

Une découverte cruciale a été faite concernant la structure du fichier `AssetConverterConfig.json`. Un fonctionnement correct dépend de la présence et de la configuration de deux sections distinctes au sein de `WebBasedGeneratorConfig` : `CardSetDocuments` et `CardSets`.

-   **`CardSetDocuments`** : Cette section définit les **tâches de sortie**. Chaque élément de cette liste correspond à un document PDF à générer, en spécifiant son nom, son format, et les ensembles de cartes (`CardSets`) qu'il doit contenir.

-   **`CardSets`** : Cette section définit les **sources de données**. Chaque élément est une source de données nommée, pointant vers les fichiers JSON ou CSV qui contiennent les informations à afficher sur les cartes.

### Le repli silencieux : un piège à éviter

Le problème principal venait d'un mécanisme de "repli silencieux". Si la section `CardSets` est absente ou mal configurée, le programme **ne génère pas d'erreur**. À la place, il utilise une configuration par défaut codée en dur, ce qui mène à des comportements inattendus : le mauvais jeu de données est utilisé, les templates ne correspondent pas, et la sortie est incorrecte, sans qu'aucun avertissement clair ne soit donné.

### Exemple de structure fonctionnelle

Pour que la génération fonctionne comme prévu, les deux sections doivent être présentes et liées par un nom commun.

Voici un exemple fonctionnel tiré de notre configuration pour générer les règles du jeu :

```json
{
  "WebBasedGeneratorConfig": {
    "CardSetDocuments": [
      {
        "DocumentName": "Argumentum_Rules_fr.pdf",
        "Enabled": true,
        "DocumentFormat": "PrintAndPlay",
        "PageSize": "A4",
        "CardSets": [
          {
            "CardSetName": "RulesPrintAndPlay"
          }
        ]
      }
    ],
    "CardSets": [
      {
        "Name": "RulesPrintAndPlay",
        "FaceCardSetInfo": {
          "DataSet": "RulesPrintAndPlay",
          "JsonFilePathDebug": "..\\..\\..\\..\\..\\..\\Cards\\Rules\\Argumentum_Rules_fr.json"
        }
      }
    ]
  }
}
```

**Analyse de cet exemple :**

1.  La section `CardSetDocuments` déclare un document PDF à créer.
2.  À l'intérieur, elle spécifie qu'il doit utiliser un `CardSetName` appelé `"RulesPrintAndPlay"`.
3.  La section `CardSets` (au même niveau que `CardSetDocuments`) définit une source de données dont le `Name` est également `"RulesPrintAndPlay"`.
4.  Le programme peut alors faire le lien : pour le document `Argumentum_Rules_fr.pdf`, il doit utiliser la source de données définie sous le nom `"RulesPrintAndPlay"`, qui pointe vers le fichier `Argumentum_Rules_fr.json`.

Cette structure à deux niveaux est essentielle pour le bon fonctionnement du générateur.