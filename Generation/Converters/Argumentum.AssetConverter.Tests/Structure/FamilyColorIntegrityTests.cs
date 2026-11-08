using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Structure
{
	/// <summary>
	/// #1460 <b>phase C</b> — le contrôle inverse de <see cref="FamilyColorIntegrity"/>, exécuté
	/// en CI <b>sans aucun artefact</b>.
	///
	/// <para>Une garde jamais vue rouge est un no-op (#1046). Chaque prédicat est donc exercé sur
	/// l'état <b>défectueux</b> avant d'être cru vert sur un bundle — et le fichier porte le
	/// <b>contrôle inverse du contrôle</b> : un jeu sain ne produit <b>aucun</b> échec, sans quoi
	/// un vérificateur toujours-rouge passerait tous les témoins ci-dessous.</para>
	///
	/// <para>⚠️ Les couleurs sont <b>synthétiques</b> : chaque témoin reste lisible sans
	/// instrument. Le calibrage sur de vraies cartes (<see cref="FamilyColorIntegrity.MaxColorDistance"/>,
	/// <see cref="FamilyColorIntegrity.MinFamilyColorSeparation"/>,
	/// <see cref="FamilyColorIntegrity.MinDominantShare"/>) a été fait séparément sur l'arbre
	/// Release de la régén d'octobre le 07/11 (1 400 cartes, distance 0,0 partout, part dominante
	/// minimale 14,8 %) et il est cité dans la doc du moteur — ⛔ ces témoins ne le re-dérivent
	/// pas.</para>
	/// </summary>
	public class FamilyColorIntegrityTests
	{
		private static FamilyColorIntegrity.Rgb Rgb(byte r, byte g, byte b) => new(r, g, b);

		private static FamilyColorIntegrity.CardFamily Card(string name, string familyClass) =>
			new(name, familyClass);

		private static FamilyColorIntegrity.MeasuredDominant Measured(string name, FamilyColorIntegrity.Rgb color, double share) =>
			new(name, color, share);

		/// <summary>Deux familles volontairement aux couleurs du gabarit réel, à 92,9 l'une de
		/// l'autre (la paire la plus serrée mesurée) : la garde structurelle doit les accepter.</summary>
		private static readonly FamilyColorIntegrity.Rgb Turquoise = Rgb(0x08, 0xaf, 0x93);   // erreurMathématique
		private static readonly FamilyColorIntegrity.Rgb Bleu = Rgb(0x00, 0x54, 0xa4);        // abusDeLangage
		private static readonly FamilyColorIntegrity.Rgb Jaune = Rgb(0xff, 0xc3, 0x07);       // tricherie

		private static IReadOnlyDictionary<string, FamilyColorIntegrity.Rgb> Colors() =>
			new Dictionary<string, FamilyColorIntegrity.Rgb>
			{
				["erreurMathématique"] = Turquoise,
				["abusDeLangage"] = Bleu,
				["tricherie"] = Jaune,
			};

		[Fact]
		public void Healthy_Set_Produces_No_Failure()
		{
			// CONTRÔLE INVERSE DU CONTRÔLE : sans lui, un vérificateur toujours-rouge passerait
			// tous les autres témoins de ce fichier.
			var failures = FamilyColorIntegrity.Check(
				new[]
				{
					Card("argumentum_fallacies_1..insuffisance_face.png", "erreurMathématique"),
					Card("argumentum_fallacies_2..tricherie_face.png", "tricherie"),
				},
				Colors(),
				new[]
				{
					Measured("argumentum_fallacies_1..insuffisance_face.png", Turquoise, 0.195),
					Measured("argumentum_fallacies_2..tricherie_face.png", Jaune, 0.148),
				});

			failures.Should().BeEmpty();
		}

		[Fact]
		public void A_Card_Painted_In_The_Wrong_Family_Is_Red_And_Names_Both_Colors()
		{
			var failures = FamilyColorIntegrity.Check(
				new[] { Card("c.png", "abusDeLangage") },
				Colors(),
				new[] { Measured("c.png", Turquoise, 0.20) }); // turquoise au lieu du bleu

			failures.Should().ContainSingle()
				.Which.Should().Contain("c.png")
				.And.Contain("#08af93").And.Contain("#0054a4")
				.And.Contain("abusDeLangage");
		}

		[Fact]
		public void A_Family_Class_Missing_From_The_Template_Is_Red()
		{
			// LE défaut documenté (CLAUDE.md) : une classe absente du gabarit rend une carte à
			// fond blanc. La garde doit le nommer même si la mesure, elle, existe.
			var failures = FamilyColorIntegrity.Check(
				new[] { Card("c.png", "familleNouvelle") },
				Colors(),
				new[] { Measured("c.png", Bleu, 0.20) });

			failures.Should().ContainSingle()
				.Which.Should().Contain("absente du gabarit").And.Contain("familleNouvelle");
		}

		[Fact]
		public void A_Faded_Family_Zone_Is_Red_Even_When_The_Little_Color_Left_Is_Exact()
		{
			// Le cœur du symptôme « fond blanc » : il peut rester des traits à la bonne teinte
			// (texte, filet), mais la zone colorée a fondu. Seul le plancher de part l'attrape.
			var failures = FamilyColorIntegrity.Check(
				new[] { Card("c.png", "tricherie") },
				Colors(),
				new[] { Measured("c.png", Jaune, 0.004) });

			failures.Should().ContainSingle()
				.Which.Should().Contain("fondu").And.Contain("0.4 %");
		}

		[Fact]
		public void Two_Template_Families_Too_Close_Is_Red_Even_When_Every_Card_Is_Exact()
		{
			// Garde structurelle : l'instrument refuse de verdir sur un gabarit où il serait
			// aveugle — ici deux familles à ~13 l'une de l'autre, sous les 60 exigés, alors que
			// la carte mesurée tombe EXACTEMENT sur sa teinte attendue. Dictionnaire dédié : la
			// paire trop proche doit être l'unique échec.
			var failures = FamilyColorIntegrity.Check(
				new[] { Card("c.png", "presqueBleu") },
				new Dictionary<string, FamilyColorIntegrity.Rgb>
				{
					["presqueBleu"] = Rgb(0x00, 0x54, 0xa4),
					["toutAutre"] = Rgb(0x00, 0x5a, 0xb0), // à ~13 de presqueBleu
				},
				new[] { Measured("c.png", Rgb(0x00, 0x54, 0xa4), 0.20) });

			failures.Should().ContainSingle()
				.Which.Should().Contain("presqueBleu").And.Contain("toutAutre")
				.And.Contain("ne pourrait plus les distinguer");
		}

		[Fact]
		public void A_Card_Without_Measurement_Is_Red()
		{
			var failures = FamilyColorIntegrity.Check(
				new[] { Card("c.png", "tricherie") },
				Colors(),
				new[] { Measured("autre.png", Jaune, 0.20) });

			failures.Should().ContainSingle()
				.Which.Should().Contain("c.png").And.Contain("aucune mesure");
		}

		[Fact]
		public void Duplicate_Measurements_Are_Red_The_Join_Is_Ambiguous()
		{
			var failures = FamilyColorIntegrity.Check(
				new[] { Card("c.png", "tricherie") },
				Colors(),
				new[]
				{
					Measured("c.png", Jaune, 0.20),
					Measured("c.png", Jaune, 0.20),
				});

			failures.Should().ContainSingle().Which.Should().Contain("deux mesures du même fichier");
		}

		[Fact]
		public void Empty_Inputs_Are_Red_Not_Silently_Green()
		{
			// Un attendu vide rendrait « 0 échec » sur un jeu jamais examiné : la forme la plus
			// discrète du faux vert.
			FamilyColorIntegrity.Check(null, Colors(), null)
				.Should().ContainSingle().Which.Should().Contain("aucune carte");
			FamilyColorIntegrity.Check(new[] { Card("c.png", "tricherie") }, null, null)
				.Should().ContainSingle().Which.Should().Contain("aucune couleur de famille");
			FamilyColorIntegrity.Check(
					new[] { Card("c.png", "tricherie") }, Colors(), new FamilyColorIntegrity.MeasuredDominant[0])
				.Should().ContainSingle().Which.Should().Contain("0 mesurée");
		}

		[Fact]
		public void Distance_Is_Zero_On_Exact_Match_And_Euclidean_Elsewhere()
		{
			FamilyColorIntegrity.Distance(Turquoise, Turquoise).Should().Be(0);

			// (3,4,0) : le triangle 3-4-5, lisible sans calculatrice.
			FamilyColorIntegrity.Distance(Rgb(0, 0, 0), Rgb(3, 4, 0))
				.Should().BeApproximately(5.0, 1e-9);
		}

		[Fact]
		public void Parser_Reads_Accented_Classes_And_Eight_Digit_Hex_And_Ignores_Rest()
		{
			// ⚠️ Le css passé ici est la valeur PARSÉE d'une clé css de gabarit (leçon #1485) :
			// le moteur ne voit jamais les backslashes du fichier JSON brut.
			const string css = @"
                .card-frame { border: 1px solid #000; }
                card.erreurMathématique {
                    --color-background: #08af93;
                    --color-text-1: #14555b;
                }
                card.tricherie { --color-background: #ffc307ff; }
                card.sansCouleur { --color-text-1: #9e7800ff; }
                .unrelated { color: #ffffff; }";

			var colors = FamilyColorIntegrity.ParseFamilyColors(css);

			colors.Count.Should().Be(2);
			colors["erreurMathématique"].Hex.Should().Be("#08af93");
			colors["tricherie"].Hex.Should().Be("#ffc307"); // hex 8 chiffres : alpha ignoré
			colors.ContainsKey("sansCouleur").Should().BeFalse();
		}

		[Fact]
		public void Parser_Returns_Empty_On_Empty_Css_Instead_Of_Throwing()
		{
			FamilyColorIntegrity.ParseFamilyColors(null).Count.Should().Be(0);
			FamilyColorIntegrity.ParseFamilyColors("").Count.Should().Be(0);
		}

		// ── Transposition Virtues (organe 3) : le gabarit Virtues mesure le 07/11 porte 7 blocs
		//    famille sur 8 en LISTES de sélecteurs (renommage additif), chacune précédée d'un
		//    commentaire CSS « /* --- */ ». Ces témoins éprouvent ce qui a changé au moteur. ──

		/// <summary>La forme mesurée du gabarit Virtues : commentaire avant le sélecteur, liste
		/// de trois générations séparées par virgule + retour à la ligne. La couleur doit
		/// atteindre CHAQUE nom — l'ancien regex n'attrapait que le dernier avant l'accolade.</summary>
		[Fact]
		public void Parser_Assigns_The_Color_To_Every_Name_Of_A_Selector_List()
		{
			const string css = @"
                /* ------ exactitude / rigueur / sens quantitatif ------ */
                card.exactitudeMathématique,
                card.rigueurMathématique,
                card.sensQuantitatif {
                    --color-background: #08af93;
                }
                card.ancienNom, card.nouveauNom { --color-background: #0054a4; }";

			var colors = FamilyColorIntegrity.ParseFamilyColors(css);

			colors.Count.Should().Be(5);
			colors["exactitudeMathématique"].Hex.Should().Be("#08af93");
			colors["rigueurMathématique"].Hex.Should().Be("#08af93");
			colors["sensQuantitatif"].Hex.Should().Be("#08af93");
			colors["ancienNom"].Hex.Should().Be("#0054a4");
			colors["nouveauNom"].Hex.Should().Be("#0054a4");
		}

		/// <summary>Un sélecteur descendant (<c>card.X .icone</c>) porte la couleur d'un ENFANT :
		/// l'assigner à la famille X ferait verdir l'organe sur un fond de carte d'une autre
		/// couleur. Le fullmatch de liste doit l'exclure — tout comme un commentaire CONTENU dans
		/// un corps de règle, qui ne doit casser aucun parse.</summary>
		[Fact]
		public void Parser_Ignores_Descendant_Selectors_And_Comments_In_Bodies()
		{
			const string css = @"
                card.famille .icone { --color-background: #ff0000; }
                card.valide {
                    /* le bleu du panneau, pas de la carte : ignoré au profit de la ligne suivante */
                    --color-text-1: #123456;
                    --color-background: #0054a4;
                }";

			var colors = FamilyColorIntegrity.ParseFamilyColors(css);

			colors.Count.Should().Be(1);
			colors["valide"].Hex.Should().Be("#0054a4");
			colors.ContainsKey("famille").Should().BeFalse();
		}

		/// <summary>LE témoin du changement de garde : les alias hérités du renommage additif
		/// partagent exactement la couleur de leur famille (mesuré : 7 paires à 0,0 sur le gabarit
		/// Virtues). Sur l'ancienne garde — paires sur les NOMS — un gabarit sain à 8 familles et
		/// 15 noms rougissait 7 fois ; la garde court désormais sur les couleurs distinctes.</summary>
		[Fact]
		public void Aliases_Sharing_A_Color_Do_Not_Trip_The_Separation_Guard()
		{
			var colors = new Dictionary<string, FamilyColorIntegrity.Rgb>
			{
				["argumentsVertueux"] = Rgb(0x55, 0x55, 0x55),
				["argumentValable"] = Rgb(0x55, 0x55, 0x55),   // alias : même couleur
				["exactitudeMathématique"] = Turquoise,
				["rigueurMathématique"] = Turquoise,            // alias : même couleur
				["sensQuantitatif"] = Turquoise,                // alias : même couleur
				["abusDeLangage"] = Bleu,
				["tricherie"] = Jaune,
			};

			var failures = FamilyColorIntegrity.Check(
				new[]
				{
					Card("argumentum_virtues_0..argument_valable_face.png", "argumentValable"),
					Card("argumentum_virtues_1.1..argument_fondé_face.png", "exactitudeMathématique"),
				},
				colors,
				new[]
				{
					Measured("argumentum_virtues_0..argument_valable_face.png", Rgb(0x55, 0x55, 0x55), 0.51),
					Measured("argumentum_virtues_1.1..argument_fondé_face.png", Turquoise, 0.47),
				});

			failures.Should().BeEmpty();
		}

		/// <summary>Un nom présent dans DEUX blocs règle : le dernier gagne (comportement du
		/// dictionnaire, documenté au lieu d'être découvert). Le renommage additif écrit les
		/// générations dans UN bloc, mais un gabarit réorganisé doit rester déterministe.</summary>
		[Fact]
		public void Parser_Is_Deterministic_When_A_Name_Reappears_Last_Block_Wins()
		{
			const string css = @"
                card.famille { --color-background: #111111; }
                card.famille { --color-background: #222222; }";

			var colors = FamilyColorIntegrity.ParseFamilyColors(css);

			colors.Count.Should().Be(1);
			colors["famille"].Hex.Should().Be("#222222");
		}

		// ── Garde par BLOC RÈGLE (suite demandée par ai-01 à la revue de l'organe 3,
		//    c.6714912295) : groupée par couleur distincte, la garde de séparation ne voyait
		//    plus une collision EXACTE entre deux familles différentes — elle rougissait à 0,1
		//    mais plus à 0,0, et le contrôle par carte ne peut pas la voir puisque les cartes
		//    des deux familles tombent exactement sur l'attendu partagé. ──

		/// <summary>LE témoin de la faille : deux blocs règle distincts, même couleur, cartes
		/// exactes — la seule garde qui puisse nommer la collision est celle des blocs.</summary>
		[Fact]
		public void Two_Distinct_Blocks_With_The_Same_Color_Are_Red_Even_Though_Every_Card_Matches()
		{
			const string css = @"
                card.inférenceMaîtrisée { --color-background: #0054a4; }
                card.justesseLexicale { --color-background: #0054a4; }";

			var blocks = FamilyColorIntegrity.ParseFamilyBlocks(css);
			var colors = FamilyColorIntegrity.ParseFamilyColors(css);
			blocks.Should().HaveCount(2, "deux blocs distincts, pas une famille aliasée");

			var failures = FamilyColorIntegrity.Check(
				new[]
				{
					Card("a.png", "inférenceMaîtrisée"),
					Card("b.png", "justesseLexicale"),
				},
				colors,
				new[]
				{
					Measured("a.png", Bleu, 0.50),
					Measured("b.png", Bleu, 0.50), // cartes exactes : la faille est invisible au contrôle par carte
				},
				blocks);

			failures.Should().ContainSingle()
				.Which.Should().Contain("inférenceMaîtrisée").And.Contain("justesseLexicale")
				.And.Contain("MÊME couleur");
		}

		/// <summary>La contre-partie : les alias écrits dans UN seul bloc sont UNE famille —
		/// la garde par bloc ne doit pas rougir sur eux (c'est le motif de renommage additif).</summary>
		[Fact]
		public void Aliases_In_One_Block_Do_Not_Trip_The_Block_Guard()
		{
			const string css = @"
                card.exactitudeMathématique,
                card.sensQuantitatif { --color-background: #08af93; }";

			var blocks = FamilyColorIntegrity.ParseFamilyBlocks(css);
			blocks.Should().HaveCount(1);

			var failures = FamilyColorIntegrity.Check(
				new[] { Card("c.png", "sensQuantitatif") },
				FamilyColorIntegrity.ParseFamilyColors(css),
				new[] { Measured("c.png", Turquoise, 0.47) },
				blocks);

			failures.Should().BeEmpty();
		}

		/// <summary>Le gabarit Virtues réel (mesuré le 07/11 : 7 listes alias + 1 bloc simple
		/// = 8 familles, 15 noms) : la garde par bloc doit y rester verte — les alias vivent DANS
		/// leur bloc, aucune famille ne partage la couleur d'une autre. Un renommage de famille
		/// qui casserait la convention (nouveau nom dans un bloc séparé) rend ce témoin rouge.</summary>
		[Fact]
		public void Real_Virtues_Template_Gives_Eight_Family_Blocks_And_Passes_The_Block_Guard()
		{
			var templatePath = Path.Combine(TestRepoRoot.Find(),
				"Cards", "Fallacies", "Argumentum_Virtues_Face_fr.json");
			Assert.True(File.Exists(templatePath), $"gabarit introuvable : {templatePath}");

			string? css;
			using (var doc = JsonDocument.Parse(File.ReadAllText(templatePath)))
			{
				css = doc.RootElement.TryGetProperty("css", out var cssElement) ? cssElement.GetString() : null;
			}

			var blocks = FamilyColorIntegrity.ParseFamilyBlocks(css);

			blocks.Should().HaveCount(8, "7 listes alias + 1 bloc simple = les 8 familles (mesuré le 07/11)");
			blocks.SelectMany(b => b.Names).Should().HaveCount(15);
			blocks.Should().OnlyHaveUniqueItems(b => b.Color, "les 8 familles ont 8 couleurs distinctes");

			var failures = FamilyColorIntegrity.Check(
				new[] { Card("argumentum_virtues_0..face.png", blocks[0].Names[0]) },
				FamilyColorIntegrity.ParseFamilyColors(css),
				new[] { Measured("argumentum_virtues_0..face.png", blocks[0].Color, 0.51) },
				blocks);
			failures.Should().BeEmpty();
		}
	}
}
