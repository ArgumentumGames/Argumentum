# Plan de Refactorisation de `PdfManager`

## 1. Contexte

L'analyse du commit `6edf683c` a révélé que la classe `PdfManager`, et plus spécifiquement la méthode `GeneratePrintAndPlay`, a introduit une logique de mise en page manuelle complexe et fragile. L'objectif de ce refactoring est de remplacer cette implémentation par une architecture moderne, déclarative et robuste en utilisant les fonctionnalités natives de QuestPDF.

La version de QuestPDF a été fixée à la `2022.12.12` pour garantir l'utilisation d'une version sous licence MIT, stable et gratuite pour tout usage commercial.

## 2. Analyse de l'Implémentation Actuelle et Points de Fragilité

La méthode `GeneratePrintAndPlay` est le principal sujet de préoccupation. Elle présente plusieurs défauts de conception majeurs :

### 2.1. Calculs Manuels et Impératifs de la Mise en Page

Le code calcule manuellement la taille et la position des éléments en se basant sur la taille du papier et des cartes, converties en points.

**Extrait de code :**
```csharp
// Lignes 94-103
var cardWidthPoints = ((float)docConfig.CardSets[0].FrontCards.WidthMM) * MmToPointsFactor;
var cardHeightPoints = ((float)docConfig.CardSets[0].FrontCards.HeigthMM) * MmToPointsFactor;
// ...
var contentWidthPoints = pageSize.Width - totalMarginPoints;
var contentHeightPoints = pageSize.Height - totalMarginPoints;
int nbColumns = docConfig.NbColumns > 0 ? docConfig.NbColumns : (int)(contentWidthPoints / cardWidthPoints);
var nbRows = (int)(contentHeightPoints / cardHeightPoints);
```

*   **Fragilité :** Le moindre changement de format de papier, de marge ou de taille de carte peut casser complètement la mise en page. La logique est rigide et non adaptable.
*   **Anti-pattern :** C'est l'opposé de l'approche déclarative promue par QuestPDF. On "dessine" manuellement au lieu de décrire la structure.

### 2.2. Logique Bifurquée et Complexe

Une simple variable booléenne `isBooklet` (ligne 119) divise la méthode en deux chemins d'exécution quasi-totalement distincts, rendant le code difficile à lire, à maintenir et à faire évoluer.

### 2.3. Algorithme d'Imposition de Livret (Booklet) Obscur

La logique pour réordonner les pages pour l'impression en livret (lignes 137-162) est complexe, difficile à suivre et contient même une implémentation alternative commentée, ce qui suggère qu'elle a été difficile à mettre au point et qu'elle est probablement fragile.

### 2.4. Duplication de Code

Dans la branche `else` (non-livret), la logique de création de page (définition de la taille, des marges, du header...) et de la table pour les cartes est répétée presque à l'identique pour la page de dos (lignes 218-245) et la page de face (lignes 249-277).

### 2.5. Inefficacités des Entrées/Sorties (I/O)

L'appel à `File.ReadAllBytes()` est effectué à l'intérieur de boucles `foreach` (lignes 185, 240, 272) pour chaque carte individuelle. Pour un jeu de cartes conséquent, cela entraîne un nombre très élevé d'opérations de lecture de fichiers, ce qui est très inefficace et dégrade les performances.

## 3. Proposition d'Architecture de Refactoring

L'objectif est de créer une architecture claire, modulaire et qui tire pleinement parti de l'API déclarative de QuestPDF.

### 3.1. Création d'un Document PDF Déclaratif

Je vais créer une nouvelle classe, par exemple `PrintAndPlayDocument`, qui implémentera l'interface `IDocument` de QuestPDF. Cette classe encapsulera toute la logique de génération du document.

```csharp
public class PrintAndPlayDocument : IDocument
{
    private readonly CardSetDocumentConfig _docConfig;
    private readonly List<CardImages> _images;

    public PrintAndPlayDocument(CardSetDocumentConfig docConfig, List<CardImages> images)
    {
        _docConfig = docConfig;
        _images = images;
    }

    public DocumentMetadata GetMetadata() => new DocumentMetadata()
    {
        // ... métadonnées ...
    };

    public void Compose(IDocumentContainer container)
    {
        // Logique de composition du document
    }
}
```

### 3.2. Composants de Page Réutilisables

La logique de mise en page des cartes sera extraite dans un composant réutilisable (`IComponent`). Ce composant prendra en charge l'affichage d'une grille de cartes.

```csharp
public class CardGridComponent : IComponent
{
    private readonly IEnumerable<byte[]> _cardImageData;
    private readonly int _columns;
    private readonly float _padding;

    public CardGridComponent(IEnumerable<byte[]> cardImageData, int columns, float padding)
    {
        _cardImageData = cardImageData;
        _columns = columns;
        _padding = padding;
    }

    public void Compose(IContainer container)
    {
        container.Grid(grid =>
        {
            grid.Columns(_columns);

            foreach (var imageData in _cardImageData)
            {
                grid.Item().Padding(_padding).Image(imageData);
            }
        });
    }
}
```

### 3.3. Optimisation des Lectures de Fichiers

Toutes les images seront lues en mémoire une seule fois au début du processus, avant la génération du PDF. La liste de `byte[]` sera passée aux composants, éliminant les accès disque répétitifs à l'intérieur des boucles de rendu.

### 3.4. Simplification de la Logique

*   **Suppression des calculs manuels :** La mise en page sera entièrement gérée par les composants `Grid`, `Column`, `Row` de QuestPDF.
*   **Logique de livret simplifiée :** La logique d'imposition sera séparée et plus lisible. Le document composera simplement les pages dans l'ordre fourni par la logique d'imposition.
*   **Suppression de la duplication de code :** La même page sera composée pour le recto et le verso en passant simplement des données différentes au `CardGridComponent`.

### 3.5. Restructuration de `PdfManager`

La classe `PdfManager` sera grandement simplifiée. Sa seule responsabilité sera de :
1.  Préparer les données (lire les images en mémoire).
2.  Instancier le `PrintAndPlayDocument`.
3.  Appeler la méthode `GeneratePdf` de QuestPDF.

```csharp
// Dans PdfManager.cs
public void GeneratePrintAndPlay(string fileName, CardSetDocumentConfig docConfig, List<CardImages> images, bool overwrite)
{
    // ... vérification de l'overwrite ...

    // 1. Lire toutes les images en mémoire une seule fois
    var frontImagesData = images.Select(img => File.ReadAllBytes(img.Front)).ToList();
    var backImagesData = images.Select(img => File.ReadAllBytes(img.Back)).ToList();

    // 2. Instancier le document
    var document = new PrintAndPlayDocument(docConfig, frontImagesData, backImagesData);

    // 3. Générer le PDF
    document.GeneratePdf(fileName);
}
```

## 4. Statut d'Implémentation

**Statut :** Terminé

Ce plan de refactorisation a été entièrement implémenté le 24/07/2025.

- La classe `PrintAndPlayDocument` a été créée et contient maintenant toute la logique de composition déclarative du document PDF.
- Le composant réutilisable `CardGridComponent` a été développé pour gérer l'affichage en grille des cartes.
- La méthode `GeneratePrintAndPlay` dans `PdfManager` a été refondue. Elle se charge maintenant uniquement de la préparation des données (lecture des images en mémoire) et de la délégation de la génération au `PrintAndPlayDocument`.
- Toute l'ancienne logique de calcul de mise en page manuelle, de duplication de code et d'I/O inefficace a été supprimée du `PdfManager`.
