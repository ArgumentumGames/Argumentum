using System;
using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
	/// <summary>
	/// Unit tests for <see cref="DataSetInfo.UpdateTableFromRecords"/> — verifies that
	/// LLM-produced records merge reliably into the target DataTable regardless of
	/// JSON type coercion (number vs string primary keys) and that silent drops are
	/// surfaced via the logger rather than swallowed.
	/// Tracks Bug 2 of issue #409.
	/// </summary>
	public class UpdateTableFromRecordsTests
	{
		private const string PrimaryKey = "pk";

		private static DataTable BuildTable(params object[][] rows)
		{
			var table = new DataTable();
			table.Columns.Add(PrimaryKey, typeof(string));
			table.Columns.Add("text_en", typeof(string));
			table.Columns.Add("desc_en", typeof(string));
			table.PrimaryKey = new[] { table.Columns[PrimaryKey]! };
			foreach (var row in rows)
			{
				var dr = table.NewRow();
				for (int i = 0; i < row.Length; i++) dr[i] = row[i];
				table.Rows.Add(dr);
			}
			return table;
		}

		private static Dictionary<string, object> Record(string pk, string? textEn = null, string? descEn = null)
		{
			var dict = new Dictionary<string, object> { [PrimaryKey] = pk };
			if (textEn != null) dict["text_en"] = textEn;
			if (descEn != null) dict["desc_en"] = descEn;
			return dict;
		}

		[Fact]
		public void UpdateTableFromRecords_WhenPkMatchesAsString_UpdatesRow()
		{
			var table = BuildTable(new object[] { "1", "old title", "old desc" });
			var tables = new Dictionary<string, DataTable> { [""] = table };
			var records = new List<Dictionary<string, object>>
			{
				Record("1", textEn: "new title", descEn: "new desc")
			};

			DataSetInfo.UpdateTableFromRecords(
				primaryKeyColumn: PrimaryKey,
				fieldsToUpdate: new List<string> { "text_en", "desc_en" },
				addNewRows: false,
				records: records,
				writeOneTargetFileByField: false,
				resultTables: tables);

			table.Rows[0]["text_en"].Should().Be("new title");
			table.Rows[0]["desc_en"].Should().Be("new desc");
		}

		[Fact]
		public void UpdateTableFromRecords_WhenPkIsJsonNumber_CoercesToStringAndUpdates()
		{
			// Simulates the most common Bug 2 scenario: LLM returns PK as JSON number
			// (e.g. 1 instead of "1"), Json.NET deserializes as double, DataTable column
			// is string. Without coercion Rows.Find returns null silently.
			var table = BuildTable(new object[] { "1", "old", null! });
			var tables = new Dictionary<string, DataTable> { [""] = table };
			var records = new List<Dictionary<string, object>>
			{
				new() { [PrimaryKey] = 1.0, ["text_en"] = "new" }
			};

			DataSetInfo.UpdateTableFromRecords(
				primaryKeyColumn: PrimaryKey,
				fieldsToUpdate: new List<string> { "text_en" },
				addNewRows: false,
				records: records,
				writeOneTargetFileByField: false,
				resultTables: tables);

			table.Rows[0]["text_en"].Should().Be("new");
		}

		[Fact]
		public void UpdateTableFromRecords_WhenPkMissing_DropsAndLogs()
		{
			// addNewRows=false → record with unknown PK must be dropped, but a warning
			// should be emitted (was silent before Bug 2 fix).
			var table = BuildTable(new object[] { "1", "old", null! });
			var tables = new Dictionary<string, DataTable> { [""] = table };
			var records = new List<Dictionary<string, object>>
			{
				Record("999", textEn: "ghost"),
				Record("1", textEn: "real")
			};

			DataSetInfo.UpdateTableFromRecords(
				primaryKeyColumn: PrimaryKey,
				fieldsToUpdate: new List<string> { "text_en" },
				addNewRows: false,
				records: records,
				writeOneTargetFileByField: false,
				resultTables: tables);

			table.Rows.Count.Should().Be(1);
			table.Rows[0]["text_en"].Should().Be("real");
		}

		[Fact]
		public void UpdateTableFromRecords_WhenAddNewRowsTrue_CreatesRow()
		{
			var table = BuildTable(new object[] { "1", "old", null! });
			var tables = new Dictionary<string, DataTable> { [""] = table };
			var records = new List<Dictionary<string, object>>
			{
				Record("2", textEn: "brand new")
			};

			DataSetInfo.UpdateTableFromRecords(
				primaryKeyColumn: PrimaryKey,
				fieldsToUpdate: new List<string> { "text_en" },
				addNewRows: true,
				records: records,
				writeOneTargetFileByField: false,
				resultTables: tables);

			table.Rows.Find("2").Should().NotBeNull();
			table.Rows.Find("2")!["text_en"].Should().Be("brand new");
		}

		[Fact]
		public void UpdateTableFromRecords_WhenWriteOneTargetFileByField_WritesToPerFieldTables()
		{
			var table = BuildTable(new object[] { "1", "old title", "old desc" });
			var tables = new Dictionary<string, DataTable> { [""] = table };
			var records = new List<Dictionary<string, object>>
			{
				Record("1", textEn: "new title", descEn: "new desc")
			};

			DataSetInfo.UpdateTableFromRecords(
				primaryKeyColumn: PrimaryKey,
				fieldsToUpdate: new List<string> { "text_en", "desc_en" },
				addNewRows: false,
				records: records,
				writeOneTargetFileByField: true,
				resultTables: tables);

			tables.Should().ContainKey("text_en");
			tables.Should().ContainKey("desc_en");
			tables["text_en"].Rows.Find("1")!["text_en"].Should().Be("new title");
			tables["desc_en"].Rows.Find("1")!["desc_en"].Should().Be("new desc");
			// Original global table must remain untouched in per-field mode
			table.Rows[0]["text_en"].Should().Be("old title");
		}

		[Fact]
		public void UpdateTableFromRecords_WhenWriteOneTargetFileByFieldAndPkMissing_SkipsFieldSafely()
		{
			// Per-field path used to throw NullReferenceException when Rows.Find returned
			// null. It must now skip the field with a warning instead of crashing.
			var table = BuildTable(new object[] { "1", "old", null! });
			var tables = new Dictionary<string, DataTable> { [""] = table };
			var records = new List<Dictionary<string, object>>
			{
				new() { [PrimaryKey] = 42.0, ["text_en"] = "ghost update" } // PK doesn't exist
			};

			var act = () => DataSetInfo.UpdateTableFromRecords(
				primaryKeyColumn: PrimaryKey,
				fieldsToUpdate: new List<string> { "text_en" },
				addNewRows: false,
				records: records,
				writeOneTargetFileByField: true,
				resultTables: tables);

			act.Should().NotThrow();
			tables.Should().NotContainKey("text_en"); // no per-field table should be created for a missing PK
			table.Rows[0]["text_en"].Should().Be("old");
		}
	}
}
