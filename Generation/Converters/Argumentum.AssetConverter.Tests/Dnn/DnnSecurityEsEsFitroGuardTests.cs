using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 — pool po-2024, item « es-ES fitro » : le typo <c>fitro</c> de
	/// <c>IpFilterUpdateError.Text</c> dans <c>security.es-ES.resx</c>, corrigé en <c>filtro</c>.
	/// Mesure au moment du fix : c'était l'<b>unique</b> occurrence de <c>fitro</c> du dépôt
	/// entier (grep insensible à la casse sur <c>DNNPlatform/</c>), alors que le même fichier
	/// épelait <c>filtro</c> correctement 12 fois en minuscule — dont la clé sœur
	/// <c>IpFilterDeleteError.Text</c>, construction parallèle exacte
	/// (« No se ha podido eliminar el <b>filtro</b>. Vuelva a intentarlo. »).
	///
	/// <para>La garde ne protège pas que la cellule : le deuxième balaie <b>toutes</b> les
	/// valeurs du fichier — le typo ne doit renaître nulle part. La valeur est épinglée
	/// littéralement, et le neutre doit continuer de porter la clé (elle est vivante :
	/// « Could not update the IP filter. Please try later. »).</para>
	/// </summary>
	public class DnnSecurityEsEsFitroGuardTests
	{
		private static string EsResx => Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "DesktopModules", "Admin", "Dnn.PersonaBar", "Modules",
			"Dnn.Security", "App_LocalResources", "security.es-ES.resx");

		private static string NeutralResx => Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "DesktopModules", "Admin", "Dnn.PersonaBar", "Modules",
			"Dnn.Security", "App_LocalResources", "Security.resx");

		private const string Key = "IpFilterUpdateError.Text";

		private const string CorrectedSpanish =
			"No se ha podido actualizar el filtro. Vuelva a intentarlo.";

		private static XDocument Load(string path) => XDocument.Load(path);

		[Fact]
		public void IpFilterUpdateError_Is_Pinned_With_The_Corrected_Spanish()
		{
			var es = Load(EsResx).Root!;
			var cell = (string?)es.Elements("data")
				.First(d => (string?)d.Attribute("name") == Key)
				.Element("value");
			cell.Should().Be(CorrectedSpanish,
				"la cellule corrigée ne doit plus bouger : le fix du 02/11 remplace le typo `fitro` " +
				"par `filtro`, épelle que le même fichier emploie correctement 12 fois en minuscule, " +
				"avec la construction parallèle d'IpFilterDeleteError.Text comme témoin");
		}

		[Fact]
		public void No_Fitro_Anywhere_In_The_Spanish_File()
		{
			var es = Load(EsResx).Root!;
			var offenders = es.Elements("data")
				.Where(d => ((string?)d.Element("value") ?? string.Empty)
					.Contains("fitro", StringComparison.OrdinalIgnoreCase))
				.Select(d => (string?)d.Attribute("name"))
				.ToList();
			offenders.Should().BeEmpty(
				"`fitro` n'est pas un mot espagnol et n'est le sous-mot d'aucun mot légitime : la " +
				"correction du 02/11 était l'unique occurrence du dépôt entier — le typo ne doit " +
				"renaître dans AUCUNE cellule du fichier");
		}

		[Fact]
		public void The_Key_Is_Live_With_Its_English_Reference()
		{
			var neutral = Load(NeutralResx).Root!;
			var cell = (string?)neutral.Elements("data")
				.FirstOrDefault(d => (string?)d.Attribute("name") == Key)
				?.Element("value");
			cell.Should().Be("Could not update the IP filter. Please try later.",
				"la clé est vivante au neutre : c'est la source qui fonde l'épingle espagnole. Si " +
				"elle disparaît ou change de sens, la cellule es-ES est à revisiter, pas à conserver");
		}

		[Fact]
		public void Committed_Blob_Is_Utf8_Bom_With_Lf_Only()
		{
			// Invariant de TRANSPORT sur le blob committé : un runner Windows core.autocrlf=true
			// convertit LF -> CRLF AU CHECKOUT — un fichier sain apparait en CRLF dans l'arbre.
			var bytes = CommittedBlob("DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Security/App_LocalResources/security.es-ES.resx");
			bytes.Length.Should().BeGreaterThan(1000, "le blob doit être le fichier entier, pas un fragment");
			bytes.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "le BOM est celui du fichier d'origine");
			bytes.Count(b => b == 0x0D).Should().Be(0, "le blob committé est en LF seul");
		}

		private static byte[] CommittedBlob(string rel) => DnnResxGuardSocle.CommittedBlob(rel);
	}
}
