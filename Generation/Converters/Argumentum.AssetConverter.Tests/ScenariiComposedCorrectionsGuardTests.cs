using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Grain 5 de la file profonde (#458, c.5993735448) - les COMPOSITIONS differrees : 10 cellules
	/// epingnees «composer» par les burn-downs de <see cref="ScenariiCorrections3GuardTests"/>
	/// (4.2.8 x3, 3.1.5 x3) et <see cref="ScenariiLanguageSpecificCorrectionsGuardTests"/> (6.2.1 x2,
	/// 5.3.5, 7.2.7), plus 3 siblings de rangee (3.1.5 contextes ar/fa/ru) par la regle du GO
	/// (c.5990803984) : meme defaut, meme rangee, meme PR.
	/// <para><b>Composer n'est pas restaurer</b> : le FR ne donne pas la forme cible. Chaque forme est
	/// ancree sur (1) un modele fidele de la meme rangee (pt/es «Despertar comprometido», «conquista»,
	/// ar/fa 6.2.1, les 4 referents de 5.3.5/7.2.7) ou (2) un precedent INTERNE au corpus (3.1.2
	/// traduit «Sa conquête» par «Его пассия» ; 6.2.1 context_zh dit deja 谈妥退选) ou (3) le
	/// vocabulaire de la carte elle-meme (گرانش au titre fa de 5.3.5, собеседование au contexte/enjeu
	/// ru de 7.2.7). Langues jamais imprimees -> correction sur delegation (regle actee, dashboard).
	/// Ecrit le 07/10/2026, chirurgie par span de champ, 13 cellules / 5 rangees / +14 octets.</para>
	/// </summary>
	public class ScenariiComposedCorrectionsGuardTests
	{
		private static string ScenariiCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Scenarii", "Argumentum Scenarii - Cards.csv");

		/// <summary>Les 13 cellules composees : rangee, colonne, valeur pleine mesuree APRES,
		/// valeur pleine AVANT, et ce que la composition rend.</summary>
		private static readonly (string Path, string Column, string Expected, string Was, string Why)[] Composed =
		{
			("4.2.8", "title_ar", "\u0627\u0633\u062a\u064a\u0642\u0627\u0638 \u0645\u062d\u0631\u062c", "\u0627\u0633\u062a\u064a\u0642\u0627\u0638 \u063a\u064a\u0631 \u0645\u062a\u0648\u0642\u0639 \u0648\u0645\u0623\u0632\u0642 \u062d\u0633\u0627\u0633", "jeu \u00abcompromis\u00bb rendu : \u0645\u062d\u0631\u062c (compromettant), modele pt/es \u00abDespertar comprometido\u00bb ; \u00ab\u063a\u064a\u0631 \u0645\u062a\u0648\u0642\u0639\u00bb (inattendu) etait ajoute sans source"),
			("4.2.8", "title_fa", "\u0628\u06cc\u062f\u0627\u0631\u06cc \u062e\u0641\u062a\u200c\u0622\u0648\u0631", "\u0628\u06cc\u062f\u0627\u0631\u06cc\u0650 \u063a\u06cc\u0631\u0645\u0646\u062a\u0638\u0631\u0647 \u0648 \u0645\u0635\u0627\u0644\u062d\u0647\u200c\u0627\u06cc \u062d\u0633\u0627\u0633", "idem fa : \u062e\u0641\u062a\u200c\u0622\u0648\u0631 (genant), calque du modele pt/es ; \u00ab\u063a\u06cc\u0631\u0645\u0646\u062a\u0638\u0631\u0647\u00bb (inattendu) etait ajoute sans source"),
			("4.2.8", "title_zh", "\u5c34\u5c2c\u7684\u82cf\u9192", "\u610f\u5916\u9192\u6765\u4e0e\u5fae\u5999\u59a5\u534f", "\u5c34\u5c2c rend le sens \u00abcompromettant\u00bb du FR ; \u59a5\u534f lisait \u00abcompromis\u00bb comme un accord negocie ; \u610f\u5916 (inattendu) etait ajoute"),
			("3.1.5", "drawer_ar", "\u0622\u062e\u0631 \u0641\u062a\u0648\u062d\u0627\u062a\u0647 \u0645\u0646 \u0627\u0644\u0644\u064a\u0644\u0629 \u0627\u0644\u0633\u0627\u0628\u0642\u0629", "\u062d\u0628\u064a\u0628\u062a\u0647 \u0645\u0646 \u0627\u0644\u0644\u064a\u0644\u0629 \u0627\u0644\u0633\u0627\u0628\u0642\u0629", "\u00abconquete\u00bb = une PERSONNE : \u0622\u062e\u0631 \u0641\u062a\u0648\u062d\u0627\u062a\u0647 (sa derniere conquete, sens figure atteste) ; \u062d\u0628\u064a\u0628\u062a\u0647 (\u00absa bien-aimee\u00bb) adoucissait (dossier ar)"),
			("3.1.5", "drawer_fa", "\u0634\u06a9\u0627\u0631 \u062f\u06cc\u0634\u0628\u06cc\u200c\u0627\u0634", "\u062f\u0644\u0628\u0631 \u062f\u06cc\u0634\u0628\u06cc\u200c\u0627\u0634", "\u0634\u06a9\u0627\u0631 (la prise) rend le registre familier du FR ; \u062f\u0644\u0628\u0631 (la dulcinee) adoucissait ; \u0634\u06a9\u0627\u0631 etait le candidat nomme par le dossier fa"),
			("3.1.5", "drawer_ru", "\u0415\u0433\u043e \u0432\u0447\u0435\u0440\u0430\u0448\u043d\u044f\u044f \u043f\u0430\u0441\u0441\u0438\u044f", "\u041d\u043e\u0432\u0430\u044f \u0438\u0437\u0431\u0440\u0430\u043d\u043d\u0438\u0446\u0430", "\u043f\u0430\u0441\u0441\u0438\u044f = l'equivalence ETABLIE par le corpus : 3.1.2 traduit \u00abSa conqu\u00eate\u00bb par \u00ab\u0415\u0433\u043e \u043f\u0430\u0441\u0441\u0438\u044f\u00bb, son contexte dit \u00ab\u0432\u0447\u0435\u0440\u0430\u0448\u043d\u0435\u0439 \u043f\u0430\u0441\u0441\u0438\u0435\u0439\u00bb ; \u0438\u0437\u0431\u0440\u0430\u043d\u043d\u0438\u0446\u0430 (l'elue) adoucissait"),
			("3.1.5", "context_ar", "\u0643\u064a \u064a\u063a\u0648\u064a\u0647\u0627\u060c \u0643\u0630\u0628 \u0627\u0644\u0645\u063a\u0627\u0644\u0650\u0637 \u0648\u0646\u0633\u0628 \u0625\u0644\u0649 \u0646\u0641\u0633\u0647 \u0648\u0638\u064a\u0641\u0629 \u0645\u0631\u0645\u0648\u0642\u0629. \u0641\u064a \u0627\u0644\u064a\u0648\u0645 \u0627\u0644\u062a\u0627\u0644\u064a\u060c \u064a\u062f\u062e\u0644 \u0622\u062e\u0631 \u0641\u062a\u0648\u062d\u0627\u062a\u0647 \u0645\u0646 \u0627\u0644\u0644\u064a\u0644\u0629 \u0627\u0644\u0633\u0627\u0628\u0642\u0629 \u0625\u0644\u0649 \u0645\u0637\u0639\u0645 \u0627\u0644\u0628\u064a\u062a\u0632\u0627 \u0627\u0644\u0630\u064a \u064a\u0639\u0645\u0644 \u0641\u064a\u0647 \u0641\u0639\u0644\u064a\u064b\u0627.", "\u0643\u064a \u064a\u063a\u0648\u064a\u0647\u0627\u060c \u0643\u0630\u0628 \u0627\u0644\u0645\u063a\u0627\u0644\u0650\u0637 \u0648\u0646\u0633\u0628 \u0625\u0644\u0649 \u0646\u0641\u0633\u0647 \u0648\u0638\u064a\u0641\u0629 \u0645\u0631\u0645\u0648\u0642\u0629. \u0641\u064a \u0627\u0644\u064a\u0648\u0645 \u0627\u0644\u062a\u0627\u0644\u064a\u060c \u062a\u062f\u062e\u0644 \u062d\u0628\u064a\u0628\u062a\u0647 \u0645\u0646 \u0627\u0644\u0644\u064a\u0644\u0629 \u0627\u0644\u0633\u0627\u0628\u0642\u0629 \u0625\u0644\u0649 \u0645\u0637\u0639\u0645 \u0627\u0644\u0628\u064a\u062a\u0632\u0627 \u0627\u0644\u0630\u064a \u064a\u0639\u0645\u0644 \u0641\u064a\u0647 \u0641\u0639\u0644\u064a\u064b\u0627.", "sibling de rangee : le contexte disait \u062d\u0628\u064a\u0628\u062a\u0647 quand FR/EN/es/pt disent tous conquest ; accord verbal \u062a\u062f\u062e\u0644->\u064a\u062f\u062e\u0644 (\u0641\u062a\u0648\u062d masculin)"),
			("3.1.5", "context_fa", "\u0628\u0631\u0627\u06cc \u062f\u0644 \u0628\u0631\u062f\u0646\u060c \u0686\u0631\u0628\u200c\u0632\u0628\u0627\u0646 \u062f\u0631\u0648\u063a \u06af\u0641\u062a\u0647 \u0648 \u0634\u063a\u0644\u06cc \u067e\u0631\u0622\u0628\u200c\u0648\u062a\u0627\u0628 \u0628\u0647 \u062e\u0648\u062f\u0634 \u0646\u0633\u0628\u062a \u062f\u0627\u062f\u0647 \u0627\u0633\u062a. \u0641\u0631\u062f\u0627\u06cc\u0634\u060c \u0634\u06a9\u0627\u0631\u0634 \u0648\u0627\u0631\u062f \u067e\u06cc\u062a\u0632\u0627\u0641\u0631\u0648\u0634\u06cc\u200c\u0627\u06cc \u0645\u06cc\u200c\u0634\u0648\u062f \u06a9\u0647 \u0627\u0648 \u062f\u0631 \u0648\u0627\u0642\u0639 \u0622\u0646\u062c\u0627 \u06a9\u0627\u0631 \u0645\u06cc\u200c\u06a9\u0646\u062f.", "\u0628\u0631\u0627\u06cc \u062f\u0644 \u0628\u0631\u062f\u0646\u060c \u0686\u0631\u0628\u200c\u0632\u0628\u0627\u0646 \u062f\u0631\u0648\u063a \u06af\u0641\u062a\u0647 \u0648 \u0634\u063a\u0644\u06cc \u067e\u0631\u0622\u0628\u200c\u0648\u062a\u0627\u0628 \u0628\u0647 \u062e\u0648\u062f\u0634 \u0646\u0633\u0628\u062a \u062f\u0627\u062f\u0647 \u0627\u0633\u062a. \u0641\u0631\u062f\u0627\u06cc\u0634\u060c \u062f\u0644\u0628\u0631\u0634 \u0648\u0627\u0631\u062f \u067e\u06cc\u062a\u0632\u0627\u0641\u0631\u0648\u0634\u06cc\u200c\u0627\u06cc \u0645\u06cc\u200c\u0634\u0648\u062f \u06a9\u0647 \u0627\u0648 \u062f\u0631 \u0648\u0627\u0642\u0639 \u0622\u0646\u062c\u0627 \u06a9\u0627\u0631 \u0645\u06cc\u200c\u06a9\u0646\u062f.", "sibling de rangee : \u062f\u0644\u0628\u0631\u0634 -> \u0634\u06a9\u0627\u0631\u0634, meme geste que le piocheur \u2014 la carte reste coherente avec elle-meme"),
			("3.1.5", "context_ru", "\u0427\u0442\u043e\u0431\u044b \u0441\u043e\u0431\u043b\u0430\u0437\u043d\u0438\u0442\u044c \u043f\u0430\u0440\u0442\u043d\u0435\u0440\u0448\u0443, \u0441\u043e\u0444\u0438\u0441\u0442 \u0441\u043e\u0432\u0440\u0430\u043b, \u0441\u043a\u0430\u0437\u0430\u0432, \u0447\u0442\u043e \u0443 \u043d\u0435\u0433\u043e \u043f\u0440\u0435\u0441\u0442\u0438\u0436\u043d\u0430\u044f \u0440\u0430\u0431\u043e\u0442\u0430. \u041d\u0430 \u0441\u043b\u0435\u0434\u0443\u044e\u0449\u0438\u0439 \u0434\u0435\u043d\u044c \u0435\u0433\u043e \u043f\u0430\u0441\u0441\u0438\u044f \u0432\u043e\u0448\u043b\u0430 \u0432 \u043f\u0438\u0446\u0446\u0435\u0440\u0438\u044e, \u0433\u0434\u0435 \u043e\u043d \u043d\u0430 \u0441\u0430\u043c\u043e\u043c \u0434\u0435\u043b\u0435 \u0440\u0430\u0431\u043e\u0442\u0430\u0435\u0442.", "\u0427\u0442\u043e\u0431\u044b \u0441\u043e\u0431\u043b\u0430\u0437\u043d\u0438\u0442\u044c \u043f\u0430\u0440\u0442\u043d\u0435\u0440\u0448\u0443, \u0441\u043e\u0444\u0438\u0441\u0442 \u0441\u043e\u0432\u0440\u0430\u043b, \u0441\u043a\u0430\u0437\u0430\u0432, \u0447\u0442\u043e \u0443 \u043d\u0435\u0433\u043e \u043f\u0440\u0435\u0441\u0442\u0438\u0436\u043d\u0430\u044f \u0440\u0430\u0431\u043e\u0442\u0430. \u041d\u0430 \u0441\u043b\u0435\u0434\u0443\u044e\u0449\u0438\u0439 \u0434\u0435\u043d\u044c \u0435\u0433\u043e \u0438\u0437\u0431\u0440\u0430\u043d\u043d\u0438\u0446\u0430 \u0432\u043e\u0448\u043b\u0430 \u0432 \u043f\u0438\u0446\u0446\u0435\u0440\u0438\u044e, \u0433\u0434\u0435 \u043e\u043d \u043d\u0430 \u0441\u0430\u043c\u043e\u043c \u0434\u0435\u043b\u0435 \u0440\u0430\u0431\u043e\u0442\u0430\u0435\u0442.", "sibling de rangee : \u0438\u0437\u0431\u0440\u0430\u043d\u043d\u0438\u0446\u0430 -> \u043f\u0430\u0441\u0441\u0438\u044f, coherence avec le piocheur et avec 3.1.2"),
			("6.2.1", "title_zh", "\u534f\u5546\u9000\u9009", "\u521d\u9009\u8fde\u73af\u8df3", "\u534f\u5546\u9000\u9009 rend negocie + retrait ; le contexte zh de la carte dit deja \u8c08\u59a5\u9000\u9009 ; \u521d\u9009\u8fde\u73af\u8df3 etait un titre invente"),
			("6.2.1", "title_ru", "\u0423\u0445\u043e\u0434 \u043f\u043e \u0434\u043e\u0433\u043e\u0432\u043e\u0440\u0451\u043d\u043d\u043e\u0441\u0442\u0438", "\u0420\u043e\u043a\u0438\u0440\u043e\u0432\u043a\u0430", "\u0423\u0445\u043e\u0434 \u043f\u043e \u0434\u043e\u0433\u043e\u0432\u043e\u0440\u0451\u043d\u043d\u043e\u0441\u0442\u0438 : \u0443\u0445\u043e\u0434 (le retrait politique usuel) + \u043f\u043e \u0434\u043e\u0433\u043e\u0432\u043e\u0440\u0451\u043d\u043d\u043e\u0441\u0442\u0438 (negocie) ; le contexte ru dit deja \u00ab\u0434\u043e\u0433\u043e\u0432\u043e\u0440\u0438\u043b\u0441\u044f \u0441\u043d\u044f\u0442\u044c \u0441\u0432\u043e\u044e \u043a\u0430\u043d\u0434\u0438\u0434\u0430\u0442\u0443\u0440\u0443\u00bb ; \u00ab\u0420\u043e\u043a\u0438\u0440\u043e\u0432\u043a\u0430\u00bb etait invente"),
			("5.3.5", "suggestion_fa", "\u0648 \u0627\u06af\u0631 \u0686\u06cc\u0632\u06cc \u06a9\u0647 \u0645\u0627 \u0622\u0646 \u0631\u0627 \u00ab\u06af\u0631\u0627\u0646\u0634\u00bb \u0645\u06cc\u200c\u0646\u0627\u0645\u06cc\u0645\u060c \u067e\u062f\u06cc\u062f\u0647\u200c\u0627\u06cc \u0628\u0627\u0634\u062f \u06a9\u0647 \u0647\u0645\u06cc\u0634\u0647 \u0627\u0634\u062a\u0628\u0627\u0647 \u0641\u0647\u0645\u06cc\u062f\u0647\u200c\u0627\u06cc\u0645\u061f", "\u0632\u0645\u06cc\u0646 \u0645\u0627 \u0631\u0627 \u0627\u0632 \u062e\u0648\u062f\u0634 \u062f\u0648\u0631 \u0645\u06cc\u200c\u0631\u0627\u0646\u062f\u061f \u0686\u0647 \u0641\u06a9\u0631 \u06a9\u0627\u0645\u0644\u0627\u064b \u0639\u062c\u06cc\u0628\u200c\u0648\u063a\u0631\u06cc\u0628\u06cc!", "la question \u00abet si\u00bb du physicien restauree, avec \u06af\u0631\u0627\u0646\u0634 = le terme du TITRE fa de cette carte (\u06af\u0631\u0627\u0646\u0634 \u0648\u0627\u0631\u0648\u0646\u0647) ; l'exclamation generique avait remplace la riposte"),
			("7.2.7", "suggestion_ru", "\u041e\u0445, \u0438\u0437\u0432\u0438\u043d\u0438, \u043d\u043e \u0443 \u043c\u0435\u043d\u044f \u043d\u0430\u043a\u043e\u043d\u0435\u0446-\u0442\u043e \u0441\u043e\u0431\u0435\u0441\u0435\u0434\u043e\u0432\u0430\u043d\u0438\u0435!", "\u0421\u043b\u0443\u0448\u0430\u0439, \u0432 \u0438\u0442\u043e\u0433\u0435 \u044f \u043d\u0435 \u0441\u043c\u043e\u0433\u0443 \u0441 \u0442\u043e\u0431\u043e\u0439 \u043f\u043e\u0435\u0445\u0430\u0442\u044c.", "\u0441\u043e\u0431\u0435\u0441\u0435\u0434\u043e\u0432\u0430\u043d\u0438\u0435 = l'entretien, pivot de la carte (le contexte et l'enjeu ru le disent deja) ; \u043d\u0430\u043a\u043e\u043d\u0435\u0446-\u0442\u043e = enfin ; la replique etait amputee de l'entretien ET de l'excuse"),
		};

		/// <summary>Les 13 formes vieilles (10 cellules pleines + 3 fragments de contexte), chacune
		/// mesuree 1+ occurrence avant et 0 apres. Les mots nus (شکار x13, دلبر, избранница) restent
		/// hors ecran : ils vivent legitimement ailleurs ou pourraient y vivre.</summary>
		private static readonly string[] EradicatedForms =
		{
			"\u0627\u0633\u062a\u064a\u0642\u0627\u0638 \u063a\u064a\u0631 \u0645\u062a\u0648\u0642\u0639 \u0648\u0645\u0623\u0632\u0642 \u062d\u0633\u0627\u0633",
			"\u0628\u06cc\u062f\u0627\u0631\u06cc\u0650 \u063a\u06cc\u0631\u0645\u0646\u062a\u0638\u0631\u0647 \u0648 \u0645\u0635\u0627\u0644\u062d\u0647\u200c\u0627\u06cc \u062d\u0633\u0627\u0633",
			"\u610f\u5916\u9192\u6765\u4e0e\u5fae\u5999\u59a5\u534f",
			"\u062d\u0628\u064a\u0628\u062a\u0647 \u0645\u0646 \u0627\u0644\u0644\u064a\u0644\u0629 \u0627\u0644\u0633\u0627\u0628\u0642\u0629",
			"\u062f\u0644\u0628\u0631 \u062f\u06cc\u0634\u0628\u06cc\u200c\u0627\u0634",
			"\u041d\u043e\u0432\u0430\u044f \u0438\u0437\u0431\u0440\u0430\u043d\u043d\u0438\u0446\u0430",
			"\u521d\u9009\u8fde\u73af\u8df3",
			"\u0420\u043e\u043a\u0438\u0440\u043e\u0432\u043a\u0430",
			"\u0632\u0645\u06cc\u0646 \u0645\u0627 \u0631\u0627 \u0627\u0632 \u062e\u0648\u062f\u0634 \u062f\u0648\u0631 \u0645\u06cc\u200c\u0631\u0627\u0646\u062f\u061f \u0686\u0647 \u0641\u06a9\u0631 \u06a9\u0627\u0645\u0644\u0627\u064b \u0639\u062c\u06cc\u0628\u200c\u0648\u063a\u0631\u06cc\u0628\u06cc!",
			"\u0421\u043b\u0443\u0448\u0430\u0439, \u0432 \u0438\u0442\u043e\u0433\u0435 \u044f \u043d\u0435 \u0441\u043c\u043e\u0433\u0443 \u0441 \u0442\u043e\u0431\u043e\u0439 \u043f\u043e\u0435\u0445\u0430\u0442\u044c.",
			"\u062a\u062f\u062e\u0644 \u062d\u0628\u064a\u0628\u062a\u0647 \u0645\u0646 \u0627\u0644\u0644\u064a\u0644\u0629 \u0627\u0644\u0633\u0627\u0628\u0642\u0629",
			"\u062f\u0644\u0628\u0631\u0634 \u0648\u0627\u0631\u062f",
			"\u0435\u0433\u043e \u0438\u0437\u0431\u0440\u0430\u043d\u043d\u0438\u0446\u0430 \u0432\u043e\u0448\u043b\u0430"
		};

		internal static List<string> Mismatches(
			IEnumerable<(string, string, string, string)> cells)
		{
			var offenders = new List<string>();
			foreach (var (path, column, expected, actual) in cells)
				if (!string.Equals(expected, actual, StringComparison.Ordinal))
					offenders.Add($"{path}.{column} : attendu «{expected}», lu «{actual}»");
			return offenders;
		}

		private static List<(string, string, string, string)> ReadAll(
			IEnumerable<(string Path, string Column, string Expected, string Was, string Why)> table)
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var read = new List<(string, string, string, string)>();
			foreach (var (path, column, expected, _, _) in table)
			{
				var v = csv.LoadColumn(column, "path", new[] { path });
				v.Should().HaveCount(1, $"la rangee {path} existe et est unique.");
				read.Add((path, column, expected, v[0]));
			}
			return read;
		}

		[Fact]
		public void Composed_Cells_Carry_The_Composed_Forms()
		{
			Mismatches(ReadAll(Composed)).Should().BeEmpty(
				"#458 grain 5 : les 13 compositions sont ecrites — revoir une seule doit nommer la rangee et la colonne.");
		}

		[Fact]
		public void Composed_Table_Is_Not_Vacuous()
		{
			Composed.Should().HaveCount(13, "10 compositions differrees + 3 siblings de rangee (3.1.5 contextes).");
			Composed.Select(f => f.Path).Distinct().Should().HaveCount(5)
				.And.BeEquivalentTo(new[] { "3.1.5", "4.2.8", "5.3.5", "6.2.1", "7.2.7" });
			Composed.Count(f => f.Column.StartsWith("title_")).Should().Be(5, "4.2.8 x3 + 6.2.1 x2.");
			Composed.Count(f => f.Column.StartsWith("drawer_")).Should().Be(3, "3.1.5 ar/fa/ru.");
			Composed.Count(f => f.Column.StartsWith("context_")).Should().Be(3, "3.1.5 ar/fa/ru (siblings).");
			Composed.Count(f => f.Column.StartsWith("suggestion_")).Should().Be(2, "5.3.5 fa + 7.2.7 ru.");
			Composed.Should().OnlyContain(f => f.Why.Length > 30, "chaque composition dit ce qu'elle rend et sur quel modele.");
			Composed.Select(f => f.Why).Distinct().Should().HaveCount(13, "13 raisons distinctes : une recopiee n'explique rien.");
			Composed.Select(f => f.Expected).Distinct().Should().HaveCount(13, "13 cellules, 13 formes distinctes.");
			Composed.Select(f => f.Was).Distinct().Should().HaveCount(13, "13 anciennes valeurs distinctes.");
		}

		[Fact]
		public void Old_Forms_Are_Eradicated_From_The_Corpus()
		{
			var corpus = File.ReadAllText(ScenariiCsv, System.Text.Encoding.UTF8);
			var found = EradicatedForms.Where(f => corpus.Contains(f, StringComparison.Ordinal)).ToList();
			found.Should().BeEmpty(
				"les 13 formes vieilles ont ete mesurees 1+ avant et 0 apres : une reapparition signale une regression.");
		}

		[Fact]
		public void Corpus_Witnesses_Survive_And_Coherence_Holds()
		{
			var corpus = File.ReadAllText(ScenariiCsv, System.Text.Encoding.UTF8);
			corpus.Should().Contain("\u043f\u0430\u0441\u0441\u0438\u0435\u0439", "le contexte de 3.1.2 (pre-existant) dit deja «\u0432\u0447\u0435\u0440\u0430\u0448\u043d\u0435\u0439 \u043f\u0430\u0441\u0441\u0438\u0435\u0439» : le corpus attestait la collocation composee ici pour 3.1.5.");
			corpus.Should().Contain("\u8c08\u59a5\u9000\u9009", "le contexte zh de 6.2.1 dit deja «negocie le retrait» : le titre compose suit la carte.");
			corpus.Should().Contain("\u0441\u043e\u0431\u0435\u0441\u0435\u0434\u043e\u0432\u0430\u043d\u0438\u0435", "contexte et enjeu ru de 7.2.7 disent deja l'entretien : la replique composee rejoint la carte.");
			corpus.Should().Contain("\u0634\u06a9\u0627\u0631", "le mot شکار vivait deja 13 fois legitiment ailleurs : l'ecran ne visait que la collocation.");
		}

		[Fact]
		public void Detector_Fires_On_A_Reverted_Cell_And_Spares_The_Witness()
		{
			var i621 = Composed.Single(f => f.Path == "6.2.1" && f.Column == "title_zh");
			var i727 = Composed.Single(f => f.Path == "7.2.7" && f.Column == "suggestion_ru");
			var reverted = new[]
			{
				(i621.Path, i621.Column, i621.Expected, i621.Was),
				(i727.Path, i727.Column, i727.Expected, i727.Expected),
			};
			var offenders = Mismatches(reverted);
			offenders.Should().HaveCount(1, "seule la cellule revertee doit rougir : le temoin sain reste vert.");
			offenders[0].Should().Contain("6.2.1.title_zh");
		}
	}
}
