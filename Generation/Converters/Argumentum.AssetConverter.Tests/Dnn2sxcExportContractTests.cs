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
	/// #136 partie 1 — contrat de l'export Dnn2sxc FALLACIES. Le mode n'avait pas tourné depuis
	/// 2023 et l'export committé de l'époque était un XML <b>invalide</b> (mesuré : la sérialisation
	/// s'arrête ligne 15357 sur <c>&lt;/SexyContentData&gt;</c> suivie des octets résiduels du
	/// fichier précédent plus long — le défaut <c>File.OpenWrite</c> ne tronque pas). Chaque fait
	/// ici rejoue l'export sur le corpus courant, dans un fichier temporaire — l'export du dépôt
	/// n'est écrit par aucun test.
	/// </summary>
	/// <remarks>
	/// <para><b>Ce que cette suite prouve</b> : l'export est un XML valide même en écrasant un
	/// fichier précédent plus long (la corruption de 2023 rejouée en contrôle inverse) ; les 4
	/// cultures du scope #136 (fr/en/ru/pt) couvrent chaque rangée du CSV ; la hiérarchie se
	/// referme (tout <c>Parent</c> pointe un Guid présent) ; deux exécutions produisent un export
	/// byte-identique ; sans fixture 2021, la racine est bootstrappée au lieu de faire NRE ; la
	/// régénération reste byte-identique à l'export <b>committé</b> (la garde du moteur partagé :
	/// toute dérive du port de <c>Apply</c> vers <c>Dnn2sxcTaxonomyExporter</c> rougit ici).</para>
	/// <para><b>Ce qu'elle ne prouve pas</b> : que 2sxc v21 accepte l'import sur le portail —
	/// c'est la partie 2 de #136, derrière le recyclage préprod.</para>
	/// </remarks>
	public class Dnn2sxcExportContractTests
	{
		private readonly ITestOutputHelper _output;

		public Dnn2sxcExportContractTests(ITestOutputHelper output)
		{
			_output = output;
		}

		private static string Repo => TestRepoRoot.Find();

		private static string CsvPath => Path.Combine(Repo,
			"Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static string ImportFixturePath => Path.Combine(Repo,
			"Generation", "Converters", "Argumentum.AssetConverter", "Data", "Dnn2sxc",
			"2sxc Fallacy  Data 20210212131131.xml");

		private static string CommittedExportPath => Path.Combine(Repo,
			"Generation", "Converters", "Argumentum.AssetConverter", "Data", "Dnn2sxc",
			"2sxcContentExport_Argumentum_Fallacies_0.0.1.xml");

		private static string MasterCulture => new Dnn2sxcConfig().Fallacies.Cultures[0].Culture;

		/// <summary>Valeur textuelle d'un enfant, vide si absent (le XML sérialise les null en éléments vides).</summary>
		private static string Val(XElement entity, string childName)
		{
			return entity.Element(childName)?.Value ?? string.Empty;
		}

		/// <summary>Config Fallacies seule (Virtues désactivée), export vers un fichier temporaire.</summary>
		private static Dnn2sxcConfig NewConfig(string exportPath, string? importPath = null)
		{
			return new Dnn2sxcConfig
			{
				Virtues = new Dnn2sxcTaxonomyExportConfig(), // désactivée : CsvPath vide
				Scenarii = new Dnn2sxcTaxonomyExportConfig(), // désactivée : CsvPath vide
				Rules = new Dnn2sxcTaxonomyExportConfig(),    // désactivée : CsvPath vide (PR 4)
				Fallacies = new Dnn2sxcTaxonomyExportConfig
				{
					EntityTypeName = "Fallacy",
					RootName = "Fallacies",
					ContextKey = "fallacy",
					CsvPath = CsvPath,
					ImportPath = importPath ?? ImportFixturePath,
					ExportPath = exportPath,
					Cultures = new Dnn2sxcConfig().Fallacies.Cultures,
				},
			};
		}

		private static string TempExportPath()
		{
			return Path.Combine(Path.GetTempPath(), $"dnn2sxc-export-{Guid.NewGuid():N}.xml");
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
				// Contrôle inverse de la corruption de 2023 : on pré-remplit la cible avec un fichier
				// PLUS LONG que l'export attendu — l'ancien File.OpenWrite y laissait sa queue.
				File.WriteAllText(exportPath, new string('x', 2_000_000));

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
		public void Every_Culture_Of_The_Scope_Covers_Every_Csv_Row()
		{
			var exportPath = TempExportPath();
			try
			{
				NewConfig(exportPath).Apply();

				var csvRowCount = Fallacy.Load(CsvPath).Count;
				var byCulture = ExportEntities(XDocument.Load(exportPath))
					.GroupBy(e => Val(e, "Language"))
					.ToDictionary(g => g.Key, g => g.ToList());

				// #136 (b) : es/ar/fa/zh rejoignent ru/pt — toutes absentes du fixture 2021, donc
				// purement CSV-déterminées (contrairement à fr-FR, héritée du fixture).
				var newCultures = new[] { "ru-RU", "pt-BR", "es-ES", "ar-SA", "fa-IR", "zh-CN" };

				_output.WriteLine($"CSV: {csvRowCount} rangées ; cultures: {string.Join(", ", byCulture.Select(p => $"{p.Key}={p.Value.Count}"))}");

				// Cultures nouvelles (ru/pt) : exactement une entité par rangée CSV (racine comprise —
				// la rangée path "0" en fait partie). Les cultures du fixture peuvent porter des
				// chemins retirés du corpus courant ⇒ plancher, pas égalité.
				foreach (var culture in newCultures)
				{
					byCulture.Should().ContainKey(culture,
						$"le scope #136 (fr/en/ru/pt) exige une entrée {culture}");
					byCulture[culture].Count.Should().Be(csvRowCount,
						$"{culture} est neuve (absente du fixture 2021) : une entité par rangée CSV attendue, racine comprise");
				}
				byCulture[MasterCulture].Count.Should().BeGreaterThanOrEqualTo(csvRowCount,
					"la culture maîtresse couvre au minimum chaque rangée du CSV courant");
				byCulture.Should().ContainKey("fr-FR", "le fixture 2021 portait des traductions fr");
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
					"les Guid des chemins nouveaux sont déterministes (dérivés du chemin) — "
					+ "sinon chaque regen réécrit l'export committé et un ré-import 2sxc duplique les entités");
			}
			finally
			{
				File.Delete(first);
				File.Delete(second);
			}
		}

		[Fact]
		public void Regeneration_Matches_The_Committed_Export_Byte_For_Byte()
		{
			// La garde du port moteur (PR 2) : le corps d'Apply a déménagé vers
			// Dnn2sxcTaxonomyExporter sans changement de comportement voulu — la preuve est que la
			// régénération reproduit exactement l'artefact committé (produit par l'ancien corps).
			var exportPath = TempExportPath();
			try
			{
				NewConfig(exportPath).Apply();

				File.ReadAllBytes(exportPath).Should().Equal(File.ReadAllBytes(CommittedExportPath),
					"le port du moteur vers Dnn2sxcTaxonomyExporter doit être un déplacement fidèle — "
					+ "toute différence ici est une dérive de comportement, pas un détail");
			}
			finally
			{
				File.Delete(exportPath);
			}
		}

		[Fact]
		public void Without_The_2021_Import_Fixture_The_Root_Is_Bootstrapped_Not_An_Nre()
		{
			var exportPath = TempExportPath();
			try
			{
				// Pré-requis des taxonomies sans fixture (Virtues/Scenarii/Rules, PR 2-4 de #136) :
				// l'ancien code NRE sur root.Guid dès le premier nœud de famille.
				NewConfig(exportPath, importPath: Path.Combine(Path.GetTempPath(), "dnn2sxc-fixture-absent.xml")).Apply();

				var entities = ExportEntities(XDocument.Load(exportPath));
				var roots = entities.Where(e => Val(e, "Language") == MasterCulture
					&& string.IsNullOrEmpty(Val(e, "Parent"))).ToList();

				roots.Should().HaveCount(1, "exactement une racine sans parent, bootstrappée depuis la rangée CSV 0");
				// Le nom final vient de la rangée CSV 0 (le bootstrap crée la racine, la boucle CSV la
				// met ensuite à jour comme n'importe quelle rangée — même comportement qu'avec le fixture).
				var rootRow = Fallacy.Load(CsvPath).First(f => f.Path == "0");
				Val(roots[0], "Name").Should().Be(rootRow.TextEn,
					"la racine reçoit le titre maître de la rangée CSV 0, comme toute autre rangée");
			}
			finally
			{
				File.Delete(exportPath);
			}
		}

		[Fact]
		public void Csv_Is_Authoritative_On_A_Translation_The_2021_Fixture_Also_Carries()
		{
			// Constat ai-01 sur #136 : la description fr-FR de PK 2 « Argument bâclé » venait du
			// fixture 2021, pas du CSV. Le fixture ne porte que 3 entités par langue (racine + 2) —
			// c'est exactement là que le défaut vivait : les autres traductions naissaient du CSV.
			// ⚠️ La garde byte-à-byte ne suffit pas seule ici : sa référence EST le fichier committé,
			// qu'une régénération réécrit — elle adopte silencieusement un export revenu au fixture.
			// Ce fait-ci nomme l'intention et rougit si le CSV cesse d'être prioritaire.
			var exportPath = TempExportPath();
			try
			{
				NewConfig(exportPath).Apply();

				var row = Fallacy.Load(CsvPath).Single(f => f.TextFr == "Argument bâclé");
				var entities = ExportEntities(XDocument.Load(exportPath));

				var master = entities.Single(e => Val(e, "Language") == MasterCulture
					&& Val(e, "Name") == row.TextEn);
				var translation = entities.Single(e => Val(e, "Guid") == Val(master, "Guid")
					&& Val(e, "Language") == "fr-FR");

				_output.WriteLine($"CSV    : {row.DescFr}");
				_output.WriteLine($"EXPORT : {Val(translation, "Description")}");

				Val(translation, "Name").Should().Be(row.TextFr);
				Val(translation, "Description").Should().Be(row.DescFr,
					"desc_fr fait foi — la description authored du fixture 2021 ne doit plus être servie");
				Val(translation, "Example").Should().Be(row.ExampleFr,
					"example_fr fait foi de même (le fixture portait « [] », le marqueur de vide 2sxc)");
			}
			finally
			{
				File.Delete(exportPath);
			}
		}

		[Fact]
		public void Every_Non_Empty_Csv_Value_Reaches_Its_Translation()
		{
			// Contrat GÉNÉRAL, sans table de correspondance : les titres ne sont PAS injectifs dans
			// ce corpus (mesuré : 89 doublons text_en, 85 text_fr), donc on compare des MULTISETS de
			// triplets (Name, Description, Example) — la répartition par Guid n'est pas ce qui est
			// asserté, le contenu l'est. Le nom de propriété est DÉRIVÉ de l'expression de config
			// (« {fallacy.DescFr} » → DescFr) : une culture ajoutée ou re-câblée est couverte sans
			// toucher à ce test, et rien de la logique d'interpolation n'est recopié ici.
			var exportPath = TempExportPath();
			try
			{
				NewConfig(exportPath).Apply();

				var rows = Fallacy.Load(CsvPath).ToList();
				var entities = ExportEntities(XDocument.Load(exportPath));

				foreach (var culture in new Dnn2sxcConfig().Fallacies.Cultures)
				{
					var expected = rows
						.Select(f => Triplet(
							CsvValue(f, culture.TitleExpression),
							CsvValue(f, culture.DescriptionExpression),
							CsvValue(f, culture.ExampleExpression)))
						.OrderBy(t => t, StringComparer.Ordinal)
						.ToList();

					var actual = entities
						.Where(e => Val(e, "Language") == culture.Culture)
						.Select(e => Triplet(Val(e, "Name"), Val(e, "Description"), Val(e, "Example")))
						.OrderBy(t => t, StringComparer.Ordinal)
						.ToList();

					_output.WriteLine($"{culture.Culture}: CSV={expected.Count} export={actual.Count}");

					actual.Should().Equal(expected,
						$"chaque valeur portée par le CSV doit atteindre la traduction {culture.Culture} "
						+ "(le fixture 2021 n'est qu'un repli sur un champ muet)");
				}
			}
			finally
			{
				File.Delete(exportPath);
			}
		}

		private static string Triplet(string name, string description, string example)
		{
			return $"{name}\u0001{description}\u0001{example}";
		}

		/// <summary>Nom de propriété lu dans l'expression de config (« {fallacy.DescFr} » → « DescFr »).</summary>
		private static string CsvValue(Fallacy fallacy, string expression)
		{
			var dot = expression.IndexOf('.');
			var close = expression.IndexOf('}');
			expression.Should().Contain("{fallacy.", "expression de culture attendue sous la forme {fallacy.Xxx}");
			dot.Should().BeGreaterThan(0);
			var property = expression.Substring(dot + 1, close - dot - 1);
			var info = typeof(Fallacy).GetProperty(property);
			info.Should().NotBeNull($"l'expression de config nomme la propriété {property} de l'entité Fallacy");
			return info!.GetValue(fallacy) as string ?? string.Empty;
		}

		[Fact]
		public void Text_Fields_Never_Carry_The_2sxc_Empty_Marker()
		{
			// Le fixture 2021 sérialise un champ texte sans valeur en « [] » (liste vide du
			// dialecte 2sxc). Exporté tel quel, le marqueur s'affiche littéralement sur le site —
			// mesuré sur deux Example fr-FR avant le correctif #136 (a) ; il en reste un tant que
			// le repli du fixture le prend pour du contenu.
			var exportPath = TempExportPath();
			try
			{
				NewConfig(exportPath).Apply();

				var marked = ExportEntities(XDocument.Load(exportPath))
					.SelectMany(e => new[] { "Name", "Description", "Example" }
						.Where(f => Val(e, f) == "[]")
						.Select(f => $"{f}@{Val(e, "Language")}:{Val(e, "Name")}"))
					.ToList();

				marked.Should().BeEmpty("« [] » est le marqueur de vide 2sxc, pas un contenu");
			}
			finally
			{
				File.Delete(exportPath);
			}
		}

		[Fact]
		public void Translated_Entities_Carry_A_Name()
		{
			// La config morte d'origine : TitleFuncFr n'était jamais appelée — les traductions
			// nouvelles partaient sans Name, alors que le fixture 2021 en porte (« Argument fallacieux »).
			var exportPath = TempExportPath();
			try
			{
				NewConfig(exportPath).Apply();

				var ruEntities = ExportEntities(XDocument.Load(exportPath))
					.Where(e => Val(e, "Language") == "ru-RU").ToList();

				ruEntities.Where(e => string.IsNullOrEmpty(Val(e, "Name")))
					.Should().BeEmpty("chaque traduction porte son Name localisé (expressions par culture)");
			}
			finally
			{
				File.Delete(exportPath);
			}
		}
	}
}
