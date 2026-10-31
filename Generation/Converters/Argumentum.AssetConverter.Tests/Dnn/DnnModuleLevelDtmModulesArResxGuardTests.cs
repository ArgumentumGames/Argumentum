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
    /// #458 grain module-level — garde du lot <c>DesktopModules</c> « modules »
    /// ar-SA : mêmes 27 fichiers / 668 clés que le lot zh-CN (#2089), sur les
    /// HUIT répertoires <c>DesktopModules/{AuthenticationServices/DNN, CoreMessaging,
    /// DDRMenu, HTML, Journal, MemberDirectory, RazorModules/RazorHost,
    /// SocialGroups}/App_LocalResources</c> — jeux de clés identiques stem par
    /// stem au lot zh livré, mêmes 10 déclarées manquantes (repli neutre DNN),
    /// zéro verbatim.
    /// <para>
    /// ⚠️ Classe autonome miroir de la garde zh, PAS un socle factorisé : la
    /// séquence zh de ce lot n'est pas close (4ᵉ garde zh gatée sur le merge
    /// de #2098 — règle « un lot par garde »), la factorisation en socle est
    /// le grain désigné une fois la séquence close, comme <c>controls</c> et
    /// <c>admin</c> l'ont fait.
    /// </para>
    /// <para>
    /// Les stems collident massivement entre répertoires (<c>Settings.ascx</c> ×5,
    /// <c>View.ascx</c> ×4, <c>SharedResources</c> ×4, <c>Edit.ascx</c> ×2) : la
    /// couverture est identifiée par le COUPLE (répertoire, stem).
    /// </para>
    /// </summary>
    public class DnnModuleLevelDtmModulesArResxGuardTests
    {
        private const string Culture = "ar-SA";

        /// <summary>Couverture livrée : 27 fichiers, 668 clés — mêmes comptes que le lot zh (#2089).</summary>
        private static readonly (string Dir, string Stem, int DeliveredKeys)[] Covered =
        {
            ("AuthenticationServices/DNN", "Settings.ascx", 5),
            ("CoreMessaging", "SharedResources", 26),
            ("CoreMessaging", "View.ascx", 114),
            ("DDRMenu", "MenuSettings.ascx", 3),
            ("DDRMenu", "MenuView.ascx", 2),
            ("HTML", "EditHtml.ascx", 56),
            ("HTML", "HtmlModule.ascx", 21),
            ("HTML", "MyWork.ascx", 5),
            ("HTML", "Settings.ascx", 18),
            ("HTML", "SharedResources", 5),
            ("HTML", "Workflow.ascx", 31),
            ("Journal", "Edit.ascx", 1),
            ("Journal", "Settings.ascx", 16),
            ("Journal", "View.ascx", 24),
            ("Journal", "SharedResources", 90),
            ("MemberDirectory", "Settings.ascx", 70),
            ("MemberDirectory", "View.ascx", 43),
            ("RazorModules/RazorHost", "AddScript.ascx", 9),
            ("RazorModules/RazorHost", "CreateModule.ascx", 23),
            ("RazorModules/RazorHost", "EditScript.ascx", 12),
            ("RazorModules/RazorHost", "RazorHost.ascx", 4),
            ("RazorModules/RazorHost", "Settings.ascx", 3),
            ("SocialGroups", "CreateSettings.ascx", 5),
            ("SocialGroups", "Edit.ascx", 3),
            ("SocialGroups", "ListSettings.ascx", 23),
            ("SocialGroups", "SharedResources", 55),
            ("SocialGroups", "View.ascx", 1),
        };

        /// <summary>Total épinglé : la somme des <see cref="Covered"/>, jamais un littéral recopié.</summary>
        private static readonly int CoveredKeyTotal = Covered.Sum(c => c.DeliveredKeys);

        /// <summary>
        /// Clés neutres volontairement ABSENTES du pack (repli neutre DNN) —
        /// les mêmes que le lot zh : <c>CoreMessaging/SharedResources</c> —
        /// <c>ResultCount</c>/<c>SingleResultCount</c> (<c>{0}</c> pur) ;
        /// <c>Journal/SharedResources</c> — <c>Notification_Mention.Body</c>
        /// (<c>{0}</c> pur) et les quatre gabarits <c>journal_status/link/photo/file</c>
        /// (zéro prose) ; <c>RazorHost/CreateModule</c> — <c>ManifestText</c>
        /// (manifest XML) et <c>ModuleControlText</c> (directive) ;
        /// <c>SocialGroups/SharedResources</c> — <c>GroupListTemplate</c>
        /// (toute la prose vit dans des <c>{resx:…}</c> déjà livrés).
        /// </summary>
        private static readonly (string Dir, string Stem, string[] Keys)[] DeclaredMissing =
        {
            ("CoreMessaging", "SharedResources", new[] { "ResultCount.Text", "SingleResultCount.Text" }),
            ("Journal", "SharedResources", new[]
            {
                "Notification_Mention.Body", "journal_status.Text", "journal_link.Text",
                "journal_photo.Text", "journal_file.Text",
            }),
            ("RazorModules/RazorHost", "CreateModule.ascx", new[] { "ManifestText.Text", "ModuleControlText.Text" }),
            ("SocialGroups", "SharedResources", new[] { "GroupListTemplate.Text" }),
        };

        /// <summary>
        /// Valeurs légitimement identiques à l'anglais — déclarées, pas tolérées.
        /// Ce lot n'en porte AUCUNE (668 valeurs arabe) : l'épinglage à 0 interdit
        /// la croissance silencieuse (un lot non traduit passerait sinon pour un
        /// lot livré).
        /// </summary>
        private static readonly (string Dir, string Stem, string Key)[] VerbatimByDesign =
        {
        };

        private static readonly Regex Placeholder = new(@"\{\d+\}", RegexOptions.Compiled);

        /// <summary>
        /// Identité d'un jeton DNN : namespace + chemin AVANT le « | ». La partie
        /// affichable qui suit est de la prose à traduire.
        /// </summary>
        private static readonly Regex Token = new(
            @"\[(?:Portal|User|Host|Profile|Membership|Custom|Date|Module|Tab|Site|Page|App):([^\]\|]*)",
            RegexOptions.Compiled);

        /// <summary>
        /// Écriture arabe : l'alphabet arabe vit dans le bloc U+0600–U+06FF.
        /// Ce que cette règle FERME par rapport au seul « ≠ anglais » du socle :
        /// une valeur qui diffère de l'anglais SANS porter AUCUN caractère arabe
        /// (espacement modifié, ponctuation, marque latine, anglais reformulé).
        /// Ce qu'elle ne ferme PAS : une valeur à moitié traduite — le fragment
        /// traduit porte déjà un caractère arabe et la règle passe (limite
        /// constatée sur #2098, non revendiquée ici).
        /// </summary>
        private static readonly Regex ArabicScript = new("[؀-ۿ]", RegexOptions.Compiled);

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

        private static string[] MissingOf(string dir, string stem) =>
            DeclaredMissing.Where(m => m.Dir == dir && m.Stem == stem)
                .SelectMany(m => m.Keys).ToArray();

        public static IEnumerable<object[]> CoveredFiles()
        {
            foreach (var (dir, stem, delivered) in Covered)
                yield return new object[] { dir, stem, delivered };
        }

        [Fact]
        public void TheLotCoversTwentySevenFilesAndSixHundredSixtyEightKeys()
        {
            Covered.Should().HaveCount(27,
                "le lot « modules » ar-SA reprend les 27 familles CORE du lot zh (#2089) — PersonaBar et NBright exclus par #2072");
            CoveredKeyTotal.Should().Be(668,
                "678 clés neutres moins les 10 déclarées non livrées — dénominateur #2072, pas un littéral");
            Covered.Select(c => (c.Dir, c.Stem)).Should().OnlyHaveUniqueItems(
                "quatre stems collident entre répertoires (Settings ×5, View ×4, SharedResources ×4, Edit ×2) : le couple identifie");
        }

        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void PackCoversTheDeclaredKeyCount(string dir, string stem, int deliveredKeys)
        {
            Values(dir, stem, Culture).Should().HaveCount(deliveredKeys,
                $"{RelOf(dir, stem, Culture)} : le nombre de clés livrées a changé — mettre à jour la garde");
        }

        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void TheDeliveredKeySetIsTheNeutralOnesMinusTheDeclaredMissing(string dir, string stem, int _)
        {
            var neutral = Values(dir, stem, string.Empty);
            var localized = Values(dir, stem, Culture);
            var declared = MissingOf(dir, stem);

            localized.Keys.Should().BeEquivalentTo(neutral.Keys.Except(declared),
                $"{RelOf(dir, stem, Culture)} : livré = neutre − déclarées manquantes ({declared.Length} ici) — "
                + "toute clé non livrée hors déclaration est une traduction perdue, toute clé en trop n'existe pas");
        }

        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void EveryDeliveredKeyIsSaneAgainstTheNeutral(string dir, string stem, int _)
        {
            var neutral = Values(dir, stem, string.Empty);
            var localized = Values(dir, stem, Culture);

            foreach (var (key, value) in localized)
            {
                value.Should().NotBeNullOrWhiteSpace($"{RelOf(dir, stem, Culture)}:{key} — valeur vide");

                Placeholder.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
                    .Should().Equal(
                        Placeholder.Matches(neutral[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
                        $"{RelOf(dir, stem, Culture)}:{key} — multiset des placeholders modifié");

                Token.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
                    .Should().Equal(
                        Token.Matches(neutral[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
                        $"{RelOf(dir, stem, Culture)}:{key} — identité des jetons DNN modifiée");

                if (VerbatimByDesign.Contains((dir, stem, key)))
                    continue;

                value.Should().NotBe(neutral[key],
                    $"{RelOf(dir, stem, Culture)}:{key} — copie de l'anglais par accident (une copie VOLONTAIRE "
                    + "se déclare dans VerbatimByDesign)");
            }
        }

        /// <summary>
        /// Toute valeur livrée porte l'écriture arabe — voir <see cref="ArabicScript"/>
        /// pour ce que la règle ferme et ne ferme pas.
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void EveryValueCarriesArabicScript(string dir, string stem, int _)
        {
            foreach (var (key, value) in Values(dir, stem, Culture))
                ArabicScript.IsMatch(value).Should().BeTrue(
                    $"{RelOf(dir, stem, Culture)}:{key} — la valeur ne porte aucun caractère arabe ; "
                    + $"valeur = « {value} »");
        }

        /// <summary>
        /// Le BOM du fichier produit suit celui de SON neutre — le module-level est
        /// hétérogène : dans ce lot, <c>AuthenticationServices/DNN/Settings</c>,
        /// <c>HTML/Settings</c> et <c>HTML/SharedResources</c> n'ont PAS de BOM
        /// quand les 24 autres en ont un. Attente DÉRIVÉE du neutre.
        /// <para>
        /// ⚠️ Transport mesuré sur le <b>blob committé</b> (<c>git cat-file</c>), jamais
        /// sur l'arbre de travail : le runner CI a <c>core.autocrlf=true</c>, qui
        /// convertit LF → CRLF au checkout (règle payée 2× : #2078, #2087).
        /// </para>
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void BomMirrorsTheNeutralAndLineEndingsAreLf(string dir, string stem, int _)
        {
            var neutral = CommittedBlob(RelOf(dir, stem, string.Empty));
            var produced = CommittedBlob(RelOf(dir, stem, Culture));

            neutral.Length.Should().BeGreaterThan(100, "le blob doit être le fichier entier, pas un fragment");
            produced.Length.Should().BeGreaterThan(100, "le blob doit être le fichier entier, pas un fragment");

            var neutralBom = neutral.Length >= 3 && neutral[0] == 0xEF && neutral[1] == 0xBB && neutral[2] == 0xBF;
            var producedBom = produced.Length >= 3 && produced[0] == 0xEF && produced[1] == 0xBB && produced[2] == 0xBF;

            producedBom.Should().Be(neutralBom,
                $"{RelOf(dir, stem, Culture)} : le BOM doit suivre celui du neutre (neutre={neutralBom})");
            produced.Should().NotContain((byte)'\r',
                $"{RelOf(dir, stem, Culture)} : les fins de ligne du dépôt sont LF — un CR est du bruit de diff");
        }

        [Fact]
        public void TheVerbatimListCannotGrowSilently()
        {
            VerbatimByDesign.Should().BeEmpty(
                "ce lot ne porte aucune copie de l'anglais par conception (668 valeurs arabe) : chaque ajout doit être "
                + "décidé, pas constaté — et une traduction recopiée est un lot non traduit");
        }

        /// <summary>
        /// La déclaration des manquantes est VIVANTE : chaque entrée vise un fichier
        /// du lot, porte des clés qui existent au neutre, et le total épinglé (10)
        /// interdit la croissance silencieuse — une clé « oubliée » deviendrait
        /// indistinguable d'un repli déclaré.
        /// </summary>
        [Fact]
        public void TheDeclaredMissingListIsLiveAndPinned()
        {
            DeclaredMissing.Length.Should().Be(4,
                "quatre fichiers portent des manquantes déclarées (CoreMessaging/SR, Journal/SR, RazorHost/CreateModule, SocialGroups/SR)");
            DeclaredMissing.Sum(m => m.Keys.Length).Should().Be(10,
                "10 clés déclarées non livrées : 2 ({0} pur CoreMessaging) + 5 (Journal : {0} pur + 4 gabarits sans prose) "
                + "+ 2 (RazorHost : manifest XML + directive) + 1 (gabarit {resx:} SocialGroups)");
            foreach (var (dir, stem, keys) in DeclaredMissing)
            {
                Covered.Should().Contain(c => c.Dir == dir && c.Stem == stem,
                    $"la déclaration manquante ({dir}/{stem}) porte sur un fichier du lot");
                var neutral = Values(dir, stem, string.Empty);
                keys.Should().OnlyContain(k => neutral.ContainsKey(k),
                    $"{dir}/{stem} : une clé déclarée manquante doit exister au neutre — sinon la déclaration est morte");
            }
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
