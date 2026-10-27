using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Argumentum.AssetConverter.Entities;
using CsvHelper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Entities
{
	/// <summary>
	/// Guard on the REAL taxonomy CSV for the EN fallacy hierarchy (#1986 follow-up, ai-01
	/// c.6400149083; pool #458 c.6446645563 item 6).
	///
	/// <see cref="Fallacy.FamilyEn"/>, <see cref="Fallacy.SubfamilyEn"/> and
	/// <see cref="Fallacy.SubsubfamilyEn"/> are mapped <c>.Optional()</c>: if the committed CSV
	/// renames one of their headers, CsvHelper raises nothing and the EN property loads as null
	/// on every node — the English mind map then loses its hierarchy without a word.
	/// <c>MindMapEnHierarchyEvaluationTests</c> pins the map on a synthetic row; nothing read the
	/// committed asset.
	///
	/// The header names come from the production <see cref="FallacyClassMap"/>, not restated
	/// here: the contract is "the real CSV carries what the map expects". On that asset, through
	/// the production loader (<see cref="CsvBase{T,TMap}.LoadFromContent"/>, the call
	/// <c>Load</c> makes), each EN member must:
	///   - have its header exactly once in the header row — ordinal, so stricter than the
	///     loader's case/separator/diacritic-insensitive matching (101 of the 102 map names
	///     match their header byte for byte; the exception is <c>pk</c>/<c>PK</c>);
	///   - be non-empty on at least one node;
	///   - differ from its FR counterpart on at least one node — an EN column holding the FR
	///     hierarchy is the #1961 symptom by another road.
	///
	/// The witnesses prove each check fires on an in-memory copy of the same asset (the
	/// committed CSV is never written): renamed header, case-only rename, FR copied into the EN
	/// column, and an identity rewrite as the control that the re-serialization itself changes
	/// nothing.
	/// </summary>
	public class FallacyEnHierarchyRealCsvGuardTests
	{
		private const string HeaderCheck = "header";
		private const string LoadCheck = "load";
		private const string NonEmptyCheck = "non-empty";
		private const string DiffersFromFrCheck = "differs-from-fr";

		private static readonly Lazy<string> RealCsv = new(() => File.ReadAllText(Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")));

		public static IEnumerable<object[]> Members => new List<object[]>
		{
			// EN member, its FR counterpart
			new object[] { nameof(Fallacy.FamilyEn), nameof(Fallacy.Famille) },
			new object[] { nameof(Fallacy.SubfamilyEn), nameof(Fallacy.SousFamille) },
			new object[] { nameof(Fallacy.SubsubfamilyEn), nameof(Fallacy.Soussousfamille) },
		};

		/// <summary>The CSV header the production map binds this member to.</summary>
		private static string MappedHeader(string memberName)
		{
			var maps = new FallacyClassMap().MemberMaps
				.Where(m => m.Data.Member?.Name == memberName)
				.ToList();
			maps.Should().HaveCount(1, "'{0}' must be mapped exactly once in {1}", memberName, nameof(FallacyClassMap));
			maps[0].Data.Names.Should().HaveCount(1, "'{0}' must be bound to a single header", memberName);
			return maps[0].Data.Names[0];
		}

		private static Func<Fallacy, string?> Getter(string memberName)
		{
			var property = typeof(Fallacy).GetProperty(memberName);
			property.Should().NotBeNull("'{0}' must exist on {1}", memberName, nameof(Fallacy));
			return fallacy => (string?)property!.GetValue(fallacy);
		}

		private static string[] HeaderRow(string content)
		{
			using var parser = new CsvParser(new StringReader(content), CultureInfo.InvariantCulture);
			return parser.Read() ? parser.Record ?? Array.Empty<string>() : Array.Empty<string>();
		}

		/// <summary>
		/// Every check of this guard, run on <paramref name="content"/>; empty when all hold.
		/// Returned rather than asserted so that the witnesses can show which check fires.
		/// </summary>
		private static List<(string Check, string Detail)> Problems(string content, string enMember, string frMember)
		{
			var problems = new List<(string Check, string Detail)>();
			var header = MappedHeader(enMember);
			var en = Getter(enMember);
			var fr = Getter(frMember);

			var occurrences = HeaderRow(content).Count(h => h == header);
			if (occurrences != 1)
			{
				problems.Add((HeaderCheck, $"header '{header}', bound to {enMember} by {nameof(FallacyClassMap)}, "
					+ $"appears {occurrences} time(s) in the header row; expected exactly 1"));
			}

			IList<Fallacy> loaded;
			try
			{
				loaded = Fallacy.LoadFromContent(content);
			}
			catch (Exception e)
			{
				problems.Add((LoadCheck, $"the production loader threw {e.GetType().Name}: {e.Message}"));
				return problems;
			}

			if (!loaded.Any(f => !string.IsNullOrWhiteSpace(en(f))))
			{
				problems.Add((NonEmptyCheck, $"{enMember} is empty on all {loaded.Count} loaded nodes, "
					+ "which is what an Optional() map yields, without any error, once its header is gone"));
			}

			if (!loaded.Any(f => !string.IsNullOrWhiteSpace(en(f)) && en(f) != fr(f)))
			{
				problems.Add((DiffersFromFrCheck, $"{enMember} never differs from {frMember} on a non-empty node: "
					+ "the EN column carries the FR hierarchy"));
			}

			return problems;
		}

		/// <summary>
		/// Re-serializes <paramref name="content"/> record by record; <paramref name="edit"/> receives
		/// the record index (0 = header row) and the record, which it may change in place.
		/// </summary>
		private static string Rewrite(string content, Action<int, string[]> edit)
		{
			using var parser = new CsvParser(new StringReader(content), CultureInfo.InvariantCulture);
			using var output = new StringWriter();
			using (var writer = new CsvWriter(output, CultureInfo.InvariantCulture))
			{
				for (var index = 0; parser.Read(); index++)
				{
					var record = (string[])parser.Record!.Clone();
					edit(index, record);
					foreach (var field in record)
					{
						writer.WriteField(field);
					}
					writer.NextRecord();
				}
			}
			return output.ToString();
		}

		private static int ColumnOf(string[] header, string name)
		{
			var index = Array.IndexOf(header, name);
			index.Should().BeGreaterThanOrEqualTo(0, "the witness mutates the column '{0}', which must exist", name);
			return index;
		}

		[Theory]
		[MemberData(nameof(Members))]
		public void Real_Csv_Carries_The_En_Hierarchy_Column_The_Map_Expects(string enMember, string frMember)
		{
			Problems(RealCsv.Value, enMember, frMember).Should().BeEmpty(
				"the committed taxonomy CSV must feed {0} through the production loader (#1986 follow-up)", enMember);
		}

		[Theory]
		[MemberData(nameof(Members))]
		public void Witness_Identity_Rewrite_Keeps_The_Guard_Green(string enMember, string frMember)
		{
			var rewritten = Rewrite(RealCsv.Value, (_, _) => { });

			Problems(rewritten, enMember, frMember).Should().BeEmpty(
				"the re-serialization used by the witnesses must not change what the guard sees");
		}

		[Theory]
		[MemberData(nameof(Members))]
		public void Witness_Renamed_Header_Is_Caught(string enMember, string frMember)
		{
			var header = MappedHeader(enMember);
			var column = ColumnOf(HeaderRow(RealCsv.Value), header);
			var renamed = Rewrite(RealCsv.Value, (index, record) =>
			{
				if (index == 0)
				{
					record[column] = "Renamed" + header;
				}
			});

			var checks = Problems(renamed, enMember, frMember).Select(p => p.Check).ToList();
			checks.Should().Contain(c => c == HeaderCheck, "the mapped header is gone from the header row");
			checks.Should().Contain(c => c == NonEmptyCheck || c == LoadCheck,
				"a renamed header either nulls {0} silently (Optional) or fails the load (were Optional removed)", enMember);
		}

		/// <summary>
		/// What the ordinal header check adds over the loader: a case-only rename still loads
		/// (the loader lowercases before matching), so only the header check sees it.
		/// </summary>
		[Theory]
		[MemberData(nameof(Members))]
		public void Witness_Case_Only_Rename_Is_Caught_By_The_Header_Check_Alone(string enMember, string frMember)
		{
			var header = MappedHeader(enMember);
			var column = ColumnOf(HeaderRow(RealCsv.Value), header);
			var lowered = Rewrite(RealCsv.Value, (index, record) =>
			{
				if (index == 0)
				{
					record[column] = header.ToLowerInvariant();
				}
			});

			var checks = Problems(lowered, enMember, frMember).Select(p => p.Check).ToList();
			checks.Should().HaveCount(1, "the loader still matches '{0}' case-insensitively, so {1} still loads", header, enMember);
			checks[0].Should().Be(HeaderCheck, "only the ordinal header check sees a map name and a header that drifted apart");
		}

		[Theory]
		[MemberData(nameof(Members))]
		public void Witness_Fr_Copied_Into_The_En_Column_Is_Caught_By_The_Fr_Check_Alone(string enMember, string frMember)
		{
			var realHeader = HeaderRow(RealCsv.Value);
			var enColumn = ColumnOf(realHeader, MappedHeader(enMember));
			var frColumn = ColumnOf(realHeader, MappedHeader(frMember));
			var copied = Rewrite(RealCsv.Value, (index, record) =>
			{
				if (index > 0)
				{
					record[enColumn] = record[frColumn];
				}
			});

			var checks = Problems(copied, enMember, frMember).Select(p => p.Check).ToList();
			checks.Should().HaveCount(1, "the header and the non-empty checks still hold on a copied column");
			checks[0].Should().Be(DiffersFromFrCheck, "only the FR comparison sees an EN column filled with the FR hierarchy");
		}
	}
}
