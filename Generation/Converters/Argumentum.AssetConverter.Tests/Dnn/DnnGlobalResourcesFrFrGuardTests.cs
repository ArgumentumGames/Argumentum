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
	/// Garde fr-FR des 30 clés que la montée DNN 10.4.1 (02/11/2026, #2123) a
	/// ajoutées au neutre <c>App_GlobalResources/GlobalResources.resx</c> :
	/// 18 rôles (<c>.Role</c> / <c>.RoleDescription</c>), 10 gabarits de workflow
	/// (<c>Start|Complete|Discard</c> × <c>Workflow|State</c> × <c>Subject|Body</c>)
	/// et 2 e-mails de rappel de mot de passe administrateur.
	/// <para>
	/// Mesuré le 03/11/2026 à <c>d8c595ac</c> : <c>es-ES</c> portait déjà les 30
	/// (paquet espagnol officiel aligné par la montée, traductions réelles vérifiées),
	/// les six autres packs en portaient 0 — dont <c>fr-FR</c>, langue par défaut du
	/// site. Sonde anonyme de la préprod (accueil FR, Login, Register, Terms) :
	/// aucune des 30 chaînes anglaises n'apparaît sur la surface publique ; elles
	/// vivent dans les contextes authentifiés (profil, gestionnaire de rôles) et
	/// les e-mails de workflow. La langue par défaut reste la première cible.
	/// </para>
	/// <para>
	/// Terminologie reprise du pack, jamais inventée : « Super-utilisateurs »
	/// (<c>//Host//SuperuserAccounts.String</c>), « enregistrement » → « Utilisateurs
	/// enregistrés », « jeton de réinitialisation du mot de passe » et « compte en
	/// attente d'approbation » (frères <c>EMAIL_PASSWORD_REMINDER_*</c>).
	/// </para>
	/// </summary>
	public class DnnGlobalResourcesFrFrGuardTests
	{
		private const string NeutralResx = "DNNPlatform/App_GlobalResources/GlobalResources.resx";
		private const string FrFrResx = "DNNPlatform/App_GlobalResources/GlobalResources.fr-FR.resx";

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

		private static Dictionary<string, string> Values(string rel)
		{
			var blob = DnnResxGuardSocle.CommittedBlob(rel);
			var doc = XDocument.Parse(Decode(blob));
			return doc.Root!.Elements("data")
				.ToDictionary(d => d.Attribute("name")!.Value, d => d.Element("value")?.Value ?? "");
		}

		/// <summary>Décode le blob en XML. Le BOM est RETIRÉ : `XDocument.Parse`
		/// sur une chaîne dont le premier caractère est U+FEFF lève
		/// « Data at the root level is invalid. Line 1, position 1 » — mesuré
		/// le 03/11 (les gardes frères passent par `XDocument.Load(fichier)`,
		/// qui détecte le BOM lui-même ; ici on lit des octets de blob).</summary>
		private static string Decode(byte[] blob) => Encoding.UTF8.GetString(blob).TrimStart('﻿');

		private static string[] Tokens(string value) =>
			Regex.Matches(value, @"\{\d+\}|\[[A-Za-z]+:\s*[A-Za-z]+\]")
				.Select(m => Regex.Replace(m.Value, @"\s+", ""))
				.OrderBy(x => x, StringComparer.Ordinal)
				.ToArray();

		/// <summary>Anti-vacuité : les 30 clés existent VRAIMENT au neutre.
		/// Sans ce test, une liste périmée rendrait les autres verts à vide
		/// (elles seraient « absentes » des deux côtés et ignorées).</summary>
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
		public void TheThirtyKeysAreDeliveredInFrFr()
		{
			var fr = Values(FrFrResx);
			var missing = NewNeutralKeys.Where(k => !fr.ContainsKey(k)).ToArray();
			missing.Should().BeEmpty(
				"fr-FR est la langue par défaut du site : les 30 clés de la montée doivent y être livrées : {0}",
				string.Join(" | ", missing));

			var empty = NewNeutralKeys.Where(k => fr.ContainsKey(k) && string.IsNullOrWhiteSpace(fr[k])).ToArray();
			empty.Should().BeEmpty("aucune cellule fr-FR vide : {0}", string.Join(" | ", empty));
		}

		/// <summary>Une cellule fr-FR égale au neutre est une non-traduction — il
		/// n'existe ici aucun homographe légitime (les 18 rôles et les 12 gabarits
		/// changent tous de forme en français).</summary>
		[Fact]
		public void NoFrFrCellIsTheEnglishCopy()
		{
			var neutral = Values(NeutralResx);
			var fr = Values(FrFrResx);
			var copies = NewNeutralKeys
				.Where(k => fr.TryGetValue(k, out var v) && v == neutral[k])
				.ToArray();
			copies.Should().BeEmpty("cellule fr-FR identique à l'anglais (non-traduction) : {0}",
				string.Join(" | ", copies));
		}

		[Fact]
		public void PlaceholderParityWithTheNeutral()
		{
			var neutral = Values(NeutralResx);
			var fr = Values(FrFrResx);
			var offenders = new List<string>();
			foreach (var key in NewNeutralKeys)
			{
				if (!fr.TryGetValue(key, out var value)) continue;
				var expected = Tokens(neutral[key]);
				var actual = Tokens(value);
				if (!expected.SequenceEqual(actual))
					offenders.Add($"{key} :: neutre [{string.Join(",", expected)}] vs fr [{string.Join(",", actual)}]");
			}
			offenders.Should().BeEmpty("parité des jetons DNN et des index de format exigée : {0}",
				string.Join(" | ", offenders));
		}

		[Fact]
		public void FrFrKeepsItsTransportAndDidNotLoseKeys()
		{
			var blob = DnnResxGuardSocle.CommittedBlob(FrFrResx);
			blob.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "BOM UTF-8 conservée");
			blob.Should().NotContain((byte)'\r', "fins de ligne LF seules dans ce pack");

			// 249 clés avant le lot + 30 = 279. Un pack qui perd une clé existante
			// ferait un trou de couverture invisible autrement.
			Values(FrFrResx).Should().HaveCount(279,
				"249 clés historiques + les 30 de la montée : le lot AJOUTE, il ne remplace pas");
		}
	}
}
