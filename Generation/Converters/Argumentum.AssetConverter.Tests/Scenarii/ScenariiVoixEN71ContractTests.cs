using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
	/// POURQUOI TROIS TESTS, PAS UN. (1) <c>SelfTest</c> appelle le mode embarqué et
	/// assert <c>PASS (4/4)</c> : témoin de l'instrument, versionné en code.
	/// (2) <c>Header_Renamed_Column_Fails_Loudly_EndToEnd</c> rejoue la mutation <c>M2</c>
	/// d'ai-01 — en-tête `context` renommé — sur une COPIE en temp via `--csv` et assert
	/// rc=2 : c'est le contrôle inverse bout-en-bout, qui ne dépend ni du self-test interne
	/// ni de l'état du worktree. (3) <c>Healthy_Csv_Emits_Header_Trace</c> vérifie la trace
	/// `(header)` sur un run normal sain (rc=0) — le nom dit ce qui est vérifié.
	///
	/// SKIP CONDITIONNEL. L'instrument est un script Python externe au repo. Sur les runners
	/// CI qui n'ont pas Python (rare), le test est Skip avec rationale explicite — jamais un
	/// vert par vacuité. Cf. <c>RequiresGitIndexFactAttribute</c> de
	/// <c>DnnBinUntrackGuardTests</c> (pattern identique pour git).
	///
	/// LA SONDE LANCE L'INTERPRÉTEUR SEUL. Le premier jet sondait via
	/// <c>Run(new[]{ "--version" })</c>, donc `python &lt;script&gt; --version` : cela
	/// EXÉCUTE la garde entière, et une garde ROUGE (rc=2, M2 par exemple) se lisait
	/// « python non exécutable » → les Facts devenaient SKIP au lieu d'échouer. Une
	/// suite verte sur un arbre cassé. Relevé par ai-01 (pool v21, c.5821321181) ;
	/// <c>ProbeInterpreter()</c> sonde désormais `python --version`, sans argument script.
	/// Épreuve mesurée sous M2 : pré-correctif 0 échec / 2 skip → post-correctif 2 échecs / 0 skip.
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
		private const string CsvRelativePath = "Cards/Scenarii/Argumentum Scenarii - Cards.csv";

		/// <summary>Chemin du CSV Scenarii du dépôt (celui que l'instrument lit par défaut).</summary>
		public static string ResolveCsvPath()
		{
			var path = Path.Combine(TestRepoRoot.Find(), CsvRelativePath);
			if (!File.Exists(path))
			{
				throw new FileNotFoundException(
					$"#1535 : CSV Scenarii absent — '{path}'.");
			}
			return path;
		}

		/// <summary>
		/// Construit une copie du CSV avec l'en-tête `context` renommé en
		/// <c>context_renamed</c> — la mutation M2 d'ai-01, rejouée sans toucher au worktree.
		/// Seule la ligne d'en-tête change ; les octets des rangées sont recopiés tels quels.
		/// </summary>
		public static string BuildRenamedHeaderCopy()
		{
			var source = ResolveCsvPath();
			var lines = File.ReadAllLines(source);
			var fields = lines[0].Split(',');
			var idx = Array.IndexOf(fields, "context");
			if (idx < 0)
			{
				throw new InvalidOperationException(
					"#1535 : colonne 'context' introuvable dans l'en-tête — la mutation M2 ne peut pas être construite.");
			}
			fields[idx] = "context_renamed";
			lines[0] = string.Join(",", fields);

			var temp = Path.Combine(Path.GetTempPath(),
				"#1535-scenarii-m2-" + Guid.NewGuid().ToString("N") + ".csv");
			File.WriteAllLines(temp, lines);
			return temp;
		}

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
			psi.StandardOutputEncoding = Encoding.UTF8;
			psi.StandardErrorEncoding = Encoding.UTF8;

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
			// Encodage explicite des DEUX côtés du tube. Sur un runner Windows, Python écrit
			// ses print() dans l'encodage ANSI de la machine (é = 0xE9) et .NET décodait en
			// CP437 (OEM) : « présentes » arrivait en « prΘsentes », et toute assertion sur un
			// libellé accentué échouait — en CI seulement, jamais en local. Mesuré sur
			// `Healthy_Csv_Emits_Header_Trace` (CI Debug/Release du 2026-09-25T01:13Z).
			psi.EnvironmentVariables["PYTHONIOENCODING"] = "utf-8";
			psi.StandardOutputEncoding = Encoding.UTF8;
			psi.StandardErrorEncoding = Encoding.UTF8;
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
		public void Header_Renamed_Column_Fails_Loudly_EndToEnd()
		{
			// Contrôle inverse exigé par la revue ai-01 (point 3) : la mutation M2 — en-tête
			// `context` renommé — rejouée sur une COPIE en temp (worktree intact) via `--csv`.
			// C'est le seul témoin qui prouve que le sens FAIL sort en rouge (rc=2), et que la
			// sonde de disponibilité ne peut plus transformer ce rouge en SKIP.
			var temp = ScenariiVoixEN71Runner.BuildRenamedHeaderCopy();
			try
			{
				var (exit, stdout) = ScenariiVoixEN71Runner.Run(new[] { "--csv", temp });

				exit.Should().Be(2,
					"l'en-tête `context` renommé doit faire sortir la garde en rc=2 (défaut M2 d'ai-01). Sortie:\n"
					+ stdout);

				stdout.Should().Contain("(header) colonnes absentes",
					"la trace du header guard doit citer explicitement les colonnes absentes — c'est la "
					+ "signature que le check d'en-tête précède les sondes pronom. Sortie:\n" + stdout);

				stdout.Should().Contain("['context']",
					"la colonne manquante doit être NOMMÉE dans la sortie, pas seulement comptée. Sortie:\n"
					+ stdout);
			}
			finally
			{
				if (File.Exists(temp))
				{
					File.Delete(temp);
				}
			}
		}

		[RequiresPythonFact]
		public void Healthy_Csv_Emits_Header_Trace()
		{
			// Trace du header guard sur un run NORMAL (CSV du dépôt) : la ligne (header) doit
			// apparaître en tête et la garde sortir rc=0. Le nom dit ce qui est vérifié —
			// le sens FAIL vit dans Header_Renamed_Column_Fails_Loudly_EndToEnd.
			var (exit, stdout) = ScenariiVoixEN71Runner.Run(Array.Empty<string>());

			exit.Should().Be(0,
				"le CSV Scenarii du dépôt porte les colonnes requises : (header) doit PASSER et "
				+ "la garde retourner rc=0. Sortie:\n" + stdout);

			stdout.Should().Contain("(header) 9 colonnes requises présentes",
				"la trace (header) doit apparaître en tête — c'est la signature du header guard. "
				+ "Avant le correctif M2, elle n'existait pas. Sortie:\n" + stdout);
		}
	}
}
