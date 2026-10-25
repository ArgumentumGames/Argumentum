using System.Collections.Generic;
using Argumentum.AssetConverter;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
	/// <summary>
	/// #1961 — the English mind map must EVALUATE to the English hierarchy, not the French one.
	///
	/// <see cref="MindMapLocalizationRegressionTests"/> asserts the expression TEXT after
	/// localization (it pins tokens like "{item.SubfamilyEn}"); it never evaluates the
	/// expression against a <see cref="Fallacy"/> instance. Both defects of #1961 are invisible
	/// to it: the en family branch pointed at the read-only alias <c>Family => Famille</c>
	/// (level 1), and Dynamic LINQ resolves the token <c>Subfamily</c> to the FR alias
	/// <c>SubFamily</c> — case-insensitive collision, measured by web2 (W20,
	/// https://github.com/ArgumentumGames/Argumentum/issues/1961#issuecomment-6390878893) —
	/// so levels 2 and 3 leaked French despite a correctly rewritten expression.
	///
	/// This suite closes that hole: it builds one Fallacy whose FR and EN hierarchies are
	/// distinct on all three levels, applies the REAL production localization to the REAL
	/// default expressions, then evaluates through the REAL
	/// <see cref="UtilityExtensions.Interpolate"/> (the same Dynamic LINQ path the mind map
	/// generator uses) and asserts the English strings.
	///
	/// Red proof is STAGED, not a master run: on master this file does not compile (the
	/// EN members do not exist there). At the intermediate state — members added, config
	/// not yet rewired — all three EN evaluations returned the French values (3 red)
	/// while the FR companion stayed green, which is exactly the discrimination this
	/// suite is built for.
	/// </summary>
	public class MindMapEnHierarchyEvaluationTests
	{
		private const string FamilleFr = "FamilleFR";
		private const string SousFamilleFr = "SousFamilleFR";
		private const string SoussousFr = "SoussousFR";
		private const string FamilyEn = "FamilyEN";
		private const string SubfamilyEn = "SubfamilyEN";
		private const string SubsubfamilyEn = "SubsubfamilyEN";

		private static Fallacy FallacyWithDistinctHierarchies => new Fallacy
		{
			Famille = FamilleFr,
			SousFamille = SousFamilleFr,
			Soussousfamille = SoussousFr,
			FamilyEn = FamilyEn,
			SubfamilyEn = SubfamilyEn,
			SubsubfamilyEn = SubsubfamilyEn,
		};

		/// <summary>
		/// The three family expressions of a fresh config, localized for "en" exactly the way
		/// the pipeline does it (every MindMapLocalization entry, reflection translate).
		/// </summary>
		private static FallacyMindMapDocumentConfig LocalizedEnConfig()
		{
			var config = new FallacyMindMapDocumentConfig();
			foreach (var localization in new AssetConverterConfig().LocalizationConfig.MindMapLocalization)
			{
				localization.DoReflectionTranslate(config, "en");
			}
			return config;
		}

		public static IEnumerable<object[]> Levels => new List<object[]>
		{
			// propertyName, defaultExpression, expectedToken, expectedEnValue, leakedFrValue
			new object[] { nameof(FallacyMindMapDocumentConfig.FamilleExpression), @"{item.Famille}", "FamilyEn", FamilyEn, FamilleFr },
			new object[] { nameof(FallacyMindMapDocumentConfig.SousFamilleExpression), @"{item.SousFamille}", "SubfamilyEn", SubfamilyEn, SousFamilleFr },
			new object[] { nameof(FallacyMindMapDocumentConfig.SoussousFamilleExpression), @"{item.Soussousfamille}", "SubsubfamilyEn", SubsubfamilyEn, SoussousFr },
		};

		/// <summary>Null-safe expression reader — property names come from nameof, never absent.</summary>
		private static string GetExpression(FallacyMindMapDocumentConfig config, string propertyName)
		{
			var property = typeof(FallacyMindMapDocumentConfig).GetProperty(propertyName);
			property.Should().NotBeNull($"'{propertyName}' must exist on {nameof(FallacyMindMapDocumentConfig)}");
			return (string)property!.GetValue(config)!;
		}

		[Theory]
		[MemberData(nameof(Levels))]
		public void EnFamilyExpressions_EvaluateToEnglishValues(
			string propertyName, string defaultExpression, string expectedToken, string expectedEnValue, string leakedFrValue)
		{
			var fallacy = FallacyWithDistinctHierarchies;

			// Sanity: the fresh config really carries the default expression this test assumes.
			var fresh = new FallacyMindMapDocumentConfig();
			GetExpression(fresh, propertyName).Should().Be(defaultExpression,
				"the test assumes the shipped default; if the default changed, update the theory data");

			var config = LocalizedEnConfig();
			var expression = GetExpression(config, propertyName);

			// Token level (what the regression suite already pins): the en branch rewrote the
			// FR keyword to the disjoint EN member name.
			expression.Should().Contain($"{{item.{expectedToken}}}",
				$"the en branch of the family hierarchy must target {expectedToken} (#1961 option a: disjoint names, no case collision)");

			// EVALUATION level — the assertion MindMapLocalizationRegressionTests cannot make.
			// This is the exact call the mind map generator performs per node.
			var evaluated = expression.Interpolate(new Dictionary<string, object> { { "item", fallacy } });
			evaluated.Should().Be(expectedEnValue,
				$"the localized expression must EVALUATE to the English hierarchy, not the French one (#1961)");
			evaluated.Should().NotBe(leakedFrValue,
				"the French value here is the measured defect: alias read-back (level 1) or Subfamily/SubFamily case collision (levels 2-3)");
		}

		/// <summary>
		/// Inverse control companion: the FR expressions (unlocalized) evaluate to the FR
		/// hierarchy on the same instance — proving the EN assertions above discriminate by
		/// language wiring, not by the instance being wrong.
		/// </summary>
		[Fact]
		public void FrFamilyExpressions_EvaluateToFrenchValues_OnTheSameInstance()
		{
			var fallacy = FallacyWithDistinctHierarchies;
			var config = new FallacyMindMapDocumentConfig();

			config.FamilleExpression.Interpolate(new Dictionary<string, object> { { "item", fallacy } })
				.Should().Be(FamilleFr);
			config.SousFamilleExpression.Interpolate(new Dictionary<string, object> { { "item", fallacy } })
				.Should().Be(SousFamilleFr);
			config.SoussousFamilleExpression.Interpolate(new Dictionary<string, object> { { "item", fallacy } })
				.Should().Be(SoussousFr);
		}

		/// <summary>
		/// The LOAD half of #1961 — cause (a), not covered by the evaluation theory above: the
		/// CSV columns <c>Family</c>/<c>Subfamily</c>/<c>Subsubfamily</c> must land on the disjoint
		/// EN members through the REAL <see cref="CsvBase{T,TMap}.LoadFromContent"/> path (the same
		/// ClassMap + header-normalization the pipeline uses). The previous map targeted the
		/// read-only FR alias <c>Family =&gt; Famille</c> and silently dropped the values at load —
		/// a regression back to that (or a header-name typo in the map) yields null here, and a
		/// swap with the FR columns yields the FR strings — both red on the asserts below.
		/// Counter-review follow-up (Opus, tick 57): the evaluation suite alone left this untested.
		/// </summary>
		[Fact]
		public void CsvEnHierarchyColumns_LoadOntoDisjointEnMembers()
		{
			// Every non-optional mapped header must be present (CsvHelper throws
			// HeaderValidationException otherwise) — the same required set the real taxonomy CSV carries.
			var csv = "pk,path,depth,Famille,Famille_camelCase,Sous-Famille,Soussousfamille,état,carte,niveau,"
				+ "nom_vulgarisé,text_fr,desc_fr,example_fr,link_fr,Remarques,decimal_path_padded,depth_max4,"
				+ "Family,Subfamily,Subsubfamily\n"
				+ $"596,1.1,2,{FamilleFr},familleCC,{SousFamilleFr},{SoussousFr},actif,175,2,"
				+ $"nom,text,desc,example,link,remarques,0001.000,0001,{FamilyEn},{SubfamilyEn},{SubsubfamilyEn}";

			var records = Fallacy.LoadFromContent(csv);

			records.Should().HaveCount(1, "the sample carries one row");
			var fallacy = records[0];
			fallacy.FamilyEn.Should().Be(FamilyEn,
				"the 'Family' column must land on FamilyEn — cause (a) of #1961: the old map targeted the read-only FR alias and dropped the values");
			fallacy.SubfamilyEn.Should().Be(SubfamilyEn, "the 'Subfamily' column must land on SubfamilyEn");
			fallacy.SubsubfamilyEn.Should().Be(SubsubfamilyEn, "the 'Subsubfamily' column must land on SubsubfamilyEn");

			// The FR columns keep their own members — the disjoint rename must not disturb the FR load.
			fallacy.Famille.Should().Be(FamilleFr, "the FR 'Famille' column is unaffected by the EN remap");
			fallacy.SousFamille.Should().Be(SousFamilleFr, "the FR 'Sous-Famille' column is unaffected by the EN remap");
			fallacy.Soussousfamille.Should().Be(SoussousFr, "the FR 'Soussousfamille' column is unaffected by the EN remap");
		}
	}
}
