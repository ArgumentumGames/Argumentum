using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde d'homonymie du sous-arbre « Infini trompeur » (#458 pool v18 grain 1, décision ai-01
	/// règle C — dossier #1578). La passe #369 (commit <c>9d45b4f9</c>) a aligné le
	/// <c>text_fr</c> du parent <c>3.2.3.2</c> (PK 661) sur celui de son fils <c>3.2.3.2.2</c>
	/// (PK 664), créant la seule homonymie fr de ce sous-arbre — les 7 autres langues
	/// désambiguïsent déjà. #1578 a établi qu'il s'agit de deux concepts distincts (parent
	/// générique vs fils « dénoncer à tort une régression ») ; cette garde épingle le retour du
	/// parent à son libellé pré-#369, lu dans l'arbre du parent du commit
	/// (<c>git show 9d45b4f9^</c> : « Mauvais argument de la régression »).
	/// Elle échoue si une autre cellule de la famille bouge (662/664 épinglés) ou si l'un des
	/// trois libellés redevient porté par plus d'une rangée.
	/// </summary>
	public class FallaciesPk661TitleGuardTests
	{
		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static readonly string[] Pk661 = { "661" };
		private static readonly string[] Pk662 = { "662" };
		private static readonly string[] Pk664 = { "664" };

		private const string ParentLabel = "Mauvais argument de la régression";
		private const string SonLabel = "Mauvais argument de la régression infinie";
		private const string SisterLabel = "Régression infinie vicieuse";

		// NB: exprimé en HaveCount(1) + Be, jamais ContainSingle(valeur, parce-que) — cette
		// surcharge résout vers ContainSingle(because) qui n'épingle PAS la valeur : mesurée
		// vacue sous mutation « GARBAGE PROBE » avant réécriture.
		private static void AssertSingleRowTitle(string[] pk, string expected, string because)
		{
			var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("text_fr", "PK", pk);
			values.Should().HaveCount(1, "une seule rangée porte ce PK dans la taxonomie.");
			values[0].Should().Be(expected, because);
		}

		[Fact]
		public void Pk661_TitleFr_Is_Pre369_Parent_Label()
		{
			AssertSingleRowTitle(Pk661, ParentLabel,
				"v18 grain 1 : PK 661 (3.2.3.2, parent générique) reprend son libellé pré-#369 lu dans " +
				"git show 9d45b4f9^ — #369 l'avait aligné sur le fils 664, créant la seule homonymie fr " +
				"du sous-arbre (dossier #1578).");
		}

		[Fact]
		public void Pk664_TitleFr_Is_Unchanged_Son_Label()
		{
			AssertSingleRowTitle(Pk664, SonLabel,
				"le fils 3.2.3.2.2 ne bouge pas : il garde le libellé que la famille porte depuis " +
				"#369 (seul le parent est renommé).");
		}

		[Fact]
		public void Pk662_TitleFr_Is_Unchanged_Sister_Label()
		{
			AssertSingleRowTitle(Pk662, SisterLabel,
				"3.2.3.2.1 est hors périmètre du grain 1 : épingler son libellé courant fait échouer " +
				"la garde si une passe touche une cellule voisine par ricochet.");
		}

		[Fact]
		public void Each_Family_Label_Has_Exactly_One_Carrier_In_Fr()
		{
			var titles = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("text_fr");

			titles.Count(t => string.Equals(t, ParentLabel, StringComparison.Ordinal)).Should().Be(1,
				"le renommage ne doit pas échanger une homonymie contre une autre : 'Mauvais argument " +
				"de la régression' n'était porté par aucune autre rangée avant le grain 1 (mesuré 26/09).");
			titles.Count(t => string.Equals(t, SonLabel, StringComparison.Ordinal)).Should().Be(1,
				"après le renommage du parent, seul le fils 664 porte 'Mauvais argument de la " +
				"régression infinie' — deux porteurs = l'homonymie #369 réintroduite.");
			titles.Count(t => string.Equals(t, SisterLabel, StringComparison.Ordinal)).Should().Be(1,
				"la sœur 662 reste l'unique porteuse de son libellé.");
		}
	}
}
