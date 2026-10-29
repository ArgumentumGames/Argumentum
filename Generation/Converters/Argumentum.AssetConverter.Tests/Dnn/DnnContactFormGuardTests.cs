using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Garde des libellés du formulaire de contact (pool #458 D3, app 50,
    /// entité App-Resources 2231FDC4) : 24 libellés dont 8 portaient encore
    /// les défauts anglais du gabarit 2sxc dans la colonne fr — traduits en
    /// français d'abord (« d'abord en fr » du dispatch), puis traduits.
    /// Ce PR (D3 · 1/2) couvre fr + en/ru/pt ; le second porte es/ar/fa/zh
    /// et étend <see cref="Languages"/>.
    /// Toutes les attentes sont DÉRIVÉES du fr au moment du test : séquence
    /// de balises (attributs compris — les URL azing.org et /terms sont les
    /// octets du fr), compte de trous de texte visible, sondes de marques
    /// (reCAPTCHA, Mailchimp, Mobius, RecaptchaApiKey). La paire
    /// expéditeur/destinataire doit rester distincte dans chaque langue.
    /// </summary>
    public class DnnContactFormGuardTests
    {
        private const string RegistryCsv = "docs/dnn-localization/eav-content.csv";
        private const string EntityGuid = "2231FDC4-3838-4F38-90B2-B29F2054B892";

        private static readonly string[] Languages = { "en", "ru", "pt" };

        // l'identité du grain : les 24 champs de l'app 50 (structurels, pas un golden de contenu)
        private static readonly string[] Fields =
        {
            "LabelSubject", "LabelMessage", "LabelSenderName", "LabelSenderMail", "LabelRequired",
            "LabelValidEmail", "LabelValidFile",
            "MessageRecaptchaMissing", "MessageRecaptchaWarning", "MessageContainsRecaptcha",
            "MessageDefaultMailChimpKey", "MessageEnableMailchimp", "MessageFormIncomplete",
            "MessageSending", "MessageSendOk", "MessageCustomerMailSend", "MessageSendError",
            "MessageNewsletterSuccess", "MessageNewsletterUnexpectedError",
            "MessageMailFromError", "MessageMailOwnerError",
            "ButtonSend", "TermsLabel", "GdprLabel",
        };

        private static readonly Regex Opaque = new Regex("<!--.*?-->|<style\\b[^>]*>.*?</style>|<script\\b[^>]*>.*?</script>|<[^>]+>",
            RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>mots anglais qui ne peuvent pas vivre dans du français (le gabarit 2sxc).</summary>
        private static readonly Regex EnglishResidue = new Regex(
            @"(?<![A-Za-zÀ-ÿ])(please|warning|using|was|specified|enter|sending|try|again|fill|fields|before|while|contains|form|settings|address|error|unexpected|still|enable|sender|owner|preset|occurred|occured|key|demo|live|required|subject|send|valid)(?![A-Za-zÀ-ÿ])",
            RegexOptions.IgnoreCase);

        /// <summary>résidu français pour les langues latines voisines (es/pt/en) : mots français sans cousin mot-entier.</summary>
        private static readonly Regex FrenchResidueLatin = new Regex(
            @"(?<![A-Za-zÀ-ÿ])(vous|nous|aux|du|les|des|une|est|et|envoi|champs|requis|patientez|erreur|adresse)(?![A-Za-zÀ-ÿ])",
            RegexOptions.IgnoreCase);

        /// <summary>résidu français pour le russe : tout mot-outil français (l'alphabet suffit ailleurs).</summary>
        private static readonly Regex FrenchFunctionWord = new Regex(
            @"(?<![A-Za-zÀ-ÿ])(le|la|les|de|des|du|et|ou|un|une|vous|nous|est|dans|pour|par|sur|au|aux|ce|ces|cette|qui|que|site)(?![A-Za-zÀ-ÿ])",
            RegexOptions.IgnoreCase);

        private static readonly Regex Cyrillic = new Regex("[А-яёЁ]");
        private static readonly Regex AnyTag = new Regex("<[^>]+>");

        private static Dictionary<string, Dictionary<string, string>> App50Rows()
        {
            var path = Path.Combine(TestRepoRoot.Find(), RegistryCsv.Replace('/', Path.DirectorySeparatorChar));
            File.Exists(path).Should().BeTrue($"le fichier doit exister dans le checkout ({RegistryCsv})");
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { MissingFieldFound = null, HeaderValidated = null };
            var rows = new List<Dictionary<string, string>>();
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Read();
                csv.ReadHeader();
                var header = csv.HeaderRecord ?? Array.Empty<string>();
                while (csv.Read())
                {
                    if (csv.GetField(1) != "50") continue;
                    var row = new Dictionary<string, string>(StringComparer.Ordinal);
                    for (var i = 0; i < header.Length; i++) row[header[i]] = csv.GetField(i) ?? string.Empty;
                    rows.Add(row);
                }
            }
            return rows.ToDictionary(r => r["field"], r => r, StringComparer.Ordinal);
        }

        /// <summary>texte visible : runs entre segments opaques, trimés (le squelette porte les espaces de bord).</summary>
        private static List<string> VisibleTexts(string value)
        {
            var texts = new List<string>();
            var pos = 0;
            foreach (Match m in Opaque.Matches(value))
            {
                var run = value.Substring(pos, m.Index - pos).Trim();
                if (run.Length > 0) texts.Add(run);
                pos = m.Index + m.Length;
            }
            var tail = value.Substring(pos).Trim();
            if (tail.Length > 0) texts.Add(tail);
            return texts;
        }

        public static IEnumerable<object[]> LanguageCases() => Languages.Select(l => new object[] { l });

        [Fact]
        public void App50_Carries_Exactly_The_24_Contact_Fields()
        {
            var rows = App50Rows();
            rows.Keys.Should().BeEquivalentTo(Fields, "les 24 libellés de contact, ni plus ni moins");
        }

        [Fact]
        public void Fr_Is_Complete_And_Carries_No_English_Template_Default()
        {
            var rows = App50Rows();
            foreach (var field in Fields)
                rows[field]["fr"].Trim().Should().NotBeNullOrEmpty($"{field} : fr rempli");

            var englishSeen = 0;
            foreach (var field in Fields)
            {
                EnglishResidue.Matches(rows[field]["fr"]).Should().BeEmpty(
                    $"{field} : plus aucun défaut anglais du gabarit dans le fr (« {rows[field]["fr"].Trim()[..Math.Min(60, rows[field]["fr"].Trim().Length)]}… »)");
                // l'instrument doit voir l'anglais là où il vit légitimement (la colonne en)
                englishSeen += EnglishResidue.Matches(rows[field]["en"]).Count;
            }
            englishSeen.Should().BeGreaterThan(0,
                "auto-contrôle : le motif voit l'anglais du gabarit dans la colonne en (sinon le vert ci-dessus serait vacu)");
        }

        [Theory]
        [MemberData(nameof(LanguageCases))]
        public void Every_Label_Is_Translated(string lang)
        {
            var rows = App50Rows();
            foreach (var field in Fields)
                rows[field][lang].Trim().Should().NotBeNullOrEmpty($"{field} [{lang}] : traduit");
        }

        [Theory]
        [MemberData(nameof(LanguageCases))]
        public void Tags_And_Their_Attributes_Are_The_Fr_Bytes(string lang)
        {
            var rows = App50Rows();
            var checkedTags = 0;
            foreach (var field in Fields)
            {
                var fr = AnyTag.Matches(rows[field]["fr"]).Select(m => m.Value).ToList();
                var loc = AnyTag.Matches(rows[field][lang]).Select(m => m.Value).ToList();
                loc.Should().Equal(fr, $"{field} [{lang}] : balises et attributs (href azing.org, /terms, /privacy-policy) = les octets du fr");
                checkedTags += fr.Count;
            }
            checkedTags.Should().BeGreaterThan(20, "auto-contrôle : le fr porte des balises (5 rangées d'avertissement + 2 à lien)");
        }

        [Theory]
        [MemberData(nameof(LanguageCases))]
        public void Every_Visible_Slot_Of_The_Fr_Has_Text(string lang)
        {
            var rows = App50Rows();
            foreach (var field in Fields)
            {
                var fr = VisibleTexts(rows[field]["fr"]);
                var loc = VisibleTexts(rows[field][lang]);
                loc.Should().HaveCount(fr.Count, $"{field} [{lang}] : un texte visible par texte visible du fr");
                foreach (var text in loc)
                    text.Trim().Should().NotBeNullOrEmpty($"{field} [{lang}] : aucun trou vide");
            }
        }

        [Theory]
        [MemberData(nameof(LanguageCases))]
        public void Brand_Tokens_Are_Copied_As_Many_Times_As_In_Fr(string lang)
        {
            var rows = App50Rows();
            var probes = new[] { "reCAPTCHA", "Mailchimp", "Mobius", "RecaptchaApiKey" };
            var seen = 0;
            foreach (var field in Fields)
                foreach (var probe in probes)
                {
                    var frCount = Regex.Matches(rows[field]["fr"], Regex.Escape(probe), RegexOptions.IgnoreCase).Count;
                    if (frCount == 0) continue;
                    seen += frCount;
                    Regex.Matches(rows[field][lang], Regex.Escape(probe), RegexOptions.IgnoreCase).Count
                        .Should().Be(frCount, $"{field} [{lang}] : « {probe} » recopié autant de fois que dans le fr");
                }
            seen.Should().BeGreaterThan(5, "auto-contrôle : les sondes voient les marques dans le fr");
        }

        [Fact]
        public void Ru_Is_Cyrillic_Without_French_Residue()
        {
            var rows = App50Rows();
            rows["MessageSendError"]["fr"].Should().Contain("erreur", "auto-contrôle : le résidu français vit dans le fr");
            foreach (var field in Fields)
            {
                var ru = rows[field]["ru"];
                Cyrillic.Matches(ru).Count.Should().BeGreaterThan(0, $"{field} [ru] : texte en cyrillique");
                FrenchFunctionWord.Matches(ru).Should().BeEmpty($"{field} [ru] : aucun mot-outil français");
            }
        }

        [Fact]
        public void Latin_Languages_Carry_No_French_Function_Word()
        {
            var rows = App50Rows();
            FrenchResidueLatin.Matches(rows["LabelValidEmail"]["fr"]).Should().NotBeEmpty(
                "auto-contrôle : la sonde voit le français (Adresse… requise)");
            foreach (var lang in Languages.Where(l => l != "ru"))
                foreach (var field in Fields)
                    FrenchResidueLatin.Matches(rows[field][lang]).Should().BeEmpty(
                        $"{field} [{lang}] : aucun mot-outil français");
        }

        [Fact]
        public void Sender_And_Owner_Warnings_Stay_Distinct()
        {
            var rows = App50Rows();
            rows["MessageMailFromError"]["fr"].Should().NotBe(rows["MessageMailOwnerError"]["fr"],
                "auto-contrôle : la paire est distincte dans le fr");
            foreach (var lang in Languages)
                rows["MessageMailFromError"][lang].Should().NotBe(rows["MessageMailOwnerError"][lang],
                    $"[{lang}] expéditeur et destinataire restent deux textes distincts");
        }
    }
}
