using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 — pool po-2024, grain « resynchronisation es-ES / ru-RU de
	/// <c>AdminResetHelp</c> » (queue : « resynchronisation es-ES/ru-RU de
	/// <c>AdminResetHelp</c>/<c>QAHelp</c> »).
	///
	/// <para><b>Constat mesuré le 03/11.</b> Le neutre
	/// (<c>Password.ascx.resx</c>, figé depuis l'upload initial) décrit un
	/// <b>bouton + e-mail + lien valable 24 h</b>. pt-BR / fr-FR / ar-SA /
	/// fa-IR / zh-CN sont synchrones. es-ES et ru-RU décrivaient une
	/// <b>génération automatique du mot de passe</b> (« La nueva contraseña se
	/// creará automáticamente. », « Пароль будет сгенерирован случайным
	/// образом. ») — sens périmé, nommé depuis la PR pt-BR du fichier
	/// (voir <c>DnnPasswordPtBrGuardTests</c>, commentaire de classe) et
	/// laissé hors périmètre à l'époque (« un fichier par PR »).</para>
	///
	/// <para><b>Périmètre de CE grain.</b> <c>AdminResetHelp.Text</c> est la
	/// seule clé réellement désynchronisée : <c>QAHelp.Text</c> ru-RU porte
	/// déjà le bon sens (« Вы можете обновить ваш вопрос и ответ… Введите ваш
	/// текущий пароль… ») — mesuré. La garde épingne donc les DEUX clés,
	/// pour verrouiller l'état synchronisé des deux côtés.</para>
	///
	/// <para><b>Contrôle inverse</b> : la garde est écrite AVANT de relire le
	/// résultat, et elle a rendu rouge sur l'arbre d'avant chirurgie (les deux
	/// cellules périmées en preuve), verte après. Le rouge-avant n'est pas une
	/// affirmation de cette note, c'est la sortie de l'instrument.</para>
	/// </summary>
	public class DnnAdminResetHelpResyncGuardTests
	{
		private const string NeutralResx = "DNNPlatform/DesktopModules/Admin/Security/App_LocalResources/Password.ascx.resx";
		private const string EsResx      = "DNNPlatform/DesktopModules/Admin/Security/App_LocalResources/Password.ascx.es-ES.resx";
		private const string RuResx      = "DNNPlatform/DesktopModules/Admin/Security/App_LocalResources/Password.ascx.ru-RU.resx";

		private static Dictionary<string, string> Cells(string resxRelativePath)
		{
			var path = Path.Combine(TestRepoRoot.Find(), resxRelativePath.Replace('/', Path.DirectorySeparatorChar));
			File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({resxRelativePath})");
			return XDocument.Load(path).Root!.Elements("data")
				.ToDictionary(d => d.Attribute("name")!.Value, d => d.Element("value")?.Value ?? "");
		}

		/// <summary>Le neutre est le référent : il décrit le bouton + e-mail +
		/// lien 24 h. Ce test échoue si le neutre venait à changer de sens —
		/// la garde se ré-ancrerait alors sur le nouveau référent.</summary>
		[Fact]
		public void Neutral_Still_Describes_The_Reset_Button_And_24h_Link()
		{
			var v = Cells(NeutralResx)["AdminResetHelp.Text"];
			v.Should().Be(
				"Press the Reset Password button to send a password reset mail to the user. The reset link will be valid for 24hrs.",
				"le neutre est le référent du resync : bouton + mail + lien 24 h");
		}

		/// <summary>es-ES dit désormais le bouton + l'e-mail + le lien 24 h —
		/// et NE dit plus « se creará automáticamente ».</summary>
		[Fact]
		public void Es_AdminResetHelp_Is_Resynced()
		{
			var v = Cells(EsResx)["AdminResetHelp.Text"];
			v.Should().Be(
				"Pulse el botón de restablecimiento de contraseña para enviar al usuario un correo con un enlace para restablecer la contraseña. El enlace será válido durante 24 horas.",
				"AdminResetHelp es-ES resynchronisée sur le neutre (bouton + mail + lien 24 h)");
			v.Should().NotContain("automáticamente", "le sens périmé « auto-généré » est retiré");
		}

		/// <summary>ru-RU dit désormais le bouton + l'e-mail + le lien 24 h —
		/// et NE dit plus « сгенерирован случайным образом ».</summary>
		[Fact]
		public void Ru_AdminResetHelp_Is_Resynced()
		{
			var v = Cells(RuResx)["AdminResetHelp.Text"];
			v.Should().Be(
				"Нажмите кнопку сброса пароля, чтобы отправить пользователю письмо со ссылкой для сброса пароля. Ссылка будет действительна в течение 24 часов.",
				"AdminResetHelp ru-RU resynchronisée sur le neutre (bouton + mail + lien 24 h)");
			v.Should().NotContain("сгенерирован", "le sens périmé « auto-généré » est retiré");
		}

		/// <summary>QAHelp ru-RU portait déjà le bon sens — épinglé pour que
		/// la clé reste synchronisée (et pour que la garde ne se contente pas
		/// de la seule clé corrigée).</summary>
		[Fact]
		public void Ru_QaHelp_Was_Already_In_Sync_And_Stays_So()
		{
			var v = Cells(RuResx)["QAHelp.Text"];
			v.Should().Contain("текущий пароль", "QAHelp ru-RU parle du mot de passe actuel (bon sens)");
			v.Should().Contain("обновить", "QAHelp ru-RU parle de mise à jour (bon sens)");
		}

		/// <summary>QAHelp es-ES est plus télégraphique que le neutre mais dit
		/// le bon acte (mettre à jour la question/réponse) — épinglée à sa
		/// valeur courante pour verrouiller l'état.</summary>
		[Fact]
		public void Es_QaHelp_Stays_So()
		{
			Cells(EsResx)["QAHelp.Text"].Should().Be(
				"Indique su nueva pregunta/respuesta secretas",
				"QAHelp es-ES : valeur télégraphique mais au bon sens, épinglée");
		}
	}
}
