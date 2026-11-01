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
    /// #458 grain module-level — garde du lot <c>DesktopModules</c> « Admin »
    /// ar-SA : jumeau arabophone du lot zh-CN 3b (<see cref="DnnModuleLevelDtmAdminZhResxGuardTests"/>).
    /// Même périmètre, mêmes comptes : <c>Admin/Security</c> (17 fichiers) +
    /// <c>Admin/ViewProfile</c> (3 fichiers), 640 clés livrées sur 645 neutres.
    /// <para>
    /// ⚠️ <c>SharedResources</c> existe dans les DEUX répertoires du lot : la
    /// couverture est identifiée par le COUPLE (répertoire, stem), jamais par le
    /// stem seul. <c>ViewProfile/SharedResources</c> est livré en COQUILLE
    /// (0 clé, 5 699 o — même taille que la zh-CN) : sa clé unique est déclarée
    /// manquante, DNN retombe sur le neutre.
    /// </para>
    /// <para>
    /// ⚠️ Le pool annonçait « <c>DesktopModules/Admin</c> » — l'arbre en porte 65
    /// fichiers neutres / 4 395 clés, dont l'écrasante majorité (PersonaBar,
    /// Console, EditBar, HtmlEditor…) n'appartient à AUCUN lot livré. Le
    /// périmètre se lit dans la table <see cref="Covered"/>, jamais dans
    /// l'étiquette. Même famille que « 9 fichiers / 381 clés → 37 / 689 ».
    /// </para>
    /// <para>
    /// Cinq clés neutres ne sont PAS livrées (<see cref="DeclaredMissing"/>) :
    /// quatre en-têtes de colonne à neutre VIDE (<c>Edit/Del/Dn/Up.Header</c> de
    /// <c>ProfileDefinitions</c> — rien à localiser) et le gabarit de
    /// <c>ViewProfile/SharedResources</c> dont TOUTE la valeur vit dans des
    /// jetons <c>[PROFILE:…]</c>/<c>[USER:…]</c>.
    /// </para>
    /// <para>
    /// Six valeurs sont légitimement identiques à l'anglais
    /// (<see cref="VerbatimByDesign"/>) : quatre marques (Facebook, Google+,
    /// LinkedIn, Twitter), Skype ID, et le filtre alphabétique A–Z. La liste est
    /// épinglée : elle ne peut pas croître en silence.
    /// </para>
    /// <para>
    /// Ce que cette garde ajoute à sa jumelle zh : <b>persan ≠ arabe</b>. Les deux
    /// écritures partagent le bloc U+0600–U+06FF, donc « porte une lettre du bloc »
    /// accepte un pack persan recopié sous <c>ar-SA</c>. Le persan écrit correct
    /// emploie <c>ی</c> U+06CC et <c>ک</c> U+06A9 là où l'arabe écrit <c>ي</c>
    /// U+064A et <c>ك</c> U+0643 ; ces caractères sont donc EXCLUS du pack ar-SA.
    /// Le zéro est calibré par un contrôle inverse rejoué sur le corpus fa-IR du
    /// dépôt (<see cref="ThePersianDetectorIsCalibratedOnTheFaCorpus"/>).
    /// </para>
    /// </summary>
    public class DnnModuleLevelDtmAdminArResxGuardTests
    {
        /// <summary>Couverture livrée : 20 fichiers, 640 clés, culture ar-SA.</summary>
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
        /// neutres VIDES : icônes de la grille) ; <c>ViewProfile/SharedResources</c>
        /// — <c>DefaultTemplate</c> (toute la valeur vit dans des jetons).
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
        /// Lettres arabes : bloc principal U+0621–U+064A. Ce que ce contrôle ajoute
        /// au « ≠ anglais » du socle : une valeur peut différer de l'anglais sans
        /// porter une seule lettre arabe (espacement, ponctuation, marque latine,
        /// texte anglais reformulé) et traverser le contrôle du socle.
        /// <para>
        /// Ce qu'il ne ferme PAS : une valeur à moitié traduite porte déjà une
        /// lettre arabe, donc elle passe. Il ne la voit pas et ne prétend pas la voir.
        /// </para>
        /// </summary>
        private static readonly Regex ArabicScript = new(@"[ء-ي]", RegexOptions.Compiled);

        /// <summary>
        /// Lettres EXCLUSIVES au persan dans le bloc partagé U+0600–U+06FF :
        /// <c>پ</c> U+067E, <c>چ</c> U+0686, <c>ژ</c> U+0698, <c>گ</c> U+06AF,
        /// <c>ک</c> U+06A9, <c>ی</c> U+06CC. L'arabe écrit <c>ك</c> U+0643 et
        /// <c>ي</c> U+064A : une valeur ar-SA correcte n'en porte aucun.
        /// <para>
        /// Calibration mesurée le 2026-11-01 sur le corpus <c>*.fa-IR.resx</c> du
        /// dépôt : <b>1 452 des 1 710 valeurs</b> portent au moins une de ces
        /// lettres, contre <b>0 sur 640</b> ici. Un zéro n'est un résultat que si
        /// le même instrument rend non-zéro là où le défaut existe.
        /// </para>
        /// </summary>
        private static readonly Regex PersianExclusive = new(
            "[پچژگکی]", RegexOptions.Compiled);

        /// <summary>
        /// Valeurs dispensées du contrôle d'écriture. Contrairement à la jumelle zh
        /// (qui devait exempter le gabarit <c>TrialFee.Text</c>, ses parenthèses
        /// pleine chasse étant hors blocs CJK), le gabarit arabe
        /// <c>{0} لمدة {1} {2}</c> porte sa prose en arabe : la liste des dispenses
        /// vaut donc EXACTEMENT les 6 verbatim. Épinglée.
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

        private const string Culture = "ar-SA";

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
                "le lot « Admin » ar-SA couvre les 17 familles de Admin/Security et les 3 de "
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
        /// une lettre arabe. Il voit les valeurs qui diffèrent de l'anglais tout en
        /// n'étant pas arabes ; il ne voit PAS une valeur à moitié traduite.
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void EveryNonExemptValueCarriesArabicScript(string dir, string stem, int _)
        {
            foreach (var (key, value) in Values(dir, stem, Culture))
            {
                if (ScriptExempt.Contains((dir, stem, key)))
                    continue;

                ArabicScript.IsMatch(value).Should().BeTrue(
                    $"{RelOf(dir, stem, Culture)}:{key} — la valeur ne porte aucune lettre arabe ; "
                    + $"valeur = « {value} »");
            }
        }

        /// <summary>
        /// Persan ≠ arabe (décision 33, deux faits de lot) : aucune valeur livrée ne
        /// porte une lettre exclusive au persan. Ce contrôle attrape un pack fa-IR
        /// recopié sous <c>ar-SA</c>, que le contrôle d'écriture arabe laisserait
        /// passer à 100 % puisque les deux écritures partagent le même bloc Unicode.
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void NoDeliveredValueCarriesAPersianExclusiveLetter(string dir, string stem, int _)
        {
            foreach (var (key, value) in Values(dir, stem, Culture))
            {
                var hit = PersianExclusive.Match(value);
                hit.Success.Should().BeFalse(
                    $"{RelOf(dir, stem, Culture)}:{key} — lettre persane exclusive "
                    + $"U+{(int)hit.Value.FirstOrDefault():X4} dans « {value} » : le pack ar-SA ne doit pas "
                    + "être un pack persan recopié");
            }
        }

        /// <summary>
        /// Contrôle inverse EN SÉANCE : le détecteur doit voir le défaut quand on le
        /// lui présente. Échantillons écrits en échappements Unicode, donc immunisés
        /// contre la normalisation d'un copier-coller ou d'un éditeur.
        /// </summary>
        [Fact]
        public void ThePersianDetectorCanSeeADefect()
        {
            const string persianKaf = "کتاب";   // کتاب (ک persan)
            const string arabicKaf = "كتاب";    // كتاب (ك arabe)
            const string persianYeh = "می";               // می (ی persan)

            PersianExclusive.IsMatch(persianKaf).Should().BeTrue("ک U+06A9 est exclusif au persan");
            PersianExclusive.IsMatch(persianYeh).Should().BeTrue("ی U+06CC est exclusif au persan");
            PersianExclusive.IsMatch(arabicKaf).Should().BeFalse(
                "كتاب est de l'arabe correct : le détecteur ne doit pas rougir sur le lot qu'il garde");
            ArabicScript.IsMatch(arabicKaf).Should().BeTrue("le contrôle d'écriture voit bien l'arabe");
        }

        /// <summary>
        /// Étalonnage du zéro sur données RÉELLES : le même instrument, rejoué sur le
        /// corpus <c>*.fa-IR.resx</c> du dépôt, doit rendre un compte largement
        /// non nul. Mesuré le 2026-11-01 : 1 452 valeurs sur 1 710. Plancher, pas
        /// compte exact : les lots fa-IR à venir ne doivent pas casser cette garde —
        /// ils ne peuvent que l'augmenter.
        /// </summary>
        [Fact]
        public void ThePersianDetectorIsCalibratedOnTheFaCorpus()
        {
            var root = Path.Combine(TestRepoRoot.Find(), "DNNPlatform");
            Directory.Exists(root).Should().BeTrue("le corpus fa-IR vit sous DNNPlatform/");

            var files = Directory.EnumerateFiles(root, "*.fa-IR.resx", SearchOption.AllDirectories).ToArray();
            files.Length.Should().BeGreaterThanOrEqualTo(40,
                "le dépôt porte au moins 40 fichiers fa-IR — en dessous, l'instrument regarde au mauvais endroit");

            var values = 0;
            var withPersian = 0;
            foreach (var file in files)
            {
                foreach (var data in XDocument.Load(file).Root!.Elements("data"))
                {
                    var value = (string?)data.Element("value") ?? string.Empty;
                    if (string.IsNullOrWhiteSpace(value))
                        continue;
                    values++;
                    if (PersianExclusive.IsMatch(value))
                        withPersian++;
                }
            }

            withPersian.Should().BeGreaterThanOrEqualTo(1000,
                $"le corpus fa-IR doit massivement porter des lettres persanes exclusives "
                + $"(mesuré 2026-11-01 : 1 452 / 1 710 ; ici {withPersian} / {values}) — "
                + "sans ce plancher, le zéro du pack ar-SA pourrait être un instrument aveugle");
            withPersian.Should().BeLessThanOrEqualTo(values, "le compte ne peut pas dépasser le corpus");
        }

        /// <summary>
        /// La liste des dispenses d'écriture est VIVANTE et ÉPINGLÉE : identique aux
        /// verbatim déclarés (le gabarit arabe <c>TrialFee.Text</c> porte sa prose en
        /// arabe, il n'a pas besoin de dispense contrairement à la zh).
        /// </summary>
        [Fact]
        public void TheScriptExemptListCannotGrowSilently()
        {
            ScriptExempt.Should().HaveCount(6,
                "six dispenses d'écriture : exactement les verbatim déclarés — aucune dispense supplémentaire en ar-SA");
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

        /// <summary>Lit le blob de <c>HEAD:&lt;path&gt;</c> — indépendant de core.autocrlf.</summary>
        private static byte[] CommittedBlob(string rel) => DnnResxGuardSocle.CommittedBlob(rel);
    }
}
