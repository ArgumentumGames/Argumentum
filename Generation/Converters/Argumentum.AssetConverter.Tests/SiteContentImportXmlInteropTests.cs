using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Argumentum.AssetConverter.Dnn2sxc;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #457 T1 — le XML d'import produit par <c>tools/dnn_i18n/site_content_pipeline.py</c> est lu
	/// par le <b>lecteur 2sxc du dépôt lui-même</b> (<see cref="SexyContentData"/> /
	/// <see cref="Entity"/>, celui que <c>Dnn2sxcConfig.Apply()</c> utilise).
	///
	/// <para><b>Ce que cette suite prouve</b> : le dialecte émis (une <c>&lt;Entity&gt;</c> par
	/// couple Guid × Language, la dimension portée par l'élément <c>&lt;Language&gt;</c>) est
	/// désérialisable sans erreur par le lecteur existant, et les blocs cibles portent bien du
	/// texte traduit dans leur propre écriture.</para>
	///
	/// <para><b>Ce qu'elle ne prouve PAS</b> — et qu'il ne faut pas lui faire dire : que 2sxc v21
	/// accepte ce fichier à l'import sur le site live. C'est l'inconnue de T1+I3, qui se tranche
	/// sur le portail, sur une entité, avant tout volume.</para>
	///
	/// <para><b>Le fait qui justifie le design</b> : la classe <see cref="Entity"/> est câblée en
	/// dur sur l'attribute-set <i>Fallacy</i>. Sur un contenu <i>Game Rule</i>, XmlSerializer
	/// ignore en silence tout élément qu'il ne connaît pas — mesuré ici : <b>10 des 15 attributs
	/// sont perdus</b> (Title, Summary, Material, MinNbPlayers, MaxNbPlayers, Installation,
	/// Content, Variants, Memo, UrlKey), tandis que les 5 de l'intersection survivent (Parent,
	/// Author, Original, Licence, Date). C'est pourquoi le pipeline est <i>schema-driven</i> et
	/// n'écrit pas via cette classe : réutiliser <see cref="Entity"/> telle quelle produirait un
	/// XML amputé sans le moindre avertissement.</para>
	/// </summary>
	public class SiteContentImportXmlInteropTests
	{
		private static string FixturePath => Path.Combine(
			TestRepoRoot.Find(), "tools", "dnn_i18n", "fixtures", "one-entity-game-rule-import.xml");

		private const string GameRuleGuid = "ae1edefa-6f1b-4593-8230-97fa1edf4f78";
		private const string FrenchTitle = "L'école des menteurs";

		private static readonly string[] Cultures =
		{
			"fr-FR", "en-US", "ru-RU", "pt-PT", "es-ES", "ar-SA", "fa-IR", "zh-CN"
		};

		/// <summary>
		/// Attributs Game Rule que la classe <see cref="Entity"/> ne peut pas représenter.
		/// Mesuré, pas supposé : Game Rule = 15 attributs, Entity en connaît 5
		/// (Parent, Author, Original, Licence, Date) ⇒ 10 tombes.
		/// </summary>
		private static readonly string[] DroppedByLegacyEntity =
		{
			"Title", "Summary", "Material", "MinNbPlayers", "MaxNbPlayers",
			"Installation", "Content", "Variants", "Memo", "UrlKey"
		};

		[Fact]
		public void Fixture_Is_Committed_And_Substantive()
		{
			File.Exists(FixturePath).Should().BeTrue(
				$"la charge d'import d'une entité doit être committée ({FixturePath})");
			new FileInfo(FixturePath).Length.Should().BeGreaterThan(1000,
				"un fichier quasi vide passerait tous les contrôles de forme sans rien porter");
		}

		[Fact]
		public void Repo_2sxc_reader_parses_the_pipeline_output()
		{
			var data = Deserialize();

			data.Should().NotBeNull();
			data!.Entity.Should().HaveCount(8, "une <Entity> par culture dans la dimension");

			// Forme non ambiguë : sur une collection de string, `Equal(valeur, "parce que")`
			// résout vers `Equal(params string[])` et le message de raison devient un SECOND
			// élément attendu — la garde passe alors pour de mauvaises raisons.
			var types = data.Entity.Select(e => e.Type).Distinct().ToList();
			types.Should().HaveCount(1);
			types[0].Should().Be("Game Rule");

			var guids = data.Entity.Select(e => e.Guid).Distinct().ToList();
			guids.Should().HaveCount(1, "toutes les langues partagent le Guid — c'est la dimension");
			guids[0].Should().Be(GameRuleGuid);

			data.Entity.Select(e => e.Language).Should()
				.Equal((IEnumerable<string>)Cultures, "l'ordre suit le pipeline, culture par culture");
		}

		[Fact]
		public void Legacy_Entity_class_silently_drops_ten_Game_Rule_attributes()
		{
			var unknowns = new List<string>();
			var serializer = new XmlSerializer(typeof(SexyContentData), new[] { typeof(Entity) });
			serializer.UnknownNode += (_, e) =>
			{
				if (e.NodeType == XmlNodeType.Element)
				{
					unknowns.Add(e.Name);
				}
			};
			using (var reader = File.OpenText(FixturePath))
			{
				serializer.Deserialize(reader);
			}

			unknowns.Distinct().Should().BeEquivalentTo(DroppedByLegacyEntity,
				"ce sont exactement les attributs Game Rule absents de la classe Entity");
			// 10 dans le bloc fr-FR (les attributs stockés de cette entité) + 7 par bloc cible.
			unknowns.Count.Should().Be(DroppedByLegacyEntity.Length + 7 * 7);

			// Nuance : la perte est PARTIELLE — Date appartient aux deux jeux et survit.
			Deserialize()!.Entity.First(e => e.Language == "fr-FR").Date
				.Should().NotBeNullOrEmpty("Date est commun à Fallacy et Game Rule, donc lu");
		}

		[Fact]
		public void Target_blocks_carry_translated_text_in_their_own_script()
		{
			var doc = XDocument.Load(FixturePath);
			var root = doc.Root!;
			root.Name.LocalName.Should().Be("SexyContentData");
			root.Elements("Entity").Should().HaveCount(8);
			var namespaces = doc.Descendants().Select(e => e.Name.NamespaceName).Distinct().ToList();
			namespaces.Should().HaveCount(1, "le dialecte attesté n'utilise aucun espace de noms");
			namespaces[0].Should().Be("");

			var byCulture = root.Elements("Entity")
				.ToDictionary(e => e.Element("Language")!.Value, e => e);

			byCulture["fr-FR"].Element("Title")!.Value.Should().Be(FrenchTitle,
				"le titre source doit traverser le CSV octet à octet");

			// PORTÉE DÉCLARÉE : ce contrôle est un détecteur de CONTAMINATION (le modèle a
			// répondu en anglais/français dans une cellule non latine), calibré sur la règle du
			// vérificateur frère (verify_game_rule_translations.py : >= 3 caractères ET ratio
			// > 0,1). Il est INSENSIBLE à la corruption d'un seul caractère par construction —
			// une garde à ratio ne peut pas mesurer la fidélité. La fidélité cellule à cellule
			// est portée par les aller-retours octet-exacts du self-test Python (RT1/RT2).
			IsScriptDominant(byCulture["ru-RU"].Element("Title")!.Value,
				'Ѐ', 'ӿ').Should().BeTrue("cyrillique attendu");
			IsScriptDominant(byCulture["ar-SA"].Element("Title")!.Value,
				'؀', 'ۿ').Should().BeTrue("écriture arabe attendue");
			IsScriptDominant(byCulture["zh-CN"].Element("Title")!.Value,
				'一', '鿿').Should().BeTrue("sinogrammes attendus");
		}

		/// <summary>
		/// Même barème que <c>verify_game_rule_translations.py</c> <c>check_script()</c> : au moins
		/// <paramref name="minCount"/> caractères de la plage ET plus de <paramref name="minRatio"/>
		/// de la cellule. Un seul caractère survivant ne suffit donc pas à valider une cellule.
		/// </summary>
		private static bool IsScriptDominant(string text, char low, char high,
			int minCount = 3, double minRatio = 0.1)
		{
			var count = text.Count(c => c >= low && c <= high);
			return count >= minCount && count / (double)Math.Max(1, text.Length) > minRatio;
		}

		private static SexyContentData? Deserialize()
		{
			var serializer = new XmlSerializer(typeof(SexyContentData), new[] { typeof(Entity) });
			using var reader = File.OpenText(FixturePath);
			return (SexyContentData?)serializer.Deserialize(reader);
		}
	}
}
