# Argumentum

Argumentum is a card-game about Argumentation. The Argumentum project involved creating tools to generate and distribute the end-material, but also to ease updating the content that it is made of.

## How to play

The game is played with two decks of cards. Most rules involve rounds where a small card is drawn and playes will compete to play and catch fallacy large cards from their hands or common pot.

More information (currently in French) is available on the project's website https://www.argumentum.games


## Content of the repository

The repository contains the following folders:

*  2 tools in [Cartes/Generation](/Cartes/Generation) make up for most of the generation workflow:
    * [CardPen](/Cartes/Generation/CardPen) is a customized version of [M.C DeMarco](https://github.com/mcdemarco/)'s cardpen from before it got a new home in Github. Merging improvements would be mutually benificial at some point.
	Cardpen can generate a set of templated cards by injecting csv data into a mustache + css html template system.
	You need 2 cardpen json files (1 for face, 1 for back) to specify a physical playing card.
	* [Converters](/Cartes/Generation/Converters) holds a multi-purpose packaging and pipeline processing command line utility that reads a large XML configuration file to specify how to process and assemble Cardpen scenarii to build the send-to-print document. 
* [Cartes](/Cartes): Then generally gathers cardpen configurations to build the decks that make Argumentum, with folders for each type, together with assets and archives.
	
* [DNNPlatform](/DNNPlatform): contains the web application that is used for the www.argumentum.games website, and that is a DNN ASP.Net application with the use of 2 community extensions, 2Sxc, which holds most content, and Open-store, which holds the selling point.  

## How to generate the cards and the images

Build and Run Converters once to generate a default configuration if needed.
Host the cardpen instance locally.
Update the converters config to point to your local hosted website.
Run Converters to generate the cards and the images.

## How to build the website

The website is currently commited without the data and decryptionkey, implying a fresh install of DNN should preexist before attempting a merge, which is a significant inconvenience. Further effort should help anonymizing a snapshot of the DNN DB, whith all pages configured.

## How to contribute

The project is open to contributions. If you want to contribute, you can do so by forking the repository, making your changes, and then submitting a pull request.

## License

The project is licensed under the LGPL-3.0 license. See the [LICENSE](/LICENSE) file for more details.

## Contact

You can contact the project maintainers via the [issues](/issues) page.

## Acknowledgements

The project is based on the work of the following people:

* [M.C DeMarco's cardpen](https://github.com/mcdemarco/cardpen) is an excellent online card designer.


