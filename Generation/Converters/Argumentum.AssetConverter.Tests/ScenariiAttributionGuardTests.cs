using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Organe d'attribution Scenarii (#1226) : le pied des cartes Scenarii est
	/// <c>&lt;div&gt;{{CCby}}&lt;/div&gt;</c> — une colonne CSV sans variante de langue. Au Golden Master
	/// (<c>0087f0ec</c>, avril 2024) le deck de 91 cartes était à 91/91 ; le deck est passé à 167 cartes
	/// et les lignes ajoutées n'ont jamais reçu l'attribution — <b>44 % du deck sortait sans sa ligne
	/// Creative Commons, à l'identique dans les 8 boîtes</b>, sans qu'aucun organe ne le voie (défaut
	/// découvert au verdict BAT #1187).
	///
	/// Ce garde-fou rend la correction auto-défendue : il échoue à l'instant où une ligne Scenarii
	/// porte un <c>CCby</c> vide (ou une valeur hors constante), c'est-à-dire exactement au moment où
	/// une nouvelle carte est ajoutée sans attribution — le motif même qui a reformé le trou de 91 à
	/// 167. Le compte n'est PAS épinglé (167 grandira légitimement) : l'invariant est « toute ligne
	/// porte l'attribution », pas « le deck a N lignes » (ce dernier contrat relève de
	/// <see cref="CardSetExpectedCardCountContractTests"/>).
	/// </summary>
	public class ScenariiAttributionGuardTests
	{
		private static string ScenariiCsv =>
			System.IO.Path.Combine(TestRepoRoot.Find(), "Cards", "Scenarii", "Argumentum Scenarii - Cards.csv");

		private const string AttributionColumn = "CCby";
		private const string CorpusConstant = "Argumentum";

		[Fact]
		public void Scenarii_EveryRow_Carries_CCby_Attribution()
		{
			var values = new HarvestCardIdsCsv(ScenariiCsv).LoadColumn(AttributionColumn);

			values.Should().NotBeEmpty("le CSV Scenarii doit porter des lignes de données");

			var empty = values.Count(v => string.IsNullOrWhiteSpace(v));
			empty.Should().Be(0,
				"#1226 : {0} ligne(s) Scenarii sur {1} portent un {2} vide — le pied de carte rend une attribution " +
				"Creative-Commons absente, dans les 8 boîtes à l'identique (colonne sans variante de langue). " +
				"Toute carte ajoutée doit recevoir la constante '" + CorpusConstant + "'.",
				empty, values.Count, AttributionColumn);
		}

		[Fact]
		public void Scenarii_CCby_Is_The_Corpus_Constant()
		{
			var values = new HarvestCardIdsCsv(ScenariiCsv).LoadColumn(AttributionColumn);

			var distinct = values.Where(v => !string.IsNullOrWhiteSpace(v)).Distinct().ToList();
			distinct.Should().BeEquivalentTo(new[] { CorpusConstant },
				"#1226 : {0} est une constante de corpus (mesurée 93×'" + CorpusConstant + "' à la découverte, " +
				"0 variante de langue). Valeurs distinctes rencontrées : {1}.",
				AttributionColumn, string.Join(", ", distinct.Select(d => $"'{d}'")));
		}
	}
}
