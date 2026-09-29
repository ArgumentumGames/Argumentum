using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.Mindmapper
{
	/// <summary>
	/// Shared HTML/SVG wrapper writer for the mind map pipelines (#1614): the generation loop
	/// formerly duplicated in <see cref="FallacyMindMapDocumentConfig"/> and
	/// <see cref="VirtueMindMapDocumentConfig"/>. Honors <c>OverwriteExistingHtmlMaps</c> —
	/// the "silent skip" trap documented in CLAUDE.md (Mind Maps): an existing wrapper file is
	/// left untouched unless overwrite is explicitly enabled, so wrapper regeneration after a
	/// corpus change requires clearing the wrappers (see MindmapWrapperFreshnessGateTests).
	/// </summary>
	public static class MindMapSvgWrapperWriter
	{
		/// <summary>
		/// Writes every HTML wrapper declared by <paramref name="svgMap"/> next to the saved SVG
		/// file: resolves the debug/release template, substitutes the placeholders via
		/// <see cref="MindMapHtmlWrapper.FormatWrapper"/> (#196), and writes UTF-8.
		/// </summary>
		public static async Task GenerateHtmlSvgWrappers(SVGFreemindMap svgMap, AssetConverterConfig config,
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

					// Issue #196: single helper, tested separately (see MindMapHtmlWrapperTests).
					htmlTemplate = MindMapHtmlWrapper.FormatWrapper(htmlTemplate, svgRelativePath, await svgContent());

					File.WriteAllText(htmlFileName, htmlTemplate, Encoding.UTF8);
					Logger.LogSuccess($"Html SVG MindMap wrapper {htmlFileName} successfully saved");
				}
			}
		}
	}
}
