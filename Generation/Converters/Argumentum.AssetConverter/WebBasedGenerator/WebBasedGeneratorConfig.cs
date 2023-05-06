using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Argumentum.AssetConverter.Mindmapper;

namespace Argumentum.AssetConverter
{

	
	


	public class WebBasedGeneratorConfig
    {


        public string CardpenUrl { get; set; }= @"https://argumentumgames.github.io/Argumentum/Generation/CardPen/index.html";
		//For local hosting http://cardpen.dnndev.me/Generation/CardPen/index.html

        public int MaxDegreeOfParallelismCardpen { get; set; } = 3;

        public int MaxDegreeOfParallelismCardpenTranslations { get; set; } = 2;

        public int MaxDegreeOfParallelismDocumentImages { get; set; } = 3;

        public int MaxDegreeOfParallelismDocumentTranslations { get; set; } = 2;

		public List<DataSetInfo> DataSets { get; set; } = new List<DataSetInfo>(
			new[]
			{
                new DataSetInfo()
                {
                    Name = KnownDataSets.Fallacies,
                    FilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum%20Fallacies%20-%20Cards.csv"
				},
                new DataSetInfo()
                {
	                Name = KnownDataSets.FallaciesPrintAndPlay,
	                FilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum%20Fallacies%20-%20Cards%20Print%20and%20Play.csv"
				},
				new DataSetInfo()
                {
	                Name = KnownDataSets.Rules,
	                FilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Rules/Argumentum%20Rules%20-%20Cards.csv"
				},
				new DataSetInfo()
				{
					Name = KnownDataSets.RulesPrintAndPlay,
					FilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Rules/Argumentum%20Rules%20-%20Cards%20Print%20and%20Play.csv"
				},
				new DataSetInfo()
                {
	                Name = KnownDataSets.Scenarii,
	                FilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Argumentum%20Scenarii%20-%20Cards.csv"
                },
                new DataSetInfo()
                {
	                Name = KnownDataSets.ScenariiPrintAndPlay,
	                FilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Argumentum%20Scenarii%20-%20Print%20and%20Play.csv"
				},



			});

		public List<CardSetConfig> CardSets { get; set; } = new List<CardSetConfig>(
			new[]
			{
				new CardSetConfig(){
					Name = KnownCardSets.Rules,
					FaceCardSetInfo = new CardSetInfo()
						{
                            DataSet = KnownDataSets.Rules,
							JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Rules/Argumentum_Rules_fr.json",
                            SkipDataUpdate = true
						}
					  },
				new CardSetConfig(){
					Name =KnownCardSets.Memo,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Fallacies,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Memo/Argumentum_Memo_Face_fr.json"
					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Fallacies,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Memo/Argumentum_Memo_Back_fr.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.Fallacies,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Fallacies,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_fr.json"

					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.None,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Back_fr.json",
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.Scenarii,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Scenarii,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Argumentum_Scenarii_Face_fr.json"
					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Scenarii,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Argumentum_Scenarii_Back_fr.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.Fallacies2,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Fallacies,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_2_fr.json"
					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.None,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Back_fr.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.Fallacies3,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Fallacies,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_3_fr.json"
					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.None,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Back_fr.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.FallaciesPrintAndPlay,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.FallaciesPrintAndPlay,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_fr.json"
					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.None,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Back_fr.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.ScenariiPrintAndPlay,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.ScenariiPrintAndPlay,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Argumentum_Scenarii_Face_fr.json"
					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.ScenariiPrintAndPlay,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Argumentum_Scenarii_Back_fr.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.RulesPrintAndPlay,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.RulesPrintAndPlay,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Rules/Argumentum_Rules_fr.json",
                        SkipDataUpdate = true,
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.MemoPrintAndPlay,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Fallacies,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Memo/Argumentum_Memo_Face_fr.json"
					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Fallacies,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Memo/Argumentum_Memo_Back_fr.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.FallaciesWeb,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Fallacies,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.FallaciesWebLight,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Fallacies,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_Web_Light_fr.json"
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.FallaciesWebThumbnails,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Fallacies,
						JsonFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_Web_Thumbnails_fr.json"
					}
				}
			});

		public LocalizationConfig LocalizationConfig { get; set; } = new LocalizationConfig()
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
						("Soussousfamille", new List<(string Language, string destFieldName)>(new []{("en", "Subsubfamily"), ("ru", "Subsubfamily_ru") }) ),
						("Sous-Famille", new List<(string Language, string destFieldName)>(new []{("en", "Subfamily"), ("ru", "Subfamily_ru") }) ),
						("Famille", new List<(string Language, string destFieldName)>(new []{("en", "Family"), ("ru", "Family_ru") }) ),
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
					}),
					ExceptionPatterns = new List<string>(new []
					{
						"{{rowset.[0].catégorie}}.jpg",
						"{{rowset.[0].catégorie}}.png"
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
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new []{
							("L'art de jamais avoir tort", new List<(string Language, string destFieldName)>(new []{("en", "The art of never being wrong"), ("ru", "Искусство никогда не ошибаться") }) ),
						}),
					FrontFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("Famille", new List<(string Language, string destFieldName)>(new []{("en", "Family"), ("ru", "Family_ru") }) ),
						("desc_fr", new List<(string Language, string destFieldName)>(new []{("en", "desc_en"), ("ru", "desc_ru") }) ),
					}),
					BackFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("Soussousfamille", new List<(string Language, string destFieldName)>(new []{("en", "Subsubfamily"), ("ru", "Subsubfamily_ru") }) ),
						("Sous-Famille", new List<(string Language, string destFieldName)>(new []{("en", "Subfamily"), ("ru", "Subfamily_ru") }) ),
						("Famille", new List<(string Language, string destFieldName)>(new []{("en", "Family"), ("ru", "Family_ru") }) ),
						("tagline_fr", new List<(string Language, string destFieldName)>(new []{("en", "tagline_en"), ("ru", "tagline_ru") }) ),
					})
				},
			}),
			MindMapLocalization = new List<DocumentLocalization>(new[]
			{
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
						{
							nameof (MindMapDocumentConfig.DocumentName),
						}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						("_fr", new List<(string Language, string destFieldName)>(new []{("en", "_en"), ("ru", "_ru") }) ),
						
					}),
				},
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
					{
						nameof(MindMapDocumentConfig.TitleExpression),
						nameof (MindMapDocumentConfig.CardExpression)
					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						("TextFr", new List<(string Language, string destFieldName)>(new []{("en", "TextEn"), ("ru", "TextRu") }) ),

					}),
				},
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
					{
						nameof (MindMapDocumentConfig.DescriptionExpression),
					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						("DescFr", new List<(string Language, string destFieldName)>(new []{("en", "DescEn"), ("ru", "DescRu") }) ),
					}),

				},
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
					{
						nameof (MindMapDocumentConfig.ExampleExpression),
					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						("ExampleFr", new List<(string Language, string destFieldName)>(new []{("en", "ExampleEn"), ("ru", "ExampleRu") }) ),
					}),
					
				},
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
					{
						nameof (MindMapDocumentConfig.LinkExpression),
					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						("LinkFrFallback", new List<(string Language, string destFieldName)>(new []{("en", "LinkEnFallback"), ("ru", "LinkRuFallback") }) ),

					})
				}
			}),
		};


		public List<CardSetDocumentConfig> CardSetDocuments { get; set; } = new List<CardSetDocumentConfig>(
            new[]
            {
                new CardSetDocumentConfig()
                {
                    DocumentName = "Argumentum_TarotCards_fr.pdf",
                    Enabled = true,
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
                new CardSetDocumentConfig()
                {
                    DocumentName = "Argumentum_PokerCards_fr.pdf",
                    Enabled = true,
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
                 new CardSetDocumentConfig()
                {
                    DocumentName = "Argumentum_TarotCards_Print&Play_A4_fr.pdf",
                    Enabled = true,
                    Translations = new List<(string sourceLang, string destLang)>(new []
                    {
	                    ("fr","en"), 
	                    //("fr", "ru")
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
                            SaveOriginalImage = false,
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
                            SaveOriginalImage = false,
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
                            SaveOriginalImage = false,
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
                new CardSetDocumentConfig()
                {
                    DocumentName = "Argumentum_PokerCards_Print&Play_A4_fr.pdf",
                    Enabled = true,
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
                            SaveOriginalImage = false,
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
                new CardSetDocumentConfig()
                {
                    DocumentName = "Argumentum_Fallacies_Web_A4_fr.pdf",
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
                new CardSetDocumentConfig()
                {
                    DocumentName = "Argumentum_Fallacies_Web_A0_fr.pdf",
                    Enabled = true,
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
                            SaveOriginalImage = false,
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
                },
                new CardSetDocumentConfig()
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
			                SaveOriginalImage = false,
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
				new CardSetDocumentConfig()
				{
					DocumentName = "Argumentum_TarotCards_2_fr.pdf",
					Enabled = true,
                    //Translations = new List<(string sourceLang, string destLang)>(new []
                    //{
	                   // ("fr","en"), 
	                   // ("fr", "ru")
                    //}),
					CardSets = new List<DocumentCardSet>(new[]
					{
						new DocumentCardSet()
						{
							CardSetName = KnownCardSets.Rules,
							NbCopies = 1,
							ConvertToCmyk = true,
							SaveOriginalImage = false,
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
							SaveOriginalImage = false,
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
							SaveOriginalImage = false,
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
				new CardSetDocumentConfig()
				{
					DocumentName = "Argumentum_TarotCards_3_fr.pdf",
					Enabled = true,
                    //Translations = new List<(string sourceLang, string destLang)>(new []
                    //{
	                   // ("fr","en"), 
	                   // ("fr", "ru")
                    //}),
					CardSets = new List<DocumentCardSet>(new[]
					{
						new DocumentCardSet()
						{
							CardSetName = KnownCardSets.Rules,
							NbCopies = 1,
							ConvertToCmyk = true,
							SaveOriginalImage = false,
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
							SaveOriginalImage = false,
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
							SaveOriginalImage = false,
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
				}
			});


		public List<MindMapDocumentConfig> MindMapDocuments { get; set; } = new List<MindMapDocumentConfig>(new []
			{
				new MindMapDocumentConfig()
				{
					DocumentName = "Argumentum_Fallacies_MindMap_fr.mm",
					Translations = new List<(string sourceLang, string destLang)>(new []
					{
						("fr","en"),
						("fr", "ru")
					}),
				},
				new MindMapDocumentConfig()
				{
					DocumentName = "Argumentum_Fallacies_MindMap_cards_fr.mm",
					InsertCards = true,
					Translations = new List<(string sourceLang, string destLang)>(new []
					{
						("fr","en"),
						("fr", "ru")
					}),
				}
			}
			);

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