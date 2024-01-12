using HarfBuzzSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter;

public class LocalizationConfig
{

	public bool Enabled { get; set; }

	public string DefaultLanguage { get; set; } = "fr";


	public List<CardSetLocalization> CardSetLocalizations { get; set; } = new List<CardSetLocalization>();

	public  List<DocumentLocalization> MindMapLocalization { get; set; } = new List<DocumentLocalization>();

	public async Task<(CardSetPayload front, CardSetPayload back)> TranslateCardSet(CardSetConfig source,
		(string sourceLang, string destLang) languages, AssetConverterConfig config)
	{
		
		
		var localization = CardSetLocalizations.First(setLocalization => setLocalization.CardSetNames.Contains(source.Name));
		var frontTranslated = await localization.TranslateCardSetInfo(source.FaceCardSetInfo, true, languages, config);
		CardSetPayload backTranslated = null;
		if (!string.IsNullOrEmpty(source.BackCardSetInfo.GetJsonFilePath(config)))
		{
			backTranslated = await localization.TranslateCardSetInfo(source.BackCardSetInfo, false, languages, config);
		}
		 

		return (frontTranslated, backTranslated);

	}

	public List<string> BuildLanguageList(List<(string sourceLanguage, string targetLanguage)> translations)
	{
		var targetLanguages = new List<string>(new[] { DefaultLanguage });
		if (Enabled)
		{
			targetLanguages.AddRange(translations.Select(t => t.targetLanguage));
		}
		return targetLanguages;
	}



}

//public enum Supportedlanguages
//{
//	fr,
//	en,
//	ru
//}