using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde des retours à la ligne intra-phrase (pool #458, dispatch
	/// c.5975630522 grain 2, mesure d'origine c.5972921914) : sur TOUT le
	/// corpus (deck et hors deck), un saut de ligne dans text_*/desc_*/
	/// example_* doit OUVRIR UNE RÉPLIQUE (marqueur « — »/« —— »/« - » en fin
	/// de ligne précédente ou en tête de la suivante) — jamais couper une
	/// phrase.
	/// <list type="bullet">
	/// <item>Re-dérivé depuis master `6bc573f7` : **23 cellules** jointes
	/// (le compte de la review était 24 — 476 example_fr n'en porte plus
	/// depuis #1735, qui y a écrit le texte de PK 1300 joint).</item>
	/// <item>Règle de jointure : zh sans espace (ponctuation pleine-largeur
	/// «论点，涵盖»), toutes les autres avec un espace ; les 11 coupures
	/// finales de format (rien après le \n) et les 12 coupures
	/// intra-phrase (« typique⏎du milieu », « …политик,⏎учитывая… ») ; aucune
	/// cellule ne séparait des éléments de liste — aucun cas douteux.</item>
	/// <item>Toutes hors deck (0 PDF) — les dialogues du deck (974/943/813)
	/// gardent leurs lignes, épargnés par la règle du marqueur.</item>
	/// <item>⚠️ La branche `fix/458-midphrase-joins` (c243f50d) reste une
	/// SOURCE, pas une base — la worklist y inclut 476 example_fr, joint
	/// depuis par #1735.</item>
	/// </list>
	/// Aucune exception nominative : le corpus ne porte plus aucun saut
	/// hors réplique.
	/// </summary>
	public class CorpusMidPhraseLineBreakGuardTests
	{
		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static IReadOnlyList<string> LoadColumn(string column) =>
			new HarvestCardIdsCsv(FallaciesCsv).LoadColumn(column);

		[Fact]
		public void AllRows_TextDescExample_LineBreaksOpenAReplyOrNothing()
		{
			var pks = LoadColumn("PK");
			foreach (var field in new[] { "text", "desc", "example" })
			{
				foreach (var lang in new[] { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" })
				{
					var column = field + "_" + lang;
					var values = LoadColumn(column);
					values.Count.Should().Be(pks.Count, "les deux colonnes couvrent les mêmes rangées.");
					for (var i = 0; i < values.Count; i++)
					{
						var cell = values[i] ?? string.Empty;
						if (!cell.Contains('\n'))
						{
							continue;
						}
						var lines = cell.Split('\n');
						for (var j = 0; j < lines.Length - 1; j++)
						{
							var opensReply = lines[j].TrimEnd().EndsWith("—")
								|| lines[j].TrimEnd().EndsWith("-")
								|| lines[j + 1].TrimStart().StartsWith("—")
								|| lines[j + 1].TrimStart().StartsWith("-");
							opensReply.Should().BeTrue(
								"PK {0} {1} : le saut de ligne coupe une phrase (23 jointes, grain 2 c.5975630522 ; "
								+ "mesure d'origine c.5972921914) — un \\n n'est légitime qu'entre répliques",
								pks[i], column);
						}
					}
				}
			}
		}

		private static readonly Dictionary<string, string> JoinedPins = new()
		{
			// 197 : couture intra-phrase française (« typique⏎du milieu académique »)
			["197|desc_fr"] = "Vous utilisez un langage formel, spécialisé et souvent complexe, typique du milieu académique, qui peut rendre votre discours inaccessibles aux non-initiés.",
			// 476 zh : couture pleine-largeur SANS espace (« 论点，⏎涵盖 »)
			["476|example_zh"] = "我的对手现在应该回应我关于各种经济、社会和环境政策的论点，涵盖短期和长期的影响，对弱势群体的影响，地缘政治考虑以及法律和伦理方面。",
			// 1300 ru : couture latine-cyrillique AVEC espace (« политик,⏎учитывая »)
			["1300|example_ru"] = "Мой оппонент теперь должен ответить на мои аргументы относительно различных экономических, социальных и экологических политик, учитывая краткосрочные и долгосрочные последствия, влияние на уязвимые населения, геополитические соображения и юридические и этические аспекты.",
			// 1117 : coupure finale de format (rien après le \n)
			["1117|desc_fr"] = "Vous sous-estimez l’impact des récompenses externes sur votre propre motivation et comportement.",
		};

		[Fact]
		public void JoinedSeams_Pinned_FullCell()
		{
			foreach (var (key, expected) in JoinedPins)
			{
				var parts = key.Split('|');
				var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn(parts[1], "PK", new[] { parts[0] });
				values.Should().HaveCount(1, "une seule rangée porte le PK {0}.", parts[0]);
				values[0].Should().Be(expected,
					"cellule jointe au grain 2 ({0} PK {1}) — la couture ne doit pas revenir", parts[1], parts[0]);
			}
		}
	}
}
