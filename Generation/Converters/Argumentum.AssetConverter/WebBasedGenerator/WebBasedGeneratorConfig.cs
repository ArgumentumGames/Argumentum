using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using ImageMagick;

namespace Argumentum.AssetConverter
{





    public class WebBasedGeneratorConfig
	{

		

		public bool ShowInfoLogs { get; set; } = true;

		public bool HeadLessBrowser { get; set; }

		



		public int MaxDegreeOfParallelismCardpen { get; set; } = 3;

		public int MaxDegreeOfParallelismCardpenTranslations { get; set; } = 2;

		public int MaxDegreeOfParallelismImages { get; set; } = 3;

		public int MaxDegreeOfParallelismImageTranslations { get; set; } = 2;

		public int MaxDegreeOfParallelismDocuments { get; set; } = 4;

		


	

		



		public string ReleaseCardpenUrl { get; set; } = @"https://argumentumgames.github.io/Argumentum/Generation/CardPen/index.html";
		public string LocalCardpenUrl { get; set; } = @"http://cardpen.dnndev.me/Generation/CardPen/index.html";



		public bool UseLocalCardpen { get; set; }



		[IgnoreDataMember]
		[JsonIgnore]
		public string CardpenUrl => UseLocalCardpen ? LocalCardpenUrl : ReleaseCardpenUrl;

		

		

		//private string GetSimpleTypeName(Type objType)
		//{
		//	return $"{objType.FullName}, {objType.Assembly.GetName().Name}";
		//}


		public List<CardSetConfig> CardSets { get; set; } = new List<CardSetConfig>(
			new[]
			{
				new CardSetConfig(){
					Name = KnownCardSets.Rules,
					FaceCardSetInfo = new CardSetInfo()
						{
							DataSet = KnownDataSets.Rules,
							JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Rules/Argumentum_Rules_fr.json",
							JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Rules\Argumentum_Rules_fr.json",
							SkipDataUpdate = false
						}
					  },
				new CardSetConfig(){
					Name =KnownCardSets.Memo,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.FallaciesTaxonomy,
						CsvFilterField = "carte",
						CsvFilterValues = new List<string>(new []
						{
							"1",
							"2"
						}),
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Memo/Argumentum_Memo_Face_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Memo\Argumentum_Memo_Face_fr.json",
					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.FallaciesTaxonomy,
						CsvFilterField = "carte",
						CsvFilterValues = new List<string>(new []
						{
							"1",
							"2"
						}),
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Memo/Argumentum_Memo_Back_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Memo\Argumentum_Memo_Back_fr.json",
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.Fallacies,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.FallaciesTaxonomy,
						CsvFilterField = "carte",
						CsvFilterValues = new List<string>(new []
						{
							"1",
							"2"
						}),
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum_Fallacies_Face_fr.json"

					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.None,
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Back_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum_Fallacies_Back_fr.json",
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.Virtues,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.VirtuesTaxonomy,
						CsvFilterField = "card",
						CsvFilterValues = new List<string>(new []
						{
							"1",
						}),
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Virtues_Face_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum_Virtues_Face_fr.json"

					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.None,
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Back_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum_Fallacies_Back_fr.json",
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.Scenarii,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Scenarii,
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Argumentum_Scenarii_Face_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Scenarii\Argumentum_Scenarii_Face_fr.json",
					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Scenarii,
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Argumentum_Scenarii_Back_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Scenarii\Argumentum_Scenarii_Back_fr.json",
						RowsetNb = 14
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.Fallacies2,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.FallaciesTaxonomy,
						CsvFilterField = "carte",
						CsvFilterValues = new List<string>(new []
						{
							"1",
							"2"
						}),
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_2_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum_Fallacies_Face_2_fr.json",
					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.None,
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Back_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum_Fallacies_Back_fr.json",
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.Fallacies3,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.FallaciesTaxonomy,
						CsvFilterField = "carte",
						CsvFilterValues = new List<string>(new []
							{
								"1",
								"2"
							}),
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_3_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum_Fallacies_Face_3_fr.json",


					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.None,
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Back_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum_Fallacies_Back_fr.json",
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.FallaciesPrintAndPlay,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.FallaciesTaxonomy,
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum_Fallacies_Face_fr.json",
						CsvFilterField = "print_and_play",
						CsvFilterValues = new List<string>(new []
						{
							"1",
						})

					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.None,
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Back_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum_Fallacies_Back_fr.json",
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.ScenariiPrintAndPlay,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Scenarii,
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Argumentum_Scenarii_Face_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Scenarii\Argumentum_Scenarii_Face_fr.json",
						CsvFilterField = "print_and_play",
						CsvFilterValues = new List<string>(new []
						{
							"1",
						})
					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.Scenarii,
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Argumentum_Scenarii_Back_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Scenarii\Argumentum_Scenarii_Back_fr.json",
						CsvFilterField = "print_and_play",
						CsvFilterValues = new List<string>(new []
							{
								"1",
							}),
						RowsetNb = 4
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.RulesPrintAndPlay,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.RulesPrintAndPlay,
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Rules/Argumentum_Rules_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Rules\Argumentum_Rules_fr.json",
						SkipDataUpdate = false,
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.MemoPrintAndPlay,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.FallaciesTaxonomy,
						CsvFilterField = "carte",
						CsvFilterValues = new List<string>(new []
						{
							"1",
							"2"
						}),
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Memo/Argumentum_Memo_Face_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Memo\Argumentum_Memo_Face_fr.json",
					},
					BackCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.FallaciesTaxonomy,
						CsvFilterField = "carte",
						CsvFilterValues = new List<string>(new []
						{
							"1",
							"2"
						}),
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Memo/Argumentum_Memo_Back_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Memo\Argumentum_Memo_Back_fr.json",
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.FallaciesWeb,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.FallaciesTaxonomy,
						CsvFilterField = "carte",
						CsvFilterValues = new List<string>(new []
						{
							"1",
							"2"
						}),
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum_Fallacies_Face_Web_fr.json",
						Dpi = 400
					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.FallaciesWebLight,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.FallaciesTaxonomy,
						CsvFilterField = "carte",
						CsvFilterValues = new List<string>(new []
						{
							"1",
							"2"
						}),
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum_Fallacies_Face_Web_fr.json",
						Dpi = 200

					}
				},
				new CardSetConfig(){
					Name =KnownCardSets.FallaciesWebThumbnails,
					FaceCardSetInfo = new CardSetInfo()
					{
						DataSet = KnownDataSets.FallaciesTaxonomy,
						CsvFilterField = "carte",
						CsvFilterValues = new List<string>(new []
						{
							"1",
							"2"
						}),
						JsonFilePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json",
						JsonFilePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum_Fallacies_Face_Web_fr.json",
						Dpi = 100
					}
				}
			});


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
						("fr", "ru"),
						("fr", "pt")
					}),
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
							CardSetName = KnownCardSets.Fallacies,
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
						}
					}),
				},
				new CardSetDocumentConfig()
				{
					DocumentName = "Argumentum_TarotCards_Virtues_fr.pdf",
					Enabled = true,
					Translations = new List<(string sourceLang, string destLang)>(),
					CardSets = new List<DocumentCardSet>(new[]
					{
						new DocumentCardSet()
						{
							CardSetName = KnownCardSets.Virtues,
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
					DocumentName = "Argumentum_PokerCards_fr.pdf",
					Enabled = true,
					Translations = new List<(string sourceLang, string destLang)>(new []
					{
						("fr","en"),
						("fr", "ru"),
						("fr", "pt")
					}),
					CardSets = new List<DocumentCardSet>(new[]
					{
						new DocumentCardSet()
						{
							CardSetName = KnownCardSets.Scenarii,
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
					DocumentName = "Argumentum_TarotCards_Print&Play_A4_fr.pdf",
					Enabled = true,
					Translations = new List<(string sourceLang, string destLang)>(new []
					{
						("fr","en"), 
	                    ("fr", "ru"),
	                    ("fr", "pt")
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
						("fr","en"),
						("fr", "ru"),
						("fr", "pt")
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
						("fr", "ru"),
						("fr", "pt")
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
							SaveOriginalImage = false,
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
						("fr", "ru"),
						("fr", "pt")
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
						("fr", "ru"),
						("fr", "pt")
					}),
					DocumentFormat = CardDocumentFormat.PrintAndPlay,
					PageSize = "A4",
					ImageFormat = MagickFormat.Png,
					TargetDensity = 0,
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


		



		

		

		


		public async Task<bool> Apply(AssetConverterConfig config)
		{
			var generator = new WebBasedGenerator(){AssetConverterConfig = config, Config = this};
			await generator.Run().ConfigureAwait(false);
			return true;
		}


	}
}