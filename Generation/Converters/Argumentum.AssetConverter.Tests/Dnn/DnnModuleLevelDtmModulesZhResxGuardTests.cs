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
    /// zh-CN, troisième lot du périmètre mesuré par #2072 (CORE sans
    /// <c>Install/</c> : 94 fichiers, 2 168 clés par langue ; ce lot :
    /// 27 fichiers répartis sur HUIT répertoires
    /// <c>DesktopModules/{AuthenticationServices/DNN, CoreMessaging, DDRMenu,
    /// HTML, Journal, MemberDirectory, RazorModules/RazorHost,
    /// SocialGroups}/App_LocalResources</c>, 668 clés livrées sur 678 neutres).
    /// <para>
    /// ⚠️ Les stems collident massivement entre répertoires (<c>Settings.ascx</c> ×5,
    /// <c>View.ascx</c> ×4, <c>SharedResources</c> ×4, <c>Edit.ascx</c> ×2) : la
    /// couverture est identifiée par le COUPLE (répertoire, stem), jamais par le
    /// stem seul.
    /// </para>
    /// <para>
    /// Comme les lots <c>controls</c> et <c>admin</c>, la référence est le NEUTRE,
    /// jamais ru-RU (15/96 fichiers CORE divergent de ru, #2072), et la garde
    /// n'épingle que le lot livré : un lot suivant ajoute ses entrées, pas ses règles.
    /// </para>
    /// <para>
    /// Dix clés neutres ne sont PAS livrées (<see cref="DeclaredMissing"/>) :
    /// repli neutre DNN volontaire. Deux familles y figurent : format pur
    /// (<c>{0}</c> sans prose : compteurs, corps de notification médaillon) et
    /// markup pur (manifest XML et directive <c>&lt;%@ Control %&gt;</c> de
    /// RazorHost ; gabarits de flux dont TOUTE la prose vit dans des jetons
    /// <c>[Journal…]</c> ou des références <c>{resx:…}</c> — aucun fragment
    /// anglais à traduire, le repli neutre rend la même chose).
    /// </para>
    /// <para>
    /// Depuis #2098 : contrôle d'écriture chinoise
    /// (<see cref="EveryNonExemptValueCarriesChineseScript"/>) — chaque valeur
    /// livrée porte au moins un caractère CJK, dispenses épinglées à 0
    /// (<see cref="ScriptExempt"/>, census : 668/668).
    /// </para>
    /// </summary>
    public class DnnModuleLevelDtmModulesZhResxGuardTests
    {
        /// <summary>Couverture livrée : 27 fichiers, 668 clés, culture zh-CN.</summary>
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
        /// Clés neutres volontairement ABSENTES du pack (repli neutre DNN) :
        /// <c>CoreMessaging/SharedResources</c> — <c>ResultCount</c>/<c>SingleResultCount</c>
        /// (<c>{0}</c> pur) ; <c>Journal/SharedResources</c> — <c>Notification_Mention.Body</c>
        /// (<c>{0}</c> pur) et les quatre gabarits <c>journal_status/link/photo/file</c>
        /// (zéro prose : jetons + markup) ; <c>RazorHost/CreateModule</c> —
        /// <c>ManifestText</c> (manifest XML à placeholders) et <c>ModuleControlText</c>
        /// (directive <c>&lt;%@ Control %&gt;</c>) ; <c>SocialGroups/SharedResources</c> —
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
        /// Ce lot n'en porte AUCUNE : l'épinglage à 0 interdit la croissance
        /// silencieuse (un lot non traduit passerait sinon pour un lot livré).
        /// </summary>
        private static readonly (string Dir, string Stem, string Key)[] VerbatimByDesign =
        {
        };

        /// <summary>
        /// Bloc CJK : Ext-A (U+3400–U+4DBF), unifié (U+4E00–U+9FFF) et formes de
        /// compatibilité (U+F900–U+FAFF). Les deux plages hors bloc unifié sont
        /// une marge défensive, pas une nécessité : sur les 3 018 valeurs
        /// <c>*.zh-CN.resx</c> du dépôt, les 2 997 portant de l'écriture chinoise
        /// relèvent TOUTES du bloc unifié (mesure #2098).
        /// <para>
        /// Ce que ce contrôle ajoute au « ≠ anglais » ci-dessous : une valeur peut
        /// différer de l'anglais sans porter un seul caractère chinois (espacement,
        /// ponctuation, marque latine, texte anglais reformulé) et traverser ce
        /// contrôle. Exiger au moins un caractère chinois ferme ce trou-là.
        /// </para>
        /// <para>
        /// Ce qu'il ne ferme PAS : une valeur à moitié traduite (moitié chinoise,
        /// moitié anglaise) porte déjà un caractère chinois, donc elle passe. Ce
        /// contrôle ne la voit pas et ne prétend pas la voir (limite constatée
        /// sur #2098, énoncée dès l'origine).
        /// </para>
        /// </summary>
        private static readonly Regex ChineseScript =
            new(@"[㐀-䶿一-鿿豈-﫿]", RegexOptions.Compiled);

        /// <summary>
        /// Valeurs dispensées du contrôle d'écriture. Census du lot (#2098 pour
        /// la méthode, re-mesuré sur les 27 fichiers) : les 668 valeurs livrées
        /// portent TOUTES l'écriture chinoise — la liste est donc VIDE et
        /// épinglée à 0 : la première dispense sera une décision, jamais un
        /// constat.
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
                "le lot « modules » compte 27 familles CORE de DesktopModules (PersonaBar et NBright exclus par #2072 ; "
                + "les deux Login.ascx « déjà couverts » exclus)");
            CoveredKeyTotal.Should().Be(668,
                "678 clés neutres moins les 10 déclarées non livrées — dénominateur #2072, pas un littéral");
            Covered.Select(c => (c.Dir, c.Stem)).Should().OnlyHaveUniqueItems(
                "quatre stems collident entre répertoires (Settings ×5, View ×4, SharedResources ×4, Edit ×2) : le couple identifie");
            Covered.Select(c => c.Dir).Distinct().Should().HaveCount(8,
                "le lot couvre HUIT répertoires de DesktopModules ({AuthenticationServices/DNN, CoreMessaging, DDRMenu, "
                + "HTML, Journal, MemberDirectory, RazorModules/RazorHost, SocialGroups}) — ce compte vivait en prose "
                + "seule, si bien qu'une entrée vers un 9ᵉ répertoire passait tous les tests (note de revue #2105)");
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
        /// hétérogène : dans ce lot, <c>AuthenticationServices/DNN/Settings</c>,
        /// <c>HTML/Settings</c> et <c>HTML/SharedResources</c> n'ont PAS de BOM
        /// quand les 24 autres en ont un. Attente DÉRIVÉE du neutre.
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
        /// Contrôle d'écriture : chaque valeur livrée non dispensée porte au moins
        /// un caractère chinois. Il voit les valeurs qui diffèrent de l'anglais tout
        /// en n'étant pas chinoises (espacement, ponctuation, marque latine, anglais
        /// reformulé) ; il ne voit PAS une valeur à moitié traduite, qui porte déjà
        /// un caractère chinois — voir <see cref="ChineseScript"/>.
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
        /// La liste des dispenses d'écriture est VIVANTE et ÉPINGLÉE : le census du
        /// lot (668 valeurs, 0 sans écriture chinoise) n'en justifie AUCUNE, et la
        /// première dispense devra viser un fichier du lot et une clé vivant au
        /// neutre.
        /// </summary>
        [Fact]
        public void TheScriptExemptListCannotGrowSilently()
        {
            ScriptExempt.Should().BeEmpty(
                "census du lot : les 668 valeurs zh portent toutes l'écriture chinoise — une dispense "
                + "nouvelle est une décision à consigner, jamais un constat pour faire passer un rouge");
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
