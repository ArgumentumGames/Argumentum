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
			await TranslateCardSetInfo(source.FaceCardSetInfo, localization.FrontFieldConversions, languages);
		var backTranslated =
			await TranslateCardSetInfo(source.BackCardSetInfo, localization.BackFieldConversions, languages);

		return (frontTranslated, backTranslated);

	}

	public async Task<CardSetPayload> TranslateCardSetInfo(CardSetInfo source, List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> conversions, (string sourceLang, string destLang) languages)
	{

		var sourceCardSetPayload = await source.GetCardSetDocument();
		var template = sourceCardSetPayload.CardSetDocument.mustache;
		foreach (var fieldConversion in conversions)
		{
			var sourceFieldPattern = FormatField(fieldConversion.sourceFieldName);
			var convertedField =
				fieldConversion.fieldConversions.First(convertedField => convertedField.Language == languages.destLang).destFieldName;
			var destFieldPattern = FormatField(convertedField);
			template = template.Replace(sourceFieldPattern, destFieldPattern);
		}

		var returnDoc = sourceCardSetPayload.CardSetDocument.Clone();
		returnDoc.mustache = template;

		
		var newFileName = GetLocalizedFileName(sourceCardSetPayload.FileName, languages.sourceLang, languages.destLang);

		return new CardSetPayload(){CardSetDocument = returnDoc, FileName = newFileName};
	}

	public string FormatField(string fieldName)
	{
		return $"{{{fieldName}";
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

	public List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> FrontFieldConversions { get; set; }
		= new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>();

	public List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> BackFieldConversions { get; set; }
		= new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>();


}