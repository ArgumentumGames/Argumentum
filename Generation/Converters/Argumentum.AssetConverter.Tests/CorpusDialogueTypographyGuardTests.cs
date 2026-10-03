using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde de typographie des dialogues et tirets du deck (pool #458, dispatch
	/// c.5971513525 grain 2) : épingle les 29 cellules rétablies pour qu'aucune passe
	/// de traduction ou de fusion ne ramène les formes aplaties ou les tirets ASCII.
	/// <list type="bullet">
	/// <item>13 cellules structurelles : 974 aplati en ru/pt/es/ar/fa/zh rétabli en
	/// 3 lignes comme le FR ; 943 aplati en pt rétabli en 2 lignes ; marqueurs de
	/// réplique « — » espacé (en/ru/pt/es/ar/fa) et « —— » double (zh, 破折号) — la
	/// colonne en 974, déjà conforme, est épinglée telle quelle (référence).</item>
	/// <item>13 tirets ASCII → cadratin espacé : example_ru 51/121/182/726/735/784/
	/// 844/1388, example_pt et example_ar 1388, desc_ru 989/1398.</item>
	/// <item>Tirets en collés (incises) : example_en 658/796 « — » → «—».</item>
	/// <item>Énumérations 784 : desc_en/desc_ar « - » → demi-cadratin « – » ×2.</item>
	/// </list>
	/// Hors périmètre, documenté : 813 es/fa/zh gardent leurs préfixes de ligne
	/// « - » (le FR 813 est monoligne — structure divergente non arbitrée) ; les
	/// doublons hors deck 1055/1341 (copies de 51/121 example_ru, ASCII conservé)
	/// relèvent du grain « tirets hors deck ». Le balayage final exige zéro
	/// « - » et zéro séparateur aplati sur tout le deck, 16 colonnes.
	/// NB : exprimé en HaveCount(1) + Be, jamais ContainSingle(valeur, parce-que)
	/// (mémoire FluentassertionsContainSingleStringVacuous).
	/// </summary>
	public class CorpusDialogueTypographyGuardTests
	{
		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static string Cell(string column, string pk)
		{
			var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn(column, "PK", new[] { pk });
			values.Should().HaveCount(1, "une seule rangée porte le PK {0} dans la taxonomie.", pk);
			return values[0];
		}

		/// <summary>Valeurs d'une colonne restreintes aux rangées du deck (carte non vide), ordre préservé.</summary>
		private static IReadOnlyList<string> DeckCells(string column)
		{
			var cartes = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("carte");
			var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn(column);
			values.Count.Should().Be(cartes.Count, "les deux colonnes couvrent les mêmes rangées.");
			return values.Where((v, i) => !string.IsNullOrWhiteSpace(cartes[i])).ToList();
		}

		private static readonly Dictionary<string, string> Reply974 = new()
		{
			["en"] = "— You don't know how to drive.\n— But I have my driver's license!\n— Yes, but you've never been able to parallel park properly...",
			["ru"] = "— Ты не умеешь водить.\n— Но у меня есть водительские права!\n— Да, но ты никогда не мог правильно парковаться.",
			["pt"] = "— Você não sabe dirigir.\n— Mas eu tenho minha carteira de motorista!\n— Sim, mas você nunca soube estacionar direito...",
			["es"] = "— No sabes conducir.\n— Pero tengo mi carnet de conducir.\n— Sí, pero nunca has sabido aparcar en paralelo correctamente...",
			["ar"] = "— لا تعرف كيف تقود السيارة.\n— ولكن لدي رخصة قيادة!\n— نعم، لكنك لم تستطع أبدًا ركن السيارة بشكل متوازي بشكل صحيح...",
			["fa"] = "— تو رانندگی بلد نیستی.\n— اما من گواهی‌نامه رانندگی دارم!\n— بله، اما تو هیچ‌وقت نتوانسته‌ای به درستی پارک دوبل کنی...",
			["zh"] = "——你不会开车。\n——但我有驾照！\n——是的，但你从来没有能够正确地平行停车……",
		};

		private static readonly Dictionary<string, string> Reply943 = new()
		{
			["en"] = "— You said you found this movie dazzling.\n— Dazzling in its stupidity!",
			["ru"] = "— Ты сказал, что нашел этот фильм потрясающим.\n— Потрясающим своей глупостью!",
			["pt"] = "— Você me disse que tinha achado o filme deslumbrante...\n— Deslumbrante de tão estúpido!",
			["es"] = "— Dijiste que encontraste esta película deslumbrante.\n— Deslumbrante por su estupidez",
			["ar"] = "— لقد قلت أنك وجدت هذا الفيلم مبهر.\n— مبهر في غبائه!",
			["fa"] = "— تو گفتی که این فیلم را خیره‌کننده دانستی.\n— خیره‌کننده در احمقانه بودنش!",
			["zh"] = "——你不是说这电影很耀眼吗？\n——耀眼的是它的愚蠢！",
		};

		private static readonly Dictionary<string, string> CadratinExampleRu = new()
		{
			["51"] = "Дети — это монстры, так что честь вам и хвала за то, как вы воспитываете вашего.",
			["121"] = "Не пытайтесь объяснить это верованиями. Религия — это личное дело каждого, и ее нельзя критиковать.",
			["182"] = "Если «орел» — я выиграл. Если «решка» — ты проиграл!",
			["726"] = "Чем больше сыра, тем больше дырок, чем больше дырок — тем меньше сыра. Так что чем больше сыра, тем его меньше.",
			["735"] = "Все философы мудры, однако, некоторые из них — идиоты.",
			["784"] = "Рыбы живут в море, но киты тоже живут в море, значит, киты — это рыбы.",
			["844"] = "Джейн отлично справляется с математикой. Джейн — дислексик. Следовательно, все дислексики хорошо справляются с математикой.",
			["1388"] = "Ты кажешься нервным, представляя свой проект — разве ты сам в него не веришь?",
		};

		private static readonly Dictionary<string, string> CadratinAutres = new()
		{
			["1388|example_pt"] = "Você parece nervoso ao apresentar seu projeto — você não acredita nele, talvez?",
			["1388|example_ar"] = "يبدو أنك متوتر عند تقديم مشروعك — ربما أنت نفسك لا تؤمن به؟",
			["989|desc_ru"] = "Вы считаете, что другая сторона должна опровергать ваши доводы, а не вы — доказывать ваши.",
			["1398|desc_ru"] = "Личная атака на собеседника, вне связи с темой дебатов. Цель — дискредитировать его самого и его аргументы одним махом.",
		};

		private static readonly Dictionary<string, string> EnGlue = new()
		{
			["658"] = "This statement is true.—But how do you know? I verified it.—But how did you verify that verification? And how did you verify the verification of that verification? …",
			["796"] = "All lawyers defend clients in court. This fruit is an avocado. Therefore, this fruit defends clients in court.—“Lawyer” and “avocado” are the same word in French, but its meaning changes: it refers to the legal profession in the first premise and to the fruit in the second. The reasoning therefore actually contains four terms instead of three.",
		};

		[Fact]
		public void Reply974_AllLanguages_ThreeLinesWithLanguageMarker()
		{
			foreach (var (lang, expected) in Reply974)
			{
				var cell = Cell("example_" + lang, "974");
				cell.Should().Be(expected,
					"974 {0} était aplati en une ligne ; répliques rétablies comme le FR, marqueur de réplique selon la norme {0}.", lang);
			}
		}

		[Fact]
		public void Reply943_AllLanguages_TwoLinesWithLanguageMarker()
		{
			foreach (var (lang, expected) in Reply943)
			{
				var cell = Cell("example_" + lang, "943");
				cell.Should().Be(expected,
					"943 {0} : pt rétabli en 2 lignes, les autres passées du préfixe « - » au marqueur de la norme {0}.", lang);
			}
		}

		[Fact]
		public void Cadratin_ExampleRu_EightCells_SpaceEmDash()
		{
			foreach (var (pk, expected) in CadratinExampleRu)
			{
				var cell = Cell("example_ru", pk);
				cell.Should().Be(expected, "tiret ASCII « - » → cadratin espacé « — » (PK {0}, grain 2).", pk);
				cell.Should().NotContain(" - ", "aucun tiret ASCII espacé ne doit subsister (PK {0}).", pk);
			}
		}

		[Fact]
		public void Cadratin_OtherColumns_FourCells_SpaceEmDash()
		{
			foreach (var (key, expected) in CadratinAutres)
			{
				var parts = key.Split('|');
				var cell = Cell(parts[1], parts[0]);
				cell.Should().Be(expected, "tiret ASCII → cadratin espacé ({0} PK {1}, grain 2).", parts[1], parts[0]);
				cell.Should().NotContain(" - ", "aucun tiret ASCII espacé ne doit subsister ({0} PK {1}).", parts[1], parts[0]);
			}
		}

		[Fact]
		public void EnGlue_658And796_EmDashClosedUp()
		{
			foreach (var (pk, expected) in EnGlue)
			{
				var cell = Cell("example_en", pk);
				cell.Should().Be(expected, "incises en : cadratin collé «—» (norme en), PK {0}.", pk);
				cell.Should().NotContain(" — ", "l'incise en ne prend pas d'espaces autour du cadratin (PK {0}).", pk);
			}
		}

		[Fact]
		public void DemiCadratin_784_Enumerations_EnDashTwice()
		{
			foreach (var column in new[] { "desc_en", "desc_ar" })
			{
				var cell = Cell(column, "784");
				Regex.Matches(cell, Regex.Escape(" – ")).Count.Should().Be(2,
					"les deux énumérations prémisse majeure/mineure/conclusion passent au demi-cadratin ({0}).", column);
				cell.Should().NotContain(" - ", "aucun tiret ASCII espacé ne doit subsister ({0}).", column);
			}
		}

		[Fact]
		public void Deck_AllLanguages_NoAsciiDashNorFlattenedSeparatorRemains()
		{
			foreach (var lang in new[] { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" })
			{
				foreach (var field in new[] { "example", "desc" })
				{
					var column = field + "_" + lang;
					foreach (var cell in DeckCells(column))
					{
						cell.Should().NotContain(" - ",
							"tiret ASCII espacé interdit sur le deck ({0}) — grain 2 converti en cadratin.", column);
						cell.Should().NotContain(".-",
							"séparateur de réplique aplati interdit sur le deck ({0}) — 974 es rétabli.", column);
					}
				}
			}
		}

		[Fact]
		public void DeckZh_NoIdeographicFlattenedSeparatorRemains()
		{
			foreach (var column in new[] { "example_zh", "desc_zh" })
			{
				foreach (var cell in DeckCells(column))
				{
					cell.Should().NotContain("。-",
						"le séparateur aplati chinois (point idéographique + tiret) ne doit plus exister ({0}).", column);
				}
			}
		}
	}
}
