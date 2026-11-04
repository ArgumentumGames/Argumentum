using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Garde ar-SA des 30 clés que la montée DNN 10.4.1 (02/11/2026, #2123) a
    /// ajoutées au neutre <c>App_GlobalResources/GlobalResources.resx</c> :
    /// 18 rôles (<c>.Role</c> / <c>.RoleDescription</c>), 10 gabarits de workflow
    /// (<c>Start|Complete|Discard</c> × <c>Workflow|State</c> × <c>Subject|Body</c>)
    /// et 2 e-mails de rappel de mot de passe administrateur. Jumeau ar-SA de
    /// <see cref="DnnGlobalResourcesPtBrGuardTests"/> (#2143) et de
    /// <see cref="DnnGlobalResourcesRuRuGuardTests"/> (#2147), même lot, autre pack.
    /// <para>
    /// Mesuré le 04/11/2026 à <c>8c74ee13</c> : le pack ar-SA portait <b>172</b> clés
    /// et <b>0</b> des 30 neuves. Comme le pack ru-RU, son commentaire d'en-tête
    /// porte <b>4</b> exemples <c>&lt;data&gt;</c> (<c>Name1</c>/<c>Color1</c>/
    /// <c>Bitmap1</c>/<c>Icon1</c>, lignes 20-25) qui gonflent un <c>grep</c> à
    /// 176 — le compte se lit au parseur, l'instrument direct. Transport propre
    /// au pack : <b>BOM UTF-8 présente</b>, LF seuls (hexdump), <b>0</b> lettre
    /// persane-exclusive sur l'ensemble du pack historique.
    /// </para>
    /// <para>
    /// Terminologie relevée avant d'écrire, DANS ce pack : « رمز إعادة تعيين كلمة
    /// المرور », « اسم المستخدم », « الاسم المعروض », « في انتظار الموافقة »,
    /// « تمت الموافقة », « مستخدم مسجَّل », « التسجيل الموثّق », « دور », « مسؤول »,
    /// « تسجيل الدخول », « الموقع الإلكتروني » (frères <c>EMAIL_*</c>) ; guillemets
    /// « » en prose (9 paires au pack historique). Formations sans précédent dans ce
    /// pack : « محررو المحتوى », « مديرو المحتوى », « المشتركون »,
    /// « المستخدمون المتميزون », « المستخدمون غير المصادَقين ».
    /// </para>
    /// </summary>
    public class DnnGlobalResourcesArSaGuardTests
    {
        private const string NeutralResx = "DNNPlatform/App_GlobalResources/GlobalResources.resx";
        private const string ArSaResx = "DNNPlatform/App_GlobalResources/GlobalResources.ar-SA.resx";

        /// <summary>Clés ajoutées au neutre par la montée, mesurées le 03/11/2026.</summary>
        private static readonly string[] NewNeutralKeys =
        {
                "Administrators.Role", "Administrators.RoleDescription",
                "All Users.Role", "All Users.RoleDescription",
                "CompleteStateBody.Text", "CompleteStateSubject.Text",
                "CompleteWorkflowBody.Text", "CompleteWorkflowSubject.Text",
                "Content Editors.Role", "Content Editors.RoleDescription",
                "Content Managers.Role", "Content Managers.RoleDescription",
                "DiscardStateBody.Text", "DiscardStateSubject.Text",
                "DiscardWorkflowBody.Text", "DiscardWorkflowSubject.Text",
                "EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_ADMINISTRATOR_BODY.Text",
                "EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_ADMINISTRATOR_SUBJECT.Text",
                "Registered Users.Role", "Registered Users.RoleDescription",
                "StartWorkflowBody.Text", "StartWorkflowSubject.Text",
                "Subscribers.Role", "Subscribers.RoleDescription",
                "Superuser.Role", "Superuser.RoleDescription",
                "Unauthenticated Users.Role", "Unauthenticated Users.RoleDescription",
                "Unverified Users.Role", "Unverified Users.RoleDescription",
        };

        private const string TokenPattern = @"\{\d+\}|\[[A-Za-z]+:\s*[A-Za-z]+\]";

        /// <summary>
        /// Lettres exclusives du persan (+ ZWNJ, séparateur morphologique persan) :
        /// le miroir exact de la garde fa-IR qui interdit les lettres arabes
        /// exclusives. Un pack ar-SA n'a aucune raison d'en porter une.
        /// </summary>
        private static readonly char[] PersianExclusive =
            { 'پ', 'چ', 'ژ', 'گ', 'ی', 'ک', '‌' };

        private static Dictionary<string, string> Values(string rel)
        {
            var blob = DnnResxGuardSocle.CommittedBlob(rel);
            var doc = XDocument.Parse(Decode(blob));
            return doc.Root!.Elements("data")
                .ToDictionary(d => d.Attribute("name")!.Value, d => d.Element("value")?.Value ?? "");
        }

        /// <summary>Décode le blob en XML, BOM RETIRÉ (`XDocument.Parse` lève en
        /// position 1 sur un U+FEFF initial — même piège que la garde pt-BR).</summary>
        private static string Decode(byte[] blob) => Encoding.UTF8.GetString(blob).TrimStart('﻿');

        private static string[] Tokens(string value) =>
            Regex.Matches(value, TokenPattern)
                .Select(m => Regex.Replace(m.Value, @"\s+", ""))
                .OrderBy(x => x, StringComparer.Ordinal)
                .ToArray();

        /// <summary>Anti-vacuité : les 30 clés existent VRAIMENT au neutre.</summary>
        [Fact]
        public void TheThirtyKeysExistInTheNeutral()
        {
            var neutral = Values(NeutralResx);
            var missing = NewNeutralKeys.Where(k => !neutral.ContainsKey(k)).ToArray();
            missing.Should().BeEmpty(
                "les 30 clés doivent exister au neutre — une clé disparue du neutre invalide le lot : {0}",
                string.Join(" | ", missing));
            NewNeutralKeys.Should().HaveCount(30).And.OnlyHaveUniqueItems();
        }

        [Fact]
        public void TheThirtyKeysAreDeliveredInArSa()
        {
            var ar = Values(ArSaResx);
            var missing = NewNeutralKeys.Where(k => !ar.ContainsKey(k)).ToArray();
            missing.Should().BeEmpty(
                "les 30 clés de la montée doivent être livrées au pack ar-SA : {0}",
                string.Join(" | ", missing));

            var empty = NewNeutralKeys.Where(k => ar.ContainsKey(k) && string.IsNullOrWhiteSpace(ar[k])).ToArray();
            empty.Should().BeEmpty("aucune cellule ar-SA vide : {0}", string.Join(" | ", empty));
        }

        /// <summary>Une cellule ar-SA égale au neutre est une non-traduction (aucun
        /// homographe possible entre l'anglais et un texte arabe).</summary>
        [Fact]
        public void NoArSaCellIsTheEnglishCopy()
        {
            var neutral = Values(NeutralResx);
            var ar = Values(ArSaResx);
            var copies = NewNeutralKeys
                .Where(k => ar.TryGetValue(k, out var v) && v == neutral[k])
                .ToArray();
            copies.Should().BeEmpty("cellule ar-SA identique à l'anglais (non-traduction) : {0}",
                string.Join(" | ", copies));
        }

        [Fact]
        public void PlaceholderParityWithTheNeutral()
        {
            var neutral = Values(NeutralResx);
            var ar = Values(ArSaResx);
            var offenders = new List<string>();
            foreach (var key in NewNeutralKeys)
            {
                if (!ar.TryGetValue(key, out var value)) continue;
                var expected = Tokens(neutral[key]);
                var actual = Tokens(value);
                if (!expected.SequenceEqual(actual))
                    offenders.Add($"{key} :: neutre [{string.Join(",", expected)}] vs ar-SA [{string.Join(",", actual)}]");
            }
            offenders.Should().BeEmpty("parité des jetons DNN et des index de format exigée : {0}",
                string.Join(" | ", offenders));
        }

        [Fact]
        public void ArSaKeepsItsTransportAndDidNotLoseKeys()
        {
            var blob = DnnResxGuardSocle.CommittedBlob(ArSaResx);
            blob.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "BOM UTF-8 conservée");
            blob.Should().NotContain((byte)'\r', "fins de ligne LF seules dans ce pack");

            // 172 clés avant le lot + 30 = 202, comptées au PARSEUR — le commentaire
            // d'en-tête porte les 4 mêmes exemples <data> que côté ru-RU, qui gonflent
            // le grep à 176 (cf. DnnGlobalResourcesRuRuGuardTests).
            Values(ArSaResx).Should().HaveCount(202,
                "172 clés historiques + les 30 de la montée : le lot AJOUTE, il ne remplace pas");
        }

        /// <summary>Garde de CONTAMINATION, pas de fidélité : hors jetons DNN, une
        /// cellule neuve est écrite en arabe et ne porte aucune lettre latine.
        /// Elle attrape une phrase anglaise laissée en place, un mot non traduit,
        /// un gabarit réduit à ses jetons ; elle ne juge pas la justesse d'une
        /// traduction.</summary>
        [Fact]
        public void NoNewCellCarriesLatinTextOutsideTokens()
        {
            var ar = Values(ArSaResx);
            var offenders = NewNeutralKeys
                .Where(k => ar.TryGetValue(k, out var v))
                .Where(k => !IsArabicOutsideTokens(ar[k]))
                .ToArray();
            offenders.Should().BeEmpty(
                "hors jetons, une cellule ar-SA neuve doit être en arabe et sans lettre latine : {0}",
                string.Join(" | ", offenders));

            // Contrôle inverse : le détecteur DOIT voir un mot anglais resté en place
            // et un gabarit vidé de son texte, et laisser passer la forme livrée.
            IsArabicOutsideTokens("Page «{0}» تم نشرها.")
                .Should().BeFalse("un mot anglais hors jetons doit être vu");
            IsArabicOutsideTokens("[Portal:PortalName] [User:Username]")
                .Should().BeFalse("un gabarit réduit à ses jetons doit être vu");
            IsArabicOutsideTokens("[Portal:PortalName] طلب مستخدم لم تتم الموافقة عليه إعادة تعيين كلمة المرور [User:Username]")
                .Should().BeTrue("la forme livrée — jetons latins compris — doit passer");
        }

        private static bool IsArabicOutsideTokens(string value)
        {
            var text = Regex.Replace(value, TokenPattern, "");
            return Regex.IsMatch(text, "[؀-ۿ]") && !Regex.IsMatch(text, "[A-Za-z]");
        }

        /// <summary>Miroir de la garde fa-IR (#2141) qui interdit les lettres
        /// arabes exclusives au persan : une lettre persane ou un ZWNJ dans une
        /// cellule ar-SA neuve est une contamination de l'autre pack, pas une
        /// variante d'écriture légitime.</summary>
        [Fact]
        public void NoNewCellCarriesPersianExclusiveLetters()
        {
            var ar = Values(ArSaResx);
            var offenders = NewNeutralKeys
                .Where(k => ar.TryGetValue(k, out var v))
                .Where(k => ar[k].IndexOfAny(PersianExclusive) >= 0)
                .ToArray();
            offenders.Should().BeEmpty(
                "une cellule ar-SA neuve ne porte ni پ چ ژ گ ni ی/ک persanes ni ZWNJ : {0}",
                string.Join(" | ", offenders));

            // Contrôle inverse : le mot persan témoin du lot fa-IR DOIT être vu.
            "گفت‌گو".IndexOfAny(PersianExclusive).Should().BeGreaterThanOrEqualTo(0,
                "le témoin «گفت‌وگو» (گ + ZWNJ) doit déclencher le détecteur — sinon le fait "
                + "principal est un vert à vide");
        }

        /// <summary>Relevé promis sur #2143 (ai-01, c.6629942784) : la première
        /// livraison pt-BR échappait ses guillemets deux fois — le texte parsé portait
        /// littéralement « &amp;quot;host&amp;quot; ». Aucune valeur ar-SA parsée ne
        /// doit contenir d'entité visible.</summary>
        [Fact]
        public void NoNewCellCarriesEscapedEntityText()
        {
            var ar = Values(ArSaResx);
            var offenders = NewNeutralKeys
                .Where(k => ar.TryGetValue(k, out var v))
                .Where(k => CarriesEntityText(ar[k]))
                .ToArray();
            offenders.Should().BeEmpty(
                "le texte parsé d'une cellule neuve ne doit contenir ni \"&amp;\" ni \"&quot;\" "
                + "(double échappement : l'écran affiche l'entité au lieu du guillemet) : {0}",
                string.Join(" | ", offenders));

            // Contrôle inverse : forme doublement échappée vue, forme livrée passée.
            CarriesEntityText("لديهم صلاحيات &quot;المضيف&quot; على الموقع")
                .Should().BeTrue("le détecteur doit voir la forme doublement échappée");
            CarriesEntityText("لديهم صلاحيات «المضيف» على الموقع")
                .Should().BeFalse("le détecteur doit laisser passer la forme livrée");
        }

        private static bool CarriesEntityText(string value) =>
            value.Contains("&amp;") || value.Contains("&quot;");
    }
}
