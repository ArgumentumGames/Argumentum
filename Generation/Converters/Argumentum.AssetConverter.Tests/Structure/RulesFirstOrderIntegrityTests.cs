using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Structure
{
	/// <summary>
	/// #1460 <b>phase C</b> — le contrôle inverse de <see cref="RulesFirstOrderIntegrity"/>, exécuté
	/// en CI <b>sans aucun artefact</b>.
	///
	/// <para>Une garde jamais vue rouge est un no-op (#1046). Chaque prédicat est donc exercé sur
	/// l'état <b>défectueux</b> avant d'être cru vert sur un bundle — et le fichier porte le
	/// <b>contrôle inverse du contrôle</b> : un jeu sain ne produit <b>aucun</b> échec, sans quoi un
	/// vérificateur toujours-rouge passerait tous les témoins ci-dessous.</para>
	///
	/// <para>⚠️ Les empreintes sont <b>synthétiques et uniformes</b> : la distance entre deux d'entre
	/// elles est exactement la différence de leur niveau, ce qui rend chaque témoin lisible sans
	/// instrument. Le calibrage sur de vraies cartes (<see cref="RulesFirstOrderIntegrity.MinSeparationRatio"/>,
	/// <see cref="RulesFirstOrderIntegrity.MaxBestDistance"/>) a été fait séparément sur
	/// <c>Argumentum_TarotCards_fr.pdf</c> le 06/11, et il est cité dans la doc du moteur — ⛔ ces
	/// témoins ne le re-dérivent pas.</para>
	/// </summary>
	public class RulesFirstOrderIntegrityTests
	{
		private static byte[] Level(byte value) =>
			Enumerable.Repeat(value, RulesFirstOrderIntegrity.Grid * RulesFirstOrderIntegrity.Grid * 3).ToArray();

		private static RulesFirstOrderIntegrity.CardPrint Card(string name, byte level) =>
			new RulesFirstOrderIntegrity.CardPrint(name, Level(level));

		[Fact]
		public void Healthy_Set_Produces_No_Failure()
		{
			// CONTRÔLE INVERSE DU CONTRÔLE : sans lui, un vérificateur toujours-rouge passerait
			// tous les autres témoins de ce fichier.
			var cards = new[]
			{
				Card("Rules/rules_01_face.png", 0),
				Card("Rules/rules_02_face.png", 50),
				Card("Fallacies/card_001.png", 200)
			};

			var failures = RulesFirstOrderIntegrity.Check(
				new[] { Level(0), Level(50) },
				cards,
				new[] { "Rules/rules_01_face.png", "Rules/rules_02_face.png" });

			failures.Should().BeEmpty();
		}

		[Fact]
		public void Swapped_Pages_Are_Red_And_Name_Both_Cards()
		{
			var cards = new[]
			{
				Card("Rules/rules_01_face.png", 0),
				Card("Rules/rules_02_face.png", 50),
				Card("Fallacies/card_001.png", 200)
			};

			var failures = RulesFirstOrderIntegrity.Check(
				new[] { Level(50), Level(0) },
				cards,
				new[] { "Rules/rules_01_face.png", "Rules/rules_02_face.png" });

			failures.Should().HaveCount(2);
			failures[0].Should().Contain("page 1").And.Contain("rules_01_face.png").And.Contain("rules_02_face.png");
			failures[1].Should().Contain("page 2").And.Contain("rules_02_face.png").And.Contain("rules_01_face.png");
		}

		[Fact]
		public void A_Deck_Starting_With_A_Fallacy_Is_Red()
		{
			// LE défaut que l'organe garde : le deck commence par autre chose qu'une règle. Le
			// candidat gagnant existe et gagne nettement — la garde doit quand même refuser.
			var cards = new[]
			{
				Card("Rules/rules_01_face.png", 0),
				Card("Fallacies/card_001.png", 200)
			};

			var failures = RulesFirstOrderIntegrity.Check(
				new[] { Level(200) },
				cards,
				new[] { "Rules/rules_01_face.png" });

			failures.Should().ContainSingle()
				.Which.Should().Contain("attendu « Rules/rules_01_face.png »")
				.And.Contain("obtenu « Fallacies/card_001.png »");
		}

		[Fact]
		public void An_Ambiguous_Match_Is_Red_Even_When_The_Name_Is_Right()
		{
			// Deux références identiques : le gagnant est nommément le bon, mais la décision ne
			// repose sur RIEN. Seul le garde-fou de marge peut l'attraper.
			var cards = new[]
			{
				Card("Rules/rules_01_face.png", 0),
				Card("Rules/rules_02_face.png", 0)
			};

			var failures = RulesFirstOrderIntegrity.Check(
				new[] { Level(0) },
				cards,
				new[] { "Rules/rules_01_face.png" });

			failures.Should().ContainSingle()
				.Which.Should().Contain("ambigu").And.Contain("marge");
		}

		[Fact]
		public void A_Page_That_Resembles_Nothing_Is_Red_Even_When_It_Wins()
		{
			// Le nom attendu est bien celui qui gagne : seul l'écart absolu peut refuser. Sans cette
			// borne, une page blanche ou corrompue « gagnerait » toujours contre une référence.
			var cards = new[]
			{
				Card("Rules/rules_01_face.png", 0),
				Card("Fallacies/far.png", 200)
			};

			var failures = RulesFirstOrderIntegrity.Check(
				new[] { Level(250) },
				cards,
				new[] { "Fallacies/far.png" });

			failures.Should().ContainSingle()
				.Which.Should().Contain("de trop loin").And.Contain("20.00");
		}

		[Fact]
		public void Too_Few_Pages_Is_Red()
		{
			var cards = new[]
			{
				Card("Rules/rules_01_face.png", 0),
				Card("Rules/rules_02_face.png", 50)
			};

			var failures = RulesFirstOrderIntegrity.Check(
				new[] { Level(0) },
				cards,
				new[] { "Rules/rules_01_face.png", "Rules/rules_02_face.png" });

			failures.Should().ContainSingle().Which.Should().Contain("1 page(s)").And.Contain("2 carte(s)");
		}

		[Fact]
		public void An_Empty_Expectation_Is_Red_Not_Silently_Green()
		{
			// Un attendu vide rendrait « 0 échec » sur un deck jamais examiné : la forme la plus
			// discrète du faux vert.
			var failures = RulesFirstOrderIntegrity.Check(
				new[] { Level(0) },
				new[] { Card("Rules/rules_01_face.png", 0), Card("Rules/rules_02_face.png", 50) },
				new string[0]);

			failures.Should().ContainSingle().Which.Should().Contain("aucun attendu");
		}

		[Fact]
		public void Distance_Refuses_Mismatched_Sizes_Instead_Of_Inventing_A_Verdict()
		{
			RulesFirstOrderIntegrity.Distance(new byte[] { 1, 2, 3 }, new byte[] { 1, 2 })
				.Should().Be(double.NaN);

			RulesFirstOrderIntegrity.Distance(new byte[] { 1, 2, 3 }, new byte[] { 0, 0, 0 })
				.Should().BeApproximately(2.0, 1e-9);
		}
	}
}
