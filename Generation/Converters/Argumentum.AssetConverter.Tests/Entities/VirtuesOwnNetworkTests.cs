using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Entities
{
	/// <summary>
	/// Guard for the virtues' OWN relational network (#988): <c>crossLink_Leverages</c>,
	/// <c>crossLink_IsRelatedTo</c> and <c>crossLink_Allows</c> must carry an intra-deck network
	/// of taxonomic PATHS, not stay empty, and every link must be resolvable and transverse.
	///
	/// Before #988, <c>crossLink_Opposes</c> (fallacy PKs) was the only populated relation: the
	/// virtues existed only as a reflection of the fallacies. The network measured here is the
	/// correction of that generator principle.
	///
	/// Two referent families deliberately coexist and must not mix: Opposes is INTER-deck
	/// (fallacy PKs — numeric, mostly NOT valid virtue paths), the three #988 verbs are INTRA-deck
	/// (virtue paths). The guards below pin each side of that split.
	///
	/// Why thresholds on the counts AND exact validity on every link: the DoD of #988 is a
	/// density comparable to the fallacies deck (Leverages 24.9%, IsRelatedTo 20.3%, Allows 4.4%
	/// there); density alone without per-link validity would accept a garbage network, and
	/// validity alone would accept a three-link token network.
	/// </summary>
	public class VirtuesOwnNetworkTests
	{
		private const string VirtuesCsvRelPath = "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv";

		private static readonly (string Column, int MinSources)[] OwnNetworkColumns =
		{
			("crossLink_Leverages", 40),
			("crossLink_IsRelatedTo", 35),
			("crossLink_Allows", 10),
		};

		private sealed record VirtueRow(string Path, string Leverages, string IsRelatedTo, string Allows, string Opposes);

		private static List<VirtueRow> ReadRows(string csvPath)
		{
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				HeaderValidated = null,
			};
			using var reader = new StreamReader(csvPath);
			using var csv = new CsvReader(reader, config);
			csv.Read();
			csv.ReadHeader();
			var headers = csv.HeaderRecord ?? Array.Empty<string>();

			foreach (var (column, _) in OwnNetworkColumns)
			{
				headers.Should().Contain(column,
					"the Virtues CSV must expose '{0}' (#988 network column)", column);
			}

			var rows = new List<VirtueRow>();
			while (csv.Read())
			{
				string Get(string col) => csv.GetField(col) ?? string.Empty;
				rows.Add(new VirtueRow(
					(Get("path") ?? string.Empty).Trim(),
					Get("crossLink_Leverages").Trim(),
					Get("crossLink_IsRelatedTo").Trim(),
					Get("crossLink_Allows").Trim(),
					Get("crossLink_Opposes").Trim()));
			}
			return rows;
		}

		/// <summary>Splits a crossLink cell into trimmed targets ("a;b" → ["a","b"]).</summary>
		internal static string[] SplitTargets(string cell) =>
			(cell ?? string.Empty).Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

		internal static bool IsAncestorOrDescendant(string a, string b) =>
			a != b && (a.StartsWith(b + ".", StringComparison.Ordinal) || b.StartsWith(a + ".", StringComparison.Ordinal));

		[Fact]
		public void Own_Network_Is_Populated_And_Every_Link_Is_Transverse_And_Resolvable()
		{
			var repoRoot = TestRepoRoot.Find();
			var csvPath = Path.Combine(repoRoot, VirtuesCsvRelPath);
			File.Exists(csvPath).Should().BeTrue("the Virtues CSV must exist at {0}", VirtuesCsvRelPath);

			var rows = ReadRows(csvPath);
			rows.Should().NotBeEmpty();
			var paths = rows.Select(r => r.Path).Where(p => p.Length > 0).ToHashSet();

			foreach (var (column, minSources) in OwnNetworkColumns)
			{
				var populated = rows.Where(r => OwnCell(r, column).Length > 0).ToList();
				populated.Should().HaveCountGreaterThanOrEqualTo(minSources,
					"{0} must carry the virtues' own network at a density comparable to the fallacies deck " +
					"(#988 DoD: Leverages ~25%, IsRelatedTo ~20%, Allows ~5-10% of rows)", column);

				var offenders = populated
					.SelectMany(r => SplitTargets(OwnCell(r, column))
						.Where(t => !paths.Contains(t) || t == r.Path || IsAncestorOrDescendant(t, r.Path))
						.Select(t => $"{r.Path} -> {t}"))
					.ToList();
				offenders.Should().BeEmpty(
					"every {0} link must target an EXISTING virtue path, never itself, never its own " +
					"taxonomic ancestor/descendant (a cross-link the tree already shows carries no " +
					"information). Offenders: {1}", column, string.Join(", ", offenders));
			}
		}

		[Fact]
		public void Opposes_Stays_Inter_Deck_And_Own_Verbs_Stay_Intra_Deck()
		{
			var repoRoot = TestRepoRoot.Find();
			var rows = ReadRows(Path.Combine(repoRoot, VirtuesCsvRelPath));
			var paths = rows.Select(r => r.Path).Where(p => p.Length > 0).ToHashSet();

			rows.Where(r => r.Opposes.Length > 0).Should().HaveCountGreaterThan(200,
				"Opposes keeps its near-total coverage of the deck");

			// The referent split documented in Virtue.cs: if Opposes targets ever became virtue
			// paths en masse, or the own-network verbs ever carried fallacy PKs, the two referent
			// families would have silently merged and every consumer would misresolve.
			var opposesResolvedAsVirtues = rows
				.Count(r => SplitTargets(r.Opposes).Any(t => paths.Contains(t)));
			opposesResolvedAsVirtues.Should().BeLessThan(20,
				"Opposes is INTER-deck (fallacy PKs): its targets must overwhelmingly not resolve as " +
				"virtue paths. {0} currently do.", opposesResolvedAsVirtues);
		}

		[Fact]
		public void Detector_Sees_A_Non_Transverse_Link()
		{
			// Inverse control (#1046): the validity predicate must be ABLE to fail, otherwise the
			// guard above is green forever regardless of the network it reads.
			IsAncestorOrDescendant("6.3", "6.3.2").Should().BeTrue("parent -> child is not transverse");
			IsAncestorOrDescendant("6.3.2.1", "6.3").Should().BeTrue("child -> parent is not transverse");
			IsAncestorOrDescendant("6.3", "6.3").Should().BeFalse("same node is handled by the self-link check");
			IsAncestorOrDescendant("6.3", "6.4").Should().BeFalse("siblings ARE transverse");
			SplitTargets(" 4.3.3.1 ; 7.1 ").Should().BeEquivalentTo("4.3.3.1", "7.1");
			SplitTargets("").Should().BeEmpty();
		}

		private static string OwnCell(VirtueRow row, string column) => column switch
		{
			"crossLink_Leverages" => row.Leverages,
			"crossLink_IsRelatedTo" => row.IsRelatedTo,
			"crossLink_Allows" => row.Allows,
			_ => throw new ArgumentOutOfRangeException(nameof(column)),
		};
	}
}
