using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Argumentum.AssetConverter
{
    public class WebBasedGeneratorConfig
    {

        public string ChromeBinaryPath { get; set; } = @"E:\LiberKey\MyApps\GoogleChromePortable\App\Chrome-bin\chrome.exe";

        public string CardpenUrl { get; set; }= @"http://argumentum.myia.org/portals/0/argumentum/Generation/CardPen/index.html";
        //http://cardpen.dnndev.me/Generation/CardPen/index.html

        //public int TestCards { get; set; } = 2;

        public List<DocumentConfig> Documents { get; set; } = new List<DocumentConfig>(
            new[]
            {
                new DocumentConfig()
                {
                    DocumentName = "Argumentum-TarotCards.pdf",
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
                    DocumentName = "Argumentum-TarotCards-Print&Play.pdf",
                    DocumentFormat = CardDocumentFormat.PrintAndPlay,
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
                    DocumentName = "Argumentum-PokerCards-Print&Play.pdf",
                    DocumentFormat = CardDocumentFormat.PrintAndPlay,
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
            });


        public string BaseTargetDirectoryName { get; set; } = @".\Target\";

        public string HarvestDirectoryName { get; set; } = @".\Harvest\";


        public string ImagesDirectoryName { get; set; } = @".\Images\";

        public string PdfsDirectoryName { get; set; } = @".\Pdfs\";

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
                            CustomJsonFileName = "Argumentum_Rules_Francais_edition_fevrier_2022.json"
                        }
                      },                  
                new CardSetConfig(){
                    Name ="Memo",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Memo - Face - Francais",
                        CustomJsonFileName = "Argumentum_Memo_Face_Francais.json"
                    },
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Memo - Back - Francais",
                        CustomJsonFileName = "Argumentum_Memo_Back_Francais.json"
                    }
                },
                new CardSetConfig(){
                    Name ="Fallacies",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.ExampleByName,
                        ExampleName = "Argumentum - Fallacies - Face - Francais",
                    },
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.ExampleByName,
                        ExampleName = "Argumentum - Fallacies - Back - Francais",
                    }
                },
                new CardSetConfig(){
                    Name ="Scenarii",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Scenarii - Face - Francais",
                        CustomJsonFileName = "Argumentum_Scenarii_Face_Francais_edition_fevrier_2022.json"
                    },
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Scenarii - Back - Francais",
                        CustomJsonFileName = "Argumentum_Scenarii_Back_Francais.json"
                    }
                },
                new CardSetConfig(){
                    Name ="Fallacies-2",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Fallacies - Face - Francais - Bis",
                        CustomJsonFileName = "Argumentum_Fallacies_Face_Francais_Bis_edition_fevrier_2022.json"
                    },
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Fallacies - Back - Francais",
                        CustomJsonFileName = "Argumentum_Fallacies_Back_Francais.json"
                    }
                },
                new CardSetConfig(){
                    Name ="Fallacies-3",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.ExampleByName,
                        ExampleName = "Argumentum - Fallacies - Face - v2 - Francais",
                    },
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.ExampleByName,
                        ExampleName = "Argumentum - Fallacies - Back - Francais",
                    }
                },
                new CardSetConfig(){
                    Name ="Fallacies-Print&Play",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Fallacies - Face - Francais - Bis",
                        CustomJsonFileName = "Argumentum_Fallacies_Face_Francais_Bis_edition_fevrier_2022_Print_and_Play.json"
                    },
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Fallacies - Back - Francais",
                        CustomJsonFileName = "Argumentum_Fallacies_Back_Francais.json"
                    }
                },
                new CardSetConfig(){
                    Name ="Scenarii-Print&Play",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Scenarii - Face - Francais",
                        CustomJsonFileName = "Argumentum_Scenarii_Face_Francais_edition_fevrier_2022_Print_and_Play.json"
                    },
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Scenarii - Back - Francais",
                        CustomJsonFileName = "Argumentum_Scenarii_Back_Francais.json"
                    }
                },
                new CardSetConfig(){
                    Name ="Rules-Print&Play",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Rules - Francais",
                        CustomJsonFileName = "Argumentum_Rules_Francais_edition_fevrier_2022_Print_and_Play.json"
                    }
                },
                new CardSetConfig(){
                    Name ="Memo-Print&Play",
                    FaceCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Memo - Face - Francais",
                        CustomJsonFileName = "Argumentum_Memo_Face_Francais_Print_and_Play.json"
                    },
                    BackCardSetInfo = new CardSetInfo()
                    {
                        CardSetType = CardSetType.CustomJson,
                        ExampleName = "Argumentum - Memo - Back - Francais",
                        CustomJsonFileName = "Argumentum_Memo_Back_Francais_Print_and_Play.json"
                    }
                },
            });

        public void Apply(Stopwatch objSw)
        {
            var generator = new WebBasedGenerator(this, objSw);
            generator.Run();

        }


    }
}