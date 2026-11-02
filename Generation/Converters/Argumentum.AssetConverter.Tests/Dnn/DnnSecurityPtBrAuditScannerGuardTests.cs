using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 — pool po-2024, volet « Audit/Scanner » de <c>security.pt-BR.resx</c> :
	/// la famille des clés de vérification sécurité (Check*, Audit, Scanner, Modified*,
	/// en-têtes de grille) était un hybride espagnol/galicien de bout en bout —
	/// <c>foin encontrado</c>, <c>foiya</c>, <c>contrasenha</c>, <c>variable oculta
	/// donde</c>, <c>Auditoría</c>, <c>mejores prácticas</c>, <c>NOME PARÁMETRO</c>,
	/// <c>conexión…ejecutar</c>, <c>acceder…fuera da web</c>… Mesurée au 02/11 :
	/// 66 cellules réécrites depuis le neutre anglais, 10 clés créées
	/// (<c>CheckTelerikPresence*</c> ×6, contrôles de retrait Telerik de DNN 9.8/10.x ;
	/// <c>CheckUserProfilePage*</c> ×4), 8 cellules déjà correctes en pt laissées
	/// telles quelles.
	///
	/// <para>Hors périmètre, délibérément : les 4 clés mortes
	/// <c>CheckTelerikVulnerability*</c> (absentes du neutre, épinglées comme mortes
	/// par <c>DnnSecurityDeadKeysPtBrGuardTests</c>, #2118) — le balayage de marqueurs les
	/// EXCLUT ; et les 3 neutres structurellement vides (<c>ScannerExplanation.Text</c>,
	/// <c>ModifiedFilesExplaination.Text</c>, <c>ModifiedSettingsExplaination.Text</c>
	/// = <c>&lt;value /&gt;</c>) — source vide, rien à traduire, aucune clé pt créée.</para>
	/// </summary>
	public class DnnSecurityPtBrAuditScannerGuardTests
	{
		private static string PtResx => Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "DesktopModules", "Admin", "Dnn.PersonaBar", "Modules",
			"Dnn.Security", "App_LocalResources", "security.pt-BR.resx");

		private static string NeutralResx => Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "DesktopModules", "Admin", "Dnn.PersonaBar", "Modules",
			"Dnn.Security", "App_LocalResources", "Security.resx");

		/// <summary>Marqueurs espagnols/galiciens qui n'existent JAMAIS en pt-BR valide
		/// (chaque entrée vérifiée contre les 76 valeurs réécrites + 8 laissées :
		/// zéro faux positif — p. ex. « mais » pt ≠ « más » es, « carregar » pt
		/// ne contient pas « cargar », « auditoria » pt ≠ « auditoría » es, et
		/// « ativado » pt ne contient pas « activad » es/gl). « si » et « son » sont
		/// bornés par des espaces (« em si » pt théorique : aucun des 84 scans).</summary>
		private static readonly string[] SpanishMarkers =
		{
			"foin", "foiya", "contrasenha", "seguridade", "donde", "recientemente",
			"conexión", "ejecut", "acceder", "fuera", "parámetro", "auditoría",
			"mejores", "cabeceras", "enlaces", "biografía", "establec", "utilizan",
			"han ", "base de datos", "más", "están", "podría", "tienen",
			"uno o más", "sitio", "frecuente", "riesgo", "detalles", "cargar",
			"borrar", "comproba", " son ", "activad", " si ", "potenciales",
			"normales", "peligros",
		};

		/// <summary>Les 4 clés mortes du volet — la propriété de #2118, pas la nôtre.</summary>
		private static readonly string[] DeadKeys =
		{
			"CheckTelerikVulnerabilityFailure.Text", "CheckTelerikVulnerabilityName.Text",
			"CheckTelerikVulnerabilityReason.Text", "CheckTelerikVulnerabilitySuccess.Text",
		};

		private static readonly string[] FamilyExtras =
		{
			"AuditChecks.Text", "AuditExplanation.Text", "ScannerChecks.Text", "ScannerExplanation.Text",
			"SettingName.Text", "SettingValue.Text", "Filename.Header", "FileName.Text",
		};

		private static IEnumerable<string> FamilyKeys(XContainer root) => root.Elements("data")
			.Select(d => (string?)d.Attribute("name"))
			.Where(n => n != null
				&& (n.StartsWith("Check", StringComparison.Ordinal)
					|| n.StartsWith("Modified", StringComparison.Ordinal)
					|| FamilyExtras.Contains(n)))
			.Cast<string>();

		private static XDocument Load(string path) => XDocument.Load(path);

		[Fact]
		public void Volet_Is_Free_Of_Spanish_Contamination()
		{
			var pt = Load(PtResx).Root!;
			var offenders = pt.Elements("data")
				.Where(d => FamilyKeys(pt).Contains((string?)d.Attribute("name")))
				.Where(d => !DeadKeys.Contains((string?)d.Attribute("name"))) // #2118 owns these
				.Select(d => new
				{
					Key = (string?)d.Attribute("name"),
					Hits = SpanishMarkers
						.Where(m => ((string?)d.Element("value") ?? string.Empty)
							.Contains(m, StringComparison.OrdinalIgnoreCase))
						.ToList(),
				})
				.Where(x => x.Hits.Count > 0)
				.ToList();
			offenders.Should().BeEmpty(
				"le volet Audit/Scanner du pt-BR a été intégralement réécrit depuis le neutre le 02/11 : " +
				"un marqueur espagnol/galicien y indique une régression ou un déversement depuis es-ES");
		}

		[Fact]
		public void Sentinel_Cells_Are_Pinned()
		{
			var pt = Load(PtResx).Root!;
			Cell(pt, "Check.Text").Should().Be("[ Verificar ]");
			Cell(pt, "FileName.Text").Should().Be("NOME DO ARQUIVO",
				"la cellule disait « NOME Arquivo » — pt valide mais télégraphique : le balayage de marqueurs ne peut pas la voir, seule l'épingle la tient");
			Cell(pt, "CheckSqlRiskSuccess.Text").Should().Be("A conta do SQL Server está configurada corretamente.",
				"idem : l'original « Configuração da conta SQL Server correta. » était du pt valide mais agrammatical — hors portée du balayage, couvert par l'épingle");
			Cell(pt, "SettingName.Text").Should().Be("NOME DA CONFIGURAÇÃO",
				"l'en-tête disait « NOME PARÁMETRO » — parámetro est espagnol");
			Cell(pt, "ScannerChecks.Text").Should().Be("Pesquisar sistema de arquivos e banco de dados",
				"« base de datos » est espagnol (pt : banco de dados)");
			Cell(pt, "CheckViewstatemacReason.Text").Should().Be(
				"Um MAC de view state é uma versão criptografada da variável oculta na qual o estado de exibição da página é persistido quando ela é enviada ao navegador. Quando essa propriedade está definida como true, o view state criptografado é verificado para confirmar que não foi adulterado no cliente.",
				"la cellule éponyme du défaut (foiya, « variable oculta donde ») est le témoin du volet");
		}

		[Fact]
		public void The_Ten_Created_Keys_Are_Pinned_With_A_Live_Neutral()
		{
			var pt = Load(PtResx).Root!;
			var neutral = Load(NeutralResx).Root!;
			var created = new Dictionary<string, string>
			{
				["CheckTelerikPresenceFailure.Text"] = "O componente Telerik foi encontrado no site.",
				["CheckTelerikPresenceInstalledAndUsed.Text"] = "Dependências do Telerik foram descobertas em assemblies que NÃO serão tratadas pela desinstalação conforme as instruções da versão 9.8.0. A lista abaixo inclui todas:",
				["CheckTelerikPresenceInstalledButNotUsed.Text"] = "O Telerik foi encontrado neste site, mas nenhuma dependência do Telerik foi descoberta. Você pode razoavelmente considerar seguro desinstalar o Telerik conforme as instruções da versão 9.8.0.",
				["CheckTelerikPresenceName.Text"] = "Verificar se o Telerik está instalado.",
				["CheckTelerikPresenceReason.Text"] = "No DNN 10.x, a remoção do Telerik será forçada e não será mais opcional.",
				["CheckTelerikPresenceSuccess.Text"] = "O componente Telerik não está instalado.",
				["CheckUserProfilePageFailure.Text"] = "A página Perfil de Usuário é pública.",
				["CheckUserProfilePageName.Text"] = "Verificar o acesso à página Perfil de Usuário",
				["CheckUserProfilePageReason.Text"] = "A página Perfil de Usuário pode estar visível para todos os usuários de forma não intencional.",
				["CheckUserProfilePageSuccess.Text"] = "A página Perfil de Usuário não é pública.",
			};
			foreach (var (key, value) in created)
			{
				Cell(pt, key).Should().Be(value,
					"clé créée le 02/11 : absente du pt-BR alors que le neutre la porte depuis DNN 9.8/10.x");
			}

			// Sources vivantes : le neutre doit continuer de porter les 10 clés non vides
			// (épingle littérale sur les deux Name, existence+non-vide pour les autres).
			foreach (var key in created.Keys)
			{
				var cell = neutral.Elements("data")
					.FirstOrDefault(d => (string?)d.Attribute("name") == key)?.Element("value");
				((string?)cell).Should().NotBeNullOrEmpty(
					$"la clé {key} est vivante au neutre : c'est la source qui fonde la création pt-BR");
			}
			Cell(neutral, "CheckTelerikPresenceName.Text").Should().Be("Check if Telerik is installed.");
			Cell(neutral, "CheckUserProfilePageName.Text").Should().Be("Check access to the User Profile page");
		}

		[Fact]
		public void Family_Key_Inventory_Is_Exact()
		{
			var pt = Load(PtResx).Root!;
			var neutral = Load(NeutralResx).Root!;
			var family = FamilyKeys(pt).ToList();
			family.Count.Should().Be(88, "78 clés historiques + 10 créées le 02/11 — ni perte, ni doublon");

			// Les 4 mortes restent présentes et INTACTES (la garde #2118 les épingle).
			foreach (var dead in DeadKeys)
			{
				family.Should().Contain(dead, "clé morte délibérément conservée — voir #2118");
			}

			// Les 3 neutres structurellement vides (<value />) ne donnent PAS de clé pt :
			// source vide = rien à traduire. Si le neutre les remplit un jour, il faudra
			// créer les contreparties pt-BR — ce test rouvrira la question.
			var emptyNeutralKeys = new[] { "ScannerExplanation.Text", "ModifiedFilesExplaination.Text", "ModifiedSettingsExplaination.Text" };
			foreach (var key in emptyNeutralKeys)
			{
				var neutralCell = (string?)neutral.Elements("data")
					.FirstOrDefault(d => (string?)d.Attribute("name") == key)?.Element("value");
				neutralCell.Should().BeNullOrEmpty($"{key} est vide au neutre — c'est ce qui justifie l'absence de clé pt");
				family.Should().NotContain(key, "source vide au 02/11 : aucune contrepartie pt-BR créée");
			}
		}

		[Fact]
		public void Committed_Blob_Is_Utf8_Bom_With_Lf_Only()
		{
			// Invariant de TRANSPORT sur le blob committé : un runner Windows core.autocrlf=true
			// convertit LF -> CRLF AU CHECKOUT — un fichier sain apparait en CRLF dans l'arbre.
			var bytes = CommittedBlob("DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Security/App_LocalResources/security.pt-BR.resx");
			bytes.Length.Should().BeGreaterThan(1000, "le blob doit être le fichier entier, pas un fragment");
			bytes.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "le BOM est celui du fichier d'origine");
			bytes.Count(b => b == 0x0D).Should().Be(0, "le blob committé est en LF seul");
		}

		private static string? Cell(XElement root, string key) => (string?)root.Elements("data")
			.First(d => (string?)d.Attribute("name") == key)
			.Element("value");

		private static byte[] CommittedBlob(string rel) => DnnResxGuardSocle.CommittedBlob(rel);
	}
}
