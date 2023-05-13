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

- either by running the executable. On Linux/Mac, 
- or by running the following command from a terminal: "dotnet ./Argumentum.AssetConverter.dll". 
Note: In MacOS after unizipping the App directory, you can open a terminal set to the target directory with a right-click from Finder. 

On MacOS/Linux, you might have to bypass additional security safeguards from the host OS. 

- For running the executable directly, you may need to grant authorizations: "chmod +x ./Argumentum.AssetConverter"
- In terminal, you might need elevated permissions to run an automated browser. In that case the command should be: "sudo dotnet ./Argumentum.AssetConverter.dll"
- If you get an error stating some of the application libraries are rejected, go to  "System Preferences -> Privacy & Security" -> Continue to unblock the library that should appear here. Note that you may need to that several times before the app runs without any error.
Also make sure that in the same settings section, download execution is granted for "AppStore & identified developpers"

### How it works

The application runs a pipeline entirely controlled by a large configuration file, "AssetConverterConfig.json", that is created on first run.

Top level configuration has the following key sections:

- **DataSets**: Those are the csv files that contain all text content for the several decks of cards, namely Fallacies, Scenarii, and Rules.
- **CardSets**: Those are the json files that contain face and back templates for all kinds of cardsets. Datasets are injected at runtime into them, and some of them make use of the same base json file, while injecting variations at runtime. Translations are also performed from French templates by doing conversions.
- **CardSetDocuments**:Those are configurations for pdf to generate from the images, namely for professional printing or home print&play, or large posters.
- **MindMapDocuments**:Those are configurations for Freemind/Freeplane mindmaps documents to generate, and SVG customizing after exporting from one of those additional free tools.
- **LocalizationConfig**: This is the part concerned with localizing fields and strings for cards and mindmaps.

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

At each stage, the App aims at creating new files; if existing files are found, generation is skipped, such that the app will quickly reach where it last ended if stopped.
It also means once everything is generated, it is relatively easy to make targeted changes by deleting the corresponding generated documents, and only the missing files will be regenerated.

### How to start customizing the generated content

The application works on datasets and templates, the path of which are all available in the configuration.
By default, the paths configured are those of the files stored on this very repository. That allows to keep updated simply by startint over generation will ensure the latest versions are downloaded.
Look for the "xxxRelease" folder paths in the configuration for their locations.

Those file, mainly located in the [Cards](/Cards) directory, consist of those types:

- Datasets, in the form of csv files. This is what's getting written on the documents.
- Cardpen templates, in the form of json files containing css and handlebars html templates.
- Html templates, to be used together with svg mindmaps, generated by Freemind, and further processed by our tool.


If you wish to start making custom changes to those files, simply download them locally, make your changes and update the path to that of the file's path on your local hard drive.

### How to setup a development environment 

The most comfortable way to customise the App's behavior is to clone this repository locally, build the App and run the App in Debug mode. 
Configuration has a debug switch such that when run in debug, all file paths for datasets and templates are configured to your version of those files in your local clone of this repository, with their path relative to the application run from the build directory.

#### Requirements

Application is a .Net 7.0 console app, so the build environment can be one of the following

- Visual Studio Community (Windows)/ Visual Studio for Mac, installed with a .Net desktop workload.
- Visual Studio Code, with the "c#" and "vscode-solution" extensions, to support c# .Net builds, and .sln environments.
- Jetbrains Rider, which works very similar to VSCommunity.

Simply load the "Argumentum Converters.sln" solution, build and run the included c# console csproj an your should be started.

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

What matters most is the data that is at the heart of this project. Those 3 files contain pretty much all text from the projet:

- Argumentum Fallacies - Taxonomy.csv
- Argumentum Scenarii - Cards.csv
- Argumentum Rules - Cards.csv

In order to make bulk collaborative editing easier, their content is edited in Google spreadsheet and exported on a regular basis to commit edits to those csv files in this repository.

Here are the corresponding spreadsheets:

- [Argumentum Fallacies](https://docs.google.com/spreadsheets/d/1TrQUyzXMMM-9pHdNWz1fdJ3xQ5XcHgwVH52SOnM61ow/edit?usp=sharing)
- [Argumentum Scenarii](https://docs.google.com/spreadsheets/d/1SQb9R7Dpi0jPz2JX-HXk1WFn9t68e3aq9MCGif7lM10/edit?usp=sharing)
- [Argumentum Rules](https://docs.google.com/spreadsheets/d/1jnhlod6PLgvVI-Qgrz3sTYytMgnrMyZrHcc8htPn_DQ/edit?usp=sharing)

Those documents are open to comments, so you can start proposing fixes, and if you are willing to commit more efforts, we'll shortly add you to the list of Edit users.

#### Translations

Our main goal is to distribute our material to the widest audience. Datasets have a set of columns of all current targeted languages. 
For most new languages, automated translations were added for now, and we need natives to go through the content and propose fixes.
Some users interested in using the end material for teaching argumentation are giving a hand, and we're definitely looking for more help.
Also, if you want to add a new translation, just let us know, and we'll add the new columns in the csv files.    

#### Improvements

Experts in one of the fields addressed by our material are more than welcom to propose improvements to any part of it. Our original extended taxonomy of fallacies comes to mind as one of the main area for improvements. Feel free to address any branch it contains.


## How to build the website

The website is currently commited without the data and decryptionkey, implying a fresh install of DNN should preexist before attempting a merge, which is a significant inconvenience. Further effort should help anonymizing a snapshot of the DNN DB, whith all pages configured.

For now, if you are interested into running a copy of Argumentum.games, just let us know.



## License

The project is licensed under the LGPL-3.0 license. See the [LICENSE](/LICENSE) file for more details.

## Contact

You can contact the project maintainers via the [issues](/issues) page.

## Acknowledgements

The project is based on the work of the following people:

* [M.C DeMarco's cardpen](https://github.com/mcdemarco/cardpen) is an excellent online card designer.


