using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Argumentum.AssetConverter
{
    public static class UtilityExtensions
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
                        config.CustomTypeProvider = new CustomTypeProvider() { DefaultProvider = config.CustomTypeProvider };

                        var e = System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(config, parameters.ToArray(), null, matchToken);
                        tokenDelegate = e.Compile();
                        _CachedIntepolationExpressions[key] = tokenDelegate;
                    }
                    return (tokenDelegate.DynamicInvoke(context.Values.ToArray()) ?? "").ToString();
                });
        }


        public static T[][] ToJaggedArray<T>(this IList<T> source, int columnLength)
        {
            var rowLength = (int) Math.Ceiling((float)source.Count / (float)columnLength);
            var toReturn = new T[rowLength][];
            for (int rowIndex = 0; rowIndex < rowLength; rowIndex++)
            {
                var startIndex = rowIndex * columnLength;
                var nbRowItems = Math.Min(columnLength, source.Count - startIndex);
                toReturn[rowIndex] = new T[nbRowItems];
                for (int colIndex = 0; colIndex < columnLength; colIndex++)
                {
                    var globalIndex = startIndex + colIndex;
                    if (globalIndex < source.Count)
                    {
                        toReturn[rowIndex][colIndex] = source[globalIndex];
                    }
                }
            }

            return toReturn;
        }

        public static T[] Flatten<T>(this T[][] source)
        {
            return source.SelectMany(x => x).ToArray();
        }

    }
}