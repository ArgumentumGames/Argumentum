using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Dnn
{
	/// <summary>
	/// Organe anti-retour du dé-track #1244(a) (PR #1365, commit 5c616ec4, décision owner du 13/09 2026) :
	/// le dépôt public ne doit plus suivre RIEN sous DNNPlatform/bin/.
	///
	/// DÉFAUT FONDATEUR. master suivait 195 fichiers sous DNNPlatform/bin/ (179 .dll) — un instantané
	/// baseline 9.11.1 : 39 assemblages en majeure 9, dont DotNetNuke.dll 9.11.1.0, quand la préprod
	/// sert 10.3.2.0. Toute opération git qui matérialise la couche suivie dans le webroot vivant
	/// (checkout d'un vieux commit, merge avec arbre ancien) ré-introduit silencieusement la source de
	/// downgrade. Le dé-track l'a retiré de l'index ; cet organe rend tout retour IMMÉDIATEMENT rouge
	/// au lieu d'attendre le prochain incident de déploiement.
	///
	/// POURQUOI L'INDEX ET PAS LE DISQUE NI LE .gitignore SEUL. Le disque peut légitimement porter bin/
	/// (le webroot vivant EST un checkout : 281 fichiers non suivis). Le .gitignore peut être correct
	/// pendant que l'index est sale — c'est l'index qui serait committé. L'organe lit donc `git ls-files`
	/// (l'index), et vérifie en sus que la règle d'ignore explicite /DNNPlatform/bin/ survit : les
	/// ré-inclusions `!` de #972 sont le piège historique (§3 de la doc de provenance) — sans la règle,
	/// un `git add .` dans le webroot vivant ré-ajouterait les 195 fichiers d'un coup.
	///
	/// PLUS FORT QUE LA FORME INITIALE DE L'ISSUE. Le corps de #1244 proposait « si DotNetNuke.dll est
	/// tracé, sa version doit être celle que la base attend ». Post-(a) l'invariant est plus strict :
	/// AUCUNE entrée sous bin/ n'est acceptable — l'ancienne forme laisserait passer 194 fichiers
	/// fautifs du moment que la seule DLL vérifiée serait à jour.
	///
	/// CE QUE FAIRE SI C'EST ROUGE. Soit l'entrée est volontaire : c'est une ré-ouverture de la décision
	/// (a), à porter à l'owner — pas à faire taire. Soit c'est un retour : `git rm -r --cached
	/// DNNPlatform/bin/` + restaurer la règle `/DNNPlatform/bin/` du .gitignore. Les fichiers restent
	/// restaurables par l'historique git et l'archive L2 — voir
	/// docs/quality/1244a-dnn-bin-untrack-provenance.md §2.
	/// </summary>
	internal sealed class RequiresGitIndexFactAttribute : FactAttribute
	{
		public RequiresGitIndexFactAttribute()
		{
			// Skip visible (précédent §4 de la provenance) : hors d'un checkout git — p.ex. un zip du
			// dépôt — l'index n'existe pas et le fait n'est PAS évaluable. Jamais un vert par vacuité.
			try
			{
				var exit = DnnBinUntrackGuard.RunGit("rev-parse --is-inside-work-tree", out var stdout, out _);
				if (exit != 0 || stdout.Trim() != "true")
				{
					Skip = $"#1244(a) : pas d'index Git évaluable ici (rev-parse exit={exit}) — l'organe exige un checkout git.";
				}
			}
			catch (Exception ex)
			{
				Skip = $"#1244(a) : git indisponible ({ex.GetType().Name}) — l'organe exige un checkout git.";
			}
		}
	}

	/// <summary>Cœur pur de l'organe : chemins en entrée, violations en sortie — testable sans git.</summary>
	public static class DnnBinUntrackGuard
	{
		internal const string BinPrefix = "DNNPlatform/bin/";

		/// <summary>Toute entrée d'index sous DNNPlatform/bin/ (récursif, insensible à la casse) est une violation.</summary>
		public static IReadOnlyList<string> FindViolations(IEnumerable<string> trackedPaths)
		{
			return trackedPaths
				.Select(p => (p ?? string.Empty).Trim().Replace('\\', '/'))
				.Where(p => p.StartsWith(BinPrefix, StringComparison.OrdinalIgnoreCase))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
				.ToList();
		}

		/// <summary>
		/// Exige une règle EXPLICITE nommant DNNPlatform/bin (forme /DNNPlatform/bin/ ou /DNNPlatform/bin).
		/// Un glob incident (p.ex. « bin/ ») ou une ré-inclusion « ! » ne compte pas : c'est précisément
		/// la forme historique #972 qui rendait le dé-track partiel et silencieux.
		/// </summary>
		public static bool IgnoreRuleShieldsBin(IEnumerable<string> gitignoreLines)
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
				if (string.Equals(pattern, "DNNPlatform/bin", StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Exécute git dans la racine du dépôt ; renvoie le code de sortie sans lever.</summary>
		internal static int RunGit(string arguments, out string stdout, out string stderr)
		{
			var psi = new ProcessStartInfo
			{
				FileName = "git",
				Arguments = arguments,
				WorkingDirectory = TestRepoRoot.Find(),
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
			};
			using var process = Process.Start(psi);
			if (process is null)
			{
				throw new InvalidOperationException($"Impossible de lancer git ({arguments}).");
			}
			stdout = process.StandardOutput.ReadToEnd();
			stderr = process.StandardError.ReadToEnd();
			if (!process.WaitForExit(TimeSpan.FromSeconds(30)))
			{
				process.Kill();
				throw new TimeoutException($"git {arguments} : pas de sortie après 30 s (stdout='{stdout}' stderr='{stderr}').");
			}
			return process.ExitCode;
		}

		internal static List<string> ParseLines(string output)
		{
			return output
				.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
				.Select(l => l.Trim())
				.Where(l => l.Length > 0)
				.ToList();
		}
	}

	public class DnnBinUntrackGuardTests
	{
		// ── Témoins de mutation (cœur pur, sans git) : une garde qu'on ne peut pas faire rougir
		//    n'est pas une garde. Chacun encode une forme réelle du retour redouté. ─────────────

		[Fact]
		public void Witness_TrackedDotNetNukeDll_IsAViolation()
		{
			var violations = DnnBinUntrackGuard.FindViolations(new[]
			{
				"Cards/Fallacies/Some.csv",
				"DNNPlatform/bin/DotNetNuke.dll",
			});
			violations.Should().BeEquivalentTo(new[] { "DNNPlatform/bin/DotNetNuke.dll" },
				"le vecteur historique exact : la DLL 9.11.1 suivie qui dégrade un webroot 10.3.2");
		}

		[Fact]
		public void Witness_NestedPathUnderBin_IsAViolation()
		{
			var violations = DnnBinUntrackGuard.FindViolations(new[] { "DNNPlatform/bin/roslyn/csc.exe.config" });
			violations.Should().HaveCount(1, "le dé-track était récursif (3 .targets et configs sous bin/), le garde l'est aussi");
		}

		[Fact]
		public void Witness_PathOutsideBinScope_IsNotAViolation()
		{
			var violations = DnnBinUntrackGuard.FindViolations(new[]
			{
				"DNNPlatform/bin.aspx",                     // adjacent, PAS sous bin/
				"DNNPlatform/DesktopModules/Some/app.config", // hors périmètre de cet organe
				"DNNPlatform/web.config",                    // périmètre #1049, pas celui-ci
			});
			violations.Should().BeEmpty("l'organe #1244 ne porte que sur DNNPlatform/bin/");
		}

		[Fact]
		public void Witness_BackslashAndCaseVariants_AreStillViolations()
		{
			var violations = DnnBinUntrackGuard.FindViolations(new[] { "dnnplatform\\BIN\\Some.dll" });
			violations.Should().BeEquivalentTo(new[] { "dnnplatform/BIN/Some.dll" },
				"git émet des /, mais l'organe ne doit pas dépendre du dialecte de l'appelant ; la casse est préservée telle que rapportée");
		}

		[Fact]
		public void Witness_ReInclusionRule_DoesNotShieldBin()
		{
			DnnBinUntrackGuard.IgnoreRuleShieldsBin(new[] { "*.dll", "!/DNNPlatform/bin/" })
				.Should().BeFalse("la ré-inclusion « ! » de #972 est le piège historique, pas un bouclier");
		}

		[Fact]
		public void Witness_IncidentalGlobRule_DoesNotShieldBin()
		{
			DnnBinUntrackGuard.IgnoreRuleShieldsBin(new[] { "bin/" })
				.Should().BeFalse("le contrat exige la règle EXPLICITE /DNNPlatform/bin/ (doc de provenance §3)");
		}

		[Fact]
		public void Witness_ExplicitRule_ShieldsBin()
		{
			DnnBinUntrackGuard.IgnoreRuleShieldsBin(new[] { "# #1244(a): DNNPlatform/bin/ est DÉ-TRACKÉ", "/DNNPlatform/bin/" })
				.Should().BeTrue();
		}

		// ── L'organe proprement dit : mesure de l'index Git du checkout où tourne le test. ─────

		[RequiresGitIndexFact]
		public void TrackedIndex_UnderDnnPlatformBin_IsEmpty()
		{
			var scopedExit = DnnBinUntrackGuard.RunGit("ls-files -- DNNPlatform/bin/", out var scopedRaw, out var scopedErr);
			scopedExit.Should().Be(0, $"git ls-files doit réussir (stderr='{scopedErr}')");

			var fullExit = DnnBinUntrackGuard.RunGit("ls-files", out var fullRaw, out var fullErr);
			fullExit.Should().Be(0, $"git ls-files doit réussir (stderr='{fullErr}')");

			var scoped = DnnBinUntrackGuard.ParseLines(scopedRaw);
			var fromFull = DnnBinUntrackGuard.FindViolations(DnnBinUntrackGuard.ParseLines(fullRaw));

			// Corroboration obligatoire : le pathspec `-- DNNPlatform/bin/` a déjà produit une sortie
			// vide fausse sur chemin suivi (artefact intermittent, consigné). Un « vide » n'est cru
			// que si les DEUX instruments s'accordent — sinon c'est la panne d'instrument qui parle.
			scoped.Should().BeEquivalentTo(fromFull,
				"les deux sondes (pathspec borné vs liste complète filtrée) doivent s'accorder avant qu'un vide soit cru");

			fromFull.Should().BeEmpty(
				"#1244(a) : DNNPlatform/bin/ est dé-tracké (PR #1365) — toute entrée d'index sous bin/ est un retour " +
				"de la source de downgrade 9.11.1. Ré-ouverture de la décision owner ou `git rm -r --cached`, " +
				"voir docs/quality/1244a-dnn-bin-untrack-provenance.md §2");
		}

		[RequiresGitIndexFact]
		public void GitIgnore_StillShieldsDnnPlatformBin()
		{
			var gitignorePath = Path.Combine(TestRepoRoot.Find(), ".gitignore");
			File.Exists(gitignorePath).Should().BeTrue("le dépôt suit un .gitignore à sa racine");

			var lines = File.ReadAllLines(gitignorePath);
			DnnBinUntrackGuard.IgnoreRuleShieldsBin(lines).Should().BeTrue(
				"sans la règle explicite /DNNPlatform/bin/, un `git add .` dans le webroot vivant (checkout git) " +
				"ré-ajouterait d'un coup les assemblages du webroot — le bouclier est la première ligne de défense, " +
				"l'organe d'index la seconde (doc de provenance §3)");
		}
	}
}
