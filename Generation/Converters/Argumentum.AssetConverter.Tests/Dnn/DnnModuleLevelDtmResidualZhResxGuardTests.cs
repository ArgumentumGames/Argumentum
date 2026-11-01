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
    /// #458 grain module-level — garde du lot <c>DesktopModules</c> « résidu » zh-CN,
    /// quatrième et dernier lot du périmètre mesuré par #2072 : ce sont les fichiers
    /// neutres qui n'avaient AUCUN jumeau <c>zh-CN</c> après les lots <c>Admin</c>
    /// (20 fichiers / 640 clés) et « modules » (#2089, 27 fichiers / 668 clés).
    /// <para>
    /// Périmètre MESURÉ au 01/11 : 37 fichiers / 689 clés, répartis sur
    /// <c>Admin/</c> (19), <c>AuthenticationServices/</c> (8), <c>Connectors/</c> (3),
    /// <c>ResourceManager/</c> (4), <c>SiteExportImport/</c> (1),
    /// <c>TelerikRemoval/</c> (1) et <c>ToSic.Sxc/</c> (1). Sept de ces fichiers
    /// n'avaient aucune sœur de culture (ni fr-FR, ni ru-RU, ni pt-BR, ni es-ES) :
    /// c'est du jamais-localisé, pas un trou zh.
    /// </para>
    /// <para>
    /// ⚠️ L'étiquette du pool annonçait « 9 fichiers / 381 clés ». Re-mesuré sur
    /// l'arbre au moment de la livraison, le résidu fait 37 fichiers / 689 clés :
    /// l'écart est consigné dans la note de lot, il n'est pas exécuté à l'aveugle.
    /// </para>
    /// <para>
    /// La référence reste le NEUTRE (jamais une autre culture), et la garde
    /// n'épingle que le lot livré : un lot suivant ajoute ses entrées, pas ses règles.
    /// </para>
    /// </summary>
    public class DnnModuleLevelDtmResidualZhResxGuardTests
    {
        /// <summary>Couverture livrée : 37 fichiers, 689 clés, culture zh-CN.</summary>
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
        /// Onze entrées, de trois natures : jetons de remplacement DNN
        /// (<c>[PortalRoot]</c>, <c>[UserFolder]</c> — traduits, ils cesseraient
        /// d'être des jetons), marqueur de tri alphabétique (<c>A-Z</c>), noms de
        /// marques (Facebook, Google, Live, Twitter) et noms de langages
        /// (<c>C#</c>, <c>Visual Basic</c>). Liste ÉPINGLÉE : un ajout pour faire
        /// passer un rouge rougit le fait de comptage.
        /// </summary>
        private static readonly (string Dir, string Stem, string Key)[] VerbatimByDesign =
        {
            ("Admin/HtmlEditorManager", "ProviderConfiguration.ascx", "PortalRoot.Text"),
            ("Admin/HtmlEditorManager", "ProviderConfiguration.ascx", "UserFolder.Text"),
            ("Admin/Dnn.EditBar/Resources/ContentEditorManager", "SharedResources",
                "pagePicker_sortAscendingButtonTitle.Text"),
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
        /// rien à livrer, et un <c>.zh-CN.resx</c> vide n'aurait aucun sens. Déclaré,
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
        /// Bloc CJK : Ext-A (U+3400–U+4DBF), unifié (U+4E00–U+9FFF) et formes de
        /// compatibilité (U+F900–U+FAFF). Les deux plages hors bloc unifié sont une
        /// marge défensive, pas une nécessité (mesure #2098 : 2 997/2 997 valeurs
        /// zh du dépôt relèvent du bloc unifié).
        /// <para>
        /// Ce que la règle ferme : une valeur qui diffère de l'anglais SANS porter
        /// aucun caractère chinois (espacement, ponctuation, marque latine, anglais
        /// reformulé). Ce qu'elle ne ferme PAS : une valeur à moitié traduite, qui
        /// porte déjà un caractère chinois.
        /// </para>
        /// </summary>
        private static readonly Regex ChineseScript =
            new(@"[㐀-䶿一-鿿豈-﫿]", RegexOptions.Compiled);

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
                "le résidu zh mesure 37 fichiers neutres sans jumeau zh-CN après les lots Admin (20) "
                + "et modules (27) — périmètre mesuré au 01/11, pas l'étiquette « 9 fichiers » du pool");
            CoveredKeyTotal.Should().Be(689,
                "somme des clés neutres des 37 fichiers : « 381 » était l'étiquette du pool, re-mesurée ici");
            Covered.Select(c => (c.Dir, c.Stem)).Should().OnlyHaveUniqueItems(
                "quatre stems portent le même nom dans des répertoires différents "
                + "(Login.ascx ×4 en AuthenticationServices, Settings.ascx ×3, View.ascx ×3) : le couple identifie");
        }

        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void PackCoversTheDeclaredKeyCount(string dir, string stem, int deliveredKeys)
        {
            Values(dir, stem, "zh-CN").Should().HaveCount(deliveredKeys,
                $"{RelOf(dir, stem, "zh-CN")} : le nombre de clés livrées a changé — mettre à jour la garde");
        }

        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void TheDeliveredKeySetIsExactlyTheNeutralOne(string dir, string stem, int _)
        {
            Values(dir, stem, "zh-CN").Keys.Should().BeEquivalentTo(Values(dir, stem, string.Empty).Keys,
                $"{RelOf(dir, stem, "zh-CN")} : ce lot ne déclare AUCUNE clé manquante — le pack couvre "
                + "tout le neutre, sinon une traduction est perdue");
        }

        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void EveryDeliveredValueIsSaneAgainstTheNeutral(string dir, string stem, int _)
        {
            var neutral = Values(dir, stem, string.Empty);
            var localized = Values(dir, stem, "zh-CN");

            foreach (var (key, value) in localized)
            {
                value.Should().NotBeNullOrWhiteSpace($"{RelOf(dir, stem, "zh-CN")}:{key} — valeur vide");

                Placeholder.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
                    .Should().Equal(
                        Placeholder.Matches(neutral[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
                        $"{RelOf(dir, stem, "zh-CN")}:{key} — multiset des placeholders modifié");

                Token.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
                    .Should().Equal(
                        Token.Matches(neutral[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
                        $"{RelOf(dir, stem, "zh-CN")}:{key} — identité des jetons DNN modifiée");

                if (VerbatimByDesign.Contains((dir, stem, key)))
                    continue;

                value.Should().NotBe(neutral[key],
                    $"{RelOf(dir, stem, "zh-CN")}:{key} — copie de l'anglais par accident (une copie VOLONTAIRE "
                    + "se déclare dans VerbatimByDesign)");
            }
        }

        /// <summary>
        /// Contrôle d'écriture chinoise (#2098), appliqué au lot dès sa première
        /// livraison : chaque valeur livrée porte au moins un caractère CJK.
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void EveryNonExemptValueCarriesChineseScript(string dir, string stem, int _)
        {
            foreach (var (key, value) in Values(dir, stem, "zh-CN"))
            {
                if (ScriptExempt.Contains((dir, stem, key)))
                    continue;
                ChineseScript.IsMatch(value).Should().BeTrue(
                    $"{RelOf(dir, stem, "zh-CN")}:{key} — la valeur ne porte aucun caractère chinois ; "
                    + $"valeur = « {value} »");
            }
        }

        /// <summary>
        /// Le BOM du fichier produit suit celui de SON neutre — le module-level est
        /// hétérogène : dans ce lot, <c>ResourceManager/ResourceManager</c> n'a PAS
        /// de BOM quand les 36 autres en ont un. Attente DÉRIVÉE du neutre.
        /// <para>
        /// ⚠️ Transport mesuré sur le <b>blob committé</b> (<c>git cat-file</c>), jamais
        /// sur l'arbre de travail : le runner CI a <c>core.autocrlf=true</c>, qui
        /// convertit LF → CRLF au checkout. Corollaire propre à ce lot :
        /// <c>ToSic.Sxc/View.ascx.resx</c> est commité en CRLF ; le fichier produit
        /// est en LF quand même — les fins de ligne du dépôt sont LF, un CR est du
        /// bruit de diff (le constructeur normalise).
        /// </para>
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void BomMirrorsTheNeutralAndLineEndingsAreLf(string dir, string stem, int _)
        {
            var neutral = CommittedBlob(RelOf(dir, stem, string.Empty));
            var produced = CommittedBlob(RelOf(dir, stem, "zh-CN"));

            neutral.Length.Should().BeGreaterThan(100, "le blob doit être le fichier entier, pas un fragment");
            produced.Length.Should().BeGreaterThan(100, "le blob doit être le fichier entier, pas un fragment");

            var neutralBom = neutral.Length >= 3 && neutral[0] == 0xEF && neutral[1] == 0xBB && neutral[2] == 0xBF;
            var producedBom = produced.Length >= 3 && produced[0] == 0xEF && produced[1] == 0xBB && produced[2] == 0xBF;

            producedBom.Should().Be(neutralBom,
                $"{RelOf(dir, stem, "zh-CN")} : le BOM doit suivre celui du neutre (neutre={neutralBom})");
            produced.Should().NotContain((byte)'\r',
                $"{RelOf(dir, stem, "zh-CN")} : les fins de ligne du dépôt sont LF — un CR est du bruit de diff");
        }

        [Fact]
        public void TheVerbatimListCannotGrowSilently()
        {
            VerbatimByDesign.Should().HaveCount(11,
                "census du lot : 11 valeurs sont identiques à l'anglais par conception "
                + "(2 jetons DNN, 1 marqueur de tri, 4 marques, 4 noms de langages) — "
                + "une dispense nouvelle est une décision à consigner, jamais un constat");
            ScriptExempt.Should().HaveCount(11,
                "les 11 valeurs verbatim sont exactement celles qui ne portent aucun caractère chinois");
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
                "ce fichier neutre ne porte aucune clé : un .zh-CN.resx vide n'aurait aucun sens. "
                + "S'il gagne des clés, il rejoint le lot et la dispense disparaît");
        }

        /// <summary>Lit le blob de <c>HEAD:&lt;path&gt;</c> — indépendant de core.autocrlf.</summary>
        private static byte[] CommittedBlob(string rel)
        {
            var psi = new ProcessStartInfo("git", $"cat-file -p \"HEAD:{rel}\"")
            {
                WorkingDirectory = TestRepoRoot.Find(),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            using var process = Process.Start(psi)
                ?? throw new InvalidOperationException("git n'a pas démarré");
            var stderr = process.StandardError.ReadToEndAsync();
            using var buffer = new MemoryStream();
            process.StandardOutput.BaseStream.CopyTo(buffer);
            process.WaitForExit(30_000);
            if (process.ExitCode != 0)
                throw new InvalidOperationException(
                    $"git cat-file sur '{rel}' a échoué ({process.ExitCode}) : {stderr.Result}");
            return buffer.ToArray();
        }
    }
}
