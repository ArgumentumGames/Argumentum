using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Contract of the interior page-title fix (#458): the breadcrumb <c>TabName</c> is a single
	/// French value (the Tabs table has no per-culture title column), so the browser tab of an
	/// interior page showed a French word on the 7 non-fr cultures. The fix resolves each
	/// breadcrumb entry through the <c>Menu.Tab&lt;Id&gt;.Text</c> keys of the culture resx —
	/// the same keys the nav templates already use — and falls back to <c>TabName</c> when the
	/// key is absent. These tests pin the data the fix consumes: fr-FR and the invariant resx
	/// carry NO Menu.Tab keys (French titles stay TabName), each non-fr culture carries a label
	/// for every interior tab of the current menu, and the title control actually consults the
	/// culture resx (a revert of the control goes red here). A future tab added without keys
	/// degrades gracefully to TabName — only today's translated surface is pinned.
	/// </summary>
	public class SkinTitleLocalizationContractTests
	{
		private static readonly string ResxDir = Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "Portals", "_default", "Skins", "2shineBS5", "App_LocalResources");

		private static readonly string TitleControlPath = Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "Portals", "_default", "Skins", "2shineBS5", "controls", "optimize-page-title.ascx");

		private static readonly string[] NonFrCultures =
		{
			"en-US", "ru-RU", "pt-BR", "es-ES", "zh-CN", "ar-SA", "fa-IR"
		};

		/// <summary>Interior tabs of the current menu (138 Accueil carries no key by design —
		/// the home title is the brand name, registry Q-2026-10-24-1).</summary>
		private static readonly int[] InteriorTabIds = { 148, 149, 150, 151, 152, 169, 170, 171 };

		private static string[] DataKeys(string culture) =>
			XDocument.Load(Path.Combine(ResxDir, "2shine." + culture + ".resx"))
				.Root!.Elements("data")
				.Select(e => (string?)e.Attribute("name"))
				.Where(n => n != null)
				.Select(n => n!)
				.ToArray();

		[Fact]
		public void FrAndInvariantResxCarryNoMenuTabKeys()
		{
			foreach (var resx in new[] { "fr-FR", "" })
			{
				var fileName = resx.Length == 0 ? "2shine.resx" : "2shine." + resx + ".resx";
				var menuKeys = XDocument.Load(Path.Combine(ResxDir, fileName))
					.Root!.Elements("data")
					.Select(e => (string?)e.Attribute("name"))
					.Where(n => n != null && n.StartsWith("Menu.Tab", StringComparison.Ordinal))
					.Select(n => n!)
					.ToArray();
				Assert.True(menuKeys.Length == 0,
					fileName + " must not carry Menu.Tab* keys (French titles are the native TabName): "
					+ string.Join(", ", menuKeys));
			}
		}

		[Fact]
		public void EveryNonFrCultureCarriesTheInteriorTabLabels()
		{
			var missing = NonFrCultures
				.SelectMany(c => InteriorTabIds.Select(id => (culture: c, key: "Menu.Tab" + id + ".Text")))
				.Where(pair => !DataKeys(pair.culture).Contains(pair.key))
				.Select(pair => pair.culture + ":" + pair.key)
				.ToArray();
			Assert.True(missing.Length == 0,
				"culture resx missing a Menu.Tab<Id>.Text label (that page title would fall back to the French TabName): "
				+ string.Join(", ", missing));
		}

		[Fact]
		public void TitleControlResolvesTheTabLabelFromTheCultureResx()
		{
			var source = File.ReadAllText(TitleControlPath);
			Assert.Contains("Menu.Tab", source);
			Assert.Contains("App_LocalResources/2shine", source);
			Assert.DoesNotContain(".Select(p => p.TabName)", source);
		}
	}
}
