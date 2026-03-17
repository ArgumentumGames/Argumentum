using Argumentum.AssetConverter.Mindmapper;
using ImageMagick;
using Sprache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;
using Utf8Json;
using Utf8Json.Resolvers;
using JsonSerializer = Utf8Json.JsonSerializer;

namespace Argumentum.AssetConverter
{
    public class DocumentConfig: ICloneable
	{
	    [DataMember(Order = 0)]
	    public virtual bool Enabled { get; set; } = true;

		[DataMember(Order = 1)]
		public virtual string DocumentName { get; set; }

		//private static readonly IJsonFormatterResolver s_defaultResolver = StandardResolver.AllowPrivateExcludeNull;


		public List<(string sourceLanguage, string targetLanguage)> Translations { get; set; } =
			new List<(string sourceLanguage, string targetLanguage)>();

		public int TargetDensity { get; set; } = 0;

		public MagickFormat ImageFormat { get; set; } = MagickFormat.Png;


		public string TemplatePathRelease { get; set; }


		public string TemplatePathDebug { get; set; }

		public string GetDensityDirectory(string baseDirectory)
		{
			var densityDirectory = Path.Combine(baseDirectory, $@".\density-{TargetDensity}\");
			if (!Directory.Exists(densityDirectory))
			{
				Directory.CreateDirectory(densityDirectory);
			}
			return densityDirectory;
		}

		protected virtual DocumentConfig GetClone()
		{
			return (DocumentConfig)this.MemberwiseClone(); 
		}

		public object Clone()
		{
			var toReturn = GetClone();
			toReturn.Translations = new List<(string sourceLanguage, string targetLanguage)>(this.Translations);
			return toReturn;
		}




	}


	





}