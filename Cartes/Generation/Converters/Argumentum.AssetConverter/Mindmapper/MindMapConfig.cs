using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Web;

namespace Argumentum.AssetConverter.Mindmapper
{
   public class MindMapConfig
    {


        public string SourcePath { get; set; } = @"..\..\..\Data\Mindmap\Argumentum Fallacies - Taxonomy.csv";
        public string DestPath { get; set; } = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2.mm";

        public string TitleExpression { get; set; } = @"{fallacy.TextFr}";

        public string DescriptionExpression { get; set; } = 
@"
<p>
    <font size='4'>{HttpUtility.HtmlEncode(fallacy.DescFr)}</font>
</p>
";

        public string CardExpression { get; set; } =
            @"
<p>
    <img src=""../../bin/Debug/netcoreapp3.1/Target/Images/density-0/Fallacies-Web-Thumbnails/{fallacy.FileName}.png"" width=""60"" height=""60""/>{fallacy.TextFr}
</p>
";

        //
        // https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cartes/Fallacies/Assets/Fallacy-front/{fallacy.Path}.png

        public string ExampleExpression { get; set; } = 
@"
<p>
    <i>{HttpUtility.HtmlEncode(fallacy.ExampleFr)}</i>
</p>
";

        public string LinkExpression { get; set; } = @"{fallacy.LinkFrFallbackEn}";

        [IgnoreDataMember]
        [JsonIgnore]
        public Func<Fallacy, string> TitleFunc
        {
            get
            {
                return fallacy => TitleExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"{fallacy.TextFr}";
            }
        }

        [IgnoreDataMember]
        [JsonIgnore]
        public Func<Fallacy, string> DescFunc
        {
            get
            {
                return fallacy => DescriptionExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"<font size='4'>{HttpUtility.HtmlEncode(fallacy.DescFr)}</font>";
            }
        }

        [IgnoreDataMember]
        [JsonIgnore]
        public Func<Fallacy, string> CardFunc
        {
            get
            {
                return fallacy => CardExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"<font size='4'>{HttpUtility.HtmlEncode(fallacy.DescFr)}</font>";
            }
        }



        [IgnoreDataMember]
        [JsonIgnore]
        public Func<Fallacy, string> ExampleFunc
        {
            get
            {
                return fallacy => ExampleExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"<i>{HttpUtility.HtmlEncode(fallacy.ExampleFr)}</i>";
            }
        }

        [IgnoreDataMember]
        [JsonIgnore]
        public Func<Fallacy, string> LinkFunc
        {
            get
            {
                return fallacy => LinkExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"{fallacy.LinkFr}";
            }
        } 

        public Dictionary<int, string> Colors { get; set; } = new Dictionary<int, string>()
        {
            {1, "#8605ab"},
            {2, "#ff66eb"},
            {3, "#08af93"},
            {4, "#8dc801"},
            {5, "#0054a4"},
            {6, "#ffc307"},
            {7, "#dc0f0a"},

        };

       


        public List<int> FontSizes { get; set; } = new List<int>(new[] { 30, 50, 40, 30, 30, 30, 25, 23, 23, 23, 23 });


        public List<int> EdgeSizes { get; set; } = new List<int>(new[] { 8, 4, 2, 1});

    }

    



}