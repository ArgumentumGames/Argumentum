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

		
		public string GetJsonFilePath(WebBasedGeneratorConfig config) => config.UseDebugParams() ? JsonFilePathDebug : JsonFilePathRelease;
		public string JsonFilePathRelease { get; set; }
		public string JsonFilePathDebug { get; set; }

		public bool PauseForEdits { get; set; }

		public List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> FieldsLocalization { get; set; }
			= new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>();

		
		public int Dpi { get; set; }

		public int RowsetNb { get; set; }


		public async Task<CardSetPayload> GetCardSetDocument(WebBasedGeneratorConfig config)
		{
			var jsonFilePath = GetJsonFilePath(config);
			if (string.IsNullOrEmpty(jsonFilePath))
			{
				return null;
			}
			var docPayload = await jsonFilePath.GetDocumentPayload();
			//var strContent = Encoding.UTF8.GetString(docPayload.Content);
			
			var cardSetDoc = JsonSerializer.Deserialize<CardSetDocument>(docPayload.Content);
			return new CardSetPayload(){CardSetDocument = cardSetDoc, FileName = docPayload.FileName} ;

		}

	}


	public class CardSetPayload
	{
		public string FileName { get; set; }
		public CardSetDocument CardSetDocument { get; set; }

		public string GetMimeType()
		{
			return "application/json";
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
		public static readonly string FallaciesWeb = "Fallacies-Web";
		public static readonly string FallaciesWebLight = "Fallacies-Web-Light";
		public static readonly string FallaciesWebThumbnails = "Fallacies-Web-Thumbnails";
		public static readonly string Virtues = "Virtues";
	}


}