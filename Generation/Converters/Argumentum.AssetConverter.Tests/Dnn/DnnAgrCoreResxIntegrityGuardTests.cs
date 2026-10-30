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
	/// <item>l'ENSEMBLE des clés manquantes est DÉCLARÉ, clé par clé, par
	/// fichier : le compte exact ne dit rien de son appartenance (contre-revue
	/// web2 sur #2079) ;</item>
	/// <item>une clé dont le NEUTRE est vide n'est JAMAIS livrée : il n'y a rien
	/// à traduire, toute valeur y serait inventée ;</item>
	/// <item>les textes juridiques MESSAGE_PORTAL_TERMS / MESSAGE_PORTAL_PRIVACY
	/// gardent leur valeur déjà arbitrée (grain juridique) — ils ne sont ni
	/// réécrits ni replacés par l'anglais.</item>
	/// </list>
	/// Cultures couvertes : zh-CN (lot 1, PR #2067), ar-SA (lot 2, PR #2068)
	/// puis fa-IR (lot 3). Packs complets <c>SharedResources</c> : zh-CN (lot 4),
	/// ar-SA (lot 5) puis fa-IR (lot 6) — même neutre, donc même dénominateur de 610.
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
			// #458 grain 9-bis lot 4 — the neutral carries 617 keys, 7 of them with an
			// EMPTY value (File.Text and six DefaultWorkflowState*.OnDiscard*/OnCompleteMessage*
			// that the product never fills). The pack therefore delivers 610: a key with an
			// empty neutral has nothing to translate and is refused by the builder rather
			// than filled with invention.
			("zh-CN", "SharedResources", 610),
			("ar-SA", "FileUpload", 4),
			("ar-SA", "WebControls", 50),
			("ar-SA", "Exceptions", 132),
			("ar-SA", "GlobalResources", 172),
			// #458 grain 9-bis lot 5 — same denominator as zh-CN (lot 4): 617 neutral
			// keys, 7 of them empty at the neutral, so 610 translatable.
			("ar-SA", "SharedResources", 610),
			("fa-IR", "FileUpload", 4),
			("fa-IR", "WebControls", 50),
			("fa-IR", "Exceptions", 132),
			("fa-IR", "GlobalResources", 172),
			// #458 grain 9-bis lot 6 — troisième pack complet SharedResources :
			// même neutre que les lots 4 et 5, donc même dénominateur (610).
			("fa-IR", "SharedResources", 610),
		};

		/// <summary>
		/// Jetons d'un pack <c>SharedResources</c> complet — l'ensemble est le MÊME pour
		/// toute culture, puisque le neutre est le même fichier. Déclaré une seule fois :
		/// recopié par culture, il dériverait en silence au lot suivant. Ce sont les huit
		/// littéraux booléens <c>True</c>/<c>False</c> (parsés PAR NOM dans les listes —
		/// les traduire changerait le comportement, pas la langue), la marque produit
		/// « APP ID », le glyphe de tri « A-Z » et les quatre unités B/KB/MB/GB.
		/// </summary>
		private static readonly string[] SharedResourcesPackTokens =
		{
			"True.Text", "False.Text",
			"OAuthConfigBase_APIKey.Text",
			"DropDownList.SortAscendingButtonTitle",
			"SizeB.Text", "SizeKb.Text", "SizeMb.Text", "SizeGb.Text",
		};

		/// <summary>
		/// Clés dont la valeur est légitimement identique à l'anglais : jeton pur,
		/// marque, ponctuation. zh-CN : le sujet d'un courriel de bannière, qui
		/// n'est qu'un jeton (« [Custom:0] »). ar-SA et fa-IR : la même, plus le
		/// suffixe de champ « : » — l'arabe comme le persan emploient le
		/// deux-points latin, ils n'ont pas de variante locale (zh-CN dispose du
		/// « ： » plein-chasse, pas eux). Les packs <c>SharedResources</c> complets
		/// (lots 4 et 5) y ajoutent <see cref="SharedResourcesPackTokens"/>.
		/// Toute AUTRE copie anglaise est un défaut.
		/// </summary>
		private static readonly Dictionary<string, string[]> VerbatimByDesign = new Dictionary<string, string[]>
		{
			["zh-CN"] = new[] { "EMAIL_BANNER_NOTIFICATION_SUBJECT.Text" }
				.Concat(SharedResourcesPackTokens).ToArray(),
			// ar-SA : même noyau (lot 5) que zh-CN (lot 4), plus « FieldSuffix.Text » et
			// le sujet de courriel de bannière déjà arbitrés au lot 2.
			["ar-SA"] = new[] { "EMAIL_BANNER_NOTIFICATION_SUBJECT.Text", "FieldSuffix.Text" }
				.Concat(SharedResourcesPackTokens).ToArray(),
			["fa-IR"] = new[] { "EMAIL_BANNER_NOTIFICATION_SUBJECT.Text", "FieldSuffix.Text" }
				.Concat(SharedResourcesPackTokens).ToArray(),
		};

		/// <summary>
		/// Clés autorisées à MANQUER dans un pack complet, par fichier — et par
		/// fichier seulement : le neutre est le même pour toutes les cultures, donc
		/// l'ensemble manquant autorisé est une propriété du FICHIER, pas de la
		/// culture. Une culture qui s'en écarte est rouge.
		/// <para>
		/// Cette déclaration ferme le trou que la contre-revue web2 a mesuré sur
		/// #2079 : le compte exact (610) ne dit rien de l'APPARTENANCE de
		/// l'ensemble manquant. Un échange tenant le compte — retirer une vraie clé
		/// et livrer une clé à neutre VIDE avec un texte inventé — passait les deux
		/// gardes, parce que la valeur neutre de <c>File.Text</c> est vide : ni
		/// « copie de l'anglais » ni « valeur vide » ne pouvait la prendre.
		/// </para>
		/// </summary>
		private static readonly Dictionary<string, string[]> AllowedMissingByStem =
			new Dictionary<string, string[]>
			{
				// Ces trois fichiers sont livrés intégralement : rien ne peut manquer.
				["FileUpload"] = Array.Empty<string>(),
				["WebControls"] = Array.Empty<string>(),
				["Exceptions"] = Array.Empty<string>(),

				// GlobalResources : deux gabarits à neutre VIDE (le produit ne les
				// remplit jamais) et huit blocs volontairement NON traduits — code
				// HTML de pages d'erreur, listes de mots-clés, corps de courriel
				// HTML et licence MIT. Les huit ont un neutre NON vide : sans
				// déclaration, ils rendraient la règle « toute clé neutre non vide
				// est livrée » fausse.
				["GlobalResources"] = new[]
				{
					"MESSAGE_LOGIN_INSTRUCTIONS.Text",
					"MESSAGE_REGISTRATION_INSTRUCTIONS.Text",
					"403_3.Error",
					"404.Error",
					"500.Error",
					"DefaultStopwordGroup.Text",
					"DefaultSynonymGroup.Text",
					"EMAIL_MESSAGING_DISPATCH_BODY.Text",
					"EMAIL_MESSAGING_DISPATCH_ITEM.Text",
					"License.Text",
				},

				// SharedResources : les sept clés à neutre VIDE du fichier neutre
				// (617 − 7 = 610 traduisibles). Le constructeur refuse de les
				// traduire ; la garde refuse qu'un autre chemin les livre.
				["SharedResources"] = new[]
				{
					"DefaultWorkflowState1.OnDiscardMessageBody",
					"DefaultWorkflowState1.OnDiscardMessageSubject",
					"DefaultWorkflowState3.OnCompleteMessageBody",
					"DefaultWorkflowState3.OnCompleteMessageSubject",
					"DefaultWorkflowState3.OnDiscardMessageBody",
					"DefaultWorkflowState3.OnDiscardMessageSubject",
					"File.Text",
				},
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

				// §4 de la contre-revue web2 — la garantie STRUCTURELLE ne dépend
				// pas de la liste blanche : elle vaut pour TOUTE clé livrée. Les
				// `continue` ci-dessous ne court-circuitent plus que la comparaison
				// à l'anglais, qui est la seule règle qu'ils doivent court-circuiter.
				Placeholder.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
					.Should().Equal(Placeholder.Matches(source).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
						$"{stem}.{culture}:{key} — multiset des placeholders modifié");

				Token.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
					.Should().Equal(Token.Matches(source).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
						$"{stem}.{culture}:{key} — identité des jetons DNN modifiée");

				// §3 de la même contre-revue — une clé dont le NEUTRE est vide ne
				// doit JAMAIS être livrée : il n'y a rien à traduire. Le
				// constructeur refuse de l'inventer ; la garde refuse qu'un autre
				// chemin la livre.
				source.Should().NotBeNullOrWhiteSpace(
					$"{stem}.{culture}:{key} — le neutre de cette clé est VIDE : elle ne doit pas être "
					+ $"livrée (toute valeur ici serait inventée)");

				if (verbatim.Contains(key, StringComparer.Ordinal))
					continue;

				if (LegalKeys.Contains(key, StringComparer.Ordinal))
					continue;

				value.Should().NotBe(source,
					$"{stem}.{culture}:{key} — copie de l'anglais par accident (une copie VOLONTAIRE "
					+ $"se déclare dans VerbatimByDesign[{culture}])");
			}
		}

		/// <summary>
		/// §3 de la contre-revue web2 — l'APPARTENANCE de l'ensemble manquant est
		/// déclarée, clé par clé. Le compte exact ne la disait pas : un échange
		/// tenant le compte (retirer une vraie clé, livrer une clé à neutre vide
		/// avec un texte inventé) passait les deux gardes. Ce test est ce qui rend
		/// <c>CompletedPacks</c> sûr à étendre au lot suivant.
		/// </summary>
		[Theory]
		[MemberData(nameof(CoveredFiles))]
		public void TheMissingKeysAreExactlyTheDeclaredOnes(string culture, string stem, int _)
		{
			var neutral = Values(stem, string.Empty);
			var localized = Values(stem, culture);

			var missing = neutral.Keys.Except(localized.Keys, StringComparer.Ordinal)
				.OrderBy(x => x, StringComparer.Ordinal).ToArray();
			var declared = AllowedMissingByStem[stem].OrderBy(x => x, StringComparer.Ordinal).ToArray();

			missing.Should().Equal(declared,
				$"{stem}.{culture}.resx : l'ensemble des clés manquantes a changé. Une clé absente "
				+ $"non déclarée est une couverture perdue ; une clé déclarée présente est un "
				+ $"remplissage non autorisé (invention).");

			localized.Keys.Except(neutral.Keys, StringComparer.Ordinal).Should().BeEmpty(
				$"{stem}.{culture}.resx : clé livrée absente du neutre");
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
