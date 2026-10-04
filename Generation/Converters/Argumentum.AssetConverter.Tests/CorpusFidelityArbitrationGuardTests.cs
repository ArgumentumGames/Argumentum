using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde des arbitrages ai-01 du pool v23 (#458) : épingle les cellules dont le
	/// verdict a été tranché, pour qu'aucune passe de traduction ou de régénération
	/// ne les ramène silencieusement à l'état fautif.
	/// <list type="bullet">
	/// <item>8 cellules de définitions arbitrées le 03/10 (c.5966971153, PR #1721) :
	/// fa ×4 (323 coquille, 361 et 696 mot-clé faux, 362 verbe), es 1357 accent,
	/// en 622/625/1362 coquilles. La PR #1721 est passée SANS garde — celle-ci
	/// comble le manque (dispatch c.5968567990).</item>
	/// <item>3 cellules d'exemples russes arbitrées le 03/10 (c.5968567847) :
	/// 361 coquille + tiret long, 1352 retour au mot imprimé « сосудистая »,
	/// 848 virgules normatives autour du relatif « которые ».</item>
	/// <item>7 cellules d'exemples arbitrées le 03/10 (c.5971513525, grain 1) :
	/// ar 622/900 accords du démonstratif, fa 1330 « billes » (verdict natif
	/// rendu par l'arbitrage), es 153 « coche » + es 673 idiome « partir la
	/// diferencia », en 673 coquille « meet up », fr 476 sauts de ligne
	/// accidentels joints (hors deck).</item>
	/// </list>
	/// NB : exprimé en HaveCount(1) + Be, jamais ContainSingle(valeur, parce-que) —
	/// cette surcharge résout vers ContainSingle(because) qui n'épingle PAS la valeur
	/// (mémoire FluentassertionsContainSingleStringVacuous). Les deux cellules fa au
	/// mot-clé faux sont épinglées par présence du mot arbitré + absence du mot
	/// fautif, pour ne pas geler une retouche lexicale future (modèle
	/// <see cref="DescZhBurdenOfProofGuardTests"/>).
	/// </summary>
	public class CorpusFidelityArbitrationGuardTests
	{
		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static void AssertSingleRowCell(string column, string pk, string expected, string because)
		{
			var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn(column, "PK", new[] { pk });
			values.Should().HaveCount(1, "une seule rangée porte le PK {0} dans la taxonomie.", pk);
			values[0].Should().Be(expected, because);
		}

		private static string Cell(string column, string pk)
		{
			var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn(column, "PK", new[] { pk });
			values.Should().HaveCount(1, "une seule rangée porte le PK {0} dans la taxonomie.", pk);
			return values[0];
		}

		// --- #1721 : 8 cellules de définitions (arbitrage c.5966971153) ---

		[Fact]
		public void DescEn_Pk622_Composition_TypoFixed()
		{
			AssertSingleRowCell("desc_en", "622",
				"Attributing properties of the parts of a set to the whole set.",
				"#1721 : « Atrributing » (triple r) était la coquille arbitrée.");
		}

		[Fact]
		public void DescEn_Pk625_Division_PluralFixed()
		{
			AssertSingleRowCell("desc_en", "625",
				"Attributing properties of a whole set to each of its individual parts.",
				"#1721 : « individual part » (pluriel manquant) était la coquille arbitrée.");
		}

		[Fact]
		public void DescEn_Pk1362_TuQuoque_FinalPeriodRestored()
		{
			AssertSingleRowCell("desc_en", "1362",
				"Discrediting the opponent's position by asserting the opponent's failure to act consistently in accordance with that position.",
				"#1721 : le point final manquant était la coquille arbitrée.");
		}

		[Fact]
		public void DescEs_Pk1357_FabricatedArguments_AccentFixed()
		{
			AssertSingleRowCell("desc_es", "1357",
				"Inventas argumentos que parecen ajustarse a la lógica de tu oponente pero que en realidad son falaces.",
				"#1721 : « Invéntas » portait un accent parasite sur une forme « tú » qui n'en prend pas.");
		}

		[Fact]
		public void DescFa_Pk323_ApproachToScorn_TypoFixed()
		{
			var cell = Cell("desc_fa", "323");
			cell.Should().Contain("ناشایسته",
				"#1721 : « نایسته » était la coquille arbitrée (mot inexistant)");
			cell.Should().NotContain("نایسته",
				"la coquille « نایسته » ne doit pas revenir (arbitrage c.5966971153)");
		}

		[Fact]
		public void DescFa_Pk361_BaitAndSwitch_KeyWordArbitrated()
		{
			var cell = Cell("desc_fa", "361");
			cell.Should().Contain("مورد قبول همگان",
				"#1721 : la définition parle d'affirmations ACCEPTÉES DE TOUS, pas « uniques » — "
				+ "« بی‌نظیر » était le mot-clé faux arbitré");
			cell.Should().NotContain("بی‌نظیر",
				"le mot-clé fautif « بی‌نظیر » ne doit pas revenir (arbitrage c.5966971153)");
		}

		[Fact]
		public void DescFa_Pk362_ComplimentSandwich_VerbArbitrated()
		{
			var cell = Cell("desc_fa", "362");
			cell.Should().Contain("قرار دادن",
				"#1721 : « قرار دادن » (placer) est le verbe arbitré — "
				+ "« بدرو آوردن » n'existe pas");
			cell.Should().NotContain("بدرو آوردن",
				"le verbe fautif « بدرو آوردن » ne doit pas revenir (arbitrage c.5966971153)");
		}

		[Fact]
		public void DescFa_Pk696_FlawedReasoning_KeyWordArbitrated()
		{
			var cell = Cell("desc_fa", "696");
			cell.Should().Contain("رسیدن به",
				"#1721 : « رسیدن به » (parvenir à) est le verbe arbitré — "
				+ "« پایان‌بخشی » (mettre fin à) inversait le sens");
			cell.Should().NotContain("پایان‌بخشی",
				"le verbe fautif « پایان‌بخشی » ne doit pas revenir (arbitrage c.5966971153)");
		}

		// --- Arbitrage c.5968567847 : 3 cellules d'exemples russes (㉖) ---

		[Fact]
		public void ExampleRu_Pk361_BaitAndSwitch_TypoAndLongDashFixed()
		{
			AssertSingleRowCell("example_ru", "361",
				"Воспитывать детей и удовлетворять их потребности — родительский долг. Поэтому именно они и должны решать, куда детям пойти учиться.",
				"c.5968567847 : « удоовлетворять » (double о, coquille de l'imprimé) corrigé, "
				+ "et le tiret de « … потребности - родительский долг » passe au tiret long « — ».");
		}

		[Fact]
		public void ExampleRu_Pk1352_PoisoningTheWell_BackToPrintedWord()
		{
			AssertSingleRowCell("example_ru", "1352",
				"Нужно ли напоминать, что сосудистая хирургия была разработана симпатизирующими нацистам учеными?",
				"c.5968567847 : « сосудистая » est le mot de la carte imprimée (archive v3) et le "
				+ "terme courant — « васкулярная » était l'anglicisme médical propre au deck.");
		}

		[Fact]
		public void ExampleRu_Pk848_AmbiguousPunctuation_NormativeCommas()
		{
			AssertSingleRowCell("example_ru", "848",
				"Члены комитета, которые утвердили это досье, будут вызваны.",
				"c.5968567847 : en russe les virgules autour d'une relative en « который » sont "
				+ "obligatoires dans les deux lectures — l'imprimé portait la même faute.");
		}

		// --- Arbitrage c.5971513525 : 7 cellules d'exemples (grain 1) ---

		[Fact]
		public void ExampleAr_Pk622_FadingGarden_DemonstrativeAgreementFixed()
		{
			var cell = Cell("example_ar", "622");
			cell.Should().Contain("هذه الحديقة",
				"c.5971513525 : الحديقة (jardin) est féminin, le démonstratif doit être هذه");
			cell.Should().NotContain("هذا الحديقة",
				"l'accord fautif هذا الحديقة ne doit pas revenir (le cellule contient déjà "
				+ "هذه النباتات, correct, pour les plantes)");
		}

		[Fact]
		public void ExampleAr_Pk900_IgnoredOption_DemonstrativeAgreementFixed()
		{
			var cell = Cell("example_ar", "900");
			cell.Should().Contain("هذا بالتأكيد خيار",
				"c.5971513525 : خيار (option) est masculin, le démonstratif doit être هذا");
			cell.Should().NotContain("هذه بالتأكيد خيار",
				"l'accord fautif هذه بالتأكيد خيار ne doit pas revenir");
			// review #1735 c.5972921914 : le pronom suffixe suit le démonstratif —
			// اقتراح (masculin) porte la voyelle hu, pas ha.
			cell.Should().Contain("اقتراحه",
				"c.5972921914 : le pronom final suit le masculin خيار — اقتراحها était la forme fautive");
			cell.Should().NotContain("اقتراحها",
				"l'accord fautif اقتراحها ne doit pas revenir (review #1735)");
		}

		[Fact]
		public void ExampleFa_Pk1330_ChewbaccaDefense_MarblesNotArrows()
		{
			var cell = Cell("example_fa", "1330");
			cell.Should().Contain("تیله‌هایم",
				"c.5971513525 : le FR dit « mes billes » (تیله) — verdict natif rendu par "
				+ "l'arbitrage après le dossier ㉙");
			cell.Should().NotContain("تیرهایم",
				"« mes flèches » (تیر) était la confusion lexicale arbitrée");
		}

		[Fact]
		public void ExampleEs_Pk153_SelfInterest_CocheNotAuto()
		{
			var cell = Cell("example_es", "153");
			cell.Should().Contain("venderme su coche.",
				"c.5971513525 : la colonne es dit « coche » (Espagne) partout ailleurs "
				+ "(614, 666, 1174) — « auto » était le régionalisme LatAm");
			cell.Should().NotContain("su auto",
				"le régionalisme « auto » ne doit pas revenir dans cette colonne");
		}

		[Fact]
		public void ExampleEs_Pk673_MiddleGround_IdiomArbitrated()
		{
			var cell = Cell("example_es", "673");
			cell.Should().Contain("Partamos la diferencia",
				"c.5971513525 : « partir la diferencia » est l'idiome espagnol de "
				+ "« couper la poire en deux »");
			cell.Should().NotContain("Midamos el punto medio",
				"le calque gauche « Midamos el punto medio » ne doit pas revenir");
		}

		[Fact]
		public void ExampleEn_Pk673_MiddleGround_MeetUpSpellingFixed()
		{
			AssertSingleRowCell("example_en", "673",
				"You prefer Monday and I Wednesday: let's meet up on Tuesday.",
				"c.5971513525 : « meetup » (nom) employé comme verbe était la coquille — "
				+ "l'en est imprimé et garde son texte (seule la coquille bouge)");
		}

		[Fact]
		public void ExampleFr_Pk476_GishGallop_TakesThePk1300Text_Arbitrated()
		{
			AssertSingleRowCell("example_fr", "476",
				"Mon adversaire devrait maintenant répondre à mes arguments sur diverses politiques économiques, sociales et environnementales, en tenant compte des implications à court et à long terme, des effets sur les populations vulnérables, des considérations géopolitiques ainsi que des aspects juridiques et éthiques.",
				"c.5972921914 : la simple jointure laissait « personnes vulnérables, populations » "
				+ "(mot orphelin devenu élément de liste) — le FR avait traduit les deux morceaux du "
				+ "retour à la ligne de mise en forme de l'EN. L'arbitrage (c.5971513525) impose le "
				+ "texte de PK 1300, même exemple, propre ; hors deck.");
		}
	}
}
