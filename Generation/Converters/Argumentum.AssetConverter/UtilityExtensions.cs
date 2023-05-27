using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;

namespace Argumentum.AssetConverter
{
	public static class UtilityExtensions
	{



		public static void ExportDataTable(this CsvWriter writer, DataTable dt)
		{
			// Write the header
			foreach (DataColumn column in dt.Columns)
			{
				writer.WriteField(column.ColumnName);
			}
			writer.NextRecord();

			// Write the rows
			foreach (DataRow row in dt.Rows)
			{
				foreach (DataColumn column in dt.Columns)
				{
					writer.WriteField(row[column]);
				}
				writer.NextRecord();
			}
		}



		public static long ToUnixTime(this DateTime objDate)
		{
			return ((DateTimeOffset)objDate).ToUnixTimeSeconds();

		}

		public static String GetRawExtensionUpper(this string fileName)
		{
			return Path.GetExtension(fileName)?.TrimStart('.').ToUpperInvariant();

		}

		static char[] invalidChars = Path.GetInvalidFileNameChars();

		public static string RemoveInvalidFileNameChars(this string fileName)
		{
			return new string(fileName.Where(ch => !invalidChars.Contains(ch)).ToArray());
		}


		private static Regex _InterpolateRegex = new Regex(@"{(.+?)}", RegexOptions.Compiled);

		private static ConcurrentDictionary<string, Delegate> _CachedIntepolationExpressions = new ConcurrentDictionary<string, Delegate>();

		public static string Interpolate(this string value, Dictionary<string, object> context)
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



		//private static ConcurrentDictionary<string, Func<Dictionary<string, object>, string>> _CachedInterpolationDelegates = new ConcurrentDictionary<string, Func<Dictionary<string, object>, string>>();

		//public static string Interpolate(string value, Dictionary<string, object> context)
		//{
		// return _InterpolateRegex.Replace(value, match =>
		// {
		//  var matchToken = match.Groups[1].Value;
		//  var key = $"{value}/{matchToken}";
		//  if (!_CachedInterpolationDelegates.TryGetValue(key, out var interpolationDelegate))
		//  {
		//   var parameters = new List<ParameterExpression> { Expression.Parameter(typeof(Dictionary<string, object>), "context") };
		//   var argumentValues = new List<Expression> { Expression.Constant(context) };

		//   var lambdaExpression = Expression.Lambda<Func<Dictionary<string, object>, string>>(
		//    Expression.Call(
		//     typeof(string).GetMethod("Format", new[] { typeof(string), typeof(object[]) }),
		//     Expression.Constant($"{{{matchToken}}}"),
		//     Expression.NewArrayInit(typeof(object), argumentValues)
		//    ),
		//    parameters
		//   );

		//   interpolationDelegate = lambdaExpression.Compile();
		//   _CachedInterpolationDelegates[key] = interpolationDelegate;
		//  }
		//  return interpolationDelegate(context);
		// });




		public static T[][] ToJaggedArray<T>(this IList<T> source, int columnLength)
		{
			var rowLength = (int)Math.Ceiling((float)source.Count / (float)columnLength);
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

		public static string GetRelativePathFrom(this string referencedPath, string mainPath)
		{
			if (string.IsNullOrEmpty(mainPath) || string.IsNullOrEmpty(referencedPath))
			{
				throw new ArgumentException("Both paths must be non-empty.");
			}

			if (!Path.IsPathRooted(referencedPath) || !Path.IsPathRooted(mainPath))
			{
				throw new ArgumentException("Both paths must be absolute.");
			}

			return Path.GetRelativePath(mainPath, referencedPath);
		}







		private static readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new();

		public static async Task<DocumentPayload> GetDocumentPayload(this string docPath)
		{
			byte[] content;
			string fileName;
			string mimeType = "";
			if (docPath.PathIsUrl())
			{
				var urlFile = new Uri(docPath);

				SemaphoreSlim semaphore = _semaphores.GetOrAdd(urlFile.Host, _ => new SemaphoreSlim(1, 1));
				await semaphore.WaitAsync();
				try
				{
					// Download the file from the specified URL
					using var client = new HttpClient();

					var response = await client.GetAsync(urlFile);
					response.EnsureSuccessStatusCode();
					fileName = response.Content.Headers.ContentDisposition?.FileName ??
					           System.IO.Path.GetFileName(urlFile.LocalPath);
					mimeType = response.Content.Headers.ContentType?.MediaType;
					content = await response.Content.ReadAsByteArrayAsync();

					Logger.Log($"Downloaded Document {docPath}");
				}
				finally
				{
					await Task.Delay(100);
					semaphore.Release();
				}
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
				Logger.Log($"File Loaded: {fullPath}");
			}

			return new DocumentPayload() { FileName = fileName, Content = content, MimeType = mimeType };
		}



		private static object lockObj = new object();


		private static TaskCompletionSource<ConsoleKeyInfo> keyPressTcs;

		public static Task<ConsoleKeyInfo> ConsoleKeyPressAsync()
		{
			if (keyPressTcs == null || keyPressTcs.Task.IsCompleted)
			{
				lock (lockObj)
				{
					if (keyPressTcs == null || keyPressTcs.Task.IsCompleted)
					{
						keyPressTcs = new TaskCompletionSource<ConsoleKeyInfo>();

						Task.Run(() =>
						{
							var keyInfo = Console.ReadKey(intercept: true);
							keyPressTcs.SetResult(keyInfo);
						});
					}
				}
			}
			return keyPressTcs.Task;
		}





	


	}
}