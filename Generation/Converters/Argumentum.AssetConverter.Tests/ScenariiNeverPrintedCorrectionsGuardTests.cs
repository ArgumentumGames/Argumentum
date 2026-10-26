using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// File profonde c.5993735448 grain 3 (3b) — les 15 cellules EN JAMAIS imprimees
	/// (dossier owner c.5990803984, Annexe C : « correction libre ») + les 23 siblings
	/// qui portent le meme defaut (regle du GO : meme defaut, meme rangee, meme PR).
	/// Source de la worklist : dossier
	/// <c>docs/translation/458-fidelite-scenarii-en-axe-imprime-2026-10-04.md</c> (§4).
	/// <para>Chirurgie par span de champ : 38 cellules exactes, 14 rangees, delta +310
	/// octets, 167 enregistrements, pas de BOM ; 3 cellules portent une virgule dans la
	/// nouvelle valeur et sont desormais quotées (6.1.1.en/es, 7.2.8.en) — une virgule nue
	/// eut coupe le champ (defaut reel du 1er run du script, attrape par le re-assert
	/// fail-closed AVANT toute ecriture). Les valeurs de cette table sont EMISES depuis la
	/// mesure (CSV courant), jamais recopiees a la main.</para>
	/// </summary>
	public class ScenariiNeverPrintedCorrectionsGuardTests
	{
		private static string ScenariiCsv => System.IO.Path.Combine(
			TestRepoRoot.Find(), "Cards", "Scenarii", "Argumentum Scenarii - Cards.csv");

		/// <summary>Les 38 cellules ecrites : rangee, colonne, valeur pleine attendue, valeur d'avant, la raison.</summary>
		private static readonly (string Path, string Column, string Expected, string Was, string Why)[] Restored =
		{
			("5.3.1", "title", "Debate with a Flat Earther", "Flat Earth Society", "FR \u00ABD\u00E9bat avec un terraplaniste\u00BB : le titre nommait l'ORGANISATION, la carte est le D\u00C9BAT ; le ru corrig\u00E9 (g1) dit d\u00E9j\u00E0 \u00AB\u0414\u0435\u0431\u0430\u0442\u044B \u0441 \u043F\u043B\u043E\u0441\u043A\u043E\u0437\u0435\u043C\u0435\u043B\u044C\u0446\u0435\u043C\u00BB"),
			("5.3.4", "title", "The 5G Conspiracy", "5g", "FR \u00ABLa conspiration de la 5G\u00BB : \u00AB5g\u00BB nu nomme la technologie, pas la th\u00E8se ; le contexte dit \u00ABthe 5G is responsible\u00BB"),
			("5.3.1", "issue", "He must convince the audience that the cosmonaut is an impostor.", "the latter is an impostor", "FR \u00ABque le cosmonaute est un imposteur\u00BB : \u00ABthe latter\u00BB est un renvoi flou, le FR nomme le personnage introduit par le contexte"),
			("6.1.4", "suggestion_en", "Our country was prepared. What happened?", "Our country had prepared.", "voix : \u00ABhad prepared\u00BB (action revendiqu\u00E9e) alors que le contexte montre des stocks D\u00C9TRUITS ; \u00ABwas prepared\u00BB d\u00E9crit l'\u00E9tat"),
			("6.2.2", "context", "The smooth talker has granted the public cemeteries management market to a cassoulet manufacturer, with a bribe. There are pieces of corpses in the cans.", "with an occult payment", "FR \u00ABmoyennant un pot-de-vin\u00BB : \u00ABoccult payment\u00BB est un euph\u00E9misme calqu\u00E9 ; \u00ABbribe\u00BB est le mot"),
			("3.2.15", "title", "The Stained T-Shirt", "The Spaghetti T-Shirt", "FR \u00ABLe t-shirt tach\u00E9\u00BB : le titre d\u00E9crivait la CAUSE (spaghetti) au lieu de l'\u00C9TAT (tach\u00E9) ; les 5 siblings suivent"),
			("6.1.1", "suggestion_en", "To restore trust, we need clear rules: the limitation period must run from the facts, as in ordinary law.", "It is absolutely necessary to rebuild the people's confidence in the political class.", "l'enjeu FR porte le M\u00C9CANISME (le d\u00E9lai de prescription courant de la date du d\u00E9lit) ; l'EN le rempla\u00E7ait par un appel vague"),
			("5.2.5", "issue", "He must convince Rachel that he did not cheat on her.", "She considers it an infidelity: try to prove her wrong.", "FR \u00ABIl doit convaincre Rachel qu'il ne l'a pas tromp\u00E9e\u00BB : Rachel n'\u00E9tait pas nomm\u00E9e, la t\u00E2che reformul\u00E9e en r\u00E9futation abstraite"),
			("7.2.8", "suggestion_en", "I'm glad to see you again too, but don't you think you're getting a bit ahead of yourself?", "Me too it makes me happy to see you again, but don't you think you're going to work a bit?", "contresens : \u00ABgoing to work a bit?\u00BB pour un ami qui demande de l'argent trop vite ; le zh porte d\u00E9j\u00E0 \u00AB\u592A\u5FC3\u6025\u00BB, la r\u00E9f\u00E9rence"),
			("5.3.2", "issue", "He must convince his neighbor, a person at risk and priority, to refuse the vaccine.", "to refuse to benefit from it", "FR \u00ABde refuser le vaccin\u00BB : \u00ABto benefit from it\u00BB ne renvoie \u00E0 rien ; le r\u00E9f\u00E9rent est le VACCIN introduit par le contexte"),
			("3.2.8", "context", "The smooth talker has completely forgotten their partner's birthday.", "all-important birthday", "\u00ABall-important\u00BB intensificateur absent du FR (\u00ABa compl\u00E8tement oubli\u00E9 l'anniversaire\u00BB) ; les 3 siblings dessous l'avaient recopi\u00E9"),
			("4.3.3", "context", "The smooth talker had a stock of the national football team jerseys delivered in anticipation of the World Cup where it was the favorite.", "where it was given favorite", "FR \u00ABdonn\u00E9e favorite\u00BB : \u00ABgiven\u00BB est un parasite de calque ; \u00ABthe favorite\u00BB est le terme"),
			("7.3.4", "title", "Goal!", "Gooal!", "coquille : l'\u00E9longation FR \u00ABBuuut !\u00BB est un cri, l'EN l'avait fig\u00E9e en \u00ABGooal\u00BB ; \u00ABGoal!\u00BB est le mot"),
			("7.2.5", "issue", "He tries to convince his neighbour, a foie gras producer, to convert to the production of lacto-fermented tofu.", "He must convince his neighbour", "FR \u00ABIl TENTE de convaincre\u00BB : l'EN disait \u00ABmust\u00BB (obligation au lieu de la tentative) ; les 3 siblings dessous suivent"),
			("7.1.6", "title", "The inheritance", "The inheritence", "coquille \u00ABinheritence\u00BB ; FR \u00ABL'h\u00E9ritage\u00BB"),
			("3.2.15", "title_ar", "\u0627\u0644\u0642\u0645\u064A\u0635 \u0627\u0644\u0645\u0644\u0637\u0651\u062E", "\u0642\u0645\u064A\u0635 \u0627\u0644\u0633\u0628\u0627\u063A\u064A\u062A\u064A", "m\u00EAme d\u00E9faut que l'EN : le t-shirt DE SPAGHETTI au lieu de l'\u00E9tat ; l'arabe porte \u00ABtach\u00E9\u00BB"),
			("3.2.15", "title_fa", "\u062A\u06CC\u200C\u0634\u0631\u062A \u0644\u06A9\u0647\u200C\u062F\u0627\u0631", "\u062A\u06CC\u200C\u0634\u0631\u062A \u0627\u0633\u067E\u0627\u06AF\u062A\u06CC\u200C\u062E\u0648\u0631\u062F\u0647", "m\u00EAme d\u00E9faut : \u00ABqui a mang\u00E9 des spaghettis\u00BB au lieu de \u00ABtach\u00E9\u00BB"),
			("3.2.15", "title_es", "La camiseta manchada", "La camiseta de espaguetis", "m\u00EAme d\u00E9faut : \u00ABde espaguetis\u00BB ; \u00ABmanchada\u00BB = tach\u00E9e, comme le FR"),
			("3.2.15", "title_ru", "\u0418\u0441\u043F\u0430\u0447\u043A\u0430\u043D\u043D\u0430\u044F \u0444\u0443\u0442\u0431\u043E\u043B\u043A\u0430", "\u0424\u0443\u0442\u0431\u043E\u043B\u043A\u0430 \u0441\u043E \u0441\u043F\u0430\u0433\u0435\u0442\u0442\u0438", "m\u00EAme d\u00E9faut : \u00AB\u0444\u0443\u0442\u0431\u043E\u043B\u043A\u0430 \u0441\u043E \u0441\u043F\u0430\u0433\u0435\u0442\u0442\u0438\u00BB ; \u00AB\u0438\u0441\u043F\u0430\u0447\u043A\u0430\u043D\u043D\u0430\u044F\u00BB = tach\u00E9e, comme le FR"),
			("3.2.15", "title_pt", "A T-shirt manchada", "A T-shirt com esparguete", "m\u00EAme d\u00E9faut : \u00ABcom esparguete\u00BB ; \u00ABmanchada\u00BB = tach\u00E9e, comme le FR"),
			("3.2.8", "context_fa", "\u0686\u0631\u0628\u200C\u0632\u0628\u0627\u0646\u060C \u0633\u0627\u0644\u06AF\u0631\u062F \u0634\u0631\u06CC\u06A9 \u0632\u0646\u062F\u06AF\u06CC\u200C\u0627\u0634 \u0631\u0627 \u06A9\u0627\u0645\u0644\u0627\u064B \u0627\u0632 \u06CC\u0627\u062F \u0628\u0631\u062F\u0647 \u0627\u0633\u062A.", "\u0633\u0627\u0644\u06AF\u0631\u062F \u0628\u0633\u06CC\u0627\u0631 \u0645\u0647\u0645 \u0634\u0631\u06CC\u06A9", "m\u00EAme d\u00E9faut que l'EN : le superlatif \u00ABle plus important\u00BB ajout\u00E9 au jour d'anniversaire, absent du FR"),
			("3.2.8", "context_ru", "\u0421\u043E\u0444\u0438\u0441\u0442 \u043D\u0430\u043F\u0440\u043E\u0447\u044C \u0437\u0430\u0431\u044B\u043B \u0434\u0435\u043D\u044C \u0440\u043E\u0436\u0434\u0435\u043D\u0438\u044F \u0441\u0432\u043E\u0435\u0433\u043E \u043F\u0430\u0440\u0442\u043D\u0435\u0440\u0430.", "\u0437\u0430\u0431\u044B\u043B \u0432\u0430\u0436\u043D\u0435\u0439\u0448\u0438\u0439 \u0434\u0435\u043D\u044C", "m\u00EAme d\u00E9faut : \u00AB\u0432\u0430\u0436\u043D\u0435\u0439\u0448\u0438\u0439\u00BB (le plus important) ajout\u00E9 au jour d'anniversaire, absent du FR"),
			("3.2.8", "context_pt", "O Embromador esqueceu-se completamente do anivers\u00E1rio da sua cara-metade.", "anivers\u00E1rio important\u00EDssimo", "m\u00EAme d\u00E9faut : \u00ABimportant\u00EDssimo\u00BB, superlatif absent du FR"),
			("5.2.5", "issue_zh", "\u8BE1\u8FA9\u8005\u5FC5\u987B\u8BF4\u670D\u745E\u79CB\uFF0C\u4ED6\u5E76\u6CA1\u6709\u51FA\u8F68\u3002", "\u8BE1\u8FA9\u8005\u5FC5\u987B\u8BF4\u670D\u5979\uFF0C\u4E8B\u5B9E\u5E76\u975E\u5982\u6B64\u3002", "m\u00EAme d\u00E9faut : \u00ABla convaincre, les faits disent le contraire\u00BB \u2014 Rachel disparue ; la translitt\u00E9ration vient du contexte zh de la carte"),
			("5.2.5", "issue_ar", "\u0639\u0644\u0649 \u0627\u0644\u0645\u063A\u0627\u0644\u0650\u0637 \u0623\u0646 \u064A\u0642\u0646\u0639 \u0631\u0627\u064A\u062A\u0634\u0644 \u0628\u0623\u0646\u0647 \u0644\u0645 \u064A\u062E\u0646\u0647\u0627.", "\u0639\u0644\u0649 \u0627\u0644\u0645\u063A\u0627\u0644\u0650\u0637 \u0623\u0646 \u064A\u0642\u0646\u0639\u0647\u0627 \u0628\u0639\u0643\u0633 \u0630\u0644\u0643.", "m\u00EAme d\u00E9faut : \u00ABla convaincre du contraire\u00BB \u2014 Rachel nomm\u00E9e comme dans le FR, forme du contexte ar"),
			("5.2.5", "issue_fa", "\u0686\u0631\u0628\u200C\u0632\u0628\u0627\u0646 \u0628\u0627\u06CC\u062F \u0631\u06CC\u0686\u0644 \u0631\u0627 \u0642\u0627\u0646\u0639 \u06A9\u0646\u062F \u06A9\u0647 \u0628\u0647 \u0627\u0648 \u062E\u06CC\u0627\u0646\u062A \u0646\u06A9\u0631\u062F\u0647 \u0627\u0633\u062A.", "\u0686\u0631\u0628\u200C\u0632\u0628\u0627\u0646 \u0628\u0627\u06CC\u062F \u0627\u0648 \u0631\u0627 \u0642\u0627\u0646\u0639 \u06A9\u0646\u062F \u06A9\u0647 \u0645\u0627\u062C\u0631\u0627 \u0627\u06CC\u0646\u200C\u0637\u0648\u0631 \u0646\u06CC\u0633\u062A.", "m\u00EAme d\u00E9faut : \u00ABla convaincre que ce n'est pas ainsi\u00BB \u2014 Rachel nomm\u00E9e, forme du contexte fa"),
			("5.2.5", "issue_es", "Debe convencer a Rachel de que no le ha sido infiel.", "Convencerla de lo contrario.", "m\u00EAme d\u00E9faut : \u00ABla convaincre du contraire\u00BB \u2014 Rachel nomm\u00E9e comme dans le FR"),
			("5.3.2", "issue_ar", "\u0639\u0644\u064A\u0647 \u0623\u0646 \u064A\u0642\u0646\u0639 \u062C\u0627\u0631\u0647\u060C \u0648\u0647\u0648 \u0634\u062E\u0635 \u0645\u0639\u0631\u0651\u0636 \u0644\u0644\u062E\u0637\u0631 \u0648\u0644\u0647 \u0623\u0648\u0644\u0648\u064A\u0629\u060C \u0628\u0631\u0641\u0636 \u0627\u0644\u0644\u0642\u0627\u062D.", "\u0628\u0631\u0641\u0636 \u0627\u0644\u0627\u0633\u062A\u0641\u0627\u062F\u0629 \u0645\u0646\u0647.", "m\u00EAme d\u00E9faut que l'EN : \u00ABrefuser d'en b\u00E9n\u00E9ficier\u00BB \u2014 le renvoi flou remplac\u00E9 par le vaccin"),
			("5.3.2", "issue_fa", "\u0627\u0648 \u0628\u0627\u06CC\u062F \u0647\u0645\u0633\u0627\u06CC\u0647\u200C\u0627\u0634 \u0631\u0627 \u06A9\u0647 \u0641\u0631\u062F\u06CC \u062F\u0631 \u0645\u0639\u0631\u0636 \u062E\u0637\u0631 \u0648 \u062F\u0631 \u0627\u0648\u0644\u0648\u06CC\u062A \u0627\u0633\u062A\u060C \u0642\u0627\u0646\u0639 \u06A9\u0646\u062F \u06A9\u0647 \u0648\u0627\u06A9\u0633\u0646 \u0646\u0632\u0646\u062F.", "\u0642\u0627\u0646\u0639 \u06A9\u0646\u062F \u06A9\u0647 \u0627\u0632 \u0627\u06CC\u0646 \u0627\u0645\u06A9\u0627\u0646 \u0627\u0633\u062A\u0641\u0627\u062F\u0647 \u0646\u06A9\u0646\u062F.", "m\u00EAme d\u00E9faut : \u00ABne pas profiter de cette possibilit\u00E9\u00BB \u2014 remplac\u00E9 par le vaccin"),
			("5.3.2", "issue_es", "Convencer a su vecino, persona de riesgo y prioritaria, de rechazar la vacuna.", "de rechazar beneficiarse de ella", "m\u00EAme d\u00E9faut : \u00ABrechazar beneficiarse de ella\u00BB \u2014 remplac\u00E9 par la vacuna"),
			("6.1.1", "suggestion_zh", "\u4E3A\u4E86\u91CD\u5EFA\u4FE1\u4EFB\uFF0C\u6211\u4EEC\u9700\u8981\u660E\u786E\u7684\u89C4\u5219\uFF1A\u65F6\u6548\u5E94\u5F53\u4ECE\u4E8B\u5B9E\u53D1\u751F\u4E4B\u65E5\u8D77\u7B97\uFF0C\u4E00\u5982\u4E00\u822C\u6CD5\u5F8B\u3002", "\u6211\u4EEC\u5FC5\u987B\u91CD\u5EFA\u4EBA\u6C11\u5BF9\u653F\u6CBB\u9636\u5C42\u7684\u4FE1\u4EFB\u3002", "m\u00EAme d\u00E9faut que l'EN : la confiance reformul\u00E9e SANS le m\u00E9canisme (prescription courant de la date du fait)"),
			("6.1.1", "suggestion_ar", "\u0644\u0627\u0633\u062A\u0639\u0627\u062F\u0629 \u0627\u0644\u062B\u0642\u0629\u060C \u0644\u0627 \u0628\u062F\u0651 \u0645\u0646 \u0642\u0648\u0627\u0639\u062F \u0648\u0627\u0636\u062D\u0629: \u064A\u062C\u0628 \u0623\u0646 \u064A\u0633\u0631\u064A \u0627\u0644\u062A\u0642\u0627\u062F\u0645 \u0627\u0646\u0637\u0644\u0627\u0642\u064B\u0627 \u0645\u0646 \u0627\u0644\u0648\u0642\u0627\u0626\u0639\u060C \u0643\u0645\u0627 \u0641\u064A \u0627\u0644\u0642\u0627\u0646\u0648\u0646 \u0627\u0644\u0639\u0627\u062F\u064A.", "\u0644\u0627 \u0628\u062F\u0651 \u0645\u0646 \u0625\u0639\u0627\u062F\u0629 \u0628\u0646\u0627\u0621 \u062B\u0642\u0629 \u0627\u0644\u0634\u0639\u0628 \u0628\u0627\u0644\u0637\u0628\u0642\u0629 \u0627\u0644\u0633\u064A\u0627\u0633\u064A\u0629.", "m\u00EAme d\u00E9faut : la prescription courant des faits \u2014 le m\u00E9canisme de l'enjeu FR"),
			("6.1.1", "suggestion_fa", "\u0628\u0631\u0627\u06CC \u0628\u0627\u0632\u06AF\u0631\u062F\u0627\u0646\u062F\u0646 \u0627\u0639\u062A\u0645\u0627\u062F\u060C \u0628\u0627\u06CC\u062F \u0642\u0648\u0627\u0639\u062F\u06CC \u0631\u0648\u0634\u0646 \u062F\u0627\u0634\u062A\u0647 \u0628\u0627\u0634\u06CC\u0645: \u062F\u0648\u0631\u0647\u0654 \u0645\u0631\u0648\u0631 \u0632\u0645\u0627\u0646 \u0628\u0627\u06CC\u062F \u0627\u0632 \u0632\u0645\u0627\u0646 \u0648\u0642\u0648\u0639 \u0648\u0627\u0642\u0639\u06CC\u062A\u200C\u0647\u0627 \u0622\u063A\u0627\u0632 \u0634\u0648\u062F\u060C \u0647\u0645\u0627\u0646\u200C\u0637\u0648\u0631 \u06A9\u0647 \u062F\u0631 \u062D\u0642\u0648\u0642 \u0639\u0627\u062F\u06CC \u0627\u0633\u062A.", "\u0628\u0627\u06CC\u062F \u0647\u0631 \u0637\u0648\u0631 \u0634\u062F\u0647 \u0627\u0639\u062A\u0645\u0627\u062F \u0645\u0631\u062F\u0645 \u0628\u0647 \u0637\u0628\u0642\u0647\u0654 \u0633\u06CC\u0627\u0633\u06CC \u0631\u0627 \u0628\u0627\u0632\u0633\u0627\u0632\u06CC \u06A9\u0646\u06CC\u0645.", "m\u00EAme d\u00E9faut : le d\u00E9lai de prescription courant depuis les faits \u2014 le m\u00E9canisme de l'enjeu FR"),
			("6.1.1", "suggestion_es", "Para restaurar la confianza hacen falta reglas claras: el plazo debe correr a partir de los hechos, como en derecho com\u00FAn.", "Es absolutamente necesario reconstruir la confianza del pueblo en la clase pol\u00EDtica.", "m\u00EAme d\u00E9faut : \u00ABel plazo debe correr a partir de los hechos\u00BB \u2014 le m\u00E9canisme de l'enjeu FR"),
			("7.2.8", "suggestion_pt", "Eu tamb\u00E9m fico feliz em v\u00EA-lo novamente, mas voc\u00EA n\u00E3o acha que est\u00E1 indo um pouco r\u00E1pido demais?", "mas voc\u00EA n\u00E3o acha que vai trabalhar um pouco?", "m\u00EAme contresens que l'EN (\u00ABaller travailler\u00BB) ; la lecture est \u00ABtrop press\u00E9\u00BB, comme le zh"),
			("7.2.5", "issue_ar", "\u064A\u062D\u0627\u0648\u0644 \u0623\u0646 \u064A\u0642\u0646\u0639 \u062C\u0627\u0631\u0647 \u0645\u0646\u062A\u062C \u0627\u0644\u0641\u0648\u0627 \u063A\u0631\u0627 \u0628\u0627\u0644\u062A\u062D\u0648\u0644 \u0625\u0644\u0649 \u0625\u0646\u062A\u0627\u062C \u0627\u0644\u062A\u0648\u0641\u0648 \u0627\u0644\u0645\u062E\u0645\u0651\u0631 \u0644\u0628\u0646\u064A\u064B\u0627.", "\u064A\u062F\u0641\u0639 \u062C\u0627\u0631\u0647 \u0645\u0646\u062A\u062C \u0627\u0644\u0641\u0648\u0627 \u063A\u0631\u0627 \u0625\u0644\u0649 \u062A\u063A\u064A\u064A\u0631 \u0646\u0634\u0627\u0637\u0647 \u0646\u062D\u0648 \u0625\u0646\u062A\u0627\u062C \u0627\u0644\u062A\u0648\u0641\u0648 \u0627\u0644\u0645\u062E\u0645\u0651\u0631 \u0644\u0628\u0646\u064A\u064B\u0627.", "m\u00EAme d\u00E9faut que l'EN : pousser (obligation) au lieu de tenter, comme le FR \u00ABIl tente\u00BB"),
			("7.2.5", "issue_fa", "\u0627\u0648 \u0633\u0639\u06CC \u0645\u06CC\u200C\u06A9\u0646\u062F \u0647\u0645\u0633\u0627\u06CC\u0647 \u062A\u0648\u0644\u06CC\u062F\u06A9\u0646\u0646\u062F\u0647 \u0641\u0648\u0627 \u06AF\u0631\u0627\u0633\u0634 \u0631\u0627 \u0642\u0627\u0646\u0639 \u06A9\u0646\u062F \u06A9\u0647 \u0628\u0647 \u062A\u0648\u0644\u06CC\u062F \u062A\u0648\u0641\u0648\u06CC \u0644\u0627\u06A9\u062A\u0648\u0641\u0631\u0645\u0627\u0646\u062A\u0647 \u062A\u063A\u06CC\u06CC\u0631 \u0645\u0633\u06CC\u0631 \u062F\u0647\u062F.", "\u0627\u0648 \u0647\u0645\u0633\u0627\u06CC\u0647 \u062A\u0648\u0644\u06CC\u062F\u06A9\u0646\u0646\u062F\u0647 \u0641\u0648\u0627 \u06AF\u0631\u0627\u0633\u0634 \u0631\u0627 \u0647\u0644 \u0645\u06CC\u200C\u062F\u0647\u062F \u06A9\u0647 \u062A\u063A\u06CC\u06CC\u0631 \u0645\u0633\u06CC\u0631 \u0628\u062F\u0647\u062F \u0648 \u0628\u0631\u0648\u062F \u0633\u0631\u0627\u063A \u062A\u0648\u0644\u06CC\u062F \u062A\u0648\u0641\u0648\u06CC \u0644\u0627\u06A9\u062A\u0648\u0641\u0631\u0645\u0627\u0646\u062A\u0647.", "m\u00EAme d\u00E9faut : \u00ABpousse-t-il\u00BB au lieu de \u00ABtente\u00BB, comme le FR"),
			("7.2.5", "issue_es", "Intenta convencer a su vecino productor de foie gras de reconvertirse en la producci\u00F3n de tofu lactofermentado.", "Empujar a su vecino productor de foie gras a reconvertirse", "m\u00EAme d\u00E9faut : \u00ABEmpuja\u00BB (pousse) au lieu de \u00ABIntenta\u00BB (tente), comme le FR"),
		};

		/// <summary>
		/// Formes anciennes ERADIQUEES du corpus entier (compte 0 exigé) — les 36 formes ont
		/// ete MESUREES a zero avant d'entrer dans cette table. Deux exclusions a dessein :
		/// «5g» (5.3.4) est trop generique, et le fragment zh de 5.2.5.issue («诡辩者必须说服她»)
		/// vit legitimement DEUX fois ailleurs dans le corpus — phrase-type d'autres cartes
		/// (« la convaincre de promouvoir le kale », « la convaincre que la polyandrie a des
		/// avantages ») ; c'est l'epingle pleine-cellule de 5.2.5.issue_zh qui couvre le defaut.
		/// Les mots simples «spaghetti» et ses ecritures sont egalement exclus : les CONTEXTES
		/// de 3.2.15 les gardent legitimement (la carte raconte l'incident).
		/// </summary>
		private static readonly (string Form, string Where)[] Eradicated =
		{
			("Flat Earth Society", "5.3.1.title \u2014 l'organisation au lieu du d\u00E9bat"),
			("The Spaghetti T-Shirt", "3.2.15.title \u2014 la cause au lieu de l'\u00E9tat"),
			("Gooal!", "7.3.4.title \u2014 la coquille fig\u00E9e"),
			("The inheritence", "7.1.6.title \u2014 la coquille"),
			("occult payment", "6.2.2.context \u2014 l'euph\u00E9misme calqu\u00E9"),
			("all-important birthday", "3.2.8.context \u2014 l'intensificateur"),
			("given favorite", "4.3.3.context \u2014 le parasite de calque"),
			("the latter is an impostor", "5.3.1.issue \u2014 le renvoi flou"),
			("to refuse to benefit from it", "5.3.2.issue \u2014 le renvoi flou"),
			("Our country had prepared.", "6.1.4.suggestion_en \u2014 la voix active revendiqu\u00E9e"),
			("It is absolutely necessary to rebuild", "6.1.1.suggestion_en \u2014 la reformulation sans m\u00E9canisme"),
			("She considers it an infidelity", "5.2.5.issue \u2014 la r\u00E9futation abstraite sans Rachel"),
			("Me too it makes me happy", "7.2.8.suggestion_en \u2014 le calque FR"),
			("He must convince his neighbour", "7.2.5.issue \u2014 la modalit\u00E9 obligation"),
			("\u0642\u0645\u064A\u0635 \u0627\u0644\u0633\u0628\u0627\u063A\u064A\u062A\u064A", "3.2.15.title_ar \u2014 la cause au lieu de l'\u00E9tat"),
			("\u062A\u06CC\u200C\u0634\u0631\u062A \u0627\u0633\u067E\u0627\u06AF\u062A\u06CC\u200C\u062E\u0648\u0631\u062F\u0647", "3.2.15.title_fa \u2014 idem"),
			("La camiseta de espaguetis", "3.2.15.title_es \u2014 idem"),
			("\u0424\u0443\u0442\u0431\u043E\u043B\u043A\u0430 \u0441\u043E \u0441\u043F\u0430\u0433\u0435\u0442\u0442\u0438", "3.2.15.title_ru \u2014 idem"),
			("A T-shirt com esparguete", "3.2.15.title_pt \u2014 idem"),
			("\u0633\u0627\u0644\u06AF\u0631\u062F \u0628\u0633\u06CC\u0627\u0631 \u0645\u0647\u0645 \u0634\u0631\u06CC\u06A9", "3.2.8.context_fa \u2014 l'intensificateur"),
			("\u0437\u0430\u0431\u044B\u043B \u0432\u0430\u0436\u043D\u0435\u0439\u0448\u0438\u0439 \u0434\u0435\u043D\u044C", "3.2.8.context_ru \u2014 l'intensificateur"),
			("anivers\u00E1rio important\u00EDssimo", "3.2.8.context_pt \u2014 l'intensificateur"),
			("\u0628\u0639\u0643\u0633 \u0630\u0644\u0643", "5.2.5.issue_ar \u2014 le contraire abstrait"),
			("\u0645\u0627\u062C\u0631\u0627 \u0627\u06CC\u0646\u200C\u0637\u0648\u0631 \u0646\u06CC\u0633\u062A", "5.2.5.issue_fa \u2014 idem"),
			("Convencerla de lo contrario.", "5.2.5.issue_es \u2014 idem"),
			("\u0628\u0631\u0641\u0636 \u0627\u0644\u0627\u0633\u062A\u0641\u0627\u062F\u0629 \u0645\u0646\u0647.", "5.3.2.issue_ar \u2014 le renvoi flou"),
			("\u0627\u0632 \u0627\u06CC\u0646 \u0627\u0645\u06A9\u0627\u0646", "5.3.2.issue_fa \u2014 idem"),
			("de rechazar beneficiarse de ella", "5.3.2.issue_es \u2014 idem"),
			("\u0625\u0639\u0627\u062F\u0629 \u0628\u0646\u0627\u0621 \u062B\u0642\u0629", "6.1.1.suggestion_ar \u2014 la reformulation sans m\u00E9canisme"),
			("\u0627\u0639\u062A\u0645\u0627\u062F \u0645\u0631\u062F\u0645 \u0628\u0647", "6.1.1.suggestion_fa \u2014 idem"),
			("Es absolutamente necesario reconstruir", "6.1.1.suggestion_es \u2014 idem"),
			("\u6211\u4EEC\u5FC5\u987B\u91CD\u5EFA", "6.1.1.suggestion_zh \u2014 idem"),
			("vai trabalhar um pouco", "7.2.8.suggestion_pt \u2014 le contresens recopi\u00E9"),
			("\u064A\u062F\u0641\u0639 \u062C\u0627\u0631\u0647", "7.2.5.issue_ar \u2014 la modalit\u00E9 pousser"),
			("\u0647\u0644 \u0645\u06CC\u200C\u062F\u0647\u062F", "7.2.5.issue_fa \u2014 idem"),
			("Empujar a su vecino", "7.2.5.issue_es \u2014 idem"),
		};

		/// <summary>
		/// Siblings deja REPARES A L'ORIGINE, epingles pour qu'un balayage futur ne les
		/// « corrige » pas : le zh de 5.3.2 dit deja «refuser le vaccin» et le zh de 7.2.8
		/// dit deja «trop presse» — ce sont les VALEURS DE REFERENCE que les corrections
		/// EN/pt/es/ar/fa ont rattrapees. La matrice de fidelite les flaguait a tort :
		/// la cellule est le fait (mesure 04/10, dossier axe imprime §5).
		/// </summary>
		private static readonly (string Path, string Column, string Current)[] SiblingObservations =
		{
			("5.3.2", "issue_zh", "\u4ED6\u5FC5\u987B\u8BF4\u670D\u81EA\u5DF1\u7684\u90BB\u5C45\u2014\u2014\u4E00\u4F4D\u9AD8\u98CE\u9669\u4E14\u4EAB\u6709\u4F18\u5148\u63A5\u79CD\u8D44\u683C\u7684\u4EBA\u2014\u2014\u62D2\u7EDD\u63A5\u79CD\u3002"),
			("7.2.8", "suggestion_zh", "\u6211\u4E5F\u5F88\u9AD8\u5174\u518D\u89C1\u5230\u4F60\uFF0C\u4F46\u4F60\u4E0D\u89C9\u5F97\u8FD9\u4E5F\u592A\u5FC3\u6025\u4E86\u5417\uFF1F"),
		};

		internal static List<string> Mismatches(
			IEnumerable<(string Path, string Column, string Expected, string Actual)> cells)
		{
			var offenders = new List<string>();
			foreach (var (path, column, expected, actual) in cells)
				if (!string.Equals(expected, actual, StringComparison.Ordinal))
					offenders.Add($"{path}.{column} : attendu «{Truncate(expected)}», lu «{Truncate(actual)}»");
			return offenders;
		}

		private static string Truncate(string s) => s.Length <= 60 ? s : s.Substring(0, 57) + "…";

		private static List<(string, string, string, string)> ReadPinned()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var read = new List<(string, string, string, string)>();
			foreach (var (path, column, expected, _, _) in Restored)
			{
				var v = csv.LoadColumn(column, "path", new[] { path });
				v.Should().HaveCount(1, $"la rangee {path} existe et est unique.");
				read.Add((path, column, expected, v[0]));
			}
			return read;
		}

		[Fact]
		public void Restored_Cells_Carry_The_Never_Printed_Corrections()
		{
			Mismatches(ReadPinned()).Should().BeEmpty(
				"#458 grain 3b : les 15 cellules EN jamais imprimees (Annexe C du dossier owner, " +
				"correction libre) et les 23 siblings defectueuses (regle du GO) sont ecrites, " +
				"valeur pleine, une raison par cellule.");
		}

		[Fact]
		public void Old_Forms_Are_Eradicated_From_The_Corpus()
		{
			var corpus = System.IO.File.ReadAllText(ScenariiCsv);
			var offenders = Eradicated
				.Where(e => corpus.Contains(e.Form, StringComparison.Ordinal))
				.Select(e => $"«{e.Form}» encore present ({e.Where})")
				.ToList();
			offenders.Should().BeEmpty(
				"les formes anciennes sont eradiquees du corpus entier — sauf les deux exclusions " +
				"a dessein documentees sur la table (le mot «5g» et le fragment zh de 5.2.5, " +
				"phrase-type d'autres cartes).");
		}

		[Fact]
		public void Restored_Table_Is_Not_Vacuous()
		{
			Restored.Should().HaveCount(38, "15 EN jamais imprimees + 23 siblings (regle du GO).");
			Restored.Select(r => (r.Path, r.Column)).Distinct().Should().HaveCount(38, "38 cellules distinctes.");
			Restored.Select(r => r.Path).Distinct().Should().HaveCount(14, "14 rangees.");
			Restored.Count(r => !r.Column.Contains('_') || r.Column == "suggestion_en").Should().Be(15, "15 cellules EN.");
			Restored.Count(r => r.Column != "suggestion_en" && r.Column.Contains('_')).Should().Be(23, "23 siblings.");
			Restored.Should().OnlyContain(r => r.Was.Length > 0 && r.Why.Length > 20, "chaque epingle porte son avant et sa raison.");
			Restored.Select(r => r.Why).Distinct().Should().HaveCount(38, "38 raisons distinctes, pas un copier-coller.");
			Eradicated.Should().HaveCount(36, "14 formes EN (le «5g» generique exclu) + 22 siblings (le fragment zh exclu).");
			Eradicated.Select(e => e.Form).Distinct().Should().HaveCount(36);
			SiblingObservations.Should().HaveCount(2, "les 2 temoins zh repares a l'origine.");
		}

		[Fact]
		public void Sibling_Observations_Stay_Pinned()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var offenders = new List<string>();
			foreach (var (path, column, current) in SiblingObservations)
			{
				var v = csv.LoadColumn(column, "path", new[] { path });
				v.Should().HaveCount(1);
				if (!string.Equals(current, v[0], StringComparison.Ordinal))
					offenders.Add($"{path}.{column} : ce temoin zh etait deja sain (repare a l'origine) — s'il a change, un balayage l'a « corrige » a tort.");
			}
			offenders.Should().BeEmpty(
				"les siblings deja saines ne bougent pas : la regle du GO corrige le meme defaut, pas le voisinage.");
		}

		[Fact]
		public void Detector_Fires_On_The_Pre_Correction_Value()
		{
			var reverted = new[]
			{
				("7.3.4", "title", Restored[12].Expected, "Gooal!"),
				("3.2.15", "title_ru", Restored[18].Expected, "\u0424\u0443\u0442\u0431\u043E\u043B\u043A\u0430 \u0441\u043E \u0441\u043F\u0430\u0433\u0435\u0442\u0442\u0438"),
				("7.1.6", "title", Restored[14].Expected, Restored[14].Expected),
			};
			var offenders = Mismatches(reverted);
			offenders.Should().HaveCount(2, "seules les cellules revertes doivent rougir : le temoin sain reste vert.");
			offenders.Should().Contain(s => s.Contains("7.3.4.title"));
			offenders.Should().Contain(s => s.Contains("3.2.15.title_ru"));
		}
	}
}
