using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde du registre « tú » des définitions espagnoles du deck (pool #458,
	/// dispatch c.5975630522 grain 1, arbitrage c.5975624315) : 74 cellules
	/// desc_es converties (§2 vosotros ×39 tel quel · §3 ustedes ×20 + §4 usted
	/// ×5 SANS « Tú » initial, verbe en majuscule · §5 descriptives ×10 au tú —
	/// le français s'adresse au lecteur). L'espagnol n'a jamais été imprimé :
	/// décision sur délégation ; le changement entrera dans les PDF es à la
	/// prochaine régénération.
	/// <list type="bullet">
	/// <item>Aucun marqueur vosotros/ustedes/usted (pronom, possessif
	/// vuestro/vuestra, désinence -áis/-éis/-ís, « sois ») ne subsiste sur les
	/// 175 définitions du deck.</item>
	/// <item>Aucune définition ne commence par « Tú » — mesuré par ai-01 :
	/// aucune définition du deck ne commence ainsi ; le tú en place omet le
	/// pronom (« Basas tu argumentación… »).</item>
	/// <item>Irrégulières épinglées pleine cellule : 956 Pides (e→i, la règle
	/// générique -ís→-es produirait « Pedes »), 128 Piensas (e→ie), 121 desvías
	/// (accent), 219 basarte (enclitique), 1120 « si no eres perfecto »
	/// (accord).</item>
	/// <item>Conservations 3ᵉ personne (jugées, pas oubliées) : 1360 « sus
	/// argumentos » (ceux de l'adversaire), 799 « su sentido establecido »,
	/// 1312 « su alcance », 356 « se den cuenta ».</item>
	/// </list>
	/// Tableau source : docs/corpus/458-v23-g39-registre-es-tu-tableau-2026-10-04.md.
	/// </summary>
	public class EsDeckRegisterTuGuardTests
	{
		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static string Cell(string column, string pk)
		{
			var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn(column, "PK", new[] { pk });
			values.Should().HaveCount(1, "une seule rangée porte le PK {0} dans la taxonomie.", pk);
			return values[0];
		}

		/// <summary>Valeurs d'une colonne restreintes aux rangées du deck (carte non vide), ordre préservé.</summary>
		private static IReadOnlyList<string> DeckCells(string column)
		{
			var cartes = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("carte");
			var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn(column);
			values.Count.Should().Be(cartes.Count, "les deux colonnes couvrent les mêmes rangées.");
			return values.Where((v, i) => !string.IsNullOrWhiteSpace(cartes[i])).ToList();
		}

		// Pronoms/possessifs de la 2ᵉ personne plurielle et de la 3ᵉ personne de politesse,
		// puis morphologie résiduelle du vosotros (désinences -áis/-éis/-ís, « sois »).
		private static readonly Regex VosotrosUstedMarkers = new(
			@"\b(vosotros|vosotras|vuestros?|vuestras?|ustedes|usted|sois)\b",
			RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static readonly Regex VosotrosEndings = new(
			@"\p{L}+(áis|éis|ís)\b",
			RegexOptions.Compiled);

		[Fact]
		public void Deck_DescEs_NoVosotrosUstedesUstedMarkersRemain()
		{
			var cells = DeckCells("desc_es");
			cells.Should().HaveCount(175, "le deck compte 175 définitions.");
			foreach (var cell in cells)
			{
				var m1 = VosotrosUstedMarkers.Match(cell);
				m1.Success.Should().BeFalse(
					"le registre arbitré (c.5975624315) est le tú : « {0} » ne doit plus porter le marqueur « {1} »",
					cell, m1.Value);
				var m2 = VosotrosEndings.Match(cell);
				m2.Success.Should().BeFalse(
					"désinence vosotros résiduelle « {0} » dans « {1} » (convertie au tú, grain 1)",
					m2.Value, cell);
			}
		}

		[Fact]
		public void Deck_DescEs_NoneStartsWithTú()
		{
			foreach (var cell in DeckCells("desc_es"))
			{
				cell.Should().NotStartWith("Tú ",
					"aucune définition du deck ne commence par « Tú » (mesuré c.5975624315) — le pronom explicite en tête serait emphatique ; forme retenue : verbe en majuscule (« Fundamentas tus argumentos… »).");
			}
		}

		private static readonly Dictionary<string, string> Irregulars = new()
		{
			["956"] = "Pides una excepción a una regla sin razón válida.", // pedir, diphtongue e→i (« Pedes » serait faux)
			["128"] = "Piensas que una idea es válida solo porque quien la presenta es rico.", // pensar, e→ie
			["121"] = "Rechazas o desvías un argumento por temor a que pueda ofender a una parte del público.", // accent desviar
			["219"] = "Usas el humor para hacer que tu argumentación sea más atractiva, sin basarte en hechos.", // enclitique basarOS → basarTE
			["1120"] = "Razonas en términos de todo o nada, sin matices: si no eres perfecto, te consideras un fracaso total.", // accord de l'attribut
		};

		[Fact]
		public void IrregularConversions_Pinned_FullCell()
		{
			foreach (var (pk, expected) in Irregulars)
			{
				var cell = Cell("desc_es", pk);
				cell.Should().Be(expected,
					"conversion irrégulière arbitrée (c.5975624315, §6 du g39) — la règle générique -ís→-es / -áis→-as serait fausse ici. PK {0}.", pk);
			}
		}

		[Fact]
		public void ThirdPersonConservations_Kept()
		{
			Cell("desc_es", "1360").Should().Contain("sus argumentos",
				"1360 : les arguments restent ceux de l'adversaire (3ᵉ) — seul « su adversario » devient « tu adversario ».");
			Cell("desc_es", "799").Should().Contain("su sentido establecido",
				"799 : le sens établi des termes reste 3ᵉ personne.");
			Cell("desc_es", "1312").Should().Contain("su alcance",
				"1312 : l'ampleur du débat reste 3ᵉ personne ; « a su favor » devient « a tu favor ».");
			Cell("desc_es", "356").Should().Contain("se den cuenta",
				"356 : les interlocuteurs (3ᵉ) se rendent compte — seule l'adresse est convertie.");
		}
	}
}
