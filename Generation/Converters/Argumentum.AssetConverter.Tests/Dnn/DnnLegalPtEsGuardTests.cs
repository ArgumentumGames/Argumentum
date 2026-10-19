using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde du grain juridique pt-BR + es-ES (pool #458, item po-2024.4, GO
	/// miroir #1908) : les deux locales portaient la tête LCEN traduite mais un
	/// corps TERMS et une PRIVACY intégralement anglais. pt-BR était en plus
	/// un botch mot-à-vous hérité (« responsível » ×4, « não » injecté dans de
	/// l'EN y compris majuscule, « interative ») — remplacé, pas retouché.
	/// Les comptes (jetons portail, SubHead, &lt;li&gt;) sont DÉRIVÉS du neutre
	/// vivant au moment du test, jamais figés : la couverture de cette garde
	/// est la parité structurelle, pas un golden.
	/// </summary>
	public class DnnLegalPtEsGuardTests
	{
		private const string GlobalNeutralResx = "DNNPlatform/App_GlobalResources/GlobalResources.resx";
		private const string LocaleBodyAnchor = "ACORDO ENTRE O USUÁRIO E";       // pt-BR
		private const string LocaleBodyAnchorEs = "ACUERDO ENTRE EL USUARIO Y";    // es-ES
		private const string EnBodyMarker = "AGREEMENT BETWEEN USER";              // doit avoir disparu

		private static XDocument Load(string resxRelativePath)
		{
			var path = Path.Combine(TestRepoRoot.Find(), resxRelativePath.Replace('/', Path.DirectorySeparatorChar));
			File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({resxRelativePath})");
			return XDocument.Load(path);
		}

		private static string ResxValue(string resxRelativePath, string key)
		{
			var entry = Load(resxRelativePath).Root!.Elements("data")
				.FirstOrDefault(d => string.Equals(d.Attribute("name")?.Value, key, StringComparison.Ordinal));
			entry.Should().NotBeNull($"la clé {key} doit exister dans {resxRelativePath}");
			return entry!.Element("value")?.Value ?? string.Empty;
		}

		private static string LocaleResx(string locale) => $"DNNPlatform/App_GlobalResources/GlobalResources.{locale}.resx";

		/// <summary>Corps TERMS du neutre : du dernier <p> avant le marqueur EN jusqu'à la fin.</summary>
		private static string NeutralBody()
		{
			var v = ResxValue(GlobalNeutralResx, "MESSAGE_PORTAL_TERMS.Text");
			var i = v.IndexOf(EnBodyMarker, StringComparison.Ordinal);
			i.Should().BeGreaterThan(0, "le neutre doit porter le marqueur de corps EN");
			return v.Substring(v.LastIndexOf("<p", i, StringComparison.Ordinal));
		}

		/// <summary>Corps TERMS d'une locale traduite : du dernier <p> avant l'ancre localisée jusqu'à la fin.</summary>
		private static string LocaleBody(string locale, string anchor)
		{
			var v = ResxValue(LocaleResx(locale), "MESSAGE_PORTAL_TERMS.Text");
			var i = v.IndexOf(anchor, StringComparison.Ordinal);
			i.Should().BeGreaterThan(0, $"{locale} doit porter le sous-titre localisé « {anchor} »");
			return v.Substring(v.LastIndexOf("<p", i, StringComparison.Ordinal));
		}

		[Theory]
		[InlineData("pt-BR", LocaleBodyAnchor)]
		[InlineData("es-ES", LocaleBodyAnchorEs)]
		public void Terms_BodyReplaced_Localized_StructuralParity(string locale, string anchor)
		{
			var full = ResxValue(LocaleResx(locale), "MESSAGE_PORTAL_TERMS.Text");
			var body = LocaleBody(locale, anchor);
			var neutral = NeutralBody();

			full.Should().NotContain(EnBodyMarker, "le marqueur EN doit avoir disparu du TERMS traduit");
			// résidus EN profonds (le corps entier était EN avant ce grain)
			full.Should().NotContain("is comprised of")
				.And.NotContain("Linked Sites")
				.And.NotContain("your Submission you are granting")
				.And.NotContain("SOLE AND EXCLUSIVE REMEDY")
				.And.NotContain("Web Site", "le corpus traduit dit site/sitio web, jamais « Web Site »");

			// parité structurelle dérivée du neutre vivant
			foreach (var token in new[] { "[Portal:PortalName]", "[Portal:Email]", "[Portal:FooterText]" })
			{
				var occurrences = Regex.Matches(body, Regex.Escape(token)).Count;
				var want = Regex.Matches(neutral, Regex.Escape(token)).Count;
				occurrences.Should().Be(want, $"{locale} body doit porter {token} autant de fois que le neutre");
			}

			Regex.Matches(body, Regex.Escape("class=\"SubHead\"")).Count
				.Should().Be(Regex.Matches(neutral, Regex.Escape("class=\"SubHead\"")).Count, $"{locale} body : sous-titres SubHead paritaires");
			Regex.Matches(body, Regex.Escape("<li>")).Count
				.Should().Be(Regex.Matches(neutral, Regex.Escape("<li>")).Count, $"{locale} body : la liste à puces doit être intégralement traduite");
			body.Should().EndWith("    </div>", "fermeture du bloc");
		}

		[Theory]
		[InlineData("pt-BR")]
		[InlineData("es-ES")]
		public void Terms_LcenHead_Local_And_IdentityPinned(string locale)
		{
			var full = ResxValue(LocaleResx(locale), "MESSAGE_PORTAL_TERMS.Text");
			var i = locale == "pt-BR"
				? full.IndexOf(LocaleBodyAnchor, StringComparison.Ordinal)
				: full.IndexOf(LocaleBodyAnchorEs, StringComparison.Ordinal);
			var head = full.Substring(0, full.LastIndexOf("<p", i, StringComparison.Ordinal));

			head.Should().StartWith("<div align=\"left\">")
				.And.Contain("AVISO LEGAL", "la tête LCEN localisée est préservée telle quelle");
			head.Should().NotContain("LEGAL NOTICE").And.NotContain("SITE IDENTIFICATION");
			// identité légale, épingle x1 (Regex.Escape : « 922020276 » est littéral)
			foreach (var pin in new[] { "ARGUMENTUM SAS", "922020276", "MY INTELLIGENCE AGENCY" })
			{
				Regex.Matches(full, Regex.Escape(pin)).Count.Should().Be(1, $"{locale} : {pin} épingle exactement une fois");
			}
		}

		[Fact]
		public void Terms_PtBr_NoWordSwapResidue()
		{
			var terms = ResxValue(LocaleResx("pt-BR"), "MESSAGE_PORTAL_TERMS.Text");
			var privacy = ResxValue(LocaleResx("pt-BR"), "MESSAGE_PORTAL_PRIVACY.Text");
			// le botch préexistant : mots mutés à ne jamais revoir
			terms.Should().NotContain("responsível").And.NotContain("interative");
			privacy.Should().NotContain("responsível").And.NotContain("interative");
			terms.Should().Contain("responsável", "la graphie portugaise correcte doit porter le corps");
		}

		[Theory]
		[InlineData("pt-BR", "Declaração de Privacidade")]
		[InlineData("es-ES", "Declaración de Privacidad")]
		public void Privacy_Translated_StructuralParity(string locale, string localizedHeading)
		{
			var privacy = ResxValue(LocaleResx(locale), "MESSAGE_PORTAL_PRIVACY.Text");
			var neutral = ResxValue(GlobalNeutralResx, "MESSAGE_PORTAL_PRIVACY.Text");

			privacy.Should().Contain(localizedHeading)
				.And.NotContain("committed to protecting")
				.And.NotContain("A cookie is a text file")
				.And.NotContain("commercially reasonable efforts");

			foreach (var probe in new[] { "[Portal:PortalName]", "mailto:[Portal:Email]", "class=\"SubHead\"", "<p>", "</p>" })
			{
				Regex.Matches(privacy, Regex.Escape(probe)).Count
					.Should().Be(Regex.Matches(neutral, Regex.Escape(probe)).Count,
						$"{locale} PRIVACY doit être structurellement paritaire au neutre sur {probe}");
			}

			privacy.Should().StartWith("<div align=\"left\">").And.EndWith("    </div>");
		}
	}
}
