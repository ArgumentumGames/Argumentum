# Spécification Technique : Refactorisation de la Liaison de Données SVG

**Auteur:** Roo, Architecte Technique
**Date:** 23/07/2025
**Statut:** Implémenté

## 1. Contexte et Problématique

L'analyse de l'historique du projet (notamment le commit `fc62618c`) a révélé que la logique actuelle de liaison entre les données des fallacies/vertus et leur représentation graphique est une source majeure de fragilité et d'instabilité.

### Critique de l'Approche Actuelle

L'approche actuelle peut être décrite comme du **"scraping d'interface utilisateur"** :

1.  **Génération Opaque :** Une application web (`Cardpen`) reçoit des données et un template de Mind Map, puis génère une page HTML interactive.
2.  **Absence d'Identifiants :** Les éléments graphiques (nœuds de la Mind Map) dans le DOM généré ne possèdent pas d'identifiant unique et stable qui les lierait à la donnée d'origine.
3.  **Liaison par l'Ordre et le Contenu :** Le `HarvestManager` tente de "deviner" la correspondance en se basant sur :
    *   L'ordre d'apparition des éléments dans le DOM.
    *   Le contenu textuel extrait de certaines balises (`.cardName`).
4.  **Complexité Palliative :** Du code complexe a été ajouté pour gérer les cas où cette "devinette" échoue (nombre de nœuds différent du nombre d'images, etc.), ce qui masque les problèmes sous-jacents au lieu de les résoudre.

Cette méthode est intrinsèquement **non déterministe**. Le moindre changement dans la structure du DOM de `Cardpen` (mise à jour de librairie JS, refactorisation CSS, etc.) ou dans la manière dont Freemind exporte ses données peut casser toute la chaîne de génération d'assets de manière silencieuse et imprévisible.

## 2. Nouvelle Architecture de Flux de Données

Pour éradiquer cette fragilité, nous proposons un nouveau flux de données basé sur un principe simple : **la liaison par identifiant explicite et stable de bout en bout.**

### Diagramme du Nouveau Flux

```mermaid
graph TD
    subgraph "Étape 1: Source de Vérité"
        A[Fichier CSV "Taxonomy.csv"] --> |Contient un ID unique par item, ex: 'trivial-justification'| B(Processus de Génération C#);
    end

    subgraph "Étape 2: Génération Augmentée"
        B --> |Lit le CSV et génère le SVG| C{SVG Augmenté};
        C --> |Chaque nœud pertinent a un 'id' correspondant, ex: <g id="fallacy-trivial-justification">| D[Application Web "Cardpen"];
        A --> |Aussi chargé comme objet JSON| D;
    end

    subgraph "Étape 3: Liaison Déterministe Côté Client"
        D --> |L'utilisateur clique sur le nœud avec id="fallacy-trivial-justification"| E{Script JS};
        E --> |1. Récupère l'ID 'fallacy-trivial-justification' depuis le SVG| F[Affichage de la Carte];
        E --> |2. Utilise l'ID pour trouver les données dans l'objet JSON| F;
    end

    subgraph "Étape 4: Récolte Fiable"
        G[HarvestManager] --> |Orchestre| B;
        G --> |Demande à Cardpen de générer l'image pour 'fallacy-trivial-justification'| D;
        D --> |Génère l'image et la retourne| H(Image 'fallacy-trivial-justification.png');
        H --> G;
    end

    style A fill:#f9f,stroke:#333,stroke-width:2px;
    style C fill:#bbf,stroke:#333,stroke-width:2px;
    style F fill:#9f9,stroke:#333,stroke-width:2px;
    style H fill:#9f9,stroke:#333,stroke-width:2px;
```

### Justification de la Solution

*   **Robustesse :** Le lien entre la donnée et le graphique est assuré par un contrat fort (l'identifiant unique), insensible aux changements de structure, de style ou d'ordre des éléments.
*   **Simplicité :** La logique de "devinette" et le code palliatif dans `HarvestManager` sont supprimés, réduisant la complexité et la surface de bugs.
*   **Maintenabilité :** Le débogage devient trivial. Si une carte est incorrecte, il suffit de vérifier que l'ID dans le SVG correspond à l'ID dans la source de données.
*   **Performance :** Bien que non testé, l'accès direct par ID est généralement plus performant que le parcours et l'interrogation du DOM.

## 3. Plan de Mise en Œuvre

La mise en place de ce nouveau flux nécessite des modifications à plusieurs niveaux.

### Étape 1 : Modification des Données Sources

1.  **Vérifier/Ajouter une colonne `id`** dans les fichiers sources `...Taxonomy.csv`. Cet `id` doit être un "slug" utilisable dans un attribut HTML/SVG (ex: `lettre-au-pere-noel`, `pente-savonneuse`).
2.  **Mettre à jour la classe C#** (`Fallacy.cs`, `Virtue.cs`) qui représente une ligne du CSV pour inclure cette nouvelle propriété `Id`.

### Étape 2 : Adapter le Générateur de Mind Map

La logique dans `FallacyMindMapCreatorConfig` et `VirtueMindMapCreatorConfig` doit être modifiée.
1.  **Identifier le code** responsable de la création des nœuds SVG.
2.  **Modifier ce code** pour qu'il récupère l' `Id` de l'objet `Fallacy`/`Virtue` et l'ajoute à l'attribut `id` de l'élément SVG principal du nœud (probablement un groupe `<g>`).

### Étape 3 : Simplifier l'Application `Cardpen`

Le fichier source de `Cardpen` (probablement `Cards/Fallacies/Mindmaps/included.html` ou un équivalent) doit être modifié.

1.  **Modifier le script JavaScript (`showOverlay`) :**
    *   Au lieu d'extraire de multiples attributs (`family`, `description`...), le script ne récupérera que l'identifiant du nœud cliqué : `let fallacyId = targetNode.id;`.
    *   Les autres informations (titre, description, etc.) seront obtenues en consultant un objet/dictionnaire JavaScript, préalablement chargé, qui mappe les `id` à leurs données complètes (`let data = allFallacies[fallacyId];`).
2.  **Modifier le chargement des données** pour créer cet objet `allFallacies` à partir du fichier CSV/JSON.

### Étape 4 : Refactoriser `HarvestManager.cs`

1.  **Supprimer la méthode `ExtractCardNames`** : elle devient obsolète.
2.  **Refactoriser `DownloadImages` :**
    *   Supprimer toute la logique de validation et de gestion des incohérences.
    *   La méthode recevra désormais une liste d'IDs de cartes à générer. Elle bouclera sur cette liste et demandera à `Cardpen` de produire chaque image de manière explicite.
3.  **Ajuster `GenerateHarvestImages`** pour orchestrer le nouveau flux en se basant sur la liste d'IDs issue de la source de données CSV.

## 4. Conclusion

Cette refactorisation est une intervention d'architecture nécessaire pour garantir la stabilité et la maintenabilité à long terme du processus de génération d'assets. Elle remplace une heuristique fragile par un mécanisme déterministe, éliminant ainsi une classe entière de bugs potentiels.