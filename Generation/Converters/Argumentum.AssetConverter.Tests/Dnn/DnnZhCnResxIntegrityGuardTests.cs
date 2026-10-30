using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 grain 9-bis — garde d'INTÉGRITÉ du pack zh-CN de la famille AGR cœur
	/// (Tier 2). Elle ne contrôle pas la COMPLÉTION (le pack est volontairement
	/// partiel : DNN retombe sur le neutre anglais, clé par clé — même contrat que
	/// le pack Tier 1 ar/fa/zh déjà en ligne, voir
	/// <see cref="ChromeTier1LocalizationContractTests"/>), mais elle garantit que
	/// ce qui EST livré est sain :
	/// <list type="bullet">
	/// <item>toute clé présente existe dans le neutre (pas de clé inventée) ;</item>
	/// <item>toute valeur est non vide ;</item>
	/// <item>le MULTISET des placeholders {0}/{1} est identique à celui du neutre —
	/// c'est le faux vert le plus probable, une traduction qui « déballe » le
	/// placeholder reste verte si l'on ne compare que la présence ;</item>
	/// <item>l'IDENTITÉ des jetons DNN ([Portal:URL], [Custom:4|…]) est conservée —
	/// seule la partie affichable est traduite ;</item>
	/// <item>aucune valeur n'est une copie de l'anglais par accident (une copie
	/// volontaire passe par la liste blanche ci-dessous) ;</item>
	/// <item>les textes juridiques MESSAGE_PORTAL_TERMS / MESSAGE_PORTAL_PRIVACY
	/// gardent leur valeur chinoise déjà arbitrée (grain juridique) — ils ne sont
	/// ni réécrits ni replacés par l'anglais.</item>
	/// </list>
	/// </summary>
	public class DnnZhCnResxIntegrityGuardTests
	{
		private const string Culture = "zh-CN";

		/// <summary>Couverture livrée par ce grain, par fichier de la famille AGR cœur.</summary>
		private static readonly (string Stem, int ExpectedKeys)[] Covered =
		{
			("FileUpload", 4),
			("WebControls", 50),
			("Exceptions", 132),
			("GlobalResources", 172),
		};

		/// <summary>
		/// Clés dont la valeur est légitimement identique à l'anglais : jeton pur,
		/// marque, texte sans prose. Pour les fichiers livrés ici il n'y en a
		/// qu'une — le sujet d'un courriel de bannière, qui n'est qu'un jeton
		/// (« [Custom:0] »). Toute AUTRE copie anglaise est un défaut.
		/// </summary>
		private static readonly string[] VerbatimByDesign = { "EMAIL_BANNER_NOTIFICATION_SUBJECT.Text" };

		private static readonly Regex Placeholder = new(@"\{\d+\}", RegexOptions.Compiled);

		/// <summary>
		/// Identité d'un jeton DNN : namespace + chemin AVANT le « | ». La partie
		/// affichable qui suit est de la prose à traduire, pas une identité.
		/// </summary>
		private static readonly Regex Token = new(
			@"\[(?:Portal|User|Host|Profile|Membership|Custom|Date|Module|Tab|Site|Page|App):([^\]\|]*)",
			RegexOptions.Compiled);

		private static readonly string[] LegalKeys =
		{
			"MESSAGE_PORTAL_TERMS.Text",
			"MESSAGE_PORTAL_PRIVACY.Text",
		};

		private static XDocument Load(string stem, string culture)
		{
			var rel = $"DNNPlatform/App_GlobalResources/{stem}{(culture.Length > 0 ? "." + culture : "")}.resx";
			var path = Path.Combine(TestRepoRoot.Find(), rel.Replace('/', Path.DirectorySeparatorChar));
			File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({rel})");
			return XDocument.Load(path);
		}

		private static Dictionary<string, string> Values(string stem, string culture) =>
			Load(stem, culture).Root!.Elements("data").ToDictionary(
				d => (string)d.Attribute("name")!,
				d => (string?)d.Element("value") ?? string.Empty,
				StringComparer.Ordinal);

		public static IEnumerable<object[]> CoveredFiles()
		{
			foreach (var (stem, expected) in Covered)
				yield return new object[] { stem, expected };
		}

		[Theory]
		[MemberData(nameof(CoveredFiles))]
		public void PackCoversTheDeclaredKeyCount(string stem, int expectedKeys)
		{
			Values(stem, Culture).Should().HaveCount(expectedKeys,
				$"{stem}.{Culture}.resx : le nombre de clés livrées a changé — mettre à jour la garde");
		}

		[Theory]
		[MemberData(nameof(CoveredFiles))]
		public void EveryDeliveredKeyIsSaneAgainstTheNeutral(string stem, int _)
		{
			var neutral = Values(stem, string.Empty);
			var culture = Values(stem, Culture);

			foreach (var (key, value) in culture)
			{
				neutral.Should().ContainKey(key, $"{stem}.{Culture} : clé « {key} » absente du neutre");
				var source = neutral[key];

				value.Should().NotBeNullOrWhiteSpace($"{stem}.{Culture}:{key} — valeur vide");

				if (VerbatimByDesign.Contains(key, StringComparer.Ordinal))
					continue;

				if (LegalKeys.Contains(key, StringComparer.Ordinal))
					continue;

				value.Should().NotBe(source,
					$"{stem}.{Culture}:{key} — copie de l'anglais par accident (une copie VOLONTAIRE "
					+ "se déclare dans VerbatimByDesign)");

				Placeholder.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
					.Should().Equal(Placeholder.Matches(source).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
						$"{stem}.{Culture}:{key} — multiset des placeholders modifié");

				Token.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
					.Should().Equal(Token.Matches(source).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
						$"{stem}.{Culture}:{key} — identité des jetons DNN modifiée");
			}
		}

		[Fact]
		public void LegalTextsKeepTheirAlreadyArbitratedChineseValues()
		{
			var culture = Values("GlobalResources", Culture);
			foreach (var key in LegalKeys)
			{
				culture.Should().ContainKey(key,
					$"GlobalResources.{Culture} : le texte juridique « {key} » a disparu du pack");
				culture[key].Should().NotBeEmpty();
				culture[key].Should().NotBe(Values("GlobalResources", string.Empty)[key],
					$"GlobalResources.{Culture}:{key} — le texte juridique a été replacé par l'anglais");
			}
		}

		[Fact]
		public void GlobalResourcesKeepsItsTwoPreExistingKeys()
		{
			// Les deux seules clés que le pack needorit pas : elles sont déjà là
			// (grain juridique) et le constructeur les doit PRESERVER, pas les
			// écraser par une traduction anglaise.
			var culture = Values("GlobalResources", Culture);
			culture.Should().ContainKey("MESSAGE_PORTAL_TERMS.Text");
			culture.Should().ContainKey("MESSAGE_PORTAL_PRIVACY.Text");
		}
	}
}
