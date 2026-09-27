using System.IO;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde ⑨ du pool v22 (#458 c.5857960866 item 3) : typographie hors deck.
	/// Règle owner 25/09 : « on corrige toujours », sans question. Fichier séparé de
	/// CassePhraseGuardTests pour ne pas entrer en collision avec la PR ⑧ (#1608)
	/// sur le même fichier de garde.
	/// Les retardataires EN étaient invisibles au détecteur de casse de titre :
	/// « fulfilling »/« politics »/« security » sont des porteurs minuscules — un titre
	/// comme « Social Engineering (politics) » n'est PAS en casse de titre pour le
	/// classeur. D'où l'épinglage positif ici plutôt qu'un élargissement du détecteur.
	/// </summary>
	public class TypographyHorsDeckGuardTests
	{
		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");
		private static string VirtuesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");

		private static string CellOf(string csvPath, string path, string column)
		{
			var csv = new HarvestCardIdsCsv(csvPath);
			var values = csv.LoadColumn(column, "path", new[] { path });
			values.Should().HaveCount(1, "une seule rangée porte ce path.");
			return values[0].Trim();
		}

		[Fact]
		public void G9_Typos_Fixed()
		{
			CellOf(FallaciesCsv, "3.1.2.2", "text_pt").Should().Be("Sofisma do acidente contrário",
				"⑨ pt : « contraio » n'existe pas — l'adjectif est « contrário ».");
			CellOf(FallaciesCsv, "1.3.3.3", "text_pt").Should().Be("Violação do cânone de Morgan",
				"⑨ pt : « cânnon » coquille — le nom est « cânone » (es « canon » est correct).");
			CellOf(FallaciesCsv, "3.1.2.3", "text_ru").Should().Be("Чрезмерное обобщение",
				"⑨ ru : « обобщение » est neutre — l'accord est « Чрезмерное », comme le zh « 过度概括 ».");
		}

		[Fact]
		public void G9_En_Casing_Stragglers_Fixed()
		{
			CellOf(FallaciesCsv, "2.3.1.3", "text_en").Should().Be("Self-fulfilling prophecy",
				"⑨ en : retardataire de ① — casse de phrase.");
			CellOf(FallaciesCsv, "2.3.1.3.1", "text_en").Should().Be("Endogenous self-fulfilling prophecy",
				"⑨ en : retardataire de ①.");
			CellOf(FallaciesCsv, "2.3.1.6", "text_en").Should().Be("Social engineering (politics)",
				"⑨ en : retardataire de ① (dispatch).");
			CellOf(FallaciesCsv, "2.3.2.2.5", "text_en").Should().Be("Social engineering (security)",
				"⑨ en : jumeau du précédent, même défaut (le dispatch nommait (politics) ; " +
				"typographie ⇒ on corrige toujours).");
			CellOf(FallaciesCsv, "6.3.2.1.2.2.2", "text_en").Should().Be("In-group favoritism",
				"⑨ en : retardataire de ①.");
		}

		[Fact]
		public void G9_Virtues_Unit_Resolution_SentenceCase()
		{
			CellOf(VirtuesCsv, "4.3.3.1.1.7", "title_en").Should().Be("Inference: unit resolution",
				"⑨ Vertus en : alignée sur sa voisine « Inference: resolution » (4.3.3.1.1.8, ⑤).");
		}

		[Fact]
		public void G9_Zh_Gish_Form()
		{
			// PROPOSITION po-2024, ai-01 tranche au merge (dispatch ⑨). Les jumeaux du
			// corpus portent « 吉什急袭 » (4.3.1.4.1) et « 吉什突袭 » (7.2.1.2.3) : la
			// translittération « 吉什 » est établie ; la forme suit le jumeau 4.3.1.4.1.
			// Alternative documentée : « 吉什突袭 » (alignement sur l'autre jumeau).
			CellOf(FallaciesCsv, "2.3.2.3.2.2.1", "text_zh").Should().Be("吉什急袭",
				"⑨ zh : « Gish Gallop » resté en anglais — proposition alignée sur le jumeau " +
				"4.3.1.4.1, ai-01 tranche (veto possible au merge).");
		}

		[Fact]
		public void Removed_G9_Forms_Absent()
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			foreach (var (column, form) in new[] {
				("text_pt", "Sofisma do acidente contraio"),
				("text_pt", "Violação do cânnon de Morgan"),
				("text_ru", "Чрезмерная обобщение"),
				("text_en", "Self-fulfilling Prophecy"),
				("text_en", "Social Engineering (politics)"),
				("text_en", "Social Engineering (security)"),
				("text_en", "In-group Favoritism"),
				("text_zh", "Gish Gallop"),
			})
				csv.LoadColumn(column).Count(v => string.Equals(v.Trim(), form, System.StringComparison.Ordinal))
					.Should().Be(0, $"la forme retirée «{form}» ne doit plus exister en {column} (⑨).");
			var virtues = new HarvestCardIdsCsv(VirtuesCsv);
			virtues.LoadColumn("title_en").Count(v => string.Equals(v.Trim(), "Inference: Unit resolution", System.StringComparison.Ordinal))
				.Should().Be(0, "la forme retirée ne doit plus exister en title_en (⑨ Vertus).");
		}
	}
}
