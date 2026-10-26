using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Contract of the #458 W13 follow-up (web2 contre-revue of C1, 2026-10-23): the two date
	/// bounds of the News list are read through a reader that cannot throw.
	/// <para>
	/// The hole: <c>IsVisibleNow</c> used to read the bounds with an unconditional cast of a
	/// <c>dynamic</c>. If a field ever arrives as a string — the shape the retired string filter
	/// tolerated — the cast raises <c>RuntimeBinderException</c> inside the view and takes the
	/// whole list page down. Measured 2026-10-25, read only, on <c>TsDynDataAttribute</c>: both
	/// News content types (140 and 325) declare <c>ShowFrom</c> and <c>ShowTo</c> as
	/// <c>DateTime</c>, so the cast cannot fail <em>today</em>. The hardening is therefore
	/// defence in depth, and its contract is a shape contract, not a behaviour one: this file
	/// cannot execute <c>Helpers.cs</c> (it derives from a 2sxc base class that is net48 and
	/// not referenced by this project), so it pins the SOURCE shape that makes the failure
	/// unreachable, and proves the detectors see the pre-fix shape.
	/// </para>
	/// <para>
	/// The culture leg is the other half. C1 removed a culture-DEPENDENT parse; re-introducing
	/// one in the defensive branch would restore the defect (a Persian rendering parsed as Latin
	/// used to empty <c>/fa/Actus</c>), so the only accepted provider is the invariant culture.
	/// </para>
	/// </summary>
	public class News5DateBoundGuardTests
	{
		private const string IsVisibleNowSignature = "public bool IsVisibleNow(dynamic article)";
		private const string ReaderSignature = "private static DateTime? NewsDateBound(object value)";

		private static readonly string HelpersPath = Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "Portals", "1", "2sxc", "News5", "shared", "Helpers.cs");

		[Fact]
		public void TheNewsListReadsBothBoundsThroughTheTolerantReader()
		{
			var body = BodyOf(File.ReadAllText(HelpersPath), IsVisibleNowSignature);

			body.Should().NotBeEmpty("the IsVisibleNow signature moved or was renamed — the guard would assert on nothing");

			ReaderCalls(body).Should().Be(2,
				"both bounds (ShowFrom and ShowTo) must go through NewsDateBound, and only those two");

			CastsToDateTime(body).Should().BeFalse(
				"an unconditional (DateTime) cast of a dynamic raises RuntimeBinderException and takes the whole list page down");

			body.Should().NotContain("DateTime.MinValue",
				"clamping an unreadable bound to MinValue empties the list — that is the C1 failure family");

			Regex.IsMatch(body, @"NewsDateBound\s*\(\s*article\.").Should().BeFalse(
				"passing article.<field> straight into the reader makes the call itself dynamic; read into an object local first");
		}

		[Fact]
		public void TheReaderAcceptsEveryShapeAndNeverFailsTheBinder()
		{
			var body = BodyOf(File.ReadAllText(HelpersPath), ReaderSignature);

			body.Should().NotBeEmpty("the NewsDateBound signature moved or was renamed — the guard would assert on nothing");

			body.Should().Contain("value == null", "an absent bound is legal and means 'no bound'");
			body.Should().Contain("is DateTime", "the shipped shape is a DateTime");
			body.Should().Contain("is DateTimeOffset", "a DateTimeOffset bound must not reach the binder either");
			body.Should().Contain("as string", "the string shape is the one the retired filter tolerated");

			body.Should().NotContain("DateTime.MinValue",
				"an unreadable value must read as absent (article stays visible), never as a clamp to MinValue");
		}

		[Fact]
		public void TheReaderParsesWithTheInvariantCultureOnly()
		{
			var body = BodyOf(File.ReadAllText(HelpersPath), ReaderSignature);

			var parses = ParseCalls(body);
			parses.Should().NotBeEmpty("the string shape must be attempted, otherwise the tolerance is decorative");

			foreach (var arguments in parses)
			{
				arguments.Count.Should().Be(4,
					"a TryParse overload without an IFormatProvider is the culture-DEPENDENT parse that C1 removed");
				arguments[1].Should().Be("CultureInfo.InvariantCulture",
					"the defensive branch must not re-introduce a culture-dependent parse");
			}
		}

		[Fact]
		public void TheGuardSeesThePreFixShapes()
		{
			// 1. the pre-fix IsVisibleNow: unconditional casts, no reader at all
			var preFix = GetText(
				"{",
				"    var now = DateTime.Now;",
				"    if(article.ShowFrom != null && ((DateTime)article.ShowFrom) > now) { return false; }",
				"    if(article.ShowTo != null && ((DateTime)article.ShowTo) <= now) { return false; }",
				"    return true;",
				"}");
			CastsToDateTime(preFix).Should().BeTrue("the guard must SEE the shape it forbids");
			ReaderCalls(preFix).Should().Be(0);

			// 2. a clamp instead of a tolerance — the C1 outage shape
			var clamped = GetText("{", "    return DateTime.MinValue;", "}");
			clamped.Should().Contain("DateTime.MinValue");

			// 3. the culture-dependent overload creeping back into the reader
			var dependent = ParseCalls(GetText("{", "    DateTime.TryParse(text, out parsed);", "}"));
			dependent.Should().ContainSingle("the extractor must find the 2-argument form");
			dependent[0].Count.Should().Be(2, "a 2-argument TryParse has no IFormatProvider, so the guard rejects it");

			// 4. control in the other direction: the invariant form is accepted
			var invariant = ParseCalls(GetText(
				"{", "    DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed);", "}"));
			invariant.Should().ContainSingle();
			invariant[0][1].Should().Be("CultureInfo.InvariantCulture");
		}

		private static string GetText(params string[] lines) => string.Join(Environment.NewLine, lines);

		/// <summary>
		/// Returns the body of the method declared by <paramref name="signature"/>: the text between
		/// its opening brace and the matching closing brace. Line and block comments are stripped
		/// first, so a future comment that documents the forbidden shape cannot turn the guard red
		/// — only live code counts. Only full-line <c>//</c> comments are cut, so a URL inside a
		/// string literal is never truncated.
		/// </summary>
		internal static string BodyOf(string source, string signature)
		{
			var code = Regex.Replace(source, @"/\*.*?\*/", " ", RegexOptions.Singleline);
			code = Regex.Replace(code, @"(?m)^\s*//.*$", " ");

			var start = code.IndexOf(signature, StringComparison.Ordinal);
			if (start < 0) { return string.Empty; }
			var open = code.IndexOf('{', start);
			if (open < 0) { return string.Empty; }

			var depth = 0;
			for (var i = open; i < code.Length; i++)
			{
				if (code[i] == '{') { depth++; }
				else if (code[i] == '}')
				{
					depth--;
					if (depth == 0) { return code.Substring(open + 1, i - open - 1); }
				}
			}
			return string.Empty;
		}

		internal static bool CastsToDateTime(string body) => Regex.IsMatch(body, @"\(\s*DateTime\s*\)");

		internal static int ReaderCalls(string body) => Regex.Matches(body, @"NewsDateBound\s*\(").Count;

		/// <summary>
		/// Returns the argument list of every <c>TryParse(</c> call in <paramref name="body"/>,
		/// split on top-level commas (so an argument that itself contains a call stays one item).
		/// </summary>
		internal static List<List<string>> ParseCalls(string body)
		{
			var calls = new List<List<string>>();
			foreach (Match match in Regex.Matches(body, @"TryParse\s*\("))
			{
				var open = match.Index + match.Length - 1;
				var depth = 0;
				var close = -1;
				for (var i = open; i < body.Length; i++)
				{
					if (body[i] == '(') { depth++; }
					else if (body[i] == ')')
					{
						depth--;
						if (depth == 0) { close = i; break; }
					}
				}
				if (close < 0) { continue; }

				var arguments = new List<string>();
				var current = new System.Text.StringBuilder();
				var nesting = 0;
				for (var i = open + 1; i < close; i++)
				{
					var c = body[i];
					if (c == '(') { nesting++; }
					if (c == ')') { nesting--; }
					if (c == ',' && nesting == 0)
					{
						arguments.Add(current.ToString().Trim());
						current.Clear();
						continue;
					}
					current.Append(c);
				}
				var last = current.ToString().Trim();
				if (last.Length > 0) { arguments.Add(last); }
				calls.Add(arguments);
			}
			return calls;
		}
	}
}
