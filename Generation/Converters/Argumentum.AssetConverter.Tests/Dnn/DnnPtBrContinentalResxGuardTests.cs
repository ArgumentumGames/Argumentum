using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 — pool po-2024, grain « pt-PT → pt-BR : la liste nominative d'abord ».
	///
	/// <para>L'instrument continental de #1858 (qui tenait les colonnes <c>_pt</c> des corpus
	/// CSV) est porté sur le jeu <c>*.pt-BR.resx</c> du dépôt. Deux familles sont bannies du
	/// brésilien, et elles ne se recouvrent pas :</para>
	///
	/// <list type="number">
	/// <item><b>Orthographe pré-AO</b> — le c latin muet que le Brésil a perdu : <c>acção</c>,
	/// <c>objecto</c>, <c>óptimo</c>, <c>selecção</c>, <c>colecção</c>, <c>facto</c>.
	/// Ce sont des formes portugaises : un fichier pt-BR les écrit sans ce c.</item>
	/// <item><b>Lexique continental</b> — vocabulaire pt-PT qui n'existe pas au Brésil :
	/// <c>ficheiro</c>/<c>arquivo</c>, <c>ecrã</c>/<c>tela</c>, <c>equipa</c>/<c>equipe</c>…</item>
	/// </list>
	///
	/// <para><b>Mesure du 02/11 sur master <c>a3ab8720</c></b> (140 fichiers pt-BR) : le lexique
	/// continental rend <b>0</b> cellule, l'orthographe pré-AO rend <b>13</b> cellules
	/// (<c>acções</c> ×6, <c>objecto(s)</c> ×7) — c'est la liste nominative que le pool
	/// demandait. Les formes ambiguës (<c>activo</c>, <c>actual</c>, <c>directo</c>,
	/// <c>efectivo</c>, <c>correcto</c>, <c>exacto</c>, <c>colectivo</c>) sont <b>hors
	/// périmètre</b> : elles sont aussi espagnoles et appartiennent aux volets de contamination
	/// espagnole (<c>licensing</c>, <c>pages</c>, <c>AdminLogs</c>, <c>extensions</c>) — y mordre
	/// ici produirait un rouge attribué au mauvais volet.</para>
	///
	/// <para><b>Deux pièges d'instrument déjà payés sur ce grain</b>, et dont les motifs
	/// ci-dessous sont la correction : (1) la classe <c>s[íi]tio</c> matchait le « sitio »
	/// <b>espagnol</b> non accentué — 136 « occurrences » qui étaient de la contamination
	/// espagnole, d'où l'accent exact ; (2) <c>\bregisto\b</c> ne voyait pas <c>registos</c> —
	/// un marqueur borgne au pluriel rend 0 par cécité. Les témoins du contrôle inverse sont
	/// donc au <b>pluriel</b> partout où le pluriel est la forme risquée.</para>
	///
	/// <para><c>detecção</c> est délibérément <b>absent</b> de la liste pré-AO : c'est la forme
	/// correcte au Brésil (le c y est prononcé) — un marqueur qui l'attrape produirait un faux
	/// positif, pas une garde.</para>
	/// </summary>
	public class DnnPtBrContinentalResxGuardTests
	{
		/// <summary>Formes bannies du jeu resx pt-BR. Le témoin de chaque entrée sert au
		/// contrôle inverse : un marqueur qui ne tire pas sur son témoin est un marqueur mort,
		/// et un balayage rendu vert par un marqueur mort ne prouve rien.</summary>
		private static readonly (Regex Pattern, string Marker, string Witness)[] Lexicon =
		{
			// —— Famille 1 : orthographe pré-AO (le c latin muet, perdu au Brésil) ——
			(new Regex(@"\bac[çc](?:ã|õ)es?\b", RegexOptions.IgnoreCase), "acção (pré-AO)", "Acções concluídas."),
			(new Regex(@"\bobject(?:o|os)\b", RegexOptions.IgnoreCase), "objecto (pré-AO)", "O objecto e os objectos criados."),
			(new Regex(@"\bóptim[oa]s?\b", RegexOptions.IgnoreCase), "óptimo (pré-AO)", "Trabalho óptimo."),
			(new Regex(@"\bselec(?:c|ç)(?:ão|ões)\b", RegexOptions.IgnoreCase), "selecção (pré-AO)", "A selecção do item."),
			(new Regex(@"\bcolec(?:c|ç)(?:ão|ões)\b", RegexOptions.IgnoreCase), "colecção (pré-AO)", "Uma colecção de itens."),
			(new Regex(@"\bfact(?:o|os)\b", RegexOptions.IgnoreCase), "facto (pré-AO)", "O facto é conhecido."),

			// —— Famille 2 : lexique continental (vocabulaire pt-PT absent du brésilien) ——
			(new Regex(@"\bpalavras?[- ]passe\b", RegexOptions.IgnoreCase), "palavra-passe", "Introduza a sua palavra-passe."),
			(new Regex(@"\baceder\b", RegexOptions.IgnoreCase), "aceder", "Não é possível aceder à página."),
			(new Regex(@"\bregistos?\b", RegexOptions.IgnoreCase), "registo", "Os registos foram apagados."),
			(new Regex(@"\bsítios?\b", RegexOptions.IgnoreCase), "sítio", "Ir para os sítios."),
			(new Regex(@"\bdescarregar\b", RegexOptions.IgnoreCase), "descarregar", "Prima para descarregar o arquivo."),
			(new Regex(@"\becr[ãa]s?\b", RegexOptions.IgnoreCase), "ecrã", "Modo de ecrã inteiro."),
			(new Regex(@"\btelem[óo]ve(?:l|is|es)\b", RegexOptions.IgnoreCase), "telemóvel", "Receba no seu telemóvel."),
			(new Regex(@"\butilizador(?:es)?\b", RegexOptions.IgnoreCase), "utilizador", "Nome de utilizador."),
			(new Regex(@"\bequipas?\b", RegexOptions.IgnoreCase), "equipa", "A equipa de suporte."),
			(new Regex(@"\bficheiros?\b", RegexOptions.IgnoreCase), "ficheiro", "Abra os ficheiros."),
			(new Regex(@"\bsec[çc](?:ã|õ)es?\b", RegexOptions.IgnoreCase), "secção", "As secções do portal."),
			(new Regex(@"\brece[çc](?:ã|õ)es?\b", RegexOptions.IgnoreCase), "receção", "As receções do hotel."),
			(new Regex(@"\bcontactos?\b", RegexOptions.IgnoreCase), "contacto", "Os contactos do portal."),
			(new Regex(@"\bcami(?:ão|ões)\b", RegexOptions.IgnoreCase), "camião", "Os camiões chegaram."),
			(new Regex(@"\bautocarros?\b", RegexOptions.IgnoreCase), "autocarro", "O autocarro das 8h."),
			(new Regex(@"\bcomboios?\b", RegexOptions.IgnoreCase), "comboio", "Perdeu o comboio."),
			(new Regex(@"\bpequeno-almo(?:ç|c)os?\b", RegexOptions.IgnoreCase), "pequeno-almoço", "Servimos o pequeno-almoço."),
			(new Regex(@"\bdesportos?\b", RegexOptions.IgnoreCase), "desporto", "Novo desporto."),
			(new Regex(@"\bgolos?\b", RegexOptions.IgnoreCase), "golo", "Marcou um golo."),
			(new Regex(@"\bguarda-redes\b", RegexOptions.IgnoreCase), "guarda-redes", "O guarda-redes defendeu."),
			(new Regex(@"\btencion(?:a|am|as|ar|ava|avam|ou|aram|ando)\b", RegexOptions.IgnoreCase), "tenciona(r)", "Tenciona viajar."),
			(new Regex(@"\best(?:á|a|ão|amos|ou) a \w+(?:r|ndo)\b", RegexOptions.IgnoreCase), "estar a + infinitif", "Está a chover."),
		};

		/// <summary>Le socle mesuré le 02/11 : 140 fichiers pt-BR. Une chute sous ce plancher
		/// signifie une glob cassée — et un balayage à vide se lirait vert.</summary>
		private const int MeasuredFileFloor = 100;

		private static string RepoRoot => TestRepoRoot.Find();

		[Fact]
		public void No_Continental_Or_PreAo_Form_In_Any_PtBr_Resx()
		{
			var files = Directory.EnumerateFiles(
					Path.Combine(RepoRoot, "DNNPlatform"), "*.pt-BR.resx", SearchOption.AllDirectories)
				.ToList();
			files.Count.Should().BeGreaterOrEqualTo(MeasuredFileFloor,
				"anti-effondrement : 140 fichiers pt-BR mesurés le 02/11 — une énumération vide ou tronquée " +
				"rendrait ce balayage vert sans avoir rien regardé");

			var offenders = new List<string>();
			foreach (var file in files)
			{
				var doc = XDocument.Load(file);
				foreach (var data in doc.Root!.Elements("data"))
				{
					var value = (string?)data.Element("value") ?? string.Empty;
					var hits = Lexicon
						.Where(e => e.Pattern.IsMatch(value))
						.Select(e => e.Marker)
						.ToList();
					if (hits.Count > 0)
					{
						offenders.Add($"{Path.GetFileName(file)}::{data.Attribute("name")?.Value} [{string.Join(", ", hits)}]");
					}
				}
			}
			// Assertion sur la liste JOINTE, pas sur la collection : FluentAssertions ne montre que
			// le premier élément d'une collection fautive, et une liste nominative tronquée à 1
			// se lit comme « une seule cellule » — on perdrait les 12 autres à chaque régression.
			string.Join(Environment.NewLine, offenders).Should().BeEmpty(
				"le jeu pt-BR s'écrit en brésilien : ni le c latin muet (acção/objecto/óptimo — formes portugaises), " +
				"ni le vocabulaire continental (ficheiro/ecrã/equipa). Mesure du 02/11 : 0 continentale, " +
				"13 pré-AO corrigées ; une occurrence nouvelle signale une régression ou un déversement " +
				"depuis un paquet pt-PT (extension de l'instrument #1858)");
		}

		[Fact]
		public void Each_Marker_Fires_On_Its_Witness()
		{
			// Contrôle inverse : un 0 n'est une absence que si l'instrument pouvait voir un 1.
			foreach (var (pattern, marker, witness) in Lexicon)
			{
				pattern.IsMatch(witness).Should().BeTrue(
					$"le marqueur « {marker} » doit reconnaître sa propre phrase témoin — sinon le balayage " +
					"rend 0 par cécité, pas par propreté");
			}
		}
	}
}
