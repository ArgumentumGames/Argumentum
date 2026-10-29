using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Argumentum.AssetConverter.Dnn2sxc;
using Argumentum.AssetConverter.Entities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #136 partie 1, PR 2 — contrat de l'export Dnn2sxc VIRTUES. Première taxonomie <b>sans
	/// fixture 2021</b> : le bootstrap de racine (PR 1) y est le chemin nominal, pas un cas de
	/// test. 223 rangées (re-mesuré 15/09), 5 cultures (fr/en/ru/pt/es — l'entité
	/// <c>ArgumentVirtue</c> porte les 5 colonnes).
	/// </summary>
	/// <remarks>
	/// <para><b>Projection</b> : le dialecte XML n'a que Name/Description/Example comme champs
	/// textuels — Title → Name, Description → Description, Remark → Example (remarks
	/// <c>Dnn2sxcConfig</c>).</para>
	/// <para><b>Ce que cette suite ne prouve pas</b> : l'acceptation par 2sxc v21 sur le portail
	/// (partie 2 de #136).</para>
	/// </remarks>
	public class Dnn2sxcVirtuesExportContractTests
	{
		private readonly ITestOutputHelper _output;

		public Dnn2sxcVirtuesExportContractTests(ITestOutputHelper output)
		{
			_output = output;
		}

		private static string Repo => TestRepoRoot.Find();

		private static string CsvPath => Path.Combine(Repo,
			"Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");

		private static string MasterCulture => new Dnn2sxcConfig().Virtues.Cultures[0].Culture;

		private static string Val(XElement entity, string childName)
		{
			return entity.Element(childName)?.Value ?? string.Empty;
		}

		/// <summary>Config Virtues seule (Fallacies désactivée), export vers un fichier temporaire.</summary>
		private static Dnn2sxcConfig NewConfig(string exportPath)
		{
			return new Dnn2sxcConfig
			{
				Fallacies = new Dnn2sxcTaxonomyExportConfig(), // désactivée : CsvPath vide
				Scenarii = new Dnn2sxcTaxonomyExportConfig(),  // désactivée : CsvPath vide
				Rules = new Dnn2sxcTaxonomyExportConfig(),     // désactivée : CsvPath vide (PR 4)
				Virtues = new Dnn2sxcTaxonomyExportConfig
				{
					EntityTypeName = "Virtue",
					RootName = "Virtues",
					ContextKey = "virtue",
					CsvPath = CsvPath,
					ImportPath = "", // pas de fixture d'époque — bootstrap de racine
					ExportPath = exportPath,
					Cultures = new Dnn2sxcConfig().Virtues.Cultures,
				},
			};
		}

		private static string TempExportPath()
		{
			return Path.Combine(Path.GetTempPath(), $"dnn2sxc-virtues-{Guid.NewGuid():N}.xml");
		}

		private static System.Collections.Generic.List<XElement> ExportEntities(XDocument document)
		{
			return document.Root!.Elements("Entity").ToList();
		}

		[Fact]
		public void Regenerated_Export_Is_Valid_Xml_Even_Over_A_Longer_Previous_File()
		{
			var exportPath = TempExportPath();
			try
			{
				File.WriteAllText(exportPath, new string('x', 2_000_000)); // contrôle inverse troncature

				NewConfig(exportPath).Apply();

				var loaded = XDocument.Load(exportPath); // lève si invalide — c'est le fait
				loaded.Root!.Name.LocalName.Should().Be("SexyContentData");
			}
			finally
			{
				File.Delete(exportPath);
			}
		}

		[Fact]
		public void Every_Culture_Covers_Every_Csv_Row_Exactly_No_Fixture()
		{
			var exportPath = TempExportPath();
			try
			{
				NewConfig(exportPath).Apply();

				var csvRowCount = ArgumentVirtue.Load(CsvPath).Count;
				var byCulture = ExportEntities(XDocument.Load(exportPath))
					.GroupBy(e => Val(e, "Language"))
					.ToDictionary(g => g.Key, g => g.ToList());

				var scopeCultures = new[] { "en-US", "fr-FR", "ru-RU", "pt-BR", "es-ES", "ar-SA", "fa-IR", "zh-CN" };

				_output.WriteLine($"CSV: {csvRowCount} rangées ; cultures: {string.Join(", ", byCulture.Select(p => $"{p.Key}={p.Value.Count}"))}");

				byCulture.Keys.Should().BeEquivalentTo(scopeCultures,
					"aucune culture ne préexiste (pas de fixture) : exactement les 8 du corpus");
				foreach (var culture in scopeCultures)
				{
					// Sans fixture, chaque culture est purement CSV-déterminée : ÉGALITÉ stricte,
					// pas un plancher comme pour les cultures héritées du fixture Fallacies.
					byCulture[culture].Count.Should().Be(csvRowCount,
						$"{culture} : une entité par rangée CSV, racine comprise");
				}
			}
			finally
			{
				File.Delete(exportPath);
			}
		}

		[Fact]
		public void Hierarchy_Parents_All_Resolve_Inside_The_Export()
		{
			var exportPath = TempExportPath();
			try
			{
				NewConfig(exportPath).Apply();

				var entities = ExportEntities(XDocument.Load(exportPath));
				var guids = entities.Select(e => Val(e, "Guid")).ToHashSet();

				var unresolved = entities
					.Select(e => Val(e, "Parent"))
					.Where(p => !string.IsNullOrEmpty(p) && !guids.Contains(p))
					.ToList();

				unresolved.Should().BeEmpty(
					"un Parent qui ne pointe aucun Guid de l'export casse l'arbre à l'import 2sxc — {0} non résolu(s)",
					string.Join(", ", unresolved.Take(3)));
			}
			finally
			{
				File.Delete(exportPath);
			}
		}

		[Fact]
		public void Two_Runs_Produce_A_Byte_Identical_Export()
		{
			var first = TempExportPath();
			var second = TempExportPath();
			try
			{
				NewConfig(first).Apply();
				NewConfig(second).Apply();

				File.ReadAllBytes(first).Should().Equal(File.ReadAllBytes(second),
					"Guid déterministes dérivés du chemin — l'export committé doit être reproductible");
			}
			finally
			{
				File.Delete(first);
				File.Delete(second);
			}
		}

		[Fact]
		public void The_Root_Is_Bootstrapped_From_The_Csv_Row_Zero()
		{
			// Le chemin nominal de cette taxonomie : pas de fixture, la racine vient du CSV.
			var exportPath = TempExportPath();
			try
			{
				NewConfig(exportPath).Apply();

				var entities = ExportEntities(XDocument.Load(exportPath));
				var roots = entities.Where(e => Val(e, "Language") == MasterCulture
					&& string.IsNullOrEmpty(Val(e, "Parent"))).ToList();

				roots.Should().HaveCount(1, "exactement une racine sans parent (rangée CSV 0)");
				var rootRow = ArgumentVirtue.Load(CsvPath).First(v => v.Path == "0");
				Val(roots[0], "Name").Should().Be(rootRow.TitleEn,
					"la racine porte le titre maître de la rangée 0 (« Valid argument » mesuré dans le corpus)");
			}
			finally
			{
				File.Delete(exportPath);
			}
		}

		[Fact]
		public void Every_Entity_Of_Every_Culture_Carries_A_Localized_Name()
		{
			var exportPath = TempExportPath();
			try
			{
				NewConfig(exportPath).Apply();

				var entities = ExportEntities(XDocument.Load(exportPath));

				entities.Where(e => string.IsNullOrEmpty(Val(e, "Name")))
					.Should().BeEmpty("chaque entité, chaque culture, porte son Name localisé");

				// Témoin verbatim du corpus (mesuré avant la PR) : la racine française.
				entities.Should().Contain(e => Val(e, "Language") == "fr-FR" && Val(e, "Name") == "Argument valable",
					"la racine fr-FR porte le titre français de la rangée 0 du corpus");
			}
			finally
			{
				File.Delete(exportPath);
			}
		}
	}
}
