using System;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 grain module-level — pack <c>controls</c> **zh-CN** (lot 1, PR #2087).
	/// Les règles du contrat vivent dans <see cref="DnnModuleLevelControlsResxGuardBase"/> ;
	/// cette classe ne déclare que sa culture, pour que le lot ar-SA (et fa-IR
	/// ensuite) les partage au lieu de les recopier.
	/// <para>
	/// Le contrôle d'écriture (lot du 31/10) est la seule règle propre à la
	/// culture : il voit les valeurs qui diffèrent de l'anglais tout en n'étant
	/// pas chinoises (espacement, ponctuation, marque latine, anglais
	/// reformulé) — celles-là traversent le contrôle « ≠ anglais » du socle.
	/// Une valeur à moitié traduite porte déjà un caractère chinois : ce
	/// contrôle ne la voit pas, et ne prétend pas la voir.
	/// </para>
	/// <para>
	/// Détail du lot, contrôles indépendants et contrôles inverses :
	/// <c>docs/dnn-localization/458-item9bis-controls-zh-2026-10-30.md</c>.
	/// </para>
	/// </summary>
	public class DnnModuleLevelControlsZhResxGuardTests : DnnModuleLevelControlsResxGuardBase
	{
		protected override string Culture => "zh-CN";

		/// <summary>
		/// Bloc CJK unifié : Ext-A (U+3400–U+4DBF), unifié (U+4E00–U+9FFF) et
		/// formes de compatibilité (U+F900–U+FAFF). Mesuré sur les 3 018 valeurs
		/// <c>*.zh-CN.resx</c> du dépôt : les 2 997 valeurs qui portent de l'écriture
		/// chinoise relèvent toutes du bloc unifié — 0 en Ext-A, 0 en compatibilité.
		/// Les deux plages hors bloc unifié sont une marge défensive, pas une
		/// nécessité mesurée.
		/// </summary>
		private static readonly Regex ChineseScript =
			new(@"[\u3400-\u4DBF\u4E00-\u9FFF\uF900-\uFAFF]", RegexOptions.Compiled);

		[Theory]
		[MemberData(nameof(CoveredFiles))]
		public void EveryNonVerbatimValueCarriesChineseScript(string stem, int _)
		{
			foreach (var (key, value) in Values(stem, Culture))
			{
				if (VerbatimByDesign.Contains((stem, key)))
					continue;

				ChineseScript.IsMatch(value).Should().BeTrue(
					$"{RelDir}/{stem}.{Culture}.resx:{key} — la valeur ne porte aucun caractère chinois ; "
					+ $"valeur = « {value} »");
			}
		}

		/// <summary>
		/// La liste d'exemption du contrôle d'écriture est celle des verbatim
		/// déclarés — et elle est ÉPINGLÉE : ajouter une exemption est une
		/// décision, jamais un constat (« HTML » et « URL » sont les deux seules
		/// valeurs du pack dont le contenu n'est pas de la prose à traduire).
		/// </summary>
		[Fact]
		public void TheChineseLotExemptsExactlyTheTwoVerbatimControls()
		{
			VerbatimByDesign.Select(v => v.Stem + ":" + v.Key).OrderBy(x => x, StringComparer.Ordinal)
				.Should().Equal(
					new[] { "TextEditor.ascx:Html.Text", "UrlTrackingControl.ascx:URL.Text" },
					"le contrôle d'écriture chinoise n'exempte que les deux verbatim déclarés du socle");
		}
	}
}
