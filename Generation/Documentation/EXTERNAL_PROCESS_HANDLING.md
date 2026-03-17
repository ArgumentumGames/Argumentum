# Gestion des Processus Externes

Ce document décrit la méthodologie et l'implémentation de référence pour l'exécution de processus externes au sein de l'écosystème Argumentum, afin d'assurer la fiabilité des tests et la stabilité du système.

## 1. Principes Clés

Tout processus externe doit être lancé de manière **non-interactive** et **contrôlée**.

- **Non-Interactif :** Le processus ne doit jamais requérir une intervention manuelle via une interface graphique (GUI).
- **Contrôlé :** L'exécution doit être encadrée par des mécanismes de sécurité (timeout) pour éviter de bloquer le thread principal.

## 2. Implémentation de Référence

L'implémentation de référence se trouve dans la méthode `TryAutomateSvgConversion` du fichier `Generation/Converters/Argumentum.AssetConverter/Mindmapper/FallacyMindMapDocumentConfig.cs`.

### 2.1. Point d'entrée

- **Fichier :** [`Generation/Converters/Argumentum.AssetConverter/Mindmapper/FallacyMindMapDocumentConfig.cs`](Generation/Converters/Argumentum.AssetConverter/Mindmapper/FallacyMindMapDocumentConfig.cs:312)
- **Méthode :** `private bool TryAutomateSvgConversion(string sourceMmPath, string destinationSvgPath, AssetConverterConfig config, bool isInteractive)`

### 2.2. Objectif du Processus

Ce processus convertit un fichier mindmap au format `.mm` (Freemind/Freeplane) en un fichier image vectorielle `.svg`.

### 2.3. Exécution Non-Interactive

Pour garantir une exécution sans interface graphique, l'argument de ligne de commande `-nogui` est systématiquement ajouté lorsque la méthode est appelée dans un contexte non-interactif (typiquement, lors des tests).

```csharp
if (!isInteractive)
{
    arguments.Append("-nogui ");
}
arguments.Append($"-X ConvertToSvg -S \"{sourceMmPath}\" \"{destinationSvgPath}\"");
```

### 2.4. Mécanisme de Sécurité

Un `timeout` de 30 secondes est implémenté pour prévenir tout blocage. Si le processus externe ne se termine pas dans le délai imparti, il est automatiquement terminé.

```csharp
var timeout = 30000; // 30 seconds
if (!process.WaitForExit(timeout))
{
    process.Kill();
    Logger.LogProblem($"SVG conversion process timed out after {timeout / 1000} seconds. The process was terminated.");
    return false;
}
```

La gestion des codes de sortie et la capture des logs d'erreurs complètent ce dispositif de fiabilisation.