using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using ImageMagick;

namespace Argumentum.AssetConverter
{





    public class WebBasedGeneratorConfig
	{

		

		public bool ShowInfoLogs { get; set; } = true;

		public bool HeadLessBrowser { get; set; }



		public bool EnableParallelism { get; set; } = true;

		public int MaxDegreeOfParallelismCardpen { get; set; } = 3;

		public int MaxDegreeOfParallelismCardpenTranslations { get; set; } = 2;

		public int MaxDegreeOfParallelismImages { get; set; } = 3;

		public int MaxDegreeOfParallelismImageTranslations { get; set; } = 2;

		public int MaxDegreeOfParallelismDocuments { get; set; } = 4;

		


	

		



		public string ReleaseCardpenUrl { get; set; } = @"https://argumentumgames.github.io/Argumentum/Generation/CardPen/index.html";
		public string LocalCardpenUrl { get; set; } = @"http://cardpen.dnndev.me/Generation/CardPen/index.html";



		public bool UseLocalCardpen { get; set; } = false;



		[IgnoreDataMember]
		[JsonIgnore]
		public string CardpenUrl => UseLocalCardpen ? LocalCardpenUrl : ReleaseCardpenUrl;

		

		

		//private string GetSimpleTypeName(Type objType)
		//{
		//	return $"{objType.FullName}, {objType.Assembly.GetName().Name}";
		//}


		public List<CardSetConfig> CardSets { get; set; } = new List<CardSetConfig>();

		public List<CardSetDocumentConfig> CardSetDocuments { get; set; } = new List<CardSetDocumentConfig>();


		



		

		

		


		public async Task<bool> Apply(AssetConverterConfig config)
		{
			var generator = new WebBasedGenerator(){AssetConverterConfig = config, Config = this};
			await generator.Run().ConfigureAwait(false);
			return true;
		}


	}
}
