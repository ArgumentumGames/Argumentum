# Rapport d'Archéologie Git : Restauration de la Configuration "Avril 2025" (Golden Master Avril 2024)

Suite à l'analyse de l'historique Git, il a été confirmé que la "Recette Perdue" correspond à l'état du projet en **Avril 2024** (Commit `0087f0ecab28ffe4d005d7bea9e6566856cfa721` du 25/04/2024), avant la refonte majeure de Mai 2025.

## 1. Fichiers de Configuration C#

### `Program.cs`
*   **Version Actuelle :** Contient une logique complexe de CLI (`--validate-taxonomy`, `--validate-owl`, etc.) pour différents modes de validation.
*   **Version Avril 2024 :** Point d'entrée simple qui charge `AssetConverterConfig.json` et lance `config.Apply()`.
*   **Recommandation :** Ne pas restaurer aveuglément pour ne pas perdre les nouveaux outils CLI, mais s'assurer que le chemin par défaut (sans arguments) exécute bien le pipeline de génération comme en 2024.

### `WebBasedGeneratorConfig.cs` (Cœur de la configuration)
*   **Différences Majeures :**
    *   **Url CardPen :** 
        *   2024: `UseLocalCardpen = false` (défaut) -> `ReleaseCardpenUrl = @"https://argumentumgames.github.io/Argumentum/Generation/CardPen/index.html"`
        *   Actuel: `UseLocalCardpen = true` (défaut)
    *   **Configuration des Documents :** La version 2024 contient des configurations précises pour `Argumentum_TarotCards_fr.pdf` (Rules, Memo, Fallacies) et `Argumentum_PokerCards_fr.pdf` (Scenarii), ainsi que des versions "Print&Play" et "Web".
*   **Snippet Config Restaurée (Structure) :**
    ```csharp
    // Extrait de la config Golden Master pour les Fallacies (Web)
    new CardSetDocumentConfig() {
        DocumentName = "Argumentum_Fallacies_Web_A0_fr.pdf",
        Enabled = true,
        DocumentFormat = CardDocumentFormat.PrintAndPlay,
        PageSize = "A0",
        Padding = 2,
        NoBack = true,
        Header = "Logo_Argumentum & QRCode.png",
        NbColumns = 11, // Valeur clé perdue
        CardSets = ...
    }
    ```

### `PdfManager.cs`
*   **Mécanismes Perdus/Modifiés :**
    *   La gestion de `GenerateAlternateFaceAndBack` utilisait une méthode simple avec `SelectMany` pour alterner Front/Back.
    *   La gestion de `GenerateBackFirstOneDocPerBack` (probablement pour les planches d'impression pro) groupait les cartes par dos.
*   **Recommandation :** Rétablir la logique de `GeneratePrintAndPlay` qui gérait correctement l'imposition des pages via `GenerateCardsPage` avec le support des marges et colonnes configurables (comme `NbColumns = 11` pour le A0).

## 2. CardPen (Moteur de Rendu)

L'inspection de `Generation/CardPen/js/main.js` (commit `0087f0ec`) a confirmé la présence des fonctionnalités suspectées disparues :

*   **Support Markdown :**
    ```javascript
    Handlebars.registerHelper("markdown", function (md) {
        return new Handlebars.SafeString(marked(md)); 
    });
    ```
    *Preuve : Ligne 1729 de main.js (version 2024)*

*   **Support Mustache vs Handlebars :**
    Le code contenait un basculement explicite :
    ```javascript
    if (data.useMustache)
        formatted += Mustache.to_html(...);
    else
        // Utilisation de Handlebars avec helpers (unidecode, breaklines, markdown, ifCond)
    ```
    *Ceci confirme que le moteur supportait les deux, et que Handlebars était utilisé pour les fonctionnalités avancées comme le Markdown.*

## 3. Plan d'Action pour la Restauration

1.  **Réintégration des Configs :** Copier les blocs `CardSetDocumentConfig` manquants (notamment les versions A0 et Web avec les bons paramètres de DPI et colonnes) de la version 2024 vers la config actuelle.
2.  **Restauration CardPen :** Vérifier si `main.js` actuel possède toujours les helpers Handlebars (`markdown`, `ifCond`). Si non, les réinjecter depuis le snippet ci-dessus.
3.  **Validation PdfManager :** S'assurer que `PdfManager.cs` supporte à nouveau la propriété `NbColumns` de la config (utilisée pour le format A0) qui semble avoir été simplifiée ou retirée.