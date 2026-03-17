# Module d'Audit de PDF (`PdfAuditor`)

## Objectif

Le `PdfAuditor` est un module intégré au pipeline de `AssetConverter` qui a pour but de vérifier automatiquement l'intégrité des documents PDF générés.

Il a été conçu pour remplacer les vérifications manuelles et garantir que le contenu visuel (les images des cartes) d'un fichier PDF "Print & Play" correspond exactement aux images sources définies dans la configuration.

## Fonctionnement

L'audit est effectué juste après la génération d'un fichier PDF par le `PdfManager`. Le processus est le suivant :

1.  **Extraction des images :** L'auditeur utilise la bibliothèque `PdfPig` pour ouvrir le PDF généré et extraire toutes les images qu'il contient, page par page.
2.  **Reconstruction de l'ordre attendu :** Il réplique la logique de mise en page du `PdfManager` (calcul du nombre de cartes par page, inversion des lignes pour les versos, etc.) pour créer une liste ordonnée des images sources qui *devraient* se trouver dans le PDF.
3.  **Comparaison par Hash :**
    -   Le hash (SHA256) de chaque image extraite du PDF est calculé.
    -   Le hash de chaque fichier image source attendu est également calculé.
    -   Les deux listes de hashs sont comparées.
4.  **Résultat :** Si le nombre d'images et chaque hash correspondent, l'audit réussit. Sinon, une erreur est consignée avec les détails des incohérences.

## Activation et Configuration

Pour utiliser l'auditeur, modifiez le fichier `AssetConverterConfig.json` :

1.  **Activez le mode :** Ajoutez `PdfAuditor` à la propriété `Mode`.
    ```json
    "Mode": "WebBasedImageGeneration, QuestPdfGeneration, PdfAuditor",
    ```

2.  **Configurez les audits :** Ajoutez une section `PdfAuditorConfig` pour spécifier quels jeux de cartes doivent être audités.
    ```json
    "PdfAuditorConfig": {
      "IsEnabled": true,
      "Audits": [
        {
          "CardSetName": "FallaciesPrintAndPlay"
        }
      ]
    }
    ```

Assurez-vous que le `CardSetName` correspond à un set de cartes configuré pour la génération dans la section `WebBasedGeneratorConfig`.

## Limites connues

L'implémentation actuelle du `PdfAuditor` ne prend pas en charge la validation des documents de type "livret" (`booklet`). Lorsqu'il rencontre ce type de document, il affiche le message `Skipping PDF audit for booklet type...` et ignore la validation. L'ajout de cette fonctionnalité est envisagé pour une future version.