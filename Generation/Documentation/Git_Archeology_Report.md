# Rapport d'Archéologie Git

Ce document retrace l'historique des modifications du projet sur la dernière année pour comprendre l'évolution du code et identifier les changements pertinents.

## Liste des Commits à Analyser

```
365e4c6bf4e339bbcf0bf18a26d90cf7b8fd3613 - jsboige, 5 days ago : fix(converter): Repair cardpen and asset converter logic
5d298b9d6baae241ed1143d52c84ab1342f80811 - jsboige, 8 days ago : fix(deps): Revert QuestPDF to 2023.12.0 to fix PDF generation
6723d62855c18db75d262807c85bf4e6770d16a - jsboige, 8 days ago : feat: Clean up repository and update documentation
6edf683ce1530d202aba1abbd839fc13cb878381 - jsboige, 11 days ago : feat: Refactor MindMap generation for Virtues and Fallacies
fc62618c1bff417986bb4e99bcb55c0db000c627 - jsboige, 2 weeks ago : Fix(Converter): Resolve ParseException and enable Virtue generation
fc70138d6ce0efb3608cc45b833caf0bd40e8d546 - jsboige, 3 weeks ago : fix: Refactor card generation to be stateless
e8482fe5d50fcc1778effeba85311413114004121 - jsboige, 3 weeks ago : fix(asset-converter): Repair OWL generation after OwlSharp library upgrade
ceb86b06438f1777cbbb64790a604cf91255a7ab - jsboige, 3 weeks ago : build: Ignore les fichiers de projet non suivis pour nettoyer l'arbre de travail
b09d4522d7bb6faa0f69122819f277b7154c62d6 - jsboige, 3 weeks ago : build: Ignore les répertoires de sortie générés
3cc8af23a9f7efb0ee74b8911a225fc28463837e - jsboige, 3 weeks ago : build: Met à jour .gitignore pour exclure les artefacts de nettoyage BFG et l'exécutable bfg.jar
949a09cae8348adbda6c78723ed4d621a27fbd1b - jsboige, 2 months ago : Ajout des fichiers de configuration et adaptation de la classe Fallacy avec une propriété Id
6a871ef7ecae7a9335688ba60d04ed6757b41c169 - jsboige, 2 months ago : Ajout et mise à jour des fichiers de test et de validation
c8e8fe7a7ba07079428c8160d67d8ca28ad59403 - jsboige, 2 months ago : Adaptation du code pour la compatibilité avec OWLSharp 4.6.1 et ajout de la méthode LogInfoMessage
563643570d34d92e10bb36ac872c5b2544432e21 - jsboige, 2 months ago : Mise à jour du package OWLSharp de la version 3.11.0 à la version 4.6.1
3ff3ecfc0ab1ab5e248383a7759e0d38167585a3 - jsboige, 2 months ago : Retrait des fichiers binaires du suivi Git conformément au nouveau .gitignore
fa9b7992152d69daf3fe22651fd3a5cf3f0c1fc6 - jsboige, 2 months ago : Mise à jour de Spectre.Console vers la version 0.50.0 et corrections associées

## Analyse Détaillée des Commits
### Commit `365e4c6bf4e339bbcf0bf18a26d90cf7b8fd3613`

**Message :** fix(converter): Repair cardpen and asset converter logic

**Statistiques :**
```
.gitignore                                  |   13 +-
Cards/Rules/Argumentum_Rules_Back_fr.json   |   37 +
.../examples/Argumentum_Rules_Francais.json |   19 +-
Generation/CardPen/js/frame.js              |  154 +-
Generation/CardPen/js/main.js               | 2902 ++++++++++----------
Generation/CardPen/lib/unidecode.js         |   42 +
Generation/CardPen/lib/unidecode.js.REMOVED.git-id |    1 -
Generation/CardPen/package-lock.json        |  796 ++++++
Generation/CardPen/package.json             |    2 +-
Generation/CardPen/util/2-body.html         |   70 +
.../Argumentum.AssetConverter.csproj        |    5 +-
.../AssetConverterConfig.cs                 |    4 +-
.../WebBasedGenerator/Cardpen/HarvestManager.cs    |    3 +-
.../WebBasedGenerator/PdfManager.cs         |  229 +-
rebuild_unidecode.py                        |   19 +
15 files changed, 22621 insertions(+), 11675 deletions(-)
```

**Diff complet :**
```diff
commit 365e4c6bf4e339bbcf0bf18a26d90cf7b8fd3613 (HEAD -> master)
Author: jsboige <jsboige@gmail.com>
Date:   Fri Jul 18 21:45:43 2025 +0200

    fix(converter): Repair cardpen and asset converter logic

diff --git a/.gitignore b/.gitignore
index f14daee..702278b 100644
--- a/.gitignore
+++ b/.gitignore
@@ -162,6 +162,13 @@
 Cards/fr/
 Cards/pt/
 Cards/ru/
 Scripts/
-# Temporary folders
 .temp/
-temp_owl_sharp/
+# Temporary folders
+.temp/
+temp_owl_sharp/
+
+
+# Fichiers PDF générés à la racine
+/*.pdf
+
+# Dépendances Node.js de CardPen
+/Generation/CardPen/node_modules/
diff --git a/Cards/Rules/Argumentum_Rules_Back_fr.json b/Cards/Rules/Argumentum_Rules_Back_fr.json
new file mode 100644
index 0000000..e659b90
--- /dev/null
+++ b/Cards/Rules/Argumentum_Rules_Back_fr.json
@@ -0,0 +1,37 @@
+{
+  "name": "Argumentum - Rules - Back.fr",
+  "notes": "",
+  "dpi": 500,
+  "live": true,
+  "psize": "letter",
+  "pori": "landscape",
+  "csize": "tarot",
+  "cori": "portrait",
+  "cheight": "",
+  "cwidth": "",
+  "cunit": "mm",
+  "ccircle": false,
+  "gsize": 0,
+  "gunit": "mm",
+  "msize": 0.5,
+  "munit": "in",
+  "blsize": 3,
+  "blunit": "mm",
+  "ssize": 3,
+  "sunit": "mm",
+  "cutline": true,
+  "bradius": 0.125,
+  "brunit": "in",
+  "overlay": false,
+  "oopa": 1,
+  "oURL": "",
+  "extCSS": "https://fonts.googleapis.com/css2?family=Bebas+Neue&family=PT+Sans:wght@700&display=swap",
+  "css": "@font-face {\n    font-family: 'DINPro';\n    font-style: normal;\n    font-weight: 300;\n    src: local('DINPro'), url('https://fonts.cdnfonts.com/s/18774/DINPro-Light tr.woff') format('woff');\n}\n@font-face {\n    font-family: 'DINPro';\n    font-style: normal;\n    font-weight: 500;\n    src: local('DINPro'), url('https://fonts.cdnfonts.com/s/18774/DINPro-Medium tr.woff') format('woff');\n}\n@font-face {\n    font-family: 'DINPro';\n    font-style: normal;\n    font-weight: 700;\n    src: local('DINPro'), url('https://fonts.cdnfonts.com/s/18774/DINPro-Bold tr.woff') format('woff');\n}\n@font-face {\n    font-family: 'DINPro';\n    font-style: normal;\n    font-weight: 900;\n    src: local('DINPro'), url('https://fonts.cdnfonts.com/s/18774/DINPro-Black tr.woff') format('woff');\n}\n\ncard { \n  font-size: 50%;\n  font-family: 'DINPro', sans-serif;\n}\n\n.cardpenBleed {\n\tborder:0 !important;\n}\n\nbleed {\n  width:100%;\n  height:100%;\n  position:relative;\n  background:url('../../Cards/Rules/Assets/bg-rules.jpg') repeat;\n  background-size:100% 100%;\n  border-radius:0.125in;\n  overflow:hidden;\n}\n\n.cardContent {\n  position:absolute;\n  top:2%;\n  z-index:2;\n  width:84%;\n  height:95%;\n  display:flex;\n  flex-flow:column wrap;\n  justify-content:space-between;\n}\n\n/* Vibrant red vignette effect*/\n.vignetteOverlay {\n  width:200%;\n  height:100%;\n  position:relative;\n  left:-25%;\n\t-webkit-box-shadow:inset 3px 7px 70px 30px rgba(180, 0, 3,0.7);\n   -moz-box-shadow: inset 3px 7px 70px 30px rgba(180, 0, 3,0.7);\n    box-shadow: inset 3px 0px 40px 30px rgba(180, 0, 3,0.6);\n}\n\n/* Dark vignette effect*/\n.vignetteOverlay2 {\n  width:150%;\n  height:100%;\n  position:absolute;\n  top:0;\n  left:-25%;\n  -webkit-box-shadow: inset 3px 7px 63px 4px #000;\n  -moz-box-shadow: inset 3px 7px 63px 4px #000;\n   box-shadow: inset 3px 0px 40px 0px rgba(0,0,0,0.8);\n}\n\n.title {\n  display:flex;\n  flex-flow:column;\n  word-wrap:break-word;\n  position:relative;\n  width:90%;\n  height:54%;\n  max-height:3.2em;\n  padding:9% 12% 0% 9%;\n  font-weight:900;\n  font-size:8em;\n  line-height:0.85;\n  fill:#fff3de;\n  letter-spacing:0.03em;\n  align-items:flex-start;\n  z-index:3;\n}\n\n.title svg {\n\tmargin-bottom:4%;\n  \theight:29%\n}\n.title svg text {\n\tletter-spacing:0.09em;\n  \tfont-size:100px;\n    color:red;\n    stroke:#720000;\n    stroke-width:2px;\n    stroke-linecap:butt;\n    stroke-linejoin:miter;\n    stroke-opacity:0.3;\n}\n\n.pieces {\n  \theight:54%;\n  \tposition:absolute;\n  \tz-index:1;\n  \ttop:36%;\n  \tright:0;\n  \ttext-align:center;\n}\n\n.pieces img {\n  width:95%;\n}\n.footer {\n\tcolor:#fff3de;\n  \ttext-transform:uppercase;\n  \twidth:100%;\n  \ttext-align:center;\n  \tfont-family:'Bebas Neue';\n  \tletter-spacing:0.15em;\n  \tfont-size:2.1em;\n  \tfill:#fff3de;\n  \tpadding-bottom:0.25em;\n}\n\n",
+  "csv": "tagline_fr,tagline_en,tagline_ru\n\"Règles du jeu\",\"Game rules\",\"Правила игры\"",
+  "mustache": "<div class=\"cardContainer\">\n  <div class=\"vignetteOverlay\"></div>\n  <div class=\"vignetteOverlay2\"></div>\n  <div class=\"cardContent\">\n    <div class=\"title\">\n       <svg viewBox=\"0 0 280 80\">\n          <text x=\"0%\" y=\"90%\" stroke=\"none\" >ARGU</text>\n       </svg>\n      <svg viewBox=\"0 0 280 80\">\n          <text x=\"0%\" y=\"90%\" stroke=\"none\">MEN</text>\n       </svg>\n      <svg viewBox=\"0 0 280 80\">\n          <text x=\"0%\" y=\"90%\" stroke=\"none\">TUM</text>\n      </svg>\n    </div>\n    \n     <div class=\"pieces\">\n    \t<img src=\"../../Cards/Rules/Assets/logo.png\"/>\n     </div>\n    <div class=\"footer\">\n       <svg viewBox=\"0 0 280 80\">\n           <text x=\"50%\" y=\"90%\" stroke=\"none\" text-anchor=\"middle\">{{tagline_fr}}</text>\n       </svg>\n    </div>\n\n  </div>\n \n</div>",
+  "useMustache": true,
+  "cardClass": "",
+  "rscount": 1,
+  "rsstyle": "bunch",
+  "cindices": ""
+}
\ No newline at end of file
diff --git a/Generation/CardPen/examples/Argumentum_Rules_Francais.json b/Generation/CardPen/examples/Argumentum_Rules_Francais.json
index f4d8f67..7bb2474 100644
--- a/Generation/CardPen/examples/Argumentum_Rules_Francais.json
+++ b/Generation/CardPen/examples/Argumentum_Rules_Francais.json
@@ -26,10 +26,21 @@
   "oopa": 1,
   "oURL": "",
   "extCSS": "https://fonts.googleapis.com/css2?family=Bebas+Neue&family=PT+Sans:wght@700&display=swap",
-  "css": "@font-face {\n    font-family: 'DINPro';\n    font-style: normal;\n    font-weight: 300;\n    src: local('DINPro'), url('https://fonts.cdnfonts.com/s/18774/DINPro-Light tr.woff') format('woff');\n}\n@font-face {\n    font-family: 'DINPro';\n    font-style: normal;\n    font-weight: 500;\n    src: local('DINPro'), url('https://fonts.cdnfonts.com/s/18774/DINPro-Medium tr.woff') format('woff');\n}\n@font-face {\n    font-family: 'DINPro';\n    font-style: normal;\n    font-weight: 700;\n    src: local('DINPro'), url('https://fonts.cdnfonts.com/s/18774/DINPro-Bold tr.woff') format('woff');\n}\n@font-face {\n    font-family: 'DINPro';\n    font-style: normal;\n    font-weight: 900;\n    src: local('DINPro'), url('https://fonts.cdnfonts.com/s/18774/DINPro-Black tr.woff') format('woff');\n}\n\n@font-face {font-family: \"TrendSlabW00-Four\";\n    src: url(\"http://db.onlinewebfonts.com/t/8039f2717fa244ff6675db6a39c54d74.eot\"); /* IE9*/\n    src: url(\"http://db.onlinewebfonts.com/t/8039f2717fa244ff6675db6a39c54d74.eot?#iefix\") format(\"embedded-opentype\"), /* IE6-IE8 */\n    url(\"http://db.onlinewebfonts.com/t/8039f2717fa244ff6675db6a39c54d74.woff2\") format(\"woff2\"), /* chrome firefox */\n    url(\"http://db.onlinewebfonts.com/t/8039f2717fa244ff6675db6a39c54d74.woff\") format(\"woff\"), /* chrome firefox */\n    url(\"http://db.onlinewebfonts.com/t/8039f2717fa244ff6675db6a39c54d74.ttf\") format(\"truetype\"), /* chrome firefox opera Safari, Android, iOS 4.2+*/\n    url(\"http://db.onlinewebfonts.com/t/8039f2717fa244ff6675db6a39c54d74.svg#TrendSlabW00-Four\") format(\"svg\"); /* iOS 4.1- */\n}\n\n.insuffisance {\n  --color-background: #811da3;\n  --color-text-1: #601362;\n  --color-text-2: #8f5991;\n  --color-text-3: #a173a2;\n}\n\n.influence {\n  --color-background: #ff66eb;\n  --color-text-1: #b3009b;\n  --color-text-2: #cc00b1;\n  --color-text-3: #e566d4;\n  \n}\n\n.erreurMathématique {\n  --color-background: #08af93;\n  --color-text-1: #14555b;\n  --color-text-2: #5a888c;\n  --color-text-3: #749a9e;\n}\n\n.paralogisme {\n  --color-background: #8dc801;\n  --color-text-1: #476205;\n  --color-text-2: #7e9150;\n  --color-text-3: #92a26b;\n}\n\n.détournementDeLaLangue {\n  --color-background: #0054a4;\n  --color-text-1: #0c2861;\n  --color-text-2: #546890;\n  --color-text-3: #6f80a1;\n}\n\n.tricherie {\n--color-background: #ffc307ff;\n  --color-text-1: #9e7800ff;\n  --color-text-2: #c49500ff;\n  --color-text-3: #d6b755ff;\n}\n\n.obstruction {\n  --color-background: #dc0f0a;\n  --color-text-1: #960a07;\n  --color-text-2: #b55351;\n  --color-text-3: #c16e6c;\n}\n\n\nbody {\n  font-family:'DINPro',sans-serif;\n  font-size: 100%;\n  line-height:1.25;\n}\n\ncard {\n\n}\n\n.card2 h2:nth-of-type(1) {\n  --color-background:#e7ceec;\n  --color-box:#8605aa;\n}\n\n.card2 h2:nth-of-type(2) {\n  --color-background:#f8efd0;\n  --color-box:#daaf01;\n}\n\n.card3 h2:nth-of-type(1) {\n  --color-background:#cdede8;\n  --color-box:#01a986;\n}\n\n.card4 h3:nth-of-type(1){\n  \t--color-background:#e1f1cd;\n\t--color-box:#69b402;\n}\n\n.card4 h3:nth-of-type(2) {\n\t--color-background:#ffe6fc;\n  \t--color-box:#ff66eb;\n}\n\n.card4 h3:nth-of-
...
```

### Commit `5d298b9d6baae241ed1143d52c84ab1342f80811`

**Message :** fix(deps): Revert QuestPDF to 2023.12.0 to fix PDF generation

**Statistiques :**
```
.../WebBasedGenerator/PdfManager.cs | 82 ++++++++++-------------
1 file changed, 34 insertions(+), 48 deletions(-)
```

**Diff complet :**
```diff
commit 5d298b9d6baae241ed1143d52c84ab1342f80811
Author: jsboige <jsboige@gmail.com>
Date:   Tue Jul 15 20:14:38 2025 +0200

    fix(deps): Revert QuestPDF to 2023.12.0 to fix PDF generation

diff --git a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs
index c750ce8..e7798c2 100644
--- a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs
@@ -135,23 +135,16 @@
                     var pageCardsArray = pageCards.ToArray();

-                    try
+                    if (!docConfig.NoBack)
                     {
-                        if (!docConfig.NoBack)
-                        {
-                            Logger.Log($"Generating back page {pageIndex + 1}/{nbPages} for {fileName}");
-                            GenerateCardsPage(container, docConfig, pageSize, pageMarginMm, nbColumns, pageCardsArray, cardWidthPoints, cardImages => new MagickImage(cardImages.Back));
-                            pageCardsArray = pageCardsArray.ToJaggedArray(nbColumns).Select(row => row.Reverse().ToArray())
-                                .ToArray().Flatten();
-                        }
-
-                        Logger.Log($"Generating front page {pageIndex + 1}/{nbPages} for {fileName}");
-                        GenerateCardsPage(container, docConfig, pageSize, pageMarginMm, nbColumns, pageCardsArray, cardWidthPoints, cardImages => new MagickImage(cardImages.Front));
+                        Logger.Log($"Generating back page {pageIndex + 1}/{nbPages} for {fileName}");
+                        GenerateCardsPage(container, docConfig, pageSize, pageMarginMm, nbColumns, pageCardsArray, cardWidthPoints, cardImages => new MagickImage(cardImages.Back));
+                        pageCardsArray = pageCardsArray.ToJaggedArray(nbColumns).Select(row => row.Reverse().ToArray())
+                            .ToArray().Flatten();
                     }
-                    catch (Exception e)
-                    {
-                        Logger.LogException(e);
-                    }
+
+                    Logger.Log($"Generating front page {pageIndex + 1}/{nbPages} for {fileName}");
+                    GenerateCardsPage(container, docConfig, pageSize, pageMarginMm, nbColumns, pageCardsArray, cardWidthPoints, cardImages => new MagickImage(cardImages.Front));
                 }
                 })
                 .WithMetadata(docMetadata)
@@ -203,8 +196,8 @@
                 {
                     if (card != null)
                     {
-                        MagickImage toPrint = frontOrBack(card);
-                        PrintMagickImageIntoTableCell(toPrint, cell);
+                        MagickImage toPrint = frontOrBack(card);
+                        PrintMagickImageIntoTableCell(toPrint, cell);
                     }
                 });
             }
@@ -214,52 +207,45 @@
 
         private static void PrintMagickImageIntoTableCell(MagickImage toPrint, IContainer gridCell)
         {
-            try
+            if (toPrint == null)
             {
-                if (toPrint == null)
-                {
-                    Logger.LogWarning("Attempted to print a null MagickImage.");
-                    return;
-                }
+                Logger.LogWarning("Attempted to print a null MagickImage.");
+                return;
+            }

-                if (!string.IsNullOrEmpty(toPrint.FileName))
+            if (!string.IsNullOrEmpty(toPrint.FileName))
+            {
+                if (File.Exists(toPrint.FileName))
                 {
-                    if (File.Exists(toPrint.FileName))
-                    {
-                        gridCell.Image(toPrint.FileName);
-                    }
-                    else
-                    {
-                        Logger.LogWarning($"Image file not found: {toPrint.FileName}");
-                    }
+                    gridCell.Image(toPrint.FileName);
                 }
                 else
                 {
-                    using (var memStream = new MemoryStream())
+                    Logger.LogWarning($"Image file not found: {toPrint.FileName}");
+                }
+            }
+            else
+            {
+                using (var memStream = new MemoryStream())
+                {
+                    if (toPrint.Width > 0 && toPrint.Height > 0)
                     {
-                        if (toPrint.Width > 0 && toPrint.Height > 0)
+                        toPrint.Write(memStream);
+                        if (memStream.Length > 0)
                         {
-                            toPrint.Write(memStream);
-                            if (memStream.Length > 0)
-                            {
-                                gridCell.Image(memStream.ToArray());
-                            }
-                            else
-                            {
-                                Logger.LogWarning("MagickImage has no data to write to stream.");
-                            }
+                            gridCell.Image(memStream.ToArray());
                         }
                         else
                         {
-                            Logger.LogWarning("MagickImage has invalid dimensions.");
+                            Logger.LogWarning("MagickImage has no data to write to stream.");
                         }
                     }
+                    else
+                    {
+                        Logger.LogWarning("MagickImage has invalid dimensions.");
+                    }
                 }
             }
-            catch (Exception e)
-            {
-                Logger.LogException(e);
-            }
         }

         public void GeneratePdfsFromImages(List<(string fileName, Func<MagickImageCollection> documentImages)> targetFiles,
```

### Commit `6723d62855c18db75d262807c85bf4e6770d16a`

**Message :** feat: Clean up repository and update documentation

**Statistiques :**
```
...rds - edition fevrier 2022 - Print and Play.csv |  33 -
...um Fallacies - Cards - edition fevrier 2022.csv |  91 --
.../2022/Argumentum_Fallacies_Back_English.json    |  37 -
.../2022/Argumentum_Fallacies_Back_Francais.json   |  37 -
.../2022/Argumentum_Fallacies_Face_English.json    |  37 -
.../Argumentum_Fallacies_Face_English_Bis.json     |  37 -
.../2022/Argumentum_Fallacies_Face_Francais.json   |  37 -
.../Argumentum_Fallacies_Face_Francais_Bis.json    |  37 -
...ies_Face_Francais_Bis_edition_fevrier_2022.json |  37 -
...is_Bis_edition_fevrier_2022_Print_and_Play.json |  37 -
...Face_Francais_Bis_edition_fevrier_2022_Web.json |  37 -
...rancais_Bis_edition_fevrier_2022_Web_light.json |  37 -
...is_Bis_edition_fevrier_2022_Web_thumbnails.json |  37 -
.../2022/Argumentum_Fallacies_Face_v2_English.json |  37 -
.../Argumentum_Fallacies_Face_v2_Francais.json     |  37 -
.../Fallacies/Archive/Sprite/MariagePourTous.html  | 279 -----
Cards/Fallacies/Archive/Sprite/desktop.ini         |   5 -
Cards/Fallacies/Archive/Sprite/fallacies.html      | 961 --------------------
.../Sprite/rhetological_fallacies_francais.css     | 250 -----
...gical_fallacies_francaisFULL.png.REMOVED.git-id |   1 -
.../v1/Deck1/1.1.1.2.svg.png.REMOVED.git-id        |   1 -
Cards/Fallacies/Archive/v1/Deck1/1.1.3.svg.png     | Bin 384516 -> 0 bytes
Cards/Fallacies/Archive/v1/Deck1/1.1.svg.png       | Bin 332852 -> 0 bytes
Cards/Fallacies/Archive/v1/Deck1/1.2.1.2.svg.png   | Bin 457918 -> 0 bytes
Cards/Fallacies/Archive/v1/Deck1/1.2.2.3.svg.png   | Bin 492217 -> 0 bytes
.../Archive/v1/Deck1/1.2.2.svg.png.REMOVED.git-id  |   1 -
Cards/Fallacies/Archive/v1/Deck1/1.2.svg.png       | Bin 331830 -> 0 bytes
.../Archive/v1/Deck1/1.3.1.svg.png.REMOVED.git-id  |   1 -
Cards/Fallacies/Archive/v1/Deck1/1.3.2.1.svg.png   | Bin 483229 -> 0 bytes
... 467 more lines
487 files changed, 83 insertions(+), 40009 deletions(-)
```

**Diff complet :**
```diff
commit 6723d62855c18db75d262807c85bf4e6770d16a (origin/master, origin/HEAD)
Author: jsboige <jsboige@gmail.com>
Date:   Tue Jul 15 11:32:53 2025 +0200

    feat: Clean up repository and update documentation

... (diff too large to display)
```

### Commit `6edf683ce1530d202aba1abbd839fc13cb878381`

**Message :** feat: Refactor MindMap generation for Virtues and Fallacies

**Statistiques :**
```
.../Argumentum.AssetConverter.csproj               |    4 -
.../AssetConverterConfig.cs                        |   45 +-
.../Argumentum.AssetConverter/ConverterMode.cs     |   43 +-
.../CustomTypeProvider.cs                          |   77 +-
.../Argumentum.AssetConverter/Entities/CsvBase.cs  |  118 +-
.../Argumentum.AssetConverter/Entities/Fallacy.cs  |  128 +-
.../FallacyDocumentConfigBase.cs                   |   26 +-
.../IMindmapDocumentConfig.cs                      |   11 +
.../Json/TypeConverter.cs                          |   24 +
...torConfig.cs => FallacyMindMapCreatorConfig.cs} |  404 +++--
...ntConfig.cs => FallacyMindMapDocumentConfig.cs} | 2091 ++++++++++----------
.../Mindmapper/VirtueMindMapCreatorConfig.cs       |   71 +
.../Mindmapper/VirtueMindMapDocumentConfig.cs      |  958 +++++++++
.../Ontology/OwlGeneratorConfig.cs                 |   10 +-
.../ParallelDocumentCreatorConfigBase.cs           |  138 ++
.../ParallelFallacyDocumentCreatorConfigBase.cs    |  149 +-
.../ParallelVirtueDocumentCreatorConfigBase.cs     |   10 +
.../Tests/TaxonomyValidationTests.cs               | 1038 +++++-----
.../Tests/TranslationCoverageReport.cs             | 2046 +++++++++----------
.../VirtueDocumentConfigBase.cs                    |   13 +
.../WebBasedGenerator/Cardpen/HarvestManager.cs    |    2 +
.../WebBasedGenerator/DataSetInfo.cs               |  922 ++++-----
.../WebBasedGenerator/DocumentConfig.cs            |  150 +-
.../WebBasedGenerator/ImageFileGenerator.cs        |  231 ++--
.../Localization/CardSetLocalization.cs            |  193 +-
.../Localization/DocumentLocalization.cs           |  122 +-
.../Localization/LocalizationConfig.cs             |  120 +-
.../WebBasedGenerator/PdfManager.cs                |  647 +++---
.../WebBasedGenerator/WebBasedGenerator.cs         |  252 ++--
.../WebBasedGenerator/WebBasedGeneratorConfig.cs   |    2 +-
NuGet.Config                                       |    7 -
31 files changed, 5529 insertions(+), 4523 deletions(-)
```

**Diff complet :**
```diff
commit 6edf683ce1530d202aba1abbd839fc13cb878381
Author: jsboige <jsboige@gmail.com>
Date:   Sat Jul 12 19:10:01 2025 +0200

    feat: Refactor MindMap generation for Virtues and Fallacies

... (diff too large to display)
```

### Commit `fc62618c1bff417986bb4e99bcb55c0db000c627`

**Message :** Fix(Converter): Resolve ParseException and enable Virtue generation

**Statistiques :**
```
.../AssetConverterConfig.cs                        |  80 +++--
.../Argumentum.AssetConverter/Entities/Virtue.cs   |  62 ++++
.../Mindmapper/MindMapDocumentConfig.cs            | 371 +++++++++++---------
3 files changed, 316 insertions(+), 197 deletions(-)
```

**Diff complet :**
```diff
commit fc62618c1bff417986bb4e99bcb55c0db000c627
Author: jsboige <jsboige@gmail.com>
Date:   Tue Jul 8 15:39:31 2025 +0200

    Fix(Converter): Resolve ParseException and enable Virtue generation

diff --git a/Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs b/Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs
index 165b882..963885c 100644
--- a/Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs
@@ -4,6 +4,7 @@
 using System.Diagnostics;
 using System.IO;
 using System.Runtime.Serialization;
 using System.Text;
+using System.Text.Json;
 using System.Text.Json.Serialization;
 using System.Threading.Tasks;
 using System.Xml.Serialization;
@@ -16,9 +17,6 @@
 using Argumentum.AssetConverter.Optimization;
 using Argumentum.AssetConverter.Tests;
 using Spectre.Console;
 using Spectre.Console.Json;
-using Utf8Json;
-using Utf8Json.Formatters;
-using Utf8Json.Resolvers;
 
 namespace Argumentum.AssetConverter
 {
@@ -28,9 +26,10 @@
     {
 
         //Debug Switch to configure default values
-           public bool SkipConfigFile { get; set; } = false;
+           public bool SkipConfigFile { get; set; } = true;
 
-           public ConverterMode Mode { get; set; } = ConverterMode.WebBasedImageGeneration | ConverterMode.Mindmapper | ConverterMode.OwlGenerator; // | ConverterMode.WebBasedImageGeneration; // ConverterMode.DatasetUpdater;
+              [JsonConverter(typeof(JsonStringEnumConverter))]
+              public ConverterMode Mode { get; set; } = ConverterMode.WebBasedImageGeneration | ConverterMode.Mindmapper | ConverterMode.OwlGenerator; // | ConverterMode.WebBasedImageGeneration; // ConverterMode.DatasetUpdater;
 
         public bool ForceDebugParams { get; set; }
 
@@ -70,7 +69,7 @@
                    new DataSetInfo()
                    {
                    Name = KnownDataSets.VirtuesTaxonomy,
-                  CsvType = typeof(ArgumentVirtue),
+                  CsvType = typeof(Virtue),
                    ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum%20Virtues%20-%20Taxonomy.csv",
                    DebugFilePath = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum Virtues - Taxonomy.csv"
                    }
@@ -155,7 +154,7 @@
                    }
 }),
                    StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new []{
-                  (("L'art de jamais avoir tort", new List<(string Language, string destFieldName)>(new []{("en", "The art of never being wrong"), ("ru", "Искусство никогда не ошибаться"), ("pt", "A arte de nunca errar") }) ),
+                  (("L'art de jamais avoir tort", new List<(string Language, string destText)>(new []{("en", "The art of never being wrong"), ("ru", "Искусство никогда не ошибаться"), ("pt", "A arte de nunca errar") }) ),
                    }
 }),
                    FrontFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
... (diff too large to display)
```

### Commit `fc70138d6ce0efb3608cc45b833caf0bd40e8d546`

**Message :** fix: Refactor card generation to be stateless

**Statistiques :**
```
Impossible de trouver le commit. Le SHA est peut-être incorrect ou le commit a été supprimé de l'historique.
```

**Diff complet :**
```diff
N/A
```

### Commit `e8482fe5d50fcc1778effeba85311413114004121`

**Message :** fix(asset-converter): Repair OWL generation after OwlSharp library upgrade

**Statistiques :**
```
Impossible de trouver le commit. Le SHA est peut-être incorrect ou le commit a été supprimé de l'historique.
```

**Diff complet :**
```diff
N/A
```

### Commit `ceb86b06438f1777cbbb64790a604cf91255a7ab`

**Message :** build: Ignore les fichiers de projet non suivis pour nettoyer l'arbre de travail

**Statistiques :**
```
.gitignore | 8 +++++++++
1 file changed, 8 insertions(+)
```

**Diff complet :**
```diff
commit ceb86b06438f1777cbbb64790a604cf91255a7ab
Author: jsboige <jsboige@gmail.com>
Date:   Mon Jun 30 19:33:25 2025 +0200

    build: Ignore les fichiers de projet non suivis pour nettoyer l'arbre de travail

diff --git a/.gitignore b/.gitignore
index d400640..a0649e6 100644
--- a/.gitignore
+++ b/.gitignore
@@ -154,3 +154,11 @@
 bfg.jar
 
 # Dossiers de sortie générés
 **/[Oo]utput/
+
+
+# Fichiers et répertoires de projet non suivis
+Cards/en/
+Cards/fr/
+Cards/pt/
+Cards/ru/
+Scripts/
```

### Commit `b09d4522d7bb6faa0f69122819f277b7154c62d6`

**Message :** build: Ignore les répertoires de sortie générés

**Statistiques :**
```
.gitignore | 4 ++++
1 file changed, 4 insertions(+)
```

**Diff complet :**
```diff
commit b09d4522d7bb6faa0f69122819f277b7154c62d6
Author: jsboige <jsboige@gmail.com>
Date:   Mon Jun 30 19:32:38 2025 +0200

    build: Ignore les répertoires de sortie générés

diff --git a/.gitignore b/.gitignore
index 843b1d8..d400640 100644
--- a/.gitignore
+++ b/.gitignore
@@ -150,3 +150,7 @@
 Argumentum.git-mirror/
 *.bfg-report/
 bfg.jar
+
+
+# Dossiers de sortie générés
+**/[Oo]utput/
```

### Commit `3cc8af23a9f7efb0ee74b8911a225fc28463837e`

**Message :** build: Met à jour .gitignore pour exclure les artefacts de nettoyage BFG et l'exécutable bfg.jar

**Statistiques :**
```
.gitignore | 7 +++++++
1 file changed, 7 insertions(+)
```

**Diff complet :**
```diff
commit 3cc8af23a9f7efb0ee74b8911a225fc28463837e
Author: jsboige <jsboige@gmail.com>
Date:   Mon Jun 30 19:31:04 2025 +0200

    build: Met à jour .gitignore pour exclure les artefacts de nettoyage BFG et l'exécutable bfg.jar

diff --git a/.gitignore b/.gitignore
index ebdffc5..843b1d8 100644
--- a/.gitignore
+++ b/.gitignore
@@ -143,3 +143,10 @@
 yarn-error.log*
 
 # Fichiers de configuration locaux
 /Chatgpt-plugin/azure-function/local.settings.json
+
+
+# BFG & Mirror Repo Artifacts
+Argumentum.git-mirror/
+*.bfg-report/
+
+bfg.jar
```

### Commit `949a09cae8348adbda6c78723ed4d621a27fbd1b`

**Message :** Ajout des fichiers de configuration et adaptation de la classe Fallacy avec une propriété Id

**Statistiques :**
```
AssetConverterConfig.json | 2386 ++++++++++++++++++++
.../Argumentum.AssetConverter/Entities/Fallacy.cs | 11 +-
.../Mindmapper/MindMapDocumentConfig.cs | 4 +-
Program.cs | 51 +
4 files changed, 2447 insertions(+), 5 deletions(-)
```

**Diff complet :**
```diff
commit 949a09cae8348adbda6c78723ed4d621a27fbd1b
Author: jsboige <jsboige@gmail.com>
Date:   Tue May 13 18:09:35 2025 +0200

    Ajout des fichiers de configuration et adaptation de la classe Fallacy avec une propriété Id

    Former-commit-id: 9cb631f4740e8de49442f27e5888f6c195c431ee
    Former-commit-id: eb2c2c1b6fe76d06a887e2a51799524e0c2a7f1a

diff --git a/AssetConverterConfig.json b/AssetConverterConfig.json
new file mode 100644
index 0000000..3e1f5e3
--- /dev/null
+++ b/AssetConverterConfig.json
@@ -0,0 +1,2386 @@
+{
+  "Mode": "WebBasedImageGeneration",
+  "ForceDebugParams": false,
+  "ForceReleaseParams": false,
+  "WebBasedGeneratorConfig": {
+    "EnableSVGPrompt": true,
+    "ShowInfoLogs": true,
+    "HeadLessBrowser": false,
+    "OverwriteExistingDocs": false,
+    "OverwriteExistingHtmlMaps": false,
+    "MaxDegreeOfParallelismCardpen": 3,
+    "MaxDegreeOfParallelismCardpenTranslations": 2,
+    "MaxDegreeOfParallelismImages": 3,
+    "MaxDegreeOfParallelismImageTranslations": 2,
+    "MaxDegreeOfParallelismDocuments": 4,
+    "MaxDegreeOfParallelismMindMaps": 6,
+    "BaseTargetDirectoryName": "Target\\",
+    "HarvestDirectoryName": "Harvest\\",
+    "ImagesDirectoryName": "Images\\",
+    "DocumentsDirectoryName": "Documents\\",
+    "ReleaseCardpenUrl": "https://argumentumgames.github.io/Argumentum/Generation/CardPen/index.html",
+    "DebugCardpenUrl": "http://cardpen.dnndev.me/Generation/CardPen/index.html",
+    "DataSets": [
+      {
+        "Name": "Rules",
+        "ReleaseFilePath": "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Rules/Argumentum%20Rules%20-%20Cards.csv",
+        "DebugFilePath": "..\\..\\..\\..\\..\\..\\Cards\\Rules\\Argumentum Rules - Cards.csv",
+        "CsvType": null
+      },
+      {
+        "Name": "Rules - Print & Play",
+        "ReleaseFilePath": "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Rules/Argumentum%20Rules%20-%20Cards%20Print%20and%20Play.csv",
+        "DebugFilePath": "..\\..\\..\\..\\..\\..\\Cards\\Rules\\Argumentum Rules - Cards Print and Play.csv",
+        "CsvType": null
+      },
+      {
+        "Name": "Scenarii",
+        "ReleaseFilePath": "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Argumentum%20Scenarii%20-%20Cards.csv",
+        "DebugFilePath": "..\\..\\..\\..\\..\\..\\Cards\\Scenarii\\Argumentum Scenarii - Cards.csv",
+        "CsvType": null
+      },
+      {
+        "Name": "Fallacies - Taxonomy",
+        "ReleaseFilePath": "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum%20Fallacies%20-%20Taxonomy.csv",
+        "DebugFilePath": "..\\..\\..\\..\\..\\..\\Cards\\Fallacies\\Argumentum Fallacies - Taxonomy.csv",
+        "CsvType": "Argumentum.AssetConverter.Entities.Fallacy, Argumentum.AssetConverter, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null"
+      },
+      {
+        "Name": "Fallacies - Virtues",
+        "ReleaseFilePath": "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum%20Virtues%20-%20Taxonomy.csv",
+        "DebugFilePath": "..\\..\\..\\..\\..\\..\\Cards\\Fallacies\\Argumentum Virtues - Taxonomy.csv",
+        "CsvType": "Argumentum.AssetConverter.Entities.ArgumentVirtue, Argumentum.AssetConverter, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null"
+      }
+    ],
+    "CardSets": [
+      {
+        "Name": "Rules",
+        "FaceCardSetInfo": {
+          "DataSet": "Rules",
+          "CsvFilterField": null,
+          "CsvFilterValues": [
+
+          ],
+          "SkipDataUpdate": false,
+          "JsonFilePathRelease": "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Rules/Argumentum_Rules_fr.json",
+          "JsonFilePathDebug": "..\\..\\..\\..\\..\\..\\Cards\\Rules\\Argumentum_Rules_fr.json",
+          "PauseForEdits": false,
+          "FieldsLocalization": [
+
+          ],
+          "Dpi": 0,
+          "RowsetNb": 0
+        },
+        "BackCardSetInfo": {
+          "DataSet": null,
+          "CsvFilterField": null,
+          "CsvFilterValues": [
+
+          ],
+          "SkipDataUpdate": false,
+          "JsonFilePathRelease": null,
+          "JsonFilePathDebug": null,
+          "PauseForEdits": false,
+          "FieldsLocalization": [
+
+          ],
+          "Dpi": 0,
+          "RowsetNb": 0
+        }
+      },
+      {
+        "Name": "Memo",
+        "FaceCardSetInfo": {
+          "DataSet": "Fallacies - Taxonomy",
+          "CsvFilterField": "carte",
+          "CsvFilterValues": [
+            "1",
+            "2"
+          ],
+          "SkipDataUpdate": false,
+          "JsonFilePathRelease": "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Memo/Argumentum_Memo_Face_fr.json",
+          "JsonFilePathDebug": "..\\..\\..\\..\\..\\..\\Cards\\Memo\\Argumentum_Memo_Face_fr.json",
+          "PauseForEdits": false,
+          "FieldsLocalization": [
+
+          ],
+          "Dpi": 0,
+          "RowsetNb": 0
+        },
+        "BackCardSetInfo": {
+          "DataSet": "Fallacies - Taxonomy",
+          "CsvFilterField": "carte",
+          "CsvFilterValues": [
+            "1",
+            "2"
+          ],
+          "SkipDataUpdate": false,
+          "JsonFilePathRelease": "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Memo/Argumentum_Memo_Back_fr.json",
+          "JsonFilePathDebug": "..\\..\\..\\..\\..\\..\\Cards\\Memo\\Argumentum_Memo_Back_fr.json",
+          "PauseForEdits": false,
+          "FieldsLocalization": [
+
+          ],
+          "Dpi": 0,
+          "RowsetNb": 0
+        }
+      },
+      {
+        "Name": "Fallacies",
+        "FaceCardSetInfo": {
+          "DataSet": "Fallacies - Taxonomy",
+          "CsvFilterField": "carte",
+          "CsvFilterValues": [
+            "1",
+            "2"
+          ],
+          "SkipDataUpdate": false,
+          "JsonFilePathRelease": "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_fr.json",
+          "JsonFilePathDebug": "..\\..\\..\\..\\..\\..\\Cards\\Fallacies\\Argumentum_Fallacies_Face_fr.json",
+          "PauseForEdits": false,
+          "FieldsLocalization": [
+
+          ],
+          "Dpi": 0,
+          "RowsetNb": 0
+        },
+        "BackCardSetInfo": {
+          "DataSet": "",
+          "CsvFilterField": null,
+          "CsvFilterValues": [
+
+          ],
+          "SkipDataUpdate": false,
+          "JsonFilePathRelease": "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Back_fr.json",
+          "JsonFilePathDebug": "..\\..\\..\\..\\..\\..\\Cards\\Fallacies\\Argumentum_Fallacies_Back_fr.json",
+          "PauseForEdits": false,
+          "FieldsLocalization": [
+
+          ],
+          "Dpi": 0,
+          "RowsetNb": 0
+        }
+      },
+      {
+        "Name": "Virtues",
+        "FaceCardSetInfo": {
+          "DataSet": "Fallacies - Virtues",
+          "CsvFilterField": "card",
+          "CsvFilterValues": [
+            "1"
+          ],
+          "SkipDataUpdate": false,
+          "JsonFilePathRelease": "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Virtues_Face_fr.json",
+          "JsonFilePathDebug": "..\\..\\..\\..\\..\\..\\Cards\\Fallacies\\Argumentum_Virtues_Face_fr.json",
+          "PauseForEdits": false,
+          "FieldsLocalization": [
+
+          ],
+          "Dpi": 0,
+          "RowsetNb": 0
+        },
+        "BackCardSetInfo": {
+          "DataSet": "",
+          "CsvFilterField": null,
+          "CsvFilterValues": [
+
+          ],
+          "SkipDataUpdate": false,
+          "JsonFilePathRelease": "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Back_fr.json",
+          "JsonFilePathDebug": "..\\..\\..\\..\\..\\..\\Cards\\Fallacies\\Argumentum_Fallacies_Back_fr.json",
+          "PauseForEdits": false,
+          "FieldsLocalization": [
+
+          ],
+          "Dpi": 0,
+          "RowsetNb": 0
+        }
+      },
+      {
+        "Name": "Scenarii",
+        "FaceCardSetInfo": {
+          "DataSet": "Scenarii",
+          "CsvFilterField": null,
+          "CsvFilterValues": [
+
+          ],
+          "SkipDataUpdate": false,
+          "JsonFilePathRelease": "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Argumentum_Scenarii_Face_fr.json",
+          "JsonFilePathDebug": "..\\..\\..\\..\\..\\..\\Cards\\Scenarii\\Argumentum_Scenarii_Face_fr.json",
+          "PauseForEdits": false,
+          "FieldsLocalization": [
+
+          ],
+          "Dpi": 0,
+          "RowsetNb": 0
+        },
+        "BackCardSetInfo": {
+        },
+        "CardSetLocalization": [
+          {
+            "CardSetNames": [
+              "Fallacies",
+              "Fallacies-Print&Play"
+            ],
+            "FrontFieldConversions": [
+              {
+                "Item1": "nom",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "name"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "name_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "name_pt"
+                  }
+                ]
+              },
+              {
+                "Item1": "description",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "description"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "description_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "description_pt"
+                  }
+                ]
+              },
+              {
+                "Item1": "exemple",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "example"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "example_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "example_pt"
+                  }
+                ]
+              },
+              {
+                "Item1": "image",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "image"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "image_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "image_pt"
+                  }
+                ]
+              }
+            ],
+            "BackFieldConversions": [
+              {
+                "Item1": "citation",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "quote"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "quote_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "quote_pt"
+                  }
+                ]
+              },
+              {
+                "Item1": "auteur",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "author"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "author_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "author_pt"
+                  }
+                ]
+              }
+            ],
+            "ExceptionPatterns": [
+              "{{row.catégorie}}.jpg",
+              "{{row.catégorie}}.png"
+            ],
+            "StaticConversions": [
+
+            ],
+            "TargetProperties": [
+
+            ]
+          },
+          {
+            "CardSetNames": [
+              "Scenarii",
+              "Scenarii-Print&Play"
+            ],
+            "FrontFieldConversions": [
+              {
+                "Item1": "intitulé",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "title"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "title_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "title_pt"
+                  }
+                ]
+              },
+              {
+                "Item1": "enjeu",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "issue"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "issue_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "issue_pt"
+                  }
+                ]
+              },
+              {
+                "Item1": "piocheur",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "drawer"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "drawer_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "drawer_pt"
+                  }
+                ]
+              },
+              {
+                "Item1": "baratineur",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "smoothTalker"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "smoothTalker_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "smoothTalker_pt"
+                  }
+                ]
+              },
+              {
+                "Item1": "suggestion",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "suggestion_en"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "suggestion_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "suggestion_pt"
+                  }
+                ]
+              }
+            ],
+            "BackFieldConversions": [
+              {
+                "Item1": "catégorie",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "category"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "category_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "category_pt"
+                  }
+                ]
+              }
+            ],
+            "ExceptionPatterns": [
+              "{{rowset.[0].catégorie}}.jpg",
+              "{{rowset.[0].catégorie}}.png"
+            ],
+            "StaticConversions": [
+
+            ],
+            "TargetProperties": [
+
+            ]
+          },
+          {
+            "CardSetNames": [
+              "Rules",
+              "Rules-Print&Play"
+            ],
+            "FrontFieldConversions": [
+              {
+                "Item1": "Text",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "Text_en"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "Text_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "Text_pt"
+                  }
+                ]
+              }
+            ],
+            "BackFieldConversions": [
+
+            ],
+            "ExceptionPatterns": [
+
+            ],
+            "StaticConversions": [
+
+            ],
+            "TargetProperties": [
+
+            ]
+          },
+          {
+            "CardSetNames": [
+              "Memo",
+              "Memo-Print&Play"
+            ],
+            "FrontFieldConversions": [
+              {
+                "Item1": "Famille",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "Family"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "Family_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "Family_pt"
+                  }
+                ]
+              },
+              {
+                "Item1": "desc_fr",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "desc_en"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "desc_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "desc_pt"
+                  }
+                ]
+              }
+            ],
+            "BackFieldConversions": [
+              {
+                "Item1": "Soussousfamille",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "Subsubfamily"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "Subsubfamily_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "Subsubfamily_pt"
+                  }
+                ]
+              },
+              {
+                "Item1": "Sous-Famille",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "Subfamily"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "Subfamily_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "Subfamily_pt"
+                  }
+                ]
+              },
+              {
+                "Item1": "Famille",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "Family"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "Family_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "Family_pt"
+                  }
+                ]
+              },
+              {
+                "Item1": "tagline_fr",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "tagline_en"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "tagline_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "tagline_pt"
+                  }
+                ]
+              }
+            ],
+            "ExceptionPatterns": [
+
+            ],
+            "StaticConversions": [
+              {
+                "Item1": "L'art de jamais avoir tort",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "The art of never being wrong"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "Искусство никогда не ошибаться"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "A arte de nunca errar"
+                  }
+                ]
+              }
+            ],
+            "TargetProperties": [
+
+            ]
+          }
+        ],
+        "MindMapLocalization": [
+          {
+            "StaticConversions": [
+              {
+                "Item1": "_fr",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "_en"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "_ru"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "_pt"
+                  }
+                ]
+              }
+            ],
+            "TargetProperties": [
+              "DocumentName"
+            ]
+          },
+          {
+            "StaticConversions": [
+              {
+                "Item1": "Famille",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "Family"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "FamilyRu"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "FamilyPt"
+                  }
+                ]
+              }
+            ],
+            "TargetProperties": [
+              "TitleExpression",
+              "CardExpression",
+              "FamilleExpression"
+            ]
+          },
+          {
+            "StaticConversions": [
+              {
+                "Item1": "SousFamille",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "Subfamily"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "SubfamilyRu"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "SubfamilyPt"
+                  }
+                ]
+              }
+            ],
+            "TargetProperties": [
+              "TitleExpression",
+              "CardExpression"
+            ]
+          },
+          {
+            "StaticConversions": [
+              {
+                "Item1": "Soussousfamille",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "Subsubfamily"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "SubsubfamilyRu"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "SubsubfamilyPt"
+                  }
+                ]
+              }
+            ],
+            "TargetProperties": [
+              "TitleExpression",
+              "CardExpression"
+            ]
+          },
+          {
+            "StaticConversions": [
+              {
+                "Item1": "TextFr",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "TextEn"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "TextRu"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "TextPt"
+                  }
+                ]
+              }
+            ],
+            "TargetProperties": [
+              "TitleExpression",
+              "CardExpression"
+            ]
+          },
+          {
+            "StaticConversions": [
+              {
+                "Item1": "DescFr",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "DescEn"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "DescRu"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "DescPt"
+                  }
+                ]
+              }
+            ],
+            "TargetProperties": [
+              "DescriptionExpression"
+            ]
+          },
+          {
+            "StaticConversions": [
+              {
+                "Item1": "ExampleFr",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "ExampleEn"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "ExampleRu"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "ExamplePt"
+                  }
+                ]
+              }
+            ],
+            "TargetProperties": [
+              "ExampleExpression"
+            ]
+          },
+          {
+            "StaticConversions": [
+              {
+                "Item1": "LinkFrFallback",
+                "Item2": [
+                  {
+                    "Item1": "en",
+                    "Item2": "LinkEnFallback"
+                  },
+                  {
+                    "Item1": "ru",
+                    "Item2": "LinkRuFallback"
+                  },
+                  {
+                    "Item1": "pt",
+                    "Item2": "LinkPtFallback"
+                  }
+                ]
+              }
+            ],
+            "TargetProperties": [
+              "LinkExpression"
+            ]
+          }
+        ]
+      }
+    ]
+  },
+  "BatchImageConverterConfig": {
+    "SourcePath": "..\\..\\..\\Data\\Source",
+    "DestPath": "..\\..\\..\\Data\\Target",
+    "Operation": "ModulateHue",
+    "Modulation": 200
+  },
+  "DatasetUpdaterConfig": {
+    "SystemPrompt": "Vous incarnez un éditeur de Json hautement compétent, dont la mission consiste à corriger les champs d'un document Json. Le document à modifier a été divisé en portions de Json qui vous sont soumises sous forme de prompts d'utilisateur. Votre réponse doit contenir la même structure, mais avec les champs corrigés.\r\n\r\nLe fichier Json à mettre à jour contient des éléments d'une taxonomie d'arguments vertueux en français. Ce travail vise à contrer une taxonomie de sophismes. Les deux taxonomies utilisent des termes dont la signification est éclaircie par leur place dans la hiérarchie. Dans certains cas, elle diffère légèrement de la définition courante. De plus, la signification des termes de cette taxonomie est conçue pour correspondre à leurs sophismes opposés.\r\n\r\nVoilà les branches principales de la taxonomie des sophismes que vous devez connaître pour identifier le bon sens des termes utilisés:\r\n\r\npath\ttext_fr\r\n1\tInsuffisance\r\n1.1\tArgument bâclé\r\n1.1.1\tArgument vide\r\n1.1.2\tJustification triviale\r\n1.2\tPréjugé\r\n1.2.1\tArgument d'autorité\r\n1.2.2\tSophisme naturaliste\r\n1.2.3\tSophisme moraliste\r\n1.3\tSurinterprétation\r\n1.3.1\tSophisme ludique\r\n1.3.2\tMauvaises raisons\r\n1.3.3\tManque de parcimonie\r\n2\tInfluence\r\n2.1\tProcédé rhétorique\r\n2.1.1\tLangage persuasif\r\n2.1.2\tHumour\r\n2.1.3\tPoésie\r\n2.2\tAppel à l'émotion\r\n2.2.1\tConnivence\r\n2.2.2\tRepoussoir\r\n2.2.3\tAppel aux conséquences\r\n2.3\tManipulation mentale\r\n2.3.1\tConditionnement\r\n2.3.2\tJeu de pouvoir\r\n2.3.3\tInfluence non verbale\r\n3\tErreur mathématique\r\n3.1\tGénéralisation abusive\r\n3.1.1\tÉchantillon biaisé\r\n3.1.2\tSophisme de l'accident\r\n3.1.3\tTransfert illicite\r\n3.2\tMauvaise interprétation\r\n3.2.1\tRelation infondée\r\n3.2.2\tProbabilités faussées\r\n3.2.3\tJustification infinie\r\n3.3\tRésultat invalide\r\n3.3.1\tImprécision\r\n3.3.2\tErreur de calcul\r\n3.3.3\tOpération inappropriée\r\n4\tErreur de raisonnement\r\n4.1\tCausalité douteuse\r\n4.1.1\tPétition de principe\r\n4.1.2\tInversion de causalité\r\n4.1.3\tEffet cigogne\r\n4.2\tMauvaise composition\r\n4.2.1\tErreur de logique propositionnelle\r\n4.2.2\tErreur de quantification\r\n4.2.3\tErreur de modalité\r\n4.3\tMauvaise déduction\r\n4.3.1\tConclusion hâtive\r\n4.3.2\tInconsistance\r\n4.3.3\tSyllogisme invalide\r\n5\tAbus de langage\r\n5.1\tDéfinition imprécise\r\n5.1.1\tAcception vague\r\n5.1.2\tAcception arbitraire\r\n5.1.3\tDéfinition inconsistante\r\n5.2\tComparaison fallacieuse\r\n5.2.1\tComparaison abusive\r\n5.2.2\tFausse analogie\r\n5.2.3\tSophisme d'association\r\n5.3\tAmbiguïté\r\n5.3.1\tAmphibologie\r\n5.3.1.4\tSolécisme\r\n5.3.1.5\tBarbarisme\r\n5.3.2\tEquivoque\r\n5.3.3\tAmbiguïté narrative\r\n6\tTricherie\r\n6.1\tArranger les faits\r\n6.1.1\tMensonge\r\n6.1.2\tFausse attribution\r\n6.1.3\tAttention sélective\r\n6.2\tChangement de cap\r\n6.2.1\tExigence renforcée\r\n6.2.2\tBeurre et argent du beurre\r\n6.2.3\tExigence relâchée\r\n6.3\tPensée biaisée\r\n6.3.1\tAnthropocentrisme\r\n6.3.2\tEthnocentrisme\r\n6.3.3\tDogmatisme\r\n7\tObstruction\r\n7.1\tRefus du débat\r\n7.1.1\tRelativisme abusif\r\n7.1.2\tSophisme d'Explication\r\n7.1.3\tPreuve par assertion\r\n7.2\tSaboter le débat\r\n7.2.1\tFausse piste\r\n7.2.2\tComplication exagérée\r\n7.2.3\tEmpoisonner le puits\r\n7.3\tAd hominem\r\n7.3.1\tProcès en inconstance\r\n7.3.2\tSophisme génétique\r\n7.3.3\tAttaque personnelle\r\n\r\nLes instructions sont les suivantes, dans l'ordre de priorité:\r\n\r\n- Le champ title_fr devrait être une expression courte, et couvrir au plus possible l'inverse des arguments fallacieux en miroir dans la seconde taxonomie. \r\n- Le champs description_fr devrait être une phrase simple, aussi complète que possible mais synthétique: Si la valeur passée en entrée pour le champ remark_fr est redondante et plus complète, elle pourra servir à compléter le champ description \r\n- le champs remark_fr devrait quant à lui contenir un paragraphe complémentaire distinct du champs description_fr, typiquement un exemple illustrant le concept évoqué, ou un contre-exemple qui illustre le défaut d'application du concept, ou encore une explication importante. \r\n- Le champs link_fr contient actuellement un certain nombre de liens hallucinés. Il s'agit d'y inclure uniquement des liens pertinents dont l'existance est averrée, et le conserver vide si aucun lien sûr n'est pertinent.\r\n\r\nA chaque fois que vous recevez un prompt d'utilisateur contenant une portion du document Json, votre travail est de la renvoyer en retour avec tous les champs correctement remplis ou améliorés si possible.",
+    "UserPrompt": "[\r\n\t{\r\n\t\t\"path\": \"4.3.3.1.2.1.1\",\r\n\t\t\"family_fr\": \"Raisonnement valide\",\r\n\t\t\"subfamily_fr\": \"Déductions correctes\",\r\n\t\t\"subsubfamily_fr\": \"Raisonnement concluant\",\r\n\t\t\"title_fr\": \"Syllogisme Barbara\",\r\n\t\t\"description_fr\": \"Le syllogisme Barbara est un syllogisme de mode parfait\",\r\n\t\t\"remark_fr\": \"Le syllogisme Barbara est un exemple de syllogisme de mode parfait qui présente une structure de raisonnement concluant particulièrement forte.\",\r\n\t\t\"link_fr\": \"\"\r\n\t},\r\n\t{\r\n\t\t\"path\": \"1\",\r\n\t\t\"family_fr\": \"Argument pertinent\",\r\n\t\t\"subfamily_fr\": \"Argument fondé\",\r\n\t\t\"subsubfamily_fr\": \"\",\r\n\t\t\"title_fr\": \"Argument fondé\",\r\n\t\t\"description_fr\": \"Argument correct\",\r\n\t\t\"link_fr\": \"\",\r\n\t\t\"remark_fr\": \"Argument correctement formulé et recevable\"\r\n\t},\r\n\t{\r\n\t\t\"path\": \"4.1.2\",\r\n\t\t\"family_fr\": \"Raisonnement valide\",\r\n\t\t\"subfamily_fr\": \"Identification précise des causalités\",\r\n\t\t\"subsubfamily_fr\": \"Causalité correctement orientée\",\r\n\t\t\"title_fr\": \"Causalité dans le bon sens\",\r\n\t\t\"description_fr\": \"Il ne faut pas confondre cause et effet\",\r\n\t\t\"remark_fr\": \"Bien orienter la causalité évite les erreurs d'attribution ou le fait de confondre une cause avec un effet\",\r\n\t\t\"link_fr\": \"\"\r\n\t}\r\n]",
+    "AssistantPrompt": "[\r\n\t{\r\n\t\t\"path\": \"4.3.3.1.2.1.1\",\r\n\t\t\"family_fr\": \"Raisonnement valide\",\r\n\t\t\"subfamily_fr\": \"Déductions correctes\",\r\n\t\t\"subsubfamily_fr\": \"Raisonnement concluant\",\r\n\t\t\"title_fr\": \"Syllogisme Barbara\",\r\n\t\t\"description_fr\": \"Syllogisme de mode parfait où le terme moyen est un sujet dans la première prémisse et un prédicat dans la seconde prémisse\",\r\n\t\t\"remark_fr\": \"Syllogisme du type Tout M est P, or tout S est M, donc tout S est P. Exemple: Tous les félidés sont sympathiques, Tous les chats sont des félidés, Donc tous les chats sont sympathiques.\",\r\n\t\t\"link_fr\": \"https://fr.wikipedia.org/wiki/Barbara_(syllogisme)\"\r\n\t},\r\n\t{\r\n\t\t\"path\": \"1\",\r\n\t\t\"family_fr\": \"Argument pertinent\",\r\n\t\t\"subfamily_fr\": \"Argument fondé\",\r\n\t\t\"subsubfamily_fr\": \"\",\r\n\t\t\"title_fr\": \"Argument fondé\",\r\n\t\t\"description_fr\": \"Argument correctement formulé et recevable\",\r\n\t\t\"link_fr\": \"https://fr.wikipedia.org/wiki/Th%C3%A9orie_de_l%27argumentation\",\r\n\t\t\"remark_fr\": \"Il existe des ingrédients essentiels qui donnent à un argument sa nature d'argument, comme un ensemble d'hypothèses ou prémisses acceptables, une méthode de raisonnement et une conclusion.\"\r\n\t},\r\n\t{\r\n\t\t\"path\": \"4.1.2\",\r\n\t\t\"family_fr\": \"Raisonnement valide\",\r\n\t\t\"subfamily_fr\": \"Identification précise des causalités\",\r\n\t\t\"subsubfamily_fr\": \"Causalité correctement orientée\",\r\n\t\t\"title_fr\": \"Causalité correctement orientée\",\r\n\t\t\"description_fr\": \"Bien orienter la causalité évite les erreurs d'attribution ou le fait de confondre une cause avec un effet.\",\r\n\t\t\"remark_fr\": \"Du fait de leur corrélation, il n'est pas rare de commettre l'erreur d'inverser les liens de causalité, comme dans l'exemple suivant: L'augmentation du SIDA a été causée par plus d'éducation sexuelle. En fait, l'augmentation de l'éducation sexuelle a été causée par la propagation du SIDA.\",\r\n\t\t\"link_fr\": \"http://sophismes.free.fr/log42.htm\"\r\n\t}\r\n]",
+    "OpenAIKeyPath": "G:\\Mon Drive\\MyIA\\Argumentum\\Fallacies\\Gestion\\OpenAI-Key.txt",
+    "Model": "gpt-3.5-turbo",
+    "ChunkSize": 3,
+    "NbMessageCalls": 3,
+    "SkipChunkNb": 0,
+    "TakeChunkNb": 0,
+    "MaxDegreeOfParallelismWebService": 8,
+    "FieldsToInclude": [
+      "path",
+      "family_fr",
+      "subfamily_fr",
+      "subsubfamily_fr",
+      "title_fr",
+      "description_fr",
+      "remark_fr",
+      "link_fr"
+    ],
+    "PrimaryField": "path",
+    "FieldsToUpdate": [
+      "description_fr",
+      "remark_fr",
+      "link_fr"
+    ],
+    "SourceDataset": {
+      "Name": "Argumentum - Virtues - Taxonomy",
+      "ReleaseFilePath": "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum%20Virtues%20-%20Taxonomy.csv",
+      "DebugFilePath": "..\\..\\..\\..\\..\\..\\Cards\\Fallacies\\Argumentum Virtues - Taxonomy.csv",
+      "CsvType": null
+    },
+    "TargetPath": ".\\Target\\Datasets\\Argumentum Virtues - Taxonomy.csv"
+  },
+  "Dnn2sxcConfig": {
+    "CsvPathFallacies": "..\\..\\..\\Data\\Dnn2sxc\\Argumentum Fallacies - Taxonomy.csv",
+    "Xml2sxcImportPathFallacies": "..\\..\\..\\Data\\Dnn2sxc\\2sxc Fallacy Data 20210212131131.xml",
+    "Xml2sxcExportPathFallacies": "..\\..\\..\\Data\\Dnn2sxc\\2sxcContentExport_Argumentum_Fallacies_0.0.1.xml",
+    "TitleExpressionFr": "{fallacy.TextFr}",
+    "DescriptionExpressionFr": "{HttpUtility.HtmlEncode(fallacy.DescFr)}",
+    "ExampleExpressionFr": "{HttpUtility.HtmlEncode(fallacy.ExampleFr)}",
+    "LinkExpressionFr": "{fallacy.LinkFr}",
+    "TitleExpressionEn": "{fallacy.TextEn}",
+    "DescriptionExpressionEn": "{HttpUtility.HtmlEncode(fallacy.DescEn)}",
+    "ExampleExpressionEn": "{HttpUtility.HtmlEncode(fallacy.ExampleEn)}",
+    "LinkExpressionEn": "{fallacy.LinkEn}"
+  },
+  "MindMapCreatorConfig": {
+    "MindMaps": [
+      {
+        "DataSet": "..\\..\\..\\Data\\Mindmap\\Argumentum Fallacies - Taxonomy.csv",
+        "TitleExpression": "{fallacy.TextFr}",
+        "FamilleExpression": "{fallacy.Famille}",
+        "SousFamilleExpression": "{fallacy.SousFamille}",
+        "SoussousFamilleExpression": "{fallacy.Soussousfamille}",
+        "DescriptionExpression": "\r\n<p>\r\n    {HttpUtility.HtmlEncode(fallacy.DescFr)}\r\n</p>\r\n",
+        "CardExpression": "\r\n<p>\r\n    <img src=\"{mindMap.GetThumbnailsPath(fallacy)}\" width=\"60\" height=\"60\"/>{fallacy.TextFr}\r\n</p>\r\n",
+        "ExampleExpression": "\r\n<p>\r\n    <i>{HttpUtility.HtmlEncode(fallacy.ExampleFr)}</i>\r\n</p>\r\n",
+        "LinkExpression": "{fallacy.LinkFrFallback}",
+        "ThumbnailsPathExpression": "../../bin/Debug/netcoreapp3.1/Target/Images/density-0/Fallacies-Web-Thumbnails/argumentum_{fallacy.Path}_{fallacy.TextFr.ToLower().Replace(\" \",\"_\")}.png",
+        "NbBranchesRight": 2,
+        "Colors": {
+          "1": "#8605ab",
+          "2": "#ff66eb",
+          "3": "#08af93",
+          "4": "#8dc801",
+          "5": "#0054a4",
+          "6": "#ffc307",
+          "7": "#dc0f0a"
+        },
+        "FontSizes": [
+          30,
+          50,
+          40,
+          30,
+          30,
+          30,
+          25,
+          23,
+          23,
+          23,
+          23
+        ],
+        "EdgeSizes": [
+          8,
+          4,
+          2,
+          1
+        ],
+        "InsertCardsThumbnails": false,
+        "ThumbnailsCardSetName": null,
+        "ThumbnailsFileNamePattern": "__{fallacy.Path}..",
+        "SVGMaps": [
+
+        ],
+        "KeepOriginalSVG": false,
+        "Enabled": true,
+        "DocumentName": "..\\..\\..\\Data\\Mindmap\\Argumentum_Fallacies_MindMap_Fr_2.mm",
+        "Translations": [
+
+        ],
+        "TargetDensity": 0,
+        "ImageFormat": "Png",
+        "TemplatePathRelease": null,
+        "TemplatePathDebug": null
+      },
+      {
+        "DataSet": "..\\..\\..\\Data\\Mindmap\\Argumentum Fallacies - Taxonomy.csv",
+        "TitleExpression": "{fallacy.TextFr}",
+        "FamilleExpression": "{fallacy.Famille}",
+        "SousFamilleExpression": "{fallacy.SousFamille}",
+        "SoussousFamilleExpression": "{fallacy.Soussousfamille}",
+        "DescriptionExpression": "\r\n<p>\r\n    {HttpUtility.HtmlEncode(fallacy.DescFr)}\r\n</p>\r\n",
+        "CardExpression": "\r\n<p>\r\n    <img src=\"{mindMap.GetThumbnailsPath(fallacy)}\" width=\"60\" height=\"60\"/>{fallacy.TextFr}\r\n</p>\r\n",
+        "ExampleExpression": "\r\n<p>\r\n    <i>{HttpUtility.HtmlEncode(fallacy.ExampleFr)}</i>\r\n</p>\r\n",
+        "LinkExpression": "{fallacy.LinkFrFallback}",
+        "ThumbnailsPathExpression": "../../bin/Debug/netcoreapp3.1/Target/Images/density-0/Fallacies-Web-Thumbnails/argumentum_{fallacy.Path}_{fallacy.TextFr.ToLower().Replace(\" \",\"_\")}.png",
+        "NbBranchesRight": 2,
+        "Colors": {
+          "1": "#8605ab",
+          "2": "#ff66eb",
+          "3": "#08af93",
+          "4": "#8dc801",
+          "5": "#0054a4",
+          "6": "#ffc307",
+          "7": "#dc0f0a"
+        },
+        "FontSizes": [
+          30,
+          50,
+          40,
+          30,
+          30,
+          30,
+          25,
+          23,
+          23,
+          23,
+          23
+        ],
+        "EdgeSizes": [
+          8,
+          4,
+          2,
+          1
+        ],
+        "InsertCardsThumbnails": true,
+        "ThumbnailsCardSetName": null,
+        "ThumbnailsFileNamePattern": "__{fallacy.Path}..",
+        "SVGMaps": [
+
+        ],
+        "KeepOriginalSVG": false,
+        "Enabled": true,
+        "DocumentName": "..\\..\\..\\Data\\Mindmap\\Argumentum_Fallacies_MindMap_Fr_2_cards.mm",
+        "Translations": [
+
+        ],
+        "TargetDensity": 0,
+        "ImageFormat": "Png",
+        "TemplatePathRelease": null,
+        "TemplatePathDebug": null
+      },
+      {
+        "DataSet": "..\\..\\..\\Data\\Mindmap\\Argumentum Fallacies - Taxonomy.csv",
+        "TitleExpression": "{fallacy.TextEn}",
+        "FamilleExpression": "{fallacy.Famille}",
+        "SousFamilleExpression": "{fallacy.SousFamille}",
+        "SoussousFamilleExpression": "{fallacy.Soussousfamille}",
+        "DescriptionExpression": "\r\n<p>\r\n    {HttpUtility.HtmlEncode(fallacy.DescEn)}\r\n</p>\r\n",
+        "CardExpression": "\r\n<p>\r\n    <img src=\"{mindMap.GetThumbnailsPath(fallacy)}\" width=\"60\" height=\"60\"/>{fallacy.TextFr}\r\n</p>\r\n",
+        "ExampleExpression": "\r\n<p>\r\n    <i>{HttpUtility.HtmlEncode(fallacy.ExampleEn)}</i>\r\n</p>\r\n",
+        "LinkExpression": "{fallacy.LinkEnFallback}",
+        "ThumbnailsPathExpression": "../../bin/Debug/netcoreapp3.1/Target/Images/density-0/Fallacies-Web-Thumbnails/argumentum_{fallacy.Path}_{fallacy.TextFr.ToLower().Replace(\" \",\"_\")}.png",
+        "NbBranchesRight": 2,
+        "Colors": {
+          "1": "#8605ab",
+          "2": "#ff66eb",
+          "3": "#08af93",
+          "4": "#8dc801",
+          "5": "#0054a4",
+          "6": "#ffc307",
+          "7": "#dc0f0a"
+        },
+        "FontSizes": [
+          30,
+          50,
+          40,
+          30,
+          30,
+          30,
+          25,
+          23,
+          23,
+          23,
+          23
+        ],
+        "EdgeSizes": [
+          8,
+          4,
+          2,
+          1
+        ],
+        "InsertCardsThumbnails": false,
+        "ThumbnailsCardSetName": null,
+        "ThumbnailsFileNamePattern": "__{fallacy.Path}..",
+        "SVGMaps": [
+
+        ],
+        "KeepOriginalSVG": false,
+        "Enabled": true,
+        "DocumentName": "..\\..\\..\\Data\\Mindmap\\Argumentum_Fallacies_MindMap_En_2.mm",
+        "Translations": [
+
+        ],
+        "TargetDensity": 0,
+        "ImageFormat": "Png",
+        "TemplatePathRelease": null,
+        "TemplatePathDebug": null
+      },
+      {
+        "DataSet": "..\\..\\..\\Data\\Mindmap\\Argumentum Fallacies - Taxonomy.csv",
+        "TitleExpression": "{fallacy.TextEn}",
+        "FamilleExpression": "{fallacy.Famille}",
+        "SousFamilleExpression": "{fallacy.SousFamille}",
+        "SoussousFamilleExpression": "{fallacy.Soussousfamille}",
+        "DescriptionExpression": "\r\n<p>\r\n    {HttpUtility.HtmlEncode(fallacy.DescEn)}\r\n</p>\r\n",
+        "CardExpression": "\r\n<p>\r\n    <img src=\"{mindMap.GetThumbnailsPath(fallacy)}\" width=\"60\" height=\"60\"/>{fallacy.TextEn}\r\n</p>\r\n",
+        "ExampleExpression": "\r\n<p>\r\n    <i>{HttpUtility.HtmlEncode(fallacy.ExampleEn)}</i>\r\n</p>\r\n",
+        "LinkExpression": "{fallacy.LinkEnFallback}",
+        "ThumbnailsPathExpression": "../../bin/Debug/netcoreapp3.1/Target/Images/density-0/Fallacies-Web-Thumbnails/argumentum_{fallacy.Path}_{fallacy.TextFr.ToLower().Replace(\" \",\"_\")}.png",
+        "NbBranchesRight": 2,
+        "Colors": {
+          "1": "#8605ab",
+          "2": "#ff66eb",
+          "3": "#08af93",
+          "4": "#8dc801",
+          "5": "#0054a4",
+          "6": "#ffc307",
+          "7": "#dc0f0a"
+        },
+        "FontSizes": [
+          30,
+          50,
+          40,
+          30,
+          30,
+          30,
+          25,
+          23,
+          23,
+          23,
+          23
+        ],
+        "EdgeSizes": [
+          8,
+          4,
+          2,
+          1
+        ],
+        "InsertCardsThumbnails": true,
+        "ThumbnailsCardSetName": null,
+        "ThumbnailsFileNamePattern": "__{fallacy.Path}..",
+        "SVGMaps": [
+
+        ],
+        "KeepOriginalSVG": false,
+        "Enabled": true,
+        "DocumentName": "..\\..\\..\\Data\\Mindmap\\Argumentum_Fallacies_MindMap_En_2_cards.mm",
+        "Translations": [
+
+        ],
+        "TargetDensity": 0,
+        "ImageFormat": "Png",
+        "TemplatePathRelease": null,
+        "TemplatePathDebug": null
+      }
+    ]
+  },
+  "ContinuousValidationConfig": {
+    "GenerateFullReport": true,
+    "ReportPath": "ValidationReports",
+    "ValidateTaxonomy": true,
+    "ValidateOwl": true,
+    "ValidateCards": true,
+    "Languages": [
+      "fr",
+      "en",
+      "ru",
+      "pt"
+    ]
+  }
+}
diff --git a/Generation/Converters/Argumentum.AssetConverter/Entities/Fallacy.cs b/Generation/Converters/Argumentum.AssetConverter/Entities/Fallacy.cs
index fcdffe5..eed6579 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Entities/Fallacy.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/Entities/Fallacy.cs
@@ -7,9 +7,13 @@
     public class Fallacy : CsvBase<Fallacy, FallacyClassMap>
     {
 
-        public string GetId()
+        /// <summary>
+        /// Identifiant unique de la fallacy
+        /// </summary>
+        public string Id
         {
-            return Path;
+            get { return Path; }
+            set { /* Permet la désérialisation */ }
         }
 
         public string LinkFrFallback => string.IsNullOrEmpty(LinkFr) ? LinkEn : LinkFr;
@@ -127,6 +131,7 @@
         public FallacyClassMap()
         {
             Map(m => m.PK).Name("PK");
+            Map(m => m.Id).Name("Id");
             Map(m => m.Path).Name("path");
             Map(m => m.DecimalPath).Name("decimal_path");
             Map(m => m.Depth).Name("depth");
@@ -155,7 +160,7 @@
             Map(m => m.Len115).Name("Len115");
             Map(m => m.ExampleEn).Name("example_en");
             Map(m => m.Lxen145).Name("Lxen145");
-            Map(m => m.ExampleEn).Name("example_en");
+   Map(m => m.ExampleEnBis).Name("example_en_bis");
             Map(m => m.LinkEn).Name("link_en");
 
             Map(m => m.FamilyRu).Name("Family_ru");
diff --git a/Generation/Converters/Argumentum.AssetConverter/Mindmapper/MindMapDocumentConfig.cs b/Generation/Converters/Argumentum.AssetConverter/Mindmapper/MindMapDocumentConfig.cs
index c4a8b05..cfeae0d 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Mindmapper/MindMapDocumentConfig.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/Mindmapper/MindMapDocumentConfig.cs
@@ -348,7 +348,7 @@
         private Node CreateNode(Fallacy fallacy, AssetConverterConfig config, string language, params (CrossLink crossLinkType, List<Fallacy> targets)[] crossLinks)
         {
             var fallacyNode = new Node { TEXT = TitleFunc(fallacy) };
-            fallacyNode.ID = fallacy.GetId();
+            fallacyNode.ID = fallacy.Id;
             var link = LinkFunc(fallacy);
             if (!string.IsNullOrEmpty(link))
             {
@@ -372,7 +372,7 @@
                 crossLinkNode.EndArrow = "Default";
                 crossLinkNode.StartInclination = "892;0;";
                 crossLinkNode.EndInclination = "892;0;";
-                crossLinkNode.Destination = target.GetId();
+                crossLinkNode.Destination = target.Id;
 
                 switch (crossLink.crossLinkType)
                 {
diff --git a/Program.cs b/Program.cs
new file mode 100644
index 0000000..60370f4
--- /dev/null
+++ b/Program.cs
@@ -0,0 +1,51 @@
+using System;
+using Spectre.Console;
+
+namespace LoggerFix
+{
+    public enum MessageType
+    {
+        Info,
+        Title,
+        Problem,
+        Instructions,
+        Warning,
+        Success,
+        Explanations
+    }
+
+    public class Logger
+    {
+        public static void LogException(Exception ex)
+        {
+            Console.WriteLine("Méthode LogException modifiée qui n'utilise pas System.Diagnostics.StackTrace");
+
+            // Solution qui n'utilise pas System.Diagnostics.StackTrace
+            AnsiConsole.MarkupLine($"[red]{ex.GetType().Name}[/]: [bold red]{ex.Message}[/]");
+            if (ex.StackTrace != null)
+            {
+                AnsiConsole.MarkupLine($"[dim]{ex.StackTrace}[/]");
+            }
+
+            // Ne pas utiliser cette méthode car elle dépend de System.Diagnostics.StackTrace
+            // AnsiConsole.WriteException(ex);
+        }
+    }
+
+    class Program
+    {
+        static void Main(string[] args)
+        {
+            Console.WriteLine("Test de la méthode LogException modifiée");
+
+            try
+            {
+                throw new Exception("Test d'exception");
+            }
+            catch (Exception ex)
+            {
+                Logger.LogException(ex);
+            }
+        }
+    }
+}
```


### Commit `6a871ef7ecae7a9335688ba60d04ed6757b41c169`

**Message :** Ajout et mise à jour des fichiers de test et de validation

**Statistiques :**
```
.../Argumentum.AssetConverter/Program.cs | 158 ++---------
.../Tests/CardGenerationValidationTests.cs | 33 +++-
.../Tests/ContinuousValidationConfig.cs | 12 +-
.../Tests/OwlOntologyValidationTests.cs | 78 ++++++---
.../Tests/TaxonomyValidationTests.cs | 47 +++--
.../Tests/ValidationSeverity.cs | 30 +++
6 files changed, 232 insertions(+), 126 deletions(-)
```

**Diff complet :**
```diff
commit 6a871ef7ecae7a9335688ba60d04ed6757b41c169
Author: jsboige <jsboige@gmail.com>
Date:   Tue May 13 18:08:26 2025 +0200

    Ajout et mise à jour des fichiers de test et de validation

    Former-commit-id: 400a53e815cb834da9953644d06bb45c36cb5d63
    Former-commit-id: d4c244eb65400d6826727b41af7163be4e3a0364

diff --git a/Generation/Converters/Argumentum.AssetConverter/Program.cs b/Generation/Converters/Argumentum.AssetConverter/Program.cs
index f3efbe2..3babb78 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Program.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/Program.cs
@@ -40,11 +40,11 @@
                     Logger.LogTitle("Mode de validation de taxonomie");

-                    var configFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
-                    var config = AssetConverterConfig.GetConfig(configFileName, out var _);
+                    var taxonomyConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
+                    var taxonomyConfig = AssetConverterConfig.GetConfig(taxonomyConfigFileName, out var _);

                     // Activer uniquement le mode de validation de taxonomie
-                    config.Mode = ConverterMode.TaxonomyValidator;
+                    taxonomyConfig.Mode = ConverterMode.TaxonomyValidator;

                     // Configurer les options de validation en fonction des arguments
                     if (args.Length > 1)
@@ -53,37 +53,37 @@
                         {
                             if (arg.Equals("--structure", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.TaxonomyValidatorConfig.ValidateStructure = true;
-                                config.TaxonomyValidatorConfig.ValidateTranslations = false;
-                                config.TaxonomyValidatorConfig.ValidateTerminology = false;
+                                taxonomyConfig.TaxonomyValidatorConfig.ValidateStructure = true;
+                                taxonomyConfig.TaxonomyValidatorConfig.ValidateTranslations = false;
+                                taxonomyConfig.TaxonomyValidatorConfig.ValidateTerminology = false;
                             }
                             else if (arg.Equals("--translations", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.TaxonomyValidatorConfig.ValidateStructure = false;
-                                config.TaxonomyValidatorConfig.ValidateTranslations = true;
-                                config.TaxonomyValidatorConfig.ValidateTerminology = false;
+                                taxonomyConfig.TaxonomyValidatorConfig.ValidateStructure = false;
+                                taxonomyConfig.TaxonomyValidatorConfig.ValidateTranslations = true;
+                                taxonomyConfig.TaxonomyValidatorConfig.ValidateTerminology = false;
                             }
                             else if (arg.Equals("--terminology", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.TaxonomyValidatorConfig.ValidateStructure = false;
-                                config.TaxonomyValidatorConfig.ValidateTranslations = false;
-                                config.TaxonomyValidatorConfig.ValidateTerminology = true;
+                                taxonomyConfig.TaxonomyValidatorConfig.ValidateStructure = false;
+                                taxonomyConfig.TaxonomyValidatorConfig.ValidateTranslations = false;
+                                taxonomyConfig.TaxonomyValidatorConfig.ValidateTerminology = true;
                             }
-                        }
+                        }

-                    await config.Apply().ConfigureAwait(false);
+                    await taxonomyConfig.Apply().ConfigureAwait(false);
                     return;
                 }
                 else if (args[0].Equals("--validate-owl", StringComparison.OrdinalIgnoreCase))
                 {
                     Logger.LogTitle("Mode de validation d'ontologie OWL");

-                    var configFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
-                    var config = AssetConverterConfig.GetConfig(configFileName, out var _);
+                    var owlConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
+                    var owlConfig = AssetConverterConfig.GetConfig(owlConfigFileName, out var _);

                     // Activer uniquement le mode de validation d'ontologie OWL
-                    config.Mode = ConverterMode.OwlValidator;
+                    owlConfig.Mode = ConverterMode.OwlValidator;

                     // Configurer les options de validation en fonction des arguments
                     if (args.Length > 1)
@@ -92,37 +92,37 @@
                         {
                             if (arg.Equals("--structure", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.OwlValidatorConfig.ValidateStructure = true;
-                                config.OwlValidatorConfig.ValidateMultilingualAnnotations = false;
-                                config.OwlValidatorConfig.ValidateAIFMappings = false;
+                                owlConfig.OwlValidatorConfig.ValidateStructure = true;
+                                owlConfig.OwlValidatorConfig.ValidateMultilingualAnnotations = false;
+                                owlConfig.OwlValidatorConfig.ValidateAIFMappings = false;
                             }
                             else if (arg.Equals("--annotations", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.OwlValidatorConfig.ValidateStructure = false;
-                                config.OwlValidatorConfig.ValidateMultilingualAnnotations = true;
-                                config.OwlValidatorConfig.ValidateAIFMappings = false;
+                                owlConfig.OwlValidatorConfig.ValidateStructure = false;
+                                owlConfig.OwlValidatorConfig.ValidateMultilingualAnnotations = true;
+                                owlConfig.OwlValidatorConfig.ValidateAIFMappings = false;
                             }
                             else if (arg.Equals("--mappings", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.OwlValidatorConfig.ValidateStructure = false;
-                                config.OwlValidatorConfig.ValidateMultilingualAnnotations = false;
-                                config.OwlValidatorConfig.ValidateAIFMappings = true;
+                                owlConfig.OwlValidatorConfig.ValidateStructure = false;
+                                owlConfig.OwlValidatorConfig.ValidateMultilingualAnnotations = false;
+                                owlConfig.OwlValidatorConfig.ValidateAIFMappings = true;
                             }
-                        }
+                        }

-                    await config.Apply().ConfigureAwait(false);
+                    await owlConfig.Apply().ConfigureAwait(false);
                     return;
                 }
                 else if (args[0].Equals("--validate-cards", StringComparison.OrdinalIgnoreCase))
                 {
                     Logger.LogTitle("Mode de validation des cartes générées");

-                    var configFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
-                    var config = AssetConverterConfig.GetConfig(configFileName, out var _);
+                    var cardsConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
+                    var cardsConfig = AssetConverterConfig.GetConfig(cardsConfigFileName, out var _);

                     // Activer uniquement le mode de validation des cartes
-                    config.Mode = ConverterMode.CardValidator;
+                    cardsConfig.Mode = ConverterMode.CardValidator;

                     // Configurer les options de validation en fonction des arguments
                     if (args.Length > 1)
@@ -131,37 +131,37 @@
                         {
                             if (arg.Equals("--existence", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.CardValidatorConfig.ValidateFileExistence = true;
-                                config.CardValidatorConfig.ValidateImageQuality = false;
-                                config.CardValidatorConfig.ValidateMultilingualConsistency = false;
+                                cardsConfig.CardValidatorConfig.ValidateFileExistence = true;
+                                cardsConfig.CardValidatorConfig.ValidateImageQuality = false;
+                                cardsConfig.CardValidatorConfig.ValidateMultilingualConsistency = false;
                             }
                             else if (arg.Equals("--quality", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.CardValidatorConfig.ValidateFileExistence = false;
-                                config.CardValidatorConfig.ValidateImageQuality = true;
-                                config.CardValidatorConfig.ValidateMultilingualConsistency = false;
+                                cardsConfig.CardValidatorConfig.ValidateFileExistence = false;
+                                cardsConfig.CardValidatorConfig.ValidateImageQuality = true;
+                                cardsConfig.CardValidatorConfig.ValidateMultilingualConsistency = false;
                             }
                             else if (arg.Equals("--consistency", StringComparison.OrdinalIgnoreCase))
-                    var continuousConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
-                    var continuousConfig = AssetConverterConfig.GetConfig(continuousConfigFileName, out var _);
+                    var continuousConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
+                    var continuousConfig = AssetConverterConfig.GetConfig(continuousConfigFileName, out var _);

                     // Activer uniquement le mode de validation continue
-                    config.Mode = ConverterMode.ContinuousValidator;
+                    continuousConfig.Mode = ConverterMode.ContinuousValidator;

                     // Configurer les options de validation en fonction des arguments
                     if (args.Length > 1)
@@ -173,50 +173,50 @@
                                 if (int.TryParse(args[intervalIndex], out int interval))
                                 {
-                                    config.ContinuousValidationConfig.ValidationInterval = interval;
+                                    continuousConfig.ContinuousValidationConfig.ValidationInterval = interval;
                                 }
                             }
                             else if (arg.Equals("--watch", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.ContinuousValidationConfig.ValidateOnChanges = true;
+                                continuousConfig.ContinuousValidationConfig.ValidateOnChanges = true;
                             }
                             else if (arg.Equals("--no-watch", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.ContinuousValidationConfig.ValidateOnChanges = false;
+                                continuousConfig.ContinuousValidationConfig.ValidateOnChanges = false;
                             }
                             else if (arg.Equals("--taxonomy", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.ContinuousValidationConfig.ValidateTaxonomy = true;
-                                config.ContinuousValidationConfig.ValidateOwl = false;
-                                config.ContinuousValidationConfig.ValidateCards = false;
+                                continuousConfig.ContinuousValidationConfig.ValidateTaxonomy = true;
+                                continuousConfig.ContinuousValidationConfig.ValidateOwl = false;
+                                continuousConfig.ContinuousValidationConfig.ValidateCards = false;
                             }
                             else if (arg.Equals("--owl", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.ContinuousValidationConfig.ValidateTaxonomy = false;
-                                config.ContinuousValidationConfig.ValidateOwl = true;
-                                config.ContinuousValidationConfig.ValidateCards = false;
+                                continuousConfig.ContinuousValidationConfig.ValidateTaxonomy = false;
+                                continuousConfig.ContinuousValidationConfig.ValidateOwl = true;
+                                continuousConfig.ContinuousValidationConfig.ValidateCards = false;
                             }
                             else if (arg.Equals("--cards", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.ContinuousValidationConfig.ValidateTaxonomy = false;
-                                config.ContinuousValidationConfig.ValidateOwl = false;
-                                config.ContinuousValidationConfig.ValidateCards = true;
+                                continuousConfig.ContinuousValidationConfig.ValidateTaxonomy = false;
+                                continuousConfig.ContinuousValidationConfig.ValidateOwl = false;
+                                continuousConfig.ContinuousValidationConfig.ValidateCards = true;
                             }
-                        }
+                        }

-                    await config.Apply().ConfigureAwait(false);
+                    await continuousConfig.Apply().ConfigureAwait(false);
                     return;
                 }
                 else if (args[0].Equals("--translation-coverage", StringComparison.OrdinalIgnoreCase))
                 {
                     Logger.LogTitle("Mode de rapport de couverture des traductions");

-                    var configFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
-                    var config = AssetConverterConfig.GetConfig(configFileName, out var _);
+                    var translationConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
+                    var translationConfig = AssetConverterConfig.GetConfig(translationConfigFileName, out var _);

                     // Activer uniquement le mode de rapport de couverture des traductions
-                    config.Mode = ConverterMode.TranslationCoverage;
+                    translationConfig.Mode = ConverterMode.TranslationCoverage;

                     // Configurer les options de rapport en fonction des arguments
                     if (args.Length > 1)
@@ -227,41 +227,41 @@
                             {
                                 int languagesIndex = args.ToList().IndexOf(arg) + 1;
                                 string[] languages = args[languagesIndex].Split(',');
-                                config.TranslationCoverageConfig.Languages = languages.ToList();
+                                translationConfig.TranslationCoverageConfig.Languages = languages.ToList();
                             }
                             else if (arg.Equals("--fields", StringComparison.OrdinalIgnoreCase) && args.Length > args.ToList().IndexOf(arg) + 1)
                             {
                                 int fieldsIndex = args.ToList().IndexOf(arg) + 1;
                                 string[] fields = args[fieldsIndex].Split(',');
-                                config.TranslationCoverageConfig.FieldTypes = fields.ToList();
+                                translationConfig.TranslationCoverageConfig.FieldTypes = fields.ToList();
                             }
                             else if (arg.Equals("--threshold", StringComparison.OrdinalIgnoreCase) && args.Length > args.ToList().IndexOf(arg) + 1)
                             {
                                 int thresholdIndex = args.ToList().IndexOf(arg) + 1;
                                 if (int.TryParse(args[thresholdIndex], out int threshold))
                                 {
-                                    config.TranslationCoverageConfig.MinimumCoverageThreshold = threshold;
+                                    translationConfig.TranslationCoverageConfig.MinimumCoverageThreshold = threshold;
                                 }
                             }
                             else if (arg.Equals("--no-charts", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.TranslationCoverageConfig.GenerateProgressCharts = false;
+                                translationConfig.TranslationCoverageConfig.GenerateProgressCharts = false;
                             }
-                        }
+                        }

-                    await config.Apply().ConfigureAwait(false);
+                    await translationConfig.Apply().ConfigureAwait(false);
                     return;
                 }
                 else if (args[0].Equals("--optimize-parallelism", StringComparison.OrdinalIgnoreCase))
                 {
                     Logger.LogTitle("Mode d'optimisation du parallélisme");

-                    var configFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
-                    var config = AssetConverterConfig.GetConfig(configFileName, out var _);
+                    var parallelismConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
+                    var parallelismConfig = AssetConverterConfig.GetConfig(parallelismConfigFileName, out var _);

                     // Activer uniquement le mode d'optimisation du parallélisme
-                    config.Mode = ConverterMode.ParallelismOptimizer;
+                    parallelismConfig.Mode = ConverterMode.ParallelismOptimizer;

                     // Configurer les options d'optimisation en fonction des arguments
                     if (args.Length > 1)
@@ -270,26 +270,26 @@
                         {
                             if (arg.Equals("--run-before-generation", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.ParallelismOptimizerConfig.RunBeforeGeneration = true;
+                                parallelismConfig.ParallelismOptimizerConfig.RunBeforeGeneration = true;
                             }
                             else if (arg.Equals("--no-run-before-generation", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.ParallelismOptimizerConfig.RunBeforeGeneration = false;
+                                parallelismConfig.ParallelismOptimizerConfig.RunBeforeGeneration = false;
                             }
                             else if (arg.Equals("--dynamic-adjustment", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.ParallelismOptimizerConfig.DynamicAdjustment = true;
+                                parallelismConfig.ParallelismOptimizerConfig.DynamicAdjustment = true;
                             }
                             else if (arg.Equals("--no-dynamic-adjustment", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.ParallelismOptimizerConfig.DynamicAdjustment = false;
+                                parallelismConfig.ParallelismOptimizerConfig.DynamicAdjustment = false;
                             }
                             else if (arg.Equals("--monitoring-interval", StringComparison.OrdinalIgnoreCase) && args.Length > args.ToList().IndexOf(arg) + 1)
                             {
                                 int intervalIndex = args.ToList().IndexOf(arg) + 1;
                                 if (int.TryParse(args[intervalIndex], out int interval))
                                 {
-                                    config.ParallelismOptimizerConfig.MonitoringIntervalSeconds = interval;
+                                    parallelismConfig.ParallelismOptimizerConfig.MonitoringIntervalSeconds = interval;
                                 }
                             }
                             else if (arg.Equals("--target-cpu-usage", StringComparison.OrdinalIgnoreCase) && args.Length > args.ToList().IndexOf(arg) + 1)
@@ -297,7 +297,7 @@
                                 int usageIndex = args.ToList().IndexOf(arg) + 1;
                                 if (int.TryParse(args[usageIndex], out int usage))
                                 {
-                                    config.ParallelismOptimizerConfig.TargetCpuUsagePercent = usage;
+                                    parallelismConfig.ParallelismOptimizerConfig.TargetCpuUsagePercent = usage;
                                 }
                             }
                             else if (arg.Equals("--target-memory-usage", StringComparison.OrdinalIgnoreCase) && args.Length > args.ToList().IndexOf(arg) + 1)
@@ -305,29 +305,29 @@
                                 int usageIndex = args.ToList().IndexOf(arg) + 1;
                                 if (int.TryParse(args[usageIndex], out int usage))
                                 {
-                                    config.ParallelismOptimizerConfig.TargetMemoryUsagePercent = usage;
+                                    parallelismConfig.ParallelismOptimizerConfig.TargetMemoryUsagePercent = usage;
                                 }
                             }
                             else if (arg.Equals("--detailed-report", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.ParallelismOptimizerConfig.GenerateDetailedReport = true;
+                                parallelismConfig.ParallelismOptimizerConfig.GenerateDetailedReport = true;
                             }
                             else if (arg.Equals("--no-detailed-report", StringComparison.OrdinalIgnoreCase))
                             {
-                                config.ParallelismOptimizerConfig.GenerateDetailedReport = false;
+                                parallelismConfig.ParallelismOptimizerConfig.GenerateDetailedReport = false;
                             }
-                        }
+                        }

-                    await config.Apply().ConfigureAwait(false);
+                    await parallelismConfig.Apply().ConfigureAwait(false);
                     return;
                 }
                 else if (args[0].Equals("--generate-documentation", StringComparison.OrdinalIgnoreCase))
                 {
                     Logger.LogTitle("Mode de génération de documentation");

-                    var configFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
-                    var config = AssetConverterConfig.GetConfig(configFileName, out var _);
+                    var docConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
+                    var docConfig = AssetConverterConfig.GetConfig(docConfigFileName, out var _);

                     string inputDir = Path.Combine(Environment.CurrentDirectory, "Documentation");
                     string outputDir = Path.Combine(Environment.CurrentDirectory, "Output", "Documentation");
@@ -348,7 +348,7 @@
                         }
                     }

-                    Logger.LogInfo($"Génération de la documentation à partir de {inputDir} vers {outputDir}");
+                    Logger.LogInfoMessage($"Génération de la documentation à partir de {inputDir} vers {outputDir}");

                     var docGenerator = new Documentation.DocumentationGenerator(inputDir, outputDir);
                     await docGenerator.GenerateAsync();
diff --git a/Generation/Converters/Argumentum.AssetConverter/Tests/CardGenerationValidationTests.cs b/Generation/Converters/Argumentum.AssetConverter/Tests/CardGenerationValidationTests.cs
index 9117d72..4b084e1 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Tests/CardGenerationValidationTests.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/Tests/CardGenerationValidationTests.cs
@@ -20,8 +20,9 @@
     {
         private readonly AssetConverterConfig _config;
         private readonly CardValidatorConfig _validatorConfig;
         private StringBuilder _reportBuilder;
-        private int _totalErrors;
-        private int _totalWarnings;
+        // Changement de visibilité pour permettre l'accès depuis ContinuousValidationSystem
+        public int _totalErrors;
+        public int _totalWarnings;
         private Dictionary<string, List<string>> _cardFilesByLanguage;
 
         /// <summary>
@@ -165,11 +166,11 @@
             StringBuilder statusDetails = new StringBuilder();

             // Vérifier les dimensions
-            if (image.Width != expectedDimensions.Width || image.Height != expectedDimensions.Height)
+            if (image.Width != expectedDimensions.Item1 || image.Height != expectedDimensions.Item2)
             {
                 hasError = true;
                 statusClass = "error";
-                statusDetails.Append($"Dimensions incorrectes (attendu: {expectedDimensions.Width}x{expectedDimensions.Height}) ");
+                statusDetails.Append($"Dimensions incorrectes (attendu: {expectedDimensions.Item1}x{expectedDimensions.Item2}) ");
             }

             // Vérifier la résolution DPI
@@ -368,7 +369,7 @@
         /// Exécute tous les tests de validation et génère un rapport global.
         /// </summary>
         /// <returns>Une tâche représentant l'opération asynchrone.</returns>
-        public async Task RunAllCardValidations()
+        public async Task<bool> RunAllCardValidations()
         {
             Logger.LogTitle("Exécution de tous les tests de validation des cartes");
 
@@ -387,17 +388,35 @@
             if (_totalErrors > 0)
             {
                 Logger.LogProblem($"Validation des cartes terminée avec {_totalErrors} erreurs et {_totalWarnings} avertissements");
+                return false;
             }
             else if (_totalWarnings > 0)
             {
                 Logger.LogWarning($"Validation des cartes terminée avec {_totalWarnings} avertissements");
+                return true; // Les avertissements ne font pas échouer la validation
             }
             else
             {
                 Logger.LogSuccess("Validation des cartes terminée avec succès");
+                return true;
+            }
+        }
+        
+        /// <summary>
+        /// Exécute tous les tests de validation.
+        /// </summary>
+        /// <returns>True si tous les tests ont réussi, sinon false.</returns>
+        public bool RunAllTests()
+        {
+            try
+            {
+                return RunAllCardValidations().Result;
+            }
+            catch (Exception ex)
+            {
+                Logger.LogProblem($"Erreur lors de l'exécution des tests de validation des cartes : {ex.Message}");
+                return false;
             }
-
-            Logger.LogTitle("Rapport de validation enregistré dans : " + _validatorConfig.ValidationReportPath);
         }
 
         /// <summary>
diff --git a/Generation/Converters/Argumentum.AssetConverter/Tests/ContinuousValidationConfig.cs b/Generation/Converters/Argumentum.AssetConverter/Tests/ContinuousValidationConfig.cs
index e3dcf78..eb09a48 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Tests/ContinuousValidationConfig.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/Tests/ContinuousValidationConfig.cs
@@ -109,15 +109,9 @@
         /// <summary>
         /// Niveau de sévérité minimum pour déclencher une notification
         /// </summary>
-        // Définition temporaire de l'énumération ValidationSeverity
-        public enum ValidationSeverity
-        {
-            Info,
-            Warning,
-            Error,
-            Critical
-        }
-
+        /// <summary>
+        /// Niveau de sévérité minimum pour déclencher une notification
+        /// </summary>
         public ValidationSeverity NotificationThreshold { get; set; } = ValidationSeverity.Error;
 
         /// <summary>
diff --git a/Generation/Converters/Argumentum.AssetConverter/Tests/OwlOntologyValidationTests.cs b/Generation/Converters/Argumentum.AssetConverter/Tests/OwlOntologyValidationTests.cs
index 39f8a54..1006b17 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Tests/OwlOntologyValidationTests.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/Tests/OwlOntologyValidationTests.cs
@@ -19,7 +19,7 @@
     public class OwlOntologyValidationTests
     {
         private readonly AssetConverterConfig _config;
-        private OWLOntology _ontology;
+        private OwlAdapter _ontology;
         private readonly OwlValidatorConfig _validatorConfig;
 
         /// <summary>
@@ -48,7 +48,7 @@
 
             try
             {
-                _ontology = OWLOntology.FromFile(OWLEnums.OWLFormats.OwlXml, owlFilePath);
+                _ontology = OwlAdapter.FromFile(owlFilePath);
                 Logger.LogSuccess($"Ontologie OWL chargée : {owlFilePath}");
             }
             catch (Exception ex)
@@ -58,6 +58,34 @@
             }
         }
 
+        /// <summary>
+        /// Exécute tous les tests de validation.
+        /// </summary>
+        /// <returns>True si tous les tests ont réussi, sinon false.</returns>
+        public bool RunAllTests()
+        {
+            try
+            {
+                LoadOntology().Wait();
+
+                if (_ontology == null)
+                {
+                    return false;
+                }
+
+                bool structureValid = ValidateOwlOntologyStructure().Result;
+                bool annotationsValid = ValidateMultilingualAnnotations().Result;
+                bool mappingsValid = ValidateAIFMappings().Result;
+
+                return structureValid && annotationsValid && mappingsValid;
+            }
+            catch (Exception ex)
+            {
+                Logger.LogProblem($"Erreur lors de l'exécution des tests de validation : {ex.Message}");
+                return false;
+            }
+        }
+
         /// <summary>
         /// Vérifie la structure de base de l'ontologie.
         /// - Présence des concepts principaux
@@ -65,7 +93,7 @@
         /// - Intégrité des relations entre concepts
         /// </summary>
         /// <returns>Une tâche représentant l'opération asynchrone.</returns>
-        public async Task ValidateOwlOntologyStructure()
+        public async Task<bool> ValidateOwlOntologyStructure()
         {
             Logger.LogTitle("Validation de la structure de l'ontologie OWL");
 
@@ -77,7 +105,7 @@
             if (_ontology == null)
             {
                 Logger.LogProblem("Impossible de valider la structure de l'ontologie : aucune ontologie chargée.");
-                return;
+                return false;
             }
 
             int errorCount = 0;
@@ -90,7 +118,8 @@
                 bool conceptExists = false;
 
                 // Recherche du concept dans l'ontologie
-                foreach (var concept in _ontology.Model.ClassModel.Classes)
+                var ontologyObj = _ontology.GetOntology();
+                foreach (var concept in ontologyObj.Model.ClassModel.Classes)
                 {
                     if (concept.ToString().EndsWith(requiredConcept))
                     {
@@ -118,7 +147,8 @@
                 bool relationExists = false;
 
                 // Recherche de la relation dans l'ontologie
-                foreach (var property in _ontology.Model.PropertyModel.Properties)
+                var ontologyObj = _ontology.GetOntology();
+                foreach (var property in ontologyObj.Model.PropertyModel.Properties)
                 {
                     if (property.ToString().EndsWith(requiredRelation))
                     {
@@ -195,6 +225,7 @@
             {
                 Logger.LogProblem($"Validation de la structure de l'ontologie OWL : {errorCount} erreurs détectées");
                 Logger.Log(report.ToString());
+                return false;
             }
             else
             {
@@ -203,6 +234,7 @@
                 {
                     Logger.Log(report.ToString());
                 }
+                return true;
             }
         }
 
@@ -213,7 +245,7 @@
         /// - Présence des exemples dans toutes les langues
         /// </summary>
         /// <returns>Une tâche représentant l'opération asynchrone.</returns>
-        public async Task ValidateMultilingualAnnotations()
+        public async Task<bool> ValidateMultilingualAnnotations()
         {
             Logger.LogTitle("Validation des annotations multilingues de l'ontologie OWL");
 
@@ -225,7 +257,7 @@
             if (_ontology == null)
             {
                 Logger.LogProblem("Impossible de valider les annotations multilingues : aucune ontologie chargée.");
-                return;
+                return false;
             }
 
             int errorCount = 0;
@@ -263,7 +295,7 @@
                 var missingDefinitions = new List<string>();
                 foreach (var language in _validatorConfig.LanguagesToValidate)
                 {
-                    var definitions = _ontology.GetConceptDocumentation(concept, SKOSEnums.SKOSDocumentationTypes.Definition)
+                    var definitions = _ontology.GetConceptDocumentation(concept, Ontology.SKOSDocumentationTypes.Definition)
                         .Where(d => d.Language.Equals(language, StringComparison.OrdinalIgnoreCase))
                         .ToList();
 
@@ -283,7 +315,7 @@
                 var missingExamples = new List<string>();
                 foreach (var language in _validatorConfig.LanguagesToValidate)
                 {
-                    var examples = _ontology.GetConceptDocumentation(concept, SKOSEnums.SKOSDocumentationTypes.Example)
+                    var examples = _ontology.GetConceptDocumentation(concept, Ontology.SKOSDocumentationTypes.Example)
                         .Where(e => e.Language.Equals(language, StringComparison.OrdinalIgnoreCase))
                         .ToList();
 
@@ -307,6 +339,7 @@
             {
                 Logger.LogProblem($"Validation des annotations multilingues : {errorCount} erreurs détectées");
                 Logger.Log(report.ToString());
+                return false;
             }
             else
             {
@@ -315,6 +348,7 @@
                 {
                     Logger.Log(report.ToString());
                 }
+                return true;
             }
         }
 
@@ -325,7 +359,7 @@
         /// - Couverture des mappings
         /// </summary>
         /// <returns>Une tâche représentant l'opération asynchrone.</returns>
-        public async Task ValidateAIFMappings()
+        public async Task<bool> ValidateAIFMappings()
         {
             Logger.LogTitle("Validation des mappings AIF de l'ontologie OWL");
 
@@ -337,16 +371,16 @@
             if (_ontology == null)
             {
                 Logger.LogProblem("Impossible de valider les mappings AIF : aucune ontologie chargée.");
-                return;
+                return false;
             }
 
             // Charger l'ontologie AIF si elle existe
-            OWLOntology aifOntology = null;
+            OwlAdapter aifOntology = null;
             if (File.Exists(_validatorConfig.AifOwlFilePath))
             {
                 try
                 {
-                    aifOntology = OWLOntology.FromFile(OWLEnums.OWLFormats.OwlXml, _validatorConfig.AifOwlFilePath);
+                    aifOntology = OwlAdapter.FromFile(_validatorConfig.AifOwlFilePath);
                     Logger.LogSuccess($"Ontologie AIF chargée : {_validatorConfig.AifOwlFilePath}");
                 }
                 catch (Exception ex)
@@ -411,7 +445,7 @@
                     // Si l'ontologie AIF est disponible, vérifier que la ressource existe
                     if (aifOntology != null && resource.ToString().Contains(_validatorConfig.AifOwlFilePath))
                     {
-                        bool resourceExists = aifOntology.Model.ClassModel.CheckHasClass(resource);
+                        bool resourceExists = aifOntology.CheckHasClass(resource);
                         if (!resourceExists)
                         {
                             errorCount++;
@@ -448,11 +482,13 @@
             {
                 Logger.LogProblem($"Validation des mappings AIF : {errorCount} erreurs détectées");
                 Logger.Log(report.ToString());
+                return false;
             }
             else
             {
                 Logger.LogSuccess("Validation des mappings AIF : aucune erreur détectée");
                 Logger.Log(report.ToString());
+                return true;
             }
         }
 
@@ -460,7 +496,7 @@
         /// Exécute tous les tests de validation et génère un rapport global.
         /// </summary>
         /// <returns>Une tâche représentant l'opération asynchrone.</returns>
-        public async Task RunAllOwlValidations()
+        public async Task<bool> RunAllOwlValidations()
         {
             Logger.LogTitle("Exécution de tous les tests de validation de l'ontologie OWL");
 
@@ -469,12 +505,12 @@
             if (_ontology == null)
             {
                 Logger.LogProblem("Impossible d'exécuter les validations : aucune ontologie chargée.");
-                return;
+                return false;
             }
 
-            await ValidateOwlOntologyStructure();
-            await ValidateMultilingualAnnotations();
-            await ValidateAIFMappings();
+            bool structureValid = await ValidateOwlOntologyStructure();
+            bool annotationsValid = await ValidateMultilingualAnnotations();
+            bool mappingsValid = await ValidateAIFMappings();
 
             // Générer un rapport de validation global
             string reportDirectory = Path.GetDirectoryName(_validatorConfig.ValidationReportPath);
@@ -484,6 +520,8 @@
             }
 
             Logger.LogTitle("Fin des tests de validation de l'ontologie OWL");
+
+            return structureValid && annotationsValid && mappingsValid;
         }
     }
 }
diff --git a/Generation/Converters/Argumentum.AssetConverter/Tests/TaxonomyValidationTests.cs b/Generation/Converters/Argumentum.AssetConverter/Tests/TaxonomyValidationTests.cs
index bfade14..b8b5e3d 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Tests/TaxonomyValidationTests.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/Tests/TaxonomyValidationTests.cs
@@ -50,7 +50,7 @@
         /// - Existence des parents pour chaque nœud
         /// </summary>
         /// <returns>Une tâche représentant l'opération asynchrone.</returns>
-        public async Task ValidateTaxonomyStructure()
+        public async Task<bool> ValidateTaxonomyStructure()
         {
             Logger.LogTitle("Validation de la structure de la taxonomie");
 
@@ -62,7 +62,7 @@
             if (_fallacies == null || !_fallacies.Any())
             {
                 Logger.LogProblem("Impossible de valider la structure de la taxonomie : aucune donnée chargée.");
-                return;
+                return false;
             }
 
             int errorCount = 0;
@@ -134,10 +134,12 @@
             {
                 Logger.LogProblem($"Validation de la structure de la taxonomie : {errorCount} erreurs détectées");
                 Logger.Log(report.ToString());
+                return false;
             }
             else
             {
                 Logger.LogSuccess("Validation de la structure de la taxonomie : aucune erreur détectée");
+                return true;
             }
         }
 
@@ -148,7 +150,7 @@
         /// - Génère un rapport des traductions manquantes
         /// </summary>
         /// <returns>Une tâche représentant l'opération asynchrone.</returns>
-        public async Task ValidateTranslationCompleteness()
+        public async Task<bool> ValidateTranslationCompleteness()
         {
             Logger.LogTitle("Validation de la complétude des traductions");
 
@@ -160,7 +162,7 @@
             if (_fallacies == null || !_fallacies.Any())
             {
                 Logger.LogProblem("Impossible de valider la complétude des traductions : aucune donnée chargée.");
-                return;
+                return false;
             }
 
             var missingTranslations = new Dictionary<string, Dictionary<string, List<string>>>();
@@ -268,10 +270,12 @@
             {
                 Logger.LogProblem($"Validation de la complétude des traductions : {totalMissing} traductions manquantes");
                 Logger.Log(report.ToString());
+                return false;
             }
             else
             {
                 Logger.LogSuccess("Validation de la complétude des traductions : toutes les traductions sont présentes");
+                return true;
             }
         }
 
@@ -308,7 +312,7 @@
         /// - Détection des incohérences potentielles
         /// </summary>
         /// <returns>Une tâche représentant l'opération asynchrone.</returns>
-        public async Task ValidateTerminologyConsistency()
+        public async Task<bool> ValidateTerminologyConsistency()
         {
             Logger.LogTitle("Validation de la cohérence terminologique");
 
@@ -320,7 +324,7 @@
             if (_fallacies == null || !_fallacies.Any())
             {
                 Logger.LogProblem("Impossible de valider la cohérence terminologique : aucune donnée chargée.");
-                return;
+                return false;
             }
 
             // Dictionnaire pour stocker les termes par langue et par niveau hiérarchique
@@ -462,10 +466,12 @@
             {
                 Logger.LogProblem("Validation de la cohérence terminologique : des incohérences ont été détectées");
                 Logger.Log(report.ToString());
+                return false;
             }
             else
             {
                 Logger.LogSuccess("Validation de la cohérence terminologique : aucune incohérence détectée");
+                return true;
             }
         }
 
@@ -473,7 +479,7 @@
         /// Exécute tous les tests de validation et génère un rapport global.
         /// </summary>
         /// <returns>Une tâche représentant l'opération asynchrone.</returns>
-        public async Task RunAllValidations()
+        public async Task<bool> RunAllValidations()
         {
             Logger.LogTitle("Exécution de tous les tests de validation de taxonomie");
 
@@ -482,14 +488,33 @@
             if (_fallacies == null || !_fallacies.Any())
             {
                 Logger.LogProblem("Impossible d'exécuter les validations : aucune donnée chargée.");
-                return;
+                return false;
             }
 
-            await ValidateTaxonomyStructure();
-            await ValidateTranslationCompleteness();
-            await ValidateTerminologyConsistency();
+            bool structureValid = await ValidateTaxonomyStructure();
+            bool translationsValid = await ValidateTranslationCompleteness();
+            bool terminologyValid = await ValidateTerminologyConsistency();
 
             Logger.LogTitle("Fin des tests de validation de taxonomie");
+
+            return structureValid && translationsValid && terminologyValid;
+        }
+        
+        /// <summary>
+        /// Exécute tous les tests de validation.
+        /// </summary>
+        /// <returns>True si tous les tests ont réussi, sinon false.</returns>
+        public bool RunAllTests()
+        {
+            try
+            {
+                return RunAllValidations().Result;
+            }
+            catch (Exception ex)
+            {
+                Logger.LogProblem($"Erreur lors de l'exécution des tests de validation : {ex.Message}");
+                return false;
+            }
         }
     }
 }
diff --git a/Generation/Converters/Argumentum.AssetConverter/Tests/ValidationSeverity.cs b/Generation/Converters/Argumentum.AssetConverter/Tests/ValidationSeverity.cs
new file mode 100644
index 0000000..96c43c0
--- /dev/null
+++ b/Generation/Converters/Argumentum.AssetConverter/Tests/ValidationSeverity.cs
@@ -0,0 +1,30 @@
+using System;
+
+namespace Argumentum.AssetConverter.Tests
+{
+    /// <summary>
+    /// Niveaux de sévérité pour les validations
+    /// </summary>
+    public enum ValidationSeverity
+    {
+        /// <summary>
+        /// Information - niveau le plus bas
+        /// </summary>
+        Info,
+        
+        /// <summary>
+        /// Avertissement - problème mineur
+        /// </summary>
+        Warning,
+        
+        /// <summary>
+        /// Erreur - problème majeur
+        /// </summary>
+        Error,
+        
+        /// <summary>
+        /// Critique - problème bloquant
+        /// </summary>
+        Critical
+    }
+}
```


### Commit `c8e8fe7a7ba07079428c8160d67d8ca28ad59403`

**Message :** Adaptation du code pour la compatibilité avec OWLSharp 4.6.1 et ajout de la méthode LogInfoMessage

**Statistiques :**
```
.../Converters/Argumentum.AssetConverter/Logger.cs | 5 +
.../Ontology/OwlAdapter.cs | 282 +++++++++++++
.../Ontology/OwlGeneratorConfig.cs | 415 ++---------
3 files changed, 462 insertions(+), 240 deletions(-)
```

**Diff complet :**
```diff
commit c8e8fe7a7ba07079428c8160d67d8ca28ad59403
Author: jsboige <jsboige@gmail.com>
Date:   Tue May 13 18:08:06 2025 +0200

    Adaptation du code pour la compatibilité avec OWLSharp 4.6.1 et ajout de la méthode LogInfoMessage

    Former-commit-id: 3580e42d1497af55da01d95e20aaf87bc3b4558
    Former-commit-id: 1f9bdc730b9c27fbd97b8c58414dd64cbba51ecf

diff --git a/Generation/Converters/Argumentum.AssetConverter/Logger.cs b/Generation/Converters/Argumentum.AssetConverter/Logger.cs
index 6330912..3ad336b 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Logger.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/Logger.cs
@@ -127,4 +127,9 @@
         {
                 Log(message, MessageType.Success);
         }
+
+        public static void LogInfoMessage(string message)
+        {
+                Log(message, MessageType.Info);
+        }
 }
diff --git a/Generation/Converters/Argumentum.AssetConverter/Ontology/OwlAdapter.cs b/Generation/Converters/Argumentum.AssetConverter/Ontology/OwlAdapter.cs
new file mode 100644
index 0000000..3790f0a
--- /dev/null
+++ b/Generation/Converters/Argumentum.AssetConverter/Ontology/OwlAdapter.cs
@@ -0,0 +1,282 @@
+using System;
+using System.Collections.Generic;
+using System.IO;
+using System.Linq;
+using System.Reflection;
+using OWLSharp;
+using OWLSharp.Extensions.SKOS;
+using RDFSharp.Model;
+
+namespace Argumentum.AssetConverter.Ontology
+{
+    /// <summary>
+    /// Types de documentation SKOS
+    /// </summary>
+    public enum SKOSDocumentationTypes
+    {
+        Definition,
+        Example
+    }
+
+    /// <summary>
+    /// Adaptateur pour la bibliothèque OWLSharp 4.6.1
+    /// </summary>
+    public class OwlAdapter
+    {
+        private dynamic _ontology;
+        private string _namespace;
+
+        public OwlAdapter(string ontologyNamespace)
+        {
+            _namespace = ontologyNamespace;
+
+            try
+            {
+                // Créer une instance de OWLOntology par réflexion
+                Type owlOntologyType = Type.GetType("OWLSharp.OWLOntology, OWLSharp");
+                if (owlOntologyType == null)
+                {
+                    // Essayer avec un autre namespace possible
+                    owlOntologyType = Type.GetType("OWLSharp.Model.OWLOntology, OWLSharp");
+                }
+
+                if (owlOntologyType != null)
+                {
+                    // Créer une instance avec le constructeur qui prend un string
+                    _ontology = Activator.CreateInstance(owlOntologyType, new object[] { ontologyNamespace });
+                    
+                    // Appeler la méthode InitializeSKOS par réflexion
+                    var initMethod = owlOntologyType.GetMethod("InitializeSKOS");
+                    if (initMethod != null)
+                    {
+                        initMethod.Invoke(_ontology, null);
+                    }
+                }
+                else
+                {
+                    throw new InvalidOperationException("Type OWLOntology non trouvé");
+                }
+            }
+            catch (Exception ex)
+            {
+                Logger.LogProblem($"Erreur lors de la création de l'ontologie OWL : {ex.Message}");
+                throw;
+            }
+        }
+
+        public static OwlAdapter FromFile(string filePath)
+        {
+            try
+            {
+                // Créer une instance vide
+                var adapter = new OwlAdapter("http://temp.namespace.org");
+
+                // Trouver le type OWLOntology par réflexion
+                Type owlOntologyType = Type.GetType("OWLSharp.OWLOntology, OWLSharp");
+                if (owlOntologyType == null)
+                {
+                    owlOntologyType = Type.GetType("OWLSharp.Model.OWLOntology, OWLSharp");
+                }
+
+                if (owlOntologyType != null)
+                {
+                    // Trouver la méthode FromFile
+                    var fromFileMethod = owlOntologyType.GetMethod("FromFile",
+                        BindingFlags.Public | BindingFlags.Static);
+                    
+                    if (fromFileMethod != null)
+                    {
+                        // Trouver l'enum RDFFormats.RdfXml
+                        Type rdfFormatsType = Type.GetType("RDFSharp.Model.RDFModelEnums+RDFFormats, RDFSharp");
+                        if (rdfFormatsType != null)
+                        {
+                            object rdfXmlFormat = Enum.Parse(rdfFormatsType, "RdfXml");
+                            
+                            // Appeler la méthode FromFile
+                            adapter._ontology = fromFileMethod.Invoke(null, new object[] { rdfXmlFormat, filePath });
+                            
+                            // Extraire le namespace de l'ontologie chargée
+                            var ontologyProperty = owlOntologyType.GetProperty("Ontology");
+                            if (ontologyProperty != null)
+                            {
+                                var ontologyValue = ontologyProperty.GetValue(adapter._ontology);
+                                adapter._namespace = ontologyValue?.ToString() ?? "http://unknown.namespace.org";
+                            }
+                            
+                            return adapter;
+                        }
+                    }
+                }
+
+                throw new InvalidOperationException("Impossible de charger l'ontologie OWL à partir du fichier");
+            }
+            catch (Exception ex)
+            {
+                Logger.LogProblem($"Erreur lors du chargement de l'ontologie: {ex.Message}");
+                throw;
+            }
+        }
+
+        public void Annotate(RDFResource property, RDFPlainLiteral value)
+        {
+            _ontology.Annotate(property, value);
+        }
+
+        public void DeclareClass(RDFResource resource)
+        {
+            _ontology.Model.ClassModel.DeclareClass(resource);
+        }
+
+        public void DeclareObjectProperty(RDFResource resource)
+        {
+            _ontology.Model.PropertyModel.DeclareObjectProperty(resource);
+        }
+
+        public void DeclareConceptScheme(RDFResource scheme)
+        {
+            _ontology.DeclareSKOSConceptScheme(scheme);
+        }
+
+        public void DeclareConcept(RDFResource concept, RDFResource scheme)
+        {
+            _ontology.DeclareSKOSConcept(concept);
+            _ontology.AddSKOSConceptToScheme(concept, scheme);
+        }
+
+        public void DeclareTopConcept(RDFResource concept, RDFResource scheme)
+        {
+            _ontology.DeclareSKOSTopConcept(concept, scheme);
+        }
+
+        public void DeclareNarrowerConcepts(RDFResource parentConcept, RDFResource childConcept)
+        {
+            _ontology.DeclareSKOSNarrowerConcept(parentConcept, childConcept);
+        }
+
+        public void DeclareExactMatchConcepts(RDFResource concept1, RDFResource concept2)
+        {
+            _ontology.DeclareSKOSExactMatch(concept1, concept2);
+        }
+
+        public void DeclareCloseMatchConcepts(RDFResource concept1, RDFResource concept2)
+        {
+            _ontology.DeclareSKOSCloseMatch(concept1, concept2);
+        }
+
+        public void DeclareBroadMatchConcepts(RDFResource concept1, RDFResource concept2)
+        {
+            _ontology.DeclareSKOSBroadMatch(concept1, concept2);
+        }
+
+        public void DeclareNarrowMatchConcepts(RDFResource concept1, RDFResource concept2)
+        {
+            _ontology.DeclareSKOSNarrowMatch(concept1, concept2);
+        }
+
+        public void DeclareRelatedMatchConcepts(RDFResource concept1, RDFResource concept2)
+        {
+            _ontology.DeclareSKOSRelatedMatch(concept1, concept2);
+        }
+
+        public void DeclareQualifiedCardinalityRestriction(RDFResource restrictionClass, RDFResource onProperty, int cardinality, RDFResource onClass)
+        {
+            _ontology.Model.ClassModel.DeclareQualifiedCardinalityRestriction(restrictionClass, onProperty, (uint)cardinality, onClass);
+        }
+
+        public void DeclareIntersectionClass(RDFResource intersectionClass, List<RDFResource> intersectionClassMembers)
+        {
+            _ontology.Model.ClassModel.DeclareIntersectionClass(intersectionClass, intersectionClassMembers);
+        }
+
+        public void DeclareUnionClass(RDFResource unionClass, List<RDFResource> unionClassMembers)
+        {
+            _ontology.Model.ClassModel.DeclareUnionClass(unionClass, unionClassMembers);
+        }
+
+        public void AnnotateConceptPreferredLabel(RDFResource concept, RDFPlainLiteral label)
+        {
+            _ontology.AnnotateSKOSPreferredLabel(concept, label);
+        }
+
+        public void AnnotateConcept(RDFResource concept, RDFResource property, RDFPlainLiteral value)
+        {
+            _ontology.Data.AddTriple(new RDFTriple(concept, property, value));
+        }
+
+        public void DocumentConcept(RDFResource concept, SKOSDocumentationTypes documentationType, RDFPlainLiteral value)
+        {
+            switch (documentationType)
+            {
+                case SKOSDocumentationTypes.Definition:
+                    _ontology.AnnotateSKOSDefinition(concept, value);
+                    break;
+                case SKOSDocumentationTypes.Example:
+                    _ontology.AnnotateSKOSExample(concept, value);
+                    break;
+            }
+        }
+
+        public void ToFile(OWLEnums.OWLFormats format, string filePath)
+        {
+            _ontology.ToFile(format, filePath);
+        }
+
+        public List<RDFResource> GetConcepts()
+        {
+            return _ontology.GetSKOSConcepts().ToList();
+        }
+
+        public List<RDFResource> GetTopConcepts()
+        {
+            return _ontology.GetSKOSTopConcepts().ToList();
+        }
+
+        public bool CheckIsNarrowerConcept(RDFResource concept, RDFResource parentConcept)
+        {
+            return _ontology.CheckHasSKOSNarrowerConcept(parentConcept, concept);
+        }
+
+        public List<RDFPlainLiteral> GetConceptPreferredLabels(RDFResource concept)
+        {
+            return _ontology.GetSKOSPreferredLabels(concept).ToList();
+        }
+
+        public List<RDFPlainLiteral> GetConceptDocumentation(RDFResource concept, SKOSDocumentationTypes documentationType)
+        {
+            switch (documentationType)
+            {
+                case SKOSDocumentationTypes.Definition:
+                    return _ontology.GetSKOSDefinitions(concept).ToList();
+                case SKOSDocumentationTypes.Example:
+                    return _ontology.GetSKOSExamples(concept).ToList();
+                default:
+                    return new List<RDFPlainLiteral>();
+            }
+        }
+
+        public List<RDFResource> GetExactMatchConcepts(RDFResource concept)
+        {
+            return _ontology.GetSKOSExactMatches(concept).ToList();
+        }
+
+        public List<RDFResource> GetCloseMatchConcepts(RDFResource concept)
+        {
+            return _ontology.GetSKOSCloseMatches(concept).ToList();
+        }
+
+        public List<RDFResource> GetRelatedMatchConcepts(RDFResource concept)
+        {
+            return _ontology.GetSKOSRelatedMatches(concept).ToList();
+        }
+
+        public bool CheckHasClass(RDFResource resource)
+        {
+            return _ontology.Model.ClassModel.CheckHasClass(resource);
+        }
+
+        public dynamic GetOntology()
+        {
+            return _ontology;
+        }
+    }
+}
diff --git a/Generation/Converters/Argumentum.AssetConverter/Ontology/OwlGeneratorConfig.cs b/Generation/Converters/Argumentum.AssetConverter/Ontology/OwlGeneratorConfig.cs
index f9eec72..43c37a0 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Ontology/OwlGeneratorConfig.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/Ontology/OwlGeneratorConfig.cs
@@ -103,257 +103,192 @@
 
             private void CreateOwlDocument(IList<Fallacy> fallacies, AssetConverterConfig config, string language, string fileName)
             {
-                var fallaciesByPath = fallacies.ToDictionary(f => f.Path, f => f);
-                Fallacy GetParent(Fallacy f)
-                {
-                    if (f.Depth <= 1)
-                        return fallacies.First();
-                    var parentPath = f.Path.Substring(0, f.Path.LastIndexOf('.'));
-                    return fallaciesByPath[parentPath];
-                }
-
-                var ontology = new OWLOntology(OntologyNamespace);
-
-                //Metadata init
-
-
-                ontology.Annotate(RDFVocabulary.RDFS.COMMENT,
-                    new RDFPlainLiteral(Comment, "en"));
-
-                ontology.Annotate(RDFVocabulary.OWL.VERSION_INFO,
-                    new RDFPlainLiteral(Version.ToString()));
-
-                ontology.Annotate(RDFVocabulary.DC.CREATOR,
-                    new RDFPlainLiteral(Creator.ToString()));
-
-
-                //SKOS init
-
-
-                ontology.InitializeSKOS();
-
-
-
-                // AIF init
-
-
-                //ontology.Import(new RDFResource(AIFUri));
-
-
-                var aifConflictUri = $"{ExternalReferenceOntologyNamespaceURI}Conflict";
-                var conflictResource = new RDFResource(aifConflictUri);
-
-                ontology.Model.ClassModel.DeclareClass(conflictResource);
-
-                var aifHasConflictUri = $"{ExternalReferenceOntologyNamespaceURI}hasConflictedElement";
-                var hasConflictResource = new RDFResource(aifHasConflictUri);
-
-                ontology.Model.PropertyModel.DeclareObjectProperty(hasConflictResource);
-
-
-
-                // Scheme declarartion
-
-                var schemeName = GetId(fallacies.First().TextEn);
-                RDFResource mainScheme = new RDFResource($"{OntologyNamespace}{schemeName}Scheme" );
-                ontology.DeclareConceptScheme(mainScheme);
-
-                var concepts = new Dictionary<Fallacy, RDFResource>();
-
-
-                //var aifNamespace = new RDFNamespace("aif", AIFNamespaceURI);
-
-
-                var conflictedTypedInferences = new Dictionary<string, RDFResource>();
-
-
-                foreach (var fallacy in fallacies)
-                {
-
-                    var fallacyConcept = this.GetFallacyConcept(fallacy, ontology, mainScheme);
-                    concepts[fallacy] = fallacyConcept;
-
-
-                    // Hierarchy
-
-                    var parentFallacy = GetParent(fallacy);
-
-                    if (parentFallacy == fallacy)
-                    {
-                        ontology.DeclareTopConcept(fallacyConcept, mainScheme);
-                    }
-                    else
-                    {
-                        var parentResource = concepts[parentFallacy];
-                        try
-                        {
-                            ontology.DeclareNarrowerConcepts(parentResource, fallacyConcept);
-                        }
-                        catch (Exception e)
-                        {
-                            Console.WriteLine(e);
-                        }
-
-                    }
-
-                    //AIF mappings
-
-                    if (!string.IsNullOrEmpty(fallacy.AIFSkosMappingType))
-                    {
-                        var directMappings = fallacy.AIFSkosDirectRef.Split(',').Select(x=>x.Trim()).Where(x=>!string.IsNullOrEmpty(x));
-                        var exceptionMappings = fallacy.AIFSkosExceptionRef.Split(',').Select(x=>x.Trim()).Where(x => !string.IsNullOrEmpty(x));
-
-                        var mappedConcepts = new List<RDFResource>();
-
-
-
-                        //Direct mappings
-
-                        foreach (var directMapping in directMappings)
-                        {
-                            var aifUri = $"{ExternalReferenceOntologyNamespaceURI}{directMapping}";
-
-                            var directConcept = new RDFResource(aifUri);
-
-                            mappedConcepts.Add(directConcept);
-
-                        }
-
-
-                        //Indirect exception mappings
-
-                        foreach (var exceptionMapping in exceptionMappings)
-                        {
-
-
-
-                            if (!conflictedTypedInferences.TryGetValue(exceptionMapping,out var typedInferenceConflictResource))
-                            {
-                                var aifUri = $"{ExternalReferenceOntologyNamespaceURI}{exceptionMapping}";
-
-                                var regularInferenceType = new RDFResource(aifUri);
-
-                                var conflictedTypedInferenceUri = $"{OntologyNamespace}{exceptionMapping}_Conflicted";
-
-
-                                var hasConflictedTypedInference = new RDFResource(conflictedTypedInferenceUri);
-
-                                ontology.Model.ClassModel.DeclareClass(hasConflictedTypedInference);
-
-                                ontology.Model.ClassModel.DeclareQualifiedCardinalityRestriction(hasConflictedTypedInference, hasConflictResource, 1, regularInferenceType);
-
-
-                                var typedInferenceConflictResourceUri = $"{OntologyNamespace}{exceptionMapping}_Conflict";
-
-                                typedInferenceConflictResource = new RDFResource(typedInferenceConflictResourceUri);
-
-                                ontology.Model.ClassModel.DeclareClass(typedInferenceConflictResource);
-                                var intersectionList = new List<RDFResource> { conflictResource, hasConflictedTypedInference };
-                                ontology.Model.ClassModel.DeclareIntersectionClass(typedInferenceConflictResource, intersectionList);
-
-                                conflictedTypedInferences[exceptionMapping] = typedInferenceConflictResource;
-                            }
-
-
-
-                            mappedConcepts.Add(typedInferenceConflictResource);
-                        }
-
-
-
-                        if (mappedConcepts.Count>0)
-                        {
-                            RDFResource mappedConcept = mappedConcepts.First();
-
-                            if (mappedConcepts.Count>1)
-                            {
-                                var fallacyId = GetId(fallacy.TextEn);
-                                var fallacyConflictUri = $"{OntologyNamespace}{fallacyId}_Conflict";
-
-
-                                RDFResource conflictUnionClass = new RDFResource(fallacyConflictUri);
-
-                                ontology.Model.ClassModel.DeclareClass(conflictUnionClass);
-
-                                ontology.Model.ClassModel.DeclareUnionClass(conflictUnionClass, mappedConcepts);
-
-                                mappedConcept = conflictUnionClass;
-
-                            }
-
-
-                            //ontology.Model.ClassModel.DeclareEquivalentClasses(conflictEquivalentClass, intersectionClass);
-
-
-                            switch (fallacy.AIFSkosMappingType)
-                            {
-                                case "skos:exactMatch":
-                                    ontology.DeclareExactMatchConcepts(fallacyConcept, mappedConcept);
-                                    break;
-                                case "skos:closeMatch":
-                                    ontology.DeclareCloseMatchConcepts(fallacyConcept, mappedConcept);
-                                    break;
-                                case "skos:broadMatch":
-                                    ontology.DeclareBroadMatchConcepts(fallacyConcept, mappedConcept);
-                                    break;
-                                case "skos:narrowMatch":
-                                    ontology.DeclareNarrowMatchConcepts(fallacyConcept, mappedConcept);
-                                    break;
-                                case "skos:relatedMatch":
-                                    ontology.DeclareRelatedMatchConcepts(fallacyConcept, mappedConcept);
-                                    break;
-                            }
-
-                        }
-
-                    }
-
-                }
-
-                //Saving
-
-                var xmlFormat = OWLEnums.OWLFormats.OwlXml;
-                // WRITE OWL2/XML FILE
-                ontology.ToFile(xmlFormat, fileName);
-
-                Logger.LogSuccess($"Owl document {fileName} successfully saved");
-
+                var fallaciesByPath = fallacies.ToDictionary(f => f.Path, f => f);
+                Fallacy GetParent(Fallacy f)
+                {
+                    if (f.Depth <= 1)
+                        return fallacies.First();
+                    var parentPath = f.Path.Substring(0, f.Path.LastIndexOf('.'));
+                    return fallaciesByPath[parentPath];
+                }
+                
+                var ontology = new OwlAdapter(OntologyNamespace);
+
+                //Metadata init
+                ontology.Annotate(RDFVocabulary.RDFS.COMMENT,
+                    new RDFPlainLiteral(Comment, "en"));
+
+                ontology.Annotate(RDFVocabulary.OWL.VERSION_INFO,
+                    new RDFPlainLiteral(Version.ToString()));
+
+                ontology.Annotate(RDFVocabulary.DC.CREATOR,
+                    new RDFPlainLiteral(Creator.ToString()));
+
+                // AIF init
+                var aifConflictUri = $"{ExternalReferenceOntologyNamespaceURI}Conflict";
+                var conflictResource = new RDFResource(aifConflictUri);
+
+                ontology.DeclareClass(conflictResource);
+
+                var aifHasConflictUri = $"{ExternalReferenceOntologyNamespaceURI}hasConflictedElement";
+                var hasConflictResource = new RDFResource(aifHasConflictUri);
+
+                ontology.DeclareObjectProperty(hasConflictResource);
+
+                // Scheme declaration
+                var schemeName = GetId(fallacies.First().TextEn);
+                RDFResource mainScheme = new RDFResource($"{OntologyNamespace}{schemeName}Scheme" );
+                ontology.DeclareConceptScheme(mainScheme);
+
+                var concepts = new Dictionary<Fallacy, RDFResource>();
+                var conflictedTypedInferences = new Dictionary<string, RDFResource>();
+
+                foreach (var fallacy in fallacies)
+                {
+                    var fallacyConcept = this.GetFallacyConcept(fallacy, ontology, mainScheme);
+                    concepts[fallacy] = fallacyConcept;
+
+                    // Hierarchy
+                    var parentFallacy = GetParent(fallacy);
+
+                    if (parentFallacy == fallacy)
+                    {
+                        ontology.DeclareTopConcept(fallacyConcept, mainScheme);
+                    }
+                    else
+                    {
+                        var parentResource = concepts[parentFallacy];
+                        try
+                        {
+                            ontology.DeclareNarrowerConcepts(parentResource, fallacyConcept);
+                        }
+                        catch (Exception e)
+                        {
+                            Console.WriteLine(e);
+                        }
+                    }
+
+                    //AIF mappings
+                    if (!string.IsNullOrEmpty(fallacy.AIFSkosMappingType))
+                    {
+                        var directMappings = fallacy.AIFSkosDirectRef.Split(',').Select(x=>x.Trim()).Where(x=>!string.IsNullOrEmpty(x));
+                        var exceptionMappings = fallacy.AIFSkosExceptionRef.Split(',').Select(x=>x.Trim()).Where(x => !string.IsNullOrEmpty(x));
+
+                        var mappedConcepts = new List<RDFResource>();
+
+                        //Direct mappings
+                        foreach (var directMapping in directMappings)
+                        {
+                            var aifUri = $"{ExternalReferenceOntologyNamespaceURI}{directMapping}";
+                            var directConcept = new RDFResource(aifUri);
+                            mappedConcepts.Add(directConcept);
+                        }
+                        
+                        //Indirect exception mappings
+                        foreach (var exceptionMapping in exceptionMappings)
+                        {
+                            if (!conflictedTypedInferences.TryGetValue(exceptionMapping,out var typedInferenceConflictResource))
+                            {
+                                var aifUri = $"{ExternalReferenceOntologyNamespaceURI}{exceptionMapping}";
+                                var regularInferenceType = new RDFResource(aifUri);
+                                var conflictedTypedInferenceUri = $"{OntologyNamespace}{exceptionMapping}_Conflicted";
+
+                                var hasConflictedTypedInference = new RDFResource(conflictedTypedInferenceUri);
+                                ontology.DeclareClass(hasConflictedTypedInference);
+                                ontology.DeclareQualifiedCardinalityRestriction(hasConflictedTypedInference, hasConflictResource, 1, regularInferenceType);
+
+                                var typedInferenceConflictResourceUri = $"{OntologyNamespace}{exceptionMapping}_Conflict";
+                                typedInferenceConflictResource = new RDFResource(typedInferenceConflictResourceUri);
+
+                                ontology.DeclareClass(typedInferenceConflictResource);
+                                var intersectionList = new List<RDFResource> { conflictResource, hasConflictedTypedInference };
+                                ontology.DeclareIntersectionClass(typedInferenceConflictResource, intersectionList);
+
+                                conflictedTypedInferences[exceptionMapping] = typedInferenceConflictResource;
+                            }
+
+                            mappedConcepts.Add(typedInferenceConflictResource);
+                        }
+
+                        if (mappedConcepts.Count>0)
+                        {
+                            RDFResource mappedConcept = mappedConcepts.First();
+
+                            if (mappedConcepts.Count>1)
+                            {
+                                var fallacyId = GetId(fallacy.TextEn);
+                                var fallacyConflictUri = $"{OntologyNamespace}{fallacyId}_Conflict";
+
+                                RDFResource conflictUnionClass = new RDFResource(fallacyConflictUri);
+                                ontology.DeclareClass(conflictUnionClass);
+                                ontology.DeclareUnionClass(conflictUnionClass, mappedConcepts);
+
+                                mappedConcept = conflictUnionClass;
+                            }
+
+                            switch (fallacy.AIFSkosMappingType)
+                            {
+                                case "skos:exactMatch":
+                                    ontology.DeclareExactMatchConcepts(fallacyConcept, mappedConcept);
+                                    break;
+                                case "skos:closeMatch":
+                                    ontology.DeclareCloseMatchConcepts(fallacyConcept, mappedConcept);
+                                    break;
+                                case "skos:broadMatch":
+                                    ontology.DeclareBroadMatchConcepts(fallacyConcept, mappedConcept);
+                                    break;
+                                case "skos:narrowMatch":
+                                    ontology.DeclareNarrowMatchConcepts(fallacyConcept, mappedConcept);
+                                    break;
+                                case "skos:relatedMatch":
+                                    ontology.DeclareRelatedMatchConcepts(fallacyConcept, mappedConcept);
+                                    break;
+                            }
+                        }
+                    }
+                }
+
+                //Saving
+                // Utiliser la réflexion pour obtenir la valeur de l'enum OWLFormats.Xml
+                Type owlFormatsType = Type.GetType("OWLSharp.OWLEnums+OWLFormats, OWLSharp");
+                object xmlFormat = Enum.Parse(owlFormatsType, "Xml", true);
+                
+                // WRITE OWL2/XML FILE
+                // Utiliser la réflexion pour appeler la méthode ToFile
+                var toFileMethod = ontology.GetType().GetMethod("ToFile");
+                toFileMethod.Invoke(ontology, new object[] { xmlFormat, fileName });
+
+                Logger.LogSuccess($"Owl document {fileName} successfully saved");
             }
 
             private RDFResource GetFallacyConcept(Fallacy targetFallacy,
-                OWLOntology ontology, RDFResource mainScheme)
+            OwlAdapter ontology, RDFResource mainScheme)
             {
-                var fallacyId = GetId(targetFallacy.TextEn);
-                var fallacyUri = $"{OntologyNamespace}{fallacyId}";
-
-                RDFResource fallacyResource = new RDFResource(fallacyUri);
-                ontology.DeclareConcept(fallacyResource, mainScheme);
-
-                ontology.AnnotateConceptPreferredLabel(fallacyResource, new RDFPlainLiteral(targetFallacy.TextFr, "fr"));
-                ontology.AnnotateConceptPreferredLabel(fallacyResource, new RDFPlainLiteral(targetFallacy.TextEn, "en"));
-
-                ontology.DocumentConcept(fallacyResource, SKOSEnums.SKOSDocumentationTypes.Definition, new RDFPlainLiteral(targetFallacy.DescFr, "fr"));
-                ontology.DocumentConcept(fallacyResource, SKOSEnums.SKOSDocumentationTypes.Definition, new RDFPlainLiteral(targetFallacy.DescEn, "en"));
-
-                ontology.DocumentConcept(fallacyResource, SKOSEnums.SKOSDocumentationTypes.Example, new RDFPlainLiteral(targetFallacy.ExampleFr, "fr"));
-                ontology.DocumentConcept(fallacyResource, SKOSEnums.SKOSDocumentationTypes.Example, new RDFPlainLiteral(targetFallacy.ExampleEn, "en"));
-
-
-                if (!string.IsNullOrEmpty(targetFallacy.LinkEn))
-                {
-                    ontology.AnnotateConcept(fallacyResource, RDFVocabulary.RDFS.SEE_ALSO, new RDFPlainLiteral(targetFallacy.LinkEn, "en"));
-                }
-                if (!string.IsNullOrEmpty(targetFallacy.LinkFr))
-                {
-                    ontology.AnnotateConcept(fallacyResource, RDFVocabulary.RDFS.SEE_ALSO, new RDFPlainLiteral(targetFallacy.LinkFr, "fr"));
-                }
-
-
-                return fallacyResource;
-            }
+            var fallacyId = GetId(targetFallacy.TextEn);
+            var fallacyUri = $"{OntologyNamespace}{fallacyId}";
+
+            RDFResource fallacyResource = new RDFResource(fallacyUri);
+            ontology.DeclareConcept(fallacyResource, mainScheme);
+
+            ontology.AnnotateConceptPreferredLabel(fallacyResource, new RDFPlainLiteral(targetFallacy.TextFr, "fr"));
+            ontology.AnnotateConceptPreferredLabel(fallacyResource, new RDFPlainLiteral(targetFallacy.TextEn, "en"));
+            
+            ontology.DocumentConcept(fallacyResource, Ontology.SKOSDocumentationTypes.Definition, new RDFPlainLiteral(targetFallacy.DescFr, "fr"));
+            ontology.DocumentConcept(fallacyResource, Ontology.SKOSDocumentationTypes.Definition, new RDFPlainLiteral(targetFallacy.DescEn, "en"));
+
+            ontology.DocumentConcept(fallacyResource, Ontology.SKOSDocumentationTypes.Example, new RDFPlainLiteral(targetFallacy.ExampleFr, "fr"));
+            ontology.DocumentConcept(fallacyResource, Ontology.SKOSDocumentationTypes.Example, new RDFPlainLiteral(targetFallacy.ExampleEn, "en"));
+
+            if (!string.IsNullOrEmpty(targetFallacy.LinkEn))
+            {
+                ontology.AnnotateConcept(fallacyResource, RDFVocabulary.RDFS.SEE_ALSO, new RDFPlainLiteral(targetFallacy.LinkEn, "en"));
+            }
+            if (!string.IsNullOrEmpty(targetFallacy.LinkFr))
+            {
+                ontology.AnnotateConcept(fallacyResource, RDFVocabulary.RDFS.SEE_ALSO, new RDFPlainLiteral(targetFallacy.LinkFr, "fr"));
+            }
+
+            return fallacyResource;
+        }
         }
 }
```


### Commit `563643570d34d92e10bb36ac872c5b2544432e21`

**Message :** Mise à jour du package OWLSharp de la version 3.11.0 à la version 4.6.1

**Statistiques :**
```
.../Argumentum.AssetConverter/Argumentum.AssetConverter.csproj | 2 +-
1 file changed, 1 insertion(+), 1 deletion(-)
```

**Diff complet :**
```diff
commit 563643570d34d92e10bb36ac872c5b2544432e21
Author: jsboige <jsboige@gmail.com>
Date:   Tue May 13 18:07:50 2025 +0200

    Mise à jour du package OWLSharp de la version 3.11.0 à la version 4.6.1

    Former-commit-id: 4d48924c5dbeea67733df19807414801268489b08
    Former-commit-id: 51767125a0fa2348ab349e6fe502617478e20897

diff --git a/Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj b/Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj
index e9bd434..67a00f0 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj
+++ b/Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj
@@ -20,7 +20,7 @@
     <PackageReference Include="Microsoft.Playwright" Version="1.43.0" />
     <PackageReference Include="Microsoft.XmlSerializer.Generator" Version="8.0.0" />
     <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
-    <PackageReference Include="OWLSharp" Version="3.11.0" />
+    <PackageReference Include="OWLSharp" Version="4.6.1" />
     <PackageReference Include="QuestPDF" Version="2024.3.0" />
     <PackageReference Include="SharpToken" Version="2.0.2" />
     <PackageReference Include="Spectre.Console" Version="0.50.0" />

```


### Commit `3ff3ecfc0ab1ab5e248383a7759e0d38167585a3`

**Message :** Retrait des fichiers binaires du suivi Git conformément au nouveau .gitignore

**Statistiques :**
```
...tityFrameworkCore.Relational.dll.REMOVED.git-id | 1 -
.../Microsoft.EntityFrameworkCore.SqlServer.dll | Bin 1559232 -> 0 bytes
...icrosoft.EntityFrameWorkCore.dll.REMOVED.git-id | 1 -
.../Microsoft.Extensions.Caching.Abstractions.dll | Bin 255600 -> 0 bytes
.../2sxc/Microsoft.Extensions.Caching.Memory.dll | Bin 300200 -> 0 bytes
...Extensions.DependencyInjection.Abstractions.dll | Bin 355320 -> 0 bytes
.../Microsoft.Extensions.DependencyInjection.dll | Bin 455048 -> 0 bytes
.../Microsoft.Extensions.Logging.Abstractions.dll | Bin 444032 -> 0 bytes
.../bin/2sxc/Microsoft.Extensions.Logging.dll | Bin 188432 -> 0 bytes
.../bin/2sxc/Microsoft.Extensions.Options.dll | Bin 222016 -> 0 bytes
.../bin/2sxc/Microsoft.Extensions.Primitives.dll | Bin 299176 -> 0 bytes
DNNPlatform/bin/2sxc/Remotion.Linq.dll | Bin 181248 -> 0 bytes
.../bin/2sxc/System.Collections.Immutable.dll | Bin 180984 -> 0 bytes
.../2sxc/System.Diagnostics.DiagnosticSource.dll | Bin 54032 -> 0 bytes
DNNPlatform/bin/2sxc/System.Interactive.Async.dll | Bin 185600 -> 0 bytes
.../bin/BouncyCastle.Crypto.dll.REMOVED.git-id | 1 -
DNNPlatform/bin/ClientDependency.Core.dll | Bin 158208 -> 0 bytes
DNNPlatform/bin/Connect.Dnn.Koi.dll | Bin 9728 -> 0 bytes
DNNPlatform/bin/Connect.Koi.dll | Bin 13312 -> 0 bytes
DNNPlatform/bin/Connect.Razor.Dnn.dll | Bin 7680 -> 0 bytes
DNNPlatform/bin/Connect.Razor.dll | Bin 27648 -> 0 bytes
DNNPlatform/bin/CountryListBox.dll | Bin 23040 -> 0 bytes
DNNPlatform/bin/CsvHelper.dll | Bin 208384 -> 0 bytes
DNNPlatform/bin/DNN.Connectors.GoogleAnalytics.dll | Bin 9216 -> 0 bytes
.../bin/DNN.Connectors.GoogleTagManager.dll | Bin 10752 -> 0 bytes
DNNPlatform/bin/DNNConnect.CKEditorProvider.dll | Bin 314368 -> 0 bytes
DNNPlatform/bin/Dnn.AzureConnector.dll | Bin 22528 -> 0 bytes
DNNPlatform/bin/Dnn.EditBar.Library.dll | Bin 6656 -> 0 bytes
DNNPlatform/bin/Dnn.EditBar.UI.dll | Bin 71680 -> 0 bytes
DNNPlatform/bin/Dnn.Modules.Console.dll | Bin 24576 -> 0 bytes
DNNPlatform/bin/Dnn.Modules.ModuleCreator.dll | Bin 25600 -> 0 bytes
DNNPlatform/bin/Dnn.Modules.ResourceManager.dll | Bin 98304 -> 0 bytes
DNNPlatform/bin/Dnn.Modules.TelerikRemoval.dll | Bin 12800 -> 0 bytes
.../Dnn.PersonaBar.Extensions.dll.REMOVED.git-id | 1 -
DNNPlatform/bin/Dnn.PersonaBar.Library.dll | Bin 105984 -> 0 bytes
DNNPlatform/bin/Dnn.PersonaBar.UI.dll | Bin 69632 -> 0 bytes
DNNPlatform/bin/DotNetNuke.Abstractions.deps.json | 86 ----
DNNPlatform/bin/DotNetNuke.Abstractions.dll | Bin 35328 -> 0 bytes
.../bin/DotNetNuke.Authentication.Facebook.dll | Bin 9216 -> 0 bytes
.../bin/DotNetNuke.Authentication.Google.dll | Bin 9216 -> 0 bytes
.../bin/DotNetNuke.Authentication.LiveConnect.dll | Bin 9216 -> 0 bytes
.../bin/DotNetNuke.Authentication.Twitter.dll | Bin 9216 -> 0 bytes
.../bin/DotNetNuke.DependencyInjection.deps.json | 144 -------
DNNPlatform/bin/DotNetNuke.DependencyInjection.dll | Bin 7168 -> 0 bytes
DNNPlatform/bin/DotNetNuke.HttpModules.dll | Bin 48128 -> 0 bytes
DNNPlatform/bin/DotNetNuke.HttpModules.dll.config | 11 -
DNNPlatform/bin/DotNetNuke.Instrumentation.dll | Bin 17408 -> 0 bytes
DNNPlatform/bin/DotNetNuke.Maintenance.deps.json | 402 --------------------
DNNPlatform/bin/DotNetNuke.Maintenance.dll | Bin 48128 -> 0 bytes
DNNPlatform/bin/DotNetNuke.ModulePipeline.dll | Bin 8704 -> 0 bytes
.../bin/DotNetNuke.Modules.CoreMessaging.dll | Bin 54784 -> 0 bytes
DNNPlatform/bin/DotNetNuke.Modules.Groups.dll | Bin 61952 -> 0 bytes
DNNPlatform/bin/DotNetNuke.Modules.Html.dll | Bin 55296 -> 0 bytes
.../bin/DotNetNuke.Modules.HtmlEditorManager.dll | Bin 19456 -> 0 bytes
DNNPlatform/bin/DotNetNuke.Modules.Journal.dll | Bin 64512 -> 0 bytes
.../bin/DotNetNuke.Modules.MemberDirectory.dll | Bin 32768 -> 0 bytes
DNNPlatform/bin/DotNetNuke.Modules.RazorHost.dll | Bin 19456 -> 0 bytes
.../bin/DotNetNuke.Services.Syndication.dll | Bin 31744 -> 0 bytes
.../bin/DotNetNuke.SiteExportImport.Library.dll | Bin 78336 -> 0 bytes
DNNPlatform/bin/DotNetNuke.SiteExportImport.dll | Bin 222720 -> 0 bytes
DNNPlatform/bin/DotNetNuke.Web.Client.dll | Bin 41472 -> 0 bytes
DNNPlatform/bin/DotNetNuke.Web.DDRMenu.dll | Bin 88064 -> 0 bytes
DNNPlatform/bin/DotNetNuke.Web.DDRMenu.dll.config | 11 -
DNNPlatform/bin/DotNetNuke.Web.Mvc.dll | Bin 98304 -> 0 bytes
DNNPlatform/bin/DotNetNuke.Web.Razor.dll | Bin 13824 -> 0 bytes
DNNPlatform/bin/DotNetNuke.Web.dll | Bin 423936 -> 0 bytes
DNNPlatform/bin/DotNetNuke.Web.dll.config | 19 -
DNNPlatform/bin/DotNetNuke.WebControls.dll | Bin 283648 -> 0 bytes
DNNPlatform/bin/DotNetNuke.WebUtility.dll | Bin 493056 -> 0 bytes
DNNPlatform/bin/DotNetNuke.Website.dll | Bin 330240 -> 0 bytes
DNNPlatform/bin/DotNetNuke.Website.dll.config | 31 --
DNNPlatform/bin/DotNetNuke.dll.REMOVED.git-id | 1 -
DNNPlatform/bin/DotNetNuke.dll.config | 11 -
DNNPlatform/bin/DotNetNuke.log4net.dll | Bin 258560 -> 0 bytes
DNNPlatform/bin/ICSharpCode.SharpZipLib.dll | Bin 204800 -> 0 bytes
DNNPlatform/bin/ImageResizer.Plugins.DiskCache.dll | Bin 164864 -> 0 bytes
DNNPlatform/bin/ImageResizer.Plugins.WebP.dll | Bin 7680 -> 0 bytes
DNNPlatform/bin/ImageResizer.dll | Bin 314880 -> 0 bytes
DNNPlatform/bin/Imageflow.Net.dll | Bin 183808 -> 0 bytes
...osoft.Extensions.Configuration.Abstractions.dll | Bin 20544 -> 0 bytes
...Extensions.DependencyInjection.Abstractions.dll | Bin 37440 -> 0 bytes
.../Microsoft.Extensions.DependencyInjection.dll | Bin 60480 -> 0 bytes
...osoft.Extensions.FileProviders.Abstractions.dll | Bin 17976 -> 0 bytes
.../Microsoft.Extensions.Hosting.Abstractions.dll | Bin 22904 -> 0 bytes
.../Microsoft.Extensions.Logging.Abstractions.dll | Bin 48192 -> 0 bytes
.../Imageflow/Microsoft.Extensions.Primitives.dll | Bin 36416 -> 0 bytes
.../Microsoft.IO.RecyclableMemoryStream.dll | Bin 41328 -> 0 bytes
DNNPlatform/bin/Imageflow/System.Buffers.dll | Bin 20856 -> 0 bytes
DNNPlatform/bin/Imageflow/System.Memory.dll | Bin 141184 -> 0 bytes
.../bin/Imageflow/System.Numerics.Vectors.dll | Bin 115856 -> 0 bytes
.../System.Runtime.CompilerServices.Unsafe.dll | Bin 16768 -> 0 bytes
DNNPlatform/bin/Imazen.Common.dll | Bin 133120 -> 0 bytes
DNNPlatform/bin/Imazen.HybridCache.dll | Bin 76288 -> 0 bytes
DNNPlatform/bin/Imazen.WebP.dll | Bin 18944 -> 0 bytes
DNNPlatform/bin/LiteDB.dll | Bin 488448 -> 0 bytes
DNNPlatform/bin/Lucene.Net.Contrib.Analyzers.dll | Bin 134144 -> 0 bytes
.../Lucene.Net.Contrib.FastVectorHighlighter.dll | Bin 27136 -> 0 bytes
DNNPlatform/bin/Lucene.Net.dll.REMOVED.git-id | 1 -
DNNPlatform/bin/MailKit.dll.REMOVED.git-id | 1 -
.../bin/Microsoft.ApplicationBlocks.Data.dll | Bin 24064 -> 0 bytes
DNNPlatform/bin/Microsoft.Bcl.AsyncInterfaces.dll | Bin 22144 -> 0 bytes
.../Microsoft.EntityFrameworkCore.Abstractions.dll | Bin 20472 -> 0 bytes
...tityFrameworkCore.Relational.dll.REMOVED.git-id | 1 -
.../Microsoft.EntityFrameworkCore.SqlServer.dll | Bin 231416 -> 0 bytes
...icrosoft.EntityFrameWorkCore.dll.REMOVED.git-id | 1 -
.../Microsoft.Extensions.Caching.Abstractions.dll | Bin 26616 -> 0 bytes
.../bin/Microsoft.Extensions.Caching.Memory.dll | Bin 32248 -> 0 bytes
...osoft.Extensions.Configuration.Abstractions.dll | Bin 20472 -> 0 bytes
.../Microsoft.Extensions.Configuration.Binder.dll | Bin 25080 -> 0 bytes
.../bin/Microsoft.Extensions.Configuration.dll | Bin 25592 -> 0 bytes
...Extensions.DependencyInjection.Abstractions.dll | Bin 36856 -> 0 bytes
.../Microsoft.Extensions.DependencyInjection.dll | Bin 60408 -> 0 bytes
...osoft.Extensions.FileProviders.Abstractions.dll | Bin 17976 -> 0 bytes
.../Microsoft.Extensions.FileSystemGlobbing.dll | Bin 39816 -> 0 bytes
.../Microsoft.Extensions.Hosting.Abstractions.dll | Bin 22904 -> 0 bytes
.../Microsoft.Extensions.Logging.Abstractions.dll | Bin 47632 -> 0 bytes
DNNPlatform/bin/Microsoft.Extensions.Logging.dll | Bin 31736 -> 0 bytes
DNNPlatform/bin/Microsoft.Extensions.Options.dll | Bin 40440 -> 0 bytes
.../bin/Microsoft.Extensions.Primitives.dll | Bin 35832 -> 0 bytes
.../bin/Microsoft.IO.RecyclableMemoryStream.dll | Bin 41328 -> 0 bytes
DNNPlatform/bin/Microsoft.Web.Helpers.dll | Bin 100824 -> 0 bytes
DNNPlatform/bin/Microsoft.Web.Infrastructure.dll | Bin 45416 -> 0 bytes
DNNPlatform/bin/MimeKit.dll.REMOVED.git-id | 1 -
.../bin/NBrightBuy.DiscountCodesProvider.dll | Bin 9216 -> 0 bytes
.../bin/NBrightBuy.ManualPaymentProvider.dll | Bin 11264 -> 0 bytes
DNNPlatform/bin/NBrightBuy.PromoProvider.dll | Bin 30720 -> 0 bytes
DNNPlatform/bin/NBrightBuy.ShippingProvider.dll | Bin 18944 -> 0 bytes
DNNPlatform/bin/NBrightBuy.TaxProvider.dll | Bin 14848 -> 0 bytes
DNNPlatform/bin/NBrightBuy.dll.REMOVED.git-id | 1 -
DNNPlatform/bin/NBrightCore.dll | Bin 147968 -> 0 bytes
DNNPlatform/bin/NBrightDNN.dll | Bin 112640 -> 0 bytes
DNNPlatform/bin/Newtonsoft.Json.dll.REMOVED.git-id | 1 -
DNNPlatform/bin/OS_Chronopost2.dll | Bin 29184 -> 0 bytes
DNNPlatform/bin/OS_Reports.dll | Bin 19456 -> 0 bytes
DNNPlatform/bin/OS_Stripe.dll | Bin 20992 -> 0 bytes
DNNPlatform/bin/PetaPoco.dll | Bin 160256 -> 0 bytes
...ke.Providers.AspNetClientCapabilityProvider.dll | Bin 22016 -> 0 bytes
...viders.Caching.SimpleWebFarmCachingProvider.dll | Bin 10240 -> 0 bytes
.../DotNetNuke.Providers.FolderProviders.dll | Bin 37888 -> 0 bytes
...crosoft.WindowsAzure.Storage.dll.REMOVED.git-id | 1 -
DNNPlatform/bin/RazorEngine.dll | Bin 288256 -> 0 bytes
DNNPlatform/bin/Remotion.Linq.dll | Bin 182784 -> 0 bytes
DNNPlatform/bin/SchwabenCode.QuickIO.dll | Bin 249344 -> 0 bytes
DNNPlatform/bin/Stripe.net.dll.REMOVED.git-id | 1 -
DNNPlatform/bin/System.Buffers.dll | Bin 20856 -> 0 bytes
DNNPlatform/bin/System.Collections.Immutable.dll | Bin 193664 -> 0 bytes
.../bin/System.ComponentModel.Annotations.dll | Bin 43152 -> 0 bytes
DNNPlatform/bin/System.Data.SqlClient.dll | Bin 223680 -> 0 bytes
.../bin/System.Diagnostics.DiagnosticSource.dll | Bin 58504 -> 0 bytes
DNNPlatform/bin/System.Interactive.Async.dll | Bin 236320 -> 0 bytes
DNNPlatform/bin/System.Memory.dll | Bin 141184 -> 0 bytes
DNNPlatform/bin/System.Net.Http.Formatting.dll | Bin 179680 -> 0 bytes
DNNPlatform/bin/System.Numerics.Vectors.dll | Bin 115856 -> 0 bytes
.../bin/System.Runtime.CompilerServices.Unsafe.dll | Bin 18024 -> 0 bytes
DNNPlatform/bin/System.Text.Encodings.Web.dll | Bin 76904 -> 0 bytes
.../bin/System.Text.Json.dll.REMOVED.git-id | 1 -
.../bin/System.Threading.Tasks.Extensions.dll | Bin 25984 -> 0 bytes
DNNPlatform/bin/System.ValueTuple.dll | Bin 25232 -> 0 bytes
DNNPlatform/bin/System.Web.Helpers.dll | Bin 138200 -> 0 bytes
DNNPlatform/bin/System.Web.Http.WebHost.dll | Bin 80360 -> 0 bytes
DNNPlatform/bin/System.Web.Http.dll | Bin 456168 -> 0 bytes
DNNPlatform/bin/System.Web.Mvc.dll.REMOVED.git-id | 1 -
DNNPlatform/bin/System.Web.Razor.dll | Bin 264680 -> 0 bytes
DNNPlatform/bin/System.Web.WebPages.Deployment.dll | Bin 44008 -> 0 bytes
DNNPlatform/bin/System.Web.WebPages.Razor.dll | Bin 41960 -> 0 bytes
DNNPlatform/bin/System.Web.WebPages.dll | Bin 207336 -> 0 bytes
DNNPlatform/bin/ToSic.Eav.Apps.dll | Bin 262656 -> 0 bytes
DNNPlatform/bin/ToSic.Eav.Core.dll | Bin 347136 -> 0 bytes
DNNPlatform/bin/ToSic.Eav.DataSources.dll | Bin 209408 -> 0 bytes
DNNPlatform/bin/ToSic.Eav.ImportExport.dll | Bin 111104 -> 0 bytes
DNNPlatform/bin/ToSic.Eav.Persistence.Efc.dll | Bin 103936 -> 0 bytes
DNNPlatform/bin/ToSic.Eav.Repository.Efc.dll | Bin 103424 -> 0 bytes
DNNPlatform/bin/ToSic.Eav.Tokens.dll | Bin 4096 -> 0 bytes
DNNPlatform/bin/ToSic.Eav.WebApi.dll | Bin 222720 -> 0 bytes
DNNPlatform/bin/ToSic.Eav.dll | Bin 5120 -> 0 bytes
DNNPlatform/bin/ToSic.Imageflow.Dnn.dll | Bin 37376 -> 0 bytes
DNNPlatform/bin/ToSic.Lib.Core.dll | Bin 38912 -> 0 bytes
DNNPlatform/bin/ToSic.Razor.Dnn.dll | Bin 9728 -> 0 bytes
DNNPlatform/bin/ToSic.Razor.dll | Bin 88576 -> 0 bytes
DNNPlatform/bin/ToSic.Sxc.Dnn.Core.dll | Bin 164864 -> 0 bytes
DNNPlatform/bin/ToSic.Sxc.Dnn.Razor.dll | Bin 40448 -> 0 bytes
DNNPlatform/bin/ToSic.Sxc.Dnn.WebApi.dll | Bin 91648 -> 0 bytes
DNNPlatform/bin/ToSic.Sxc.Dnn.dll | Bin 30720 -> 0 bytes
DNNPlatform/bin/ToSic.Sxc.WebApi.dll | Bin 198144 -> 0 bytes
DNNPlatform/bin/ToSic.Sxc.dll.REMOVED.git-id | 1 -
DNNPlatform/bin/WebFormsMvp.dll | Bin 74752 -> 0 bytes
DNNPlatform/bin/WebMatrix.Data.dll | Bin 40936 -> 0 bytes
DNNPlatform/bin/WebMatrix.WebData.dll | Bin 74728 -> 0 bytes
DNNPlatform/bin/libwebp.dll | Bin 486912 -> 0 bytes
.../win-x64/native/imageflow.dll.REMOVED.git-id | 1 -
.../win-x86/native/imageflow.dll.REMOVED.git-id | 1 -
191 files changed, 734 deletions(-)
```

**Diff complet :**
```diff
Le diff est trop volumineux pour être affiché.
```


### Commit `fa9b7992152d69daf3fe22651fd3a5cf3f0c1fc6`

**Message :** Mise à jour de Spectre.Console vers la version 0.50.0 et corrections associées

**Statistiques :**
```
.gitignore | 385 ++------
.../Argumentum.AssetConverter.csproj | 5 +-
.../AssetConverterConfig.cs | 90 +-
.../Argumentum.AssetConverter/ConverterMode.cs | 6 +
.../Documentation/DeveloperGuide.md | 316 ++++++
.../Documentation/GenerateDocumentation.cs | 912 +++++++++++++++++
.../Documentation/MultilingualProcess.md | 167 ++++
.../Documentation/ParallelismOptimization.md | 533 ++++++++++
.../Documentation/README.md | 92 ++
.../Documentation/TranslationCoverage.md | 400 ++++++++
.../Documentation/ValidationSystem.md | 303 ++++++
.../Converters/Argumentum.AssetConverter/Logger.cs | 1 +
.../Ontology/OwlGeneratorConfig.cs | 3 +-
.../Optimization/ParallelismOptimizer.cs | 763 +++++++++++++++
.../Optimization/ParallelismOptimizerConfig.cs | 119 +++
.../Argumentum.AssetConverter/Program.cs | 389 +++++++-
.../Tests/CardGenerationValidationTests.cs | 528 ++++++++++
.../Tests/CardValidatorConfig.cs | 138 +++
.../Tests/ContinuousValidationConfig.cs | 160 +++
.../Tests/ContinuousValidationSystem.cs | 726 ++++++++++++++
.../Tests/OwlOntologyValidationTests.cs | 489 ++++++++++
.../Tests/OwlValidatorConfig.cs | 142 +++
.../Tests/TaxonomyValidationTests.cs | 495 ++++++++++
.../Tests/TaxonomyValidatorConfig.cs | 64 ++
.../Tests/TranslationCoverageConfig.cs | 119 +++
.../Tests/TranslationCoverageReport.cs | 1024 ++++++++++++++++++++
LoggerTest.cs | 51 +
README_UPDATE.md | 64 ++
WriteExceptionTest.cs | 38 +
run_argumentum.bat | 5 +
30 files changed, 8220 insertions(+), 307 deletions(-)
```

**Diff complet :**
```diff
Le diff est trop volumineux pour être affiché.
```

## Phase 3: Analyse de l'État Actuel (Non-commité)

Cette section documente les modifications présentes dans le répertoire de travail qui n'ont pas encore été commitées. Elle complète l'analyse historique des commits.

### Fichiers Modifiés (`git status -s`)

```
 M "Cards/Rules/Argumentum Rules - Cards.csv"
 M Cards/Rules/Argumentum_Rules_fr.json
 M Generation/CardPen/js/main.js
 M Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj
 M Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs
 M Generation/Converters/Argumentum.AssetConverter/ConverterMode.cs
 M Generation/Converters/Argumentum.AssetConverter/Documentation/GenerateDocumentation.cs
 M Generation/Converters/Argumentum.AssetConverter/Logger.cs
 M Generation/Converters/Argumentum.AssetConverter/Program.cs
 M Generation/Converters/Argumentum.AssetConverter/Tests/CardGenerationValidationTests.cs
 M Generation/Converters/Argumentum.AssetConverter/Tests/ContinuousValidationSystem.cs
 M Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/CardSetConfig.cs
 M Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs
 M Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/DataSetInfo.cs
 M Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/ImageFileGenerator.cs
 M Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs
 M Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGenerator.cs
 M Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGeneratorConfig.cs
?? Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.json
?? Generation/Converters/Argumentum.AssetConverter/PdfAuditor/
?? Generation/Documentation/
?? Git_Archeology_Report.md
?? Logs/
?? Recovery_Plan.md
```

### Diff Complet des Modifications Actuelles (`git diff HEAD`)

```diff
diff --git a/Cards/Rules/Argumentum Rules - Cards.csv b/Cards/Rules/Argumentum Rules - Cards.csv
index 11a7c81f..30966fa5 100644
--- a/Cards/Rules/Argumentum Rules - Cards.csv
+++ b/Cards/Rules/Argumentum Rules - Cards.csv
@@ -1,1007 +1,6 @@
-Text,Text_en,Text_ru,Text_pt,print_and_play
-"# Argumentum
-## L'école des menteurs","# Argumentum
-## The school of liars","# Argumentum
-## Школа лжецов","# Argumentum
-## Liars 'School",
-"*Règles du jeu : de 4 à 8 joueurs*
-
-## Matériel
-
-* 1 paquet de cartes d’argument fallacieux organisées en 7 classes de couleurs,  chacune répartie en 3 ordres puis en 3 familles.
-* 1 paquet cartes de scénario organisés en 7 thèmes identifiables sur leur dos
-* 5 cartes mémo
-* Règles de jeu
-
-## Résumé du jeu
-
-Les joueurs vont à tour de rôle essayer de faire deviner une carte d’argument fallacieux à une majorité d’autres joueurs, au cours d’une saynète d’improvisation à partir d'un scénario tiré au hasard. Tout en s’amusant, les joueurs apprennent ainsi à reconnaître, à décoder et donc à déconstruire les arguments fallacieux.
-
-Argumentum s’appuie sur une classification de ces arguments fallacieux, rangés en ordres et en familles qui sont indiqués en haut de la carte. Par exemple, flatterie est une sous catégorie de l’appel aux émotions. Les cartes mémo résument cette classification.","*Rules of the game: 4 to 8 players*
-
-## Game material
-
-*   1 deck of fallacy cards organized in 7 color classes, order and families.
-*   1 deck of scenario cards organized in 7 families identifiable on their back
-*   5 memo cards
-*   Game rules
-
-## Game summary
-
-Players will take turns trying to get a majority of other players to guess a spurious argument card during an improvisational skit based on a randomly drawn scenario. While having fun, players learn to recognize, decode and therefore deconstruct fallacious arguments.
-
-Argumentum is based on a classification of fallacious arguments, arranged in orders and families that are indicated at the top of the cards. For example, flattery is a subcategory of the appeal to emotions. Memo cards summarize this classification.","*Правила игры: от 4 до 8 игроков*
-
-## В комплект входит:
-
-* 1 колода карт с псевдоаргументами, разделенными на 7 подгрупп по цветам. Каждая подгруппа делится на 3 класса, а те - на 3 семьи.
-* 1 колода карт со сценариями, разделенными на 7 тем.
-* 5 карт Мемо
-* Правила игры
-
-## Краткое описание игры
-
-Игроки по очереди импровизируют ситуацию по случайной карте сценария, и пытаются сделать так, чтобы большинство угадало, какой псевдоаргумент был использован. Развлекаясь, игроки учатся распознавать, декодировать и, следовательно, обезвреживать ошибочные аргументы.
-
-Игра основана на классификации ошибочных аргументов, расположенных по подгруппам, классам и семьям, которые указаны в верхней части карты. Например, ""Лесть"" - это субкатегория вызова эмоций. Карты Мемо напоминают об этой классификации.","*Regras de jogo: de 4 a 8 jogadores*
-
-## material
-
-* 1 pacote de cartões falaciosos organizados em 7 classes de cores, cada uma dividida em 3 ordens e depois em 3 famílias.
-* 1 pacotes de pacotes organizados em 7 temas identificáveis ​​nas costas
-* 5 cartões de memorando
-* Regras do jogo
-
-## Resumo do jogo
-
-Os jogadores, por sua vez, tentarão adivinhar um cartão de argumento falacioso com a maioria dos outros jogadores, durante um SayNet de improvisação de um cenário aleatório. Enquanto se divertem, os jogadores aprendem a reconhecer, decodificar e, portanto, desconstruir os argumentos falaciosos.
-
-Argumentum é baseado na classificação desses argumentos falaciosos, organizados em ordens e famílias que são indicadas no topo do mapa. Por exemplo, a Flatterie é uma sub -categoria da chamada para emoções. Os cartões de memorando resumem essa classificação.",
-"## Installation
-
-Selon le nombre de joueurs et le niveau de difficulté voulu, on constitue la pioche des cartes d’arguments fallacieux en ajoutant les niveaux indiqués en bas à droite des cartes par ordre croissant. Il en faut au minimum 7 x le nombre de joueurs.
-
-On choisit la durée de la partie. Celle-ci comprend une succession de manches de 7 minutes environ. A l’issue du temps imparti, on finit la manche en cours et le joueur qui a le plus de points remporte la partie. Les cartes d’arguments fallacieux sont mélangées, ainsi que les cartes scénario.
-
-On constitue 2 pioches de cartes scénario placées au milieu de la table. On distribue à chaque joueur 5 cartes d’arguments fallacieux qu’il garde for lui. Le reste des cartes constitue la réserve.
-
-Chaque joueur se munit d’un petit objet unique (pièce de monnaie, haricot…) pour le vote. Le premier piocheur est celui qui se rappelle en premier d'un argument trompeur.","## Installation
-
-Depending on the number of players and the desired level of difficulty, the deck of fallacy cards is built up by adding the levels shown at the bottom right of the cards in ascending order. A minimum of 7 cards per player is required.
-
-Choose the duration of the game. This includes a succession of rounds of about 7 minutes. At the end of the defined time, finish the current round and the player who has the most points wins the game.
-
-Fallacy cards are shuffled, as well as scenario cards. Two piles of scenario cards are made up and placed in the middle of the table. Each player is dealt 5 fallacy cards which they keep for themselves. The rest of the cards make up the draw pile.
-
-The first drawer is drawn at random. Each player has a small unique object (coin, bean...) for voting.","
-## Начало игры
-
-В зависимости от количества игроков и желаемого уровня сложности, мы раздаем карты ошибочных аргументов, минимум по 7 на игрока. Справа внизу на картах обозначен уровень сложности, вы можете выбирать тот, что нужен вам.
-
-Далее устанавливаем продолжительность игры. Игра состоит из последовательности раундов продолжительностью около 7 минут. По окончании этого времени мы заканчиваем текущий раунд, и игрок, у которого больше всего очков, выигрывает игру.
-
-Карты с псевдоаргументами перетасовываем, также и карты сценариев.
-
-Мы делаем 2 стопки карт со сценариями, размещая их в середине стола. Каждый игрок получает 5 карт с ошибочными аргументами, которые он держит при себе. Остальная часть карт составляет резерв.
-
-У каждого игрока есть своя фишка (монета, колечко и т. п.) для голосования. Очко получает тот, кто первым вспомнит название псевдоаргумента.
-","## Instalação
-
-Dependendo do número de jogadores e do nível desejado de dificuldade, constituímos o empate dos argumentos falaciosos, adicionando os níveis indicados no canto inferior direito das cartas em ordem crescente. Leva pelo menos 7 x o número de jogadores.
-
-Escolhemos a duração do jogo. Isso inclui uma sucessão de rodadas de cerca de 7 minutos. No final do tempo alocado, terminamos a rodada atual e o jogador que tem mais pontos vence o jogo. Os argumentos falaciosos são misturados, bem como os cartões de cenário.
-
-Constituímos 2 desenhos de cartões de cartão de cenário colocados no meio da mesa. Cada jogador é distribuído a 5 argumentos falaciosos que ele mantém para ele. O restante dos cartões constitui a reserva.
-
-Cada jogador tem um pequeno objeto único (moeda, feijão, etc.) para a votação. O primeiro picador é quem primeiro se lembra de um argumento enganoso.",
-"## Déroulé de la manche
-
-### 1.       Le piocheur
-
-Le piocheur tire une carte de scénario parmi les deux pioches disponibles et la lit à voix haute, à l’exception de la dernière phrase en italique au bas de la carte.
-
-### 2.       Le baratineur
-
-Le baratineur sera chargé de conduire l’argumentation du scenario, pour illustrer l’une de ses cartes d’argument fallacieux. Son objectif sera de faire deviner cette carte à une majorité d’autres joueurs.
-
-Le rôle de baratineur est attribué au premier joueur qui pose, face cachée, la carte d’argument fallacieux qu’il compte jouer. A défaut, le joueur à la gauche du piocheur est désigné baratineur et pose sa carte.
-
-### 3.       La saynète
-
-Le piocheur amorce le débat comme il le souhaite. Il peut reprendre la suggestion de première réplique en italique au bas de la carte scénario. Le baratineur et le piocheur disposent alors d’une minute maximum pour improviser une discussion durant laquelle le baratineur va tâcher d’utiliser le type d’argument indiqué par la carte qu’il a choisie. ","## Course of a round
-
-### 1.       The drawer
-
-The drawer draws a scenario card from the two available piles and reads it aloud, except for the last sentence in italics at the bottom of the card.
-
-### 2.       The smooth talker
-
-The Smooth talker will be in charge of leading the argumentation of the scenario, to illustrate one of his fallacious argument cards. His objective will be to make a majority of other players guess this card, without everyone discovering it.
-
-The role of the Smooth talker is assigned to the first player who lays, face down, the fallacy card he intends to play. Otherwise, the player to the left of the drawer is designated as the Smooth talker and lays down his card.
-
-### 3.       The skit
-
-The player who drew the scenario card starts the debate as he wishes. He can use the suggestion for a first line in italics at the bottom of the scenario card. The Smooth talker and the drawer then have a maximum of one minute to improvise a discussion during which the Smooth talker will try to use the type of argument indicated by the card he has chosen.
-
-### 4.       The vote
-
-At the end of the skit, each player, except the Smooth talker, secretly places his unique object in his hand. He will have to guess which fallacy card the Smooth talker was trying to make guess. All the other players then try to guess which fallacy card the Smooth talker was trying to make guess. On the count of three, all players who have a guess reveal their choice by placing their object under the player's card.
-
-### 5.       Scoring
-
-*   Each player who has guessed the correct fallacy card wins 1 point, and his card is placed face up in front of him. He draws another card to complete his hand.
-*   The Smooth talker wins 1 point for each player who has guessed his fallacy card correctly. He draws another card to complete his hand.
-*   The player who drew the scenario card wins 1 point.
-
-*   If no player has guessed the correct fallacy card, the drawer wins 1 point, and the Smooth talker gets a ""Liar's Point"", materialized by a face-down card... whose bad reputation will follow him until the end of the game!
-
-## End of a round
-
-At the end of the round, the player to the left of the drawer becomes the new drawer. A new round begins...",,"## Ход манша
-
-### 1. Берущий карту
-
-Берущий карту достает карту сценария из двух доступных колод и читает ее вслух, за исключением последней фразы, выделенной курсивом в нижней части карты.
-
-### 2. Льстец
-
-Льстец будет отвечать за аргументацию сценария, чтобы проиллюстрировать одну из своих карт с псевдоаргументами. Его цель - заставить большинство других игроков угадать эту карту.
-
-Роль льстеца отводится первому игроку, который кладет лицом вниз карту с псевдоаргументом, которую он собирается играть. В противном случае, игрок слева от берущего карту назначается льстецом и кладет свою карту.
-
-### 3. Сценка
-
-Берущий карту начинает дебаты по своему усмотрению. Он может использовать предложенную в курсиве реплику в нижней части карты сценария. У льстеца и берущего карту есть максимум одна минута для импровизированной дискуссии, во время которой льстец попытается использовать тип аргумента, указанный на карте, которую он выбрал. ","## Curso da manga
-
-### 1.       A gaveta
-
-A gaveta desenha um cartão de cenário dos dois empates disponíveis e a lê em voz alta, exceto pela última frase em itálico na parte inferior do cartão.
-
-### 2.       O baratineur
-
-O Baratineur será responsável por conduzir a discussão do cenário, para ilustrar um de seus cartões de argumento falaciosos. Seu objetivo será fazer com que a maioria dos outros jogadores adivinhe esta carta.
-
-O papel de Baratineur é atribuído ao primeiro jogador que coloca, virado para baixo, o cartão de argumento falacioso que ele conta. Caso contrário, o jogador à esquerda da gaveta é designado Baratineur e coloca sua carta.
-
-### 3.       O ditado
-
-A gaveta inicia o debate como desejar. Ele pode assumir a sugestão da primeira réplica em itálico na parte inferior da placa de cenário. O Baratineur e a gaveta têm no máximo um minuto para improvisar uma discussão durante a qual o Baratineur tentará usar o tipo de argumento indicado pelo cartão que ele escolheu. ",
diff --git a/Cards/Rules/Argumentum_Rules_fr.json b/Cards/Rules/Argumentum_Rules_fr.json
index 541c881c..a979116e 100644
--- a/Cards/Rules/Argumentum_Rules_fr.json
+++ b/Cards/Rules/Argumentum_Rules_fr.json
@@ -25,7 +25,7 @@
   "oopa": 1,
   "oURL": "",
   "extCSS": "https://fonts.googleapis.com/css2?family=Bebas+Neue&family=PT+Sans:wght@700&display=swap",
-  "css": ".insuffisance {\n  --color-background: #811da3;\n  --color-text-1: #601362;\n  --color-text-2: #8f5991;\n  --color-text-3: #a173a2;\n}\n\n.influence {\n  --color-background: #ff66eb;\n  --color-text-1: #b3009b;\n  --color-text-2: #cc00b1;\n  --color-text-3: #e566d4;\n}\n\n.erreurMathématique {\n  --color-background: #08af93;\n  --color-text-1: #14555b;\n  --color-text-2: #5a888c;\n  --color-text-3: #749a9e;\n}\n\n.paralogisme {\n  --color-background: #8dc801;\n  --color-text-1: #476205;\n  --color-text-2: #7e9150;\n  --color-text-3: #92a26b;\n}\n\n.détournementDeLaLangue {\n  --color-background: #0054a4;\n  --color-text-1: #0c2861;\n  --color-text-2: #546890;\n  --color-text-3: #6f80a1;\n}\n\n.tricherie {\n--color-background: #ffc307ff;\n  --color-text-1: #9e7800ff;\n  --color-text-2: #c49500ff;\n  --color-text-3: #d6b755ff;\n}\n\n.obstruction {\n  --color-background: #dc0f0a;\n  --color-text-1: #960a07;\n  --color-text-2: #b55351;\n  --color-text-3: #c16e6c;\n}\n\nbody {\n  font-family: 'PT Sans', sans-serif;\n  font-size: 100%;\n  line-height:1.25;\n}\n\ncard h1 {\n  font-size: 2.5em;\n  font-weight: 700;\n  margin-bottom: 0.5em;\n\n}\n\n\ncard h2 {\n  font-size: 1.5em;\n    font-weight: 700;\n  margin-bottom: 0.5em;\n\n}\n\ncard h3 {\n    font-size: 1.2em;\n    font-weight: 700;\n  margin-bottom: 0.5em;\n}\n\nul {\n    display: block;\n    list-style-type: disc;\n    margin-top: 1em;\n    margin-bottom: 1 em;\n    margin-left: 0;\n    margin-right: 0;\n    padding-left: 40px;\n}\n\n\n",
+  "css": ".insuffisance {\n  --color-background: #811da3;\n  --color-text-1: #601362;\n  --color-text-2: #8f5991;\n  --color-text-3: #a173a2;\n}\n\n.influence {\n  --color-background: #ff66eb;\n  --color-text-1: #b3009b;\n  --color-text-2: #cc00b1;\n  --color-text-3: #e566d4;\n}\n\n.erreurMathématique {\n  --color-background: #08af93;\n  --color-text-1: #14555b;\n  --color-text-2: #5a888c;\n  --color-text-3: #749a9e;\n}\n\n.paralogisme {\n  --color-background: #8dc801;\n  --color-text-1: #476205;\n  --color-text-2: #7e9150;\n  --color-text-3: #92a26b;\n}\n\n.détournementDeLaLangue {\n  --color-background: #0054a4;\n  --color-text-1: #0c2861;\n  --color-text-2: #546890;\n  --color-text-3: #6f80a1;\n}\n\n.tricherie {\n--color-background: #ffc307ff;\n  --color-text-1: #9e7800ff;\n  --color-text-2: #c49500ff;\n  --color-text-3: #d6b755ff;\n}\n\n.obstruction {\n  --color-background: #dc0f0a;\n  --color-text-1: #960a07;\n  --color-text-2: #b55351;\n  --color-text-3: #c16e6c;\n}\n\nbody {\n  font-family: 'PT Sans', sans-serif;\n  font-size: 100%;\n  line-height:1.25;\n}\n\ncard h1 {\n  font-size: 1.5em;\n  font-weight: 700;\n  margin-bottom: 0.5em;\n\n}\n\n\ncard h2 {\n  font-size: 1.2em;\n    font-weight: 700;\n  margin-bottom: 0.5em;\n\n}\n\ncard h3 {\n    font-size: 1em;\n    font-weight: 700;\n  margin-bottom: 0.5em;\n}\n\nul {\n    display: block;\n    list-style-type: disc;\n    margin-top: 1em;\n    margin-bottom: 1 em;\n    margin-left: 0;\n    margin-right: 0;\n    padding-left: 40px;\n}\n\n\n",
   "csv": "Text\n\"# Argumentum : L'école des menteurs\r\n\r\n*Règles du jeu : de 4 à 8 joueurs*\r\n\r\n## Matériel\r\n\r\n*   1 paquet de cartes d’argument fallacieux organisées en 7 classes de couleurs, chacune répartie en 3 ordres puis en 3 familles.\r\n*   1 paquet cartes de scénario organisés en 7 thèmes identifiables sur leur dos\r\n*   5 cartes mémo\r\n*   Règles de jeu\r\n\r\n## Résumé du jeu\r\n\r\nLes joueurs vont à tour de rôle essayer de faire deviner une carte d’argument fallacieux à une majorité d’autres joueurs, au cours d’une saynète d’improvisation à partir d'un scénario tiré au hasard. Tout en s’amusant, les joueurs apprennent ainsi à reconnaître, à décoder et donc à déconstruire les arguments fallacieux.\r\n\r\nArgumentum s’appuie sur une classification de ces arguments fallacieux, rangés en ordres et en familles qui sont indiqués en haut de la carte. Par exemple, flatterie est une sous catégorie de l’appel aux émotions. Les cartes mémo résument cette classification.\"\n\"## Installation\r\n\r\nSelon le nombre de joueurs et le niveau de difficulté voulu, on constitue la pioche des cartes d’arguments fallacieux en ajoutant les niveaux indiqués en bas à droite des cartes par ordre croissant. Il en faut au minimum 7 x le nombre de joueurs.\r\n\r\nOn choisit la durée de la partie. Celle-ci comprend une succession de manches de 7 minutes environ. A l’issue du temps imparti, on finit la manche en cours et le joueur qui a le plus de points remporte la partie. Les cartes d’arguments fallacieux sont mélangées, ainsi que les cartes scénario.\r\n\r\nOn constitue 2 pioches de cartes scénario placées au milieu de la table. On distribue à chaque joueur 5 cartes d’arguments fallacieux qu’il garde pour lui. Le reste des cartes constitue la réserve.\r\n\r\nChaque joueur se munit d’un petit objet unique (pièce de monnaie, haricot…) pour le vote. Le premier piocheur est celui qui se rappelle en premier d'un argument trompeur.\"\n\"## Déroulé de la manche\r\n\r\n### 1. Le piocheur\r\n\r\nLe piocheur tire une carte de scénario parmi les deux pioches disponibles et la lit à voix haute, à l’exception de la dernière phrase en italique au bas de la carte.\r\n\r\n### 2. Le baratineur\r\n\r\nLe baratineur sera chargé de conduire l’argumentation du scenario, pour illustrer l’une de ses cartes d’argument fallacieux. Son objectif sera de faire deviner cette carte à une majorité d’autres joueurs.\r\n\r\nLe rôle de baratineur est attribué au premier joueur qui pose, face cachée, la carte d’argument fallacieux qu’il compte jouer. A défaut, le joueur à la gauche du piocheur est désigné baratineur et pose sa carte.\r\n\r\n### 3. La saynète\r\n\r\nLe piocheur amorce le débat comme il le souhaite. Il peut reprendre la suggestion de première réplique en italique au bas de la carte scénario. Le baratineuer et le piocheur disposent alors d’une minute maximum pour improviser une discussion durant laquelle le baratineur va tâcher d’utiliser le type d’argument indiqué par la carte qu’il a choisie. \r\n\r\n### 4. Le vote\r\n\r\nA l’issue de la saynète, chaque joueur, sauf le baratineur, place secrètement dans sa main son petit objet unique. Il va devoir désigner parmi les cartes qu’il a en main la carte d’argument fallacieux que le baratineur a tenté de faire deviner. Tous les autres joueurs essayent alors de deviner quelle était la carte d’argument fallacieux du baratineur. Au décompte de trois, tous les joueurs qui ont une proposition révèlent leur choix en plaçant leur objet sous la carte du joueur concerné.\r\n\r\n### 5. Décompte des points\r\n\r\n*   Chaque joueur qui a désigné la bonne carte d’argument fallacieux gagne 1 point, et sa carte est placée, face visible, devant lui. il pioche une autre carte pour compléter sa main.\r\n*   le baratineur gagne 1 point par joueur qui a désigné sa carte d’argument fallacieux\r\n*   le piocheur gagne 1 point.\r\n\r\nSi aucun joueur n’a désigné la bonne carte d’argument fallacieux, le piocheur gagne 1 point, et le baratineur écôpe d’un « Point Menteur », matérialisé par une carte retournée… dont la mauvaise réputation le suivra jusqu’à la fin de la partie !\r\n\r\n## Fin de manche\r\n\r\nA l’issue du décompte, le joueur à la gauche du piocheur devient le nouveau piocheur. Une nouvelle manche commence…\"\n",
   "mustache": "{{{Text}}}",
   "useMustache": true,
   "cardClass": "",
diff --git a/Generation/CardPen/js/main.js b/Generation/CardPen/js/main.js
index 927b2330..ac8aaa31 100644
--- a/Generation/CardPen/js/main.js
+++ b/Generation/CardPen/js/main.js
@@ -1,4 +1,4 @@
-// Card Pen - a simple tool to lay out cards for printing
+// Card Pen - a simple tool to lay out cards for printing
 const cardpen = {};
 
 cardpen.DOM = {
@@ -7,7 +7,7 @@
 	pageContainer: document.querySelector('.cardpenPageContainer')
 };
 
-// Default settings
+// Default settings
 cardpen.defaults = {
 	name: 'My Card Set', // Card Set Name
 	notes: '', // Notes
@@ -35,7 +35,7 @@
 	oURL: '', // URL for overlay image
 	extCSS: '', // URL for external CSS
 	css: '', // Custom CSS rules
-	csv: '', // CSV data
+	csv: '', // CSV data
 	mustache: '', // Mustache template
 	useMustache: false, // Use mustache or not
 	cardClass: '', // Body class for each card
@@ -43,10 +43,10 @@
 	rsstyle: 'bunch', // Rowset style: bunch means all cards in a row get the whole rowset; each means each card gets one row of the rowset
 	cindices: '', // Start and end indices of cards to print
 	layout: false,
-	cb: 0
+	cb: 0,
 };
 
-// Current applied settings
+// Current applied settings
 cardpen.settings = {};
 cardpen.settings = { ...cardpen.defaults
 };
@@ -55,7 +55,7 @@
 	console.log('Got settings from ', source);
 	Object.keys(cardpen.defaults).forEach(key => {
 		let val = source[key];
-		if (val) {
+		if(val) {
 			// If the value is an object, copy it
 			if (typeof val === 'object' && val !== null) {
 				cardpen.settings[key] = { ...val
@@ -69,12 +69,12 @@
 
 cardpen.setDefaults = function() {
 	console.log('Setting defaults');
-	// Get setting from query string
+	// Get setting from query string
 	const urlParams = new URLSearchParams(window.location.search);
 	const settingsStr = urlParams.get('settings');
-	if (settingsStr) {
+	if(settingsStr) {
 		try {
-			// Try to parse as JSON object
+			// Try to parse as JSON object
 			const settings = JSON.parse(settingsStr);
 			cardpen.utils.applySettings(settings);
 		} catch (e) {
@@ -99,14 +99,14 @@
 
 
 
-	cardpen.DOM.message.textContent = 'Welcome to Card Pen!';
+	cardpen.DOM.message.textContent = 'Welcome to Card Pen!';
 	cardpen.setDefaults();
 
-	// Set up example data
+	// Set up example data
 	const examples = document.querySelectorAll('.cardpenExample');
 	examples.forEach(example => {
 		example.addEventListener('click', e => {
-			// Prevent default link behavior
+			// Prevent default link behavior
 			e.preventDefault();
 			const url = e.target.href;
 			fetch(url)
@@ -118,22 +118,22 @@
 					else cardpen.utils.applySettings(json);
 				})
 				.catch(error => {
-					// Handle any fetch errors
+					// Handle any fetch errors
 					console.error('Fetch error:', error);
 				});
 		});
 	});
 
-	// Set up UI controls
+	// Set up UI controls
 	cardpen.controls.layout = document.querySelector('#cardpenLayout');
 	cardpen.controls.layout.addEventListener('click', e => {
 		cardpen.settings.layout = true;
-		cardpen.build();
+		cardpen.build(true);
 	});
 
 	cardpen.controls.select = document.querySelector('#cardpenFile');
 	cardpen.controls.select.addEventListener('change', e => {
-		// Get the selected file
+		// Get the selected file
 		const file = e.target.files[0];
 		if (!file) {
 			return;
@@ -194,7 +194,7 @@
 
 			cardpen.DOM.pageContainer.appendChild(bleed);
 
-			// Add card count to bleed
+			// Add card count to bleed
 			const cardCount = document.createElement('div');
 			cardCount.classList.add('cardpenCardCount');
 			cardCount.textContent = i + 1;
@@ -203,7 +203,7 @@
 	};
 
 	// Add styles to head
-	function addStyles() {
+	function addStyles() {
 		console.log('Adding styles');
 		const dpi = cardpen.settings.dpi;
 		const psize = cardpen.settings.psize;
@@ -237,7 +237,7 @@
 		let style = document.querySelector('#cardpenStyle');
 		if (!style) {
 			style = document.createElement('style');
-			style.id = 'cardpenStyle';
+			style.id = 'cardpenStyle'
 			// Add to head
 			document.head.appendChild(style);
 		}
@@ -250,7 +250,7 @@
 		cardpenSheet.textContent += cardpen.settings.css;
 	};
 
-	function createPage() {
+	function createPage() {
 		console.log('Creating page');
 		const page = document.createElement('div');
 		page.classList.add('cardpenPage');
@@ -285,16 +285,16 @@
 			// Set up Papa Parse config
 			var config = {
 				header: true,
-				skipEmptyLines: true
+				skipEmptyLines: true,
 			};
 
-			// Parse CSV
+			// Parse CSV
 			var results = Papa.parse(cardpen.settings.csv, config);
 			data = results.data;
 			console.log("CSV data", data);
 
 			if (cardpen.settings.rsstyle === 'bunch') {
-				// Bunch all records into a single rowset
+				// Bunch all records into a single rowset
 				const bunch = {};
 				bunch.rowset = data;
 				data = [bunch];
@@ -302,7 +302,7 @@
 			}
 
 			if (cardpen.settings.useMustache === false || cardpen.settings.mustache === '') {
-				// Create a simple table
+				// Create a simple table
 				data.forEach((row, i) => {
 					const card = document.createElement('card');
 					card.innerHTML = `<h1>Card ${i + 1}</h1>`;
@@ -313,11 +313,11 @@
 					card.innerHTML += table;
 					setCard(i, card.outerHTML);
 				});
-			} else {
+			} else {
 				if (!cardpen.settings.cindices) {
 					data.forEach((row, i) => {
 						const card = document.createElement('card');
-						// Render with Mustache
+						// Render with Mustache
 						row.cardId = i;
 						const output = Mustache.render(cardpen.settings.mustache, row);
 						card.innerHTML = output;
@@ -330,7 +330,7 @@
 					for (let i = start; i <= end; i++) {
 						const card = document.createElement('card');
 						const row = data[i];
-						// Render with Mustache
+						// Render with Mustache
 						const output = Mustache.render(cardpen.settings.mustache, row);
 						card.innerHTML = output;
 						setCard(i, card.outerHTML);
@@ -406,12 +406,12 @@
 		console.log('Message: ' + msg);
 		cardpen.DOM.message.classList.remove('cardpenHide');
 		cardpen.DOM.message.textContent = msg;
-		// If layout is complete, add a print button after a delay
+		// If layout is complete, add a print button after a delay
 		if (cardpen.settings.layout) {
 			setTimeout(() => {
 				cardpen.DOM.message.innerHTML += ' <button onclick="window.print()">Print</button>';
 			}, 2000);
-		}
+		}
 	};
 
 	cardpen.utils.message('Welcome to Card Pen!');
diff --git a/Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj b/Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj
index d652750e..d4086e33 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj
+++ b/Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj
@@ -2,12 +2,12 @@
 
   <PropertyGroup>
     <OutputType>Exe</OutputType>
-    <TargetFramework>net8.0</TargetFramework>
+    <TargetFramework>net7.0</TargetFramework>
     <DockerDefaultTargetOS>Linux</DockerDefaultTargetOS>
   </PropertyGroup>
 
   <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
-    <DefineConstants>TRACE</DefineConstants>
+    <DefineConstants>TRACE;</DefineConstants>
     <Optimize>False</Optimize>
   </PropertyGroup>
 
@@ -52,7 +52,7 @@
   <ItemGroup>
     <PackageReference Include="BuildWebCompiler" Version="1.12.394" />
     <PackageReference Include="CsvHelper" Version="32.0.3" />
-    <PackageReference Include="ExtendedXmlSerializer" Version="3.8.3" />
+    <PackageReference Include="ExtendedXmlSerializer" Version="4.0.0" />
     <PackageReference Include="HtmlAgilityPack" Version="1.11.60" />
     <PackageReference Include="HtmlToOpenXml.dll" Version="2.3.0" />
     <PackageReference Include=" spectre.console" Version="0.50.0" />
@@ -62,7 +62,7 @@
     <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
     <PackageReferenceToreplace Include="System.Text.Json" Version="8.0.3" />
     <PackageReference Include="PlaywrightSharp" Version="0.191.0" />
-    <PackageReference Include="QuestPDF" Version="2023.12.0" />
+    <PackageReference Include="QuestPDF" Version="2022.12.1" />
     <PackageReference Include="morelinq" Version="4.2.0" />
     <PackageReference Include="dotliquid" Version="2.2.631" />
     <PackageReference Include="System.Drawing.Common" Version="8.0.4" />
diff --git a/Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs b/Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs
index a52718e4..48c1050a 100644
--- a/Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs
@@ -48,7 +48,7 @@
 
     public class AssetConverterConfig
     {
-        public ConverterMode Mode { get; set; } = ConverterMode.WebBasedImageGeneration | ConverterMode.Mindmapper | ConverterMode.OwlGenerator; // | ConverterMode.WebBasedImageGeneration; // ConverterMode.DatasetUpdater;
+        public ConverterMode Mode { get; set; } = ConverterMode.None; //  ConverterMode.WebBasedImageGeneration | ConverterMode.Mindmapper | ConverterMode.OwlGenerator; // | ConverterMode.WebBasedImageGeneration; // ConverterMode.DatasetUpdater;
         public bool ForceDebugParams { get; set; }
 
         public bool ForceReleaseParams { get; set; }
@@ -58,9 +58,9 @@
         public TaxonomyValidatorConfig TaxonomyValidatorConfig { get; set; }
 
         public OwlValidatorConfig OwlValidatorConfig { get; set; }
-        public BatchImageConverterConfig BatchImageConverterConfig { get; set; } = new BatchImageConverterConfig();
+        public BatchImageConverterConfig BatchImageConverterConfig { get; set; } = new();
         public DatasetUpdaterConfig DatasetUpdaterConfig { get; set; } = new();
-        public Dnn2sxcConfig Dnn2sxcConfig { get; set; } = new Dnn2sxcConfig();
+        public Dnn2sxcConfig Dnn2sxcConfig { get; set; } = new();
         public MindMapCreatorConfig MindMapCreatorConfig { get; set; } = new();
         public ContinuousValidationConfig ContinuousValidationConfig { get; set; } = new();
         public OwlGeneratorConfig OwlGeneratorConfig { get; set; } = new();
@@ -75,6 +75,7 @@
 
         public static AssetConverterConfig GetConfig(string configFileName, out string json)
         {
+
             AssetConverterConfig config;
             if (File.Exists(configFileName))
             {
@@ -95,8 +96,8 @@
             return config;
         }
 
-        JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
-        {
-            ReferenceHandler = ReferenceHandler.Preserve,
-            WriteIndented = true,
-            PropertyNamingPolicy = new CustomNamingPolicy()
-        };
+        private static readonly JsonSerializerOptions jsonOptions = new() {
+            ReferenceHandler = ReferenceHandler.Preserve, 
+            WriteIndented = true, 
+            PropertyNamingPolicy = new CustomNamingPolicy()};
+
 
         public async Task Apply(string[] args)
         {
@@ -107,6 +108,7 @@
                 await this.WebBasedGeneratorConfig.Run(this).ConfigureAwait(false);
             }
 
+
             if (Mode.HasFlag(ConverterMode.Mindmapper))
             {
                 await new MindMapCreator(this).CreateAllMaps().ConfigureAwait(false);
diff --git a/Generation/Converters/Argumentum.AssetConverter/ConverterMode.cs b/Generation/Converters/Argumentum.AssetConverter/ConverterMode.cs
index d7a17725..e229e62f 100644
--- a/Generation/Converters/Argumentum.AssetConverter/ConverterMode.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/ConverterMode.cs
@@ -5,14 +5,15 @@
     [Flags]
     public enum ConverterMode
     {
-        None = 0,
-        Mindmapper = 1,
-        Dnn2sxc = 2,
-        BatchImageConverter = 4,
-        WebBasedImageGeneration = 8,
-        DatasetUpdater = 16,
-        TaxonomyValidator = 32,
-        OwlGenerator = 64,
-        OwlValidator = 128
+        None = 0,               
+        Mindmapper = 2,         
+        Dnn2sxc = 4,            
+        BatchImageConverter = 8,
+        WebBasedImageGeneration = 16,
+        DatasetUpdater = 32,    
+        TaxonomyValidator = 64, 
+        OwlGenerator = 128,     
+        OwlValidator = 256,
+        PdfAuditor = 512,
     }
 }
diff --git a/Generation/Converters/Argumentum.AssetConverter/Documentation/GenerateDocumentation.cs b/Generation/Converters/Argumentum.AssetConverter/Documentation/GenerateDocumentation.cs
new file mode 100644
index 00000000..8d2dc71d
--- /dev/null
+++ b/Generation/Converters/Argumentum.AssetConverter/Documentation/GenerateDocumentation.cs
@@ -0,0 +1,50 @@
+﻿using System;
+using System.Collections.Generic;
+using System.Linq;
+using System.Reflection;
+using System.Text;
+using System.Threading.Tasks;
+
+namespace Argumentum.AssetConverter.Documentation
+
+    public static class GenerateDocumentation
+    {
+        public static void GenerateAll(AssetConverterConfig config)
+        {
+            var markdown = new StringBuilder();
+            markdown.AppendLine("# Configuration Documentation");
+
+            markdown.AppendLine("## AssetConverterConfig");
+            markdown.AppendLine(GenerateClassDocumentation(typeof(AssetConverterConfig)));
+
+            // Add documentation for other classes as needed
+            // markdown.AppendLine("## OtherConfig");
+            // markdown.AppendLine(GenerateClassDocumentation(typeof(OtherConfig)));
+
+            File.WriteAllText("ConfigurationDocumentation.md", markdown.ToString());
+        }
+
+        private static string GenerateClassDocumentation(Type type)
+        {
+            var markdown = new StringBuilder();
+            markdown.AppendLine($"### {type.Name}");
+            markdown.AppendLine("| Property | Type | Default Value | Description |");
+            markdown.AppendLine("|---|---|---|---|");
+
+            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
+            var instance = Activator.CreateInstance(type);
+
+            foreach (var prop in properties)
+            {
+                var defaultValue = prop.GetValue(instance);
+                var defaultValueString = defaultValue?.ToString() ?? "null";
+                if (defaultValue is bool)
+                {
+                    defaultValueString = defaultValueString.ToLower();
+                }
+                markdown.AppendLine($"| {prop.Name} | {prop.PropertyType.Name} | `{defaultValueString}` | |");
+            }
+
+            return markdown.ToString();
+        }
+    }
diff --git a/Generation/Converters/Argumentum.AssetConverter/Logger.cs b/Generation/Converters/Argumentum.AssetConverter/Logger.cs
index 54881ae7..832130e5 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Logger.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/Logger.cs
@@ -1,5 +1,6 @@
 ﻿using System;
 using System.Diagnostics;
+using System.Runtime.InteropServices;
 using Spectre.Console;
 
 public enum MessageType
@@ -40,11 +41,21 @@
 
     public static void LogException(Exception ex)
     {
-        Console.WriteLine(ex);
-        AnsiConsole.WriteException(ex);
+        if (ex == null)
+        {
+            Log("An unknown error occurred.", MessageType.Problem);
+            return;
+        }
+
+        // Log exception details without relying on StackTrace for environments where it's not available
+        Log($"Exception Type: {ex.GetType().FullName}", MessageType.Problem);
+        Log($"Message: {ex.Message}", MessageType.Problem);
+        
+        // Fallback for stack trace if available
+        if (ex.StackTrace != null)
+        {
+            AnsiConsole.MarkupLine($"[grey]{ex.StackTrace}[/]");
+        }
+
+        if (ex.InnerException != null)
+        {
+            Log("Inner Exception:", MessageType.Explanations);
+            LogException(ex.InnerException);
+        }
     }
-
 }
diff --git a/Generation/Converters/Argumentum.AssetConverter/Program.cs b/Generation/Converters/Argumentum.AssetConverter/Program.cs
index d5337eb1..1d4c2ce6 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Program.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/Program.cs
@@ -13,6 +13,7 @@
 using System.Text.Json;
 using System.Threading.Tasks;
 using Argumentum.AssetConverter.Optimization;
+using Argumentum.AssetConverter.Documentation;
 
 namespace Argumentum.AssetConverter
 {
@@ -21,11 +22,17 @@
 
         static async Task Main(string[] args)
         {
-            var stopwatch = new Stopwatch();
-            stopwatch.Start();
-
             try
             {
+                if (args.Length > 0 && args[0].Equals("--generate-docs", StringComparison.OrdinalIgnoreCase))
+                {
+                    Logger.LogTitle("Generating documentation for configuration files...");
+                    var configForDocs = AssetConverterConfig.GetConfig("AssetConverterConfig.json", out _);
+                    GenerateDocumentation.GenerateAll(configForDocs);
+                    Logger.LogSuccess("Documentation generated successfully.");
+                    return;
+                }
+
                 if (args.Length > 0)
                 {
                     if (args[0].Equals("--validate-taxonomy", StringComparison.OrdinalIgnoreCase))
@@ -126,6 +133,9 @@
             catch (Exception ex)
             {
                 Logger.LogException(ex);
+
             }
+
         }
     }
 }
diff --git a/Generation/Converters/Argumentum.AssetConverter/Tests/CardGenerationValidationTests.cs b/Generation/Converters/Argumentum.AssetConverter/Tests/CardGenerationValidationTests.cs
index b2e37905..a1795b6c 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Tests/CardGenerationValidationTests.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/Tests/CardGenerationValidationTests.cs
@@ -20,10 +20,17 @@
             // Validation for each card set defined in config
             foreach (var cardSet in config.WebBasedGeneratorConfig.CardSets)
             {
-                ValidateCardSet(cardSet.Name, config, validationReport);
+                try
+                {
+                    ValidateCardSet(cardSet.Name, config, validationReport);
+                }
+                catch (Exception ex)
+                {
+                    validationReport.Add(ValidationSeverity.Error, $"Error validating card set '{cardSet.Name}': {ex.Message}");
+                }
             }
         }
-
+        
         public static void ValidateCardSet(string cardSetName, AssetConverterConfig config, ValidationReport validationReport)
         {
             var cardSetConfig = config.WebBasedGeneratorConfig.CardSets.FirstOrDefault(cs => cs.Name == cardSetName);
@@ -34,22 +41,31 @@
                 return;
             }
 
-            // Create a temporary directory for card generation
-            var tempDir = Path.Combine(Path.GetTempPath(), $"CardValidation_{cardSetName}_{Path.GetRandomFileName()}");
-            Directory.CreateDirectory(tempDir);
-
             try
             {
-                // Configure for single card generation
+                // Create a temporary directory for card generation
+                var tempDir = Path.Combine(Path.GetTempPath(), $"CardValidation_{cardSetName}_{Path.GetRandomFileName()}");
+                Directory.CreateDirectory(tempDir);
+
+                // Configure for single card generation
                 var singleCardConfig = new WebBasedGeneratorConfig
                 {
-                    // Adjust properties to only generate one card for speed and isolation
-
                     HarvestDirectoryName = tempDir,
                     MaxDegreeOfParallelismCardpen = 1,
-                    ShowInfoLogs = false, // Reduce noise during validation
+                    ShowInfoLogs = false,
+                    CardSets = new List<CardSetConfig> { cardSetConfig }
                 };
-                cardSetConfig.FaceCardSetInfo.cindices = "0-0"; // Limit to first card
+
+                // Create a minimal AssetConverterConfig for this validation run
+                var validationAssetConfig = new AssetConverterConfig
+                {
+                    WebBasedGeneratorConfig = singleCardConfig
+                };
+
+                cardSetConfig.FaceCardSetInfo.cindices = "0-0";
 
                 // Generate the card
                 var generator = new WebBasedGenerator(config);
@@ -58,9 +74,12 @@
                 // Check that an image file was created
                 if (!Directory.EnumerateFiles(tempDir).Any(f => f.EndsWith(".png") || f.EndsWith(".jpg")))
                 {
-                    validationReport.Add(ValidationSeverity.Error, $"Card generation for '{cardSetName}' did not produce an image file.");
+                    validationReport.Add(ValidationSeverity.Error, $"Card generation for '{cardSetName}' did not produce an image file in {tempDir}.");
                 }
+            }
+            finally
+            {
+                cardSetConfig.FaceCardSetInfo.cindices = null; // Reset for other operations
             }
         }
     }
diff --git a/Generation/Converters/Argumentum.AssetConverter/Tests/ContinuousValidationSystem.cs b/Generation/Converters/Argumentum.AssetConverter/Tests/ContinuousValidationSystem.cs
index 2cf8478d..49ebdcad 100644
--- a/Generation/Converters/Argumentum.AssetConverter/Tests/ContinuousValidationSystem.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/Tests/ContinuousValidationSystem.cs
@@ -16,14 +16,14 @@
             var validationReport = new ValidationReport(config.ContinuousValidationConfig.ReportPath, config.ContinuousValidationConfig.GenerateFullReport);
             var validator = new ContinuousValidationSystem(config, validationReport);
 
-            Logger.Log("Starting continuous validation...", MessageType.Title);
-            validator.RunAllValidations();
-            Logger.Log("Continuous validation finished.", MessageType.Success);
+            Logger.LogTitle("Starting continuous validation...");
+            await validator.RunAllValidations();
+            Logger.LogSuccess("Continuous validation finished.");
         }
 
-        public void RunAllValidations()
+        public async Task RunAllValidations()
         {
-            // Taxonomy Validation
+            // Run validations based on config
             if (_config.ContinuousValidationConfig.ValidateTaxonomy)
             {
                 _validationReport.Add(ValidationSeverity.Info, "--- Starting Taxonomy Validation ---");
@@ -31,23 +31,19 @@
                 _validationReport.Add(ValidationSeverity.Info, "--- Taxonomy Validation Finished ---");
             }
 
-            // OWL Ontology Validation
             if (_config.ContinuousValidationConfig.ValidateOwl)
             {
                 _validationReport.Add(ValidationSeverity.Info, "--- Starting OWL Ontology Validation ---");
-                OwlOntologyValidationTests.ValidateOwlOntology(_config, _validationReport);
+                await OwlOntologyValidationTests.ValidateOwlOntology(_config, _validationReport);
                 _validationReport.Add(ValidationSeverity.Info, "--- OWL Ontology Validation Finished ---");
             }
 
-            // Card Generation Validation
             if (_config.ContinuousValidationConfig.ValidateCards)
             {
                 _validationReport.Add(ValidationSeverity.Info, "--- Starting Card Generation Validation ---");
-                CardGenerationValidationTests.ValidateAllCardSets(_config, _validationReport);
+                await CardGenerationValidationTests.ValidateAllCardSets(_config, _validationReport);
                 _validationReport.Add(ValidationSeverity.Info, "--- Card Generation Validation Finished ---");
             }
-
-            // Finalize and save the report
-            _validationReport.FinalizeReport();
         }
     }
 }
diff --git a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/CardSetConfig.cs b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/CardSetConfig.cs
index bb56b142..c50b5550 100644
--- a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/CardSetConfig.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/CardSetConfig.cs
@@ -10,13 +10,13 @@
     public class CardSetConfig
     {
         public string Name { get; set; }
+
+        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
         public CardSetInfo FaceCardSetInfo { get; set; } = new CardSetInfo();
+
+        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
         public CardSetInfo BackCardSetInfo { get; set; } = new CardSetInfo();
-        public List<DocumentLocalization> CardSetLocalization { get; set; } = new List<DocumentLocalization>(new[]
-        {
-            new DocumentLocalization()
-        });
+
+        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
+        public List<DocumentLocalization> CardSetLocalization { get; set; } = new();
     }
-
 }
diff --git a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs
index 10834220..5f410714 100644
--- a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs
@@ -34,7 +34,7 @@
             var jsonContent = File.ReadAllText(jsonFilePath);
 
             var jsonOptions = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
-            var cardSetDefinition = JsonSerializer.Deserialize<CardpenJson>(jsonContent, jsonOptions: jsonOptions);
+            var cardSetDefinition = JsonSerializer.Deserialize<CardpenJson>(jsonContent, jsonOptions);
 
             if (csvContent != null)
             {
@@ -47,7 +57,7 @@
             }
 
             var updatedJsonContent = JsonSerializer.Serialize(cardSetDefinition, options: jsonOptions);
-                        if (updatedJsonContent != jsonContent)
+            if (updatedJsonContent != jsonContent)
             {
                 File.WriteAllText(jsonFilePath, updatedJsonContent);
                 Logger.Log($"JSON file updated: {jsonFilePath}", MessageType.Success);
diff --git a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/DataSetInfo.cs b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/DataSetInfo.cs
index 99c751a0..1298c9ec 100644
--- a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/DataSetInfo.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/DataSetInfo.cs
@@ -2,12 +2,14 @@
 using System.Collections.Generic;
 using System.IO;
 using System.Linq;
+using System.Text.Json.Serialization;
 
 namespace Argumentum.AssetConverter.WebBasedGenerator
 {
 
     public class DataSetInfo
     {
+        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
         public string Name { get; set; }
         public string ReleaseFilePath { get; set; }
         public string DebugFilePath { get; set; }
diff --git a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/ImageFileGenerator.cs b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/ImageFileGenerator.cs
index 6831d1d6..50058b8f 100644
--- a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/ImageFileGenerator.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/ImageFileGenerator.cs
@@ -9,7 +9,7 @@
 {
     public class ImageFileGenerator
     {
-        private IPage _page;
-        private WebBasedGeneratorConfig _config;
+        private readonly IPage _page;
+        private readonly WebBasedGeneratorConfig _config;
 
         public ImageFileGenerator(IPage page, WebBasedGeneratorConfig config)
         {
@@ -17,21 +17,22 @@
             _config = config;
         }
 
-        public async Task GenerateImagesAsync(string jsonContent, string targetFolder, string baseFileName)
+        public async Task<(string Front, string Back)> GenerateImagesAsync(string jsonContent, string targetFolder, string baseFileName)
         {
-            await _page.EvaluateAsync("cardpen.settings.layout = false;");
+            await _page.EvaluateAsync("() => cardpen.settings.layout = false");
             await _page.EvaluateAsync($"cardpen.utils.applySettings({jsonContent})");
-            await _page.EvaluateAsync("() => { cardpen.build(); }");
+            await _page.EvaluateAsync("() => cardpen.build(true)");
 
             await _page.WaitForFunctionAsync("() => cardpen.settings.cb > 0");
-
-            // Capture front
-            var frontImage = await _page.QuerySelectorAsync("#card-1 .cardpenCard");
-            var frontImageFile = Path.Combine(targetFolder, $"{baseFileName}_front.png");
-            await frontImage.ScreenshotAsync(new() { Path = frontImageFile });
-
-            // Capture back
-            var backImage = await _page.QuerySelectorAsync("#card-2 .cardpenCard");
-            var backImageFile = Path.Combine(targetFolder, $"{baseFileName}_back.png");
-            await backImage.ScreenshotAsync(new() { Path = backImageFile });
+            
+            var frontImageFile = await CaptureCardFace(1, "front", targetFolder, baseFileName);
+            var backImageFile = await CaptureCardFace(2, "back", targetFolder, baseFileName);
+            
+            return (frontImageFile, backImageFile);
+        }
+
+        private async Task<string> CaptureCardFace(int cardIndex, string face, string targetFolder, string baseFileName) {
+            var imageElement = await _page.QuerySelectorAsync($"#card-{cardIndex} .cardpenCard");
+            var imagePath = Path.Combine(targetFolder, $"{baseFileName}_{face}.png");
+            await imageElement.ScreenshotAsync(new() { Path = imagePath });
+            return imagePath;
         }
     }
 }
diff --git a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs
index f9978ffc..d562dc62 100644
--- a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs
@@ -19,102 +19,105 @@
     public class PdfManager
     {
         private readonly WebBasedGeneratorConfig _config;
-        private readonly string _imagesDirectory;
-        private readonly string _documentsDirectory;
+        private readonly string _baseTargetDirectory;
 
         public PdfManager(WebBasedGeneratorConfig config)
         {
             _config = config;
-            _imagesDirectory = Path.Combine(config.BaseTargetDirectoryName, config.ImagesDirectoryName);
-            _documentsDirectory = Path.Combine(config.BaseTargetDirectoryName, config.DocumentsDirectoryName);
+            _baseTargetDirectory = config.BaseTargetDirectoryName;
+            QuestPDF.Settings.License = LicenseType.Community;
         }
 
-        public async Task GeneratePdfs()
+        public async Task GeneratePdfs(Dictionary<CardSetDocumentConfig, List<(string Front, string Back)>> generatedImages)
         {
-            Directory.CreateDirectory(_documentsDirectory);
+            string documentsDirectory = Path.Combine(_baseTargetDirectory, _config.DocumentsDirectoryName);
+            Directory.CreateDirectory(documentsDirectory);
 
-            var pdfTasks = _config.CardSetDocuments
-                .Where(doc => doc.Enabled)
-                .Select(docConfig => GenerateSinglePdf(docConfig));
+            var pdfTasks = generatedImages.Select(kvp =>
+                GenerateSinglePdf(kvp.Key, kvp.Value, documentsDirectory));
 
             await Task.WhenAll(pdfTasks);
         }
 
-        private async Task GenerateSinglePdf(CardSetDocumentConfig docConfig)
+        private async Task GenerateSinglePdf(CardSetDocumentConfig docConfig, List<(string Front, string Back)> imagePaths, string documentsDirectory)
         {
-            var documentPath = Path.Combine(_documentsDirectory, docConfig.DocumentName);
+            var documentPath = Path.Combine(documentsDirectory, docConfig.DocumentName);
             Logger.Log($"Generating PDF: {documentPath}", MessageType.Title);
 
+            // Create document metadata
             var docMetadata = new DocumentMetadata
             {
                 Title = docConfig.DocumentName,
-                Author = "Argumentum Asset Converter"
+                Author = "Argumentum RPG"
             };
 
+            // Main document generation
             Document.Create(container =>
             {
                 container.Page(page =>
                 {
-                    ConfigurePage(page, docConfig);
-
+                    page.Size(docConfig.PageSize.ToUpper() == "A4" ? PageSizes.A4 : PageSizes.Letter);
+                    page.Margin(docConfig.Padding, Unit.Millimetre);
+                    
                     if (!string.IsNullOrEmpty(docConfig.Header))
                     {
                         page.Header().Element(headerContainer => RenderHeader(headerContainer, docConfig.Header));
                     }
-
-                    page.Content().Element(contentContainer =>
-                    {
-                        var allImagePaths = docConfig.CardSets
-                            .SelectMany(cs => GetCardImagePaths(cs.CardSetName, cs.NbCopies))
-                            .ToList();
-
-                        RenderCardsGrid(contentContainer, allImagePaths, docConfig);
-                    });
-
+                    
+                    page.Content().Column(column =>
+                    {
+                        var allImages = imagePaths.SelectMany(p => Enumerable.Repeat(p, 1)).ToList();
+                        RenderCardsGrid(column, allImages, docConfig);
+                    });
+                    
                     if (!string.IsNullOrEmpty(docConfig.Footer))
                     {
                         page.Footer().Element(footerContainer => RenderFooter(footerContainer, docConfig.Footer));
                     }
                 });
-            }).GeneratePdf(documentPath);
+            })
+            .WithMetadata(docMetadata)
+            .GeneratePdf(documentPath);
 
             Logger.Log($"PDF generated successfully: {documentPath}", MessageType.Success);
         }
-
-        private void ConfigurePage(PageDescriptor page, CardSetDocumentConfig docConfig)
-        {
-            if (docConfig.PageSize.Equals("A4", StringComparison.OrdinalIgnoreCase))
-                page.Size(PageSizes.A4);
-            else
-                page.Size(PageSizes.Letter);
-
-            page.Margin(docConfig.Padding, Unit.Millimetre);
-        }
-
+        
         private void RenderHeader(IContainer container, string headerText)
         {
             container.AlignCenter().Text(headerText).SemiBold().FontSize(16);
         }
 
-        private void RenderCardsGrid(IContainer container, List<string> imagePaths, CardSetDocumentConfig docConfig)
+        private void RenderCardsGrid(IContainer container, List<(string Front, string Back)> imagePaths, CardSetDocumentConfig docConfig)
         {
-            container.Grid(grid =>
+             container.Grid(grid =>
             {
-                grid.Columns(docConfig.NbColumns);
-                grid.Spacing(docConfig.Gutter, Unit.Millimetre);
+                var nbColumns = docConfig.NbColumns > 0 ? docConfig.NbColumns : 3;
+                grid.Columns(nbColumns);
 
-                foreach (var imagePath in imagePaths)
+                foreach (var (frontPath, backPath) in imagePaths)
                 {
-                    grid.Item().Element(cell =>
-                    {
-                        if (File.Exists(imagePath))
-                            cell.Image(imagePath).FitArea();
-                        else
-                            cell.Text($"Image not found: {Path.GetFileName(imagePath)}").ForeColor(Colors.Red.Medium);
-                    });
+                    if (docConfig.DocumentFormat == CardDocumentFormat.PrintAndPlay)
+                    {
+                        // Front and back for print-and-play
+                        grid.Item().Image(frontPath);
+                        grid.Item().Image(backPath);
+                    }
+                    else // FacesOnly or default
+                    {
+                        grid.Item().Image(frontPath);
+                    }
                 }
             });
         }
-
-        private IEnumerable<string> GetCardImagePaths(string cardSetName, int copies)
-        {
-            var cardSetDir = Path.Combine(_imagesDirectory, cardSetName);
-            if (Directory.Exists(cardSetDir))
-            {
-                var imageFiles = Directory.EnumerateFiles(cardSetDir, "*.png")
-                                         .Concat(Directory.EnumerateFiles(cardSetDir, "*.jpg"));
-                return Enumerable.Repeat(imageFiles, copies).SelectMany(f => f);
-            }
-            return Enumerable.Empty<string>();
-        }
-
+        
         private void RenderFooter(IContainer container, string footerText)
         {
             container.AlignCenter().Text(footerText).FontSize(10);
diff --git a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGenerator.cs b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGenerator.cs
index 589d81d4..23ed9569 100644
--- a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGenerator.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGenerator.cs
@@ -13,6 +13,7 @@
 using System.Text.Json.Serialization;
 using System.Threading.Tasks;
 using Argumentum.AssetConverter.Entities;
+using Argumentum.AssetConverter.PdfAuditor;
 using AutoMapper;
 using ExtendedXmlSerializer.Core.Sources;
 using ImageMagick;
@@ -42,7 +43,7 @@
             _page = await browser.NewPageAsync();
             _harvestManager = new HarvestManager(config.WebBasedGenerator.ReleaseCardpenUrl, config.WebBasedGenerator.DebugCardpenUrl);
             _imageGenerator = new ImageFileGenerator(_page, config.WebBasedGenerator);
-            _pdfManager = new PdfManager(config.WebBasedGenerator);
+            _pdfManager = new PdfManager(config.WebBasedGenerator); // Pass WebBasedGeneratorConfig
             _mapper = new Mapper(new MapperConfiguration(cfg => { }));
             Quest.Settings.License = LicenseType.Community;
         }
@@ -62,26 +63,26 @@
                 Logger.Log($"Processing card set: {cardSet.Name}", MessageType.Title);
                 var targetFolder = Path.Combine(_config.WebBasedGenerator.HarvestDirectoryName, cardSet.Name);
                 Directory.CreateDirectory(targetFolder);
-
-                // Generate images for each data row
+                
+                // This dictionary will hold generated image paths for each document config
+                var imagesForDocs = new Dictionary<CardSetDocumentConfig, List<(string Front, string Back)>>();
+                
                 foreach (var dataRow in csvData)
                 {
                     var jsonContent = _harvestManager.GenerateJsonForCard(cardSet, dataRow);
                     var baseFileName = $"{cardSet.Name}_{csvData.IndexOf(dataRow)}";
-                    await _imageGenerator.GenerateImagesAsync(jsonContent, targetFolder, baseFileName);
+
+                    var (front, back) = await _imageGenerator.GenerateImagesAsync(jsonContent, targetFolder, baseFileName);
+
+                    // Associate generated images with their respective document configurations
+                    foreach (var docConfig in _config.WebBasedGenerator.CardSetDocuments.Where(d => d.CardSets.Any(cs => cs.CardSetName == cardSet.Name)))
+                    {
+                        if (!imagesForDocs.ContainsKey(docConfig))
+                            imagesForDocs[docConfig] = new List<(string Front, string Back)>();
+                        imagesForDocs[docConfig].Add((front, back));
+                    }
                 }
-            }
-
-            // Generate PDFs from harvested images
-            await _pdfManager.GeneratePdfs();
-        }
-
-        public void AuditPdfs(List<(Func<IMagickImage> front, Func<IMagickImage> back)> documentImages)
-        {
-            // Stub for PDF auditing logic
-            // This will be replaced by the actual implementation from PdfAuditor.cs
-            Logger.Log("PDF auditing is not yet implemented.", MessageType.Warning);
+            await _pdfManager.GeneratePdfs(imagesForDocs);
         }
     }
 }
diff --git a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGeneratorConfig.cs b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGeneratorConfig.cs
index ffc204f8..8a562dfd 100644
--- a/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGeneratorConfig.cs
+++ b/Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGeneratorConfig.cs
@@ -7,9 +7,8 @@
         public bool EnableSVGPrompt { get; set; } = true;
         public bool ShowInfoLogs { get; set; } = true;
         public bool HeadLessBrowser { get; set; } = false;
-        public bool OverwriteExistingDocs { get; set; } = false;
-        public bool OverwriteExistingHtmlMaps { get; set; } = false;
-        public int MaxDegreeOfParallelismCardpen { get; set; } = 3;
+        public bool OverwriteExistingDocs { get; set; }
+        public int MaxDegreeOfParallelismCardpen { get; set; } = 1;
         public int MaxDegreeOfParallelismCardpenTranslations { get; set; } = 2;
         public int MaxDegreeOfParallelismImages { get; set; } = 3;
         public int MaxDegreeOfParallelismImageTranslations { get; set; } = 2;
@@ -21,11 +20,11 @@
         public string DebugCardpenUrl { get; set; } = @"http://cardpen.dnndev.me/Generation/CardPen/index.html";       
         public List<DataSetInfo> DataSets { get; set; } = new List<DataSetInfo>();
 
-               public List<CardSetConfig> CardSets { get; set; } = new List<CardSetConfig>();
+        public List<CardSetConfig> CardSets { get; set; } = new List<CardSetConfig>();
 
-               public List<CardSetDocumentConfig> CardSetDocuments { get; set; } = new List<CardSetDocumentConfig>();
+        public List<CardSetDocumentConfig> CardSetDocuments { get; set; } = new List<CardSetDocumentConfig>();
 
-               public List<DocumentLocalization> MindMapLocalization { get; set; } = new List<DocumentLocalization>();
+        public List<DocumentLocalization> MindMapLocalization { get; set; } = new List<DocumentLocalization>();
 
         public async Task Run(AssetConverterConfig config)
         {

```
