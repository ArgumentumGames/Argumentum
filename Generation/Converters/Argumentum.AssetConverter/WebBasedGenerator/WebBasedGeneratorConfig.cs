using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Argumentum.AssetConverter
{

	public static class KnownCardSets
	{
		public static string Fallacies = "Fallacies";
		public static string Scenarii = "Scenarii";
		public static string Rules = "Rules";
		public static string Memo = "Memo";
		public static string Fallacies2 = "Fallacies-2";
		public static string Fallacies3 = "Fallacies-3";
		public static string FallaciesPrintAndPlay = "Fallacies-Print&Play";
		public static string ScenariiPrintAndPlay = "Scenarii-Print&Play";
		public static string RulesPrintAndPlay = "Rules-Print&Play";
		public static string MemoPrintAndPlay = "Memo-Print&Play";
		public static string FallaciesWeb = "Fallacies-Web";
		public static string FallaciesWebLight = "Fallacies-Web-Light";
		public static string FallaciesWebThumbnails = "Fallacies-Web-Thumbnails";


	}
	


	public class WebBasedGeneratorConfig
    {


        public string CardpenUrl { get; set; }= @"https://argumentumgames.github.io/Argumentum/Generation/CardPen/index.html";
        //For local hosting http://cardpen.dnndev.me/Generation/CardPen/index.html
		
        
		public LocalizationConfig LocalizationConfig = new LocalizationConfig()
		{
            Enabled = true,
            CardSetLocalizations = new List<CardSetLocalization>(new[]{
				new CardSetLocalization()
				{
					CardSetNames = new List<string>(new []
					{
						KnownCardSets.Fallacies, 
						KnownCardSets.Fallacies2, 
						KnownCardSets.Fallacies3,
						KnownCardSets.FallaciesPrintAndPlay,
						KnownCardSets.FallaciesWeb,
						KnownCardSets.FallaciesWebLight,
						KnownCardSets.FallaciesWebThumbnails,

					}),
					FrontFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("Famille", new List<(string Language, string destFieldName)>(new []{("en", "Family"), ("ru", "Family_ru") }) ),
						("Sous-Famille", new List<(string Language, string destFieldName)>(new []{("en", "Subfamily"), ("ru", "Subfamily_ru") }) ),
						("Soussousfamille", new List<(string Language, string destFieldName)>(new []{("en", "Subsubfamily"), ("ru", "Subsubfamily_ru") }) ),
						("text_fr", new List<(string Language, string destFieldName)>(new []{("en", "text_en"), ("ru", "text_ru") }) ),
						("desc_fr", new List<(string Language, string destFieldName)>(new []{("en", "desc_en"), ("ru", "desc_ru") }) ),
						("example_fr", new List<(string Language, string destFieldName)>(new []{("en", "example_en"), ("ru", "example_ru") }) ),
					}),
					BackFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("tagline_fr", new List<(string Language, string destFieldName)>(new []{("en", "tagline_en"), ("ru", "tagline_ru") }) ),
					})
				},
				new CardSetLocalization()
				{
					CardSetNames = new List<string>(new []
					{
						KnownCardSets.Scenarii,
						KnownCardSets.ScenariiPrintAndPlay,

					}),
					FrontFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("catégorie", new List<(string Language, string destFieldName)>(new []{("en", "category"), ("ru", "category_ru") }) ),
						("titre", new List<(string Language, string destFieldName)>(new []{("en", "title"), ("ru", "title_ru") }) ),
						("contexte", new List<(string Language, string destFieldName)>(new []{("en", "context"), ("ru", "context_ru") }) ),
						("enjeu", new List<(string Language, string destFieldName)>(new []{("en", "issue"), ("ru", "issue_ru") }) ),
						("piocheur", new List<(string Language, string destFieldName)>(new []{("en", "drawer"), ("ru", "drawer_ru") }) ),
						("baratineur", new List<(string Language, string destFieldName)>(new []{("en", "smoothTalker"), ("ru", "smoothTalker_ru") }) ),
						("suggestion", new List<(string Language, string destFieldName)>(new []{("en", "suggestion_en"), ("ru", "suggestion_en_ru") }) ),
					}),
					BackFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("catégorie", new List<(string Language, string destFieldName)>(new []{("en", "category"), ("ru", "category_ru") }) ),
					})
				},
				new CardSetLocalization()
				{
					CardSetNames = new List<string>(new []
					{
						KnownCardSets.Rules,
						KnownCardSets.RulesPrintAndPlay,

					}),
					FrontFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("Text", new List<(string Language, string destFieldName)>(new []{("en", "Text_en"), ("ru", "Text_ru") }) ),
					})
				},
				new CardSetLocalization()
				{
					CardSetNames = new List<string>(new []
					{
						KnownCardSets.Memo,
						KnownCardSets.MemoPrintAndPlay,

					}),
					FrontFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("Famille", new List<(string Language, string destFieldName)>(new []{("en", "Family"), ("ru", "Family_ru") }) ),
						("desc_fr", new List<(string Language, string destFieldName)>(new []{("en", "desc_en"), ("ru", "desc_ru") }) ),
					}),
					BackFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("Famille", new List<(string Language, string destFieldName)>(new []{("en", "Family"), ("ru", "Family_ru") }) ),
						("Sous-Famille", new List<(string Language, string destFieldName)>(new []{("en", "Subfamily"), ("ru", "Subfamily_ru") }) ),
						("Soussousfamille", new List<(string Language, string destFieldName)>(new []{("en", "Subsubfamily"), ("ru", "Subsubfamily_ru") }) ),
						("tagline_fr", new List<(string Language, string destFieldName)>(new []{("en", "tagline_en"), ("ru", "tagline_ru") }) ),
					})
				},
			})
		};

		public List<CardSetConfig> CardSets { get; set; } = new List<CardSetConfig>(
			new[]
			{
				new CardSetConfig(){
					Name = KnownCardSets.Rules,
					FaceCardSetInfo = new CardSetInfo()
						{
							JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Rules/Argumentum_Rules_Francais_edition_fevrier_2022.json",
						}
					  },
				new CardSetConfig(){
					Name =KnownCardSets.Memo,
					FaceCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Memo/Argumentum_Memo_Face_Francais.json"
					},
					BackCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Memo/Argumentum_Memo_Back_Francais.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.Fallacies,
					FaceCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Face_Francais.json"

					},
					BackCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Back_Francais.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.Scenarii,
					FaceCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Scenarii/Argumentum_Scenarii_Face_Francais_edition_fevrier_2022.json"
					},
					BackCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Scenarii/Argumentum_Scenarii_Back_Francais.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.Fallacies2,
					FaceCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Face_Francais_Bis_edition_fevrier_2022.json"
					},
					BackCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Back_Francais.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.Fallacies3,
					FaceCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Face_v2_Francais.json"
					},
					BackCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Back_Francais.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.FallaciesPrintAndPlay,
					FaceCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Face_Francais_Bis_edition_fevrier_2022_Print_and_Play.json"
					},
					BackCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Back_Francais.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.ScenariiPrintAndPlay,
					FaceCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Scenarii/Argumentum_Scenarii_Face_Francais_edition_fevrier_2022_Print_and_Play.json"
					},
					BackCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Scenarii/Argumentum_Scenarii_Back_Francais.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.RulesPrintAndPlay,
					FaceCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Rules/Argumentum_Rules_Francais_edition_fevrier_2022_Print_and_Play.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.MemoPrintAndPlay,
					FaceCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Memo/Argumentum_Memo_Face_Francais_Print_and_Play.json"
					},
					BackCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Memo/Argumentum_Memo_Back_Francais_Print_and_Play.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.FallaciesWeb,
					FaceCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Face_Francais_Bis_edition_fevrier_2022_Web.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.FallaciesWebLight,
					FaceCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Face_Francais_Bis_edition_fevrier_2022_Web_light.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.FallaciesWebThumbnails,
					FaceCardSetInfo = new CardSetInfo()
					{
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Argumentum_Fallacies_Face_Francais_Bis_edition_fevrier_2022_Web_thumbnails.json"
					}
				}
			});




		public List<DocumentConfig> Documents { get; set; } = new List<DocumentConfig>(
            new[]
            {
                new DocumentConfig()
                {
                    DocumentName = "Argumentum_FallacyCards_fr.pdf",
                    Enabled = false,
                    Translations = new List<(string sourceLang, string destLang)>(new []
                    {
	                    ("fr","en"),
	                    ("fr", "ru")
                    }),
					CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = KnownCardSets.Rules,
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
                            CardSetName = KnownCardSets.Memo,
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
                            CardSetName = KnownCardSets.Fallacies,
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
                    DocumentName = "Argumentum_PokerCards_fr.pdf",
                    Enabled = false,
                    Translations = new List<(string sourceLang, string destLang)>(new []
                    {
	                    ("fr","en")
                    }),
					CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = KnownCardSets.Scenarii,
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
                    DocumentName = "Argumentum_FallacyCards_2_fr.pdf",
                    Enabled = false,
                    Translations = new List<(string sourceLang, string destLang)>(new []
                    {
	                    ("fr","en"), 
	                    ("fr", "ru")
                    }),
					CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = KnownCardSets.Rules,
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
                            CardSetName = KnownCardSets.Memo,
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
                            CardSetName = KnownCardSets.Fallacies2,
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
                    DocumentName = "Argumentum_FallacyCards_3_fr.pdf",
                    Enabled = false,
                    Translations = new List<(string sourceLang, string destLang)>(new []
                    {
	                    ("fr","en"), 
	                    ("fr", "ru")
                    }),
					CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = KnownCardSets.Rules,
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
                            CardSetName = KnownCardSets.Memo,
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
                            CardSetName = KnownCardSets.Fallacies3,
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
                    DocumentName = "Argumentum_FallacyCards_Print&Play_A4_fr.pdf",
                    Enabled = false,
                    Translations = new List<(string sourceLang, string destLang)>(new []
                    {
	                    ("fr","en"), 
	                    ("fr", "ru")
                    }),
					DocumentFormat = CardDocumentFormat.PrintAndPlay,
                    PageSize = "A4",
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = KnownCardSets.RulesPrintAndPlay,
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
                            CardSetName = KnownCardSets.MemoPrintAndPlay,
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
                            CardSetName = KnownCardSets.FallaciesPrintAndPlay,
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
                    DocumentName = "Argumentum_FallacyCards_Print&Play_A4_fr.pdf",
                    Enabled = false,
                    Translations = new List<(string sourceLang, string destLang)>(new []
                    {
	                    ("fr","en")
                    }),
					DocumentFormat = CardDocumentFormat.PrintAndPlay,
                    PageSize = "A4",
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = KnownCardSets.ScenariiPrintAndPlay,
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
                    DocumentName = "Argumentum_Fallacies_Web_Thumbnails_fr.pdf",
                    Enabled = true,
                    Translations = new List<(string sourceLang, string destLang)>(new []
                    {
	                    ("fr","en"), 
	                    ("fr", "ru")
                    }),
					DocumentFormat = CardDocumentFormat.PrintAndPlay,
                    PageSize = "A4",
                    NoBack = true,
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = KnownCardSets.FallaciesWebThumbnails,
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
                    DocumentName = "Argumentum_Fallacies_Web_A4_fr.pdf",
                    Enabled = false,
                    Translations = new List<(string sourceLang, string destLang)>(new []
                    {
	                    ("fr","en"), 
	                    ("fr", "ru")
                    }),
					DocumentFormat = CardDocumentFormat.PrintAndPlay,
                    PageSize = "A4",
                    NoBack = true,
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = KnownCardSets.FallaciesWebLight,
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
                    DocumentName = "Argumentum_Fallacies_Web_A0_fr.pdf",
                    Enabled = false,
                    Translations = new List<(string sourceLang, string destLang)>(new []
                    {
	                    ("fr","en"),
	                    ("fr", "ru")
                    }),
					DocumentFormat = CardDocumentFormat.PrintAndPlay,
                    PageSize = "A0",
                    NoBack = true,
                    Header = "Logo_Argumentum.png",
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = KnownCardSets.FallaciesWeb,
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

        public string GetBaseTargetDirectory(string language)
        {
            var toReturn =  Path.Combine(System.Environment.CurrentDirectory, BaseTargetDirectoryName);
            if (!Directory.Exists(toReturn))
            {
                Directory.CreateDirectory(toReturn);
            }
            toReturn = Path.Combine(toReturn, $"{language}\\");
            if (!Directory.Exists(toReturn))
            {
	            Directory.CreateDirectory(toReturn);
            }

			return toReturn;
        }

        public string GetHarvestDirectory(string language)
        {
            var toReturn = Path.Combine(GetBaseTargetDirectory(language), HarvestDirectoryName);
            if (!Directory.Exists(toReturn))
            {
                Directory.CreateDirectory(toReturn);
            }

            return toReturn;
        }

        public string GetImagesDirectory(string language)
        {
            var toReturn = Path.Combine(GetBaseTargetDirectory(language), ImagesDirectoryName);
            if (!Directory.Exists(toReturn))
            {
                Directory.CreateDirectory(toReturn);
            }

            return toReturn;
        }

        public string GetPdfsDirectory(string language)
        {
            var toReturn = Path.Combine(GetBaseTargetDirectory(language), PdfsDirectoryName);
            if (!Directory.Exists(toReturn))
            {
                Directory.CreateDirectory(toReturn);
            }

            return toReturn;
        }




        

        public void Apply(Stopwatch objSw)
        {
            var generator = new WebBasedGenerator(this, objSw);
            generator.Run();

        }


    }
}