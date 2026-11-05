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
    /// Garde zh-CN des 30 clés que la montée DNN 10.4.1 (02/11/2026, #2123) a
    /// ajoutées au neutre <c>App_GlobalResources/GlobalResources.resx</c> :
    /// 18 rôles (<c>.Role</c> / <c>.RoleDescription</c>), 10 gabarits de workflow
    /// (<c>Start|Complete|Discard</c> × <c>Workflow|State</c> × <c>Subject|Body</c>)
    /// et 2 e-mails de rappel de mot de passe administrateur. Dernier pack du lot
    /// après fr-FR (#2133), pt-BR (#2143), ru-RU (#2147), ar-SA (#2149) et
    /// fa-IR (#2151).
    /// <para>
    /// Mesuré le 05/11/2026 à <c>66b28511</c> : le pack zh-CN portait <b>172</b>
    /// clés au parseur et <b>0</b> des 30 neuves. Comme ar-SA et fa-IR, son
    /// commentaire d'en-tête porte <b>4</b> exemples <c>&lt;data&gt;</c>
    /// (<c>Name1</c>/<c>Color1</c>/<c>Bitmap1</c>/<c>Icon1</c>) qui gonflent un
    /// <c>grep</c> à 176 — le compte se lit au parseur. Transport : <b>BOM UTF-8</b>,
    /// LF seuls, 61 612 octets. Enveloppe majoritaire :
    /// <c>xml:space="preserve"</c> sur <c>&lt;data&gt;</c> (172 cellules) contre
    /// 2 sur <c>&lt;value&gt;</c>.
    /// </para>
    /// <para>
    /// Script mesuré sur tout le pack historique : <b>8 101</b> caractères Han,
    /// ponctuation pleine-chasse （），：；！ et idéographique 、。， guillemets
    /// courbes “” tiret — points de suspension … — et <b>zéro</b> kana, <b>zéro</b>
    /// hangul. La garde bannit donc les syllabaires japonais (U+3040–U+30FF) et
    /// le hangul (U+1100–U+11FF, U+AC00–U+D7AF) : dans un pack chinois simplifié,
    /// c'est une contamination par une mauvaise langue, pas une variante
    /// d'écriture. La ponctuation CJK (U+3001 、 et U+3002 。) vit SOUS la plage
    /// bannie et passe légitimement.
    /// </para>
    /// <para>
    /// Terminologie relevée avant d'écrire, DANS ce pack : « 密码重置令牌 » et
    /// « 密码提醒 » (<c>EMAIL_PASSWORD_REMINDER_*</c>), « 等待审批 »
    /// (<c>EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_BODY</c>), « 另行发送密码重置邮件 »,
    /// « 帐户 » (18 occurrences, jamais « 账户 »), « 用户名 » (10), « 显示名称 »
    /// (corps admin), « 主机 » (<c>//Host.String</c>), « 注册用户 », « 页面 »,
    /// « 提交 », « 审核 », « 发布 », « 拒绝 », « 角色 », « 验证 », « 登录 », « 订阅 »,
    /// « 匿名 ». Formations sans précédent pack : « 所有人 », « 超级用户 »,
    /// « 未认证用户 », « 未验证用户 », « 订阅者 », « 内容编辑 », « 内容管理员 »,
    /// « 丢弃 », « 上线 ».
    /// </para>
    /// </summary>
    public class DnnGlobalResourcesZhCnGuardTests
    {
        private const string NeutralResx = "DNNPlatform/App_GlobalResources/GlobalResources.resx";
        private const string ZhCnResx = "DNNPlatform/App_GlobalResources/GlobalResources.zh-CN.resx";

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
        /// Un caractère est un kana (hiragana U+3040–U+309F, katakana
        /// U+30A0–U+30FF) ou un hangul (jamo U+1100–U+11FF, syllabes
        /// U+AC00–U+D7AF) : les deux syllabaires que le corpus zh-CN n'emploie
        /// JAMAIS (mesuré : 0 occurrence sur les 172 cellules historiques). La
        /// ponctuation CJK 、。 vit sous U+3040 et n'est pas touchée.
        /// </summary>
        private static bool IsKanaOrHangul(char c) =>
            (c >= '぀' && c <= 'ヿ')
            || (c >= 'ᄀ' && c <= 'ᇿ')
            || (c >= '가' && c <= '힯');

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
        public void TheThirtyKeysAreDeliveredInZhCn()
        {
            var zh = Values(ZhCnResx);
            var missing = NewNeutralKeys.Where(k => !zh.ContainsKey(k)).ToArray();
            missing.Should().BeEmpty(
                "les 30 clés de la montée doivent être livrées au pack zh-CN : {0}",
                string.Join(" | ", missing));
            var empty = NewNeutralKeys.Where(k => zh.ContainsKey(k) && string.IsNullOrWhiteSpace(zh[k])).ToArray();
            empty.Should().BeEmpty("aucune cellule zh-CN vide : {0}", string.Join(" | ", empty));
        }

        /// <summary>Une cellule zh-CN égale au neutre est une non-traduction (aucun
        /// homographe possible entre l'anglais et un texte chinois).</summary>
        [Fact]
        public void NoZhCnCellIsTheEnglishCopy()
        {
            var neutral = Values(NeutralResx);
            var zh = Values(ZhCnResx);
            var copies = NewNeutralKeys
                .Where(k => zh.TryGetValue(k, out var v) && v == neutral[k])
                .ToArray();
            copies.Should().BeEmpty("cellule zh-CN identique à l'anglais (non-traduction) : {0}",
                string.Join(" | ", copies));
        }

        [Fact]
        public void PlaceholderParityWithTheNeutral()
        {
            var neutral = Values(NeutralResx);
            var zh = Values(ZhCnResx);
            var offenders = new List<string>();
            foreach (var key in NewNeutralKeys)
            {
                if (!zh.TryGetValue(key, out var value)) continue;
                var expected = Tokens(neutral[key]);
                var actual = Tokens(value);
                if (!expected.SequenceEqual(actual))
                    offenders.Add($"{key} :: neutre [{string.Join(",", expected)}] vs zh-CN [{string.Join(",", actual)}]");
            }
            offenders.Should().BeEmpty("parité des jetons DNN et des index de format exigée : {0}",
                string.Join(" | ", offenders));
        }

        [Fact]
        public void ZhCnKeepsItsTransportAndDidNotLoseKeys()
        {
            var blob = DnnResxGuardSocle.CommittedBlob(ZhCnResx);
            blob.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "BOM UTF-8 conservée");
            blob.Should().NotContain((byte)'\r', "fins de ligne LF seules dans ce pack");

            // 172 clés avant le lot + 30 = 202, comptées au PARSEUR — le commentaire
            // d'en-tête porte les 4 mêmes exemples <data> que ar-SA/fa-IR, qui
            // gonflent le grep à 176.
            Values(ZhCnResx).Should().HaveCount(202,
                "172 clés historiques + les 30 de la montée : le lot AJOUTE, il ne remplace pas");
        }

        /// <summary>Garde de CONTAMINATION, pas de fidélité : hors jetons DNN, une
        /// cellule neuve est écrite en chinois et ne porte aucune lettre latine.
        /// Elle attrape une phrase anglaise laissée en place, un mot non traduit,
        /// un gabarit réduit à ses jetons ; elle ne juge pas la justesse d'une
        /// traduction.</summary>
        [Fact]
        public void NoNewCellCarriesLatinTextOutsideTokens()
        {
            var zh = Values(ZhCnResx);
            var offenders = NewNeutralKeys
                .Where(k => zh.TryGetValue(k, out var v))
                .Where(k => !IsHanOutsideTokens(zh[k]))
                .ToArray();
            offenders.Should().BeEmpty(
                "hors jetons, une cellule zh-CN neuve doit être en chinois et sans lettre latine : {0}",
                string.Join(" | ", offenders));

            // Contrôle inverse : le détecteur DOIT voir un mot anglais resté en place
            // et un gabarit vidé de son texte, et laisser passer la forme livrée.
            IsHanOutsideTokens("页面“{0}” published.")
                .Should().BeFalse("un mot anglais hors jetons doit être vu");
            IsHanOutsideTokens("[Portal:PortalName] [User:Username]")
                .Should().BeFalse("un gabarit réduit à ses jetons doit être vu");
            IsHanOutsideTokens("[Portal:PortalName] 未批准用户 [User:Username] 请求的密码提醒")
                .Should().BeTrue("la forme livrée — jetons latins compris — doit passer");
        }

        private static bool IsHanOutsideTokens(string value)
        {
            var text = Regex.Replace(value, TokenPattern, "");
            return Regex.IsMatch(text, "[一-鿿]") && !Regex.IsMatch(text, "[A-Za-z]");
        }

        /// <summary>Garde de script propre au pack : le corpus zh-CN n'emploie
        /// JAMAIS de kana ni de hangul (mesuré : 0 occurrence). Dans un pack
        /// chinois simplifié, leur apparition est une contamination par le
        /// japonais ou le coréen — une mauvaise langue collée, pas une variante
        /// d'écriture. Même famille que la garde persan-exclusif d'ar-SA (#2149)
        /// et son miroir fa-IR (#2151).</summary>
        [Fact]
        public void NoNewCellCarriesKanaOrHangul()
        {
            var zh = Values(ZhCnResx);
            var offenders = NewNeutralKeys
                .Where(k => zh.TryGetValue(k, out var v) && v.Any(IsKanaOrHangul))
                .ToArray();
            offenders.Should().BeEmpty(
                "une cellule zh-CN neuve ne porte ni kana ni hangul (contamination ja/ko) : {0}",
                string.Join(" | ", offenders));

            // Contrôles inverses : les syllabaires DOIVENT déclencher, les han du
            // pack et sa ponctuation DOIVENT passer (sinon le fait principal est
            // un vert à vide, ou un rouge à faux positif).
            "テスト".Any(IsKanaOrHangul).Should().BeTrue(
                "le katakana テ doit déclencher le détecteur");
            "あいう".Any(IsKanaOrHangul).Should().BeTrue(
                "le hiragana あ doit déclencher le détecteur");
            "한국어".Any(IsKanaOrHangul).Should().BeTrue(
                "le hangul 한 doit déclencher le détecteur");
            "测试页面已提交".Any(IsKanaOrHangul).Should().BeFalse(
                "les han du pack doivent passer");
            "、。（）“”—…".Any(IsKanaOrHangul).Should().BeFalse(
                "la ponctuation du pack (U+3001/3002, formes pleines) vit sous la plage bannie et doit passer");
        }

        /// <summary>Engagement #2143 (ai-01, c.6629942784) : le texte PARSÉ d'une
        /// cellule neuve ne contient ni « &amp;amp; » ni « &amp;quot; » (double
        /// échappement = entité visible à l'écran).</summary>
        [Fact]
        public void NoNewCellCarriesEscapedEntityText()
        {
            var zh = Values(ZhCnResx);
            var offenders = NewNeutralKeys
                .Where(k => zh.TryGetValue(k, out var v))
                .Where(k => CarriesEntityText(zh[k]))
                .ToArray();
            offenders.Should().BeEmpty(
                "le texte parsé d'une cellule neuve ne doit contenir ni \"&amp;\" ni \"&quot;\" "
                + "(double échappement : l'écran affiche l'entité au lieu du guillemet) : {0}",
                string.Join(" | ", offenders));

            // Contrôle inverse : forme doublement échappée vue, forme livrée passée.
            CarriesEntityText("密码&quot;提醒&quot;")
                .Should().BeTrue("le détecteur doit voir la forme doublement échappée");
            CarriesEntityText("“{0}”的密码提醒")
                .Should().BeFalse("le détecteur doit laisser passer la forme livrée");
        }

        private static bool CarriesEntityText(string value) =>
            value.Contains("&amp;") || value.Contains("&quot;");
    }
}
