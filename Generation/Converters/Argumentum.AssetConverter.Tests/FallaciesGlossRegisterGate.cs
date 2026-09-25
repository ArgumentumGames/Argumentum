using System;
using System.Collections.Generic;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Organ G2-C-W / G2-C-W2 (#1499, decisions owner Q-12 (1a/2a/3a) et Q-16 (c) du 24/09-25/09,
	/// arbitrages ai-01 [#458 c.5825475175](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5825475175),
	/// [#1499 c.5828321925](https://github.com/ArgumentumGames/Argumentum/issues/1499#issuecomment-5828321925)).
	///
	/// - 11 gloses didactiques retirees (88 cellules, G2-C-W) ;
	/// - PK 598 : exemple 2024 restaure x8 (source mesuree 3eb08fc6^, pre-composite #1265) ;
	/// - PK 1361 : registre poli fr/ru/pt/es/zh/fa ; ar sans distinction standard, en indifferencie ;
	/// - G2-C-W2 (Q-16 (c)) : 7 exceptions RESTAUREES a l'octet depuis 9f606c98^ (la cellule
	///   d'avant #1546, glose comprise) :
	///     PK 796 example_{en,ru,pt,es,ar,zh} et PK 848 example_zh.
	///   ⛔ PK 796 example_fr N'EST PAS restaure (Q-16 (c)) : la glose fr y reste coupee.
	///
	/// Les attendus sont INSCRITS ici : toute derive du CSV fait rouge AVANT regeneration,
	/// et le rouge NOMME la cellule - pas un diff muet.
	/// </summary>
	internal static class FallaciesGlossRegisterGate
	{
		public static readonly string[] Languages = { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

		/// <summary>PK des 11 cartes a glose (dossier G2-C section 1) - ordre du dossier.</summary>
		public static readonly string[] GlossPks =
		{
			"33", "322", "362", "432", "658", "796", "834", "848", "908", "1092", "1120",
		};

		/// <summary>G2-C-W2 (Q-16 (c)) : les 7 cellules dont la glose est RESTAUREE (pk:lang).</summary>
		public static readonly string[] RestoredExceptions =
		{
			"796:en", "796:ru", "796:pt", "796:es", "796:ar", "796:zh", "848:zh",
		};

		/// <summary>
		/// Texte attendu, par PK puis langue (gloses coupees SAUF les 7 exceptions G2-C-W2).
		/// </summary>
		public static readonly Dictionary<string, Dictionary<string, string>> GlossExpected = new()
		{
			{
				"33", new Dictionary<string, string>
				{
					{ "fr", "Je conviens que votre calcul est correct. Mais votre raisonnement est insignifiant : ce détail ne change rien au problème de fond." },
					{ "en", "I agree that your calculation is correct. But your reasoning is insignificant: this detail changes nothing about the underlying issue." },
					{ "ru", "Я согласен, что ваш расчёт верен. Но ваше рассуждение несущественно: эта деталь ничего не меняет в основной проблеме." },
					{ "pt", "Concordo que o seu cálculo está correto. Mas o seu raciocínio é insignificante: esse detalhe não muda em nada o problema de fundo." },
					{ "es", "Admito que su cálculo es correcto. Pero su razonamiento es insignificante: este detalle no cambia nada del problema de fondo." },
					{ "ar", "أُقرّ بأن حسابك صحيح. لكن استدلالك عديم الأهمية: هذه التفصيلة لا تغيّر شيئًا في جوهر المشكلة." },
					{ "fa", "می‌پذیرم که محاسبهٔ شما درست است. اما استدلالتان بی‌اهمیت است: این جزئیات هیچ تغییری در مسئلهٔ اصلی ایجاد نمی‌کند." },
					{ "zh", "我承认你的计算是正确的。但你的论证无关紧要：这个细节丝毫不影响根本问题。" },
				}
			},
			{
				"322", new Dictionary<string, string>
				{
					{ "fr", "Les collapsologues veulent nous ramener à l’âge de pierre et abolir toute industrie." },
					{ "en", "Collapsologists want to take us back to the Stone Age and abolish all industry." },
					{ "ru", "Коллапсологи хотят вернуть нас в каменный век и упразднить всю промышленность." },
					{ "pt", "Os colapsólogos querem nos levar de volta à Idade da Pedra e abolir toda a indústria." },
					{ "es", "Los colapsólogos quieren devolvernos a la Edad de Piedra y abolir toda la industria." },
					{ "ar", "يريد أنصار علم الانهيار إعادتنا إلى العصر الحجري وإلغاء الصناعة بأكملها." },
					{ "fa", "فروپاشی‌باوران می‌خواهند ما را به عصر حجر برگردانند و تمام صنایع را برچینند." },
					{ "zh", "崩溃论者想让我们倒退回石器时代，并废除一切工业。" },
				}
			},
			{
				"362", new Dictionary<string, string>
				{
					{ "fr", "Votre travail sur le projet a été impressionnant. En revanche, il y a eu quelques erreurs dans vos calculs. Mais, dans l’ensemble, vous avez montré un grand engagement." },
					{ "en", "Your work on the project was impressive. However, there were a few errors in your calculations. But overall, you showed great commitment." },
					{ "ru", "Ваша работа над проектом была впечатляющей. Однако в ваших расчётах было несколько ошибок. Но в целом вы проявили большую самоотдачу." },
					{ "pt", "Seu trabalho no projeto foi impressionante. No entanto, houve alguns erros nos seus cálculos. Mas, no geral, você demonstrou grande comprometimento." },
					{ "es", "Tu trabajo en el proyecto fue impresionante. Sin embargo, hubo algunos errores en tus cálculos. Pero, en general, mostraste un gran compromiso." },
					{ "ar", "كان عملك في المشروع مثيرًا للإعجاب. ومع ذلك، كانت هناك بعض الأخطاء في حساباتك. لكنك، بوجه عام، أظهرت التزامًا كبيرًا." },
					{ "fa", "کار شما روی پروژه واقعاً چشمگیر بود. با این حال، چند اشتباه در محاسبات شما وجود داشت. اما در مجموع، تعهد زیادی از خود نشان دادید." },
					{ "zh", "你在项目上的工作令人印象深刻。不过，你的计算中出现了一些错误。但总的来说，你表现出了极大的投入。" },
				}
			},
			{
				"432", new Dictionary<string, string>
				{
					{ "fr", "Puisque vous êtes favorables à la transition écologique, que ceux d’entre vous qui accepteront de prendre le train plutôt que l’avion pour leurs prochains déplacements lèvent la main." },
					{ "en", "Since you support the ecological transition, those of you who are willing to take the train rather than fly on your next trips, raise your hands." },
					{ "ru", "Поскольку вы поддерживаете экологический переход, пусть те из вас, кто согласится в своих следующих поездках путешествовать на поезде, а не на самолёте, поднимут руку." },
					{ "pt", "Já que vocês são favoráveis à transição ecológica, aqueles entre vocês que aceitarem viajar de trem em vez de avião em seus próximos deslocamentos levantem a mão." },
					{ "es", "Puesto que estáis a favor de la transición ecológica, que levanten la mano quienes, para sus próximos desplazamientos, acepten viajar en tren en lugar de en avión." },
					{ "ar", "بما أنكم تؤيدون التحول البيئي، فليرفع أيديهم من بينكم الذين سيوافقون على ركوب القطار بدلًا من الطائرة في تنقلاتهم المقبلة." },
					{ "fa", "از آنجا که شما طرفدار گذار زیست‌محیطی هستید، آن دسته از شما که می‌پذیرند در سفرهای آینده‌شان به‌جای هواپیما با قطار سفر کنند، دستشان را بالا ببرند." },
					{ "zh", "既然你们支持生态转型，那么你们当中愿意在接下来的出行中乘坐火车而非飞机的人请举手。" },
				}
			},
			{
				"658", new Dictionary<string, string>
				{
					{ "fr", "Cette affirmation est vraie. — Mais comment le savez-vous ? Je l’ai vérifiée. — Mais comment avez-vous vérifié cette vérification ? Et la vérification de cette vérification, comment l’avez-vous vérifiée ? …" },
					{ "en", "This statement is true. — But how do you know? I verified it. — But how did you verify that verification? And how did you verify the verification of that verification? …" },
					{ "ru", "Это утверждение истинно. — Но откуда вы это знаете? Я это проверил. — Но как вы проверили эту проверку? А проверку этой проверки — как вы проверили её? …" },
					{ "pt", "Esta afirmação é verdadeira. — Mas como você sabe? Eu a verifiquei. — Mas como você verificou essa verificação? E a verificação dessa verificação, como você a verificou? …" },
					{ "es", "Esta afirmación es verdadera. — Pero ¿cómo lo sabes? La he verificado. — Pero ¿cómo verificaste esa verificación? Y la verificación de esa verificación, ¿cómo la verificaste?…" },
					{ "ar", "هذا الادعاء صحيح. — ولكن كيف تعرف ذلك؟ لقد تحققت منه. — ولكن كيف تحققت من عملية التحقق هذه؟ وكيف تحققت من التحقق من هذا التحقق؟ …" },
					{ "fa", "این ادعا درست است. — اما از کجا می‌دانید؟ آن را بررسی کرده‌ام. — اما چگونه بررسی کرده‌اید که این بررسی درست بوده است؟ و بررسیِ این بررسی را چگونه بررسی کرده‌اید؟ …" },
					{ "zh", "这个断言是真的。——但你怎么知道？我已经验证过了。——但你又是怎么验证这次验证的？而对这次验证的验证，你又是怎么验证的？……" },
				}
			},
			{
				"796", new Dictionary<string, string>
				{
					{ "fr", "Tous les avocats défendent des clients au tribunal. Ce fruit est un avocat. Donc ce fruit défend des clients au tribunal." },
					{ "en", "All lawyers defend clients in court. This fruit is an avocado. Therefore, this fruit defends clients in court. — “Lawyer” and “avocado” are the same word in French, but its meaning changes: it refers to the legal profession in the first premise and to the fruit in the second. The reasoning therefore actually contains four terms instead of three." },
					{ "ru", "Все адвокаты защищают клиентов в суде. Этот плод — авокадо. Следовательно, этот плод защищает клиентов в суде. — Французское слово «avocat» меняет значение: в первой посылке оно обозначает юридическую профессию, а во второй — плод авокадо. Таким образом, в действительности рассуждение содержит четыре термина вместо трёх." },
					{ "pt", "Todos os advogados defendem clientes no tribunal. Esta fruta é um abacate. Portanto, esta fruta defende clientes no tribunal. — Em francês, a palavra « avocat » muda de sentido: refere-se à profissão jurídica na primeira premissa e à fruta na segunda. Portanto, o raciocínio contém, na verdade, quatro termos em vez de três." },
					{ "es", "Todos los abogados defienden a clientes ante los tribunales. Esta fruta es un aguacate. Por lo tanto, esta fruta defiende a clientes ante los tribunales. — «Abogado» y «aguacate» corresponden a la misma palabra en francés, «avocat», que cambia de sentido: profesión jurídica en la primera premisa y fruta en la segunda. Por lo tanto, el razonamiento contiene en realidad cuatro términos en lugar de tres." },
					{ "ar", "كل من هو «أفوكات» يدافع عن موكّلين في المحكمة. هذه الثمرة «أفوكات». إذن هذه الثمرة تدافع عن موكّلين في المحكمة. — يتغيّر معنى «أفوكات»: فهو يعني صاحب مهنة قانونية في المقدمة الأولى، ويعني ثمرة في المقدمة الثانية. لذلك يحتوي الاستدلال في الواقع على أربعة مصطلحات بدلًا من ثلاثة." },
					{ "fa", "همهٔ «آوُکا»ها، یعنی وکلا، در دادگاه از موکلان دفاع می‌کنند. این میوه یک «آوُکا»، یعنی آووکادو، است. پس این میوه در دادگاه از موکلان دفاع می‌کند." },
					{ "zh", "所有阿沃卡都在法庭上为当事人辩护。这个水果是阿沃卡。因此，这个水果在法庭上为当事人辩护。——“阿沃卡”一词改变了含义：在第一个前提中指律师，在第二个前提中指牛油果。因此，这个推理实际上包含四个词项，而不是三个。" },
				}
			},
			{
				"834", new Dictionary<string, string>
				{
					{ "fr", "Gérer une classe, c’est comme piloter une machine : si une pièce ralentit le système, il suffit de la remplacer." },
					{ "en", "Managing a class is like operating a machine: if one part slows down the system, you simply replace it." },
					{ "ru", "Управлять классом — всё равно что управлять машиной: если какая-то деталь замедляет работу системы, достаточно её заменить." },
					{ "pt", "Gerenciar uma turma é como operar uma máquina: se uma peça deixa o sistema mais lento, basta substituí-la." },
					{ "es", "Gestionar una clase es como manejar una máquina: si una pieza ralentiza el sistema, basta con reemplazarla." },
					{ "ar", "إدارة صف دراسي تشبه تشغيل آلة: إذا أبطأت قطعةٌ ما النظام، يكفي استبدالها." },
					{ "fa", "مدیریت یک کلاس مثل هدایت یک ماشین است: اگر قطعه‌ای سرعت سیستم را کم کند، کافی است آن را عوض کنید." },
					{ "zh", "管理一个班级就像操控一台机器：如果某个零件拖慢了系统，只需把它换掉就行。" },
				}
			},
			{
				"848", new Dictionary<string, string>
				{
					{ "fr", "Les membres du comité qui ont validé ce dossier seront convoqués." },
					{ "en", "The committee members who approved this file will be summoned." },
					{ "ru", "Члены комитета которые утвердили это досье будут вызваны." },
					{ "pt", "Os membros do comitê que aprovaram este dossiê serão convocados." },
					{ "es", "Los miembros del comité que aprobaron este expediente serán convocados." },
					{ "ar", "سيُستدعى أعضاء اللجنة الذين صادقوا على هذا الملف." },
					{ "fa", "اعضای کمیته که این پرونده را تأیید کرده‌اند احضار خواهند شد." },
					{ "zh", "批准了这份材料的委员会成员将被传唤。——限制性：只有批准了这份材料的成员才会被传唤。委员会成员，他们批准了这份材料，将被传唤。——解释性：所有成员都会被传唤，同时补充说明他们批准了这份材料。" },
				}
			},
			{
				"908", new Dictionary<string, string>
				{
					{ "fr", "On sait tous que les pyramides ont été construites par des esclaves." },
					{ "en", "We all know that the pyramids were built by slaves." },
					{ "ru", "Все знают, что пирамиды были построены рабами." },
					{ "pt", "Todos sabemos que as pirâmides foram construídas por escravos." },
					{ "es", "Todos sabemos que las pirámides fueron construidas por esclavos." },
					{ "ar", "نعلم جميعًا أن الأهرامات بناها العبيد." },
					{ "fa", "همه می‌دانیم که اهرام را بردگان ساخته‌اند." },
					{ "zh", "我们都知道，金字塔是奴隶建造的。" },
				}
			},
			{
				"1092", new Dictionary<string, string>
				{
					{ "fr", "Le rapport indique que la nouvelle politique a amélioré neuf indicateurs sur dix : moins d’accidents, moins de retards, moins de coûts et une meilleure satisfaction des usagers. Mais je la refuse, parce qu’un indicateur s’est légèrement dégradé." },
					{ "en", "The report indicates that the new policy improved nine out of ten indicators: fewer accidents, fewer delays, lower costs, and greater user satisfaction. But I reject it because one indicator declined slightly." },
					{ "ru", "В отчёте указано, что новая политика улучшила девять показателей из десяти: стало меньше аварий, задержек и расходов, а удовлетворённость пользователей повысилась. Но я отвергаю её, потому что один показатель немного ухудшился." },
					{ "pt", "O relatório indica que a nova política melhorou nove de dez indicadores: menos acidentes, menos atrasos, custos menores e maior satisfação dos usuários. Mas eu a rejeito porque um indicador piorou ligeiramente." },
					{ "es", "El informe indica que la nueva política ha mejorado nueve de cada diez indicadores: menos accidentes, menos retrasos, menos costes y una mayor satisfacción de los usuarios. Pero la rechazo porque un indicador ha empeorado ligeramente." },
					{ "ar", "يشير التقرير إلى أن السياسة الجديدة حسّنت تسعة مؤشرات من أصل عشرة: حوادث أقل، وتأخيرات أقل، وتكاليف أقل، ورضا أفضل لدى المستخدمين. لكنني أرفضها لأن مؤشرًا واحدًا تراجع تراجعًا طفيفًا." },
					{ "fa", "گزارش نشان می‌دهد که سیاست جدید نه شاخص از ده شاخص را بهبود بخشیده است: حوادث کمتر، تأخیرهای کمتر، هزینه‌های کمتر و رضایت بیشتر کاربران. اما من آن را رد می‌کنم، چون یک شاخص اندکی بدتر شده است." },
					{ "zh", "报告显示，新政策改善了十项指标中的九项：事故更少、延误更少、成本更低，用户满意度也更高。但我拒绝这项政策，因为有一项指标略有恶化。" },
				}
			},
			{
				"1120", new Dictionary<string, string>
				{
					{ "fr", "Soit j’ai la moyenne, soit je suis complètement nul." },
					{ "en", "Either I get a passing grade, or I’m completely hopeless." },
					{ "ru", "Либо у меня средний балл, либо я полный ноль." },
					{ "pt", "Ou eu tiro a média, ou sou completamente incapaz." },
					{ "es", "O bien saco el aprobado, o soy un completo inútil." },
					{ "ar", "إمّا أن أحصل على المعدّل، وإمّا أن أكون عديم الكفاءة تمامًا." },
					{ "fa", "یا معدّلم را می‌گیرم، یا کاملاً بی‌کفایتم." },
					{ "zh", "要么我达到平均分，要么我就完全是个差生。" },
				}
			},
		};

		/// <summary>PK 598 : l'exemple 2024 restaure x8 (source 3eb08fc6^).</summary>
		public static readonly Dictionary<string, string> Pk598Expected = new()
		{
			{ "fr", "Avez-vous vu l’augmentation des cas de fraude chez les nouveaux employés ? La nouvelle génération n’est pas fiable." },
			{ "en", "Have you seen the increase in fraud cases among new employees? The new generation is unreliable." },
			{ "ru", "Вы видели увеличение случаев мошенничества среди новых сотрудников? Новое поколение ненадежно." },
			{ "pt", "Você viu o aumento dos casos de fraude entre os novos funcionários? A nova geração não é confiável." },
			{ "es", "¿Has visto el aumento de casos de fraude entre los nuevos empleados? La nueva generación no es fiable." },
			{ "ar", "هل رأيت زيادة حالات الاحتيال بين الموظفين الجدد؟ الجيل الجديد غير موثوق به." },
			{ "fa", "آیا موارد افزایش تقلب بین کارمندان جدید را دیده‌اید؟ نسل جدید قابل اعتماد نیست." },
			{ "zh", "你看见新员工中欺诈案件的增加了吗？新一代人不可靠。" },
		};

		/// <summary>
		/// PK 1361 : registre poli (Q-12 3a, PK 1361 seul). ar = laisse tel quel
		/// (l'arabe standard ne distingue pas tu/vous), en = indifferencie. Les deux sont
		/// epingles POUR constater le caractere delimite de l'absence de changement.
		/// </summary>
		public static readonly Dictionary<string, string> Pk1361Expected = new()
		{
			{ "fr", "À vous entendre, tout achat est immoral. Pourtant je vous ai vu faire les soldes l’autre jour, et ma morale le tolère." },
			{ "en", "To hear you tell it, every purchase is immoral. Yet I saw you shopping the sales the other day, and my morality allows it." },
			{ "ru", "Если вас послушать, всякая покупка аморальна. Однако на днях я видел, как вы ходили на распродажи, и моя мораль это допускает." },
			{ "pt", "A ouvi-lo, toda compra é imoral. No entanto, vi-o a aproveitar os saldos outro dia, e a minha moral tolera isso." },
			{ "es", "Oyéndole, toda compra es inmoral. Sin embargo, le vi ir de rebajas el otro día, y mi moral lo tolera." },
			{ "ar", "إذا استمعتُ إليك، فكل شراء غير أخلاقي. ومع ذلك فقد رأيتك تتسوق في التخفيضات قبل أيام، وأخلاقيتي تقبل ذلك." },
			{ "fa", "از حرف‌هایتان که برمی‌آید، هر خریدی غیراخلاقی است. با این حال، چند روز پیش دیدم که در حراج‌ها خرید می‌کردید، و اخلاق من این را تحمل می‌کند." },
			{ "zh", "照您这么说，任何购买都是不道德的。可我前几天还看见您去抢购打折商品，而我的道德观是容许这一点的。" },
		};
}
}
