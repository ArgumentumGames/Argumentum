using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 grain module-level — socle de garde du pack <c>controls</c>, premier
	/// dossier du périmètre mesuré par #2072 (CORE sans <c>Install/</c> : 94 fichiers,
	/// 2 168 clés par langue ; ce lot : 11 fichiers, 133 clés).
	/// <para>
	/// ⚠️ Ce n'est PAS une garde de complétion du grain : elle n'épingle que le lot
	/// livré. Les règles sont écrites pour être **partagées entre cultures** — le
	/// contrat (fichiers, clés attendues, valeurs verbatim) ne dépend pas de la
	/// langue, seule <see cref="Culture"/> en change. Un lot suivant ajoute une
	/// sous-classe de trois lignes, il ne recopie pas les règles.
	/// </para>
	/// <para>
	/// Le neutre est la seule référence : la mesure #2072 a montré que **15 des
	/// 96 fichiers CORE ont un jeu de clés différent en ru-RU** (jusqu'à
	/// <c>Copyright.ascx</c>, où ru n'a aucune clé). Une garde adossée à ru
	/// accepterait des clés manquantes : elle se dérive du neutre.
	/// </para>
	/// </summary>
	public abstract class DnnModuleLevelControlsResxGuardBase
	{
		/// <summary>Culture couverte par le lot (ex. <c>zh-CN</c>, <c>ar-SA</c>).</summary>
		protected abstract string Culture { get; }

		/// <summary>Couverture livrée par le lot : 11 fichiers, 133 clés, toutes cultures.</summary>
		protected static readonly (string Stem, int ExpectedKeys)[] Covered =
		{
			("Address.ascx", 34),
			("DualListControl.ascx", 6),
			("filepickeruploader.ascx", 4),
			("Help.ascx", 4),
			("LocaleSelectorControl.ascx", 2),
			("ModuleAuditControl.ascx", 3),
			("SkinControl.ascx", 7),
			("TextEditor.ascx", 13),
			("URLControl.ascx", 27),
			("UrlTrackingControl.ascx", 9),
			("User.ascx", 24),
		};

		/// <summary>Total épinglé : la somme des <see cref="Covered"/>, jamais un littéral recopié.</summary>
		protected static readonly int CoveredKeyTotal = Covered.Sum(c => c.ExpectedKeys);

		/// <summary>
		/// Valeurs légitimement identiques à l'anglais — déclarées, pas tolérées.
		/// <c>HTML</c> est le nom du format ; <c>URL</c> est l'en-tête de colonne d'un
		/// tableau et s'emploie tel quel dans les deux cultures. Toute autre copie est
		/// un défaut (un lot non traduit passerait sinon pour un lot livré).
		/// </summary>
		protected virtual (string Stem, string Key)[] VerbatimByDesign => new[]
		{
			("TextEditor.ascx", "Html.Text"),
			("UrlTrackingControl.ascx", "URL.Text"),
		};

		private static readonly Regex Placeholder = new(@"\{\d+\}", RegexOptions.Compiled);

		/// <summary>
		/// Identité d'un jeton DNN : namespace + chemin AVANT le « | ». La partie
		/// affichable qui suit est de la prose à traduire.
		/// </summary>
		private static readonly Regex Token = new(
			@"\[(?:Portal|User|Host|Profile|Membership|Custom|Date|Module|Tab|Site|Page|App):([^\]\|]*)",
			RegexOptions.Compiled);

		protected const string RelDir = "DNNPlatform/controls/App_LocalResources";

		protected static string PathOf(string stem, string culture) =>
			Path.Combine(
				TestRepoRoot.Find(),
				RelDir.Replace('/', Path.DirectorySeparatorChar),
				$"{stem}{(culture.Length > 0 ? "." + culture : "")}.resx");

		protected static Dictionary<string, string> Values(string stem, string culture)
		{
			var path = PathOf(stem, culture);
			File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({RelDir}/{stem}.{culture}.resx)");
			return XDocument.Load(path).Root!.Elements("data").ToDictionary(
				d => (string)d.Attribute("name")!,
				d => (string?)d.Element("value") ?? string.Empty,
				StringComparer.Ordinal);
		}

		public static IEnumerable<object[]> CoveredFiles()
		{
			foreach (var (stem, expected) in Covered)
				yield return new object[] { stem, expected };
		}

		[Fact]
		public void TheLotCoversElevenFilesAndOneHundredThirtyThreeKeys()
		{
			Covered.Should().HaveCount(11, "le lot `controls` compte 11 familles CORE");
			CoveredKeyTotal.Should().Be(133, "le dénominateur du lot est mesuré par #2072, pas recopié");
			Covered.Select(c => c.Stem).Should().OnlyHaveUniqueItems();
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
		public void TheDeliveredKeySetIsExactlyTheNeutralOnes(string stem, int _)
		{
			var neutral = Values(stem, string.Empty);
			var localized = Values(stem, Culture);

			localized.Keys.Should().BeEquivalentTo(neutral.Keys,
				$"{stem}.{Culture}.resx : le pack doit couvrir TOUTES les clés du neutre — une clé absente "
				+ "retombe sur l'anglais, une clé en trop n'existe pas");
		}

		[Theory]
		[MemberData(nameof(CoveredFiles))]
		public void EveryDeliveredKeyIsSaneAgainstTheNeutral(string stem, int _)
		{
			var neutral = Values(stem, string.Empty);
			var localized = Values(stem, Culture);

			foreach (var (key, value) in localized)
			{
				value.Should().NotBeNullOrWhiteSpace($"{stem}.{Culture}:{key} — valeur vide");

				Placeholder.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
					.Should().Equal(
						Placeholder.Matches(neutral[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
						$"{stem}.{Culture}:{key} — multiset des placeholders modifié");

				Token.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
					.Should().Equal(
						Token.Matches(neutral[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
						$"{stem}.{Culture}:{key} — identité des jetons DNN modifiée");

				if (VerbatimByDesign.Contains((stem, key)))
					continue;

				value.Should().NotBe(neutral[key],
					$"{stem}.{Culture}:{key} — copie de l'anglais par accident (une copie VOLONTAIRE "
					+ "se déclare dans VerbatimByDesign)");
			}
		}

		/// <summary>
		/// Le BOM du fichier produit suit celui de SON neutre — convention du
		/// module-level, où le dossier <c>controls/App_LocalResources</c> mélange
		/// les deux : <c>Address.ascx.resx</c> n'a pas de BOM, <c>URLControl.ascx.resx</c>
		/// en a un. L'attente est DÉRIVÉE du neutre, jamais écrite en dur : une
		/// liste de 11 booléens recopiés serait fausse au premier fichier qui change.
		/// <para>
		/// ⚠️ Transport mesuré sur le <b>blob committé</b> (<c>git cat-file</c>), jamais
		/// sur l'arbre de travail : le runner CI a <c>core.autocrlf=true</c>, qui
		/// convertit LF → CRLF au checkout, donc un fichier sain y apparaît en CRLF.
		/// La première version lisait <c>File.ReadAllBytes</c> et rendait 7 rouges en
		/// CI pour des blobs à 0 CR — l'instrument mesurait la configuration de
		/// checkout, pas le dépôt (précédent #2078, <c>Committed_Blob_Is_Utf8_Bom_With_Lf_Only</c>).
		/// </para>
		/// </summary>
		[Theory]
		[MemberData(nameof(CoveredFiles))]
		public void BomMirrorsTheNeutralAndLineEndingsAreLf(string stem, int _)
		{
			var neutral = CommittedBlob($"{RelDir}/{stem}.resx");
			var produced = CommittedBlob($"{RelDir}/{stem}.{Culture}.resx");

			neutral.Length.Should().BeGreaterThan(100, "le blob doit être le fichier entier, pas un fragment");
			produced.Length.Should().BeGreaterThan(100, "le blob doit être le fichier entier, pas un fragment");

			var neutralBom = neutral.Length >= 3 && neutral[0] == 0xEF && neutral[1] == 0xBB && neutral[2] == 0xBF;
			var producedBom = produced.Length >= 3 && produced[0] == 0xEF && produced[1] == 0xBB && produced[2] == 0xBF;

			producedBom.Should().Be(neutralBom,
				$"{stem}.{Culture}.resx : le BOM doit suivre celui du neutre (neutre={neutralBom})");
			produced.Should().NotContain((byte)'\r',
				$"{stem}.{Culture}.resx : les fins de ligne du dépôt sont LF — un CR est du bruit de diff");
		}

		/// <summary>Lit le blob de <c>HEAD:&lt;path&gt;</c> — indépendant de core.autocrlf.</summary>
		protected static byte[] CommittedBlob(string rel) => DnnResxGuardSocle.CommittedBlob(rel);

		[Fact]
		public void TheVerbatimListCannotGrowSilently()
		{
			VerbatimByDesign.Should().HaveCount(2,
				"une liste blanche qui grandit en silence transforme « non traduit » en « traduit » : "
				+ "chaque ajout doit être décidé, pas constaté");
			VerbatimByDesign.Should().OnlyHaveUniqueItems();
			foreach (var (stem, key) in VerbatimByDesign)
			{
				Covered.Should().Contain(c => c.Stem == stem,
					$"la déclaration verbatim ({stem}, {key}) porte sur un fichier du lot");
				Values(stem, Culture).Should().ContainKey(key);
			}
		}
	}
}
