using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// #458 grain module-level — garde du lot <c>DesktopModules</c> « résidu » ar-SA,
    /// jumeau arabe du lot zh-CN livré par <c>DnnModuleLevelDtmResidualZhResxGuardTests</c> :
    /// les mêmes 37 fichiers neutres, les mêmes 689 clés, la même référence (le NEUTRE).
    /// <para>
    /// Périmètre MESURÉ au 01/11 : 37 fichiers / 689 clés. Sept de ces fichiers
    /// n'avaient aucune sœur de culture (ni fr-FR, ni ru-RU, ni pt-BR, ni es-ES) :
    /// c'est du jamais-localisé, pas un trou ar.
    /// </para>
    /// <para>
    /// 🔀 <b>Le sens du contrôle d'écriture est INVERSÉ par rapport au lot zh</b>
    /// (décision 33). Pour zh la règle est « chaque valeur porte au moins un
    /// caractère chinois ». Pour ar-SA c'est l'inverse qui est vrai : les lettres
    /// <b>exclusives de l'arabe</b> (ي U+064A, ك U+0643, ى U+0649) sont ATTENDUES —
    /// mesuré 418/679 valeurs non verbatim, 61,6 % — tandis que les lettres
    /// <b>exclusives du persan</b> (پ چ ژ گ ک ی) doivent être ABSENTES. Écrire ici
    /// un contrôle de « part persane » à 80 % rougirait sur du contenu correct ; la
    /// seule règle transposable telle quelle est l'interdiction, pas le plancher.
    /// </para>
    /// </summary>
    public class DnnModuleLevelDtmResidualArResxGuardTests
    {
        /// <summary>Couverture livrée : 37 fichiers, 689 clés, culture ar-SA.</summary>
        private static readonly (string Dir, string Stem, int DeliveredKeys)[] Covered =
        {
            ("Admin/Authentication", "Authentication.ascx", 3),
            ("Admin/Authentication", "Logoff.ascx", 1),
            ("Admin/Console", "Settings.ascx", 38),
            ("Admin/Console", "ViewConsole.ascx", 7),
            ("Admin/Dnn.EditBar", "AddExistingModule", 1),
            ("Admin/Dnn.EditBar", "AddModule", 3),
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
            ("ResourceManager", "ResourceManager", 106),
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
        /// Dix entrées, de deux natures : jetons de remplacement DNN
        /// (<c>[PortalRoot]</c>, <c>[UserFolder]</c> — traduits, ils cesseraient
        /// d'être des jetons), noms de marques (Facebook, Google, Live, Twitter) et
        /// noms de langages (<c>C#</c>, <c>Visual Basic</c>). Liste ÉPINGLÉE : un
        /// ajout pour faire passer un rouge rougit le fait de comptage.
        /// <para>
        /// ⚠️ Le lot zh portait ONZE entrées : il comptait en plus le marqueur de tri
        /// <c>A-Z</c> (<c>pagePicker_sortAscendingButtonTitle.Text</c>). En arabe ce
        /// marqueur est TRADUIT en « أ-ي » — l'alphabet arabe n'a pas le même ordre,
        /// et <c>A-Z</c> n'y désignerait rien. Dix est donc le compte correct ici, et
        /// recopier onze ferait échouer le fait de comptage.
        /// </para>
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
        /// rien à livrer, et un <c>.ar-SA.resx</c> vide n'aurait aucun sens. Déclaré,
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
        /// Bloc arabe (U+0600–U+06FF). Volontairement PAS restreint aux lettres : les
        /// chiffres arabo-indiens et la ponctuation du bloc comptent aussi comme
        /// écriture localisée.
        /// <para>
        /// Ce que la règle ferme : une valeur qui diffère de l'anglais SANS porter
        /// aucun caractère arabe (espacement, ponctuation latine, anglais reformulé).
        /// Ce qu'elle ne ferme PAS : une valeur à moitié traduite, qui porte déjà un
        /// caractère arabe.
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
        public void TheLotCoversThirtySevenFilesAndSixHundredEightyNineKeys()
        {
            Covered.Should().HaveCount(37,
                "le résidu ar-SA mesure 37 fichiers, les mêmes que le lot zh livré avant lui — "
                + "périmètre mesuré au 01/11 sur l'arbre, pas l'étiquette du pool");
            CoveredKeyTotal.Should().Be(689,
                "somme des clés neutres des 37 fichiers — identique au lot zh, qui porte sur les mêmes fichiers");
            Covered.Select(c => (c.Dir, c.Stem)).Should().OnlyHaveUniqueItems(
                "quatre stems portent le même nom dans des répertoires différents "
                + "(Login.ascx ×4 en AuthenticationServices, Settings.ascx ×3, View.ascx ×3) : le couple identifie");
        }

        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void PackCoversTheDeclaredKeyCount(string dir, string stem, int deliveredKeys)
        {
            Values(dir, stem, "ar-SA").Should().HaveCount(deliveredKeys,
                $"{RelOf(dir, stem, "ar-SA")} : le nombre de clés livrées a changé — mettre à jour la garde");
        }

        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void TheDeliveredKeySetIsExactlyTheNeutralOne(string dir, string stem, int _)
        {
            Values(dir, stem, "ar-SA").Keys.Should().BeEquivalentTo(Values(dir, stem, string.Empty).Keys,
                $"{RelOf(dir, stem, "ar-SA")} : ce lot ne déclare AUCUNE clé manquante — le pack couvre "
                + "tout le neutre, sinon une traduction est perdue");
        }

        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void EveryDeliveredValueIsSaneAgainstTheNeutral(string dir, string stem, int _)
        {
            var neutral = Values(dir, stem, string.Empty);
            var localized = Values(dir, stem, "ar-SA");

            foreach (var (key, value) in localized)
            {
                value.Should().NotBeNullOrWhiteSpace($"{RelOf(dir, stem, "ar-SA")}:{key} — valeur vide");

                Placeholder.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
                    .Should().Equal(
                        Placeholder.Matches(neutral[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
                        $"{RelOf(dir, stem, "ar-SA")}:{key} — multiset des placeholders modifié");

                Token.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
                    .Should().Equal(
                        Token.Matches(neutral[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
                        $"{RelOf(dir, stem, "ar-SA")}:{key} — identité des jetons DNN modifiée");

                if (VerbatimByDesign.Contains((dir, stem, key)))
                    continue;

                value.Should().NotBe(neutral[key],
                    $"{RelOf(dir, stem, "ar-SA")}:{key} — copie de l'anglais par accident (une copie VOLONTAIRE "
                    + "se déclare dans VerbatimByDesign)");
            }
        }

        /// <summary>
        /// Contrôle d'écriture arabe, sens INVERSÉ du lot zh (décision 33) : chaque
        /// valeur livrée non exemptée porte au moins un caractère du bloc arabe.
        /// Mesuré à la livraison : 679/679 valeurs non verbatim.
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void EveryNonExemptValueCarriesArabicScript(string dir, string stem, int _)
        {
            foreach (var (key, value) in Values(dir, stem, "ar-SA"))
            {
                if (ScriptExempt.Contains((dir, stem, key)))
                    continue;
                ArabicScript.IsMatch(value).Should().BeTrue(
                    $"{RelOf(dir, stem, "ar-SA")}:{key} — la valeur ne porte aucun caractère arabe ; "
                    + $"valeur = « {value} »");
            }
        }

        /// <summary>
        /// ⛔ Aucune lettre propre au persan (پ چ ژ گ ک ی) dans un pack arabe.
        /// <c>ک</c> (U+06A9) et <c>ك</c> (U+0643) sont presque indiscernables à l'œil :
        /// une traduction produite depuis un clavier persan les confond, et le défaut
        /// est invisible à la relecture. C'est la raison d'être de ce fait.
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void NoDeliveredValueUsesPersianOnlyLetters(string dir, string stem, int _)
        {
            foreach (var (key, value) in Values(dir, stem, "ar-SA"))
            {
                PersianScriptMarkers.PersianOnly.IsMatch(value).Should().BeFalse(
                    $"{RelOf(dir, stem, "ar-SA")}:{key} — lettre propre au persan dans un pack ar-SA ; "
                    + $"valeur = « {value} »");
            }
        }

        /// <summary>
        /// Contrôle INVERSE, rejoué à chaque exécution : le même instrument
        /// (<see cref="PersianScriptMarkers"/>), appliqué à CE lot, doit conclure
        /// « ce n'est pas du persan » sur les DEUX critères. Sans lui, un instrument
        /// devenu aveugle resterait vert et personne ne le saurait.
        /// </summary>
        [Fact]
        public void ThePersianInstrumentRejectsThisArabicLot()
        {
            var values = Covered
                .SelectMany(c => Values(c.Dir, c.Stem, "ar-SA").Values)
                .ToList();

            var (total, persian, arabicOnly) = PersianScriptMarkers.Profile(values);

            total.Should().Be(689, "le lot entier doit être lu, pas un échantillon");
            ((double)persian / total).Should().BeLessThan(
                PersianScriptMarkers.MinimumPersianShare,
                $"{persian}/{total} valeurs portent une lettre persane — si le lot arabe passait le "
                + "plancher de 80 %, l'instrument ne distinguerait plus les deux écritures");
            arabicOnly.Should().BeGreaterThan(0,
                "aucune lettre propre à l'arabe (ي / ك / ى) : le second critère ne serait pas exercé, "
                + "et le contrôle inverse ne prouverait rien");
        }

        /// <summary>
        /// Le BOM du fichier produit suit celui de SON neutre — le module-level est
        /// hétérogène : dans ce lot, <b>30 fichiers portent le BOM et 7 n'en portent
        /// pas</b>. ⚠️ Cette énumération est un CONSTAT daté, pas la règle : l'attente
        /// se DÉRIVE du neutre fichier par fichier (37/37 sans écart).
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
            var produced = CommittedBlob(RelOf(dir, stem, "ar-SA"));

            neutral.Length.Should().BeGreaterThan(100, "le blob doit être le fichier entier, pas un fragment");
            produced.Length.Should().BeGreaterThan(100, "le blob doit être le fichier entier, pas un fragment");

            var neutralBom = neutral.Length >= 3 && neutral[0] == 0xEF && neutral[1] == 0xBB && neutral[2] == 0xBF;
            var producedBom = produced.Length >= 3 && produced[0] == 0xEF && produced[1] == 0xBB && produced[2] == 0xBF;

            producedBom.Should().Be(neutralBom,
                $"{RelOf(dir, stem, "ar-SA")} : le BOM doit suivre celui du neutre (neutre={neutralBom})");
            produced.Should().NotContain((byte)'\r',
                $"{RelOf(dir, stem, "ar-SA")} : les fins de ligne du dépôt sont LF — un CR est du bruit de diff");
        }

        [Fact]
        public void TheVerbatimListCannotGrowSilently()
        {
            VerbatimByDesign.Should().HaveCount(10,
                "census du lot : 10 valeurs sont identiques à l'anglais par conception "
                + "(2 jetons DNN, 4 marques, 4 noms de langages) — une dispense nouvelle est une "
                + "décision à consigner, jamais un constat. Le lot zh en compte 11 : le marqueur "
                + "de tri A-Z y est verbatim, alors qu'en arabe il est traduit en « أ-ي »");
            ScriptExempt.Should().HaveCount(10,
                "les 10 valeurs verbatim sont exactement celles qui ne portent aucun caractère arabe");
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
                "ce fichier neutre ne porte aucune clé : un .ar-SA.resx vide n'aurait aucun sens. "
                + "S'il gagne des clés, il rejoint le lot et la dispense disparaît");
        }

        /// <summary>Lit le blob de <c>HEAD:&lt;path&gt;</c> — indépendant de core.autocrlf.</summary>
        private static byte[] CommittedBlob(string rel) => DnnResxGuardSocle.CommittedBlob(rel);
    }
}
