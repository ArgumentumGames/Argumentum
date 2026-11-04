using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// #458 lot 4a — garde du lot <c>DesktopModules</c> « résidu » fa-IR, jumeau
    /// persan du lot ar-SA livré par <c>DnnModuleLevelDtmResidualArResxGuardTests</c>
    /// (#2115) : les mêmes 37 fichiers neutres, la même référence (le NEUTRE).
    /// <para>
    /// Périmètre dérivé du doc
    /// <c>docs/dnn-localization/458-lot4a-fa-ir-base-arithmetique-2026-11-04.md</c>
    /// (fusion #2131) : candidate ⇔ neutre existe ET <c>.ar-SA.resx</c> existe ET
    /// <c>.fa-IR.resx</c> n'existe pas — 37 fichiers.
    /// </para>
    /// <para>
    /// 🔢 <b>Arithmétique : 696 clés, pas les 689 du doc.</b> Les 689 du doc étaient
    /// le compte AVANT la montée DNN 10.4.1 (02/11, #2123), qui a ajouté 7 clés au
    /// neutre (AddExistingModule +1, AddModule +1, ResourceManager +5). Le lot ar-SA
    /// figé ne les porte pas et les déclare en exception ; le pack fa-IR, créé après
    /// la montée, les couvre nativement — d'où <see cref="CoveredKeyTotal"/> = 696
    /// et l'égalité STRICTE au neutre ci-dessous, sans exception déclarée.
    /// </para>
    /// <para>
    /// ✍️ Écriture : le persan s'écrit dans le bloc arabe (U+0600–U+06FF) ; le lot
    /// doit porter les lettres propres au persan (plancher <see
    /// cref="PersianScriptMarkers.MinimumPersianShare"/>) et aucune lettre
    /// exclusive de l'arabe (ي U+064A, ك U+0643, ى U+0649). Mesuré à la livraison :
    /// 586/686 valeurs non verbatim à lettre persane (85,4 %), 0 lettre arabe
    /// exclusive.
    /// </para>
    /// </summary>
    public class DnnModuleLevelDtmResidualFaResxGuardTests
    {
        /// <summary>Couverture livrée : 37 fichiers, 696 clés, culture fa-IR.</summary>
        private static readonly (string Dir, string Stem, int DeliveredKeys)[] Covered =
        {
            ("Admin/Authentication", "Authentication.ascx", 3),
            ("Admin/Authentication", "Logoff.ascx", 1),
            ("Admin/Console", "Settings.ascx", 38),
            ("Admin/Console", "ViewConsole.ascx", 7),
            ("Admin/Dnn.EditBar", "AddExistingModule", 2),
            ("Admin/Dnn.EditBar", "AddModule", 4),
            ("Admin/Dnn.EditBar", "Common", 3),
            ("Admin/Dnn.EditBar", "EditBar", 1),
            ("Admin/Dnn.EditBar", "QuickAddModule", 2),
            ("Admin/Dnn.EditBar/Resources/ContentEditorManager", "SharedResources", 51),
            ("Admin/EditExtension", "AuthenticationEditor.ascx", 15),
            ("Admin/EditExtension", "EditExtension.ascx", 44),
            ("Admin/HtmlEditorManager", "ProviderConfiguration.ascx", 142),
            ("Admin/ModuleCreator", "CreateModule.ascx", 20),
            ("Admin/ModuleCreator", "ViewSource.ascx", 34),
            ("Admin/SearchResults", "ResultsSettings.ascx", 29),
            ("Admin/SearchResults", "SearchResults.ascx", 35),
            ("Admin/SearchResults", "SearchableModules", 10),
            ("Admin/Security", "DataConsent.ascx", 7),
            ("AuthenticationServices/Facebook", "Login.ascx", 3),
            ("AuthenticationServices/Facebook", "Settings.ascx", 1),
            ("AuthenticationServices/Google", "Login.ascx", 3),
            ("AuthenticationServices/Google", "Settings.ascx", 1),
            ("AuthenticationServices/Live", "Login.ascx", 3),
            ("AuthenticationServices/Live", "Settings.ascx", 1),
            ("AuthenticationServices/Twitter", "Login.ascx", 3),
            ("AuthenticationServices/Twitter", "Settings.ascx", 1),
            ("Connectors/Azure", "SharedResources", 14),
            ("Connectors/GoogleAnalytics", "SharedResources", 9),
            ("Connectors/GoogleTagManager", "SharedResources", 5),
            ("ResourceManager", "EditFolderMapping.ascx", 16),
            ("ResourceManager", "ResourceManager", 111),
            ("ResourceManager", "Settings.ascx", 5),
            ("ResourceManager", "View.ascx", 3),
            ("SiteExportImport", "ExportImport", 10),
            ("TelerikRemoval", "View.ascx", 45),
            ("ToSic.Sxc", "View.ascx", 14),
        };

        /// <summary>Total épinglé : la somme des <see cref="Covered"/>, jamais un littéral recopié.</summary>
        private static readonly int CoveredKeyTotal = Covered.Sum(c => c.DeliveredKeys);

        /// <summary>
        /// Valeurs légitimement identiques à l'anglais — déclarées, pas tolérées.
        /// Dix entrées, les MÊMES que le lot ar-SA : jetons de remplacement DNN
        /// (<c>[PortalRoot]</c>, <c>[UserFolder]</c> — traduits, ils cesseraient
        /// d'être des jetons), noms de marques (Facebook, Google, Live, Twitter) et
        /// noms de langages (<c>C#</c>, <c>Visual Basic</c>). Liste ÉPINGLÉE : un
        /// ajout pour faire passer un rouge rougit le fait de comptage.
        /// </summary>
        private static readonly (string Dir, string Stem, string Key)[] VerbatimByDesign =
        {
            ("Admin/HtmlEditorManager", "ProviderConfiguration.ascx", "PortalRoot.Text"),
            ("Admin/HtmlEditorManager", "ProviderConfiguration.ascx", "UserFolder.Text"),
            ("AuthenticationServices/Facebook", "Login.ascx", "Title.Text"),
            ("AuthenticationServices/Google", "Login.ascx", "Title.Text"),
            ("AuthenticationServices/Live", "Login.ascx", "Title.Text"),
            ("AuthenticationServices/Twitter", "Login.ascx", "Title.Text"),
            ("Admin/ModuleCreator", "CreateModule.ascx", "C#.Text"),
            ("Admin/ModuleCreator", "CreateModule.ascx", "VB.Text"),
            ("Admin/ModuleCreator", "ViewSource.ascx", "C#.Text"),
            ("Admin/ModuleCreator", "ViewSource.ascx", "VB.Text"),
        };

        /// <summary>
        /// Fichier neutre du périmètre dont le neutre ne porte AUCUNE clé : il n'y a
        /// rien à livrer, et un <c>.fa-IR.resx</c> vide n'aurait aucun sens. Déclaré,
        /// et le fait ci-dessous vérifie que son neutre est toujours vide — sans quoi
        /// la dispense deviendrait une traduction perdue.
        /// </summary>
        private const string DeclaredEmptyNeutral =
            "Admin/HtmlEditorManager/Controls/App_LocalResources/InvalidConfiguration.ascx";

        private static readonly Regex Placeholder = new(@"\{\d+\}", RegexOptions.Compiled);

        /// <summary>
        /// Identité d'un jeton DNN : namespace + chemin AVANT le « | ». La partie
        /// affichable qui suit est de la prose à traduire.
        /// </summary>
        private static readonly Regex Token = new(
            @"\[(?:Portal|User|Host|Profile|Membership|Custom|Date|Module|Tab|Site|Page|App):([^\]\|]*)",
            RegexOptions.Compiled);

        /// <summary>
        /// Bloc arabe (U+0600–U+06FF), dans lequel s'écrit aussi le persan. Volontairement
        /// PAS restreint aux lettres : les chiffres persans (۰-۹, U+06F0–U+06F9) et la
        /// ponctuation du bloc comptent aussi comme écriture localisée.
        /// <para>
        /// Ce que la règle ferme : une valeur qui diffère de l'anglais SANS porter
        /// aucun caractère du bloc. Ce qu'elle ne ferme PAS : une valeur à moitié
        /// traduite, qui porte déjà un caractère du bloc.
        /// </para>
        /// </summary>
        private static readonly Regex ArabicScript =
            new(@"[؀-ۿ]", RegexOptions.Compiled);

        private static readonly (string Dir, string Stem, string Key)[] ScriptExempt = VerbatimByDesign;

        private static string RelOf(string dir, string stem, string culture) =>
            $"DNNPlatform/DesktopModules/{dir}/App_LocalResources/{stem}{(culture.Length > 0 ? "." + culture : "")}.resx";

        private static string PathOf(string dir, string stem, string culture) =>
            Path.Combine(
                TestRepoRoot.Find(),
                RelOf(dir, stem, culture).Replace('/', Path.DirectorySeparatorChar));

        private static Dictionary<string, string> Values(string dir, string stem, string culture)
        {
            var path = PathOf(dir, stem, culture);
            File.Exists(path).Should().BeTrue(
                $"le resx doit exister dans le checkout ({RelOf(dir, stem, culture)})");
            return XDocument.Load(path).Root!.Elements("data").ToDictionary(
                d => (string)d.Attribute("name")!,
                d => (string?)d.Element("value") ?? string.Empty,
                StringComparer.Ordinal);
        }

        public static IEnumerable<object[]> CoveredFiles()
        {
            foreach (var (dir, stem, delivered) in Covered)
                yield return new object[] { dir, stem, delivered };
        }

        [Fact]
        public void TheLotCoversThirtySevenFilesAndSixHundredNinetySixKeys()
        {
            Covered.Should().HaveCount(37,
                "le lot fa-IR mesure 37 fichiers, les mêmes que les lots ar-SA et zh-CN livrés "
                + "avant lui — périmètre dérivé du doc 458-lot4a, pas l'étiquette du pool");
            CoveredKeyTotal.Should().Be(696,
                "somme des clés neutres des 37 fichiers APRÈS la montée 10.4.1 — les 689 du doc "
                + "d'arithmétique dataient d'avant ; les +7 (AddExistingModule +1, AddModule +1, "
                + "ResourceManager +5) sont couverts nativement par ce pack");
            Covered.Select(c => (c.Dir, c.Stem)).Should().OnlyHaveUniqueItems(
                "quatre stems portent le même nom dans des répertoires différents "
                + "(Login.ascx ×4 en AuthenticationServices, Settings.ascx ×3, View.ascx ×3) : le couple identifie");
        }

        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void PackCoversTheDeclaredKeyCount(string dir, string stem, int deliveredKeys)
        {
            Values(dir, stem, "fa-IR").Should().HaveCount(deliveredKeys,
                $"{RelOf(dir, stem, "fa-IR")} : le nombre de clés livrées a changé — mettre à jour la garde");
        }

        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void TheDeliveredKeySetIsExactlyTheNeutralOne(string dir, string stem, int _)
        {
            // AUCUNE exception déclarée, à la différence du jumeau ar-SA : ce pack est
            // créé APRÈS la montée DNN 10.4.1 (02/11, #2123) qui a ajouté 7 clés au
            // neutre — il les couvre. Une clé manquante est donc toujours une
            // traduction perdue, et une clé orpheline un vestige à retirer.
            Values(dir, stem, "fa-IR").Keys.Should().BeEquivalentTo(
                Values(dir, stem, string.Empty).Keys,
                $"{RelOf(dir, stem, "fa-IR")} : livré = neutre EXACTEMENT (les 7 clés ajoutées au neutre "
                + "par la montée 10.4.1 sont couvertes nativement — contrairement au lot ar-SA figé, "
                + "aucune exception déclarée ici) ; toute clé non livrée est une traduction perdue");
        }

        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void EveryDeliveredValueIsSaneAgainstTheNeutral(string dir, string stem, int _)
        {
            var neutral = Values(dir, stem, string.Empty);
            var localized = Values(dir, stem, "fa-IR");

            foreach (var (key, value) in localized)
            {
                value.Should().NotBeNullOrWhiteSpace($"{RelOf(dir, stem, "fa-IR")}:{key} — valeur vide");

                Placeholder.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
                    .Should().Equal(
                        Placeholder.Matches(neutral[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
                        $"{RelOf(dir, stem, "fa-IR")}:{key} — multiset des placeholders modifié");

                Token.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
                    .Should().Equal(
                        Token.Matches(neutral[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
                        $"{RelOf(dir, stem, "fa-IR")}:{key} — identité des jetons DNN modifiée");

                if (VerbatimByDesign.Contains((dir, stem, key)))
                    continue;

                value.Should().NotBe(neutral[key],
                    $"{RelOf(dir, stem, "fa-IR")}:{key} — copie de l'anglais par accident (une copie VOLONTAIRE "
                    + "se déclare dans VerbatimByDesign)");
            }
        }

        /// <summary>
        /// Contrôle d'écriture : chaque valeur livrée non exemptée porte au moins un
        /// caractère du bloc arabe (dans lequel s'écrit le persan). Mesuré à la
        /// livraison : 686/686 valeurs non verbatim.
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void EveryNonExemptValueCarriesArabicBlockScript(string dir, string stem, int _)
        {
            foreach (var (key, value) in Values(dir, stem, "fa-IR"))
            {
                if (ScriptExempt.Contains((dir, stem, key)))
                    continue;
                ArabicScript.IsMatch(value).Should().BeTrue(
                    $"{RelOf(dir, stem, "fa-IR")}:{key} — la valeur ne porte aucun caractère du bloc arabe ; "
                    + $"valeur = « {value} »");
            }
        }

        /// <summary>
        /// ⛔ Aucune lettre exclusive de l'arabe (ي U+064A, ك U+0643, ى U+0649) dans
        /// une valeur persane — règle par valeur, plus forte que le plancher de lot :
        /// <c>ک</c> (U+06A9) et <c>ك</c> (U+0643) sont presque indiscernables à l'œil,
        /// une coquille de clavier arabe serait invisible à la relecture.
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void NoDeliveredValueUsesArabicOnlyLetters(string dir, string stem, int _)
        {
            foreach (var (key, value) in Values(dir, stem, "fa-IR"))
            {
                PersianScriptMarkers.ArabicOnly.IsMatch(value).Should().BeFalse(
                    $"{RelOf(dir, stem, "fa-IR")}:{key} — lettre exclusive de l'arabe (ي / ك / ى) dans un pack "
                    + $"fa-IR ; valeur = « {value} »");
            }
        }

        /// <summary>
        /// Le lot est persan : plancher de lettres persanes ET zéro lettre arabe
        /// exclusive (fait de LOT via <see cref="PersianScriptMarkers"/>). Mesuré à
        /// la livraison : 586/686 (85,4 %).
        /// </summary>
        [Fact]
        public void TheLotIsPersan()
        {
            PersianScriptMarkers.AssertLotIsPersian(
                Covered.SelectMany(c => Values(c.Dir, c.Stem, "fa-IR").Values).ToList(),
                "DesktopModules résidu fa-IR (lot 4a)");
        }

        /// <summary>
        /// Contrôle INVERSE, rejoué à chaque exécution : le même instrument, appliqué
        /// au lot ar-SA des mêmes fichiers, doit conclure « ce n'est pas du persan »
        /// sur les DEUX critères. Sans lui, un instrument devenu aveugle resterait
        /// vert et personne ne le saurait.
        /// </summary>
        [Fact]
        public void ThePersianInstrumentRejectsTheArabicTwinLot()
        {
            PersianScriptMarkers.AssertInstrumentRejectsTheArabicLot(
                Covered.SelectMany(c => Values(c.Dir, c.Stem, "ar-SA").Values).ToList(),
                "DesktopModules résidu ar-SA (jumeau du lot 4a)");
        }

        /// <summary>
        /// Le BOM du fichier produit suit celui de SON neutre — le module-level est
        /// hétérogène : dans ce lot, la majorité des neutres porte le BOM et une
        /// minorité n'en porte pas. ⚠️ Le partage n'est pas cité ici comme un compte :
        /// il a déjà été publié faux deux fois (30/7 sur le lot zh, puis 30/7 encore
        /// sur le lot 4a — mesuré 28/9 le 04/11 par une contre-revue), parce qu'un
        /// chiffre recopié dérive. La règle est l'égalité par fichier, que cette
        /// théorie asserte ; le compte, lui, se re-mesure et ne s'épingle pas.
        /// <para>
        /// ⚠️ Transport mesuré sur le <b>blob committé</b> (<c>git cat-file</c>), jamais
        /// sur l'arbre de travail : le runner CI a <c>core.autocrlf=true</c>, qui
        /// convertit LF → CRLF au checkout.
        /// </para>
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void BomMirrorsTheNeutralAndLineEndingsAreLf(string dir, string stem, int _)
        {
            var neutral = CommittedBlob(RelOf(dir, stem, string.Empty));
            var produced = CommittedBlob(RelOf(dir, stem, "fa-IR"));

            neutral.Length.Should().BeGreaterThan(100, "le blob doit être le fichier entier, pas un fragment");
            produced.Length.Should().BeGreaterThan(100, "le blob doit être le fichier entier, pas un fragment");

            var neutralBom = neutral.Length >= 3 && neutral[0] == 0xEF && neutral[1] == 0xBB && neutral[2] == 0xBF;
            var producedBom = produced.Length >= 3 && produced[0] == 0xEF && produced[1] == 0xBB && produced[2] == 0xBF;

            producedBom.Should().Be(neutralBom,
                $"{RelOf(dir, stem, "fa-IR")} : le BOM doit suivre celui du neutre (neutre={neutralBom})");
            produced.Should().NotContain((byte)'\r',
                $"{RelOf(dir, stem, "fa-IR")} : les fins de ligne du dépôt sont LF — un CR est du bruit de diff");
        }

        [Fact]
        public void TheVerbatimListCannotGrowSilently()
        {
            VerbatimByDesign.Should().HaveCount(10,
                "census du lot : 10 valeurs sont identiques à l'anglais par conception "
                + "(2 jetons DNN, 4 marques, 4 noms de langages) — les mêmes que le lot ar-SA ; "
                + "une dispense nouvelle est une décision à consigner, jamais un constat");
            ScriptExempt.Should().HaveCount(10,
                "les 10 valeurs verbatim sont exactement celles qui ne portent aucun caractère du bloc arabe");
        }

        /// <summary>
        /// La dispense du fichier vide est VIVANTE : son neutre doit exister et ne
        /// porter aucune clé. S'il en gagne une, la dispense devient une traduction
        /// perdue et ce fait rougit.
        /// </summary>
        [Fact]
        public void TheDeclaredEmptyNeutralIsStillEmpty()
        {
            var path = Path.Combine(
                TestRepoRoot.Find(),
                ("DNNPlatform/DesktopModules/" + DeclaredEmptyNeutral + ".resx")
                    .Replace('/', Path.DirectorySeparatorChar));
            File.Exists(path).Should().BeTrue(
                $"le neutre déclaré vide doit exister dans le checkout ({DeclaredEmptyNeutral}) — "
                + $"chemin résolu : « {path} », racine : « {TestRepoRoot.Find()} »");
            XDocument.Load(path).Root!.Elements("data").Should().BeEmpty(
                "ce fichier neutre ne porte aucune clé : un .fa-IR.resx vide n'aurait aucun sens. "
                + "S'il gagne des clés, il rejoint le lot et la dispense disparaît");
        }

        /// <summary>Lit le blob de <c>HEAD:&lt;path&gt;</c> — indépendant de core.autocrlf.</summary>
        private static byte[] CommittedBlob(string rel) => DnnResxGuardSocle.CommittedBlob(rel);
    }
}
