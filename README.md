# Argumentum: The Art of Argumentation

Welcome to Argumentum, a captivating card game that celebrates the intricate art of argumentation. This project not only involves the creation and distribution of the end product but also aims to facilitate content updating with ease.

## Getting Started

Argumentum is a dual-deck card game, with gameplay revolving around drawing a small card, followed by players competing to play and identify fallacy large cards from their hand or a common pot.

Discover more about the game (currently in French) on the project's [website](https://www.argumentum.games).


## Repository Structure

The repository is organized as follows:

- **[Generation](/Generation)**: Contains two tools that form the core of the generation workflow:
    - **[CardPen](/Generation/CardPen/index.html)**: This is a tailored version of [M.C DeMarco](https://github.com/mcdemarco/)'s cardpen. It's designed to generate a set of templated cards by injecting CSV data into a mustache/handlebars + CSS HTML template system. For generating a physical playing card, you'll require two cardpen JSON files (one for face, one for back).
    - **[Converters](/Generation/Converters)**: This is a pipeline processing command-line utility. It employs a large json configuration file to detail how to process and assemble Cardpen scenarios to create the send-to-print document.

- **[Cards](/Cards)**: This directory houses cardpen configurations to construct the decks that constitute Argumentum. It includes folders for each card type, along with assets and archives.

- **[DNNPlatform](/DNNPlatform)**: This is home to the web application for the Argumentum website. It's a DNN ASP.Net application employing two community extensions: 2Sxc (for most content) and Open-store (as the selling point).
 

 ## Generating Cards images and documents

The generation tool is a .NET 9.0 console application. It uses Playwright to automate Chromium for card rendering, ImageMagick for image processing, and QuestPDF for printable PDF creation.

### Quick Start (Developers)

```bash
# Prerequisites: .NET 9.0 SDK + Node.js (for Playwright)

# 1. Clone and build
git clone https://github.com/ArgumentumGames/Argumentum.git
cd Argumentum
dotnet build "Argumentum Converters.sln"

# 2. Install Playwright browsers (first time only)
pwsh Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0/playwright.ps1 install chromium

# 3. Run the pipeline (generates images + PDFs)
dotnet run --project "Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj"
```

Output goes to `Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0/Target/`.

### Quick Start (End Users — no build required)

1. Install the [.NET 9.0 runtime](https://dotnet.microsoft.com/download/dotnet/9.0/runtime).
2. Run from the repository root:

   ```bash
   dotnet run --project "Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj"
   ```

3. Generated PDFs appear in `Target/{language}/Documents/`.

> **Note**: On first run, `AssetConverterConfig.json` is auto-generated with default settings. Edit it to customize which card sets and languages to generate.

### MacOS / Linux

On macOS or Linux, you may need additional steps:

- Grant execute permissions: `chmod +x ./Argumentum.AssetConverter`
- For browser automation, elevated permissions may be needed: `sudo dotnet run --project ...`
- If libraries are rejected, go to **System Preferences → Privacy & Security** and unblock them (may need to be repeated).



### Application Workflow

The application operates based on a pipeline that is entirely orchestrated by an extensive configuration file, "AssetConverterConfig.json". This file is produced during the application's initial run.

The top-level configuration is composed of several key sections:

- **DataSets**: These CSV files house the text content for the various decks of cards, including Fallacies, Scenarii, and Rules.
- **CardSets**: These JSON files contain templates for the faces and backs of all cardsets. The datasets are injected into these templates at runtime, with some templates using the same base JSON file, but injecting variations at runtime. Translations are also performed from French templates via conversions.
- **CardSetDocuments**: These are configurations to generate PDFs from the images, suitable for professional printing, home-based print-and-play, or large posters.
- **MindMapDocuments**: These are configurations for generating Freemind/Freeplane mindmap documents, and for customizing SVG files after exporting from one of those free tools.
- **LocalizationConfig**: This segment is responsible for localizing fields and strings for cards and mindmaps.

```json
{
  "Mode": "WebBasedImageGeneration",
  "WebBasedGeneratorConfig": {
    "EnableSVGPrompt": true,
    "ShowInfoLogs": false,
    "HeadLessBrowser": false,
    "OverwriteExistingDocs": false,
    "MaxDegreeOfParallelismCardpen": 3,
    "MaxDegreeOfParallelismCardpenTranslations": 2,
    "MaxDegreeOfParallelismImages": 3,
    "MaxDegreeOfParallelismImageTranslations": 2,
    "MaxDegreeOfParallelismDocuments": 4,
    "MaxDegreeOfParallelismMindMaps": 6,
    "ForceDebugParams": false,
    "ForceReleaseParams": false,
    "BaseTargetDirectoryName": "Target\\",
    "HarvestDirectoryName": "Harvest\\",
    "ImagesDirectoryName": "Images\\",
    "DocumentsDirectoryName": "Documents\\",
    "ReleaseCardpenUrl": "https://argumentumgames.github.io/Argumentum/index.html",
    "DebugCardpenUrl": "https://argumentum.myia.io/index.html",
    "DataSets": [
	(...)
    ],
    "CardSets": [
	(...)
    ],
    "CardSetDocuments": [
	(...)
    ],
    "MindMapDocuments": [
	(...)
    ],
    "LocalizationConfig": {
	(...)
    }
  },
  "BatchImageConverterConfig": {  },
  "Dnn2sxcConfig": {  },
  "MindMapCreatorConfig": {    }
}
```

The application's objective at each phase is to generate new files. If it encounters existing files, it skips the generation process, which means the application can quickly resume from where it was interrupted. Consequently, once all files have been generated, making targeted modifications is simplified. This can be done by deleting the corresponding generated documents, and the application will only regenerate the missing files.

### How to start customizing the generated content

Customization of the card data and design is made possible through modification of datasets and templates. By default, the program uses files stored in this repository, but you can download them locally, make your changes, and update the path in the configuration to your local files. Files for customization are primarily located in the [Cards](/Cards) directory.

### How to setup a development environment 

If you'd like to customize the app's behavior, clone this repository, build the app, and run it in Debug mode.  

#### Requirements

* Visual Studio Community (Windows)/ Visual Studio for Mac (with .NET desktop workload)
* Visual Studio Code (with "C#" and "vscode-solution" extensions)
* JetBrains Rider

Just load the "Argumentum Converters.sln" solution, build and run the included C# console csproj, and you should be started.

#### Code structure

The entry point has other tools available in command line, but default configuration runs the WebBasedGenerator, which is the multi-stage generation tool.

That class makes use of the following components:

- HarvestManager takes in charge the automated browsing of cardpen, submitting customized json templates and download generated images
- ImageFil eGenerator manages MagickNet image processing to create individual image files
- PdfManager is responsible for creating pdf documents using QuestPdf
- MindMapDocumentConfig hols the logic to manipulate Freemind, SVG and Html mindmaps


## How to contribute

We are open to contributions of many kinds. You don't have to clone this repository or be a developper to give a hand.

### Text contributions 

If you're interested in helping with the data at the heart of this project, consider contributing to these Google spreadsheets:

* [Argumentum Fallacies](https://docs.google.com/spreadsheets/d/1TrQUyzXMMM-9pHdNWz1fdJ3xQ5XcHgwVH52SOnM61ow/edit?usp=sharing)
* [Argumentum Scenarii](https://docs.google.com/spreadsheets/d/1SQb9R7Dpi0jPz2JX-HXk1WFn9t68e3aq9MCGif7lM10/edit?usp=sharing)
* [Argumentum Rules](https://docs.google.com/spreadsheets/d/1jnhlod6PLgvVI-Qgrz3sTYytMgnrMyZrHcc8htPn_DQ/edit?usp=sharing)

The content is regularly exported to CSV files in this repository to the following files:

- [Argumentum Fallacies - Taxonomy.csv](/Cards/Fallacies/Argumentum%20Fallacies%20-%20Taxonomy.csv)
- [Argumentum Scenarii - Cards.csv](/Cards/Scenarii/Argumentum%20Scenarii%20-%20Cards.csv)       
- [Argumentum Rules - Cards.csv](/Cards/Rules/Argumentum%20Rules%20-%20Cards.csv)

Direct Pull requests are welcom too.

#### Translations

We aim to distribute our material to the widest possible audience. If you are fluent in a language not available yet, please let us know. We'd love to add more translations.

As for existing translations, some of them were made with automated tools, so we more than welcome corrections and improvements.

#### Improvements

If you're an expert in a field addressed by our material, we welcome suggestions for improvements, particularly regarding our extended taxonomy of fallacies.


## How to build the website

The DNN website's data and decryption key are not included in the current commit. If you're interested in running a copy of Argumentum.games, please contact us.


## License

The project is licensed under the LGPL-3.0 license. See the [LICENSE](/LICENSE) file for more details.

## Contact

You can contact the project maintainers via the [issues](/issues) page.

## Acknowledgements

The project is based on the work of the following people:

* [M.C DeMarco's cardpen](https://github.com/mcdemarco/cardpen) is an excellent online card designer.


