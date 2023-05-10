# Argumentum

Argumentum is a card-game about Argumentation. The Argumentum project involved creating tools to generate and distribute the end-material, but also to ease updating the content that it is made of.

## How to play

The game is played with two decks of cards. Most rules involve rounds where a small card is drawn and playes will compete to play and catch fallacy large cards from their hands or common pot.

More information (currently in French) is available on the project's website https://www.argumentum.games


## Content of the repository

The repository contains the following folders:

*  2 tools in [Generation](/Generation) make up for most of the generation workflow:
    * [CardPen](/Generation/CardPen/index.html) is a customized version of [M.C DeMarco](https://github.com/mcdemarco/)'s cardpen from before it got a new home in Github. Merging improvements would be mutually benificial at some point.
	Cardpen can generate a set of templated cards by injecting csv data into a mustache + css html template system.
	You need 2 cardpen json files (1 for face, 1 for back) to specify a physical playing card.
	* [Converters](/Generation/Converters) holds a multi-purpose packaging and pipeline processing command line utility that reads a large XML configuration file to specify how to process and assemble Cardpen scenarii to build the send-to-print document. 
* [Cards](/Cards): Then generally gathers cardpen configurations to build the decks that make Argumentum, with folders for each type, together with assets and archives.
	
* [DNNPlatform](/DNNPlatform): contains the web application that is used for the www.argumentum.games website, and that is a DNN ASP.Net application with the use of 2 community extensions, 2Sxc, which holds most content, and Open-store, which holds the selling point.  

## How to generate the cards and the images

The generation tool is a .Net 7.0 console/terminal application that drives several browsers to draw the images from a hosted Cardpen website, uses Magick.Net library to process image files individually and then QuestPDF library to generate printable Pdfs.
A prerelease of the executable tool is available in the [following directory](https://github.com/ArgumentumGames/Argumentum/tree/master/Generation/Converters/Argumentum.AssetConverter/Published/v1.3)
To run it, you need to [install the runtime](https://dotnet.microsoft.com/download/dotnet/7.0/runtime).

You can launch the program :

- either by running the executable. On Linux/Mac, you may need to grant authorizations: "chmod +x ./Argumentum.AssetConverter"
- or by running the following command from a terminal: "dotnet ./Argumentum.AssetConverter.dll"

### How it works

The application runs a pipeline entirely controlled by a large configuration file, "AssetConverterConfig.json", that is created on first run.

Top level configuration has the following key sections:

- DataSets: Those are the csv files that contain all text content for the several decks of cards, namely Fallacies, Scenarii, and Rules.
- CardSets: Those are the json files that contain face and back templates for all kinds of cardsets. Datasets are injected at runtime into them, and some of them make use of the same base json file, while injecting variations at runtime. Translations are also performed from French templates by doing conversions.
- CardSetDocuments:Those are configurations for pdf to generate from the images, namely for professional printing or home print&play, or large posters.
- MindMapDocuments:Those are configurations for Freemind/Freeplane mindmaps documents to generate, and SVG customizing after exporting from one of those additional free tools.
- LocalizationConfig: This is the part concerned with localizing fields and strings for cards and mindmaps.

```json
{
  "Mode": "WebBasedImageGeneration",
  "WebBasedGeneratorConfig": {
    "ReleaseCardpenUrl": "https://argumentumgames.github.io/Argumentum/Generation/CardPen/index.html",
    "DebugCardpenUrl": "http://cardpen.dnndev.me/Generation/CardPen/index.html",
    "OverwriteExistingDocs": false,
    "MaxDegreeOfParallelismCardpen": 3,
    "MaxDegreeOfParallelismCardpenTranslations": 2,
    "MaxDegreeOfParallelismImages": 3,
    "MaxDegreeOfParallelismImageTranslations": 2,
    "MaxDegreeOfParallelismDocuments": 3,
    "BaseTargetDirectoryName": "Target\\",
    "HarvestDirectoryName": "Harvest\\",
    "ImagesDirectoryName": "Images\\",
    "DocumentsDirectoryName": "Documents\\",
    "ForceDebugParams": false,
    "ForceReleaseParams": false,
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
  "BatchImageConverterConfig": {
  },
  "Dnn2sxcConfig": {
  },
  "MindMapCreatorConfig": {  
  }
}
```




## How to build the website

The website is currently commited without the data and decryptionkey, implying a fresh install of DNN should preexist before attempting a merge, which is a significant inconvenience. Further effort should help anonymizing a snapshot of the DNN DB, whith all pages configured.

For now, if you are interested into running a copy of Argumentum.games, just let us know.

## How to contribute

The project is open to contributions. If you want to contribute, you can do so by forking the repository, making your changes, and then submitting a pull request.

## License

The project is licensed under the LGPL-3.0 license. See the [LICENSE](/LICENSE) file for more details.

## Contact

You can contact the project maintainers via the [issues](/issues) page.

## Acknowledgements

The project is based on the work of the following people:

* [M.C DeMarco's cardpen](https://github.com/mcdemarco/cardpen) is an excellent online card designer.


