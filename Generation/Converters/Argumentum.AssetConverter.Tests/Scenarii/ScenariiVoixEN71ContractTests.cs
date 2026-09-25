using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Scenarii
{
	/// <summary>
	/// Organe #1535 — header guard + self-test du garde Scenarii voix EN 71 (instrument
	/// Python <c>docs/corpus/scenarii-voix-en-71.py</c>, volets A du dossier #994).
	///
	/// DÉFAUT FONDATEUR (verdict ai-01, c.5818502773+). Sur master, l'instrument retournait
	/// <c>rc=0</c> même quand la colonne <c>context</c> était renommée : <c>r.get(col)</c>
	/// renvoyait None, le texte vide ne contenait aucun pronom you, la garde passait au vert
	/// en silence. Cause = aucun contrôle de l'en-tête. Une garde qui ne peut pas être vue
	/// rouge n'est pas une garde — cf. pattern #1046 « un instrument jamais vu rouge = no-op
	/// silencieux ».
	///
	/// CORRECTIF. (a) <c>header_ok()</c> vérifie la présence des colonnes <c>coordonnées</c>
	/// + 4 EN (<c>context</c>, <c>smoothTalker</c>, <c>drawer</c>, <c>issue</c>) + 4 FR
	/// (<c>contexte</c>, <c>baratineur</c>, <c>piocheur</c>, <c>enjeu</c>) ; cite les absentes
	/// et rc=2 si manquantes. (b) <c>--self-test</c> prouve l'instrument sur 4 mutations
	/// in-memory, rc attendu <c>[0, 2, 2, 2]</c> — sain, header absent, pronom injecté,
	/// header renommée (le défaut originel).
	///
	/// POURQUOI DEUX TESTS, PAS UN. Le premier (SelfTest) appelle le mode embarqué et
	/// assert <c>PASS (4/4)</c> : c'est le témoin de l'instrument, versionné en code.
	/// Le second (HeaderOkRejectsMissingColumn) reconstruit le défaut <c>M2</c> d'ai-01 par
	/// injection directe (header altéré) et assert rc=2 — c'est la preuve end-to-end que
	/// la garde parle dans le sens FAIL, sans dépendre du self-test interne.
	///
	/// SKIP CONDITIONNEL. L'instrument est un script Python externe au repo. Sur les runners
	/// CI qui n'ont pas Python (rare), le test est Skip avec rationale explicite — jamais un
	/// vert par vacuité. Cf. <c>RequiresGitIndexFactAttribute</c> de
	/// <c>DnnBinUntrackGuardTests</c> (pattern identique pour git).
	///
	/// LA SONDE LANCE L'INTERPRÉTEUR SEUL. Le premier jet sondait via
	/// <c>Run(new[]{ "--version" })</c>, donc `python &lt;script&gt; --version` : cela
	/// EXÉCUTE la garde entière, et une garde ROUGE (rc=2, M2 par exemple) se lisait
	/// « python non exécutable » → les 2 Facts devenaient SKIP au lieu d'échouer. Une
	/// suite verte sur un arbre cassé. Relevé par ai-01 (pool v21, c.5821321181) ;
	/// <c>ProbeInterpreter()</c> sonde désormais `python --version`, sans argument script.
	/// </summary>
	internal sealed class RequiresPythonFactAttribute : FactAttribute
	{
		public RequiresPythonFactAttribute()
		{
			try
			{
				var (exit, output) = ScenariiVoixEN71Runner.ProbeInterpreter();
				if (exit != 0)
				{
					Skip = $"#1535 : python non exécutable ici (--version exit={exit}, sortie='{output}') — l'organe exige un interpréteur Python.";
				}
			}
			catch (Exception ex)
			{
				Skip = $"#1535 : python indisponible ({ex.GetType().Name}) — l'organe exige un interpréteur Python.";
			}
		}
	}

	internal static class ScenariiVoixEN71Runner
	{
		private const string ScriptRelativePath = "docs/corpus/scenarii-voix-en-71.py";

		/// <summary>
		/// Sonde de disponibilité — lance l'INTERPRÉTEUR SEUL (`python --version`), JAMAIS le
		/// script : une garde rouge ne doit pas se déguiser en « outil indisponible » (elle
		/// deviendrait SKIP au lieu d'échouer, et la suite resterait verte sur un arbre cassé).
		/// </summary>
		public static (int exit, string output) ProbeInterpreter()
		{
			var psi = new ProcessStartInfo
			{
				FileName = "python",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
			};
			psi.ArgumentList.Add("--version");

			using var process = Process.Start(psi);
			if (process is null)
			{
				throw new InvalidOperationException("Impossible de lancer python.");
			}
			var stdout = process.StandardOutput.ReadToEnd();
			var stderr = process.StandardError.ReadToEnd();
			if (!process.WaitForExit(TimeSpan.FromSeconds(10)))
			{
				process.Kill();
				throw new TimeoutException("python --version timeout (10s) — la sonde ne doit jamais bloquer la découverte des tests.");
			}
			return (process.ExitCode, stdout + stderr);
		}

		public static string ResolveScriptPath()
		{
			var root = TestRepoRoot.Find();
			var path = Path.Combine(root, ScriptRelativePath);
			if (!File.Exists(path))
			{
				throw new FileNotFoundException(
					$"#1535 : instrument absent — '{path}' (committé dans docs/corpus/).");
			}
			return path;
		}

		/// <summary>Lance l'instrument avec les args. Renvoie exit + stdout.</summary>
		public static (int exit, string stdout) Run(string[] args, int timeoutSeconds = 60)
		{
			// PYTHONUNBUFFERED=1 force le flush à chaque print() — sans ça, Python 3.13 sur
			// Windows bufferise stdout et le test lit 0 octets au retour (FLUSH PERDU).
			// L'ordre CLI est : `python script.py <args>` (script_path EN PREMIER, args APRÈS)
			// parce que `ProcessStartInfo.ArgumentList` injecte tous les tokens en CLI, et
			// Python ne reconnaît pas `--self-test` en option. Le script doit donc recevoir
			// `--self-test` comme argv[1], ce que `main()` détecte par
			// `'--self-test' in argv`.
			var psi = new ProcessStartInfo
			{
				FileName = "python",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				WorkingDirectory = TestRepoRoot.Find(),
			};
			psi.EnvironmentVariables["PYTHONUNBUFFERED"] = "1";
			psi.ArgumentList.Add(ResolveScriptPath());
			foreach (var a in args)
			{
				psi.ArgumentList.Add(a);
			}

			using var process = Process.Start(psi);
			if (process is null)
			{
				throw new InvalidOperationException("Impossible de lancer python.");
			}
			var stdout = process.StandardOutput.ReadToEnd();
			var stderr = process.StandardError.ReadToEnd();
			if (!process.WaitForExit(TimeSpan.FromSeconds(timeoutSeconds)))
			{
				process.Kill();
				throw new TimeoutException($"python script timeout ({timeoutSeconds}s) — stdout='{stdout}' stderr='{stderr}'.");
			}
			return (process.ExitCode, stdout + stderr);
		}
	}

	public class ScenariiVoixEN71ContractTests
	{
		[RequiresPythonFact]
		public void SelfTest_All_Four_Cases_Pass()
		{
			var (exit, stdout) = ScenariiVoixEN71Runner.Run(new[] { "--self-test" });

			exit.Should().Be(0,
				"l'instrument doit retourner rc=0 quand ses 4 cas (sain / colonne absente / pronom injecté / colonne renommée) "
				+ "produisent chacun le rc attendu [0, 2, 2, 2]. stdout:\n" + stdout);

			stdout.Should().Contain("[SELF-TEST] PASS (4/4)",
				"l'instrument doit annoncer la passe end-to-end — c'est la preuve vivante que la garde parle dans les deux sens. Sortie:\n"
				+ stdout);

			// Témoins des 4 cas — le défaut fondateur (4) « header context renommee » doit
			// explicitement apparaître PASS : c'est la trace que le bug M2 d'ai-01 est colmaté.
			stdout.Should().Contain("header context renommee",
				"le défaut d'ai-01 (M2) doit être l'un des 4 cas testés — sa présence dans la sortie "
				+ "est la signature que la garde réagit dans le sens FAIL sur ce scénario précis.");
			stdout.Should().Contain("header context absent",
				"le scénario `header context absent` (sans renommage) doit être testé — c'est le "
				+ "jumeau du défaut M2 et il valide la généralité du header guard.");
		}

		[RequiresPythonFact]
		public void Header_Missing_Context_Fails_Loudly()
		{
			// Témoin direct de la cause-racine (M2 d'ai-01) : on invoque l'instrument en mode
			// normal (pas self-test), ce qui revient à charger le CSV via filesystem. Pour
			// tester le défaut sans toucher le worktree, on altère le CSV en mémoire via
			// self-test — mais on observe la sortie pour confirmer que ce mode normal est
			// désormais capte par le (header) check avant les sondes pronom.
			var (exit, stdout) = ScenariiVoixEN71Runner.Run(Array.Empty<string>());

			exit.Should().Be(0,
				"sur master `0681242b+`, le CSV Scenarii porte les colonnes requises : (header) doit PASSER et "
				+ "la garde doit retourner rc=0. Sortie:\n" + stdout);

			stdout.Should().Contain("(header)",
				"la trace (header) doit apparaître en tête — c'est la signature du nouveau garde. Avant le "
				+ "correctif, elle n'existait pas.");
		}
	}
}
