using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde du correctif de registre HORS deck (pool #458, arbitrage ai-01
	/// c.6233032782 item 3a) : PK 1142 « Reactancia » (path 6.3.1.2.2.3.2,
	/// carte vide — hors deck) portait le seul accord brisé INTRA-cellule du
	/// corpus Fallacies mesuré c.6213886301 : « Resistís » (vosotros) mêlé à
	/// « se te dice / tu libertad » (tú). L'autre cellule mélangée du corpus
	/// (rangée 509 example_es « Todos ustedes… ») est une réplique citée,
	/// légitime. Arbitration : convertir au tú — « Resistes », resistir 2sg,
	/// conversion RÉGULIÈRE -ís→-es (pas une irrégulière type 956/128).
	/// Les 12 exemples deck au registre mélangé restent statu quo (item 3b :
	/// répliques fidèles au FR « vous ») — la garde deck G9
	/// (<see cref="EsDeckRegisterTuGuardTests"/>) couvre desc_es du deck,
	/// celle-ci couvre la cellule hors deck arbitrée.
	/// </summary>
	public class EsOffDeckRegisterTuGuardTests
	{
		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static string Cell(string column, string pk)
		{
			var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn(column, "PK", new[] { pk });
			values.Should().HaveCount(1, "une seule rangée porte le PK {0} dans la taxonomie.", pk);
			return values[0];
		}

		// Mêmes marqueurs que la garde deck G9 : pronoms/possessifs 2pl + politesse,
		// puis morphologie résiduelle du vosotros (désinences -áis/-éis/-ís).
		private static readonly Regex VosotrosUstedMarkers = new(
			@"\b(vosotros|vosotras|vuestros?|vuestras?|ustedes|usted|sois)\b",
			RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static readonly Regex VosotrosEndings = new(
			@"\p{L}+(áis|éis|ís)\b",
			RegexOptions.Compiled);

		[Fact]
		public void Pk1142_DescEs_Pinned_TuRegister()
		{
			Cell("desc_es", "1142").Should().Be(
				"Resistes hacer lo que se te dice para preservar tu libertad de elección.",
				"arbitrage ai-01 (c.6233032782 item 3a) : l'accord brisé « Resistís » (vosotros) + " +
				"« se te dice / tu libertad » (tú) se résout au tú — resistir 2sg, -ís→-es régulier.");
		}

		[Fact]
		public void Pk1142_DescEs_NoVosotrosMarkerRemains()
		{
			var cell = Cell("desc_es", "1142");
			var m1 = VosotrosUstedMarkers.Match(cell);
			m1.Success.Should().BeFalse(
				"la cellule arbitrée au tú ne doit plus porter le marqueur « {0} » : « {1} »", m1.Value, cell);
			var m2 = VosotrosEndings.Match(cell);
			m2.Success.Should().BeFalse(
				"désinence vosotros résiduelle « {0} » dans « {1} »", m2.Value, cell);
		}

		/// <summary>
		/// Contrôle inverse corpus-scoped : « Resistís » était UNIQUE dans les
		/// colonnes _es (pré-chirurgie, mesuré 17/10 : 1 occurrence — cette
		/// cellule). Après le fix, 0 partout : l'ancienne forme ne survit
		/// nulle part, et une réinjection rouge. Les 429 cellules VOSOTROS
		/// légitimes hors deck ne portent PAS ce mot (unicité mesurée).
		/// </summary>
		[Fact]
		public void Resistis_OldForm_AbsentFromAllEsColumns()
		{
			var loader = new HarvestCardIdsCsv(FallaciesCsv);
			var offenders = new List<string>();
			foreach (var column in new[]
			{
				"Family_es", "Subfamily_es", "Subsubfamily_es",
				"text_es", "desc_es", "example_es",
			})
			{
				var hits = loader.LoadColumn(column)
					.Select((v, i) => (v, i))
					.Where(t => t.v.Contains("Resistís"))
					.Select(t => $"{column}[{t.i}]")
					.ToList();
				offenders.AddRange(hits);
			}
			offenders.Should().BeEmpty(
				"« Resistís » était unique (cette cellule, pré-fix mesuré) — toute occurrence " +
				"restante est une réinjection de la forme convertie : {0}",
				string.Join(", ", offenders));
		}
	}
}
