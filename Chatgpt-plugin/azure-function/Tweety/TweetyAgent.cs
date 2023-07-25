using System.Collections;
using System.Text;
using java.util;
using net.sf.tweety.agents;
using net.sf.tweety.arg.aspic.syntax;
using net.sf.tweety.arg.deductive.accumulator;
using net.sf.tweety.arg.deductive.categorizer;
using net.sf.tweety.arg.deductive.reasoner;
using net.sf.tweety.arg.deductive.syntax;
using net.sf.tweety.arg.dung.reasoner;
using net.sf.tweety.arg.dung.semantics;
using net.sf.tweety.arg.dung.syntax;
using net.sf.tweety.beliefdynamics.kernels;
using net.sf.tweety.beliefdynamics;
using net.sf.tweety.beliefdynamics.mas;
using net.sf.tweety.beliefdynamics.operators;
using net.sf.tweety.commons;
using net.sf.tweety.graphs.orders;
using net.sf.tweety.logics.commons.syntax;
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
using net.sf.tweety.logics.commons.analysis;
using net.sf.tweety.logics.pcl.analysis;
using net.sf.tweety.logics.pcl.parser;
using net.sf.tweety.logics.pcl.syntax;
using net.sf.tweety.math.opt;
using net.sf.tweety.math.opt.solver;
using net.sf.tweety.logics.rcl.parser;
using net.sf.tweety.logics.rcl.reasoner;
using net.sf.tweety.logics.rcl.semantics;
using net.sf.tweety.logics.rcl.syntax;
using net.sf.tweety.logics.rpcl.reasoner;
using net.sf.tweety.logics.rpcl.syntax;
using net.sf.tweety.logics.rpcl.semantics;
using net.sf.tweety.math.probability;
using System.Globalization;
using net.sf.tweety.logics.rpcl.parser;
using ArrayList = java.util.ArrayList;
using System.Collections.Generic;


public class TweetyAgent
{
	public string PathToMarco { get; set; } = "";
	public Parser Parser { get; set; }

	public BeliefBase BeliefSet { get; set; }
	public List<Formula> Formulas { get; set; }

	public Formula Query { get; set; }

	static TweetyAgent()
	{
		ConfigureReasoners();
	}


	/// <summary>
	/// Configure les solveurs pour la logique propositionnelle, la logique des prédicats et la logique modale.
	/// </summary>
	private static void ConfigureReasoners()
	{

		SatSolver.setDefaultSolver(new Sat4jSolver());
		string pathToEProver = @"./ext/EProver/eprover.exe";
		FolReasoner.setDefaultReasoner(new EFOLReasoner(pathToEProver));
		var pathToSpass = "./ext/SPASS/SPASS.exe";
		AbstractModalReasoner.setDefaultReasoner(new SPASSModalReasoner(pathToSpass));
	}

	/// <summary>
	/// Évalue une conjonction de formules logiques propositionnelles.
	/// </summary>
	public string EvaluatePL()
	{

		PossibleWorld w = new PossibleWorld();
		foreach (var proposition in Formulas)
		{
			w.add(proposition);
		}

		return w.satisfies((PlBeliefSet)BeliefSet).ToString(CultureInfo.InvariantCulture); ;
	}

	/// <summary>
	/// Évalue l'incohérence d'un ensemble de croyances.
	/// </summary>
	public string EvaluateInconsistency(BeliefSetInconsistencyMeasure inconsistencyMeasure)
	{
		double result = inconsistencyMeasure.inconsistencyMeasure(BeliefSet).doubleValue();
		return result.ToString(CultureInfo.InvariantCulture);
	}

	/// <summary>
	/// Trouve les sous-ensembles incohérents minimaux.
	/// </summary>
	public string FindMinimalInconsistentSubsets()
	{
		return FindSubsets( false);
	}


	/// <summary>
	/// Trouve les sous-ensembles cohérents maximaus.
	/// </summary>
	public string FindMaximalConsistentSubsets()
	{
		return FindSubsets( true);
	}



	private string FindSubsets( bool isMCS)
	{
		// Create a MUS enumerator with the path to the marco.py file
		MarcoMusEnumerator musEnumerator = new MarcoMusEnumerator(PathToMarco);

		// Get the minimal inconsistent subsets of the belief set
		Collection musCollection;
		if (isMCS)
		{
			musCollection = musEnumerator.maximalConsistentSubsets((Collection)BeliefSet);
		}
		else
		{
			musCollection = musEnumerator.minimalInconsistentSubsets((Collection)BeliefSet);
		}

		// Convert the Collection to a List
		List<PlBeliefSet> result = new List<PlBeliefSet>();
		foreach (PlBeliefSet subset in musCollection.toArray())
		{
			result.Add(subset);
		}
		return string.Join(", ", result.Select(subset => subset.ToString()));
	}


	/// <summary>
	/// Évalue les formules de logique du premier ordre.
	/// </summary>
	public string EvaluateFOLFormulas()
	{
		return FolReasoner.getDefaultReasoner().query((FolBeliefSet)BeliefSet, (FolFormula)Query).booleanValue().ToString(CultureInfo.InvariantCulture); ;
	}



	/// <summary>
	/// Obtient les mesures de culpabilité.
	/// </summary>
	public string GetCulpabilityMeasurements()
	{
		MeanDistanceCulpabilityMeasure culpabilityMeasure = new MeanDistanceCulpabilityMeasure(false);
		var culpabilityResults = new Dictionary<ProbabilisticConditional, double>();
		foreach (ProbabilisticConditional conditional in ((IEnumerable) BeliefSet))
		{
			culpabilityResults[conditional] = culpabilityMeasure.culpabilityMeasure((BeliefSet)BeliefSet, conditional).doubleValue();
		}
		return string.Join(", ", culpabilityResults.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
	}

	/// <summary>
	/// Répare l'ensemble de croyances PCL avec la machine shop pénalisante.
	/// </summary>
	public string RepairPclBeliefSetWithPenalizingCreepingMachineShop()
	{
		PenalizingCreepingMachineShop creepingMachineShop = new PenalizingCreepingMachineShop();
		return ((PclBeliefSet)creepingMachineShop.repair(BeliefSet)).ToString();
	}

	/// <summary>
	/// Répare l'ensemble de croyances PCL avec la machine shop équilibrée.
	/// </summary>
	public string RepairPclBeliefSetWithBalancedMachineShop()
	{
		MeanDistanceCulpabilityMeasure culpabilityMeasure = new MeanDistanceCulpabilityMeasure(false);
		BalancedMachineShop balancedMachineShop = new BalancedMachineShop(culpabilityMeasure);
		return ((PclBeliefSet)balancedMachineShop.repair(BeliefSet)).ToString();
	}

	/// <summary>
	/// Répare l'ensemble de croyances PCL.
	/// </summary>
	public string RepairPclBeliefSet( int repairParameter)
	{
		Solver.setDefaultGeneralSolver(new OpenOptWebSolver());
		return new GeneralizedMeMachineShop(repairParameter).repair(BeliefSet).toString();
	}

	/// <summary>
	/// Évalue les formules RCL.
	/// </summary>
	public string EvaluateRclFormulas()
	{
		TweetyLogging.logLevel = TweetyConfiguration.LogLevel.DEBUG;
		TweetyLogging.initLogging();

		RelationalRankingFunction model = new SimpleRelationalCReasoner(true).getModel((RclBeliefSet)BeliefSet, ((RclParser)Parser).getSignature());

		Dictionary<string, int> formulaRankings = new Dictionary<string, int>();
		foreach (var formula1 in Formulas)
		{
			var formula = (FolFormula)formula1;
			int rank = model.rank(formula).intValue();
			formulaRankings[formula.toString()] = rank;
		}

		return string.Join(", ", formulaRankings.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
	}


	/// <summary>
	/// Évalue les croyances RPCL.
	/// </summary>
	public string EvaluateRpclBeliefs()
	{
		Solver.setDefaultGeneralSolver(new OctaveSqpSolver());
		RpclMeReasoner reasoner = new RpclMeReasoner(new AggregatingSemantics());

		RpclProbabilityDistribution result = reasoner.getModel((RpclBeliefSet)BeliefSet, ((RpclParser) Parser).getSignature());

		var beliefProbabilities = new Dictionary<string, double>();
		foreach (KeyValuePair<Interpretation, Probability> entry in result.entrySet().toArray())
		{
			beliefProbabilities[entry.Key.ToString()] = entry.Value.getValue().doubleValue();
		}

		return string.Join(", ", beliefProbabilities.Select(kvp => $"{kvp.Key}: {kvp.Value}"));

	}



	/// <summary>
	/// Évalue des formules en logique modale.
	/// </summary>
	public string EvaluateModalLogic()
	{

		StringBuilder result = new StringBuilder();
		AbstractModalReasoner reasoner = AbstractModalReasoner.getDefaultReasoner();
		
		return $"{Query.ToString()}: {reasoner.query((ModalBeliefSet)BeliefSet, (FolFormula)Query)}";
	}


	/// <summary>
	/// Révise un ensemble de croyances selon différents opérateurs de révision.
	/// </summary>
	public string ReviseBeliefs( ArrayList agents, Order ordreCredibilite, CrMasBeliefSet baseCroyances, HashSet newInformation)
	{
		var toReturn = new StringBuilder();
		// simple prioritized revision (without considering credibilities)
		CrMasRevisionWrapper rev = new CrMasRevisionWrapper(
			new LeviMultipleBaseRevisionOperator(
				new KernelContractionOperator(new RandomIncisionFunction(), new SimpleReasoner()),
				new DefaultMultipleBaseExpansionOperator()
			));
		toReturn.Append("PRIO       :\t " + rev.revise(baseCroyances, newInformation));

		// simple non-prioritized revision (with credibilities)
		CrMasSimpleRevisionOperator rev2 = new CrMasSimpleRevisionOperator();
		toReturn.Append("N-PRIO CRED:\t " + rev2.revise(baseCroyances, newInformation));

		// credibility-based argumentative revision
		CrMasArgumentativeRevisionOperator theRevision = new CrMasArgumentativeRevisionOperator();
		toReturn.Append("ARG        :\t " + theRevision.revise(baseCroyances, newInformation));
		return toReturn.ToString();
	}

	/// <summary>
	/// Obtient les extensions d'un cadre d'argumentation abstraite (dite de Dung).
	/// </summary>
	public string GetAbstractArgumentationExtensions(AbstractExtensionReasoner reasoner)
	{
		var toReturn = new StringBuilder();
		foreach (Extension extension in reasoner.getModels(BeliefSet).toArray())
		{
			toReturn.Append(extension);
		}

		return toReturn.ToString();
	}

	/// <summary>
	/// Convertit un cadre d'argumentation Aspic en cadre d'argumentation Dung.
	/// </summary>
	public string AspicToDungArgumentation(AspicArgumentationTheory t, bool printArguments = true, bool printAttacks = true)
	{
		var toReturn = new StringBuilder();
		DungTheory aaf = t.asDungTheory();
		if (printArguments)
		{
			foreach (var arg in aaf)
			{
				toReturn.Append(arg);
			}
		}
		if (printAttacks)
		{
			foreach (var att in aaf.getAttacks().toArray())
			{
				toReturn.Append(att);
			}
		}
		return toReturn.ToString();
	}

	/// <summary>
	/// Effectue une logique déductive d'argumentation sur une base de connaissances déductive.
	/// </summary>
	public string ArgumentationDeductiveLogic( PropositionalFormula query)
	{
		var toReturn = new StringBuilder();

		TweetyLogging.logLevel = TweetyConfiguration.LogLevel.TRACE;
		TweetyLogging.initLogging();
		SatSolver.setDefaultSolver(new Sat4jSolver());
		AbstractDeductiveArgumentationReasoner reasoner =
			new SimpleDeductiveReasoner(new ClassicalCategorizer(), new SimpleAccumulator());
		toReturn.Append(reasoner.query(BeliefSet, query));

		return toReturn.ToString();
	}
}