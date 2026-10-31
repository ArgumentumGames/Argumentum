using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 grain module-level — pack <c>controls</c> **ar-SA** (lot 3, après zh-CN).
	/// Mêmes 11 fichiers / 133 clés, même contrat : les règles sont héritées de
	/// <see cref="DnnModuleLevelControlsResxGuardBase"/> et ne sont pas recopiées.
	/// <para>
	/// Détail du lot et contrôles indépendants :
	/// <c>docs/dnn-localization/458-item9bis-controls-ar-2026-10-31.md</c>.
	/// </para>
	/// </summary>
	public class DnnModuleLevelControlsArResxGuardTests : DnnModuleLevelControlsResxGuardBase
	{
		protected override string Culture => "ar-SA";

		/// <summary>
		/// Écriture arabe (blocs Arabic, Arabic Supplement, Arabic Extended-A).
		/// </summary>
		private static readonly Regex ArabicScript = new(@"[؀-ۿ]", RegexOptions.Compiled);

		/// <summary>
		/// Contrôle propre à cette culture, que le socle ne peut pas porter : la
		/// règle « valeur ≠ anglais » n'attrape qu'une copie **entière**. Une valeur
		/// moitié traduite (« Website: » → « Website: الموقع ») passerait. Ici toute
		/// valeur non déclarée verbatim doit porter au moins un caractère arabe —
		/// les deux valeurs verbatim (<c>HTML</c>, <c>URL</c>) sont les seules
		/// légitimement latines.
		/// </summary>
		[Theory]
		[MemberData(nameof(CoveredFiles))]
		public void EveryNonVerbatimValueCarriesArabicScript(string stem, int _)
		{
			var localized = Values(stem, Culture);

			foreach (var (key, value) in localized)
			{
				if (VerbatimByDesign.Contains((stem, key)))
					continue;

				ArabicScript.IsMatch(value).Should().BeTrue(
					$"{stem}.ar-SA:{key} — « {value} » ne porte aucun caractère arabe : valeur non traduite "
					+ "ou copie partielle de l'anglais");
			}
		}

		[Fact]
		public void TheArabicPackDeclaresTheSameTwoVerbatimValuesAsTheZhLot()
		{
			VerbatimByDesign.Select(v => v.Stem + ":" + v.Key).OrderBy(x => x)
				.Should().Equal(new[] { "TextEditor.ascx:Html.Text", "UrlTrackingControl.ascx:URL.Text" },
					"les deux seules valeurs latines légitimes sont les mêmes d'une culture à l'autre");
		}
	}
}
