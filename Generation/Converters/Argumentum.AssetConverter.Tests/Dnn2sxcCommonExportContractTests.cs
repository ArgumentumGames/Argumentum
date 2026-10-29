using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Argumentum.AssetConverter.Dnn2sxc;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Sondes communes aux taxonomies Dnn2sxc — la <b>seule</b> source de vérité sur « quelles
	/// taxonomies existent ». Elle ne nomme aucune taxonomie en dur : l'ensemble se dérive de
	/// <see cref="Dnn2sxcConfig"/> avec le prédicat exact qu'utilise <c>Apply()</c> pour décider
	/// d'exporter (<c>CsvPath</c> renseigné). Une 5ᵉ taxonomie ajoutée à la config entre donc
	/// automatiquement dans la garde commune, et une taxonomie câblée sans export committé fait
	/// tomber le fait d'appariement.
	/// </summary>
	internal static class TaxonomyProbe
	{
		/// <summary>Préfixe des exports committés (le fixture 2021 et le CSV du même dossier n'en font pas partie).</summary>
		internal const string ExportPrefix = "2sxcContentExport_";

		internal static string Repo => TestRepoRoot.Find();

		internal static string CommittedDir => Path.Combine(Repo,
			"Generation", "Converters", "Argumentum.AssetConverter", "Data", "Dnn2sxc");

		private static IEnumerable<PropertyInfo> AllTaxonomyProperties()
		{
			return typeof(Dnn2sxcConfig)
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(p => p.PropertyType == typeof(Dnn2sxcTaxonomyExportConfig))
				.OrderBy(p => p.Name, StringComparer.Ordinal);
		}

		/// <summary>
		/// Taxonomies réellement exportées : propriétés de type <see cref="Dnn2sxcTaxonomyExportConfig"/>
		/// dont le <c>CsvPath</c> est renseigné. C'est <b>littéralement</b> le test de <c>Apply()</c>
		/// (« chaque taxonomie ne tourne que si elle est configurée ») — dérivé, pas recopié.
		/// </summary>
		internal static IReadOnlyList<string> WiredNames()
		{
			var config = new Dnn2sxcConfig();
			return AllTaxonomyProperties()
				.Where(p => !string.IsNullOrEmpty(((Dnn2sxcTaxonomyExportConfig)p.GetValue(config)!).CsvPath))
				.Select(p => p.Name)
				.ToList();
		}

		/// <summary>
		/// Config <b>réelle</b>, mutée pour n'exporter que <paramref name="taxonomy"/> vers
		/// <paramref name="exportPath"/> : les autres voient leur <c>CsvPath</c> vidé (le commutateur
		/// d'<c>Apply</c>). Les suites antérieures reconstruisaient une config à la main en recopiant
		/// <c>EntityTypeName</c>/<c>RootName</c>/<c>ContextKey</c> : un changement de production
		/// laissait alors le test vert sur l'ancienne valeur. Ici rien n'est recopié.
		/// </summary>
		internal static Dnn2sxcConfig ConfigWithOnly(string taxonomy, string exportPath)
		{
			var config = new Dnn2sxcConfig();
			var found = false;
			foreach (var property in AllTaxonomyProperties())
			{
				var taxonomyConfig = (Dnn2sxcTaxonomyExportConfig)property.GetValue(config)!;
				if (property.Name == taxonomy)
				{
					taxonomyConfig.ExportPath = exportPath; // n'écrit jamais l'artefact committé
					found = true;
				}
				else
				{
					taxonomyConfig.CsvPath = string.Empty;
				}
			}
			if (!found)
			{
				throw new ArgumentOutOfRangeException(nameof(taxonomy),
					$"'{taxonomy}' n'est pas une propriété Dnn2sxcTaxonomyExportConfig de Dnn2sxcConfig");
			}
			return config;
		}

		/// <summary>Exporte une seule taxonomie vers <paramref name="exportPath"/>.</summary>
		internal static void ExportOnly(string taxonomy, string exportPath)
		{
			ConfigWithOnly(taxonomy, exportPath).Apply();
		}

		/// <summary>Nom de fichier de l'export committé, <b>tel que la config le déclare</b>.</summary>
		internal static string DeclaredFileName(string taxonomy)
		{
			var config = new Dnn2sxcConfig();
			var property = typeof(Dnn2sxcConfig).GetProperty(taxonomy)!;
			var taxonomyConfig = (Dnn2sxcTaxonomyExportConfig)property.GetValue(config)!;
			taxonomyConfig.ExportPath.Should().NotBeNullOrEmpty(
				$"la taxonomie {taxonomy} déclare le chemin de son artefact committé");
			return Path.GetFileName(taxonomyConfig.ExportPath);
		}

		internal static string CommittedExportPath(string taxonomy)
		{
			return Path.Combine(CommittedDir, DeclaredFileName(taxonomy));
		}

		internal static string Val(XElement entity, string childName)
		{
			return entity.Element(childName)?.Value ?? string.Empty;
		}

		internal static List<XElement> Entities(string exportPath)
		{
			return XDocument.Load(exportPath).Root!.Elements("Entity").ToList();
		}

		internal static string TempExportPath(string taxonomy)
		{
			return Path.Combine(Path.GetTempPath(), $"dnn2sxc-common-{taxonomy}-{Guid.NewGuid():N}.xml");
		}
	}

	/// <summary>
	/// Un export partagé par taxonomie pour la durée de la classe : chaque fait structurel lit le
	/// même fichier au lieu de relancer <c>Apply()</c> sept fois. Seul le fait de reproductibilité
	/// refait une seconde passe.
	/// </summary>
	public sealed class CommonExportFixture : IDisposable
	{
		private readonly Dictionary<string, string> _paths = new Dictionary<string, string>(StringComparer.Ordinal);

		public string Export(string taxonomy)
		{
			if (!_paths.TryGetValue(taxonomy, out var path))
			{
				path = TaxonomyProbe.TempExportPath(taxonomy);
				TaxonomyProbe.ExportOnly(taxonomy, path);
				_paths[taxonomy] = path;
			}
			return path;
		}

		public void Dispose()
		{
			foreach (var path in _paths.Values)
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
		}
	}

	/// <summary>
	/// #136 (b), consolidation — garde <b>commune</b> aux quatre taxonomies Dnn2sxc.
	/// </summary>
	/// <remarks>
	/// <para><b>Pourquoi une suite de plus</b> : les quatre suites par taxonomie portaient chacune
	/// leur copie de quatre invariants identiques (XML valide sur fichier plus long, deux passes
	/// byte-identiques, hiérarchie résolue, entité nommée). Une copie ne prouve que ce qu'elle
	/// regarde : la régression qui n'entre que par la cinquième taxonomie n'était gardée par rien.
	/// Ici les invariants sont écrits <b>une fois</b> et s'appliquent à l'ensemble dérivé de la
	/// config.</para>
	/// <para><b>Invariants ajoutés</b> (qu'aucune suite existante ne portait) : toutes les cultures
	/// d'un export couvrent le <b>même jeu de Guid</b> (une traduction s'attache par Guid — un Guid
	/// manquant laisse une entité non traduite côté site), exactement <b>une racine par culture</b>,
	/// et l'ensemble des cultures exportées est <b>exactement</b> celui de la config.</para>
	/// <para><b>Mesuré avant d'asserter</b> (29/10, sur les 4 exports committés) : jeu de Guid
	/// identique dans les 8 cultures partout (1408 / 223 / 196 / 16 Guid), 1 racine par culture
	/// partout, 0 Name vide, 0 marqueur <c>[]</c>, 0 <c>&amp;#</c>. Les faits ci-dessous ne font que
	/// figer ce qui tient déjà.</para>
	/// <para><b>Non couvert</b> : l'acceptation par 2sxc v21 sur le portail (partie 2 de #136), et
	/// la justesse linguistique des valeurs (contrôle d'écriture, hors tests).</para>
	/// </remarks>
	public class Dnn2sxcCommonExportContractTests : IClassFixture<CommonExportFixture>
	{
		private readonly CommonExportFixture _exports;
		private readonly ITestOutputHelper _output;

		public Dnn2sxcCommonExportContractTests(CommonExportFixture exports, ITestOutputHelper output)
		{
			_exports = exports;
			_output = output;
		}

		/// <summary>Les taxonomies câblées — dérivées, jamais listées à la main.</summary>
		public static IEnumerable<object[]> WiredTaxonomies => TaxonomyProbe.WiredNames().Select(name => new object[] { name });

		[Fact]
		public void The_Wired_Taxonomy_Set_Matches_The_Committed_Exports_One_For_One()
		{
			// Deux énumérations indépendantes : la config (code) et le dossier d'artefacts (disque).
			// Elles doivent coïncider — c'est ce qui empêche une taxonomie de rejoindre la garde sans
			// que son export soit committé, et un export de survivre à la taxonomie qui le produisait.
			var declared = TaxonomyProbe.WiredNames()
				.Select(TaxonomyProbe.DeclaredFileName)
				.OrderBy(f => f, StringComparer.Ordinal)
				.ToList();

			// Par taxonomie d'abord : le message nomme celle qui manque, au lieu d'un diff de listes.
			foreach (var name in TaxonomyProbe.WiredNames())
			{
				File.Exists(TaxonomyProbe.CommittedExportPath(name)).Should().BeTrue(
					$"{name} : l'export committé « {TaxonomyProbe.DeclaredFileName(name)} » existe sous Data/Dnn2sxc");
			}

			var committed = Directory
				.EnumerateFiles(TaxonomyProbe.CommittedDir, TaxonomyProbe.ExportPrefix + "*.xml")
				.Select(Path.GetFileName)
				.OrderBy(f => f, StringComparer.Ordinal)
				.ToList();

			_output.WriteLine($"câblées : {string.Join(", ", declared)}");
			_output.WriteLine($"committées : {string.Join(", ", committed)}");

			declared.Should().NotBeEmpty("Dnn2sxcConfig déclare au moins une taxonomie active");
			declared.Should().Equal(committed,
				"une taxonomie câblée a exactement un export committé, et aucun export n'est orphelin");
		}

		[Theory]
		[MemberData(nameof(WiredTaxonomies))]
		public void Export_Is_Valid_Xml_Even_Over_A_Longer_Previous_File(string taxonomy)
		{
			var exportPath = TaxonomyProbe.TempExportPath(taxonomy);
			try
			{
				// Contrôle inverse de la troncature : 2 Mo de résidus, l'export doit les effacer.
				File.WriteAllText(exportPath, new string('x', 2_000_000));
				TaxonomyProbe.ExportOnly(taxonomy, exportPath);

				var document = XDocument.Load(exportPath); // lève si le XML est invalide — c'est le fait
				document.Root!.Name.LocalName.Should().Be("SexyContentData");
			}
			finally
			{
				File.Delete(exportPath);
			}
		}

		[Theory]
		[MemberData(nameof(WiredTaxonomies))]
		public void Two_Runs_Produce_A_Byte_Identical_Export(string taxonomy)
		{
			var first = TaxonomyProbe.TempExportPath(taxonomy);
			var second = TaxonomyProbe.TempExportPath(taxonomy);
			try
			{
				TaxonomyProbe.ExportOnly(taxonomy, first);
				TaxonomyProbe.ExportOnly(taxonomy, second);

				File.ReadAllBytes(first).Should().Equal(File.ReadAllBytes(second),
					"Guid déterministes dérivés du chemin — le ré-import 2sxc doit mettre à jour, pas dupliquer");
			}
			finally
			{
				File.Delete(first);
				File.Delete(second);
			}
		}

		[Theory]
		[MemberData(nameof(WiredTaxonomies))]
		public void Every_Configured_Culture_Is_Exported_And_No_Other(string taxonomy)
		{
			var entities = TaxonomyProbe.Entities(_exports.Export(taxonomy));
			var configured = DeclaredCultures(taxonomy);
			var exported = entities.Select(e => TaxonomyProbe.Val(e, "Language")).Distinct().ToList();

			_output.WriteLine($"configurées : {string.Join(", ", configured)} | exportées : {string.Join(", ", exported)}");

			exported.Should().BeEquivalentTo(configured,
				"le périmètre de langues exporté est celui de la config, ni plus ni moins");
		}

		[Theory]
		[MemberData(nameof(WiredTaxonomies))]
		public void Every_Culture_Covers_The_Same_Guid_Set(string taxonomy)
		{
			// Une traduction s'attache par Guid partagé : un Guid présent dans la culture maîtresse et
			// absent d'une autre laisse une entité non traduite côté site, sans erreur d'import.
			var entities = TaxonomyProbe.Entities(_exports.Export(taxonomy));
			var byCulture = entities
				.GroupBy(e => TaxonomyProbe.Val(e, "Language"))
				.ToDictionary(g => g.Key, g => g.Select(e => TaxonomyProbe.Val(e, "Guid")).ToHashSet());

			var master = DeclaredCultures(taxonomy)[0];
			byCulture.Should().ContainKey(master, $"la culture maîtresse {master} est la première de la config");

			foreach (var pair in byCulture.OrderBy(p => p.Key, StringComparer.Ordinal))
			{
				var missing = byCulture[master].Except(pair.Value).ToList();
				var extra = pair.Value.Except(byCulture[master]).ToList();
				_output.WriteLine($"{pair.Key} : {pair.Value.Count} Guid (manquants {missing.Count}, en trop {extra.Count})");

				missing.Should().BeEmpty($"{pair.Key} doit couvrir toutes les entités de {master} — {string.Join(", ", missing.Take(3))}");
				extra.Should().BeEmpty($"{pair.Key} ne doit porter aucune entité absente de {master} — {string.Join(", ", extra.Take(3))}");
			}
		}

		[Theory]
		[MemberData(nameof(WiredTaxonomies))]
		public void Exactly_One_Root_Per_Culture(string taxonomy)
		{
			var entities = TaxonomyProbe.Entities(_exports.Export(taxonomy));

			foreach (var culture in entities.GroupBy(e => TaxonomyProbe.Val(e, "Language")))
			{
				var roots = culture
					.Where(e => string.IsNullOrEmpty(TaxonomyProbe.Val(e, "Parent")))
					.ToList();

				roots.Should().HaveCount(1,
					$"{culture.Key} : l'arbre a une seule racine (rangée CSV 0 ou {taxonomy}.RootName)");
			}
		}

		[Theory]
		[MemberData(nameof(WiredTaxonomies))]
		public void Hierarchy_Parents_All_Resolve_Inside_The_Export(string taxonomy)
		{
			var entities = TaxonomyProbe.Entities(_exports.Export(taxonomy));
			var guids = entities.Select(e => TaxonomyProbe.Val(e, "Guid")).ToHashSet();

			var unresolved = entities
				.Select(e => TaxonomyProbe.Val(e, "Parent"))
				.Where(parent => !string.IsNullOrEmpty(parent) && !guids.Contains(parent))
				.ToList();

			unresolved.Should().BeEmpty(
				"un Parent qui ne pointe aucun Guid de l'export casse l'arbre à l'import 2sxc — {0} non résolu(s)",
				string.Join(", ", unresolved.Take(3)));
		}

		[Theory]
		[MemberData(nameof(WiredTaxonomies))]
		public void Text_Fields_Carry_No_Placeholder_Literal_Nor_Double_Encoding(string taxonomy)
		{
			// Deux littéraux de gabarit atteignent la donnée et s'affichent tels quels sur le site :
			//  « [] » = champ texte vide sérialisé par 2sxc (mesuré fr-FR avant #2055) ;
			//  « {} » = expression d'interpolation vide — Interpolate découpe sur {(.+?)} et laisse
			//          donc « {} » passer littéralement (mesuré : 120 <Example>{}</Example> dans
			//          l'export Rules, 15 règles × 8 cultures, corrigé dans la config le 29/10).
			// Et un double échappement HTML se rend « &#233; » au lieu de « é ».
			var placeholders = new[] { "[]", "{}" };
			var entities = TaxonomyProbe.Entities(_exports.Export(taxonomy));

			entities.Where(e => string.IsNullOrEmpty(TaxonomyProbe.Val(e, "Name")))
				.Should().BeEmpty("chaque entité, chaque culture, porte son Name");

			var marked = entities
				.SelectMany(e => new[] { "Name", "Description", "Example" }
					.Select(field => new { Entity = e, Field = field, Value = TaxonomyProbe.Val(e, field) }))
				.Where(x => placeholders.Contains(x.Value))
				.ToList();
			marked.Should().BeEmpty(
				"un littéral de gabarit (« [] », « {{}} ») n'est jamais du contenu — {0} occurrence(s), ex. {1}.{2}=« {3} »",
				marked.Count,
				marked.FirstOrDefault()?.Entity.Element("Language")?.Value,
				marked.FirstOrDefault()?.Field,
				marked.FirstOrDefault()?.Value);

			var encoded = entities
				.SelectMany(e => new[] { "Name", "Description", "Example" }
					.Select(field => TaxonomyProbe.Val(e, field)))
				.Where(value => value.Contains("&#"))
				.ToList();
			encoded.Should().BeEmpty("les accents sortent bruts — un « &#…; » littéral est un double échappement");
		}

		private static List<string> DeclaredCultures(string taxonomy)
		{
			var config = new Dnn2sxcConfig();
			var property = typeof(Dnn2sxcConfig).GetProperty(taxonomy)!;
			var taxonomyConfig = (Dnn2sxcTaxonomyExportConfig)property.GetValue(config)!;
			return taxonomyConfig.Cultures.Select(c => c.Culture).ToList();
		}
	}
}
