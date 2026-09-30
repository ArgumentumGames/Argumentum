using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde du grain « concepts dupliqués » (dispatch #458 c.5915478409, clôture du point 2 de
	/// #1622 côté titres) : quand un concept figure DEUX FOIS dans la taxonomie — même
	/// <c>text_fr</c> ET même <c>text_en</c>, l'identité éditoriale du corpus — et qu'UNE des
	/// copies est une carte imprimée, la copie hors deck porte LE MÊME TITRE que la carte dans
	/// chaque langue : « la copie hors deck sur la carte imprimée ».
	///
	/// <para><b>Pourquoi cette règle.</b> Deux rangées jumelles produisent la même identité OWL
	/// (l'IRI dérive du titre anglais) : des titres divergents y font porter à UN concept deux
	/// <c>prefLabel</c> différents par langue — SKOS n'admet qu'un <c>prefLabel</c> par langue et
	/// par concept (#1622 point 2). Le grain a aligné 71 cellules sur 25 rangées hors deck
	/// (fa 23 · ru 13 · ar 13 · zh 10 · pt 7 · es 5), toutes en profondeur &gt; 3 : aucun bandeau
	/// ne référence leur titre, l'invariant ⑱w (bandeau = titre du rang) est neutre, et AUCUN
	/// PDF ne bouge (la copie imprimée est la source, jamais la cible).</para>
	///
	/// <para><b>Périmètre assumé (écart déclaré dans la PR).</b> Les paires jumelles dont AUCUNE
	/// copie n'est imprimée (52 groupes mesurés) restent hors périmètre : la règle est muette
	/// quand il n'y a pas de carte imprimée à faire autorité — arbitrage à part. Les paires
	/// homonymes (même titre anglais, titres français DIFFÉRENTS = concepts distincts) sont hors
	/// clé par construction : la clé est le couple (fr, en).</para>
	///
	/// <para><b>Anti-vacuité.</b> Le nombre de groupes imprimés est épinglé (23 au grain, mesuré
	/// sur <c>3bd84bc1</c>) : une lecture vide rendrait l'invariant vert sans rien prouver. Le
	/// compte ne peut pas descendre sans qu'un concept dupliqué disparaisse — recaler avec la
	/// date, jamais effacer la ligne.</para>
	/// </summary>
	public class DuplicateConceptTitleAlignmentGuardTests
	{
		private static readonly string[] Languages = { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

		private static List<Dictionary<string, string>> LoadRows()
		{
			var csvPath = Path.Combine(TestRepoRoot.Find(),
				"Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				BadDataFound = null,
				HeaderValidated = null,
			};
			using var reader = new StringReader(File.ReadAllText(csvPath));
			using var csv = new CsvReader(reader, config);
			var rows = new List<Dictionary<string, string>>();
			foreach (var record in csv.GetRecords<dynamic>())
			{
				var dict = (IDictionary<string, object>)record;
				rows.Add(dict.ToDictionary(kv => kv.Key, kv => kv.Value?.ToString() ?? string.Empty,
					StringComparer.Ordinal));
			}
			rows.Should().HaveCount(1408, "la taxonomie des sophismes compte 1408 rangées.");
			return rows;
		}

		private static string Cell(Dictionary<string, string> row, string column) =>
			(row.GetValueOrDefault(column) ?? string.Empty).Trim();

		/// <summary>
		/// Les paires jumelles AVEC carte imprimée : (identité fr+en) → membres. Partagé par
		/// l'invariant et le témoin nommé.
		/// </summary>
		private static List<List<Dictionary<string, string>>> DeckBackedDuplicateGroups(
			List<Dictionary<string, string>> rows)
		{
			return rows
				.Where(r => Cell(r, "text_fr").Length > 0 && Cell(r, "text_en").Length > 0)
				.GroupBy(r => (Fr: Cell(r, "text_fr"), En: Cell(r, "text_en")))
				.Where(g => g.Count() > 1 && g.Any(r => !string.IsNullOrWhiteSpace(r.GetValueOrDefault("carte"))))
				.Select(g => g.OrderBy(r => r.GetValueOrDefault("PK"), StringComparer.Ordinal).ToList())
				.ToList();
		}

		[Fact]
		public void OffDeck_Copies_Of_Printed_Concepts_Carry_The_Card_Titles()
		{
			var rows = LoadRows();
			var groups = DeckBackedDuplicateGroups(rows);

			groups.Should().HaveCount(23,
				"anti-vacuité : 23 concepts dupliqués à carte imprimée mesurés au grain (3bd84bc1, " +
				"30/09). Le compte ne peut pas descendre sans qu'un concept dupliqué disparaisse — " +
				"recaler avec la date, jamais effacer la ligne.");

			var failures = new List<string>();
			foreach (var group in groups)
			{
				var deck = group.First(r => !string.IsNullOrWhiteSpace(r.GetValueOrDefault("carte")));
				foreach (var off in group.Where(r => string.IsNullOrWhiteSpace(r.GetValueOrDefault("carte"))))
				{
					foreach (var lang in Languages)
					{
						var deckTitle = Cell(deck, "text_" + lang);
						var offTitle = Cell(off, "text_" + lang);
						if (deckTitle != offTitle)
							failures.Add(
								$"PK {off.GetValueOrDefault("PK")} [{lang}] «{offTitle}» != carte PK " +
								$"{deck.GetValueOrDefault("PK")} «{deckTitle}»");
					}
				}
			}

			failures.Should().BeEmpty(
				"l'invariant du grain : chaque copie hors deck d'un concept imprimé porte le titre de " +
				"LA CARTE dans toutes les langues (71 cellules alignées, fa 23 · ru 13 · ar 13 · zh 10 · " +
				$"pt 7 · es 5). Écarts restants : {string.Join(" ; ", failures.Take(8))}");
		}

		[Fact]
		public void Pk1123_Es_Aligned_On_Its_Printed_Twin()
		{
			// Témoin nommé : « Généralisation abusive » / « Faulty generalisation » — la carte est
			// le PK 595 (rang 3.1), la copie hors deck est le PK 1123 (6.3.1.2.2.1.3). Avant le
			// grain, la copie portait « Generalización excesiva » et « 错误的普遍化 ».
			var rows = LoadRows();
			var byPk = rows.ToDictionary(r => Cell(r, "PK"), StringComparer.Ordinal);
			var deck = byPk["595"];
			var off = byPk["1123"];
			(!string.IsNullOrWhiteSpace(deck.GetValueOrDefault("carte"))).Should().BeTrue(
				"le témoin suppose que le PK 595 est la carte imprimée du concept.");
			string.IsNullOrWhiteSpace(off.GetValueOrDefault("carte")).Should().BeTrue(
				"le témoin suppose que le PK 1123 est la copie hors deck.");
			Cell(off, "text_es").Should().Be(Cell(deck, "text_es"),
				"la copie hors deck a pris le titre espagnol de la carte imprimée.");
			Cell(off, "text_zh").Should().Be(Cell(deck, "text_zh"),
				"idem en chinois — pas l'ancien 错误的普遍化.");
		}

		[Fact]
		public void Deck_Still_Has_175_Cards()
		{
			var rows = LoadRows();
			rows.Count(r => !string.IsNullOrWhiteSpace(r.GetValueOrDefault("carte"))).Should().Be(175,
				"le grain n'écrit que des copies hors deck — le deck ne change pas de taille (#1288).");
		}
	}
}
