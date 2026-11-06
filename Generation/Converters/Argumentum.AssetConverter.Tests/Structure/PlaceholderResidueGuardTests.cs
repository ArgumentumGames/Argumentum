using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests.Structure
{
	/// <summary>
	/// #1460 phase B, first organ — <b>no literal placeholder survives into a card or a wrapper</b>.
	///
	/// <para>
	/// The phase-B source was re-measured on 2026-11-06 (issue #1460, comment c.6688584824): a card
	/// PDF carries <b>no text layer</b> (0 characters with both <c>pypdf</c> and <c>pdftotext</c>,
	/// 0 fonts; the same instrument reads 17,616 characters in a text-bearing PDF), and the
	/// <c>.harvest.json</c> carries <b>pixels only</b> (176 <c>data:image/png;base64</c>, 0
	/// non-<c>dataUrl</c> field). So a "presence of content" probe cannot be written against either
	/// artefact — it would read 0 on a correct bundle and 0 on a broken one. The text lives one step
	/// upstream: <b>template <c>mustache</c> × DataSet CSV row</b>.
	/// </para>
	///
	/// <para>
	/// This organ asserts the defect that upstream makes detectable without a browser: a variable the
	/// data cannot fill renders <em>literally</em> on the card (<c>{{text_fr}}</c>), which is exactly
	/// the failure family of #216 (a conversion map naming fields the template does not use) and
	/// #1141 (a CardSet with no localization group shipping FR content under a foreign suffix). It is
	/// a <b>structural</b> fact — the reference is either resolvable or it is not — never a proportion,
	/// so it needs no calibrated threshold.
	/// </para>
	///
	/// <para>
	/// The wrapper half calls the <b>production</b> formatter (<see cref="MindMapHtmlWrapper.FormatWrapper"/>)
	/// rather than re-implementing the substitution. An organ that transcribed the writer's rules
	/// would pass while the writer drifted — the second-referential defect this repository keeps
	/// paying for.
	/// </para>
	/// </summary>
	public class PlaceholderResidueGuardTests
	{
		private readonly ITestOutputHelper _output;

		public PlaceholderResidueGuardTests(ITestOutputHelper output) => _output = output;

		/// <summary>A <c>{{…}}</c> mustache expression, single level (no nested braces).</summary>
		private static readonly Regex MustacheToken = new Regex(@"\{\{([^{}]*)\}\}", RegexOptions.Compiled);

		/// <summary>
		/// A wrapper token: an upper-case word in brackets. Deliberately narrow — CSS attribute
		/// selectors and array literals are lower-case or start with a digit, so they are not
		/// matched. Measured on the two root templates (2026-11-06): the only tokens present are
		/// <c>[DIRECTION]</c>, <c>[LANGUAGE]</c>, <c>[TITLE]</c> and one of <c>[SVGCONTENT]</c> /
		/// <c>[SVGPATH]</c> depending on the file.
		/// </summary>
		private static readonly Regex WrapperToken = new Regex(@"\[[A-Z][A-Z0-9_]*\]", RegexOptions.Compiled);

		/// <summary>
		/// Handlebars block helpers in use across <c>Cards/**/*.json</c> (census of 2026-11-06:
		/// <c>if</c> 69 uses, <c>each</c> 9). The three <c>if…Header</c>/<c>ifLeafFirst</c> entries are
		/// registered by CardPen itself (<c>Generation/CardPen/js/main.js:1455-1470</c>) and take no
		/// data expression, which is why they are listed here instead of being read as variables.
		/// </summary>
		private static readonly HashSet<string> BlockHelpers = new HashSet<string>(StringComparer.Ordinal)
		{
			"if", "each", "ifFamilyHeader", "ifLeafFirst", "ifSubfamilyHeader"
		};

		/// <summary>
		/// Handlebars inline helpers (census: <c>ifCond</c> 13, <c>breaklines</c> 21, <c>markdown</c> 7).
		/// Their first argument is data; their remaining arguments may be quoted literals, which the
		/// classifier skips.
		/// </summary>
		private static readonly HashSet<string> InlineHelpers = new HashSet<string>(StringComparer.Ordinal)
		{
			"ifCond", "breaklines", "markdown"
		};

		/// <summary>
		/// Values CardPen injects at render time, i.e. present in no CSV.
		/// <c>cardIndex</c> is set per card (<c>main.js:1473</c>); <c>rowset</c> is the grouping
		/// object CardPen builds from the parsed CSV, addressable as <c>rowset.N.field</c> or
		/// <c>rowset.[N].field</c> — measured as the only other unresolvable names on the live
		/// templates (Memo face/back, Scenarii back).
		/// </summary>
		private static readonly HashSet<string> RuntimeBuiltIns = new HashSet<string>(StringComparer.Ordinal)
		{
			"cardIndex", "rowset"
		};

		[Fact]
		public void Every_Card_Template_Variable_Resolves_Against_Its_Data()
		{
			var config = new AssetConverterConfig();
			var unresolved = new List<string>();

			foreach (var (path, mustache, columns) in CardTemplates(config))
			{
				foreach (var name in VariablesIn(mustache))
				{
					if (!Resolves(name, columns))
						unresolved.Add($"{Relative(path)} → {{{{ {name} }}}}");
				}
			}

			unresolved.Should().BeEmpty(
				"a mustache variable that neither the CardSet's DataSet CSV nor the template's own " +
				"embedded csv carries, and that CardPen does not inject, is rendered literally on the " +
				"card — the #216/#1141 family. Phase B source: template × CSV, never the PDF (0 text " +
				"layer) and never the .harvest.json (pixels only). Unresolved: " + string.Join(" · ", unresolved));
		}

		[Fact]
		public void Card_Template_Mustache_Blocks_Are_Balanced()
		{
			var config = new AssetConverterConfig();
			var unbalanced = new List<string>();

			foreach (var (path, mustache, _) in CardTemplates(config))
			{
				var opens = Regex.Matches(mustache, @"\{\{#if\b").Count;
				var closes = Regex.Matches(mustache, @"\{\{/if\b").Count;
				if (opens != closes)
					unbalanced.Add($"{Relative(path)} → {opens} {{{{#if}}}} vs {closes} {{{{/if}}}}");

				var eachOpens = Regex.Matches(mustache, @"\{\{#each\b").Count;
				var eachCloses = Regex.Matches(mustache, @"\{\{/each\b").Count;
				if (eachOpens != eachCloses)
					unbalanced.Add($"{Relative(path)} → {eachOpens} {{{{#each}}}} vs {eachCloses} {{{{/each}}}}");
			}

			unbalanced.Should().BeEmpty(
				"an unclosed {{#if}} swallows the rest of the template, so the card renders a prefix and " +
				"silently loses every field after it. Measured 06/11: the corpus is balanced (69/69 {{#if}}), " +
				"so this guard is a floor, not a fix. Unbalanced: " + string.Join(" · ", unbalanced));
		}

		[Fact]
		public void The_Template_Scan_Is_Not_Vacuous()
		{
			var config = new AssetConverterConfig();
			var templates = CardTemplates(config).ToList();

			foreach (var t in templates)
				_output.WriteLine($"{Relative(t.Path)} | {t.Columns.Count} colonnes | {VariablesIn(t.Mustache).Count} références");

			// Anti-collapse floor. A resolved path that silently stops resolving would let the two
			// tests above pass over an empty set — the "0 defects because 0 inputs" mode.
			// MEASURED on master 33f25e82 (06/11): 11 distinct live templates, 101 variable
			// references (per-template inventory printed above). The floors sit below both and are
			// floors, not equalities: adding or retiring a CardSet must not make this test red for
			// the wrong reason.
			templates.Should().HaveCountGreaterThanOrEqualTo(8,
				"the scan must reach the live card templates of the configured CardSets");

			var variableRefs = templates.Sum(t => VariablesIn(t.Mustache).Count);
			variableRefs.Should().BeGreaterThanOrEqualTo(80,
				$"the scan must actually read mustache expressions; saw {variableRefs}");
		}

		[Fact]
		public void A_Renamed_Column_Is_Detected_Mutation_Of_A_Real_Template()
		{
			// Inverse control, run against a REAL template rather than a hand-written fixture: take
			// the first resolvable variable of the largest template, rename it to a field no data
			// source carries, and require the classifier to report it. This is the mutation the
			// dispatch asks for, kept in the suite so it cannot rot into a transcribed affirmation.
			var config = new AssetConverterConfig();

			var target = CardTemplates(config)
				.Select(t => new { t.Mustache, t.Columns })
				.OrderByDescending(t => t.Mustache.Length)
				.First();

			// The probe must RESOLVE today, otherwise the mutation proves nothing.
			var probe = VariablesIn(target.Mustache).First(n => Resolves(n, target.Columns));
			var ghost = "zz_colonne_absente_zz";

			var mutated = Regex.Replace(target.Mustache, @"\{\{\s*" + Regex.Escape(probe) + @"\s*\}\}",
				"{{" + ghost + "}}");

			mutated.Should().NotBe(target.Mustache, "the mutation must actually rewrite the template");

			var reported = VariablesIn(mutated).Where(n => !Resolves(n, target.Columns)).ToList();
			reported.Should().Contain(ghost,
				"the guard must flag a variable the data cannot fill — otherwise it is a transcription, " +
				"not a detector");
		}

		[Fact]
		public void Wrapper_Templates_Leave_No_Token_After_The_Production_Formatter()
		{
			var root = TestRepoRoot.Find();
			var mindmaps = Path.Combine(root, "Cards", "Fallacies", "Mindmaps");
			var templates = new[] { "included.html", "external.html" }
				.Select(f => Path.Combine(mindmaps, f))
				.Where(File.Exists)
				.ToList();

			templates.Should().HaveCount(2,
				"the two root wrapper templates (outside the <lang>/ directories) are the contract");

			var residues = new List<string>();
			foreach (var path in templates)
			{
				var template = File.ReadAllText(path);

				// The raw template MUST carry tokens, otherwise the zero below would be an absence
				// the instrument could not have seen (a 0 is only an absence if it could have been a 1).
				WrapperToken.Matches(template).Count.Should().BeGreaterThan(0,
					$"'{Relative(path)}' must carry at least one token — otherwise this test's zero proves nothing");

				var rendered = MindMapHtmlWrapper.FormatWrapper(
					template,
					svgRelativePath: "__SVG_PATH__",
					svgContent: "__SVG_CONTENT__",
					language: "xx",
					title: "__TITLE__");

				foreach (Match m in WrapperToken.Matches(rendered))
					residues.Add($"{Relative(path)} → {m.Value}");
			}

			residues.Should().BeEmpty(
				"FormatWrapper consumes [LANGUAGE], [DIRECTION], [TITLE], [SVGPATH] and [SVGCONTENT]; any " +
				"other bracket token reaching the committed wrapper would be published verbatim. Residues: "
				+ string.Join(" · ", residues));
		}

		[Fact]
		public void An_Unconsumed_Token_Is_Detected_Mutation_Of_The_Wrapper_Formatter()
		{
			// Inverse control for the wrapper half: a token the production formatter does not know
			// must survive it and be reported. Without this case the guard above would pass equally
			// if FormatWrapper blindly stripped everything.
			var template = "<html lang=\"[LANGUAGE]\"><title>[TITLE]</title>[SVGCONTENT]</html>";

			var rendered = MindMapHtmlWrapper.FormatWrapper(template, "__P__", "__C__", "fr", "__T__");
			WrapperToken.Matches(rendered).Count.Should().Be(0,
				"the known tokens must all be consumed by the production formatter");

			var mutated = template.Replace("[SVGCONTENT]", "[SVGCONTENT][ZZ_UNKNOWN_TOKEN]");
			var survivors = WrapperToken.Matches(
					MindMapHtmlWrapper.FormatWrapper(mutated, "__P__", "__C__", "fr", "__T__"))
				.Select(m => m.Value).ToList();

			survivors.Should().Contain("[ZZ_UNKNOWN_TOKEN]",
				"a token added to a template without a matching substitution must be caught — the formatter " +
				"applies a fixed list, it does not validate");
		}

		// ────────────────────────────── helpers ──────────────────────────────

		private static IEnumerable<(string Path, string Mustache, ISet<string> Columns)> CardTemplates(
			AssetConverterConfig config)
		{
			var dataSets = config.DataSets
				.Where(d => d != null && !string.IsNullOrWhiteSpace(d.Name))
				.GroupBy(d => d.Name, StringComparer.Ordinal)
				.ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

			var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			foreach (var set in config.WebBasedGeneratorConfig.CardSets.Where(c => c != null && c.IsEnabled))
			{
				foreach (var info in new[] { set.FaceCardSetInfo, set.BackCardSetInfo }.Where(i => i != null))
				{
					var debugPath = info.JsonFilePathDebug;
					if (string.IsNullOrWhiteSpace(debugPath)) continue;

					var resolved = ResolveRepoRelative(debugPath);
					if (resolved == null || !File.Exists(resolved)) continue;
					if (!seen.Add(resolved)) continue;

					string mustache;
					string embeddedCsv;
					try
					{
						using var doc = JsonDocument.Parse(File.ReadAllText(resolved));
						mustache = doc.RootElement.TryGetProperty("mustache", out var m) && m.ValueKind == JsonValueKind.String
							? m.GetString() ?? string.Empty
							: string.Empty;
						// The embedded csv is the STALE snapshot for sets that declare a DataSet
						// (`_csv_note`, #813/#812) — but it is the ONLY data source for the back
						// CardSets, which declare DataSet = KnownDataSets.None ("" → no CSV file).
						// Measured 06/11: Argumentum_Fallacies_Back_fr.json carries a 2-line embedded
						// csv whose header is `tagline_fr,…,tagline_zh`, and no corpus CSV has a
						// tagline column at all. Reading it here is therefore required, not tolerated.
						embeddedCsv = doc.RootElement.TryGetProperty("csv", out var c) && c.ValueKind == JsonValueKind.String
							? c.GetString() ?? string.Empty
							: string.Empty;
					}
					catch (JsonException)
					{
						continue;
					}

					var columns = new HashSet<string>(StringComparer.Ordinal);
					foreach (var h in HeaderOf(embeddedCsv)) columns.Add(h);

					if (!string.IsNullOrWhiteSpace(info.DataSet)
					    && dataSets.TryGetValue(info.DataSet, out var ds)
					    && !string.IsNullOrWhiteSpace(ds.DebugFilePath))
					{
						var csvPath = ResolveRepoRelative(ds.DebugFilePath);
						if (csvPath != null && File.Exists(csvPath))
						{
							foreach (var h in HeaderOf(File.ReadAllText(csvPath))) columns.Add(h);
						}
					}

					yield return (resolved, mustache, columns);
				}
			}
		}

		/// <summary>
		/// First CSV record, quote-aware. Deliberately not <c>line.Split(',')</c>: a quoted header
		/// containing a comma would silently split one column into two and make the resolution
		/// check pass for a field that does not exist.
		/// </summary>
		private static IEnumerable<string> HeaderOf(string csv)
		{
			if (string.IsNullOrEmpty(csv)) yield break;

			var line = new List<char>();
			var inQuotes = false;
			foreach (var ch in csv)
			{
				if (ch == '"') { inQuotes = !inQuotes; line.Add(ch); continue; }
				if (!inQuotes && (ch == '\n' || ch == '\r')) break;
				line.Add(ch);
			}

			var fields = new List<string>();
			var current = new System.Text.StringBuilder();
			inQuotes = false;
			foreach (var ch in line)
			{
				if (ch == '"') { inQuotes = !inQuotes; continue; }
				if (ch == ',' && !inQuotes) { fields.Add(current.ToString().Trim()); current.Clear(); continue; }
				current.Append(ch);
			}
			fields.Add(current.ToString().Trim());

			foreach (var f in fields.Where(f => f.Length > 0))
				yield return f;
		}

		/// <summary>
		/// Every data lookup a mustache body performs. Block openers contribute their expression,
		/// closers and <c>else</c> contribute nothing, inline helpers contribute their non-literal
		/// arguments.
		/// </summary>
		private static List<string> VariablesIn(string mustache)
		{
			var found = new List<string>();
			if (string.IsNullOrEmpty(mustache)) return found;

			foreach (Match m in MustacheToken.Matches(mustache))
			{
				var inner = m.Groups[1].Value.Trim();
				if (inner.Length == 0) continue;

				var sigil = inner[0];
				var body = inner.TrimStart('#', '/', '^').Trim();
				if (body.Length == 0 || body == "else") continue;
				if (sigil == '/') continue;

				var parts = body.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
				var head = parts[0];

				if (sigil == '#')
				{
					if (head == "if" || head == "each")
					{
						// The tested / iterated expression is the first argument.
						if (parts.Length > 1) found.Add(Unquote(parts[1]));
					}
					else if (head == "ifCond" || InlineHelpers.Contains(head))
					{
						// Block form of an inline helper. Measured 06/11 on
						// Cards/Memo/Argumentum_Memo_Face_fr.json: `{{#ifCond Famille "==" text_fr }}`
						// — two data operands and one quoted operator, so the identifier filter is
						// what separates `"=="` from `Famille`/`text_fr`.
						for (var i = 1; i < parts.Length; i++)
						{
							var arg = Unquote(parts[i]);
							if (IsBareIdentifier(arg)) found.Add(arg);
						}
					}
					else if (!BlockHelpers.Contains(head))
					{
						// Unknown block helper: report the head rather than assume it is a helper.
						found.Add(head);
					}
					continue;
				}

				if (InlineHelpers.Contains(head))
				{
					for (var i = 1; i < parts.Length; i++)
					{
						var arg = Unquote(parts[i]);
						if (IsBareIdentifier(arg)) found.Add(arg);
					}
					continue;
				}

				found.Add(head);
			}

			return found;
		}

		private static bool Resolves(string name, ISet<string> columns) =>
			RuntimeBuiltIns.Contains(name)
			|| name.StartsWith("rowset.", StringComparison.Ordinal)
			|| columns.Contains(name);

		private static string Unquote(string s) =>
			s.Length >= 2 && (s[0] == '"' || s[0] == '\'') && s[s.Length - 1] == s[0]
				? s.Substring(1, s.Length - 2)
				: s;

		private static bool IsBareIdentifier(string s) =>
			Regex.IsMatch(s, @"^[A-Za-z_][A-Za-z0-9_.\[\]]*$");

		/// <summary>
		/// Resolves a config path to a repository file. The pipeline's Debug paths are relative to
		/// the converter's own <c>bin/</c> directory (<c>..\..\..\..\..\..\Cards\…</c>), whose depth
		/// differs from the test assembly's — so the segment count is NOT reinterpreted. The path is
		/// re-anchored at its <c>Cards</c> segment under the located repository root, which is the
		/// only stable part of the contract.
		/// </summary>
		private static string? ResolveRepoRelative(string configPath)
		{
			var normalized = configPath.Replace('\\', '/');
			var idx = normalized.IndexOf("Cards/", StringComparison.OrdinalIgnoreCase);
			if (idx < 0) return null; // an absolute URL (Release path) is not a local template

			var tail = normalized.Substring(idx).Replace('/', Path.DirectorySeparatorChar);
			return Path.Combine(TestRepoRoot.Find(), tail);
		}

		private static string Relative(string absolute)
		{
			var root = TestRepoRoot.Find();
			return absolute.StartsWith(root, StringComparison.OrdinalIgnoreCase)
				? absolute.Substring(root.Length).TrimStart(Path.DirectorySeparatorChar, '/')
				: absolute;
		}
	}
}
