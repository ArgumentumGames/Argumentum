using OpenAI.GPT3.ObjectModels;
using Spectre.Console;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Argumentum.AssetConverter.DatasetUpdater;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Planning;
using Microsoft.SemanticKernel.Skills.Core;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
using System.Diagnostics;
using System.Text.Json;
using ImageMagick;
using Microsoft.SemanticKernel.Planning.Stepwise;
using Kernel = Microsoft.SemanticKernel.Kernel;


namespace Argumentum.AssetConverter;

public class DatasetUpdaterConfig
{

	public SemanticMode SemanticMode { get; set; } = SemanticMode.RunChGPTPluginAnalysis;

	public string SystemPrompt { get; set; } = Resource1.JsonPromptSystem;

	public string UserPrompt { get; set; } = Resource1.JsonPromptSampleUser;


	public string AssistantPrompt { get; set; } = Resource1.JsonPromptSampleAssistant;

	public string OpenAIKeyPath { get; set; } = @"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\OpenAI-Key.txt";

	public string Model { get; set; } = Models.ChatGpt3_5Turbo;

	public int ChunkSize { get; set; } = 3;

	public int NbMessageCalls { get; set; } = 3;

	public int SkipChunkNb { get; set; } = 0;

	public int TakeChunkNb { get; set; } = 0;

	public int MaxDegreeOfParallelismWebService { get; set; } = 8;

	public List<string> FieldsToInclude { get; set; } = new List<string>(new[]
	{
		"path",
		"family_fr",
		"subfamily_fr",
		"subsubfamily_fr",
		"title_fr",
		"description_fr",
		"remark_fr",
		"link_fr"
	});

	public string PrimaryField { get; set; } = "path";

	public List<string> FieldsToUpdate { get; set; } = new List<string>(new[]
	{
		//"title_fr",
		"description_fr",
		"remark_fr",
		"link_fr"
	});

	public DataSetInfo SourceDataset { get; set; } = new DataSetInfo()
	{
		Name = "Argumentum - Virtues - Taxonomy",
		ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum%20Virtues%20-%20Taxonomy.csv",
		DebugFilePath = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum Virtues - Taxonomy.csv",
	};

	public string TargetPath { get; set; } = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv";


	public async Task Apply(bool useDebugPath)
	{
		var openAIKey = await File.ReadAllTextAsync(OpenAIKeyPath);

		switch (SemanticMode)
		{
			case SemanticMode.UpdateWithChatpGptAPI:
				await UpdateWithChatGPT(useDebugPath, openAIKey);
				break;
			case SemanticMode.RunChGPTPluginAnalysis:
				await RunChGPTPluginAnalysis(useDebugPath, openAIKey);
				break;
			//case SemanticMode.IndexWithSemanticKernel:
			//	await IndexTaxonomyWithSemanticKernel(useDebugPath, openAIKey);
			//	break;
			default:
				throw new ArgumentOutOfRangeException();
		}
		
	}

	private async Task RunChGPTPluginAnalysis(bool useDebugPath, string openAiKey)
	{
		var sw = Stopwatch.StartNew();

		var kernel = Kernel.Builder
			.WithLogger(ConsoleLogger.Log)
			//.WithOpenAITextCompletionService("text-davinci-003", openAIKey)
			.WithOpenAITextCompletionService(Models.Model.TextDavinciV3.EnumToString(), openAiKey)
			//.WithOpenAITextEmbeddingGenerationService("text-embedding-ada-002", openAIKey)
			.WithOpenAITextEmbeddingGenerationService(Models.Model.TextEmbeddingAdaV2.EnumToString(), openAiKey)
			.WithMemoryStorage(new VolatileMemoryStore())

			.Build();
		const string pluginManifestUrl = "http://localhost:7071/.well-known/ai-plugin.json";
		var argumentationPlugin = await kernel.ImportChatGptPluginSkillFromUrlAsync("ArgumentationPlugin", new Uri(pluginManifestUrl));





		// We're going to ask the planner to find a function to achieve this goal.

		//var goal = "The following is the transcript or description of a debate. Return a logic and argumentation analysis of the debate. The plan should like that: First, use the Tweety skill TextToLogicType to select the type of formal logic relevant to the situation. Then use TextToBeliefset to create a parsable belief set representing the logic statements. Then use BeliefStateToQuery to find a potential query to run using Tweety. Then use native method QueryLogicReasoner to run the tweety engine and get the result. Finally use semantic function LogicAnalysisToText to inject the text, belief set, query and result into an intepreter prompt that will return a human readable intepretation of the analysis.\n\n";


		//var goal = "The following is the transcript or description of a debate. Return an analysis of the debate. The plan should like that: First, use TextToLogicType then TextToBeliefset then BeliefStateToQuery then QueryLogicReasoner, finally use LogicAnalysisToText to inject the text, belief set, query and result into an intepreter prompt that will return a human readable intepretation of the analysis.\n\n";

		//var goal = "The following is the transcript or description of a debate. Return an analysis of the debate. There are 2 pipelines for analysis, Informal analysis is a 3 step process based on fallacy identification. Formal analysis is based on running queries against belief sets, in propositional, first order or modal logic. Informal analysis might help guide formal analysis. Formal analysis steps are translation to belief set, identification of relevant queries, execution of the queries against belief set and interpretation of results.\n\n";

		var goal = "The following is the transcript of a debate with fallacious arguments. Please use the Fallacy functions to do fallacy analysis, starting with the prepare function to ellicit branches of analysis, then the branch analysis function, and then the complete for a wrap up.\n\n";


		//var debateText = "Debate Context: Alice claims that if it's sunny, then Bob will go to the park. Bob says that it's indeed sunny, but he won't go to the park because he has work to do.";

		//var debateText ="Debate Context: Federalists claim that if the Constitution is adopted, then there will be a stronger federal government. They further claim that a stronger federal government will better protect the rights of the people and ensure stability. Anti-Federalists, on the other hand, claim that if the Constitution is adopted, then there will be a stronger federal government, but they believe that a stronger federal government will lead to an abuse of power and the loss of individual rights.";


		//var debateText = "Debate Context: Federalists claim that if the Constitution is adopted, then there will be a stronger federal government. They further claim that a stronger federal government will better protect the rights of the people and ensure stability. Anti-Federalists, on the other hand, claim that if the Constitution is adopted, then there will be a stronger federal government, but they fear that a stronger federal government may lead to an abuse of power and the loss of individual rights. They believe that without checks and balances, the risk of tyranny is real. Yet, the Federalists assert that adoption of the Constitution includes a commitment to checks and balances, aiming to prevent such abuse of power.";

		//var debateText = "Debate Context: In the nascent era of the United States, two significant groups, the Federalists and the Anti-Federalists, grappled with disagreements on the structure of the government.\r\n\r\nThe Federalists staunchly advocated for a robust central government. They postulated that the flourishing nation could only continue to grow and stabilize by upholding a range of fundamental conditions. Among these were: nurturing democratic traditions, ensuring the rule of law, maintaining an independent judiciary, promoting an enlightened citizenry, encouraging responsible citizenship, fostering a vibrant civil society, and preserving faith in the electoral process.\r\n\r\nWith significant emphasis, the Federalists contended that a strong central government was pivotal in coordinating comprehensive responses to national crises, including economic recessions, foreign invasions, and pandemics. They held the conviction that a unified action against these adversities would bolster national resilience and instill a sense of security within the citizenry.\r\n\r\nMoreover, they reasoned that an empowered central government, contrary to the fears of the Anti-Federalists, would be less likely to spiral into tyranny. A strong government, armed with the ability to enforce its laws uniformly across the states, they argued, would ensure that no single state could domineer over others or impose its will on the minority. Such a government would also effectively counterbalance the concentration of wealth and power.\r\n\r\nSimultaneously, the Federalists proposed that the Constitution, the blueprint of this central government, was designed to include checks and balances to prevent the misuse of power. They asserted that the Constitution was not merely an instrument for centralizing power, but was built to address the Anti-Federalist's fears about potential overreach by the central government.\r\n\r\nThe Anti-Federalists, despite recognizing the importance of the aforementioned conditions, remained apprehensive of a potent federal government. They voiced concerns that even with these safeguards in place, such a government might overstep its boundaries and infringe upon individual freedoms.\r\n\r\nHowever, both groups agreed on one crucial point: the rule of law and an independent judiciary were indispensable in checking any misuse of power. They jointly reasoned that a misuse of power would signify a compromised judiciary or a flouting of the rule of law. Conversely, they concurred that if these two conditions were steadfastly preserved, they would serve as robust bulwarks against power misuse.";

		var debateText =
			@"TRUMP: Thank you very much, Chris. I will tell you very simply. We won the election. Elections have consequences. We have the Senate, we have the White House, and we have a phenomenal nominee respected by all. Top, top academic, good in every way. Good in every way. In fact, some of her biggest endorsers are very liberal people from Notre Dame and other places. So I think she’s going to be fantastic. We have plenty of time. Even if we did it after the election itself. I have a lot of time after the election, as you know. So I think that she will be outstanding. She’s going to be as good as anybody that has served on that court. We really feel that. We have a professor at Notre Dame, highly respected by all, said she’s the single greatest student he’s ever had. He’s been a professor for a long time at a great school.

And we won the election and therefore we have the right to choose her, and very few people knowingly would say otherwise. And by the way, the Democrats, they wouldn’t even think about not doing it. The only difference is they’d try and do it faster. There’s no way they would give it up. They had Merrick Garland, but the problem is they didn’t have the election so they were stopped. And probably that would happen in reverse, also. Definitely would happen in reverse. So we won the election and we have the right to do it, Chris.

WALLACE: President Trump, thank you. Same question to you, Vice President Biden. You have two minutes.

BIDEN: Well, first of all, thank you for doing this and looking forward to this, Mr. President.

TRUMP: Thank you, Joe.

BIDEN: The American people have a right to have a say in who the Supreme Court nominee is and that say occurs when they vote for United States Senators and when they vote for the President of United States. They’re not going to get that chance now because we’re in the middle of an election already. The election has already started. Tens of thousands of people already voted and so the thing that should happen is we should wait. We should wait and see what the outcome of this election is because that’s the only way the American people get to express their view is by who they elect as President and who they elect as Vice President.

Now, what’s at stake here is the President’s made it clear, he wants to get rid of the Affordable Care Act. He’s been running on that, he ran on that and he’s been governing on that. He’s in the Supreme Court right now trying to get rid of the Affordable Care Act, which will strip 20 million people from having health insurance now, if it goes into court. And the justice, I’m not opposed to the justice, she seems like a very fine person. But she’s written, before she went in the bench, which is her right, that she thinks that the Affordable Care Act is not Constitutional. The other thing that’s on the court, and if it’s struck down, what happens? Women’s rights are fundamentally changed. Once again, a woman could pay more money because she has a pre-existing condition of pregnancy. They’re able to charge women more for the same exact procedure a man gets.

And that ended when we, in fact, passed the Affordable Care Act, and there’s a hundred million people who have pre-existing conditions and they’ll be taken away as well. Those pre-existing conditions, insurance companies are going to love this. And so it’s just not appropriate to do this before this election. If he wins the election and the Senate is Republican, then he goes forward. If not, we should wait until February.

TRUMP: There aren’t a hundred million people with pre-existing conditions. As far as a say is concerned, the people already had their say. Okay, Justice Ginsburg said very powerfully, very strongly, at some point 10 years ago or so, she said a President and the Senate is elected for a period of time, but a President is elected for four years. We’re not elected for three years. I’m not elected for three years. So we have the Senate, we have a President-

BIDEN: He’s elected to the next election.

TRUMP: During that period of time, during that period of time, we have an opening. I’m not elected for three years. I’m elected for four years. Joe, the hundred million people is totally wrong. I don’t know where you got that number. The bigger problem that you have is that you’re going to extinguish 180 million people with their private health care, that they’re very happy with.

BIDEN: That’s simply not true.

TRUMP: Well, you’re certainly going to socialist. You’re going to socialist medicine-

WALLACE: Gentlemen, we’re now into open discussion.

BIDEN: Open discussion.

WALLACE: Open discussion, yes, I agree. Go ahead, Vice President.

BIDEN: Number one, he knows what I proposed. What I proposed is that we expand Obamacare and we increase it. We do not wipe any. And one of the big debates we had with 23 of my colleagues trying to win the nomination that I won, were saying that Biden wanted to allow people to have private insurance still. They can. They do. They will under my proposal.

TRUMP: That’s not what you’ve said and it’s not what your party is saying.

BIDEN: That is simply a lie.";

		//var debateText = "Debate Context: À l'aube de l'ère des États-Unis, deux groupes importants, les Fédéralistes et les Anti-Fédéralistes, se sont affrontés sur la structure du gouvernement.\r\n\r\nLes Fédéralistes ont fermement défendu l'idée d'un gouvernement central fort. Ils ont postulé que la nation en plein essor ne pourrait continuer à croître et à se stabiliser qu'en respectant un ensemble de conditions fondamentales. Parmi celles-ci figuraient la préservation des traditions démocratiques, l'application de l'état de droit, le maintien d'un pouvoir judiciaire indépendant, la promotion d'une citoyenneté éclairée, l'encouragement à une citoyenneté responsable, le soutien à une société civile dynamique et la préservation de la confiance dans le processus électoral.\r\n\r\nDe plus, les Fédéralistes ont soutenu que la Constitution, en tant que document fondateur, était conçue pour inclure des mécanismes de contrôle visant à prévenir les abus de pouvoir. Ils ont affirmé que la Constitution n'était pas simplement un instrument de centralisation du pouvoir, mais qu'elle était spécifiquement élaborée pour répondre aux préoccupations des Anti-Fédéralistes concernant un éventuel dépassement des limites par le gouvernement central.\r\n\r\nLes Anti-Fédéralistes, tout en reconnaissant l'importance des conditions énoncées, ont exprimé des préoccupations quant à un gouvernement central fort. Ils ont souligné que même avec ces mesures de sauvegarde en place, il y avait toujours un risque que le gouvernement central empiète sur les libertés individuelles et abuse de son pouvoir.\r\n\r\nCependant, les Fédéralistes et les Anti-Fédéralistes étaient d'accord sur un point crucial : le respect de l'état de droit et d'un pouvoir judiciaire indépendant était essentiel pour prévenir tout abus de pouvoir. Ils étaient d'accord sur le fait qu'un abus de pouvoir signifierait une compromission du pouvoir judiciaire ou une violation de l'état de droit. Inversement, ils étaient d'accord que si ces deux conditions étaient fermement respectées, elles serviraient de solides remparts contre tout abus de pouvoir.";

		goal = goal + debateText;

		// The planner returns a plan, consisting of a single function
		// to execute and achieve the goal requested.
		//var plan = planner.CreatePlanAsync(goal).GetAwaiter().GetResult();

		var plan = GetFallacyPlan(goal, argumentationPlugin, debateText, argumentationPlugin);


		// Create a stepwise planner and invoke it
		//var planner = new StepwisePlanner(kernel, new StepwisePlannerConfig() { MaxIterations = 30, MaxRelevantFunctions = 10, MaxTokens = 700 });
		//////var question = "I have $2130.23. How much would I have after it grew by 24% and after I spent $5 on a latte?";
		//var plan = planner.CreatePlan(goal);



		Console.WriteLine("Original plan:\n");
		Console.WriteLine(JsonSerializer.Serialize(plan, new JsonSerializerOptions { WriteIndented = true }));

		//var startPrompt =
		//	"Based on the provided text and logic applied, the query {{$query}} has been accepted by the belief set. This suggests that within the constraints and rules of the belief set, the situation described by the query can indeed occur. Please note that the interpretation needs to be in line with the original text and logic applied, but in the context of the natural language text, this could mean \n";




		var result = await plan.InvokeAsync((SKContext)kernel.CreateNewContext());

		// Print the results
		Console.WriteLine("Result: " + result);

		// Print details about the plan
		if (result.Variables.TryGetValue("stepCount", out string? stepCount))
		{
			Console.WriteLine("Steps Taken: " + stepCount);
		}
		if (result.Variables.TryGetValue("skillCount", out string? skillCount))
		{
			Console.WriteLine("Skills Used: " + skillCount);
		}

		// Show the result, which should match the given goal
		//Console.WriteLine($"Result: {startPrompt}{result.Result}\n");
		Console.WriteLine($"Result: {result.Result}\n");
		//Console.WriteLine($"Context: {JsonConvert.SerializeObject(result.Variables)}\n");
		foreach (var contextVariable in result.Variables)
		{
			if (contextVariable.Key.ToUpperInvariant() != "INPUT")
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
		planStep.Parameters.Set("payload",
			debateText);
		planStep.Outputs.Add("belief_set");
		plan.AddSteps(planStep);

		planStep = new Plan(tweetySemanticSkill["BeliefStateToQueriesPl"]);
		planStep.Parameters.Set("input",
			debateText);
		planStep.Parameters.Set("belief_set", "$belief_set");
		planStep.Parameters.Set("payload",
			"$input");
		planStep.Outputs.Add("queries");
		plan.AddSteps(planStep);

		planStep = new Plan(tweetyNativeSkill["RunPropositionalQueries"]);
		planStep.Parameters.Set("input", "$belief_set");
		planStep.Parameters.Set("payload",
			"$input");
		planStep.Parameters.Set("belief_set", "$belief_set");
		planStep.Parameters.Set("queries", "queries");
		planStep.Outputs.Add("tweety_result");
		plan.AddSteps(planStep);

		planStep = new Plan(tweetySemanticSkill["LogicAnalysisToText"]);
		planStep.Parameters.Set("input", debateText);
		planStep.Parameters.Set("payload",
			"$input");
		planStep.Parameters.Set("logicType", "Propositional");
		planStep.Parameters.Set("belief_set", "$belief_set");
		planStep.Parameters.Set("queries", "queries");
		planStep.Parameters.Set("tweety_result", "$tweety_result");
		planStep.Outputs.Add("human interpretation");
		plan.AddSteps(planStep);
		return plan;
	}


	private static Plan GetFallacyPlan(string goal, IDictionary<string, ISKFunction> tweetySemanticSkill, string debateText,
		IDictionary<string, ISKFunction> tweetyNativeSkill)
	{
		var plan = new Plan(goal);
		var planStep = new Plan(tweetySemanticSkill["FallacyAnalysisPrepare"]);
		planStep.Parameters.Set("input",
			debateText);
		planStep.Parameters.Set("payload",
			debateText);
		planStep.Outputs.Add("fallacyBranches");
		plan.AddSteps(planStep);

		planStep = new Plan(tweetySemanticSkill["AnalyzeTextFallaciesInBranch"]);
		planStep.Parameters.Set("input",
			debateText);
		planStep.Parameters.Set("payload", "$input");
		planStep.Parameters.Set("baseFallacyPath", "");
		planStep.Outputs.Add("BranchAnalysis1");
		plan.AddSteps(planStep);

		planStep = new Plan(tweetyNativeSkill["FallacyAnalysisComplete"]);
		planStep.Parameters.Set("input", "$belief_set");
		planStep.Parameters.Set("payload",
			"$input");
		planStep.Parameters.Set("belief_set", "$belief_set");
		planStep.Parameters.Set("queries", "queries");
		planStep.Outputs.Add("fallacyAnalysis");
		plan.AddSteps(planStep);

		
		return plan;
	}








	//private async Task IndexTaxonomyWithSemanticKernel(bool useDebugPath, string openAIKey)
	//{

	//	var chunks = await SourceDataset.SplitContentIntoJsonChunks(ChunkSize, FieldsToInclude, useDebugPath);

	//	var kernel = Kernel.Builder
	//		.WithLogger(ConsoleLogger.Log)
	//		//.WithOpenAITextCompletionService("text-davinci-003", openAIKey)
	//		.WithOpenAITextCompletionService(Models.Model.TextDavinciV3.EnumToString(), openAIKey)
	//		//.WithOpenAITextEmbeddingGenerationService("text-embedding-ada-002", openAIKey)
	//		.WithOpenAITextEmbeddingGenerationService(Models.Model.TextEmbeddingAdaV2.EnumToString(), openAIKey)
	//		.WithMemoryStorage(new VolatileMemoryStore())

	//		.Build();

	//	AddSkills(kernel);

	//}

	//private void AddSkills(IKernel kernel)
	//{
	//	kernel.ImportSkill(new MathSkill(), "math");
	//	kernel.ImportSkill(new HttpSkill(), "http");
	//	kernel.ImportSkill(new FileIOSkill(), "fileIO");
	//	kernel.ImportSkill(new TextSkill(), "text");
	//	kernel.ImportSkill(new TimeSkill(), "time");
	//	kernel.ImportSkill(new WaitSkill(), "wait");
	//	var parentDirectory = @"E:\Dev\AI\Libs\Argumentum\Generation\Converters\Argumentum.AssetConverter\DatasetUpdater\Resources\Skills\Core";
	//	kernel.ImportSemanticSkillFromDirectory(parentDirectory);


	//}


	private async Task UpdateWithChatGPT(bool useDebugPath, string openAIKey)
	{
		try
		{
			// load dataset in chunks, 
			var chunks = await SourceDataset.SplitContentIntoJsonChunks(ChunkSize, FieldsToInclude, useDebugPath);

			//Doing short tests
			if (SkipChunkNb > 0)
			{
				chunks = chunks.Skip(SkipChunkNb).ToList();
			}

			if (TakeChunkNb > 0)
			{
				chunks = chunks.Take(TakeChunkNb).ToList();
			}

			var csvAnswers = new ConcurrentBag<string>();


			var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelismWebService };
			await Parallel.ForEachAsync(chunks, parallelOptions, async (chunk, token) =>
			{
				SystemPrompt = Resource1.JsonPromptSystem;
				var dataPrompt = new Prompt()
				{
					ApiKey = openAIKey,
					SystemPrompt = SystemPrompt,
					Example = new PromptExample()
					{
						UserPrompt = UserPrompt,
						Answer = AssistantPrompt
					},
					UserPrompt = chunk
				};

				string result = "";

				for (int i = 0; i < NbMessageCalls; i++)
				{
					Logger.Log($"Calling ChatGPT API with chunk: \n{Markup.Escape(chunk)}\n");

					try
					{
						result = await dataPrompt.Send();
						//result = chunk;
						Logger.Log($"ChatGPT answered \n{Markup.Escape(chunk)}\n with chunk \n{Markup.Escape(result)}\n");
						dataPrompt.UserPrompt = result;
					}
					catch (Exception e)
					{
						Logger.LogException(e);
					}
				}


				csvAnswers.Add(result);
			});


			// Merge answers into one csv
			var mergedCsv =
				await SourceDataset.MergeJsonResponsesIntoCsv(csvAnswers.ToList(), PrimaryField, FieldsToUpdate, ",",
					false);


			// Save mergedCsv to file

			if (File.Exists(TargetPath))
			{
				try
				{
					File.Delete(TargetPath);
				}
				catch (Exception e)
				{
					Logger.LogException(e);
					Console.ReadKey();
				}
			}

			if (!Directory.Exists(Path.GetDirectoryName(TargetPath)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(TargetPath));
			}

			await SourceDataset.SaveContent(TargetPath, mergedCsv);


			Logger.Log("Completed ChatGPT calls and saved the output to " + TargetPath);
		}
		catch (Exception ex)
		{
			Logger.LogException(ex);
		}
	}
}

public enum SemanticMode
{
	UpdateWithChatpGptAPI,
	IndexWithSemanticKernel,
	RunChGPTPluginAnalysis
}






/// <summary>
/// Basic logger printing to console
/// </summary>
internal static class ConsoleLogger
{
	internal static ILogger Log => LogFactory.CreateLogger<object>();

	private static ILoggerFactory LogFactory => s_loggerFactory.Value;

	private static readonly Lazy<ILoggerFactory> s_loggerFactory = new(LogBuilder);

	private static ILoggerFactory LogBuilder()
	{
		return LoggerFactory.Create(builder =>
		{
			builder.SetMinimumLevel(LogLevel.Warning);

			// builder.AddFilter("Microsoft", LogLevel.Trace);
			// builder.AddFilter("Microsoft", LogLevel.Debug);
			// builder.AddFilter("Microsoft", LogLevel.Information);
			// builder.AddFilter("Microsoft", LogLevel.Warning);
			// builder.AddFilter("Microsoft", LogLevel.Error);

			builder.AddFilter("Microsoft", LogLevel.Warning);
			builder.AddFilter("System", LogLevel.Warning);

			builder.AddConsole();
		});
	}
}