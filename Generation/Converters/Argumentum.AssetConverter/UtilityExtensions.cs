using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Net;
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

		// #1296: explicit Timeout — the .NET default (100 s) is what turned a minutes-long WAN blip into
		// 56 dead CardSets on 2026-09-06; combined with the bounded retry below, a transient outage is
		// retried through instead of costing the set.
		// NB: these must be declared BEFORE _sharedHttpClient — static field initializers run in
		// declaration order, and the HttpClient reads HttpDownloadTimeout at construction.
		internal static readonly TimeSpan HttpDownloadTimeout = TimeSpan.FromSeconds(30);

		// Backoff between download attempts: 4 attempts x 30 s timeout + 35 s backoff = 155 s budget,
		// which absorbs a 2-minute network cut (#1296 DoD). Readonly — tests pass shorter delays
		// explicitly instead of mutating this (process-wide static mutation from tests, #1192).
		internal static readonly TimeSpan[] HttpRetryDelays = { TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20) };

		// Upper bound on waiting for the per-host download slot; without it, one stuck fetch
		// serialized the whole queue behind it and froze the run (#1296).
		internal static readonly TimeSpan HttpSemaphoreAcquireTimeout = TimeSpan.FromMinutes(10);

		// Shared HttpClient (was `new HttpClient()` per call — #29 H6). Thread-safe for concurrent calls;
		// avoids socket exhaustion / GC pressure on long runs with repeated downloads. Output-neutral.
		private static readonly HttpClient _sharedHttpClient = CreateSharedHttpClient();

		private static HttpClient CreateSharedHttpClient()
		{
			var client = new HttpClient();
			client.Timeout = HttpDownloadTimeout;
			return client;
		}


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
					// Include parameter types in cache key to avoid type mismatches
					// (e.g., Fallacy vs Virtue both using "{item.Text}" expression)
					var typeKey = string.Join(",", context.Values.Select(v => v.GetType().FullName));
					var key = $"{value}/{matchToken}/{typeKey}";
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
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}

			// Trim to handle potential whitespace issues
			var trimmedPath = path.Trim();

			// Use Uri.TryCreate to safely parse the path
			if (Uri.TryCreate(trimmedPath, UriKind.Absolute, out Uri uri))
			{
				// Check for explicit http or https schemes
				return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
			}

			return false;
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

		private static bool IsTransientStatus(HttpStatusCode statusCode)
		{
			var code = (int)statusCode;
			return code == 408 || code == 429 || code >= 500;
		}

		public static async Task<DocumentPayload> GetDocumentPayload(this string docPath, TimeSpan[] retryDelays = null)
		{
			byte[] content;
			string fileName;
			string mimeType = "";
			if (docPath.PathIsUrl())
			{
				var urlFile = new Uri(docPath);

				SemaphoreSlim semaphore = _semaphores.GetOrAdd(urlFile.Host, _ => new SemaphoreSlim(1, 1));
				if (!await semaphore.WaitAsync(HttpSemaphoreAcquireTimeout))
				{
					Logger.LogWarning($"Failed to download document {docPath}: timed out after {HttpSemaphoreAcquireTimeout.TotalMinutes:F0} min " +
					                  $"waiting for the per-host download slot of '{urlFile.Host}' (another download of the same host is stuck)");
					return null;
				}
				try
				{
					// Download the file from the specified URL, retrying transient failures (#1296)
					var client = _sharedHttpClient;
					retryDelays ??= HttpRetryDelays;

					for (var attempt = 1; ; attempt++)
					{
						var retryIndex = attempt - 1;
						try
						{
							using var response = await client.GetAsync(urlFile);
							if (response.IsSuccessStatusCode)
							{
								fileName = response.Content.Headers.ContentDisposition?.FileName ??
								           System.IO.Path.GetFileName(urlFile.LocalPath);
								mimeType = response.Content.Headers.ContentType?.MediaType;
								content = await response.Content.ReadAsByteArrayAsync();

								Logger.Log(attempt > 1
									? $"Downloaded Document {docPath} (attempt {attempt})"
									: $"Downloaded Document {docPath}");
								break;
							}

							if (retryIndex < retryDelays.Length && IsTransientStatus(response.StatusCode))
							{
								Logger.LogWarning($"HTTP {(int)response.StatusCode} downloading document {docPath} — attempt {attempt}/{retryDelays.Length + 1}, " +
								                  $"retrying in {retryDelays[retryIndex].TotalSeconds:F0} s");
								await Task.Delay(retryDelays[retryIndex]);
								continue;
							}

							Logger.LogWarning($"Failed to download document {docPath}. Status code: {response.StatusCode}");
							return null;
						}
						catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
						{
							// TaskCanceledException here is the HttpClient timeout firing
							// (no CancellationToken is ever passed to GetAsync in this code path).
							if (retryIndex >= retryDelays.Length)
							{
								Logger.LogWarning($"Network failure downloading document {docPath} after {attempt} attempts " +
								                  $"({ex.GetType().Name}: {ex.Message})");
								return null;
							}

							Logger.LogWarning($"Network failure downloading document {docPath} ({ex.GetType().Name}: {ex.Message}) — " +
							                  $"attempt {attempt}/{retryDelays.Length + 1}, retrying in {retryDelays[retryIndex].TotalSeconds:F0} s");
							await Task.Delay(retryDelays[retryIndex]);
						}
					}
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

		public static async Task<string> GetDocumentContent(this string docPath)
		{
			var payload = await docPath.GetDocumentPayload();
			if (payload == null)
			{
				return null;
			}
			return Encoding.UTF8.GetString(payload.Content).TrimStart('﻿');
		}


		private static object lockObj = new object();


		internal static TaskCompletionSource<ConsoleKeyInfo> KeyPressTcs;

		public static Task<ConsoleKeyInfo> ConsoleKeyPressAsync()
		{
			if (KeyPressTcs == null || KeyPressTcs.Task.IsCompleted)
			{
				lock (lockObj)
				{
					if (KeyPressTcs == null || KeyPressTcs.Task.IsCompleted)
					{
						KeyPressTcs = new TaskCompletionSource<ConsoleKeyInfo>();

						Task.Run(() =>
						{
							var keyInfo = Console.ReadKey(intercept: true);
							KeyPressTcs.SetResult(keyInfo);
						});
					}
				}
			}
			return KeyPressTcs.Task;
		}





	


	}
}