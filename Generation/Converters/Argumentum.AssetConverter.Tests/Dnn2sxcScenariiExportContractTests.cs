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
	/// #136 partie 1, PR 3 — contrat de l'export Dnn2sxc SCENARII. Spécificité mesurée : le CSV ne
	/// porte que les feuilles — 167 chemins de profondeur 3, tous distincts, <b>sans rangées « 1 »
	/// ni « 1.1 »</b> — alors que le moteur résout chaque parent : les 7 catégories et 21
	/// sous-catégories sont synthétisées depuis les colonnes catégorie/sous-catégorie de la
	/// première rangée de chaque groupe (<see cref="Dnn2sxcSyntheticGroupRow"/>). Total attendu
	/// par culture : 1 racine + 7 + 21 + 167 = 196.
	/// </summary>
	/// <remarks>
	/// <para><b>Projection</b> : titre → Name, contexte → Description, enjeu → Example. 5 cultures
	/// (fr/en/ru/pt/es — l'entité <c>Scenario</c> n'expose pas d'ar/fa/zh).</para>
	/// <para><b>Ce que cette suite ne prouve pas</b> : l'acceptation par 2sxc v21 sur le portail
	/// (partie 2 de #136), ni la fidélité des noms de sous-catégorie au-delà de la première
	/// rangée de chaque groupe (choix de synthèse documenté, pas une mesure ligne à ligne).</para>
	/// </remarks>
	public class Dnn2sxcScenariiExportContractTests
	{
		private readonly ITestOutputHelper _output;

		public Dnn2sxcScenariiExportContractTests(ITestOutputHelper output)
		{
			_output = output;
		}

		private static string Repo => TestRepoRoot.Find();

		private static string CsvPath => Path.Combine(Repo,
			"Cards", "Scenarii", "Argumentum Scenarii - Cards.csv");

		private static string MasterCulture => new Dnn2sxcConfig().Scenarii.Cultures[0].Culture;

		private static string Val(XElement entity, string childName)
		{
			return entity.Element(childName)?.Value ?? string.Empty;
		}

		/// <summary>Config Scenarii seule (Fallacies/Virtues désactivées), export temporaire.</summary>
		private static Dnn2sxcConfig NewConfig(string exportPath)
		{
			return new Dnn2sxcConfig
			{
				Fallacies = new Dnn2sxcTaxonomyExportConfig(), // désactivée : CsvPath vide
				Virtues = new Dnn2sxcTaxonomyExportConfig(),   // désactivée : CsvPath vide
				Rules = new Dnn2sxcTaxonomyExportConfig(),     // désactivée : CsvPath vide (PR 4)
				Scenarii = new Dnn2sxcTaxonomyExportConfig
				{
					EntityTypeName = "Scenario",
					RootName = "Scenarii",
					ContextKey = "scenario",
					CsvPath = CsvPath,
					ImportPath = "", // pas de fixture d'époque — bootstrap de racine
					ExportPath = exportPath,
					Cultures = new Dnn2sxcConfig().Scenarii.Cultures,
				},
			};
		}

		private static string TempExportPath()
		{
			return Path.Combine(Path.GetTempPath(), $"dnn2sxc-scenarii-{Guid.NewGuid():N}.xml");
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
		public void Every_Culture_Carries_Root_Plus_Synthetic_Groups_Plus_Every_Leaf()
		{
			var exportPath = TempExportPath();
			try
			{
				NewConfig(exportPath).Apply();

				var leaves = Scenario.Load(CsvPath).Count;
				var categories = Scenario.Load(CsvPath).Select(s => s.path.Split('.')[0]).Distinct().Count();
				var subcategories = Scenario.Load(CsvPath)
					.Select(s => s.path.Substring(0, s.path.LastIndexOf('.'))).Distinct().Count();
				var expectedPerCulture = 1 + categories + subcategories + leaves;

				var byCulture = ExportEntities(XDocument.Load(exportPath))
					.GroupBy(e => Val(e, "Language"))
					.ToDictionary(g => g.Key, g => g.ToList());

				var scopeCultures = new[] { "en-US", "fr-FR", "ru-RU", "pt-BR", "es-ES", "ar-SA", "fa-IR", "zh-CN" };

				_output.WriteLine($"CSV: {leaves} feuilles, {categories} catégories, {subcategories} sous-catégories ; cultures: {string.Join(", ", byCulture.Select(p => $"{p.Key}={p.Value.Count}"))}");

				byCulture.Keys.Should().BeEquivalentTo(scopeCultures,
					"aucune culture ne préexiste (pas de fixture) : exactement les 8 du corpus");
				foreach (var culture in scopeCultures)
				{
					byCulture[culture].Count.Should().Be(expectedPerCulture,
						$"{culture} : racine + {categories} catégories + {subcategories} sous-catégories synthétisées + {leaves} feuilles");
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
			// LA garde du choix de synthèse : sans les rangées de groupe, « 1.1.1 » chercherait
			// son parent « 1.1 » inexistant et le moteur lèverait KeyNotFoundException.
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
					"Guid déterministes + synthèse déterministe (ordre CSV, première rangée de chaque groupe)");
			}
			finally
			{
				File.Delete(first);
				File.Delete(second);
			}
		}

		[Fact]
		public void Synthetic_Categories_Carry_Their_Localized_Names()
		{
			var exportPath = TempExportPath();
			try
			{
				NewConfig(exportPath).Apply();

				var entities = ExportEntities(XDocument.Load(exportPath));
				var masterRoots = entities.Where(e => Val(e, "Language") == MasterCulture
					&& string.IsNullOrEmpty(Val(e, "Parent"))).ToList();
				masterRoots.Should().HaveCount(1, "une seule racine, nommée par config (le CSV Scenarii n'a pas de rangée 0)");
				Val(masterRoots[0], "Name").Should().Be(new Dnn2sxcConfig().Scenarii.RootName);

				// Témoins verbatim du corpus (mesurés avant la PR) : la catégorie 1 « histoire » et
				// la première feuille « La mère de César et Cléopâtre ».
				var fr = entities.Where(e => Val(e, "Language") == "fr-FR").ToList();
				fr.Should().Contain(e => Val(e, "Name") == "histoire",
					"la catégorie de niveau 1 porte le nom français de la colonne catégorie");
				fr.Should().Contain(e => Val(e, "Name") == "La mère de César et Cléopâtre",
					"témoin feuille verbatim du corpus");
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
			}
			finally
			{
				File.Delete(exportPath);
			}
		}
	}
}
