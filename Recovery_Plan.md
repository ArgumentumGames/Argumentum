# Plan de Reprise : Génération des Règles

Ce document détaille le plan d'action pour corriger la génération des PDF de règles et revenir à une version fonctionnelle et alignée avec les exigences du projet.

## Phase 1 : Audit & Nettoyage (Terminée)

L'objectif était d'analyser l'ensemble des modifications pour comprendre la cause de la régression visuelle et fonctionnelle.

### 1.2 : Analyse des Fichiers Clés - Conclusions

*   **`Cards/Rules/Argumentum_Rules_fr.json`**:
    *   **Constat :** Le CSS original, complexe et spécifique aux cartes de tarot, a été entièrement remplacé par un CSS générique pour document A4.
    *   **Impact :** Destruction complète de l'identité visuelle des cartes de règles.

*   **`Generation/.../HarvestManager.cs`**:
    *   **Constat :** La logique de génération a été altérée. La validation a été affaiblie pour accepter des résultats incorrects (nombre d'images générées différent du nombre de cartes), et les librairies de traitement JSON ont été modifiées sans raison valable.
    *   **Impact :** Corruption du cœur du générateur, masquage de la véritable erreur au lieu de la corriger.

*   **`Cards/Rules/Argumentum Rules - Cards.csv`**:
    *   **Constat :** Le contenu original, contenant l'intégralité des règles en plusieurs langues, a été supprimé et remplacé par une version simplifiée et inventée.
    *   **Impact :** Perte critique des données sources du jeu.

### Conclusion de l'Audit
L'approche adoptée était fondamentalement erronée, résultant en une régression sur les couches de données, de logique et de présentation. L'exigence de produire des *cartes de règles stylisées* a été manquée.

---

*Ce document sera mis à jour au fur et à mesure de l'avancement du plan.*
## Phase 4: Plan de Restauration Technique

Ce plan est la synthèse de l'analyse historique et de l'état actuel. Il définit les actions précises à mener pour chaque fichier concerné afin de nettoyer l'état du projet avant de reprendre le développement.

### 1. Fichier de Données : `Cards/Rules/Argumentum Rules - Cards.csv` (Restauration Complète après Sauvegarde)

*   **Diagnostic :** Le fichier a subi une refonte complète, remplaçant la structure multi-langue originale par un format CSV simplifié. Le contenu textuel de ces nouvelles règles est le seul "grain" potentiellement utile, bien que déconnecté du format original.
*   **Action requise :**
    1.  **Sauvegarder le Grain (Contenu) :** Copier le contenu textuel des nouvelles règles (colonnes `main_content`, `secondary_content`, `markdown_content_fr`) dans un fichier temporaire `patch_rules.txt` pour référence future.
    2.  **Restauration Complète :** Exécuter `git checkout HEAD -- "Cards/Rules/Argumentum Rules - Cards.csv"` pour annuler la refonte et restaurer l'ancienne structure et le contenu original.

### 2. Fichier de Configuration : `Cards/Rules/Argumentum_Rules_fr.json` (Restauration Complète)

*   **Diagnostic :** Le fichier a été entièrement modifié, notamment le CSS et le contenu CSV embarqué. Ces changements sont la cause de la régression visuelle. Il n'y a aucun "îlot de correction" à préserver.
*   **Action requise :**
    1.  **Restauration Complète :** Exécuter `git checkout HEAD -- "Cards/Rules/Argumentum_Rules_fr.json"` pour restaurer le fichier à son état d'origine.

### 3. Fichier de Logique : `Generation/.../HarvestManager.cs` (Restauration Sélective)

*   **Diagnostic :** L'analyse différentielle fine révèle deux "îlots de corrections" légitimes au milieu de modifications problématiques (changement de librairie JSON, affaiblissement de la validation).
    *   **Grain 1 : Robustesse de la sérialisation JSON.** La nouvelle méthode utilisant `System.Text.Json` avec un `FileStream` est une correction de bug critique qui empêche la corruption de fichiers.
    *   **Grain 2 : Validation améliorée.** La nouvelle logique dans `DownloadImages` gère plus finement les cas où le nombre d'images générées ne correspond pas aux attentes (ex: dos de carte commun), évitant des erreurs inattendues.
*   **Action requise :**
    1.  **Isoler le Grain :**
        *   Copier le bloc de code de la nouvelle sérialisation JSON (méthode de sauvegarde dans `HarvestManager.cs`) dans un fichier temporaire `patch_harvestmanager_serialization.cs.txt`.
        *   Copier le bloc de code de la logique de validation (`if (generatedCount != cardNames.Count) { ... }`) dans la méthode `DownloadImages` dans un fichier `patch_harvestmanager_validation.cs.txt`.
    2.  **Restauration Complète :** Exécuter `git checkout HEAD -- "Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs"` pour annuler toutes les modifications.
    3.  **Ré-application Manuelle :**
        *   Dans le fichier `HarvestManager.cs` à présent restauré, remplacer la logique de sérialisation JSON d'origine par le contenu de `patch_harvestmanager_serialization.cs.txt`.
        *   Remplacer la logique de validation dans `DownloadImages` par le contenu de `patch_harvestmanager_validation.cs.txt`.

### 4. Gestion exhaustive de l'état non-commité (24 fichiers)

Cette section détaille le plan d'action pour chaque fichier modifié, en attente ou non suivi, afin d'assainir complètement l'espace de travail.

**a) Fichiers à Restaurer (ANNULER les modifications locales)**
*   **Diagnostic :** Modifications liées à la tentative échouée de génération de PDF au format A4. Ces changements ont corrompu la logique de génération et dégradé la qualité visuelle des cartes. Aucun "grain" à conserver.
*   **Actions :** Exécuter les commandes suivantes pour annuler les modifications locales.
    *   `git checkout HEAD -- "Cards/Rules/Argumentum Rules - Cards.csv"`
    *   `git checkout HEAD -- "Cards/Rules/Argumentum_Rules_fr.json"`
    *   `git checkout HEAD -- "Generation/CardPen/js/main.js"`
    *   `git checkout HEAD -- "Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/CardSetConfig.cs"`
    *   `git checkout HEAD -- "Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs"`
    *   `git checkout HEAD -- "Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/DataSetInfo.cs"`
    *   `git checkout HEAD -- "Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/ImageFileGenerator.cs"`
    *   `git checkout HEAD -- "Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs"`
    *   `git checkout HEAD -- "Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGenerator.cs"`
    *   `git checkout HEAD -- "Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGeneratorConfig.cs"`

**b) "Commits en retard" à Préparer pour Commit (CHANGEMENTS LÉGITIMES)**
*   **Diagnostic :** Ces modifications semblent être des améliorations légitimes (mise à jour de dépendances, refactoring, ajout de fonctionnalités) qui n'ont jamais été commitées. Elles doivent être préservées et intégrées proprement.
*   **Action :** Exécuter `git add` sur les fichiers et répertoires suivants pour les préparer au prochain commit.
    *   `Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj`
    *   `Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs`
    *   `Generation/Converters/Argumentum.AssetConverter/ConverterMode.cs`
    *   `Generation/Converters/Argumentum.AssetConverter/Documentation/GenerateDocumentation.cs`
    *   `Generation/Converters/Argumentum.AssetConverter/Logger.cs`
    *   `Generation/Converters/Argumentum.AssetConverter/Program.cs`
    *   `Generation/Converters/Argumentum.AssetConverter/Tests/CardGenerationValidationTests.cs`
    *   `Generation/Converters/Argumentum.AssetConverter/Tests/ContinuousValidationSystem.cs`
    *   `Generation/Converters/Argumentum.AssetConverter/PdfAuditor/`
    *   `Generation/Documentation/`

**c) Fichiers à Ignorer et Nettoyer**
*   **Diagnostic :** Le rapport a identifié des fichiers et répertoires qui ne devraient pas être suivis par Git (configurations locales, logs).
*   **Actions :**
    1.  **Mettre à jour `.gitignore` :** S'assurer que les lignes suivantes sont présentes dans le fichier `.gitignore` pour éviter le suivi futur de ces fichiers.
        *   `Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.json`
        *   `Logs/`
    2.  **Nettoyer le répertoire de travail :** Si le répertoire `Logs/` est toujours présent après la mise à jour du `.gitignore`, exécuter `git clean -fdx "Logs/"` pour le supprimer.

**d) Documentation du Processus de Récupération à Commiter**
*   **Diagnostic :** Les rapports générés (`Git_Archeology_Report.md`) et ce plan de reprise (`Recovery_Plan.md`) sont des artefacts importants de ce processus de nettoyage. Ils doivent être conservés pour référence future.
*   **Action :** Exécuter `git add` sur ces fichiers pour les inclure dans le commit de nettoyage.
    *   `Git_Archeology_Report.md`
    *   `Recovery_Plan.md`