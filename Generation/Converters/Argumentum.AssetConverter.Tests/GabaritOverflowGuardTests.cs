using System.IO;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde ⑦ du pool v22 (#458 c.5856549779) : le gabarit Fallacies Face porte les deux
	/// leviers mesurés contre les débordements de titres et de bandeaux de famille.
	/// Mesures po-2023 (27/09, run Debug restreint fr+es, OverflowDetector tolérance 2 px) :
	/// le line-height du titre est LE levier (33,1 px → 4,8 px sur la sonde 53 chars ;
	/// padding et structure .image testés SANS apport). Le bandeau ru « Злоупотребление
	/// языком » (23 cartes, débordement bilatéral) est un mot insécable de 15 chars plus
	/// large que la ligne : `hyphens: auto` (lang réécrit par langue à l'injection,
	/// AssetConverterConfig ARGU_LANG_MARKER) + `overflow-wrap: break-word` en repli.
	/// </summary>
	public class GabaritOverflowGuardTests
	{
		private static string FallaciesFaceCss()
		{
			var path = Path.Combine(TestRepoRoot.Find(), "Cards", "Fallacies",
				"Argumentum_Fallacies_Face_fr.json");
			var json = JsonDocument.Parse(File.ReadAllText(path));
			return json.RootElement.GetProperty("css").GetString();
		}

		private static string RuleOf(string css, string selector)
		{
			// le JSON embarque la feuille dans une chaîne : découpe règle par règle,
			// blocs sans imbrication (cas de ce gabarit). Le sélecteur exact est le
			// fragment depuis le dernier '}' / retour ligne / ',' — « card .famille »
			// (variante couleur) ne doit PAS matcher « .famille ».
			var parts = css.Split('{');
			for (var i = 1; i < parts.Length; i++)
			{
				var raw = parts[i - 1];
				var cut = raw.LastIndexOfAny(new[] { '}', '\n', '\r', ',' });
				var sel = (cut >= 0 ? raw.Substring(cut + 1) : raw).Trim();
				if (sel == selector)
					return parts[i].Split('}')[0];
			}
			return string.Empty;
		}

		[Fact]
		public void Title_Rule_Carries_The_Measured_LineHeight_Lever()
		{
			var css = FallaciesFaceCss();
			var title = RuleOf(css, ".title");
			title.Should().Contain("line-height: 1em",
				"⑦ : 1.2em débordait (33,1 px sur la sonde es 53 chars) ; 1em est le levier " +
				"mesuré par po-2023 (résiduel 4,8 px), les autres gestes testés étaient sans apport.");
			title.Should().NotContain("line-height: 1.2em",
				"l'ancienne valeur ne doit plus exister dans .title.");
		}

		[Fact]
		public void Famille_Rule_Carries_Hyphens_And_BreakWord()
		{
			var css = FallaciesFaceCss();
			var famille = RuleOf(css, ".famille");
			famille.Should().Contain("hyphens: auto",
				"⑦ : le bandeau ru « Злоупотребление языком » (15 chars insécables) césure " +
				"proprement — l'attribut lang est réécrit par langue à l'injection.");
			famille.Should().Contain("overflow-wrap: break-word",
				"⑦ : repli garanti quand le dictionnaire de césure n'a pas de point " +
				"(le mot coupe plutôt que de déborder des deux côtés).");
		}

		[Fact]
		public void Other_Rules_Untouched_By_The_Grain()
		{
			var css = FallaciesFaceCss();
			RuleOf(css, ".desc_fr").Should().Contain("line-height: 1.4em",
				"le corps de description garde son interligne — ⑦ ne touche que .title et .famille.");
			RuleOf(css, ".famille").Should().Contain("font-size: 1.8em",
				"la taille du bandeau ne change pas — le fix est conditionnel par construction " +
				"(césure/repli ne s'activent que si le mot ne tient pas).");
		}
	}
}
