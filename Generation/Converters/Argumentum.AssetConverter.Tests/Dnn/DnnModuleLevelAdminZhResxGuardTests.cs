using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// #458 grain module-level — lot <c>admin</c> zh-CN (PR #2088, 27 fichiers,
    /// 328 clés). Toutes les règles vivent dans le socle abstrait (motif acté
    /// pour <c>controls</c>, décision du 31/10) : cette classe ne porte que
    /// l'identité culturelle du lot — un lot suivant est une sous-classe, pas
    /// une copie des règles.
    /// <para>
    /// Le contrôle d'écriture (lot du 31/10) est la seule règle propre à la
    /// culture : le contrôle « ≠ anglais » du socle n'attrape que les copies
    /// INTÉGRALES — une valeur à moitié traduite passe inaperçue. Aucune
    /// exemption n'est nécessaire ici : les 328 valeurs du pack portent toutes
    /// de l'écriture chinoise (mesuré, 0 valeur latine hors périmètre déclaré).
    /// </para>
    /// </summary>
    public class DnnModuleLevelAdminZhResxGuardTests : DnnModuleLevelAdminResxGuardBase
    {
        /// <inheritdoc />
        protected override string Culture => "zh-CN";

        /// <summary>
        /// Bloc CJK unifié : Ext-A (U+3400–U+4DBF), unifié (U+4E00–U+9FFF) et
        /// formes de compatibilité (U+F900–U+FAFF).
        /// </summary>
        private static readonly Regex ChineseScript =
            new(@"[\u3400-\u4DBF\u4E00-\u9FFF\uF900-\uFAFF]", RegexOptions.Compiled);

        [Theory]
        [MemberData(nameof(DnnModuleLevelAdminResxGuardBase.CoveredFiles), MemberType = typeof(DnnModuleLevelAdminResxGuardBase))]
        public void EveryValueCarriesChineseScript(string dir, string stem, int _)
        {
            foreach (var (key, value) in Values(dir, stem, Culture))
            {
                if (VerbatimByDesign.Contains((dir, stem, key)))
                    continue;

                ChineseScript.IsMatch(value).Should().BeTrue(
                    $"{RelOf(dir, stem, Culture)}:{key} — la valeur ne porte aucun caractère chinois ; "
                    + $"valeur = « {value} » (une valeur à moitié traduite passe le contrôle « ≠ anglais » du socle)");
            }
        }
    }
}
