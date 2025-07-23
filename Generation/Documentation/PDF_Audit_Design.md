# Conception du Module d'Audit de PDF

## 1. Objectif

Ce module vise à automatiser la vérification du contenu visuel des documents PDF générés par l'outil `AssetConverter`. Il doit garantir que les images intégrées dans chaque page du PDF correspondent exactement aux images sources spécifiées dans la configuration de la génération.

Cet audit remplacera la vérification manuelle, fiabilisera le processus de build et permettra de détecter rapidement toute régression ou erreur dans la génération des documents "Print & Play".

## 2. Architecture Proposée

L'architecture s'inspirera des modules de validation existants (`CardValidator`, `TaxonomyValidator`) pour une intégration cohérente.

### 2.1. Nouvelles Classes

#### `PdfAuditorConfig.cs`
- contiendra les paramètres spécifiques à l'audit.
- `IsEnabled` (bool): Activer ou désactiver le module.
- `Audits` (List<PdfAuditItem>): Liste des audits à effectuer.

#### `PdfAuditItem.cs`
- Définira un audit spécifique.
- `CardSetName` (string): Le nom du set de cartes à auditer (ex: `KnownCardSets.FallaciesPrintAndPlay`).
- `GeneratedPdfPath` (string): Le chemin vers le fichier PDF généré à vérifier.
- `ExpectedImages` (List<CardImages>): La liste ordonnée des images (recto/verso) qui devraient se trouver dans le PDF.

#### `PdfAuditor.cs`
- La classe principale contenant la logique de l'audit.
- `Apply(PdfAuditorConfig config)`: La méthode principale qui orchestrera les audits.
- `AuditPdf(PdfAuditItem item)`: La méthode qui effectuera la vérification pour un PDF donné.

#### `AuditResult.cs`
- Un objet simple pour stocker le résultat d'un audit.
- `IsSuccess` (bool): Indique si l'audit a réussi.
- `Messages` (List<string>): Contient les messages d'erreur ou d'information.

### 2.2. Modifications des Classes Existantes

#### `ConverterMode.cs` (si c'est un fichier séparé) ou `AssetConverterConfig.cs`
- Ajouter une nouvelle valeur à l'énumération `ConverterMode`:
- `PdfAuditor = 1 << 10` (ou la prochaine valeur de puissance de 2 disponible).

#### `AssetConverterConfig.cs`
- Ajouter une propriété pour la configuration de l'auditeur :
```csharp
public PdfAuditorConfig PdfAuditorConfig { get; set; } = new PdfAuditorConfig();
```
- Dans la méthode `Apply()`, ajouter le bloc pour exécuter l'auditeur :
```csharp
if (Mode.HasFlag(ConverterMode.PdfAuditor))
{
    // Logique pour appeler PdfAuditorConfig.Apply(this)
}
```

## 3. Logique de Vérification (Dans `PdfAuditor.cs`)

La méthode `AuditPdf` suivra ces étapes :

1.  **Ouvrir le PDF :** Utiliser une bibliothèque tierce pour charger le fichier `GeneratedPdfPath`.
2.  **Extraire les Images :** Parcourir chaque page du document PDF et extraire toutes les images qui s'y trouvent, en conservant leur ordre d'apparition.
3.  **Calculer les Hashs :**
    -   Pour chaque image extraite du PDF, calculer son hash (ex: SHA256).
    -   Pour chaque image attendue dans `ExpectedImages`, lire le fichier source et calculer son hash.
4.  **Comparer :**
    -   Comparer le nombre total d'images extraites avec le nombre d'images attendues.
    -   Comparer le hash de chaque image extraite avec le hash de l'image attendue correspondante dans la séquence. La logique de `GeneratePrintAndPlay` (inversion des versos, etc.) devra être prise en compte pour établir la séquence attendue correcte.
5.  **Générer le Rapport :**
    -   Si toutes les comparaisons sont correctes, l'audit est un succès.
    -   Sinon, l'audit échoue et un message d'erreur détaillé est ajouté au `AuditResult` (ex: "Page 2, Image 3: Le hash ne correspond pas. Attendu: `hash_A` (image.png), Obtenu: `hash_B`.").

## 4. Dépendance Externe

Pour la manipulation de PDF en C#, une bibliothèque est nécessaire.

-   **Option A (Vérifier l'existant) :** Je vais d'abord inspecter le fichier `.csproj` du projet pour voir si une bibliothèque comme `iTextSharp` ou `PdfSharp` est déjà une dépendance.
-   **Option B (Nouvelle Dépendance) :** Si aucune bibliothèque adéquate n'est présente, je proposerai d'ajouter le package NuGet `PdfPig`. C'est une bibliothèque moderne et open-source sous licence Apache 2.0 qui est bien adaptée à l'extraction de contenu, y compris les images.

## 5. Intégration

Après la génération d'un PDF dans `PdfManager.cs`, les informations nécessaires (chemin du PDF, configuration, liste d'images) seront passées au `PdfAuditor` pour une vérification immédiate.

## Historique de la résolution (Juillet 2025)

Le développement et l'intégration du `PdfAuditor` ont suivi un parcours itératif pour résoudre un problème critique de génération de PDF.

*   **Problème initial :** La chaîne de génération via `PdfManager.cs` produisait des documents corrompus, notamment avec une page blanche en début de fichier et des erreurs dans l'imposition des pages pour le format livret. Une simple correction de la logique ne garantissait pas contre de futures régressions.

*   **Fausses pistes :** Une première approche a été explorée pour mettre en place un audit visuel automatisé. L'idée était d'utiliser un agent (similaire à Playwright) pour "regarder" les PDF générés et les comparer à des captures d'écran de référence. Cette piste a été abandonnée car elle s'est avérée complexe, peu fiable et très sensible aux moindres variations de rendu (anti-aliasing, compression, etc.), rendant les comparaisons instables.

*   **Solution retenue :** Face aux limites de l'audit visuel, la décision a été prise par l'utilisateur d'implémenter une solution plus robuste : le `PdfAuditor`. Ce module valide le contenu en comparant le hash SHA256 des images extraites du PDF avec celui des images sources. Cette méthode garantit une correspondance parfaite et binaire entre le contenu attendu et le contenu généré, éliminant toute ambiguïté.

*   **Finalisation :** L'intégration du `PdfAuditor` a nécessité un débogage final du pipeline. Des erreurs de compilation ont été corrigées, et un problème de chemin d'accès relatif au fichier PDF à analyser a été résolu pour assurer que l'auditeur puisse localiser et traiter correctement le document juste après sa création. Ces ajustements ont permis de rendre la chaîne de génération et de validation entièrement fonctionnelle.

*   **État actuel :** Le pipeline de génération de PDF est maintenant stable et auto-validé pour les formats standards. Cependant, comme mentionné dans le `README.md` du module, la logique d'audit spécifique aux documents de type "livret" (`booklet`) n'est pas encore implémentée et reste une amélioration future.