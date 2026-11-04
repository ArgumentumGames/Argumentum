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
	/// Garde pt-BR des 30 clés que la montée DNN 10.4.1 (02/11/2026, #2123) a
	/// ajoutées au neutre <c>App_GlobalResources/GlobalResources.resx</c> :
	/// 18 rôles (<c>.Role</c> / <c>.RoleDescription</c>), 10 gabarits de workflow
	/// (<c>Start|Complete|Discard</c> × <c>Workflow|State</c> × <c>Subject|Body</c>)
	/// et 2 e-mails de rappel de mot de passe administrateur. Jumeau pt-BR de
	/// <see cref="DnnGlobalResourcesFrFrGuardTests"/> (#2133), même lot, autre pack.
	/// <para>
	/// Mesuré le 04/11/2026 à <c>af8723f3</c> : le pack pt-BR portait <b>182</b> clés
	/// (les packs ne sont pas jumeaux historiques : fr-FR en avait 249 avant son lot)
	/// et <b>0</b> des 30 neuves — <c>es-ES</c> les porte toutes (paquet officiel aligné
	/// par la montée, référence sémantique, jamais une source), <c>fr-FR</c> les a
	/// reçues de #2133. Transport propre au pack : <b>BOM UTF-8 présente</b>, LF seuls
	/// (mesuré par hexdump — un premier instrument à comparant échappé avait lu
	/// « BOM absente », artefact du littéral, pas du fichier).
	/// </para>
	/// <para>
	/// Terminologie comptée dans le pack avant d'écrire : « função » (7 occurrences
	/// avec pluriel), « redefinição de senha » et « Lembrete de senha » (frères
	/// <c>EMAIL_PASSWORD_REMINDER_*</c>), « registro » / « usuário registrado »,
	/// « e-mail », « página », « conteúdo », « Estado », « Todos », « aprovado »,
	/// « Token », « host ». Formations naturelles SANS précédent pack — le pack de
	/// 182 clés ne couvre pas ce domaine, et l'absence n'est pas une contre-preuve :
	/// « Superusuários », « Gestores », « gerenciar », « rejeitar », « descartar »,
	/// « Assinantes ». Le pack pt-BR descend historiquement de l'espagnol (cellules
	/// « Actualización », « envío » mesurées dans son corpus) : les cellules neuves
	/// sont du portugais propre, épingle par le test de contamination.
	/// </para>
	/// </summary>
	public class DnnGlobalResourcesPtBrGuardTests
	{
		private const string NeutralResx = "DNNPlatform/App_GlobalResources/GlobalResources.resx";
		private const string PtBrResx = "DNNPlatform/App_GlobalResources/GlobalResources.pt-BR.resx";

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
		public void TheThirtyKeysAreDeliveredInPtBr()
		{
			var pt = Values(PtBrResx);
			var missing = NewNeutralKeys.Where(k => !pt.ContainsKey(k)).ToArray();
			missing.Should().BeEmpty(
				"les 30 clés de la montée doivent être livrées au pack pt-BR : {0}",
				string.Join(" | ", missing));

			var empty = NewNeutralKeys.Where(k => pt.ContainsKey(k) && string.IsNullOrWhiteSpace(pt[k])).ToArray();
			empty.Should().BeEmpty("aucune cellule pt-BR vide : {0}", string.Join(" | ", empty));
		}

		/// <summary>Une cellule pt-BR égale au neutre est une non-traduction — il
		/// n'existe ici aucun homographe légitime (les 18 rôles et les 12 gabarits
		/// changent tous de forme en portugais).</summary>
		[Fact]
		public void NoPtBrCellIsTheEnglishCopy()
		{
			var neutral = Values(NeutralResx);
			var pt = Values(PtBrResx);
			var copies = NewNeutralKeys
				.Where(k => pt.TryGetValue(k, out var v) && v == neutral[k])
				.ToArray();
			copies.Should().BeEmpty("cellule pt-BR identique à l'anglais (non-traduction) : {0}",
				string.Join(" | ", copies));
		}

		[Fact]
		public void PlaceholderParityWithTheNeutral()
		{
			var neutral = Values(NeutralResx);
			var pt = Values(PtBrResx);
			var offenders = new List<string>();
			foreach (var key in NewNeutralKeys)
			{
				if (!pt.TryGetValue(key, out var value)) continue;
				var expected = Tokens(neutral[key]);
				var actual = Tokens(value);
				if (!expected.SequenceEqual(actual))
					offenders.Add($"{key} :: neutre [{string.Join(",", expected)}] vs pt-BR [{string.Join(",", actual)}]");
			}
			offenders.Should().BeEmpty("parité des jetons DNN et des index de format exigée : {0}",
				string.Join(" | ", offenders));
		}

		[Fact]
		public void PtBrKeepsItsTransportAndDidNotLoseKeys()
		{
			var blob = DnnResxGuardSocle.CommittedBlob(PtBrResx);
			blob.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "BOM UTF-8 conservée");
			blob.Should().NotContain((byte)'\r', "fins de ligne LF seules dans ce pack");

			// 182 clés avant le lot + 30 = 212. Un pack qui perd une clé existante
			// ferait un trou de couverture invisible autrement.
			Values(PtBrResx).Should().HaveCount(212,
				"182 clés historiques + les 30 de la montée : le lot AJOUTE, il ne remplace pas");
		}

		/// <summary>Le pack pt-BR descend historiquement de l'espagnol (cellules
		/// « Actualización », « envío » mesurées dans son corpus ; cf. la surface
		/// espagnole épinglée par <c>DnnPtBrSpanishSurfaceGuardTests</c>). Les 30
		/// cellules neuves doivent être du portugais : les désinences espagnoles
		/// en « -ción » / « -sión » n'existent pas en portugais (qui écrit
		/// « -ção » / « -são »), et « ñ » n'y figure pas non plus.</summary>
		[Fact]
		public void NoNewCellCarriesThePackSSpanishContamination()
		{
			var pt = Values(PtBrResx);
			var offenders = NewNeutralKeys
				.Where(k => pt.TryGetValue(k, out var v))
				.Select(k => (Key: k, Value: pt[k]))
				.Where(x => x.Value.Contains("ción") || x.Value.Contains("sión") || x.Value.Contains("ñ"))
				.Select(x => x.Key)
				.ToArray();
			offenders.Should().BeEmpty(
				"les cellules neuves pt-BR doivent écrire « -ção »/« -são », jamais les "
				+ "désinences espagnoles dont le pack historique est contaminé : {0}",
				string.Join(" | ", offenders));
		}

		/// <summary>Correction demandée sur la PR (ai-01, c.6629942784) : la première
		/// livraison échappait ses guillemets deux fois — le texte parsé portait
		/// littéralement « &amp;quot;host&amp;quot; », qu'un écran affiche tel quel
		/// au lieu du guillemet. Le lot corrigé écrit des guillemets typographiques
		/// (« “host” »), comme le neutre écrit ses guillemets en clair. Le même
		/// relevé accompagne les packs ru-RU, ar-SA, fa-IR et zh-CN.</summary>
		[Fact]
		public void NoNewCellCarriesEscapedEntityText()
		{
			var pt = Values(PtBrResx);
			var offenders = NewNeutralKeys
				.Where(k => pt.TryGetValue(k, out var v))
				.Select(k => (Key: k, Value: pt[k]))
				.Where(x => CarriesEntityText(x.Value))
				.Select(x => x.Key)
				.ToArray();
			offenders.Should().BeEmpty(
				"le texte parsé d'une cellule neuve ne doit contenir ni \"&amp;\" ni \"&quot;\" "
				+ "(double échappement : l'écran affiche l'entité au lieu du guillemet) : {0}",
				string.Join(" | ", offenders));

			// Contrôle inverse : la forme doublement échappée de la première livraison,
			// une fois parsée, DOIT être vue — et la forme corrigée laissée passer.
			CarriesEntityText("Os superusuários têm acesso de &quot;host&quot; ao site.")
				.Should().BeTrue("le détecteur doit voir la valeur fautive poussée dans la PR");
			CarriesEntityText("Os superusuários têm acesso de “host” ao site.")
				.Should().BeFalse("le détecteur doit laisser passer la forme corrigée");
		}

		/// <summary>Un texte parsé qui contient « &amp;amp; » ou « &amp;quot; » encode
		/// un échappement visible : le lecteur voit l'entité, pas le caractère.</summary>
		private static bool CarriesEntityText(string value) =>
			value.Contains("&amp;") || value.Contains("&quot;");
	}
}
