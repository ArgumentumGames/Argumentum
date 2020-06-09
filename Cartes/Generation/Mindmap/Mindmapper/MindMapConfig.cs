using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Web;

namespace Mindmapper
{
   public class MindMapConfig
    {


        public string SourcePath { get; set; } = @"..\..\..\Data\Argumentum Fallacies - Taxonomy.csv";
        public string DestPath { get; set; } = @"..\..\..\Data\\Argumentum_Fallacies_MindMap_Fr.mm";

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

    public static class Extensions
    {




        public static long ToUnixTime(this DateTime objDate)
        {
            return ((DateTimeOffset)objDate).ToUnixTimeSeconds();

        }

        public static String GetRawExtensionUpper(this string fileName)
        {
            return Path.GetExtension(fileName)?.TrimStart('.').ToUpperInvariant();

        }

    


        private static Regex _InterpolateRegex = new Regex(@"{(.+?)}", RegexOptions.Compiled);

        private static Dictionary<string, Delegate> _CachedIntepolationExpressions = new Dictionary<string, Delegate>();

        public static string Interpolate(this string value, Dictionary<string, Object> context)
        {
            return _InterpolateRegex.Replace(value,
                match =>
                {
                    var matchToken = match.Groups[1].Value;
                    var key = $"{value}/{matchToken}";
                    if (!_CachedIntepolationExpressions.TryGetValue(key, out var tokenDelegate))
                    {
                        var parameters = new List<ParameterExpression>(context.Count);
                        foreach (var contextObject in context)
                        {
                            var p = Expression.Parameter(contextObject.Value.GetType(), contextObject.Key);
                            parameters.Add(p);
                        }
                        ParsingConfig config = new ParsingConfig();
                        config.CustomTypeProvider = new CustomTypeProvider(){DefaultProvider = config.CustomTypeProvider};
                        
                        var e = System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(config, parameters.ToArray(), null, matchToken);
                        tokenDelegate = e.Compile();
                        _CachedIntepolationExpressions[key] = tokenDelegate;
                    }
                    return (tokenDelegate.DynamicInvoke(context.Values.ToArray()) ?? "").ToString();
                });
        }

    }


    public class CustomTypeProvider : IDynamicLinkCustomTypeProvider
    {

        public IDynamicLinkCustomTypeProvider DefaultProvider { get; set; }


        public HashSet<Type> GetCustomTypes()
        {
            HashSet<Type> types = DefaultProvider.GetCustomTypes();
            types.Add(typeof(HttpUtility));
            return types;
        }

        public Type ResolveType(string typeName)
        {
            return DefaultProvider.ResolveType(typeName);
        }

        public Type ResolveTypeBySimpleName(string simpleTypeName)
        {
            return DefaultProvider.ResolveTypeBySimpleName(simpleTypeName);
        }
    }



}