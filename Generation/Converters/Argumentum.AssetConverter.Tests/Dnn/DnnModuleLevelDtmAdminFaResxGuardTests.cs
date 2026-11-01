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
    /// fa-IR : jumeau persan des lots zh-CN 3b (<see cref="DnnModuleLevelDtmAdminZhResxGuardTests"/>)
    /// et ar-SA (<see cref="DnnModuleLevelDtmAdminArResxGuardTests"/>). Même
    /// périmètre, mêmes comptes : <c>Admin/Security</c> (17 fichiers) +
    /// <c>Admin/ViewProfile</c> (3 fichiers), 640 clés livrées sur 645 neutres.
    /// <para>
    /// ⚠️ <c>SharedResources</c> existe dans les DEUX répertoires du lot : la
    /// couverture est identifiée par le COUPLE (répertoire, stem), jamais par le
    /// stem seul. <c>ViewProfile/SharedResources</c> est livré en COQUILLE
    /// (0 clé, 5 699 o — même taille que les coquilles zh-CN et ar-SA) : sa clé
    /// unique est déclarée manquante, DNN retombe sur le neutre.
    /// </para>
    /// <para>
    /// ⚠️ Le pool annonçait « <c>DesktopModules/Admin</c> » — l'arbre en porte 65
    /// fichiers neutres / 4 395 clés, dont l'écrasante majorité n'appartient à
    /// AUCUN lot livré. Le périmètre se lit dans la table <see cref="Covered"/>,
    /// jamais dans l'étiquette.
    /// </para>
    /// <para>
    /// <b>Résidu mesuré, nommé, hors lot</b> : <c>Security/DataConsent.ascx</c>
    /// (7 clés neutres) porte des sœurs <c>es-ES</c>/<c>ru-RU</c>/<c>zh-CN</c> —
    /// des packs DNN d'origine, pas des lots de traduction — et n'a NI ar-SA NI
    /// fa-IR. Il n'entre pas dans ce lot : le jumeau ar-SA ne l'a pas livré non
    /// plus, et la règle « un lot par garde » vaut. Il est épinglé ici pour
    /// qu'une livraison future le déclare au lieu de le glisser.
    /// </para>
    /// <para>
    /// Ce que cette garde ajoute à sa jumelle ar : le SENS de la distinction.
    /// Le persan et l'arabe partagent le bloc U+0600–U+06FF, donc « porte de
    /// l'écriture arabe » ne distingue pas un pack persan d'un pack arabe. Ici
    /// la règle ferme le sens inverse — aucune valeur livrée ne porte les trois
    /// lettres EXCLUSIVES à l'arabe (<c>ي</c> U+064A, <c>ك</c> U+0643,
    /// <c>ى</c> U+0649) — et son contrôle inverse est rejoué DANS la suite sur
    /// le corpus ar-SA du dépôt, pas seulement consigné dans une note.
    /// </para>
    /// </summary>
    public class DnnModuleLevelDtmAdminFaResxGuardTests
    {
        /// <summary>Couverture livrée : 20 fichiers, 640 clés, culture fa-IR.</summary>
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
        /// Clés neutres volontairement ABSENTES du pack (repli neutre DNN) —
        /// les mêmes que les lots zh et ar : <c>ProfileDefinitions</c> —
        /// <c>Edit/Del/Dn/Up.Header</c> (quatre neutres VIDES : icônes de la
        /// grille) ; <c>ViewProfile/SharedResources</c> — <c>DefaultTemplate</c>
        /// (toute la valeur vit dans des jetons).
        /// </summary>
        private static readonly (string Dir, string Stem, string[] Keys)[] DeclaredMissing =
        {
            ("Admin/Security", "ProfileDefinitions.ascx",
                new[] { "Edit.Header", "Del.Header", "Dn.Header", "Up.Header" }),
            ("Admin/ViewProfile", "SharedResources", new[] { "DefaultTemplate.Text" }),
        };

        /// <summary>
        /// Valeurs légitimement identiques à l'anglais — déclarées, pas tolérées :
        /// quatre marques + « Skype ID » + le filtre alphabétique A–Z. Épinglée à
        /// 6 : toute croissance est une décision, pas un constat.
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
        /// Écriture arabo-persane : bloc U+0621–U+064A plus les lettres propres au
        /// persan (پ U+067E, چ U+0686, ژ U+0698, ک U+06A9, گ U+06AF) et ی U+06CC.
        /// Ferme une valeur qui diffère de l'anglais sans porter aucun caractère
        /// de cette écriture.
        /// </summary>
        private static readonly Regex PersianScript = new(
            "[ء-يپچژکگی]", RegexOptions.Compiled);

        /// <summary>
        /// Les trois lettres EXCLUSIVES à l'arabe : ي U+064A, ك U+0643, ى U+0649.
        /// Un texte persan écrit correctement n'en porte aucune (il emploie
        /// ی U+06CC et ک U+06A9). C'est la règle qui distingue un pack persan
        /// d'un pack arabe recopié — les deux passent <see cref="PersianScript"/>.
        /// <para>
        /// Calibration mesurée le 2026-11-01 sur le corpus <c>*.ar-SA.resx</c> du
        /// dépôt (75 fichiers, 2 378 valeurs non vides) : <b>1 616 valeurs</b>
        /// portent au moins une de ces lettres, contre <b>0 sur 640</b> ici. Un
        /// zéro n'est un résultat que si le même instrument rend non-zéro là où
        /// le défaut existe.
        /// </para>
        /// </summary>
        private static readonly Regex ArabicExclusive = new(
            "[يكى]", RegexOptions.Compiled);

        /// <summary>
        /// Valeurs dispensées du contrôle d'écriture : exactement les 6 verbatim
        /// (marques latines et filtre A–Z). Contrairement à la jumelle zh, le
        /// gabarit <c>TrialFee.Text</c> n'a pas besoin de dispense : sa prose
        /// persane (<c>{0} برای {1} {2}</c>) porte l'écriture. Épinglée.
        /// </summary>
        private static readonly (string Dir, string Stem, string Key)[] ScriptExempt = VerbatimByDesign;

        private static readonly Regex Placeholder = new(@"\{\d+\}", RegexOptions.Compiled);

        /// <summary>
        /// Identité d'un jeton DNN : namespace + chemin AVANT le « | ». La partie
        /// affichable qui suit est de la prose à traduire.
        /// </summary>
        private static readonly Regex Token = new(
            @"\[(?:Portal|User|Host|Profile|Membership|Custom|Date|Module|Tab|Site|Page|App):([^\]\|]*)",
            RegexOptions.Compiled);

        private const string Culture = "fa-IR";

        /// <summary>
        /// Résidu hors lot, mesuré : <c>Security/DataConsent.ascx</c> — 7 clés
        /// neutres, sœurs es-ES/ru-RU/zh-CN (packs DNN d'origine), ni ar-SA ni
        /// fa-IR. Épinglé pour qu'une livraison future le déclare.
        /// </summary>
        private const string OutOfLotStem = "DataConsent.ascx";

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
                "le lot « Admin » fa-IR couvre les 17 familles de Admin/Security et les 3 de "
                + "Admin/ViewProfile — l'arbre Admin en porte 65 fichiers neutres, dont 45 hors de tout lot livré");
            CoveredKeyTotal.Should().Be(640,
                "645 clés neutres moins les 5 déclarées non livrées");
            Covered.Select(c => (c.Dir, c.Stem)).Should().OnlyHaveUniqueItems(
                "SharedResources existe dans les deux répertoires du lot : le couple identifie");
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
        /// Le BOM du fichier produit suit celui de SON neutre. Attente DÉRIVÉE du
        /// neutre, mesurée sur le <b>blob committé</b> (<c>git cat-file</c>), jamais
        /// sur l'arbre de travail : le runner CI a <c>core.autocrlf=true</c>.
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
            VerbatimByDesign.Should().HaveCount(6,
                "six verbatim déclarés : quatre marques, Skype ID, et le filtre alphabétique A–Z");
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
        /// un caractère de l'écriture arabo-persane (voir <see cref="PersianScript"/>).
        /// Il ne voit PAS une valeur à moitié traduite.
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void EveryNonExemptValueCarriesPersianScript(string dir, string stem, int _)
        {
            foreach (var (key, value) in Values(dir, stem, Culture))
            {
                if (ScriptExempt.Contains((dir, stem, key)))
                    continue;

                PersianScript.IsMatch(value).Should().BeTrue(
                    $"{RelOf(dir, stem, Culture)}:{key} — la valeur ne porte aucun caractère persan ; "
                    + $"valeur = « {value} »");
            }
        }

        /// <summary>
        /// Persan ≠ arabe (décision 33) : aucune valeur livrée ne porte une lettre
        /// exclusive à l'arabe. Ce contrôle attrape un pack ar-SA recopié sous
        /// <c>fa-IR</c>, que le contrôle d'écriture laisserait passer à 100 %
        /// puisque les deux écritures partagent le même bloc Unicode.
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void NoDeliveredValueCarriesArabicExclusiveLetters(string dir, string stem, int _)
        {
            foreach (var (key, value) in Values(dir, stem, Culture))
            {
                var bad = ArabicExclusive.Matches(value).Select(m => m.Value).Distinct().ToArray();
                bad.Should().BeEmpty(
                    $"{RelOf(dir, stem, Culture)}:{key} — lettres arabes exclusives [{string.Join(",", bad)}] : "
                    + "le persan écrit correctement emploie ی U+06CC et ک U+06A9 ; "
                    + $"valeur = « {value} »");
            }
        }

        /// <summary>
        /// Contrôle inverse EN SÉANCE : le détecteur doit voir le défaut quand on le
        /// lui présente. Échantillons écrits en échappements Unicode, donc immunisés
        /// contre la normalisation d'un copier-coller ou d'un éditeur.
        /// </summary>
        [Fact]
        public void TheArabicExclusiveDetectorCanSeeADefect()
        {
            const string arabicKaf = "كتاب";   // كاف عربي
            const string persianKaf = "کتاب";  // کاف فارسي
            const string arabicYeh = "مي";               // ي عربي
            const string persianYeh = "می";              // ی فارسي

            PersianScript.IsMatch(arabicKaf).Should().BeTrue("les deux écritures partagent le bloc U+0600–U+06FF");
            PersianScript.IsMatch(persianKaf).Should().BeTrue();

            ArabicExclusive.IsMatch(arabicKaf).Should().BeTrue("l'échantillon porte ك U+0643");
            ArabicExclusive.IsMatch(persianKaf).Should().BeFalse("l'échantillon porte ک U+06A9");
            ArabicExclusive.IsMatch(arabicYeh).Should().BeTrue("l'échantillon porte ي U+064A");
            ArabicExclusive.IsMatch(persianYeh).Should().BeFalse("l'échantillon porte ی U+06CC");
        }

        /// <summary>
        /// Étalonnage du zéro sur données RÉELLES : le même instrument, rejoué sur le
        /// corpus <c>*.ar-SA.resx</c> du dépôt, doit rendre un compte largement non
        /// nul. Mesuré le 2026-11-01 : 1 616 valeurs sur 2 378 (75 fichiers).
        /// Plancher, pas compte exact : les lots ar-SA à venir ne doivent pas casser
        /// cette garde — ils ne peuvent que l'augmenter.
        /// </summary>
        [Fact]
        public void TheArabicExclusiveDetectorIsCalibratedOnTheArCorpus()
        {
            var root = Path.Combine(TestRepoRoot.Find(), "DNNPlatform");
            Directory.Exists(root).Should().BeTrue("le corpus ar-SA vit sous DNNPlatform/");

            var files = Directory.EnumerateFiles(root, "*.ar-SA.resx", SearchOption.AllDirectories).ToArray();
            files.Length.Should().BeGreaterThanOrEqualTo(40,
                "le dépôt porte au moins 40 fichiers ar-SA — en dessous, l'instrument regarde au mauvais endroit");

            var values = 0;
            var withArabicExclusive = 0;
            foreach (var file in files)
            {
                foreach (var data in XDocument.Load(file).Root!.Elements("data"))
                {
                    var value = (string?)data.Element("value") ?? string.Empty;
                    if (string.IsNullOrWhiteSpace(value))
                        continue;
                    values++;
                    if (ArabicExclusive.IsMatch(value))
                        withArabicExclusive++;
                }
            }

            withArabicExclusive.Should().BeGreaterThanOrEqualTo(1000,
                $"le corpus ar-SA doit massivement porter ي/ك/ى "
                + $"(mesuré 2026-11-01 : 1 616 / 2 378 ; ici {withArabicExclusive} / {values}) — "
                + "sans ce plancher, le zéro du pack fa-IR pourrait être un instrument aveugle");
            withArabicExclusive.Should().BeLessThanOrEqualTo(values, "le compte ne peut pas dépasser le corpus");
        }

        /// <summary>
        /// La liste des dispenses d'écriture est VIVANTE et ÉPINGLÉE : identique aux
        /// verbatim déclarés (le gabarit persan <c>TrialFee.Text</c> porte sa prose
        /// en persan, il n'a pas besoin de dispense contrairement à la zh).
        /// </summary>
        [Fact]
        public void TheScriptExemptListCannotGrowSilently()
        {
            ScriptExempt.Should().HaveCount(6,
                "six dispenses d'écriture : exactement les verbatim déclarés — aucune dispense supplémentaire en fa-IR");
            ScriptExempt.Should().Equal(VerbatimByDesign,
                "la dispense d'écriture ne doit pas dépasser les verbatim : toute exemption nouvelle est une décision");

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

        /// <summary>
        /// Le résidu <c>DataConsent.ascx</c> est HORS lot et le reste : il porte des
        /// sœurs de culture de stock (es/ru/zh) mais ni ar-SA ni fa-IR. Ce test le
        /// nomme — une livraison fa-IR de ce fichier devra passer par une décision
        /// (mise à jour de <see cref="Covered"/>), pas par un ajout silencieux.
        /// </summary>
        [Fact]
        public void TheOutOfLotResidualIsNamedAndStillOutOfLot()
        {
            Covered.Should().NotContain(c => c.Stem == OutOfLotStem,
                "DataConsent est mesuré hors lot : le jumeau ar-SA ne l'a pas livré");

            var neutral = Values("Admin/Security", OutOfLotStem, string.Empty);
            neutral.Should().HaveCount(7,
                $"DataConsent porte 7 clés neutres (mesuré 2026-11-01) ; ici {neutral.Count} — "
                + "si le neutre a bougé, le résidu doit être re-mesuré, pas recopié");

            File.Exists(PathOf("Admin/Security", OutOfLotStem, Culture)).Should().BeFalse(
                $"{RelOf("Admin/Security", OutOfLotStem, Culture)} : le jour où ce fichier existe, il faut "
                + "l'ajouter à Covered et à ce test DÉLIBÉRÉMENT — pas le laisser entrer par la porte de service");
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
