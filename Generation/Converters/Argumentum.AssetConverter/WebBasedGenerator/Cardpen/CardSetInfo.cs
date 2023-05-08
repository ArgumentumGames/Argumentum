using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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

		public bool SkipDataUpdate { get; set; }

		public string JsonFilePath { get; set; }

		public bool PauseForEdits { get; set; }

		public List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> FieldsLocalization { get; set; }
			= new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>();

		
		public int Dpi { get; set; }

		public int RowsetNb { get; set; }


		public async Task<CardSetPayload> GetCardSetDocument()
		{
			if (string.IsNullOrEmpty(JsonFilePath))
			{
				return null;
			}
			var docPayload = await JsonFilePath.GetDocumentPayload();
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
		public static string Fallacies = "Fallacies";
		public static string Scenarii = "Scenarii";
		public static string Rules = "Rules";
		public static string Memo = "Memo";
		public static string Fallacies2 = "Fallacies-2";
		public static string Fallacies3 = "Fallacies-3";
		public static string FallaciesPrintAndPlay = "Fallacies-Print&Play";
		public static string ScenariiPrintAndPlay = "Scenarii-Print&Play";
		public static string RulesPrintAndPlay = "Rules-Print&Play";
		public static string MemoPrintAndPlay = "Memo-Print&Play";
		public static string FallaciesWeb = "Fallacies-Web";
		public static string FallaciesWebLight = "Fallacies-Web-Light";
		public static string FallaciesWebThumbnails = "Fallacies-Web-Thumbnails";


	}


}