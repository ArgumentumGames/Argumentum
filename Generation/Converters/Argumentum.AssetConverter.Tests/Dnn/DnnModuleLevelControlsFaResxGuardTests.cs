using System.Collections.Generic;
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
        /// Contrôle d'écriture propre à cette culture, que le socle ne peut pas
        /// porter : le « ≠ anglais » du socle compare au neutre, donc une valeur qui
        /// diffère de l'anglais **sans porter un seul caractère persan** (espacement,
        /// ponctuation, marque latine, anglais reformulé) le traverse. Ici toute
        /// valeur non déclarée verbatim doit porter au moins un caractère persan —
        /// les deux valeurs verbatim (<c>HTML</c>, <c>URL</c>) sont les seules
        /// légitimement latines.
        /// <para>
        /// Ce qu'il ne ferme PAS : une valeur à moitié traduite (« Website: » →
        /// « Website: وب‌سایت ») porte déjà un caractère persan, donc elle passe. Ce
        /// contrôle ne la voit pas et ne prétend pas la voir.
        /// </para>
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

        /// <summary>
        /// Valeurs du lot pour une culture : les clés livrées, moins les verbatim
        /// déclarés (les valeurs légitimement latines, qui n'ont rien à porter).
        /// </summary>
        private List<string> LotValues(string culture) =>
            Covered.SelectMany(c => Values(c.Stem, culture)
                    .Where(v => !VerbatimByDesign.Contains((c.Stem, v.Key)))
                    .Select(v => v.Value))
                .ToList();

        /// <summary>
        /// Fait de LOT : ce pack est du persan, pas une copie de l'arabe. Le contrôle
        /// d'écriture ci-dessus borne au bloc U+0600–U+06FF, commun aux deux langues —
        /// il accepterait une traduction arabe à 100 %. Deux mesures de lot l'en
        /// empêchent : un plancher de lettres propres au persan, et l'absence totale
        /// de ي / ك / ى.
        /// </summary>
        [Fact]
        public void TheLotIsPersianNotAnArabicCopy() =>
            PersianScriptMarkers.AssertLotIsPersian(LotValues(Culture), "controls/fa-IR");

        /// <summary>
        /// Contrôle inverse, rejoué à chaque exécution : le même instrument appliqué au
        /// lot <c>ar-SA</c> des mêmes fichiers doit échouer aux DEUX critères. Sans lui,
        /// un plancher trop bas resterait vert sur une copie arabe.
        /// </summary>
        [Fact]
        public void TheInstrumentRejectsTheArabicLot() =>
            PersianScriptMarkers.AssertInstrumentRejectsTheArabicLot(LotValues("ar-SA"), "controls/ar-SA");
    }
}
