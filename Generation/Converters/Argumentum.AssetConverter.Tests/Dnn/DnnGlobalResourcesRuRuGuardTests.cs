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
	/// Garde ru-RU des 30 clés que la montée DNN 10.4.1 (02/11/2026, #2123) a
	/// ajoutées au neutre <c>App_GlobalResources/GlobalResources.resx</c> :
	/// 18 rôles (<c>.Role</c> / <c>.RoleDescription</c>), 10 gabarits de workflow
	/// (<c>Start|Complete|Discard</c> × <c>Workflow|State</c> × <c>Subject|Body</c>)
	/// et 2 e-mails de rappel de mot de passe administrateur. Jumeau ru-RU de
	/// <see cref="DnnGlobalResourcesPtBrGuardTests"/> (#2143), même lot, autre pack.
	/// <para>
	/// Mesuré le 04/11/2026 à <c>8c74ee13</c> : le pack ru-RU portait <b>182</b> clés
	/// et <b>0</b> des 30 neuves. ⚠️ Un <c>grep -c '&lt;data '</c> y compte <b>186</b> :
	/// les 4 surnuméraires (<c>Name1</c>, <c>Color1</c>, <c>Bitmap1</c>, <c>Icon1</c>)
	/// sont les exemples du commentaire d'en-tête resx standard, que le pack pt-BR
	/// n'a pas — le compte se lit au parseur, jamais au grep. Transport propre au
	/// pack : <b>BOM UTF-8 présente</b>, LF seuls (hexdump).
	/// </para>
	/// <para>
	/// Terminologie relevée avant d'écrire, dans ce pack et les autres ressources
	/// ru-RU du dépôt : « Все пользователи », « Подписчики », « Зарегистрированные
	/// пользователи », « Суперпользователи », « Администраторы » (libellés de rôles
	/// déjà attestés) ; « учетная запись », « неподтвержденная », « одобрена »,
	/// « сброс пароля », « Имя пользователя », « Отображаемое имя » (frères
	/// <c>EMAIL_*</c>) ; « в состоянии » (workflow) ; guillemets « » (13 paires au pack,
	/// zéro guillemet droit). Formations sans précédent : « Редакторы контента »,
	/// « Менеджеры контента », « Неаутентифицированные пользователи ».
	/// </para>
	/// </summary>
	public class DnnGlobalResourcesRuRuGuardTests
	{
		private const string NeutralResx = "DNNPlatform/App_GlobalResources/GlobalResources.resx";
		private const string RuRuResx = "DNNPlatform/App_GlobalResources/GlobalResources.ru-RU.resx";

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
		public void TheThirtyKeysAreDeliveredInRuRu()
		{
			var ru = Values(RuRuResx);
			var missing = NewNeutralKeys.Where(k => !ru.ContainsKey(k)).ToArray();
			missing.Should().BeEmpty(
				"les 30 clés de la montée doivent être livrées au pack ru-RU : {0}",
				string.Join(" | ", missing));

			var empty = NewNeutralKeys.Where(k => ru.ContainsKey(k) && string.IsNullOrWhiteSpace(ru[k])).ToArray();
			empty.Should().BeEmpty("aucune cellule ru-RU vide : {0}", string.Join(" | ", empty));
		}

		/// <summary>Une cellule ru-RU égale au neutre est une non-traduction (aucun
		/// homographe possible entre l'anglais et un texte cyrillique).</summary>
		[Fact]
		public void NoRuRuCellIsTheEnglishCopy()
		{
			var neutral = Values(NeutralResx);
			var ru = Values(RuRuResx);
			var copies = NewNeutralKeys
				.Where(k => ru.TryGetValue(k, out var v) && v == neutral[k])
				.ToArray();
			copies.Should().BeEmpty("cellule ru-RU identique à l'anglais (non-traduction) : {0}",
				string.Join(" | ", copies));
		}

		[Fact]
		public void PlaceholderParityWithTheNeutral()
		{
			var neutral = Values(NeutralResx);
			var ru = Values(RuRuResx);
			var offenders = new List<string>();
			foreach (var key in NewNeutralKeys)
			{
				if (!ru.TryGetValue(key, out var value)) continue;
				var expected = Tokens(neutral[key]);
				var actual = Tokens(value);
				if (!expected.SequenceEqual(actual))
					offenders.Add($"{key} :: neutre [{string.Join(",", expected)}] vs ru-RU [{string.Join(",", actual)}]");
			}
			offenders.Should().BeEmpty("parité des jetons DNN et des index de format exigée : {0}",
				string.Join(" | ", offenders));
		}

		[Fact]
		public void RuRuKeepsItsTransportAndDidNotLoseKeys()
		{
			var blob = DnnResxGuardSocle.CommittedBlob(RuRuResx);
			blob.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "BOM UTF-8 conservée");
			blob.Should().NotContain((byte)'\r', "fins de ligne LF seules dans ce pack");

			// 182 clés avant le lot + 30 = 212, comptées au PARSEUR (le grep en voit
			// 4 de plus : les exemples du commentaire d'en-tête resx).
			Values(RuRuResx).Should().HaveCount(212,
				"182 clés historiques + les 30 de la montée : le lot AJOUTE, il ne remplace pas");
		}

		/// <summary>Garde de CONTAMINATION, pas de fidélité : hors jetons DNN, une
		/// cellule neuve est écrite en cyrillique et ne porte aucune lettre latine.
		/// Elle attrape une phrase anglaise laissée en place, un mot non traduit, un
		/// gabarit réduit à ses jetons ; elle ne juge pas la justesse d'une traduction
		/// (« хост » est un emprunt translittéré, il passe — c'est voulu).</summary>
		[Fact]
		public void NoNewCellCarriesLatinTextOutsideTokens()
		{
			var ru = Values(RuRuResx);
			var offenders = NewNeutralKeys
				.Where(k => ru.TryGetValue(k, out var v))
				.Where(k => !IsCyrillicOutsideTokens(ru[k]))
				.ToArray();
			offenders.Should().BeEmpty(
				"hors jetons, une cellule ru-RU neuve doit être cyrillique et sans lettre latine : {0}",
				string.Join(" | ", offenders));

			// Contrôle inverse : le détecteur DOIT voir un mot anglais resté en place
			// et un gabarit vidé de son texte, et laisser passer la forme livrée.
			IsCyrillicOutsideTokens("Page «{0}» опубликована и теперь доступна.")
				.Should().BeFalse("un mot anglais hors jetons doit être vu");
			IsCyrillicOutsideTokens("[Portal:PortalName] [User:Username]")
				.Should().BeFalse("un gabarit réduit à ses jetons doit être vu");
			IsCyrillicOutsideTokens("[Portal:PortalName] Запрос на восстановление пароля от неодобренного пользователя [User:Username]")
				.Should().BeTrue("la forme livrée — jetons latins compris — doit passer");
		}

		private static bool IsCyrillicOutsideTokens(string value)
		{
			var text = Regex.Replace(value, TokenPattern, "");
			return Regex.IsMatch(text, @"[Ѐ-ӿ]") && !Regex.IsMatch(text, "[A-Za-z]");
		}

		/// <summary>Relevé promis sur #2143 (ai-01, c.6629942784) : la première
		/// livraison pt-BR échappait ses guillemets deux fois — le texte parsé portait
		/// littéralement « &amp;quot;host&amp;quot; ». Aucune valeur ru-RU parsée ne
		/// doit contenir d'entité visible.</summary>
		[Fact]
		public void NoNewCellCarriesEscapedEntityText()
		{
			var ru = Values(RuRuResx);
			var offenders = NewNeutralKeys
				.Where(k => ru.TryGetValue(k, out var v))
				.Where(k => CarriesEntityText(ru[k]))
				.ToArray();
			offenders.Should().BeEmpty(
				"le texte parsé d'une cellule neuve ne doit contenir ni \"&amp;\" ni \"&quot;\" "
				+ "(double échappement : l'écran affiche l'entité au lieu du guillemet) : {0}",
				string.Join(" | ", offenders));

			// Contrôle inverse : forme doublement échappée vue, forme livrée passée.
			CarriesEntityText("имеют доступ уровня &quot;хост&quot; к сайту")
				.Should().BeTrue("le détecteur doit voir la forme doublement échappée");
			CarriesEntityText("имеют доступ уровня «хост» к сайту")
				.Should().BeFalse("le détecteur doit laisser passer la forme livrée");
		}

		private static bool CarriesEntityText(string value) =>
			value.Contains("&amp;") || value.Contains("&quot;");
	}
}
