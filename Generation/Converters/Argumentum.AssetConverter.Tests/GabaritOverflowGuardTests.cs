using System.IO;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde ⑦ du pool v22 (#458 c.5856549779, complément review 5332544432) : le
	/// gabarit Fallacies Face porte le levier titre mesuré (line-height 1em, po-2023
	/// 27/09) et le levier bandeau RETRAVAILLÉ : le bandeau de famille ru « Злоупотребление
	/// языком » (22 cartes qui débordaient de 15/11 px sur la bordure, famille 5.x)
	/// se règle par RÉDUCTION de police via autoFitTitle (#400), jamais par césure —
	/// décision ai-01 : « ЗЛОУПОТРЕБ-ЛЕНИЕ » coupé est pire qu'un bandeau plus petit.
	/// L'ancien levier hyphens:auto + overflow-wrap:break-word n'était pas sans effet
	/// par accident : il est RETIRÉ du gabarit ET de la garde (une garde épinglant un
	/// levier interdit ferait échouer la correction elle-même).
	/// </summary>
	public class GabaritOverflowGuardTests
	{
		private static string FallaciesFaceCss()
		{
			var path = Path.Combine(TestRepoRoot.Find(), "Cards", "Fallacies",
				"Argumentum_Fallacies_Face_fr.json");
			var json = JsonDocument.Parse(File.ReadAllText(path));
			var css = json.RootElement.GetProperty("css").GetString() ?? string.Empty;
			css.Should().NotBeNullOrEmpty("le gabarit Fallacies Face porte une clé css.");
			return css;
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

		private static string FrameJs()
		{
			var path = Path.Combine(TestRepoRoot.Find(), "Generation", "CardPen", "js", "frame.js");
			return File.ReadAllText(path);
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
		public void Famille_Rule_Forbids_Hyphenation_Keeps_BaseSize()
		{
			var css = FallaciesFaceCss();
			var famille = RuleOf(css, ".famille");
			famille.Should().NotContain("hyphens",
				"complément ⑦ (review 5332544432) : on ne coupe JAMAIS un nom de famille " +
				"imprimé — décision typographique ai-01. La césure est retirée du gabarit.");
			famille.Should().NotContain("overflow-wrap",
				"complément ⑦ : le repli break-word couperait le mot si autoFit n'agit pas — " +
				"pire que le débordement selon la décision ai-01.");
			famille.Should().Contain("font-size: 1.8em",
				"la taille de base ne change pas — la réduction est conditionnelle " +
				"(autoFitTitle au moment de la capture), pas structurelle.");
		}

		[Fact]
		public void Framejs_AutoFit_Selector_Includes_Famille()
		{
			var js = FrameJs();
			js.Should().Contain("querySelectorAll('.title, .subtitle, .footer-tagline, .famille')",
				"complément ⑦ : le levier bandeau VIT dans frame.js — autoFitTitle réduit la " +
				"police du bandeau quand son mot insécable déborde le panneau (no-op sinon). " +
				"Le gabarit seul ne peut pas réduire conditionnellement.");
			js.Should().Contain("function autoFitTitle",
				"le mécanisme #400 réutilisé tel quel — pas un second mécanisme écrit.");
		}

		[Fact]
		public void Famille_Box_Constrained_To_Panel()
		{
			var css = FallaciesFaceCss();
			// LA contrainte du levier : .header est display:flex, donc .supersetWrapper
			// est un flex item shrink-to-fit — sans largeur forcée, la boîte du bandeau
			// épouse le mot (mesuré : boîte 252px = mot 252px, scrollWidth == clientWidth,
			// autoFitTitle jamais déclenché). C'est le diagnostic de la review : « le mot
			// tient dans sa boîte, mais sa boîte déborde du panneau ».
			RuleOf(css, ".supersetWrapper").Should().Contain("width: 100%",
				"complément ⑦ : le flex item est contraint à la largeur du panneau — c'est CE " +
				"qui rend scrollWidth > clientWidth (donc la réduction autoFit) possible " +
				"quand le mot est plus large que le panneau.");
			RuleOf(css, ".supersetWrapper .famille").Should().Contain("width: 100%",
				"le bandeau remplit le wrapper contraint (les deux moitiés du levier).");
		}

		[Fact]
		public void Other_Rules_Untouched_By_The_Grain()
		{
			var css = FallaciesFaceCss();
			RuleOf(css, ".desc_fr").Should().Contain("line-height: 1.4em",
				"le corps de description garde son interligne — ⑦ ne touche que .title et .famille.");
		}
	}
}
