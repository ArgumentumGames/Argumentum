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
	/// fichier 2/5 : <c>DesktopModules/Admin/Security/App_LocalResources/Password.ascx.pt-BR.resx</c>
	/// — la page « gérer le mot de passe » (champ disciplinaire de l'admin
	/// comme du compte lui-même). Les 37 cellules étaient macaroniques
	/// (« contrasenha », « introduzca », « Gestionar », « foi recibido »),
	/// dont 6 byte-égales à l'es-ES. Retraduites en pt-BR, registre você,
	/// terminologie posée par la série D4 (#2022 / #2029 / #2030) et le
	/// fichier 1/5 (#2059) : senha, usuário, e-mail, pergunta de senha,
	/// redefinir senha, código de redefinição.
	/// <para>
	/// Choix éditorial mesuré : AdminResetHelp et QAHelp sont traduites du
	/// NEUTRE (bouton + lien valable 24 h ; saisir mot de passe + question),
	/// corroboré par le fr-FR — l'es-ES (et le ru-RU pour la première) porte
	/// un sens périmé (« mot de passe auto-généré »). L'écart es/ru est
	/// déclaré dans la PR, hors périmètre de ce grain (un fichier par PR).
	/// </para>
	/// </summary>
	public class DnnPasswordPtBrGuardTests
	{
		private const string PtResx = "DNNPlatform/DesktopModules/Admin/Security/App_LocalResources/Password.ascx.pt-BR.resx";
		private const string EsResx = "DNNPlatform/DesktopModules/Admin/Security/App_LocalResources/Password.ascx.es-ES.resx";
		private const string NeutralResx = "DNNPlatform/DesktopModules/Admin/Security/App_LocalResources/Password.ascx.resx";

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
		// marqueur naïf ferait un faux rouge) : Indique, proporcione,
		// Guardar, gestionar, reiniciar, enlace, clicando, web, momento,
		// está, puede(s), breve, poder, verificar, elegir.
		// ------------------------------------------------------------------
		private static readonly Regex[] SpanishMarkers =
		{
			new(@"\w*ñ\w*"),
			new(@"\w*ción\b", RegexOptions.IgnoreCase),
			new(@"\bpregunta\b", RegexOptions.IgnoreCase),
			new(@"\brespuesta\b", RegexOptions.IgnoreCase),
			new(@"\bintroduzca\b", RegexOptions.IgnoreCase),
			new(@"\bintroducir\b", RegexOptions.IgnoreCase),
			new(@"\brecibirá\b", RegexOptions.IgnoreCase),
			new(@"\brecibido\b", RegexOptions.IgnoreCase),
			new(@"\bcambi\w*", RegexOptions.IgnoreCase),
			new(@"\bautomáticamente\b", RegexOptions.IgnoreCase),
			new(@"\bconozca\b", RegexOptions.IgnoreCase),
			new(@"\bUd\b"),
			new(@"\bcreará\b", RegexOptions.IgnoreCase),
			new(@"\brequire\b", RegexOptions.IgnoreCase),
			new(@"\bdisponible\b", RegexOptions.IgnoreCase),
			new(@"\bbotón\b", RegexOptions.IgnoreCase),
			new(@"\bcaduc\w*", RegexOptions.IgnoreCase),
			new(@"\bcorreo\b", RegexOptions.IgnoreCase),
			new(@"\belectrónico\b", RegexOptions.IgnoreCase),
			new(@"\bsitio\b", RegexOptions.IgnoreCase),
			new(@"\bva a\b", RegexOptions.IgnoreCase),
			new(@"\busuario\b", RegexOptions.IgnoreCase),   // es sans accent ; pt « usuário »
			new(@"\bnombre\b", RegexOptions.IgnoreCase),
			new(@"\bfecha\b", RegexOptions.IgnoreCase),
			new(@"\bpueden\b", RegexOptions.IgnoreCase),    // pt : « podem »
			new(@"\bse ha\b", RegexOptions.IgnoreCase),
			new(@"\bpor lo que\b", RegexOptions.IgnoreCase),
			new(@"\bel\b"),
			new(@"\bla\b"),
			new(@"\blos\b|\blas\b", RegexOptions.IgnoreCase),
			new(@"\bdel\b", RegexOptions.IgnoreCase),
			new(@"\bdesactivad[ao]s?\b", RegexOptions.IgnoreCase),   // pt : « desativado » (sans c)
			new(@"\brecordatorio\b", RegexOptions.IgnoreCase),
		};

		[Fact]
		public void Keys_Match_The_Neutral_File()
		{
			var pt = Cells(PtResx).Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			var ne = Cells(NeutralResx).Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			pt.Should().Equal(ne, "le fichier pt-BR doit porter exactement les clés du neutre (aucune ajoutée, aucune retirée)");
			pt.Should().HaveCount(37);
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
				"les 37 cellules pt-BR de la page « gérer le mot de passe » sont en portugais — " +
				"tout marqueur restant est une réinjection : {0}", string.Join(" | ", offenders));
		}

		/// <summary>Contrôle falsifiant par littéraux : les formes macaroniques
		/// pt-spécifiques de l'ancienne valeur (hybrides es/pt que l'es-ES ne
		/// porte pas telles quelles) sont nommées ici — présence prouvée dans
		/// le littéral historique, absence exigée du fichier courant.</summary>
		[Fact]
		public void Historical_Macaronic_Forms_Are_Gone()
		{
			var historical = new Dictionary<string, string>
			{
				["contrasenha"] = "Mudar contrasenha",
				["Gestionar contrasenha"] = "Gestionar contrasenha - {0} (Id: {1})",
				["foi recibido"] = "que foi recibido em su correo",
				["em este momento"] = "não está disponível em este momento",
				["se criará automáticamente"] = "A nova contrasenha se criará automáticamente.",
				["a continuação"] = "e a continuação a nova contrasenha e a confirmação",
				["somente Ud. conozca"] = "procure que somente Ud. conozca a respuesta",
				["em botón"] = "clicando em botón Reiniciar contrasenha",
				["enlace de reinicio de contrasenha"] = "Enviar enlace de reinicio de contrasenha",
				["reiniciar su contrasenha"] = "um enlace para reiniciar su contrasenha",
				["Pregunta secreta"] = "Pregunta secreta:",
				["Respuesta secreta"] = "Respuesta secreta:",
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
			["AdminChangeHelp.Text"] = "Para alterar a senha deste usuário, informe a nova senha e, em seguida, informe-a novamente para confirmar.",
			["AdminResetHelp.Text"] = "Pressione o botão de redefinição de senha para enviar ao usuário um e-mail com um link para redefinir a senha. O link será válido por 24 horas.",
			["CannotChangePassword.Text"] = "Sua configuração exige que o usuário informe uma pergunta e uma resposta para o lembrete de senha. Quando essa opção está ativada, os administradores não podem alterar as senhas dos usuários; por isso, esse recurso foi desativado neste site.",
			["ChangePassword.Text"] = "Alterar senha",
			["ChangeQA.Text"] = "Editar pergunta e resposta",
			["ForcedExpiry.Text"] = "O administrador do site determinou que você altere sua senha antes de entrar. Para isso, sua conta foi marcada para exigir uma nova senha, o que envia um e-mail com um código de redefinição de senha. Verifique seu e-mail e use o link para redefinir sua senha. Se não encontrar o e-mail, pressione Enviar link de redefinição de senha para solicitar outro.",
			["NoExpiry.Text"] = "A senha não expira",
			["OptionUnavailable.Text"] = "A opção de redefinição de senha não está disponível neste momento.",
			["PasswordSent.Text"] = "Se o nome de usuário informado estiver correto, você receberá em breve um e-mail com um link para redefinir sua senha.",
			["PasswordTitle.Text"] = "Gerenciar senha - {0} (Id: {1})",
			["plAnswer.Help"] = "Informe a resposta à pergunta de senha.",
			["plAnswer.Text"] = "Resposta de senha:",
			["plEditAnswer.Help"] = "Informe a resposta à sua pergunta de senha.",
			["plEditAnswer.Text"] = "Resposta de senha:",
			["plEditQuestion.Help"] = "Informe a pergunta de senha que você deseja usar. Escolha uma pergunta cuja resposta somente você conheça.",
			["plEditQuestion.Text"] = "Pergunta de senha:",
			["plExpires.Help"] = "A data de expiração desta senha.",
			["plExpires.Text"] = "Expiração de senha:",
			["plLastChanged.Help"] = "A data da última alteração desta senha.",
			["plLastChanged.Text"] = "Última alteração de senha:",
			["plNewConfirm.Help"] = "Informe novamente a nova senha para confirmar.",
			["plNewConfirm.Text"] = "Confirmar senha:",
			["plNewPassword.Help"] = "Informe a nova senha.",
			["plNewPassword.Text"] = "Nova senha:",
			["plOldPassword.Help"] = "Informe sua senha atual.",
			["plOldPassword.Text"] = "Senha atual:",
			["plQAPassword.Help"] = "Informe sua senha atual.",
			["plQAPassword.Text"] = "Senha:",
			["plQuestion.Help"] = "Informe uma pergunta a ser usada para recuperar sua senha.",
			["plQuestion.Text"] = "Pergunta de senha:",
			["plResetToken.Help"] = "Informe o código de redefinição que você recebeu em seu e-mail.",
			["plResetToken.Text"] = "Informe o código de redefinição",
			["QAHelp.Text"] = "Você pode atualizar sua pergunta e resposta de senha informando sua senha atual, além da pergunta e da resposta que deseja usar.",
			["ResetPassword.Text"] = "Enviar link de redefinição de senha",
			["SaveQA.Text"] = "Salvar pergunta e resposta de senha",
			["UserChangeHelp.Text"] = "Para alterar sua senha, você deve informar sua senha atual, além da nova senha e da confirmação da nova senha.",
			["UserResetHelp.Text"] = "Você pode redefinir sua senha para uma nova senha gerada aleatoriamente ao informar a resposta à pergunta de senha a seguir.",
		};

		[Fact]
		public void Pins_Exact_Values()
		{
			Pins.Should().HaveCount(37);
			foreach (var (key, expected) in Pins)
				ResxValue(PtResx, key).Should().Be(expected, "pin du sweep pt-BR — clé {0}.", key);
		}

		/// <summary>Parité des placeholders exigée sur CHAQUE clé, dans les
		/// deux sens — le titre porte {0} et {1}, aucune autre cellule n'en
		/// porte (fichier 1/5 : un {0} perdu avait déjà été constaté sur la
		/// surface voisine, la parité est la garde générique de cette famille).</summary>
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
			ResxValue(PtResx, "PasswordTitle.Text").Should().Contain("{0}").And.Contain("{1}",
				"le titre interpole le nom d'utilisateur ({0}) et l'identifiant ({1})");
		}

		/// <summary>Convention pt-BR : pas d'espace avant les deux-points
		/// (contrairement au fr) — les anciennes valeurs copiaient la
		/// typographie de la source.</summary>
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
		/// « senha » (terminologie D4) — et la forme galicienne
		/// « contrasenha » n'existe nulle part.</summary>
		[Fact]
		public void Password_Cells_Say_Senha_Never_Contrasenha()
		{
			var pt = Cells(PtResx);
			var ne = Cells(NeutralResx);
			var passwordCells = ne.Where(c => Regex.IsMatch(c.Value, "password", RegexOptions.IgnoreCase)).Select(c => c.Key).ToArray();
			passwordCells.Should().HaveCount(34, "34 cellules du neutre parlent de mot de passe — témoin du périmètre");
			foreach (var key in passwordCells)
				Regex.IsMatch(pt[key], "senha", RegexOptions.IgnoreCase).Should().BeTrue(
					"terminologie D4 : « senha » (la casse initiale des libellés « Senha: » compte), jamais « contrasenha » ({0})", key);
			string.Join("\n", pt.Values).Should().NotContain("contrasenha");
		}

		/// <summary>Toute cellule dont le neutre parle de « reset » dit
		/// « redefin... » (redefinir / redefinição) — jamais le calque
		/// « reiniciar » (valide en pt pour « redémarrer », donc hors grille
		/// d'espagnismes, mais faux ici).</summary>
		[Fact]
		public void Reset_Cells_Say_Redefin()
		{
			var pt = Cells(PtResx);
			var ne = Cells(NeutralResx);
			var resetCells = ne.Where(c => Regex.IsMatch(c.Value, "reset", RegexOptions.IgnoreCase)).Select(c => c.Key).ToArray();
			resetCells.Should().HaveCount(8, "8 cellules du neutre parlent de reset — témoin du périmètre");
			foreach (var key in resetCells)
				pt[key].Should().Contain("redefin", "terminologie D4/2059 : « redefinir senha », « código de redefinição » ({0})", key);
		}

		/// <summary>Avant le sweep, 6 cellules étaient byte-égales à l'es-ES
		/// (Pregunta secreta, Respuesta secreta, ChangeQA, SaveQA…) : des
		/// non-traductions pures. Après, AUCUNE égalité n'est légitime — il
		/// n'existe pas ici d'homographe es/pt comme « Cancelar » au 1/5.</summary>
		[Fact]
		public void No_Cell_Equals_The_Spanish_Cell()
		{
			var pt = Cells(PtResx);
			var es = Cells(EsResx);
			var equal = pt.Where(c => es.TryGetValue(c.Key, out var v) && v == c.Value)
				.Select(c => c.Key).OrderBy(k => k, StringComparer.Ordinal).ToArray();
			equal.Should().BeEmpty(
				"aucune cellule pt-BR ne doit rester byte-égale à l'es-ES (les 6 égalités d'avant le sweep sont des non-traductions) : {0}",
				string.Join(" | ", equal));
		}
	}
}
