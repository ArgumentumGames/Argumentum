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
    /// Garde fa-IR des 30 clés que la montée DNN 10.4.1 (02/11/2026, #2123) a
    /// ajoutées au neutre <c>App_GlobalResources/GlobalResources.resx</c> :
    /// 18 rôles (<c>.Role</c> / <c>.RoleDescription</c>), 10 gabarits de workflow
    /// (<c>Start|Complete|Discard</c> × <c>Workflow|State</c> × <c>Subject|Body</c>)
    /// et 2 e-mails de rappel de mot de passe administrateur. Dernier jumeau du lot
    /// après fr-FR (#2133), pt-BR (#2143), ru-RU (#2147) et ar-SA (#2149).
    /// <para>
    /// Mesuré le 05/11/2026 à <c>66b28511</c> : le pack fa-IR portait <b>172</b> clés
    /// au parseur et <b>0</b> des 30 neuves. Comme ar-SA et ru-RU, son commentaire
    /// d'en-tête porte <b>4</b> exemples <c>&lt;data&gt;</c>
    /// (<c>Name1</c>/<c>Color1</c>/<c>Bitmap1</c>/<c>Icon1</c>) qui gonflent un
    /// <c>grep</c> à 176 — le compte se lit au parseur. Transport : <b>BOM UTF-8</b>,
    /// LF seuls. Enveloppe majoritaire : <c>xml:space="preserve"</c> sur
    /// <c>&lt;data&gt;</c> (172 cellules) contre 2 sur <c>&lt;value&gt;</c>.
    /// </para>
    /// <para>
    /// Alphabet mesuré sur tout le pack historique : persan exclusif largement
    /// attesté (ی 2 542, ک 694, ZWNJ 492, گ 228, پ 158, چ 60, ژ 37) et
    /// <b>zéro</b> arabe exclusif ك/ي/ة/ى — mais <b>أ</b> (alef-hamza) y vit
    /// <b>15</b> fois : il est donc LÉGITIME et la garde ne le bannit pas. Le
    /// contrôle porte sur les quatre lettres que ce pack n'emploie jamais, pas sur
    /// une pureté théorique de l'alphabet.
    /// </para>
    /// <para>
    /// Terminologie relevée avant d'écrire, DANS ce pack : « نشانه بازنشانی
    /// گذرواژه » (jeton de réinitialisation), « در انتظار تأیید », « حساب کاربری »,
    /// « نام کاربری », « نام نمایشی », « نقش », « میزبان » (<c>//Host.String</c>),
    /// « مدیر », « ثبت‌نام », « رایانامه », « دسترسی », « محتوا ». Formations sans
    /// précédent pack : « مدیران محتوا », « ویرایشگران محتوا », « مشترکان »,
    /// « کاربران ممتاز », « کاربران واردنشده ».
    /// </para>
    /// </summary>
    public class DnnGlobalResourcesFaIrGuardTests
    {
        private const string NeutralResx = "DNNPlatform/App_GlobalResources/GlobalResources.resx";
        private const string FaIrResx = "DNNPlatform/App_GlobalResources/GlobalResources.fa-IR.resx";

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
        /// Lettres arabes-exclusives que le pack fa-IR n'emploie JAMAIS (mesuré : 0
        /// occurrence chacune sur les 172 cellules historiques) : kaf et yeh arabes,
        /// teh marbuta, alef maksura. L'alef-hamza أ (U+0623) n'y est PAS — il vit
        /// 15 fois dans le pack, il est légitime.
        /// </summary>
        private static readonly char[] ArabicExclusive =
            { 'ك', 'ي', 'ة', 'ى' };

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
        public void TheThirtyKeysAreDeliveredInFaIr()
        {
            var fa = Values(FaIrResx);
            var missing = NewNeutralKeys.Where(k => !fa.ContainsKey(k)).ToArray();
            missing.Should().BeEmpty(
                "les 30 clés de la montée doivent être livrées au pack fa-IR : {0}",
                string.Join(" | ", missing));

            var empty = NewNeutralKeys.Where(k => fa.ContainsKey(k) && string.IsNullOrWhiteSpace(fa[k])).ToArray();
            empty.Should().BeEmpty("aucune cellule fa-IR vide : {0}", string.Join(" | ", empty));
        }

        /// <summary>Une cellule fa-IR égale au neutre est une non-traduction (aucun
        /// homographe possible entre l'anglais et un texte persan).</summary>
        [Fact]
        public void NoFaIrCellIsTheEnglishCopy()
        {
            var neutral = Values(NeutralResx);
            var fa = Values(FaIrResx);
            var copies = NewNeutralKeys
                .Where(k => fa.TryGetValue(k, out var v) && v == neutral[k])
                .ToArray();
            copies.Should().BeEmpty("cellule fa-IR identique à l'anglais (non-traduction) : {0}",
                string.Join(" | ", copies));
        }

        [Fact]
        public void PlaceholderParityWithTheNeutral()
        {
            var neutral = Values(NeutralResx);
            var fa = Values(FaIrResx);
            var offenders = new List<string>();
            foreach (var key in NewNeutralKeys)
            {
                if (!fa.TryGetValue(key, out var value)) continue;
                var expected = Tokens(neutral[key]);
                var actual = Tokens(value);
                if (!expected.SequenceEqual(actual))
                    offenders.Add($"{key} :: neutre [{string.Join(",", expected)}] vs fa-IR [{string.Join(",", actual)}]");
            }
            offenders.Should().BeEmpty("parité des jetons DNN et des index de format exigée : {0}",
                string.Join(" | ", offenders));
        }

        [Fact]
        public void FaIrKeepsItsTransportAndDidNotLoseKeys()
        {
            var blob = DnnResxGuardSocle.CommittedBlob(FaIrResx);
            blob.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "BOM UTF-8 conservée");
            blob.Should().NotContain((byte)'\r', "fins de ligne LF seules dans ce pack");

            // 172 clés avant le lot + 30 = 202, comptées au PARSEUR — le commentaire
            // d'en-tête porte les 4 mêmes exemples <data> que ar-SA/ru-RU, qui
            // gonflent le grep à 176.
            Values(FaIrResx).Should().HaveCount(202,
                "172 clés historiques + les 30 de la montée : le lot AJOUTE, il ne remplace pas");
        }

        /// <summary>Garde de CONTAMINATION, pas de fidélité : hors jetons DNN, une
        /// cellule neuve est écrite en persan et ne porte aucune lettre latine.
        /// Elle attrape une phrase anglaise laissée en place, un mot non traduit,
        /// un gabarit réduit à ses jetons ; elle ne juge pas la justesse d'une
        /// traduction.</summary>
        [Fact]
        public void NoNewCellCarriesLatinTextOutsideTokens()
        {
            var fa = Values(FaIrResx);
            var offenders = NewNeutralKeys
                .Where(k => fa.TryGetValue(k, out var v))
                .Where(k => !IsPersianOutsideTokens(fa[k]))
                .ToArray();
            offenders.Should().BeEmpty(
                "hors jetons, une cellule fa-IR neuve doit être en persan et sans lettre latine : {0}",
                string.Join(" | ", offenders));

            // Contrôle inverse : le détecteur DOIT voir un mot anglais resté en place
            // et un gabarit vidé de son texte, et laisser passer la forme livrée.
            IsPersianOutsideTokens("صفحه «{0}» published.")
                .Should().BeFalse("un mot anglais hors jetons doit être vu");
            IsPersianOutsideTokens("[Portal:PortalName] [User:Username]")
                .Should().BeFalse("un gabarit réduit à ses jetons doit être vu");
            IsPersianOutsideTokens("[Portal:PortalName] یادآور گذرواژه برای کاربر تأییدنشده [User:Username]")
                .Should().BeTrue("la forme livrée — jetons latins compris — doit passer");
        }

        private static bool IsPersianOutsideTokens(string value)
        {
            var text = Regex.Replace(value, TokenPattern, "");
            return Regex.IsMatch(text, "[؀-ۿ]") && !Regex.IsMatch(text, "[A-Za-z]");
        }

        /// <summary>Miroir inversé de la garde ar-SA (#2149) : ce pack est persan,
        /// les quatre lettres arabes-exclusives que son corpus historique n'emploie
        /// JAMAIS sont une contamination du pack arabe, pas une variante d'écriture.
        /// L'alef-hamza أ (U+0623) est attesté 15 fois au pack : il n'est PAS banni.</summary>
        [Fact]
        public void NoNewCellCarriesArabicExclusiveLetters()
        {
            var fa = Values(FaIrResx);
            var offenders = NewNeutralKeys
                .Where(k => fa.TryGetValue(k, out var v))
                .Where(k => fa[k].IndexOfAny(ArabicExclusive) >= 0)
                .ToArray();
            offenders.Should().BeEmpty(
                "une cellule fa-IR neuve ne porte ni ك ni ي ni ة ni ى (formes arabes exclusives) : {0}",
                string.Join(" | ", offenders));

            // Contrôles inverses : la forme arabe DOIT déclencher, la forme persane
            // et l'alef-hamza légitime DOIVENT passer (sinon le fait principal est un
            // vert à vide, ou un rouge à faux positif).
            "كتاب".IndexOfAny(ArabicExclusive).Should().BeGreaterThanOrEqualTo(0,
                "کاف arabe ك doit déclencher le détecteur");
            "کتاب".IndexOfAny(ArabicExclusive).Should().BeLessThan(0,
                "کاف persan ک est la forme du pack et doit passer");
            "کتاب‌خانهٔ أ".IndexOfAny(ArabicExclusive).Should().BeLessThan(0,
                "l'alef-hamza أ, attesté 15 fois au pack, ne doit pas être banni");
        }

        /// <summary>Engagement #2143 (ai-01, c.6629942784) : le texte PARSÉ d'une
        /// cellule neuve ne contient ni « &amp;amp; » ni « &amp;quot; » (double
        /// échappement = entité visible à l'écran).</summary>
        [Fact]
        public void NoNewCellCarriesEscapedEntityText()
        {
            var fa = Values(FaIrResx);
            var offenders = NewNeutralKeys
                .Where(k => fa.TryGetValue(k, out var v))
                .Where(k => CarriesEntityText(fa[k]))
                .ToArray();
            offenders.Should().BeEmpty(
                "le texte parsé d'une cellule neuve ne doit contenir ni \"&amp;\" ni \"&quot;\" "
                + "(double échappement : l'écran affiche l'entité au lieu du guillemet) : {0}",
                string.Join(" | ", offenders));

            // Contrôle inverse : forme doublement échappée vue, forme livrée passée.
            CarriesEntityText("دسترسی &quot;میزبان&quot; به سایت")
                .Should().BeTrue("le détecteur doit voir la forme doublement échappée");
            CarriesEntityText("دسترسی «میزبان» به سایت")
                .Should().BeFalse("le détecteur doit laisser passer la forme livrée");
        }

        private static bool CarriesEntityText(string value) =>
            value.Contains("&amp;") || value.Contains("&quot;");
    }
}
