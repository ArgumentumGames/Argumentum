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
	/// fichier 1/5 : <c>admin/Security/App_LocalResources/SendPassword.ascx.pt-BR.resx</c>.
	/// Le formulaire « mot de passe oublié » — visité par un utilisateur qui
	/// ne peut plus se connecter — portait 33 cellules sur 35 en espagnol ou
	/// macaroniques (« contrasenha », « Cerrar », « Entre su código »,
	/// « Vuelva a intentarlo », « Sin embargo »), dont 3 byte-égales à l'es-ES.
	/// Retraduites en pt-BR, registre você, terminologie posée par la série D4
	/// (#2022 / #2029 / #2030) : senha, usuário, endereço de e-mail, código de
	/// verificação, redefinir senha.
	/// <para>
	/// Deux cellules étaient déjà saines et sont conservées : <c>plUsername.Text</c>
	/// (« Nome do usuário: ») et <c>hlLogin.Text</c> (« Cancelar »).
	/// </para>
	/// <para>
	/// Défaut trouvé en chemin, indépendant de la langue : la valeur pt de
	/// <c>PasswordSent.Text</c> avait PERDU le placeholder <c>{0}</c> que porte
	/// le neutre — l'adresse en cours de récupération n'était donc plus
	/// interpolée. Restauré, et épinglé par la parité de placeholders.
	/// </para>
	/// </summary>
	public class DnnSendPasswordPtBrGuardTests
	{
		private const string PtResx = "DNNPlatform/admin/Security/App_LocalResources/SendPassword.ascx.pt-BR.resx";
		private const string EsResx = "DNNPlatform/admin/Security/App_LocalResources/SendPassword.ascx.es-ES.resx";
		private const string NeutralResx = "DNNPlatform/admin/Security/App_LocalResources/SendPassword.ascx.resx";

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
		// Grille de marqueurs espagnols FORTS — formes qui ne sont PAS du
		// portugais valide. Chaque entrée est auto-contrôlée : elle doit
		// toucher au moins une cellule de l'es-ES, sinon l'instrument lit 0
		// sur un zéro dont il n'aurait pas su voir un 1.
		// Voluntary exclusions (mots valides dans les deux langues, dont un
		// marqueur naïf ferait un faux rouge) : está, con, que, se, Este,
		// durante, Reiniciar, visualizado, Clicando, página, aleatoriamente,
		// bloqueada, "antes de", "Proceder" (pt formel), "demasiados" (pt-PT).
		// ------------------------------------------------------------------
		private static readonly Regex[] SpanishMarkers =
		{
			new(@"\w*ñ\w*"),
			new(@"\w*ción\b", RegexOptions.IgnoreCase),
			new(@"contraseñ\w*", RegexOptions.IgnoreCase),
			new(@"\bseguridad\b", RegexOptions.IgnoreCase),
			new(@"\bverificación\b", RegexOptions.IgnoreCase),
			new(@"\binicio\b", RegexOptions.IgnoreCase),
			new(@"\bsesión\b", RegexOptions.IgnoreCase),
			new(@"\bCerrar\b"),
			new(@"\bRespuesta\b"),
			new(@"\brespuesta\b"),
			new(@"\bSin embargo\b"),
			new(@"\bcaja\b", RegexOptions.IgnoreCase),
			new(@"\bdirección\b", RegexOptions.IgnoreCase),
			new(@"\bactualmente\b", RegexOptions.IgnoreCase),
			new(@"\bdesactivad[ao]s?\b", RegexOptions.IgnoreCase),
			new(@"\brecordatorio\b", RegexOptions.IgnoreCase),
			new(@"\bdatos\b", RegexOptions.IgnoreCase),
			new(@"\bcorreo\b", RegexOptions.IgnoreCase),
			new(@"\benlace\b", RegexOptions.IgnoreCase),
			new(@"\bobtener\b", RegexOptions.IgnoreCase),
			new(@"\bmayoría\b", RegexOptions.IgnoreCase),
			new(@"\bmás\b"),
			new(@"\busuario\b", RegexOptions.IgnoreCase),   // es sans accent ; pt « usuário »
			new(@"\bPregunta\b", RegexOptions.IgnoreCase),
			new(@"\bRecuerde\b", RegexOptions.IgnoreCase),
			new(@"\bdebería\b", RegexOptions.IgnoreCase),
			new(@"\bpodrá\b", RegexOptions.IgnoreCase),
			new(@"\bdonde\b", RegexOptions.IgnoreCase),
			new(@"\bSi\b"),
			new(@"\bpuede\b", RegexOptions.IgnoreCase),
			new(@"\bel\b"),
			new(@"\bla\b"),
			new(@"\blos\b|\blas\b", RegexOptions.IgnoreCase),
			new(@"\bdel\b", RegexOptions.IgnoreCase),
			new(@"\bolvid\w*", RegexOptions.IgnoreCase),
			new(@"\bintentos?\b", RegexOptions.IgnoreCase),
			new(@"\bfallidos?\b", RegexOptions.IgnoreCase),
			new(@"\bdespués\b", RegexOptions.IgnoreCase),
			new(@"\bnueva\b|\bnuevo\b", RegexOptions.IgnoreCase),
			new(@"\bgenerada\b", RegexOptions.IgnoreCase),
			new(@"\bintroducidos\b", RegexOptions.IgnoreCase),
			new(@"\bcorrectos?\b", RegexOptions.IgnoreCase),
			new(@"\bincorrecto\b", RegexOptions.IgnoreCase),
			new(@"\bha sido\b", RegexOptions.IgnoreCase),
			new(@"\bPóngase\b", RegexOptions.IgnoreCase),
			new(@"\bcontactar con\b", RegexOptions.IgnoreCase),
			new(@"\bEspere\b", RegexOptions.IgnoreCase),
			new(@"\bIndique\b", RegexOptions.IgnoreCase),
			new(@"\bproporcion\w*", RegexOptions.IgnoreCase),
			new(@"\butilizó\b", RegexOptions.IgnoreCase),
			new(@"\bcrear\b", RegexOptions.IgnoreCase),
			new(@"\brecibir\b", RegexOptions.IgnoreCase),   // pt : « receber »
			new(@"\bvuelva\b", RegexOptions.IgnoreCase),
			new(@"\bintentarlo\b", RegexOptions.IgnoreCase),
			new(@"\bNombre\b", RegexOptions.IgnoreCase),
			new(@"\bsitio\b", RegexOptions.IgnoreCase),
			new(@"\bdetalles?\b", RegexOptions.IgnoreCase),
			new(@"\benvío\b", RegexOptions.IgnoreCase),
			new(@"(?<![-\w])email\b", RegexOptions.IgnoreCase),   // pt : « e-mail » (le lookbehind épargne le tiret)
		};

		[Fact]
		public void Keys_Match_The_Neutral_File()
		{
			var pt = Cells(PtResx).Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			var ne = Cells(NeutralResx).Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			pt.Should().Equal(ne, "le fichier pt-BR doit porter exactement les clés du neutre (aucune ajoutée, aucune retirée)");
			pt.Should().HaveCount(35);
		}

		/// <summary>Auto-contrôle de l'instrument : chaque marqueur doit
		/// toucher au moins une cellule de l'es-ES. Un marqueur que l'es-ES
		/// lui-même ne porte pas ne prouve rien sur le pt-BR.</summary>
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

		[Fact]
		public void Every_Cell_Carries_No_Spanish_Marker()
		{
			var offenders = new List<string>();
			foreach (var (key, value) in Cells(PtResx))
			foreach (var marker in SpanishMarkers)
				if (marker.IsMatch(value))
					offenders.Add($"{key} [{marker}] :: {value}");
			offenders.Should().BeEmpty(
				"les 35 cellules pt-BR du formulaire « mot de passe oublié » sont en portugais — " +
				"tout marqueur restant est une réinjection : {0}", string.Join(" | ", offenders));
		}

		/// <summary>Contrôle falsifiant par littéraux : les formes macaroniques
		/// pt-spécifiques de l'ancienne valeur (absentes de l'es-ES, donc hors
		/// grille auto-contrôlée) sont nommées ici — présence prouvée dans le
		/// littéral historique, absence exigée du fichier courant.</summary>
		[Fact]
		public void Historical_Macaronic_Forms_Are_Gone()
		{
			var historical = new Dictionary<string, string>
			{
				["contrasenha"] = "Enviar contrasenha",
				["foi sido"] = "Esta conta foi sido bloqueada",
				["olvidadeo"] = "Si foi olvidadeo su contrasenha",
				["devería"] = "devería recibir um novo email",
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
			// 33 cellules retraduites
			["CaptchaText.Text"] = "Informe o código exibido acima na caixa abaixo.",
			["cmdClose.Text"] = "Fechar",
			["cmdLogin.Text"] = "Prosseguir para o login",
			["ControlTitle_sendpassword.Text"] = "Redefinir senha",
			["DisabledPasswordHelp.Text"] = "A recuperação de senha está desativada neste site.",
			["EmailNotFound.Text"] = "Não encontramos nenhuma conta com esse endereço de e-mail. Tente novamente.",
			["EnterCode.Text"] = "Informe seu código de verificação.",
			["EnterUsername.Text"] = "Informe seu nome de usuário.",
			["EnterUsernameEmail.Text"] = "Informe seu nome de usuário ou o endereço de e-mail fornecido no cadastro.",
			["InvalidCaptcha.Text"] = "Código de segurança incorreto",
			["LoginFailed.Text"] = "Falha no login. Lembre-se de que as senhas diferenciam maiúsculas de minúsculas.",
			["MultipleUsers.Text"] = "Foi encontrado mais de um usuário com este endereço de e-mail. Informe seu nome de usuário e tente novamente.",
			["OptionUnavailable.Text"] = "A opção de redefinir senha não está disponível no momento.",
			["PasswordRetrievalDisabled.Text"] = "A recuperação de senha está desativada neste site. Entre em contato com o administrador para obter ajuda sobre os dados da sua conta.",
			["PasswordSent.Text"] = "Se os dados informados para {0} estiverem corretos, você receberá em breve uma mensagem de e-mail com um link para redefinir sua senha.",
			["plAnswer.Help"] = "Informe a resposta à sua pergunta de senha.",
			["plAnswer.Text"] = "Resposta",
			["plCaptcha.Help"] = "Informe o código de segurança.",
			["plCaptcha.Text"] = "Código de segurança",
			["plEmail.Help"] = "Informe o endereço de e-mail fornecido no cadastro para recuperar sua senha.",
			["plEmail.Text"] = "Endereço de e-mail:",
			["plQuestion.Help"] = "A pergunta de senha que você informou.",
			["plQuestion.Text"] = "Pergunta de senha:",
			["plUsername.Help"] = "Informe seu nome de usuário aqui. Na maioria dos casos, será seu endereço de e-mail.",
			["RequiresQuestionAndAnswer.Text"] = "<br/><br/> Você também deve informar a resposta à pergunta definida no cadastro.",
			["RequiresUniqueEmail.Text"] = "<br/><br/>Você pode solicitar o e-mail informando seu endereço de e-mail ou seu nome de usuário.",
			["ResetPassword.Text"] = "Redefinir senha",
			["ResetPasswordHelp.Text"] = "Este site não oferece recuperação de senha, mas uma nova senha aleatória pode ser gerada para você e enviada ao endereço de e-mail fornecido no cadastro.",
			["ResetToken.Text"] = "Enviar link de redefinição",
			["ResetTokenHelp.Text"] = "Se você esqueceu sua senha, um e-mail com um link de redefinição será enviado para você. Clique no link desse e-mail e você será levado a uma página onde poderá criar uma nova senha.",
			["SendPassword.Text"] = "Enviar senha",
			["SMTPNotConfigured.Text"] = "Falha ao enviar o e-mail. SMTP não configurado.",
			["UserLockedOut.Text"] = "Esta conta foi bloqueada após muitas tentativas de login malsucedidas. Aguarde 10 minutos antes de tentar entrar novamente. Se você esqueceu sua senha, tente a opção de lembrete de senha antes de entrar em contato com um administrador.",
			// 2 cellules déjà saines, conservées telles quelles
			["plUsername.Text"] = "Nome do usuário:",
			["hlLogin.Text"] = "Cancelar",
		};

		[Fact]
		public void Pins_Exact_Values()
		{
			Pins.Should().HaveCount(35);
			foreach (var (key, expected) in Pins)
				ResxValue(PtResx, key).Should().Be(expected, "pin du sweep pt-BR — clé {0}.", key);
		}

		/// <summary>Le placeholder {0} du neutre avait disparu de la valeur
		/// pt : l'adresse en cours de récupération n'était plus interpolée.
		/// Parité des placeholders exigée sur CHAQUE clé, dans les deux sens.</summary>
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
			ResxValue(PtResx, "PasswordSent.Text").Should().Contain("{0}",
				"le placeholder perdu de PasswordSent.Text est restauré (defect indépendant de la langue)");
		}

		/// <summary>Les deux cellules à fragment HTML conservent leur préfixe
		/// échappé, octet pour octet — la chirurgie ne touche que le texte.</summary>
		[Fact]
		public void Html_Fragments_Preserved()
		{
			ResxValue(PtResx, "RequiresQuestionAndAnswer.Text").Should().StartWith("<br/><br/> ");
			ResxValue(PtResx, "RequiresUniqueEmail.Text").Should().StartWith("<br/><br/>");
			var withTags = Cells(PtResx).Where(c => c.Value.Contains("<", StringComparison.Ordinal))
				.Select(c => c.Key).OrderBy(k => k, StringComparer.Ordinal).ToArray();
			withTags.Should().Equal(new[] { "RequiresQuestionAndAnswer.Text", "RequiresUniqueEmail.Text" },
				"seules ces deux cellules portent un fragment HTML (aucune balise ajoutée ailleurs)");
		}

		/// <summary>Convention pt-BR : pas d'espace avant les deux-points
		/// (contrairement au fr) — les anciennes valeurs copiaient parfois la
		/// typographie espagnole ou française.</summary>
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
		/// n'existe nulle part.</summary>
		[Fact]
		public void Password_Cells_Say_Senha_Never_Contrasenha()
		{
			var pt = Cells(PtResx);
			var ne = Cells(NeutralResx);
			var passwordCells = ne.Where(c => Regex.IsMatch(c.Value, "password", RegexOptions.IgnoreCase)).Select(c => c.Key).ToArray();
			passwordCells.Should().HaveCount(15, "15 cellules du neutre parlent de mot de passe — témoin du périmètre");
			foreach (var key in passwordCells)
				pt[key].Should().Contain("senha", "terminologie D4 : « senha », jamais « contrasenha » ({0})", key);
			string.Join("\n", pt.Values).Should().NotContain("contrasenha");
		}

		/// <summary>Une seule cellule peut rester byte-égale à l'es-ES :
		/// « Cancelar » est le même mot dans les deux langues. Les deux autres
		/// égalités d'avant le sweep (« Cerrar », « Respuesta ») sont parties.</summary>
		[Fact]
		public void Only_Cancelar_May_Equal_The_Spanish_Cell()
		{
			var pt = Cells(PtResx);
			var es = Cells(EsResx);
			var equal = pt.Where(c => es.TryGetValue(c.Key, out var v) && v == c.Value)
				.Select(c => c.Key).OrderBy(k => k, StringComparer.Ordinal).ToArray();
			equal.Should().Equal(new[] { "hlLogin.Text" },
				"seul « Cancelar » est légitimement identique en pt-BR et es-ES ; toute autre égalité est une non-traduction");
		}
	}
}
