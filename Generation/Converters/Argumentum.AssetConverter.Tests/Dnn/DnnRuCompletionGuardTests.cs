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
	/// Garde de complétion ru-RU (pool #458, dispatch ai-01 19/10 09:17 item
	/// po-2024.1) : la base R7 (#1903, 162 resx MIT) couvrait 951/1005 clés
	/// du neutre 10.3.2 ; cette PR complète les 54 clés manquantes — 53
	/// livrées ici (38 traductions RU + 9 vides miroir du neutre + 6 verbatim
	/// par nature) + 4 rafraîchissements de valeurs stale (EN 9.6 → 10.3.2
	/// modifié : Event Viewer→Admin Logs, sujet SMTP « from [Portal:PortalName] »,
	/// exemple URL http→https, reformulation PrivateMessageDisabled). Le
	/// fichier List_Country.ru-RU.resx est NOUVEAU : 256 clés ISO alpha-2,
	/// valeurs CLDR main ru.xml (254) + AN/CS codes abrogés déclarés [K].
	/// <list type="bullet">
	/// <item>MESSAGE_PORTAL_PRIVACY.Text (traduction intégrale) + tête LCEN
	/// de TERMS (identité ARGUMENTUM) : livrés par le grain juridique empilé
	/// (même chantier que #1876/#1883) — l'ancien témoin d'absence est
	/// devenu la garde de présence ci-dessous.</item>
	/// <item>Les 10 clés « r7-only » (DASHBOARD_*, Insecure*, retirées du
	/// neutre 10.3.2) sont laissées telles qu'importées (décision ai-01).</item>
	/// <item>Les 6 verbatim sont vérifiés par ÉGALITÉ au neutre (miroir
	/// dérivé, pas de pin figé) : License (texte MIT anglais), 500.Error
	/// « {0} », DefaultSynonymGroup, EMAIL_BANNER_NOTIFICATION_SUBJECT
	/// « [Custom:0] », EMAIL_MESSAGING_DISPATCH_ITEM (HTML+tokens purs,
	/// mesuré 0 prose) et FieldSuffix « : ».</item>
	/// </list>
	/// </summary>
	public class DnnRuCompletionGuardTests
	{
		private const string SharedResx = "DNNPlatform/App_GlobalResources/SharedResources.ru-RU.resx";
		private const string GlobalResx = "DNNPlatform/App_GlobalResources/GlobalResources.ru-RU.resx";
		private const string ExceptionsResx = "DNNPlatform/App_GlobalResources/Exceptions.ru-RU.resx";
		private const string WebControlsResx = "DNNPlatform/App_GlobalResources/WebControls.ru-RU.resx";
		private const string ListCountryRuResx = "DNNPlatform/App_GlobalResources/List_Country.ru-RU.resx";
		private const string ListCountryNeutralResx = "DNNPlatform/App_GlobalResources/List_Country.resx";
		private const string SharedNeutralResx = "DNNPlatform/App_GlobalResources/SharedResources.resx";
		private const string GlobalNeutralResx = "DNNPlatform/App_GlobalResources/GlobalResources.resx";
		private const string ExceptionsNeutralResx = "DNNPlatform/App_GlobalResources/Exceptions.resx";
		private const string WebControlsNeutralResx = "DNNPlatform/App_GlobalResources/WebControls.resx";

		private static XDocument Load(string resxRelativePath)
		{
			var path = Path.Combine(TestRepoRoot.Find(), resxRelativePath.Replace('/', Path.DirectorySeparatorChar));
			File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({resxRelativePath})");
			return XDocument.Load(path);
		}

		private static string ResxValue(string resxRelativePath, string key)
		{
			var entry = Load(resxRelativePath).Root!.Elements("data")
				.FirstOrDefault(d => string.Equals(d.Attribute("name")?.Value, key, StringComparison.Ordinal));
			entry.Should().NotBeNull($"la clé {key} doit exister dans {resxRelativePath}");
			return entry!.Element("value")?.Value ?? "";
		}

		private static bool HasKey(string resxRelativePath, string key) =>
			Load(resxRelativePath).Root!.Elements("data")
				.Any(d => string.Equals(d.Attribute("name")?.Value, key, StringComparison.Ordinal));

		// ------------------------------------------------------------------
		// Les 53 clés livrées, par fichier (38 traduites + 9 vides + 6 verbatim).
		private static readonly string[] SharedAdded =
		{
			"EXCEPTION_GlobDotDotNotSupportedInCleanup.Text", "ErrorDecryptingSetting.Text",
			"SQL_Manifest_BadFile.Text", "SQL_Manifest_Error.Text",
		};

		private static readonly string[] SharedEmpties =
		{
			"DefaultWorkflowState1.OnDiscardMessageBody", "DefaultWorkflowState1.OnDiscardMessageSubject",
			"DefaultWorkflowState3.OnCompleteMessageBody", "DefaultWorkflowState3.OnCompleteMessageSubject",
			"DefaultWorkflowState3.OnDiscardMessageBody", "DefaultWorkflowState3.OnDiscardMessageSubject",
			"File.Text",
		};

		private static readonly string[] ExceptionsAdded =
		{
			"AddFolderNotAllowed.Text", "AmbiguousController.Text", "ContentTypeFieldDefinitionDoesNotExist.Text",
			"ControllerNotFound.Text", "CreateValidatorException.Text", "DynamicContentEntityInUse.Text",
			"EntityIsNotValid.Text", "ErrorMessageCannotBeNullOrEmpty.Text", "FieldDefinitionInvalidDeadLoop.Text",
			"FileNameInvalid.Text", "FolderAlreadyExists.Text", "InvalidMappedPath.Text",
			"InvalidValidatorException.Text", "JsonContentTypeInvalid.Text", "JsonInvalidField.Text",
			"MsgEmptyToListFoundError.Text", "MsgRolesOrUsersRequiredError.Text", "PropertyCannotBeEmpty.Text",
			"SitemapProviderError.Text", "SitemapUrlGenerationError.Text", "TabHasNotAnUnpublishedVersion.Text",
			"TabVersionCannotBeDiscarded_OnlyOneVersion.Text", "TabVersionCannotBeRolledBack_LastVersion.Text",
			"UpdateUserMembershipFailed.Text", "UserHasNoPermissionToAdd.Text", "UserHasNoPermissionToBrowseFolder.Text",
			"UserHasNoPermissionToEditFile.Text", "UserHasNoPermissionToEditFolder.Text",
			"UserRelationshipExistsError.Text", "WorkflowAlreadyStarted.Text",
			"WorkflowSystemWorkflowStateCannotComplete.Text",
		};

		private static readonly string[] GlobalTranslated =
		{
			"403_3.Error", "404.Error", "EMAIL_MESSAGING_DISPATCH_BODY.Text",
		};

		private static readonly string[] GlobalEmpties =
		{
			"MESSAGE_LOGIN_INSTRUCTIONS.Text", "MESSAGE_REGISTRATION_INSTRUCTIONS.Text",
		};

		/// <summary>(fichier RU, fichier neutre, clé) — égalité exigée.</summary>
		private static readonly (string ru, string neutral, string key)[] VerbatimPairs =
		{
			(GlobalResx, GlobalNeutralResx, "500.Error"),
			(GlobalResx, GlobalNeutralResx, "DefaultSynonymGroup.Text"),
			(GlobalResx, GlobalNeutralResx, "EMAIL_BANNER_NOTIFICATION_SUBJECT.Text"),
			(GlobalResx, GlobalNeutralResx, "EMAIL_MESSAGING_DISPATCH_ITEM.Text"),
			(GlobalResx, GlobalNeutralResx, "License.Text"),
			(WebControlsResx, WebControlsNeutralResx, "FieldSuffix.Text"),
		};

		[Fact]
		public void Delivered53_AllPresent_TranslatedCarryCyrillic()
		{
			var cyrillic = new Regex("[А-Яа-яЁё]");
			foreach (var key in SharedAdded)
			{
				HasKey(SharedResx, key).Should().BeTrue("clé livrée {0}.", key);
				cyrillic.IsMatch(ResxValue(SharedResx, key)).Should().BeTrue(
					"{0} : une traduction RU livrée doit porter du cyrillique.", key);
			}
			foreach (var key in ExceptionsAdded)
			{
				HasKey(ExceptionsResx, key).Should().BeTrue("clé livrée {0}.", key);
				cyrillic.IsMatch(ResxValue(ExceptionsResx, key)).Should().BeTrue(
					"{0} : une traduction RU livrée doit porter du cyrillique.", key);
			}
			foreach (var key in GlobalTranslated)
			{
				HasKey(GlobalResx, key).Should().BeTrue("clé livrée {0}.", key);
				cyrillic.IsMatch(ResxValue(GlobalResx, key)).Should().BeTrue(
					"{0} : une traduction RU livrée doit porter du cyrillique.", key);
			}
			HasKey(WebControlsResx, "FieldSuffix.Text").Should().BeTrue("clé verbatim livrée.");
		}

		/// <summary>Les 9 vides livrés MIRRORENT le neutre : la garde dérive
		/// l'exigence (neutre vide ⇒ RU vide), pas de liste d'attente figée.</summary>
		[Fact]
		public void Empties9_MirrorNeutralEmptyValues()
		{
			foreach (var key in SharedEmpties)
			{
				ResxValue(SharedNeutralResx, key).Should().BeEmpty("{0} : le neutre est vide — invariant du miroir.", key);
				ResxValue(SharedResx, key).Should().BeEmpty("{0} : le RU doit livrer la même vacuité.", key);
			}
			foreach (var key in GlobalEmpties)
			{
				ResxValue(GlobalNeutralResx, key).Should().BeEmpty("{0} : le neutre est vide — invariant du miroir.", key);
				ResxValue(GlobalResx, key).Should().BeEmpty("{0} : le RU doit livrer la même vacuité.", key);
			}
		}

		/// <summary>Verbatim par nature : la valeur RU est ÉGALE au neutre —
		/// un futur import qui la « traduirait » rougira ici.</summary>
		[Fact]
		public void Verbatim6_EqualNeutral()
		{
			foreach (var (ru, neutral, key) in VerbatimPairs)
				ResxValue(ru, key).Should().Be(
					ResxValue(neutral, key),
					"{0} : verbatim par nature (identifiant, jeton ou texte juridique) — miroir du neutre.", key);
		}

		[Fact]
		public void Refresh4_StaleValuesBroughtTo10_3_2()
		{
			ResxValue(SharedResx, "CriticalError.Error").Should().Be(
				"Возникла критическая ошибка. Подробности доступны в журнале администратора.",
				"EN 10.3.2 « Admin Logs » — l'ancienne valeur R7 (9.6) disait « журнале событий ».");
			ResxValue(GlobalResx, "EMAIL_SMTP_TEST_SUBJECT.Text").Should().Be(
				"[Host:HostTitle] Тест конфигурации SMTP от [Portal:PortalName]",
				"EN 10.3.2 ajoute « from [Portal:PortalName] » — l'ancienne valeur R7 s'arrêtait à SMTP.");
			ResxValue(ExceptionsResx, "PrivateMessageDisabledError.Text").Should().Be(
				"Текущие настройки обмена сообщениями не разрешают отправку сообщений другим пользователям/группам.",
				"EN 10.3.2 « Current messaging settings do not allow… » — miroir direct.");
			ResxValue(WebControlsResx, "FileUpload.UrlTooltip.Text").Should().Be(
				"Введите URL-адрес ресурса, например https://SomeWebSite.com/Images/About.png",
				"EN 10.3.2 : exemple https:// (l'ancienne valeur R7 citait http://).");
		}

		[Fact]
		public void Tokens_SurviveInErrorPagesAndEmailTemplate()
		{
			var v403 = ResxValue(GlobalResx, "403_3.Error");
			v403.Should().Contain("{0}", "la page 403 substitue le message d'erreur technique.");
			v403.Should().Contain("IIS AppPool", "la consigne d'ACL cite le compte du pool IIS.");
			v403.Should().Contain("images/403-3.gif", "l'illustration de la page est conservée.");

			var v404 = ResxValue(GlobalResx, "404.Error");
			v404.Should().Contain("Домен {0} не существует в базе данных", "en-tête du 404 (jeton = le domaine demandé).");
			Regex.Matches(v404, "Имя домена: ").Count.Should().Be(5,
				"les 5 lignes d'exemples URL du 404 gardent leur libellé localisé (URLs intactes).");
			foreach (var url in new[]
			{
				"https://www.domain.com/default.aspx", "https://209.75.24.131/default.aspx",
				"https://localhost/DotNetNuke/default.aspx", "https://www.domain.com/virtualdirectory/default.aspx",
				"https://www.domain.com/directory/default.aspx",
			})
				v404.Should().Contain(url, "l'exemple {0} reste byte-identique au neutre.", url);

			var email = ResxValue(GlobalResx, "EMAIL_MESSAGING_DISPATCH_BODY.Text");
			foreach (var token in new[]
			{ "[NOTIFICATIONURL]", "[SITEURL]", "[LOGOURL]", "[PORTALNAME]", "[MESSAGEBODY]", "[UNSUBSCRIBEURL]" })
				email.Should().Contain(token, "EMAIL_MESSAGING_DISPATCH_BODY : jeton {0} préservé.", token);
			email.Should().Contain("Проблемы с просмотром письма?", "préheader RU.");
			email.Should().Contain(">Уведомления</h2>", "titre du gabarit RU.");
			email.Should().Contain("Управление подписками", "lien de gestion des abonnements.");
			email.Should().Contain("Просмотреть онлайн", "lien « View online » localisé.");
			email.Should().Contain("Все права защищены.", "copyright du gabarit.");
			email.Should().Contain("отменить подписку", "phrase de désabonnement.");
		}

		/// <summary>List_Country.ru-RU.resx (nouveau) : 256 clés ISO == jeu du
		/// neutre, AUCUNE valeur vide, échantillons CLDR épinglés.</summary>
		[Fact]
		public void ListCountryRu_256Keys_FilledFromCldr_SamplesPinned()
		{
			var neutralKeys = Load(ListCountryNeutralResx).Root!.Elements("data")
				.Select(d => d.Attribute("name")!.Value).ToArray();
			var ru = Load(ListCountryRuResx).Root!.Elements("data")
				.ToDictionary(d => d.Attribute("name")!.Value, d => d.Element("value")?.Value ?? "");

			neutralKeys.Should().HaveCount(256, "le neutre porte les 256 codes ISO alpha-2.");
			ru.Keys.Should().BeEquivalentTo(neutralKeys, "le fichier RU couvre exactement le même jeu de codes.");
			ru.Where(kv => string.IsNullOrWhiteSpace(kv.Value)).Select(kv => kv.Key).Should().BeEmpty(
				"toutes les valeurs RU sont remplies (fr-FR témoin de la convention : 256/256)");

			foreach (var (code, expected) in new[]
			{
				("US.Text", "Соединенные Штаты"),
				("FR.Text", "Франция"),
				("DE.Text", "Германия"),
				("GB.Text", "Великобритания"),
				("ZZ.Text", "неизвестный регион"),
				("AN.Text", "Нидерландские Антильские острова"),  // [K] code ISO abrogé (2010), absent CLDR
				("CS.Text", "Сербия и Черногория"),               // [K] code ISO abrogé (2006), absent CLDR
			})
				ru.Should().ContainKey(code).WhoseValue.Should().Be(expected, "pin CLDR {0}.", code);
		}

		/// <summary>Grain juridique : PRIVACY livré — présence, cyrillique,
		/// parité de jetons DÉRIVÉE du neutre vivant (substitutions de portail,
		/// courriel de contact) et les 6 sous-titres localisés.</summary>
		[Fact]
		public void MessagePortalPrivacy_Present_TranslatedWithTokens()
		{
			HasKey(GlobalResx, "MESSAGE_PORTAL_PRIVACY.Text").Should().BeTrue(
				"livré par le grain juridique (miroir #1876/#1883) — l'ancien témoin d'absence est retourné.");
			var ru = ResxValue(GlobalResx, "MESSAGE_PORTAL_PRIVACY.Text");
			var neutral = ResxValue(GlobalNeutralResx, "MESSAGE_PORTAL_PRIVACY.Text");
			new Regex("[А-Яа-яЁё]").IsMatch(ru).Should().BeTrue("la traduction RU porte du cyrillique.");
			Regex.Matches(ru, Regex.Escape("[Portal:PortalName]")).Count.Should().Be(
				Regex.Matches(neutral, Regex.Escape("[Portal:PortalName]")).Count,
				"chaque substitution de portail du neutre est préservée (parité dérivée, pas de pin figé).");
			ru.Should().Contain("mailto:[Portal:Email]", "le lien de contact garde son jeton de courriel.");
			Regex.Matches(ru, "SubHead").Count.Should().Be(6, "les 6 sous-titres du neutre sont localisés.");
			ru.Should().StartWith("<div align=\"left\">");
			ru.Should().EndWith("</div>", "structure HTML du bloc préservée.");
		}

		/// <summary>Grain juridique : TERMS = tête LCEN 10.3.2 traduite (identité
		/// légale VERBATIM — adresse, SIREN et URL legifrance ne se traduisent pas)
		/// + corps R7 préservé (boilerplate EN byte-identique entre 9.6 et 10.3.2,
		/// mesuré 15 715 car — pas de retraduction nécessaire).</summary>
		[Fact]
		public void LegalTerms_LcenHeadTranslated_IdentityVerbatim_R7BodyPreserved()
		{
			var v = ResxValue(GlobalResx, "MESSAGE_PORTAL_TERMS.Text");
			v.Should().Contain("ЮРИДИЧЕСКАЯ ИНФОРМАЦИЯ", "en-tête du Legal Notice 10.3.2.");
			v.Should().Contain("ИДЕНТИФИКАЦИЯ САЙТА", "Site Identification (LCEN).");
			v.Should().Contain("№ 2004-575", "la loi LCEN citée (article 6).");
			foreach (var pin in new[]
			{
				"ARGUMENTUM SAS, 5 B RUE MARTEL, 75010 PARIS 10",
				"SIREN: 922020276",
				"MY INTELLIGENCE AGENCY, 5 B RUE MARTEL, 75010 PARIS 10",
				"https://www.legifrance.gouv.fr/jorf/id/JORFTEXT000000801164",
			})
				Regex.Matches(v, Regex.Escape(pin)).Count.Should().Be(1, "identité légale verbatim : {0}.", pin);
			v.Should().Contain("<span class=\"SubHead\">СОГЛАШЕНИЕ МЕЖДУ ПОЛЬЗОВАТЕЛЕМ И [Portal:PortalName]</span>",
				"le corps R7 reste la traduction de référence (boilerplate 9.6 == 10.3.2, mesuré).");
			v.Should().EndWith("</div>", "structure HTML du bloc préservée.");
			v.Should().NotContain("LEGAL NOTICE", "la tête est localisée, pas bilingue.");
			v.Should().NotContain("SITE IDENTIFICATION", "idem — aucun résidu anglais de la tête neutre.");
		}
	}
}