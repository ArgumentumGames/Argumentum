using System;
using System.IO;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Organe de parité #458 G-CC-bis : l'overlay des vignettes News5
	/// (<c>shared/_Article Image.cshtml</c> → <c>&lt;span class="overlay"&gt;</c>) est positionné en
	/// absolu avec <c>top/bottom: 0</c> et <c>width: 100%</c>. Sans <c>left: 0</c>, il prend sa
	/// <b>position statique</b> : tant que l'image différée n'est pas chargée, le conteneur s'effondre à
	/// une boîte de ligne de 24 px et l'overlay démarre à la position inline centrée (50 % de la carte),
	/// puis s'étend d'une largeur pleine vers la droite. Mesuré sur la préprod : <c>scrollWidth</c>
	/// 539 px pour <c>clientWidth</c> 375 (mobile, /en et /ar) au lieu de 378 ; <c>left</c> calculé
	/// 175,5 px au lieu de 0. La déclaration existe dans <c>bs3</c>, les builds <c>bs4</c>/<c>bs5</c>
	/// l'ont perdue — c'est donc une régression de build silencieuse, que rien ne voyait.
	///
	/// L'organe épingle la <b>parité</b> des trois variantes, source SCSS et CSS minifié : si un build
	/// ultérieur reperd la déclaration, la suite passe au rouge au lieu de rendre la page glissante.
	/// </summary>
	public class News5OverlayCssParityTests
	{
		private const string AppDir = "DNNPlatform/Portals/1/2sxc/News5";

		/// <summary>Base selector of the overlay rule, as it appears (unminified / minified) in the dist CSS.</summary>
		private static readonly Regex DistRule = new Regex(
			@"\.app-news5-details-link \.app-news5-img span\{([^}]*)\}",
			RegexOptions.Compiled);

		/// <summary>Declarations of the <c>.app-news5-img span { … }</c> block in the SCSS source.</summary>
		private static readonly Regex ScssRule = new Regex(
			@"\.app-news5-img\s*\{[\s\S]*?span\s*\{([^{}]*)\}",
			RegexOptions.Compiled);

		private static readonly Regex LineComment = new Regex(@"//[^\r\n]*", RegexOptions.Compiled);

		[Theory]
		[InlineData("bs3")]
		[InlineData("bs4")]
		[InlineData("bs5")]
		public void Dist_Css_Overlay_Declares_Left_Zero(string variant)
		{
			var declarations = DeclarationsFromDist(variant);

			Regex.IsMatch(declarations, @"(^|;)left:0(;|$)")
				.Should().BeTrue(
					"#458 G-CC-bis : la règle {0}/dist/styles.min.css doit déclarer left:0 — sans elle, " +
					"l'overlay News5 prend sa position statique avant le chargement des images différées et " +
					"la page mobile glisse horizontalement (mesuré : scrollWidth 539 au lieu de 378). " +
					"Déclarations trouvées : '{1}'.", variant, declarations);
		}

		[Theory]
		[InlineData("bs3")]
		[InlineData("bs4")]
		[InlineData("bs5")]
		public void Scss_Source_Overlay_Declares_Left_Zero(string variant)
		{
			var declarations = DeclarationsFromScss(variant);

			Regex.IsMatch(declarations, @"(^|;|\s)left\s*:\s*0\s*;")
				.Should().BeTrue(
					"#458 G-CC-bis : {0}/styles/_list.scss doit déclarer left: 0 dans le bloc " +
					".app-news5-img span, sinon le prochain build webpack réintroduit le défaut dans le " +
					"CSS minifié. Déclarations trouvées : '{1}'.", variant, declarations);
		}

		private static string DeclarationsFromDist(string variant)
		{
			var path = Path.Combine(TestRepoRoot.Find(), AppDir, variant, "dist", "styles.min.css");
			File.Exists(path).Should().BeTrue("le CSS minifié de la variante {0} doit exister : {1}", variant, path);

			var text = File.ReadAllText(path);
			var match = DistRule.Match(text);
			match.Success.Should().BeTrue(
				"la règle '.app-news5-details-link .app-news5-img span' doit exister dans {0}/dist/styles.min.css " +
				"— si elle a disparu, cet organe ne mesure plus rien et doit être révisé", variant);

			return match.Groups[1].Value.Trim();
		}

		private static string DeclarationsFromScss(string variant)
		{
			var path = Path.Combine(TestRepoRoot.Find(), AppDir, variant, "styles", "_list.scss");
			File.Exists(path).Should().BeTrue("la source SCSS de la variante {0} doit exister : {1}", variant, path);

			var text = LineComment.Replace(File.ReadAllText(path), string.Empty);
			var match = ScssRule.Match(text);
			match.Success.Should().BeTrue(
				"le bloc '.app-news5-img span' doit exister dans {0}/styles/_list.scss " +
				"— si la structure a changé, cet organe doit être révisé", variant);

			return Regex.Replace(match.Groups[1].Value, @"\s+", " ").Trim();
		}
	}
}
