using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
using net.sf.tweety.arg.aspic.examples;
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
using net.sf.tweety.commons;
using net.sf.tweety.logics.fol.parser;
using net.sf.tweety.logics.fol.reasoner;
using net.sf.tweety.logics.ml.parser;
using net.sf.tweety.logics.ml.reasoner;
using net.sf.tweety.logics.pcl.parser;
using net.sf.tweety.logics.pcl.syntax;
using net.sf.tweety.logics.pl.parser;
using net.sf.tweety.logics.pl.reasoner;
using net.sf.tweety.logics.pl.sat;
using net.sf.tweety.logics.pl.semantics;
using net.sf.tweety.logics.pl.syntax;

namespace TweetyIKVM
{
	public class TweetySkill
	{
		private const string TheFormulaToCheckAgainstTheBeliefSet = "The formula to check against the belief set";

		public enum LogicType
		{
			Propositional,
			FirstOrder,
			Modal
			// Ajoutez d'autres types de logique ici si nécessaire
		}

		public enum VariableNames
		{
			logicType,
			queries,
		}


		//[SKFunctionName(nameof(CheckPossibleWorldPL))]
		//[SKFunction("Check if a possible world satisfies a belief set in propositional logic")]
		//[SKFunctionInput(Description = "The belief set in a string format that can be parsed by parseBeliefBase")]
		//[SKFunctionContextParameter(Name = "world", Description = "The possible world as a list of true propositions")]
		public static string CheckPossibleWorldPL(string input, SKContext context)
		{
			// Parse the belief set
			var beliefSet = (PlBeliefSet)new PlParser().parseBeliefBase(input);

			// Create a possible world by specifying the true propositions
			PossibleWorld w = new PossibleWorld();
			var propositions = context["world"].ToString().Split(',');
			foreach (var prop in propositions)
			{
				w.add(new Proposition(prop.Trim()));
			}

			// Check if the possible world satisfies all the formulas in the belief set
			bool satisfiesAll = beliefSet.toArray().All(f => w.satisfies((Formula)f));

			// Return the result
			return "The possible world " + context["world"].ToString() + " " + (satisfiesAll ? "satisfies" : "does not satisfy") + " all the formulas in the belief set.";
		}


		//[SKFunctionName(nameof(PropositionalQueryLogicReasoner))]
		//[SKFunction("Checks if a formula is true in a belief set in propositional logic")]
		//[SKFunctionInput(Description = "The belief set in a string format that conforms to the propositional logic BNF")]
		//[SKFunctionContextParameter(Name = nameof(VariableNames.queries), Description = TheFormulaToCheckAgainstTheBeliefSet)]
		public static string PropositionalQueryLogicReasoner(string input, SKContext context)
		{
			context[nameof(VariableNames.logicType)] = LogicType.Propositional.ToString();
			return QueryLogicReasoner(input, context);
		}

		//[SKFunctionName(nameof(FirstOrderQueryLogicReasoner))]
		//[SKFunction("Checks if a formula is true in a belief set in first order logic")]
		//[SKFunctionInput(Description = "The belief set in a string format that conforms to the first order logic BNF")]
		//[SKFunctionContextParameter(Name = nameof(VariableNames.queries), Description = TheFormulaToCheckAgainstTheBeliefSet)]
		public static string FirstOrderQueryLogicReasoner(string input, SKContext context)
		{
			context[nameof(VariableNames.logicType)] = LogicType.FirstOrder.ToString();
			return QueryLogicReasoner(input, context);
		}

		//[SKFunctionName(nameof(ModalQueryLogicReasoner))]
		//[SKFunction("Checks if a formula is true in a belief set in modal logic")]
		//[SKFunctionInput(Description = "The belief set in a string format that conforms to the modal logic BNF")]
		//[SKFunctionContextParameter(Name = nameof(VariableNames.queries), Description = TheFormulaToCheckAgainstTheBeliefSet)]
		public static string ModalQueryLogicReasoner(string input, SKContext context)
		{
			context[nameof(VariableNames.logicType)] = LogicType.Modal.ToString();
			return QueryLogicReasoner(input, context);
		}



		//[SKFunctionName(nameof(QueryLogicReasoner))]
		//[SKFunction("Check if a formula is true in a belief set in the given type of logic")]
		//[SKFunctionInput(Description = "The belief set in a string format that can be parsed by parseBeliefBase")]
		//[SKFunctionContextParameter(Name = "query", Description = "The formula to check against the belief set")]
		//[SKFunctionContextParameter(Name = "logicType", Description = "The type of formal logic to use for checking the formula")]
		public static string QueryLogicReasoner(string input, SKContext context)
		{
			// Determine the type of logic to use
			var strlogic = context[nameof(VariableNames.logicType)].Trim();
			var newLineIndex = strlogic.IndexOf("\n", StringComparison.Ordinal);
			if (newLineIndex > 0)
			{
				strlogic = strlogic.Substring(0, newLineIndex);
			}
			LogicType logicType = (LogicType)Enum.Parse(typeof(LogicType), strlogic);

			// Initialize the appropriate parser and reasoner
			Parser parser;
			BeliefBase beliefSet;
			Formula formula;
			Reasoner reasoner;
			bool result;

			switch (logicType)
			{
				case LogicType.Propositional:
					parser = new PlParser();
					reasoner = new SatReasoner();

					break;
				case LogicType.FirstOrder:
					parser = new FolParser();
					reasoner = FolReasoner.getDefaultReasoner();
					break;
				case LogicType.Modal:
					parser = new ModalParser();
					reasoner = AbstractModalReasoner.getDefaultReasoner();
					break;
				default:
					throw new ArgumentException($"Unsupported logic type: {strlogic}");
			}
			beliefSet = parser.parseBeliefBase(input);
			var strQueries = context[nameof(VariableNames.queries)].ToString().Split('\n').Where(s => s.Trim().Length>0);
			var toReturn = new StringBuilder();
			foreach (var strQuery in strQueries)
			{
				formula = parser.parseFormula(strQuery);
				result = ((java.lang.Boolean)reasoner.query(beliefSet, formula)).booleanValue();
				var strResult =
					$"The query {strQuery} is {(result ? "accepted" : "rejected")} by the belief set.";
				toReturn.AppendLine(strResult);
			}
			

			// Return the result
			return toReturn.ToString();
		}



		//[SKFunctionName(nameof(QueryAspic))]
		//[SKFunction("Query an ASPIC+ argumentation theory with a propositional formula in credulous inference mode")]
		//[SKFunctionInput(Description = "The content of an ASPIC+ file")]
		//[SKFunctionContextParameter(Name = "query", Description = "The query to test against the theory")]
		public static string QueryAspic(string input, SKContext context)
		{
			// Initialisation du parser pour les logiques propositionnelles
			PlParser plparser = new PlParser();

			// Initialisation du parser pour l'ASPIC
			var parser = new AspicParser(plparser, new PlFormulaGenerator());

			// Parsing du contenu du fichier ASPIC
			AspicArgumentationTheory at = (AspicArgumentationTheory)parser.parseBeliefBase(input);

			// Initialisation du raisonneur pour l'ASPIC
			SimpleAspicReasoner ar = new SimpleAspicReasoner(AbstractExtensionReasoner.getSimpleReasonerForSemantics(Semantics.CONFLICTFREE_SEMANTICS));

			string query;
			query = context[nameof(query)].ToString();
			// Parsing de la requête à tester
			PropositionalFormula pf = (PropositionalFormula)plparser.parseFormula(query);

			// Test de la requête contre la théorie ASPIC
			bool result = ar.query(at, pf, InferenceMode.CREDULOUS).booleanValue();

			// Retour du résultat
			return "The query " + query + " is " + (result ? "accepted" : "rejected") + " by the ASPIC+ theory.";
		}

		//[SKFunctionName(nameof(AspicToDungAttacks))]
		//[SKFunction("Generate Dung theory from ASPIC+ file and exhibit attack arguments")]
		//[SKFunctionInput(Description = "The content of an ASPIC+ file")]
		public static string AspicToDungAttacks(string input)
		{
			// Initialize the propositional logic parser
			PlParser plparser = new PlParser();

			// Initialize the ASPIC parser
			var parser = new AspicParser(plparser, new PlFormulaGenerator());

			// Parse the ASPIC file content
			AspicArgumentationTheory t = (AspicArgumentationTheory)parser.parseBeliefBase(input);

			// Convert the ASPIC theory into a Dung theory
			DungTheory aaf = t.asDungTheory();

			// Create a StringBuilder to build the result string
			StringBuilder sb = new StringBuilder();

			// Add the arguments of the Dung theory to the result string
			sb.AppendLine("Arguments:");
			foreach (var arg in aaf)
			{
				sb.AppendLine(arg.ToString());
			}

			sb.AppendLine(); // Add a line break for readability

			// Add the attacks of the Dung theory to the result string
			sb.AppendLine("Attacks:");
			foreach (var att in aaf.getAttacks().toArray())
			{
				sb.AppendLine(att.ToString());
			}

			// Return the result string
			return sb.ToString();
		}




		//[SKFunctionName(nameof(QueryDeductiveArgumentation))]
		//[SKFunction("Query a deductive argumentation theory with a propositional formula")]
		//[SKFunctionInput(Description = "The belief set in a string format that can be parsed by parseBeliefBase")]
		//[SKFunctionContextParameter(Name = "query", Description = "The query to test against the theory")]
		public static string QueryDeductiveArgumentation(string input, SKContext context)
		{
			// Initialize the propositional logic parser
			PlParser parser = new PlParser();

			// Create a deductive knowledge base
			DeductiveKnowledgeBase kb = new DeductiveKnowledgeBase();

			// Parse the belief set and add each formula to the knowledge base
			foreach (var formula in (BeliefSet)parser.parseBeliefBase(input))
			{
				kb.add((PropositionalFormula)formula);
			}

			// Create a deductive argumentation reasoner
			AbstractDeductiveArgumentationReasoner reasoner = new SimpleDeductiveReasoner(new ClassicalCategorizer(), new SimpleAccumulator());

			// Parse the query
			PropositionalFormula queryFormula = (PropositionalFormula)parser.parseFormula(context["query"].ToString());

			// Query the knowledge base using the reasoner
			var result = reasoner.query(kb, queryFormula);

			// Return the result
			return $"The query {context["query"]} returns {result} by the deductive argumentation theory reasoner.";
		}




	}
}
