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

	public string FormatField(string fieldName) => FormatFieldToken(fieldName);

	/// <summary>
	/// Formats a mustache field token's closing-brace pattern (e.g. "Famille" → "Famille}}").
	/// Centralised so the pure static conversion helpers below share a single source of truth
	/// with the instance <see cref="FormatField"/> used by callers and the localization tests.
	/// </summary>
	private static string FormatFieldToken(string fieldName) => $"{fieldName}}}";

	/// <summary>
	/// Pure, output-neutral extraction of the field-conversion transform performed by
	/// <see cref="TranslateCardSetInfo"/>: for each source field whose dest-lang conversion is
	/// present, its mustache token is replaced in the template, and any exception patterns that
	/// absorbed the replacement are back-tracked afterwards. Byte-identical to the previous
	/// inline logic. Extracted so the #216 contract (field present → replaced; field absent →
	/// reported by <see cref="FindAbsentSourceFields"/>) can be unit-tested directly.
	/// </summary>
	public static string ApplyFieldConversions(
		string template,
		IEnumerable<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> fieldConversions,
		IEnumerable<string> exceptionPatterns,
		string destLanguage)
	{
		var exceptionsBacktrack = new List<(string sourcePattern, string destPattern)>();
		foreach (var fieldConversion in fieldConversions)
		{
			var sourceFieldPattern = FormatFieldToken(fieldConversion.sourceFieldName);
			if (fieldConversion.fieldConversions == null || !fieldConversion.fieldConversions.Any())
			{
				continue;
			}
			var conversion = fieldConversion.fieldConversions.FirstOrDefault(convertedField => convertedField.Language == destLanguage);
			if (string.IsNullOrEmpty(conversion.destFieldName))
			{
				continue;
			}
			var destFieldPattern = FormatFieldToken(conversion.destFieldName);
			template = template.Replace(sourceFieldPattern, destFieldPattern);
			foreach (var exception in exceptionPatterns ?? Enumerable.Empty<string>())
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

		return template;
	}

	/// <summary>
	/// Pure capability that surfaces the #216 footgun: the source field names whose mustache
	/// token is NOT present in the template — so <see cref="ApplyFieldConversions"/> would
	/// silently no-op on them (template.Replace returns the string unchanged when the pattern is
	/// absent), leaving untranslated content with no error. This silent false-pass was the root
	/// cause of #216 (Fallacies mapping referenced "Titre"/"Definition"/"Exemple" which never
	/// existed in the templates). NOT yet wired to Logger in production: that one-line wiring is
	/// a deliberate post-gate follow-up to keep this extraction output-neutral during the release
	/// gate (the gate holds even output-neutral pipeline behaviour changes).
	/// </summary>
	public static IReadOnlyList<string> FindAbsentSourceFields(
		string template,
		IEnumerable<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> fieldConversions,
		string destLanguage)
	{
		var absent = new List<string>();
		foreach (var fieldConversion in fieldConversions)
		{
			if (fieldConversion.fieldConversions == null || !fieldConversion.fieldConversions.Any())
			{
				continue;
			}
			var conversion = fieldConversion.fieldConversions.FirstOrDefault(convertedField => convertedField.Language == destLanguage);
			if (string.IsNullOrEmpty(conversion.destFieldName))
			{
				continue;
			}
			var sourceFieldPattern = FormatFieldToken(fieldConversion.sourceFieldName);
			if (!template.Contains(sourceFieldPattern))
			{
				absent.Add(fieldConversion.sourceFieldName);
			}
		}
		return absent;
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

		// Output-neutral pure extraction (#216 root-cause contract). Field tokens absent from the
		// template are silently skipped by template.Replace; FindAbsentSourceFields is the
		// fail-loud companion that surfaces them (wiring to Logger is a post-gate follow-up).
		template = ApplyFieldConversions(template, fieldConversions, this.ExceptionPatterns, languages.destLang);

		template = DoStaticConversions(template, languages.destLang);


		var returnDoc = sourceCardSetPayload.CardSetDocument.Clone();
		returnDoc.mustache = template;


		var newFileName = GetLocalizedFileName(sourceCardSetPayload.FileName, languages.sourceLang, languages.destLang);

		return new CardSetPayload() { CardSetDocument = returnDoc, FileName = newFileName };
	}



}