using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using CsvHelper;
using Utf8Json;
using Utf8Json.Formatters;
using Utf8Json.Resolvers;


namespace Argumentum.AssetConverter.Mindmapper
{
    public class MindMapCreatorConfig
    {

	     static MindMapCreatorConfig()
	    {
            //Attempt to fix missing Type/assembly.
		    var tempConverter = new TypeConverter();
	    }

        public MindMapCreatorConfig()
        {
            var newConfig = new List<MindMapDocumentConfig>();
            newConfig.Add(new MindMapDocumentConfig(){DocumentName = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2.mm" });
            
            var frConfigCards = new MindMapDocumentConfig() { DocumentName = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2_cards.mm" };
            frConfigCards.InsertCardsThumbnails = true;
           
            newConfig.Add(frConfigCards);
            
            var enConfig = new MindMapDocumentConfig();
            enConfig.DocumentName = enConfig.DocumentName.Replace("Fr", "En");
            enConfig.DescriptionExpression = enConfig.DescriptionExpression.Replace("Fr", "En");
            enConfig.TitleExpression = enConfig.TitleExpression.Replace("Fr", "En");
            enConfig.ExampleExpression = enConfig.ExampleExpression.Replace("Fr", "En");
            enConfig.LinkExpression = enConfig.LinkExpression.Replace("Fr", "En");
            newConfig.Add(enConfig);
            
            var enConfigCards = new MindMapDocumentConfig();
            enConfigCards.InsertCardsThumbnails = true;
            enConfigCards.DocumentName = enConfig.DocumentName.Replace(".mm", "_cards.mm");
            enConfigCards.DescriptionExpression = enConfig.DescriptionExpression;
            enConfigCards.TitleExpression = enConfig.TitleExpression;
            enConfigCards.ExampleExpression = enConfig.ExampleExpression;
            enConfigCards.LinkExpression = enConfig.LinkExpression;
            enConfigCards.CardExpression = enConfig.CardExpression.Replace("Fr", "En");
            newConfig.Add(enConfigCards);

            MindMaps = newConfig;
        }


        public void Run(string[] args)
        {

            foreach (var config in this.MindMaps)
            {

                var fallacies = Fallacy.LoadFallacies(config.DataSet);
                config.GenerateMindMapFile( fallacies, null, "", "");

            }
            Console.WriteLine($"Generation finished, press any key to close");
            Console.ReadKey();

        }

        public List<MindMapDocumentConfig> MindMaps { get; set; }
    }
}
