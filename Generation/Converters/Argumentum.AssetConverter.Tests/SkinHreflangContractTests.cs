using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Contract of the #458 hreflang grain: the served skin must emit, for every culture the PORTAL
	/// serves, a <c>&lt;link rel="alternate" hreflang&gt;</c>, plus a self-referencing
	/// <c>&lt;link rel="canonical"&gt;</c> and an <c>x-default</c> pointing at the portal default
	/// language.
	///
	/// Why it exists: this DNN emits none of it — measured 2026-10-25 on 20 routes (preprod + prod),
	/// 0 occurrence, and no compiled emitter in the webroot or in any bin assembly. The skin is the
	/// only emitter, so nothing else can regress it silently. Decision 2 of #458 fixes the shape:
	/// languages derived from the portal (never hardcoded), <c>x-default</c> = portal default,
	/// canonical self-referencing, and prod only at go-live.
	///
	/// The predicates below are pure functions of the emitter's source text, and each is exercised
	/// twice: once on the real file (must hold) and once on a mutated copy of that same text (must
	/// break). A predicate that matched nothing would read as a pass on the real file and would keep
	/// reading as a pass on its mutant — the mutation is what distinguishes the two.
	/// </summary>
	public class SkinHreflangContractTests
	{
		private static readonly string SkinFile = Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "Portals", "_default", "Skins", "2shineBS5", "controls", "theme-body.ascx");

		private const string EmitterStart = "private void EmitLanguageAlternates()";
		private const string EmitterEnd = "protected string ResourceFilePath";

		/// <summary>
		/// The emitter block only. The file also carries the visible switcher's
		/// <c>Languages="fr-FR:FR,…"</c> attribute, which lists 5 cultures on purpose (zh/ar/fa are
		/// gated until the minimal chrome ships) — an assertion scoped to the whole file would read
		/// that legitimately hardcoded menu as a violation, or worse, satisfy a "languages are
		/// declared" check it has nothing to do with.
		/// </summary>
		private static string EmitterBlock()
		{
			var text = File.ReadAllText(SkinFile);
			var start = text.IndexOf(EmitterStart, StringComparison.Ordinal);
			Assert.True(start >= 0, "EmitLanguageAlternates() not found in " + SkinFile);
			var end = text.IndexOf(EmitterEnd, start, StringComparison.Ordinal);
			Assert.True(end > start, "could not delimit the emitter block in " + SkinFile);
			var block = text.Substring(start, end - start);
			Assert.True(block.Length > 500, "emitter block looks truncated: " + block.Length + " chars");
			return block;
		}

		private static bool EmitsAlternate(string block) =>
			block.Contains("rel=\\\"alternate\\\"", StringComparison.Ordinal)
			&& block.Contains("hreflang=\\\"", StringComparison.Ordinal);

		private static bool EmitsCanonical(string block) =>
			block.Contains("rel=\\\"canonical\\\"", StringComparison.Ordinal);

		private static bool EmitsXDefault(string block) =>
			block.Contains("hreflang=\\\"x-default\\\"", StringComparison.Ordinal);

		/// <summary>
		/// The culture set comes from the portal, and no culture literal is enumerated: a hardcoded
		/// pair such as <c>"pt-BR","ru-RU"</c> would silently stop tracking the portal the day a
		/// culture is added or retired, which is exactly the failure this grain exists to avoid.
		/// All 8 portal culture codes are banned — the emitter block carries no legitimate literal,
		/// so a partial ban is only a hole (measured in review: a 3-code ban let 5 of 8 through).
		/// </summary>
		private static bool DerivesLanguagesFromPortal(string block) =>
			block.Contains("GetLocales(", StringComparison.Ordinal)
			&& block.Contains("portal.PortalId", StringComparison.Ordinal)
			&& !BannedCultureLiteralSuffixes.Any(suffix => block.Contains(suffix, StringComparison.Ordinal));

		private static readonly string[] BannedCultureLiteralSuffixes =
		{
			"-FR\"", "-US\"", "-ES\"", "-BR\"", "-RU\"", "-CN\"", "-SA\"", "-IR\"",
		};

		private static bool XDefaultUsesPortalDefault(string block) =>
			block.Contains("portal.DefaultLanguage", StringComparison.Ordinal);

		/// <summary>
		/// The canonical target is a value computed at request time. A literal would name one fixed
		/// page for every page of the site — the classic way a canonical does more harm than its
		/// absence. Asserted exactly where it is decidable: the C# string literal that carries
		/// <c>href=</c> must close immediately, leaving the target to an <c>.Append(…)</c>.
		/// </summary>
		private static bool CanonicalHrefIsComputed(string block)
		{
			const string marker = "rel=\\\"canonical\\\" href=\\\"";
			var at = block.IndexOf(marker, StringComparison.Ordinal);
			if (at < 0) { return false; }
			var after = at + marker.Length;
			return block.Length > after + 1 && block[after] == '"' && block[after + 1] == ')';
		}

		/// <summary>
		/// Applies a mutation to the real emitter text. The mutation is asserted to apply: a mutation
		/// that silently did nothing would leave the invariant holding and read as "the control
		/// passed", which is the exact way an inverse control can lie.
		/// </summary>
		private static string Mutate(string block, string from, string to)
		{
			Assert.Contains(from, block, StringComparison.Ordinal);
			return block.Replace(from, to);
		}

		[Fact]
		public void TheServedSkinEmitsAlternatesCanonicalAndXDefault()
		{
			var block = EmitterBlock();
			Assert.True(EmitsAlternate(block), "the emitter does not write rel=\"alternate\" hreflang links");
			Assert.True(EmitsCanonical(block), "the emitter does not write a rel=\"canonical\" link");
			Assert.True(EmitsXDefault(block), "the emitter does not write an x-default alternate");
		}

		[Fact]
		public void LanguagesAreDerivedFromThePortalAndNeverHardcoded()
		{
			var block = EmitterBlock();
			Assert.True(DerivesLanguagesFromPortal(block),
				"the emitter must read the portal's locales (LocaleController.GetLocales) and must not enumerate culture literals");
			Assert.True(XDefaultUsesPortalDefault(block),
				"x-default must target portal.DefaultLanguage, never a literal culture");
			Assert.True(CanonicalHrefIsComputed(block),
				"the canonical target must be computed per request, never a literal URL");
		}

		[Fact]
		public void TheAlternatesAreAddedToThePageHeadAndGuardedAgainstFailure()
		{
			var block = EmitterBlock();
			Assert.Contains("FindControl(\"Head\")", block, StringComparison.Ordinal);
			Assert.Contains("head.Controls.Add(", block, StringComparison.Ordinal);
			// SEO only: an exception here must never take the page down.
			Assert.Contains("catch", block, StringComparison.Ordinal);
			// One emission per request, whatever the render path.
			Assert.Contains("_alternatesEmitted", block, StringComparison.Ordinal);
		}

		// --- inverse controls: each predicate must go RED on a mutated copy of the real text ---

		[Fact]
		public void WithoutTheXDefaultLineTheXDefaultCheckFails()
		{
			var mutated = Mutate(EmitterBlock(), "hreflang=\\\"x-default\\\"", "hreflang=\\\"default\\\"");
			Assert.True(EmitsAlternate(mutated), "control is broken: the mutation also removed the alternates");
			Assert.False(EmitsXDefault(mutated), "the x-default check passed on text with no x-default — it is vacuous");
		}

		[Fact]
		public void WithoutTheCanonicalLinkTheCanonicalCheckFails()
		{
			var mutated = Mutate(EmitterBlock(), "rel=\\\"canonical\\\"", "rel=\\\"prefetch\\\"");
			Assert.False(EmitsCanonical(mutated), "the canonical check passed on text with no canonical — it is vacuous");
		}

		[Fact]
		public void WithAHardcodedCultureListThePortalDerivationCheckFails()
		{
			// pt-BR/ru-RU: the pair that escaped the earlier 3-code ban (review-observed) — this
			// control bites only because all 8 codes are now banned, proving the hole is closed.
			var mutated = Mutate(EmitterBlock(), "locales.Keys", "new[] { \"pt-BR\", \"ru-RU\" }");
			Assert.False(DerivesLanguagesFromPortal(mutated),
				"the hardcoded-culture check passed on a literal culture list — it is vacuous");
		}

		[Fact]
		public void WithALiteralXDefaultCultureThePortalDefaultCheckFails()
		{
			var mutated = Mutate(EmitterBlock(), "portal.DefaultLanguage", "\"fr-FR\"");
			Assert.False(XDefaultUsesPortalDefault(mutated),
				"the x-default source check passed on a literal culture — it is vacuous");
		}

		[Fact]
		public void WithALiteralCanonicalUrlTheComputedTargetCheckFails()
		{
			var mutated = Mutate(EmitterBlock(),
				"rel=\\\"canonical\\\" href=\\\"\"", "rel=\\\"canonical\\\" href=\\\"https://dnn.argumentum.myia.io/\\\"");
			Assert.False(CanonicalHrefIsComputed(mutated),
				"the computed-canonical check passed on a literal URL — it is vacuous");
		}
	}
}
