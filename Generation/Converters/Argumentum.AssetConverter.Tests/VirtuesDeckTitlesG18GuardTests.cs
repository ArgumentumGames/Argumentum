using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Épingles du grain ⑱ (arbitrage #458 c.5944233865, cycle K) : 7 cellules de titres
	/// arbitrées par ai-01, plus les 18 cellules de cascade bandeau que l'invariant
	/// ⑩/⑲w (<see cref="VirtuesDeckBandInvariantGuardTests"/>) exige mécaniquement quand
	/// le titre d'un rang bouge — la garde existante est restée rouge jusqu'à la dernière.
	///
	/// <para><b>Les 7 cellules arbitrées.</b> pk 175 ar/fa/ru disaient le titre de la
	/// carte VOISINE 176 (« tenir compte des biais ») au lieu de « reconnaître SES biais » ;
	/// la construction vient des cartes sœurs 169/172, qui disent bien « reconnaître » dans
	/// ces trois langues — la valeur est donc DÉRIVÉE de 172 (qualificatif remplacé), pas
	/// retapée. pk 167 ar : « جهد موضوعي » = « un effort objectif » (adjectif) →
	/// « السعي إلى الموضوعية » SOURCÉ (ar.ilive « بدون تحيز: السعي إلى الموضوعية »,
	/// neutralité de presse alhurriyah.sy). pk 168 fa : « جهان‌گرایی » = <i>mondialisme</i>
	/// → « جهان‌شمولی » SOURCÉ (Hamshahri « «جهان‌شمولی» و «تعهد» », revue de droit SBU
	/// « تکاپوی جهان شمولی »). pk 162/164 fa : retrait du kasra d'ezafe isolé (U+0650) —
	/// 129 des 131 titres fa n'en portent pas.</para>
	///
	/// <para><b>La cascade.</b> 11 subfamily_ar sous le rang 6.3, 3 subsubfamily_fa sous
	/// 6.3.1, 3 subsubfamily_fa sous 6.2.3 (+1 hors deck découverte par la garde ⑲w) :
	/// sans elles, le bandeau imprimé aurait continué de dire « effort objectif » et
	/// « mondialisme » après la correction du titre.</para>
	/// </summary>
	public class VirtuesDeckTitlesG18GuardTests
	{
		private static string VirtuesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");

		private static Dictionary<string, Dictionary<string, string>> LoadRowsByPk()
		{
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				BadDataFound = null,
				HeaderValidated = null,
			};
			using var reader = new StringReader(File.ReadAllText(VirtuesCsv));
			using var csv = new CsvReader(reader, config);
			var rows = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
			foreach (var record in csv.GetRecords<dynamic>())
			{
				var dict = (IDictionary<string, object>)record;
				var row = dict.ToDictionary(kv => kv.Key, kv => kv.Value?.ToString() ?? string.Empty, StringComparer.Ordinal);
				var pk = row.GetValueOrDefault("pk")?.Trim();
				if (!string.IsNullOrEmpty(pk))
					rows[pk] = row;
			}
			rows.Should().HaveCount(223, "le CSV Vertus compte 223 rangées.");
			return rows;
		}

		[Fact]
		public void Arbitrated_Titles_Are_In_Place_And_Derived_From_The_Sister_Cards()
		{
			var rows = LoadRowsByPk();
			rows.Values.Count(r => !string.IsNullOrWhiteSpace(r.GetValueOrDefault("card")))
				.Should().Be(131, "le deck des Vertus compte 131 cartes — anti-vacuité.");

			// pk 175 — construction de 172, qualificatif remplacé (dérivation, pas retypage).
			rows["175"].GetValueOrDefault("title_ar").Should().Be("التعرّف إلى التحيزات الأيديولوجية الشخصية",
				"⑱ pk 175 ar : « reconnaître » (التعرّف), la construction de 172 — pas « مراعاة » (tenir compte), le titre de la carte voisine 176.");
			rows["175"].GetValueOrDefault("title_fa").Should().Be("بازشناسی سوگیری‌های ایدئولوژیک شخصی",
				"⑱ pk 175 fa : idem, « بازشناسی » comme 169/172.");
			rows["175"].GetValueOrDefault("title_ru").Should().Be("Признание личных идеологических искажений",
				"⑱ pk 175 ru : « Признание » comme 169/172 — ⑯ n'avait pas relevé le ru.");

			// La carte VOISINE 176 garde « tenir compte » : c'est son sens à elle.
			rows["176"].GetValueOrDefault("title_ar").Should().Be("مراعاة التحيزات الأيديولوجية لدى الخصم",
				"176 (les biais de l'ADVERSAIRE) garde le verbe « tenir compte » — la correction de 175 ne l'atteint pas.");

			// pk 167 ar — sourcé.
			rows["167"].GetValueOrDefault("title_ar").Should().Be("السعي إلى الموضوعية",
				"⑱ pk 167 ar : l'effort VISE l'objectivité (ru/fa le disaient déjà), pas « un effort objectif » (adjectif).");

			// pk 168 fa — sourcé.
			rows["168"].GetValueOrDefault("title_fa").Should().Be("جهان‌شمولی",
				"⑱ pk 168 fa : le terme philosophique (universalisme), pas « جهان‌گرایی » (mondialisme).");

			// pk 162/164 fa — kasra retiré, rien d'autre.
			rows["162"].GetValueOrDefault("title_fa").Should().Be("سطح مناسب اثبات",
				"⑱ pk 162 fa : kasra d'ezafe retiré (harmonisation typographique, 129/131 titres fa n'en portent pas).");
			rows["164"].GetValueOrDefault("title_fa").Should().Be("هدف غیرمسامحه‌آمیز",
				"⑱ pk 164 fa : idem.");

			// Anti-dérive PÉRIMÉTRÉE : seules les deux rangées arbitrées sont exigées sans
			// kasra. Quatre autres titres fa en portent encore (mesuré 02/10 : « گواهیِ
			// غیرمناقشه‌برانگیز », « آرایه‌های پشتیبانِ روشنی », « سطح کافیِ دلیل » — ezafe ;
			// « قیاس فِرِسیسون » — kasra PHONÉTIQUE, pas ezafe) : hors périmètre ⑱, à
			// arbitrer séparément — le dossier ⑰ chiffrait « 129 sans » sur une mesure qui
			// ne les voyait pas.
			rows["162"].GetValueOrDefault("title_fa").Should().NotContain("ِ",
				"le kasra a été retiré du titre 162 fa.");
			rows["164"].GetValueOrDefault("title_fa").Should().NotContain("ِ",
				"le kasra a été retiré du titre 164 fa.");
		}

		[Fact]
		public void Retired_Forms_Are_Gone_From_The_Whole_Taxonomy()
		{
			var rows = LoadRowsByPk();

			// « جهد موضوعي » (l'effort objectif) : plus aucun titre ni bandeau ar.
			rows.Values.Select(r => r.GetValueOrDefault("title_ar"))
				.Should().NotContain("جهد موضوعي", "la forme adjectivale retirée a disparu des titres ar.");
			rows.Values.Select(r => r.GetValueOrDefault("subfamily_ar"))
				.Should().NotContain("جهد موضوعي",
					"…et de la cascade bandeau ar sous le rang 6.3 (11 + 1 cellules ⑱).");

			// « جهان‌گرایی » (mondialisme) : plus aucun titre ni bandeau fa.
			rows.Values.Select(r => r.GetValueOrDefault("title_fa"))
				.Should().NotContain("جهان‌گرایی", "le mondialisme a quitté les titres fa.");
			rows.Values.Select(r => r.GetValueOrDefault("subsubfamily_fa"))
				.Should().NotContain("جهان‌گرایی", "…et la cascade bandeau fa sous le rang 6.3.1.");

			// « مراعاة … الخاصة » (tenir compte de ses propres biais) : le titre 175 ar retiré.
			rows["175"].GetValueOrDefault("title_ar").Should().NotBe("مراعاة التحيزات الأيديولوجية الخاصة",
				"l'ancien titre de 175 ar (le verbe de la voisine) est retiré.");
		}

		[Fact]
		public void Pk175_Says_Recognize_Like_Its_Sisters_169_And_172()
		{
			// Témoin de famille : 169 (cognitifs), 172 (culturels), 175 (idéologiques) —
			// les trois cartes « reconnaître ses biais X » partagent la construction dans
			// chaque langue, seul le qualificatif change. Si un futur grain retouche l'une
			// des trois, ce témoin l'attrape.
			var rows = LoadRowsByPk();
			var cases = new[]
			{
				("ar", "التعرّف إلى التحيزات", new[] { "169", "172", "175" }),
				("fa", "بازشناسی سوگیری", new[] { "169", "172", "175" }),
				("ru", "Признание", new[] { "169", "172", "175" }),
			};
			foreach (var (lang, head, pks) in cases)
			{
				foreach (var pk in pks)
				{
					var title = (rows[pk].GetValueOrDefault("title_" + lang) ?? "").Trim();
					title.Should().StartWith(head,
						$"pk {pk} [{lang}] : la famille « reconnaître ses biais » commence par « {head} » — " +
						"le verbe de la carte voisine 176 (tenir compte) n'y pénètre pas.");
				}
			}
		}
	}
}
