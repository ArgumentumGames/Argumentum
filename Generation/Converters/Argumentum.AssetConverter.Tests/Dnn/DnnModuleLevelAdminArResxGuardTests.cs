using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// #458 grain module-level — lot <c>admin</c> ar-SA : mêmes 27 fichiers /
    /// 328 clés que zh-CN (PR #2088), sous-classe du socle abstrait (motif
    /// <c>controls</c>, décision du 31/10). La seule règle propre à l'arabe est
    /// la couverture d'écriture : le contrôle « ≠ anglais » du socle n'attrape
    /// que les copies INTÉGRALES — une valeur à moitié traduite (le libellé
    /// traduit, le reste du texte laissé en anglais) passe inaperçue. La
    /// présence de l'écriture arabe dans chaque valeur la rattrape (motif
    /// prouvé par le contrôle inverse latin-non-anglais du lot controls ar-SA,
    /// PR #2092).
    /// </summary>
    public class DnnModuleLevelAdminArResxGuardTests : DnnModuleLevelAdminResxGuardBase
    {
        /// <inheritdoc />
        protected override string Culture => "ar-SA";

        private static readonly Regex ArabicScript = new(@"[؀-ۿ]", RegexOptions.Compiled);

        [Theory]
        [MemberData(nameof(DnnModuleLevelAdminResxGuardBase.CoveredFiles), MemberType = typeof(DnnModuleLevelAdminResxGuardBase))]
        public void EveryValueCarriesArabicScript(string dir, string stem, int _)
        {
            foreach (var (key, value) in Values(dir, stem, Culture))
                ArabicScript.IsMatch(value).Should().BeTrue(
                    $"{RelOf(dir, stem, Culture)}:{key} — la valeur ne porte aucun caractère arabe ; "
                    + $"valeur = « {value} » (une valeur à moitié traduite passe le contrôle « ≠ anglais » du socle)");
        }

        [Fact]
        public void TheArabicLotDeclaresNoVerbatimValues()
        {
            // Le socle épingle VerbatimByDesign à vide pour TOUTE culture (y
            // compris zh) : ce fait nomme explicitement la parité — le lot ar
            // ne comporte aucune valeur identique à l'anglais par conception,
            // comme le lot zh dont il partage le périmètre exact.
            VerbatimByDesign.Should().BeEmpty(
                "le lot admin ar-SA ne comporte aucune copie de l'anglais par conception, parité avec zh-CN");
        }
    }
}
