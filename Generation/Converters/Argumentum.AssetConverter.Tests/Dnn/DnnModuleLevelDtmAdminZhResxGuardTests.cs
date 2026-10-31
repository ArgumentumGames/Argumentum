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
    /// #458 grain module-level — garde du lot <c>DesktopModules</c> « Admin »
    /// zh-CN, lot 3b du périmètre mesuré par #2072 (CORE sans <c>Install/</c> :
    /// 94 fichiers, 2 168 clés par langue). Ce lot : <c>Admin/Security</c>
    /// (17 fichiers) + <c>Admin/ViewProfile</c> (3 fichiers), 640 clés livrées
    /// sur 645 neutres.
    /// <para>
    /// ⚠️ <c>SharedResources</c> existe dans les DEUX répertoires du lot : la
    /// couverture est identifiée par le COUPLE (répertoire, stem), jamais par
    /// le stem seul. <c>ViewProfile/SharedResources</c> est livré VIDE : sa
    /// clé unique est déclarée manquante (jetons purs), le fichier-coquille
    /// reste dans le pack — repli neutre DNN clé par clé.
    /// </para>
    /// <para>
    /// Comme les lots <c>controls</c>, <c>admin</c> et 3a, la référence est le
    /// NEUTRE, jamais ru-RU, et la garde n'épingle que le lot livré : le lot
    /// 3c (reste Admin) ajoutera ses entrées, pas ses règles.
    /// </para>
    /// <para>
    /// Cinq clés neutres ne sont PAS livrées (<see cref="DeclaredMissing"/>) :
    /// quatre en-têtes de colonne à neutre VIDE (<c>Edit/Del/Dn/Up.Header</c>
    /// de <c>ProfileDefinitions</c> — rien à localiser, le repli neutre rend
    /// la même chose) et le gabarit de <c>ViewProfile/SharedResources</c>
    /// dont TOUTE la valeur vit dans des jetons <c>[PROFILE:…]</c>/<c>[USER:…]</c>
    /// — aucun fragment anglais à traduire.
    /// </para>
    /// <para>
    /// Six valeurs sont légitimement identiques à l'anglais
    /// (<see cref="VerbatimByDesign"/>) : quatre marquees (Facebook, Google+,
    /// LinkedIn, Twitter), Skype ID, et le filtre alphabétique A–Z. La liste
    /// est épinglée : elle ne peut pas croître en silence.
    /// </para>
    /// </summary>
    public class DnnModuleLevelDtmAdminZhResxGuardTests
    {
        /// <summary>Couverture livrée : 20 fichiers, 640 clés, culture zh-CN.</summary>
        private static readonly (string Dir, string Stem, int DeliveredKeys)[] Covered =
        {
            ("Admin/Security", "EditGroups.ascx", 8),
            ("Admin/Security", "Register.ascx", 6),
            ("Admin/Security", "Roles.ascx", 17),
            ("Admin/Security", "EditUser.ascx", 20),
            ("Admin/Security", "SharedResources", 42),
            ("Admin/Security", "Membership.ascx", 30),
            ("Admin/Security", "MemberServices.ascx", 26),
            ("Admin/Security", "EditProfileDefinition.ascx", 56),
            ("Admin/Security", "EditRoles.ascx", 51),
            ("Admin/Security", "ManageUsers.ascx", 51),
            ("Admin/Security", "Password.ascx", 37),
            ("Admin/Security", "Profile.ascx", 93),
            ("Admin/Security", "ProfileDefinitions.ascx", 18),
            ("Admin/Security", "SecurityRoles.ascx", 32),
            ("Admin/Security", "User.ascx", 31),
            ("Admin/Security", "UserSettings.ascx", 43),
            ("Admin/Security", "Users.ascx", 61),
            ("Admin/ViewProfile", "Settings.ascx", 7),
            ("Admin/ViewProfile", "ViewProfile.ascx", 11),
            ("Admin/ViewProfile", "SharedResources", 0),
        };

        /// <summary>Total épinglé : la somme des <see cref="Covered"/>, jamais un littéral recopié.</summary>
        private static readonly int CoveredKeyTotal = Covered.Sum(c => c.DeliveredKeys);

        /// <summary>
        /// Clés neutres volontairement ABSENTES du pack (repli neutre DNN) :
        /// <c>ProfileDefinitions</c> — <c>Edit/Del/Dn/Up.Header</c> (quatre
        /// neutres VIDES : icônes de la grille, rien à localiser) ;
        /// <c>ViewProfile/SharedResources</c> — <c>DefaultTemplate</c> (toute
        /// la valeur vit dans des jetons <c>[PROFILE:…]</c>/<c>[USER:…]</c>).
        /// </summary>
        private static readonly (string Dir, string Stem, string[] Keys)[] DeclaredMissing =
        {
            ("Admin/Security", "ProfileDefinitions.ascx",
                new[] { "Edit.Header", "Del.Header", "Dn.Header", "Up.Header" }),
            ("Admin/ViewProfile", "SharedResources", new[] { "DefaultTemplate.Text" }),
        };

        /// <summary>
        /// Valeurs légitimement identiques à l'anglais — déclarées, pas tolérées :
        /// quatre marques + « Skype ID » + le filtre alphabétique A–Z (le
        /// filtrage par initiale pinyin utilise les mêmes lettres latines).
        /// Épinglée à 6 : toute croissance est une décision, pas un constat.
        /// </summary>
        private static readonly (string Dir, string Stem, string Key)[] VerbatimByDesign =
        {
            ("Admin/Security", "Profile.ascx", "ProfileProperties_Facebook.Text"),
            ("Admin/Security", "Profile.ascx", "ProfileProperties_GooglePlus.Text"),
            ("Admin/Security", "Profile.ascx", "ProfileProperties_LinkedIn.Text"),
            ("Admin/Security", "Profile.ascx", "ProfileProperties_Twitter.Text"),
            ("Admin/Security", "Profile.ascx", "ProfileProperties_Skype.Text"),
            ("Admin/Security", "Users.ascx", "Filter.Text"),
        };

        /// <summary>
        /// Bloc CJK : Ext-A (U+3400–U+4DBF), unifié (U+4E00–U+9FFF) et formes de
        /// compatibilité (U+F900–U+FAFF). Mesuré sur les 3 018 valeurs
        /// <c>*.zh-CN.resx</c> du dépôt : 2 997 portent de l'écriture chinoise et
        /// **toutes** relèvent du bloc unifié — aucune d'Ext-A, aucune des formes de
        /// compatibilité (0/2 997). Les deux plages hors bloc unifié sont donc une
        /// marge défensive, pas une nécessité mesurée.
        /// <para>
        /// Ce que ce contrôle ajoute au « ≠ anglais » du socle : une valeur peut
        /// différer de l'anglais sans porter un seul caractère chinois (espacement,
        /// ponctuation, marque latine, texte anglais reformulé) et traverser le
        /// contrôle du socle. Exiger au moins un caractère chinois ferme ce trou-là.
        /// </para>
        /// <para>
        /// Ce qu'il ne ferme PAS : une valeur à moitié traduite (moitié chinoise,
        /// moitié anglaise) porte déjà un caractère chinois, donc elle passe. Ce
        /// contrôle ne la voit pas et ne prétend pas la voir.
        /// </para>
        /// </summary>
        private static readonly Regex ChineseScript =
            new(@"[\u3400-\u4DBF\u4E00-\u9FFF\uF900-\uFAFF]", RegexOptions.Compiled);

        /// <summary>
        /// Valeurs dispensées du contrôle d'écriture : les verbatim déclarés
        /// (marques, Skype ID, A–Z) PLUS <c>MemberServices.ascx:TrialFee.Text</c>,
        /// qui n'est qu'un gabarit de mise en forme <c>{0}（{1} {2}）</c> — ses
        /// parenthèses pleine chasse sont hors des trois blocs CJK et sa prose
        /// vient des arguments. Liste ÉPINGLÉE à 7 : une exemption nouvelle est
        /// une décision, jamais un constat.
        /// </summary>
        private static readonly (string Dir, string Stem, string Key)[] ScriptExempt =
            VerbatimByDesign
                .Append(("Admin/Security", "MemberServices.ascx", "TrialFee.Text"))
                .ToArray();

        private static readonly Regex Placeholder = new(@"\{\d+\}", RegexOptions.Compiled);

        /// <summary>
        /// Identité d'un jeton DNN : namespace + chemin AVANT le « | ». La partie
        /// affichable qui suit est de la prose à traduire.
        /// </summary>
        private static readonly Regex Token = new(
            @"\[(?:Portal|User|Host|Profile|Membership|Custom|Date|Module|Tab|Site|Page|App):([^\]\|]*)",
            RegexOptions.Compiled);

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
        public void TheLotCoversTwentyFilesAndSixHundredFortyKeys()
        {
            Covered.Should().HaveCount(20,
                "le lot « Admin 3b » compte les 17 familles de Admin/Security et les 3 de Admin/ViewProfile "
                + "(dénominateur #2072 re-mesuré : 20 fichiers / 645 clés neutres)");
            CoveredKeyTotal.Should().Be(640,
                "645 clés neutres moins les 5 déclarées non livrées — dénominateur #2072, pas un littéral");
            Covered.Select(c => (c.Dir, c.Stem)).Should().OnlyHaveUniqueItems(
                "SharedResources existe dans les deux répertoires du lot : le couple identifie");
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
        /// Le BOM du fichier produit suit celui de SON neutre. Attente DÉRIVÉE du
        /// neutre, mesurée sur le <b>blob committé</b> (<c>git cat-file</c>), jamais
        /// sur l'arbre de travail : le runner CI a <c>core.autocrlf=true</c>, qui
        /// convertit LF → CRLF au checkout (règle payée 3× : #2078, #2087, #2088).
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
            VerbatimByDesign.Should().HaveCount(6,
                "six verbatim déclarés : quatre marques (Facebook, Google+, LinkedIn, Twitter), Skype ID, "
                + "et le filtre alphabétique A–Z — toute entrée nouvelle est une décision à consigner");
            foreach (var (dir, stem, key) in VerbatimByDesign)
            {
                Covered.Should().Contain(c => c.Dir == dir && c.Stem == stem,
                    $"le verbatim déclaré ({dir}/{stem}) porte sur un fichier du lot");
                Values(dir, stem, string.Empty).Should().ContainKey(key,
                    $"{dir}/{stem}:{key} : un verbatim déclaré doit exister au neutre — sinon la déclaration est morte");
            }
        }

        /// <summary>
        /// Contrôle d'écriture : chaque valeur livrée non dispensée porte au moins
        /// un caractère chinois. Il voit les valeurs qui diffèrent de l'anglais tout
        /// en n'étant pas chinoises (espacement, ponctuation, marque latine, anglais
        /// reformulé) ; il ne voit PAS une valeur à moitié traduite, qui porte déjà
        /// un caractère chinois.
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
        /// La liste des dispenses d'écriture est VIVANTE et ÉPINGLÉE : elle vaut
        /// exactement les 6 verbatim déclarés plus le gabarit de frais d'essai, et
        /// chaque dispense supplémentaire vise un fichier du lot et une clé qui
        /// existe au neutre.
        /// </summary>
        [Fact]
        public void TheScriptExemptListCannotGrowSilently()
        {
            ScriptExempt.Should().HaveCount(7,
                "sept dispenses d'écriture : six verbatim déclarés + le gabarit TrialFee.Text");

            ScriptExempt.Skip(6).Should().Equal(
                new[] { ("Admin/Security", "MemberServices.ascx", "TrialFee.Text") },
                "la seule dispense hors verbatim est le gabarit de mise en forme TrialFee.Text");

            foreach (var (dir, stem, key) in ScriptExempt)
            {
                Covered.Should().Contain(c => c.Dir == dir && c.Stem == stem,
                    $"la dispense ({dir}/{stem}) porte sur un fichier du lot");
                Values(dir, stem, string.Empty).Should().ContainKey(key,
                    $"{dir}/{stem}:{key} : une dispense doit exister au neutre — sinon la déclaration est morte");
            }
        }

        /// <summary>
        /// La déclaration des manquantes est VIVANTE : chaque entrée vise un fichier
        /// du lot, porte des clés qui existent au neutre, et le total épinglé (5)
        /// interdit la croissance silencieuse.
        /// </summary>
        [Fact]
        public void TheDeclaredMissingListIsLiveAndPinned()
        {
            DeclaredMissing.Length.Should().Be(2,
                "deux fichiers portent des manquantes déclarées (ProfileDefinitions, ViewProfile/SharedResources)");
            DeclaredMissing.Sum(m => m.Keys.Length).Should().Be(5,
                "5 clés déclarées non livrées : 4 en-têtes à neutre vide + 1 gabarit à jetons purs");
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
