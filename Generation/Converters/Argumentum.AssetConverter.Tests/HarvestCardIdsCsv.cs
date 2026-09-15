using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Reads a single CSV column — optionally filtered by an allowed white-list on another column —
	/// using production-compatible header matching. This is the reading engine behind the
	/// <see cref="CardSetExpectedCardCountContractTests"/> organ (#1187): it pins card counts at the
	/// <b>CSV × predicate × config</b> level instead of re-measuring the generated PDFs (the prior
	/// manual measure via <c>ConvertFrom-Json</c> on <c>Target/&lt;lang&gt;/Harvest/*_harvest_*.json</c>
	/// is slow — &gt;30 MB per Scenarii — and fragile: a hashtable <c>Count</c> is empty).
	/// </summary>
	/// <remarks>
	/// <para><b>Encoding resilience</b> (mirrors the persistence layer's tolerance): UTF-8 BOM,
	/// UTF-8 without BOM, UTF-16 LE/BE BOM, and a Latin-1 (ISO-8859-1) fallback for non-UTF-8 files.
	/// The source CSVs are UTF-8 (Rules/Fallacies with BOM, Scenarii without), but the fallback keeps
	/// the organ green on a Latin-1 source that would otherwise produce replacement characters.</para>
	/// <para><b>Header matching</b> mirrors <see cref="CsvBase{T,TMap}.LoadFromContent"/>: diacritics,
	/// underscore, hyphen and space are stripped, then lowercased — so <c>catégorie</c>,
	/// <c>sous-catégorie</c>, <c>print_and_play</c> all match regardless of accent form.</para>
	/// <para><b>No silent failure</b>: a missing target column, a missing filter column, or an
	/// empty/header-only file throws <see cref="InvalidOperationException"/> naming the column, the
	/// source path, the number of data rows actually read, and the <i>detected encoding</i> (so a
	/// silent Latin-1 fallback that swallows a non-UTF-8 source is visible from the failure alone —
	/// the reviewer concern raised against PR #1212 T2).</para>
	/// </remarks>
	public sealed class HarvestCardIdsCsv
	{
		private readonly string _path;

		/// <summary>Creates an instance bound to a single CSV file path.</summary>
		public HarvestCardIdsCsv(string path)
		{
			_path = path ?? throw new ArgumentNullException(nameof(path));
		}

		/// <summary>
		/// Reads the target column from the bound file as an ordered list (duplicates preserved).
		/// Wraps <see cref="File.ReadAllBytes"/> + encoding detection, then delegates to
		/// <see cref="LoadCsvColumn(string, string, string, IReadOnlyList{string}, string)"/>.
		/// </summary>
		public IReadOnlyList<string> LoadColumn(string columnName, string? filterField = null, IReadOnlyList<string>? filterValues = null)
			=> LoadFromFile(_path, columnName, filterField, filterValues);

		/// <summary>Reads the target column from the bound file, deduplicated (ordinal).</summary>
		public HashSet<string> LoadColumnSet(string columnName, string? filterField = null, IReadOnlyList<string>? filterValues = null)
			=> new HashSet<string>(LoadFromFile(_path, columnName, filterField, filterValues), StringComparer.Ordinal);

		/// <summary>
		/// Reads the bound file's header record (raw column names, in file order). Used by the
		/// #1187 PDF-level card-count derivation to count data rows through the same
		/// encoding-tolerant reader as <see cref="LoadColumn"/> without having to name a column
		/// (any column's row count is the row count; the first header column is as good as any).
		/// </summary>
		public IReadOnlyList<string> ReadHeader()
		{
			if (!File.Exists(_path))
				throw new InvalidOperationException($"HarvestCardIdsCsv: CSV not found at '{_path}'.");
			var content = DecodeCsvBytes(File.ReadAllBytes(_path), out _);
			if (string.IsNullOrWhiteSpace(content))
				throw new InvalidOperationException($"HarvestCardIdsCsv: source '{_path}' is empty (0 bytes / whitespace only).");
			using var reader = new StringReader(content);
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				PrepareHeaderForMatch = args => NormalizeHeader(args.Header),
				MissingFieldFound = null,
				BadDataFound = null,
				HeaderValidated = null,
			};
			using var csv = new CsvReader(reader, config);
			if (!csv.Read() || !csv.ReadHeader() || csv.HeaderRecord is null)
				throw new InvalidOperationException($"HarvestCardIdsCsv: source '{_path}' has no readable header row.");
			return csv.HeaderRecord.ToList();
		}

		// ─────────────────────────────────────────────────────────────────────────
		// Static pure API (no I/O) — directly testable on in-memory content.
		// ─────────────────────────────────────────────────────────────────────────

		/// <summary>
		/// Reads a CSV column from in-memory content. <paramref name="columnName"/> is the column
		/// whose values are returned; <paramref name="filterField"/>/<paramref name="filterValues"/>
		/// (both optional, equivalent to <c>CsvFilterField</c>/<c>CsvFilterValues</c> on a card set)
		/// keep only rows whose value in <paramref name="filterField"/> is one of
		/// <paramref name="filterValues"/>. Returns the raw values, order and duplicates preserved.
		/// <paramref name="sourcePath"/> is used only to make exception messages useful.
		/// </summary>
		public static IReadOnlyList<string> LoadCsvColumn(string csvContent, string columnName,
			string? filterField = null, IReadOnlyList<string>? filterValues = null, string? sourcePath = null)
			=> Extract(csvContent, columnName, filterField, filterValues, sourcePath);

		/// <summary>Same as <see cref="LoadCsvColumn"/> but returns a deduplicated ordinal set.</summary>
		public static HashSet<string> LoadCsvColumnSet(string csvContent, string columnName,
			string? filterField = null, IReadOnlyList<string>? filterValues = null, string? sourcePath = null)
			=> new HashSet<string>(Extract(csvContent, columnName, filterField, filterValues, sourcePath), StringComparer.Ordinal);

		// ─────────────────────────────────────────────────────────────────────────
		// File reading (encoding-robust)
		// ─────────────────────────────────────────────────────────────────────────

		private static IReadOnlyList<string> LoadFromFile(string path, string columnName, string? filterField, IReadOnlyList<string>? filterValues)
		{
			if (!File.Exists(path))
				throw new InvalidOperationException($"HarvestCardIdsCsv: CSV not found at '{path}'.");
			var content = DecodeCsvBytes(File.ReadAllBytes(path), out var detectedEncoding);
			return Extract(content, columnName, filterField, filterValues, path, detectedEncoding);
		}

		/// <summary>
		/// Decodes the raw CSV bytes and reports the encoding that was used (via <paramref name="detectedEncoding"/>).
		/// Callers can log <c>detectedEncoding</c> to surface silent encoding choices — particularly the
		/// Latin-1 fallback, which by design swallows a non-UTF-8 source without raising so that an accented
		/// character never degrades to a replacement glyph (production pattern, mirrored from
		/// <see cref="CsvBase{T,TMap}.LoadFromContent"/>). The detection order is:
		/// <list type="number">
		///   <item>UTF-8 BOM (0xEF 0xBB 0xBF)</item>
		///   <item>UTF-16 LE BOM (0xFF 0xFE)</item>
		///   <item>UTF-16 BE BOM (0xFE 0xFF)</item>
		///   <item>strict UTF-8 (throws on invalid bytes)</item>
		///   <item>Latin-1 fallback (ISO-8859-1, never throws)</item>
		/// </list>
		/// </summary>
		private static string DecodeCsvBytes(byte[] bytes, out string detectedEncoding)
		{
			if (bytes == null || bytes.Length == 0)
			{
				detectedEncoding = "<empty>";
				return string.Empty;
			}

			// Byte-Order-Mark detection first (UTF-8, UTF-16 LE, UTF-16 BE).
			if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
			{
				detectedEncoding = "UTF-8 BOM";
				return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
			}
			if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
			{
				detectedEncoding = "UTF-16 LE BOM";
				return Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2);
			}
			if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
			{
				detectedEncoding = "UTF-16 BE BOM";
				return Encoding.BigEndianUnicode.GetString(bytes, 2, bytes.Length - 2);
			}

			// No BOM: accept the file as strict UTF-8; if that fails, fall back to Latin-1 so an
			// accented source never silently degrades to replacement characters.
			try
			{
				detectedEncoding = "UTF-8 (no BOM, strict)";
				return new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true).GetString(bytes);
			}
			catch (DecoderFallbackException)
			{
				detectedEncoding = "Latin-1 (ISO-8859-1 fallback)";
				return Encoding.Latin1.GetString(bytes);
			}
		}

		private static IReadOnlyList<string> Extract(string csvContent, string columnName,
			string? filterField, IReadOnlyList<string>? filterValues, string? sourcePath, string? detectedEncoding = null)
		{
			// Surface the encoding choice via the exception messages so a misread (Latin-1 fallback
			// silently kicking in) is detectable from the failure alone, without needing the trace log.
			var encodingTag = string.IsNullOrEmpty(detectedEncoding) ? "" : $" [decoded as {detectedEncoding}]";
			if (columnName is null) throw new ArgumentNullException(nameof(columnName));

			if (string.IsNullOrWhiteSpace(csvContent))
				throw new InvalidOperationException(
					$"HarvestCardIdsCsv: source '{sourcePath ?? "<content>"}' is empty (0 bytes / whitespace only).{encodingTag}");

			var target = NormalizeHeader(columnName);
			var filter = filterField is null ? null : NormalizeHeader(filterField);

			using var reader = new StringReader(csvContent);
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				PrepareHeaderForMatch = args => NormalizeHeader(args.Header),
				MissingFieldFound = null,   // tolerate short rows (production pattern, DataSetInfo.GetDataTableFromCsv)
				BadDataFound = null,        // tolerate malformed fields (production pattern)
				HeaderValidated = null,     // we validate headers ourselves below
			};

			using var csv = new CsvReader(reader, config);
			var rows = csv.GetRecords<dynamic>().ToList();
			var header = csv.Context?.Reader?.HeaderRecord ?? Array.Empty<string>();
			var normalizedHeader = new HashSet<string>(header.Select(NormalizeHeader), StringComparer.Ordinal);

			if (rows.Count == 0)
				throw new InvalidOperationException(
					$"HarvestCardIdsCsv: source '{sourcePath ?? "<content>"}' has 0 data rows (empty or header-only).{encodingTag} " +
					$"Headers read: {FormatHeader(header)}.");

			if (!normalizedHeader.Contains(target))
				throw new InvalidOperationException(
					$"HarvestCardIdsCsv: column '{columnName}' not found in '{sourcePath ?? "<content>"}'.{encodingTag} " +
					$"{rows.Count} data row(s) read; headers present: {FormatHeader(header)}.");

			if (filter != null && !normalizedHeader.Contains(filter))
				throw new InvalidOperationException(
					$"HarvestCardIdsCsv: filter column '{filterField}' not found in '{sourcePath ?? "<content>"}'.{encodingTag} " +
					$"{rows.Count} data row(s) read; headers present: {FormatHeader(header)}.");

			var allowedFilter = filterValues is null
				? null
				: new HashSet<string>(filterValues.Select(v => v ?? string.Empty), StringComparer.Ordinal);

			var result = new List<string>();
			foreach (var row in rows)
			{
				var dict = (IDictionary<string, object>)row;
				if (dict is null) continue;

				if (allowedFilter != null && filter != null)
				{
					var filterValue = dict.TryGetValue(filter, out var f) ? f?.ToString() ?? string.Empty : string.Empty;
					if (!allowedFilter.Contains(filterValue)) continue;
				}

				var value = dict.TryGetValue(target, out var v) ? v?.ToString() : null;
				result.Add(value ?? string.Empty);
			}

			return result;
		}

		private static string FormatHeader(string[] header) => header.Length == 0 ? "(none)" : string.Join(", ", header);

		/// <summary>
		/// Mirrors <see cref="CsvBase{T,TMap}.LoadFromContent"/>'s header normalization: strip
		/// non-spacing diacritics, then lowercase and remove <c>_</c>, <c>-</c> and spaces. This makes
		/// <c>catégorie</c>, <c>sous-catégorie</c> and <c>print_and_play</c> lookups accent/separator
		/// agnostic.
		/// </summary>
		private static string NormalizeHeader(string header)
		{
			if (string.IsNullOrEmpty(header)) return string.Empty;
			var deaccented = header.Normalize(NormalizationForm.FormD);
			var sb = new StringBuilder(deaccented.Length);
			foreach (var c in deaccented)
			{
				if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
					sb.Append(c);
			}
			return sb.ToString()
				.Normalize(NormalizationForm.FormC)
				.ToLowerInvariant()
				.Replace("_", "")
				.Replace("-", "")
				.Replace(" ", "");
		}
	}
}
