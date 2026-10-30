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
    /// #458 grain module-level — garde du pack <c>admin</c> zh-CN, second lot
    /// du périmètre mesuré par #2072 (CORE sans <c>Install/</c> : 94 fichiers,
    /// 2 168 clés par langue ; ce lot : 27 fichiers répartis sur SIX répertoires
    /// <c>admin/{Modules,Portal,Security,Skins,Tabs,Users}/App_LocalResources</c>,
    /// 328 clés livrées sur 338 neutres).
    /// <para>
    /// ⚠️ Quatre stems collident entre répertoires (<c>Export</c>, <c>Import</c>,
    /// <c>Privacy</c>, <c>Terms</c>) : la couverture est identifiée par le COUPLE
    /// (répertoire, stem), jamais par le stem seul.
    /// </para>
    /// <para>
    /// Comme le lot <c>controls</c>, la référence est le NEUTRE, jamais ru-RU
    /// (15/96 fichiers CORE divergent de ru, #2072), et la garde n'épingle que le
    /// lot livré : un lot suivant ajoute ses entrées, pas ses règles.
    /// </para>
    /// <para>
    /// Dix clés neutres ne sont PAS livrées (<see cref="DeclaredMissing"/>) :
    /// repli neutre DNN volontaire, même philosophie que les packs AGR — une clé
    /// sans contenu localisable (vide, markup pur, format pur, URL) n'est pas
    /// « traduite » en recopiant l'anglais.
    /// </para>
    /// </summary>
    public class DnnModuleLevelAdminZhResxGuardTests
    {
        /// <summary>Couverture livrée : 27 fichiers, 328 clés, culture zh-CN.</summary>
        private static readonly (string Dir, string Stem, int DeliveredKeys)[] Covered =
        {
            ("Modules", "Export.ascx", 12),
            ("Modules", "Import.ascx", 14),
            ("Modules", "ModuleLocalization.ascx", 12),
            ("Modules", "ModulePermissions.ascx", 5),
            ("Modules", "ModuleSettings.ascx", 114),
            ("Modules", "ViewSource.ascx", 8),
            ("Portal", "Privacy.ascx", 1),
            ("Portal", "Terms.ascx", 1),
            ("Security", "AccessDenied.ascx", 2),
            ("Security", "PasswordReset.ascx", 17),
            ("Security", "SendPassword.ascx", 35),
            ("Skins", "Copyright.ascx", 1),
            ("Skins", "Language.ascx", 1),
            ("Skins", "LinkToFullSite.ascx", 1),
            ("Skins", "LinkToMobileSite.ascx", 1),
            ("Skins", "LinkToTabletSite.ascx", 1),
            ("Skins", "Privacy.ascx", 1),
            ("Skins", "Search.ascx", 10),
            ("Skins", "Tags.ascx", 8),
            ("Skins", "Terms.ascx", 1),
            ("Skins", "Toast.ascx", 2),
            ("Skins", "TreeViewMenu.ascx", 3),
            ("Skins", "User.ascx", 11),
            ("Skins", "UserAndLogin.ascx", 11),
            ("Tabs", "Export.ascx", 17),
            ("Tabs", "Import.ascx", 36),
            ("Users", "ViewProfile.ascx", 2),
        };

        /// <summary>Total épinglé : la somme des <see cref="Covered"/>, jamais un littéral recopié.</summary>
        private static readonly int CoveredKeyTotal = Covered.Sum(c => c.DeliveredKeys);

        /// <summary>
        /// Clés neutres volontairement ABSENTES du pack (repli neutre DNN) :
        /// <c>Language.ascx</c> — 3 neutres vides + 5 templates HTML purs sans
        /// prose affichable ; <c>Search.ascx:URL.Text</c> — modèle d'URL Google ;
        /// <c>User.ascx:NewMessages.Text</c> — « ({0}) », format pur. Une clé
        /// sans contenu localisable n'est pas traduite en recopiant l'anglais.
        /// </summary>
        private static readonly (string Dir, string Stem, string[] Keys)[] DeclaredMissing =
        {
            ("Skins", "Language.ascx", new[]
            {
                "AlternateTemplate.Default", "CommonFooterTemplate.Default", "CommonHeaderTemplate.Default",
                "FooterTemplate.Default", "HeaderTemplate.Default", "ItemTemplate.Default",
                "SelectedItemTemplate.Default", "SeparatorTemplate.Default",
            }),
            ("Skins", "Search.ascx", new[] { "URL.Text" }),
            ("Skins", "User.ascx", new[] { "NewMessages.Text" }),
        };

        /// <summary>
        /// Valeurs légitimement identiques à l'anglais — déclarées, pas tolérées.
        /// Ce lot n'en porte AUCUNE : l'épinglage à 0 interdit la croissance
        /// silencieuse (un lot non traduit passerait sinon pour un lot livré).
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

        private static string RelOf(string dir, string stem, string culture) =>
            $"DNNPlatform/admin/{dir}/App_LocalResources/{stem}{(culture.Length > 0 ? "." + culture : "")}.resx";

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
        public void TheLotCoversTwentySevenFilesAndThreeHundredTwentyEightKeys()
        {
            Covered.Should().HaveCount(27, "le lot `admin` compte 27 familles CORE (Login.ascx de Skins exclu : ar/fa/zh déjà livrés)");
            CoveredKeyTotal.Should().Be(328,
                "338 clés neutres moins les 10 déclarées non livrées — dénominateur #2072, pas un littéral");
            Covered.Select(c => (c.Dir, c.Stem)).Should().OnlyHaveUniqueItems(
                "quatre stems collident entre répertoires (Export, Import, Privacy, Terms) : le couple identifie");
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
        public void TheDeliveredKeySetIsTheNeutralOnesMinusTheDeclaredMissing(string dir, string stem, int _)
        {
            var neutral = Values(dir, stem, string.Empty);
            var localized = Values(dir, stem, "zh-CN");
            var declared = MissingOf(dir, stem);

            localized.Keys.Should().BeEquivalentTo(neutral.Keys.Except(declared),
                $"{RelOf(dir, stem, "zh-CN")} : livré = neutre − déclarées manquantes ({declared.Length} ici) — "
                + "toute clé non livrée hors déclaration est une traduction perdue, toute clé en trop n'existe pas");
        }

        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void EveryDeliveredKeyIsSaneAgainstTheNeutral(string dir, string stem, int _)
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
        /// Le BOM du fichier produit suit celui de SON neutre — le module-level est
        /// hétérogène : en admin, <c>PasswordReset</c>/<c>SendPassword</c> (Security),
        /// <c>LinkToFull/Mobile/TabletSite</c> (Skins) et <c>ViewProfile</c> (Users)
        /// n'ont PAS de BOM quand les 22 autres en ont un. Attente DÉRIVÉE du neutre.
        /// <para>
        /// ⚠️ Transport mesuré sur le <b>blob committé</b> (<c>git cat-file</c>), jamais
        /// sur l'arbre de travail : le runner CI a <c>core.autocrlf=true</c>, qui
        /// convertit LF → CRLF au checkout (règle payée 2× : #2078, puis #2087 dont
        /// la première version lisait <c>File.ReadAllBytes</c> et rougissait 7× en CI).
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
            VerbatimByDesign.Should().BeEmpty(
                "ce lot ne porte aucune copie de l'anglais par conception : chaque ajout doit être "
                + "décidé, pas constaté — et une traduction recopiée est un lot non traduit");
        }

        /// <summary>
        /// La déclaration des manquantes est VIVANTE : chaque entrée vise un fichier
        /// du lot, porte des clés qui existent au neutre, et le total épinglé (10)
        /// interdit la croissance silencieuse — une clé « oubliée » deviendait
        /// indistinguable d'un repli déclaré.
        /// </summary>
        [Fact]
        public void TheDeclaredMissingListIsLiveAndPinned()
        {
            DeclaredMissing.Length.Should().Be(3,
                "trois fichiers portent des manquantes déclarées (Language, Search, User — tous dans Skins)");
            DeclaredMissing.Sum(m => m.Keys.Length).Should().Be(10,
                "10 clés déclarées non livrées : 8 (Language markup/vide) + URL.Text + NewMessages.Text");
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
