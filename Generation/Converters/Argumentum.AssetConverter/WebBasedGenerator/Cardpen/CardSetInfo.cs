using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Utf8Json;

namespace Argumentum.AssetConverter
{
	public class CardSetInfo
	{
		//[JsonIgnore()]
		//public bool IsSet => (CardSetType == CardSetType.ExampleByName && !string.IsNullOrEmpty(ExampleName))
		//					 || (CardSetType == CardSetType.CustomJson && !string.IsNullOrEmpty(CustomJsonFileName));

		//public CardSetType CardSetType { get; set; }

		//public string ExampleName { get; set; }

		public string DataSet { get; set; }

		public string CsvFilterField { get; set; }
		public List<string> CsvFilterValues { get; set; } = new List<string>();

		public bool SkipDataUpdate { get; set; }


		public string GetJsonFilePath(AssetConverterConfig config)
	{
		var useDebug = config.UseDebugParams;
		Logger.Log($"[CardSetInfo] JsonFilePathDebug='{JsonFilePathDebug}', JsonFilePathRelease='{JsonFilePathRelease}'");
		var selectedPath = useDebug ? JsonFilePathDebug : JsonFilePathRelease;
		Logger.Log($"[CardSetInfo] UseDebugParams={useDebug}, Selected path: '{selectedPath}'");
		return selectedPath;
	}
		public string JsonFilePathRelease { get; set; }
		public string JsonFilePathDebug { get; set; }

		public bool PauseForEdits { get; set; }

		public List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> FieldsLocalization { get; set; }
			= new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>();


		public int Dpi { get; set; }
		public string CardSize { get; set; }

		public int RowsetNb { get; set; }


		public async Task<CardSetPayload> GetCardSetDocument(AssetConverterConfig config)
		{
			var jsonFilePath = GetJsonFilePath(config);
			if (string.IsNullOrEmpty(jsonFilePath))
			{
				return null;
			}
			var docPayload = await jsonFilePath.GetDocumentPayload();
			//var strContent = Encoding.UTF8.GetString(docPayload.Content);

			var cardSetDoc = JsonSerializer.Deserialize<CardSetDocument>(docPayload.Content);

			// Transform relative filesystem paths to absolute web paths when using local CardPen
			if (config.WebBasedGeneratorConfig.UseLocalCardpen)
			{
				if (!string.IsNullOrEmpty(cardSetDoc.css))
				{
					cardSetDoc.css = cardSetDoc.css.Replace("../../Cards/", "/Cards/");
				}
				if (!string.IsNullOrEmpty(cardSetDoc.mustache))
				{
					cardSetDoc.mustache = cardSetDoc.mustache.Replace("../../Cards/", "/Cards/");
				}
				if (!string.IsNullOrEmpty(cardSetDoc.extCSS))
				{
					cardSetDoc.extCSS = cardSetDoc.extCSS.Replace("../../Cards/", "/Cards/");
				}
				Logger.Log($"[CardSetInfo] Transformed paths from '../../Cards/' to '/Cards/' for local CardPen");
			}

			return new CardSetPayload(){CardSetDocument = cardSetDoc, FileName = docPayload.FileName} ;

		}

	}


	public class CardSetPayload
	{
		public string FileName { get; set; }
		public CardSetDocument CardSetDocument { get; set; }

		[JsonIgnore]
		public System.Type CsvType { get; set; }

		public string GetMimeType()
		{
			return "application/json";
		}
	}

	public class CardSetPayloadDto
	{
		public string FileName { get; set; }
		public CardSetDocument CardSetDocument { get; set; }
		public string CsvTypeName { get; set; }

		public static CardSetPayloadDto FromCardSetPayload(CardSetPayload payload)
		{
			// Assurer que les champs textuels ne sont jamais nuls pour éviter les erreurs JavaScript.
			var doc = payload.CardSetDocument;
			if (doc != null)
			{
				doc.csv = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(doc.csv ?? string.Empty));
				doc.css = doc.css ?? string.Empty;
				doc.mustache = doc.mustache ?? string.Empty;
			}

			return new CardSetPayloadDto
			{
				FileName = payload.FileName,
				CardSetDocument = payload.CardSetDocument,
				CsvTypeName = payload.CsvType?.FullName
			};
		}
	}


	public static class KnownCardSets
	{
		public static readonly string Fallacies = "Fallacies";
		public static readonly string Scenarii = "Scenarii";
		public static readonly string Rules = "Rules";
		public static readonly string Memo = "Memo";
		public static readonly string Fallacies2 = "Fallacies-2";
		public static readonly string Fallacies3 = "Fallacies-3";
		public static readonly string FallaciesPrintAndPlay = "Fallacies-Print&Play";
		public static readonly string ScenariiPrintAndPlay = "Scenarii-Print&Play";
		public static readonly string RulesPrintAndPlay = "Rules-Print&Play";
		public static readonly string MemoPrintAndPlay = "Memo-Print&Play";
		// #645 — Print&Play "Light" (historical sample, print_and_play=1) vs "Standard" (all cards, free digital game).
		// FallaciesPrintAndPlayLight = the 35 Feb-2022 demo fallacies (filter print_and_play=1), vs FallaciesPrintAndPlay = all 176 real cards.
		// ScenariiPrintAndPlayFull = all 167 scenarii (no filter), vs ScenariiPrintAndPlay = the 27 demo scenarii (print_and_play=1).
		public static readonly string FallaciesPrintAndPlayLight = "Fallacies-Print&Play-Light";
		public static readonly string ScenariiPrintAndPlayFull = "Scenarii-Print&Play-Full";
		public static readonly string FallaciesWeb = "Fallacies-Web";
		public static readonly string FallaciesWebLight = "Fallacies-Web-Light";
		public static readonly string FallaciesWebThumbnails = "Fallacies-Web-Thumbnails";
		public static readonly string Virtues = "Virtues";
	}


}
