using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json.Serialization;
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


		public string JsonFilePath { get; set; }

		public bool PauseForEdits { get; set; }

		public List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> FieldsLocalization { get; set; }
			= new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>();


		public async Task<CardSetPayload> GetCardSetDocument()
		{
			byte[] content;
			string fileName;
			if (JsonFilePath.PathIsUrl())
			{
				var urlFile = new Uri(JsonFilePath);
				//string filename = System.IO.Path.GetFileName(urlFile.LocalPath);

				// Télécharger le fichier à partir de l'URL spécifiée
				using var client = new HttpClient();
				var response = await client.GetAsync(urlFile);
				response.EnsureSuccessStatusCode();
				fileName = response.Content.Headers.ContentDisposition?.FileName ??
				               System.IO.Path.GetFileName(urlFile.LocalPath); //"file.json";
				var mimeType = response.Content.Headers.ContentType?.MediaType ?? "application/json";
				content = await response.Content.ReadAsByteArrayAsync();

				Console.WriteLine($"Downloaded CardSet {JsonFilePath}");
			}
			else
			{
				var fullPath = JsonFilePath;
				if (!Path.IsPathFullyQualified(JsonFilePath))
				{
					fullPath = Path.Combine(Environment.CurrentDirectory, JsonFilePath);
				}

				fileName = Path.GetFileName(fullPath);

				content = await File.ReadAllBytesAsync(fullPath);

			}

			var toReturn = JsonSerializer.Deserialize<CardSetDocument>(content);
			return new CardSetPayload(){CardSetDocument = toReturn, FileName = fileName} ;

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

}