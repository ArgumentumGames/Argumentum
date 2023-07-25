using net.sf.tweety.logics.commons.analysis;
using net.sf.tweety.logics.fol.parser;
using net.sf.tweety.logics.fol.syntax;
using net.sf.tweety.logics.pl.parser;
using net.sf.tweety.logics.pl.syntax;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using net.sf.tweety.commons;
using net.sf.tweety.commons.analysis;
using net.sf.tweety.logics.ml.parser;
using net.sf.tweety.logics.pcl.parser;
using net.sf.tweety.logics.pcl.syntax;
using net.sf.tweety.logics.pl.analysis;
using net.sf.tweety.logics.pl.sat;
using net.sf.tweety.logics.pl.semantics;
using net.sf.tweety.logics.rcl.parser;
using net.sf.tweety.logics.rpcl.parser;
using net.sf.tweety.logics.rpcl.syntax;
using Utf8Json;


/// <summary>
/// That class hold information useful to interpret back the result of a Tweety command into natural language
/// </summary>
public class TweetyContext
{
	public string AnalysisContext { get; set; }

	public string GoalStatement { get; set; }

	/// <summary>
	/// This property holds the human interpretation for each Tweety symbols introduced (statements, arguments, predicates etc.)
	/// </summary>
	public Dictionary<string, string> SymbolsMeaning { get; set; }

}

/// <summary>
/// That base class caries a context for the command to be executed, that is serialized back into the response to ease further natural language interpretation.
/// </summary>
public abstract class TweetyMessage
{
	public TweetyContext Context { get; set; }
}

public enum InconsistencyMeasureType
{
	//This measure seeks an interpretation I such that the the sum of the distances between every formula of the knowledge base and I is minimal. The value  of the inconsistency is than exactly this sum. The distance can be parameterized.
	DSumInconsistencyMeasure,
	// This measure seeks an interpretation I such that the maximal distance between every formula of the knowledge base and I is minimal. The value of the inconsistency is exactly this value.
	DMaxInconsistencyMeasure,
	//  This measure seeks an interpretation I such that the number of formulas not satisfied by I is minimal. The value of the inconsistency is exactly this number.
	DHitInconsistencyMeasure,
	//MI^C inconsistency measure, uses an enumerator to find minimal unsatisfiable subsets (MUSes).
	MicInconsistencyMeasure,
	// The knowledge base is called maximal n-consistent if every subset of the base of size n is consistent, and this is not true for n+1. As a measure of inconsistency, I(K) is defined as |K|-n (in order to have I(K)=0 for consistent K).
	NConsInconsistencyMeasure,
	// I_{P_m} (K)= \sum_{x\in Var(K)}|Pm(x)|*|Pm(-x)| where Pm(x) is the set of all (not necessarily consistent)* minimal proofs of x in K that mention x.
	PmInconsistencyMeasure
}

public class TweetyCommand: TweetyMessage
{
	/// <summary>
	/// This property holds the name of the TweetyAgent method that will be called.
	/// </summary>
	public string MethodNameToExecuteInTweetyAgent { get; set; }

	public InconsistencyMeasureType InconsistencyMeasure { get; set; }

	/// <summary>
	/// This property holds a belief set in a string format, to be parsed using one of the supported file format readers or a specific logic parsers depending on the target TweetyAgentMethod.
	/// </summary>
	public string BeliefSet { get; set; }

	/// <summary>
	/// This property holds a collection of string formulas, to be parsed using one of the supported file format readers or a specific logic parsers depending on the target TweetyAgentMethod.
	/// </summary>
	public Dictionary<string, string> Formulas { get; set; }

	/// <summary>
	/// This property holds the query to execute on the beliefset, depending on the target TweetyAgentMethod.
	/// </summary>
	public string Query { get; set; }


	/// <summary>
	/// Static method to be called with serialized parameters
	/// </summary>
	public static string Run(string jsonCommand)
	{
		TweetyCommand deserialized = Utf8Json.JsonSerializer.Deserialize<TweetyCommand>(jsonCommand);
		return deserialized.Run();
	}



	/// <summary>
	/// That method parses the instance string fields into the appropriate Tweety beliefsets and formulas, and then calls TweetyAgent appropriate methods to run the command.
	/// </summary>
	/// <returns>the</returns>
	public string Run()
	{
		try
		{
			var agent = new TweetyAgent();
			Dictionary<string, object> parameters = ParseMethodParameters(agent);
			var agentResponse = CallTweetyAgent(MethodNameToExecuteInTweetyAgent, parameters, agent);

			return JsonSerializer.PrettyPrint(JsonSerializer.ToJsonString(agentResponse));
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return e.ToString();
		}

	}

	/// <summary>
	/// Prepares the parameters to pass to Tweety from string beliefset, formulas and query inputs
	/// </summary>
	/// <param name="tweetyAgent"></param>
	private Dictionary<string, object> ParseMethodParameters(TweetyAgent tweetyAgent)
	{

		var toReturn = new Dictionary<string, object>();

		switch (MethodNameToExecuteInTweetyAgent)
		{
			case nameof(TweetyAgent.EvaluatePL):
				// Create a new PlParser
				PlParser parser = new PlParser();
				tweetyAgent.Parser = parser;

				List<Formula> parsedFormulasPL = Formulas.Values.Select(f => parser.parseFormula(f)).ToList();
				tweetyAgent.Formulas = parsedFormulasPL;
				break;

			case nameof(TweetyAgent.EvaluateInconsistency):
				// Create a new parser
				var inconsistencyParser = new PlParser();
				tweetyAgent.Parser = inconsistencyParser;
				// Parse belief set using parser
				tweetyAgent.BeliefSet= inconsistencyParser.parseBeliefBase(BeliefSet);
				PropositionalSignature sig = (PropositionalSignature)tweetyAgent.BeliefSet.getSignature();
				BeliefSetInconsistencyMeasure inconsistencyMeasure;
				switch (InconsistencyMeasure)
				{
					case InconsistencyMeasureType.DSumInconsistencyMeasure:
						inconsistencyMeasure = new DSumInconsistencyMeasure((InterpretationDistance)new DalalDistance(), (InterpretationIterator)new PossibleWorldIterator(sig));
						break;
					case InconsistencyMeasureType.DHitInconsistencyMeasure:
						inconsistencyMeasure = new DHitInconsistencyMeasure((InterpretationDistance)new DalalDistance(), (InterpretationIterator)new PossibleWorldIterator(sig));
						break;
					case InconsistencyMeasureType.DMaxInconsistencyMeasure:
						inconsistencyMeasure = new DMaxInconsistencyMeasure((InterpretationDistance)new DalalDistance(), (InterpretationIterator)new PossibleWorldIterator(sig));
						break;
					case InconsistencyMeasureType.MicInconsistencyMeasure:
						inconsistencyMeasure = new MicInconsistencyMeasure((MusEnumerator) PlMusEnumerator.getDefaultEnumerator());
						break;
					case InconsistencyMeasureType.NConsInconsistencyMeasure:
						inconsistencyMeasure = new NConsInconsistencyMeasure((MusEnumerator)PlMusEnumerator.getDefaultEnumerator());
						break;
					case InconsistencyMeasureType.PmInconsistencyMeasure:
						inconsistencyMeasure = new PmInconsistencyMeasure();
						break;
					default:
						throw new NotImplementedException($"unknown measure {InconsistencyMeasure}");
						break;
				}

				// Add parsed objects to the parameters dictionary
				
				toReturn.Add("inconsistencyMeasure", inconsistencyMeasure);
				break;
			case nameof(TweetyAgent.FindMinimalInconsistentSubsets):
				// Create a new PlParser
				PlParser misParser = new PlParser();
				tweetyAgent.Parser = misParser;

				// Parse belief set using PlParser
				var misBeliefSet = misParser.parseBeliefBase(BeliefSet);
				tweetyAgent.BeliefSet = misBeliefSet;
				break;
			case nameof(TweetyAgent.FindMaximalConsistentSubsets):
				// Create a new PlParser
				PlParser mcsParser = new PlParser();
				//toReturn.Add("mcsParser", mcsParser);
				tweetyAgent.Parser = mcsParser;
				// Parse belief set using PlParser
				var mcsBeliefSet = mcsParser.parseBeliefBase(BeliefSet);
				tweetyAgent.BeliefSet = mcsBeliefSet;
				break;
			case nameof(TweetyAgent.EvaluateFOLFormulas):
				// Create a new FolParser
				FolParser folParser = new FolParser();
				tweetyAgent.Parser = folParser;
				// Parse belief set and signature using FolParser
				var fbs = folParser.parseBeliefBase(BeliefSet);
				FolSignature signature = folParser.parseSignature(BeliefSet); // assuming the signature is also included in the belief set

				// Parse formulas using FolParser
				List<Formula> parsedFormulas = Formulas.Values.Select(f => folParser.parseFormula(f)).ToList();

				// Parse query formula using FolParser
				Formula queryFormula = folParser.parseFormula(Query);

				// Add parsed objects to the parameters dictionary
				tweetyAgent.BeliefSet = fbs;
				
				tweetyAgent.Formulas = parsedFormulas;
				tweetyAgent.Query =queryFormula;
				break;
			case nameof(TweetyAgent.GetCulpabilityMeasurements):
			case nameof(TweetyAgent.RepairPclBeliefSetWithPenalizingCreepingMachineShop):
			case nameof(TweetyAgent.RepairPclBeliefSetWithBalancedMachineShop):
				// Création d'un nouveau PclParser
				PclParser pclParser = new PclParser();
				// Analyse de la base de croyances avec PclParser
				var beliefBase = pclParser.parseBeliefBase(BeliefSet);
				tweetyAgent.BeliefSet = beliefBase;
				break;
			case nameof(TweetyAgent.RepairPclBeliefSet):
				var pclParser2 = new PclParser();
				tweetyAgent.Parser = pclParser2;
				List<Formula> parsedFormulas2 = Formulas.Values.Select(f => pclParser2.parseFormula(f)).ToList();
				tweetyAgent.Formulas = parsedFormulas2;
				//TODO handle repairParameter properly
				toReturn.Add("repairParameter", 1);
				break;
				break;
			case nameof(TweetyAgent.EvaluateRclFormulas):
				// Création d'un nouveau RclParser
				RclParser rclParser = new RclParser();
				tweetyAgent.Parser = rclParser;
				var beliefBase2 = rclParser.parseBeliefBase(BeliefSet);
				tweetyAgent.BeliefSet = beliefBase2;

				// Analyse des formules avec RclParser
				List<Formula> parsedFormulas3 = Formulas.Values.Select(f => rclParser.parseFormula(f)).ToList();
				tweetyAgent.Formulas = parsedFormulas3;

				break;
				break;
			case nameof(TweetyAgent.EvaluateRpclBeliefs):
				// Création d'un nouveau RpclParser
				RpclParser rpclParser = new RpclParser();
				// Analyse de la base de croyances avec RpclParser
				var beliefBase3 = rpclParser.parseBeliefBase(BeliefSet);
				
				tweetyAgent.BeliefSet = beliefBase3;
				
				break;
			case nameof(TweetyAgent.EvaluateModalLogic):
				// Create a new ModalLogicParser
				var mlParser = new ModalParser();
				tweetyAgent.Parser = mlParser;
				// Parse belief set using ModalLogicParser
				var mlBeliefSet = mlParser.parseBeliefBase(BeliefSet);
				tweetyAgent.BeliefSet = mlBeliefSet;

				// Parse formulas using ModalLogicParser
				List<Formula> mlParsedFormulas = Formulas.Values.Select(f => mlParser.parseFormula(f)).ToList();
				tweetyAgent.Formulas = mlParsedFormulas;
				break;
			case nameof(TweetyAgent.ReviseBeliefs):
				//TODO: Create parameters
				break;
			case nameof(TweetyAgent.GetAbstractArgumentationExtensions):
				//TODO: Create parameters
				break;
			case nameof(TweetyAgent.AspicToDungArgumentation):
				//TODO: Create parameters
				break;
			case nameof(TweetyAgent.ArgumentationDeductiveLogic):
				//TODO: Create parameters
				break;
			default:
				throw new NotImplementedException($"unknown method {MethodNameToExecuteInTweetyAgent}");
				break;
		}

		return toReturn;
	}

	/// <summary>
	/// Calls the TweetyAgent method together with the previously parsed parameters
	/// </summary>
	private TweetyResponse CallTweetyAgent(string methodName, Dictionary<string, object> parameters, TweetyAgent agent)
	{


		MethodInfo? method = agent.GetType().GetMethod(methodName);

		if (method == null)
		{
			throw new Exception($"No method '{methodName}' found in TweetyAgent.");
		}

		object[] methodParams = method.GetParameters()
			.Select(p => parameters.ContainsKey(p.Name) ? parameters[p.Name] : null)
			.ToArray();

		string methodResult = (string)method.Invoke(agent, methodParams);

		return new TweetyResponse() { Context = Context, Result = methodResult };
	}


}

/// <summary>
/// That class is serialized and provided as an answer to your command
/// </summary>
public class TweetyResponse : TweetyMessage
{

	public string Result { get; set; }


}