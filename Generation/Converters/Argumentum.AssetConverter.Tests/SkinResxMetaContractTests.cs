using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Contract of the #1946 follow-up (withdrawal PR): the fr-FR skin resx carries NO
	/// <c>Meta.*</c> keys. French pages already serve their native meta from two distinct
	/// carriers (Tabs.138 on /, PortalLocalization on /Actus) — a resx-side fr override
	/// would rewrite /Actus with the / text. The 7 other cultures keep their Meta.* keys:
	/// their shared pages would otherwise serve French meta. Pins both sides so a future
	/// resx regeneration cannot silently re-introduce the fr override or drop the others.
	/// </summary>
	public class SkinResxMetaContractTests
	{
		private static readonly string ResxDir = Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "Portals", "_default", "Skins", "2shineBS5", "App_LocalResources");

		private static readonly string[] CulturesWithMetaOverride =
		{
			"en-US", "ru-RU", "pt-BR", "es-ES", "zh-CN", "ar-SA", "fa-IR"
		};

		private static string[] DataKeys(string culture) =>
			XDocument.Load(Path.Combine(ResxDir, "2shine." + culture + ".resx"))
				.Root!.Elements("data")
				.Select(e => (string?)e.Attribute("name"))
				.Where(n => n != null)
				.Select(n => n!)
				.ToArray();

		[Fact]
		public void FrFrSkinResxCarriesNoMetaOverrideKeys()
		{
			var metaKeys = DataKeys("fr-FR").Where(k => k.StartsWith("Meta.", StringComparison.Ordinal)).ToArray();
			Assert.True(metaKeys.Length == 0,
				"fr-FR resx must not carry Meta.* keys (fr pages serve native meta from Tabs.138 / PortalLocalization): "
				+ string.Join(", ", metaKeys));
		}

		[Fact]
		public void FrFrSkinResxKeepsTheLocalizedFooterKeys()
		{
			var keys = DataKeys("fr-FR");
			Assert.Contains("Imprint.Text", keys);
			Assert.Contains("Privacy.Text", keys);
		}

		[Fact]
		public void EveryOtherCultureKeepsItsMetaOverrideKeys()
		{
			var missing = CulturesWithMetaOverride
				.Where(c => !DataKeys(c).Contains("Meta.Description"))
				.ToArray();
			Assert.True(missing.Length == 0,
				"cultures missing Meta.Description (their pages would serve French meta): " + string.Join(", ", missing));
		}
	}
}
