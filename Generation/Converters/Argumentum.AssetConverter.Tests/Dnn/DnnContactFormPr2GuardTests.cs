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
    /// Garde D3 · 2/2 : les 24 libellés du formulaire de contact (app 50) en
    /// es / ar / fa / zh. Mêmes 8 correctifs fr que D3 PR1, portés par la
    /// seconde branche depuis master. Les colonnes en/ru/pt restent vides
    /// sur cette branche (PR1 les porte). Après merge des deux PR, la garde
    /// de PR1 et celle-ci se rejoignent sur les 7 langues.
    /// Attentes DÉRIVÉES du fr : balises+attributs = octets du fr, compte de
    /// trous visibles, jetons de marque, paire expéditeur/destinataire.
    /// Assertions spécifiques : écriture arabe (ar/fa), ZWNJ persan, caractères
    /// chinois simplifiés, 您 (forme polie).
    /// </summary>
    public class DnnContactFormPr2GuardTests
    {
        private const string RegistryCsv = "docs/dnn-localization/eav-content.csv";
        private const string EntityGuid = "2231FDC4-3838-4F38-90B2-B29F2054B892";
        private static readonly string[] Languages = { "es", "ar", "fa", "zh" };

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
        private static readonly Regex AnyTag = new Regex("<[^>]+>");
        private static readonly Regex Arabic = new Regex("[؀-ۿ]");
        private static readonly Regex Cjk = new Regex("[一-鿿]");
        private static readonly Regex TraditionalCjk = new Regex("們這個為與於對說會來時從種樣機網頁資訊權條隱護處據務關係經過還將發應當則該須確認證單選擇購買實際");

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
        public void Fr_Is_Complete()
        {
            // La moitié « en/ru/pt restent vides sur la branche PR2 » est retirée au merge de PR1 (#2052) :
            // c'était un état de branche, pas un invariant. La complétude en/ru/pt est gardée par
            // DnnContactFormGuardTests.Every_Label_Is_Translated.
            var rows = App50Rows();
            foreach (var field in Fields)
                rows[field]["fr"].Trim().Should().NotBeNullOrEmpty($"{field} : fr rempli");
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
                loc.Should().Equal(fr, $"{field} [{lang}] : balises et attributs = les octets du fr");
                checkedTags += fr.Count;
            }
            checkedTags.Should().BeGreaterThan(20, "auto-contrôle : le fr porte des balises");
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
        public void Sender_And_Owner_Warnings_Stay_Distinct()
        {
            var rows = App50Rows();
            foreach (var lang in Languages)
                rows["MessageMailFromError"][lang].Should().NotBe(rows["MessageMailOwnerError"][lang],
                    $"[{lang}] expéditeur et destinataire restent deux textes distincts");
        }

        [Fact]
        public void Arabic_And_Persian_Use_Their_Own_Letters()
        {
            var rows = App50Rows();
            foreach (var field in Fields)
            {
                var ar = rows[field]["ar"];
                var fa = rows[field]["fa"];
                Arabic.Matches(ar).Count.Should().BeGreaterThan(0, $"{field} [ar] : écriture arabe");
                Arabic.Matches(fa).Count.Should().BeGreaterThan(0, $"{field} [fa] : écriture arabe");
                // fa : ی/ک persans, jamais ي/ك/ى arabes ; ar : ي/ك arabes, jamais ی/ک persans
                fa.Should().NotContainAny(new[] { "ي", "ك", "ى" }, $"{field} [fa] : ی/ک persans");
                ar.Should().NotContainAny(new[] { "ی", "ک" }, $"{field} [ar] : ي/ك arabes");
            }
        }

        [Fact]
        public void Chinese_Is_Simplified_And_Uses_Polite_Form()
        {
            var rows = App50Rows();
            foreach (var field in Fields)
            {
                var zh = rows[field]["zh"];
                Cjk.Matches(zh).Count.Should().BeGreaterThan(0, $"{field} [zh] : caractères chinois");
                TraditionalCjk.Matches(zh).Should().BeEmpty($"{field} [zh] : chinois simplifié, pas traditionnel");
            }
            // 您 (forme polie) apparaît dans les messages utilisateur, pas dans les libellés courts
            rows["MessageNewsletterSuccess"]["zh"].Should().Contain("您", "zh : forme polie 您 dans les messages");
        }

        [Fact]
        public void Persian_Uses_Zwnj_In_Compound_Words()
        {
            var rows = App50Rows();
            // au moins un ZWNJ dans les composés persans (می‌کند, می‌پذیرم, می‌فهمم…)
            var faAll = string.Join(" ", Fields.Select(f => rows[f]["fa"]));
            faAll.Should().Contain("‌", "fa : ZWNJ présent dans les composés verbaux");
        }
    }
}
