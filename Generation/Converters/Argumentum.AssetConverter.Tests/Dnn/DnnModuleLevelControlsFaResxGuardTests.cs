using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// #458 grain module-level — pack <c>controls</c> **fa-IR** (troisième culture du
    /// grain, après zh-CN et ar-SA). Mêmes 11 fichiers / 133 clés, même contrat : les
    /// règles sont héritées de <see cref="DnnModuleLevelControlsResxGuardBase"/> et ne
    /// sont pas recopiées.
    /// <para>
    /// Détail du lot et contrôles indépendants :
    /// <c>docs/dnn-localization/458-item9bis-controls-fa-2026-10-31.md</c>.
    /// </para>
    /// </summary>
    public class DnnModuleLevelControlsFaResxGuardTests : DnnModuleLevelControlsResxGuardBase
    {
        protected override string Culture => "fa-IR";

        /// <summary>
        /// Écriture persane : l'alphabet persan vit dans le bloc Arabic
        /// (U+0600–U+06FF — lettres partagées avec l'arabe plus پ چ ژ گ ی ک
        /// propres au persan). Le test est donc le même intervalle que pour le
        /// lot ar-SA : toute valeur non déclarée verbatim doit porter l'écriture.
        /// </summary>
        private static readonly Regex PersianScript = new("[؀-ۿ]", RegexOptions.Compiled);

        /// <summary>
        /// Contrôle propre à cette culture, que le socle ne peut pas porter : la
        /// règle « valeur ≠ anglais » n'attrape qu'une copie **entière**. Une valeur
        /// moitié traduite (« Website: » → « Website: وب‌سایت ») passerait. Ici toute
        /// valeur non déclarée verbatim doit porter au moins un caractère persan —
        /// les deux valeurs verbatim (<c>HTML</c>, <c>URL</c>) sont les seules
        /// légitimement latines.
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void EveryNonVerbatimValueCarriesPersianScript(string stem, int _)
        {
            var localized = Values(stem, Culture);

            foreach (var (key, value) in localized)
            {
                if (VerbatimByDesign.Contains((stem, key)))
                    continue;

                PersianScript.IsMatch(value).Should().BeTrue(
                    $"{stem}.fa-IR:{key} — « {value} » ne porte aucun caractère persan : valeur non traduite "
                    + "ou copie partielle de l'anglais");
            }
        }

        [Fact]
        public void ThePersianPackDeclaresTheSameTwoVerbatimValuesAsTheZhLot()
        {
            VerbatimByDesign.Select(v => v.Stem + ":" + v.Key).OrderBy(x => x)
                .Should().Equal(new[] { "TextEditor.ascx:Html.Text", "UrlTrackingControl.ascx:URL.Text" },
                    "les deux seules valeurs latines légitimes sont les mêmes d'une culture à l'autre");
        }
    }
}
