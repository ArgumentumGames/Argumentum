﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using ImageMagick;
using Spectre.Console;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using Color = System.Drawing.Color;
using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter.Mindmapper
{
    public class MindMapDocumentConfig : FallacyDocumentConfigBase
	{
		// Trying to making sure the assemblies get published
		private static readonly System.Diagnostics.StackTrace temp1 = new();
		private static readonly System.Drawing.Color temp2 = Color.AliceBlue;

		//public string DocumentName { get; set; } = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2.mm";



		const string DefaultTitleExpression = @"{fallacy.TextFr}";

		public MindMapFormat Format { get; set; } = MindMapFormat.Freemind;

		public string TitleExpression { get; set; } = DefaultTitleExpression;

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<Fallacy, string> TitleFunc
		{
			get
			{
				return fallacy =>
				{
					
					var title = TitleExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } });
					if (AddNodePath)
					{
						title = $"{fallacy.Path} - {title}";
					}
					return title;
				}; 
			}
		}

		public bool AddNodePath { get; set; } = false;


		const string DefaultFamilleExpression = @"{fallacy.Famille}";
		public string FamilleExpression { get; set; } = DefaultFamilleExpression;

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<Fallacy, string> FamilleFunc
		{
			get
			{
				return fallacy => FamilleExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"{fallacy.TextFr}";
			}
		}


		const string DefaultSousFamilleExpression = @"{fallacy.SousFamille}";
		public string SousFamilleExpression { get; set; } = DefaultSousFamilleExpression;

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<Fallacy, string> SousFamilleFunc
		{
			get
			{
				return fallacy => SousFamilleExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"{fallacy.TextFr}";
			}
		}


		const string DefaultSoussousFamilleExpression = @"{fallacy.Soussousfamille}";
		public string SoussousFamilleExpression { get; set; } = DefaultSoussousFamilleExpression;

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<Fallacy, string> SoussousFamilleFunc
		{
			get
			{
				return fallacy => SoussousFamilleExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"{fallacy.TextFr}";
			}
		}

		public string DescriptionExpression { get; set; } =
@"
<p>
    {HttpUtility.HtmlEncode(fallacy.DescFr)}
</p>
";

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<Fallacy, string> DescFunc
		{
			get
			{
				return fallacy => DescriptionExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"<font size='4'>{HttpUtility.HtmlEncode(fallacy.DescFr)}</font>";
			}
		}


		public string CardExpression { get; set; } =
			@"
<p>
    <img src=""{mindMap.GetThumbnailsPath(fallacy)}"" width=""60"" height=""60""/>" + DefaultTitleExpression + @"
</p>
";


		[IgnoreDataMember]
		[JsonIgnore]
		public Func<Fallacy, string> CardFunc
		{
			get
			{
				return fallacy => CardExpression.Interpolate(new Dictionary<string, object>() { { "mindMap", this }, { "fallacy", fallacy } }); // $"<font size='4'>{HttpUtility.HtmlEncode(fallacy.DescFr)}</font>";
			}
		}



		public string ExampleExpression { get; set; } =
@"
<p>
    <i>{HttpUtility.HtmlEncode(fallacy.ExampleFr)}</i>
</p>
";


		[IgnoreDataMember]
		[JsonIgnore]
		public Func<Fallacy, string> ExampleFunc
		{
			get
			{
				return fallacy => ExampleExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"<i>{HttpUtility.HtmlEncode(fallacy.ExampleFr)}</i>";
			}
		}



		public string LinkExpression { get; set; } = @"{fallacy.LinkFrFallback}";


		[IgnoreDataMember]
		[JsonIgnore]
		public Func<Fallacy, string> LinkFunc
		{
			get
			{
				return fallacy => LinkExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"{fallacy.LinkFr}";
			}
		}






		//public string ImagePathExpression { get; set; } = @"../../bin/Debug/netcoreapp3.1/Target/Images/density-0/Fallacies-Web-Thumbnails/{fallacy.FileName}.png";
		public string ThumbnailsPathExpression { get; set; } = @"../../bin/Debug/netcoreapp3.1/Target/Images/density-0/Fallacies-Web-Thumbnails/argumentum_{fallacy.Path}_{fallacy.TextFr.ToLower().Replace("" "",""_"")}.png";


		public string GetThumbnailsPath(Fallacy fallacy)
		{
			return ThumbnailsPathFunc(fallacy);
		}


		private Func<Fallacy, string> _Thumbnails;
		


		[IgnoreDataMember]
		[JsonIgnore]
		public Func<Fallacy, string> ThumbnailsPathFunc
		{
			get
			{
				return _Thumbnails ??= fallacy =>
					ThumbnailsPathExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"{fallacy.TextFr}";
			}
			set
			{
				_Thumbnails = value;
			}
		}


		public int NbBranchesRight { get; set; } = 2;

		public Dictionary<int, string> Colors { get; set; } = new Dictionary<int, string>()
		{
			{1, "#8605ab"},
			{2, "#ff66eb"},
			{3, "#08af93"},
			{4, "#8dc801"},
			{5, "#0054a4"},
			{6, "#ffc307"},
			{7, "#dc0f0a"},

		};


		public List<int> FontSizes { get; set; } = new List<int>(new[] { 60, 60, 50, 40, 30, 30, 25, 23, 23, 23, 23 });


		public List<int> EdgeSizes { get; set; } = new List<int>(new[] { 20, 10, 5, 1});


		public bool InsertCardsThumbnails { get; set; }


		public string ThumbnailsCardSetName { get; set; }

		public string ThumbnailsFileNamePattern { get; set; } = "_{fallacy.Path}..";


		public List<SVGFreemindMap> SVGMaps { get; set; } = new List<SVGFreemindMap>();

		public bool KeepOriginalSVG { get; set; } = true;

		public CrossLink CrossLinks { get; set; } = CrossLink.None;

		public string MatchThumbnailsName(string targetDirectory, Fallacy fallacy)
		{
			var fileNames = Directory.GetFiles(targetDirectory);
			var thumbnailsFallacyPattern = ThumbnailsFileNamePattern.Interpolate(
				new Dictionary<string, object>() { { "fallacy", fallacy } });
			return fileNames.FirstOrDefault(fileName => fileName.Contains(thumbnailsFallacyPattern));
		}


		public override async Task GenerateFallacyFile(IList<Fallacy> fallacies, AssetConverterConfig config, string targetDirectory, string language)
		{
			if (string.IsNullOrEmpty(language)) 
				language=config.LocalizationConfig.DefaultLanguage ;


			var fileName = DocumentName;
			if (!string.IsNullOrEmpty(targetDirectory))
			{
				fileName = Path.Combine(targetDirectory, fileName);

			}
			var documentPath = Path.Combine(targetDirectory, DocumentName);

			CreateFreemindmap(fallacies, config, language, documentPath, fileName);

			//Task.Run(async () => await ProcessSVGFiles(fallacies, fileName, webBasedGeneratorConfig, webBasedGeneratorConfig.EnableSVGPrompt)).GetAwaiter().GetResult() ;
			await ProcessSvgFilesAsync(fallacies, fileName, config, config.EnableSVGPrompt, language);
		}

		private void CreateFreemindmap(IList<Fallacy> fallacies, AssetConverterConfig config, string language, string documentPath, string fileName)
		{
			if (File.Exists(documentPath) && !config.OverwriteExistingDocs)
			{
				Logger.Log($"Skip existing Mindmap: {documentPath}");
			}
			else
			{
				Logger.Log($"Creating Freemind mind map {DocumentName}");
				FreemindMap freemindMap;
				if (Format == MindMapFormat.Freeplane)
				{
					freemindMap = new FreeplaneMap();
				}
				else
				{
					freemindMap = new FreemindMap();
				}
				
				var nodesByPath = new Dictionary<string, Node>(fallacies.Count);
				CreateFallacyNodes(freemindMap, fallacies, nodesByPath, config, language);


				SerializeMindMapAsync(freemindMap, fileName);
			}
		}


		private void CreateFallacyNodes(FreemindMap freemindMap, IList<Fallacy> fallacies, Dictionary<string, Node> nodesByPath, AssetConverterConfig config, string language)
		{
			var linkedFallacies = new HashSet<Fallacy>();

			foreach (var fallacy in fallacies)
			{
				linkedFallacies.Add(fallacy);
				if (string.IsNullOrEmpty(fallacy.PK)) continue;

				var localPath = fallacy.Path;

				List<(CrossLink crossLinkType, List<Fallacy> targets)> crossLinks = new();

				if (this.CrossLinks.HasFlag(CrossLink.Identity))
				{
					var identityFallacies = fallacies.Where(f => f.TextFr == fallacy.TextFr && !linkedFallacies.Contains(f)).ToList();
					crossLinks.Add((CrossLink.Identity, identityFallacies));
				}

				var fallacyNode = CreateNode(fallacy, config, language, crossLinks.ToArray());
				nodesByPath[localPath] = fallacyNode;

				var lastDotIndex = localPath.LastIndexOf('.');
				int familyNb;
				if (lastDotIndex > -1)
				{
					familyNb = int.Parse(fallacy.Path[0].ToString(), CultureInfo.InvariantCulture);
					var parentPath = localPath[..lastDotIndex];
					var parentNode = nodesByPath[parentPath];
					parentNode.Nodes.Add(fallacyNode);
				}
				else
				{
					familyNb = int.Parse(localPath);
					AddNodeToFreemindMap(freemindMap, fallacyNode, familyNb);
				}

				SetNodeStyle(fallacyNode, fallacy, familyNb);
			}
		}

		

		private Node CreateNode(Fallacy fallacy, AssetConverterConfig config, string language, params (CrossLink crossLinkType, List<Fallacy> targets)[] crossLinks)
		{
			var fallacyNode = new Node { TEXT = TitleFunc(fallacy) };
			fallacyNode.ID = fallacy.Id;
			var link = LinkFunc(fallacy);
			if (!string.IsNullOrEmpty(link))
			{
				fallacyNode.LINK = link;
			}

			var descRichContent = CreateRichContent(fallacy);
			fallacyNode.Richcontents.Add(descRichContent);

			if (fallacy.Carte.HasValue)
			{
				AddCardIcon(fallacy, fallacyNode, config, language);
			}

			foreach (var crossLink in crossLinks)
			{
				foreach (var target in crossLink.targets)
				{
					var crossLinkNode = new Arrowlink();
					crossLinkNode.StartArrow = "Default";
					crossLinkNode.EndArrow = "Default";
					crossLinkNode.StartInclination = "892;0;";
					crossLinkNode.EndInclination = "892;0;";
					crossLinkNode.Destination = target.Id;

					switch (crossLink.crossLinkType)
					{
						case CrossLink.Identity:
							crossLinkNode.Color = "#dbffd6 ";
							break;
						case CrossLink.AppealTo:
							crossLinkNode.Color = "#ccffff";
							break;
						case CrossLink.Opposite:
							crossLinkNode.Color = "#ffcfcc";
							break;
						default:
							throw new ArgumentOutOfRangeException($"cross link type {crossLink.crossLinkType} unsupported");
					}
					fallacyNode.Arrowlinks.Add(crossLinkNode);

				}
			}


			return fallacyNode;
		}

		private Richcontent CreateRichContent(Fallacy fallacy)
		{
			var descDoc = new XmlDocument();
			descDoc.LoadXml($"{DescFunc(fallacy)}");

			var descRichContent = new Richcontent { TYPE = "NOTE" };
			descRichContent.Html.Body.Elements.Add(descDoc.DocumentElement);

			descDoc.LoadXml($"{ExampleFunc(fallacy)}");
			descRichContent.Html.Body.Elements.Add(descDoc.DocumentElement);

			return descRichContent;
		}

		private void AddNodeToFreemindMap(FreemindMap freemindMap, Node fallacyNode, int familyNb)
		{
			if (familyNb == 0)
			{
				fallacyNode.ID = "ID_706669011";
				freemindMap.Node = fallacyNode;
			}
			else
			{
				fallacyNode.POSITION = familyNb > NbBranchesRight && familyNb <= 6 ? "left" : "right";
				freemindMap.Node.Nodes.Add(fallacyNode);
			}
		}

		private void SetNodeStyle(Node fallacyNode, Fallacy fallacy, int familyNb)
		{
			if (fallacy.Depth < FontSizes.Count)
			{
				fallacyNode.Font = new Font() { Size = FontSizes[fallacy.Depth].ToString() };
			}

			if (fallacy.Depth < EdgeSizes.Count)
			{

				

				if (familyNb > 0)
				{
					fallacyNode.Edge = new Edge() { WIDTH = EdgeSizes[fallacy.Depth - 1].ToString(CultureInfo.InvariantCulture) };
					fallacyNode.Edge.COLOR = Colors[familyNb];
					fallacyNode.BACKGROUND_COLOR = HLSColor.GetLighterColor(Colors[familyNb]);
				}

				fallacyNode.STYLE = "bubble";
			}
			else
			{
				fallacyNode.STYLE = "fork";
				if (fallacy.Depth == EdgeSizes.Count)
				{
					fallacyNode.Edge = new Edge() { WIDTH = EdgeSizes[fallacy.Depth - 1].ToString(CultureInfo.InvariantCulture) };
				}
			}

			if (fallacy.Depth <= EdgeSizes.Count)
			{
				fallacyNode.Font.BOLD = "true";
			}

			if (fallacy.Depth >= EdgeSizes.Count)
			{
				fallacyNode.COLOR = HLSColor.GetDarkerColor(Colors[familyNb]);
			}
		}

		private void AddCardIcon(Fallacy fallacy, Node fallacyNode, AssetConverterConfig assetConverterConfig, string language)
		{
			fallacyNode.Icons.Add(new Icon() { BUILTIN = $"full-{fallacy.Carte}" });

			if (InsertCardsThumbnails )
			{
				var cardSetConfig = assetConverterConfig.WebBasedGeneratorConfig.CardSets.FirstOrDefault(c => c.Name == this.ThumbnailsCardSetName, null);
				if (cardSetConfig != null)
				{
					this.ThumbnailsPathFunc = objFallacy =>
					{
						var cardSetDirectory = ImageHelper.GetImageFolder(assetConverterConfig, this, language, ThumbnailsCardSetName);
						var imageFileName = MatchThumbnailsName(cardSetDirectory, fallacy);
						if (string.IsNullOrEmpty(imageFileName))
						{
							Logger.LogProblem($"No thumbnail for fallacy {TitleFunc(fallacy)} in directory {cardSetDirectory}");
						}
						else
						{
							var targetDirectory = assetConverterConfig.GetDocumentDirectory(language);
							imageFileName = imageFileName.GetRelativePathFrom(targetDirectory);
						}
						return imageFileName;
					};
				}

				var cardDoc = new XmlDocument();
				cardDoc.LoadXml($"{CardFunc(fallacy)}");
				var cardRichContent = new Richcontent();
				fallacyNode.Richcontents.Add(cardRichContent);
				cardRichContent.TYPE = "NODE";
				cardRichContent.Html.Body.Elements.Add(cardDoc.DocumentElement);
			}
		}


		
		private static void SerializeMindMapAsync(FreemindMap toReturn, string fileName)
		{
			var serializer = new XmlSerializer(typeof(FreemindMap));

			using (var fs = File.Create(fileName))
			{
				XmlWriterSettings writerSettings = new() { Indent = true, OmitXmlDeclaration = true };
				using var writer = XmlWriter.Create(fs, writerSettings);
				serializer.Serialize(writer, toReturn);
			}


			Logger.LogSuccess($"Mind map {fileName} successfully generated!");
		}
		private async Task ProcessSvgFilesAsync(IList<Fallacy> fallacies, string fileName,
			AssetConverterConfig webBasedGeneratorConfig, bool enableSvgUpdates, string language)
		{
			string svgFilePath = Path.ChangeExtension(fileName, "svg");


			foreach (var svgFreemindMap in SVGMaps)
			{
				var svgSavedFilePath = Path.ChangeExtension(svgFilePath, svgFreemindMap.DocumentName);
				Func<Task<string>> svgLoader;
				if (File.Exists(svgSavedFilePath) && !webBasedGeneratorConfig.OverwriteExistingDocs)
				{
					Logger.Log($"Skipping existing processed SVG: {svgSavedFilePath}");
					svgLoader = async () => await File.ReadAllTextAsync(svgSavedFilePath);
				}
				else
				{

					if (!File.Exists(svgFilePath))
					{
						if (enableSvgUpdates)
						{
							await DisplaySvgFileNotFoundMessage(svgFilePath);
						}
						else
						{
							Logger.LogWarning($"File {svgFilePath} not found and skipped. Switch \"EnableSVGPrompt\" on for Freemind-assisted SVG generation.");
						}
					}

					if (!File.Exists(svgFilePath))
					{
						return;
					}
					XDocument svgDoc = XDocument.Load(svgFilePath);

					if (!string.IsNullOrEmpty(svgFreemindMap.SvgViewBox))
					{
						svgDoc.Root.SetAttributeValue("viewBox", svgFreemindMap.SvgViewBox);
					}
					if (!string.IsNullOrEmpty(svgFreemindMap.SvgWidth))
					{
						svgDoc.Root.SetAttributeValue("width", svgFreemindMap.SvgWidth);
					}
					if (!string.IsNullOrEmpty(svgFreemindMap.SvgHeight))
					{
						svgDoc.Root.SetAttributeValue("height", svgFreemindMap.SvgHeight);
					}

					XNamespace svgNamespace = "http://www.w3.org/2000/svg";
					XNamespace xlinkNamespace = "http://www.w3.org/1999/xlink";

					UpdateSvgWithFallacies(svgFreemindMap, fallacies, svgDoc, svgNamespace, xlinkNamespace);


					svgLoader = () => Task.FromResult(GetSvgContent(svgDoc));


					await File.WriteAllTextAsync(svgSavedFilePath, await svgLoader(), Encoding.UTF8);
					Logger.LogSuccess($"SVG file with detailed content {svgSavedFilePath} successfully saved");

				}

				await GenerateHtmlSvgWrappers(svgFreemindMap, webBasedGeneratorConfig, svgSavedFilePath, svgLoader, language);

				

			}

			if (!this.KeepOriginalSVG && File.Exists(svgFilePath))
			{
				File.Delete(svgFilePath);
			}

		}

		//private void AdjustSvgViewBox(XDocument svgDoc)
		//{
		//	double minX = double.MaxValue, minY = double.MaxValue;
		//	double maxX = double.MinValue, maxY = double.MinValue;

		//	// Assumer une logique simplifiée pour le traitement des éléments <g> avec translate
		//	var gElements = svgDoc.Descendants().Where(el => el.Name.LocalName == "g");
		//	foreach (var gEl in gElements)
		//	{
		//		var transform = gEl.Attribute("transform")?.Value;
		//		double translateX = 0, translateY = 0;
		//		if (transform != null && transform.StartsWith("translate"))
		//		{
		//			var translateValues = transform.Split('(')[1].Split(')')[0].Split(',');
		//			translateX = double.Parse(translateValues[0]);
		//			translateY = double.Parse(translateValues[1]);
		//		}

		//		// Traiter les éléments enfants comme <text> et <image>
		//		foreach (var child in gEl.Elements())
		//		{
		//			double x = 0, y = 0, width = 0, height = 0;
		//			switch (child.Name.LocalName)
		//			{
		//				case "text":
		//					x = double.Parse(child.Attribute("x")?.Value ?? "0") + translateX;
		//					y = double.Parse(child.Attribute("y")?.Value ?? "0") + translateY;
		//					// Approximation: le texte n'a pas de width/height explicite, ajustement simplifié
		//					minX = Math.Min(minX, x);
		//					minY = Math.Min(minY, y);
		//					break;
		//				case "image":
		//					x = double.Parse(child.Attribute("x")?.Value ?? "0") + translateX;
		//					y = double.Parse(child.Attribute("y")?.Value ?? "0") + translateY;
		//					width = double.Parse(child.Attribute("width")?.Value ?? "0");
		//					height = double.Parse(child.Attribute("height")?.Value ?? "0");
		//					minX = Math.Min(minX, x);
		//					minY = Math.Min(minY, y);
		//					maxX = Math.Max(maxX, x + width);
		//					maxY = Math.Max(maxY, y + height);
		//					break;
		//					// Ajouter des cas pour d'autres types d'éléments ici
		//			}
		//		}
		//	}

		//	// Ajuster les dimensions de la viewBox
		//	string viewBoxValue = $"{minX} {minY} {maxX - minX} {maxY - minY}";
		//	svgDoc.Root.SetAttributeValue("viewBox", viewBoxValue);
		//}



		private static async Task DisplaySvgFileNotFoundMessage(string svgFilePath)
		{
			Logger.LogInstructions($"SVG mindmap {svgFilePath} was not found.\n Please download open-source software freemind to generate a SVG export from the original .mm file.\n" +
			                       $"[link]https://sourceforge.net/projects/freemind/[/]\nSvg export will be further edited to include fields and links\nPress any key to resume and update or skip the SVG file...");

			await UtilityExtensions.ConsoleKeyPressAsync();
			//await UtilityExtensions.KeyPressSemaphore.WaitAsync();
		}





		private void UpdateSvgWithFallacies(SVGFreemindMap svgMap, IList<Fallacy> fallacies, XDocument svgDoc, XNamespace svgNamespace, XNamespace xlinkNamespace)
		{


			var fallacyToSVGNodes = CollectPossibleSvgNodes(fallacies, svgDoc, svgNamespace);
			var disambiguatedFallacyToSVGNode = DisambiguateSvgNodes(fallacyToSVGNodes, fallacies, svgNamespace);
			var warned = false;
			foreach (var pair in disambiguatedFallacyToSVGNode)
			{
				UpdateSvgMatch(svgMap, pair.Value, pair.Key, svgNamespace, xlinkNamespace, ref warned);
			}

			// Optionally remove all SVG images
			if (svgMap.RemoveImages)
			{
				switch (this.Format)
				{
					case MindMapFormat.Freemind:
						var imageTags = svgDoc.Descendants(svgNamespace + "image").ToList();
						imageTags = imageTags.Where(i => i.Attributes("width").All(wAttr => wAttr.Value != "60")).ToList();
						imageTags.Remove();
						break;
					case MindMapFormat.Freeplane:
						var iconGroups = svgDoc.Descendants(svgNamespace + "g").Where(g => g.Elements(svgNamespace + "path").Any(x => x.Attributes("stroke").Any(att => att.Value == "none"))).ToList();
						iconGroups.Remove();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				
			}
		}



		private Dictionary<Fallacy, List<XElement>> CollectPossibleSvgNodes(IList<Fallacy> fallacies, XDocument svgDoc, XNamespace svgNamespace)
		{
			Dictionary<Fallacy, List<XElement>> fallacyToSvgNodes = new();
			var textGroups = svgDoc.Descendants(svgNamespace + "g").Where(g => g.Elements(svgNamespace + "text").Any()).ToList();

			foreach (var fallacy in fallacies)
			{
				string title = TitleFunc(fallacy);
				var matchingGroups = textGroups.Where(g => string.Join("", g.Elements(svgNamespace + "text").Select(t => t.Value)).Contains(title)).ToList();

				if (matchingGroups.Any())
				{
					// Group the g elements by the length of their text content
					var groupedGroups = matchingGroups.GroupBy(g => string.Join("", g.Elements(svgNamespace + "text").Select(t => t.Value)).Length);

					// Get the minimum length among the groups
					var groups = groupedGroups as IGrouping<int, XElement>[] ?? groupedGroups.ToArray();
					var minLength = groups.Min(g => g.Key);

					// Retain only the g elements with the minimum length
					var minLengthGroups = groups.First(g => g.Key == minLength).ToList();

					fallacyToSvgNodes[fallacy] = minLengthGroups;
				}
				else
				{
					var closeMatches = textGroups.Where(g => string.Join("", g.Elements(svgNamespace + "text").Select(t => t.Value)).Contains(title.Substring(0, 3))).ToList();
					var closeMatchesMessages = closeMatches.Select(g => string.Join(" ", g.Elements(svgNamespace + "text").Select(t => t.Value))).ToList().Aggregate("", (s1, s2) => $"{s1}\n{s2}");
					Logger.LogProblem($"Could not find Svg node for fallacy {TitleFunc(fallacy)}\nClose matches:\n{closeMatchesMessages}");
				}
			}

			return fallacyToSvgNodes;
		}


		private Dictionary<Fallacy, XElement> DisambiguateSvgNodes(
			Dictionary<Fallacy, List<XElement>> fallacyToSvgNodes, IList<Fallacy> fallacies, XNamespace svgNamespace)
		{
			var tempNode = fallacyToSvgNodes.First().Value.First();
			var allNodesList = tempNode.Document.Descendants(svgNamespace + tempNode.Name.LocalName).ToList();
			var nodeIndices = allNodesList.Select((n, i) => new { Node = n, Index = i }).ToDictionary(n => n.Node, n => n.Index);
			//checking that all nodes have an index
			foreach (var fallacyToSvgNode in fallacyToSvgNodes)
			{
				foreach (var svgNode in fallacyToSvgNode.Value)
				{
					if (!nodeIndices.TryGetValue(svgNode, out int tempIndex))
					{
						Debugger.Break();
					}
				}
			}
			Dictionary<Fallacy, XElement> disambiguatedFallacyToSvgNode = new();
			Dictionary<XElement, Fallacy> svgNodeToFallacy = new();

			foreach (var pair in fallacyToSvgNodes)
			{
				Fallacy fallacy = pair.Key;
				List<XElement> candidateSvgNodes = pair.Value;

				if (candidateSvgNodes.Count == 1)
				{
					var candidate = candidateSvgNodes.First();
					disambiguatedFallacyToSvgNode[fallacy] = candidate;
					svgNodeToFallacy[candidate] = fallacy;
				}
				else
				{
					string parentDecimalPath = fallacy.DecimalPath.Remove(fallacy.DecimalPath.Length == 3 ? 1: fallacy.DecimalPath.Length - 1);
					var parentFallacyCandidates = fallacies.Where(f => f.DecimalPath == parentDecimalPath).ToArray();
					if (parentFallacyCandidates.Length == 0)
					{
						Logger.LogProblem($"Parent fallacy not found for {TitleFunc(fallacy)} - {fallacy.Path}");
						break;
					}

					var parentFallacy = parentFallacyCandidates.First();

					if (!disambiguatedFallacyToSvgNode.TryGetValue(parentFallacy, out var parentSvgNode))
					{
						if (fallacyToSvgNodes.TryGetValue(parentFallacy, out List<XElement> parentSvgNodes))
						{
							if (parentSvgNodes.Count > 1)
							{
								Logger.LogProblem($"Could not disambiguate SVG nodes for fallacy {TitleFunc(fallacy)} because its parent {TitleFunc(fallacy)} does not have a single corresponding SVG node.");
							}
							parentSvgNode = parentSvgNodes.First();
						}
						else
						{
							Logger.LogProblem($"Could not find parent node from {TitleFunc(fallacy)}");
							break;
						}
					}

					// Get the index of the parent SVG node in the document order
					if (!nodeIndices.TryGetValue(parentSvgNode, out int parentIndex))
					{
						Logger.LogProblem($"SVG Node index for parent fallacy: {parentFallacy.Path}-{TitleFunc(parentFallacy)} of fallacy {fallacy.Path}-{TitleFunc(fallacy)} not found");
					}
					else
					{


						// Sort the candidate SVG nodes based on their index difference with the parent node
						XElement closestSvgNode = candidateSvgNodes
							.OrderBy(node => Math.Abs(nodeIndices[node] - parentIndex))
							.First();

						disambiguatedFallacyToSvgNode[fallacy] = closestSvgNode;

						// Check for conflicts in the node-to-fallacy mapping
						if (svgNodeToFallacy.TryGetValue(closestSvgNode, out var existingFallacy))
						{
							Logger.LogProblem($"Conflicting attribution of SVG node to fallacies: {fallacy.Path}-{TitleFunc(fallacy)} and {existingFallacy.Path}-{TitleFunc(existingFallacy)}");
						}
						else
						{
							svgNodeToFallacy[closestSvgNode] = fallacy;
						}
					}
				}
			}

			return disambiguatedFallacyToSvgNode;
		}


		private void UpdateSvgMatch(SVGFreemindMap svgMap, XElement match, Fallacy fallacy, XNamespace svgNamespace,
			XNamespace xlinkNamespace, ref bool warned)
		{
			if (match.Parent.Name.LocalName == "a" && !warned)
			{
				Logger.LogWarning($"Existing refined content found in SVG file {DocumentName}. Updates will be applied, but some nodes might be missing. Please delete processed SVG file for a fresh processing");
				warned = true;
			}

			string description = DescFunc(fallacy);
			string example = ExampleFunc(fallacy);
			string link = LinkFunc(fallacy);
			string family = FamilleFunc(fallacy);
			string subfamily = SousFamilleFunc(fallacy);
			string subsubfamily = SoussousFamilleFunc(fallacy);


			if (svgMap.SetSVGNodeAttributes)
			{
				match.SetAttributeValue("class", "node");
				match.SetAttributeValue(nameof(family), family);
				match.SetAttributeValue(nameof(subfamily), subfamily);
				match.SetAttributeValue(nameof(subsubfamily), subsubfamily);
				match.SetAttributeValue(nameof(description), description);
				match.SetAttributeValue(nameof(example), example);
				match.SetAttributeValue(nameof(link), link);
				match.SetAttributeValue("depth", fallacy.Depth);
				match.SetAttributeValue("familyclass", fallacy.FamilleCamelCase);
			}

			if (svgMap.WrapNodeByLink)
			{
				XElement linkElem = match.Parent.Name.LocalName == "a"
					? match.Parent
					: new XElement(XName.Get("a", svgNamespace.NamespaceName));

				linkElem.SetAttributeValue(XName.Get("href", xlinkNamespace.NamespaceName), link);
				linkElem.SetAttributeValue("target", "_blank");

				if (match.Parent.Name.LocalName != "a")
				{
					match.ReplaceWith(linkElem);
					linkElem.Add(match);
				}
			}


		}


		private static string GetSvgContent(XDocument svgDoc)
		{
			StringBuilder sb = new();
			XmlWriterSettings settings = new()
			{
				Indent = true,
				IndentChars = "\t", // use tab for indentation
				NewLineChars = Environment.NewLine,
				NewLineHandling = NewLineHandling.Replace
			};

			using (XmlWriter writer = XmlWriter.Create(sb, settings))
			{
				svgDoc.Save(writer);
			}
			string svgContent = sb.ToString();
			return svgContent;
		}



		private static async Task GenerateHtmlSvgWrappers(SVGFreemindMap svgMap, AssetConverterConfig config,
			string svgSavedFilePath,
			Func<Task<string>> svgContent, string language)
		{
			foreach (var htmlSvgWrapper in svgMap.HtmlWrappers)
			{
				var templateFilePath = config.UseDebugParams
					? htmlSvgWrapper.TemplatePathDebug
					: htmlSvgWrapper.TemplatePathRelease;

				string htmlTemplate = (await templateFilePath.GetDocumentPayload()).AsString();

				var languageAwareDocName = htmlSvgWrapper.DocumentName.Replace("[LANGUAGE]", language);

				var htmlFileName = Path.Combine(Directory.GetParent(svgSavedFilePath)!.FullName, languageAwareDocName);  // Path.ChangeExtension(svgSavedFilePath, $".{Path.GetFileName(templateFilePath)}");


				if (File.Exists(htmlFileName) && !config.OverwriteExistingHtmlMaps)
				{
					

					Logger.Log($"Skip existing Html SVG Wrapper: {htmlFileName}");

				}
				else
				{
					var svgRelativePath = svgSavedFilePath.GetRelativePathFrom(Path.GetDirectoryName(htmlFileName));

					htmlTemplate = htmlTemplate.Replace("[SVGPATH]", svgRelativePath);
					htmlTemplate = htmlTemplate.Replace("<!-- Insert here the SVG -->", await svgContent());

					await File.WriteAllTextAsync(htmlFileName, htmlTemplate, Encoding.UTF8);
					Logger.LogSuccess($"Html SVG MindMap wrapper {htmlFileName} successfully saved");
				}

				
			}
		}



		public MindMapDocumentConfig CloneMindMap()
		{
			// Note: Use MemberwiseClone for shallow copy
			var clone = (MindMapDocumentConfig)this.MemberwiseClone();

			// Deep copy collections
			clone.Colors = new Dictionary<int, string>(this.Colors);
			clone.FontSizes = new List<int>(this.FontSizes);
			clone.EdgeSizes = new List<int>(this.EdgeSizes);
			clone.SVGMaps = this.SVGMaps.Select(map => (SVGFreemindMap)map.Clone()).ToList();

			return clone;
		}


		protected override DocumentConfig GetClone()
		{
			return CloneMindMap();
		}
	}

	public enum MindMapFormat
	{
		Freemind,
		Freeplane
	}
}