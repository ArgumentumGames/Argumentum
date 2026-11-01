using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 — item 2 du pool po-2024 : <b>nommer les 13 cles mortes</b> de
	/// <c>Dnn.Security/App_LocalResources/security.pt-BR.resx</c>. Morte = presente dans le
	/// pt-BR, <b>absente du neutre</b> <c>Security.resx</c> et absente de l'es-ES : il n'existe
	/// aucune source anglaise a traduire, et l'es-ES ne les porte pas non plus.
	///
	/// <para><b>Provenance mesuree sur l'historique du neutre</b> (git log -S + git show) :
	/// <c>CheckTelerikVulnerability*</c> et <c>plSSLEnabled*</c> y existaient au depot initial
	/// (<c>ac211318</c>) et en ont ete retirees par <c>4e3f4c1f</c> (« Upgrade DNN 9.11 ») ;
	/// <c>Redirect_After*</c> ont ete retirees par <c>27675a95</c> (« Update Argumentum DNN
	/// website ») ; <c>Não.Text</c> n'y a <b>jamais</b> existe (alias pt-only de <c>No.Text</c>).
	/// Le neutre les a remplacees : <c>plSSLEnforced.*</c> pour le SSL,
	/// <c>RedirectionMovedToSiteSettings.Text</c> pour les redirections, <c>No.Text</c> pour
	/// « Não ». Le prefixe <c>CheckTelerikVulnerability</c> n'a <b>plus aucun</b> descendant au
	/// neutre — la verification Telerik a quitte ce fichier.</para>
	///
	/// <para><b>Decision</b> : conservees, ni supprimees ni traduites. Une suppression serait un
	/// changement de comportement potentiel pour un DNN qui lirait encore ces cles (regle du
	/// depot : jamais de suppression sans preuve de preservation). Cette garde rend les deux
	/// gestes silencieux impossibles : le compte exact (13) rougit si une cle disparait OU si
	/// une 14e nait, et chaque valeur est epinglee — « traduire » une cle morte devient rouge.</para>
	///
	/// <para><b>Instrument</b> : l'inventaire se mesure en C# par <c>XDocument</c>, pas par
	/// regex — le neutre porte trois <c>&lt;value /&gt;</c> auto-fermes
	/// (<c>ScannerExplanation.Text</c>, <c>ModifiedFilesExplaination.Text</c>,
	/// <c>ModifiedSettingsExplaination.Text</c>) et un parse regex qui ignore la valeur
	/// auto-fermee AVALE LA CLE SUIVANTE (mesure 02/11 : un tel parse compte 15 mortes au lieu
	/// de 13).</para>
	/// </summary>
	public class DnnSecurityDeadKeysPtBrGuardTests
	{
		private static string PtResx => Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "DesktopModules", "Admin", "Dnn.PersonaBar", "Modules",
			"Dnn.Security", "App_LocalResources", "security.pt-BR.resx");

		private static string NeutralResx => Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "DesktopModules", "Admin", "Dnn.PersonaBar", "Modules",
			"Dnn.Security", "App_LocalResources", "security.resx");

		private static string EsResx => Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "DesktopModules", "Admin", "Dnn.PersonaBar", "Modules",
			"Dnn.Security", "App_LocalResources", "security.es-ES.resx");

		private static Dictionary<string, string> Load(string path)
		{
			var root = XDocument.Load(path).Root!;
			return root.Elements("data")
				.ToDictionary(d => (string)d.Attribute("name")!, d => (string?)d.Element("value") ?? string.Empty,
					StringComparer.Ordinal);
		}

		private static string Sha256Hex(string cell)
		{
			using var sha = SHA256.Create();
			return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(cell))).ToLowerInvariant();
		}

		/// <summary>Les 13 cles mortes, nommees — l'inventaire mesure doit lui EGALER.</summary>
		private static readonly string[] DeadKeys =
		{
			"CheckTelerikVulnerabilityFailure.Text", "CheckTelerikVulnerabilityName.Text",
			"CheckTelerikVulnerabilityReason.Text", "CheckTelerikVulnerabilitySuccess.Text",
			"N\u00e3o.Text", "Redirect_AfterLogin.Help",
			"Redirect_AfterLogin.Text", "Redirect_AfterLogout.Help",
			"Redirect_AfterLogout.Text", "Redirect_AfterRegistration.Help",
			"Redirect_AfterRegistration.Text", "plSSLEnabled.Help",
			"plSSLEnabled.Text",
		};

		/// <summary>Ce qui les a remplacees au neutre — temoins de la provenance.</summary>
		private static readonly string[] LiveCounterparts =
		{
			"No.Text", "RedirectionMovedToSiteSettings.Text",
			"plSSLEnforced.Text", "plSSLEnforced.Help",
		};

		/// <summary>Empreinte de chaque valeur morte : personne ne « traduit » une cle sans source.</summary>
		private static readonly (string Key, string Sha256)[] Pins =
		{
			("CheckTelerikVulnerabilityFailure.Text", "e72baa33726093213044d3e2b8108edfc06525c6befe0bd68ffc27cad15e378e"),
			("CheckTelerikVulnerabilityName.Text", "0c8c8d6f08e5e364adb35585e696ce9e1453462fe38d69a422e4466991ce5a7d"),
			("CheckTelerikVulnerabilityReason.Text", "45d67698265ad567fd994070b9cb09c4493bb1730a6987bffc8263f85569e39c"),
			("CheckTelerikVulnerabilitySuccess.Text", "8a7249e18cd193434d126e7fe2812358cb531225e19781ad9887adfacf68e225"),
			("N\u00e3o.Text", "9bc947e58b62b2fef2954dc2813f3bfe458455fea1bb40b2d7a33e010dfe9ab6"),
			("Redirect_AfterLogin.Help", "8ad4ba69841c91fc3e1fa0363ee9cdd6673694a2d0570875663dcb619ba8ed5d"),
			("Redirect_AfterLogin.Text", "36fe1590c1ac2a73e9a6bf5596e58c84b68e10149ac38a01bcf768d5f04f2d36"),
			("Redirect_AfterLogout.Help", "9bd68e4ca18ec7c1592c53035e4695069c8977bce2d0744a99c0122793f2aefb"),
			("Redirect_AfterLogout.Text", "ffc9112a3f44c2e31d5d2a205f53e5985c2cf29343f905576a64dea008fa27a0"),
			("Redirect_AfterRegistration.Help", "f92110715fb547e17717a522e0676db0a37ccce4418555cb524c968ba77ff71a"),
			("Redirect_AfterRegistration.Text", "811cad9cd428dc945a309ad86c171a03c892ab9f2257fac9a9de89ee49f51be4"),
			("plSSLEnabled.Help", "4280f5c07b08859905423ac3c89d38fb64874968429e0185b30339adfa4a4679"),
			("plSSLEnabled.Text", "52279a0f002e635f7405a7406f6b7910ec0635d5a5992410e7ef8bd3b5e530d7"),
		};

		[Fact]
		public void Dead_Keys_Inventory_Is_Exactly_The_Named_Thirteen()
		{
			var pt = Load(PtResx);
			var neu = Load(NeutralResx);
			var measured = pt.Keys.Where(k => !neu.ContainsKey(k))
				.OrderBy(k => k, StringComparer.Ordinal).ToList();
			var expected = DeadKeys.OrderBy(k => k, StringComparer.Ordinal).ToList();
			measured.Should().Equal(expected,
				"l'inventaire des cles mortes doit rester EXACTEMENT les 13 nommees : une cle disparue " +
				"(suppression silencieuse) ou une 14e nee (copie silencieuse) rougit ce test — les deux " +
				"gestes sont deliberes ou ne sont pas");
			DeadKeys.Should().HaveCount(13);
			DeadKeys.Distinct().Should().HaveCount(13);
		}

		[Fact]
		public void Dead_Keys_Are_Dead_In_The_Spanish_Too()
		{
			var es = Load(EsResx);
			DeadKeys.Where(k => es.ContainsKey(k)).Should().BeEmpty(
				"une cle morte presente dans l'es-ES ne serait pas morte : elle aurait une source a " +
				"traduire, et l'inventaire devrait etre refait");
		}

		[Fact]
		public void Dead_Keys_Values_Are_Pinned()
		{
			var pt = Load(PtResx);
			Pins.Should().HaveCount(13);
			var offenders = Pins
				.Where(p => !pt.ContainsKey(p.Key) || Sha256Hex(pt[p.Key]) != p.Sha256)
				.Select(p => p.Key)
				.ToList();
			offenders.Should().BeEmpty(
				"modifier la valeur d'une cle morte (la « traduire ») est un geste non voulu : il n'existe " +
				"aucune source anglaise, aucune reecriture n'est definissable");
		}

		[Fact]
		public void Live_Counterparts_Exist_And_The_Retired_Prefix_Is_Gone()
		{
			var neu = Load(NeutralResx);
			foreach (var counterpart in LiveCounterparts)
			{
				neu.Should().ContainKey(counterpart,
					"la contrepartie vivante documente POURQUOI la cle est morte (remplacee au neutre) : " +
					"si elle disparait a son tour, l'inventaire doit etre refait");
			}
			neu.Keys.Where(k => k.StartsWith("CheckTelerikVulnerability", StringComparison.Ordinal))
				.Should().BeEmpty("la verification Telerik a quitte ce fichier : aucun descendant ne doit y revenir");
		}

		[Fact]
		public void Committed_Blob_Is_Utf8_Bom_With_Lf_Only()
		{
			// Invariant de TRANSPORT sur le blob committe : un runner Windows core.autocrlf=true
			// convertit LF -> CRLF AU CHECKOUT — un fichier sain apparait en CRLF dans l'arbre.
			var bytes = CommittedBlob("DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Security/App_LocalResources/security.pt-BR.resx");
			bytes.Length.Should().BeGreaterThan(1000, "le blob doit etre le fichier entier, pas un fragment");
			bytes.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "le BOM est celui du fichier d'origine");
			bytes.Count(b => b == 0x0D).Should().Be(0, "le blob committe est en LF seul");
		}

		private static byte[] CommittedBlob(string rel) => DnnResxGuardSocle.CommittedBlob(rel);
	}
}
