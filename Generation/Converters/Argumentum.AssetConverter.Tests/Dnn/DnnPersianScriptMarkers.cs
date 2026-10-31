using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Marqueurs d'écriture qui distinguent le persan de l'arabe, pour les lots
	/// <c>fa-IR</c>. Le persan s'écrit dans le même bloc Unicode que l'arabe
	/// (U+0600–U+06FF) : un contrôle d'écriture borné à ce bloc accepte à 100 % une
	/// copie intégrale de l'arabe, et ne le signale pas.
	/// <para>
	/// ⚠️ Le critère est un fait de LOT, jamais une règle par valeur : des valeurs
	/// persanes légitimes ne portent aucune lettre proprement persane — un nombre en
	/// chiffres, un nom propre, un libellé d'une seule lettre partagée. Exiger la
	/// lettre persane valeur par valeur rougirait sur du contenu correct.
	/// </para>
	/// <para>
	/// Mesuré le 31/10 sur les deux lots fa livrés et leurs jumeaux ar-SA des mêmes
	/// fichiers : <c>controls</c> fa 116/133 (87 %) contre ar 0/133 (0 %, 72 valeurs à
	/// lettre arabe exclusive) ; <c>admin</c> fa 281/330 (85 %) contre ar 0/330
	/// (0 %, 218 valeurs). Le plancher de 80 % laisse passer un lot légitimement
	/// pauvre en lettres persanes et refuse une copie arabe.
	/// </para>
	/// </summary>
	public static class PersianScriptMarkers
	{
		/// <summary>
		/// Lettres propres au persan : پ چ ژ گ ک ی (U+067E, U+0686, U+0698, U+06AF,
		/// U+06A9, U+06CC). Écrites en échappements Unicode à dessein : les recopier
		/// en clair invite la confusion entre ک (U+06A9) et ك (U+0643), visuellement
		/// presque identiques — c'est exactement le défaut que ce fichier mesure.
		/// </summary>
		public static readonly Regex PersianOnly =
			new(@"[پچژگکی]", RegexOptions.Compiled);

		/// <summary>
		/// Lettres propres à l'arabe : ي ك ى (U+064A, U+0643, U+0649). Le persan ne
		/// les emploie pas — leur présence est le marqueur direct d'une copie arabe.
		/// </summary>
		public static readonly Regex ArabicOnly =
			new(@"[يكى]", RegexOptions.Compiled);

		/// <summary>
		/// Plancher du lot. Mesuré à 87 % (<c>controls</c>) et 85 % (<c>admin</c>) sur
		/// le lot livré, à 0 % pour une copie arabe : 80 % sépare les deux sans mordre
		/// sur la marge légitime.
		/// </summary>
		public const double MinimumPersianShare = 0.80;

		/// <summary>Profil d'un lot : valeurs, valeurs à lettre persane, valeurs à lettre arabe exclusive.</summary>
		public static (int Total, int Persian, int ArabicOnly) Profile(IEnumerable<string> values)
		{
			var list = values.ToList();
			return (
				list.Count,
				list.Count(PersianOnly.IsMatch),
				list.Count(ArabicOnly.IsMatch));
		}

		/// <summary>
		/// Le lot est persan : plancher de lettres persanes ET zéro lettre arabe
		/// exclusive. Les deux critères sont nécessaires — une traduction arabe échoue
		/// aux deux, un lot persan pauvre en lettres propres n'échoue qu'au premier.
		/// </summary>
		public static void AssertLotIsPersian(IReadOnlyCollection<string> values, string lot)
		{
			var (total, persian, arabicOnly) = Profile(values);

			total.Should().BeGreaterThan(50,
				$"{lot} : le fait de lot porte sur le pack entier — un total de {total} signale "
				+ "un lot vide ou une énumération qui n'a rien lu");

			((double)persian / total).Should().BeGreaterThanOrEqualTo(
				MinimumPersianShare,
				$"{lot} : {persian}/{total} valeurs portent une lettre propre au persan — sous le "
				+ $"plancher de {MinimumPersianShare:P0}, le pack n'est plus distinguable d'une traduction arabe");

			arabicOnly.Should().Be(0,
				$"{lot} : {arabicOnly} valeur(s) emploient ي / ك / ى, que le persan n'emploie pas — "
				+ "c'est la signature d'une copie de l'arabe");
		}

		/// <summary>
		/// Contrôle inverse, rejoué à chaque exécution : le MÊME instrument appliqué au
		/// lot arabe des mêmes fichiers doit échouer aux DEUX critères. Sans lui, un
		/// plancher trop bas resterait vert sur une copie arabe sans que personne ne
		/// le sache — l'instrument ne serait pas prouvé discriminant.
		/// </summary>
		public static void AssertInstrumentRejectsTheArabicLot(IReadOnlyCollection<string> values, string lot)
		{
			var (total, persian, arabicOnly) = Profile(values);

			total.Should().BeGreaterThan(50,
				$"{lot} : le lot de comparaison doit être lu en entier ({total} valeurs)");

			((double)persian / total).Should().BeLessThan(
				MinimumPersianShare,
				$"{lot} : {persian}/{total} valeurs portent une lettre persane — si le lot arabe passait "
				+ "le plancher, l'instrument ne distinguerait plus rien");

			arabicOnly.Should().BeGreaterThan(0,
				$"{lot} : aucune lettre propre à l'arabe dans le lot de comparaison — "
				+ "le second critère ne serait pas exercé");
		}
	}
}
