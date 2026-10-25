using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Contract of the #458 login/logoff tooltip grain: the icon-only links in
	/// theme-body.ascx and Landingpage.ascx (an svg with no visible text — the tooltip is
	/// the only accessible label) read <c>Login.Tooltip</c> / <c>Logoff.Tooltip</c> from the
	/// skin resx instead of the hardcoded <c>Title="Login"</c>/<c>Title="Logoff"</c> that
	/// served English on all 8 cultures. Pins three sides so a regeneration or a skin update
	/// cannot silently regress: every culture file carries both keys with a non-English value
	/// (except en-US), the base file keeps the English fallback (the base is what the
	/// culture-name overload normalizes to for the unregistered en-US — measured
	/// 2026-10-24, see SkinResxMetaContractTests), and the markup holds no hardcoded
	/// tooltip attribute anymore.
	/// </summary>
	public class SkinLoginTooltipContractTests
	{
		private static readonly string SkinDir = Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "Portals", "_default", "Skins", "2shineBS5");

		private static readonly string[] NonEnglishCultures =
		{
			"fr-FR", "es-ES", "pt-BR", "ru-RU", "zh-CN", "ar-SA", "fa-IR"
		};

		private static readonly string[] MarkupFiles =
		{
			Path.Combine(SkinDir, "controls", "theme-body.ascx"),
			Path.Combine(SkinDir, "Landingpage.ascx")
		};

		private static string ResxPath(string culture) =>
			Path.Combine(SkinDir, "App_LocalResources", "2shine." + culture + ".resx");

		private static string? TooltipValue(string file, string key)
		{
			var data = XDocument.Load(file).Root!.Elements("data")
				.FirstOrDefault(e => (string?)e.Attribute("name") == key);
			return data == null ? null : (string?)data.Element("value") ?? "";
		}

		[Fact]
		public void EveryCultureFileCarriesBothTooltipKeys()
		{
			var missing = new[] { "en-US" }.Concat(NonEnglishCultures)
				.SelectMany(c => new[] { "Login.Tooltip", "Logoff.Tooltip" }
					.Where(k => string.IsNullOrWhiteSpace(TooltipValue(ResxPath(c), k)))
					.Select(k => c + ":" + k))
				.ToArray();
			Assert.True(missing.Length == 0,
				"culture files missing a tooltip key (the icon-only link would serve the key name): "
				+ string.Join(", ", missing));
		}

		[Fact]
		public void BaseFileCarriesTheEnglishFallback()
		{
			var resx = Path.Combine(SkinDir, "App_LocalResources", "2shine.resx");
			Assert.Equal("Login", TooltipValue(resx, "Login.Tooltip"));
			Assert.Equal("Logoff", TooltipValue(resx, "Logoff.Tooltip"));
		}

		[Fact]
		public void NonEnglishCulturesDoNotServeTheEnglishValue()
		{
			var english = new[] { "Login", "Logoff" };
			var offenders = NonEnglishCultures
				.SelectMany(c => new[] { "Login.Tooltip", "Logoff.Tooltip" }
					.Where(k => TooltipValue(ResxPath(c), k) is { } v && english.Contains(v))
					.Select(k => c + ":" + k))
				.ToArray();
			Assert.True(offenders.Length == 0,
				"culture files whose tooltip equals the English string (a copy-paste fallback, not a localization): "
				+ string.Join(", ", offenders));
		}

		[Fact]
		public void MarkupHasNoHardcodedTooltipAndUsesTheResxKeys()
		{
			foreach (var file in MarkupFiles)
			{
				var markup = File.ReadAllText(file);
				Assert.DoesNotContain("Title=\"Logoff\"", markup);
				Assert.DoesNotContain("Title=\"Login\"", markup);
				Assert.Contains("LocalizeString(\"Login.Tooltip\")", markup);
				Assert.Contains("LocalizeString(\"Logoff.Tooltip\")", markup);
			}
		}
	}
}
