using System.Diagnostics;
using System.Text.Json;
using java.util;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Planning;
using Microsoft.SemanticKernel.SkillDefinition;
using net.sf.tweety.agents;
using net.sf.tweety.arg.aspic.parser;
using net.sf.tweety.arg.aspic.reasoner;
using net.sf.tweety.arg.aspic.ruleformulagenerator;
using net.sf.tweety.arg.aspic.syntax;
using net.sf.tweety.arg.deductive.accumulator;
using net.sf.tweety.arg.deductive.categorizer;
using net.sf.tweety.arg.deductive.reasoner;
using net.sf.tweety.arg.deductive.syntax;
using net.sf.tweety.arg.dung.reasoner;
using net.sf.tweety.arg.dung.semantics;
using net.sf.tweety.arg.dung.syntax;
using net.sf.tweety.beliefdynamics;
using net.sf.tweety.beliefdynamics.kernels;
using net.sf.tweety.beliefdynamics.mas;
using net.sf.tweety.beliefdynamics.operators;
using net.sf.tweety.commons;
using net.sf.tweety.graphs.orders;
using net.sf.tweety.logics.commons.syntax;
using net.sf.tweety.logics.dl.parser;
using net.sf.tweety.logics.dl.syntax;
using net.sf.tweety.logics.fol.parser;
using net.sf.tweety.logics.fol.reasoner;
using net.sf.tweety.logics.fol.syntax;
using net.sf.tweety.logics.ml.parser;
using net.sf.tweety.logics.ml.reasoner;
using net.sf.tweety.logics.ml.syntax;
using net.sf.tweety.logics.pl.parser;
using net.sf.tweety.logics.pl.reasoner;
using net.sf.tweety.logics.pl.sat;
using net.sf.tweety.logics.pl.semantics;
using net.sf.tweety.logics.pl.syntax;
using Newtonsoft.Json;
using OpenAI.GPT3.ObjectModels;
using TweetyIKVM;
using JsonSerializer = System.Text.Json.JsonSerializer;

public class ExemplesTweety
{

	public static void TestKernel()
	{
		var sw = Stopwatch.StartNew();
		var openAIKeyPath = @"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\OpenAI-Key.txt";
		var urlOobaboogaHost = @"jesse.freeboxos.fr";
		var urlOobaboogaBlockingPort = "32768";
		var urlOobaboogaStramingPort = "32769";

		var openAIKey = File.ReadAllText(openAIKeyPath);

		var kernel = Kernel.Builder
			.WithLogger(ConsoleLogger.Log)
			//.WithOpenAITextCompletionService(Models.Model.TextDavinciV3.EnumToString(), openAIKey)
			//.WithAIService<ITextCompletion>("myService1", new OogaboogaTextCompletion(urlOobaboogaHost, urlOobaboogaBlockingPort, urlOobaboogaStramingPort))
			//.WithOpenAITextEmbeddingGenerationService(Models.Model.TextEmbeddingAdaV2.EnumToString(), openAIKey)
			.WithMemoryStorage(new VolatileMemoryStore())
			.Build();

		//kernel.ImportSkill(new MathSkill(), "math");
		//kernel.ImportSkill(new HttpSkill(), "http");
		//kernel.ImportSkill(new FileIOSkill(), "fileIO");
		//kernel.ImportSkill(new TextSkill(), "text");
		//kernel.ImportSkill(new TimeSkill(), "time");
		//kernel.ImportSkill(new WaitSkill(), "wait");
		//var parentDirectory = @"E:\Dev\AI\Libs\Argumentum\Generation\Converters\Argumentum.AssetConverter\DatasetUpdater\Resources\Skills\Core";
		//kernel.ImportSemanticSkillFromDirectory(parentDirectory);

		var tweetyNativeSkill= kernel.ImportSkill(new TweetySkill(), "tweety");
		var parentDirectory = @"E:\Dev\AI\Libs\Argumentum\Generation\Converters\Argumentum.AssetConverter\DatasetUpdater\Resources\Skills\Custom";
		var tweetySemanticSkill = kernel.ImportSemanticSkillFromDirectory(parentDirectory, "TweetySkill");

		var planner = new SequentialPlanner(kernel);

		// We're going to ask the planner to find a function to achieve this goal.

		//var goal = "The following is the transcript or description of a debate. Return a logic and argumentation analysis of the debate. The plan should like that: First, use the Tweety skill TextToLogicType to select the type of formal logic relevant to the situation. Then use TextToBeliefset to create a parsable belief set representing the logic statements. Then use BeliefStateToQuery to find a potential query to run using Tweety. Then use native method QueryLogicReasoner to run the tweety engine and get the result. Finally use semantic function LogicAnalysisToText to inject the text, belief set, query and result into an intepreter prompt that will return a human readable intepretation of the analysis.\n\n";


		//var goal = "The following is the transcript or description of a debate. Return an analysis of the debate. The plan should like that: First, use TextToLogicType then TextToBeliefset then BeliefStateToQuery then QueryLogicReasoner, finally use LogicAnalysisToText to inject the text, belief set, query and result into an intepreter prompt that will return a human readable intepretation of the analysis.\n\n";

		var goal = "The following is the transcript or description of a debate. Return an analysis of the debate. \n\n";


		//var debateText = "Debate Context: Alice claims that if it's sunny, then Bob will go to the park. Bob says that it's indeed sunny, but he won't go to the park because he has work to do.";

		//var debateText ="Debate Context: Federalists claim that if the Constitution is adopted, then there will be a stronger federal government. They further claim that a stronger federal government will better protect the rights of the people and ensure stability. Anti-Federalists, on the other hand, claim that if the Constitution is adopted, then there will be a stronger federal government, but they believe that a stronger federal government will lead to an abuse of power and the loss of individual rights.";


		//var debateText = "Debate Context: Federalists claim that if the Constitution is adopted, then there will be a stronger federal government. They further claim that a stronger federal government will better protect the rights of the people and ensure stability. Anti-Federalists, on the other hand, claim that if the Constitution is adopted, then there will be a stronger federal government, but they fear that a stronger federal government may lead to an abuse of power and the loss of individual rights. They believe that without checks and balances, the risk of tyranny is real. Yet, the Federalists assert that adoption of the Constitution includes a commitment to checks and balances, aiming to prevent such abuse of power.";

		//var debateText = "Debate Context: In the nascent era of the United States, two significant groups, the Federalists and the Anti-Federalists, grappled with disagreements on the structure of the government.\r\n\r\nThe Federalists staunchly advocated for a robust central government. They postulated that the flourishing nation could only continue to grow and stabilize by upholding a range of fundamental conditions. Among these were: nurturing democratic traditions, ensuring the rule of law, maintaining an independent judiciary, promoting an enlightened citizenry, encouraging responsible citizenship, fostering a vibrant civil society, and preserving faith in the electoral process.\r\n\r\nWith significant emphasis, the Federalists contended that a strong central government was pivotal in coordinating comprehensive responses to national crises, including economic recessions, foreign invasions, and pandemics. They held the conviction that a unified action against these adversities would bolster national resilience and instill a sense of security within the citizenry.\r\n\r\nMoreover, they reasoned that an empowered central government, contrary to the fears of the Anti-Federalists, would be less likely to spiral into tyranny. A strong government, armed with the ability to enforce its laws uniformly across the states, they argued, would ensure that no single state could domineer over others or impose its will on the minority. Such a government would also effectively counterbalance the concentration of wealth and power.\r\n\r\nSimultaneously, the Federalists proposed that the Constitution, the blueprint of this central government, was designed to include checks and balances to prevent the misuse of power. They asserted that the Constitution was not merely an instrument for centralizing power, but was built to address the Anti-Federalist's fears about potential overreach by the central government.\r\n\r\nThe Anti-Federalists, despite recognizing the importance of the aforementioned conditions, remained apprehensive of a potent federal government. They voiced concerns that even with these safeguards in place, such a government might overstep its boundaries and infringe upon individual freedoms.\r\n\r\nHowever, both groups agreed on one crucial point: the rule of law and an independent judiciary were indispensable in checking any misuse of power. They jointly reasoned that a misuse of power would signify a compromised judiciary or a flouting of the rule of law. Conversely, they concurred that if these two conditions were steadfastly preserved, they would serve as robust bulwarks against power misuse.";

		var debateText = "Debate Context: À l'aube de l'ère des États-Unis, deux groupes importants, les Fédéralistes et les Anti-Fédéralistes, se sont affrontés sur la structure du gouvernement.\r\n\r\nLes Fédéralistes ont fermement défendu l'idée d'un gouvernement central fort. Ils ont postulé que la nation en plein essor ne pourrait continuer à croître et à se stabiliser qu'en respectant un ensemble de conditions fondamentales. Parmi celles-ci figuraient la préservation des traditions démocratiques, l'application de l'état de droit, le maintien d'un pouvoir judiciaire indépendant, la promotion d'une citoyenneté éclairée, l'encouragement à une citoyenneté responsable, le soutien à une société civile dynamique et la préservation de la confiance dans le processus électoral.\r\n\r\nDe plus, les Fédéralistes ont soutenu que la Constitution, en tant que document fondateur, était conçue pour inclure des mécanismes de contrôle visant à prévenir les abus de pouvoir. Ils ont affirmé que la Constitution n'était pas simplement un instrument de centralisation du pouvoir, mais qu'elle était spécifiquement élaborée pour répondre aux préoccupations des Anti-Fédéralistes concernant un éventuel dépassement des limites par le gouvernement central.\r\n\r\nLes Anti-Fédéralistes, tout en reconnaissant l'importance des conditions énoncées, ont exprimé des préoccupations quant à un gouvernement central fort. Ils ont souligné que même avec ces mesures de sauvegarde en place, il y avait toujours un risque que le gouvernement central empiète sur les libertés individuelles et abuse de son pouvoir.\r\n\r\nCependant, les Fédéralistes et les Anti-Fédéralistes étaient d'accord sur un point crucial : le respect de l'état de droit et d'un pouvoir judiciaire indépendant était essentiel pour prévenir tout abus de pouvoir. Ils étaient d'accord sur le fait qu'un abus de pouvoir signifierait une compromission du pouvoir judiciaire ou une violation de l'état de droit. Inversement, ils étaient d'accord que si ces deux conditions étaient fermement respectées, elles serviraient de solides remparts contre tout abus de pouvoir.";

		goal = goal + debateText;

		// The planner returns a plan, consisting of a single function
		// to execute and achieve the goal requested.
		//var plan = planner.CreatePlanAsync(goal).GetAwaiter().GetResult();

		var plan = GetPLogicPlan(goal, tweetySemanticSkill, debateText, tweetyNativeSkill);


		//Console.WriteLine("Original plan:\n");
		//Console.WriteLine(JsonSerializer.Serialize(plan, new JsonSerializerOptions { WriteIndented = true }));

		// Execute the full plan (which is a single function)
		SKContext result = plan.InvokeAsync().GetAwaiter().GetResult();

		//var startPrompt =
		//	"Based on the provided text and logic applied, the query {{$query}} has been accepted by the belief set. This suggests that within the constraints and rules of the belief set, the situation described by the query can indeed occur. Please note that the interpretation needs to be in line with the original text and logic applied, but in the context of the natural language text, this could mean \n";

		// Show the result, which should match the given goal
		//Console.WriteLine($"Result: {startPrompt}{result.Result}\n");
		Console.WriteLine($"Result: {result.Result}\n");
		//Console.WriteLine($"Context: {JsonConvert.SerializeObject(result.Variables)}\n");
		foreach (var contextVariable in result.Variables)
		{
			if (contextVariable.Key.ToUpperInvariant()!="INPUT")
			{
				Console.WriteLine($"{contextVariable.Key}: {contextVariable.Value}\n");
			}
		}
	
		Console.WriteLine($"Elapsed: {sw.Elapsed}");
	}

	private static Plan GetPLogicPlan(string goal, IDictionary<string, ISKFunction> tweetySemanticSkill, string debateText,
		IDictionary<string, ISKFunction> tweetyNativeSkill)
	{
		var plan = new Plan(goal);
		var planStep = new Plan(tweetySemanticSkill["TextToBeliefsetPl"]);
		planStep.Parameters.Set("input",
			debateText);
		planStep.Outputs.Add("belief_set");
		plan.AddSteps(planStep);

		planStep = new Plan(tweetySemanticSkill["BeliefStateToQueriesPl"]);
		planStep.Parameters.Set("input",
			debateText);
		planStep.Parameters.Set("belief_set", "$belief_set");
		planStep.Outputs.Add("queries");
		plan.AddSteps(planStep);

		planStep = new Plan(tweetyNativeSkill[nameof(TweetySkill.PropositionalQueryLogicReasoner)]);
		planStep.Parameters.Set("input", "$belief_set");
		planStep.Parameters.Set("belief_set", "$belief_set");
		planStep.Parameters.Set("queries", "queries");
		planStep.Outputs.Add("tweety_result");
		plan.AddSteps(planStep);

		planStep = new Plan(tweetySemanticSkill["LogicAnalysisToText"]);
		planStep.Parameters.Set("input",
			debateText);
		planStep.Parameters.Set("logicType", TweetySkill.LogicType.Propositional.ToString());
		planStep.Parameters.Set("belief_set", "$belief_set");
		planStep.Parameters.Set("queries", "queries");
		planStep.Parameters.Set("tweety_result", "$tweety_result");
		planStep.Outputs.Add("human interpretation");
		plan.AddSteps(planStep);
		return plan;
	}




	private static Plan GetMultiLogicPlan(string goal, IDictionary<string, ISKFunction> tweetySemanticSkill, string debateText,
		IDictionary<string, ISKFunction> tweetyNativeSkill)
	{
		var plan = new Plan(goal);
		var planStep = new Plan(tweetySemanticSkill["TextToLogicType"]);
		planStep.Parameters.Set("input",
			debateText);
		//planStep.Parameters.Set("chapterCount", "3");
		planStep.Outputs.Add("logicType");
		plan.AddSteps(planStep);

		planStep = new Plan(tweetySemanticSkill["TextToBeliefset"]);
		planStep.Parameters.Set("input",
			debateText);
		planStep.Parameters.Set("logicType", "$logicType");
		planStep.Outputs.Add("belief_set");
		plan.AddSteps(planStep);

		planStep = new Plan(tweetySemanticSkill["BeliefStateToQuery"]);
		planStep.Parameters.Set("input",
			debateText);
		planStep.Parameters.Set("logicType", "$logicType");
		planStep.Parameters.Set("belief_set", "$belief_set");
		planStep.Outputs.Add("query");
		plan.AddSteps(planStep);

		planStep = new Plan(tweetyNativeSkill[nameof(TweetySkill.QueryLogicReasoner)]);
		planStep.Parameters.Set("input",
			"$belief_set");
		planStep.Parameters.Set("logicType", "$logicType");
		planStep.Parameters.Set("belief_set", "$belief_set");
		planStep.Parameters.Set("query", "$query");
		planStep.Outputs.Add("tweety_result");
		plan.AddSteps(planStep);

		planStep = new Plan(tweetySemanticSkill["LogicAnalysisToText"]);
		planStep.Parameters.Set("input",
			debateText);
		planStep.Parameters.Set("logicType", "$logicType");
		planStep.Parameters.Set("belief_set", "$belief_set");
		planStep.Parameters.Set("query", "$query");
		planStep.Parameters.Set("tweety_result", "$tweety_result");
		planStep.Outputs.Add("human interpretation");
		plan.AddSteps(planStep);
		return plan;
	}


	public static void TestSkill()
	{
		string plFileContent = File.ReadAllText(@"./Resources/examplebeliefbase.proplogic");
		var ctx = new SKContext(new ContextVariables());
		ctx.Variables.Update(plFileContent);
		ctx.Variables.Set("logicType", nameof(TweetySkill.LogicType.Propositional));
		ctx.Variables.Set("queries", "!a && !b\n!c && !d");
		var result = TweetySkill.QueryLogicReasoner(plFileContent, ctx);
		Console.WriteLine(result);

	}




	/// <summary>
	/// Exemple de création d'un monde possible, d'analyse de formules propositionnelles
	/// et de vérification si un monde possible satisfait une formule propositionnelle.
	/// </summary>
	public static void ExamplePL()
	{
		Console.WriteLine($"\n*** {nameof(ExamplePL)}\n");
		// Analyse la formule propositionnelle à partir d'une chaîne de caractères
		PropositionalFormula f = (PropositionalFormula)new PlParser().parseFormula("!a && b");
		PropositionalFormula g = (PropositionalFormula)new PlParser().parseFormula("b || c");
		// Combine les deux formules en une seule formule en utilisant l'opérateur logique 'and'
		PropositionalFormula h = f.combineWithAnd(g).toDnf();
		// Crée un monde possible en spécifiant les propositions vraies
		PossibleWorld w = new PossibleWorld();
		w.add(new Proposition("a"));
		w.add(new Proposition("b"));
		// Vérifie si le monde possible satisfait la formule propositionnelle
		Console.WriteLine(w.satisfies(f));
	}


	/// <summary>
	/// Configure les solveurs pour la logique propositionnelle, la logique des prédicats et la logique modale.
	/// </summary>
	public static void ConfigureReasoners()
	{
		// Configure le solveur SAT par défaut pour la logique propositionnelle
		SatSolver.setDefaultSolver(new Sat4jSolver());
		// Configure le raisonneur EFOL (First-Order Logic) par défaut pour la logique des prédicats
		// en spécifiant le chemin d'accès à l'exécutable EProver
		string pathToEProver = @"./ext/EProver/eprover.exe";
		FolReasoner.setDefaultReasoner(new EFOLReasoner(pathToEProver));
		// Configure le raisonneur modal par défaut pour la logique modale
		// en spécifiant le chemin d'accès à l'exécutable SPASS
		var pathToSpass = "./ext/SPASS/SPASS.exe";
		AbstractModalReasoner.setDefaultReasoner(new SPASSModalReasoner(pathToSpass));
	}

	/// <summary>
	/// Exemple d'utilisation de la logique des prédicats du premier ordre.
	/// </summary>
	public static void FirstOrderLogicExample()
	{
		Console.WriteLine($"\n*** {nameof(FirstOrderLogicExample)}\n");
		// Crée un analyseur syntaxique de la logique des prédicats
		FolParser folParser = new FolParser();
		// Crée un ensemble de croyances de la logique des prédicats
		FolBeliefSet fbs = new FolBeliefSet();
		// Crée une signature pour les symboles de la logique des prédicats
		FolSignature signature = new FolSignature();
		// Crée un tri pour les symboles "Animal"
		Sort sort = new Sort("Animal");
		signature.add((object)sort);
		// Crée une liste de tri pour les arguments des prédicats "A"
		ArrayList arrayList = new ArrayList();
		arrayList.add((object)sort);
		arrayList.add((object)sort);
		// Ajoute le prédicat "A" et les constantes "b" et "c" à la signature
		signature.add((object)new Predicate("A", arrayList));
		signature.add((object)new Constant("b", sort));
		signature.add((object)new Constant("c", sort));
		// Configure l'analyseur syntaxique avec la signature
		folParser.setSignature(signature);
		// Ajoute deux formules propositionnelles à l'ensemble de croyances
		fbs.add(folParser.parseFormula("forall X: (forall Y: ((!A(X,Y) || A(Y,X)) && (!A(Y,X) || A(X,Y))))"));
		fbs.add(folParser.parseFormula("A(b,c)"));
		// Vérifie si la formule propositionnelle est vraie
		Console.WriteLine(FolReasoner.getDefaultReasoner().query(fbs, (FolFormula)folParser.parseFormula("A(c,b)")));
	}

	/// <summary>
	/// Exemple d'utilisation de la logique modale.
	/// </summary>
	public static void ModalLogicExample()
	{
		Console.WriteLine($"\n*** {nameof(ModalLogicExample)}\n");
		// Crée un ensemble de croyances de la logique modale
		ModalBeliefSet bs = new ModalBeliefSet();
		// Crée un analyseur syntaxique de la logique modale
		ModalParser parser = new ModalParser();
		// Crée une signature pour les symboles de la logique des prédicats
		FolSignature sig = new FolSignature();
		// Ajoute les prédicats "p", "q" et "r" à la signature
		sig.add(new Predicate("p", 0));
		sig.add(new Predicate("q", 0));
		sig.add(new Predicate("r", 0));
		// Configure l'analyseur syntaxique avec la signature
		parser.setSignature(sig);
		// Ajoute cinq formules propositionnelles à l'ensemble de croyances
		bs.add((RelationalFormula)parser.parseFormula("!(<>(p))"));
		bs.add((RelationalFormula)parser.parseFormula("p || r"));
		bs.add((RelationalFormula)parser.parseFormula("!r || [](q && r)"));
		bs.add((RelationalFormula)parser.parseFormula("[](r && <>(p || q))"));
		bs.add((RelationalFormula)parser.parseFormula("!p && !q"));
		// Affiche l'ensemble de croyances
		Console.WriteLine("Base de connaissances Modale: " + bs);
		// Récupère le raisonneur modal par défaut pour la logique modale
		AbstractModalReasoner reasoner = AbstractModalReasoner.getDefaultReasoner();
		// Vérifie si les formules propositionnelles sont vraies
		Console.WriteLine("[](!p) " + reasoner.query(bs, (FolFormula)parser.parseFormula("[](!p)")));
		Console.WriteLine("<>(q || r) " + reasoner.query(bs, (FolFormula)parser.parseFormula("<>(q || r)")));
		Console.WriteLine("p " + reasoner.query(bs, (FolFormula)parser.parseFormula("p")));
		Console.WriteLine("r " + reasoner.query(bs, (FolFormula)parser.parseFormula("r")));
		Console.WriteLine("[](r) " + reasoner.query(bs, (FolFormula)parser.parseFormula("[](r)")));
		Console.WriteLine("[](q) " + reasoner.query(bs, (FolFormula)parser.parseFormula("[](q)")));
	}


	/// <summary>
	/// Exemple d'utilisation de la logique de description.
	/// </summary>
	public static void DescriptionLogicExample()
	{
		Console.WriteLine($"\n*** {nameof(DescriptionLogicExample)}\n");
		// Crée une signature pour les concepts, rôles et individus de la logique de description
		AtomicConcept humain = new AtomicConcept("Humain");
		AtomicConcept masculin = new AtomicConcept("Masculin");
		AtomicConcept feminin = new AtomicConcept("Féminin");
		AtomicConcept maison = new AtomicConcept("Maison");
		AtomicConcept pere = new AtomicConcept("Père");
		AtomicRole pereDe = new AtomicRole("pèreDe");
		Individual bob = new Individual("Bob");
		Individual alice = new Individual("Alice");
		// Crée des axiomes terminologiques pour la logique de description
		EquivalenceAxiom femmeHumain = new EquivalenceAxiom(feminin, humain);
		EquivalenceAxiom hommeHumain = new EquivalenceAxiom(masculin, humain);
		EquivalenceAxiom pereEq = new EquivalenceAxiom(pere, new Union(masculin, pereDe)); // TODO remplacer l'union
		EquivalenceAxiom maisonPasHumain = new EquivalenceAxiom(maison, new Complement(humain));
		// Crée des axiomes assertoriaux pour la logique de description
		ConceptAssertion aliceHumain = new ConceptAssertion(alice, humain);
		ConceptAssertion bobHumain = new ConceptAssertion(bob, humain);
		ConceptAssertion aliceFeminin = new ConceptAssertion(alice, feminin);
		ConceptAssertion bobMasculin = new ConceptAssertion(bob, masculin);
		RoleAssertion bobPereDeAlice = new RoleAssertion(bob, alice, pereDe);
		// Crée une base de connaissances pour la logique de description et ajoute les axiomes à la base de connaissances
		DlBeliefSet dbs = new DlBeliefSet();
		dbs.add(femmeHumain);
		dbs.add(hommeHumain);
		dbs.add(pereEq);
		dbs.add(maisonPasHumain);
		dbs.add(aliceHumain);
		dbs.add(bobHumain);
		dbs.add(aliceFeminin);
		dbs.add(bobMasculin);
		dbs.add(bobPereDeAlice);
// Affiche la base de connaissances
		Console.WriteLine(dbs);
		Console.WriteLine("Only the ABox: " + dbs.getABox());
		Console.WriteLine("Only the TBox: " + dbs.getTBox());
// Parse la base de connaissances à partir d'un fichier
		DlParser parser = new DlParser();
		DlBeliefSet parseddbs =
			(DlBeliefSet)new DlParser().parseBeliefBaseFromFile("./Resources/examplebeliefbase.dlogic");
		DlSignature parsedsig = (DlSignature)parseddbs.getSignature();
		Console.WriteLine("\nSignature analysée : " + parsedsig.getIndividuals() + "," + parsedsig.getConcepts() + "," +
		                  parsedsig.getRoles());
		Console.WriteLine("\nBase de connaissances analysée : ");
		foreach (DlAxiom dax in parseddbs)
			Console.WriteLine(dax);
	}

	/// <summary>
	/// Exemple d'utilisation des révision de croyances.
	/// </summary>
	public static void BeliefDynamicsExample()
	{
		Console.WriteLine($"\n*** {nameof(BeliefDynamicsExample)}\n");
		// Crée un analyseur syntaxique pour la logique propositionnelle
		PlParser parser = new PlParser();
		// Crée des agents
		ArrayList agents = new ArrayList();
		agents.add(new DummyAgent("A1"));
		agents.add(new DummyAgent("A2"));
		agents.add(new DummyAgent("A3"));
		// Définit un ordre de crédibilité : A3 < A2 < A1 (A1 est le plus crédible)
		Order ordreCredibilite = new Order(agents);
		ordreCredibilite.setOrderedBefore(agents.get(0), agents.get(1));
		ordreCredibilite.setOrderedBefore(agents.get(1), agents.get(2));
		// Crée une base de croyances pour la logique propositionnelle avec l'ordre de crédibilité défini
		CrMasBeliefSet baseCroyances = new CrMasBeliefSet(ordreCredibilite);
		baseCroyances.add(new InformationObject((PropositionalFormula)parser.parseFormula("!c"), (Agent)agents.get(1)));
		baseCroyances.add(new InformationObject((PropositionalFormula)parser.parseFormula("b"), (Agent)agents.get(2)));
		baseCroyances.add(new InformationObject((PropositionalFormula)parser.parseFormula("!b||!a"),
			(Agent)agents.get(2)));

		// some new information
		var newInformation = new HashSet();
		newInformation.add(new InformationObject((PropositionalFormula)parser.parseFormula("a"), (Agent)agents.get(2)));
		newInformation.add(new InformationObject((PropositionalFormula)parser.parseFormula("!a||c"),
			(Agent)agents.get(2)));

		Console.WriteLine(baseCroyances + " * " + newInformation);
		Console.WriteLine();

		// simple prioritized revision (without considering credibilities)
		CrMasRevisionWrapper rev = new CrMasRevisionWrapper(
			new LeviMultipleBaseRevisionOperator(
				new KernelContractionOperator(new RandomIncisionFunction(), new SimpleReasoner()),
				new DefaultMultipleBaseExpansionOperator()
			));
		Console.WriteLine("PRIO       :\t " + rev.revise(baseCroyances, newInformation));

		// simple non-prioritized revision (with credibilities)
		CrMasSimpleRevisionOperator rev2 = new CrMasSimpleRevisionOperator();
		Console.WriteLine("N-PRIO CRED:\t " + rev2.revise(baseCroyances, newInformation));

		// credibility-based argumentative revision
		CrMasArgumentativeRevisionOperator theRevision = new CrMasArgumentativeRevisionOperator();
		Console.WriteLine("ARG        :\t " + theRevision.revise(baseCroyances, newInformation));

	}


	/// <summary>
	/// Exemple d'utilisation de l'argumentation abstraite.
	/// </summary>
	public static void AbstractArgumentation()
	{
		// Crée une théorie de Dung, c'est à dire la théorie de base dont dérivent les autres
		DungTheory theory = new DungTheory();
		Argument a = new Argument("a");
		Argument b = new Argument("b");
		Argument c = new Argument("c");
		Argument d = new Argument("d");
		Argument e = new Argument("e");
		Argument f = new Argument("f");
		theory.add(a);
		theory.add(b);
		theory.add(c);
		theory.add(d);
		theory.add(e);
		theory.add(f);
		theory.add(new Attack(a, b));
		theory.add(new Attack(b, c));
		theory.add(new Attack(c, d));
		theory.add(new Attack(d, e));
		theory.add(new Attack(e, a));
		theory.add(new Attack(e, f));
		SimpleCF2Reasoner reasoner = new SimpleCF2Reasoner();
		// Calcule les extensions de la théorie de Dung en utilisant le raisonneur SimpleCF2
		foreach (Extension extension in reasoner.getModels(theory).toArray())
		{
			Console.WriteLine(extension);
		}
	}


	/// <summary>
	/// This example demonstrates how to use the ASPIC+ framework to create an argumentation theory.
	/// </summary>
	public static void ArgumentationAspic()
	{
		Console.WriteLine($"\n*** {nameof(ArgumentationAspic)}\n");
		// Création de propositions pour les règles
		Proposition a = new Proposition("a");
		Proposition b = new Proposition("b");
		Proposition c = new Proposition("c");
		Proposition d = new Proposition("d");
		// Création d'une théorie d'argumentation ASPIC
		AspicArgumentationTheory t = new AspicArgumentationTheory(new PlFormulaGenerator());
		t.setRuleFormulaGenerator(new PlFormulaGenerator());
		// Ajout des règles à la théorie
		DefeasibleInferenceRule r1 = new DefeasibleInferenceRule();
		r1.setConclusion(a);
		r1.addPremise(b);
		r1.addPremise(c);
		t.addRule(r1);
		r1 = new DefeasibleInferenceRule();
		r1.setConclusion(d);
		r1.addPremise(b);
		t.addRule(r1);
		r1 = new DefeasibleInferenceRule();
		r1.setConclusion(new net.sf.tweety.logics.pl.syntax.Negation(d));
		r1.addPremise(a);
		t.addRule(r1);
		// Ajout d'axiomes à la théorie
		t.addAxiom(b);
		t.addAxiom(c);
		Console.WriteLine(t);
		Console.WriteLine();
		// Conversion de la théorie ASPIC en théorie Dung
		DungTheory aaf = t.asDungTheory();
		// Affichage des arguments de la théorie Dung
		foreach (var arg in aaf)
		{
			Console.WriteLine(arg);
		}

		Console.WriteLine();
		// Affichage des attaques de la théorie Dung
		foreach (var att in aaf.getAttacks().toArray())
		{
			Console.WriteLine(att);
		}
	}



	/// <summary>
	/// This example demonstrates how to use the ASPIC+ framework to create an argumentation theory from a file.
	/// </summary>
	public static void ArgumentationAspicFile()
	{
		Console.WriteLine($"\n*** {nameof(ArgumentationAspicFile)}\n");
		PlParser plparser = new PlParser();
		var parser = new AspicParser(plparser, new PlFormulaGenerator());
		string aspicFileContent = File.ReadAllText("./Resources/ex1.aspic");
		AspicArgumentationTheory at = (AspicArgumentationTheory)parser.parseBeliefBase(aspicFileContent);
		SimpleAspicReasoner ar = new SimpleAspicReasoner(AbstractExtensionReasoner.getSimpleReasonerForSemantics(Semantics.CONFLICTFREE_SEMANTICS));
		PropositionalFormula pf = (PropositionalFormula)plparser.parseFormula("p");
		Console.WriteLine(at);
		Console.WriteLine(pf + "\t" + ar.query(at, pf, InferenceMode.CREDULOUS));
	}



	/// <summary>
	/// Exemple d'utilisation de l'argumentation en logique déductive.
	/// </summary>
	public static void ArgumentationDeductiveLogic()
	{
		Console.WriteLine($"\n*** {nameof(ArgumentationDeductiveLogic)}\n");
		TweetyLogging.logLevel = TweetyConfiguration.LogLevel.TRACE;
		TweetyLogging.initLogging();
		SatSolver.setDefaultSolver(new Sat4jSolver());
		// Crée une base de connaissances déductives
		DeductiveKnowledgeBase kb = new DeductiveKnowledgeBase();
		PlParser parser = new PlParser();
		kb.add((PropositionalFormula)parser.parseFormula("s"));
		kb.add((PropositionalFormula)parser.parseFormula("!s || h"));
		kb.add((PropositionalFormula)parser.parseFormula("f"));
		kb.add((PropositionalFormula)parser.parseFormula("!f || !h"));
		kb.add((PropositionalFormula)parser.parseFormula("v"));
		kb.add((PropositionalFormula)parser.parseFormula("!v || !h"));
		Console.WriteLine(kb);
		// Crée un raisonneur en logique déductive simple
		AbstractDeductiveArgumentationReasoner reasoner =
			new SimpleDeductiveReasoner(new ClassicalCategorizer(), new SimpleAccumulator());
		// Récupère la réponse à la requête "h" en utilisant le raisonneur
		Console.WriteLine(reasoner.query(kb, (PropositionalFormula)parser.parseFormula("h")));

	}
}