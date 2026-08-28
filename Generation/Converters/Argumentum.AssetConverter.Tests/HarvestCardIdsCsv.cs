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
	/// source path, and the number of data rows actually read.</para>
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
			var content = DecodeCsvBytes(File.ReadAllBytes(path));
			return Extract(content, columnName, filterField, filterValues, path);
		}

		private static string DecodeCsvBytes(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
				return string.Empty;

			// Byte-Order-Mark detection first (UTF-8, UTF-16 LE, UTF-16 BE).
			if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
				return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
			if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
				return Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2);
			if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
				return Encoding.BigEndianUnicode.GetString(bytes, 2, bytes.Length - 2);

			// No BOM: accept the file as strict UTF-8; if that fails, fall back to Latin-1 so an
			// accented source never silently degrades to replacement characters.
			try
			{
				return new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true).GetString(bytes);
			}
			catch (DecoderFallbackException)
			{
				return Encoding.Latin1.GetString(bytes);
			}
		}

		private static IReadOnlyList<string> Extract(string csvContent, string columnName,
			string? filterField, IReadOnlyList<string>? filterValues, string? sourcePath)
		{
			if (columnName is null) throw new ArgumentNullException(nameof(columnName));

			if (string.IsNullOrWhiteSpace(csvContent))
				throw new InvalidOperationException(
					$"HarvestCardIdsCsv: source '{sourcePath ?? "<content>"}' is empty (0 bytes / whitespace only).");

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
					$"HarvestCardIdsCsv: source '{sourcePath ?? "<content>"}' has 0 data rows (empty or header-only). " +
					$"Headers read: {FormatHeader(header)}.");

			if (!normalizedHeader.Contains(target))
				throw new InvalidOperationException(
					$"HarvestCardIdsCsv: column '{columnName}' not found in '{sourcePath ?? "<content>"}'. " +
					$"{rows.Count} data row(s) read; headers present: {FormatHeader(header)}.");

			if (filter != null && !normalizedHeader.Contains(filter))
				throw new InvalidOperationException(
					$"HarvestCardIdsCsv: filter column '{filterField}' not found in '{sourcePath ?? "<content>"}'. " +
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
