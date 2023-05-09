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
using ImageMagick;
using Spectre.Console;

namespace Argumentum.AssetConverter.Mindmapper
{

	public class MindMapDocumentConfig : DocumentConfig, ICloneable
	{

		public bool EnableSVGUpdate { get; set; } = false;

		public string DataSet { get; set; } = @"..\..\..\Data\Mindmap\Argumentum Fallacies - Taxonomy.csv";
		public string DocumentName { get; set; } = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2.mm";


		public string TitleExpression { get; set; } = @"{fallacy.TextFr}";

		public string DescriptionExpression { get; set; } =
@"
<p>
    <font size='4'>{HttpUtility.HtmlEncode(fallacy.DescFr)}</font>
</p>
";

		public string CardExpression { get; set; } =
			@"
<p>
    <img src=""{mindMap.ThumbnailsPathFunc(fallacy)}"" width=""60"" height=""60""/>{fallacy.TextFr}
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



		public void GenerateMindMapFile(IList<Fallacy> fallacies,
			WebBasedGeneratorConfig webBasedGeneratorConfig, string targetDirectory)
		{

			Console.WriteLine($"Creating Freemind mind map {DocumentName}");

			var toReturn = new FreemindMap();
			var nodesByPath = new Dictionary<string, Node>(fallacies.Count());
			foreach (var fallacy in fallacies)
			{
				if (!string.IsNullOrEmpty(fallacy.PK))
				{
					var localPath = fallacy.Path;
					var fallacyNode = new Node();
					fallacyNode.TEXT = TitleFunc(fallacy);
					var link = LinkFunc(fallacy);
					if (!string.IsNullOrEmpty(link))
					{
						fallacyNode.LINK = link;
					}
					var descDoc = new XmlDocument();
					descDoc.LoadXml($"{DescFunc(fallacy)}");

					var descRichContent = new Richcontent();
					fallacyNode.Richcontents.Add(descRichContent);
					descRichContent.TYPE = "NOTE";
					descRichContent.Html.Body.Elements.Add(descDoc.DocumentElement);

					descDoc.LoadXml($"{ExampleFunc(fallacy)}");
					descRichContent.Html.Body.Elements.Add(descDoc.DocumentElement);

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
						if (familyNb == 0)
						{
							fallacyNode.ID = "ID_706669011";
							toReturn.Node = fallacyNode;

						}
						else
						{
							if (familyNb > 2)
							{
								fallacyNode.POSITION = "left";
							}
							else
							{
								fallacyNode.POSITION = "right";
							}
							toReturn.Node.Nodes.Add(fallacyNode);
						}
					}

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
							//fallacyNode.COLOR = ColorTranslator.ToHtml(ColorTranslator.FromHtml(config.Colors[familyNb]).GetContrast(true));
							//fallacyNode.Font.Color = ColorTranslator.ToHtml(Color.AliceBlue);
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

					if (fallacy.Carte.HasValue)
					{
						fallacyNode.Icons.Add(new Icon() { BUILTIN = $"full-{fallacy.Carte}" });
						//if (fallacy.Path.StartsWith("1.1"))
						//{

						if (InsertCardsThumbnails)
						{
							if (webBasedGeneratorConfig != null)
							{
								var cardSetConfig =  webBasedGeneratorConfig.CardSets.First(c => c.Name == this.ThumbnailsCardSetName);
								if (cardSetConfig != null)
								{
									this.ThumbnailsPathFunc = objFallacy =>
										ImageHelper.GetImageFileName(webBasedGeneratorConfig, this,
											webBasedGeneratorConfig.LocalizationConfig.DefaultLanguage, ThumbnailsCardSetName,
											objFallacy.FileName);
								}
								
							}


							var cardDoc = new XmlDocument();
							cardDoc.LoadXml($"{CardFunc(fallacy)}");
							var cardRichContent = new Richcontent();
							fallacyNode.Richcontents.Add(cardRichContent);
							cardRichContent.TYPE = "NODE";
							cardRichContent.Html.Body.Elements.Add(cardDoc.DocumentElement);
						}

						//}


					}
				}

			}



			var serializer = new XmlSerializer(typeof(FreemindMap));

			var fileName = DocumentName;
			if (!string.IsNullOrEmpty(targetDirectory ))
			{
				fileName = Path.Combine(targetDirectory, fileName);

			}

			using (var fs = File.Create(fileName))
			{
				XmlWriterSettings writerSettings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
				using (var writer = XmlWriter.Create(fs, writerSettings))
				{
					serializer.Serialize(writer, toReturn);
				}
			}


			Console.WriteLine($"Mind map {fileName} successfully generated!");

			// Check if EnableSVGUpdate is true
			if (EnableSVGUpdate)
			{
				string svgFilePath = Path.ChangeExtension(fileName, "svg");
				if (!File.Exists(svgFilePath))
				{
					Console.WriteLine("SVG mindmap {svgFilePath} was not found.");
					Console.WriteLine("Use open-source software freemind to generate a SVG export from the original .mm file.");
					AnsiConsole.Markup("[link]https://sourceforge.net/projects/freemind/[/]");
					Console.WriteLine("Svg export will be further edited to include fields and links");
					Console.WriteLine(" Press any key to resume and update or skip the SVG file...");
					Console.ReadKey();
				}

				// Read the SVG file
				
				if (File.Exists(svgFilePath))
				{
					XDocument svgDoc = XDocument.Load(svgFilePath);

					// Define XNamespace for SVG
					XNamespace svgNamespace = "http://www.w3.org/2000/svg";
					XNamespace xlinkNamespace = "http://www.w3.org/1999/xlink";

					// Iterate through fallacies
					foreach (var fallacy in fallacies)
					{
						string title = TitleFunc(fallacy);

						// Find the text elements in the SVG
						var textElements = svgDoc.Descendants(svgNamespace + "text").Where(t => t.Value.Contains(title)).ToList();

						// Filter multiple matches
						XElement shortestMatch = textElements.MinBy(t => Math.Abs(t.Value.Length - title.Length));

						if (shortestMatch != null)
						{

							if (shortestMatch.Attribute("class") == null ||
							    !shortestMatch.Attribute("class").Value.Contains("node"))
							{
								// Add the missing fallacy fields
								string desc = DescFunc(fallacy);
								string example = ExampleFunc(fallacy);
								string link = LinkFunc(fallacy);


								shortestMatch.SetAttributeValue("description", desc);
								shortestMatch.SetAttributeValue("example", example);
								shortestMatch.SetAttributeValue("link", example);
								shortestMatch.SetAttributeValue("class", "node");

								// Make sure the link is clickable
								XElement linkElem = new XElement(XName.Get("a", svgNamespace.NamespaceName));
								linkElem.SetAttributeValue(XName.Get("href", xlinkNamespace.NamespaceName), link);
								linkElem.SetAttributeValue("target", "_blank");
								shortestMatch.ReplaceWith(linkElem);
								linkElem.Add(shortestMatch);



								//XElement parentGroup = shortestMatch.Parent;
								//XElement newDesc = new XElement(svgNs + "text", desc);
								//XElement newExample = new XElement(svgNs + "text", example);

								//// Set the position of the new elements
								//newDesc.SetAttributeValue("x", shortestMatch.Attribute("x").Value);
								//newDesc.SetAttributeValue("y", (float.Parse(shortestMatch.Attribute("y").Value) + 20).ToString());
								//newExample.SetAttributeValue("x", shortestMatch.Attribute("x").Value);
								//newExample.SetAttributeValue("y", (float.Parse(shortestMatch.Attribute("y").Value) + 40).ToString());

								//parentGroup.Add(newDesc);
								//parentGroup.Add(newExample);

							}
							else
							{
								AnsiConsole.WriteLine("[red]Found existing content in svg mindmap. Updates aborted. SVG must be regenerated before applying new changes.[/]");
								return;
							}



						}
					}

					// Save the modified SVG file
					using (var writer = XmlWriter.Create(svgFilePath, new XmlWriterSettings { Indent = true }))
					{
						svgDoc.Save(writer);
					}
					Console.WriteLine($"SVG file {svgFilePath} successfully updated!");
				}
				else
				{
					Console.WriteLine($"SVG file {svgFilePath} not found. Please make sure it exists.");
				}
			}


		}


		public MindMapDocumentConfig CloneMindMap()
		{
			var clone = new MindMapDocumentConfig()
			{
				EnableSVGUpdate = this.EnableSVGUpdate,
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
				InsertCardsThumbnails = this.InsertCardsThumbnails
			};

			return clone;
		}


		public object Clone()
		{
			return CloneMindMap();
		}
	}








}