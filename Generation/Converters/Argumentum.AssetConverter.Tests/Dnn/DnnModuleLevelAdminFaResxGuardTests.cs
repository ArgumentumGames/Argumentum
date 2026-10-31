using System.Collections.Generic;
using System.Linq;
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
    /// du socle compare au neutre, donc une valeur qui diffère de l'anglais
    /// **sans porter un seul caractère persan** (espacement, ponctuation, marque
    /// latine, anglais reformulé) le traverse — c'est ce trou-là que la règle
    /// ferme. Elle ne ferme PAS la traduction partielle : une valeur à moitié
    /// traduite porte déjà un caractère persan, donc elle passe, et la règle ne
    /// prétend pas la voir.
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

        /// <summary>
        /// Valeurs du lot pour une culture : les clés livrées, moins les verbatim
        /// déclarés (les valeurs légitimement latines, qui n'ont rien à porter).
        /// </summary>
        private List<string> LotValues(string culture) =>
            Covered.SelectMany(c => Values(c.Dir, c.Stem, culture)
                    .Where(v => !VerbatimByDesign.Contains((c.Dir, c.Stem, v.Key)))
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
            PersianScriptMarkers.AssertLotIsPersian(LotValues(Culture), "admin/fa-IR");

        /// <summary>
        /// Contrôle inverse, rejoué à chaque exécution : le même instrument appliqué au
        /// lot <c>ar-SA</c> des mêmes fichiers doit échouer aux DEUX critères. Sans lui,
        /// un plancher trop bas resterait vert sur une copie arabe.
        /// </summary>
        [Fact]
        public void TheInstrumentRejectsTheArabicLot() =>
            PersianScriptMarkers.AssertInstrumentRejectsTheArabicLot(LotValues("ar-SA"), "admin/ar-SA");
    }
}
