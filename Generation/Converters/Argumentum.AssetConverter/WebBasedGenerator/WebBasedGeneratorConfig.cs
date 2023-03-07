using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Argumentum.AssetConverter
{
    public class WebBasedGeneratorConfig
    {

        //public string ChromeBinaryPath { get; set; } = @"E:\LiberKey\MyApps\GoogleChromePortable\App\Chrome-bin\chrome.exe";

        public string CardpenUrl { get; set; }= @"https://argumentumgames.github.io/Argumentum/Generation/CardPen/index.html";
        //For local hosting http://cardpen.dnndev.me/Generation/CardPen/index.html

        //public int TestCards { get; set; } = 2;

        public List<DocumentConfig> Documents { get; set; } = new List<DocumentConfig>(
            new[]
            {
                new DocumentConfig()
                {
                    DocumentName = "Argumentum-TarotCards.pdf",
                    Enabled = false,
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = "Rules",
                            NbCopies = 1,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                                {
                                    BorderMM = 0,
                                    HeigthMM = 113,
                                    WidthMM = 60,
                                },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            }
                        },
                        new DocumentCardSet()
                        {
                            CardSetName = "Memo",
                            NbCopies = 7,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            }
                        },
                        new DocumentCardSet()
                        {
                            CardSetName = "Fallacies",
                            NbCopies = 1,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            }
                        },
                    }),
                },
                new DocumentConfig()
                {
                    DocumentName = "Argumentum-PokerCards.pdf",
                    Enabled = false,
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = "Scenarii",
                            NbCopies = 1,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 89,
                                WidthMM = 58,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 89,
                                WidthMM = 58,
                            }
                        }
                    }),
                },
                new DocumentConfig()
                {
                    DocumentName = "Argumentum-TarotCards-2.pdf",
                    Enabled = false,
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = "Rules",
                            NbCopies = 1,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            }
                        },
                        new DocumentCardSet()
                        {
                            CardSetName = "Memo",
                            NbCopies = 7,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            }
                        },
                        new DocumentCardSet()
                        {
                            CardSetName = "Fallacies-2",
                            NbCopies = 1,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            }
                        },
                    }),
                },
                new DocumentConfig()
                {
                    DocumentName = "Argumentum-TarotCards-3.pdf",
                    Enabled = false,
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = "Rules",
                            NbCopies = 1,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            }
                        },
                        new DocumentCardSet()
                        {
                            CardSetName = "Memo",
                            NbCopies = 7,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            }
                        },
                        new DocumentCardSet()
                        {
                            CardSetName = "Fallacies-3",
                            NbCopies = 1,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            }
                        },
                    }),
                },
                 new DocumentConfig()
                {
                    DocumentName = "Argumentum-TarotCards-Print&Play-A4.pdf",
                    Enabled = false,
                    DocumentFormat = CardDocumentFormat.PrintAndPlay,
                    PageSize = "A4",
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = "Rules-Print&Play",
                            NbCopies = 1,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                                {
                                    BorderMM = 0,
                                    HeigthMM = 113,
                                    WidthMM = 60,
                                },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            }
                        },
                        new DocumentCardSet()
                        {
                            CardSetName = "Memo-Print&Play",
                            NbCopies = 5,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            }
                        },
                        new DocumentCardSet()
                        {
                            CardSetName = "Fallacies-Print&Play",
                            NbCopies = 1,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 113,
                                WidthMM = 60,
                            }
                        },
                    }),
                },
                new DocumentConfig()
                {
                    DocumentName = "Argumentum-PokerCards-Print&Play-A4.pdf",
                    Enabled = false,
                    DocumentFormat = CardDocumentFormat.PrintAndPlay,
                    PageSize = "A4",
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = "Scenarii-Print&Play",
                            NbCopies = 1,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 89,
                                WidthMM = 58,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 89,
                                WidthMM = 58,
                            }
                        }
                    }),
                },
                new DocumentConfig()
                {
                    DocumentName = "Argumentum-Fallacies-Web-Thumbnails.pdf",
                    Enabled = true,
                    DocumentFormat = CardDocumentFormat.PrintAndPlay,
                    PageSize = "A4",
                    NoBack = true,
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = "Fallacies-Web-Thumbnails",
                            NbCopies = 1,
                            ConvertToCmyk = false,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 50,
                                WidthMM = 50,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 72,
                                WidthMM = 72,
                            }
                        }
                    }),
                },
                new DocumentConfig()
                {
                    DocumentName = "Argumentum-Fallacies-Web-A4.pdf",
                    Enabled = false,
                    DocumentFormat = CardDocumentFormat.PrintAndPlay,
                    PageSize = "A4",
                    NoBack = true,
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = "Fallacies-Web-Light",
                            NbCopies = 1,
                            ConvertToCmyk = false,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 70,
                                WidthMM = 70,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 70,
                                WidthMM = 70,
                            }
                        }
                    }),
                },
                new DocumentConfig()
                {
                    DocumentName = "Argumentum-Fallacies-Web-A0.pdf",
                    Enabled = false,
                    DocumentFormat = CardDocumentFormat.PrintAndPlay,
                    PageSize = "A0",
                    NoBack = true,
                    Header = "Logo_Argumentum.png",
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = "Fallacies-Web",
                            NbCopies = 1,
                            ConvertToCmyk = true,
                            SaveOriginalImage = true,
                            FrontCards = new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 72,
                                WidthMM = 72,
                            },
                            BackCards =  new DocumentCard()
                            {
                                BorderMM = 0,
                                HeigthMM = 72,
                                WidthMM = 72,
                            }
                        }
                    }),
                }
            });


        public string BaseTargetDirectoryName { get; set; } = @"Target\";

        public string HarvestDirectoryName { get; set; } = @"Harvest\";


        public string ImagesDirectoryName { get; set; } = @"Images\";

        public string PdfsDirectoryName { get; set; } = @"Pdfs\";

        public string GetBaseTargetDirectory()
        {
            var toReturn =  Path.Combine(System.Environment.CurrentDirectory, BaseTargetDirectoryName);
            if (!Directory.Exists(toReturn))
            {
                Directory.CreateDirectory(toReturn);
            }

            return toReturn;
        }

        public string GetHarvestDirectory()
        {
            var toReturn = Path.Combine(GetBaseTargetDirectory(), HarvestDirectoryName);
            if (!Directory.Exists(toReturn))
            {
                Directory.CreateDirectory(toReturn);
            }

            return toReturn;
        }

        public string GetImagesDirectory()
        {
            var toReturn = Path.Combine(GetBaseTargetDirectory(), ImagesDirectoryName);
            if (!Directory.Exists(toReturn))
            {
                Directory.CreateDirectory(toReturn);
            }

            return toReturn;
        }

        public string GetPdfsDirectory()
        {
            var toReturn = Path.Combine(GetBaseTargetDirectory(), PdfsDirectoryName);
            if (!Directory.Exists(toReturn))
            {
                Directory.CreateDirectory(toReturn);
            }

            return toReturn;
        }




        public List<CardSetConfig> CardSets { get; set; } = new List<CardSetConfig>(
            new []
            {
                new CardSetConfig(){
                    Name ="Rules",
                    FaceCardSetInfo = new CardSetInfo()
                        {
                            CardSetType = CardSetType.CustomJson,
                            ExampleName = "Argumentum - Rules - Francais",
                            CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Rules/Argumentum_Rules_Francais_edition_fevrier_2022.json"
						}
                      },                  
                new CardSetConfig(){
                    Name ="Memo",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Memo - Face - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Memo/Argumentum_Memo_Face_Francais.json"
					},
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Memo - Back - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Memo/Argumentum_Memo_Back_Francais.json"
					}
                },
                new CardSetConfig(){
                    Name ="Fallacies",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.ExampleByName,
                        ExampleName = "Argumentum - Fallacies - Face - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Face_Francais.json"

					},
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.ExampleByName,
                        ExampleName = "Argumentum - Fallacies - Back - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Back_Francais.json"
					}
                },
                new CardSetConfig(){
                    Name ="Scenarii",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Scenarii - Face - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Scenarii/Argumentum_Scenarii_Face_Francais_edition_fevrier_2022.json"
					},
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Scenarii - Back - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Scenarii/Argumentum_Scenarii_Back_Francais.json"
					}
                },
                new CardSetConfig(){
                    Name ="Fallacies-2",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Fallacies - Face - Francais - Bis",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Face_Francais_Bis_edition_fevrier_2022.json"
					},
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Fallacies - Back - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Back_Francais.json"
					}
                },
                new CardSetConfig(){
                    Name ="Fallacies-3",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.ExampleByName,
                        ExampleName = "Argumentum - Fallacies - Face - v2 - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Face_v2_Francais.json"
					},
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.ExampleByName,
                        ExampleName = "Argumentum - Fallacies - Back - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Back_Francais.json"
					}
                },
                new CardSetConfig(){
                    Name ="Fallacies-Print&Play",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Fallacies - Face - Francais - Bis",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Face_Francais_Bis_edition_fevrier_2022_Print_and_Play.json"
					},
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Fallacies - Back - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Back_Francais.json"
					}
                },
                new CardSetConfig(){
                    Name ="Scenarii-Print&Play",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Scenarii - Face - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Scenarii/Argumentum_Scenarii_Face_Francais_edition_fevrier_2022_Print_and_Play.json"
					},
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Scenarii - Back - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Scenarii/Argumentum_Scenarii_Back_Francais.json"
					}
                },
                new CardSetConfig(){
                    Name ="Rules-Print&Play",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Rules - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Rules/Argumentum_Rules_Francais_edition_fevrier_2022_Print_and_Play.json"
					}
                },
                new CardSetConfig(){
                    Name ="Memo-Print&Play",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Memo - Face - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Memo/Argumentum_Memo_Face_Francais_Print_and_Play.json"
					},
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Memo - Back - Francais",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Memo/Argumentum_Memo_Back_Francais_Print_and_Play.json"
					}
                },
                new CardSetConfig(){
                    Name ="Fallacies-Web",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Fallacies - Face - Francais - Bis",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Face_Francais_Bis_edition_fevrier_2022_Web.json"
					}
                },
                new CardSetConfig(){
                    Name ="Fallacies-Web-Light",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Fallacies - Face - Francais - Bis",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Face_Francais_Bis_edition_fevrier_2022_Web_light.json"
					}
                },
                new CardSetConfig(){
                    Name ="Fallacies-Web-Thumbnails",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Fallacies - Face - Francais - Bis",
                        CustomJsonFileName = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Face_Francais_Bis_edition_fevrier_2022_Web_thumbnails.json"
					}
                }
            });

        public void Apply(Stopwatch objSw)
        {
            var generator = new WebBasedGenerator(this, objSw);
            generator.Run();

        }


    }
}