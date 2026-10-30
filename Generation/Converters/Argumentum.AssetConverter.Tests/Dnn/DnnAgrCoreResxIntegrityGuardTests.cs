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
	/// #458 grain 9-bis — garde d'INTÉGRITÉ des packs de la famille AGR cœur
	/// (Tier 2), par culture. Elle ne contrôle pas la COMPLÉTION (le pack est
	/// volontairement partiel : DNN retombe sur le neutre anglais, clé par clé —
	/// même contrat que le pack Tier 1 ar/fa/zh déjà en ligne, voir
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
	/// gardent leur valeur déjà arbitrée (grain juridique) — ils ne sont ni
	/// réécrits ni replacés par l'anglais.</item>
	/// </list>
	/// Cultures couvertes : zh-CN (lot 1, PR #2067), ar-SA (lot 2, PR #2068)
	/// puis fa-IR (lot 3).
	/// </summary>
	public class DnnAgrCoreResxIntegrityGuardTests
	{
		/// <summary>Couverture livrée par ce grain, par culture et par fichier.</summary>
		private static readonly (string Culture, string Stem, int ExpectedKeys)[] Covered =
		{
			("zh-CN", "FileUpload", 4),
			("zh-CN", "WebControls", 50),
			("zh-CN", "Exceptions", 132),
			("zh-CN", "GlobalResources", 172),
			("ar-SA", "FileUpload", 4),
			("ar-SA", "WebControls", 50),
			("ar-SA", "Exceptions", 132),
			("ar-SA", "GlobalResources", 172),
			("fa-IR", "FileUpload", 4),
			("fa-IR", "WebControls", 50),
			("fa-IR", "Exceptions", 132),
			("fa-IR", "GlobalResources", 172),
		};

		/// <summary>
		/// Clés dont la valeur est légitimement identique à l'anglais : jeton pur,
		/// marque, ponctuation. zh-CN : le sujet d'un courriel de bannière, qui
		/// n'est qu'un jeton (« [Custom:0] »). ar-SA et fa-IR : la même, plus le
		/// suffixe de champ « : » — l'arabe comme le persan emploient le
		/// deux-points latin, ils n'ont pas de variante locale (zh-CN dispose du
		/// « ： » plein-chasse, pas eux).
		/// Toute AUTRE copie anglaise est un défaut.
		/// </summary>
		private static readonly Dictionary<string, string[]> VerbatimByDesign = new Dictionary<string, string[]>
		{
			["zh-CN"] = new[] { "EMAIL_BANNER_NOTIFICATION_SUBJECT.Text" },
			["ar-SA"] = new[] { "EMAIL_BANNER_NOTIFICATION_SUBJECT.Text", "FieldSuffix.Text" },
			["fa-IR"] = new[] { "EMAIL_BANNER_NOTIFICATION_SUBJECT.Text", "FieldSuffix.Text" },
		};

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
			foreach (var (culture, stem, expected) in Covered)
				yield return new object[] { culture, stem, expected };
		}

		[Theory]
		[MemberData(nameof(CoveredFiles))]
		public void PackCoversTheDeclaredKeyCount(string culture, string stem, int expectedKeys)
		{
			Values(stem, culture).Should().HaveCount(expectedKeys,
				$"{stem}.{culture}.resx : le nombre de clés livrées a changé — mettre à jour la garde");
		}

		[Theory]
		[MemberData(nameof(CoveredFiles))]
		public void EveryDeliveredKeyIsSaneAgainstTheNeutral(string culture, string stem, int _)
		{
			var neutral = Values(stem, string.Empty);
			var localized = Values(stem, culture);
			var verbatim = VerbatimByDesign[culture];

			foreach (var (key, value) in localized)
			{
				neutral.Should().ContainKey(key, $"{stem}.{culture} : clé « {key} » absente du neutre");
				var source = neutral[key];

				value.Should().NotBeNullOrWhiteSpace($"{stem}.{culture}:{key} — valeur vide");

				if (verbatim.Contains(key, StringComparer.Ordinal))
					continue;

				if (LegalKeys.Contains(key, StringComparer.Ordinal))
					continue;

				value.Should().NotBe(source,
					$"{stem}.{culture}:{key} — copie de l'anglais par accident (une copie VOLONTAIRE "
					+ $"se déclare dans VerbatimByDesign[{culture}])");

				Placeholder.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
					.Should().Equal(Placeholder.Matches(source).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
						$"{stem}.{culture}:{key} — multiset des placeholders modifié");

				Token.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
					.Should().Equal(Token.Matches(source).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
						$"{stem}.{culture}:{key} — identité des jetons DNN modifiée");
			}
		}

		[Theory]
		[InlineData("zh-CN")]
		[InlineData("ar-SA")]
		[InlineData("fa-IR")]
		public void LegalTextsKeepTheirAlreadyArbitratedValues(string culture)
		{
			var localized = Values("GlobalResources", culture);
			foreach (var key in LegalKeys)
			{
				localized.Should().ContainKey(key,
					$"GlobalResources.{culture} : le texte juridique « {key} » a disparu du pack");
				localized[key].Should().NotBeEmpty();
				localized[key].Should().NotBe(Values("GlobalResources", string.Empty)[key],
					$"GlobalResources.{culture}:{key} — le texte juridique a été replacé par l'anglais");
			}
		}
	}
}
