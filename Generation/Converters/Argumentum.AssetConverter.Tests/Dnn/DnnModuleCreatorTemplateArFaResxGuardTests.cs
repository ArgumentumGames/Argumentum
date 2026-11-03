using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 grain « 12 clés ModuleCreator » — les gabarits <b>embarqués</b> du
	/// ModuleCreator (<c>Admin/ModuleCreator/Templates/</c>), distincts des deux
	/// contrôles module-level <c>CreateModule.ascx</c> / <c>ViewSource.ascx</c> que
	/// portent déjà <c>DnnModuleLevelDtmResidual*ResxGuardTests</c> : ceux-là vivent à
	/// la racine du module, ceux-ci sous <c>Templates/</c> et n'étaient couverts par
	/// aucune garde.
	/// <para>
	/// Périmètre : 6 gabarits × 2 cultures = 12 fichiers, 2 clés chacun
	/// (<c>plField.Text</c>, <c>plField.Help</c>) ⇒ <b>12 clés</b> au sens du pool
	/// (compte des fichiers produits), 24 cellules.
	/// </para>
	/// <para>
	/// ⚠️ <b>Le fichier doit être committé AVANT d'être mesuré</b> : la garde lit
	/// <c>HEAD:&lt;path&gt;</c> via <see cref="DnnResxGuardSocle.CommittedBlob"/>, jamais
	/// l'arbre de travail — sur Windows <c>core.autocrlf</c> réécrit les fins de ligne
	/// au checkout, si bien qu'une garde qui lit le disque mesure deux objets
	/// différents selon l'hôte. Conséquence assumée : un ajout non committé rend un
	/// rouge bruyant (<c>git cat-file</c> exit 128), pas un vert optimiste.
	/// </para>
	/// </summary>
	public class DnnModuleCreatorTemplateArFaResxGuardTests
	{
		private const string NeutralScript = "Field:";

		private static readonly string TemplatesRoot =
			"DNNPlatform/DesktopModules/Admin/ModuleCreator/Templates";

		private static readonly string[] Cultures = { "ar-SA", "fa-IR" };

		/// <summary>Écriture arabe (le persan y est inclus : même bloc Unicode).</summary>
		private static readonly Regex ArabicScript = new(@"[؀-ۿ]", RegexOptions.Compiled);

		/// <summary>Les deux gabarits ascx de chaque langage, et les deux extensions.</summary>
		public static IEnumerable<object[]> Templates => new[]
		{
			new object[] { "C#/Module - Inline Script", "template.ascx" },
			new object[] { "C#/Module - User Control", "template.ascx" },
			new object[] { "C#/Settings - User Control", "Settings.ascx" },
			new object[] { "VB/Module - Inline Script", "template.ascx" },
			new object[] { "VB/Module - User Control", "template.ascx" },
			new object[] { "VB/Settings - User Control", "Settings.ascx" },
		};

		private static string Rel(string dir, string stem, string? culture = null)
		{
			var name = culture == null ? stem + ".resx" : stem + "." + culture + ".resx";
			return TemplatesRoot + "/" + dir + "/" + name;
		}

		private sealed record Resx(byte[] Raw, string Text, IReadOnlyList<string> Keys, IReadOnlyDictionary<string, string> Values)
		{
			public bool HasBom => Raw.Length >= 3 && Raw[0] == 0xEF && Raw[1] == 0xBB && Raw[2] == 0xBF;
			public int CarriageReturns => Raw.Count(b => b == 13);
		}

		private static Resx Read(string rel)
		{
			var raw = DnnResxGuardSocle.CommittedBlob(rel);
			var hasBom = raw.Length >= 3 && raw[0] == 0xEF && raw[1] == 0xBB && raw[2] == 0xBF;
			var text = new UTF8Encoding(false).GetString(hasBom ? raw.Skip(3).ToArray() : raw);

			var doc = XDocument.Parse(text); // lève sur XML invalide : c'est le point
			var root = doc.Root
				?? throw new InvalidDataException($"{rel} : resx sans élément racine");
			root.Name.LocalName.Should().Be("root", $"{rel} : racine resx attendue");

			static string NameOf(XElement data, string rel) =>
				data.Attribute("name")?.Value
				?? throw new InvalidDataException($"{rel} : <data> sans attribut name");

			var keys = root.Elements("data").Select(e => NameOf(e, rel)).ToList();
			var values = root.Elements("data").ToDictionary(
				e => NameOf(e, rel),
				e => e.Element("value")?.Value
					?? throw new InvalidDataException($"{rel}:{NameOf(e, rel)} — <data> sans <value>"));

			return new Resx(raw, text, keys, values);
		}

		[Theory]
		[MemberData(nameof(Templates))]
		public void EachTemplateCarriesBothCulturesAsCommittedResx(string dir, string stem)
		{
			var neutral = Read(Rel(dir, stem));

			// `because:` nommé à dessein : sans lui, les trois chaînes se lient comme UNE
			// collection attendue (les deux clés + le message), et l'assertion compare
			// 2 clés lues à 3 éléments — un rouge qui ne dit rien du resx.
			neutral.Keys.Should().BeEquivalentTo(new[] { "plField.Text", "plField.Help" },
				because: $"{Rel(dir, stem)} : le neutre porte exactement les deux clés du gabarit");

			foreach (var culture in Cultures)
			{
				var rel = Rel(dir, stem, culture);
				var resx = Read(rel);

				resx.HasBom.Should().BeTrue(
					$"{rel} : BOM attendu (le neutre en porte un ; un resx sans BOM casse les parseurs DNN)");

				resx.CarriageReturns.Should().Be(0,
					$"{rel} : {resx.CarriageReturns} CR — le blob committé doit être en LF seul, comme le neutre");

				resx.Keys.Should().BeEquivalentTo(neutral.Keys,
					because: $"{rel} : les clés doivent être exactement celles du neutre (ni perdue, ni inventée)");

				foreach (var key in neutral.Keys)
				{
					var value = resx.Values[key];

					value.Should().NotBeNullOrWhiteSpace($"{rel}:{key} — valeur vide");
					value.Should().NotBe(neutral.Values[key],
						$"{rel}:{key} — la valeur est la copie de l'anglais « {neutral.Values[key]} »");
					ArabicScript.IsMatch(value).Should().BeTrue(
						$"{rel}:{key} — aucun caractère arabe/persan ; valeur = « {value} »");
				}
			}
		}

		[Theory]
		[MemberData(nameof(Templates))]
		public void TheArabicSiblingCarriesNoPersianOnlyLetter(string dir, string stem)
		{
			// Le persan s'écrit dans le MÊME bloc Unicode que l'arabe : un contrôle borné
			// au bloc accepterait à 100 % une copie persane. Ces gabarits disent « الحقل »
			// (vocabulaire déjà employé par EditProfileDefinition.ascx.ar-SA.resx) et ne
			// doivent porter aucune des six lettres propres au persan.
			var rel = Rel(dir, stem, "ar-SA");
			foreach (var (key, value) in Read(rel).Values)
				PersianScriptMarkers.PersianOnly.IsMatch(value).Should().BeFalse(
					$"{rel}:{key} — lettre propre au persan dans le frère arabe ; valeur = « {value} »");
		}

		[Fact]
		public void ThePersianLotIsPersianAndTheArabicLotIsNot()
		{
			// Critère de LOT, jamais par valeur (voir DnnPersianScriptMarkers) : ces deux
			// clés sont courtes et peuvent légitimement n'avoir aucune lettre propre —
			// c'est la comparaison des deux lots qui décide. Rejoué à chaque exécution :
			// sans lui, un frère persan copié de l'arabe resterait vert.
			var fa = CulturesValues("fa-IR");
			var ar = CulturesValues("ar-SA");

			var faPersian = fa.Count(PersianScriptMarkers.PersianOnly.IsMatch);
			var faArabic = fa.Count(PersianScriptMarkers.ArabicOnly.IsMatch);
			var arPersian = ar.Count(PersianScriptMarkers.PersianOnly.IsMatch);
			var arArabic = ar.Count(PersianScriptMarkers.ArabicOnly.IsMatch);

			faPersian.Should().BeGreaterThan(0,
				"le lot persan ne porte aucune lettre propre au persan : il est indistinguable d'une copie arabe");
			faArabic.Should().Be(0,
				$"le lot persan porte {faArabic} valeur(s) avec ي / ك / ى, que le persan n'emploie pas");

			// Contrôle inverse : le MÊME instrument, appliqué au lot arabe, doit rendre
			// l'autre verdict — sinon il ne mesure rien.
			arArabic.Should().BeGreaterThan(0,
				"le lot arabe ne porte aucune lettre propre à l'arabe : le second critère ne serait pas exercé");
			arPersian.Should().Be(0,
				$"le lot arabe porte {arPersian} valeur(s) avec une lettre persane — les deux lots seraient confondus");
		}

		[Fact]
		public void TheScriptPredicatesRejectTheNeutral()
		{
			// Contrôle inverse du contrôle : les prédicats doivent rendre FAUX sur
			// l'anglais. Un prédicat qui accepte « Field: » ne prouve rien sur l'arabe.
			const string English = "Field level help.";
			ArabicScript.IsMatch(English).Should().BeFalse();
			PersianScriptMarkers.PersianOnly.IsMatch(English).Should().BeFalse();
			PersianScriptMarkers.ArabicOnly.IsMatch(English).Should().BeFalse();

			// …et VRAI sur les valeurs livrées, dans le même passage.
			ArabicScript.IsMatch("الحقل:").Should().BeTrue();
			PersianScriptMarkers.PersianOnly.IsMatch("فیلد:").Should().BeTrue();
			PersianScriptMarkers.ArabicOnly.IsMatch("تعليمات").Should().BeTrue();
			ArabicScript.IsMatch(NeutralScript).Should().BeFalse();
		}

		private static List<string> CulturesValues(string culture)
		{
			var values = new List<string>();
			foreach (var row in Templates)
			{
				var (dir, stem) = ((string)row[0], (string)row[1]);
				values.AddRange(Read(Rel(dir, stem, culture)).Values.Values);
			}
			return values;
		}
	}
}
