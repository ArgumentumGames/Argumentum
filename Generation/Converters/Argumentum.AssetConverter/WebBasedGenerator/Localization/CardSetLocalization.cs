using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter;

public class CardSetLocalization:DocumentLocalization
{

	public List<string> CardSetNames { get; set; } = new List<string>();

	

	public List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> FrontFieldConversions { get; set; }
		= new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>();

	public List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> BackFieldConversions { get; set; }
		= new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>();

	
	public List<string> ExceptionPatterns { get; set; } = new List<string>();

	public string FormatField(string fieldName)
	{
		return $"{fieldName}}}";
	}


	public static string GetLocalizedFileName(string fileName, string defaultLanguage, string targetLanguage)
	{
		if (string.IsNullOrEmpty(fileName))
		{
			return fileName;
		}

		string newFileName;
		if (fileName.Contains($"_{defaultLanguage}"))
		{
			newFileName = fileName.Replace($"_{defaultLanguage}", $"_{targetLanguage}");
		}
		else
		{
			string extension = Path.GetExtension(fileName);
			if (string.IsNullOrEmpty(extension))
			{
				// If there's no extension, just append the language code.
				newFileName = $"{fileName}_{targetLanguage}";
			}
			else
			{
				// Safely replace the extension part
				var fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
				newFileName = $"{fileNameWithoutExt}_{targetLanguage}{extension}";
			}
		}
		return newFileName;
	}

	public async Task<CardSetPayload> TranslateCardSetInfo(CardSetInfo source,
		bool front, (string sourceLang, string destLang) languages, AssetConverterConfig config)
	{
		var fieldConversions = this.FrontFieldConversions;
		if (!front)
		{
			fieldConversions = this.BackFieldConversions;
		}
		var sourceCardSetPayload = await source.GetCardSetDocument(config);
		if (sourceCardSetPayload?.CardSetDocument == null)
		{
			Logger.Log($"[CardSetLocalization] No CardSetDocument found for translation ({languages.sourceLang} → {languages.destLang}). Returning null.");
			return null;
		}
		var template = sourceCardSetPayload.CardSetDocument.mustache;
		var exceptionsBacktrack = new List<(string sourcePattern, string destPattern)>();
		foreach (var fieldConversion in fieldConversions)
		{
			var sourceFieldPattern = FormatField(fieldConversion.sourceFieldName);
			if (fieldConversion.fieldConversions == null || !fieldConversion.fieldConversions.Any())
			{
				continue;
			}
			var conversion = fieldConversion.fieldConversions.FirstOrDefault(convertedField => convertedField.Language == languages.destLang);
			if (string.IsNullOrEmpty(conversion.destFieldName))
			{
				continue;
			}
			var destFieldPattern = FormatField(conversion.destFieldName);
			template = template.Replace(sourceFieldPattern, destFieldPattern);
			foreach (var exception in this.ExceptionPatterns)
			{
				var exceptionPattern = exception.Replace(sourceFieldPattern, destFieldPattern);
				if (exceptionPattern != exception)
				{
					exceptionsBacktrack.Add((exceptionPattern, exception));
				}
			}
		}

		foreach (var exceptionBacktrackTransform in exceptionsBacktrack)
		{
			template = template.Replace(exceptionBacktrackTransform.sourcePattern,
				exceptionBacktrackTransform.destPattern);
		}


		template = DoStaticConversions(template, languages.destLang);


		var returnDoc = sourceCardSetPayload.CardSetDocument.Clone();
		returnDoc.mustache = template;


		var newFileName = GetLocalizedFileName(sourceCardSetPayload.FileName, languages.sourceLang, languages.destLang);

		return new CardSetPayload() { CardSetDocument = returnDoc, FileName = newFileName };
	}



}