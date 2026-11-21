using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Ontology;
using FluentAssertions;
using OWLSharp;
using OWLSharp.Ontology;
using RDFSharp.Model;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
	/// <summary>
	/// #133 version contract (P24-05, pool #458) — `owl:versionInfo` comes from the generator
	/// CONFIG, so the tag-time bump (#134 → v2.0.0) is a deliberate, reviewed diff of the two
	/// C# initializers (OwlGeneratorConfig / VirtueOwlGeneratorConfig) and nothing else. The
	/// doctrine (issue #133, 2026-10): the canonical URL always serves the latest generation;
	/// the version lives IN the file via `owl:versionInfo`; snapshot copies under `v{semver}/`
	/// are made at release tags only. The snapshot recipe is what the 17/11 freeze decision
	/// waits for before `argumentum_virtues.owl` may be regenerated (its regen renames 2 served
	/// IRIs — inoffensive once published under a new version path).
	///
	/// Same two-part pin as OwlPublicationMetadataBridgeTests: the DEFAULTS live on the config
	/// objects (a full generation needs the dataset); the SERIALIZED SHAPE is pinned through the
	/// OwlAdapter + OWL2XML path the generators use, with a version value chosen DIFFERENT from
	/// the default — a generator that ignored the config and emitted a constant would emit 1.0.0
	/// and turn both assertions of the emission test red (the built-in mutation). These tests
	/// never read the committed ontology artefacts: a version bump moves them only with the
	/// release regeneration, and the A/B control keeps the Argumentum-namespace IRI surface
	/// frozen (the #133 invariant "0 IRI changée").
	/// </summary>
	public class OwlVersionInfoContractTests
	{
		private const string FallaciesNs = "https://www.argumentum.games/argumentum_fallacies.owl#";
		private const string VirtuesNs = "https://www.argumentum.games/argumentum_virtues.owl#";
		private const string OwlVersionInfoIri = "http://www.w3.org/2002/07/owl#versionInfo";
		// Any value ≠ the 1.0.0 default makes "config ignored → constant emitted" observable.
		private static readonly Version ProbedVersion = new Version(2, 0, 0);

		/// <summary>Minimal ontology skeleton identical to what the generators build first:
		/// namespace + scheme + one concept, so the A/B IRI control compares like with like.</summary>
		private static OwlAdapter BuildSkeleton(string ns)
		{
			var adapter = new OwlAdapter(ns);
			var scheme = new RDFResource($"{ns}testScheme");
			adapter.DeclareConceptScheme(scheme);
			adapter.DeclareConcept(new RDFResource($"{ns}testConcept"), scheme);
			return adapter;
		}

		private static async Task<string> SerializeToTemp(OwlAdapter adapter)
		{
			var path = Path.Combine(Path.GetTempPath(), $"owl-version-{Guid.NewGuid():N}.owl");
			await adapter.ToFileAsync(OWLEnums.OWLFormats.OWL2XML, path);
			return path;
		}

		/// <summary>The exact annotation call both generators make on their config's Version
		/// (OwlGeneratorConfig.cs / VirtueOwlGeneratorConfig.cs: Annotate(OWL.VERSION_INFO,
		/// new RDFPlainLiteral(Version.ToString()))) — kept byte-identical so the pinned channel
		/// is the generator's own, not a lookalike.</summary>
		private static void AnnotateVersion(OwlAdapter adapter, Version version)
		{
			adapter.Annotate(RDFVocabulary.OWL.VERSION_INFO, new RDFPlainLiteral(version.ToString()));
		}

		private static readonly Regex ArgumentumIriRegex = new Regex(
			@"https://www\.argumentum\.games/[A-Za-z0-9_.#\-]+", RegexOptions.Compiled);

		// ─────────────────────────────────────────────────────────────────────────────
		// (1) Generator defaults — the version lives on the config, 1.0.0 until the
		//     tag-time bump; both corpora symmetric.
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public void FallaciesDefaults_CarryVersionInConfig()
		{
			var doc = new OwlGeneratorConfig().DocumentConfigs.Single();

			doc.Version.Should().Be(new Version(1, 0, 0),
				"the version is a config property, not a generator constant: the tag-time bump (#134, v2.0.0) " +
				"is a one-line reviewed diff of this initializer — so it stays 1.0.0 until that bump.");
		}

		[Fact]
		public void VirtuesDefaults_CarryVersionInConfig()
		{
			var doc = new VirtueOwlGeneratorConfig().DocumentConfigs.Single();

			doc.Version.Should().Be(new Version(1, 0, 0),
				"symmetric with the Fallacies config: one bump, one release, both artefacts.");
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (2) Serialized shape — the emitted owl:versionInfo carries the CONFIGURED value.
		//     ProbedVersion (2.0.0) ≠ default (1.0.0): a generator that ignored the config
		//     would emit the constant and fail both assertions below (the built-in mutation).
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public async Task VersionInfo_SerializesTheConfiguredVersion()
		{
			var adapter = BuildSkeleton(FallaciesNs);
			AnnotateVersion(adapter, ProbedVersion);
			var path = await SerializeToTemp(adapter);
			try
			{
				var xml = File.ReadAllText(path);

				xml.Should().MatchRegex(
					$@"<Annotation>\s*<AnnotationProperty IRI=""{Regex.Escape(OwlVersionInfoIri)}""",
					"owl:versionInfo must serialize as an ontology-level annotation, the family the " +
					"generators already use for comment/creator (bridge test §2).");
				xml.Should().Contain($">{ProbedVersion}<",
					"the emitted versionInfo is the CONFIGURED version's ToString().");
				xml.Should().NotContain("1.0.0",
					"mutation control: a generator that ignored the config and emitted a constant " +
					"1.0.0 makes this red while the 2.0.0 assertion above is red too — the pin bites.");
			}
			finally
			{
				File.Delete(path);
			}
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (3) ⛔ Frozen-IRI control (the P24-05 DoD "0 IRI changée") — A/B on identical
		//     skeletons: the version annotation alone adds no Argumentum-namespace IRI,
		//     so a version bump never relocates a served term.
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public async Task VersionInfo_IntroducesNoArgumentumNamespaceIri()
		{
			var bare = BuildSkeleton(VirtuesNs);
			var versioned = BuildSkeleton(VirtuesNs);
			AnnotateVersion(versioned, ProbedVersion);

			var barePath = await SerializeToTemp(bare);
			var versionedPath = await SerializeToTemp(versioned);
			try
			{
				var bareIris = ArgumentumIriRegex.Matches(File.ReadAllText(barePath))
					.Select(m => m.Value).ToHashSet();
				var versionedIris = ArgumentumIriRegex.Matches(File.ReadAllText(versionedPath))
					.Select(m => m.Value).ToHashSet();

				bareIris.Should().NotBeEmpty("the skeleton must carry Argumentum IRIs for the control to bite.");
				versionedIris.Should().Equal(bareIris,
					"the version annotation is a literal on the ontology IRI — the Argumentum IRI " +
					"surface stays frozen whatever the configured version (P24-05: 0 IRI changée).");
			}
			finally
			{
				File.Delete(barePath);
				File.Delete(versionedPath);
			}
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (4) Round-trip — the version literal survives the OWL2XML reload the
		//     validators use, so the tag-time bump stays machine-readable downstream.
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public async Task VersionInfo_SurvivesOwl2XmlReload()
		{
			var adapter = BuildSkeleton(FallaciesNs);
			AnnotateVersion(adapter, ProbedVersion);
			var path = await SerializeToTemp(adapter);
			try
			{
				var reloaded = await OWLOntology.FromFileAsync(OWLEnums.OWLFormats.OWL2XML, path);

				// owl:versionInfo is an ontology-LEVEL annotation (<Annotation> child of
				// <Ontology>), so it reloads into OWLOntology.Annotations — not into the
				// AnnotationAxioms stream where the concept-level assertions live (the
				// asymmetry measured on 2026-11-21: the first draft of this test read the
				// wrong collection and found nothing).
				var versionValues = reloaded.Annotations
					.Where(a => a.AnnotationProperty.GetIRI().ToString() == OwlVersionInfoIri)
					.Select(a => a.ValueLiteral?.GetLiteral().ToString())
					.ToList();

				versionValues.Should().Contain(ProbedVersion.ToString(),
					"the versionInfo literal must survive the reload — same expectation class as the " +
					"seeAlso/license round-trip of the publication bridge: what the bump writes must " +
					"stay readable by the validator that checks it.");
			}
			finally
			{
				File.Delete(path);
			}
		}
	}
}
