using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Argumentum.AssetConverter
{
    public class DocumentConfig
    {
	    [DataMember(Order = 0)]
	    public virtual bool Enabled { get; set; } = true;

		[DataMember(Order = 1)]
		public virtual string DocumentName { get; set; }

      

		public List<(string sourceLanguage, string targetLanguage)> Translations { get; set; } =
			new List<(string sourceLanguage, string targetLanguage)>();

		
    }
}