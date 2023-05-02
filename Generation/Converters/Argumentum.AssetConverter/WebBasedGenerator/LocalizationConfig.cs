using HarfBuzzSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter;

public class LocalizationConfig
{

	public bool Enabled { get; set; }

	public string DefaultLanguage { get; set; } = "fr";


	public List<CardSetLocalization> CardSetLocalizations { get; set; } = new List<CardSetLocalization>();

	public async Task<(CardSetPayload front, CardSetPayload back)>  TranslateCardSet(CardSetConfig source, (string sourceLang, string destLang) languages)
	{
		
		
		var localization = CardSetLocalizations.First(setLocalization => setLocalization.CardSetNames.Contains(source.Name));
		var frontTranslated =
			await TranslateCardSetInfo(source.FaceCardSetInfo, localization.FrontFieldConversions, localization.StaticConversions, localization.ExceptionPatterns, languages);
		CardSetPayload backTranslated = null;
		if (!string.IsNullOrEmpty(source.BackCardSetInfo.JsonFilePath))
		{
			backTranslated = await TranslateCardSetInfo(source.BackCardSetInfo, localization.BackFieldConversions, localization.StaticConversions, localization.ExceptionPatterns, languages);
		}
		 

		return (frontTranslated, backTranslated);

	}

	public async Task<CardSetPayload> TranslateCardSetInfo(CardSetInfo source, 
		List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> fieldConversions,
		List<(string sourceText, List<(string Language, string destText)> textConversions)> staticConversions, List<string> exceptionList,

	(string sourceLang, string destLang) languages)
	{

		var sourceCardSetPayload = await source.GetCardSetDocument();
		var template = sourceCardSetPayload.CardSetDocument.mustache;
		var exceptionsBacktrack = new List<(string sourcePattern, string destPattern)>();
		foreach (var fieldConversion in fieldConversions)
		{
			var sourceFieldPattern = FormatField(fieldConversion.sourceFieldName);
			var convertedField =
				fieldConversion.fieldConversions.First(convertedField => convertedField.Language == languages.destLang).destFieldName;
			var destFieldPattern = FormatField(convertedField);
			template = template.Replace(sourceFieldPattern, destFieldPattern);
			foreach (var exception in exceptionList)
			{
				var exceptionPattern = exception.Replace(sourceFieldPattern, destFieldPattern);
				if (exceptionPattern!= exception)
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

		foreach (var staticConversion in staticConversions)
		{
			var convertedText =
				staticConversion.textConversions.First(convertedText => convertedText.Language == languages.destLang).destText;
			template = template.Replace(staticConversion.sourceText, convertedText);
		}

		
		

		var returnDoc = sourceCardSetPayload.CardSetDocument.Clone();
		returnDoc.mustache = template;

		
		var newFileName = GetLocalizedFileName(sourceCardSetPayload.FileName, languages.sourceLang, languages.destLang);

		return new CardSetPayload(){CardSetDocument = returnDoc, FileName = newFileName};
	}

	public string FormatField(string fieldName)
	{
		return $"{fieldName}}}";
	}


	public static string GetLocalizedFileName(string fileName, string defaultLanguage, string targetLanguage)
	{
		string newFileName;
		if (fileName.Contains($"_{defaultLanguage}"))
		{
			newFileName = fileName.Replace($"_{defaultLanguage}", $"_{targetLanguage}");
		}
		else
		{
			string extension = Path.GetExtension(fileName);
			newFileName = fileName.Replace(extension, $"_{targetLanguage}{extension}");
		}
		return newFileName;
	}


}

//public enum Supportedlanguages
//{
//	fr,
//	en,
//	ru
//}



public class CardSetLocalization
{

	public List<string> CardSetNames { get; set; } = new List<string>();

	public List<(string sourceText, List<(string Language, string destText)> textConversions)> StaticConversions { get; set; }
		= new List<(string sourceText, List<(string Language, string destText)> textConversions)>();

	public List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> FrontFieldConversions { get; set; }
		= new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>();

	public List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> BackFieldConversions { get; set; }
		= new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>();

	public List<string> ExceptionPatterns { get; set; } = new List<string>();


}