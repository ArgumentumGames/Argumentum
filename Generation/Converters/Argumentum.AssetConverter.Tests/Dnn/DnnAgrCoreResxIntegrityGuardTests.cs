using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 grain 9-bis — garde d'INTÉGRITÉ des packs de la famille AGR cœur
	/// (Tier 2), par culture. Elle ne contrôle pas la COMPLÉTION (le pack est
	/// volontairement partiel : DNN retombe sur le neutre anglais, clé par clé —
	/// même contrat que le pack Tier 1 ar/fa/zh déjà en ligne, voir
	/// <see cref="ChromeTier1LocalizationContractTests"/>), mais elle garantit que
	/// ce qui EST livré est sain :
	/// <list type="bullet">
	/// <item>toute clé présente existe dans le neutre (pas de clé inventée) ;</item>
	/// <item>toute valeur est non vide ;</item>
	/// <item>le MULTISET des placeholders {0}/{1} est identique à celui du neutre —
	/// c'est le faux vert le plus probable, une traduction qui « déballe » le
	/// placeholder reste verte si l'on ne compare que la présence ;</item>
	/// <item>l'IDENTITÉ des jetons DNN ([Portal:URL], [Custom:4|…]) est conservée —
	/// seule la partie affichable est traduite ;</item>
	/// <item>aucune valeur n'est une copie de l'anglais par accident (une copie
	/// volontaire passe par la liste blanche ci-dessous) ;</item>
	/// <item>l'ENSEMBLE des clés manquantes est DÉCLARÉ, clé par clé, et résolu
	/// par COUPLE (fichier, culture) — défaut du fichier, dérogation du couple si
	/// elle est déclarée : le compte exact ne dit rien de son appartenance
	/// (contre-revue web2 sur #2079, puis sur #2084) ;</item>
	/// <item>une clé dont le NEUTRE est vide n'est JAMAIS livrée : il n'y a rien
	/// à traduire, toute valeur y serait inventée ;</item>
	/// <item>les textes juridiques MESSAGE_PORTAL_TERMS / MESSAGE_PORTAL_PRIVACY
	/// gardent leur valeur déjà arbitrée (grain juridique) — ils ne sont ni
	/// réécrits ni replacés par l'anglais.</item>
	/// </list>
	/// Cultures couvertes : zh-CN (lot 1, PR #2067), ar-SA (lot 2, PR #2068)
	/// puis fa-IR (lot 3). Packs complets <c>SharedResources</c> : zh-CN (lot 4),
	/// ar-SA (lot 5) puis fa-IR (lot 6) — même neutre, donc même dénominateur de 610.
	/// </summary>
	public class DnnAgrCoreResxIntegrityGuardTests
	{
		/// <summary>Couverture livrée par ce grain, par culture et par fichier.</summary>
		private static readonly (string Culture, string Stem, int ExpectedKeys)[] Covered =
		{
			("zh-CN", "FileUpload", 4),
			("zh-CN", "WebControls", 50),
			("zh-CN", "Exceptions", 132),
			// #458 grain zh-CN 30 clés (05/11/2026) : dernier pack du lot — la
			// montée DNN 10.4.1 est livrée sur les trois cultures 172-clés
			// (ar-SA #2149, fa-IR #2151, zh-CN ce grain), 172 -> 202.
			("zh-CN", "GlobalResources", 202),
			// #458 grain 9-bis lot 4 — the neutral carries 617 keys, 7 of them with an
			// EMPTY value (File.Text and six DefaultWorkflowState*.OnDiscard*/OnCompleteMessage*
			// that the product never fills). The pack therefore delivers 610: a key with an
			// empty neutral has nothing to translate and is refused by the builder rather
			// than filled with invention.
			("zh-CN", "SharedResources", 610),
			("ar-SA", "FileUpload", 4),
			("ar-SA", "WebControls", 50),
			("ar-SA", "Exceptions", 132),
			// #458 grain ar-SA 30 clés (04/11/2026) : la montée DNN 10.4.1 avait
			// ajouté 30 clés au neutre ; le pack ar-SA les LIVRE désormais
			// (172 -> 202), suivi de fa-IR (#2151, 05/11) puis de zh-CN
			// (05/11, dernier pack du lot). Les trois cultures 172-clés ont
			// désormais une dérogation vivante dans
			// AllowedMissingByFileAndCulture.
			("ar-SA", "GlobalResources", 202),
			// #458 grain 9-bis lot 5 — same denominator as zh-CN (lot 4): 617 neutral
			// keys, 7 of them empty at the neutral, so 610 translatable.
			("ar-SA", "SharedResources", 610),
			("fa-IR", "FileUpload", 4),
			("fa-IR", "WebControls", 50),
			("fa-IR", "Exceptions", 132),
			("fa-IR", "GlobalResources", 172),
			// #458 grain 9-bis lot 6 — troisième pack complet SharedResources :
			// même neutre que les lots 4 et 5, donc même dénominateur (610).
			("fa-IR", "SharedResources", 610),
		};

		/// <summary>
		/// Jetons d'un pack <c>SharedResources</c> complet — l'ensemble est le MÊME pour
		/// toute culture, puisque le neutre est le même fichier. Déclaré une seule fois :
		/// recopié par culture, il dériverait en silence au lot suivant. Ce sont les huit
		/// littéraux booléens <c>True</c>/<c>False</c> (parsés PAR NOM dans les listes —
		/// les traduire changerait le comportement, pas la langue), la marque produit
		/// « APP ID », le glyphe de tri « A-Z » et les quatre unités B/KB/MB/GB.
		/// </summary>
		private static readonly string[] SharedResourcesPackTokens =
		{
			"True.Text", "False.Text",
			"OAuthConfigBase_APIKey.Text",
			"DropDownList.SortAscendingButtonTitle",
			"SizeB.Text", "SizeKb.Text", "SizeMb.Text", "SizeGb.Text",
		};

		/// <summary>
		/// Clés dont la valeur est légitimement identique à l'anglais : jeton pur,
		/// marque, ponctuation. zh-CN : le sujet d'un courriel de bannière, qui
		/// n'est qu'un jeton (« [Custom:0] »). ar-SA et fa-IR : la même, plus le
		/// suffixe de champ « : » — l'arabe comme le persan emploient le
		/// deux-points latin, ils n'ont pas de variante locale (zh-CN dispose du
		/// « ： » plein-chasse, pas eux). Les packs <c>SharedResources</c> complets
		/// (lots 4 et 5) y ajoutent <see cref="SharedResourcesPackTokens"/>.
		/// Toute AUTRE copie anglaise est un défaut.
		/// </summary>
		private static readonly Dictionary<string, string[]> VerbatimByDesign = new Dictionary<string, string[]>
		{
			["zh-CN"] = new[] { "EMAIL_BANNER_NOTIFICATION_SUBJECT.Text" }
				.Concat(SharedResourcesPackTokens).ToArray(),
			// ar-SA : même noyau (lot 5) que zh-CN (lot 4), plus « FieldSuffix.Text » et
			// le sujet de courriel de bannière déjà arbitrés au lot 2.
			["ar-SA"] = new[] { "EMAIL_BANNER_NOTIFICATION_SUBJECT.Text", "FieldSuffix.Text" }
				.Concat(SharedResourcesPackTokens).ToArray(),
			["fa-IR"] = new[] { "EMAIL_BANNER_NOTIFICATION_SUBJECT.Text", "FieldSuffix.Text" }
				.Concat(SharedResourcesPackTokens).ToArray(),
		};

		/// <summary>
		/// Clés autorisées à MANQUER, **défaut par fichier** : le neutre est le même
		/// pour toutes les cultures, donc l'ensemble manquant est d'abord une
		/// propriété du FICHIER, et le déclarer cinq fois le ferait dériver.
		/// <para>
		/// Cette déclaration ferme le trou que la contre-revue web2 a mesuré sur
		/// #2079 : le compte exact (610) ne dit rien de l'APPARTENANCE de
		/// l'ensemble manquant. Un échange tenant le compte — retirer une vraie clé
		/// et livrer une clé à neutre VIDE avec un texte inventé — passait les deux
		/// gardes, parce que la valeur neutre de <c>File.Text</c> est vide : ni
		/// « copie de l'anglais » ni « valeur vide » ne pouvait la prendre.
		/// </para>
		/// <para>
		/// ⚠️ Un DÉFAUT de fichier n'est pas une vérité par culture. Une culture qui
		/// doit s'en écarter ne modifie pas ce dictionnaire : elle **déclare** son
		/// ensemble dans <see cref="AllowedMissingByFileAndCulture"/>, couple par
		/// couple. La résolution se fait par <see cref="AllowedMissing"/> — jamais
		/// par indexation directe sur le fichier, sinon une dérogation déclarée
		/// resterait sans effet.
		/// </para>
		/// </summary>
		private static readonly Dictionary<string, string[]> AllowedMissingDefaultByStem =
			new Dictionary<string, string[]>
			{
				// Ces trois fichiers sont livrés intégralement : rien ne peut manquer.
				["FileUpload"] = Array.Empty<string>(),
				["WebControls"] = Array.Empty<string>(),
				["Exceptions"] = Array.Empty<string>(),

				// GlobalResources : deux gabarits à neutre VIDE (le produit ne les
				// remplit jamais) et huit blocs volontairement NON traduits — code
				// HTML de pages d'erreur, listes de mots-clés, corps de courriel
				// HTML et licence MIT. Les huit ont un neutre NON vide : sans
				// déclaration, ils rendraient la règle « toute clé neutre non vide
				// est livrée » fausse.
				["GlobalResources"] = new[]
				{
					"MESSAGE_LOGIN_INSTRUCTIONS.Text",
					"MESSAGE_REGISTRATION_INSTRUCTIONS.Text",
					"403_3.Error",
					"404.Error",
					"500.Error",
					"DefaultStopwordGroup.Text",
					"DefaultSynonymGroup.Text",
					"EMAIL_MESSAGING_DISPATCH_BODY.Text",
					"EMAIL_MESSAGING_DISPATCH_ITEM.Text",
					"License.Text",

					// ── Ajouts du NEUTRE par la montée DNN 10.4.1 (02/11/2026, #2123) ──
					// Mesuré le 03/11 sur l'arbre de préprod après restauration des
					// deux blocs juridiques : le neutre porte 211 clés, les packs
					// ar-SA/fa-IR/zh-CN en livrent 172, l'ensemble manquant vaut
					// EXACTEMENT 40 = ces 30 clés + les 10 blocs non traduits ci-dessus.
					// Identique sur les trois cultures au moment de la mesure, donc
					// bien un défaut de FICHIER : c'est le neutre qui a gagné ces
					// clés, pas un pack qui en a perdu. Elles ne sont pas traduites
					// (grain de traduction distinct de #2127) ; les déclarer ici est
					// ce qui empêche un futur lot de croire à une couverture perdue,
					// et une clé RETIRÉE de cette liste sans traduction livrée rend
					// le test rouge (appartenance exacte). DÉROGATIONS ar-SA (04/11),
					// fa-IR (#2151, 05/11) puis zh-CN (05/11) : ces 30 clés SONT
					// livrées par les trois cultures 172-clés, qui déclarent chacune
					// leur ensemble à 10 dans AllowedMissingByFileAndCulture. Le
					// défaut à 40 reste le point de chute de toute future culture
					// couverte — et la référence dont chaque dérogation doit DIFFÉRER.
					"EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_ADMINISTRATOR_BODY.Text",
					"EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_ADMINISTRATOR_SUBJECT.Text",
					"StartWorkflowSubject.Text",
					"StartWorkflowBody.Text",
					"CompleteStateSubject.Text",
					"CompleteStateBody.Text",
					"DiscardStateSubject.Text",
					"DiscardStateBody.Text",
					"CompleteWorkflowSubject.Text",
					"CompleteWorkflowBody.Text",
					"DiscardWorkflowSubject.Text",
					"DiscardWorkflowBody.Text",
					"Administrators.Role",
					"All Users.Role",
					"Superuser.Role",
					"Unauthenticated Users.Role",
					"Registered Users.Role",
					"Unverified Users.Role",
					"Subscribers.Role",
					"Content Editors.Role",
					"Content Managers.Role",
					"Administrators.RoleDescription",
					"All Users.RoleDescription",
					"Content Editors.RoleDescription",
					"Content Managers.RoleDescription",
					"Subscribers.RoleDescription",
					"Superuser.RoleDescription",
					"Unauthenticated Users.RoleDescription",
					"Unverified Users.RoleDescription",
					"Registered Users.RoleDescription",
				},

				// SharedResources : les sept clés à neutre VIDE du fichier neutre
				// (617 − 7 = 610 traduisibles). Le constructeur refuse de les
				// traduire ; la garde refuse qu'un autre chemin les livre.
				["SharedResources"] = new[]
				{
					"DefaultWorkflowState1.OnDiscardMessageBody",
					"DefaultWorkflowState1.OnDiscardMessageSubject",
					"DefaultWorkflowState3.OnCompleteMessageBody",
					"DefaultWorkflowState3.OnCompleteMessageSubject",
					"DefaultWorkflowState3.OnDiscardMessageBody",
					"DefaultWorkflowState3.OnDiscardMessageSubject",
					"File.Text",
				},
			};

		/// <summary>
		/// Dérogations au défaut de fichier, **par couple (fichier, culture)**.
		/// <para>
		/// Ce dictionnaire est né vide (les trois cultures couvertes livraient le
		/// même ensemble manquant, la cause étant dans le NEUTRE commun) pour que le
		/// premier lot où les cultures divergent n'ait pas à choisir entre un rouge
		/// faux et un élargissement silencieux du défaut : il déclare. C'est le point
		/// de fragilité que la contre-revue web2 a relevé sur #2084 — « à passer en
		/// (stem, culture) avant que les lots ne divergent ».
		/// </para>
		/// <para>
		/// Dérogations vivantes : ar-SA (#2149, 04/11/2026) puis fa-IR (#2151,
		/// 05/11) et zh-CN (05/11) — le lot des 30 clés de la montée 10.4.1 est
		/// livré par les trois, dont l'ensemble manquant retombe aux 10 blocs
		/// historiques, sans les 30. Le défaut à 40 ne couvre plus aucune culture
		/// couverte : il reste le point de chute d'une future culture et la
		/// référence dont chaque dérogation doit différer.
		/// </para>
		/// <para>
		/// Une entrée ici doit DIFFÉRER du défaut de son fichier et viser un couple
		/// couvert : les deux sont assertés (<c>EveryOverrideIsLive</c>), sans quoi
		/// une dérogation morte resterait en place en donnant l'illusion d'un cas
		/// particulier traité.
		/// </para>
		/// </summary>
		private static readonly Dictionary<(string Stem, string Culture), string[]> AllowedMissingByFileAndCulture =
			new Dictionary<(string, string), string[]>
			{
				// Les 10 blocs volontairement non traduits de GlobalResources, SANS
				// les 30 clés 10.4.1 livrées par le grain ar-SA (cf. défaut pour
				// la liste commentée des 30).
				[("GlobalResources", "ar-SA")] = new[]
				{
					"MESSAGE_LOGIN_INSTRUCTIONS.Text",
					"MESSAGE_REGISTRATION_INSTRUCTIONS.Text",
					"403_3.Error",
					"404.Error",
					"500.Error",
					"DefaultStopwordGroup.Text",
					"DefaultSynonymGroup.Text",
					"EMAIL_MESSAGING_DISPATCH_BODY.Text",
					"EMAIL_MESSAGING_DISPATCH_ITEM.Text",
					"License.Text",
				},

				// #458 grain zh-CN 30 clés (05/11/2026) — dernier pack du lot :
				// mêmes 10 blocs historiques que ar-SA (et fa-IR #2151), sans
				// les 30 clés 10.4.1 désormais livrées (172 -> 202).
				[("GlobalResources", "zh-CN")] = new[]
				{
					"MESSAGE_LOGIN_INSTRUCTIONS.Text",
					"MESSAGE_REGISTRATION_INSTRUCTIONS.Text",
					"403_3.Error",
					"404.Error",
					"500.Error",
					"DefaultStopwordGroup.Text",
					"DefaultSynonymGroup.Text",
					"EMAIL_MESSAGING_DISPATCH_BODY.Text",
					"EMAIL_MESSAGING_DISPATCH_ITEM.Text",
					"License.Text",
				},
			};

		/// <summary>
		/// Clés que le pack LIVRE alors que le neutre ne les porte PLUS, **déclarées
		/// par fichier**. Sans cette déclaration, une clé orpheline est indistinguable
		/// d'une clé INVENTÉE : les deux sont « livrée, absente du neutre ».
		/// <para>
		/// Mesuré le 03/11/2026 (#2127) : la montée DNN 10.4.1 a retiré
		/// <c>ActivatingLicense.Text</c> du neutre <c>GlobalResources</c> ; les trois
		/// packs ar-SA/fa-IR/zh-CN la livrent encore, à l'identique (1 clé hors-neutre
		/// par culture, mesurée). C'est un défaut de FICHIER — le neutre est commun aux
		/// trois cultures.
		/// </para>
		/// <para>
		/// La déclaration n'est PAS un blanc-seing : l'ensemble livré-hors-neutre est
		/// asserté ÉGAL à celui-ci (<c>TheMissingKeysAreExactlyTheDeclaredOnes</c>), donc
		/// une clé inventée, ou une seconde clé retirée du neutre, reste rouge. Retirer
		/// une entrée ici sans que le pack cesse de livrer la clé rend le test rouge.
		/// </para>
		/// </summary>
		private static readonly Dictionary<string, string[]> RemovedFromNeutralByStem =
			new Dictionary<string, string[]>
			{
				// DNN 10.4.1 (montée du 02/11/2026) : clé de licence disparue du neutre.
				["GlobalResources"] = new[] { "ActivatingLicense.Text" },
			};

		/// <summary>
		/// Clés déclarées orphelines pour un fichier ; liste vide si rien n'est déclaré —
		/// jamais <c>null</c>, pour que les consommateurs n'aient pas à distinguer
		/// « non déclaré » de « déclaré vide ».
		/// </summary>
		private static string[] RemovedFromNeutral(string stem) =>
			RemovedFromNeutralByStem.TryGetValue(stem, out var declared) ? declared : Array.Empty<string>();

		/// <summary>
		/// Clés dont le NEUTRE a été RÉÉCRIT par une montée DNN alors que la traduction
		/// livrée n'a pas encore été rafraîchie. Le décalage est DÉCLARÉ ici avec ses
		/// compteurs MESURÉS, jamais toléré en silence : sans cette table, la garde
		/// d'identité des jetons serait soit rouge à vie (et contournée), soit
		/// désactivée (et aveugle). Les deux compteurs sont assertés — si la traduction
		/// est refaite, ou si le neutre bouge encore, le test rougit et force à
		/// retirer ou mettre à jour la déclaration.
		/// <para>
		/// Cas fondateur (mesuré le 03/11/2026, #2127) : DNN 10.4.1 avait réécrit la
		/// valeur anglaise de <c>EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_BODY.Text</c>
		/// en ajoutant une PHRASE entière (304 → 398 caractères) qui nomme une troisième
		/// fois le portail — 2 → 3 <c>[Portal:PortalName]</c>, soit 4 jetons DNN au
		/// neutre contre 3 dans les trois packs (ar-SA, fa-IR, zh-CN). Déclaration
		/// (3, 4) retirée le 03/11 par la PR de retraduction de cette phrase — la
		/// table est VIDE tant qu'aucun neutre n'a été réécrit sous nos traductions :
		/// la prochaine montée qui le fait se déclare ici, avec ses compteurs mesurés.
		/// Note : <c>docs/dnn-localization/458-email-4th-phrase-ar-fa-zh-2026-11-03.md</c>.
		/// </para>
		/// </summary>
		private static readonly Dictionary<(string Stem, string Key), (int Delivered, int Neutral)> NeutralRewrittenPendingRetranslation =
			new Dictionary<(string, string), (int, int)>
			{
			};

		/// <summary>
		/// Résolution de l'ensemble manquant autorisé pour UN couple : la dérogation
		/// déclarée si elle existe, sinon le défaut du fichier. Aucun autre chemin
		/// n'est admis — une lecture directe de <see cref="AllowedMissingDefaultByStem"/>
		/// ignorerait silencieusement toute dérogation.
		/// </summary>
		private static string[] AllowedMissing(string stem, string culture) =>
			Resolve(AllowedMissingByFileAndCulture, AllowedMissingDefaultByStem, stem, culture).Set;

		/// <summary>
		/// Résolution PURE, sur les dictionnaires passés. La pureté n'est pas une
		/// question de parallélisme (les tests d'une même classe partagent la même
		/// collection xUnit et s'exécutent SÉQUENTIELLEMENT) : c'est la seule façon
		/// de prouver la précédence sur des dictionnaires LOCAUX, sans toucher aux
		/// déclarations de production. Le test <c>OverrideWinsOverTheFileDefault</c>
		/// s'en sert avec des dictionnaires jetables ; la production l'appelle avec
		/// les deux déclarations ci-dessus.
		/// <para>
		/// Elle renvoie aussi le PÉRIMÈTRE appliqué (<see cref="Resolve"/>.Scope) :
		/// le message d'échec du consommateur nomme le chemin pris SANS le
		/// recalculer à côté — un scope dérivé d'une seconde branche divergerait
		/// le jour où la résolution en gagne une troisième.
		/// </para>
		/// </summary>
		private static (string[] Set, string Scope) Resolve(
			IReadOnlyDictionary<(string Stem, string Culture), string[]> overrides,
			IReadOnlyDictionary<string, string[]> defaultsByStem,
			string stem,
			string culture)
		{
			if (overrides.TryGetValue((stem, culture), out var declared))
				return (declared, $"dérogation déclarée pour le couple ({stem}, {culture})");

			if (defaultsByStem.TryGetValue(stem, out var byStem))
				return (byStem, $"défaut du fichier « {stem} » (aucune dérogation pour {culture})");

			throw new InvalidOperationException(
				$"Aucun ensemble manquant déclaré pour « {stem} » (couple {stem}.{culture}) : "
				+ "ajouter le défaut du fichier dans AllowedMissingDefaultByStem, ou une dérogation "
				+ "par couple dans AllowedMissingByFileAndCulture.");
		}

		/// <summary>
		/// Une dérogation est MORTE si son ensemble est ÉQUIVALENT au défaut de son
		/// fichier — mêmes clés, l'ordre près : le consommateur trie les deux côtés
		/// avant de comparer, donc une telle dérogation ne change AUCUN test tout en
		/// donnant l'illusion qu'un cas particulier est traité. Pure, pour être
		/// prouvée sur dictionnaires locaux (<c>AnOverrideCopyingItsDefaultIsDead</c>)
		/// — le dictionnaire réel étant vide, sa logique de refus n'a sinon aucun
		/// sujet vivant en CI.
		/// </summary>
		private static bool OverrideIsDead(
			IReadOnlyDictionary<string, string[]> defaultsByStem,
			(string Stem, string Culture) key,
			string[] declared) =>
			defaultsByStem.TryGetValue(key.Stem, out var byStem)
			&& declared.OrderBy(x => x, StringComparer.Ordinal)
				.SequenceEqual(byStem.OrderBy(x => x, StringComparer.Ordinal));

		private static readonly Regex Placeholder = new(@"\{\d+\}", RegexOptions.Compiled);

		/// <summary>
		/// Identité d'un jeton DNN : namespace + chemin AVANT le « | ». La partie
		/// affichable qui suit est de la prose à traduire, pas une identité.
		/// </summary>
		private static readonly Regex Token = new(
			@"\[(?:Portal|User|Host|Profile|Membership|Custom|Date|Module|Tab|Site|Page|App):([^\]\|]*)",
			RegexOptions.Compiled);

		private static readonly string[] LegalKeys =
		{
			"MESSAGE_PORTAL_TERMS.Text",
			"MESSAGE_PORTAL_PRIVACY.Text",
		};

		private static XDocument Load(string stem, string culture)
		{
			var rel = $"DNNPlatform/App_GlobalResources/{stem}{(culture.Length > 0 ? "." + culture : "")}.resx";
			var path = Path.Combine(TestRepoRoot.Find(), rel.Replace('/', Path.DirectorySeparatorChar));
			File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({rel})");
			return XDocument.Load(path);
		}

		private static Dictionary<string, string> Values(string stem, string culture) =>
			Load(stem, culture).Root!.Elements("data").ToDictionary(
				d => (string)d.Attribute("name")!,
				d => (string?)d.Element("value") ?? string.Empty,
				StringComparer.Ordinal);

		public static IEnumerable<object[]> CoveredFiles()
		{
			foreach (var (culture, stem, expected) in Covered)
				yield return new object[] { culture, stem, expected };
		}

		[Theory]
		[MemberData(nameof(CoveredFiles))]
		public void PackCoversTheDeclaredKeyCount(string culture, string stem, int expectedKeys)
		{
			Values(stem, culture).Should().HaveCount(expectedKeys,
				$"{stem}.{culture}.resx : le nombre de clés livrées a changé — mettre à jour la garde");
		}

		[Theory]
		[MemberData(nameof(CoveredFiles))]
		public void EveryDeliveredKeyIsSaneAgainstTheNeutral(string culture, string stem, int _)
		{
			var neutral = Values(stem, string.Empty);
			var localized = Values(stem, culture);
			var verbatim = VerbatimByDesign[culture];
			var removed = RemovedFromNeutral(stem);

			foreach (var (key, value) in localized)
			{
				if (removed.Contains(key, StringComparer.Ordinal))
				{
					// Clé ORPHELINE DÉCLARÉE : le neutre ne la porte plus (retirée par une
					// montée DNN). Le pack la livre encore ; DNN ignore une clé sans
					// contrepartie neutre, il n'y a donc plus de source à laquelle la
					// comparer. On n'exige que sa non-vacuité — et l'APPARTENANCE exacte
					// de cet ensemble est assertée dans TheMissingKeysAreExactlyTheDeclaredOnes,
					// sans quoi cette branche deviendrait le blanc-seing des clés inventées.
					value.Should().NotBeNullOrWhiteSpace(
						$"{stem}.{culture}:{key} — clé orpheline déclarée mais VIDE");
					continue;
				}

				neutral.Should().ContainKey(key, $"{stem}.{culture} : clé « {key} » absente du neutre");
				var source = neutral[key];

				value.Should().NotBeNullOrWhiteSpace($"{stem}.{culture}:{key} — valeur vide");

				// §4 de la contre-revue web2 — la garantie STRUCTURELLE ne dépend
				// pas de la liste blanche : elle vaut pour TOUTE clé livrée. Les
				// `continue` ci-dessous ne court-circuitent plus que la comparaison
				// à l'anglais, qui est la seule règle qu'ils doivent court-circuiter.
				Placeholder.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
					.Should().Equal(Placeholder.Matches(source).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
						$"{stem}.{culture}:{key} — multiset des placeholders modifié");

				if (NeutralRewrittenPendingRetranslation.TryGetValue((stem, key), out var declaredDrift))
				{
					// Le neutre a été réécrit par une montée : la traduction est en retard
					// d'une phrase. La déclaration porte les DEUX compteurs mesurés ;
					// on les asserte tels quels, donc une nouvelle dérive du neutre — ou
					// une retraduction qui arrive — rougit et force à mettre à jour la table.
					Token.Matches(value).Count().Should().Be(declaredDrift.Delivered,
						$"{stem}.{culture}:{key} — le pack a changé de nombre de jetons ; mettre à jour "
						+ "NeutralRewrittenPendingRetranslation (ou retirer la déclaration si la traduction est refaite)");
					Token.Matches(source).Count().Should().Be(declaredDrift.Neutral,
						$"{stem}:{key} — le neutre a encore bougé ; remesurer le décalage déclaré");
					continue;
				}

				Token.Matches(value).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal)
					.Should().Equal(Token.Matches(source).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal),
						$"{stem}.{culture}:{key} — identité des jetons DNN modifiée");

				// §3 de la même contre-revue — une clé dont le NEUTRE est vide ne
				// doit JAMAIS être livrée : il n'y a rien à traduire. Le
				// constructeur refuse de l'inventer ; la garde refuse qu'un autre
				// chemin la livre.
				source.Should().NotBeNullOrWhiteSpace(
					$"{stem}.{culture}:{key} — le neutre de cette clé est VIDE : elle ne doit pas être "
					+ $"livrée (toute valeur ici serait inventée)");

				if (verbatim.Contains(key, StringComparer.Ordinal))
					continue;

				if (LegalKeys.Contains(key, StringComparer.Ordinal))
					continue;

				value.Should().NotBe(source,
					$"{stem}.{culture}:{key} — copie de l'anglais par accident (une copie VOLONTAIRE "
					+ $"se déclare dans VerbatimByDesign[{culture}])");
			}
		}

		/// <summary>
		/// §3 de la contre-revue web2 — l'APPARTENANCE de l'ensemble manquant est
		/// déclarée, clé par clé. Le compte exact ne la disait pas : un échange
		/// tenant le compte (retirer une vraie clé, livrer une clé à neutre vide
		/// avec un texte inventé) passait les deux gardes. Ce test est ce qui rend
		/// <c>CompletedPacks</c> sûr à étendre au lot suivant.
		/// </summary>
		[Theory]
		[MemberData(nameof(CoveredFiles))]
		public void TheMissingKeysAreExactlyTheDeclaredOnes(string culture, string stem, int _)
		{
			var neutral = Values(stem, string.Empty);
			var localized = Values(stem, culture);

			var missing = neutral.Keys.Except(localized.Keys, StringComparer.Ordinal)
				.OrderBy(x => x, StringComparer.Ordinal).ToArray();

			// Le périmètre de la déclaration appliquée est nommé dans le message :
			// « défaut du fichier » et « dérogation du couple » n'ont pas la même
			// portée, et un message qui ne le dit pas fait chercher au mauvais endroit.
			// Il vient DE LA RÉSOLUTION elle-même (Resolve.Scope) — pas d'une seconde
			// branche recalculée à côté, qui divergerait si Resolve gagnait un chemin.
			var resolved = Resolve(AllowedMissingByFileAndCulture, AllowedMissingDefaultByStem, stem, culture);
			var scope = resolved.Scope;
			var declared = resolved.Set.OrderBy(x => x, StringComparer.Ordinal).ToArray();

			missing.Should().Equal(declared,
				$"{stem}.{culture}.resx : l'ensemble des clés manquantes a changé, mesuré contre la "
				+ $"{scope}. Une clé absente non déclarée est une couverture perdue ; une clé "
				+ $"déclarée présente est un remplissage non autorisé (invention).");

			// Clés livrées HORS NEUTRE : orphelines DÉCLARÉES (le neutre les a perdues
			// lors d'une montée) — l'ensemble doit être EXACTEMENT celui déclaré, donc
			// une clé inventée reste rouge, et retirer une déclaration sans que le pack
			// cesse de livrer la clé rougit aussi. C'est ce qui rend inoffensive la
			// branche « orpheline » de EveryDeliveredKeyIsSaneAgainstTheNeutral.
			var removed = RemovedFromNeutral(stem).OrderBy(x => x, StringComparer.Ordinal).ToArray();
			localized.Keys.Except(neutral.Keys, StringComparer.Ordinal)
				.OrderBy(x => x, StringComparer.Ordinal)
				.Should().Equal(removed,
					$"{stem}.{culture}.resx : l'ensemble des clés livrées ABSENTES du neutre a changé — "
					+ "une clé orpheline doit être déclarée dans RemovedFromNeutralByStem (le neutre l'a "
					+ "perdue lors d'une montée) et une déclaration morte doit être retirée");
		}

		/// <summary>
		/// La déclaration des ensembles manquants doit couvrir EXACTEMENT les fichiers
		/// sous garde : un fichier couvert sans défaut rendrait la résolution
		/// auto-rouge par exception (message cryptique au milieu de vingt tests),
		/// et un défaut sans fichier couvert est une déclaration morte que personne
		/// ne relira.
		/// </summary>
		[Fact]
		public void TheMissingSetDeclarationCoversExactlyTheGuardedFiles()
		{
			var guardedStems = Covered.Select(c => c.Stem).Distinct(StringComparer.Ordinal)
				.OrderBy(x => x, StringComparer.Ordinal).ToArray();
			var declaredStems = AllowedMissingDefaultByStem.Keys
				.OrderBy(x => x, StringComparer.Ordinal).ToArray();

			declaredStems.Should().Equal(guardedStems,
				"les défauts d'ensemble manquant doivent couvrir exactement les fichiers sous garde "
				+ "(ajouter le fichier ET son défaut, ou retirer les deux)");

			foreach (var (culture, stem, _) in Covered)
			{
				// Aucun throw : prouve que la résolution aboutit pour chaque couple
				// couvert (5 fichiers × 3 cultures = 15 aujourd'hui — se recompter,
				// pas se recopier).
				AllowedMissing(stem, culture).Should().NotBeNull(
					$"la résolution de l'ensemble manquant doit aboutir pour {stem}.{culture}");
			}
		}

		/// <summary>
		/// Une dérogation doit être VIVANTE : elle vise un couple réellement sous
		/// garde, et son contenu diffère du défaut de son fichier. Recopier le défaut
		/// dans une dérogation ne change aucun test — elle donne seulement l'illusion
		/// qu'un cas particulier est traité, et masque le jour où les deux devraient
		/// diverger.
		/// <para>
		/// L'équivalence est mesurée par <see cref="OverrideIsDead"/> — à l'ORDRE
		/// près, comme le consommateur qui trie : une dérogation portant les mêmes
		/// clés dans un autre ordre ne change aucun test non plus, et une jointure
		/// de chaînes la déclarerait vivante à tort (en inventant de surcroît une
		/// sémantique de séquence que personne d'autre n'utilise).
		/// </para>
		/// </summary>
		[Fact]
		public void EveryOverrideIsLive()
		{
			foreach (var (key, declared) in AllowedMissingByFileAndCulture)
			{
				Covered.Should().Contain(c => c.Culture == key.Culture && c.Stem == key.Stem,
					$"la dérogation ({key.Stem}, {key.Culture}) vise un couple qui n'est pas sous garde : "
					+ "elle ne s'appliquera jamais");

				AllowedMissingDefaultByStem.Should().ContainKey(key.Stem,
					$"la dérogation ({key.Stem}, {key.Culture}) vise un fichier qui n'a aucun défaut déclaré");

				OverrideIsDead(AllowedMissingDefaultByStem, key, declared).Should().BeFalse(
					$"la dérogation ({key.Stem}, {key.Culture}) est équivalente au défaut de son fichier "
					+ "(mêmes clés, l'ordre près) : elle ne change aucun test — soit elle est inutile "
					+ "(la retirer), soit les deux ont divergé et l'un des deux est faux");
			}
		}

		/// <summary>
		/// Contrôle inverse de <see cref="EveryOverrideIsLive"/> sur dictionnaires
		/// LOCAUX : le refus des dérogations mortes ne se vérifie pas sur la seule
		/// entrée vivante du dictionnaire réel — sans ce test, une régression du
		/// prédicat (comparaison ordonnée, jointure de chaînes) resterait verte.
		/// Mêmes clés dans un autre
		/// ORDRE = morte ; clés différentes = vivante ; couple sans défaut = vivante
		/// (l'absence de défaut est attrapée par la garde de couverture, pas ici).
		/// </summary>
		[Fact]
		public void AnOverrideCopyingItsDefaultIsDead()
		{
			var defaults = new Dictionary<string, string[]> { ["S"] = new[] { "a", "b" } };

			OverrideIsDead(defaults, ("S", "fr"), new[] { "b", "a" }).Should().BeTrue(
				"mêmes clés dans un autre ordre : le consommateur trie, la dérogation ne change aucun test");
			OverrideIsDead(defaults, ("S", "fr"), new[] { "a", "b" }).Should().BeTrue(
				"copie exacte du défaut : déclaration morte");
			OverrideIsDead(defaults, ("S", "fr"), new[] { "a", "c" }).Should().BeFalse(
				"clés différentes : la dérogation change les tests, elle est vivante");
			OverrideIsDead(defaults, ("T", "fr"), new[] { "a" }).Should().BeFalse(
				"un couple sans défaut déclaré ne peut pas être jugé équivalent ici");
		}

		/// <summary>
		/// Précédence prouvée sur des dictionnaires LOCAUX : la dérogation gagne sur
		/// le défaut, et un couple sans aucune déclaration échoue explicitement plutôt
		/// que de retomber silencieusement sur autre chose. Sans ce test, une erreur
		/// d'ordre dans <see cref="Resolve"/> rendrait TOUS les tests verts — chaque
		/// couple couvert ayant, aujourd'hui, un défaut valide.
		/// </summary>
		[Fact]
		public void OverrideWinsOverTheFileDefault()
		{
			var defaults = new Dictionary<string, string[]> { ["S"] = new[] { "a" } };
			var overrides = new Dictionary<(string, string), string[]>
			{
				[("S", "fr")] = new[] { "b" },
			};

			Resolve(overrides, defaults, "S", "fr").Set.Should().Equal(new[] { "b" },
				"la dérogation du couple doit gagner sur le défaut du fichier");
			Resolve(overrides, defaults, "S", "en").Set.Should().Equal(new[] { "a" },
				"un couple sans dérogation prend le défaut de son fichier");

			Action unknown = () => Resolve(overrides, defaults, "T", "fr");
			unknown.Should().Throw<InvalidOperationException>()
				.Which.Message.Should().Contain("Aucun ensemble manquant déclaré");

			// Contrôle inverse de la précédence : sans dérogation, « fr » suivrait le
			// défaut. Si l'assertion ci-dessus passait par accident, celle-ci tomberait.
			Resolve(new Dictionary<(string, string), string[]>(), defaults, "S", "fr").Set
				.Should().Equal(new[] { "a" });
		}

		[Theory]
		[InlineData("zh-CN")]
		[InlineData("ar-SA")]
		[InlineData("fa-IR")]
		public void LegalTextsKeepTheirAlreadyArbitratedValues(string culture)
		{
			var localized = Values("GlobalResources", culture);
			foreach (var key in LegalKeys)
			{
				localized.Should().ContainKey(key,
					$"GlobalResources.{culture} : le texte juridique « {key} » a disparu du pack");
				localized[key].Should().NotBeEmpty();
				localized[key].Should().NotBe(Values("GlobalResources", string.Empty)[key],
					$"GlobalResources.{culture}:{key} — le texte juridique a été replacé par l'anglais");
			}
		}
	}
}
