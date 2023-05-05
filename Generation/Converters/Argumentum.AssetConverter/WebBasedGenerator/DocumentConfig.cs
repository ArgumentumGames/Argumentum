using Argumentum.AssetConverter.Mindmapper;
using Sprache;
using System;
using System.Collections.Generic;
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
    public class DocumentConfig 
	{
	    [DataMember(Order = 0)]
	    public virtual bool Enabled { get; set; } = true;

		[DataMember(Order = 1)]
		public virtual string DocumentName { get; set; }

		private static readonly IJsonFormatterResolver s_defaultResolver = StandardResolver.AllowPrivateExcludeNull;


		public List<(string sourceLanguage, string targetLanguage)> Translations { get; set; } =
			new List<(string sourceLanguage, string targetLanguage)>();


		
	}


	





}