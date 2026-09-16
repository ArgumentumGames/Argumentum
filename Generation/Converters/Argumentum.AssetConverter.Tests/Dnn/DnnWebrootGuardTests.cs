using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Dnn
{
	/// <summary>
	/// Organe anti-retour #1049 — le `web.config` RACINE de DNNPlatform ne doit plus jamais être
	/// suivi par git : toute entrée d'index à ce chemin est le toxique qui a mis la préprod à terre
	/// deux fois. Frère de la garde #1244 (DnnBinUntrackGuardTests, PR #1393, commit 517bbecf) :
	/// DEUX déclencheurs, DEUX gardes — #1244 verrouille DNNPlatform/bin/ (source de downgrade
	/// 9.11.1), celui-ci verrouille le web.config racine (500 total). Le lien est documenté des
	/// deux côtés : docs/quality/1244a-dnn-bin-untrack-provenance.md §7 et
	/// docs/quality/1049-webroot-guard.md.
	///
	/// DÉFAUT FONDATEUR, DEUX FOIS. (1) 2026-08-10 01:13 : le blob suivi (79 286 o — instantané
	/// DNN 9.11.1 de 2023 retouché CSP #131, jamais capable de faire tourner le site) a été écrit
	/// par-dessus le web.config vivant (96 984 o, 10.3.2) par une opération git de routine ⇒ 500
	/// déterministe sur tout le site. #1055 (167fbd33) l'a dé-tracké (→ web.config.example) avec
	/// la règle d'ignore. (2) 2026-08-16 12:12 : sur un master local étale qui le trackait encore,
	/// `git checkout master` a ré-écrit le blob par-dessus le vivant, puis le `pull --ff-only`
	/// avançant jusqu'au commit qui le dé-tracke l'a SUPPRIMÉ du working tree ⇒ 500 (2 h 35).
	/// Leçon : dé-tracker un fichier vivant est actif au checkout, pas seulement au commit.
	///
	/// DEUX SONDES, UN PAR SENS DE LA BRÈCHE. (a) L'INDEX : tant que le chemin n'est pas suivi,
	/// aucune opération git (checkout, pull, restore, stash, reset, merge) ne peut matérialiser le
	/// toxique dans un webroot — c'est le sens dépôt → webroot. (b) LA RÈGLE D'IGNORE EXPLICITE
	/// /DNNPlatform/web.config : le webroot vivant EST un checkout git (#1358) et le web.config
	/// vivant porte des secrets (machineKey — exposition déjà payée, rotation 25/07) ; sans la
	/// règle, un `git add .` dans le webroot PUBLIE le vivant dans le dépôt public — c'est le sens
	/// webroot → dépôt. L'index est la seconde ligne de défense ; la règle d'ignore, la première.
	///
	/// PÉRIMÈTRE PRÉCIS. Le web.config RACINE uniquement : IIS sert ce nom exact pour le site. Les
	/// Web.config de sous-répertoires (DesktopModules/, Install/, Portals/…) sont du contenu
	/// plateforme suivi, jamais mesuré comme toxique ; web.config.example est assaini et jamais
	/// servi ; DNNPlatform/bin/ relève de la garde sœur #1393 ; Generation/CardPen/web.config
	/// sert un autre hôte (site CardPen local), pas le webroot DNN.
	///
	/// CE QUE FAIRE SI C'EST ROUGE. Entrée d'index : c'est le retour du toxique — soit une
	/// ré-ouverture délibérée de la décision #1055 (à porter à l'owner, pas à faire taire), soit
	/// un retour accidentel (`git rm --cached DNNPlatform/web.config`, le fichier vivant n'est pas
	/// à toucher). Règle d'ignore manquante : restaurer `/DNNPlatform/web.config` dans le
	/// .gitignore AVANT toute autre commande git dans le webroot — sans elle, la prochaine
	/// publication embarque le vivant et ses secrets.
	/// </summary>
	public static class DnnWebrootGuard
	{
		internal const string RootWebConfig = "DNNPlatform/web.config";

		/// <summary>
		/// Toute entrée d'index ÉGALE au chemin racine (insensible à la casse — IIS l'est — et au
		/// dialecte de séparateurs) est une violation. Égalité exacte, pas de préfixe :
		/// web.config.example, les Web.config de sous-répertoires et CardPen ne le sont pas.
		/// </summary>
		public static IReadOnlyList<string> FindViolations(IEnumerable<string> trackedPaths)
		{
			return trackedPaths
				.Select(p => (p ?? string.Empty).Trim().Replace('\\', '/'))
				.Where(p => string.Equals(p, RootWebConfig, StringComparison.OrdinalIgnoreCase))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToList();
		}

		/// <summary>
		/// Exige une règle EXPLICITE nommant DNNPlatform/web.config (forme /DNNPlatform/web.config
		/// ou DNNPlatform/web.config). Un glob incident (« web.config » nu, « DNNPlatform/ ») ou une
		/// ré-inclusion « ! » ne compte pas : le contrat exige la règle qui nomme le chemin, même
		/// exigence que la garde sœur #1393 pour /DNNPlatform/bin/.
		/// </summary>
		public static bool IgnoreRuleShieldsWebConfig(IEnumerable<string> gitignoreLines)
		{
			foreach (var raw in gitignoreLines)
			{
				var line = (raw ?? string.Empty).Trim();
				if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal))
				{
					continue;
				}
				if (line.StartsWith("!", StringComparison.Ordinal))
				{
					continue; // ré-inclusion : effet inverse du bouclier exigé
				}
				var pattern = line.TrimEnd('/');
				if (pattern.StartsWith("/", StringComparison.Ordinal))
				{
					pattern = pattern.Substring(1);
				}
				if (string.Equals(pattern, RootWebConfig, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}
	}

	public class DnnWebrootGuardTests
	{
		// ── Témoins de mutation (cœur pur, sans git) : une garde qu'on ne peut pas faire rougir
		//    n'est pas une garde. Chacun encode une forme réelle du retour redouté. ─────────────

		[Fact]
		public void Witness_TrackedRootWebConfig_IsAViolation()
		{
			var violations = DnnWebrootGuard.FindViolations(new[]
			{
				"Cards/Fallacies/Some.csv",
				"DNNPlatform/web.config",
			});
			violations.Should().BeEquivalentTo(new[] { "DNNPlatform/web.config" },
				"le vecteur exact des deux incidents : le web.config racine suivi, écrit par-dessus le vivant");
		}

		[Fact]
		public void Witness_CaseAndBackslashVariants_AreStillViolations()
		{
			var violations = DnnWebrootGuard.FindViolations(new[] { "dnnplatform\\WEB.CONFIG" });
			violations.Should().BeEquivalentTo(new[] { "dnnplatform/WEB.CONFIG" },
				"IIS sert ce nom sans égard à la casse, et l'organe ne doit pas dépendre du dialecte " +
				"de séparateurs de l'appelant ; la casse est préservée telle que rapportée");
		}

		[Fact]
		public void Witness_SimilarButDistinctPaths_AreNotViolations()
		{
			var violations = DnnWebrootGuard.FindViolations(new[]
			{
				"DNNPlatform/web.config.example",                    // le template assaini de #1055, jamais servi
				"DNNPlatform/DesktopModules/ToSic.Sxc/web.config",   // contenu plateforme suivi, jamais mesuré toxique
				"DNNPlatform/Portals/Web.config",                    // idem
				"DNNPlatform/web.config.live-snapshot-20260816",     // snapshot incident, pas un nom IIS-actif
				"Generation/CardPen/web.config",                     // hôte CardPen local, pas le webroot DNN
			});
			violations.Should().BeEmpty(
				"le toxique mesuré est le web.config RACINE (nom exact servi par IIS pour le site) — " +
				"l'égalité est exacte, pas un préfixe ni un glob");
		}

		[Fact]
		public void Witness_ReInclusionRule_DoesNotShieldWebConfig()
		{
			DnnWebrootGuard.IgnoreRuleShieldsWebConfig(new[] { "*.config", "!/DNNPlatform/web.config" })
				.Should().BeFalse("la ré-inclusion « ! » est le piège historique #972, pas un bouclier");
		}

		[Fact]
		public void Witness_IncidentalGlobRule_DoesNotShieldWebConfig()
		{
			DnnWebrootGuard.IgnoreRuleShieldsWebConfig(new[] { "web.config", "DNNPlatform/" })
				.Should().BeFalse(
					"un glob nu couvrirait le chemin par effet de bord sans nommer la décision ; le contrat " +
					"exige la règle EXPLICITE /DNNPlatform/web.config (même exigence que la garde sœur #1393)");
		}

		[Fact]
		public void Witness_ExplicitRule_ShieldsWebConfig()
		{
			DnnWebrootGuard.IgnoreRuleShieldsWebConfig(new[]
			{
				"# #972 / #1049: DNNPlatform/ is a LIVE webroot",
				"/DNNPlatform/web.config",
			})
				.Should().BeTrue();
		}

		// ── L'organe proprement dit : mesure de l'index Git du checkout où tourne le test. ─────

		[RequiresGitIndexFact]
		public void TrackedIndex_HasNoDnnPlatformRootWebConfig()
		{
			var scopedExit = DnnBinUntrackGuard.RunGit("ls-files -- DNNPlatform/web.config", out var scopedRaw, out var scopedErr);
			scopedExit.Should().Be(0, $"git ls-files doit réussir (stderr='{scopedErr}')");

			var fullExit = DnnBinUntrackGuard.RunGit("ls-files", out var fullRaw, out var fullErr);
			fullExit.Should().Be(0, $"git ls-files doit réussir (stderr='{fullErr}')");

			var scoped = DnnBinUntrackGuard.ParseLines(scopedRaw);
			var fromFull = DnnWebrootGuard.FindViolations(DnnBinUntrackGuard.ParseLines(fullRaw));

			// Corroboration obligatoire : le pathspec borné a déjà produit un vide faux sur chemin
			// suivi (artefact intermittent, consigné). Un « vide » n'est cru que si les DEUX
			// instruments s'accordent — sinon c'est la panne d'instrument qui parle.
			scoped.Should().BeEquivalentTo(fromFull,
				"les deux sondes (pathspec borné vs liste complète filtrée) doivent s'accorder avant qu'un vide soit cru");

			fromFull.Should().BeEmpty(
				"#1049/#1055 : le web.config racine est dé-tracké — toute entrée d'index à ce chemin est le " +
				"toxique des incidents 10/08 et 16/08 (checkout/pull/restore le réécriraient par-dessus le " +
				"vivant, 500 total). Ré-ouverture de la décision owner ou `git rm --cached DNNPlatform/web.config` " +
				"(sans toucher au fichier vivant) — voir docs/quality/1049-webroot-guard.md");
		}

		[RequiresGitIndexFact]
		public void GitIgnore_StillShieldsDnnPlatformRootWebConfig()
		{
			var gitignorePath = Path.Combine(TestRepoRoot.Find(), ".gitignore");
			File.Exists(gitignorePath).Should().BeTrue("le dépôt suit un .gitignore à sa racine");

			var lines = File.ReadAllLines(gitignorePath);
			DnnWebrootGuard.IgnoreRuleShieldsWebConfig(lines).Should().BeTrue(
				"sans la règle explicite /DNNPlatform/web.config, un `git add .` dans le webroot vivant " +
				"(qui EST un checkout git, #1358) publierait le web.config vivant — machineKey comprise — " +
				"dans le dépôt public. Le bouclier est la première ligne de défense, l'organe d'index la " +
				"seconde (docs/quality/1049-webroot-guard.md §3)");
		}
	}
}
