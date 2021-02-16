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
        public string DestPath { get; set; } = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr.mm";

        public string TitleExpression { get; set; } = @"{fallacy.TextFr}";

        public string DescriptionExpression { get; set; } = 
@"
<p>
    <font size='4'>{HttpUtility.HtmlEncode(fallacy.DescFr)}</font>
</p>
";

        public string ExampleExpression { get; set; } = 
@"
<p>
    <i>{HttpUtility.HtmlEncode(fallacy.ExampleFr)}</i>
</p>
";

        public string LinkExpression { get; set; } = @"{fallacy.LinkFr}";

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
            {2, "#daaf02"},
            {3, "#6ab203"},
            {4, "#01a987"},
            {5, "#022cb2"},
            {6, "#bc0546"},
            {7, "#af1313"},

        };

       


        public List<int> FontSizes { get; set; } = new List<int>(new[] { 30, 50, 40, 30, 30, 30, 25, 23, 23, 23, 23 });


        public List<int> EdgeSizes { get; set; } = new List<int>(new[] { 8, 4, 2, 1});

    }

    



}