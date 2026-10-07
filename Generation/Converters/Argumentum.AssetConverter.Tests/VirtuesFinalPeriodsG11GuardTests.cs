using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Grain 11 (#458 file profonde) — ponctuation finale des <c>description_&lt;lang&gt;</c>
	/// des Vertus : <b>64 cellules, 6 langues, UN seul geste</b>.
	///
	/// <para><b>Pourquoi un seul geste.</b> Les dossiers des grains 6 → 9 ont établi que la
	/// famille n'est pas « une par langue » mais <b>une seule</b>, perdue à une passe partagée :
	/// en mesurant l'intersection inter-langues, pk 8 perd le point dans <b>7</b> langues,
	/// pk 4/6/7 dans <b>6</b>, pk 12 dans 5, pk 2 dans 4 — 22 rangées touchées au total, et le
	/// <c>fr</c> est la <b>seule</b> langue à avoir toujours porté le point. Corriger langue par
	/// langue aurait produit six PR pour un seul défaut.</para>
	///
	/// <para><b>Ce que la garde épingle.</b> L'invariant est désormais total et vérifiable :
	/// <b>223/223</b> descriptions se terminent par la ponctuation de leur langue — <c>。</c>
	/// (U+3002) pour le <c>zh</c>, <c>.</c> pour les six autres — exactement comme le
	/// <c>fr</c> de référence, qui était déjà à 223/223. La convention de ponctuation est
	/// <b>lue dans le corpus</b> (zh : 215/223 avant correction, 96 %), jamais imposée depuis
	/// le français.</para>
	///
	/// <para><b>Frontière avec #1795 (grain 10).</b> Les 13 cellules <c>description_en</c> du
	/// même défaut sont portées par la PR #1795, non fusionnée à l'écriture de cette garde :
	/// le <c>en</c> est donc <b>hors périmètre</b> ici, et reste à 210/223. Aucune épingle de
	/// cette classe ne vise une colonne <c>_en</c> — c'est asserté explicitement, pour que la
	/// fusion de #1795 ne rende pas cette garde rouge.</para>
	///
	/// <para><b>Instrument.</b> Un écran de ponctuation qui ne connaît pas <c>。</c> lisait
	/// « 223/223 descriptions zh sans point final » alors que 215 en portaient un ; un écran
	/// qui ne connaît pas les guillemets courbes lit un point final suivi de <c>”</c> comme
	/// absent. Les deux pièges sont consignés, et le contrôle inverse ci-dessous vérifie que
	/// l'écran <b>voit un 1</b> là où il y en a un.</para>
	/// </summary>
	public class VirtuesFinalPeriodsG11GuardTests
	{
		/// <summary>Ponctuation finale des six langues corrigées (et du fr de référence).</summary>
		private const string LatinPeriod = ".";

		/// <summary>Ponctuation finale du chinois — convention propre au corpus, pas une traduction.</summary>
		private const string CjkPeriod = "。";

		private const int CorpusRows = 223;
		private const int DeckRows = 131;

		/// <summary>
		/// Les six langues corrigées par ce grain. <c>en</c> en est <b>exclu</b> : ses 13 cellules
		/// sont portées par #1795 (grain 10).
		/// </summary>
		private static readonly string[] CorrectedLanguages = { "ru", "pt", "es", "ar", "fa", "zh" };

		/// <summary>
		/// Worklist exacte du grain : les 64 cellules qui ne portaient pas leur ponctuation
		/// finale. Les rangées hors deck (pk 3, 11, 19, 30, 44) en font partie : la perte est
		/// née d'une passe partagée, elle a frappé le corpus entier, pas seulement l'imprimé.
		/// </summary>
		private static readonly (string Lang, string[] Pks)[] CorrectedWorklist =
		{
			("ru", new[] { "8", "12", "24", "144" }),
			("pt", new[] { "2", "3", "4", "6", "7", "8", "11", "12", "24", "33", "34", "144", "209" }),
			("es", new[] { "2", "3", "4", "6", "7", "8", "11", "12", "24", "44", "144", "220", "221" }),
			("ar", new[] { "3", "4", "5", "6", "7", "8", "10", "11" }),
			("fa", new[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "19", "28", "29", "30", "31", "32", "33" }),
			("zh", new[] { "3", "4", "6", "7", "8", "9", "10", "11" }),
		};

		private const int CorrectedCellCount = 64;

		/// <summary>
		/// Épingles SHA-256 du <b>noyau structurel</b> — les rangées que 6 ou 7 langues perdent
		/// ensemble (pk 4, 6, 7, 8). Empreintes <b>émises depuis la mesure</b> sur le contenu
		/// corrigé, jamais recopiées : elles figent le texte qui a reçu le point, pas seulement
		/// sa présence — sans quoi « une cellule réparée contre une autre abîmée » passerait.
		/// </summary>
		private static readonly (string Pk, string Lang, string Sha256)[] CorePins =
		{
			("4", "ru", "a0083df12d2cb5bebbdfa2d5a6b59a3bc59fd1c7da996aacb447fc1fc5948b4a"),
			("4", "pt", "b920acf7e4de2886933cd981758ca0e4be36f629fe10914f92f259ec3d26e464"),
			("4", "es", "80835d7ef55f0f3af27c4d595a9404c22fecc1dc45b2e8123e7110cba8368bcf"),
			("4", "ar", "c8bb92dca3e184dcb16f025ec5edf474d4b5161d06428f37b794f079a029d01d"),
			("4", "fa", "3e6c985564524dcdc79e45a97a57ef970bcc06267ed9a90e60a5da6a96b09d8d"),
			("4", "zh", "ec700c6685e151475bbfa201e3c14a8513e262e5ac8e545bb2c98c4b73eba1a0"),
			("6", "ru", "b6776dce31d0dc24c83efb215a80a552f0ef600899885fdb161f574bf3afc01e"),
			("6", "pt", "d877aee178d6739dd3e88f2c76d9553bf75f12871e03f323a5c3fa1fced51275"),
			("6", "es", "f25afb8756c38c9aa04e9121b48ee98d497dac1ec1519f8b84ef2a3c13a01483"),
			("6", "ar", "09e5a596a662b28ca77a893556494c8ec195518ccc8c3d2b8c9d41e67509d01f"),
			("6", "fa", "b1f4eca830215c932b0785fc23e99d2f4e1dfde6e934e683f234313f7d21fc96"),
			("6", "zh", "7f7a777091ac7459ea35b413feddd2722db82f7e3ef9555d49d40bb2a04025b9"),
			("7", "ru", "f5bbbf53ef85df5cb9a6adf04ebfff160f443fedcca210756f2ce615c69a4272"),
			("7", "pt", "54be631eaa0b6231ea1f17ceeef9361cccec7ae0c45dca5c3c8ccaf413878518"),
			("7", "es", "8285aaaee1fb56e2758e63d5bb45e4fcbe62cba34ff0b7586e6314b47938a28e"),
			("7", "ar", "481fb046ef1e3c4a3aaf648da539f4b0b46504c49cbf269e45e07c4a800a540f"),
			("7", "fa", "59b490f8a604f585cf0a2d17b835e52cb2cd0715d4dc3ac9b685ac1b64e218ce"),
			("7", "zh", "41a0e17291f6cd4104b04f1cf38f1e018a54e745211c801c2624ef51f72904e5"),
			("8", "ru", "5f5301cf9dc6ab1d3514d50e947266327e95bf52ab0f19232e1d63f2b5d94e6a"),
			("8", "pt", "955e3a49e8531fc571764c96cc5a98f9d91e8410fc9f089a44946e4051ada498"),
			("8", "es", "d17af7e7b8f157c7cb8be7f09f906244ef4d142051a41af34b1b59d7ca9253ec"),
			("8", "ar", "1313b7a38dc57bfe480757679f1848c5c7dbee93ec7ce1ba8a3c0395d99a0456"),
			("8", "fa", "3e0b2eda7cba8be8061dc887dc3614f8bdc83fcd5917afa4542330da56945a4d"),
			("8", "zh", "b9c8a21fce9082b7b6589913e36fdb258a17f1ca4f79692e1be9aaadc7b2df13"),
		};

		private static string VirtuesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");

		private static Dictionary<string, Dictionary<string, string>> LoadRowsByPk()
		{
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				BadDataFound = null,
				HeaderValidated = null,
			};
			using var reader = new StringReader(File.ReadAllText(VirtuesCsv));
			using var csv = new CsvReader(reader, config);
			var rows = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
			foreach (var record in csv.GetRecords<dynamic>())
			{
				var dict = (IDictionary<string, object>)record;
				var row = dict.ToDictionary(kv => kv.Key, kv => kv.Value?.ToString() ?? string.Empty, StringComparer.Ordinal);
				var pk = row.GetValueOrDefault("pk")?.Trim();
				if (!string.IsNullOrEmpty(pk))
					rows[pk] = row;
			}
			rows.Should().HaveCount(CorpusRows, "le CSV Vertus compte 223 rangées.");
			return rows;
		}

		/// <summary>
		/// L'écran : une description non vide se termine-t-elle par la ponctuation de SA langue ?
		/// Le <c>zh</c> exige <c>。</c> — un point latin n'y vaut pas la convention du corpus.
		/// </summary>
		private static bool HasTerminalPunctuation(string lang, string? text)
		{
			var t = (text ?? string.Empty).Trim();
			if (t.Length == 0)
				return true;
			var expected = lang == "zh" ? CjkPeriod : LatinPeriod;
			return t.EndsWith(expected, StringComparison.Ordinal);
		}

		private static string Sha256Hex(string? value)
		{
			using var sha = SHA256.Create();
			var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value ?? string.Empty));
			var sb = new StringBuilder(bytes.Length * 2);
			foreach (var b in bytes)
				sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
			return sb.ToString();
		}

		[Fact]
		public void Every_Description_Ends_With_Its_Language_Punctuation()
		{
			var rows = LoadRowsByPk();
			rows.Values.Count(r => !string.IsNullOrWhiteSpace(r.GetValueOrDefault("card")))
				.Should().Be(DeckRows, "le deck des Vertus compte 131 cartes — anti-vacuité.");

			// Le fr est le témoin : il portait déjà le point partout AVANT ce grain.
			// L'invariant vérifié ici est donc celui du corpus lui-même, pas une nouveauté.
			var languages = new[] { "fr" }.Concat(CorrectedLanguages).ToArray();
			foreach (var lang in languages)
			{
				var column = "description_" + lang;
				var missing = rows
					.Where(kv => !HasTerminalPunctuation(lang, kv.Value.GetValueOrDefault(column)))
					.Select(kv => kv.Key)
					.OrderBy(pk => int.Parse(pk, CultureInfo.InvariantCulture))
					.ToArray();

				missing.Should().BeEmpty(
					$"grain 11 : aucune description [{lang}] ne doit rester sans ponctuation finale — " +
					"la famille a été mesurée comme UNE seule, perdue à une passe partagée (noyau pk 4/6/7/8). " +
					$"Rangées fautives : {string.Join(", ", missing)}");

				var punctuated = rows.Values.Count(r => HasTerminalPunctuation(lang, r.GetValueOrDefault(column)));
				punctuated.Should().Be(CorpusRows,
					$"223/223 descriptions [{lang}] se terminent par « {(lang == "zh" ? CjkPeriod : LatinPeriod)} » — " +
					"mesuré après correction, comme le fr de référence.");
			}
		}

		[Fact]
		public void The_SixtyFour_Corrected_Cells_Carry_Their_Punctuation()
		{
			var rows = LoadRowsByPk();
			var seen = 0;

			foreach (var (lang, pks) in CorrectedWorklist)
			{
				CorrectedLanguages.Should().Contain(lang, "seules les six langues de ce grain sont couvertes.");
				foreach (var pk in pks)
				{
					var cell = rows[pk].GetValueOrDefault("description_" + lang);
					HasTerminalPunctuation(lang, cell).Should().BeTrue(
						$"pk {pk} [{lang}] : cellule réparée par le grain 11 — « …{Tail(cell)} »");
					seen++;
				}
			}

			seen.Should().Be(CorrectedCellCount,
				"le grain 11 a réparé exactement 64 cellules (4 ru · 13 pt · 13 es · 8 ar · 18 fa · 8 zh).");
		}

		[Fact]
		public void Core_Rows_Are_Frozen_By_Sha256()
		{
			var rows = LoadRowsByPk();
			foreach (var (pk, lang, expected) in CorePins)
			{
				Sha256Hex(rows[pk].GetValueOrDefault("description_" + lang)).Should().Be(expected,
					$"noyau grain 11 : pk {pk} [{lang}] est une des rangées que 6 ou 7 langues perdent " +
					"ensemble — son texte corrigé est figé, pas seulement sa ponctuation.");
			}
			CorePins.Should().HaveCount(24, "4 rangées du noyau (4/6/7/8) × 6 langues corrigées.");
		}

		[Fact]
		public void This_Grain_Does_Not_Touch_English()
		{
			// Frontière déclarée avec #1795 (grain 10, 13 cellules description_en du même défaut).
			// Sans cet écran, la fusion de #1795 ferait rougir une garde qui n'a rien à y voir.
			CorrectedWorklist.SelectMany(w => w.Pks.Select(_ => w.Lang))
				.Should().NotContain("en", "les cellules description_en sont portées par #1795, pas par ce grain.");
			CorrectedLanguages.Should().NotContain("en");
			CorePins.Select(p => p.Lang).Should().NotContain("en");
		}

		[Fact]
		public void The_Screen_Sees_A_One_Where_There_Is_One()
		{
			// Contrôle INVERSE : un écran qui n'a jamais vu de cellule fautive ne prouve rien.
			// On lui presente une cellule privee de son point, et on exige qu'il la nomme.
			"Un raisonnement rigoureux avance etape par etape".Should().NotMatch(".*[.]$");
			HasTerminalPunctuation("ru", "Уместное сходство делает идею понятной").Should().BeFalse(
				"l'écran doit signaler une description ru sans point final.");
			HasTerminalPunctuation("zh", "通过具体例子支持论证").Should().BeFalse(
				"l'écran doit signaler une description zh sans 。");
			HasTerminalPunctuation("zh", "通过具体例子支持论证.").Should().BeFalse(
				"en zh, la convention est 。 : un point latin ne la satisfait pas — c'est le corpus qui le dit (215/223 avant correction).");

			// Et le témoin positif : ce que l'écran accepte, il l'accepte pour la bonne raison.
			HasTerminalPunctuation("ru", "…делающими её более понятной.").Should().BeTrue();
			HasTerminalPunctuation("zh", "…使其更易理解。").Should().BeTrue();

			// Sonde de non-cécité : l'écran voit un « 1 » dans le corpus réel.
			var rows = LoadRowsByPk();
			rows.Values.Count(r => (r.GetValueOrDefault("description_zh") ?? string.Empty).Trim().EndsWith(CjkPeriod, StringComparison.Ordinal))
				.Should().Be(CorpusRows, "223/223 descriptions zh portent 。 — l'écran voit bien la ponctuation qu'il cherche.");
		}

		private static string Tail(string? value)
		{
			var t = (value ?? string.Empty).Trim();
			return t.Length <= 34 ? t : t.Substring(t.Length - 34);
		}
	}
}
