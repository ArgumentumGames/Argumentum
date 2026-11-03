using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Contract of the #458 localized-breadcrumb grain: the breadcrumb must serve the SAME label as
	/// the menu in the 7 translated cultures, and French must stay exactly as it was.
	///
	/// Why it exists: the breadcrumb is a compiled DNN skin object whose node label is
	/// <c>Tabs.TabName</c> — the internal French page name — served as-is in every culture.
	/// Measured on preprod 2026-11-03: 4 of 32 (page × culture) nodes equalled their menu label, and
	/// those 4 were the French control. The menu's label comes from the skin resx
	/// (<c>Menu.Tab&lt;id&gt;.Text</c>, #1946), so theme-body.ascx now rewrites the breadcrumb node
	/// from that same key rather than from a second vocabulary that could drift.
	///
	/// Four sides are pinned, because each one alone is satisfiable by a broken tree: the keys exist
	/// in every translated culture (a missing key silently leaves the French name in place — the
	/// exact defect), fr-FR carries none (that absence IS the French control: the DNN page name is
	/// already the French label, so rewriting it must be a no-op), both templates build the key the
	/// same way, and the skin still finds the control and the literal it rewrites after a DNN upgrade.
	/// </summary>
	public class SkinBreadcrumbLabelContractTests
	{
		private static readonly string RepoRoot = TestRepoRoot.Find();

		private static readonly string SkinDir = Path.Combine(
			RepoRoot, "DNNPlatform", "Portals", "_default", "Skins", "2shineBS5");

		/// <summary>The four pages the grain measures (TabID, internal French name).</summary>
		private static readonly (int TabId, string FrenchName)[] Pages =
		{
			(148, "Actus"),
			(150, "Téléchargements"),
			(151, "Amis"),
			(152, "Règles")
		};

		private static readonly string[] TranslatedCultures =
		{
			"en-US", "es-ES", "pt-BR", "ru-RU", "zh-CN", "ar-SA", "fa-IR"
		};

		private static string ResxPath(string culture) =>
			Path.Combine(SkinDir, "App_LocalResources", "2shine." + culture + ".resx");

		private static string MenuKey(int tabId) => "Menu.Tab" + tabId + ".Text";

		/// <summary>Value of a resx key, or null when the key is absent (absent ≠ empty).</summary>
		private static string? Value(string file, string key)
		{
			var data = XDocument.Load(file).Root!.Elements("data")
				.FirstOrDefault(e => (string?)e.Attribute("name") == key);
			return data == null ? null : (string?)data.Element("value");
		}

		[Fact]
		public void EveryTranslatedCultureCarriesTheFourMenuLabels()
		{
			var missing = TranslatedCultures
				.SelectMany(c => Pages
					.Where(p => string.IsNullOrWhiteSpace(Value(ResxPath(c), MenuKey(p.TabId))))
					.Select(p => c + ":Menu.Tab" + p.TabId + ".Text"))
				.ToArray();

			Assert.True(missing.Length == 0,
				"culture files missing the label the breadcrumb now reads (the node would keep serving "
				+ "the French page name): " + string.Join(", ", missing));

			// Anti-vacuity: the loop above asserts an absence, which a wrong path also produces.
			var found = TranslatedCultures.Sum(c => Pages.Count(p => Value(ResxPath(c), MenuKey(p.TabId)) != null));
			Assert.Equal(TranslatedCultures.Length * Pages.Length, found);
		}

		[Fact]
		public void FrenchCarriesNoMenuLabelSoThePageNameIsTheFrenchControl()
		{
			var file = ResxPath("fr-FR");
			Assert.True(File.Exists(file), "fr-FR culture file missing: " + file);

			// The file is read at all — otherwise "no key" would also come from a wrong path.
			Assert.NotEmpty(XDocument.Load(file).Root!.Elements("data"));

			foreach (var page in Pages)
			{
				Assert.Null(Value(file, MenuKey(page.TabId)));
			}
		}

		[Fact]
		public void BothTemplatesBuildTheMenuKeyTheSameWay()
		{
			var body = File.ReadAllText(Path.Combine(SkinDir, "controls", "theme-body.ascx"));
			var menu = File.ReadAllText(Path.Combine(SkinDir, "nav", "main", "template.cshtml"));

			// The menu template's key shape …
			Assert.Contains("var key = \"Menu.Tab\" + tabId + \".\" + kind;", menu);
			// … and the breadcrumb's, built from the tab id the DNN control reports.
			Assert.Contains("CultureString(\"Menu.Tab\" + tab.TabID + \".Text\")", body);

			// Both read the same file set through the explicit-culture-file path (#1962).
			Assert.Contains("CultureString", body);
			Assert.Contains("2shine", menu);
		}

		[Fact]
		public void TheSkinStillFindsTheControlAndTheLiteralItRewrites()
		{
			var body = File.ReadAllText(Path.Combine(SkinDir, "controls", "theme-body.ascx"));
			Assert.Contains("ID=\"SkinBreadcrumb\"", body);
			Assert.Contains("FindControl(\"SkinBreadcrumb\")", body);
			Assert.Contains("LocalizeBreadcrumb();", body);

			// The literal id is DNN's, not ours: a DNN upgrade that renames it would leave the
			// rewrite silently doing nothing, which reads exactly like "the defect is gone".
			var dnnControl = Path.Combine(RepoRoot, "DNNPlatform", "admin", "Skins", "breadcrumb.ascx");
			Assert.True(File.Exists(dnnControl), "DNN breadcrumb skin object missing: " + dnnControl);
			Assert.Contains("FindControl(\"lblBreadCrumb\")", body);
			Assert.Contains("id=\"lblBreadCrumb\"", File.ReadAllText(dnnControl), StringComparison.OrdinalIgnoreCase);
		}
	}
}
