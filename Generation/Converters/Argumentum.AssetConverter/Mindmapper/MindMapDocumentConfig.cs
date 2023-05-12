using System;
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
using HarfBuzzSharp;
using ImageMagick;
using Spectre.Console;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Argumentum.AssetConverter.Mindmapper
{
	public class MindMapDocumentConfig : DocumentConfig, ICloneable
	{



		public string DataSet { get; set; } = @"..\..\..\Data\Mindmap\Argumentum Fallacies - Taxonomy.csv";
		public string DocumentName { get; set; } = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2.mm";

		 const string DefaultTitleExpression = @"{fallacy.TextFr}";
		public string TitleExpression { get; set; } = DefaultTitleExpression;

		public string DescriptionExpression { get; set; } =
@"
<p>
    <font size='4'>{HttpUtility.HtmlEncode(fallacy.DescFr)}</font>
</p>
";

		public string CardExpression { get; set; } =
			@"
<p>
    <img src=""{mindMap.GetThumbnailsPath(fallacy)}"" width=""60"" height=""60""/>" + DefaultTitleExpression + @"
</p>
";


		//
		// https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Assets/Fallacy-front/{fallacy.Path}.png

		public string ExampleExpression { get; set; } =
@"
<p>
    <i>{HttpUtility.HtmlEncode(fallacy.ExampleFr)}</i>
</p>
";

		public string LinkExpression { get; set; } = @"{fallacy.LinkFrFallback}";


		


		[IgnoreDataMember]
		[JsonIgnore]
		public Func<Fallacy, string> TitleFunc
		{
			get
			{
				return fallacy => TitleExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"{fallacy.TextFr}";
			}
		}

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<Fallacy, string> DescFunc
		{
			get
			{
				return fallacy => DescriptionExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"<font size='4'>{HttpUtility.HtmlEncode(fallacy.DescFr)}</font>";
			}
		}

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<Fallacy, string> CardFunc
		{
			get
			{
				return fallacy => CardExpression.Interpolate(new Dictionary<string, object>() { { "mindMap", this}, { "fallacy", fallacy } }); // $"<font size='4'>{HttpUtility.HtmlEncode(fallacy.DescFr)}</font>";
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
				if (_Thumbnails == null)
				{
					_Thumbnails = fallacy => ThumbnailsPathExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"{fallacy.TextFr}";
				}

				return _Thumbnails;
			}
			set
			{
				_Thumbnails = value;
			}
		}



		[IgnoreDataMember]
		[JsonIgnore]
		public Func<Fallacy, string> ExampleFunc
		{
			get
			{
				return fallacy => ExampleExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"<i>{HttpUtility.HtmlEncode(fallacy.ExampleFr)}</i>";
			}
		}

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<Fallacy, string> LinkFunc
		{
			get
			{
				return fallacy => LinkExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"{fallacy.LinkFr}";
			}
		}

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


		public List<int> FontSizes { get; set; } = new List<int>(new[] { 30, 50, 40, 30, 30, 30, 25, 23, 23, 23, 23 });


		public List<int> EdgeSizes { get; set; } = new List<int>(new[] { 8, 4, 2, 1 });


		public bool InsertCardsThumbnails { get; set; }


		public string ThumbnailsCardSetName { get; set; }

		public string ThumbnailsFileNamePattern { get; set; } = "_{fallacy.Path}..";


		public List<HtmlSVGWrapper> HtmlSvgWrappers { get; set; } = new List<HtmlSVGWrapper>();



		public string MatchThumbnailsName(string targetDirectory, Fallacy fallacy)
		{
			var fileNames = Directory.GetFiles(targetDirectory);
			var thumbnailsFallacyPattern = ThumbnailsFileNamePattern.Interpolate(
				new Dictionary<string, object>() { { "fallacy", fallacy } });
			return fileNames.First(fileName => fileName.Contains(thumbnailsFallacyPattern));
		}


		public async Task GenerateMindMapFile(IList<Fallacy> fallacies, WebBasedGeneratorConfig webBasedGeneratorConfig, string targetDirectory, string language )
		{
			if (string.IsNullOrEmpty(language)) 
				language=webBasedGeneratorConfig.LocalizationConfig.DefaultLanguage ;


			var fileName = DocumentName;
			if (!string.IsNullOrEmpty(targetDirectory))
			{
				fileName = Path.Combine(targetDirectory, fileName);

			}
			var documentPath = Path.Combine(targetDirectory, DocumentName);

			await CreateFreemindmap(fallacies, webBasedGeneratorConfig, language, documentPath, fileName);

			//Task.Run(async () => await ProcessSVGFiles(fallacies, fileName, webBasedGeneratorConfig, webBasedGeneratorConfig.EnableSVGPrompt)).GetAwaiter().GetResult() ;
			await ProcessSVGFiles(fallacies, fileName, webBasedGeneratorConfig,
				webBasedGeneratorConfig.EnableSVGPrompt);
		}

		private async Task CreateFreemindmap(IList<Fallacy> fallacies, WebBasedGeneratorConfig webBasedGeneratorConfig, string language,
			string documentPath, string fileName)
		{
			if (File.Exists(documentPath) && !webBasedGeneratorConfig.OverwriteExistingDocs)
			{
				Logger.Log($"Skip existing Mindmap: {documentPath}");
			}
			else
			{
				Logger.Log($"Creating Freemind mind map {DocumentName}");
				var freemindMap = new FreemindMap();
				var nodesByPath = new Dictionary<string, Node>(fallacies.Count());
				await CreateFallacyNodes(freemindMap, fallacies, nodesByPath, webBasedGeneratorConfig, language);


				await SerializeMindMap(freemindMap, fileName);
			}
		}


		private async Task CreateFallacyNodes(FreemindMap freemindMap, IList<Fallacy> fallacies, Dictionary<string, Node> nodesByPath,
			WebBasedGeneratorConfig webBasedGeneratorConfig, string language)
		{
			foreach (var fallacy in fallacies)
			{
				if (string.IsNullOrEmpty(fallacy.PK)) continue;

				var localPath = fallacy.Path;
				var fallacyNode = await CreateNode(fallacy, webBasedGeneratorConfig, language);
				nodesByPath[localPath] = fallacyNode;

				var lastDotIndex = localPath.LastIndexOf('.');
				int familyNb;
				if (lastDotIndex > -1)
				{
					familyNb = int.Parse(fallacy.Path[0].ToString(), CultureInfo.InvariantCulture);
					var parentPath = localPath.Substring(0, lastDotIndex);
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

		private async Task<Node> CreateNode(Fallacy fallacy, WebBasedGeneratorConfig webBasedGeneratorConfig, string language)
		{
			var fallacyNode = new Node { TEXT = TitleFunc(fallacy) };
			var link = LinkFunc(fallacy);
			if (!string.IsNullOrEmpty(link))
			{
				fallacyNode.LINK = link;
			}

			var descRichContent = await CreateRichContent(fallacy);
			fallacyNode.Richcontents.Add(descRichContent);

			if (fallacy.Carte.HasValue)
			{
				await AddCardIcon(fallacy, fallacyNode, webBasedGeneratorConfig, language);
			}

			return fallacyNode;
		}

		private async Task<Richcontent> CreateRichContent(Fallacy fallacy)
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
				fallacyNode.POSITION = familyNb > 2 ? "left" : "right";
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
				fallacyNode.Edge = new Edge() { WIDTH = EdgeSizes[fallacy.Depth].ToString(CultureInfo.InvariantCulture) };

				if (familyNb > 0)
				{
					fallacyNode.Edge.COLOR = Colors[familyNb];
					fallacyNode.BACKGROUND_COLOR = HLSColor.GetLighterColor(Colors[familyNb]);
				}

				fallacyNode.STYLE = "bubble";
			}
			else
			{
				fallacyNode.STYLE = "fork";
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

		private async Task AddCardIcon(Fallacy fallacy, Node fallacyNode, WebBasedGeneratorConfig webBasedGeneratorConfig, string language)
		{
			fallacyNode.Icons.Add(new Icon() { BUILTIN = $"full-{fallacy.Carte}" });

			if (InsertCardsThumbnails && webBasedGeneratorConfig != null)
			{
				var cardSetConfig = webBasedGeneratorConfig.CardSets.FirstOrDefault(c => c.Name == this.ThumbnailsCardSetName, null);
				if (cardSetConfig != null)
				{
					this.ThumbnailsPathFunc = objFallacy =>
					{
						var cardSetDirectory = ImageHelper.GetImageFolder(webBasedGeneratorConfig, this, language, ThumbnailsCardSetName);
						var imageFileName = MatchThumbnailsName(cardSetDirectory, fallacy);
						var targetDirectory = webBasedGeneratorConfig.GetDocumentDirectory(language);
						imageFileName = imageFileName.GetRelativePathFrom(targetDirectory);

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


		
		private async Task SerializeMindMap(FreemindMap toReturn, string fileName)
		{
			var serializer = new XmlSerializer(typeof(FreemindMap));

			using (var fs = File.Create(fileName))
			{
				XmlWriterSettings writerSettings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
				using (var writer = XmlWriter.Create(fs, writerSettings))
				{
					serializer.Serialize(writer, toReturn);
				}
			}


			Logger.LogSuccess($"Mind map {fileName} successfully generated!");
		}
		private async Task ProcessSVGFiles(IList<Fallacy> fallacies, string fileName,
			WebBasedGeneratorConfig webBasedGeneratorConfig, bool enableSvgUpdates)
		{
			string svgFilePath = Path.ChangeExtension(fileName, "svg");
			var svgSavedFilePath = svgFilePath.Replace(".svg", "_Full.svg");
			Func<Task<string>> svgLoader;
			if (File.Exists(svgSavedFilePath) && !webBasedGeneratorConfig.OverwriteExistingDocs)
			{
				Logger.Log($"Skipping existing processed SVG: {svgSavedFilePath}");
				svgLoader = async  () => await File.ReadAllTextAsync(svgSavedFilePath);
			}
			else
			{
				if (enableSvgUpdates)
				{
					DisplaySVGFileNotFoundMessage(svgFilePath);
				}
				else
				{
					Logger.LogWarning($"File {svgFilePath} not found and skipped. Switch \"EnableSVGPrompt\" on for Freemind-assisted SVG generation.");
				}
				
				if (!File.Exists(svgFilePath))
				{
					return;
				}
				XDocument svgDoc = XDocument.Load(svgFilePath);
				XNamespace svgNamespace = "http://www.w3.org/2000/svg";
				XNamespace xlinkNamespace = "http://www.w3.org/1999/xlink";

				UpdateSVGWithFallacies(fallacies, svgDoc, svgNamespace, xlinkNamespace);


				svgLoader = async () => await GetSvgContent(svgDoc);


				await File.WriteAllTextAsync(svgSavedFilePath,await svgLoader(), Encoding.UTF8);
				Logger.LogSuccess($"SVG file with detailed content {svgFilePath} successfully saved");

			}



			await GenerateHtmlSVGWrappers(webBasedGeneratorConfig, svgSavedFilePath, svgLoader);
			//Task.Run(async () => await GenerateHtmlSVGWrappers(webBasedGeneratorConfig, svgSavedFilePath, svgLoader))
			//	.GetAwaiter().GetResult();




		}

		private static object lockObj = new object();

		private static bool locked;

		private void DisplaySVGFileNotFoundMessage(string svgFilePath)
		{
			Logger.LogInstructions($"SVG mindmap {svgFilePath} was not found.\n Please download open-source software freemind to generate a SVG export from the original .mm file.\n" +
			                       $"[link]https://sourceforge.net/projects/freemind/[/]\nSvg export will be further edited to include fields and links\nPress any key to resume and update or skip the SVG file...");
			if (!locked)
			{
				lock (lockObj)
				{
					if (!locked)
					{
						locked = true;
						Console.ReadKey();
						locked = false;
					}
				}
			}
			
		}

		private void UpdateSVGWithFallacies(IList<Fallacy> fallacies, XDocument svgDoc, XNamespace svgNamespace, XNamespace xlinkNamespace)
		{
			foreach (var fallacy in fallacies)
			{
				string title = TitleFunc(fallacy);
				var textElements = svgDoc.Descendants(svgNamespace + "text").Where(t => t.Value.Contains(title)).ToList();

				var groupedMatches = textElements.OrderBy(t => Math.Abs(t.Value.Length - title.Length))
					.GroupBy(t => Math.Abs(t.Value.Length - title.Length)).ToList();

				if (groupedMatches.Any())
				{
					var shortestMatches = groupedMatches.First();

					foreach (var match in shortestMatches)
					{
						UpdateSVGMatch(match, fallacy, svgNamespace, xlinkNamespace);
					}
				}
			}
		}

		private void UpdateSVGMatch(XElement match, Fallacy fallacy, XNamespace svgNamespace, XNamespace xlinkNamespace)
		{
			if (match.Parent.Name.LocalName == "a")
			{
				Logger.LogWarning("Existing refined content found in SVG file. Updates will be applied, but some nodes might be missing. Please delete processed SVG file for a fresh processing");
			}

			string desc = DescFunc(fallacy);
			string example = ExampleFunc(fallacy);
			string link = LinkFunc(fallacy);

			match.SetAttributeValue("description", desc);
			match.SetAttributeValue("example", example);
			match.SetAttributeValue("link", example);
			match.SetAttributeValue("depth", fallacy.Depth);
			match.SetAttributeValue("familyClass", fallacy.FamilleCamelCase);
			match.SetAttributeValue("class", "node");
			

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

		private async Task<string> GetSvgContent(XDocument svgDoc)
		{
			StringBuilder sb = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings
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



		private async Task GenerateHtmlSVGWrappers(WebBasedGeneratorConfig webBasedGeneratorConfig, string svgSavedFilePath,
			Func<Task<string>> svgContent)
		{
			foreach (var htmlSvgWrapper in HtmlSvgWrappers)
			{
				var templateFilePath = webBasedGeneratorConfig.UseDebugParams
					? htmlSvgWrapper.TemplatePathDebug
					: htmlSvgWrapper.TemplatePathRelease;

				string htmlTemplate = (await templateFilePath.GetDocumentPayload()).AsString();

				var htmlFileName = Path.ChangeExtension(svgSavedFilePath, $".{Path.GetFileName(templateFilePath)}");


				if (File.Exists(htmlFileName) && !webBasedGeneratorConfig.OverwriteExistingDocs)
				{
					

					Logger.Log($"Skip existing Html SVG Wrapper: {htmlFileName}");

				}
				else
				{
					var svgRelativePath = svgSavedFilePath.GetRelativePathFrom(Directory.GetParent(htmlFileName)?.FullName);

					htmlTemplate = htmlTemplate.Replace("[SVGPATH]", svgRelativePath);
					htmlTemplate = htmlTemplate.Replace("[SVGCONTENT]", await svgContent());

					await File.WriteAllTextAsync(htmlFileName, htmlTemplate, Encoding.UTF8);
					Logger.LogSuccess($"Html SVG MindMap wrapper {htmlFileName} successfully saved");
				}

				
			}
		}



		public MindMapDocumentConfig CloneMindMap()
		{
			var clone = new MindMapDocumentConfig()
			{
				DataSet = this.DataSet,
				DocumentName = this.DocumentName,
				TitleExpression = this.TitleExpression,
				DescriptionExpression = this.DescriptionExpression,
				CardExpression = this.CardExpression,
				ExampleExpression = this.ExampleExpression,
				LinkExpression = this.LinkExpression,
				Colors = new Dictionary<int, string>(this.Colors),
				FontSizes = new List<int>(this.FontSizes),
				EdgeSizes = new List<int>(this.EdgeSizes),
				InsertCardsThumbnails = this.InsertCardsThumbnails,
				ThumbnailsFileNamePattern = this.ThumbnailsFileNamePattern,
				ThumbnailsPathExpression = this.ThumbnailsPathExpression,
				ThumbnailsPathFunc = this.ThumbnailsPathFunc,
				ThumbnailsCardSetName = this.ThumbnailsCardSetName,
				Enabled = this.Enabled,
				ImageFormat = this.ImageFormat,
				TargetDensity = this.TargetDensity,
				Translations = this.Translations,
				HtmlSvgWrappers = this.HtmlSvgWrappers

			};

			return clone;
		}


		public object Clone()
		{
			return CloneMindMap();
		}
	}








}