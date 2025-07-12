using System;
using System.Collections;
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
    public class VirtueMindMapDocumentConfig : VirtueDocumentConfigBase
	{
		public MindMapFormat Format { get; set; } = MindMapFormat.Freemind;
		// Trying to making sure the assemblies get published
		private static readonly System.Diagnostics.StackTrace temp1 = new();
		private static readonly System.Drawing.Color temp2 = Color.AliceBlue;

		const string DefaultTitleExpression = @"{item.Text}";

		public string TitleExpression { get; set; } = DefaultTitleExpression;

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> TitleFunc
		{
			get
			{
				return item =>
				{
					
					var expression = DefaultTitleExpression;
					var title = expression.Interpolate(new Dictionary<string, object>() { { "item", item } });
					if (AddNodePath)
					{
						title = $"{item.Path} - {title}";
					}
					return title;
				};
			}
		}

		public bool AddNodePath { get; set; } = false;


		const string DefaultFamilleExpression = @"{item.Family}";
		public string FamilleExpression { get; set; } = DefaultFamilleExpression;

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> FamilleFunc
		{
			get
			{
				return item => FamilleExpression.Replace("fallacy.", "item.").Interpolate(new Dictionary<string, object>() { { "item", item } }); // $"{item.TextFr}";
			}
		}


		const string DefaultSousFamilleExpression = @"{item.SubFamily}";
		public string SousFamilleExpression { get; set; } = DefaultSousFamilleExpression;

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> SousFamilleFunc
		{
			get
			{
				return item => SousFamilleExpression.Replace("fallacy.", "item.").Interpolate(new Dictionary<string, object>() { { "item", item } }); // $"{item.TextFr}";
			}
		}


		const string DefaultSoussousFamilleExpression = @"{item.SubSubFamily}";
		public string SoussousFamilleExpression { get; set; } = DefaultSoussousFamilleExpression;

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> SoussousFamilleFunc
		{
			get
			{
				return item => SoussousFamilleExpression.Replace("fallacy.", "item.").Interpolate(new Dictionary<string, object>() { { "item", item } }); // $"{item.TextFr}";
			}
		}

		public string DescriptionExpression { get; set; } =
@"
<p>
    {HttpUtility.HtmlEncode(item.Description)}
</p>
";

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> DescFunc
		{
			get
			{
				return item => DescriptionExpression.Replace("fallacy.", "item.").Interpolate(new Dictionary<string, object>() { { "item", item } }); // $"<font size='4'>{HttpUtility.HtmlEncode(item.Description)}</font>";
			}
		}


		public string CardExpression { get; set; } =
			@"
<p>
    <img src=""{mindMap.GetThumbnailsPath(item)}"" width=""60"" height=""60""/>" + DefaultTitleExpression + @"
</p>
";


		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> CardFunc
		{
			get
			{
				return item => CardExpression.Replace("fallacy.", "item.").Replace("fallacy", "item").Interpolate(new Dictionary<string, object>() { { "mindMap", this }, { "item", item } }); // $"<font size='4'>{HttpUtility.HtmlEncode(item.Description)}</font>";
			}
		}



		public string ExampleExpression { get; set; } =
@"
<p>
    <i>{HttpUtility.HtmlEncode(item.Example)}</i>
</p>
";


		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> ExampleFunc
		{
			get
			{
				return item => ExampleExpression.Replace("fallacy.", "item.").Interpolate(new Dictionary<string, object>() { { "item", item } }); // $"<i>{HttpUtility.HtmlEncode(item.Example)}</i>";
			}
		}



		public string LinkExpression { get; set; } = @"{item.Link}";


		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> LinkFunc
		{
			get
			{
				return item => LinkExpression.Replace("fallacy.", "item.").Replace("fallacy", "item").Interpolate(new Dictionary<string, object>() { { "item", item } }); // $"{item.Link}";
			}
		}

		public string ThumbnailsPathExpression { get; set; } = @"../../bin/Debug/netcoreapp3.1/Target/Images/density-0/Fallacies-Web-Thumbnails/argumentum_{item.Path}_{item.Text.ToLower().Replace("" "",""_"")}.png";


		public string GetThumbnailsPath(IMindMapItem item)
		{
			return ThumbnailsPathFunc(item);
		}


		private Func<IMindMapItem, string> _Thumbnails;
		


		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> ThumbnailsPathFunc
		{
			get
			{
				return _Thumbnails ??= item =>
					ThumbnailsPathExpression.Replace("fallacy.", "item.").Interpolate(new Dictionary<string, object>() { { "item", item } }); // $"{item.Text}";
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

		public string ThumbnailsFileNamePattern { get; set; } = "_{item.Path}..";


		public List<SVGFreemindMap> SVGMaps { get; set; } = new List<SVGFreemindMap>();

		public bool KeepOriginalSVG { get; set; } = true;

		public CrossLink CrossLinks { get; set; } = CrossLink.None;

		public string MatchThumbnailsName(string targetDirectory, IMindMapItem item)
		{
			var fileNames = Directory.GetFiles(targetDirectory);
			var thumbnailsFallacyPattern = ThumbnailsFileNamePattern.Replace("fallacy.", "item.").Interpolate(
				new Dictionary<string, object>() { { "item", item } });
			return fileNames.FirstOrDefault(fileName => fileName.Contains(thumbnailsFallacyPattern));
		}


		public override async Task GenerateMindMapFile(IList objects, AssetConverterConfig config, string targetDirectory, string language)
		{
			var mindMapItems = objects.Cast<IMindMapItem>().ToList();
			if (string.IsNullOrEmpty(language))
				language = config.LocalizationConfig.DefaultLanguage;


			var fileName = DocumentName;
			if (!string.IsNullOrEmpty(targetDirectory))
			{
				fileName = Path.Combine(targetDirectory, fileName);

			}
			var documentPath = Path.Combine(targetDirectory, DocumentName);

			CreateFreemindmap(mindMapItems, config, language, documentPath, fileName);
			
			await ProcessSvgFilesAsync(mindMapItems, fileName, config, config.EnableSVGPrompt, language);
		}

		private void CreateFreemindmap(IList<IMindMapItem> mindMapItems, AssetConverterConfig config, string language, string documentPath, string fileName)
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
				
				var nodesByPath = new Dictionary<string, Node>(mindMapItems.Count);
				CreateMindMapNodes(freemindMap, mindMapItems, nodesByPath, config, language);


				SerializeMindMapAsync(freemindMap, fileName);

				var svgPath = Path.ChangeExtension(fileName, "svg");
				if (!TryAutomateSvgConversion(fileName, svgPath, config) && config.EnableSVGPrompt)
				{
					// If conversion fails, the existing logic will ask the user.
				}
			}
		}

		private bool TryAutomateSvgConversion(string sourceMmPath, string destinationSvgPath, AssetConverterConfig config)
		{
			if (string.IsNullOrEmpty(config.FreeplanePath) || !File.Exists(config.FreeplanePath))
			{
				Logger.LogWarning("Freeplane executable not found or path not configured. Skipping automatic SVG conversion.");
				return false;
			}
		
			try
			{
				var processStartInfo = new ProcessStartInfo
				{
					FileName = config.FreeplanePath,
					Arguments = $"-X ConvertToSvg -S \"{sourceMmPath}\" \"{destinationSvgPath}\"",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true
				};
		
				using (var process = new Process { StartInfo = processStartInfo })
				{
					Logger.Log($"Attempting automatic SVG conversion for: {sourceMmPath}");
					process.Start();
					string output = process.StandardOutput.ReadToEnd();
					string error = process.StandardError.ReadToEnd();
					process.WaitForExit();
		
					if (process.ExitCode == 0)
					{
						Logger.LogSuccess("SVG conversion successful.");
						if (!string.IsNullOrWhiteSpace(output)) Logger.Log(output);
						return true;
					}
					else
					{
						Logger.LogWarning($"SVG conversion failed with exit code {process.ExitCode}.");
						if (!string.IsNullOrWhiteSpace(output)) Logger.Log(output);
						if (!string.IsNullOrWhiteSpace(error)) Logger.LogProblem(error);
						Logger.LogWarning("Manual conversion might be required.");
						return false;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.LogProblem($"An exception occurred during automatic SVG conversion: {ex.Message}");
				Logger.LogWarning("Manual conversion might be required.");
				return false;
			}
		}


		private void CreateMindMapNodes(FreemindMap freemindMap, IList<IMindMapItem> mindMapItems, Dictionary<string, Node> nodesByPath, AssetConverterConfig config, string language)
		{
			var linkedItems = new HashSet<IMindMapItem>();

			foreach (var item in mindMapItems)
			{
				linkedItems.Add(item);
				if (string.IsNullOrEmpty(item.PK)) continue;

				var localPath = item.Path;

				List<(CrossLink crossLinkType, List<IMindMapItem> targets)> crossLinks = new();

				if (this.CrossLinks.HasFlag(CrossLink.Identity))
				{
					var identityItems = mindMapItems.Where(f => f.Text == item.Text && !linkedItems.Contains(f)).ToList();
					crossLinks.Add((CrossLink.Identity, identityItems));
				}

				var itemNode = CreateNode(item, config, language, crossLinks.ToArray());
				nodesByPath[localPath] = itemNode;

				var lastDotIndex = localPath.LastIndexOf('.');
				int familyNb;
				if (lastDotIndex > -1)
				{
					familyNb = int.Parse(item.Path[0].ToString(), CultureInfo.InvariantCulture);
					var parentPath = localPath[..lastDotIndex];
					var parentNode = nodesByPath[parentPath];
					parentNode.Nodes.Add(itemNode);
				}
				else
				{
					familyNb = int.Parse(localPath);
					AddNodeToFreemindMap(freemindMap, itemNode, familyNb);
				}

				SetNodeStyle(itemNode, item, familyNb);
			}
		}

		

		private Node CreateNode(IMindMapItem item, AssetConverterConfig config, string language, params (CrossLink crossLinkType, List<IMindMapItem> targets)[] crossLinks)
		{
			var itemNode = new Node { TEXT = TitleFunc(item) };
			itemNode.ID = item.Id;
			var link = LinkFunc(item);
			if (!string.IsNullOrEmpty(link))
			{
				itemNode.LINK = link;
			}

			var descRichContent = CreateRichContent(item);
			itemNode.Richcontents.Add(descRichContent);

			if (item.Carte.HasValue)
			{
				AddCardIcon(item, itemNode, config, language);
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
					itemNode.Arrowlinks.Add(crossLinkNode);

				}
			}


			return itemNode;
		}

		private Richcontent CreateRichContent(IMindMapItem item)
		{
			var descDoc = new XmlDocument();
			descDoc.LoadXml($"{DescFunc(item)}");

			var descRichContent = new Richcontent { TYPE = "NOTE" };
			descRichContent.Html.Body.Elements.Add(descDoc.DocumentElement);

			descDoc.LoadXml($"{ExampleFunc(item)}");
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

		private void SetNodeStyle(Node node, IMindMapItem item, int familyNb)
		{
			if (item.Depth < FontSizes.Count)
			{
				node.Font = new Font() { Size = FontSizes[item.Depth].ToString() };
			}

			if (item.Depth < EdgeSizes.Count)
			{



				if (familyNb > 0)
				{
					node.Edge = new Edge() { WIDTH = EdgeSizes[item.Depth - 1].ToString(CultureInfo.InvariantCulture) };
					node.Edge.COLOR = Colors[familyNb];
					node.BACKGROUND_COLOR = HLSColor.GetLighterColor(Colors[familyNb]);
				}

				node.STYLE = "bubble";
			}
			else
			{
				node.STYLE = "fork";
				if (item.Depth == EdgeSizes.Count)
				{
					node.Edge = new Edge() { WIDTH = EdgeSizes[item.Depth - 1].ToString(CultureInfo.InvariantCulture) };
				}
			}

			if (item.Depth <= EdgeSizes.Count)
			{
				node.Font.BOLD = "true";
			}

			if (item.Depth >= EdgeSizes.Count)
			{
				node.COLOR = HLSColor.GetDarkerColor(Colors[familyNb]);
			}
		}

		private void AddCardIcon(IMindMapItem item, Node node, AssetConverterConfig assetConverterConfig, string language)
		{
			node.Icons.Add(new Icon() { BUILTIN = $"full-{item.Carte}" });

			if (InsertCardsThumbnails )
			{
				var cardSetConfig = assetConverterConfig.WebBasedGeneratorConfig.CardSets.FirstOrDefault(c => c.Name == this.ThumbnailsCardSetName, null);
				if (cardSetConfig != null)
				{
					this.ThumbnailsPathFunc = objItem =>
					{
						var cardSetDirectory = ImageHelper.GetImageFolder(assetConverterConfig, this, language, ThumbnailsCardSetName);
						var imageFileName = MatchThumbnailsName(cardSetDirectory, item);
						if (string.IsNullOrEmpty(imageFileName))
						{
							Logger.LogProblem($"No thumbnail for item {TitleFunc(item)} in directory {cardSetDirectory}");
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
				cardDoc.LoadXml($"{CardFunc(item)}");
				var cardRichContent = new Richcontent();
				node.Richcontents.Add(cardRichContent);
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
		private async Task ProcessSvgFilesAsync(IList<IMindMapItem> mindMapItems, string fileName,
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
							Logger.LogWarning($"File {svgFilePath} not found and skipped. Automatic conversion failed. Switch \"EnableSVGPrompt\" on for Freemind-assisted SVG generation.");
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

					UpdateSvgWithItems(svgFreemindMap, mindMapItems, svgDoc, svgNamespace, xlinkNamespace);


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
		
		private static async Task DisplaySvgFileNotFoundMessage(string svgFilePath)
		{
			if (Program.IsInteractive)
			{
				Logger.LogInstructions($"SVG mindmap {svgFilePath} was not found.\n Please download open-source software freemind to generate a SVG export from the original .mm file.\n" +
									   $"[link]https://sourceforge.net/projects/freemind/[/]\nSvg export will be further edited to include fields and links\nPress any key to resume and update or skip the SVG file...");

				await UtilityExtensions.ConsoleKeyPressAsync();
			}
			//await UtilityExtensions.KeyPressSemaphore.WaitAsync();
		}
		
		private void UpdateSvgWithItems(SVGFreemindMap svgMap, IList<IMindMapItem> items, XDocument svgDoc, XNamespace svgNamespace, XNamespace xlinkNamespace)
		{


			var itemToSVGNodes = CollectPossibleSvgNodes(items, svgDoc, svgNamespace);
			var disambiguatedItemToSVGNode = DisambiguateSvgNodes(itemToSVGNodes, items, svgNamespace);
			var warned = false;
			foreach (var pair in disambiguatedItemToSVGNode)
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



		private Dictionary<IMindMapItem, List<XElement>> CollectPossibleSvgNodes(IList<IMindMapItem> items, XDocument svgDoc, XNamespace svgNamespace)
		{
			Dictionary<IMindMapItem, List<XElement>> itemToSvgNodes = new();
			var textGroups = svgDoc.Descendants(svgNamespace + "g").Where(g => g.Elements(svgNamespace + "text").Any()).ToList();

			foreach (var item in items)
			{
				string title = TitleFunc(item);
				var matchingGroups = textGroups.Where(g => string.Join("", g.Elements(svgNamespace + "text").Select(t => t.Value)).Contains(title)).ToList();

				if (matchingGroups.Any())
				{
					var groupedGroups = matchingGroups.GroupBy(g => string.Join("", g.Elements(svgNamespace + "text").Select(t => t.Value)).Length);
					var groups = groupedGroups as IGrouping<int, XElement>[] ?? groupedGroups.ToArray();
					var minLength = groups.Min(g => g.Key);
					var minLengthGroups = groups.First(g => g.Key == minLength).ToList();

					itemToSvgNodes[item] = minLengthGroups;
				}
				else
				{
					var closeMatches = textGroups.Where(g => string.Join("", g.Elements(svgNamespace + "text").Select(t => t.Value)).Contains(title.Substring(0, 3))).ToList();
					var closeMatchesMessages = closeMatches.Select(g => string.Join(" ", g.Elements(svgNamespace + "text").Select(t => t.Value))).ToList().Aggregate("", (s1, s2) => $"{s1}\n{s2}");
					Logger.LogProblem($"Could not find Svg node for item {TitleFunc(item)}\nClose matches:\n{closeMatchesMessages}");
				}
			}

			return itemToSvgNodes;
		}


		private Dictionary<IMindMapItem, XElement> DisambiguateSvgNodes(
			Dictionary<IMindMapItem, List<XElement>> itemToSvgNodes, IList<IMindMapItem> items, XNamespace svgNamespace)
		{
			if (!itemToSvgNodes.Any() || !itemToSvgNodes.First().Value.Any())
			{
				Logger.LogProblem("No SVG nodes to disambiguate.");
				return new Dictionary<IMindMapItem, XElement>();
			}

			var tempNode = itemToSvgNodes.First().Value.First();
			var allNodesList = tempNode.Document.Descendants(svgNamespace + tempNode.Name.LocalName).ToList();
			var nodeIndices = allNodesList.Select((n, i) => new { Node = n, Index = i }).ToDictionary(n => n.Node, n => n.Index);

			foreach (var itemToSvgNode in itemToSvgNodes)
			{
				foreach (var svgNode in itemToSvgNode.Value)
				{
					if (!nodeIndices.ContainsKey(svgNode))
					{
						Logger.LogWarning($"SVG node for item {TitleFunc(itemToSvgNode.Key)} not found in document index. It might be a new or detached node.");
					}
				}
			}

			Dictionary<IMindMapItem, XElement> disambiguatedItemToSvgNode = new();
			Dictionary<XElement, IMindMapItem> svgNodeToItem = new();

			foreach (var pair in itemToSvgNodes)
			{
				IMindMapItem item = pair.Key;
				List<XElement> candidateSvgNodes = pair.Value;

				if (candidateSvgNodes.Count == 1)
				{
					var candidate = candidateSvgNodes.First();
					disambiguatedItemToSvgNode[item] = candidate;
					svgNodeToItem[candidate] = item;
				}
				else
				{
					if (string.IsNullOrEmpty(item.DecimalPath) || item.DecimalPath.Length <= 1)
					{
						Logger.LogProblem($"Cannot determine parent for item {TitleFunc(item)} - {item.Path}");
						continue;
					}
					string parentDecimalPath = item.DecimalPath.Remove(item.DecimalPath.Length - 1);
					var parentItemCandidates = items.Where(f => f.DecimalPath == parentDecimalPath).ToArray();
					if (parentItemCandidates.Length == 0)
					{
						Logger.LogProblem($"Parent item not found for {TitleFunc(item)} - {item.Path}");
						continue;
					}

					var parentItem = parentItemCandidates.First();

					if (!disambiguatedItemToSvgNode.TryGetValue(parentItem, out var parentSvgNode))
					{
						if (itemToSvgNodes.TryGetValue(parentItem, out List<XElement> parentSvgNodes))
						{
							if (parentSvgNodes.Count > 1)
							{
								Logger.LogProblem($"Could not disambiguate SVG nodes for item {TitleFunc(item)} because its parent {TitleFunc(parentItem)} does not have a single corresponding SVG node.");
								continue;
							}
							parentSvgNode = parentSvgNodes.FirstOrDefault();
							if (parentSvgNode == null)
							{
								Logger.LogProblem($"List of parent SVG nodes for {TitleFunc(parentItem)} is empty.");
								continue;
							}
						}
						else
						{
							Logger.LogProblem($"Could not find parent node from {TitleFunc(item)}");
							continue;
						}
					}

					if (!nodeIndices.TryGetValue(parentSvgNode, out int parentIndex))
					{
						Logger.LogProblem($"SVG Node index for parent item: {parentItem.Path}-{TitleFunc(parentItem)} of item {item.Path}-{TitleFunc(item)} not found");
						continue;
					}
					
					var closestSvgNode = candidateSvgNodes
						.Where(node => nodeIndices.ContainsKey(node))
						.OrderBy(node => Math.Abs(nodeIndices[node] - parentIndex))
						.FirstOrDefault();
					
					if (closestSvgNode != null)
					{
						disambiguatedItemToSvgNode[item] = closestSvgNode;
						if (svgNodeToItem.TryGetValue(closestSvgNode, out var existingItem))
						{
							Logger.LogProblem($"Conflicting attribution of SVG node to items: {item.Path}-{TitleFunc(item)} and {existingItem.Path}-{TitleFunc(existingItem)}");
						}
						else
						{
							svgNodeToItem[closestSvgNode] = item;
						}
					}
					else
					{
						Logger.LogWarning($"Could not find a valid matching SVG node for item {TitleFunc(item)} among candidates.");
					}
				}
			}

			return disambiguatedItemToSvgNode;
		}


		private void UpdateSvgMatch(SVGFreemindMap svgMap, XElement match, IMindMapItem item, XNamespace svgNamespace,
			XNamespace xlinkNamespace, ref bool warned)
		{
			if (match.Parent.Name.LocalName == "a" && !warned)
			{
				Logger.LogWarning($"Existing refined content found in SVG file {DocumentName}. Updates will be applied, but some nodes might be missing. Please delete processed SVG file for a fresh processing");
				warned = true;
			}

			string description = DescFunc(item);
			string example = ExampleFunc(item);
			string link = LinkFunc(item);
			string family = FamilleFunc(item);
			string subfamily = SousFamilleFunc(item);
			string subsubfamily = SoussousFamilleFunc(item);


			if (svgMap.SetSVGNodeAttributes)
			{
				match.SetAttributeValue("class", "node");
				match.SetAttributeValue(nameof(family), family);
				match.SetAttributeValue(nameof(subfamily), subfamily);
				match.SetAttributeValue(nameof(subsubfamily), subsubfamily);
				match.SetAttributeValue(nameof(description), description);
				match.SetAttributeValue(nameof(example), example);
				match.SetAttributeValue(nameof(link), link);
				match.SetAttributeValue("depth", item.Depth);
				match.SetAttributeValue("familyclass", item.Family.Replace(" ", ""));
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

				var htmlFileName = Path.Combine(Directory.GetParent(svgSavedFilePath)!.FullName, languageAwareDocName);  


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

		public VirtueMindMapDocumentConfig CloneMindMap()
		{
			var clone = (VirtueMindMapDocumentConfig)this.MemberwiseClone();
			
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
}