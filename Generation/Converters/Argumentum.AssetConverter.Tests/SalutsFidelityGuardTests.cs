using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde ⑯ du pool v22 (#458 c.5861963967) : famille des saluts hors deck
	/// (2.3.3.4.2.3 « Salutation » et feuilles) — contresens de traduction, pas de
	/// casse. Fichier distinct des gardes ⑤/⑩ pour éviter la collision avec les
	/// PR CSV parallèles.
	///
	/// Sources des formes (mesurées 28/09) :
	/// - ru « Приподнимание шляпы » : article ru.wiki du geste, exactement la
	///   desc_fr (« soulevez ou touchez brièvement votre chapeau ») ; l'ancienne
	///   forme « Спасение из шляпы » lisait « sauvetage hors d'un chapeau » ;
	/// - ru « Скаутский салют » : terme dominant de la littérature scout (НОРС-Р,
	///   scouts.ru, fédérations) et cohérent avec la famille « Военный салют »,
	///   « Вулканский салют » ; l'ancienne « Привет, скаут » lisait « Bonjour,
	///   scout » ;
	/// - ru « Поклон в будо » : le salut budō est le рэй/поклон (ru.wiki
	///   « Рэй (поклон) » : « В будо существует несколько видов поклона ») ; la
	///   forme épouse fr/en/pt/es/ar/fa/zh qui disent tous « salut/inclinaison en
	///   budo » ; l'ancienne « Привет в будо » lisait « Bonjour en budo » ;
	/// - fa « سلام و احوالپرسی » : collocation standard persane pour « salut et
	///   accueil », déjà celle de la desc_fa de la rangée ; l'ancienne « تبریک »
	///   signifie « félicitations » ;
	/// - zh « 脱帽致意 » : terme lexicalisé (dictionnaires, usage littéraire) du
	///   salut par le chapeau ; l'ancienne « 致意帽 » n'est pas du chinois (un
	///   nom, pas un geste) ;
	/// - zh « 吉什急袭 » : alignement du jumeau 7.2.1.2.3 sur la forme retenue
	///   en ⑨ pour 4.3.1.4.1 (arbitrage ai-01 : la carte jumelle porte la forme
	///   de référence).
	/// </summary>
	public class SalutsFidelityGuardTests
	{
		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static string CellOf(string path, string column)
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			var values = csv.LoadColumn(column, "path", new[] { path });
			values.Should().HaveCount(1, "une seule rangée porte ce path.");
			return values[0].Trim();
		}

		private static IReadOnlyList<string> Column(string column) =>
			new HarvestCardIdsCsv(FallaciesCsv).LoadColumn(column);

		[Fact]
		public void G16_Cells_Pinned()
		{
			CellOf("2.3.3.4.2.3", "text_fa").Should().Be("سلام و احوالپرسی",
				"⑯ fa : « Salutation » = salut/accueil (collocation standard, celle de la desc_fa) — « تبریک » = félicitations.");
			CellOf("2.3.3.4.2.3.2", "text_ru").Should().Be("Приподнимание шляпы",
				"⑯ ru : geste ru.wiki du « hat tip » — l'ancienne forme lisait « sauvetage hors d'un chapeau ».");
			CellOf("2.3.3.4.2.3.2", "text_zh").Should().Be("脱帽致意",
				"⑯ zh : terme lexicalisé du salut par le chapeau — « 致意帽 » n'est pas une expression chinoise.");
			CellOf("2.3.3.4.2.3.5", "text_ru").Should().Be("Скаутский салют",
				"⑯ ru : terme des organisations scouts, cohérent avec la famille (Военный/Вулканский салют) — l'ancienne forme lisait « Bonjour, scout ».");
			CellOf("2.3.3.4.2.3.6", "text_ru").Should().Be("Поклон в будо",
				"⑯ ru : le рэй est un поклон (ru.wiki « Рэй (поклон) ») — l'ancienne forme lisait « Bonjour en budo ».");
			CellOf("7.2.1.2.3", "text_zh").Should().Be("吉什急袭",
				"⑯ zh : jumeau aligné sur la forme retenue en ⑨ pour 4.3.1.4.1 (arbitrage ai-01).");
		}

		[Fact]
		public void G16_Family_Witnesses_Unchanged()
		{
			// Témoins non touchés par ⑯ : la famille garde ses formes saines —
			// ces épingles prouvent aussi que le garde lit le bon voisinage.
			CellOf("2.3.3.4.2.3.3", "text_ru").Should().Be("Военный салют",
				"témoin ⑯ : salut militaire, forme déjà correcte.");
			CellOf("2.3.3.4.2.3.4", "text_ru").Should().Be("Римское приветствие",
				"témoin ⑯ : titre ru.wiki du salut romain.");
			CellOf("2.3.3.4.2.3.6", "text_zh").Should().Be("武道鞠躬",
				"témoin ⑯ zh : « salut budō » déjà correct.");
			CellOf("2.3.3.4.2.3.5", "text_zh").Should().Be("童军敬礼",
				"témoin ⑯ zh : « salut scout » déjà correct.");
		}

		[Fact]
		public void Removed_G16_Forms_Absent()
		{
			foreach (var t in Column("text_ru"))
			{
				t.Should().NotContain("Спасение из шляпы", "contresens retiré (⑯ ru).");
				t.Should().NotContain("Привет, скаут", "contresens retiré (⑯ ru).");
				t.Should().NotContain("Привет в будо", "contresens retiré (⑯ ru).");
			}
			foreach (var t in Column("text_fa"))
				t.Should().NotContain("تبریک", "contresens retiré (⑯ fa : félicitations ≠ salutation).");
			foreach (var t in Column("text_zh"))
			{
				t.Should().NotContain("致意帽", "non-sens retiré (⑯ zh).");
				t.Should().NotContain("吉什突袭", "forme jumeau divergente retirée (⑯ zh, alignement ⑨).");
			}
		}
	}
}
