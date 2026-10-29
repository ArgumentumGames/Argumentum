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
	/// Garde du sweep pt-BR (pool #458 c.6436567243 item 8 : « sweep
	/// hispanismes pt-BR hors D4 », ordre = ce qu'un visiteur voit d'abord),
	/// fichier 3/5 : <c>DesktopModules/Admin/Security/App_LocalResources/ManageUsers.ascx.pt-BR.resx</c>
	/// — la page « comptes du site » de l'administration (lister, créer,
	/// autoriser, gérer un utilisateur).
	/// <para>
	/// Mesuré avant le sweep : 25 cellules sur 51 portaient un marqueur fort,
	/// dont 8 byte-égales à l'es-ES — la valeur y était espagnole franche
	/// (« Gestionar perfil », « Gestionar roles para este usuario »,
	/// « Registro de usuarios ») ou macaronique (« contrasenha »,
	/// « Introduzca a nova contrasenha », « foi sido deshabilitada »,
	/// « su suscripção », « propriedadees »). Retraduites en pt-BR, registre
	/// você, terminologie de la série D4 (#2022/#2029/#2030) et des fichiers
	/// 1/5 (#2059) et 2/5 (#2062) : senha, usuário, endereço de e-mail,
	/// função (role), redefinir senha, login.
	/// </para>
	/// <para>
	/// Terminologie « role » → « função » et « login » → « login » : lues dans
	/// le corpus pt-BR déjà livré (« Esta função já foi atribuída a este
	/// usuário. », « &lt; Todas as funções &gt; », « Prosseguir para o login »,
	/// « Falha no login »), jamais choisies par goût.
	/// </para>
	/// </summary>
	public class DnnManageUsersPtBrGuardTests
	{
		private const string PtResx = "DNNPlatform/DesktopModules/Admin/Security/App_LocalResources/ManageUsers.ascx.pt-BR.resx";
		private const string EsResx = "DNNPlatform/DesktopModules/Admin/Security/App_LocalResources/ManageUsers.ascx.es-ES.resx";
		private const string NeutralResx = "DNNPlatform/DesktopModules/Admin/Security/App_LocalResources/ManageUsers.ascx.resx";

		private static XDocument Load(string resxRelativePath)
		{
			var path = Path.Combine(TestRepoRoot.Find(), resxRelativePath.Replace('/', Path.DirectorySeparatorChar));
			File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({resxRelativePath})");
			return XDocument.Load(path);
		}

		private static Dictionary<string, string> Cells(string resxRelativePath) =>
			Load(resxRelativePath).Root!.Elements("data")
				.ToDictionary(d => d.Attribute("name")!.Value, d => d.Element("value")?.Value ?? "");

		private static string ResxValue(string resxRelativePath, string key) => Cells(resxRelativePath)[key];

		// ------------------------------------------------------------------
		// Grille de marqueurs espagnols — formes qui ne sont PAS du portugais
		// valide. Chaque entrée est auto-contrôlée : elle doit toucher au moins
		// une cellule de l'es-ES, sinon l'instrument lit 0 sur un zéro dont il
		// n'aurait pas su voir un 1.
		// Exclusions volontaires (valides dans les deux langues, un marqueur
		// naïf en ferait un faux rouge) : perfil, conta, esta, este, gratuito,
		// senha, função, registrar.
		// ------------------------------------------------------------------
		private static readonly Regex[] SpanishMarkers =
		{
			new(@"\w*ñ\w*"),
			new(@"\busuarios?\b", RegexOptions.IgnoreCase),          // es sans accent ; pt « usuário(s) »
			new(@"\bcuenta\b", RegexOptions.IgnoreCase),            // pt « conta »
			new(@"\bsitio\b", RegexOptions.IgnoreCase),             // pt « site »
			new(@"\bGestionar\b", RegexOptions.IgnoreCase),         // pt « Gerenciar »
			new(@"\brol\b|\broles\b", RegexOptions.IgnoreCase),     // pt « função/funções »
			new(@"\bpropied\w*", RegexOptions.IgnoreCase),          // pt « propriedades »
			new(@"\bintroduzc\w*", RegexOptions.IgnoreCase),        // pt « informe/introduza »
			new(@"\bintroducid[oa]s?\b", RegexOptions.IgnoreCase),  // pt « informado »
			new(@"\bUd\b"),
			new(@"\bdebe\b", RegexOptions.IgnoreCase),              // pt « deve »
			new(@"\bpuede\b|\bpueden\b", RegexOptions.IgnoreCase),  // pt « pode/podem »
			new(@"\btiene\b", RegexOptions.IgnoreCase),             // pt « tem »
			new(@"\bdemasiados\b", RegexOptions.IgnoreCase),
			new(@"\bintentos\b", RegexOptions.IgnoreCase),
			new(@"\bfallidos\b", RegexOptions.IgnoreCase),
			new(@"\bexceder[íi]a\b", RegexOptions.IgnoreCase),
			new(@"\bdeshabilitad\w*", RegexOptions.IgnoreCase),     // pt « desativado »
			new(@"\brequiere\b", RegexOptions.IgnoreCase),          // pt « requer » (grep « quiere » nu voyait « requiere »)
			new(@"\bdespués\b", RegexOptions.IgnoreCase),           // pt « depois »
			new(@"\bhasta\b", RegexOptions.IgnoreCase),             // pt « até »
			new(@"\bdel\b", RegexOptions.IgnoreCase),
			new(@"\bel\b"),
			new(@"\bla\b"),
			new(@"\blos\b|\blas\b", RegexOptions.IgnoreCase),
			new(@"\bsu\b", RegexOptions.IgnoreCase),                // possessif es ; pt « seu/sua »
			new(@"\bsus\b", RegexOptions.IgnoreCase),
			new(@"\w*ción\b", RegexOptions.IgnoreCase),             // pt « -ção »
			new(@"\bactualiz\w*", RegexOptions.IgnoreCase),         // pt « atualiz- »
			new(@"\bseguridad\b", RegexOptions.IgnoreCase),         // pt « segurança »
		};

		[Fact]
		public void Keys_Match_The_Neutral_File()
		{
			var pt = Cells(PtResx).Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			var ne = Cells(NeutralResx).Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			pt.Should().Equal(ne, "le fichier pt-BR doit porter exactement les clés du neutre (aucune ajoutée, aucune retirée)");
			pt.Should().HaveCount(51);
		}

		/// <summary>Auto-contrôle de l'instrument : chaque marqueur doit toucher
		/// au moins une cellule de l'es-ES. Un marqueur que l'es-ES lui-même ne
		/// porte pas ne prouve rien sur le pt-BR.</summary>
		[Fact]
		public void Marker_Grid_Is_Non_Vacuous_On_Es()
		{
			var esCells = Cells(EsResx).Values.ToArray();
			var blind = SpanishMarkers
				.Where(m => !esCells.Any(v => m.IsMatch(v)))
				.Select(m => m.ToString())
				.ToArray();
			blind.Should().BeEmpty("chaque marqueur doit être vu par l'es-ES (sinon son 0 sur le pt-BR est aveugle)");
		}

		/// <summary>Invariant de FICHIER ENTIER (et non d'un sous-ensemble de
		/// clés) : les 51 cellules sont traduites, donc aucun marqueur espagnol
		/// ne peut subsister nulle part.</summary>
		[Fact]
		public void Every_Cell_Carries_No_Spanish_Marker()
		{
			var offenders = new List<string>();
			foreach (var (key, value) in Cells(PtResx))
			foreach (var marker in SpanishMarkers)
				if (marker.IsMatch(value))
					offenders.Add($"{key} [{marker}] :: {value}");
			offenders.Should().BeEmpty(
				"les 51 cellules pt-BR de la page « comptes du site » sont en portugais — " +
				"tout marqueur restant est une réinjection : {0}", string.Join(" | ", offenders));
		}

		/// <summary>Contrôle falsifiant par littéraux : les formes macaroniques
		/// pt-spécifiques de l'ancienne valeur (hybrides es/pt que l'es-ES ne
		/// porte pas tels quels, donc invisibles à la grille) sont nommées ici —
		/// présence prouvée dans le littéral historique, absence exigée du
		/// fichier courant.</summary>
		[Fact]
		public void Historical_Macaronic_Forms_Are_Gone()
		{
			var historical = new Dictionary<string, string>
			{
				["contrasenha"] = "Gestionar contrasenha",
				["Contrasenha"] = "Contrasenha",
				["propriedadees"] = "Gestionar propriedadees do perfil",
				["suscripção"] = "Foi cancelado su suscripção ao rol {0}.",
				["foi sido deshabilitada"] = "pelo que esta opção foi sido deshabilitada para este sitio",
				["se encontra bloqueada"] = "Esta conta se encontra bloqueada",
				["Introduzca"] = "Introduzca a nova contrasenha",
				["seguridade"] = "Escriba o código de seguridade",
				["inicio de sesión"] = "Proceder ao inicio de sesión",
				["em o seguinte"] = "em o seguinte inicio de sesión",
				["corretamente"] = "Usuario autorizado corretamente",
				["actualizado como"] = "O usuário foi actualizado como",
				["Acerca do módulo"] = "Acerca do módulo de Contas do usuário",
				["se registren"] = "permite que os usuarios se registren",
			};
			var current = string.Join("\n", Cells(PtResx).Values);
			foreach (var (form, witness) in historical)
			{
				witness.Should().Contain(form, "témoin littéral : la forme historique doit être présente dans le littéral");
				current.Should().NotContain(form,
					$"« {form} » est une forme macaronique retirée — sa réapparition est une régression");
			}
		}

		private static readonly Dictionary<string, string> Pins = new()
		{
			["AccountSettings.Text"] = "Configurações da conta",
			["AddContent.Action"] = "Adicionar novo usuário",
			["AddUser.Text"] = "Adicionar novo usuário",
			["AddUser.Title"] = "Adicionar novo usuário",
			["CannotAddUser.Text"] = "Este site está configurado para exigir que os usuários informem uma pergunta e uma resposta para receber lembretes de senha. Esta configuração é incompatível com a adição de usuários por administradores e, por isso, foi desativada neste site.",
			["cmdLogin.Text"] = "Prosseguir para o login",
			["cmdPassword.Text"] = "Gerenciar senha",
			["cmdProfile.Text"] = "Gerenciar perfil",
			["cmdRegister.Text"] = "Registrar",
			["cmdRoles.Text"] = "Gerenciar funções deste usuário",
			["cmdServices.Text"] = "Gerenciar serviços",
			["cmdUpdatePassword.Text"] = "Atualizar senha",
			["cmdUser.Text"] = "Gerenciar conta",
			["ControlTitle_edit.Text"] = "Editar contas de usuário",
			["ControlTitle_profile.Text"] = "Gerenciar perfil",
			["ControlTitle_register.Text"] = "Registrar",
			["EmailError.Text"] = "Este site exige um endereço de e-mail único. O endereço de e-mail informado já está em uso.",
			["ExceededRegisterQuota.Text"] = "Não é possível registrar-se neste momento neste site. Entre em contato com o administrador do site para saber quando o registro será reaberto.",
			["ExceededUserQuota.Text"] = "Adicionar este usuário excederá a cota de usuários deste site. Entre em contato com o seu provedor de hospedagem para consultas relacionadas ao aumento da cota de usuários.",
			["InvalidUser.Text"] = "Esta conta não é de um usuário deste site.",
			["ManageProfile.Action"] = "Gerenciar propriedades do perfil",
			["MembershipDetails.Text"] = "Informações da conta",
			["MembershipTitle.Text"] = "Informações de associação",
			["ModuleHelp.Text"] = "<h1>Sobre o módulo de gerenciamento de usuários</h1><p>Os administradores podem gerenciar os usuários registrados. Este módulo permite adicionar novos usuários, modificar usuários existentes, excluir usuários e gerenciar as funções de segurança dos usuários.</p>",
			["NewPassword.Text"] = "Você deve informar uma nova senha.",
			["NotAuthorized.Text"] = "Você não tem autorização para editar este usuário.",
			["NoUser.Text"] = "Esta conta não existe.",
			["Password.Action"] = "Gerenciar a senha do usuário",
			["PasswordQAChanged.Text"] = "Pergunta e resposta de senha atualizadas com sucesso.",
			["plCaptcha.Help"] = "Informe o código de segurança.",
			["plCaptcha.Text"] = "Código de segurança:",
			["plNewConfirm.Help"] = "Informe novamente a nova senha para confirmar.",
			["plNewConfirm.Text"] = "Confirmar nova senha:",
			["plNewPassword.Help"] = "Informe a nova senha.",
			["plNewPassword.Text"] = "Nova senha:",
			["Profile.Action"] = "Gerenciar o perfil do usuário",
			["Register.Text"] = "Registro de usuário",
			["Register.Title"] = "Registro de usuário",
			["RegisterWarning.Text"] = " - <em>(<strong>Atenção:</strong> O registro pode levar alguns segundos. Depois de clicar no botão Registrar, aguarde a resposta do sistema.)</em>",
			["ResetPassword.Text"] = "Redefinir senha",
			["Roles.Action"] = "Gerenciar funções deste usuário",
			["SuperUser.Text"] = "Esta conta pertence a um superusuário. Você não tem permissão para editar contas de superusuário.",
			["UserAuthorized.Text"] = "Usuário autorizado com sucesso.",
			["UserDemotedFromSuperuser.Text"] = "O usuário foi atualizado para um usuário comum.",
			["UserLockedOut.Text"] = "Esta conta está bloqueada após muitas tentativas de login malsucedidas.",
			["UserPasswordUpdateChanged.Text"] = "O usuário deve atualizar a senha no próximo login.",
			["UserPromotedToSuperuser.Text"] = "O usuário foi atualizado para superusuário.",
			["UserSubscribed.Text"] = "Você foi inscrito com sucesso na função {0}.",
			["UserTitle.Text"] = "Editar usuário - {0} (Id: {1})",
			["UserUnAuthorized.Text"] = "Usuário desautorizado com sucesso.",
			["UserUnSubscribed.Text"] = "Você cancelou com sucesso a inscrição na função {0}.",
		};

		[Fact]
		public void Pins_Exact_Values()
		{
			Pins.Should().HaveCount(51, "les 51 clés sont épinglées : la garde marque-taux et l'épingle couvrent le même périmètre");
			foreach (var (key, expected) in Pins)
				ResxValue(PtResx, key).Should().Be(expected, "pin du sweep pt-BR — clé {0}.", key);
		}

		/// <summary>Parité des placeholders exigée sur CHAQUE clé, dans les deux
		/// sens — le titre porte {0} et {1}, les deux messages d'abonnement {0}.</summary>
		[Fact]
		public void Placeholder_Parity_With_Neutral()
		{
			var pt = Cells(PtResx);
			var ne = Cells(NeutralResx);
			var token = new Regex(@"\{\d+\}");
			foreach (var key in ne.Keys)
			{
				var expected = token.Matches(ne[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal).ToArray();
				var actual = token.Matches(pt[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal).ToArray();
				actual.Should().Equal(expected, "placeholders de {0} : le neutre porte {1}", key, string.Join(",", expected));
			}
			ResxValue(PtResx, "UserTitle.Text").Should().Contain("{0}").And.Contain("{1}",
				"le titre interpole le nom d'utilisateur ({0}) et l'identifiant ({1})");
		}

		/// <summary>Le squelette de balises des deux cellules qui portent du HTML
		/// (ModuleHelp, RegisterWarning) est identique au neutre — une
		/// traduction ne doit ni perdre ni ajouter de balise. Comparaison sur la
		/// valeur DÉCODÉE (le fichier porte les balises échappées).</summary>
		[Fact]
		public void Html_Skeleton_Matches_Neutral()
		{
			foreach (var key in new[] { "ModuleHelp.Text", "RegisterWarning.Text" })
			{
				var tags = new Func<string, string[]>(html =>
					Regex.Matches(html, @"</?[a-zA-Z][^>]*>").Select(m => m.Value.ToLowerInvariant()).ToArray());
				tags(ResxValue(PtResx, key)).Should().Equal(tags(ResxValue(NeutralResx, key)),
					"la séquence de balises de {0} doit être celle du neutre (traduction, pas réécriture)", key);
			}
		}

		/// <summary>Convention pt-BR : pas d'espace avant les deux-points.</summary>
		[Fact]
		public void No_Space_Before_Punctuation()
		{
			var spaceBefore = new Regex(@"\s+[:;]");
			var offenders = Cells(PtResx)
				.Where(c => spaceBefore.IsMatch(c.Value))
				.Select(c => $"{c.Key} :: {c.Value}")
				.ToArray();
			offenders.Should().BeEmpty("pt-BR : aucun espace avant « : » ni « ; » : {0}", string.Join(" | ", offenders));
		}

		/// <summary>Toute cellule dont le neutre parle de mot de passe dit
		/// « senha » (terminologie D4) — et la forme galicienne « contrasenha »
		/// n'existe nulle part. La casse initiale des libellés (« Nova senha: »)
		/// impose de tester sans tenir compte de la casse (leçon du fichier 2/5).</summary>
		[Fact]
		public void Password_Cells_Say_Senha_Never_Contrasenha()
		{
			var pt = Cells(PtResx);
			var ne = Cells(NeutralResx);
			var passwordCells = ne.Where(c => Regex.IsMatch(c.Value, "password", RegexOptions.IgnoreCase))
				.Select(c => c.Key).ToArray();
			passwordCells.Should().HaveCount(12, "12 cellules du neutre parlent de mot de passe — témoin du périmètre");
			foreach (var key in passwordCells)
				Regex.IsMatch(pt[key], "senha", RegexOptions.IgnoreCase).Should().BeTrue(
					"terminologie D4 : « senha », jamais « contrasenha » ({0} :: {1})", key, pt[key]);
			string.Join("\n", pt.Values).Should().NotContain("contrasenha");
		}

		/// <summary>Enregistreur « você » (norme BR #1879/#1890) : aucune forme
		/// de vouvoiement espagnol ne subsiste.</summary>
		[Fact]
		public void Voce_Register_No_Spanish_Form_Of_Address()
		{
			var joined = string.Join("\n", Cells(PtResx).Values);
			joined.Should().NotContain("Ud.");
			joined.Should().NotMatchRegex(@"\bsu\s+(conta|senha|perfil|suscrip)",
				"registre você : le possessif est « sua/seus », jamais « su »");
			joined.Should().Contain("Você", "les messages adressés à l'utilisateur emploient « Você »");
		}

		/// <summary>Avant le sweep, 8 cellules étaient byte-égales à l'es-ES
		/// (« Gestionar perfil », « Gestionar roles para este usuario »,
		/// « Gestionar servicios », « Registrar »…). Après, seules les deux
		/// formes « Registrar » restent — homographe pt/es légitime, comme
		/// « Cancelar » au fichier 1/5 : ce n'est pas une non-traduction.</summary>
		[Fact]
		public void No_Cell_Equals_The_Spanish_Cell_Except_Legitimate_Homographs()
		{
			var pt = Cells(PtResx);
			var es = Cells(EsResx);
			var legitimate = new[] { "cmdRegister.Text", "ControlTitle_register.Text" };
			var equal = pt.Where(c => es.TryGetValue(c.Key, out var v) && v == c.Value)
				.Select(c => c.Key).OrderBy(k => k, StringComparer.Ordinal).ToArray();
			equal.Should().Equal(legitimate.OrderBy(k => k, StringComparer.Ordinal).ToArray(),
				"seules les deux cellules « Registrar » sont légitimement identiques à l'es-ES (homographe) ; " +
				"toute autre égalité est une non-traduction");
			foreach (var key in legitimate)
				pt[key].Should().Be("Registrar", "témoin du seul homographe admis ({0})", key);
		}
	}
}
