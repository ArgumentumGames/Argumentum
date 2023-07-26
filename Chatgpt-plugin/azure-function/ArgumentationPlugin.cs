using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Microsoft.SemanticKernel.Orchestration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YamlDotNet.Core;
using net.sf.tweety.commons;
using net.sf.tweety.logics.fol.parser;
using net.sf.tweety.logics.fol.reasoner;
using net.sf.tweety.logics.ml.parser;
using net.sf.tweety.logics.ml.reasoner;
using net.sf.tweety.logics.pl.parser;
using net.sf.tweety.logics.pl.reasoner;
using Parser = net.sf.tweety.commons.Parser;
using net.sf.tweety.logics.pl.sat;
using net.sf.tweety.logics.commons.analysis;
using net.sf.tweety.logics.pl.analysis;
using net.sf.tweety.commons.analysis;
using net.sf.tweety.logics.pl.semantics;
using AIPlugins.AzureFunctions.Extensions;
using Microsoft.Extensions.Logging;
using sk_chatgpt_azure_function;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SkillDefinition;
using Models;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace sk_chatgpt_azure_function
{

	public class ArgumentationPlugin
	{
		private readonly IKernel _kernel;
		private readonly ILogger _logger;
		private readonly IAIPluginRunner _pluginRunner;




		static ArgumentationPlugin()
		{
			ConfigureReasoners();
		}

		public ArgumentationPlugin(IKernel kernel, IAIPluginRunner pluginRunner, ILoggerFactory loggerFactory)
		{
			this._kernel = kernel;
			this._pluginRunner = pluginRunner;
			this._logger = loggerFactory.CreateLogger<ArgumentationPlugin>();
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
			measure,
			baseFallacyPath,
			fallaciesShort,
			fallaciesLong
		}

		public enum SemanticFunctionNames
		{
			BeliefStateToQueriesPl,
			LogicAnalysisToText,
			TextToBeliefsetFol,
			TextToBeliefsetMl,
			TextToBeliefsetPl,
			TextToLogicType,
			FallacyAnalysisBranch
		}


		// This function will handle the propositional logic
		[Function("RunPropositionalQueries")]
		[OpenApiOperation(operationId: "RunPropositionalQueries", tags: new[] { "ExecuteFunction" }, Description = "Runs a series of propositional logic queries against a belief set and returns the results")]
		[OpenApiParameter(name: "input", Description = "The belief set in a string format", Required = true, In = ParameterLocation.Query)]
		[OpenApiParameter(name: "queries", Description = "The formulas to check against the belief set, one per line", Required = true, In = ParameterLocation.Query)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "Returns the result of the queries against the belief set.")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "Returns the error of the input.")]
		public HttpResponseData RunPropositionalQueries([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
		{
			return RunLogic(req, LogicType.Propositional);
		}

		// This function will handle the first order logic
		[Function("RunFirstOrderQueries")]
		[OpenApiOperation(operationId: "RunFirstOrderQueries", tags: new[] { "ExecuteFunction" }, Description = "Runs a series of first order logic queries against a belief set and returns the results")]
		[OpenApiParameter(name: "input", Description = "The belief set in a string format", Required = true, In = ParameterLocation.Query)]
		[OpenApiParameter(name: "queries", Description = "The formulas to check against the belief set, one per line", Required = true, In = ParameterLocation.Query)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "Returns the result of the queries against the belief set.")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "Returns the error of the input.")]
		public HttpResponseData RunFirstOrderQueries([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
		{
			return RunLogic(req, LogicType.FirstOrder);
		}

		//// This function will handle the modal logic
		//[Function("RunModalLogicQueries")]
		//[OpenApiOperation(operationId: "RunModalLogicQueries", tags: new[] { "ExecuteFunction" }, Description = "Runs a series of modal logic queries against a belief set and returns the results")]
		//[OpenApiParameter(name: "input", Description = "The belief set in a string format", Required = true, In = ParameterLocation.Query)]
		//[OpenApiParameter(name: "queries", Description = "The formulas to check against the belief set, one per line", Required = true, In = ParameterLocation.Query)]
		//[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "Returns the result of the queries against the belief set.")]
		//[OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "Returns the error of the input.")]
		//public HttpResponseData RunModalLogicQueries([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
		//{
		//	return RunLogic(req, LogicType.Modal);
		//}
		[Function("LoadFallacies")]
		[OpenApiOperation(operationId: "LoadFallacies", tags: new[] { "ExecuteFunction" }, Description = "Loads a branch from a fallacy taxonomy, returned serialized in short or detailed format")]
		[OpenApiParameter(name: "input", Description = "The optional path of the branch to restrict the analysis on, within a taxonomy of fallacies", Required = true, In = ParameterLocation.Query)]
		[OpenApiParameter(name: "shortVersion", Description = "true to return a condensed branch with paths and title, false for a detailed version with description and example", Required = true, In = ParameterLocation.Query)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "Returns the analysis of the text within the given branch")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "Returns the error of the input.")]
		public HttpResponseData LoadFallacies(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req, bool shortVersion = true)
		{

			string jsonInput = req.Query["input"];
			string input;

			JsonSerializer jsonSerializer = new();
			using (var sr = new JsonTextReader(new StringReader(jsonInput)))
			{
				input = jsonSerializer.Deserialize<OpenAPIMessage>(sr).content;

			}

			var fallacies = LoadFallacies(false, input, shortVersion, 30).ConfigureAwait(false);
			var serializeResult = JsonConvert.SerializeObject(fallacies, Formatting.Indented);

			var response = req.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Content-Type", "text/plain;charset=utf-8");
			response.WriteString(serializeResult);
			return response;
		}



		[Function("AnalyzeTextFallaciesInBranch")]
		[OpenApiOperation(operationId: "AnalyzeTextFallaciesInBranch", tags: new[] { "ExecuteFunction" }, Description = "in a given text, identifies potential fallacies belonging to a fallacy taxonomy branch")]
		[OpenApiParameter(name: "input", Description = "The argumentative text to perform analysis against", Required = true, In = ParameterLocation.Query)]
		[OpenApiParameter(name: nameof(VariableNames.baseFallacyPath), Description = "The optional path of the branch to restrict the analysis on, within a taxonomy of fallacies", Required = false, In = ParameterLocation.Query)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "Returns the analysis of the text within the given branch")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "Returns the error of the input.")]
		public HttpResponseData AnalyzeTextFallaciesInBranch([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,  string baseFallacyPath = "")
		{
			ContextVariables contextVariables = LoadContextVariablesFromRequest(req);

			var fallaciesShort = LoadFallacies(false, baseFallacyPath, true, 30).ConfigureAwait(false).GetAwaiter().GetResult();
			var strFallaciesShort = JsonConvert.SerializeObject(fallaciesShort, Formatting.Indented);

			contextVariables.Set(nameof(VariableNames.fallaciesShort), strFallaciesShort);

			var fallaciesLong = LoadFallacies(false, baseFallacyPath, false, 4).ConfigureAwait(false).GetAwaiter().GetResult();
			var strFallaciesLong = JsonConvert.SerializeObject(fallaciesLong, Formatting.Indented);

			contextVariables.Set(nameof(VariableNames.fallaciesLong), strFallaciesLong);

			var appSettings = AppSettings.LoadSettings();

			if (!this._kernel.Skills.TryGetFunction(
				    skillName: appSettings.AIPlugin.NameForModel,
				    functionName: nameof(SemanticFunctionNames.FallacyAnalysisBranch),
				    out ISKFunction? function))
			{
				HttpResponseData errorResponse = req.CreateResponse(HttpStatusCode.NotFound);
				errorResponse.WriteString($"Function {SemanticFunctionNames.FallacyAnalysisBranch} not found");
				return errorResponse;
			}

			var result = this._kernel.RunAsync(contextVariables, function).Result;
			if (result.ErrorOccurred)
			{
				HttpResponseData errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
				errorResponse.WriteString(result.LastErrorDescription);
				return errorResponse;
			}

			var response = req.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Content-Type", "text/plain;charset=utf-8");
			response.WriteString(result.Result);
			return response;
		}





		/// <summary>
		/// Runs a semantic function using the operationID and returns back an HTTP response.
		/// </summary>
		/// <param name="req"></param>
		/// <param name="operationId"></param>
		private async Task<HttpResponseData> RunAIPluginOperationAsync(HttpRequestData req, string operationId)
		{
			ContextVariables contextVariables = LoadContextVariablesFromRequest(req);

			var appSettings = AppSettings.LoadSettings();

			if (!this._kernel.Skills.TryGetFunction(
				skillName: appSettings.AIPlugin.NameForModel,
				functionName: operationId,
				out ISKFunction? function))
			{
				HttpResponseData errorResponse = req.CreateResponse(HttpStatusCode.NotFound);
				await errorResponse.WriteStringAsync($"Function {operationId} not found");
				return errorResponse;
			}

			var result = await this._kernel.RunAsync(contextVariables, function);
			if (result.ErrorOccurred)
			{
				HttpResponseData errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
				await errorResponse.WriteStringAsync(result.LastErrorDescription);
				return errorResponse;
			}

			var response = req.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Content-Type", "text/plain;charset=utf-8");
			await response.WriteStringAsync(result.Result);
			return response;
		}

		/// <summary>
		/// Grabs the context variables to send to the semantic function from the original HTTP request.
		/// </summary>
		/// <param name="req"></param>
		protected static ContextVariables LoadContextVariablesFromRequest(HttpRequestData req)
		{
			ContextVariables contextVariables = new ContextVariables();
			foreach (string? key in req.Query.AllKeys)
			{
				if (!string.IsNullOrEmpty(key))
				{
					contextVariables.Set(key, req.Query[key]);
				}
			}

			// If "input" was not specified in the query string, then check the body
			if (string.IsNullOrEmpty(req.Query.Get("input")))
			{
				// Load the input from the body
				string? body = req.ReadAsString();
				if (!string.IsNullOrEmpty(body))
				{
					contextVariables.Update(body);
				}
			}

			return contextVariables;
		}

		public class OpenAPIMessage
		{
			public string content { get; set; }

			public string contentType { get; set; }
		}


		private HttpResponseData RunLogic(HttpRequestData req, LogicType logicType)
		{
			
			string jsonInput = req.Query["input"];
			string jsonQueries = req.Query["queries"];

			string input;
			string queries;

			JsonSerializer jsonSerializer = new();
			using (var sr = new JsonTextReader(new StringReader(jsonInput)))
			{
				input = jsonSerializer.Deserialize<OpenAPIMessage>(sr).content;

			}
			using (var sr = new JsonTextReader(new StringReader(jsonQueries)))
			{
				queries = jsonSerializer.Deserialize<OpenAPIMessage>(sr).content;

			}

			// You can add error checks here if input, or queries is not supplied or invalid

			// Construct the SKContext
			SKContext context = new SKContext();
			context[nameof(VariableNames.queries)] = queries;

			// Call the QueryLogicReasoner function from TweetySkill
			string result = RunQuery(input, context, logicType);

			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Content-Type", "text/plain");
			response.WriteString(result);

			return response;
		}


		private static string RunQuery(string input, SKContext context, LogicType logicType)
		{
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
					throw new ArgumentException($"Unsupported logic type: {logicType}");
			}
			beliefSet = parser.parseBeliefBase(input);
			var strQueries = context[nameof(VariableNames.queries)].ToString().Split(',').Where(s => s.Trim().Length > 0);
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



		private async Task<Dictionary<string, KeyValuePair<string, Dictionary<string, object>>>> LoadFallacies(bool useDebugPath, string basePath, bool shortVersion, int fallacyNb)
		{
			
			var appSettings = AppSettings.LoadSettings();

			// load dataset in chunks, 
			var filteredFallacies = await appSettings.ArgSettings.SourceDataset.GetContentDictionary(shortVersion? appSettings.ArgSettings.FieldsShort: appSettings.ArgSettings.FieldsLong, ",", "path", "path", new List<string> {basePath},  true, useDebugPath).ConfigureAwait(false);

			var sortedFallacies = filteredFallacies.OrderBy(pair => pair.Key.Length).Take(fallacyNb).ToDictionary(pair =>pair.Key);

			return sortedFallacies;

		}


	}

}



