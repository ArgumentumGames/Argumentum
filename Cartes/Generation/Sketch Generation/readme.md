# Procédure pour la génération des cartes sous Sketch

## Prérequi :
* [Sketch](https://www.sketch.com/)
* Le plugin Sketch [Datapopulator](https://datapopulator.com/)
* Utile mais pas indispensable le plugin Sketch [Renameit](https://github.com/rodi01/RenameIt/releases/tag/v4.2.2) (pour renommer toutes les cartes à la volée et faire l'export simplement).

## Générer les cartes 

* Faire un export JSON des données à rendre. Par facilité on peu utiliser le site [convertCSV](http://www.convertcsv.com/csv-to-json.htm), ainsi un simple copié coller des données permet de générer le JSON.
* Créer une nouvelle page (en haut à gauche, _Layers_ "+") ou utiliser la page en cours.
* Insérer le modèle de la carte à créer sur cette page. Pour une carte argument faire Insert -> Symbol / Document -> Carte Argument
* Sélectionner la carte puis dans le menu Plugins -> DataPopulator -> Populate with JSON
* Choisir le fichier JSON, sélectionner "Create Grid" pour rendre toutes les cartes (21 par 4 pour les 84 cartes par exemple)

## Fixer les images
Choisir le layer Symbols (en haut à gauche), séctionner l'élément {Image} dans le groupe "Carte Argument / Carte" puis dans le menu de droite dans la partie "Fills" changer le display par "Fit".

## Exporter les cartes
### Renommer les cartes de manière unique
Revenir à la page de génération, sélectionner toutes les cartes et les renommer : Plugins -> RenameIt -> Rename selected Layers. Dans la popup remplir dans Name : "Carte %N"

### Choisir le bon format d'export
Sélectionner toutes les cartes, puis tout en bas de la colonne de droite -> Make exportable.
Choisir au moins 4x pour avoir une bonne qualité d'image.

