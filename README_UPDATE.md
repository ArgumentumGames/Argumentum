# Mise à jour de Spectre.Console pour Argumentum.AssetConverter

## Problème initial

L'application Argumentum.AssetConverter rencontrait une erreur liée à la dépendance System.Diagnostics.StackTrace lors de l'utilisation de la méthode AnsiConsole.WriteException() de la bibliothèque Spectre.Console. Cette erreur se produisait lorsque l'application tentait d'afficher des exceptions dans la console, ce qui nécessitait un script de contournement (`run_without_stacktrace_error.bat`) pour filtrer les messages d'erreur liés à System.Diagnostics.StackTrace.

## Solution mise en place

1. **Mise à jour des packages**
   - Spectre.Console: 0.47.0 → 0.50.0
   - Spectre.Console.Json: 0.47.0 → 0.50.0

2. **Modifications du code**
   - Mise à jour de la méthode LogException dans Logger.cs pour utiliser directement AnsiConsole.WriteException avec le format ExceptionFormats.ShortenEverything
   - Suppression des commentaires obsolètes dans le fichier de configuration Argumentum.AssetConverter.exe.config

3. **Script de déploiement**
   - Création d'un script PowerShell (`update_spectre_console_v2.ps1`) pour automatiser la mise à jour
   - Création d'un projet temporaire pour télécharger les nouvelles versions des DLLs
   - Sauvegarde des DLLs existantes dans un répertoire de backup
   - Mise à jour des DLLs et du fichier de configuration
   - Suppression du script de contournement obsolète
   - Création d'un nouveau script d'exécution (`run_argumentum.bat`)

## Tests effectués

1. **Test de la méthode WriteException**
   - Création d'un projet de test (WriteExceptionTest) pour vérifier le fonctionnement de AnsiConsole.WriteException()
   - Vérification que la méthode ne génère plus d'erreur liée à System.Diagnostics.StackTrace

2. **Test de la méthode LogException**
   - Création d'un test (LoggerTest) pour vérifier le fonctionnement de la méthode LogException avec la nouvelle version de Spectre.Console
   - Confirmation que la méthode fonctionne correctement avec la version 0.50.0

## Comment utiliser l'application mise à jour

1. Exécutez le script de déploiement:
   ```
   .\update_spectre_console_v2.ps1
   ```

2. Lancez l'application avec le nouveau script:
   ```
   .\run_argumentum.bat
   ```

## Recommandations pour les futures mises à jour

1. **Gestion des dépendances**
   - Maintenir à jour les packages NuGet, en particulier Spectre.Console
   - Vérifier régulièrement la compatibilité avec les nouvelles versions de .NET
   - Utiliser des tests automatisés pour valider les mises à jour de dépendances

2. **Documentation**
   - Documenter toutes les modifications apportées dans ce fichier README
   - Maintenir une liste des problèmes connus et des solutions appliquées

3. **Scripts de déploiement**
   - Continuer à utiliser des scripts de déploiement automatisés pour faciliter les mises à jour
   - Inclure des mécanismes de sauvegarde et de restauration en cas de problème

4. **Tests**
   - Créer des tests spécifiques pour les fonctionnalités critiques qui dépendent de bibliothèques externes
   - Vérifier que les exceptions sont correctement gérées et affichées