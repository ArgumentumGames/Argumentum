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
    /// fa-IR : mêmes 27 fichiers / 668 clés que les lots zh-CN (#2089) et ar-SA
    /// du même périmètre, sur les HUIT répertoires <c>DesktopModules/{AuthenticationServices/DNN,
    /// CoreMessaging, DDRMenu, HTML, Journal, MemberDirectory, RazorModules/RazorHost,
    /// SocialGroups}/App_LocalResources</c> — mêmes 10 déclarées manquantes
    /// (repli neutre DNN), zéro verbatim.
    /// </summary>
    /// <remarks>
    /// <para>
    /// ⚠️ Classe autonome miroir des gardes zh et ar, PAS un socle factorisé :
    /// la règle « un lot par garde » tient tant que la séquence n'est pas close ;
    /// la factorisation est le grain désigné ensuite, comme <c>controls</c> et
    /// <c>admin</c> l'ont fait.
    /// </para>
    /// <para>
    /// Les stems collident massivement entre répertoires (<c>Settings.ascx</c> ×5,
    /// <c>View.ascx</c> ×4, <c>SharedResources</c> ×4, <c>Edit.ascx</c> ×2) : la
    /// couverture est identifiée par le COUPLE (répertoire, stem).
    /// </para>
    /// <para>
    /// Ce que cette garde ajoute par rapport à la garde ar : le persan et l'arabe
    /// partagent le bloc U+0600–U+06FF, donc « porte de l'écriture arabe » ne
    /// distingue PAS une valeur persane d'une valeur arabe. La règle
    /// <see cref="NoDeliveredValueCarriesArabicExclusiveLetters"/> ferme cette
    /// confusion sur les trois lettres propres à l'arabe ; son contrôle inverse
    /// est joué dans la suite, pas seulement hors de la suite.
    /// </para>
    /// </remarks>
    public class DnnModuleLevelDtmModulesFaResxGuardTests
    {
        private const string Culture = "fa-IR";

        /// <summary>Couverture livrée : 27 fichiers, 668 clés — mêmes comptes que les lots zh (#2089) et ar.</summary>
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
        /// les mêmes que les lots zh et ar : <c>CoreMessaging/SharedResources</c> —
        /// <c>ResultCount</c>/<c>SingleResultCount</c> (<c>{0}</c> pur) ;
        /// <c>Journal/SharedResources</c> — <c>Notification_Mention.Body</c>
        /// (<c>{0}</c> pur) et les quatre gabarits <c>journal_status/link/photo/file</c>
        /// (balisage + jetons <c>[JournalControl:…]</c>, zéro prose) ;
        /// <c>RazorHost/CreateModule</c> — <c>ManifestText</c> (manifest XML) et
        /// <c>ModuleControlText</c> (directive) ; <c>SocialGroups/SharedResources</c> —
        /// <c>GroupListTemplate</c> (toute la prose vit dans des <c>{resx:…}</c> déjà livrés).
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
        /// Ce lot n'en porte AUCUNE (668 valeurs persanes) : l'épinglage à 0 interdit
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
            @"\[(?:Portal|User|Host|Profile|Membership|Custom|Date|Module|Tab|Site|Page|App|Journal)[^\]]*",
            RegexOptions.Compiled);

        /// <summary>
        /// Écriture arabo-persane : bloc U+0621–U+064A plus les cinq lettres
        /// propres au persan (پ U+067E, چ U+0686, ژ U+0698, ک U+06A9, گ U+06AF)
        /// et ی U+06CC. Ferme une valeur qui diffère de l'anglais sans porter
        /// aucun caractère de cette écriture.
        /// </summary>
        private static readonly Regex PersianScript = new(
            "[ء-يپچژکگی]", RegexOptions.Compiled);

        /// <summary>
        /// Les trois lettres EXCLUSIVES à l'arabe : ي U+064A, ك U+0643, ى U+0649.
        /// Un texte persan écrit correctement n'en porte aucune (il emploie
        /// ی U+06CC et ک U+06A9). C'est la règle qui distingue un pack persan
        /// d'un pack arabe recopié — les deux passent <see cref="PersianScript"/>.
        /// </summary>
        private static readonly Regex ArabicExclusive = new(
            "[يكى]", RegexOptions.Compiled);

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
                "le lot « modules » fa-IR reprend les 27 familles CORE des lots zh et ar — PersonaBar et NBright exclus par #2072");
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
        /// Toute valeur livrée porte l'écriture arabo-persane — voir
        /// <see cref="PersianScript"/> pour ce que la règle ferme.
        /// </summary>
        [Theory]
        [MemberData(nameof(CoveredFiles))]
        public void EveryValueCarriesPersianScript(string dir, string stem, int _)
        {
            foreach (var (key, value) in Values(dir, stem, Culture))
                PersianScript.IsMatch(value).Should().BeTrue(
                    $"{RelOf(dir, stem, Culture)}:{key} — la valeur ne porte aucun caractère persan ; "
                    + $"valeur = « {value} »");
        }

        /// <summary>
        /// Aucune valeur livrée ne porte une lettre exclusive à l'arabe. Sans
        /// cette règle, un pack arabe recopié sous <c>fa-IR</c> passerait toutes
        /// les autres assertions (même bloc Unicode, même absence de latin).
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
        /// Contrôle INVERSE de <see cref="ArabicExclusive"/> : le détecteur doit
        /// voir le défaut là où il existe. Mesuré hors suite sur les fichiers
        /// ar-SA du même périmètre : 390 des 668 valeurs portent ي/ك/ى — le zéro
        /// rendu sur les fa-IR n'est donc pas un détecteur aveugle.
        /// <para>
        /// Les échantillons sont écrits en échappements Unicode : un littéral
        /// recopié depuis une page web peut être normalisé par le navigateur ou
        /// l'éditeur et changer de point de code sans changer d'apparence.
        /// </para>
        /// </summary>
        [Fact]
        public void TheArabicExclusiveDetectorCanSeeADefect()
        {
            const string arabicKaf = "كتاب";   // كتاب
            const string persianKaf = "کتاب";  // کتاب
            const string arabicYeh = "مي";               // مي
            const string persianYeh = "می";              // می

            PersianScript.IsMatch(arabicKaf).Should().BeTrue("les deux écritures partagent le bloc U+0600–U+06FF");
            PersianScript.IsMatch(persianKaf).Should().BeTrue();

            ArabicExclusive.IsMatch(arabicKaf).Should().BeTrue("كتاب porte ك U+0643");
            ArabicExclusive.IsMatch(persianKaf).Should().BeFalse("کتاب porte ک U+06A9");
            ArabicExclusive.IsMatch(arabicYeh).Should().BeTrue("مي porte ي U+064A");
            ArabicExclusive.IsMatch(persianYeh).Should().BeFalse("می porte ی U+06CC");
        }

        /// <summary>
        /// Le BOM du fichier produit suit celui de SON neutre — le module-level est
        /// hétérogène : dans ce lot, <c>AuthenticationServices/DNN/Settings</c>,
        /// <c>HTML/Settings</c> et <c>HTML/SharedResources</c> n'ont PAS de BOM
        /// quand les 24 autres en ont un. Attente DÉRIVÉE du neutre, jamais recopiée.
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
                "ce lot ne porte aucune copie de l'anglais par conception (668 valeurs persanes) : chaque ajout doit être "
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
        private static byte[] CommittedBlob(string rel) => DnnResxGuardSocle.CommittedBlob(rel);
    }
}
