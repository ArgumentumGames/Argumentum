using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// #458 grain module-level — lot <c>admin</c> fa-IR : mêmes 27 fichiers /
    /// 328 clés que zh-CN (PR #2088) et ar-SA (PR #2093), sous-classe du socle
    /// abstrait (motif <c>controls</c>, décision du 31/10). La seule règle
    /// propre au persan est la couverture d'écriture : le contrôle « ≠ anglais »
    /// du socle n'attrape que les copies INTÉGRALES — une valeur à moitié
    /// traduite (le libellé traduit, le reste du texte laissé en anglais) passe
    /// inaperçue. La présence de l'écriture persane dans chaque valeur la
    /// rattrape (motif prouvé par le contrôle inverse latin-non-anglais du lot
    /// controls fa-IR, PR #2096).
    /// <para>
    /// ⚠️ L'alphabet persan vit dans le bloc arabe U+0600–U+06FF (lettres
    /// partagées + پ چ ژ گ ی ک) : aucune seconde plage n'est nécessaire, et la
    /// règle ne recopie pas celle du lot ar — elle la remplace par culture.
    /// </para>
    /// </summary>
    public class DnnModuleLevelAdminFaResxGuardTests : DnnModuleLevelAdminResxGuardBase
    {
        /// <inheritdoc />
        protected override string Culture => "fa-IR";

        private static readonly Regex PersianScript = new(@"[؀-ۿ]", RegexOptions.Compiled);

        [Theory]
        [MemberData(nameof(DnnModuleLevelAdminResxGuardBase.CoveredFiles), MemberType = typeof(DnnModuleLevelAdminResxGuardBase))]
        public void EveryValueCarriesPersianScript(string dir, string stem, int _)
        {
            foreach (var (key, value) in Values(dir, stem, Culture))
                PersianScript.IsMatch(value).Should().BeTrue(
                    $"{RelOf(dir, stem, Culture)}:{key} — la valeur ne porte aucun caractère persan ; "
                    + $"valeur = « {value} » (une valeur à moitié traduite passe le contrôle « ≠ anglais » du socle)");
        }

        [Fact]
        public void ThePersianLotDeclaresNoVerbatimValues()
        {
            // Le socle épingle VerbatimByDesign à vide pour TOUTE culture (y
            // compris zh et ar) : ce fait nomme explicitement la parité — le lot
            // fa ne comporte aucune valeur identique à l'anglais par conception.
            VerbatimByDesign.Should().BeEmpty(
                "le lot admin fa-IR ne comporte aucune copie de l'anglais par conception, parité avec zh-CN et ar-SA");
        }
    }
}
