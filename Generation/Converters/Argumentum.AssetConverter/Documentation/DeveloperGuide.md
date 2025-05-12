# Guide du développeur Argumentum

Ce document fournit un guide complet pour les développeurs qui souhaitent contribuer au projet Argumentum ou étendre ses fonctionnalités.

## Configuration de l'environnement de développement

### Prérequis

Pour travailler sur le projet Argumentum, vous aurez besoin des éléments suivants :

- **.NET SDK** (version 6.0 ou supérieure)
- **Visual Studio 2022** ou **Visual Studio Code** avec les extensions C#
- **Git** pour la gestion du code source
- **Node.js** (pour les composants web)
- **Chrome ou Edge** (pour le générateur basé sur le web)

### Installation

1. **Cloner le dépôt**

   ```bash
   git clone https://github.com/ArgumentumGames/Argumentum.git
   cd Argumentum
   ```

2. **Restaurer les packages NuGet**

   ```bash
   dotnet restore "Argumentum Converters.sln"
   ```

3. **Compiler le projet**

   ```bash
   dotnet build "Argumentum Converters.sln"
   ```

4. **Configurer les fichiers de configuration**

   Copiez les fichiers de configuration d'exemple et modifiez-les selon vos besoins :

   ```bash
   cp AssetConverterConfig.example.json AssetConverterConfig.json
   ```

### Structure du projet

Le projet Argumentum est organisé comme suit :

```
Argumentum/
├── Cards/                      # Données des cartes (CSV, images, etc.)
│   ├── Fallacies/              # Sophismes
│   ├── Rules/                  # Règles du jeu
│   └── Scenarii/               # Scénarios
├── DNNPlatform/                # Plateforme DNN
├── Generation/                 # Outils de génération
│   ├── CardPen/                # Outil de création de cartes web
│   └── Converters/             # Convertisseurs d'assets
│       └── Argumentum.AssetConverter/
│           ├── DatasetUpdater/ # Mise à jour des ensembles de données
│           ├── Dnn2sxc/        # Conversion DNN vers 2sxc
│           ├── Entities/       # Entités de données
│           ├── Mindmapper/     # Création de cartes mentales
│           ├── Ontology/       # Génération d'ontologies
│           ├── Optimization/   # Optimisation du parallélisme
│           ├── Tests/          # Tests de validation
│           └── WebBasedGenerator/ # Générateur basé sur le web
└── Sketch/                     # Fichiers de conception
```

## Ajout d'une nouvelle langue

Le projet Argumentum prend actuellement en charge le français (fr), l'anglais (en), le russe (ru) et le portugais (pt). Pour ajouter une nouvelle langue, suivez ces étapes :

### 1. Mettre à jour la configuration

Modifiez la classe `TranslationCoverageConfig` pour ajouter la nouvelle langue :

```csharp
public List<string> Languages { get; set; } = new List<string> { "fr", "en", "ru", "pt", "es" }; // Ajout de l'espagnol
```

### 2. Mettre à jour la taxonomie

Ajoutez les colonnes correspondantes à la nouvelle langue dans le fichier CSV de taxonomie :

```
Id,Famille,Sous-Famille,Soussousfamille,text_fr,desc_fr,example_fr,link_fr,text_en,desc_en,example_en,link_en,...,text_es,desc_es,example_es,link_es
```

### 3. Mettre à jour la configuration de localisation

Modifiez la classe `LocalizationConfig` pour ajouter les mappings de champs pour la nouvelle langue :

```csharp
FrontFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
    ("text_fr", new List<(string Language, string destFieldName)>(new []{
        ("en", "text_en"), 
        ("ru", "text_ru"), 
        ("pt", "text_pt"),
        ("es", "text_es") // Ajout de l'espagnol
    }) ),
    // Autres champs...
})
```

### 4. Ajouter les prompts de traduction

Créez de nouveaux fichiers de prompts pour la traduction vers la nouvelle langue dans le répertoire `DatasetUpdater/Resources` :

- `PromptTranslateEsInstructionsUser.txt`
- `PromptTranslateEsInstructionsAssistant.txt`

### 5. Mettre à jour le générateur de cartes

Assurez-vous que le générateur de cartes prend en charge la nouvelle langue en ajoutant les mappings nécessaires dans la classe `WebBasedGenerator`.

### 6. Tester la nouvelle langue

Exécutez les tests de validation pour vous assurer que la nouvelle langue est correctement prise en charge :

```bash
dotnet run --validate-taxonomy --translations
dotnet run --translation-coverage --languages fr,en,ru,pt,es
```

## Création de nouveaux tests

Le projet Argumentum utilise plusieurs types de tests pour valider la qualité des données et des assets générés. Pour créer de nouveaux tests, suivez ces étapes :

### 1. Créer une nouvelle classe de test

Créez une nouvelle classe de test dans le répertoire `Tests` :

```csharp
using System;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.Tests
{
    public class MyNewTests
    {
        private readonly AssetConverterConfig _config;
        
        public MyNewTests(AssetConverterConfig config)
        {
            _config = config;
        }
        
        public async Task RunTests()
        {
            // Implémentation des tests
        }
        
        private async Task TestFeature1()
        {
            // Test de la fonctionnalité 1
        }
        
        private async Task TestFeature2()
        {
            // Test de la fonctionnalité 2
        }
    }
}
```

### 2. Créer une classe de configuration

Créez une classe de configuration pour vos tests :

```csharp
namespace Argumentum.AssetConverter.Tests
{
    public class MyNewTestsConfig
    {
        public bool EnableFeature1Test { get; set; } = true;
        public bool EnableFeature2Test { get; set; } = true;
        
        public async Task Apply(AssetConverterConfig config)
        {
            var tests = new MyNewTests(config);
            await tests.RunTests();
        }
    }
}
```

### 3. Mettre à jour la classe AssetConverterConfig

Ajoutez votre nouvelle configuration à la classe `AssetConverterConfig` :

```csharp
public MyNewTestsConfig MyNewTestsConfig { get; set; } = new MyNewTestsConfig();
```

### 4. Mettre à jour l'énumération ConverterMode

Ajoutez un nouveau mode pour vos tests dans l'énumération `ConverterMode` :

```csharp
[Flags]
public enum ConverterMode
{
    // Modes existants...
    MyNewTests = 1 << 12, // 4096
}
```

### 5. Mettre à jour la méthode Apply

Mettez à jour la méthode `Apply` de la classe `AssetConverterConfig` pour exécuter vos tests :

```csharp
public async Task<bool> Apply()
{
    // Autres modes...
    
    if (Mode.HasFlag(ConverterMode.MyNewTests))
    {
        if (AsynchronousPipeline)
        {
            tasks.Add(Task.Run(() => MyNewTestsConfig.Apply(this)));
        }
        else
        {
            await MyNewTestsConfig.Apply(this);
        }
    }
    
    // Autres modes...
}
```

### 6. Mettre à jour Program.cs

Ajoutez une option de ligne de commande pour vos tests dans la classe `Program` :

```csharp
if (args[0].Equals("--my-new-tests", StringComparison.OrdinalIgnoreCase))
{
    Logger.LogTitle("Mode de mes nouveaux tests");
    
    var configFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
    var config = AssetConverterConfig.GetConfig(configFileName, out var _);
    
    // Activer uniquement le mode de mes nouveaux tests
    config.Mode = ConverterMode.MyNewTests;
    
    // Configurer les options de test en fonction des arguments
    if (args.Length > 1)
    {
        foreach (var arg in args.Skip(1))
        {
            if (arg.Equals("--feature1", StringComparison.OrdinalIgnoreCase))
            {
                config.MyNewTestsConfig.EnableFeature1Test = true;
                config.MyNewTestsConfig.EnableFeature2Test = false;
            }
            else if (arg.Equals("--feature2", StringComparison.OrdinalIgnoreCase))
            {
                config.MyNewTestsConfig.EnableFeature1Test = false;
                config.MyNewTestsConfig.EnableFeature2Test = true;
            }
        }
    }
    
    await config.Apply().ConfigureAwait(false);
    return;
}
```

## Bonnes pratiques

### Gestion du code

- **Utiliser des branches Git** : Créez une branche pour chaque fonctionnalité ou correction de bug.
- **Suivre les conventions de nommage** : Utilisez les conventions de nommage C# standard (PascalCase pour les classes et méthodes, camelCase pour les variables locales).
- **Documenter le code** : Ajoutez des commentaires XML pour toutes les classes et méthodes publiques.
- **Écrire des tests** : Assurez-vous que toutes les nouvelles fonctionnalités sont couvertes par des tests.

### Gestion des performances

- **Utiliser l'optimiseur de parallélisme** : Activez l'optimiseur de parallélisme pour maximiser les performances.
- **Surveiller l'utilisation des ressources** : Utilisez les outils de surveillance des ressources pour identifier les goulots d'étranglement.
- **Optimiser les opérations d'E/S** : Utilisez des opérations asynchrones pour les opérations d'E/S.
- **Mettre en cache les résultats** : Mettez en cache les résultats des opérations coûteuses pour éviter de les recalculer.

### Gestion des traductions

- **Maintenir la cohérence terminologique** : Utilisez des glossaires pour assurer la cohérence des traductions.
- **Valider les traductions** : Utilisez les outils de validation pour vérifier la qualité des traductions.
- **Suivre la couverture des traductions** : Utilisez le système de rapport de couverture pour suivre la progression des traductions.
- **Prioriser les traductions** : Concentrez-vous d'abord sur les champs les plus importants (Text, Desc) avant de traduire les champs secondaires.

## Génération de la documentation

Le projet Argumentum inclut un script pour générer une documentation HTML à partir des fichiers Markdown. Pour générer la documentation, utilisez l'option de ligne de commande `--generate-documentation` :

```bash
dotnet run --generate-documentation
```

La documentation générée sera disponible dans le répertoire `Output/Documentation`.

## Ressources supplémentaires

- **Dépôt GitHub** : [https://github.com/ArgumentumGames/Argumentum](https://github.com/ArgumentumGames/Argumentum)
- **Documentation API** : [https://argumentumgames.github.io/api](https://argumentumgames.github.io/api)
- **Guide de contribution** : [CONTRIBUTING.md](https://github.com/ArgumentumGames/Argumentum/blob/master/CONTRIBUTING.md)
- **Guide de style de code** : [CODE_STYLE.md](https://github.com/ArgumentumGames/Argumentum/blob/master/CODE_STYLE.md)

## Conclusion

Ce guide du développeur fournit les informations nécessaires pour contribuer au projet Argumentum ou étendre ses fonctionnalités. Si vous avez des questions ou des suggestions, n'hésitez pas à ouvrir une issue sur le dépôt GitHub ou à contacter l'équipe de développement.