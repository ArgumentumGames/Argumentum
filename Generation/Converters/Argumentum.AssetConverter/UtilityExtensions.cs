using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading.Tasks;

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


        public static bool PathIsUrl(this string path)
        {
	        if (File.Exists(path))
		        return false;
	        try
	        {
		        Uri uri = new Uri(path);
		        return true;
	        }
	        catch (Exception)
	        {
		        return false;
	        }
        }


        public static async Task<DocumentPayload> GetDocumentPayload(this string docPath)
        {
	        byte[] content;
	        string fileName;
	        string mimeType = "";
	        if (docPath.PathIsUrl())
	        {
		        var urlFile = new Uri(docPath);

		        // Télécharger le fichier à partir de l'URL spécifiée
		        using var client = new HttpClient();
                
		        var response = await client.GetAsync(urlFile);
		        response.EnsureSuccessStatusCode();
		        fileName = response.Content.Headers.ContentDisposition?.FileName ??
		                   System.IO.Path.GetFileName(urlFile.LocalPath); //"file.json";
		        mimeType = response.Content.Headers.ContentType?.MediaType;
		        content = await response.Content.ReadAsByteArrayAsync();

		        Console.WriteLine($"Downloaded Document {docPath}");
		        //Console.WriteLine($"Content :\n\n {Encoding.UTF8.GetString(content)}");

			}
	        else
	        {
		        var fullPath = docPath;
		        if (!Path.IsPathFullyQualified(docPath))
		        {
			        fullPath = Path.Combine(Environment.CurrentDirectory, docPath);
		        }

		        fileName = Path.GetFileName(fullPath);

		        content = await File.ReadAllBytesAsync(fullPath);

	        }

	        return new DocumentPayload() { FileName = fileName, Content = content, MimeType = mimeType };

        }


	}
}